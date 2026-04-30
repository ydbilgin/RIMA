# RIMA Skill Sheet Identity Feedback to Claude
Date: 2026-04-30
Author: Codex
Input folder: `C:\Users\ydbil\OneDrive\Masaustu\RIMA_SPRITESHEET`
Compared against: `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`, `CODEX.md`, PixelLab memory files

## Executive Verdict

The 10 skill sheets are visually strong as concept/showcase boards, but they should not be treated as production-ready skill identity locks.

Main issue: many skills read as "same character + same class color + bigger slash/explosion" instead of distinct gameplay actions. RIMA's skill design is state-driven and verb-driven, but several sheets communicate mostly VFX volume, not mechanical role.

The sheets are useful for:
- mood and style reference
- rough class fantasy validation
- identifying which VFX palettes feel strong
- showing the professor/non-dev reviewers that each class has a visual identity

The sheets are not sufficient for:
- final production spritesheets
- per-skill animation locks
- 128x128 gameplay readability
- deciding that every listed skill is visually distinct enough in actual combat

Important technical note: every sheet is `1774x887`, not a 128x128 frame grid. These are concept boards, not importable Unity animation sheets.

## Highest Priority Feedback

### 1. Skill identity is too often color-based

Within each class, color is doing most of the identity work:
- Shadowblade = purple slash/phase
- Ravager = red blood impact
- Brawler = beige/white punch impact
- Hexer = green curse explosion
- Warblade = blue steel slash + ground crack

That makes class identity strong, but skill identity weak.

RIMA needs each skill to separate by at least one of these:
- pose: what the character is physically doing
- shape: line, cone, ring, trap, mark, field, projectile, tether, summon
- state: Broken, Scar, Marked, Hexed, Launched, Corpse Field, etc.
- scale tier: basic/core/payoff/V burst

Current problem: several skills differ by intensity, not by behavior.

### 2. State language is underrepresented

The design doc is now state-based, but the sheets still mostly show "attack moment" illustrations.

Missing/weak visual states:
- Warblade: Broken/Sundered armor readable on enemy
- Shadowblade: persistent Rift Scar left in space
- Ranger: Marked/Trapped distinction
- Brawler: Cracked/Shattered/Off-Balance/Pinned body state
- Gunslinger: Suppressed/Exposed Line/Heat reset
- Hexer: stack thresholds or Overloaded state

If these are not visible, the player will see many skills as generic attacks.

### 3. Some sheets use obsolete or partially drifted skill naming

This is not fatal because the sheets are likely concept selections, but Claude should not treat them as exact canonical lists.

Examples:
- Ronin sheet includes `Mugen no Kiri`, while canonical V Burst in the doc is not presented that way in the current table.
- Warblade sheet includes old-style `Death Blow` fantasy, but R4/Master says HP<30 execute gates are forbidden and executes must be state-gated.
- Gunslinger sheet still gives `Deadshot` strong line identity, but `Point Blank Execute` should not remain a separate major visual family if Claude keeps the R4 consolidation direction.
- Several sheets show 8 skills, while the design doc holds 12 skills + basics/V/R4 extras. These are representative boards, not full skill manifests.

## Class-by-Class Review

### Warblade

Visual quality: high.
Class fantasy: mostly correct.
Production risk: medium.

What works:
- Heavy melee impact reads well.
- Blue steel/rift accent separates Warblade from Ravager.
- `Bladestorm` has clear V burst scale.
- `Rage Outlet` and `Gravity Cleave` feel heavy and readable.

Main problem:
Too many skills are ground impact / sword arc / crack effects. `Momentum Slam`, `Iron Charge`, `Earthsplitter`, and parts of `Gravity Cleave` can collapse into the same combat read: "big melee impact with rocks."

Specific concerns:
- `Iron Combo` is acceptable as basic, but should stay smaller in final production.
- `Momentum Slam` and `Iron Charge` need a clearer role split. One should read as shoulder/entry impact; the other as weapon-led engage.
- `Earthsplitter` should be line crack/knockup, not just another radial ground slam.
- `Death Blow` should visually depend on Broken/Sundered target state. It should show fractured armor or a crack window, not only a red blood execution.

