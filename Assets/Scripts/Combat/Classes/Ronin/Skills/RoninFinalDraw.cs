using UnityEngine;

namespace RIMA
{
    public class RoninFinalDraw : SkillBase
    {
        [SerializeField] private float coneRadius = 5.2f;
        [SerializeField] private float coneHalfAngle = 42f;
        [SerializeField] private int baseDamage = 30;
        [SerializeField] private float damagePerTension = 1.25f;

        private TensionSystem tension;

        protected override void Awake()
        {
            base.Awake();
            tension = GetComponentInParent<TensionSystem>();
            skillName = "Final Draw";
            cooldown = 12f;
            resourceCost = 0;
        }

        protected override void Execute()
        {
            if (tension == null) tension = GetComponentInParent<TensionSystem>();
            int spent = tension != null ? tension.SpendAll() : 0;
            if (spent <= 0) return;

            Vector2 origin = transform.position;
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            int damage = Mathf.RoundToInt(baseDamage + spent * damagePerTension);

            foreach (var health in SkillRuntime.EnemiesInCone(origin, dir, coneRadius, coneHalfAngle))
            {
                SkillRuntime.DealDamage(health, damage);
                SkillRuntime.State(health)?.Apply("Opened", 6f, 1, 2);
            }

            SkillRuntime.SpawnCircleVisual(origin + dir.normalized * 2.1f, new Color(0.9f, 0.94f, 1f, 0.68f), 1.8f, 0.18f, "Ronin_FinalDraw");
            HitStop.Instance?.FreezeMedium();
            LightPulse.Emit(new Color(0.58f, 0.88f, 1f), 1.25f, 0.08f);
        }
    }
}
