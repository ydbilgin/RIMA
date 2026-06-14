# CX CODE TASK — RoomPainter Window v2: map library + top-down place + save + playtest (S6)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"` if needed.

## THIS IS YOUR OWN DESIGN
You (Codex) already wrote the architecture in `CODEX_DONE.md` (search "Q2 - Real map editor architecture", ~line 4847). IMPLEMENT THAT. Read it first. Below are the resolved open decisions + MVP scope.

READ: `CODEX_DONE.md` (your Q2 section) · `Assets/Scripts/RoomPainter/` (RoomData.cs, RoomLayer.cs, RoomPainterAsset.cs, RoomLayerData.cs) · `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` · `Assets/Scripts/DevTools/WallRunBuilder.cs` + `RuinedKeepComposer.cs` · `Assets/Scripts/Live/RuntimeAssetRegistry.cs`.

## OPUS DECISIONS on your 3 open questions
1. **RoomData schema:** SEPARATE typed lists (not one unified list): keep per-layer tile-cell lists (floor/cliff) + a `List<WallSegment>` (walls) + a `List<PropPlacement>` (props/decals: {assetGuidOrName, cell/pos, rotation, scale}) + `string thumbnailPath`. Add `roomId` (stable) + `displayName` if missing.
2. **Thumbnails:** PNG files under `Assets/Data/Rooms/Thumbnails/<roomId>.png` (AssetDatabase-loadable, simple), baked on save/load.
3. **WallSegment/WallPiece namespace:** MOVE the data structs `WallPiece`, `WallSegment`, `SegmentKind` OUT of the `#if UNITY_EDITOR||DEVELOPMENT_BUILD`-gated `WallRunBuilder.cs` into a NEW ungated runtime file `Assets/Scripts/RoomPainter/RoomPlacementTypes.cs` (namespace `RIMA.RoomPainter`) so the runtime `RoomData` SO can serialize them. Update `WallRunBuilder.cs`, `RuinedKeepComposer.cs`, `InPlayMapPaintOverlay.cs` to reference `RIMA.RoomPainter.WallPiece` etc. (the BuildRun *logic* stays gated in DevTools; only the data structs move). Compile both gated + ungated.

## MVP SCOPE (v1 — demoable; advanced = later)
Build "RoomPainter Window v2" = a dockable Editor window (extend `RimaRoomPainterWindow` or a v2 alongside it) with:
1. **MAP LIBRARY / BROWSE (top of window or a mode):** `AssetDatabase.FindAssets("t:RoomData", new[]{"Assets/Data/Rooms"})` → list maps with name + thumbnail. Buttons: **New** (create RoomData asset w/ default layers + new roomId), **Duplicate**, **Delete**, **Open** (set active). Empty-state if folder missing (create it).
2. **TOP-DOWN EDIT VIEW:** when a map is open, drive editing in the **SceneView** (top-down): a `RoomDataComposer` clears `[RoomPreview_Generated]` root and rebuilds floor/cliff tiles + walls (via `WallRunBuilder.BuildRun` over the segment list) + props from the active RoomData. Pan/zoom = SceneView native.
3. **PALETTE (left, tabbed by layer Floor/Cliff/Wall/Prop):** reuse `RoomPainterAssetScanner`/registry; thumbnails. Selecting an asset = active brush.
4. **PLACE → RoomData (placement sink):** SceneView mouse: click/drag places into the ACTIVE RoomData FIRST (tile cell / wall segment line-stroke / prop at cell), THEN the composer updates the preview. Right-click/erase removes. This is the core "place → map is built" loop.
5. **SAVE:** explicit Save button → write active RoomData (`EditorUtility.SetDirty` + `AssetDatabase.SaveAssets`) + bake a top-down thumbnail PNG (RoomThumbnailBaker: render the preview root from a top-down ortho cam to a RenderTexture → PNG → `Thumbnails/<roomId>.png`). Dirty-flag indicator.
6. **PLAYTEST button (prominent, top bar):** save, then load the active RoomData into the play scene (compose into the live scene's room root) and `EditorApplication.isPlaying = true`. Minimal: a `RoomDataComposer.ComposeInto(sceneRoot)` + enter play. (If full play-wiring is heavy, a BLOCKED note + compose-into-scene without auto-play is acceptable for v1.)

Suggested components (your design): `RoomDataAuthoringController` (active room, dirty, load/save), `RoomDataPlacementSink` (paint→RoomData), `RoomDataComposer` (clear/rebuild from RoomData; reuse WallRunBuilder), `RoomThumbnailBaker`. Editor window = the shell (browse panel + palette + inspector + top bar). Keep the in-Play F2 overlay AS-IS (secondary scratch tool — do not break it).

## ACCEPTANCE
- `dotnet build` + Unity compile clean (gated + ungated halves). Report files new/edited + the RoomData schema + any BLOCKED.
- Smoke: New map → place a few floor tiles + a wall run + a prop → Save (asset + thumbnail written) → reopen from browser → preview re-composes identically.
- Do NOT touch character art / combat / the F2 overlay's existing behavior. Do NOT commit.
- If MVP is too large for one pass, implement in the listed order 1→6 and STOP with a clear "done through step N" note rather than half-doing everything.
