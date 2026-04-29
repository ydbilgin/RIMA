# USER TASK — IDLE + WALK + DEATH ANIMATIONS
*Güncelleme: 2026-04-09 | Sen yaparsın. Bitince Claude'a söyle.*

---

## ÖNCE OKU — Ne Yapıyoruz?

**Araç:** Aseprite → Animate > Pro Tools > **Interpolate (new)**

**Mantık:** Başlangıç frame + Bitiş frame ver → AI aralarını doldurur.

```
IDLE / WALK:
  Start = base sprite (o yön)
  End   = AYNI base sprite
  → AI nefes/yürüyüş hareketi yaratır, loop temiz olur

DEATH:
  Start = base sprite (ayakta)
  End   = pixellab.ai Edit Image PRO → ölü poz (yerde yatan)
  → AI düşme hareketini doldurur
```

**Aseprite'ta Interpolate açmak:**
1. `Edit → PixelLab → Open Plugin` (veya `Ctrl+Space+P`)
2. Sol panelde **Animate** → **Pro Tools** → **Interpolate (new)** tıkla

---

## BASE SPRITE YOLLARI

```
Warblade:     F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Warblade\base\warblade_{yön}.png
Elementalist: F:\...\Elementalist\base\elementalist_{yön}.png
Ranger:       F:\...\Ranger\base\ranger_{yön}.png
Shadowblade:  F:\...\Shadowblade\base\shadowblade_{yön}.png
```

Yön dosya adları: `_S` (south), `_N`, `_E`, `_W`, `_NE`, `_NW`, `_SE`, `_SW`

---

## KAYIT YERLERİ

```
Idle:  RIMA\Assets\Sprites\Characters\{Char}\animations\fight-stance-idle\{yön}\frame_000.png
Walk:  RIMA\Assets\Sprites\Characters\{Char}\animations\walk\{yön}\frame_000.png
Death: RIMA\Assets\Sprites\Characters\{Char}\animations\death\{yön}\frame_000.png

Death end frame (geçici, silinebilir sonra):
       RIMA\Assets\Sprites\Characters\{Char}\death_frames\death_{yön}.png
```

---

## ÖNEMLİ NOT — Yön Ekleme

Her prompt'a başlangıçta yön cümlesi ekle:

| Yön | Eklenecek cümle |
|-----|----------------|
| S (south) | `facing downward toward camera` |
| N (north) | `facing upward away from camera` |
| E (east)  | `facing right` |
| W (west)  | `facing left` |
| SE | `facing lower-right diagonal` |
| SW | `facing lower-left diagonal` |
| NE | `facing upper-right diagonal` |
| NW | `facing upper-left diagonal` |

---

# ══════════════════════════════
# IDLE ANIMATIONS
# ══════════════════════════════

**Interpolate ayarları:**
- Set start image: `base/{char}_{yön}.png`
- Set end image: **AYNI** `base/{char}_{yön}.png`
- Number of frames: **8**

---

## WARBLADE — Fight Stance Idle

**SHORT** *(Enhance with AI düğmesini kullan)*
```
Heavily armored plate warrior standing still in combat stance. Chest rising and falling with breath. Armor plates shift slightly. Cloak edge flutters.
```

**LONG** *(direkt kullan, Enhance with AI basma)*
```
Heavily armored plate-clad warrior standing in combat fight stance, 128x128 pixel art, low top-down view. Breathing idle loop: chest rises and falls slowly. Shoulder pauldrons shift slightly with each breath. Heavy cloak edge moves minimally from breath. Greatsword held ready at side, grip firm. Combat-ready stillness. Seamless loop, no displacement.
```

---

## ELEMENTALIST — Idle

**SHORT**
```
Robed battle mage standing still. Magical energy wisping around hands. Robe fabric shifting gently from magical aura. Breathing.
```

