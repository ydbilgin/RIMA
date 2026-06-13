#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA.UI.BuildMode
{
    /// <summary>
    /// ONE shared visual-style helper for the in-game Build Mode panels (Act1 canon).
    /// Both BuildPlacementController (left BUILD panel) and BuildTileBrushController (right TILE
    /// BRUSH panel) build their runtime uGUI through these factories so the two tools read as a
    /// single, deliberate, premium in-game build tool — not programmer art.
    ///
    /// Palette (project canon — STAGING / MEMORY): slate base #3A3D42; dark-slate panel bg #16181C
    /// at ~0.93 alpha; void purple #3A1A4A subtle accents; EMBER #E89020 = primary / selected; cyan
    /// only sparingly; text off-white #E8E8EC + muted #8A929C.
    ///
    /// Pure presentation: no scene/prefab edits, no gameplay logic. DisableDomainReload-safe
    /// (font cache is a plain static cleared by domain reload; under DisableDomainReload it simply
    /// stays warm, which is harmless).
    /// </summary>
    internal static class BuildModeUiStyle
    {
        // ---- Act1 palette -------------------------------------------------------------------
        public static readonly Color PanelBg      = Hex(0x16181C, 0.93f);  // dark slate panel
        public static readonly Color PanelBorder  = Hex(0x3A3D42, 1.00f);  // slate edge
        public static readonly Color HeaderText   = Hex(0xE8E8EC, 1.00f);  // off-white
        public static readonly Color MutedText    = Hex(0x8A929C, 1.00f);  // muted slate-grey
        public static readonly Color Ember        = Hex(0xE89020, 1.00f);  // primary / selected
        public static readonly Color EmberDim     = Hex(0xE89020, 0.55f);  // accent bars / underline
        public static readonly Color VoidPurple   = Hex(0x3A1A4A, 1.00f);  // subtle accent
        public static readonly Color ButtonIdle   = Hex(0x2A2D32, 1.00f);  // unselected button fill
        public static readonly Color ButtonBorder = Hex(0x3A3D42, 1.00f);  // unselected button edge
        public static readonly Color SelectedText = Hex(0x16181C, 1.00f);  // dark text on ember fill
        public static readonly Color HintBg       = Hex(0x101216, 0.95f);  // hint box (slightly darker)

        // ---- layout rhythm (consistent across both panels) ----------------------------------
        public const float PanelWidth = 232f;
        public const float Padding    = 16f;
        public const float ItemGap    = 8f;
        public const float BorderPx   = 1f;
        public const float AccentBarW = 4f;   // ember left accent on a selected button

        /// <summary>Mutable references for a built button, so callers can flip selected state.</summary>
        public sealed class ButtonStyle
        {
            public Button button;
            public Image background;
            public Image accentBar; // ember left bar shown only when selected
            public TextMeshProUGUI label;
        }

        // ---- font (matches DirectorMode's Jersey 10 SDF for in-game consistency) -------------
        private static TMP_FontAsset _font;
        private static bool _fontResolved;

        public static TMP_FontAsset Font
        {
            get
            {
                if (_fontResolved) return _font;
                _fontResolved = true;
#if UNITY_EDITOR
                _font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                    "Assets/Fonts/Jersey10/Jersey10-Regular SDF.asset");
#endif
                if (_font == null) _font = Resources.Load<TMP_FontAsset>("Fonts/Jersey10-Regular SDF");
                return _font; // null -> TMP falls back to its default font (still renders).
            }
        }

        // ---- factories ----------------------------------------------------------------------

        /// <summary>
        /// Dark-slate panel with a crisp 1px slate border. Returns the CONTENT RectTransform
        /// (already inset by the border + Padding) so callers add children straight into it.
        /// </summary>
        public static RectTransform MakePanel(Transform parent, string name, float width)
        {
            // Outer = border color; the inner bg sits 1px in, so the border reads as a hairline edge.
            // Fills its parent (the panel-root RectTransform the caller already sized + positioned).
            GameObject outerGo = new GameObject(name, typeof(RectTransform), typeof(Image));
            RectTransform outer = outerGo.GetComponent<RectTransform>();
            outer.SetParent(parent, false);
            Stretch(outer, Vector2.zero, Vector2.zero);
            outer.GetComponent<Image>().color = PanelBorder;

            GameObject bgGo = new GameObject("Bg", typeof(RectTransform), typeof(Image));
            RectTransform bg = bgGo.GetComponent<RectTransform>();
            bg.SetParent(outer, false);
            Stretch(bg, new Vector2(BorderPx, BorderPx), new Vector2(-BorderPx, -BorderPx));
            bg.GetComponent<Image>().color = PanelBg;

            GameObject contentGo = new GameObject("Content", typeof(RectTransform));
            RectTransform content = contentGo.GetComponent<RectTransform>();
            content.SetParent(bg, false);
            Stretch(content, new Vector2(Padding, Padding), new Vector2(-Padding, -Padding));
            return content;
        }

        /// <summary>
        /// Bold UPPERCASE header with letter-spacing + an ember underline accent bar beneath it.
        /// Anchored to the TOP of <paramref name="content"/>; returns its height (header + bar + gap)
        /// so callers know where the body starts.
        /// </summary>
        public static float MakeHeader(RectTransform content, string text)
        {
            const float headerH = 30f;
            const float barH = 3f;

            GameObject go = new GameObject("Header", typeof(RectTransform));
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(content, false);
            Top(rt, headerH, 0f);
            TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.font = Font;
            tmp.text = text.ToUpperInvariant();
            tmp.fontSize = 24f;
            tmp.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
            tmp.characterSpacing = 8f;
            tmp.color = HeaderText;
            tmp.alignment = TextAlignmentOptions.BottomLeft;
            tmp.raycastTarget = false;
            tmp.enableWordWrapping = false;

            GameObject barGo = new GameObject("HeaderBar", typeof(RectTransform), typeof(Image));
            RectTransform bar = barGo.GetComponent<RectTransform>();
            bar.SetParent(content, false);
            Top(bar, barH, headerH + 4f);
            // ember underline, left-weighted (40% width) so it reads as an accent, not a divider.
            bar.anchorMax = new Vector2(0.4f, bar.anchorMax.y);
            bar.GetComponent<Image>().color = Ember;

            return headerH + 4f + barH + ItemGap;
        }

        /// <summary>
        /// A premium button: slate fill + subtle border + an (initially hidden) ember LEFT accent
        /// bar. Selected state = ember fill + dark text + visible accent bar (call ApplySelected).
        /// </summary>
        public static ButtonStyle MakeButton(Transform parent, string text)
        {
            GameObject go = new GameObject("Btn_" + text, typeof(RectTransform), typeof(Image), typeof(Button));
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);

            Image bg = go.GetComponent<Image>();
            bg.color = ButtonIdle;

            // 1px inner border via a slightly-inset darker frame would need a second image; instead
            // we keep a single crisp fill and an outline-by-contrast border drawn as a thin child.
            GameObject borderGo = new GameObject("Border", typeof(RectTransform), typeof(Image));
            RectTransform border = borderGo.GetComponent<RectTransform>();
            border.SetParent(rt, false);
            Stretch(border, Vector2.zero, Vector2.zero);
            Image borderImg = border.GetComponent<Image>();
            borderImg.color = new Color(ButtonBorder.r, ButtonBorder.g, ButtonBorder.b, 0.9f);
            borderImg.raycastTarget = false;
            // inner fill on top of the border frame (1px reveal on all sides)
            GameObject innerGo = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            RectTransform inner = innerGo.GetComponent<RectTransform>();
            inner.SetParent(rt, false);
            Stretch(inner, new Vector2(BorderPx, BorderPx), new Vector2(-BorderPx, -BorderPx));
            Image innerImg = inner.GetComponent<Image>();
            innerImg.color = ButtonIdle;
            innerImg.raycastTarget = false;
            // The root Image stays as the Button target graphic but is hidden behind the fill;
            // make it transparent so only border + fill show.
            bg.color = new Color(0f, 0f, 0f, 0f);

            GameObject accentGo = new GameObject("Accent", typeof(RectTransform), typeof(Image));
            RectTransform accent = accentGo.GetComponent<RectTransform>();
            accent.SetParent(inner, false);
            accent.anchorMin = new Vector2(0f, 0f);
            accent.anchorMax = new Vector2(0f, 1f);
            accent.pivot = new Vector2(0f, 0.5f);
            accent.sizeDelta = new Vector2(AccentBarW, 0f);
            accent.anchoredPosition = Vector2.zero;
            Image accentImg = accent.GetComponent<Image>();
            accentImg.color = Ember;
            accentImg.raycastTarget = false;
            accentGo.SetActive(false);

            TextMeshProUGUI label = new GameObject("TMP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            label.rectTransform.SetParent(inner, false);
            Stretch(label.rectTransform, new Vector2(AccentBarW + 6f, 2f), new Vector2(-8f, -2f));
            label.font = Font;
            label.text = text;
            label.fontSize = 15f;
            label.fontStyle = FontStyles.Bold;
            label.alignment = TextAlignmentOptions.MidlineLeft;
            label.color = MutedText;
            label.enableWordWrapping = false;
            label.overflowMode = TextOverflowModes.Ellipsis;
            label.raycastTarget = false;

            return new ButtonStyle { button = go.GetComponent<Button>(), background = innerImg, accentBar = accentImg, label = label };
        }

        /// <summary>Flip a button between premium idle (slate, muted) and selected (ember, dark text).</summary>
        public static void ApplySelected(ButtonStyle b, bool selected)
        {
            if (b == null) return;
            if (b.background != null) b.background.color = selected ? Ember : ButtonIdle;
            if (b.label != null) b.label.color = selected ? SelectedText : MutedText;
            if (b.accentBar != null) b.accentBar.gameObject.SetActive(selected);
        }

        /// <summary>
        /// Dark hint box anchored bottom-left of <paramref name="content"/>, muted monospace-ish
        /// hotkey text. Returns the TMP so the caller can update the hotkey list at runtime.
        /// </summary>
        public static TextMeshProUGUI MakeHintBox(RectTransform content, float height)
        {
            GameObject boxGo = new GameObject("HintBox", typeof(RectTransform), typeof(Image));
            RectTransform box = boxGo.GetComponent<RectTransform>();
            box.SetParent(content, false);
            box.anchorMin = new Vector2(0f, 0f);
            box.anchorMax = new Vector2(1f, 0f);
            box.pivot = new Vector2(0.5f, 0f);
            box.sizeDelta = new Vector2(0f, height);
            box.anchoredPosition = Vector2.zero;
            Image bg = box.GetComponent<Image>();
            bg.color = HintBg;
            bg.raycastTarget = false;

            // thin ember top-edge so the hint reads as a deliberate footer.
            GameObject edgeGo = new GameObject("Edge", typeof(RectTransform), typeof(Image));
            RectTransform edge = edgeGo.GetComponent<RectTransform>();
            edge.SetParent(box, false);
            edge.anchorMin = new Vector2(0f, 1f);
            edge.anchorMax = new Vector2(1f, 1f);
            edge.pivot = new Vector2(0.5f, 1f);
            edge.sizeDelta = new Vector2(0f, 1f);
            edge.anchoredPosition = Vector2.zero;
            Image edgeImg = edge.GetComponent<Image>();
            edgeImg.color = EmberDim;
            edgeImg.raycastTarget = false;

            TextMeshProUGUI tmp = new GameObject("TMP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            tmp.rectTransform.SetParent(box, false);
            Stretch(tmp.rectTransform, new Vector2(10f, 8f), new Vector2(-10f, -8f));
            tmp.font = Font;
            tmp.fontSize = 13f;
            tmp.fontStyle = FontStyles.Normal;
            tmp.characterSpacing = 1.5f;
            tmp.color = MutedText;
            tmp.alignment = TextAlignmentOptions.TopLeft;
            tmp.enableWordWrapping = true;
            tmp.raycastTarget = false;
            return tmp;
        }

        // ---- low-level RectTransform helpers -------------------------------------------------

        public static void Stretch(RectTransform rt, Vector2 offsetMin, Vector2 offsetMax)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = offsetMin;
            rt.offsetMax = offsetMax;
        }

        /// <summary>Anchor a row to the top of its parent at <paramref name="yFromTop"/>, full width.</summary>
        public static void Top(RectTransform rt, float height, float yFromTop)
        {
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.sizeDelta = new Vector2(0f, height);
            rt.anchoredPosition = new Vector2(0f, -yFromTop);
        }

        private static Color Hex(int rgb, float a)
        {
            return new Color(((rgb >> 16) & 0xFF) / 255f, ((rgb >> 8) & 0xFF) / 255f, (rgb & 0xFF) / 255f, a);
        }
    }
}
#endif
