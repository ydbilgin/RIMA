# Codex Task — Sprint 7: Automation (Auto-Dress + Regenerate Decorative + Smart Fill)

**Type:** Implementation (higher-level automation orchestrators)
**Effort:** medium
**Estimated:** 1 day Codex
**Dispatch:** `python cx_dispatch.py --task-file STAGING/codex_brush_sprint7_automation.md --effort high`

---

## 0. MUST READ FIRST

1. `STAGING/map_designer_unified_brush_design.md` (§8 Automation Features especially)
2. `STAGING/codex_safety_review_output.md`
3. `STAGING/codex_brush_sprint2_executor_l3wall.md` (BrushAlongEdges pattern to reuse)
4. `STAGING/codex_brush_sprint4_decorative_executors.md` (decorative executors to call)
5. `STAGING/codex_brush_sprint6_brushpack_composite.md` (default brush pack to use)
6. `Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs` — LIVE room generator (RoomData source)

---

## 1. Context

Sprints 2–6 built the executors + UI. Sprint 7 wires them into one-button workflow accelerators. Three features:
1. **Auto-Dress Room** — given a RoomRecipe + Skin + seed, fully decorate the room
2. **Regenerate Decorative Layers** — preserve L1/L2, re-roll L3/L4/L5/L6 with new or same seed
3. **Smart Fill Selection** — rect-select an area, apply chosen brush inside the rect respecting all Karar #143 rules

All three operate as single Undo groups.

---

## 2. Scope — Files to Create

### 2.1 Editor asmdef files (`Assets/Scripts/MapDesigner/Brush/Automation/Editor/`)

All in namespace `RIMA.MapDesigner.Brush.Automation.Editor`. Asmdef: `RIMA.Editor`.

#### 2.1.1 `AutoDressRoom.cs`
```csharp
public static class AutoDressRoom {
    public static BrushExecutorResult Run(RoomRecipeSO recipe, BiomeSkinSO skin, int seed) {
        if (recipe == null || recipe.allowedBrushPack == null)
            return new BrushExecutorResult { success = false, errorMessage = "Recipe or BrushPack null" };

        var router = new BrushExecutorRouter();
        var composite = new CompositeStrokeExecutor();
        var compositionResult = new BrushExecutorResult { success = true, spawnedObjects = new() };

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName($"Auto-Dress: {recipe.name}");

        var room = LoadRoomData(recipe);  // existing API

        // 1. Wall pass: BrushAlongEdges with wall brush from pack
        var wallBrush = recipe.allowedBrushPack.brushes.FirstOrDefault(b => b.category == BrushCategory.Wall);
        if (wallBrush != null) {
            var wallResult = BrushAlongEdgesAutomation.Run(wallBrush, room, skin, seed);
            compositionResult.spawnedObjects.AddRange(wallResult.spawnedObjects);
        }

        // 2. Transition pass: for each transition brush in autoDressBrushes (if specified) or default subset
        var transitionBrushes = recipe.autoDressBrushes
            ?.Where(b => b.category == BrushCategory.Transition)
            ?? recipe.allowedBrushPack.brushes.Where(b => b.category == BrushCategory.Transition);

        foreach (var tBrush in transitionBrushes) {
            // Scatter across walkable cells with edge-bias
            ScatterAcrossRoom(tBrush, room, skin, seed, router, compositionResult);
        }

        // 3. Detail pass: same pattern with L5 brushes
        var detailBrushes = recipe.allowedBrushPack.brushes.Where(b => b.category == BrushCategory.Detail);
        foreach (var dBrush in detailBrushes) {
            ScatterAcrossRoom(dBrush, room, skin, seed, router, compositionResult);
        }

        // 4. Accent pass: L6 sparse (only if room flagged "rift_zone")
        if (IsRiftZone(recipe)) {
            var accentBrushes = recipe.allowedBrushPack.brushes.Where(b => b.category == BrushCategory.RiftAccent);
            foreach (var aBrush in accentBrushes) {
                ScatterAcrossRoom(aBrush, room, skin, seed, router, compositionResult);
            }
        }

        Undo.CollapseUndoOperations(group);
        return compositionResult;
    }

    private static void ScatterAcrossRoom(MapDesignerBrushPresetSO brush, RoomData room, BiomeSkinSO skin,
                                          int seed, BrushExecutorRouter router, BrushExecutorResult acc) {
        for (int x = 0; x < room.size.x; x++)
        for (int y = 0; y < room.size.y; y++) {
            var cell = new Vector2Int(x, y);
            foreach (var op in brush.operations) {
                float density = Karar143Enforcement.EffectiveDensity(cell, CellToWorld(cell), room, op);
                var rng = new System.Random(seed ^ (cell.x * 73) ^ (cell.y * 17) ^ op.targetLayer.GetHashCode());
                if (rng.NextDouble() > density) continue;

                var stroke = new BrushStroke { currentCell = cell, room = room, biomeSkin = skin,
                                                seed = seed + cell.x + cell.y * 1000 };
                var subResult = router.Dispatch(stroke, op, brush);
                if (subResult.spawnedObjects != null) acc.spawnedObjects.AddRange(subResult.spawnedObjects);
            }
        }
    }
}
```