**LONG**
```
Robed battle mage with heroic proportions, 128x128 pixel art, low top-down view. Breathing idle loop: chest rises and falls. Magical energy particles slowly orbit both hands with soft glow. Robe hem sways gently. Hood fabric shifts slightly. Magical aura pulses softly around figure. Combat-ready stillness. Seamless loop, no displacement.
```

---

## RANGER — Idle

**SHORT**
```
Archer standing ready, slight weight shift between feet. Bow held loosely. Alert scanning stance. Breathing.
```

**LONG**
```
Archer ranger with heroic proportions holding a longbow, 128x128 pixel art, low top-down view. Breathing idle loop: chest expands and contracts. Very slight weight shift left-right between feet. Bow arm held loosely at side. Arrow quiver visible at back, no movement. Hair or hood fabric moves minimally. Alert scanning readiness. Seamless loop, no displacement.
```

---

## SHADOWBLADE — Idle

**SHORT**
```
Dual-blade assassin standing still. Dark cloak fluttering slightly. Both blades held ready. Shadowy energy wisping. Breathing.
```

**LONG**
```
Dual-wielding dark assassin holding two short blades, 128x128 pixel art, low top-down view. Breathing idle loop: torso rises and falls. Dark cloak edges flutter very slightly as if in faint breeze. Both blades held ready at sides. Faint shadow energy wisps around figure edges. Predatory stillness. Seamless loop, no displacement.
```

---

# ══════════════════════════════
# WALK ANIMATIONS
# ══════════════════════════════

**Interpolate ayarları:**
- Set start image: `base/{char}_{yön}.png`
- Set end image: **AYNI** `base/{char}_{yön}.png`
- Number of frames: **8**

---

## WARBLADE — Walk

> ⚠ Walk için Interpolate same-frame yöntemi çalışmayabilir (AI hareket yönü bilemez).
> **Önce Seçenek A'yı dene. Tutmazsa Seçenek B.**

### Seçenek A — Animate with text (new) [Birincil]

Interpolate yerine **Animate → Animate with text (new)** kullan.
Set reference image: `warblade_S.png` | Number of frames: 8

**SHORT**
```
Heavily armored plate warrior walking toward camera. Strong left-right body sway each step. Counter-swing arms. Head bobs. Weight shifts dramatically between feet. Loop.
```

**LONG**
```
Heavily armored plate-clad warrior walking forward, 128x128 pixel art, low top-down view, facing downward toward camera.
Walking cycle loop: body sways strongly left and right with each footfall — entire torso tilts side to side. Right arm swings forward when left leg steps, left arm forward when right leg steps. Clear up-down head bob with each step. Heavy armor plates shift and clank from weight transfer. Deliberate heavy footfall, boots planting hard. 8 frames, seamless loop.
```

### Seçenek B — Interpolate + mid-stride keyframe

ADIM 1: **Edit Image PRO** → mid-stride pozu üret (start: warblade_S.png):

**SHORT**
```
Heavily armored warrior mid-stride — left leg fully forward, right leg back, torso tilted forward into step, right arm counter-swung forward. Maximum stride extension.
```

