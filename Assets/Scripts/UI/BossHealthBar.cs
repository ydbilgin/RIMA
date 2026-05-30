using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Boss HP bar — ekranın ÜST-ortasında (skill bar ile yarışmaz, CONTROL_SCHEME §5), oyun başladığında gizli.
    /// PenitentSovereign.Start() → Initialize() → Show() çağırır.
    ///
    /// Hiyerarşi (runtime'da oluşturulur):
    ///   Canvas [Screen Space - Overlay]
    ///     └── BossPanel [Image, dark bg]
    ///           ├── BossNameText [TMP]
    ///           └── HPBarBG [Image]
    ///                 └── HPBarFill [Image, mask fill]
    ///
    /// Eğer sahnede bir Canvas yoksa yeni biri oluşturulur.
    /// </summary>
    public class BossHealthBar : MonoBehaviour
    {
        // ─── Runtime References ───────────────────────────────────────────────

        private GameObject panel;
        private TextMeshProUGUI nameText;
        private Image hpFill;
        private int currentHP;
        private int maxHP;

        // ─── Public API ───────────────────────────────────────────────────────

        /// <summary>Boss adı ve max HP ile bar'ı hazırla. Show() çağrısından önce yapılmalı.</summary>
        public void Initialize(string bossName, int bossMaxHP)
        {
            maxHP     = bossMaxHP;
            currentHP = bossMaxHP;

            if (panel == null)
                BuildUI();

            if (nameText != null)
                nameText.text = bossName;

            UpdateFill();
        }

        /// <summary>HP değerini güncelle.</summary>
        public void UpdateHP(int current)
        {
            currentHP = Mathf.Max(0, current);
            UpdateFill();
        }

        public void Show() { if (panel != null) panel.SetActive(true); }
        public void Hide() { if (panel != null) panel.SetActive(false); }

        // ─── UI Build ─────────────────────────────────────────────────────────

        private void BuildUI()
        {
            // Prefer the HUD canvas (anchor top-center UNDER it); never grab the first arbitrary Canvas (CONTROL_SCHEME §5).
            Canvas canvas = HUDController.Instance != null
                ? HUDController.Instance.GetComponentInParent<Canvas>()
                : null;
            if (canvas == null)
            {
                var canvasGO = new GameObject("BossCanvas");
                canvas = canvasGO.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 90; // above the gameplay HUD, below pause/settings overlays
                canvasGO.AddComponent<CanvasScaler>();
                canvasGO.AddComponent<GraphicRaycaster>();
            }

            // Panel background — TOP-center (must not sit at the bottom competing with the skill bar).
            panel = new GameObject("BossHealthPanel");
            panel.transform.SetParent(canvas.transform, false);

            var panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.22f, 0.88f);
            panelRect.anchorMax = new Vector2(0.78f, 0.96f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            var panelImg = panel.AddComponent<Image>();
            panelImg.color = new Color(0.05f, 0.05f, 0.05f, 0.85f);

            // Boss name text
            var nameGO = new GameObject("BossNameText");
            nameGO.transform.SetParent(panel.transform, false);
            var nameRect = nameGO.AddComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0f, 0.5f);
            nameRect.anchorMax = new Vector2(1f, 1f);
            nameRect.offsetMin = new Vector2(8f, 0f);
            nameRect.offsetMax = new Vector2(-8f, -2f);

            nameText = nameGO.AddComponent<TextMeshProUGUI>();
            nameText.alignment = TextAlignmentOptions.Center;
            nameText.fontSize   = 14f;
            nameText.color      = new Color(0.9f, 0.85f, 0.7f);
            nameText.fontStyle  = FontStyles.Bold;

            // HP bar background
            var barBG = new GameObject("HPBarBG");
            barBG.transform.SetParent(panel.transform, false);
            var bgRect = barBG.AddComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0.02f, 0.05f);
            bgRect.anchorMax = new Vector2(0.98f, 0.5f);
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            var bgImg = barBG.AddComponent<Image>();
            bgImg.color = new Color(0.15f, 0.05f, 0.05f, 1f);

            // HP fill
            var fillGO = new GameObject("HPFill");
            fillGO.transform.SetParent(barBG.transform, false);
            var fillRect = fillGO.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = new Vector2(2f, 2f);
            fillRect.offsetMax = new Vector2(-2f, -2f);

            hpFill = fillGO.AddComponent<Image>();
            hpFill.color = new Color(0.85f, 0.15f, 0.1f, 1f);
            hpFill.type  = Image.Type.Filled;
            hpFill.fillMethod = Image.FillMethod.Horizontal;
            hpFill.fillOrigin = 0;  // left to right

            panel.SetActive(false);
        }

        private void UpdateFill()
        {
            if (hpFill == null || maxHP <= 0) return;
            hpFill.fillAmount = (float)currentHP / maxHP;

            // Color shift: green → yellow → red
            float ratio = (float)currentHP / maxHP;
            hpFill.color = ratio > 0.5f
                ? Color.Lerp(new Color(0.9f, 0.7f, 0.1f), new Color(0.2f, 0.8f, 0.2f), (ratio - 0.5f) * 2f)
                : Color.Lerp(new Color(0.85f, 0.15f, 0.1f), new Color(0.9f, 0.7f, 0.1f), ratio * 2f);
        }
    }
}
