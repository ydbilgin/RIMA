# Codex Final Strategy Review: PixelLab Tool Stratejisi (S66)

**Tarih:** 2026-05-13
**Tip:** ARCHITECTURE REVIEW + FINAL DECISION
**Çıktı:** CODEX_DONE.md append

## Kullanıcı Vizyonu (NET, son hali)

> "PixelLab **sadece tile/asset üretim** için kullanılacak. Map tasarımı, oda layout, kompozisyon, mockup — **HEPSİ Unity Room Designer içinde** olacak. PixelLab Map editor artık kullanılmayacak (mockup için bile). Ben Unity'de PixelLab'daki gibi brush ile boyayıp sileceğim, Claude+Codex baseline'ı kuracak."

Bu vizyon Karar #115 (Map Builder), #117 (Portable Core), #118 (Hybrid Tile Composition) ile uyumlu — ama tool seçim stratejisini Codex final değerlendirsin.

## Mevcut Asset Durumu

PixelLab Map export edildi (`STAGING/TILESET_OUTPUT/F1_FloorWallPath_2026-05-13/`):
- ✅ Wang Floor↔Wall sheet (128x128, 4x4 grid, 16 tile) — `asset_006.png`
- ✅ Wang Floor↔Path sheet (128x128, 4x4 grid, 16 tile) — `asset_007.png`
- ✅ `asset_000.json` — terrain metadata + `wangIndexMapping: "standard"`
- ✅ Warblade sprite — `asset_008.png` (bonus)
- ❌ Floor variant zenginliği (sadece Wang'in 16 tile'ı)
- ❌ Decals (moss, rift crack) henüz yok
- ❌ Props henüz yok

## Üç Aday Strateji

### A — Wang Minimal + create_tiles_pro Maksimal
- Wang: Sadece **Floor↔Wall** (ana gameplay collider ilişkisi)
- `create_tiles_pro` 64-batch: Floor variants (64) + Wall variants (64) + Path variants (64)
- `create_object`: decals + props
- Mevcut Wang Floor↔Path tutulur (transition reference olarak)
- Toplam PixelLab gen: ~5-7
- Avantaj: Variant zenginliği max, Unity RuleTile + RandomTile esnek
- Dezavantaj: Path-floor edge için Unity'de Codex ek RuleTile rule iş

### B — Wang Primary + create_tiles_pro Refinement
- Wang: Tüm major terrain pair'leri (Floor↔Wall, Floor↔Path, Floor↔Moss, F2↔F1)
- `create_tiles_pro`: Her terrain için variant batch
- `create_object`: props sadece
- Toplam PixelLab gen: ~10-12
- Avantaj: Tüm transition'lar PixelLab'dan hazır
- Dezavantaj: Maliyetli, decals Wang'a sokuluyor (overlay olmalıydı)

### C — Hybrid Asymmetric (mevcut Karar #118 ana çizgisi)
- Wang: Floor↔Wall (DONE) + Floor↔Path (DONE — tut)
- `create_tiles_pro` 64-batch: Floor variants + Wall variants
- `create_object`: Moss decals, Rift crack decals, Props
- Faz 2/3 biome geçişleri için Wang chain
- Toplam ek PixelLab gen: ~4-5
- Avantaj: Mevcut Wang sheet'ler kullanılır + variant zenginliği + decal/prop ayrımı
- Dezavantaj: Yok (en dengeli)

## JSON Parser Önerisi (Faz 1.0'a ek)

PixelLab export `asset_000.json` yapısı:
```json
"tilesets": [{
  "lowerTerrainId": 1, "upperTerrainId": 2,
  "tileSize": 32, "gridLayout": "4x4",
  "wangIndexMapping": "standard"
}]
```

**Öneri:** `TileImportWizard`'a PixelLab Export Parser ek modül:
1. User folder seç (export klasörü)
2. JSON parse → terrain + tileset metadata
3. Sheet PNG'ler auto-slice (Wang grid + tile size)
4. RuleTile asset'leri Wang index ile auto-create
5. Multi-layer tilemap iskeleti hazır

Faz 1.0'a +2-3 saat ek scope. Gelecek tüm PixelLab export'larını saniyeler içinde Unity'ye getirir.

## Senin Görevin (Codex)

3 stratejiyi RIMA bağlamında değerlendir + JSON parser kararı. LOCKED bağlamlar:
- Karar #100: 32x32 tile, ~35° top-down, PPU=64, chibi
- Karar #115: Unity Editor F2 Map Builder
- Karar #116: Tile Transition Quality (Raggedness, edge-blend, decal layer, drop shadow)
- Karar #117: Portable Core (RIMA-specific Game Layer)
- Karar #118: Multi-layer tilemap (Base/Decal/Wall/Prop)
- Karar #90: PixelLab Batch Economy (32px → 64 cell)

### Output Format

```
# Codex Final Strategy Review: PixelLab Tool S66

## Verdict
[A / B / C / Hybrid blend + neden]

## Strateji Karşılaştırma Tablosu
| Konu | A | B | C |
|---|---|---|---|
| Variant zenginliği | ... | ... | ... |
| Cost | ... | ... | ... |
| Unity entegrasyon karmaşıklığı | ... | ... | ... |
| Decal/Prop ayrımı | ... | ... | ... |
| Karar #118 uyumu | ... | ... | ... |

## Mevcut Wang Sheet'leri Kullanım Önerisi
- Floor↔Wall: KEEP/REPLACE — neden
- Floor↔Path: KEEP/REPLACE — neden

## JSON Parser Önerisi
- INTEGRATE Faz 1.0 / DEFER Faz 1.5 / REJECT — neden
- Implementation tasarım önerisi (TileImportWizard hook noktası)

## Pixellab Pipeline Önerilen Sıralama (Adım Adım)
1. ... (concrete action)
2. ...

## RIMA Spesifik Risk + Mitigation
- Variant zenginliği yetersiz olursa: ...
- Wang sheet'in Unity RuleTile rule'a mapping karmaşıklığı: ...
- create_tiles_pro 64-batch palet driftleyebilir: ...

## Karar #118 Güncelleme İhtiyacı
[Varsa ek madde önerisi, yoksa "no change"]

## Faz 1.0 MVP Codex Task Scope Önerisi
[RoomBaselineGenerator + TileImportWizard PixelLab parser + multi-layer setup için somut iş listesi, saat tahmini]

## ORCHESTRATOR NEXT STEP
1. ... (concrete)
2. ...
```

## Kısıtlar
- LOCKED kararları override etme; ek refine öner
- Türkçe, 800-1200 kelime
- Kod yazma yok, sadece strateji + tasarım
- User pipeline disiplini istiyor — "şu kadar gen + şu kadar iş, hangi siparişle" net olmalı
- Effort: high
