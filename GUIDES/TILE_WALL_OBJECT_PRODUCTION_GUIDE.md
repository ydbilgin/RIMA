# Tile, Wall, and Object Production Guide
**Project:** RIMA — 2D Isometric Roguelite
**Style:** Fractured Epic (gritty, compact, non-chibi — Hades / Dead Cells density)
**Last updated:** 2026-05-03
**Budget snapshot:** ~2414 gen remaining, expires 2026-05-18

---

## 1. Asset Inventory

### Starting Biomes

| Biome | Theme | Palette anchor |
|-------|-------|----------------|
| **Stone Dungeon** | Cold carved stone, iron fixtures, deep shadows | Cool grey, dark teal accent, orange torch glow |
| **Mossy Crypt** | Ancient burial chambers, wet stone, overgrowth | Warm grey, olive/moss green, pale bone accent |

---

### 1.1 Floor Tile Variants (per biome)

Minimum 4 variants (A/B/C/D) per biome. Variants must read as "same material" but carry distinct micro-detail so tiling seams are invisible at runtime.

| Variant | Description |
|---------|-------------|
| A | Clean / base tile — primary coverage tile |
| B | Minor crack, 1-2 hairlines across surface |
| C | Edge worn — rougher perimeter, slight color shift |
| D | Stain/damage — scorch mark, moss patch, or pooled grime |

Stone Dungeon specifics:
- A: Smooth chiseled granite, subtle grout lines
- B: Single diagonal crack, chipped corner
- C: Worn edge, exposed aggregate
- D: Dark water stain, small algae smear

Mossy Crypt specifics:
- A: Old stone, faint carved line pattern
- B: Hairline crack with thin moss seeping in
- C: Uneven surface, one corner crumbled
- D: Dense moss patch, small puddle tint

Canvas per floor tile: **64x32 px**

---

### 1.2 Wall Types (per biome)

Each type has 1-2 variants (V1 = standard, V2 = damaged/alt).

| Type | Description | Variants |
|------|-------------|----------|
| Straight wall | Flat wall face, full-height block | V1 standard, V2 cracked |
| Corner piece | 90-degree inside or outside corner | V1 only (unique silhouette) |
| Pillar / Column | Freestanding load-bearing column | V1 standard, V2 chipped |

Stone Dungeon walls: cut stone blocks, iron bracket detail, torch mount socket on straight V1.
Mossy Crypt walls: older rougher cut, vine traces crawling up, skull niche detail on straight V1.

Canvas per wall: **64x96 px**

---

### 1.3 Props — Tier List (per biome unless noted)

#### Common Scatter (high frequency — aim for 6-8 types per biome)

| # | Prop | Notes |
|---|------|-------|
| 1 | Rubble pile small | 2-4 broken stones |
| 2 | Rubble pile large | Larger heap, occludes slightly |
| 3 | Floor crack (standalone) | Decorative, no collider |
| 4 | Scattered bones | Arm/rib fragments |
| 5 | Skull (single) | Detail accent |
| 6 | Candle (unlit) | Melted wax, cold |
| 7 | Candle (lit) | Warm glow ring, small flame |
| 8 | Dirt/ash smear | Floor decal-style, flat |

#### Mid Props (medium frequency — 4-6 types)

| # | Prop | Notes |
|---|------|-------|
| 1 | Wooden barrel | Intact, iron banding |
| 2 | Broken barrel | Staves splayed outward |
| 3 | Wooden crate | Stackable silhouette |
| 4 | Torch sconce (wall) | Mounted, flame or extinguished |
| 5 | Hanging chain | Ceiling anchor point |
| 6 | Iron brazier | Low standing, embers or cold |

#### Landmark (rare — 1 per room type, 2-4 types total)

| # | Prop | Canvas | Notes |
|---|------|--------|-------|
| 1 | Altar | 64x64 or 128x128 | Stone slab, carved runes |
| 2 | Statue (broken) | 64x64 or 128x128 | Headless figure, stone |
| 3 | Chest (closed) | 64x64 | Iron-banded, ornate lock |
| 4 | Chest (open/looted) | 64x64 | Lid ajar, empty |

Canvas per prop (default): **64x64 px**
Canvas per large landmark: **128x128 px**

---

## 2. MCP Tool Routing

| Asset category | MCP tool | Canvas | confirm_cost rule |
|----------------|----------|--------|-------------------|
| Floor tiles | `create_isometric_tile` | 64x32 | Always true for batches >10 gen |
| Walls (straight, corner, pillar) | `create_map_object` | 64x96 | Always true for batches >10 gen |
| Props (all tiers) | `create_object` | 64x64 (128x128 for landmarks) | Always true for batches >10 gen |

Notes:
- `create_isometric_tile` enforces the 2:1 diamond ratio — do not use `create_object` for floors.
- `create_map_object` is the correct route for anything structural that occupies wall height. Do not use `create_object` for walls.
- Single one-off test gen: `confirm_cost` may be false to skip prompt. For any batch, always set `confirm_cost: true`.

---

## 3. Prompt Templates

### Style Anchor Block (reuse verbatim in every prompt)

