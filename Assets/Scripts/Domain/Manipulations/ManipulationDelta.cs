namespace Domain.Manipulations;

public struct ManipulationDelta
{
    public readonly ManipulationState PreviousState;
    public readonly ManipulationState NextState;

    public ManipulationDelta(ManipulationState previousState, ManipulationState nextState)
    {
        PreviousState = previousState;
        NextState = nextState;
    }
}