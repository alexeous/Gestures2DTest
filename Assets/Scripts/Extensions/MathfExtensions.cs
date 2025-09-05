namespace Extensions;

public static class MathfExtensions
{
    public static bool Approximately(Vector2 a, Vector2 b)
    {
        return Mathf.Approximately(a.x, b.x) &&
               Mathf.Approximately(a.y, b.y);
    }
}