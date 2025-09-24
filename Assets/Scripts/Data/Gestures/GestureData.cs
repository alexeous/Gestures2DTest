using Data.Curves;
using Domain.Gestures;

namespace Data.Gestures;

[CreateAssetMenu(fileName = "GestureData", menuName = "Gestures/GestureData")]
public class GestureData : ScriptableObject
{
    public SmoothCurveData PathData;

    public Gesture ToGesture()
    {
        var path = PathData.ToCurve();

        return new Gesture(path);
    }
}