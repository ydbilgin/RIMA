using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Walls.V2
{
    public enum SocketType
    {
        Torch,
        Banner,
        Bookshelf,
        Sarcophagus,
        Altar,
        Crystal,
        Cage,
        EnemyMelee,
        EnemyRanged,
        EnemyElite,
        EnemyBoss,
        EnemyWave,
        ObjectiveDoor,
        ObjectiveExit,
        ObjectiveChest,
        ObjectiveTrigger,
        ObjectiveRitual,
        ObjectivePortal
    }

    [Serializable]
    public struct RoomSocket
    {
        public Vector2Int cell;
        public SocketType type;
        public string metadata;
    }

    [Serializable]
    public class RoomSpec
    {
        public string roomName = "Room";
        public int widthCells = 8;
        public int heightCells = 6;
        public RoomShapeType shapeType = RoomShapeType.Rectangle;
        public bool rearWallEnabled = true;
        public bool sideWallsEnabled = true;
        public FrontEdgeMode frontEdgeMode = FrontEdgeMode.LowWall;
        public Vector2Int doorPosition = new Vector2Int(-1, -1); // (-1,-1) = no door
        public List<Vector2Int> alcovePositions = new List<Vector2Int>();
        public List<Vector2Int> protrusionPositions = new List<Vector2Int>();

        [Header("Layout Constraints")]
        public int interiorMarginCells = 1;
        public int rearMinWidthCells = 3;
        public int frontMinOpeningCells = 3;
        public bool enforceCenteredRearDoor = true;

        [Header("Diamond Shape")]
        public int diamondTopWidthCells = 6;
        public int diamondStepMin = 1;
        public int diamondStepMax = 2;

        [Header("Rhythm")]
        public int connectorSpacingMin = 2;
        public int connectorSpacingMax = 3;

        [Header("Reserved Zones")]
        public int reservedCenterRadiusCells = 0;
        public List<RectInt> interiorIslandRects = new List<RectInt>();
        public List<RectInt> waterPoolRects = new List<RectInt>();
        public List<RoomSocket> sockets = new List<RoomSocket>();

        [Header("Niche/Protrusion Specs (full formula)")]
        public List<NicheSpec> nicheSpecs = new List<NicheSpec>();
        public List<ProtrusionSpec> protrusionSpecs = new List<ProtrusionSpec>();

        [Header("Preset")]
        public string roomPresetId = "";

        // Optional explicit walkable footprint (Irregular only).
        // If null or empty, ComputeFootprint falls back to default Irregular = full rectangle.
        // Used by RoomPainterWindow to feed painted cells directly.
        public List<Vector2Int> walkableCells = new List<Vector2Int>();

        public bool HasDoor => doorPosition.x >= 0 && doorPosition.y >= 0;
        public bool HasCustomWalkable => walkableCells != null && walkableCells.Count > 0;
    }

    [Serializable]
    public struct NicheSpec
    {
        public string side;
        public int anchorRow;
        public int width;
        public int depth;
        public bool mirror;
    }

    [Serializable]
    public struct ProtrusionSpec
    {
        public string side;
        public int anchorRow;
        public int width;
        public int depth;
        public bool mirror;
    }
}
