# RIMA Animation Production Guide -- Wave 1 (ChatGPT Context Document)

---

## Game Context

- RIMA: 2D isometric ARPG roguelite, Unity 2D URP
- Camera: Low Top-Down, 30-35 degree ARPG tilt (Diablo/Hades style)
- Character canvas: 128x128px final in-game target
- PPU: 64 (Unity import)
- 4 playable directions produced: S / E / N / W
- Tone: Fractured Epic -- compact, non-chibi, reads clearly at 64px downscale

---

## PixelLab Direction Convention

| Direction | DirX  | DirY  | Sprite suffix | BlendPos       | PixelLab source file (S43 offset) |
|-----------|-------|-------|---------------|----------------|-----------------------------------|
| S         | 0     | -1    | _S            | (0, -1)        | south-east.png                    |
| SE        | +0.71 | -0.71 | _SE           | (0.71, -0.71)  | east.png                          |
| E         | +1    | 0     | _E            | (1, 0)         | north-east.png                    |
| NE        | +0.71 | +0.71 | _NE           | (0.71, 0.71)   | north.png                         |
| N         | 0     | +1    | _N            | (0, 1)         | north-west.png                    |
| NW        | -0.71 | +0.71 | _NW           | (-0.71, 0.71)  | west.png                          |
| W         | -1    | 0     | _W            | (-1, 0)        | south-west.png                    |
| SW        | -0.71 | -0.71 | _SW           | (-0.71, -0.71) | south.png                         |

S43 offset note: anchors generated SW-facing (1 step CW offset from expected). Use the "PixelLab source file" column to know which PixelLab direction to download for each in-game direction. Example: to get the in-game S (front-facing) sprite, download the PixelLab "south-east" render.

Naming rule: uppercase suffix. `warblade_run_S.png` is correct, `warblade_run_s.png` is wrong.

---

## Animation Pipeline Rules (LOCKED)

> WARNING CORRECTION (2026-05-02): The "128px = 16 frames" claim is INCORRECT per official PixelLab docs.
> All canvas sizes 65-256px yield 4 frames (2x2 grid). Keep 252px canvas. Crop frames to 128px in post-processing only.
> Source: pixellab.ai/docs/tools/animate-with-text-pro

### Canvas Decision (CORRECTED 2026-05-02)
- Keep 252px canvas for all animation. Do NOT crop to 128px.
- Reason: Official docs show 65-256px all yield 4 frames (2x2 grid). Cropping gives no benefit and reduces headroom the AI needs for limb extension.
- User's experience: up to 8 frames visible at 252px in "Animate with Text NEW" (may be tier/version specific)
- For more than 4-8 frames: chain multiple 4-frame animation runs and stitch in post
- Final Unity import: crop/scale each frame to 128x128 as post-processing step

### Core Animation Constraints Block

Add this block verbatim to every PixelLab animation prompt. Replace [action phrase here] with the skill-specific phrase.

```
FOOTPRINT LOCK: identical pixel extents (top, bottom, left, right) across all frames.
ANCHOR: feet aligned to same pixel row, head height constant throughout animation.
CONTINUITY: same character design, weapon, armor, palette -- same character rotated, not redesigned.
NO EMBEDDED VFX: no impact sparks, no weapon trails, no particle effects baked into sprite frames. Clean character motion only.
full body, centered, same scale as reference, no zoom-in.
ACTION: [action phrase here]
```

### Frame Count Guidelines

| Animation type | Target frames | Method |
|---|---|---|
| Run loop | 4-8 frames | Animate with Text NEW (chain if needed) |
| Idle loop | 4 frames | Animate with Text NEW |
| Simple attack (1-2 states) | 4 frames | Animate with Text NEW |
| Complex skill (3 states) | 4+4 chained | Two separate runs, stitch |
| Keyframe-precise move | any | Keyframe poses + Interpolation |

### Keyframe + Interpolation Workflow

Use when: animation needs more than 8 frames OR has 3+ distinct pose phases (windup/hold/release with clear silhouette changes at each).

1. Generate POSE A (start keyframe) as a static sprite using "Create Object" or upload a reference image.
2. Generate POSE B (end keyframe) as a static sprite.
3. Use PixelLab "Interpolate NEW" (Animate Between 2 Frames): upload A + B, specify action, set target frame count.
4. Review output. If middle frames drift or character identity breaks, add POSE C as an intermediate keyframe and interpolate A->C and C->B separately.

Best for: iron_charge (windup -> sprint -> impact), death_blow (raise -> apex -> slam), blizzard (arms raise -> spread -> drop), veil_burst (gather -> 4-point radial strike).

### What NOT to Do

- Do NOT crop canvas from 252px to 128px -- no frame benefit, and the AI needs the headroom for limb extension.
- Do NOT accept embedded weapon sparks, trails, or particle effects baked into frames -- regenerate with NO EMBEDDED VFX constraint.
- Do NOT use MCP animate_character tool (quality too low, 4-frame hard limit).
- Do NOT mirror W direction from E in code -- generate W separately (asymmetric armor and weapon hand positioning).
- Do NOT change PPU to fix size drift -- PPU is locked at 64. If size looks wrong, regenerate the sprite.

### Import Standard (Unity, LOCKED)

| Setting | Value |
|---|---|
| PPU | 64 |
| Sprite Mode | Multiple |
| Frame size | 128x128 per cell |
| Pivot | center (0.5, 0.5) |
| Filter | Point (no filter) |
| Compression | Uncompressed |

Any deviation from this causes visual bugs. Do not adjust as a workaround.

---

## Characters

---

### WARBLADE

**Lore:** Heavy armored warrior. Massive two-handed sword. Rage resource mechanic -- Rage builds on hits and impacts, powers advanced skills.

**Weapon constraint (add to EVERY Warblade prompt):**
```
both hands gripping long two-handed sword hilt, right hand near crossguard, left hand near pommel
```

**Palette:** Muted steel-grey armor. Amber-orange Rage crack accent along armor seams and weapon edge. No bright colors on base armor.

**Camera note (add to every Warblade prompt):**
```
MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally.
```

**Reference sprite:** `Assets/Sprites/Characters/Warblade/base/warblade_S.png`

---

#### iron_charge

- **Role:** Gap-closer / stun initiator / Rage builder. Shoulder charge into target; hold variant becomes wide blade slash through multiple enemies.
- **Animation states:**
  - wind_up (2f): weight shift back, weapon arm cocks, boot plants -- leans back before burst
  - dash_travel (3f): low-center-of-gravity sprint, shoulder leads, weapon trails
  - impact (2f): shoulder collision, shockwave on ground; enemy stun lock held pose
  - recovery (1f): character straightens, weapon returns to guard
  - blade_rush_variant (3f): standing horizontal weapon sweep across 6m line (hold mode only)
- **Total frames:** 11
- **Action phrase:** `heavy armored shoulder charge with ground tremor impact and weapon collision`
- **Priority:** P0 -- blocks Warblade playable build
- **Keyframe needed:** YES -- 5 distinct phases (windup, dash, impact, recovery, variant branch)
- **Suggested method:** Produce standard path (wind_up -> dash_travel -> impact -> recovery) as one Animate with Text run (7 frames), then generate blade_rush_variant separately (3 frames). Keyframe+Interpolate for wind_up to dash transition.

---

#### sunder_mark

- **Role:** Armor debuff mark applier. Precise weapon strike places Sundered state (armor -40% for 8s) on target. This is the Warblade's exclusive Sundered fissure skill.
- **Animation states:**
  - wind_up (1f): weapon tip angled at target, forward lean, precision stance
  - active (2f): sharp forward thrust/slash at armor location; crack propagates on F2
  - recovery (1f): weapon returns, slight head tilt reading the debuff
