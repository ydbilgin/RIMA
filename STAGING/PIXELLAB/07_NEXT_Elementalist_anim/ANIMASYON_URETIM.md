# Elementalist — Animasyon Uretim Rehberi
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
1. Pixelorama'da ac: Characters/anchors/elementalist/rotations/[direction].png
2. Eraser tool -> arka plan piksellerini temizle (anti-alias kenarlar dahil)
3. Kaydet: elementalist_[direction]_clean.png
4. BU DOSYAYI KULLAN -- PixelLab orijinalini ASLA start frame olarak koyma

---

## ADIM 1 -- 8 Yon Base Sprite
Asimetrik -> 4 yon (S/E/N/W) uretilir. Create Character'da "8 Directions" sec.

- PixelLab -> Create Character Pro -> "8 Directions" sec
- Her yon icin Eraser Pass uygula -> _clean.png kaydet

```text
Pixel art elementalist mage character, body-only, no weapon, NO book, NO staff (hands free for spell gestures), character occupies ~50% of canvas height (~128px tall) centered on a 252x252 transparent canvas. Wide transparent padding on all sides for animation headroom — DO NOT fill the canvas. High top-down view 30-35°. Long flowing robe, hood NOT up (face visible — confident mage), short hair. Robe palette: deep blue-grey #2A3848 / #3E4C5E / #525E74 (cool neutral default — element accent only on spell anims). Trim accent: faint cool #B8C8D0. Sash at waist #3A2818 leather. Skin #C9A084. Body pose: slightly forward, hands held at chest level, palms angled outward (ready to cast). Robe hem sways. NO weapon, NO held object. [FACING S | E | N | W]. Hard pixel edges, no anti-aliasing.
```

## ADIM 2 -- Idle
- Custom Animation V3
- Start Frame: elementalist_[direction]_clean.png | End Frame: bos | Frames: 6-8 | Keep First: ON

```text
Calm meditative float or stand -- weight centered, spine upright, shoulders relaxed and low.
Both hands hang at sides with fingers loosely curled, palms face inward toward body.
Subtle energy shimmer implied in micro finger movements -- fingertips occasionally extend then relax.
Palm zones (~12x12px each) kept clear for engine VFX overlay. Breathing slow and deep.
```

## ADIM 3 -- Hurt
- Custom Animation V3
- Start Frame: elementalist_[direction]_clean.png | End Frame: bos | Frames: 4 | Keep First: ON

```text
Both hands snap up reflexively -- defensive ward gesture, palms forward and outward.
Body steps back one pace hard, torso twists away from impact direction.
Head ducks slightly. Fingers splay wide in the ward position. Recovery frames 3-4: hands lower, body re-centers.
```

## ADIM 4 -- Death
- Custom Animation V3
- Start Frame: elementalist_[direction]_clean.png | End Frame: bos | Frames: 6-8 | Keep First: OFF

```text
Hands lose their energy -- fingers uncurl slowly, palms fall to face downward, arms drop.
Body sways once then folds: knees give, torso tips forward, arms trail behind.
No dramatic pose -- pure gravity taking over. Body settles into ground slowly, hands last to fall. 6-8 frames.
```

## ADIM 5 -- Walk Cycle (3-sub-step)
- 5a: Standalone Animate -> Start Frame: Characters/anchors/elementalist/rotations/[direction].png -> 12 frames -> en uc poz sec -> PoseA_clean.png kaydet
- 5b: Aseprite'de PoseA'yi flipX -> PoseB_clean.png kaydet
- 5c: Custom Animation V3, Start=PoseA_clean.png, End=PoseB_clean.png, Frames: 6, Keep First: ON

```text
Smooth deliberate mage walk -- front foot places first, weight rolls gently through the step.
Hands remain free at sides with fingers loosely curled, palms inward, palm zones unobstructed.
Robe trails behind and settles on each foot plant. Spine stays upright, shoulders low, head steady.
```

