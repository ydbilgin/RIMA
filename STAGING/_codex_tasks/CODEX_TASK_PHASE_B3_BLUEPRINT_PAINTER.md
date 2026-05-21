# Phase B-3: Blueprint Painter (Semantic Zone Brush + Auto-Populate)

Status: SPEC_REVISED_POST_CONSULTATION
Date: 2026-05-18
Profile: TBD (cx_dispatch auto-quota — laurethgame fresh 24%)
Effort: xhigh
Timeout: 7200s
Consultation: rima-sonnet critique applied (4 changes + 5 risks + asset gap awareness)

## Context

Phase B-2 LIVE (commit 20e88a6, 351/351 PASS): click-to-place + ghost preview + auto-collider + inspector edits. Browser path: `Tools/RIMA/Map Designer/Asset Pack Browser`. 124 sprite browse-able from RIMA_v2_Pack (84) + RIMA_v3_Pack (40).

**User feedback 2026-05-18 night (LIVE LOCK):** "saçma obj yerleştirme" problemi. Solution = **Blueprint-First Map Design** 3-step process:
1. Semantic zone blueprint (intent map) — paint where path/grass/stone/wall/water/feature should be
2. Rule-based prop placement — each zone draws from its own pool only
3. Adjacency transition decals — organic boundaries between neighboring zones

Reference memory: `memory/feedback_blueprint_first_map_design.md` (read before implementing)

## Goal

Add **semantic intent layer** to Map Designer. User paints 6 zone types with brush, clicks "Auto-Populate" to fill each zone with rule-based props from per-zone pool. "Adjacency Pass" places transition decals at zone boundaries. Result: mathematically impossible to scatter a "barrel in grass" — every prop belongs to its zone's pool.

## Files to ADD

### SO Contracts (`RIMA.MapDesigner.SO` namespace — Phase 1A SO, NOT Brush V1)

- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs`
  - Fields: `string zoneId`, `string displayName`, `Color brushColor`, `BlueprintPropPoolSO propPool`, `[Range(0,1)] float defaultDensity`, `int maxFeatureProps` (only relevant for feature zone, default 99)
  - CreateAssetMenu: `RIMA/MapDesigner/Blueprint/Zone Type`

- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintPropPoolSO.cs`
  - Fields: `string poolId`, `WeightedProp[] entries` where `[Serializable] class WeightedProp { RIMA.MapDesigner.SO.PropDefinitionSO prop; [Min(0)] float weight = 1f; }`
  - Method: `RIMA.MapDesigner.SO.PropDefinitionSO PickWeighted(int seed)` — deterministic weighted pick
  - CreateAssetMenu: `RIMA/MapDesigner/Blueprint/Prop Pool`

- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintAdjacencyRuleSO.cs`
  - Fields: `string ruleId`, `string zoneIdA`, `string zoneIdB`, `BlueprintPropPoolSO transitionPool`, `[Range(0,1)] float density = 0.5f`
  - CreateAssetMenu: `RIMA/MapDesigner/Blueprint/Adjacency Rule`

- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintProfileSO.cs`
  - Fields: `string profileId`, `BlueprintZoneTypeSO[] zones`, `BlueprintAdjacencyRuleSO[] adjacencyRules`, `Vector2Int gridSize = new(36,22)` (PlayableRoom default)
  - Method: `BlueprintZoneTypeSO GetZone(string zoneId)`, `BlueprintAdjacencyRuleSO GetRule(string zoneA, string zoneB)` (commutative — try both orderings)
  - CreateAssetMenu: `RIMA/MapDesigner/Blueprint/Profile`

### Editor (under existing `RIMA.MapDesigner.Editor.asmdef` — DO NOT create new asmdef in Blueprint/ subfolder; Unity directory inheritance covers it)

