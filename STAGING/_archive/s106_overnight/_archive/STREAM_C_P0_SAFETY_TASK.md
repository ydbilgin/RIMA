ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Otherwise read-only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / STAGING/s106_overnight/MASTER_CONTEXT.md / Assets/Scripts/Runtime/Walls/V2/**/*.cs / Assets/Scripts/Editor/Walls/V2/**/*.cs / Assets/ScriptableObjects/Walls/V2/*.asset

Amaç: RIMA Wall V2 logic'inde 6 P0 safety bug'ını surgical fix et. Bu fix'ler tamamlanmadan Stream D (painter UX) ve Stream E (5 test odası) güvenli claim edilemez. NO new features, NO refactor — sadece listed bug'lar.

---

# STREAM C — P0 SAFETY FIX PASS

## Context

S105 Codex P0 wave 7 bug fix tamamladı, ama Codex ideation (yasinderyabilgin profile, 2026-05-25 03:13) DEEPER review yaptı ve 6 KALAN bug buldu. Hepsi line-number ile spesifik, hepsi Opus tarafından file-level doğrulandı. Bu fix'ler Stream D/E claim'inin VERIFIABLE olmasının ön şartı.

**Verifiable proof gerek (her bug için):**
- Before / after diff (line numbers ile)
- Reasoning (neden bu fix doğru)
- 0 Unity console error (compile sonrası)
- Eğer behavior değişti → manuel test scenario yaz

---

## BUG 1 — Edge sort axis (WallChainRoomBuilder.cs)

**Symptom:** Irregular footprint (non-rectangular) odalarda rear/side/front edge'ler GroupConsecutive'a yanlış sırada gidiyor → wall chain fragmente oluyor, runs kırılıyor.

**Lines & fix:**
- `WallChainRoomBuilder.cs:275` — `list.Sort((a, b) => a.cell.x.CompareTo(b.cell.x));`
  → Change to: sort by `(y, x)` so segments on the SAME ROW group together first, then by x ordering within row.
- `WallChainRoomBuilder.cs:286` — `list.Sort((a, b) => a.cell.y.CompareTo(b.cell.y));`
  → Change to: sort by `(x, y)` so segments on the SAME COLUMN group together first, then by y.
- `WallChainRoomBuilder.cs:296` — same fix as line 275 (front edge).

**Sort key suggestion:**
```csharp
// Rear/Front: same y first, then x ascending
list.Sort((a, b) => {
    int yc = a.cell.y.CompareTo(b.cell.y);
    return yc != 0 ? yc : a.cell.x.CompareTo(b.cell.x);
});
// Side: same x first, then y ascending
list.Sort((a, b) => {
    int xc = a.cell.x.CompareTo(b.cell.x);
    return xc != 0 ? xc : a.cell.y.CompareTo(b.cell.y);
});
```

**Verification:** Build an irregular footprint (e.g. L-shape, 5×5 with NE corner cut) and confirm rear chain doesn't span across two separate rows (which would indicate wrong grouping). Add a XML comment above each Sort explaining the (axis, axis2) ordering rationale.

---

## BUG 2 — length=2 corner suppression missing (WallChainRoomBuilder.cs:419-428)

**Symptom:** `FillRunWithSpans` length=1 has `if (startIsCorner || endIsCorner) return;` (line 405) but length=2 (line 419-428) IGNORES that check → wall pieces overlap corner prefabs on 2-cell runs.

**Fix:** Add the same corner check at line 419-428:
```csharp
if (length == 2)
{
    bool flipX;
    var single = GetSpanForLength(dir, 1, out flipX);
    if (single != null)
    {
        // Skip cells already covered by corner prefabs
        if (!startIsCorner)
            SpawnPieceFromData(single, GetCellWorld(start, fixedCoord, horizontal), flipX);
        if (!endIsCorner)
            SpawnPieceFromData(single, GetCellWorld(start + 1, fixedCoord, horizontal), flipX);
    }
    return;
}
```

**Verification:** Test rectangle room with size 4×4. Side chain length will be 2, both ends are corners. Confirm no wall piece spawns inside the corner cells (only the corner prefab itself).

---

## BUG 3 — wpd_door_arch.asset has gameplay collider (BLOCKER!)

**Symptom:** Door has `colliderSize: {x: 2, y: 1}` (line 28 of asset YAML). `WallPiece.cs:28-32` ApplyMetadata sets `BoxCollider2D.size = data.colliderSize`. **Result: player CANNOT walk through doors.** This silently breaks every test room.

**File:** `Assets/ScriptableObjects/Walls/V2/wpd_door_arch.asset`

**Fix:** Set `colliderSize: {x: 0, y: 0}` (effectively no collider). Keep `footprintSize: {x: 2, y: 1}` unchanged (footprint is for sprite scaling/anchoring, not collision).

**Alternative (cleaner):** Add a `hasCollider` bool field to `WallPieceData`, defaulting true; `wpd_door_arch` and `wpd_open_gap` set it to false; `WallPiece.ApplyMetadata` skips collider apply if false AND optionally `Destroy(footprintCollider)` to fully remove the component. If you take this path, also update `WallPieceData.cs` enum + `WallPiece.cs` ApplyMetadata.

**Choice:** Use **Alternative (cleaner)** if you have time. Otherwise just zero the collider on both assets (simpler, faster).

**Verification:** In Unity Editor, select a door_arch prefab. Confirm BoxCollider2D size is (0,0) or disabled. Run a quick "Spawn DoorArch + Walk Through" mini-test in scene.

---

## BUG 4 — wpd_open_gap.asset has gameplay collider (BLOCKER!)

**Symptom:** Same as Bug 3. `colliderSize: {x: 1, y: 1}` blocks the "open" gap meant for player passage.

**File:** `Assets/ScriptableObjects/Walls/V2/wpd_open_gap.asset`

**Fix:** Same approach as Bug 3 (zero collider OR add hasCollider=false flag).

**Verification:** Same as Bug 3.

---

## BUG 5 — OnDrawGizmos color legend missing (WallChainRoomBuilder.cs:858-864)

**Symptom:** Current implementation:
```csharp
void OnDrawGizmos()
{
    if (roomParent == null) return;
    Gizmos.color = Color.white;
    foreach (Transform t in roomParent)
        Gizmos.DrawLine(t.position, t.position + Vector3.up * 0.1f);
}
```
This is JUST white tick marks. Blueprint spec REQUIRES color-coded legend:
- **Green:** walkable area cells
- **Red:** blocked footprint
- **Yellow:** wall chain
- **Purple:** door / entrance
- **Blue:** sockets / spawn points
- **Cyan:** low front / open edge
- **Orange:** connector / corner nodes

**Fix design:**
- Cache `lastSpec` (RoomSpec) and `lastFootprint` (HashSet<Vector2Int>) from the most recent `Build()` call as class fields. Persist them across OnDrawGizmos.
- For each spawned child under `roomParent`, dispatch to a color based on its `WallPieceType`:
  - Connector / OuterCorner / InnerCorner → orange
  - DoorArch → purple
  - LowFront / OpenGap → cyan
  - RearWall / SideWall (any length) → yellow (wall chain)
  - SocketMarker (if exists) → blue
- For walkable cells (green) and blocked (red), iterate the cached footprint and the spec's wall extents, drawing 0.9×0.9 wire cubes at each cell.
- Use `Gizmos.color` switching + `DrawWireCube` for cells, `DrawSphere(0.1f)` for sockets.

**Important — Unity safety:** Do NOT cache references to Editor-only objects in serialized fields. Use `[System.NonSerialized]` for the cached spec/footprint to survive domain reloads without errors. If cache is null after reload, skip gizmo draw silently.

**Verification:** Generate a room via RoomPainterWindow, click "Generate", then in SceneView confirm you see green walkable cells, yellow chain, purple door, orange corners/connectors. Take a screenshot saved as `STAGING/s106_overnight/stream_c_validation/gizmo_color_legend.png`.

---

## BUG 6 — Painter door save/load schema mismatch (RoomPainterWindow.cs)

**Symptom:**
- Line 849 SAVES door as flat array: `"door": [5,3]`
- Line 996-1024 `GetCellArray` PARSES nested arrays only: `[[x,y], [x,y], ...]`
- Result: Loading a saved layout with door silently drops the door (door reappears as not-set).

**File:** `Assets/Scripts/Editor/Walls/V2/RoomPainterWindow.cs`

**Fix:** Two options:

**Option A (recommended — local to LoadLayout):** Add a dedicated point-parser method `GetPointOrNull(string key)` that handles the flat `[x,y]` or `null` form. Use it ONLY for `door` field. Other fields (alcoves, protrusions, walkable) keep using `GetCellArray`.

```csharp
public Vector2Int? GetPointOrNull(string key)
{
    int i = FindKey(key);
    if (i < 0) return null;
    int open = s.IndexOf('[', i);
    // Detect "null" first
    int nullIdx = s.IndexOf("null", i, StringComparison.Ordinal);
    if (nullIdx > 0 && (open < 0 || nullIdx < open)) return null;
    if (open < 0) return null;
    int close = s.IndexOf(']', open);
    if (close < 0) return null;
    var inner = s.Substring(open + 1, close - open - 1);
    var parts = inner.Split(',');
    if (parts.Length >= 2
        && int.TryParse(parts[0].Trim(), out int x)
        && int.TryParse(parts[1].Trim(), out int y))
        return new Vector2Int(x, y);
    return null;
}
```

Then in load code, instead of `var doorList = parser.GetCellArray("door");`, use `doorCell = parser.GetPointOrNull("door");`.

**Option B (alternative):** Change SerializeLayout to write nested form `"door": [[5,3]]` or `"door": []`, and continue using GetCellArray which returns a list (take first if any).

**Choice:** **Option A** preserves wire compatibility with v2-saved files. Use it.

**Verification:** Save a layout in painter with door at (5,3). Reload it. Confirm doorCell == (5,3). Add a regression test note in `STAGING/s106_overnight/stream_c_validation/painter_load_door_test.md` describing the test steps.

---

## Procedure & Safety

1. **Backup first** — Before editing, copy the 4 source files to `Assets/_archive~/pre_s106_c_safety/`:
   - `WallChainRoomBuilder.cs`
   - `RoomPainterWindow.cs`
   - `wpd_door_arch.asset`
   - `wpd_open_gap.asset`

2. **Edit in surgical order** — Bug 1 → Bug 2 → Bug 5 → Bug 6 → Bug 3 → Bug 4 (assets last, easy to revert).

3. **Compile-check after EACH file edit** — Don't batch. Use UnityMCP `read_console` after each edit to confirm 0 error.

4. **No scene operations** during this pass — purely code/asset edits. No prefab regeneration. No scene loading.

5. **AssetDatabase.Refresh()** only at the end, ONCE, not per-edit (avoid domain reload loop).

---

## Output (mandatory format)

Write final report to `CODEX_DONE_<profile>.md` as:

```
# STREAM C P0 SAFETY — <profile> — 2026-05-25 03:XX

## STATUS: DONE | PARTIAL | FAILED

## Bug 1 (edge sort)
- Files: WallChainRoomBuilder.cs:275,286,296
- Diff:
  ```csharp
  <before/after>
  ```
- Verification: <what was tested>

## Bug 2 (length=2 corner suppression)
<same format>

## Bug 3 (door collider)
<same format>

## Bug 4 (open_gap collider)
<same format>

## Bug 5 (gizmo color legend)
<same format>
- Screenshot path: STAGING/s106_overnight/stream_c_validation/gizmo_color_legend.png

## Bug 6 (painter door save/load)
<same format>
- Regression test note path: STAGING/s106_overnight/stream_c_validation/painter_load_door_test.md

## Compile check
- Unity console errors: 0
- Warnings: <count>

## Files touched (final list)
- <full list>

## Time taken: <minutes>
```

If any bug is BLOCKED or PARTIAL, explain WHY in detail and stop — do not continue with the next bug. Wake-flag for orchestrator review.

---

**Estimated time:** 60-90 min. If you exceed 120 min, stop and write PARTIAL report with what's done.