```
isometric pixel art, fractured epic style, gritty and worn, rich directional shadow,
hard pixel edges, no anti-aliasing, no dithering, no soft gradients,
light source from top-left, dark atmosphere, high contrast
```

---

### 3.1 Floor Tile Template

```
Prompt:
  isometric floor tile, {BIOME} material, variant {VARIANT_LETTER},
  {VARIANT_DETAIL}, isometric pixel art, fractured epic style, gritty and worn,
  rich directional shadow, hard pixel edges, no anti-aliasing, no dithering,
  no soft gradients, light source from top-left, dark atmosphere, high contrast,
  64x32 pixel canvas, top-down diamond shape, seamless-ready edges

Negative:
  cartoon, chibi, anime, smooth gradients, anti-aliased edges, watercolor,
  3D render, bright colors, flat lighting, top-down orthographic
```

`{VARIANT_DETAIL}` examples:
- A: "clean surface, subtle grout lines"
- B: "single diagonal hairline crack, chipped corner"
- C: "worn rough edge, exposed aggregate texture"
- D: "dark water stain, small moss smear"

---

### 3.2 Wall Template

```
Prompt:
  isometric wall {WALL_TYPE}, {BIOME} stone, {VARIANT_DETAIL},
  isometric pixel art, fractured epic style, gritty and worn,
  rich directional shadow, hard pixel edges, no anti-aliasing, no dithering,
  no soft gradients, light source from top-left, dark atmosphere, high contrast,
  64x96 pixel canvas, tall isometric block, bottom-anchored

Negative:
  cartoon, chibi, anime, smooth gradients, anti-aliased edges, floating,
  top-only sprite, incorrect isometric angle, flat lighting
```

`{WALL_TYPE}` values: "straight face", "corner junction", "freestanding column/pillar"

---

### 3.3 Prop Template

```
Prompt:
  isometric prop, {PROP_NAME}, {BIOME} dungeon setting,
  isometric pixel art, fractured epic style, gritty and worn,
  rich directional shadow, hard pixel edges, no anti-aliasing, no dithering,
  no soft gradients, light source from top-left, dark atmosphere, high contrast,
  {CANVAS_SIZE} pixel canvas, readable at small scale, strong silhouette

Negative:
  cartoon, chibi, anime, smooth gradients, anti-aliased edges,
  UI element style, flat icon, blurry, oversized details
```

`{CANVAS_SIZE}`: "64x64" for standard, "128x128" for large landmarks.

---

### 3.4 Biome Palette Notes (embed in prompt as needed)

| Biome | Add to prompt |
|-------|---------------|
| Stone Dungeon | "cool grey stone, dark teal shadow accent, orange torch glow" |
| Mossy Crypt | "warm grey aged stone, olive moss green, pale bone accent, damp surface" |

---

## 4. Variety Strategy

### Why 4+ Floor Variants

With fewer than 4 variants, random placement produces visible repetition clusters — the human eye detects even randomized tiling with only 2-3 tiles at ~30% of viewport area. Four variants at correct weights suppress this to imperceptible.

### Recommended Weights

| Variant | Weight | Rationale |
|---------|--------|-----------|
| A (clean) | 40% | Base coverage, most neutral |
| B (crack) | 25% | Common but not dominant |
| C (worn edge) | 20% | Adds texture variation |
| D (stain/damage) | 15% | Accent — too much reads as dirty |

### Unity Assignment Rule

Use **Unity Rule Tile** (`RuleTile` or a custom `WeightedRandomTile`):
- Do NOT use checkerboard or fixed offset patterns.
- Assign variants by weighted random draw per cell at Tilemap paint time OR at runtime via a TilePlacementSystem component.
- Enforce: no two identical variant tiles on orthogonally adjacent cells (swap one if collision detected during placement).
- `Random.InitState(seed)` tied to room seed so layout is deterministic per run.

### Recommended Unity Tools

- `WeightedRandomTile` (available in Unity 2D Extras / Tilemap Extras package)
- Or custom `TileBase` subclass with weighted selection in `GetTileData`
- Avoid `AnimatedTile` for static variants — unnecessary overhead

---

## 5. Unity Import Pipeline

### 5.1 Floor Tiles

| Setting | Value |
|---------|-------|
| Texture type | Sprite (2D and UI) |
| Sprite mode | Single |
| PPU | 64 |
| Filter mode | Point (no filter) |
| Compression | None (pixel art) |
| Pivot | Bottom Center |
| Tilemap type | Isometric Tilemap |
| Grid cell size | (1, 0.5) for 64x32 isometric |

Placement: Paint via Isometric Tilemap, Sorting Order = Y-axis based (set on Tilemap renderer).

### 5.2 Walls

| Setting | Value |
|---------|-------|
| Texture type | Sprite (2D and UI) |
| PPU | 64 |
| Filter mode | Point (no filter) |
| Pivot | Bottom Center |
| Object type | Prefab (not Tilemap) |
| Sorting | SortingOrder driven by Y position (lower Y = higher order) |
| Collider | PolygonCollider2D or BoxCollider2D on prefab, sized to base footprint only |