Suggested visual rule:
Warblade should not be "CC king with many ground slams." It should be "enemy-state opener." Every major Warblade payoff should show armor crack, exposed plates, or a Sundered window.

PixelLab feasibility:
- High for sword arcs, blue steel cracks, and impact poses.
- Medium for precise Broken/Sundered enemy-state indicators; PixelLab may draw random cracks but not consistent state language.
- Recommendation: generate the character pose/impact in PixelLab, but implement Broken/Sundered icons/overlays/armor shard VFX in Unity.

### Elementalist

Visual quality: very high.
Class fantasy: strong.
Production risk: medium-low.

What works:
- Best overall elemental readability.
- Fire/Frost/Light palette is clear.
- `Element Switch` is excellent as a system illustration.
- `Trinity Storm` is visually distinct and reads as a V burst.

Main problem:
Some spell families can still blur into "colored magic projectile/beam."

Specific concerns:
- `Rift Bolt`, `Fireball`, and `Prism Beam` need sharper shape separation.
- `Fireball` should be a discrete projectile.
- `Prism Beam` should be a clean line/channel beam, not a rainbow spray that feels like generic magic.
- `Glacial Spike` is strong visually, but future Frost skills must avoid all becoming "ice eruption."

Suggested visual rule:
Elementalist should be the shape class. Each element should have a different geometry:
- Fire = projectile/meteor/explosion
- Frost = wall/spike/orb/control shape
- Light = beam/pillar/radiant echo

PixelLab feasibility:
- High for static elemental VFX concepts.
- Medium for exact beam/wall/rune interaction concepts.
- Low for gameplay-accurate refraction, beam bouncing, or targetable rune logic as a single generated asset.
- Recommendation: use PixelLab for peak frames and spell icons; use Unity for beam path, wall collision, rune triggers, Lightbreak UI, and readable state overlays.

### Shadowblade

Visual quality: good.
Class fantasy: partially correct.
Production risk: high.

What works:
- Purple void identity is strong.
- Character silhouette reads agile.
- `Death Mark` and `Wraith Form` are the clearest pieces.

Main problem:
Too many skills are purple slash + ghost trail. The redesign in the doc is about phase/scar/collapse, but the sheet still reads like a stylish rogue/assassin with void VFX.

Specific concerns:
- `Veil Strike`, `Veil Flicker`, `Seam Rend`, and `Phase Step` are too close visually.
- The player needs to see "a wound left in space," not only motion blur.
- `Rift Scar` should be a persistent, thin, spatial cut mark that remains after the hit.
- `Collapse` should have a different visual from applying Scar.

Suggested visual rule:
Shadowblade needs three distinct visual verbs:
- phase = body passing through / transparent silhouette
- scar = stationary rift wound left behind
- collapse = scar implosion or snap closure

PixelLab feasibility:
- Medium for cool phase poses and purple slashes.
- Low-medium for consistent persistent Rift Scar language across many skills.
- Recommendation: PixelLab can make the character pose and peak slash. Unity should own Scar decals, collapse timing, teleport anchors, and enemy state indicators.

### Ranger

Visual quality: good.
Class fantasy: mostly correct.
Production risk: medium.

What works:
- White hair + tactical archer silhouette reads clearly.
- `Bone Trap` is one of the better state/setup visuals.
- `Spirit Bow` is readable as a burst.
- `Tactical Roll` and `Hunter's Step` at least show different body motion.

Main problem:
Too many skills still read as "shoot arrow." The design doc moved Ranger toward mark/trap/detonate, but the sheet still leans heavily into arrow delivery.

Specific concerns:
- `Rift Arrow`, `Pinning Shot`, `Sweep Volley`, and `Final Strike` need more visual role separation.
- `Marked` is not strongly visible.
- Trap identity is weak outside `Bone Trap`.
- `Final Strike` should not visually imply a generic low-HP execute; it should depend on Marked/Trapped or long-range condition.

Suggested visual rule:
Ranger should show three families:
- mark = target sigil / gold-green reticle
- trap = ground device, wireline, snare geometry
- detonate = mark shatter / chain reaction

