<!-- Source: rima-design (Opus) verdict 2026-05-17. Codex review pending. -->

# RIMA Fluid Transition Architecture — Design Verdict

## Top-line decision

**Adopt a hybrid "zones + stamps + stacked tiles" model.** Wang16 stays locked to elevation/feature boundaries only (Karar #143-B/C honored). All same-elevation visual variation comes from three orthogonal systems: (1) **manual oval brush** for hero/authored decoration, (2) **zone-driven auto-scatter** for procedural density gradients, (3) **adjacency rules** for edge decoration (LDtk auto-layer equivalent). These three feed a unified `DecalPlacementBuffer` consumed by `MapLayerOrchestrator` at compose time. This preserves Brush V1's deterministic seed-based composition while adding the fluid look.

**SYSTEMS AFFECTED:** Brush V1 (extend, do not rewrite), MapLayerOrchestrator, PatchAtlasSO, RoomTemplateSO, new SOs (ZoneMaskSO, AdjacencyRuleSO, RoomVisualProfileSO, LightingProfileSO, TerrainDefinitionSO), Editor window stack.

**CONFLICTS WITH LOCKED RULES?** NONE. Wang16 remains elevation-only. PPU=32 base / 64px chibi sprites preserved. Karar #143 6-layer pipeline extended to L0–L11 (additive, not contradictory).

---

## 1. Manual Oval Brush Stroke Tool

### Decision
Build `BrushStrokeWindow` as a **separate EditorWindow** that operates on the active `RoomTemplateSO` through a dedicated `DecalLayerSO` per layer (L4/L5/L6/L7). Brush strokes write to a **persistent decal list** on RoomTemplateSO, not to a tilemap. At room paint time, MapLayerOrchestrator reads the decal list and instantiates sprites with full transform jitter.

### Why this and not alternatives
- **Not Scene-view handles**: drag-paint on a baked tilemap loses authoring intent (you can't tweak stroke density after). Persistent decal list is editable and re-composable.
- **Not auto-only**: artist needs hero placements for arena focal points (Hades does this — every arena has 2-3 authored decoration anchors).
- **Not Tile-based**: decals are sprites with rotation+scale+flip jitter; Tile assets enforce grid alignment that kills the organic look (this is the empirically-validated Phase 0 finding).

### Editor UI
```
[Active Layer Dropdown]  L4 Organic ▼   (L4/L5/L6/L7)
[Decal Pool]             ┌─ MossPatch_A (×12 variants)
                         ├─ MossPatch_B (×8)
                         └─ [Multi-select with weights]
[Brush Size]             ●────────○─── 64px radius
[Spacing]                ●──○────────── 0.4 (interval / radius)
[Falloff]                Constant / Linear / Gauss
[Opacity / Density]      ●────○──────── 0.75
[Jitter] Rotation: ±180°  Scale: 0.8–1.2  FlipX ☑  FlipY ☑
[Sorting Order Range]    -2 … +2  (added to layer base)
[Mode]                   Add / Erase / Smudge(rotate-in-place)
[Stroke Sample]          Poisson / Random  (Poisson recommended)
[Snap to grid]           ☐    (off = fully organic; on = 8px snap optional)
[Undo Stack]             16 deep
```

### Drag-to-paint logic
- Sample pointer at fixed `Time.unscaledDeltaTime` intervals.
- Maintain `lastPlacedWorldPos`; place next decal when cursor has moved `spacing * radius` from last placement (resolution-independent).
- Within brush disk, pick placement via **Poisson disk** (with `min-distance` from PatchAtlasSO) for organic spread; fall back to weighted random if Poisson budget exceeded.
- Each placement: weighted decal pick + rotation jitter + scale jitter + flipX/Y dice + sortingOrder dice within range.

### C# skeleton
```csharp
public class BrushStrokeWindow : EditorWindow {
  RoomTemplateSO target;
  DecalLayer layer;            // L4/L5/L6/L7
  PatchAtlasSO pool;
  BrushSettings settings;      // size, spacing, jitter, opacity
  DecalStrokeBuffer activeStroke;
  void OnGUI() { /* sliders, pool picker, mode toggle */ }
  void OnSceneGUI(SceneView v) { /* drag handler -> EmitPlacement */ }
  void EmitPlacement(Vector2 worldPos) { /* Poisson pick + jitter */ }
  void CommitStroke() { Undo.RecordObject(target,"BrushStroke"); target.decals.AddRange(activeStroke.placements); }
}

[Serializable] public struct DecalPlacement {
  public string decalId;       // resolves to sprite via PatchAtlasSO
  public Vector2 localPos;     // room-space, sub-grid precision
  public float rotationDeg;
  public Vector2 scale;
  public bool flipX, flipY;
  public int sortingOrderDelta;
  public DecalLayer layer;     // L4..L7
}

public class BrushStrokeManager {
  // Headless API for tests + procedural callers
  public static List<DecalPlacement> EvaluateStroke(Vector2 from, Vector2 to,
      BrushSettings s, PatchAtlasSO pool, int seed);
}
```

### Undo/redo
Use `Undo.RecordObject(roomTemplate, "BrushStroke")` per **committed stroke** (mouse-up), not per placement — keeps undo stack readable. `EditorUtility.SetDirty` on commit.

### Integration with allowFlipX/Y/rotation/scale jitter
PatchAtlasSO entries already carry these flags (Karar #143-M..R). BrushSettings can **override** per-stroke (e.g., artist temporarily forces no-flip for a directional decal). Override is stroke-local, not persisted to PatchAtlasSO.

**TRADE-OFF:** Persistent decal list grows RoomTemplateSO file size. Mitigation: cap ~500 decals per room (Hades arenas don't exceed this); if needed, bake to a sprite atlas at room finalization (Phase 2).

---

## 2. Zone-Based Density Auto-Scatter

### Decision
`ZoneMaskSO` is **per-room**, not per-biome. Stored as a 2D byte array sized to room dimensions. Zones are **enumerated** (`Center`, `Edge`, `WallProximity`, `FeatureProximity`, `Custom_A..D`), not arbitrary numeric. PatchAtlasSO gains a `densityPerZone` dictionary so the same decal pool can scatter with different densities per zone.

### Auto-derived zones (no painting required)
At room template load:
- `WallProximity`: distance ≤ 2 cells from any non-walkable cell
- `Edge`: distance ≤ 1 cell from room bounding rect
- `FeatureProximity`: distance ≤ 3 cells from any prop with `isFeatureAnchor=true`
- `Center`: everything else

These are **computed**, not stored — the SO only stores `Custom_A..D` and overrides.

### Manual zone painting
Brush V1 gets a "Paint Zone" mode (same brush UI, different commit target). User paints `Custom_A` over a quadrant; PatchAtlasSO entries referencing `Custom_A` scatter there.

### PatchAtlasSO extension
```csharp
[Serializable] public struct ZoneDensity {
  public ZoneId zone;          // enum: Center/Edge/WallProx/FeatureProx/Custom_A..D
  public float density;        // placements per cell (e.g., 0.05 = 5%)
  public float minDistance;    // overrides global min-distance per zone
  public bool edgeBiased;
}

public class PatchAtlasSO : ScriptableObject {
  public List<PatchEntry> entries;
}

[Serializable] public class PatchEntry {
  public string id;
  public Sprite[] variants;
  public List<ZoneDensity> zoneDensities;   // NEW
  public float globalMinDistance;
  public Vector2Int sortingOrderRange;
  public bool allowFlipX, allowFlipY;
  public Vector2 rotationRangeDeg;
  public Vector2 scaleRange;
}
```

### Compose-time logic
For each room cell, MapLayerOrchestrator:
1. Resolves cell's zone (auto + custom mask layered).
2. For each enabled PatchEntry, looks up `zoneDensities[zone]`.
3. Rolls `density` against seeded RNG → maybe place.
4. Respects `minDistance` against already-placed decals (KDTree or grid bucket).

**TRADE-OFF:** Density-based scatter at compose time costs CPU on room enter. Mitigation: pre-bake scatter results into RoomTemplateSO's decal list when room is "finalized" (editor button). Runtime rooms use cached list; live-editing rooms regenerate.

---

## 3. Auto-Border Decoration (LDtk Auto-Layer Rule Equivalent)

### Decision
Implement `AdjacencyRuleSO` with a **3×3 kernel pattern** language. Each rule = (pattern, target decal, density, layer, sortingOrder). Rules evaluate against walkability grid (or any IntGrid layer). Symmetric variants are auto-generated at SO save time (90°/180°/270°/mirror).

### Why 3×3 kernel and not Wang-style 4-corner
- LDtk's actual rules go up to 7×7. We start at 3×3 (sufficient for "wall on north → moss on south interior cell"). Upgrade to 5×5 if needed Phase 2.
- 3×3 covers all 256 boolean adjacency cases; enough for organic edge decoration.
- Wang16 corner masks are reserved for elevation boundaries (Karar #143-B/C). Don't conflate.

### Rule definition
```csharp
[Serializable] public class AdjacencyRule {
  public string ruleId;
  public CellPredicate[] kernel;    // 9 entries: WalkableYes/No/Any
  public string targetDecalId;      // PatchAtlasSO entry
  public float chance;              // 0..1
  public DecalLayer layer;          // L4/L5/L6/L7
  public Vector2Int sortingOrderRange;
  public bool autoSymmetric;        // generate 4 rotations + 4 mirrors
  public int priority;              // higher wins on conflict
}

public class AdjacencyRuleSO : ScriptableObject {
  public List<AdjacencyRule> rules;
  // OnValidate: expand autoSymmetric rules into runtime list
}
```

### Evaluation
At compose, for each cell:
1. Sort rules by `priority` desc.
2. For each rule, check 3×3 kernel against grid.
3. First match (or all matches up to `maxRulesPerCell=3`): roll `chance`, place decal at cell center with full jitter.

### Symmetric rule generation
On SO `OnValidate`, if `autoSymmetric=true`, generate 7 additional rotations/mirrors and store in a `runtimeRules` non-serialized list. Editor shows source rule only.

**TRADE-OFF:** Rule conflicts are possible (two rules want the same cell). Decision: priority wins, ties broken by seeded RNG. Document this clearly so artists don't fight invisible precedence.

---

## 4. Layer L9 Shadows

### Decision
**Sprite-based shadows for L9, no runtime URP shadow caster.** Each prop has an optional `shadowSprite` (ellipse blob) instanced at `prop.position + (0, -0.1)`, layer L9, sortingOrder one below prop. Wall contact shadows are baked into WallKit modules (no runtime cost). Cliff shadows deferred to Phase 2.

### Why sprite-based and not URP `ShadowCaster2D`
- URP 2D ShadowCaster2D requires a 2D Light to cast against. We want **always-on grounding shadow**, regardless of lighting. Sprite blob is unconditional.
- ShadowCaster2D performance scales with light count × caster count. With 100 props and 5 lights = 500 caster evaluations per frame. Sprite blob = static draw.
- Aesthetic match: Hades / CrossCode use baked blob shadows for prop grounding; URP shadows for actor + dynamic lights.

### Architecture
```csharp
public class PropDefinitionSO : ScriptableObject {
  // existing
  public Sprite shadowSprite;       // NEW: optional, ellipse blob
  public Vector2 shadowOffset;      // NEW: usually (0, -0.08)
  public Vector2 shadowScale;       // NEW: usually (1.1, 0.4)
  public float shadowAlpha;         // NEW: 0.35 default
}

public class ShadowRenderer : MonoBehaviour {
  // Auto-instantiated by PropRuntimeSpawner when shadowSprite != null
  // Single SpriteRenderer child, no Update loop, static after spawn
}
```

### Wall contact shadow
Bake into WallKit module sprites themselves — every wall sprite includes a ~3px dark gradient extending onto the adjacent floor cell. Saves runtime cost; loses dynamic light response (acceptable trade for Phase 1).

### Sorting order
L9 shadows always render at `propSortingOrder - 1` so prop occludes its own shadow correctly when player walks behind.

**TRADE-OFF:** Shadows don't respond to dynamic light direction. Acceptable — Hades doesn't either. Phase 2 can add directional sprite swap (3-4 angles) if needed.

---

## 5. Layer L10 Lighting Helpers

### Decision
URP 2D `Light2D` per emitter prop, configured via `LightingProfileSO` referenced from `RoomVisualProfileSO`. Global ambient set by a single `GlobalLight2D` per room driven by RoomVisualProfileSO. No per-cell lighting.

### Architecture
```csharp
public class LightingProfileSO : ScriptableObject {
  public Color globalAmbient;           // e.g., #2A2535 dark dungeon
  public float globalIntensity;         // 0.35 (matches existing lighting WIP)
  public List<EmitterProfile> emitters; // brazier/candle/rift/torch
}

[Serializable] public class EmitterProfile {
  public string id;                      // matches PropDefinitionSO.lightingProfileId
  public Color color;
  public float intensity;
  public float outerRadius;
  public float innerRadius;
  public Light2D.LightType type;         // Point / Freeform
  public bool flicker;                   // candle/torch
  public Vector2 flickerRange;           // intensity min/max
  public float flickerHz;
}

public class RoomVisualProfileSO : ScriptableObject {
  public Sprite backgroundTier_B;        // for Type B arenas
  public Color floorTone;                // L1 wash
  public LightingProfileSO lighting;
  public AdjacencyRuleSO adjacencyRules;
  public Material floorMaterial;         // optional palette LUT
}
```

### Per-prop wiring
`PropDefinitionSO` gains `lightingProfileId` string. `PropRuntimeSpawner` instantiates Light2D child when set, configures from EmitterProfile.

### Global ambient
Single `GlobalLight2D` at room root, configured from `LightingProfileSO.globalAmbient/Intensity`. Replaces existing 0.35 hardcoded value.

**TRADE-OFF:** Many lights = fillrate cost. URP 2D Renderer batches but soft-caps at ~16 active lights per camera frame. Mitigation: emitter prop count budget = 8 per room. Document budget in RoomVisualProfileSO with a validator.

---

## 6. Tile Stacking (LDtk Pattern)

### Decision
Use **separate Tilemaps per layer**, not multi-tile-per-cell on one Tilemap. Each layer (L2, L2b, L3, L4...) is its own Tilemap child under a single GridLayout root, with explicit sortingOrder offsets. Walkability is owned by L0 data (RoomTemplateSO.walkableGrid), never derived from visual stacking.

### Why separate Tilemaps and not stacked Tiles
- Unity Tilemap supports one tile per cell. "Stacking" via custom `Tile.GetTileData` flags is brittle and costs introspection.
- Separate Tilemaps = clean sorting, clean culling, separate materials (palette LUT per layer), independent enable/disable for debug.
- Performance: 6-8 Tilemaps with cells in 30×30 room = trivial draw call count after batching.

### Sorting convention
```
L1  BaseTone           sortingOrder = -200  (Background sorting layer)
L2  BaseTerrain        sortingOrder = -100
L2b FloorMacroVariant  sortingOrder = -90
L3  Wang16Borders      sortingOrder = -80   (elevation seams only)
L4  OrganicDecals      sortingOrder = -50
L5  DetailScatter      sortingOrder = -40
L6  AccentOverlays     sortingOrder = -30
L7  (reserved)         sortingOrder = -20
L8  PropClusters       sortingOrder = Y-sorted (Default sorting layer, dynamic)
L9  Shadows            sortingOrder = prop-1 (per-prop)
L10 Lighting           (Light2D, no sortingOrder)
L11 ManualOverrides    sortingOrder = +500 (always on top, authored)
```

L4-L7 are decals, not tiles — they live on separate sprite layers via `DecalLayerRenderer` (single GameObject parent with many SpriteRenderer children), not Tilemap.

### Walkability source of truth
Always L0 (`RoomTemplateSO.walkableGrid`). Decals are **purely visual**, never affect movement. Props with collision affect walkability via L8 (collider on instantiated prefab). This separation is critical — don't let visual layers leak into gameplay.

**TRADE-OFF:** Many GameObjects in scene (Tilemap × 5 + DecalLayer × 4 + PropParent + LightingRoot). Mitigation: Single `RoomRoot` prefab with all children pre-arranged; static batching where applicable.

---

## 7. Required SO Scaffolding — Phase 1 vs Deferred

### Phase 1A (MUST, before any PixelLab dispatch)
| SO | Purpose | Status |
|---|---|---|
| `TerrainDefinitionSO` | Floor terrain type (stone/grass/sand) → references sprite pool | NEW |
| `PatchAtlasSO` (extend) | + `zoneDensities` field | EXTEND |
| `PropDefinitionSO` (extend) | + `shadowSprite/Offset/Scale/Alpha`, `lightingProfileId`, `isFeatureAnchor` | EXTEND |
| `RoomVisualProfileSO` | Per-room visual recipe (tone/lighting/adjacency) | NEW |
| `ZoneMaskSO` | Custom zone painting | NEW |
| `AdjacencyRuleSO` | LDtk-equiv 3×3 rules | NEW |
| `LightingProfileSO` | Ambient + emitter profiles | NEW |
| `ImportAssetRole` (enum) | base/decal/detail/accent/wall/prop/lightSource | NEW |
| `JsonCoordinateOrigin` (enum) | TopLeft/BottomLeft for PixelLab JSON | NEW |

### Phase 1B (MUST, but after first asset import validates the pipe)
| SO | Purpose |
|---|---|
| `WallKitSO` (minimal) | straight/corner/doorway/pillar refs only |
| `TilesetGenerationSettings` | PixelLab dispatch params capture |

### Phase 2+ (DEFER)
- `TerrainTransitionGraphSO` — multi-terrain transitions (stone→grass→sand chains). Wait until Phase 1 proves single-terrain look.
- `DungeonRecipeSO` — meta-room composition. Wait until 20-30 room MVP exists.
- `PropClusterSO` — pre-composed prop stamps. Phase 1A can use ad-hoc clusters in RoomTemplateSO; promote to SO when patterns emerge.

### Decision logic
**Rule:** an SO ships in Phase 1A if its absence forces hard-coded magic numbers in the orchestrator. PropCluster doesn't (can be inline prop list). TerrainTransitionGraph doesn't (only one terrain in Phase 1A). WallKit minimal does (Type A rooms need walls).

---

## 8. PixelLab Dispatch List — Phase 1A

### Verdict on ChatGPT's list
**Mostly correct, two adjustments.**

### Refined dispatch (Phase 1A, single biome = StoneDungeon)
| # | Tool | Subject | Output expected |
|---|---|---|---|
| 1 | `create_tiles_pro` | Stone floor base (clean variants) | 6-8 floor variants 32×32 |
| 2 | `create_tiles_pro` | Stone floor — damaged/cracked variants | 4-6 variants 32×32 |
| 3 | `create_object` n=16 | Moss patches organic | 12-15 picks 32-64px irregular |
| 4 | `create_object` n=16 | Dirt/grime stains | 10-12 picks |
| 5 | `create_object` n=16 | Damp puddles | 8-10 picks |
| 6 | `create_object` n=16 | Pebble clusters | 10-12 picks |
| 7 | `create_object` n=16 | Floor cracks (organic, not Wang) | 8-10 picks |
| 8 | `create_object` n=16 | Bone fragments / dust | 8-10 picks |
| 9 | `create_object` n=8 | Rift scar accent (large) | 2-3 picks |
| 10 | `create_object` n=8 | Ritual mark accent | 2-3 picks |
| 11 | `create_object` n=8 | Wall kit — straight segment | 1-2 picks |
| 12 | `create_object` n=8 | Wall kit — outer corner | 1-2 picks |
| 13 | `create_object` n=8 | Wall kit — doorway | 1-2 picks |
| 14 | `create_object` n=8 | Wall kit — pillar | 1-2 picks |
| 15 | `create_object` n=16 | Brazier prop (with lit/unlit) | 3-4 picks |
| 16 | `create_object` n=16 | Wooden crate variants | 4-6 picks |
| 17 | `create_object` n=16 | Skull pile / debris cluster | 4-6 picks |

**Total: 17 dispatches.** ChatGPT's count matches; my refinements:
- **Split floor into clean + damaged** (#1, #2). One pool = 6-8 variants is not enough for 30×30 room without visible repetition.
- **Add brazier explicitly** (#15) — needed to validate L10 lighting in Phase 1A; without it lighting work is theoretical.
- **Walls remain 4 modules minimum** (#11-14). Phase 1 doesn't need topcap/frontface/niche/damaged.

### Critical pre-dispatch tasks
- Lock palette LUT (single shared palette across all 17 dispatches; PixelLab supports style image reference).
- Lock 32×32 base grid for tiles, 32-128px for decals/props.
- Lock pixel-art rendering directive in every prompt (no painterly drift — Tier A foreground).
- For Type B atmospheric backgrounds (Hades-style distant scenery): **separate dispatch batch, Phase 1B**, not part of this 17.

---

## 9. Implementation Order

### Decision: serial-with-early-validation, not parallel
The pipe has too many unknowns to parallelize. Order:

```
[STEP 1] SO scaffolding — Phase 1A SOs created, validated, empty
         |  Verifies: SO design compiles, inspectors usable, serialization clean
         |  Risk gate: if SO design is wrong, dispatch wastes budget
         v
[STEP 2] Pipe-validation dispatch — ONE tile + ONE decal + ONE prop only
         |  3 PixelLab calls
         |  Import to TerrainDefinitionSO + PatchAtlasSO + PropDefinitionSO
         |  Compose minimal 10×10 room in editor
         |  Verdict gate: does it LOOK fluid? Style match? Palette right?
         v
[STEP 3] If pipe verdict = GO, dispatch remaining 14 calls
         |  If pipe verdict = NO-GO, adjust prompts/SOs/import, redo Step 2
         v
[STEP 4] Import all 17 dispatch results
         |  ImportAssetRole tagging
         |  Wang16 metadata (for walls only, Karar #143-B/C honored)
         |  Density/zone wiring in PatchAtlasSO
         v
[STEP 5] Compose 1 reference room — Type A enclosed dungeon
         |  All 11 layers active
         |  Brush V1 manual decal pass (artist authoring)
         |  Visual verdict gate
         v
[STEP 6] Compose 1 reference room — Type B Hades arena
         |  Validate atmospheric background works with same foreground assets
         |  Visual verdict gate
         v
[STEP 7] Brush V1 extension features (Sections 1-3) coded
         |  Manual oval brush, zone painting, adjacency rules
         v
[STEP 8] 5 additional rooms — variety test
         |  Same assets, different recipes — verifies no visible repetition
         v
[STEP 9] L9 shadows + L10 lighting integration
         |  Per-prop shadow sprites, brazier Light2D
         v
[STEP 10] 20-30 room MVP per existing RoomBank target
```

**Why pipe-validation gate (Step 2) is non-negotiable:** Each PixelLab call costs budget + time. A 17-call batch that returns the wrong style = full re-prompt + re-dispatch. The 3-call gate spends ~18% to validate 100%.

**Parallelizable subset:** Steps 7 (Brush V1 extensions) can run parallel to Steps 4-6 (asset import + compose) because they touch different files. Orchestrator can spawn rima-codex for Brush V1 work while artist iterates on compose.

---

## 10. Risk Analysis

### Risk 1: PixelLab style drift between dispatches
**Impact:** Floor #1 and floor #2 don't match palette/style → fragmented look.
**Mitigation:** Lock single style reference image, pass to every dispatch via PixelLab style image param. Reject any output that drifts on palette match (eyeball check, no automation Phase 1A).
**Fallback:** Re-prompt drifting dispatch only, not full batch.

### Risk 2: Decal density tuning is hand-crafted hell
**Impact:** Every room needs manual density tweaking → doesn't scale to 30 rooms.
**Mitigation:** RoomVisualProfileSO carries default densities per zone. Rooms inherit profile; only outliers override. AdjacencyRuleSO does the heavy lifting automatically.
**Fallback:** If profile inheritance fails, bake per-room density to RoomTemplateSO and accept manual authoring cost.

### Risk 3: Sprite-based shadow looks dated
**Impact:** Modern eye expects dynamic shadows; flat blobs read as PS1.
**Mitigation:** Use soft-edge ellipse with multiply blend at 35% alpha; bake subtle directional gradient. Validate on reference room before scaling.
**Fallback:** Upgrade to URP ShadowCaster2D for hero props only (3-5 per room), keep blobs for clutter. Phase 2 decision.

### Risk 4: 3×3 adjacency rules insufficient for organic look
**Impact:** Rules feel grid-locked, defeats fluid intent.
**Mitigation:** Combine adjacency rules (broad zones) with manual brush (hero detail) — neither alone is sufficient; together they cover. Adjacency = base layer, brush = polish.
**Fallback:** Expand to 5×5 kernel Phase 2. If still rigid, drop adjacency and lean fully on zones + brush.

### Risk 5: Type B Hades arena needs assets not in Phase 1A dispatch
**Impact:** Atmospheric background bake adds 5-10 more dispatches outside budget.
**Mitigation:** Phase 1A explicitly validates **Type A only** (enclosed dungeon). Type B is Phase 1B with separate budget. Don't conflate.
**Fallback:** If Type B is critical for early playtest, dispatch 2-3 background pieces in Phase 1A as exploratory; do not commit full Hades bake until foreground pipe is locked.

### Risk 6: ScriptableObject schema churn breaks existing 321 tests
**Impact:** Brush V1 tests fail after PatchAtlasSO/PropDefinitionSO extensions.
**Mitigation:** Additive fields only. New fields have safe defaults (zoneDensities = empty list = legacy behavior, shadowSprite = null = no shadow). Run full test suite after every SO edit; require rima-qc review before commit.
**Fallback:** Version PatchAtlasSO with `[FormerlySerializedAs]` if any field renames are unavoidable.

### Risk 7: Sub-grid decal positions break determinism
**Impact:** Same seed produces different layouts on different runs → tests flaky.
**Mitigation:** All sub-grid placements seeded through single `System.Random` instance per room, passed through compose pipeline. No `UnityEngine.Random` in placement code (banned by convention). Add determinism test: compose room twice with same seed, assert decal list byte-equal.
**Fallback:** Quantize sub-grid to 8px steps if determinism issue resists fixing — loses ~50% organic precision but keeps tests stable.

### Risk 8: Editor window count proliferation
**Impact:** Brush V1 already exists; adding BrushStrokeWindow + ZoneMaskWindow + AdjacencyRuleWindow = 4 windows artist must juggle.
**Mitigation:** Single `RoomComposerWindow` with tabbed sections: [Tile Paint] [Brush Decals] [Zones] [Adjacency Rules] [Props] [Lighting]. One window, mode switch, shared room context.
**Fallback:** Keep separate windows but add a "Room Composer" dockable layout preset.

---

## Concrete next-step recommendations

1. **Spawn rima-doc** with this entire response and instructions to write `STAGING/RIMA_FLUID_TRANSITION_DESIGN.md` matching the 10-section format. Doc-writer can expand voice and prose; do not let it second-guess the decisions above.
2. **After doc lands, spawn rima-qc** for Codex review pass on the design doc.
3. **Step 1 of Section 9 — SO scaffolding — is the next codex-task.** Orchestrator should prepare a CODEX_TASKS.md entry with the 9 Phase 1A SOs (see Section 7 table). Allowed file paths: `Assets/Scripts/Rima/MapDesigner/SO/*.cs` and tests under `Assets/Tests/EditMode/MapDesigner/SO/*Tests.cs`. Strict additive policy on existing SOs (PatchAtlasSO, PropDefinitionSO).
4. **Do not dispatch PixelLab Phase 1A until Step 1 SOs are in trunk and test suite passes 321+N green.**
5. **Step 2 pipe-validation dispatch (3 calls) is the first PixelLab task.** Orchestrator should schedule that as a separate user-confirmed action — auto-dispatch banned per `feedback_pixellab_mcp.md`.

---

**ORCHESTRATOR NEXT STEP:** Spawn rima-doc with this full text + instruction to write `STAGING/RIMA_FLUID_TRANSITION_DESIGN.md` preserving all 10 sections, decisions, code skeletons, and tables. Then queue rima-qc review. Then queue Phase 1A SO scaffolding codex-task.