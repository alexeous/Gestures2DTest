using Domain.Manipulations;

namespace Domain.Gestures.Recognition.Competition;

public class FractionOfFollowingCompetitorErrorCriterion : IEarlyWinnerCriterion
{
    public float MinimumTracedDistance { get; }
    public float FractionThreshold { get; }

    public FractionOfFollowingCompetitorErrorCriterion(float minimumTracedDistance, float fractionThreshold = 0.5f)
    {
        Assert(minimumTracedDistance > 0, "Minimum traced distance must be greater than 0");
        Assert(fractionThreshold is >= 0 and <= 1, "Fraction threshold must be between 0 and 1 inclusive");

        MinimumTracedDistance = minimumTracedDistance;
        FractionThreshold = fractionThreshold;
    }

    public bool IsLeadingCompetitorAnEarlyWinner(IReadOnlyList<GesturePerformingAssessment> sortedCompetitors, in ManipulationState manipulationState)
    {
        Assert(sortedCompetitors.Count > 0, "There must be at least one competitor");

        if (sortedCompetitors.Count == 1)
            return true;

        return manipulationState.TracedDistance > MinimumTracedDistance &&
               sortedCompetitors[0].Error < sortedCompetitors[1].Error * FractionThreshold;
    }
}