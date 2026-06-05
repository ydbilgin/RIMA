using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Düşmana vurulduğunda kısa bir zaman dondurma (hitstop) uygular.
    /// Revize: Intensity-based — normal hit, heavy hit, kill için farklı süreler.
    /// PlayerAttack.cs bu singleton'ı çağırır: HitStop.Instance.Freeze(duration)
    /// </summary>
    [System.Obsolete("Use HitPauseDriver — single timeScale owner")]
    public class HitStop : MonoBehaviour
    {
        public static HitStop Instance { get; private set; }

        [Header("Durations")]
        [SerializeField] private float lightHitDuration = 0.03f;   // Normal vuruş
        [SerializeField] private float mediumHitDuration = 0.06f;  // Ağır vuruş, skill
        [SerializeField] private float heavyHitDuration = 0.10f;   // Kill, critical
        [SerializeField] private float explosionDuration = 0.15f;  // Boss death, explosion

        private Coroutine stopCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
        }

        /// <summary>Time.timeScale'i sıfırlar, ardından geri yükler.</summary>
        public void Freeze(float duration)
        {
            if (stopCoroutine != null) StopCoroutine(stopCoroutine);
            stopCoroutine = StartCoroutine(DoFreeze(duration));
        }

        /// <summary>Hafif vuruş hitstop (normal hit).</summary>
        public void FreezeLight()
        {
            Freeze(lightHitDuration);
        }

        /// <summary>Orta vuruş hitstop (heavy hit, skill).</summary>
        public void FreezeMedium()
        {
            Freeze(mediumHitDuration);
        }

        /// <summary>Güçlü hitstop (kill, critical).</summary>
        public void FreezeHeavy()
        {
            Freeze(heavyHitDuration);
        }

        /// <summary>Maksimum hitstop (boss death, explosion).</summary>
        public void FreezeExplosion()
        {
            Freeze(explosionDuration);
        }

        private IEnumerator DoFreeze(float duration)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
        }
    }
}
