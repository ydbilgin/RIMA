using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// "Rift Runeları" — 6 hexagonal skill slots (LMB, RMB, Q, E, R, F — binding-driven labels).
    /// Ashen Glyph spec: hex mask bg, no border on bg, no drag-drop.
    /// States: Ready (normal), Cooldown (30% opacity + clockwise pie), Empty ("—"), Active (1px cyan glow).
    /// </summary>
    public class SkillBarUI : MonoBehaviour
    {
        // ── Controller references (resolved at runtime) ─────────────
        private Warblade_SkillController     warbladeCtrl;
        private Elementalist_SkillController elemCtrl;
        private Ranger_SkillController       rangerCtrl;
        private Shadowblade_SkillController  shadowCtrl;
        private RoninController              roninCtrl;

        // ── Layout constants (Ashen Glyph spec) ─────────────────────
        private const float PrimarySize   = 56f;
        private const float SecondarySize = 44f;
        private const float SlotGap       = 8f;
        private const float ReadyFlashDuration = 0.18f;

        // Bar slots → gameplay actions (CONTROL_SCHEME_SYNTHESIS_S6 §7): LMB, RMB, Q, E, R, F.
        private static readonly GameAction[] SlotActions =
        {
            GameAction.Attack, GameAction.ClassSecondary,
            GameAction.Skill1, GameAction.Skill2, GameAction.Skill3, GameAction.Skill4
        };

        // Label shown on a slot — driven by the live binding so rebinds reflect immediately.
        private static string SlotLabel(int i) => KeyBindManager.GetKeyName(SlotActions[i]);

        // ── Slots ───────────────────────────────────────────────────
        private const int SlotCount = 6;
        private SlotUI[] slots = new SlotUI[SlotCount];
        private bool[] wasOnCooldown = new bool[SlotCount];
        private float[] readyFlashTimers = new float[SlotCount];
        private Color cachedClassAccent = RimaUITheme.ClassAccent(ClassType.Warblade);
        private bool controllersResolved;
        private GameObject cachedPlayer;

        private struct SlotUI
        {
            public GameObject root;
            public Image bg;
            public Image icon;
            public Image cdOverlay;
            public Image glowBorder;
            public TextMeshProUGUI keyLabel;
            public float size;
        }

        // ─── Lifecycle ──────────────────────────────────────────────

        private void Start()
        {
            ResolveControllers();
            BuildSlots();
            ScheduleLateResolveControllers();

            if (PlayerClassManager.Instance != null)
            {
                cachedClassAccent = RimaUITheme.ClassAccent(PlayerClassManager.Instance.PrimaryClass);
                PlayerClassManager.Instance.OnPrimaryClassSet += OnPrimaryClassSet;
                PlayerClassManager.Instance.OnSecondaryClassSelected += OnSecondaryPicked;
            }
        }

        // Static-event subscription lives on enable/disable so a disabled bar doesn't linger
        // on KeyBindManager.OnBindingsChanged (cx C1-C3 review, Q3).
        private void OnEnable()
        {
            KeyBindManager.OnBindingsChanged += RefreshKeyLabels;
            RefreshKeyLabels(); // guarded if slots not built yet
        }

        private void OnDisable()
        {
            KeyBindManager.OnBindingsChanged -= RefreshKeyLabels;
        }

        private void OnDestroy()
        {
            if (PlayerClassManager.Instance != null)
            {
                PlayerClassManager.Instance.OnPrimaryClassSet -= OnPrimaryClassSet;
                PlayerClassManager.Instance.OnSecondaryClassSelected -= OnSecondaryPicked;
            }
        }

        // Refresh slot key labels from the registry (called on rebind).
        private void RefreshKeyLabels()
        {
            if (slots == null) return;
            for (int i = 0; i < slots.Length; i++)
                if (slots[i].keyLabel != null) slots[i].keyLabel.text = SlotLabel(i);
        }

        private void OnPrimaryClassSet(ClassType primaryClass)
        {
            cachedClassAccent = RimaUITheme.ClassAccent(primaryClass);
            controllersResolved = false;
            cachedPlayer = null;
            warbladeCtrl = null;
            elemCtrl = null;
            rangerCtrl = null;
            shadowCtrl = null;
            roninCtrl = null;
            ResolveControllers();
            ScheduleLateResolveControllers();
        }

        private void OnSecondaryPicked(ClassType _)
        {
            // Re-resolve to pick up secondary controller
            controllersResolved = false;
            ResolveControllers();
            ScheduleLateResolveControllers();
        }

        // ─── Build ──────────────────────────────────────────────────

        private void BuildSlots()
        {
            var container = GetComponent<RectTransform>();

            // Calculate total width: 2 primary + 4 secondary + gaps (6 slots)
            float totalW = 2 * PrimarySize + 4 * SecondarySize + 5 * SlotGap;
            float x = -totalW / 2f;

            var backingGo = MakeChild(container, "HUDBacking", totalW + 32f, PrimarySize + 28f);
            var backingImg = backingGo.AddComponent<Image>();
            backingImg.sprite = RimaUITheme.SkillBarBacking;
            backingImg.type = RimaUITheme.SkillBarBackingIsSliced ? Image.Type.Sliced : Image.Type.Simple;
            backingImg.color = backingImg.sprite != null ? new Color(1f, 1f, 1f, 0.12f) : new Color(0.03f, 0.04f, 0.08f, 0.14f);
            backingImg.raycastTarget = false;
            backingGo.transform.SetAsFirstSibling();

            for (int i = 0; i < SlotCount; i++)
            {
                float size = i < 2 ? PrimarySize : SecondarySize;

                var slotGo = new GameObject($"Slot_{SlotLabel(i)}", typeof(RectTransform));
                slotGo.transform.SetParent(container, false);
                var rt = slotGo.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(size, size);
                rt.anchoredPosition = new Vector2(x + size / 2f, 0f);
                x += size + SlotGap;

                // Hex background (#0D0D12 at 55% opacity, no border)
                var bgImg = slotGo.AddComponent<Image>();
                bgImg.color = RimaUITheme.SlotHexBg;
                bgImg.raycastTarget = false;
                var bgShadow = slotGo.AddComponent<Shadow>();
                bgShadow.effectColor = new Color(0f, 0f, 0f, 0.46f);
                bgShadow.effectDistance = new Vector2(2f, -3f);
                bgShadow.useGraphicAlpha = true;

                var frameGo = MakeChild(slotGo.transform, "Frame", size, size);
                var frameImg = frameGo.AddComponent<Image>();
                frameImg.sprite = RimaUITheme.SkillSlotFrameAsset;
                frameImg.type = Image.Type.Simple;
                frameImg.color = frameImg.sprite != null ? Color.white : new Color(0.28f, 0.82f, 1f, 0.35f);
                frameImg.raycastTarget = false;

                // Icon (centered, slightly smaller)
                float iconSize = size * 0.72f;
                var iconGo = MakeChild(slotGo.transform, "Icon", iconSize, iconSize);
                var iconImg = iconGo.AddComponent<Image>();
                iconImg.color = new Color(0.3f, 0.3f, 0.4f, 0.5f); // empty
                iconImg.raycastTarget = false;

                // CD overlay (radial fill, clockwise)
                var cdGo = MakeChild(slotGo.transform, "CD", size * 0.82f, size * 0.82f);
                var cdImg = cdGo.AddComponent<Image>();
                cdImg.color = new Color(0.02f, 0.025f, 0.07f, 0.88f);
                cdImg.type = Image.Type.Filled;
                cdImg.fillMethod = Image.FillMethod.Radial360;
                cdImg.fillOrigin = (int)Image.Origin360.Top;
                cdImg.fillClockwise = true;
                cdImg.fillAmount = 0f;
                cdImg.raycastTarget = false;

                // Glow border (class accent, only when ready)
                var glowGo = MakeChild(slotGo.transform, "Glow", size + 2f, size + 2f);
                var glowImg = glowGo.AddComponent<Image>();
                glowImg.sprite = RimaUITheme.SkillSlotFrameAsset;
                glowImg.type = Image.Type.Simple;
                glowImg.color = Color.clear;
                glowImg.raycastTarget = false;
                glowGo.transform.SetAsFirstSibling();

                // Key label (lower-right corner)
                var keyGo = MakeChild(slotGo.transform, "Key", size * 0.54f, size * 0.32f);
                var keyTxt = keyGo.AddComponent<TextMeshProUGUI>();
                keyTxt.text = SlotLabel(i);
                keyTxt.fontSize = i < 2 ? 13f : 10f;
                keyTxt.fontStyle = FontStyles.Bold;
                keyTxt.color = new Color(0.86f, 0.92f, 0.96f, 0.92f);
                keyTxt.alignment = TextAlignmentOptions.BottomRight;
                keyTxt.outlineColor = new Color(0f, 0f, 0f, 0.82f);
                keyTxt.outlineWidth = 0.18f;
                keyTxt.raycastTarget = false;
                var keyShadow = keyGo.AddComponent<Shadow>();
                keyShadow.effectColor = new Color(0f, 0f, 0f, 0.72f);
                keyShadow.effectDistance = new Vector2(1f, -1f);
                keyShadow.useGraphicAlpha = true;
                var keyRt = keyGo.GetComponent<RectTransform>();
                keyRt.anchorMin = new Vector2(1f, 0f);
                keyRt.anchorMax = new Vector2(1f, 0f);
                keyRt.pivot = new Vector2(1f, 0f);
                keyRt.anchoredPosition = new Vector2(-1f, 1f);

                slots[i] = new SlotUI
                {
                    root = slotGo,
                    bg = bgImg,
                    icon = iconImg,
                    cdOverlay = cdImg,
                    glowBorder = glowImg,
                    keyLabel = keyTxt,
                    size = size
                };
            }
        }

        // ─── Update ─────────────────────────────────────────────────

        private void Update()
        {
            int active = GetActiveSlotCount();
            for (int i = 0; i < SlotCount; i++)
            {
                if (i < active)
                    UpdateSlot(i, GetActiveSlot(i));
                else
                    UpdateSlotEmpty(i);
            }
        }

        private void UpdateSlot(int i, SkillBase skill)
        {
            var ui = slots[i];
            if (ui.root == null) return;

            if (skill == null)
            {
                UpdateSlotEmpty(i);
                return;
            }

            // Icon
            if (skill.icon != null)
            {
                ui.icon.sprite = skill.icon;
                ui.icon.color = Color.white;
            }
            else
            {
                ui.icon.sprite = null;
                ui.icon.color = new Color(0.35f, 0.25f, 0.55f, 0.85f);
            }

            // Cooldown
            float cdPct = skill.CooldownPercent;
            ui.cdOverlay.fillAmount = cdPct;
            if (readyFlashTimers[i] > 0f)
                readyFlashTimers[i] = Mathf.Max(0f, readyFlashTimers[i] - Time.deltaTime);

            if (cdPct > 0f)
            {
                wasOnCooldown[i] = true;
                readyFlashTimers[i] = 0f;

                // On cooldown: dim icon
                ui.icon.color = new Color(ui.icon.color.r, ui.icon.color.g, ui.icon.color.b, 0.30f);
                ui.glowBorder.color = new Color(cachedClassAccent.r, cachedClassAccent.g, cachedClassAccent.b, 0.18f);
            }
            else
            {
                if (wasOnCooldown[i])
                {
                    readyFlashTimers[i] = ReadyFlashDuration;
                    wasOnCooldown[i] = false;
                }

                float flashT = readyFlashTimers[i] / ReadyFlashDuration;
                float alpha = Mathf.Lerp(0.5f, 0.95f, flashT);
                ui.glowBorder.color = new Color(cachedClassAccent.r, cachedClassAccent.g, cachedClassAccent.b, alpha);
            }
        }

        private void UpdateSlotEmpty(int i)
        {
            var ui = slots[i];
            if (ui.root == null) return;

            ui.icon.sprite = null;
            ui.icon.color = new Color(0.3f, 0.3f, 0.4f, 0.3f);
            ui.cdOverlay.fillAmount = 0f;
            ui.glowBorder.color = Color.clear;
            wasOnCooldown[i] = false;
            readyFlashTimers[i] = 0f;
        }

        // ─── Controller resolution ──────────────────────────────────

        private void ResolveControllers()
        {
            if (cachedPlayer == null)
                cachedPlayer = GameObject.FindGameObjectWithTag("Player");
            if (cachedPlayer == null) return;

            if (warbladeCtrl == null) warbladeCtrl = cachedPlayer.GetComponent<Warblade_SkillController>();
            if (elemCtrl == null)     elemCtrl     = cachedPlayer.GetComponent<Elementalist_SkillController>();
            if (rangerCtrl == null)   rangerCtrl   = cachedPlayer.GetComponent<Ranger_SkillController>();
            if (shadowCtrl == null)   shadowCtrl   = cachedPlayer.GetComponent<Shadowblade_SkillController>();
            if (roninCtrl == null)    roninCtrl    = cachedPlayer.GetComponent<RoninController>();

            controllersResolved = warbladeCtrl != null || elemCtrl != null ||
                                  rangerCtrl != null || shadowCtrl != null ||
                                  roninCtrl != null;
        }

        private void ScheduleLateResolveControllers()
        {
            if (controllersResolved)
            {
                CancelInvoke(nameof(LateResolveControllers));
                return;
            }

            if (!IsInvoking(nameof(LateResolveControllers)))
                Invoke(nameof(LateResolveControllers), 0.5f);
        }

        private void LateResolveControllers()
        {
            ResolveControllers();
            ScheduleLateResolveControllers();
        }

        private bool UseElementalist() =>
            PlayerClassManager.Instance != null &&
            PlayerClassManager.Instance.PrimaryClass == ClassType.Elementalist &&
            elemCtrl != null && elemCtrl.enabled;

        private bool UseRanger() =>
            PlayerClassManager.Instance != null &&
            PlayerClassManager.Instance.PrimaryClass == ClassType.Ranger &&
            rangerCtrl != null && rangerCtrl.enabled;

        private bool UseShadowblade() =>
            PlayerClassManager.Instance != null &&
            PlayerClassManager.Instance.PrimaryClass == ClassType.Shadowblade &&
            shadowCtrl != null && shadowCtrl.enabled;

        private bool UseRonin() =>
            PlayerClassManager.Instance != null &&
            PlayerClassManager.Instance.PrimaryClass == ClassType.Ronin &&
            roninCtrl != null && roninCtrl.enabled;

        private int GetActiveSlotCount()
        {
            if (UseElementalist()) return Mathf.Min(elemCtrl.SlotCount, SlotCount);
            if (UseRanger())       return Mathf.Min(rangerCtrl.SlotCount, SlotCount);
            if (UseShadowblade())  return Mathf.Min(shadowCtrl.SlotCount, SlotCount);
            if (UseRonin())        return Mathf.Min(roninCtrl.SlotCount, SlotCount);
            return warbladeCtrl != null ? Mathf.Min(warbladeCtrl.SlotCount, SlotCount) : 0;
        }

        private SkillBase GetActiveSlot(int index)
        {
            if (UseElementalist()) return elemCtrl.GetSlot(index);
            if (UseRanger())       return rangerCtrl.GetSlot(index);
            if (UseShadowblade())  return shadowCtrl.GetSlot(index);
            if (UseRonin())        return roninCtrl.GetSlot(index);
            return warbladeCtrl != null ? warbladeCtrl.GetSlot(index) : null;
        }

        // ─── Util ───────────────────────────────────────────────────

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
}
