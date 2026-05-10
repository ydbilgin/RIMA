using UnityEngine;

namespace RIMA.Rooms
{
    /// <summary>
    /// Marks a position in the room as a mob spawn location.
    /// Populated by RimaTmxPostProcessor from Tiled object layer custom properties.
    /// </summary>
    public class MobSpawnPoint : MonoBehaviour
    {
        [Header("Spawn Definition")]
        public string spawnTier;
        public string spawnTags;

        [Header("Runtime State")]
        public bool hasSpawned;

        public string[] GetTagArray()
        {
            if (string.IsNullOrEmpty(spawnTags))
                return System.Array.Empty<string>();
            return spawnTags.Split(',');
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.4f);
#if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f,
                string.Format("Spawn:{0}", spawnTier));
#endif
        }
    }
}
