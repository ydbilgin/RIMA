using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA.Runtime.Encounter
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class IntraEncounterDoorTrigger : MonoBehaviour
    {
        [SerializeField] private string fromDoorSocketId;
        private BoxCollider2D col;
        private bool isActive;
        private bool playerInRange;
        private const Key InteractKey = Key.G;

        private void Awake()
        {
            col = GetComponent<BoxCollider2D>();
            col.isTrigger = true;
            SetActive(false);
        }

        public void Configure(string socketId)
        {
            fromDoorSocketId = socketId;
        }

        public void SetActive(bool active)
        {
            isActive = active;
            if (col == null) col = GetComponent<BoxCollider2D>();
            col.enabled = active;
            if (!active) ClearPlayerRange();
        }

        private void Update()
        {
            if (!isActive || !playerInRange) return;
            if (RoomTransitionFX.Instance != null && RoomTransitionFX.Instance.IsFading) return;
            if (Keyboard.current == null || !Keyboard.current[InteractKey].wasPressedThisFrame) return;

            ClearPlayerRange();
            SubRoomSequenceController.Active?.AdvanceSubRoom(fromDoorSocketId);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;
            if (!other.CompareTag("Player")) return;

            playerInRange = true;
            HUDController.Instance?.SetInteractionPrompt("Devam et");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            ClearPlayerRange();
        }

        private void ClearPlayerRange()
        {
            if (!playerInRange) return;
            playerInRange = false;
            HUDController.Instance?.HideInteractionPrompt();
        }
    }
}
