using System.Collections;
using UnityEngine;

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

        private Rigidbody2D rb;
        private Coroutine   activeKnockback;

        private void Awake() => rb = GetComponent<Rigidbody2D>();

        /// <param name="direction">Normalize edilmiş knockback yönü (genelde saldırı yönü).</param>
        /// <param name="force">Güç (units/s).</param>
        /// <param name="duration">Knockback süresi (s).</param>
        public void ApplyKnockback(Vector2 direction, float force, float duration = 0.12f)
        {
            if (knockbackResistance >= 1f) return;
            float actualForce = force * (1f - knockbackResistance);

            if (activeKnockback != null) StopCoroutine(activeKnockback);
            activeKnockback = StartCoroutine(DoKnockback(direction.normalized * actualForce, duration));
        }

        private IEnumerator DoKnockback(Vector2 velocity, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                // Lineer decay: başta hızlı, sonda yavaşlar
                float t = 1f - (elapsed / duration);
                rb.linearVelocity = velocity * t;
                elapsed += Time.deltaTime;
                yield return null;
            }
            rb.linearVelocity = Vector2.zero;
        }
    }
}
