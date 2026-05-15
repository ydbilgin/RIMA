# PixelLab L4 / L5 / L6 Batch — Karar #143 Sprint 3 (post-L3)

**Type:** PixelLab MCP `create_object` batch prompt set
**Date:** 2026-05-16 (S85)
**Status:** Ready for dispatch AFTER L3 wall batch (pixellab_l3_wall_batch.md) is QC PASS
**Estimated credits:** ~15 credits (5 calls × n=16 + selection)
**Total Sprint 3 = L3 + L4/L5/L6 = 29 sprites, ~36 credits combined**

---

## 0. Production Order

This batch dispatches AFTER L3 wall batch is locked. Reason: wall style sets the painterly grain language for the whole biome. L4/L5/L6 decals must visually match the wall stone aesthetic — generating them before walls risks style drift.

**Order:**
1. L3 wall batch (pixellab_l3_wall_batch.md) — Sprite 1-7 — LOCK FIRST
2. L4 transition (this file) — 6 sprites
3. L5 detail (this file) — 6 sprites
4. L6 accent (this file) — 3 sprites

---

## 1. Locked Style Direction (shared with L3)

**Same palette LOCK as L3:**
- `#1A1C20` deep shadow
- `#2A2D34` dark stone
- `#3A3D48` mid stone
- `#4E5260` highlight
- `#606575` chip light
- `#7BA7BC` ice-blue mineral accent (subtle, sparse)

**Critical differences from L3 walls:**
- L4/L5/L6 are FLOOR-LAYER decals (sit ON TOP of L1/L2 tilemap)
- Transparency is allowed (and required) on outer edges
- IRREGULAR organic silhouette MANDATORY — NO grid, NO rectangle
- ChatGPT §17 LOCK: **NO Gaussian blur**. Softness from pixel cluster breakup + irregular shape.

**Universal shared prompt header (paste into every L4/L5/L6 prompt below):**

> Hades-style painter pixel art floor decal for a shattered keep dungeon. Top-down ~30-35 degree high overhead view. Painterly large brush strokes, ORGANIC IRREGULAR silhouette — NO grid lines, NO repeating block pattern, NO uniform shape. Decal sits on top of stone floor; outer edges fade with crisp pixel breakup (not Gaussian blur, not smooth gradient). Palette: #1A1C20 deep shadow / #2A2D34 dark stone / #3A3D48 mid / #4E5260 highlight / #606575 chip / #7BA7BC ice-blue (sparingly). Match style_image reference for stone grain and brushwork scale.

**Universal negative prompt:**

```
avoid: Gaussian blur, smooth gradient fade, bright colors, modern UI chrome, repeating tile pattern,
uniform rectangular shape, anti-aliased rounded curves, cartoon outline, warm brown palette,
glowing magic effect, fantasy hero element, character, weapon, ivy carpet, lush vegetation,
grass overgrowth, photorealistic detail, 3D rendered shading
```

**Style reference image (path for every MCP call):**
`Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png`

---

## 2. L4 — Transition Brush Sprites (6 total)

L4 transition decals are LARGE oval organic patches placed on floor cells to:
- Hide tile-grid repetition
- Suggest biome erosion (moss creeping over stone)
- Bridge between cleaner floor and walls

### 2.1 Moss Patch Oval — 3 variations

**Canvas:** 256 × 256 px
**Variants:** 3
**Strategy:** Single n=16 call, pick 3 best

**MCP call:**
```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Hades-style painter pixel art moss patch decal for shattered keep floor, 256x256 pixels, top-down 30-35 degree overhead view. ORGANIC IRREGULAR OVAL shape with crisp pixel-broken edges — NO smooth curves, NO Gaussian blur, NO grid alignment. Patch is dark green-grey moss tones blended into the locked stone palette: deep moss shadow #1A1C20, moss body using muted #2A2D34 and #3A3D48 with a slight green tint, highlight edges #4E5260. Subtle ice-blue mineral fleck #7BA7BC at one or two crack edges (sparse, NOT dominant). Outer silhouette is jagged irregular pixel cluster fade — small isolated pixel clusters break off the main shape like organic erosion. Sits ON TOP of floor as semi-transparent overlay; only the moss body is opaque; surrounding pixels are fully transparent.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [256, 256],
    "n": 16,
    "style_strength": 0.65,
    "negative_prompt": "Gaussian blur, smooth gradient fade, bright green, lush vegetation, ivy, grass overgrowth, modern UI, repeating tile pattern, uniform oval, anti-aliased curves, cartoon outline, warm brown, photorealistic, character, weapon"
  }
}
```

