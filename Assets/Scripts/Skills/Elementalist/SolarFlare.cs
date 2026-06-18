using UnityEngine;

namespace RIMA
{
    public class SolarFlare : SkillBase
    {
        [SerializeField] private float radius = 5f;
        [SerializeField] private float halfAngle = 38f;
        [SerializeField] private int damage = 46;

        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Solar Flare";
            cooldown = 12f;
            resourceCost = 45;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            int finalDamage = ctrl != null && ctrl.LightStateActive ? Mathf.RoundToInt(damage * 1.25f) : damage;

            SkillVfx.CastFlash(player != null ? player.gameObject : gameObject, VfxElement.Fire);

            foreach (var health in SkillRuntime.EnemiesInCone(transform.position, dir, radius, halfAngle))
                SkillRuntime.DealDamage(health, finalDamage, true,
                    player != null ? player.gameObject : null, dir, element: "light");

            ctrl?.RegisterElementCast(ElementalistElement.Light, 1);
            SkillRuntime.SpawnCircleVisual((Vector2)transform.position + dir * 2.2f, new Color(1f, 0.78f, 0.22f, 0.6f), 1.8f, 0.22f, "SolarFlare_Runtime");
            // Bright flare burst at the cone mouth.
            SkillVfx.ImpactBurst((Vector3)((Vector2)transform.position + dir * 2.2f), VfxElement.Fire);
        }
    }
}