#### 2.1.2 `RegenerateDecorativeLayers.cs`
```csharp
public static class RegenerateDecorativeLayers {
    public static BrushExecutorResult Run(RoomRecipeSO recipe, BiomeSkinSO skin, int newSeed) {
        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName($"Regenerate Decorative: seed {newSeed}");

        // 1. Clear L3/L4/L5/L6 parent GameObject children
        ClearLayerContainers(new[] { TargetLayer.L3, TargetLayer.L4, TargetLayer.L5, TargetLayer.L6 });

        // 2. Re-run auto-dress with new seed
        var result = AutoDressRoom.Run(recipe, skin, newSeed);

        Undo.CollapseUndoOperations(group);
        return result;
    }

    private static void ClearLayerContainers(TargetLayer[] layers) {
        foreach (var layer in layers) {
            var container = GameObject.Find($"Layer_{layer}");
            if (container == null) continue;
            for (int i = container.transform.childCount - 1; i >= 0; i--) {
                Undo.DestroyObjectImmediate(container.transform.GetChild(i).gameObject);
            }
        }
    }
}
```

#### 2.1.3 `SmartFillSelection.cs`
```csharp
public static class SmartFillSelection {
    public static BrushExecutorResult Run(RectInt selection, MapDesignerBrushPresetSO brush,
                                          RoomData room, BiomeSkinSO skin, int seed) {
        if (brush == null) return new BrushExecutorResult { success = false, errorMessage = "Brush null" };

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName($"Smart Fill: {brush.brushName}");

        var router = new BrushExecutorRouter();
        var result = new BrushExecutorResult { success = true, spawnedObjects = new() };
        var composite = brush.paintMode == PaintMode.CompositeStroke ? new CompositeStrokeExecutor() : null;

        for (int x = selection.xMin; x < selection.xMax; x++)
        for (int y = selection.yMin; y < selection.yMax; y++) {
            var cell = new Vector2Int(x, y);
            if (cell.x < 0 || cell.y < 0 || cell.x >= room.size.x || cell.y >= room.size.y) continue;

            var stroke = new BrushStroke { currentCell = cell, room = room, biomeSkin = skin,
                                            seed = seed + cell.x + cell.y * 1000 };

            if (composite != null) {
                var subResult = composite.ApplyComposite(stroke, brush, router);
                if (subResult.spawnedObjects != null) result.spawnedObjects.AddRange(subResult.spawnedObjects);
            } else {
                foreach (var op in brush.operations) {
                    float density = Karar143Enforcement.EffectiveDensity(cell, CellToWorld(cell), room, op);
                    var rng = new System.Random(stroke.seed ^ op.targetLayer.GetHashCode());
                    if (rng.NextDouble() > density) continue;
                    var subResult = router.Dispatch(stroke, op, brush);
                    if (subResult.spawnedObjects != null) result.spawnedObjects.AddRange(subResult.spawnedObjects);
                }
            }
        }

        Undo.CollapseUndoOperations(group);
        return result;
    }
}
```

