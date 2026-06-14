# AX DESIGN CONSULT — ISOMETRIC FLOOR LOOK (RIMA, 2026-06-01)

You are a game-art / visual-direction consultant. Return a concise, opinionated recipe. No code. Output a focused summary (this will be read by Opus who makes the final call).

## The problem (user verbatim, Turkish)
"ax ve cx'e de sor istediğim şekilde olmuyor gibi şu an topdown oluyor isometric olacak benim oyunum"
→ The game MUST read ISOMETRIC (Hades / Children of Morta / Bastion family). The current demo floor reads TOP-DOWN. The user wants it fixed.

## What exists
- A floating-island arena: a diamond-shaped field of 64×64 floor tiles in a dark void.
- The floor tiles were generated FLAT and TOP-DOWN (PixelLab, camera straight-down at 90°, zero depth) because in a PREVIOUS round the user complained the older tiles were "lumpy / popping up like 3D blocks" and asked for them to be "on the same plane" (flat). The pipeline over-corrected into pure top-down flat tiles.
- Cliff side-face sprites exist (a "KitB" cliff set: S/SE/SW/E/W faces, tall 128×192) that can hang below the island edges.
- Camera is orthographic 2D.

## QUESTIONS — answer all, concise
1. **What actually makes a 2D floor READ as isometric vs top-down?** Rank the contributing factors (tile top-face projection angle, visible vertical surfaces/walls/cliffs, object height & 3/4 sprites, cast shadows, camera). Which 2-3 matter most?
2. **The flat top-down floor tiles** — are they a dead end for an iso look, or can they be rescued by surrounding depth (cliffs/walls/objects)? In Hades, is the FLOOR itself drawn in iso perspective, or is it fairly flat with the iso read coming from walls/props/characters? Give the reference-game truth.
3. If the tiles SHOULD be regenerated as isometric: describe the target look in words (top-face shown at an iso angle, subtle but NOT lumpy, seamless tessellation, value/contrast range) so it reads iso-ground without each tile popping as a separate 3D block. Reconcile "isometric" with the user's earlier "same plane / not lumpy" demand — what's the sweet spot?
4. **Floating-island depth:** how should the cliff side-faces + contact shadow be composed under the diamond so it reads "iso island hovering in void" and NOT "top-down hole"? Camera-front edges only (S/SE/SW) or all? Cyan rim/rift accent budget (project canon: cyan #00FFCC ~5-8% of surface).
5. **Camera:** any orthographic-size / framing guidance for the iso read.

Be specific and reference Hades/CoM/Bastion/Diablo where useful. This is a direction call, keep it under ~400 words.
