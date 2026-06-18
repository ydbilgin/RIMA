using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Combat HUD — "Kan Çatlağı" (Ashen Glyph spec).
    /// Top-left: HP bar (212×16), resource bar (160×10), transient room name.
    /// Bottom-center: interaction prompt.
    /// No gold display, no objective arrow, no ESC handling (UIManager owns that).
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        public static HUDController Instance { get; private set; }

        // ── References (built at runtime) ────────────────────────────────
        [SerializeField] private bool buildRuntimeTemplate = true;
        // Minimap disabled for now (user decision 2026-06-04): corner MiniMap was bound to the legacy
        // DungeonGraph and did not reflect MapFlowManager progression. Re-enable via this flag once the
        // DungeonGraph unification lands and the map reflects real run progress.
        [SerializeField] private bool showMiniMap = false;
        [SerializeField] private MiniMap miniMap;

        private RectTransform hpFill;
        private TextMeshProUGUI hpLabel;
        private Image hpFillImage;

        private RectTransform resourceFill;
        private Image resourceFillImage;
        private CanvasGroup resourceGroup;

        private TextMeshProUGUI echoBalanceLabel;
        private int lastEchoBalance = int.MinValue;

        private Image lowHpVignette;

        private TextMeshProUGUI roomNameLabel;
        // K5.1: center entry banner (large, 1.5 s) — separate from the small persistent label.
        private TextMeshProUGUI roomBannerLabel;
        private Coroutine roomBannerCoroutine;

        private RectTransform interactionPanel;
        private TextMeshProUGUI interactionText;

        // ── Cached systems ───────────────────────────────────────────────
        private RageSystem rageSystem;
        private PlayerResourceBase resourceSystem;
        private Health     playerHealth;
        private ClassType  currentClass = ClassType.None;

        // ── HP/resource bar dims (bottom-left, Hades/Diablo spec) ────────
        private const float HpBarWidth  = 260f;
        private const float HpBarHeight = 20f;
        private const float ResBarWidth  = 220f;
        private const float ResBarHeight = 8f;
        private const float HudMargin    = 12f;

        // Bottom-left layout offsets (anchored from bottom-left corner).
        private const float HudLeft        = 24f;
        private const float HpBarBottom    = 30f;   // HP track Y (px from screen bottom)
        private const float ResBarBottom   = 16f;   // resource track Y, just under HP
        private const float TextStackBottom = 60f;  // Echo + room name, above the bars

        // Bottom-left bar colors (Hades/Diablo spec).
        private static readonly Color HpFillCrimson  = new Color32(0xC0, 0x10, 0x20, 0xFF); // #C01020
        private static readonly Color ResFillCyan    = new Color32(0x10, 0xA0, 0xC0, 0xFF); // #10A0C0
        private static readonly Color BarTrackSlate  = new Color(0x1B / 255f, 0x1F / 255f, 0x28 / 255f, 0.8f); // #1B1F28 @ 0.8
        private const float LowHpThreshold = 0.20f;
        private const float LowHpMinAlpha = 0.12f;
        private const float LowHpMaxAlpha = 0.18f;
        private const float LowHpPulseSeconds = 0.90f;

        // ── Room name fade ───────────────────────────────────────────────
        private Coroutine roomNameCoroutine;

        // ── HP pulse ─────────────────────────────────────────────────────
        private Coroutine hpPulseCoroutine;
        private bool isPulsing;

        // ── Resource pulse ───────────────────────────────────────────────
        private Coroutine resPulseCoroutine;
        private bool isResPulsing;

        // ── Control hint (first-room, fade-out once) ─────────────────────
        private TextMeshProUGUI _controlHintLabel;
        private bool _controlHintShown;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            EnsureScreenSpaceOverlay();
        }

        // HUD_Canvas was scene-misconfigured as World Space (no Event Camera) — UI floats/scales wrong and
        // raycasts break. A combat HUD must be screen-space overlay; enforce at runtime so a bad scene value
        // can't make the game "look wrong". (Also fix the scene value to clear the edit-mode warning.)
        private void EnsureScreenSpaceOverlay()
        {
            var cv = GetComponent<Canvas>();
            if (cv != null && cv.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                cv.renderMode = RenderMode.ScreenSpaceOverlay;
                cv.worldCamera = null;
            }
        }

        private void Start()
        {
            if (buildRuntimeTemplate)
                BuildHUD();

            EchoWallet.EnsureInitialized();
            UpdateEchoDisplay();

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                // P0-8: player not spawned yet (EnsureHUD runs before EnsurePlayerAtSpawn in some frames).
                // Retry binding until player appears, up to ~2 seconds (20 × 0.1 s).
                StartCoroutine(LateBindPlayerCoroutine());
                return;
            }

            BindPlayer(player);
        }

        /// <summary>P0-8: retry binding up to ~2 s so HUD never starts with empty HP / resource.</summary>
        private System.Collections.IEnumerator LateBindPlayerCoroutine()
        {
            const int maxRetries = 20;
            const float interval = 0.1f;
            for (int i = 0; i < maxRetries; i++)
            {
                yield return new WaitForSecondsRealtime(interval);
                var p = GameObject.FindGameObjectWithTag("Player");
                if (p != null)
                {
                    BindPlayer(p);
                    yield break;
                }
            }
            Debug.LogWarning("[HUDController] Player not found after 2s — HP/resource bar will remain empty.");
        }

        private void BindPlayer(GameObject player)
        {
            currentClass = PlayerClassManager.Instance != null
                ? PlayerClassManager.Instance.PrimaryClass
                : ClassType.Warblade;

            rageSystem   = player.GetComponent<RageSystem>();
            resourceSystem = ResolveResource(player, currentClass);
            playerHealth = player.GetComponent<Health>();

            if (resourceSystem != null)
            {
                resourceSystem.OnResourceChanged.RemoveListener(OnResourceChanged); // idempotent guard
                resourceSystem.OnResourceChanged.AddListener(OnResourceChanged);
                OnResourceChanged(resourceSystem.Current, resourceSystem.Max);
            }

            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged.RemoveListener(OnHPChanged);
                playerHealth.OnHealthChanged.AddListener(OnHPChanged);
                playerHealth.OnDamageTaken.RemoveListener(OnPlayerDamaged);
                playerHealth.OnDamageTaken.AddListener(OnPlayerDamaged);
                OnHPChanged(playerHealth.CurrentHP, playerHealth.MaxHP);
            }

            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.OnPrimaryClassSet += OnClassChanged;

            ShowControlHintOnce();
        }

        private void Update()
        {
            UpdateEchoDisplay();
        }

        private void OnDestroy()
        {
            if (resourceSystem != null) resourceSystem.OnResourceChanged.RemoveListener(OnResourceChanged);
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged.RemoveListener(OnHPChanged);
                playerHealth.OnDamageTaken.RemoveListener(OnPlayerDamaged);
            }
            if (PlayerClassManager.Instance != null) PlayerClassManager.Instance.OnPrimaryClassSet -= OnClassChanged;
            if (Instance == this) Instance = null;
        }

        // ─── HP ─────────────────────────────────────────────────────────

        private void OnHPChanged(int current, int max)
        {
            float pct = max > 0 ? (float)current / max : 0f;

            // Fill width
            if (hpFill != null)
                hpFill.sizeDelta = new Vector2(HpBarWidth * pct, HpBarHeight);

            // HP fill is ALWAYS crimson #C01020 — the class tint (cyan/etc.) must never bleed onto the
            // HP bar; cyan is reserved for the resource/seal bar. The low-HP pulse below still shimmers it.
            if (hpFillImage != null && !isPulsing)
                hpFillImage.color = HpFillCrimson;

            // Number-only label: "314"
            if (hpLabel != null)
                hpLabel.text = current.ToString();

            // Pulse at low HP (shimmer the crimson bar toward white — preserved low-HP feedback).
            if (pct <= LowHpThreshold && pct > 0f && !isPulsing)
                hpPulseCoroutine = StartCoroutine(PulseBar(hpFillImage, HpFillCrimson));
            else if ((pct > LowHpThreshold || pct <= 0f) && isPulsing)
                StopPulse();

            // Low-HP vignette: edge-only pulse under 20% HP; center stays transparent in the sprite.
            if (lowHpVignette != null && !hitFlashActive)
            {
                Color vc = lowHpVignette.color;
                vc.a = SustainedVignetteAlpha();
                lowHpVignette.color = vc;
            }
        }

        // ─── Player-hit feedback (hit-stop + vignette flash) ────────────

        private bool hitFlashActive;

        private void OnPlayerDamaged(int amount)
        {
            // Brief hit-stop on taking damage (FEEL-FIRST, 0-cost) — respect the accessibility toggle.
            if (RIMA.Combat.Juice.FeelToggleSettings.HitstopEnabled)
                RIMA.Combat.HitPauseDriver.Instance?.TriggerPause(0.06f);

            if (lowHpVignette != null && isActiveAndEnabled)
                StartCoroutine(HitFlash());
        }

        private IEnumerator HitFlash()
        {
            hitFlashActive = true;
            const float dur = 0.18f;
            float t = 0f;
            while (t < dur && lowHpVignette != null)
            {
                t += Time.unscaledDeltaTime;
                float flash = Mathf.Lerp(0.22f, 0f, t / dur);
                Color c = lowHpVignette.color;
                c.a = Mathf.Max(SustainedVignetteAlpha(), flash); // composite over the sustained low-HP level
                lowHpVignette.color = c;
                yield return null;
            }
            hitFlashActive = false;
            if (lowHpVignette != null)
            {
                Color c = lowHpVignette.color;
                c.a = SustainedVignetteAlpha();
                lowHpVignette.color = c;
            }
        }

        private float SustainedVignetteAlpha()
        {
            if (playerHealth == null || playerHealth.MaxHP <= 0) return 0f;
            float pct = (float)playerHealth.CurrentHP / playerHealth.MaxHP;
            if (pct <= 0f || pct >= LowHpThreshold) return 0f;

            float severity = Mathf.InverseLerp(LowHpThreshold, 0f, pct);
            float targetAlpha = Mathf.Lerp(LowHpMinAlpha, LowHpMaxAlpha, severity);
            float pulseT = 0.5f + 0.5f * Mathf.Sin((Time.unscaledTime / LowHpPulseSeconds) * Mathf.PI * 2f);
            return Mathf.Lerp(LowHpMinAlpha, targetAlpha, pulseT);
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
            // Restore the steady crimson after a shimmer ends (pulse leaves a mid-lerp tint).
            if (hpFillImage != null) hpFillImage.color = HpFillCrimson;
        }

        // ─── Resource (Rage) ────────────────────────────────────────────

        private void OnResourceChanged(int current, int max)
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
                resourceFillImage.color = pct >= 1f
                    ? Color.Lerp(RimaUITheme.ResourceFill(currentClass), Color.white, 0.35f)
                    : RimaUITheme.ResourceFill(currentClass);

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
                resourceFillImage.color = Color.Lerp(RimaUITheme.ResourceFill(currentClass), Color.white, t * 0.3f);
                yield return null;
            }
        }

        private void StopResPulse()
        {
            isResPulsing = false;
            if (resPulseCoroutine != null) { StopCoroutine(resPulseCoroutine); resPulseCoroutine = null; }
        }

        private void UpdateEchoDisplay()
        {
            if (echoBalanceLabel == null)
            {
                return;
            }

            int balance = EchoWallet.Balance;
            if (balance == lastEchoBalance)
            {
                return;
            }

            lastEchoBalance = balance;
            echoBalanceLabel.text = balance.ToString();
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

        // ─── Room Label (K5.1 — persistent small label + entry banner) ────

        /// <summary>
        /// K5.1: Called once per room build with "ODA {n}/6 — {TİP}".
        /// Shows a large 1.5 s center banner (fade-in 0.2 s / hold 0.9 s / fade-out 0.4 s) and
        /// then keeps a small persistent label visible in the top-left below the HP bars.
        /// </summary>
        public void SetRoomLabel(string text)
        {
            // Persistent small label — always visible, no animation.
            if (roomNameLabel != null)
            {
                roomNameLabel.text = text;
                roomNameLabel.alpha = 0.80f;
            }

            // Center entry banner — timed fade.
            if (roomBannerLabel == null) return;
            if (roomBannerCoroutine != null) StopCoroutine(roomBannerCoroutine);
            roomBannerCoroutine = StartCoroutine(ShowRoomBanner(text));
        }

        private IEnumerator ShowRoomBanner(string text)
        {
            roomBannerLabel.text = text;
            // Fade in 0.2 s
            float t = 0f;
            while (t < 0.2f)
            {
                t += Time.unscaledDeltaTime;
                roomBannerLabel.alpha = Mathf.Clamp01(t / 0.2f);
                yield return null;
            }
            roomBannerLabel.alpha = 1f;

            // Hold 0.9 s
            yield return new WaitForSecondsRealtime(0.9f);

            // Fade out 0.4 s
            t = 0f;
            while (t < 0.4f)
            {
                t += Time.unscaledDeltaTime;
                roomBannerLabel.alpha = Mathf.Clamp01(1f - t / 0.4f);
                yield return null;
            }
            roomBannerLabel.alpha = 0f;
            roomBannerCoroutine = null;
        }

        private void BuildRoomBannerLabel(RectTransform root)
        {
            var go = new GameObject("RoomBanner", typeof(RectTransform));
            go.transform.SetParent(root, false);
            var rt = go.GetComponent<RectTransform>();
            // Upper third of screen, centered.
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0f, -60f);
            rt.sizeDelta = new Vector2(0f, 32f);

            roomBannerLabel = go.AddComponent<TextMeshProUGUI>();
            roomBannerLabel.text = "";
            roomBannerLabel.fontSize = 22f;
            roomBannerLabel.fontStyle = FontStyles.Bold;
            roomBannerLabel.color = RimaUITheme.Cyan;
            roomBannerLabel.alpha = 0f;
            roomBannerLabel.alignment = TextAlignmentOptions.Center;
            roomBannerLabel.outlineColor = new Color(0f, 0f, 0f, 0.75f);
            roomBannerLabel.outlineWidth = 0.2f;
            roomBannerLabel.raycastTarget = false;
        }

        // ─── Interaction Prompt (preserved API) ─────────────────────────

        internal static string ComposeInteractionPrompt(string actionName)
        {
            string trimmed = actionName?.TrimStart();
            if (!string.IsNullOrEmpty(trimmed) && IsBracketTokenStart(trimmed))
            {
                return actionName;
            }

            return $"[G] {actionName}";
        }

        private static bool IsBracketTokenStart(string text)
        {
            if (text[0] != '[') return false;

            int close = text.IndexOf(']');
            if (close <= 1) return false;

            for (int i = 1; i < close; i++)
            {
                if (text[i] < 'A' || text[i] > 'Z') return false;
            }

            return true;
        }

        public void SetInteractionPrompt(string actionName)
        {
            if (interactionPanel == null) return;
            if (interactionText != null) interactionText.text = ComposeInteractionPrompt(actionName);
            interactionPanel.gameObject.SetActive(true);
        }

        public void HideInteractionPrompt()
        {
            if (interactionPanel != null) interactionPanel.gameObject.SetActive(false);
        }

        // ─── Toast (transient on-screen notice) ─────────────────────────

        // Brief center-screen text notice for events the skill bar can't show (e.g. a granted passive).
        // Text-only, outline-only — matches the 'UI yoktur bilgi vardır' rule used by the unlock banner.
        public void ShowToast(string message, float duration = 2.5f)
        {
            if (string.IsNullOrEmpty(message)) return;
            StartCoroutine(ToastRoutine(message, duration));
        }

        private IEnumerator ToastRoutine(string message, float duration)
        {
            var go = new GameObject("HudToast", typeof(RectTransform));
            go.transform.SetParent(transform, false);

            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = message;
            tmp.fontSize = 30f;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = new Color(0.85f, 0.78f, 1f, 1f); // soft arcane-violet notice
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.outlineWidth = 0.26f;
            tmp.outlineColor = new Color32(0, 0, 0, 230);
            tmp.raycastTarget = false;

            var rt = tmp.rectTransform;
            rt.anchorMin = new Vector2(0.1f, 0.70f);
            rt.anchorMax = new Vector2(0.9f, 0.78f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            float elapsed = 0f;
            const float fadeLen = 0.4f;
            while (elapsed < duration && tmp != null)
            {
                elapsed += Time.unscaledDeltaTime;
                float fadeStart = duration - fadeLen;
                if (elapsed > fadeStart)
                {
                    Color c = tmp.color;
                    c.a = 1f - Mathf.Clamp01((elapsed - fadeStart) / fadeLen);
                    tmp.color = c;
                }
                yield return null;
            }

            if (go != null) Destroy(go);
        }

        // ─── Class Change (preserved API) ───────────────────────────────

        /// <summary>Call when the player's primary class changes to refresh resource bar color.</summary>
        public void OnClassChanged(ClassType cls)
        {
            currentClass = cls;
            Color fill = RimaUITheme.ResourceFill(cls);
            if (resourceFillImage != null) resourceFillImage.color = fill;

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            if (resourceSystem != null)
                resourceSystem.OnResourceChanged.RemoveListener(OnResourceChanged);

            resourceSystem = ResolveResource(player, cls);
            if (resourceSystem != null)
            {
                resourceSystem.OnResourceChanged.AddListener(OnResourceChanged);
                OnResourceChanged(resourceSystem.Current, resourceSystem.Max);
            }
        }

        // ─── Gold (preserved stub — no UI) ──────────────────────────────

        /// <summary>Legacy API stub. Gold display removed from HUD.</summary>
        public void NotifyGoldGain(int amount) { }

        // ─── Build HUD Programmatically ─────────────────────────────────

        private void BuildHUD()
        {
            var root = transform as RectTransform;
            if (root == null) return;

            BuildLowHpVignette(root); // first → bottom layer (over gameplay, behind the bars)
            BuildHpBar(root);
            BuildResourceBar(root);
            BuildEchoDisplay(root);
            BuildRoomNameLabel(root);
            BuildRoomBannerLabel(root);
            BuildInteractionPrompt(root);
            BuildControlHint(root);
            if (showMiniMap)
            {
                BuildMiniMap(root);
            }
            else
            {
                // Disable any authored MiniMapPanel child (present in the scene HUD) while the minimap is off.
                var existing = GetComponentInChildren<MiniMap>(true);
                if (existing != null) existing.gameObject.SetActive(false);
            }
        }

        // ── On-brand UI pack (iron-stone bar socket + cyan energy fill). ──
        // Loaded LAZILY on first use (from BuildHUD/Start context). Resources.Load is NOT allowed in a
        // field initializer / constructor, so we defer it; null-guarded so a missing import keeps the flat fallback.
        private static Sprite _packBarFrame;
        private static Sprite _packBarFill;
        private static bool _packLoaded;

        private static void EnsurePackLoaded()
        {
            if (_packLoaded) return;
            _packLoaded = true;
            _packBarFrame = Resources.Load<Sprite>("UI/RIMA/Pack/bar_frame_9slice");
            _packBarFill  = Resources.Load<Sprite>("UI/RIMA/Pack/bar_fill");
        }

        // Drop the on-brand iron-stone socket onto a bar's background Image (9-slice). No-op if art missing.
        private static void ApplyPackSocket(Image socket)
        {
            EnsurePackLoaded();
            if (socket == null || _packBarFrame == null) return;
            socket.sprite = _packBarFrame;
            socket.type   = Image.Type.Sliced;
            socket.color  = Color.white; // let the iron-stone art show; trough is empty-center
        }

        // Drop the on-brand cyan-energy fill shape onto a bar's fill Image (sprite only; color set by value logic).
        private static void ApplyPackFill(Image fill)
        {
            EnsurePackLoaded();
            if (fill == null || _packBarFill == null) return;
            fill.sprite = _packBarFill;
            fill.type   = Image.Type.Simple;
        }

        private void BuildHpBar(RectTransform root)
        {
            // HP track (empty bg) — bottom-left.
            var trackGo = new GameObject("HPTrack", typeof(RectTransform));
            trackGo.transform.SetParent(root, false);
            var trackRt = trackGo.GetComponent<RectTransform>();
            trackRt.anchorMin = new Vector2(0f, 0f);
            trackRt.anchorMax = new Vector2(0f, 0f);
            trackRt.pivot = new Vector2(0f, 0f);
            trackRt.anchoredPosition = new Vector2(HudLeft, HpBarBottom);
            trackRt.sizeDelta = new Vector2(HpBarWidth, HpBarHeight);

            var trackImg = trackGo.AddComponent<Image>();
            trackImg.color = BarTrackSlate; // slate #1B1F28 @ 0.8 (not pitch black)
            trackImg.raycastTarget = false;
            ApplyPackSocket(trackImg); // iron-stone trough (overrides flat-color when art present)

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
            hpFillImage.color = HpFillCrimson; // crimson #C01020 init; OnHPChanged keeps the value-driven threshold tint
            hpFillImage.raycastTarget = false;
            // Share the on-brand fill SHAPE; tint stays health-red/warm (value-driven thresholds in
            // OnHPChanged), never cyan — cyan is reserved for the resource/seal bar.
            ApplyPackFill(hpFillImage);

            // HP number label — right-aligned over the right edge of the HP bar.
            var labelGo = new GameObject("HPLabel", typeof(RectTransform));
            labelGo.transform.SetParent(root, false);
            var labelRt = labelGo.GetComponent<RectTransform>();
            labelRt.anchorMin = new Vector2(0f, 0f);
            labelRt.anchorMax = new Vector2(0f, 0f);
            labelRt.pivot = new Vector2(1f, 0f);
            labelRt.anchoredPosition = new Vector2(HudLeft + HpBarWidth - 6f, HpBarBottom + 2f);
            labelRt.sizeDelta = new Vector2(68f, HpBarHeight);

            hpLabel = labelGo.AddComponent<TextMeshProUGUI>();
            hpLabel.text = "0";
            hpLabel.fontSize = 14f;
            hpLabel.fontStyle = FontStyles.Bold;
            hpLabel.color = new Color(1f, 1f, 1f, 0.70f);
            hpLabel.alignment = TextAlignmentOptions.Right;
            hpLabel.raycastTarget = false;
        }

        private void BuildResourceBar(RectTransform root)
        {
            // Resource group (invisible at 0) — bottom-left, just under the HP bar.
            var groupGo = new GameObject("ResourceGroup", typeof(RectTransform));
            groupGo.transform.SetParent(root, false);
            var groupRt = groupGo.GetComponent<RectTransform>();
            groupRt.anchorMin = new Vector2(0f, 0f);
            groupRt.anchorMax = new Vector2(0f, 0f);
            groupRt.pivot = new Vector2(0f, 0f);
            groupRt.anchoredPosition = new Vector2(HudLeft, ResBarBottom);
            groupRt.sizeDelta = new Vector2(ResBarWidth, ResBarHeight);

            resourceGroup = groupGo.AddComponent<CanvasGroup>();
            resourceGroup.alpha = 0f;

            // Track
            var trackImg = groupGo.AddComponent<Image>();
            trackImg.color = BarTrackSlate; // slate #1B1F28 @ 0.8
            trackImg.raycastTarget = false;
            ApplyPackSocket(trackImg); // iron-stone trough (overrides flat-color when art present)

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
            resourceFillImage.color = ResFillCyan; // cyan #10A0C0 init; OnResourceChanged keeps the class tint
            resourceFillImage.raycastTarget = false;
            // Cyan seal-energy fill — this is its home. Color still set by OnResourceChanged (class tint).
            ApplyPackFill(resourceFillImage);
        }

        private void BuildEchoDisplay(RectTransform root)
        {
            // Above the bars, bottom-left, minimal.
            var groupGo = new GameObject("EchoDisplay", typeof(RectTransform));
            groupGo.transform.SetParent(root, false);
            var groupRt = groupGo.GetComponent<RectTransform>();
            groupRt.anchorMin = new Vector2(0f, 0f);
            groupRt.anchorMax = new Vector2(0f, 0f);
            groupRt.pivot = new Vector2(0f, 0f);
            groupRt.anchoredPosition = new Vector2(HudLeft, TextStackBottom);
            groupRt.sizeDelta = new Vector2(78f, 14f);

            var iconGo = new GameObject("EchoIcon", typeof(RectTransform));
            iconGo.transform.SetParent(groupGo.transform, false);
            var iconRt = iconGo.GetComponent<RectTransform>();
            iconRt.anchorMin = new Vector2(0f, 0.5f);
            iconRt.anchorMax = new Vector2(0f, 0.5f);
            iconRt.pivot = new Vector2(0.5f, 0.5f);
            iconRt.anchoredPosition = new Vector2(5f, -6f);
            iconRt.sizeDelta = new Vector2(7f, 7f);
            iconRt.localEulerAngles = new Vector3(0f, 0f, 45f);

            var icon = iconGo.AddComponent<Image>();
            icon.color = new Color(0.28f, 0.96f, 1f, 0.9f);
            icon.raycastTarget = false;

            var labelGo = new GameObject("EchoBalance", typeof(RectTransform));
            labelGo.transform.SetParent(groupGo.transform, false);
            var labelRt = labelGo.GetComponent<RectTransform>();
            labelRt.anchorMin = new Vector2(0f, 1f);
            labelRt.anchorMax = new Vector2(0f, 1f);
            labelRt.pivot = new Vector2(0f, 1f);
            labelRt.anchoredPosition = new Vector2(14f, 0f);
            labelRt.sizeDelta = new Vector2(62f, 14f);

            echoBalanceLabel = labelGo.AddComponent<TextMeshProUGUI>();
            echoBalanceLabel.text = "0";
            echoBalanceLabel.fontSize = 11f;
            echoBalanceLabel.fontStyle = FontStyles.Italic;
            echoBalanceLabel.color = new Color(0.82f, 0.96f, 1f, 0.70f);
            echoBalanceLabel.alignment = TextAlignmentOptions.Left;
            echoBalanceLabel.raycastTarget = false;
        }

        private static PlayerResourceBase ResolveResource(GameObject player, ClassType cls)
        {
            return cls switch
            {
                ClassType.Warblade => player.GetComponent<RageSystem>(),
                ClassType.Elementalist => player.GetComponent<ManaSystem>(),
                ClassType.Shadowblade => player.GetComponent<EnergySystem>(),
                ClassType.Ranger => player.GetComponent<FocusSystem>(),
                ClassType.Ronin => player.GetComponent<TensionSystem>(),
                _ => player.GetComponent<PlayerResourceBase>()
            };
        }

        private void BuildRoomNameLabel(RectTransform root)
        {
            // Above the Echo line, bottom-left, italic + minimal.
            var go = new GameObject("RoomName", typeof(RectTransform));
            go.transform.SetParent(root, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0f);
            rt.anchorMax = new Vector2(0f, 0f);
            rt.pivot = new Vector2(0f, 0f);
            rt.anchoredPosition = new Vector2(HudLeft, TextStackBottom + 18f);
            rt.sizeDelta = new Vector2(220f, 18f);

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

        private void BuildLowHpVignette(RectTransform root)
        {
            var go = new GameObject("LowHpVignette", typeof(RectTransform));
            go.transform.SetParent(root, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;

            lowHpVignette = go.AddComponent<Image>();
            lowHpVignette.sprite = Resources.Load<Sprite>("UI/RIMA/lowhp_vignette");
            if (lowHpVignette.sprite == null)
            {
                // No vignette art imported yet — don't render a full-screen solid; skip the effect.
                Destroy(go);
                lowHpVignette = null;
                return;
            }
            lowHpVignette.type = Image.Type.Simple;
            lowHpVignette.color = new Color(0.7f, 0.05f, 0.08f, 0f); // dark red; alpha driven by HP in OnHPChanged
            lowHpVignette.raycastTarget = false;
        }

        // ─── Control Hint (first-room tutorial line) ─────────────────────

        private void BuildControlHint(RectTransform root)
        {
            var go = new GameObject("ControlHint", typeof(RectTransform));
            go.transform.SetParent(root, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);
            rt.pivot     = new Vector2(0.5f, 0f);
            rt.anchoredPosition = new Vector2(0f, 18f);
            rt.sizeDelta = new Vector2(560f, 18f);

            _controlHintLabel = go.AddComponent<TextMeshProUGUI>();
            _controlHintLabel.text = "WASD Hareket  ·  SPACE Dash  ·  Q/E/R/F Beceri  ·  G Etkileşim  ·  ESC Menü";
            _controlHintLabel.fontSize = 13f;
            _controlHintLabel.color = RimaUITheme.TextMuted;
            _controlHintLabel.alignment = TextAlignmentOptions.Center;
            _controlHintLabel.raycastTarget = false;
            _controlHintLabel.outlineWidth = 0.18f;
            _controlHintLabel.outlineColor = new Color32(0, 0, 0, 180);
            _controlHintLabel.alpha = 0f; // hidden until ShowControlHintOnce fires
        }

        private void ShowControlHintOnce()
        {
            if (_controlHintShown || _controlHintLabel == null) return;
            _controlHintShown = true;
            StartCoroutine(ControlHintRoutine());
        }

        private IEnumerator ControlHintRoutine()
        {
            // Fade in (0.5s)
            float t = 0f;
            while (t < 0.5f)
            {
                t += Time.deltaTime;
                _controlHintLabel.alpha = Mathf.Clamp01(t / 0.5f) * 0.75f;
                yield return null;
            }
            _controlHintLabel.alpha = 0.75f;

            // Hold 8s
            yield return new WaitForSeconds(8f);

            // Fade out (1.5s)
            t = 0f;
            while (t < 1.5f)
            {
                t += Time.deltaTime;
                _controlHintLabel.alpha = Mathf.Clamp01(1f - t / 1.5f) * 0.75f;
                yield return null;
            }
            _controlHintLabel.alpha = 0f;
        }
    }
}
