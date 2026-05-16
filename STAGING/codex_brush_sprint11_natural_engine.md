# Sprint 11 — Natural Engine: CompositionRoleMap + Wang Context-Aware Paint
**Codex Task Spec v1.0 | 2026-05-16 S86 SPRINT11**

---

## Header

| Field | Value |
|---|---|
| **Task name** | Sprint 11 Natural Engine V1 (Composition Roles + Wang Context-Aware) |
| **Owner** | Opus implement → Codex review (16-18 May workflow override) / Codex implement (19 May+ normal routing) |
| **Estimate** | 1.5–2 days |
| **Dependencies** | Sprint 9 LIVE (BrushAtlasImporter + Wang variants) + Sprint 10 LIVE (RoomTemplateSO + RoomBank) |
| **Source of truth** | `STAGING/sprite_strategy_FINAL_LOCK.md` §5 Natural Engine + CURRENT_STATUS S86 PREP-3 §"Composition roles primary model" |
| **Memory anchors** | [[natural-paint-integration]] [[brush-tool-v1-design]] [[karar-143-layered-pipeline]] [[room-library-architecture]] [[combat-feel-research-combined]] |

---

## 1. Context + Rationale

**Karar #143 pipeline L1-L2 LIVE, L3 Wang LIVE.** Sprint 9 BrushAtlasImporter + Wang variants çalışıyor. Sprint 10 RoomTemplate + RoomBank vertical slice geçti. Sıradaki:

**Problem:** Mevcut painter'lar (WallOverlayPainter, DetailDecalPainter, AccentPainter, FreeformDecalExecutor) **uniform random** dağıtım yapıyor. Hades tier okunabilirlik için yetersiz — her tile aynı olasılıkla decorate ediliyor, ama gerçek Hades odaları kompozisyon hiyerarşisine sahip: **clean center / decorated edge / focal cluster / wall band / door safety**.

**Wang side:** L3 wall painter şu an variantları rastgele picks. Komşu tile'larla context-aware seçim YOK → wall corners arasında "jump" görüntüsü oluşuyor.

**Sprint 11 hedef:**
1. **CompositionRoleMap** — RoomTemplate'ten otomatik üretilen role grid (her tile pozisyonuna composition role atar)
2. **WangContextResolver** — komşu tile'lara bakarak doğru Wang case auto-select
3. **1 painter integration** (WallOverlayPainter) — role'e göre paint decision

V1 scope dar: 2 core sistem + 1 integration. V1.5: kalan 5 painter'ı role-aware yap. V2: focal cluster authoring tools + Bridson Poisson.

**Why before Sprint 12 Props:** Props (Sprint 12) yerleşimi composition role'leri kullanacak. Sprint 11 olmadan props rastgele dağılır — Hades tier bozulur. [[natural-paint-integration]]

---

## 2. Deliverables — Exhaustive Checklist

### 2.1 `CompositionRole` enum

```csharp
namespace RIMA.MapDesigner.Composition
{
    /// <summary>
    /// Composition role per tile. Primary model: clean center / decorated edge / focal cluster
    /// / wall band / door safety. Default = Empty (outside bounds).
    /// </summary>
    public enum CompositionRole
    {
        Empty,           // Outside RoomTemplate bounds — no paint
        WallBand,        // Immediate wall tile (Karar #143 L3 layer area)
        DoorSafety,      // 2-3 tile clearance around door sockets — minimal paint
        DecoratedEdge,   // 1-2 tile band inside walls — high decoration density
        CleanCenter,     // Interior playable area — low decoration, high readability
        FocalCluster     // Designed visual interest zone — authored marker required
    }
}
```

### 2.2 `CompositionRoleMap` data class

```csharp
namespace RIMA.MapDesigner.Composition
{
    /// <summary>
    /// Tile-space role grid for a single room. Generated from RoomTemplateSO at edit-time.
    /// Runtime read via GetRoleAt(Vector2Int). Default cell = Empty.
    /// </summary>
    [Serializable]
    public class CompositionRoleMap
    {
        public RectInt bounds;
        public CompositionRole[] flatRoleGrid;   // length = bounds.width * bounds.height

        public CompositionRole GetRoleAt(Vector2Int tilePos);
        public void SetRoleAt(Vector2Int tilePos, CompositionRole role);
        public int CountOfRole(CompositionRole role);
    }
}
```