PixelLab feasibility:
- High for archer poses, volley, spirit bow.
- Medium for trap concepts.
- Low for clean two-point wireline/trap readability at 128px if left entirely to PixelLab.
- Recommendation: generate trap/pose concepts in PixelLab, but draw trap lines, mark reticles, and detonation indicators as Unity VFX/overlays.

### Ravager

Visual quality: high impact.
Class fantasy: strong but too narrow.
Production risk: medium-high.

What works:
- Brutality is clear.
- Red/blood palette separates from Warblade.
- `Blood Pact` is visually distinct and should be kept as a direction.
- `Berserk Mode` reads as a major state.

Main problem:
Almost every skill reads as red blood violence. The class fantasy is not just damage; it is suffer/trade/frenzy. The sheet underplays the "trade" part and overplays blood splashes.

Specific concerns:
- `Brutal Swing`, `Bloodlust Strike`, `Frenzied Leap`, and `Blood-Drunk Leap` risk feeling like variants of "hit hard with axe."
- `Fury Tackle` should be shoulder/body impact, not another red weapon slash.
- `Blood Pact` should be the template for HP trade visuals: self-cost, rune/blood debt, controlled pain.
- `Berserk Mode` is good but must not become identical to Warblade Bladestorm or Brawler Overdrive as "red aura = stronger."

Suggested visual rule:
Ravager needs three visual languages:
- suffer = self-wound, blood debt, controlled hit acceptance
- trade = cost marker, pact sigil, HP sacrifice
- frenzy = violent payoff after risk

PixelLab feasibility:
- High for brutal poses and blood effects.
- Medium for self-damage/cost communication.
- Low for precise "perfect hurt window" or Blood Debt mechanic in a static generated frame.
- Recommendation: PixelLab for pose and mood; Unity UI/VFX must show Fury gain, Blood Debt, self-cost, and cooldown/abuse locks.

### Ronin

Visual quality: good.
Class fantasy: elegant but too safe.
Production risk: medium.

What works:
- Clean white/silver slash identity is strong.
- `Sheath Walk`, `Quickdraw Slash`, and `Void Cleave` give a restrained samurai direction.
- Less noisy than other sheets, which is good for Ronin.

Main problem:
The wait/draw/punish fantasy is not visible enough. Several skills are white motion slash variants.

Specific concerns:
- `Drawn Edge`, `Quickdraw Slash`, `Iaido Blur`, and `Haste Dash` are close as fast slash motions.
- Ronin's core drama should be the pre-draw moment, not just the cut.
- `Sakura Veil` / counter timing is not represented in this 8-skill sheet, but it is important to distinguish Ronin from Warblade/Brawler.
- `Mugen no Kiri` naming may not be canonical; verify before using in docs or prompts.

Suggested visual rule:
Ronin should show negative space and timing:
- wait = sheathed stillness, tension ring, quiet petals
- draw = single sharp line, minimal VFX
- punish = enemy opened state / delayed cut

PixelLab feasibility:
- High for elegant katana poses.
- Medium for stance and tension concepts.
- Low for frame-perfect timing, Opened state, and counter feedback if relying only on generated art.
- Recommendation: PixelLab should create 1-2 clean key poses; Unity must carry hit-stop, afterimage timing, thin slash line, and Opened-state cue.

### Gunslinger

Visual quality: good.
Class fantasy: mostly strong.
Production risk: medium.

What works:
- Female gunslinger identity reads clearly.
- Yellow/brass muzzle flash language is strong.
- `Cursor Storm`, `Deadshot`, and `Full Metal Storm` are among the clearest visuals.
- The class does not look neon/cyberpunk; this is good for RIMA tone.

Main problem:
Several pistol skills still risk collapsing into "shooting guns." The class needs reload/heat rhythm, not only muzzle flashes.

Specific concerns:
- `Dual Fire`, `Hip Shot`, `Quickdraw`, and `Deadshot` need stronger gameplay separation.
- `Rift Dash` currently uses purple slash-like energy, which can visually drift toward Shadowblade.
- `Deadshot` should remain the precision long-line execute/payoff.
- `Hip Shot` should be close panic/side-slide, not another normal shot.
- Reload rhythm is missing from the sheet, even though it is central to the revised class identity.

