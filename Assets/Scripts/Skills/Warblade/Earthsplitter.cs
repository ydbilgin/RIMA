using System.Collections;
using UnityEngine;

namespace RIMA
{
    public class Earthsplitter : SkillBase
    {
        [SerializeField] private float length = 3f;
        [SerializeField] private float width = 1f;
        [SerializeField] private int damage = 34;
        [SerializeField] private int rageOnUse = 25;
        [SerializeField] private float stunDuration = 2f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Earthsplitter";
            cooldown = 10f;
            rageCost = 0;
        }

        protected override void Execute()
        {
            StartCoroutine(WaveRoutine());
        }

        private IEnumerator WaveRoutine()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            for (int wave = 0; wave < 3; wave++)
            {
                Vector2 origin = (Vector2)transform.position + dir * (wave * 1.2f);
                foreach (var health in SkillRuntime.EnemiesInLine(origin, dir, length, width))
                {
                    SkillRuntime.DealDamage(health, damage);
                    health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, stunDuration);
                    SkillRuntime.State(health)?.Apply(SkillStateTracker.Broken, 6f, 1, 3);
                }

                SkillRuntime.SpawnCircleVisual(origin + dir * (length * 0.5f), new Color(0.82f, 0.68f, 0.42f, 0.55f), 1.05f, 0.16f, "Earthsplitter_Wave");
                yield return new WaitForSeconds(0.08f);
            }

            rage?.AddRage(rageOnUse);
        }
    }
}
