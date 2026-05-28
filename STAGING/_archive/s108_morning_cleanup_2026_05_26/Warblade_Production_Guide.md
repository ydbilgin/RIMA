# Warblade Animasyon — Üretim Rehberi (v3 NARRATIVE FORMAT)

**Tarih:** 2026-05-13 (S68 revize)
**Hedef:** PixelLab Web UI'da Warblade animasyonlarını narrative prompt formatıyla üretmek
**Spec referans:** `TASARIM\karakterler\Warblade_AnimationSpec.md`
**Karar otoritesi:** S68 — pixel/loop instruction'ları kaldırıldı, motion + weapon lock format

---

## PROMPT STILI (S68 LOCKED — MOTION ONLY)

**KRITIK:** PixelLab Characters ekranı karakteri ZATEN GÖRÜYOR. Prompt **görsel kimliği TARIF ETMEZ**, sadece **HAREKET DİNAMİKLERİNİ** anlatır.

**Yapılmayacak:**
- ❌ Görsel kimlik ("warrior in armor", "Warblade")
- ❌ Statik konum ("left hand on hilt", "blade at hip", "two-handed grip")
- ❌ Pixel sayıları ("3-4 pixels chest rise")
- ❌ Frame numarası talimatı ("Frames 1-2: X")
- ❌ Loop talimatı ("frame N equals frame 1", "PERFECT SEAMLESS LOOP")
- ❌ Negatif kelimeler ("NO head tilt", "NO scanning")

**Yapılacak:**
- ✅ Vücut parts'ın **nasıl hareket ettiği**: sway/rock/roll/drift/arc/buckle/lunge/dip
- ✅ Silahın **nasıl hareket ettiği** (konum değil): "blade sways", "greatsword arcs", "blade trails", "tip dips"
- ✅ Locked parts pozitif dil: "head stays level", "feet grounded"
- ✅ Mood/intent ("heavy weight", "patient rhythm", "explosive")
- ✅ Karar #71 motion-form: "blade stays in grip, dragged with body" (release verb YASAK)

**3-5 cümle, her cümle motion verb içerir.** Karar #99 (sol el) için motion direction tarif yeterli — uploaded sprite zaten doğru eli gösterir.

## FRAME COST BRACKET (S68 LOCKED)

PixelLab Custom V3 cost **bracket-based**, doğrusal değil:

| Frame | Cost/direction |
|---|---|
| 4-5f | 1 gen |
| **6-8f** | **2 gen** |
| **10-12f** | **3 gen** |
| 14-16f | 4 gen |

**Default:** Bracket içinde **max frame** seç (free polish). 6f → 8f bedava, 10f → 12f bedava.

---

## GENEL AYARLAR (her animasyon için aynı)

**PixelLab Web App → Custom Animation v3** (Animate with Text NEW)

