using System.Collections;
using UnityEngine;

namespace RIMA
{
    public class ChainCull : ShadowbladeSkillBase
    {
        [SerializeField] private float radius = 8f;
        [SerializeField] private int damage = 38;
        [SerializeField] private int maxHops = 3;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Chain Cull";
            cooldown = 10f;
            resourceCost = 25;
        }

        protected override void Execute()
        {
            StartCoroutine(CullRoutine());
        }

        private IEnumerator CullRoutine()
        {
            int hops = 0;
            while (hops < maxHops)
            {
                Health target = null;
                foreach (var health in SkillRuntime.EnemiesInCircle(transform.position, radius))
                {
                    var state = SkillRuntime.State(health);
                    if (state != null && (state.Has(SkillStateTracker.BackstabMarked) || state.Has(SkillStateTracker.RiftScar)))
                    {
                        target = health;
                        break;
                    }
                }

                if (target == null) yield break;
                SkillRuntime.DealDamage(target, damage);
                transform.position = target.transform.position;
                shadow?.AddSever(10);
                hops++;
                yield return new WaitForSeconds(0.08f);
            }
        }
    }
}
