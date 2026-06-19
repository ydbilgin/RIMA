using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Environment
{
    [CreateAssetMenu(fileName = "DirectionalCliffTile", menuName = "RIMA/Environment/Directional Cliff Tile")]
    public sealed class DirectionalCliffTile : TileBase
    {
        [Header("Sprites (Arrays for cosmetic variation)")]
        public Sprite[] spritesS = new Sprite[0];
        public Sprite[] spritesSE = new Sprite[0];
        public Sprite[] spritesSW = new Sprite[0];
        public Sprite[] spritesE = new Sprite[0];
        public Sprite[] spritesW = new Sprite[0];
        public Sprite[] spritesN = new Sprite[0];
        public Sprite[] spritesNE = new Sprite[0];
        public Sprite[] spritesNW = new Sprite[0];

        [Header("Layout")]
        public Vector3 transformOffset;
        public Vector2 spriteScale = Vector2.one;

        [Header("Organic Variation (design-review consensus)")]
        [Tooltip("Cluster cells into short/mid/full hang tiers via Perlin noise + x jitter. Top contact line stays fixed.")]
        public bool heightVariation = true;
        [Tooltip("Lower = larger clusters (slower noise). ~0.10-0.20 gives 2-5 cell runs.")]
        public float clusterFrequency = 0.14f;
        [Tooltip("Max upward lift (world units) = the SHORTEST hang. Larger = more dramatic long/short spread.")]
        public float maxLift = 1.1f;
        [Range(0f, 1f)]
        [Tooltip("0 = smooth clustered runs, 1 = fully per-cell random long/short mix.")]
        public float randomness = 0.7f;
        [Tooltip("Max +/- horizontal jitter to break grid alignment.")]
        public float gridJitter = 0.10f;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.colliderType = Tile.ColliderType.None;
            tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
            tileData.color = Color.white;
            
            // Design-review consensus: break "wallpaper" repetition with clustered
            // height variation while keeping the TOP contact line stable. We lift some cells
            // UP (further under the floor = shorter visible hang, never a gap below), seeded by
            // low-frequency Perlin noise so neighbours cluster into 2-5 cell runs. A tiny x
            // jitter breaks perfect grid alignment. No sprite stretching (pixel-art safe).
            int seed = DeterministicSeed(position);
            Vector3 hangOffset = transformOffset;
            if (heightVariation)
            {
                // Continuous (non-stepped) lift: blend smooth Perlin (some coherence) with a
                // per-cell hash (randomness) so some cliffs hang much longer, some much shorter
                // — not ordered runs. randomness=0 -> clustered, 1 -> fully random per cell.
                float smooth = Mathf.PerlinNoise(position.x * clusterFrequency, position.y * clusterFrequency);
                float rand = (seed & 0xFFFF) / 65535f;
                float t = Mathf.Lerp(smooth, rand, randomness);
                float liftY = t * maxLift;
                float jitterX = (((seed >> 8) & 0xFF) / 255f - 0.5f) * gridJitter;
                hangOffset += new Vector3(jitterX, liftY, 0f);
            }
            tileData.transform = Matrix4x4.TRS(
                hangOffset,
                Quaternion.identity,
                new Vector3(spriteScale.x, spriteScale.y, 1f));

            // Default fallback
            tileData.sprite = GetRandomVariant(spritesS, seed);

            // Find the placer in the scene to reference the floor map
            CliffAutoPlacer placer = Object.FindObjectOfType<CliffAutoPlacer>();
            if (placer == null || placer.floorTilemap == null)
            {
                return;
            }

            Tilemap floor = placer.floorTilemap;

            // Correct mapping of neighbors in Isometric grid:
            // (1, 1, 0)   = North (N)
            // (-1, -1, 0) = South (S)
            // (1, -1, 0)  = East (E)
            // (-1, 1, 0)  = West (W)
            // (1, 0, 0)   = North-East (NE)
            // (0, 1, 0)   = North-West (NW)
            // (0, -1, 0)  = South-East (SE)
            // (-1, 0, 0)  = South-West (SW)

            bool hasN  = floor.HasTile(position + new Vector3Int(1, 1, 0));
            bool hasS  = floor.HasTile(position + new Vector3Int(-1, -1, 0));
            bool hasE  = floor.HasTile(position + new Vector3Int(1, -1, 0));
            bool hasW  = floor.HasTile(position + new Vector3Int(-1, 1, 0));
            
            bool hasNE = floor.HasTile(position + new Vector3Int(1, 0, 0));
            bool hasNW = floor.HasTile(position + new Vector3Int(0, 1, 0));
            bool hasSE = floor.HasTile(position + new Vector3Int(0, -1, 0));
            bool hasSW = floor.HasTile(position + new Vector3Int(-1, 0, 0));

            // We determine the cliff direction based on where the floor is relative to the cliff cell
            if (hasN)  tileData.sprite = GetRandomVariant(spritesS, seed);
            else if (hasNW) tileData.sprite = GetRandomVariant(spritesSE, seed);
            else if (hasNE) tileData.sprite = GetRandomVariant(spritesSW, seed);
            else if (hasW)  tileData.sprite = GetRandomVariant(spritesE, seed);
            else if (hasE)  tileData.sprite = GetRandomVariant(spritesW, seed);
            else if (hasSW) tileData.sprite = GetRandomVariant(spritesNE, seed);
            else if (hasSE) tileData.sprite = GetRandomVariant(spritesNW, seed);
            else if (hasS)  tileData.sprite = GetRandomVariant(spritesN, seed);
        }

        private Sprite GetRandomVariant(Sprite[] array, int seed)
        {
            if (array == null || array.Length == 0) return null;
            int idx = (seed & 0x7fffffff) % array.Length;
            return array[idx];
        }

        private static int DeterministicSeed(Vector3Int pos)
        {
            unchecked
            {
                int h = 17;
                h = h * 31 + pos.x;
                h = h * 31 + pos.y;
                h = h * 31 + pos.z;
                h ^= h << 13; h ^= h >> 17; h ^= h << 5;
                return h;
            }
        }
    }
}
