using System.Collections.Generic;
using Extensions;

namespace MathUtils.Curves;

/// <summary>
/// Кривая, кусочно составленная из других кривых.
/// <seealso href="https://ru.wikipedia.org/wiki/%D0%9A%D1%83%D1%81%D0%BE%D1%87%D0%BD%D0%BE-%D0%B7%D0%B0%D0%B4%D0%B0%D0%BD%D0%BD%D0%B0%D1%8F_%D1%84%D1%83%D0%BD%D0%BA%D1%86%D0%B8%D1%8F"/>
/// </summary>
public class PiecewiseCurve : ICurve
{
    public readonly PiecewiseCurvePiece[] Pieces;

    private PiecewiseCurve(PiecewiseCurvePiece[] pieces)
    {
        Pieces = pieces;
    }

    public Vector2 GetPosition(float t)
    {
        t = Mathf.Clamp01(t);
        var piece = GetPieceByT(t);

        return piece.Curve.GetPosition(piece.GlobalToLocalT(t));
    }

    public Vector2 GetDerivative(float t)
    {
        t = Mathf.Clamp01(t);
        var piece = GetPieceByT(t);

        return piece.Curve.GetDerivative(piece.GlobalToLocalT(t)) / piece.Range.Length;
    }

    public Vector2 GetDerivative2(float t)
    {
        t = Mathf.Clamp01(t);
        var piece = GetPieceByT(t);

        return piece.Curve.GetDerivative2(piece.GlobalToLocalT(t)) / piece.Range.Length;
    }

    private PiecewiseCurvePiece GetPieceByT(float t)
    {
        if (t <= 0)
            return Pieces[0];

        foreach (var piece in Pieces)
        {
            if (piece.Range.Contains(t))
                return piece;
        }

        return Pieces[^1];
    }

    public class Builder
    {
        private readonly List<PiecewiseCurvePiece> _pieces = [];

        public Builder AddPiece(ICurve curve, FloatRange range)
        {
            Assert(curve != null, "Curve must not be null");

            if (_pieces.Count == 0)
            {
                Assert(range.From.IsApproximately(0), $"Range of first piece must start with 0, but got {range}");
            }
            else
            {
                var last = _pieces[^1];

                Assert(last.Range.To.IsApproximately(range.From), $"Ranges of adjacent pieces must be connected, but they are {last.Range}, {range}");
                Assert(last.Curve.GetPosition(1).IsApproximately(curve.GetPosition(0)), "Adjacent pieces must be connected");
            }

            _pieces.Add(new PiecewiseCurvePiece(curve, range));

            return this;
        }

        public PiecewiseCurve Build()
        {
            Assert(_pieces.Count > 0, "There must be at least one piece added to build a piecewise curve");
            Assert(_pieces[^1].Range.To.IsApproximately(1), $"Range of last piece must end with 1, but got {_pieces[^1].Range}");

            return new PiecewiseCurve(_pieces.ToArray());
        }
    }
}

public readonly struct PiecewiseCurvePiece
{
    public readonly ICurve Curve;
    public readonly FloatRange Range;

    public PiecewiseCurvePiece(ICurve curve, FloatRange range)
    {
        Assert(curve != null, "Curve must not be null");
        Assert(range.From.IsGreaterThanOrApproximately(0));
        Assert(range.To.IsLessThanOrApproximately(1));

        Curve = curve;
        Range = range;
    }

    public float GlobalToLocalT(float globalT)
    {
        return (globalT - Range.From) / Range.Length;
    }
}