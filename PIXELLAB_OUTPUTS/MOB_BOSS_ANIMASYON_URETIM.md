# MOB & BOSS -- Animasyon Uretim Rehberi
*MEMORY/pixellab_master_pipeline.md Bolum 0 ve Bolum 14 HARD RULES*

---

## TEMEL FARK: Player siniflarindan farkli -- mob/boss PixelLab'de karakter olarak degil, referans sprite olarak baslar

---

## ADIM 0 -- Sisteme Al (ZORUNLU -- mob/boss'ta bu adim VAR, player siniflarinda YOK)
1. Base sprite uret: Create Image PRO -> hedef canvas boyutunda (boyut tablosuna bak)
2. Eraser Pass -> _clean.png
3. PixelLab -> Create Character -> "Create from Reference V3"
   - Input: _clean.png upload
   - Directions: 8 sec
   - Karakter artik sistemde -> buradan sonra player ile ayni pipeline

## MOB ANIMASYON SETI (LOCKED)
| Tier | Animasyonlar | Adet |
|------|-------------|------|
| Standart Mob | Walk / Attack / Hurt / Death | 4 |
| Elite Mob | Walk / Attack / Hurt / Death / Telegraph | 5 |
| Boss | Idle / Walk / Attack_1 / Attack_2 / Telegraph / Hurt / Death / Phase_Transition | 8 |

## BOYUT & PPU TABLOSU (LOCKED)
| Boss tipi | PixelLab üretim | Unity PPU | Unity ekran | Player'a oran |
|---|---|---|---|---|
| Player (referans) | 252×252, karakter ≈128px | 64 | ~2.0 unit | 1× |
| Miniboss | 252×252, karakter ≈128-160px | 128 | ~1.0 unit görsel ama büyük gözükür | 2× hissi |
| Act Boss | 252×252, karakter ≈160px | 64 | ~2.5 unit | ~3-4× hissi |
| **Final Boss** | **252×252** (ASLA 512+!) | **32** | ~5 unit | **~6× hissi (devasa)** |
| Architect canavar form | 256×256 | 32 | ~8 unit | LOCKED (memory) |

## FRAME SAYILARI
- Hurt: 4 frames
- Walk: 6 frames
- Attack (3-segment): 4+4+4 -> 8 unique frames (PEAK paylasilir)
- Death: 6-8 frames
- Idle (boss): 6-8 frames
- Telegraph: 4-6 frames
- Phase_Transition: 8-12 frames

## ANIMASYON ADIMLARI
Same Custom Animation V3 pipeline as player classes -- Start Frame = _clean.png ZORUNLU.
Use same 3-segment Attack structure (PEAK -> Windup -> Follow).
Keep First rules same as player.

```text
GENERIC ATTACK WINDUP TEMPLATE:
[Weapon/limb type] draws back for attack -- [starting position, angle, grip if applicable].
Body coils: [rotation direction, shoulder/hip drive, weight shift to which foot].
[Non-attacking limb position and role]. Frame 4 = peak: [exact peak position description with angle/clock reference].

GENERIC ATTACK FOLLOW-THROUGH TEMPLATE:
[Weapon/limb] releases from [peak position] -- [movement arc description with clock references].
Body uncoils: [which body part drives, rotation direction, weight transfer].
[Non-attacking limb counterbalance role]. Frame 2-3: [full extension moment]. Frame 4: [deceleration/recovery description].

FINAL BOSS EXAMPLE (stone fist slam):
WINDUP: Massive left stone fist raises overhead -- arm extends up and slightly right, elbow bends 30 degrees.
Right blade arm extends forward as counterbalance. Body rises: legs straighten, torso tilts back.
Shoulders tilt right side up. Weight on left foot. Frame 4: fist at maximum overhead reach, full body lean right.

FOLLOW-THROUGH: Left fist drives straight down -- arm straightens fully, fist descends to ground level.
Impact: body weight drops with the strike, knees bend absorbing force. Ground crack VFX placement zone = center of fist path.
Right arm pulls back to balance. Frame 2: fist at impact. Frames 3-4: arm begins to lift, body recovers upright.
```

## QC CHECKLIST
- [ ] Adim 0 yapildi (Create from Reference V3)
- [ ] Eraser Pass her base sprite icin
- [ ] Boyut LOCKED tablodan kontrol edildi
- [ ] Animasyon set tier'a gore dogru (4/5/8 anim)
- [ ] Custom Animation V3 kullanildi (Standalone NEW YASAK)

## KAYIT KLASORU
```text
outputs/mobs/[mob_name]/
  walk/ | attack/ | hurt/ | death/ | telegraph/ (elite+boss)
outputs/bosses/[boss_name]/
  idle/ | walk/ | attack_1/ | attack_2/ | telegraph/ | hurt/ | death/ | phase_transition/
```