Suggested visual rule:
Gunslinger should separate by gun handling:
- shoot = muzzle flash / line
- slide = low body displacement / dust trail
- reload = cylinder/magazine/empty-shell motion
- overheat = weapon glow + Heat venting

PixelLab feasibility:
- High for muzzle flashes, poses, and bullet storm concepts.
- Medium for reload poses if prompted separately.
- Low-medium for exact Heat/Overheat state and perfect reload timing as generated art.
- Recommendation: generate separate reload/empty-mag/keyframe concepts. Use Unity for Heat bar, Exposed Line, bullet tracers, casing particles, and reload timing feedback.

### Brawler

Visual quality: medium.
Class fantasy: weakest visual differentiation.
Production risk: high.

What works:
- Physical impact direction is clear.
- The no-magic dust/impact palette is good.
- `Flying Knee` and `Overdrive` are readable.

Main problem:
Too many skills are "man punches enemy." Brawler has the highest risk of looking underpowered or generic compared to the flashier classes.

Specific concerns:
- `Jab`, `Mach Punch`, and `Pivot Hook` are too close.
- `Weave` should be body movement / evasion / whiff punish, but currently looks like another motion blur.
- `Shockwave Slam` is readable but can overlap Warblade/Ravager ground slam language.
- `Aerial Rave` needs the enemy launch/juggle state to read much more clearly.
- Brawler's unique states (Cracked, Shattered, Off-Balance, Pinned, Launched) are not visible enough.

Suggested visual rule:
Brawler needs body-mechanics separation, not bigger punch effects:
- Jab = tiny fast impact
- Mach Punch = multi-arm/afterimage barrage
- Pivot Hook = feet planted, hip rotation, one huge side impact
- Weave = enemy whiffs past him; Brawler is offset from attack line
- Aerial Rave = target clearly airborne
- Shockwave Slam = ground crack line/ring, but smaller than Warblade's break language

PixelLab feasibility:
- Medium for punch poses.
- Low-medium for subtle weave/whiff timing, because AI may turn it into another dash blur.
- Medium for launch/juggle if prompt explicitly includes airborne enemy.
- Recommendation: Brawler production should use very strict prompts with body mechanics and target state. Unity should add Off-Balance/Pinned/Launched indicators and hit-stop. Do not rely on PixelLab alone to communicate the combat system.

### Summoner

Visual quality: very high.
Class fantasy: strongest sheet.
Production risk: medium.

What works:
- Command/raise/summon/sacrifice/army are visually distinct.
- Lantern identity is readable.
- `Summon Golem`, `Mass Sacrifice`, and `Army of the Dead` are clear.
- The class feels different from all other classes.

Main problem:
Production readability may collapse when many minions/ghosts/fields exist simultaneously in actual gameplay.

Specific concerns:
- `Command Strike` and `Soul Dart` are distinguishable here, but in 128px gameplay their cyan lines may need simplification.
- `Mass Sacrifice` and `Corpse Explosion` both use vertical bone/ghost blast energy. They need different timing/shape in production.
- `Army of the Dead` is impressive, but full swarm visuals may be too noisy in gameplay.

Suggested visual rule:
Summoner needs hard VFX budgets:
- max active corpse field visuals
- minion outline priority
- sacrifice pre-flash
- command line clarity
- Army visual should imply many minions without covering the playfield

PixelLab feasibility:
- High for summon/golem/army concepts.
- Medium for consistent minion type roles.
- Low for gameplay-safe swarm readability if generated as dense illustrations.
- Recommendation: PixelLab can design minion silhouettes and peak sacrifice frames. Unity must control minion count, corpse field duration, command line, sacrifice markers, and visual priority.

### Hexer

Visual quality: good.
Class fantasy: strong.
Production risk: medium.

What works:
- Sickly green curse palette is strong.
- Staff/lantern/sigil identity reads.
- `Blight Sigil` is clear and should be kept.
- `Hex Cascade` looks like a large payoff.

Main problem:
Several skills collapse into green curse energy/explosion. Stack/spread/blast behavior is not visible enough.

Specific concerns:
- `Corruption`, `Pandemic`, `Hexblast`, and `Hex Cascade` are all green curse VFX families.
- Stack thresholds are not visible.
- `Pandemic` should show spread/link behavior, not just another AoE.
- `Hexblast` should be single target or compact payoff; `Hex Cascade` should be chain/room-scale payoff.

