# Shadowblade — Animasyon Uretim Rehberi
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR
- Tool: Custom Animation V3 (karakter sayfasi -> Add Animation -> Custom Animation V3)
- YASAK: Standalone Animate with Text NEW | animate_character MCP | Preset butonlar
- Start Frame: HER ZAMAN _clean.png (Eraser Pass sonrasi, PixelLab orijinalini kullanma)
- Yonler: Asimetrik -> 4 yon (8 directions Create Character)
- Canvas: 252x252 (v3 otomatik)

---

## ERASER PASS (ZORUNLU -- her base sprite uretiminden sonra)
1. Pixelorama'da ac: Characters/anchors/shadowblade/rotations/[direction].png
2. Eraser tool -> arka plan piksellerini temizle (anti-alias kenarlar dahil)
3. Kaydet: shadowblade_[direction]_clean.png
4. BU DOSYAYI KULLAN -- PixelLab orijinalini ASLA start frame olarak koyma

---

## ADIM 1 -- 8 Yon Base Sprite
Asimetrik -> 4 yon (S/E/N/W) uretilir. Create Character'da "8 Directions" sec.

- PixelLab -> Create Character Pro -> "8 Directions" sec
- Her yon icin Eraser Pass uygula -> _clean.png kaydet

```text
Pixel art shadowblade assassin character, body-only, no weapon, character occupies ~50% of canvas height (~128px tall) centered on a 252x252 transparent canvas. Wide transparent padding on all sides for animation headroom — DO NOT fill the canvas. High top-down view 30-35°. Slim agile build, full dark hooded cloak, body almost entirely silhouetted in dark with violet undertones. Palette: cloak black-purple #1A0E2A / #2A1A3A, mid #3A2A4E, accent violet #5A2A8A, skin partial visible #C9A084 (only chin and jawline below hood), leather straps #3A2818. Crouched ready stance, body lean forward, weight on balls of feet. Hood deep — no eyes visible (silhouette only). NO weapon, NO embedded glow. [FACING S | E | N | W]. Hard pixel edges.
```

## ADIM 2 -- Idle
- Custom Animation V3
- Start Frame: shadowblade_[direction]_clean.png | End Frame: bos | Frames: 6-8 | Keep First: ON

```text
Low predatory stance -- knees bent, weight forward on balls of feet.
Left blade held low at hip pointing back, right blade raised near right shoulder pointing forward.
Subtle constant weight shift: rocking heel-to-toe, never fully still. Cloak or clothing ripples.
Head stays level -- eyes scanning, no large head movement.
```

## ADIM 3 -- Hurt
- Custom Animation V3
- Start Frame: shadowblade_[direction]_clean.png | End Frame: bos | Frames: 4 | Keep First: ON

```text
Both blades flinch inward as body recoils -- arms cross defensively in front of torso.
Body snaps backward, low crouch gets lower. Knees buckle, weight drops and shifts back.
Head tucks -- chin down, shoulders up in protective curl. Recovery frames 3-4: blades re-extend, crouch re-establishes.
```

## ADIM 4 -- Death
- Custom Animation V3
- Start Frame: shadowblade_[direction]_clean.png | End Frame: bos | Frames: 6-8 | Keep First: OFF

```text
Blades fall from both hands simultaneously -- fingers open, blades drop and clatter at feet.
Body loses its predatory crouch entirely: knees straighten briefly then give out completely.
Collapse is forward and down -- body pitches toward ground, face-down. Arms spread wide as body falls. 6-8 frames.
```

## ADIM 5 -- Walk Cycle (3-sub-step)
- 5a: Standalone Animate -> Start Frame: Characters/anchors/shadowblade/rotations/[direction].png -> 12 frames -> en uc poz sec -> PoseA_clean.png kaydet
- 5b: Aseprite'de PoseA'yi flipX -> PoseB_clean.png kaydet
- 5c: Custom Animation V3, Start=PoseA_clean.png, End=PoseB_clean.png, Frames: 6, Keep First: ON

```text
Silent predator walk -- body remains low, knees bent, weight on balls of feet.
Left blade stays low at hip pointing back; right blade hovers near shoulder, tip forward.
Feet place softly with short stride distance. Cloak trails behind with restrained sway, head level, torso compressed and ready.
```

## ADIM 6a -- Attack LMB (3-Segment)
- 6a-1: PEAK frame -- Custom Animation V3, Start=shadowblade_[direction]_clean.png, End=bos, Frames=4, Keep First=OFF -> son frame = PEAK_clean.png
- 6a-2: Windup -- Custom Animation V3, Start=shadowblade_[direction]_clean.png, End=PEAK_clean.png, Frames=4, Keep First=ON
- 6a-3: Follow -- Custom Animation V3, Start=PEAK_clean.png, End=shadowblade_[direction]_clean.png, Frames=4, Keep First=ON
- Toplam unique frames: 8 (PEAK paylasilir, sayilmaz 2x)