- **`Assets/Editor/MapDesigner/PropPlacementService.cs`** — **REQUIRED REFACTOR** (Change 4 from consultation):
  - Static class `PropPlacementService`
  - Method: `public static GameObject PlaceEntry(AssetPackEntry entry, Transform parent, Vector3 worldPos, bool registerUndo = true)` — extracted from `AssetPackBrowserWindow.PlaceEntry` (currently private static, inaccessible). Move body verbatim, preserve all behavior (Undo, collider attachment, sorting, name pattern).
  - Method overload: `public static GameObject PlacePropDefinition(RIMA.MapDesigner.SO.PropDefinitionSO prop, Transform parent, Vector3 worldPos, string namePrefix, bool registerUndo = true)` — variant for Blueprint placement that uses namePrefix (e.g., `_BlueprintPlaced_{zoneId}_{tileX}_{tileY}`).
  - Both `AssetPackBrowserWindow.PlaceSelectedAt` AND `AutoPopulator.PopulateZones/PopulateAdjacency` call this service. Zero API surface change to B-2 tests (PlaceEntryForTests still works).

- `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs` — Main window
  - Menu: `Tools/RIMA/Map Designer/Blueprint Painter`
  - Layout:
    - Left panel (200px): zone brush palette (color swatch + name buttons for each zone in active profile) + brush size slider (1-5) + density slider per selected zone
    - Center: 36×22 grid canvas (each cell = 16×16 pixel UI, tinted by painted zone color, hovered cell highlighted)
    - Right panel (200px): Active Profile field (BlueprintProfileSO), Active Room Root field (Transform), seed input (int), "Auto-Populate" button, "Adjacency Pass" button, "Clear Blueprint" button, "Clear Placed Props" button
    - Bottom (40px): tile coord label (mouse hover) + paint count + status text

- `Assets/Editor/MapDesigner/Blueprint/BlueprintCanvas.cs` — Pure C# class (no MonoBehaviour, no ScriptableObject)
  - Field: `Dictionary<Vector2Int, string> intentMap` (Vector2Int tile coord → zoneId string)
  - Methods: `Paint(Vector2Int, string zoneId, int brushSize)`, `Erase(Vector2Int, int brushSize)`, `FloodFill(Vector2Int seed, string zoneId)` **MUST be iterative (BFS/DFS with explicit visited HashSet) — recursive will stack-overflow on 792-cell canvas**, `Clear()`, `IEnumerable<Vector2Int> CellsForZone(string zoneId)`, `IEnumerable<(Vector2Int a, Vector2Int b)> BoundaryEdges()` — returns adjacent cell pairs with different zone IDs
  - Serializable to JSON for future save/load

- `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs` — Logic class (static methods)
  - Method: `public static int PopulateZones(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)` — for each cell, weighted pick from zone's pool, place via `PropPlacementService.PlacePropDefinition`. Returns placed count.
  - Method: `public static int PopulateAdjacency(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot, int seed)` — scan BoundaryEdges, place transition decals per rule. Returns placed count.
  - Method: `public static int ClearPlacedProps(Transform roomRoot)` — destroy all children whose `transform.name.StartsWith("_BlueprintPlaced_")` (name prefix, NOT tag — no custom tag registration). Returns destroyed count.
  - Naming: placed objects named `_BlueprintPlaced_{zoneId}_{tileX}_{tileY}` for re-populate cleanup
  - Special case: feature zone respects `maxFeatureProps` per contiguous region (use iterative flood-fill region detection)

### Tests (under existing `RIMA.Tests.EditMode.asmdef` — NO asmdef modification needed; existing asmdef already references `RIMA.MapDesigner.Editor`)

- `Assets/Tests/EditMode/MapDesigner/Blueprint/BlueprintCanvasTests.cs` (6 tests):
  1. Paint_SingleCell_StoresZoneId
  2. Paint_WithBrushSize3_Stores9Cells (3×3 brush centered)
  3. Erase_RemovesCell
  4. FloodFill_FillsContiguousRegion_Iterative (must complete on 36×22 canvas without stack overflow)
  5. Clear_RemovesAllCells
  6. BoundaryEdges_DetectsZoneBoundaries

