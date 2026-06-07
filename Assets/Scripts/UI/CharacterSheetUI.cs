using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// TAB overlay — build overview panel.
    /// Opened/closed exclusively by UIManager.
    /// Full-screen dark overlay with class identity, active kit, expanded minimap, passives.
    /// </summary>
    public class CharacterSheetUI : MonoBehaviour
    {
        public static CharacterSheetUI Instance { get; private set; }

        private CanvasGroup canvasGroup;
        private bool isVisible;

        // ── UI elements ──────────────────────────────────────────────
        private GameObject panel;
        private TextMeshProUGUI classNameLabel;
        private TextMeshProUGUI classTaglineLabel;
        private readonly List<TextMeshProUGUI> kitLabels = new List<TextMeshProUGUI>();
        private readonly List<Image> synergyChips = new List<Image>();
        private readonly List<TextMeshProUGUI> passiveLabels = new List<TextMeshProUGUI>();
        private Image expandedMapBg;

        // ── Layout constants ─────────────────────────────────────────
        private const float ExpandedMapSize = 200f;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

            BuildOverlay();
            Hide();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ─── Public API (called by UIManager) ───────────────────────

        public void Show()
        {
            isVisible = true;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            RefreshContent();
        }

        public void Hide()
        {
            isVisible = false;
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        // ─── Build ──────────────────────────────────────────────────

        private void BuildOverlay()
        {
            var root = GetComponent<RectTransform>();
            // Full screen
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.offsetMin = root.offsetMax = Vector2.zero;

            // Dark overlay bg
            var bgImg = gameObject.GetComponent<Image>();
            if (bgImg == null) bgImg = gameObject.AddComponent<Image>();
            bgImg.color = RimaUITheme.OverlayDark;
            bgImg.raycastTarget = true;

            // ── Left panel: class identity + kit ─────────────────────
            var leftPanel = MakeRect("LeftPanel", root);
            leftPanel.anchorMin = new Vector2(0f, 0f);
            leftPanel.anchorMax = new Vector2(0.45f, 1f);
            leftPanel.offsetMin = new Vector2(40f, 40f);
            leftPanel.offsetMax = new Vector2(0f, -40f);

            // Class name
            classNameLabel = MakeTMP("ClassName", leftPanel);
            var cnRt = classNameLabel.GetComponent<RectTransform>();
            cnRt.anchorMin = new Vector2(0f, 1f);
            cnRt.anchorMax = new Vector2(1f, 1f);
            cnRt.pivot = new Vector2(0f, 1f);
            cnRt.anchoredPosition = Vector2.zero;
            cnRt.sizeDelta = new Vector2(0f, 32f);
            classNameLabel.fontSize = 22f;
            classNameLabel.fontStyle = FontStyles.Bold;
            classNameLabel.color = RimaUITheme.Cyan;

            // Class tagline
            classTaglineLabel = MakeTMP("ClassTagline", leftPanel);
            var ctRt = classTaglineLabel.GetComponent<RectTransform>();
            ctRt.anchorMin = new Vector2(0f, 1f);
            ctRt.anchorMax = new Vector2(1f, 1f);
            ctRt.pivot = new Vector2(0f, 1f);
            ctRt.anchoredPosition = new Vector2(0f, -36f);
            ctRt.sizeDelta = new Vector2(0f, 20f);
            classTaglineLabel.fontSize = 11f;
            classTaglineLabel.fontStyle = FontStyles.Italic;
            classTaglineLabel.color = new Color(0.6f, 0.7f, 0.75f, 0.85f);

            // Kit section header
            var kitHeader = MakeTMP("KitHeader", leftPanel);
            var khRt = kitHeader.GetComponent<RectTransform>();
            khRt.anchorMin = new Vector2(0f, 1f);
            khRt.anchorMax = new Vector2(1f, 1f);
            khRt.pivot = new Vector2(0f, 1f);
            khRt.anchoredPosition = new Vector2(0f, -70f);
            khRt.sizeDelta = new Vector2(0f, 18f);
            kitHeader.text = Loc.T("char_sheet.active_kit");
            kitHeader.fontSize = 10f;
            kitHeader.fontStyle = FontStyles.Bold;
            kitHeader.color = RimaUITheme.Gold;

            // Kit slots (LMB, RMB, 1-5)
            for (int i = 0; i < 7; i++)
            {
                string label = i switch { 0 => "LMB", 1 => "RMB", _ => (i - 1).ToString() };
                var kitTmp = MakeTMP($"Kit_{label}", leftPanel);
                var kRt = kitTmp.GetComponent<RectTransform>();
                kRt.anchorMin = new Vector2(0f, 1f);
                kRt.anchorMax = new Vector2(1f, 1f);
                kRt.pivot = new Vector2(0f, 1f);
                kRt.anchoredPosition = new Vector2(0f, -92f - i * 18f);
                kRt.sizeDelta = new Vector2(0f, 16f);
                kitTmp.fontSize = 10f;
                kitTmp.color = new Color(0.8f, 0.85f, 0.9f, 0.9f);
                kitTmp.text = $"[{label}] —";
                kitLabels.Add(kitTmp);
            }

            // Synergy chips section
            var synHeader = MakeTMP("SynergyHeader", leftPanel);
            var shRt = synHeader.GetComponent<RectTransform>();
            shRt.anchorMin = new Vector2(0f, 1f);
            shRt.anchorMax = new Vector2(1f, 1f);
            shRt.pivot = new Vector2(0f, 1f);
            shRt.anchoredPosition = new Vector2(0f, -230f);
            shRt.sizeDelta = new Vector2(0f, 18f);
            synHeader.text = Loc.T("char_sheet.synergies");
            synHeader.fontSize = 10f;
            synHeader.fontStyle = FontStyles.Bold;
            synHeader.color = RimaUITheme.Gold;

            // ── Right panel: expanded minimap + recent rewards ───────
            var rightPanel = MakeRect("RightPanel", root);
            rightPanel.anchorMin = new Vector2(0.55f, 0f);
            rightPanel.anchorMax = new Vector2(1f, 1f);
            rightPanel.offsetMin = new Vector2(0f, 40f);
            rightPanel.offsetMax = new Vector2(-40f, -40f);

            // Expanded map background
            var mapGo = new GameObject("ExpandedMap", typeof(RectTransform));
            mapGo.transform.SetParent(rightPanel, false);
            var mapRt = mapGo.GetComponent<RectTransform>();
            mapRt.anchorMin = new Vector2(0.5f, 1f);
            mapRt.anchorMax = new Vector2(0.5f, 1f);
            mapRt.pivot = new Vector2(0.5f, 1f);
            mapRt.anchoredPosition = Vector2.zero;
            mapRt.sizeDelta = new Vector2(ExpandedMapSize, ExpandedMapSize);

            expandedMapBg = mapGo.AddComponent<Image>();
            expandedMapBg.color = RimaUITheme.MapFrame;
            expandedMapBg.raycastTarget = false;

            // Route label
            var routeLabel = MakeTMP("RouteLabel", rightPanel);
            var rlRt = routeLabel.GetComponent<RectTransform>();
            rlRt.anchorMin = new Vector2(0f, 1f);
            rlRt.anchorMax = new Vector2(1f, 1f);
            rlRt.pivot = new Vector2(0.5f, 1f);
            rlRt.anchoredPosition = new Vector2(0f, -ExpandedMapSize - 8f);
            rlRt.sizeDelta = new Vector2(0f, 18f);
            routeLabel.text = Loc.T("char_sheet.dungeon_route");
            routeLabel.fontSize = 10f;
            routeLabel.fontStyle = FontStyles.Bold;
            routeLabel.color = RimaUITheme.Gold;
            routeLabel.alignment = TextAlignmentOptions.Center;

            // ── Bottom panel: active passives/echoes ─────────────────
            var bottomPanel = MakeRect("BottomPanel", root);
            bottomPanel.anchorMin = new Vector2(0f, 0f);
            bottomPanel.anchorMax = new Vector2(1f, 0f);
            bottomPanel.pivot = new Vector2(0.5f, 0f);
            bottomPanel.anchoredPosition = Vector2.zero;
            bottomPanel.sizeDelta = new Vector2(0f, 60f);
            bottomPanel.offsetMin = new Vector2(40f, 10f);
            bottomPanel.offsetMax = new Vector2(-40f, 0f);

            var passHeader = MakeTMP("PassiveHeader", bottomPanel);
            var phRt = passHeader.GetComponent<RectTransform>();
            phRt.anchorMin = new Vector2(0f, 1f);
            phRt.anchorMax = new Vector2(1f, 1f);
            phRt.pivot = new Vector2(0f, 1f);
            phRt.anchoredPosition = Vector2.zero;
            phRt.sizeDelta = new Vector2(0f, 16f);
            passHeader.text = Loc.T("char_sheet.active_echoes");
            passHeader.fontSize = 10f;
            passHeader.fontStyle = FontStyles.Bold;
            passHeader.color = RimaUITheme.Gold;
        }

        // ─── Refresh ────────────────────────────────────────────────

        private void RefreshContent()
        {
            var pcm = PlayerClassManager.Instance;
            ClassType primary = pcm != null ? pcm.PrimaryClass : ClassType.None;

            // Class name + tagline
            if (classNameLabel != null)
            {
                classNameLabel.text = primary != ClassType.None
                    ? primary.ToString().ToUpperInvariant()
                    : Loc.T("char_sheet.no_class");
                classNameLabel.color = RimaUITheme.ClassAccent(primary);
            }

            if (classTaglineLabel != null)
            {
                classTaglineLabel.text = primary switch
                {
                    ClassType.Warblade     => "Melee berserker — iron and fury",
                    ClassType.Elementalist => "Elemental caster — fire, frost, lightning",
                    ClassType.Ranger       => "Precision marksman — traps and volleys",
                    ClassType.Shadowblade  => "Shadow assassin — stealth and burst",
                    _ => ""
                };
            }

            // Kit labels
            RefreshKitLabels(primary);
        }

        private void RefreshKitLabels(ClassType primary)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            // Try to get active skills from controller
            int count = 0;
            System.Func<int, SkillBase> getter = null;

            switch (primary)
            {
                case ClassType.Warblade:
                    var wb = player.GetComponent<Warblade_SkillController>();
                    if (wb != null) { count = wb.SlotCount; getter = wb.GetSlot; }
                    break;
                case ClassType.Elementalist:
                    var el = player.GetComponent<Elementalist_SkillController>();
                    if (el != null) { count = el.SlotCount; getter = el.GetSlot; }
                    break;
                case ClassType.Ranger:
                    var rn = player.GetComponent<Ranger_SkillController>();
                    if (rn != null) { count = rn.SlotCount; getter = rn.GetSlot; }
                    break;
                case ClassType.Shadowblade:
                    var sb = player.GetComponent<Shadowblade_SkillController>();
                    if (sb != null) { count = sb.SlotCount; getter = sb.GetSlot; }
                    break;
            }

            string[] labels = { "LMB", "RMB", "1", "2", "3", "4", "5" };
            for (int i = 0; i < kitLabels.Count; i++)
            {
                if (i < count && getter != null)
                {
                    var skill = getter(i);
                    kitLabels[i].text = skill != null
                        ? $"[{labels[i]}] {skill.skillName}"
                        : $"[{labels[i]}] —";
                }
                else
                {
                    kitLabels[i].text = $"[{labels[i]}] —";
                }
            }
        }

        // ─── Util ───────────────────────────────────────────────────

        private static RectTransform MakeRect(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go.GetComponent<RectTransform>();
        }

        private static TextMeshProUGUI MakeTMP(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.raycastTarget = false;
            return tmp;
        }
    }
}