```text
WINDUP: Right blade draws back for horizontal slash -- right arm pulls elbow back to hip level, blade pointing backward at 8 o'clock.
Left blade stays forward and low as guard, left elbow bent at 45 degrees.
Body coils: right shoulder pulls hard back, hips rotate right, weight loads onto right foot.
Low aggressive crouch deepens slightly. Frame 4: right blade cocked at maximum reach-back, left guard blade held steady.

FOLLOW-THROUGH: Right blade releases in fast horizontal slash -- sweeps from 8 o'clock low-right across body to 10 o'clock left-center.
Blade arc is tight and flat -- elbow-driven, not shoulder-driven, keeping strike fast.
Body uncoils explosively: right shoulder drives forward, right foot pushes off.
Left blade simultaneously pulls back to hip for follow-up readiness. Frame 3: full extension, right arm across body.
```

## ADIM 6b -- Attack RMB (3-Segment)
- 6b-1: PEAK frame -- Custom Animation V3, Start=shadowblade_[direction]_clean.png, End=bos, Frames=4, Keep First=OFF -> son frame = PEAK_clean.png
- 6b-2: Windup -- Custom Animation V3, Start=shadowblade_[direction]_clean.png, End=PEAK_clean.png, Frames=4, Keep First=ON
- 6b-3: Follow -- Custom Animation V3, Start=PEAK_clean.png, End=shadowblade_[direction]_clean.png, Frames=4, Keep First=ON
- Toplam unique frames: 8 (PEAK paylasilir, sayilmaz 2x)

```text
WINDUP: Left blade coils for forward thrust -- left shoulder rotates back, left arm bends at elbow, blade points rearward at 4 o'clock.
Right blade sweeps forward as decoy/guard distraction, arm extended toward target.
Hips rotate: left hip pulls back loading the thrust, weight transfers to right foot.
Frame 4 = maximum coil: left blade fully drawn back, right blade extended forward, body in deep rotation.

FOLLOW-THROUGH: Left blade drives forward in straight-line thrust -- arm extends from bent elbow directly toward target.
Blade path: from 4 o'clock rearward to 12 o'clock forward, tip leads the motion throughout.
Body uncoils: left hip drives forward, left shoulder snaps through, weight shifts hard to left foot.
Right blade pulls back as counterbalance. Frame 2: full extension, left arm straight, tip at maximum reach.
```

## ADIM 6c -- Dash
- Custom Animation V3
- Start Frame: shadowblade_[direction]_clean.png | End Frame: bos | Frames: 4 | Keep First: ON

```text
Shadow-step burst -- body vanishes into low sprint crouch, legs drive hard.
Both blades pulled tight to body: left blade along left forearm pointing back, right blade along right forearm pointing forward.
No wasted arm motion -- full tuck position to minimize silhouette. Feet barely leave ground. Frame 2: full horizontal body lean.
```

## ADIM 7 -- Weapon Pass
- Edit Image Pro -> weapon layer uzerinde calis
- Silahi dogru tutma pozisyonuna getir / detaylandir
- Her frame icin uygula

```text
Add twin short blades — one per hand. Each blade: ~0.6x character height, narrow silhouette, dark steel #4A4E5A / #5C6070, hilt wrapped black-violet (#1A0E2A / #5A2A8A). Curved or straight short-sword profile. Apply per direction: S, E, N, W (each painted separately).
```

## QC CHECKLIST
- [ ] Tum animasyonlar shadowblade_[direction]_clean.png start frame kullandi (anchor: Characters/anchors/shadowblade/rotations/[direction].png)
- [ ] Custom Animation V3 disinda tool kullanilmadi
- [ ] Keep First degerleri dogru (Idle/Hurt/Walk/Attack windup+follow=ON, Death/PEAK=OFF)
- [ ] Frame sayilari: Idle=6-8, Hurt=4, Death=6-8, Walk=6, Attack segment=4+4+4=8 unique, Dash=4
- [ ] Accent void purple #5A2A8A korundu
- [ ] Twin short blades weapon pass uygulandi
- [ ] S/E/N/W yonleri ayri uretildi
- [ ] Embedded glow karakter sprite'inda yok

## KAYIT KLASORU
```text
outputs/shadowblade/
  idle/ | hurt/ | death/ | walk/ | attack_lmb/ | attack_rmb/ | dash/
  weapon/
```
