# Phase 5: Room Connector + Chain Builder Plan
# Session S103 | HD-2D Shattered Keep | 2026-05-23
# Status: PLAN — awaiting orchestrator approval before Codex dispatch

## 1. Connector Pattern Overview

Each room prefab carries one or more child empty GameObjects with a `RoomConnector` component, placed at doorway positions. A `RoomChainBuilder` maintains a queue of open connectors. To attach a new room, it picks a room prefab from a pool, finds a connector on that prefab with the opposing direction, then translates+rotates the candidate so its connector world-position coincides with the open connector's world-position. An `OverlapBox` bounds-check rejects placements that intersect already-placed rooms; on rejection the builder tries the next prefab in the pool. Once placed, the used connectors on both sides are marked closed and newly-exposed connectors of the placed room are enqueued. The process repeats until `maxRooms` is reached or the queue empties.

---

## 2. RoomConnector.cs Spec

**New file:** `Assets/Scripts/Rooms/RoomConnector.cs`
**Namespace:** `RIMA.Rooms`
**Component type:** `MonoBehaviour` (child empty GO on each room prefab)

| Member | Type | Notes |
|--------|------|-------|
| `direction` | `Direction` (enum: N, E, S, W) | Which face this connector exits on |
| `isOpen` | `bool` | True = available for chaining; set false once mated |
| `doorWidth` | `float` | Width of doorway in world units; default 2× `cellSize` = 4.0 |

**Direction enum:** `Assets/Scripts/Rooms/Direction.cs` (or nested in RoomConnector.cs)

```csharp
// Direction opposites used by chain builder
N ↔ S, E ↔ W
```

**Gizmo (OnDrawGizmos):**
- Draw a `Gizmos.DrawWireCube` at connector local position, size = `(doorWidth, 2f, 0.2f)`
- Draw an arrow in the direction vector (forward = direction facing outward)
- Color: green if `isOpen`, grey if closed

**Placement convention:**
- Position: cell edge world position matching the door cell in the footprint grid
- Rotation: faces outward from the room (N connector faces +Z outward, etc.)
- Naming: `Connector_N`, `Connector_S`, `Connector_E`, `Connector_W`

---

## 3. RoomChainBuilder.cs Spec

**New file:** `Assets/Scripts/Rooms/RoomChainBuilder.cs`
**Namespace:** `RIMA.Rooms`
**Component type:** `MonoBehaviour`

### Public Fields

| Field | Type | Default | Notes |
|-------|------|---------|-------|
| `roomPrefabs` | `GameObject[]` | — | Pool of room prefabs (each has `RoomShellBuilder` + `RoomConnector` children) |
| `seed` | `int` | 0 | RNG seed for deterministic generation |
| `maxRooms` | `int` | 8 | Hard stop |
| `overlapLayerMask` | `LayerMask` | Environment | Layers checked for collision |
| `overlapBoxHalfExtents` | `Vector3` | computed | Per-room: `footprint.widthCells * cellSize * 0.5f - 0.1f` on XZ |
| `snapToleranceUnits` | `float` | 0.05f | Alignment validation threshold |

### Algorithm (Pseudocode)

```
BuildChain():
  rng = new Random(seed)
  Place seedRoom at origin → add to placedRooms
  Enqueue all open connectors of seedRoom → openQueue

  while openQueue.Count > 0 AND placedRooms.Count < maxRooms:
    hostConnector = openQueue.Dequeue()
    candidateRooms = Shuffle(roomPrefabs, rng)

    for each candidateRoomPrefab in candidateRooms:
      oppositeConnectors = FindConnectors(candidateRoomPrefab, Opposite(hostConnector.direction))
      if oppositeConnectors is empty → continue

      mateConnector = oppositeConnectors[rng.Next(oppositeConnectors.Length)]
      
      // Compute snap transform
      snapRotation  = RotationToAlign(mateConnector.forward, -hostConnector.forward)
      snapTranslation = hostConnector.worldPos - Rotate(mateConnector.localPos, snapRotation)
      
      candidateBounds = ComputeBounds(candidateRoomPrefab, snapTranslation, snapRotation)
      
      if Physics.OverlapBox(candidateBounds, overlapLayerMask) is empty:
        instance = Instantiate(candidateRoomPrefab, snapTranslation, snapRotation)
        instance.GetComponent<RoomShellBuilder>().Rebuild()
        instance.GetComponent<RoomDecorator>()?.Decorate()   // Phase 4 hook
        
        hostConnector.isOpen  = false
        mateConnector (on instance).isOpen = false
        
        placedRooms.Add(instance)
        Enqueue all remaining open connectors of instance → openQueue
        break  // success — move to next queue entry
    // if all candidates rejected → hostConnector stays closed (dead end)
```

