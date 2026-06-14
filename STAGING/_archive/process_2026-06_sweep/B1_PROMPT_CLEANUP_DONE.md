# B1 PROMPT CLEANUP — DONE

**Tarih:** 2026-05-28  
**Task:** b1_animation_catalog_weaponless_cleanup_task.md  
**Scope:** STAGING/ANIMATION_PROMPT_CATALOG.md — weaponless variant cleanup  
**Status:** COMPLETE

---

## Verify Checklist

- [x] **CHARACTER block (Section 4.1)** — "two-handed greatsword" kaldırıldı. "EMPTY HANDS, fists loosely clenched in weapon-ready grip posture" eklendi.
- [x] **CONSTRAINTS block (Section 4.3)** — "WEAPON LOCK: weapon stays firmly in hands, do not use words drop, release, slip, fall, throw." kaldırıldı. "NO weapons, NO held items, hands empty throughout." eklendi.
- [x] **11 anim ACTION block — tümü weaponless:**
  - A — Idle: arm + fist language, weapon reference kaldırıldı
  - B — Walk: arm swing natural, greatsword kaldırıldı
  - C — Basic Attack: arm arc motion guide (greatsword path trace note — intentional hand anchor compatibility), apex state `warblade_attack_LMB_apex_weaponless_state`
  - D — Hurt: arm flail + guard raise, greatsword kaldırıldı
  - E — Death: hand on chest fall, greatsword kaldırıldı
  - F — Iron Charge: shoulder + arm motion guide, greatsword kaldırıldı
  - G — Earthsplitter: two-hand fists clasped, greatsword kaldırıldı (overlay note korundu)
  - H — Gravity Cleave: arm extension arc, blade ref kaldırıldı
  - I — Death Blow: arm wind-back + extension, greatsword kaldırıldı
  - J — Iron Counter: forearm barrier + counter arc, sword kaldırıldı
  - K — Sunder Mark: fingertip cast, sword kaldırıldı
- [x] **Karar #71 (Section 1.4)** — "WEAPONLESS PRODUCTION'DA UYGULANMAZ" başlık eklendi. Eski text strikethrough yapıldı. Yeni açıklama: weapon mount Karar #144+#123+#146 ile yapılır.
- [x] **Karar #108 (Section 1.8)** — SUPERSEDED note eklendi. Yeni cost mapping tablo: 4f=1 / 6-8f=2 / 10-12f=3 / 14-16f=4 gen/dir.
- [x] **Cost tablolar güncellendi (S114 V3 mapping):**
  - Tier 1 south-only: ~28-52 gen (eskisi: ~31-58 gen)
  - Tier 2 south-only: ~120-220 gen (eskisi: ~126-230 gen)
  - Toplam south-only: ~148-272 gen (eskisi: ~150-290 gen)
  - 5-dir expansion: ~740-1360 gen (eskisi: ~750-1450 gen)
- [x] **Cross-link canonical references (bottom)** — Karar #71 ve #108 notları güncellendi.
- [x] **Risk Section 8 item 1** — cost totals S114 ile uyumlu.
- [x] **Risk Section 8 item 3** — flipX note weaponless body için güncellendi.
- [x] **Apex State naming** — `attack_LMB_apex` → `attack_LMB_apex_weaponless` (Section 5 table + Section C table).

---

## Değişen Satırlar Özeti

| Bölüm | Değişiklik |
|---|---|
| Section 1.4 | Karar #71 başlık + strikethrough + yeni açıklama |
| Section 1.8 | SUPERSEDED note + yeni V3 cost mapping tablosu |
| Section A Idle prompt | greatsword → arm/fist language |
| Section B Walk prompt | greatsword → arm swing |
| Section C apex state adı | `_apex_state` → `_apex_weaponless_state` |
| Section C cost cell | ~3 gen → 2 gen (S114) |
| Section C Part 1 prompt | greatsword → arm arc motion guide |
| Section C Part 2 prompt | greatsword recovery → arm recovery |
| Section D Hurt prompt | greatsword arm → right arm flail |
| Section E Death prompt | greatsword fallen → empty hand |
| Section F Iron Charge cost | ~26-46 → 24-44 gen |
| Section F Part 1+2 prompts | greatsword → shoulder/arm |
| Section G Earthsplitter cost | ~26-46 → 24-44 gen |
| Section G Part 1+2 prompts | greatsword → two-hand clasped fists |
| Section H Gravity Cleave cost | ~26-46 → 24-44 gen |
| Section H Part 1+2 prompts | blade → arm extension |
| Section I Death Blow cost | ~26-46 → 24-44 gen |
| Section I Part 1+2 prompts | greatsword → arm wind-back/extension |
| Section J Iron Counter cost | 21-43 → 22-42 gen |
| Section J prompt | sword block → forearm barrier |
| Section K Sunder Mark cost | 1-3 → 2 gen |
| Section K prompt | greatsword tip → fingertip cast |
| Section 4.1 CHARACTER block | two-handed greatsword → EMPTY HANDS + weapon-ready grip |
| Section 4.3 CONSTRAINTS block | WEAPON LOCK → NO weapons language |
| Section 5 apex table | attack_LMB_apex → attack_LMB_apex_weaponless |
| Section 8 Risk cost total | ~150-290 → 148-272 gen |
| Section 8 Risk flipX note | weaponless body clarification |
| Section 9 Opus sentez total | updated gen totals |
| Cross-link Karar #71 ref | YASAK note |
| Cross-link Karar #108 ref | SUPERSEDED note + new mapping |

---

## BLOCKED Flag

Karar #120 (split-animation pipeline) weaponless conflict YOK. Apex frame hand position weapon mount ile uyumlu:
- Basic Attack apex: `right arm fully extended in wide horizontal arc` → HandAnchor S direction offset (0.00, -0.08) ile uyumlu (weapon sword will attach to extended right hand at apex)
- Tüm apex state'ler right arm/hand açık grip pozisyonda → OrientationSync rotation + WeaponSorter sort order sorunsuz çalışır.

---

## Dosya

`STAGING/ANIMATION_PROMPT_CATALOG.md` — ~26.6 KB → ~29 KB (tahmini)
