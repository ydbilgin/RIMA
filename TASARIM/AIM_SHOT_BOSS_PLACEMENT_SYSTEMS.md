# Aim Shot, Boss Weak Spot, Area Skill Placement
Status: LOCKED 2026-05-06 | UPDATED 2026-05-09 (S47: hold→tap-to-aim mode)

---

## 1. Aim Shot System

### Core Rules (UPDATED S47)
- Character body stays 4-cardinal (S/E/N/W). No new sprite directions, ever.
- Projectile direction is decoupled from body facing. Projectile vector = cursor world position - player world position (normalized).
- **Activation: TAP skill key → enters aim mode (cursor changes to reticle). TAP/CLICK to fire. TAP key again to cancel mode.**
- No time dilation. Game runs at full speed during aim mode. Risk is real.
- Character snaps body facing to nearest cardinal of cursor direction on aim mode entry.
- Body animation during aim: 2-frame plant loop layered on normal idle. No new cardinal set needed.

### Why TAP-mode (not hold) — S47 Decision
ActionCommitProfile sistem-wide hold yasak. Aim Shot eski "hold past 0.25s" tasarımı bu yasakla çakışıyordu. Çözüm: 2-stage tap. Birinci tap aim mode açar (cursor reticle olur), ikinci tap ateşler. Hold gerekmez, aynı taktiksel okuma korunur.

### Aim Indicator (Unity VFX, shared)
- Cursor sprite **reticle**'a dönüşür (aim mode aktif görsel)
- Dotted trajectory line from player to cursor
- Class-tinted color: green (Ranger), element color (Elementalist), gold (Gunslinger)

### Per-Class Binding (UPDATED S47)
| Class | Binding | Notes |
|---|---|---|
| Ranger | LMB tap-mode | Primary identity. Tap LMB = enter aim mode. Tap LMB again = fire Aim Shot + bank Draw Weight. RMB = quick cardinal shot (no aim, fast escape). |
| Elementalist | Lightbreak skill tap-mode | Tap skill key = plant + aim beam. Tap again = fire beam in cursor direction. |
| Gunslinger | Hip Shot skill tap-mode | Tap skill key = side-step into plant + reticle. Tap again = precise cursor-aimed shot. Auto-crits if 6th/12th chamber. |

### PixelLab Cost
- Shared plant pose: 2 frames x 4 directions = 8 frames (one-time, shared across all 3 classes)
- Per-class arm overlay: 2 frames x 4 directions x 3 classes = 24 frames
- Total: 32 new PixelLab frames for the entire Aim Shot system
- All aim indicator VFX, trajectory line, reticle, beam rotation: Unity only

---

## 2. Boss Weak Spot System

### Mechanic
Weak spots are sub-colliders on boss rigs flagged WeakSpot.
Cardinal basic attacks pass through weak spots without registering.
Only projectiles with aimed=true flag resolve hits on weak-spot colliders.

### Weak Spot Hit Tiers (per-boss assignment)
| Tier | Effect |
|---|---|
| Bonus damage | 1.75x damage (default, most bosses) |
| Status apply | Stagger, Expose (next 2s all hits crit), or Bleed-stack |
| Phase window | 3s vulnerability window (act bosses only, not minibosses) |

### Telegraph
- Outside aim mode: weak spot glow is HIDDEN. No permanent visual noise.
- In aim mode: faint pulsing red glow on the boss sprite, 1 Hz throb.
- Brightens when crosshair is within hit cone.

### Class Access
Aim-shot classes (Ranger, Elementalist, Gunslinger) access weak spots directly via aimed projectiles.

Non-aim-shot classes (Warblade, Shadowblade, Ravager, Ronin, Brawler) access via:
- A skill with aim sub-mode (e.g. Ronin Iaijutsu Throw aimed variant)
- Environmental hazards the player baits the boss into (hazards deal weak-spot damage regardless of source)

### Weak Spot Hit Feel
- Hitstop: 90ms (vs normal 50ms)
- Boss red flash on hit
- Distinct higher-pitched impact SFX
- +1 ammo refund on aimed shot (Gunslinger/Ranger)

---

## 3. Area Skill Placement System

### Input Flow (UPDATED S47 — tap-mode)
1. **TAP** skill key -> ground indicator (radius preview circle, color-coded per skill) appears at cursor, placement mode active
2. Move mouse/stick to desired position
3. **TAP** skill key again (or LMB) -> AoE drops at indicator center
4. Right-click during placement = cancel (no cooldown spent)
5. ESC also cancels mode (no cooldown spent)

### Rules
- Max placement range: 6 tiles from player center
- Out of range: indicator turns red, clamps to max range edge in cursor direction. No null-cast.
- Cast animation: 0.2s wind-up loop while indicator active, 0.35s commit on confirm tap
- Movement locked during placement, facing locks to current cardinal
- No time dilation. World ticks normally. Enemies advance. Placement is a real risk window.

### Controller (UPDATED S47)
Press skill button = enter placement mode. Right analog stick steers indicator from player-anchored origin. Press skill button again = fire. B button = cancel.

### Which Classes Use This
ALL AoE skills across the full roster route through this system.
No cardinal-direction AoE exists in RIMA.
Primarily relevant for: Elementalist, Ranger (trap placement), Summoner, Hexer.

---

## 4. Boss Sprite Direction Spec

### Stationary Bosses (arena center, no movement)
- 1 direction: South only (facing camera)
- Production: 1 sprite set

### Mobile Bosses
- 3 directions: South, South-East, South-West
- SE and SW can mirror each other (Unity flipX) -> only 2 unique sprite sets produced
- Production: South (1 set) + South-East (1 set, mirrored for SW in Unity)
- North-facing almost never visible to player in isometric view -> omitted

### Boss Canvas Sizes
| Boss Type | Canvas | PPU | Game Size |
|---|---|---|---|
| Miniboss | 256px | 128 | 2x player |
| Act Boss | 256px | 64 | 4x player |
| Final Boss | 512px | 64 | Large/imposing |

### Boss Animation Set (per boss)
- idle (loop, 3-4 frames)
- attack_1 (main pattern)
- attack_2 (secondary or phase 2 pattern)
- hurt (2-3 frames)
- death (4-5 frames)
Each set produced via: create_image_pro (pose sheet) -> animate (per motion).
