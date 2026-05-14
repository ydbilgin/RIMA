using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    public class CornerWangTileSetSO : ScriptableObject
    {
        [Header("Terrain Labels")]
        public string lowerTerrainLabel = "lower (floor)";
        public string upperTerrainLabel = "upper (wall)";

        public TileBase[] tiles = new TileBase[16];

        [System.Serializable]
        public class WangVariants
        {
            public TileBase[] variants = new TileBase[1];
        }

        public WangVariants[] variantsByKey = new WangVariants[16];

        public TileBase GetTile(int nw, int ne, int sw, int se, int hashSeed = 0)
        {
            int key = ((nw != 0 ? 1 : 0) << 3)
                | ((ne != 0 ? 1 : 0) << 2)
                | ((sw != 0 ? 1 : 0) << 1)
                | (se != 0 ? 1 : 0);

            return GetTileForKey(key, hashSeed);
        }

        public TileBase GetTileForKey(int key, int hashSeed = 0)
        {
            if (key < 0 || key >= 16)
            {
                return null;
            }

            TileBase legacyTile = tiles != null && key < tiles.Length ? tiles[key] : null;
            if (variantsByKey == null || variantsByKey.Length != 16 || variantsByKey[key] == null)
            {
                return legacyTile;
            }

            TileBase[] variants = variantsByKey[key].variants;
            if (variants == null || variants.Length == 0)
            {
                return legacyTile;
            }

            if (variants.Length == 1)
            {
                return variants[0] != null ? variants[0] : legacyTile;
            }

            int index = (int)((uint)hashSeed % (uint)variants.Length);
            if (variants[index] != null)
            {
                return variants[index];
            }

            for (int i = 0; i < variants.Length; i++)
            {
                if (variants[i] != null)
                {
                    return variants[i];
                }
            }

            return legacyTile;
        }

        [ContextMenu("Sync Variants From Legacy Tiles")]
        public void SyncFromLegacy()
        {
            if (variantsByKey == null || variantsByKey.Length != 16)
            {
                variantsByKey = new WangVariants[16];
            }

            for (int i = 0; i < 16; i++)
            {
                if (variantsByKey[i] == null)
                {
                    variantsByKey[i] = new WangVariants();
                }

                if (variantsByKey[i].variants == null || variantsByKey[i].variants.Length == 0)
                {
                    variantsByKey[i].variants = new[]
                    {
                        tiles != null && i < tiles.Length ? tiles[i] : null
                    };
                }
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
