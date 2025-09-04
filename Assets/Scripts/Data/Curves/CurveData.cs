using System;
using MathObjects;
using MathObjects.Curves;
using UnityEngine;

namespace Unity.Data.Curves.Serializable;

[Serializable]
public class CurveData
{
    public CurveSegmentData[] Segments = [new()];

    public ICurve ToCurve()
    {
        var curveBuilder = new PiecewiseCurve.Builder();
        foreach (var segment in Segments)
        {
            curveBuilder.Add(
                new BezierCurve(segment.P0, segment.P1, segment.P2),
                new FloatRange(segment.From, segment.To));
        }

        var curve = curveBuilder.Build();
        return curve;
    }
}

[Serializable]
public struct CurveSegmentData
{
    public Vector2 P0, P1, P2;

    [Range(0, 1)] public float From;
    [Range(0, 1)] public float To;
}