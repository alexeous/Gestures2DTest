namespace Domain.Manipulations;

public class Manipulation
{
    public ManipulationState State { get; private set; }

    public Manipulation(Vector2 initialInputPosition)
    {
        State = new ManipulationState(initialInputPosition, 0);
    }

    public ManipulationDelta Advance(Vector2 currentInputPosition)
    {
        var previousState = State;

        var deltaDistance = Vector2.Distance(previousState.Position, currentInputPosition);
        var newTracedDistance = previousState.TracedDistance + deltaDistance;

        State = new ManipulationState(currentInputPosition, newTracedDistance);

        return new ManipulationDelta(previousState, State);
    }
}