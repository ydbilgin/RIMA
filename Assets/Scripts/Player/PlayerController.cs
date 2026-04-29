using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

namespace RIMA
{
    public enum DashMode { FacingDirection, TowardsMouse }

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4.5f;

        public float MoveSpeed => moveSpeed;

        [Header("Dash")]
        [SerializeField] private float dashSpeed = 18f;
        [SerializeField] private float dashDuration = 0.15f;
        [SerializeField] private float dashCooldown = 0.8f;

        [Header("Combat Feel")]
        [SerializeField] private float commitmentMoveMult = 0.25f;  // attack commitment'ta hareket %25

        private Rigidbody2D rb;
        private Collider2D col;
        private Camera mainCam;
        private StatusEffectSystem statusEffects;
        private PlayerAttack attack;
        private Vector2 moveInput;
        private Vector2 lastMoveDir = Vector2.down;

        private bool isDashing;
        private float dashTimer;
        private float dashCooldownTimer;
        private Vector2 dashDir;

        // Obstacle awareness
        // NarrowPassage: during dash we switch the player's physics layer so
        // the overlap with the NarrowPassage collider is ignored by 2D physics.
        // Chasm: while inside a Chasm trigger the player is pushed back unless dashing.
        private readonly HashSet<Chasm> activeChasms = new();
        private readonly HashSet<NarrowPassage> activeNarrowPassages = new();

        private int defaultLayer;
        private const string DashPassLayer = "Ignore Raycast"; // temporarily used for NarrowPassage pass-through

        private InputAction moveAction;
        private InputAction dashAction;

        public Vector2 FacingDirection => lastMoveDir;
        public bool IsDashing => isDashing;
        public bool IsMoving => moveInput.sqrMagnitude > 0.01f;

        // Ayar: ESC menüsünden değiştirilir, PlayerPrefs ile saklanır
        public DashMode DashMode
        {
            get => (DashMode)PlayerPrefs.GetInt("DashMode", (int)DashMode.FacingDirection);
            set => PlayerPrefs.SetInt("DashMode", (int)value);
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            statusEffects = GetComponent<StatusEffectSystem>();
            attack = GetComponent<PlayerAttack>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            mainCam = Camera.main;
            defaultLayer = gameObject.layer;

            moveAction = new InputAction("Move", InputActionType.Value);
            moveAction.AddCompositeBinding("2DVector")
                .With("Up",    "<Keyboard>/w")
                .With("Down",  "<Keyboard>/s")
                .With("Left",  "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            moveAction.AddBinding("<Gamepad>/leftStick");

            dashAction = new InputAction("Dash", InputActionType.Button);
            dashAction.AddBinding("<Keyboard>/space");
            dashAction.AddBinding("<Gamepad>/buttonSouth");
        }

        private void OnEnable()
        {
            moveAction.Enable();
            dashAction.Enable();
            dashAction.performed += HandleDash;
        }

        private void OnDisable()
        {
            dashAction.performed -= HandleDash;
            moveAction.Disable();
            dashAction.Disable();
        }

        private void HandleDash(InputAction.CallbackContext ctx)
        {
            if (isDashing || dashCooldownTimer > 0f) return;

            if (DashMode == DashMode.TowardsMouse && mainCam != null)
            {
                Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 toMouse = mouseWorld - (Vector2)transform.position;
                dashDir = toMouse.sqrMagnitude > 0.01f ? toMouse.normalized : lastMoveDir;
            }
            else
            {
                dashDir = lastMoveDir;
            }

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            // NarrowPassage geçiş: dash sırasında Obstacle layer ile çakışmayı kapat
            if (activeNarrowPassages.Count > 0)
                gameObject.layer = LayerMask.NameToLayer(DashPassLayer);
        }

        private void Update()
        {
            moveInput = moveAction.ReadValue<Vector2>();
            if (moveInput != Vector2.zero)
                lastMoveDir = moveInput.normalized;

            if (dashCooldownTimer > 0f)
                dashCooldownTimer -= Time.deltaTime;

            if (isDashing)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f)
                {
                    isDashing = false;
                    gameObject.layer = defaultLayer;
                }
            }
        }

        private void FixedUpdate()
        {
            float speedMult = statusEffects != null ? statusEffects.moveSpeedMultiplier : 1f;
            if (attack != null && attack.IsCommitted) speedMult *= commitmentMoveMult;

            if (isDashing)
                rb.linearVelocity = dashDir * dashSpeed;
            else
                rb.linearVelocity = moveInput * moveSpeed * speedMult;
        }

        // ────────────────────────────────────────────────────────────────
        // Chasm push-back
        // ────────────────────────────────────────────────────────────────
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Chasm>(out var chasm))
                activeChasms.Add(chasm);

            if (other.TryGetComponent<NarrowPassage>(out var np))
                activeNarrowPassages.Add(np);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Chasm>(out var chasm))
                activeChasms.Remove(chasm);

            if (other.TryGetComponent<NarrowPassage>(out var np))
                activeNarrowPassages.Remove(np);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            // Chasm: dash değilken içinde ise geri it
            if (!isDashing && other.TryGetComponent<Chasm>(out _))
            {
                // Chasm merkezinden itme
                Vector2 chasmCenter = other.bounds.center;
                Vector2 pushDir = ((Vector2)transform.position - chasmCenter).normalized;
                if (pushDir == Vector2.zero) pushDir = Vector2.up;
                rb.linearVelocity = pushDir * moveSpeed * 2f;
            }

            // NarrowPassage: yürüyerek geçilemez, dash sırasında geçilebilir.
            if (!isDashing && other.TryGetComponent<NarrowPassage>(out _))
            {
                Vector2 passageCenter = other.bounds.center;
                Vector2 pushDir = ((Vector2)transform.position - passageCenter).normalized;
                if (pushDir == Vector2.zero) pushDir = -lastMoveDir;
                rb.linearVelocity = pushDir * moveSpeed * 2f;
            }
        }
    }
}