- **Total frames:** 4
- **Action phrase:** `precise forward thrust into enemy armor creating visible fissure crack`
- **Priority:** P0 -- Sundered state origin skill, required before Death Blow and Iron Crush synergy
- **Keyframe needed:** NO -- 4 frames, simple three-phase attack
- **Suggested method:** Animate with Text NEW (4 frames)

---

#### crippling_blow

- **Role:** Heavy damage + healing debuff applier. Anatomical blow aimed at joint or flank. Healing reduction -50% (6s). After Iron Charge: full heal-block.
- **Animation states:**
  - wind_up (2f): weapon raised overhead, body twists at waist; F1 coil, F2 apex -- weapon fully above character center
  - active (2f): downward diagonal slam into target flank; F1 contact, F2 weapon buried/drag
  - recovery (2f): weapon wrenched back to guard, shoulders settle
- **Total frames:** 6
- **Action phrase:** `heavy overhead weapon slam into enemy's flank with deliberate impact`
- **Priority:** P1 -- core damage skill, required for Warblade combo chain
- **Keyframe needed:** NO -- 6 frames, clean three-phase
- **Suggested method:** Animate with Text NEW (6 frames)

---

#### iron_crush

- **Role:** 6-second damage amplification stance. Weapon forward, low threatening posture. All damage +30%. Amber-orange glow along weapon edge signals active state.
- **Animation states:**
  - activate (3f): character shifts into low stance, weapon angles forward, armor edge glow appears; F1 shift, F2 settle, F3 glow established
  - active_loop (2f): subtle breathing hold, weapon forward, glow pulses slowly
  - deactivate (1f): stance returns to guard, glow fades
- **Total frames:** 6
- **Action phrase:** `character shifts into low threatening stance anchoring weight with weapon forward`
- **Priority:** P1 -- core stance skill, required before Sunder Mark synergy testing
- **Keyframe needed:** NO -- 6 frames, stance activation
- **Suggested method:** Animate with Text NEW (6 frames)

---

#### gravity_cleave

- **Role:** AoE pull + damage + slow. Both hands raise weapon overhead and slam into ground, creating inward-dragging shockwave that pulls enemies toward impact point.
- **Animation states:**
  - wind_up (2f): both hands grip overhead, body rises on toes then drives weight down; F1 raise, F2 apex -- weapon tip clears silhouette top
  - active (2f): slam into ground; F1 contact, F2 ground shockwave ring expanding
  - pull_settle (1f): brief hold at impact, pull moment
  - recovery (2f): weapon wrenched up, guard restored, debris settles
- **Total frames:** 7
- **Action phrase:** `both hands grip overhead slam into ground creating inward-dragging shockwave`
- **Priority:** P1 -- core AoE skill, required for crowd-control chain
- **Keyframe needed:** YES -- 4 distinct phases including hold and recovery
- **Suggested method:** Keyframe+Interpolate: POSE A = apex (both hands overhead), POSE B = impact (weapon in ground). Interpolate A->B for slam. Generate pull_settle and recovery with Animate with Text.

---

#### earthsplitter

- **Role:** Knockup + Rage builder. Weapon drives into ground, erupts upward. Hold variant adds 3 sequential ground-crack waves applying Broken stacks to enemies in path.
- **Animation states:**
  - wind_up (2f): overhead weapon drive down, vertical alignment, foot-plant emphasis
  - impact (1f): weapon contacts ground, upward eruption spike; debris goes UP not outward
  - knockup_hold (1f): static hold, enemies in air above
  - wave_variant_1 (2f): first wave crack rolling forward (hold mode)
  - wave_variant_2 (1f): second wave, Broken stack applied
  - wave_variant_3 (1f): third wave, final stack
  - recovery (1f): weapon pulled from ground, guard restored
- **Total frames:** 9 (5 standard + 4 wave extension)
- **Action phrase:** `weapon drives into ground erupting upward in massive knockup`
- **Priority:** P1 -- Broken stack origin skill, required before Bladestorm synergy testing
- **Keyframe needed:** YES -- 5 distinct phases, 9 total frames
- **Suggested method:** Keyframe+Interpolate: generate wind_up->impact as one chain (3f). Generate wave sequence separately with Animate with Text. Combine rows in Aseprite.

---

#### ironclad_momentum

- **Role:** 6-second defensive damage-absorption stance. Weapon held horizontal across body, knees bent, chin down. Incoming damage partially converted to Rage. Amber seam glow intensifies as Rage charges.
- **Animation states:**
  - activate (2f): drop into guard stance; F1 drop, F2 amber seams light on absorption window open
  - active_loop (3f): held defensive posture, seam glow varies -- shows Rage charging; subtle body micro-sway
  - hit_absorb (1f): single-frame body brace + bright amber flash per incoming hit during stance
  - deactivate (1f): stance releases, seam glow fades, return to guard
- **Total frames:** 7
- **Action phrase:** `character drops into guard stance with weapon held horizontally across body`
- **Priority:** P1 -- absorb-counter identity skill, required for Rule 57 compliance review
- **Keyframe needed:** NO -- 7 frames, stance activation + loop
- **Suggested method:** Animate with Text NEW (7 frames). Generate active_loop as a separate 3-frame loop chunk.

---

#### iron_counter

- **Role:** Reactive counter (0.8s window). Ready brace, absorb incoming hit, explosive counter-slam at 180% damage. Stuns 0.5s, generates Rage +25. At Rage 80+: adds knockback shockwave.
- **Animation states:**
  - ready_window (2f): weight drop, weapon arm cocks back; amber seam flicker signals window open
  - absorb_flash (1f): armor flash when incoming hit lands during window
  - counter_strike (3f): explosive pivot + weapon slam; F1 pivot turn, F2 impact, F3 follow-through
  - recovery (1f): weapon returns, Rage bar surge visible
- **Total frames:** 7
- **Action phrase:** `planted armor brace pivots to explosive counter-slam erupting force outward`
- **Priority:** P0 -- counter identity skill, required for Rule 57 compliance review and class distinction audit
- **Keyframe needed:** YES -- 4 distinct phases with pivot (rotational motion benefits from keyframes)
- **Suggested method:** Keyframe+Interpolate: POSE A = brace/ready, POSE B = pivot mid, POSE C = impact. Interpolate A->B->C for counter_strike. Generate ready_window and recovery with Animate with Text.

---

#### battle_surge

- **Role:** 8-second sustain stance. Every Rage spend recovers 5% HP. Character drives weapon pommel into ground then rises. Armor seams and weapon pulse amber at full burn. At Rage 80+: extends to 12s (no new animation row).
- **Animation states:**
  - activate (3f): pommel plant; F1 pommel drives down, F2 rise with seam ignition, F3 fully lit surge stance
  - active_loop (3f): aggressive-forward combat-ready loop, seam burn holds at full
  - surge_pulse (1f): per Rage-spend event: brief amber chest flash
  - deactivate (1f): seam burn fades, pommel lifts, return to guard
- **Total frames:** 8
- **Action phrase:** `character drives weapon pommel into ground igniting armor seams in surge stance`
- **Priority:** P2 -- advanced sustain skill, secondary to P0/P1 combat chain skills
- **Keyframe needed:** NO -- 8 frames, stance activation
- **Suggested method:** Animate with Text NEW (8 frames)

---

#### deep_wound

- **Role:** DoT applier + Rage builder. Deliberate raking slash leaving 8s bleed DoT. Grants Rage +35. During Iron Crush: bleed tick doubles.
- **Animation states:**
  - wind_up (1f): weapon draws back along hip, low angle prep -- surgical stance
  - active (3f): raking weapon draw across target; F1 entry contact, F2 drag across, F3 exit follow-through
  - recovery (2f): weapon returns, Rage bar surges, character checks stance
