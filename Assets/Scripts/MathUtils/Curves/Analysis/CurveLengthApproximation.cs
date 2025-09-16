using MathUtils.Integration;

namespace MathUtils.Curves.Analysis;

/// <summary>
/// Модель для приближённого вычисления длин частей кривой (и её общей длины в частности).
/// </summary>
public class CurveLengthApproximation
{
    private readonly float[] _lengthTable;
    private readonly int _steps;

    public readonly float TotalLength;

    private CurveLengthApproximation(float[] lengthTable)
    {
        _lengthTable = lengthTable;
        _steps = _lengthTable.Length - 1;

        TotalLength = _lengthTable[^1];
    }

    public float GetLengthFrom0To(float t)
    {
        t = Mathf.Clamp01(t);

        var sectionIdx = Mathf.FloorToInt(t * _steps);
        var sectionRange = GetSectionRange(sectionIdx, _steps);
        var lengthRange = GetLengthRange(sectionIdx);

        return lengthRange.Lerp(sectionRange.InverseLerp(t));
    }

    public float GetLength(FloatRange range)
    {
        var from = Mathf.Clamp01(range.From);
        var to = Mathf.Clamp01(range.To);

        return GetLengthFrom0To(to) - GetLengthFrom0To(from);
    }

    /// <summary>
    /// Ищет параметр t, соответствующий точке на кривой, расстояние от которой до её начала равно <paramref name="lengthFrom0"/>.
    /// </summary>
    public float SeekT(float lengthFrom0)
    {
        if (lengthFrom0 <= 0)
            return 0;

        if (lengthFrom0 >= TotalLength)
            return 1;

        var sectionIdx = Array.BinarySearch(_lengthTable, lengthFrom0);
        if (sectionIdx < 0)
            sectionIdx = ~sectionIdx - 1;

        var lengthRange = GetLengthRange(sectionIdx);
        var sectionRange = GetSectionRange(sectionIdx, _steps);

        return sectionRange.Lerp(lengthRange.InverseLerp(lengthFrom0));
    }

    /// <summary>
    /// Создаёт модель длин кривой.
    /// </summary>
    /// <param name="curve">Кривая.</param>
    /// <param name="steps">Число разбиений кривой. Чем больше, тем точнее приближение.</param>
    public static CurveLengthApproximation Build(ICurve curve, int steps = 500)
    {
        Assert(curve != null, "Curve must not be null");
        Assert(steps > 0, "There must be at least one step");

        var lengthTable = new float[steps + 1];
        lengthTable[0] = 0;

        var integrand = GetCurveLengthCalculationIntegrand(curve);
        var lengthSoFar = 0f;
        for (var sectionIdx = 0; sectionIdx < steps; sectionIdx++)
        {
            var sectionRange = GetSectionRange(sectionIdx, steps);
            var curvePieceLength = SimpsonIntegral.Calculate(integrand, sectionRange, steps: 5);
            lengthSoFar += curvePieceLength;

            lengthTable[sectionIdx + 1] = lengthSoFar;
        }

        return new CurveLengthApproximation(lengthTable);
    }

    private static FloatRange GetSectionRange(int sectionIdx, int steps)
    {
        return new FloatRange(
            (float)sectionIdx / steps,
            (float)(sectionIdx + 1) / steps);
    }

    private FloatRange GetLengthRange(int sectionIdx)
    {
        return new FloatRange(
            _lengthTable[sectionIdx],
            _lengthTable[sectionIdx + 1]);
    }

    // https://en.wikipedia.org/wiki/Arc_length
    private static Func<float, float> GetCurveLengthCalculationIntegrand(ICurve curve)
    {
        return t => curve.GetDerivative(t).magnitude;
    }
}