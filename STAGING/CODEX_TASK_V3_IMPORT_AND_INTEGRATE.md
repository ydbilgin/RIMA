# Codex Task — v3 Materials Import + Vertical Sprite Swap + Lighting

**Profile:** auto-selected (quota-aware)
**Effort:** high
**Timeout:** 3600s (60 min)
**Type:** Unity import + scene composition + Light2D atmospheric setup

## Context

`STAGING/RIMA_AssetParts_v3/sliced/` has 40 sliced PNG ready:
- `walls/wall_01..12.png` (64×64)
- `props/prop_01..08.png` (128×128, vertical props: column intact, column broken, brazier lit, banner torn, statue, chain, debris stack, candelabra)
- `biome_floors/biome_floor_01..16.png` (32×32, 4 biomes × 4 variants)
- `accents/accent_01..04.png` (256×256, portal puddle, ash circle, mossy ruin, obsidian shards)

QC contact sheet: `STAGING/RIMA_AssetParts_v3/qc_contact_sheet_v3.png` — quality verified.

Current PlayableRoom (`Assets/Scenes/Demo/RoomPipelineTest.unity`) has procedural placeholder walls/columns/banner. Goal: replace with real Codex v3 sprites + add atmospheric Light2D for Hades-style mood.

## CRITICAL — Unity instance pre-check

Unity OPEN, instance `RIMA@ed023e0b`, scene `RoomPipelineTest`. Verify via `mcpforunity://instances` + `manage_scene action=get_active` before touching tools.

## Task 1 — Import 40 v3 PNGs

Target paths (mirror sliced/ structure):
- `Assets/Sprites/Environment/RIMA_AssetParts_v3/walls/wall_01..12.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v3/props/prop_01..08.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v3/biome_floors/biome_floor_01..16.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v3/accents/accent_01..04.png`

Importer settings (same as v2):
- `textureType = Sprite`
- `spriteImportMode = Single`
- `filterMode = Point`
- `mipmapEnabled = false`
- `alphaIsTransparency = true`
- `pixelsPerUnit = 32`
- `textureCompression = Uncompressed`
- `spriteExtrude = 1`
- `wrapMode = Clamp` (walls may need Repeat — check seamless behavior)
- `spritePivot = (0.5, 0.0)` for walls and props (bottom-center, for vertical placement)
- `spritePivot = (0.5, 0.5)` for biome_floors and accents (center)

## Task 2 — Create new PatchAtlasSO assets

Output paths:
- `Assets/Data/Brush/AssetParts_v3/Walls.asset` — references wall_01..12 sprites, role=MacroPatch (or new role if enum extended)
- `Assets/Data/Brush/AssetParts_v3/VerticalProps.asset` — references prop_01..08
- `Assets/Data/Brush/AssetParts_v3/BiomeFloor_Mossy.asset` — biome_floor_01..04
- `Assets/Data/Brush/AssetParts_v3/BiomeFloor_Sandy.asset` — biome_floor_05..08
- `Assets/Data/Brush/AssetParts_v3/BiomeFloor_Blood.asset` — biome_floor_09..12
- `Assets/Data/Brush/AssetParts_v3/BiomeFloor_Cave.asset` — biome_floor_13..16
- `Assets/Data/Brush/AssetParts_v3/AtmosphericAccents.asset` — accent_01..04

If `PatchAtlasSO.PatchRole` enum doesn't have proper roles for walls/props, do NOT extend the enum (architecture LOCK) — instead use `MacroPatch` for walls and `Accent` for props as approximation. Document this in DONE marker.

## Task 3 — Scene composition: replace placeholders + add real props

In `RoomPipelineTest.unity` → PlayableRoom hierarchy:

### 3a. Remove Vertical_Placeholders parent

The current `PlayableRoom/Vertical_Placeholders` has 5 procedural walls + 2 columns + 1 banner. Destroy it entirely.

### 3b. Add real walls (north border + east partial)

