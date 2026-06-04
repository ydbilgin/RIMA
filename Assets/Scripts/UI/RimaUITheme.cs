using UnityEngine;

namespace RIMA
{
    public static class RimaUITheme
    {
        // ── Asset Paths ───────────────────────────────────────────────────
        public const string ResourceFramePath        = "UI/RIMA/RIMA_UI_ResourceFrame";
        public const string SkillSlotFramePath       = "UI/RIMA/RIMA_UI_SkillSlotFrame";
        public const string MiniMapFramePath         = "UI/RIMA/RIMA_UI_MiniMapFrame";
        public const string RoomBannerFramePath      = "UI/RIMA/RIMA_UI_RoomBannerFrame";
        public const string SmallPanelFramePath      = "UI/RIMA/RIMA_UI_SmallPanelFrame";
        public const string MenuDungeonBackgroundPath = "UI/RIMA/RIMA_MenuDungeonBackground";
        public const string DraftCardFramePath       = "UI/RIMA/Pack/card_frame_9slice";
        public const string CardSelectFlashPath      = "UI/RIMA/card_select_flash";
        public const string SkillSlotFrameFallbackPath = "UI/RIMA/icon_frame_hex";
        public const string SkillBarBackingPath      = "UI/RIMA/Pack/bar_frame_9slice";
        public const string SkillBarBackingFallbackPath = "UI/RIMA/Pack/pedestal_seal";

        public static string RarityGlowPath(SkillTier tier) => tier switch
        {
            SkillTier.Rare      => "UI/RIMA/rarity_glow_rare",
            SkillTier.Epic      => "UI/RIMA/rarity_glow_epic",
            SkillTier.Legendary => "UI/RIMA/rarity_glow_legendary",
            _                   => "UI/RIMA/rarity_glow_common",
        };

        // ── Base Colors ───────────────────────────────────────────────────
        public static readonly Color PanelTint    = new Color(0.06f, 0.07f, 0.10f, 0.88f);
        public static readonly Color PanelBorder  = new Color(0.20f, 0.28f, 0.38f, 0.70f);
        public static readonly Color TextPrimary  = new Color(0.86f, 0.92f, 0.94f, 1f);
        public static readonly Color TextMuted    = new Color(0.54f, 0.66f, 0.70f, 1f);
        public static readonly Color Cyan         = new Color(0.28f, 0.88f, 1f,   1f);
        public static readonly Color Gold         = new Color(0.95f, 0.74f, 0.24f, 1f);
        public static readonly Color DangerRed    = new Color(0.90f, 0.18f, 0.12f, 1f);

        // ── HP Bar States ─────────────────────────────────────────────────
        public static readonly Color HpHealthy    = new Color(0.29f, 0.62f, 0.75f, 1f); // #4A9EBF  >60%
        public static readonly Color HpWarning    = new Color(0.78f, 0.45f, 0.16f, 1f); // #C8742A  30-60%
        public static readonly Color HpCritical   = new Color(0.77f, 0.17f, 0.17f, 1f); // #C42B2B  <30%
        public static readonly Color HpEmpty      = new Color(0.10f, 0.10f, 0.10f, 0.60f); // #1A1A1A

        // ── Resource Bar ──────────────────────────────────────────────────
        public static readonly Color RageDefault   = new Color(0.48f, 0.25f, 0.63f, 1f); // #7B3FA0
        public static readonly Color RageMax       = new Color(0.72f, 0.42f, 1.00f, 1f); // #B86AFF

        // ── Minimap ───────────────────────────────────────────────────────
        public static readonly Color MapActive     = new Color(0.43f, 0.78f, 1.00f, 1f); // #6EC6FF
        public static readonly Color MapVisited    = new Color(0.29f, 0.25f, 0.21f, 1f); // #4A4035
        public static readonly Color MapBoss       = new Color(0.55f, 0.13f, 0.13f, 1f); // #8B2020
        public static readonly Color MapTreasure   = new Color(0.72f, 0.53f, 0.04f, 1f); // #B8860B
        public static readonly Color MapUnvisited  = new Color(0.10f, 0.08f, 0.06f, 1f); // #1A1510
        public static readonly Color MapFrame      = new Color(0.23f, 0.19f, 0.16f, 1f); // #3A3028

