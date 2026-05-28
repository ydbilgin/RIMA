using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Environment
{
    [CreateAssetMenu(fileName = "DeterministicVariantTile", menuName = "RIMA/Environment/Deterministic Variant Tile")]
    public sealed class DeterministicVariantTile : TileBase
    {
        public Sprite baseSprite;
        public Sprite[] variants;
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

            if (variants != null && variants.Length > 0)
            {
                int seed = DeterministicSeed(position);
                tileData.sprite = variants[(seed & 0x7fffffff) % variants.Length];
            }
            else
            {
                tileData.sprite = baseSprite;
            }
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