**Selection criteria (pick 3 from 16):**
1. Outer silhouette is pixel-broken irregular (NOT smooth oval)
2. Pixel clusters break off the main body (organic erosion look)
3. Palette stays in locked hex range (no bright green)
4. Subtle ice-blue mineral fleck present, not dominant
5. Body opaque, surroundings transparent
6. 3 picks should differ in silhouette: (a) elongated, (b) compact round, (c) irregular blob

**Output paths:**
- `STAGING/TILESET_OUTPUT/L4_Transition/moss_oval_01.png`
- `STAGING/TILESET_OUTPUT/L4_Transition/moss_oval_02.png`
- `STAGING/TILESET_OUTPUT/L4_Transition/moss_oval_03.png`

### 2.2 Dirt Patch Oval — 3 variations

**Canvas:** 256 × 256 px
**Variants:** 3

**MCP call:**
```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Hades-style painter pixel art dirt patch decal for shattered keep floor, 256x256 pixels, top-down 30-35 degree overhead view. ORGANIC IRREGULAR OVAL shape with crisp pixel-broken edges — NO smooth curves, NO Gaussian blur, NO grid alignment. Patch is dusty stone debris tones using locked palette: dark dust #1A1C20, dirt body #2A2D34 with cooler grey-brown tint (NOT warm brown), highlight specks #3A3D48 and #4E5260, fine grit #606575. NO ice-blue accent on dirt (reserved for cracks). Outer silhouette is jagged pixel cluster fade — small isolated grit pixels break off like wind-scattered debris. Body has subtle pebble texture (1-3 pixel speckles) scattered within. Sits ON TOP of floor; body opaque; surroundings transparent.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [256, 256],
    "n": 16,
    "style_strength": 0.65,
    "negative_prompt": "Gaussian blur, smooth gradient, warm brown, photorealistic dirt, lush, modern UI, repeating tile, uniform oval, anti-aliased curves, ice-blue glow, ivy, grass, character"
  }
}
```

**Selection criteria:**
1. Irregular pixel-broken silhouette (similar to moss)
2. Cool grey-brown palette (NOT warm chocolate brown)
3. Pebble speckle texture present but subtle
4. Body opaque, surroundings transparent
5. 3 picks vary: scattered, compact, elongated

**Output paths:**
- `STAGING/TILESET_OUTPUT/L4_Transition/dirt_oval_01.png` through `_03.png`

---

## 3. L5 — Detail Decal Sprites (6 total)

