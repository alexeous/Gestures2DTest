using Domain.Manipulations;

namespace Domain.Gestures.Recognition.ErrorEvaluation;

public class RemainingDistanceFinalErrorEvaluator : IFinalErrorEvaluator
{
    private readonly float _idealFinishRadius;

    public RemainingDistanceFinalErrorEvaluator(float idealFinishRadius)
    {
        Assert(idealFinishRadius >= 0, "Ideal finish radius must not be negative");

        _idealFinishRadius = idealFinishRadius;
    }

    public float Evaluate(Gesture gesture, float accumulatedDeltaError, in ManipulationState finalManipulationState)
    {
        var tracedDistanceMismatch = Mathf.Abs(gesture.Path.Length - finalManipulationState.TracedDistance);
        var finalError = accumulatedDeltaError + Mathf.Max(0, tracedDistanceMismatch - _idealFinishRadius);

        return finalError;
    }
}