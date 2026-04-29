# USER TASK — CHARACTER ATTACK ANIMATIONS
*Güncelleme: 2026-04-09 | Sen yaparsın. Bitince Claude'a "X attacks hazır" de.*

---

## ANA WORKFLOW — 2 Adım

```
ADIM 1 — pixellab.ai → Edit Image PRO
  → O yönün base sprite'ını yükle (sürükle-bırak)
  → SHORT prompt kullan → "Enhance with AI" → dene
  → Beğenmezsen LONG prompt ile direkt dene
  → En iyi impact frame'i PNG kaydet

ADIM 2 — Aseprite → Interpolate (new)
  → Set start image = base/{char}_{yön}.png
  → Set end image = ADIM 1 impact frame
  → Interpolate prompt gir
  → Number of frames: 10
  → Generate
```

**Her yön için:**
- south → `base\{char}_S.png`
- north → `base\{char}_N.png`
- west → `base\{char}_W.png`

**Edit Image PRO ayarları (sabit):**
- Output size: 128x128
- Remove background: ✓ açık

**Araç uyarıları:**
- `Animate with text (new)` ✅ fallback için
- `Animate with text (pro)` ❌ frame seçimi yok
- `Animate with text (utility)` [Ctrl+Space+A] ❌ sadece 64px

---

## KAYIT YERLERİ

```
Impact frames (geçici):
  Assets\Sprites\Characters\{Char}\impact_frames\{attack}_{yön}.png

Animasyon çıktıları:
  Warblade:    animations\heavy-slash-step0\{yön}\frame_000.png
               animations\heavy-slash-step1\{yön}\frame_000.png
               animations\heavy-slash-step2\{yön}\frame_000.png
  Elementalist: animations\elemental-cast\{yön}\frame_000.png
  Ranger:       animations\arrow-shot\{yön}\frame_000.png
  Shadowblade:  animations\shadow-combo-step0\{yön}\frame_000.png
                animations\shadow-combo-step1\{yön}\frame_000.png
                animations\shadow-combo-step2\{yön}\frame_000.png
```

Base path: `F:\Antigravity Projeler\2d roguelite\RIMA\`

---

---

## WARBLADE — Heavy Slash (3 combo step)

Combo mantığı: Step 0 ascending (yukarı) → Step 1 horizontal → Step 2 overhead slam

**Stil satırı (her Edit Image PRO prompt'una başa ekle):**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
```

---

### STEP 0 — Ascending Slash (Sol alttan sağ üste)

#### ADIM 1 — Edit Image PRO (Impact Keyframe)

**SHORT** *(Enhance with AI kullanabilirsin)*
```
Heavily armored warrior with greatsword. Peak ascending diagonal slash — sword fully extended upper-right, arms at max reach, body weight shifted forward and upward.
```

**LONG** *(direkt kullan)*
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Peak impact frame of ascending diagonal slash. Sword fully extended to upper-right, both arms at maximum reach. Body weight shifted forward and upward into the strike. Armor plates displaced from upward momentum. Blade angled upper-right at full extension.
```

#### ADIM 2 — Interpolate (Animasyon)

```
Armored warrior performs ascending diagonal slash. Sword sweeps from low-left ground level up to upper-right full extension. Torso uncoils, shoulders drive the blade upward. Body weight commits forward and upward into the rising strike.
```

---

### STEP 1 — Horizontal Sweep (Soldan sağa)

#### ADIM 1 — Edit Image PRO (Impact Keyframe)

**SHORT**
```
Heavily armored warrior with greatsword. Peak horizontal sweep — sword fully extended right, arms at max reach, torso fully uncoiled, armor plates shifted sideways.
```

**LONG**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Peak impact frame of horizontal sword sweep. Blade fully extended to the right at chest level, arms at maximum reach. Torso has fully uncoiled left-to-right, armor plates shifted sideways from momentum. Body weight committed into the sweep direction.
```

#### ADIM 2 — Interpolate (Animasyon)

```
Armored warrior performs powerful horizontal sword sweep from left to right. Torso uncoils explosively, shoulders drive the blade across full body width. Armor follows the rotational momentum.
```

---

### STEP 2 — Overhead Slam (Tepeden aşağı)

#### ADIM 1 — Edit Image PRO (Impact Keyframe)

**SHORT**
```
Heavily armored warrior with greatsword. Overhead slam fully landed — sword extended straight down, both hands at max downward reach, upper body hunched forward from impact.
```

**LONG**
```
Heavily armored plate-clad warrior holding a massive two-handed sword, 128x128 pixel art, low top-down view.
Peak impact frame of overhead slam. Sword driven straight down to maximum downward extension, both hands gripping blade. Upper body hunched forward from impact force. Armor jolted forward with the downward momentum. Ground impact pose.
```

#### ADIM 2 — Interpolate (Animasyon)

```
Armored warrior performs explosive overhead slam. Sword drives from fully raised position straight down to ground impact. Entire upper body commits to the blow downward. Armor follows the downward momentum.
```

---

---

## ELEMENTALIST — Elemental Cast

*Tüm elementler (fire/ice/light) aynı animasyonu kullanır. Projektil prefab Unity'de değişir.*

#### ADIM 1 — Edit Image PRO (Impact Keyframe)

**SHORT**
```
Robed battle mage. Peak fireball release — right arm fully extended forward, large crackling fireball at open palm, energy trail behind it.
```

