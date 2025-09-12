using System.Runtime.CompilerServices;

namespace Extensions;

public static class FloatExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sign(this float value)
    {
        return Mathf.Sign(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SignInt(this float value)
    {
        return value >= 0 ? 1 : -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this float value, float other)
    {
        return Mathf.Approximately(value, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrApproximately(this float value, float other)
    {
        return value < other || Mathf.Approximately(value, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrApproximately(this float value, float other)
    {
        return value > other || Mathf.Approximately(value, other);
    }
}