- **Total frames:** 6
- **Action phrase:** `controlled raking slash leaving precise wound trail across target`
- **Priority:** P2 -- advanced DoT skill, secondary to P0/P1 combat chain skills
- **Keyframe needed:** NO -- 6 frames, simple three-phase raking slash
- **Suggested method:** Animate with Text NEW (6 frames)

---

#### death_blow

- **Role:** Master finisher. Requires target Broken OR Sundered. Empties all Rage in one catastrophic downward strike. 400% damage (Crippling Blow active: 600%).
- **Animation states:**
  - wind_up (4f): both hands raise weapon, body fully rotates; all Rage channels into weapon -- amber seams drain toward weapon across 4 frames
  - apex (1f): weapon held at full overhead -- maximum Rage glow, brief hold before execution
  - active (2f): catastrophic downward slam; F1 contact + Rage detonation burst, F2 aftermath dust settling
  - recovery (2f): weapon heavy on ground, character straightens, all glow gone -- Rage empty
- **Total frames:** 9
- **Action phrase:** `overwhelming downward strike with all Rage channeled into catastrophic weapon detonation`
- **Priority:** P1 -- Master finisher, required for Warblade identity review; depends on Sunder Mark and Earthsplitter
- **Keyframe needed:** YES -- 4 distinct phases, 9 total frames, apex hold requires precise control
- **Suggested method:** Keyframe+Interpolate: POSE A = neutral/ready, POSE B = full wind_up apex (weapon highest point, maximum glow), POSE C = impact (weapon in ground). Interpolate A->B for wind_up (4f), B->C for active (2f). Generate recovery with Animate with Text.

---

### ELEMENTALIST

**Lore:** Academic spell-weaver. Casts via hand gestures -- no weapon. Element system: Fire / Frost / Light. Element reactions trigger when spells interact (e.g. Fireball + Glacial Spike = Freeze). Lightbreak is a special combined-element state.

**Weapon constraint (add to EVERY Elementalist prompt):**
```
hands open, palm-out casting gesture, no weapon held
```

**Palette:** Muted blue-grey academic robe base. Element color appears ONLY on spell VFX, not on the character body. Fire: red-orange. Frost: cold blue-white. Light: white-gold / prismatic. The caster body stays muted throughout.

**Camera note (add to every Elementalist prompt):**
```
MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally.
```

**Reference sprite:** `Assets/Sprites/Characters/Elementalist/base/elementalist_S.png`

---

#### fireball

- **Role:** Primary fire damage / Fire State builder / Living Bomb setup. Repeatable orb cast. One arm extends, hand opens, orb forms and launches.
- **Animation states:**
  - cast_start (2f): arm extends forward, hand opens -- orb forming in palm; F1 arm raise, F2 orb visible glowing red-orange
  - cast_release (1f): hand pushes forward -- orb launches; palm follow-through
  - recovery (1f): arm retracts, neutral ready stance
- **Total frames:** 4
- **Action phrase:** `arm extends forward palm open launching fire orb projectile`
- **Priority:** P0 -- starred core skill, blocks Elementalist playable build
- **Keyframe needed:** NO -- 4 frames, simple cast-release
- **Suggested method:** Animate with Text NEW (4 frames)

---

#### glacial_spike

- **Role:** Frost line damage / slow / Fire State consumer / Freeze trigger. Opposite arm from Fireball: draw back, thrust forward in stabbing motion projecting elongated ice shard.
- **Animation states:**
  - cast_start (2f): arm draws back, fingers tensed, frost mist forms at knuckles; F1 draw back, F2 frost mist visible
  - cast_release (1f): arm thrusts forward in stabbing motion -- spike launches as line projectile
  - recovery (1f): arm retracts, ready stance
- **Total frames:** 4
- **Action phrase:** `arm draws back then thrusts forward in stabbing motion launching ice spike line`
- **Priority:** P1 -- core Frost spell, required for element reaction chain testing
- **Keyframe needed:** NO -- 4 frames, draw-and-thrust
- **Suggested method:** Animate with Text NEW (4 frames)

---

#### living_bomb

- **Role:** Delayed AoE explosion / chain propagation on kill. Both hands converge, orb builds between palms, launches and embeds in target. Detonates after 5s.
- **Animation states:**
  - cast_start (3f): both hands come together at chest; F1 hands converge, F2 orb forming between palms, F3 orb at full size pulsing
  - cast_release (2f): both hands push forward -- orb launches; F1 push launch, F2 hands spread after release
  - recovery (1f): return to ready stance
- **Total frames:** 6
- **Action phrase:** `both hands converge building fire orb between palms then push forward launching delayed bomb`
- **Priority:** P1 -- core fire chain skill, required for Fireball combo testing
- **Keyframe needed:** NO -- 6 frames, two-hand convergence and push
- **Suggested method:** Animate with Text NEW (6 frames)

---

#### blink

- **Role:** 6m teleport / damage to crossed enemies / next spell +20%. Body dissolves into light, jumps 6m, rematerializes. Post-blink aura signals spell buff.
- **Animation states:**
  - depart (2f): light accumulates around caster; F1 light builds, F2 caster fades to white flash
  - in_transit (1f): near-invisible placeholder frame (body in transit)
  - arrive (2f): reverse flash; F1 light burst at destination, F2 caster solidifies from light
  - post_blink_ready (2f): subtle aura shimmer on body -- next spell buff active; loops until next cast
- **Total frames:** 7
- **Action phrase:** `body dissolves into radiant light then rematerializes at destination with white-blue flash`
- **Priority:** P1 -- core mobility skill, required for Frozen Orb and Light State chain testing
- **Keyframe needed:** YES -- 4 distinct phases, transparency required for in_transit
- **Suggested method:** Keyframe+Interpolate: POSE A = solid caster, POSE B = full light dissolution. Interpolate for depart. Generate arrive as separate Interpolate (POSE C = light burst, POSE D = solidified). Confirm PixelLab alpha channel support.

---

#### frozen_orb

- **Role:** Slow-moving large AoE chill field. Both arms arc overhead then project forward -- large orb builds between raised arms. Blink through orb detonates it.
- **Animation states:**
  - cast_start (3f): arms arc overhead then project; F1 arms rise, F2 orb forms between raised arms (larger than Fireball), F3 arms push down to project
  - cast_release (1f): both arms push forward and outward -- orb launched at slow speed
  - recovery (1f): arms lower, caster watches orb travel
- **Total frames:** 5
- **Action phrase:** `both arms arc overhead forming large ice orb between raised arms then push forward projecting slow field orb`
- **Priority:** P1 -- core Frost control skill, required for Blink detonation synergy testing
- **Keyframe needed:** NO -- 5 frames, overhead arc and push
- **Suggested method:** Animate with Text NEW (5 frames)

---

#### prism_beam

- **Role:** Cursor-aimed channel beam / pierce / Light State damage scaler. One arm extended forward, beam emanates from palm. Other hand supports at elbow. 3-frame channel loop. At Light State 3+: burst fires.
- **Animation states:**
  - cast_start (2f): arm extends forward, hand opens; F1 arm raise, F2 palm open, beam gathering
  - channel_loop (3f): extended pose with arm tremor; rainbow edge glow on beam; 3-frame sustained effort loop
  - burst_release (2f): Light State 3+ only -- arm recoils then forward burst; F1 recoil, F2 forward flare
  - recovery (1f): arm lowers, head tilt exhale
- **Total frames:** 8
- **Action phrase:** `arm extended forward palm open channeling prismatic light beam with sustained arm tremor`
- **Priority:** P1 -- core Light channel skill, required for Light State chain testing
- **Keyframe needed:** YES -- 4 phases, 8 frames, channel loop requires distinct sustained-effort posture
- **Suggested method:** Animate with Text NEW (8 frames at 128px). Generate channel_loop as its own 3-frame loop separate from cast_start and burst_release if needed.

