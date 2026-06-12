# Review Summary for Opus / Council

## Decision
**KOŞULLU ONAY.**

## Why not full ONAY?
The core approach is good, but the spec has three production-blocking cleanup items:

1. Color mismatch: Lightning should be `#FFE600`, not Crit gold `#FFD24A`.
2. Static-only `SkillVfx` API is too rigid for tuning. Use static façade + profile registry.
3. Pixel-art implementation rules are too vague. Must explicitly forbid default soft particles, runtime material allocation, and PPU mismatch.

## Why not RED?
Because the direction is correct:
- ParticleSystem is fine for small pixel-styled particles.
- Flipbooks/sprites are correctly reserved for silhouette-critical moments.
- Reusing HitSpark/HandGlow/RiftGlow/DeathBurst is sensible.
- `PlayerProjectile.SetOnHit` exists and supports additive impact VFX.
- Current Warblade/Elementalist skills have obvious safe insertion points.

## Top risk
The biggest risk is not combat correctness if the VFX layer stays additive. The biggest risk is visual mismatch: smooth Unity glow, color confusion, excessive particles, and trail smears that look modern instead of pixel-art.

## Production recommendation
Patch spec first, then implement the core reusable system and only then wire skills. Do not hand-code one-off VFX into every skill script.
