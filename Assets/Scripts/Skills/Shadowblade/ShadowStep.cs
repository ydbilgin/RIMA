using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 4 — Shadowstep
    /// Hedefe 8m ışınlan, 0.5s stun, Energy-25.
    /// Evasion aktifken → CD sıfırlanır.
    /// </summary>
    public class ShadowStep : SkillBase
    {
        [Header("Shadowstep")]
        [SerializeField] private int   stunTime  = 0;   // float olacak, aşağıda kullan
        [SerializeField] private float stunDuration = 0.5f;
        [SerializeField] private float maxRange     = 8f;

        private bool empowered;

        public bool ConsumeEmpowerment() { bool v = empowered; empowered = false; return v; }

        private Shadowblade_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Shadow Step";
            cooldown = 7f;
            resourceCost = 25;
            ctrl = GetComponentInParent<Shadowblade_SkillController>();
        }

        protected override void Execute()
        {
            // Evasion aktifse CD sıfırla
            if (ctrl != null && ctrl.EvasionActive)
                cooldownTimer = 0f;

            var target = FindNearestEnemyInRange();
            if (target == null) return;

            var rb = GetComponentInParent<Rigidbody2D>();
            Vector2 behindPos = (Vector2)target.transform.position +
                (Vector2)(transform.position - target.transform.position).normalized * 0.8f;

            if (rb != null) rb.position = behindPos;
            else transform.position = behindPos;

            target.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, stunDuration);
            empowered = true; // Backstab için
        }

        private Collider2D FindNearestEnemyInRange()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, maxRange);
            float minD = float.MaxValue;
            Collider2D best = null;
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                if (h.GetComponent<Health>() == null) continue;
                float d = Vector2.Distance(transform.position, h.transform.position);
                if (d < minD) { minD = d; best = h; }
            }
            return best;
        }
    }
}
