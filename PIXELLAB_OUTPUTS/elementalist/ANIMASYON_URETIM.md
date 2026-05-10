# Elementalist — Animasyon Uretim Rehberi
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR

- **Tool**: PixelLab web app -> karakter sayfasi -> Add Animation -> **Custom Animation V3**
- **YASAK**: Standalone "Animate with Text NEW" | `animate_character` MCP | Preset butonlar
- **Start Frame kaynagi**: `Characters/anchors/elementalist/rotations/<direction>_clean.png` (Eraser Pass sonrasi)
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

- **Silah/Held**: SAG elde **parlayan orb**. Sicak beyaz / altin renk. Sag kol dirsekten kirik, omuz hizasinda — orb yukarida sabit. Sol el serbest.
- **Kiyafet**: Sarisin, neat **bun** (topuz). Mavi kolsuz ust, mavi mini etek, kahve deri cizmeler + kahve deri eldiven.
- **Karakter**: Kadin, zarif yapi.
- **PALM ZONE**: Sag avuc icinin etrafi (~12 px) sprite'ta BOS birakilir — engine VFX overlay icin alan.
- **YOK**: pelerin, hood, asa/staff, kitap, sprite icine isaretlenmis glow/spark VFX.

---

## ERASER PASS (ZORUNLU)

8 yon icin Pixelorama:
1. `Characters/anchors/elementalist/rotations/<direction>.png` ac.
2. Magic Wand ile yesil bg sec, sil.
3. Disindaki kalintilar -> Eraser.
4. **Orb'u silme** -- orb karakter parcasi (ama embedded glow/halo varsa onu temizle, sadece orb solid silueti kalsin).
5. Avuc etrafindaki ~12px alanda sicak isik artiklarini sil (palm zone bos).
6. Kaydet -> `<direction>_clean.png`.

---

## YON REFERANS TABLOSU (orb arm = sag)

| Yon | Orb ekranda |
|---|---|
| south | Screen-LEFT (sag kol gorunur) |
| south-east | Screen-left, hafif one |
| east | Far side (kamera uzagi) |
| north-east | Screen-RIGHT |
| north | Screen-RIGHT |
| north-west | Screen-right, merkeze yakin |
| west | Near camera (one dogru) |
| south-west | Screen-left |

> Tum animasyonlarda her yon icin **DIRECTION NOTE** orbun hangi tarafta gorundugune gore yorumlanir. Palm zone **her yonde** bos kalir.

---

## ADIM 1 -- Idle

**Settings**: 7 frame | Keep First: ON | Start=clean | End=bos
```
High top-down view, idle breathing loop. Right arm holds a small orb at
shoulder height, elbow bent, position STABLE (the orb does not move).
Left arm hangs free with subtle finger curl. Chest rises and falls
slowly. Weight shifts subtly between feet. Blonde hair in a neat bun
holds shape; loose strands catch micro-motion. Blue sleeveless top,
blue mini skirt, brown leather boots, brown leather glove on right
hand stay consistent. PALM ZONE around the orb stays empty in pixels —
no embedded glow drawn into the sprite, leave space for engine VFX.
```
**DIRECTION NOTE**: Yon tablosundan orb tarafini ekle.
  Ornek (south): "Orb visible on screen-left side, right arm clearly readable."
  Ornek (north): "Orb visible on screen-right side."

---

## ADIM 2 -- Hurt

**Settings**: 4 frame | Keep First: ON | Start=clean | End=bos
```
High top-down view, 4-frame hit reaction. Frame 1: neutral. Frame 2:
both hands snap forward and up in instinctive ward gesture, palms
splayed, fingers spread. Body steps backward. Frame 3: hands lower
slightly, weight stays back. Frame 4: recovery, right arm returns
to orb-holding position at shoulder height. Skirt and bun show subtle
recoil motion. Palm zones stay empty.
```
**DIRECTION NOTE**: Ward gesture govdenin onunde olusur; tablodaki orb tarafi geciktirme olmadan resetlenir.

---

## ADIM 3 -- Death

**Settings**: 7 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, death sequence. Frame 1: stagger. Frame 2: orb
slowly slips from right hand — fingers open, right arm drops. Frame
3-4: body sways, knees soften. Frame 5-6: forward collapse, weight
fully surrenders. Frame 7: body settles, hair and skirt fan on ground.
Slow, weight-driven, almost graceful collapse.
```
**DIRECTION NOTE**: Orb yon tablosundaki tarafa duser (sag elden serbest birakilir).

---

## ADIM 4 -- Walk Cycle (3 alt-adim)

### 4a -- PoseA secimi
1. Standalone Animate with Text NEW -> 12 frame walk uret.
2. En uc stride pozunu sec.
3. Eraser pass -> `outputs/walk/<direction>/PoseA_clean.png` (palm zone bos).

### 4b -- PoseB (stride flip)
- Aseprite/Pixelorama -> PoseA_clean -> Horizontal Flip.
- Stride parity flip.
- Kaydet -> `PoseB_clean.png`.

> Flip sonrasi orb sol el gibi gorunse de stride parity icindir; 6-frame interpolasyon prompt'u sag-el orb tutusunu zorlar.

### 4c -- Walk interpolation
**Settings**: 6 frame | Keep First: ON | Start=PoseA | End=PoseB
```
High top-down view, smooth deliberate walk. Right arm keeps the orb at
shoulder height with MINIMAL bob — the held position stays stable. Left
arm swings naturally opposite to stride. Skirt sways with hip motion.
Bun stays neat. Boots step lightly, no heel slam. Orb stays in right
hand throughout. Palm zone stays empty (engine VFX target).
```
**DIRECTION NOTE**: Orb yon tablosundaki tarafta sabit kalir; yuruyusten cok az etkilenir.

---

## ADIM 5a -- Attack LMB (Quick Cast) -- 3 segment

### 5a.1 -- PEAK
**Settings**: 4 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, quick projectile cast, peak on final frame.
Frame 4 (PEAK): both arms fully extended forward in push gesture.
Right arm thrusts the orb forward at chest level (orb projected
forward). Left palm extends forward as guide/release gesture, fingers
spread. Body squared toward cast direction, weight on leading foot.
Sharp release pose. NO projectile drawn, NO VFX (engine handles).
Palm zones empty.
```
PEAK -> `outputs/attack_lmb/<direction>/PEAK.png` (Eraser pass, palm zone temiz).

