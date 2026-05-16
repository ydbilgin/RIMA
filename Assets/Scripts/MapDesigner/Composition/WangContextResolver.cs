using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner.Composition
{
    public class WangContextResolver
    {
        public string ResolveCaseAt(Vector2Int pos, Tilemap walkableMaskTilemap)
        {
            if (walkableMaskTilemap == null)
            {
                return null;
            }

            if (!HasWallAt(walkableMaskTilemap, pos.x, pos.y))
            {
                return null;
            }

            int ne = HasWallAt(walkableMaskTilemap, pos.x + 1, pos.y + 1) ? 1 : 0;
            int nw = HasWallAt(walkableMaskTilemap, pos.x - 1, pos.y + 1) ? 1 : 0;
            int se = HasWallAt(walkableMaskTilemap, pos.x + 1, pos.y - 1) ? 1 : 0;
            int sw = HasWallAt(walkableMaskTilemap, pos.x - 1, pos.y - 1) ? 1 : 0;

            return string.Concat("wang_", ne.ToString(), nw.ToString(), se.ToString(), sw.ToString());
        }

        public BrushAssetVariant PickVariantForCase(string wangCase, List<BrushAssetVariant> candidates)
        {
            if (candidates == null || candidates.Count == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(wangCase))
            {
                return candidates[0];
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                BrushAssetVariant candidate = candidates[i];
                if (candidate != null && !string.IsNullOrEmpty(candidate.variantId) && candidate.variantId == wangCase)
                {
                    return candidate;
                }
            }

            return candidates[0];
        }

        private static bool HasWallAt(Tilemap tilemap, int x, int y)
        {
            TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
            return tile != null;
        }
    }
}
