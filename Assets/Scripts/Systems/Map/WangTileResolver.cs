using UnityEngine;

namespace RIMA.Systems.Map
{
    // Deterministic seed-based Wang tile resolver (Karar #115 compliance).
    public class WangTileResolver : MonoBehaviour
    {
        [SerializeField] private TileAssetMetadata[] tileLibrary;

        public TileAssetMetadata Resolve(int wangMask, Vector3Int cellPos, int seed)
        {
            if (tileLibrary == null || tileLibrary.Length == 0)
            {
                return null;
            }

            var candidates = System.Array.FindAll(tileLibrary, tile => tile != null && tile.wangMask == wangMask);
            if (candidates.Length == 0)
            {
                return null;
            }

            if (candidates.Length == 1)
            {
                return candidates[0];
            }

            int hash = seed ^ (cellPos.x * 73856093) ^ (cellPos.y * 19349663);
            float totalWeight = 0f;
            foreach (TileAssetMetadata candidate in candidates)
            {
                totalWeight += Mathf.Max(0f, candidate.weight);
            }

            if (totalWeight <= 0f)
            {
                return candidates[0];
            }

            float rand = (Mathf.Abs(hash % 10000) / 10000f) * totalWeight;
            float cumulative = 0f;
            foreach (TileAssetMetadata candidate in candidates)
            {
                cumulative += Mathf.Max(0f, candidate.weight);
                if (rand <= cumulative)
                {
                    return candidate;
                }
            }

            return candidates[candidates.Length - 1];
        }

        public static int ComputeWangMask(bool north, bool east, bool south, bool west)
        {
            return (north ? 1 : 0) | (east ? 2 : 0) | (south ? 4 : 0) | (west ? 8 : 0);
        }
    }
}
