# RIMA Terrain Blend System — Claude Code Task

## Objective
Finalize the shader-based terrain blending system in RIMA to achieve "Colossus: Eternal Blight" quality terrain transitions. The system is already built — your job is to REFINE, TEST, and make it production-ready.

## Visual Reference (CRITICAL — STUDY THESE)
Look at these reference screenshots in `REFERENCES/colossus/`:
- `01_profile_boss_fight.png` — Boss arena with stone→grass transitions, organic cave terrain
- `02_google_terrain_overview.png` — Google Images overview showing multiple environments
- `03_steam_terrain_detail.png` — Steam page detail showing water, grass, stone, path transitions
- `04_rpgfan_environments.png` — RPGFan screenshots showing village, dungeon, and outdoor terrain

### Key Visual Observations from Colossus
1. **NO grid lines** — all terrain transitions are organic and noise-driven
2. **Front-facing chibi sprites** on top-down terrain (same as RIMA's angle)
3. **4+ terrain types** blending seamlessly: grass, stone, dirt path, water edge, moss
4. **Parallax depth** — elevated terrain (cliffs, walls) casts subtle shadows
5. **Decoration layers** — small rocks, flowers, grass tufts scattered on transitions
6. **Color palette** — muted dark fantasy with rich greens, warm browns, cool grays
7. **Tile-to-character ratio** — one stone slab/tile is roughly 2-3x character height

## Terrain Textures — PixelLab Create Tiles PRO

4 seamless terrain textures were generated using PixelLab's "Create Tiles PRO" tool.

### PixelLab Settings (same for all 4)
- **Tool:** Create Tiles PRO (25 generations per run)
- **Tile type:** Square top-down
- **Tile size:** 64px
- **View angle:** 90° (full top-down)
- **Thickness:** 0%
- **Outline mode:** No outline
- **Style tiles:** None

### Prompts Used (in PixelLab Description field, combined as one prompt)
```
1) dark dungeon stone floor, cracked aged stone tiles, gray with subtle brown undertones, dark fantasy style, weathered ancient ruins floor
2) lush green grass ground, top-down grass with individual blade detail, dark green with light green variation, forest floor, dark fantasy style
3) worn dirt path, packed brown earth with small pebbles and stones, warm brown tones, medieval fantasy footpath, well-traveled road
4) mossy overgrown stone ground, dark green moss growing over cracked stone, damp ancient floor, dungeon entrance, dark fantasy atmosphere
```

### Output Files (ALREADY IN PROJECT — DO NOT REGENERATE)
```
Assets/Art/TerrainBlend/stone_floor.png    — Tile 1 (R channel in splat map)
Assets/Art/TerrainBlend/grass_ground.png   — Tile 2 (G channel → reassigned to A)
Assets/Art/TerrainBlend/dirt_path.png      — Tile 3 (B channel → reassigned to G)
Assets/Art/TerrainBlend/moss_ground.png    — Tile 4 (A channel → reassigned to B)
```
Texture import settings already configured: Repeat wrap, Point filter, Uncompressed, no mipmaps, 64x64.

### Correct Shader Slot Assignment (TESTED AND VERIFIED)
```
_TerrainTex0 = stone_floor.png   (R channel = Floor)
_TerrainTex1 = dirt_path.png     (G channel = Path)
_TerrainTex2 = moss_ground.png   (B channel = Moss)
_TerrainTex3 = grass_ground.png  (A channel = Empty/Outer)
```

## Live Test Results (May 18, 2026)

### Verified Working Parameters
| Parameter | Value | Notes |
|-----------|-------|-------|
| `_TerrainTiling` | **14** | Each tile ≈ 2.9 world units. Character = 1 unit. Ratio 2.9:1 (Colossus-like) |
| `_BlendSharpness` | **6** | Sharper terrain boundaries |
| `_NoiseStrength` | **0.15** | Subtle organic edge perturbation |
| `_NoiseScale` | **2** | Noise frequency |

### Character Scale Reference
- **Brawler anchor** (`ANCHORS/characters/02_brawler.png`): 64x64, PPU=64, world size = 1x1 units
- **Sprite assigned in scene**: `Assets/Art/Characters/Brawler/brawler_south.png`
- **Correct ratio**: Character height (1 unit) : tile repeat (2.9 units) ≈ 1:3 — matches Colossus

### Splat Map Workflow
The system uses a **splat map** (RGBA PNG) where each color channel controls terrain placement:
- **Red paint** → stone floor appears
- **Green paint** → dirt path appears
- **Blue paint** → moss appears
- **Alpha/transparent** → grass appears

Users can hand-paint splat maps in any image editor (Paint, Photoshop, Aseprite) for custom room layouts. Current procedural splat map: `Assets/Art/TerrainBlend/generated_splat.png` (128x128).

## Existing System (DO NOT RECREATE — IMPROVE)

### Files Already Implemented
```
Assets/Shaders/TerrainBlend.shader                — URP shader, 4 splat layers + noise
Assets/Scripts/Systems/Map/TerrainBlendConfig.cs   — ScriptableObject config
Assets/Scripts/Systems/Map/TerrainBlendRenderer.cs — Splat map generator + quad mesh
Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs — Toggle: legacy vs shader blend
Assets/Art/TerrainBlend/                           — PixelLab 64px textures + splat map
Assets/Scenes/Demo/ShaderBlend_Test.unity          — Working test scene with Brawler
```

### How the System Works
1. `TerrainBlendRenderer` receives a `ProceduralRoomGenerator.RoomData` with a `vertexGrid`
2. Each vertex has a terrain type (Floor=0, Path=1, Moss=2, Empty=3)
3. Renderer generates an RGBA splat map where each channel = one terrain weight
4. Shader samples 4 terrain textures, multiplies by splat weights, adds noise perturbation
5. Result: single quad mesh, single draw call, organic transitions

## Tasks (Priority Order)

### Task 1: Skip — Textures Done
PixelLab textures are already placed and configured. DO NOT generate procedural textures.

### Task 2: Enhance Shader
Add to `TerrainBlend.shader`:
- **Height-based blending**: Instead of linear blend, use a height map per terrain so transitions look like grass growing through stone cracks
- **Edge darkening**: Subtle shadow at terrain transition boundaries (multiply blend)
- **Per-layer tiling**: Each terrain texture can tile at different rates (stone=larger, grass=smaller detail)

### Task 3: Improve Splat Map Generation
In `TerrainBlendRenderer.cs`, make the splat map generation more organic:
- Add Perlin noise offset to terrain boundaries
- Create island-shaped rooms instead of cross-shaped
- Add scattered "pockets" of secondary terrain (small moss patches in stone areas)
- Edge falloff: terrain fades to black/void at room edges
- Make transitions gradual and noise-perturbed (no sharp channel switches)

### Task 4: Add Decoration Layer
Create a simple scatter system that places small sprite objects on the terrain:
- Pebbles on stone
- Grass tufts on grass edges
- Fallen leaves on path
- Use object pooling, keep under 50 active objects per room

### Task 5: Scene Polish
In `ShaderBlend_Test.unity`:
- Player sprite = Brawler (already assigned)
- Verify WASD movement works (`TestPlayerMovement` component)
- CameraFollow tracks player (already configured)
- Orthographic size = 5-6 (gameplay zoom)
- Add 2-3 point lights with warm/cool tones for atmosphere

## Constraints
- URP pipeline only — no built-in RP shaders
- Single draw call for terrain (no tile GameObjects)
- Mobile-friendly: avoid per-pixel noise in fragment shader hot path (use texture lookup)
- Keep all new scripts in `Assets/Scripts/Systems/Map/` namespace `RIMA.Map`
- Keep all art assets in `Assets/Art/TerrainBlend/`
- `_TerrainTiling` must stay around 14 (verified correct character-to-tile ratio)

## Validation
After changes:
1. Open `ShaderBlend_Test.unity`
2. Enter Play Mode
3. Walk around with WASD
4. Verify: no grid lines, organic transitions, Colossus-like feel
5. Verify: character-to-tile ratio looks correct (character ≈ 1/3 of one tile)
6. Check Profiler: terrain should be 1 draw call

## Character Angle Confirmation
RIMA uses **front-facing "True South" chibi sprites** on **orthographic top-down terrain** — this is the SAME approach as Colossus: Eternal Blight. The character angle is correct.

Current anchors are in `ANCHORS/characters/` (01_warblade through 10_summoner).
Active sprite in scene: `Assets/Art/Characters/Brawler/brawler_south.png`
