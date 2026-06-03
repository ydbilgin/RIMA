# Room Tool Audit — S6 (ground-truth, code-grounded)

Auditor pass over the just-built map-authoring tool. Every claim below is traced to a
specific file + method I actually read. No design, no speculation.

Files read: `RimaRoomPainterWindow.cs`, `RoomPainterMode.cs`, `RoomPainterScenePlacer.cs`,
`Authoring/RoomDataComposer.cs`, `Authoring/RoomDataPlacementSink.cs` (critical — pulled in),
`Authoring/RoomDataAuthoringController.cs` (critical — pulled in), `Authoring/RoomThumbnailBaker.cs`,
`RoomData.cs`, `RoomLayer.cs`, `RoomPlacementTypes.cs`, `InPlayMapPaintOverlay.cs`,
`WallRunBuilder.cs`, `RuinedKeepComposer.cs`.

---

## (a) Editor Window — RimaRoomPainterWindow

WORKS (verified in code):
- **Browse / library**: `RoomDataAuthoringController.RefreshLibrary()` scans `t:RoomData` under
  `Assets/Data/Rooms`, builds `RoomLibraryEntry` rows with thumbnails. Drawn by `DrawRoomLibraryPanel` / `DrawRoomLibraryRow`.
- **New / Duplicate / Delete**: `CreateNewRoom()` (CreateAsset + SaveAssets), `DuplicateRoom()`
  (CopyAsset + new GUID), `DeleteRoom()` (confirm dialog + DeleteAsset). All real asset ops.
- **Open**: `OpenRoom()` + `OpenRoomInAuthoring()` → `_roomComposer.Compose(room)` rebuilds a scene
  preview under `[RoomPreview_Generated]` and `FocusTopDown()` flips SceneView to 2D ortho. **This is the top-down view.**
- **Save**: `SaveActiveRoom()` → `controller.SaveActiveRoom(composer)` → Compose + `RoomThumbnailBaker.Bake`
  (renders a 256px PNG to `Assets/Data/Rooms/Thumbnails/<roomId>.png`) + SetDirty + SaveAssets. Thumbnail bake is real and works.
- **Playtest**: `PlaytestActiveRoom()` saves, then `ComposeIntoPlaytestRoot` (separate `[RoomPlaytest_Generated]` root)
  then `EditorApplication.isPlaying = true`.
- **Place (SceneView)**: `OnSceneGui` → when `ActiveRoom != null` it routes to
  `RoomDataPlacementSink.OnSceneGUI(...)`. LMB paints, RMB erases, drag continues, Shift axis-locks,
  R rotates 90°. Ghost preview drawn (`DrawAuthoringGhost`).
- **Palette / filters**: asset grid from `RoomPainterAssetScanner.Scan(folderPath)`; 4 authoring tabs
  (Floor/Cliff/Wall/Prop → `_targetLayer`); layer bitmask sub-filter + category chips; 4 paint MODES
  (Tile/Cliff/Decor/Object, hotkeys 1-4) surfaced in toolbar + menu + statusbar.
- **Compose**: `RoomDataComposer.Compose` rebuilds floor/cliff (`ComposeTileCells`), walls
  (`ComposeWallSegments` → `WallRunBuilder`), props (`ComposeProps`) from RoomData lists.

NOTE — the placement sink is the live authoring path **only while an ActiveRoom exists**. If no room
is open, `OnSceneGui` falls through to the legacy `RoomPainterScenePlacer` (instantiate-only, names
objects `Cliff_*` / `Parallax_*`, does NOT touch RoomData). So there are TWO SceneView placement
paths in one file; only the RoomData-open path persists.

## (b) In-game F2 overlay — InPlayMapPaintOverlay

WORKS (verified):
- **Self-bootstrap**: `[RuntimeInitializeOnLoadMethod]` spawns the host; no scene wiring. F2 toggles, pauses (`Time.timeScale = 0`).
- **Tilemap discovery**: `DiscoverTilemaps` finds Floor/Cliff Tilemaps by name substring off the loaded room (`RoomLoader.OnRoomLoaded`) or scene scan.
- **Palette**: registry-first (`RuntimeAssetRegistry.Instance` tiles) with fallback to scanning tiles already on the tilemaps; prop palette from registry "wall"/"prop" tags, then `Resources/Live/WallKit`, then an Editor-only direct scan of `Assets/Sprites/Environment/RuinedKeepKit`.
- **Place / drag**: `BeginStroke`/`ContinueStroke`/`CommitStroke`. Tile layers paint via `PaintCell` along a Bresenham `GridLine`. Shift axis-locks (`AxisLockedCell`).
- **Prop mode**: drag-run via `WallRunBuilder.BuildRun` into `[DragPlace_Props]` parent (corner-sprite swap mid-run if `cornerSprite` set).
- **Undo**: tile strokes (`_undo` stack, restores `before` TileBase) and prop strokes (`_propUndo` stack, destroy-or-reactivate). Ctrl+Z. RMB cancels an in-progress LMB stroke.
- **Compose button**: "Compose Ruined Keep" → `RuinedKeepComposer.Compose()` (but that composer ships with an empty `_segments` list → logs "no wall segments authored" unless populated in inspector).

## (c) Do the two surfaces share RoomData? — NO (this is the core gap)

- **Editor window: YES, writes RoomData.** `RoomDataPlacementSink` calls `Undo.RecordObject(room,...)`
  then `PlaceCell`/`EraseCell` which mutate `room.floorCells` / `room.cliffCells` / `room.propPlacements` /
  `room.wallSegments`, then `controller.MarkDirty()` + `composer.Compose(room)`. Persisted on Save. Solid.
