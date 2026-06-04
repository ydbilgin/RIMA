using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Basic Attack")]
        [SerializeField] private BasicAttackProfile basicAttackProfile;

        [Header("VFX")]
        [SerializeField] private SlashArcVFX slashArcVFX;

        public event Action<int> OnComboStep;

        private PlayerController controller;
        private RageSystem rage;
        private SkillFlowTracker flowTracker;
        private InputBufferService inputBuffer;
        private InputAction attackAction;
        private InputAction secondaryAction;
        private InputAction riftBreakAction;
        private IBasicAttackBehavior behavior;

        [HideInInspector] public float outgoingDamageMultiplier = 1f;
        [HideInInspector] public int baseDamage = 0;

        internal int ComboStep { get; set; }
        internal float ComboTimer { get; set; }
        internal float CommitTimer { get; set; }
        internal bool BufferedAttack { get; set; }
        internal float RangerAttackStartedAt { get; set; } = -1f;
        internal float ElementalistSecondaryStartedAt { get; set; } = -1f;
        internal float WarbladeRageOutletTimer { get; set; }

        public bool IsCommitted => CommitTimer > 0f;

        /// <summary>
        /// A2 — finisher (last combo step) hit reach, so the commit-beat BREAK lands exactly
        /// where the 3rd-hit swing connects. Mirrors BasicAttackBehaviorBase.ApplyMeleeHit:
        /// hitCenter = transform.position + FacingDirection * range, OverlapCircle(range, radius).
        /// Returns false if no profile is assigned.
        /// </summary>
        public bool TryGetFinisherReach(out Vector2 facing, out float range, out float radius)
        {
            facing = controller != null ? controller.FacingDirection : Vector2.right;
            if (basicAttackProfile == null) { range = 0f; radius = 0f; return false; }
            int finisherStep = basicAttackProfile.comboLength - 1;
            range = basicAttackProfile.GetHitRangeForStep(finisherStep);
            radius = basicAttackProfile.GetHitRadiusForStep(finisherStep);
            return true;
        }

        /// <summary>
        /// Duration of the procedural weapon swing for the current attack, spanning the
        /// startup windup through the commitment follow-through. Consumed by the
        /// orientation/mount bridge (HandAnchorAttach) to drive OrientationSync.BeginSwing.
        /// </summary>
        public float CurrentSwingWindow => basicAttackProfile != null
            ? Mathf.Max(0.12f, basicAttackProfile.attackStartup + basicAttackProfile.commitment)
            : 0.2f;

        /// <summary>
        /// Normalized time within <see cref="CurrentSwingWindow"/> at which the mechanical
        /// hit fires (attackStartup). Used to align the visual swing's strike frame to the hit.
        /// </summary>
        public float CurrentStrikeFraction => basicAttackProfile != null && CurrentSwingWindow > 0f
            ? Mathf.Clamp01(basicAttackProfile.attackStartup / CurrentSwingWindow)
            : 0.35f;

        /// <summary>
        /// Returns true if a dash is allowed. If within the cancel window, interrupts the commitment.
        /// Returns false if mid-commit and past the cancel window (dash blocked).
        /// </summary>
        public bool TryCancelForDash()
        {
            if (!IsCommitted) return true;
            if (basicAttackProfile == null) return true;

            float totalCommit = basicAttackProfile.commitment;
            if (totalCommit <= 0f) return true;

            float progress = (totalCommit - CommitTimer) / totalCommit; // 0→1 during commit
            float cancelWindow = basicAttackProfile.GetCancelWindow();

            if (progress < cancelWindow)
            {
                CommitTimer = 0f;
                return true;
            }
            return false;
        }

        internal PlayerController Controller => controller;

        internal RageSystem Rage
        {
            get
            {
                if (rage == null) rage = GetComponent<RageSystem>();
                return rage;
            }
        }

        internal SkillFlowTracker FlowTracker
        {
            get
            {
                if (flowTracker == null) flowTracker = GetComponent<SkillFlowTracker>();
                return flowTracker;
            }
        }

        internal ClassType PrimaryClass => PlayerClassManager.Instance != null
            ? PlayerClassManager.Instance.PrimaryClass
            : ClassType.Warblade;

        public void SetBasicAttackProfile(BasicAttackProfile profile)
        {
            if (profile == null) return;
            basicAttackProfile = profile;
            behavior = basicAttackProfile.CreateBehavior();
            ComboStep = 0;
            ComboTimer = 0f;
            CommitTimer = 0f;
            BufferedAttack = false;
            enabled = true;
        }

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            rage = GetComponent<RageSystem>();
            flowTracker = GetComponent<SkillFlowTracker>();
            inputBuffer = GetComponent<InputBufferService>();

            if (basicAttackProfile == null)
            {
                basicAttackProfile = LoadDefaultBasicAttackProfile();
                if (basicAttackProfile == null)
                {
                    Debug.LogError($"[PlayerAttack] No BasicAttackProfile assigned on {gameObject.name}. " +
                        "Assign a class-specific profile in the Inspector.", this);
                    enabled = false;
                    return;
                }
            }

            behavior = basicAttackProfile.CreateBehavior();
            BuildInputActions();
        }

        private static BasicAttackProfile LoadDefaultBasicAttackProfile()
        {
            ClassType type = PlayerClassManager.SelectedClass != ClassType.None
                ? PlayerClassManager.SelectedClass
                : ClassType.Warblade;
            BasicAttackProfile profile = Resources.Load<BasicAttackProfile>($"Combat/BasicAttack/BasicAttackProfile_{type}");

#if UNITY_EDITOR
            if (profile == null && type == ClassType.Ronin)
                profile = UnityEditor.AssetDatabase.LoadAssetAtPath<BasicAttackProfile>(
                    "Assets/Data/Combat/Profiles/Ronin_BasicAttackProfile.asset");
#endif

            return profile;
        }

        // Recreates any null InputActions (Awake-created fields are nulled by a mid-play domain reload)
        // and enables them when active. Called from Awake and the Update self-heal.
        private void BuildInputActions()
        {
            if (attackAction == null)
            {
                attackAction = new InputAction("Attack", InputActionType.Button);
                attackAction.AddBinding(KeyBindManager.GetBinding(GameAction.Attack));
                attackAction.AddBinding("<Gamepad>/buttonWest");
            }
            if (secondaryAction == null)
            {
                secondaryAction = new InputAction("ClassSecondary", InputActionType.Button);
                secondaryAction.AddBinding(KeyBindManager.GetBinding(GameAction.ClassSecondary));
                secondaryAction.AddBinding("<Gamepad>/buttonEast");
            }
            if (riftBreakAction == null)
            {
                riftBreakAction = new InputAction("RiftBreak", InputActionType.Button);
                riftBreakAction.AddBinding(KeyBindManager.GetBinding(GameAction.RiftBreak));
            }
            if (isActiveAndEnabled)
            {
                attackAction.Enable();
                secondaryAction.Enable();
                riftBreakAction.Enable();
            }
        }

        // Force-recreate the actions from the registry after a rebind (BuildInputActions only fills nulls).
        private void RebuildInputActions()
        {
            attackAction?.Disable();    attackAction?.Dispose();    attackAction = null;
            secondaryAction?.Disable(); secondaryAction?.Dispose(); secondaryAction = null;
            riftBreakAction?.Disable(); riftBreakAction?.Dispose(); riftBreakAction = null;
            BuildInputActions();
        }

        private void OnEnable()
        {
            attackAction?.Enable();
            secondaryAction?.Enable();
            riftBreakAction?.Enable();
            KeyBindManager.OnBindingsChanged += RebuildInputActions;
        }

        private void OnDisable()
        {
            KeyBindManager.OnBindingsChanged -= RebuildInputActions;
            attackAction?.Disable();
            secondaryAction?.Disable();
            riftBreakAction?.Disable();
        }

        private void Update()
        {
            // Self-heal: a script recompile / domain reload during play mode nulls
            // non-serialized fields like `behavior` (Awake is not re-invoked mid-play).
            // Recreate it from the serialized profile instead of NRE-spamming every frame.
            if (behavior == null || attackAction == null || secondaryAction == null || riftBreakAction == null)
            {
                if (basicAttackProfile != null && behavior == null) behavior = basicAttackProfile.CreateBehavior();
                BuildInputActions();
                if (behavior == null) return;
            }
            behavior.OnUpdate(this, basicAttackProfile, Time.deltaTime);

            bool attackPressed = attackAction.WasPressedThisFrame();
            if (attackPressed && IsCommitted && inputBuffer != null)
            {
                inputBuffer.RequestAttack();
                attackPressed = false;
            }

            behavior.OnLMBInput(
                this, basicAttackProfile,
                attackPressed,
                attackAction.WasReleasedThisFrame());
            behavior.OnRMBInput(
                this, basicAttackProfile,
                secondaryAction.WasPressedThisFrame(),
                secondaryAction.WasReleasedThisFrame());

            HandleRiftBreakInput();
        }

        private void HandleRiftBreakInput()
        {
            if (!riftBreakAction.WasPressedThisFrame()) return;

            RageSystem currentRage = Rage;
            if (currentRage == null) return;

            if (currentRage.TryConsume(100))
                Debug.Log("[RiftBreak] Triggered!");
            else
                Debug.Log("[RiftBreak] Not enough rage.");
        }

        internal void RaiseComboStep(int step)
        {
            OnComboStep?.Invoke(step);
        }

        internal void ExecuteBufferedPrimaryAttack()
        {
            if (IsCommitted || basicAttackProfile == null) return;
            if (behavior == null) behavior = basicAttackProfile.CreateBehavior();
            behavior?.OnLMBInput(this, basicAttackProfile, true, false);
        }

#if UNITY_INCLUDE_TESTS
        /// <summary>
        /// Test-only hook: invokes the basic attack behavior directly, bypassing
        /// InputAction device binding. Safe to call from PlayMode tests.
        /// </summary>
        public void InvokeBasicAttackForTest()
        {
            if (behavior == null || basicAttackProfile == null) return;
            behavior.OnLMBInput(this, basicAttackProfile, true, false);
        }
#endif

        internal void EmitSlashArc(Vector2 facingDirection, int step)
        {
            slashArcVFX?.Emit(facingDirection, step);
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying || controller == null || basicAttackProfile == null) return;
            float range = basicAttackProfile.GetHitRangeForStep(ComboStep);
            float radius = basicAttackProfile.GetHitRadiusForStep(ComboStep);
            Gizmos.color = new Color(1f, 1f, 0f, 0.35f);
            Gizmos.DrawWireSphere((Vector2)transform.position + controller.FacingDirection * range, radius);
        }
    }
}
