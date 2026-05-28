# Walkability + Dash MVP (S107)

**Status:** MVP implemented 2026-05-26
**Related:** [[Cliff_System]] [[S107_Overnight_Log]] [[Open_Decisions]]

## Components

### `WalkabilityMap.cs`
- Path: `Assets/Scripts/Environment/WalkabilityMap.cs`
- Tilemap-based cell lookup
- Static `Instance` for global access
- API: `IsWalkable(Vector3 worldPos)` / `IsWalkableCell(Vector3Int cell)`

### `IObstacle.cs` interface
- Path: `Assets/Scripts/Environment/IObstacle.cs`
- Future hook for passable/destructible tile types
- Currently unused by gameplay; reserved for breakable cliffs / spike-tile mods

### `PlayerController` integration
- Path: `Assets/Scripts/Player/PlayerController.cs`
- `TryDash()` pre-check — validates dash destination walkability (gap atlama enabled)
- `FixedUpdate()` movement validation — prevents player from leaving painted floor

### Void blocker tile
- Asset: `Assets/ScriptableObjects/Tiles/VoidBlocker_Tile.asset`
- AutoFill triggered on Floor tilemap change (perimeter + collision)
- Composite collider on void Tilemap blocks runtime movement

## Validation Flow

1. Floor painted (manual via Painter v4 or auto via Painter)
2. AutoFill places VoidBlocker around painted perimeter
3. WalkabilityMap rebuilt from current Floor tilemap
4. PlayerController FixedUpdate clamps movement to walkable cells
5. PlayerController TryDash allows landing on walkable cells across non-walkable cells (gap jumping)

## Edge Cases

- Stuck pocket: if player ends up on non-walkable tile (e.g. spawn placed wrong), no auto-recovery yet
- Multiple Floor tilemaps: WalkabilityMap currently single-tilemap; multi-layer requires refactor
- Reachability for portal spawn: NOT YET — flood-fill check is queued (Sonnet follow-up, see [[Reward_Portal_Flow]])

## Sabah Open Items

See [[Open_Decisions]] — reachability constraint for `PortalSpawnAnchor` + flood-fill Player-reachable check.
