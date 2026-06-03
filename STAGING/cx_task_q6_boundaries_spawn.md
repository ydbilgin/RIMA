# CX TASK — Q6: Player spawn + reward point off the void corner (3 scenes)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç (purpose)
The player currently spawns at the SW void/cliff corner instead of on the playable floor, and the room-clear `RewardSpawnPoint` sits at that same corner. Move both to the **center of the visible iso-diamond floor** in all 3 gameplay scenes, and confirm the walkable boundary actually matches the diamond so the player cannot walk into the void. Pure scene-transform edits — NO script changes, NO recompile.

## Diagnosis already done (orchestrator, _IsoGame, edit mode)
- `Player` (root, tagged Player) authored at **(-2.07, 3.10)**. It has NO spawn-manager script repositioning it at runtime, so editing the scene transform sticks.
- `Player` children `BoundaryCollider` and `SlashArcVFX` are parented to Player → they follow automatically. Do NOT touch them.
- `Ground` Tilemap localBounds: center (1.92, 8.19), size (26.88, 16.38) → **floor diamond center ≈ (1.92, 8.19)**.
- `RewardSpawnPoint` (root) authored at **(-2.07, 3.10)** = same void corner. Must move to floor center.
- `IsoFloorBoundary` EdgeCollider2D (5 pts, Default layer) centroid ≈ (2.16, 8.04) — already matches `_IsoGame` floor well; leave it unless broken.

## Scenes (do each, IN ORDER)
1. `Assets/Scenes/_IsoGame.unity`
2. `Assets/Scenes/_IsoGame_Map02.unity`
3. `Assets/Scenes/_IsoGame_Map03.unity`

## Per-scene procedure (UnityMCP, Unity instance RIMA@ed023e0b)
Use `execute_code` (action:execute, NO `using` directives — fully-qualified names like `UnityEngine.Tilemaps.Tilemap`). For EACH scene:

1. **Load** the scene (single, not additive). Make it active.
2. **Compute the floor diamond center** from the `Ground` Tilemap (find Tilemap whose GameObject name == "Ground"):
   - `var c = ground.transform.TransformPoint(ground.localBounds.center);` → use `(c.x, c.y)` as the target, keep each object's own original Z.
3. **Move Player**: find root GO tagged `Player` (fallback: name contains "player"/"warblade"). Set `transform.position = new Vector3(c.x, c.y, player.transform.position.z)`. Record old→new.
4. **Move RewardSpawnPoint**: find root GO named `RewardSpawnPoint`. Set its position to `(c.x, c.y, its own z)`. If the scene has no such object, log it (do NOT create one — note for Q5).
5. **Boundary check (verify, conservative)**: find GO named `IsoFloorBoundary` with an `EdgeCollider2D`. Compute the collider points' world centroid. If the boundary is **missing**, has **<4 points**, or its centroid is **>2.0 units** from the floor center `c`, then REBUILD it as a diamond from the Ground cellBounds corners:
   ```
   var cb = ground.cellBounds;
   var p0 = (Vector2)ground.CellToWorld(new Vector3Int(cb.xMin, cb.yMin, 0));
   var p1 = (Vector2)ground.CellToWorld(new Vector3Int(cb.xMax, cb.yMin, 0));
   var p2 = (Vector2)ground.CellToWorld(new Vector3Int(cb.xMax, cb.yMax, 0));
   var p3 = (Vector2)ground.CellToWorld(new Vector3Int(cb.xMin, cb.yMax, 0));
   // EdgeCollider2D points are LOCAL — subtract the boundary GO's world position; close the loop with p0 again.
   ```
   Set `edge.points = new[]{ p0-off, p1-off, p2-off, p3-off, p0-off }` where `off=(Vector2)boundaryGO.transform.position`. Keep the GO on the `Default` layer. **If the centroid is within 2.0u, LEAVE the boundary untouched** (it already matches — do not rebuild).
6. **Mark dirty + save** the scene (`EditorSceneManager.MarkSceneDirty` + `EditorSceneManager.SaveScene`, or `manage_scene save`).

## Verification (report in CODEX_DONE.md)
For each scene report a table: `oldPlayerPos → newPlayerPos`, `oldRewardPos → newRewardPos`, floor center used, boundary action (LEFT / REBUILT, with centroid before/after), and any missing object. Confirm `read_console` shows 0 new errors. Do NOT enter play mode (orchestrator will runtime-verify).

## Output
Write results to `CODEX_DONE.md` (append a Q6 section). State BLOCKED + reason if any scene can't be loaded or `Ground`/`Player` not found. Do NOT commit.
