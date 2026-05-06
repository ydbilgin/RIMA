# Cross-Class Proc System
Status: LOCKED 2026-05-06

## Core Mechanic

Cross-class procs fire on the LMB commit-beat — the final hit of each class's basic combo.
Never on RMB, never on every swing. Proc is a rhythm reward, not background noise.

Cooldown floor: 1.2s between procs (spammed combos do not flood the screen).
Damage: 35% of the origin class's basic attack base damage, scaled from the origin class.
Visual: small class sigil (24-32px glyph) flashes at impact point on the TARGET, not on the player. Lasts 0.4s, fades out.
Status: each proc applies a 2-second family tag to the target.

## Commit-Beat Per Class

| Class | Commit-Beat |
|---|---|
| Warblade | 3rd Iron Combo hit |
| Elementalist | 3rd Rift Bolt hit |
| Shadowblade | 3rd Veil Strike hit |
| Ranger | 3rd tap-shot OR full charged hold release |
| Ravager | 3rd Brutal Swing hit |
| Ronin | 3rd hit (sheathed) OR single heavy cut (drawn) |
| Gunslinger | 6th chamber shot (auto-crit beat) |
| Brawler | 4th Jab combo hit (Cross) |

## Per-Class Proc Identity

| Class | Proc Name | Effect | Family Tag |
|---|---|---|---|
| Warblade | Iron Verdict | Small AoE at target, -15% armor for 2s | Fracture |
| Elementalist | Triprism | Hit in current element, applies that element's Brand | Echo |
| Shadowblade | Rift Nick | 35% Rift damage, target gets +20% crit chance taken for 2s | Veil |
| Ranger | Tracker's Notch | 35% damage, target Tracked: next skill ignores 25% distance falloff | Pierce |
| Ravager | Blood Tithe | 35% Bleed-typed damage, target loses 1% max HP/sec for 2s | Bleed |
| Ronin | Drawn Verdict | 35% damage, target attack windup +15% slower for 2s | Cut |
| Gunslinger | Ricochet Mark | 35% to primary + 20% to nearest enemy in 4 tiles, Pinned: miss +10% for 2s | Pierce |
| Brawler | Knuckle Quake | 35% damage, target takes +8% incoming damage for 2s | Pressure |

## Family Tag System

Tags stack additively on the same target. Multiple families on one target encourage cross-class synergies.
When a target carries 3+ different family tags, the next hit can trigger a Rift proc (see RIFT family).

| Family | Owner | Notes |
|---|---|---|
| Fracture | Warblade | Armor reduction |
| Echo | Elementalist | Element brand, detonates on second same-element application |
| Veil | Shadowblade | Precision/crit window |
| Pierce | Ranger, Gunslinger | Ranged identity, shared |
| Bleed/Hemorrhage | Ravager | DoT, lifesteal fuel |
| Cut | Ronin | Attack speed debuff |
| Pressure | Ronin, Brawler | Shared — martial intimidation family |
| Strike | Brawler | Physical contact identity |
| Rift | Meta-family | Triggers when 3+ families on same target; 50% resistance ignore |
