using System.Collections;
using System.Collections.Generic;
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
        private readonly List<Coroutine> _trackedCoroutines = new();

        // ── Feature B (Shadow Echo) origin/aim override ───────────────────────────
        // When a CrossClassEcho performs this skill FROM the silhouette's position, it sets a
        // transient override before calling Execute(), then clears it. Echo-capable skills read
        // SkillOrigin / SkillAim instead of transform.position / player.FacingDirection so the
        // SAME damage/break logic fires from the echo's location. Default (no override active) =
        // current behavior, so non-echo callers are unaffected.
        private bool _hasOriginOverride;
        private Vector3 _originOverride;
        private Vector2 _aimOverride;

        /// <summary>World origin the skill should fire FROM. Defaults to the player (transform.position);
        /// overridden while an Echo is performing the skill. Echo-capable skills MUST use this.</summary>
        protected Vector3 SkillOrigin => _hasOriginOverride ? _originOverride : transform.position;

        /// <summary>Aim/facing direction. Defaults to player.FacingDirection; overridden while an Echo
        /// performs the skill (aim toward the echo's target). Echo-capable skills MUST use this.</summary>
        protected Vector2 SkillAim
        {
            get
            {
                if (_hasOriginOverride && _aimOverride.sqrMagnitude > 0.0001f) return _aimOverride.normalized;
                return player != null ? player.FacingDirection : Vector2.right;
            }
        }

        /// <summary>True while this skill is being performed by an Echo (origin/aim overridden).</summary>
        protected bool IsEchoPerforming => _hasOriginOverride;

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
            Debug.Log($"[Cast] {skillName} ({(player != null ? player.name : name)})");
            return true;
        }

        public void ForceReady() => cooldownTimer = 0f;

        /// <summary>
        /// Feature B (B3 crux) — perform this skill's effect FROM an external origin/aim, used by
        /// <see cref="CrossClassEcho"/> so a guest skill fires from the silhouette's position. Runs the
        /// SAME <see cref="Execute"/> (identical damage/break/state logic, layer "Enemy"); only the
        /// origin/aim are overridden. Bypasses cost/cooldown gating (the Echo manages its own "guest
        /// favor" cooldown). NOT cosmetic — this is the real hit. Additive + opt-in: skills that have
        /// not been migrated to read SkillOrigin/SkillAim will simply fire from the player as before
        /// (the echo flags those as fallback candidates).
        /// </summary>
        public void ExecuteAt(Vector3 origin, Vector2 aim)
        {
            if (player == null) player = GetComponentInParent<PlayerController>();
            if (rage == null) rage = GetComponentInParent<RageSystem>();
            resource = ResolvePreferredResource();
            if (flowTracker == null) flowTracker = GetComponentInParent<SkillFlowTracker>();

            _hasOriginOverride = true;
            _originOverride = origin;
            _aimOverride = aim;
            try { Execute(); }
            finally { _hasOriginOverride = false; }
        }

        /// <summary>True if this skill type honors an origin override (reads SkillOrigin/SkillAim) so an
        /// Echo can fire it from afar / on the enemy. Self-positioning skills (Blink, dashes) return
        /// false — the echo must fall back for them. Curated demo list; see CrossClassEcho audit.</summary>
        public virtual bool SupportsEchoOrigin => false;

        protected Coroutine RegisterCoroutine(IEnumerator routine)
        {
            var c = StartCoroutine(routine);
            _trackedCoroutines.Add(c);
            return c;
        }

        protected void CancelTrackedCoroutines()
        {
            foreach (var c in _trackedCoroutines)
                if (c != null) StopCoroutine(c);
            _trackedCoroutines.Clear();
        }

        protected abstract void Execute();

        private PlayerResourceBase ResolvePreferredResource()
        {
            if (GetComponentInParent<Elementalist_SkillController>() != null)
                return GetComponentInParent<ManaSystem>();
            if (GetComponentInParent<Shadowblade_SkillController>() != null)
                return GetComponentInParent<EnergySystem>();
            if (GetComponentInParent<Ranger_SkillController>() != null)
                return GetComponentInParent<FocusSystem>();
            if (this is RoninQuickdraw || this is RoninIaidoStance ||
                this is RoninFinalDraw || this is RoninSakuraVeil)
                return GetComponentInParent<TensionSystem>();

            return GetComponentInParent<PlayerResourceBase>();
        }
    }
}
