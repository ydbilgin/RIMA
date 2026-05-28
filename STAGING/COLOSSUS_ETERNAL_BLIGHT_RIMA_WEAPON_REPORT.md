# Colossus - Eternal Blight RIMA Weapon Research

Date: 2026-05-28
Reviewed by: Codex web pass + Antigravity second-eye pass
Purpose: extract weapon/combat ideas for RIMA before the player character has a hand-held weapon.

## Short Verdict

Colossus - Eternal Blight is useful for RIMA because it is much closer to RIMA's likely production scale than Blades of Mirage.

Public sources point to a 2D / pixel-art action RPG:
- Steam tags include `2D`, `Pixel Graphics`, `Action RPG`, `Hack and Slash`, `Class-Based`, `Exploration`, and `Choices Matter`.
- The Steam description calls it a story-driven action RPG with precision combat and a hand-crafted pixel art world.
- The official Rustic Panda page describes retro aesthetics, modern action gameplay, fast-paced combat, rich lore, exploration, weapon upgrades, and secrets.
- Steam community updates describe new dungeon assets, jump indicators, Blight purification quests, localization work, and a Kickstarter/demo cycle.

Main lesson: Colossus is not a "big 3D pipeline" reference. It is a 2D action-RPG readability and production-scope reference.

## What Colossus Does That Matters For RIMA

### 1. Combat is rhythm, not just damage

Steam describes fights as timing, positioning, adaptability, parry, counter, dash, and environment use. It also says weapons change the rhythm of combat.

For RIMA, that means the first weapon decision should not be "which weapon looks coolest?" The first decision should be "which attack rhythm makes the player feel readable and in control?"

Useful weapon rhythm axes:
- fast short slash: low commitment, low damage, easy to cancel;
- heavy cleave: longer windup/recovery, wider hitbox, strong knockback;
- ranged/focus shot: safe spacing, resource or cooldown cost;
- burst/VFX attack: visually strong, cooldown-limited, tied to class/element identity.

### 2. Pixel art weapons have hidden animation cost

If the weapon is baked into every player frame, every new weapon multiplies the animation workload:
- idle/run/turn/attack/damage for 4 or 8 directions;
- different swing arcs per weapon;
- frame cleanup for every character state.

Colossus can afford this because it is explicitly a hand-crafted pixel-art action RPG. RIMA should not assume that same manual frame budget before the core combat loop is proven.

### 3. Class switching is attractive but expensive

Colossus advertises three classes with skill trees/specializations and free strategy evolution. That is a good late-game fantasy, but it is not a good first RIMA combat task.

For RIMA, do not start with "classes". Start with two weapon rhythms under one character:
- quick attack;
- heavy attack or ranged focus.

Only promote them into classes if the two rhythms are fun and distinct.

### 4. Blight-style corruption is a strong roguelite fit

Colossus' Blight idea is very transferable: corruption offers power at a cost, and choices scar the world.

For RIMA:
- spend HP, max HP, shield, curse, or room reward quality for a temporary power spike;
- corruption can modify the weapon/VFX rather than create a whole new weapon set;
- roguelite runs can carry "scar" modifiers without needing permanent story branching immediately.

This is a better near-term RIMA hook than full Zelda-style puzzle dungeons.

## Recommended RIMA Weapon Direction

### Recommendation: VFX-first weapon, then attached weapon sprite

RIMA should not immediately redraw the player holding a weapon in every animation.

Prototype order:
1. Add a no-visible-weapon slash/hitbox VFX on attack frames.
2. Add hitstop, knockback, screen shake, SFX, and clear enemy reaction.
3. Add a simple attached weapon sprite only after the timing feels good.
4. Keep the attached sprite modular: one pivot, one local offset per facing direction.
5. Add a second rhythm only after the first weapon feels solid.

This gives RIMA weapon feel without locking art direction too early.

## First Prototype Spec

### Weapon A: Quick Spirit Blade

Purpose: default close combat.

