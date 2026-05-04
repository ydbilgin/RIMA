# RIMA Loadout, Skill Behavior, Minimap Roadmap - 2026-05-04

Status: staging note for Claude/design decision. Not a locked GDD change.

## Canon Check

RIMA is not currently locked as a literal shelter-only fiction. Current memory points to an old controlled keep / seal / machine / ritual structure built to delay or contain the Rift March.

Practical interpretation for Act 1:
- Use "Shattered Keep / Sunken Sanctuary / seal shelter" visually.
- Rooms should feel human-built: walls, thresholds, arches, pillars, locked cells, broken ritual machinery.
- Avoid random rock scatter as the primary structure language.
- Scatter is detail only: breach rubble, collapsed wall, shrine remains, rift crystal spill, broken chain anchor.

## Loadout Philosophy

RIMA already has a loadout direction, but it should be made explicit:
- Small active bar, not a giant MMO bar.
- Primary class determines starting verb set and offer pool.
- Run offers change behavior, not only numbers.
- A player should choose between AoE clear, single-target focus, control, defense, and route utility.
- No permanent visual skill tree on the main screen. Skill identity should be readable through offers, skill bar icons, VFX state, and the C-side build codex.

Bastion takeaway:
- Bastion's Arsenal limits the player to two weapons and one Secret Skill, while Spirits act as passive loadout modifiers.
- RIMA should not copy the weapon count literally.
- What to take: compact loadout pressure, strong identity per equipped action, passive "spirit/echo" style modifiers, and mid-run swap/upgrade moments.

## AoE vs Single Target Contract

Room and skill balance should deliberately create tradeoffs:
- Swarm/chaff rooms reward AoE, chain, field, trap, and persistent zone skills.
- Elite rooms reward burst windows, CC setup, mark detonation, armor break, and single-target scaling.
- Boss rooms reward uptime, defensive timing, predictable burst, and boss-only modifiers.
- A build can cover both, but it must pay in cooldown, resource cost, slot pressure, or setup time.

Decision target:
- Every active skill has at least one AoE route and one single-target route over time.
- Not every route needs new animation. Many can be VFX/targeting/property changes.

## Lightning Identity

Lightning should not read as generic "explosion" by default. It should read as:
- Chain
- Shock
- Conduit
- Stun/electrocute
- Fast burst
- Marked discharge
- Forking line or ring nova

Lightning explosion is valid only when framed as discharge/nova:
- Static Orb style: travelling lightning orb with stronger explosion/discharge at the end.
- Shock Nova style: ring-shaped lightning burst, not fireball-like blast.
- Inpulsa style: shocked enemies explode on death as a conditional clear payoff.

RIMA Elementalist proposal:
- Chain Lightning: baseline chain / shock / conductor, mostly VFX and targeting.
- Storm Conduit: place a rift rod/orb; lightning skills hit it to discharge arcs. VFX plus spawn object.
- Shock Nova: close-range ring clear, animation can reuse cast pose, needs circular VFX.
- Lightning Rod: single-target mark; later hits discharge into boss/elite. VFX plus mark state.
- Static Break: consumes Shock stacks for boss burst. Needs state logic and VFX, not new character animation.

Do not make Chain Lightning a big explosion by default. Let it become AoE through chain count, forks, or conditional shocked-kill discharge.

## Elementalist Build Model

Elementalist should support four valid lanes:
- Fire specialist
- Frost specialist
- Lightning specialist
- Mixed element caster

The balance should not be "mixed is always best". The trade should be:
- Single element = stronger identity, deeper stacking, cleaner resource loop.
- Mixed element = more answers, more combo windows, less peak scaling unless the player invests in hybrid offers.

Elementalist lanes:

Fire:
- Fantasy: burn, delayed blast, ground denial, high AoE clear.
- Strength: swarm clear and room pressure.
- Weakness: mobile elites/bosses unless marked or trapped.
- Behavior routes: Fireball pierce/splash, Living Bomb detonate-on-death, Meteor burn field.

Frost:
- Fantasy: slow, brittle, zone control, shard burst.
- Strength: safety, elite control, setup.
- Weakness: lower raw clear without shatter route.
- Behavior routes: Glacial Spike single-target crit, Blizzard field control, Frozen Orb orbit/slow.

Lightning:
- Fantasy: speed, chain, shock, conductor/discharge.
- Strength: target finding, medium AoE, elite burst after setup.
- Weakness: worse against isolated single target unless Lightning Rod / Static Break is chosen.
- Behavior routes: Chain Lightning forks, Shock Nova close clear, Lightning Rod boss mark, Static Break consumes Shock stacks.

