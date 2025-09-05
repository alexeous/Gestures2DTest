namespace Data.Curves;

[Serializable]
public class SmoothCurveCornerData
{
    public Vector2 Position = Vector2.up;
    [Range(0, 1)] public float InBendingPointT = 0.5f;
    [Range(0, 1)] public float OutBendingPointT = 0.5f;

    public Vector2 GetInBendingPosition(Vector2 prevPosition)
    {
        return Vector2.Lerp(prevPosition, Position, InBendingPointT);
    }

    public Vector2 GetOutBendingPosition(Vector2 nextPosition)
    {
        return Vector2.Lerp(Position, nextPosition, OutBendingPointT);
    }
}