# ChatGPT Authoritative Correction Memo — 2026-05-19

Source: ChatGPT review of CHATGPT_REVIEW_RIMA_MAP_ARCHITECTURE.md.

This is **authoritative**. All revisions below override previous plan documents.

---

## Critical fix #1 — PPU LOCK ERROR

**WRONG (eski spec):**
- PPU=64, Floor=64×64, Wang16=32×32 → half-cell scaling, alignment bugs, broken brush

**RIGHT (ChatGPT lock):**
- **Base terrain grid = 32×32 px**
- **Unity PPU = 32**
- **1 Unity unit = 32px = 1 terrain cell**
- **Character 64×64 = 2×2 cells** (this is normal for ARPG, character can span tiles)
- **Wang16 native 32×32 = exactly 1 cell**

`ImportAssetRole` enum required:
```csharp
public enum ImportAssetRole {
    Terrain32, Wang16_32, FloorMacro64, Decal, Scatter, Prop,
    Character, TierBBackground, UI
}
```

Per-role: PPU + pivot + filter + compression + alpha + asset type.

## Critical fix #2 — Wang16 RESOLVER must be CORNER-BASED

**WRONG:** Unity RuleTile N/E/S/W neighbor check
**RIGHT:** PixelLab corner-based mask:

```
mask = NW<<3 | NE<<2 | SW<<1 | SE<<0
```

- `wang_0` = all corners lower
- `wang_15` = all corners upper
- `lower`/`upper` is **pair-LOCAL** (NOT global Terrain property)
- Same terrain may be lower in pair A and upper in pair B
- Walkable/blocking/elevation/wall-ness NEVER derived from upper/lower

## Critical fix #3 — JSON-driven tileset import

- PixelLab `tileset_data.tiles[].corners` + `bounding_box` JSON is source of truth
- Filename `wang_XXXX` is fallback only
- **Y-axis flip** required: PixelLab top-origin → Unity bottom-origin
- Support both `wang_13` decimal and `wang_1101` binary, normalize to mask=13

## Critical fix #4 — WallKitSO mandatory (NOT Wang16 alone)

Image 1 target REQUIRES `WallKitSO` system:
- modular straight walls + corners + top caps + front faces
- pillars + niches + doorways + damaged variants
- banner / candle / moss-grime sockets
- wall modules may be **LARGER than 32×32** (aligned to 32 grid as `Vector2Int footprintTiles`)

Wang16 alone = "tiny game" look. WallKit = Image 1 quality.

## Critical fix #5 — Tier B is PIXEL-ART-COMPATIBLE atmospheric (NOT painterly)

**WRONG:** "Tier B = painterly Hades background"
**RIGHT:** "Tier B = pixel-art-compatible atmospheric background"

- Same palette family as Tier A
- Lower contrast than Tier A
- NO smooth AI-painterly gradients clashing with foreground
- Receives pixel cleanup / posterization / dithering if needed
- Behind gameplay plane
- Doesn't compete with character readability

Sizes:
- 512×512 = modular wall chunks
- 1024×512 = wide hero backdrops
- 1024×1024 = large hero chamber piece
- 256×512 = distant arch / statue / silhouette
- 256-512 = lighting / glow overlay
- **DON'T stretch 512 across 1920×1080** → use modular

## Critical fix #6 — Two room types via PROFILE-BASED SO, not hardcoded enum

`RoomVisualProfileSO`:
```csharp
public enum RoomVisualMode {
    DungeonEnclosed, HadesArena, RuinedCourtyard,
    Shrine, RiftChamber, BossArena
}

[Field] bool usesTierBBackground;
[Field] bool usesWallKit;
[Field] bool usesWangFeatureEdges;
[Field] bool usesCloseGameplayWalls;
[Field] WallKitSO wallKit;
[Field] Wang16AtlasSO defaultFeatureEdgeAtlas;
[Field] LightingProfileSO lightingProfile;
[Field] int defaultWallThicknessTiles;
[Field] float backgroundParallaxStrength;
[Field] bool allowLargePaintedBackground;
```

Type A / Type B are **presets**, NOT two separate codebases.

## Critical fix #7 — Wang16 dual-use SEPARATE atlases

`Wang16AtlasSO` with `usageRole`:
```csharp
public enum Wang16UsageRole {
    TerrainTransition, FeatureEdge, CliffEdge, RaisedPlatformEdge,
    WallEdge, WaterBorder, HazardBorder
}
```

Resolver shared, atlases separate. NOT one atlas for everything.

## Critical fix #8 — 4×8 transitions NOT for basic resolver

- Phase 1: Standard 4×4 Wang16 / tileset15 / 32×32 native / 16 masks / lower-upper only
- Phase 2: 4×8 ragged / transition_size=1.0 → Edge Naturalization Pass + PatchAtlas source

NEVER process 4×8 through basic Wang16 resolver.