        // ── Overlays ──────────────────────────────────────────────────────
        public static readonly Color OverlayDark     = new Color(0.05f, 0.05f, 0.07f, 0.85f); // #0D0D12
        public static readonly Color BackgroundDark  = new Color(0.05f, 0.05f, 0.06f, 1f);    // #0D0D0F

        // ── Skill Offer Tiers ─────────────────────────────────────────────
        public static readonly Color TierCommon    = new Color(0.50f, 0.55f, 0.55f, 1f);
        public static readonly Color TierRare      = new Color(0.20f, 0.60f, 0.60f, 1f);
        public static readonly Color TierEpic      = new Color(0.50f, 0.20f, 0.60f, 1f);
        public static readonly Color TierLegendary = new Color(0.80f, 0.65f, 0.15f, 1f);

        // ── Hex Slot ──────────────────────────────────────────────────────
        public static readonly Color SlotHexBg     = new Color(0.05f, 0.05f, 0.07f, 0.55f); // #0D0D12

        // ── Slot Visuals ──────────────────────────────────────────────────
        public static readonly Color SlotBg          = new Color(0.07f, 0.08f, 0.12f, 0.92f);
        public static readonly Color SlotBorderReady = new Color(0.28f, 0.82f, 1f,   0.75f);
        public static readonly Color SlotBorderEmpty = new Color(0.20f, 0.26f, 0.34f, 0.50f);
        public static readonly Color SlotCDOverlay   = new Color(0.03f, 0.04f, 0.10f, 0.78f);
        public static readonly Color SlotLocked      = new Color(0.08f, 0.09f, 0.13f, 0.82f);
        public static readonly Color SlotDragHL      = new Color(0.40f, 0.72f, 1f,   0.35f);

        // ── Class Accent Colors (primary) ─────────────────────────────────
        // Each class has: accent (border/glow), resource fill, resource track
        public static Color ClassAccent(ClassType c) => c switch
        {
            ClassType.Warblade     => new Color(0.95f, 0.38f, 0.08f, 1f), // deep orange
            ClassType.Elementalist => new Color(0.28f, 0.68f, 1f,   1f), // arcane blue
            ClassType.Shadowblade  => new Color(0.56f, 0.18f, 0.90f, 1f), // violet
            ClassType.Ranger       => new Color(0.32f, 0.82f, 0.42f, 1f), // forest green
            ClassType.Ravager      => new Color(0.88f, 0.22f, 0.22f, 1f), // blood red
            ClassType.Ronin        => new Color(0.42f, 0.95f, 1f,   1f), // cyan-violet
            ClassType.Gunslinger   => new Color(0.98f, 0.72f, 0.18f, 1f), // brass gold
            ClassType.Brawler      => new Color(0.78f, 0.46f, 0.18f, 1f), // amber
            ClassType.Summoner     => new Color(0.42f, 0.88f, 0.62f, 1f), // jade
            ClassType.Hexer        => new Color(0.72f, 0.28f, 0.88f, 1f), // deep purple
            _                      => Cyan,
        };

        // Resource bar fill color per class
        public static Color ResourceFill(ClassType c) => c switch
        {
            ClassType.Warblade     => new Color(0.95f, 0.38f, 0.08f, 0.96f),
            ClassType.Elementalist => new Color(0.22f, 0.55f, 1f,   0.96f),
            ClassType.Shadowblade  => new Color(0.48f, 0.14f, 0.82f, 0.96f),
            ClassType.Ranger       => new Color(0.28f, 0.78f, 0.38f, 0.96f),
            ClassType.Ravager      => new Color(0.82f, 0.18f, 0.18f, 0.96f),
            ClassType.Ronin        => new Color(0.48f, 0.36f, 0.96f, 0.96f),
            ClassType.Gunslinger   => new Color(0.92f, 0.68f, 0.14f, 0.96f),
            ClassType.Brawler      => new Color(0.72f, 0.42f, 0.14f, 0.96f),
            ClassType.Summoner     => new Color(0.38f, 0.82f, 0.56f, 0.96f),
            ClassType.Hexer        => new Color(0.66f, 0.22f, 0.82f, 0.96f),
            _                      => new Color(0.95f, 0.38f, 0.08f, 0.96f),
        };

