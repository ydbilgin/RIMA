using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RIMA
{
    /// <summary>
    /// Skill ve hit event'leri tarafından tetiklenen Light2D flash sistemi.
    ///
    /// Kullanım (herhangi bir script'ten):
    ///   LightPulse.Emit(Color.blue, 1.5f, 0.12f);
    ///
    /// Bu component'i Light2D olan bir GameObject'e ekle (örn. PlayerLight).
    /// Sahnede birden fazla LightPulse olabilir — hepsi aynı anda yanıt verir.
    /// HitStop sırasında da çalışır (WaitForSecondsRealtime).
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    public class LightPulse : MonoBehaviour
    {
        // ─── Static Event ─────────────────────────────────────────────────────

        /// <summary>
        /// Herhangi bir yerden çağır → tüm aktif LightPulse bileşenleri yanıt verir.
        /// </summary>
        public static event Action<Color, float, float> OnEmit;

        /// <param name="color">Pulse rengi</param>
        /// <param name="intensity">Pulse yoğunluğu</param>
        /// <param name="duration">Saniye (unscaled — HitStop'tan bağımsız)</param>
        public static void Emit(Color color, float intensity, float duration)
            => OnEmit?.Invoke(color, intensity, duration);

        // ─── Instance ─────────────────────────────────────────────────────────

        private Light2D  light2D;
        private Coroutine pulseCoroutine;
        private float    originalIntensity;
        private Color    originalColor;

        private void Awake()
        {
            light2D           = GetComponent<Light2D>();
            originalIntensity = light2D.intensity;
            originalColor     = light2D.color;
        }

        private void OnEnable()  => OnEmit += HandleEmit;
        private void OnDisable() => OnEmit -= HandleEmit;

        private void HandleEmit(Color color, float intensity, float duration)
        {
            if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);
            pulseCoroutine = StartCoroutine(DoPulse(color, intensity, duration));
        }

        private IEnumerator DoPulse(Color color, float intensity, float duration)
        {
            light2D.color     = color;
            light2D.intensity = intensity;

            yield return new WaitForSecondsRealtime(duration);

            light2D.color     = originalColor;
            light2D.intensity = originalIntensity;
            pulseCoroutine    = null;
        }
    }
}
