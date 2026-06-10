using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Runtime-built overlay that fires after boss death and lets the player pick a secondary class.
    /// Subscribes to PlayerClassManager.OnClassSelectionRequested; pauses the game (timeScale=0)
    /// while open; resumes + calls SelectSecondaryClass on confirmation.
    ///
    /// Bootstrap: [RuntimeInitializeOnLoadMethod] creates a DontDestroyOnLoad host so no scene wiring
    /// is required — the overlay exists in every scene automatically.
    /// </summary>
    public class ClassSelectionUI : MonoBehaviour
    {
        // ── Singleton guard ──────────────────────────────────────────────
        private static ClassSelectionUI _instance;

        // ── UI references ────────────────────────────────────────────────
        private Canvas _canvas;
        private bool   _isOpen;

        // ── Class data for the two demo choices ──────────────────────────
        // DEMO LOCK (2026-06-10): only Warblade ↔ Elementalist are valid secondaries.
        // Both classes have ClassKits + controller-routing. Showing Ranger here would
        // result in an empty skill bar (no kit defined). Expand pool when kits land.
        private static readonly (ClassType type, string accentHex, string descLine1, string descLine2) ElementalistCard =
        (
            ClassType.Elementalist,
            "#FFDD00",
            "CASTER · RİTİM · ELEMENTLER",
            "Ateş. Buz. Şimşek. Üçüncü volta güç biriktir."
        );

        private static readonly (ClassType type, string accentHex, string descLine1, string descLine2) WarbladeCard =
        (
            ClassType.Warblade,
            "#F2610F",
            "YAKIN · AĞIR · MOMENTUM",
            "Büyük kılıç. Ağır vuruş. Momentum zinciri kur."
        );

        /// <summary>
        /// Returns a two-card array with exactly the two demo classes.
        /// Primary class is always excluded — the two shown are the OTHER class
        /// (the valid secondary) plus itself as the first/second slot.
        /// With only 2 demo classes the result is always the same pair in
        /// [secondary, primary] order so the panel layout stays consistent.
        /// </summary>
        private static (ClassType type, string accentHex, string descLine1, string descLine2)[] BuildChoices()
        {
            ClassType primary = PlayerClassManager.Instance != null
                ? PlayerClassManager.Instance.PrimaryClass
                : ClassType.None;

            // Only two valid demo secondaries: whichever the player didn't pick as primary.
            // We still show two cards (left = non-primary, right = fallback matching non-primary
            // description) so the two-column UI layout is preserved.
            // Both slots always resolve to kit-equipped classes.
            if (primary == ClassType.Warblade)
                return new[] { ElementalistCard, ElementalistCard };
            if (primary == ClassType.Elementalist)
                return new[] { WarbladeCard, WarbladeCard };

            // Fallback (shouldn't reach in demo, but be safe): offer Elementalist first.
            return new[] { ElementalistCard, WarbladeCard };
        }

        // ── Bootstrap ────────────────────────────────────────────────────

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            // If already exists (scene reload) — skip.
            if (_instance != null) return;

            var go = new GameObject("[ClassSelectionUI]");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<ClassSelectionUI>();
        }

        // ── Lifecycle ────────────────────────────────────────────────────

        private void Awake()
        {
            // Enforce single instance across scene reloads.
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void OnEnable()
        {
            SubscribeToManager();
        }

        private void OnDisable()
        {
            if (_subscribedManager != null)
                _subscribedManager.OnClassSelectionRequested -= HandleClassSelectionRequested;
            _subscribedManager = null;
        }

        private void Start()
        {
            // In case PlayerClassManager wasn't ready during OnEnable, try again.
            SubscribeToManager();
        }

        /// <summary>Re-checks every frame: PlayerClassManager is a SCENE object (no DDOL), so a
        /// death-restart or Play-Again reload destroys it and spawns a fresh instance. This overlay
        /// IS DDOL — a one-shot subscribed flag would keep pointing at the dead manager and the
        /// selection would never open on the second run (softlock at boss). Track the instance.</summary>
        private void Update()
        {
            if (!_isOpen)
                SubscribeToManager(); // re-subscribes whenever the manager instance changes
        }

        // ── Subscription helper (safe against null/replaced Instance) ────

        private PlayerClassManager _subscribedManager;

        private void SubscribeToManager()
        {
            var mgr = PlayerClassManager.Instance;
            if (mgr == null || ReferenceEquals(mgr, _subscribedManager)) return;
            if (_subscribedManager != null)
                _subscribedManager.OnClassSelectionRequested -= HandleClassSelectionRequested;
            mgr.OnClassSelectionRequested += HandleClassSelectionRequested;
            _subscribedManager = mgr;
        }

        // ── Handler ──────────────────────────────────────────────────────

        private void HandleClassSelectionRequested()
        {
            if (_isOpen) return;
            Show();
        }

        // ── Build & Show ─────────────────────────────────────────────────

        private void Show()
        {
            _isOpen = true;
            Time.timeScale = 0f;

            EnsureEventSystem();
            BuildUI();
        }

        private void BuildUI()
        {
            // Destroy any leftover canvas from a previous call (shouldn't happen, but be safe).
            if (_canvas != null) Destroy(_canvas.gameObject);

            var canvasGo = new GameObject("ClassSelection_Canvas");
            canvasGo.transform.SetParent(transform, false);
            _canvas = canvasGo.AddComponent<Canvas>();
            _canvas.renderMode    = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder  = 190; // below DemoCompleteOverlay (200) but above gameplay
            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();

            // ── Dark full-screen backdrop ──────────────────────────────────
            Image bg = CreateImage("BG", _canvas.transform, new Color(0.04f, 0.04f, 0.06f, 0.92f));
            Stretch(bg.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            // ── Central panel ──────────────────────────────────────────────
            var panel = CreatePanel("Panel", _canvas.transform,
                new Color(0.07f, 0.08f, 0.12f, 0.96f),
                RimaUITheme.Cyan);
            Stretch(panel, new Vector2(0.15f, 0.18f), new Vector2(0.85f, 0.82f), Vector2.zero, Vector2.zero);

            // ── Header label ───────────────────────────────────────────────
            TextMeshProUGUI header = CreateText("Header", panel,
                "DUAL-CLASS  ·  İKİNCİL SINIF SEÇ", 28f,
                RimaUITheme.Cyan, TextAlignmentOptions.Center);
            header.fontStyle   = FontStyles.Bold;
            header.outlineWidth  = 0.18f;
            header.outlineColor  = new Color32(0, 0, 0, 220);
            Stretch(header.rectTransform, new Vector2(0f, 0.82f), new Vector2(1f, 1f),
                new Vector2(16f, 0f), new Vector2(-16f, -8f));

            // ── Sub-label ──────────────────────────────────────────────────
            string primaryName = (PlayerClassManager.Instance?.PrimaryClass ?? ClassType.Warblade).ToString();
            TextMeshProUGUI sub = CreateText("Sub", panel,
                $"{primaryName} becerilerin yanına bir yol daha aç. Bu seçim kalıcıdır.",
                15f, RimaUITheme.TextMuted, TextAlignmentOptions.Center);
            Stretch(sub.rectTransform, new Vector2(0.04f, 0.72f), new Vector2(0.96f, 0.84f), Vector2.zero, Vector2.zero);

            // ── Divider ────────────────────────────────────────────────────
            Image divider = CreateImage("Divider", panel, new Color(0.20f, 0.28f, 0.38f, 0.55f));
            Stretch(divider.rectTransform, new Vector2(0.04f, 0.695f), new Vector2(0.96f, 0.705f), Vector2.zero, Vector2.zero);

            // ── Class cards ────────────────────────────────────────────────
            // DEMO: only Warblade ↔ Elementalist are valid secondaries, so BuildChoices()
            // returns a single unique class (same card in both slots). Show one wide centered
            // card instead of two duplicates.
            var choices = BuildChoices();
            bool singleChoice = choices[0].type == choices[1].type;
            if (singleChoice)
            {
                BuildClassCard(panel, choices[0],
                    new Vector2(0.20f, 0.08f), new Vector2(0.80f, 0.68f));
            }
            else
            {
                float cardLeft  = 0.04f;
                float cardRight = 0.96f;
                float cardMid   = (cardLeft + cardRight) / 2f;
                float gap       = 0.015f;
                BuildClassCard(panel, choices[0],
                    new Vector2(cardLeft, 0.08f), new Vector2(cardMid - gap, 0.68f));
                BuildClassCard(panel, choices[1],
                    new Vector2(cardMid + gap, 0.08f), new Vector2(cardRight, 0.68f));
            }
        }

        private void BuildClassCard(
            RectTransform parent,
            (ClassType type, string accentHex, string descLine1, string descLine2) data,
            Vector2 anchorMin, Vector2 anchorMax)
        {
            ColorUtility.TryParseHtmlString(data.accentHex, out Color accent);

            // Card background
            var card = CreatePanel($"Card_{data.type}", parent,
                new Color(0.06f, 0.07f, 0.10f, 0.94f), accent);
            Stretch(card, anchorMin, anchorMax, Vector2.zero, Vector2.zero);

            // Accent top bar
            Image topBar = CreateImage("AccentBar", card, new Color(accent.r, accent.g, accent.b, 0.72f));
            Stretch(topBar.rectTransform, new Vector2(0f, 0.88f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);

            // Class name
            TextMeshProUGUI className = CreateText("ClassName", card,
                data.type.ToString().ToUpper(), 24f,
                Color.white, TextAlignmentOptions.Center);
            className.fontStyle    = FontStyles.Bold;
            className.outlineWidth = 0.15f;
            className.outlineColor = new Color32(0, 0, 0, 200);
            Stretch(className.rectTransform, new Vector2(0f, 0.66f), new Vector2(1f, 0.88f), Vector2.zero, Vector2.zero);

            // Tag line 1
            TextMeshProUGUI tag1 = CreateText("Tag1", card,
                data.descLine1, 11f, new Color(accent.r, accent.g, accent.b, 0.9f),
                TextAlignmentOptions.Center);
            tag1.fontStyle = FontStyles.Bold;
            Stretch(tag1.rectTransform, new Vector2(0.04f, 0.52f), new Vector2(0.96f, 0.66f), Vector2.zero, Vector2.zero);

            // Description line
            TextMeshProUGUI desc = CreateText("Desc", card,
                data.descLine2, 12f, RimaUITheme.TextMuted, TextAlignmentOptions.Center);
            desc.enableWordWrapping = true;
            Stretch(desc.rectTransform, new Vector2(0.06f, 0.28f), new Vector2(0.94f, 0.52f), Vector2.zero, Vector2.zero);

            // Resource name pill
            string resourceName = RimaUITheme.ResourceName(data.type);
            TextMeshProUGUI resource = CreateText("Resource", card,
                $"KAYNAK  ·  {resourceName}", 10f,
                new Color(accent.r, accent.g, accent.b, 0.70f), TextAlignmentOptions.Center);
            Stretch(resource.rectTransform, new Vector2(0.04f, 0.18f), new Vector2(0.96f, 0.28f), Vector2.zero, Vector2.zero);

            // Select button
            Button btn = CreateButton($"SelectBtn_{data.type}", card,
                "SEÇ", accent, RimaUITheme.BackgroundDark, 17f);
            Stretch((RectTransform)btn.transform, new Vector2(0.12f, 0.04f), new Vector2(0.88f, 0.16f), Vector2.zero, Vector2.zero);

            var capturedType = data.type;
            btn.onClick.AddListener(() => OnClassChosen(capturedType));
        }

        // ── Selection callback ────────────────────────────────────────────

        private void OnClassChosen(ClassType type)
        {
            Time.timeScale = 1f;
            _isOpen = false;

            PlayerClassManager.Instance?.SelectSecondaryClass(type);

            if (_canvas != null)
            {
                Destroy(_canvas.gameObject);
                _canvas = null;
            }
        }

        // ── UI helpers (mirrored from DemoCompleteOverlay) ────────────────

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;
            var go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<InputSystemUIInputModule>();
        }

        private static RectTransform CreatePanel(string name, Transform parent, Color fill, Color edge)
        {
            Image panel = CreateImage(name, parent, fill);
            panel.sprite = RimaUITheme.SmallPanelFrame;
            panel.type   = Image.Type.Sliced;

            Image border = CreateImage("Edge", panel.transform, edge);
            border.sprite = RimaUITheme.SmallPanelFrame;
            border.type   = Image.Type.Sliced;
            border.raycastTarget = false;
            Stretch(border.rectTransform, Vector2.zero, Vector2.one,
                new Vector2(-3f, -3f), new Vector2(3f, 3f));
            border.transform.SetAsFirstSibling();

            return panel.rectTransform;
        }

        private static Image CreateImage(string name, Transform parent, Color color)
        {
            var go  = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var img = go.AddComponent<Image>();
            img.color = color;
            return img;
        }

        private static TextMeshProUGUI CreateText(string name, Transform parent, string text,
            float size, Color color, TextAlignmentOptions alignment)
        {
            var go  = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text      = text;
            tmp.fontSize  = size;
            tmp.color     = color;
            tmp.alignment = alignment;
            tmp.raycastTarget = false;
            return tmp;
        }

        private static Button CreateButton(string name, Transform parent,
            string label, Color edge, Color textColor, float fontSize)
        {
            Image bg = CreateImage(name, parent, new Color(0.04f, 0.05f, 0.08f, 0.96f));
            bg.sprite = RimaUITheme.SmallPanelFrame;
            bg.type   = Image.Type.Sliced;

            Image border = CreateImage("Edge", bg.transform, edge);
            border.sprite = RimaUITheme.SmallPanelFrame;
            border.type   = Image.Type.Sliced;
            border.raycastTarget = false;
            Stretch(border.rectTransform, Vector2.zero, Vector2.one,
                new Vector2(-2f, -2f), new Vector2(2f, 2f));
            border.transform.SetAsFirstSibling();

            Button button = bg.gameObject.AddComponent<Button>();
            ColorBlock cb = button.colors;
            cb.normalColor      = Color.white;
            cb.highlightedColor = new Color(0.85f, 1f, 0.96f, 1f);
            cb.pressedColor     = new Color(0.65f, 0.9f, 0.84f, 1f);
            button.colors = cb;

            TextMeshProUGUI txt = CreateText("Label", bg.transform, label, fontSize,
                textColor, TextAlignmentOptions.Center);
            txt.fontStyle = FontStyles.Bold;
            Stretch(txt.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            return button;
        }

        private static void Stretch(RectTransform rt,
            Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            rt.anchorMin  = min;
            rt.anchorMax  = max;
            rt.offsetMin  = offsetMin;
            rt.offsetMax  = offsetMax;
        }
    }
}
