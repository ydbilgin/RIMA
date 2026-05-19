# Codex Task — v15e-A: L8 Atmospheric Layer Cap (small chunk)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Background
v15d composition LOCK enforced budget on L4-L7 (hero clusters cap=3) AND floor (70/20/10) AND path (15%, decal-protected) AND negative space (20%). **L8 atmospheric layer is NOT budgeted** — current v15d screenshot shows 67 L8 atmospheric props (mist/particles) covering the whole room, single biggest source of visual noise.

User asked for smaller chunked iterations after seeing the chaos. v15e-A is the smallest fix: add L8 cap.

## Scope (SMALL — single concern)
Add `atmosphericCap` field to `BlueprintZoneTypeSO` + enforce in `AutoPopulator` L8 placement.

## Files to modify

### 1. `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs`
Add ONE field next to existing composition budget fields:
```csharp
[Range(0, 25)]
[Tooltip("Max L8 atmospheric props (mist/particles/scatter). 0=disabled, default 10. Prevents whole-room fog noise.")]
public int atmosphericCap = 10;
```

Backward compat: if 0, behave as legacy (no cap).

### 2. `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs`
In L8 placement path (search for layer 8 / atmospheric / L8 references), add a counter:
- Sum atmosphericCap across all painted zones (room-level total)
- Or per-zone enforcement (preferred — each zone respects its own cap)
- When cap reached, stop L8 placement for that zone

### 3. `Assets/Editor/MapDesigner/Blueprint/RimaV15cSceneComposer.cs`
Update metrics output: add `L8 cap` row to budget check:
```
L8 atmospheric: 9 / 10 cap -- OK
```

### 4. Update zone .asset files
- `Assets/Data/Blueprint/ZoneTypes/zone_stone.asset` → atmosphericCap = 10
- `Assets/Data/Blueprint/ZoneTypes/zone_grass.asset` → atmosphericCap = 8
- Other zones: keep default 10 OR set explicit

### 5. 2 new EditMode tests in `Assets/Tests/EditMode/MapDesigner/Blueprint/AutoPopulatorCompositionBudgetTests.cs`
- atmospheric cap respected per zone
- backward compat (cap=0 = legacy unbounded)

## Acceptance
- 403 existing PASS → 405 PASS (2 new tests added)
- Re-run `RimaV15cSceneComposer.Build()` to render v15e-A screenshot
- New screenshot at `Assets/Screenshots/PlayableRoom_combat_v15e_A_L8cap_LIVE.png`
- New metrics line in `STAGING/CODEX_DONE_v15e_A_L8_CAP_metrics.txt`
- L8 total should drop from 67 → ~10-30 (across all zones in room)

## DONE marker
`STAGING/CODEX_TASK_v15e_A_L8_CAP_DONE.md` with: files modified, test count delta, screenshot path, L8 count before/after.

## What NOT to do
- NO touch to L1-L7 budget logic (already locked v15d)
- NO new ScriptableObject
- NO change to Brush V1 files (user uncommitted)
- NO secondary cluster fix (that's v15e-B, separate task)
- NO path-protect change (that's v15e-C, separate task)
- NO commit
- NO PixelLab calls
- Surgical: only L8 budget logic
