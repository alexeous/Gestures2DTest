using UnityEngine;

namespace MathObjects;

public readonly struct FloatRange
{
    public readonly float From;
    public readonly float To;

    public FloatRange(float from, float to)
    {
        Debug.Assert(from <= to);

        From = from;
        To = to;
    }

    public bool Contains(float value)
    {
        return From <= value && value <= To;
    }
}