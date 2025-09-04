using Unity.Data.Curves;
using UnityEditor;
using UnityEngine;

namespace Editor.Data
{
    [CustomEditor(typeof(CurveDataEditProxy))]
    public class CurveDataEditor : UnityEditor.Editor
    {
        private CurveDataEditProxy Script => (CurveDataEditProxy)target;

        public void OnSceneGUI()
        {
            if (Script.Storage == null)
                return;

            DrawCurve();
            DrawHandles();
        }

        private void DrawCurve()
        {
            var curve = Script.Storage.CurveData.ToCurve();

            var origin = Script.transform.position;

            // Origin (rendered as a cross)
            Handles.color = Color.white;
            const float crossSize = 0.5f;
            Handles.DrawLine(origin - new Vector3(0, crossSize, 0), origin + new Vector3(0, crossSize, 0));
            Handles.DrawLine(origin - new Vector3(crossSize, 0, 0), origin + new Vector3(crossSize, 0, 0));

            // Curve
            Handles.color = Color.yellow;
            const float step = 0.01f;
            var prevPosition = curve.GetPosition(0);
            for (var t = step; t - step < 1f; t += step)
            {
                var currentPosition = curve.GetPosition(t);
                Handles.DrawLine(origin + (Vector3)prevPosition, origin + (Vector3)currentPosition);
                prevPosition = currentPosition;
            }
        }

        private void DrawHandles()
        {
            var origin = Script.transform.position;

            var prevEnd = (Vector2?)null;
            for (var i = 0; i < Script.Storage.CurveData.Segments.Length; i++)
            {
                ref var segment = ref Script.Storage.CurveData.Segments[i];

                segment.P0 = prevEnd ?? DoHandleForPoint(origin, segment.P0); // keeps segments connected and avoids doubling handles
                segment.P1 = DoHandleForPoint(origin, segment.P1);
                segment.P2 = DoHandleForPoint(origin, segment.P2);

                prevEnd = segment.P2;
            }
        }

        private Vector3 DoHandleForPoint(Vector3 origin, Vector3 point)
        {
            return Handles.PositionHandle(origin + point, Quaternion.identity) - origin;
        }
    }
}