### 2.2 UI integration (extend Sprint 5 window)

Add 3 buttons to `MapDesignerBrushWindow` bottom bar:
- `[Auto-Dress Room]` — calls `AutoDressRoom.Run(activeRoom, activeSkin, activeSeed)`
- `[Regenerate Decorative]` — calls `RegenerateDecorativeLayers.Run(activeRoom, activeSkin, newSeed)`
- `[Smart Fill]` — toggles SmartFill mode; user drags rect in scene, on release calls `SmartFillSelection.Run(...)`

Update existing `MapDesignerBrushWindow.cs` minimally — add bottom bar buttons + smart fill mode flag. Do NOT refactor existing window code.

### 2.3 EditMode tests under `Assets/Tests/EditMode/Brush/`

`BrushAutomationTests.cs` — minimum 6 cases:

1. **AutoDressRoom_PreservesL1L2** — before/after L1/L2 tilemap state identical
2. **AutoDressRoom_SpawnsWallsAlongPerimeter** — wallEdges.Count > 0 → spawnedObjects.Count >= wallEdges.Count - doorways
3. **AutoDressRoom_AvoidsNonWalkable** — set walkable[5,5]=false → no spawn at (5,5)
4. **RegenerateDecorative_DifferentSeedDifferentResult** — same room, seed=1 vs seed=2 → spawn count or positions differ
5. **RegenerateDecorative_PreservesTilemaps** — L1/L2 unchanged before/after
6. **SmartFill_StaysWithinSelection** — rect (10,10)–(20,20) → no spawn outside that rect

---

## 3. V1 EXCLUSIONS

- BiomeSkin live render swap during regenerate (Sprint 8)
- Sub-region biome blending (V2 only)
- Custom layout heuristics (e.g., "place 1 corruption blob at room center") — V2
- Auto-Dress progress bar UI (Sprint 8 polish or V2)
- Multi-room batch Auto-Dress (V2)

---

## 4. Acceptance Criteria

A. dotnet build both asmdefs pass 0 errors.
B. 6 EditMode tests PASS.
C. Auto-Dress preserves L1/L2 (TEST 1).
D. Auto-Dress respects walkable mask (TEST 3).
E. Regenerate produces different result for different seed (TEST 4).
F. Smart Fill stays within selection (TEST 6).
G. All three automations wrap in single Undo group (verify with `Undo.GetCurrentGroupName` after).
H. Sprint 1-6 tests still PASS.
I. No painter modifications.

---

## 5. Safety Rules

All previous rules apply. Key:

1. **Max 5 files per dispatch.** Sprint 7: 3 source + 1 test + UI extension to existing window = 5 → single dispatch OK if UI extension is minor.
2. **Per-cell loop discipline:** Auto-Dress iterates room.size.x * room.size.y cells. For a 50x50 room = 2500 iterations. Use `EditorUtility.DisplayProgressBar` to give the user feedback for >1s operations (optional polish; not blocking).
3. **Undo group discipline:** EVERY automation MUST `IncrementCurrentGroup`, `SetCurrentGroupName`, then `CollapseUndoOperations` at end.
4. **No `AssetDatabase.Refresh()`** — automation only mutates scene GameObjects, not assets.
5. **No painter modifications.**

---

## 6. Codex Self-Review Checklist

1. Read all 6 MUST READ files?
2. Does Auto-Dress preserve L1/L2 tilemap?
3. Does Auto-Dress call BrushAlongEdgesAutomation for walls (reuse, not reimplement)?
4. Does Regenerate clear ONLY L3/L4/L5/L6 parents?
5. Does Smart Fill respect selection rect bounds?
6. Are all three automations single Undo group?
7. Did both dotnet build pass?
8. Did 6 EditMode tests pass + previous sprint tests still pass?
9. Did I avoid painter modifications?
10. Did I add UI buttons to MapDesignerBrushWindow with minimal changes (no refactor)?

---

## 7. Output Format

Same CODEX_DONE.md structure as previous sprints.

---

## 8. Dependencies

**Blocked by:** Sprints 1–6 complete.
**Blocks:** Sprint 8 polish (BiomeSkin Auto-Dress preview).
