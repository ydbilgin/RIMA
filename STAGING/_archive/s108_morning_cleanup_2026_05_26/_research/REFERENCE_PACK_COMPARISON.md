# Reference Pack Comparison — 3-Dungeon Showcase Analysis

**Date:** 2026-05-21 S97
**Scene:** `Assets/Scenes/Demo/PlayableRoom_v2.unity`
**Screenshot:** `Assets/Screenshots/screenshot-20260521-163919.png` (3-dungeon side-by-side)
**Lore basis:** `memory/project_act1_shattered_keep_lore_lock.md`

## 1. Setup

3 farklı asset pack (free/CC0 / pay-what-you-want) PlayableRoom_v2 sahnesine eş zamanlı kuruldu:

| Dungeon | Pack | Konum (cell) | Wall sayısı |
|---|---|---|---|
| **A** | Kenney Isometric Miniature Dungeon (CC0) | X[1..7] Y[0..5] | 22 piece |
| **B** | Nilsen303 Isometric Dungeon Tileset (free) | X[14..20] Y[0..5] | 16 piece |
| **C** | Jaqmarti Isometric Walls Library (PWYW) | X[27..33] Y[5] | 7 piece (back row) |

Ortak floor: bizim `tile_floor_granite_v1-v4.asset` (64×64 PixelLab v01).

## 2. Pack-by-Pack Bulgular

### Dungeon A — Kenney (sandstone, 256×512 vector iso)

**Güçlü:**
- 43 unique wall variant + 70 anatomic piece toplam
- Per-cell wall pieces, corner pieces SEPARATE
- 4-direction system (_E/_N/_S/_W)
- İskelet/anatomik yapı = production blueprint
- Sample.png ideal kompozisyon referansı

**Zayıf:**
- 256×512 vector style — RIMA pixel art density'sine uymaz
- Light sandstone palette — RIMA dark granite ile çelişir
- 256×512 = sahnemizin floor ile mismatched scale (PPU 256 ile dengelendi)

**RIMA fit:** **8/10** (anatomi)

### Dungeon B — Nilsen303 (dark pixel art, 336×560 sheet)

**Güçlü:**
- Pixel art density yakın RIMA stiline
- Dark stone palette
- 14 auto-slice sub-sprite (cube + long + corner + floor patches)
- Cyan accent dots (RIMA cyan rift estetiğine yakın)

**Zayıf:**
- Cubes (66×66) tek başına ALINDIĞINDA loose stone gibi görünüyor
- "Wall" hissi vermek için OVERLAP/connection logic gerekli
- Pieces çok küçük (0.5 cell tall) — occlusion yetersiz
- Anatomic set yetersiz (8 wall piece toplam, Kenney'nin 1/5'i)

**RIMA fit:** **6/10** (stil yakın ama yetersiz parça + connection issue)

### Dungeon C — Jaqmarti (library walls, 2048×2048 sheet)

**Güçlü:**
- Header+Body split tekniği öğretici (üst kalın + alt ince band)
- 311×494 büyük walls — high detail
- 4 renk varyant (white/cyan/yellow/red accent)
- Decoration banding (vitral/painted)

**Zayıf:**
- Library teması RIMA dungeon ile uyumsuz
- Çok TALL (2-3 cell) — sub-room scale'i bozar
- Tek wall tipi × renk varyantı — anatomic set yok
- Boyut/PPU mismatch çıkardı

**RIMA fit:** **4/10** (tema farklı, sadece tekniğin öğrenildiği bir kaynak)

## 3. Sweet Spot Sentezi (RIMA için)

```
RIMA Wall = (Kenney anatomy) + (Nilsen pixel density) + (Jaqmarti H+B technique)
```

| Aspect | Source pack | RIMA değeri |
|---|---|---|
| Anatomi liste | Kenney 43 variant | 6 base × 3 variant = 18 sprite (TIER 1) |
| Pixel density | Nilsen303 | 64-96 PPU, chunky pixel block |
| Header+body split | Jaqmarti | Sadece TALL walls için (2-tier system) |
| Connectivity | Kenney | Per-cell + corner SEPARATE |
| Damage variants | Kenney aged/broken | intact / rift_minor / rift_major |

## 4. Critical Inferences (5 madde)

### 4.1 Wall Height Standardization
- Kenney: ~1 cell tall (orta)
- Nilsen: ~0.5 cell tall (kısa, **fail**)
- Jaqmarti: ~2-3 cell tall (çok yüksek)
- **RIMA decision:** **2 tier system** — TALL perimeter (~1.5-2 cell) + LOW inner (~1 cell)