---

#### meteor

- **Role:** 0.5s wind-up then large meteor falls from above onto cursor point. Single raised arm pointing skyward (calling meteor), then arm drops at arrival. 3 wind-up frames. AoE knockdown at impact.
- **Animation states:**
  - wind_up (3f): arm rises skyward pointing up; F1 arm rising, F2 arm fully raised pointing, F3 arm pulls down sharply signaling impact
  - release (1f): arm drops, palm faces down -- impact moment; caster continues moving
  - recovery (2f): caster steps back or shifts stance, watches impact zone
- **Total frames:** 6
- **Action phrase:** `single arm raised skyward index finger pointing calling meteor then arm drops sharply at impact`
- **Priority:** P1 -- core large AoE skill, required for Blizzard synergy testing
- **Keyframe needed:** YES -- 3-phase wind_up requires precise keyframe at apex (arm fully extended skyward)
- **Suggested method:** Keyframe+Interpolate: POSE A = neutral, POSE B = arm fully raised skyward. Interpolate A->B for wind_up (3f). Generate release and recovery with Animate with Text.

---

#### frost_wall

- **Role:** Cursor-placed line barrier. Arm sweeps horizontally across body tracing wall line, then pushes outward materializing the wall. Ice-light surface, 4s duration.
- **Animation states:**
  - cast_start (2f): arm sweeps horizontally across front of body; F1 arm sweep across, F2 arm at end of sweep
  - cast_release (2f): arm pushes forward -- wall fully materializes; F1 push forward, F2 arm extended wall confirmed
  - recovery (1f): arm lowers, caster may continue moving
- **Total frames:** 5
- **Action phrase:** `arm sweeps horizontally across body tracing wall line then pushes forward materializing ice barrier`
- **Priority:** P1 -- core wall control skill, required for Light State chain testing
- **Keyframe needed:** NO -- 5 frames, sweep and push gesture
- **Suggested method:** Animate with Text NEW (5 frames)

---

#### solar_flare

- **Role:** Cursor-aimed cone radiant burst / Light State amplifier. Arm raises to chest height palm facing forward, then sharp forward push as cone fires. Light State adds a secondary pulse ripple.
- **Animation states:**
  - wind_up (2f): arm rises to chest height, palm forward, solar radiance accumulates; F1 arm rises, F2 palm charged gold-white
  - active (2f): sharp forward push -- cone fires; F1 push mid-extension, F2 fully extended cone visible
  - radiant_pulse (1f): Light State only -- secondary pulse wave ripples from palm after active F2
  - recovery (2f): arm lowers, palm dims, head tilts back slightly
- **Total frames:** 7
- **Action phrase:** `arm raised chest height palm forward then sharp forward push launching cone of solar radiant light`
- **Priority:** P2 -- advanced Light skill, secondary to P0/P1 core chain
- **Keyframe needed:** NO -- 7 frames, two-phase push with optional pulse
- **Suggested method:** Animate with Text NEW (7 frames)

---

#### radiant_pillar

- **Role:** 6-second radiant aura stance. Arm extends downward pointing at ground, pillar of radiant light rises from caster feet. Every Fire/Frost spell creates a radiant echo during window.
- **Animation states:**
  - activate (3f): arm extends downward, index finger points ground; F1 arm lowers, F2 finger points at ground, F3 pillar rising confirmed
  - active_loop (3f): caster stands in radiant column, arms at sides, head slightly raised; light column pulses slowly
  - echo_pulse (1f): per Fire/Frost cast during window -- brief radiant flash on caster
  - deactivate (1f): pillar fades downward, return to neutral
- **Total frames:** 8
- **Action phrase:** `arm extends downward index finger pointing at ground calling radiant light pillar rising from feet`
- **Priority:** P2 -- advanced aura skill, secondary to core chain skills
- **Keyframe needed:** NO -- 8 frames, stance activation with downward pointing
- **Suggested method:** Animate with Text NEW (8 frames)

---

#### element_charge

- **Role:** 8-second fire haste buff. Both fists close at sides, fire energy rises through body feet-to-head as shimmer. All Fire spells become instant-cast during window.
- **Animation states:**
  - activate (3f): both fists close at sides, fire shimmer rises; F1 hands close, F2 shimmer to waist, F3 shimmer reaches head
  - active_loop (2f): hands slightly closed, orange shimmer on knuckles; ready-cast stance; 2-frame loop
  - fire_cast_flash (1f): per Fire spell during window -- brief intensification orange flash on both hands
  - deactivate (1f): fists open, shimmer fades, exhale
- **Total frames:** 7
- **Action phrase:** `both fists close at sides as fire energy shimmer rises through body from feet to head`
- **Priority:** P2 -- advanced fire haste skill, secondary to core chain skills
- **Keyframe needed:** NO -- 7 frames, shimmer-rise buff activation
- **Suggested method:** Animate with Text NEW (7 frames)

---

#### blizzard

- **Role:** Master zone caster. 1s wind-up, 8s persistent blizzard zone at cursor. Both arms raise overhead then spread wide in arc then pull downward -- calling the storm. Caster moves freely after.
- **Animation states:**
  - wind_up (4f): both arms rise then spread wide arc; F1 arms rise, F2 arms overhead apex, F3 arms begin spreading, F4 arms fully spread -- storm called
  - cast_release (2f): both arms pull downward in final arc push; F1 downward pull, F2 arms at sides zone confirmed
  - recovery (1f): arms lower fully, movement-ready stance
- **Total frames:** 7
- **Action phrase:** `both arms rise overhead then spread wide in arc then pull downward calling blizzard storm zone`
- **Priority:** P1 -- Master AoE skill, required for Meteor synergy chain testing
- **Keyframe needed:** YES -- 4-frame wind_up with wide arm arc requires precise keyframes for apex and full-spread poses
- **Suggested method:** Keyframe+Interpolate: POSE A = neutral, POSE B = both arms raised overhead, POSE C = both arms fully spread wide. Interpolate A->B (F1-F2), B->C (F3-F4). Generate cast_release and recovery with Animate with Text.

---

### RANGER

**Lore:** Hunter/tracker. Bow as primary weapon. Precision traps, marks, and kill-zone methodology. Distance discipline -- Ranger never approaches target. Deliberate and methodical, not aggressive.

**Weapon constraint:** Bow drawn or at rest -- specify per skill. Non-bow hand used for trap placement and casting gestures.

**Palette:** Earthy greens and browns. Bone-white accent (mark reticles, trap lines, root rings). Minimal color. Earth-green for active energy accents. Focus meter accent: subtle green-amber on bow limbs at Focus 75+.

**Reference sprite:** `Assets/Sprites/Ranger/ranger_neutral_S43.png`

**Critical identity rule:** Ranger NEVER moves toward target during any skill animation. Any approach motion is a QC blocker.

---

#### ranger_rift_arrow

- **Role:** LMB basic -- pressure / opener. Instant tap: quick snap-shot. Hold 1s: charges and guarantees Mark on hit.
- **Animation states:**
  - wind_up (2f): bow draw; tap = partial draw (1f notch + 1f partial pull); hold = same start extended
  - active (2f): release + arrow flight start; F1 release (string snap, body snaps back); F2 follow-through; charged variant: brighter bow-limb flash on F1
  - recovery (1f): bow arm lowers, weight shifts back
- **Total frames:** 5
- **Action phrase:** `bow draw and release snap-shot arrow with deliberate planted stance`
- **Priority:** P0 -- LMB basic, blocks Ranger playable build
- **Keyframe needed:** NO -- 5 frames, basic draw-release
- **Suggested method:** Animate with Text NEW (5 frames). Mark reticle is a separate 2-frame micro-asset (pulse + idle); do not bake into caster sheet.

---

