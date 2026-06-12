# Short Task Version for Claude

RIMA repo: `github.com/ydbilgin/RIMA`, branch `master`.

Review result for `STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md`: **KOŞULLU ONAY**.

Before implementing VFX, patch the spec:

1. Fix Lightning color:
   - Use `#FFE600`.
   - Reserve `#FFD24A` for Crit only.

2. Replace pure static `SkillVfx` with static façade + profile-backed registry:
   - Skill code can call `SkillVfx.Play(...)`.
   - Prefab/material/lifetime/sorting/particle caps should live in `VfxProfile` or a small database.
   - Skill-specific SOs only when needed.

3. Add hard pixel-art VFX rules:
   - chunky square/diamond particles;
   - Point filter;
   - no soft default blobs;
   - low particle count;
   - no `new Material` per cast;
   - pooling required;
   - PPU 64, do not rely on PPU-32 runtime placeholders.

Then implement in this priority order:
1. `SkillVfx` core + pooling + profile registry.
2. Fireball cast/trail/impact via `PlayerProjectile.SetOnHit`.
3. Warblade basic arc + small HitSpark.
4. Gravity Cleave arc/vortex/pull visual.
5. Chain Lightning shared-material jagged LineRenderer + hit sparks.
6. Iron Charge dash trail + impact sparks.
7. Earthsplitter crack/debris waves.
8. Glacial Spike frost crack/shatter.
9. Elementalist basic mini bolt.

Do not change combat logic: no DamagePacket/hitbox/cooldown/status/movement edits in this VFX pass.

Play Mode visual QA is mandatory. If not visually verified, mark commit `[visual unverified]`.
