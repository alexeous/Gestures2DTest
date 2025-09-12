using MathUtils.Equations;

namespace MathUtils.Curves;

public readonly struct EquationForFindingPerpendicularsToCurve : IDifferentiableEquation
{
    public readonly ICurve Curve;
    public readonly Vector2 Point;

    public EquationForFindingPerpendicularsToCurve(ICurve curve, Vector2 point)
    {
        Assert(curve != null);

        Curve = curve;
        Point = point;
    }

    public float Function(float t)
    {
        var derivative = Curve.GetDerivative(t);
        var pointToCurve = Curve.GetPosition(t) - Point;

        return Vector3.Dot(derivative.normalized, pointToCurve);
    }

    public float FunctionDerivative(float t)
    {
        var derivative = Curve.GetDerivative(t);
        var derivative2 = Curve.GetDerivative2(t);
        var pointToCurve = Curve.GetPosition(t) - Point;

        var derivativeNormalized = derivative.normalized;
        var derivative2Normalized = derivative2.normalized;

        var derivativeNormalizedDerivative = derivative2Normalized - Vector2.Dot(derivativeNormalized, derivative2Normalized) * derivativeNormalized;

        return Vector2.Dot(derivativeNormalizedDerivative, pointToCurve) + Vector2.Dot(derivativeNormalized, derivative);
    }
}