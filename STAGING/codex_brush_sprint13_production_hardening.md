# Sprint 13 — Production Hardening + Batch Gate
**Codex Task Spec v1.0 | 2026-05-17 S87_PREP**

---

## Header

| Field | Value |
|---|---|
| **Task name** | Sprint 13 Production Hardening (walkableGrid + Bridson Poisson + rotation + variant pool + Collider2D + PropRegistry + Tilemap sorting) + Batch Gate (perf + undo + dep report + 10-room library) |
| **Owner** | 16-18 May Opus implement window (1 gün kaldı) → Codex review. 19 May+: Codex implement. |
| **Estimate** | 1.5–2 days |
| **Dependencies** | Sprint 9/10/11/12 hepsi LIVE (BrushAtlasImporter + RoomBank + Natural Engine + Props Mode V1) |
| **Source of truth** | [[brush-tool-v1-design]] §"Sprint 9-13 LOCK" + Sprint 12 rima-qc verdict (PASS-WITH-CONDITIONS, Condition 1) + Sprint 12 DONE.md forward path |
| **Memory anchors** | [[brush-tool-v1-design]] [[room-library-architecture]] [[karar-143-layered-pipeline]] [[natural-paint-integration]] [[combat-feel-research-combined]] |

---

## 1. Context + Rationale

**Sprint 12 LIVE** — PropDefinitionSO + PropPlacementData + PropFootprintValidator + PropsTab + PropPlacer + RoomTemplateSO.props + sample barrel_001.asset + 21 EditMode test. 282/282 EditMode PASS. rima-qc PASS-WITH-CONDITIONS, 2 condition:

