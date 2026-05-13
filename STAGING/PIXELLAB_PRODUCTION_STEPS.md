# PixelLab Uretim -- Alabaster Dawn Asset Model

**Son guncelleme:** 2026-05-13 (S67 revize)
**Strateji:** Karar #118 + Alabaster Dawn philosophy -- minimal ham asset + Room Designer procedural composition (Faz 1.0 commit a4757ae)

---

## DEPRECATED (S66 64-batch sub-style mix)

Eski 192-sprite spec (64 floor + 64 wall + 64 decor mixed sub-style) IPTAL. Sebep:
- PixelLab Create Tiles PRO 32px'te max 16 variant dondurur (64 degil)
- Sub-style mix tek pool'a girince Unity RandomTile clean ile full-moss yan yana koyar -- kaotik
- Room Designer Faz 1.0 RandomTile + Wang + decal layer composition'i halleder; tek tile'da bake gereksiz

---

## YENi MODEL -- 46 unique sprite, 6 batch, ~120 kredi

| Batch | Tool | Icerik | Adet | Tahmini Kredi |
|---|---|---|---|---|
| 1 | create_tiles_pro | F1-Base-Clean floor | 8 | ~15 |
| 2 | create_object | F1-Decal-Moss (transparent BG) | 8 | ~15 |
| 3 | create_object | F1-Decal-Rift (transparent BG) | 8 | ~15 |
| 4 | create_object | F1-Decal-Dust (transparent BG) | 6 | ~12 |
| 5 | create_tiles_pro | F1-Wall-Clean variant + corner | 6 | ~15 |
| 6 | create_object | F1-Props | 10 | ~25 |
| **TOPLAM** | | | **46** | **~97-120** |

---

## Roller -- Asset vs Composition

| Gorev | Kim yapar | Notlar |
|---|---|---|
| Ham sprite uretimi | PixelLab / Aseprite | Clean floor, moss/rift/dust decal, wall, prop |
| RandomTile variant secimi | Room Designer (Faz 1.0) | FloorVariantPainter LUT tabani |
| Wang autotile dikiş | Room Designer | WallAutoConnect bake -- 4-bit NSEW mask |
| Decal scatter + asymmetric weathering | Room Designer | Overlay tilemap katmani |
| Runtime aydinlatma | Unity URP 2D Light | Point Light 2D prop-anchored, floating light YASAK |

## Wang Transition Seam Outline -- ZORUNLU

PixelLab Wang transition prompt'larina su mandatory clause eklenecek:

```
1px lighter outline on upper terrain seam (HSV +10%), 1px darker outline on lower terrain seam (HSV -10%), crisp edges no muddy pixel blend
```

Not: Mevcut F1 Wang tile sheet'ler otomatik re-generate edilmeyecek. Bir sonraki PixelLab batch'inde bu clause ile yeniden uretilecek.

---

## BATCH 1: F1-Base-Clean (8 sade clean floor)

**Tool:** create_tiles_pro (PixelLab web UI)

| Alan | Deger |
|---|---|
| Tile type | square_topdown |
| Tile size | 32 |
| View angle | 35 (slider) |
| Thickness | 0% |
| Outline mode | segmentation |
| Style Tiles | BOŞ BIRAK |

**Prompt:**

