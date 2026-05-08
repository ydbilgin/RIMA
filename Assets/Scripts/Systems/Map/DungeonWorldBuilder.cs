using System.Collections.Generic;
using UnityEngine;
using RIMA;

namespace RIMA.Systems.Map
{
    public class DungeonWorldBuilder : MonoBehaviour
    {
        public static DungeonWorldBuilder Instance { get; private set; }

        [SerializeField] private LargeDungeonMapPainterBase painter;
        [SerializeField] private RoomTemplate[] roomTemplates;
        [SerializeField] private DepthBandTileSet[] depthBands;
        [SerializeField] private Vector2Int roomStride = new Vector2Int(240, 170);
        [SerializeField] private int corridorWidth = 8;

        private readonly Dictionary<int, Vector3Int> roomOffsets = new Dictionary<int, Vector3Int>();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void BuildWorld()
        {
            if (DungeonGraph.Instance == null) return;
            var nodes = DungeonGraph.Instance.AllNodes;

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var offset = new Vector3Int(node.lane * roomStride.x, node.depth * roomStride.y, 0);
                roomOffsets[node.id] = offset;

                var template = (roomTemplates != null && i < roomTemplates.Length) ? roomTemplates[i] : null;
                var layout = template != null
                    ? template.layout
                    : LargeDungeonMapPainterBase.LayoutKind.BrokenEntryHall;

                var band = GetBand(node.depth);
                if (band != null && band.floorTiles != null && band.floorTiles.Length > 0)
                    painter.SetTilePool(band.floorTiles, band.wallTiles);

                painter.PaintTemplateAtOffset(layout, offset);
            }
        }

        public BoundsInt GetRoomBounds(int roomIndex)
        {
            if (!roomOffsets.TryGetValue(roomIndex, out var offset))
                return default;
            return new BoundsInt(offset, new Vector3Int(roomStride.x, roomStride.y, 1));
        }

        private DepthBandTileSet GetBand(int depth)
        {
            if (depthBands == null) return null;
            foreach (var band in depthBands)
                if (depth >= band.minDepth && depth <= band.maxDepth) return band;
            return depthBands.Length > 0 ? depthBands[depthBands.Length - 1] : null;
        }
    }
}
