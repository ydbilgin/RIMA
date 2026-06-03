using System.Collections.Generic;
using UnityEngine;

namespace RIMA.RoomPainter
{
    [CreateAssetMenu(fileName = "RoomData", menuName = "RIMA/Room Painter/Room Data")]
    public sealed class RoomData : ScriptableObject
    {
        private static readonly RoomLayer[] DefaultLayerSlots =
        {
            RoomLayer.Floor,
            RoomLayer.Edge,
            RoomLayer.Cliff,
            RoomLayer.Wall,
            RoomLayer.Props,
            RoomLayer.Decals,
            RoomLayer.Lighting,
            RoomLayer.Collision,
            RoomLayer.Occlusion,
            RoomLayer.Parallax
        };

        public string roomId;
        public string displayName;
        public string thumbnailPath;
        public List<RoomLayerData> layers = new List<RoomLayerData>(DefaultLayerSlots.Length);

        public List<TileCellRecord> floorCells = new List<TileCellRecord>();
        public List<TileCellRecord> cliffCells = new List<TileCellRecord>();
        public List<WallSegment> wallSegments = new List<WallSegment>();
        public List<WallCell> wallCells = new List<WallCell>();
        public List<PropPlacement> propPlacements = new List<PropPlacement>();
        public List<PortalPlacement> portalPlacements = new List<PortalPlacement>();

        private void Reset()
        {
            EnsureDefaults();
        }

        private void OnValidate()
        {
            EnsureDefaults();
        }

        public void EnsureDefaults()
        {
            EnsureIdentity();
            EnsureLists();
            EnsureLayerSlotCount();
        }

        private void EnsureIdentity()
        {
            if (string.IsNullOrEmpty(roomId))
            {
                roomId = System.Guid.NewGuid().ToString("N");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = string.IsNullOrEmpty(name) ? "Room" : name;
            }
        }

        private void EnsureLists()
        {
            if (floorCells == null)
            {
                floorCells = new List<TileCellRecord>();
            }

            if (cliffCells == null)
            {
                cliffCells = new List<TileCellRecord>();
            }

            if (wallSegments == null)
            {
                wallSegments = new List<WallSegment>();
            }

            if (wallCells == null)
            {
                wallCells = new List<WallCell>();
            }

            if (propPlacements == null)
            {
                propPlacements = new List<PropPlacement>();
            }

            if (portalPlacements == null)
            {
                portalPlacements = new List<PortalPlacement>();
            }
        }

        private void EnsureLayerSlotCount()
        {
            if (layers == null)
            {
                layers = new List<RoomLayerData>(DefaultLayerSlots.Length);
            }

            while (layers.Count < DefaultLayerSlots.Length)
            {
                layers.Add(null);
            }

            if (layers.Count > DefaultLayerSlots.Length)
            {
                layers.RemoveRange(DefaultLayerSlots.Length, layers.Count - DefaultLayerSlots.Length);
            }
        }

        [System.Serializable]
        public struct TileCellRecord
        {
            public string assetGuidOrName;
            public string sourceGroupId;
            public Vector3Int cell;
            public Vector3 worldPos;
            public float rotation;
            public Vector2 scale;
        }

        [System.Serializable]
        public struct PropPlacement
        {
            public string assetGuidOrName;
            public Vector3Int cell;
            public Vector3 position;
            public float rotation;
            public Vector2 scale;
            public RoomLayer layer;
        }

        /// <summary>
        /// A placed portal/exit. Distinct from PropPlacement because a portal carries
        /// GRAPH metadata (which branch it leads to + its room type) that a plain prop has no
        /// slot for. The visual is still an asset (assetGuidOrName); the graph fields drive
        /// the portal/preview system (see PORTAL_PREVIEW_SYSTEM_SPEC_S6).
        /// </summary>
        [System.Serializable]
        public struct PortalPlacement
        {
            public string assetGuidOrName;
            public Vector3Int cell;
            public Vector3 position;
            public float rotation;
            public Vector2 scale;
            public int exitIndex;        // slot order among this room's exits
            public int targetNodeId;     // DungeonGraph node this portal leads to (-1 = unassigned)
            public string roomTypeId;    // destination room-type tag (combat/elite/treasure/...)
        }

    }

    [System.Serializable]
    public struct WallCell
    {
        public Vector3Int cell;
        public SegmentKind kind;
        public WangShape shape;
        public float rotation;
        public string pieceId;
        public float height;
    }
}
