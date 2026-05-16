#if UNITY_EDITOR
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public sealed class EraseByLayerExecutor : IBrushExecutor
    {
        public PaintMode SupportedMode
        {
            get { return PaintMode.EraseByLayer; }
        }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            Transform root = DecorativeEraseUtility.FindRoot(op != null ? op.targetLayer : TargetLayer.L4);
            if (root == null)
            {
                return new BrushExecutorResult { success = true, spawnedCount = 0 };
            }

            float radius = Mathf.Clamp(op != null && op.minDistance > 0f ? op.minDistance / 32f : 0.5f, 0.5f, 2f);
            float radiusSq = radius * radius;
            int removed = 0;

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Brush Erase Layer");

            for (int i = root.childCount - 1; i >= 0; i--)
            {
                GameObject child = root.GetChild(i).gameObject;
                Vector2 delta = (Vector2)child.transform.position - stroke.currentPositionWorld;
                if (delta.sqrMagnitude <= radiusSq)
                {
                    Undo.DestroyObjectImmediate(child);
                    removed++;
                }
            }

            Undo.CollapseUndoOperations(group);
            return new BrushExecutorResult { success = true, spawnedCount = removed };
        }
    }
}
#endif
