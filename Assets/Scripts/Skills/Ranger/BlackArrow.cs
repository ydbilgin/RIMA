using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 7 — Black Arrow
    /// Zehirli ok: anında 4 stack Poison + küçük ani hasar.
    /// Frozen veya Chill'li hedefe çarparsa Weakened de ekler.
    /// </summary>
    public class BlackArrow : SkillBase
    {
        [Header("Black Arrow")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 20f;
        [SerializeField] private int   directDamage    = 25;
        [SerializeField] private float poisonDuration  = 8f;
        [SerializeField] private int   poisonStacks    = 4;
        [SerializeField] private float weakenDuration  = 4f;

        protected override void Awake()
        {
            base.Awake();
            skillName    = "Black Arrow";
            cooldown     = 12f;
            resourceCost = 30;
        }

        protected override void Execute()
        {
            if (projectilePrefab == null) return;
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var go   = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var proj = go.GetComponent<PlayerProjectile>();
            proj?.Init(dir * projectileSpeed, directDamage, life: 5f,
                       applyPoison: true, poisonDuration: poisonDuration);

            // Intercept OnTrigger to apply bonus stacks + Weakened
            var extra = go.AddComponent<BlackArrowExtra>();
            extra.Init(poisonStacks - 1, weakenDuration);
        }
    }

    /// <summary>Extra stacks + Weakened on hit — piggybacked onto the projectile GO.</summary>
    internal class BlackArrowExtra : MonoBehaviour
    {
        private int extraPoisonStacks;
        private float weakenDuration;
        private bool hit;

        public void Init(int poison, float weaken)
        {
            extraPoisonStacks = poison;
            weakenDuration    = weaken;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (hit) return;
            if (other.CompareTag("Player")) return;
            var status = other.GetComponent<StatusEffectSystem>();
            if (status == null) return;

            hit = true;

            // Extra poison stacks
            for (int i = 0; i < extraPoisonStacks; i++)
                status.ApplyEffect(StatusEffectType.Poison, 8f);

            // Weakened bonus on chilled/frozen targets
            if (status.HasEffect(StatusEffectType.Chill) || status.HasEffect(StatusEffectType.Frozen))
                status.ApplyEffect(StatusEffectType.Weakened, weakenDuration);
        }
    }
}
