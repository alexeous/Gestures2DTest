using Data.Curves;
using MathUtils.Curves;

namespace Components.Curves;

public class SmoothCurveEditingFixture : MonoBehaviour
{
    public SmoothCurveData Target;

#if UNITY_EDITOR

    public bool AlwaysShowInScene;

    private void OnDrawGizmos()
    {
        if (Target == null)
            return;

        if (AlwaysShowInScene)
            DrawCurve();
    }

    private void OnDrawGizmosSelected()
    {
        if (Target == null)
            return;

        if (!AlwaysShowInScene)
            DrawCurve();

        DrawOriginAndAxes();
    }

    private void DrawCurve()
    {
        var origin = transform.position;
        var curve = Target.ToCurve();

        Gizmos.color = Color.yellow;
        const float step = 0.01f;
        var prevPosition = curve.GetPosition(0);
        for (var t = step; t - step < 1f; t += step)
        {
            var currentPosition = curve.GetPosition(t);
            Gizmos.DrawLine(origin + (Vector3)prevPosition, origin + (Vector3)currentPosition);
            prevPosition = currentPosition;
        }

        DrawRunningAnts(curve);
    }

    private void DrawRunningAnts(ICurve curve)
    {
        const int ants = 20;

        Gizmos.color = new Color(1, 1, 1, 0.5f);

        for (var i = 0; i < ants; i++)
        {
            var t = ((float)i / ants + Time.unscaledTime * 0.1f) % 1;
            var position = curve.GetPosition(t);

            Gizmos.DrawSphere(position, 0.025f);
        }
    }

    private void DrawOriginAndAxes()
    {
        const float crossSize = 0.1f;

        var origin = transform.position;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(origin - new Vector3(0, crossSize, 0), origin + new Vector3(0, 1, 0));
        Gizmos.DrawLine(origin - new Vector3(crossSize, 0, 0), origin + new Vector3(1, 0, 0));
    }

#endif
}