#### ranger_pinning_shot

- **Role:** Control -- Root 1.5s. Disciplined snap-shot from braced planted stance. Full draw, release, follow-through.
- **Animation states:**
  - wind_up (2f): half-draw stance, rear elbow raised, front arm extended; wider arm span than neutral
  - active (3f): full draw F1, release snap F2 (arrow departs, audio anchor), follow-through F3
  - recovery (2f): bow arm lowers, weight settles; caster remains planted
- **Total frames:** 7
- **Action phrase:** `bow full draw with planted stance then deliberate release snap and follow-through`
- **Priority:** P1
- **Keyframe needed:** NO -- 7 frames, standard draw-release-recover
- **Suggested method:** Animate with Text NEW (7 frames)

---

#### ranger_marked_detonate

- **Role:** Remote detonation of Mark from distance. Two-finger gesture-cast trigger. Caster does NOT move. Explosion happens at marked target location as separate world VFX.
- **Animation states:**
  - wind_up (2f): raised arm, two fingers extended forward toward target
  - active (3f): F1 finger-trigger squeeze (caster side); F2 off-caster explosion at target (world VFX); F3 caster arm lowers
  - recovery (2f): arm drop, weight neutral -- Ranger planted at range
- **Total frames:** 7
- **Action phrase:** `raised arm two fingers extended forward then finger-trigger squeeze gesture remote detonation`
- **Priority:** P1
- **Keyframe needed:** NO -- 7 frames, gesture cast
- **Suggested method:** Animate with Text NEW (7 frames)

---

#### ranger_bone_trap

- **Role:** Control -- trap placement at cursor. Root + Mark applied when enemy crosses. Ranger reaches to belt/pouch, extends arm, drops trap at cursor zone.
- **Animation states:**
  - wind_up (2f): Ranger looks toward cursor, hand reaches to belt/pouch
  - active (2f): arm extends toward zone, trap drops on F2 (appears at cursor, caster does not move)
  - recovery (2f): arm pulls back, neutral distance stance resumed
- **Total frames:** 6
- **Action phrase:** `hand reaches to belt then arm extends dropping trap at cursor zone without moving toward target`
- **Priority:** P1
- **Keyframe needed:** NO -- 6 frames, reach-and-drop placement gesture
- **Suggested method:** Animate with Text NEW (6 frames). Trap world-object is a separate sprite.

---

#### ranger_sweep_volley

- **Role:** Pressure -- cone of 3-5 arrows swept left-to-right. Methodical hip pivot, NOT a rapid spray. Single sweeping draw-and-release fans arrows across cone.
- **Animation states:**
  - wind_up (2f): bow raised to left side of cone -- body rotates toward leftmost target; full draw, bow arm pointed hard left
  - active (4f): F1 left arrow release; F2 center pivot + center arrow release; F3 right pivot + right arrow release; F4 sweep follow-through completing right arc
  - recovery (2f): bow arm lowers from right, body returns to neutral forward facing
- **Total frames:** 8
- **Action phrase:** `bow raised hard left then methodical hip pivot sweeping bow arm right releasing arrows across arc`
- **Priority:** P1
- **Keyframe needed:** YES -- the pivot sweep across 4 active frames benefits from keyframed left-start and right-end poses
- **Suggested method:** Keyframe+Interpolate: POSE A = bow arm hard left (wind_up F2), POSE B = bow arm hard right (active F4). Interpolate A->B for active sweep (4f). Generate wind_up and recovery with Animate with Text.

---

#### ranger_hunters_step

- **Role:** Reposition. Tap: quick lateral dash step with next-attack-crit buff. Hold 0.3s: void phase step 4m along ground plane (gradual shimmer transparency, NOT a teleport warp).
- **Animation states:**
  - wind_up (2f): coiled weight shift, rear foot lifts slightly; hold adds 1 extra held-coil frame
  - active_tap (3f): quick lateral dash step; trailing earth-green afterimage; bow raised on F3 signaling crit buff
  - active_hold (4f): void entry shimmer F1; near-transparent void phase F2-F3 (ghost outline, alpha ~30%); exit rematerialization F4
  - recovery (2f): landing settle, weapon resettled
- **Total frames:** tap path = 7; hold path = 9
- **Action phrase (tap):** `quick lateral dash step with earth-green afterimage bow raised for crit-ready stance`
- **Action phrase (hold):** `gradual body shimmer dissolves into void phase step along ground plane then rematerializes`
- **Priority:** P1
- **Keyframe needed:** YES -- hold path transparency requires precise alpha control across 4 frames
- **Suggested method:** Generate tap path with Animate with Text (7f). Generate hold path separately with Keyframe+Interpolate for void phase transparency. Confirm PixelLab alpha channel support before dispatch.

---

#### ranger_predators_mark

- **Role:** Area Mark application -- cursor 4m AoE. Non-bow hand raised palm forward, ground reticle expands, Mark reticles appear on all enemies simultaneously. Focus 75+: 5 targets.
- **Animation states:**
  - wind_up (3f): Ranger raises non-bow hand, palm forward, scanning toward cursor zone; F3 peak palm-forward
  - active (3f): F1 ground reticle expands at cursor (world VFX); F2 peak expansion, Mark reticles drop on all enemies simultaneously; F3 caster hand drops
  - recovery (2f): caster hand lowers, scanning stance relaxes
- **Total frames:** 8
- **Action phrase:** `non-bow hand raised palm forward scanning zone then palm push as area reticle expands marking multiple targets`
- **Priority:** P1
- **Keyframe needed:** NO -- 8 frames, palm-raise and push gesture
- **Suggested method:** Animate with Text NEW (8 frames). World VFX reticles are separate assets.

---

#### ranger_wireline_trap

- **Role:** Control -- two anchor points placed at cursor, tensioned wire between them. Crossing enemies get Snare + Mark for 8s.
- **Animation states:**
  - wind_up (2f): Ranger reaches to belt, draws anchor spike in non-bow hand
  - active_point1 (2f): arm extends, first anchor planted at cursor point A on F2
  - active_point2 (2f): arm re-extends, second anchor planted at cursor B on F2; wire snaps between A and B on this frame
  - recovery (2f): hand returns, Ranger steps back to observe zone
- **Total frames:** 8
- **Action phrase (two-step):** `reach to belt draw anchor then arm extends planting first anchor then second anchor placing tensioned wire between two points`
- **Priority:** P1
- **Keyframe needed:** NO -- 8 frames, dual anchor placement sequence
- **Suggested method:** Animate with Text NEW (8 frames -- treat as one continuous placement sequence). Wire world-object is a separate sprite.

---

#### ranger_final_strike

- **Role:** Signature execution shot. Requires BOTH Mark AND Trap active on target. 400% damage. Slow deliberate full-draw, planted stance, arrow departs, dual-state consumed simultaneously at impact.
- **Animation states:**
  - wind_up (4f): full draw, very deliberate; Ranger lowers eye to arrow, weight fully planted; F4 peak-hold at max tension
  - active (3f): F1 release (arrow departs); F2 arrow impact + Mark reticle implosion + Trap snap-burst simultaneously; F3 camera-hold (damage number)
  - recovery (2f): bow arm lowers slowly, deliberate calm
- **Total frames:** 9
- **Action phrase:** `slow deliberate full-draw planted stance lowering eye to arrow then single decisive release`
- **Priority:** P1
- **Keyframe needed:** YES -- 4-frame wind_up with peak-hold requires keyframe at apex draw pose
- **Suggested method:** Keyframe+Interpolate: POSE A = neutral, POSE B = full draw peak-hold. Interpolate A->B for wind_up (4f). Generate active and recovery with Animate with Text.

---

#### ranger_spirit_bow

