using System.Linq;
using MathUtils;
using MathUtils.Curves;

namespace Data.Curves;

[CreateAssetMenu(menuName = "Curves/Smooth Curve")]
public class SmoothCurveData : ScriptableObject
{
    public Vector2 Start = Vector2.zero;
    public Vector2 End = Vector2.one;
    public SmoothCurveCornerData[] Corners = [new()];

    public int PositionCount => Corners.Length + 2;

    public ref Vector2 PositionAt(int index)
    {
        if (index < 0 || index >= PositionCount)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (index == 0)
            return ref Start;

        if (index == PositionCount - 1)
            return ref End;

        return ref Corners[index - 1].Position;
    }

    public ICurve ToCurve()
    {
        if (Corners.Length == 0)
            return new LineSegment(Start, End);

        var curveBuilder = new PiecewiseCurve.Builder();
        var guideSegmentLengthT = 1f / (Corners.Length + 1);

        TryAddLeadingLineSegment(curveBuilder, guideSegmentLengthT);

        var lastT = Corners[0].InBendingPointT * guideSegmentLengthT;
        for (var i = 0; i < Corners.Length; i++)
        {
            if (i > 0)
                TryAddIntermediaryLineSegment(curveBuilder, ref lastT, i, guideSegmentLengthT);

            AddCornerBezierCurve(curveBuilder, ref lastT, i, guideSegmentLengthT);
        }

        TryAddTrailingLineSegment(curveBuilder, guideSegmentLengthT);

        return curveBuilder.Build();
    }

    private void TryAddLeadingLineSegment(PiecewiseCurve.Builder curveBuilder, float guideSegmentLengthT)
    {
        var firstCorner = Corners[0];

        if (Mathf.Approximately(firstCorner.InBendingPointT, 0))
            return;

        curveBuilder.AddPiece(
            new LineSegment(Start, firstCorner.GetInBendingPosition(Start)),
            new FloatRange(0, firstCorner.InBendingPointT * guideSegmentLengthT));
    }

    private void TryAddTrailingLineSegment(PiecewiseCurve.Builder curveBuilder, float guideSegmentLengthT)
    {
        var lastCorner = Corners.Last();

        if (Mathf.Approximately(lastCorner.OutBendingPointT, 1))
            return;

        curveBuilder.AddPiece(
            new LineSegment(lastCorner.GetOutBendingPosition(End), End),
            new FloatRange(1 - (1 - lastCorner.OutBendingPointT) * guideSegmentLengthT, 1));
    }

    private void TryAddIntermediaryLineSegment(PiecewiseCurve.Builder curveBuilder, ref float lastT, int cornerIndex, float guideSegmentLengthT)
    {
        var prevCorner = Corners[cornerIndex - 1];
        var currentCorner = Corners[cornerIndex];

        if (Mathf.Approximately(currentCorner.InBendingPointT, prevCorner.OutBendingPointT))
            return;

        var prevOutBendingPosition = prevCorner.GetOutBendingPosition(currentCorner.Position);
        var currentInBendingPosition = currentCorner.GetInBendingPosition(prevCorner.Position);
        var lengthT = (currentCorner.InBendingPointT - prevCorner.OutBendingPointT) * guideSegmentLengthT;

        curveBuilder.AddPiece(
            new LineSegment(prevOutBendingPosition, currentInBendingPosition),
            new FloatRange(lastT, lastT + lengthT));

        lastT += lengthT;
    }

    private void AddCornerBezierCurve(PiecewiseCurve.Builder curveBuilder, ref float lastT, int i, float guideSegmentLengthT)
    {
        var corner = Corners[i];
        var prevPosition = i == 0
            ? Start
            : Corners[i - 1].Position;

        var nextPosition = i == Corners.Length - 1
            ? End
            : Corners[i + 1].Position;

        var inBendingPosition = corner.GetInBendingPosition(prevPosition);
        var outBendingPosition = corner.GetOutBendingPosition(nextPosition);
        var lengthT = ((1 - corner.InBendingPointT) + corner.OutBendingPointT) * guideSegmentLengthT;

        curveBuilder.AddPiece(
            new BezierCurve(inBendingPosition, corner.Position, outBendingPosition),
            new FloatRange(lastT, lastT + lengthT));

        lastT += lengthT;
    }
}