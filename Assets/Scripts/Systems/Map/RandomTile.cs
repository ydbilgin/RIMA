using UnityEngine;

namespace UnityEngine.Tilemaps
{
    public sealed class RandomTile : TileBase
    {
        public Sprite[] sprites = System.Array.Empty<Sprite>();
        public float[] weights = System.Array.Empty<float>();
        public Tile.ColliderType colliderType = Tile.ColliderType.None;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = SelectSprite(position);
            tileData.colliderType = colliderType;
            tileData.flags = TileFlags.LockTransform;
            tileData.transform = Matrix4x4.identity;
        }

        private Sprite SelectSprite(Vector3Int position)
        {
            if (sprites == null || sprites.Length == 0)
            {
                return null;
            }

            float totalWeight = 0f;
            for (int i = 0; i < sprites.Length; i++)
            {
                float weight = weights != null && i < weights.Length ? Mathf.Max(0f, weights[i]) : 1f;
                totalWeight += weight;
            }

            if (totalWeight <= 0f)
            {
                return sprites[0];
            }

            float pick = Hash01(position) * totalWeight;
            for (int i = 0; i < sprites.Length; i++)
            {
                float weight = weights != null && i < weights.Length ? Mathf.Max(0f, weights[i]) : 1f;
                if (pick <= weight)
                {
                    return sprites[i];
                }

                pick -= weight;
            }

            return sprites[sprites.Length - 1];
        }

        private static float Hash01(Vector3Int position)
        {
            unchecked
            {
                uint hash = (uint)(position.x * 73856093) ^ (uint)(position.y * 19349663) ^ (uint)(position.z * 83492791);
                hash ^= hash >> 13;
                hash *= 1274126177u;
                return (hash & 0x00FFFFFF) / 16777215f;
            }
        }
    }
}
