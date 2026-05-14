# Warblade Animation Spec — FINAL LOCKED

**Son güncelleme:** 2026-05-13 (S67)
**Karar otoritesi:** Opus (rima-design) game-feel judgment + Codex teknik review + orchestrator sentez
**Geçerli kararlar:** #71 (silah hep elde), #80 (Class Silhouette Bible), #100 (palette), #107 (animation prompts skill alignment), #108 (PixelLab UI capability), #114 (8-direction lock), #110 (Combat FAZ 1.0)

---

## TL;DR

Warblade = ağır 2-el greatsword karakteri (chibi 64 px). **124×124 canvas → 16 frame'e kadar luxury budget.** Hades-tier richness (14-18 frame heavy) MÜMKÜN. "Ağır, biriktirilmiş kontrol" + AKICI animasyon birlikte. Post-effect kompansasyon hâlâ value-add ama 8-frame tavanı yoktu — ESKİ 252×252 spec yanlıştı, chibi karakter 124×124.

## ÖNEMLİ DÜZELTME — 2026-05-13 (sonradan)

Önceki spec **252×252 zorunlu** diyordu (NLM eski mature batch dönemi). **Bu yanlış.** Karar #100 RESTORE ile chibi 64px karakter + Custom V3 canvas 120-124×124 (Warblade MCP karakter ID `6bf7afdb-fc51-4bb4-9fc0-6c1039b1c0f3` = 120×120 doğrulandı). 124×124×16 = 246,016 ≤ 524,288 ✓ — **frame budget rahat 16'ya kadar gider.** Silah karakterle aynı sprite'ta (Karar #71 LOCKED, separate layer YOK).

---

## 1. Frame Count Table — REVIZE LOCKED (124×124 budget)

