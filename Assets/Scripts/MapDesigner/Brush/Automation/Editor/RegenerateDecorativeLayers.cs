#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;

namespace RIMA.MapDesigner.Brush.Automation.Editor
{
    public static class RegenerateDecorativeLayers
    {
        private static readonly TargetLayer[] DecorativeLayers = new[]
        {
            TargetLayer.L3, TargetLayer.L4, TargetLayer.L5, TargetLayer.L6
        };

        public static BrushExecutorResult Run(BrushPackSO pack, RoomData room, BiomeSkinSO skin, int newSeed)
        {
            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Regenerate Decorative: seed " + newSeed);

            try
            {
                ClearLayerContainers();
                return AutoDressRoom.Run(pack, room, skin, newSeed);
            }
            finally
            {
                Undo.CollapseUndoOperations(group);
            }
        }

        public static void ClearLayerContainers()
        {
            foreach (var layer in DecorativeLayers)
            {
                var container = GameObject.Find("Layer_" + layer);
                if (container == null) continue;
                for (int i = container.transform.childCount - 1; i >= 0; i--)
                {
                    var child = container.transform.GetChild(i).gameObject;
                    Undo.DestroyObjectImmediate(child);
                }
            }
        }
    }
}
#endif
