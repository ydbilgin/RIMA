using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 7 — Meteor
    /// 1.5s kanal → büyük AoE knockdown.
    /// Frozen/slowed hedef → knockdown 3s + hasar +%50.
    /// </summary>
    public class Meteor : SkillBase
    {
        [Header("Meteor")]
        [SerializeField] private float channelTime  = 1.5f;
        [SerializeField] private float impactRadius = 3f;
        [SerializeField] private int   damage       = 90;
        [SerializeField] private float knockdownTime = 1.5f;
        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Meteor";
            cooldown = 14f;
            resourceCost = 60;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute() => StartCoroutine(MeteorRoutine());

        private IEnumerator MeteorRoutine()
        {
            // Kanal — animasyon trigger burada çekilebilir
            yield return new WaitForSeconds(channelTime);
            ctrl?.RegisterElementCast(ElementalistElement.Fire, 1);

            Vector2 target = player != null
                ? (Vector2)player.transform.position + player.FacingDirection * 4f
                : (Vector2)transform.position;

            var hits = Physics2D.OverlapCircleAll(target, impactRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player")) continue;
                var hp = hit.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                int dmg = damage;
                float kd = knockdownTime;

                var status = hit.GetComponent<StatusEffectSystem>();
                if (status != null &&
                    (status.GetStacks(StatusEffectType.Frozen) > 0 || status.GetStacks(StatusEffectType.Chill) > 0))
                {
                    dmg = Mathf.RoundToInt(damage * 1.5f);
                    kd = 3f;
                }

                SkillRuntime.DealDamage(hp, dmg, this);
                status?.ApplyEffect(StatusEffectType.Stunned, kd);

                var rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir = (hit.transform.position - (Vector3)target).normalized;
                    rb.AddForce(dir * 5f, ForceMode2D.Impulse);
                }
            }
        }
    }
}
