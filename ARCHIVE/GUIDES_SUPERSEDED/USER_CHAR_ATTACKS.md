# USER TASK — CHARACTER ATTACK ANIMATIONS
> Güncelleme: 2026-04-14 | Sen yaparsın. Bitince Claude'a "X attacks hazır" de.
> **Toplam: 12 frame / step | 3 segment (3f anticipation + 6f strike + 3f recovery)**

---

## TEMEL KARAR

**Neden 3 segment:**
- Anticipation olmadan attack mekanik hissettiriyor
- Tek interpolasyonda 12 frame = AI drift, silah kayar
- 3 × kısa segment = AI az tahmin eder → tutarlı kalır

**Segment başına frame limiti:** Max 6f. Üzerinde AI drift başlar.

---

## WORKFLOW — Her Step İçin (3 Aşama)

### AŞAMA 1 — Keyframe Üretimi (Edit Image PRO, zincirleme)

```
ADIM 1: Base sprite → WINDUP keyframe üret
ADIM 2: WINDUP'ı input olarak yükle → PEAK keyframe üret
  (Zincirleme: PEAK aynı görsel dilden beslenir → tutarlılık artar)
```

**Edit Image PRO ayarları (sabit):**
- Output size: 128x128
- Remove background: ✓

---

### AŞAMA 2 — Interpolate (3 Segment)

```
Segment 1: Base → WINDUP          | 3 frame | anticipation
Segment 2: WINDUP → PEAK          | 6 frame | strike
Segment 3: PEAK → Base (original) | 3 frame | recovery
```

**Aseprite — Interpolate (new):**
- Set start / end image → aşağıda belirtilen
- Number of frames: segmente göre (3 veya 6)
- Action description: aşağıda her attack için verilmiş

---

### AŞAMA 3 — Aseprite Birleştirme

```
1. Yeni Aseprite dosyası: 128×128, 12 frame
2. Seg1 çıktısı: frame 1-3 (Base→Windup arası, Windup dahil)
3. Seg2 çıktısı: frame 4-9 (Windup→Peak arası, Peak dahil)
4. Seg3 çıktısı: frame 10-12 (Peak→Base arası)
5. Frame delay: 60ms tümü
6. Export Sprite Sheet → Horizontal Strip
```

---

## KAYIT YERLERİ

```
Keyframeler (geçici, silinebilir):
  Assets\Sprites\Characters\{Char}\attack_frames\{attack}_{step}_windup_{yön}.png
  Assets\Sprites\Characters\{Char}\attack_frames\{attack}_{step}_peak_{yön}.png

Animasyon çıktıları:
  Warblade:     animations\heavy-slash-step0\{yön}\frame_000.png  (step 0-2)
  Elementalist: animations\elemental-cast\{yön}\frame_000.png
  Ranger:       animations\arrow-shot\{yön}\frame_000.png
  Shadowblade:  animations\shadow-combo-step0\{yön}\frame_000.png (step 0-2)
```

