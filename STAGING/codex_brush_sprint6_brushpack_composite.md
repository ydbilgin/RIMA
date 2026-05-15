# Codex Task — Sprint 6: Default Brush Pack (8-12) + CompositeExecutor

**Type:** Implementation (composite executor + content authoring)
**Effort:** medium
**Estimated:** 1 day Codex
**Dispatch:** `python cx_dispatch.py --task-file STAGING/codex_brush_sprint6_brushpack_composite.md --effort high`
**Output:** Code + .asset files + EditMode tests + CODEX_DONE.md report

---

## 0. MUST READ FIRST

1. `STAGING/map_designer_unified_brush_design.md` (§6 Smart Composite Brush deep-dive especially)
2. `STAGING/codex_safety_review_output.md`
3. `STAGING/codex_brush_sprint1_data_layer.md` (data SOs)
4. `STAGING/codex_brush_sprint2_executor_l3wall.md` (router pattern)
5. `STAGING/codex_brush_sprint4_decorative_executors.md` (decorative executors to invoke)

---

## 1. Context

Sprints 2 and 4 added executors for individual paint modes. Sprint 6 adds the `CompositeStrokeExecutor` — the executor that gives the user the "painter feel" by triggering multiple `BrushLayerOperation` instances per stroke. Plus the 8-12 default brush pack the user sees in the palette.

**Composite rule (LOCKED V1):** flat only. No nested composites. A `BrushLayerOperation` never references another `MapDesignerBrushPresetSO`.

---

## 2. Scope — Files to Create

### 2.1 Editor asmdef — CompositeStrokeExecutor

#### 2.1.1 `Assets/Scripts/MapDesigner/Brush/Executors/Editor/CompositeStrokeExecutor.cs`

```csharp
public class CompositeStrokeExecutor : IBrushExecutor {
    public PaintMode SupportedMode => PaintMode.CompositeStroke;

    public BrushExecutorResult Apply(BrushStroke stroke, BrushLayerOperation primaryOp) {
        // CompositeStroke uses preset.operations list directly, not the single 'primaryOp' param.
        // Router passes the first op; executor reads preset from stroke context (separate field) OR
        // we extend the interface to pass preset.
        // Cleanest: add a second method signature. See §2.1.2.
    }

    public BrushExecutorResult ApplyComposite(BrushStroke stroke, MapDesignerBrushPresetSO preset,
                                              BrushExecutorRouter router) {
        if (preset.paintMode != PaintMode.CompositeStroke) {
            return new BrushExecutorResult { success = false, errorMessage = "Brush is not CompositeStroke" };
        }
        if (preset.operations == null || preset.operations.Count == 0) {
            return new BrushExecutorResult { success = false, errorMessage = "No operations defined" };
        }

        var result = new BrushExecutorResult { success = true, spawnedObjects = new(), modifiedAssets = new() };
        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName($"Composite Brush: {preset.brushName}");

        foreach (var op in preset.operations) {
            // Probability gate
            var rng = new System.Random(stroke.seed ^ (op.targetLayer.GetHashCode() << 8));
            if (rng.NextDouble() > op.probability) continue;

            // Re-dispatch each op via router with sub-mode based on the op's paint context
            // For composite, the sub-mode for each operation is implicit: L1/L2 ops use GridTile,
            // L3 ops use Stamp, L4/L5/L6 ops use FreeformDecal (or ScatterAlongStroke if op specifies).
            // We need a small mapping helper.
            var subMode = MapOpToSubMode(op);
            var subStroke = stroke;  // copy
            var subResult = router.DispatchWithMode(subStroke, op, subMode);

            if (subResult.spawnedObjects != null) result.spawnedObjects.AddRange(subResult.spawnedObjects);
            result.spawnedCount += subResult.spawnedCount;
        }

        Undo.CollapseUndoOperations(group);
        return result;
    }

    private PaintMode MapOpToSubMode(BrushLayerOperation op) {
        return op.targetLayer switch {
            TargetLayer.L1 => PaintMode.GridTile,
            TargetLayer.L2 => PaintMode.GridTileRandom,
            TargetLayer.L3 => PaintMode.Stamp,
            TargetLayer.L4 => PaintMode.FreeformDecal,
            TargetLayer.L5 => PaintMode.FreeformDecal,
            TargetLayer.L6 => PaintMode.FreeformDecal,
            _ => PaintMode.FreeformDecal
        };
    }
}
```

#### 2.1.2 Extend `BrushExecutorRouter` (add one method)

Add to existing router (created in Sprint 2):
```csharp
public BrushExecutorResult DispatchWithMode(BrushStroke stroke, BrushLayerOperation op, PaintMode mode) {
    if (op.assetPool == null)
        return new BrushExecutorResult { success = false, errorMessage = "AssetPool is null" };

    if (op.respectsWalkableMask && !IsCellWalkable(stroke.currentCell, stroke.room))
        return new BrushExecutorResult { success = true, spawnedCount = 0 };

    if (!registry.TryGetValue(mode, out var exec))
        return new BrushExecutorResult { success = false, errorMessage = $"No executor for {mode}" };

    return exec.Apply(stroke, op);
}
```

This is a 1-method addition to an existing file. Document in CODEX_DONE.md.

### 2.2 Default Brush Pack .asset files under `Assets/Data/Brush/Default/`

These are SHIPPED with V1 — the user-facing palette out of the box.

#### 2.2.1 8-12 brush .asset files

