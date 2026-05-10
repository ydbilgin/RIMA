using UnityEngine;

namespace RIMA.Rooms
{
    /// <summary>
    /// Stores room-level metadata imported from Tiled map custom properties.
    /// Populated by RimaTmxPostProcessor on TMX import.
    /// </summary>
    public class RoomMetadata : MonoBehaviour
    {
        [Header("Room Identity")]
        public string roomId;
        public string roomType;
        public string biome;

        [Header("Difficulty")]
        public int difficulty;
        public int actBand;

        [Header("Combat")]
        public string combatQuestion;
    }
}
