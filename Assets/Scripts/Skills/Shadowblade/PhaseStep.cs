using UnityEngine;

namespace RIMA
{
    public class PhaseStep : ShadowbladeSkillBase
    {
        [SerializeField] private float distance = 4f;
        [SerializeField] private int damage = 24;

        private Rigidbody2D rb;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponentInParent<Rigidbody2D>();
            skillName = "Phase Step";
            cooldown = 5f;
            resourceCost = 20;
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 start = transform.position;
            foreach (var health in SkillRuntime.EnemiesInLine(start, dir, distance, 0.8f))
            {
                SkillRuntime.DealDamage(health, damage);
                Scar(health);
            }

            Vector2 end = start + dir.normalized * distance;
            if (rb != null) rb.position = end;
            else transform.position = end;
            shadow?.EnterStealth(0.3f);
            shadow?.AddSever(12);
        }
    }
}
