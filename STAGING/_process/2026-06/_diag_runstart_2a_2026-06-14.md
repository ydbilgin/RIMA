# DIAG — Run-start "first skill" ambiguity (P3 / ISSUE 2a) — 2026-06-14

DIAGNOSE ONLY. No code changed. Resolves the ambiguity from
`_diag_tooltip_skillflow_2026-06-13.md` ISSUE 2a ("ilk skill yok" = LMB basic OR Q opening-kit?).

## 1. Demo scope — which classes are actually SELECTABLE?

The live selection flow is the **Attunement Chamber** (`ChamberSelectBootstrap`), not the
classic `CharacterSelectScreen` (the classic screen is hidden and only re-shown as the TAB/Dummy
fallback overlay; `CharacterSelectController` is a separate legacy screen not in the active scene).

Two independent gates BOTH restrict the demo to the same two classes:
- `ChamberSelectBootstrap.IsDemoSelectable(cls)` (`:1972-1975`) returns **true ONLY for Warblade
  and Elementalist**. Other figures show a "— Kilitli" (locked) prompt with NO unlock path
  (`:294-299`), so they can never be entered.
- `ClassUnlockPolicy.IsUnlocked` (`:9-14`) unlocks only Warblade + Elementalist by default;
  others need an Echo unlock — but `IsDemoSelectable` blocks them anyway.

**SELECTABLE in the demo: Warblade, Elementalist. (The other 8 are visible but locked.)**

Crucially, `DraftManager.ClassKits` (`:73-77`) contains kits for **exactly Warblade + Elementalist**
— i.e. the kit roster matches the demo-selectable roster 1:1. The kit-less classes are also the
non-selectable classes, so the "kit-less class has no first skill" gap is **unreachable in the demo**.

## 2. Code state (confirmed by reading)
- `Warblade_SkillController.EnsureDefaultLoadout` (`:71-75`): runs start with an EMPTY 6-slot array,
  no hardcoded defaults (design lock 2026-06-10).
- LMB/RMB basic = display-only in the bar / `GameAction.Attack` input, NOT a SkillBase slot
  (per prior diag; confirmed it fires — blue SlashArc).
- Run-start Q skill = opening-kit draft: `RoomRunDirector.BeginRun` → `OpeningKitDraftSequence`
  (`:204-211, 230-277`) → `DraftManager.ShowOpeningKitDraft` (`:285-334`). 90s timeout
  (`DraftAutoCloseTimeoutSec=90`) then `ForcePickFirstOpeningKitSkill` auto-equips slot 0.
- `ShowOpeningKitDraft`: if `ClassKits.TryGetValue(primary)` FAILS (kit-less primary) it logs
  "No kit defined" and falls through to normal `ShowDraft()` (`:296-301`) — it does NOT leave the
  player empty; it just opens the standard room-1 pool draft.
- `ForcePickFirstOpeningKitSkill` (`:262-278`): for a kit-less primary, `ClassKits.TryGetValue`
  fails and it silently `return`s. This is the only true "silent no-op" — but it ONLY runs on the
  90s timeout AND only matters for a kit-less primary, which is non-selectable in the demo.

## 3. In-game observation (Play mode, full flow MainMenu start)
playModeStartScene=MainMenu (confirmed). Entered Play → MainMenu loaded. Drove the run
programmatically (set class on PlayerClassManager + LoadScene("_Arena") — the same scene the
chamber's StartRun loads) and inspected the resulting draft + slot state. (The manual chamber
walk/G-prompt could not be physically driven via MCP keyboard sim; the scene-load shortcut
exercises the identical RoomRunDirector → DraftManager run-start path.)

| Class | Demo-selectable | Opening-kit Q draft appears at run-start? | After picking card 1 | Ends with N skills |
|-------|-----------------|-------------------------------------------|----------------------|--------------------|
| **Warblade** | YES | YES — `IsDraftActive=true`, SkillOfferUI active, 3 cards = Iron Charge / Gravity Cleave / Earthsplitter (title "ODA 1 — ÖDÜL SEÇ") | Warblade_SkillController slot 0 (Q) = **"Gravity Cleave"**, draft closed | 1 active (Q) + LMB/RMB basic |
| **Elementalist** | YES | YES — `IsDraftActive=true`, SkillOfferUI active, kit cards (Glacial Spike etc.) | Elementalist_SkillController slot 0 (Q) = **"Glacial Spike"**, draft closed | 1 active (Q) + LMB/RMB basic |
| Ranger (kit-less, forced for latent test) | NO | n/a — when forced via static, the scene's PlayerClassManager defaulted Primary→Warblade and showed the Warblade kit draft; a true kit-less primary would fall to normal `ShowDraft()` per code, not an empty start | not applicable | — |

Console: 0 errors throughout. After Play stop: only the known-benign play-EXIT
"Some objects were not cleaned up when closing the scene" teardown warning (already logged as
benign in CURRENT_STATUS.md). Scene `_Arena` `isDirty=false`; static `SelectedClass` reset to None.

## 4. Ambiguity verdict
The "missing first skill" is **(c) by-design empty start that is actually fine for the demo classes.**
- It is NOT (a): LMB basic fires (confirmed previously; not re-broken).
- It is NOT (b) in the demo: the only kit-less-class gap (Q draft not appearing / silent
  force-pick return) requires selecting a kit-less class, which `IsDemoSelectable` makes impossible.
- For BOTH demo-selectable classes (Warblade, Elementalist), the Q opening-kit draft appears at
  run-start and a real skill lands in slot 0 after the pick. The run does NOT start with zero usable
  skills — the player has LMB/RMB basic immediately plus the drafted Q within seconds.

## 5. Recommendation
**No fix needed for the demo.** Both demo-selectable classes get a working opening-kit Q skill.
The run-start "empty by design" is correct and verified end-to-end.

LATENT (post-demo, only if more classes become selectable): the kit-less gap in
`ForcePickFirstOpeningKitSkill` (`:266`, silent return) and the `ShowOpeningKitDraft` fall-through
to a generic draft would need ClassKits entries for the newly-selectable classes. Change surface
IF/WHEN that happens: `DraftManager.cs:73-77` (add ClassKits for each newly demo-selectable class).
Until then, DO NOT touch — adding code for an unreachable path is speculative.
