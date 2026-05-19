using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Systems.Map
{
    using RoomType = global::RIMA.RoomType;

    public class RoomConfig : MonoBehaviour
    {
        [Header("Identity")]
        public string roomId;
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
        public Vector3 cellSize = new Vector3(0.94f, 0.94f, 1f);
        public GridLayout.CellLayout gridLayout = GridLayout.CellLayout.Isometric;
        public GridLayout.CellSwizzle orientation = GridLayout.CellSwizzle.XYZ;
    }
}
