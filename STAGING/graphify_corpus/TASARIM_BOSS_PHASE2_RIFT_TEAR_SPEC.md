---
status: LOCKED
faz: 1
tarih: 2026-05-14
ozet: "Penitent Sovereign Faz 2 Rift Tear+Bloom spesifikasyonu"
---
# BOSS_PHASE2_RIFT_TEAR_SPEC.md
**STATUS: KARAR ADAYI 2026-05-13 — Penitent Sovereign Faz 2 Hazard Introduction**
**Boss spec LOCKED (NLM teyit), bu doc 6 design decision + accessibility + tutarlilik check.**

---

## ONEMLI DUZELTME — CURRENT_STATUS HATASI

CURRENT_STATUS.md S60 Session 2'de "Rift Bloom DEGIL, mevcut Rift Tear genisletilir" notu YANLIS.
NLM teyit (2026-05-13):
- Faz 1'de Rift Tear YOK (Litany of Restraint var)
- Faz 2'de Rift Tear 3m + Rift Bloom 8s cycle INTRODUCED (hazard introduction, escalation degil)
- Faz 3'te Rift Tear KALIR + Echo Phantom Summon eklenir

Konu rima-design'a iletildi — Karar #84 ihlali YOK cunku Faz 2 spec zaten Rift Bloom iceriyor (LOCKED baseline).

---

## Niyet

Penitent Sovereign boss fight uclu yapi icinde Faz 2 ("Kirilan Zincir") spec'i NLM uzerinde LOCKED. Bu dokuman Faz 2'nin neden bu sekilde kuruldugundan degil, alinin 6 design decision'in neyi cozdugundan bahseder: phase transition cooldown, visual sinerjinin okunakli kalabilmesi, accessibility telegraph protokolu, V Burst interaksiyonu, ve mob spawn red-line. Hedef sabah kullanici confirm'ine hazir, implementation-ready referans.

Faz 2 mekanikleri (Rift Tear + Rift Bloom) zaten NLM Faz 2 LOCKED baseline'inda. T3 sizdirmasi iddiasi (Decision 2) mooted — bu doc o karari kapatiyor.

---

## 3-Phase Boss Ozeti (referans)

| Faz | HP | Posture | Lore Adi | Hazard | Yeni Skill |
|-----|-----|---------|----------|--------|------------|
| 1 | 100-66% | 700 | Zincirin Altinda | Litany of Restraint (4 chain anchor) | Chain Whip + Penitent Surge + Shackle Cast |
| 2 | 66-33% | 850 | Kirilan Zincir | Rift Tear 3m + Rift Bloom 8s cycle | Fracture Strike + Chain Detonation + Shackle Cast |
| 3 | 33-0% | 1000 | Sovereign Awakened | Echo Phantom Summon + Rift Tear KEEP | Fracture Charge + Sovereign's Wrath + hizlandirilmis tekrarlar |

---

## Faz 1 Baseline (referans)

- Posture: 700
- Chain Whip: 0.8s telegraph, 6m duz cizgi, 30 hasar, 1.0s window
- Penitent Surge: 1.2s telegraph, 4m radius itme + 35 hasar, 1.5s window
- Shackle Cast: 1.0s telegraph, 8m menzil tek hedef, 2s %50 slow + 15 hasar, 1.2s window
- Litany of Restraint (env): 4 chain anchor pusula koselerinde, 40 HP/anchor destroyable, chain line gecmek = 1.5s slow + 15 dmg

---

## Faz 2 Spec — Detay

### Trigger

- Esik: %66 HP
- Cinematic: 1.5s (Sovereign yere coker, gogsuunden mor isik, zincirler kirilir, hiz +%30, arena merkezinde Rift Tear belirir)
- **DECISION 4 (i-frame):** Cinematic boyunca full invuln — oyuncu hasar almaz, V Burst windup baslatilabilir
- **DECISION 1 (re-trigger cooldown):** Gecis tetiklendikten sonra 8s boyunca Faz 3 re-trigger blocked — oyuncu burst penceresiyle Faz 2'yi atlamamali

Posture %50 + HP %60 backup overlap onceligi: posture FIRST, HP %60 yalnizca 60s stall durumunda devreye girer (OPEN QUESTION 2).

### Skill Set (Faz 2)

| Skill | Telegraph | Etki | Window | Not |
|-------|-----------|------|--------|-----|
| Fracture Strike | 0.5s tell | 3-hit combo, 22 dmg/hit | 0.8s | Parry acik |
| Chain Detonation | 1.0s tell + 2.5s detonation | 3 nokta 2m radius 40 dmg | 1.5s | Shackle Cast gap zorunlu |
| Shackle Cast | 1.0s telegraph | 8m tek hedef, 2s %50 slow + 15 dmg | 1.2s | Faz 1 ile ayni, korunuyor |