- **Role:** V Burst ultimate (6s). Spirit bow manifests over physical bow. Every shot auto-applies Mark. 13 total frames -- justified by ultimate tier.
- **Animation states:**
  - wind_up (3f): both arms raise, spirit bow materializes over physical bow; F3 peak spirit-bow-full-form hold
  - active_entry (2f): snap to firing stance, spirit bow fully visible, first arrow nocked, eyes forward
  - loop (3f): idle-with-spirit-bow, slight sway, spirit bow maintains glow; methodical ready
  - active_fire (3f): per-shot during V Burst -- 1f draw, 1f release, 1f follow-through; faster than standard shot; spirit bow glows on release
  - recovery (2f): spirit bow dissolves (fade over 2 frames), physical bow remains, Ranger lowers weapon
- **Total frames:** 13
- **Action phrase (wind_up):** `both arms raise as spirit bow materializes as semi-transparent overlay over physical bow`
- **Action phrase (active_fire):** `rapid bow draw release follow-through with spirit bow glowing at release`
- **Priority:** P1
- **Keyframe needed:** YES -- 13 frames, multiple distinct phases, spirit bow overlay transparency
- **Suggested method:** Split into chunks. Wind_up: Keyframe+Interpolate (POSE A = neutral, POSE B = arms raised, spirit bow forming). Loop and active_fire: Animate with Text NEW (6 frames combined). Recovery: Animate with Text (2f fade).

---

### SHADOWBLADE

**Lore:** Shadow assassin. Daggers (typically dual). Phase-through geometry -- body dissolves and rematerializes rather than teleporting. Scar is the exclusive mechanic: diagonal black-violet gash decals placed on enemy bodies, collapsed for massive payoff.

**Weapon constraint:** Dual daggers -- reverse-grip is typical. Specify which hand leads for each skill. During phase steps and void transitions, weapon stays in hand (does not disappear).

**Palette:** Violet-black dominant. Low-saturation overall. Single bright accent on blade edge (thin violet-white highlight). Shadow energy: dark violet mist. Gash decals: black-violet.

**Reference sprite:** `Assets/Sprites/Shadowblade/shadowblade_neutral_S43.png`

**Critical identity rule:** Phase movement is NOT a blink-flash teleport. Body DISSOLVES along a path and REMATERIALIZES -- the arc is visible as a ghost outline during transit. This distinguishes Shadowblade from Ronin.

---

#### shadowblade_veil_strike

- **Role:** Basic LMB attack -- opener/pressure. Reverse-grip slash, places mark on hit, Sever resource +8 per hit. LMB x3 + Hold 0.3s triggers Twin Carve variant (2-slash combo + phase-step backward).
- **Animation states (base):**
  - wind_up (2f): reverse-grip blade drawn back, weight shifted
  - active (3f): F1 slash extension (reverse-grip, upward diagonal), F2 contact frame with mark flash, F3 return arc
  - recovery (2f): blade returns to reverse-grip guard
  - cancel (1f): momentum redirect allowed after F1 for chaining
- **Animation states (Twin Carve variant):**
  - loop (2f): hold-charge shimmer on blade, body lowers
  - active_twin (4f): F1-F2 two rapid crossing slashes, F3 phase-step backward begins (body half-dissolves), F4 rematerializes behind
  - recovery_twin (2f): rematerialized stance settle
- **Total frames:** 16 (8 base + 8 variant)
- **Action phrase (base):** `reverse-grip blade drawn back then upward diagonal slash with contact flash`
- **Action phrase (twin carve):** `two rapid crossing slashes then body dissolves backward rematerializing behind departure point`
- **Priority:** P0 -- basic skill, blocks Shadowblade playable build
- **Keyframe needed:** YES -- 16 frames total, phase-step transparency in Twin Carve requires precise alpha
- **Suggested method:** Generate base sequence with Animate with Text NEW (8f). Generate Twin Carve separately with Keyframe+Interpolate for the phase-step transition (active_twin F3-F4).

---

#### shadowblade_phase_step

- **Role:** Basic reposition. Short dash with 0.3s invisibility on exit. Body dissolves mid-dash, rematerializes at endpoint. Ghost outline (~30% alpha) visible during void phase.
- **Animation states:**
  - wind_up (2f): lean in dash direction, body edge begins dissolving on F2
  - active (3f): F1 body fully dissolved (ghost outline only), F2 mid-dash void (near-transparent outline for player read), F3 rematerialization at endpoint
  - recovery (2f): stance settle post-rematerialization
- **Total frames:** 7
- **Action phrase:** `body edge dissolves into ghost outline dashes through space then rematerializes at endpoint`
- **Priority:** P0 -- basic mobility skill, blocks Shadowblade playable build
- **Keyframe needed:** YES -- transparency alpha on F1-F2 requires controlled interpolation
- **Suggested method:** Keyframe+Interpolate: POSE A = solid caster, POSE B = near-transparent ghost outline. Interpolate A->B for dissolve (wind_up F2 + active F1-F2). Generate rematerialization and recovery with Animate with Text. Confirm alpha support.

---

#### shadowblade_backstab_mark

- **Role:** Passive overlay -- guaranteed crit when hitting marked enemy from behind. 2-frame passive overlay, does not count against skill quota.
- **Animation states:**
  - active (2f): F1 backstab contact frame from behind, brief violet edge-glow on blade; F2 crit flash + mark consumed (mark snaps off target)
- **Total frames:** 2
- **Action phrase:** `backstab contact behind enemy with violet blade-edge glow then crit flash mark consumed`
- **Priority:** P1 -- passive, requires mark system functional
- **Keyframe needed:** NO -- 2 frames, passive overlay
- **Suggested method:** Animate with Text NEW (2 frames)

---

#### shadowblade_scarbinding

- **Role:** Scar placement -- phases PAST target (not at them), leaving Scar decal anchored on enemy. This is placement only; collapse is a separate skill.
- **Animation states:**
  - wind_up (2f): low crouch, blade angled back, body half-dissolved
  - active (4f): F1 enter (silhouette dissolves), F2 mid-pass (overlap with target, both half-rendered, Scar placed), F3 emerge behind, F4 turn-back glance blade still extended
  - recovery (2f): exhale, body rematerializes, blade lowered
- **Total frames:** 8
- **Action phrase:** `body dissolves phases through target leaving scar mark then emerges behind with turn-back glance`
- **Priority:** P1 -- batch-paired with Scar Collapse; neither dispatches without the other
- **Keyframe needed:** YES -- phase-through overlap in F2 (caster partially over target) requires precise transparency control
- **Suggested method:** Keyframe+Interpolate: POSE A = dissolved approach, POSE B = mid-overlap (half-rendered on target), POSE C = emerged behind. Interpolate A->B->C for active frames. Scar decal is a separate 2-frame micro-asset (pulse + idle).

---

#### shadowblade_scar_collapse

- **Role:** Signature detonation -- requires 3+ Scars on target. Geometric arc drawn connecting all Scar points, then all Scars implode simultaneously inward. No weapon swing -- the geometry is the attack.
- **Animation states:**
  - wind_up (2f): open hand raised, fingers spread toward target -- geometric intent, not weapon threat
  - active (4f): F1 arc line trace (thin violet trace connecting all Scar points); F2 simultaneous Scar implosion on target; F3 gash lines close inward; F4 void-dark afterimage on target silhouette
  - recovery (2f): caster exhales, hand drops, stance resets
- **Total frames:** 8
- **Action phrase:** `open hand raised toward target drawing geometric arc as all scar marks implode simultaneously inward`
- **Priority:** P1 -- batch-paired with Scarbinding
- **Keyframe needed:** NO -- 8 frames, open-hand trace gesture
- **Suggested method:** Animate with Text NEW (8 frames). Scar implosion VFX reuses the Scarbinding decal in reverse.

---

#### shadowblade_shadow_clone

