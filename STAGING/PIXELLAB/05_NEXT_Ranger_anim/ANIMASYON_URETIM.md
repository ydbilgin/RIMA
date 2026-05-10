# Ranger — Animasyon Uretim Rehberi
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
1. Pixelorama'da ac: Characters/anchors/ranger/rotations/[direction].png
2. Eraser tool -> arka plan piksellerini temizle (anti-alias kenarlar dahil)
3. Kaydet: ranger_[direction]_clean.png
4. BU DOSYAYI KULLAN -- PixelLab orijinalini ASLA start frame olarak koyma

---

## ADIM 1 -- 8 Yon Base Sprite
Asimetrik -> 4 yon (S/E/N/W) uretilir. Create Character'da "8 Directions" sec.

- PixelLab -> Create Character Pro -> "8 Directions" sec
- Her yon icin Eraser Pass uygula -> _clean.png kaydet

```text
Pixel art ranger character, body-only, no weapon, character occupies ~50% of canvas height (~128px tall) centered on a 252x252 transparent canvas. Wide transparent padding on all sides for animation headroom — DO NOT fill the canvas. High top-down view 30-35°. Lean agile build, hooded cloak, cold blue undertunic (#7BA7BC), forest green cloak (#3A4A38 / #4E5E48). Quiver visible on back (leather strap). Palette: cloak green #3A4A38 / #4E5E48, leather #3A2818 / #5A4028, accent blue #7BA7BC, skin #C9A084. Light leather armor, flexible stance, feet hip-width. Hood up, partial face shadow. NO weapon held. [FACING S | E | N | W]. Hard pixel edges, no anti-aliasing.
```

## ADIM 2 -- Idle
- Custom Animation V3
- Start Frame: ranger_[direction]_clean.png | End Frame: bos | Frames: 6-8 | Keep First: ON

```text
Relaxed but alert -- weight on right foot, left foot slightly forward.
Bow held in left hand at side, lower limb near ground, string loose.
Subtle torso sway 5-10 degrees side to side. Right hand rests near quiver or hangs relaxed.
Head scans slowly left then right. Breathing visible in chest rise/fall.
```

## ADIM 3 -- Hurt
- Custom Animation V3
- Start Frame: ranger_[direction]_clean.png | End Frame: bos | Frames: 4 | Keep First: ON

```text
Bow flinches upward as body recoils -- left arm raises bow defensively, right arm pulls back.
Torso twists away from hit direction, weight shifts hard to back foot.
Head ducks and turns. Recovery over frames 3-4: body straightens, bow lowers back to side.
```

## ADIM 4 -- Death
- Custom Animation V3
- Start Frame: ranger_[direction]_clean.png | End Frame: bos | Frames: 6-8 | Keep First: OFF

```text
Bow slips from left hand -- fingers open, bow falls forward and to ground.
Body lists to one side, right hand reaches out reflexively then falls limp.
Legs buckle, collapse is slow and heavy -- no dramatic fall, just a weight-giving-out descent. 6-8 frames total.
```

## ADIM 5 -- Walk Cycle (3-sub-step)
- 5a: Standalone Animate -> Start Frame: Characters/anchors/ranger/rotations/[direction].png -> 12 frames -> en uc poz sec -> PoseA_clean.png kaydet
- 5b: Aseprite'de PoseA'yi flipX -> PoseB_clean.png kaydet
- 5c: Custom Animation V3, Start=PoseA_clean.png, End=PoseB_clean.png, Frames: 6, Keep First: ON

```text
Light-footed forward walk -- left foot places softly before weight transfers.
Bow remains in left hand at side, lower limb near ground, string loose and controlled.
Right hand stays near quiver or relaxed by hip. Cape sways behind with each step, torso lean stays slight, head continues scanning.
```

## ADIM 6a -- Attack LMB (3-Segment)
- 6a-1: PEAK frame -- Custom Animation V3, Start=ranger_[direction]_clean.png, End=bos, Frames=4, Keep First=OFF -> son frame = PEAK_clean.png
- 6a-2: Windup -- Custom Animation V3, Start=ranger_[direction]_clean.png, End=PEAK_clean.png, Frames=4, Keep First=ON
- 6a-3: Follow -- Custom Animation V3, Start=PEAK_clean.png, End=ranger_[direction]_clean.png, Frames=4, Keep First=ON
- Toplam unique frames: 8 (PEAK paylasilir, sayilmaz 2x)

