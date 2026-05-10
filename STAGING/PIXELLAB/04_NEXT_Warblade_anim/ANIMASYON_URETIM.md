# Warblade — Animasyon Uretim Rehberi
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR

- **Tool**: PixelLab web app -> karakter sayfasi -> Add Animation -> **Custom Animation V3**
- **YASAK**: Standalone "Animate with Text NEW" | `animate_character` MCP | Preset butonlar
- **Start Frame kaynagi**: `Characters/anchors/warblade/rotations/<direction>_clean.png` (Eraser Pass sonrasi)
- **Canvas**: 252x252 (v3 otomatik)
- **Yon uretimi**: 8 yon HEPSI ayri uretilir, FLIP YOK.
  Yonler: `south`, `south-west`, `south-east`, `east`, `north-east`, `north`, `north-west`, `west`
- **Frame kurallari** (kesinlikle uy):
  - Idle: 6-8 frame | Keep First: ON
  - Hurt: 4 frame TAM SAYI (3 gecersiz) | Keep First: ON
  - Death: 6-8 frame | Keep First: **OFF**
  - Walk interpolation: 6 frame | Keep First: ON
  - Attack PEAK: 4 frame | Keep First: **OFF** -> son frame = PEAK
  - Attack Windup: 4 frame | Keep First: ON | Start=clean, End=PEAK
  - Attack Follow: 4 frame | Keep First: ON | Start=PEAK, End=clean
  - Toplam attack unique: 8 frame (PEAK paylasilir)
  - Dash: 4 frame | Keep First: ON

---

## KARAKTER GORSEL OZETI (uymak ZORUNLU)

- **Silah**: SAG elde TEK EL longsword. Blade ~%65 karakter boyu, mavi-teal kristal kenar, blade ucu serbest tutusta asagi.
- **Kiyafet**: Bare arms (ciplak kollar), deri kilt/etek, deri kemer, **SOL omuzda** round metal pauldron.
- **Karakter**: Erkek, siyah kisa sac + kisa sakal, guclu yapi.
- **YOK**: pelerin, gauntlet, plaka zirh, greatsword, two-handed grip.

---

## ERASER PASS (ZORUNLU)

8 yon icin Pixelorama workflow:
1. `Characters/anchors/warblade/rotations/<direction>.png` ac.
2. Magic Wand ile saf yesil background'u sec, sil.
3. Karakter siluetinin disindaki kalintilari Eraser ile temizle.
4. **Sword'u silme** -- silah karakter parcasidir.
5. Kaydet -> `Characters/anchors/warblade/rotations/<direction>_clean.png`

Bu dosyalar tum Custom Animation V3 cagrilarinda Start/End frame olarak kullanilacak.

---

## YON REFERANS TABLOSU (sword arm = sag)

| Yon | Kilic ekranda |
|---|---|
| south | Screen-LEFT |
| south-east | Screen-left, hafif one |
| east | Far side (kamera uzagi) |
| north-east | Screen-RIGHT |
| north | Screen-RIGHT |
| north-west | Screen-right, merkeze yakin |
| west | Near camera (one dogru) |
| south-west | Screen-left |

> Tum animasyonlarda her yon icin **DIRECTION NOTE** silahin hangi tarafta gorundugune gore yorumlanir.

---

## ADIM 1 -- Idle

**Settings**
- Tool: Custom Animation V3
- Start Frame: `<direction>_clean.png`
- End Frame: bos (loop)
- Frames: 7
- Keep First: ON

**Prompt** (copy-paste)
```
High top-down view, idle breathing loop. Sword arm (right) hangs relaxed,
blade tip pointing down to the ground, hilt loose in palm. Chest rises
and falls slowly with breath. Weight shifts subtly between feet. Head
sweeps 10-15 degrees side to side scanning. Off-hand (left) hangs free,
fingers slightly curled. Bare arms, leather kilt, left shoulder pauldron
stays consistent. Black hair and short beard catch micro-motion. No
weapon swing, no foot lift, only breath and weight shift.
```

**DIRECTION NOTE (her yonde tekrar et)**
- Yon referans tablosundan kilic pozisyonunu prompt'a ek bir satirla belirt.
  Ornek (south): "Sword visible on screen-left side."
  Ornek (north): "Sword visible on screen-right side."

---

## ADIM 2 -- Hurt

**Settings**
- Frames: 4 (tam sayi)
- Keep First: ON
- Start Frame: `<direction>_clean.png`
- End Frame: bos

