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

## Anchor Referans (Adim 1 -- Baslangic Noktasi)

Weaponless base body URETME. Bu sinifin standing pose anchor'i zaten hazir:

  Characters/anchors/warblade/rotations/south.png
  Characters/anchors/warblade/rotations/south-east.png
  Characters/anchors/warblade/rotations/east.png
  Characters/anchors/warblade/rotations/north-east.png
  Characters/anchors/warblade/rotations/north.png
  Characters/anchors/warblade/rotations/north-west.png
  Characters/anchors/warblade/rotations/west.png
  Characters/anchors/warblade/rotations/south-west.png

Bu dosyalar silahini tutan, duran poz karakteri icerir -- temiz arka plan, uretim hazir.
Tum animasyonlarin start frame kaynagi bu anchor dosyalaridir.

Edit Image Pro kullanim durumlari:
- Anchor'dan uretilemayan asiri pozlar icin (walk extreme pose A/B, attack windup) use anchor as source image in Edit Image Pro to produce variant
- Animasyon start frame ve end frame'i yeni uretimden gelecekse: Edit Image Pro + anchor source
- Weapon zaten anchor'da var; ayri weapon pass GEREKMIYOR

## Idle (Adım 2)

```
Subtle breathing motion, 6-8 frames. Character chest rises and falls slowly, weight shifts subtly between feet. Same pose as base sprite, greatsword held ready on right shoulder. South-facing.
```

## Hurt (Adım 2)

```
Flinch backwards, 3 frames. Character's torso jerks back from impact, head tilts away, greatsword held ready on right shoulder. Cold blue accent (#7BA7BC) flickers slightly. Frame 1: idle pose. Frame 2: peak flinch (max backward lean). Frame 3: recovery toward idle.
```

## Death (Adım 2)

```
Collapse to ground, 6 frames. Heavy character falls forward to knees then face-down. greatsword held ready on right shoulder. Frame 1: stagger. Frame 2: knees buckle. Frame 3: kneeling. Frame 4: torso falls forward. Frame 5: arm catches ground. Frame 6: prone, motionless.
```

## Walk (Adım 3, Brian's Extreme Pose)

**Extreme Pose A prompt:**
```
Walking forward, right leg fully extended in stride, weight shifted to front foot, arms swing in counter-rhythm, body lean slight forward. Heavy warrior gait, greatsword held ready on right shoulder. South-facing.
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
Quick forward lunge, 4 frames. Frame 1: anticipation crouch (knees bent). Frame 2: explosive forward push, leading leg extended, body horizontal. Frame 3: airborne mid-dash, arms back. Frame 4: landing crouch, recovery. greatsword held ready on right shoulder.
```

## Edit Image Pro -- Yeni Frame Uretimi (Adim 5, ihtiyac varsa)

Anchor standing pose yeterli degilse (attack PEAK, walk extreme) yeni frame uret:
1. Source image: ilgili yon anchor dosyasi (Characters/anchors/warblade/rotations/south.png)
2. Prompt: istenen poz degisikligini describe et, weapon zaten source'da gorunur
3. Output: direction_variant.png olarak kaydet, eraser pass uygula

Weapon ayrica ekleme -- anchor'da zaten var.

## Notes

- **Faz 1 priority** — Yasin tek sınıf P0 → P1 → P2 sırasıyla bitirecek
- Diğer 9 sınıfın style anchor'ı bu olacak (idle S sprite kullan)
- BasicAttackProfile_Warblade.asset = Melee strategy
