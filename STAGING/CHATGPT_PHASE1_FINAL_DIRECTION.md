# RIMA Room Composer — FINAL DIRECTION (ChatGPT, 2026-05-17)

Source: ChatGPT verdict after Phase 0 + paint-big-slice-small pivot proposal.
Authority: **FINAL. All previous plan documents that conflict are superseded.**

---

## TL;DR — final formula

The visible floor should not be a field of equally important tiles. It should be a calm base surface with layered irregular visual information on top.

```
Grid          = logic
Base tile     = foundation              (clean 32×32, low-contrast, no focal marks)
Macro patch   = broad natural form      (64/128/256 irregular alpha, sliced from large painterly sources)
Organic decal = surface character       (moss, grime, dirt, damp)
Scatter       = fine detail             (pebbles, cracks, bones, dust)
Stamp/prop    = composition             (brazier, crate pile, broken pillar)
Shadow/light  = depth                   (prop grounding + 2D light helpers)
Manual polish = final art direction     (locked overrides)
```

User paints intent (semantic stroke). System interprets that intent into a layered visual composition across L0–L11.

---

## 1. Scale LOCK (unchanged from Phase 0)

- Base logical grid = 32×32 px, PPU = 32, 1 tile cell = 1 Unity unit
- Wang16 tiles 32×32 native
- Character 64×64 = ~2×2 cells
- 64×64 assets = decals, macro patches, props, characters
- 128×128 / 256×256 = large macro patches, accent stamps
- 512×512 / 1024×512 / 1024×1024 = Tier B background / hero backdrop ONLY
- **DO NOT** revert to PPU=64. **DO NOT** make 64×64 the base terrain grid.

## 2. Wang16 scope LOCK

**USE Wang16 ONLY for:**
- cliff edges, raised platform edges
- water / lava / hazard borders
- strong elevation/material boundaries
- feature edges where a visible boundary is expected

**DO NOT use Wang16 for:**
- moss spreading over stone, grime over floor, dirt/wear
- same-family floor variations, clean→dirty transitions
- ANY same-elevation natural blending

Same-elevation blending = single base + macro patches + organic decals + scatter + random transforms + spatial masks + manual polish.

## 3. Three Paint Modes

### A. Terrain Paint
- Changes logical terrain data (walkability, collision, elevation, feature boundaries)
- Examples: Stone, Dirt, Cliff, Wall, Water, Hazard, Platform
- Writes to: `cornerField` / `terrainField` / collision / elevation
- Can invoke: Wang16 resolver (elevation/feature boundaries only)

### B. Organic Paint
- Visual-only, does NOT change gameplay collision
- Examples: Moss, Grime, Dust, Damp stain, Rubble dust, Hairline cracks
- Writes to: `MacroFloorPatch` + `OrganicDecal` + `DetailScatter` layers
- Does NOT invoke Wang16

### C. Stamp / Prop Paint
- Places composed objects or handcrafted clusters
- Examples: Broken pillar cluster, Brazier corner setup, Crate pile, Bone pile, Ritual mark, Wall candle group
- Writes to: `PropCluster` + `Stamp` layers + collision footprint + Y-sort data

## 4. Paint Big, Slice Small — CORRECT INTERPRETATION

**WRONG:** Generate one big image, slice random 32×32 opaque chunks, use as primary tile pool. Continuity breaks when chunks are rearranged.

**RIGHT:** Use big painterly source for:
- 64×64 macro floor patches
- 128×128 organic patches
- 256×256 broad floor washes
- Transparent/irregular masked decals
- Grime / moss / dirt / wear overlays

**Processing pipeline:**
1. Generate large continuous floor source
2. Crop 64×64 / 128×128 / 256×256 candidates
3. Reject focal symbols, runes, bright orange, blood unless intended as accent
4. Apply irregular alpha mask (not square)
5. Downsample with LANCZOS or AREA (NOT direct nearest from high-res painterly)
6. Palette clamp / quantize to RIMA palette
7. Optional light dithering
8. Save as `MacroFloorPatch` / `OrganicDecal` asset
9. Place on **L2b or L4, NOT as opaque base terrain**

## 5. Final Layer Model (L0–L11)

