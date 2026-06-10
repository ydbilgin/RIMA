using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// "Rift Runeları" — 6 hexagonal skill slots (LMB, RMB, Q, E, R, F — binding-driven labels).
    /// Ashen Glyph spec: hex mask bg, no border on bg.
    /// States: Ready (normal), Cooldown (30% opacity + clockwise pie), Empty ("—"), Active (1px cyan glow).
    /// Drag-drop reordering: hold LMB on a slot and drag to another to swap skills and keybinds.
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
        private const float SynergyPulseDuration = 0.45f;
        private const float SynergyPulseScale = 0.12f;

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
        private float[] synergyPulseTimers = new float[SlotCount];
        // K1.1: tracks when secondary slots (indices 4-5, Z/X) have been unlocked and revealed
        private bool secondarySlotsRevealed;
        private Color cachedClassAccent = RimaUITheme.ClassAccent(ClassType.Warblade);
        private bool controllersResolved;
        private GameObject cachedPlayer;

        // ── Drag-drop state (shared across all SlotDragHandlers) ────
        /// <summary>The SkillBarUI currently hosting an active drag. Null when idle.</summary>
        internal static SkillBarUI ActiveDragBar;
        /// <summary>Slot index of the drag source. -1 when idle.</summary>
        internal static int ActiveDragIndex = -1;

        private struct SlotUI
        {
            public GameObject root;
            public Image bg;
            public Image rim;         // procedural rim border (tinted to state color)
            public Image icon;
            public Image cdOverlay;
            public Image glowBorder;
            public TextMeshProUGUI keyLabel;
            public TextMeshProUGUI cdTimer;   // optional numeric countdown (5s+ cooldowns)
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
            // K1.1: reveal Z/X slots now that secondary class is unlocked
            RevealSecondarySlots();
        }

        // ─── Build ──────────────────────────────────────────────────

        private void BuildSlots()
        {
            // Drag-drop requires an EventSystem in the scene. Create one if absent.
            if (FindObjectOfType<EventSystem>() == null)
            {
                var esGo = new GameObject("EventSystem");
                esGo.AddComponent<EventSystem>();
                esGo.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
                Debug.Log("[SkillBarUI] EventSystem auto-created for drag-drop support.");
            }

            var container = GetComponent<RectTransform>();

            // Calculate total width: 2 primary + 4 secondary + gaps (6 slots)
            float totalW = 2 * PrimarySize + 4 * SecondarySize + 5 * SlotGap;
            float x = -totalW / 2f;

            // Backing panel: procedural rounded-rect (#040609 @12%, 1px border #1A2540 @30%, r=8).
            // No PNG asset — fully procedural; always looks clean at any scale.
            var backingGo = MakeChild(container, "HUDBacking", totalW + 32f, PrimarySize + 28f);
            var backingImg = backingGo.AddComponent<Image>();
            var backingSprite = RimaUITheme.ProceduralBarBacking;
            if (backingSprite != null)
            {
                backingImg.sprite = backingSprite;
                backingImg.type   = Image.Type.Sliced;
                backingImg.color  = Color.white;
            }
            else
            {
                backingImg.color = new Color(0.016f, 0.024f, 0.035f, 0.14f);
            }
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

                // ── Layer 0: Glow (behind everything, class-accent tinted rarity_glow_common) ──
                var glowGo = MakeChild(slotGo.transform, "Glow", size + 4f, size + 4f);
                var glowImg = glowGo.AddComponent<Image>();
                glowImg.sprite = RimaUITheme.RarityGlowCommon;
                glowImg.type   = Image.Type.Simple;
                glowImg.color  = Color.clear;
                glowImg.raycastTarget = false;
                glowGo.transform.SetAsFirstSibling();

                // ── Layer 1: BG — procedural rounded-rect (#07090F @88%, r=10) ──
                var bgImg = slotGo.AddComponent<Image>();
                var slotBgSprite = RimaUITheme.ProceduralSlotBg;
                if (slotBgSprite != null)
                {
                    bgImg.sprite = slotBgSprite;
                    bgImg.type   = Image.Type.Simple;  // sprite scales to fit; no stretch artifacts
                    bgImg.preserveAspect = false;
                    bgImg.color  = Color.white;
                }
                else
                {
                    bgImg.color = new Color(0.028f, 0.035f, 0.059f, 0.88f);
                }
                bgImg.raycastTarget = false;

                // Drop shadow on the slot
                var bgShadow = slotGo.AddComponent<Shadow>();
                bgShadow.effectColor    = new Color(0f, 0f, 0f, 0.46f);
                bgShadow.effectDistance = new Vector2(2f, -3f);
                bgShadow.useGraphicAlpha = true;

                // ── Layer 2: Rim — thin procedural border ring (tinted per state) ──
                var rimGo  = MakeChild(slotGo.transform, "Rim", size, size);
                var rimImg = rimGo.AddComponent<Image>();
                var rimSprite = RimaUITheme.ProceduralSlotRim;
                if (rimSprite != null)
                {
                    rimImg.sprite = rimSprite;
                    rimImg.type   = Image.Type.Simple;  // transparent-fill border ring, scales fine
                    rimImg.preserveAspect = false;
                    rimImg.color  = new Color(0.110f, 0.157f, 0.208f, 0.35f); // empty state: #1C2535 @35%
                }
                else
                {
                    rimImg.color = new Color(0.110f, 0.157f, 0.208f, 0.35f);
                }
                rimImg.raycastTarget = false;

                // ── Layer 3: Icon (~71% of slot size) ──
                float iconSize = size * 0.71f;
                var iconGo  = MakeChild(slotGo.transform, "Icon", iconSize, iconSize);
                var iconImg = iconGo.AddComponent<Image>();
                iconImg.color = new Color(0.3f, 0.3f, 0.4f, 0.5f); // empty placeholder tint
                iconImg.raycastTarget = false;

                // ── Layer 4: CD overlay (radial clock-wipe, #030510 @86%) ──
                var cdGo  = MakeChild(slotGo.transform, "CD", size * 0.82f, size * 0.82f);
                var cdImg = cdGo.AddComponent<Image>();
                cdImg.color       = new Color(0.012f, 0.020f, 0.063f, 0.86f);
                cdImg.type        = Image.Type.Filled;
                cdImg.fillMethod  = Image.FillMethod.Radial360;
                cdImg.fillOrigin  = (int)Image.Origin360.Top;
                cdImg.fillClockwise = true;
                cdImg.fillAmount  = 0f;
                cdImg.raycastTarget = false;

                // ── Layer 5: CD numeric timer (font 10, cyan, top-center, hidden when not needed) ──
                var cdTimerGo  = MakeChild(slotGo.transform, "CDTimer", size * 0.80f, size * 0.40f);
                var cdTimerTxt = cdTimerGo.AddComponent<TextMeshProUGUI>();
                cdTimerTxt.text      = "";
                cdTimerTxt.fontSize  = 10f;
                cdTimerTxt.fontStyle = FontStyles.Bold;
                cdTimerTxt.color     = new Color(0.28f, 0.89f, 1f, 0.90f); // cyan
                cdTimerTxt.alignment = TextAlignmentOptions.Center;
                cdTimerTxt.outlineColor = new Color(0.016f, 0.024f, 0.051f, 0.90f);
                cdTimerTxt.outlineWidth = 0.28f;
                cdTimerTxt.raycastTarget = false;
                var cdTimerRt  = cdTimerGo.GetComponent<RectTransform>();
                cdTimerRt.anchorMin = new Vector2(0f, 1f);
                cdTimerRt.anchorMax = new Vector2(1f, 1f);
                cdTimerRt.pivot     = new Vector2(0.5f, 1f);
                cdTimerRt.anchoredPosition = new Vector2(0f, -2f);

                // ── Layer 6: Keybind label (lower-right, bold, outlined) ──
                var keyGo  = MakeChild(slotGo.transform, "Key", size * 0.54f, size * 0.32f);
                var keyTxt = keyGo.AddComponent<TextMeshProUGUI>();
                keyTxt.text       = SlotLabel(i);
                keyTxt.fontSize   = i < 2 ? 13f : 10f;
                keyTxt.fontStyle  = FontStyles.Bold;
                keyTxt.color      = new Color(0.86f, 0.92f, 0.96f, 0.92f);
                keyTxt.alignment  = TextAlignmentOptions.BottomRight;
                keyTxt.outlineColor = new Color(0.016f, 0.024f, 0.051f, 0.90f); // #04060D
                keyTxt.outlineWidth = 0.28f;                                      // spec: 0.28f
                keyTxt.raycastTarget = false;
                var keyShadow = keyGo.AddComponent<Shadow>();
                keyShadow.effectColor    = new Color(0f, 0f, 0f, 0.72f);
                keyShadow.effectDistance = new Vector2(1f, -1f);
                keyShadow.useGraphicAlpha = true;
                var keyRt = keyGo.GetComponent<RectTransform>();
                keyRt.anchorMin = new Vector2(1f, 0f);
                keyRt.anchorMax = new Vector2(1f, 0f);
                keyRt.pivot     = new Vector2(1f, 0f);
                keyRt.anchoredPosition = new Vector2(-1f, 1f);

                slots[i] = new SlotUI
                {
                    root       = slotGo,
                    bg         = bgImg,
                    rim        = rimImg,
                    icon       = iconImg,
                    cdOverlay  = cdImg,
                    cdTimer    = cdTimerTxt,
                    glowBorder = glowImg,
                    keyLabel   = keyTxt,
                    size       = size
                };

                // Drag-drop: each slot root needs a Graphic for raycasting and a handler.
                // The bg Image already provides the raycast hit area; ensure raycastTarget = true.
                bgImg.raycastTarget = true;
                AttachDragHandler(slotGo, i);
            }
        }

        // ─── Secondary slot reveal (K1.1 — boss-clear unlock) ──────

        /// <summary>
        /// Called when secondary class is picked: scale-pops the last two slots (R/F = Z/X keybinds)
        /// to draw the player's eye as the unlock draft fills them.
        /// </summary>
        public void RevealSecondarySlots()
        {
            if (secondarySlotsRevealed) return;
            secondarySlotsRevealed = true;
            StartCoroutine(SecondarySlotRevealPop());
        }

        private System.Collections.IEnumerator SecondarySlotRevealPop()
        {
            // Brief scale-pop on slots 4-5 (R/F, now Z/X keybinds) so the player notices them.
            const float popDuration = 0.4f;
            float t = 0f;
            while (t < popDuration)
            {
                t += Time.unscaledDeltaTime;
                float progress = t / popDuration;
                float scale = 1f + 0.3f * Mathf.Sin(progress * Mathf.PI);
                for (int i = 4; i < 6 && i < SlotCount; i++)
                {
                    if (slots[i].root != null)
                        slots[i].root.transform.localScale = Vector3.one * scale;
                }
                yield return null;
            }
            for (int i = 4; i < 6 && i < SlotCount; i++)
                if (slots[i].root != null)
                    slots[i].root.transform.localScale = Vector3.one;

            Debug.Log("[SkillBarUI] Secondary slots (Z/X) pop-revealed.");
        }

        // ─── Update ─────────────────────────────────────────────────

        // BUG-4 FIX: visual bar layout is LMB | RMB | Q | E | R | F.
        // Slots 0-1 (LMB/RMB) are Attack/ClassSecondary actions — they carry no skill component.
        // Slots 2-5 (Q/E/R/F) map to skill-controller slots 0-3 (index offset = 2).
        // Without the offset, skills assigned to Q (controller slot 0) were shown under the LMB
        // label, making the bar appear to mismatch the actual keybindings.
        private const int SkillBarOffset = 2; // first skill-controller slot lives at visual index 2

        private void Update()
        {
            int active = GetActiveSlotCount();
            for (int i = 0; i < SlotCount; i++)
            {
                if (synergyPulseTimers[i] > 0f)
                    synergyPulseTimers[i] = Mathf.Max(0f, synergyPulseTimers[i] - Time.unscaledDeltaTime);

                // Slots 0-1 (LMB/RMB) are attack slots with no skill; always render empty.
                // Slots 2-5 (Q/E/R/F) map to controller skill slots 0-3.
                int ctrlIndex = i - SkillBarOffset;
                if (ctrlIndex >= 0 && ctrlIndex < active)
                    UpdateSlot(i, GetActiveSlot(ctrlIndex));
                else
                    UpdateSlotEmpty(i);
            }
        }

        public void PulseSynergySlot(string skillName)
        {
            if (string.IsNullOrEmpty(skillName)) return;

            int active = GetActiveSlotCount();
            for (int i = SkillBarOffset; i < SlotCount; i++)
            {
                int ctrlIndex = i - SkillBarOffset;
                if (ctrlIndex >= active) break;
                var skill = GetActiveSlot(ctrlIndex);
                if (skill != null && string.Equals(skill.skillName, skillName, System.StringComparison.OrdinalIgnoreCase))
                    synergyPulseTimers[i] = SynergyPulseDuration;
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

            // Icon — BUG-4 (2026-06-10): SkillBase.icon is never serialized on runtime-attached
            // skill components, so the bar showed dark squares while 72 icons sat unused in the
            // registry. Resolve from SkillIconRegistry by skillName (fallback: type name) once
            // and cache the result on the component.
            if (skill.icon == null)
                skill.icon = ResolveIconFromRegistry(skill);

            if (skill.icon != null)
            {
                ui.icon.sprite = skill.icon;
                ui.icon.color = Color.white;
            }
            else
            {
                ui.icon.sprite = null;
                ui.icon.color = new Color(0.18f, 0.20f, 0.24f, 0.85f);
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

                // On cooldown: dim icon to 30%, hide glow, show cold rim
                ui.icon.color = new Color(ui.icon.color.r, ui.icon.color.g, ui.icon.color.b, 0.30f);
                ui.glowBorder.color = Color.clear;                                         // spec: glow hidden on CD
                if (ui.rim != null) ui.rim.color = new Color(0.165f, 0.220f, 0.314f, 0.50f); // #2A3850 @50%

                // CD numeric timer: show when cooldown >= 5 s total (approximate via pct)
                // We show remaining seconds as an integer.  We need skill.CooldownRemaining if available.
                if (ui.cdTimer != null)
                {
                    float remaining = skill.RemainingCooldown;
                    if (remaining >= 5f)
                        ui.cdTimer.text = Mathf.CeilToInt(remaining).ToString();
                    else
                        ui.cdTimer.text = "";
                }
            }
            else
            {
                if (wasOnCooldown[i])
                {
                    readyFlashTimers[i] = ReadyFlashDuration;
                    wasOnCooldown[i] = false;
                }

                if (ui.cdTimer != null) ui.cdTimer.text = "";

                float flashT = readyFlashTimers[i] / ReadyFlashDuration;
                // Ready: glow @50% base, flash up to @95% on first ready frame (0.18s)
                float alpha = Mathf.Lerp(0.50f, 0.95f, flashT);
                ui.glowBorder.color = new Color(cachedClassAccent.r, cachedClassAccent.g, cachedClassAccent.b, alpha);

                // Ready rim: class-accent @65%
                if (ui.rim != null) ui.rim.color = new Color(cachedClassAccent.r, cachedClassAccent.g, cachedClassAccent.b, 0.65f);

                // Ready-flash: brief white flash on the rim itself
                if (flashT > 0.01f && ui.rim != null)
                {
                    Color flashRim = Color.Lerp(
                        new Color(cachedClassAccent.r, cachedClassAccent.g, cachedClassAccent.b, 0.65f),
                        new Color(1f, 1f, 1f, 0.90f),
                        flashT);
                    ui.rim.color = flashRim;
                }
            }

            ApplySynergyPulse(i, ui);
        }

        private void UpdateSlotEmpty(int i)
        {
            var ui = slots[i];
            if (ui.root == null) return;

            ui.icon.sprite = null;
            ui.icon.color = new Color(0.3f, 0.3f, 0.4f, 0.3f);
            ui.cdOverlay.fillAmount = 0f;
            ui.glowBorder.color = Color.clear;
            if (ui.rim != null) ui.rim.color = new Color(0.110f, 0.157f, 0.208f, 0.35f); // #1C2535 @35%
            if (ui.cdTimer != null) ui.cdTimer.text = "";
            if (ui.root != null) ui.root.transform.localScale = Vector3.one;
            synergyPulseTimers[i] = 0f;
            wasOnCooldown[i] = false;
            readyFlashTimers[i] = 0f;
        }

        private void ApplySynergyPulse(int i, SlotUI ui)
        {
            if (ui.root == null || ui.glowBorder == null) return;

            float remaining = synergyPulseTimers[i];
            if (remaining <= 0f)
            {
                ui.root.transform.localScale = Vector3.one;
                return;
            }

            float t = 1f - Mathf.Clamp01(remaining / SynergyPulseDuration);
            float wave = Mathf.Sin(t * Mathf.PI);
            ui.root.transform.localScale = Vector3.one * (1f + SynergyPulseScale * wave);
            Color current = ui.glowBorder.color;
            float alpha = Mathf.Max(current.a, Mathf.Lerp(0.95f, 0.35f, t));
            ui.glowBorder.color = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, alpha);
        }

        // ─── Icon registry (BUG-4) ──────────────────────────────────

        private static SkillIconRegistry iconRegistry;
        private static bool iconRegistryLoadAttempted;

        private static Sprite ResolveIconFromRegistry(SkillBase skill)
        {
            if (!iconRegistryLoadAttempted)
            {
                iconRegistryLoadAttempted = true;
                iconRegistry = Resources.Load<SkillIconRegistry>("SkillIconRegistry");
                if (iconRegistry == null)
                    Debug.LogWarning("[SkillBarUI] SkillIconRegistry not found in Resources; bar icons stay empty.");
            }

            if (iconRegistry == null || skill == null) return null;

            Sprite sprite = iconRegistry.Get(skill.skillName);
            if (sprite == null)
                sprite = iconRegistry.Get(skill.GetType().Name);
            return sprite;
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

        // ─── Drag-drop slot swap ────────────────────────────────────

        /// <summary>
        /// Swap the skills in two visual slot indices (SkillBarOffset-adjusted) and rebuild
        /// controller bindings so the keybinds follow the visual order.
        /// Called by SlotDragHandler on successful drop. Visual indices below SkillBarOffset
        /// (LMB/RMB attack slots) cannot be swapped into skill slots.
        /// </summary>
        public void SwapSlots(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex) return;
            if (fromIndex < 0 || fromIndex >= SlotCount || toIndex < 0 || toIndex >= SlotCount) return;

            // Convert visual indices to controller indices (subtract SkillBarOffset).
            // Ignore swaps that touch the LMB/RMB attack slots (visual index < SkillBarOffset).
            int fromCtrl = fromIndex - SkillBarOffset;
            int toCtrl   = toIndex   - SkillBarOffset;
            if (fromCtrl < 0 || toCtrl < 0) return;

            // Swap in the active controller(s)
            if (UseElementalist())
            {
                elemCtrl.SwapSlots(fromCtrl, toCtrl);
            }
            else if (UseRanger())
            {
                rangerCtrl.SwapSlots(fromCtrl, toCtrl);
            }
            else if (UseShadowblade())
            {
                shadowCtrl.SwapSlots(fromCtrl, toCtrl);
            }
            else if (UseRonin())
            {
                roninCtrl.SwapSlots(fromCtrl, toCtrl);
            }
            else if (warbladeCtrl != null)
            {
                warbladeCtrl.SwapSlots(fromCtrl, toCtrl);
            }

            Debug.Log($"[SkillBarUI] Swapped slots {fromIndex}↔{toIndex}");
        }

        /// <summary>Attaches a SlotDragHandler to a slot root and returns it.</summary>
        private SlotDragHandler AttachDragHandler(GameObject slotRoot, int index)
        {
            var h = slotRoot.AddComponent<SlotDragHandler>();
            h.Init(this, index);
            return h;
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

    /// <summary>
    /// Per-slot drag-drop handler. Attached to each slot root by SkillBarUI.BuildSlots.
    /// Ghost follows the pointer; on drop onto another slot the two skills swap.
    /// </summary>
    internal sealed class SlotDragHandler : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private SkillBarUI bar;
        private int slotIndex;

        // Ghost — a copy of the icon that follows the pointer during drag.
        private GameObject ghost;
        private Canvas rootCanvas;
        private CanvasGroup ghostCG;

        // While dragging, dim this slot's own icon.
        private Image ownIconImage;

        // Highlight overlay shown on hovered target slot.
        private static SlotDragHandler hoveredTarget;
        private Image highlightOverlay;
        private static readonly Color HighlightColor = new Color(0.28f, 0.88f, 1f, 0.35f);

        public void Init(SkillBarUI owner, int index)
        {
            bar = owner;
            slotIndex = index;
        }

        private void Awake()
        {
            // Build a small highlight overlay child (hidden by default).
            var overlayGo = new GameObject("DragHighlight", typeof(RectTransform));
            overlayGo.transform.SetParent(transform, false);
            var rt = overlayGo.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            highlightOverlay = overlayGo.AddComponent<Image>();
            highlightOverlay.color = Color.clear;
            highlightOverlay.raycastTarget = false;
        }

        // ── IPointerEnterHandler / IPointerExitHandler ──────────────

        public void OnPointerEnter(PointerEventData _)
        {
            if (hoveredTarget != null && hoveredTarget != this)
                hoveredTarget.SetHighlight(false);
            hoveredTarget = this;
            // Only highlight if a drag is in progress (from a sibling slot on the same bar).
            if (SkillBarUI.ActiveDragBar == bar && SkillBarUI.ActiveDragIndex != slotIndex)
                SetHighlight(true);
        }

        public void OnPointerExit(PointerEventData _)
        {
            SetHighlight(false);
            if (hoveredTarget == this) hoveredTarget = null;
        }

        private void SetHighlight(bool on)
        {
            if (highlightOverlay != null)
                highlightOverlay.color = on ? HighlightColor : Color.clear;
        }

        // ── IBeginDragHandler ───────────────────────────────────────

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            SkillBarUI.ActiveDragBar = bar;
            SkillBarUI.ActiveDragIndex = slotIndex;

            rootCanvas = GetComponentInParent<Canvas>();

            // Dim this slot's icon.
            ownIconImage = GetComponentsInChildren<Image>(true).Length > 1
                ? GetComponentsInChildren<Image>(true)[1]  // bg=0, icon=1
                : null;
            if (ownIconImage != null)
            {
                var c = ownIconImage.color;
                ownIconImage.color = new Color(c.r, c.g, c.b, 0.3f);
            }

            // Build ghost.
            ghost = new GameObject("DragGhost", typeof(RectTransform));
            ghost.transform.SetParent(rootCanvas != null ? rootCanvas.transform : transform.parent, false);
            var ghostRT = ghost.GetComponent<RectTransform>();
            ghostRT.sizeDelta = GetComponent<RectTransform>().sizeDelta;
            ghostRT.pivot = new Vector2(0.5f, 0.5f);

            ghostCG = ghost.AddComponent<CanvasGroup>();
            ghostCG.alpha = 0.7f;
            ghostCG.blocksRaycasts = false; // let events pass through to drop targets

            var ghostImg = ghost.AddComponent<Image>();
            // Copy the icon sprite from the slot.
            Image iconSrc = GetComponentsInChildren<Image>(true).Length > 1
                ? GetComponentsInChildren<Image>(true)[1] : null;
            if (iconSrc != null)
            {
                ghostImg.sprite = iconSrc.sprite;
                ghostImg.color  = new Color(iconSrc.color.r, iconSrc.color.g, iconSrc.color.b, 0.85f);
            }
            else
            {
                ghostImg.color = new Color(0.28f, 0.88f, 1f, 0.5f);
            }

            UpdateGhostPosition(eventData);
        }

        // ── IDragHandler ────────────────────────────────────────────

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            UpdateGhostPosition(eventData);
        }

        private void UpdateGhostPosition(PointerEventData eventData)
        {
            if (ghost == null || rootCanvas == null) return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootCanvas.transform as RectTransform,
                eventData.position,
                rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : eventData.enterEventCamera,
                out Vector2 localPos);
            (ghost.transform as RectTransform).anchoredPosition = localPos;
        }

        // ── IEndDragHandler ─────────────────────────────────────────

        public void OnEndDrag(PointerEventData eventData)
        {
            // Restore own icon alpha.
            if (ownIconImage != null)
            {
                var c = ownIconImage.color;
                ownIconImage.color = new Color(c.r, c.g, c.b, 1f);
                ownIconImage = null;
            }

            if (ghost != null) { Destroy(ghost); ghost = null; }
            if (hoveredTarget != null) { hoveredTarget.SetHighlight(false); hoveredTarget = null; }
            SkillBarUI.ActiveDragBar = null;
            SkillBarUI.ActiveDragIndex = -1;
        }

        // ── IDropHandler ────────────────────────────────────────────

        public void OnDrop(PointerEventData eventData)
        {
            if (SkillBarUI.ActiveDragBar != bar) return;
            int from = SkillBarUI.ActiveDragIndex;
            if (from < 0 || from == slotIndex) return;

            bar.SwapSlots(from, slotIndex);
            SetHighlight(false);
        }
    }
}
