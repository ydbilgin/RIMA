# CX TASK — Q4 CLIFF: make the mesh read as NATURAL ROCK with depth (procedural, no texture dependency)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: code / STAGING / memory.

## Amaç (purpose)
The core procedural cliff MESH works (continuous skirt, taper, dissolve into void) but currently reads as a FLAT dark band — not like real rock with depth. User wants it to look like a believable cliff with NATURAL depth, like a floating island underside (Hades / Children of Morta). Make it look natural using PROCEDURAL detail in the shader + irregular mesh bottom — NO external texture dependency yet (a hand-painted PixelLab texture is a LATER pass). Work in `_IsoGame`, Unity `RIMA@ed023e0b` live.

## Current state (orchestrator already did)
- `Assets/Scripts/Environment/CliffMeshGenerator.cs` on `CliffRing` in `_IsoGame`: 1 outer loop, 256-vert skirt, `cliffHeightWorld=1.8`, `taperAmount=0.25`, flush top, sorting Ground/-10.
- `Assets/Shaders/CliffVoidFade.shader` + `Assets/Materials/CliffVoidFade_Mesh.mat`. Material values tuned dark: `_TopTint=(0.24,0.26,0.30)`, `_MidTint=(0.12,0.13,0.18)`, `_BottomVoidColor=(0.11,0.02,0.19)`, `_DarkenStrength=0.85`, `_DesaturateStrength=0.55`, `_TopOpaqueBand=0.30`, `_AlphaFadeStart=0.55`, `_RimShadowStrength=0.5`, `_NoiseStrength=0.35` (but no NoiseTex → no visible noise).
- UVs: u = cumulative perimeter distance / tileWorldLength, v = 1 top → 0 bottom.

## Changes to implement

### A. Shader `CliffVoidFade.shader` — add PROCEDURAL rock detail (no texture needed)
All driven by UV + a cheap procedural hash/value-noise (e.g. `frac(sin(dot(...)) * ...)` value noise; 2-3 octaves max). Add material properties (with sane defaults) so it stays tunable:
1. **Horizontal STRATA bands:** darken in repeating bands along `v`, with IRREGULAR spacing/thickness jittered by noise (so they read as natural rock layers, not a ruler). Props: `_StrataCount` (~6-9), `_StrataStrength` (~0.35), `_StrataJitter` (~0.4).
2. **Surface mottle noise:** multiply albedo by value-noise (uv * scale) so the face isn't flat. Props: `_RockNoiseScale`, `_RockNoiseStrength` (~0.25). Reuse/repurpose `_NoiseStrength` if cleaner.
3. **Top-lip light band:** a thin BRIGHT cool highlight where `v` is near 1 (the floor lip catching ambient light), fading down fast. Props: `_LipLightStrength` (~0.5), `_LipLightBand` (~0.08), `_LipLightColor` (cool near-white). This gives the crucial floor↔cliff separation.
4. **Contact shadow (AO):** a dark band just BELOW the lip light (occlusion under the overhang). Props: `_AOStrength` (~0.4), `_AOBand` (~0.12).
5. Keep the existing color-temp gradient (top granite → mid cool → bottom `_BottomVoidColor`) + noisy lower alpha dissolve + purple rim. Ensure the gradient uses a COLOR-TEMPERATURE shift (warmer/neutral top → cool desaturated mid → purple void), not a flat black lerp.
- URP 2D, `ZWrite Off`, transparent. Backward-compatible defaults so the old tilemap material still renders.

### B. `CliffMeshGenerator.cs` — irregular NATURAL bottom edge (teeth)
- Replace the uniform smooth bottom with an IRREGULAR bottom: vary each bottom vertex's hang depth (`cliffHeightWorld * (1 ± jitter)`) and lateral offset, CLUSTERED into groups so the bottom breaks into tapering teeth/spikes of varying length (some short, some long), NOT a uniform comb and NOT a straight line.
- Determinism: derive jitter from a hash of the vertex's loop-position/index (stable across regenerate — NOT `Random`), so re-running gives the same shape.
- New serialized params: `bottomJitter` (~0.35), `teethClustering` (~0.5), keep `taperAmount`.
- Do NOT change the TOP edge (must stay flush to the floor lip). Keep UV.v=0 at the (now varying) bottom so the fade still works.
- Regenerate the mesh in `_IsoGame` after the change.

## Constraints
- UnityMCP `execute_code` action:execute, NO `using` directives (fully-qualified). After edits, `read_console` → 0 compile errors before regenerating.
- _IsoGame only. Do NOT enter play mode. Do NOT commit. Do NOT touch Map02/03.
- Save the scene after regenerating.

## Report (CODEX_DONE.md)
- Shader props added (names + defaults), mesh changes, new vertex/tri count, 0-compile-error confirmation. The orchestrator will screenshot + tune. BLOCKED + reason if shader won't compile.
