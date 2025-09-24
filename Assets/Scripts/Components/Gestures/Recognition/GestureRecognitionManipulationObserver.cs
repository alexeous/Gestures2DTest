using System.Linq;
using Components.Manipulations;
using Domain.Manipulations;

namespace Components.Gestures.Recognition;

public class GestureRecognitionManipulationObserver : MonoBehaviour, IManipulationObserver
{
    public GestureRecognizer Recognizer;

    // For debug
    public Vector2 Position;
    public float TracedDistance;

    public int EarlyWinnerGestureId;
    public float EarlyWinnerError;
    public int WinnerGestureId;
    public float WinnerError;

    public DashboardEntry[] Dashboard;

    public void OnManipulationBegun(in ManipulationState initialManipulationState)
    {
        Recognizer.Begin();

        EarlyWinnerGestureId = 0;
        EarlyWinnerError = float.PositiveInfinity;
        WinnerGestureId = 0;
        WinnerError = float.PositiveInfinity;

        Dashboard = null;
    }

    public void OnManipulationAdvanced(in ManipulationDelta manipulationDelta)
    {
        Recognizer.Advance(manipulationDelta);

        TracedDistance = manipulationDelta.NextState.TracedDistance;
        Position = manipulationDelta.NextState.Position;

        EarlyWinnerGestureId = Recognizer.EarlyWinner?.Gesture.Id ?? 0;
        EarlyWinnerError = Recognizer.EarlyWinner?.Error ?? float.PositiveInfinity;

        Dashboard = Recognizer.Competition?.SortedCompetitors
            .Select(c => new DashboardEntry(c.Gesture.Id, c.Error))
            .ToArray();
    }

    public void OnManipulationFinished(in ManipulationState finalManipulationState)
    {
        var winner = Recognizer.Finish(finalManipulationState);

        WinnerGestureId = winner?.Gesture.Id ?? 0;
        WinnerError = winner?.Error ?? float.PositiveInfinity;
    }
}

[Serializable]
public struct DashboardEntry
{
    public int Id;
    public float Error;

    public DashboardEntry()
    {
    }

    public DashboardEntry(int id, float error)
    {
        Id = id;
        Error = error;
    }
}