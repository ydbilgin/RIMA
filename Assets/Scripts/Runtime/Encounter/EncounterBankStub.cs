using RIMA.MapDesigner.Encounter;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Runtime.Encounter
{
    public class EncounterBankStub : MonoBehaviour, IEncounterBank
    {
        [SerializeField] private EncounterTemplateSO defaultEncounter;
        [SerializeField] private GameObject defaultEnemyPrefab;

        public bool TryResolve(RoomNode macroNode, string biomeId, out EncounterTemplateSO template)
        {
            template = defaultEncounter;
            return template != null &&
                   macroNode != null &&
                   (macroNode.roomType == RoomType.Combat || macroNode.roomType == RoomType.Elite);
        }

        public SubRoomEnemyPlan GetSubRoomEnemies(string encounterId, int subRoomIndex)
        {
            SubRoomEnemyPlan plan = new SubRoomEnemyPlan();
            RoomTemplateSO room = GetRoom(subRoomIndex);
            if (room == null || room.enemySpawnSockets == null || defaultEnemyPrefab == null)
                return plan;

            foreach (EnemySpawnSocket socket in room.enemySpawnSockets)
            {
                if (socket == null) continue;
                plan.Enemies.Add(new EnemyAssignment
                {
                    socketId = socket.socketId,
                    enemyPrefab = defaultEnemyPrefab,
                    isElite = false
                });
            }

            return plan;
        }

        public int GetSubRoomKillQuota(string encounterId, int subRoomIndex)
        {
            return GetSubRoomEnemies(encounterId, subRoomIndex).Enemies.Count;
        }

        private RoomTemplateSO GetRoom(int subRoomIndex)
        {
            if (defaultEncounter == null ||
                defaultEncounter.subRooms == null ||
                subRoomIndex < 0 ||
                subRoomIndex >= defaultEncounter.subRooms.Count)
            {
                return null;
            }

            return defaultEncounter.subRooms[subRoomIndex]?.room;
        }
    }
}
