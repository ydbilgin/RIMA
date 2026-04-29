using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 2 — Concussive Arrow
    /// Knockback 4m + root 2s.
    /// Disengage sırasında → uzaklık 6m + slow 3s.
    /// </summary>
    public class ConcussiveArrow : SkillBase
    {
        [Header("Concussive Arrow")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 14f;
        [SerializeField] private int   damage          = 25;
        [SerializeField] private float rootDuration    = 2f;
        [SerializeField] private float knockbackForce  = 12f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Concussive Arrow";
            cooldown = 6f;
            resourceCost = 15;
        }

        protected override void Execute()
        {
            if (projectilePrefab == null) return;
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var proj = go.GetComponent<PlayerProjectile>();
            proj?.Init(dir * projectileSpeed, damage, life: 4f, knockback: Mathf.RoundToInt(knockbackForce));

            // Root on hit is handled via a ConcussiveArrowHit component
            var hit = go.AddComponent<OnHitApplyEffect>();
            hit.Init(StatusEffectType.Stunned, rootDuration);
        }
    }

    /// <summary> Projektile'e eklenen tek seferlik efekt uygulayıcısı. </summary>
    public class OnHitApplyEffect : MonoBehaviour
    {
        private StatusEffectType effectType;
        private float duration;

        public void Init(StatusEffectType t, float d) { effectType = t; duration = d; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) return;
            other.GetComponent<StatusEffectSystem>()?.ApplyEffect(effectType, duration);
        }
    }
}
