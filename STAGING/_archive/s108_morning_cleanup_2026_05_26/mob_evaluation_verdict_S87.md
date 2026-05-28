---
status: VERDICT
faz: 1
tarih: 2026-05-18
ozet: "12 legacy mob inceleme — açı verdict, archive aksiyonu, regen scope"
karar_ref: #100 (chibi 30-35°), #74 (64×64), #144 (silahsız body), #145 v2 (states)
---

# Mob Evaluation — S87 Verdict

## Özet
**12 legacy mob incelendi.** Hiçbiri Karar #100 30-35° high top-down + chibi standardına uymaz. Hepsi `Mobs/_archive_legacy_S86/` altında arşivlendi. Yeniden üretim Sprint 14+ kapsamında.

---

## Slot-by-slot Verdict

| # | Dosya | Tipi | Görsel açı | Verdict |
|---|---|---|---|---|
| 1 | `fracture_imp.png` | Mor demon/imp | **FRONT VIEW** (eye-level, full-body portrait) | ❌ REGEN |
| 2 | `relic_caster.png` | Hooded book-holder | **FRONT VIEW** (character portrait) | ❌ REGEN |
| 3 | `seam_crawler.png` | Cyan spider | Top-down ~80-90° (too overhead, not 30-35°) | ◐ Borderline REGEN |
| 4 | `plate_widow.png` | Skull spider | Top-down ~80-90° (too overhead) | ◐ Borderline REGEN |
| 5 | `hollow_arbitter.png` | Crowned king | **FRONT VIEW** (regal portrait) | ❌ REGEN |
| 6 | `rift_gound.png` | Small hound | **FRONT VIEW** | ❌ REGEN |
| 7 | `11_spire_choirling.png` | Hooded ghost flying | **FRONT VIEW** (slight elevation but flat) | ❌ REGEN |
| 8 | `12_shard_walker.png` | Crystal humanoid | **FRONT VIEW** | ❌ REGEN |
| 9 | `13_penitent_bruiser.png` | Dark hooded muscular | **FRONT VIEW** | ❌ REGEN |
| 10 | `14_riftbound_augur.png` | Hooded female caster | **FRONT VIEW** | ❌ REGEN |
| 11 | `15_hollow_hulk.png` | Stone golem | **FRONT VIEW** | ❌ REGEN |
| 12 | `16_rift_acolyte.png` | Hooded orb caster | **FRONT VIEW** | ❌ REGEN |

**Sonuç:** 10 humanoid kesin REGEN, 2 örümcek borderline REGEN (pure overhead → 30-35° tilt'e adjust gerekir).

---

## Ana sorunlar

### 1. Kamera açısı tutarsız
- 10 humanoid mob: **FRONT VIEW / EYE-LEVEL** — Karar #100 (30-35° high top-down) ihlali
- 2 örümcek mob: **PURE OVERHEAD ~80-90°** — yine standart dışı, player chibi'siyle uyumsuz
- **Player chibi (30-35°) vs mob (0° flat veya 90° overhead) = visual mismatch** ekrana yansıyacak

### 2. Sprite proportions tutarsız
- Mob'lar daha "realistic body proportion" (5-6 head height)
- Player chibi 3-4 head height
- Yan yana ekrana geldiğinde proportion shock olur

### 3. Stil farkı
- Mob'lar farklı zaman/pipeline'da üretilmiş — palette + outline + shading farkı
- Karar #74 hard pixel + no anti-aliasing + max 2 tone shading discipline

---

## Aksiyon

1. ✅ **Mobs/ klasörü temizlendi** — tüm legacy `_archive_legacy_S86/` altında
2. ✅ **`Mobs/README.md` yazıldı** — production standards + v1 roster + workflow
3. ⏳ **Sprint 14+ mob prompt yazımı** — Karar #100 chibi 30-35° kuralı ile yeni mob anchor prompt (v1)
4. ⏳ **Tier 1 öncelik** — Fracture Imp + Rift Hound + Seam Crawler + Rift Acolyte (4 mob, 64×64)
5. ⏳ **Tier 2 sonra** — 4 medium (96×96)
6. ⏳ **Tier 3 elite/boss** — 4 large (128×128), Phase 1.5+
7. ⏳ **Use #6 variant matrix:** Her tier 1-2 mob × 3 variant = 24+ effective mob roster (Karar #145 v2)

---

## Map Designer'a geçiş hazırlığı

Mob klasörü temiz → ready for Sprint 14:
- Map Designer ile parallel Combat integration (player skills + 10 sınıf canonical anchors)
- Mob production Sprint 14 sonu / Sprint 15 başı
- Boss room procgen Sprint 16+

**Önce ne yapacağız:**
1. Canonical character anchor PixelLab create (user task)
2. Map Designer'da room production (combat integration için)
3. Mob production parallel başlar (Sprint 14)
