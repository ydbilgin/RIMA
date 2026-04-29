using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Rage görsel feedback — Rage yükseldikçe ekran kenarlarında kırmızı vignette + karakter glow.
    /// Rage 0-40: yok | 40-70: hafif | 70-100: yoğun
    /// </summary>
    public class RageVisualFeedback : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RageSystem rageSystem;
        [SerializeField] private SpriteRenderer playerSprite;

        [Header("Vignette Settings")]
        [SerializeField] private Image vignetteImage;
        [SerializeField] private Color vignetteColor = new Color(0.8f, 0.1f, 0.1f, 0.5f);
        [SerializeField] private float vignetteMaxAlpha = 0.5f;

        [Header("Glow Settings")]
        [SerializeField] private Color glowColorLow = new Color(1f, 0.6f, 0.2f, 0.3f);
        [SerializeField] private Color glowColorHigh = new Color(1f, 0.2f, 0.1f, 0.6f);
        [SerializeField] private float glowPulseSpeed = 2f;

        [Header("Thresholds")]
        [SerializeField] private float lowThreshold = 40f;
        [SerializeField] private float highThreshold = 70f;

        private Material playerMaterial;
        private bool hasGlowMaterial;

        private void Start()
        {
            // Auto-find references
            if (rageSystem == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    rageSystem = player.GetComponent<RageSystem>();
            }

            if (playerSprite == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    playerSprite = player.GetComponentInChildren<SpriteRenderer>();
            }

            // Create vignette if not assigned
            if (vignetteImage == null)
                CreateVignette();

            // Setup glow material
            if (playerSprite != null)
            {
                playerMaterial = playerSprite.material;
                hasGlowMaterial = playerMaterial.HasProperty("_GlowColor");
            }
        }

        private void Update()
        {
            if (rageSystem == null) return;

            float rage = rageSystem.CurrentRage;
            float ragePercent = rage / rageSystem.MaxRage;

            UpdateVignette(rage, ragePercent);
            UpdateGlow(rage, ragePercent);
        }

        private void UpdateVignette(float rage, float ragePercent)
        {
            if (vignetteImage == null) return;

            float alpha = 0f;

            if (rage < lowThreshold)
            {
                // No vignette
                alpha = 0f;
            }
            else if (rage < highThreshold)
            {
                // Low rage: fade in
                float t = (rage - lowThreshold) / (highThreshold - lowThreshold);
                alpha = Mathf.Lerp(0f, vignetteMaxAlpha * 0.5f, t);
            }
            else
            {
                // High rage: intense
                float t = (rage - highThreshold) / (100f - highThreshold);
                alpha = Mathf.Lerp(vignetteMaxAlpha * 0.5f, vignetteMaxAlpha, t);
            }

            Color c = vignetteColor;
            c.a = alpha;
            vignetteImage.color = c;
        }

        private void UpdateGlow(float rage, float ragePercent)
        {
            if (playerSprite == null) return;

            if (rage < lowThreshold)
            {
                // No glow
                if (hasGlowMaterial)
                    playerMaterial.SetColor("_GlowColor", Color.clear);
                return;
            }

            // Pulse effect
            float pulse = Mathf.Sin(Time.time * glowPulseSpeed) * 0.5f + 0.5f;

            Color glowColor;
            if (rage < highThreshold)
            {
                // Low rage: orange glow
                float t = (rage - lowThreshold) / (highThreshold - lowThreshold);
                glowColor = Color.Lerp(Color.clear, glowColorLow, t);
            }
            else
            {
                // High rage: red glow
                float t = (rage - highThreshold) / (100f - highThreshold);
                glowColor = Color.Lerp(glowColorLow, glowColorHigh, t);
            }

            // Apply pulse
            glowColor.a *= (0.7f + pulse * 0.3f);

            if (hasGlowMaterial)
            {
                playerMaterial.SetColor("_GlowColor", glowColor);
            }
            else
            {
                // Fallback: tint sprite color
                Color tint = Color.Lerp(Color.white, glowColor, glowColor.a);
                playerSprite.color = tint;
            }
        }

        private void CreateVignette()
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning("[RageVisualFeedback] Canvas bulunamadı, vignette oluşturulamadı");
                return;
            }

            // Create vignette GameObject
            var vignetteGo = new GameObject("RageVignette", typeof(RectTransform));
            vignetteGo.transform.SetParent(canvas.transform, false);

            var rt = vignetteGo.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            vignetteImage = vignetteGo.AddComponent<Image>();
            vignetteImage.color = Color.clear;
            vignetteImage.raycastTarget = false;

            // Load vignette sprite (placeholder: solid color with radial gradient)
            // TODO: Faz 2'de gerçek vignette texture eklenebilir
            vignetteImage.sprite = CreateVignetteSprite();
        }

        private Sprite CreateVignetteSprite()
        {
            // Create a simple radial gradient texture
            int size = 512;
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[size * size];

            Vector2 center = new Vector2(size / 2f, size / 2f);
            float maxDist = size * 0.7f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Vector2 pos = new Vector2(x, y);
                    float dist = Vector2.Distance(pos, center);
                    float alpha = Mathf.Clamp01((dist - maxDist * 0.3f) / (maxDist * 0.7f));
                    pixels[y * size + x] = new Color(0f, 0f, 0f, alpha);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();

            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }

        private void OnDestroy()
        {
            // Reset sprite color
            if (playerSprite != null)
                playerSprite.color = Color.white;
        }
    }
}
