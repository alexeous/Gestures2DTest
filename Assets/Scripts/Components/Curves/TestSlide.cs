using System.Collections.Generic;
using Data.Curves;
using Extensions;
using MathUtils;
using MathUtils.Curves;
using MathUtils.Curves.Analysis;
using MathUtils.Equations.Solving;

namespace Components.Curves;

[ExecuteInEditMode]
public class TestSlide : MonoBehaviour
{
    public SmoothCurveData SmoothCurveData;

    public bool DrawAdditionalGizmos = true;
    public bool AlwaysShowInScene;

    public float ClosestPointT;
    public float ClosestPointT2;
    public bool ClosestPointFound;
    public List<float> Candidates = [];

    private IUniformlyParameterizedCurve _curve;

    private void Update()
    {
        if (SmoothCurveData == null)
            return;

        _curve = SmoothCurveData.ToCurve();

        RenewCandidates();
        FindClosestPoint();
    }

    private void OnDrawGizmosSelected()
    {
        if (AlwaysShowInScene)
            return;

        DrawGizmos();
    }

    private void OnDrawGizmos()
    {
        if (!AlwaysShowInScene)
            return;

        DrawGizmos();
    }

    private void DrawGizmos()
    {
        if (_curve == null)
            return;

        if (DrawAdditionalGizmos)
        {
            // DrawLines();
            PlotEquation();
        }

        Gizmos.color = Color.red;
        // Gizmos.DrawSphere(_curve.GetPosition(ClosestPointT), 0.025f);

        if (!ClosestPointFound)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_curve.GetPosition(ClosestPointT2), 0.025f);
    }

    private void RenewCandidates()
    {
        var equation = new EquationForFindingPerpendicularsToCurve(_curve, transform.position);

        const int steps = 100;
        const float stepSize = 1f / steps;

        Candidates.Clear();
        Candidates.Add(0);
        Candidates.Add(1);

        for (var i = 0; i < steps; i++)
        {
            var left = i * stepSize;
            var right = (i + 1) * stepSize;

            var funcAtLeft = equation.Function(left);
            var funcAtRight = equation.Function(right);

            if (funcAtLeft.SignInt() != funcAtRight.SignInt())
            {
                var perpendicularPointT = BisectionSolver.TrySolve(equation, new FloatRange(left, right));
                if (perpendicularPointT.HasValue)
                    Candidates.Add(perpendicularPointT.Value);
            }
        }
    }

    private void FindClosestPoint()
    {
        ClosestPointT = TryFindClosestPointUsingNewtonMethod() ?? FindClosestPointAmongCandidates();
        var newton = TryFindClosestPointUsingNewtonMethod();

        ClosestPointT2 = newton ?? ClosestPointT2;
        ClosestPointFound = newton.HasValue;
    }

    private float FindClosestPointAmongCandidates()
    {
        var newClosestPointT = ClosestPointT;
        var minDistance = float.PositiveInfinity;
        foreach (var candidate in Candidates)
        {
            var distance = Mathf.Abs(candidate - ClosestPointT);
            if (distance < minDistance)
            {
                minDistance = distance;
                newClosestPointT = candidate;
            }
        }

        return newClosestPointT;

        // var currentPosition = (Vector2)transform.position;
        //
        // var newClosestPointT = (float?)null;
        // var minSqrDistance = float.MaxValue;
        //
        // foreach (var basePointT in Candidates)
        // {
        //     var sqrDistance = (_curve.GetPosition(basePointT) - currentPosition).SqrMagnitude();
        //     if (sqrDistance < minSqrDistance)
        //     {
        //         minSqrDistance = sqrDistance;
        //         newClosestPointT = basePointT;
        //     }
        // }
        //
        // return newClosestPointT!.Value;
    }

    private float? TryFindClosestPointUsingNewtonMethod()
    {
        var currentPosition = (Vector2)transform.position;
        var equation = new EquationForFindingPerpendicularsToCurve(_curve, currentPosition);

        return NewtonSolver.TrySolve(equation, ClosestPointT, minArg: 0, maxArg: 1);
    }

    private void PlotEquation()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector2(0, -1), new Vector2(1, -1));
        Gizmos.DrawLine(new Vector2(ClosestPointT, -0.95f), new Vector2(ClosestPointT, -1.05f));

        Gizmos.color = Color.red;

        var equation = new EquationForFindingPerpendicularsToCurve(_curve, transform.position);
        const float step = 0.002f;
        for (var t = step; t - step < 1f; t += step)
        {
            var from = new Vector2(t - step, equation.Function(t - step) / 3 - 1);
            var to = new Vector2(t, equation.Function(t) / 3 - 1);

            Gizmos.DrawLine(from, to);
        }

        Gizmos.color = Color.yellow;
        for (var t = step; t - step < 1f; t += step)
        {
            var from = new Vector2(t - step, equation.FunctionDerivative(t - step) / 2 - 1);
            var to = new Vector2(t, equation.FunctionDerivative(t) / 2 - 1);

            Gizmos.DrawLine(from, to);
        }
    }

    private void DrawLines()
    {
        Gizmos.color = Color.white;

        var currentPosition = (Vector2)transform.position;
        foreach (var basePointT in Candidates)
        {
            var perpendicularBase = _curve.GetPosition(basePointT);

            Gizmos.DrawLine(currentPosition, perpendicularBase);
            Gizmos.DrawSphere(perpendicularBase, 0.025f);
        }
    }
}