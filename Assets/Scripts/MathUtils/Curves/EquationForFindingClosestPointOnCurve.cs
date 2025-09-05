namespace MathUtils.Curves;

public readonly struct EquationForFindingClosestPointOnCurve
{
    public readonly ICurve Curve;
    public readonly Vector2 Point;

    public EquationForFindingClosestPointOnCurve(ICurve curve, Vector2 point)
    {
        Assert(curve != null);

        Curve = curve;
        Point = point;
    }

    public float Function(float t)
    {
        var derivative = Curve.GetDerivative(t);
        var pointToCurve = Curve.GetPosition(t) - Point;

        return Vector3.Dot(derivative, pointToCurve);
    }

    public float FunctionDerivative(float t)
    {
        var derivative = Curve.GetDerivative(t);
        var derivative2 = Curve.GetDerivative2(t);
        var pointToCurve = Curve.GetPosition(t) - Point;

        return Vector3.Dot(derivative2, pointToCurve) + Vector2.Dot(derivative, derivative);
    }
}