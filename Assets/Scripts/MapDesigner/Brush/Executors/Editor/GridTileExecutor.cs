#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Stroke;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner.Brush.Executors.Editor
{
    public sealed class GridTileExecutor : IBrushExecutor
    {
        public GridTileExecutor()
            : this(PaintMode.GridTile)
        {
        }

        public GridTileExecutor(PaintMode supportedMode)
        {
            SupportedMode = supportedMode;
        }

        public PaintMode SupportedMode { get; private set; }

        public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op)
        {
            if (op == null || op.assetPool == null || op.assetPool.tiles == null || op.assetPool.tiles.Count == 0)
            {
                return Error("Grid tile asset pool is empty");
            }

            Tilemap tilemap = ResolveTilemap(op.targetLayer);
            if (tilemap == null)
            {
                return Error("No Tilemap found for " + op.targetLayer);
            }

            TileBase tile = PickTile(op.assetPool, stroke, SupportedMode == PaintMode.GridTileRandom);
            if (tile == null)
            {
                return Error("No non-null tile found in asset pool");
            }

            Undo.RegisterCompleteObjectUndo(tilemap, "Brush Grid Tile");
            Vector3Int cell = new Vector3Int(stroke.currentCell.x, stroke.currentCell.y, 0);
            tilemap.SetTile(cell, tile);

            return new BrushExecutorResult
            {
                success = true,
                spawnedCount = 0,
                modifiedAssets = new List<UnityEngine.Object> { tilemap }
            };
        }

        private static Tilemap ResolveTilemap(TargetLayer targetLayer)
        {
            Tilemap[] tilemaps = UnityEngine.Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
            if (tilemaps.Length == 0)
            {
                return null;
            }

            string[] priority = GetNamePriority(targetLayer);
            for (int i = 0; i < priority.Length; i++)
            {
                for (int j = 0; j < tilemaps.Length; j++)
                {
                    Tilemap tilemap = tilemaps[j];
                    if (tilemap != null && tilemap.name.IndexOf(priority[i], StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return tilemap;
                    }
                }
            }

            return tilemaps[0];
        }

        private static string[] GetNamePriority(TargetLayer targetLayer)
        {
            if (targetLayer == TargetLayer.L2)
            {
                return new[] { "L2", "Variation", "Variant" };
            }

            return new[] { "L1", "FloorTilemap", "Floor", "Base", "Ground" };
        }

        private static TileBase PickTile(AssetPoolSO pool, BrushStroke stroke, bool random)
        {
            if (!random)
            {
                return FirstNonNull(pool.tiles);
            }

            int index = PickWeightedIndex(pool.tiles.Count, pool.spriteWeights, stroke.seed, stroke.currentCell.x, stroke.currentCell.y);
            for (int i = 0; i < pool.tiles.Count; i++)
            {
                TileBase candidate = pool.tiles[(index + i) % pool.tiles.Count];
                if (candidate != null)
                {
                    return candidate;
                }
            }

            return null;
        }

        private static TileBase FirstNonNull(List<TileBase> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i] != null)
                {
                    return tiles[i];
                }
            }

            return null;
        }

        private static int PickWeightedIndex(int count, List<float> weights, int seed, int x, int y)
        {
            if (weights == null || weights.Count < count)
            {
                return PositiveModulo(Mix(seed, x, y), count);
            }

            float total = 0f;
            for (int i = 0; i < count; i++)
            {
                total += Mathf.Max(0f, weights[i]);
            }

            if (total <= 0f)
            {
                return PositiveModulo(Mix(seed, x, y), count);
            }

            float pick = Hash01(seed, x, y) * total;
            float cursor = 0f;
            for (int i = 0; i < count; i++)
            {
                cursor += Mathf.Max(0f, weights[i]);
                if (pick <= cursor)
                {
                    return i;
                }
            }

            return count - 1;
        }

        private static float Hash01(int seed, int x, int y)
        {
            return (PositiveModulo(Mix(seed, x, y), 1000000) / 999999f);
        }

        private static int Mix(int seed, int a, int b)
        {
            unchecked
            {
                int hash = seed;
                hash = (hash * 397) ^ a;
                hash = (hash * 397) ^ b;
                hash ^= hash >> 16;
                return hash;
            }
        }

        private static int PositiveModulo(int value, int modulo)
        {
            int result = value % modulo;
            return result < 0 ? result + modulo : result;
        }

        private static BrushExecutorResult Error(string message)
        {
            return new BrushExecutorResult { success = false, errorMessage = message };
        }
    }
}
#endif
