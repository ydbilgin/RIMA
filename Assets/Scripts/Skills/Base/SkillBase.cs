using UnityEngine;

namespace RIMA
{
    public abstract class SkillBase : MonoBehaviour
    {
        [Header("Skill Info")]
        public string skillName = "Unnamed Skill";
        public Sprite icon;
        public float cooldown = 1f;

        [Header("Cost")]
        public int rageCost = 0;        // Warblade rage cost
        public int resourceCost = 0;    // Elementalist/Shadowblade/Ranger generic cost

        protected float cooldownTimer;
        protected PlayerController player;
        protected RageSystem rage;
        protected PlayerResourceBase resource;

        public bool IsReady => cooldownTimer <= 0f;
        public float CooldownPercent => Mathf.Clamp01(cooldownTimer / cooldown);
        public float RemainingCooldown => Mathf.Max(0f, cooldownTimer);

        protected SkillFlowTracker flowTracker;

        protected virtual void Awake()
        {
            player      = GetComponentInParent<PlayerController>();
            rage        = GetComponentInParent<RageSystem>();
            resource    = ResolvePreferredResource();
            flowTracker = GetComponentInParent<SkillFlowTracker>();
        }

        private void Update()
        {
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;
        }

        public bool TryActivate()
        {
            if (!IsReady) return false;
            if (player == null) player = GetComponentInParent<PlayerController>();
            if (rage == null) rage = GetComponentInParent<RageSystem>();
            resource = ResolvePreferredResource();
            if (flowTracker == null) flowTracker = GetComponentInParent<SkillFlowTracker>();
            if (rageCost > 0 && (rage == null || !rage.TrySpend(rageCost))) return false;
            if (resourceCost > 0 && (resource == null || !resource.TrySpend(resourceCost))) return false;
            player?.FaceCombatTarget();
            Execute();
            cooldownTimer = cooldown;
            flowTracker?.NotifySkillUsed(this);
            return true;
        }

        public void ForceReady() => cooldownTimer = 0f;

        protected abstract void Execute();

        private PlayerResourceBase ResolvePreferredResource()
        {
            if (GetComponentInParent<Elementalist_SkillController>() != null)
                return GetComponentInParent<ManaSystem>();
            if (GetComponentInParent<Shadowblade_SkillController>() != null)
                return GetComponentInParent<EnergySystem>();
            if (GetComponentInParent<Ranger_SkillController>() != null)
                return GetComponentInParent<FocusSystem>();

            return GetComponentInParent<PlayerResourceBase>();
        }
    }
}
