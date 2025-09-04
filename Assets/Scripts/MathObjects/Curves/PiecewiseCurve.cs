using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Extensions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MathObjects.Curves;

public class PiecewiseCurve : ICurve
{
    public readonly IReadOnlyList<PiecewiseCurveSegment> Segments;

    public PiecewiseCurve(IReadOnlyList<PiecewiseCurveSegment> segments)
    {
        ValidateSegments(segments);

        Segments = segments;
    }

    public Vector2 GetPosition(float t)
    {
        var segment = GetSegmentByT(t);
        return segment.Curve.GetPosition(segment.GlobalToLocalT(t));
    }

    public Vector2 GetDerivative(float t)
    {
        var segment = GetSegmentByT(t);
        return segment.Curve.GetDerivative(segment.GlobalToLocalT(t));
    }

    public Vector2 GetDerivative2(float t)
    {
        var segment = GetSegmentByT(t);
        return segment.Curve.GetDerivative2(segment.GlobalToLocalT(t));
    }

    private PiecewiseCurveSegment GetSegmentByT(float t)
    {
        if (t <= 0)
            return Segments.First();

        foreach (var segment in Segments)
        {
            if (segment.Range.Contains(t))
                return segment;
        }

        return Segments.Last();
    }

    [Conditional("UNITY_EDITOR")]
    private static void ValidateSegments(IReadOnlyList<PiecewiseCurveSegment> segments)
    {
        Debug.Assert(segments != null, "Segments must not be null");
        Debug.Assert(segments.Count >= 1, "Segments must have at least one segment");

        Debug.Assert(Mathf.Approximately(segments.First().Range.From, 0), "Range of first segment must start with 0");
        Debug.Assert(Mathf.Approximately(segments.Last().Range.To, 1), "Range of last segment must end with 1");

        var prev = segments.First();
        foreach (var next in segments.Skip(1))
        {
            Debug.Assert(Mathf.Approximately(prev.Range.To, next.Range.From), "Ranges of adjacent segments must be connected");
            Debug.Assert(MathfExtensions.Approximately(prev.Curve.GetPosition(1), next.Curve.GetPosition(0)), "Adjacent segments must be connected");

            prev = next;
        }
    }

    public class Builder
    {
        private readonly List<PiecewiseCurveSegment> _segments = [];

        public Builder Add(ICurve curve, FloatRange range)
        {
            _segments.Add(new PiecewiseCurveSegment(curve, range));
            return this;
        }

        public PiecewiseCurve Build() => new(_segments);
    }
}

public class PiecewiseCurveSegment
{
    public readonly ICurve Curve;
    public readonly FloatRange Range;

    public PiecewiseCurveSegment(ICurve curve, FloatRange range)
    {
        Debug.Assert(range.From >= 0);
        Debug.Assert(range.To <= 1);

        Curve = curve;
        Range = range;
    }

    public float GlobalToLocalT(float globalT)
    {
        return Mathf.InverseLerp(Range.From, Range.To, globalT);
    }
}