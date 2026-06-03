using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Systems.Map
{
    using RoomType = global::RIMA.RoomType;

    public class RoomConfig : MonoBehaviour
    {
        public static readonly Vector3 IsoCellSize = new Vector3(0.96f, 0.585f, 1f);
        public const GridLayout.CellLayout IsoGridLayout = GridLayout.CellLayout.Isometric;
        public const GridLayout.CellSwizzle IsoOrientation = GridLayout.CellSwizzle.XYZ;

        [Header("Identity")]
        public string roomId;

        [Tooltip("Canonical shared RoomData asset. When set, the F2 map-paint overlay and " +
            "the Editor Room Painter edit THIS asset (same roomId / same JSON sidecar) so the " +
            "two surfaces never diverge. Leave null only for rooms not yet authored with the map tool.")]
        public global::RIMA.RoomPainter.RoomData roomData;

        public RoomType roomType;
        public int depthBandMin;
        public int depthBandMax;

        [Header("Anchors")]
        public Transform[] spawnPoints;
        public Transform[] entryAnchors;
        public Transform[] exitAnchors;
        public Transform[] doorAnchors;
        public Transform[] pickupAnchors;

        [Header("Grid Contract")]
        public Vector3 cellSize = IsoCellSize;
        public GridLayout.CellLayout gridLayout = IsoGridLayout;
        public GridLayout.CellSwizzle orientation = IsoOrientation;
    }
}
