using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Map
{
    [CreateAssetMenu(fileName = "MaterialVariantPool", menuName = "RIMA/Map/Material Pool")]
    public class MaterialVariantPoolSO : ScriptableObject
    {
        [Serializable]
        public class MaterialEntry
        {
            public string materialId;
            public TileBase[] variants;
        }

        public MaterialEntry[] materials;

        public TileBase GetVariant(string materialId, int seed)
        {
            if (materials == null || string.IsNullOrEmpty(materialId)) return null;

            MaterialEntry entry = Array.Find(materials, m => m != null && m.materialId == materialId);
            if (entry == null || entry.variants == null || entry.variants.Length == 0) return null;

            int index = Mathf.Abs(seed) % entry.variants.Length;
            return entry.variants[index];
        }
    }
}
