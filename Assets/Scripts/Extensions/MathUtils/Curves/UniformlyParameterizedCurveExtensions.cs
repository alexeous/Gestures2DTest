using MathUtils.Curves;

namespace Extensions.MathUtils.Curves;

public static class UniformlyParameterizedCurveExtensions
{
    public static Vector2 GetPositionAtDistance(this IUniformlyParameterizedCurve curve, float distance)
    {
        var t = distance / curve.Length;

        return curve.GetPosition(t);
    }
}