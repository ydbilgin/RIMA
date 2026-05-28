# RIMA Mekanik Anti-Generic — Opus Verdict (S93 2026-05-19)

## VERDICT (özet)

RIMA'nın **1 gerçek signature'ı var: 10 sınıf × 10 farklı kaynak ekonomisi** (Rage/Focus/Tension/Heat/Fury/Charge/Hex/Mana+Element/Energy+Combo/Charges). MMORPG-yoğunluğunda resource ritimleri hızlı izometrik roguelite'a — Hades/DC/Brotato yapmadı. **AMA** 17 sistemin tasarımı %100 LOCKED, implementasyonun ortalaması ~%20. Şu an çalışan oyun = Warblade + 7 mob + 8-9 oda. Signature **kanıtlanmadı**.

CB'nin avantajı yeni oyun olması değil — **pitch netliği**: "Paint the floor. Trigger the chain. Erase the room." Tek cümle. RIMA aynı netliği bulamıyor.

## 17 sistem implementation reality

| # | Sistem | Design | Impl | Playtest |
|---|---|---|---|---|
| 1 | 10-class unique resource | 100% | 15% (Warblade Rage only) | **HAYIR** |
| 2 | Combat v4 (3-beat + dash-cancel) | 100% | 20% | HAYIR |
| 3 | Dash (pure mobility, 0.4s i-frame) | 100% | **100%** | **EVET** |
| 4 | Per-class Deflect (Iron/Sakura/etc.) | 100% | 10% (Warblade only) | HAYIR |
| 5 | Hitstop / Shake / Flash scaffold | 100% | 100% scaffold | KISMEN |
| 6 | Cross-Class Skill Drafting | 100% | %10 altyapı | HAYIR |
| 7 | Shadow Echo T1-T4 (50 echo) | 100% | 5% | HAYIR |
| 8 | Family Tag → Rift Proc | 100% | **0%** | HAYIR |
| 9 | V Burst / Rift Break boss duel | 100% | 0% | HAYIR |
| 10 | Burden/Gift Curse Gate | 100% | 0% | HAYIR |
| 11 | Rift Portal opportunity | 100% | 0% | HAYIR |
| 12 | Echo Imprint (1/3 oda) | 100% | 0% | HAYIR |
| 14 | Secondary Class (Act 1 boss sonrası) | 100% | 0% | HAYIR |
| 15 | Map Fragment partial map | 100% | **LIVE** | **EVET** |
| 16 | Room types (Combat/Elite/Boss/...) | 100% | KISMI | KISMEN |
| 17 | HP economy (no Rest, Shop/Boss/Curse recovery) | 100% | 0% | HAYIR |

## Generic vs Unique scorecard

| Sistem | Yargı | Risk |
|---|---|---|
| 10 farklı kaynak ekonomisi | **UNIQUE — RIMA'nın imzası** | EN BÜYÜK RİSK: 9/10 sınıf kanıtlanmazsa imza yok |
| Per-class Deflect | UNIQUE-LITE — Sekiro'yu sınıf kişiliğine dağıtmak orjinal | Orta |
| V Burst / Rift Break boss duel | UNIQUE — Hades bosslar invul fade, RIMA interactive duel | Yüksek (per-class scripting) |
| Cross-Class Drafting | **HADES KLON** — Yunan tanrı → MMO sınıf rename | Düşük (kanıtlı formül) |
| Family Tag Rift Proc | HADES REWORK — Privileged Status sis-sis | Yüksek (44-50h kod) |
| Shadow Echo algorithmic auto-bond | HALF-UNIQUE — algoritm zeki, oyuncu hissi Brotato/RoR2 | Orta-yüksek |
| Burden/Gift, Rift Portal, Echo Imprint | SAF Spire/Hades KLONLARI | Düşük orjinalite |
| 10 sınıf × 12 skill = 120 skill + 80 evrim | **OVERDESIGNED** — Hades 6, DC silahsız, Brotato pool | **EXTREME** — solo dev için 2027+ |

**Score:** 5/14 saf klon + 3/14 rework + 3 gerçek signature (henüz playable değil).

## Cut önerileri (acımasız)

