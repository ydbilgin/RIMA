# CX TASK — Q4 cliff: propagate the locked _IsoGame cliff setup to Map02 + Map03

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: code / STAGING / memory.

## Amaç (purpose)
`_IsoGame` now has the final working cliff: `CliffMeshGenerator` (textured mesh underlay, bright unlit, cliffHeightWorld=2.35) + `CliffOverlayDecorator` (92 ref_kit_b spire sprites, direction-correct) on `CliffRing`, old `CliffTilemap` renderer disabled, `CliffOverlays` container ACTIVE. Apply the SAME setup to `Assets/Scenes/_IsoGame_Map02.unity` and `_IsoGame_Map03.unity` so all 3 gameplay maps have the consistent cliff. Unity `RIMA@ed023e0b` live.

## Per-map procedure (Map02, then Map03)
1. Load the scene (single). Find `CliffRing` (it has the OLD `CliffAutoPlacer` + `CliffTilemap`).
2. Add `CliffMeshGenerator` + `CliffOverlayDecorator` to `CliffRing` (or copy the exact component values from `_IsoGame`'s CliffRing). Wire:
   - CliffMeshGenerator: Ground tilemap ref, `CliffVoidFade_Mesh.mat` material (with `_MainTex` = `Assets/Sprites/Environment/CliffMesh/cliff_face_strip.png`), `cliffHeightWorld=2.35`, `tileWorldLength=3.84`, the same serialized defaults as _IsoGame.
   - CliffOverlayDecorator: assign the 9 `Assets/Sprites/Environment/CliffKit_RefB/` sprites (cliffS..cliffNW + cliffCyanGlow), same defaults (spacing 0.70, clusterChance 0.42, cyanChance 0.08, scaleMin 0.90, scaleMax 1.35, jitter 0.14).
3. Disable the old `CliffTilemap` TilemapRenderer (keep as fallback, don't delete). Disable/remove the old `CliffAutoPlacer` generation if it conflicts.
4. Generate the mesh + overlays. **CRITICAL GOTCHA:** ensure the `CliffOverlays` container ends up **ACTIVE** (SetActive true) — in _IsoGame it was accidentally left inactive and the spires didn't render. Verify `CliffOverlays.activeInHierarchy == true` and its 92-ish children have enabled SpriteRenderers with `cliff_*` sprites.
5. Save the scene.

## Verify (DO before reporting)
- For EACH map: `manage_camera` screenshot (Main Camera at pos (1.92,-0.5,-10) ortho 4.5), confirm the spire cliff renders (like `Assets/Screenshots/Q4_hybrid_overlays_active_v1.png`). Save to `Assets/Screenshots/Q4_map02_cliff.png` / `Q4_map03_cliff.png`.
- Count `cliff_*` SpriteRenderers visible (should be >0, ~90). `read_console` → 0 errors. No play mode. No commit.
- NOTE: each map's floor diamond may differ → the generator regenerates from each Ground; that's expected.

## Report (CODEX_DONE.md)
Per map: components added, overlays count + ACTIVE confirm, screenshot path, 0-error confirm. BLOCKED + reason if a map's CliffRing/Ground is missing.