### 4.2 Per-Cell Wall Piece Standard
- Her cell'e 1 piece (Nilsen'in loose cube failure'ından ders)
- Corner pieces SEPARATE asset (auto-tile / RuleTile compatible)
- 4-direction OR 2+mirror (Karar #114 8-dir mirror strategy)

### 4.3 Cyan Rift Bake-In (NOT seam)
- Pilot A failure: cyan rift PIECES ARASINDA seam → micro-gap visible
- Doğru yaklaşım: cyan rift **SPRITE İÇİNDE** baked, scatter logic
- 3 variant per wall: intact / rift_minor / rift_major (cosmetic)

### 4.4 Sort Order Architecture (technical)
- Project sorting layers: Default(0) → Ground(1) → Walls(2) → Entities(3) → VFX(4) → Floor(5)
- ⚠️ **Floor LAST = renders ON TOP** of Walls layer!
- Wall SpriteRenderer'ları → `sortingLayerName = "Floor"` + higher order (Y-based)
- Veya project sorting layer order yeniden düzenle (Floor → Walls → Entities → VFX) — daha temiz mimari

### 4.5 Construction Pattern (best of all 3)
- Floor base: granite v1-v4 random
- Wall perimeter: per-cell pieces (Kenney pattern)
- Inner room dividers: low walls (image 1 reference)
- Decor: chains + banners + candles + moss + skeleton scatter
- Lighting: 2D Light per candle (warm) + per rift (cool) + global ambient

## 5. Wall Closure Method Catalog (Image 3 analiz)

Reference image `RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png` close-up çözümlemesi:

| # | Yöntem | Image 3'te | RIMA için kullan |
|---|---|---|---|
| 1 | Pure black void backdrop | ✅ | ✅ Ana (sealed/underground) |
| 2 | Vertical chain decoration | ✅ | ✅ Asset gen (3 length variant) |
| 3 | 2D Light falloff | ✅ | ✅ URP 2D Renderer hazır |
| 4 | Tall walls (2-3 cell) | ✅ | ✅ TIER 1 wall spec |
| 5 | Moss/vine overlay | ✅ | ✅ Decor scatter |
| 6 | Vignette edge | ⚠️ | ✅ URP Volume post-process |

**Anahtar prensip:** "**Implied ceiling**" — tavan ÇİZİLMEZ, ima edilir (chains "yukarıdan", absolute black above). Rendering yükü minimum, atmosfer maximum.

## 6. PixelLab Production Plan — Capacity-Aware Batching

### 6.1 PixelLab Capacity Tiers (LOCK from `pixellab-production-knowledge`)

| Size range | Items / batch | Cost per call |
|---|---|---|
| **32-40 px** | **64 items** | 20-40 gen |
| **48-80 px** | **16 items** | 20-40 gen |
| **88-168 px** | **4 items** | 20-40 gen |

**Optimization rule:** Aynı size tier'daki item'ları **TEK batch'te** topla → gen başına item maliyeti minimize.

### 6.2 Wall Family — TIER 1 (6 base × 3 variant = 18 sprite)

**Strategy:** `create_object` ile 6 base intact → `create_object_state` ile her base için 2 rift variant. State pipeline palette/style match garanti + intact'tan ucuz.

**Base intact üretim (büyük tier 88-168px, 4/batch):**

| Batch | Items (4 per call) | Canvas | Gen cost |
|---|---|---|---|
| W-B1 | wall_tall_straight + wall_tall_corner + wall_archway + wall_endcap_column | 96-128 wide × 160 tall | ~30 gen |
| W-B2 | wall_low_straight + wall_low_corner + (2 buffer slot — extra corner orient?) | 96 × 96 | ~30 gen |

**Variant üretim (create_object_state, base-anchored, cheaper):**
- 6 base × 2 state (rift_minor + rift_major) = 12 state calls
- ~10-15 gen per state call → ~150 gen toplam

**TIER 1 Wall total: ~210 gen (60 base + ~150 state)**

### 6.3 Decor Family — Smart Batching

**Batch D1 — Small props (48-80 px, 16/batch):**
| Item | Count | Canvas |
|---|---|---|
| banner_purple (intact + torn) | 2 | 48×96 |
| moss_overlay (4 placement variant) | 4 | 56×56 |
| skeleton_scatter (3 pose) | 3 | 64×48 |
| urn_intact + urn_broken | 2 | 48×48 |
| barrel | 1 | 56×64 |
| chest_closed | 1 | 64×56 |
| spike_trap | 1 | 64×32 |
| floor_bone_scatter | 2 | 56×40 |
| **Total batch D1** | **16** | **1 call ~35 gen** |

**Batch D2 — Tiny decor (32-40 px, 64/batch):**
| Item | Count | Canvas |
|---|---|---|
| candle_wall_sconce (4 wall positions) | 4 | 32×32 |
| chain_hanging (3 length) | 3 | 32-link tiled |
| floor_dust_decal (8 variant) | 8 | 32×24 |
| floor_blood_decal (4 variant) | 4 | 32×24 |
| floor_crack_decal (8 variant) | 8 | 40×24 |
| coin_pickup + gem_pickup + key_pickup | 3 | 24×24 |
| arrow_floor_scatter (4 dir) | 4 | 32×16 |
| skull_floor (4 variant) | 4 | 32×24 |
| wall_torch_unlit + wall_torch_lit | 2 | 24×40 |
| candle_floor (3 height) | 3 | 24×40 |
| **rune_pebble scatter** (4 cyan rift accent) | 4 | 24×24 |
| (buffer 11 slot for iteration) | 11 | reserved |
| **Total batch D2** | **64 slot (53 active)** | **1 call ~35 gen** |

**Batch D3 — Large decor (88-168 px, 4/batch):**
| Item | Count | Canvas |
|---|---|---|
| pillar_tall (broken variant) | 1 | 96×160 |
| statue_warrior (Act 1 lore) | 1 | 96×128 |
| brazier_lit (light source) | 1 | 96×128 |
| altar_central (sub-room feature) | 1 | 128×96 |
| **Total batch D3** | **4** | **1 call ~30 gen** |

**Decor total: ~100 gen (3 batch calls)**

### 6.4 Floor Decal Family — Bonus (32-40 px tier already efficient)

Mevcut floor: granite v1-v4 (16 PNG) ZATEN VAR. Sadece **accent decal** üretiminde tier'ı kullan:

**Batch F1 — Floor decals (32px, 64/batch):**
- cyan_rift_crack_floor × 8 yön/length
- mortar_seam_overlay × 8
- moss_floor_patch × 8
- dust_pile × 8
- worn_path × 8
- blood_trail × 4
- footprint × 8 (4 dir × 2 size)
- **48 active / 64 slot, 1 call ~35 gen**

### 6.5 Total Production Budget (TIER 1 — Act 1 Shattered Keep Visual Sufficient)

| Family | Batch count | Gen cost |
|---|---|---|
| Wall base intact (W-B1, W-B2) | 2 | ~60 |
| Wall state variants (12 calls) | 12 | ~150 |
| Decor small props (D1) | 1 | ~35 |
| Decor tiny (D2) | 1 | ~35 |
| Decor large (D3) | 1 | ~30 |
| Floor decals (F1) | 1 | ~35 |
| **Subtotal** | **18 calls** | **~345 gen** |
| Buffer (FAIL retry + iteration) | +20% | ~70 gen |
| **GRAND TOTAL** | **~22 calls** | **~415 gen** |

**RIMA budget impact:** PixelLab 2,820/5,000 mevcut → 415 spend → **~2,405 reserve** (Act 2/3/4 + char animation için).

### 6.6 Production Sequence (Codex/Orchestrator Dispatch)

**Sprint 1 — Wall MVP (1 batch, ~60 gen, hızlı validation):**
1. Dispatch W-B1 (4 large walls intact) — 30 gen
2. Visual review → PASS/FAIL
3. PASS → dispatch W-B2 (low walls) — 30 gen

**Sprint 2 — Wall Variants (12 state calls, ~150 gen):**
4. For each W-B1+W-B2 base → `create_object_state` × 2 (rift_minor + rift_major)
5. State pipeline = style match GARANTİ, daha ucuz

**Sprint 3 — Decor + Floor Decals (3 batch, ~100 gen):**
6. D1 (small props batch 16-item)
7. D2 (tiny decor batch 64-item)
8. D3 (large feature decor batch 4-item)
9. F1 (floor decals 64-item)

**Total dispatch: 4 sprint, ~22 PixelLab calls, ~415 gen**

### 6.7 Lighting Setup (Codex, Unity-side, no PixelLab)

- 2D Light per candle: Point, 2-radius, warm `#FFB060`
- 2D Light per cyan rift: Point, 3-radius, cool `#40D0E0`
- Global Light: ambient `#0A0810` (Act 1 base dark)
- Vignette post-process: URP Volume + Bloom
- 0 gen cost (kod tarafı)

## 7. Sonuç + Sıradaki Adımlar

✅ **Lore lock:** Failed Shelter + Convergence biphasic ([[project_act1_shattered_keep_lore_lock]])
✅ **Reference packs imported:** `Assets/Art/_TempReferencePacks/` 3 vendor folder
✅ **3-dungeon showcase live:** PlayableRoom_v2 X[1..33] zone'lar
✅ **Inferences locked:** wall height 2-tier, per-cell pieces, rift bake-in, sort architecture

**Sıradaki:**
1. Git commit (current scene + memory + this doc)
2. PixelLab Spec v2 — TIER 1 18-sprite wall family + decor + reference image bundle
3. Codex dispatch — lighting setup + sub-room layout helper

## 8. Related Files

- Lore: `memory/project_act1_shattered_keep_lore_lock.md`
- Visual goal: `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`
- Scene: `Assets/Scenes/Demo/PlayableRoom_v2.unity`
- Pack source raw: `STAGING/_reference_packs_raw/`
- Pack imported: `Assets/Art/_TempReferencePacks/`
