# Ranger — Class-Specific Prompts
*Faz 2, asimetrik (4 yön ayrı)*

## Class Identity

| Field | Value |
|---|---|
| Type | **Asimetrik** (S, E, N, W ayrı üret — yay tek elde) |
| Weapon | Compound bow (sol elde tutulur, sağ el çekiş) |
| Accent Color | **Cold blue #7BA7BC** (Warblade ile aynı) |
| Yasak | Mor renk, void energy |
| Basic Attack | ShotCadence (range bands, <5 tile -%15 / 5+ tile +%15+crit) |
| RMB | Aim Shot (TAP-MODE, 2-stage placeable) |
| Silhouette | İnce, çevik, hood + cape, sırt sadağı |

## Anchor Referans (Adim 1 -- Baslangic Noktasi)

Weaponless base body URETME. Bu sinifin standing pose anchor'i zaten hazir:

  Characters/anchors/ranger/rotations/south.png
  Characters/anchors/ranger/rotations/south-east.png
  Characters/anchors/ranger/rotations/east.png
  Characters/anchors/ranger/rotations/north-east.png
  Characters/anchors/ranger/rotations/north.png
  Characters/anchors/ranger/rotations/north-west.png
  Characters/anchors/ranger/rotations/west.png
  Characters/anchors/ranger/rotations/south-west.png

Bu dosyalar silahini tutan, duran poz karakteri icerir -- temiz arka plan, uretim hazir.
Tum animasyonlarin start frame kaynagi bu anchor dosyalaridir.

Edit Image Pro kullanim durumlari:
- Anchor'dan uretilemayan asiri pozlar icin (walk extreme pose A/B, attack windup) use anchor as source image in Edit Image Pro to produce variant
- Animasyon start frame ve end frame'i yeni uretimden gelecekse: Edit Image Pro + anchor source
- Weapon zaten anchor'da var; ayri weapon pass GEREKMIYOR

## Idle (Adım 2)

```
Alert breathing, 6-8 frames. Hood slightly sways, head subtly scans, weight subtly shifts. Tense posture but relaxed shoulders. compound bow held in left hand, right hand relaxed at side.
```

## Hurt (Adım 2)

```
Flinch sideways, 3 frames. Light agile recoil — body twists rather than falls back. Cape flares from motion. Frame 1: idle. Frame 2: peak twist (45° body turn). Frame 3: recovery.
```

## Death (Adım 2)

```
Collapse sideways, 6 frames. Light body falls to one side, hood slips off head. compound bow held in left hand, right hand relaxed at side. Frame 1: stagger. Frame 2-3: knees buckle and lean. Frame 4: hand catches ground. Frame 5: lying on side. Frame 6: motionless, hood on ground.
```

## Walk (Adım 3, Extreme Pose)

**Extreme Pose A:**
```
Walking forward light-footed, right leg extended in stride, body lean very slight forward, cape sways behind. Quick agile gait. compound bow held in left hand, right hand relaxed at side. South-facing.
```

## Attack_LMB — Bow Shot (Adım 4, 3-segment)

**PEAK frame:**
```
Bow drawn fully, left arm extended forward holding bow, right hand at cheek anchor, arrow knocked. Body twisted 45° (asymmetric stance), bow vertical. Cold blue accent (#7BA7BC) glints on bowstring. Full draw commitment.
```

**START → PEAK:** 4 frame (bow raised, drawing motion)
**PEAK → END:** 4 frame (release — string snaps forward, recovery to ready)

## Attack_RMB — Aim Shot 2-Stage (Adım 4, 3-segment)

**PEAK frame:**
```
Slow aim — bow at full draw with extra time, breath held, body very still and centered. Arrow tip glows faintly cold blue (#7BA7BC charge). Pose more deliberate than LMB.
```

**START → PEAK:** 4 frame (slow draw, settling stance)
**PEAK → END:** 4 frame (release with stronger recoil — Aim Shot heavier projectile)

## Dash (Adım 4, 4 frame)

```
Quick agile roll, 4 frames. Frame 1: crouch. Frame 2: forward dive low to ground, body horizontal. Frame 3: tucked roll mid-air. Frame 4: emerge upright with bow ready. compound bow held in left hand, right hand relaxed at side (bow held in left hand throughout).
```

## Edit Image Pro -- Yeni Frame Uretimi (Adim 5, ihtiyac varsa)

Anchor standing pose yeterli degilse (attack PEAK, walk extreme) yeni frame uret:
1. Source image: ilgili yon anchor dosyasi (Characters/anchors/ranger/rotations/south.png)
2. Prompt: istenen poz degisikligini describe et, weapon zaten source'da gorunur
3. Output: direction_variant.png olarak kaydet, eraser pass uygula

Weapon ayrica ekleme -- anchor'da zaten var.

## Notes

- **Asimetrik** = 4 yön ayrı üret, ~%33 daha fazla call vs Warblade
- BasicAttackProfile_Ranger.asset = ShotCadence strategy
- Range bands mekaniği gameplay-side (anim aynı)
