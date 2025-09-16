namespace MathUtils.Integration;

/// <summary>
/// Численное интегрирование по Симпсону.
/// <seealso href="https://ru.wikipedia.org/wiki/%D0%A4%D0%BE%D1%80%D0%BC%D1%83%D0%BB%D0%B0_%D0%A1%D0%B8%D0%BC%D0%BF%D1%81%D0%BE%D0%BD%D0%B0"/>
/// </summary>
public static class SimpsonIntegral
{
    public static float Calculate(Func<float, float> function, FloatRange range, int steps)
    {
        var step = range.Length / steps;

        return step / 3 * (function(range.From) +
                           CalculateEvenSum(function, range.From, step, steps) +
                           CalculateOddSum(function, range.From, step, steps) +
                           function(range.To));
    }

    private static float CalculateEvenSum(Func<float, float> function, float from, float step, int steps)
    {
        var sum = 0f;
        for (var i = 2; i < steps; i += 2)
        {
            sum += function(from + step * i);
        }

        return sum * 2;
    }

    private static float CalculateOddSum(Func<float, float> function, float from, float step, int steps)
    {
        var sum = 0f;
        for (var i = 1; i < steps; i += 2)
        {
            sum += function(from + step * i);
        }

        return sum * 4;
    }
}