### Exposed Editor Controls

- `[ContextMenu("Build Chain")]` → calls `BuildChain()` in edit mode
- `[ContextMenu("Clear Chain")]` → destroys all placed rooms except seed

---

## 4. Integration with RoomFootprint

### ASCII Extension: 'D' Character

**Modified file:** `Assets/Scripts/Environment/Modular/RoomFootprint.cs`

| Change | Detail |
|--------|--------|
| Add `doorCells` field | `public List<Vector2Int> doorCells` — populated by `GetOccupancyGrid()` |
| Parse 'D' in ASCII grid | 'D' cell = occupied floor (treated as '#') AND recorded in `doorCells` |
| `GetOccupancyGrid()` side-effect | Clears `doorCells`, then fills it during parse |

**Example ASCII with doors (8×8 MedRect):**

```
########
#......#
#......#
D......D
D......D
#......#
#......#
########
```

(D on west = west door; D on east = east door)

### RoomShellBuilder Integration

**Modified file:** `Assets/Scripts/Environment/Modular/RoomShellBuilder.cs`

| Change | Detail |
|--------|--------|
| Door cell wall skip | In `BuildNorthWalls()` and `BuildWestWalls()`, check `footprint.doorCells.Contains(new Vector2Int(x,z))`; if true → skip wall spawn |
| Auto-spawn RoomConnector | After skipping wall on door cell → `Spawn` a child empty GO named `Connector_{dir}` with `RoomConnector` component, direction inferred from which wall face was skipped |
| Connector group | New `CreateGroup("Connectors")` child parallel to existing groups |

---

## 5. Validation Rules for Chained Rooms

| Rule | Method | Threshold | Action on Fail |
|------|--------|-----------|----------------|
| No room overlap | `Physics.OverlapBox` on placed bounds vs `overlapLayerMask` | 0 hits | Reject candidate room, try next in pool |
| Connector alignment | `Vector3.Distance(hostConnector.worldPos, mateConnector.worldPos)` after snap | ≤ 0.1 units | Log error + reject; should not occur with correct snap math |
| Connectivity (BFS post-build) | BFS from seed via `isOpen=false` connector pairs | All rooms reachable | Log warning; RoomChainBuilder exposes `bool ValidateConnectivity()` |
| Min floor area | `sum(placedRooms[i].footprint.widthCells * heightCells * cellSize²)` | ≥ `minTotalFloorArea` (default 200 u²) | Log warning only (non-blocking) |
| Door width clearance | `doorWidth` on connector ≥ 1× `cellSize` | 2.0 units min | Assert in `RoomConnector.OnValidate()` |

---

## 6. Showcase Scene

**New scene:** `Assets/Scenes/RoomChainShowcase.unity`

| Element | Detail |
|---------|--------|
| Chain | Seed room = `RF_MedRect_12x12` + 5 chained rooms picked from all 7 footprint shapes |
| Camera | Orthographic, size ~30, position Y=20, Euler (-35, 45, 0) — matches HD-2D convention |
| Lighting | Directional light at HD-2D angle; no post-processing (Phase 3 volume optional) |
| RoomChainBuilder GO | Placed at scene root; `maxRooms=6`, `seed=42` |
| Labels | Each room GO named `Room_0_MedRect`, `Room_1_LShape`, etc. for readability |
| Purpose | Visual smoke-test; not gameplay; no enemies or player |

---

## 7. Open Questions for User

1. **Rotation constraint** — should RoomChainBuilder allow all 4 rotations (0/90/180/270) when snapping, or only 0/180 (no sideways rooms)? Affects snap math complexity.
2. **Dead-end rooms** — when all connector candidates are rejected, should dead-end connectors get a sealed-door prefab spawned (Phase 4 gate socket), or just an open wall gap?
3. **Footprint door cell authoring** — are door positions authored only in ASCII ('D' char), or should there also be an Inspector list override for programmatic door assignment?
4. **RoomConnector on existing footprint assets** — the 7 `.asset` files under `Assets/Data/Environment/Footprints/` have no 'D' cells yet. Should Phase 5 Codex task add 'D' cells to all 7, or only to the 3 most common shapes (`RF_SmallRect_8x8`, `RF_MedRect_12x12`, `RF_LShape_16x16`)?
5. **Collision layer setup** — `RoomChainBuilder.overlapLayerMask` needs a `RoomBounds` layer in Unity LayerManager. Should Phase 5 Codex task create that layer, or assume it exists?
