using System.Linq;
using Components.Manipulations;
using Data.Gestures;
using Domain.Gestures;
using Domain.Gestures.Recognition;
using Domain.Gestures.Recognition.Competition;
using Domain.Gestures.Recognition.ErrorEvaluation;
using Domain.Manipulations;

namespace Components.Gestures.Recognition;

public class GestureRecognizer : MonoBehaviour
{
    public GestureData[] GestureDatas;
    public float EarlyWinnerMinimumTracedDistance = 0.6f;
    public float EarlyWinnerFractionOfFollowingCompetitorError = 0.5f;
    public float ErrorCutoff = 0.5f;

    private GesturePerformingAssessmentData[] _gesturePerformingAssessmentDatas;

    public GesturePerformingCompetition Competition { get; private set; }
    public GesturePerformingAssessment EarlyWinner { get; private set; }

    private void Awake()
    {
        _gesturePerformingAssessmentDatas = GestureDatas
            .Select(g => new GesturePerformingAssessmentData(g))
            .ToArray();
    }

    public void Begin()
    {
        var competitors = CreateCompetitors();
        var earlyWinnerCriterion = CreateEarlyWinnerCriterion();

        Competition = new GesturePerformingCompetition(competitors, earlyWinnerCriterion);
        EarlyWinner = null;
    }

    public void Advance(in ManipulationDelta manipulationDelta)
    {
        if (EarlyWinner != null)
        {
            AdvanceEarlyWinner(manipulationDelta);
            return;
        }

        AdvanceCompetition(manipulationDelta);
    }

    public GesturePerformingAssessment Finish(in ManipulationState finalManipulationState)
    {
        if (EarlyWinner != null)
            return FinishEarlyWinner(finalManipulationState);

        return FinishCompetition(finalManipulationState);
    }

    private GesturePerformingAssessment FinishEarlyWinner(in ManipulationState finalManipulationState)
    {
        EarlyWinner.Finish(finalManipulationState);

        var winner = EarlyWinner.Error <= ErrorCutoff
            ? EarlyWinner
            : null;

        EarlyWinner = null;

        return winner;
    }

    private GesturePerformingAssessment FinishCompetition(in ManipulationState finalManipulationState)
    {
        Competition.Finish(finalManipulationState);

        var leadingCompetitor = Competition.SortedCompetitors[0];
        var winner = leadingCompetitor.Error <= ErrorCutoff
            ? leadingCompetitor
            : null;

        Competition = null;

        return winner;
    }

    private void AdvanceCompetition(in ManipulationDelta manipulationDelta)
    {
        Competition.Advance(manipulationDelta);

        if (Competition.HasEarlyWinner(manipulationDelta.NextState, out var earlyWinner))
        {
            EarlyWinner = earlyWinner;
            Competition = null;
        }
    }

    private void AdvanceEarlyWinner(in ManipulationDelta manipulationDelta)
    {
        EarlyWinner.Advance(manipulationDelta);
    }

    private GesturePerformingAssessment[] CreateCompetitors()
    {
        return _gesturePerformingAssessmentDatas
            .Select(d => d.ToGesturePerformingAssessment())
            .ToArray();
    }

    private IEarlyWinnerCriterion CreateEarlyWinnerCriterion()
    {
        return new FractionOfFollowingCompetitorErrorCriterion(EarlyWinnerMinimumTracedDistance, EarlyWinnerFractionOfFollowingCompetitorError);
    }

    private readonly struct GesturePerformingAssessmentData
    {
        public readonly Gesture Gesture;
        public readonly IDeltaErrorEvaluator DeltaErrorEvaluator;
        public readonly IFinalErrorEvaluator FinalErrorEvaluator;

        public GesturePerformingAssessmentData(GestureData gestureData)
        {
            Gesture = gestureData.ToGesture();
            DeltaErrorEvaluator = gestureData.GetDeltaErrorEvaluator();
            FinalErrorEvaluator = gestureData.GetFinalErrorEvaluator();
        }

        public GesturePerformingAssessment ToGesturePerformingAssessment()
        {
            return new GesturePerformingAssessment(Gesture, DeltaErrorEvaluator, FinalErrorEvaluator);
        }
    }
}