| # | Brush Name | Category | PaintMode | Ops | Notes |
|---|---|---|---|---|---|
| 1 | Clean Stone Floor | Floor | GridTile | 1 (L1) | base floor |
| 2 | Dark Variation | Variation | GridTileRandom | 1 (L2) | sparse darker tiles |
| 3 | Hades Wall Cap | Wall | Stamp | 1 (L3) | uses L3 wall asset pool |
| 4 | Moss Soft Oval | Transition | FreeformDecal | 1 (L4) | uses moss pool |
| 5 | Dirt Patch | Transition | FreeformDecal | 1 (L4) | uses dirt pool |
| 6 | Crack Scatter | Detail | ScatterAlongStroke | 1 (L5) | uses crack pool, minDist 32 |
| 7 | Rubble Cluster | Detail | Stamp | 1 (L5) | uses rubble pool |
| 8 | Rift Scar | RiftAccent | FreeformDecal | 1 (L6) | uses rift fracture pool, low density 0.08 |
| 9 | Mossy Broken Edge | Composite | CompositeStroke | 3 (L2+L4+L5) | classic composite |
| 10 | Rift-Damaged Corner | Composite | CompositeStroke | 4 (L2+L4+L5+L6) | story corruption |
| 11 | Edge Trim | Composite | CompositeStroke | 2 (L4+L5) | dirt path between encounters |
| 12 | Battle Aftermath | Composite | CompositeStroke | 3 (L4+L5+L6 sparse) | post-combat dressing |

**Each brush .asset file:**
- Located at `Assets/Data/Brush/Default/Brush_<Name>.asset`
- All fields populated
- `previewIcon` set to a sample sprite from the pool (or null — Sprint 8 generates real icons)
- `hotkeyIndex` assigned for top 9: brushes 1-9 get hotkey 1-9; brushes 10-12 no hotkey
- `respectsWalkableMask` = true for all decorative ops
- `wallProximityCurve` for L4/L5 ops uses edge-biased preset (e.g., linear 0→1.0, 1→0.6, 2→0.3, 3→0.1)

#### 2.2.2 Default BrushPack .asset

`Assets/Data/Brush/Default/BrushPack_ShatteredKeep_Default.asset`:
- packName: "ShatteredKeep Default"
- version: "1.0"
- brushes: list of all 8-12 above
- referencedPools: list of all AssetPool references (Floor, Walls, Moss, Dirt, Crack, Rubble, RiftFracture, RiftCorruption)
- coverImage: null (Sprint 8 may generate)

### 2.3 EditMode tests

`BrushCompositeTests.cs` — minimum 5 cases:

1. **Composite_FiresAllOperations** — composite brush with 3 ops, all probability=1 → 3 sub-dispatches occurred
2. **Composite_RespectsProbabilityGate** — op with probability=0 → never fires; probability=1 → always fires
3. **Composite_SingleUndoGroup** — composite stroke spawns 3 GOs, Ctrl+Z → all 3 reverted in single undo
4. **DefaultPack_AllBrushesValid** — load BrushPack_ShatteredKeep_Default → every brush has at least 1 operation, every operation has non-null AssetPool ref (or empty list of operations rejected)
5. **DefaultPack_HotkeyUniqueness** — no two brushes share the same `hotkeyIndex != -1`

---

## 3. V1 EXCLUSIONS

- Nested composites (BrushLayerOperation referring to another BrushPreset)
- Brush thumbnail generation (Sprint 8 or manual)
- Brush pack import from JSON file (Sprint 8 or V2 — Sprint 6 only creates the default pack as .asset)
- BiomeSkin variant per brush (Sprint 8)
- Composite branching logic (e.g., "if cell is corner do X else Y")

---

## 4. Acceptance Criteria

A. `dotnet build RIMA.Runtime.csproj` AND `dotnet build RIMA.Editor.csproj` both return 0 errors.
B. 8-12 brush .asset files exist under `Assets/Data/Brush/Default/`.
C. 1 BrushPack .asset references all default brushes.
D. CompositeStrokeExecutor registered in router and fires.
E. Composite stroke produces single Undo group (TEST 3).
F. All 5 EditMode tests PASS.
G. Sprint 1-5 tests still PASS (regression).
H. `BrushExecutorRouter.DispatchWithMode` is the only addition to Sprint 2 file (no other changes).
I. No new asmdef. No painter modifications.

---

## 5. Safety Rules

All previous rules apply. Key:

1. Max 5 files per dispatch. Sprint 6: 1 executor + 12 .asset + 1 .asset (pack) + 1 test = many .asset files. Use AssetDatabase batch (`StartAssetEditing`/`StopAssetEditing` per Codex safety §Q1).
2. **All .asset creation in one batch** with single `SaveAssets()` at end.
3. **No `AssetDatabase.Refresh()`** during .asset batch (no external file IO).
4. Codex may split into 2-3 sub-dispatches: (a) executor + 4 brushes + test, (b) 4 brushes + composite brushes + pack, (c) test extension + pack.

---

## 6. Codex Self-Review Checklist

1. Read all 5 MUST READ files in §0?
2. Did CompositeStrokeExecutor use `Undo.IncrementCurrentGroup` + `CollapseUndoOperations`?
3. Does the router add only ONE method (`DispatchWithMode`) with no logic changes to existing methods?
4. Are all 8-12 default brush .asset files created with non-null AssetPool refs (or explicitly null with explanation)?
5. Are hotkey indices unique?
6. Did I respect probability gate in composite execution?
7. Did all .asset creation happen in single `StartAssetEditing`/`StopAssetEditing` block?
8. Did both `dotnet build` commands pass?
9. Did all 5 new + previous sprint tests PASS?
10. Did I avoid nested composites?
11. Did I list all new .meta files?

---

## 7. Output Format

Same as previous sprint CODEX_DONE.md.

---

## 8. Dependencies

**Blocked by:** Sprints 1, 2, 4 complete (data + executors).
**Blocks:** Sprint 7 (Auto-Dress uses default pack), Sprint 8 (BiomeSkin renders default pack brushes).
