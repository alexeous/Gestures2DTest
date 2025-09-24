namespace Domain.Gestures.Recognition.ErrorEvaluation;

public interface IDeltaErrorEvaluator
{
    float Evaluate(Gesture gesture, ManipulationDelta manipulationDelta);
}