### 5a.2 -- Windup
**Settings**: 4 frame | Keep First: ON | Start=clean | End=PEAK
```
Right arm draws the orb back briefly toward shoulder, left hand rises
to meet beside it. Body coils slightly, weight loads onto back foot.
Both elbows pull in. Quick compact load.
```

### 5a.3 -- Follow
**Settings**: 4 frame | Keep First: ON | Start=PEAK | End=clean
```
Both arms hold extended for one frame, then retract. Right arm returns
to shoulder-height carry position with the orb. Left arm relaxes back
to side. Body recovers to neutral.
```
**DIRECTION NOTE**: Push gesture karakterin baktigi yone dogru; PEAK'te her iki kol one fully extended, orb yon tablosundaki tarafi koruyarak ileride.

---

## ADIM 5b -- Attack RMB (Charged Cast) -- 3 segment

### 5b.1 -- PEAK
**Settings**: 4 frame | Keep First: **OFF** | Start=clean | End=bos
```
High top-down view, overhead charged cast, peak on final frame.
Frame 4 (PEAK): right arm fully extended UPWARD overhead, orb held
high above head, fingers wrapped around it. Left arm extended forward
at waist height as stabilizing/channeling gesture, palm forward.
Body in maximum vertical extension, both feet planted wide. Hair
slightly displaced by upward motion. Power channeling pose. NO VFX,
palm zones empty.
```
PEAK -> `outputs/attack_rmb/<direction>/PEAK.png`.

### 5b.2 -- Windup
**Settings**: 4 frame | Keep First: ON | Start=clean | End=PEAK
```
Right arm raises slowly from shoulder to fully overhead. Left arm
extends forward and stabilizes at waist. Body straightens, weight
settles wide between feet. Slow deliberate channeling load.
```

### 5b.3 -- Follow
**Settings**: 4 frame | Keep First: ON | Start=PEAK | End=clean
```
Right arm lowers in controlled arc back to shoulder-height carry
position. Left arm retracts to side. Hair settles back. Body
returns to neutral idle posture.
```
**DIRECTION NOTE**: Vertical extension her yonde ayni — orb head'in uzerinde merkez. Tablodaki taraf bilgisi sadece sag kolun gorunur tarafini etkiler.

---

## ADIM 6 -- Dash

**Settings**: 4 frame | Keep First: ON | Start=clean | End=bos
```
High top-down view, forward dash. Frame 1: launch. Frame 2: peak forward
lean (~22 degrees), right arm tucks the orb close to chest for protection,
left arm extends slightly back for balance. Frame 3: front foot lands.
Frame 4: recovery, right arm returns to shoulder-height carry. Skirt
and loose hair strands stream behind. Orb stays secure throughout.
Palm zone stays empty.
```
**DIRECTION NOTE**: Dash karakterin baktigi yone dogru. Orb gogse cekildigi icin tablodaki taraf bilgisi tuck sirasinda azalir; recovery frame'inde geri doner.

---

## ADIM 7 -- Weapon Pass (Edit Image Pro)

**Prompt**
```
Refine the orb held in the right hand. Orb diameter ~16 pixels. Core:
warm white #FFF4D8. Subtle golden rim shading #E8C870 (one pixel ring).
Held by fingers wrapped around it, NO embedded glow halo, NO outward
sparks, NO trail. Maintain orb size and silhouette consistent across
all frames of this direction. PALM ZONE — the ring of pixels immediately
surrounding the orb on the open palm side — must remain transparent /
empty so the engine can render runtime VFX over it. Brown leather glove
on right hand stays visible.
```
Frame -> Eraser pass (palm zone artigi temizlenir) -> kaydet.

---

## QC CHECKLIST

- [ ] Orb SAG elde mi? (Tum frame'lerde)
- [ ] Orb cap ~16 px mi?
- [ ] Orb rengi sicak beyaz + altin rim mi?
- [ ] PALM ZONE (~12px) bos mu? (Embedded glow REJECT)
- [ ] Sag elde kahve deri eldiven gorunur mu?
- [ ] Mavi kolsuz ust + mavi mini etek korunmus mu?
- [ ] Sarisin bun saci dagilmamis mi (idle)?
- [ ] Hood/pelerin/asa eklenmemis mi?
- [ ] Sag kol idle'da omuz hizasinda sabit mi?
- [ ] Yon tablosundaki orb pozisyonu dogru mu?
- [ ] Frame sayisi tam mi?
- [ ] Eraser pass yapildi mi?
- [ ] Karakter canvas merkezinde mi?

---

## KAYIT KLASORU

```
Characters/anchors/elementalist/
  rotations/
    south_clean.png, ... (8 yon)

PIXELLAB_OUTPUTS/elementalist/outputs/
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
