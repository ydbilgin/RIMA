using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Walls
{
    [CreateAssetMenu(fileName = "WallChunkLibrary", menuName = "RIMA/Walls/Wall Chunk Library")]
    public class WallChunkLibrary : ScriptableObject
    {
        private static readonly WallType[] PlaceholderTypes =
        {
            WallType.Connector_Straight,
            WallType.Connector_OuterCorner,
            WallType.Connector_InnerCorner,
            WallType.Connector_End,
            WallType.Connector_DoorLeft,
            WallType.Connector_DoorRight,
            WallType.Connector_Alcove,
            WallType.Connector_Protrusion,
            WallType.WallSpan_PlainMidA,
            WallType.WallSpan_PlainMidB,
            WallType.WallSpan_PlainMidC,
            WallType.WallSpan_Short,
            WallType.WallSpan_Medium,
            WallType.WallSpan_Long,
            WallType.LowWall_1x,
            WallType.LowWall_2x,
            WallType.OuterCorner_L,
            WallType.OuterCorner_R,
            WallType.InnerCorner_L,
            WallType.InnerCorner_R,
            WallType.DoorArch_2w,
            WallType.DoorArch_3w,
            WallType.Seam_Straight,
            WallType.Seam_OuterCorner,
            WallType.Seam_InnerCorner,
            WallType.Seam_DoorJambL,
            WallType.Seam_DoorJambR,
            WallType.Seam_BaseTrim,
            WallType.Seam_FrontEdgeL,
            WallType.Seam_FrontEdgeR,
            WallType.Seam_FrontCornerL,
            WallType.Seam_FrontCornerR,
            WallType.Seam_PillarWall,
            WallType.Seam_ShadowPatch,
            WallType.Seam_CleanupCap,
            WallType.Seam_GapFiller,
            WallType.Seam_PlainFiller,
            WallType.Seam_MicroOccluder
        };

        private readonly Dictionary<WallType, WallChunkData> placeholderCache = new Dictionary<WallType, WallChunkData>();

        [System.Serializable]
        public struct LibEntry
        {
            public WallChunkData data;
            public GameObject prefab;
        }

        public List<LibEntry> entries = new List<LibEntry>();

        public WallChunkData GetConnectorFor(WallType type)
        {
            return FindByType(type) ?? GetPlaceholder(type);
        }

        public WallChunkData GetConnectorFor(WallChainBuilder.EdgeType edge)
        {
            return GetConnectorFor(WallType.Connector_Straight);
        }

        public WallChunkData GetSpanFor(WallType type)
        {
            return FindByType(type) ?? GetPlaceholder(type);
        }

        public WallChunkData GetSpanFor(WallChainBuilder.EdgeType edge, WallType type)
        {
            string preferred = edge == WallChainBuilder.EdgeType.N ||
                edge == WallChainBuilder.EdgeType.NE ||
                edge == WallChainBuilder.EdgeType.E ||
                edge == WallChainBuilder.EdgeType.SE
                    ? "ne_mid_plain"
                    : "nw_mid_plain";

            WallChunkData fallback = null;
            foreach (LibEntry entry in entries)
            {
                if (entry.data == null)
                {
                    continue;
                }

                if (entry.data.chunkId != null && entry.data.chunkId.Contains(preferred))
                {
                    return entry.data;
                }

                if (fallback == null && entry.data.wallType == type)
                {
                    fallback = entry.data;
                }
            }

            return fallback != null ? fallback : GetSpanFor(type);
        }

        public WallChunkData GetSeamFor(WallChainBuilder.EdgeType prev, WallChainBuilder.EdgeType next)
        {
            int angleClass = WallChainBuilder.GetAngleClass(prev, next);
            if (angleClass == 0)
            {
                return GetConnectorFor(WallType.Seam_Straight);
            }

            if (angleClass == 90)
            {
                return GetConnectorFor(WallType.Seam_OuterCorner);
            }

            if (angleClass == -90)
            {
                return GetConnectorFor(WallType.Seam_InnerCorner);
            }

            return null;
        }

        public WallChunkData GetLowWallFor(int length)
        {
            return GetConnectorFor(length == 1 ? WallType.LowWall_1x : WallType.LowWall_2x);
        }

        public GameObject GetPrefab(WallChunkData data)
        {
            foreach (LibEntry entry in entries)
            {
                if (entry.data == data)
                {
                    return entry.prefab;
                }
            }

            return null;
        }

        private WallChunkData FindByType(WallType type)
        {
            foreach (LibEntry entry in entries)
            {
                if (entry.data != null && entry.data.wallType == type)
                {
                    return entry.data;
                }
            }

            return null;
        }

        private WallChunkData GetPlaceholder(WallType type)
        {
            if (!IsPlaceholderType(type))
            {
                return null;
            }

            if (placeholderCache.TryGetValue(type, out WallChunkData cached))
            {
                return cached;
            }

            WallChunkData data = CreateInstance<WallChunkData>();
            data.chunkId = type.ToString();
            data.wallType = type;
            data.visual = null;
            data.colliderSize = GetDefaultColliderSize(type);
            data.colliderOffset = Vector2.zero;
            placeholderCache[type] = data;
            return data;
        }

        private static bool IsPlaceholderType(WallType type)
        {
            foreach (WallType placeholderType in PlaceholderTypes)
            {
                if (placeholderType == type)
                {
                    return true;
                }
            }

            return false;
        }

        private static Vector2 GetDefaultColliderSize(WallType type)
        {
            return type switch
            {
                WallType.WallSpan_Long => new Vector2(6f, 1f),
                WallType.WallSpan_Medium => new Vector2(4f, 1f),
                WallType.LowWall_2x => new Vector2(4f, 0.5f),
                WallType.LowWall_1x => new Vector2(2f, 0.5f),
                WallType.DoorArch_2w => new Vector2(4f, 0.25f),
                WallType.DoorArch_3w => new Vector2(6f, 0.25f),
                _ => new Vector2(2f, 1f)
            };
        }
    }
}
