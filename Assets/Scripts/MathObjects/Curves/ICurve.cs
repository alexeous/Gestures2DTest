using UnityEngine;

namespace MathObjects.Curves;

public interface ICurve
{
    Vector2 GetPosition(float t);
    Vector2 GetDerivative(float t);
    Vector2 GetDerivative2(float t);
}