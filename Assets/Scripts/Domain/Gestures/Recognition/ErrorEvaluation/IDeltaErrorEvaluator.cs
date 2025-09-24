using Domain.Manipulations;

namespace Domain.Gestures.Recognition.ErrorEvaluation;

public interface IDeltaErrorEvaluator
{
    float Evaluate(Gesture gesture, in ManipulationDelta manipulationDelta);
}