## ADIM 6a -- Attack LMB (3-Segment)
- 6a-1: PEAK frame -- Custom Animation V3, Start=elementalist_[direction]_clean.png, End=bos, Frames=4, Keep First=OFF -> son frame = PEAK_clean.png
- 6a-2: Windup -- Custom Animation V3, Start=elementalist_[direction]_clean.png, End=PEAK_clean.png, Frames=4, Keep First=ON
- 6a-3: Follow -- Custom Animation V3, Start=PEAK_clean.png, End=elementalist_[direction]_clean.png, Frames=4, Keep First=ON
- Toplam unique frames: 8 (PEAK paylasilir, sayilmaz 2x)

```text
WINDUP: Both hands draw back toward chest, palms rotating inward (gathering gesture).
Fingers curl slightly as if compressing energy between palms and chest center.
Elbows pull back, shoulders round forward into the gather. Body leans slightly forward in anticipation.
Frame 4 = peak gather: hands at chest center, palms facing each other ~15cm apart, energy compressed.
Palm zones (~12x12px each) intentionally left clear for engine VFX overlay throughout.

RELEASE: Both arms extend forward and outward from chest -- palms rotating to face forward (push gesture).
Elbows straighten, arms reach full extension, fingers spread open wide.
Body leans into the push: slight forward lean of torso follows arm extension.
Frame 2: arms fully extended, palms forward at shoulder height. Frames 3-4: body settles back, hands lower to sides.
```

## ADIM 6b -- Attack RMB (3-Segment)
- 6b-1: PEAK frame -- Custom Animation V3, Start=elementalist_[direction]_clean.png, End=bos, Frames=4, Keep First=OFF -> son frame = PEAK_clean.png
- 6b-2: Windup -- Custom Animation V3, Start=elementalist_[direction]_clean.png, End=PEAK_clean.png, Frames=4, Keep First=ON
- 6b-3: Follow -- Custom Animation V3, Start=PEAK_clean.png, End=elementalist_[direction]_clean.png, Frames=4, Keep First=ON
- Toplam unique frames: 8 (PEAK paylasilir, sayilmaz 2x)

```text
WINDUP: Right hand raises overhead, palm up -- slow deliberate raising motion, elbow bends then straightens as arm rises.
Left hand extends forward at waist height, palm down, acting as anchor/stabilizer.
Body straightens and rises slightly: weight shifts forward onto front foot, spine extends.
Frame 6 = peak charge: right arm fully overhead, palm cupped upward, left arm extended forward. Body maximally extended vertically.
Palm zones clear throughout for engine VFX placement.

RELEASE: Right arm drives downward in arc from overhead -- elbow leads the drive, hand follows through.
Palm flips from facing up to facing forward-down as arm descends.
Left hand simultaneously pulls back to hip as counterbalance to the release force.
Frame 2: right arm at waist-level mid-arc. Frames 3-4: arm settles at side, body re-centers.
```

## ADIM 6c -- Dash
- Custom Animation V3
- Start Frame: elementalist_[direction]_clean.png | End Frame: bos | Frames: 4 | Keep First: ON

```text
Quick stride or glide forward -- body leans 20-25 degrees forward, legs drive.
Both hands pull back along sides, elbows bent, palms trailing (aerodynamic tuck).
No spell casting during dash -- hands in neutral closed position, fingers relaxed.
Frame 2: maximum forward lean. Frame 4: feet plant, body decelerates back to upright.
```

## QC CHECKLIST
- [ ] Tum animasyonlar elementalist_[direction]_clean.png start frame kullandi (anchor: Characters/anchors/elementalist/rotations/[direction].png)
- [ ] Custom Animation V3 disinda tool kullanilmadi
- [ ] Keep First degerleri dogru (Idle/Hurt/Walk/Attack windup+follow=ON, Death/PEAK=OFF)
- [ ] Frame sayilari: Idle=6-8, Hurt=4, Death=6-8, Walk=6, Attack segment=4+4+4=8 unique, Dash=4
- [ ] Cool neutral robe palette korundu (#2A3848 / #3E4C5E / #525E74)
- [ ] Silah, kitap, staff yok; el jestleri kullanildi
- [ ] Element rengi karakter sprite'ina gomulmedi
- [ ] S/E/N/W yonleri ayri uretildi

## KAYIT KLASORU
```text
outputs/elementalist/
  idle/ | hurt/ | death/ | walk/ | attack_lmb/ | attack_rmb/ | dash/
```