Mixed:
- Fantasy: combo mage, element rotation, elemental state reactions.
- Strength: adapts to room type, can solve many enemy profiles.
- Weakness: weaker single lane scaling, more cooldown/resource tension.
- Behavior routes:
  - Fire + Frost = steam burst / brittle burn, moderate AoE plus slow.
  - Frost + Lightning = superconduct, lightning chains farther on chilled/shocked targets.
  - Fire + Lightning = overcharge, shocked enemies burn faster or discharge on death.
  - Triple element = Rift Prism / unstable high-cost payoff, rare offer only.

Elementalist offer rule:
- Early offers should let the player declare a lane by room 2-3.
- Later offers should either deepen the chosen lane or deliberately open a hybrid branch.
- Lightning should be added as a real lane, not as a blue recolor of fire explosion.

Suggested default bar direction:
- Slot 1: Fireball or elemental bolt baseline.
- Slot 2: Chain Lightning baseline if player chooses Elementalist, replacing Living Bomb in default test bar for now.
- Slot 3: Frost control skill.
- Slot 4: high cooldown payoff skill based on lane offer.

Implementation priority:
- VFX/data first: shock stacks, chain/fork, ring nova, lightning rod marker.
- Animation later: channel pose, charged overhead cast, dedicated ultimate cast.

## Behavior Change Taxonomy

VFX-only / data-only:
- Damage type tint.
- Impact shape.
- Trail color.
- Hit flash.
- Cooldown/resource/radius/tick rate.

Targeting logic:
- Chain count.
- Fork angle.
- Pierce vs bounce.
- Nearest shocked target priority.
- Execute target priority.

Projectile/area behavior:
- Orb travels then detonates.
- Cone becomes line.
- Slash leaves ground fissure.
- Trap arms then triggers.
- Field persists for X seconds.

State interaction:
- Shock stacks.
- Bleed stacks.
- Marked target.
- Frozen/chilled target bonus.
- Rage/resource threshold.
- Shadow veil/clone state.

Extra VFX asset required:
- New rune circle.
- Lightning rod/conduit object.
- Poison cloud.
- Blood mark.
- Elite/boss telegraph.

Extra character animation required:
- New dash/phase pose.
- Held channel pose.
- Two-handed slam windup.
- Bow full draw variant.
- Backstab/finisher pose.

## Class Behavior Routes

Warblade:
- AoE route: cleave, ground fissure, rage shockwave.
- Single-target route: Sunder Mark, Death Blow, armor break, exposed target.
- VFX enough for fissures/marks; new animation needed for big slam or execution pose.

Elementalist:
- AoE route: Blizzard, Shock Nova, Chain Lightning forks, Meteor field.
- Single-target route: Lightning Rod, Static Break, Glacial Spike boss shard.
- VFX plus target/state logic handles most. New animation only for channel/beam fantasies.

Ranger:
- AoE route: Sweep Volley, trap field, ricochet arrows.
- Single-target route: Predator Mark, Pinning Shot, Final Strike.
- VFX plus projectile logic handles most. New animation needed for charged snipe.

Shadowblade:
- AoE route: Veil Burst, chain cull, smoke poison field.
- Single-target route: Backstab Mark, Death Mark, scar collapse.
- VFX plus state logic handles most. New animation needed for true backstab/finisher.

## Minimap Direction

ARPG pattern from PoE2 and Last Epoch:
- Small corner minimap during normal combat.
- Toggle key opens a larger overlay map.
- Fog/reveal matters.
- Icons appear based on discovery and game state.
- Map transparency/zoom controls matter because overlay can hide combat readability.

RIMA implementation target:
- Current pass: corner minimap plus TAB tactical map toggle.
- Corner map shows current room footprint, player, enemies, map fragments, exits, and short route preview.
- Tactical map expands same data instead of replacing the whole game with a menu.
- Later: fog-of-war masks, discovered room silhouettes, clickable route nodes after room clear, zoom/transparency sliders.

## Dungeon Visual Direction

Immediate production rule:
- Build floor/wall/object assets from generated RIMA-style pieces now.
- Preserve Unity Tile asset references by replacing PNG contents at existing paths.
- Future PixelLab pass can refine the exact same filenames and importer settings.

Room generation rule:
- Every room needs a readable purpose: gate hall, shrine chamber, broken barracks, flooded corridor, chain anchor, rift lab, collapsed audience hall.
- Walls must form architecture. Obstacles must preserve traversal spine and spawn/gate safety.
- Use landmark props at corners/thresholds, not random blockers in the middle of every room.
