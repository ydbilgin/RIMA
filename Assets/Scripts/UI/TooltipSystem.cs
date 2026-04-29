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

        private GameObject tooltipPanel;
        private TextMeshProUGUI tooltipText;
        private RectTransform tooltipRect;
        private Canvas canvas;

        private Coroutine showCoroutine;
        private bool isVisible;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            BuildTooltip();
        }

        private void BuildTooltip()
        {
            canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            // Tooltip panel
            tooltipPanel = new GameObject("Tooltip", typeof(RectTransform));
            tooltipPanel.transform.SetParent(canvas.transform, false);
            tooltipRect = tooltipPanel.GetComponent<RectTransform>();
            tooltipRect.sizeDelta = new Vector2(280f, 150f);
            tooltipRect.pivot = new Vector2(0f, 1f); // Top-left pivot

            // Background
            var bg = tooltipPanel.AddComponent<Image>();
            bg.color = new Color(0.08f, 0.08f, 0.12f, 0.95f);

            // Border
            var outline = tooltipPanel.AddComponent<Outline>();
            outline.effectColor = new Color(0.35f, 0.35f, 0.45f, 1f);
            outline.effectDistance = new Vector2(1f, -1f);

            // Text
            var textGo = new GameObject("Text", typeof(RectTransform));
            textGo.transform.SetParent(tooltipPanel.transform, false);
            var textRect = textGo.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(padding, padding);
            textRect.offsetMax = new Vector2(-padding, -padding);

            tooltipText = textGo.AddComponent<TextMeshProUGUI>();
            tooltipText.fontSize = 10;
            tooltipText.color = new Color(0.85f, 0.87f, 0.95f);
            tooltipText.alignment = TextAlignmentOptions.TopLeft;
            tooltipText.enableWordWrapping = true;
            tooltipText.raycastTarget = false;

            tooltipPanel.SetActive(false);
        }

        /// <summary>
        /// Tooltip göster (0.3s delay ile).
        /// </summary>
        public void Show(string content, Vector2? screenPosition = null)
        {
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

        private IEnumerator ShowDelayed(string content, Vector2? screenPosition)
        {
            yield return new WaitForSeconds(showDelay);

            if (tooltipPanel == null || tooltipText == null) yield break;

            // Set content
            tooltipText.text = content;

            // Force text update to get correct size
            tooltipText.ForceMeshUpdate();
            Vector2 textSize = tooltipText.GetRenderedValues(false);
            tooltipRect.sizeDelta = new Vector2(
                Mathf.Min(textSize.x + padding * 2f, 320f),
                Mathf.Min(textSize.y + padding * 2f, 400f)
            );

            // Position
            Vector2 pos = screenPosition ?? (Vector2)Input.mousePosition;
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

        private void Update()
        {
            // Update position while visible (follows mouse)
            if (isVisible && tooltipPanel != null && tooltipPanel.activeSelf)
            {
                PositionTooltip(Input.mousePosition);
            }
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

            // Description
            sb.AppendLine(skill.description);
            sb.AppendLine();

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
