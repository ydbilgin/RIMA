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
    }
}
