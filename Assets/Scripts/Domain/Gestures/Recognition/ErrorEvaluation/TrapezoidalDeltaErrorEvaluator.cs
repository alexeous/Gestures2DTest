using MathUtils.Curves;

namespace Domain.Gestures.Recognition.ErrorEvaluation;

public class TrapezoidalDeltaErrorEvaluator : IDeltaErrorEvaluator
{
    private readonly AnimationCurve _idealDistanceZone;

    public TrapezoidalDeltaErrorEvaluator(AnimationCurve idealDistanceZone)
    {
        _idealDistanceZone = idealDistanceZone;
    }

    public float Evaluate(Gesture gesture, ManipulationDelta manipulationDelta)
    {
        var path = gesture.Path;

        var previousT = manipulationDelta.PreviousState.TracedDistance / path.Length;
        var previousMomentaryError = CalculateMomentaryError(path, previousT, manipulationDelta.PreviousState.Position);

        var nextT = manipulationDelta.NextState.TracedDistance / path.Length;
        var nextMomentaryError = CalculateMomentaryError(path, nextT, manipulationDelta.NextState.Position);

        return (previousMomentaryError + nextMomentaryError) / 2;
    }

    private float CalculateMomentaryError(IUniformlyParameterizedCurve path, float t, Vector2 manipulationPosition)
    {
        var distanceFromInputPosToExpectedPointAtPath = Vector2.Distance(path.GetPosition(t), manipulationPosition);
        var momentaryError = Mathf.Max(0, distanceFromInputPosToExpectedPointAtPath - _idealDistanceZone.Evaluate(t));

        return momentaryError;
    }
}