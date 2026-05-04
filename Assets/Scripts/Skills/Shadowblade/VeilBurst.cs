using System.Collections;
using UnityEngine;

namespace RIMA
{
    public class VeilBurst : ShadowbladeSkillBase
    {
        [SerializeField] private float radius = 5f;
        [SerializeField] private int damage = 30;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Veil Burst";
            cooldown = 11f;
            resourceCost = 30;
        }

        protected override void Execute()
        {
            StartCoroutine(BurstRoutine());
        }

        private IEnumerator BurstRoutine()
        {
            for (int i = 0; i < 4; i++)
            {
                Health target = SkillRuntime.FindNearestEnemy(transform.position, radius);
                if (target == null) yield break;
                SkillRuntime.DealDamage(target, damage);
                Scar(target);
                transform.position = (Vector2)target.transform.position + Random.insideUnitCircle.normalized * 0.8f;
                shadow?.AddSever(8);
                yield return new WaitForSeconds(0.08f);
            }
        }
    }
}