Suggested visual rule:
Hexer should separate by curse geometry:
- stack = small marks on target
- grasp = hand/tether
- spread = links between enemies
- sigil = ground glyph
- blast = target ruptures
- cascade = chain propagation

PixelLab feasibility:
- High for sigils and curse mood.
- Medium for linked enemy compositions.
- Low for exact stack count/phase readability in generated art.
- Recommendation: PixelLab for sigil motifs and keyframes; Unity for stack pips, threshold glow, link lines, Pandemic spread, Hexblast/Cascade timing.

## Cross-Class Visual Identity Problems

### Warblade vs Ravager vs Brawler

All three use body impact, ground debris, and melee contact.

Required split:
- Warblade = steel/armor fracture, disciplined break, blue rift cracks
- Ravager = blood cost, self-risk, red frenzy, accepted pain
- Brawler = bare physical timing, dust, body mechanics, launch/juggle

If all three use "big ground impact," they will cannibalize each other.

### Shadowblade vs Ronin

Both can become fast slash classes.

Required split:
- Shadowblade = spatial wound, phase-through, purple Scar/collapse
- Ronin = stillness, one clean draw, white/silver line, Opened window

If both use afterimages and horizontal slash arcs, they blur.

### Elementalist vs Hexer vs Summoner

All can become caster VFX clouds.

Required split:
- Elementalist = geometric element shaping
- Hexer = stack/link/curse phase behavior
- Summoner = minion command, sacrifice, corpse field

If all become "colored magical AoE," class identity will weaken.

### Ranger vs Gunslinger

Both are ranged projectile classes.

Required split:
- Ranger = prepared shot, mark, trap, distance discipline
- Gunslinger = slide, reload, heat, muzzle rhythm

If both are just "projectile trails," they overlap.

## Skills That Feel Weak or Too Generic Visually

These are not necessarily bad designs, but the current visuals do not justify a 4/6 active slot choice strongly enough.

### High concern

- Brawler `Jab`: acceptable as LMB, but too weak as a showcase panel.
- Brawler `Mach Punch`: needs more afterimage/multi-hit identity.
- Brawler `Pivot Hook`: needs planted footwork and single heavy hook read.
- Shadowblade `Veil Strike`: too generic purple slash.
- Shadowblade `Phase Step`: too close to other phase skills.
- Ravager `Bloodlust Strike`: visually close to other axe impacts.
- Ravager `Blood-Drunk Leap`: close to `Frenzied Leap`.
- Gunslinger `Hip Shot`: too close to normal shooting unless the side-slide is emphasized.

### Medium concern

- Warblade `Momentum Slam`: overlaps `Iron Charge`/`Earthsplitter`.
- Ranger `Pinning Shot`: needs clearer pin/root state.
- Ranger `Final Strike`: should not feel like generic execute shot.
- Ronin `Iaido Blur`: can overlap Haste Dash/Quickdraw.
- Hexer `Corruption`: needs clearer difference from Hex Bolt/Hexblast.

## PixelLab Feasibility Assessment

### What PixelLab can do well

PixelLab is suitable for:
- strong class mood frames
- peak impact keyframes
- single-skill concept poses
- big readable elemental effects
- summon/golem/army fantasy frames
- muzzle flash, sword slash, ground impact, magic sigil concepts
- static "what should this skill feel like?" boards

PixelLab is especially good for the current sheet style: dramatic composition, character pose, large VFX, fantasy readability.

### What PixelLab cannot reliably own alone

PixelLab should not be expected to solve:
- exact 128x128 gameplay readability
- exact state stack representation
- consistent per-skill enemy state language
- multi-frame gameplay timing
- hit-stop / slow-mo / perfect input feedback
- precise wire/trap endpoints
- exact projectile collision or beam behavior
- clean UI-like icons, reticles, lock markers, stack pips
- balance readability such as "this is setup, this is payoff"
- animation consistency across every direction unless char_id and strict workflow are used

### Production implication

The correct pipeline should be:

