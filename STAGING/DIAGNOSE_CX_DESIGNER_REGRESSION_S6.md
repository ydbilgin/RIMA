ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# DEEP DIAGNOSIS — Map Designer regression (ANALYSIS ONLY, NO EDITS)

**DO NOT edit any file. DO NOT connect to Unity MCP. DO NOT run the game.** Unity + the live scene are owned by the Opus orchestrator. Your entire job is to READ code + git history and write a precise diagnosis + fix-forward plan to `CODEX_DONE.md` under header `## CX DIAGNOSE - DESIGNER REGRESSION`. Opus applies fixes.

## Amaç (user verbatim, Turkish)
The user reports the recent "Unified Designer" rewrite **regressed working features**:
1. "şu an oyunumuz topdown gibi gözüküyor, böyle değildi, eskiden bozuldu bir şeyler" → the game now reads TOP-DOWN; it used to read ISOMETRIC; something broke.
2. "map designer'da boyama olmuyor" → painting in the Map Designer does not work.
3. "varyantları gruplamıştık duruma göre varyant boyama yapıyorduk, o özellikler de kalkmış" → we had GROUPED variants and did CONTEXT/STATE-AWARE variant painting; those features are GONE.

## CRITICAL FACT (Opus established)
Everything since commit `ca043d1c` is UNCOMMITTED (785 changed entries). So `ca043d1c` = the PRE-rewrite WORKING baseline. The old painter/variant files were modified, not deleted. The committed baseline is the safety net. The user chose "deep diagnosis first" — produce the diagnosis, do NOT revert or fix yet.

## What Opus already traced (verify + go deeper)
- `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`: SceneView paint, MouseDown/Drag → `_core.Paint(cell, center)`.
- `Assets/Scripts/RoomPainter/UnifiedDesignerCore.cs`: `Paint(cell, worldPos)` → `RoomDataMutator.PutCategory(ActiveRoom, Category, SelectedAssetId, cell, worldPos, Rotation, Scale)`. **It writes a single SelectedAssetId to RoomData. No variant/Wang selection.**
- The variant/Wang system still exists in code: `RoomPainter/WangResolver.cs`, `RoomPainter/FloorWangResolver.cs`, `RoomPainter/WangRebuild.cs`, `MapDesigner/Composition/WangContextResolver.cs`, `Systems/Map/WangTileResolver.cs`, `Systems/Map/CornerWangPainter.cs`, `Environment/DeterministicVariantTile.cs`, `Map/Data/MaterialVariantPoolSO.cs`, `MapDesigner/Brush/Data/AssetPoolSO.cs` (`PickVariant_BucketAware` test), `MapDesigner/Brush/Data/BrushAssetVariant.cs`.

## QUESTIONS — answer all with file:line evidence + git diff
1. **Painting broken — exact cause.** Trace the full paint path in the NEW unified designer. Does painting render anything to a SCENE tilemap immediately, or only mutate RoomData (.asset/JSON) with no visible scene result? Where/when does RoomData become visible tiles (a Compose step? a tilemap? play-mode only?)? Is the palette empty for the active category (recall: registry has NO `portal`/`light` tags; `floor`/`cliff`/`prop` exist)? Enumerate the concrete reasons a user clicking in the SceneView sees nothing. Use `git diff ca043d1c -- Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs Assets/Scripts/RoomPainter/` to see what changed.
2. **Variant grouping — what existed, how it was invoked, why it's gone.** In the committed baseline (`git show ca043d1c:<path>` / `git log -- <path>`), how did the OLD painter (likely `RimaRoomPainterWindow.cs` and/or the Brush window `MapDesignerBrushWindow.cs` + `AssetPoolSO`/`BrushPackSO`) select a VARIANT during painting? What is the "grouped variants / paint by situation/state" data model — variant buckets (size/role), Wang neighbor-context (`WangContextResolver`/`CornerWangPainter`), or deterministic per-cell variant (`DeterministicVariantTile`)? Show the old call path that did variant selection, and confirm the new `UnifiedDesignerCore.Paint` bypasses it.
3. **Iso look regression — confirm.** Confirm the floor read top-down because the demo floor tileset `PixelLabFloorFlat` (`ce6f15c7`, `tile_view_angle 90`) is top-down-rendered, and that an iso read needs iso-projected tiles (Opus has now placed `b340684f` iso granite) PLUS cliff depth (KitB + `DirectionalCliffTile_Hades` + `RoomCliffSolver`). Is any global setting (camera, transparency sort axis, sprite sort) ALSO changed in the uncommitted diff that could flatten the look? Check `git diff ca043d1c` for ProjectSettings / camera / sort changes.
4. **What exactly did the rewrite remove/replace?** Summarize the meaningful designer/painter diffs vs `ca043d1c` (not asset/meta noise). Identify the specific deletions that caused regressions 1-3.

## DELIVERABLE
A) Root cause of each of the 3 regressions, with file:line + git evidence.
B) A concrete **FIX-FORWARD plan**: exact files + methods to change so the UNIFIED designer's painting (a) renders to the scene immediately and (b) uses the existing variant/Wang grouping system again — WITHOUT losing the consolidation. List each edit as "file → method → change". Keep it minimal/surgical.
C) Risk notes on the 785 uncommitted changes (anything that would make fix-forward or a selective revert dangerous).

Output to CODEX_DONE.md under `## CX DIAGNOSE - DESIGNER REGRESSION`.
