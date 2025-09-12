using System.Runtime.CompilerServices;

namespace Extensions;

public static class Vector2Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsApproximately(this Vector2 vector, Vector2 other)
    {
        return vector.x.IsApproximately(other.x) &&
               vector.y.IsApproximately(other.y);
    }
}