**LONG**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view, facing downward toward camera.
Mid-stride keyframe: left leg fully stepped forward, right leg extended back. Torso leaning forward into stride. Right arm swung forward counter to left leg, left arm pulled back. Full body weight committed to forward foot. Maximum stride extension — clear, readable walking pose.
```

ADIM 2: **Interpolate (new)**
- Start: `warblade_S.png` | End: mid-stride frame | Frames: 8
```
Heavily armored warrior strides forward — shifts from upright stance into full mid-stride lean, weight commits to forward foot, arm counter-swings. Heavy deliberate walking motion.
```

---

## ELEMENTALIST — Walk

**SHORT**
```
Robed battle mage walking forward. Robes flowing and billowing with movement. Magical energy trailing from hands.
```

**LONG**
```
Robed battle mage walking forward, 128x128 pixel art, low top-down view. Walking cycle: robe hems flow and billow with each step. Arms swing with stride, faint magical energy trails from hands. Light graceful footfall. Staff (if held) swings slightly. Robe fabric follows movement momentum. Looping walk cycle, returns to start pose.
```

---

## RANGER — Walk

**SHORT**
```
Archer walking forward. Light athletic movement. Bow held in front hand. Arrow quiver bouncing slightly at hip.
```

**LONG**
```
Archer ranger walking forward holding a longbow, 128x128 pixel art, low top-down view. Walking cycle: athletic light footfall, arms swing with stride. Bow held in leading hand, quiver bouncing slightly at hip from step momentum. Alert head, slight forward lean. Looping walk cycle, returns to start pose.
```

---

## SHADOWBLADE — Walk

**SHORT**
```
Dual-blade assassin walking forward. Silent predatory movement. Slight crouch. Both blades swinging at sides.
```

**LONG**
```
Dual-wielding dark assassin walking forward holding two short blades, 128x128 pixel art, low top-down view. Walking cycle: silent low predatory movement with slight crouch. Both blades swing forward-back with each stride. Dark cloak flows behind movement. Light-footed, near-silent footfall. Looping walk cycle, returns to start pose.
```

---

# ══════════════════════════════
# DEATH ANIMATIONS
# ══════════════════════════════

Death 2 adımlıdır. Önce ölü pozu üret, sonra Interpolate yap.

---

## ADIM 1 — Edit Image PRO (Ölü Poz Sprite)

pixellab.ai → **Edit Image PRO**

**Ayarlar (sabit):**
- Image to edit: o yönün base sprite'ını yükle (sürükle-bırak)
- Output size: **128x128**
- Remove background: **✓ açık**

**Çıktıyı kaydet:**
```
RIMA\Assets\Sprites\Characters\{Char}\death_frames\death_{yön}.png
```

---

### WARBLADE — Death Pose (Edit Image PRO)

**SHORT**
```
Heavily armored warrior fallen dead. Collapsed completely flat on ground face-down. Arms spread to sides. Motionless.
```

**LONG**
```
Heavily armored plate-clad warrior, 128x128 pixel art, low top-down view. Death pose: warrior has collapsed completely onto the ground, lying flat face-down. Both arms spread to sides, legs extended behind. Heavy armor now dead weight pressing to ground. Greatsword fallen at side. Motionless, permanent lying pose. Transparent background.
```

---

### ELEMENTALIST — Death Pose (Edit Image PRO)

**SHORT**
```
Robed mage fallen dead. Collapsed flat on ground face-down. Staff fallen beside. Robes spread around body. Motionless.
```

**LONG**
```
Robed battle mage, 128x128 pixel art, low top-down view. Death pose: mage collapsed completely onto the ground, lying flat face-down. Robes spread and draped around fallen body. Staff fallen to one side. Arms limp at sides, legs extended. Magical energy fully dissipated. Motionless. Transparent background.
```

---

### RANGER — Death Pose (Edit Image PRO)

**SHORT**
```
Archer fallen dead. Collapsed flat on ground face-down. Longbow fallen beside. Arrows scattered. Motionless.
```

**LONG**
```
Archer ranger, 128x128 pixel art, low top-down view. Death pose: ranger collapsed completely onto the ground, lying flat face-down. Longbow fallen to one side. Arrows scattered from quiver. Arms limp, legs extended. Body motionless. Transparent background.
```

---

### SHADOWBLADE — Death Pose (Edit Image PRO)

**SHORT**
```
Dual-blade assassin fallen dead. Collapsed flat face-down. Both blades fallen at sides. Dark cloak spread. Motionless.
```

**LONG**
```
Dual-wielding dark assassin, 128x128 pixel art, low top-down view. Death pose: assassin collapsed completely onto the ground, lying flat face-down. Both short blades fallen to sides. Dark cloak spread around the fallen body. Arms limp, legs extended. Shadow energy fully dissipated. Motionless. Transparent background.
```

---

## ADIM 2 — Interpolate (Düşme Animasyonu)

**Interpolate ayarları:**
- Set start image: `base/{char}_{yön}.png` (ayakta)
- Set end image: ADIM 1 çıktısı (yerde)
- Number of frames: **10**

---

### WARBLADE — Death Fall Interpolate

**SHORT**
```
Heavily armored warrior staggers and falls forward collapsing dead to the ground. No loop.
```

**LONG**
```
Heavily armored plate-clad warrior struck down. Takes a staggering step, knees buckle, falls forward collapsing completely to the ground. Heavy armor momentum carries the fall. Arms give way, body crumples flat. Final lying pose. No loop.
```

---

### ELEMENTALIST — Death Fall Interpolate

**SHORT**
```
Robed mage struck down, staggers and falls forward collapsing dead to the ground. Robes billow as they fall. No loop.
```

**LONG**
```
Robed battle mage struck down. Staggers backward, arms go limp, robes billow outward, falls forward collapsing completely to the ground. Body folds and crumples flat. Staff clatters. Final lying pose. No loop.
```

---

### RANGER — Death Fall Interpolate

**SHORT**
```
Archer struck down, staggers and falls forward collapsing dead to the ground. Bow hand drops. No loop.
```

**LONG**
```
Archer ranger struck down. Staggers, bow drops from hand, knees buckle, falls forward collapsing completely to the ground. Body crumples flat. Quiver spills. Final lying pose. No loop.
```

---

### SHADOWBLADE — Death Fall Interpolate

**SHORT**
```
Dual-blade assassin struck down, staggers and collapses dead to the ground. Both blades drop. No loop.
```

**LONG**
```
Dual-wielding dark assassin struck down. Staggers, both blades drop from hands, dark cloak billows outward, collapses forward to the ground. Body crumples flat. Shadow energy dissipates. Final lying pose. No loop.
```

---

# ══════════════════════════════
# QC — Kalite Kontrol
# ══════════════════════════════

```
IDLE PASS:
  ✓ Loop seamless — başlangıç ve bitiş aynı görünüyor
  ✓ Küçük hareket var (nefes, kumaş, enerji)
  ✓ Karakter sabit duruyor, yer değiştirmiyor

