# Council — LEAN / SHIP-FAST / OVER-ENGINEERING CRITIQUE lens (Gemini 3.5 Flash High)

You are the PRAGMATIC advisor. Your job: the LEANEST path to fix these problems, and to call out any over-engineering. The orchestrator will get a deep-design answer separately; YOU give the ship-fast counter-angle. Numeric, terse.

## Context
RIMA, high 3/4 top-down ARPG, close follow camera (refs Hades/Children of Morta/Diablo III). URP 2D + Pixel Perfect Camera, PPU 64, refRes 640x360, player sprite 120x120. Chamber = class-select room: ~10 class silhouettes on glowing oval pedestals, a cyan rift portal exit, a training dummy to preview a class.

## Problems (live playtest)
(a) player too LOW on screen (bottom edge), big empty dark space above.
(b) interaction prompt "[G] SHADOWBLADE'A GEC" floats in WORLD space above the character/portal; user wants it fixed at screen bottom-center (Hades-style).
(c) chamber TOO DARK.
(d) training dummy cramped at far map edge.

## Reusable code that already exists (do NOT rebuild from scratch)
- CameraFollow component (worldOffset, smoothTime) — currently mis-set so camera locks to room center while player spawns low.
- HUDController already has a SCREEN-SPACE bottom-center interaction prompt (anchor 0.5,0.15) + SetInteractionPrompt(), but it's combat-only and not used by the chamber. Chamber instead draws a WORLD-space TextMeshPro prompt.
- Light2D global + per-pedestal point lights already wired (global 0.92, pedestal 0.42/r2.2).

## Questions — give the LEANEST fix for each
Q1 CAMERA: Simplest change to stop the player rendering at the bottom edge. One-liner offset fix vs bigger camera rework? Give the exact worldOffset.y and ortho size you'd ship (PPU 64 / refRes 640x360). Follow player or fix-frame the room — which is less work and good enough?

Q2 PROMPT: Leanest way to get a bottom-center screen prompt — reuse the existing HUDController screen prompt by adding it to the chamber, or just swap the chamber's world-text for a tiny screen-space Text? Which is fewer lines? Flag if multi-interactable target logic is over-engineering for this room.

Q3 LIGHTING: Minimum value bumps to make it readable — is it just "raise global intensity to X"? Give the single smallest change first, then optional extras. Don't over-light.

Q4 DUMMY+MAP+HUD: Cheapest way to give the dummy room (move it to an open cell vs enlarge the whole map?) and where the player should spawn for Q1. Plus: leanest way to show the Echo count in the chamber (full HUDController vs a 5-line label?). Call out anything that's over-scoped for a demo class-select room.

Be blunt about what NOT to build.
