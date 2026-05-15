# Codex Task — Sprint 2: Executor Router + L3 Wall Executor + Brush Along Edges

**Type:** Implementation (paint dispatch infrastructure)
**Effort:** high
**Estimated:** 1.5 days Codex
**Dispatch:** `python cx_dispatch.py --task-file STAGING/codex_brush_sprint2_executor_l3wall.md --effort high`
**Output:** Code + EditMode tests + CODEX_DONE.md report

---

## 0. MUST READ FIRST

Before any code, read these files for full context:
1. `STAGING/map_designer_unified_brush_design.md` — full design spec (15 sections + 6 addendum)
2. `STAGING/codex_safety_review_output.md` — Unity safety contract (binding)
3. `STAGING/codex_brush_sprint1_data_layer.md` — Sprint 1 output (data types you will reference)
4. `Assets/Scripts/MapDesigner/WallOverlayPainter.cs` — the LIVE painter you will delegate to
5. `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs` — the LIVE orchestrator (for context)

---

## 1. Context

Sprint 1 delivered the data layer (SOs + enums + JSON round-trip). Sprint 2 adds the paint-dispatch infrastructure: the interface that defines "what an executor does," a router that maps `PaintMode → executor`, and the first two concrete executors (`GridTile` for L1/L2, `WallStamp` for L3). Plus the `BrushAlongEdges` automation that walks a room perimeter and auto-places wall sprites.

**Why this sprint matters:** L3 walls are the production gate. Without working wall placement, rooms do not visually close. Sprint 3 (PixelLab asset gen) runs in parallel with this sprint.

