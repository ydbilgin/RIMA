# PixelLab Üretim — Adım Adım Talimatlar

**Son güncelleme:** 2026-05-13 (S66 sonu, S67 başlangıcı için hazırlandı)
**Strateji:** Karar #118 (Hybrid Tile Composition)
**Style Reference dosyaları:** `C:\Users\ydbil\Downloads\PixelLab_Map_Cozumu\asset_006_sliced\style_refs\` (4 tile)

---

## ⚙️ HER BATCH İÇİN ORTAK KURAL

**Style Reference Images** alanına HER ZAMAN şu 4 tile'ı yükle:
- `style_ref_00.png` — saf zemin
- `style_ref_05.png` — transition iç köşe
- `style_ref_10.png` — transition iç köşe
- `style_ref_15.png` — saf duvar

(Palette tutması için kritik — atlama.)

---

## 🟢 BATCH 1: Floor Variant 64 — `create_tiles_pro`

### Adım Adım
1. PixelLab web UI'da **Create tiles PRO** tool'unu aç (Map editor değil)
2. Form'a şunları gir:

| Alan | Değer |
|---|---|
| Tile type | **square_topdown** |
| Tile size | **32** |
| View angle | **35°** (slider) |
| Thickness | **0%** |
| Outline mode | **segmentation** |

3. **Style Tiles** alanına yukarıdaki 4 style_ref PNG'sini yükle
4. **Description** alanına yapıştır:

```
Dark rubble stone floor variations for a shattered keep, 32x32 top-down pixel art tiles viewed from ~35 degrees high top-down angle (Hades reference). Generate a natural varied set where each tile is a distinct flagstone arrangement on the same shared material — they must read as belonging to the same dungeon floor but each carries different character.

Mix freely across these states: clean weathered flagstones with light dust; cracked variants with hairline cyan rift dust seeping into cracks; moss-covered variants with cold grey-green lichen patches in corners and crevices; rune-dust scattered variants with faint silver sigil fragments half-buried; damp shaded variants with subtle moisture sheen and mineral staining; broken rubble variants with chipped slab edges and small debris piles; foot-traffic polished variants with smoother centers; lichen-fern fringe variants where moss spreads to plant tufts.

