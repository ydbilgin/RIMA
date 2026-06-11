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

        // Character Select v3.2 palette: 60-30-10, neutral-first.
        public static readonly Color CharSelectVoidBlack    = new Color(0.059f, 0.051f, 0.082f, 1f);  // #0F0D15
        public static readonly Color CharSelectPanelFill    = new Color(0.051f, 0.051f, 0.067f, 0.85f); // #0D0D11 @ 85%
        public static readonly Color CharSelectIronGrey     = new Color(0.184f, 0.188f, 0.216f, 1f);  // #2F3037
        public static readonly Color CharSelectTextBody     = new Color(0.690f, 0.702f, 0.737f, 1f);  // #B0B3BC
        public static readonly Color CharSelectParchment    = new Color(0.918f, 0.918f, 0.918f, 1f);  // #EAEAEA
        public static readonly Color CharSelectDivider      = new Color(0.286f, 0.231f, 0.369f, 1f);  // #493B5E
        public static readonly Color CharSelectCyan         = new Color(0.000f, 1.000f, 0.800f, 1f);  // #00FFCC
        public static readonly Color CharSelectOrange       = new Color(0.910f, 0.565f, 0.125f, 1f);  // #E89020
        public static readonly Color CharSelectButtonFill   = new Color(0.082f, 0.086f, 0.110f, 1f);  // #15161C
        public static readonly Color CharSelectStatFill     = new Color(0.784f, 0.804f, 0.847f, 1f);  // #C8CDD8
        public static readonly Color CharSelectStatEmpty    = new Color(0.102f, 0.106f, 0.133f, 1f);  // #1A1B22
        public static readonly Color CharSelectLockedText   = new Color(0.420f, 0.380f, 0.480f, 1f);

        // Act 1 UI canon: cold slate foundation, void-purple overlay, ember accents.
        public static readonly Color Act1Slate       = new Color(0.227f, 0.239f, 0.259f, 1f); // #3A3D42
        public static readonly Color Act1VoidPurple  = new Color(0.227f, 0.102f, 0.290f, 1f); // #3A1A4A
        public static readonly Color Act1Ember       = new Color(0.910f, 0.565f, 0.125f, 1f); // #E89020
        public static readonly Color Act1PanelFill   = new Color(0.035f, 0.036f, 0.047f, 0.92f);
        public static readonly Color Act1Overlay     = new Color(0.075f, 0.035f, 0.095f, 0.88f);
        public static readonly Color Act1ButtonFill  = new Color(0.105f, 0.090f, 0.125f, 0.94f);

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

        public static (string motto, string playstyle, string resource) ClassIdentity(ClassType cls) => cls switch
        {
            ClassType.Warblade => (
                "Yaklaş. Sabitle. Zırh kır. İnfaz et.",
                "Ağır zırh-kırıcı iki el kılıcı ve parry ile ritmi dikte eder.",
                "Rage 0-100 · sadece vurarak/CC ile dolar; Sundered sonrası infaz."),
            ClassType.Ranger => (
                "Sana ulaşamazlar. Her saniye kayıp veriyorsun.",
                "Mesafe disiplinini, tuzakları ve Mark baskısını aynı hatta tutar.",
                "Focus · 4m+ mesafede dolar, yakında erir."),
            ClassType.Shadowblade => (
                "Görmeden önce hissedilir.",
                "Hızlı pozisyonel oyun, Phase pencereleri ve toplu infaz üstüne kurulur.",
                "Energy + Combo · Rift Scar/Sever karar anı."),
            ClassType.Elementalist => (
                "Her şeyi yakıyorum. Ama önce ritmi buluyorum.",
                "Fire/Frost ritmini kurar, doğru döngüde Lightbreak ile alanı kırar.",
                "Mana + Elemental State · element ritmi güç biriktirir."),
            ClassType.Ravager => (
                "Az canken daha tehlikeliyim.",
                "Düşük HP momentumuyla çift balta kan-furyasını büyütür.",
                "Fury · sadece hasar alarak dolar; düşük HP daha hızlı."),
            ClassType.Ronin => (
                "Çek. Kes. Kın. Bir nefeste.",
                "Iaido bekle-cezalandır temposu, hareketsizlik ve tek temiz kesiş ister.",
                "Tension · kın ve hareketsiz kalınca birikir."),
            ClassType.Gunslinger => (
                "Mermin yok. Senin zamanın da yok.",
                "Kinetik run-and-gun oynar; ısıyı yönetip kusursuz soğutma arar.",
                "Heat · mükemmel Sıfır soğutma ile tempo sıfırlanır."),
            ClassType.Brawler => (
                "Düşersen kalk. Ama önce yumruğum kalkar.",
                "Silahsız ritmik kombo, yakın baskı ve whiff-punish üstüne kurulur.",
                "Charge 0-5 · Shattered pencereleri yumruk gücüne döner."),
            ClassType.Summoner => (
                "Ben savaşmıyorum. Feda ediyorum.",
                "Minyon kondüktörü gibi oynar; çağırır, konumlandırır, feda-burst açar.",
                "Charges 0-4 · çağır ve feda et."),
            ClassType.Hexer => (
                "Sabır. 10'a gelince sen bitiyorsun.",
                "Lanet biriktirir, yayar ve doğru karar anında patlatır.",
                "Hex Stacks 0-10 · Hexblast ile karar anı."),
            _ => ("UNKNOWN", "", ""),
        };

        public static (int damage, int durability, int speed, int control, int difficulty) ClassStats(ClassType cls) => cls switch
        {
            ClassType.Warblade     => (8, 8, 4, 6, 3),
            ClassType.Elementalist => (7, 3, 5, 9, 7),
            ClassType.Ranger       => (6, 4, 8, 8, 4),
            ClassType.Shadowblade  => (8, 3, 9, 6, 8),
            ClassType.Ronin        => (8, 5, 7, 6, 7),
            ClassType.Ravager      => (9, 9, 3, 5, 6),
            ClassType.Gunslinger   => (8, 4, 7, 7, 7),
            ClassType.Brawler      => (7, 8, 6, 5, 5),
            ClassType.Summoner     => (5, 4, 4, 9, 8),
            ClassType.Hexer        => (6, 3, 5, 10, 9),
            _                      => (0, 0, 0, 0, 0),
        };

        // ── Sprites ───────────────────────────────────────────────────────
        // Procedural rounded-rect sprites instead of AI-generated PNGs.
        // Clean, resolution-independent, 9-slice compatible.

        private static Sprite _panelSprite;
        private static Sprite _frameSprite;
        private static Sprite _promptSprite;
        private static Sprite _skillSlotSprite;
        private static Sprite _act1PanelSprite;
        private static Sprite _act1ButtonSprite;
        private static Sprite _act1IconWellSprite;
        private static Sprite _act1RuleSprite;

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

        public static Sprite Act1PanelFrame => _act1PanelSprite ??= MakeRoundedRect(
            96, 96, 8,
            new Color(Act1PanelFill.r, Act1PanelFill.g, Act1PanelFill.b, 0.94f),
            new Color(Act1Ember.r, Act1Ember.g, Act1Ember.b, 0.55f), 2);

        public static Sprite Act1ButtonFrame => _act1ButtonSprite ??= MakeRoundedRect(
            96, 32, 5,
            Act1ButtonFill,
            new Color(Act1Ember.r, Act1Ember.g, Act1Ember.b, 0.72f), 2);

        public static Sprite Act1IconWell => _act1IconWellSprite ??= MakeRoundedRect(
            48, 48, 6,
            new Color(0.065f, 0.055f, 0.080f, 0.96f),
            new Color(Act1Ember.r, Act1Ember.g, Act1Ember.b, 0.62f), 2);

        public static Sprite Act1TitleRule => _act1RuleSprite ??= MakeRoundedRect(
            96, 8, 3,
            new Color(Act1Ember.r, Act1Ember.g, Act1Ember.b, 0.78f),
            new Color(Act1Slate.r, Act1Slate.g, Act1Slate.b, 0.55f), 1);

        // ── Procedural Skill Bar sprites (slot + bar backing, no PNG asset needed) ──

        private static Sprite _proceduralSlotBg;
        private static Sprite _proceduralSlotRim;
        private static Sprite _proceduralBarBacking;

        /// <summary>
        /// Slot background layer: #07090F @ 88%, rounded radius 10.
        /// Used as the filled bg of each skill slot (replaces stone-frame PNG).
        /// </summary>
        public static Sprite ProceduralSlotBg => _proceduralSlotBg ??= MakeRoundedRect(
            64, 64, 10,
            new Color(0.028f, 0.035f, 0.059f, 0.88f),   // #07090F @88%
            new Color(0.104f, 0.141f, 0.314f, 0.50f),   // #1A2450 @50% — subtle cold rim
            2);

        /// <summary>
        /// Slot rim / border layer: 1.5 px clear fill, border-only sprite used for the colored rim.
        /// Ready=class-accent @65%, Cooldown=#2A3850 @50%, Empty=#1C2535 @35%.
        /// </summary>
        public static Sprite ProceduralSlotRim => _proceduralSlotRim ??= MakeRoundedRectRimOnly(
            64, 64, 10, 2);

        /// <summary>
        /// Bar backing panel: #040609 @ 12%, 1 px border #1A2540 @ 30%, radius 8.
        /// Replaces bar_frame_9slice PNG for the horizontal backing behind all slots.
        /// </summary>
        public static Sprite ProceduralBarBacking => _proceduralBarBacking ??= MakeRoundedRect(
            128, 80, 8,
            new Color(0.016f, 0.024f, 0.035f, 0.14f),  // #040609 @14% (slightly more visible)
            new Color(0.102f, 0.145f, 0.251f, 0.30f),  // #1A2540 @30%
            1);

        /// <summary>Loads rarity_glow_common.png from Resources (used as glow sprite tinted to class accent).</summary>
        public static Sprite RarityGlowCommon => Resources.Load<Sprite>("UI/RIMA/rarity_glow_common");

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

        private static Sprite _proceduralFootRing;

        /// <summary>Procedural ellipse foot ring for character selection.</summary>
        public static Sprite ProceduralFootRing => _proceduralFootRing ??= MakeEllipseRing(160, 100, 64f, 6.0f, 8.0f);

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

                tex.Apply(true, false); // keep readable so Sprite stays valid

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

        /// <summary>
        /// Creates a rounded rect sprite that is transparent in the fill area —
        /// only the border ring (borderWidth pixels) is opaque white.
        /// Tint this sprite at runtime to get the colored rim without affecting the slot bg.
        /// </summary>
        private static Sprite MakeRoundedRectRimOnly(int w, int h, int radius, int borderWidth)
        {
            try
            {
                var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
                tex.filterMode = FilterMode.Bilinear;
                tex.wrapMode   = TextureWrapMode.Clamp;

                for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    float dist = RoundedRectSDF(x, y, w, h, radius);
                    Color c;
                    if (dist > 0.5f)
                    {
                        c = Color.clear;
                    }
                    else if (dist > -borderWidth)
                    {
                        // border band — anti-alias outer edge
                        float aa = dist > -0.5f ? Mathf.Clamp01(0.5f - dist) : 1f;
                        c = new Color(1f, 1f, 1f, aa);
                    }
                    else
                    {
                        c = Color.clear; // interior is transparent
                    }
                    tex.SetPixel(x, y, c);
                }

                tex.Apply(true, false); // keep readable
                int brd = radius;
                return Sprite.Create(tex, new Rect(0, 0, w, h),
                    new Vector2(0.5f, 0.5f), 100f, 0,
                    SpriteMeshType.FullRect,
                    new Vector4(brd, brd, brd, brd));
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[RimaUITheme] MakeRoundedRectRimOnly failed: {ex.Message}");
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

        private static Sprite MakeEllipseRing(int w, int h, float ringRadius, float thickness, float glowWidth)
        {
            try
            {
                var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
                if (tex == null) return null;

                tex.filterMode = FilterMode.Bilinear;
                tex.wrapMode = TextureWrapMode.Clamp;

                float cx = (w - 1) / 2f;
                float cy = (h - 1) / 2f;
                float aspect = 1f / 0.61f; // ~1.6393
                float halfThick = thickness * 0.5f;

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        float dx = x - cx;
                        float dy = (y - cy) * aspect;
                        float dist = Mathf.Sqrt(dx * dx + dy * dy);

                        float alpha = 0f;
                        if (dist >= ringRadius - halfThick && dist <= ringRadius + halfThick)
                        {
                            alpha = 1f;
                        }
                        else if (dist < ringRadius - halfThick)
                        {
                            // Inner anti-aliasing edge (over 1.5 pixels)
                            float innerEdgeStart = ringRadius - halfThick - 1.5f;
                            if (dist > innerEdgeStart)
                            {
                                alpha = Mathf.Clamp01((dist - innerEdgeStart) / 1.5f);
                            }
                        }
                        else
                        {
                            // Outer glow falloff (over glowWidth pixels)
                            float outerEdgeStart = ringRadius + halfThick;
                            float outerEdgeEnd = outerEdgeStart + glowWidth;
                            if (dist < outerEdgeEnd)
                            {
                                float t = Mathf.Clamp01((dist - outerEdgeStart) / glowWidth);
                                alpha = (1f - t) * (1f - t); // quadratic falloff for soft glow
                            }
                        }

                        if (alpha > 0f)
                        {
                            tex.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                        }
                        else
                        {
                            tex.SetPixel(x, y, Color.clear);
                        }
                    }
                }

                tex.Apply(true, false); // keep readable

                return Sprite.Create(tex, new Rect(0f, 0f, w, h),
                    new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[RimaUITheme] MakeEllipseRing failed (EditMode?): {ex.Message}");
                return null;
            }
        }
    }
}
