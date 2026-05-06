# Rift Portal Opportunity
Status: LOCKED 2026-05-06

## What Is It

A rare surprise event that can appear after clearing a combat room.
The floor cracks, smoke rises, a whisper sound plays. You can walk through it or ignore it.
If you enter, you get one offer: a powerful bonus paired with a cost. No reroll. No second chance.

## Placement Rules

- Triggers after: Combat, Elite, Unknown combat rooms are cleared
- Never appears in: Shop, Spirit, Boss, tutorial/start rooms, or the room after a boss
- First possible appearance: after 2 completed combat rooms
- Base chance: 4% per eligible room
- Spacing: minimum 8 rooms between portals
- Cap: max 1 per act, max 2 per full run

## Spawn Details

- Spawns near a room edge or wall seam
- Never blocks: gate sockets, reward pickups, enemy spawn points, required path
- Cue sequence: floor crack appears (holds 0.8s) -> whisper sound -> cyan/purple smoke -> interact prompt

## Offer Format

One Burden + Gift pair. Player accepts or walks away.
No browsing, no comparison. The rarity is part of the design.

Burden timer starts on first encounter AFTER accepting, not on the accept frame.

## Duration Rules

- Gifts: run-permanent by default
- Burdens: encounter-limited by default
- Hard rule: every burden ends no later than the end of the current act, regardless of counter

## Second Portal Rule

When a second portal appears in a run, its Burden options are weighted to complement the first
Burden's category — movement, vision, cooldown, economy. Not duplicated. 30% wildcard override.

## Minimap

No minimap icon. The portal is a discovery, not a waypoint.

## Relation to Curse Gate

Separate system (RiftOpportunity). Curse Gate is a route-level node the player commits to before
entering a room. Rift Portal is a post-clear stochastic surprise. Different trigger, different UI,
different player mental model. Shared utility helpers (modifier registration, duration ticking) are fine.

## Burden + Gift Table (Baseline)

| Burden | Duration | Gift | Duration |
|---|---|---|---|
| Move speed -15% | 3 encounters | Dash leaves a damaging rift trail | run |
| Reward previews hidden | 4 chambers | All skill offers gain +1 rarity roll | run |
| Next 3 rooms spawn +20% enemies | 3 encounters | Damage +18% when surrounded by 3+ enemies | run |
| Skill cooldowns +20% | 2 encounters | LMB/RMB combo finishers trigger a rift echo hit | run |
| Max HP -12% | 2 acts | Gain +1 revive shard or emergency shield proc | run |
| Dash cooldown +30% | 3 encounters | Every 4th dash creates a pull pulse | run |
| Pickups heal 0 HP (healing only, not currency/keys) | 3 encounters | Skill damage lifesteals 2% vs elites/bosses | run |
| Minimap route previews disabled | 3 rooms | Next 3 rewards are upgraded or tagged | 3 rewards |

## Extended Ideas (Not Yet Scheduled)

- Burden manifests visually on character (cracked makeup overlay) so NPCs/shrines can react to burdened state
- Rift slightly warps next room modifier (extra elite or guaranteed shrine) via one-shot RuntimeRoomManager flag
- Meta: if same Gift was taken in a previous run, show a mutated variant instead (gate behind opt-in toggle)
