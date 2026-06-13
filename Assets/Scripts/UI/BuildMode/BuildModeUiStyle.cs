#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        // BRIGHTENED (consolidation item 6 — user: "too dark"). The slate ramp is lifted ~1.4-1.6x so
        // panels/buttons/borders/muted text read on a lit display; ember/void/text-on-ember stay put.
        public static readonly Color PanelBg      = Hex(0x202329, 0.93f);  // dark slate panel (lifted)
        public static readonly Color PanelBorder  = Hex(0x4A4E55, 1.00f);  // slate edge (now visible)
        public static readonly Color HeaderText   = Hex(0xE8E8EC, 1.00f);  // off-white (unchanged)
        public static readonly Color MutedText    = Hex(0xA6ADB6, 1.00f);  // muted slate-grey (lifted)
        public static readonly Color Ember        = Hex(0xE89020, 1.00f);  // primary / selected (unchanged)
        public static readonly Color EmberDim     = Hex(0xE89020, 0.55f);  // accent bars / underline (unchanged)
        public static readonly Color VoidPurple   = Hex(0x3A1A4A, 1.00f);  // subtle accent (unchanged)
        public static readonly Color ButtonIdle   = Hex(0x363A41, 1.00f);  // unselected button fill (raised)
        public static readonly Color ButtonBorder = Hex(0x4A4E55, 1.00f);  // unselected button edge (visible)
        public static readonly Color SelectedText = Hex(0x16181C, 1.00f);  // dark text on ember fill (unchanged)
        public static readonly Color HintBg       = Hex(0x181B20, 0.95f);  // hint box (lifted, still darkest)

        // ---- NEW tokens (design spec 1.1 elevation ramp + state matrix) ----------------------
        // Depth is expressed by LIGHTER surfaces, never drop shadows (dark-UI convention).
        public static readonly Color CardIdle       = Hex(0x2C2F36, 1.00f);  // asset card / raised tile (elev 2, lifted)
        public static readonly Color SurfaceHover   = Hex(0x3E424A, 1.00f);  // hover lighten (nudged up)
        public static readonly Color SurfacePressed = Hex(0x24272D, 1.00f);  // press darken (lifted)
        public static readonly Color BorderHover    = Hex(0x4A4E55, 1.00f);  // brightened edge on hover
        public static readonly Color EmberGlow      = Hex(0xE89020, 0.22f);  // selected-card outer wash
        public static readonly Color DisabledText   = Hex(0x8A929C, 0.45f);  // unavailable item / tool
        public static readonly Color ScrollHandle   = Hex(0x3A3D42, 0.85f);  // thin scrollbar handle
        public static readonly Color TopHighlight   = new Color(1f, 1f, 1f, 0.04f); // 1px premium top edge

        // ---- layout rhythm (consistent across both panels) ----------------------------------
        public const float PanelWidth = 232f;
        public const float Padding    = 16f;
        public const float ItemGap    = 8f;
        public const float BorderPx   = 1f;
        public const float AccentBarW = 4f;   // ember left accent on a selected button

        // 4/8 spacing scale (design spec 1.2). Padding(=Space4) + ItemGap(=Space2) keep their names.
        public const float Space1 = 4f;
        public const float Space2 = 8f;
        public const float Space3 = 12f;
        public const float Space4 = 16f;
        public const float Space6 = 24f;

        // Corner radii (design spec 1.3) realized via runtime-generated rounded 9-slice sprites.
        public const int RadiusSm = 4;   // chips, search field, scrollbar handle
        public const int RadiusMd = 6;   // buttons, asset cards
        public const int RadiusLg = 10;  // panel outer frame

        // State-matrix transition timing (design spec 1.6) — coroutine crossfade, no DOTween.
        public const float JuiceFade  = 0.08f;
        public const float HoverScale = 1.03f;
        public const float PressScale = 0.97f;

        /// <summary>Mutable references for a built button, so callers can flip selected state.</summary>
        public sealed class ButtonStyle
        {
            public Button button;
            public Image background;
            public Image accentBar; // ember left bar shown only when selected
            public TextMeshProUGUI label;
            // NEW (design spec 1.6 state matrix): the border + scale-target + juice so hover/press
            // states can layer on top of the existing selected flag. Older callers ignore these.
            public Image border;
            public RectTransform scaleTarget;
            public ButtonJuice juice;
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

        // ---- procedural rounded-rect 9-slice (design spec 1.3) ------------------------------
        // Generated once per radius, cached in a static dict (DisableDomainReload-safe like _font:
        // under DisableDomainReload it simply stays warm, which is harmless). Served as a Sliced
        // Image so it stays crisp at any panel size with zero new shader/asset/package dependency.
        private static readonly Dictionary<int, Sprite> _rounded = new Dictionary<int, Sprite>();

        /// <summary>
        /// A white rounded-rect Sprite with 9-slice borders = <paramref name="radius"/>. Tint via
        /// Image.color and set Image.type = Sliced. Antialiased edge so corners read clean.
        /// </summary>
        public static Sprite RoundedSprite(int radius)
        {
            if (radius < 1) radius = 1;
            if (_rounded.TryGetValue(radius, out Sprite cached) && cached != null) return cached;

            // The texture is just big enough to hold the 4 rounded corners + a 1px stretchable middle.
            int size = radius * 2 + 1;
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            Color32[] pixels = new Color32[size * size];
            float r = radius;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // distance from the nearest corner-center -> signed-distance to the rounded edge.
                    float cx = x < radius ? r : (x > radius ? size - 1 - r : x);
                    float cy = y < radius ? r : (y > radius ? size - 1 - r : y);
                    float dx = x - cx;
                    float dy = y - cy;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    // 1px antialias band at the rounded edge; inside = opaque, outside = transparent.
                    float a = Mathf.Clamp01(r - dist + 0.5f);
                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
            }
            tex.SetPixels32(pixels);
            tex.Apply(false, false);

            Vector4 border = new Vector4(radius, radius, radius, radius);
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f),
                100f, 0, SpriteMeshType.FullRect, border);
            sprite.hideFlags = HideFlags.DontSave;
            _rounded[radius] = sprite;
            return sprite;
        }

        /// <summary>Apply the rounded 9-slice sprite at <paramref name="radius"/> to an existing Image.</summary>
        public static void Round(Image img, int radius)
        {
            if (img == null) return;
            img.sprite = RoundedSprite(radius);
            img.type = Image.Type.Sliced;
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
            Image outerImg = outerGo.GetComponent<Image>();
            outerImg.color = PanelBorder;
            Round(outerImg, RadiusLg);   // rounded outer frame (design spec 2.1)

            GameObject bgGo = new GameObject("Bg", typeof(RectTransform), typeof(Image));
            RectTransform bg = bgGo.GetComponent<RectTransform>();
            bg.SetParent(outer, false);
            Stretch(bg, new Vector2(BorderPx, BorderPx), new Vector2(-BorderPx, -BorderPx));
            Image bgImg = bgGo.GetComponent<Image>();
            bgImg.color = PanelBg;
            Round(bgImg, RadiusLg - 1);  // inner bg rounded just inside the hairline edge

            // 1px premium top-highlight strip just inside the top inner edge (depth, not shadow).
            AddTopHighlight(bg);

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
            Round(borderImg, RadiusMd);   // rounded button frame (design spec 2.3)
            // inner fill on top of the border frame (1px reveal on all sides)
            GameObject innerGo = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            RectTransform inner = innerGo.GetComponent<RectTransform>();
            inner.SetParent(rt, false);
            Stretch(inner, new Vector2(BorderPx, BorderPx), new Vector2(-BorderPx, -BorderPx));
            Image innerImg = inner.GetComponent<Image>();
            innerImg.color = ButtonIdle;
            innerImg.raycastTarget = false;
            Round(innerImg, RadiusMd - 1);
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

            ButtonStyle style = new ButtonStyle
            {
                button = go.GetComponent<Button>(),
                background = innerImg,
                accentBar = accentImg,
                label = label,
                border = borderImg,
                scaleTarget = rt
            };
            style.juice = AttachJuice(style, ButtonIdle, ButtonBorder);
            return style;
        }

        /// <summary>
        /// Flip a button between premium idle (slate, muted) and selected (ember, dark text).
        /// The hover/press layer (ButtonJuice) reads the selected flag and keeps ember while selected.
        /// </summary>
        public static void ApplySelected(ButtonStyle b, bool selected)
        {
            if (b == null) return;
            if (b.juice != null) b.juice.SetSelected(selected); // single authority drives the full matrix.
            else
            {
                if (b.background != null) b.background.color = selected ? Ember : ButtonIdle;
                if (b.label != null) b.label.color = selected ? SelectedText : MutedText;
            }
            if (b.accentBar != null) b.accentBar.gameObject.SetActive(selected);
        }

        /// <summary>Attach + configure a ButtonJuice for a built control (button or card).</summary>
        public static ButtonJuice AttachJuice(ButtonStyle b, Color idleFill, Color idleBorder)
        {
            if (b == null || b.button == null) return null;
            ButtonJuice j = b.button.gameObject.GetComponent<ButtonJuice>();
            if (j == null) j = b.button.gameObject.AddComponent<ButtonJuice>();
            j.Bind(b, idleFill, idleBorder);
            return j;
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

        // ---- NEW component factories (design spec section 2) --------------------------------

        /// <summary>Spec 1.5: a 1px faint white top-highlight strip just inside the top inner edge.</summary>
        public static void AddTopHighlight(RectTransform parent)
        {
            if (parent == null) return;
            GameObject go = new GameObject("TopHighlight", typeof(RectTransform), typeof(Image));
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.sizeDelta = new Vector2(-(RadiusLg * 2f), 1f);
            rt.anchoredPosition = new Vector2(0f, -1f);
            Image img = go.GetComponent<Image>();
            img.color = TopHighlight;
            img.raycastTarget = false;
        }

        /// <summary>
        /// Spec 2.4: a segmented toggle (one rounded track, N equal segments). Promotes the inline
        /// PROP|TILE control. Returns the ButtonStyle[] (one per segment) so the caller wires clicks
        /// + ApplySelected. Layout is a HorizontalLayoutGroup so segments are equal-width.
        /// </summary>
        public static ButtonStyle[] MakeSegmented(RectTransform parent, string[] labels)
        {
            if (labels == null || labels.Length == 0) return Array.Empty<ButtonStyle>();
            HorizontalLayoutGroup hl = parent.GetComponent<HorizontalLayoutGroup>();
            if (hl == null) hl = parent.gameObject.AddComponent<HorizontalLayoutGroup>();
            hl.spacing = Space1;
            hl.childControlWidth = true;
            hl.childForceExpandWidth = true;
            hl.childControlHeight = true;
            hl.childForceExpandHeight = true;

            ButtonStyle[] segs = new ButtonStyle[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                ButtonStyle b = MakeButton(parent, labels[i]);
                b.label.alignment = TextAlignmentOptions.Center;
                b.label.fontSize = 16f;
                b.label.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
                b.label.characterSpacing = 4f;
                segs[i] = b;
            }
            return segs;
        }

        /// <summary>One data row for the category tab bar (spec 2.5.1).</summary>
        public sealed class TabSpec
        {
            public string label;
            public int count;
            public Color dot;
        }

        /// <summary>Mutable handle for a built tab so the caller can flip selected + update the count.</summary>
        public sealed class TabHandle
        {
            public Button button;
            public Image underline;     // ember underline shown when selected
            public Image background;
            public TextMeshProUGUI label;
            public ButtonJuice juice;
        }

        /// <summary>
        /// Spec 2.5.1: a DATA-DRIVEN horizontal category tab bar. Adding a category = one TabSpec in
        /// the list, no UI rewrite. Selected = ember underline + bright label (NOT an ember fill, to
        /// keep the bar calm and reserve ember-fill for the cards). Builds a HorizontalLayoutGroup row.
        /// </summary>
        public static TabHandle[] MakeTabBar(RectTransform parent, IReadOnlyList<TabSpec> tabs, Action<int> onSelect)
        {
            HorizontalLayoutGroup hl = parent.GetComponent<HorizontalLayoutGroup>();
            if (hl == null) hl = parent.gameObject.AddComponent<HorizontalLayoutGroup>();
            hl.spacing = Space1;
            hl.childControlWidth = true;
            hl.childForceExpandWidth = true;
            hl.childControlHeight = true;
            hl.childForceExpandHeight = true;
            hl.childAlignment = TextAnchor.MiddleCenter;

            int n = tabs != null ? tabs.Count : 0;
            TabHandle[] handles = new TabHandle[n];
            for (int i = 0; i < n; i++)
            {
                TabSpec t = tabs[i];
                int captured = i;

                GameObject go = new GameObject("Tab_" + t.label, typeof(RectTransform), typeof(Image), typeof(Button));
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.SetParent(parent, false);
                Image bg = go.GetComponent<Image>();
                bg.color = ButtonIdle;
                Round(bg, RadiusSm);

                TextMeshProUGUI lbl = new GameObject("TMP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
                lbl.rectTransform.SetParent(rt, false);
                Stretch(lbl.rectTransform, new Vector2(2f, 0f), new Vector2(-2f, 2f));
                lbl.font = Font;
                lbl.text = t.count > 0 ? $"{t.label} {t.count}" : t.label;
                lbl.fontSize = 13f;
                lbl.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
                lbl.characterSpacing = 2f;
                lbl.alignment = TextAlignmentOptions.Center;
                lbl.color = MutedText;
                lbl.enableWordWrapping = false;
                lbl.overflowMode = TextOverflowModes.Ellipsis;
                lbl.raycastTarget = false;

                GameObject ulGo = new GameObject("Underline", typeof(RectTransform), typeof(Image));
                RectTransform ul = ulGo.GetComponent<RectTransform>();
                ul.SetParent(rt, false);
                ul.anchorMin = new Vector2(0.15f, 0f);
                ul.anchorMax = new Vector2(0.85f, 0f);
                ul.pivot = new Vector2(0.5f, 0f);
                ul.sizeDelta = new Vector2(0f, 2f);
                ul.anchoredPosition = Vector2.zero;
                Image ulImg = ulGo.GetComponent<Image>();
                ulImg.color = Ember;
                ulImg.raycastTarget = false;
                ulGo.SetActive(false);

                TabHandle h = new TabHandle { button = go.GetComponent<Button>(), underline = ulImg, background = bg, label = lbl };
                handles[i] = h;
                if (onSelect != null) h.button.onClick.AddListener(() => onSelect(captured));
            }
            return handles;
        }

        /// <summary>Flip a tab between idle (muted) and selected (ember underline + bright label).</summary>
        public static void ApplyTabSelected(TabHandle h, bool selected)
        {
            if (h == null) return;
            if (h.underline != null) h.underline.gameObject.SetActive(selected);
            if (h.label != null) h.label.color = selected ? HeaderText : MutedText;
            if (h.background != null) h.background.color = selected ? SurfacePressed : ButtonIdle;
        }

        /// <summary>
        /// Spec 2.5.2: a search field with a leading glyph (procedural ring+handle) + ember focus
        /// ring. Filtering logic (incl. '-' exclude) lives in the caller via the onChanged callback.
        /// </summary>
        public static TMP_InputField MakeSearchField(RectTransform parent, Action<string> onChanged)
        {
            GameObject go = new GameObject("Search", typeof(RectTransform), typeof(Image));
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            Stretch(rt, Vector2.zero, Vector2.zero);
            Image bg = go.GetComponent<Image>();
            bg.color = PanelBg;
            Round(bg, RadiusSm);

            // border frame that turns ember on focus.
            GameObject brGo = new GameObject("Border", typeof(RectTransform), typeof(Image));
            RectTransform br = brGo.GetComponent<RectTransform>();
            br.SetParent(rt, false);
            Stretch(br, Vector2.zero, Vector2.zero);
            Image brImg = brGo.GetComponent<Image>();
            brImg.color = PanelBorder;
            brImg.raycastTarget = false;
            Round(brImg, RadiusSm);
            // re-inset the fill so the border reads as a hairline.
            GameObject fillGo = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            RectTransform fill = fillGo.GetComponent<RectTransform>();
            fill.SetParent(rt, false);
            Stretch(fill, new Vector2(BorderPx, BorderPx), new Vector2(-BorderPx, -BorderPx));
            Image fillImg = fillGo.GetComponent<Image>();
            fillImg.color = PanelBg;
            fillImg.raycastTarget = false;
            Round(fillImg, RadiusSm);

            // leading search glyph (a magnifier drawn as a Jersey10 char keeps it dependency-free).
            TextMeshProUGUI glyph = new GameObject("Glyph", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            glyph.rectTransform.SetParent(rt, false);
            glyph.rectTransform.anchorMin = new Vector2(0f, 0f);
            glyph.rectTransform.anchorMax = new Vector2(0f, 1f);
            glyph.rectTransform.pivot = new Vector2(0f, 0.5f);
            glyph.rectTransform.sizeDelta = new Vector2(24f, 0f);
            glyph.rectTransform.anchoredPosition = new Vector2(6f, 0f);
            glyph.font = Font;
            glyph.text = "[o]"; // a compact, on-brand magnifier stand-in (single font, no atlas).
            glyph.fontSize = 13f;
            glyph.color = MutedText;
            glyph.alignment = TextAlignmentOptions.Center;
            glyph.raycastTarget = false;

            TextMeshProUGUI textArea = new GameObject("Text", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            textArea.rectTransform.SetParent(rt, false);
            Stretch(textArea.rectTransform, new Vector2(28f, 1f), new Vector2(-8f, -1f));
            textArea.font = Font;
            textArea.fontSize = 14f;
            textArea.color = HeaderText;
            textArea.alignment = TextAlignmentOptions.MidlineLeft;
            textArea.enableWordWrapping = false;
            textArea.overflowMode = TextOverflowModes.Ellipsis;

            TextMeshProUGUI placeholder = new GameObject("Placeholder", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            placeholder.rectTransform.SetParent(rt, false);
            Stretch(placeholder.rectTransform, new Vector2(28f, 1f), new Vector2(-8f, -1f));
            placeholder.font = Font;
            placeholder.fontSize = 14f;
            placeholder.fontStyle = FontStyles.Italic;
            placeholder.color = MutedText;
            placeholder.text = "Search...";
            placeholder.alignment = TextAlignmentOptions.MidlineLeft;
            placeholder.enableWordWrapping = false;
            placeholder.raycastTarget = false;

            TMP_InputField input = go.AddComponent<TMP_InputField>();
            input.textViewport = rt;
            input.textComponent = textArea;
            input.placeholder = placeholder;
            input.targetGraphic = bg;
            input.lineType = TMP_InputField.LineType.SingleLine;
            input.customCaretColor = true;
            input.caretColor = Ember;
            input.selectionColor = EmberGlow;

            // ember focus ring (spec 2.5.2): border swaps to ember while focused.
            input.onSelect.AddListener(_ => { if (brImg != null) brImg.color = Ember; });
            input.onDeselect.AddListener(_ => { if (brImg != null) brImg.color = PanelBorder; });
            if (onChanged != null) input.onValueChanged.AddListener(v => onChanged(v));

            return input;
        }

        /// <summary>Handles for a built scroll grid so callers populate the content + read the ScrollRect.</summary>
        public sealed class ScrollGrid
        {
            public ScrollRect scrollRect;
            public RectTransform content;       // parent the asset cards here (GridLayoutGroup applied)
            public RectTransform viewport;
            public GridLayoutGroup grid;
        }

        /// <summary>
        /// Spec 2.5.3 / 2.5.5: a vertical ScrollRect + RectMask2D viewport + GridLayoutGroup content
        /// + a slim auto-hide rounded scrollbar. Returns the handle; the caller adds cards to .content.
        /// </summary>
        public static ScrollGrid MakeScrollGrid(RectTransform parent, Vector2 cellSize, int columns)
        {
            GameObject srGo = new GameObject("Grid", typeof(RectTransform), typeof(ScrollRect));
            RectTransform srRt = srGo.GetComponent<RectTransform>();
            srRt.SetParent(parent, false);
            Stretch(srRt, Vector2.zero, Vector2.zero);
            ScrollRect sr = srGo.GetComponent<ScrollRect>();
            sr.horizontal = false;
            sr.vertical = true;
            sr.movementType = ScrollRect.MovementType.Clamped;
            sr.scrollSensitivity = 24f;

            GameObject vpGo = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(RectMask2D));
            RectTransform vp = vpGo.GetComponent<RectTransform>();
            vp.SetParent(srRt, false);
            Stretch(vp, Vector2.zero, Vector2.zero);
            Image vpImg = vpGo.GetComponent<Image>();
            vpImg.color = new Color(0f, 0f, 0f, 0f); // transparent; RectMask2D needs a graphic to mask.
            vpImg.raycastTarget = false;

            GameObject contentGo = new GameObject("Content", typeof(RectTransform), typeof(GridLayoutGroup), typeof(ContentSizeFitter));
            RectTransform content = contentGo.GetComponent<RectTransform>();
            content.SetParent(vp, false);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = new Vector2(1f, 1f);
            content.pivot = new Vector2(0.5f, 1f);
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0f, 0f);

            GridLayoutGroup grid = contentGo.GetComponent<GridLayoutGroup>();
            grid.cellSize = cellSize;
            grid.spacing = new Vector2(Space2, Space2);
            grid.padding = new RectOffset((int)Space2, (int)Space2, (int)Space2, (int)Space2);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = Mathf.Max(1, columns);
            grid.childAlignment = TextAnchor.UpperCenter;

            ContentSizeFitter fitter = contentGo.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

            sr.viewport = vp;
            sr.content = content;

            // slim auto-hide rounded scrollbar (spec 2.5.5).
            GameObject sbGo = new GameObject("Scrollbar", typeof(RectTransform), typeof(Image), typeof(Scrollbar));
            RectTransform sbRt = sbGo.GetComponent<RectTransform>();
            sbRt.SetParent(srRt, false);
            sbRt.anchorMin = new Vector2(1f, 0f);
            sbRt.anchorMax = new Vector2(1f, 1f);
            sbRt.pivot = new Vector2(1f, 0.5f);
            sbRt.sizeDelta = new Vector2(5f, 0f);
            sbRt.anchoredPosition = Vector2.zero;
            Image sbBg = sbGo.GetComponent<Image>();
            sbBg.color = new Color(0f, 0f, 0f, 0f); // near-invisible track
            sbBg.raycastTarget = false;

            GameObject handleGo = new GameObject("Handle", typeof(RectTransform), typeof(Image));
            RectTransform handleRt = handleGo.GetComponent<RectTransform>();
            handleRt.SetParent(sbRt, false);
            Stretch(handleRt, Vector2.zero, Vector2.zero);
            Image handleImg = handleGo.GetComponent<Image>();
            handleImg.color = ScrollHandle;
            Round(handleImg, RadiusSm);

            Scrollbar sb = sbGo.GetComponent<Scrollbar>();
            sb.direction = Scrollbar.Direction.BottomToTop;
            sb.handleRect = handleRt;
            sb.targetGraphic = handleImg;
            sr.verticalScrollbar = sb;
            sr.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            sr.verticalScrollbarSpacing = -3f;

            return new ScrollGrid { scrollRect = sr, content = content, viewport = vp, grid = grid };
        }

        /// <summary>Mutable handle for a built asset card (full-card click target, spec 2.5.3).</summary>
        public sealed class AssetCard
        {
            public Button button;
            public Image background;
            public Image border;
            public Image thumbnail;
            public Image checkTick;     // ember corner tick shown when selected
            public TextMeshProUGUI name;
            public ButtonJuice juice;
            public RectTransform root;
        }

        /// <summary>
        /// Spec 2.5.3: an asset card — the whole card is one click target (Fitt's Law). Thumbnail on
        /// top (preserveAspect), name strip below. Reuses an EXISTING sprite as the thumbnail (no new
        /// art); a null sprite falls back to the procedural empty glyph so a missing asset never breaks.
        /// </summary>
        public static AssetCard MakeAssetCard(RectTransform gridContent, Sprite thumb, string name, Action onClick)
        {
            GameObject go = new GameObject("Card_" + name, typeof(RectTransform), typeof(Image), typeof(Button));
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(gridContent, false);
            Image bg = go.GetComponent<Image>();
            bg.color = CardIdle;
            Round(bg, RadiusMd);

            GameObject brGo = new GameObject("Border", typeof(RectTransform), typeof(Image));
            RectTransform br = brGo.GetComponent<RectTransform>();
            br.SetParent(rt, false);
            Stretch(br, Vector2.zero, Vector2.zero);
            Image brImg = brGo.GetComponent<Image>();
            brImg.color = PanelBorder;
            brImg.raycastTarget = false;
            Round(brImg, RadiusMd);

            GameObject fillGo = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            RectTransform fill = fillGo.GetComponent<RectTransform>();
            fill.SetParent(rt, false);
            Stretch(fill, new Vector2(BorderPx, BorderPx), new Vector2(-BorderPx, -BorderPx));
            Image fillImg = fillGo.GetComponent<Image>();
            fillImg.color = CardIdle;
            fillImg.raycastTarget = false;
            Round(fillImg, RadiusMd - 1);

            // thumbnail well (top region) on a slightly darker bg = object-fit contain.
            GameObject wellGo = new GameObject("Well", typeof(RectTransform), typeof(Image));
            RectTransform well = wellGo.GetComponent<RectTransform>();
            well.SetParent(fill, false);
            well.anchorMin = new Vector2(0f, 0f);
            well.anchorMax = new Vector2(1f, 1f);
            well.offsetMin = new Vector2(4f, 22f);  // leave 20px name strip + 2px gap at the bottom
            well.offsetMax = new Vector2(-4f, -4f);
            Image wellImg = wellGo.GetComponent<Image>();
            wellImg.color = PanelBg;
            wellImg.raycastTarget = false;
            Round(wellImg, RadiusSm);

            GameObject thumbGo = new GameObject("Thumb", typeof(RectTransform), typeof(Image));
            RectTransform thumbRt = thumbGo.GetComponent<RectTransform>();
            thumbRt.SetParent(well, false);
            Stretch(thumbRt, new Vector2(4f, 4f), new Vector2(-4f, -4f));
            Image thumbImg = thumbGo.GetComponent<Image>();
            thumbImg.preserveAspect = true;
            thumbImg.raycastTarget = false;
            if (thumb != null)
            {
                thumbImg.sprite = thumb;
                thumbImg.color = Color.white;
            }
            else
            {
                // procedural empty glyph: a muted rounded square stands in for missing art.
                thumbImg.sprite = RoundedSprite(RadiusSm);
                thumbImg.type = Image.Type.Sliced;
                thumbImg.color = DisabledText;
            }

            // name strip (bottom 20px).
            TextMeshProUGUI lbl = new GameObject("Name", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            lbl.rectTransform.SetParent(fill, false);
            lbl.rectTransform.anchorMin = new Vector2(0f, 0f);
            lbl.rectTransform.anchorMax = new Vector2(1f, 0f);
            lbl.rectTransform.pivot = new Vector2(0.5f, 0f);
            lbl.rectTransform.sizeDelta = new Vector2(-6f, 20f);
            lbl.rectTransform.anchoredPosition = new Vector2(0f, 1f);
            lbl.font = Font;
            lbl.text = name;
            lbl.fontSize = 13f;
            lbl.fontStyle = FontStyles.Bold;
            lbl.color = HeaderText;
            lbl.alignment = TextAlignmentOptions.Center;
            lbl.enableWordWrapping = false;
            lbl.overflowMode = TextOverflowModes.Ellipsis;
            lbl.raycastTarget = false;

            // ember check tick (top-right) shown only when selected.
            GameObject tickGo = new GameObject("Tick", typeof(RectTransform), typeof(Image));
            RectTransform tick = tickGo.GetComponent<RectTransform>();
            tick.SetParent(fill, false);
            tick.anchorMin = new Vector2(1f, 1f);
            tick.anchorMax = new Vector2(1f, 1f);
            tick.pivot = new Vector2(1f, 1f);
            tick.sizeDelta = new Vector2(12f, 12f);
            tick.anchoredPosition = new Vector2(-3f, -3f);
            Image tickImg = tickGo.GetComponent<Image>();
            tickImg.color = Ember;
            tickImg.raycastTarget = false;
            Round(tickImg, RadiusSm);
            tickGo.SetActive(false);

            Button button = go.GetComponent<Button>();
            if (onClick != null) button.onClick.AddListener(() => onClick());

            AssetCard card = new AssetCard
            {
                button = button,
                background = fillImg,
                border = brImg,
                thumbnail = thumbImg,
                checkTick = tickImg,
                name = lbl,
                root = rt
            };
            // reuse ButtonJuice via a ButtonStyle shim so the card gets the same hover/press matrix.
            ButtonStyle shim = new ButtonStyle { button = button, background = fillImg, label = lbl, border = brImg, scaleTarget = rt };
            card.juice = AttachJuice(shim, CardIdle, PanelBorder);
            card.juice.SetCardMode(true, card.checkTick); // cards keep their name color bright; tick = selected.
            return card;
        }

        /// <summary>Flip an asset card between idle and selected (ember frame + glow + tick).</summary>
        public static void ApplyCardSelected(AssetCard c, bool selected)
        {
            if (c == null) return;
            if (c.juice != null) c.juice.SetSelected(selected);
            if (c.checkTick != null) c.checkTick.gameObject.SetActive(selected);
        }

        /// <summary>Set a card's disabled look (missing/unavailable asset): dim thumbnail + muted name.</summary>
        public static void ApplyCardDisabled(AssetCard c, bool disabled)
        {
            if (c == null) return;
            if (c.thumbnail != null)
            {
                Color t = c.thumbnail.color;
                c.thumbnail.color = new Color(t.r, t.g, t.b, disabled ? 0.4f : 1f);
            }
            if (c.name != null) c.name.color = disabled ? DisabledText : HeaderText;
            if (c.button != null) c.button.interactable = !disabled;
        }

        /// <summary>
        /// Spec 2.5.4: a centered empty-state block (glyph + line + subtitle) shown when a tab/search
        /// yields zero cards. Returns the root GameObject so the caller toggles it active/inactive.
        /// </summary>
        public static GameObject MakeEmptyState(RectTransform viewport, string msg, string subtitle)
        {
            GameObject root = new GameObject("EmptyState", typeof(RectTransform));
            RectTransform rt = root.GetComponent<RectTransform>();
            rt.SetParent(viewport, false);
            Stretch(rt, Vector2.zero, Vector2.zero);

            GameObject glyphGo = new GameObject("Glyph", typeof(RectTransform), typeof(Image));
            RectTransform glyph = glyphGo.GetComponent<RectTransform>();
            glyph.SetParent(rt, false);
            glyph.anchorMin = new Vector2(0.5f, 0.5f);
            glyph.anchorMax = new Vector2(0.5f, 0.5f);
            glyph.pivot = new Vector2(0.5f, 0.5f);
            glyph.sizeDelta = new Vector2(32f, 32f);
            glyph.anchoredPosition = new Vector2(0f, 22f);
            Image glyphImg = glyphGo.GetComponent<Image>();
            glyphImg.color = DisabledText;
            glyphImg.raycastTarget = false;
            Round(glyphImg, RadiusSm);

            TextMeshProUGUI line = new GameObject("Line", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            line.rectTransform.SetParent(rt, false);
            line.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            line.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            line.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            line.rectTransform.sizeDelta = new Vector2(180f, 18f);
            line.rectTransform.anchoredPosition = new Vector2(0f, -6f);
            line.font = Font;
            line.text = msg;
            line.fontSize = 15f;
            line.fontStyle = FontStyles.Bold;
            line.color = DisabledText;
            line.alignment = TextAlignmentOptions.Center;
            line.raycastTarget = false;

            TextMeshProUGUI sub = new GameObject("Sub", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
            sub.rectTransform.SetParent(rt, false);
            sub.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            sub.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            sub.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            sub.rectTransform.sizeDelta = new Vector2(190f, 28f);
            sub.rectTransform.anchoredPosition = new Vector2(0f, -26f);
            sub.font = Font;
            sub.text = subtitle;
            sub.fontSize = 11f;
            sub.color = DisabledText;
            sub.alignment = TextAlignmentOptions.Center;
            sub.enableWordWrapping = true;
            sub.raycastTarget = false;

            return root;
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

    /// <summary>
    /// Design spec 1.6 — the reusable interaction layer for every Build Mode control. Implements the
    /// full state matrix (idle / hover / pressed / selected / disabled) via pointer events + a short
    /// coroutine color crossfade + scale punch (NO DOTween, per the spec). The SELECTED flag stays the
    /// authority (set by ApplySelected / ApplyCardSelected); this layers hover/press on top and keeps
    /// the ember fill while selected. Pure presentation; no gameplay logic.
    /// </summary>
    internal sealed class ButtonJuice : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Image fill;
        private Image border;
        private TextMeshProUGUI label;
        private RectTransform scaleTarget;

        private Color idleFill;
        private Color idleBorder;

        private bool selected;
        private bool hover;
        private bool pressed;
        private bool cardMode;          // cards keep a bright name + use the check tick for selected.
        private Image checkTick;

        private Coroutine anim;

        public void Bind(BuildModeUiStyle.ButtonStyle style, Color idleFillColor, Color idleBorderColor)
        {
            if (style == null) return;
            fill = style.background;
            border = style.border;
            label = style.label;
            scaleTarget = style.scaleTarget;
            idleFill = idleFillColor;
            idleBorder = idleBorderColor;
            ApplyImmediate();
        }

        public void SetCardMode(bool isCard, Image tick)
        {
            cardMode = isCard;
            checkTick = tick;
            ApplyImmediate();
        }

        public void SetSelected(bool value)
        {
            selected = value;
            Animate();
        }

        public void OnPointerEnter(PointerEventData e) { hover = true; Animate(); }
        public void OnPointerExit(PointerEventData e) { hover = false; pressed = false; Animate(); }
        public void OnPointerDown(PointerEventData e) { pressed = true; Animate(); }
        public void OnPointerUp(PointerEventData e) { pressed = false; Animate(); }

        private void OnDisable()
        {
            // never leave a coroutine dangling across a tool switch / domain reload; snap to target.
            anim = null;
            ApplyImmediate();
        }

        // Resolve the target visual for the current state (the spec 1.6 matrix).
        // Buttons: selected = ember fill + dark text. Cards: selected = ember frame + raised fill,
        // name stays bright (the ember check tick communicates selection on the card).
        private void Targets(out Color fillC, out Color borderC, out Color textC, out float scale)
        {
            scale = 1f;
            if (selected)
            {
                fillC = cardMode ? BuildModeUiStyle.SurfaceHover : BuildModeUiStyle.Ember;
                borderC = BuildModeUiStyle.Ember;
                textC = cardMode ? BuildModeUiStyle.HeaderText : BuildModeUiStyle.SelectedText;
                if (hover) scale = BuildModeUiStyle.HoverScale;
            }
            else if (pressed)
            {
                fillC = BuildModeUiStyle.SurfacePressed;
                borderC = BuildModeUiStyle.BorderHover;
                textC = BuildModeUiStyle.HeaderText;
                scale = BuildModeUiStyle.PressScale;
            }
            else if (hover)
            {
                fillC = BuildModeUiStyle.SurfaceHover;
                borderC = BuildModeUiStyle.BorderHover;
                textC = BuildModeUiStyle.HeaderText;
                scale = BuildModeUiStyle.HoverScale;
            }
            else
            {
                fillC = idleFill;
                borderC = idleBorder;
                textC = cardMode ? BuildModeUiStyle.HeaderText : BuildModeUiStyle.MutedText;
            }
        }

        private void Animate()
        {
            if (checkTick != null) checkTick.gameObject.SetActive(selected);
            if (!isActiveAndEnabled || !Application.isPlaying)
            {
                ApplyImmediate();
                return;
            }
            if (anim != null) StopCoroutine(anim);
            anim = StartCoroutine(Crossfade());
        }

        private IEnumerator Crossfade()
        {
            Targets(out Color fillC, out Color borderC, out Color textC, out float scale);
            Color f0 = fill != null ? fill.color : fillC;
            Color b0 = border != null ? border.color : borderC;
            Color t0 = label != null ? label.color : textC;
            Vector3 s0 = scaleTarget != null ? scaleTarget.localScale : Vector3.one;
            Vector3 s1 = Vector3.one * scale;

            float dur = BuildModeUiStyle.JuiceFade;
            float t = 0f;
            while (t < dur)
            {
                t += Time.unscaledDeltaTime; // Build Mode pauses gameplay (timeScale 0).
                float k = dur > 0f ? Mathf.Clamp01(t / dur) : 1f;
                if (fill != null) fill.color = Color.Lerp(f0, fillC, k);
                if (border != null) border.color = Color.Lerp(b0, borderC, k);
                if (label != null) label.color = Color.Lerp(t0, textC, k);
                if (scaleTarget != null) scaleTarget.localScale = Vector3.Lerp(s0, s1, k);
                yield return null;
            }
            if (fill != null) fill.color = fillC;
            if (border != null) border.color = borderC;
            if (label != null) label.color = textC;
            if (scaleTarget != null) scaleTarget.localScale = s1;
            anim = null;
        }

        private void ApplyImmediate()
        {
            Targets(out Color fillC, out Color borderC, out Color textC, out float scale);
            if (fill != null) fill.color = fillC;
            if (border != null) border.color = borderC;
            if (label != null) label.color = textC;
            if (scaleTarget != null) scaleTarget.localScale = Vector3.one * scale;
            if (checkTick != null) checkTick.gameObject.SetActive(selected);
        }
    }
}
#endif
