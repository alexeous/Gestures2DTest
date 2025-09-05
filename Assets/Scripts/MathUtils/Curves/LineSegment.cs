namespace MathUtils.Curves;

public class LineSegment : ICurve
{
    public readonly Vector2 Start;
    public readonly Vector2 End;

    public LineSegment(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
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