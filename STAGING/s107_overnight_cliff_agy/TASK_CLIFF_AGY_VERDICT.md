# TASK: Cliff Placement Final Verdict + Autonomous Fix Recommendation

ACTIVE RULES: (1) think before reviewing (2) min response, cite specific image areas (3) inline only (4) BLOCKED if image unreadable.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: Mevcut cliff placement (8-direction + offset.y=1.0) sonucunu görsel analiz et. Önceki S+SE+SW (3-yön, 262 tile) ile karşılaştır. RIMA wall-less arena için **en iyi konfig** ne, somut tweak değeri öner. Orchestrator (Opus) bu öneriyi autonomous uygulayacak.

## Screenshot Paths
1. `Assets/Screenshots/cliff_offset_1_0.png` — Mevcut state: 8-direction + offset.y=1.0 + 413 tile (full perimeter)
2. `Assets/Screenshots/screenshot-20260526-022538.png` — Önceki: 3-direction (S+SE+SW) + offset.y=1.5 + 262 tile
3. `Assets/Screenshots/cliff_full_perimeter_check.png` — 8-direction önceki offset=0.6 (overlay/taşma)

## Mevcut Konfig
```
CliffAutoPlacer: 8 direction (S/N/E/W + 4 corner) check
Sprite: 128×192 px PPU 64 → world 2×3 unit
DeterministicVariantTile:
  spriteScale: (1.0, 1.0)
  transformOffset: (0, 1.0, 0)
TilemapRenderer:
  Cliff: layer=Floor, order=-1 (Floor sprite arkasında render)
  Floor: layer=Floor, order=0
```

## Sorular (her biri için somut cevap)

### 1. Mevcut 8-dir + offset 1.0 değerlendirmesi
- Hangi bölgeler iyi oturmuş?
- Hangi bölgeler hala taşıyor / yanlış görünüyor?
- N (kuzey) kenarında cliff sprite ters duruyor mu (drop face aşağı bakıyor, top deck yukarıda)?

### 2. 3-dir (S+SE+SW) vs 8-dir karşılaştırma
- Hades pattern'e hangi daha yakın?
- RIMA wall-less arena için hangisi tercih edilmeli?
- Tile sayısı (262 vs 413) perf etkisi var mı?

### 3. EN İYİ KONFIG ÖNERİSİ (autonomous uygulanacak)
- Direction set: 1-dir / 3-dir / 4-dir / 5-dir / 6-dir / 8-dir?
- transformOffset.y değeri (0.3-2.0 arası)?
- Sprite scale değeri (1.0 native veya tweak)?
- Per-direction offset gerekli mi (S için farklı, N için farklı)? Eğer evet, DeterministicVariantTile'a per-cell direction-aware logic eklenmeli (büyük refactor)
- N kenar için ayrı asset (top wall) gerekli mi (büyük iş — yeni sprite üretimi)?

### 4. Final Recommendation
- Tek satır: "8-dir + offset Y, spriteScale Z" formatında net karar
- Effort tahmini (config tweak XS / refactor M / new asset L)
- Risk

## Hard Constraints
- Inline only
- Net karar zorunlu — autonomous uygulanacak
- Speculative chaining yasak
- Bilgi yetersizse BLOCKED

## Beklenen uzunluk
400-600 kelime, max signal.
