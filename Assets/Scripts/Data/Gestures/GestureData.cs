using Data.Curves;
using Domain.Gestures;
using Domain.Gestures.Recognition.ErrorEvaluation;

namespace Data.Gestures;

[CreateAssetMenu(fileName = "GestureData", menuName = "Gestures/GestureData")]
public class GestureData : ScriptableObject
{
    public int Id;
    public SmoothCurveData PathData;

    public AnimationCurve IdealDistanceZone = AnimationCurve.Linear(0, 0.1f, 1, 0.17f);
    public float IdealFinishRadius = 0.2f;

    public Gesture ToGesture()
    {
        var path = PathData.ToCurve();

        return new Gesture(Id, path);
    }

    public IDeltaErrorEvaluator GetDeltaErrorEvaluator()
    {
        return new TrapezoidalDeltaErrorEvaluator(IdealDistanceZone);
    }

    public IFinalErrorEvaluator GetFinalErrorEvaluator()
    {
        return new RemainingDistanceFinalErrorEvaluator(IdealFinishRadius);
    }
}