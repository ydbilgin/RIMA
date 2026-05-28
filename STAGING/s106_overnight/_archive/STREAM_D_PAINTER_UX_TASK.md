ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Otherwise read-only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / STAGING/s106_overnight/MASTER_CONTEXT.md / STAGING/s106_overnight/MASTER_PLAN.md / Assets/Scripts/Runtime/Walls/V2/**/*.cs / Assets/Scripts/Editor/Walls/V2/**/*.cs / Assets/ScriptableObjects/Walls/V2/*.asset / STAGING/concepts/chatgpt_ref/blueprint_room/*.png

Amaç: RoomPainterWindow'u user'ın "dünyanın en kolay kullanımı olan toolu" hedefine doğru cerrahi olarak ilerlet. User kendi sözleriyle: "ben oraya blueprint çizeyim otomatik duvarları entegre etsin, saçma sapan etmesin birbirine bağlayacak şekilde, 2D box'ları ayarlı şekilde olacak". 5 test room presetinin tek tıkla çalışması + live preview + validation + auto-clean öncelik. NO UI Toolkit migration — keep IMGUI base, enhance.

---

# STREAM D — PAINTER UX OVERHAUL (USER #1 PRIORITY)

## Dependencies (BLOCKING — verify before starting)

Stream C P0 SAFETY must be DONE first (specifically Bug 6 — painter door save/load schema). Check `CODEX_DONE_<profile>.md` for STATUS: DONE and the Bug 6 section before touching RoomPainterWindow.cs.

If Stream C is not done → BLOCKED, write WAITING entry and stop.

## Files in scope (edit only these)
- `Assets/Scripts/Editor/Walls/V2/RoomPainterWindow.cs` (main painter, ~1100 lines)
- `Assets/Scripts/Runtime/Walls/V2/RoomSpec.cs` (add socket schema — see P0.6)
- `Assets/Scripts/Editor/Walls/V2/PainterValidator.cs` (NEW — validation logic, ~150 lines)
- `Assets/Scripts/Runtime/Walls/V2/RoomDebugGizmo.cs` (NEW — color-coded gizmo component, ~120 lines)

Touch nothing else unless absolutely required for compilation.

---

## Priority 0 Features (MUST HAVE — all needed for "world's easiest" claim)

### P0.1 — Live Geometry Preview (`Generate Preview` toggle)

**Behavior:** User paints walkable cells, toggles a "Live Preview" checkbox. Painter draws (via IMGUI overlay on the canvas, NOT spawning Unity objects):
- Rear wall span outlines (yellow)
- Side wall span outlines (yellow)
- Low front + open gap outlines (cyan)
- Corner positions (orange)
- Door cell (purple)
- Each placed piece's COLLIDER footprint (green wire box) — so user sees what's passable

**Implementation:**
- Add `bool livePreview = true;` field with toggle in toolbar
- New method `RecomputePreview()` that runs the same edge-extraction logic as `WallChainRoomBuilder` (extract rear/side/front edges, group, predict piece placement) but does NOT spawn GameObjects — returns a list of `PreviewPiece { Vector2Int cell; WallPieceType type; WallDirection dir; Vector2 colliderSize; }`
- In `DrawCanvas()`, after the cell painting overlay, iterate `previewPieces` and draw colored outline/symbol per type
- Recompute on cell change, not every OnGUI tick (use a `bool previewDirty` flag)

**Reuse:** Extract the edge logic from WallChainRoomBuilder into a static helper `WallChainPredictor.PredictPieces(RoomSpec, HashSet<Vector2Int>) → List<PreviewPiece>` so painter doesn't depend on a live MonoBehaviour spawn. Builder uses the SAME predictor internally to ensure preview matches actual generation.

### P0.2 — Validation Panel

**File:** New `PainterValidator.cs` (static class)

```csharp
public static class PainterValidator {
    public enum Severity { Info, Warning, Error }
    public struct Issue { public Severity sev; public string code; public string message; public Vector2Int? cell; }

    public static List<Issue> Validate(RoomSpec spec, HashSet<Vector2Int> walkable, Vector2Int? door,
                                       List<RectInt> water, List<RectInt> islands, List<RoomSocket> sockets);
}
```

**Required checks (all P0):**
- E001 PlayerTrapped — no door/opening AND no front opening
- E002 NarrowCorridor — connected component bottleneck of 1 cell width between two areas ≥4 cells (BFS)
- E003 InvalidDoor — door cell exists but not adjacent to a wall edge (rear or front)
- E004 WaterCrossesWalkable — a waterPoolRect overlaps a walkable cell (semantic conflict)
- E005 OrphanCell — single walkable cell with no walkable neighbors
- W101 DisconnectedRegion — walkable area has ≥2 disconnected components (BFS island count)
- W102 SeamGap — predicted corner has no corresponding registry piece (WallPieceRegistry lookup)
- W103 EmptyPaint — gridWidth × gridHeight has 0 walkable cells
- I201 SizeRecommend — if footprint < 6×6, suggest "larger for combat"

**Display:** New panel in painter window (collapsible "Validation" section):
- Each issue: icon (i / ⚠ / ✖), code, message, "[Jump]" button if cell present
- "Auto-Clean" button at top of panel — runs P0.3 fixes for autofixable issues

### P0.3 — Auto-Clean / Normalize Footprint

**Button:** "Auto-Clean" in toolbar AND in Validation panel.

**Behavior:**
- **Normalize origin:** Find minX/minY of walkable cells, shift everything by `(-minX, -minY)` so layout starts at (0,0). Shift door/alcove/protrusion/water/islands/sockets identically.
- **Remove orphan cells:** Single walkable cells with no walkable neighbors → erase (or warn if part of intentional island).
- **Merge adjacent alcoves:** Detect connected alcove cell groups, replace individual NicheSpec entries with a single grouped one (width × depth).
- **Enforce front opening:** If FrontEdgeMode = Open or LowWall AND no front gap is paintable, log a warning (don't auto-add).
- **Snap to grid:** No-op for now (canvas IS grid).

**Verification:** After Auto-Clean, re-run Validation. Errors should drop. Add unit-test-like assertion comment in code.

### P0.4 — Five One-Click Templates (including Boss Arena — currently missing!)

**Existing presets (lines ~188-200, current state):** Basic Combat, Stepped Diamond, Library, Flooded. Boss Arena missing.

**Required revised presets (replace OR add — ADIM 4 + ADIM 5 inspired):**

| Preset Id | Footprint | Door | Special |
|---|---|---|---|
| `combat_basic` | 14×12 rectangle | rear center (cell (7, 11)) | 4 enemy spawn sockets at corners-of-inner, 2 prop sockets along rear |
| `ritual_diamond` | 13×13 stepped (top width 7, expand to 13 over 4 rows, contract back over 4 rows) | rear center | 1 ritual socket at center, 4 cyan rift sockets at 4 corners, symmetric enemy spawn |
| `flooded_crypt` | 14×11 with 2 water reserved rects (3×3 at left and right thirds), interior walkable platforms | rear center | 2 sarcophagus sockets along rear, 1 altar at center walkable island |
| `library_alcove` | 11×11 (per ADIM 4) with 2 grouped alcoves (mouth=2, depth=2) on left side, 1 alcove on right | rear center | 4 bookshelf sockets along rear + alcove walls, 1 desk socket front-center |
| `boss_arena` | 18×14 with rear setpiece extending +2 cells back, stepped sides (3 steps each) | front center (player entrance) + rear center (boss gate, optional second door) | 1 boss spawn at center, 4 wave-spawn sockets on side mid-points |

**Implementation:** Each preset = a method that fills the cell grid + sets door + alcoves + water + sockets in current state. NO scene operation, just paint. User can then click "Generate Room" to spawn.

**UI:** Replace single dropdown with 5-button row at top of window: `[Combat] [Ritual] [Flooded] [Library] [Boss]`. Click = paint preset onto canvas. Highlight active preset button.

### P0.5 — Brush Tool Modes

Current modes (existing): Walkable, Erase, Door, Alcove, Protrusion. Add:
- **Water** — paints a rect (drag), adds to `waterPools` list. Auto-marked non-walkable.
- **Island** — paints a rect, adds to `interiorIslands`. Auto-marked non-walkable.
- **PropSocket** — paints a single cell with sub-type dropdown (Torch / Banner / Bookshelf / Sarcophagus / Altar / Crystal / Cage)
- **EnemySpawn** — single cell, enemy type dropdown (Melee / Ranged / Boss / Wave / Elite)
- **ObjectiveSocket** — single cell, sub-type (Door / Exit / Chest / Trigger / Ritual / Portal)

Toolbar layout: row of icon buttons, hotkeys (W/E/D/A/P/T/I/S/N/O) for fast switching.

### P0.6 — RoomSpec Schema: Add Sockets

Edit `Assets/Scripts/Runtime/Walls/V2/RoomSpec.cs` to add:

```csharp
public enum SocketType { Torch, Banner, Bookshelf, Sarcophagus, Altar, Crystal, Cage,
                         EnemyMelee, EnemyRanged, EnemyElite, EnemyBoss, EnemyWave,
                         ObjectiveDoor, ObjectiveExit, ObjectiveChest, ObjectiveTrigger, ObjectiveRitual, ObjectivePortal }

[Serializable]
public struct RoomSocket {
    public Vector2Int cell;
    public SocketType type;
    public string metadata;  // optional free-form (spawn rate, item id, etc.)
}

// Add to RoomSpec:
public List<RoomSocket> sockets = new List<RoomSocket>();
```

Backwards compat: existing rooms without sockets work fine (empty list).

### P0.7 — Door Mode Toggle (User-painted vs Centered)

Current `enforceCenteredRearDoor = true` always overrides user-painted door. Add toggle in painter:
- **Door Position:** [Centered] [User-Painted]
- When User-Painted: store door at exact painted cell, builder respects it (need to also update WallChainRoomBuilder.cs to honor non-centered door — see P0.7 secondary fix)

**Secondary fix in WallChainRoomBuilder.cs:356-371:** Honor `spec.enforceCenteredRearDoor` flag. If false, use `spec.doorPosition` directly without recentering.

Add `bool enforceCenteredRearDoor;` field to `RoomSpec` (already exists per line 25).

### P0.8 — Save/Load Schema Preservation (water/islands/sockets)

Current SerializeLayout includes walkable, alcoves, protrusions, water, islands. Add `sockets` field. Bump schema version to 3 (back-compat: v2 loader works, sockets default empty).

Also fix Bug 6 (Stream C may have done this — if so, skip).

---

## Priority 1 (NICE-TO-HAVE — implement if time)

### P1.1 — Undo/Redo (ScriptableObject + native Undo)

Per Antigravity research recommendation. Replace `CellState[,] cells` raw field with a wrapper ScriptableObject that participates in Unity's undo stack via `Undo.RegisterCompleteObjectUndo`. Listen to `Undo.undoRedoPerformed` to re-render canvas.

### P1.2 — Brush Size + Line Tool + Mirror + Flood Fill

- **Brush size 1-4** dropdown — affects walkable/erase/alcove/protrusion modes
- **Line tool** — click cell A, hold Shift, click cell B, paints rectangle/line between
- **Mirror mode** — toggle that mirrors paint operations across vertical center (for symmetric Ritual/Boss layouts)
- **Flood fill** — bucket icon, click a cell, fills the connected empty region with current brush

### P1.3 — One-Click Proof Export

Button: "Export Proof". Generates:
- Screenshot of current scene (uses `Camera.main.Render()` or SceneView screenshot)
- Screenshot of debug gizmos (separate frame)
- Asset usage list (which WallPieceData IDs were spawned, count each)
- Missing asset list (any GetSpanForLength returned null)
- Save all to `STAGING/s106_overnight/stream_e_rooms/<roomName>/` with timestamp

### P1.4 — Asset Dressing Toggle

Top toolbar checkbox: "Use Real Assets" (default unchecked = placeholders). When checked, WallChainRoomBuilder uses real-sprite prefabs from the wpd_*.asset entries instead of placeholder colors. Currently placeholders are color-tinted cubes; real assets need PNG sprite. After Stream B taxonomy, real assets will be classified — until then this checkbox is gray/disabled.

---

## Architecture notes

### Hybrid IMGUI — DO NOT migrate to UI Toolkit
The existing painter uses pure IMGUI EditorWindow. UI Toolkit migration is a separate ~3-hour refactor not in scope tonight. Keep IMGUI; add new features within IMGUI patterns.

### Live preview pattern
- `RecomputePreview()` runs on cell-change (set `previewDirty = true`)
- `OnGUI` checks `previewDirty`, if true runs recompute (max once per frame)
- Preview drawing is overlay on the existing cell canvas, NOT a separate window

### Validation pattern
- Validate runs on demand (button click) AND on cell-change (debounced 250ms)
- Issues cached, panel renders cached list
- Jump-to-cell button scrolls canvas + highlights target cell briefly

### RoomSocket painting
- Each socket sub-mode shows a sub-toolbar with type dropdown
- Paint = click cell → adds to `sockets` list
- Visual on canvas: small colored icon overlay per socket type (text label optional)

---

## Procedure

1. **Verify Stream C done** — read `CODEX_DONE_<profile>.md`, confirm Bug 6 fixed. If not, BLOCKED.
2. **Backup current painter** → `Assets/_archive~/pre_s106_d_painter/RoomPainterWindow.cs`
3. **Implement P0 in order:** P0.6 (schema) → P0.5 (brushes use schema) → P0.4 (presets use brushes) → P0.7 (door toggle) → P0.8 (save/load) → P0.2 (validation) → P0.3 (auto-clean) → P0.1 (live preview) — preview last because it's largest
4. **Compile-check after each P0 item** (UnityMCP read_console). If error, STOP and fix before next.
5. **Implement P1 items only if time** (look at clock, hard stop at 06:00).
6. **Manual test scenario:** Open painter, click each of 5 presets, click Auto-Clean, click Generate. Verify no console errors, room spawns, gizmos colored. Take screenshot.
7. **Output report:** `CODEX_DONE_<profile>.md` with:
   - STATUS: DONE / PARTIAL / FAILED
   - Per-P0 item: implementation summary + line numbers + verification
   - Per-P1 item: same OR "SKIPPED (time)"
   - Files touched (full list)
   - Compile check: errors=0, warnings=N
   - Manual test result: each preset → spawned cleanly y/n
   - Screenshots saved (paths)
   - Time taken

---

## Safety constraints (HARD)

- ❌ NO Unity crash: AssetDatabase batch with try/finally for all writes
- ❌ NO scene operations during code editing
- ❌ NO UI Toolkit migration
- ❌ NO refactor outside P0/P1 scope
- ✅ Compile-check after each file edit
- ✅ Use `EditorUtility.DisplayProgressBar` for long ops (preset paint, validate, auto-clean) so user sees progress

## Estimated time: 120-180 min total (P0 only). Time-box at 06:00.
