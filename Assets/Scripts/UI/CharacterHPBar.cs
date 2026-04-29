using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Karakterin üstünde floating HP bar.
    /// - Ayarlar menüsündeki toggle ile açılır/kapanır (PlayerPrefs: "show_char_hp_bar")
    /// - HP doluyken 3 saniye sonra solar, hasar alınca tekrar görünür
    /// - World space canvas — kamera ile scale etmez
    /// </summary>
    public class CharacterHPBar : MonoBehaviour
    {
        public static readonly string PrefKey = "show_char_hp_bar";

        [Header("Layout")]
        [SerializeField] private float barWidth  = 44f;   // world units × 0.01 (canvas px)
        [SerializeField] private float barHeight = 4f;
        [SerializeField] private float yOffset   = 0.72f; // karakter üstünde kaç birim

        [Header("Timing")]
        [SerializeField] private float fadeDelay  = 3f;   // full HP'de bu kadar sonra solar
        [SerializeField] private float fadeSpeed  = 2f;

        private PlayerStats stats;
        private Canvas worldCanvas;
        private Image bgImage;
        private Image fillImage;
        private CanvasGroup canvasGroup;

        private float lastDamagedTime = -999f;
        private float lastHP;

        private static readonly Color bgCol   = new Color(0.08f, 0.05f, 0.05f, 0.85f);
        private static readonly Color fillCol = new Color(0.52f, 0.04f, 0.04f, 1f);   // deep blood red

        // ── Lifecycle ──────────────────────────────────────────────

        private void Awake()
        {
            stats = GetComponent<PlayerStats>() ?? GetComponentInParent<PlayerStats>();
            BuildBar();
        }

        private void Start()
        {
            ApplyVisibility();
            if (stats != null) lastHP = stats.CurrentHP;
        }

        private void Update()
        {
            if (worldCanvas == null || stats == null) return;

            float hp    = stats.CurrentHP;
            float maxHp = stats.MaxHP;

            // HP değişti mi?
            if (!Mathf.Approximately(hp, lastHP))
            {
                lastDamagedTime = Time.time;
                lastHP = hp;
            }

            // Fill güncelle
            fillImage.fillAmount = maxHp > 0 ? hp / maxHp : 0f;

            // Görünürlük: full HP + fade delay geçtiyse solar
            bool isFull    = Mathf.Approximately(hp, maxHp);
            float timeSince = Time.time - lastDamagedTime;

            float targetAlpha = (isFull && timeSince > fadeDelay) ? 0f : 1f;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        }

        // ── Public ─────────────────────────────────────────────────

        public void ApplyVisibility()
        {
            bool show = PlayerPrefs.GetInt(PrefKey, 0) == 1;
            if (worldCanvas != null)
                worldCanvas.gameObject.SetActive(show);
        }

        // ── Build ──────────────────────────────────────────────────

        private void BuildBar()
        {
            // World space canvas — boyutu sabit, zoom'dan etkilenmez
            var go = new GameObject("CharHPBar_Canvas", typeof(RectTransform));
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(0, yOffset, 0);

            worldCanvas = go.AddComponent<Canvas>();
            worldCanvas.renderMode = RenderMode.WorldSpace;
            worldCanvas.sortingLayerName = "UI";
            worldCanvas.sortingOrder = 5;

            canvasGroup = go.AddComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            var rt = go.GetComponent<RectTransform>();
            // World space'de 1 unit = 100 canvas px varsayımı (PPU=64 projede ~0.01 scale)
            float scale = 0.01f;
            rt.localScale = Vector3.one * scale;
            rt.sizeDelta  = new Vector2(barWidth / scale * scale, barHeight / scale * scale);
            // Daha basit: canvas'ı direkt pixel boyutlarında tut, scale ile küçült
            rt.sizeDelta = new Vector2(barWidth, barHeight);
            rt.localScale = Vector3.one * (1f / 64f); // PPU=64 ile pixel → world unit

            // Arka plan
            var bgGo  = MakeRect(go.transform, "BG", barWidth, barHeight);
            bgImage   = bgGo.AddComponent<Image>();
            bgImage.color = bgCol;
            bgImage.raycastTarget = false;

            // Fill
            var fillGo = MakeRect(go.transform, "Fill", barWidth, barHeight);
            fillImage  = fillGo.AddComponent<Image>();
            fillImage.color = fillCol;
            fillImage.type  = Image.Type.Filled;
            fillImage.fillMethod  = Image.FillMethod.Horizontal;
            fillImage.fillOrigin  = (int)Image.OriginHorizontal.Left;
            fillImage.fillAmount  = 1f;
            fillImage.raycastTarget = false;
            // Fill'i BG üstüne çek — 1px inner padding
            var frt = fillGo.GetComponent<RectTransform>();
            frt.anchorMin = Vector2.zero;
            frt.anchorMax = Vector2.one;
            frt.offsetMin = new Vector2(1, 1);
            frt.offsetMax = new Vector2(-1, -1);
            frt.sizeDelta = Vector2.zero; // stretch mod
        }

        private static GameObject MakeRect(Transform parent, string name, float w, float h)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot     = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(w, h);
            rt.anchoredPosition = Vector2.zero;
            return go;
        }
    }
}
