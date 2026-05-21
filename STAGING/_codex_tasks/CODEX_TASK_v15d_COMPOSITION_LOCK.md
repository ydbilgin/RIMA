# Codex Task — v15d Composition Budget LOCK Implementation

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Background
3-agent verdict (Opus + Codex + Gemini, 2026-05-18) LOCKED a composition budget for RIMA map design. See `STAGING/CODEX_DONE_BOONA_REVIEW.md` and `STAGING/GEMINI_DONE_BOONA_REVIEW.md` for full rationale.

**Problem this fixes:** v15c maps look chaotic (purple crystals + blue runes + skulls all uniform-scattered). 3-agent consensus = composition discipline missing, not assets missing.

## Goal
Implement composition budget contract in `BlueprintZoneTypeSO` + refactor `AutoPopulator` to two-pass planner + add metrics output to `RimaV15cSceneComposer`. Pass all existing tests + add ~10 new tests for budget enforcement.

## Files to modify

### 1. `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs`
Add these fields (with `[SerializeField]` + sensible defaults + tooltips):

```csharp
[Header("Composition Budget v15d")]
[Range(0f, 0.4f)] public float negativeSpaceRatio = 0.20f;   // 18-22% range typical
public Vector3 floorWeights = new Vector3(0.70f, 0.20f, 0.10f); // dominant/secondary/accent
public BlueprintPoolSO dominantFloorPool;   // L2 base
public BlueprintPoolSO secondaryFloorPool;  // L3 overlay
public BlueprintPoolSO accentFloorPool;     // L3 accent (small %)
public bool pathProtect = false;            // true for combat zones, false for fillers
[Range(1, 6)] public int heroPropClusterCap = 3;
public Vector2Int heroPropClusterSize = new Vector2Int(2, 5);
[Range(1, 4)] public int heroPropClusterBuffer = 2;
[Range(0f, 0.3f)] public float pathCellRatio = 0.15f;
[Range(1, 4)] public int pathMinWidth = 2;
```

Backward compat: if fields are 0/null/default, AutoPopulator falls back to legacy behavior (so existing zone .asset files don't break).

### 2. `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs`
Refactor `PopulatePoolLayer` and related methods into two-pass planner:

**Pass 1 — Reserve:**
- Compute `reservedCells = pathCells ∪ negativeSpaceCells`
- Path cells: connected cells forming primary path (≥pathMinWidth, total ≥pathCellRatio × room cells)
- Negative space cells: random selection from non-path cells, total = negativeSpaceRatio × room cells
- Both are EXCLUDED from L3-L7 placement

**Pass 2 — Place:**
- L2 (base floor): fill all non-reserved cells with dominantFloorPool
- L3 (overlay): place secondary/accent per floorWeights ratio, excluding reserved cells
- L4-L7 (props): cluster-based placement
  - Total clusters across L4-L7 = `heroPropClusterCap` (default 3)
  - Each cluster = random anchor cell + 2-5 nearby props within cluster size
  - 2-cell buffer (no props) around each cluster footprint
  - Tall focal placement participates in cluster cap (not separate `maxTallFocalPerRegion`)

**Critical:** Replace per-cell density rolls. Currently each cell rolls independently → uniform scatter. New code: cluster anchors first, then fill around anchors.

### 3. `Assets/Editor/MapDesigner/Blueprint/RimaV15cSceneComposer.cs`
Add metrics output method. After AutoPopulate run:

```
[v15d Metrics] CombatRoom 36x22 = 792 cells
  Reserved cells: 280 (35.4%) — path: 122 (15.4%), neg space: 158 (19.9%)
  Floor split: dominant 388 (49.0%), secondary 110 (13.9%), accent 55 (6.9%)
  Hero clusters: 3 / 3 cap — sizes: [4, 3, 5] = 12 props
  Layer prop totals: L4=8, L5=6, L6=4, L7=2
  Budget check: ✓ neg space (19.9% in 18-22%), ✓ floor weights, ✓ cluster cap, ✓ path ratio
```

If any budget violated → log WARNING with which constraint failed.

### 4. New EditMode tests in `Assets/Tests/EditMode/MapDesigner/`
~10 tests covering:
- Negative space ratio respected (within 18-22%)
- Floor weights ratio (70/20/10 ± 5%)
- Path cells form connected region ≥pathMinWidth wide
- Path cells ≥pathCellRatio × room cells
- Hero cluster count ≤ heroPropClusterCap
- Each cluster contains 2-5 props
- 2-cell buffer respected (no props within buffer of cluster footprint)
- Backward compat: zone with zero composition fields falls back to legacy behavior
- Metrics output non-null + parseable
- Two-pass planner produces deterministic output for fixed seed

## v15c carry-over (bundle in this task)
Push 22 existing imagegen sprites from `Assets/Data/Brush/AssetParts_v3/` (or wherever they live — check `STAGING/CODEX_TASK_PHASE_A_v15c_LAYER1_LAYER8_IMAGEGEN_DONE.md` for paths) to zone .asset Layer1Sprites + Layer8Sprites arrays for combat biome.

This is necessary so L1/L8 are not empty when the new AutoPopulator runs.

## Acceptance criteria
- All existing tests still PASS (current baseline 392)
- New tests added: ≥10, all PASS
- Total ≥ 402 EditMode PASS
- `Pro_Redesign_v15c_8LayerPainted_CombatRoom` scene re-populated with new logic
- Screenshot saved: `Assets/Screenshots/PlayableRoom_combat_v15d_composition_LIVE.png`
- Metrics output captured to `STAGING/CODEX_DONE_v15d_COMPOSITION_LOCK_metrics.txt`
- No regressions in 3 modified Brush V1 files (BrushLayerOperation, MapDesignerBrushPresetSO, BrushExecutorRouter — these are user-uncommitted, DO NOT touch them)

## DONE marker
Write `STAGING/CODEX_TASK_v15d_COMPOSITION_LOCK_DONE.md` with:
- Files modified (list)
- Test count delta
- Screenshot path
- Metrics output verbatim
- Any deviations from spec + why

## What NOT to do
- No new ScriptableObject classes (extend existing only)
- No shader work (deferred S90+)
- No POI system implementation (cluster cap is the lightweight version)
- Don't touch Brush V1 files (BrushLayerOperation.cs, MapDesignerBrushPresetSO.cs, BrushExecutorRouter.cs)
- Don't refactor unrelated AutoPopulator code
- Don't update CURRENT_STATUS.md (Opus handles)
- Don't commit (Opus + user handle commit hygiene)
