using MathUtils.Curves.Analysis;

namespace MathUtils.Curves;

/// <summary>
/// Равномерно параметризованная кривая (строго говоря, приближение таковой), полученная из другой
/// путём замены параметра с помощью специально построенной кусочно-линейной функции.
/// <seealso href="http://dfgm.math.msu.su/files/ivanov-tuzhilin/2017-2018/lecture6.pdf"/>
/// </summary>
public class PiecewiseLinearlyUniformedCurve : IUniformlyParameterizedCurve
{
    private readonly ICurve _innerCurve;
    private readonly float[] _remappingTable;
    private readonly int _remappingSectionCount;
    private readonly float _remappingStep;

    public float Length { get; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="curve">Исходная кривая.</param>
    /// <param name="sectionCount">
    ///     Количество кусков, из которых будет состоять кусочно-линейная функция, используемая для замены параметра.
    ///     Чем больше, тем лучше приближение к идеальной равномерности.
    /// </param>
    public PiecewiseLinearlyUniformedCurve(ICurve curve, int sectionCount)
    {
        Assert(curve != null, "Curve must not be null");
        Assert(sectionCount > 0, "Uniformed curve section count must be > 0");

        var lengthApproximation = CurveLengthApproximation.Build(curve, steps: sectionCount); // NOTE: вообще steps не обязан равняться sectionCount

        Assert(lengthApproximation.TotalLength > 0, "Curve length must be positive");

        _innerCurve = curve;
        _remappingTable = BuildRemappingTable(sectionCount, lengthApproximation);
        Length = lengthApproximation.TotalLength;

        _remappingSectionCount = _remappingTable.Length - 1;
        _remappingStep = 1f / _remappingSectionCount;
    }

    public Vector2 GetPosition(float t)
    {
        var remappedT = RemapT(t);

        return _innerCurve.GetPosition(remappedT);
    }

    public Vector2 GetDerivative(float t)
    {
        var remappedT = RemapT(t);

        // since the purpose of this class is to make "motion" along the given curve uniform, this is legitimate and reasonable
        return _innerCurve.GetDerivative(remappedT).normalized * Length;
    }

    public Vector2 GetDerivative2(float t)
    {
        var remappedT = RemapT(t);

        var derivative = _innerCurve.GetDerivative(remappedT);
        var derivative2 = _innerCurve.GetDerivative2(remappedT);

        var derivativeMagnitude = derivative.magnitude * Length;
        var derivativeNormalized = derivative / derivativeMagnitude;
        var derivative2Normalized = derivative2 / derivativeMagnitude;

        return derivative2Normalized - Vector2.Dot(derivativeNormalized, derivative2Normalized) * derivativeNormalized;
    }

    private float RemapT(float t)
    {
        if (t <= 0)
            return 0;

        if (t >= 1)
            return 1;

        var sectionIdx = Mathf.FloorToInt(t * _remappingSectionCount);

        var sectionRange = GetSectionRange(sectionIdx);
        var remappedTRange = GetRemappedTRange(sectionIdx);
        var remappedT = remappedTRange.Lerp(sectionRange.InverseLerp(t));

        return remappedT;
    }

    private FloatRange GetSectionRange(int sectionIdx)
    {
        return new FloatRange(
            sectionIdx * _remappingStep,
            (sectionIdx + 1) * _remappingStep);
    }

    private FloatRange GetRemappedTRange(int sectionIdx)
    {
        return new FloatRange(
            _remappingTable[sectionIdx],
            _remappingTable[sectionIdx + 1]);
    }

    private static float[] BuildRemappingTable(int sectionCount, CurveLengthApproximation lengthApproximation)
    {
        var remappingTable = new float[sectionCount + 1];
        remappingTable[0] = 0;

        for (var sectionIdx = 0; sectionIdx < sectionCount; sectionIdx++)
        {
            var curveLengthSoFar = (float)(sectionIdx + 1) / sectionCount * lengthApproximation.TotalLength;
            var t = lengthApproximation.SeekT(lengthFrom0: curveLengthSoFar);

            remappingTable[sectionIdx + 1] = t;
        }

        return remappingTable;
    }
}