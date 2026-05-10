# PixelLab Production — Master Index
*Son güncelleme: 2026-05-09 | Klasör sırası = üretim sırası*

Bu klasör altındaki numaralı alt klasörler **üretim sırasına göre** dizilmiştir. Aç, içindeki `HOWTO.md` ve `PROMPT.md`'i oku, üret, çıktıları `outputs/` altına koy.

---

## Klasör Sırası

| # | Klasör | Ne | Tool | Tahmini kredit |
|---|---|---|---|---|
| 01 | `01_NEXT_walls/` | W1 + W2 + OBW duvar tile'ları | Create Tile — Isometric | ~150-200 |
| 02 | `02_NEXT_floors/` | F1 + F2 + F3 + 2 transition floor | Create Tiles Pro (Isometric) | ~120-180 |
| 03 | `03_NEXT_obstacles/` | Pillar/rubble/torch/crack/barrel/bone | Create Image S-XL (new) | ~100-150 |
| 04 | `04_NEXT_Warblade_anim/` | Warblade 7 anim (referans sınıf) | Web App pipeline (Master) | ~750-1100 |
| 05 | `05_NEXT_Ranger_anim/` | Ranger 7 anim (asimetrik, 4 yön) | Web App pipeline | ~750-1100 |
| 06 | `06_NEXT_Shadowblade_anim/` | Shadowblade 7 anim (asimetrik) | Web App pipeline | ~750-1100 |
| 07 | `07_NEXT_Elementalist_anim/` | Elementalist 7 anim (silahsız) | Web App pipeline | ~750-1100 |

> **4 ana class:** Warblade (Melee), Ranger (ShotCadence), Shadowblade (VeilStrike), Elementalist (CastRhythm). BasicAttackProfile contract'ı bu 4'te tanımlı. Diğer 6 sınıf v2 sprintinde.

---

## Workflow

1. **Aç** → numaralı klasörü seç (01'den başla)
2. **Oku** → `HOWTO.md` (tool, settings, adımlar) + `PROMPT.md` (her tile/anim için ayrı prompt)
3. **Üret** → PixelLab web app veya MCP — çıktıyı `outputs/<alt>/...` klasörüne PNG olarak kaydet
4. **Bitirince** → klasör adına `_DONE_` prefix ekle, ya da içeriği `_DONE/` altına taşı

---

## Pipeline Referansları

- **Master Art Pipeline:** `GUIDES/RIMA_MASTER_ART_PIPELINE.md` (LOCKED v1)
- **PixelLab Production Guide:** `GUIDES/PIXELLAB_PRODUCTION_GUIDE_v1.md`
- **Animation Bible:** `TASARIM/ANIMATION_BIBLE.md` (7 anim, 4 yön + mirror)
- **Style anchor:** `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`

## Class Energy (animation klasörlerinde tekrar yazılı)

| Class | Accent | Yasak |
|---|---|---|
| Warblade | Cold blue #7BA7BC | El glow, mor |
| Ranger | Cold blue #7BA7BC | Mor |
| Shadowblade | Void purple | — |
| Elementalist | Element bazlı (Fire/Frost/Lightning/Light) | Void energy |

## Unity Import Standartları (LOCKED, hepsi için)

| Setting | Value |
|---|---|
| PPU | 64 |
| Filter Mode | Point |
| Compression | None |
| Pivot (floor) | Center (0.5, 0.5) |
| Pivot (wall/character) | Bottom-center (0.5, 0.0) |
| Sprite Mode (anim) | Multiple |

## process_tiles.py (chromakey green #00FF00)

```
python STAGING/process_tiles.py --source <png> --output Assets/Art/Tiles/Act1/<klasör> --cols N --rows M --width W --height H --prefix <pref>_
```

Filter: `G>200 AND R<60 AND B<60`. Binary alpha snap. Detay: `STAGING/process_tiles.py`.

## Bittikten sonra

- Klasörü `_DONE/` altına taşı **VEYA** klasör adına `__DONE_` prefix ekle.
- Yeni Act/sınıf üretimleri için yeni numara aç (`08_NEXT_Brawler_anim` vs.).
- `_ARCHIVE/` eski (v1/v2) prompt dosyalarının yedeği — bu klasörü silme, ama içine bakmak da gerekmez.
