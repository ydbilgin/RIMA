# RIMA — Skill VFX Production Spec Review Fix Prompt for Claude

Repo: `github.com/ydbilgin/RIMA`  
Branch: `master`  
Context: Unity 2D top-down pixel-art roguelite ARPG, C#, PPU 64.  
Target: Warblade + Elementalist demo VFX pass.

## Task
Read the review package in this zip, then revise `STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md` before production. Do **not** start coding VFX until the three required changes below are reflected in the spec.

## Verdict
**KOŞULLU ONAY.**

The overall direction is sound for demo: reusable VFX core + existing ParticleSystem prefabs + projectile impact hooks + selective flipbook/sprite-sheet use. But the spec needs corrections before implementation, otherwise it will create avoidable art inconsistency and some delightful little technical debt creatures.

## Must-change before production
1. **Fix color taxonomy mismatch.**
   - Spec currently maps Lightning to `#FFD24A`.
   - Taxonomy decision maps Lightning to `#FFE600` because `#FFD24A` is Crit gold.
   - Update spec and VFX mapping accordingly.

2. **Replace the single static API with a small hybrid API.**
   - Keep a simple static façade for skill scripts: `SkillVfx.Play(...)`.
   - Back it with `VfxProfile` / prefab mappings so colors, prefabs, lifetime, sorting, particle caps, and material references are data/config driven.
   - Do not make every skill own its own full SO if variation is minimal. Use archetype profiles first, skill override profiles only when actually needed.

3. **Define pixel-art VFX implementation rules explicitly.**
   - ParticleSystem is allowed, but only with chunky pixel sprites, Point filter, low particle count, short lifetime, sorted to VFX layer.
   - No default soft round blobs.
   - Do not instantiate `new Material` per cast. Preload/reuse materials and pool short-lived VFX objects.
   - Fix/avoid PPU-32 runtime placeholder sprites in a PPU-64 project.

## Implementation constraints
- VFX must be additive and must not change DamagePacket, damage numbers, hitbox geometry, cooldowns, timings, status application, chain logic, or movement logic.
- Use existing hooks where possible:
  - `PlayerProjectile.SetOnHit(...)` for projectile impact VFX.
  - Direct `SkillVfx.Play(...)` after existing hit detection loops for instant/AOE/melee skills.
  - Do not replace `SkillRuntime.DealDamage(...)` calls during this VFX pass.
- Visual QA is mandatory in Play Mode. Compile success alone is not enough, because apparently Unity can compile ugly things very confidently.

## Files to inspect
- `STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md`
- `STAGING/DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md`
- `Assets/Scripts/Skills/Elementalist/Fireball.cs`
- `Assets/Scripts/Skills/Elementalist/GlacialSpike.cs`
- `Assets/Scripts/Skills/Elementalist/ChainLightning.cs`
- `Assets/Scripts/Skills/Elementalist/ElementalistRuntimeVisuals.cs`
- `Assets/Scripts/Skills/Warblade/IronCharge.cs`
- `Assets/Scripts/Skills/Warblade/GravityCleave.cs`
- `Assets/Scripts/Skills/Warblade/Earthsplitter.cs`
- `Assets/Scripts/Skills/PlayerProjectile.cs`
- `Assets/Prefabs/VFX/HitSpark.prefab`
- `Assets/Prefabs/VFX/DeathBurst.prefab`
- `Assets/Prefabs/VFX/HandGlowVFX.prefab`
- `Assets/Prefabs/VFX/RiftGlowVFX.prefab`

## Expected output from Claude
1. Updated spec file or patch proposal.
2. A short implementation plan with phase order.
3. A risk note listing what was intentionally not touched.
4. A Play Mode visual QA checklist.
5. No blind “looks good” claims unless screenshots or Unity play validation exist.
