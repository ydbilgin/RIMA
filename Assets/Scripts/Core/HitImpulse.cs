using System;
using UnityEngine;

namespace RIMA
{
    [Serializable]
    public struct HitImpulse
    {
        public Vector2 direction;
        public float force;
        public float duration;
        public bool canKnockdown;
        public KnockdownProfile knockdownProfile;

        public HitImpulse(Vector2 direction, float force, float duration, bool canKnockdown = false,
            KnockdownProfile knockdownProfile = null)
        {
            this.direction = direction;
            this.force = Mathf.Max(0f, force);
            this.duration = Mathf.Max(0f, duration);
            this.canKnockdown = canKnockdown;
            this.knockdownProfile = knockdownProfile;
        }

        public HitImpulse WithDirection(Vector2 newDirection)
        {
            direction = newDirection;
            return this;
        }

        public bool HasLinearImpulse => force > 0f && duration > 0f;

        public bool ShouldKnockdown(GameObject target)
        {
            if (!canKnockdown || target == null) return false;
            if (!target.TryGetComponent(out SkillStateTracker tracker)) return false;
            return tracker.Has(SkillStateTracker.Broken) || tracker.Has(SkillStateTracker.Sundered);
        }

        public KnockdownProfile ResolveProfile(KnockdownProfile fallback)
        {
            return knockdownProfile != null ? knockdownProfile : fallback != null ? fallback : KnockdownProfile.Default;
        }
    }
}