| Anim | Frame | Yön | Tool | Notlar |
|---|---|---|---|---|
| **Idle** | **12** | 3 gen (S/E/N) + W mirror | Animate with Text NEW | Heavy breathing rhythm, blade-grounded sway, occasional shoulder roll. First+last interpolate (Karar #42) |
| **Run** | **8** | 8 (5 gen + 3 mirror, Karar #114) | PixelLab Animate (built-in) | Weighted gait, flip YASAK (Karar #46). 8 frame smooth gait |
| **Dash** | **8** | 4 (S/E/N/W) | Animate Text NEW | Heavy follow-through, anticipation + thrust + recovery |
| **Hurt** | **6** | 4 (S/E/N/W) | Animate Text NEW | Reaction + minor recovery. Çift sayı zorunlu (4/6/8/10/12/14/16) |
| **Death** | **12** | 4 (S/E/N/W) | Animate Text NEW | Heavy collapse dramatic: stagger → knee → fall → final pose |
| **Attack_LMB Beat 1** (low sweep) | **8** | 8 | Animate Text + Interpolate | Wind-up 2 + hit (frame 4) + follow 3 |
| **Attack_LMB Beat 2** (overhead cut) | **12** | 8 | Animate Text + Interpolate | Wind-up 4 + PEAK + hit (frame 6) + follow 5 |
| **Attack_LMB Beat 3** (shoulder ram finisher) | **14** | 8 | Animate Text + Interpolate | Knockback finisher full impact. Wind-up 5 + PEAK + hit (frame 7) + heavy recovery 6 |
| **Attack_RMB Heavy** | **14** (PEAK-shared) | 8 | Animate Text + Interpolate | Hades-tier heavy. START→PEAK 7 + PEAK→END 7 = 14 unique frame (PEAK paylaşılır) |

### Pixel Budget Hesabı (124×124 canvas, REVİZE)

```
Formül: width × height × frame_count ≤ 524,288
124 × 124 × 16 = 246,016 ✓ (rahat) — TEORİK MAX
124 × 124 × 14 = 215,264 ✓ — RMB Heavy + LMB Beat 3
124 × 124 × 12 = 184,512 ✓ — Idle, Death, LMB Beat 2
124 × 124 ×  8 = 122,944 ✓ — Run, Dash, LMB Beat 1
124 × 124 ×  6 =  92,256 ✓ — Hurt
```

**Tool çift sayı zorunlu: 4/6/8/10/12/14/16 only.** Tüm anim'lerde uygulanır.

**ESKİ YANLIŞ HESAP (252×252 spec):** 252×252×8 = 508,032 tavan vardı. Bu chibi'ye uygulanmaz — Karar #100 chibi 64px restored, canvas 124×124.

---

## 2. Direction Policy — Çelişki Çözüldü

| Anim Türü | Yön | Kaynak |
|---|---|---|
| Locomotion (Run/Dash) | **8** | Karar #114 LOCKED (5 gen + 3 mirror) |
| Idle | **3 gen + mirror** | Karar #46 ruh + budget tasarrufu |
| Hurt/Death | **4** (S/E/N/W) | Karar #48 LOCKED |
| Attack (LMB/RMB) | **8** | Locomotion mantığı (gameplay facing) |

**SUPERSEDED:** NLM eski 3-yön (S/E/N) Hurt/Death spec — Karar #48 (4 yön) lehine **geçersiz** ilan edildi.

W yönü mirror ile bedava — atılmamalı.

---

## 3. Canvas Standardı — REVİZE (124×124 chibi)

**Aktif: 124×124** (Karar #100 RESTORE chibi 64px, Custom V3 canvas)

| Canvas | Max Frame (budget) | Kullanım |
|---|---|---|
| **120×120 / 124×124** ← aktif | **16** | Chibi karakter, silah dahil aynı sprite |
| 64×64 (Unity import) | 16+ | PPU=64, world unit (Karar #100) |
| 252×252 | 8 | ESKİ mature batch (REVOKED Karar #100 ile) |

**Karar #100 LOCKED:** chibi 64x64 + ~35° kamera (Hades match). 252 mature batch terk edildi.

**PixelLab üretim:** Custom V3 ile 124×124 canvas, silah karakterle aynı sprite (Karar #71 silah hep elde, sheath/draw YOK).

**Unity import:** 124×124 üretim → direkt Sprite Editor multiple sprite (crop gerekmez) → PPU=64.

---

## 4. Post-Effect Kompansasyonu — VALUE-ADD (zorunlu değil ama önerilir)

ESKİ spec'te 8 frame tavanı vardı, post-effect "zorunlu" idi. **YENI:** 14 frame heavy attack ile Hades richness'a ulaştık, ama post-effect hâlâ %30 heavy feel ekler. Optional ama önerilir:

| Effect | Hafif vuruş (LMB Beat 1) | Heavy vuruş (LMB Beat 3, RMB) |
|---|---|---|
| **Hit-stop** (frame freeze on impact) | 1-2 frame | **2-4 frame** (REF_NUGGETS §1) |
| **Screen shake** | 4-6 px, 0.10s | **8-12 px, 0.15-0.2s** |
| **Camera punch** (vuruş yönü) | 2-3 px | **4-6 px** |
| **Hit flash** (1 frame beyaz overlay) | ✓ | ✓ |
| **Impact particle** (REF_NUGGETS §1 dust fan-out) | 6 px 0.10s | **10-12 px 0.15-0.2s** |
| **Hit SFX** | Mid-thump | **Heavy bass thunk + metal scrape** |
| **Damage number** (TMPro world-space) | beyaz | **sarı (critical: kırmızı)** 0.6-0.8s fade |
| **White health bar lag** (HK tarz) | 0.5s | 0.5s |

**Çıkarım (revize):** 14 frame heavy attack ile animation tek başına Hades-tier okunur. Post-effect eklendiğinde "premium" tier'e çıkar — opsiyonel ama önerilir. Combat FAZ 1.0 zaten hit-stop progress-based, integrate edilebilir.

---

## 5. Tool Workflow

**5a. PixelLab Web App (MCP YASAK — Karar 2026-05-02):**
- `Animate with Text NEW` — PEAK frame üretimi
- `Interpolate NEW` — segment birleştirme
- 4-16 çift sayı frame
- Custom V3 4-16 frame kapasitesi (Karar #108)

**5b. Karar #47 üretim metodu:**
- Run/Idle/Hurt/Death = `Animate with Text NEW` direkt (single-phase)
- Attack/Skill/Dash = `Animate with Text NEW` PEAK + `Interpolate NEW` segments (3-segment workflow)

**5c. PixelLab'da Warblade karakter ID:** `6bf7afdb-fc51-4bb4-9fc0-6c1039b1c0f3` (chibi 120×120, MCP list'ten — Faz 1 primary)

**5d. Aseprite post-processing:**
- 124×124 üretim → direkt kullan (crop gerekmez)
- Aseprite eraser pass (sprite clean) → `*_clean.png` (opsiyonel)
- Sprite slice → multiple sprite (Unity Sprite Editor)
- Unity PPU=64, Sprite Editor pivot bottom-center

---

## 6. Per-Anim Prompt Template Hatırlatma

Mevcut Warblade prompt'ları S65 sonu Opus final judgment'a göre LOCKED:
- **Warblade attack_basic** — Iron Combo Beat 1+2+3 chain (S65 ACCEPT, mevcut prompt korunur)
- **Warblade attack_heavy** — RMB heavy swing (S65 ACCEPT, mevcut prompt korunur)
- **Warblade run** — 8-direction weighted gait (Karar #46)
- **Warblade idle** — Ambient Idle System (Karar #109) ile uyumlu, low-guard breathing + occasional shoulder roll

Mevcut prompt dosyası: `STAGING/pixellab_animation_prompts_10char.md` (Karar #107).

**S65 sonu FIX listesi Warblade'i içermiyor — attack prompt'ları yeniden yazılmaz, ACCEPTED.**

---

## 7. Üretim Sırası (öneri — REVİZE 124×124 budget)

```
1. Idle (12f × 3 gen + mirror)          ─ test feel
2. Run (8f × 8 gen, flip yok)            ─ locomotion temel
3. Hurt (6f × 4 gen) + Death (12f × 4)   ─ feedback animations
4. Dash (8f × 4 gen)                     ─ mobility
5. Attack_LMB Beat 1 (8f × 8 yön)        ─ chain başlangıç
6. Attack_LMB Beat 2 (12f × 8 yön)       ─ chain ortası
7. Attack_LMB Beat 3 (14f × 8 yön)       ─ chain finisher
8. Attack_RMB Heavy (14f × 8 yön)        ─ heavy slot
```

**Toplam Warblade gen call (yön bazında):**
- Idle: 3 gen × 12f = 36 frame
- Run: 5 gen × 8f = 40 frame (3 mirror bedava)
- Dash: 4 gen × 8f = 32 frame
- Hurt: 4 gen × 6f = 24 frame
- Death: 4 gen × 12f = 48 frame
- LMB Beat 1: 5 gen × 8f = 40 frame (3 mirror bedava)
- LMB Beat 2: 5 gen × 12f = 60 frame (3 mirror bedava)
- LMB Beat 3: 5 gen × 14f = 70 frame (3 mirror bedava)
- RMB: 5 gen × 14f = 70 frame (3 mirror bedava)
- **TOPLAM: 40 gen call, ~420 frame.** Aynı gen call sayısı (Custom V3 frame sayısı 1 gen'de hallediyor), credit ~aynı.

---

## 8. QC Checklist (her animasyon sonrası)

1. **Top-down high angle ~35°** (Hades match, Karar #100 KEEP)
2. **Silah hep elde** (Karar #71) — Idle/Run/Hurt/Death dahil
3. **Frame count budget** (252×252×N ≤ 524,288)
4. **Yön tutarlılığı** — silah tarafı tüm yönlerde aynı el (mirror eden W için manuel düzeltme — Karar #114 notu)
5. **Pose distinctness** — özellikle LMB Beat 1/2/3 zincir okunaklığı (Beat 3 ile Beat 2 karışmasın — Opus risk #1)
6. **PEAK frame readability** — RMB heavy için PEAK silhouette + sword arc temiz (Codex risk)
7. **Hit-stop friendly timing** — hit frame'leri PEAK pozisyonu (Beat 1 frame 3, Beat 2 frame 4, Beat 3 frame 4, RMB frame 4-5)
8. **No outline drift** — Karar #100 outline policy korunur

---

## 9. Risk Listesi (Opus + Codex toplamı)

1. **Beat 3 shoulder ram 8 frame'de "knockback finisher" wind-up okunaklılığı düşük** — pose distinctness prompt'ta agresif vurgu (Opus)
2. **RMB Heavy 8 unique frame Hades referansının yarısı** — solo "wow" momenti çalışmayabilir, post-effect kompansasyon zorunlu (Opus)
3. **Run 6 frame × 8 yön weighted gait** — stride drift riski yüksek, QC yön tutarlılığı gerekli (Opus)
4. **PEAK-shared 7 unique frame** — silhouette + sword arc çok temiz olmalı, AI drift riski (Codex risk #4)
5. **Interpolation NEW old 64×64 limitli** — 252 workflow ile direkt uymuyor, Animate with Text NEW kullanılır (Codex risk)
6. **3 ayrı LMB anim** — Unity'de state machine ile chain bağlanır, tek 14-frame imkansız (Codex doğruladı)

---

## 10. Sistem Üzerine Etki

- **Combat (Karar #110):** BasicAttackProfile.cancelWindowFraction (progress-based) + hit-stop integration
- **Camera (Faz 1.5+):** CameraShake + camera punch component zorunlu
- **VFX (Faz 1.5+):** Impact particle pool, hit flash overlay shader
- **Audio (Karar #103 local pipeline):** Heavy attack SFX set (MusicGen Large + AudioGen RTX 5080 local)
- **Animator state machine:** 8-yön Run + 4-yön Hurt/Death blend, LMB 3-hit chain trigger

---

## 11. Üretim Sonrası Adımlar

1. PixelLab web UI'da Warblade kara `6bf7afdb-fc51-4bb4-9fc0-6c1039b1c0f3` referans olarak
2. Frame Count Table sırasıyla üret (Madde 7)
3. Aseprite eraser pass + 128×128 crop
4. Unity Sprite Editor multiple sprite, PPU=64
5. Animator Controller per-anim clip oluştur
6. State machine 3-hit chain (LMB Beat 1→2→3 trigger)
7. Combat playtest: BasicAttackProfile cancelWindowFraction tune
8. Post-effect entegrasyon (Madde 4 listesi)
9. QC checklist (Madde 8)

---

## 12. DECISION META — REVİZE

- **DECISION:** Warblade anim set **124×124 chibi canvas + 16 frame'e kadar luxury budget**. Frame breakdown: Idle 12, Run 8, Dash 8, Hurt 6, Death 12, LMB Beat 1 8, Beat 2 12, Beat 3 14, RMB 14. Hades-tier heavy attack richness MÜMKÜN.
- **RATIONALE:** Karar #100 chibi 64px restored — canvas 252→124, budget bolluğu açıldı. Eski 8-frame tavan yanlıştı (mature batch dönem spec'iydi).
- **TRADE-OFF:** Post-effect kompansasyon "zorunlu"dan "value-add"a düştü. Hades richness animation alone'da ulaşılır.
- **SYSTEMS AFFECTED:** Animation pipeline, Combat (BasicAttackProfile + hit-stop opsiyonel), Camera (punch/shake opsiyonel), VFX layer, Audio.
- **CONFLICTS WITH LOCKED RULES?** NONE — Karar #114 (locomotion 8 yön), Karar #48 (Hurt/Death 4 yön), Karar #100 (chibi 64px + canvas 124×124), Karar #71 (silah hep elde aynı sprite), Karar #46 (Run flip yasak) hepsine saygı.
- **AUTHORITY:** Opus (rima-design) game-feel ruling, Codex teknik validation, **kullanıcı düzeltmesi (252×252 hatası)**.
- **ÖNCEKİ HATA:** İlk spec 252×252 mature batch dönemi spec'iyle yapılmıştı. Karar #100 RESTORE'u (chibi 64px) ile bu yanlış oldu. Kullanıcı düzeltti, spec revize edildi.
