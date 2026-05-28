# PixelLab Inventory Dump — Tamamlandı

**Tarih:** 2026-05-28 (S112 sabahı)
**Durum:** DONE — 5 batch çekildi, 243 obje kategorize edildi

---

## Batch Özeti

| Batch | Offset | Beklenen | Alınan |
|---|---|---|---|
| 0 | 0 | 50 | 50 |
| 1 | 50 | 50 | 50 |
| 2 | 100 | 50 | 50 |
| 3 | 150 | 50 | 50 |
| 4 | 200 | ~20 | 43 |
| **Toplam** | — | **220** | **243** |

Not: Task brief 220 diyordu ama API 243 döndürdü. Fark 23 — S112 sırasında yeni üretim yapılmış.

---

## Kategori Dağılımı

| Kategori | Complete | Awaiting-Selection | FAILED | Toplam |
|---|---|---|---|---|
| walls_s95 | 11 | 0 | 0 | 11 |
| walls_batch1 | 4 | 0 | 0 | 4 |
| walls_misc | 2 | 0 | 0 | 2 |
| mobs (64x64) | 16 | 0 | 0 | 16 |
| statues | 11 | 1 | 0 | 12 |
| mounting_apparatus | 15 | 1 | 0 | 16 |
| room_decor_misc | 27 | 0 | 0 | 27 |
| painterly_hand | 10 | 0 | 0 | 10 |
| painterly_topdown | 17 | 0 | 0 | 17 |
| weapons_1dir | 7 | 0 | 0 | 7 |
| weapons_8dir | 3 | 0 | 0 | 3 |
| weapons_review | 0 | 6 | 0 | 6 |
| weapons_untagged | 2 | 0 | 0 | 2 |
| vfx_anim | 2 | 0 | 0 | 2 |
| vfx_blood | 0 | 1 | 0 | 1 |
| props_crates | 2 | 1 | 0 | 3 |
| tiles_rift_cliff | 2 | 0 | 0 | 2 |
| tiles_review | 0 | 2 | 0 | 2 |
| scatter_floor1 | 16 | 4 | 0 | 20 |
| keep_decal_v2 | 8 | 1 | 0 | 9 |
| keep_wall_v2 | 4 | 1 | 0 | 5 |
| alabaster_decal | 4 | 1 | 0 | 5 |
| alabaster_wall | 4 | 1 | 0 | 5 |
| concept_mockups | 9 | 0 | 0 | 9 |
| skill_icons | 19 | 0 | 0 | 19 |
| skill_icons_special | 3 | 0 | 0 | 3 |
| misc_props | 8 | 0 | 0 | 8 |
| misc_review | 0 | 3 | 0 | 3 |
| transforms | 4 | 0 | 1 | 5 |
| cliff_face | 1 | 0 | 0 | 1 |
| floor_large | 2 | 0 | 0 | 2 |
| walls_weathered_large | 1 | 0 | 0 | 1 |
| **TOPLAM** | **218** | **23** | **1** | **242+1=243** |

---

## Local Cross-Check Özeti

| Lokasyon | PNG sayısı | Kaynak |
|---|---|---|
| Assets/Sprites/Environment/ShatteredKeep_PixelLab/Props/ | 29 PNG | 14 statue + 15 mounting |
| Assets/Sprites/Mobs/ShatteredKeep_PixelLab/ | 16 PNG | tüm 16 mob |
| Assets/Sprites/Environment/PixelLab_Selected_Assets/ | 1 PNG | alabaster_decal_5ccc5721 |
| Assets/Prefabs/Props/ShatteredKeep_PixelLab/ | 29 prefab | 1:1 statue+mounting |
| **Toplam local** | **46 PNG + 29 prefab** | — |

**Cloud-only (hiç local yok): 197 obje**

### Orphan Alert (local'de var, cloud'da yok)
- `statue_04_3675a661` — cloud list'te yok
- `statue_11_d5574785` — cloud list'te yok
- `statue_13_c5711681` — cloud list'te yok

Bu 3 ID muhtemelen başka account'ta üretilmiş veya export sırasında farklı ID ile kaydedilmiş.

---

## Kritik Bulgular

1. **64x64 mob endişesi**: 16 mob ALL S95'te, kullanıcı onayıyla üretilmiş — gece halt ihlali DEĞİL. Bunlar 1-dir referans görsel, animasyonlu karakter değil.

2. **23 Yeni obje** (task brief 220 → gerçek 243): S112 sırasında üretilmiş: `RIMA_Wall_Production_v1_Batch1` (4 adet) + scatter/keep/alabaster serisi (~20+ yeni).

3. **15 Awaiting-Selection objesi**: Kullanıcı web UI'da onay/ret bekliyor. Bunlar credit harcamaz ama silinmezse cloud'da kalır.

4. **Yeni keşfedilen kategoriler** (task brief'te yoktu):
   - 23 skill_icons (tamamen cloud-only, hiç import edilmemiş)
   - 9 concept_mockups 256x256
   - 32x32 keep/alabaster tile setleri
   - scatter_floor1 decal seti (16 complete)
   - 8-dir weapon sprites (Painterly serisi)

5. **Silme adayları için 3-AI analiz HAZIR**: orphan 3 ID + 1 FAILED transform + 15 awaiting-selection + duplicate listing pattern.

---

## Output Dosyaları

- `STAGING/PIXELLAB_INVENTORY_MASTER.md` — tam 243-obje tablo + kategori özeti + local cross-check
- `STAGING/PIXELLAB_INVENTORY_DUMP_DONE.md` — bu dosya

## Sonraki Adım

3-AI analiz (Opus + Codex + agy) için `PIXELLAB_INVENTORY_MASTER.md` temel data olarak kullanılabilir.
