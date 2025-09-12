namespace MathUtils.Equations;

/// <summary>
/// An equation in the form
/// "f(x) = 0"
/// where f(x) is a differentiable function.
/// </summary>
public interface IDifferentiableEquation : IEquation
{
    /// <summary>
    /// f'(x) - the derivative of f(x).
    /// </summary>
    float FunctionDerivative(float x);
}