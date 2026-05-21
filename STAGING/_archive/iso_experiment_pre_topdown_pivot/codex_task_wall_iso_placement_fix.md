# Codex Task — Wall Pack v3 Iso Placement Architecture Fix

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

---

## CONTEXT — Bu sahnedeki sorun

Wall Pack v3 (22 tile, `Assets/Data/Tiles/Act1_ShatteredKeep/walls_v3/`) Map 1 + Map 2 paint test'te dungeon hissi vermiyor. Duvarlar dağınık görünüyor. User feedback: "saçma sapan yerleşmiş".

**Screenshots:**
- `Assets/Screenshots/WallTest_Map1_Rectangle.png` — 10×6 rectangle, 4 outer corners + south archway + 2 columns + hero wall
- `Assets/Screenshots/WallTest_Map2_LShape.png` — L-shape, inner corner + 2 archways

**Scenes:**
- `Assets/Scenes/Demo/WallTest_Map1_Rectangle.unity` — birinci test
- `Assets/Scenes/Demo/WallTest_Map2_LShape.unity` — ikinci test
- `Assets/Scenes/Demo/PlayableRoom_v2.unity` — baseline (3 floor segment + 1 wall rectangle)

**Wall tile sprite metadata (Unity'den okundu):**
```
straight_NE:  85x94 px  pivot=(42.5, 0)   → world size 1.33 x 1.47 (1 cell aşıyor)
straight_SE:  84x95 px  pivot=(42, 0)     → world size 1.31 x 1.48
hero:        152x166 px pivot=(76, 0)     → world size 2.38 x 2.59 (2-cell genişlik!)
archway_NE:  115x161 px pivot=(57.5, 0)   → world size 1.80 x 2.52
archway_SE:  116x162 px pivot=(58, 0)     → world size 1.81 x 2.53
column:       64x162 px pivot=(32, 0)     → world size 1.00 x 2.53
corner_outer_a: 74x83 pivot=(37, 0)       → world size 1.16 x 1.30
corner_inner_a: 84x100 pivot=(42, 0)      → world size 1.31 x 1.56
T_junction_a: 83x97 pivot=(41.5, 0)       → world size 1.30 x 1.52
low_wall_straight: 79x82 pivot=(39.5, 0)  → world size 1.23 x 1.28
foundation_a: 84x69 pivot=(42, 0)         → world size 1.31 x 1.08
floor_edge:   84x64 pivot=(42, 0)         → world size 1.31 x 1.00
```

**Grid setup:**
- `cellLayout=IsometricZAsY, cellSize=(1, 0.5, 1)` → 1 cell = 1 world unit X × 0.5 unit Y
- Floor tiles 64x64 px, **çoğunluk pivot (32,32) = CENTER (YANLIŞ — iso için (32,16) lazım)**
- `placeholder_iso` pivot (32,16) doğru, diğer 16 floor tile yanlış

## ROOT CAUSE DIAGNOSIS (Opus tarafından)

1. **Iso convention ihlal:** 4 edge'e de full-height wall koydum. Hades/Diablo standardı: SADECE back walls (kuzey + batı edge) full-height; front (güney + doğu) low_wall veya foundation/floor_edge ile contained kalır, kamera görüşü açık.

2. **Hero wall multi-cell footprint sorunu:** 2.4 cell genişlik. 2 hero tile yan yana koymak (5,7)+(6,7) overlap üretti → split görüntü.

3. **Wall sprite'lar tüm yönlerden 1.3+ cell genişlik:** Adjacent cell'lere taşıyor. T-junction/corner placement bu offset'i bilmeli; aksi halde overlap.

4. **Floor pivot (32,32):** Floor sprite'lar iso diamond hizasında değil, perspective bozuk.

## TASK — Yapılacaklar

### Adım 1 — Diagnosis Verification
UnityMCP üzerinden:
- 22 wall sprite'ın her birini görsel inspect et (`Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_pack_v3/*.png`)
- Sprite içeriği vs name uyumlu mu doğrula (e.g., `straight_NE` gerçekten NE-running wall mı?)
- Her tile'ın "footprint" (kaç cell kaplıyor) ve "edge orientation" (hangi cell edge'e oturuyor) listele
- Çıktı: `STAGING/wall_pack_v3_tile_audit.md` — her 22 tile için satır: name, footprint (cells_wide × cells_tall), edge_orientation, placement_anchor (cell-bottom/cell-center/etc.), recommended_usage_rule

### Adım 2 — Iso Wall Convention Decision
3 strateji öner ve bir tane SEÇ (rationale yazılı):

