namespace MathUtils.Curves;

public static class UniformlyParameterizedCurveFactory
{
    public static IUniformlyParameterizedCurve CreateFrom(ICurve curve, int sectionCount = 1000)
    {
        Assert(curve != null, "Curve must not be null");

        if (curve is IUniformlyParameterizedCurve uniformlyParameterizedCurve)
            return uniformlyParameterizedCurve;

        return new PiecewiseLinearlyUniformedCurve(curve, sectionCount);
    }
}