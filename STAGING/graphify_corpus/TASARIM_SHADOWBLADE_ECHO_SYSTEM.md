---
status: REFERENCE
faz: 1
tarih: 2026-05-06
ozet: "Shadowblade Echo sistemi"
---
# Shadowblade — Echo System
Status: LOCKED 2026-05-06

## What Is the Echo

A translucent indigo Shadowblade copy that mirrors the player's basic attacks with a 0.4s delay
at 50% damage. Enemies see it and split attention (~30% aggro weight) but cannot hit it.
Maximum 1 Echo active at a time (skill tree node can push to 2 at 30% damage penalty).

## Echo Creation — Veil Thread Weave

Veil Flicker is a SKILL SLOT (not RMB). Echo is created by COMBO RHYTHM, not by spamming RMB.

Two sources of Veil Threads:
- Veil Strike LMB 3rd hit: places a floor thread at player's feet (lasts 4s, max 2 active)
- Veil Flicker skill: places one thread at cast origin

Crossing definition: player's collider center passes through the thread segment line (single-frame
intersection). Enter+exit NOT required. Lingering overlap does NOT count.

Trigger: cross 3 threads within an 8-second rolling window -> Echo spawns at player's current position.

Echo Charge UI: 3 pips beneath HP bar. Each thread crossing fills one pip.
8s window timer arc drains around the pip cluster. Full pips + arc > 0 = Echo spawns on next eligible crossing.

## Echo Behavior

- Mirrors all LMB basic attacks (Veil Strike chain) with 0.4s delay, 50% damage
- Does NOT auto-mirror skills
- Skills tagged [EchoEligible] fire from both player and Echo simultaneously
- Current [EchoEligible] whitelist: Rift Scar, Shadow Step
- All future Shadowblade skills default to NOT EchoEligible — requires explicit tag + design approval
- Echo lifetime: 12 seconds or until Cull

## Crossfade (Position Swap)

Hold a hotkey (dedicated bind) for 0.5s while Echo is active: player and Echo swap positions instantly.
Brief violet streak VFX (0.2s) between positions.
Puts Veil Flicker skill on 2s lockout after swap.

## Cull (Detonation)

LMB + RMB simultaneous tap while Echo is active:
- Echo collapses into a Rift implosion at its location
- AoE: 2-tile radius
- Damage: 80% of total damage Echo dealt during its lifetime
- Refunds 1 Veil Thread charge

Natural Echo expiry (timer runs out): 30% of accumulated damage as a small pulse. Punishes passive play.

## Interaction with Shadowblade Mechanics

- Veil Strike 3rd hit: places thread AND is the commit-beat for cross-class proc
- Scar (status on enemies): when Echo's mirrored basic hits a Scar'd target, Scar detonates 25% faster
- Rift Scar (skill, [EchoEligible]): two Rift Scars fire from both positions; if they intersect mid-flight, intersection point detonates for bonus damage

## Visual Language

- Echo: indigo/violet, 50% opacity, Unity shader override on player sprite sheet (zero new PixelLab work)
- Echo has no ground shadow (player has normal drop shadow — instant differentiation)
- Echo has thin dark-violet rim light for visibility on dark tiles
- Echo shows 1-frame ghost trail on movement so motion reads even when still
- Crossfade: violet streak (0.2s) — loud enough to register, short enough not to obscure combat
- Cull: black-violet implosion burst, Unity VFX

## Balance Anchors

1. Mirror at 50%, no skill auto-mirror — Echo DPS capped at ~35-40% of player DPS
2. 1 Echo cap — no clone armies
3. Charge-gated: Echo requires combo discipline. Skills-only Shadowblade never gets Echo.

