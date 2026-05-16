#if UNITY_EDITOR
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public sealed class EraseAllDecorativeExecutor : IBrushExecutor
    {
        public PaintMode SupportedMode
        {
            get { return PaintMode.EraseAllDecorative; }
        }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            TargetLayer[] layers = { TargetLayer.L3, TargetLayer.L4, TargetLayer.L5, TargetLayer.L6 };
            int removed = 0;

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Brush Erase All Decorative");

            for (int i = 0; i < layers.Length; i++)
            {
                Transform root = DecorativeEraseUtility.FindRoot(layers[i]);
                if (root == null)
                {
                    continue;
                }

                for (int child = root.childCount - 1; child >= 0; child--)
                {
                    Undo.DestroyObjectImmediate(root.GetChild(child).gameObject);
                    removed++;
                }
            }

            Undo.CollapseUndoOperations(group);
            return new BrushExecutorResult { success = true, spawnedCount = removed };
        }
    }

    internal static class DecorativeEraseUtility
    {
        public static Transform FindRoot(TargetLayer layer)
        {
            string rootName = RootNameFor(layer);
            if (string.IsNullOrEmpty(rootName))
            {
                return null;
            }

            GameObject rootObject = GameObject.Find(rootName);
            if (rootObject != null)
            {
                return rootObject.transform;
            }

            Component[] hosts = Object.FindObjectsByType<Component>(FindObjectsSortMode.None);
            for (int i = 0; i < hosts.Length; i++)
            {
                Transform child = hosts[i].transform.Find(rootName);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        private static string RootNameFor(TargetLayer layer)
        {
            switch (layer)
            {
                case TargetLayer.L3:
                    return "WallOverlay";
                case TargetLayer.L4:
                    return "TransitionBrushLayer";
                case TargetLayer.L5:
                    return "DetailDecalLayer";
                case TargetLayer.L6:
                    return "AccentLayer";
                default:
                    return null;
            }
        }
    }
}
#endif
