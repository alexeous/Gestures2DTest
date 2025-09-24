using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Domain.Manipulations;
using JetBrains.Annotations;

namespace Domain.Gestures.Recognition.Competition;

public class GesturePerformingCompetition
{
    private static readonly Comparison<GesturePerformingAssessment> CompetitorsComparison =
        (a, b) => a.Error.CompareTo(b.Error);

    private readonly List<GesturePerformingAssessment> _sortedCompetitors;
    private readonly IEarlyWinnerCriterion _earlyWinnerCriterion;

    public IReadOnlyList<GesturePerformingAssessment> SortedCompetitors => _sortedCompetitors;

    public GesturePerformingCompetition(GesturePerformingAssessment[] competitors, [CanBeNull] IEarlyWinnerCriterion earlyWinnerCriterion)
    {
        Assert(competitors != null, "Competitors array must not be null");
        Assert(competitors.Length > 0, "Competitors array must not be empty");

        _sortedCompetitors = competitors.ToList();
        _earlyWinnerCriterion = earlyWinnerCriterion;
    }

    public void Advance(in ManipulationDelta manipulationDelta)
    {
        foreach (var competitor in _sortedCompetitors)
        {
            competitor.Advance(manipulationDelta);
        }

        SortCompetitors();
    }

    public bool HasEarlyWinner(in ManipulationState manipulationState, [NotNullWhen(true)] out GesturePerformingAssessment winner)
    {
        if (_earlyWinnerCriterion == null ||
            !_earlyWinnerCriterion.IsLeadingCompetitorAnEarlyWinner(_sortedCompetitors, manipulationState))
        {
            winner = null;
            return false;
        }

        winner = _sortedCompetitors[0];
        return true;
    }

    public void Finish(in ManipulationState finalManipulationState)
    {
        foreach (var competitor in _sortedCompetitors)
        {
            competitor.Finish(finalManipulationState);
        }

        SortCompetitors();
    }

    private void SortCompetitors()
    {
        _sortedCompetitors.Sort(CompetitorsComparison);
    }
}