# CONSULT (industry/UX research) — VFX for pixel-art action roguelites: hand-animated sprites vs engine particles

ACTIVE RULES: (1) think before answering (2) no speculation, cite games (3) RIMA-fit is criterion #1 (4) say BLOCKED if unclear.
Respond INLINE in your final message (NOT a file — dispatcher captures stdout). ~1 page, decision-useful.

## What RIMA is
2D **top-down** pixel-art ARPG roguelite, URP 2D, **PPU 64, pixel-perfect**, cyan rift #00FFCC brand over dark-purple
void. Fast crunchy combat (Hades/Children-of-Morta feel). It can produce VFX two ways: **PixelLab** (AI pixel-art
sprite-sheet generator → flipbook) or **Unity engine-native** (ParticleSystem / shaders / TrailRenderer / procedural).

## The question (synthesized with a Codex code consult + Opus final)
1. **Sprite-sheet vs engine particles for PIXEL ART:** In top-down/2D pixel action games (cite: Dead Cells, Children
   of Morta, Nuclear Throne, Brotato, Halls of Torment, Death Must Die, Skul, Blasphemous, Moonlighter, Enter the
   Gungeon, Hyper Light Drifter), which VFX are **hand-animated sprite sheets** vs **engine particle systems**, and
   why? What's the actual industry norm — and the "pixelated particle" technique (point-filtered low-res particle
   textures, palette-locked, pixel-snapped) that lets engine particles match pixel art?
2. **Which VFX categories are WORTH hand-animating** (signature/hero moments) vs which should stay procedural
   (high-frequency, directional, gameplay-reactive)? Give a concrete split for: hitsparks, slash arcs, dash trails,
   knockback dust, projectile trails, status auras (burn/freeze/poison), big skill payoffs (meteor/explosion),
   boss-death, rift/portal opens, ultimate bursts, ambient environment loops.
3. **The directional problem:** pixel-art sprites rotate badly (break the grid). For directional effects (dash trail,
   8-dir slash), do these games bake per-direction sprites, rotate anyway, or use procedural emission? What's the
   pro move for a PPU64 top-down game?
4. **Cost/scale reality:** hand-animated VFX cost art time + (for RIMA) the PixelLab animate step is now
   user-approval-gated. Given a small team + demo scope, what's the highest-ROI VFX strategy — and the most common
   mistake indie pixel roguelites make with VFX (too smooth? too few frames? clashing resolution? over-partic­ling?).
5. **One paragraph:** the single most important thing for RIMA's VFX to read as cohesive, crunchy pixel art.

Reference: two RIMA VFX already exist as PixelLab sprites — a radial 7-frame hitspark (direction-agnostic) and a
directional 9-frame dash trail (baked one orientation). Use them to illustrate the right/wrong fit. No code.