        // Resource name per class
        public static string ResourceName(ClassType c) => c switch
        {
            ClassType.Elementalist => "MANA",
            ClassType.Shadowblade  => "ENERGY",
            ClassType.Ranger       => "FOCUS",
            ClassType.Ronin        => "TENSION",
            ClassType.Gunslinger   => "HEAT",
            ClassType.Summoner     => "RIFT",
            ClassType.Hexer        => "CURSE",
            _                      => "RAGE",
        };

        // Character idle sprite path under Resources/Characters/<Class>/ (new canonical PixelLab set, 2026-05-31).
        // Previously pointed to Characters/Anchors/<class>_anchor which never existed under Resources -> white-box bug.
        public static string AnchorPath(ClassType c) => c switch
        {
            ClassType.Warblade     => "Characters/Warblade/warblade_idle_south",
            ClassType.Elementalist => "Characters/Elementalist/elementalist_idle_south",
            ClassType.Shadowblade  => "Characters/Shadowblade/shadowblade_idle_south",
            ClassType.Ranger       => "Characters/Ranger/ranger_idle_south",
            ClassType.Ravager      => "Characters/Ravager/ravager_idle_south",
            ClassType.Ronin        => "Characters/Ronin/ronin_idle_south",
            ClassType.Gunslinger   => "Characters/Gunslinger/gunslinger_idle_south",
            ClassType.Brawler      => "Characters/Brawler/brawler_idle_south",
            ClassType.Summoner     => "Characters/Summoner/summoner_idle_south",
            ClassType.Hexer        => "Characters/Hexer/hexer_idle_south",
            _                      => "Characters/Warblade/warblade_idle_south",
        };

        // Class description keywords for CharacterSelect
        public static (string line1, string line2) ClassTagline(ClassType c) => c switch
        {
            ClassType.Warblade     => ("HEAVY · MELEE · RAGE", "3-step iron combo. Anger fuels destruction."),
            ClassType.Elementalist => ("CASTER · RHYTHM · ELEMENTS", "Fire. Frost. Lightning. Empowered 3rd bolt."),
            ClassType.Shadowblade  => ("AGILE · STEALTH · EXECUTE", "Veil Strike into shadow. Delete and vanish."),
            ClassType.Ranger       => ("RANGED · PRECISION · TRAPS", "Tap shot or hold to aim. Traps set the kill zone."),
            ClassType.Ravager      => ("HEAVY · BLEED · FRENZY", "Stacks wounds. Gets faster as enemies fall."),
            ClassType.Ronin        => ("FAST · PARRY · IAIJUTSU", "Precise counter-window. One clean draw, one kill."),
            ClassType.Gunslinger   => ("RANGED · HEAT · CHAMBERS", "6-chamber revolver. Crits on 6th and 12th shot."),
            ClassType.Brawler      => ("MELEE · ARMOR · MOMENTUM", "Absorb hits, convert to punch power. Never stop."),
            ClassType.Summoner     => ("COMMAND · RIFT · MINIONS", "Rift fragments become soldiers. Lead the swarm."),
            ClassType.Hexer        => ("CURSE · SPREAD · DETONATE", "Mark. Stack. Explode. Chain reaction specialist."),
            _                      => ("UNKNOWN", ""),
        };

        // ── Sprites ───────────────────────────────────────────────────────
        // Procedural rounded-rect sprites instead of AI-generated PNGs.
        // Clean, resolution-independent, 9-slice compatible.

        private static Sprite _panelSprite;
        private static Sprite _frameSprite;
        private static Sprite _promptSprite;
        private static Sprite _skillSlotSprite;

        /// <summary>Dark glass panel (main panels, reward cards, char sheet).</summary>
        public static Sprite SmallPanelFrame => _panelSprite ??= MakeRoundedRect(
            64, 64, 10, new Color(0.06f, 0.07f, 0.10f, 0.92f),
            new Color(0.18f, 0.24f, 0.32f, 0.65f), 2);

        /// <summary>Resource bar / room banner frame.</summary>
        public static Sprite ResourceFrame => _frameSprite ??= MakeRoundedRect(
            64, 20, 6, new Color(0.05f, 0.06f, 0.09f, 0.88f),
            new Color(0.20f, 0.28f, 0.38f, 0.55f), 1);

        public static Sprite RoomBannerFrame => ResourceFrame;