**LONG**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view.
Peak impact frame of fireball cast. Right arm fully extended forward at shoulder height. Large bright crackling orange fireball visible at palm, fingers spread open from release force. Energy trail behind the projectile. Wrist extended at full reach.
```

#### ADIM 2 — Interpolate (Animasyon)

```
Robed mage performs fireball cast. Right arm raises and extends forward, large glowing fireball forms at palm and fires. Arm snaps back slightly from recoil after release. Cast to follow-through.
```

---

---

## RANGER — Arrow Shot

#### ADIM 1 — Edit Image PRO (Keyframe — Full Draw)

*Not: Bu animasyonda "impact" = maksimum gerilim anıdır (release değil). Ok fırlaması Unity'de Arrow.prefab ile halledilir.*

**SHORT**
```
Archer with longbow. Maximum draw — bowstring pulled to cheek, arrow nocked and aimed at target, full bow tension visible.
```

**LONG**
```
Archer ranger with heroic proportions holding a tall wooden longbow, 128x128 pixel art, low top-down view.
Key frame: bowstring at maximum tension. Left arm holding longbow fully extended forward. Right hand pulling bowstring back to cheek. Arrow visibly nocked, tip aimed directly at target. Full tension visible in the bow curve. Focused draw stance.
```

#### ADIM 2 — Interpolate (Animasyon)

```
Archer draws longbow to full tension and releases arrow. Left arm raises bow, right hand pulls string back to cheek with arrow nocked. At full draw, string releases and bow arm follows through. Bow relaxes to ready stance.
```

---

---

## SHADOWBLADE — Shadow Combo (3 step)

*Hızlı karakter — kısa anticipation, explosive impact.*

**Stil satırı (her Edit Image PRO prompt'una başa ekle):**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
```

---

### STEP 0 — Sol Yatay Slash

#### ADIM 1 — Edit Image PRO (Impact Keyframe)

**SHORT**
```
Dual-blade assassin. Peak left-blade horizontal slash — left arm fully extended right, dark shadow trail behind blade.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Peak impact frame of fast horizontal slash. Left blade fully extended to the right, arm at maximum reach. Dark shadow trail visible behind the blade arc. Body weight shifted right from slash momentum.
```

#### ADIM 2 — Interpolate (Animasyon)

```
Assassin performs fast horizontal slash with left blade, sweeping left to right. Dark shadow trail follows the blade arc. Sharp explosive snap motion, arm returns to ready.
```

---

### STEP 1 — Sağ Dikey Slash

#### ADIM 1 — Edit Image PRO (Impact Keyframe)

**SHORT**
```
Dual-blade assassin. Peak right-blade vertical slash — right arm fully extended downward, dark shadow trail below showing vertical arc.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Peak impact frame of fast vertical slash. Right blade fully extended downward, arm at maximum downward reach. Dark shadow trail below the blade showing the vertical arc. Body weight shifted downward from the slash.
```

#### ADIM 2 — Interpolate (Animasyon)

```
Assassin performs fast downward vertical slash with right blade. Blade drops from overhead to full downward extension with dark shadow trail. Sharp fast snap, arm returns to ready after impact.
```

---

### STEP 2 — Çift İleri Thrust

#### ADIM 1 — Edit Image PRO (Impact Keyframe)

**SHORT**
```
Dual-blade assassin. Peak double forward stab — both arms fully extended forward, both blade tips pointed at target, body leaning hard into the lunge.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Peak impact frame of double forward stab. Both arms fully outstretched forward, both blade tips pointed directly at target. Body leaning hard forward into the lunge, full commitment. Maximum forward extension of both blades simultaneously.
```

#### ADIM 2 — Interpolate (Animasyon)

```
Assassin performs explosive double forward stab, both blades lunging simultaneously. Body leans hard into the thrust, both blades reach full forward extension. Sharp retract back to ready after impact.
```

---

---

## QC (her animasyon sonrası)

```
PASS kriterleri:
  ✓ Başlangıç = base sprite ile eşleşiyor
  ✓ Bitiş = impact keyframe ile eşleşiyor
  ✓ Aradaki hareket okunabilir, silah arc görünüyor
  ✓ Karakter kimliği korunmuş (silah, kıyafet tutarlı)

FAIL ve çözüm:
  Ortalar bozuk / karakter şekil değiştiriyor
    → Start ve end frame daha "aynı" görünmeli (benzer oran/stil)
  Impact frame zayıf
    → Edit Image PRO'da farklı varyasyon seç veya LONG prompt dene
  Hareket çok yumuşak
    → Interpolate prompt'a "explosive", "sharp snap" ekle
  Silah kaybolmuş
    → Impact frame'de silah açıkça görünüyor mu kontrol et, yoksa yeni üret
```

---

## RETRY STRATEJİSİ

| Sorun | Adım |
|---|---|
| Edit Image PRO impact frame kötü | SHORT dene → Enhance with AI → beğenmezsen LONG dene |
| Interpolate ortaları bozuk | Impact frame değiştir — daha net poz seç |
| 3 denemede tutmuyor | `Animate with text (new)` fallback'e geç |
| Fallback da tutmuyor | Claude'a ilet |

---

## BİTİNCE

"Elementalist attacks hazır" / "Ranger attacks hazır" / vs. de.
Tümünü beklemene gerek yok — tamamlananı hemen bildir.

Claude şunları yapar:
1. Sprite import (PPU=64, Point filter, No compression)
2. .anim clip build (10 frame, loopTime=false)
3. AnimatorController'a Attack BlendTree ekle
4. PlayerAnimator combo step logic güncelle