**Strategy A — Back walls only (Hades/Diablo standardı)**
- Kuzey edge (y=max): straight_NE
- Batı edge (x=min): straight_NE (rotation? Veya separate sprite var mı?)
- Güney edge (y=min): low_wall_straight veya foundation
- Doğu edge (x=max): low_wall_straight veya foundation
- 4 köşe: outer_a/b/c/d back-corner için, low_wall_corner front-corner için
- Archway: kuzey + güney mid-point (single sprite, NOT pair — multi-cell footprint nedeniyle)
- Hero wall: north-center'a TEK kez (2 cell aralık bırak yan yana koymadan)
- Pro: clean view, doğru iso convention
- Con: south + east'ten içeri girişi nasıl temsil ederiz? (low_wall ile)

**Strategy B — Pair-based archway, all-side full walls + camera lift**
- 4 edge'i de full wall ile kapat
- Camera 35° tilt ekleyip front walls'u perspektif ile küçült
- Pro: tam kapalı oda
- Con: archway pair'i overlap eder; camera tilt scene asset değil global etki

**Strategy C — Wall tile pivot recalibration**
- Tüm wall sprite'ların pivot'u (X, 0)'dan (X, -32) gibi negative-offset ile yukarı çekip cell-center'a oturt
- Floor pivot da düzelt (32, 16)
- Adjacent cell overlap'i kabullen, T-junction/hero için multi-cell pattern uygula
- Pro: maximum flexibility
- Con: pivot edit ile tüm scene'de wall pozisyon kayar; mevcut scene'lerde paint'i yeniden yap

**SEÇ ve rationale yaz.**

### Adım 3 — Map 1 Repaint
Seçilen strategy ile `WallTest_Map1_Rectangle.unity` yi yeniden paint et. 10×6 hero room, south archway + back walls + 4 corner + hero wall + 1 freestanding column (interior). UnityMCP `execute_code` ile programmatic paint.

**ÖNEMLI:**
- Mevcut paint'i `ClearAllTiles()` ile sıfırla
- Scene save sonra
- Screenshot al: `Assets/Screenshots/WallTest_Map1_v2_Rectangle.png` (1280px scene_view capture)

### Adım 4 — Map 2 Repaint
Aynı seçilen strategy ile `WallTest_Map2_LShape.unity` yi yeniden paint et. L-shape, inner corner + 2 archway + interior column.

Screenshot: `Assets/Screenshots/WallTest_Map2_v2_LShape.png`

### Adım 5 — Comparison Report
`STAGING/wall_iso_placement_fix_REPORT.md` yaz:
- Seçilen strategy + why
- Map 1 v1 (önceki) vs v2 (yeni) verdict (her connection için PASS/IMPROVED/STILL_BROKEN)
- Map 2 v1 vs v2 verdict
- Kalan gap'ler (hangi tile combination test edilemedi, hangi sorun çözülmedi)
- Recommended next test (Map 3?)

### Adım 6 — (Optional) Floor Pivot Fix
Eğer Strategy C seçildiyse veya time yeterse:
- 16 floor tile sprite import setting'i (32,32) → (32,16) güncelle
- TextureImporter SpriteAlignment.Custom + SetSpriteCustomPivot
- Scene reload + screenshot
- Eğer pivot fix scene'i bozarsa undo, raporda flag at

## ALLOWED FILES TO EDIT

- `STAGING/wall_pack_v3_tile_audit.md` (new)
- `STAGING/wall_iso_placement_fix_REPORT.md` (new)
- `Assets/Scenes/Demo/WallTest_Map1_Rectangle.unity` (repaint via UnityMCP)
- `Assets/Scenes/Demo/WallTest_Map2_LShape.unity` (repaint via UnityMCP)
- `Assets/Screenshots/WallTest_Map1_v2_Rectangle.png` (new)
- `Assets/Screenshots/WallTest_Map2_v2_LShape.png` (new)
- (Strategy C için) Floor tile PNG import setting'leri `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/*.png.meta` veya `isometric_v01` ne ise

## DELIVERABLES

1. Tile audit doc (22 entry tablo)
2. Strategy decision + rationale
3. 2 repainted scene + 2 screenshot
4. Comparison report PASS/IMPROVED/STILL_BROKEN per connection

## SUCCESS CRITERIA

User'ın "dungeon gibi hissetmiyor" feedback'i çözüldü mü:
- Oda silüeti net rectangular/L
- Wall pieces overlap YOK
- Hero wall split YOK (tek parça)
- Floor area görünür (walls dominate etmiyor)
- Connection seams clean

Eğer success belirsiz kalırsa raporda ITERATE_NEEDED flag at, sonraki Codex round için spec yaz.

## NOTES

- Console error olursa fix + recheck zorunlu (HARD RULE feedback_codex_fix_unity_errors_always)
- UnityMCP çağrılarında compile fail (`return` statement içinde void tuple gibi) dikkat
- `GetUsedTilesCount` = distinct tile sayısı (cell count değil), `cellBounds.allPositionsWithin` üzerinden cell saymak gerek
- Unity açık olmalı (UnityMCP)
- Effort: high (visual judgment + iteration needed)