## Critical fix #9 — Props tiered n_frames

**WRONG:** Default n_frames=64 for all props
**RIGHT:**
- Small/simple scatter (pebbles, bones, broken pottery) → **n_frames=16**
- Common props (small crates, candles, debris) → **n_frames=16 or 32**
- Hero props (brazier, altar, statue, ritual device, class shrine) → **n_frames=64**

Default 16, escalate selectively.

## Critical fix #10 — Same-family terrain pair WARNING

Low-contrast same-family pairs → blocky/chaotic Wang results.

Validation rule:
```
if ColorDistance(lower.avg, upper.avg) < threshold
   && lower.category == upper.category
   && !edge.hasStrongTransitionPrompt:
       warn USE_PATCH_OVERLAY_NOT_WANG
```

Same-family variation → use **one base terrain + PatchAtlas overlay** instead of Wang16.

## Critical fix #11 — Runtime vs Editor generation modes

```csharp
public enum RoomGenerationMode {
    EditorBake, RuntimeCached, RuntimeLite, Preview
}
```

PixelLab generation is **asset production**, NOT runtime. Don't run 9-stage pipeline synchronously when player opens a door. RuntimeCached loads pre-baked data.

## Critical fix #12 — LoRA is NOT primary production

**WRONG:** "Custom LoRA trained tonight → production tomorrow"
**RIGHT:**
- Primary: **PixelLab vendor model + style anchors + controlled prompts**
- Secondary: Aseprite / manual cleanup
- Research branch: SDXL pixel LoRA test
- Fallback: custom RIMA LoRA only if clearly beats PixelLab

LoRA must NOT replace PixelLab JSON-driven tileset pipeline.

## Critical fix #13 — Production sequence is VERTICAL SLICE FIRST

**WRONG:** Bulk-generate 100+ assets, then test
**RIGHT:** Phased vertical slice, prove stack BEFORE bulk:

- Phase 0 — Scale Test: PPU=32, 1 terrain, 1 Wang16, 1 character, 1 prop, 1 decal → verify alignment
- Phase 1 — Minimal Type A: 1 floor + 1 Wang16 pair + 1 WallKit module set + 1 prop cluster + 1 decal atlas + character
- Phase 2 — Minimal Type B: 1 Tier B background chunk + 1 floor + 1 feature edge + 1 prop + 1 2D light + character
- Phase 3 — Sırıtma Test: foreground/background match? scale? lighting unify?
- **ONLY THEN:** bulk production

## Critical fix #14 — Room Composer layer model expanded (L0-L11)

```
L0  Data (cornerField, collision, elevation, walkability, locks)
L1  Base Tone (ambient wash)
L2  Base Terrain (32×32 tilemap)
L2b Floor Macro Variants (optional 64×64 patches)
L3  Wang16 Terrain / Feature Edges
L4  WallKit / Modular Architecture
L5  Organic Decals (moss/dirt/grime/blood/stains)
L6  Detail Scatter (pebbles/cracks/bones/small rubble)
L7  Accent Overlays (rift scars/ritual marks/large splatter)
L8  PropCluster / Stamps
L9  Shadows (prop grounding, wall contact, cliff)
L10 Glow / Lighting Helpers (candle, brazier, rift, 2D lights)
L11 Manual Overrides (locked cells, hand-polish edits)
```

Image 1 NOT achievable from tilemap alone → composed via this layer stack.

## Critical fix #15 — Required SOs

- `TerrainDefinitionSO` (visual + gameplay decoupled)
- `TerrainTransitionGraphSO` (pair-based edges, 16 masks required)
- `Wang16AtlasSO` (usageRole enum)
- `WallKitSO` (modular wall library)
- `PatchAtlasSO` (decal/scatter density rules)
- `PropClusterSO` (multi-prop stamps)
- `RoomVisualProfileSO` (Type A/B profile presets)
- `RoomRecipeSO` (full room procgen recipe)
- `DungeonRecipeSO` (dungeon composition + adjacency)
- `TilesetGenerationSettings` (PixelLab metadata storage)

---

## Things to REMOVE / REWRITE from previous plan

| Eski (yanlış) | Yeni (ChatGPT lock) |
|---|---|
| PPU=64 | **PPU=32 gameplay-world** |
| L2 Floor 64×64 base grid | **L2 base terrain 32×32**, 64×64 macro patch optional |
| Wang16 32×32 → tall walls 64×96 | **Wang16 32×32 ONLY for resolver, tall walls → WallKitSO prefabs** |
| Wang16 = Type A room walls | **Wang16 may assist edges, Image 1 walls require WallKit** |
| Props n=64 default | **n=16 default, n=64 hero only** |
| Tier B "painterly Hades" | **Tier B pixel-art-compatible atmospheric, palette-locked, lower contrast** |

---

## End of memo.
