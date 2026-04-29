# PRODUCTION_BACKLOG.md
> **Ne zaman yükle:** Art üretim session'ı başında ne üretileceğine bakarken.

---

## MEVCUT DURUM (2026-04-02)

**approved_final:**
- [done] Warblade — 8 yön, 47 clip, Unity entegrasyonu ✓
- [done] ShardWalker — 4 yön, 4 animasyon, Unity ✓
- [done] VoidThrall — 4 yön, 4 animasyon, Unity ✓

**In progress:**
- [active] SeamCrawler — BASE + idle/walk tamamlandı, attack+death bekliyor

---

## VALIDATION BATCH (ŞİMDİ — büyük üretim öncesi test)

| # | Asset | Araç | Aşama hedefi | Gen |
|---|---|---|---|---|
| 1 | Warblade hero re-check | `create_character` pro, 96px, 8 yön | candidate_base | ~30 |
| 2 | ShardWalker enemy | `create_character` standard, 64px, 8 yön | prototype_temp | 8 |
| 3 | Broken pillar prop | `create_tiles_pro`, 32px, seed=7, 1 tile | prototype_temp | — |
| 4 | Act 1 floor tile | `create_tiles_pro`, 16px, seed=42, 4 tile | prototype_temp | — |
| 5 | Slash VFX concept | Aseprite manuel, 32x32, 5 frame | animation_test | — |

**Onay ver → validation batch başlar.**

---

## FAZ 1 — KALAN ASSETLER

### Gameplay kritik
- [ ] [next] SeamCrawler attack+death — template anim (8 gen)
- [ ] [next] Act 1 floor tile — `create_tiles_pro` seed=42 — prototype_temp
- [ ] [next] Act 1 wall tile — `create_topdown_tileset` stone+rift crack — prototype_temp
- [ ] [next] Iron Warden boss — 128px pro, 8 yön — candidate_base (onay bekler)

### Opsiyonel Faz 1
- [ ] Hit spark VFX — Aseprite manuel, 32px, 5 frame
- [ ] Sword trail VFX — Aseprite manuel, 64x32, 4 frame
- [ ] Skill ikonları (3 adet) — `create_tiles_pro` 64px

---

## FAZ 2 — ASSET LİSTESİ

| Asset | Tip | Boyut | Aşama | Öncelik |
|---|---|---|---|---|
| Elementalist | character pro | 96px | — | yüksek |
| Shadowblade | character pro | 96px | — | yüksek |
| Ranger | character pro | 96px | — | yüksek |
| EchoHound | enemy standard | 80px | — | orta |
| The Twice-Born (×2) | enemy standard | 64px | — | orta |
| Act 1 tileset full | tileset | 16px | — | yüksek |
| Kapı sprite | prop | 32px | — | orta |
| Map fragment pickup | prop | 16px | — | düşük |

---

## FAZ 3+ (ertelenmiş)
- Act 2 tileset (mor/organik)
- Fractured King boss (160px)
- Hub tileset + 4 NPC
- Spirit NPC'leri
- Kalan 4 class
- Hollow Sovereign, Nexus Core

---

## ÜRETIM KURALLARI

```
1 pro karakter = 8 job → Tier 2 dolar
Paralel: tile üretimi + animasyon (farklı karakter)
Kuyruğa alma: pro job bitmeden yeni pro başlatma
```

| Asset tipi | Tahmini gen |
|---|---|
| Character pro (8 yön) | ~30 gen |
| Template anim full set (6 × 8 yön) | 48 gen |
| Tile batch (tiles_pro) | flat, gen sayılmaz |
| Enemy standard (8 yön) | 8 gen |
