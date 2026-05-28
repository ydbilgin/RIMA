# Codex Re-Review — Sprint 11 IMPL Delta
**Date:** 2026-05-16 S86 SPRINT11_IMPL_FIX
**Previous verdict:** FAIL with 4 blockers (see `STAGING/codex_review_sprint11_impl_DONE.md`)

## Fixes Applied

### Fix 1: WallOverlayPainter signature (Blocker 1)
**File:** `Assets/Scripts/MapDesigner/WallOverlayPainter.cs`
- Added spec-exact 4-arg overload: `public void PlaceWallSprite_ContextAware(Vector2Int pos, CompositionRoleMap, WangContextResolver, Tilemap walkableMask)`
- This new public method delegates to the existing variant-taking helper, reading candidates from new `[SerializeField] private AssetPoolSO l3WallVariantPool;` field
- The variant-taking helper renamed to `PlaceWallSprite_ContextAware_WithCandidates` (different signature, no collision)
- Both methods available; spec contract preserved via 4-arg version

### Fix 2: Fallback behavior (Blocker 2)
**Same file as Fix 1**
- If `compositionMap == null` → skip role gating (paint at every pos)
- If `wangResolver == null OR walkableMask == null` → pick first candidate (random fallback)
- If candidates empty/null → return null (cannot paint)
- The void 4-arg overload uses serialized field; gracefully no-ops if field unset

### Fix 3: WangContextResolver null-on-non-wall (Blocker 3)
**File:** `Assets/Scripts/MapDesigner/Composition/WangContextResolver.cs`
- Added check at start of `ResolveCaseAt`: if `!HasWallAt(walkableMaskTilemap, pos.x, pos.y)` → return null
- Per spec docstring: "Returns null if pos itself is not a wall cell."
- Test fixture updated: 2 existing tests now place a wall at `pos` itself; new test `ResolveCaseAt_PosNotWallCell_ReturnsNull` added

### Note on Blocker 4 (FreeformDecalExecutor worktree pollution)
This is **NOT a Sprint 11 issue**. The file was modified in Sprint 9 (per initial git status at session start: `M Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs`). Sprint 11 implementation did NOT touch this file. The blocker is technically valid (uncommitted prior-sprint work clutters scope verification) but cannot be fixed by Sprint 11 — needs a separate "commit Sprint 9 leftovers" task.

## Re-Review Scope

Verify ONLY the 3 fixes above. Do not re-verify full EC matrix (previous review confirmed EC-1 through EC-10 all PASS).

### Specific checks
1. `WallOverlayPainter.cs` line where 4-arg method declared — matches spec §2.5 exactly (`public void`, 4 params, names as spec).
2. `WallOverlayPainter.cs` — `[SerializeField] private AssetPoolSO l3WallVariantPool;` field present.
3. `WallOverlayPainter.cs` — existing public methods UNCHANGED (`PaintWalls`, `GetOutwardAnchor`, `PlaceWallSprite`).
4. `WangContextResolver.cs` `ResolveCaseAt` — null check at start for `pos` itself.
5. Test files updated to use renamed method (`PlaceWallSprite_ContextAware_WithCandidates`).

### Expected verdict
PASS (3 fixes verified) or FAIL with specific remaining issue.

## Output
`STAGING/codex_review_sprint11_impl_delta_DONE.md` — concise verdict, evidence, any remaining blockers.

Effort: medium.
