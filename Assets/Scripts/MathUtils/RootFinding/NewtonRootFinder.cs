namespace MathUtils.RootFinding;

public static class NewtonRootFinder
{
    public static float? TryFind(float startArg, Func<float, float> function, Func<float, float> derivative,
        float? minArg = null,
        float? maxArg = null,
        float epsilon = 0.0001f,
        int maxIterations = 100)
    {
        var lastArg = startArg;
        var iterations = 0;

        while (iterations++ < maxIterations)
        {
            var improvedArg = lastArg - function(lastArg) / derivative(lastArg);

            if (improvedArg < minArg)
                improvedArg = minArg.Value;

            if (improvedArg > maxArg)
                improvedArg = maxArg.Value;

            if (Math.Abs(improvedArg - lastArg) < epsilon)
                return improvedArg;

            lastArg = improvedArg;
        }

        return null;
    }
}