| Layer | Content | Source |
|---|---|---|
| L0 | Data: cornerField, collision, elevation, walkability, locks, path/combat masks | Authoring |
| L1 | Base Tone ambient wash | Color |
| L2 | Base Terrain clean 32×32 low-contrast | PixelLab `create_tiles_pro` |
| L2b | Macro Floor Variation 64/128/256 irregular | Big painterly source → crop → mask |
| L3 | Wang16 feature/elevation edges ONLY | PixelLab `create_topdown_tileset` |
| L4 | Organic decals (moss/grime/dirt/damp/wall-base dirt) | `create_object` n=16 |
| L5 | Detail scatter (pebbles/cracks/bones/dust) | `create_object` n=16 |
| L6 | Accent overlays (rift scar/ritual circle/blood) | `create_object` n=16 |
| L7 | WallKit modular architecture | `create_object` n=8 per module |
| L8 | PropCluster/Stamps | `create_object` n=16-64 |
| L9 | Shadows (prop grounding, wall contact, cliff) | Sprite blob + baked |
| L10 | Glow/Lighting helpers (candle/brazier/rift/2D Lights) | Light2D + sprite glow |
| L11 | Manual overrides (locked cells, hand-polish) | Authoring |

## 6. Brush Stroke Data Model

```csharp
[Serializable]
public sealed class RoomPaintStroke {
    public string strokeId;
    public PaintMode mode;
    public string brushId;
    public RectInt affectedBounds;
    public List<Vector2> sampledWorldPoints;
    public float radius, density, strength, falloff;
    public int seed;
    public bool respectsWalkability;
    public bool avoidCombatCenter;
    public bool edgeBiased;
    public bool wallProximityBiased;
    public bool allowOverlap;
    public bool locked;
}

public enum PaintMode {
    TerrainPaint, OrganicPaint, StampPaint,
    DirectTileOverride, Erase, RegenerateSelected
}
```

Stroke MUST be replayable/deterministic (same seed + same asset library = same placements unless manually edited).

## 7. Organic Paint Placement Loop

```
1. Build candidate area from brush mask
2. Remove forbidden cells (blocked, locked, invalid terrain, combat center if guarded)
3. Compute density map (brush strength × wall prox × edge prox × prop prox × center reduction × Perlin)
4. Place LARGE patches first (128/64, low freq, larger min distance, irregular alpha)
5. Place MEDIUM decals (64, fill gaps)
6. Place SMALL scatter (32 or smaller, low visual weight, avoid regular spacing)
7. Apply transforms (flipX/Y, rot 0/90/180/270, scale 0.75-1.25, alpha 0.75-1.0)
8. Validate (no over-density, no path block, no repeat cluster, no square silhouette)
9. Save as editable placements
```

**Placement order matters:** large patches → medium decals → tiny scatter. Do NOT place everything with equal per-tile probability.

## 8. Separate Atlas Types

- `BaseFloorAtlasSO` — clean 32×32, no focal marks, low-contrast
- `MacroFloorPatchAtlasSO` — 64/128/256 transparent/irregular, broad variation
- `OrganicDecalAtlasSO` — moss/dirt/damp stain/wall grime/cracks
- `DetailScatterAtlasSO` — pebbles/bone shards/tiny rubble/dust/hairline cracks
- `AccentAtlasSO` — rift scar/ritual circle/blood mark, rare focal

**Do NOT mix into one generic sprite bucket.** Brush logic depends on role separation.

## 9. Decal Density Guidelines

**Type A enclosed dungeon (12×8 = 96 cells):**
- L4 organic large: 0.10–0.14 (~10–14 patches)
- L5 detail scatter: 0.16–0.22 (~15–22 scatter)
- L6 accent: 0.02–0.035 (~1–3)

**Type B Hades-style arena (protect combat readability):**
- Center: L4 organic 0.03–0.07
- General floor: L4 organic 0.08–0.12
- Edges / wall prox / prop prox / feature prox: L4 organic 0.15–0.25
- L5 detail: 0.14–0.18 general, reduced center
- L6 accent: 0.01–0.025, encounter-aware

**Do NOT solve naturalness by increasing density everywhere.** Use zones.

## 10. Style Drift Mitigation

**BAD:** "NO stones NO blocks NO slabs NO grass NO moss NO runes NO blood"

**GOOD:** "plain undecorated dark slate stone floor tile, muted low-contrast surface, subtle worn stone grain, sparse hairline cracks, no focal markings, no glowing elements, no painted symbols, consistent neutral gray-brown palette"

