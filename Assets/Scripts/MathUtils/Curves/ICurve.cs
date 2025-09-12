namespace MathUtils.Curves;

/// <summary>
/// Абстрактная 2-мерная кривая, заданная параметрически.
/// Параметр t меняется от 0 до 1 включительно.
/// </summary>
public interface ICurve
{
    Vector2 GetPosition(float t);
    Vector2 GetDerivative(float t);
    Vector2 GetDerivative2(float t);
}