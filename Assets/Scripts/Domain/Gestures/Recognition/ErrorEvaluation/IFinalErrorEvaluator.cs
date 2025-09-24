using Domain.Manipulations;

namespace Domain.Gestures.Recognition.ErrorEvaluation;

public interface IFinalErrorEvaluator
{
    float Evaluate(Gesture gesture, float accumulatedDeltaError, in ManipulationState finalManipulationState);
}