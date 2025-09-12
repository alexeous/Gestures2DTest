using Extensions;
using MathUtils.Curves.Analysis;

namespace MathUtils.Curves;

public class UniformedCurve : ICurve
{
    private readonly ICurve _innerCurve;
    private readonly float[] _remappingTable;
    private readonly int _remappingSectionCount;
    private readonly float _remappingStep;

    public UniformedCurve(ICurve innerCurve, float[] remappingTable)
    {
        Assert(innerCurve != null, "Curve must not be null");

        Assert(remappingTable != null, "Remapping table must not be null");
        Assert(remappingTable.Length >= 2, $"Remapping table must have at least 2 elements, but got {remappingTable.Length}");
        Assert(remappingTable[0].IsApproximately(0), $"First value in remapping table must be 0, but got {remappingTable[0]}");
        Assert(remappingTable[^1].IsApproximately(1), $"Last value in remapping table must be 1, but got {remappingTable[^1]}");

        _innerCurve = innerCurve;
        _remappingTable = remappingTable;

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

        // since the purpose of this class is to make "motion" along the given curve uniform, just normalizing the vector is legitimate and reasonable
        return _innerCurve.GetDerivative(remappedT).normalized;
    }

    public Vector2 GetDerivative2(float t)
    {
        var remappedT = RemapT(t);

        var derivative = _innerCurve.GetDerivative(remappedT);
        var derivative2 = _innerCurve.GetDerivative2(remappedT);

        var derivativeMagnitude = derivative.magnitude;
        var derivativeNormalized = derivative / derivativeMagnitude;
        var derivative2Normalized = derivative2 / derivativeMagnitude;

        return derivative2Normalized - Vector2.Dot(derivativeNormalized, derivative2Normalized) * derivativeNormalized;
    }

    public static UniformedCurve CreateFrom(ICurve curve, int sectionCount = 500)
    {
        Assert(sectionCount > 0, "Uniformed curve section count must be > 0");

        var curveLengthApproximation = CurveLengthApproximation.Build(curve, steps: sectionCount);

        var remappingTable = new float[sectionCount + 1];
        remappingTable[0] = 0;

        for (var sectionIdx = 0; sectionIdx < sectionCount; sectionIdx++)
        {
            var curveLengthSoFar = (float)(sectionIdx + 1) / sectionCount * curveLengthApproximation.TotalLength;
            var t = curveLengthApproximation.SeekT(curveLengthSoFar);

            remappingTable[sectionIdx + 1] =  t;
        }

        return new UniformedCurve(curve, remappingTable);
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
}