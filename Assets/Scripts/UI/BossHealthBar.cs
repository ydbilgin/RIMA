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
        private TextMeshProUGUI phaseText;
        private Image hpFill;
        private int currentHP;
        private int maxHP;

        private static readonly Color StoneFrame = new Color(0.11f, 0.12f, 0.14f, 0.96f);
        private static readonly Color SlateTrack = new Color(0.025f, 0.03f, 0.04f, 1f);
        private static readonly Color CrimsonFill = new Color(0.68f, 0.06f, 0.08f, 1f);
        private static readonly Color AmberWarn = new Color(0.86f, 0.52f, 0.18f, 1f);
        private static readonly Color TextWarm = new Color(0.88f, 0.82f, 0.68f, 1f);

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
                var scaler = canvasGO.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                scaler.matchWidthOrHeight = 0.5f;
                canvasGO.AddComponent<GraphicRaycaster>();
            }

            // Panel background — TOP-center (must not sit at the bottom competing with the skill bar).
            panel = new GameObject("BossHealthPanel");
            panel.transform.SetParent(canvas.transform, false);

            var panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 1f);
            panelRect.pivot = new Vector2(0.5f, 1f);
            panelRect.anchoredPosition = new Vector2(0f, -22f);
            panelRect.sizeDelta = new Vector2(820f, 62f);

            var panelImg = panel.AddComponent<Image>();
            panelImg.color = StoneFrame;

            // Boss name text
            var nameGO = new GameObject("BossNameText");
            nameGO.transform.SetParent(panel.transform, false);
            var nameRect = nameGO.AddComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0f, 0.52f);
            nameRect.anchorMax = new Vector2(0.68f, 1f);
            nameRect.offsetMin = new Vector2(18f, 0f);
            nameRect.offsetMax = new Vector2(-8f, -4f);

            nameText = nameGO.AddComponent<TextMeshProUGUI>();
            nameText.alignment = TextAlignmentOptions.Left;
            nameText.fontSize   = 16f;
            nameText.color      = TextWarm;
            nameText.fontStyle  = FontStyles.Bold;

            var phaseGO = new GameObject("BossPhaseText");
            phaseGO.transform.SetParent(panel.transform, false);
            var phaseRect = phaseGO.AddComponent<RectTransform>();
            phaseRect.anchorMin = new Vector2(0.68f, 0.52f);
            phaseRect.anchorMax = new Vector2(1f, 1f);
            phaseRect.offsetMin = new Vector2(8f, 0f);
            phaseRect.offsetMax = new Vector2(-18f, -4f);

            phaseText = phaseGO.AddComponent<TextMeshProUGUI>();
            phaseText.alignment = TextAlignmentOptions.Right;
            phaseText.fontSize = 13f;
            phaseText.color = AmberWarn;
            phaseText.fontStyle = FontStyles.Bold;

            // HP bar background
            var barBG = new GameObject("HPBarBG");
            barBG.transform.SetParent(panel.transform, false);
            var bgRect = barBG.AddComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0f, 0f);
            bgRect.anchorMax = new Vector2(1f, 0f);
            bgRect.pivot = new Vector2(0.5f, 0f);
            bgRect.anchoredPosition = new Vector2(0f, 10f);
            bgRect.sizeDelta = new Vector2(-36f, 22f);

            var bgImg = barBG.AddComponent<Image>();
            bgImg.color = SlateTrack;

            // HP fill
            var fillGO = new GameObject("HPFill");
            fillGO.transform.SetParent(barBG.transform, false);
            var fillRect = fillGO.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = new Vector2(3f, 3f);
            fillRect.offsetMax = new Vector2(-3f, -3f);

            hpFill = fillGO.AddComponent<Image>();
            hpFill.color = CrimsonFill;
            hpFill.type  = Image.Type.Filled;
            hpFill.fillMethod = Image.FillMethod.Horizontal;
            hpFill.fillOrigin = 0;  // left to right

            CreatePhaseNotch("Phase66Notch", barBG.transform, 0.66f);
            CreatePhaseNotch("Phase33Notch", barBG.transform, 0.33f);

            panel.SetActive(false);
        }

        private void UpdateFill()
        {
            if (hpFill == null || maxHP <= 0) return;
            float ratio = Mathf.Clamp01((float)currentHP / maxHP);
            hpFill.fillAmount = ratio;
            hpFill.color = ratio <= 0.33f
                ? Color.Lerp(CrimsonFill, AmberWarn, 0.18f)
                : CrimsonFill;

            if (phaseText != null)
            {
                phaseText.text = ratio > 0.66f
                    ? "PHASE I"
                    : ratio > 0.33f
                        ? "PHASE II"
                        : "UNLEASHED";
            }
        }

        private static void CreatePhaseNotch(string name, Transform parent, float anchorX)
        {
            var notchGO = new GameObject(name);
            notchGO.transform.SetParent(parent, false);
            var notchRect = notchGO.AddComponent<RectTransform>();
            notchRect.anchorMin = notchRect.anchorMax = new Vector2(anchorX, 0.5f);
            notchRect.pivot = new Vector2(0.5f, 0.5f);
            notchRect.sizeDelta = new Vector2(3f, 26f);
            notchRect.anchoredPosition = Vector2.zero;

            var notch = notchGO.AddComponent<Image>();
            notch.color = new Color(0.72f, 0.64f, 0.48f, 0.95f);
            notch.raycastTarget = false;
        }
    }
}
