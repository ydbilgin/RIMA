# Build Mode Phase 2 — audit fixes (1)

Date: 2026-06-13 · Scope: DirectorMode.cs + BuildPlacementController.cs only.

## Fixes
- **[BLOCKER] Double-place gated** — `DirectorMode.cs` UpdateSpawnTool Build-tab branch
  (~954-964): when `BuildModeController.IsActive`, call `HidePropGhost()` + `return`
  BEFORE `UpdatePropTool(dt)`. BuildPlacementController is now the SOLE placer/ghost on
  the Build tab. Spawn tab + other tabs untouched.
- **[MINOR 1] Walkability comment** — `BuildPlacementController.cs` RefreshWalkability
  (~640): comment now states props block via Default-layer BoxCollider2D (not
  template.walkableGrid). InitFromTemplate KEPT — it still resets the Phase 1
  reachability flood-fill cache (verified: WalkabilityMap.InitFromTemplate sets
  `_reachableCache = null`). No behavior change.
- **[MINOR 2] Cell-based RMB erase** — `BuildPlacementController.cs`: new
  `FindPlacedAtCell(Vector2Int)` matches the prop whose rotated footprint covers
  `grid.WorldToCell(mouse)`. CommitEraseAt + EraseForValidation now use it (was
  world-radius EraseRadius search). Eyedropper keeps proximity FindPlacedNear
  (out of scope).

## Verify
- refresh_unity (force, scripts, compile=request) -> idle.
- read_console errors+warnings: **0**.

## BLOCKED / deviations
- None. All assumed methods/lines matched (verified BuildModeController.IsActive
  static prop, DirectorMode.HidePropGhost, GetRotatedFootprint(def,int) signature).
- Did NOT enter Play Mode (orchestrator runtime-verifies via *ForValidation hooks).
