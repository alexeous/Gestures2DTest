namespace Domain.Gestures.Recognition.ErrorEvaluation;

public interface IFinalErrorEvaluator
{
    float Evaluate(Gesture gesture, float accumulatedDeltaError, ManipulationState finalManipulationState);
}