        /// <summary>MiniMap background frame.</summary>
        public static Sprite MiniMapFrame => _promptSprite ??= MakeRoundedRect(
            48, 48, 8, new Color(0.04f, 0.05f, 0.08f, 0.90f),
            new Color(0.22f, 0.34f, 0.42f, 0.60f), 1);

        /// <summary>Skill slot frame (individual slots).</summary>
        public static Sprite SkillSlotFrame => _skillSlotSprite ??= MakeRoundedRect(
            64, 64, 10, new Color(0.06f, 0.07f, 0.10f, 0.92f),
            new Color(0.18f, 0.24f, 0.32f, 0.65f), 2);

        /// <summary>Full-screen menu background — still loads the PNG if present.</summary>
        public static Sprite MenuDungeonBackground => RimaGeneratedSpriteCache.Load(MenuDungeonBackgroundPath);

        public static Sprite DraftCardFrame => Resources.Load<Sprite>(DraftCardFramePath);
        public static Sprite CardSelectFlash => Resources.Load<Sprite>(CardSelectFlashPath);
        public static Sprite RarityGlow(SkillTier tier) => Resources.Load<Sprite>(RarityGlowPath(tier));
        public static Sprite SkillSlotFrameAsset =>
            Resources.Load<Sprite>(SkillSlotFramePath) ?? Resources.Load<Sprite>(SkillSlotFrameFallbackPath);
        public static Sprite SkillBarBacking =>
            Resources.Load<Sprite>(SkillBarBackingPath) ?? Resources.Load<Sprite>(SkillBarBackingFallbackPath);
        public static bool SkillBarBackingIsSliced => Resources.Load<Sprite>(SkillBarBackingPath) != null;

        // ── Passive draft-card icons (16-cell 4x4 sheet, runtime-sliced) ──
        // Active skills carry their own icon; passives/relics have none, so we
        // give each a stable on-brand icon from this generated pack by name hash.
        public const string PassiveIconSheetPath = "UI/RIMA/PassiveIcons/passive_icons_sheet_4x4_96";
        private const int PassiveIconCols = 4, PassiveIconRows = 4, PassiveIconCell = 96;
        private static Sprite[] _passiveIcons;

        public static Sprite PassiveIcon(string key)
        {
            EnsurePassiveIcons();
            if (_passiveIcons == null || _passiveIcons.Length == 0) return null;
            int h = 0;
            if (!string.IsNullOrEmpty(key)) foreach (char ch in key) h = h * 31 + ch;
            return _passiveIcons[(h & 0x7fffffff) % _passiveIcons.Length];
        }

