# CONSULT (code architecture) — RIMA VFX: PixelLab-animated sprites vs engine-native

ACTIVE RULES: (1) think before answering (2) min code, no speculation (3) surgical — read listed files only (4) BLOCKED if unclear.
NLM ACCESS: query NLM if you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Respond INLINE (captured to CODEX_DONE.md). Do NOT write/edit files. ~1 page, concrete.

## The question (synthesized with an ax research consult + Opus final)
Is it better to (A) generate VFX as **PixelLab animated sprite-sheets** and play them as flipbooks, or (B) build them
**engine-native** (Unity ParticleSystem / shaders / procedural / the prefabs RIMA already has)? And **which specific
RIMA VFX should be sprite vs native?** RIMA-fit (pixel-perfect PPU64, cyan #00FFCC brand, demo scope) is criterion #1.

## Two real reference objects (user-made; READ-only context — do not regenerate)
- `11127e69` "Hitspark burst" — 64x64, 7 frames, **radial/symmetric** single-shot impact flash. Direction-agnostic.
- `58c183a0` "Dash trail" — 64x64, 9 frames, **directional** looping motion-smear. Baked at one orientation.
Opus's prior: radial/symmetric effects are good sprite candidates; directional/variable-length effects (dash, slash)
fight the pixel grid when rotated and can't adapt to 8-dir/scale → usually better native or bake-per-direction.

## Your job: ground this in RIMA's ACTUAL current VFX code
Read and report what EXISTS today and how it's built (native vs sprite), then recommend keep/swap per effect.
- `Assets/Scripts/**/SlashArcVFX*.cs`, `HitSpark*`/`HitImpact*`, `VFXRouter*`, `CombatJuice*`, `ScreenShake*`,
  `ImpactFrame*`/`ImpactFrameDriver`, `HitFlash*`, any `ParticleSystem`/`TrailRenderer`/`LineRenderer` usage,
  `DamageNumber*`, dash VFX hook in `PlayerController` (`CombatEventBus.PublishDash`), skill VFX
  (`ElementalistRuntimeVisuals`, projectile/AoE skills under `Assets/Scripts/Skills/**` and `Assets/Scripts/Combat/**`).
- Grep for: `ParticleSystem`, `TrailRenderer`, `Animator`/`SpriteRenderer` flipbook VFX, `Resources.Load` of VFX sprites.

## Deliverable (inline)
1. **Inventory:** for each RIMA VFX in play today (hitspark, slash arc, dash trail, knockback dust, projectile trail,
   class skill payoffs, status auras, boss/death, screen-space impact-frame/vignette), say native or sprite TODAY.
2. **Per-effect recommendation:** keep-native / swap-to-PixelLab-sprite / hybrid (sprite core + native garnish).
   Weigh: pixel-perfect/rotation cost, 8-dir need, gameplay-reactivity (scale/tint/direction), instance count.
3. **Integration cost** of adding a PixelLab sprite-flipbook VFX into RIMA's current pipeline (import settings PPU64
   point-filter; Animator vs simple frame-driver; how VFXRouter/CombatJuice would spawn it; pooling).
4. **The directional/pixel-rotation problem:** for slash arc + dash trail, is per-direction baking (8 sprites) or
   native emission the cleaner RIMA fit? Code reasoning.
5. **Risks / what NOT to touch** in the working juice stack.
