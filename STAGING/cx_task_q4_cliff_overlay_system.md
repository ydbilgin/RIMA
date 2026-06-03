# CX TASK — Q4 HYBRID cliff: build the OVERLAY-SPRITE spawn system (implement, CliffKit_RefB pilot)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: code / STAGING / memory.

## Amaç (purpose)
Final hybrid cliff = procedural MESH underlay (done) + hand-painted rock TEXTURE on _MainTex (being produced separately) + HAND-PAINTED OVERLAY SPRITES along the boundary to break the silhouette and give natural hand-crafted detail. Build the OVERLAY-SPAWN SYSTEM now, PILOT with the EXISTING `Assets/Sprites/Environment/CliffKit_RefB/` sprites (the art the user found natural). Implement in `_IsoGame`, Unity `RIMA@ed023e0b` live.

## What exists
- `Assets/Scripts/Environment/CliffMeshGenerator.cs` on `CliffRing` in `_IsoGame` builds boundary loops (outer + inner holes) and a skirt mesh. It already computes the boundary loop points/segments and stores `cliffCells` metadata. `cliffHeightWorld=3`, sorting Ground/-10.
- `Assets/Sprites/Environment/CliffKit_RefB/`: transparent PNG sprites, PPU 64, 128x192 canvas, TOP pivot (suitable for hanging down): `cliff_N/NE/E/SE/S/SW/W/NW.png` (directional stone-spire/chunk modules) + `cliff_cyan_glow.png` (sparse cyan accent).

## Implement: overlay-spawn system
Add a new component `Assets/Scripts/Environment/CliffOverlayDecorator.cs` (or extend CliffMeshGenerator — your call, keep it clean) that, from the SAME boundary loop data, spawns overlay sprite GameObjects:
- **Sampling:** walk each boundary loop's perimeter, place an overlay roughly **every 0.7–1.1 world units** (serialized `spacing`), only on the camera-facing / void-facing lower edges primarily (don't clutter the far back-north edge as densely).
- **Direction sprite pick:** choose the CliffKit_RefB sprite from the segment's outward NORMAL quantized to S/SE/SW/E/W/N/NE/NW (use the canonical iso neighbor mapping; the mesh already knows the void side). This FIXES the old bug where everything was cliff_S.
- **Placement:** TOP-anchored at the boundary edge point so the sprite hangs DOWN below the floor lip (break the BOTTOM silhouette, keep the TOP lip clean). Add deterministic per-sample jitter (position + scale 0.7–1.2 + tiny rotation) hashed from the sample index (NOT Random — stable regen).
- **Clustering:** with `clusterChance` (~0.35) place 2–3 overlays close together (varied lengths) for natural clumps, else single.
- **Cyan accent:** `cliff_cyan_glow` on only ~8–12% of samples (`cyanChance`), never continuous.
- **Sorting:** sortingLayer `Entities` (or just ABOVE the mesh's Ground/-10), order so overlays draw in front of the mesh but BEHIND the player. Per-overlay order can vary slightly by depth. Use a child container `CliffOverlays` under CliffRing.
- **Regenerate:** `[ContextMenu("Regenerate Overlays")]` + regenerate alongside the mesh; clear old overlay children first (idempotent).
- Serialized params: `spacing`, `clusterChance`, `cyanChance`, `scaleMin/Max`, `jitter`, `seed`.

## Constraints
- UnityMCP `execute_code` action:execute, NO `using` directives (fully-qualified). After creating the script, `read_console` → 0 compile errors before wiring.
- Wire it on `CliffRing` in `_IsoGame`, assign the CliffKit_RefB sprites (load from `Assets/Sprites/Environment/CliffKit_RefB/`), regenerate, save scene.
- _IsoGame only. Do NOT enter play mode. Do NOT commit.
- This is the PILOT — sparse + deterministic. Orchestrator will screenshot + tune density after the mesh texture lands.

## Report (CODEX_DONE.md)
Component name, params/defaults, how many overlays spawned in _IsoGame, sprite-direction mapping used, 0-compile-error confirmation, sorting choice. BLOCKED + reason if boundary loop data isn't accessible from the generator.
