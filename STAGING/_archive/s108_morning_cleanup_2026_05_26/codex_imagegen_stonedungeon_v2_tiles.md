# Codex Task — StoneDungeon_v2 Tile Set Production

## Gorev
**Hybrid pipeline KARAR:** Codex imagegen tile/decal/wall set'lerini uretir, PixelLab discrete object/character/prop uretir.

Bu task ilk dispatch: **StoneDungeon_v2 tile set** — sample room demo icin yeni v1-painterly-quality tile assetleri.

## Reference Context
- v1 final concept (target mood): `STAGING/concept_art_rima_sample_room.png`
- v2 final concept (alt mood): `STAGING/concept_art_rima_sample_room_v2.png`
- Real mevcut tile drift: `STAGING/RIMA_REAL_TILESET_ROOM.png` (eski set isometric drift'li)

User feedback: v1 mood (atmospheric, organic, painterly) istiyor — eski StoneDungeon tile set Karar #100 (30-35° top-down) ile uyumsuz drift'li.

## Hedef Outputs

### Klasor yapisi (Codex bunu yaratacak)
```
Assets/Sprites/Environment/StoneDungeon_v2/
  ├── Tiles/         (6 floor tile, 64x64 each)
  ├── Walls/         (16 Wang16 wall tile, 64x96 each)
  ├── Decals_L4/     (5 organic moss/dirt decals, 64x64)
  ├── Detail_L5/     (4 detail elements, 32x32)
  └── Accents_L6/    (3 large accent overlays, 128x128)
```

### Tile Set A — 6 Floor Tiles (64x64 each)
Tek wide image (256x256 contact sheet, 2x3 grid) icinde 6 farkli floor variant.

Each tile:
- Seamless tiling (no visible borders when placed adjacent)
- Dark weathered stone, Vivid Vulnerability palette
- Karar #100 high top-down 30-35 derece perspective
- 64x64 pixel art native
- Hard pixel edges, no anti-aliasing
- Tone: deep brown-gray dominant + dusty blue accent + faint cold blue rim
- Subtle organic variation per tile (cracks, stains, weathered marks) but **NOT random drift between tiles**
- Per-tile distinct identity: 1 clean, 2 mossy, 3 cracked, 4 worn, 5 stained, 6 hairline-cracked
- Tiles look natural when placed in a grid (mood: painterly Hades-style ARPG floor)

### Tile Set B — 16 Wang16 Walls (64x96 each)
Tek wide image (256x384 contact sheet, 4x4 grid) icinde 16 Wang16 wall config.

Wang16 setup:
- 16 distinct wall tiles encoding N/E/S/W edge connections
- Stone block wall, 64-pixel base + 32-pixel top cap visible (64x96 native)
- Seamless edge-to-edge connection when tiled
- Same Vivid Vulnerability palette as floor
- Karar #100 30-35° top-down perspective showing top of wall
- Sparse moss creeping at bottom edge
- 1px solid black outline at silhouette

Wang16 index reference (standard mapping):
- 0: isolated block
- 1-4: single edge connections
- 5-8: two-edge straights (N-S, E-W)
- 9-12: two-edge corners (NE, SE, SW, NW)
- 13-15: three-edge T-junctions
- (Optional: 16 cross-junction — sometimes part of Wang16)

### Decal Pack L4 — Organic (5 decals, 64x64 each)
Tek wide image (320x64, 5x1 grid) icinde 5 farkli organic decal:
1. Large moss tuft cluster (deep moss green organic blob)
2. Small moss patch (single small tuft)
3. Dirt patch (irregular brown stain)
4. Vegetation cluster (small weeds + creeping moss)
5. Wet/dark stain (subtle dark damp area)

Each decal:
- Transparent background
- Irregular oval/organic shape, NOT geometric
- Soft semi-transparent edges (will blend on tile boundaries)
- 64x64 native
- Painted to be drawn on top of floor tiles in Brush V1 paint mode

### Detail Pack L5 (4 elements, 32x32 each)
Tek wide image (128x32, 4x1 grid):
1. Pixel cracks pattern
2. Pebble scatter (small stones)
3. Bone fragments
4. Debris cluster (rubble)

Each 32x32, transparent bg, scatter-friendly.

### Accent Pack L6 (3 large accents, 128x128 each)
Tek wide image (384x128, 3x1 grid):
1. **Rift Scar** — Big dark crimson irregular multi-blob with radial cracks (mood: catastrophic ritual)
2. **Battle Aftermath** — Blood splatter + dust cloud (organic dark stain)
3. **Scorch Mark** — Dark burned area with charcoal black center

Each 128x128, transparent bg, large overlay decals for L6 paint mode.

## STIL DIRECTIVES (ortak — tum tile/decal'lar icin)

**Vivid Vulnerability + Ritual Catastrophe palette:**
- Dominant: dark slate gray, deep brown, dusty blue
- Accent: faint dark red (rift), warm orange (fire-touched), deep moss green, cold blue rim highlight
- Mood: weathered, ritual, post-battle aftermath, ancient, hollow watchful
- NOT bright cartoon, NOT anime, NOT sterile

**Karar #100 perspective:**
- 30-35 degree high top-down angle
- Floor tiles seen from above at slight tilt (NOT pure 90 degree top-down, NOT isometric)
- Wall tops visible (showing upper plane of wall blocks at the tilt)
- Tile-to-tile **cohesion** — all tiles look like they belong to same artist same style

**Pixel art discipline:**
- 1px solid black outline at silhouettes (subtle, not heavy cartoon outline)
- Hard pixel edges, no anti-aliasing
- Max 2-3 tones per color
- Painterly mood preserved within pixel constraints
- Seamless tileability for L2/L3 (no visible edge mismatches)

## Negative Direktifler

- NO visible grid borders on individual tiles (let edges be subtle, blend-friendly)
- NO sharp tile-line edges
- NO isometric projection (tiles look like Karar #100 top-down, NOT diamond/iso)
- NO bright cartoon colors
- NO anime style
- NO 3D render, NO photorealistic
- NO text, labels, numbers in tiles
- NO different perspectives between tiles (consistency MANDATORY)
- NO assets that drift from Vivid Vulnerability palette

## Output Specs

| Output | Path | Dimensions | Tile count |
|---|---|---|---|
| Floor tile set | `Assets/Sprites/Environment/StoneDungeon_v2/Tiles/floor_set_a.png` | 256x256 contact sheet | 6 tiles (2x3 grid of 64x64) |
| Wall Wang16 set | `Assets/Sprites/Environment/StoneDungeon_v2/Walls/wang16_set.png` | 256x384 contact sheet | 16 tiles (4x4 grid of 64x96) |
| L4 decal pack | `Assets/Sprites/Environment/StoneDungeon_v2/Decals_L4/decals_organic.png` | 320x64 contact sheet | 5 decals (5x1 grid of 64x64) |
| L5 detail pack | `Assets/Sprites/Environment/StoneDungeon_v2/Detail_L5/detail_scatter.png` | 128x32 contact sheet | 4 elements (4x1 grid of 32x32) |
| L6 accent pack | `Assets/Sprites/Environment/StoneDungeon_v2/Accents_L6/accents_overlay.png` | 384x128 contact sheet | 3 accents (3x1 grid of 128x128) |

Plus assembled sample room render: `STAGING/concept_room_with_v2_tileset.png` — yeni tile set kullanarak compose edilmis sample room (v1 quality target).

## Codex'ten Bilgi
Bu task tamamlandiginda Codex `STAGING/codex_imagegen_stonedungeon_v2_tiles_DONE.md` icinde su bilgileri yaz:
1. **Hangi image model** kullanildi (gpt-image-1, DALL-E 3, vb. — varsa belirt)
2. Quality assessment: v1 painterly mood gercekten yakalandi mi
3. Tile cohesion (16 Wang16 ayni stilde gorunuyor mu)
4. Pipeline truth: bu tile'lar Unity'de Brush V1'a wire edilince room render eski drift'li tileset'ten farkli olacak mi
5. Hangi tile/decal regenerate gerekebilir (eger varsa)

## Onemli — Studio-Wide Pattern

Bu task **hybrid pipeline pattern**'in test'i:
- Codex imagegen → tile/decal/wall set production
- PixelLab → character/mob/prop production

Eger bu test basarili olursa: Karar #157 candidate "Hybrid Asset Production Strategy" Studio-wide LOCK olur, future Laureth Studio oyunlari ayni split kullanir.
