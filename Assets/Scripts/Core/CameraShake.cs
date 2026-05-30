using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Kamera sallama. CameraFollow.LateUpdate tarafından okunur.
    /// CameraShake.Instance.Shake(intensity, duration) çağır.
    /// </summary>
    [System.Obsolete("Not the live spine - see WORK_ORDER_24_48H_S6. Live shake = VFX/ScreenShakeDriver.")]
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance { get; private set; }

        /// <summary>CameraFollow bu değeri pozisyona ekler.</summary>
        public Vector3 CurrentOffset { get; private set; }

        private Coroutine shakeCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
        }

        public void Shake(float intensity = 0.15f, float duration = 0.12f)
        {
            if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(DoShake(intensity, duration));
        }

        private IEnumerator DoShake(float intensity, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = 1f - (elapsed / duration);
                CurrentOffset = (Vector3)Random.insideUnitCircle * (intensity * t);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            CurrentOffset = Vector3.zero;
        }
    }
}