Why flat array not 2D: Unity serializer is bad with 2D arrays; flat with width*y+x indexing is standard.

### 2.3 `CompositionRoleMapGenerator`

```csharp
namespace RIMA.MapDesigner.Composition
{
    /// <summary>
    /// Generates CompositionRoleMap from RoomTemplateSO at edit-time (or runtime if needed).
    /// Algorithm:
    /// 1. Fill bounds with CleanCenter default
    /// 2. Mark perimeter (1 tile border inside bounds) as WallBand
    /// 3. Mark 1-2 tile band inside WallBand as DecoratedEdge
    /// 4. For each DoorSocket: mark radius 2-3 tiles centered on socket position as DoorSafety
    /// 5. (V1 skip) FocalCluster authored separately via marker GameObject in prefab
    /// </summary>
    public static class CompositionRoleMapGenerator
    {
        public static CompositionRoleMap GenerateFromRoom(RoomTemplateSO room);

        // Tunable parameters (V1 hardcoded; V1.5 SO):
        public const int DecoratedEdgeWidth = 2;     // tiles inset from WallBand
        public const int DoorSafetyRadius = 3;       // tiles from door socket center
    }
}
```

Priority order (highest wins):
1. DoorSafety (overrides everything)
2. WallBand (perimeter)
3. DecoratedEdge (1-2 tile inset)
4. FocalCluster (V1 skip — manual marker integration V1.5)
5. CleanCenter (default interior)

### 2.4 `WangContextResolver` (L3 paint context-aware)

```csharp
namespace RIMA.MapDesigner.Composition
{
    /// <summary>
    /// Resolves correct Wang variant case for L3 wall paint based on 8-neighbor tile context.
    /// Variant tag format: "wang_NESW" (N/E/S/W = which corners have wall presence).
    /// </summary>
    public class WangContextResolver
    {
        /// <summary>
        /// Looks at the 8 neighbors of `pos` in `tilemap` and returns a Wang case identifier
        /// matching the variant tag convention from BrushAtlasImporter / WangSliceGenerator.
        /// Returns null if pos itself is not a wall cell.
        /// </summary>
        public string ResolveCaseAt(Vector2Int pos, Tilemap walkableMaskTilemap);

        /// <summary>
        /// From a list of BrushAssetVariant candidates, picks the variant whose variantId
        /// matches the resolved Wang case. Falls back to first variant if no match.
        /// </summary>
        public BrushAssetVariant PickVariantForCase(
            string wangCase,
            List<BrushAssetVariant> candidates);
    }
}
```

Wang case encoding (matches Sprint 9 importer output):
- 4-bit corner mask: NE / NW / SE / SW
- Format: `wang_{NE}{NW}{SE}{SW}` where each is 0/1
- Example: `wang_1110` = NE+NW+SE corners filled, SW empty

### 2.5 Integration — WallOverlayPainter extension

**Important:** `WallOverlayPainter` is `public sealed class`. **NO `partial` split** — add the new method directly inside the existing file body. Existing methods + sealed modifier UNCHANGED.

```csharp
// Existing WallOverlayPainter.PlaceWallSprite signature stays.
// Add new public method `PlaceWallSprite_ContextAware` inside the SAME sealed class body.

// Inside Assets/Scripts/MapDesigner/WallOverlayPainter.cs (existing file):
public sealed class WallOverlayPainter : MonoBehaviour
{
    // ... existing methods unchanged ...

    /// <summary>
    /// V1 context-aware overload. Falls back to existing random pick if compositionMap or
    /// wangResolver is null. Skips placement if role is not WallBand.
    /// </summary>
    public void PlaceWallSprite_ContextAware(
        Vector2Int pos,
        CompositionRoleMap compositionMap,
        WangContextResolver wangResolver,
        Tilemap walkableMask)
    {
        // ... implementation ...
    }
}
```

V1 behavior:
- If `compositionMap.GetRoleAt(pos)` is **not WallBand** → skip (no wall placement outside intended band)
- If WallBand → resolve Wang case via WangContextResolver
- Pick variant from L3 AssetPool matching the case (variant matched on `BrushAssetVariant.variantId`)

### 2.6 Test Suite

