# Codex Task — Phase B-1 Asset Pack Browser IMPLEMENTATION

**Profile:** auto-selected (laurethayday should win — yasinderyabilgin near limit, laurethgame revoked)
**Effort:** xhigh
**Timeout:** 7200s (2 hours)
**Type:** Editor tool implementation following approved spec

## Context

Phase B-1 spec finalized + approved by orchestrator:
- Spec: `STAGING/SPEC_PHASE_B1_ASSET_PACK_BROWSER.md`
- Skeleton: `Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs.skeleton`
- Audit: `STAGING/CODEX_PHASE_B1_AUDIT.md`
- Design verdict: `STAGING/RIMA_DESIGN_PHASE_B_PRIORITY_VERDICT.md`

User directives (S88_NIGHT):
- "pro şekilde UI/UX designer ve işlevselliğini de eklesene asıl büyük iş o"
- "bana hiçbir şey sormayın, kararı sen ver"
- "tatmin olana kadar yapın, mutually iterate"

Phase B-1 deliverable scope (P0-a + P0-g per design verdict):
- Asset Pack Browser window (read-only first)
- AssetPackManifestSO data backbone
- Category tree + sprite grid + search + pack switcher + hover preview
- Selected sprite inspector (read-only)
- NO placement (B-2)
- NO inspector edits (B-2/B-3)
- NO undo/redo yet (B-3)

## CRITICAL — Unity pre-check + workflow guardrails

Unity OPEN, instance `RIMA@ed023e0b`. Do NOT enter Play mode. Do NOT modify scenes.

Editor tool ONLY — pure Editor assembly + new SO assets. No runtime impact.

## Implementation stages

### Stage 1 — Build AssetPackManifestSO

`Assets/Scripts/MapDesigner/Brush/Data/AssetPackManifestSO.cs`:
- ScriptableObject
- Fields: `string packId`, `string displayName`, `List<PatchAtlasSO> atlases`, `List<PropDefinitionSO> props`, `List<AssetPackCategory> categories`
- `AssetPackCategory` struct: `string categoryName`, `List<string> atlasNames` (string ref, not direct — for hot-swap), `Sprite categoryIcon`
- Renderer-agnostic (NO Unity rendering types in public API — per `[[3d-portability-strategy]]`)

Create 2 sample manifests:
- `Assets/Data/Brush/AssetPacks/RIMA_v2_Pack.asset` — aggregates all 7 v2 PatchAtlasSO (BaseFloor, Moss, Dirt, Pebbles, Cracks, Rift, Ritual)
- `Assets/Data/Brush/AssetPacks/RIMA_v3_Pack.asset` — aggregates 7 v3 PatchAtlasSO (Walls, VerticalProps, BiomeFloor_Mossy/Sandy/Blood/Cave, AtmosphericAccents)

### Stage 2 — AssetPackBrowserWindow implementation

`Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs` (rename .skeleton):

Per spec §2.2 layout:
- Window class: `AssetPackBrowserWindow : EditorWindow`
- Menu: `Tools/RIMA/Map Designer/Asset Pack Browser`
- MinSize 1100×620
- Left panel 260px: search field + pack dropdown + category tree
- Center panel flex: selected sprite preview 256×256 max + checker bg + hover metadata
- Right panel 300px: selected sprite inspector (read-only)
- Bottom panel 220px: sprite grid (thumbnails 48-96px, virtualized scroll)

Behavior:
- On window open: enumerate `AssetDatabase.FindAssets("t:AssetPackManifestSO")` → populate pack dropdown
- Pack select → populate category tree from manifest categories
- Category select → populate sprite grid (atlases + props in that category)
- Sprite hover → preview center + metadata (name, source pack, source atlas, pixel size, PPU, category, default sortingOrder, default blocksMovement)
- Sprite click → select (highlight in grid + populate right inspector)
- Search input → filter grid (case-insensitive substring on sprite name)
- Thumbnail size slider top-right (range 48-96)

### Stage 3 — Tests

`Assets/Tests/EditMode/MapDesigner/AssetPackBrowserTests.cs`:
- 5-8 tests minimum:
  - `OpenWindow_Succeeds_Without_Errors`
  - `Manifest_Enumerates_All_Atlases`
  - `CategorySelect_Populates_SpriteGrid`
  - `Search_Filter_Reduces_Grid_Count`
  - `SpriteSelect_Populates_Inspector`
  - `MetadataReadout_Shows_PPU_And_Category`
  - `ThumbnailSize_Slider_Clamps_48_to_96`
  - `PackSwitch_Resets_Selection_Without_Errors`

### Stage 4 — Verification

- Compile + wait `isCompiling = false`
- Menu item `Tools/RIMA/Map Designer/Asset Pack Browser` opens window
- Window default size 1100×620
- 2 packs visible in dropdown
- Click each category → grid populates
- Hover sprite → preview + metadata appears
- Click sprite → inspector shows metadata
- Search filters grid
- All EditMode tests: target 333 + 8 = **341/341 PASS**

### Stage 5 — Self-iterate (max 3)

If something doesn't work, iterate. Stop when:
- Window opens without errors
- All sprites browse-able
- All tests PASS

### Stage 6 — DONE marker

`STAGING/CODEX_TASK_PHASE_B1_IMPLEMENT_DONE.md`:
- Files created (SO scripts + asset files + window + tests)
- Test count delta (must reach 341+)
- Iterations attempted
- Window screenshot path (use `manage_camera scene_view` after opening window — show it docked)
- Console error count
- Edge cases hit (empty pack? missing sprite? null variant?)
- Visual verdict: "Phase B-1 deliverable acceptable" or "still needs work"

## Constraints

- DO NOT modify SO contract scripts (PatchAtlasSO.cs, PropDefinitionSO.cs, etc.)
- DO NOT extend PatchAtlasSO.PatchRole enum
- DO NOT modify scenes
- DO NOT enter Play mode
- Window class in NAMESPACE `RIMA.MapDesigner.Editor`
- Asset pack SO in NAMESPACE `RIMA.MapDesigner.SO`
- Use `UIElements` (UI Toolkit) if available in project; otherwise IMGUI is acceptable for Phase B-1 (check existing `MapDesignerBrushWindow.cs` for pattern reference)
- Renderer-agnostic SO (no Unity render-stack types in public API)

## NEXT_SIGNAL

DONE → orchestrator reviews window screenshot + verifies tests. If PASS → Phase B-2 (Click-to-Place + auto-collider) implementation dispatch. If FAIL → iterate with specific fixes.
