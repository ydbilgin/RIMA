# DECISION — 3-Screen UI Redesign (MainMenu · Settings-wire · CharacterSelect)
Date: 2026-06-04 · Council: cx (feasibility/reuse) + ax-3.1-Pro (deep design) + ax-3.5-Flash (lean) → Opus synthesis
Brief: STAGING/UI_REDESIGN_3SCREENS_BRIEF_2026-06-04b.md · Advisor outputs: CODEX_DONE.md (cx), _council_q_31pro/_35flash_ui3_redesign.md

## VERDICT IN ONE LINE
Rebuild all 3 screens procedurally, **ZERO new asset generation for v1** (reuse Pack frames + idle_south sprites), static idle showcase + faux-life, and standardize every CanvasScaler to 1920×1080.

---

## RESOLVED DISAGREEMENTS (cx reality-checks settled these)
1. **Center showcase: ANIMATED vs STATIC → STATIC + faux-life (v1).**
   cx reality: animator controllers exist ONLY for the 4 unlocked classes (Warblade/Elementalist/Ranger/Shadowblade); the other 6 have only `idle_south` static sprite; playing an Animator inside a UI Image needs a RenderTexture+Camera rig (heavy). True animation is also gated (needs 6 new PixelLab controllers).
   → DECISION: large static `idle_south` (scale ~3.2x) on pedestal + **unscaled vertical bob + cyan pedestal pulse + selected-class accent glow + brief flash on selection**. Reads "alive", consistent across all 10. **v2 upgrade** = RenderTexture showcase once all 10 controllers exist (GATED, with user).
2. **Columns: 20/55/25 (3.1) vs 20/50/30 (3.5) → 20% / 52% / 28%** (center hero biggest; right wide enough for skill text). Use NORMALIZED anchors, not fixed pixel widths (this is the 4K-collapse fix).
3. **Center backdrop: generate vs reuse → REUSE.** `bg_seal_keep` (dimmed ~0.5) + `pedestal_seal` already exist. No generation v1. Revisit ONLY if play-verify feels cheap.

## AGREEMENTS (all three)
- **Panel treatment ("obsidian glass + etched runes"):** existing 9-slice Pack frames (`card_frame_9slice`/`button_9slice`/`panel_frame_9slice`) → tint void-purple `#100818` @ alpha ~0.62–0.85 + a child sliced image scaled +2px tinted **cyan #00FFCC** as a glow edge-line. Optional `bg_seal_keep` dimmed (~0.3) inside as stone texture. Readability of a panel WITHOUT a flat gray box; "ink-on-paper" comes from **emissive TextMeshPro typography**, not background art.
- **Color discipline:** Cyan `#00FFCC` = RESERVED for interaction states (hover/confirm) + seals/edge-lighting. **Class accent color** used ONLY in selected-roster indicator + right-panel class name + skill titles. Warm-orange `#E89020` = lore whispers ("Yine geldin."). Void-purple = canvas/shadows. "If everything is cyan, nothing is."
- **Typography:** Display (headers/names/buttons) = ALL CAPS, heavy, tracked. Body (lore/skills) = mixed case, ~75-80% opacity so headings lead hierarchy.
- **Reuse-first:** zero generation v1. Skill icons = SkillIconRegistry → PassiveIcon fallback (no bespoke gen until skill data finalized).

---

## SCREEN SPECS

### A) MAIN MENU (`MainMenuController.cs`, scene `Scenes/UI/MainMenu.unity`)
- **FIX SCALER BUG:** MainMenu scene CanvasScaler is **480×270** → change to **1920×1080, MatchWidthOrHeight, match 0.5**. (CharacterSelect is already correct.)
- Keep ruins BG (`main_menu_bg` via `CreateFullScreenBackdrop`). Keep radial vignette.
- **Composition:** move interactive block to **lower-left quadrant**, left-aligned. Right side breathes (BG art).
  - Title **RIMA** large, heavy, widely tracked (top of left column). "THE RIFT HUNTERS" immediately below, muted, extra-tracked.
  - "Yine geldin." = small italic **warm-orange** whisper just above BAŞLA.
  - Buttons BAŞLA / AYARLAR / ÇIKIŞ as a vertical column beside a **thin 2px vertical cyan divider** (fades top/bottom). State: idle = dim white (~0.7); hover = pure cyan + scale 1.08 + a `>` caret (or small seal glyph) reveal to the left; pressed = warm-orange.
  - Version `v1.0` bottom-right, muted.
- Keep Turkish text.

### B) SETTINGS WIRE (in `MainMenuController.cs`)
- `OnSettingsClicked()`: remove the "Yakında." stub (`ShowSettingsOverlay`/`BuildSettingsOverlay`). Instead open the EXISTING fully-functional `SettingsMenuUI` (Gameplay/Accessibility/Audio + **Controls click-to-rebind**, KeyBindManager defaults + JSON persist).
  - Hook: `UIManager.Instance?.OpenSettings()` if present, else `FindFirstObjectByType<SettingsMenuUI>()?.Open()` (SettingsMenuUI auto-creates globally via AutoInit, DontDestroyOnLoad, sortingOrder 1100).
