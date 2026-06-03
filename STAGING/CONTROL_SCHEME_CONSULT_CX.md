# CONSULT (code architecture) — RIMA control scheme + input rebinding + HUD bars

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Role
You are the CODE-ARCHITECTURE advisor. **Do NOT write production code.** Read the listed files, then
return an inline written recommendation (architecture + concrete deltas). This is a design consult that will be
synthesized with an Antigravity UX consult and an Opus final decision. RIMA-fit is the #1 criterion.

## Context — current reality (verified by orchestrator, do not re-discover)
RIMA = 2D top-down ARPG roguelite (Hades/Children-of-Morta lineage), URP 2D, PPU 64, WASD move, demo = single
class (Warblade) + 5 rooms + boss. Combat = melee-ranged hybrid per class.

**Input today is split across hardcoded code-created InputActions (NOT the `.inputactions` asset):**
- `Assets/Scripts/Player/PlayerController.cs`:
  - Move = `new InputAction` 2DVector WASD + Gamepad leftStick. Dash = Space + Gamepad South.
  - `enum DashMode { FacingDirection, TowardsMouse }` and `enum CombatAimMode { CharacterFacing, TowardsMouse }`.
  - `AttackAimMode` defaults to **TowardsMouse** (PlayerPrefs `AttackAimMode` + one-time migration
    `AttackAimModeCursorDefault_20260503`). `FaceCombatTarget()` -> `GetMouseDirectionOrFallback()` (ScreenToWorld).
    So **cursor-aim already works** and is the default; ESC menu toggles it.
  - `DashMode` also PlayerPrefs-backed, ESC-toggle.
- `Assets/Scripts/Player/PlayerAttack.cs`: Attack=`<Mouse>/leftButton`+GamepadWest, Secondary=`<Mouse>/rightButton`+GamepadEast,
  RiftBreak=`<Keyboard>/v`. All `new InputAction(...)` in `BuildInputActions()` — **no rebinding, no override save/load.**
- `Assets/InputSystem_Actions.inputactions` EXISTS but is unused by gameplay.
- Skill activation keys: read `Assets/Scripts/UI/SkillBarUI.cs` (self-builds 7 hex slots) +
  `Assets/Scripts/Skills/DraftManager.cs` + any skill-input handler to find how skills 1-7/Q/E/R are bound today.
- HUD bars: `Assets/Scripts/UI/HUDController.cs`, `CharacterHPBar.cs`, `BossHealthBar.cs`, RageSystem (HP + Rage today).
- Settings UI: `Assets/Scripts/UI/SettingsMenuUI.cs` (does it expose aim-mode/dash-mode toggles? does it have any rebind UI?).

## Deliverable (inline written answer, ~1 page, concrete)
1. **Rebinding architecture (main ask):** the cleanest minimal path to add user-rebindable keys given that gameplay
   uses code-created InputActions, not the asset. Recommend ONE: (a) migrate gameplay to a shared
   `InputActionAsset` (the existing `.inputactions`) + PlayerInput + `PerformInteractiveRebinding` + `SaveBindingOverridesAsJson`/PlayerPrefs;
   or (b) keep code-created actions but add a thin rebind+override-persistence layer. State the migration cost
   (which files, how many call-sites change) and the risk. Surgical > grand refactor.
2. **Cursor-aim:** verify cursor-aim is correctly wired end-to-end through the attack behaviors (does
   `FaceCombatTarget()` get called at attack start by the BasicAttackBehavior classes? where?). Flag any gap so the
   "attack toward cursor" feel is reliable. Note hold-to-aim vs snap-on-press.
3. **Skill/ability binding:** what keys should skills 1-7 + dash + secondary + rift-break map to by default, and how
   the rebind layer should treat them (mouse buttons rebindable? reserved keys?).
4. **HUD bars logical layout (code/data view):** given HP + Rage + SkillBar + (future) minimap, recommend the
   canonical on-screen anchoring + which existing scripts own each, and the smallest wiring to make it coherent.
5. **Risks / what NOT to touch.** List anything that would regress the working cursor-aim or combat-facing-lock.

Read the actual files before answering. If a file contradicts the context above, trust the file and say so.
Respond INLINE in your final message (this transcript is captured to CODEX_DONE.md). Do not edit any file.