**EditMode — Composition Role Generator** (`CompositionRoleMapGeneratorTests.cs`):
- Generate from 10×10 room with 1 north door at (5, 9)
  - Assert perimeter cells = WallBand
  - Assert (5, 9) area = DoorSafety (radius 3)
  - Assert (5, 5) center = CleanCenter
  - Assert (1, 1), (8, 1), (1, 8), (8, 8) = DecoratedEdge (inset 1 from WallBand)
- Edge case: 4×4 minimal room (too small for DecoratedEdge)
  - Assert all cells either WallBand or CleanCenter (no DecoratedEdge)

**EditMode — Wang Context Resolver** (`WangContextResolverTests.cs`):
- Build a 5×5 walkable mask tilemap with walls at (0,0), (1,0), (2,0), (0,1) (L-shape corner)
- Query position (1,1) (interior near walls)
- Assert resolved case = `wang_1100` or similar (SE+SW walls present, NE+NW empty)
- Test against synthetic Wang variant list of 16 cases → assert correct variant returned

**EditMode — Integration** (`CompositionPainterIntegrationTests.cs`):
- Build minimal 10×10 RoomTemplateSO + CompositionRoleMap
- Place WallOverlayPainter.PlaceWallSprite_ContextAware at WallBand cell → assert sprite placed
- Same call at CleanCenter cell → assert NO sprite placed (role mismatch)

---

## 3. File Scope (LOCK)

Codex may only touch these files. Any other file requires orchestrator approval.

| Path | Status |
|---|---|
| `Assets/Scripts/MapDesigner/Composition/CompositionRole.cs` | NEW |
| `Assets/Scripts/MapDesigner/Composition/CompositionRoleMap.cs` | NEW |
| `Assets/Scripts/MapDesigner/Composition/CompositionRoleMapGenerator.cs` | NEW |
| `Assets/Scripts/MapDesigner/Composition/WangContextResolver.cs` | NEW |
| `Assets/Scripts/MapDesigner/WallOverlayPainter.cs` | MODIFY — add `PlaceWallSprite_ContextAware` method INSIDE existing `sealed class` body (no `partial` split; sealed modifier kept). Existing methods untouched. |
| `Assets/Scripts/MapDesigner/Brush/Data/BrushAssetVariant.cs` | READ-ONLY (verify `variantId` field exists; do not modify) |
| `Assets/Tests/EditMode/Composition/CompositionRoleMapGeneratorTests.cs` | NEW |
| `Assets/Tests/EditMode/Composition/WangContextResolverTests.cs` | NEW |
| `Assets/Tests/EditMode/Composition/CompositionPainterIntegrationTests.cs` | NEW |

**EditMode test temp assets** must write under `Assets/TempTests/Composition/...` and clean up via `AssetDatabase.DeleteAsset` in `[TearDown]`. Never pollute production paths.

---

## 4. Forbidden — Sprint 12+ Scope

Codex must NOT implement any of the following in Sprint 11:

- `PropDefinitionSO` or any Props Mode code (Sprint 12)
- Bridson Poisson sampler integration (Sprint 12 Props use)
- Density mask sampling beyond existing `FeatureMaskSO` (Sprint 12)
- SpriteAtlas integration or per-biome packing (V2 polish)
- AI tag suggestion (any form)
- Auto-Dress integration with composition map (V1.5 maybe)
- Markov clustering or sub-template rooms (deferred)
- **FocalCluster authoring tools** (V1.5 — V1 emits FocalCluster only if marker authored manually, V1 generator skips this role)
- **Integration with painters OTHER than WallOverlayPainter** (V1.5: DetailDecalPainter / AccentPainter / TransitionBrushPainter)
- Editor window or visualizer UI for CompositionRoleMap (V1.5)

Violation of this list = immediate review fail.

---

## 5. Exit Criteria

| # | Criterion | How to verify |
|---|---|---|
| EC-1 | `dotnet build` PASS, zero new errors | CI output |
| EC-2 | All existing Brush V1 + Sprint 10 tests still green | `dotnet test` |
| EC-3 | CompositionRoleMap generates correct role grid for 10×10 room with 1 door | `CompositionRoleMapGeneratorTests` PASS |
| EC-4 | DoorSafety radius 3 overrides WallBand at door position | EC-3 sub-test |
| EC-5 | 4×4 minimal room: no DecoratedEdge (gracefully degrades) | EC-3 sub-test |
| EC-6 | WangContextResolver returns correct case for L-shape corner | `WangContextResolverTests` PASS |
| EC-7 | WangContextResolver picks matching variant from candidate list | EC-6 sub-test |
| EC-8 | WallOverlayPainter context-aware placement: WallBand → sprite, CleanCenter → no sprite | `CompositionPainterIntegrationTests` PASS |
| EC-9 | No runtime non-integer scale anywhere in Sprint 11 code | Code review |
| EC-10 | No editor-only class referenced from runtime assembly (`CompositionRoleMap` + Generator + Resolver are runtime-safe) | Compile with UNITY_EDITOR stripped — no error |

