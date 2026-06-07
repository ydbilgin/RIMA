// ⚠️ LEGACY (2026-06-07): Bu sınıf CANLI demo yolunda DEĞİL (kanıt: STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md).
// Canlı yol: _Arena → RoomRunDirector → IsoRoomBuilder.BuildExitDoors. Yeni iş BURAYA BAĞLANMAZ.
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using RIMA.Systems.Map;

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
        [SerializeField] private string targetRoomId;
        [SerializeField] private string targetSpawnPoint;
        [SerializeField] private bool autoEnterOnOverlap = false;

        [Header("Visual Feedback")]
        [SerializeField] private SpriteRenderer doorIndicator; // optional: glow/arrow sprite
        [SerializeField] private Color activeColor = new Color(0.2f, 1f, 0.4f, 0.6f);
        [SerializeField] private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        [SerializeField] private Canvas promptCanvas;
        [SerializeField] private Text promptText;
        [SerializeField] private ParticleSystem pulseParticles;

        private const Key InteractKey = Key.G;
        private static Sprite generatedGlowSprite;
        private BoxCollider2D col;
        private bool isActive;
        private bool triggered;
        private bool playerInRange;
        private bool HasTargetRoom => !string.IsNullOrEmpty(targetRoomId);
        public DoorDirection Direction => direction;

        private void Awake()
        {
            col = GetComponent<BoxCollider2D>();
            col.isTrigger = true;
            EnsureDoorVisuals();
            SetActive(false);
        }

        private void OnEnable()
        {
            if (col == null) col = GetComponent<BoxCollider2D>();
            if (!isActive || col == null) return;

            triggered = false;
            col.enabled = true;
        }

        public void ConfigureTransition(string roomId, string spawnPoint)
        {
            targetRoomId = roomId;
            targetSpawnPoint = spawnPoint;
        }

        public void SetActive(bool active)
        {
            if (col == null) col = GetComponent<BoxCollider2D>();
            EnsureDoorVisuals();
            isActive = active;
            col.enabled = active;
            if (!active) triggered = false;
            if (!active) ClearPlayerRange();

            if (doorIndicator != null)
                doorIndicator.color = active ? activeColor : inactiveColor;

            SetPulseActive(active);
        }

        private void Update()
        {
            if (!isActive || triggered || !playerInRange) return;
            if (Keyboard.current == null || !Keyboard.current[InteractKey].wasPressedThisFrame) return;
            if (!IsGateUnlocked()) return;

            triggered = true;
            col.enabled = false;
            ClearPlayerRange();

            TriggerTransition();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;
            if (!other.CompareTag("Player")) return;
            if (triggered) return;

            if (autoEnterOnOverlap)
            {
                if (!IsGateUnlocked()) return;

                triggered = true;
                col.enabled = false;
                ClearPlayerRange();

                TriggerTransition();
                return;
            }

            playerInRange = true;
            ShowPrompt();
        }

        private void TriggerTransition()
        {
            if (!IsGateUnlocked())
            {
                triggered = false;
                if (col != null) col.enabled = true;
                return;
            }

            if (RuntimeRoomManager.Instance == null)
            {
                MapFlowManager.ActiveInstance?.GoToNextMap();
                return;
            }

            if (HasTargetRoom)
                RuntimeRoomManager.Instance.TransitionToRoom(targetRoomId, targetSpawnPoint);
            else
                RuntimeRoomManager.Instance.OnPlayerEnteredDoor(direction);
        }

        private bool IsGateUnlocked()
        {
            GateBehavior gateBehavior = GetComponent<GateBehavior>();
            if (gateBehavior != null) return gateBehavior.IsOpen;

            RIMA.Environment.Gate gate = GetComponent<RIMA.Environment.Gate>();
            return gate == null || gate.CurrentState == RIMA.Environment.Gate.State.Unlocked;
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
            if (promptCanvas != null) promptCanvas.enabled = false;
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

        private void ShowPrompt()
        {
            if (promptCanvas != null)
            {
                if (Camera.main != null) promptCanvas.worldCamera = Camera.main;
                promptCanvas.enabled = true;
            }

            if (promptText != null)
                promptText.text = "Press G to enter";

            HUDController.Instance?.SetInteractionPrompt("Enter");
        }

        private void EnsureDoorVisuals()
        {
            if (doorIndicator == null)
            {
                GameObject glow = new GameObject("DoorGlow");
                glow.transform.SetParent(transform, false);
                glow.transform.localPosition = Vector3.zero;
                glow.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
                doorIndicator = glow.AddComponent<SpriteRenderer>();
                doorIndicator.sprite = GetGlowSprite();
                doorIndicator.sortingOrder = 12;
            }

            if (pulseParticles == null)
            {
                GameObject pulse = new GameObject("DoorPulse");
                pulse.transform.SetParent(transform, false);
                pulse.transform.localPosition = Vector3.zero;
                pulseParticles = pulse.AddComponent<ParticleSystem>();

                ParticleSystem.MainModule main = pulseParticles.main;
                main.startLifetime = 0.7f;
                main.startSpeed = 0.15f;
                main.startSize = 0.2f;
                main.startColor = activeColor;
                main.loop = true;

                ParticleSystem.EmissionModule emission = pulseParticles.emission;
                emission.rateOverTime = 2f;
            }

            if (promptCanvas == null)
            {
                GameObject canvasGO = new GameObject("DoorPromptCanvas");
                canvasGO.transform.SetParent(transform, false);
                canvasGO.transform.localPosition = new Vector3(0f, 1.25f, 0f);
                canvasGO.transform.localScale = Vector3.one * 0.01f;

                promptCanvas = canvasGO.AddComponent<Canvas>();
                promptCanvas.renderMode = RenderMode.WorldSpace;
                promptCanvas.sortingOrder = 80;

                RectTransform canvasRt = canvasGO.GetComponent<RectTransform>();
                canvasRt.sizeDelta = new Vector2(180f, 36f);

                GameObject textGO = new GameObject("PromptText");
                textGO.transform.SetParent(canvasGO.transform, false);
                promptText = textGO.AddComponent<Text>();
                promptText.text = "Press G to enter";
                promptText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                promptText.fontSize = 18;
                promptText.alignment = TextAnchor.MiddleCenter;
                promptText.color = Color.white;

                RectTransform textRt = textGO.GetComponent<RectTransform>();
                textRt.anchorMin = Vector2.zero;
                textRt.anchorMax = Vector2.one;
                textRt.offsetMin = Vector2.zero;
                textRt.offsetMax = Vector2.zero;
                promptCanvas.enabled = false;
            }
        }

        private void SetPulseActive(bool active)
        {
            if (pulseParticles == null) return;

            ParticleSystem.EmissionModule emission = pulseParticles.emission;
            emission.rateOverTime = active ? 2f : 0f;
            if (active && !pulseParticles.isPlaying)
                pulseParticles.Play();
            else if (!active)
                pulseParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        private static Sprite GetGlowSprite()
        {
            if (generatedGlowSprite != null) return generatedGlowSprite;

            Texture2D texture = new Texture2D(8, 8, TextureFormat.RGBA32, false);
            Color clear = new Color(1f, 1f, 1f, 0f);
            Color fill = new Color(1f, 1f, 1f, 0.65f);
            Vector2 center = new Vector2(3.5f, 3.5f);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), center);
                    texture.SetPixel(x, y, dist <= 3.5f ? fill : clear);
                }
            }

            texture.Apply();
            generatedGlowSprite = Sprite.Create(texture, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 8f);
            return generatedGlowSprite;
        }
    }
}

