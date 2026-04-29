using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 1 — Backstab ★
    /// Arkadan: %200 hasar + 3 CP. Önden: normal hasar.
    /// Shadowstep sonrası → +%50 hasar.
    /// </summary>
    public class Backstab : SkillBase
    {
        [Header("Backstab")]
        [SerializeField] private int   baseDamage  = 35;
        [SerializeField] private float attackRange = 1.5f;

        private ComboPointSystem combo;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Backstab";
            cooldown = 1.5f;
            resourceCost = 0;
            combo = GetComponentInParent<ComboPointSystem>();
        }

        protected override void Execute()
        {
            var target = FindNearestEnemy();
            if (target == null) return;

            bool fromBehind = IsFromBehind(target);
            int dmg = fromBehind ? Mathf.RoundToInt(baseDamage * 2f) : baseDamage;
            int cpGain = fromBehind ? 3 : 1;

            // Shadowstep empowerment
            var ss = GetComponentInParent<ShadowStep>();
            if (ss != null && ss.ConsumeEmpowerment())
                dmg = Mathf.RoundToInt(dmg * 1.5f);

            var hp = target.GetComponent<Health>();
            hp?.TakeDamage(dmg);
            combo?.Add(cpGain);
        }

        private bool IsFromBehind(Collider2D target)
        {
            // Behind = player position is in the rear 120° arc of the enemy
            Vector2 toPlayer = (Vector2)transform.position - (Vector2)target.transform.position;
            var enemyAI = target.GetComponent<EnemyAI>();
            if (enemyAI == null) return false;
            // Use enemy's movement direction as facing
            var enemyRb = target.GetComponent<Rigidbody2D>();
            if (enemyRb == null || enemyRb.linearVelocity.sqrMagnitude < 0.01f) return false;
            Vector2 enemyFacing = enemyRb.linearVelocity.normalized;
            return Vector2.Dot(toPlayer.normalized, -enemyFacing) > 0.5f;
        }

        private Collider2D FindNearestEnemy()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
            float minD = float.MaxValue;
            Collider2D best = null;
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                if (h.GetComponent<Health>() == null) continue;
                float d = Vector2.Distance(transform.position, h.transform.position);
                if (d < minD) { minD = d; best = h; }
            }
            return best;
        }
    }
}
