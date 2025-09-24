using MathUtils.Curves;

namespace Domain.Gestures;

public class Gesture
{
    public readonly int Id;
    public readonly IUniformlyParameterizedCurve Path;

    public Gesture(int id, IUniformlyParameterizedCurve path)
    {
        Id = id;
        Path = path;
    }
}