- **Condition 1 (Sprint 13 backlog):** `PropFootprintValidator.IsWalkableTile` `cameraBounds.tileRect` fallback semantik yanlış (camera framing rect ≠ walkability map, wall tiles içeri pas ediyor). → RoomTemplateSO'ya `walkableGrid` field ekle + validator güncelle.
- **Condition 2 (pre-Sprint-12 cleanup):** Sprint 11 LIVE files (WallOverlayPainter + Composition/) uncommitted. → S87 git commit hygiene ile çözülür (Sprint 13 spec'i değil).

**Sprint 13 hedefi — iki ana stream:**

### Stream A — Sprint 12 forward path implementations (deferred from Sprint 12 spec §4 forbidden)
1. **walkableGrid field** RoomTemplateSO'ya (Condition 1 fix)
2. **Bridson Poisson** density-aware auto-place (Sprint 11 CompositionRoleMap integration)
3. **Prop rotation** (`rotationSteps ∈ {0, 90, 180, 270}`)
4. **Prop variant pool** (multiple sprites per PropDefinitionSO, random pick on placement)
5. **Unity Collider2D integration** (`blocksWalkable` flag → auto-add `BoxCollider2D`)
6. **PropRegistrySO** runtime GUID → PropDefinitionSO lookup (editor-only `AssetDatabase` yerine runtime registry)
7. **Tilemap sorting layer integration** (prop SR Y-position sort runtime)

### Stream B — Batch Gate + Production Hardening (memory original Sprint 13 scope)
8. **Perf smoke test** — 10-room library load + spawn + render < 2s
9. **Undo stress test** — 100x undo/redo on brush stroke + prop place, no Unity crash
10. **Dependency report** — auto-generated `RIMA_BrushTool_Dependencies.md` listing all SO refs, asset paths, asmdef edges
11. **10-room library** — user-authored 10 sample rooms in `Assets/Data/Rooms/Library/` for production smoke test

**Why before "Sprint 14+":** Brush V1 ship-readiness gate. Sprint 13 close = V1 LIVE for production. Sprint 14+ = combat integration, boss room procgen, meta-progression.

---

## 2. Deliverables — Exhaustive Checklist

### Stream A — Sprint 12 forward path

#### 2.1 `RoomTemplateSO.walkableGrid` field (Condition 1 fix)

Modify `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` (Sprint 10 LIVE — extend, don't break):

```csharp
[Header("Walkable Grid (Sprint 13 — Condition 1 fix)")]
[Tooltip("Per-tile walkability map. Index = (y * bounds.width) + x. true = walkable, false = wall/blocked. Empty array = full bounds walkable (fallback).")]
public bool[] walkableGrid;

public bool IsWalkable(Vector2Int tilePos)
{
    if (walkableGrid == null || walkableGrid.Length == 0)
        return bounds.Contains(tilePos);  // fallback: full bounds walkable
    Vector2Int local = tilePos - bounds.position;
    if (local.x < 0 || local.x >= bounds.width || local.y < 0 || local.y >= bounds.height)
        return false;
    int idx = (local.y * bounds.width) + local.x;
    return idx >= 0 && idx < walkableGrid.Length && walkableGrid[idx];
}
```

Update `PropFootprintValidator.IsWalkableTile` (`Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs`):

- Replace `cameraBounds.tileRect` fallback with `template.IsWalkable(tilePos)` call
- Remove Sprint 12 cameraBounds fallback comment (now obsolete)

#### 2.2 `BridsonPoissonAutoPlacer` (density-aware auto-place)

New file: `Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs` (sealed, editor-only)

```csharp
namespace RIMA.MapDesigner.Props.Auto
{
    #if UNITY_EDITOR
    using System.Collections.Generic;
    using UnityEngine;
    using RIMA.MapDesigner.Composition;
    using RIMA.MapDesigner.Room.Data;

    public sealed class BridsonPoissonAutoPlacer
    {
        public struct PlacementCandidate
        {
            public Vector2Int tilePos;
            public PropDefinitionSO prop;
        }

        /// <summary>
        /// Generate prop placements using Bridson Poisson disk sampling with role-aware density.
        /// </summary>
        /// <param name="template">Target room template (provides bounds + walkable + existing props).</param>
        /// <param name="roleMap">Composition role map (Sprint 11). Drives density per region.</param>
        /// <param name="propPool">Candidate props with their forbiddenRoles + distanceFromOtherProps.</param>
        /// <param name="seed">Deterministic seed for reproducibility.</param>
        /// <param name="targetDensity">0-1, base density before role multiplier.</param>
        public List<PlacementCandidate> Generate(
            RoomTemplateSO template,
            CompositionRoleMap roleMap,
            IReadOnlyList<PropDefinitionSO> propPool,
            int seed,
            float targetDensity);
    }
    #endif
}
```

Density rules:
- `CleanCenter` → 0.1× base density
- `DecoratedEdge` → 1.0× base
- `FocalCluster` → 2.0× base (clustered)
- `WallBand` → 0.0× (forbidden by default)
- `DoorSafety` → 0.0× (forbidden by default)
- `Empty` → 0.0×

Algorithm: Bridson Poisson disk sampling (radius = `distanceFromOtherProps`), role-aware rejection sampling, deterministic per-seed.

#### 2.3 `PropPlacementData.rotationSteps` activation

Modify `Assets/Scripts/MapDesigner/Props/PropPlacementData.cs`:

- Field already exists (Sprint 12 declared, always 0). Now: `rotationSteps ∈ {0, 90, 180, 270}` enforced.
- Add `RotateClockwise()` instance method that increments by 90 modulo 360.

Update `PropPlacer.cs` (Sprint 12 editor tool):

- Hotkey: `R` rotates current preview prop by 90° clockwise.
- On click, store current rotation in `PropPlacementData.rotationSteps`.

Update `PropFootprintValidator.cs`:

- For 2×N or N×2 footprints, rotation 90° / 270° swaps width and height. Validator must account for this when computing occupied tiles.

#### 2.4 `PropDefinitionSO.variantSprites` (variant pool)

Modify `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs`:

```csharp
[Header("Variant Pool (Sprint 13)")]
public Sprite[] variantSprites;  // Empty = use worldSprite only. Populated = random pick per placement.

public Sprite PickVariant(int seed)
{
    if (variantSprites == null || variantSprites.Length == 0)
        return worldSprite;
    int idx = unchecked(seed * 1103515245 + 12345) & int.MaxValue;
    return variantSprites[idx % variantSprites.Length];
}
```

Modify `PropPlacementData`:

```csharp
public int variantIndex = -1;  // -1 = unpicked, use worldSprite; >=0 = use variantSprites[variantIndex]
```

Update `PropPlacer.OnSceneClick`: set `variantIndex = (seed-based or random) % variantSprites.Length` if pool exists.

#### 2.5 Unity Collider2D integration

New file: `Assets/Scripts/MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs` (runtime, no `#if UNITY_EDITOR`)

```csharp
namespace RIMA.MapDesigner.Props.Runtime
{
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PropColliderAutoBuilder : MonoBehaviour
    {
        [SerializeField] private PropDefinitionSO propDef;

        private void Awake()
        {
            if (propDef == null || !propDef.blocksWalkable) return;
            if (GetComponent<BoxCollider2D>() != null) return;
            var box = gameObject.AddComponent<BoxCollider2D>();
            box.size = new Vector2(propDef.footprintSize.x, propDef.footprintSize.y);
            box.offset = new Vector2(propDef.footprintSize.x * 0.5f, propDef.footprintSize.y * 0.5f);
        }
    }
}
```

#### 2.6 `PropRegistrySO` (runtime GUID lookup)

New file: `Assets/Scripts/MapDesigner/Props/PropRegistrySO.cs`

```csharp
namespace RIMA.MapDesigner.Props
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "RIMA/MapDesigner/Props/PropRegistry", fileName = "PropRegistry")]
    public sealed class PropRegistrySO : ScriptableObject
    {
        [SerializeField] private List<PropDefinitionSO> allProps = new List<PropDefinitionSO>();

        private Dictionary<string, PropDefinitionSO> guidToProp;

        public PropDefinitionSO ResolveGuid(string guid)
        {
            if (guidToProp == null) RebuildIndex();
            return (guidToProp != null && guidToProp.TryGetValue(guid, out var prop)) ? prop : null;
        }

        public void RebuildIndex()
        {
            guidToProp = new Dictionary<string, PropDefinitionSO>();
            #if UNITY_EDITOR
            foreach (var prop in allProps)
            {
                if (prop == null) continue;
                var path = UnityEditor.AssetDatabase.GetAssetPath(prop);
                var guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);
                if (!string.IsNullOrEmpty(guid)) guidToProp[guid] = prop;
            }
            #endif
        }

        public IReadOnlyList<PropDefinitionSO> AllProps => allProps;
    }
}
```

Runtime-only resolver. Editor populates `allProps` list via menu utility `RIMA → MapDesigner → Props → Rebuild Registry`.

#### 2.7 Tilemap sorting layer integration

New file: `Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs`

```csharp
namespace RIMA.MapDesigner.Props.Runtime
{
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteAlways]
    public sealed class PropSorterRuntime : MonoBehaviour
    {
        [SerializeField] private PropDefinitionSO propDef;
        private SpriteRenderer sr;

        private void OnEnable()
        {
            sr = GetComponent<SpriteRenderer>();
            Apply();
        }

        private void LateUpdate()
        {
            if (propDef == null || sr == null) return;
            if (propDef.sortingMode == PropDefinitionSO.PropSortingMode.YPosition)
            {
                // Sort by Y position — lower Y = front (higher sortingOrder)
                sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100f);
            }
        }

        public void Apply()
        {
            if (propDef == null || sr == null) return;
            switch (propDef.sortingMode)
            {
                case PropDefinitionSO.PropSortingMode.FixedOrder:
                    sr.sortingOrder = propDef.sortingOrder;
                    break;
                case PropDefinitionSO.PropSortingMode.AboveAll:
                    sr.sortingOrder = 32760;  // near int16 max for Unity sprite sorting
                    break;
                case PropDefinitionSO.PropSortingMode.YPosition:
                    // Handled in LateUpdate
                    break;
            }
            if (propDef.sortingLayerOverride != 0)
                sr.sortingLayerID = propDef.sortingLayerOverride;
        }
    }
}
```

### Stream B — Batch Gate

#### 2.8 Perf smoke test

New test: `Assets/Tests/PlayMode/Brush/PerfSmokeTests.cs`

```
[Test]
public IEnumerator LoadTenRoomLibrary_SpawnAll_RendersUnder2Seconds()
{
    // Load all 10 RoomTemplateSO from Assets/Data/Rooms/Library/
    // Instantiate each (spawn player + enemies via RoomBankRuntimeTester pattern)
    // Stopwatch from start to last sprite rendered
    // Assert.Less(stopwatch.ElapsedMilliseconds, 2000)
}
```

#### 2.9 Undo stress test

New test: `Assets/Tests/EditMode/Brush/UndoStressTests.cs`

```
[Test]
public void Undo100x_BrushStroke_NoUnityCrash()
{
    // Author room, perform 100x brush strokes
    // Undo 100 times
    // Assert no NullReferenceException, Tilemap state matches initial
}

[Test]
public void Undo100x_PropPlace_NoUnityCrash()
{
    // Place 100 props (Sprint 12 PropPlacer)
    // Undo 100 times
    // Assert template.props.Count == 0
}
```

#### 2.10 Dependency report auto-generation

New editor utility: `Assets/Editor/MapDesigner/Brush/DependencyReportGenerator.cs`

- Menu item: `RIMA → MapDesigner → Brush → Generate Dependency Report`
- Output: `STAGING/RIMA_BrushTool_Dependencies.md`
- Contents:
  - All ScriptableObject assets under `Assets/Data/Brush/`
  - All MonoBehaviour types referenced by `MapDesignerBrushWindow`
  - All asmdef edges (RIMA.MapDesigner.Brush.* → dependencies)
  - All sprite atlases used (BrushAtlas/, Tiles/F1/Generated/)
  - Auto-generated table with file:line for refs

#### 2.11 10-room library

New folder: `Assets/Data/Rooms/Library/` with 10 user-authored `RoomTemplateSO` assets:

- `library_room_01_combat_shatteredkeep.asset` — basic combat, 12×10 tiles
- `library_room_02_combat_shatteredkeep.asset` — alt layout
- `library_room_03_elite_shatteredkeep.asset` — elite encounter
- `library_room_04_shop_neutral.asset` — shop room
- `library_room_05_rest_neutral.asset` — rest room
- `library_room_06_combat_corridor.asset` — narrow corridor
- `library_room_07_combat_arena.asset` — wide arena 16×12
- `library_room_08_curse_gate.asset` — curse gate variant
- `library_room_09_mystery.asset` — mystery room
- `library_room_10_boss_intro.asset` — boss intro corridor

Each authored manually using `MapDesignerBrushWindow` + SaveRoom. NOT auto-generated by Codex — user authors after Sprint 13 spec implement.

Sprint 13 includes the **`RoomBankSO_Library_v1.asset`** that references all 10.

---

## 3. Tests (EditMode + PlayMode)

### 3.1 walkableGrid tests (`RoomTemplateWalkableGridTests.cs`)
- `IsWalkable_EmptyGrid_FallsBackToBounds`
- `IsWalkable_PopulatedGrid_ReturnsCorrectValue`
- `IsWalkable_OutOfBounds_ReturnsFalse`

### 3.2 Bridson Poisson tests (`BridsonPoissonAutoPlacerTests.cs`)
- `Generate_DeterministicSeed_SameOutput`
- `Generate_CleanCenterRole_LowDensity`
- `Generate_FocalClusterRole_HighDensity`
- `Generate_RespectsMinDistance`
- `Generate_RespectsExistingProps`

### 3.3 Rotation tests (`PropRotationTests.cs`)
- `RotateClockwise_Increments90`
- `RotateClockwise_Wraps360To0`
- `Validator_Rotation90_SwapsFootprint`
- `Validator_Rotation270_SwapsFootprint`

### 3.4 Variant pool tests (`PropVariantTests.cs`)
- `PickVariant_EmptyPool_ReturnsWorldSprite`
- `PickVariant_DeterministicSeed_SameVariant`
- `PickVariant_DifferentSeeds_DistributesAcrossPool`

### 3.5 Collider auto-build tests (`PropColliderTests.cs`)
- `Awake_BlocksWalkable_AddsCollider`
- `Awake_DoesNotBlockWalkable_NoCollider`

### 3.6 PropRegistry tests (`PropRegistryTests.cs`)
- `ResolveGuid_KnownProp_Returns`
- `ResolveGuid_UnknownGuid_ReturnsNull`
- `RebuildIndex_RebuildsAllPropGuids`

### 3.7 Sorting tests (`PropSorterTests.cs`)
- `LateUpdate_YPositionMode_SortsCorrectly`
- `Apply_FixedOrderMode_UsesField`
- `Apply_AboveAllMode_HighSortingOrder`

### 3.8 Perf + undo (Stream B)
- `PerfSmokeTests.LoadTenRoomLibrary_SpawnAll_RendersUnder2Seconds` (PlayMode)
- `UndoStressTests.Undo100x_BrushStroke_NoUnityCrash` (EditMode)
- `UndoStressTests.Undo100x_PropPlace_NoUnityCrash` (EditMode)

**Total Sprint 13 new tests: ~24** (matches Sprint 10/11/12 cadence).

---

## 4. Forbidden in Sprint 13 (V1 close LOCK)

- ❌ Combat integration (Sprint 14+)
- ❌ Boss room procgen (Sprint 15+)
- ❌ Meta-progression hooks (Phase 2+)
- ❌ Modifying Sprint 9-12 LIVE files outside scope (only additive)
- ❌ Modifying Karar #143-D/E/K enforcement (defaults stable)
- ❌ Modifying Karar #144 weapon-less spec (orthogonal)
- ❌ New brush executor types (V1 close, no new layers)

---

## 5. File paths (LOCK)

**Source files (new):**
- `Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs` (UNITY_EDITOR)
- `Assets/Scripts/MapDesigner/Props/PropRegistrySO.cs`
- `Assets/Scripts/MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs`
- `Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs`
- `Assets/Editor/MapDesigner/Brush/DependencyReportGenerator.cs` (UNITY_EDITOR)

**Source files (modify, +field only — no break):**
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` (+`walkableGrid` field + `IsWalkable` method)
- `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs` (+`variantSprites` array + `PickVariant` method)
- `Assets/Scripts/MapDesigner/Props/PropPlacementData.cs` (+`variantIndex` field, activate `rotationSteps`)
- `Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs` (use `IsWalkable` + rotation-aware footprint)
- `Assets/Scripts/MapDesigner/Props/Editor/PropPlacer.cs` (R hotkey rotate + variant pick on click)

**Test files (new):**
- `Assets/Tests/EditMode/Props/PropRotationTests.cs`
- `Assets/Tests/EditMode/Props/PropVariantTests.cs`
- `Assets/Tests/EditMode/Props/PropRegistryTests.cs`
- `Assets/Tests/EditMode/Props/BridsonPoissonAutoPlacerTests.cs`
- `Assets/Tests/EditMode/Brush/UndoStressTests.cs`
- `Assets/Tests/EditMode/Room/RoomTemplateWalkableGridTests.cs`
- `Assets/Tests/PlayMode/Brush/PerfSmokeTests.cs`
- `Assets/Tests/PlayMode/Props/PropColliderTests.cs`
- `Assets/Tests/PlayMode/Props/PropSorterTests.cs`

**Asset files (new):**
- `Assets/Data/Brush/Props/PropRegistry_v1.asset` (PropRegistrySO with allProps list)
- `Assets/Data/Rooms/Library/.gitkeep` (user authors 10 rooms here)

---

## 6. Open Questions (Codex must resolve before impl)

- **OQ1 — Bridson Poisson radius interpretation:** `distanceFromOtherProps = 1f` means 1 tile minimum or 1 Unity unit? V1 LOCK: 1 **tile** (Vector2Int.Distance basis), consistent with Sprint 12 validator. Confirm test data.
- **OQ2 — Rotation pivot for 2×N props:** When rotating, does the prop pivot stay at footprint origin (bottom-left) or center? V1 LOCK: origin stays at bottom-left, footprint width/height swap. PropPlacer preview reflects new footprint dimensions.
- **OQ3 — Variant pool determinism:** PickVariant uses seed × LCG. Should seed be `tilePosition.GetHashCode()` (deterministic per-tile) or `template.GetInstanceID() + propsCount` (deterministic per-placement-order)? V1 LOCK: `tilePosition.GetHashCode()` — same tile always picks same variant on re-load.
- **OQ4 — PropRegistry runtime population:** Editor builds index via AssetDatabase. Runtime build (`RebuildIndex` called from `Awake` on first scene load) — populate from `allProps` list directly using `propDef.propId` (string GUID stored in SO)? V1 LOCK: `propId` IS the GUID at edit-time (auto-populated via `AssetPostprocessor`). Runtime uses `propId` for lookup.
- **OQ5 — Collider auto-build collision layer:** Default layer for prop colliders? V1 LOCK: `Default` Unity layer. Sprint 14+ may add custom prop layer.

---

## 7. Acceptance Criteria

- [ ] `dotnet build` PASS (all RIMA csproj)
- [ ] All 24+ Sprint 13 new tests PASS
- [ ] Full EditMode 306+/306+ PASS (282 + 24 new)
- [ ] Sprint 12 Condition 1 resolved (`PropFootprintValidator` uses `IsWalkable`, no more `cameraBounds.tileRect` semantic hack)
- [ ] All 7 forward-path items LIVE (walkableGrid, Bridson, rotation, variant, Collider2D, PropRegistry, Sorting)
- [ ] Perf smoke: 10-room library load + spawn < 2s (PlayMode test)
- [ ] Undo stress: 100x undo no crash (EditMode test)
- [ ] Dependency report generated to `STAGING/RIMA_BrushTool_Dependencies.md`
- [ ] 10-room library scaffolding ready (`Assets/Data/Rooms/Library/.gitkeep` + `PropRegistry_v1.asset` + `RoomBankSO_Library_v1.asset` stub)
- [ ] NO modification to Sprint 9-12 LIVE method signatures (additive only)
- [ ] Karar #143-D/E/K compliance verified

---

## 8. Routing

- Effort: **high**
- Background dispatch via `cx_dispatch.py` (orchestrator launches with `run_in_background: true`)
- Profile: laurethgame (auto-select)
- Expected duration: 1.5–2 days Codex implement OR ~6-8 hours Opus implement (16-18 May window — 1 gün kaldı)
- Review cycle: Codex spec review → 0-2 fix → impl → Codex impl review → 0-2 fix → PASS

---

## 9. Brush V1 close (Sprint 13 PASS sonrası)

Sprint 13 PASS = **Brush V1 SHIP-READY**. Sprint 9-13 complete:
- Sprint 1-8: V1 close (data + executor + brushes + composite + automation + biome skin)
- Sprint 9: Atlas Importer + Wang variants
- Sprint 10: RoomBank vertical slice
- Sprint 11: Natural Engine (Composition Roles)
- Sprint 12: Props Mode MVP
- Sprint 13: Production Hardening + Batch Gate

**Sprint 14+ (post V1):**
- Combat integration (mob spawn from RoomBank, encounter pacing)
- Boss room procgen (boss arena patterns)
- Meta-progression hooks (Rift Break tier unlocks, room evolution)
- Marketplace + namespace prefixing (V2 ecosystem)

---

End of Sprint 13 spec v1.0.
