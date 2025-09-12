namespace MathUtils.Equations;

/// <summary>
/// An equation in the form
/// "f(x) = 0"
/// </summary>
public interface IEquation
{
    /// <summary>
    /// The function f(x)
    /// </summary>
    float Function(float x);
}