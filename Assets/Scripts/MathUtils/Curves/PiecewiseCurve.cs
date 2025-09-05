using System.Collections.Generic;
using System.Diagnostics;
using Extensions;

namespace MathUtils.Curves;

public class PiecewiseCurve : ICurve
{
    public readonly PiecewiseCurvePiece[] Pieces;

    public PiecewiseCurve(PiecewiseCurvePiece[] pieces)
    {
        ValidatePieces(pieces);

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

    [Conditional("UNITY_EDITOR")]
    private static void ValidatePieces(PiecewiseCurvePiece[] pieces)
    {
        Assert(pieces != null, "Pieces array must not be null");
        Assert(pieces.Length >= 1, "Pieces array must have at least one element");

        Assert(Mathf.Approximately(pieces[0].Range.From, 0), "Range of first piece must start with 0");
        Assert(Mathf.Approximately(pieces[^1].Range.To, 1), "Range of last piece must end with 1");

        for (var i = 0; i < pieces.Length - 1; i++)
        {
            var prev = pieces[i];
            var next = pieces[i + 1];

            Assert(Mathf.Approximately(prev.Range.To, next.Range.From), $"Ranges of adjacent pieces must be connected but are {prev.Range.To}, {next.Range.From}");
            Assert(MathfExtensions.Approximately(prev.Curve.GetPosition(1), next.Curve.GetPosition(0)), "Adjacent pieces must be connected");
        }
    }

    public class Builder
    {
        private readonly List<PiecewiseCurvePiece> _pieces = [];

        public Builder AddPiece(ICurve curve, FloatRange range)
        {
            _pieces.Add(new PiecewiseCurvePiece(curve, range));
            return this;
        }

        public PiecewiseCurve Build() => new(_pieces.ToArray());
    }
}

public class PiecewiseCurvePiece
{
    public readonly ICurve Curve;
    public readonly FloatRange Range;

    public PiecewiseCurvePiece(ICurve curve, FloatRange range)
    {
        Assert(range.From > 0 || Mathf.Approximately(range.From, 0));
        Assert(range.To < 1 || Mathf.Approximately(range.To, 1));

        Curve = curve;
        Range = range;
    }

    public float GlobalToLocalT(float globalT)
    {
        return (globalT - Range.From) / Range.Length;
    }
}