L5 decals are SMALL high-frequency overlays scattered with edge-biased density (Karar #143-E: 10x near walls, 0.1x center).

### 3.1 Crack Lines — 3 variations

**Canvas:** 64 × 128 px (or 128 × 64 — orientation flexibility)
**Variants:** 3 (rotation allowed at runtime)

**MCP call:**
```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Hades-style painter pixel art crack line decal for shattered keep stone floor, 128x64 pixels, top-down 30-35 degree overhead view. THIN organic crack vein running diagonally or sinuously across the sprite — NOT a straight line, NOT a grid pattern. Crack is dark #1A1C20 against transparency, with subtle #2A2D34 inner shadow and 1 or 2 fine ice-blue mineral fleck pixels #7BA7BC along the crack (sparse, hairline). Crack thickness 2-3 pixels at widest, tapering to 1 pixel at ends. Crack body fully opaque; surrounding pixels fully transparent. ORGANIC IRREGULAR path — sinuous, branching, no straight segments longer than 5 pixels.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [128, 64],
    "n": 16,
    "style_strength": 0.6,
    "negative_prompt": "Gaussian blur, straight line, ruler edge, grid pattern, bright colors, glowing crack, modern UI, anti-aliased curves, thick crack, photorealistic, character"
  }
}
```

**Selection criteria:**
1. Crack path is organic and irregular (no straight segments)
2. Thickness varies (2-3px peak, taper to 1px)
3. Hairline ice-blue mineral fleck present but extremely subtle
4. 3 picks vary: (a) diagonal single, (b) branching Y-shape, (c) sinuous wave

**Output paths:**
- `STAGING/TILESET_OUTPUT/L5_Detail/crack_01.png` through `_03.png`

### 3.2 Rubble Cluster — 3 variations

**Canvas:** 64 × 64 px
**Variants:** 3

**MCP call:**
```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Hades-style painter pixel art small rubble cluster decal for shattered keep stone floor, 64x64 pixels, top-down 30-35 degree overhead view. 3-7 small irregular stone chunks scattered in a loose cluster — varying sizes (4-12 pixel chunks), no two identical, no symmetric arrangement. Stone palette #2A2D34 / #3A3D48 / #4E5260 with #606575 highlight catch on top edges. Each chunk has a 1-pixel dark #1A1C20 shadow on the south side (sells the 30-35 degree view). Surrounding pixels fully transparent. NO grid alignment, NO repeating pattern.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [64, 64],
    "n": 16,
    "style_strength": 0.65,
    "negative_prompt": "Gaussian blur, smooth shading, identical chunks, grid pattern, bright colors, modern UI, anti-aliased shapes, photorealistic, character, large boulder, single rock"
  }
}
```

**Selection criteria:**
1. 3-7 chunks of varying size
2. Each chunk has a 1-pixel south-side shadow
3. No symmetric or grid arrangement
4. 3 picks vary: (a) dense cluster, (b) scattered line, (c) compact pile

**Output paths:**
- `STAGING/TILESET_OUTPUT/L5_Detail/rubble_01.png` through `_03.png`

---

## 4. L6 — Rift Accent Sprites (3 total)

L6 is SPARSE high-priority magical detail. Per-room 0-3 instances only. Sits at highest sortingOrder.

### 4.1 Rift Fracture Thin — 2 variations

**Canvas:** 96 × 128 px
**Variants:** 2

**MCP call:**
```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Hades-style painter pixel art rift fracture decal for shattered keep stone floor, 96x128 pixels, top-down 30-35 degree overhead view. Thin magical fracture line — like a tear in reality — running vertically/diagonally across the sprite. Fracture is dark #1A1C20 core with prominent ice-blue mineral glow #7BA7BC along the inner edges (this is L6 magical accent — ice-blue is REQUIRED here, sparingly elsewhere). Fracture thickness 3-5 pixels at widest. Outer pixels along the fracture have a subtle ice-blue rim halo (1-2 pixels). Surrounding pixels fully transparent. NO smooth glow gradient — halo is crisp pixel breakup. ORGANIC IRREGULAR fracture path with branching micro-cracks.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [96, 128],
    "n": 16,
    "style_strength": 0.65,
    "negative_prompt": "Gaussian blur, smooth glow, straight line, ruler edge, bright magic, modern UI, anti-aliased curves, photorealistic, character, lens flare, particle effect"
  }
}
```

**Selection criteria:**
1. Fracture path is irregular branching
2. Ice-blue rim halo is crisp pixel breakup (NOT smooth gradient)
3. 2 picks vary: (a) single tear, (b) branching Y

**Output paths:**
- `STAGING/TILESET_OUTPUT/L6_Accent/rift_fracture_01.png` and `_02.png`

### 4.2 Rift Corruption Blob — 1 sprite

**Canvas:** 128 × 128 px
**Variants:** 1

**MCP call:**
```json
{
  "tool": "create_object",
  "parameters": {
    "description": "Hades-style painter pixel art rift corruption blob decal for shattered keep stone floor, 128x128 pixels, top-down 30-35 degree overhead view. Organic irregular dark stain — like void corruption pooled on the stone. Core is deepest #1A1C20 black void, outer body uses #2A2D34 with ice-blue mineral fleck halos #7BA7BC scattered through the body (3-6 hairline flecks). Outer silhouette is pixel-broken irregular — small isolated dark pixel clusters break off like the corruption is spreading. NO smooth glow, NO Gaussian fade — only crisp pixel breakup. Surrounding pixels fully transparent. ORGANIC IRREGULAR shape, no symmetry.",
    "style_image": "Assets/Art/Tiles/F1/Tilesets/floor_wall/spritesheet.png",
    "sprite_size": [128, 128],
    "n": 16,
    "style_strength": 0.65,
    "negative_prompt": "Gaussian blur, smooth glow, bright magic, particle effect, modern UI, anti-aliased curves, photorealistic, character, symmetric blob, perfect circle"
  }
}
```

**Selection criteria:**
1. Irregular pixel-broken silhouette
2. 3-6 hairline ice-blue mineral flecks scattered (NOT dominant)
3. No symmetry, no perfect circle
4. Body opaque, surroundings transparent

**Output path:**
- `STAGING/TILESET_OUTPUT/L6_Accent/rift_corruption.png`

---

## 5. Estimated Credits

| Sprite | n | Picks | Credits |
|---|---|---|---|
| L4 Moss oval | 16 | 3 | ~3 |
| L4 Dirt oval | 16 | 3 | ~3 |
| L5 Crack lines | 16 | 3 | ~3 |
| L5 Rubble cluster | 16 | 3 | ~3 |
| L6 Rift fracture | 16 | 2 | ~3 |
| L6 Rift corruption | 16 | 1 | ~3 |
| **L4/L5/L6 total** | | **15** | **~18 (5 calls)** |

Combined with L3 (~14-21 credits) = **~32-39 credits for full Sprint 3**.

---

## 6. QC Gate (each sprite type)

PASS criteria (all six must be true):

| Check | Pass Condition |
|---|---|
| Irregular silhouette | Pixel-broken, no smooth curve, no rectangle, no grid |
| Palette match | All pixels within 6 locked hex values |
| No blur | Crisp pixel edges, no Gaussian, no anti-aliasing |
| Transparency correct | Body opaque, surroundings fully transparent |
| L6 ice-blue presence | Required on L6 (fracture rim, corruption flecks); subtle elsewhere |
| Style match | Coherent with L3 wall stone aesthetic |

**FAIL action:** regenerate with adjusted `style_strength` (0.55-0.75 range) or prompt emphasis tweak.

---

## 7. Post-Generation Integration (orchestrator action)

After all 15 L4/L5/L6 sprites produced:

1. Create folder structure:
   ```
   Assets/Art/Karar143/L4_Transition/
   Assets/Art/Karar143/L5_Detail/
   Assets/Art/Karar143/L6_Accent/
   ```

2. Import each PNG:
   - Filter Mode: **Point (no filter)**
   - Compression: **None**
   - Sprite Mode: Single
   - Pixels Per Unit: **32**
   - Pivot: **Center**

3. Create AssetPool .asset files under `Assets/Data/Brush/`:
   - `AssetPool_Moss_L4.asset` — 3 moss sprites
   - `AssetPool_Dirt_L4.asset` — 3 dirt sprites
   - `AssetPool_Crack_L5.asset` — 3 crack sprites
   - `AssetPool_Rubble_L5.asset` — 3 rubble sprites
   - `AssetPool_RiftFracture_L6.asset` — 2 rift fracture
   - `AssetPool_RiftCorruption_L6.asset` — 1 corruption

4. Wire AssetPools to default brush presets created in Sprint 6.

5. PatchAtlas legacy assets (`PatchAtlas_Moss_ShatteredKeep`, `PatchAtlas_Detail_ShatteredKeep`, `PatchAtlas_Rift_Fracture`) — kept for backward compat OR archived to `Assets/Art/_archive_faz1/`. Decision per orchestrator.

---

## 8. Dispatch Order Reminder

1. L3 wall (pixellab_l3_wall_batch.md) — LOCK FIRST
2. L4 transition (this file §2)
3. L5 detail (this file §3)
4. L6 accent (this file §4)

L4/L5/L6 may overlap dispatch if L3 style is locked and PixelLab API tolerates parallel calls — orchestrator judgment.