Rules:
- Positive constrained descriptions
- Avoid long "NO" chains
- Use "plain", "undecorated", "low-contrast", "no focal markings", "no glowing elements" only when necessary
- ONE semantic role per dispatch batch (don't ask one batch for clean/moss/blood/rift/cracked all together)
- Use `style_images` / approved references when available
- Save QC metadata (accepted/rejected, reason, color drift, contamination flag, painterly score, perspective check, tileability check)
- Aseprite cleanup for near-good assets

## 11. Phase 1A Revised Dispatch Plan

Phase 1A goal: validate organic decal-driven blending + clean base + macro patches + minimal walls + props + shadows + character scale + NO VISIBLE GRID.

If WallKit skipped, rename to **"Phase 1A Layer Stack Visual Test"** (not "Minimal Type A enclosed dungeon").

| # | Layer | Tool | Count | Pick |
|---|---|---|---|---|
| 1 | L2 base floor — plain dark slate | `create_tiles_pro` | 1 call (16 var) | 4-5 cleanest |
| 2 | L2 base floor — subtle cracked/worn slate | `create_tiles_pro` | 1 call (16 var) | 3-4 cleanest |
| 3 | L2b macro patches | PixelLab Create Image Pro (web UI) | 1 large source (512px) | crop 10-20 patches (64/128) + irregular alpha |
| 4 | L4 moss | `create_object` n=16 | 1 call | 5-7 |
| 5 | L4 grime/dirt | `create_object` n=16 | 1 call | 4-5 |
| 6 | L4 damp stain | `create_object` n=16 | 1 call | 3-4 |
| 7 | L5 pebbles/rubble | `create_object` n=16 | 1 call | 4-5 |
| 8 | L5 hairline cracks | `create_object` n=16 | 1 call | 3-4 |
| 9 | L5 bones/dust flecks | `create_object` n=16 | 1 call | 3-4 |
| 10 | L6 rift scar | `create_object` n=16 | 1 call | 2-3 |
| 11 | L6 ritual mark | `create_object` n=16 | 1 call | 2-3 |
| 12 | Props — crate/urn batch | `create_object` n=16 | 1 call | 4-6 |
| 13 | Props — brazier/candle batch | `create_object` n=16 | 1 call | 4-6 |
| 14 | Props — statue/pillar batch | `create_object` n=16 | 1 call | 4-6 |
| 15-18 | Minimal WallKit IF Type A | `create_object` n=8 | 4 calls (straight/corner/door/pillar) | 1-2 each |

**Total: 14 dispatches (Layer Stack Visual Test) or 18 (full Type A).** Existing 64×64 character sprite reused.

**Success criteria:**
- No visible 32×32 grid repetition
- No per-tile soft outline
- Floor reads as continuous
- Character scale feels correct
- Props sit on ground with shadows
- Center remains readable
- Wall/edge dressing does not overpower gameplay
- Generated room looks **composed, not tiled**

## 12. Minimal Phase 1 SO Contracts (MUST exist before asset library growth)

```csharp
public class TerrainDefinitionSO : ScriptableObject {
    public string terrainId, displayName;
    public TileBase baseTile;
    public bool walkable, blocksMovement;
    public VisualCategory visualCategory;
    public Color averageColor;
    public float defaultDecalDensity, defaultScatterDensity;
}

public class PatchAtlasSO : ScriptableObject {
    public string atlasId;
    public PatchRole role;
    public List<TerrainId> validTerrains;
    public float density, minDistance;
    public bool edgeBiased, wallProximityBiased;
    public AllowedTransforms allowedTransforms;
}

public class PropDefinitionSO / PropClusterSO {
    public Sprite/Prefab visual;
    public Vector2Int footprint;
    public bool hasCollision;
    public Vector2 ySortPivot;
    public List<TerrainId> validTerrains;
}

public class RoomVisualProfileSO {
    public RoomVisualMode visualMode;
    public bool usesWallKit, usesTierBBackground;
    public LightingProfileSO lighting;
    public List<PatchAtlasSO> allowedPatchAtlases;
    public List<PropClusterSO> allowedPropClusters;
}

public enum ImportAssetRole {
    Terrain32, MacroPatch64_128, OrganicDecal,
    DetailScatter, Accent, Prop, Character, TierBBackground
}
```

`TerrainTransitionGraphSO` + `DungeonRecipeSO` can wait until later. Above contracts MUST exist before asset growth.

## 13. Core Principle — Alabaster Dawn-like Fluidity

**Do NOT attempt fluid look from tile generation alone.** Tiles are equally important = grid look. Fluidity comes from the LAYERED COMPOSITION on top of tiles.

The player should feel like the room was painted. The editor internally knows which part is: terrain / decal / scatter / prop / shadow / collision / manual override.

## 14. Final Architecture (LOCKED)

```
32×32 base grid + PPU=32
PixelLab Wang16 → elevation/feature boundaries ONLY
Clean low-detail base floor tiles
Macro patches from large painted sources (cropped + alpha-masked)
Organic decals for same-elevation blending
Detail scatter for texture
Prop/stamp clusters for composition
Shadows/glow for depth
Manual polish and lock/regenerate tools
```

**Proceed with:** Paint-like semantic brush + procedural dressing layers.
**NOT:** Random 32×32 tile pool. NOT: Wang16 for natural moss blending. NOT: One baked PixelLab map as final source.
