# Codex Task — Sprint 4: L4+L5+L6 Executors + Karar #143 Enforcement

**Type:** Implementation (decorative paint executors)
**Effort:** high
**Estimated:** 1.5 days Codex
**Dispatch:** `python cx_dispatch.py --task-file STAGING/codex_brush_sprint4_decorative_executors.md --effort high`
**Output:** Code + EditMode tests + CODEX_DONE.md report

---

## 0. MUST READ FIRST

1. `STAGING/map_designer_unified_brush_design.md` — full design spec
2. `STAGING/codex_safety_review_output.md` — Unity safety contract
3. `STAGING/codex_brush_sprint1_data_layer.md` — Sprint 1 data layer
4. `STAGING/codex_brush_sprint2_executor_l3wall.md` — Sprint 2 executor router pattern (reference)
5. `Assets/Scripts/MapDesigner/TransitionBrushPainter.cs` — LIVE painter to delegate to
6. `Assets/Scripts/MapDesigner/DetailDecalPainter.cs` — LIVE painter to delegate to
7. `Assets/Scripts/MapDesigner/AccentPainter.cs` — LIVE painter to delegate to
8. `Assets/Scripts/Data/FeatureMaskSO.cs` — LIVE FeatureMask type

---

## 1. Context

Sprint 2 delivered the executor router + GridTile + Wall executors. Sprint 4 adds the three decorative executors (L4/L5/L6) and the full Karar #143-D/E/K enforcement chain.

**Painters that must NOT be modified:** `TransitionBrushPainter`, `DetailDecalPainter`, `AccentPainter`, `NaturalFeatureGraph`, `FeatureEdgeSmoothingPass`. The new executors call existing public methods on these classes.

**Karar #143 enforcement:**
- **D** Walkable mask filter — every decorative cell receives a check, skips if `walkable[x,y] == false`
- **E** Edge-biased density — `wallProximityCurve.Evaluate(distanceToNearestWall)` multiplies base density
- **K** Feature mask multiplier — if `op.featureMaskMultiplier != null`, sample texture alpha at cell, multiply density

These checks happen INSIDE the executor, BEFORE the painter is called.

---

## 2. Scope — Files to Create

### 2.1 Editor asmdef files (`Assets/Scripts/MapDesigner/Brush/Executors/Editor/`)

All in namespace `RIMA.MapDesigner.Brush.Executors.Editor`. Asmdef: `RIMA.Editor` (existing).

#### 2.1.1 `FreeformDecalExecutor.cs`
- SupportedMode: `PaintMode.FreeformDecal`
- For single click → places one decal at click position
- Delegates to `TransitionBrushPainter.PlaceAt(pos, sprite, parent, seed)` for L4
- Delegates to `DetailDecalPainter.PlaceAt(pos, sprite, parent, seed)` for L5
- Delegates to `AccentPainter.PlaceAt(pos, sprite, parent, seed)` for L6
- Asset pick: random sprite from `op.assetPool.sprites` with weights (if `op.assetPool.spriteWeights` non-empty)
- Random rotation/flip/scale per `op.allowRotation/allowFlipX/allowFlipY/scaleRange`
- Uses `Undo.RegisterCreatedObjectUndo(spawnedGO, "Brush Decal Stamp")`

