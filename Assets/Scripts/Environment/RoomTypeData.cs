using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>
    /// Per-room-type portal count weights (1 / 2 / 3 portals).
    /// Picked once when PortalSpawnController.SpawnPortals fires.
    /// </summary>
    [CreateAssetMenu(menuName = "RIMA/Environment/Room Type Data", fileName = "RoomType_New")]
    public sealed class RoomTypeData : ScriptableObject
    {
        public enum RoomCategory { Combat, Treasure, Ritual, BossApproach, Bridge }

        public RoomCategory category = RoomCategory.Combat;

        [Range(0, 100)] public int weight1Portal = 20;
        [Range(0, 100)] public int weight2Portal = 50;
        [Range(0, 100)] public int weight3Portal = 30;

        /// <summary>
        /// Weighted-random pick. Returns 1/2/3. Falls back to 1 if all weights are zero.
        /// </summary>
        public int PickPortalCount(System.Random rng)
        {
            int total = weight1Portal + weight2Portal + weight3Portal;
            if (total <= 0) return 1;
            int roll = (rng ?? new System.Random()).Next(total);
            if (roll < weight1Portal) return 1;
            if (roll < weight1Portal + weight2Portal) return 2;
            return 3;
        }
    }
}
