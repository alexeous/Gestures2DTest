using MathUtils.Curves;

namespace Domain.Gestures;

public class Gesture
{
    public readonly IUniformlyParameterizedCurve Path;

    public Gesture(IUniformlyParameterizedCurve path)
    {
        Path = path;
    }
}