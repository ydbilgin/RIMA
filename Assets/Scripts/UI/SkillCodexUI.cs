using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Runtime-built full-screen skill codex. UIManager owns open state and timeScale.
    /// </summary>
    public class SkillCodexUI : MonoBehaviour
    {
        private static readonly ClassType[] Classes =
        {
            ClassType.Warblade,
            ClassType.Elementalist,
            ClassType.Shadowblade,
            ClassType.Ranger,
            ClassType.Ravager,
            ClassType.Ronin,
            ClassType.Gunslinger,
            ClassType.Brawler,
            ClassType.Summoner,
            ClassType.Hexer
        };

        private CanvasGroup canvasGroup;
        private RectTransform classButtonRoot;
        private RectTransform skillContent;
        private TMP_Text classTitle;
        private TMP_Text emptyLabel;
        private readonly Dictionary<ClassType, Image> classButtonFrames = new Dictionary<ClassType, Image>();
        private ClassType selectedClass = ClassType.Warblade;
        private bool built;

        public static SkillCodexUI EnsureInstance()
        {
            var existing = FindAnyObjectByType<SkillCodexUI>();
            if (existing != null) return existing;

            EnsureEventSystem();

            var canvasGo = new GameObject("[SkillCodexUI]");
            DontDestroyOnLoad(canvasGo);

            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1095;

            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            canvasGo.AddComponent<GraphicRaycaster>();
            return canvasGo.AddComponent<SkillCodexUI>();
        }

        private void Awake()
        {
            EnsureReady();
            Close();
        }

        public void Open()
        {
            EnsureReady();
            selectedClass = ResolveDefaultClass();
            RefreshClassButtons();
            RefreshSkillList();
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Close()
        {
            EnsureReady();
            TooltipSystem.Instance?.Hide();
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        private void EnsureReady()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            if (built) return;
            BuildUI();
            built = true;
        }

        private static ClassType ResolveDefaultClass()
        {
            if (PlayerClassManager.SelectedClass != ClassType.None)
                return PlayerClassManager.SelectedClass;
            if (PlayerClassManager.Instance != null && PlayerClassManager.Instance.PrimaryClass != ClassType.None)
                return PlayerClassManager.Instance.PrimaryClass;
            return ClassType.Warblade;
        }

        private void BuildUI()
        {
            var root = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.offsetMin = root.offsetMax = Vector2.zero;

            var overlay = MakePanel("Overlay", root);
            SetStretch(overlay, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var overlayImg = overlay.GetComponent<Image>();
            overlayImg.color = new Color(0.012f, 0.018f, 0.026f, 0.92f);
            overlayImg.raycastTarget = true;

            var frame = MakePanel("Frame", overlay);
            SetStretch(frame, new Vector2(0.06f, 0.07f), new Vector2(0.94f, 0.92f), Vector2.zero, Vector2.zero);
            var frameImg = frame.GetComponent<Image>();
            frameImg.sprite = RimaUITheme.ResourceFrame;
            frameImg.type = Image.Type.Sliced;
            frameImg.color = new Color(RimaUITheme.CharSelectCyan.r, RimaUITheme.CharSelectCyan.g, RimaUITheme.CharSelectCyan.b, 0.72f);
            frameImg.raycastTarget = false;

            var panel = MakePanel("Panel", frame);
            SetStretch(panel, Vector2.zero, Vector2.one, new Vector2(2f, 2f), new Vector2(-2f, -2f));
            var panelImg = panel.GetComponent<Image>();
            panelImg.sprite = RimaUITheme.SmallPanelFrame;
            panelImg.type = Image.Type.Sliced;
            panelImg.color = new Color(0.024f, 0.028f, 0.038f, 0.86f);
            panelImg.raycastTarget = false;

            var title = MakeText(Loc.T("codex.title"), panel, 26f, FontStyles.Bold, RimaUITheme.CharSelectParchment);
            title.alignment = TextAlignmentOptions.Left;
            var titleRt = title.rectTransform;
            titleRt.anchorMin = new Vector2(0f, 1f);
            titleRt.anchorMax = new Vector2(1f, 1f);
            titleRt.pivot = new Vector2(0f, 1f);
            titleRt.anchoredPosition = new Vector2(32f, -22f);
            titleRt.sizeDelta = new Vector2(-64f, 36f);

            classTitle = MakeText("", panel, 18f, FontStyles.Bold, RimaUITheme.CharSelectCyan);
            classTitle.alignment = TextAlignmentOptions.Left;
            var classTitleRt = classTitle.rectTransform;
            classTitleRt.anchorMin = new Vector2(0f, 1f);
            classTitleRt.anchorMax = new Vector2(1f, 1f);
            classTitleRt.pivot = new Vector2(0f, 1f);
            classTitleRt.anchoredPosition = new Vector2(32f, -74f);
            classTitleRt.sizeDelta = new Vector2(-64f, 28f);

            BuildClassSelector(panel);
            BuildSkillScroll(panel);
        }

        private void BuildClassSelector(RectTransform parent)
        {
            classButtonRoot = MakeRect("ClassSelector", parent);
            classButtonRoot.anchorMin = new Vector2(0f, 1f);
            classButtonRoot.anchorMax = new Vector2(1f, 1f);
            classButtonRoot.pivot = new Vector2(0.5f, 1f);
            classButtonRoot.anchoredPosition = new Vector2(0f, -114f);
            classButtonRoot.sizeDelta = new Vector2(-64f, 82f);

            var grid = classButtonRoot.gameObject.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(176f, 34f);
            grid.spacing = new Vector2(8f, 8f);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 5;
            grid.childAlignment = TextAnchor.UpperCenter;

            for (int i = 0; i < Classes.Length; i++)
            {
                ClassType cls = Classes[i];
                var buttonRt = MakePanel(cls.ToString(), classButtonRoot);
                var img = buttonRt.GetComponent<Image>();
                img.sprite = RimaUITheme.ResourceFrame;
                img.type = Image.Type.Sliced;
                img.raycastTarget = true;
                classButtonFrames[cls] = img;

                var label = MakeText(cls.ToString().ToUpperInvariant(), buttonRt, 10f, FontStyles.Bold, RimaUITheme.CharSelectParchment);
                label.alignment = TextAlignmentOptions.Center;
                SetStretch(label.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

                var button = buttonRt.gameObject.AddComponent<Button>();
                button.targetGraphic = img;
                button.onClick.AddListener(() =>
                {
                    selectedClass = cls;
                    RefreshClassButtons();
                    RefreshSkillList();
                });
            }
        }

        private void BuildSkillScroll(RectTransform parent)
        {
            var viewport = MakePanel("SkillViewport", parent);
            SetStretch(viewport, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(32f, 32f), new Vector2(-32f, -214f));
            viewport.gameObject.AddComponent<RectMask2D>();
            var viewportImg = viewport.GetComponent<Image>();
            viewportImg.color = new Color(0f, 0f, 0f, 0f);
            viewportImg.raycastTarget = true;

            skillContent = MakeRect("Content", viewport);
            skillContent.anchorMin = new Vector2(0f, 1f);
            skillContent.anchorMax = new Vector2(1f, 1f);
            skillContent.pivot = new Vector2(0.5f, 1f);
            skillContent.anchoredPosition = Vector2.zero;
            skillContent.sizeDelta = new Vector2(0f, 0f);

            var layout = skillContent.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 8f;
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            var fitter = skillContent.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.viewport = viewport;
            scroll.content = skillContent;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;

            emptyLabel = MakeText(Loc.T("codex.empty"), viewport, 16f, FontStyles.Normal, RimaUITheme.CharSelectTextBody);
            emptyLabel.alignment = TextAlignmentOptions.Center;
            SetStretch(emptyLabel.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private void RefreshClassButtons()
        {
            foreach (var kv in classButtonFrames)
            {
                Color accent = RimaUITheme.ClassAccent(kv.Key);
                kv.Value.color = kv.Key == selectedClass
                    ? new Color(accent.r, accent.g, accent.b, 0.78f)
                    : new Color(RimaUITheme.CharSelectIronGrey.r, RimaUITheme.CharSelectIronGrey.g, RimaUITheme.CharSelectIronGrey.b, 0.82f);
            }

            if (classTitle != null)
            {
                classTitle.text = selectedClass.ToString().ToUpperInvariant();
                classTitle.color = RimaUITheme.ClassAccent(selectedClass);
            }
        }

        private void RefreshSkillList()
        {
            if (skillContent == null) return;

            for (int i = skillContent.childCount - 1; i >= 0; i--)
                Destroy(skillContent.GetChild(i).gameObject);

            var skills = SkillsFor(selectedClass);
            if (emptyLabel != null) emptyLabel.gameObject.SetActive(skills.Count == 0);

            for (int i = 0; i < skills.Count; i++)
                BuildSkillRow(skillContent, skills[i]);
        }

        private static List<SkillData> SkillsFor(ClassType cls)
        {
            var result = new List<SkillData>();
            var db = EnsureSkillDatabase();
            if (db == null) return result;

            var all = db.GetAll();
            for (int i = 0; i < all.Count; i++)
            {
                SkillData skill = all[i];
                if (skill != null && skill.classType == cls)
                    result.Add(skill);
            }

            return result;
        }

        private void BuildSkillRow(RectTransform parent, SkillData skill)
        {
            bool implemented = skill.isImplemented;
            Color accent = RimaUITheme.ClassAccent(skill.classType);

            var row = MakePanel("Skill_" + SafeName(skill.skillName), parent);
            row.sizeDelta = new Vector2(0f, implemented ? 58f : 34f);
            var layout = row.gameObject.AddComponent<LayoutElement>();
            layout.preferredHeight = implemented ? 58f : 34f;
            layout.minHeight = implemented ? 58f : 34f;

            var rowImg = row.GetComponent<Image>();
            rowImg.sprite = RimaUITheme.SmallPanelFrame;
            rowImg.type = Image.Type.Sliced;
            rowImg.color = implemented
                ? new Color(0.030f, 0.034f, 0.046f, 0.72f)
                : new Color(0.010f, 0.010f, 0.014f, 0.58f);
            rowImg.raycastTarget = implemented;

            var stripe = MakePanel("Accent", row);
            stripe.anchorMin = new Vector2(0f, 0f);
            stripe.anchorMax = new Vector2(0f, 1f);
            stripe.pivot = new Vector2(0f, 0.5f);
            stripe.sizeDelta = new Vector2(2f, 0f);
            stripe.anchoredPosition = Vector2.zero;
            stripe.GetComponent<Image>().color = implemented
                ? new Color(accent.r, accent.g, accent.b, 0.82f)
                : new Color(0.18f, 0.18f, 0.20f, 0.42f);

            if (implemented)
                BuildImplementedRow(row, skill, accent);
            else
                BuildPlaceholderRow(row, skill);
        }

        private void BuildImplementedRow(RectTransform row, SkillData skill, Color accent)
        {
            var iconFrame = MakePanel("IconFrame", row);
            iconFrame.anchorMin = new Vector2(0f, 0.5f);
            iconFrame.anchorMax = new Vector2(0f, 0.5f);
            iconFrame.pivot = new Vector2(0.5f, 0.5f);
            iconFrame.anchoredPosition = new Vector2(30f, 0f);
            iconFrame.sizeDelta = new Vector2(38f, 38f);
            var frameImg = iconFrame.GetComponent<Image>();
            frameImg.sprite = RimaUITheme.SmallPanelFrame;
            frameImg.type = Image.Type.Sliced;
            frameImg.color = new Color(accent.r, accent.g, accent.b, 0.25f);
            frameImg.raycastTarget = false;

            Sprite icon = skill.icon != null ? skill.icon : RimaUITheme.PassiveIcon(skill.skillName);
            if (icon != null)
            {
                var iconRt = MakePanel("Icon", iconFrame);
                SetStretch(iconRt, Vector2.zero, Vector2.one, new Vector2(4f, 4f), new Vector2(-4f, -4f));
                var iconImg = iconRt.GetComponent<Image>();
                iconImg.sprite = icon;
                iconImg.preserveAspect = true;
                iconImg.color = Color.white;
                iconImg.raycastTarget = false;
            }

            var name = MakeText(skill.skillName.ToUpperInvariant(), row, 12f, FontStyles.Bold, RimaUITheme.CharSelectParchment);
            name.alignment = TextAlignmentOptions.TopLeft;
            name.enableWordWrapping = false;
            name.overflowMode = TextOverflowModes.Ellipsis;
            name.rectTransform.anchorMin = new Vector2(0f, 0.54f);
            name.rectTransform.anchorMax = new Vector2(0.68f, 0.96f);
            name.rectTransform.offsetMin = new Vector2(60f, 0f);
            name.rectTransform.offsetMax = new Vector2(-4f, 0f);

            var desc = MakeText(OneLine(skill.description), row, 9f, FontStyles.Normal, new Color(RimaUITheme.CharSelectTextBody.r, RimaUITheme.CharSelectTextBody.g, RimaUITheme.CharSelectTextBody.b, 0.88f));
            desc.alignment = TextAlignmentOptions.TopLeft;
            desc.enableWordWrapping = false;
            desc.overflowMode = TextOverflowModes.Ellipsis;
            desc.rectTransform.anchorMin = new Vector2(0f, 0.08f);
            desc.rectTransform.anchorMax = new Vector2(0.78f, 0.50f);
            desc.rectTransform.offsetMin = new Vector2(60f, 0f);
            desc.rectTransform.offsetMax = new Vector2(-4f, 0f);

            var meta = MakeText(MetaText(skill), row, 10f, FontStyles.Bold, new Color(accent.r, accent.g, accent.b, 0.88f));
            meta.alignment = TextAlignmentOptions.Right;
            meta.enableWordWrapping = false;
            meta.overflowMode = TextOverflowModes.Ellipsis;
            meta.rectTransform.anchorMin = new Vector2(0.72f, 0f);
            meta.rectTransform.anchorMax = new Vector2(1f, 1f);
            meta.rectTransform.offsetMin = new Vector2(4f, 0f);
            meta.rectTransform.offsetMax = new Vector2(-14f, 0f);

            AddTooltip(row.gameObject, skill);
        }

        private void BuildPlaceholderRow(RectTransform row, SkillData skill)
        {
            var name = MakeText(Loc.T("codex.coming_soon", skill.skillName.ToUpperInvariant()), row, 10.5f, FontStyles.Bold, RimaUITheme.CharSelectLockedText);
            name.alignment = TextAlignmentOptions.Left;
            name.enableWordWrapping = false;
            name.overflowMode = TextOverflowModes.Ellipsis;
            name.rectTransform.anchorMin = Vector2.zero;
            name.rectTransform.anchorMax = Vector2.one;
            name.rectTransform.offsetMin = new Vector2(18f, 0f);
            name.rectTransform.offsetMax = new Vector2(-12f, 0f);
        }

        private static void AddTooltip(GameObject target, SkillData skill)
        {
            var trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();

            var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            enter.callback.AddListener(_ =>
            {
                EnsureTooltipSystem();
                TooltipSystem.Instance?.Show(TooltipSystem.FormatSkill(skill));
            });
            trigger.triggers.Add(enter);

            var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            exit.callback.AddListener(_ => TooltipSystem.Instance?.Hide());
            trigger.triggers.Add(exit);
        }

        private static SkillDatabase EnsureSkillDatabase()
        {
            if (SkillDatabase.Instance != null)
            {
                SkillDatabase.Instance.EnsureBuilt();
                RebuildInterruptedDatabase(SkillDatabase.Instance);
                return SkillDatabase.Instance;
            }

            var existing = FindAnyObjectByType<SkillDatabase>();
            if (existing != null)
            {
                existing.EnsureBuilt();
                RebuildInterruptedDatabase(existing);
                return existing;
            }

            var go = new GameObject("SkillDatabase_Auto");
            DontDestroyOnLoad(go);
            var db = go.AddComponent<SkillDatabase>();
            db.EnsureBuilt();
            RebuildInterruptedDatabase(db);
            return db;
        }

        private static void RebuildInterruptedDatabase(SkillDatabase database)
        {
            if (database == null || database.GetAll().Count > 0) return;

            var builtField = typeof(SkillDatabase).GetField(
                "built",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            builtField?.SetValue(database, false);
            database.EnsureBuilt();
        }

        private static void EnsureTooltipSystem()
        {
            if (TooltipSystem.Instance != null) return;

            var go = new GameObject("TooltipSystem_Runtime");
            DontDestroyOnLoad(go);
            go.AddComponent<TooltipSystem>();
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;

            var go = new GameObject("EventSystem");
            DontDestroyOnLoad(go);
            go.AddComponent<EventSystem>();
            go.AddComponent<InputSystemUIInputModule>();
        }

        private static string MetaText(SkillData skill)
        {
            if (skill.isPassive) return $"{skill.tier}  PASSIVE";
            return skill.cooldown > 0f ? $"{skill.tier}  CD {skill.cooldown:0.#}S" : skill.tier.ToString();
        }

        private static string OneLine(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "" : value.Replace('\n', ' ').Replace('\r', ' ');
        }

        private static string SafeName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "Unknown";
            return value.Replace(" ", "").Replace("'", "").Replace("-", "");
        }

        private static RectTransform MakeRect(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go.GetComponent<RectTransform>();
        }

        private static RectTransform MakePanel(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            return rt;
        }

        private static TextMeshProUGUI MakeText(string text, RectTransform parent, float size, FontStyles style, Color color)
        {
            var go = new GameObject("Lbl", typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.fontStyle = style;
            tmp.color = color;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;
            tmp.enableWordWrapping = false;
            return tmp;
        }

        private static void SetStretch(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = offsetMin;
            rt.offsetMax = offsetMax;
        }
    }
}
