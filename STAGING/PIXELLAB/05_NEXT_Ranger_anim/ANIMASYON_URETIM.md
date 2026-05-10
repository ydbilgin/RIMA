# Ranger — Animasyon Uretim Rehberi
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR

- **Tool**: PixelLab web app -> karakter sayfasi -> Add Animation -> **Custom Animation V3**
- **YASAK**: Standalone "Animate with Text NEW" | `animate_character` MCP | Preset butonlar
- **Start Frame kaynagi**: `Characters/anchors/ranger/rotations/<direction>_clean.png` (Eraser Pass sonrasi)
- **Canvas**: 252x252 (v3 otomatik)
- **Yon uretimi**: 8 yon HEPSI ayri uretilir, FLIP YOK.
  Yonler: `south`, `south-west`, `south-east`, `east`, `north-east`, `north`, `north-west`, `west`
- **Frame kurallari**:
  - Idle: 6-8 frame | Keep First: ON
  - Hurt: 4 frame TAM SAYI | Keep First: ON
  - Death: 6-8 frame | Keep First: **OFF**
  - Walk interpolation: 6 frame | Keep First: ON
  - Attack PEAK: 4 frame | Keep First: **OFF**
  - Attack Windup: 4 frame | Keep First: ON | Start=clean, End=PEAK
  - Attack Follow: 4 frame | Keep First: ON | Start=PEAK, End=clean
  - Toplam attack unique: 8 frame
  - Dash: 4 frame | Keep First: ON

---

## KARAKTER GORSEL OZETI (uymak ZORUNLU)

- **Silah**: SOL elde dikey **recurve bow** (kompakt). Alt limb bacak yaninda, ip gevsek. Sag el serbest.
- **Kiyafet**: Uzun vahsi beyaz/gumus sac (cok belirgin). Koyu crop top, koyu sort, deri uyluk cizmesi, deri kemer + kucuk pouch.
- **Karakter**: Kadin, atletik, kendinden emin.
- **YOK**: baslik / hood, pelerin, belirgin sirt sadagi (quiver), iki elli yay tutusu, kompozit asiri sus.

---

## ERASER PASS (ZORUNLU)

8 yon icin Pixelorama:
1. `Characters/anchors/ranger/rotations/<direction>.png` ac.
2. Magic Wand ile yesil bg sec, sil.
3. Karakter siluetinin disindaki kalintilar -> Eraser.
4. Yay'i silme -- silah karakter parcasi.
5. Kaydet -> `<direction>_clean.png`.

---

## YON REFERANS TABLOSU (bow arm = sol)

| Yon | Yay ekranda |
|---|---|
| south | Screen-RIGHT |
| south-east | Screen-right, hafif one |
| east | Far side (kamera uzagi) |
| north-east | Screen-LEFT |
| north | Screen-LEFT |
| north-west | Screen-left |
| west | Near camera (one dogru) |
| south-west | Screen-right |

> Tum animasyonlarda her yon icin **DIRECTION NOTE** yayin hangi tarafta gorundugune gore yorumlanir.

---

## ADIM 1 -- Idle

**Settings**
- Frames: 7 | Keep First: ON
- Start: `<direction>_clean.png` | End: bos

**Prompt**
```
High top-down view, idle breathing loop. Bow arm (left) holds the recurve
bow vertical, lower limb resting beside the leg, string slack. Off-hand
(right) hangs free with subtle finger curl. Chest rises and falls with
breath. Weight shifts subtly between feet. Long silver-white hair
catches micro-motion. Crop top, leather thigh boots, belt pouch stay
consistent. Confident athletic posture. No bow draw, no foot lift.
```

**DIRECTION NOTE**: Yon tablosundan yay tarafini ekle.
  Ornek (south): "Bow visible on screen-right side, lower limb beside right leg of figure (camera-left)."
  Ornek (north): "Bow visible on screen-left side."

---

## ADIM 2 -- Hurt