```text
WINDUP: Compound bow raised to shooting position -- left arm extends forward holding bow vertical, elbow slightly bent.
Right hand draws string back to right cheek anchor point.
Body turns 45 degrees to face right/south-right (shooting stance -- side-on to target).
Left shoulder extends forward, right elbow up at shoulder height. Frame 4 = full draw: arrow at cheek, string taut, body still.

RELEASE: Release from full draw -- right hand opens, string snaps forward, left arm straightens with slight recoil.
Bow arm (left) vibrates from string release, slight backward push.
Right hand follows through, arm extends slightly back from release point.
Body begins to relax from shooting stance, weight re-centers over frames 3-4.
```

## ADIM 6b -- Attack RMB (3-Segment)
- 6b-1: PEAK frame -- Custom Animation V3, Start=ranger_[direction]_clean.png, End=bos, Frames=4, Keep First=OFF -> son frame = PEAK_clean.png
- 6b-2: Windup -- Custom Animation V3, Start=ranger_[direction]_clean.png, End=PEAK_clean.png, Frames=4, Keep First=ON
- 6b-3: Follow -- Custom Animation V3, Start=PEAK_clean.png, End=ranger_[direction]_clean.png, Frames=4, Keep First=ON
- Toplam unique frames: 8 (PEAK paylasilir, sayilmaz 2x)

```text
WINDUP: Slow deliberate draw -- left arm raises bow with controlled, unhurried movement.
Right hand draws string gradually to right cheek anchor point.
Body settles into stillness mid-draw: shoulders square, breath held.
Minimal body movement in final 2 frames -- body visibly stabilized in hold.
Frame 6 = peak: arrow tip aimed, bow at full draw, body stone still, right elbow high.

RELEASE: Controlled precision release -- right hand opens cleanly, string exits with minimal hand tremor.
Bow arm holds steady for 1 frame after release (follow-through discipline).
Right hand retracts slowly to quiver in deliberate follow-through.
Body unwinds from shooting stance gradually -- weight shifts back to neutral over frames 3-4.
```

## ADIM 6c -- Dash
- Custom Animation V3
- Start Frame: ranger_[direction]_clean.png | End Frame: bos | Frames: 4 | Keep First: ON

```text
Quick side-step or back-step -- body shifts direction fast, low center of gravity.
Bow stays in left hand, held close to body, string side tucked in.
Feet rapid-step: lead foot plants, trail foot follows. Minimal arm swing -- bow arm stays controlled throughout.
```

## ADIM 7 -- Weapon Pass
- Edit Image Pro -> weapon layer uzerinde calis
- Silahi dogru tutma pozisyonuna getir / detaylandir
- Her frame icin uygula

```text
Add compound bow held in LEFT hand. Bow: vertical orientation when at rest, ~1.2x character height. Wood riser #5A4028 / #7A5838, limbs darker #3A2818, string thin off-white #C8C0A8. Cold blue grip wrap (#7BA7BC). Quiver on back already in base sprite. Apply per direction: S, E, N, W (each painted separately — flip changes weapon hand).
```

## QC CHECKLIST
- [ ] Tum animasyonlar ranger_[direction]_clean.png start frame kullandi (anchor: Characters/anchors/ranger/rotations/[direction].png)
- [ ] Custom Animation V3 disinda tool kullanilmadi
- [ ] Keep First degerleri dogru (Idle/Hurt/Walk/Attack windup+follow=ON, Death/PEAK=OFF)
- [ ] Frame sayilari: Idle=6-8, Hurt=4, Death=6-8, Walk=6, Attack segment=4+4+4=8 unique, Dash=4
- [ ] Accent cold blue #7BA7BC korundu
- [ ] Compound bow sol elde ve 4 yon ayri boyandi
- [ ] S/E/N/W yonleri ayri uretildi
- [ ] No embedded glow / VFX

## KAYIT KLASORU
```text
outputs/ranger/
  idle/ | hurt/ | death/ | walk/ | attack_lmb/ | attack_rmb/ | dash/
  weapon/
```
