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

## Base Body Prompt (Adım 1, body-only, silahsız)

```
Pixel art ranger character, body-only, no weapon, 128x128 sprite on 252x252 canvas. High top-down view 30-35°. Lean agile build, hooded cloak, cold blue undertunic (#7BA7BC), forest green cloak (#3A4A38 / #4E5E48). Quiver visible on back (leather strap). Palette: cloak green #3A4A38 / #4E5E48, leather #3A2818 / #5A4028, accent blue #7BA7BC, skin #C9A084. Light leather armor, flexible stance, feet hip-width. Hood up, partial face shadow. NO weapon held. South-facing default. Hard pixel edges, no anti-aliasing.
```

Yönler: **S, E, N, W** dördünü ayrı üret (asimetrik — yay tek elde, flip silahı yanlış ele atar).

## Idle (Adım 2)

```
Alert breathing, 6-8 frames. Hood slightly sways, head subtly scans, weight subtly shifts. Tense posture but relaxed shoulders. No weapon.
```

## Hurt (Adım 2)

```
Flinch sideways, 3 frames. Light agile recoil — body twists rather than falls back. Cape flares from motion. Frame 1: idle. Frame 2: peak twist (45° body turn). Frame 3: recovery.
```

## Death (Adım 2)

```
Collapse sideways, 6 frames. Light body falls to one side, hood slips off head. No weapon. Frame 1: stagger. Frame 2-3: knees buckle and lean. Frame 4: hand catches ground. Frame 5: lying on side. Frame 6: motionless, hood on ground.
```

## Walk (Adım 3, Extreme Pose)

**Extreme Pose A:**
```
Walking forward light-footed, right leg extended in stride, body lean very slight forward, cape sways behind. Quick agile gait. No weapon. South-facing.
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
Quick agile roll, 4 frames. Frame 1: crouch. Frame 2: forward dive low to ground, body horizontal. Frame 3: tucked roll mid-air. Frame 4: emerge upright with bow ready. No weapon (or bow held in left hand throughout — implementation choice).
```

## Weapon Pass (Adım 5, Edit Image Pro)

```
Add compound bow held in LEFT hand. Bow: vertical orientation when at rest, ~1.2x character height. Wood riser #5A4028 / #7A5838, limbs darker #3A2818, string thin off-white #C8C0A8. Cold blue grip wrap (#7BA7BC). Quiver on back already in base sprite. Apply per direction: S, E, N, W (each painted separately — flip changes weapon hand).
```

## Notes

- **Asimetrik** = 4 yön ayrı üret, ~%33 daha fazla call vs Warblade
- BasicAttackProfile_Ranger.asset = ShotCadence strategy
- Range bands mekaniği gameplay-side (anim aynı)