WALK PASS:
  ✓ Adım hareketi okunabilir
  ✓ Loop seamless
  ✓ Karakter kimliği korunmuş (silah, kıyafet)

DEATH PASS:
  ✓ İlk frame: ayakta/base sprite ile eşleşiyor
  ✓ Son frame: yerde yatan ile eşleşiyor
  ✓ Düşme hareketi akışlı, no-loop
  ✓ Frame sayısı: 10

FAIL çözümleri:
  Hareket yok (idle)   → "clear subtle breathing motion, chest rising and falling" ekle
  Loop kırık           → "seamless perfect loop, identical start and end pose" ekle
  Walk donmuş          → "clear walking stride, weight shifting, arms swinging" ekle
  Death stiff          → "staggers, knees buckle, crumples, collapses" ekle
  Karakter şekil değiş → Death end frame'i daha yakın görünümlü üret (daha az sapma)
  Silah/kıyafet kaybol → Edit Image PRO'da "holding [silah]" ekle death pose'a
```

---

# ══════════════════════════════
# RETRY STRATEJİSİ
# ══════════════════════════════

| Deneme | Değişiklik |
|--------|-----------|
| 1 | SHORT prompt → Enhance with AI |
| 2 | LONG prompt direkt |
| 3 | "Enhance with AI" kapalıyken LONG prompt tekrar |
| 4 | Death için: Edit Image PRO'da farklı varyasyon seç |
| 5 | Claude'a ilet |

---

## BİTİNCE NE YAP

"Warblade idle S hazır", "Elementalist walk tüm yönler hazır" vs. de.
Her yön bitince söyleyebilirsin, tümünü bekleme.

Claude şunları yapar:
1. Sprite import (PPU=64, Point filter, No compression)
2. .anim clip build (idle/walk: loop=true, death: loop=false)
3. AnimatorController'a state + BlendTree ekleme
