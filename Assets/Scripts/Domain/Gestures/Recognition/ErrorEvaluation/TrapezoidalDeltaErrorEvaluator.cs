using Domain.Manipulations;
using MathUtils.Curves;

namespace Domain.Gestures.Recognition.ErrorEvaluation;

public class TrapezoidalDeltaErrorEvaluator : IDeltaErrorEvaluator
{
    private readonly AnimationCurve _idealDistanceZone;

    public TrapezoidalDeltaErrorEvaluator(AnimationCurve idealDistanceZone)
    {
        _idealDistanceZone = idealDistanceZone;
    }

    public float Evaluate(Gesture gesture, in ManipulationDelta manipulationDelta)
    {
        var previousMomentaryError = CalculateMomentaryError(gesture.Path, manipulationDelta.PreviousState);
        var nextMomentaryError = CalculateMomentaryError(gesture.Path, manipulationDelta.NextState);

        return (previousMomentaryError + nextMomentaryError) / 2;
    }

    private float CalculateMomentaryError(IUniformlyParameterizedCurve path, in ManipulationState manipulationState)
    {
        var t = manipulationState.TracedDistance / path.Length;
        var expectedPointAtPath = path.GetPosition(t);
        var distanceFromManipPosToExpectedPointAtPath = Vector2.Distance(expectedPointAtPath, manipulationState.Position);
        var momentaryError = Mathf.Max(0, distanceFromManipPosToExpectedPointAtPath - _idealDistanceZone.Evaluate(t));

        return momentaryError;
    }
}