Note: walls at 64x96 occupy ~32px floor base + ~64px visible height. The pivot at bottom center ensures correct isometric depth sorting.

### 5.3 Props

| Setting | Value |
|---------|-------|
| Texture type | Sprite (2D and UI) |
| PPU | 64 |
| Filter mode | Point (no filter) |
| Pivot | Bottom Center |
| Object type | Prefab |
| Collider | Optional per prop (scatter = none, barrels/crates = yes, landmarks = yes) |
| Sorting | Y-axis based, same layer as walls |

Large landmarks (128x128): PPU stays 64, so sprite reads as 2x2 world units. Confirm pivot is at bottom center of the base footprint, not geometric center.

### 5.4 Atlas Packing

- Group sprites into SpriteAtlas per biome.
- Naming convention: `Atlas_StoneDungeon`, `Atlas_MossyCrypt`
- Include floors, walls, and props for that biome in one atlas.
- Shared/cross-biome props (chests, altar): `Atlas_Shared_Props`
- Max atlas size: 2048x2048 for each. Verify no sprite is cropped at pack time.

---

## 6. Budget Estimate

### Calculation

| Category | Count | Gen cost |
|----------|-------|----------|
| Stone Dungeon floors (4 variants) | 4 | 4 |
| Stone Dungeon walls (3 types x 2 variants) | 6 | 6 |
| Mossy Crypt floors (4 variants) | 4 | 4 |
| Mossy Crypt walls (3 types x 2 variants) | 6 | 6 |
| **Subtotal: tiles + walls** | **20** | **20** |
| Common scatter props (8 types) | 8 | 8 |
| Mid props (6 types) | 6 | 6 |
| Landmark props (4 types) | 4 | 4 |
| **Subtotal: props** | **18** | **18** |
| **Raw total** | **38** | **38** |
| +20% buffer | +8 | +8 |
| **Total with buffer** | **46** | **46** |

### Budget Health

| Metric | Value |
|--------|-------|
| Estimated spend | ~46 gen |
| Remaining budget | ~2414 gen |
| Headroom | ~2368 gen (98% remaining after this set) |
| Budget expiry | 2026-05-18 |

The full biome-1 + biome-2 tile and prop set costs under 2% of remaining budget. Significant headroom exists for additional biomes, prop expansions, and regen of rejected outputs.

---

## 7. Production Order

Follow this order to derisk pipeline issues early and keep momentum.

| Step | Work | Reason |
|------|------|--------|
| 1 | Stone Dungeon floor variants (A/B/C/D) | Cheapest, 4 gen. Proves `create_isometric_tile` pipeline and pivot alignment before wall work. |
| 2 | Stone Dungeon walls (straight V1+V2, corner, pillar V1+V2) | Validates `create_map_object` at 64x96, depth sorting in Unity. |
| 3 | Common scatter props for Stone Dungeon | High-use assets. Fast 8-gen batch. Populates a playable room immediately. |
| 4 | Mossy Crypt floors + walls | Repeat proven pipeline in second biome. Spot-check palette shift reads distinctly. |
| 5 | Mid props (barrels, crates, sconces, chains) | Medium complexity. Validate collider fitting on barrel/crate prefabs. |
| 6 | Landmark props (altar, statue, chests) | Largest canvas (128x128), lowest frequency. Do last when pipeline is stable. |

Do not batch steps 1 and 4 together on first run. Confirm Stone Dungeon reads correctly in Unity before starting the second biome.

---

## 8. QC Checklist (per batch)

Run these checks after each generation batch before committing assets.

### Isometric Alignment

- [ ] Diamond footprint snaps cleanly to 64x32 grid — no sub-pixel overhang
- [ ] Floor tile edges align with adjacent tiles without visible seam or gap at default zoom
- [ ] Wall base aligns with floor tile edge it sits on

### Pixel Art Fidelity

- [ ] No anti-aliasing bleed at any edge (hard pixels only)
- [ ] No dithering artifacts on flat color surfaces
- [ ] No soft gradients — all shading via solid pixel steps
- [ ] Compression artifacts absent (import as uncompressed or lossless)

### Lighting Consistency

- [ ] Shadow direction: top-left light source across all assets in batch
- [ ] Highlight face on top-left facet, shadow on right and bottom facets
- [ ] No asset with reversed or ambient-only shading mixed into batch

### Readability

- [ ] Wall height reads cleanly at 64x96 — top of wall does not bleed into ceiling void
- [ ] Props are legible at 50% zoom (simulated game view distance)
- [ ] Landmark props (128x128) read as larger and distinct from standard 64x64 props
- [ ] No prop silhouette is confused with a floor crack or wall element

### Unity Integration

- [ ] PPU = 64 on all imported sprites
- [ ] Pivot set to Bottom Center — confirm in Sprite Editor if auto-set is wrong
- [ ] Depth sort in scene appears correct (near objects render above far objects)
- [ ] Atlas pack completed with no sprite cropping warnings
- [ ] No material or shader errors on prefabs in scene

---

*End of guide. For PixelLab API parameter details see `F:/Antigravity Projeler/Pixellab/PIXELLAB_API_V2.md`.*
*For project-wide S43 sprite rules see `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`.*
