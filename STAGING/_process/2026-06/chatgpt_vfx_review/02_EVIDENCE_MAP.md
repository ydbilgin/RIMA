# Evidence Map

This file maps the review claims to concrete repo observations.

## Main spec exists and is current
File: `STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md`

Observed:
- Scope: Warblade and Elementalist opening-kit VFX plus two basic attacks.
- Approach: Unity ParticleSystem/Shuriken, not PixelLab explosions.
- Proposed solution: pixel-snapped chunky particles plus selective flipbook sprite-sheets.
- Reuse: HitSpark, DeathBurst, HandGlowVFX, RiftGlowVFX, PlayerProjectile impact hook, Fireball 8-dir sprites.

Key issue:
- Spec maps Lightning to `#FFD24A`, but taxonomy says Lightning is `#FFE600` and `#FFD24A` is Crit.

## Damage taxonomy
File: `STAGING/DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md`

Observed:
- `DamageType`: Physical, Ability, True.
- `ElementTag`: None, Fire, Frost, Lightning, Void, Light, Poison.
- ElementTag is visual/status/future-resist only, not damage math.
- Colors:
  - Physical: `#E89020`
  - Ability/Magic: `#00FFCC`
  - True: `#F4F0E6`
  - Crit: `#FFD24A`
  - Fire: `#FF6A1F`
  - Frost: `#7FE0FF`
  - Lightning: `#FFE600`
  - Void: `#7B3FA8`
  - Light: `#FFF0B0`
  - Poison: `#7BC043`

Review implication:
- `VfxElement` should not casually invent a separate permanent taxonomy that drifts from this file.
- `Arcane` can exist as VFX alias for Ability/Magic cyan, but do not confuse it with `ElementTag` unless the taxonomy is changed.

## Fireball.cs
File: `Assets/Scripts/Skills/Elementalist/Fireball.cs`

Observed:
- Uses 8-direction sprites loaded from `Resources/VFX/Fireball/`.
- Runtime fallback uses `ElementalistRuntimeVisuals.GetCircleSprite()`.
- Runtime projectile uses `SpriteRenderer` sorting layer `VFX`, sorting order `20`.
- Projectile gets `PlayerProjectile.Init(...)` with burn settings.
- Current Fireball does not wire `SetOnHit(...)`; this is where impact VFX should be added.

Review implication:
- Spec is correct that Fireball has an existing directional projectile basis.
- Add cast flash, trail, and impact via additive VFX calls/hooks; do not change projectile damage/hitbox.

## ElementalistRuntimeVisuals.cs
File: `Assets/Scripts/Skills/Elementalist/ElementalistRuntimeVisuals.cs`

Observed:
- Creates procedural circle and crack sprites at runtime.
- Texture filter mode is Point.
- Sprite PPU is `32f`, while current requested project context says PPU 64.
- Crack sprite comment says `TODO: replace with PixelLab crack decal art`.

Review implication:
- These are acceptable placeholders, not production VFX anchors.
- Spec should explicitly replace or quarantine PPU-32 placeholders.

## PlayerProjectile.cs
File: `Assets/Scripts/Skills/PlayerProjectile.cs`

Observed:
- Has `SetOnHit(Action<Collider2D>)` callback.
- Has `SetDamagePacket(DamagePacket packet)` path.
- On trigger, after damage is resolved, `onHit?.Invoke(other)` is called.
- Non-packet path directly calls `hp.TakeDamage(finalDamage)` and `SkillRuntime.PublishSkillHit(...)`.

Review implication:
- `SetOnHit` is a valid impact VFX hook.
- VFX can be attached without touching damage math.
- Do not use this VFX pass to silently refactor projectile damage packet behavior unless that is a separate combat fix task.

## ChainLightning.cs
File: `Assets/Scripts/Skills/Elementalist/ChainLightning.cs`

Observed:
- Already uses a `LineRenderer` arc visual.
- It spawns a circle visual at each hit.
- It creates a `new Material(Shader.Find("Sprites/Default"))` per arc.
- It destroys the arc after `0.12f`.

Review implication:
- LineRenderer is the right base for demo chain lightning.
- Must avoid per-cast material allocation.
- Upgrade it to jagged/snap-segment pixel lightning rather than particle trail.

## GlacialSpike.cs
File: `Assets/Scripts/Skills/Elementalist/GlacialSpike.cs`

Observed:
- Hit detection uses `Physics2D.BoxCastAll` line region.
- Visual loads `Resources/VFX/Skills/glacial_spike_cluster`.
- Fallback uses `ElementalistRuntimeVisuals.GetCircleSprite()`.
- Creates temporary GameObjects and fades via coroutine.

Review implication:
- The visual already has a sprite/decal direction.
- Replace the fallback circle with production frost-shard visuals or a controlled VFX prefab.
- Keep hit detection untouched.

## IronCharge.cs
File: `Assets/Scripts/Skills/Warblade/IronCharge.cs`

Observed:
- Movement/damage is driven in `FixedUpdate` while `charging` is true.
- Damage uses `SkillRuntime.DealDamage(hp, damage)` inside the CircleCast loop.
- Applies stun/knockback/rage.

Review implication:
- VFX should attach to charge start, dash trail during charge, and impact sparks on first target contact.
- Do not touch `rb.linearVelocity`, `chargeTimer`, `hitTargets`, or rage/status logic.

## GravityCleave.cs
File: `Assets/Scripts/Skills/Warblade/GravityCleave.cs`

Observed:
- AoE radius logic uses `Physics2D.OverlapCircleAll`.
- Pull force is applied toward player position.
- Status changes depend on chained state.
- Current only visual-ish code is `OnDrawGizmosSelected`.

Review implication:
- Needs a clear visual tell: swing arc + vortex/pull burst + ground crack.
- If adding a pre-tell delay would change combat timing, avoid it for demo. Use instantaneous anticipation flash instead.

## Earthsplitter.cs
File: `Assets/Scripts/Skills/Warblade/Earthsplitter.cs`

Observed:
- Coroutine emits 3 waves with `0.08s` spacing.
- Hit detection uses `SkillRuntime.EnemiesInLine(...)`.
- Current visual is `SkillRuntime.SpawnCircleVisual(...)`.

Review implication:
- Replace circle placeholder with crack-line/debris VFX per wave.
- Preserve coroutine timing and `EnemiesInLine(...)` geometry.

## Existing VFX prefabs
Files verified by direct URL:
- `Assets/Prefabs/VFX/HitSpark.prefab`
- `Assets/Prefabs/VFX/DeathBurst.prefab`
- `Assets/Prefabs/VFX/HandGlowVFX.prefab`
- `Assets/Prefabs/VFX/RiftGlowVFX.prefab`

Observed:
- All exist and are ParticleSystem-based prefabs.
- Full visual/material/texture softness could not be completely verified because connector output was truncated on large YAML files.

Review implication:
- Reuse is valid, but first production step should inspect/import-test these prefabs in Unity and clone variants if needed.
