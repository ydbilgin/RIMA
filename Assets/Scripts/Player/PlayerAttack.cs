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

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            rage = GetComponent<RageSystem>();
            flowTracker = GetComponent<SkillFlowTracker>();

            if (basicAttackProfile == null)
            {
                Debug.LogError($"[PlayerAttack] No BasicAttackProfile assigned on {gameObject.name}. " +
                    "Assign a class-specific profile in the Inspector.", this);
                enabled = false;
                return;
            }

            behavior = basicAttackProfile.CreateBehavior();

            attackAction = new InputAction("Attack", InputActionType.Button);
            attackAction.AddBinding("<Mouse>/leftButton");
            attackAction.AddBinding("<Gamepad>/buttonWest");

            secondaryAction = new InputAction("ClassSecondary", InputActionType.Button);
            secondaryAction.AddBinding("<Mouse>/rightButton");
            secondaryAction.AddBinding("<Gamepad>/buttonEast");

            riftBreakAction = new InputAction("RiftBreak", InputActionType.Button);
            riftBreakAction.AddBinding("<Keyboard>/v");
        }

        private void OnEnable()
        {
            attackAction?.Enable();
            secondaryAction?.Enable();
            riftBreakAction?.Enable();
        }

        private void OnDisable()
        {
            attackAction?.Disable();
            secondaryAction?.Disable();
            riftBreakAction?.Disable();
        }

        private void Update()
        {
            behavior.OnUpdate(this, basicAttackProfile, Time.deltaTime);
            behavior.OnLMBInput(
                this, basicAttackProfile,
                attackAction.WasPressedThisFrame(),
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
