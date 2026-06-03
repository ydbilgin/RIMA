# ROOMTOOL IMPROVEMENT PLAN — Townscaper-but-2D (S6)

**Owner of doc:** tech-art/tools lead (Opus 4.8). **Status:** PLAN (no code written yet).
**Merges:** `ROOMTOOL_UX_SPEC_S6.md` (feel/UX) + `ROOMTOOL_FUNC_SPEC_S6.md` (data/engine) + `ROOMTOOL_AUDIT_S6.md` (ground truth of what already exists).
**Vision (user, translated):** "like Townscaper but 2D, simpler — I select and place a piece, and pieces connect to each other."
**Locked:** ONE shared `RoomData` across BOTH surfaces — (a) in-game F2 overlay, (b) Unity Editor window. Connect = BOTH click-single Wang auto-connect AND press-hold-drag connected run. Placeholders OK; the TOOL is the deliverable.

All file/line claims below are traced to code I actually read: `RoomData.cs`, `RoomPlacementTypes.cs`, `WallRunBuilder.cs` (`RIMA.DevTools`, runtime asmdef, `#if UNITY_EDITOR || DEVELOPMENT_BUILD`), `RoomDataPlacementSink.cs` (Editor), `InPlayMapPaintOverlay.cs`, `RoomDataComposer.cs`, `RIMA.Runtime.asmdef` (no `UnityEditor` ref — confirms runtime can host the resolver).

---

## CONTRADICTIONS RESOLVED (between the three specs)

1. **"Editor does NOT auto-connect on click" (UX/Func framing) vs "Editor already writes RoomData" (Audit).**
   Both are true and not in conflict once separated. The Audit (verified) shows the Editor surface ALREADY persists to RoomData via `RoomDataPlacementSink` (`PlaceCell`/`EraseCell` → `floorCells`/`cliffCells`/`propPlacements`/`wallSegments`, `Undo.RecordObject`, `MarkDirty`, `Compose`). So we **do NOT rebuild the Editor write path** — we EXTEND the existing sink with Wang re-orientation. The UX spec's "PaintCell drops the exact sprite" critique is about *the legacy `RoomPainterScenePlacer` fallback path* (used only when no room is open) and about the *visual variant* not being neighbor-resolved — NOT about RoomData persistence. Plan reflects this: Wang is a new resolve step layered onto the working sink, not a new placement system.

2. **"Overlay shares RoomData" — UX ⭐7 says route overlay through "same data sink"; Func §3 says the sink is Editor-only (`AssetDatabase`) so the overlay needs a runtime bridge.** Func spec is correct and wins: `RoomDataPlacementSink` lives in `RIMA.RoomPainter.Editor` and uses `AssetDatabase`/`Undo`, which cannot compile in a `DEVELOPMENT_BUILD`. Resolution: extract the *pure mutation logic* (add/remove cell records — no `AssetDatabase`/`Undo`) into a runtime-safe `RoomDataMutator` in `RIMA.Runtime`. Both the Editor sink AND the overlay call it. Editor wraps it with `Undo`; overlay wraps it with its in-memory stacks. This is the merge of UX "same data sink" intent + Func "runtime-safe" constraint.