**Prompt**
```
High top-down view, 4-frame hit reaction. Frame 1: neutral stance.
Frame 2: whole body recoils backward, sword arm tightens grip on hilt
(knuckles clench), knees flex slightly. Frame 3: weight stays back,
blade rises into a defensive guard between body and threat. Frame 4:
recovery, posture returning to neutral. No blade swing, recoil only.
Pauldron and kilt show subtle motion from impact.
```

**DIRECTION NOTE**: Yon tablosuna gore kilic guard'a kalkarken hangi tarafa cikiyor belirt.

---

## ADIM 3 -- Death

**Settings**
- Frames: 7
- Keep First: **OFF**
- Start Frame: `<direction>_clean.png`
- End Frame: bos

**Prompt**
```
High top-down view, death sequence. Frame 1: stagger, weight uncertain.
Frame 2: sword slips from right hand, blade falls to ground beside the
character. Frame 3-4: knees buckle and drop. Frame 5-6: torso pitches
forward over collapsed knees. Frame 7: body settles, head down. Heavy,
weight-driven motion. No dramatic flourish. Pauldron tilts as shoulder
drops.
```

**DIRECTION NOTE**: Kilic dustugu yer = yon tablosundaki kilic tarafi (orn. south -> blade lands screen-left).

---

## ADIM 4 -- Walk Cycle (3 alt-adim)

### 4a -- PoseA secimi
1. PixelLab web app -> Standalone **Animate with Text NEW** ile karakteri yurut.
2. Anchor rotation'ini start frame ver, "walk" promptu, 12 frame, Keep First ON.
3. Ciktidan en uc stride pozunu sec (one ayak ileri, arka ayak geride, kalca rotasyonu maksimum).
4. Sec -> Eraser pass -> `outputs/walk/<direction>/PoseA_clean.png`

### 4b -- PoseB uretimi (stride flip)
- Aseprite (veya Pixelorama) -> PoseA_clean.png ac -> **Horizontal Flip**.
- Bu **direction flip DEGIL**, stride phase flip (one ayak <-> arka ayak swap).
- Kaydet -> `outputs/walk/<direction>/PoseB_clean.png`

> Onemli: Flip sonrasi sword arm karakterin sol elinde gibi gorunse de, bu sadece stride parityidir. Walk cycle 6 frame interpolasyonda tek el longsword tutusu korunmali (asagidaki prompt zorlar).

### 4c -- Walk interpolation
**Settings**
- Tool: Custom Animation V3
- Start Frame: `PoseA_clean.png`
- End Frame: `PoseB_clean.png`
- Frames: 6
- Keep First: ON

**Prompt**
```
High top-down view, walking warrior, heavy deliberate steps. Heel strikes
first, weight rolls forward. Sword arm (right hand) swings naturally but
the blade stays controlled, tip trailing slightly behind. Off-hand swings
opposite to stride. Pauldron rocks with shoulder rotation. Kilt sways
with hip motion. Black hair has subtle bob. Single-handed grip on the
longsword maintained throughout, never two-handed.
```

**DIRECTION NOTE**: Yon tablosuna gore kilic ayni tarafta kalir; trailing arc o tarafta yapilir.

---

## ADIM 5a -- Attack LMB (Quick Slash) -- 3 segment

> Once PEAK uret, sonra Windup ve Follow segmentlerinde PEAK'i ortak kullan.

### 5a.1 -- PEAK
**Settings**
- Frames: 4
- Keep First: **OFF**
- Start Frame: `<direction>_clean.png`
- End Frame: bos

**Prompt**
```
High top-down view, single-handed horizontal sword sweep, building to
peak strike on the final frame. Frame 4 (PEAK): sword arm fully extended,
blade has crossed body silhouette to the opposite side, wrist locked,
torso twisted 20-30 degrees into the swing, hips rotated, leading foot
planted. Bare arm muscle tension visible. Pauldron catches motion.
Off-hand counter-balances away from swing. NO trail VFX, NO motion blur,
clean pixel silhouette.
```

Son frame'i `outputs/attack_lmb/<direction>/PEAK.png` olarak Eraser pass + kaydet.

### 5a.2 -- Windup
**Settings**
- Frames: 4 | Keep First: ON
- Start: `<direction>_clean.png` | End: `PEAK.png`

**Prompt**
```
Sword arm draws back from rest into wind-up. Elbow pulls behind torso,
blade angled away from target line. Shoulder coils. Body weight shifts
to back foot. Off-hand rises slightly for balance. Single-handed grip.
```

### 5a.3 -- Follow
**Settings**
- Frames: 4 | Keep First: ON
- Start: `PEAK.png` | End: `<direction>_clean.png`

