# USER TASK — IDLE + WALK + DEATH ANIMATIONS
> Güncelleme: 2026-04-14 | Sen yaparsın. Bitince Claude'a söyle.

---

## FRAME KARARLARI

| Animasyon | Segment | Dağılım | Toplam | Loop |
|-----------|---------|---------|--------|------|
| Idle | 1 | 8f | **8f** | ✓ |
| Walk | 2 | 5f + 5f | **10f** | ✓ |
| Death | 2 | 4f + 8f | **12f** | ✗ |

**Neden bu sayılar:**
- Idle: Same-frame loop — AI burada en iyi. Artırmak anlamsız.
- Walk: Mid-stride pivot noktası gerekli, 10f roguelite ritmi için ideal.
- Death: Stagger kısa/keskin (4f), düşme ağır/uzun (8f).

---

## BASE SPRITE YOLLARI

```
Warblade:     F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Warblade\base\warblade_{yön}.png
Elementalist: F:\...\Elementalist\base\elementalist_{yön}.png
Ranger:       F:\...\Ranger\base\ranger_{yön}.png
Shadowblade:  F:\...\Shadowblade\base\shadowblade_{yön}.png
```

**Yönler:** S / N / W (East = West mirror, Unity'de flipX)

---

## KAYIT YERLERİ

```
Idle:  Assets\Sprites\Characters\{Char}\animations\fight-stance-idle\{yön}\frame_000.png
Walk:  Assets\Sprites\Characters\{Char}\animations\walk\{yön}\frame_000.png
Death: Assets\Sprites\Characters\{Char}\animations\death\{yön}\frame_000.png

Keyframeler (geçici):
  walk_frames\mid_stride_{yön}.png
  death_frames\stagger_{yön}.png
  death_frames\flat_dead_{yön}.png
```

---

## YÖN CÜMLESI (her prompt başına ekle)

| Yön | Eklenecek cümle |
|-----|----------------|
| S | `facing downward toward camera` |
| N | `facing upward away from camera` |
| W | `facing left` |

---

---

# IDLE ANIMATIONS — 1 SEGMENT, 8 FRAME

**Araç:** Aseprite → Animate → Pro Tools → Interpolate (new)

**Ayarlar:**
- Set start image: `base/{char}_{yön}.png`
- Set end image: **AYNI** `base/{char}_{yön}.png`
- Number of frames: **8**

*Açıklama: Aynı frame'i iki uç olarak vermek AI'a "bu loop olmalı" sinyali verir. Nefes, kumaş, enerji hareketi AI'ın güçlü alanı — müdahale etme.*

---

## WARBLADE — Fight Stance Idle

**SHORT** *(Enhance with AI kullan)*
```
Heavily armored plate warrior standing still in combat stance. Chest rising and falling with breath. Shoulder pauldrons shift slightly. Cloak edge flutters.
```

**LONG** *(direkt kullan)*
```
Heavily armored plate-clad warrior standing in combat fight stance, 128x128 pixel art, low top-down view. Breathing idle loop: chest rises and falls slowly. Shoulder pauldrons shift slightly with each breath. Heavy cloak edge moves minimally. Greatsword held at side, grip firm. Combat-ready stillness. Seamless loop, no displacement.
```

---

## ELEMENTALIST — Idle

**SHORT**
```
Robed battle mage standing still. Magical energy wisping around hands. Robe fabric shifting gently. Breathing.
```

**LONG**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view. Breathing idle loop: chest rises and falls. Magical energy particles slowly orbit both hands with soft glow. Robe hem sways gently. Hood fabric shifts slightly. Magical aura pulses softly. Combat-ready stillness. Seamless loop, no displacement.
```

---

## RANGER — Idle

**SHORT**
```
Archer standing ready. Bow held loosely. Slight alert scanning. Breathing.
```

**LONG**
```
Archer ranger with heroic proportions holding a longbow, 128x128 pixel art, low top-down view. Breathing idle loop: chest expands and contracts. Very slight weight shift left-right. Bow arm held loosely at side. Arrow quiver at back, still. Hair or hood fabric moves minimally. Alert readiness. Seamless loop, no displacement.
```

---

## SHADOWBLADE — Idle

**SHORT**
```
Dual-blade assassin standing still. Dark cloak fluttering slightly. Both blades ready. Shadow energy wisping. Breathing.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view. Breathing idle loop: torso rises and falls. Dark cloak edges flutter very slightly. Both blades held ready at sides. Faint shadow energy wisps around figure edges. Predatory stillness. Seamless loop, no displacement.
```

---

---

# WALK ANIMATIONS — 2 SEGMENT, 10 FRAME TOPLAM

**Neden 2 segment:**
- Same-frame tek pasaj ile AI yürüyüş yönünü bilemez → "hovering" etkisi
- Mid-stride keyframe pivot noktası verir → loop temiz kapanır

## ADIM 1 — Mid-Stride Keyframe (Edit Image PRO)

**Edit Image PRO ayarları:**
- Image to edit: `base/{char}_{yön}.png`
- Output size: 128x128
- Remove background: ✓

*Her karakter için mid-stride promptlar aşağıda.*

**Kaydet:** `walk_frames\mid_stride_{yön}.png`

## ADIM 2 — Segment 1: Base → Mid-Stride (Interpolate, 5 frame)

- Set start image: `base/{char}_{yön}.png`
- Set end image: `mid_stride_{yön}.png`
- Number of frames: **5**

## ADIM 3 — Segment 2: Mid-Stride → Base (Interpolate, 5 frame)

- Set start image: `mid_stride_{yön}.png`
- Set end image: `base/{char}_{yön}.png`
- Number of frames: **5**

## ADIM 4 — Aseprite Birleştirme

```
1. Yeni dosya: 128×128, 10 frame
2. Seg1 çıktısı: frame 1-5 (Base→MidStride, MidStride dahil)
3. Seg2 çıktısı: frame 6-10 (MidStride→Base arası, başlangıç Base hariç)
4. Frame delay: 80ms tümü
5. Export Sprite Sheet → Horizontal Strip → walk_{yön}.png
```

---

## WARBLADE — Walk Mid-Stride Keyframe

**SHORT**
```
Heavily armored warrior mid-stride — left leg fully forward, right leg extended back, body leaning forward, right arm counter-swung forward.
```

**LONG**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view, {facing direction}.
Mid-stride keyframe: left leg fully stepped forward, right leg extended back. Torso leaning forward into stride. Right arm swung forward counter to left leg, left arm pulled back. Full body weight on forward foot. Maximum stride extension — clear, readable mid-step pose. Armor plates shifted from momentum.
```

**Segment 1 Interpolate prompt:**
```
Armored warrior shifts weight forward, left leg steps out, right arm counter-swings. Body leans into stride from neutral stance.
```

**Segment 2 Interpolate prompt:**
```
Armored warrior completes step — weight transitions over forward foot, body returns upright, legs re-center to neutral ready stance. Smooth step completion.
```

---

## ELEMENTALIST — Walk Mid-Stride Keyframe

**SHORT**
```
Robed battle mage mid-stride — right leg forward, left leg back, robes billowing forward from movement, right arm trailing.
```

**LONG**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view, {facing direction}.
Mid-stride keyframe: right leg stepped forward, left leg extended back. Robes billowing forward from walking momentum. Right arm trailing slightly back, left arm slightly forward for balance. Body weight on front foot, forward lean from stride.
```

**Segment 1 Interpolate prompt:**
```
Robed mage steps forward, robes begin to flow with movement, arms shift for balance. Graceful stride beginning from neutral.
```

**Segment 2 Interpolate prompt:**
```
Robed mage completes step, robes settle, legs re-center to neutral. Smooth walking return.
```

---

## RANGER — Walk Mid-Stride Keyframe

**SHORT**
```
Archer mid-stride — right leg forward, left extended back, bow in forward hand, body in light athletic lean, quiver bouncing.
```

**LONG**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down view, {facing direction}.
Mid-stride keyframe: right leg forward, left leg back, body in slight athletic forward lean. Bow held loosely in left hand at side. Arrow quiver at hip, slightly displaced from step bounce. Light footfall, agile movement.
```

**Segment 1 Interpolate prompt:**
```
Archer steps forward lightly, bow swings slightly, quiver bounces at hip. Athletic walk stride from neutral.
```

**Segment 2 Interpolate prompt:**
```
Archer completes step, body settles upright, returns to neutral ready stance. Light agile step completion.
```

---

## SHADOWBLADE — Walk Mid-Stride Keyframe

**SHORT**
```
Dual-blade assassin mid-stride — right leg forward, left back, body in low predatory lean, both blades swinging at sides.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view, {facing direction}.
Mid-stride keyframe: right leg stepped forward, left leg back, body in low slight crouch-lean forward. Right blade swung slightly forward, left blade back. Dark cloak trailing behind from movement. Silent predatory forward lean.
```

**Segment 1 Interpolate prompt:**
```
Assassin steps forward in low predatory stride, both blades swing with movement, cloak trails. Silent approach from neutral.
```

**Segment 2 Interpolate prompt:**
```
Assassin completes predatory step, body re-centers low, returns to neutral dual-ready stance. Silent light-footed completion.
```

---

---

# DEATH ANIMATIONS — 2 SEGMENT, 12 FRAME TOPLAM

**Neden 2 segment:**
- Tek pasajda AI "erime" yapıyor — düşme yönü ve diz çökme kaybolur
- Stagger keyframe düşme vektörünü kilitler

## ADIM 1 — Stagger Keyframe (Edit Image PRO)

**Edit Image PRO ayarları:**
- Image to edit: `base/{char}_{yön}.png`
- Output size: 128x128
- Remove background: ✓

**Kaydet:** `death_frames\stagger_{yön}.png`

## ADIM 2 — Flat Dead Keyframe (Edit Image PRO)

**Ayarlar aynı.**
**Kaydet:** `death_frames\flat_dead_{yön}.png`

## ADIM 3 — Segment 1: Base → Stagger (Interpolate, 4 frame)

- Start: `base/{char}_{yön}.png`
- End: `stagger_{yön}.png`
- Number of frames: **4**

**Açıklama prompt (segment 1):**
```
{Character} staggers from a fatal blow. Weight buckles, knees begin to give way. Hit reaction before collapse.
```

## ADIM 4 — Segment 2: Stagger → Flat Dead (Interpolate, 8 frame)

- Start: `stagger_{yön}.png`
- End: `flat_dead_{yön}.png`
- Number of frames: **8**

**Açıklama prompt (segment 2):**
```
{Character} falls forward from stagger — knees fully buckle, body collapses and crumples flat to the ground. Heavy dead weight impact. No loop.
```

## ADIM 5 — Aseprite Birleştirme

```
1. Yeni dosya: 128×128, 12 frame
2. Seg1: frame 1-4 (Base→Stagger)
3. Seg2: frame 5-12 (Stagger→FlatDead)
4. Frame delay: 70ms tümü
5. Export → death_{yön}.png
```

---

## WARBLADE — Death Keyframes

**Stagger keyframe:**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view.
Stagger from fatal hit: warrior lurches — knees bent and buckling, upper body pitching forward off-balance. One hand releasing the greatsword grip slightly, arm going limp. Armor plates jolting from the impact. Clearly about to fall, not yet on the ground.
```

**Flat Dead keyframe:**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view.
Death pose: warrior collapsed completely face-down on the ground. Both arms spread to sides, legs extended behind. Heavy armor pressing flat against ground. Greatsword fallen beside body. Motionless. Transparent background.
```

---

## ELEMENTALIST — Death Keyframes

**Stagger keyframe:**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view.
Stagger from fatal hit: mage staggers backward, upper body thrown back off-balance. Arms going limp outward, robes billowing from sudden movement. Knees buckling, weight shifting backward. About to collapse, not yet on ground.
```

**Flat Dead keyframe:**
```
Robed battle mage, 128x128 pixel art, low top-down view.
Death pose: mage collapsed completely face-down. Robes spread and draped around fallen body. Arms limp at sides, legs extended. Staff fallen to one side. Magical energy dissipated. Motionless. Transparent background.
```

---

## RANGER — Death Keyframes

**Stagger keyframe:**
```
Archer ranger holding a longbow, 128x128 pixel art, low top-down view.
Stagger from fatal hit: ranger lurches forward, bow dropping from loosening grip. Knees bending and buckling. Body pitching forward off-balance. About to collapse, not yet on ground.
```

**Flat Dead keyframe:**
```
Archer ranger, 128x128 pixel art, low top-down view.
Death pose: ranger collapsed completely face-down. Longbow fallen to side. Arrows scattered from quiver. Arms limp, legs extended. Body motionless. Transparent background.
```

---

## SHADOWBLADE — Death Keyframes

**Stagger keyframe:**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view.
Stagger from fatal hit: assassin lurches — knees buckling, blades dropping from loosening grip. Dark cloak billowing out from sudden impact. Upper body thrown off-balance. Shadow energy beginning to dissipate. About to collapse, not yet on ground.
```

**Flat Dead keyframe:**
```
Dual-wielding dark assassin, 128x128 pixel art, low top-down view.
Death pose: assassin collapsed completely face-down. Both blades fallen at sides. Dark cloak spread around fallen body. Arms limp, legs extended. Shadow energy fully dissipated. Motionless. Transparent background.
```

---

---

# QC

```
IDLE PASS:
  ✓ Loop seamless — başlangıç ve bitiş aynı görünüyor
  ✓ Küçük hareket var (nefes, kumaş, enerji)
  ✓ Karakter sabit duruyor, yer değiştirmiyor

WALK PASS:
  ✓ Mid-stride keyframe okunabilir — adım hareketi net
  ✓ Loop seamless (segment 2 bitişi base sprite ile örtüşüyor)
  ✓ Karakter kimliği korunmuş

DEATH PASS:
  ✓ Stagger inandırıcı — denge kaybı okunuyor
  ✓ Segment geçişi (stagger→fall) akışlı
  ✓ Son frame: tamamen yerde, motionless
  ✓ Frame sayısı: 12, no-loop

FAIL çözümleri:
  Walk loop kırık     → Mid-stride keyframe base sprite'a görsel olarak daha yakın üret
  Stagger yok gibi    → "knees buckling, body pitching off-balance" güçlendir
  Death stiff         → Seg2 prompt'a "crumples, dead weight" ekle
  Karakter şekil değiş → Stagger/dead frame'i base sprite'a daha benzer üret
```

---

## BİTİNCE

"Warblade idle S hazır", "Shadowblade walk tüm yönler hazır" vs. de.

Claude şunları yapar:
1. Sprite import (PPU=64, Point filter, No compression)
2. .anim clip build (idle/walk: loop=true, death: loop=false)
3. AnimatorController'a state + BlendTree ekleme
