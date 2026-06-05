using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Knockback component — düşmanlara vuruşta geri itme efekti.
    /// Health.cs'den çağrılır: GetComponent<KnockbackComponent>()?.ApplyKnockback(direction, force)
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(KnockbackReceiver))]
    public class KnockbackComponent : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float knockbackResistance = 0f; // 0-1, 1 = immune
        [SerializeField] private float recoveryTime = 0.2f; // Knockback sonrası kontrol geri gelme süresi

        private Rigidbody2D rb;
        private KnockbackReceiver receiver;
        private float knockbackEndTime;
        private bool isKnockedBack;

        public bool IsKnockedBack => isKnockedBack && Time.time < knockbackEndTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            EnsureReceiver();
        }

        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
            EnsureReceiver();
        }

        /// <summary>
        /// Knockback uygula.
        /// </summary>
        /// <param name="direction">Knockback yönü (normalize edilmiş)</param>
        /// <param name="force">Knockback gücü</param>
        public void ApplyKnockback(Vector2 direction, float force)
        {
            if (rb == null) return;

            // Resistance uygula
            float effectiveForce = force * (1f - knockbackResistance);
            if (effectiveForce <= 0f) return;

            // Legacy boss path forwards into the unified receiver pipeline.
            EnsureReceiver();
            if (receiver != null)
                receiver.ApplyImpulse(new HitImpulse(direction, effectiveForce, recoveryTime));

            // Knockback state
            isKnockedBack = true;
            knockbackEndTime = Time.time + recoveryTime;
        }

        /// <summary>
        /// Knockback uygula (kaynak pozisyonundan).
        /// </summary>
        /// <param name="sourcePosition">Vuruşun geldiği pozisyon</param>
        /// <param name="force">Knockback gücü</param>
        public void ApplyKnockbackFrom(Vector2 sourcePosition, float force)
        {
            Vector2 direction = ((Vector2)transform.position - sourcePosition).normalized;
            ApplyKnockback(direction, force);
        }

        private void FixedUpdate()
        {
            // Knockback bittiğinde velocity'yi yavaşça azalt
            if (isKnockedBack && Time.time >= knockbackEndTime)
            {
                isKnockedBack = false;
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.3f);
            }
        }

        /// <summary>
        /// Knockback resistance ayarla (0-1).
        /// </summary>
        public void SetResistance(float resistance)
        {
            knockbackResistance = Mathf.Clamp01(resistance);
        }

        /// <summary>
        /// Knockback'i hemen durdur.
        /// </summary>
        public void CancelKnockback()
        {
            isKnockedBack = false;
            knockbackEndTime = 0f;
            receiver?.CancelImpulse();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }

        private void EnsureReceiver()
        {
            receiver = GetComponent<KnockbackReceiver>();
            if (receiver == null)
                receiver = gameObject.AddComponent<KnockbackReceiver>();
        }
    }
}