**Prompt**
```
Follow-through after horizontal slash. Sword arm continues across body
to opposite side at full extension, then decelerates. Torso untwists,
hips return to neutral. Sword arm relaxes back to resting position with
blade tip lowering. Off-hand returns to side.
```

**DIRECTION NOTE**: PEAK frame'inde kilic, yon tablosundaki "kilic ekranda" tarafinin **karsi** tarafina gecmis olmali (sweep karsi tarafa biter).

---

## ADIM 5b -- Attack RMB (Power Slam) -- 3 segment

### 5b.1 -- PEAK
**Settings**
- Frames: 4 | Keep First: **OFF**
- Start: `<direction>_clean.png` | End: bos

**Prompt**
```
High top-down view, single-handed overhead vertical slam, peak on final
frame. Frame 4 (PEAK): sword arm fully extended downward in front of
character, blade vertical pointing toward ground at the strike line,
wrist locked. Shoulders square, both feet planted wide for power. Off-hand
extended back for counterweight. Maximum power pose. NO impact VFX.
```

Son frame -> `outputs/attack_rmb/<direction>/PEAK.png` (Eraser pass).

### 5b.2 -- Windup
**Settings**: 4 frame | Keep First: ON | Start=clean, End=PEAK
```
Sword arm raises overhead. Elbow lifts above head, wrist rotates so
blade points up and slightly back. Body coils, weight loads onto back
leg. Pauldron rises with shoulder. Off-hand drops to balance.
```

### 5b.3 -- Follow
**Settings**: 4 frame | Keep First: ON | Start=PEAK, End=clean
```
After overhead slam: blade rests low in front, sword arm decelerates,
shoulder relaxes. Body straightens from forward weight commit. Sword
arm returns to neutral resting position with blade tip down.
```

**DIRECTION NOTE**: Slam ekseni karakterin one yonu boyunca; kilic vertikal merkez hatti uzerinde tamamlanir.

---

## ADIM 6 -- Dash

**Settings**
- Frames: 4 | Keep First: ON
- Start: `<direction>_clean.png` | End: bos

**Prompt**
```
High top-down view, forward dash. Frame 1: launch, leading foot pushes
off. Frame 2: peak forward lean (torso ~25 degrees), back foot trailing,
sword arm pulls blade backward along body line for aerodynamic profile.
Frame 3: front foot lands. Frame 4: recovery to neutral stance.
Single-handed sword grip maintained, blade trailing behind. Bare arms,
kilt streams behind from speed.
```

**DIRECTION NOTE**: Dash karakterin baktigi yone dogru. Kilic trailing icin yon tablosundaki tarafin tersine cekilebilir (drag arc).

---

## ADIM 7 -- Weapon Pass (Edit Image Pro)

Her yonun her animasyon frame'inde silah tutarliligi icin pass:
1. PixelLab web app -> Karakter sayfasi -> ilgili animasyon -> **Edit Image Pro**.
2. Frame'leri tek tek ac, asagidaki prompt'u uygula.

**Prompt**
```
Refine the longsword held in the right hand. Single-handed grip only,
never two-handed. Blade: steel body color #6E7280 with highlight #8A8E98,
blue-teal crystal cutting edge #7BA7BC. Hilt: leather wrap #3A2818,
crossguard dark steel #282830. Blade length about 65% of character
height. Maintain blade silhouette consistent across all frames of this
direction. Do not add motion trails, sparks, or VFX.
```

3. Frame -> Eraser pass (residual artifact temizligi) -> kaydet.

---

## QC CHECKLIST (her animasyon sonrasi)

- [ ] Kilic SAG elde mi? (Tum frame'lerde)
- [ ] Tek el grip mi? (Two-handed grip varsa REJECT)
- [ ] Blade boyu ~%65 karakter boyu mu?
- [ ] Blade rengi steel + mavi-teal kristal kenar mi?
- [ ] Sol omuzda pauldron var mi?
- [ ] Bare arms gorunur mu? (Gauntlet/sleeve eklenmemis)
- [ ] Pelerin/hood eklenmemis mi?
- [ ] Yon tablosundaki kilic pozisyonu dogru mu?
- [ ] Frame sayisi prompt'taki ile birebir mi?
- [ ] Eraser pass yapildi mi (background tamamen seffaf)?
- [ ] Karakter canvas merkezinde mi (kayma yok)?

---

## KAYIT KLASORU

```
Characters/anchors/warblade/
  rotations/
    south_clean.png, south-west_clean.png, ... (8 yon)

STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/
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

8 yon x 7 animasyon = 56 set. PEAK paylasimli oldugundan attack unique frame = 8 (windup 3 + PEAK 1 + follow 3 + clean 1).
