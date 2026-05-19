---
status: REFERENCE
faz: 1
tarih: 2026-04-30
ozet: "Summoner + Hexer sinif tasarımı"
---
# Summoner + Hexer Class Design
Status: LOCKED 2026-05-06

---

## SUMMONER

### Basic Attack

**LMB — Bone Spike**
Summoner personally hurls a bone shard from the off-hand.
3-beat cadence: flick (light) / flick (light) / drive (commit-beat, heavier + knockback wobble).
Frames: 4 / 4 / 6 per direction. Tempo: 0.32s / 0.32s / 0.45s.
The shard is intentionally weak — value lives in the Mark it places.

**RMB — Tether Pull**
Short bone-thread whip that yanks the nearest minion one tile toward cursor.
On enemies: chip damage + tags them as minion focus target.
No resource, 0.25s recovery. True filler. Used for repositioning between LMB beats.

### Identity Mechanic — Mark & Sic (Command Cadence)

Every LMB hit deposits 1 Mark stack on target (cap 3, decay 4s).
LMB commit-beat (3rd flick) fires a Sic pulse: all active minions immediately dash to attack
the highest-Marked enemy in range.

The loop: basic attack IS the minion command channel.
Without LMB Marks, minions default to passive guard-orbit around the Summoner (defensive chip only).
They commit hard only when Sic'd — which only happens on the commit-beat.

**Micro-decision:** Sic now on the priority target, or Tether-Pull (RMB) to reposition first
and Sic on the next beat? You are conducting, not chasing a rotation.

**Visual cues:**
- Marks: skull glyphs orbiting target (1/2/3 visible)
- Sic beat: radial pulse from Summoner + minion eyes flash white as they pivot
- 3rd LMB shard glows brighter on release frame

**Cross-class echo (stripped):** Other classes' commit-beat applies 1 Mark — but no Sic, no minion redirect.
Just a debuff timer that boosts that class's own next hit. Pure family hook, no pet management.

### Minion System

- **Cap:** 3 minions baseline (skill tree can expand)
- **Starter type — Bone Hound:** Low-HP melee skirmisher, ~60% player movespeed at idle, sprints on Sic
- **Other types unlock via skill slots:** Bone Sentinel (ranged), Bone Ogre (tank), Carrion Swarm
- **Persistence:** persist across rooms, NOT across runs. They have HP; death requires re-summon via skill (no auto-respawn). Out of combat: slow regen toward soft-cap, not full HP.

### Aim Shot

Not integrated on LMB. Bone Spike is a snap-throw; plant-and-aim would interrupt the conducting cadence.
Aim Shot belongs on a dedicated skill (e.g. Bone Lance, charged long-range shot).

### Cross-Class Proc — Family: Bone (NEW)

Trigger: LMB commit-beat. 35% damage, 1.2s CD.
Effect: applies [Bone-Brittle] — target takes +12% damage from minion sources for 3s.
Bonus: next minion hit on Bone-Brittle target shaves 1s off next minion-summoning skill cooldown.
Visual: pale bone-dust crack on target silhouette; brief outline flash on minion hit.

### PixelLab Cost

| Element | Type | Frames/Dir |
|---|---|---|
| Idle | PixelLab | 4 |
| Walk | PixelLab | 6 |
| LMB Bone Spike (3-beat) | PixelLab | 4+4+6 (arm overlay only, shared base) |
| RMB Tether Pull | PixelLab | 4 |
| Sic command pose (commit-beat overlay) | PixelLab | 3 |
| Hit react + death | PixelLab | 2+4 |
| Mark skull glyphs | Unity VFX | — |
| Sic radial pulse | Unity VFX | — |
| Bone shard projectile + impact | Unity VFX | — |
| Minion eye-flash on Sic | Unity (minion prefab) | — |

Bone Hound sprites = separate PixelLab production, not counted above.

### Feel

You enter a room, LMB-flick-flick-flick on the front-line elite. Your three Hounds, orbiting
passively, suddenly lurch as one and tear into the marked target. While they chew, you Tether-Pull
the trailing Hound toward a flanking caster, then LMB-flick-flick-flick on the caster — the pack
pivots mid-stride to the new Mark. You are never just damaging: you are pointing. The room ends
with you walking through bone-dust while your Hounds finish stragglers. Less wizard, more
kennel-master with a whistle made of vertebrae.

---

## HEXER

### Basic Attack

