using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace RIMA.Environment
{
    /// <summary>
    /// Designer-placed marker that anchors the portal fan layout for a room.
    /// PortalSpawnController.SpawnPortals(anchor) consumes this to place 1/2/3 portals.
    ///
    /// S110 LATE naming refactor: previously PortalSpawnAnchor. Canonical room
    /// exit is the "Gate" with a MapFragment reward; "Portal" is reserved for the
    /// rare Rift Portal event. Field/runtime hooks preserved via [MovedFrom].
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [MovedFrom(false, sourceNamespace: "RIMA.Environment", sourceClassName: "PortalSpawnAnchor")]
    public sealed class FragmentDropAnchor : MonoBehaviour
    {
        [Tooltip("Drives portal count weights and (later) destination biasing. Null falls back to default 20/40/40.")]
        public RoomTypeData roomType;

        [Tooltip("Axis along which the portal fan spreads. Right = horizontal fan.")]
        public Vector2 fanDirection = Vector2.right;

        [Tooltip("Phase 1: portal is the sole gate to the Skill Draft (auto-timer suppressed). Phase 2+ flips this off for branching flows.")]
        public bool usePortalGatedDraft = true;

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            Vector2 dir = fanDirection.sqrMagnitude > 0.0001f ? fanDirection.normalized : Vector2.right;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(pos, 0.2f);
            Gizmos.DrawLine(pos - (Vector3)(dir * 1.5f), pos + (Vector3)(dir * 1.5f));

            // Perpendicular tick to show "up" of fan (rotation Adım C reference).
            Vector3 up = new Vector3(-dir.y, dir.x, 0f) * 0.25f;
            Gizmos.DrawLine(pos - up, pos + up);

            // Reachability check vs. Player (Phase 1 gate): red sphere if anchor is unreachable from Player start.
            var walk = WalkabilityMap.Instance;
            if (walk != null && walk.floorTilemap != null && Application.isPlaying)
            {
                var cell = walk.floorTilemap.WorldToCell(pos);
                if (!walk.IsReachableFromPlayer(cell))
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(pos, 0.5f);
                }
            }
        }
    }
}