Behavior:
- short range cone or capsule hitbox;
- fast startup;
- short recovery;
- light knockback;
- small slash VFX, color-coded to player class/element;
- no visible held sword required at first.

Why:
- proves melee readability quickly;
- avoids full character redraw;
- supports current RIMA 2D/2.5D flow.

### Weapon B: Heavy Blight Cleave

Purpose: commitment-based alternate rhythm.

Behavior:
- slower startup;
- wider arc;
- bigger hitstop;
- higher damage or stagger;
- costs resource/corruption/cooldown;
- VFX can look like a temporary corrupted blade rather than a permanent weapon.

Why:
- imports the "power at a cost" idea without building three classes;
- gives player a meaningful rhythm swap;
- keeps art budget in VFX instead of full weapon animation sets.

### Optional Later Weapon C: Focus Shot

Purpose: ranged spacing tool.

Do not implement first unless melee feels too limiting. Ranged weapons quickly create enemy AI and room-shape balance problems.

## Mechanics To Borrow Now

Borrow now:
- dash with cooldown and short invulnerability or collision bypass window;
- counter only if enemy telegraphs are readable;
- two attack rhythms instead of full arsenal;
- corruption/risk power as temporary weapon modifier;
- environmental combat only as simple hazards/barrels/zones, not full puzzle dungeons;
- clear active frames with VFX and hitbox debugging.

Borrow later:
- three classes;
- weapon upgrade trees;
- permanent NPC/world scars;
- Zelda-style dungeon puzzle chains;
- heavy narrative choice system.

Avoid now:
- baked weapon-in-hand animation for every direction;
- parry before enemies have readable windup frames;
- class switching UI;
- too many weapons before one weapon has good hit feel;
- permanent story consequence system inside a still-unstable roguelite loop.

## AI / Asset Pipeline Implication

AI images are useful for:
- slash VFX sheets;
- weapon silhouette exploration;
- icon sets;
- corruption/blight overlays;
- enemy telegraph shapes;
- mood boards and palette targets.

AI images are risky for:
- consistent frame-by-frame player weapon animation;
- exact hand placement across 4/8 directions;
- clean pixel animation timing;
- readable hitbox-to-frame alignment.

Recommended pipeline:
1. AI creates broad weapon silhouettes and VFX concepts.
2. RIMA implements weapon feel with simple procedural/VFX hitboxes.
3. Only the winning weapon gets a cleaned pixel sprite/animation pass.
4. Keep the weapon as a separate layer/socket whenever possible.

## Antigravity Second-Eye Summary

Antigravity agreed that Colossus is best read as a 2D top-down pixel action RPG reference and focused on production risk:
- visible physical weapons multiply animation cost;
- no-visible-weapon VFX or a programmatically attached weapon sprite is the safest first step;
- two weapon rhythms are better than full class switching for the first RIMA prototype;
- Blight-style power-at-cost is transferable;
- parry/counter is risky until enemy telegraphs are very clear.

Antigravity mentioned Unity/2.5D as a possible implementation model. I did not find a public official source confirming the engine, so this report does not treat engine choice as verified.

## RIMA Action Items

1. Prototype Quick Spirit Blade as VFX + hitbox only.
2. Add hitstop, knockback, cooldown, and visible enemy reaction before adding more weapons.
3. Add Heavy Blight Cleave as the second rhythm, not a second class.
4. Add a simple socket/attachment layer only after attack timing is approved.
5. Delay class switching, full arsenal, and Zelda dungeon puzzle systems.

## Sources

- Steam store: https://store.steampowered.com/app/1722800/Colossus__Eternal_Blight/
- Official site: https://www.rusticpandagames.com/colossus-eternal-blight
- SteamDB info: https://steamdb.info/app/1722800/info/
- Steam community hub/dev updates: https://steamcommunity.com/app/1722800
- Reddit developer post, secondary context: https://www.reddit.com/r/ZeldaLikes/comments/1sskrli/were_making_a_zeldainspired_game_with_jrpg_and/
- Local Antigravity output: `AGY_DONE_yasinderyabilgin.md`
