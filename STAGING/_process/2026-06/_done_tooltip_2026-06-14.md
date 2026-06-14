# DONE — Skill Bar Tooltip (P2) 2026-06-14

VERDICT: DONE. Console: 0 compile errors.

## Files changed (surgical, scope-respected)
- `Assets/Scripts/UI/SkillBarUI.cs` — FIX A
- `Assets/Scripts/UI/TooltipSystem.cs` — FIX B
- READ only: `Assets/Scripts/Combat/BasicAttack/BasicAttackProfile.cs`

## FIX A — Basic-attack slots (visual idx 0,1 = LMB/RMB) now tip
`ShowSlotTooltip` (SkillBarUI.cs ~:422) now branches on `visualIndex < SkillBarOffset`:
- For basic-attack slots it calls a new private `BuildBasicAttackTooltip(visualIndex)` that sources
  text from `GetActiveBasicAttackProfile()` (the same profile the bar already loads from
  `Resources/Combat/BasicAttack/BasicAttackProfile_{Class}`).
- BasicAttackProfile has NO description field — usable text fields are `lmbName`/`rmbName`,
  `comboDamage[]`, `projectileDamage`, `rmbDamage`, `rmbCost`, `rmbCooldown`. Tooltip built from
  name + damage (+ cost/CD for RMB). Turkish labels: "Temel Saldırı" / "Temel Yetenek" / "Hasar" /
  "Maliyet" / "CD".
- Relaxed the skill-slot early-return: was `string.IsNullOrWhiteSpace(data.description)` →
  now `string.IsNullOrWhiteSpace(data.skillName)`. A skill slot (idx>=2) with a name but no
  description still tips. `FormatSkill` is unchanged, so existing rich skill tooltips are intact.

### FIX A proof (execute_code reflection, current class = Warblade)
```
CurrentClass=Warblade
BuildBasicAttackTooltip found=True
--- slot 0 nonNull=True ---  <b>Cleave</b> Temel Saldırı / Hasar: 25
--- slot 1 nonNull=True ---  <b>War Stomp</b> Temel Yetenek / Hasar: 34 / Maliyet: 30 / CD: 1,5s
```

## FIX B — First-hover drop fixed (lazy + idempotent build)
TooltipSystem.cs:
- Added `private bool built;`
- Added `EnsureBuilt()` — guards `if (built && tooltipPanel != null) return;` else `BuildTooltip()`.
- `BuildTooltip()` sets `built = true` after canvas resolves.
- `Start()` now calls `EnsureBuilt()` (was `BuildTooltip()`) — no double build.
- `Show()` calls `EnsureBuilt()` at the top so the first Show always has a ready panel even when
  the component was AddComponent'd the same frame and Start() has not run yet.

### FIX B proof (execute_code, edit mode where Start never runs)
```
Before Show: panel=null built=False        (Start did NOT run)
After 1st Show: panel=Tooltip built=True    (first Show built it)
After 2x EnsureBuilt: samePanelRef=True     (idempotent, no duplicate)
Tooltip panel count in scene=0 (post-cleanup)
```

## SCENE CHECK
Gameplay scene = `_Arena` (has RoomRunDirector). SkillBarUI + TooltipSystem are BUILT AT RUNTIME
(not present in edit mode), so a populated-state count must come from runtime.
- TooltipSystem duplication is structurally impossible: `Awake()` singleton guard destroys
  duplicates, and all 4 AddComponent call sites (SkillBarUI, SkillOfferUI, SkillCodexUI,
  CharacterSelectScreen) are `if (TooltipSystem.Instance != null) return;` guarded. → max ONE.
- Occluder scan of `_Arena` edit-mode hierarchy: **0 full-screen raycast-blocking Graphics.**
  (Brief Play landed on MainMenu/SettingsMenuUI per playModeStartScene rule — its full-screen
  `[SettingsMenuUI]/Overlay` is a menu backdrop in a different scene, NOT over the gameplay skill
  bar; the actual run-room state wasn't reachable without a full manual menu flow.)
- No clear duplicate or occluder in the gameplay scene → nothing to fix here (report-only, per task).

## State discipline
- Entered Play for scene check → STOPPED play; editor left in clean edit-mode.
- Removed a stray `SkillDatabase_ProofTmp` root left in `_Arena` (from a prior proof run) plus my
  own temp test objects via DestroyImmediate. Active scene isDirty=False (no save).
- Console: 0 CS compile errors. One benign play-teardown notice ("Some objects were not cleaned
  up when closing the scene") unrelated to these changes.

## Residual risk
- Real in-room hover not exercised live (run-room state needs full menu flow). Wiring + data both
  proven by reflection; raycast prereqs unchanged. Low risk.
- The description-guard relaxation means an idx>=2 skill with empty description now shows a
  name+tier tooltip with a blank body line — intended per task, cosmetically thin but not broken.
