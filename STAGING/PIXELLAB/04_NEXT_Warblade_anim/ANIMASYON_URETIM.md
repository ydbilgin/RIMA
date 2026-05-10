# Warblade — Animasyon Uretim Rehberi
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR
- Tool: Custom Animation V3 (karakter sayfasi -> Add Animation -> Custom Animation V3)
- YASAK: Standalone Animate with Text NEW | animate_character MCP | Preset butonlar
- Start Frame: HER ZAMAN _clean.png (Eraser Pass sonrasi, PixelLab orijinalini kullanma)
- Yonler: Simetrik -> S/E/N uret, W = Unity flipX (8 directions sec Create Character'da)
- Canvas: 252x252 (v3 otomatik)

---

## ERASER PASS (ZORUNLU -- her base sprite uretiminden sonra)
1. Pixelorama'da ac: Characters/anchors/warblade/rotations/[direction].png
2. Eraser tool -> arka plan piksellerini temizle (anti-alias kenarlar dahil)
3. Kaydet: warblade_[direction]_clean.png
4. BU DOSYAYI KULLAN -- PixelLab orijinalini ASLA start frame olarak koyma

---

## ADIM 1 -- 8 Yon Base Sprite
Simetrik -> S/E/N uret, W = Unity flipX. Create Character'da yine "8 Directions" sec.

- PixelLab -> Create Character Pro -> "8 Directions" sec
- Her yon icin Eraser Pass uygula -> _clean.png kaydet

```text
Pixel art warrior character, body-only, no weapon, character occupies ~50% of canvas height (~128px tall) centered on a 252x252 transparent canvas. Wide transparent padding on all sides for animation headroom — DO NOT fill the canvas. High top-down view 30-35° elevation. Heavy plate armor, broad shoulders, cold blue cloth accent #7BA7BC at sash and shoulder straps. Palette: armor steel #4A4E5A / #5C6070 / #6E7280, accent blue #7BA7BC, leather #3A2818 / #5A4028, skin #C9A084 / #A07858, hair dark brown. Stoic stance, feet shoulder-width, arms relaxed. Hard pixel edges, no anti-aliasing, pixel cluster min 4px. NO embedded glow, NO VFX, NO weapon. [FACING SOUTH | FACING EAST | FACING NORTH] (face camera for south).
```

## ADIM 2 -- Idle
- Custom Animation V3
- Start Frame: warblade_[direction]_clean.png | End Frame: bos | Frames: 6-8 | Keep First: ON

```text
Slow controlled breathing -- chest expands and contracts, shoulders rise slightly on inhale.
Greatsword held at right side, blade tip near ground, grip relaxed but ready.
Weight shifts subtly from right to left foot over 4-6 frames, body center stays stable.
Head turns 10-15 degrees left then returns -- scanning. Knees micro-bend on weight shift.
```

## ADIM 3 -- Hurt
- Custom Animation V3
- Start Frame: warblade_[direction]_clean.png | End Frame: bos | Frames: 4 | Keep First: ON

```text
Sharp full-body recoil -- torso snaps backward 15-20 degrees from impact force, head jerks back.
Both hands reflexively tighten on greatsword hilt, sword swings back with body momentum.
Knees buckle slightly, weight drops and shifts backward. Recovery: body pushes back upright frames 3-4.
```

## ADIM 4 -- Death
- Custom Animation V3
- Start Frame: warblade_[direction]_clean.png | End Frame: bos | Frames: 6-8 | Keep First: OFF

```text
Greatsword drops from loosening grip -- right hand releases first, sword falls and clatters.
Body collapses forward: knees give, torso folds, arms fall limp to sides.
Head drops last. Full collapse over 6-8 frames, body settling into ground-level heap.
```

## ADIM 5 -- Walk Cycle (3-sub-step)
- 5a: Standalone Animate -> Start Frame: Characters/anchors/warblade/rotations/[direction].png -> 12 frames -> en uc poz sec -> PoseA_clean.png kaydet
- 5b: Aseprite'de PoseA'yi flipX -> PoseB_clean.png kaydet
- 5c: Custom Animation V3, Start=PoseA_clean.png, End=PoseB_clean.png, Frames: 6, Keep First: ON

```text
Heavy warrior walk cycle -- lead foot plants with weight dropping through the heel.
Greatsword stays held at right side, blade trailing low, both hands keeping loose control.
Torso stays upright with small shoulder sway. Hips shift over the planted foot, rear foot pushes forward, then body settles into the opposite stride.
```

## ADIM 6a -- Attack LMB (3-Segment)
- 6a-1: PEAK frame -- Custom Animation V3, Start=warblade_[direction]_clean.png, End=bos, Frames=4, Keep First=OFF -> son frame = PEAK_clean.png
- 6a-2: Windup -- Custom Animation V3, Start=warblade_[direction]_clean.png, End=PEAK_clean.png, Frames=4, Keep First=ON
- 6a-3: Follow -- Custom Animation V3, Start=PEAK_clean.png, End=warblade_[direction]_clean.png, Frames=4, Keep First=ON
- Toplam unique frames: 8 (PEAK paylasilir, sayilmaz 2x)

```text
WINDUP: Two-handed greatsword windup -- both hands grip hilt, right hand dominant at top.
Sword draws back and upward, rotating to position blade at upper-right: tip up at ~2 o'clock angle, arms pull right shoulder back.
Torso rotates clockwise 30-40 degrees, right shoulder pulls far back, hips pivot right.
Weight fully shifts to right foot. Frame 4 = maximum coil: blade at upper-right, body loaded.

FOLLOW-THROUGH: Release from coiled position -- both hands drive the greatsword in a wide horizontal sweep from right to left.
Blade arc: starts at 2 o'clock upper-right, sweeps through center at 9 o'clock, tip exits left silhouette fully extended.
Body uncoils counter-clockwise: right shoulder drives forward, left foot becomes anchor, weight transfers left.
Arms fully extended at mid-swing (frame 2). Deceleration frames 3-4: arms pull back, body settles, sword lowers to ready.
```

## ADIM 6b -- Attack RMB (3-Segment)
- 6b-1: PEAK frame -- Custom Animation V3, Start=warblade_[direction]_clean.png, End=bos, Frames=4, Keep First=OFF -> son frame = PEAK_clean.png
- 6b-2: Windup -- Custom Animation V3, Start=warblade_[direction]_clean.png, End=PEAK_clean.png, Frames=4, Keep First=ON
- 6b-3: Follow -- Custom Animation V3, Start=PEAK_clean.png, End=warblade_[direction]_clean.png, Frames=4, Keep First=ON
- Toplam unique frames: 8 (PEAK paylasilir, sayilmaz 2x)

```text
WINDUP: Overhead slam setup -- both hands bring greatsword directly overhead with both arms extended up.
Grip: right hand above left, hilt at crown level, blade pointing straight up at 12 o'clock.
Body rises onto balls of feet, knees extend, torso leans slightly back to accommodate overhead reach.
Core engages to stabilize maximum overhead extension. Frame 4 = peak: arms fully up, sword vertical.

FOLLOW-THROUGH: From slammed position -- sword tip at feet, both hands at chest level having driven through.
Begin recovery: arms absorb impact force, elbows bend to bring hilt up toward waist.
Knees re-bend to absorb body weight momentum. Torso rolls back upright over frames 2-3.
Frame 4: sword lifts off ground, body returns to upright ready stance.
```

## ADIM 6c -- Dash
- Custom Animation V3
- Start Frame: warblade_[direction]_clean.png | End Frame: bos | Frames: 4 | Keep First: ON

```text
Explosive forward lunge -- lead foot drives hard into ground, body pitches forward at 30 degree lean.
Greatsword tucked close to right side, both hands maintain grip, blade trailing horizontal.
Arms press sword back to reduce air resistance. Body fully horizontal at frame 2-3. Recovery: feet catch, stance widens, sword raises back to ready.
```

## ADIM 7 -- Weapon Pass
- Edit Image Pro -> weapon layer uzerinde calis
- Silahi dogru tutma pozisyonuna getir / detaylandir
- Her frame icin uygula

```text
Add greatsword on right shoulder, two-handed grip when raised. Sword: 3.5 head-tall blade, steel #6E7280 / #8A8E98 / #A6AAB4, hilt wrapped leather #3A2818, crossguard iron #282830. Cold blue cloth wrap on hilt (#7BA7BC). NO glow, NO embedded VFX. Apply per direction: S, E, N (W = flip E with sword in correct hand — no separate weapon paint).
```

## QC CHECKLIST
- [ ] Tum animasyonlar warblade_[direction]_clean.png start frame kullandi (anchor: Characters/anchors/warblade/rotations/[direction].png)
- [ ] Custom Animation V3 disinda tool kullanilmadi
- [ ] Keep First degerleri dogru (Idle/Hurt/Walk/Attack windup+follow=ON, Death/PEAK=OFF)
- [ ] Frame sayilari: Idle=6-8, Hurt=4, Death=6-8, Walk=6, Attack segment=4+4+4=8 unique, Dash=4
- [ ] Accent cold blue #7BA7BC korundu
- [ ] Greatsword weapon pass uygulandi
- [ ] S/E/N uretildi, W Unity flipX ile kullanilacak
- [ ] No embedded glow / VFX

## KAYIT KLASORU
```text
outputs/warblade/
  idle/ | hurt/ | death/ | walk/ | attack_lmb/ | attack_rmb/ | dash/
  weapon/
```
