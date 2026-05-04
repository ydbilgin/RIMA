using UnityEngine;

namespace RIMA
{
    public class PinningShot : RangerSkillBase
    {
        [SerializeField] private int damage = 32;
        [SerializeField] private float speed = 16f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Pinning Shot";
            cooldown = 5f;
            resourceCost = 15;
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var projectile = SkillRuntime.SpawnProjectile(transform.position, dir, speed, FocusDamage(damage), new Color(0.44f, 0.92f, 0.46f, 0.95f), 0.28f, 3f, "PinningShot_Runtime");
            projectile.SetOnHit(hit =>
            {
                var health = hit.GetComponent<Health>();
                if (health == null) return;
                health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 1.5f);
                Trap(health, 3f);
                Mark(health);
            });
        }
    }
}
