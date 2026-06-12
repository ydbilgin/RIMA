# Test and Visual QA Checklist

## Compile checks
- Unity compile passes.
- No missing prefab references in `SkillVfxDatabase` / profiles.
- No missing material references.
- No missing sprite references.
- No namespace mismatch.
- No build-only references to editor APIs.

## Runtime safety checks
- No `new Material(...)` inside repeated cast/hit code.
- No unbounded `new GameObject(...)` for high-frequency VFX unless pooled or extremely temporary and justified.
- No per-frame Resources.Load.
- No per-frame texture creation.
- No VFX object persists forever after effect ends.
- No VFX sorting behind floor or above UI incorrectly.
- No particle burst exceeds agreed particle caps.

## Combat non-regression checks
For each skill, confirm:
- Damage value unchanged.
- Cooldown unchanged.
- Resource/rage cost unchanged.
- Hitbox radius/line/box unchanged.
- Status effects unchanged.
- Movement/dash timing unchanged.
- Chain target logic unchanged.
- Projectile velocity/lifetime/collider unchanged.

## Visual checks by skill

### Fireball
- Cast flash appears at correct hand/origin.
- Fireball projectile still uses 8-direction visual or correct fallback.
- Trail follows projectile and does not smear too smoothly.
- Impact burst appears exactly on hit target.
- Burning status still applies.

### Glacial Spike
- Spike/crack aligns with hitbox direction.
- Frost visual is readable on dark floor.
- Frost is distinguished by shard shape, not just cyan-blue color.
- Freeze/chill combo still works.

### Chain Lightning
- Bolt connects actual chain targets.
- Bolt is jagged and pixel-ish.
- No `new Material` allocations per bolt.
- Lightning color is `#FFE600`.
- It does not look like Crit feedback.

### Warblade basic
- Arc direction matches player facing.
- Arc does not hide enemy or damage number readability.
- HitSpark is small and punchy.
- Repeated basic attacks do not flood the screen.

### Iron Charge
- Dash trail shows direction and speed.
- Impact sparks/dust appear on unique target hits.
- Trail ends when charge ends.
- No lingering ghost trail.

### Gravity Cleave
- Wide arc/vortex reads as pull/cleave.
- AoE size visually matches actual radius closely enough.
- Chained version has stronger/stun variant if desired, but does not imply a different hitbox.

### Earthsplitter
- Three waves are visually distinct.
- Crack line matches actual `EnemiesInLine` direction.
- Dust/debris does not obscure the player.
- Timing remains 0.08s between waves.

### Elementalist basic
- Mini bolt is clear but lower intensity than Fireball.
- Mini impact does not look like a major skill.

## Player vs enemy telegraph readability
- Player VFX uses saturated skill colors.
- Enemy danger telegraphs must use a separate visual language, preferably red/orange warning shape, not identical player impact colors.
- Player VFX should not create floor zones that look like enemy danger zones.
- Screen shake/hit-stop, if used, must be minor for basic attacks and stronger only for major skills.

## Performance checks
- Spam Fireball for 20 casts.
- Spam Warblade basic for 30 swings.
- Trigger Chain Lightning on 5+ enemies repeatedly.
- Trigger Earthsplitter waves repeatedly.
- Watch Unity profiler for GC allocation spikes.
- Confirm pooled objects return to pool.