**Settings**: 4 frame | Keep First: ON | Start=clean | End=bos
```
High top-down view, 4-frame hit reaction. Frame 1: neutral. Frame 2:
bow arm jerks upward as left shoulder reflexes, body twists away from
hit direction. Frame 3: right hand pulls back behind body, weight on
back foot. Frame 4: recovery toward neutral, bow lowering. Long silver
hair flares from impact motion. No bow draw.
```
**DIRECTION NOTE**: Yon tablosundan yayin yukseldigi tarafi koru.

---

## ADIM 3 -- Death

**Settings**: 7 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, death sequence. Frame 1: stagger, weight uncertain.
Frame 2: recurve bow slips from left hand, falls beside the figure.
Frame 3: right arm reaches out then drops. Frame 4-5: knees give,
slow collapse. Frame 6-7: torso settles forward, hair fans on ground.
Slow weight-driven collapse, no theatrics.
```
**DIRECTION NOTE**: Yay yon tablosundaki tarafa duser.

---

## ADIM 4 -- Walk Cycle (3 alt-adim)

### 4a -- PoseA secimi
1. Standalone Animate with Text NEW -> 12 frame walk uret.
2. En uc stride pozunu sec (one/arka ayak max ayrim).
3. Eraser pass -> `outputs/walk/<direction>/PoseA_clean.png`.

### 4b -- PoseB (stride flip)
- Aseprite/Pixelorama -> PoseA_clean -> Horizontal Flip.
- Stride parity flip (yon flip DEGIL).
- Kaydet -> `PoseB_clean.png`.

> Flip sonrasi yay sag elde gorunse de bu sadece stride parity. 6-frame interpolasyon prompt'u sol-el yay tutusunu zorlar.

### 4c -- Walk interpolation
**Settings**: 6 frame | Keep First: ON | Start=PoseA | End=PoseB
```
High top-down view, light agile walk. Bow arm (left) keeps the recurve
bow vertical, lower limb close to thigh, controlled. Off-hand (right)
swings naturally opposite to stride. Long silver hair sways with each
step. Belt pouch bounces lightly. Confident smooth gait, no heavy
heel strike. Bow stays in left hand throughout, never two-handed.
```
**DIRECTION NOTE**: Yay ayni tarafta kalir; salllanma yok, kontrollu tutus.

---

## ADIM 5a -- Attack LMB (Quick Shot) -- 3 segment

### 5a.1 -- PEAK
**Settings**: 4 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, fast bow snap shot, peak draw on final frame.
Frame 4 (PEAK): bow arm (left) extended toward target, recurve bow
tilted to match target angle, string fully drawn back. Right hand at
chin/cheek anchor, fingers around string. Body squared, leading foot
planted. Tense athletic pose. NO arrow VFX, NO trail. Hair pulled
back from draw motion.
```
PEAK frame -> `outputs/attack_lmb/<direction>/PEAK.png` (Eraser pass).

### 5a.2 -- Windup
**Settings**: 4 frame | Keep First: ON | Start=clean | End=PEAK
```
Bow arm (left) raises and rotates toward target line. Right hand
reaches up and clamps the string, beginning the draw. Body squares
toward aim direction, weight settles over centerline.
```

### 5a.3 -- Follow
**Settings**: 4 frame | Keep First: ON | Start=PEAK | End=clean
```
After release: right hand snaps back past ear, fingers spread. Bow arm
relaxes forward then lowers. String shows brief vibration on first
frame after release. Body unwinds, weight returns to neutral.
```
**DIRECTION NOTE**: Yay yon tablosundaki tarafta kalir; sag el (string hand) ekranda zit tarafta cene/kulak hizasinda anchor olur.

---

## ADIM 5b -- Attack RMB (Charged Shot) -- 3 segment

