#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using RIMA.MapDesigner.Brush.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Import.Editor
{
    public static class WangSliceGenerator
    {
        [Serializable] private class WangMetadata { public TilesetData tileset_data; }
        [Serializable] private class TilesetData { public WangTile[] tiles; }
        [Serializable] private class WangTile { public string id; public string name; public WangCorners corners; public WangBox bounding_box; }
        [Serializable] private class WangCorners { public string NE; public string NW; public string SE; public string SW; }
        [Serializable] private class WangBox { public int x; public int y; public int width; public int height; }

        public static List<SliceCell> GenerateCells(Texture2D texture, SliceLayoutTemplateSO template, string metadataJsonPath = null)
        {
            var cells = new List<SliceCell>();
            if (texture == null)
            {
                return cells;
            }

            WangMetadata meta = TryLoadMetadata(metadataJsonPath);
            WangTile[] tiles = meta != null && meta.tileset_data != null ? meta.tileset_data.tiles : null;
            if (tiles != null && tiles.Length > 0)
            {
                for (int i = 0; i < tiles.Length; i++)
                {
                    var tile = tiles[i];
                    if (tile == null || tile.corners == null || tile.bounding_box == null)
                    {
                        continue;
                    }

                    string tag = WangTagFromCorners(tile.corners);
                    bool isAllFloor = tag == "wang_0000";
                    string uniqueSuffix = !string.IsNullOrEmpty(tile.name)
                        ? "_" + tile.name
                        : $"_x{tile.bounding_box.x}_y{tile.bounding_box.y}_i{i}";
                    string cellName = isAllFloor ? $"all_floor_reference{uniqueSuffix}" : (tag + uniqueSuffix);
                    cells.Add(new SliceCell
                    {
                        cellName = cellName,
                        rect = new RectInt(tile.bounding_box.x, tile.bounding_box.y, tile.bounding_box.width, tile.bounding_box.height),
                        bucket = SizeBucket.Micro,
                        tags = isAllFloor
                            ? new[] { tag, "all_floor_reference" }
                            : new[] { tag, "wang_wall" },
                        usePivotOverride = false,
                        heroAllowed = false
                    });
                }
                return cells;
            }

            int tileSize = 32;
            int cols = Mathf.Max(1, texture.width / tileSize);
            int rows = Mathf.Max(1, texture.height / tileSize);
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    string name = $"cell_{x}_{y}";
                    cells.Add(new SliceCell
                    {
                        cellName = name,
                        rect = new RectInt(x * tileSize, y * tileSize, tileSize, tileSize),
                        bucket = SizeBucket.Micro,
                        tags = new[] { name },
                        usePivotOverride = false,
                        heroAllowed = false
                    });
                }
            }
            return cells;
        }

        private static string WangTagFromCorners(WangCorners c)
        {
            int ne = c.NE == "upper" ? 1 : 0;
            int nw = c.NW == "upper" ? 1 : 0;
            int se = c.SE == "upper" ? 1 : 0;
            int sw = c.SW == "upper" ? 1 : 0;
            return $"wang_{ne}{nw}{se}{sw}";
        }

        private static WangMetadata TryLoadMetadata(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }

            try
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<WangMetadata>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}
#endif
