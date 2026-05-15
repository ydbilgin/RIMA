# Asset Gen Batch — Karar #143 Aşama 1 (PixelLab)

**Date:** 2026-05-16 S84 sonu
**Status:** Ready-to-execute (kullanıcı manual veya MCP)
**Hedef:** Aşama 1 düz floor pipeline'ı doğal görünüm için 34 sprite + 2 tileset
**Code dependency:** TransitionBrushPainter (minDistance + flipX/Y + encounterAvoidRadius LIVE)

---

## 1. Üretim Sırası (önem)

1. **Wall (L3)** — 14 sprite — map siluetinin kapanması, en kritik (önce bu, oda perimeter'ı görünür hale gelir)
2. **Floor base + variation (L1/L2)** — Shattered Keep biome için 4 base + 4 variation tile (mevcut tileset üzerinden gözden geçir)
3. **Detail decal (L5)** — 9 sprite — küçük crack/rubble/moss
4. **Transition brush (L4)** — 6 sprite — büyük oval moss/dirt/erosion
5. **Rift accent (L6)** — 3 sprite — sparse cyan
6. **Large biome patch (L4 manuel)** — 2-3 sprite — Web UI Pro 512x512

**Toplam:** ~34 sprite + 2 tileset (Pair E/F Aşama 2 için, defer).

---

## 2. PixelLab Prompt Templates (ChatGPT spec + RIMA adapt)

### Template A — Base Floor Tile (32x32)
**Tool:** PixelLab Create Tiles Pro
**Output:** seamless tileset, 4 variants per terrain
**Canvas:** 32x32 her tile
**Prompt:**
```
Create a seamless pixel art dungeon stone floor tile set for Unity.
Output size 32x32.
Material: dark fractured stone, readable gameplay floor, muted cool gray (#3A3D48 / #4E5260 / #606575), subtle vertical masonry hint.
Lighting: top-down neutral, no strong highlights, no glowing details.
Top-down floor surface, no visible side faces.
Hard pixel edges, no blur, no anti-aliasing, no soft gradients.
Designed for repeated use without obvious seams.
Generate 4 subtle variations: clean / hairline cracked / moss-edge / rune-etched.
```

### Template B — Floor Variation Tile (32x32 or 64x64)
**Tool:** PixelLab Create Tiles Pro
**Output:** 4 variants matching base palette
**Canvas:** 32x32
**Prompt:**
```
Create 4 subtle floor variation tiles for dungeon pixel art.
Output 32x32 each.
Same dark stone material as base, palette #3A3D48 / #4E5260 / #606575.
Variation 1: hairline crack diagonal.
Variation 2: scuff/scratch pattern.
Variation 3: small moss tuft (corner only).
Variation 4: dust accumulation (subtle tonal shift).
Hard pixel edges, no blur. Must seamlessly tile with base.
```

### Template C — Wall Overlay Brush (Hades-style cap, RIMA L3)
**Tool:** PixelLab Create Image Pro (Web UI önerilir, MCP'de fallback create_object n=16)
**Output:** transparent PNG, single sprite per variant
**Sizes:** 384x216 horizontal / 424x632 vertical / 341x341 corner (rectangular AR kritik)
**Prompt:**
```
Create a transparent PNG pixel art dungeon wall brush sprite for Hades-style perimeter cap overlay.
Canvas: 384x216 (horizontal wall variant).
Material: dark angular vertical masonry stone bricks, palette #1A1C20 / #2A2D34 / #3A3D48 + accent ice-blue #7BA7BC.
Wall faces forward (top-down 3/4 view, ~30-35 degree pitch like Hades dungeon).
Top of wall: irregular jagged silhouette (raggedness 40%+), broken stone fragments, lichen growth at top edge.
Bottom of wall: clean floor meeting line, slight rubble accumulation.
NO square border, NO rectangular frame, NO repeating pattern.
Edges feathered using pixel clusters (not blur).
Hard pixel edges, no anti-aliasing.
Transparent background.
Designed to overlay along room perimeter polyline as one of 4 variants.
```
**Repeat for:** vertical (424x632), corner NE/NW/SE/SW (341x341), doorway gap (192x144 thin).

### Template D — Transition Brush Oval (L4, 256x256)
**Tool:** PixelLab Create Image Pro (MCP create_object n=16 OK)
**Output:** transparent PNG oval
**Canvas:** 256x256
**Prompt:**
```
Create a transparent PNG pixel art organic overlay decal brush for a dungeon floor.
Canvas 256x256.
Irregular oval natural patch shape, not a perfect circle.
Material: dark moss (cold green #4A6A50), dirt (warm brown #6B5340), worn stone dust, subtle erosion over cracked dungeon stone.
Edges must be broken and feathered using pixel clusters.
NO hard square border.
NO visible rectangular frame.
NO repeating pattern.
Designed to be placed on top of 32x32 dungeon floor tiles as a decorative transition overlay covering 4-8 tiles.
Pixel art, hard edges, no blur, no anti-aliasing.
Transparent background.
Generate 3 variants: moss patch / dirt patch / wet stain (slight cyan #5A8A95).
```

### Template E — Large Biome Patch (L4 manuel, 512x512)
**Tool:** PixelLab Web UI Create Image Pro ONLY (MCP max 256)
**Output:** transparent PNG large patch
**Canvas:** 512x512
**Prompt:**
```
Create a transparent PNG pixel art large natural dungeon floor overlay.
Canvas 512x512.
Irregular organic patch, broken oval silhouette, NO square border.
Dark cracked stone mixed with dirt (#6B5340), moss (#4A6A50), rubble dust (#5C5552), subtle erosion.
Designed as large room dressing decal over repeated 32x32 tilemaps.
Edges must fade through scattered pixel clusters, not blur.
NO full tile shape, NO UI border, NO shadow outside the patch.
Transparent background.
Readable from gameplay camera (top-down ~30 degree pitch).
Hard pixel edges, no blur.
```

### Template F — Crack Decal (L5, 64-128px)
**Tool:** PixelLab Create Image Pro (MCP create_object n=4)
**Output:** transparent PNG small decal
**Canvas:** 128x128 (crops to 64x64 if needed)
**Prompt:**
```
Create a transparent PNG pixel art floor crack decal set.
Canvas 128x128.
Several small cracks and chipped stone lines (3-5 cracks per sprite).
Dark thin cracks (color #1A1C20), sparse detail, NO crossing/grid pattern.
NO square background, NO glow, NO outline.
Designed to overlay dungeon stone floor tiles, 1-2 tile coverage.
Hard pixel art edges, transparent background.
Generate 3 variants: hairline / chipped corner / broken slab seam.
```

### Template G — Small Rubble Tuft (L5, 32-64px)
**Tool:** PixelLab Create Image Pro (MCP create_object n=4)
**Output:** small transparent PNG
**Canvas:** 64x64
**Prompt:**
```
Create transparent PNG pixel art small rubble tuft decals.
Canvas 64x64.
Material: 3-7 small broken stone fragments (#3A3D48 / #4E5260), some tiny moss flecks (#4A6A50).
Cast subtle pixel shadow beneath fragments.
NO square background, NO outline.
Designed to scatter near walls and corners as edge detail.
Hard pixel edges, transparent background.
Generate 3 variants: stone shards / mixed rubble+moss / dust pile.
```

### Template H — Rift Accent (L6, 64-128px)
**Tool:** PixelLab Create Image Pro (MCP create_object n=4)
**Output:** transparent PNG rift fracture
**Canvas:** 128x128
**Prompt:**
```
Create a transparent PNG pixel art magical rift crack decal.
Canvas 128x128.
Thin broken cyan energy cracks (#7BA7BC primary, #B0D4E0 highlight) inside dark stone fractures.
Very subtle glow, controlled and sparse — NOT neon.
Glow falloff via 1-2 pixel ring of mid cyan, not blur.
NO square border, NO rectangular frame.
Transparent background.
Designed as rare accent detail for a dark dungeon floor — per-map 0-3 instances.
Hard pixel art edges.
Generate 3 variants: thin fracture line / corruption bloom / shattered void scar.
```

---

## 3. RIMA Asset Inventory Plan (Shattered Keep F1 biome)

### L3 Wall (14 sprite)

| Sprite | Boyut | Adet | Template | Credit (MCP) |
|---|---|---|---|---|
| Wall horizontal | 384x216 | 4 varyant | C (horizontal) | 4 × MCP n=16 = 4 |
| Wall vertical | 424x632 | 4 varyant | C (vertical) | 4 × MCP n=16 = 4 |
| Wall corner NE/NW/SE/SW | 341x341 | 4 varyant | C (corner) | 4 × MCP n=16 = 4 |
| Doorway gap | 192x144 | 2 varyant | C (doorway) | 2 × MCP n=4 = 2 |
| **L3 toplam** | | **14** | | **14** |

### L4 Transition (6 sprite + 3 manuel)

| Sprite | Boyut | Adet | Template | Credit |
|---|---|---|---|---|
| Moss patch oval | 256x256 | 3 | D | 3 |
| Dirt patch oval | 256x256 | 3 | D | 3 |
| Large biome blend | 512x512 | 2-3 | E (Web UI manuel) | manuel |
| **L4 toplam** | | **8-9** | | **6 + manuel** |

### L5 Detail (9 sprite — 3 mevcut reuse)

| Sprite | Boyut | Adet | Template | Credit |
|---|---|---|---|---|
| Mevcut moss decals (moss_2, decal_2_moss_trail, decal_6_moss_curve) | 64x64 | 3 LIVE | — | 0 |
| Crack lines | 128x128 | 3 | F | 2 |
| Small rubble tufts | 64x64 | 3 | G | 2 |
| **L5 toplam** | | **9** | | **4** |

### L6 Accent (3 sprite)

| Sprite | Boyut | Adet | Template | Credit |
|---|---|---|---|---|
| Rift fracture thin | 64-128 | 2 | H (variants 1+3) | 2 |
| Rift corruption blob | 128x128 | 1 | H (variant 2) | 1 |
| **L6 toplam** | | **3** | | **3** |

### Toplam (Aşama 1)
- **27 PixelLab credit (MCP)** + **2-3 Web UI manuel** + **3 mevcut reuse**
- Toplam asset: **34 sprite**
- Tahmini süre: ~2-3 saat üretim + import

---

## 4. Pair E/F Tileset (Aşama 2, defer)

| Pair | Floor base | Feature | Tool | Boyut |
|---|---|---|---|---|
| **Pair E** rubble↔cliff_drop | Shattered Keep rubble | Cliff drop (yükseklik) | PixelLab Pro tileset (16 corner Wang) | 32x32 per tile |
| **Pair F** rubble↔water_pool | Shattered Keep rubble | Water pool (derinlik) | PixelLab Pro tileset (16 corner Wang) | 32x32 per tile |

Aşama 1 LOCK sonrası dispatch — `STAGING/codex_pair_ef_tileset_gen.md` ayrı task.

---

## 5. Unity Wire-up (Asset gen sonrası)

Sırasıyla:

1. **Wall sprite import** → `Assets/Art/Tiles/F1/Wall/`
   - Pixel Per Unit: 32, Filter: Point, Compression: None
   - `WallBrushSetSO` asset oluştur, 14 sprite atan
2. **L4/L5/L6 sprite import** → `Assets/Art/Tiles/F1/Decals/`
   - Aynı import ayarları
   - 3 yeni `PatchAtlasSO` (Transition / Detail / Accent), her birinde uygun sprite + PatchEntry parametreleri
3. **RoomRecipe biome wire**
   - `Shattered_Keep_F1_BiomePreset.asset` veya RoomRecipe'a:
     - `wallBrushSet` → yeni WallBrushSetSO
     - `transitionAtlas` → moss/dirt patch atlas
     - `decalAtlas` → crack+rubble atlas
     - `accentAtlas` → rift atlas
4. **PatchAtlasSO parametre default'ları** (ChatGPT large-to-small hierarchy):
   - **Transition (L4):** density 0.08, edgeBiased=true, wallProximityFactor=1.5, centerPathDensityReduction=0.05, minDistance=6 tile, allowFlipX=true
   - **Detail (L5):** density 0.18, edgeBiased=true, wallProximityFactor=1.2, centerPathDensityReduction=0.08, minDistance=2 tile, allowFlipX=true, allowFlipY=true
   - **Accent (L6):** density 0.03, edgeBiased=false, encounterAvoidRadius=4, minDistance=12 tile
5. **MapLayerOrchestrator config** test scene'inde:
   - L1/L2/L3 toggle ON, L4/L5/L6 toggle ON
   - Seed: 12345 (deterministic test)
6. **Game window screenshot** → before/after compare

---

## 6. Success Criteria (ChatGPT'den)

- ✓ Readable gameplay space preserved (encounter zones temiz, center 4x4 sparse)
- ✓ Tile repetition gizlenmiş (L4 oval brushes + L2 4 variant)
- ✓ Doğal görünüm (edge-biased + minDistance + flipX/Y)
- ✓ Large organic regions (L4 256/512) + tiny details (L5 32-128)
- ✓ NO square overlay borders (asset gen prompt'larda explicit)
- ✓ NO AI texture soup (prompt'larda "pixel clusters not blur", "no anti-aliasing")
- ✓ Collision clean (overlay'lerden gelmiyor)

---

## 7. Notes

- **5-layer ChatGPT spec vs 6-layer RIMA:** ChatGPT'de wall yok (floor-only). Bizde L3 wall overlay (Hades cap) eklendi. Wall = SpriteRenderer overlay (NEVER tileset, Karar #143-C).
- **Rectangular AR (16:9 horizontal, 2:3 vertical) wall için kritik.** 256x256 square wall = grid hissi. Studio doc `layered_environment_pipeline.md` Bölüm 6'da explicit.
- **MCP create_object max 256:** Large 512x512 brush sadece Web UI Pro.
- **Walkable filter Karar #143-D zorunlu** — TÜM painter'larda otomatik (kod-side check).
- **encounterAvoidRadius LIVE** (PatchAtlasSO field) — encounter slot etrafı temiz kalır.
- **minDistance LIVE** (PatchEntry field) — aynı decal türü minimum tile mesafede tekrar etmez (clumping önler).
