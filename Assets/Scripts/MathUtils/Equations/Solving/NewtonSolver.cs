namespace MathUtils.Equations.Solving;

public static class NewtonSolver
{
    public static float? TrySolve<TEquation>(in TEquation equation, float startArg,
        float? minArg = null, float? maxArg = null,
        float epsilon = 0.0001f, int maxIterations = 100)
        where TEquation : IDifferentiableEquation
    {
        Assert(equation != null);

        var lastArg = startArg;
        var iterations = 0;

        while (iterations++ < maxIterations)
        {
            var improvedArg = lastArg - equation.Function(lastArg) / equation.FunctionDerivative(lastArg);

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