namespace RIMA.Editor.RoomDesigner
{
    using System.Collections.Generic;
    using RIMA.Editor.RoomDesigner.Brushes;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public static class RoomDesignerWallPolish
    {
        private static readonly Vector3Int Up = new Vector3Int(0, 1, 0);
        private static readonly Vector3Int Down = new Vector3Int(0, -1, 0);
        private const string DropShadowTilePath = "Assets/Art/VFX/DropShadow_Wall.asset";

        public static List<CellEdit> ExpandWallEdits(IRoomDesignerContext ctx, IList<CellEdit> edits)
        {
            var expanded = new List<CellEdit>(edits);
            if (ctx?.WallsTilemap == null)
            {
                return expanded;
            }

            var wallWrites = new Dictionary<Vector3Int, TileBase>();
            foreach (CellEdit edit in edits)
            {
                if (edit.Target == ctx.WallsTilemap)
                {
                    wallWrites[edit.Cell] = edit.Tile;
                }
            }

            if (wallWrites.Count == 0)
            {
                return expanded;
            }

            var derived = new Dictionary<(Tilemap target, Vector3Int cell), TileBase>();
            foreach (KeyValuePair<Vector3Int, TileBase> write in wallWrites)
            {
                Vector3Int cell = write.Key;
                bool hasWallAfter = write.Value != null;

                if (ctx.WallTopTilemap != null)
                {
                    Vector3Int topCell = cell + Up;
                    if (hasWallAfter)
                    {
                        derived[(ctx.WallTopTilemap, cell)] = null;
                        derived[(ctx.WallTopTilemap, topCell)] = IsWallOccupiedAfter(ctx, topCell, wallWrites)
                            ? null
                            : ctx.ActiveTile ?? write.Value;
                    }
                    else
                    {
                        derived[(ctx.WallTopTilemap, topCell)] = null;
                        Vector3Int belowCell = cell + Down;
                        derived[(ctx.WallTopTilemap, cell)] = IsWallOccupiedAfter(ctx, belowCell, wallWrites)
                            ? ctx.ActiveTile ?? ctx.WallsTilemap.GetTile(belowCell)
                            : null;
                    }
                }

                TileBase dropShadowTile = LoadDropShadowTile();
                if (ctx.DecalsTilemap != null && dropShadowTile != null)
                {
                    Vector3Int shadowCell = cell + Down;
                    derived[(ctx.DecalsTilemap, shadowCell)] = hasWallAfter && ctx.FloorTilemap != null && ctx.FloorTilemap.GetTile(shadowCell) != null
                        ? dropShadowTile
                        : null;
                }
            }

            foreach (KeyValuePair<(Tilemap target, Vector3Int cell), TileBase> edit in derived)
            {
                expanded.Add(new CellEdit(edit.Key.target, edit.Key.cell, edit.Value));
            }

            return expanded;
        }

        public static void RefreshAll(Tilemap wallFront, Tilemap wallTop, Tilemap floor, Tilemap decals, TileBase wallTopTile, TileBase dropShadowTile)
        {
            if (wallFront == null)
            {
                return;
            }

            wallTop?.ClearAllTiles();
            BoundsInt bounds = wallFront.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    var cell = new Vector3Int(x, y, 0);
                    TileBase frontTile = wallFront.GetTile(cell);
                    if (frontTile == null)
                    {
                        continue;
                    }

                    Vector3Int topCell = cell + Up;
                    if (wallTop != null && wallFront.GetTile(topCell) == null)
                    {
                        wallTop.SetTile(topCell, wallTopTile != null ? wallTopTile : frontTile);
                    }

                    Vector3Int shadowCell = cell + Down;
                    if (decals != null && dropShadowTile != null && floor != null && floor.GetTile(shadowCell) != null)
                    {
                        decals.SetTile(shadowCell, dropShadowTile);
                    }
                }
            }

            wallTop?.RefreshAllTiles();
            decals?.RefreshAllTiles();
        }

        private static bool IsWallOccupiedAfter(IRoomDesignerContext ctx, Vector3Int cell, Dictionary<Vector3Int, TileBase> wallWrites)
        {
            return wallWrites.TryGetValue(cell, out TileBase pending)
                ? pending != null
                : ctx.WallsTilemap.GetTile(cell) != null;
        }

        private static TileBase LoadDropShadowTile()
        {
            return AssetDatabase.LoadAssetAtPath<TileBase>(DropShadowTilePath);
        }
    }
}