- **Role:** Signature misdirection -- spawns decoy phantom at target point. Clone mirrors caster idle, draws enemy attention, no combat state. Clone loops until hit or duration end.
- **Animation states (caster):**
  - wind_up (2f): hand extends, shadow-matter pools at target point
  - active (3f): F1 clone coalesces (shadow to solid outline), F2 clone fully formed (idle pose), F3 caster returns to guard
  - recovery (2f): caster guard resumes
- **Animation states (clone sub-asset -- separate sheet):**
  - loop (3f): clone idle -- slow breath, blade at low guard
  - dissipate (2f): clone dissolves solid -> ghost outline -> gone
- **Total frames:** 7 caster + 5 clone
- **Action phrase (caster):** `hand extends toward target as shadow-matter coalesces into standing phantom clone`
- **Action phrase (clone idle):** `standing at low guard idle slow breath, faint violet-dark outline`
- **Priority:** P1 -- requires phantom entity rendering pipeline
- **Keyframe needed:** NO -- clone loop and dissipate are simple
- **Suggested method:** Animate with Text NEW for caster (7f). Generate clone loop and dissipate as a separate mini-sheet (2 rows, 5f). Clone uses same Shadowblade reference sprite but desaturated/tinted.

---

#### shadowblade_death_mark

- **Role:** Signature delayed detonation -- projects thin ray from blade tip placing a 4s countdown mark on target. Mark auto-detonates. Urgency communicated by pulse frequency on target.
- **Animation states (caster):**
  - wind_up (2f): blade tip angled at target, body still -- precision; minimal motion
  - active (3f): F1 thin violet ray from blade to target; F2 mark seals on target (placement); F3 caster withdraws blade
  - recovery (2f): guard resume
- **Animation states (mark sub-asset -- separate sheet):**
  - mark_idle (3f): mark pulses on target: F1 dim, F2 mid-bright, F3 bright-peak; loops; pulse rate increases as 4s approaches
  - mark_detonate (3f): F1 mark flares white-violet; F2 implosion burst (inward); F3 void-dark smoke residual
- **Total frames:** 7 caster + 6 mark
- **Action phrase (caster):** `blade tip angled at target body still then thin violet ray projected placing countdown mark`
- **Priority:** P1 -- requires timer system functional
- **Keyframe needed:** NO -- 7 caster frames, precision stillness gesture
- **Suggested method:** Animate with Text NEW for caster (7f). Generate mark sub-asset separately (2 rows, 6f total). Mark_detonate uses inward implosion -- NOT outward explosion.

---

#### shadowblade_shadow_pin

- **Role:** Signature control -- thrown dagger roots target 1.5s. Shadow-anchor blooms at feet of target. Root is shadow-geometry, not physical pin.
- **Animation states (caster):**
  - wind_up (2f): reverse-grip dagger drawn back to throwing position, weight shifts
  - active (3f): F1 release frame (dagger departs); F2 mid-flight; F3 impact + shadow-anchor bloom expands at target feet
  - recovery (2f): throwing arm returns to guard
- **Animation states (root sub-asset -- separate sheet):**
  - root_idle (3f): shadow-anchor blooms and loops at ground under target; dark shadow-anchor expands
  - root_expire (2f): shadow-anchor shrinks and dissolves
- **Total frames:** 7 caster + 5 root
- **Action phrase:** `reverse-grip dagger drawn back then release throw with dagger arc to target planting shadow-anchor at feet`
- **Priority:** P1 -- root requires enemy movement-lock system
- **Keyframe needed:** NO -- 7 frames, reverse-grip throw
- **Suggested method:** Animate with Text NEW (7f). Root sub-asset generated separately (2 rows, ground-plane decal). Dagger projectile may need standalone 1-frame in-flight asset.

---

#### shadowblade_night_aperture

- **Role:** Advanced buff (6s) -- each dash during buff spawns Scar at entry AND exit points. Faint violet rim light on caster during buff. Passive Scar multiplication -- predator identity.
- **Animation states:**
  - wind_up (2f): brief crouch, blade drawn back, body edge dims -- signals buff activation
  - active (3f): buff active loop -- idle variant with faint violet rim on limbs + shadow-thread trail at feet; 1-frame entry pulse on buff activate
  - recovery (2f): exhale, buff fade, shadow-thread at feet dissipates
- **Note:** dash_scar_spawn (2f world-space event) fires per dash during buff -- these are world-space decal pops at origin and destination, not caster animation frames. Reuse SHADOWBLADE_SCARBINDING Scar decal, no new gen needed.
- **Total frames:** 7 caster frames
- **Action phrase:** `brief crouch blade drawn back then buff active idle with faint violet rim light and shadow-thread trail at feet`
- **Priority:** P1 -- Scar-multiplication tool, pairs with Scar Collapse for combo payoff
- **Keyframe needed:** NO -- 7 frames, buff activation
- **Suggested method:** Animate with Text NEW (7f). Scar decal reused from Scarbinding -- zero additional gen cost.

---

#### shadowblade_smoke_veil

- **Role:** Signature reposition/concealment. Releases shadow-smoke burst around caster, applies stealth. Caster fades into smoke (~20% alpha ghost outline). Deliberate concealment, not explosion.
- **Animation states:**
  - wind_up (2f): hand draws across body gathering shadow-smoke, body compresses inward
  - active (3f): F1 smoke burst releases outward (controlled unfurl, NOT explosion ring); F2 caster begins fade (alpha drops); F3 caster at full stealth alpha within smoke
  - recovery (2f): caster at stealth -- near-invisible faint outline
  - loop (3f): stealth idle -- caster as ghost outline within smoke, minimal movement
  - exit_stealth (2f): smoke dissipates edge-fade, caster rematerializes solid
- **Total frames:** 12
- **Action phrase:** `shadow-smoke unfurls outward from caster body as caster fades into ghost outline within smoke cloud`
- **Priority:** P1 -- smoke AoE environment requires particle system
- **Keyframe needed:** YES -- deep alpha fade across multiple frames requires controlled interpolation
- **Suggested method:** Keyframe+Interpolate for fade sequence (active F2-F3 + recovery). Generate wind_up, loop, and exit with Animate with Text separately.

---

#### shadowblade_veil_burst

- **Role:** Ultimate -- 4 radial phase-strikes around caster. Body dissolves from center, appears and strikes at 4 radial points (N/S/E/W), returns to origin. Identity is the PATTERN, not individual strikes.
- **Animation states:**
  - wind_up (3f): body edges dissolve outward in 4 radial hints -- shows 4-direction intent in silhouette
  - active (6f): F1 phase-out (body dissolves from center); F2 strike N; F3 strike S; F4 strike E; F5 strike W; F6 return phase-in at origin
  - recovery (3f): body fully rematerializes, stance lowers, heavy breath
- **Total frames:** 12
- **Action phrase:** `body dissolves from center then emerges and strikes at four radial points north south east west then returns to origin`
- **Priority:** P2 -- ultimate, blocks late-game build but not playable prototype
- **Keyframe needed:** YES -- 12 frames, each of F2-F5 must show caster at a distinct radial position (N/S/E/W quadrant); keyframes lock spatial positions
- **Suggested method:** Keyframe+Interpolate: generate 4 distinct strike poses (one per direction) as static sprites. Chain into sequence. Arc trail is mandatory VFX connecting all 4 positions -- without it, looks like 4 separate blink-flashes.

---

#### shadowblade_chain_cull

- **Role:** Ultimate -- 3-hop phase-lunge chain between up to 3 marked enemies. Each hop: dissolve, arc mid-ghost visible, emerge and strike at target. Consumes mark per hop. Arc MUST be visible (distinguishes from Ronin blink).
- **Animation states:**
  - wind_up (2f): crouch-lock onto first target, blade cocked, body edge dissolves
  - active_hop (4f per hop, x3): F1 phase-out (dissolve); F2 lunge arc mid-point (ghost outline visible along arc); F3 phase-in + emerge (solidify at target); F4 strike frame
  - recovery (3f): final hop, body fully rematerializes, stance drops low
