# Codex Task — Review Two Master Sheet Candidates + Production Verdict

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

İki ayrı kaynaktan modular wall asset pack master sheet üretildi. **Kalite karşılaştırması + production verdict + slicing recommendations** yap.

## Input Images

1. **Codex imagegen v1:**
   - Path: `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_codex_v1.png`
   - Boyut: 512×512 RGBA
   - Source: Codex imagegen (gpt-image-1)

2. **ChatGPT (DALL-E 3) v1:**
   - Path: `STAGING/_pixellab_outputs/walls/v2/act1_wall_modular_pack_chatgpt_v1.png`
   - Boyut: 1024×1024 RGBA (büyük olasılıkla, kontrol et)
   - Source: ChatGPT manuel dispatch by user (DALL-E 3 backend)

## Beklenen Tile Içeriği (her iki sheet için)

52 tile, 4 section:

### Section A — Features (4 büyük tile)
- archway_full (cyan portal içinde)
- big_corner (L-shape)
- big_column (tall pillar)
- wall_tall_hero (straight wall)

### Section B — Modular Walls (16 medium tile, 2 rows)
- straight_NE, straight_NW, corner_outer_a/b, corner_inner_a/b, T_junction_a/b
- endcap_a/b, low_wall_str, low_wall_corner, low_wall_end, foundation_a/b, floor_edge

### Section C — Rift Overlays (16 small tile)
- crack_h, crack_v, burst_s, burst_l, scar_a, scar_b, glow_a, glow_b, drop_a, drop_b, spiral, zigzag, pulse_a, pulse_b, burst_h, burst_v

### Section D — Decoration (16 small tile)
- moss_a/b/c/d, candle_a/b, torch_unlit/lit, banner_a/b, chain_short/long, scatter_stone, dust_pile, skull_floor, gem_pickup

## Görev Detayı

### A) Kalite Karşılaştırması (her iki sheet için)

Her sheet için Python PIL ile basit analiz yap:
- Image dimensions verify
- Distinct tile count (alpha-based bbox detection)
- Color palette analysis (dominant colors, cyan presence)
- Pixel art quality check (color count, smooth-vs-sharp edges via gradient detection)
- Transparent background validation

Output:
```
| Aspect | Codex imagegen | ChatGPT | Winner |
|---|---|---|---|
| Image size | ... | ... | ... |
| Tile distinctness | ... | ... | ... |
| Cyan rift palette | ... | ... | ... |
| Pixel art density | ... | ... | ... |
| RIMA atmosphere fit | ... | ... | ... |
```

### B) Per-Section Visual Inspection

Her section için (A/B/C/D) hangi tile'lar **görünür ve identifiable**, hangileri **eksik veya unclear**.

### C) Slicing Plan

Yüksek-kaliteli olan sheet için tile-by-tile **crop coordinate** önerisi:
- Image dimensions altında her section'ın Y aralığı
- Her tile'ın approximate (X1, Y1) - (X2, Y2)
- Çıktı: 52 ayrı PNG (her tile)
- Save path: `Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_modular_v1/tile_<name>.png`

### D) Production Verdict

Final recommendation:
- Hangi sheet **PRODUCTION-READY**?
- Eksik tile'lar var mı? (Phase 2'ye eklenecekler)
- Wall_archway için "empty + animated rift" pattern öneriyor muyuz?
- RuleTile setup için tile naming convention
- Unity tile asset PPU önerisi

### E) Next Steps Roadmap

1. Slicing approach (Codex Python script)
2. Unity import settings (PPU, filter mode, sprite mode)
3. RuleTile config önerisi
4. PlayableRoom_v2 test paint plan
5. Missing pieces production list (Phase 2)

## Output

Tek dosya: `STAGING/_research/MASTER_SHEET_REVIEW_VERDICT.md`

İçerik:
1. Executive Summary (2-3 madde, verdict)
2. Quality comparison tablo
3. Per-section visual inspection
4. Slicing plan (winner sheet için)
5. Missing tiles + Phase 2 list
6. Unity import + RuleTile recommendations

## Effort

medium-high — visual analysis + concrete recommendations gerekiyor.

## Constraint

Bu task **kararsal**: hangi sheet kullanılacak, ne eksik, sonra ne yapılacak. PASS/FAIL/REVISE verdict net olsun.

## NLM Hint

NLM olmayabilir gerekli ama RIMA visual signature için query yapılabilir:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "Act 1 Shattered Keep visual palette + lore"`
