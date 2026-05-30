using UnityEngine;
using System.Collections;

namespace RIMA
{
    /// <summary>
    /// Camera shake sistemi — vuruşlarda, kill'lerde, büyük saldırılarda kamera sallanır.
    /// Intensity-based: hafif vuruş = küçük shake, kill = büyük shake.
    /// </summary>
    [System.Obsolete("Not the live spine - see WORK_ORDER_24_48H_S6. This rotates the camera; live shake = ScreenShakeDriver offset.")]
    public class ScreenShake : MonoBehaviour
    {
        public static ScreenShake Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private float traumaDecay = 1.5f; // Trauma azalma hızı
        [SerializeField] private float maxShakeAngle = 5f;
        [SerializeField] private float maxShakeOffset = 0.3f;

        private Camera cam;
        private Vector3 originalPosition;
        private float trauma; // 0-1 arası, shake yoğunluğu

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            cam = Camera.main;
            if (cam != null)
                originalPosition = cam.transform.localPosition;
        }

        private void Update()
        {
            if (cam == null) return;

            // Trauma decay
            if (trauma > 0f)
            {
                trauma -= traumaDecay * Time.deltaTime;
                trauma = Mathf.Max(0f, trauma);
            }

            // Apply shake
            if (trauma > 0f)
            {
                float shake = trauma * trauma; // Quadratic falloff

                float angleZ = maxShakeAngle * shake * Mathf.PerlinNoise(Time.time * 25f, 0f) * 2f - maxShakeAngle * shake;
                float offsetX = maxShakeOffset * shake * (Mathf.PerlinNoise(Time.time * 25f, 1000f) * 2f - 1f);
                float offsetY = maxShakeOffset * shake * (Mathf.PerlinNoise(Time.time * 25f, 2000f) * 2f - 1f);

                cam.transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
                cam.transform.localRotation = Quaternion.Euler(0f, 0f, angleZ);
            }
            else
            {
                // Reset
                cam.transform.localPosition = originalPosition;
                cam.transform.localRotation = Quaternion.identity;
            }
        }

        /// <summary>
        /// Shake ekle (intensity: 0-1).
        /// </summary>
        public void AddTrauma(float intensity)
        {
            trauma = Mathf.Clamp01(trauma + intensity);
        }

        /// <summary>
        /// Hafif vuruş shake (normal hit).
        /// </summary>
        public void ShakeLight()
        {
            AddTrauma(0.15f);
        }

        /// <summary>
        /// Orta vuruş shake (heavy hit, skill).
        /// </summary>
        public void ShakeMedium()
        {
            AddTrauma(0.35f);
        }

        /// <summary>
        /// Güçlü shake (kill, boss hit, ultimate).
        /// </summary>
        public void ShakeHeavy()
        {
            AddTrauma(0.65f);
        }

        /// <summary>
        /// Maksimum shake (boss death, explosion).
        /// </summary>
        public void ShakeExplosion()
        {
            AddTrauma(1f);
        }

        /// <summary>
        /// Custom intensity shake.
        /// </summary>
        public void Shake(float intensity)
        {
            AddTrauma(intensity);
        }
    }
}
