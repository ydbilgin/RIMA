# Warblade Animation Prompts — PixelLab
Date: 2026-05-07 | Status: READY TO GENERATE

## Global Settings (all Warblade animations)
- Canvas: **252×252px**
- Camera: **Low Top-Down** (35° 3/4 ARPG view — Diablo 2 / Hades style)
- Directions: **4 cardinal** (S, E, N, W). East generated → flip horizontal for West.
- Endpoint: `animate-with-text-v3`
- Palette: Dark steel armor, Cold Blue accent `#7BA7BC` (armor cracks, sword edge), `#66AAFF` (rift energy/hit flashes)
- FORBIDDEN in prompts: purple tones, hand glows, fire VFX, impact sparks, swoosh trails (all engine-side)
- Workflow: **Body first, weapon second pass** (Edit Image Pro for sword overlay to maintain consistent scale)

---

## 1. Idle
**Frames:** 8
**Technique:** Standard generation

```
2D fantasy RPG pixel art spritesheet, 8-frame idle animation, low top-down 35-degree 3/4 ARPG perspective.
Heavy armored knight standing still, breathing heavily, slight armor sway. Two-handed greatsword resting
on shoulder. Muted cool steel plate armor with #7BA7BC cold blue accent lines on joints and edge.
Clean pixel clusters, no noise, no baked VFX, transparent background.
```

---

## 2. Run Cycle
**Frames:** 6 per direction (no walk cycle — run only)
**Technique:** Extreme Pose Method — generate extreme high-knee pose first, flip for opposite, interpolate between

```
2D fantasy RPG pixel art spritesheet, 6-frame run cycle animation, low top-down 35-degree 3/4 ARPG perspective.
Heavy armored knight running fast, alternating arms and legs, lifting knees high, heavy forward lean.
Two-handed greatsword carried across back or at side. Muted cool steel armor with #7BA7BC cold blue accents.
Clean pixel clusters, no noise, transparent background.
```

---

## 3. LMB — Iron Combo (3-beat chain)

### Beat 1: Low Sweep
**Frames:** 4 (hit on frame 3)
**Technique:** Generate peak frame (mid-sweep) first, interpolate START→PEAK→END

```
2D fantasy RPG pixel art spritesheet, 4-frame attack animation, low top-down 35-degree 3/4 ARPG perspective.
Heavy armored knight performing a low wide horizontal sweep with a two-handed greatsword, grounded stance,
twisting torso, sword cutting left to right at knee height. Muted steel armor with #7BA7BC cold blue accent.
No impact sparks, no VFX trails, transparent background.
```

### Beat 2: Overhead Cut
**Frames:** 5 (hit on frame 4)
**Technique:** Generate peak frame (sword at zenith) first, interpolate down

```
2D fantasy RPG pixel art spritesheet, 5-frame attack animation, low top-down 35-degree 3/4 ARPG perspective.
Heavy armored knight bringing a two-handed greatsword down in a brutal overhead vertical chop, downward
momentum, heavy shoulder mass dropping, both hands driving blade downward. Muted steel armor with
#7BA7BC cold blue accent. No VFX, no trails, transparent background.
```

### Beat 3: Shoulder Ram / Blade Drive (Commit Beat → triggers Iron Verdict proc)
**Frames:** 5-6 (hit on frame 4)
**Technique:** Generate forward-thrust peak first

```
2D fantasy RPG pixel art spritesheet, 5-frame attack animation, low top-down 35-degree 3/4 ARPG perspective.
Heavy armored knight thrusting aggressively forward, leading with armored shoulder, greatsword pushed
forward as battering ram. Armored weight shifting hard forward, feet pushing off ground. Muted steel
armor with #7BA7BC cold blue accent. No VFX, transparent background.
```

---

## 4. RMB — Crossguard Bash
**Frames:** 4-5
**Technique:** Standard, grounded pose — NOT a sweeping slash

```
2D fantasy RPG pixel art spritesheet, 4-frame attack animation, low top-down 35-degree 3/4 ARPG perspective.
Heavy armored knight performing a short aggressive forward shoulder-check, bracing the greatsword's
crossguard across chest as a shield, feet planted, leaning into the bash with full armor weight.
Compact motion, not a sweep. Muted steel armor with #7BA7BC cold blue accent. No VFX, transparent background.
```

---

## 5. Hit React (Flinch)
**Frames:** 3
```
2D fantasy RPG pixel art spritesheet, 3-frame hit reaction, low top-down 35-degree 3/4 ARPG perspective.
Heavy armored knight recoiling from a hit, slight backward stumble, armor rattling, head snapping back.
Muted steel armor with #7BA7BC cold blue accent. Transparent background.
```

---

## 6. Death
**Frames:** 6
```
2D fantasy RPG pixel art spritesheet, 6-frame death animation, low top-down 35-degree 3/4 ARPG perspective.
Heavy armored knight collapsing forward onto knees then falling face-down, greatsword clattering to ground.
Heavy armored weight conveyed. Muted steel armor with #7BA7BC cold blue accent fading. Transparent background.
```

---

## Generation Order (recommended)
1. Idle (easiest reference frame → establishes silhouette)
2. Run — South direction (body only first, weapon second pass)
3. LMB Beat 1 — South
4. LMB Beat 2 — South
5. LMB Beat 3 — South
6. RMB — South
7. Repeat Run + attacks for East → flip for West
8. North direction last (back-facing, simplest)
