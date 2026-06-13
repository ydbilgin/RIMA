using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// HP multiplier + above-head HP bar with tier color.
    /// Normal=1x red, Elite=3x orange, Champion=5x purple, MiniBoss=10x gold.
    /// Attach alongside Health on any enemy. Call Init() from enemy's Awake AFTER this component.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class EnemyTier : MonoBehaviour
    {
        public enum TierType { Normal = 1, Elite = 3, Champion = 5, MiniBoss = 10 }

        [SerializeField] public TierType tier = TierType.Normal;
        [SerializeField] private float barWidth = 0.6f;
        [SerializeField] private float barYOffset = 0.75f;

        private Health health;
        private Transform fillTransform;
        private SpriteRenderer fillRenderer;

        public static Color GetTierColor(TierType t) => t switch
        {
            TierType.Normal   => new Color(0.85f, 0.15f, 0.15f),  // kırmızı
            TierType.Elite    => new Color(1f,    0.55f, 0.10f),  // turuncu
            TierType.Champion => new Color(0.65f, 0.20f, 0.95f),  // mor
            TierType.MiniBoss => new Color(1f,    0.85f, 0.10f),  // altın
            _ => Color.red
        };

        private void Awake()
        {
            health = GetComponent<Health>();
            // Scale maxHP by tier multiplier
            int mult = (int)tier;
            if (mult > 1)
                health.ScaleMaxHP(mult);
        }

        private void Start()
        {
            BuildHPBar();
            health.OnHealthChanged.AddListener(UpdateFill);
        }

        private void OnDestroy()
        {
            if (health != null)
                health.OnHealthChanged.RemoveListener(UpdateFill);
        }

        // ─── Build ───────────────────────────────────────────────────────────

        private void BuildHPBar()
        {
            var root = new GameObject("TierHPBar");
            root.transform.SetParent(transform);
            root.transform.localPosition = new Vector3(0f, barYOffset, 0f);

            var white = CreateWhiteSprite();

            // BG
            var bgGo = new GameObject("BG", typeof(SpriteRenderer));
            bgGo.transform.SetParent(root.transform);
            bgGo.transform.localPosition = Vector3.zero;
            bgGo.transform.localScale = new Vector3(barWidth, 0.07f, 1f);
            var bgSr = bgGo.GetComponent<SpriteRenderer>();
            bgSr.sprite = white;
            bgSr.color = new Color(0.08f, 0.08f, 0.08f, 0.85f);
            bgSr.sortingOrder = 20;

            // Fill (pivot at left edge for clean scale animation)
            var fillGo = new GameObject("Fill", typeof(SpriteRenderer));
            fillGo.transform.SetParent(root.transform);
            fillGo.transform.localPosition = new Vector3(0f, 0f, -0.01f);
            fillGo.transform.localScale = new Vector3(barWidth, 0.05f, 1f);
            fillRenderer = fillGo.GetComponent<SpriteRenderer>();
            fillRenderer.sprite = white;
            fillRenderer.sortingOrder = 21;
            fillTransform = fillGo.transform;
            UpdateFill(health.CurrentHP, health.MaxHP);

            // Tier label (E / C / M for Elite/Champion/MiniBoss)
            if (tier != TierType.Normal)
            {
                var labelGo = new GameObject("TierLabel", typeof(SpriteRenderer));
                labelGo.transform.SetParent(root.transform);
                labelGo.transform.localPosition = new Vector3(barWidth / 2f + 0.12f, 0f, 0f);
                // Tier harf label is cosmetic — can be replaced with TextMeshPro later
            }
        }

        private void UpdateFill(int current, int max)
        {
            if (fillTransform == null) return;
            float pct = max > 0 ? (float)current / max : 0f;
            var s = fillTransform.localScale;
            fillTransform.localScale = new Vector3(barWidth * pct, s.y, s.z);
            // Shift left so bar shrinks from the right
            fillTransform.localPosition = new Vector3(-barWidth * (1f - pct) / 2f, 0f, -0.01f);
            if (fillRenderer != null)
                fillRenderer.color = Color.Lerp(new Color(0.85f, 0.15f, 0.15f), new Color(0.15f, 0.85f, 0.15f), Mathf.Clamp01(pct));
        }

        private static Sprite CreateWhiteSprite()
        {
            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
        }
    }
}
