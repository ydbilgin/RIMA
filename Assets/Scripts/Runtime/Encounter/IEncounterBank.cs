using RIMA.MapDesigner.Encounter;

namespace RIMA.Runtime.Encounter
{
    public interface IEncounterBank
    {
        bool TryResolve(RoomNode macroNode, string biomeId, out EncounterTemplateSO template);
        SubRoomEnemyPlan GetSubRoomEnemies(string encounterId, int subRoomIndex);
        int GetSubRoomKillQuota(string encounterId, int subRoomIndex);
    }
}