Each tile asymmetric weathering — cracks off-center, debris randomly clustered, moss patches irregular shape. NO copy-paste micro-detail between tiles, no uniform grid alignment, no perfect borders. Tiles must blend seamlessly when placed adjacent: shared palette discipline (#2C2A2A dark rubble base, #4A3F3F shadow, #7BA7BC cold blue rift accent, occasional pale grey-green lichen, occasional silver rune dust), shared texture vocabulary, shared lighting direction (subtle top-left).

Mat painterly pixel art, dark gritty palette, heavy texture, no anti-aliased gradients, pixel-honest dithering. Vivid Vulnerability mood — Salt and Sanctuary chibi-but-serious + Hades theatrical mythic tone. Ritual catastrophe aesthetic (cyan-violet rift only, NO blood, NO gore).

Do not include characters, props, walls, transitions to other terrain types — pure floor tile set only, full coverage (no transparent areas), every tile tileable on all 4 edges.
```

5. **Generate** bas
6. ~60-90 sn bekle
7. Çıktıyı (256×256 sheet, 8×8 grid, 64 tile) **Export** veya save image
8. Kaydet: `STAGING/TILESET_OUTPUT/F1_FloorVariants_64batch/floor_64_variants.png`
9. **QC için Claude'a göster** → palette + variant zenginliği + ~35° view angle kontrolü

---

## 🟢 BATCH 2: Wall Variant 64 — `create_tiles_pro` (Floor PASS sonrası)

### Adım Adım
1. **Create tiles PRO** tool'u aç (Batch 1 ile aynı)
2. Form ayarları **Batch 1 ile aynı** (square_topdown / 32 / 35° / 0% / segmentation)
3. **Style Tiles** alanına yükle:
   - `style_ref_00.png`, `style_ref_15.png` (Wang sheet'ten)
   - + **Batch 1'in best 2 floor tile'ı** (Aseprite/PixelOrama'da kırparak — Claude'a göster, hangileri en iyi söylesin)
4. **Description** alanına yapıştır:

```
Dark broken stone wall variations for a ruined keep, 32x32 top-down pixel art tiles viewed from ~35 degrees high top-down angle (Hades reference). Generate a natural varied set of wall surface tiles — all share same fortress masonry vocabulary but each carries different damage character.

Mix freely across these states: clean fortress masonry with subtle weathering; cracked variants with hairline cyan rift dust seeping through mortar gaps; moss-fringe variants with cold grey-green lichen creeping at base; rune-carved variants with faint silver sigil fragments embedded in stone; damaged variants with collapsed block gaps and rubble pile fragments; soot-stained variants from old fires; banner-fragment variants with torn cloth scraps hanging; chained variants with rusted iron loops embedded.

Each tile asymmetric damage pattern — cracks branching off-center, debris clustered irregularly, moss patches edge-only. NO copy-paste micro-detail, no uniform grid, no perfect borders. Tiles must blend seamlessly when adjacent: shared palette discipline (#4A3F3F dark stone base, #2C2A2A deep crevice shadow, #7BA7BC cold blue rim highlight, occasional pale grey-green lichen, occasional rust orange), shared masonry style, shared lighting direction (subtle top-left).

Mat painterly pixel art, dark gritty palette, heavy texture, no anti-aliased gradients, pixel-honest dithering. Vivid Vulnerability mood — Salt and Sanctuary chibi-but-serious + Hades theatrical mythic tone. Ritual catastrophe aesthetic.

Do not include characters, doors, archways, transitions to floor — pure wall surface tile set only, full coverage, every tile tileable on all 4 edges.
```

5. **Generate** → 60-90 sn → sheet kaydet
6. Kaydet: `STAGING/TILESET_OUTPUT/F1_WallVariants_64batch/wall_64_variants.png`
7. **QC için Claude'a göster**

---

## 🟢 BATCH 3: F1 Decor + Decal + Gameplay 64 — `create_object` (Web UI)

### Adım Adım
1. PixelLab web UI'da **Create Object** tool'unu aç
2. Form'da şunları gir:

| Alan | Değer |
|---|---|
| Directions | **1** |
| Default Style View | **Top-Down** |
| Size | **32** (slider) — otomatik 8×8 grid = 64 obje |

3. **Style Reference Images** alanına yükle:
   - `style_ref_00.png`, `style_ref_05.png`, `style_ref_10.png`, `style_ref_15.png` (4 Wang tile)
   - + **Batch 1 ve Batch 2'nin best 2-3 tile'ı** (palette anchor için)

4. **Object Description** (üst, genel alan) yapıştır:

```
Single environmental object or decal for a shattered keep dungeon, 32x32 transparent background top-down pixel art viewed from ~35 degrees high top-down angle (Hades reference). Mat painterly pixel, dark gritty palette (#2C2A2A dark stone, #4A3F3F shadow, #7BA7BC cold blue, occasional cyan-violet rift accent), Salt and Sanctuary chibi-but-serious tone, Vivid Vulnerability mood. Single isolated object, no characters, no walls, no large background. Decal-style (no shadow baked, Unity URP 2D Light handles lighting). Each item asymmetric weathering.
```

5. Arayüz **Describe each item (64)** açacak → her item kutusuna **alttaki listeden ilgili satırı** yapıştır:

### Item 1-20: DECALS (yere serilen overlay)
```
1. small moss patch, cold grey-green lichen cluster, organic irregular shape
2. medium moss patch, denser lichen with small fern tufts
3. dry moss patch, withered grey-green, sparse coverage
4. rift crack, cyan-violet hairline, short jagged line
5. rift crack, cyan-violet hairline, long branching pattern
6. rift crack, cyan-violet hairline, curved diagonal sweep
7. rift fragment cluster, small floating cyan-violet dots, scattered
8. dust pile, fine grey ash with stone chips, light coverage
9. dust trail, elongated grey ash drift, directional
10. blood stain old, dark dried brown patch, irregular splatter
11. grime patch, dark stain on stone, oily texture
12. soot mark, charcoal black streak, smoke residue
13. small ash pile, white-grey volcanic ash mound
14. broken pottery fragment, terracotta shard scattered, 3-4 pieces
15. spilled candle wax, off-white dried wax pool
16. ink spill, dark blue-black liquid stain
17. small bone fragments, ivory white shard scatter
18. scratched stone overlay, hairline gouges across floor
19. footprint mark, dark boot tread on dust, single track
20. ritual sigil residue, faint silver-cyan glow pattern, half-erased
```

### Item 21-50: SMALL PROPS
```
21. small wooden barrel, weathered planks, iron bands
22. small iron chest, dark metal, rusted hinges, closed
23. small clay urn, terracotta, cracked rim
24. broken stone urn, fragments scattered around base
25. lit candle, white wax stub, small flame
26. unlit candle, white wax stub, melted top
27. candle holder, dark iron base with candle
28. small skull, human, weathered ivory
29. small bone pile, mixed fragments, vertebrae and ribs
30. broken sword fragment, snapped blade in stone
31. broken shield half, splintered wood with iron rim
32. rusted dagger, half-buried in dust
33. broken arrow, snapped shaft with iron tip
34. coin pile small, tarnished silver coins, scattered
35. coin pile medium, mix of silver and copper
36. scroll rolled, dark parchment with red wax seal
37. scroll unrolled, faded parchment partially visible
38. spell book closed, leather-bound dark tome
39. broken book pages, scattered parchment fragments
40. ink pot, dark glass with quill, half-empty
41. small rubble pile, mixed stones and dust
42. medium rubble pile, larger broken masonry chunks
43. broken stone tile single, chipped 32x32 fragment
44. cracked floor stone, hairline rift crack visible
45. moss covered rock, small boulder with lichen
46. chain segment short, rusted iron links
47. iron ring single, dark metal, embedded in floor
48. broken padlock, snapped shackle, dark iron
49. ritual stone small, carved sigil, faint glow
50. rune fragment, broken stone with cyan glyph
```

### Item 51-64: GAMEPLAY (Karar #86 F1 minimum 6 + variants)
```
51. interactive chest gameplay, polished dark wood with iron bands, intact, closed
52. interactive chest opened, lid raised, glowing interior
53. shrine altar small, dark stone slab with rune carving, intact
54. broken shrine altar, split stone with rune fragment scattered
55. spike trap embedded, iron spikes protruding from stone floor
56. spike trap retracted, closed iron grate flush with floor
57. lever wall mounted, dark iron handle, upright position
58. lever pulled, dark iron handle, lowered position
59. pressure plate, stone tile slightly recessed, faint glow
60. brazier small, dark iron bowl with cold blue flame
61. unlit brazier, dark iron bowl, ash residue
62. rift gate small, cyan-violet portal fragment, swirling void
63. door wooden small, dark planks with iron studs, closed
64. door wooden small, opened position, swung inward
```

6. **Generate** bas → ~2-4 dakika bekle (büyük batch)
7. Çıktıyı (sheet, 8×8 grid, 64 obje, transparent BG) kaydet
8. Kaydet: `STAGING/PROP_OUTPUT/F1_Decor_64batch_32px/decor_64.png`
9. **QC için Claude'a göster**

---

## 🟡 BATCH 4: F1 Medium Props 16 — `create_object` (Batch 1+2+3 sonrası)

### Adım Adım
1. **Create Object** tool aç (Batch 3 ile aynı)
2. Form değişikliği:

| Alan | Değer |
|---|---|
| Size | **64** → 4×4 grid = 16 obje |

3. **Style Reference Images:**
   - 4 style_ref Wang tile
   - + Batch 3'ten **en iyi 2-3 prop** (palette anchor)

4. **Object Description** (üst alan):
```
Single medium-sized environmental prop or interactive object for a shattered keep dungeon, 64x64 transparent background top-down pixel art viewed from ~35 degrees high top-down angle (Hades reference). Mat painterly pixel, dark gritty palette (#2C2A2A dark stone, #4A3F3F shadow, #7BA7BC cold blue, occasional rust orange #C4682A, occasional cyan-violet rift accent), Salt and Sanctuary chibi-but-serious tone. Single isolated object, no characters, no walls.
```

5. **Item 1-16:**
```
1. broken_pillar_section, 32x64 fragment, half-collapsed dark stone column
2. iron_cage_large, rusted bars, broken door, hanging chain
3. large_chest, dark wood with brass bindings, closed
4. brazier_tall, iron pillar with cold blue flame bowl on top
5. altar_intact, dark stone slab with rune carving, ritual circle base
6. altar_broken, split stone slab with shattered rune fragments
7. throne_fragment, broken stone throne side, ancient royal seat
8. statue_head_fallen, weathered stone head on side, half-buried
9. large_urn, terracotta vessel, intact with carved sigil
10. large_rubble_pile, heaped masonry chunks with dust cloud
11. broken_column_top, crowned capital piece lying on floor
12. iron_torch_stand, wall-mount holder with flame
13. ritual_circle_partial, faded chalk pattern with cold glow remains
14. broken_statue_torso, headless figure remains, kneeling pose
15. large_skull_horned, oversized ram or beast skull, ivory bone
16. ancient_anvil, dark iron forge anvil with hammer marks
```

6. **Generate** → bekle → kaydet
7. Yer: `STAGING/PROP_OUTPUT/F1_MediumProps_16batch_64px/medium_props_16.png`

---

## ⚪ BATCH 5 (FAZ 1.5 — Opsiyonel): Large Props 4 — `create_object`

### Adım Adım
1. **Create Object** aç
2. Size: **128** → 2×2 grid = 4 obje
3. Item 1-4:
```
1. full_pillar_intact, complete dark stone column, ornate base and capital
2. boss_arena_altar, large dark ritual altar with cold blue glow centerpiece
3. shattered_statue_full, complete fallen guardian statue, ruined
4. rift_obelisk, tall cyan-violet glowing monolith, ritual catastrophe centerpiece
```

4. Faz 1.5'te, şimdi opsiyonel — boss arena için lazım olduğunda yap.

---

## 📋 Sıra ve Bağımlılıklar

```
Batch 1 (Floor 64)  ──┐
                      ├──> Batch 3 (Decor 64) ──> Batch 4 (Medium 16)
Batch 2 (Wall 64)   ──┘                                  │
                                                          └──> Batch 5 (Large 4, Faz 1.5)
```

**Önemli:**
- Batch 2 başlamadan **Batch 1 PASS olmalı** (style ref Batch 2'ye Batch 1'in best tile'ları girer)
- Batch 3 başlamadan **Batch 1+2 PASS olmalı** (style ref karışımı)
- Her batch sonrası **Claude'a göster QC için** — devam onayı al

---

## ❌ Yapma Listesi

- ❌ PixelLab Map editor'ünde harita çizme — mockup bile Unity'de yapılacak (Karar #118 disiplin)
- ❌ Wang chain üretme (Floor↔Moss, Floor↔Rift) — moss/rift decal olarak Batch 3'te
- ❌ Style reference olmadan generate — palette drift eder
- ❌ Tile size 32 dışında bir şey (Karar #100 LOCKED)
- ❌ View angle 35° dışında (Karar #113 LOCKED)
- ❌ Outline `single color` veya `selective` (`segmentation`/`lineless` kullan — dark gritty için)

---

## 🔧 Yardımcı Script'ler

**Wang sheet'ten 32px tile kesimi:**
```bash
python tools/slice_wang_sheet.py "<sheet.png>"
```
Otomatik 16 tile + 4 style_ref klasörü üretir.

**Büyük görseli resize (Claude okuyabilmek için):**
```bash
python tools/resize_image.py "<image.png>"
```
Aspect-ratio koruyarak max 1800px'e indirir.

---

## ✅ Her Batch Sonrası QC Checklist (Claude bunu yapacak)

1. **Palette tutarlılığı** — #2C2A2A / #4A3F3F / #7BA7BC paletinden çıkmamış mı?
2. **View angle ~35°** — 45° kuş bakışı değil, hafif eğik
3. **Raggedness ≥40%** (Karar #116) — tile kenarları doğal, grid-block hissi yok
4. **Variant zenginliği** — 64 tile'da gerçekten farklı kompozisyonlar
5. **No banned content** — kan splatter, parlak cartoon renk, outline cartoon, gradient yok
6. **Tileable** — yan yana koyunca dikiş okunmuyor (4x zoom'da hairline crack continuity)

Tüm 6 PASS olursa → **Next Batch** veya **Aseprite cleanup + Unity import**
