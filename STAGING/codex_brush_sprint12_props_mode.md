# Sprint 12 — Props Mode MVP
**Codex Task Spec v1.0 | 2026-05-16 S86 SPRINT12_PREP**

---

## Header

| Field | Value |
|---|---|
| **Task name** | Sprint 12 Props Mode MVP (PropDefinitionSO + Props tab + footprint validator + save/load) |
| **Owner** | 16-18 May window: Opus implement → Codex review. 19 May+: Codex implement. |
| **Estimate** | 1–1.5 days |
| **Dependencies** | Sprint 9 LIVE (BrushAtlasImporter + Wang variants) + Sprint 10 LIVE (RoomTemplateSO + RoomBank + GUID-preserving save/load) + Sprint 11 LIVE (CompositionRoleMap + WangContextResolver) |
| **Source of truth** | [[brush-tool-v1-design]] §"Sprint 9-13 LOCK" + [[room-library-architecture]] + [[natural-paint-integration]] |
| **Memory anchors** | [[brush-tool-v1-design]] [[room-library-architecture]] [[karar-143-layered-pipeline]] [[natural-paint-integration]] [[s86-opus-signoff-decisions]] |

---

## 1. Context + Rationale

**Sprint 11 LIVE** — `CompositionRoleMap` her tile'a role atar (CleanCenter / DecoratedEdge / FocalCluster / WallBand / DoorSafety). Painter'lar role-aware paint kararı veriyor (V1.5'te diğer 5 painter da role-aware).

**Sprint 12 hedefi:** **Props** — decorative or interactable gameplay objects (barrel, table, statue, chest, lantern, sign, brazier) için authoring + placement + save/load.

**Props ≠ paint stroke** — props **single-place** (tile click ile yerleştirilir, footprint kadar tile işgal eder). User Props tab'da prop seç → scene'de tile'a tıkla → yerleştir. Validation: footprint walkable preserve, role-aware (FocalCluster prefer, DoorSafety avoid), footprint clear (overlap yok).

**V1 scope dar (LOCK):**
1. Data layer: `PropDefinitionSO` + `PropPlacementData`
2. RoomTemplate extension: `List<PropPlacementData>` field
3. Validator: `PropFootprintValidator` (5 rule)
4. UI: Props tab in existing brush EditorWindow
5. Scene tool: `PropPlacer` (hover + click)
6. Save/load: piggyback on Sprint 10 GUID-preserving flow

**V2 (Sprint 13+ defer):** Bridson Poisson auto-place, prop rotation, prop variant pool, ambient generation, Unity Collider2D integration.

**Why Sprint 11 önce Sprint 12:** Props yerleşimi `CompositionRole`'leri kullanır. Sprint 11 olmadan props rastgele dağılır — Hades tier okunabilirliği bozulur. [[natural-paint-integration]]

---

## 2. Deliverables — Exhaustive Checklist

### 2.1 `CompositionRole` reference (already LIVE in Sprint 11)

```csharp
// EXISTING Sprint 11 — DO NOT redefine
namespace RIMA.MapDesigner.Composition
{
    public enum CompositionRole
    {
        Empty, WallBand, DoorSafety, DecoratedEdge, CleanCenter, FocalCluster
    }
}
```

Props use this enum — no new enum in Sprint 12.

### 2.2 `PropDefinitionSO`

```csharp
namespace RIMA.MapDesigner.Props
{
    using UnityEngine;
    using RIMA.MapDesigner.Composition;

    [CreateAssetMenu(menuName = "RIMA/MapDesigner/Props/PropDefinition", fileName = "NewPropDefinition")]
    public sealed class PropDefinitionSO : ScriptableObject
    {
        [Header("Identity")]
        public string propId;                                 // Unique within Props folder (user enforces uniqueness)
        public string displayName;                            // EditorWindow palette label
        [TextArea(2, 4)] public string description;
        public Sprite icon;                                   // 32x32 UI palette icon
        public Sprite worldSprite;                            // Tile-space sprite for placement preview + runtime

        [Header("Footprint (V1: rect-based, no rotation)")]
        public Vector2Int footprintSize = Vector2Int.one;     // Default 1x1 tile
        public Vector2Int spriteAnchor = Vector2Int.zero;     // Sprite pivot offset from footprint origin (tile-space)

        [Header("Placement Rules")]
        public bool blocksWalkable = true;                    // V1: if true, footprint tiles flip walkable=false
        public bool requiresWalkableTile = false;             // V1: if true, all footprint tiles must be walkable=true pre-placement
        public CompositionRole[] preferredRoles;              // Empty = no preference. Populated = soft preference (validator passes but warns if violated)
        public CompositionRole[] forbiddenRoles = new[]       // Hard exclusion. Default: door safety + wall band
        {
            CompositionRole.DoorSafety, CompositionRole.WallBand
        };
        public float distanceFromOtherProps = 1f;             // Min tile distance from any other prop (Euclidean, tile-space)

        [Header("Karar #143 Compliance (Sprint 12 V1 LOCK)")]
        public bool respectsWalkableMask = true;              // ALWAYS true V1 (Karar #143-D enforcement)

        [Header("Visual Sorting")]
        public PropSortingMode sortingMode = PropSortingMode.YPosition;
        public int sortingLayerOverride = 0;                  // 0 = use prop layer default; >0 = explicit override
        public int sortingOrder = 0;

        public enum PropSortingMode
        {
            YPosition,    // Sorting order derived from world Y (lower Y = front, higher Y = back)
            FixedOrder,   // sortingOrder field is authoritative
            AboveAll      // Render above all other props in scene (overlay icons, glow markers)
        }
    }
}
```

### 2.3 `PropPlacementData`

```csharp
namespace RIMA.MapDesigner.Props
{
    using System;
    using UnityEngine;

    [Serializable]
    public sealed class PropPlacementData
    {
        public string propDefinitionGuid;     // GUID reference to PropDefinitionSO asset (Sprint 10 GUID-preserving compatible)
        public Vector2Int tilePosition;       // Footprint origin (bottom-left)
        public int rotationSteps = 0;         // V1: always 0 (no rotation enforced by validator)
        public string placedByUser;           // Audit field — editor session identifier or empty in headless

        public PropPlacementData() { }

        public PropPlacementData(string guid, Vector2Int pos)
        {
            this.propDefinitionGuid = guid;
            this.tilePosition = pos;
            this.rotationSteps = 0;
        }
    }
}
```

### 2.4 `RoomTemplateSO` extension (modify Sprint 10 LIVE file)

```csharp
// MODIFY existing Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs
// ADD this field (do not touch existing fields):

[Header("Props (Sprint 12)")]
public System.Collections.Generic.List<RIMA.MapDesigner.Props.PropPlacementData> props
    = new System.Collections.Generic.List<RIMA.MapDesigner.Props.PropPlacementData>();
```

Backward compatibility: existing RoomTemplate `.asset` files without `props` field will deserialize with empty list (Unity default). Save/load roundtrip must preserve empty list.

### 2.5 `PropFootprintValidator`

```csharp
namespace RIMA.MapDesigner.Props
{
    using System.Collections.Generic;
    using UnityEngine;
    using RIMA.MapDesigner.Composition;
    using RIMA.MapDesigner.Room.Data;

    public static class PropFootprintValidator
    {
        public enum ValidationResult
        {
            Valid,
            OutOfBounds,
            OverlapsExistingProp,
            ViolatesWalkableConstraint,
            ViolatesRoleConstraint,
            TooCloseToOtherProp,
            InvalidArgument
        }

        /// <summary>
        /// Validates whether <paramref name="propDef"/> can be placed at <paramref name="tilePosition"/> in <paramref name="template"/>.
        /// </summary>
        /// <param name="propDef">Prop definition. Null → InvalidArgument.</param>
        /// <param name="tilePosition">Footprint origin (bottom-left).</param>
        /// <param name="template">Target room template. Null → InvalidArgument.</param>
        /// <param name="roleMap">Composition role map from Sprint 11. Null → ViolatesRoleConstraint check skipped (warn).</param>
        /// <param name="existingProps">All props already placed in the room. Empty → no overlap check needed.</param>
        /// <param name="failureDetail">Human-readable failure detail (empty on Valid).</param>
        public static ValidationResult Validate(
            PropDefinitionSO propDef,
            Vector2Int tilePosition,
            RoomTemplateSO template,
            CompositionRoleMap roleMap,
            IReadOnlyList<PropPlacementData> existingProps,
            out string failureDetail);
    }
}
```

**Validation order (V1 LOCK):**
1. `propDef == null` OR `template == null` → `InvalidArgument`
2. Footprint bounds check (`tilePosition` to `tilePosition + footprintSize` must fit in `template.bounds`) → `OutOfBounds`
3. Walkable constraint:
   - If `requiresWalkableTile` and any footprint tile is non-walkable → `ViolatesWalkableConstraint`
   - If `!blocksWalkable` and `requiresWalkableTile` → still requires walkable=true
4. Role constraint:
   - If `roleMap != null` and ANY footprint tile has role in `forbiddenRoles` → `ViolatesRoleConstraint`
   - `preferredRoles` violation = warn only, returns `Valid` with detail string
5. Existing prop overlap (rect intersection of footprints) → `OverlapsExistingProp`
6. Min distance check (Euclidean tile-space distance to nearest other prop origin < `distanceFromOtherProps`) → `TooCloseToOtherProp`
7. Else → `Valid`

### 2.6 `PropsTab` (EditorWindow integration)

Existing brush `EditorWindow` (Sprint 5 LIVE) has tab structure. Add new tab **"Props"** alongside existing tabs (Paint, Brushes, BiomeSkin, etc.).

**Layout:**
- **Left panel (200px)**: Vertical scroll list of all `PropDefinitionSO` assets discovered in `Assets/Data/Brush/Props/` (recursive). Each entry: icon (32x32) + displayName + propId. Click selects.
- **Center panel**: Selected prop preview — worldSprite, footprintSize visualization, sortingMode label.
- **Right panel (180px)**: Placement rules summary (blocksWalkable, requiresWalkableTile, preferred/forbidden roles, distanceFromOtherProps).
- **Bottom panel**: Live validation feedback for hovered tile (when PropPlacer active in scene).

**File:** `Assets/Scripts/MapDesigner/Props/Editor/PropsTab.cs` (sealed, `#if UNITY_EDITOR`)

### 2.7 `PropPlacer` (scene tool)

```csharp
namespace RIMA.MapDesigner.Props.Editor
{
    #if UNITY_EDITOR
    using UnityEngine;
    using RIMA.MapDesigner.Composition;
    using RIMA.MapDesigner.Room.Data;

    public sealed class PropPlacer
    {
        public bool IsActive { get; set; }

        public void OnSceneClick(
            Vector2Int tilePos,
            PropDefinitionSO selectedProp,
            RoomTemplateSO targetTemplate,
            CompositionRoleMap roleMap);

        public void OnHover(
            Vector2Int tilePos,
            PropDefinitionSO selectedProp,
            RoomTemplateSO targetTemplate,
            CompositionRoleMap roleMap);

        public void DrawPreview(
            Vector2Int tilePos,
            PropDefinitionSO selectedProp,
            bool isValid,
            Color validColor,
            Color invalidColor);
    }
    #endif
}
```

**Behavior:**
- Hover: call `PropFootprintValidator.Validate`, store last result, drive `DrawPreview` (green = valid, red = invalid).
- Click: if last validation `Valid`, append `new PropPlacementData(guid, tilePos)` to `template.props`, mark template dirty (`EditorUtility.SetDirty`).
- No multi-tile drag in V1 (one prop per click).

### 2.8 Save/Load integration (NO signature change)

- `RoomTemplateSaver` (Sprint 10 LIVE) — props field auto-serialized via `EditorUtility.CopySerialized` flow already established. No code change in Saver.
- `RoomTemplateLoader` (Sprint 10 LIVE) — props field auto-loaded. No code change in Loader.
- **GUID preservation test:** Re-save existing room template with 3 props placed; verify `propDefinitionGuid` strings preserved per-prop (Sprint 10 GUID flow handles this).

### 2.9 Sample asset

Commit ONE sample PropDefinitionSO for manual smoke test:
- `Assets/Data/Brush/Props/Barrel/barrel_001.asset`
- propId: `"barrel_001"`
- displayName: `"Wooden Barrel"`
- footprintSize: (1, 1)
- blocksWalkable: true
- requiresWalkableTile: false
- preferredRoles: empty
- forbiddenRoles: { DoorSafety, WallBand }
- distanceFromOtherProps: 1f
- icon + worldSprite: null (user populates later; missing sprite must not crash validator)

---

## 3. Tests (EditMode, RIMA.Brush.Tests assembly)

### 3.1 `PropDefinitionSOTests` (4 tests)

- `Defaults_FootprintIsOneByOne`
- `Defaults_RespectsWalkableMaskTrue`
- `Defaults_ForbiddenRolesContainsDoorSafetyAndWallBand`
- `Defaults_PreferredRolesEmpty`

### 3.2 `PropFootprintValidatorTests` (10 tests minimum)

- `Validate_NullPropDef_ReturnsInvalidArgument`
- `Validate_NullTemplate_ReturnsInvalidArgument`
- `Validate_OutOfBoundsFootprint_ReturnsOutOfBounds`
- `Validate_OverlapsExistingProp_ReturnsOverlapsExistingProp`
- `Validate_RequiresWalkableButNonWalkable_ReturnsViolatesWalkableConstraint`
- `Validate_OnDoorSafetyRole_ReturnsViolatesRoleConstraint`
- `Validate_OnWallBandRole_ReturnsViolatesRoleConstraint`
- `Validate_OnFocalClusterRole_ReturnsValid`
- `Validate_WithinMinDistance_ReturnsTooCloseToOtherProp`
- `Validate_NullRoleMap_SkipsRoleCheck_StillValidates`

### 3.3 `PropsRoomSerializationTests` (3 tests, PlayMode optional EditMode preferred)

- `SaveRoomWithThreeProps_LoadsBack_GuidsPreserved`
- `SaveEmptyPropsList_LoadsBack_EmptyList`
- `ModifyPropPosition_Resave_PositionPersists`

### 3.4 `PropPlacerTests` (3 tests, EditMode)

- `OnClick_ValidTile_AppendsToTemplate`
- `OnClick_InvalidTile_TemplateUnchanged`
- `OnHover_UpdatesLastValidationResult`

**Total Sprint 12 new tests: 20** (matches Sprint 10/11 cadence).

---

## 4. Forbidden in Sprint 12 (V1 scope LOCK)

- ❌ **Bridson Poisson auto-placement** (Sprint 13+ V2)
- ❌ **Prop rotation** (Sprint 13+ V2; `rotationSteps` field exists but always 0)
- ❌ **Prop variant pool** (Sprint 13+ V2; single sprite per definition V1)
- ❌ **Ambient prop auto-generation** (Sprint 13+ V2)
- ❌ **Unity `Collider2D` integration** (Sprint 13+ V2; `blocksWalkable` is data-only flag V1)
- ❌ **Multi-tile drag placement** (Sprint 13+ V2; one click = one prop V1)
- ❌ **L7+ rendering layer** (Sprint 13+ production hardening)
- ❌ **Modifying Sprint 11 LIVE files** (`Composition/*`, `WallOverlayPainter.cs`) — props read CompositionRoleMap, do not write
- ❌ **Modifying Karar #143-D/E/K enforcement** (default `respectsWalkableMask = true` is hard-coded)

---

## 5. File paths (LOCK)

**Source files (new):**
- `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs`
- `Assets/Scripts/MapDesigner/Props/PropPlacementData.cs`
- `Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs`
- `Assets/Scripts/MapDesigner/Props/Editor/PropsTab.cs` (UNITY_EDITOR)
- `Assets/Scripts/MapDesigner/Props/Editor/PropPlacer.cs` (UNITY_EDITOR)

**Source files (modify):**
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` (+1 field `props`, no other change)

**Test files (new):**
- `Assets/Tests/EditMode/Props/PropDefinitionSOTests.cs`
- `Assets/Tests/EditMode/Props/PropFootprintValidatorTests.cs`
- `Assets/Tests/EditMode/Props/PropsRoomSerializationTests.cs`
- `Assets/Tests/EditMode/Props/PropPlacerTests.cs`

**Asset files (new):**
- `Assets/Data/Brush/Props/Barrel/barrel_001.asset` (sample, fields per §2.9)
- `Assets/Data/Brush/Props/.gitkeep` (empty folder marker for prop SO authoring)

**Assembly definition (modify or create):**
- If new `.asmdef` needed: `Assets/Scripts/MapDesigner/Props/RIMA.MapDesigner.Props.asmdef`
- Reference: `RIMA.MapDesigner.Composition`, `RIMA.MapDesigner.Room`, `Unity.TextMeshPro` (if used)

---

## 6. Karar #143 Compliance Checks

- ✅ All `PropDefinitionSO.respectsWalkableMask` defaults to **true** (Karar #143-D)
- ✅ Default `forbiddenRoles` = `{ DoorSafety, WallBand }` (Karar #143-E door safety + wall band protect)
- ✅ Props **DO NOT** modify `wallProximityCurve` (V1 doesn't expose; V2 may)
- ✅ Props **DO NOT** modify `featureMaskMultiplier` (V1 doesn't expose; V2 may)
- ✅ Props use `CompositionRoleMap` as **read-only** input — no mutation of role map data

---

## 7. Open Questions (Codex must resolve before impl)

- **OQ1 — Sorting layer runtime integration:** `PropSortingMode.YPosition` requires runtime SR sorting integration with existing tilemap sorting layers. **V1 LOCK:** editor-only Y-sort preview (PropPlacer.DrawPreview reflects sort, but runtime Tilemap sorting is Sprint 13 production hardening scope). Confirm in Codex review.
- **OQ2 — Footprint origin convention:** For a 2x2 prop at `(3, 3)`, does it occupy `(3,3), (4,3), (3,4), (4,4)` (bottom-left expansion) or `(3,3), (4,3), (3,2), (4,2)` (top-left expansion)? **V1 LOCK:** bottom-left origin, occupies `(origin.x + dx, origin.y + dy)` for `dx ∈ [0, w), dy ∈ [0, h)`. Confirm test data uses this convention.
- **OQ3 — `propDefinitionGuid` lookup at runtime:** Loaded RoomTemplate carries GUIDs, not direct SO refs. Runtime resolution requires `AssetDatabase.GUIDToAssetPath` (editor-only) or a `PropRegistrySO` mapping GUID → PropDefinitionSO. **V1 LOCK:** editor-only resolution; runtime PropRegistry deferred to Sprint 13. PropPlacementData.propDefinitionGuid is stored, resolved on `RoomTemplateLoader.Load` via `AssetDatabase` inside `#if UNITY_EDITOR`.
- **OQ4 — Empty `forbiddenRoles` after deserialization:** If a user clears the default `{ DoorSafety, WallBand }` array, deserialization may produce null instead of empty array depending on Unity version. **V1 LOCK:** Validator treats `null` as `empty` (no role exclusion). Test covers this case.

---

## 8. Acceptance Criteria

- [ ] `dotnet build` PASS (RIMA.Runtime + RIMA.Editor + RIMA.Brush.Tests)
- [ ] All 20 Sprint 12 new tests PASS
- [ ] Full EditMode 197/197 PASS (after Codex parallel 4-test failure fix; if those 4 still fail, accept 197 minus those 4)
- [ ] `PropsTab` visible in brush EditorWindow alongside existing tabs (manual verification via Unity Editor screenshot or unity_reflect MCP query)
- [ ] 1 sample `barrel_001.asset` committed for smoke test
- [ ] **NO** Sprint 11 LIVE files touched (Composition/*, WallOverlayPainter.cs)
- [ ] Karar #143-D/E/K compliance verified in test (`Defaults_RespectsWalkableMaskTrue`, `Defaults_ForbiddenRolesContainsDoorSafetyAndWallBand`)
- [ ] 4 open questions resolved with documented decisions in `STAGING/codex_brush_sprint12_props_mode_DONE.md`

---

## 9. Routing

- Effort: **high**
- Background dispatch via `cx_dispatch.py` (orchestrator launches with `run_in_background: true`)
- Profile: laurethgame (auto-select via LastRefresh)
- Expected duration: 1–1.5 days Codex implement OR ~4-6 hours Opus implement (16-18 May window)
- Review cycle expected: Codex spec review (~30 min) → Opus/Codex implement → Codex impl review → 0-2 fix iterations (Sprint 10/11 cadence pattern)

---

## 10. Forward path (Sprint 13 preview, not in Sprint 12 scope)

Sprint 13 = Production Hardening:
- Bridson Poisson auto-place (density-aware via existing `wallProximityCurve` + `featureMaskMultiplier`)
- Prop rotation (`rotationSteps ∈ {0, 90, 180, 270}`)
- Prop variant pool (multiple sprites per definition, random pick on placement)
- Unity Collider2D integration (`blocksWalkable` flag drives auto-collider)
- PropRegistrySO runtime GUID lookup
- Tilemap sorting layer integration

Sprint 12 must NOT pre-empt Sprint 13 work. If a deliverable bleeds into Sprint 13 scope, halt and document in DONE.md.

---

End of Sprint 12 spec v1.0.
