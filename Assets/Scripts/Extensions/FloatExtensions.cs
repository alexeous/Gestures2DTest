using System.Runtime.CompilerServices;

namespace Extensions;

public static class FloatExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sign(this float value) => Mathf.Sign(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SignInt(this float value) => value >= 0 ? 1 : -1;
}