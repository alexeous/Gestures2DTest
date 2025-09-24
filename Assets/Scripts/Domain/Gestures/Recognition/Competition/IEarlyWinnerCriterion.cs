namespace Domain.Gestures.Recognition.Competition;

public interface IEarlyWinnerCriterion
{
    bool IsLeadingCompetitorAnEarlyWinner(IReadOnlyList<GesturePerformingAssessment> sortedCompetitors, ManipulationState manipulationState);
}