Here is the lean, ship-fast testing critique and plan for the **RIMA Demo B-lite** run loop.

---

### 1. The Leanest Test Set (~6-10 Tests)
Ranked by **regression-value-per-effort** (highest value, lowest maintenance cost):

1. **[DungeonGraph.Generate](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs#L68) Determinism** (EditMode)
   * *Behavior:* Assert that calling `DungeonGraph.Generate(seed, depth)` twice with the same seed outputs identical node IDs, depth levels, room types, and parent-child connections.
   * *Why:* procedural generation bugs are hard to find manually. Setting a seed must guarantee identical graphs.
2. **[DungeonGraph.Generate](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs#L68) Connectivity & No Orphans** (EditMode)
   * *Behavior:* Assert that every node $id > 0$ has at least one incoming connection, and every node at $depth < maxDepth$ has at least one child connection.
   * *Why:* Catch graph-generation changes that isolate rooms, creating game-breaking soft-locks where a player enters a room with no exits.
3. **[DungeonGraph.Generate](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs#L68) Boundary Constraints** (EditMode)
   * *Behavior:* Assert that depth 0 has exactly one node (type `Combat`), the final depth has exactly one node (type `Boss`), and intermediate depths have 2–3 nodes.
   * *Why:* Enforces the core layout design of the run.
4. **[RoomRunDirector.BeginRun](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L36) State Setup** (EditMode)
   * *Behavior:* Call `BeginRun()` with a stub builder; assert `CurrentNodeId == 0`, `IsRunComplete == false`, and the starting template is selected correctly.
   * *Why:* Verifies the initial run state transitions correctly.
5. **[RoomRunDirector.AdvanceTo](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L102) Graph Traversal** (EditMode)
   * *Behavior:* Traverse from node 0 to a child node; assert that `CurrentNodeId` updates to the chosen child's ID and `IsRunComplete` updates appropriately.
   * *Why:* Validates the core logic of progressing through a run.
6. **[RoomRunDirector.AdvanceTo](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L102) Invalid Input Handling** (EditMode)
   * *Behavior:* Call `AdvanceTo(-1)` or `AdvanceTo(outOfBoundsIndex)`; assert that `CurrentNodeId` does not change and a warning is logged.
   * *Why:* Protects against out-of-bounds array exceptions during room progression.
7. **[IsoRoomBuilder.Build](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs#L65) Tile Placement Boundary** (PlayMode/EditMode integration)
   * *Behavior:* Pass a mock 3x3 `RoomTemplateSO` (center walkable, edges void); assert that `groundTilemap` has tiles at the center and `collisionTilemap` has tiles surrounding it.
   * *Why:* Verifies that walls/boundaries are drawn correctly relative to floor cells.
8. **[IsoRoomBuilder.BuildExitDoors](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs#L577) Count Matching** (PlayMode/EditMode integration)
   * *Behavior:* Call `BuildExitDoors` with 2 door types; verify exactly 2 exit door GameObjects are instantiated in the container, and each child's sprite corresponds to its designated room type.
   * *Why:* Ensures the physical exit doors generated match the logical progression options.

---

### 2. Over-Engineering Critique (Do NOT Write These)
* **[RunMapOverlay](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RunMapOverlay.cs#L8) UI Rendering Tests:** Do not write unit tests verifying colors (`ColorFor`), labels, lines, or UI box coordinates. Legacy `OnGUI` UI is notoriously difficult to mock, and visual appearance is best verified by pressing `M` in-game.
* **Cliff Sprite Tuck Offsets / Sort Order Math:** Do not unit test float positions (e.g., `tuckSouthEast = new Vector2(-0.48f, 0.29f)`) or dynamic sorting calculations (`cliffSortOrderBase + Mathf.RoundToInt(...)`). These are visual polish variables that designers will tweak constantly; unit testing them creates high-maintenance tests that break on harmless aesthetic edits.
* **Physics Colliders and Rigidbodies Settling:** Testing whether a `CompositeCollider2D` successfully stops a player or whether physics objects settle down correctly. Physics testing is flaky, frame-rate dependent, and takes 5 seconds to test by playing.
* **Player Teleportation Math:** Testing whether `player.position` exactly matches `PlayerSpawnMarker.position`. It is a simple assignment; testing it requires building fake hierarchies and mock components for very little value.

---

### 3. Flaky-Risk in Unity & How to Avoid
* **Physics / Trigger-based Run Advancement:** If you try to test running through the level by simulating the player's physical collider hitting a door trigger to invoke `AdvanceTo`, the test will be flaky due to physics step intervals (`FixedUpdate`). 
  * *Fix:* Decouple trigger detection from run logic. Test `AdvanceTo` directly via method calls on the script interface, and trust Unity's native collision events for the actual game.
* **Input-Triggered Menus (`OnGUI` / `Event.current`):** Simulating a keyboard down event for `KeyCode.M` to test if [RunMapOverlay](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RunMapOverlay.cs#L8) toggles. `Event.current` is unreliable in editor unit tests.
  * *Fix:* Expose a public method `Toggle()` and test the boolean state toggle directly without simulating keystrokes.
* **ScriptableObject Assets Dependency:** If `RoomRunDirector` tests depend on loading assets via resource paths or database lookups, they will break on clean builds.
  * *Fix:* Pass simple in-memory mock instances of `RoomTemplateSO` and `RoomBankSO` during testing setup.

---

### 4. EditMode vs PlayMode Partitioning
* **EditMode (90% of code logic):** Everything inside [DungeonGraph](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs#L14) is pure C# and should be tested in EditMode. The traversal logic inside [RoomRunDirector](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L7) can also run in EditMode by ensuring it handles `builder = null` or a simple mock.
* **PlayMode (Minimum requirement):** Only the tile painting, child GameObject instantiation, and collider generation inside [IsoRoomBuilder](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs#L11) genuinely need PlayMode because they interact with Unity's active `Tilemap` systems, physics updates, and GameObject lifecycles.
* **Minimum PlayMode set:** Exactly **1 integration test** that spins up a test scene, calls `BeginRun()`, and verifies that tiles are painted on `groundTilemap` and exit doors are instantiated.

---

### 5. Ship-Fast Recommendation
> **Test NOW:** Focus entirely on EditMode tests for [DungeonGraph](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs#L14) generation (determinism, depth rules, and orphan prevention) and logical run state transitions in [RoomRunDirector](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L7). These are low-overhead, run instantly, and protect against catastrophic run-breaking game loops.
>
> **Defer:** Defer all tests for visual offsets, cliff rendering heuristics, legacy `OnGUI` overlay visibility, and physical player collision triggers. These components are volatile during a demo's design phase; they are best validated visually during manual playtests and can wait until the design layout is locked down for production.

