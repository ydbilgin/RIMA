# ASSET_FOLDER_STRUCTURE.md
> **Ne zaman yükle:** Yeni asset klasörü oluştururken veya dosya nereye gidecek sorusunda.

---

## KLASÖR YAPISI

```
ART/
├── 00_style_reference/          ← Sanatsal referans görseller (değişmez)
│   └── references/              (Dead Cells, Hades ekran görüntüleri vb.)
│
├── 01_prototype/                ← prototype_temp aşaması assetler
│   ├── characters/
│   ├── enemies/
│   ├── props/
│   ├── tiles/
│   └── fx/
│
├── 02_candidates/               ← candidate_base + candidate_animation aşaması
│   ├── characters/
│   ├── enemies/
│   ├── props/
│   └── tiles/
│
├── 03_approved/                 ← approved_base + approved_animation + approved_final
│   ├── characters/
│   ├── enemies/
│   ├── props/
│   ├── tiles/
│   └── fx/
│
├── 04_derived_variants/         ← approved_base'den türetilmiş varyantlar
│   ├── elite_variants/
│   └── color_swaps/
│
├── 05_deprecated/               ← artık kullanılmayan, ama silinmeyen
│
├── 06_logs/                     ← tüm ASSET_LOG.md dosyaları
│   ├── characters/
│   ├── enemies/
│   └── tiles/
│
├── 07_exports/                  ← Unity'ye gidecek EXPORT_COPY'ler
│   ├── characters/
│   ├── enemies/
│   ├── tiles/
│   ├── fx/
│   └── ui/
│
├── 08_aseprite_workfiles/       ← .aseprite dosyaları (editlenebilir)
│   ├── characters/
│   └── tiles/
│
├── 09_pixellab_metadata/        ← character_id'ler, seed'ler, promptlar
│   └── [isim]_meta.json         (ASSET_LOG.md'nin ham veri versiyonu)
│
├── _REVIEW/                     ← geçici onay bekleme alanı
│   ├── player_classes/
│   ├── mobs/
│   └── bosses/
│
└── _ARSIV/                      ← eski session raporları, geçmiş
```

---

## NE NEREYE GİDER

| Asset durumu | Klasör |
|---|---|
| İlk MCP üretimi, test | `01_prototype/[tip]/` |
| South.png onay bekliyor | `_REVIEW/[tip]/` |
| Pro üretim, onay bekliyor | `02_candidates/[tip]/` |
| Sen onayladın | `03_approved/[tip]/` |
| Palette swap / tier varyantı | `04_derived_variants/` |
| Unity'e gidecek | `07_exports/[tip]/` |
| Aseprite .aseprite dosyası | `08_aseprite_workfiles/[tip]/` |
| Artık kullanılmıyor | `05_deprecated/` |
| character_id, seed kayıtları | `09_pixellab_metadata/` |

---

## UNITY SPRITES KARŞILIĞI

```
ART/07_exports/characters/warblade/  →  RIMA/Assets/Sprites/Characters/Warblade/
ART/07_exports/enemies/shardwalker/  →  RIMA/Assets/Sprites/Enemies/ShardWalker/
ART/07_exports/tiles/act1_stone/     →  RIMA/Assets/Sprites/Tiles/Act1/
ART/07_exports/fx/                   →  RIMA/Assets/Sprites/VFX/
```

Prototype Unity klasörü: `RIMA/Assets/Sprites/PROTO_[isim]/`
Final Unity klasörü: `RIMA/Assets/Sprites/[isim]/`

---

## KRİTİK KURALLAR

1. `03_approved/` içindeki hiçbir dosyaya doğrudan yazma — WORKING_COPY oluştur
2. `07_exports/` içindeki dosyalar temiz EXPORT_COPY'dir — düzenleme yapma
3. `08_aseprite_workfiles/` içindeki .aseprite dosyaları editlenebilir — buradan çalış
4. `_REVIEW/` geçicidir — onay sonrası uygun klasöre taşı
5. `09_pixellab_metadata/` character_id ve seed'lerin tek kaynağı — yedekle
