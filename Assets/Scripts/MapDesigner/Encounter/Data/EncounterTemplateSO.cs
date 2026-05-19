using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Encounter
{
    [CreateAssetMenu(menuName = "RIMA/Encounter/EncounterTemplate", fileName = "Encounter_New", order = 210)]
    public class EncounterTemplateSO : ScriptableObject
    {
        public string schemaVersion = "1.0";
        public string encounterId;
        public RIMA.RoomType macroRoomType;
        public string biomeId;
        public int encounterSeed;
        public List<SubRoomEntry> subRooms = new List<SubRoomEntry>();
        public List<string> encounterTags = new List<string>();
        public string encounterBankKey;
    }
}
