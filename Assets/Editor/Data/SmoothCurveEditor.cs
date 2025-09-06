using Components.Curves;
using Data.Curves;
using UnityEditor;
using UnityEngine;

namespace Editor.Data
{
    [CustomEditor(typeof(SmoothCurveEditingFixture))]
    [CanEditMultipleObjects]
    public class SmoothCurveEditor : UnityEditor.Editor
    {
        private const float PositionHandleSize = 0.035f;
        private const float BendingHandleSize = 0.02f;
        private const float BendingHandlePerpShift = 0.02f;

        private SmoothCurveEditingFixture Fixture => (SmoothCurveEditingFixture)target;

        public void OnSceneGUI()
        {
            if (Fixture.Target == null)
                return;

            DrawGuides();
            DoPositionsHandles();
            DoBendingPointsHandles();

            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Fixture.Target == null)
                return;

            var targetCurveEditor = CreateEditor(Fixture.Target);
            targetCurveEditor.OnInspectorGUI();
            targetCurveEditor.serializedObject.ApplyModifiedProperties();
        }

        private void DrawGuides()
        {
            var smoothCurveData = Fixture.Target;
            var origin = Fixture.transform.position;

            Handles.color = new Color(1f, 1f, 1f, 0.5f);
            for (var i = 0; i < smoothCurveData.PositionCount - 1; i++)
            {
                Handles.DrawDottedLine(origin + (Vector3)smoothCurveData.PositionAt(i), origin + (Vector3)smoothCurveData.PositionAt(i + 1), 2f);
            }
        }

        private void DoPositionsHandles()
        {
            var smoothCurveData = Fixture.Target;

            Handles.color = Color.white;
            for (var i = 0; i < smoothCurveData.PositionCount; i++)
            {
                smoothCurveData.PositionAt(i) = DoPositionHandle(smoothCurveData.PositionAt(i));
            }
        }

        private void DoBendingPointsHandles()
        {
            var smoothCurveData = Fixture.Target;

            Handles.color = Color.white;
            for (var i = 0; i < smoothCurveData.Corners.Length; i++)
            {
                var corner = smoothCurveData.Corners[i];

                corner.InBendingPointT = DoInBendingPointHandle(smoothCurveData, i);
                corner.OutBendingPointT = DoOutBendingPointHandle(smoothCurveData, i);
            }
        }

        private Vector2 DoPositionHandle(Vector2 position)
        {
            var origin = Fixture.transform.position;

            return Handles.FreeMoveHandle(origin + (Vector3)position, PositionHandleSize, Vector3.zero, Handles.SphereHandleCap) - origin;
        }

        private float DoInBendingPointHandle(SmoothCurveData smoothCurveData, int cornerIndex)
        {
            var corner = smoothCurveData.Corners[cornerIndex];
            var prevPosition = smoothCurveData.PositionAt(cornerIndex);

            var inBendingPosition = corner.GetInBendingPosition(prevPosition);
            var inDir = corner.Position - prevPosition;

            var newInBendingPosition = DoBendingHandle(inBendingPosition, inDir);
            var newInBendingPointT = (corner.Position - newInBendingPosition).sqrMagnitude > inDir.sqrMagnitude
                ? 0
                : (newInBendingPosition - prevPosition).magnitude / inDir.magnitude;

            newInBendingPointT = Mathf.Clamp01(newInBendingPointT);
            if (cornerIndex > 0)
                newInBendingPointT = Mathf.Max(newInBendingPointT, smoothCurveData.Corners[cornerIndex - 1].OutBendingPointT);

            return newInBendingPointT;
        }

        private float DoOutBendingPointHandle(SmoothCurveData smoothCurveData, int cornerIndex)
        {
            var corner = smoothCurveData.Corners[cornerIndex];
            var nextPosition = smoothCurveData.PositionAt(cornerIndex + 2);

            var outBendingPosition = corner.GetOutBendingPosition(nextPosition);
            var outDir = corner.Position - nextPosition;

            var newOutBendingPosition = DoBendingHandle(outBendingPosition, outDir);
            var newOutBendingPointT = (nextPosition - newOutBendingPosition).sqrMagnitude > outDir.sqrMagnitude
                ? 0
                : (newOutBendingPosition - corner.Position).magnitude / outDir.magnitude;

            newOutBendingPointT = Mathf.Clamp01(newOutBendingPointT);
            if (cornerIndex < smoothCurveData.Corners.Length - 1)
                newOutBendingPointT = Mathf.Min(newOutBendingPointT, smoothCurveData.Corners[cornerIndex + 1].InBendingPointT);

            return newOutBendingPointT;
        }

        private Vector2 DoBendingHandle(Vector2 bendingPosition, Vector2 direction)
        {
            direction.Normalize();

            var origin = (Vector2)Fixture.transform.position;

            var perpOffset = BendingHandlePerpShift * Vector2.Perpendicular(direction);
            Handles.DrawLine(bendingPosition, bendingPosition + perpOffset);

            var longOffset = BendingHandleSize * 0.5f * direction;
            var totalOffset = (Vector3)(origin + longOffset + perpOffset);

            return Handles.Slider((Vector3)bendingPosition + totalOffset, direction, BendingHandleSize, Handles.ConeHandleCap, snap: 0) - totalOffset;
        }
    }
}