#### 2.1.2 `ScatterAlongStrokeExecutor.cs`
- SupportedMode: `PaintMode.ScatterAlongStroke`
- For drag stroke → walks `stroke.strokePath` and scatters decals at intervals
- Respects `op.minDistance` (Bridson-style; reject candidates closer than minDistance to existing)
- Density-driven sample count: `pathLength * op.density / op.minDistance`
- Per-cell walkable + edge-bias + feature mask filter (Karar #143-D/E/K)
- Same delegation pattern as FreeformDecal

#### 2.1.3 `StampExecutor.cs`
- SupportedMode: `PaintMode.Stamp`
- Single click → one decal at click position with weight-picked sprite
- Used by composite operations when a layer needs exactly one decal (no scatter)
- Same delegation pattern

#### 2.1.4 `EraseByLayerExecutor.cs`
- SupportedMode: `PaintMode.EraseByLayer`
- Removes all GameObjects in the target layer's container that intersect a small radius around the click position
- Uses `Undo.DestroyObjectImmediate` per removed GO
- Wraps all destroys in `IncrementCurrentGroup`/`CollapseUndoOperations`

#### 2.1.5 `EraseAllDecorativeExecutor.cs`
- SupportedMode: `PaintMode.EraseAllDecorative`
- Removes all GameObjects under L3/L4/L5/L6 parent containers (preserves L1/L2 Tilemap)
- Whole-room operation (no per-cell radius)
- Single Undo group

### 2.2 Karar #143 enforcement helper (Runtime asmdef OR shared with Editor)

#### 2.2.1 `Karar143Enforcement.cs`
Location: `Assets/Scripts/MapDesigner/Brush/Executors/Karar143Enforcement.cs` (Runtime).
Namespace: `RIMA.MapDesigner.Brush.Executors`.

```csharp
public static class Karar143Enforcement {
    /// <summary>Karar #143-D: per-cell walkable check.</summary>
    public static bool IsCellWalkable(Vector2Int cell, RoomData room) { ... }

    /// <summary>Karar #143-E: edge-biased density curve evaluation.</summary>
    public static float ComputeWallProximityMultiplier(Vector2Int cell, RoomData room, AnimationCurve curve) {
        int distToWall = ComputeDistanceToNearestWall(cell, room);
        return curve.Evaluate(distToWall);
    }

    /// <summary>Karar #143-K: optional feature mask texture sample.</summary>
    public static float SampleFeatureMask(Vector2 worldPos, RoomData room, FeatureMaskSO mask) {
        if (mask == null || mask.alphaTexture == null) return 1f;
        // ... bilinear sample texture at normalized room coords
        return mask.remap.Evaluate(sampledAlpha);
    }

    /// <summary>Combined density evaluation for executors.</summary>
    public static float EffectiveDensity(Vector2Int cell, Vector2 worldPos, RoomData room,
                                         BrushLayerOperation op) {
        if (op.respectsWalkableMask && !IsCellWalkable(cell, room)) return 0f;
        float baseDens = op.density;
        float wallMul = ComputeWallProximityMultiplier(cell, room, op.wallProximityCurve);
        float maskMul = op.featureMaskMultiplier != null
            ? SampleFeatureMask(worldPos, room, op.featureMaskMultiplier) : 1f;
        return baseDens * wallMul * maskMul;
    }

    private static int ComputeDistanceToNearestWall(Vector2Int cell, RoomData room) { ... }
}
```

**This is the single source of truth for Karar #143-D/E/K logic.** All executors call this. No executor reimplements distance-to-wall or feature mask sampling.

### 2.3 EditMode tests under `Assets/Tests/EditMode/Brush/`

`BrushDecorativeExecutorTests.cs` — minimum 8 cases:

1. **FreeformDecal_PlacesOneSprite** — single click, walkable cell, density=1 → 1 GO spawned
2. **FreeformDecal_SkipsNonWalkableCell** — single click, walkable=false → 0 GO spawned
3. **ScatterAlongStroke_RespectsMinDistance** — long stroke with minDistance=32 → no two spawns closer than 32px
4. **Karar143E_EdgeBiasDensityMultiplier** — wallProximityCurve = (0→1.0, 1→0.6, 2→0.3, 3→0.1), cell distance 1 → multiplier 0.6
5. **Karar143K_FeatureMaskMultiplier** — featureMaskMultiplier set, mask alpha=0.5 at cell → density multiplied by 0.5
6. **EraseByLayer_RemovesOnlyTargetLayer** — L4 + L5 each have 1 decal, erase L4 only → L5 decal still present
7. **EraseAllDecorative_PreservesL1L2** — L1 tilemap + L2 tilemap + L4/L5/L6 decals → erase all decorative → L1/L2 untouched
8. **WeightedAssetPick_RespectsWeights** — pool with 3 sprites weights [0,1,0] → 100 picks all return sprite[1]

---

## 3. V1 EXCLUSIONS — DO NOT IMPLEMENT

- CompositeStrokeExecutor (Sprint 6)
- Editor UI / EditorWindow (Sprint 5)
- BiomeSkin render rules / soft alpha shader (Sprint 8)
- Auto-Dress / Regenerate Decorative / Smart Fill (Sprint 7 — those are higher-level automations)
- Ghost preview (Sprint 5)
- Hotkeys (Sprint 5)

---

## 4. Acceptance Criteria

A. `dotnet build RIMA.Runtime.csproj` returns 0 errors.
B. `dotnet build RIMA.Editor.csproj` returns 0 errors.
C. All 8 EditMode tests PASS.
D. Karar #143-D test passes (TEST 2: walkable=false → 0 spawns).
E. Karar #143-E test passes (TEST 4: curve multiplier correct).
F. Karar #143-K test passes (TEST 5: mask multiplier correct).
G. No modifications to existing painters (TransitionBrushPainter, DetailDecalPainter, AccentPainter, NaturalFeatureGraph, FeatureMaskSO). If a method addition is needed on a painter, add the minimum public signature only — document in CODEX_DONE.md.
H. Sprint 1+2 tests still PASS (regression).
I. No asmdef changes.

---

## 5. Safety Rules (binding)

All rules from `codex_safety_review_output.md` apply. Key:

1. Max **5 files per dispatch.** Sprint 4 has 6 source files + 1 test = 7 → split into 2 sub-dispatches:
   - Sub 1: `Karar143Enforcement.cs` + `FreeformDecalExecutor.cs` + `ScatterAlongStrokeExecutor.cs` + `StampExecutor.cs` + test (start)
   - Sub 2: `EraseByLayerExecutor.cs` + `EraseAllDecorativeExecutor.cs` + test (extend)
2. **No painter modifications** unless absolutely required (one method signature max, documented).
3. **Read before Edit** on all painter files referenced.
4. **Undo discipline** — every executor wraps spawns/destroys in `RegisterCreatedObjectUndo` / `DestroyObjectImmediate(go, true)` (true = with undo).
5. **No `AssetDatabase.Refresh()`.**
6. **No commit.**

---

## 6. Codex Self-Review Checklist

1. Read all 8 MUST READ files in §0?
2. Did Karar143Enforcement.cs implement walkable + edge-bias + feature mask in one place?
3. Do all 4 decorative executors (FreeformDecal, ScatterAlongStroke, Stamp + Erase*) call Karar143Enforcement instead of reimplementing logic?
4. Do all executors use `Undo.RegisterCreatedObjectUndo` / `Undo.DestroyObjectImmediate(go, true)`?
5. Did I avoid modifying any LIVE painter?
6. Are Karar143-D/E/K tests all passing?
7. Did `dotnet build` (both Runtime and Editor) pass 0 errors?
8. Did I respect max 5 files per dispatch (split into 2 sub-dispatches)?
9. Did I list all new .meta files in CODEX_DONE.md?
10. Did weighted asset pick test pass (TEST 8)?
11. Are L1/L2 Tilemap layers untouched by EraseAllDecorative (TEST 7)?
12. Does EraseByLayer use single Undo group?

---

## 7. Output Format

Same as Sprint 1/2 CODEX_DONE.md structure: Files Created, Painter Changes (must be empty or minimum), .meta GUID scan, Self-Review answers, Build/Test results, Open Questions, Files Modified Outside Scope.

---

## 8. Dependencies

**Blocked by:** Sprint 2 complete (executor router pattern).
**Blocks:** Sprint 6 (composite executor reuses these), Sprint 7 (automation calls these).
