using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 2 — Glacial Spike
    /// 6m buz hattı: hattaki düşmanlar %40 slow + %180 hasar. Frost State+2.
    /// Fireball DoT aktif hedefe → Freeze 2s + DoT hasarı tek seferde patlar.
    /// </summary>
    public class GlacialSpike : SkillBase
    {
        [Header("Glacial Spike")]
        [SerializeField] private float lineLength = 6f;
        [SerializeField] private float lineWidth  = 0.6f;
        [SerializeField] private int   damage     = 45;
        [SerializeField] private float slowDuration = 2.5f;

        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Glacial Spike";
            cooldown = 5f;
            resourceCost = 25;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 origin = transform.position;

            var hits = Physics2D.BoxCastAll(origin + dir * (lineLength * 0.5f),
                new Vector2(lineWidth, lineLength), Vector2.SignedAngle(Vector2.up, dir), Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player")) continue;
                var hp = hit.collider.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                int dmg = damage;
                var status = hit.collider.GetComponent<StatusEffectSystem>();
                if (status != null)
                {
                    // Fireball DoT aktifse → Freeze + DoT burst
                    if (status.GetStacks(StatusEffectType.Burning) > 0)
                    {
                        status.ApplyEffect(StatusEffectType.Frozen, 2f);
                        dmg = Mathf.RoundToInt(damage * 1.5f);
                    }
                    else
                    {
                        status.ApplyEffect(StatusEffectType.Chill, slowDuration);
                    }
                }
                hp.TakeDamage(dmg);
            }

            ctrl?.RegisterElementCast(ElementalistElement.Frost, 2);
            ctrl?.ConsumeFireState(1);
        }
    }
}