**Vertical loop sentence:** "10×10 room → CompositionRoleMap generated → WallBand cells painted with Wang-context-aware variants matching neighbor walls → CleanCenter cells skipped." If broken, Sprint 11 not done.

---

## 6. Open Questions — RESOLVED (Opus signoff 2026-05-16)

All 4 OQs LOCKED with recommended values. Codex implements per these resolutions — no further signoff needed.

| # | Question | Resolution | Status |
|---|---|---|---|
| OQ-1 | CompositionRoleMap storage | **Runtime-generate** each load (~1ms negligible, no asset bloat). RoomTemplateSO does NOT cache. | ✅ RESOLVED |
| OQ-2 | Wang case encoding | **4-bit corner mask** matching Sprint 9 LIVE (NE-NW-SE-SW order, format `wang_NESW`). | ✅ RESOLVED |
| OQ-3 | Wall band thickness | **1 tile WallBand + 2 tiles DecoratedEdge** (combined visual = 3-tile wall zone). | ✅ RESOLVED |
| OQ-4 | Door safety radius | **3 tiles** centered on door socket position. | ✅ RESOLVED |

---

## 7. Review Checklist (Codex Review Pass)

When implementation submitted, Codex reviews:

- [ ] All Sprint 11 exit criteria (§5) verified — no partial credit
- [ ] **CompositionRoleMap is RUNTIME-SAFE** (no UnityEditor namespace ref in non-Editor files)
- [ ] CompositionRoleMapGenerator is **deterministic** (same RoomTemplate → same role grid every call)
- [ ] WangContextResolver case encoding matches Sprint 9 BrushAtlasImporter output exactly (`wang_NESW` 4-bit, no drift)
- [ ] WallOverlayPainter existing methods **UNCHANGED** (only new overload added; backward-compat preserved)
- [ ] EditMode tests use temp paths under `Assets/TempTests/Composition/...` and clean up in `[TearDown]`
- [ ] No painter OTHER than WallOverlayPainter modified
- [ ] All §6 open questions answered or explicitly deferred with fallback
- [ ] No forbidden-list (§4) item appears anywhere in Sprint 11 code (grep verification)

---

## 8. References

- `STAGING/sprite_strategy_FINAL_LOCK.md` §5 Natural Engine (composition roles primary model)
- `STAGING/codex_brush_sprint10_room_template_bank.md` (Sprint 10 context, §4 forbidden listed Sprint 11+ items)
- Sprint 9 spec: `STAGING/codex_brush_sprint9_atlas_importer.md`
- Memory: [[natural-paint-integration]] [[karar-143-layered-pipeline]] [[room-library-architecture]] [[brush-tool-v1-design]]
- Sprint 12 forward ref: `PropDefinitionSO` will consume `CompositionRoleMap.GetRoleAt(pos)` to gate prop placement

---

## 9. V1.5 + V2 Roadmap (NOT Sprint 11 scope, informational only)

**Sprint 11.5 (defer 1 mini-sprint):**
- DetailDecalPainter + AccentPainter + TransitionBrushPainter consume CompositionRole
- DecoratedEdge → high density, CleanCenter → low density mapping
- FocalCluster authoring marker integration

**Sprint 12 (Props Mode):**
- PropDefinitionSO + Props painter
- Bridson Poisson sampler for natural prop distribution
- Density-aware prop placement using CompositionRoleMap roles
- Footprint collision check (props don't overlap enemy spawns)

**Sprint 13+ (Polish):**
- SpriteAtlas integration per biome
- Wang adjacency editor visualizer
- AI tag suggestion (deferred — template-based + Preview override per Karar #80)

---

*End of Sprint 11 spec. Codex review dispatch (16-18 May window): rima-codex agent. After 19 May: cx_dispatch.py.*
