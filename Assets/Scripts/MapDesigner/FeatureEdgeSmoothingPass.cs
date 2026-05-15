using System;
using System.Collections.Generic;
using RIMA.Data;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner
{
    public sealed class FeatureEdgeSmoothingPass : MonoBehaviour
    {
        private const string OverlayRootName = "FeatureEdgeOverlayLayer";

        [Serializable]
        public struct PaintResult
        {
            public int boundaryCellCount;
            public int wangTilePlacements;
            public int overlayPlacements;
        }

        public PaintResult Paint(Tilemap tilemap, RimaBiomePreset biome, RoomData room, FeatureEdgeSmoothingProfileSO profile, int seed)
        {
            return PaintFeatureEdges(transform, tilemap, biome, room, profile, seed);
        }

        public static PaintResult PaintFeatureEdges(Transform host, Tilemap tilemap, RimaBiomePreset biome, RoomData room, FeatureEdgeSmoothingProfileSO profile, int seed)
        {
            PaintResult result = default;
            if (!NaturalFeatureGraph.HasFeatureData(room.naturalFeatures) || room.walkable == null)
            {
                return result;
            }

            List<Vector2Int> boundaryCells = BuildBoundaryCells(room);
            result.boundaryCellCount = boundaryCells.Count;
            FeatureEdgeSmoothingMode mode = profile != null ? profile.smoothingMode : FeatureEdgeSmoothingMode.WangAndSprite;

            if (mode == FeatureEdgeSmoothingMode.WangAndSprite && tilemap != null && biome != null)
            {
                for (int i = 0; i < boundaryCells.Count; i++)
                {
                    Vector2Int cell = boundaryCells[i];
                    TileBase tile = ResolveBoundaryTile(biome, room, cell, seed);
                    if (tile == null)
                    {
                        continue;
                    }

                    tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), tile);
                    result.wangTilePlacements++;
                }

                tilemap.RefreshAllTiles();
            }

            if (profile != null && profile.overlaySpriteSet != null && profile.overlaySpriteSet.Count > 0)
            {
                Transform root = EnsureRoot(host, OverlayRootName);
                ClearChildren(root);
                int boundaryWidth = Mathf.Max(1, profile.boundaryWidth);
                for (int i = 0; i < boundaryCells.Count; i++)
                {
                    Vector2Int cell = boundaryCells[i];
                    if (DistanceToFeatureOutside(room.naturalFeatures.featureMask, cell) > boundaryWidth)
                    {
                        continue;
                    }

                    Sprite sprite = PickSprite(profile.overlaySpriteSet, seed, cell, i);
                    if (sprite == null)
                    {
                        continue;
                    }

                    CreateOverlay(root, tilemap, cell, sprite, seed, i);
                    result.overlayPlacements++;
                }
            }

            return result;
        }

        public static List<Vector2Int> BuildBoundaryCells(RoomData room)
        {
            var cells = new List<Vector2Int>();
            if (!NaturalFeatureGraph.HasFeatureData(room.naturalFeatures) || room.walkable == null)
            {
                return cells;
            }

            bool[,] mask = room.naturalFeatures.featureMask;
            int width = Mathf.Min(mask.GetLength(0), room.walkable.GetLength(0));
            int height = Mathf.Min(mask.GetLength(1), room.walkable.GetLength(1));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (!room.walkable[x, y] || !mask[x, y])
                    {
                        continue;
                    }

                    if (!IsFeature(mask, x + 1, y) || !IsFeature(mask, x - 1, y) || !IsFeature(mask, x, y + 1) || !IsFeature(mask, x, y - 1))
                    {
                        cells.Add(new Vector2Int(x, y));
                    }
                }
            }

            return cells;
        }

        private static TileBase ResolveBoundaryTile(RimaBiomePreset biome, RoomData room, Vector2Int cell, int seed)
        {
            FeatureType featureType = FeatureTypeForCell(room.naturalFeatures, cell);
            int featureTerrain = NaturalFeatureGraph.TerrainIdForFeature(featureType);
            int floorTerrain = ResolveFloorTerrain(room, cell);

            int nw = IsFeature(room.naturalFeatures.featureMask, cell.x, cell.y + 1) ? featureTerrain : floorTerrain;
            int ne = IsFeature(room.naturalFeatures.featureMask, cell.x + 1, cell.y + 1) ? featureTerrain : floorTerrain;
            int sw = IsFeature(room.naturalFeatures.featureMask, cell.x, cell.y) ? featureTerrain : floorTerrain;
            int se = IsFeature(room.naturalFeatures.featureMask, cell.x + 1, cell.y) ? featureTerrain : floorTerrain;

            return CornerWangPainter.ResolveTile(biome, nw, ne, sw, se, cell.x, cell.y, seed, true);
        }

        private static FeatureType FeatureTypeForCell(NaturalFeatureGraphResult features, Vector2Int cell)
        {
            if (!NaturalFeatureGraph.HasFeatureData(features) ||
                cell.x < 0 ||
                cell.y < 0 ||
                cell.x >= features.siteIndex.GetLength(0) ||
                cell.y >= features.siteIndex.GetLength(1))
            {
                return FeatureType.None;
            }

            int site = features.siteIndex[cell.x, cell.y];
            return site >= 0 && site < features.siteTypes.Length ? features.siteTypes[site] : FeatureType.None;
        }

        private static int ResolveFloorTerrain(RoomData room, Vector2Int cell)
        {
            if (room.terrainGrid != null &&
                cell.x >= 0 &&
                cell.y >= 0 &&
                cell.x < room.terrainGrid.GetLength(0) &&
                cell.y < room.terrainGrid.GetLength(1) &&
                room.terrainGrid[cell.x, cell.y] > 0)
            {
                return room.terrainGrid[cell.x, cell.y];
            }

            return NaturalFeatureGraph.FloorTerrainId;
        }

        private static int DistanceToFeatureOutside(bool[,] mask, Vector2Int cell)
        {
            if (!IsFeature(mask, cell.x, cell.y))
            {
                return 0;
            }

            return (!IsFeature(mask, cell.x + 1, cell.y) ||
                !IsFeature(mask, cell.x - 1, cell.y) ||
                !IsFeature(mask, cell.x, cell.y + 1) ||
                !IsFeature(mask, cell.x, cell.y - 1)) ? 1 : int.MaxValue;
        }

        private static bool IsFeature(bool[,] mask, int x, int y)
        {
            return mask != null &&
                x >= 0 &&
                y >= 0 &&
                x < mask.GetLength(0) &&
                y < mask.GetLength(1) &&
                mask[x, y];
        }

        private static Sprite PickSprite(List<Sprite> sprites, int seed, Vector2Int cell, int salt)
        {
            if (sprites == null || sprites.Count == 0)
            {
                return null;
            }

            int start = Mathf.FloorToInt(NaturalFeatureGraph.Hash01(seed, cell.x, cell.y, salt) * sprites.Count) % sprites.Count;
            for (int i = 0; i < sprites.Count; i++)
            {
                Sprite sprite = sprites[(start + i) % sprites.Count];
                if (sprite != null)
                {
                    return sprite;
                }
            }

            return null;
        }

        private static void CreateOverlay(Transform root, Tilemap tilemap, Vector2Int cell, Sprite sprite, int seed, int index)
        {
            GameObject overlay = new GameObject("FeatureEdgeOverlay_" + index.ToString("0000"));
            overlay.transform.SetParent(root, false);
            overlay.transform.position = CellToWorld(tilemap, new Vector2(cell.x + 0.5f, cell.y + 0.5f));
            overlay.transform.rotation = Quaternion.Euler(0f, 0f, (NaturalFeatureGraph.Hash01(seed + 43, cell.x, cell.y, index) * 2f - 1f) * 10f);

            SpriteRenderer renderer = overlay.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Patch";
            renderer.sortingOrder = 2;
        }

        private static Transform EnsureRoot(Transform host, string rootName)
        {
            if (host == null)
            {
                return null;
            }

            Transform existing = host.Find(rootName);
            if (existing != null)
            {
                return existing;
            }

            GameObject root = new GameObject(rootName);
            root.transform.SetParent(host, false);
            return root.transform;
        }

        private static Vector3 CellToWorld(Tilemap tilemap, Vector2 cell)
        {
            if (tilemap == null)
            {
                return new Vector3(cell.x, cell.y, 0f);
            }

            Vector3 origin = tilemap.CellToWorld(Vector3Int.zero);
            Vector3 unitX = tilemap.CellToWorld(Vector3Int.right) - origin;
            Vector3 unitY = tilemap.CellToWorld(Vector3Int.up) - origin;
            return origin + unitX * cell.x + unitY * cell.y;
        }

        private static void ClearChildren(Transform root)
        {
            if (root == null)
            {
                return;
            }

            for (int i = root.childCount - 1; i >= 0; i--)
            {
                GameObject child = root.GetChild(i).gameObject;
                if (Application.isPlaying)
                {
                    Destroy(child);
                }
                else
                {
                    DestroyImmediate(child);
                }
            }
        }
    }
}
