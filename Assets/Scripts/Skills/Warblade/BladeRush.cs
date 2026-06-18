using UnityEngine;

namespace RIMA
{
    public class BladeRush : SkillBase
    {
        [Header("Blade Rush")]
        [SerializeField] private float chargeSpeed = 20f;
        [SerializeField] private float chargeDuration = 0.28f;
        [SerializeField] private float hitRadius = 0.7f;
        [SerializeField] private int damage = 48;
        [SerializeField] private float knockbackForce = 6f;
        [SerializeField] private int ragePerHit = 15;

        private Rigidbody2D rb;
        private bool charging;
        private float chargeTimer;
        private Vector2 chargeDir;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponentInParent<Rigidbody2D>();
            skillName = "Blade Rush";
            cooldown = 10f;
            rageCost = 0;
        }

        protected override void Execute()
        {
            chargeDir = player.FacingDirection;
            charging = true;
            chargeTimer = chargeDuration;
        }

        private void FixedUpdate()
        {
            if (!charging) return;
            chargeTimer -= Time.fixedDeltaTime;
            // FIX (off-map): clamp the rush velocity to walkable cells so the dash cannot strand
            // the player in the void. Mirrors PlayerController.FixedUpdate / KnockbackReceiver;
            // ClampVelocityToWalkable is permissive (returns desired) when no WalkabilityMap exists.
            Vector2 rushVel = RIMA.Environment.WalkabilityMap.ClampVelocityToWalkable(
                RIMA.Environment.WalkabilityMap.Instance, rb.position, chargeDir * chargeSpeed, Time.fixedDeltaTime);
            rb.linearVelocity = rushVel;

            var hits = Physics2D.CircleCastAll(rb.position, hitRadius, chargeDir, 0.05f, LayerMask.GetMask("Enemy"));
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == rb.gameObject) continue;
                var hp = hit.collider.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;
                SkillRuntime.DealDamage(hp, damage, this);
                rage?.AddRage(ragePerHit);
                var erb = hit.collider.GetComponent<Rigidbody2D>();
                if (erb != null) erb.AddForce(chargeDir * knockbackForce, ForceMode2D.Impulse);
            }

            if (chargeTimer <= 0f) charging = false;
        }
    }
}
