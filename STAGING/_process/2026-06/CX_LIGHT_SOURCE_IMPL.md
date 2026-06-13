# Task: Code-grounded — implementable light-SOURCE rework for a floating arena (RIMA, URP 2D)

ACTIVE RULES: (1) think before answering (2) cite file:line / scene-object (3) analysis only, no code written (4) BLOCKED if missing.
NLM ACCESS: optional; if errors, proceed from code. Direct-read: scene + lighting/decor scripts + this file.
RESPOND INLINE -> CODEX_DONE.md. Codex (cx) task; an agy design task runs in parallel.

## Context
Arenas are floating slabs in a void. Current lights are discrete **Point `Light2D`** at "Brazier_*"/"Pillar_*" positions in `PlayableArena_Test01` (implies physical lamps — illogical for a floating slab). We're moving the light SOURCE to be in-world-logical: emanating from **floor seal-rift cracks (light from below/within the slab)**, **under-slab edge/abyss glow**, and/or **floating seal-shards**. agy decides the art direction; you map the URP-2D implementable path.

## Find + read
- The Light2D rig in `PlayableArena_Test01` (Point vs Freeform vs Global lights, their parents).
- URP 2D Light types available: Freeform Light2D (custom shape — good for crack-shaped/edge light), Sprite Light2D (light shaped by a sprite), Point, Global.
- Any emissive-sprite + Light2D decor prefabs (rift crystal, seal glyph). `RoomBackgroundRig`, `EdgeFX`, cliff edge objects.

## Answer (code-cited)
1. **Floor seal-rift light** — to make light read as coming FROM cracks in the floor: is a **Sprite Light2D** (using a crack/rune sprite as the light cookie) or a **Freeform Light2D** shaped along cracks the right tool? Where would these be placed (floor decor layer)? Implementable with existing prefabs/sprites or need new?
2. **Under-slab / edge abyss glow** — can a Freeform Light2D trace the island's outer edge to glow the rim from beneath? Does the cliff/edge geometry expose a usable boundary path?
3. **Floating seal-shard lights** — emissive sprite prop + small Point/Sprite Light2D as a child; do shard/crystal prefabs exist? cheapest way to add 2-3 hovering ones.
4. **Removing the brazier point-lights** — safe to delete/convert the 4 Brazier + Pillar point lights? Anything reference them by name?
5. **Per-room** — does `RoomLightingController` / `RoomLoader.OnRoomChanged` exist to drive this per room?
6. **Single cheapest implementable path** to "light logically comes from floor-rifts + edge-glow (not lamps)" using URP 2D — concrete steps + hook points (file:line / scene object).

## Deliver (inline -> CODEX_DONE.md)
Per-item, code-cited. End with the lowest-risk implementable path + exact objects/scripts to touch.
