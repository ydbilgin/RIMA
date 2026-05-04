using UnityEngine;

namespace RIMA
{
    public class WirelineTrap : RangerSkillBase
    {
        [SerializeField] private float length = 6f;
        [SerializeField] private float width = 0.5f;
        [SerializeField] private int damage = 24;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Wireline Trap";
            cooldown = 13f;
            resourceCost = 35;
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 origin = (Vector2)transform.position + dir.normalized;

            foreach (var health in SkillRuntime.EnemiesInLine(origin, dir, length, width))
            {
                SkillRuntime.DealDamage(health, FocusDamage(damage));
                health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 1.2f);
                Trap(health, 8f);
                Mark(health, 8f);
            }

            SkillRuntime.SpawnCircleVisual(origin + dir * (length * 0.5f), new Color(0.62f, 0.95f, 0.72f, 0.45f), 1.25f, 0.24f, "WirelineTrap_Runtime");
        }
    }
}
