using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using RIMA.Environment;

namespace RIMA
{
    public enum DashMode { FacingDirection, TowardsMouse }
    public enum CombatAimMode { CharacterFacing, TowardsMouse }

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
        [SerializeField] private float combatFacingLockDuration = 0.18f;

        private Rigidbody2D rb;
        private Collider2D col;
        private Camera mainCam;
        private StatusEffectSystem statusEffects;
        private PlayerAttack attack;
        private Health health;
        private InputBufferService inputBuffer;
        private Vector2 moveInput;
        private Vector2 lastMoveDir = new(1f, -1f);
        private Vector2 movementFacingDir = new(1f, -1f);
        private Vector2 combatFacingDir = new(1f, -1f);
        private float combatFacingLockedUntil;

        private bool isDashing;
        private float dashTimer;
        private float dashCooldownTimer;
        private Vector2 dashDir;
        private bool dashWasImmune;
        private bool _mercifulDodgeActive;
        private float _mercifulDodgeExpiry;

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
        private const float MoveDeadzoneSqr = 0.01f;
        public const string AttackAimModePrefKey = "AttackAimMode";
        private const string AttackAimModeCursorDefaultMigrationKey = "AttackAimModeCursorDefault_20260503";

        public Vector2 FacingDirection => HasCombatFacingOverride ? combatFacingDir : movementFacingDir;
        public Vector2 MovementFacingDirection => movementFacingDir;
        public bool HasCombatFacingOverride => Time.time < combatFacingLockedUntil;
        public bool IsDashing => isDashing;
        public bool IsMoving => moveInput.sqrMagnitude > MoveDeadzoneSqr;

        public event System.Action CombatFacingChanged;

        // Ayar: ESC menüsünden değiştirilir, PlayerPrefs ile saklanır
        public DashMode DashMode
        {
            get => (DashMode)PlayerPrefs.GetInt("DashMode", (int)DashMode.FacingDirection);
            set => PlayerPrefs.SetInt("DashMode", (int)value);
        }

        public CombatAimMode AttackAimMode
        {
            get => (CombatAimMode)PlayerPrefs.GetInt(AttackAimModePrefKey, (int)CombatAimMode.TowardsMouse);
            set => PlayerPrefs.SetInt(AttackAimModePrefKey, (int)value);
        }

        // CapsuleCollider2D sizing rationale (set in Warblade.prefab Inspector):
        // width  = effective sprite width  × 0.9  — 10% inset prevents shoulder pixels
        //          (alpha-threshold ~5%) from clipping into wall colliders.
        // height = effective sprite height (full) — no vertical inset needed for top-down.
        // "Effective" bounds = alpha-threshold scan at 5% opacity cutoff on the sprite sheet.
        // Example: effective width 0.5938 → collider width 0.53 (0.5938 × 0.9 ≈ 0.535 → rounded 0.53).
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            statusEffects = GetComponent<StatusEffectSystem>();
            attack = GetComponent<PlayerAttack>();
            inputBuffer = GetComponent<InputBufferService>();
            health = GetComponent<Health>();
            if (health != null)
                health.OnDamageTaken.AddListener(ActivateMercifulDodge);
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            mainCam = Camera.main;
            defaultLayer = gameObject.layer;
            EnsureCombatAimDefault();
            GroundBlobShadow.Ensure(transform, new Vector2(1.0f, 0.34f), 0.30f);

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

        private static void EnsureCombatAimDefault()
        {
            if (PlayerPrefs.GetInt(AttackAimModeCursorDefaultMigrationKey, 0) != 0) return;

            PlayerPrefs.SetInt(AttackAimModePrefKey, (int)CombatAimMode.TowardsMouse);
            PlayerPrefs.SetInt(AttackAimModeCursorDefaultMigrationKey, 1);
            PlayerPrefs.Save();
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
            TryDash();
        }

