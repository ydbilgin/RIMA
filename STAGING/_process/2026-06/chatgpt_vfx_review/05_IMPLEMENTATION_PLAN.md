# Recommended Implementation Plan

## Phase 0 — Spec patch only
Before code:
1. Fix Lightning color to `#FFE600`.
2. Add static façade + VfxProfile/registry architecture.
3. Add pixel VFX rules.
4. Add pooling/material reuse rules.
5. Add visual QA checklist.

Commit this as spec-only.

## Phase 1 — Core VFX system
Add:
- `Assets/Scripts/VFX/SkillVfx.cs`
- `Assets/Scripts/VFX/VfxElement.cs`
- `Assets/Scripts/VFX/VfxArchetype.cs`
- `Assets/Scripts/VFX/VfxProfile.cs`
- `Assets/Scripts/VFX/SkillVfxDatabase.cs` or Resources-backed registry
- `Assets/Scripts/VFX/PooledVfxInstance.cs`

Rules:
- No skill logic changes.
- No DamagePacket or hitbox edits.
- No material allocation in runtime hot paths.
- Pool short-lived VFX prefabs.

## Phase 2 — Prefab variants / material audit
Inspect existing prefabs in Unity:
- HitSpark
- DeathBurst
- HandGlowVFX
- RiftGlowVFX

Create variants if needed:
- `VFX_HitSpark_Physical`
- `VFX_HitSpark_Fire`
- `VFX_HitSpark_Frost`
- `VFX_HitSpark_Lightning`
- `VFX_CastFlash_Fire`
- `VFX_CastFlash_Frost`
- `VFX_CastFlash_Lightning`
- `VFX_CastFlash_Physical`
- `VFX_GroundCrack_Physical`
- `VFX_MeleeArc_Physical`
- `VFX_ChainBolt_Lightning`

Do not overwrite the original prefabs blindly. Variant them or copy intentionally.

## Phase 3 — Elementalist VFX wiring

### Fireball
Current:
- 8-direction sprite projectile exists.
- PlayerProjectile exists.

Add:
- Cast flash at hand/origin.
- Trail on projectile object or child.
- `SetOnHit(...)` callback for impact burst.
- Optional ember particles on impact.

Do not change:
- projectile speed;
- damage;
- burn duration;
- collider radius;
- lifetime.

### Glacial Spike
Current:
- Line BoxCast hit logic.
- 3 spike visuals along line.

Add/replace:
- Cast frost shimmer.
- Ground spike/crack sprite or flipbook along line.
- Frost shatter impact at hit enemies.
- Chill/freeze shape language, not just blue tint.

Do not change:
- BoxCast dimensions;
- damage;
- status logic;
- Fireball combo behavior.

### Chain Lightning
Current:
- Target chain logic exists.
- LineRenderer arc exists.
- Current code creates new material per arc.

Add/replace:
- Shared material or pooled LineRenderer prefab.
- Jagged 3-5 point line.
- Hit spark per target.
- Cast static flash.

Do not change:
- chain count;
- jump range;
- damage falloff;
- shocked duration;
- target selection logic.

## Phase 4 — Warblade VFX wiring

### Warblade basic attack
Add:
- hand-drawn melee arc sprite or pixel mesh arc;
- small physical HitSpark on contact;
- no soft trail.

Do not confuse finisher/crit visual logic unless combat crit system is explicit. This project already had a finisher/crit ambiguity risk elsewhere.

### Iron Charge
Current:
- Dash movement in FixedUpdate.
- CircleCast hits enemies.

Add:
- Start body flash.
- Dash trail behind player during charge.
- Impact dust/spark on first hit or each unique target.

Do not change:
- velocity assignment;
- charge duration;
- hitTargets set;
- rage gain;
- stun and knockback.

### Gravity Cleave
Current:
- OverlapCircle AoE.
- Pull and slow/stun logic.

Add:
- Wide melee arc sprite.
- Void/physical vortex around caster.
- Pull streaks toward center.
- Ground crack pulse.

Do not add cast delay unless explicitly approved, because that changes gameplay timing.

### Earthsplitter
Current:
- 3 coroutine waves.
- Circle visual placeholder.

Add:
- Ground crack decal/line per wave.
- Dust/debris burst along line.
- Optional small camera shake on wave spawn, if screen-shake system exists and is approved.

Do not change:
- wave count;
- 0.08s spacing;
- EnemiesInLine geometry;
- stun/Broken state logic.

## Phase 5 — Visual QA
Run Play Mode with:
- Warblade basic repeated 20 times.
- Elementalist basic repeated 20 times.
- Each of 6 skills used in empty room and in combat room.
- Skill spam test for GC/material allocation spikes.
- Camera at real demo zoom.
- Dark floor/background map.
- Enemy VFX visible nearby to verify player telegraph separation.

Commit only after compile + Play Mode visual capture. If screenshots are not reviewed, mark commit `[visual unverified]`.
