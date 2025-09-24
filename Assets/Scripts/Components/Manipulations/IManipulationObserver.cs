using Domain.Manipulations;

namespace Components.Manipulations;

public interface IManipulationObserver
{
    void OnManipulationBegun(in ManipulationState initialManipulationState);
    void OnManipulationAdvanced(in ManipulationDelta manipulationDelta);
    void OnManipulationFinished(in ManipulationState finalManipulationState);
}