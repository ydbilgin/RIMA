# RIMA Skill VFX Production Spec Review

## Verdict
**KOŞULLU ONAY.**

The spec is directionally correct: use Unity ParticleSystem/Shuriken for reusable short-lived VFX, reuse existing VFX prefabs, add a common SkillVfx layer, and use flipbook/sprite-sheet only where silhouette/readability needs it. This is the right demo-scale path.

But production should not start before the spec is corrected in three places:

1. Lightning color conflicts with the damage taxonomy.
2. The proposed `SkillVfx` API is too static-only for prefab/material/profile configuration.
3. Pixel-art constraints are not concrete enough to prevent soft default particles and PPU mismatch.

The whole point is to avoid building a VFX system that technically works but visually looks like a Unity tutorial sneezed on a pixel-art game.

---

## 1. Approach solidity

### Assessment
**Solid with constraints.**

Unity ParticleSystem can fit a PPU-64 pixel-art top-down game if the particles are treated like pixel sprites, not default soft circles. The spec correctly rejects PixelLab-generated explosions for procedural particles and correctly proposes “pixel-snapped chunky particle” plus selective flipbooks.

### What is right
- The CAST / TRAVEL / IMPACT split is a good reusable VFX model.
- Existing prefabs like `HitSpark`, `HandGlowVFX`, `RiftGlowVFX`, and `DeathBurst` should be reused or variant-cloned instead of rebuilt from nothing.
- `PlayerProjectile.SetOnHit(...)` is a real hook and can be used for projectile impact VFX without changing damage logic.
- Sorting layer convention `VFX`, order 20+ matches `Fireball.cs` runtime sprite sorting.
- Selective flipbooks are appropriate for frost shatter, hand-drawn melee arcs, and major impact silhouettes.

### What needs correction
- ParticleSystem must be constrained hard:
  - low count, usually 8 to 20 particles for burst effects;
  - square/chunky particle sprites;
  - Point filter import settings;
  - no default soft circular texture;
  - no per-cast material allocation;
  - pooled or self-returning objects;
  - clear sorting and lifetime caps.
- Existing `ElementalistRuntimeVisuals` creates procedural sprites with PPU 32. The project prompt says PPU 64, so this should not be the long-term fallback for production VFX.
- `new GameObject` / `Destroy` per visual is already present in some skills. The VFX pass should reduce this pattern, not multiply it.

---

## 2. Direct answer to requested evaluation points

### 1) Unity ParticleSystem + reusable SkillVfx core
**Approved conditionally.**

Use ParticleSystem for reusable small bursts, sparks, embers, dust, frost chips, and cast glows. Use flipbook/sprite-sheet or hand-drawn sprites for shape-critical effects like:

- melee swing arc;
- frost shatter burst;
- ground crack decals;
- Earthsplitter crack line;
- large Gravity Cleave arc/vortex read.

Do not use default round smooth particles. That would clash with the pixel-art direction.

### 2) Spec §8 answers
See `03_SECTION_8_ANSWERS.md`.

### 3) Reuse correctness
**Mostly correct.**

The spec correctly identifies the existing VFX prefab set and `PlayerProjectile` hook. It should add one warning: current prefab contents were only partially verified because prefab YAML was huge/truncated in the connector. Their existence is verified, but visual softness/import texture details were not fully verified.

### 4) Combat risk
**Additive layer is safe if strict boundaries are kept.**

Do not change:
- hitbox queries;
- `SkillRuntime.DealDamage(...)` calls;
- damage numbers;
- status application;
- projectile collision logic;
- cooldowns;
- movement/dash timing.

VFX calls should be inserted after or around existing events, not used as the source of truth for gameplay.

### 5) Missing critical items
The spec should add:
- sound sync hooks, even if audio assets are placeholders;
- screen-shake / hit-stop coordination rules;
- explicit pooling policy;
- material reuse rule;
- pixel import settings checklist;
- 8-direction consistency rule for projectile and melee arcs;
- player-vs-enemy telegraph color/shape separation;
- VFX debug scene or Director tab validation recipe.

---

## Final production stance
Go forward after the three required changes are applied. Without them, this will still compile, naturally, because compilers have no taste.
