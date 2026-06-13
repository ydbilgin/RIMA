# DONE — Build Mode PHASE 3 (cell-authoritative tile/walkability/overlay brush)

> 2026-06-13. Implemented per STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md (Phase 3 row + 3.5)
> + STAGING/BUILDMODE_TERRAIN_DECISION_2026-06-13.md (organic = OUT; discrete Grid brush only).

## FILES
- NEW  Assets/Scripts/UI/BuildMode/BuildTileBrushController.cs (925 lines) — the brush.
- EDIT Assets/Scripts/UI/BuildMode/BuildPlacementController.cs (839 -> 938, +99) — BuildTool selector
  (exclusivity), shared CommandStack accessor, PROP|TILE UI toggle, *ForValidation tool hooks.

## WHAT IT DOES
- Sub-modes: FloorPaint (LMB walkable+floor tile / RMB void+clear), WalkableToggle (LMB walk /
  RMB block, walkability-only), OverlayPaint (LMB overlay / RMB clear). 1=Floor 2=Walk 3=Overlay.
- LMB paints / RMB erases at grid.WorldToCell(mouse); cursor highlight snaps to
  grid.GetCellCenterWorld (green=in-bounds, red=out). Optional radius 1-3 (-/+); 1x1 is exact.
- Writes BOTH the room-data authority (working-copy walkableGrid/overlayMask) AND the live Tilemap
  (groundTilemap.SetTile / OverlayTilemap), then WalkabilityMap.InitFromTemplate(workingCopy) so
  pathing stays correct (it aliases the array -> live update).
- Reversible: every brush op is an IBuildOp on BuildPlacementController's SHARED BuildCommandStack
  (captures prior per-cell state); Ctrl+Z/Y interleave prop+tile undo.

## TWO HARD CONSTRAINTS
1. TOOL EXCLUSIVITY: single ActiveTool selector inside BuildPlacementController. Update() dispatches
   the click by ActiveTool BEFORE any leftButton read — Prop tool runs HandleCursor only when
   ActiveTool==Prop (else HideGhost, no place); the brush runs its loop only when ActiveTool==Tile.
   One LMB = exactly one tool. No second click-reader resurrected.
2. NO ASSET POLLUTION: brush NEVER mutates CurrentTemplate (the raw source .asset). On first edit
   per room it Object.Instantiate()s a working copy (deep-copies bool[]/int[]; hideFlags=DontSave),
   edits THAT + live tiles, feeds it to WalkabilityMap. EditorUtility.SetDirty is NEVER called
   (disk persistence deferred to Phase 4). UsesWorkingCopyForValidation() proves copy != source.

## VERIFICATION
- refresh_unity force compile -> read_console: 0 errors, 0 BuildMode warnings (Unity idle, NOT played).
- Static reflection (no Play Mode): all 3 types resolve; BuildTool{Prop,Tile}; BrushMode{FloorPaint,
  WalkableToggle,OverlayPaint}; ActiveTool/SetActiveTool/CommandStack(internal)/SelectToolForValidation/
  ActiveToolForValidation present; all 15 brush *ForValidation hooks present.
- Runtime behavioral proof (paint/erase/walkability/exclusivity invariant) = orchestrator Play-Mode
  job via the exposed hooks. NOT run here (Play Mode forbidden by task).

## KNOWN LIMITATIONS / DEVIATIONS
- Overlay stamp falls back to the sampled floor tile (IsoRoomBuilder.overlayTiles[] is private, no
  accessor). overlayMask data is written correctly (the authority); a full Build() would render the
  real overlay tile. Documented in-code.
- Tool toggle surfaced on BuildPlacementController's own palette (PROP|TILE row), NOT on
  DirectorMode's Build panel — keeps the edit surgical (no DirectorMode change needed; the contract
  allowed either). Mode buttons live on the brush's own right-side panel.
- Working copy carries walkability/overlay only; props still go to the source asset via the existing
  P2 path (unchanged, out of Phase-3 scope).
