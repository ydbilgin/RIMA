using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Rooms
{
    [CreateAssetMenu(fileName = "RoomFootprint", menuName = "RIMA/Rooms/Room Footprint Polygon")]
    public class RoomFootprintPolygon : ScriptableObject
    {
        [Header("Polygon vertices (iso grid coordinates)")]
        [Tooltip("Vertex order: clockwise from top. Edges between consecutive verts form wall chain.")]
        public List<Vector2Int> vertices = new List<Vector2Int>();

        [Header("Open edges (no walls, front-facing parapet)")]
        public List<int> openEdgeIndices = new List<int>();

        [Header("Door and low wall edges")]
        public List<int> doorEdgeIndices = new List<int>();
        public List<int> lowEdgeIndices = new List<int>();

        [Header("Entry points")]
        public List<Vector2Int> entryPoints = new List<Vector2Int>();
    }
}
