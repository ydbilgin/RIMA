# Task: Code-grounded — what's implementable for floating-arena lighting + room-fill (RIMA)

ACTIVE RULES: (1) think before answering (2) cite file:line for every system claim (3) analysis only, write NO code (4) BLOCKED if files missing.
NLM ACCESS: optional; if it errors, proceed from code. Direct-read: the systems below + CURRENT_STATUS.md + this file.
RESPOND INLINE -> CODEX_DONE.md. Codex (cx) task; an agy design task runs in parallel.

## Context
Demo arenas are floating islands in a void. Current lighting = gas-lamp/brazier point lights (reads absurd on a floating slab). Rooms look empty. We're moving to **cyan seal-energy emissive** lighting + **filling rooms with decor**, possibly **enclosing rooms** later. Opus + agy decide the art direction; you map what our CODE can already do and the cheapest path.

## Find + read the relevant systems
- `RoomLightingController` (if it exists) + any URP `Light2D` rig in the scene; `RoomBackgroundRig` / parallax / nebula backdrop; `EdgeFX` / cliff AO; the decor/prop placement systems (RoomPainter / prop prefabs / `AssetCategory`); `RoomLoader.OnRoomChanged` hook; `CliffAutoPlacer`.
- Grep for: Light2D usage, `RoomBackgroundRig`, `RoomLightingController`, prop/decor spawners, `OnRoomChanged`.

## Answer (code-cited)
1. **Light source swap** — where are the current gas-lamp/brazier Light2D point lights defined/spawned (scene or code)? To swap them for cyan rift-crack / seal-glyph emissive: is it a Light2D color/type change + sprite swap, or a new emitter? Cheapest hook?
2. **Per-room mood** — does `RoomLightingController` exist and hook `RoomLoader.OnRoomChanged`? Can it drive Light2D color/intensity per room (R1 intact → boss broken)? What's already wired vs missing?
3. **Room-fill** — what decor/prop placement exists (prefabs, categories, RoomPainter, auto-placers)? Can we author a per-room decor pass (centerpiece + edge framing) with existing prefabs, or is new tooling needed? What prop prefabs already exist and are import-ready?
4. **Open vs enclosed** — if we later enclose rooms (walls/interior), what in the current floor/cliff/walkability/camera systems would need to change? Rough scope.
5. **Cheapest implementable path** for: (a) replace gas-lamp light with cyan emissive, (b) fill a room with existing decor. Give concrete hook points (file:line).

## Deliverable (inline -> CODEX_DONE.md)
Per-item, code-cited. End with the single lowest-risk implementable path for "thematic light + filled room" using what already exists.
