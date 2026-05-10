# Warblade — Class-Specific Prompts
*Faz 1 referans sınıfı — diğer sınıflar için style anchor*

## Class Identity

| Field | Value |
|---|---|
| Type | **Simetrik** (3 yön üret + W = E flipX) |
| Weapon | Greatsword (two-handed, ortalanmış) |
| Accent Color | **Cold blue #7BA7BC** |
| Yasak | El glow, mor renk, void energy |
| Basic Attack | Melee chain (3-hit combo) |
| RMB | Heavy slam (Verdict Ledger stack) |
| Silhouette | Geniş omuz, ağır zırh, iki elli kılıç |

## Base Body Prompt (Adım 1, body-only, silahsız)

```
Pixel art warrior character, body-only, no weapon, 128x128 sprite on 252x252 canvas. High top-down view 30-35° elevation. Heavy plate armor, broad shoulders, cold blue cloth accent #7BA7BC at sash and shoulder straps. Palette: armor steel #4A4E5A / #5C6070 / #6E7280, accent blue #7BA7BC, leather #3A2818 / #5A4028, skin #C9A084 / #A07858, hair dark brown. Stoic stance, feet shoulder-width, arms relaxed. Hard pixel edges, no anti-aliasing, pixel cluster min 4px. NO embedded glow, NO VFX, NO weapon. South-facing default (face camera).
```

Yönler: **S, E, N** üret. **W = Unity flipX** (üretme!)

## Idle (Adım 2)

```
Subtle breathing motion, 6-8 frames. Character chest rises and falls slowly, weight shifts subtly between feet. Same pose as base sprite, no weapon. South-facing.
```

## Hurt (Adım 2)

```
Flinch backwards, 3 frames. Character's torso jerks back from impact, head tilts away, no weapon. Cold blue accent (#7BA7BC) flickers slightly. Frame 1: idle pose. Frame 2: peak flinch (max backward lean). Frame 3: recovery toward idle.
```

## Death (Adım 2)

```
Collapse to ground, 6 frames. Heavy character falls forward to knees then face-down. No weapon. Frame 1: stagger. Frame 2: knees buckle. Frame 3: kneeling. Frame 4: torso falls forward. Frame 5: arm catches ground. Frame 6: prone, motionless.
```

## Walk (Adım 3, Brian's Extreme Pose)

**Extreme Pose A prompt:**
```
Walking forward, right leg fully extended in stride, weight shifted to front foot, arms swing in counter-rhythm, body lean slight forward. Heavy warrior gait, no weapon. South-facing.
```

Pose A → flip → Pose B → Interpolate 4-6 frames arası.

## Attack_LMB — Greatsword Chain (Adım 4, 3-segment)

**PEAK frame:**
```
Greatsword horizontal slash at full extension, arms parallel to ground, sword tip past character silhouette right edge. Body twisted 30° to follow slash, weight on back foot, full commitment. Cold blue accent flickers at sword wake (#7BA7BC).
```

**START → PEAK:** 4 frame windup (sword raised over right shoulder → unwind into slash)
**PEAK → END:** 4 frame follow-through (sword crosses to left side → returns to ready stance)

## Attack_RMB — Heavy Slam (Adım 4, 3-segment)

**PEAK frame:**
```
Greatsword slammed into ground, both hands gripping hilt at chest level, blade vertical with tip at character's feet. Body fully forward, knees bent, weight committed downward. Impact frame — peak commitment.
```

**START → PEAK:** 4 frame (sword raised overhead, max windup)
**PEAK → END:** 4 frame (recovery, pull sword from ground, return to ready)

## Dash (Adım 4, 4 frame)

```
Quick forward lunge, 4 frames. Frame 1: anticipation crouch (knees bent). Frame 2: explosive forward push, leading leg extended, body horizontal. Frame 3: airborne mid-dash, arms back. Frame 4: landing crouch, recovery. No weapon.
```

## Weapon Pass (Adım 5, Edit Image Pro)

```
Add greatsword on right shoulder, two-handed grip when raised. Sword: 3.5 head-tall blade, steel #6E7280 / #8A8E98 / #A6AAB4, hilt wrapped leather #3A2818, crossguard iron #282830. Cold blue cloth wrap on hilt (#7BA7BC). NO glow, NO embedded VFX. Apply per direction: S, E, N (W = flip E with sword in correct hand — no separate weapon paint).
```

## Notes

- **Faz 1 priority** — Yasin tek sınıf P0 → P1 → P2 sırasıyla bitirecek
- Diğer 9 sınıfın style anchor'ı bu olacak (idle S sprite kullan)
- BasicAttackProfile_Warblade.asset = Melee strategy
