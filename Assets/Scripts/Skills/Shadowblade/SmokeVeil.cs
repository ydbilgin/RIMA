using UnityEngine;

namespace RIMA
{
    public class SmokeVeil : ShadowbladeSkillBase
    {
        [SerializeField] private float duration = 4f;
        [SerializeField] private float radius = 3f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Smoke Veil";
            cooldown = 13f;
            resourceCost = 30;
        }

        protected override void Execute()
        {
            shadow?.EnterStealth(duration);
            SkillRuntime.State(gameObject)?.Apply(SkillStateTracker.SmokeVeiled, duration, 1, 1);
            foreach (var health in SkillRuntime.EnemiesInCircle(transform.position, radius))
                health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Chill, 2f);
            SkillRuntime.SpawnCircleVisual(transform.position, new Color(0.35f, 0.22f, 0.46f, 0.45f), 1.8f, duration, "SmokeVeil_Runtime");
        }
    }
}
