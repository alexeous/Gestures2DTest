namespace MathUtils.Curves;

/// <summary>
/// Кривая Безье (квадратичная) с опорными точками <see cref="P0"/>, <see cref="P2"/> и <see cref="P2"/>, причём
/// <see cref="P0"/> является началом кривой, а <see cref="P2"/> — её концом.
/// <seealso href="https://ru.wikipedia.org/wiki/%D0%9A%D1%80%D0%B8%D0%B2%D0%B0%D1%8F_%D0%91%D0%B5%D0%B7%D1%8C%D0%B5#%D0%9A%D0%B2%D0%B0%D0%B4%D1%80%D0%B0%D1%82%D0%B8%D1%87%D0%BD%D1%8B%D0%B5_%D0%BA%D1%80%D0%B8%D0%B2%D1%8B%D0%B5"/>
/// </summary>
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