- **Total frames:** 17 (2+12+3) -- within 18-frame cap
- **Action phrase (hop):** `body dissolves then ghost outline arc traverses to marked target then rematerializes with strike`
- **Priority:** P2 -- ultimate, requires marked-enemy targeting system
- **Keyframe needed:** YES -- arc ghost visibility in F2 requires careful transparency; active_hop generated once and tiled x3 in engine
- **Suggested method:** Generate active_hop (4f) once with Keyframe+Interpolate for the arc ghost. Engine instantiates 3 times. Generate wind_up and recovery with Animate with Text. Confirm engine supports hop-chaining animation replay.

---

#### shadowblade_scarbinding (note: separate entry from scarbinding above -- same skill)

This entry is already documented above under `shadowblade_scarbinding`. No duplication needed.

---

## Production Order Recommendation

### P0 -- Must complete first (blocks playable builds)

| # | Character | Skill | Frames | Method | Notes |
|---|---|---|---|---|---|
| 1 | Warblade | iron_counter | 7 | Keyframe+Interpolate | Rule 57 counter identity audit |
| 2 | Warblade | iron_charge | 11 | Keyframe+Interpolate + Animate with Text | Gap-closer, blocks build |
| 3 | Warblade | sunder_mark | 4 | Animate with Text NEW | Sundered origin skill |
| 4 | Elementalist | fireball | 4 | Animate with Text NEW | Fire orb, simplest skill in kit |
| 5 | Ranger | ranger_rift_arrow | 5 | Animate with Text NEW | LMB basic, simplest |
| 6 | Shadowblade | shadowblade_veil_strike | 16 | Keyframe+Interpolate | Basic + Twin Carve variant |
| 7 | Shadowblade | shadowblade_phase_step | 7 | Keyframe+Interpolate | Mobility, blocks build |

### P1 -- Core skills (required for class function and synergy testing)

Ordered simple-first within priority:

| # | Character | Skill | Frames | Method |
|---|---|---|---|---|
| 8 | Warblade | crippling_blow | 6 | Animate with Text NEW |
| 9 | Warblade | iron_crush | 6 | Animate with Text NEW |
| 10 | Warblade | deep_wound | 6 | Animate with Text NEW |
| 11 | Elementalist | glacial_spike | 4 | Animate with Text NEW |
| 12 | Elementalist | living_bomb | 6 | Animate with Text NEW |
| 13 | Elementalist | frozen_orb | 5 | Animate with Text NEW |
| 14 | Elementalist | frost_wall | 5 | Animate with Text NEW |
| 15 | Ranger | ranger_pinning_shot | 7 | Animate with Text NEW |
| 16 | Ranger | ranger_marked_detonate | 7 | Animate with Text NEW |
| 17 | Ranger | ranger_bone_trap | 6 | Animate with Text NEW |
| 18 | Ranger | ranger_predators_mark | 8 | Animate with Text NEW |
| 19 | Shadowblade | shadowblade_backstab_mark | 2 | Animate with Text NEW |
| 20 | Shadowblade | shadowblade_night_aperture | 7 | Animate with Text NEW |
| 21 | Shadowblade | shadowblade_scar_collapse | 8 | Animate with Text NEW |
| 22 | Warblade | ironclad_momentum | 7 | Animate with Text NEW |
| 23 | Warblade | earthsplitter | 9 | Keyframe+Interpolate |
| 24 | Warblade | gravity_cleave | 7 | Keyframe+Interpolate |
| 25 | Warblade | death_blow | 9 | Keyframe+Interpolate |
| 26 | Elementalist | prism_beam | 8 | Animate with Text NEW (16f canvas) |
| 27 | Elementalist | meteor | 6 | Keyframe+Interpolate |
| 28 | Elementalist | blizzard | 7 | Keyframe+Interpolate |
| 29 | Ranger | ranger_sweep_volley | 8 | Keyframe+Interpolate |
| 30 | Ranger | ranger_hunters_step | 7/9 | Keyframe+Interpolate (hold path) |
| 31 | Ranger | ranger_final_strike | 9 | Keyframe+Interpolate |
| 32 | Ranger | ranger_spirit_bow | 13 | Keyframe+Interpolate (split into chunks) |
| 33 | Ranger | ranger_wireline_trap | 8 | Animate with Text NEW |
| 34 | Shadowblade | shadowblade_scarbinding | 8 | Keyframe+Interpolate |
| 35 | Shadowblade | shadowblade_death_mark | 7 | Animate with Text NEW |
| 36 | Shadowblade | shadowblade_shadow_pin | 7 | Animate with Text NEW |
| 37 | Shadowblade | shadowblade_shadow_clone | 7 | Animate with Text NEW |
| 38 | Shadowblade | shadowblade_smoke_veil | 12 | Keyframe+Interpolate |

### P2 -- Advanced/late skills (secondary priority)

| # | Character | Skill | Frames | Method |
|---|---|---|---|---|
| 39 | Warblade | battle_surge | 8 | Animate with Text NEW |
| 40 | Elementalist | solar_flare | 7 | Animate with Text NEW |
| 41 | Elementalist | radiant_pillar | 8 | Animate with Text NEW |
| 42 | Elementalist | element_charge | 7 | Animate with Text NEW |
| 43 | Shadowblade | shadowblade_veil_burst | 12 | Keyframe+Interpolate |
| 44 | Shadowblade | shadowblade_chain_cull | 17 | Keyframe+Interpolate |

---

## Checklist Before Each Animation

- [ ] Canvas set to 252px in PixelLab (crop to 128px per frame in post-processing, NOT before animating)
- [ ] Reference sprite uploaded (correct character, correct direction -- check S43 direction offset table)
- [ ] Core constraints block included in prompt (FOOTPRINT LOCK + ANCHOR + CONTINUITY + NO EMBEDDED VFX + full body centered same scale)
- [ ] Character-specific weapon constraint added (Warblade: both hands on hilt; Elementalist: hands open palm-out; Ranger: bow drawn or at rest; Shadowblade: reverse-grip dagger specified)
- [ ] Action phrase is specific to this skill (not generic)
- [ ] Frame count is appropriate for animation type (see Frame Count Guidelines table)
- [ ] Method confirmed: Animate with Text NEW (<=8f simple) or Keyframe+Interpolate (complex or >8f)
- [ ] Direction generated: S first (front-facing), then E, N, W separately -- do NOT mirror
- [ ] After generation: verify NO embedded sparks, trails, or VFX baked into frames
- [ ] After generation: verify character design, armor, weapon, and palette are identical across all frames (continuity check)
- [ ] Import check: PPU=64, Multiple mode, 128x128 rect, center pivot (0.5, 0.5), Point filter, Uncompressed

---

## Batch Dependencies (Skills That Must Ship Together)

| Batch | Skills | Reason |
|---|---|---|
| Batch A | ranger_rift_arrow + Mark reticle micro-asset | Mark reticle needed for all Mark skills |
| Batch B | ranger_bone_trap + ranger_wireline_trap | Shared bone/sinew world-object visual language |
| Batch C | ranger_marked_detonate + ranger_bone_trap + ranger_final_strike | Dual-consume VFX chain |
| Batch D | shadowblade_scarbinding + shadowblade_scar_collapse | Scar placement and collapse are one system |
| Batch E | shadowblade_scarbinding + shadowblade_night_aperture | Night Aperture reuses Scarbinding Scar decal |

---

*This document is the complete production reference for Wave 1 animation. Pass it to ChatGPT or Claude as context for each session.*
