# RIMA Mobs — Canonical Sprite Production

**Status:** Clean folder, S87 reset. Tüm legacy mobs `_archive_legacy_S86/` altında — front-view / yanlış kamera açısı.

---

## LOCK Standards (Karar #100/#74/#144/#145 ile uyumlu)

| Boyut | Değer |
|---|---|
| Canvas | 64×64 (tier 1 small) / 96×96 (tier 2 medium) / 128×128 (tier 3 elite/boss) |
| Camera angle | High top-down 30-35° (chibi player'a match) |
| Body convention | Silahsız body + WeaponSR child SR (Karar #144) |
| State workflow | PixelLab Character States (Karar #145 v2 — 6 use case) |
| Direction setup | 5 produce (S, SE, E, NE, N) + 3 mirror |
| PPU | 64 |
| Outline | 1px solid black, hard pixel edges, no anti-aliasing |
| Shading | Flat cell shading, max 2 tones per color |

---

## v1 Mob Roster (target — Sprint 14+)

### Tier 1 (small, 64×64) — Phase 1 priority
1. Fracture Imp (mor demon)
2. Rift Hound (küçük yırtık kurdu)
3. Seam Crawler (spider, cyan)
4. Rift Acolyte (büyücü)

### Tier 2 (medium, 96×96) — Phase 1 priority
5. Penitent Bruiser (kara hooded melee)
6. Riftbound Augur (caster)
7. Spire Choirling (ghost flying)
8. Shard Walker (crystal hum)

### Tier 3 (elite/boss, 128×128) — Phase 1.5+
9. Hollow Arbiter (crowned king)
10. Hollow Hulk (stone golem)
11. Plate Widow (boss spider)
12. Relic Caster (book caster)

---

## Production Workflow (S87+)

1. **Mob prompt yazımı:** v1 mob prompt sheet — Karar #100 camera/proportion rules adapt to mob aesthetic (NOT chibi, but high top-down 30-35° MANDATORY)
2. **PixelLab Create Image Pro:** anchor üretim
3. **Character States workflow:** idle/walk/attack/hit/death anim states (Karar #145)
4. **Use #6 variants:** "armored variant", "elite captain", "boss form" (Karar #145 v2)
5. **Unity import:** Mobs/ klasörü altında final 64×64/96×96/128×128 PNG'ler
6. **Mob Bank ScriptableObject:** RoomBankSO integration

---

## Legacy Archive Not

`_archive_legacy_S86/` altında 12 dosya:
- 10 humanoid mob FRONT VIEW (eye-level, character portrait stili — Karar #100 ihlali)
- 2 örümcek mob top-down 80-90° (pure overhead — yine standart dışı)

**Sebep:** Karar #100 chibi 30-35° high top-down lock — legacy mob'lar farklı pipeline ile üretildi. Yeniden üretim gerekir.

**Yeniden kullanım kararı:** Hiçbiri canonical pipeline'a dahil edilemez. Tümü `_archive_legacy_S86/` altında referans olarak kalır (silinmez).
