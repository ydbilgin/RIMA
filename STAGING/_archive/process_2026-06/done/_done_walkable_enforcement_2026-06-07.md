# DONE: Walkable Enforcement — 2026-06-07

Commit: `3b800815`  
"fix(physics): enforce walkable grid for players and mobs incl. interior holes and knockback"

## Work Items — File:Line Map

### 1. Boundary ring (inner holes)
`Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs:479-497` — `BuildBoundary` already
places collision tiles on all non-walkable cells that have at least one walkable 8-neighbor
(covers inner hole cells as well as the outer ring). No code change needed here — the audit
concern was verified correct by the new test.

### 2. WalkabilityMap — source from template
`Assets/Scripts/Environment/WalkabilityMap.cs:196-255`
- Added `InitFromTemplate(RoomTemplateSO)` method (line 199): stores `walkableGrid` + `bounds`
  in private fields `_templateWalkableGrid`, `_templateBounds`, `_hasTemplateGrid`.
- `IsWalkable(Vector3Int)` (line 225): template path first (`IsWalkableByTemplate`), tilemap
  fallback when no template loaded.
- `IsWalkableWorld(Vector3)` (line 237): same priority, FloorToInt approximation when no
  tilemap for cell coordinate conversion.
- `IsDashableWorld(Vector3)` (line 265): same priority.

`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:197-206` — `BuildCurrentRoom`
calls `WalkabilityMap.Instance?.InitFromTemplate(template)` after each `builder.Build()`.

### 3. Mob movement clamp
`Assets/Scripts/Enemies/BaseMobBehavior.cs:181-183` — Chase branch now calls
`WalkabilityMap.ClampVelocityToWalkable(WalkabilityMap.Instance, transform.position, desiredVel, Time.fixedDeltaTime)`.
Permissive (returns velocity unchanged) when `WalkabilityMap.Instance == null`.

### 4. Knockback clamp (player + mob)
`Assets/Scripts/Core/KnockbackReceiver.cs:83-89` — `DoKnockback` coroutine caches
`WalkabilityMap.Instance` once before loop; clamps `frameVel` each iteration.
`Assets/Scripts/Core/KnockdownDriver.cs:185-194` — `MoveArc` coroutine caches walkMap and
clamps launch velocity each frame (knockdown path does NOT go through DoKnockback, so this
is the required coverage).

### 5. Diagonal corner-cut fix (shared helper)
`Assets/Scripts/Environment/WalkabilityMap.cs:283-326` — static `ClampVelocityToWalkable`:
  - `diagonal` = both velocity components non-zero.
  - Diagonal: check `nextPos` → if void, try X-slide then Y-slide individually.
  - Axis-aligned: if `nextPos` void → zero.
  - No corner-cutting: xOk/yOk evaluated independently; a blocked slide returns zero.
`Assets/Scripts/Player/PlayerController.cs:305-322` — `FixedUpdate` replaces the old inline
axis-split with `ClampVelocityToWalkable`; dash FixedUpdate path also clamped.

### 6. Elite teleport
`Assets/Scripts/Enemies/EliteAffix.cs:183-203` — `Teleport` now has `_warnedTeleportFail`
bool; after all attempts fail, `Debug.LogWarning` logged once per mob.
`Assets/Scripts/Enemies/EliteAffix.cs:205-237` — `IsTeleportDestinationValid` uses
`WalkabilityMap.Instance.IsWalkableWorld` + `IsReachableFromPlayer` as primary check;
legacy tilemap fallback used only when no WalkabilityMap in scene.

### 7. Prop layer
`Assets/Scripts/MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs:24-51` — `EnsureCollider`
calls `EnsureDefaultLayer()` after adding or finding an existing collider.
`EnsureDefaultLayer()` (line 52-56): `LayerMask.NameToLayer("Default")` → set if different.
`Awake` (line 58-60): also calls `EnsureDefaultLayer()` unconditionally.

### 8. Tests
`Assets/Tests/EditMode/Room/WalkableEnforcementTests.cs` — 10 tests, all pass:

| Test | Result |
|------|--------|
| `BuildBoundary_DonutTemplate_HoleCellGetsCollisionTileAndWalkableRimDoesNot` | PASS |
| `BuildBoundary_SolidRoom_NoInnerCollisionTiles` | PASS |
| `WalkabilityMap_InitFromTemplate_HoleCellReturnsNotWalkable` | PASS |
| `WalkabilityMap_InitFromTemplate_NullClearsGrid` | PASS |
| `ClampVelocity_DirectlyIntoVoid_ZeroVelocity` | PASS |
| `ClampVelocity_DiagonalIntoVoidCenter_SlidesThenZeroWhenBothBlocked` | PASS |
| `ClampVelocity_KnockbackProbe_StopsAtVoidBoundary` | PASS |
| `ClampVelocity_WalkableTarget_PassesThrough` | PASS |
| `ClampVelocity_NullWalkMap_Permissive` | PASS |
| `PropColliderAutoBuilder_BlockingProp_AssignsDefaultLayer` | PASS |

Full EditMode suite: 449 tests, same 19 pre-existing failures as before, 0 new failures.

## Probe Evidence

### (a) Mob can't cross donut hole
`ClampVelocityToWalkable` called in `BaseMobBehavior.FixedUpdate` for chase velocity.
Probe test: actor at (1.5, 2.5) with velocity (10,0) toward hole (2,2); dt=0.09 → returns
`Vector2.zero` — confirmed by `ClampVelocity_KnockbackProbe_StopsAtVoidBoundary` (same
geometry with higher force).

### (b) Player knockback into hole stops at boundary
`DoKnockback` per-frame clamp: actor at (1.5, 2.5), frameVel=(10,0)*t, dt=0.016 → void →
zero applied. Confirmed by `ClampVelocity_KnockbackProbe_StopsAtVoidBoundary`.

### (c) Smoke / existing tests unchanged
Full EditMode 449 tests: 430 pass, 19 fail (same 19 as pre-existing).
- Socket tests: pass (RoomTemplateSocketTests).
- Round-trip tests: pass (RoomTemplateSaveLoadTests, IsoRoomBuilderOverlayTests).
- New 10 tests: all pass.

## Constraints Satisfied
- Player↔Enemy collision NOT changed (BaseMobBehavior:86 untouched).
- Chasm/NarrowPassage trigger system NOT touched.
- All clamps are O(1) grid lookups — no BFS in hot path.
- UnifiedMapDesigner / RoomJsonImporter / RoomTemplateJsonExporter NOT touched.
