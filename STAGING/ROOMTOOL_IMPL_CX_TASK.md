# CX CODE TASK — Implement Townscaper-2D map tool (Wang auto-connect + shared RoomData)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only files named in the plan steps (4) say BLOCKED / "done through STEP N" if you run long, do NOT half-do many steps.
NLM ACCESS: `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"` if needed.

## THE SPEC IS ALREADY WRITTEN — IMPLEMENT IT
Read and implement **`STAGING/ROOMTOOL_IMPROVEMENT_PLAN_S6.md`** (ordered, code-grounded, owners + acceptance per step). Supporting detail (read as needed):
- `STAGING/ROOMTOOL_FUNC_SPEC_S6.md` (WangResolver bitmask table, RoomDataMutator, JSON bridge — exact signatures)
- `STAGING/ROOMTOOL_AUDIT_S6.md` (what already exists — do NOT rebuild the working Editor write path)
- `STAGING/ROOMTOOL_UX_SPEC_S6.md` (the feel these enable)

## SCOPE THIS RUN: critical path to milestone M2 — STEPS 0 → 6, then 10 → 11
Implement IN ORDER (each compile-clean before the next):
- **STEP 0** cleanup: remove dead `RoomData.placements`/`PlacementRecord`; route all Editor placement through `RoomDataPlacementSink` (disable legacy `RoomPainterScenePlacer` instantiate-only path); fix `active ? "Open":"Open"` + hardcoded `Cliff_*` name.
- **STEP 1** per-cell model: `WallCell{cell,kind,shape,rotation,pieceId,height}` + `List<WallCell> wallCells` in `RoomData.cs`; extend `WallPiece` with variant sprite slots (straight/corner/t/cross/end/single) + `pieceId` in `RoomPlacementTypes.cs`. Additive (existing rooms still deserialize).
- **STEP 2** `Assets/Scripts/RoomPainter/WangResolver.cs` (runtime, NO UnityEditor): `enum WangShape{Single,End,Straight,Corner,T,Cross}`, `struct WangResult{shape,rotationDegrees,neighborMask}`, `Resolve4(Vector3Int cell, System.Func<Vector3Int,bool> isOccupied)` per FUNC_SPEC bitmask table (N=1,E=2,S=4,W=8; popcount→family; rotation aligns one canonical). + EditMode unit test asserting all 16 masks → correct shape+rotation.
- **STEP 3** `Assets/Scripts/RoomPainter/WangRebuild.cs` (runtime): `ReorientWallCells(RoomData, IEnumerable<Vector3Int> dirty)` — for each dirty∪4-neighbors, build occupancy from `wallCells` (Dictionary index), `Resolve4`, write shape+rotation back. Recompute dirty 3x3 only.
- **STEP 4** `Assets/Scripts/RoomPainter/RoomDataMutator.cs` (runtime, NO AssetDatabase/Undo): pure add/remove on floorCells/cliffCells/propPlacements/wallCells; `AppendWallRun(room,from,to,pieceId,footprint)` (reuse the existing Bresenham GridLine, step by footprint.x, dedup, then `WangRebuild.ReorientWallCells`); single click = 1-cell run; `MigrateSegmentsToCells(room)`. MUST compile with zero UnityEditor symbols.
- **STEP 5** composer/builder per-cell shape: `WallRunBuilder` picks sprite by `WallCell.shape`→WallPiece variant slot + `Quaternion.Euler(0,0,rotation)` (replace the incoming≠outgoing cornerSprite heuristic); `RoomDataComposer.ComposeWallCells(...)` pass; migrate legacy segments on open.
- **STEP 6** Editor sink Wang: in `RoomDataPlacementSink`, wall place/erase → `RoomDataMutator.AppendWallRun`/remove + `ReorientWallCells` on ActiveRoom inside the existing `Undo.RecordObject`+`MarkDirty`+`Compose`; `Alt`=place exact variant. **M1 milestone: Editor click-place auto-connects on one .asset.**
- **STEP 10** `RoomDataJson.cs` + `RoomDataPaths.cs` (runtime): `[Serializable] RoomDataDTO` (cell lists only, asset refs as `assetGuidOrName` strings), JsonUtility read/write; `JsonFor(roomId)`→`Assets/Data/Rooms/<roomId>.room.json`; `RoomDataAuthoringController.SaveActiveRoom` also writes the JSON mirror (.asset stays canonical).
- **STEP 11** overlay shares RoomData: in `InPlayMapPaintOverlay.cs`, on `RoomLoader.OnRoomLoaded` load `<roomId>.room.json` into an in-memory RoomData; replace the `[DragPlace_Props]` instantiate path with `RoomDataMutator.AppendWallRun` + runtime sprite-only re-compose; "Save" writes JSON + (`#if UNITY_EDITOR`) `EditorUtility.CopySerialized` into the .asset; fix the `FindTilemapForEdit` wrong-tilemap undo bug. **M2 milestone: both surfaces share ONE RoomData; F2 edits persist + appear in Editor; drag persists+connects.**

DEFER (do NOT do this run): STEP 7-9 (ghost UX), STEP 12-14 (palette/browser polish), STEP 15 (sprite art). Wang ships with GREY-BOX placeholder variants (resolver returns shape+rotation independent of art) — if a variant sprite slot is empty, fall back to the existing run sprite + rotation so it still renders + is testable.

## ACCEPTANCE (verify, report honestly)
- Each step's acceptance criterion in the plan. dotnet build + Unity recompile clean (`read_console` 0 errors). The WangResolver/Mutator/Rebuild/Json files MUST compile with no UnityEditor symbols (runtime asmdef). The overlay's UnityEditor calls stay `#if UNITY_EDITOR` guarded.
- Smoke: Editor — click two adjacent wall cells → auto-join (corner/T) without drag; F2 — place walls → Save → stop → open Editor → same walls.
- Report: files new/edited, the WangResolver+RoomDataMutator+RoomDataDTO signatures you produced, "done through STEP N", any BLOCKED/DEVIATION. Do NOT commit. Do NOT touch character art / combat / the F2 pause+thumbnail UI already built.
