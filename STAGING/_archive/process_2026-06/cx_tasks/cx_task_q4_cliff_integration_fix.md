# CX TASK — Q4 cliff: make the hybrid render CLEAR + NATURAL + STABLE (own the integration)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: code / STAGING / memory.

## Amaç (purpose)
User feedback: the cliff is "too dark, can't make anything out, make it nicer." You built the hybrid components; make them render CLEARLY, look NATURAL, and be STABLE — end to end, with GOOD DEFAULTS, so no manual param-poking is needed. Unity `RIMA@ed023e0b` live, scene `_IsoGame`. Verify with screenshots yourself before reporting.

## Current broken state (orchestrator diagnosed)
1. **Mesh renders near-BLACK** even unlit + `_TopTint`/`_MidTint`=white + `_DarkenStrength`=0. ROOT (in `Assets/Shaders/CliffVoidFade.shader` Frag, ~lines 155-191): the color is crushed by (a) `tint` lerp toward `_BottomVoidColor` for `depth>0.45` (line 158-160) — darkens the LOWER HALF; (b) `oldFade` blend toward `_VoidColor` using `_FadeStart`/`_FadeEnd` (line 188-189); (c) `alphaFade` toward `_BottomVoidColor` + alpha cut (line 185-191). The hand-painted texture `_MainTex` (`Assets/Sprites/Environment/CliffMesh/cliff_face_strip.png`, 512x256, IS assigned) gets buried. The sprite overlays render bright (unlit Sprites/Default) — the mesh should read at a SIMILAR brightness.
2. **Overlays VANISH on mesh regen.** `CliffMeshGenerator.Regenerate()` clears `CliffRing`'s children (including the `CliffOverlays` container created by `CliffOverlayDecorator`). Calling `CliffOverlayDecorator.RegenerateOverlays()` afterward does NOT recreate them (it produced 0 children / no container). The two systems are coupled badly.

## Fix — make it render clear, natural, stable
### A. Shader/material brightness (`CliffVoidFade.shader` + `CliffVoidFade_Mesh.mat`)
- Rebalance the Frag so the textured rock is CLEARLY VISIBLE (mid-bright slate, reads like the sprite overlays), with ONLY the bottom ~20-25% darkening + dissolving into purple void. The TOP ~75% should show the `_MainTex` rock with its fractures at near-full brightness.
- Concretely: make the `_BottomVoidColor` tint blend start much lower (only the bottom band, not depth>0.45), neutralize the `oldFade`→`_VoidColor` path (or set its material params so it's off), keep `_LipLightStrength` for the top lip, keep a gentle AO under the lip, and keep the bottom alpha-dissolve only in the lowest band. Set sensible DEFAULTS in the material so it looks right without external tuning.
- Keep it UNLIT (already converted), transparent, ZWrite off, URP 2D. Keep backward-compat for the old tilemap material.
- The `_MainTex` strip's lower rows are dark by design — consider sampling so the cliff shows more of the texture's upper/mid (brighter) content, or just ensure the shader doesn't compound the darkness.

### B. Overlay stability (`CliffOverlayDecorator.cs` + maybe `CliffMeshGenerator.cs`)
- Make the overlays SURVIVE / auto-rebuild after a mesh regen. Either: (i) have `CliffMeshGenerator.Regenerate()` call the overlay decorator's regen at the end, or (ii) make `CliffOverlayDecorator` not depend on being a CliffRing child that gets cleared (own container outside the cleared set), and have its `RegenerateOverlays()` rebuild the boundary from the mesh/floor independently. Result: after ANY regen, the overlays are present.
- Fix `RegenerateOverlays()` so it ACTUALLY spawns (it currently yields 0). Good default density: sprites every ~0.7 world units, clustered, cyan rare. They must be VISIBLE hanging spires breaking the bottom silhouette.

### C. Good defaults + final look
- Target: a CLEAR, NATURAL floating-island cliff = visible textured slate rock wall (mesh) + natural hanging spire overlays (CliffKit_RefB) breaking the bottom silhouette, dissolving into purple void at the very bottom. NOT dark/muddy. Reads clearly.
- Tune `cliffHeightWorld` if 3.0 is too tall/dark (try 2.0-2.5).

## Verify (DO THIS before reporting)
- Use `manage_camera` screenshot (camera "Main Camera", set it to e.g. pos (1.92,-0.5,-10) ortho 4.5 to frame the south cliff edge) and LOOK at the result. Iterate your own params until the cliff is clearly visible (textured rock + spires, not near-black). Save 1-2 screenshots to `Assets/Screenshots/Q4_cx_hybrid_*.png`.
- Then SAVE the scene. `read_console` → 0 errors. No play mode. No commit.

## Report (CODEX_DONE.md)
Shader/material changes, overlay-stability fix, final default params, overlay count spawned, and the screenshot path(s) you verified. BLOCKED + reason if you cannot get it to render clearly.