3. **Per-cell occupancy: Func §4 adds a new `WallCell` list; current model stores `wallSegments` (runs).** No conflict — Func is additive: keep `wallSegments` for back-compat, add `wallCells`, migrate runs→cells once. Plan adopts this. (Note: `RoomData.placements`/`PlacementRecord` is dead per Audit (e) — clean it in STEP 0 before extending the model, so we don't carry a third orphan list.)

4. **Ghost: UX wants valid/invalid + connection preview; Func defines the resolver that powers it.** No conflict — the UX ghost is the *consumer* of the Func `WangResolver`. Plan orders the resolver (engine) before the honest ghost (presentation) so the ghost can call `Resolve4` to show the resolved variant. The current ghost is `RoomDataPlacementSink.DrawAuthoringGhost` (single tinted sprite + wirecube, fixed color) — extend it, the overlay has none (build it).

5. **Sprite generation: both UX (⭐1 ConnectorSet) and Func (§5) need T/Cross/End art that DOES NOT EXIST.** Resolved as the explicit final follow-up STEP; the resolver returns shape+rotation and ships with placeholder grey-box variants so all logic is testable before PixelLab art lands.

**Net design after merge:** ONE per-cell occupancy grid (`wallCells`) → ONE pure `WangResolver` (runtime) → ONE pure `RoomDataMutator` (runtime) consumed by BOTH the Editor sink and the F2 overlay → shared ghost/MRU draw helpers. Editor `.asset` is canonical; runtime reads/writes a JSON sidecar mirror.

---

## ORDERED STEP LIST

Legend — **Surface:** `[SHARED]` runtime asmdef, both surfaces · `[EDITOR]` Unity window · `[IN-GAME]` F2 overlay.
**Owner:** `cx-code` (mechanical impl, Codex/Sonnet) · `Opus` (decision/multi-system write or review) · `PixelLab-gen` (art).
`∥` = parallelizable with the step(s) noted.

---

### PHASE 0 — Cleanup + data model (prereq for everything)

**STEP 0 — Remove dead model + fold the legacy SceneView placement path** `[EDITOR]` — owner: cx-code
- Action: delete `RoomData.placements` + `PlacementRecord` (Audit (e): no reader/writer). Fold/disable the legacy `RoomPainterScenePlacer` instantiate-only path in `RimaRoomPainterWindow.OnSceneGui` so ALL placement goes through `RoomDataPlacementSink` (Audit gap 3 — "paint into the void" footgun). Fix the `active ? "Open" : "Open"` leftover and the `Cliff_*` hardcoded name (minor, same file touch).
- Inputs: `RoomData.cs`, `RimaRoomPainterWindow.cs`, `RoomPainterScenePlacer.cs`.
- Acceptance: project compiles; opening the window and painting with NO room open either is blocked or routes to the sink (never instantiates non-persisting `Cliff_*`/`Parallax_*`); no references to `placements`/`PlacementRecord` remain.

**STEP 1 — Per-cell wall occupancy model + variant slots** `[SHARED]` — owner: cx-code
- Action: in `RoomData.cs` add `[Serializable] struct WallCell { Vector3Int cell; SegmentKind kind; WangShape shape; float rotation; string pieceId; float height; }` + `List<WallCell> wallCells` + null-guard in `EnsureLists()`. In `RoomPlacementTypes.cs` extend `WallPiece` with `straightSprite, cornerSprite (exists), tSprite, crossSprite, endSprite, singleSprite` + `string pieceId`. Keep `wallSegments` for migration.
- Inputs: `RoomData.cs`, `RoomPlacementTypes.cs`.
- Acceptance: compiles; existing rooms still deserialize (additive fields); `wallCells` defaults to empty list, no NRE on `EnsureDefaults`.

---

### PHASE 1 — Wang auto-connect CORE (the "connect" engine) — UX ⭐1, Func §1

**STEP 2 — `WangResolver` (pure, runtime-safe)** `[SHARED]` — owner: Opus (multi-system core; cx-code reviews)
- Action: new `Assets/Scripts/RoomPainter/WangResolver.cs`, namespace `RIMA.RoomPainter`, NO `UnityEditor` / no Unity-asset refs. `enum WangShape {Single,End,Straight,Corner,T,Cross}`; `struct WangResult {shape, rotationDegrees, neighborMask}`; `Resolve4(Vector3Int cell, Func<Vector3Int,bool> isOccupied)` using the Func §1 bitmask table (N=1,E=2,S=4,W=8; popcount→family; rotation aligns canonical). `EdgeMask8(...)` stub reserved for floor edges (Phase 2). Occupancy via delegate so Editor (RoomData lists) and runtime (in-memory RoomData) share one resolver.
- Inputs: Func §1 bitmask/rotation table.
- Acceptance: pure unit test (EditMode) — every 16 masks maps to the table's shape+rotation; runs with no Unity asset; lives in `RIMA.Runtime` (confirmed `RIMA.Runtime.asmdef` has no `UnityEditor` ref).
- `∥` with STEP 4 (mutator) — independent files.

**STEP 3 — `WangRebuild.ReorientWallCells(room, dirtyCells)`** `[SHARED]` — owner: cx-code (Opus reviews)
- Action: new `Assets/Scripts/RoomPainter/WangRebuild.cs` (runtime). For each dirty cell ∪ its 4 neighbors, build the occupancy delegate from `room.wallCells` (use a non-serialized `Dictionary<Vector3Int,int>` index per Func §4 for O(1)), call `WangResolver.Resolve4`, write resolved `shape`+`rotation` back into the matching `WallCell`. Recompute the dirty 3×3 only (perf).
- Inputs: STEP 1 model, STEP 2 resolver.
- Acceptance: placing a cell adjacent to a straight run flips the existing neighbor's `shape` Straight→T in `wallCells` (assertable without rendering).
- Depends on STEP 2.

**STEP 4 — Runtime-safe `RoomDataMutator` (the shared write path)** `[SHARED]` — owner: Opus (resolves contradiction 2; cx-code reviews)
- Action: new `Assets/Scripts/RoomPainter/RoomDataMutator.cs` (runtime, no `AssetDatabase`/`Undo`). Pure add/remove on `floorCells`/`cliffCells`/`propPlacements`/`wallCells`. Add `AppendWallRun(room, from, to, pieceId, footprint)` — rasterize the SAME Bresenham `GridLine` already in `WallRunBuilder`/sink, step by footprint.x, write `wallCells` (dedup vs existing), then call `WangRebuild.ReorientWallCells(room, newCells)`. Single click = 1-cell run through this path. Migration helper `MigrateSegmentsToCells(room)` expands legacy `wallSegments`.
- Inputs: STEP 1, STEP 3; existing `GridLine` in `WallRunBuilder.cs`/`RoomDataPlacementSink.cs`.
- Acceptance: calling `AppendWallRun` then composing draws an L-corner where the run bends and a T where two runs meet — with ZERO `UnityEditor` symbols referenced (compiles under `DEVELOPMENT_BUILD`).
- `∥` with STEP 2 once STEP 1 lands (different files; STEP 4 finalizes after STEP 3).

**STEP 5 — Composer + `WallRunBuilder` draw per-cell shape** `[SHARED]/[EDITOR]` — owner: cx-code
- Action: in `WallRunBuilder` (runtime, used by both surfaces) pick sprite by `WallCell.shape` mapped onto the `WallPiece` variant slots (STEP 1) + apply `Quaternion.Euler(0,0,rotation)`, replacing the current "incoming≠outgoing → `cornerSprite`" heuristic. In `RoomDataComposer.cs` add a `ComposeWallCells(room.wallCells, grid, wallParent)` pass; run the old `ComposeWallSegments` only for unmigrated rooms (call `MigrateSegmentsToCells` on open).
- Inputs: STEP 1, STEP 4; `RoomDataComposer.cs`, `WallRunBuilder.cs`.
- Acceptance: re-composing a room with mixed shapes renders straight/corner/T/cross/end at correct rotation; legacy run-only rooms still render after migration.
- Depends on STEP 4.

**STEP 6 — Editor sink: route wall placement through Wang** `[EDITOR]` — owner: cx-code (Opus reviews)
- Action: in `RoomDataPlacementSink.cs`, on wall-layer place/erase call `RoomDataMutator.AppendWallRun` / a remove-cell + `ReorientWallCells` on the ActiveRoom (wrapped in the existing `Undo.RecordObject` + `MarkDirty` + `Compose`). Single click = 1-cell run = neighbor re-eval. Keep `Alt = place exact selected variant` escape hatch (UX ⭐1 power-user). This makes the EDITOR surface fully Wang on the single canonical `.asset` — verify here BEFORE the runtime bridge.
- Inputs: STEPS 4–5; `RoomDataPlacementSink.cs`.
- Acceptance: in the Editor window, clicking two separate wall cells that become adjacent auto-joins them (corner/T) without a drag; Undo collapses the whole stroke; `Alt`-click places the raw selected sprite unresolved.
- Depends on STEP 5. **End-of-Phase-1 milestone = Editor surface feels like Townscaper on one `.asset`.**

---

### PHASE 2 — Click-place + honest GHOST UX — UX ⭐2, ⭐3, ⭐4

**STEP 7 — Shared ghost/draw helpers (runtime-safe core, Editor/overlay wrappers)** `[SHARED]` — owner: Opus (UX-critical; cx-code reviews)
- Action: new `Assets/Scripts/RoomPainter/RoomToolGhostModel.cs` (runtime, pure): given hovered cell + selected family + RoomData, return the RESOLVED variant (calls `WangResolver`), a validity verdict (occupied-same-layer / off-floor-footprint / out-of-bounds), and the list of neighbor cells whose `shape` WOULD change. No drawing here — just the model. Editor draws it with `Handles` (extend `DrawAuthoringGhost`), overlay draws it with IMGUI (`GUI.DrawTextureWithTexCoords`).
- Inputs: STEP 2 resolver; `RoomData` footprint/bounds.
- Acceptance: model returns the same resolved shape + validity that an actual place would produce (parity test against `AppendWallRun`).
- Depends on STEP 2.

**STEP 8 — Editor honest ghost** `[EDITOR]` — owner: cx-code
- Action: rewrite `RoomDataPlacementSink.DrawAuthoringGhost` to use STEP 7 model: render the resolved connecting variant under the cursor; valid green-cyan `(0.4,0.9,1,0.55)` / invalid warm-red `(1,0.35,0.3,0.55)` (replace the fixed wall-orange/cyan); draw the real `WallPiece.footprint` rect (not the fixed `1×1` wirecube); during a drag render the whole projected Bresenham run as faint pre-resolved ghosts + a `Handles.Label` "x7" count at the cursor; pulse/outline neighbor cells that will re-shape (UX ⭐3).
- Inputs: STEP 7; `RoomDataPlacementSink.cs` (`DrawAuthoringGhost`, `DrawWallStroke`).
- Acceptance: hovering a wall next to an existing run shows the to-be-T ghost AND a pulse on the neighbor; invalid cells tint red; drag shows the full run + count before mouse-up.
- Depends on STEP 7. `∥` with STEP 9 (different surfaces).

**STEP 9 — Place/erase/undo feel + hint-strip (Editor)** `[EDITOR]` — owner: cx-code
- Action: brief white-flash + scale-pop on placed piece (editor-time); erase-mode hover highlights the target piece in red BEFORE click; one-line hint strip in the status bar mirroring the overlay wording ("click=place+connect · hold-drag=run · RMB=erase · Shift=straight · Ctrl+Z=undo"); status toast "Undid stroke (N cells)".
- Inputs: `RimaRoomPainterWindow.cs` status bar; `RoomDataPlacementSink.cs`.
- Acceptance: place shows a pop; erase hover shows red target; hint strip text === overlay text (verify same string constant).
- `∥` with STEP 8.

---

### PHASE 3 — Shared RoomData across BOTH surfaces (drag-array persist + parity) — UX ⭐7, Func §2/§3

**STEP 10 — JSON sidecar bridge** `[SHARED]` — owner: Opus (biggest-risk per Func; cx-code reviews)
- Action: new `Assets/Scripts/RoomPainter/RoomDataJson.cs` + `RoomDataPaths.cs` (runtime). `[Serializable] RoomDataDTO` mirrors cell lists only (asset refs as `assetGuidOrName` strings — no `UnityEngine.Object`); `JsonUtility` read/write. `RoomDataPaths.JsonFor(roomId)` → `Assets/Data/Rooms/<roomId>.room.json` (Editor) / `Application.streamingAssetsPath/Rooms/...` (player). In `RoomDataAuthoringController.SaveActiveRoom`, after `SaveAssets`, ALSO write the JSON mirror (one line). `.asset` stays canonical; JSON is derived.
- Inputs: `RoomData.cs`, `RoomDataAuthoringController.cs`.
- Acceptance: Editor save produces `<roomId>.room.json` whose round-trip back into a fresh `RoomData` equals the asset's cell lists.
- Depends on STEP 1. `∥` with STEP 8/9.

**STEP 11 — Overlay reads/writes RoomData via mutator + bridge (kills the silent loss bug)** `[IN-GAME]` — owner: Opus (multi-system; cx-code reviews)
- Action: in `InPlayMapPaintOverlay.cs`, on `RoomLoader.OnRoomLoaded` resolve room id, load `<roomId>.room.json` into an in-memory `RoomData` (`ScriptableObject.CreateInstance` is runtime-legal). Replace the `[DragPlace_Props]` loose-instantiation wall path with `RoomDataMutator.AppendWallRun` on that in-memory RoomData, then re-compose the wall group via a runtime-safe sprite-only subset of the composer. Drag = connected run that PERSISTS + auto-connects (Conor-Dart, UX/Func). A "Save" button writes JSON back; under `#if UNITY_EDITOR` also `EditorUtility.CopySerialized` into the `.asset` so opening the Editor shows the same map (Func §3 mitigation). Fix the F2 erase-undo wrong-tilemap bug (`FindTilemapForEdit` ignoring the cell — Audit (d)).
- Inputs: STEPS 4, 5, 10; `InPlayMapPaintOverlay.cs`, `WallRunBuilder.cs`.
- Acceptance: place walls in F2 → Save → stop Play → open Editor window → SAME walls present (round-trip). Drag in F2 produces a connected, persisted run. No `[DragPlace_Props]` orphans left on stop.
- Depends on STEPS 5, 10. **End-of-Phase-3 milestone = both surfaces share ONE RoomData, drag persists + connects.**

**STEP 12 — Floor/Cliff in overlay mirror into RoomData (kill remaining live-only loss)** `[IN-GAME]` — owner: cx-code
- Action: overlay Floor/Cliff `PaintCell` ALSO writes `floorCells`/`cliffCells` via `RoomDataMutator` (currently Tilemap-only, lost on stop — Audit (b)/(c)).
- Inputs: STEP 4, 11; `InPlayMapPaintOverlay.cs`.
- Acceptance: floor painted in F2 survives stop+Editor-open.
- Depends on STEP 11. `∥` with STEP 13/14.

---

### PHASE 4 — Palette / browser UX polish — UX ⭐5, ⭐6

**STEP 13 — Collapse to 3 buckets + recently-used + search** `[EDITOR]/[IN-GAME]` — owner: cx-code
- Action: front-and-center palette = **Floor · Wall · Prop** (Cliff folds under Wall sub-family; Decor/Object/Parallax + the 10-bit `_layerFilterMask` move under an "Advanced ▸" twirl-down, collapsed). Add an MRU row (last 6 placed `guid`s) at top of BOTH palettes. Add a name-substring search box (Editor toolbar + overlay header) filtering `_assetCache`/`_palette`. Overlay: render real thumbnails for prefabs/RuleTiles via registry `e.sprite`; never an empty button (Audit (b) `TileSprite` null fallback).
- Inputs: `RimaRoomPainterWindow.cs` palette/filter section; `InPlayMapPaintOverlay.cs` palette.
- Acceptance: default view shows exactly 3 buckets + MRU + search; Advanced is collapsed by default; overlay palette shows a thumbnail or labeled box for every entry (no blanks).
- `∥` with STEP 14.

**STEP 14 — Thumbnail map browser (Editor) + map row (overlay)** `[EDITOR]/[IN-GAME]` — owner: cx-code
- Action: Editor — convert `DrawRoomLibraryPanel` list → 2-up thumbnail card grid using existing `RoomThumbnailBaker` images, name + dirty dot, Open on click, hover reveals Dup/Delete; keep New/Dup/Delete/Save/Playtest (green, one click). Overlay — add a top map row: current name + `*` dirty + ◀▶ cycle + New + Save + Playtest(resume) (depends on STEP 11 write path).
- Inputs: `RimaRoomPainterWindow.cs` (`DrawRoomLibraryPanel`/`DrawRoomLibraryRow`), `RoomThumbnailBaker.cs`; `InPlayMapPaintOverlay.cs`.
- Acceptance: Editor browser shows baked thumbnails as cards; overlay can pick/new/save/playtest a map in-game without leaving Play.
- Depends on STEP 11 (overlay half). Editor half `∥` with STEP 13.

---

### PHASE 5 — Connecting-piece sprite gen (FOLLOW-UP, flagged) — UX ⭐1 art, Func §5

**STEP 15 — Generate Wang variant sprites** `[art]` — owner: PixelLab-gen (Opus specs prompts)
- Action: per wall theme generate `wall_<theme>_{straight,corner,t,cross,end,single}` at PPU64, pivot at base (Custom-Axis Y-sort), transparent. `straight`+`corner` reuse existing Ruined-Keep kit; `T`/`Cross`/`End` are NEW (do not exist — Func §5). Verify corner reads correctly at all 4 rotations on a top-down 3/4 wall; if lighting mirrors wrong, author 4 distinct corners instead of rotating one. Floor 8-neighbor edge set (edge strip + inner/outer corner trims) is a SECOND lighter set, Phase-2-of-art. Log every output as placeholder in `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md` (project rule); characters stay PixelLab-only, never imagegen.
- Inputs: STEP 1 variant slots; existing Ruined-Keep wall kit.
- Acceptance: each `WallPiece` variant slot filled; placing/dragging shows art (not grey boxes) that visually joins; registry updated.
- **FOLLOW-UP — NOT blocking:** all of PHASES 0–4 ship and are testable with placeholder grey-box variants because the resolver returns shape+rotation independent of art. This step only swaps art in.

---

## PARALLELIZATION SUMMARY
- STEP 2 (resolver) ∥ STEP 4 (mutator) once STEP 1 lands.
- STEP 8 (Editor ghost) ∥ STEP 9 (Editor feel) ∥ STEP 10 (JSON bridge).
- STEP 12 ∥ STEP 13 ∥ STEP 14 (Editor browser half).
- STEP 15 (art) ∥ EVERYTHING — placeholders unblock all logic; swap art last.
- Critical path: 0 → 1 → 2 → 3 → 4 → 5 → 6 (Editor Wang milestone) → 10 → 11 (shared-RoomData milestone) → 14.

## MILESTONES
- **M1 (end Phase 1, STEP 6):** Editor surface = Townscaper on one `.asset` — click-place auto-connects, drag-runs auto-corner/T, all RoomData-backed.
- **M2 (end Phase 3, STEP 11):** BOTH surfaces share ONE RoomData; F2 edits persist + appear in Editor; drag-array persists and connects.
- **M3 (end Phase 4):** simple 3-bucket palette + MRU + search + thumbnail browser on both; in-game new/save/playtest.
- **M4 (Phase 5):** placeholder Wang variants replaced with on-theme art.

---

## DEFINITION OF DONE (matches user vision)
1. **In-game AND editor:** the SAME tool works in the F2 in-Play overlay and the Unity Editor window, sharing ONE `RoomData` (`.asset` canonical + JSON mirror). Place in one, see it in the other.
2. **Select → place → auto-connect, click AND drag:** the user picks ONE family swatch (Floor/Wall/Prop), single-click places a piece that auto-orients to its neighbors (Wang), and press-hold-drag lays a connected run that auto-corners/Ts — pieces visibly connect to each other without the user ever choosing a variant or rotation.
3. **Townscaper-simple:** default UI = 3 buckets + recently-used + search; an honest ghost (valid/invalid + shows the connection before commit) is the only documentation needed; everything Tiled-like (10-layer mask, parallax, decor/object split) is hidden under "Advanced ▸". One verb per input: click=place+connect, drag=run, RMB=erase, Shift=straight, Alt=exact-variant escape hatch.
4. **No silent loss:** nothing painted in F2 is discarded on Play-stop; the legacy non-persisting placement path is gone; dead `placements` model removed.

---
*Placeholders are fine per the task — the TOOL is the deliverable. Wang logic ships with grey-box variants; PixelLab art (STEP 15) swaps in without touching the resolve/mutate code.*

**File:** `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ROOMTOOL_IMPROVEMENT_PLAN_S6.md`
