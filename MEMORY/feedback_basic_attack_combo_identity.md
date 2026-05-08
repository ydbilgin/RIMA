---
name: Basic attack combo identity lock
type: feedback
trigger: basic attack, LMB, RMB, combo, weapon speed, attack animation
description: Character-specific LMB/RMB combo and animation identity rule
---

## Design Lock

Every playable class must own its LMB and RMB identity.

- LMB is not a generic shared attack. It is the class primary attack.
- RMB is not a generic shared utility. It is the class secondary attack/outlet.
- LMB and RMB may each have internal combo/state rhythm when the class fantasy needs it.
- Basic attack animation clips must be short, readable, and fluid.
- Weapon/body fantasy controls timing:
  - heavy weapon = slower anticipation, heavier hit frame, stronger recovery
  - light weapon = faster startup, smaller recovery, sharper chain timing
  - caster/ranged = cast/release rhythm instead of melee slash timing
- Fluid does not mean long. Prefer small 3-6 frame attack beats with clear windup, hit frame,
  and recovery.

## Current Code Reality (updated 2026-05-07 -- Antigravity QC fix)

`BasicAttackProfile.cs` strategy pattern uygulanmis durumda:
- `IBasicAttackBehavior` interface + `BasicAttackBehaviorBase` mevcut
- `MeleeChainBehavior` -> Warblade (+ diger melee siniflara fallback)
- `CastRhythmBehavior` -> Elementalist (Rift Bolt + Switch/Lightbreak)
- `ShotCadenceBehavior` -> Ranger (tap/hold arrow + Tactical Roll)
- `VeilStrikeBehavior` -> Shadowblade (Veil Strike + Veil Flicker)
- `HeatGaugeBehavior` (Gunslinger) + `MarkPulseBehavior` henuz implement edilmedi, MeleeChain'e fallback
- `.asset` dosyalari (BasicAttackProfile_Warblade.asset vb.) henuz olusturulmadi -- Inspector assign gerekiyor
- `PlayerAttack.Awake()` artik null profile icin LogError + disable veriyor (sessiz fallback kaldirildi)
- `OnCommitBeat` event kaldirildi (dead code + SO leak riski)
- `classType` artik `ClassType` enum (eski `int` degil)
- `ClassType` enum 10 sinifi kapsiyor: Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer

Kalan eski sorunlar (kod duzeyinde):
- Elementalist / Ranger / Shadowblade icin LMB chain ritmi hala tek-vuruslu;
  `CastRhythmBehavior` / `ShotCadenceBehavior` / `VeilStrikeBehavior` cadence altyapisini kuruyor
  ama 3-bolt / multi-shot / 3-slash full chain animasyon entegrasyonu yapilmadi.
- `LMBEcolSystem` stub kalmaya devam ediyor.

## Implementation Rule

Before adding new playable classes or finalizing animation production, introduce an explicit
per-class basic attack contract:

1. LMB identity and combo rhythm.
2. RMB identity and combo/resource relationship.
3. Attack speed class: heavy, medium, fast, ranged, caster.
4. Per-step animation frame count and hit-frame timing.
5. Per-step hitbox/projectile/VFX timing.

No class should ship with default Warblade fallback unless it is Warblade.

## PixelLab Rule

For basic attacks, generate movement/pose sheets first:

- one sheet per class LMB chain
- one sheet per class RMB action if it has animation identity
- separate weapon/body motion from VFX/projectile
- keep prompt action narrow: one step or one chain, no baked enemy reaction

