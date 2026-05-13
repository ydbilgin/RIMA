using UnityEngine;

namespace RIMA.Runtime.Rooms
{
    [CreateAssetMenu(menuName = "RIMA/Room Blueprint")]
    public class RoomBlueprint : ScriptableObject
    {
        [Header("Identity")]
        public string roomId;
        public string roomType;
        public BiomeType biomeType;
        public int gateCount;

        [System.Obsolete("Use biomeType instead")]
        public string biome;

        [Header("Dimensions")]
        public int roomWidth = 20;
        public int roomHeight = 20;
        public Vector3Int roomOrigin;

        [Header("Procedural Seed")]
        public int noiseSeed;

        [Header("Baked Variant Data")]
        // floorVariantIndex[cell] = tile variant index (0-15), length = roomWidth * roomHeight
        // Populated by FloorVariantPainter on save. Empty = not yet baked.
        public byte[] floorVariantIndex;
        // wallVariantIndex[cell] = connection variant index (0-7), length = roomWidth * roomHeight
        // Populated by WallAutoConnector on save. Empty = not yet baked.
        public byte[] wallVariantIndex;
        // decalVariantIndex[cell] = decal variant index + 1, 0 means no decal.
        public byte[] decalVariantIndex;
        // overrideVariantIndex[cell] = true -> manual override, do not re-bake this cell
        public bool[] overrideVariantIndex;

        [Header("Prefab")]
        public GameObject prefab;
    }
}
