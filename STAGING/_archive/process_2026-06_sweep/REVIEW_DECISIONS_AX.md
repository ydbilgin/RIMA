# AX REVIEW — validate 2 S6 strategic decisions (reviewer role, NOT writer)

ACTIVE RULES: (1) think before reviewing (2) min, no speculation (3) surgical (4) flag if unclear.
NLM ACCESS: query NLM for RIMA canon if needed:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Respond INLINE in your AGY_DONE file. You are a REVIEWER: AGREE / DISAGREE + one-line why per point. Do NOT write code/files.

Read `STAGING/DECISIONS_S6.md`. Two Opus decisions to validate:

1. **Audio/anim timing:** spec now, produce later; animation = user-gated; audio AFTER animation (so SFX lock to real
   anim frames); the mechanical timing layer (hitstop tiers, commit-beat frame, attack startup) is the animation skeleton.
   - Is "audio after animation" the right call for a wishlist demo, or should a minimal SFX pass come first for feel? Why?

2. **Mob/boss art direction:** demo NOW = graybox (already in scene); FINAL base mobs = archive-restore + slate recolor
   (0-gen); FINAL boss = high-quality gen (128-192px, chained, cyan seal-energy, 8-dir, GATED). Cyan reserved for
   player/rift/telegraph; mobs muted slate; boss carries cyan via chains/seal-cracks.
   - Does graybox-now + archive-mobs + gen-boss maximize a playable-demo-fast goal? Any genre/readability risk in
     "mobs muted, only boss+player+rift cyan"? Any reason to gen base mobs instead of archive-restore?

Be terse. If you DISAGREE strongly on either, give the alternative in one line (Opus will weigh it; big conflicts go to the user).