### 5b.1 -- PEAK
**Settings**: 4 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, charged power shot, peak hold on final frame.
Frame 4 (PEAK): bow arm (left) fully extended toward target, recurve
deeply bent (deeper than quick shot), right hand drawn behind ear with
elbow high. Body absolutely still, breath held, both feet anchored
wide. Maximum draw tension visible. Eyes locked forward. NO VFX,
NO arrow trail.
```
PEAK -> `outputs/attack_rmb/<direction>/PEAK.png`.

### 5b.2 -- Windup
**Settings**: 4 frame | Keep First: ON | Start=clean | End=PEAK
```
Slow deliberate pull. Bow arm extends forward, right hand clamps
string and pulls back through full range. Right elbow rises high.
Stance widens, weight centers. Long pull, controlled tension.
```

### 5b.3 -- Follow
**Settings**: 4 frame | Keep First: ON | Start=PEAK | End=clean
```
Release: right hand snaps back forcefully past ear and shoulder.
Bow arm dips slightly from recoil then settles. Body briefly
recoils backward, then recovers to neutral. Hair displaced by
release impulse.
```
**DIRECTION NOTE**: PEAK'te full draw geometrisi her yonde dogru — bow arm hedefe, string hand cene/kulak arkasinda.

---

## ADIM 6 -- Dash

**Settings**: 4 frame | Keep First: ON | Start=clean | End=bos
```
High top-down view, agile sidestep / quick reposition dash. Frame 1:
push-off. Frame 2: peak lateral lean (~25 degrees), bow arm tucks
recurve close to body for clearance, right hand crosses body for
balance. Frame 3: trailing foot lands. Frame 4: recovery. Hair
streams in dash direction. Bow stays securely in left hand.
```
**DIRECTION NOTE**: Dash karakterin baktigi yone dogru. Yay vucuda yakin tutulur, yon tablosundaki tarafindan ayrilmaz.

---

## ADIM 7 -- Weapon Pass (Edit Image Pro)

**Prompt**
```
Refine the recurve bow held in the left hand. Compact bow, length
about 70% of character height. Wood body: warm brown #5A4028 with
highlight #7A5838. String: off-white #C8C0A8, single hair-thin pixel
line. Lower limb rests beside the leg in idle, draws back when firing.
NEVER draw a horizontal arrow on the bow — arrows are engine VFX.
Keep silhouette consistent across all frames of this direction.
No glow, no sparkle.
```
Frame -> Eraser pass -> kaydet.

---

## QC CHECKLIST

- [ ] Yay SOL elde mi? (Tum frame'lerde)
- [ ] Recurve bow boyu ~karakter boyu %70 mi?
- [ ] String tek piksel cizgisi mi (kalin sürüm REJECT)?
- [ ] Yatay ok cizilmemis mi (engine VFX)?
- [ ] Hood/baslik/pelerin eklenmemis mi?
- [ ] Uzun gumus/beyaz sac belirgin mi?
- [ ] Crop top + sort + uyluk cizme korunmus mu?
- [ ] Yon tablosundaki yay pozisyonu dogru mu?
- [ ] Frame sayisi tam mi?
- [ ] Eraser pass yapildi mi (saf transparan bg)?
- [ ] Karakter canvas merkezinde mi?

---

## KAYIT KLASORU

```
Characters/anchors/ranger/
  rotations/
    south_clean.png, ... (8 yon)

STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/
  idle/<direction>/frames/
  hurt/<direction>/frames/
  death/<direction>/frames/
  walk/<direction>/PoseA_clean.png
  walk/<direction>/PoseB_clean.png
  walk/<direction>/frames/
  attack_lmb/<direction>/PEAK.png
  attack_lmb/<direction>/windup/
  attack_lmb/<direction>/follow/
  attack_rmb/<direction>/PEAK.png
  attack_rmb/<direction>/windup/
  attack_rmb/<direction>/follow/
  dash/<direction>/frames/
```

8 yon x 7 animasyon = 56 set. Attack unique frame = 8.
