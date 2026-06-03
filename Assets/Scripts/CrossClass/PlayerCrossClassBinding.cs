using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Feature B (B1) — holds the player's single active cross-class Echo binding and fires it.
    ///
    /// Lives on the PLAYER ROOT (same GO as the *_SkillController). On bind it AddComponents the
    /// guest <see cref="SkillBase"/> onto the player — exactly the pattern DraftManager already uses
    /// (AddComponent(skill.skillType)) — so the guest skill resolves player/rage/resource via
    /// GetComponentInParent correctly. Pressing the Echo key spawns a transient <see cref="CrossClassEcho"/>
    /// silhouette which, at its strike frame, calls <see cref="SkillBase.ExecuteAt"/> on that guest
    /// component (so the SAME damage/break logic fires, just from the echo's position).
    ///
    /// The guest SkillBase stays a normal (player-parented) component but is NOT placed in a skill-bar
    /// slot — it is invoked only through the echo, on its own cooldown tracked here (a "guest favor").
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerCrossClassBinding : MonoBehaviour
    {
        [Tooltip("Bound guest entry (set via Bind()). Drives archetype + guest skill ref.")]
        [SerializeField] private CrossClassSkillData binding;

        [Tooltip("Cooldown for the Echo activation (a 'guest favor'). Falls back to binding.cooldown, " +
                 "then to this default if both are 0.")]
        [SerializeField] private float defaultCooldown = 12f;

        private SkillBase guestSkill;   // the AddComponent'd guest behavior (lives on player root)
        private float cooldownTimer;
        private InputAction echoAction;

        /// <summary>True if a guest Echo is bound and ready to fire.</summary>
        public bool HasEcho => binding != null && binding.IsEcho && guestSkill != null;
        public bool IsReady => cooldownTimer <= 0f;
        public float CooldownPercent => CurrentCooldown <= 0f ? 0f : Mathf.Clamp01(cooldownTimer / CurrentCooldown);
        public CrossClassSkillData Binding => binding;
        public EchoArchetype Archetype => binding != null ? binding.archetype : EchoArchetype.Ranged;
        public SourceClass GuestClass => binding != null ? binding.sourceClass : SourceClass.Warblade;

        private float CurrentCooldown =>
            binding != null && binding.cooldown > 0f ? binding.cooldown : defaultCooldown;

        // Build the Echo action from the binding registry (KeyBindManager) so the duplicate/reserved
        // guard covers it and rebinds via the controls UI take effect.
        private void BuildEchoAction()
        {
            echoAction = new InputAction("CrossClassEcho", InputActionType.Button,
                KeyBindManager.GetBinding(GameAction.CrossClassEcho));
        }

        // Re-create the live action from the registry after a rebind, preserving enabled state + handler.
        private void RebuildEchoAction()
        {
            if (echoAction != null)
            {
                echoAction.performed -= OnEchoPerformed;
                echoAction.Disable();
                echoAction.Dispose();
            }
            BuildEchoAction();
            echoAction.performed += OnEchoPerformed;
            echoAction.Enable();
        }

        private void OnEnable()
        {
            if (echoAction == null) BuildEchoAction();
            echoAction.performed += OnEchoPerformed;
            echoAction.Enable();
            KeyBindManager.OnBindingsChanged += RebuildEchoAction;
        }

        private void OnDisable()
        {
            KeyBindManager.OnBindingsChanged -= RebuildEchoAction;
            if (echoAction == null) return;
            echoAction.performed -= OnEchoPerformed;
            echoAction.Disable();
        }

        private void Update()
        {
            if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
        }

        /// <summary>
        /// Bind a guest Echo. Resolves + attaches the guest SkillBase on the player (DraftManager
        /// pattern) and stamps the archetype from the guest skill's tags so positioning is data-driven.
        /// </summary>
        public void Bind(CrossClassSkillData data)
        {
            if (data == null || string.IsNullOrEmpty(data.guestSkillName))
            {
                Debug.LogWarning("[Echo] Bind called with null/empty guest skill — ignored.");
                return;
            }

            var sd = SkillDatabase.Instance != null
                ? SkillDatabase.Instance.FindByName(data.guestSkillName)
                : null;
            if (sd == null || sd.skillType == null)
            {
                Debug.LogWarning($"[Echo] Guest skill '{data.guestSkillName}' not found in SkillDatabase — bind aborted.");
                return;
            }

            // Attach the guest behavior to the player root (so it resolves player/rage/resource).
            guestSkill = GetComponent(sd.skillType) as SkillBase
                      ?? gameObject.AddComponent(sd.skillType) as SkillBase;
            if (guestSkill == null)
            {
                Debug.LogWarning($"[Echo] Could not attach guest SkillBase for '{data.guestSkillName}'.");
                return;
            }

            // Stamp archetype from the guest skill's own metadata (NOT hardcoded per class).
            data.archetype = ResolveArchetype(sd.tags);
            binding = data;
            cooldownTimer = 0f;
            Debug.Log($"[Echo] Bound '{data.guestSkillName}' ({data.sourceClass}) as {data.archetype} echo.");
        }

        public void ClearBinding() => binding = null;

        private void OnEchoPerformed(InputAction.CallbackContext _) => TryActivate();

        /// <summary>Fire the Echo: spawn the silhouette actor (which performs the guest skill).</summary>
        public bool TryActivate()
        {
            if (!HasEcho || !IsReady) return false;

            var player = GetComponent<PlayerController>() ?? GetComponentInChildren<PlayerController>();
            CrossClassEcho.Spawn(this, guestSkill, binding, player);
            cooldownTimer = CurrentCooldown;
            return true;
        }

        /// <summary>Map the guest skill's SkillTags to an echo archetype (positioning rule).</summary>
        private static EchoArchetype ResolveArchetype(SkillTag[] tags)
        {
            if (tags == null) return EchoArchetype.Ranged;

            bool hasMelee = false, hasRanged = false, hasAoe = false;
            foreach (var t in tags)
            {
                if (t == SkillTag.Melee) hasMelee = true;
                else if (t == SkillTag.Ranged) hasRanged = true;
                else if (t == SkillTag.AOE || t == SkillTag.Trap) hasAoe = true;
            }

            if (hasMelee) return EchoArchetype.Melee;     // close-quarters → spawn on enemy
            if (hasRanged) return EchoArchetype.Ranged;   // projectile → over shoulder
            if (hasAoe) return EchoArchetype.Zone;        // ground hazard → at cursor
            return EchoArchetype.Ranged;
        }
    }
}
