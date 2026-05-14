using System.Collections.Generic;
using RIMA.RoomDesigner.Core;
using RIMA.Runtime.Rooms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public static class DecalPainter
    {
        private const float MinDistance = 2.5f;
        private const float NoiseFrequency = 0.17f;
        private const float NoiseThreshold = 0.2f;

        private static readonly Dictionary<Sprite, Tile> TileCache = new Dictionary<Sprite, Tile>();

        public static bool PaintDecals(Tilemap decalTilemap, RoomBlueprint bp, Sprite[] decalSprites, int masterSeed, float density)
        {
            if (decalTilemap == null || bp == null)
            {
                Debug.LogWarning("DecalPainter: decal tilemap or blueprint is missing.");
                return false;
            }

            if (decalSprites == null || decalSprites.Length == 0)
            {
                Debug.LogWarning("DecalPainter: decalSprites is empty.");
                return false;
            }

            if (decalTilemap.GetComponent<TilemapCollider2D>() != null)
            {
                Debug.LogWarning("DecalPainter: DecalsTilemap should not have a TilemapCollider2D.");
            }

            int width = Mathf.Max(0, bp.roomWidth);
            int height = Mathf.Max(0, bp.roomHeight);
            int total = width * height;
            if (total == 0)
            {
                return false;
            }

            density = Mathf.Clamp01(density);
            int subSeed = SeedPipeline.DeriveSubSeed(masterSeed, "decal");
            bp.decalVariantIndex = new byte[total];
            decalTilemap.ClearAllTiles();

            if (density <= 0f)
            {
                decalTilemap.RefreshAllTiles();
                return true;
            }

            List<Vector2Int> points = GeneratePoissonPoints(width, height, subSeed, density);
            float seedOffset = subSeed * 0.013f;

            for (int i = 0; i < points.Count; i++)
            {
                Vector2Int local = points[i];
                int x = bp.roomOrigin.x + local.x;
                int y = bp.roomOrigin.y + local.y;
                float noise = Mathf.PerlinNoise((x + seedOffset) * NoiseFrequency, (y - seedOffset) * NoiseFrequency);
                if (density * noise <= NoiseThreshold)
                {
                    continue;
                }

                uint hash = (uint)((x * 73856093) ^ (y * 19349663) ^ subSeed);
                int spriteIndex = (int)(hash % (uint)decalSprites.Length);
                Sprite sprite = decalSprites[spriteIndex];
                if (sprite == null)
                {
                    continue;
                }

                int arrIdx = local.y * width + local.x;
                if (arrIdx < 0 || arrIdx >= bp.decalVariantIndex.Length)
                {
                    continue;
                }

                decalTilemap.SetTile(new Vector3Int(x, y, 0), GetOrCreateTile(sprite));
                bp.decalVariantIndex[arrIdx] = (byte)Mathf.Clamp(spriteIndex + 1, 1, byte.MaxValue);
            }

            decalTilemap.RefreshAllTiles();
            return true;
        }

        private static List<Vector2Int> GeneratePoissonPoints(int width, int height, int seed, float density)
        {
            var random = new System.Random(seed);
            var points = new List<Vector2Int>();
            int maxAttempts = Mathf.Max(32, width * height * 8);
            int targetCount = Mathf.CeilToInt(width * height * density / (MinDistance * MinDistance));
            float minDistanceSq = MinDistance * MinDistance;

            for (int attempt = 0; attempt < maxAttempts && points.Count < targetCount; attempt++)
            {
                int x = random.Next(0, width);
                int y = random.Next(0, height);
                var candidate = new Vector2Int(x, y);
                bool accepted = true;

                for (int i = 0; i < points.Count; i++)
                {
                    Vector2Int delta = points[i] - candidate;
                    if (delta.sqrMagnitude < minDistanceSq)
                    {
                        accepted = false;
                        break;
                    }
                }

                if (accepted)
                {
                    points.Add(candidate);
                }
            }

            return points;
        }

        private static Tile GetOrCreateTile(Sprite sprite)
        {
            if (TileCache.TryGetValue(sprite, out Tile tile) && tile != null)
            {
                return tile;
            }

            tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = sprite.name + "_DecalTile";
            tile.sprite = sprite;
            TileCache[sprite] = tile;
            return tile;
        }
    }
}
