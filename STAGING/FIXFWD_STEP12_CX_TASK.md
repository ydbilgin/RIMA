ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# FIX-FORWARD STEP 1+2 — make unified-designer painting VISIBLE (IMPLEMENT)

You may EDIT C# source files for this task. **DO NOT connect to Unity MCP, do NOT run the game, do NOT touch scenes/assets.** Opus owns the live Unity session and will compile + test your edits. This task implements steps B1 and B3 of your own diagnosis in `CODEX_DONE.md → ## CX DIAGNOSE - DESIGNER REGRESSION`.

## Amaç
After the unified-designer rewrite, painting writes RoomData but the SceneView shows nothing, because (1) no recompose after each stroke and (2) the composer cannot render TileBase floor/cliff entries. Fix BOTH so a paint stroke immediately shows tiles in the SceneView. Keep it surgical — do NOT touch variant/Wang yet (that is a later step).

## STEP 1 — Recompose + repaint after every paint/erase/generate
File: `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`
- `OnCoreChanged()` (around line 59-63): currently only `EditorUtility.SetDirty(_core.ActiveRoom); Repaint();`.
  Change to ALSO, when `_core.ActiveRoom != null`: mark the controller dirty, recompose the active room into the preview, and repaint the SceneView. Use the existing composer/controller already referenced by this window (`_composer`, `_controller`) and the same Compose call used by `BuildPreview()` (around 359-364) and `SaveActive()`. Net effect: `_composer.Compose(_core.ActiveRoom); SceneView.RepaintAll();`.
- Keep the explicit `Build Preview` button working as a manual recovery.
- Guard: if composing on every MouseDrag stutters, wrap the recompose in an `EditorApplication.delayCall` debounce (only one pending recompose at a time). Correctness first; only add debounce if obviously needed.
- If a paint happens with no active RoomData, do nothing (no crash) — verify `_core.ActiveRoom` null-safe everywhere in this path.

## STEP 2 — Composer renders TileBase entries onto a Tilemap (not only prefab/sprite)
File: `Assets/Editor/RoomPainter/Authoring/RoomDataComposer.cs`
- `ComposeTileCells(...)` (around 137-154) / `CreateVisual(...)` / `InstantiateAsset(...)` (around 238-267): today these only `LoadAssetAtPath<GameObject>` (prefab) or `<Sprite>`. Floor/cliff registry entries are frequently `TileBase` assets (`Assets/Sprites/Environment/PixelLabFloorFlat/Tiles/flat_tile_*.asset` and `Assets/Sprites/Environment/PixelLabFloorIso/Tiles/iso_floor_*.asset`). Those currently render nothing.
- Add a TileBase render path for Floor and Cliff layers:
  - Resolve the cell's asset GUID/name to `UnityEngine.Tilemaps.TileBase` FIRST (before prefab/sprite).
  - If it is a TileBase: ensure a Tilemap child exists under the preview root for that layer (e.g. a `Grid` with `Floor`/`Cliff` `Tilemap` children; reuse the Grid if the composer already builds one — check how `PreviewGrid` is structured). Use the project's iso grid convention: Grid `cellLayout = Isometric`, `cellSize = (0.94,0.94,1)` (matches the floor tiles). Call `tilemap.SetTile(cell, tile)`.
  - Floor tilemap sorting layer `Floor` order 0; Cliff tilemap sorting layer `Ground` order -10 (per `RoomDepthStack`). If `RoomDepthStack.SlotFor(...)` exists, use it.
  - Keep the existing SpriteRenderer/prefab path as fallback for sprite-only / prefab entries (props/portals/lights).
- Make sure re-composing CLEARS previous tilemap tiles (ClearAllTiles) so repeated compose does not stack/duplicate. Check how the composer currently clears prefab visuals and mirror that for the tilemap.

## Constraints
- Editor-only code; do not change runtime asmdef boundaries.
- Do not modify the variant/Wang systems, RoomDataJson, or the live reloader in this task (later steps).
- Do not rename public API used elsewhere; additive/surgical only.
- After editing, report: exact diffs (file → method → what changed), and a 1-line note on whether you added a debounce. Output to `CODEX_DONE.md` under `## CX FIXFWD STEP12`.
- Do NOT compile in Unity (Opus does that). If something is ambiguous (e.g. how PreviewGrid is structured), state the assumption you made and where Opus should verify.
