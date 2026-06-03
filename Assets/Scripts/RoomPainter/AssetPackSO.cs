using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.RoomPainter
{
    [CreateAssetMenu(fileName = "AssetPack", menuName = "RIMA/Room Painter/Asset Pack")]
    public sealed class AssetPackSO : ScriptableObject
    {
        public string packId;
        public string displayName;
        public string version;
        public List<Entry> entries = new List<Entry>();

        [System.Serializable]
        public sealed class Entry
        {
            public string guid;
            public Sprite sprite;
            public TileBase tile;
            public GameObject prefab;
            public DesignerCategory category = DesignerCategory.Object;
            public string registryTag;
            public string layer;
            public int sortingOverride;
        }
    }
}
