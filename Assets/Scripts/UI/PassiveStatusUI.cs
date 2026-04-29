using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// HUD panel: oyuncunun aktif status efektlerini ikonlar halinde gösterir.
    /// SkillBarUI'nin soluna konumlandırılır. Her icon: renkli daire + stack sayısı + tooltip.
    /// </summary>
    public class PassiveStatusUI : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private float iconSize = 36f;
        [SerializeField] private float spacing = 6f;
        [SerializeField] private Color tooltipBg = new Color(0.08f, 0.08f, 0.16f, 0.95f);

        private StatusEffectSystem playerEffects;
        private readonly Dictionary<StatusEffectType, IconSlot> icons = new Dictionary<StatusEffectType, IconSlot>();
        private GameObject tooltipGo;
        private TextMeshProUGUI tooltipText;
        private StatusEffectType hoveredEffect;

        private struct IconSlot
        {
            public GameObject root;
            public Image bg;
            public TextMeshProUGUI stackLabel;
            public TextMeshProUGUI nameLabel;
        }

        // Effect display info: (short name, color, tooltip text)
        private static readonly Dictionary<StatusEffectType, (string name, Color color, string tip)> EffectInfo
            = new Dictionary<StatusEffectType, (string, Color, string)>
        {
            { StatusEffectType.Chill,    ("CHI", new Color(0.4f, 0.8f, 1f),   "Chill: Hareket hızı -%10/yığın. 3 yığın → Frozen.") },
            { StatusEffectType.Frozen,   ("FRZ", new Color(0.7f, 0.95f, 1f),  "Frozen: Tamamen dondu. Vur → Ice Shatter (3x hasar, AOE).") },
            { StatusEffectType.Burning,  ("BRN", new Color(1f,  0.5f,  0.1f), "Burning: 1 hasar/sn/yığın. 3 yığın → Scorch.") },
            { StatusEffectType.Scorch,   ("SCH", new Color(1f,  0.2f,  0f),   "Scorch: 3 hasar/sn + zırh -%25.") },
            { StatusEffectType.Poison,   ("PSN", new Color(0.3f, 0.9f, 0.3f), "Poison: 0.5 hasar/sn/yığın (maks 8, azalan getiri).") },
            { StatusEffectType.Shocked,  ("SHK", new Color(1f,  0.95f, 0.2f), "Shocked: Saldırı hızı -%30. Sonraki skill bedava.") },
            { StatusEffectType.Stunned,  ("STN", new Color(0.8f, 0.8f, 0.3f), "Stunned: Hareket etemiyor (kısa süre).") },
            { StatusEffectType.Weakened, ("WKN", new Color(0.8f, 0.3f, 0.8f), "Weakened: +%25 hasar alır.") },
            { StatusEffectType.RiftMark, ("RFT", new Color(0.55f,0.2f, 1f),   "Rift Mark: 5 yığın → Void Burst (Warblade/Elementalist sinerji).") },
        };

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            playerEffects = player.GetComponent<StatusEffectSystem>();
            if (playerEffects == null) return;

            playerEffects.OnEffectApplied.AddListener(OnEffectApplied);
            playerEffects.OnEffectRemoved.AddListener(OnEffectRemoved);

            BuildTooltip();
        }

        private void OnDestroy()
        {
            if (playerEffects != null)
            {
                playerEffects.OnEffectApplied.RemoveListener(OnEffectApplied);
                playerEffects.OnEffectRemoved.RemoveListener(OnEffectRemoved);
            }
        }

        // ─── Event Handlers ──────────────────────────────────────────────────

        private void OnEffectApplied(StatusEffectType type, int stacks)
        {
            if (!icons.ContainsKey(type))
                CreateIcon(type);
            UpdateIcon(type, stacks);
            LayoutIcons();
        }

        private void OnEffectRemoved(StatusEffectType type)
        {
            if (icons.TryGetValue(type, out var slot))
            {
                Destroy(slot.root);
                icons.Remove(type);
            }
            LayoutIcons();
        }

        // ─── Icon Build ──────────────────────────────────────────────────────

        private void CreateIcon(StatusEffectType type)
        {
            if (!EffectInfo.TryGetValue(type, out var info)) return;

            var root = new GameObject($"Status_{type}", typeof(RectTransform));
            root.transform.SetParent(transform, false);
            var rt = root.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(iconSize, iconSize);

            // Background circle (colored)
            var bgGo = new GameObject("BG", typeof(RectTransform));
            bgGo.transform.SetParent(root.transform, false);
            var bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.sizeDelta = new Vector2(iconSize, iconSize);
            bgRt.anchorMin = bgRt.anchorMax = new Vector2(0.5f, 0.5f);
            bgRt.anchoredPosition = Vector2.zero;
            var bgImg = bgGo.AddComponent<Image>();
            bgImg.color = info.color;

            // Short name label (center)
            var nameGo = MakeChild(root.transform, "Name", iconSize, iconSize);
            var nameTmp = nameGo.AddComponent<TextMeshProUGUI>();
            nameTmp.text = info.name;
            nameTmp.fontSize = 9;
            nameTmp.fontStyle = FontStyles.Bold;
            nameTmp.color = Color.black;
            nameTmp.alignment = TextAlignmentOptions.Center;
            nameTmp.raycastTarget = false;

            // Stack count (bottom-right)
            var stackGo = MakeChild(root.transform, "Stack", 18f, 14f);
            var stackRt = stackGo.GetComponent<RectTransform>();
            stackRt.anchorMin = stackRt.anchorMax = new Vector2(1f, 0f);
            stackRt.pivot = new Vector2(1f, 0f);
            stackRt.anchoredPosition = new Vector2(-1f, 1f);
            var stackTmp = stackGo.AddComponent<TextMeshProUGUI>();
            stackTmp.fontSize = 9;
            stackTmp.fontStyle = FontStyles.Bold;
            stackTmp.color = Color.white;
            stackTmp.alignment = TextAlignmentOptions.Right;
            stackTmp.raycastTarget = false;

            // Hover trigger
            var hover = root.AddComponent<StatusIconHover>();
            hover.Init(type, this);
            bgImg.raycastTarget = true;

            icons[type] = new IconSlot { root = root, bg = bgImg, stackLabel = stackTmp, nameLabel = nameTmp };
        }

        private void UpdateIcon(StatusEffectType type, int stacks)
        {
            if (!icons.TryGetValue(type, out var slot)) return;
            slot.stackLabel.text = stacks > 1 ? stacks.ToString() : "";
        }

        private void LayoutIcons()
        {
            int i = 0;
            foreach (var kvp in icons)
            {
                var rt = kvp.Value.root.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(i * (iconSize + spacing), 0f);
                i++;
            }
        }

        // ─── Tooltip ─────────────────────────────────────────────────────────

        private void BuildTooltip()
        {
            tooltipGo = new GameObject("StatusTooltip", typeof(RectTransform));
            tooltipGo.transform.SetParent(transform.root, false);

            var rt = tooltipGo.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(220f, 50f);
            rt.pivot = new Vector2(0f, 0f);

            var bg = tooltipGo.AddComponent<Image>();
            bg.color = tooltipBg;

            var txtGo = MakeChild(tooltipGo.transform, "Txt", 210f, 40f);
            tooltipText = txtGo.AddComponent<TextMeshProUGUI>();
            tooltipText.fontSize = 11;
            tooltipText.color = Color.white;
            tooltipText.alignment = TextAlignmentOptions.TopLeft;
            tooltipText.raycastTarget = false;
            tooltipText.margin = new Vector4(6, 4, 6, 4);

            tooltipGo.SetActive(false);
        }

        public void ShowTooltip(StatusEffectType type, Vector2 screenPos)
        {
            if (!EffectInfo.TryGetValue(type, out var info)) return;
            tooltipText.text = info.tip;
            tooltipGo.SetActive(true);
            tooltipGo.GetComponent<RectTransform>().position = screenPos + new Vector2(0, 44f);
        }

        public void HideTooltip()
        {
            if (tooltipGo != null)
                tooltipGo.SetActive(false);
        }

        // ─── Util ────────────────────────────────────────────────────────────

        private static GameObject MakeChild(Transform parent, string name, float w, float h)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(w, h);
            rt.anchoredPosition = Vector2.zero;
            return go;
        }
    }

    // ─── Hover helper ────────────────────────────────────────────────────────

    public class StatusIconHover : MonoBehaviour,
        UnityEngine.EventSystems.IPointerEnterHandler,
        UnityEngine.EventSystems.IPointerExitHandler
    {
        private StatusEffectType effectType;
        private PassiveStatusUI ui;

        public void Init(StatusEffectType t, PassiveStatusUI parent)
        {
            effectType = t;
            ui = parent;
        }

        public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData e)
            => ui?.ShowTooltip(effectType, e.position);

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData e)
            => ui?.HideTooltip();
    }
}
