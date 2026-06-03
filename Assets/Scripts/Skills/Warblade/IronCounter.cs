using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Iron Counter
    /// 0.8s pencere: aktif edilince oyuncu hasar alırsa → yakın düşmana %180 karşı saldırı
    /// + Rage+25 + 0.5s stun.
    /// Chain: Rage 80+ → 2× tetiklenir.
    /// </summary>
    public class IronCounter : SkillBase
    {
        [Header("Iron Counter")]
        [SerializeField] private float parryWindow = 0.8f;
        [SerializeField] private float counterRange = 3f;
        [SerializeField] private int baseDamage = 35;
        [SerializeField] private float damageMultiplier = 1.8f;
        [SerializeField] private int rageOnCounter = 25;
        [SerializeField] private float stunDuration = 0.5f;

        private Health playerHealth;
        private bool windowActive;
        private int prevHP;
        private int counterCharges;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Iron Counter";
            cooldown = 8f;
            rageCost = 0;
            playerHealth = GetComponentInParent<Health>();
        }

        protected override void Execute()
        {
            counterCharges = (rage != null && rage.CurrentRage >= 80) ? 2 : 1;
            StartCoroutine(ParryWindow());
        }

        private IEnumerator ParryWindow()
        {
            windowActive = true;
            prevHP = playerHealth != null ? playerHealth.CurrentHP : 0;
            float elapsed = 0f;

            while (elapsed < parryWindow && counterCharges > 0)
            {
                elapsed += Time.deltaTime;

                if (playerHealth != null && playerHealth.CurrentHP < prevHP)
                {
                    TriggerCounter();
                    counterCharges--;
                    prevHP = playerHealth.CurrentHP;
                }

                yield return null;
            }

            windowActive = false;
        }

        private void TriggerCounter()
        {
            int damage = Mathf.RoundToInt(baseDamage * damageMultiplier);

            var hits = Physics2D.OverlapCircleAll(transform.position, counterRange, LayerMask.GetMask("Enemy"));
            float best = float.MaxValue;
            Health target = null;

            foreach (var h in hits)
            {
                if (h.gameObject == player.gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                float d = Vector2.Distance(transform.position, h.transform.position);
                if (d < best) { best = d; target = hp; }
            }

            if (target == null) return;

            SkillRuntime.DealDamage(target, damage, this);
            rage?.AddRage(rageOnCounter);

            var status = target.GetComponent<StatusEffectSystem>();
            status?.ApplyEffect(StatusEffectType.Stunned, stunDuration, 1);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = windowActive
                ? new Color(1f, 1f, 0f, 0.5f)
                : new Color(0.5f, 0.5f, 0f, 0.2f);
            Gizmos.DrawWireSphere(transform.position, counterRange);
        }
    }
}
