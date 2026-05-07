using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Area Skill Placement input system.
    /// Handles hold-to-aim / release-to-cast ground-targeted AoE skills.
    ///
    /// Input flow (per AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md contract):
    ///   1. Skill key press  -> show ground indicator at cursor position
    ///   2. Mouse move       -> update indicator position (world-space raycast)
    ///   3. Skill key release -> cast skill at confirmed position, hide indicator
    ///   4. RMB or ESC       -> cancel placement (no cooldown spent)
    ///
    /// Rules:
    ///   - Max placement range: 6 tiles from player center
    ///   - Out of range: indicator turns red, clamps to max range edge
    ///   - Movement locked during placement
    ///   - No time dilation — world ticks normally
    ///
    /// Attach to the Player GameObject alongside PlayerAttack.
    /// Skills flagged as requiresPlacement route through this component.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class AreaSkillPlacer : MonoBehaviour
    {
        [Header("Placement Settings")]
        [SerializeField] private float maxPlacementRange = 6f;
        [SerializeField] private float windUpDuration = 0.20f;
        [SerializeField] private float commitDuration = 0.35f;

        [Header("Indicator")]
        [SerializeField] private GameObject circleIndicatorPrefab;
        [SerializeField] private GameObject coneIndicatorPrefab;

        [Header("Colors")]
        [SerializeField] private Color validColor = new Color(0.3f, 0.85f, 0.4f, 0.45f);
        [SerializeField] private Color invalidColor = new Color(0.9f, 0.2f, 0.15f, 0.45f);

        // ── Public state ──────────────────────────────────────────────
        /// <summary>True while the player is aiming a ground-targeted skill.</summary>
        public bool IsPlacing { get; private set; }

        /// <summary>World position where the skill will land if released now.</summary>
        public Vector2 PlacementPosition { get; private set; }

        // ── Events ────────────────────────────────────────────────────
        /// <summary>
        /// Fires when the player confirms placement (key released).
        /// Vector2 = confirmed world position.
        /// Subscriber (e.g. PlayerAttack or skill executor) triggers the actual skill.
        /// </summary>
        public event System.Action<Vector2> OnPlacementConfirmed;

        /// <summary>Fires when placement is cancelled (ESC / RMB).</summary>
        public event System.Action OnPlacementCancelled;

        // ── Internal ──────────────────────────────────────────────────
        private PlayerController controller;
        private Camera mainCam;
        private InputAction cancelAction;

        private GameObject activeIndicator;
        private SpriteRenderer indicatorRenderer;
        private bool isCircle;
        private float indicatorRadius;

        // ── Lifecycle ─────────────────────────────────────────────────
        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            mainCam = Camera.main;

            cancelAction = new InputAction("CancelPlacement", InputActionType.Button);
            cancelAction.AddBinding("<Keyboard>/escape");
        }

        private void OnEnable()
        {
            cancelAction.Enable();
        }

        private void OnDisable()
        {
            cancelAction.Disable();
            if (IsPlacing) CancelPlacement();
        }

        private void Update()
        {
            if (!IsPlacing) return;

            // Cancel on ESC
            if (cancelAction.WasPressedThisFrame())
            {
                CancelPlacement();
                return;
            }

            // Cancel on LMB (during aim)
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                CancelPlacement();
                return;
            }

            UpdateIndicatorPosition();
        }

        // ── Public API ────────────────────────────────────────────────

        /// <summary>
        /// Begin placement mode for a ground-targeted skill.
        /// Called by PlayerAttack or skill system when a skill with RequiresPlacement is activated.
        /// </summary>
        /// <param name="radius">Visual radius of the indicator circle.</param>
        /// <param name="skillColor">Tint color for the indicator (class/element themed).</param>
        /// <param name="useCircle">True = circle indicator, false = cone.</param>
        public void BeginPlacement(float radius, Color skillColor, bool useCircle = true)
        {
            if (IsPlacing) CancelPlacement();

            IsPlacing = true;
            isCircle = useCircle;
            indicatorRadius = radius;
            validColor = new Color(skillColor.r, skillColor.g, skillColor.b, 0.45f);

            SpawnIndicator();
            UpdateIndicatorPosition();
        }

        /// <summary>
        /// Confirm placement at the current indicator position.
        /// Called when the skill key is released.
        /// </summary>
        public void ConfirmPlacement()
        {
            if (!IsPlacing) return;

            Vector2 confirmedPos = PlacementPosition;
            CleanupIndicator();
            IsPlacing = false;

            OnPlacementConfirmed?.Invoke(confirmedPos);
        }

        /// <summary>
        /// Cancel placement without casting (ESC, RMB, or LMB during aim).
        /// </summary>
        public void CancelPlacement()
        {
            if (!IsPlacing) return;

            CleanupIndicator();
            IsPlacing = false;

            OnPlacementCancelled?.Invoke();
        }

        // ── Indicator management ──────────────────────────────────────

        private void SpawnIndicator()
        {
            GameObject prefab = isCircle ? circleIndicatorPrefab : coneIndicatorPrefab;

            if (prefab != null)
            {
                activeIndicator = Instantiate(prefab);
            }
            else
            {
                // Fallback: create a runtime circle indicator
                activeIndicator = CreateRuntimeCircleIndicator();
            }

            indicatorRenderer = activeIndicator.GetComponentInChildren<SpriteRenderer>();
            if (indicatorRenderer != null)
            {
                indicatorRenderer.color = validColor;
                indicatorRenderer.sortingLayerName = "VFX";
                indicatorRenderer.sortingOrder = 5;
            }

            float diameter = indicatorRadius * 2f;
            activeIndicator.transform.localScale = new Vector3(diameter, diameter, 1f);
        }

        private GameObject CreateRuntimeCircleIndicator()
        {
            var go = new GameObject("AreaIndicator_Runtime");
            var sr = go.AddComponent<SpriteRenderer>();

            // Use a basic circle sprite (same pattern as other runtime visuals)
            sr.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            sr.color = validColor;
            sr.sortingLayerName = "VFX";
            sr.sortingOrder = 5;

            return go;
        }

        private void UpdateIndicatorPosition()
        {
            if (activeIndicator == null) return;
            if (mainCam == null) mainCam = Camera.main;
            if (mainCam == null || Mouse.current == null) return;

            // Raycast mouse to ground plane (2D: just use ScreenToWorldPoint z=0)
            Vector3 mouseScreen = Mouse.current.position.ReadValue();
            mouseScreen.z = -mainCam.transform.position.z;
            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(mouseScreen);

            Vector2 playerPos = transform.position;
            Vector2 toMouse = mouseWorld - playerPos;
            float distance = toMouse.magnitude;

            bool inRange = distance <= maxPlacementRange;

            if (!inRange)
            {
                // Clamp to max range edge in cursor direction
                PlacementPosition = playerPos + toMouse.normalized * maxPlacementRange;
            }
            else
            {
                PlacementPosition = mouseWorld;
            }

            activeIndicator.transform.position = (Vector3)PlacementPosition;

            // Color feedback: valid = green-ish, out of range = red
            if (indicatorRenderer != null)
                indicatorRenderer.color = inRange ? validColor : invalidColor;
        }

        private void CleanupIndicator()
        {
            if (activeIndicator != null)
            {
                Destroy(activeIndicator);
                activeIndicator = null;
            }
            indicatorRenderer = null;
        }

        // ── Gizmos ────────────────────────────────────────────────────
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.3f, 0.8f, 0.4f, 0.2f);
            Gizmos.DrawWireSphere(transform.position, maxPlacementRange);

            if (IsPlacing)
            {
                Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
                Gizmos.DrawWireSphere((Vector3)PlacementPosition, indicatorRadius);
            }
        }
    }
}
