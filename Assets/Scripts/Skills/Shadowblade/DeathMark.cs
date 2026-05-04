using System.Collections;
using UnityEngine;

namespace RIMA
{
    public class DeathMark : ShadowbladeSkillBase
    {
        [SerializeField] private float range = 7f;
        [SerializeField] private float delay = 4f;
        [SerializeField] private int explosionDamage = 70;
        [SerializeField] private float radius = 2.2f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Death Mark";
            cooldown = 9f;
            resourceCost = 25;
        }

        protected override void Execute()
        {
            Health target = SkillRuntime.FindNearestEnemy(transform.position, range);
            if (target == null)
            {
                cooldownTimer = 0f;
                return;
            }

            SkillRuntime.State(target)?.Apply(SkillStateTracker.DeathMarked, delay, 1, 1);
            StartCoroutine(ExplodeLater(target));
        }

        private IEnumerator ExplodeLater(Health target)
        {
            yield return new WaitForSeconds(delay);
            if (target == null) yield break;
            Vector2 center = target.transform.position;
            foreach (var health in SkillRuntime.EnemiesInCircle(center, radius))
                SkillRuntime.DealDamage(health, explosionDamage);
            SkillRuntime.SpawnCircleVisual(center, new Color(0.5f, 0.18f, 0.78f, 0.55f), 1.35f, 0.22f, "DeathMark_Runtime");
        }
    }
}
