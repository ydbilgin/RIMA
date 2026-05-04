using UnityEngine;

namespace RIMA
{
    public class PrismBeam : SkillBase
    {
        [SerializeField] private float length = 7f;
        [SerializeField] private float width = 0.7f;
        [SerializeField] private int damage = 42;

        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Prism Beam";
            cooldown = 7f;
            resourceCost = 35;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            int lightSpent = ctrl != null ? ctrl.ConsumeLightState(3) : 0;
            int finalDamage = damage + lightSpent * 18;

            foreach (var health in SkillRuntime.EnemiesInLine(transform.position, dir, length, width))
                SkillRuntime.DealDamage(health, finalDamage);

            if (lightSpent >= 3)
            {
                foreach (var health in SkillRuntime.EnemiesInCircle((Vector2)transform.position + dir * length, 2f))
                    SkillRuntime.DealDamage(health, 24);
            }

            ctrl?.RegisterElementCast(ElementalistElement.Light, 1);
            SkillRuntime.SpawnCircleVisual((Vector2)transform.position + dir * (length * 0.5f), new Color(1f, 0.9f, 0.38f, 0.55f), 1.15f, 0.16f, "PrismBeam_Runtime");
        }
    }
}
