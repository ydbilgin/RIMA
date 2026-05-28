using RIMA;
using RIMA.Systems.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public static class TilemapMutator
    {
        public static int ApplyTerrainGrid(Tilemap outputTilemap, RimaBiomePreset biome, int[,] terrainGrid, int w, int h)
        {
            if (outputTilemap == null || biome == null || terrainGrid == null)
            {
                return 0;
            }

            Undo.RegisterCompleteObjectUndo(outputTilemap, "Apply RIMA Map");
            CornerWangPainter.Paint(outputTilemap, biome, terrainGrid, w, h);
            EditorUtility.SetDirty(outputTilemap);
            Debug.Log("[TilemapMutator] Applied single terrain grid");
            return 1;
        }
    }
}
