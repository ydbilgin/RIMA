using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 12 — Point Blank
    /// Yakın mesafe ok patlaması: 1.5m yarıçap konik AoE, yüksek hasar.
    /// Focus düşükken (yakına sıkışınca) otomatik ForceReady → ceza yok.
    /// </summary>
    public class PointBlank : SkillBase
    {
        [Header("Point Blank")]
        [SerializeField] private int   damage        = 70;
        [SerializeField] private float coneRadius    = 1.8f;
        [SerializeField] private float coneHalfAngle = 60f;
        [SerializeField] private int   knockback     = 12;
        [SerializeField] private float lowFocusThreshold = 25f;

        protected override void Awake()
        {
            base.Awake();
            skillName    = "Point Blank";
            cooldown     = 8f;
            resourceCost = 20;
        }

        private void Update()
        {
            // If Focus is critically low, auto-reset cooldown (trapped at close range)
            if (resource is FocusSystem focus && focus.Current <= lowFocusThreshold)
                ForceReady();
        }

        protected override void Execute()
        {
            Vector2 origin  = transform.position;
            Vector2 forward = player != null ? player.FacingDirection : Vector2.right;

            var hits = Physics2D.OverlapCircleAll(origin, coneRadius);
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;

                Vector2 toEnemy = ((Vector2)h.transform.position - origin).normalized;
                float angle = Vector2.Angle(forward, toEnemy);
                if (angle > coneHalfAngle) continue;

                h.GetComponent<Health>()?.TakeDamage(damage);

                if (knockback > 0)
                {
                    var rb = h.GetComponent<Rigidbody2D>();
                    if (rb != null) rb.AddForce(toEnemy * knockback, ForceMode2D.Impulse);
                }
            }
        }
    }
}
