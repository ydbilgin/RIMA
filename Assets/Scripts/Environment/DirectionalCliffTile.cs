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

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.colliderType = Tile.ColliderType.None;
            tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
            tileData.color = Color.white;
            
            tileData.transform = Matrix4x4.TRS(
                transformOffset,
                Quaternion.identity,
                new Vector3(spriteScale.x, spriteScale.y, 1f));

            // Default fallback
            tileData.sprite = GetRandomVariant(spritesS, 0);

#if UNITY_EDITOR
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

            int seed = DeterministicSeed(position);

            // We determine the cliff direction based on where the floor is relative to the cliff cell
            if (hasN)  tileData.sprite = GetRandomVariant(spritesS, seed);
            else if (hasNW) tileData.sprite = GetRandomVariant(spritesSE, seed);
            else if (hasNE) tileData.sprite = GetRandomVariant(spritesSW, seed);
            else if (hasW)  tileData.sprite = GetRandomVariant(spritesE, seed);
            else if (hasE)  tileData.sprite = GetRandomVariant(spritesW, seed);
            else if (hasSW) tileData.sprite = GetRandomVariant(spritesNE, seed);
            else if (hasSE) tileData.sprite = GetRandomVariant(spritesNW, seed);
            else if (hasS)  tileData.sprite = GetRandomVariant(spritesN, seed);
#endif
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
