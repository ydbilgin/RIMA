using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [CreateAssetMenu(menuName = "RIMA/Room/RoomTemplate", fileName = "RoomTemplate_New", order = 200)]
    public class RoomTemplateSO : ScriptableObject
    {
        public string schemaVersion = "1.0";
        public string roomId;
        public string biomeId;
        public RIMA.RoomType roomType;
        public RectInt bounds;

        public List<DoorSocket> doorSockets = new List<DoorSocket>();
        public PlayerSpawnSocket playerSpawn;
        public List<EnemySpawnSocket> enemySpawnSockets = new List<EnemySpawnSocket>();
        public CameraBounds cameraBounds;
        public GameObject prefabRef;

        public List<string> encounterTags = new List<string>();
        public List<string> difficultyTags = new List<string>();
        public List<string> blockerTags = new List<string>();

        [Header("Props (Sprint 12)")]
        public List<RIMA.MapDesigner.Props.PropPlacementData> props = new List<RIMA.MapDesigner.Props.PropPlacementData>();

        [Header("Walkable Grid (Sprint 13 — Condition 1 fix)")]
        [Tooltip("Per-tile walkability map. Index = (y * bounds.width) + x. true = walkable, false = wall/blocked. Empty array = full bounds walkable (fallback).")]
        public bool[] walkableGrid;

        public bool IsWalkable(Vector2Int tilePos)
        {
            if (walkableGrid == null || walkableGrid.Length == 0)
            {
                return tilePos.x >= bounds.xMin && tilePos.x < bounds.xMax &&
                       tilePos.y >= bounds.yMin && tilePos.y < bounds.yMax;
            }

            int lx = tilePos.x - bounds.xMin;
            int ly = tilePos.y - bounds.yMin;
            if (lx < 0 || lx >= bounds.width || ly < 0 || ly >= bounds.height) return false;

            int idx = (ly * bounds.width) + lx;
            return idx >= 0 && idx < walkableGrid.Length && walkableGrid[idx];
        }
    }
}
