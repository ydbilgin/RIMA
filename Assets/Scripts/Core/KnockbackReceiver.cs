using System.Collections;
using UnityEngine;
using RIMA.Environment;

namespace RIMA
{
    /// <summary>
    /// Düşmana (veya oyuncuya) knockback uygular.
    /// PlayerAttack.ApplyHit sonrasında çağrılır.
    /// Health bileşeniyle aynı GameObject'te kullanılır.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class KnockbackReceiver : MonoBehaviour
    {
        [SerializeField] private float knockbackResistance = 0f;  // 0=tam, 1=immune
        [SerializeField] private bool isKnockdownable = true;
        [SerializeField] private KnockdownProfile defaultKnockdownProfile;

        private Rigidbody2D rb;
        private Coroutine   activeKnockback;
        private KnockdownDriver knockdownDriver;
        private Health health;

        public bool IsKnockdownable
        {
            get => isKnockdownable;
            set => isKnockdownable = value;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            knockdownDriver = KnockdownDriver.Ensure(gameObject);
        }

        /// <param name="direction">Normalize edilmiş knockback yönü (genelde saldırı yönü).</param>
        /// <param name="force">Güç (units/s).</param>
        /// <param name="duration">Knockback süresi (s).</param>
        public void ApplyKnockback(Vector2 direction, float force, float duration = 0.12f)
        {
            ApplyImpulse(new HitImpulse(direction, force, duration));
        }

        public void ApplyImpulse(HitImpulse impulse)
        {
            if (knockbackResistance >= 1f) return;
            if (health != null && health.IsDead) return;
            if (knockdownDriver != null && knockdownDriver.IsDownOrGettingUp) return;

            float actualForce = impulse.resistancePreApplied ? impulse.force : impulse.force * (1f - knockbackResistance);
            impulse.force = actualForce;

            if (activeKnockback != null) StopCoroutine(activeKnockback);
            activeKnockback = null;

            if (isKnockdownable && impulse.ShouldKnockdown(gameObject))
            {
                knockdownDriver ??= KnockdownDriver.Ensure(gameObject);
                if (knockdownDriver != null && knockdownDriver.TryStart(impulse, defaultKnockdownProfile))
                    return;
            }

            if (!impulse.HasLinearImpulse) return;
            activeKnockback = StartCoroutine(DoKnockback(impulse.direction.normalized * actualForce, impulse.duration));
        }

        public void CancelImpulse()
        {
            if (activeKnockback != null)
            {
                StopCoroutine(activeKnockback);
                activeKnockback = null;
            }

            knockdownDriver?.Cancel();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        private IEnumerator DoKnockback(Vector2 velocity, float duration)
        {
            if (duration <= 0f) yield break;

            float elapsed = 0f;
            WalkabilityMap walkMap = WalkabilityMap.Instance;
            while (elapsed < duration)
            {
                // Lineer decay: başta hızlı, sonda yavaşlar
                float t = 1f - (elapsed / duration);
                Vector2 frameVel = velocity * t;
                // Walkability clamp: stop (don't bounce) if knockback would push into void.
                // Uses the shared O(1) helper; permissive when no WalkabilityMap in scene.
                frameVel = WalkabilityMap.ClampVelocityToWalkable(walkMap, transform.position, frameVel, Time.deltaTime);
                rb.linearVelocity = frameVel;
                elapsed += Time.deltaTime;
                yield return null;
            }
            rb.linearVelocity = Vector2.zero;
            activeKnockback = null;
        }
    }
}
