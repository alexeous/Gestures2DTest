namespace Domain.Gestures.Recognition;

public readonly struct ManipulationState
{
    public readonly Vector2 Position;
    public readonly float TracedDistance;

    public ManipulationState(Vector2 position, float tracedDistance)
    {
        Position = position;
        TracedDistance = tracedDistance;
    }
}