| Ayar | Değer |
|---|---|
| Tool | Animate with Text NEW (loop) veya Interpolate NEW (attack split) |
| Canvas | **124×124** chibi (silah dahil aynı sprite, Karar #71) |
| Outline mode | Match input |
| Anchor reference | Warblade chibi anchor PNG (`Assets/Resources/Characters/Anchors/warblade_anchor.png`) |
| FPS playback | 10-12 |
| Style image | İlk gen'de Warblade anchor — sonraki gen'lerde önceki anim'in iyi frame'i (style drift azaltır) |

**Yön kuralı (Karar #114 S68 REVISED LOCKED):**
- **8 yön DİREKT generate** — mirror flip YASAK (Karar #99 left-hand silah yanlış ele atıyor)
- Her anim 8 gen call (S, SE, E, NE, N, NW, W, SW)
- Idle/Hurt/Death/Dash dahil hepsi 8 direkt gen

**Tool çift sayı zorunlu:** 4 / 6 / 8 / 10 / 12 / 14 / 16

**Karar #99:** Greatsword **sol elde** birincil grip, sağ el blade'in üzerinde stabilize. HER prompt'ta anatomik ifade içinde.

---

## ÜRETİM SIRASI

```
1. Idle      ────→ test feel, hızlı kazanım
2. Run       ────→ locomotion temel
3. Hurt      ────→ feedback (kısa)
4. Death     ────→ feedback (uzun)
5. Dash      ────→ mobility
6. LMB Beat 1 → chain başlangıç
7. LMB Beat 2 → chain ortası (Karar #120 split adayı)
8. LMB Beat 3 → chain finisher (Karar #120 split adayı)
9. RMB Heavy → ana ağır slot (Karar #120 split adayı)
```

---

# 🎬 ANIMASYON 1 — IDLE

| Ayar | Değer |
|---|---|
| Frame | 8 |
| Yön | 8 direkt gen |
| Tool | Animate with Text NEW |

### Prompt (motion-only)
```
Slow patient breathing rhythm — chest rises gently and falls back, shoulders drift up with the inhale then settle. The left shoulder rolls back in a weary settle then eases forward, repeating the cycle. The blade tip drifts up slightly with each inhale and back down with the exhale. Head stays level, hips and feet grounded throughout. Mood is weary and patient.
```

### 8 gen call
- Gen 1: `facing south`
- Gen 2: `facing south-east`
- Gen 3: `facing east`
- Gen 4: `facing north-east`
- Gen 5: `facing north`
- Gen 6: `facing north-west`
- Gen 7: `facing west`
- Gen 8: `facing south-west`

### QC
- Greatsword her frame'de sol elde, tip sol ayak yakını ✓
- Shoulders/chest drift, alt vücut sabit ✓
- Mood "weary veteran" okunabilir ✓

---

# 🎬 ANIMASYON 2 — RUN

| Ayar | Değer |
|---|---|
| Frame | 8 |
| Yön | 8 direkt gen |
| Tool | Animate with Text NEW |

### Prompt (motion-only)
```
Heavy plodding running cycle — left foot steps forward and plants flat, then the right foot pushes off into its forward step. The greatsword sways across the hips with each stride, blade tip swinging gently in counter to the body movement. Torso rocks subtly with the weight transfer, shoulders rolling forward then back. Head stays level and forward-locked. Heavy grounded weight throughout, no bouncing.
```

### 8 gen call
- Gen 1-8: `running south`, `running south-east`, ..., `running south-west`

### QC
- Stride alternates clearly (sol-sag-sol-sag) ✓
- Greatsword hep elde, sol hip seviyesinde ✓
- Heavy plodding hissi var (light run değil) ✓

---

# 🎬 ANIMASYON 3 — HURT

| Ayar | Değer |
|---|---|
| Frame | **8** (cost bracket — 6→8 bedava upgrade) |
| Yön | 8 direkt gen |
| Tool | Animate with Text NEW |

### Prompt (motion-only)
```
Sharp impact recoil — shoulder snaps backward and head ducks briefly. The blade tip dips down with the recoil but stays in grip throughout. Body braces back into the back foot, hips barely shifting. Recovery rolls shoulders forward and lifts head back up. Recoil is contained, not flailing.
```

### 8 gen call
- Gen 1-8: `facing south taking hit`, `facing south-east taking hit`, ...

### QC
- Silah elden çıkmaz (Karar #71) ✓
- Recoil → hold → recovery 3 beat okunabilir ✓
- Hit-stop friendly (orta frame'de sabit görünür) ✓

---

# 🎬 ANIMASYON 4 — DEATH

| Ayar | Değer |
|---|---|
| Frame | 12 |
| Yön | 8 direkt gen |
| Tool | Animate with Text NEW |

### Prompt (motion-only)
```
Heavy collapse sequence — body staggers a single step then sinks onto the right knee. The blade dips and drives point-first into the ground for support, dragged down with the body and never releasing from grip. Torso slumps forward, head bows toward the pommel as the right hand slides down to brace. Shoulders settle, chest stills. Final pose holds with no recovery.
```

### 8 gen call
- Gen 1-8: `dying facing south`, `dying facing south-east`, ...

### QC
- Karar #71: silah elden çıkmaz, planted veya gripped final pose ✓
- Forbidden YASAK kelimeler kontrol: "drops/falls/releases/slips/let go/separation" YOK ✓
- Stagger → kneel → plant → slump 4 beat ✓

---

# 🎬 ANIMASYON 5 — DASH

| Ayar | Değer |
|---|---|
| Frame | 8 |
| Yön | 8 direkt gen |
| Tool | Animate with Text NEW |

### Prompt (motion-only)
```
Heavy weighted dash — body coils into a brief anticipation crouch then launches forward. The blade pulls close during the coil, then trails slightly behind as the body lunges forward and the back foot pushes off hard. Body coasts with momentum before knees absorb the landing and the blade settles back to its ready angle. Heavy weight throughout, never floaty.
```

### 8 gen call
- Gen 1-8: `dashing south`, `dashing south-east`, ...

### QC
- Anticipation + thrust + recovery 3 beat okunabilir ✓
- Heavy hissi var (light dash gibi teleport DEĞIL) ✓
- Greatsword her frame'de elde, blade angle tutarlı ✓

---

# 🎬 ANIMASYON 6 — LMB BEAT 1 (Low Sweep, Iron Combo 1/3)

| Ayar | Değer |
|---|---|
| Frame | 8 |
| Yön | 8 direkt gen |
| Tool | Animate with Text NEW |
| Hit frame | Orta civarı (impact apex) |

### Prompt (motion-only)
```
Powerful low horizontal sweep — body winds up by pulling the blade back along the right side, shoulder coiling. Then the blade arcs from right to left across the front at knee height, torso rotating with the strike. Weight transfers onto the front foot during the sweep. The blade finishes at chest level on the opposite side, ready for the next chain. Motion is heavy and grounded, never flicked.
```

### 8 gen call
- Gen 1-8: `attacking south`, `attacking south-east`, ...

### QC
- Impact frame net okunabilir (low sweep apex) ✓
- Final frame blade chest level (Beat 2'ye geçiş hint) ✓
- Karar #99 sol el lead hep görünür ✓

---

# 🎬 ANIMASYON 7 — LMB BEAT 2 (Overhead Cut, Iron Combo 2/3) — Karar #120 SPLIT

**Frame 12 + tek apex → 3-stage split kullan (Karar #120):**
- State: AttackApex_Overhead (PixelLab Create State, 20-40 gen)
- Part 1: Custom V3 6f, End Frame = AttackApex
- Part 2: Custom V3 6f, First Frame = AttackApex

### State Prompt (AttackApex_Overhead, statik anchor pose)
```
Blade raised fully overhead, back arched with chest open, weight shifted onto the back foot with the rear heel lifted. Body coiled at the apex of the windup, the moment before the downswing.
```

### Part 1 Prompt (Wind-up: Base → AttackApex, motion-only)
```
Powerful wind-up coil — body rotates and weight shifts onto the back foot. Blade arcs upward from the low ready position past the shoulder and overhead. Shoulders pull back, chest opens. Rear heel lifts slightly. Deliberate anticipation, gathering force without lunging.
```

### Part 2 Prompt (Downswing: AttackApex → Base, motion-only)
```
Heavy downward chop — blade drives from overhead through a diagonal arc back down across the body. Torso rotates with the swing as weight transfers onto the front leg. Shoulders square, chest closes. Head tracks the blade then lifts forward. Recovery settles into the low ready, breath visibly heavy.
```

### Gen call per yön
- State: 8 gen (her yön için apex pose)
- Part 1: 8 gen + Part 2: 8 gen — toplam 24 gen + 8 state = 32 (Karar #120 cost)
- Tek seferde 12f de üretilebilir (8 gen) — split apex polish için

### QC
- Apex frame silhouette temiz, blade tam overhead ✓
- Part 1 son frame ≈ State (frame match) ✓
- Part 2 ilk frame ≈ State (chain seamless) ✓

---

# 🎬 ANIMASYON 8 — LMB BEAT 3 (Shoulder Ram Finisher)

**Frame 12 (cost-optimal, 3-gen bracket cap). Karar #120 split opsiyonel — single 12f tercih edildi (cost discipline).**

### State Prompt (AttackApex_ShoulderRam, statik anchor pose)
```
Shoulder extended fully forward in a ramming pose, body twisted into the line of force. Blade angled forward across the body into the strike line. Front foot planted hard, back leg pushed off. Apex of the ram, moment of impact.
```

### Part 1 (Wind-up + Ram Start, motion-only)
```
Coil phase — weight loads onto back leg, shoulder pulls back, torso winds. Blade stays drawn close at chest level, building force. Rear heel lifts, knees flex. Head locked forward, building the explosive ram.
```

### Part 2 (Ram Impact + Recovery, motion-only)
```
Explosive shoulder ram — body drives forward and blade extends as a battering line. Torso pitches into the strike then settles back as the shoulder pulls home. Recovery is heavy and slow, blade returning to low guard with visible weight.
```

### QC
- Frame middle = ram apex, body fully extended ✓
- Final frame = low guard (Idle base, chain reset) ✓
- Karar #99 sol el lead hep görünür ✓

---

# 🎬 ANIMASYON 9 — RMB HEAVY (Greatsword Smash)

**Frame 12 (cost-optimal, 3-gen bracket cap). Karar #120 split aday — signature attack için single 12f → tatmin edici değilse iter 2 split: State + 8f Part 1 + 8f Part 2 = 5 gen.**

### State Prompt (AttackApex_HeavySmash, statik anchor pose)
```
Blade raised fully overhead in a dramatic windup, blade pointing straight up. Body coiled with back arched, feet planted wide for power, knees slightly bent. The moment before the explosive smash. Head locked forward on the strike target.
```

### Part 1 (Anticipation + Wind-up, motion-only)
```
Dramatic anticipation crouch — body coils as the blade arcs upward past the shoulder and reaches fully overhead. Back arches slightly, feet plant wide, knees absorb the loading weight. Chest opens at the apex. The pose gathers explosive force, breath held before the strike.
```

### Part 2 (Smash + Recovery, motion-only)
```
Explosive downward smash — body and blade drive straight down with full weight committing. Blade impacts ground level. Body leans over the impact point, shoulders compressed. Recovery is heavy and slow — blade pulls back up with visible struggle, body resets to low guard with weighted exhale.
```

### QC
- Apex frame silhouette clean, blade tam overhead ✓
- Smash impact frame net (ground strike) ✓
- Recovery yavaş ağır (heavy realism) ✓

---

## 📋 ÜRETIM SONRASI ADIMLAR (her anim)

1. **Aseprite indir + temizle:** halo/anti-aliasing pass → `{anim}_clean.png`
2. **Unity'ye drop:** `Assets/Sprites/Characters/Warblade/{anim}/{anim}_{dir}.png`
3. **Sprite Editor:** Multiple, slice 124×124 grid
4. **AnimatorController:** her yön ayrı clip, FPS 10-12, loop bayrağı Idle/Run = Yes, diğerleri No
5. **Hit frame event:** AnimationEvent → `BasicAttackProfile.OnHitFrame()` (Karar #64)

---

## ⚠️ ÜRETIM SIRASINDA DİKKAT

1. **Style drift azalt:** İlk gen sonrası, sonraki gen'lerde önceki anim'in iyi frame'ini style reference olarak yükle
2. **Çift frame sayısı zorunlu:** 4/6/8/10/12/14/16 — tek reddedilir
3. **Karar #71 silah hep elde:** Idle/Run/Hurt/Death/Dash dahil greatsword sprite'tan kaybolmaz, "falls/drops" YASAK
4. **Karar #99 silah siluet/görünürlük:** Karar #99 silah elde veya belde net siluet kuralı, **hangi el değil**. Sprite hangi elde gösteriyorsa o doğru — promptta el zikretme (drift riski)
5. **Karar #114 8 direkt gen:** Mirror flip yok, her yön için ayrı gen
6. **Karar #120 split:** Frame ≥12 + tek apex varsa State + Part 1/2 split (Beat 2, Beat 3, RMB Heavy)
7. **Locked parts pozitif dil:** "head/hips/feet remain anchored" — "NO X" YASAK

---

## 💾 KLASÖR ORGANIZASYONU

```
STAGING\WARBLADE_ANIMS\
  01_idle\warblade_idle_S.png ... warblade_idle_SW.png (8 yön)
  02_run\warblade_run_S.png ... (8)
  03_hurt\... (8)
  04_death\... (8)
  05_dash\... (8)
  06_lmb_beat1\... (8)
  07_lmb_beat2\
    states\warblade_apex_overhead_S.png ... (8 state pose)
    part1\... (8)
    part2\... (8)
  08_lmb_beat3\... benzer 3-stage
  09_rmb_heavy\... benzer 3-stage
```

---

## 🎯 TOPLAM ÜRETIM HACMİ (S68 cost-bracket optimize)

| Anim | Frame | Cost/dir | Total (8 dir) |
|---|---|---|---|
| Idle | 8 | 2 gen | 16 |
| Run | 8 | 2 gen | 16 |
| Hurt | 8 | 2 gen | 16 |
| Death | 12 | 3 gen | 24 |
| Dash | 8 | 2 gen | 16 |
| LMB Beat 1 | 8 | 2 gen | 16 |
| LMB Beat 2 | 12 | 3 gen (single) | 24 |
| LMB Beat 3 | 12 | 3 gen (single) | 24 |
| RMB Heavy | 12 | 3 gen (single) | 24 |
| **TOPLAM** | — | — | **176 gen** |

**Optimizasyon notu:** Beat 2/3/RMB önceden 14f split planlıydı (~96 gen daha). 12f single cap + Karar #120 split SADECE signature için, signature RMB Heavy bile single 12f deneyimle değerlendirilir.

**Üretim süresi:** ~10-12 saat (Web UI manual, 8 yön × 9 anim)
**PixelLab credit budget:** 176 gen ≈ **%22 of 800 gen budget** — Warblade tek sınıf için. 10 sınıf = ~1760 gen (mevcut 800 + sonraki 5000 wave ile rahat).

---

## 📚 İLGİLİ KARARLAR

- **Karar #42:** Walk YOK, Run var
- **Karar #71:** Silah hep elde, sheath/draw YOK, "falls" YASAK
- **Karar #99:** Greatsword **sol elde** birincil grip
- **Karar #100:** Chibi 64px + canvas 124×124
- **Karar #108:** PixelLab Custom V3 4-16 frame
- **Karar #114 S68 REVISED:** 8 direkt gen (mirror YASAK — silah yanlış ele)
- **Karar #120:** Frame ≥12 + tek apex → 3-stage split
- **Karar #123 (proposed):** Polish katmanı (rotation/flip jitter, HSV jitter)

---

## 🚀 BAŞLA

İlk gen: **Animasyon 1 — Idle** (en hızlı, 8 gen, hızlı görsel feedback).

PixelLab Web UI'da:
1. Style image = Warblade anchor PNG yükle
2. Custom Animation V3 seç
3. Frame count = 8
4. Prompt = yukarıdaki Idle prompt'unu **+ `facing south`** ekle
5. Generate
6. Sonuçlar gelince Claude'a göster, QC yap
7. South iyi olduktan sonra sırayla diğer 7 yön

İyi denemeler.