| Cut | Kazanç |
|---|---|
| 80 evrim → 40 (sadece Q+R, A+B path) | 35-45h |
| 50 Shadow Echo → 20 (per-class 2) | 30h+ |
| 12 skill/sınıf → 8 | 200h (10 sınıf × 20h) |
| 9 family tag → 4 (Fracture/Echo/Bleed/Rift meta) | UI clutter çözer |
| Secondary class Faz 5'e defer | Balance cehennemi 90 combo → 10 |
| Steam EA'da 6 sınıf, 4 post-launch | Ship 1 yıl öne |
| 3 Act → 2 Act + Final EA, Act 3 DLC | Ship 6 ay öne |

**Toplam cut kazancı:** ~300-400 saatlik dev work → 5-6 sınıf daha implement edilebilir.

## CB pivot honest take

| Soru | RIMA | CB |
|---|---|---|
| Sub-genre slot net mi? | Hayır ("Hades-like + class depth" amorf) | **EVET** ("Real-Time Generative Action Roguelike") |
| Pitch tek cümlede? | Hayır | **EVET** ("Paint the floor. Trigger the chain. Erase the room.") |
| Mekanik signature kanıtlı mı? | %15 (Warblade only) | %0 (henüz başlamadı) |
| 6 ay yatırım | Geride | Sıfır |
| Solo-dev ship realistic | Faz 4 = 2027+ | 16-week MVP plan var |

**Brutal observation:** CB'nin parlak görünmesinin sebebi yeni oyun olması değil, **tek cümle pitch'i çözmüş olması**. RIMA pitch netliği bulabilirse rekabet edebilir.

## Decision matrix

- **Validation Gate PASS** → RIMA devam, CB ikinci proje 2027
- **Validation Gate FAIL** → CB pivot ciddi düşünülmeli
- **Validation Gate MIXED (2/4)** → 10 sınıfı 4'e indir, scope demo-ready 6 ay

## FINAL RECOMMENDATION

**TEK EYLEM:** **5-7 günlük "Two-Class Combat Stress Test"**

Ronin'i Warblade'in yanına koy. İki farklı kaynak ritmi (Warblade Rage = vurarak doldur, Ronin Tension = hareketsiz beklemek) gerçekten farklı hissedilip hissedilmiyor mu KANITLA.

### 5-7 gün plan

| Gün | İş |
|---|---|
| 1 | Ronin BasicAttackProfile + 4 skill .cs + .asset (Warblade pattern copy via Codex) |
| 2 | Tension resource UI + Sakura Veil deflect entegrasyonu |
| 3 | Hit feel tuning matrix (40/80/50ms hitstop, parry micro-freeze, white flash) — BasicAttackProfile field genişletme |
| 4 | Cross-class T1 echo: Warblade Beat 3 → Ronin Quickdraw echo VFX + audio |
| 5 | Spawn_01 PlayableRoom 7 mob spawn, 2 sınıfla A/B test 5dk her biri |
| 6 | Self-honesty checklist PASS/FAIL/MIXED karar |
| 7 | Buffer |

### PASS kriterleri (4 soru)

1. Warblade + Ronin yan yana → "bu iki sınıf farklı oyunlar oynar gibi hissettiriyor" diyebiliyor musun?
2. Combat hit feel: parry 50ms micro-freeze + white flash hissedildi mi?
3. Cross-class echo "ekstra dopamin" mi yoksa "visual clutter" mı?
4. 5 dakika oyunda "tekrar oynamak isterim" hissi var mı?

### Test sırasında YASAK

- Visual asset üretim (sprite/tile/animation) — **1 HAFTA DONDURULDU**
- Yeni doc üretim
- Yeni sınıf > 1 (Ronin yeter)
- Yeni mekanik tasarımı

### Test sırasında ZORUNLU

- Codex pattern-copy Ronin (~12-15h)
- Hit feel tuning matrix (~6-8h)
- Cross-class T1 echo Ronin variant (~4-5h)
- Playtest A/B 5dk (~2h)
- Honesty checklist + karar (~1h)

**Toplam ~30-40h, 1 hafta.**

## Conflicts with locked rules

NONE. Karar #80 silüet, #144 weaponless body, #143 6-layer pipeline etkilenmez. Bu gameplay sprint; art geçici donduruluyor.
