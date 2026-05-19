# PixelLab Tile Production Batches v1 — Alabaster Dawn Pixel-Art Target

**Hedef:** RIMA chibi karakterleri (PixelLab Create Image Pro) ile **aynı pixel-art ailede** zemin/duvar/decal üretmek. Cinderia tarzı painterly/illustrator NO; Alabaster Dawn / CrossCode 16-32-bit pixel hard-edge YES.

**Style invariants (her batch'te tut):**
- 64px native pixel art, hard pixel edges, NO anti-aliasing
- Max 2-3 tone per region, 1px subtle silhouette outline
- High top-down 30-35° (Karar #100), NOT pure 90°, NOT isometric
- **Palette: Vivid Vulnerability + Ritual Catastrophe**
  - Dominant: dark slate gray (#3a3530), deep brown (#4a3a30), dusty blue (#5a6a78)
  - Accent: faint dark red (#8a3030 rift), warm orange (#c87830 fire-touched), deep moss green (#3a5a3a), cold blue rim (#8aa8c0)
- Mood: weathered, ritual aftermath, hollow watchful — NOT cartoon, NOT anime, NOT painterly gradient

---

## BATCH 1 — Floor Tiles (PRIORITY 0)

**Tool:** `mcp__pixellab__create_tiles_pro`

**Mode A — Shape (recommended ilk pas, no reference):**
```
tile_type: "square_topdown"
tile_size: 64
tile_view: "high top-down"
tile_view_angle: 35
outline_mode: "segmentation"
no_outline: false
prompt: |
  1). clean weathered stone floor, deep brown-gray base, slight grain variation, ancient ritual temple atmosphere
  2). stone floor with sparse moss patch, deep moss green spot blending into dark stone
  3). cracked stone floor, thin hairline fractures with darker shadow lines, subtle dust accumulation
  4). worn smooth stone floor, polished by foot traffic, faint cold blue rim highlight
  5). stained stone floor, dusty blue mineral residue, faint abstract sigil-like discoloration
  6). hairline-cracked stone with cold blue glow at crack edges, weathered ancient feel
```

**Mode B — Style (sonraki pas, ilk batch onaylanırsa):**
- Birinci batch'ten approved 1 tile → `style_images` parametresi
- Sonraki batch'lerin tutarlılığı garanti

**Beklenen output:** 6 floor variant, 64×64 each, seamless tiling, Wang-free (decoration tiles).
**Karar #143-A:** L1 base floor pool — 6 tiles.

---

## BATCH 2 — Wang16 Wall Tileset (PRIORITY 0)

**İki path mevcut — A native, B manual:**

### Path A (RECOMMENDED) — Native Wang16 auto-edge

**Tool:** `mcp__pixellab__create_topdown_tileset`

```
tile_size: 32           ← PixelLab cap (64 desteklenmiyor bu tool'da)
lower_description: |
  deep brown-gray weathered stone floor, dusty grain variation,
  matches Batch 1 floor palette and pixel discipline
upper_description: |
  dark weathered stone wall block top, dusty blue rim highlight at upper edge,
  sparse moss creeping at lower edge, hollow watchful ritual temple atmosphere,
  1px solid silhouette outline, hard pixel edges
transition: true        ← floor→wall transition tiles dahil
view: "high top-down"
view_angle: 35
```

**Output:** 23 tile (16 Wang + 7 transition) — auto-matched edges.

**Pixel reality:** 32px native. Unity import:
- Sprite PPU = 32
- Brush V1 tile size = 64 effective (Unity scales 2x)
- VEYA: PPU = 64 native, sprite görünür 2x büyük (kaldır)

**Sorun:** 32→64 upscale pixel discipline'a uyumlu (her piksel 2x büyür, anti-alias yok), ama detay daha az olur. Test edip karar.

### Path B (FALLBACK) — Manual 64px Wang

**Tool:** `mcp__pixellab__create_tiles_pro`

```
tile_type: "square_topdown"
tile_size: 64
tile_view: "high top-down"
tile_view_angle: 35
outline_mode: "segmentation"
prompt: |
  1). isolated stone wall block, all 4 edges visible, top cap dusty blue rim
  2). wall with N edge connection, S/E/W edges visible
  3). wall with E edge connection
  4). wall with S edge connection
  5). wall with W edge connection
  6). wall N-S straight (vertical run)
  7). wall E-W straight (horizontal run)
  8). wall N-E corner
  9). wall E-S corner
  10). wall S-W corner
  11). wall W-N corner
  12). wall N-E-S T-junction
  13). wall E-S-W T-junction
  14). wall S-W-N T-junction
  15). wall W-N-E T-junction
  16). wall all 4-way cross junction
```

**Risk:** Auto-edge matching garanti değil — manual QC + retry gerekebilir.

**Recommendation:** Path A ile başla. Çözünürlük yetersiz görünürse Path B'ye geç.

---

## BATCH 3 — Organic Decals (PRIORITY 1)

**Tool:** `mcp__pixellab__create_object`

**Recipe:** Her decal türü için 16 frame review pack — sonra `select_object_frames` ile en iyi 4-5 pick.

```
directions: 1
n_frames: 16
view: "high top-down"
object_view: "top-down"
size: 64
transparent background: true
outline: subtle 1px silhouette
```

**6 decal türü (paralel veya sıralı dispatch):**

| # | description | Variants needed |
|---|---|---|
| D1 | small moss tuft cluster, deep moss green organic blob shape, transparent background, top-down view, soft edge blend, painterly Hades atmosphere but hard pixel art technique | 4-5 picks from 16 |
| D2 | dirt patch, irregular brown stain shape, transparent background, organic blob, dusty texture | 4-5 picks |
| D3 | vegetation cluster, small dark green weeds with creeping moss base, transparent background | 4-5 picks |
| D4 | wet dark stain, subtle dark damp area, irregular oval shape, transparent background | 4-5 picks |
| D5 | rubble scatter, small dark stone fragments arrangement, transparent background | 4-5 picks |
| D6 | bone fragment scatter, weathered bone shards arrangement, transparent background | 4-5 picks |

---

## BATCH 4 — Large Accent Overlays (PRIORITY 2)

**Tool:** `mcp__pixellab__create_object`

```
directions: 1
n_frames: 16
view: "high top-down"
size: 128                 ← native 128 (RIMA L6 layer)
transparent background: true
```

**3 accent türü:**

| # | description |
|---|---|
| A1 | rift scar, large dark crimson irregular multi-blob with radial cracks, 128px, transparent background, cold rim glow at crack edges, catastrophic ritual mood, hard pixel art technique |
| A2 | battle aftermath, dark red blood splatter combined with dust cloud, irregular organic stain shape, 128px, transparent background |
| A3 | scorch mark, dark charred burn area with charcoal black center fading to ember orange at edges, irregular blob, 128px, transparent background |

---

## BATCH 5 — Detail Scatter (PRIORITY 3)

**Tool:** `mcp__pixellab__create_object`

```
directions: 1
n_frames: 16
view: "high top-down"
size: 32                  ← küçük scatter
transparent background: true
```

| # | description |
|---|---|
| S1 | pixel crack pattern, thin dark hairline cracks radiating outward, 32px, transparent bg |
| S2 | pebble scatter, small dark stones arranged organically, 32px, transparent bg |
| S3 | bone fragment, single weathered bone shard, 32px, transparent bg |
| S4 | debris cluster, small rubble pieces arrangement, 32px, transparent bg |

---

## Production Order

1. **Batch 1 ilk** (floor foundation, palette anchor)
2. User QC → 1 floor tile approved
3. Approved tile → `style_images` parametresi olarak Batch 2-5'e geçer
4. **Batch 2 paralel** (walls), **Batch 3 paralel** (decals)
5. **Batch 4 + 5** sonraki gün

## Credit estimate (RIMA scope)

| Batch | Tool | Calls | Credit/call | Total |
|---|---|---|---|---|
| 1 | create_tiles_pro | 1 (6-tile prompt) | ~6 | 6 |
| 2A | create_topdown_tileset | 1 (23 tiles) | ~10 | 10 |
| 2B fallback | create_tiles_pro | 1 (16-tile prompt) | ~16 | 16 |
| 3 | create_object × 6 | 6 (16 frames each) | 3 | 18 |
| 4 | create_object × 3 | 3 (16 frames each) | 3 | 9 |
| 5 | create_object × 4 | 4 (16 frames each) | 3 | 12 |
| **TOTAL Path A** | | | | **~55 credit** |
| **TOTAL Path B fallback** | | | | **~61 credit** |

**5000 gen budget'a karşı:** trivial (~1%).

---

## Unity Integration (PixelLab outputs)

**Klasör:**
```
Assets/Sprites/Environment/StoneDungeon_v3/
  ├── Tiles/        (Batch 1 → 6 floor PNG)
  ├── Walls/        (Batch 2 → 16-23 wall PNG)
  ├── Decals_L4/    (Batch 3 → 4-5 picks per type)
  ├── Accents_L6/   (Batch 4 → 3 accent PNG)
  └── Detail_L5/    (Batch 5 → 4 detail PNG)
```

**Brush V1 wire-up:**
1. BrushAtlasImporter scan StoneDungeon_v3/
2. Wang16 mapping (Path A: auto, Path B: manual via WangAtlasMapper)
3. RoomBank `StoneDungeon_v3` SO create
4. Sample room re-compose Unity Editor
5. Screenshot → STAGING/sample_room_pixellab_v3.png

---

## Codex Paralel Test (separate)

Aynı anda Codex'e forced `gpt-image-1` dispatch (Pillow fallback BANNED) — single 64×64 floor tile.

Output: `STAGING/codex_floor_tile_forced_gpt_image_1.png`

A/B comparison:
- PixelLab Batch 1 → 6 tiles
- Codex forced → 1 tile (proof of concept)

Unity'de ikisini birlikte composer'a koyup yan yana göster.