        public bool TryDash()
        {
            if (isDashing || dashCooldownTimer > 0f) return false;

            bool bypassAttackCommit = false;
            if (_mercifulDodgeActive)
            {
                if (Time.time <= _mercifulDodgeExpiry)
                {
                    _mercifulDodgeActive = false;
                    _mercifulDodgeExpiry = 0f;
                    if (attack != null)
                        attack.CommitTimer = 0f;
                    bypassAttackCommit = true;
                }
                else
                {
                    _mercifulDodgeActive = false;
                }
            }

            // Blocked if mid-commit and past the dash-cancel window
            if (!bypassAttackCommit && attack != null && !attack.TryCancelForDash())
            {
                inputBuffer?.RequestDash();
                return false;
            }

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

            // Walkability pre-check (Hades pattern): if dash would land on void, abort.
            // Input is registered, cooldown is NOT consumed, no movement starts.
            // MVP: dash end = current + dashDir * (dashSpeed * dashDuration).
            // If no WalkabilityMap in scene, behavior is unchanged (legacy permissive).
            WalkabilityMap walkMap = WalkabilityMap.Instance;
            if (walkMap != null)
            {
                Vector3 dashEnd = transform.position + (Vector3)(dashDir * (dashSpeed * dashDuration));
                if (!walkMap.IsDashableWorld(dashEnd))
                {
                    // Future: trigger dash-fail SFX / anim hook here.
                    return false;
                }
            }

            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            dashWasImmune = health != null && health.IsImmune;
            health?.SetImmune(true);

            // OnDash passive proc
            CrossClassSkillManager.Instance?.OnDash();

            // NarrowPassage geçiş: dash sırasında Obstacle layer ile çakışmayı kapat
            if (activeNarrowPassages.Count > 0)
                gameObject.layer = LayerMask.NameToLayer(DashPassLayer);

            return true;
        }

        private void Update()
        {
            if (_mercifulDodgeActive && Time.time > _mercifulDodgeExpiry)
                _mercifulDodgeActive = false;

            moveInput = moveAction.ReadValue<Vector2>();

            if (moveInput.sqrMagnitude <= MoveDeadzoneSqr)
            {
                moveInput = Vector2.zero;
            }
            else
            {
                lastMoveDir = moveInput.normalized;
                movementFacingDir = lastMoveDir;
            }

            if (dashCooldownTimer > 0f)
                dashCooldownTimer -= Time.deltaTime;

            if (isDashing)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f)
                {
                    isDashing = false;
                    health?.SetImmune(dashWasImmune);
                    gameObject.layer = defaultLayer;
                }
            }
        }

        private void FixedUpdate()
        {
            float speedMult = statusEffects != null ? statusEffects.moveSpeedMultiplier : 1f;
            if (attack != null && attack.IsCommitted) speedMult *= commitmentMoveMult;

            if (isDashing)
            {
                rb.linearVelocity = dashDir * dashSpeed;
                return;
            }

            // Defensive walkable pre-check: if next-frame position lands on a void cell,
            // zero velocity instead of moving. Acts as a fallback for VoidBlocker collider.
            // Permissive when no WalkabilityMap exists (legacy behavior preserved).
            Vector2 desiredVel = moveInput * moveSpeed * speedMult;
            WalkabilityMap walkMap = WalkabilityMap.Instance;
            if (walkMap != null && desiredVel.sqrMagnitude > MoveDeadzoneSqr)
            {
                Vector3 nextPos = transform.position + (Vector3)(desiredVel * Time.fixedDeltaTime);
                if (!walkMap.IsWalkableWorld(nextPos))
                {
                    rb.linearVelocity = Vector2.zero;
                    return;
                }
            }

            rb.linearVelocity = desiredVel;
        }

        public void FaceCombatTarget()
        {
            Vector2 targetDir;
            if (AttackAimMode == CombatAimMode.TowardsMouse)
                targetDir = GetMouseDirectionOrFallback();
            else
                targetDir = movementFacingDir.sqrMagnitude > MoveDeadzoneSqr ? movementFacingDir : lastMoveDir;

            FaceCombatDirection(targetDir);
        }

        public void FaceCombatDirection(Vector2 direction, float lockDuration = -1f)
        {
            if (direction.sqrMagnitude <= MoveDeadzoneSqr)
                direction = FacingDirection.sqrMagnitude > MoveDeadzoneSqr ? FacingDirection : lastMoveDir;

            combatFacingDir = direction.normalized;
            combatFacingLockedUntil = Time.time + (lockDuration >= 0f ? lockDuration : combatFacingLockDuration);
            CombatFacingChanged?.Invoke();
        }

        private Vector2 GetMouseDirectionOrFallback()
        {
            if (mainCam == null) mainCam = Camera.main;
            if (mainCam == null || Mouse.current == null)
                return FacingDirection.sqrMagnitude > MoveDeadzoneSqr ? FacingDirection : lastMoveDir;

            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 toMouse = mouseWorld - (Vector2)transform.position;
            return toMouse.sqrMagnitude > MoveDeadzoneSqr ? toMouse.normalized : lastMoveDir;
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

        private void ActivateMercifulDodge(int _)
        {
            _mercifulDodgeActive = true;
            _mercifulDodgeExpiry = Time.time + 0.18f;
        }
    }
}
