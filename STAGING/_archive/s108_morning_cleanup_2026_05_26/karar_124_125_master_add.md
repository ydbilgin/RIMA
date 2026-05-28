# MASTER_KARAR — Karar #124 + #125 + #18 HYBRID — Execute every step, commit at end

## Context

rima-design (Opus) weapon variation extension önerdi (`STAGING/weapon_variation_extension.md`):
- **Karar #124 Weapon Form Variation** — LOCK, Faz 1 MVP reduced (Warblade Base + T2 Rift only)
- **Karar #125 Extra Weapon Attach** — LOCK + DEFER to Faz 2+ (Faz 1'de SIFIR extra)
- **Karar #18 HYBRID pozisyon** — "no stat-bearing equipment slot" core korunur, "class identity-bound secondary weapon" boundary extend

## STEP 1 — TASARIM/MASTER_KARAR_BELGESI.md'ye 2 yeni karar entry ekle

Yer: **Karar #123'ten sonra** (Yol A weapon decouple, en son LOCK). Table row format (#119/#122/#123 ile aynı):

### #124 — Weapon Form Variation — LOCKED 2026-05-14
```
| #124 | Weapon Form Variation | Karar #123 (Yol A) extension — aynı silah arketipi runtime'da farklı sprite (tier evolution, rift-empowered skin). **Faz 1 MVP reduced scope:** sadece Warblade × 2 form (Base çelik + T2 Rift-cracked). **Faz 2 full matrix:** 10 class × 3-5 form ≈ 32 weapon sprite (~8 saat). **Trigger architecture 3-layer:** (1) Karar #122 T3 Empowered Skill cast → temporary form, (2) Karar #18 Relic persistent → run-içi kalıcı form, (3) Karar #122 T4 Rift Proc Bond → 3-tag stack temporary. Aynı animation clip + farklı sprite (Yol A decouple free). Karar #120 split-animation + Karar #122 NATIVE integration. Detay: `STAGING/weapon_variation_extension.md`. | 2026-05-14 |
```

### #125 — Extra Weapon Attach (Class Identity-Bound Secondary) — LOCKED + DEFER Faz 2+ 2026-05-14
```
| #125 | Extra Weapon Attach (Class Identity-Bound Secondary) | Karar #123 (Yol A) extension — class doğuştan sahip olduğu **secondary weapon** sprite + Unity attach (Karar #18 HYBRID — drop-able stat-bearing equipment DEĞİL, kimliğe bağlı). **10 class secondary roster:** 8 LOCK + Elementalist REVISE + Brawler REVISE-to-#124. **Kategori A (skill prop, mechanic veriyor):** Warblade kalkan (parry window), Ronin tanto (off-hand stab), Shadowblade throwing dagger (aux ranged proc), Ravager throwing hatchet, Hexer trinket. **Kategori B (visual only):** Ranger belt-dagger, Gunslinger bandolier, Summoner bone fetish. **Faz 1: SIFIR extra weapon** (25-gün deadline scope creep riski). **Faz 2+: production rollout.** Slot architecture: OffHandAnchor transform (Yol A Level 2 per-frame anchor zorunlu). Detay: `STAGING/weapon_variation_extension.md`. | 2026-05-14 |
```

## STEP 2 — Karar #18 satırına HYBRID annotation ekle

MASTER_KARAR_BELGESI.md'de **Karar #18** satırını bul (Item System D ile ilgili). Sonuna ekle:

```
**HYBRID extension 2026-05-14 (Karar #125 entegrasyon):** Core "no stat-bearing equipment slot, drop economy yok" korunur. **Genişleme:** "Class identity-bound secondary weapon" (Karar #125) istisna — secondary weapon class doğuştan sahip, run-içi drop YOK, stat eklemiyor (sadece mechanic/visual). Karar #18'in özü (relic + skill modifier ekonomisi) bozulmaz.
```

## STEP 3 — FAZ_MASTER karar sync güncelle

`TASARIM/FAZLAR/FAZ_MASTER.md` Karar sync tablosu (lint fix S70'te #72-#123 condensed eklenmişti). 2 yeni satır ekle:

- #124 (Weapon Form Variation) → Faz 1 reduced (Warblade × 2), Faz 2 full matrix
- #125 (Extra Weapon Attach) → DEFER Faz 2+, Faz 1 sıfır

## STEP 4 — Commit

```bash
git add TASARIM/MASTER_KARAR_BELGESI.md TASARIM/FAZLAR/FAZ_MASTER.md
git commit -m "[karar] #124 weapon form variation + #125 extra weapon + #18 HYBRID

- #124 LOCK: Yol A extension, Faz 1 reduced (Warblade Base+T2 Rift)
- #125 LOCK + DEFER Faz 2+: 8 class secondary roster (Karar #18 HYBRID)
- #18 annotation: no stat-bearing slot korunur, class-bound secondary istisna
- Source: rima-design Opus brief STAGING/weapon_variation_extension.md"
```

## STEP 5 — CODEX_DONE.md update

Append:
```
## [2026-05-14 S70] Karar #124 + #125 + #18 HYBRID — MASTER_KARAR extension

- TASARIM/MASTER_KARAR_BELGESI.md: +2 karar (#124, #125) + #18 HYBRID annotation
- TASARIM/FAZLAR/FAZ_MASTER.md: +2 sync row
- Source: rima-design Opus weapon_variation_extension.md
Commit: [hash]
```

## CONSTRAINTS

- Execute every step
- Do NOT touch existing #119/#122/#123 entries (lint fix'te eklendi)
- Do NOT touch user-memory MEMORY.md (Codex erişimi yok auto-memory'e)
- Do NOT modify STAGING/weapon_variation_extension.md (rima-design source, korunur)
