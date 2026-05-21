# Codex Task — v15g Map Cleanup + Clean Redesign with PixelLab Tiles

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Background
User feedback: "şu an mapde çok kalabalık şeyler var bunları bi kaldıralım. pixellabdakilerle tekrar dizayn edelim bizim mantığımızda temiz şekilde"

Current state (v15e-A LIVE): L8 mist fixed but 7-8 purple crystal scatter remains. Composition not clean enough. User wants RESET + redesign with PixelLab tiles in disciplined v15d composition.

PixelLab assets available NOW:
- `STAGING/pixellab_tiles_pro_pilot/` — 16 cobblestone+path+dirt tiles (v1, segmentation mode, painterly)
- `STAGING/pixellab_dirt_v1/` — 16 dirt earth tiles
- Master reference: `STAGING/RIMA_master_TILES_reference_v3.png`

## Goal
1. CLEAN current scene chaos (disable/remove crowded scatter)
2. Wire PixelLab tiles as PRIMARY foundation in v15f/v15g zone .asset variants (DUPLICATE, don't touch v15d)
3. Apply STRICTER composition budget (75/18/7 floor, cluster cap=2, neg=25%, L8 cap=5)
4. Render `v15g_minimal_clean.png` — minimal pass for visual validation
5. Side-by-side: v15d_chaos vs v15g_minimal

## Files to touch

### 1. Import PixelLab tiles to Unity
`Assets/Data/Brush/AssetParts_v3/CombatBiome_v15g/`:
- Copy from `STAGING/pixellab_tiles_pro_pilot/` (16 tiles) → import as 32×32 sprites
- Copy from `STAGING/pixellab_dirt_v1/` (16 tiles) → import as 32×32 sprites
- Generate Unity .meta files (transparency, pixels-per-unit=32, point filter, no compression)

### 2. New PropPools
`Assets/Data/Blueprint/PropPools/`:
- `pool_v15g_pixellab_dominant.asset` — pixellab cobble tiles (tile_0/2/8/10/14)
- `pool_v15g_pixellab_secondary.asset` — pixellab cobble darker (tile_4/6/12)
- `pool_v15g_pixellab_path.asset` — pixellab path (tile_4/6 of pilot, lighter cream)
- `pool_v15g_pixellab_dirt.asset` — pixellab dirt tiles (all 16)
- `pool_v15g_pixellab_transition.asset` — moss/dirt blend tiles

### 3. New Zone .asset variants
`Assets/Data/Blueprint/ZoneTypes/v15g/`:
- `zone_stone_v15g.asset` — dominantFloorPool → pool_v15g_pixellab_dominant + STRICTER composition
- `zone_path_v15g.asset` — path pool
- `zone_dirt_v15g.asset` — dirt zone (new)
- Set: floorWeights = {0.75, 0.18, 0.07}, heroPropClusterCap=2, atmosphericCap=5, negativeSpaceRatio=0.25, pathProtect=true

### 4. New Profile
`Assets/Data/Blueprint/Profiles/profile_v15g_minimal_pixellab.asset`:
- References v15g zone variants
- AdjacencyRules: use existing rule_grass_stone etc. but with NEW tile-aware decals (skip purple crystals)

### 5. Scene composer variant
Modify `Assets/Editor/MapDesigner/Blueprint/RimaV15cSceneComposer.cs` OR add new `RimaV15gMinimalComposer.cs`:
- ScenePath: `Assets/Scenes/Demo/RoomPipelineTest.unity` (same scene, new root)
- V15gRootName: `Pro_Redesign_v15g_Minimal_PixelLab_CombatRoom`
- Disable v15c/v15d/v15e roots (SetActive false)
- Use profile_v15g_minimal_pixellab
- Screenshot path: `Assets/Screenshots/PlayableRoom_combat_v15g_minimal_pixellab_LIVE.png`
- Metrics output: `STAGING/CODEX_DONE_v15g_metrics.txt`

### 6. EditMode tests (4 new)
- v15g composition budget enforcement
- pixellab tile pool wired correctly
- v15d untouched (rollback intact)
- metric output format

## Acceptance
- 403+4=407 tests PASS
- Screenshot saved with NOTICEABLY cleaner composition (player silhouette pop, fewer scatter elements, smoother tile edges)
- v15d zones/profile UNTOUCHED (rollback safe)
- DONE marker: `STAGING/CODEX_TASK_v15g_map_cleanup_redesign_DONE.md`

## What NOT to do
- No touch to v15d zone .asset files (rollback safety)
- No PixelLab/Codex API calls (tiles already exist)
- No new Brush V1 logic (user uncommitted)
- No animation/character work (parallel separate task)
- Don't commit

## Honest constraints
- Tiles are PIL pilot v1 — quality variable but proven smooth (segmentation mode)
- Wang chain test parallel happening — if completes during this task, prefer Wang tiles for transitions over manual decal placement
- Phase 1.5 chunked renderer NOT in scope (v15g uses standard Tilemap)
