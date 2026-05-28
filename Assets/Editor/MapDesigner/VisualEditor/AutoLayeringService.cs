#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.MapDesigner.Brush.Data;

namespace RIMA.MapDesigner.VisualEditor
{
    public static class AutoLayeringService
    {
        public static Tilemap FindTargetTilemap(MapDesignerBrushPresetSO brush, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (brush == null)
            {
                errorMsg = "No brush selected.";
                return null;
            }

            // Decide which tilemap name to look for based on category
            string targetName = "FloorTilemap";
            switch (brush.category)
            {
                case BrushCategory.Floor:
                case BrushCategory.Variation:
                    targetName = "FloorTilemap";
                    break;
                case BrushCategory.Wall:
                    targetName = "WallTilemap";
                    break;
                case BrushCategory.Cliff:
                    // S110 Phase 2: dedicated Cliff brush category targets CliffTilemap
                    targetName = "CliffTilemap";
                    break;
                case BrushCategory.Transition:
                case BrushCategory.Composite:
                    // Usually transitions/cliffs go to CliffTilemap
                    targetName = "CliffTilemap";
                    break;
                default:
                    targetName = "FloorTilemap";
                    break;
            }

            // Find the tilemap in the active scene
            GameObject go = GameObject.Find(targetName);
            if (go == null && targetName == "FloorTilemap")
            {
                go = GameObject.Find("Tilemap"); // RIMA's floor tilemap is named "Tilemap"
            }
            if (go != null)
            {
                Tilemap tm = go.GetComponent<Tilemap>();
                if (tm != null) return tm;
            }

            // Fallback: search all tilemaps in scene
            Tilemap[] allTilemaps = Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
            foreach (Tilemap tm in allTilemaps)
            {
                if (tm.name.Contains(targetName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return tm;
                }
            }

            // Second fallback: find by standard names
            foreach (Tilemap tm in allTilemaps)
            {
                if (brush.category == BrushCategory.Wall && tm.name.Contains("wall", System.StringComparison.OrdinalIgnoreCase))
                    return tm;
                if (brush.category == BrushCategory.Floor && tm.name.Contains("floor", System.StringComparison.OrdinalIgnoreCase))
                    return tm;
            }

            if (allTilemaps.Length > 0)
            {
                return allTilemaps[0]; // fallback to first found
            }

            errorMsg = $"Could not find target Tilemap '{targetName}' in the scene.";
            return null;
        }

        public static Transform FindPropsContainer()
        {
            GameObject container = GameObject.Find("PropsContainer");
            if (container == null)
            {
                container = GameObject.Find("Props");
            }
            if (container == null)
            {
                container = new GameObject("PropsContainer");
            }
            return container.transform;
        }
    }
}
#endif
