namespace Domain.Gestures.Recognition;

public class Manipulation
{
    private readonly Vector2 _initialInputPosition;

    public ManipulationState State { get; private set; }

    public Manipulation(Vector2 initialInputPosition)
    {
        _initialInputPosition = initialInputPosition;
    }

    public ManipulationDelta Advance(Vector2 currentInputPosition)
    {
        var previousState = State;

        var currentLocalPosition = currentInputPosition - _initialInputPosition;

        var deltaDistance = Vector2.Distance(previousState.Position, currentLocalPosition);
        var newTracedDistance = previousState.TracedDistance + deltaDistance;

        State = new ManipulationState(currentLocalPosition, newTracedDistance);

        return new ManipulationDelta(previousState, State);
    }
}