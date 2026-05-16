#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public sealed class ScatterAlongStrokeExecutor : IBrushExecutor
    {
        public PaintMode SupportedMode
        {
            get { return PaintMode.ScatterAlongStroke; }
        }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            if (op == null || op.assetPool == null)
            {
                return new BrushExecutorResult { success = false, errorMessage = "Scatter operation or AssetPool is null" };
            }

            List<Vector2> path = BuildPath(stroke);
            float pathLength = ComputePathLength(path);
            float minDistance = Mathf.Max(0.01f, op.minDistance);
            int sampleCount = Mathf.Max(1, Mathf.CeilToInt(pathLength * Mathf.Max(0f, op.density) / minDistance));
            var spawned = new List<GameObject>();
            var placed = new List<Vector2>();

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Brush Scatter Stroke");

            for (int i = 0; i < sampleCount; i++)
            {
                Vector2 pos = SamplePath(path, sampleCount == 1 ? 0f : i / (float)(sampleCount - 1));
                if (IsTooClose(pos, placed, minDistance))
                {
                    continue;
                }

                BrushStroke candidate = stroke;
                candidate.currentPositionWorld = pos;
                candidate.currentCell = DecorativeExecutorUtility.WorldToCell(pos);
                if (!DecorativeExecutorUtility.CanPlace(candidate, op, i))
                {
                    continue;
                }

                Sprite sprite = DecorativeExecutorUtility.PickSprite(op.assetPool, stroke.seed, i);
                GameObject go = DecorativeExecutorUtility.PlaceAt(pos, sprite, op.targetLayer, op, stroke.seed, i);
                if (go == null)
                {
                    continue;
                }

                Undo.RegisterCreatedObjectUndo(go, "Brush Scatter Stroke");
                spawned.Add(go);
                placed.Add(pos);
            }

            Undo.CollapseUndoOperations(group);
            return new BrushExecutorResult
            {
                success = true,
                spawnedCount = spawned.Count,
                spawnedObjects = spawned
            };
        }

        private static List<Vector2> BuildPath(BrushStroke stroke)
        {
            if (stroke.strokePath != null && stroke.strokePath.Count > 0)
            {
                return stroke.strokePath;
            }

            return new List<Vector2> { stroke.startPositionWorld, stroke.currentPositionWorld };
        }

        private static float ComputePathLength(List<Vector2> path)
        {
            float length = 0f;
            for (int i = 1; i < path.Count; i++)
            {
                length += Vector2.Distance(path[i - 1], path[i]);
            }

            return length;
        }

        private static Vector2 SamplePath(List<Vector2> path, float t)
        {
            if (path.Count == 0)
            {
                return Vector2.zero;
            }

            if (path.Count == 1)
            {
                return path[0];
            }

            float total = ComputePathLength(path);
            if (total <= 0f)
            {
                return path[0];
            }

            float target = total * Mathf.Clamp01(t);
            float walked = 0f;
            for (int i = 1; i < path.Count; i++)
            {
                float segment = Vector2.Distance(path[i - 1], path[i]);
                if (walked + segment >= target)
                {
                    float local = segment <= 0f ? 0f : (target - walked) / segment;
                    return Vector2.Lerp(path[i - 1], path[i], local);
                }

                walked += segment;
            }

            return path[path.Count - 1];
        }

        private static bool IsTooClose(Vector2 candidate, List<Vector2> placed, float minDistance)
        {
            float minSq = minDistance * minDistance;
            for (int i = 0; i < placed.Count; i++)
            {
                if ((candidate - placed[i]).sqrMagnitude < minSq)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
#endif
