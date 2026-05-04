using UnityEngine;

namespace RIMA
{
    public class ShadowPin : ShadowbladeSkillBase
    {
        [SerializeField] private int damage = 24;
        [SerializeField] private float speed = 15f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Shadow Pin";
            cooldown = 6f;
            resourceCost = 20;
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var projectile = SkillRuntime.SpawnProjectile(transform.position, dir, speed, damage, new Color(0.56f, 0.24f, 0.86f, 0.95f), 0.25f, 3f, "ShadowPin_Runtime");
            projectile.SetOnHit(hit =>
            {
                var health = hit.GetComponent<Health>();
                if (health == null) return;
                health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 1.5f);
                Scar(health);
            });
        }
    }
}