**Architectural rule (Karar #143 reuse):** The executors do NOT reimplement painter logic. They delegate to the existing LIVE painters (`WallOverlayPainter`, future `TransitionBrushPainter`, etc.). The brush tool is a thin UX + dispatch layer above stable painter code.

---

## 2. Scope — Files to Create

### 2.1 Runtime asmdef files (under `Assets/Scripts/MapDesigner/Brush/`)

All in namespace `RIMA.MapDesigner.Brush.Executors` (or sub-namespace as appropriate).

#### 2.1.1 `Stroke/BrushStroke.cs` — data carrier
```csharp
namespace RIMA.MapDesigner.Brush.Stroke;

[Serializable]
public struct BrushStroke {
    public Vector2 startPositionWorld;       // world-space click point
    public Vector2 currentPositionWorld;     // for drag strokes
    public Vector2Int startCell;             // tile-space
    public Vector2Int currentCell;
    public bool isDrag;                      // true if mouse drag, false if single click
    public int seed;                         // per-stroke seed (per-room default, override allowed)
    public RoomData room;                    // current room context (walkable mask, wallEdges)
    public BiomeSkinSO biomeSkin;            // active skin (for render rule lookup)
    public List<Vector2> strokePath;         // for ScatterAlongStroke; null/empty for click
}
```

**Note:** `RoomData` is the existing struct from `ProceduralRoomGenerator` (see `Assets/Scripts/MapDesigner/` LIVE code).

#### 2.1.2 `Executors/IBrushExecutor.cs` — interface
```csharp
namespace RIMA.MapDesigner.Brush.Executors;

public interface IBrushExecutor {
    PaintMode SupportedMode { get; }
    BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation op);
}

[Serializable]
public struct BrushExecutorResult {
    public bool success;
    public int spawnedCount;
    public List<GameObject> spawnedObjects;
    public List<UnityEngine.Object> modifiedAssets;
    public string errorMessage;
}
```

**Pure interface — no AssetDatabase, no UnityEditor. Editor-only concrete classes implement it in §2.2.**

### 2.2 Editor asmdef files (under `Assets/Scripts/MapDesigner/Brush/Executors/Editor/`)

All in namespace `RIMA.MapDesigner.Brush.Executors.Editor`. Asmdef: `RIMA.Editor` (existing — do NOT modify the .asmdef file).

#### 2.2.1 `BrushExecutorRouter.cs`
```csharp
public class BrushExecutorRouter {
    private readonly Dictionary<PaintMode, IBrushExecutor> registry = new();

    public BrushExecutorRouter() {
        Register(new GridTileExecutor());
        Register(new WallStampExecutor());
        // FreeformDecal, ScatterAlongStroke, Stamp, CompositeStroke,
        // EraseByLayer, EraseAllDecorative — registered by future sprints
    }

    public void Register(IBrushExecutor exec) {
        registry[exec.SupportedMode] = exec;
    }

    public BrushExecutorResult Dispatch(BrushStroke stroke, BrushLayerOperation op,
                                        MapDesignerBrushPresetSO preset) {
        if (op.assetPool == null)
            return new BrushExecutorResult { success = false, errorMessage = "AssetPool is null" };

        // Karar #143-D walkable mask check at router level (double-belt with executor)
        if (op.respectsWalkableMask && !IsCellWalkable(stroke.currentCell, stroke.room))
            return new BrushExecutorResult { success = true, spawnedCount = 0 };  // skip silently

        if (!registry.TryGetValue(preset.paintMode, out var exec))
            return new BrushExecutorResult { success = false, errorMessage = $"No executor for {preset.paintMode}" };

        return exec.Apply(stroke, op);
    }

    private bool IsCellWalkable(Vector2Int cell, RoomData room) {
        if (cell.x < 0 || cell.y < 0 || cell.x >= room.size.x || cell.y >= room.size.y)
            return false;
        return room.walkable[cell.x, cell.y];
    }
}
```

#### 2.2.2 `GridTileExecutor.cs`
- SupportedMode: `PaintMode.GridTile` (and `GridTileRandom` as secondary registration)
- Calls `Tilemap.SetTile(cell, tileRef)` for the target layer's tilemap
- Picks tile from `op.assetPool.tiles` (uniform pick for `GridTile`, weighted random for `GridTileRandom`)
- For L1/L2 paint:
  - Records `Undo.RegisterCompleteObjectUndo(tilemap, "Brush Grid Tile")`
  - SetTile
  - Returns result with `modifiedAssets = [tilemap]`

#### 2.2.3 `WallStampExecutor.cs`
- SupportedMode: `PaintMode.Stamp` (when target is L3)
- **Delegates to existing `WallOverlayPainter`** — does NOT reimplement wall logic
- Looks up nearest `WallSegment` in `stroke.room.wallEdges` for orientation hint
- Picks sprite from `op.assetPool.sprites` based on segment direction (horizontal/vertical/corner/doorway via `AssetCategory` tags)
- Instantiates SpriteRenderer GameObject as child of the active wall layer parent
- Records `Undo.RegisterCreatedObjectUndo(spawnedGO, "Brush Wall Stamp")`
- Returns result with `spawnedObjects = [the new GO]`

**Critical:** the existing `WallOverlayPainter.cs` likely has a `PlaceWallSprite(WallSegment seg, Sprite sprite, Transform parent)` method or similar. If yes, call it. If the method does not exist, ADD ONLY the minimum public method to `WallOverlayPainter` — but prefer to call existing methods first. Document any new method in CODEX_DONE.md "WallOverlayPainter changes" section.

#### 2.2.4 `Automation/Editor/BrushAlongEdgesAutomation.cs`
```csharp
public static class BrushAlongEdgesAutomation {
    public static BrushExecutorResult Run(MapDesignerBrushPresetSO wallBrush, RoomData room,
                                          BiomeSkinSO skin, int seed) {
        if (wallBrush.category != BrushCategory.Wall)
            return new BrushExecutorResult { success = false, errorMessage = "Brush is not Wall category" };

        var result = new BrushExecutorResult { success = true, spawnedObjects = new(), modifiedAssets = new() };
        var router = new BrushExecutorRouter();

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Brush Along Edges");

        foreach (var seg in room.wallEdges) {
            if (seg.isDoorway) continue;  // skip doorways for primary wall pass; separate pass for doorway gaps
            var stroke = new BrushStroke {
                currentCell = seg.start,
                room = room,
                biomeSkin = skin,
                seed = seed + seg.start.x * 73 + seg.start.y * 17,  // per-segment deterministic
            };
            foreach (var op in wallBrush.operations) {
                var subResult = router.Dispatch(stroke, op, wallBrush);
                if (subResult.spawnedObjects != null)
                    result.spawnedObjects.AddRange(subResult.spawnedObjects);
                result.spawnedCount += subResult.spawnedCount;
            }
        }

        Undo.CollapseUndoOperations(group);
        return result;
    }
}
```

### 2.3 EditMode tests under `Assets/Tests/EditMode/Brush/`

`BrushExecutorTests.cs` — minimum 6 cases:

1. **RouterDispatchesToCorrectExecutor** — register two executors, dispatch GridTile stroke → GridTileExecutor.Apply called
2. **RouterReturnsErrorForUnregisteredMode** — dispatch CompositeStroke (not registered) → returns success=false with error message
3. **RouterSkipsCellWhenWalkableMaskFails** — op.respectsWalkableMask=true, cell.walkable=false → spawnedCount==0, success=true (silent skip)
4. **GridTileExecutor_SetsTileOnTilemap** — mock RoomData with tilemap, dispatch → Tilemap.GetTile returns expected
5. **WallStampExecutor_DelegatesToWallOverlayPainter** — provide minimum WallSegment + AssetPool with 1 sprite → returns 1 spawned GO
6. **BrushAlongEdges_WalksAllSegments** — RoomData with 8 wall segments (1 of which isDoorway=true) → 7 segments processed, 1 skipped
7. **BrushAlongEdges_UndoCollapsesAsOneGroup** — perform automation → undo once → all walls reverted

Test asmdef: existing `Assets/Tests/EditMode/Brush/RIMA.Brush.Tests.asmdef` (created in Sprint 1).

---

## 3. V1 EXCLUSIONS — DO NOT IMPLEMENT

- Editor UI / EditorWindow code (Sprint 5)
- CompositeStrokeExecutor (Sprint 6)
- FreeformDecalExecutor, ScatterAlongStrokeExecutor (Sprint 4)
- EraseByLayer / EraseAllDecorative executors (Sprint 4 / 7)
- BiomeSkin render rules (Sprint 8)
- L4/L5/L6 actual sprite placement (Sprint 4 — only the interface exists in Sprint 2)
- PixelLab asset integration (Sprint 3 — wait for sprite import)
- Doorway-specific placement (Sprint 4 — Sprint 2 only handles wall segments where `isDoorway=false`)
- Hotkey handling (Sprint 5)
- Ghost preview (Sprint 5)
- Smart Fill (Sprint 7)
- Auto-Dress full implementation (Sprint 7 — only the `BrushAlongEdges` automation in Sprint 2)

---

## 4. Acceptance Criteria

A. `dotnet build RIMA.Runtime.csproj` returns **0 errors**.

B. `dotnet build RIMA.Editor.csproj` returns **0 errors** (regression — Sprint 2 adds Editor code).

C. All new files in correct namespace + asmdef:
   - Runtime: `RIMA.MapDesigner.Brush.Stroke`, `RIMA.MapDesigner.Brush.Executors` → `RIMA.Runtime`
   - Editor: `RIMA.MapDesigner.Brush.Executors.Editor`, `RIMA.MapDesigner.Brush.Automation.Editor` → `RIMA.Editor`

D. **DO NOT** create new asmdef files. Do not edit existing `.asmdef`.

E. **DO NOT** modify Karar #143 LIVE painters except to add ONE minimum public method to `WallOverlayPainter` if absolutely required. Document any change in CODEX_DONE.md. Prefer calling existing methods first.

F. EditMode test suite all PASS (6-7 cases).

G. Karar #143-D walkable mask enforcement test passes (TEST 3 above).

H. Sprint 1 sample assets still load without errors (regression check).

I. BrushAlongEdges Undo group test passes (TEST 7 above) — single undo restores all wall placements.

J. No file modified outside scope folders:
   - `Assets/Scripts/MapDesigner/Brush/Stroke/` (create)
   - `Assets/Scripts/MapDesigner/Brush/Executors/` (create runtime)
   - `Assets/Scripts/MapDesigner/Brush/Executors/Editor/` (create editor)
   - `Assets/Scripts/MapDesigner/Brush/Automation/Editor/` (create)
   - `Assets/Tests/EditMode/Brush/` (extend existing)
   - `Assets/Scripts/MapDesigner/WallOverlayPainter.cs` (only IF adding one minimum public method)

---

## 5. Safety Rules (binding — Codex safety review addendum applies)

All rules from `STAGING/codex_safety_review_output.md` apply. Key reminders:

1. **Read before Edit** on any file you reference.
2. **No destructive ops** (no `git reset --hard`, no `Library/` delete).
3. **Max 5 file per dispatch.** Sprint 2 has 6-7 files. Codex may split into 2 sub-dispatches:
   - Sub-dispatch 1: `BrushStroke.cs` + `IBrushExecutor.cs` + `GridTileExecutor.cs` + `BrushExecutorTests.cs` (start) — 4 files
   - Sub-dispatch 2: `BrushExecutorRouter.cs` + `WallStampExecutor.cs` + `BrushAlongEdgesAutomation.cs` + test extension — 3-4 files
4. **No asmdef changes.** Existing `RIMA.Runtime.asmdef` and `RIMA.Editor.asmdef` are off-limits.
5. **Undo discipline.** Every executor that mutates scene/asset MUST use Undo. Use the pattern from `codex_safety_review_output.md` §Q4 — `IncrementCurrentGroup` + `RegisterCompleteObjectUndo` / `RegisterCreatedObjectUndo` / `CollapseUndoOperations`.
6. **No `AssetDatabase.Refresh()`** unless writing files outside Unity (Sprint 2 does not).
7. **WallOverlayPainter change discipline:** Prefer using existing public methods. If you MUST add one method, add the minimum signature with no logic changes to existing methods. Document explicitly in CODEX_DONE.md.
8. **No commit.** Orchestrator commits after rima-qc review.
9. **Halt on ambiguity.** Document in CODEX_DONE.md "Open Questions" — do not pause to ask.

---

## 6. Codex Self-Review Checklist

Answer yes/no in CODEX_DONE.md. Any "no" needs explanation.

1. Did I read all 5 MUST READ files in §0?
2. Did I avoid reimplementing any logic that exists in `WallOverlayPainter`?
3. Did I add at most ONE new public method to `WallOverlayPainter` (and only if necessary)?
4. Is `IBrushExecutor` a pure interface with no `UnityEditor` namespace dependency?
5. Are concrete executors in `Editor/` folder under `RIMA.Editor` asmdef?
6. Does `BrushExecutorRouter.Dispatch` check `op.respectsWalkableMask` BEFORE calling the executor (double-belt with Karar #143-D)?
7. Does `BrushAlongEdges` skip `isDoorway=true` segments?
8. Does every executor that mutates scene/assets use `Undo.RegisterCreatedObjectUndo` or `Undo.RegisterCompleteObjectUndo`?
9. Does `BrushAlongEdges` wrap all stamps in one `IncrementCurrentGroup` + `CollapseUndoOperations`?
10. Did `dotnet build RIMA.Runtime.csproj` AND `dotnet build RIMA.Editor.csproj` both return 0 errors?
11. Did all 6-7 EditMode tests PASS?
12. Did I list all new `.meta` files (paths + GUIDs) in CODEX_DONE.md?
13. Did I respect the 5-file-per-dispatch limit (split if needed)?
14. Did I AVOID modifying any existing `.asmdef`?
15. Did I AVOID modifying Sprint 1 data layer files?

---

## 7. Output Format (CODEX_DONE.md)

```markdown
# Sprint 2 — Executor Router + L3 Wall + Brush Along Edges — Codex Report

## Files Created
[list with line counts]

## WallOverlayPainter Changes
[either "no changes" OR exact method signature added + line count]

## New .meta Files (GUID scan)
[path + GUID for each]

## Self-Review Checklist (1-15)
[yes/no + brief note each]

## Build Result
- `dotnet build RIMA.Runtime.csproj`: [PASS / FAIL + excerpt]
- `dotnet build RIMA.Editor.csproj`: [PASS / FAIL + excerpt]

## Test Results
EditMode tests: [PASS / FAIL count + names]

## Sub-dispatch Strategy
[either "single dispatch, X files" OR "split into 2 sub-dispatches: ..."]

## Open Questions / Deviations
[any spec ambiguity you resolved]

## Files Modified Outside Scope
[must be empty; if not, explain]
```

---

## 8. Estimated Effort

~10–12 hours of Codex time (high effort). Possibly 2 sub-dispatches due to 5-file limit. Orchestrator runs rima-qc after each sub-dispatch and again at sprint end.

---

## 9. Dependencies

**Blocked by:** Sprint 1 complete (data types referenced).
**Blocks:** Sprint 4 (L4/L5/L6 executors extend the router pattern), Sprint 5 (UI calls router), Sprint 6 (composite executor).
**Parallel with:** Sprint 3 (PixelLab asset gen) — no shared code.