Create `PlayableRoom/Walls_Real` parent. Place wall sprites:
- North border (full): 8 walls along y=16, x from 14 to 22 (each wall sprite native 64×64 = 2×2 unit, pivot bottom-center)
- Use mix of wall_01 (straight), wall_07 (cracked), wall_02 (worn), randomized
- All sortingOrder = 6
- Tag invisible BoxCollider2D children for collision (optional, only if walls should block)

### 3c. Add real vertical props (intentional placement, NOT random)

Create `PlayableRoom/VerticalProps_Real` parent:
- 2 columns flanking ritual focal: prop_01 (intact) at (15, 11) and prop_02 (broken) at (21, 11), scale 1.0, sortingOrder 7
- 1 brazier lit near focal: prop_03 at (18, 13.5), scale 0.8, sortingOrder 7, attach Light2D point warm orange color (intensity 1.2, radius 5)
- 1 banner torn east of rift: prop_04 at (26, 13), scale 0.9, sortingOrder 7
- 1 kneeling statue in west zone: prop_05 at (10, 14), scale 0.9, sortingOrder 7
- 1 hanging chain east of rift: prop_06 at (28, 14.5), scale 0.8, sortingOrder 7
- 1 candelabra near banner: prop_08 at (28, 11), scale 0.8, sortingOrder 7, attach Light2D point warm yellow (intensity 0.9, radius 3)

### 3d. Add atmospheric accents (1-2 max, focal beats)

Create `PlayableRoom/Decoration/06_AtmosphericAccents`:
- 1 portal puddle accent (accent_01) at (26, 11), scale 0.8, sortingOrder 4, attach Light2D point cool cyan (intensity 1.0, radius 4)
- 1 cursed obsidian cluster (accent_04) at (12, 8), scale 0.6, sortingOrder 4, attach Light2D point cool violet (intensity 0.7, radius 3)

### 3e. Global ambient adjustments

- Reduce `Global Light 2D` intensity to 0.20 (dim baseline for atmosphere)
- Background color: (0.03, 0.025, 0.04) very dark warm

## Task 4 — Compile + Play mode test

1. `manage_editor action=play`
2. Wait 2 sec
3. `manage_camera action=screenshot capture_source=game_view screenshot_file_name=PlayableRoom_v3_real_props.png include_image=true max_resolution=1100`
4. `manage_editor action=stop`
5. Verify console: 0 errors (TestPlayerMovement now uses new Input System after this session's fix)

## Task 5 — Visual gate verdict ROUND 4

Codex looking at the new screenshot — answer in DONE marker:

- Does it now read as a "real game" (Alabaster Dawn / Hades feeling)?
- Vertical depth visible (walls + columns + statue creating fake-3D)?
- Atmospheric lighting working (brazier warm glow + portal cool glow + dim ambient)?
- PASS / PARTIAL / FAIL with reasons

## Task 6 — DONE marker

`STAGING/CODEX_TASK_V3_IMPORT_AND_INTEGRATE_DONE.md`:
- File counts moved + 7 PatchAtlasSO created
- Scene mod summary
- New screenshot path
- Visual gate verdict ROUND 4
- EditMode test count (should remain 333/333)
- Console error count after play mode

## Constraints

- Do NOT modify SO contract scripts (extend approximation if needed but DO NOT edit `PatchAtlasSO.cs`)
- Do NOT modify Phase 1.5 data-first executors
- Use new Input System (legacy Input.GetAxisRaw banned in scripts now per Player Settings)
- Wall sprite pivot bottom-center critical for vertical placement
- Save scene at end

## NEXT_SIGNAL

If PASS: orchestrator commits + sends user the new screenshot. Phase A complete → Phase B (Map Designer UI/UX) begins.
If PARTIAL/FAIL: identify specific failure (vertical not enough? Lighting wrong? Composition still flat?). Orchestrator decides retry vs HD-2D escape.