1. Use these sheets as mood/concept references.
2. For each skill, write a one-line visual contract:
   - verb
   - shape
   - state
   - scale
   - what must be readable at 128px
3. Generate only the key pose/peak frame in PixelLab.
4. Use Unity-side VFX for state indicators, ground decals, line/circle telegraphs, projectile behavior, stack pips, target marks, and timing feedback.
5. Validate in-game at 128x128 and PPU 64, not in large concept-sheet form.

### PixelLab prompt strategy

Prompts should not ask for a whole mechanic paragraph. They should be short and structural.

Good prompt shape:
`Brawler pivot hook, feet planted, hip rotation, one heavy side punch impact, enemy bent off-balance, dust only, no magic`

Bad prompt shape:
`Brawler uses Pivot Hook, a skill that consumes Charge and opens a body-shot crit window, make it powerful and dynamic`

Reason: PixelLab will render drama, not mechanic logic. Mechanic logic must be converted into visible pose/shape/state.

### Feasibility by class

| Class | PixelLab feasibility | Notes |
|---|---|---|
| Warblade | Medium-high | Impacts easy; Broken/Sundered state should be Unity-side overlay. |
| Elementalist | High | Element VFX easy; shape interactions need Unity. |
| Shadowblade | Medium-low | Cool purple phase easy; persistent Scar/collapse needs Unity. |
| Ranger | Medium | Archer poses easy; trap/mark readability should be engine-side. |
| Ravager | Medium | Brutality easy; self-cost/trade readability needs UI/VFX. |
| Ronin | Medium | Elegant poses easy; timing/counter fantasy needs animation/game feel. |
| Gunslinger | Medium-high | Shooting easy; reload/heat feedback needs Unity. |
| Brawler | Medium-low | Punch poses easy; weave/whiff/launch logic hard to show clearly. |
| Summoner | High concept, medium production | Summon fantasy easy; gameplay swarm readability needs strict VFX budget. |
| Hexer | Medium-high | Sigils/curses easy; stack thresholds and spread logic need Unity. |

## Recommended Claude Decisions

### Decision 1: Keep sheets as concept references, not production locks

Do not discard them. They are useful.

But Claude should mark them as:
`Concept Skill Identity Boards -- Not Production Sprite Sheets`

### Decision 2: Create a Skill Visual Contract before more PixelLab work

For each skill, require:
- gameplay role: basic/setup/movement/payoff/V
- visual shape: line/cone/ring/field/projectile/trap/link
- state cue: what visible state it applies/consumes
- forbidden overlap: which skill it must not resemble
- PixelLab vs Unity ownership: what is generated vs what is engine-side

### Decision 3: Fix the four riskiest classes before producing more final art

Priority:
1. Brawler
2. Shadowblade
3. Ravager
4. Gunslinger/Ranger ranged split

Reason:
- Brawler can look generic/weak.
- Shadowblade can become generic purple rogue.
- Ravager can become one-note blood axe.
- Gunslinger/Ranger can both become projectile classes.

### Decision 4: Make state VFX engine-owned

PixelLab should not be the source of truth for:
- Broken/Sundered
- Rift Scar/Collapse
- Marked/Trapped
- Cracked/Shattered/Off-Balance/Pinned
- Suppressed/Exposed Line/Heat
- Hexed/Overloaded/Cursed Link
- Corpse Field/Sacrifice Mark

These need consistent Unity-side visual language.

### Decision 5: Apply "8-panel hierarchy" to future sheets

If creating more concept sheets, structure each class as:
- 1 basic/LMB
- 1 resource/RMB
- 1 movement/dash
- 2 setup/state skills
- 1 payoff
- 1 defensive/utility identity skill
- 1 V burst

This prevents every panel from competing to be the biggest VFX.

## Bottom Line

The sheets are visually attractive and useful, but they confirm the user's concern: some skills are too similar. The main fix is not "more detail"; it is stricter visual contracts per skill.

PixelLab can produce the dramatic peak frames, but RIMA's actual skill identity should be locked through:
- shape language
- state cues
- scale hierarchy
- Unity-side consistent overlays/VFX
- 128px readability checks

If this is not done, the final game risks having strong class art but weak skill readability, especially in Brawler, Shadowblade, Ravager, and the ranged classes.
