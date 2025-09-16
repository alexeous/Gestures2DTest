namespace MathUtils.Curves;

/// <summary>
/// Отрезок, ограниченный точками <see cref="Start"/> и <see cref="End"/>.
/// </summary>
public class LineSegment : IUniformlyParameterizedCurve
{
    public readonly Vector2 Start;
    public readonly Vector2 End;

    public float Length { get; }

    public LineSegment(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;

        Length = Vector2.Distance(start, end);
    }

    public Vector2 GetPosition(float t)
    {
        return Vector2.Lerp(Start, End, t);
    }

    public Vector2 GetDerivative(float t)
    {
        return End - Start;
    }

    public Vector2 GetDerivative2(float t)
    {
        return Vector2.zero;
    }
}