```
Clean weathered dark stone floor for a shattered keep dungeon, 32x32 top-down pixel art tiles,
high top-down angle ~35 degrees (Hades reference). Generate 8 distinct floor tile variants,
all SAME terrain type (no moss, no rift cracks -- pure clean floor only):

1). Flat charcoal flagstone, worn cracked mortar, chipped slab edges, base #2C2A2A, cold blue shadow tint #7BA7BC, minimal dust, foot-traffic polish
2). Uneven flagstone with slight height variation, deeper shadow in crevice #4A3F3F, sparse stone chips at corners
3). Large single-slab tile, hairline spider-web surface crack, near-pristine #2C2A2A, very subtle cold blue rim
4). Two-slab layout, mortar line off-center, asymmetric edge wear, base #2C2A2A
5). Rough-hewn stone, deep irregular mortar channels, base #2C2A2A, cool shadow pools #4A3F3F
6). Polished central zone with worn perimeter, faint gloss dithering, base #2C2A2A to #4A3F3F
7). Mottled surface with cold mineral staining, trace dust accumulation, base #2C2A2A
8). Worn threshold stone, directional scratch marks, edge chips, base #2C2A2A

STRICT PALETTE: ONLY #2C2A2A dark rubble base, #4A3F3F deep shadow, #7BA7BC cold blue rim.
NO moss, NO rift cracks, NO props, NO walls, NO transparent areas.
Each tile fully tileable on all 4 edges.
Mat painterly pixel art, dark gritty palette, pixel-honest dithering only, no anti-aliasing.
Salt and Sanctuary chibi-but-serious tone + Hades theatrical mythic mood.
```

**Kaydet:** `STAGING/TILESET_OUTPUT/F1_Floor_Clean_8/floor_clean_8.png`

---

## BATCH 2: F1-Decal-Moss (8 moss patch, transparent BG)

**Tool:** create_object (PixelLab web UI)

| Alan | Deger |
|---|---|
| Directions | 1 |
| Default Style View | Top-Down |
| Size | 32 |

**Prompt (Object Description ust alan):**

```
Single moss patch decal for dungeon floor overlay, 32x32 transparent background,
top-down ~35 degree angle. Isolated organic shape, NO baked floor underneath.
Dark gritty palette, pixel art, mat painterly, no anti-aliasing.
Unity URP 2D Light handles all lighting -- no shadow baked into sprite.
```

**Her item icin (64 obje alani -- sadece 8 doldur, geri bos birak):**

```
1). small moss patch, cold grey-green lichen cluster, irregular amoeba shape, sparse coverage
2). medium moss patch, denser cold grey-green lichen with tiny fern tufts at edges
3). large moss patch, layered lichen mat, dark centre #2C2A2A showing through at gaps
4). dry moss patch, withered grey-green, crumbling edges, sparse bleached tips
5). corner moss cluster, wedge-shaped lichen growth, hugs an implied corner
6). moss tuft thin strip, narrow elongated growth, fits along a wall edge
7). moss with mineral stain, cold grey-green lichen over dark mineral seep mark
8). heavy moss mat, near-full coverage, small stone chips visible through growth
```

**Kaydet:** `STAGING/PROP_OUTPUT/F1_Decal_Moss_8/decal_moss_8.png`

---

## BATCH 3: F1-Decal-Rift (8 cyan rift crack + rune dust, transparent BG)

**Tool:** create_object

**Prompt (Object Description ust alan):**

```
Single rift crack or rune dust decal for dungeon floor overlay, 32x32 transparent background,
top-down ~35 degree angle. Isolated shape, NO baked floor underneath.
Cyan-violet rift accent (#00FFCC-adjacent, muted), silver rune dust fragments.
Dark gritty pixel art, mat painterly, no anti-aliasing, no bright glow baked in sprite.
Unity URP 2D Light handles glow -- sprite is the shape only.
```

**Her item icin:**

```
1). short rift crack, cyan-violet hairline, straight with slight jag
2). long rift crack, cyan-violet hairline, branching Y-shape, asymmetric
3). curved rift crack, cyan-violet diagonal sweep, tapers at ends
4). rift fragment cluster, small floating cyan-violet dust motes, scattered irregular
5). rune dust patch, silver-cyan glyph fragment half-buried in implied floor, illegible
6). rift seam wide, two parallel hairlines with void-dark gap between, faint inner glow shape
7). rift crack with rune dust, branching crack plus scattered silver fragment trail
8). micro rift cluster, three short hairline cracks radiating from a centre point
```

**Kaydet:** `STAGING/PROP_OUTPUT/F1_Decal_Rift_8/decal_rift_8.png`

---

