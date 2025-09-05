using Data.Curves;
using MathUtils.Curves;
using MathUtils.RootFinding;

[ExecuteInEditMode]
public class TestSlide : MonoBehaviour
{
    public SmoothCurveData SmoothCurveData;

    public float ClosestPointT;
    public bool ClosestPointFound;

    private ICurve _curve;

    private void Update()
    {
        if (SmoothCurveData == null)
            return;

        _curve = SmoothCurveData.ToCurve();

        FindClosestPoint();
    }

    private void OnDrawGizmosSelected()
    {
        if (_curve == null)
            return;

        // DrawEquation();

        if (!ClosestPointFound)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(_curve.GetPosition(ClosestPointT), 0.025f);
    }

    private void FindClosestPoint()
    {
        var equation = new EquationForFindingClosestPointOnCurve(_curve, transform.position);

        var newClosestPointT = NewtonRootFinder.TryFind(ClosestPointT, equation.Function, equation.FunctionDerivative, minArg: 0, maxArg: 1);
        ClosestPointT = newClosestPointT ?? ClosestPointT;
        ClosestPointFound = newClosestPointT.HasValue;
    }

    private void PlotEquation()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector2(0, -1), new Vector2(1, -1));
        Gizmos.DrawLine(new Vector2(ClosestPointT, -0.95f), new Vector2(ClosestPointT, -1.05f));

        Gizmos.color = Color.red;

        var equation = new EquationForFindingClosestPointOnCurve(_curve, transform.position);
        const float step = 0.01f;
        for (var t = step; t - step < 1f; t += step)
        {
            var from = new Vector2(t - step, equation.Function(t - step) - 1);
            var to = new Vector2(t, equation.Function(t) - 1);

            Gizmos.DrawLine(from, to);
        }
    }
}