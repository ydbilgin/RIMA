using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Combat HUD — "Kan Çatlağı" (Ashen Glyph spec).
    /// Top-left: HP bar (72×4), resource bar (48×3), transient room name.
    /// Bottom-center: interaction prompt.
    /// No gold display, no objective arrow, no ESC handling (UIManager owns that).
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        public static HUDController Instance { get; private set; }

        // ── References (built at runtime) ────────────────────────────────
        [SerializeField] private bool buildRuntimeTemplate = true;
        [SerializeField] private MiniMap miniMap;

        private RectTransform hpFill;
        private TextMeshProUGUI hpLabel;
        private Image hpFillImage;

        private RectTransform resourceFill;
        private Image resourceFillImage;
        private CanvasGroup resourceGroup;

        private TextMeshProUGUI roomNameLabel;

        private RectTransform interactionPanel;
        private TextMeshProUGUI interactionText;

        // ── Cached systems ───────────────────────────────────────────────
        private RageSystem rageSystem;
        private Health     playerHealth;
        private ClassType  currentClass = ClassType.None;

        // ── HP bar dims (Ashen Glyph spec) ───────────────────────────────
        private const float HpBarWidth  = 72f;
        private const float HpBarHeight = 4f;
        private const float ResBarWidth  = 48f;
        private const float ResBarHeight = 3f;
        private const float HudMargin    = 12f;

        // ── Room name fade ───────────────────────────────────────────────
        private Coroutine roomNameCoroutine;

        // ── HP pulse ─────────────────────────────────────────────────────
        private Coroutine hpPulseCoroutine;
        private bool isPulsing;

        // ── Resource pulse ───────────────────────────────────────────────
        private Coroutine resPulseCoroutine;
        private bool isResPulsing;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            if (buildRuntimeTemplate)
                BuildHUD();

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            rageSystem   = player.GetComponent<RageSystem>();
            playerHealth = player.GetComponent<Health>();

            if (rageSystem != null)
            {
                rageSystem.OnRageChanged.AddListener(OnRageChanged);
                OnRageChanged(rageSystem.Current, rageSystem.Max);
            }

            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged.AddListener(OnHPChanged);
                OnHPChanged(playerHealth.CurrentHP, playerHealth.MaxHP);
            }
        }

        private void OnDestroy()
        {
            if (rageSystem   != null) rageSystem.OnRageChanged.RemoveListener(OnRageChanged);
            if (playerHealth != null) playerHealth.OnHealthChanged.RemoveListener(OnHPChanged);
            if (Instance == this) Instance = null;
        }

        // ─── HP ─────────────────────────────────────────────────────────

        private void OnHPChanged(int current, int max)
        {
            float pct = max > 0 ? (float)current / max : 0f;

            // Fill width
            if (hpFill != null)
                hpFill.sizeDelta = new Vector2(HpBarWidth * pct, HpBarHeight);

            // Fill color by threshold
            if (hpFillImage != null)
            {
                Color c;
                if (pct > 0.6f)      c = RimaUITheme.HpHealthy;
                else if (pct > 0.3f) c = RimaUITheme.HpWarning;
                else                  c = RimaUITheme.HpCritical;
                hpFillImage.color = c;
            }

            // Number-only label: "314"
            if (hpLabel != null)
                hpLabel.text = current.ToString();

            // Pulse at <30%
            if (pct <= 0.3f && pct > 0f && !isPulsing)
                hpPulseCoroutine = StartCoroutine(PulseBar(hpFillImage, RimaUITheme.HpCritical));
            else if ((pct > 0.3f || pct <= 0f) && isPulsing)
                StopPulse();
        }

        private IEnumerator PulseBar(Image img, Color baseColor)
        {
            isPulsing = true;
            while (isPulsing && img != null)
            {
                float t = Mathf.PingPong(Time.unscaledTime * 2.5f, 1f);
                img.color = Color.Lerp(baseColor, Color.white, t * 0.25f);
                yield return null;
            }
        }

        private void StopPulse()
        {
            isPulsing = false;
            if (hpPulseCoroutine != null) { StopCoroutine(hpPulseCoroutine); hpPulseCoroutine = null; }
        }

        // ─── Resource (Rage) ────────────────────────────────────────────

        private void OnRageChanged(int current, int max)
        {
            if (max <= 0)
            {
                if (resourceGroup != null) resourceGroup.alpha = 0f;
                return;
            }

            float pct = (float)current / max;

            // Invisible at 0
            if (resourceGroup != null)
                resourceGroup.alpha = current > 0 ? 1f : 0f;

            if (resourceFill != null)
                resourceFill.sizeDelta = new Vector2(ResBarWidth * pct, ResBarHeight);

            if (resourceFillImage != null)
                resourceFillImage.color = pct >= 1f ? RimaUITheme.RageMax : RimaUITheme.RageDefault;

            // Pulse at max
            if (pct >= 1f && !isResPulsing)
                resPulseCoroutine = StartCoroutine(PulseResourceBar());
            else if (pct < 1f && isResPulsing)
                StopResPulse();
        }

        private IEnumerator PulseResourceBar()
        {
            isResPulsing = true;
            while (isResPulsing && resourceFillImage != null)
            {
                float t = Mathf.PingPong(Time.unscaledTime * 3f, 1f);
                resourceFillImage.color = Color.Lerp(RimaUITheme.RageMax, Color.white, t * 0.3f);
                yield return null;
            }
        }

        private void StopResPulse()
        {
            isResPulsing = false;
            if (resPulseCoroutine != null) { StopCoroutine(resPulseCoroutine); resPulseCoroutine = null; }
        }

        // ─── Room Name (transient) ──────────────────────────────────────

        /// <summary>Show room name top-left, fade in 0.4s → hold 3s → fade out 1.2s.</summary>
        public void SetRoomStatus(string text)
        {
            if (roomNameLabel == null) return;
            if (roomNameCoroutine != null) StopCoroutine(roomNameCoroutine);
            roomNameCoroutine = StartCoroutine(ShowRoomName(text));
        }

        private IEnumerator ShowRoomName(string text)
        {
            roomNameLabel.text = text;
            // Fade in
            float t = 0f;
            while (t < 0.4f)
            {
                t += Time.unscaledDeltaTime;
                roomNameLabel.alpha = Mathf.Clamp01(t / 0.4f) * 0.85f;
                yield return null;
            }
            roomNameLabel.alpha = 0.85f;

            // Hold
            yield return new WaitForSecondsRealtime(3f);

            // Fade out
            t = 0f;
            while (t < 1.2f)
            {
                t += Time.unscaledDeltaTime;
                roomNameLabel.alpha = Mathf.Clamp01(1f - t / 1.2f) * 0.85f;
                yield return null;
            }
            roomNameLabel.alpha = 0f;
        }

        // ─── Interaction Prompt (preserved API) ─────────────────────────

        public void SetInteractionPrompt(string actionName)
        {
            if (interactionPanel == null) return;
            if (interactionText != null) interactionText.text = $"[G] {actionName}";
            interactionPanel.gameObject.SetActive(true);
        }

        public void HideInteractionPrompt()
        {
            if (interactionPanel != null) interactionPanel.gameObject.SetActive(false);
        }

        // ─── Class Change (preserved API) ───────────────────────────────

        /// <summary>Call when the player's primary class changes to refresh resource bar color.</summary>
        public void OnClassChanged(ClassType cls)
        {
            currentClass = cls;
            Color fill = RimaUITheme.ResourceFill(cls);
            if (resourceFillImage != null) resourceFillImage.color = fill;
        }

        // ─── Gold (preserved stub — no UI) ──────────────────────────────

        /// <summary>Legacy API stub. Gold display removed from HUD.</summary>
        public void NotifyGoldGain(int amount) { }

        // ─── Build HUD Programmatically ─────────────────────────────────

        private void BuildHUD()
        {
            var root = transform as RectTransform;
            if (root == null) return;

            BuildHpBar(root);
            BuildResourceBar(root);
            BuildRoomNameLabel(root);
            BuildInteractionPrompt(root);
            BuildMiniMap(root);
        }

        private void BuildHpBar(RectTransform root)
        {
            // HP track (empty bg)
            var trackGo = new GameObject("HPTrack", typeof(RectTransform));
            trackGo.transform.SetParent(root, false);
            var trackRt = trackGo.GetComponent<RectTransform>();
            trackRt.anchorMin = new Vector2(0f, 1f);
            trackRt.anchorMax = new Vector2(0f, 1f);
            trackRt.pivot = new Vector2(0f, 1f);
            trackRt.anchoredPosition = new Vector2(HudMargin, -HudMargin);
            trackRt.sizeDelta = new Vector2(HpBarWidth, HpBarHeight);

            var trackImg = trackGo.AddComponent<Image>();
            trackImg.color = RimaUITheme.HpEmpty;
            trackImg.raycastTarget = false;

            // HP fill
            var fillGo = new GameObject("HPFill", typeof(RectTransform));
            fillGo.transform.SetParent(trackGo.transform, false);
            hpFill = fillGo.GetComponent<RectTransform>();
            hpFill.anchorMin = Vector2.zero;
            hpFill.anchorMax = new Vector2(0f, 1f);
            hpFill.pivot = new Vector2(0f, 0.5f);
            hpFill.anchoredPosition = Vector2.zero;
            hpFill.sizeDelta = new Vector2(HpBarWidth, HpBarHeight);

            hpFillImage = fillGo.AddComponent<Image>();
            hpFillImage.color = RimaUITheme.HpHealthy;
            hpFillImage.raycastTarget = false;

            // HP number label (left of bar)
            var labelGo = new GameObject("HPLabel", typeof(RectTransform));
            labelGo.transform.SetParent(root, false);
            var labelRt = labelGo.GetComponent<RectTransform>();
            labelRt.anchorMin = new Vector2(0f, 1f);
            labelRt.anchorMax = new Vector2(0f, 1f);
            labelRt.pivot = new Vector2(1f, 1f);
            labelRt.anchoredPosition = new Vector2(HudMargin - 4f, -HudMargin + 2f);
            labelRt.sizeDelta = new Vector2(60f, 12f);

            hpLabel = labelGo.AddComponent<TextMeshProUGUI>();
            hpLabel.text = "0";
            hpLabel.fontSize = 10f;
            hpLabel.fontStyle = FontStyles.Bold;
            hpLabel.color = new Color(1f, 1f, 1f, 0.70f);
            hpLabel.alignment = TextAlignmentOptions.Right;
            hpLabel.raycastTarget = false;
        }

        private void BuildResourceBar(RectTransform root)
        {
            // Resource group (invisible at 0)
            var groupGo = new GameObject("ResourceGroup", typeof(RectTransform));
            groupGo.transform.SetParent(root, false);
            var groupRt = groupGo.GetComponent<RectTransform>();
            groupRt.anchorMin = new Vector2(0f, 1f);
            groupRt.anchorMax = new Vector2(0f, 1f);
            groupRt.pivot = new Vector2(0f, 1f);
            groupRt.anchoredPosition = new Vector2(HudMargin, -HudMargin - HpBarHeight - 4f);
            groupRt.sizeDelta = new Vector2(ResBarWidth, ResBarHeight);

            resourceGroup = groupGo.AddComponent<CanvasGroup>();
            resourceGroup.alpha = 0f;

            // Track
            var trackImg = groupGo.AddComponent<Image>();
            trackImg.color = RimaUITheme.HpEmpty;
            trackImg.raycastTarget = false;

            // Fill
            var fillGo = new GameObject("ResFill", typeof(RectTransform));
            fillGo.transform.SetParent(groupGo.transform, false);
            resourceFill = fillGo.GetComponent<RectTransform>();
            resourceFill.anchorMin = Vector2.zero;
            resourceFill.anchorMax = new Vector2(0f, 1f);
            resourceFill.pivot = new Vector2(0f, 0.5f);
            resourceFill.anchoredPosition = Vector2.zero;
            resourceFill.sizeDelta = new Vector2(0f, ResBarHeight);

            resourceFillImage = fillGo.AddComponent<Image>();
            resourceFillImage.color = RimaUITheme.RageDefault;
            resourceFillImage.raycastTarget = false;
        }

        private void BuildRoomNameLabel(RectTransform root)
        {
            var go = new GameObject("RoomName", typeof(RectTransform));
            go.transform.SetParent(root, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(HudMargin, -HudMargin - HpBarHeight - ResBarHeight - 12f);
            rt.sizeDelta = new Vector2(200f, 18f);

            roomNameLabel = go.AddComponent<TextMeshProUGUI>();
            roomNameLabel.text = "";
            roomNameLabel.fontSize = 11f;
            roomNameLabel.fontStyle = FontStyles.Italic;
            roomNameLabel.color = RimaUITheme.Cyan;
            roomNameLabel.alpha = 0f;
            roomNameLabel.alignment = TextAlignmentOptions.Left;
            roomNameLabel.raycastTarget = false;
        }

        private void BuildInteractionPrompt(RectTransform root)
        {
            var panelGo = new GameObject("InteractionPrompt", typeof(RectTransform));
            panelGo.transform.SetParent(root, false);
            interactionPanel = panelGo.GetComponent<RectTransform>();
            interactionPanel.anchorMin = new Vector2(0.5f, 0.15f);
            interactionPanel.anchorMax = new Vector2(0.5f, 0.15f);
            interactionPanel.pivot = new Vector2(0.5f, 0.5f);
            interactionPanel.sizeDelta = new Vector2(200f, 32f);
            interactionPanel.anchoredPosition = Vector2.zero;

            var bgImg = panelGo.AddComponent<Image>();
            bgImg.color = RimaUITheme.PanelTint;
            bgImg.preserveAspect = false;
            bgImg.raycastTarget = false;

            var txtGo = new GameObject("Text", typeof(RectTransform));
            txtGo.transform.SetParent(panelGo.transform, false);
            var txtRt = txtGo.GetComponent<RectTransform>();
            txtRt.anchorMin = Vector2.zero;
            txtRt.anchorMax = Vector2.one;
            txtRt.offsetMin = txtRt.offsetMax = Vector2.zero;

            interactionText = txtGo.AddComponent<TextMeshProUGUI>();
            interactionText.text = "";
            interactionText.fontSize = 14f;
            interactionText.alignment = TextAlignmentOptions.Center;
            interactionText.color = Color.white;
            interactionText.raycastTarget = false;

            panelGo.SetActive(false);
        }

        private void BuildMiniMap(RectTransform root)
        {
            if (miniMap == null)
                miniMap = GetComponentInChildren<MiniMap>(true);

            RectTransform mapRoot;
            if (miniMap != null)
            {
                mapRoot = miniMap.transform as RectTransform;
            }
            else
            {
                var go = new GameObject("MiniMapPanel", typeof(RectTransform));
                go.transform.SetParent(root, false);
                mapRoot = go.GetComponent<RectTransform>();
                miniMap = go.AddComponent<MiniMap>();
            }

            // Top-right, 72×72 with 10px margin (spec)
            mapRoot.anchorMin = new Vector2(1f, 1f);
            mapRoot.anchorMax = new Vector2(1f, 1f);
            mapRoot.pivot = new Vector2(1f, 1f);
            mapRoot.anchoredPosition = new Vector2(-10f, -10f);
            mapRoot.sizeDelta = new Vector2(72f, 72f);
        }
    }
}
