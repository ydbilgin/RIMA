# Council — DEEP / ARCHITECTURE / DESIGN lens (Gemini 3.1 Pro High)

You are the DEEP DESIGN advisor for the RIMA Unity ARPG. Answer with concrete, implementable design standards grounded in genre conventions. Numeric where possible.

## Context
High 3/4 top-down ARPG, close-up follow camera. Refs = Hades / Children of Morta / Diablo III. URP 2D Renderer + Pixel Perfect Camera, PPU 64, refRes 640x360. Player char actual canvas 120x120. The "chamber" is an isometric diamond-floor class-select room: ~10 class silhouettes stand on glowing oval pedestals; a cyan rift portal exits the room; a training dummy lets you preview a class before committing.

## Current problems (from live playtest screenshots)
(a) Player character sits TOO LOW on screen, near the bottom edge, huge empty dark space above.
(b) Interaction prompts ("[G] SHADOWBLADE'A GEC") float in WORLD SPACE above the world character/portal instead of fixed at the bottom-center of the screen.
(c) The whole chamber is TOO DARK — barely readable silhouettes.
(d) The training dummy ("KUKLA") is cramped against the far map edge with no room around it.

## Questions
**Q1 CAMERA FRAMING STANDARD.** In a high 3/4 top-down ARPG with a close follow camera, where should the player character sit VERTICALLY on screen during exploration? Give a concrete target (e.g. "player anchor at ~55-60% down from the top"). Should the chamber camera FOLLOW the player or be FIXED framing the whole room? Justify via genre convention (Hades = follow with lookahead; CoM/Diablo = follow centered-ish). Give exact guidance for a Unity follow-camera worldOffset.y (world units) and orthographic size given PPU 64 / refRes 640x360 so the player is comfortably framed and NOT at the bottom edge. The current code accidentally locks the camera to the room center while the player spawns low — recommend the corrected behavior + numbers.

**Q2 HADES INTERACTION PROMPT.** Research the Hades convention carefully. How does Hades actually display interaction/use prompts (e.g. the "G — interact" style hints)? Is it screen-anchored bottom-center, world-anchored above the object, or a hybrid? The user wants a fixed bottom-center SCREEN prompt. Specify exact placement + styling: screen anchor, offset from bottom edge (in 640x360 ref px), font size, button-glyph treatment ([G] key cap), fade-in timing. When multiple interactables are near, how to pick which one the prompt targets?

**Q3 CHAMBER LIGHTING.** For a moody-but-READABLE Hades-like chamber with URP 2D lights, give concrete values: global/ambient Light2D intensity, per-pedestal point Light2D intensity + outer radius + color, portal glow, and whether a soft fill light is needed. Current values (too dark): global 0.92 cool-cyan, pedestal point 0.42 intensity / 2.2 radius. Target = silhouettes clearly readable + atmosphere, NOT pitch black. Give the corrected numbers.

**Q4 DUMMY + MAP LAYOUT.** The dummy is cramped at the map edge. Recommend a chamber layout: where to place the training dummy so it has open, well-lit room (relative to the portal, the figure rows, and the player spawn); how big the floor should be; and where the player should SPAWN so the Q1 camera framing works (not at the bottom edge). Sketch the ideal top-down layout in words (portal here, figures there, dummy there, spawn there).

Keep each answer tight and numeric. This is design guidance for an orchestrator who will turn it into code.
