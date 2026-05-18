# Autosprite Tile Inventory — RIMA Pilot

_2026-05-18 | Sadece duvar + zemin (objeler / props hariç)_

## Bağlam

User direktifi: "5 adetten ziyade gerçek uygulanabilir tile durumunu çıkarsak ya, objeler harici sadece duvar ve zemin olarak."

RIMA 8-layer canonical recipe (`memory/feedback_layered_terrain_mandatory.md`) içinde tile-tabanlı katmanlar:
- **Layer 2** — Base floor (biome'a göre, %100 cell coverage)
- **Layer 4** — Detail texture (cracks/moss/dirt, %30 sparse)
- **Layer 7** — Tall focal (walls, columns — sadece **wall part** tile bazlı; statues/banners prop)

Layer 6 medium props (rocks, bushes), Layer 5 small scatter (pebbles), Layer 8 atmospheric → tile değil, prop. **Pilot SCOPE dışı.**

## RIMA tam tile envanteri (production hedef)

### 6 zone × 4-6 biome:

| Zone | Base floor tile | Wall part | Transition pair count |
|---|---|---|---|
| path | 4-6 biome variant | top edge + side | path↔grass, path↔stone (4 var) |
| grass | 4-6 biome variant | (yok — outdoor) | grass↔path, grass↔water, grass↔stone (6 var) |
| stone | 4-6 biome variant | top edge + side | stone↔path, stone↔grass, stone↔wall (6 var) |
| wall | 4-6 biome variant | cap + side + 4 corner (NE/NW/SE/SW) | wall↔stone (2 var) |
| water | 2-3 biome variant | (yok — sıvı kenar = transition) | water↔grass, water↔feature (4 var) |
| feature | 2-3 biome variant | (özel; pre-existing) | feature↔water, feature↔stone (4 var) |

**Toplam production hedef:** ~80-100 tile (free plan 15-20 kredi'yi katlar).

## Pilot MVP set — autosprite free plan (15 tile)

Free plan: 15-20 kredi/ay. 1 tile ≈ 1-2 kredi → 15 tile = 15-30 kredi (ya tek seferde, ya 2 free ayda yayılır).

### A) Base floor (6 tile, 32×32 seamless)

Biome agnostik (test için sadece "warm amber Hades reference" tek palet):

1. `path_floor.png` — kobalt taş yol, painterly brushwork
2. `grass_floor.png` — yeşil çim, painterly Hades tone
3. `stone_floor.png` — kahve granite, painterly
4. `wall_floor.png` — koyu gri/black wall base (wall hücresi de zemin lazım)
5. `water_floor.png` — derin mavi/teal pool surface
6. `feature_floor.png` — kırmızı/kan ritual mat (Combat room feature zone)

### B) Wall part (6 tile)

Tek biome (cave/dark tone):

7. `stone_wall_cap.png` — yatay üst kenar (cliff edge horizontal)
8. `stone_wall_side.png` — dikey yan kenar (cliff edge vertical)
9. `stone_wall_corner_NE.png` — köşe NE (mirror'la NW yapılır)
10. `stone_wall_corner_SE.png` — köşe SE (mirror'la SW)
11. `wall_focal_pillar.png` — tall vertical column (Layer 7 focal)
12. `wall_focal_arch.png` — yatay arch geçit

### C) Kritik transition (3 tile)

En kritik 3 adjacency pair:

13. `transition_path_to_grass.png` — taş yolun çim'e ufalanan kenar
14. `transition_grass_to_water.png` — çimin suya geçişi (reeds/wet edge)
15. `transition_stone_to_wall.png` — taş zemin'in duvar dibine yanaşması

## QC karşılaştırma metodolojisi

Autosprite çıktısı + mevcut Codex imagegen `Assets/Data/Brush/AssetParts_v3/` set'i yan yana koy. Unity'de 4×4 tile grid'de yay:

1. **Seam görünürlüğü** — yan yana 16 tile, dikiş yerleri belli mi?
2. **Palette tutarlılığı** — 6 base floor aynı warm amber Hades palette'ten geliyor mu?
3. **Painterly kalitesi** — fırça izleri Codex imagegen ile karşılaştırılabilir mi?
4. **Wang16-ready** — wall corner tile'lar mevcut Wang Full 16 corner set'e (memory `karar-143-layered-pipeline`) drop-in uyuyor mu?

## Promptlar (autosprite'da çalıştırılacak)

### Base floor template (6 zone)
```
top-down [ZONE] floor tile, painterly brushwork visible, warm amber palette (Hades reference),
seamless 32×32, no shadows, transparent alpha optional, no character no prop,
2D pixel art, low detail, high contrast edges
```

### Wall part template
```
top-down stone wall [PART], dark cave palette, painterly brushwork,
seamless tileable, 32×32, vertical cliff edge, no prop no decoration,
Hades dungeon reference, 2D pixel art
```

### Transition template
```
top-down terrain transition [ZONE_A] to [ZONE_B], painterly blend,
seamless 32×32, scatter pixels at boundary, no prop, Hades reference,
2D pixel art, low detail
```

## Verdict gate

15 tile üretildikten sonra:
- ✅ Seam ≥ Codex imagegen quality → **Karar #157 hybrid pipeline update**: tile rolünü autosprite'a kaydır
- ◐ Seam = Codex quality, palette > Codex → Codex tile + autosprite palette post-pass
- ✗ Seam < Codex quality → autosprite'ı tile rolünden çıkar, sadece VFX rolü için tut

## Out of scope (pilot)

- Karakter (PixelLab kalır)
- Props/decals (Codex imagegen kalır — Layer 4-6)
- 8-dir animation (PixelLab)
- VFX (ayrı pilot, slash/spark/burst için)
- Biome variant explosion (6 × 6 = 36 — production faz, pilot 1 palet)

## Sonraki adım

User Unity hazır olunca + UnityMCP fix DONE olunca: pilot başlatılır.
User isterse free plan trial **şimdi** açabilir (autosprite.io signup), 15 tile listesini elle promptlar üzerinden üretir. Çıktı `Assets/Data/Brush/AssetParts_autosprite_pilot/` altına gelir, ben Unity'de QC grid yaparım.
