using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace RIMA
{
    /// <summary>
    /// Evrensel tooltip sistemi — tüm UI elementlerinde kullanılabilir.
    /// Hover başlar → 0.3s sonra tooltip açılır.
    /// Mouse pozisyonuna göre dinamik konumlanır, ekran kenarında flip yapar.
    /// </summary>
    public class TooltipSystem : MonoBehaviour
    {
        public static TooltipSystem Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private float showDelay = 0.3f;
        [SerializeField] private Vector2 offset = new Vector2(15f, -15f);
        [SerializeField] private float padding = 10f;

        private const float MinWidth = 220f;
        private const float PreferredWidth = 280f;
        private const float MaxWidth = 320f;
        private const float MaxHeight = 400f;

        private GameObject tooltipPanel;
        private TextMeshProUGUI tooltipText;
        private RectTransform tooltipRect;
        private Canvas canvas;

        private Coroutine showCoroutine;
        private bool isVisible;
        private bool built;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            EnsureBuilt();
        }

        /// <summary>
        /// Build the tooltip panel lazily and idempotently. Safe to call before Start()
        /// runs (e.g. when the component is AddComponent'd the same frame as the first hover),
        /// and a no-op on every subsequent call so the panel is never duplicated.
        /// </summary>
        private void EnsureBuilt()
        {
            if (built && tooltipPanel != null) return;
            BuildTooltip();
        }

        private void BuildTooltip()
        {
            canvas = GetComponentInParent<Canvas>() ?? FindObjectOfType<Canvas>();
            if (canvas == null) return;

            built = true;

            // Tooltip panel
            tooltipPanel = new GameObject("Tooltip", typeof(RectTransform));
            tooltipPanel.transform.SetParent(canvas.transform, false);
            tooltipRect = tooltipPanel.GetComponent<RectTransform>();
            tooltipRect.sizeDelta = new Vector2(PreferredWidth, 150f);
            tooltipRect.pivot = new Vector2(0f, 1f); // Top-left pivot

            // Translucent ink-wash: readable, but not an opaque UI box.
            var bg = tooltipPanel.AddComponent<Image>();
            bg.color = new Color(0.01f, 0.018f, 0.026f, 0.62f);
            bg.raycastTarget = false;

            var washGo = new GameObject("InkWash", typeof(RectTransform));
            washGo.transform.SetParent(tooltipPanel.transform, false);
            var washRect = washGo.GetComponent<RectTransform>();
            washRect.anchorMin = Vector2.zero;
            washRect.anchorMax = Vector2.one;
            washRect.offsetMin = washRect.offsetMax = Vector2.zero;
            var wash = washGo.AddComponent<Image>();
            wash.sprite = RimaUITheme.RarityGlow(SkillTier.Common);
            wash.color = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.08f);
            wash.raycastTarget = false;

            // Cyan hairline, with top/bottom strokes carrying the frame.
            var outline = tooltipPanel.AddComponent<Outline>();
            outline.effectColor = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.55f);
            outline.effectDistance = new Vector2(1f, -1f);

            AddHairline("TopHairline", tooltipPanel.transform, true);
            AddHairline("BottomHairline", tooltipPanel.transform, false);

            // Text
            var textGo = new GameObject("Text", typeof(RectTransform));
            textGo.transform.SetParent(tooltipPanel.transform, false);
            var textRect = textGo.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(padding, padding);
            textRect.offsetMax = new Vector2(-padding, -padding);

            tooltipText = textGo.AddComponent<TextMeshProUGUI>();
            tooltipText.fontSize = 11;
            tooltipText.color = new Color(0.86f, 0.94f, 0.96f);
            tooltipText.alignment = TextAlignmentOptions.TopLeft;
            tooltipText.enableWordWrapping = true;
            tooltipText.overflowMode = TextOverflowModes.Overflow;
            tooltipText.raycastTarget = false;

            tooltipPanel.SetActive(false);
        }

        /// <summary>
        /// Tooltip göster (0.3s delay ile).
        /// </summary>
        public void Show(string content, Vector2? screenPosition = null)
        {
            EnsureBuilt(); // first Show may land before Start() — guarantee a ready panel

            if (showCoroutine != null)
                StopCoroutine(showCoroutine);

            showCoroutine = StartCoroutine(ShowDelayed(content, screenPosition));
        }

        /// <summary>
        /// Tooltip'i hemen gizle.
        /// </summary>
        public void Hide()
        {
            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            if (tooltipPanel != null)
                tooltipPanel.SetActive(false);

            isVisible = false;
        }

        private void OnDisable()
        {
            // Panel Canvas'ın çocuğu — host kapatılırsa açık tooltip ekranda asılı kalmasın (T4 Flash review Note 2)
            Hide();
        }

        private void OnDestroy()
        {
            // Panel Canvas altında yaşıyor; host destroy olunca orphan kalmasın (T4 Flash review Note 1)
            if (tooltipPanel != null)
                Destroy(tooltipPanel);
        }

        private IEnumerator ShowDelayed(string content, Vector2? screenPosition)
        {
            yield return new WaitForSecondsRealtime(showDelay);

            if (tooltipPanel == null || tooltipText == null) yield break;

            // Set content
            tooltipText.text = content;

            float width = PreferredWidth;
            float textWidth = width - padding * 2f;
            Vector2 textSize = tooltipText.GetPreferredValues(content, textWidth, 0f);
            tooltipRect.sizeDelta = new Vector2(
                Mathf.Clamp(textSize.x + padding * 2f, MinWidth, MaxWidth),
                Mathf.Min(textSize.y + padding * 2f, MaxHeight)
            );

            // Position (new Input System — legacy Input.mousePosition throws under InputSystem package)
            Vector2 pos = screenPosition ?? MousePos();
            PositionTooltip(pos);

            tooltipPanel.SetActive(true);
            isVisible = true;
        }

        private void PositionTooltip(Vector2 screenPos)
        {
            if (tooltipRect == null || canvas == null) return;

            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.worldCamera,
                out localPos
            );

            // Apply offset
            localPos += offset;

            // Get canvas rect
            var canvasRect = canvas.GetComponent<RectTransform>();
            float canvasWidth = canvasRect.rect.width;
            float canvasHeight = canvasRect.rect.height;

            // Flip horizontally if too close to right edge
            if (localPos.x + tooltipRect.sizeDelta.x > canvasWidth / 2f)
                localPos.x -= tooltipRect.sizeDelta.x + offset.x * 2f;

            // Flip vertically if too close to bottom edge
            if (localPos.y - tooltipRect.sizeDelta.y < -canvasHeight / 2f)
                localPos.y += tooltipRect.sizeDelta.y - offset.y * 2f;

            tooltipRect.anchoredPosition = localPos;
        }

        private static void AddHairline(string name, Transform parent, bool top)
        {
            var lineGo = new GameObject(name, typeof(RectTransform));
            lineGo.transform.SetParent(parent, false);
            var lineRt = lineGo.GetComponent<RectTransform>();
            lineRt.anchorMin = top ? new Vector2(0f, 1f) : new Vector2(0f, 0f);
            lineRt.anchorMax = top ? new Vector2(1f, 1f) : new Vector2(1f, 0f);
            lineRt.pivot = top ? new Vector2(0.5f, 1f) : new Vector2(0.5f, 0f);
            lineRt.offsetMin = new Vector2(8f, 0f);
            lineRt.offsetMax = new Vector2(-8f, 0f);
            lineRt.sizeDelta = new Vector2(lineRt.sizeDelta.x, 1f);
            var line = lineGo.AddComponent<Image>();
            line.color = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.62f);
            line.raycastTarget = false;
        }

        private void Update()
        {
            // Update position while visible (follows mouse)
            if (isVisible && tooltipPanel != null && tooltipPanel.activeSelf)
            {
                PositionTooltip(MousePos());
            }
        }

        /// <summary>Pointer position via the new Input System (legacy Input.mousePosition throws under the InputSystem package).</summary>
        private static Vector2 MousePos()
        {
            var m = UnityEngine.InputSystem.Mouse.current;
            return m != null ? m.position.ReadValue() : Vector2.zero;
        }

        // ── Content Formatters ──────────────────────────────────

        /// <summary>
        /// Skill tooltip formatı.
        /// </summary>
        public static string FormatSkill(SkillData skill)
        {
            if (skill == null) return "";

            var sb = new System.Text.StringBuilder();

            // Header: Name + Tier
            string tierColor = skill.tier switch
            {
                SkillTier.Common => "#AAAAAA",
                SkillTier.Rare => "#5F9FFF",
                SkillTier.Epic => "#A335EE",
                SkillTier.Mythic => "#FF8000",
                SkillTier.Legendary => "#E6CC80",
                _ => "#FFFFFF"
            };

            sb.AppendLine($"<size=12><b>{skill.skillName}</b></size>  <color={tierColor}>{skill.tier}</color>");
            sb.AppendLine("────────────────────");

            // Description (name-only skills have no description → no blank line)
            if (!string.IsNullOrWhiteSpace(skill.description))
            {
                sb.AppendLine(skill.description);
                sb.AppendLine();
            }

            // Stats
            if (!skill.isPassive)
            {
                if (skill.cooldown > 0f)
                    sb.AppendLine($"<color=#87CEEB>CD:</color> {skill.cooldown:F1}s");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Passive tooltip formatı (seviye bazlı).
        /// </summary>
        public static string FormatPassive(SkillData passive, int currentLevel)
        {
            if (passive == null) return "";

            var sb = new System.Text.StringBuilder();

            sb.AppendLine($"<size=12><b>{passive.skillName}</b></size>  <color=#FFA07A>Pasif</color>");
            sb.AppendLine("────────────────────");
            sb.AppendLine(passive.description);
            sb.AppendLine();
            sb.AppendLine($"<color=#5FD35F>Mevcut Seviye:</color> {currentLevel}/3");

            return sb.ToString();
        }

        /// <summary>
        /// Trait tooltip formatı.
        /// </summary>
        public static string FormatTrait(string traitName, string description, int stacks, int maxStacks)
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine($"<size=12><b>{traitName}</b></size>  <color=#F7DC6F>Trait</color>");
            sb.AppendLine("────────────────────");
            sb.AppendLine(description);
            sb.AppendLine();
            sb.AppendLine($"<color=#98D8C8>Stack:</color> {stacks}/{maxStacks}");

            return sb.ToString();
        }
    }
}