- **Menu-context guards (cx flagged):**
  - `SettingsMenuUI` Aim/Dash gameplay toggles read `Player` (tag lookup) → null in menu. **Hide/disable those 2 rows when no Player exists** (or wrap getters/setters in null-checks — they already no-op, just hide for cleanliness).
  - Ensure `SettingsMenuUI.AutoInit()` CanvasScaler is configured **ScaleWithScreenSize 1920×1080 match 0.5** (currently unconfigured → can collapse at 4K).
  - Prefer `UIManager.OpenSettings()` so RESUME/Close path is consistent (timeScale handled). timeScale=0 in menu is harmless (menu uses no scaled-time anim).
- **Defaults / rebind = ALREADY DONE** (KeyBindManager): Move WASD, Dash Space, Attack LMB, Secondary RMB, RiftBreak V, Skill1-4 Q/E/R/F, CrossEcho C. ESC/TAB reserved. User asked for "default + değiştirilebilir" → satisfied; just expose it.

### C) CHARACTER SELECT (`CharacterSelectScreen.cs`, scene `Scenes/UI/CharacterSelect.unity`) — CENTERPIECE
Keep procedural. Keep `BuildScreen/SelectClass/IsUnlocked/UnlockCost/LockedButtonText/IdentityLockText/LoadCanonicalSprite`. CanvasScaler already 1920×1080 — make COLUMNS use normalized anchors (fix 4K collapse).
- **LEFT (anchor 0.00–0.20) — Class Roster list (10 rows, no scroll needed at 1080p):**
  - Replace `BuildCardGrid` → `BuildRosterList`. Each row (height ~7–8% screen): square **masked portrait** (idle_south via AnchorPath, RectMask2D, preserveAspect, head/torso reads) + class NAME.
  - States: **Locked (6)** = alpha ~0.35, grayscale-ish, padlock glyph + Echo cost hint. **Unlocked idle (3)** = alpha ~0.75, full color. **Selected (1)** = alpha 1.0, 4px left **accent-color** bar, row scale 1.05, accent glow on portrait frame.
- **CENTER (anchor 0.20–0.72) — Selected-class showcase (the hero):**
  - Backdrop: `bg_seal_keep` dimmed (~0.5) cover-cropped. `pedestal_seal` near floor with additive cyan glow. Large `idle_south` (~3.2x, pixel-crisp) standing on pedestal.
  - Faux-life: unscaled vertical bob (small sine), cyan pedestal pulse, accent glow on select, brief cyan/white flash on selection change.
- **RIGHT (anchor 0.72–1.00) — Class details + skills (scrollable):**
  - `BuildRightPanel` → `BuildSkillDetailPanel`. Top fixed identity block: class name (accent), motto/playstyle/resource from `RimaUITheme.ClassIdentity()`.
  - Below: **scrollable** skill list (add a bounded `MakeScrollArea` helper: ScrollRect + Viewport(Image+Mask) + Content(VerticalLayoutGroup + ContentSizeFitter)). Per skill row: 48px icon (SkillIconRegistry → PassiveIcon fallback) + name (accent) + 2-line desc (~75% white).
  - **Data reality (cx):** SkillDatabase has skills only for Warblade/Elementalist/Shadowblade/Ranger. Query `SkillDatabase.Instance.GetAll().Where(s => s.classType == sel && !s.isPassive)`. For the 6 classes WITHOUT data → show identity/resource block + a muted "Yetenekler yakında" note (NO icon generation).
  - Bottom fixed: **SEÇ/CONFIRM** (accent-colored, interactable per unlock) + **GERİ** back button.

---

## ASSET GENERATION LIST (final)
- **v1: ZERO generation.** All reuse (idle_south ×10, bg_seal_keep, pedestal_seal, 9-slice Pack frames, main_menu_bg, SkillIconRegistry + PassiveIcon fallback).
- **Optional later (only if play-verify demands):** 1 CharSelect center backdrop painting; a small skill-icon batch AFTER per-class skill data is finalized for the 6 unbuilt classes.
- **Gated (with user):** the 6 missing class idle animation controllers (PixelLab) → enables v2 animated showcase.

## IMPLEMENTATION ORDER (orchestrator)
1. **cx Task 1 — MainMenu restyle + Settings wire** (one file `MainMenuController.cs` + scaler fix + SettingsMenuUI menu-guards). Compile + play-verify.
2. **cx Task 2 — CharacterSelect rebuild** (the centerpiece). Compile + play-verify.
3. QC each (Opus play-verify D3D11) + user feel-test → commit per verified screen.

## BIGGEST RISK (3.5 Flash) + MITIGATION
Time-sink = animator/icon pipelines for 10 classes. **Mitigation: v1 = static sprites + text skills + reuse only.** Strict layout-first.
