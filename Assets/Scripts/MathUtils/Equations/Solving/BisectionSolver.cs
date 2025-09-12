using Extensions;

namespace MathUtils.Equations.Solving;

public static class BisectionSolver
{
    public static float? TrySolve<TEquation>(in TEquation equation, FloatRange within,
        float epsilon = 0.0001f, int maxIterations = 100)
        where TEquation : IEquation
    {
        Assert(equation != null);

        var left = within.From;
        var right = within.To;

        var leftValue = equation.Function(left);
        var rightValue = equation.Function(right);

        Assert(leftValue.SignInt() != rightValue.SignInt(), $"Function values at the ends of the range {within} " +
                                                            $"must have different signs but are: {leftValue}, {rightValue}");

        var iterations = 0;
        while (iterations++ < maxIterations)
        {
            var middle = (left + right) * 0.5f;
            var middleValue = equation.Function(middle);

            if (Mathf.Abs(middleValue) < epsilon)
                return middle;

            if (middleValue.SignInt() == leftValue.SignInt())
                left = middle;
            else
                right = middle;
        }

        return null;
    }
}