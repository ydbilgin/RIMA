using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Walls
{
    public enum WallType
    {
        Connector_Straight,
        Connector_CornerOuter,
        Connector_CornerInner,
        Connector_End,
        Connector_DoorLeft,
        Connector_DoorRight,
        Connector_OuterCorner,
        Connector_InnerCorner,
        Connector_Alcove,
        Connector_Protrusion,
        WallSpan_PlainMidA,
        WallSpan_PlainMidB,
        WallSpan_PlainMidC,
        WallSpan_Short,
        WallSpan_Medium,
        WallSpan_Long,
        LowWall_1x,
        LowWall_2x,
        OuterCorner_L,
        OuterCorner_R,
        InnerCorner_L,
        InnerCorner_R,
        DoorArch_2w,
        DoorArch_3w,
        OpenGap,
        ShortStop,
        Seam_Straight,
        Seam_OuterCorner,
        Seam_InnerCorner,
        Seam_DoorJambL,
        Seam_DoorJambR,
        Seam_BaseTrim,
        Seam_FrontEdgeL,
        Seam_FrontEdgeR,
        Seam_FrontCornerL,
        Seam_FrontCornerR,
        Seam_PillarWall,
        Seam_ShadowPatch,
        Seam_CleanupCap,
        Seam_GapFiller,
        Seam_PlainFiller,
        Seam_MicroOccluder,
        Landmark,
        Pillar,
        SeamOverlay
    }

    public enum WallHeight
    {
        Normal,
        Tall,
        Base,
        Mid,
        Cap
    }

    [System.Serializable]
    public struct SocketDef
    {
        public string socketName;
        public Vector2 localPosition;
    }

    [CreateAssetMenu(fileName = "WallChunkData", menuName = "RIMA/Walls/Wall Chunk Data")]
    public class WallChunkData : ScriptableObject
    {
        [Header("Identity")]
        public string chunkId;
        public WallType wallType;
        public WallHeight heightVariant = WallHeight.Normal;
        public Sprite visual;

        [Header("Footprint (iso cell occupancy)")]
        [Tooltip("List of iso cells occupied. (0,0) = anchor cell. (1,0) = east of anchor.")]
        public List<Vector2Int> footprintCells = new List<Vector2Int> { Vector2Int.zero };

        [Header("Anchor")]
        [Tooltip("Local offset from cell center to sprite pivot (anchored at footprint base).")]
        public Vector2 anchorOffset = Vector2.zero;

        [Header("Sockets")]
        public List<SocketDef> sockets = new List<SocketDef>();

        [Header("Collision")]
        public Vector2 colliderSize = new Vector2(2f, 1f);
        public Vector2 colliderOffset = Vector2.zero;
    }
}
