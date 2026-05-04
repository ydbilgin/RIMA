using UnityEngine;

namespace RIMA
{
    public class BackstabMark : ShadowbladeSkillBase
    {
        [SerializeField] private float range = 2.2f;
        [SerializeField] private int damage = 35;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Backstab Mark";
            cooldown = 4f;
            resourceCost = 15;
        }

        protected override void Execute()
        {
            Health target = SkillRuntime.FindNearestEnemy(transform.position, range);
            if (target == null)
            {
                cooldownTimer = 0f;
                return;
            }

            bool fromStealth = shadow != null && shadow.IsInStealth;
            int finalDamage = fromStealth ? Mathf.RoundToInt(damage * 2.2f) : damage;
            SkillRuntime.DealDamage(target, finalDamage);
            ApplyBackstabMark(target);
            Scar(target);
            shadow?.AddSever(fromStealth ? 20 : 10);
            shadow?.ExitStealth();
        }
    }
}