## BATCH 4: F1-Decal-Dust (6 dust/ash overlay, transparent BG)

**Tool:** create_object

**Prompt (Object Description ust alan):**

```
Single dust or ash overlay decal for dungeon floor, 32x32 transparent background,
top-down ~35 degree angle. Isolated shape, NO baked floor underneath.
Pale grey ash, fine stone chips, dark gritty palette, mat pixel art, no anti-aliasing.
```

**Her item icin:**

```
1). fine grey ash pile, circular low mound, soft dithered edge
2). ash trail, elongated directional drift, tapers at one end
3). stone chip scatter, 4-6 tiny irregular fragments, ivory-grey
4). mixed dust and chip, ash pool with stone chip cluster at centre
5). soot streak, charcoal-black elongated mark, smoke residue
6). ash drift thin, narrow wisp-shaped deposit along implied edge
```

**Kaydet:** `STAGING/PROP_OUTPUT/F1_Decal_Dust_6/decal_dust_6.png`

---

## BATCH 5: F1-Wall-Clean (6 wall variant + corner)

**Tool:** create_tiles_pro

**Form ayarlari Batch 1 ile ayni** (square_topdown / 32 / 35 / 0% / segmentation / Style Tiles bos)

**Prompt:**

```
Clean dark stone wall variants for a ruined keep, 32x32 top-down pixel art tiles,
high top-down angle ~35 degrees. Generate 6 wall tile variants, all SAME terrain
(no rune carvings, no heavy damage -- clean intact masonry only):

1). Standard wall tile, raised rough stone blocks, shadow crevice #4A3F3F, cold blue rim #7BA7BC
2). Wall tile with vertical mortar line off-center, subtle weathering chips
3). Larger block face, single horizontal crack near top edge, base #4A3F3F
4). Corner wall tile (implied 90-degree exterior corner), two faces meeting, asymmetric shadow
5). Wall end cap, one open edge (floor-facing), mortar exposed at cut edge
6). Wall with minor soot stain from old fire, dark streak on base #4A3F3F, no structural damage

STRICT PALETTE: ONLY #4A3F3F dark wall stone, #2C2A2A deep crevice, #7BA7BC cold blue rim.
NO moss, NO rune carvings, NO rift cracks, NO characters, NO floor.
Each tile fully tileable on relevant edges. Full coverage no transparent areas.
Mat painterly pixel art, pixel-honest dithering, no anti-aliasing.
```

**Kaydet:** `STAGING/TILESET_OUTPUT/F1_Wall_Clean_6/wall_clean_6.png`

---

## BATCH 6: F1-Props (10 prop)

**Tool:** create_object

| Alan | Deger |
|---|---|
| Directions | 1 |
| Default Style View | Top-Down |
| Size | 32 |

**Prompt (Object Description ust alan):**

```
Single environmental prop for a shattered keep dungeon, 32x32 transparent background,
top-down ~35 degree angle (Hades reference). Single isolated object.
Dark gritty palette (#2C2A2A, #4A3F3F, #7BA7BC cold blue, occasional #C4682A rust).
Mat painterly pixel art, no anti-aliasing, no shadow baked (Unity URP 2D Light).
Salt and Sanctuary chibi-but-serious tone. Asymmetric weathering on each prop.
```

**Her item icin:**

```
1). small iron chest, dark metal, rusted hinges, closed, tarnished surface
2). small clay urn, terracotta, cracked rim, dust-coated
3). lit brazier small, dark iron bowl, cold blue flame (no orange glow baked)
4). small skull, human, weathered ivory, jaw slightly displaced
5). scroll rolled, dark parchment with cracked red wax seal
6). broken sword fragment, snapped blade half-buried in implied floor
7). small rubble pile, 3-4 mixed masonry chunks with dust
8). chain segment short, rusted iron links, coiled loosely
9). ritual stone small, carved sigil, muted silver-cyan accent (no bright glow baked)
10). interactive chest gameplay, dark wood with iron bands, intact closed, slightly larger than item 1
```

