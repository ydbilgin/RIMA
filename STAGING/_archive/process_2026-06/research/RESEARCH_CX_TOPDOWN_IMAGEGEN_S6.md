ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

# RESEARCH (no edits) — TOP-DOWN pivot: image-gen tools, resolution, bulk-vs-modular, room size

ANALYSIS ONLY. No Unity, no edits. Output to CODEX_DONE.md under `## CX RESEARCH - TOPDOWN IMAGEGEN`.

## Context
After a long isometric-floor struggle, the user may PIVOT the whole game to TOP-DOWN. They want to possibly use BULK whole-map images (drawn by an image-gen tool) instead of modular tiles. Character sprites are 64px (PPU 64). RIMA = 2D ARPG roguelite (Unity 6000.3, URP 2D). They asked: which image-gen tools are available (Codex imagegen / Codex Plus account / Gemini / other), at what resolution maps get drawn, bulk vs modular, and room size relative to a 64px character.

## QUESTIONS — concrete + numeric
1. **Available image-gen tools in THIS environment** for drawing top-down 2D maps/rooms. Enumerate what is actually usable here and how invoked: (a) Codex `image_gen` via the local skills (`generate2dmap`, `generate2dsprite`) and the Codex Plus/ChatGPT-image path (`gpt-image-2` skill); (b) Gemini image gen (nano-banana / Imagen) if reachable; (c) RunComfy models (`flux-2-klein`, `seedream`, etc.) if the `runcomfy` CLI is present; (d) PixelLab (pixflux create map). For each: is it reachable now, max output resolution, cost, transparent-PNG support, top-down map suitability. Verify presence by checking for the CLIs/skills, do not assume.
2. **Resolution math for 64px character.** If character ≈ 64px at PPU 64 (≈1 world unit tall), and a room should hold ~N×N tiles of combat space, what PIXEL resolution should a BULK room image be? Give a table: room logical size (in 64px tiles) -> image px (e.g. 20x15 tiles -> 1280x960 px) -> import PPU. Note image-gen model max-res limits (most cap ~1024-2048) and the downscale/tiling implications.
3. **Bulk whole-map image vs modular tiles** in Unity for an ARPG: integration tradeoffs — collision/walkable authoring (a single image has no per-cell data; how to add colliders/nav), layering (props/entities above floor), variety/reuse, regen cost, file size, runtime memory. When is bulk better than modular? Recommend for RIMA's roguelite (procedural variety need).
4. **Room size relative to 64px char.** User says rooms should be "a bit bigger" than current. Give concrete combat-room dimensions (world units + px) for a 64px character ARPG (dodge/space for mobs+boss), referencing Hades/CoM room scales. Camera orthographic size implications.

Keep tight, numeric, evidence where possible (check files/CLIs). Flag anything you cannot verify.
