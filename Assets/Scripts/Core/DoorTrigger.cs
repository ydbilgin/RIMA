using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    public enum DoorDirection { North, South, East, West }

    /// <summary>
    /// Invisible trigger collider placed at each door gap.
    /// When player enters → tells RuntimeRoomManager to transition.
    /// Disabled by default; enabled only after room is cleared.
    ///
    /// Setup: Create 4 empty GameObjects at door positions, each with:
    ///   - BoxCollider2D (isTrigger = true, size ≈ 2x2)
    ///   - This script (set direction in Inspector)
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private DoorDirection direction;

        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer doorIndicator; // optional: glow/arrow sprite
        [SerializeField] private Color activeColor = new Color(0.2f, 1f, 0.4f, 0.6f);
        [SerializeField] private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

        private const Key InteractKey = Key.G;
        private BoxCollider2D col;
        private bool isActive;
        private bool triggered;
        private bool playerInRange;

        private void Awake()
        {
            col = GetComponent<BoxCollider2D>();
            col.isTrigger = true;
            SetActive(false);
        }

        public void SetActive(bool active)
        {
            isActive = active;
            col.enabled = active;
            if (!active) triggered = false;
            if (!active) ClearPlayerRange();

            if (doorIndicator != null)
                doorIndicator.color = active ? activeColor : inactiveColor;
        }

        private void Update()
        {
            if (!isActive || triggered || !playerInRange) return;
            if (Keyboard.current == null || !Keyboard.current[InteractKey].wasPressedThisFrame) return;

            triggered = true;
            col.enabled = false;
            ClearPlayerRange();

            if (RuntimeRoomManager.Instance != null)
                RuntimeRoomManager.Instance.OnPlayerEnteredDoor(direction);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;
            if (!other.CompareTag("Player")) return;
            if (triggered) return;

            playerInRange = true;
            HUDController.Instance?.SetInteractionPrompt(GetPromptText());
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

        private string GetPromptText()
        {
            return direction switch
            {
                DoorDirection.North => "İlerle",
                DoorDirection.East => "Sağ Odaya Geç",
                DoorDirection.West => "Sol Odaya Geç",
                DoorDirection.South => "Geri Dön",
                _ => "İlerle"
            };
        }
    }
}
