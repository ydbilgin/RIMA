# RIMA Room Designer — Master Spec v3
**Status:** PROPOSED 2026-05-14 (Karar #134 LOCK pending user ratification)
**Supersedes context:** Karar #131 (Corner Wang Pipeline), Karar #132 (Map Designer), ChatGPT spec 1, ChatGPT spec 2
**Authors:** Claude (Opus 4.7) synthesizing ChatGPT v1+v2 + today's PixelLab test session

---

## §0 — Paradigm Lock: Procedural + Polish (NOT manual paint)

Roguelite scope requires 100+ rooms per run. Manual paint scales to ~20 rooms; beyond that, designer effort collapses. Genre-standard solution: **seed-deterministic procedural generation + selective lock-and-regenerate manual polish.**

- **PixelLab = Asset Factory** (Pro UI manual + MCP automated). Generates tiles, transitions, decals, props.
- **Unity Room Designer = Procedural Generator + Polish Editor + Source-of-Truth.** Owns final editable room data. Never treats PixelLab map export as editable source.
- **Output:** RoomAsset (SO) — seed + recipe + polish overrides. Reproducible, diffable, version-controllable.

**Anti-pattern hard rule (from ChatGPT spec 2 §9):**
> Claude/Codex must build the GENERATOR and reusable rules — not write tile coordinates by hand. Output must be seed-based, deterministic, and manually polishable.

---

## §1 — Asset Pipeline: 3 PixelLab APIs → 5 layer roles

Each PixelLab generation API maps to one or more layer-role pools. None is locked to a single layer.

| PixelLab API | Output | Layer-role pools fed | Generation source |
|---|---|---|---|
| `create_topdown_tileset` (4×4 standard) | 16-tile Wang autotile | Base Tilemap (cross-family terrain boundary) | MCP_Standard or ProUI_Manual |
| `create_topdown_tileset` (4×8 transition, raggedness 100%) | 23-tile blend strip | PatchAtlas / Edge Naturalization (Phase 2) | ProUI_Manual only |
| `create_tiles_pro` | N independent tiles | Base-variant pool / Patch pool / Decal candidate / Scatter pool — **any** | MCP only |
| `create_map_object` | Single transparent-corner sprite | Decal Layer / Prop Cluster member / Patch Layer organic blob | MCP only |
| PixelLab Map Editor ZIP export | composite + metadata | Reference only — NOT editable source | Manual import |

**Note (test finding F4 expanded by ChatGPT correction #4):** `create_tiles_pro` is multi-use — base variant scatter (interior anti-repeat), patch overlay (mid-size organic), decal candidate (small detail), scatter pool member. Importer should tag each `create_tiles_pro` output with `intendedRole: enum[BaseVariant, Patch, Decal, Scatter]` at import time.

---

## §2 — Core Data Structures

### §2.1 TerrainDefinition (ScriptableObject)
```csharp
[CreateAssetMenu]
public class TerrainDefinition : ScriptableObject {
    public TerrainId terrainId;              // enum / int
    public string displayName;
    public Sprite baseTile;                  // single 32×32, MUST be present
    public VisualCategory visualCategory;    // Stone / Dust / Water / Vegetation / Crystal / Hazard
    public Color paletteSwatch;              // UI swatch for low-zoom view

    [Header("Gameplay")]
    public bool walkable;
    public bool blocksMovement;
    public int elevationLevel;               // 0 = floor, 1 = raised, 2 = wall, -1 = pit
    public CollisionType collisionType;      // None / SolidBlock / TraversableSlope / Hazard

    [Header("Procedural defaults")]
    public float defaultScatterDensity;      // 0-1; auto-scatter density when terrain is painted
    public float defaultDecalDensity;        // 0-1; auto-decal density
    public ScatterPool scatterPool;          // → list of (Sprite, weight)
    public DecalPool decalPool;              // → list of (Sprite, weight)

    [Header("Prompt anti-drift (S67 lesson)")]
    [TextArea(3,5)] public string basePromptTemplate;  // positive-only, no NO-bombardment
    public string saturationHint;            // "muted dusty washed-out" anchor
}
```
**Critical (test finding F5):** `basePromptTemplate` must be pure positive language. Never include "NO STONES NO BLOCKS NO SLABS" — confuses PixelLab AI across batch generations.

### §2.2 TerrainTransitionGraph (ScriptableObject)
```csharp
public class TerrainTransitionGraph : ScriptableObject {
    public List<TransitionEdge> edges;

    public TransitionEdge Lookup(TerrainId a, TerrainId b);  // order-insensitive
    public bool Supported(TerrainId a, TerrainId b);
}

[Serializable]
public class TransitionEdge {
    public TerrainId lowerTerrainId;
    public TerrainId upperTerrainId;
    public Texture2D sourceTexture;          // 128×128 (or 128×256 for 4×8)
    public TextAsset sourceJson;             // PixelLab metadata
    public Sprite[] maskToTile;              // index = wang mask 0-15, value = sliced Sprite — O(1) lookup
    public GridLayout gridLayout;            // _4x4 / _4x8
    public WangIndexMapping wangIndexMapping; // Standard / Transition
    public bool supported;                   // false if asset missing → RED_X
    public string transitionDescription;
    public float transitionSize;             // 0.0-1.0
    public TilesetGenerationSettings generationSettings;
}
```

**Pair-local rule (ChatGPT spec 1+2 enforcement):** Lower/Upper is a per-edge visual role. Same terrain can be `lower` in one edge and `upper` in another. **Never store global upper/lower on TerrainDefinition.** Never infer walkability/blocking from upper/lower.

### §2.3 TilesetGenerationSettings (Serializable, captured manually at import)
```csharp
[Serializable]
public class TilesetGenerationSettings {
    public GenerationSource source;          // MCP_Standard / ProUI_Manual / Export_Import
    public DateTime generatedAt;
    public string pixelLabTilesetId;         // UUID from PixelLab

    [Header("Shape (Pro UI sliders — captured manually)")]
    public float transitionHeightPercent;    // 0-100
    public float transitionPercent;          // 0-100
    public float spreadPercent;              // 0-100
    public float raggednessPercent;          // 0-100

    [Header("Prompts (captured from MCP/UI)")]
    [TextArea(2,4)] public string promptLower;
    [TextArea(2,4)] public string promptUpper;
    [TextArea(2,4)] public string promptTransition;
    public string seed;                      // if reproducible
}
```
**Rationale (test finding F3 + ChatGPT correction #5):** PixelLab MCP metadata does NOT return Pro UI slider values. To reproduce/regenerate a tileset, settings MUST be captured at import time and stored on the edge. Source-of-truth = this struct on TransitionEdge, not PixelLab's response.

### §2.4 CornerField + ResolveVisualTile
```csharp
public TerrainId[,] cornerField; // size (width+1) × (height+1)

// For rendered cell at (x, y):
// NW = cornerField[x,     y + 1]
// NE = cornerField[x + 1, y + 1]
// SW = cornerField[x,     y    ]
// SE = cornerField[x + 1, y    ]

Sprite ResolveVisualTile(int x, int y) {
    var corners = SampleCorners(x, y);
    var distinct = corners.Distinct().Where(t => t != TerrainId.Empty).ToList();
    if (distinct.Count == 0) return null;
    if (distinct.Count == 1) return TerrainDefs[distinct[0]].baseTile;
    if (distinct.Count == 2) {
        var edge = transitionGraph.Lookup(distinct[0], distinct[1]);
        if (edge == null || !edge.supported) return RED_X_INVALID_TILE;
        int mask = ComputeWangMask(corners, edge.lowerTerrainId, edge.upperTerrainId);
        return edge.maskToTile[mask];
    }
    return RED_X_INVALID_TILE; // 3+ terrains; future: special junction stamps
}

int ComputeWangMask(Corners c, TerrainId lower, TerrainId upper) {
    int m = 0;
    if (c.NW == upper) m |= 8; else if (c.NW != lower) return -1;
    if (c.NE == upper) m |= 4; else if (c.NE != lower) return -1;
    if (c.SW == upper) m |= 2; else if (c.SW != lower) return -1;
    if (c.SE == upper) m |= 1; else if (c.SE != lower) return -1;
    return m;
}
```
Universal lookup (matches Karar #131): `mask = NW<<3 | NE<<2 | SW<<1 | SE<<0`.

### §2.5 RoomRecipe (ScriptableObject)
```csharp
[CreateAssetMenu]
public class RoomRecipe : ScriptableObject {
    public RoomType roomType;                // CombatArena / Corridor / Treasure / Shrine / Transition / Cliff / RuinedGarden
    public BiomePreset biomePreset;
    public Vector2Int dimensions;
    public int seed;

    [Header("Terrain composition")]
    public TerrainId mainTerrain;
    public TerrainId[] secondaryTerrains;
    public TerrainId wallTerrain;
    public TerrainId[] hazardTerrains;

    [Header("Layout")]
    public EntranceRules entranceRules;      // doors per side, min/max
    public float centerClearance;            // 0-1, reserved combat zone
    public float pathClearance;              // 0-1, walkable corridor width

    [Header("Dressing")]
    public float propDensity;
    public float decalDensity;
    public float scatterDensity;
    public float edgeRaggedness;
    public PropClusterReference[] allowedClusters;
    public PatchAtlas patchAtlas;
}
```

### §2.6 DungeonRecipe (ScriptableObject) — NEW (gap fill G1)
```csharp
[CreateAssetMenu]
public class DungeonRecipe : ScriptableObject {
    public string runName;
    public int floorNumber;
    public int seed;

    public RoomGraph roomGraph;              // nodes + edges + branch rules
    public DifficultyCurve difficultyCurve;  // room index → enemy budget
    public RoomTypeMix typeMix;              // weights per room type per floor depth

    public RoomRecipe[] roomRecipePool;      // selectable pool for this dungeon
    public PortalRecipe[] portalRecipes;     // door, ladder, rift gate
}
```
Solves: room→room composition. RoomRecipe owns single room; DungeonRecipe owns the chain.

### §2.7 PatchAtlas (ScriptableObject) — NEW (gap fill G3)
```csharp
[CreateAssetMenu]
public class PatchAtlas : ScriptableObject {
    public TerrainId hostTerrain;            // patches authored for this terrain
    public PatchEntry[] entries;
}

[Serializable]
public class PatchEntry {
    public Sprite patchSprite;               // from create_map_object or 4×8 transition slice
    public PatchSize size;                   // Small / Medium / Large
    public PatchShape shape;                 // Oval / Irregular / Strip
    public float weight;                     // selection probability
    public float rotationVariance;           // 0-360 random rotation
    public Vector2 scaleRange;               // 0.8-1.2 random scale
    public PatchPlacement placement;         // EdgePreferred / Random / CenterAvoid
}
```
Sources: `create_map_object` ovals (current bugünkü cream drift hazır), `create_tiles_pro` flat variants (intendedRole=Patch), 4×8 transition slices (Phase 2).

### §2.8 PropCluster (ScriptableObject) — NEW (gap fill G4)
```csharp
[CreateAssetMenu]
public class PropCluster : ScriptableObject {
    public string clusterName;               // "BrokenPillar+Rubble", "MossyRockGroup", "CrystalBloom"
    public ClusterMember[] members;
    public Vector2Int boundingBoxCells;      // collision footprint in cells
    public Vector2 ySortPivot;               // relative to bbox bottom-left
    public TerrainId[] validHostTerrains;    // whitelist
    public bool blocksMovement;
}

[Serializable]
public class ClusterMember {
    public GameObject prefab;                // OR Sprite + SpriteRenderer setup
    public Vector2 relativeOffset;           // from cluster origin
    public float rotationDeg;
    public int sortingOrderOffset;
}
```

### §2.9 RoomAsset (ScriptableObject — runtime output)
```csharp
[CreateAssetMenu]
public class RoomAsset : ScriptableObject {
    public RoomRecipe sourceRecipe;
    public int generationSeed;
    public TerrainId[,] cornerFieldSnapshot;
    public TileOverride[] manualOverrides;
    public BoundsInt[] lockedRegions;
    public DecalPlacement[] manualDecals;
    public PropPlacement[] manualProps;
    public DateTime lastEdited;
}
```
This is the editable source-of-truth. Recipe + seed defines procedural baseline; overrides/locked/manual additions are polish on top.

---

## §3 — 9-Stage Generation Pipeline

Each stage is an independent function on `GenerationContext`. Stages are skippable, lockable, re-runnable per-region.

| Stage | Function | Reads | Writes | Skippable | Region-scoped |
|---|---|---|---|---|---|
| A. Layout | `BuildLayout(recipe)` | RoomRecipe | shape mask, entrances, gameplay path | No | No |
| B. Terrain | `PaintTerrain(layout)` | layout, recipe.terrains | cornerField | No | Yes |
| C. PixelLab Resolve | `ResolveVisuals(cornerField)` | cornerField, TransitionGraph | Base Tilemap | No | Yes (dirty rect) |
| D. Edge Naturalize | `Naturalize(cornerField)` | cornerField, edgeRaggedness, PatchAtlas | cornerField patches + Patch Layer | Yes | Yes |
| E. Decal | `PlaceDecals(layout)` | TerrainDef.defaultDecalDensity, decalPool | Decal Tilemap | Yes | Yes |
| F. Prop Stamp | `StampProps(layout)` | recipe.allowedClusters, propDensity | Prop SpriteRenderer Layer | Yes | Yes |
| G. Scatter | `ScatterGenerate(layout)` | TerrainDef.defaultScatterDensity, scatterDensity | Scatter Layer | Yes | Yes |
| H. Shadow + YSort | `ApplyShadows()` | props, walls | Shadow Layer + sorting orders | Yes | No |
| I. Validation | `Validate()` | full state | warnings/errors list | No | No |

Pipeline orchestrator:
```csharp
class RoomGenerator {
    public RoomAsset Generate(RoomRecipe recipe, int seed, StageFlags stages = StageFlags.All) {
        var ctx = new GenerationContext(recipe, seed);
        if ((stages & StageFlags.Layout) != 0) BuildLayout(ctx);
        if ((stages & StageFlags.Terrain) != 0) PaintTerrain(ctx);
        if ((stages & StageFlags.Resolve) != 0) ResolveVisuals(ctx);
        if ((stages & StageFlags.Naturalize) != 0) Naturalize(ctx);
        if ((stages & StageFlags.Decal) != 0) PlaceDecals(ctx);
        if ((stages & StageFlags.PropStamp) != 0) StampProps(ctx);
        if ((stages & StageFlags.Scatter) != 0) ScatterGenerate(ctx);
        if ((stages & StageFlags.Shadow) != 0) ApplyShadows(ctx);
        if ((stages & StageFlags.Validation) != 0) Validate(ctx);
        return ctx.ToRoomAsset();
    }
}
```

---

## §4 — EditorBakeMode vs RuntimeMode (gap fill G2, ChatGPT correction #2)

≤100 ms is a **target, not a hard architectural rule**. Real architecture:

| Mode | Use case | Path | Quality |
|---|---|---|---|
| **EditorBakeMode** | Designer authoring in Unity Editor; pre-run baking | Full 9-stage pipeline, all assets, no caching | High (seconds OK) |
| **RuntimeMode** | Player walking through dungeon door | Load pre-baked RoomAsset from disk; or lite-stage + cache | Smooth (<100 ms target) |
| **AsyncStreamMode** | Procedural runs without prebake (Daily Dungeon, modded recipes) | Background coroutine over multiple frames; show loading shimmer | Moderate (200-500 ms acceptable) |

**Caching strategy:**
- All RoomAssets generated in EditorBakeMode are committed to disk as `.asset` files
- Runtime first tries cache hit by `(recipe.guid, seed)` key
- Cache miss → AsyncStreamMode generation + write-through to disk
- Cache invalidation on RoomRecipe edit

**Implication:** Phase 1 only implements EditorBakeMode. Runtime/Async are Phase 2-3.

---

## §5 — Polish Editor Modes (9 modes)

| Mode | Verb | Persists | Survives regen? |
|---|---|---|---|
| A. Terrain Paint | Paint cornerField | cornerField | If region not locked |
| B. Direct Tile Override | Lock exact Sprite at cell | manualOverrides[] | Yes (overrides resolver) |
| C. Decal Paint | Place visual-only overlay | manualDecals[] | Yes if locked |
| D. Prop Stamp | Place cluster | manualProps[] | Yes if locked |
| E. Scatter Regenerate | Rerun stage G in bounds | replaces scatter in region | n/a (manual operation) |
| F. Lock Region | Mark bbox as locked | lockedRegions[] | All stages skip these cells |
| G. Regenerate Selected Region | Rerun stages X-Y in bbox | per-stage | Preserves locks + overrides |
| H. Clear Dressing Only | Remove decals+scatter+props, keep base | clears manualDecals/Props + dressing | n/a |
| I. Validate Room | Visual debug overlay + RED_X list | UI only | n/a |

**Critical primitive — Direct Tile Override (B):** Resolver-resistant. Designer can pin a specific Sprite at cell (x,y) and no regeneration overwrites it unless they explicitly clear the override.

---

## §6 — Layer Stack (5 layers)

```
Base Tilemap         → ResolveVisualTile output (Wang transitions + base tiles)
Decal Tilemap        → cracks, stains, moss patches (overlay, no collision)
Wall/Cliff Tilemap   → wall/cliff terrains (collision + elevation)
Prop SR Layer        → trees, pillars, crystals (Y-sort, collision footprint)
Scatter SR Layer     → small organic detail (no collision, scattered)
```
Each layer is independent; one stage writes to exactly one layer. Polish edits target one layer at a time.

---

## §7 — Validation Rules

### Errors (block save / red X)
- 3+ distinct terrains at any single tile's 4 corners
- Missing TerrainDefinition.baseTile
- Missing TransitionEdge for a touching terrain pair (edge.supported = false)
- 4×8 transition tileset routed through 4×4 resolver
- TilesetGenerationSettings.source unset on a TransitionEdge
- Walkability inferred from upper/lower role (anti-pattern detector)

### Warnings (allow but flag)
- **Same-family pair using Wang** (ChatGPT correction #1): `visualCategory(lower) == visualCategory(upper)` AND `colorDistance < threshold`. Warning text: *"Low-contrast Wang pair may produce blocky output. Consider patch overlay (PatchAtlas) instead. Allow if transition prompt creates strong readable boundary."*
- Blocked gameplay path (cornerField makes entrance→exit impossible)
- Over-dressing in combat zone (decal density > 0.5 in centerClearance bounds)
- 4×8 transition tileset present but no PatchAtlas consumer (asset stranded)
- Manual override exists in a region marked for regeneration (silent conflict)

### Informational
- Patch density per region
- Average cluster size
- Cache hit/miss for the recipe

---

## §8 — Debug Overlay

For selected cell (x, y), display:
- NW / NE / SW / SE terrainId + displayName
- distinctTerrainCount
- selected TransitionEdge (if 2 terrains)
- computed wangMask binary + decimal
- selected wang_N from maskToTile
- final Sprite source (base tile / transition tile / manual override / RED X)
- manualOverride status (locked? by whom?)
- walkable / blocksMovement / elevationLevel from TerrainDefinition
- decal/prop/scatter present in this cell

---

## §9 — Hard Rules / Anti-Patterns (NEVER do)

1. **Never write tile coordinates manually.** Build generators that emit coordinates deterministically from RoomRecipe + seed. (ChatGPT spec 2 §9)
2. **Never infer walkability/blocking/elevation from PixelLab upper/lower.** These are pair-local visual roles only. Walkability lives on TerrainDefinition.walkable + blocksMovement.
3. **Never process 4×8 transition tileset in the 4×4 Wang resolver.** Phase 1 hard-skips with inspector warning. Phase 2 routes to PatchAtlas/Naturalize Pass.
4. **Never trust MCP `get_topdown_tileset` for Pro UI slider state.** It returns prompts + base_tile_ids only. Slider values must be captured manually at import time and stored on TransitionEdge.generationSettings.
5. **Never use "NO X NO Y NO Z" negative bombardment in prompts.** Use pure positive descriptors + saturation anchor. Confirmed cause of magenta-drift in today's test session.
6. **Never use `RGB()` color notation as a saturation control.** PixelLab AI doesn't reliably parse it for saturation; use named descriptors ("muted dusty washed-out desaturated pastel").
7. **Never treat PixelLab Map Editor ZIP export as editable source.** It's a rendered-output package without cell grid. Reference / inspiration only.
8. **Never generate `create_topdown_tileset` for same-family low-contrast pairs without warning.** Validator should flag; user can override if transition prompt is strong.

---

## §10 — Roadmap

### Phase 1 (Foundation — 1-2 weeks Codex chain)
Goal: Minimum viable procedural room from RoomRecipe.

- [ ] §2 data structures: TerrainDefinition, TerrainTransitionGraph (with TilesetGenerationSettings), RoomRecipe SO, RoomAsset SO
- [ ] §3 pipeline stages A-C (Layout / Terrain / PixelLab Resolve) only
- [ ] §5 polish modes A-B (Terrain Paint / Direct Tile Override)
- [ ] §7 validation errors (not warnings yet)
- [ ] §8 debug overlay
- [ ] EditorBakeMode only
- [ ] 1 working RoomRecipe: "ShatteredKeep_CombatArena"
- [ ] Existing Map Designer (Karar #132) becomes the Polish Editor host for modes A-B

### Phase 1.5 (Iteration — 3-5 days)
- [ ] §5 modes F-G (Lock Region / Regenerate Selected) — killer iteration loop
- [ ] §5 mode H (Clear Dressing Only)
- [ ] §7 validation warnings (same-family Wang detector, blocked path)
- [ ] Stage D (Edge Naturalize) — uses §2.7 PatchAtlas
- [ ] §2.7 PatchAtlas SO + 1 atlas for Shattered Keep
- [ ] PixelLab asset import script: `create_tiles_pro` output → tagged pool member

### Phase 2 (Dressing — 1-2 weeks)
- [ ] §3 stages E-G (Decal / Prop Stamp / Scatter)
- [ ] §2.8 PropCluster SO + 5 starter clusters (BrokenPillar, MossyRock, CrystalBloom, DebrisHeap, DeadRoot)
- [ ] §5 modes C-E (Decal Paint / Prop Stamp / Scatter Regenerate)
- [ ] §4 Layer Stack — 5-tilemap setup
- [ ] §3 stage H (Shadow + Y-sort)
- [ ] 4×8 transition tileset import + PatchAtlas integration
- [ ] §2.6 DungeonRecipe scaffold (single floor, 5-7 rooms)

### Phase 3 (Runtime)
- [ ] §4 RuntimeMode + cache + AsyncStreamMode
- [ ] §2.6 Full dungeon composition (multi-floor, branch/loop topology)
- [ ] Runtime room transitions
- [ ] Polish editor full feature set
- [ ] 7 RoomType presets fully tuned

---

## §11 — Migration from current state

Existing assets/work that survives:
- Karar #131 Corner Wang Pipeline → becomes §2.4 ResolveVisualTile
- Karar #132 Map Designer (RimaMapDesignerWindow) → becomes §5 Polish Editor host
- S73-S75 cornerField multi-terrain refactor → §2.4 cornerField
- 11 existing PixelLab tilesets → ingested as §2.2 TransitionEdge instances (with generationSettings filled retroactively)
- Bugünkü Pro path↔moss tileset (`b41919aa`) → first Phase 1 PatchAtlas candidate
- Bugünkü `create_tiles_pro` 16 variants → tagged as patch/decal/scatter pool (cherry-pick the 4 usable ones)
- Bugünkü cream drift `create_map_object` → first PatchEntry in Alabaster PatchAtlas

Existing things that get re-framed:
- "Multi-terrain paint" workflow → Polish Editor mode A (one of 9 modes)
- "Wall paint" → just another TerrainDefinition with blocksMovement=true, elevationLevel=2
- Scatter brush (Karar #121) → becomes §5 mode E (Scatter Regenerate) + §3 stage G (auto Scatter)

Things explicitly retired:
- Treating PixelLab Map Editor ZIP as editable map source (was never viable, now formal anti-pattern)
- Single-mode brush in Map Designer (now 9 modes)
- Per-pair lower/upper inferring gameplay semantics (now strictly visual)

---

## Appendix A — Today's session findings folded in

F1. Same-family pair Wang fail → §7 Warnings + §9 Rule #8
F2. MCP create-side Standard-only → §2.3 GenerationSource enum
F3. Pro UI slider state not in metadata → §2.3 TilesetGenerationSettings on edge
F4. 3-API role map → §1 (broadened per ChatGPT correction #4; create_tiles_pro feeds 4 pools)
F5. Negative-prompt bombardment confusion → §2.1 basePromptTemplate + §9 Rule #5+6

## Appendix B — ChatGPT corrections folded in

C1. Same-family Wang = warning not ban → §7 Warnings
C2. ≤100 ms = target not rule, bake/cache/async architecture → §4
C3. 4×8 = Phase 2 PatchAtlas/Naturalize source → §1, §3 stage D, §10 Phase 2
C4. create_tiles_pro multi-role → §1, §2.7 PatchAtlas sources
C5. Slider state captured at import → §2.3 TilesetGenerationSettings

## Appendix C — RoomType taxonomy

| Type | Use | Signature |
|---|---|---|
| CombatArena | Main combat encounter | Open center, prop ring, multiple entrances |
| Corridor | Connector between rooms | Narrow path, hazard accents |
| Treasure | Reward room | Small, decorated, single entrance |
| Shrine | Buff/altar room | Central altar, symmetric, ambient lighting |
| Transition | Biome change marker | Mixed terrain, story moment |
| Cliff | Vertical traversal | Elevation pair, drop hazards |
| RuinedGarden | Atmospheric break room | Vegetation overgrowth, low combat |
