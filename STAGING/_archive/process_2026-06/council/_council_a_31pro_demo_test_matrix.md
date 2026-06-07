Here is the deep test architecture matrix for the RIMA Demo B-lite loop. 

This plan segregates tests by their **feedback loop speed** and **fragility**. Pure logic goes to lightning-fast EditMode property tests, Unity scene manipulation goes to PlayMode, and IMGUI aesthetics are delegated to manual Play-Verify.

### 1. DungeonGraph (Pure Logic / Generator)
**Lens:** The core mathematical generator. Must be completely bulletproof. We test this entirely in **EditMode** (headless C#, no Unity scene required).

| Behavior / Invariant | Assertion | Mode | Priority |
| :--- | :--- | :--- | :--- |
| **Determinism** | `Generate(seed, D)` called twice yields identical graph structures (same IDs, types, edges). | EditMode | P0 |
| **Start Node Invariant** | `NodesAtDepth(0).Count == 1` AND `type == Combat`. | EditMode | P0 |
| **Boss Node Invariant** | `NodesAtDepth(depthCount - 1).Count == 1` AND `type == Boss` AND `childIds.Count == 0`. | EditMode | P0 |
| **No Orphans (Indegree ≥ 1)** | Every node at `depth > 0` has at least 1 incoming edge from `depth - 1`. | EditMode | P0 |
| **Reachability (100%)**| A BFS/DFS starting from `startId` discovers exactly `graph.nodes.Count` nodes. | EditMode | P0 |
| **Outdegree Bounds** | Every non-Boss node has `[1, 3]` children. *(Note: guaranteed mathematically because `NodeCountAtDepth` maxes at 3).* | EditMode | P1 |
| **Edge Case: Depth < 2** | `Generate(seed, 1)` clamped to maxDepth=1 (Combat → Boss). | EditMode | P1 |
| **Edge Case: Seed = 0 / Neg**| `Generate(0, 5)` and `Generate(-99, 5)` execute without exception and form valid graphs. | EditMode | P2 |

### 2. RoomRunDirector (State Machine / Orchestrator)
**Lens:** State tracking and navigation. This is a `MonoBehaviour`, but its logical state (`graph`, `CurrentNodeId`, `CurrentChoices`) can be tested **WITHOUT a full scene** (EditMode) by instantiating a dummy GameObject and either allowing the `IsoRoomBuilder` reference to be null (expecting early-out error logs but valid state transitions) or injecting a minimal mock. 

| Behavior / Invariant | Assertion | Mode | Priority |
| :--- | :--- | :--- | :--- |
| **BeginRun Init** | `CurrentNodeId == graph.startId` AND `CurrentChoices.Count == 1` (since depth 1 has 1..3 nodes). | EditMode | P0 |
| **Valid Navigation** | `AdvanceTo(0)` updates `CurrentNodeId` to `CurrentChoices[0].id`. | EditMode | P0 |
| **Invalid Navigation** | `AdvanceTo(99)` and `AdvanceTo(-1)` are no-ops; `CurrentNodeId` remains unchanged. | EditMode | P1 |
| **Run Completion** | `IsRunComplete` is `false` initially, and exactly `true` when `CurrentNode.roomType == Boss` (or childless). | EditMode | P0 |
| **Template Fallback** | If `RoomBankSO.Pick` returns null, `CurrentTemplate` strictly equals `fallbackTemplate`. | EditMode | P1 |
| **Integration: Door Build** | `BuildExitDoors` receives a list of `RoomType` exactly matching the lengths and types of `CurrentChoices`. | PlayMode | P1 |

### 3. IsoRoomBuilder (Scene / Geometry Generation)
**Lens:** Side-effect heavy. Needs Unity Grid, Tilemaps, and Physics. Must run in **PlayMode** where Unity can execute a physics frame and populate transforms.

| Behavior / Invariant | Assertion | Mode | Priority |
| :--- | :--- | :--- | :--- |
| **Safe Execution** | `Build(validTemplate)` throws no exceptions. `Build(null)` logs warning, no crash. | PlayMode | P0 |
| **Floor Area Mass** | `LastFloorCells.Count` exactly equals `walkable cell count` + `blocking prop footprints` from the template. | PlayMode | P0 |
| **Player Teleport Node** | If template has `playerSpawn`, `PlayerSpawnMarker` is successfully instantiated and `!= null`. | PlayMode | P1 |
| **Exit Door Span** | `BuildExitDoors(types)` spawns exactly `types.Count` GameObjects under `gatesContainer`. | PlayMode | P1 |
| **Idempotency / Cleanup**| Calling `Build(A)` then `Build(B)` results in NO leaked markers, props, or tiles from A (count child transforms). | PlayMode | P1 |
| **Collider Generation** | `collisionTilemap` contains tiles for boundary; `CompositeCollider2D` path count > 0 after generation. | PlayMode | P2 |

### 4. RunMapOverlay (IMGUI Visualization)
**Lens:** Visual layout algorithm. Because it relies heavily on `OnGUI`, `Event.current`, and screen-space coordinates, unit testing this is brittle and low ROI. We define **CRISP manual acceptance criteria** for a human or vision-agent to play-verify.

| Behavior / Invariant | Acceptance Criteria (What to explicitly look for) | Mode | Priority |
| :--- | :--- | :--- | :--- |
| **Toggle & Input** | Pressing `M` once renders the overlay. Pressing `M` again completely hides it. | Manual | P0 |
| **Graph Orientation** | The Start Node (Depth 0) is at the **bottom** of the structure. The Boss Node is at the **top**. | Manual | P0 |
| **Current Node Focus** | EXACTLY ONE node has a glowing cyan border (`Color(0f, 1f, 0.8f, 1f)`) identifying the player's position. | Manual | P0 |
| **Path Sanity** | Lines only flow upward (from lower Y to higher Y). No crossed backwards lines. No floating disconnected nodes. | Manual | P1 |
| **Content Legibility** | Text within boxes strictly follows the `"{id}: {RoomType}"` format (e.g., `0: Combat`, `4: Elite`). | Manual | P2 |

---

### 5. Synthesis: Property Tests vs. Single Example Tests

To translate this into actual NUnit tests efficiently, we split our strategy:

**A. PROPERTY TESTS (Looping many seeds)**
*Best used for pure mathematical or algorithmic invariants.*
- **`DungeonGraph` Validation:** Loop 100 iterations with `random.Next()` seeds and varying `depthCount` (e.g., 3 to 10). 
- In a single test method, assert: `NodesAtDepth(0).Count == 1`, `orphans == 0`, `reachable == graph.nodes.Count`, and `1 <= children <= 3`. 
- *Why:* Uncovers bizarre edge-case seeds where lane-sorting ties or integer math rounding might cause a node to miss a parent or float off into space.

**B. SINGLE EXAMPLE TESTS (Given-When-Then)**
*Best used for State Machines and Scene manipulation.*
- **`RoomRunDirector` Navigation:** Given a fixed seed (e.g., `123`), verify `AdvanceTo(0)` moves to the precise hardcoded ID we know that seed produces.
- **`IsoRoomBuilder` Assembly:** Given a specific mock `RoomTemplateSO` (e.g., a 3x3 walkable square with 1 blocking prop), verify `LastFloorCells.Count == 10`. 
- *Why:* Running IsoRoomBuilder 100 times in PlayMode is slow. We just need to know it correctly interprets the data payload, which 1 or 2 explicitly crafted templates will prove perfectly.