**Overlap-safety:** Shackle Cast → Chain Detonation arasi min 0.8s gap zorunlu (ayni anda slow + patlama cognitive overload).

Hiz: Faz 1'e gore +%30 artis (gerekli — Rift Bloom alan kisitlamasiyla dengede).

### Cevre Tehlikesi: Rift Tear + Rift Bloom

#### Rift Tear (3m radius hazard)

- Konum: arena merkezinde sabit, Faz 2 basinda olusur, Faz 3'e kadar kalir
- Dash lane: tek-yonlu olur (Faz 1'deki 2-yonlu serbestten geri alinir)
- Temas hasari: TBD playtest tuning (NLM'de spesifik deger yok — OPEN QUESTION 4)
- **DECISION 3 visual:** cyan #00FFCC core @ %100 alpha + hard edge outline 2px dark navy #001A33

#### Rift Bloom (Periyodik void crack)

- Cycle: her 8s yeni crack olusur (arena yavas yavas daralir)
- Telegraph: 1s pre-spawn (crack zeminde seklini alirken)
- Crack hasari: 25 dmg/s DoT + %30 slow (uzerinde durulmasi durumunda)
- **DECISION 3 visual:** cyan #00FFCC @ %100 alpha + hard edge outline 2px dark navy #001A33 (Rift Tear ile ayni kural)

### Telegraph Spec — Accessibility (DECISION 4)

Her Rift Bloom crack icin 3-kanal telegraph:

1. **Outline thickness pulse:** 0px -> 4px -> 0px @ 0.6s — renge bagimsiz, primary cue
2. **Ground shake:** subtle, max 2px ekran shake — audio-off backup (slider 0-2px erisilebilirlik ayari)
3. **Cyan glow:** dekoratif / uc uncu kanal (deuteranopia/protanopia icin zayif, tek basina guvenilmemeli)

Rift Tear telegraph: ayni 3-kanal, aktivasyon aninda (Faz 2 girisi).

### Visual Sinerjisi — Shadow Echo + Rift (DECISION 3)

| Element | Renk | Alpha | Edge | Rol |
|---------|------|-------|------|-----|
| Shadow Echo phantom (oyuncu) | cyan #00FFCC | %70 | soft glow, no hard outline | oyuncu-dostu, zararsiz |
| Rift Tear (hazard) | cyan #00FFCC | %100 | hard 2px outline #001A33 | tehlike, birincil okuma |
| Rift Bloom crack (hazard) | cyan #00FFCC | %100 | hard 2px outline #001A33 | tehlike, birincil okuma |

Ayni palet ailesi, farklı edge profili = okunakli + tematik bagli. Oyuncu phantom'ini hazarddan net ayirt edebilir.

---

## Oyuncu Counterplay

### Hareket

- Rift Tear: arena merkezi kapali, koseler gorece guvenli
- Rift Bloom: 1s telegraph + dash window (3-kanal cue, bkz. yukarisi)
- Dash lane: tek-yonlu kisitlama Faz 2'de aktif (kuzey-guney veya ters, harita tasarimina gore TBD)
- Dash i-frame suresi: NLM/spec'ten — placeholder (OPEN QUESTION)

### Skill Interaksiyonu

- **Shadow Echo (Karar #60):** phantom layer Rift'lere dokunmadan damage transfer — cyan trail luminance ayri (DECISION 3 kurali korunuyor)
- **F-slot cross-class (Karar #24):** phase transition i-frame surecinde cooldown reset window acik
- **V Burst (DECISION 5):** transition cinematic 1.5s'te windup KABUL, hasar normal — **bonus YOK** (ekstra bonus = Faz 2 ogrenimi atlama riski)

### Parry

- Rift Tear / Rift Bloom: parry YOK (zemin/AOE, melee degil)
- Sovereign melee swing: parry acik — Fracture Strike + Shackle Cast

---

## Hasar Tablosu (Placeholder)

| Element | Faz 1 | Faz 2 | Not |
|---------|-------|-------|-----|
| Chain Whip | 30 | — | Faz 2'de yok (Fracture Strike alti) |
| Penitent Surge | 35 | — | Faz 2'de yok |
| Fracture Strike | — | 22 dmg/hit x3 | Yeni |
| Chain Detonation | — | 40 dmg x3 nokta | Yeni |
| Shackle Cast | 15 + slow | 15 + slow | Ayni |
| Rift Tear temas | YOK | TBD playtest | OPEN QUESTION 4 |
| Rift Bloom DoT | YOK | 25 dmg/s + %30 slow | LOCKED |

Class balance notu: Faz 2 melee %15 resistance vs ranged uniform tartismali. Oneri: uniform damage azaltma (OPEN QUESTION 3).

---

## Faz 3'e Gecis (referans)

- Trigger: %33 HP, 2.0s cinematic (Sovereign havaya kalkar, zincirler erir, govde yarisi rift, hiz +%50 total)
- DECISION 1 cooldown: Faz 2 girisindan 8s sonra Faz 3 tetiklenebilir
- Yeni: Echo Phantom Summon + Fracture Charge + Sovereign's Wrath (hareketli 2m safe zone, center/edge/boss-arc)
- Rift Tear 3m KALIR (Faz 2'den devam)
- Echo Phantom: her 10s Chain Warden Echo cagrir, 50 HP, 12s lifespan, max 3 active, Triple Chain + Chain Pull
- Faz 3 tam spec bu dokumun kapsami disinda — eger yazilirsa: BOSS_PHASE3_SOVEREIGN_AWAKENED_SPEC.md

---

## Butce Kontrolu (Karar #82 + #84)

- Rift Tear + Rift Bloom: Faz 2 BASELINE LOCKED (T3 degil) — Decision 2 MOOTED, ihlal yok
- Echo Phantom Summon: Faz 3 mekanigi — **OPEN QUESTION 1:** Faz 1 production budget bunu kapsatmali mi, yoksa Faz 2-3'e mi erteleyelim?
- Mob spawn: arena ICINDE YOK (DECISION 6) — cognitive load 3 paralel kanal (tear telegraph + Sovereign swing + Shadow Echo) yeterli. Mob boss-oncesi koridorda kullanilir (T2 mob spec ayri track)
- Chain anchor (Litany of Restraint): Faz 2'de kaldirilir — arena already tighter with Rift Tear

---

## Design Decision Ozeti (1-6)

| # | Baslik | Karar | Durum |
|---|--------|-------|-------|
| 1 | Phase Transition Cooldown | Posture %50 + HP %60 backup, +8s re-trigger blocked | KABUL |
| 2 | T3 Escalation Siniri (Karar #84) | MOOTED — Rift Bloom zaten Faz 2 LOCKED baseline'da, ihlal yok | KAPALI |
| 3 | Cyan Visual Sinerjisi | Shadow Echo (%70 alpha soft glow) vs Rift hazard (%100 alpha hard 2px outline #001A33) | KABUL |
| 4 | Color-Blind Telegraph | 3-kanal: outline pulse + ground shake + cyan glow (oncelik sirasi) | KABUL |
| 5 | V Burst Transition Penceresi | Windup OK, hasar normal, ekstra bonus YOK | KABUL |
| 6 | Mob Spawn (Faz 2) | YOK — boss-oncesi koridor track'i | KABUL |

---

## Open Questions (sabah kullaniciya)

1. **Echo Phantom Summon budget:** Faz 1 production budget bunu iceriyor mu yoksa Faz 2-3 delivery'e mi erteleyelim? (Karar #82 T3 disabled Faz 1)
2. **Posture vs HP onceligi:** DECISION 1 onerisi posture FIRST + 8s cooldown — confirm?
3. **Melee/ranged damage resistance:** Faz 2 melee %15 resistance favoring mi yoksa uniform mu? Oneri: uniform.
4. **Rift Tear contact damage:** NLM'de spesifik deger yok — placeholder kalsın, playtest mi belirlesin?
5. **Phase transition i-frame confirm:** 1.5s tam invuln + V Burst windup OK karar kabul edilsin mi?

---

## Cross-References

- Karar #31 REV, #60 (Shadow Echo), #24 (F-slot cross-class), #82, #84
- Faz 1 LOCKED baseline: NLM teyit (Litany of Restraint, Chain Whip, Surge, Shackle)
- Bagli dokler: SHADOW_ECHO_MATRIX, DAMAGE_CALCULATION, SKILL_SYSTEM_v2, MOB_COMPOSITION_RULES
- Faz 3 spec (eger yazilirsa): BOSS_PHASE3_SOVEREIGN_AWAKENED_SPEC.md

---

## Implementation Hint

- **VFX system:** cyan palette atlas icinde iki materyel — soft glow shader (Shadow Echo phantom) + hard outline shader (Rift hazard). Ayni atlas, farkli material instance.
- **Boss FSM:** Phase1Idle -> Phase1Combat -> Phase1Transition (i-frame 1.5s) -> Phase2Combat (hazards aktif, speed +30%) -> Phase2Transition (i-frame) -> Phase3Combat
- **Accessibility ayarlari:** ekran shake amplitude slider 0-2px; outline thickness multiplier (zayif goruslulere); ses efekti crack spawn icin (3. kanal audio backup)
- **Dash lane:** Rift Tear merkezde sabit oldugu icin lane tek-yonlu kisitlamasi harita grid'inde isaretlenmeli, runtime'da flip yapmasin

---

*Bu dokuman rima-doc tarafindan yazildi — 2026-05-13. Sabah kullanici confirm -> STATUS: KARAR ONAYLANDI.*

