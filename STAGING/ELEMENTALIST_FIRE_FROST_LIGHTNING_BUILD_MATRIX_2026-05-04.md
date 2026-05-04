# Elementalist Fire / Frost / Lightning Build Matrix - 2026-05-04

Status: staging design. For Claude/design approval before production lock.

## Goal

Elementalist must support:
- Pure Fire
- Pure Frost
- Pure Lightning
- Mixed element builds

The balance target:
- Pure lanes have higher peak scaling and clearer identity.
- Mixed lanes have better coverage and combo utility, but pay with setup, cooldown, resource, or lower peak multipliers.

## Lane Identity

Fire lane:
- Core verbs: burn, delayed detonate, ground field, ignite spread.
- Best at: AoE clear, packed rooms, soft enemy waves.
- Weak at: mobile elite/boss unless the player takes mark/field anchoring.
- Boss solution: burn stacking, exposed target, meteor/flare window.

Frost lane:
- Core verbs: chill, freeze, brittle, shatter, slow field.
- Best at: control, safety, elite pacing, creating kill windows.
- Weak at: raw room clear unless shatter chain is chosen.
- Boss solution: brittle stacks, spike crit window, safe uptime.

Lightning lane:
- Core verbs: chain, shock, fork, conductor, discharge.
- Best at: mid-density rooms, fast target acquisition, setup burst.
- Weak at: isolated bosses unless Lightning Rod / Static Break is chosen.
- Boss solution: shock stack into rod/discharge, high burst but setup dependent.

Mixed lane:
- Core verbs: state reaction, element rotation, prism payoff.
- Best at: flexible routes, enemy variety, solving map modifiers.
- Weak at: peak damage and simple execution.
- Boss solution: combo windows rather than one dominant stack.

## Offer Rules

Early offer rooms:
- Offer at least one lane-defining choice by room 2.
- Avoid giving all three elements equal shallow upgrades forever.
- If player picks two same-lane offers, bias later offers toward that lane plus one hybrid escape.

Mid run:
- Pure lane offers deepen identity:
  - Fire: bigger burn/detonate.
  - Frost: better brittle/control/shatter.
  - Lightning: better chain/conductor/discharge.
- Mixed offers should require two element states or alternating casts.

Late run:
- Pure lane gets one high peak payoff.
- Mixed lane gets one high complexity payoff.
- No late offer should be a generic +damage only unless paired with behavior.

## Reaction Rules

Fire + Frost:
- Steam Crack: chilled enemies hit by fire emit a short cone burst.
- Balance: good clear, low boss value unless brittle is present.

Frost + Lightning:
- Superconduct: lightning chains farther through chilled/frozen targets.
- Balance: excellent control clear, moderate boss value.

Fire + Lightning:
- Overcharge: shocked burning enemies discharge on death.
- Balance: strong AoE, limited boss value unless a rod exists.

Triple:
- Rift Prism: after casting three different elements, next skill gains a prism modifier.
- Balance: high skill ceiling, high resource or cooldown cost.

## Skill Behavior Slots

Fireball:
- Pure Fire route: bigger burn field, delayed ember burst.
- Mixed route: applies heat state for steam/overcharge.
- Needs: projectile logic + impact VFX. No new animation.

Blizzard / Frozen Orb:
- Pure Frost route: longer chill field, brittle stacks, shatter.
- Mixed route: primes superconduct or steam.
- Needs: VFX and status logic. No new animation.

Chain Lightning:
- Pure Lightning route: extra chains, shocked priority, fork on kill.
- Single-target route: Lightning Rod causes chains to return to marked elite/boss.
- Mixed route: chilled targets extend chains; burning shocked kills discharge.
- Needs: targeting/state/VFX. No new animation.

Shock Nova:
- Pure Lightning AoE route: player-centered ring, short range, high clear.
- Mixed route: ring inherits last element state.
- Needs: circular VFX. No new animation.

Static Break:
- Pure Lightning boss route: consumes shock stacks for burst.
- Mixed route: consumes fire/frost states for different secondary effect.
- Needs: state logic + impact VFX. No new animation.

## Production Order

1. Add element state tags: Burning, Chilled/Frozen, Shocked, Brittle, Conductor.
2. Make Chain Lightning target from cell/world logic and shocked priority.
3. Add Lightning Rod as boss/elite single-target branch.
4. Add Shock Nova as AoE branch.
5. Add mixed reactions after pure lanes are readable.

