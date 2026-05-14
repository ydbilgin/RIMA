using System.Collections.Generic;
using RIMA;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public static class TilemapMutator
    {
        public static int ApplyVertexGrids(List<RimaMapDesignerWindow.MapLayer> layers, int w, int h)
        {
            int painted = 0;
            if (layers == null)
            {
                return painted;
            }

            foreach (RimaMapDesignerWindow.MapLayer layer in layers)
            {
                if (layer == null || !layer.enabled || layer.tilemap == null || layer.tileSet == null || layer.vertGrid == null)
                {
                    continue;
                }

                Undo.RegisterCompleteObjectUndo(layer.tilemap, "Apply RIMA Tilemap Layer");
                CornerWangPainter.Paint(layer.tilemap, layer.tileSet, layer.vertGrid, w, h);
                EditorUtility.SetDirty(layer.tilemap);
                painted++;
            }

            Debug.Log("[TilemapMutator] Applied " + painted + " layers");
            return painted;
        }
    }
}
