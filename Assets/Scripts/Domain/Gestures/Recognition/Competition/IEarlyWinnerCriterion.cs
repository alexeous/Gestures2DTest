using Domain.Manipulations;

namespace Domain.Gestures.Recognition.Competition;

public interface IEarlyWinnerCriterion
{
    bool IsLeadingCompetitorAnEarlyWinner(IReadOnlyList<GesturePerformingAssessment> sortedCompetitors, in ManipulationState manipulationState);
}