**Yönler:** S / N / W (East = West mirror, Unity'de flipX)

---

---

# WARBLADE — Heavy Slash (3 combo step)

**Stil satırı (her Edit Image PRO prompt başına ekle):**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
```

---

## STEP 0 — Ascending Slash (Sol alttan sağ üste)

### WINDUP keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Heavily armored warrior with greatsword. Wind-up for ascending slash — sword pulled back low-left, body coiled right, weight loaded on back foot, ready to release.
```

**LONG**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Anticipation pose before ascending diagonal slash. Sword drawn back to low-left, blade angled downward-left at maximum pull-back. Both hands gripping handle, right shoulder back and coiled. Body weight shifted to right rear foot, torso twisted right preparing to uncoil. Armor plates compressed on right side.
```

---

### PEAK keyframe — Edit Image PRO
*Input: WINDUP frame (zincirleme)*

**SHORT**
```
Heavily armored warrior with greatsword. Peak ascending slash — sword fully extended upper-right, arms at max reach, body weight committed forward and upward.
```

**LONG**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Peak impact of ascending diagonal slash. Sword fully extended to upper-right, both arms at maximum reach overhead-right. Body weight shifted forward and upward into the strike. Torso fully uncoiled left-to-right. Armor plates displaced from upward momentum. Blade angled upper-right at full extension.
```

---

### Interpolate Prompts

**Segment 1 — Base → WINDUP (3 frame)**
```
Armored warrior pulls greatsword back to low-left, body coils right, weight loads to back foot. Slow deliberate wind-up.
```

**Segment 2 — WINDUP → PEAK (6 frame)**
```
Armored warrior explodes from coiled wind-up into ascending diagonal slash. Sword sweeps from low-left up to upper-right full extension. Torso uncoils fast, shoulders drive blade upward. Body weight commits forward and upward into the rising strike.
```

**Segment 3 — PEAK → Base (3 frame)**
```
Armored warrior returns from peak ascending slash extension back to neutral combat stance. Arms lower, weight re-centers, sword settles at side.
```

---

## STEP 1 — Horizontal Sweep (Soldan sağa)

### WINDUP keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Heavily armored warrior with greatsword. Wind-up for horizontal sweep — sword pulled back to far left, torso coiled left, weight shifted left.
```

**LONG**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Anticipation pose before horizontal sweep. Sword drawn back to far left, blade behind left shoulder. Torso twisted left, left shoulder back. Body weight loaded on left foot. Armor plates shifted left from coiled tension. Both hands on grip, right hand leading, ready to uncoil rightward.
```

---

### PEAK keyframe — Edit Image PRO
*Input: WINDUP frame (zincirleme)*

**SHORT**
```
Heavily armored warrior with greatsword. Peak horizontal sweep — sword fully extended right, arms at max reach, torso fully uncoiled, armor shifted sideways.
```

**LONG**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Peak impact of horizontal sword sweep. Blade fully extended to the right at chest level, arms at maximum reach. Torso has fully uncoiled left-to-right, armor plates shifted sideways from momentum. Body weight committed into the sweep direction. Maximum right extension.
```

---

### Interpolate Prompts

**Segment 1 — Base → WINDUP (3 frame)**
```
Armored warrior draws sword back to far left, torso coils, weight shifts. Loading for horizontal sweep.
```

**Segment 2 — WINDUP → PEAK (6 frame)**
```
Armored warrior releases from coiled left position into explosive horizontal sword sweep. Sword sweeps across full body width left to right. Torso uncoils fast, armor follows rotational momentum.
```

**Segment 3 — PEAK → Base (3 frame)**
```
Armored warrior returns from full right extension back to neutral combat stance. Weight re-centers, sword settles.
```

---

## STEP 2 — Overhead Slam (Tepeden aşağı)

### WINDUP keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Heavily armored warrior with greatsword. Wind-up for overhead slam — sword raised fully overhead, both arms extended upward, body stretched tall, weight on balls of feet.
```

**LONG**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Anticipation pose before overhead slam. Sword raised fully overhead, both arms fully extended upward gripping the handle. Body stretched tall and slightly back, weight shifted to balls of feet, ready to crash down. Armor plates lifted from upward reach. Maximum upward extension before the drop.
```

---

### PEAK keyframe — Edit Image PRO
*Input: WINDUP frame (zincirleme)*

**SHORT**
```
Heavily armored warrior with greatsword. Overhead slam landed — sword driven straight down, both hands at max downward reach, upper body hunched forward from impact.
```

**LONG**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Peak impact of overhead slam. Sword driven straight down to maximum downward extension, both hands gripping at impact. Upper body hunched forward and down from impact force. Armor jolted forward with the downward momentum. Ground impact pose. Full commitment downward.
```

---

### Interpolate Prompts

**Segment 1 — Base → WINDUP (3 frame)**
```
Armored warrior raises greatsword fully overhead, body stretches upward, weight loads on feet. Slow overhead raise before the drop.
```

**Segment 2 — WINDUP → PEAK (6 frame)**
```
Armored warrior drops greatsword from full overhead raise in explosive downward slam. Sword drives straight down, entire upper body commits to the blow. Heavy armor momentum amplifies the crash. Fast and devastating drop.
```

**Segment 3 — PEAK → Base (3 frame)**
```
Armored warrior recovers from overhead slam impact, upper body rises, sword lifts back to ready position at side.
```

---

---

# ELEMENTALIST — Elemental Cast

*Tüm elementler (fire/ice/light) aynı animasyonu kullanır. Projektil prefab Unity'de değişir.*

**Stil satırı:**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view.
```

---

### WINDUP keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Robed battle mage. Wind-up for cast — both hands drawn inward to chest, magical energy gathering at palms, body slightly coiled forward.
```

**LONG**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view.
Anticipation pose before elemental cast. Both hands drawn inward toward chest, glowing magical energy visibly gathering and condensing at palms. Body slightly coiled forward, robes pressed inward from energy condensation. Right arm slightly leading, preparing to extend.
```

---

### PEAK keyframe — Edit Image PRO
*Input: WINDUP frame (zincirleme)*

**SHORT**
```
Robed battle mage. Peak cast release — right arm fully extended forward, large crackling fireball at open palm, energy trail behind it.
```

**LONG**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view.
Peak of elemental cast. Right arm fully extended forward at shoulder height. Large bright crackling orange fireball visible at palm, fingers spread open from release force. Energy trail behind the projectile. Left arm extended slightly back for balance. Wrist at full reach, magic released.
```

---

### Interpolate Prompts

**Segment 1 — Base → WINDUP (3 frame)**
```
Robed mage draws hands inward to chest, magical energy condenses at palms. Gathering power before cast.
```

**Segment 2 — WINDUP → PEAK (6 frame)**
```
Robed mage releases condensed magical energy outward, right arm extends fully forward, fireball forms and fires from palm. Explosive magical release from gathered position.
```

**Segment 3 — PEAK → Base (3 frame)**
```
Robed mage arm recoils slightly from cast release, returns to neutral ready stance. Energy dissipates.
```

---

---

# RANGER — Arrow Shot

*Not: Peak frame = maksimum gerilim anı (full draw). Ok fırlaması Unity'de Arrow.prefab ile yapılır.*

**Stil satırı:**
```
Archer ranger with heroic proportions holding a tall wooden longbow, 128x128 pixel art, low top-down view.
```

---

### WINDUP keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Archer with longbow. Beginning draw — bow raised forward, right hand beginning to pull string, arrow nocked, body weight shifting into shooting stance.
```

**LONG**
```
Archer ranger holding a tall wooden longbow, 128x128 pixel art, low top-down view.
Early draw pose. Left arm raised holding bow forward, right hand beginning to pull bowstring back, arrow nocked and aligned. Body weight shifted into stable shooting stance, feet planted. Bow at half-tension — arm beginning to pull but not yet at full draw.
```

---

### PEAK keyframe — Edit Image PRO
*Input: WINDUP frame (zincirleme)*

**SHORT**
```
Archer with longbow. Maximum draw — bowstring pulled fully to cheek, arrow aimed at target, full bow tension, right elbow high and back.
```

**LONG**
```
Archer ranger holding a tall wooden longbow, 128x128 pixel art, low top-down view.
Maximum draw. Left arm holding longbow fully extended forward. Right hand pulling bowstring back to cheek, elbow high and back. Arrow nocked, tip aimed directly at target. Full tension visible in the bow curve — maximum bend. Focused draw stance, body still.
```

---

### Interpolate Prompts

**Segment 1 — Base → WINDUP (3 frame)**
```
Archer raises longbow forward, nocks arrow, begins pulling string. Shifting into draw stance.
```

**Segment 2 — WINDUP → PEAK (6 frame)**
```
Archer draws longbow to full tension. Right hand pulls string back to cheek, elbow rises back. Bow bends to maximum curve. Body holds firm in full draw stance.
```

**Segment 3 — PEAK → Base (3 frame)**
```
Archer releases arrow and bow arm follows through — string released, bow arm lowers, returns to ready stance.
```

---

---

# SHADOWBLADE — Shadow Combo (3 step)

*Hızlı karakter — kısa anticipation, explosive impact.*

**Stil satırı:**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
```

---

## STEP 0 — Sol Yatay Slash

### WINDUP keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Dual-blade assassin. Wind-up for left slash — left blade drawn back behind left shoulder, body twisted right, weight coiled.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Anticipation before left horizontal slash. Left blade pulled back behind left shoulder, left arm coiled tight. Body twisted right to load the swing. Right blade held back and down for balance. Body weight ready to uncoil leftward into the strike.
```

---

### PEAK keyframe — Edit Image PRO
*Input: WINDUP frame (zincirleme)*

**SHORT**
```
Dual-blade assassin. Peak left slash — left arm fully extended right, dark shadow trail behind blade, body fully uncoiled.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Peak of fast horizontal slash with left blade. Left blade fully extended to the right at full arm reach. Dark shadow trail visible behind the blade arc. Body fully uncoiled, weight shifted right from slash momentum. Right blade pulled back at opposite side for balance.
```

---

### Interpolate Prompts

**Segment 1 — Base → WINDUP (3 frame)**
```
Assassin draws left blade back behind shoulder, body coils. Loading the horizontal slash.
```

**Segment 2 — WINDUP → PEAK (6 frame)**
```
Assassin explodes from coiled position into fast horizontal slash with left blade, sweeping left to right. Dark shadow trail follows the arc. Sharp explosive snap.
```

**Segment 3 — PEAK → Base (3 frame)**
```
Assassin retracts left blade from extended position back to dual ready stance.
```

---

## STEP 1 — Sağ Dikey Slash

### WINDUP keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Dual-blade assassin. Wind-up for right downward slash — right blade raised fully overhead, body coiled upward.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Anticipation before right vertical slash. Right blade raised overhead, right arm coiled above shoulder. Body lifted slightly upward, weight on toes, ready to drive blade down. Left blade pulled back at side.
```

---

### PEAK keyframe — Edit Image PRO
*Input: WINDUP frame (zincirleme)*

**SHORT**
```
Dual-blade assassin. Peak right downward slash — right arm fully extended downward, dark shadow trail below, body hunched from impact.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Peak of fast downward vertical slash with right blade. Right blade fully extended downward, arm at maximum downward reach. Dark shadow trail below showing the vertical arc. Body slightly hunched forward from the downward drive. Left blade held steady.
```

---

### Interpolate Prompts

**Segment 1 — Base → WINDUP (3 frame)**
```
Assassin raises right blade overhead, body coils upward. Loading the downward slash.
```

**Segment 2 — WINDUP → PEAK (6 frame)**
```
Assassin drives right blade downward in fast vertical slash from overhead to full downward extension. Dark shadow trail follows the drop. Sharp snap downward.
```

**Segment 3 — PEAK → Base (3 frame)**
```
Assassin retracts right blade from downward extension back to dual ready stance.
```

---

## STEP 2 — Çift İleri Thrust

### WINDUP keyframe — Edit Image PRO
*Input: base sprite*

**SHORT**
```
Dual-blade assassin. Wind-up for double stab — both blades pulled back to sides, body crouched low, weight loaded back, coiled to lunge.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Anticipation before double forward stab. Both blades pulled back to sides, elbows back. Body crouched low, weight loaded on back foot, leaning slightly back. Compressed and coiled, ready to explode forward.
```

---

### PEAK keyframe — Edit Image PRO
*Input: WINDUP frame (zincirleme)*

**SHORT**
```
Dual-blade assassin. Peak double stab — both arms fully extended forward, both blade tips pointed at target, body fully committed into lunge.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Peak of double forward stab. Both arms fully outstretched forward, both blade tips pointed directly at target simultaneously. Body leaning hard forward into the lunge, maximum forward commitment. Shadow energy trailing from both blades.
```

---

### Interpolate Prompts

**Segment 1 — Base → WINDUP (3 frame)**
```
Assassin pulls both blades back to sides, crouches low, weight coils back. Loading double lunge.
```

**Segment 2 — WINDUP → PEAK (6 frame)**
```
Assassin explodes from low crouched coil into double forward stab, both blades lunging simultaneously. Body commits hard forward into full extension. Explosive burst lunge.
```

**Segment 3 — PEAK → Base (3 frame)**
```
Assassin retracts both blades from forward extension, body straightens back to dual ready stance.
```

---

---

## QC (her animasyon sonrası)

```
PASS:
  ✓ Windup okunabilir — silah çekilmiş, beden coiled
  ✓ Strike arc temiz — silah doğru yönde ilerliyor
  ✓ Recovery temiz — base sprite'a geri dönüş smooth
  ✓ Karakter kimliği 3 segment boyunca korunmuş (silah, kıyafet)
  ✓ Shadow trail/enerji efektleri tutarlı (Shadowblade/Elem)

FAIL ve çözüm:
  Windup okunmuyor        → Edit Image PRO'da daha extreme pull-back iste
  Segment geçişi sert     → Keyframe'ler birbirine görsel olarak daha yakın olmalı
  Silah kaybolmuş         → Peak frame'de silah açıkça görünüyor mu kontrol et
  Karakter şekil değişti  → Zincirleme bozulmuş — PEAK'i WINDUP üzerinden yeniden üret
  Strike çok yavaş hissediyorsa → Unity Animator'da Step1'in speed'ini artır (frame sayısı değiştirme)
```

---

## RETRY STRATEJİSİ

| Sorun | Çözüm |
|-------|-------|
| Windup keyframe kötü | SHORT dene → Enhance with AI → beğenmezsen LONG |
| PEAK tutarsız (silah farklı) | WINDUP'ı input olarak yeniden Edit Image PRO (zincirleme kur) |
| Segment 2 ortaları bozuk | WINDUP veya PEAK frame'i değiştir — daha net poz seç |
| 3 denemede tutmuyor | Animate with text (new) fallback ile tüm 12 frame'i tek pasajda dene |

---

## BİTİNCE

"Warblade attacks hazır" / "Elementalist attack hazır" vs. de.

Claude şunları yapar:
1. Sprite import (PPU=64, Point filter, No compression)
2. .anim clip build (12 frame, loopTime=false, her step ayrı clip)
3. AnimatorController'a Attack BlendTree + combo step logic
