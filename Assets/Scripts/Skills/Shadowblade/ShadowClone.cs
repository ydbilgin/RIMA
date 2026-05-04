using System.Collections;
using UnityEngine;

namespace RIMA
{
    public class ShadowClone : ShadowbladeSkillBase
    {
        [SerializeField] private float duration = 5f;
        [SerializeField] private float pulseRadius = 2.2f;
        [SerializeField] private int pulseDamage = 18;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Shadow Clone";
            cooldown = 14f;
            resourceCost = 35;
        }

        protected override void Execute()
        {
            StartCoroutine(CloneRoutine(transform.position));
            shadow?.AddSever(10);
        }

        private IEnumerator CloneRoutine(Vector2 position)
        {
            var visual = SkillRuntime.SpawnCircleVisual(position, new Color(0.45f, 0.22f, 0.72f, 0.48f), 0.75f, duration, "ShadowClone_Runtime");
            float elapsed = 0f;
            while (elapsed < duration)
            {
                foreach (var health in SkillRuntime.EnemiesInCircle(position, pulseRadius))
                {
                    SkillRuntime.DealDamage(health, pulseDamage);
                    Scar(health, 6f);
                }

                yield return new WaitForSeconds(1f);
                elapsed += 1f;
                if (visual == null) yield break;
            }
        }
    }
}
