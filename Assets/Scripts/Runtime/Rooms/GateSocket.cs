using UnityEngine;

namespace RIMA.Rooms
{
    /// <summary>
    /// Marks a position in the room as a connection point to an adjacent room.
    /// Populated by RimaTmxPostProcessor from Tiled object layer custom properties.
    /// </summary>
    public class GateSocket : MonoBehaviour
    {
        [Header("Gate Identity")]
        public string socketId;
        public string direction;
        public string gateVisual;
        public string route;

        [Header("Runtime State")]
        public bool isConnected;
        public string connectedRoomId;

        private void OnDrawGizmos()
        {
            Gizmos.color = isConnected ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
#if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 0.6f,
                string.Format("Gate:{0} ({1})", socketId, direction));
#endif
        }
    }
}
