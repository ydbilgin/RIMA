# DIAG — Tooltip + Skill Flow (2026-06-13)

Read-only diagnosis for backlog #2 (tooltip) + #3 (skill flow). Change surface only — apply AFTER facing fix lands (avoid Unity compile churn during facing play-test). Sequential.

## ISSUE 1 — Skill bar tooltip (wiring EXISTS, correct on paper)
Hover path is fully implemented: `SlotDragHandler : IPointerEnterHandler` (`SkillBarUI.cs:821-822`, attached `:796-801`) → `OnPointerEnter` → `bar.ShowSlotTooltip` (`:871-874`) → resolves SkillData + lazy `EnsureTooltipSystem` (`:480-484`) → `TooltipSystem.Show` (delay 0.3s, `TooltipSystem.cs:99-104,137-160`). Raycast prereqs met (slot bg `raycastTarget=true` `:332`; GraphicRaycaster+EventSystem auto `RoomRunDirector.cs:1551,1535-1540`).

**Real blockers (most-likely first):**
1. Slots 0,1 (LMB/RMB basic) NEVER tip — `GetSkillDataForVisualSlot` returns null for visual idx < SkillBarOffset(2) (`SkillBarUI.cs:436-439`); empty Q/E/R/F slots null; early-return on empty description (`:425`). → tester likely hovered basic-attack/empty slot = expected-but-unintended "no tooltip".
2. First-hover drop: `TooltipSystem.BuildTooltip` only in `Start()` (`:36-44`); if hover same frame as on-demand AddComponent, `ShowDelayed` guards `tooltipPanel==null → yield break` (`:141`). Subsequent hovers work.
3. Z-order/raycast occlusion — needs in-editor scene check (BLOCKED static).

**Change surface 1:** `SkillBarUI.cs:422-459` (allow idx 0-1 basic-attack tooltip from BasicAttackProfile + relax empty-desc early-return); `TooltipSystem.cs:36-44` (make build lazy/idempotent on first Show). + scene check: exactly one TooltipSystem, no raycast overlay.

## ISSUE 2 — Skill flow
Architecture: **SkillBarUI has NO refresh API — it polls** controller slots every Update (`:383-405`). So "skill not appearing" is ALWAYS upstream (grant/assign), never a missing UI refresh.

### 2a — run-start "first/basic skill" missing
- Runs start EMPTY by design lock (`Warblade_SkillController.EnsureDefaultLoadout` `:71-75`, comment "No hardcoded default skills").
- Basic attack = LMB/RMB **display-only** in bar (`UpdateBasicAttackSlot` `:576-597`, from `Resources/Combat/BasicAttack/BasicAttackProfile_{Class}` `:612`); actual input = `GameAction.Attack` (slot 0), NOT a SkillBase slot. (We confirmed LMB fires → blue SlashArcVFX.)
- First Q skill = opening-kit draft (`RoomRunDirector.OpeningKitDraftSequence` → `DraftManager.ShowOpeningKitDraft` `:230-277,261`). **GAP:** `DraftManager.ClassKits` only Warblade+Elementalist (`:73-77`); other classes fall to `ShowDraft()` (`:296-301`) and timeout `ForcePickFirstOpeningKitSkill` silently `return`s for kit-less classes (`:266`). Matches `DEMO_FINALIZE_DECISION_2026-06-10.md:9`.
- **AMBIGUITY:** "ilk skill yok" = LMB basic-attack OR Q opening-kit? Two different paths. → resolve by in-game test (Warblade has a kit; observe actual run-start state).
- **Change surface 2a:** `DraftManager.cs:73-77` (ClassKits add demo classes) and/or `:262-278` (ForcePickFirstOpeningKitSkill kit-less handling). If it's LMB: check `BasicAttackProfile_{Class}` asset + GameAction.Attack wiring (separate subsystem).

### 2b — 2nd skill not arriving after reward
- `ShowDraftWithSkill` (chest path) MISSING `EnsureDependencies()` + `IsDraftActive` → silent `return` if offerUI null (`DraftManager.cs:372-377`).
- **`AssignActive` silently swallows picks where `skillType==null`** (placeholder/unimplemented): adds SkillData to `currentActiveSkills` (`:663-664`) but binds NO SkillBase component → bar shows nothing (`:642-665`).
- `ChestBehavior.BuildChestOffers` pulls UNFILTERED `GetAll()` incl. `isImplemented=false` placeholders (`ChestBehavior.cs:130-138`), unlike GetPool which filters (`SkillDatabase.cs:580-593`).
- **Change surface 2b:** `DraftManager.AssignActive:642-665` (guard/log skillType==null); `ChestBehavior.cs:130-138` (filtered pool); `DraftManager.cs:372-377` (ShowDraftWithSkill add EnsureDependencies+IsDraftActive).

### 2c — 4-slots-full (already sane UX)
- Current = **replace-mode prompt** (choose 1 of 4 to overwrite, or skip). NOT dropped/errored. `HandleActivePick` count>=maxSlots → `offerUI.ShowReplaceMode` (`DraftManager.cs:480-488`).
- **Edge bug:** count includes secondary slots vs `maxSlots`=primary only (`:480-482`); `OnReplaceChosen` fallback `FindNextPrimarySlot` returns 0 → could clobber slot 0 if FindSlotOf fails (`:495-509,704-729`).
- **Change surface 2c:** `:478-493`, `:495-509`, count/cap mismatch `:480-482`. Mostly verify + edge fix, not redesign.
