using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Zemine yerleştirilen hasar/efekt bölgesi.
    /// Prefab'a eklenir; skill script Init ile yapılandırır.
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class DamageZone : MonoBehaviour
    {
        private int tickDamage;
        private float tickInterval;
        private float duration;
        private StatusEffectType? slowEffect;
        private float slowDuration;
        private bool applyBurning;

        private float tickTimer;
        private float lifeTimer;

        public void Init(float duration, int tickDmg = 0, float tickInterval = 0.5f,
            StatusEffectType? slowEffect = null, float slowDuration = 1f, bool applyBurning = false)
        {
            this.duration     = duration;
            tickDamage        = tickDmg;
            this.tickInterval = tickInterval;
            this.slowEffect   = slowEffect;
            this.slowDuration = slowDuration;
            this.applyBurning = applyBurning;

            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
        }

        private void Update()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= duration) Destroy(gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player")) return;
            var hp = other.GetComponent<Health>();
            if (hp == null || hp.IsDead) return;

            tickTimer += Time.deltaTime;
            if (tickTimer < tickInterval) return;
            tickTimer = 0f;

            if (tickDamage > 0) hp.TakeDamage(tickDamage);

            var status = other.GetComponent<StatusEffectSystem>();
            if (status != null)
            {
                if (slowEffect.HasValue) status.ApplyEffect(slowEffect.Value, slowDuration);
                if (applyBurning)        status.ApplyEffect(StatusEffectType.Burning, 1f);
            }
        }
    }
}
