# CX TASK — Q4 cliff: make CliffVoidFade shader UNLIT so the painted texture shows

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: code / STAGING / memory.

## Amaç (purpose)
The hybrid cliff mesh now has a hand-painted rock texture (`_MainTex` = `Assets/Sprites/Environment/CliffMesh/cliff_face_strip.png`, 512x256). BUT the cliff renders near-BLACK regardless of material tints. ROOT CAUSE (orchestrator verified): `Assets/Shaders/CliffVoidFade.shader` is LIT (samples URP 2D `_MainLight`/2D lights). The cliff hangs DOWN into the void where there are NO 2D lights → the lit shader multiplies the albedo by ~0 → near-black, even with `_TopTint=white`, `_MidTint=white`, `_DarkenStrength=0`. The sprite overlays (CliffKit_RefB) ARE visible because they use an UNLIT sprite shader.

## Fix: make `CliffVoidFade.shader` UNLIT
Convert it to an UNLIT URP 2D shader so the painted `_MainTex` shows at its own brightness, independent of scene 2D lights (the cliff art carries its own shading; it must not depend on void lighting). KEEP all the existing procedural effects layered ON TOP of the unlit textured albedo:
- Sample `_MainTex` as the base albedo (tiled by existing UVs u=perimeter, v=depth).
- Keep: depth color-temp gradient (`_TopTint`/`_MidTint` multipliers — but as ART tints, not lighting), `_BottomVoidColor` blend at the bottom, `_AlphaFadeStart`/`_AlphaFadeEnd` lower alpha dissolve into void, `_TopOpaqueBand`, `_LipLightStrength`/`_LipLightColor` (a procedural bright top band), `_AOStrength`/`_AOBand` (dark band under lip), optional `_RimShadowStrength`. Keep `_StrataStrength` path but it is currently 0 (texture provides detail).
- REMOVE the 2D light sampling / `Universal2D` lit pass dependency. Use a plain unlit fragment: `col = tex2D(_MainTex, uv) * tints ... ; apply gradient/lip/AO/voidfade/alpha`. URP 2D compatible, transparent queue, `ZWrite Off`, `Blend SrcAlpha OneMinusSrcAlpha`.
- Result target: the cliff face should read as a clearly-visible dark-slate rock wall with vertical fractures (the texture), with a cool top lip, darkening + dissolving into purple void at the bottom — at roughly the same brightness as the sprite overlays, NOT near-black.

## Constraints
- Edit ONLY `Assets/Shaders/CliffVoidFade.shader` (and the material if a property changes). Do NOT change CliffMeshGenerator, the scene, or overlays.
- Keep backward-compat: the old tilemap material (`CliffVoidFade.mat`) must still render (it uses the same shader).
- UnityMCP: after editing the shader, `read_console` → 0 errors. Do NOT enter play mode. Do NOT commit.
- Unity `RIMA@ed023e0b` live. The mesh material is `CliffVoidFade_Mesh.mat` (current values: `_TopTint=white`, `_MidTint=white`, `_DarkenStrength=0`, `_AlphaFadeStart=0.15`, `_MainTex` assigned).

## Report (CODEX_DONE.md)
What changed in the shader (lit→unlit approach), confirm 0 compile errors, confirm the material still has its properties. The orchestrator will screenshot + tune. BLOCKED + reason if the shader structure can't be safely converted.