**LMB — Curseweave**
3-bolt sequence of dark filaments from outstretched hand.
3-beat cadence: whisper (light) / whisper (light) / utter (commit-beat, fatter bolt + audible word-snap).
Frames: 4 / 4 / 6 per direction. Tempo: 0.35s / 0.35s / 0.50s.
Slightly slower than caster norm — deliberate, each beat thinks.
Bolts have slight tracking-arc drift by default; Aim Shot straightens them.

**RMB — Ill Wind**
Quick conical puff of cursed mist, close-mid range (3 tiles). Chip damage.
0.3s recovery. True filler. Used to tag adjacent enemies with 1 Hex stack between LMB cycles,
or to briefly clear a flanking group. Naturally repeatable.

### Identity Mechanic — Hex Stacks & Overload (Stack Cadence)

Each LMB bolt applies 1 Hex stack to the target (cap 5 per enemy, decay 5s).
The commit-beat (3rd bolt) applies 1 stack AND reads the current stack count for an Overload payoff:

| Stacks on commit | Overload |
|---|---|
| 1-2 | Minor extra damage only |
| 3 | Wither — target slowed 25% for 2s |
| 4 | Gnaw — DoT, ticks 4 times |
| 5 | Rupture — small AoE burst from target, applies 1 Hex stack to neighbors, resets target stacks to 0 |

**Micro-decision:** Commit at 3 stacks for an immediate slow (kiting), bank to 5 for AoE seed
(crowd control), or spread stacks across multiple enemies with Ill Wind to set up simultaneous
Ruptures? The LMB rhythm does not change — the meaning of the third bolt does, every cycle.

**Visual cues:**
- Hex stacks: ring of rune-marks rotating at target's feet (1-5 visible)
- Stack tiers change color: pale violet (1-2) -> deep indigo (3-4) -> black-red (5)
- 3rd LMB bolt has longer tracer when target is at 3+
- Cast hand flares brighter the higher the pending Overload tier

**Cross-class echo (stripped):** Other classes' commit-beat applies 1 Hex stack. But no Overload —
stacks sit as a flat +3% damage-amp per stack on that target. Only Hexer can read stacks into
Wither/Gnaw/Rupture.

### Aim Shot

**Integrated on LMB hold.**
Holding LMB plants the Hexer (no time dilation). Bolts fire in cursor direction with tighter
accuracy and ~15% longer range. The 3-beat cadence is preserved during plant — foot position
locks, arm continues the weave. Useful for sniping a specific target to bank stacks without drift.
The plant looks like incantation — fits the deliberate Hexer feel.

### Cross-Class Proc — Family: Hex (NEW)

Trigger: LMB commit-beat. 35% damage, 1.2s CD.
Effect: applies [Hex-Touched] — target's incoming damage from any source amplified +8% for 3s.
Bonus: if target dies while [Hex-Touched] active, explodes for small soul-burst (chip AoE, no stack application).
[Hex-Touched] is a DIFFERENT tag from Hex stacks — "this enemy has been seen by a curse" rather than stack-counted.
Visual: purple-black sigil burns briefly on target's chest. Death-burst: small inward implosion + dust ring.

### PixelLab Cost

| Element | Type | Frames/Dir |
|---|---|---|
| Idle | PixelLab | 4 |
| Walk | PixelLab | 6 |
| LMB Curseweave (3-beat) | PixelLab | 4+4+6 |
| LMB Aim Shot plant entry | PixelLab | 3 (enters plant, reuses cycle frames) |
| RMB Ill Wind | PixelLab | 4 |
| Hit react + death | PixelLab | 2+4 |
| Hex stack runes (orbiting target) | Unity VFX | — |
| Bolt projectiles (3 brightness tiers) | Unity VFX | — |
| Wither/Gnaw/Rupture payoff VFX | Unity VFX | — |
| [Hex-Touched] sigil + death-burst | Unity VFX | — |

### Feel

You walk in slow. First enemy: weave-weave-utter — 3 stacks land, Wither, it limps. While it
limps, Ill Wind across two backline enemies — they each get 1 stack. Re-target the original:
weave-weave, now at 5, utter — Rupture pops, the stack-bomb seeds 1 stack on every neighbor
including the two you Ill-Winded. Now half the room is at 1-2 stacks. You plant (Aim Shot) on
the priority caster, snipe-utter to push it to 3, Wither again. The room dies in waves of
self-detonating curses, and you barely moved. Less damage dealer, more clockmaker — every
commit-beat is a tooth turning a gear, and the gears are inside the enemies.

