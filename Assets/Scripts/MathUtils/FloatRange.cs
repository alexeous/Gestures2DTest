namespace MathUtils;

public readonly struct FloatRange
{
    public readonly float From;
    public readonly float To;

    public float Length => To - From;

    public FloatRange(float from, float to)
    {
        Assert(from <= to, "From must be <= To");

        From = from;
        To = to;
    }

    public bool Contains(float value)
    {
        return From <= value && value <= To;
    }

    public float Lerp(float t)
    {
        return Mathf.Lerp(From, To, t);
    }

    public float InverseLerp(float t)
    {
        return Mathf.InverseLerp(From, To, t);
    }

    public override string ToString()
    {
        return $"|{From}, {To}|";
    }
}