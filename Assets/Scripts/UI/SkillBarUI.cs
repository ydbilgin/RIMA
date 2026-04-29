using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// HUD skill bar: 4 primary (Q/E/R/F) + 2 secondary (Z/X, başta kilitli).
    /// Secondary, PlayerClassManager.OnSecondaryClassSelected ile açılır.
    /// </summary>
    public class SkillBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Warblade_SkillController skillController;

        [Header("Style")]
        [SerializeField] private float slotSize    = 52f;
        [SerializeField] private float spacing     = 8f;
        [SerializeField] private float groupGap    = 20f; // primary / secondary arası boşluk

        private readonly Color bgColor          = new Color(0.10f, 0.10f, 0.18f, 0.90f);
        private readonly Color cdOverlayColor   = new Color(0.05f, 0.05f, 0.12f, 0.75f);
        private readonly Color readyBorderColor = new Color(0.28f, 0.72f, 0.95f, 0.60f);
        private readonly Color keyLabelColor    = new Color(0.85f, 0.88f, 0.95f, 0.95f);
        private readonly Color emptySlotColor   = new Color(0.15f, 0.15f, 0.22f, 0.80f);
        private readonly Color lockedColor      = new Color(0.10f, 0.10f, 0.14f, 0.70f);
        private readonly Color dragHighlight    = new Color(0.45f, 0.75f, 1f,   0.35f);
        private Color secondaryAccent           = new Color(0.6f,  0.25f, 0.9f, 0.60f); // secondary class rengiyle güncellenir

        private static readonly string[] SlotKeys = { "Q", "E", "R", "F", "Z", "X" };

        private SlotUI[] slotUIs = new SlotUI[6];

        private int dragSourceSlot = -1;
        private GameObject dragGhost;
        private float holdTimer;
        private const float HoldThreshold = 0.3f;
        private bool isDragging;

        private struct SlotUI
        {
            public GameObject root;
            public Image bg, icon, cdOverlay, border, lockOverlay;
            public TextMeshProUGUI keyLabel, cdText;
            public int index;
        }

        private void Start()
        {
            if (skillController == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) skillController = player.GetComponent<Warblade_SkillController>();
            }

            BuildSlots();

            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.OnSecondaryClassSelected += OnSecondaryPicked;
        }

        private void OnDestroy()
        {
            if (PlayerClassManager.Instance != null)
                PlayerClassManager.Instance.OnSecondaryClassSelected -= OnSecondaryPicked;
        }

        private void OnSecondaryPicked(ClassType type)
        {
            // Secondary slot accent rengini class'a göre güncelle
            secondaryAccent = type switch
            {
                ClassType.Elementalist => new Color(0.3f, 0.6f, 1f,  0.60f),
                ClassType.Shadowblade  => new Color(0.6f, 0.2f, 0.9f, 0.60f),
                ClassType.Ranger       => new Color(0.3f, 0.8f, 0.4f, 0.60f),
                _                      => secondaryAccent
            };

            // Kilit kaldır
            for (int i = 4; i < 6; i++)
            {
                if (slotUIs[i].lockOverlay != null)
                    slotUIs[i].lockOverlay.gameObject.SetActive(false);
                if (slotUIs[i].border != null)
                    slotUIs[i].border.color = secondaryAccent;
            }
        }

        public void RefreshKeyLabels()
        {
            for (int i = 0; i < 4; i++)
                if (slotUIs[i].keyLabel != null)
                    slotUIs[i].keyLabel.text = KeyBindManager.GetKeyName(i);
        }

        // ─── Build ──────────────────────────────────────────────

        private void BuildSlots()
        {
            var container = GetComponent<RectTransform>();

            // Toplam genişlik: 4 primary + gap + 2 secondary
            float totalW = 4 * slotSize + 3 * spacing + groupGap + 2 * slotSize + spacing;
            float startX = -totalW / 2f + slotSize / 2f;

            for (int i = 0; i < 6; i++)
            {
                float x;
                if (i < 4)
                    x = startX + i * (slotSize + spacing);
                else
                    x = startX + 4 * (slotSize + spacing) + groupGap + (i - 4) * (slotSize + spacing);

                var slot = new GameObject($"SkillSlot_{i}", typeof(RectTransform));
                slot.transform.SetParent(container, false);
                var rt = slot.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(slotSize, slotSize);
                rt.anchoredPosition = new Vector2(x, 0);

                var bgImg = slot.AddComponent<Image>();
                bgImg.color = bgColor;
                bgImg.raycastTarget = true;

                // Border
                var borderGo = MakeChild(slot.transform, "Border", slotSize + 4, slotSize + 4);
                var borderImg = borderGo.AddComponent<Image>();
                borderImg.color = Color.clear;
                borderImg.raycastTarget = false;
                borderGo.transform.SetAsFirstSibling();

                // Icon
                var iconGo = MakeChild(slot.transform, "Icon", slotSize - 8, slotSize - 8);
                var iconImg = iconGo.AddComponent<Image>();
                iconImg.color = emptySlotColor;
                iconImg.raycastTarget = false;

                // CD overlay
                var cdGo  = MakeChild(slot.transform, "CD", slotSize - 8, slotSize - 8);
                var cdImg = cdGo.AddComponent<Image>();
                cdImg.color = cdOverlayColor;
                cdImg.type = Image.Type.Filled;
                cdImg.fillMethod = Image.FillMethod.Radial360;
                cdImg.fillOrigin = (int)Image.Origin360.Top;
                cdImg.fillClockwise = false;
                cdImg.fillAmount = 0f;
                cdImg.raycastTarget = false;

                // CD text
                var cdTxtGo = MakeChild(slot.transform, "CDTxt", slotSize, 20);
                var cdTxt = cdTxtGo.AddComponent<TextMeshProUGUI>();
                cdTxt.alignment = TextAlignmentOptions.Center;
                cdTxt.fontSize = 12;
                cdTxt.color = Color.white;
                cdTxt.text = "";
                cdTxt.raycastTarget = false;

                // Key label
                var keyGo  = MakeChild(slot.transform, "Key", 20, 16);
                var keyTxt = keyGo.AddComponent<TextMeshProUGUI>();
                keyTxt.text = SlotKeys[i];
                keyTxt.fontSize = 10;
                keyTxt.fontStyle = FontStyles.Bold;
                keyTxt.color = keyLabelColor;
                keyTxt.alignment = TextAlignmentOptions.Center;
                keyTxt.raycastTarget = false;
                var keyRt = keyGo.GetComponent<RectTransform>();
                keyRt.anchorMin = new Vector2(1, 0);
                keyRt.anchorMax = new Vector2(1, 0);
                keyRt.pivot = new Vector2(1, 0);
                keyRt.anchoredPosition = new Vector2(-2, 2);

                // Kilit overlay (sadece secondary slotlar)
                Image lockImg = null;
                if (i >= 4)
                {
                    var lockGo = MakeChild(slot.transform, "Lock", slotSize, slotSize);
                    lockImg = lockGo.AddComponent<Image>();
                    lockImg.color = lockedColor;
                    lockImg.raycastTarget = false;

                    // Kilit yazısı
                    var lockTxtGo = MakeChild(lockGo.transform, "LockTxt", slotSize, slotSize);
                    var lockTxt   = lockTxtGo.AddComponent<TextMeshProUGUI>();
                    lockTxt.text      = "X";
                    lockTxt.fontSize  = 18;
                    lockTxt.alignment = TextAlignmentOptions.Center;
                    lockTxt.raycastTarget = false;
                }

                slotUIs[i] = new SlotUI
                {
                    root = slot, bg = bgImg, icon = iconImg,
                    cdOverlay = cdImg, border = borderImg,
                    keyLabel = keyTxt, cdText = cdTxt,
                    lockOverlay = lockImg, index = i
                };
            }
        }

        // ─── Update ─────────────────────────────────────────────

        private bool wasSecondaryUnlocked = false;

        private void Update()
        {
            if (skillController == null) return;

            // Event kaçırılmış olabilir — state'i poll et
            if (!wasSecondaryUnlocked && skillController.SecondaryUnlocked)
            {
                wasSecondaryUnlocked = true;
                var type = PlayerClassManager.Instance?.SecondaryClass ?? ClassType.None;
                OnSecondaryPicked(type);
            }

            int count = skillController.SlotCount;
            for (int i = 0; i < count; i++)
                UpdateSlot(i, skillController.GetSlot(i));

            HandleDragInput();
        }

        private void UpdateSlot(int i, SkillBase skill)
        {
            var ui = slotUIs[i];
            if (ui.root == null) return;

            if (skill == null)
            {
                ui.icon.color = emptySlotColor;
                ui.icon.sprite = null;
                ui.cdOverlay.fillAmount = 0f;
                ui.border.color = Color.clear;
                ui.cdText.text = "";
                return;
            }

            if (skill.icon != null) { ui.icon.sprite = skill.icon; ui.icon.color = Color.white; }
            else { ui.icon.sprite = null; ui.icon.color = new Color(0.35f, 0.25f, 0.55f, 0.85f); }

            float cdPct = skill.CooldownPercent;
            ui.cdOverlay.fillAmount = cdPct;

            if (cdPct > 0f)
            {
                ui.cdText.text = Mathf.CeilToInt(cdPct * skill.cooldown).ToString();
                ui.border.color = Color.clear;
            }
            else
            {
                ui.cdText.text = "";
                ui.border.color = i >= 4 ? secondaryAccent : readyBorderColor;
            }
        }

        // ─── Drag & Drop ─────────────────────────────────────────

        private void HandleDragInput()
        {
            int activeSlots = skillController != null ? skillController.SlotCount : 4;

            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                if (!isDragging)
                {
                    holdTimer += Time.unscaledDeltaTime;
                    if (holdTimer >= HoldThreshold && dragSourceSlot < 0)
                    {
                        int hovered = GetHoveredSlot(activeSlots);
                        if (hovered >= 0 && skillController.GetSlot(hovered) != null)
                        {
                            dragSourceSlot = hovered;
                            isDragging = true;
                            CreateDragGhost(hovered);
                        }
                    }
                }
                else if (dragGhost != null)
                {
                    dragGhost.transform.position = Mouse.current.position.ReadValue();
                    int hovered = GetHoveredSlot(activeSlots);
                    for (int i = 0; i < activeSlots; i++)
                        slotUIs[i].bg.color = (i == hovered && hovered != dragSourceSlot) ? dragHighlight : bgColor;
                }
            }
            else
            {
                if (isDragging && dragSourceSlot >= 0)
                {
                    int target = GetHoveredSlot(activeSlots);
                    if (target >= 0 && target != dragSourceSlot)
                        skillController.SwapSlots(dragSourceSlot, target);

                    for (int i = 0; i < activeSlots; i++)
                        slotUIs[i].bg.color = bgColor;
                }

                DestroyDragGhost();
                dragSourceSlot = -1;
                isDragging = false;
                holdTimer = 0f;
            }
        }

        private int GetHoveredSlot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (slotUIs[i].root == null) continue;
                var rt = slotUIs[i].root.GetComponent<RectTransform>();
                if (Mouse.current != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(rt, Mouse.current.position.ReadValue(), null))
                    return i;
            }
            return -1;
        }

        private void CreateDragGhost(int slot)
        {
            DestroyDragGhost();
            dragGhost = new GameObject("DragGhost", typeof(RectTransform), typeof(CanvasGroup));
            dragGhost.transform.SetParent(transform.root, false);

            var rt = dragGhost.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(slotSize, slotSize);

            var img = dragGhost.AddComponent<Image>();
            img.color = new Color(readyBorderColor.r, readyBorderColor.g, readyBorderColor.b, 0.6f);
            img.raycastTarget = false;

            var cg = dragGhost.GetComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
            cg.alpha = 0.75f;

            var lbl = new GameObject("Lbl", typeof(RectTransform));
            lbl.transform.SetParent(dragGhost.transform, false);
            var lrt = lbl.GetComponent<RectTransform>();
            lrt.anchorMin = Vector2.zero; lrt.anchorMax = Vector2.one;
            lrt.offsetMin = Vector2.zero; lrt.offsetMax = Vector2.zero;
            var tmp = lbl.AddComponent<TextMeshProUGUI>();
            tmp.text = slot < SlotKeys.Length ? SlotKeys[slot] : "?";
            tmp.fontSize = 20;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;
        }

        private void DestroyDragGhost()
        {
            if (dragGhost != null) Destroy(dragGhost);
            dragGhost = null;
        }

        private static GameObject MakeChild(Transform parent, string name, float w, float h)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            return go;
        }
    }
}