- **F2 in-game overlay: NO — instantiate/SetTile-only.** It paints into live `Tilemap` objects
  (`target.SetTile`) and instantiates prop GameObjects under `[DragPlace_Props]`. It **never references
  `RoomData`, `RoomDataAuthoringController`, or the placement sink.** Its undo stacks are in-memory only.
  Nothing it paints is written back to the RoomData asset; on Play-stop or room reload it is **lost**.

**Conclusion**: the "ONE shared RoomData across both surfaces" locked decision is **half-built**. The
Editor surface authors RoomData; the F2 surface is a transient live-paint that does not read or write
RoomData. They do not share data today.

## (d) Any Wang / auto-connect today? — NO

- **Zero neighbor-aware / autotile logic anywhere in the tool.** Grep for Wang/AutoTile/RuleTile/
  neighbor/adjacent/connect across `Assets/Editor/RoomPainter` returns only: a `"wang16"` *physics-rule
  string key* (`RoomPainterPhysicsRules.cs`), `"wang16"`/`"wang"` *filename categorizers* in
  `RoomPainterAssetScanner.cs` (they just bucket pre-baked sprites named "wang*" into the Floor layer),
  and a registry-baker keyword. None of these compute or apply tile connectivity.
- What exists instead = **single-cell paint** (`PlaceCell`/`PaintCell`) + **straight drag-RUNS**
  (`WallRunBuilder.BuildRun` along a Bresenham line, with an optional pre-authored `cornerSprite` swap
  when the run bends). The corner swap is the *closest thing* to connection-awareness, but it only
  fires inside a single drag stroke and needs a hand-authored corner sprite on the `WallPiece` — it is
  not Wang, not neighbor-scanning, and does not react to separately-placed adjacent pieces.
- So of the two locked connect modes: **drag-run = partially present** (straight runs + in-stroke
  corner sprite), **Townscaper click-single + auto-connect-to-neighbors = entirely absent.**

## (e) Compile state + bugs / dead code

Compile: nothing in the read set looks like it would fail to compile. Editor code is under
`Assets/Editor/...` (editor-only); F2 tool is `#if UNITY_EDITOR || DEVELOPMENT_BUILD` and avoids
UnityEditor APIs in the shared path (the one `UnityEditor` use in `LoadResourceWallKit` is itself
`#if UNITY_EDITOR`-guarded — correct). Cannot run the actual Unity compiler from here, so "green" is a
read-level judgement, not a verified build.

Bugs / smells:
- **Two divergent SceneView placement paths** in `RimaRoomPainterWindow.OnSceneGui`: RoomData-backed
  `RoomDataPlacementSink` vs legacy instantiate-only `RoomPainterScenePlacer`. The legacy path runs
  only when no room is open and writes nothing persistent — easy to paint "into the void" thinking it saved.
- **`RoomData.placements` (List<PlacementRecord>) is dead.** Declared + null-guarded in `EnsureDefaults`,
  but no writer or reader anywhere in the composer/sink. The live data lives in floorCells/cliffCells/
  wallSegments/propPlacements. `PlacementRecord` is orphaned.
- **`RuinedKeepComposer` ships empty** (`_segments` default empty) → the F2 "Compose Ruined Keep" button
  warns and does nothing unless segments are inspector-authored. Effectively a no-op button today.
- **F2 erase resolves wrong tilemap on undo**: `FindTilemapForEdit` ignores the cell and just returns
  the *currently active* tilemap (`overlay.ActiveTilemap()`), so undoing a stroke after switching
  Floor↔Cliff layers can restore tiles onto the wrong tilemap.
- **`worldPos == Vector3.zero` sentinel** in `RoomDataComposer.ComposeTileCells` / `ComposeProps`: a cell
  legitimately authored at world-origin is mistaken for "unset" and re-derived from the grid. Low-impact
  but a real edge bug.
- Minor: `DrawRoomLibraryRow` button reads `active ? "Open" : "Open"` (both branches identical — leftover).
- Minor: `RoomPainterScenePlacer.PaintCell` hardcodes the painted name to `Cliff_*` regardless of layer
  (legacy path only).

---

## TOP GAPS blocking the Townscaper feel (ranked)

1. **No auto-connect / Wang at all.** This IS the Townscaper feel and it does not exist. Today = single-cell
   stamp + straight drag-runs. Needed: neighbor-scan on place (and on neighbor place/erase) that picks the
   correct connecting sprite/tile from a bitmask, so separately-placed adjacent pieces visually join.
2. **F2 overlay does not share RoomData.** It is instantiate/SetTile-only and discards everything on stop.
   Until F2 writes through `RoomDataPlacementSink` (or an equivalent runtime sink) into the same RoomData
   asset, the "one shared RoomData, both surfaces" decision is unmet — Editor and in-game are separate tools.
3. **Two competing Editor placement paths** (`RoomDataPlacementSink` vs legacy `RoomPainterScenePlacer`).
   The legacy instantiate-only path should be removed/folded so all placement is RoomData-backed; the
   silent non-persisting fallback is a footgun.
4. **Wall "connect" is shallow.** `WallRunBuilder` only does straight Bresenham runs + a pre-authored
   `cornerSprite` swap inside one stroke. No T-junctions, no end-caps, no reaction to pre-existing
   neighbors — i.e. no real connected-array autotiling for the Conor-Dart drag mode.
5. **Dead `RoomData.placements` + empty `RuinedKeepComposer`** — minor, but clean before extending the data model.

**File written:** `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ROOMTOOL_AUDIT_S6.md`