**Kaydet:** `STAGING/PROP_OUTPUT/F1_Props_10/props_10.png`

---

## Split-Animation Workflow (Karar #120)

Trigger: frame budget >= 12f VE net tek apex frame.

Adimlar:
1. Apex frame # ve pose tanimla (kaydet: apex_frame, apex_state_id per direction)
2. Create State ile apex pose uret (20-40 gen, 8 yon per Karar #108+#114)
3. Part 1 Custom V3: frames 1..APEX, End Frame = Apex State
4. Part 2 Custom V3: frames APEX..N, First Frame = Apex State
5. Aseprite: Part 1 full + Part 2 frame 2..N, apex bir kez, label fNN_APEX

Export QC (5 madde -- PASS olmadan commit yok):
[ ] Part 1 son frame hash == Part 2 ilk frame hash (per direction)
[ ] Sheet frame count per row == N
[ ] Exactly 1 fNN_APEX per direction
[ ] Frame order: Part 1 full + Part 2 frames 2..N
[ ] Apex frame hash == Stage 1 apex_pixel_hash

Part 1 + Part 2 prompt sablonu (weapon animasyonlari icin Karar #71 blok):
[motion description]
weapon gripped and attached every frame, weapon hand fingers and wrist fixed on haft every frame,
during collapse weapon follows hand as one rigid unit, [body-part lock list per drift rules]

---

## Yapma Listesi

- Floor tile'a moss / rift / dust BAKE -- decal overlay katmanina ayir (Room Designer halleder)
- Tile icinde light bake -- Unity URP 2D Light runtime (Point Light 2D prop-anchored)
- 64 variant istemek -- PixelLab 32px = max 16 tile dondurur; batch basina 6-8 hedefle
- View angle 30 derece -- 35 derece LOCKED (Karar #113)
- Tek pool'a sub-style mix -- clean / moss / rift her biri ayri pool veya decal katman
- PixelLab Map editor'unde harita cizme -- mockup bile Unity'de (Karar #118 disiplin)
- Style reference olmadan generate -- palette drift eder; pilot tile set sonrasi style_ref ver
- Outline mode single_color veya selective -- segmentation veya lineless kullan (dark gritty)
- Floating light -- her Point Light 2D'nin gorunur prop sahibi olmali (REF_NUGGETS kural)

---

## Per-Batch QC Checklist

1. Palette discipline -- #2C2A2A / #4A3F3F / #7BA7BC paletinden cikmamis mi?
2. Tile view -- pure high top-down floor (Batch 1/5); no vertical wall in floor tiles
3. Raggedness yuzde 40 veya uzeri (Karar #116) -- tile kenarlari dogal, grid-block hissi yok
4. Decal transparent BG, no baked floor under (Batch 2/3/4)
5. Asymmetric weathering -- simetrik pattern, merkeze kilitlenen dekor yok
6. Tileable (Batch 1/5) -- 4-edge seamless, yan yana koyunca dikis okunmuyor
7. No banned content -- kan splatter, parlak cartoon renk, outline cartoon, gradient yok

Tum 7 PASS olursa -- Next Batch veya Aseprite cleanup + Unity import.

---

## Yardimci Scriptler

**Wang sheet'ten 32px tile kesimi:**
```bash
python tools/slice_wang_sheet.py "<sheet.png>"
```
Otomatik 16 tile + 4 style_ref klasoru uretir.

**Buyuk gorseli resize (Claude okuyabilmek icin):**
```bash
python tools/resize_image.py "<image.png>"
```
Aspect-ratio koruyarak max 1800px'e indirir.

---

**Revize tarihi:** 2026-05-13 (S67)
**Revize sebebi:** Faz 1.0 Room Designer (commit a4757ae) procedural composition'i sagliyor -- floor/wall/decal katman ayirimi Room Designer gorevidir, PixelLab sadece minimal ham asset uretir. 192-sprite overkill spec kaldirildi.
