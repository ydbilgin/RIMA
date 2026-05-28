using UnityEngine;

namespace RIMA
{
    [System.Obsolete("Stale — use PlayerController. Kept only because Warblade.prefab serializes this component. Do NOT add new logic here.")]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        // moveSpeed removed — PlayerController owns movement (4.5f).
        // Rigidbody2D setup removed — PlayerController.Awake() owns gravityScale + freezeRotation.

        // This component is retained solely because Warblade.prefab has a serialized
        // reference to it. RuntimeRoomManager already targets PlayerController directly
        // for enable/disable during room transitions. Safe to remove once the prefab
        // reference is stripped via the Inspector.
    }
}
