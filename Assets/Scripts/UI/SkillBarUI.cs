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
        [SerializeField] private Elementalist_SkillController elementalistController;
        [SerializeField] private Ranger_SkillController rangerController;
        [SerializeField] private Shadowblade_SkillController shadowbladeController;

        [Header("Style")]
        [SerializeField] private float slotSize    = 64f;
        [SerializeField] private float spacing     = 10f;
        [SerializeField] private float groupGap    = 24f; // primary / secondary arası boşluk

        private readonly Color bgColor          = new Color(1f, 1f, 1f, 0.95f);
        private readonly Color cdOverlayColor   = new Color(0.05f, 0.05f, 0.12f, 0.75f);
        private readonly Color readyBorderColor = new Color(0.28f, 0.72f, 0.95f, 0.60f);
        private readonly Color keyLabelColor    = new Color(0.85f, 0.88f, 0.95f, 0.95f);
        private readonly Color emptySlotColor   = new Color(0.15f, 0.15f, 0.22f, 0.80f);
        private readonly Color lockedColor      = new Color(0.10f, 0.10f, 0.14f, 0.70f);
        private readonly Color dragHighlight    = new Color(0.45f, 0.75f, 1f,   0.35f);
        private Color secondaryAccent           = new Color(0.6f,  0.25f, 0.9f, 0.60f); // secondary class rengiyle güncellenir

        private static readonly string[] SlotKeys = { "Q", "E", "R", "F", "Z", "X" };

        private SlotUI[] slotUIs = new SlotUI[6];
        private TextMeshProUGUI inputHintLabel;
        private TextMeshProUGUI classLabel;
        private TextMeshProUGUI resourceLabel;
        private PlayerResourceBase activeResource;

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
                if (player != null)
                {
                    skillController = player.GetComponent<Warblade_SkillController>();
                    elementalistController = player.GetComponent<Elementalist_SkillController>();
                    rangerController = player.GetComponent<Ranger_SkillController>();
                    shadowbladeController = player.GetComponent<Shadowblade_SkillController>();
                    activeResource = ResolveResource(player);
                }
            }

            CleanupCombatLabelRemnants();
            BuildSlots();

            if (PlayerClassManager.Instance != null)
            {
                PlayerClassManager.Instance.OnSecondaryClassSelected += OnSecondaryPicked;
                PlayerClassManager.Instance.OnPrimaryClassSet += OnPrimaryClassSet;
            }

            ResolveActiveControllers();
        }

        private void OnDestroy()
        {
            if (PlayerClassManager.Instance != null)
            {
                PlayerClassManager.Instance.OnSecondaryClassSelected -= OnSecondaryPicked;
                PlayerClassManager.Instance.OnPrimaryClassSet -= OnPrimaryClassSet;
            }
        }

        private void OnPrimaryClassSet(ClassType _)
        {
            ResolveActiveControllers();
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
                bgImg.sprite = RimaUITheme.SkillSlotFrame;
                bgImg.preserveAspect = true;
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
                var iconRt = iconGo.GetComponent<RectTransform>();
                iconRt.sizeDelta = new Vector2(slotSize - 18f, slotSize - 18f);

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
                    lockImg.color = new Color(lockedColor.r, lockedColor.g, lockedColor.b, 0.46f);
                    lockImg.raycastTarget = false;

                    // Kilit yazısı
                    var lockTxtGo = MakeChild(lockGo.transform, "LockTxt", slotSize, slotSize);
                    var lockTxt   = lockTxtGo.AddComponent<TextMeshProUGUI>();
                    lockTxt.text      = "LOCK";
                    lockTxt.fontSize  = 11;
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
            ResolveActiveControllers();

            // Event kaçırılmış olabilir — state'i poll et
            if (!wasSecondaryUnlocked && IsSecondaryUnlocked())
            {
                wasSecondaryUnlocked = true;
                var type = PlayerClassManager.Instance?.SecondaryClass ?? ClassType.None;
                OnSecondaryPicked(type);
            }

            int count = GetActiveSlotCount();
            for (int i = 0; i < count; i++)
                UpdateSlot(i, GetActiveSlot(i));

            for (int i = count; i < slotUIs.Length; i++)
                UpdateSlot(i, null);

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
            int activeSlots = GetActiveSlotCount();

            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                if (!isDragging)
                {
                    holdTimer += Time.unscaledDeltaTime;
                    if (holdTimer >= HoldThreshold && dragSourceSlot < 0)
                    {
                        int hovered = GetHoveredSlot(activeSlots);
                        if (hovered >= 0 && GetActiveSlot(hovered) != null)
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
                        SwapActiveSlots(dragSourceSlot, target);

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

        private void CleanupCombatLabelRemnants()
        {
            DestroyChildIfExists("CombatInputHint");
            DestroyChildIfExists("ClassLabel");
            DestroyChildIfExists("ResourceLabel");
        }

        private void DestroyChildIfExists(string childName)
        {
            Transform child = transform.Find(childName);
            if (child == null) return;
            Destroy(child.gameObject);
        }

        private static GameObject MakeChild(Transform parent, string name, float w, float h)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            return go;
        }

        private void ResolveActiveControllers()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            if (skillController == null) skillController = player.GetComponent<Warblade_SkillController>();
            if (elementalistController == null) elementalistController = player.GetComponent<Elementalist_SkillController>();
            if (rangerController == null) rangerController = player.GetComponent<Ranger_SkillController>();
            if (shadowbladeController == null) shadowbladeController = player.GetComponent<Shadowblade_SkillController>();
            activeResource = ResolveResource(player);
        }

        private bool UseElementalistBar()
        {
            return PlayerClassManager.Instance != null &&
                   PlayerClassManager.Instance.PrimaryClass == ClassType.Elementalist &&
                   elementalistController != null &&
                   elementalistController.enabled;
        }

        private bool UseRangerBar()
        {
            return PlayerClassManager.Instance != null &&
                   PlayerClassManager.Instance.PrimaryClass == ClassType.Ranger &&
                   rangerController != null &&
                   rangerController.enabled;
        }

        private bool UseShadowbladeBar()
        {
            return PlayerClassManager.Instance != null &&
                   PlayerClassManager.Instance.PrimaryClass == ClassType.Shadowblade &&
                   shadowbladeController != null &&
                   shadowbladeController.enabled;
        }

        private int GetActiveSlotCount()
        {
            if (UseElementalistBar()) return elementalistController.SlotCount;
            if (UseRangerBar()) return rangerController.SlotCount;
            if (UseShadowbladeBar()) return shadowbladeController.SlotCount;
            return skillController != null ? skillController.SlotCount : 4;
        }

        private bool IsSecondaryUnlocked()
        {
            if (UseElementalistBar()) return false;
            return skillController != null && skillController.SecondaryUnlocked;
        }

        private SkillBase GetActiveSlot(int index)
        {
            if (UseElementalistBar()) return elementalistController.GetSlot(index);
            if (UseRangerBar()) return rangerController.GetSlot(index);
            if (UseShadowbladeBar()) return shadowbladeController.GetSlot(index);
            return skillController != null ? skillController.GetSlot(index) : null;
        }

        private void SwapActiveSlots(int a, int b)
        {
            if (UseElementalistBar()) elementalistController.SwapSlots(a, b);
            else if (UseRangerBar()) rangerController.SwapSlots(a, b);
            else if (UseShadowbladeBar()) shadowbladeController.SwapSlots(a, b);
            else skillController?.SwapSlots(a, b);
        }

        private void BuildCombatLabels()
        {
            var hintGo = MakeChild(transform, "CombatInputHint", 820f, 22f);
            var hintRt = hintGo.GetComponent<RectTransform>();
            hintRt.anchorMin = new Vector2(0.5f, 1f);
            hintRt.anchorMax = new Vector2(0.5f, 1f);
            hintRt.pivot = new Vector2(0.5f, 0f);
            hintRt.anchoredPosition = new Vector2(0f, 10f);
            inputHintLabel = hintGo.AddComponent<TextMeshProUGUI>();
            inputHintLabel.alignment = TextAlignmentOptions.Center;
            inputHintLabel.fontSize = 12;
            inputHintLabel.color = new Color(0.82f, 0.90f, 1f, 0.95f);
            inputHintLabel.raycastTarget = false;

            var classGo = MakeChild(transform, "ClassLabel", 180f, 20f);
            var classRt = classGo.GetComponent<RectTransform>();
            classRt.anchorMin = new Vector2(0f, 1f);
            classRt.anchorMax = new Vector2(0f, 1f);
            classRt.pivot = new Vector2(0f, 0f);
            classRt.anchoredPosition = new Vector2(-230f, 10f);
            classLabel = classGo.AddComponent<TextMeshProUGUI>();
            classLabel.alignment = TextAlignmentOptions.Left;
            classLabel.fontSize = 12;
            classLabel.fontStyle = FontStyles.Bold;
            classLabel.color = new Color(0.55f, 0.82f, 1f, 1f);
            classLabel.raycastTarget = false;

            var resourceGo = MakeChild(transform, "ResourceLabel", 360f, 20f);
            var resourceRt = resourceGo.GetComponent<RectTransform>();
            resourceRt.anchorMin = new Vector2(1f, 1f);
            resourceRt.anchorMax = new Vector2(1f, 1f);
            resourceRt.pivot = new Vector2(1f, 0f);
            resourceRt.anchoredPosition = new Vector2(310f, 10f);
            resourceLabel = resourceGo.AddComponent<TextMeshProUGUI>();
            resourceLabel.alignment = TextAlignmentOptions.Right;
            resourceLabel.fontSize = 12;
            resourceLabel.fontStyle = FontStyles.Bold;
            resourceLabel.color = new Color(0.72f, 0.90f, 1f, 1f);
            resourceLabel.raycastTarget = false;
        }

        private void UpdateCombatLabels()
        {
            ClassType primary = PlayerClassManager.Instance != null
                ? PlayerClassManager.Instance.PrimaryClass
                : ClassType.Warblade;

            if (classLabel != null)
                classLabel.text = primary.ToString().ToUpperInvariant();

            if (inputHintLabel != null)
            {
                inputHintLabel.text = primary switch
                {
                    ClassType.Elementalist => "LMB RIFT BOLT | RMB SWITCH | Q FIREBALL | E GLACIAL SPIKE | R CHAIN LIGHTNING | F BLINK",
                    ClassType.Ranger => "LMB RIFT ARROW | HOLD MARK | RMB ROLL | Q PINNING | E BONE TRAP | R DETONATE | F VOLLEY",
                    ClassType.Shadowblade => "LMB VEIL STRIKE | RMB VEIL FLICKER | Q PHASE STEP | E BACKSTAB MARK | R DEATH MARK | F SHADOW PIN",
                    _ => "LMB IRON COMBO | RMB RAGE OUTLET | Q CHARGE | E GRAVITY | R SUNDER | F EARTHSPLITTER"
                };
            }

            if (resourceLabel != null)
            {
                if (activeResource != null)
                {
                    string extra = "";
                    if (primary == ClassType.Elementalist && elementalistController != null)
                    {
                        extra = $" | {elementalistController.ActiveElement.ToString().ToUpperInvariant()} FIRE {elementalistController.FireState} FROST {elementalistController.FrostState} LIGHT {elementalistController.LightState}";
                    }
                    else if (primary == ClassType.Shadowblade && shadowbladeController != null)
                    {
                        extra = $" | SEVER {shadowbladeController.Sever}/100";
                    }
                    resourceLabel.text = $"{GetResourceName(primary)} {activeResource.Current}/{activeResource.Max}{extra}";
                }
                else
                    resourceLabel.text = "";
            }
        }

        private static string GetResourceName(ClassType type)
        {
            return type switch
            {
                ClassType.Elementalist => "MANA",
                ClassType.Shadowblade => "ENERGY",
                ClassType.Ranger => "FOCUS",
                _ => "RAGE"
            };
        }

        private static PlayerResourceBase ResolveResource(GameObject player)
        {
            ClassType primary = PlayerClassManager.Instance != null
                ? PlayerClassManager.Instance.PrimaryClass
                : ClassType.Warblade;

            return primary switch
            {
                ClassType.Elementalist => player.GetComponent<ManaSystem>(),
                ClassType.Shadowblade => player.GetComponent<EnergySystem>(),
                ClassType.Ranger => player.GetComponent<FocusSystem>(),
                _ => player.GetComponent<RageSystem>()
            };
        }
    }
}
