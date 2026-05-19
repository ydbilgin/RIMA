# PixelLab Production Guide — RIMA Map Designer v2

**Hedef:** Alabaster Dawn / CrossCode pixel art aile, RIMA chibi karakter ile %100 stil uyum. Karar #143 6-layer pipeline + Karar #74 PPU=64 LOCK.

**Stil invariants (her prompt'ta tut):**
- Pixel art native, hard pixel edges, NO anti-aliasing
- Max 2-3 tone per color region, 1px subtle silhouette outline
- High top-down 30-35° angle (Karar #100, NOT pure 90°, NOT isometric)
- **Palette:** dark slate gray `#3a3530` + deep brown `#4a3a30` + dusty blue `#5a6a78` + moss green `#3a5a3a` + faint dark red rift `#8a3030` + warm orange fire `#c87830` + cold blue rim `#8aa8c0`
- Mood: weathered ritual aftermath, ancient temple, hollow watchful

---

## TOOL SELECTION (her layer için)

| Layer | PixelLab Tool | Native Size | Variant Strategy |
|---|---|---|---|
| **L2 Floor (8 var)** | `create_tiles_pro` square_topdown | 64×64 | 1 call → 8 numbered variants |
| **L3 Wang16 Walls (16 var)** | `create_topdown_tileset` Wang autotile | 32×32 native + 2x upscale → 64×64 effective wall body (cap separate) | 1 mega call → 16+7 transitions |
| **L4 Decals (8 var)** | `create_object` n_frames=16 review | 64×64 transparent | 1 call per type, pick 4-5 from 16 |
| **L5 Detail (6 var)** | `create_object` n_frames=16 review | 32×32 transparent | 1 call per type, pick from 16 |
| **L6 Accents (4 var)** | `create_object` n_frames=16 review | 128×128 transparent | 1 call per type, pick |
| **Props (12 var)** | `create_object` n_frames=16 review | 64×64 transparent (banner 64×128 custom) | 1 call per prop, pick 1-2 |

**Toplam call count tahminim:** 1 (floor) + 1 (Wang16) + 8 (decal) + 6 (detail) + 4 (accent) + 12 (prop) = **32 PixelLab call** → 5000 budget'ın <1%.

---

## L2 FLOOR — 1 mega call ile 8 variant

**Tool:** `create_tiles_pro` square_topdown 64×64

**Parameters:**
```yaml
tile_type: "square_topdown"
tile_size: 64
tile_view: "high top-down"
tile_view_angle: 35
outline_mode: "segmentation"
no_outline: false
```

**Numbered prompt (tek block — `tiles_pro` numbered format):**

```
1). Clean weathered stone floor tile, dark slate gray base with deep brown undertone, slight grain variation, ancient ritual temple atmosphere, hard pixel edges, max 3 tones, 1px subtle outline, top-down 35 degree angle, seamless tileable

2). Stone floor tile with sparse moss patch, deep moss green organic spot blending into dark stone, weathered ancient feel, hard pixel edges, max 3 tones, top-down 35 degree

3). Cracked stone floor tile, thin hairline fractures with darker shadow lines, subtle dust accumulation, hard pixel edges, max 3 tones, top-down 35 degree

4). Worn smooth stone floor tile, polished by foot traffic, faint cold blue rim highlight, hard pixel edges, max 3 tones, top-down 35 degree

5). Stained stone floor tile, dusty blue mineral residue, faint abstract sigil-like discoloration, hard pixel edges, max 3 tones, top-down 35 degree

6). Rift-touched stone floor tile, hairline cracks with cold blue glow at crack edges, weathered ancient temple, hard pixel edges, max 3 tones, top-down 35 degree

7). Dirt-covered stone floor tile, organic earth tones over dark stone, weathered atmosphere, hard pixel edges, max 3 tones, top-down 35 degree

8). Faded blood stain floor tile, dark crimson aged blood stain on dark stone, ritual aftermath, hard pixel edges, max 3 tones, top-down 35 degree
```

**Output:** 8 tile (numbered grid). Unity import: `Assets/Sprites/Environment/RIMA_Pixel_Pack_v1/floor/`

---

## L3 WANG16 WALLS — 1 mega tileset call

**Tool:** `create_topdown_tileset` (native Wang auto-tile)

**Parameters:**
```yaml
tile_size: 32          # native max (32 → Unity 2x effective)
lower_description: |
  dark weathered stone floor, deep brown-gray base, matches floor tile palette
upper_description: |
  ancient weathered stone wall block, dark gray stone with sparse moss at base, faint cold blue rim highlight on top edge, hard pixel edges, ritual temple atmosphere, max 3 tones, 1px outline
transition: true       # 7 transition tiles include
view: "high top-down"
view_angle: 35
```

**Output:** 16 Wang16 wall + 7 floor-wall transitions = 23 tile native 32×32.

Unity import: Brush V1 BrushAtlasImporter scan `Assets/Sprites/Environment/RIMA_Pixel_Pack_v1/walls/`, Wang config auto-detected.

**Pixel size:** 32×32 native → Unity PPU = 32 (or scale 2x = 64 effective). Karar #143 spec 64×96 wall ile uyum: 32×48 tile at PPU=32 = 1×1.5 unit (64×96 effective). Brush V1'da custom PPU per tileset desteklenir.

---

## L4 ORGANIC DECALS — 8 ayrı `create_object` call

**Tool:** `create_object` standard

**Per-call parameters:**
```yaml
directions: 1
n_frames: 16
view: "high top-down"
object_view: "top-down"
size: 64
transparent_background: true
```

**Per-type prompts (8 ayrı call):**

### D1 — Moss Tuft Small
```
Small moss tuft cluster, deep moss green organic blob shape, transparent background, top-down 35 degree view, soft edge blend, pixel art, hard edges within pixel grid, max 3 tones, NO anti-aliasing, ritual temple atmosphere

Negative Prompt :
character, creature, weapon, isometric, 3d render, painterly soft gradient, anime, cartoon bright, anti-aliasing, watermark
```

### D2 — Moss Patch Large
```
Large moss patch with creeping edges, dense deep moss green organic spread, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, ritual temple

[same negative]
```

### D3 — Dirt Stain
```
Irregular dirt stain, brown organic patch with darker shadows, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, weathered

[same negative]
```

### D4 — Wet Patch
```
Dark wet damp stain, irregular shadow on stone, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, atmospheric

[same negative]
```

### D5 — Grass Tuft
```
Small grass tuft cluster, green organic spread, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, ancient ruin atmosphere

[same negative]
```

### D6 — Vine Patch
```
Creeping vine fragment, dark green tendrils with small leaves, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones

[same negative]
```

### D7 — Ash Patch
```
Grey ash residue patch, irregular dust spread, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, post-battle aftermath

[same negative]
```

### D8 — Blood Patch
```
Old blood stain patch, dark crimson aged blood, irregular organic shape, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, ritual aftermath

[same negative]
```

**Output per call:** 16 variants → user pick best 4-5 → `select_object_frames` → 4-5 separate PNG.

Unity import: `Assets/Sprites/Environment/RIMA_Pixel_Pack_v1/decals_L4/`

---

## L5 DETAIL SCATTER — 6 ayrı `create_object` call

**Tool:** `create_object`, **size: 32×32**

**Per-call parameters:**
```yaml
directions: 1
n_frames: 16
view: "high top-down"
object_view: "top-down"
size: 32
transparent_background: true
```

**6 types:**

1. **Pebbles:** `Small dark stones cluster scattered, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative as L4]`

2. **Crack Pattern:** `Hairline crack pattern, thin darker lines on stone, transparent background, pixel art hard edges, max 3 tones [negative]`

3. **Bone Fragment:** `Single weathered bone shard, light grey bone on transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

4. **Debris:** `Small rubble cluster, broken stone fragments, transparent background, pixel art hard edges, max 3 tones [negative]`

5. **Dust:** `Subtle dust patch, light grey particle scatter, transparent background, pixel art hard edges, max 3 tones [negative]`

6. **Burn Mark:** `Small scorch mark, charcoal black with ember edges, transparent background, pixel art hard edges, max 3 tones [negative]`

Unity import: `Assets/Sprites/Environment/RIMA_Pixel_Pack_v1/detail_L5/`

---

## L6 LARGE ACCENTS — 4 ayrı `create_object` call

**Tool:** `create_object`, **size: 128×128**

**Per-call parameters:**
```yaml
directions: 1
n_frames: 16
view: "high top-down"
object_view: "top-down"
size: 128
transparent_background: true
```

**4 types:**

1. **Rift Scar:**
```
Large dark crimson irregular multi-blob with radial cracks, cold blue rim glow at crack edges, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones per color, ritual catastrophe mood

Negative Prompt :
character, creature, isometric, 3d render, painterly gradient, anime, cartoon bright, anti-aliasing, watermark, soft edges
```

2. **Battle Aftermath:**
```
Blood splatter combined with dust cloud, organic dark stain shape, dark crimson + grey dust, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, post-battle ritual aftermath

[same negative]
```

3. **Scorch Burn:**
```
Large burned area, charcoal black center fading to warm ember orange edges, irregular blob shape, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones

[same negative]
```

4. **Ritual Circle:**
```
Faded ritual circle with abstract sigil markings, dusty blue and faint cold blue, irregular ring pattern with inner symbols, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, ancient ritual ground

[same negative]
```

Unity import: `Assets/Sprites/Environment/RIMA_Pixel_Pack_v1/accents_L6/`

---

## PROPS — 12 ayrı `create_object` call

**Tool:** `create_object`, **size: 64×64 (banner exception: 64×128 Custom Size Beta)**

**Per-call parameters:**
```yaml
directions: 1
n_frames: 16
view: "high top-down"
object_view: "top-down"
size: 64
transparent_background: true
```

**12 props:**

1. **Wooden Crate:** `Wooden crate with iron banding, dark brown wood, weathered, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

2. **Stone Urn Intact:** `Ancient stone urn intact, grey-brown stone with simple ritual carvings, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

3. **Stone Urn Broken:** `Broken stone urn fragments, grey-brown stone shards arranged on ground, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

4. **Wooden Barrel:** `Wooden barrel with iron bands, dark brown wood, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

5. **Candle Holder:** `Iron candle holder with lit candle, dark iron base with warm flame, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, atmospheric light source [negative]`

6. **Burning Brazier:** `Iron tripod brazier with bright fire, dark iron stand with warm orange flame and ember glow, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

7. **Hanging Banner:** `Torn red banner hanging vertical, dark crimson fabric with iron mount at top, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones — Custom Size 64x128 Beta [negative]`

8. **Stone Column Intact:** `Intact stone column with weathered surface, vertical pillar, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

9. **Broken Pillar:** `Broken pillar fragment lying on ground, weathered stone, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

10. **Chest Closed:** `Closed wooden treasure chest with iron bands, dark brown wood, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

11. **Chest Open:** `Open wooden chest revealing gold coins inside, dark wood with bright gold contents, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones [negative]`

12. **Statue Torso:** `Ancient stone statue torso humanoid bust, grey stone with weathered surface, transparent background, top-down 35 degree, pixel art hard edges, max 3 tones, ritual temple atmosphere [negative]`

Unity import: `Assets/Sprites/Environment/RIMA_Pixel_Pack_v1/props/`

---

## UNITY BRUSH V1 WIRE-UP (üretim sonrası)

### Klasör yapısı (PixelLab sonrası)
```
Assets/Sprites/Environment/RIMA_Pixel_Pack_v1/
  ├── floor/        (8 PNG, 64×64 native)
  ├── walls/        (16 Wang16 + 7 transition = 23 PNG, 32×32 native or 64×96 effective)
  ├── decals_L4/    (8 type × 4-5 pick = 32-40 PNG, 64×64)
  ├── detail_L5/    (6 type × 4-5 pick = 24-30 PNG, 32×32)
  ├── accents_L6/   (4 type × 4-5 pick = 16-20 PNG, 128×128)
  └── props/        (12 PNG, 64×64 + banner 64×128)
```

### Brush V1 importer otomasyonu

1. **Sprite import settings (automated by Editor script):**
   - Floor/Decal/Accent: PPU=64, FilterMode=Point, alphaIsTransparency=true, max 1024
   - Walls (32×32 native): PPU=32 (or use 64 with 2x scale)
   - Props: PPU=64, FilterMode=Point

2. **BrushAtlasImporter scan:**
   - `RIMA → MapDesigner → Import Tile Pack → RIMA_Pixel_Pack_v1`
   - Auto-detect Wang config from filename pattern (`wang_XXXX`)
   - Auto-create Sprite assets + Tile assets (TileBase)

3. **PatchAtlasSO setup (L4/L5/L6):**
   - Editor menu: `RIMA → MapDesigner → Create PatchAtlas`
   - L4 → density 0.18, edgeBiased, minDistance 2
   - L5 → density 0.18, allowFlipX/Y, minDistance 1
   - L6 → density 0.03, minDistance 12, encounterAvoidRadius 4

4. **PropDefinitionSO setup:**
   - 12 prop = 12 PropDefinitionSO
   - Footprint config per prop (crate 1×1, banner 1×2 etc.)

5. **RoomBank entry:**
   - New biome `RimaPixelPack_v1` in RoomBank
   - 10 sample rooms updated with new tile references
   - Brush V1 RoomDesigner UI compose

### Compose sample room (Brush V1)
- `RIMA → MapDesigner → Open Brush Window`
- Select biome `RimaPixelPack_v1`
- Load sample `RoomTemplate_Spawn`
- Brush V1 auto-paints L1 base + L2 floor + L3 walls + L4/L5/L6 procedural
- Manual: drop props from PropDefinitionSO library
- Screenshot Game View

---

## ÜRETİM SIRASI (öncelik)

| Sıra | İş | Tool | Süre |
|---|---|---|---|
| 1 | **L2 Floor** 1 mega call (8 variant) | `create_tiles_pro` | ~2 dk |
| 2 | **L3 Wang16 Walls** 1 mega call (16+7 = 23) | `create_topdown_tileset` | ~3 dk |
| 3 | **Props batch 1** (crate, urn intact, candle, brazier) | `create_object` × 4 | ~8 dk |
| 4 | **L6 Accent rift scar** (en kritik atmosfer) | `create_object` × 1 | ~2 dk |
| 5 | **L4 Decal moss + dirt** (2 type, en sık kullanılan) | `create_object` × 2 | ~4 dk |
| 6 | **Props batch 2** (urn broken, barrel, banner, column) | `create_object` × 4 | ~8 dk |
| 7 | **L5 Detail pebbles + cracks** (2 type) | `create_object` × 2 | ~4 dk |
| 8 | **L4 Decal kalan** (6 type) | `create_object` × 6 | ~12 dk |
| 9 | **L5 Detail kalan** (4 type) | `create_object` × 4 | ~8 dk |
| 10 | **L6 Accent kalan** (3 type) | `create_object` × 3 | ~6 dk |
| 11 | **Props batch 3** (chest x2, statue, pillar) | `create_object` × 4 | ~8 dk |

**Toplam:** ~65 dakika PixelLab call süresi, **5000 budget'ta ~100 gen** harcanır.

---

## ÖNCELİK YORUMU

İlk 4 adım (Floor + Wang16 + Rift Accent + 4 prop) ~15 dk → **minimum viable sample room** için yeterli. Sen Web UI'da bu sırayla başla, ben aralarda paralel Unity import script'i + Brush V1 wire-up'ı hazırlayım.

**İlk dispatch'te ne yapacaksın?** `create_tiles_pro` L2 Floor mu, yoksa `create_topdown_tileset` L3 Wang16 mu? İkincisi Brush V1 wang16 zorunlu bağımlılığı, ilkinde olmadan compose yapılamaz.

**Önerim:** Sıra 1 → Sıra 2 (Floor sonra Wang16, Brush V1 atlas import ikisini de bekler).

Hangisini Web UI'da başlatmaya hazırsın?