        private static void EnsurePassiveIcons()
        {
            if (_passiveIcons != null) return;
            Sprite sheet = Resources.Load<Sprite>(PassiveIconSheetPath);
            Texture2D tex = sheet != null ? sheet.texture : Resources.Load<Texture2D>(PassiveIconSheetPath);
            if (tex == null) { _passiveIcons = System.Array.Empty<Sprite>(); return; }
            var list = new System.Collections.Generic.List<Sprite>(PassiveIconCols * PassiveIconRows);
            for (int r = 0; r < PassiveIconRows; r++)
                for (int c = 0; c < PassiveIconCols; c++)
                {
                    // Reading order (top-left first); texture origin is bottom-left so flip the row.
                    var rect = new Rect(c * PassiveIconCell, (PassiveIconRows - 1 - r) * PassiveIconCell, PassiveIconCell, PassiveIconCell);
                    list.Add(Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f), 96f));
                }
            _passiveIcons = list.ToArray();
        }

        // ── Full-screen backdrop (cover/crop, never distort) ──────────────
        /// <summary>
        /// Create a full-bleed backdrop Image under <paramref name="parent"/>, loaded from
        /// Resources/<paramref name="resourcePath"/>. Uses an AspectRatioFitter (EnvelopeParent) so a
        /// square painting COVERS a 16:9 screen by cropping overflow — never stretch-distorted.
        /// Inserted as the first sibling (drawn behind sibling content); non-raycast. Because the
        /// parent's own graphic draws before its children, an opaque backdrop child cleanly hides the
        /// parent's flat fill. Falls back to a flat <paramref name="fallbackTint"/> fill if the sprite
        /// is missing, so a not-yet-imported backdrop keeps the prior flat-color look.
        /// </summary>
        public static UnityEngine.UI.Image CreateFullScreenBackdrop(Transform parent, string resourcePath, Color fallbackTint)
        {
            var go = new GameObject("Backdrop", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            go.transform.SetAsFirstSibling();
            var rt = go.GetComponent<RectTransform>();
            var img = go.AddComponent<UnityEngine.UI.Image>();
            img.raycastTarget = false;

            var spr = RimaGeneratedSpriteCache.Load(resourcePath);
            if (spr != null)
            {
                // Center-anchored; the AspectRatioFitter drives size to cover the parent rect.
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = Vector2.zero;
                img.sprite = spr;
                img.type   = UnityEngine.UI.Image.Type.Simple;
                img.color  = Color.white;
                var fitter = go.AddComponent<UnityEngine.UI.AspectRatioFitter>();
                fitter.aspectMode   = UnityEngine.UI.AspectRatioFitter.AspectMode.EnvelopeParent;
                fitter.aspectRatio  = spr.rect.width / Mathf.Max(1f, spr.rect.height);
            }
            else
            {
                rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
                rt.offsetMin = rt.offsetMax = Vector2.zero;
                img.color = fallbackTint;
            }
            return img;
        }

        public static Sprite NodeIcon(RoomType type)
        {
            string path = type switch
            {
                RoomType.Elite    => "UI/RIMA/RIMA_UI_Node_Elite",
                RoomType.Boss     => "UI/RIMA/RIMA_UI_Node_Boss",
                RoomType.Chest    => "UI/RIMA/RIMA_UI_Node_Chest",
                RoomType.Merchant => "UI/RIMA/RIMA_UI_Node_Event",
                RoomType.Event    => "UI/RIMA/RIMA_UI_Node_Event",
                RoomType.Forge    => "UI/RIMA/RIMA_UI_Node_Forge",
                RoomType.Curse    => "UI/RIMA/RIMA_UI_Node_Curse",
                _                 => "UI/RIMA/RIMA_UI_Node_Combat",
            };
            return RimaGeneratedSpriteCache.Load(path);
        }

        // ── Procedural Sprite Generator ──────────────────────────────────
        private static Sprite MakeRoundedRect(int w, int h, int radius,
            Color fill, Color border, int borderWidth)
        {
            try
            {
                var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
                if (tex == null) return null;

                tex.filterMode = FilterMode.Bilinear;
                tex.wrapMode = TextureWrapMode.Clamp;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        float dist = RoundedRectSDF(x, y, w, h, radius);
                        if (dist > 0.5f)
                        {
                            tex.SetPixel(x, y, Color.clear);
                        }
                        else if (dist > -borderWidth)
                        {
                            float t = Mathf.Clamp01((dist + borderWidth) / 1.2f);
                            Color c = Color.Lerp(border, fill, t);
                            // Anti-alias the outer edge
                            if (dist > -0.5f) c.a *= Mathf.Clamp01(0.5f - dist);
                            tex.SetPixel(x, y, c);
                        }
                        else
                        {
                            tex.SetPixel(x, y, fill);
                        }
                    }
                }

                tex.Apply(false, true); // makeNoLongerReadable for memory

                int brd = radius; // 9-slice border
                return Sprite.Create(tex, new Rect(0, 0, w, h),
                    new Vector2(0.5f, 0.5f), 100f, 0,
                    SpriteMeshType.FullRect,
                    new Vector4(brd, brd, brd, brd)); // left, bottom, right, top
            }
            catch (System.Exception ex)
            {
                // EditMode test context may not have graphics pipeline ready.
                // Image components tolerate null sprite (renders solid color).
                Debug.LogWarning($"[RimaUITheme] MakeRoundedRect failed (EditMode?): {ex.Message}");
                return null;
            }
        }

        private static float RoundedRectSDF(int px, int py, int w, int h, int r)
        {
            // Signed distance from pixel to rounded rect edge (negative = inside)
            float cx = Mathf.Max(Mathf.Abs(px - w * 0.5f) - (w * 0.5f - r), 0f);
            float cy = Mathf.Max(Mathf.Abs(py - h * 0.5f) - (h * 0.5f - r), 0f);
            return Mathf.Sqrt(cx * cx + cy * cy) - r;
        }
    }
}
