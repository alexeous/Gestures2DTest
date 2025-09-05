namespace MathUtils.Curves;

public class BezierCurve : ICurve
{
    public readonly Vector2 P0;
    public readonly Vector2 P1;
    public readonly Vector2 P2;

    public BezierCurve(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        P0 = p0;
        P1 = p1;
        P2 = p2;
    }

    public Vector2 GetPosition(float t)
    {
        t = Mathf.Clamp01(t);
        return t * ((P1 - P0) * (2 - t) + t * (P2 - P1)) + P0;
    }

    public Vector2 GetDerivative(float t)
    {
        t = Mathf.Clamp01(t);
        return 2 * (t * (P2 - 2 * P1 + P0) + P1 - P0);
    }

    public Vector2 GetDerivative2(float t)
    {
        return 2 * (P2 - 2 * P1 + P0);
    }
}