- `Assets/Tests/EditMode/MapDesigner/Blueprint/AutoPopulatorTests.cs` (7 tests):
  1. WeightedPick_DeterministicWithSeed (same seed = same pick)
  2. WeightedPick_RespectsWeights (statistical, 1000 samples; weight 3:1 ratio → ~75% pick A)
  3. PopulateZones_PlacesPropsOnlyFromZonePool (asserts placed prop is in zone's pool)
  4. PopulateZones_RespectsDensity (0.5 density → 40-60% cells filled within tolerance)
  5. PopulateZones_FeatureZoneRespectsMaxCap (max 2 features per contiguous region)
  6. PopulateAdjacency_PlacesTransitionAtBoundary
  7. ClearPlacedProps_RemovesOnlyBlueprintTagged (asserts user-authored objects NOT destroyed; uses name prefix check)

**Test scene setup note (Risk #1 from consultation):** Tests that call `PopulateZones` need a scene Transform. Use `new GameObject("TestRoot").transform` in `[SetUp]` and `Object.DestroyImmediate(root.gameObject)` in `[TearDown]`. **DO NOT** rely on Undo system in tests — pass `registerUndo: false` to `PropPlacementService.PlacePropDefinition` from `AutoPopulator` if a test-mode flag is needed. Simpler: AutoPopulator always passes `registerUndo: true`; tests use `new GameObject` parent which works fine with Undo.RegisterCreatedObjectUndo in EditMode.

### Data assets

- `Assets/Data/Blueprint/ZoneTypes/zone_path.asset` (brushColor: tan #C9A875)
- `Assets/Data/Blueprint/ZoneTypes/zone_grass.asset` (brushColor: green #4A8B3A)
- `Assets/Data/Blueprint/ZoneTypes/zone_stone.asset` (brushColor: grey #7A7A7A)
- `Assets/Data/Blueprint/ZoneTypes/zone_wall.asset` (brushColor: dark grey #3A3A3A)
- `Assets/Data/Blueprint/ZoneTypes/zone_water.asset` (brushColor: blue #4A6B8B, defaultDensity: 0.3)
- `Assets/Data/Blueprint/ZoneTypes/zone_feature.asset` (brushColor: gold #D4AF37, maxFeatureProps: 2)

**Pool population strategy (REVISED per consultation):**

Existing 124 sprites live in RIMA_v2_Pack (categories: `BaseFloor`, `Moss`, `Dirt`, `Pebbles`, `Cracks`, `Rift`, `Ritual`) + RIMA_v3_Pack (`Walls`, `VerticalProps`, `BiomeFloor_Mossy`, `BiomeFloor_Sandy`, `BiomeFloor_Blood`, `BiomeFloor_Cave`, `AtmosphericAccents`). **Pool population MUST use category name matching, NOT filename pattern matching.** Map as follows:

- `pool_path.asset` ← `BiomeFloor_Sandy` + `Dirt` (partial fill OK)
- `pool_grass.asset` ← `Moss` + `BiomeFloor_Mossy` (partial fill OK; flora-density gap noted in DONE)
- `pool_stone.asset` ← `Pebbles` + `Cracks` + `BiomeFloor_Cave` (good fill)
- `pool_wall.asset` ← `Walls` + `VerticalProps` (good fill)
- `pool_water.asset` ← EMPTY (no matching category — log warning, list in DONE Asset Gaps; orchestrator dispatches Codex imagegen post-B-3)
- `pool_feature.asset` ← `Ritual` + `Rift` + `AtmosphericAccents` (good fill)

For each pool: scan `Assets/Data/Brush/AssetPacks/RIMA_v2_Pack/{category}/*.prop_*.asset` and `Assets/Data/Brush/AssetPacks/RIMA_v3_Pack/{category}/*.prop_*.asset` (or wherever PropDefinitionSO instances live under each category folder) and add reference to pool's `entries[]` with weight 1.0. If category folder has no `.asset` files, leave pool empty with `// TODO: imagegen prompts -> {category description}` comment in `## Asset Gaps` section of DONE marker.

- `Assets/Data/Blueprint/AdjacencyRules/rule_grass_stone.asset` (transitionPool: pool_grass — placeholder until mossy-stone transition pool is imagegen'd)
- `Assets/Data/Blueprint/AdjacencyRules/rule_path_grass.asset` (transitionPool: pool_path — placeholder)
- `Assets/Data/Blueprint/AdjacencyRules/rule_water_grass.asset` (transitionPool: pool_grass — placeholder)

- `Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset` (all 6 zones + 3 adjacency rules + gridSize 36×22)

## Files to MODIFY

- **`Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs`** — TWO changes:
  1. Replace internal `PlaceEntry` call with `PropPlacementService.PlaceEntry` (private method becomes wrapper, OR remove and call service directly from PlaceSelectedAt). Ensure `PlaceEntryForTests` still works.
  2. Add "Open Blueprint Painter" button in toolbar (right side, after existing controls). Calls `BlueprintPainterWindow.ShowWindow()`. Add `using` statement if needed for Blueprint namespace.

**No other modifications.** PaintMode enum, both PropDefinitionSO paths, Brush V1 — all unaffected.

## Acceptance Criteria

1. **Window opens** via `Tools/RIMA/Map Designer/Blueprint Painter`
2. **6 zone palette** functional — click swatch selects, paint on canvas, cells show colored
3. **Brush size 1-5** works (paints square region centered on cursor cell)
4. **Right-click erase + Shift+click flood fill** work (flood fill iterative, no stack overflow on full 36×22)
5. **"Auto-Populate" button** places props in scene at correct cell positions via `PropPlacementService` (Undo, collider, sorting all preserved)
6. **"Adjacency Pass"** places transition decals at zone boundaries
7. **"Clear Placed Props"** destroys only `_BlueprintPlaced_*` named objects, asserts user-authored objects untouched
8. **Seed determinism** — same seed + same intent map = same prop placement
9. **6+7 = 13 new EditMode tests PASS** (BlueprintCanvasTests + AutoPopulatorTests)
10. **Full EditMode: 364+/364+ PASS** (351 baseline + 13 new)
11. **3 screenshots:**
    - `Assets/Screenshots/phase_b3_blueprint_painted.png` — window with all 6 zones painted on canvas
    - `Assets/Screenshots/phase_b3_auto_populated.png` — scene view after Auto-Populate (props per zone where pool has entries; empty zones acceptable for pool_water)
    - `Assets/Screenshots/phase_b3_adjacency_pass.png` — scene view after Adjacency Pass
12. **Console: 0 errors, 0 warnings** from Blueprint Painter code (asset gap warnings allowed and logged in DONE)
13. **B-2 baseline preserved:** AssetPackBrowserTests 8/8 + AssetPackBrowserPlacementTests 10/10 = 18/18 still PASS after `PropPlacementService` extraction
14. **DONE marker:** `STAGING/CODEX_TASK_PHASE_B3_IMPLEMENT_DONE.md` with full verification report + Asset Gaps section listing missing prop categories per zone (orchestrator will dispatch Codex imagegen post-B-3 to fill gaps and re-screenshot)
15. **Visual self-verdict:** PASS_FOR_ORCHESTRATOR_REVIEW

## Edge cases

- Empty propPool when populating → log warning `[Blueprint] Pool '{poolId}' is empty, skipping zone '{zoneId}'`, don't crash; count as 0 placed; continue with other zones
- IntentMap with 0 cells → "Auto-Populate" no-op, status "No zones painted"
- Adjacency rule missing for pair → skip boundary, log info (not warning)
- Feature zone with single cell → place 1 feature prop max (not 0, not 2)
- Re-populate over existing placement → call `ClearPlacedProps` first automatically (clean idempotent re-run)
- Active Room Root unset → status "Set Active Room Root first" + disable populate buttons
- Active Profile unset → disable all zone palette buttons + status "Set Active Profile first"
- User-authored object whose name happens to start with `_BlueprintPlaced_` → still destroyed (documented risk, low probability)

## Architecture notes

- **IntentMap data structure:** `Dictionary<Vector2Int, string>` runtime; serializable as `List<IntentMapEntry> { Vector2Int pos; string zoneId; }` for future save/load
- **Blueprint placement uses `PropPlacementService`** (extracted from Phase B-2 in this dispatch, see Change 4) — do NOT duplicate placement logic
- **Naming convention** for cleanup: `_BlueprintPlaced_{zoneId}_{tileX}_{tileY}` GameObject name; matching prefix used in `ClearPlacedProps` via `StartsWith("_BlueprintPlaced_")`
- **Brush V1 PaintMode enum unaffected** — Blueprint Painter is intent layer, not terrain layer
- **Atlas/asset agnostic** — PropPool refs `RIMA.MapDesigner.SO.PropDefinitionSO` (Phase 1A SO); asset swap (PixelLab/Codex imagegen) workflow unchanged
- **Grid coord system:** Vector2Int (x, y) where (0,0) = bottom-left, matches Brush V1 + PlayableRoom convention; cell size = **1 integer unit** (PPU 32); paint world position = `roomRoot.position + new Vector3(x + 0.5f, y + 0.5f, 0)` (cell center). **Not sub-pixel snapped — B-2's 1/32 snap is for manual placement only; Blueprint cells are integer-unit aligned**

## Non-goals (defer)

- Save/load IntentMap as separate asset (Phase B-4)
- Multi-room blueprint linking (Phase B-5)
- Realtime auto-populate as user paints (Phase B-5)
- Custom user-created zone types via UI (Phase B-5)
- Path-aware corridor generation (advanced, separate sprint)
- Migrate Combat v14 to Blueprint Painter (orchestrator task v15 post-B-3 LIVE)
- Imagegen for asset gaps (pool_water, mossy-stone transitions) — orchestrator dispatch post-B-3

## Self-iteration mandate

If first implementation pass has:
- < 351 baseline tests still PASS → fix regression first, re-test (especially after PropPlacementService extraction — verify B-2 tests unchanged)
- Any of 13 new tests FAIL → iterate fix locally before declaring DONE
- Compile errors → iterate fix locally
- Console errors (project code, not MCP transport) → iterate fix locally
- Asset gap so severe that screenshot 2 has < 3 props per zone in zones with non-empty pools → iterate to add fallback (try alternative category mappings); if still gap, list in DONE Asset Gaps (orchestrator handles imagegen)
- Empty pool for a zone (e.g., pool_water) → DO NOT iterate; document in DONE Asset Gaps with suggested imagegen prompts

Max 3 internal iterations before writing DONE marker.

## Verification commands (run in order)

1. `manage_editor` set state to EDITOR (force domain reload after asmdef + script changes)
2. `read_console` → expect 0 errors
3. `run_tests` mode EditMode → expect 364+ PASS (351 baseline + 13 new)
4. Verify B-2 baseline: AssetPackBrowserTests 8/8 + AssetPackBrowserPlacementTests 10/10 = 18/18 still PASS
5. Open Blueprint Painter window via menu
6. Paint each of 6 zones in a small region (Codex's choice of layout — recommend 6 horizontal bands)
7. Click Auto-Populate → save scene → screenshot
8. Click Adjacency Pass → save scene → screenshot
9. Write DONE marker

## Verdict format

```
# Phase B-3 Blueprint Painter Implement Done
Status: DONE_FOR_ORCHESTRATOR_REVIEW
Date: 2026-05-18
Executor: Codex {profile}

## Files added
- {list}

## Files modified
- Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs (PropPlacementService refactor + Blueprint Painter button)

## Refactor verification
- AssetPackBrowserTests: X/X PASS (must be 8/8)
- AssetPackBrowserPlacementTests: X/X PASS (must be 10/10)
- B-2 placement behavior preserved: {yes/no with evidence}

## Test count delta
- New BlueprintCanvasTests: X/X PASS (target 6/6)
- New AutoPopulatorTests: X/X PASS (target 7/7)
- Full EditMode: X/X PASS (target 364+/364+)

## Sample screenshots
- {paths}

## Iterations attempted
1. ...

## Asset Gaps (orchestrator dispatch needed for imagegen)
- pool_{zoneId}: empty/sparse — suggested imagegen prompt sketch: "{description}"
- Transition decals for pair {A,B}: missing — suggested prompt: "{description}"

## Console errors
- {none / list}

## Phase B-3 deliverable verdict
{PASS_FOR_ORCHESTRATOR_REVIEW / NEEDS_HELP}
```

---

## Time budget

Consultation estimate: 4-5h realistic, 7200s borderline. If at 5500s Codex senses timeout risk on data asset creation (13 .asset files), prioritize completing PropPlacementService extraction + 3 SO contracts + BlueprintCanvas + AutoPopulator + tests + 1 data asset (profile only) + screenshots. Move remaining 12 data asset files to follow-up `STAGING/CODEX_TASK_PHASE_B3_FOLLOWUP_DATA_ASSETS.md` and note in DONE. Better to ship a working framework with minimal data than fail timeout with half-baked code.
