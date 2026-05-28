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
        public List<RoomLayerData> layers = new List<RoomLayerData>(DefaultLayerSlots.Length);
        public List<PlacementRecord> placements = new List<PlacementRecord>();

        private void Reset()
        {
            EnsureLayerSlotCount();
        }

        private void OnValidate()
        {
            EnsureLayerSlotCount();
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
        public struct PlacementRecord
        {
            public RoomPainterAsset asset;
            public Vector3 worldPos;
            public RoomLayer layer;
            public int orderOverride;
            public Vector2 scaleOverride;
        }
    }
}
