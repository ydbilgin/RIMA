# RIMA Map Designer — Unified Brush System Design

**Status:** Pre-production design draft for ChatGPT cross-review
**Date:** 2026-05-16 (S85)
**Author:** Claude Opus (RIMA orchestrator) + user vision + ChatGPT spec input
**Decision target:** Approve architecture, then dispatch Codex Sprint 1

---

## 0. How to Read This Document

This is a self-contained design draft for an art-direction tool. It can be reviewed without any RIMA repo access. Section 1 explains the project context. Sections 2–10 are the actual design. Section 11 lists open questions for cross-review.

---

## 1. Project Context (RIMA)

RIMA is a 2D top-down roguelite (Unity URP 2D, pixel art, 128px characters, Hades-like 30–35° "low top-down" view). Map system is procedural — rooms generated per run.

**Locked decision (Karar #143) — 6-layer map architecture:**

```
L1 — Floor Base       (Tilemap, 32x32, walkable cells only)
L2 — Floor Variation  (Tilemap, multi-variant within same terrain)
L3 — Wall Overlay     (SpriteRenderer, Hades-style perimeter cap — NOT tileset)
L4 — Transition Brush (SpriteRenderer, oval 256x256 organic patches)
L5 — Detail Decals    (SpriteRenderer, cracks/rubble/moss 32–128px)
L6 — Rift Accent      (SpriteRenderer, sparse magical traces 64–128px)
```

**Already LIVE in code (Karar #143 Aşama 1+2 LOCK, dotnet build 0 errors):**

- `MapLayerOrchestrator` (6-layer dispatcher)
- `WallOverlayPainter` (L3 perimeter polyline → wall brush)
- `TransitionBrushPainter` (L4 edge-biased density + walkable filter)
- `DetailDecalPainter` (L5 scatter)
- `AccentPainter` (L6 sparse)
- `NaturalFeatureGraph` + `VoronoiWaterFeatureGenerator` + `VoronoiElevationFeatureGenerator` (Aşama 2 — water/elevation feature zones)
- `FeatureEdgeSmoothingPass` (Wang tileset boundary smoothing for water/cliff)
- `FeatureMaskSO` (vertex-paint analog — Texture2D alpha + AnimationCurve remap)
- `RimaMapDesignerWindow` (basic 6-layer toggle UI — needs full refactor)

**Locked rules (must be honored by the new brush tool):**

- **Karar #143-D** — Decoration painters MUST respect walkable mask. `walkable == false` cells never receive L4/L5/L6 decals (prevents patches on walls/cliffs).
- **Karar #143-E** — Edge-biased density: cells adjacent to walls = 10x density multiplier, center cells = 0.1x. Combat readability requires clean center "arena" zones.
- **Karar #143-K** — Feature proximity density: L5 detail density multiplied by `FeatureMaskSO` (e.g., dense rubble near cliff rim, sparse in open floor).
- **Deterministic seed:** same seed = same output. Required for testing and run replay.

**Visual identity (locked palette):**

- Act 1: `#1A1C20` / `#2A2D34` / `#3A3D48` / `#4E5260` / `#606575` + accent `#7BA7BC` (ice-blue)
- Style target: **Hades painter-net floor + Death's Door soft-edge patches** (hybrid). NOT pure pixel-perfect, NOT full watercolor.

---

## 2. Problem Statement

Current `RimaMapDesignerWindow` exposes 6 separate layer toggles (L1/L2/L3/L4/L5/L6). The user must mentally manage technical layers while painting. This is wrong UX — the user thinks in artistic intent ("paint a mossy broken corner"), not technical layers.

**Goal:** Replace this with one unified Photoshop-style brush tool. The user selects a brush preset; the tool decides which layer(s), which assets, which density. The 6-layer pipeline becomes invisible.

---

## 3. Architecture Decision — Unity-Host + JSON-Portable Hybrid

### 3.1 Why not fully standalone

A separate program (Electron/Qt/Godot) would discard:
- Live Unity scene preview (non-negotiable — paint and see immediately)
- ~1500 lines of LIVE Karar #143 painter code
- Unity's IMGUI/UI Toolkit (free UI framework)
- Unity Undo/Redo (free, robust)

Standalone is correct **eventually** (as a sellable RIMA-independent product, like Aseprite), but is a 2–3 month project orthogonal to RIMA shipping.

### 3.2 Why pure-Unity is also wrong

Pure `ScriptableObject` data means brush packs can't be shared between projects, can't be diff'd in git, can't be downloaded from a marketplace.

### 3.3 The hybrid

**Tool lives in Unity Editor.** All data structures are:
- `ScriptableObject` for in-editor use (Inspector edit, asset references)
- **Round-trip JSON serializable** — every SO has `ToJson()` / `FromJson(json, assetResolver)` methods
- Sprite/Tile references serialize as **project-relative paths** (`Art/Patches/moss_oval_01.png`), resolved via `AssetDatabase.LoadAssetAtPath` on import

**Brush pack format:**

```
shattered_keep_default.brushpack/
├── pack.json              (manifest: name, version, brush list, asset pool refs)
├── brushes/
│   ├── clean_stone.json
│   ├── moss_oval.json
│   └── ...
├── assetpools/
│   ├── moss_256.json      (sprite path list + metadata)
│   └── ...
└── Art/                   (optional — bundle sprites)
    ├── moss_oval_01.png
    └── ...
```

**Import flow:** Drag `.brushpack` folder into Unity Project → tool reads `pack.json` → creates SOs → resolves sprite paths (uses bundled `Art/` if local refs missing).

**Export flow:** Tool menu "Export Brush Pack" → writes JSON folder + optionally copies sprites.

### 3.4 Migration path

Today: Unity-host with JSON-portable data
Tomorrow: If we want standalone, write a separate front-end that consumes the same JSON. Painter logic ports to engine-agnostic C# or rewrites in the standalone host language. The data layer is already engine-agnostic from day 1.

---

## 4. Data Model

### 4.1 Enums

```csharp
public enum BrushCategory {
    Floor,        // L1 base tiles
    Variation,    // L2 floor variation
    Wall,         // L3 perimeter overlay
    Transition,   // L4 organic patches
    Detail,       // L5 small decals
    RiftAccent,   // L6 magical accents
    Composite     // multi-layer brush
}

public enum PaintMode {
    GridTile,            // single tile per cell, snapped
    GridTileRandom,      // random variant per cell, snapped
    FreeformDecal,       // single decal at click position
    ScatterAlongStroke,  // decals spawn along drag with min-distance
    Stamp,               // one decal per click, no scatter
    CompositeStroke,     // multi-layer per stroke
    EraseByLayer,        // erase target layer only
    EraseAllDecorative   // erase L3-L6, keep L1-L2
}

public enum TargetLayer { L1, L2, L3, L4, L5, L6 }

public enum SnapMode { None, FullGrid32, HalfGrid16, QuarterGrid8 }
```

### 4.2 AssetPoolSO

A reusable pool of sprites/tiles. Multiple brushes can share one pool.

```csharp
public class AssetPoolSO : ScriptableObject {
    public string poolName;
    public AssetCategory category;          // Floor, Moss, Crack, Wall, etc.
    public List<Sprite> sprites;
    public List<TileBase> tiles;
    public List<GameObject> prefabs;        // optional
    public Vector2Int nativeSize;           // expected size (e.g., 256x256)
    public bool supportsRotation;
    public bool supportsFlip;
    public bool isSoftEdge;                 // hint for render preset
    public string sourcePath;               // for JSON round-trip
}
```

**JSON form:**

```json
{
  "poolName": "moss_oval_256",
  "category": "Moss",
  "sprites": ["Art/Patches/moss_oval_01.png", "Art/Patches/moss_oval_02.png"],
  "tiles": [],
  "nativeSize": [256, 256],
  "supportsRotation": true,
  "supportsFlip": true,
  "isSoftEdge": true
}
```

### 4.3 BrushLayerOperation (serializable class, not SO)

One operation = one layer painted per stroke. A simple brush has 1 operation; a composite brush has 2–4.

```csharp
[Serializable]
public class BrushLayerOperation {
    public TargetLayer targetLayer;
    public AssetPoolSO assetPool;
    public float density = 0.5f;            // 0–1
    public float probability = 1.0f;        // chance this op fires per stroke
    public float minDistance = 32f;         // pixels between scatter points
    public Vector2 scaleRange = new(0.85f, 1.15f);
    public bool allowRotation = true;
    public bool allowFlipX = true;
    public bool allowFlipY = false;
    public float rotationSnapDegrees = 0f;  // 0 = continuous
    public Color tint = Color.white;
    public Vector2 positionJitter = new(0f, 0f);
    public int sortingOrderOffset = 0;
    public bool affectsCollision = false;

    // Karar #143 enforcement fields:
    public bool respectsWalkableMask = true;      // Karar #143-D
    public AnimationCurve wallProximityCurve;     // Karar #143-E (distance → multiplier)
    public FeatureMaskSO featureMaskMultiplier;   // Karar #143-K (optional)
}
```

### 4.4 MapDesignerBrushPresetSO

```csharp
public class MapDesignerBrushPresetSO : ScriptableObject {
    public string brushName;
    public BrushCategory category;
    public PaintMode paintMode;
    public List<BrushLayerOperation> operations;  // 1 for simple, 2-4 for composite
    public Sprite previewIcon;
    public bool showInPalette = true;
    public string description;                    // tooltip
    public int hotkeyIndex = -1;                  // 1-9 quick select, -1 = none
}
```

### 4.5 BrushPackSO

```csharp
public class BrushPackSO : ScriptableObject {
    public string packName;
    public string version;
    public string author;
    public List<MapDesignerBrushPresetSO> brushes;
    public List<AssetPoolSO> referencedPools;
    public Texture2D coverImage;
}
```

### 4.6 BiomeSkinSO (render preset)

Defines HOW each layer is rendered (alpha mode, tint, shader). Lets the same map paint look like Hades-net or Death's Door-soft by swapping skin.

```csharp
public class BiomeSkinSO : ScriptableObject {
    public string skinName;
    public BrushPackSO defaultBrushPack;
    public LayerRenderRule[] layerRenderRules;    // one per L1-L6
    public Color globalTint = Color.white;
    public float ambientLightIntensity = 0.35f;
}

[Serializable]
public class LayerRenderRule {
    public TargetLayer layer;
    public AlphaMode alphaMode;       // Hard, SoftAlpha8, SoftAlpha16, MultiplyBlend
    public Color tint = Color.white;
    public Material overrideMaterial; // optional, for shader effects
    public int sortingOrder;
}

public enum AlphaMode { Hard, SoftAlpha8, SoftAlpha16, MultiplyBlend }
```

### 4.7 RoomRecipeSO (already exists in repo, extend)

```csharp
public class RoomRecipeSO : ScriptableObject {
    // existing fields stay
    public BiomeSkinSO biomeSkin;
    public BrushPackSO allowedBrushPack;
    public int seed;
    public Dictionary<TargetLayer, float> densityMultipliers;
    public List<MapDesignerBrushPresetSO> autoDressBrushes;  // for auto-compose
}
```

---

## 5. Brush Categories — Detailed

### 5.1 Floor (L1)
- Paint mode: `GridTile`
- Snap: FullGrid32
- Affects collision: yes (walkable)
- Erase erases L1 only (rare — typically locked once room generated)

### 5.2 Variation (L2)
- Paint mode: `GridTileRandom`
- Snap: FullGrid32
- Density typically 0.25 (sparse darker stone variants)
- Affects collision: no

### 5.3 Wall (L3)
- Paint mode: `Stamp` (manual) or auto-triggered by "Brush Along Edges"
- Snap: HalfGrid16 (allows offset within perimeter)
- Asset pool: 14 wall sprites (horizontal 256x128, vertical 128x256, corners 128x128, doorways 128x96)
- Edge-aware: prefers placing on `wallEdges` polyline from `ProceduralRoomGenerator`

### 5.4 Transition (L4)
- Paint mode: `FreeformDecal` or `Stamp`
- Snap: None (free position)
- Density typically 0.5
- Sizes: 256x256 (MCP-generated) or 512x512 (Web UI Pro manual)
- Random rotation + flip + scale 0.85-1.15

### 5.5 Detail (L5)
- Paint mode: `ScatterAlongStroke`
- Snap: None
- Density 0.4, min-distance 32px
- Sizes: 32–128px
- Edge-biased density (Karar #143-E) ON by default

### 5.6 Rift Accent (L6)
- Paint mode: `FreeformDecal`
- Snap: None
- Density 0.08 (rare)
- Sizes: 64–128px
- High sortingOrder (always on top)

### 5.7 Composite (multi-layer)
- Paint mode: `CompositeStroke`
- 2–4 operations weighted
- Examples: "Mossy Broken Edge" (L2 dark + L4 moss + L5 crack), "Rift-Damaged Corner" (L2 + L4 + L5 + L6)

---

## 6. Smart Composite Brush — Deep Dive

This is the most important brush type — it's what makes the tool feel "painter-like."

### 6.1 Concept

One stroke triggers multiple `BrushLayerOperation` instances in a defined order, with weighted density. The user sees ONE brush; the tool fires 3-4 layered paints with respect to all Karar #143 rules.

### 6.2 Example: "Mossy Broken Edge"

```json
{
  "brushName": "Mossy Broken Edge",
  "category": "Composite",
  "paintMode": "CompositeStroke",
  "operations": [
    {
      "targetLayer": "L2",
      "assetPool": "floor_variation_dark",
      "density": 0.35,
      "probability": 1.0,
      "respectsWalkableMask": true
    },
    {
      "targetLayer": "L4",
      "assetPool": "moss_oval_256",
      "density": 0.45,
      "probability": 0.85,
      "scaleRange": [0.85, 1.15],
      "allowRotation": true,
      "respectsWalkableMask": true,
      "wallProximityCurve": "edge_biased_x6"
    },
    {
      "targetLayer": "L5",
      "assetPool": "crack_decals_64",
      "density": 0.20,
      "probability": 0.6,
      "minDistance": 32,
      "respectsWalkableMask": true
    }
  ]
}
```

When the user drags this brush across the scene:
1. Engine raycasts cells under the stroke.
2. For each cell, evaluates each operation in order with its own density/probability/walkable filter.
3. Spawns the sprite/tile via the appropriate existing painter (e.g., `TransitionBrushPainter.PlaceAt(cell, sprite, seed)`).
4. Records all spawned objects under one Undo group.

### 6.3 Why this matters

Without composite brush, achieving "natural mossy broken corner" requires:
1. Select Variation brush → paint dark floor
2. Switch to Transition brush → paint moss
3. Switch to Detail brush → paint cracks

3 tool switches per artistic intent. Composite = 1 switch.

### 6.4 Built-in composite presets (default pack)

| Preset Name | Layers | Use Case |
|---|---|---|
| Mossy Broken Edge | L2+L4+L5 | Natural erosion near walls |
| Rift-Damaged Corner | L2+L4+L5+L6 | Story corruption zone |
| Clean Combat Arena | L1 only (erase L4-L6) | Central fight area |
| Dirt Trail | L4+L5 | Path between encounters |
| Battle Aftermath | L4 dark + L5 rubble dense + L6 sparse | Post-combat dressing |
| Water Edge Smoothing | L4 + L5 driven by feature mask | Edge of water pools (uses Karar #143-K) |

---

## 7. UI Design

### 7.1 Layout

```
┌──────────────────────────────────────────────────────────────────────┐
│ RIMA Map Designer       [Room: Combat_01 ▼]  [Skin: Grimdark ▼]   ⚙ │
├──────────────────────────────────────────────────────────────────────┤
│ Toolbar:  [Pick] [Brush B] [Erase E] [Composite C] [Auto-Dress] ↶ ↷ │
├─────────────────┬─────────────────────────────────┬──────────────────┤
│ BRUSH PALETTE   │                                  │ BRUSH SETTINGS   │
│ (left, 260px)   │                                  │ (right, 280px)   │
│                 │                                  │                  │
│ 🔍 [search…]    │                                  │ Size:   [== 64==]│
│                 │                                  │ Density:[==0.5=] │
│ Category:       │                                  │ Min Dist: [32px] │
│ ▼ All           │                                  │                  │
│                 │       SCENE VIEW                 │ Rotation:        │
│ ┌───┬───┐       │       (paint target)             │  ☑ Random        │
│ │ 🟫 │ 🟫 │ Floor│                                  │  Snap [0°]       │
│ ├───┼───┤       │       ⬛⬛⬛⬛⬛⬛                 │ Flip:            │
│ │ 🟦 │ 🟦 │ Wall │       ⬛⬜⬜⬜⬜⬛                 │  ☑ X    ☐ Y      │
│ ├───┼───┤       │       ⬛⬜⬜·⬜⬛  ← brush ghost  │ Scale Range:     │
│ │ 🟢 │ 🟢 │ Trans│       ⬛⬜⬜⬜⬜⬛                 │  [0.85] → [1.15] │
│ ├───┼───┤       │       ⬛⬛⬛⬛⬛⬛                 │                  │
│ │ ✦  │ ✦  │ Rift │                                  │ Seed:    [12345] │
│ ├───┼───┤       │                                  │ ☑ Walkable filter│
│ │ 🎨 │ 🎨 │Compo │                                  │ ☑ Edge-bias x10  │
│ └───┴───┘       │                                  │ ☐ Feature mask   │
│                 │                                  │                  │
│ ━━━━━━━━━━━━━━ │                                  │ ── LAYER VIEW ── │
│ Selected:       │                                  │ L1 Floor   👁 🔒 │
│ "Moss Soft Oval"│                                  │ L2 Variation 👁  │
│ → L4 Transition │                                  │ L3 Wall      👁  │
│ Mode: Freeform  │                                  │ L4 Trans  👁 Solo│
│ Hotkey: [3]     │                                  │ L5 Detail    👁  │
│ ⌨ 1-9 quick     │                                  │ L6 Accent    👁  │
├─────────────────┴─────────────────────────────────┴──────────────────┤
│ STATUS: Stroke 14 sprites | Last: 8ms | Pool 142/500 | Seed: 12345  │
└──────────────────────────────────────────────────────────────────────┘
```

### 7.2 Key UX Rules

1. **Brush is always the central concept.** Layer is metadata, not a control.
2. **Layer panel = visibility/solo only.** Boyamak için DEĞIL.
3. **Composite brushes visually distinct** (🎨 icon, different border color).
4. **Selecting a composite expands a small detail panel** showing what it paints:
   ```
   "Mossy Broken Edge" affects:
   ├─ L2: floor_variation_dark (×0.35)
   ├─ L4: moss_oval_256 (×0.45, edge-biased)
   └─ L5: crack_decals_64 (×0.20)
   ```
   No surprises.
5. **Scene ghost preview** — semi-transparent mock of what the next click will paint, follows cursor.
6. **Hotkeys (Photoshop muscle memory):**
   - `B` = Brush mode
   - `E` = Erase mode
   - `[` / `]` = decrease/increase brush size
   - `Alt+Click` = eyedropper (pick existing brush in scene)
   - `Shift+Click` = straight line from last point
   - `Ctrl+Z` / `Ctrl+Shift+Z` = undo/redo
   - `1-9` = quick select first 9 brushes in palette
   - `Space+Drag` = pan scene view (Unity default)

### 7.3 Top bar

- **Room dropdown** — switches active `RoomRecipeSO` (loads its tile/wall/decal state)
- **Skin dropdown** — swaps `BiomeSkinSO` live, same map re-renders in 1-2 seconds
- **Settings gear** — opens preferences (snap defaults, color picker, brush pack import/export)

### 7.4 Bottom bar (automation)

```
[Auto-Dress Room] [Regenerate Decorative] [Clear Selected Layer] 
[Clear L3-L6]    [Save RoomRecipe]      [Randomize Seed]
[Preview Before/After]   [Export Brush Pack]   [Import Brush Pack]
```

---

## 8. Automation Features

### 8.1 Auto-Dress Room
Input: current `RoomRecipeSO` + `BiomeSkinSO` + seed
Process:
1. Read room walkable mask + wall edges
2. For each brush in `RoomRecipeSO.autoDressBrushes`, apply procedural placement:
   - Wall brushes → along perimeter polyline
   - Transition brushes → edge-biased scatter, walkable cells only
   - Detail brushes → Karar #143-E density curve
   - Accent brushes → sparse, only in "rift_zone" flagged rooms
3. All operations under one Undo group → user can undo entire dressing in one Ctrl+Z

### 8.2 Brush Along Edges
- Select a Wall brush
- Click "Brush Along Edges"
- Tool walks `wallEdges` polyline, places appropriate horizontal/vertical/corner/doorway sprites
- No manual click-per-segment needed

### 8.3 Regenerate Decorative Layers
- Preserves L1+L2 (base floor)
- Clears L3-L6
- Re-runs Auto-Dress with new or same seed
- Use case: "I like this floor layout, but try different decoration"

### 8.4 Smart Fill Selection
- Drag a rectangular selection in scene view
- Pick a brush (or composite brush)
- Click "Fill Selection"
- Tool fills only walkable cells inside selection, respecting density/walkable/edge rules

### 8.5 Biome Brush
- A meta-brush that switches `BiomeSkinSO` for a sub-region of the room
- Use case: half the room is ShatteredKeep stone, half is BloodMoor swamp transition
- Implementation: per-cell biome ID field in `RoomData`, painters read it

---

## 9. BiomeSkin / Render Preset

### 9.1 Why a separate concept

The same brush data ("paint moss here") can render two ways:
- **Hard pixel:** sharp alpha, no blending, snap to grid
- **Soft watercolor:** alpha8 fade at edges, multiply blend, sub-grid jitter

Without separation, you'd need two copies of every brush. With BiomeSkin:
- One brush definition
- N render rules per skin
- Swap skin → entire map re-renders in 1-2s

### 9.2 Four default skins

| Skin | L1-L2 Render | L3 Wall | L4 Patch | L5 Detail | L6 Accent | Mood |
|---|---|---|---|---|---|---|
| **Hades-Net** | Hard pixel | Hard pixel | Hard cut | Hard, edge-biased x10 | Sparse hard | Sharp, readable |
| **Soft-Painter** | Hard tile | SoftAlpha8 wall | SoftAlpha16 patch | Soft edge-biased x5 | Medium soft | Watercolor-ish |
| **Bold-Graphic** | Hard tile w/ outline | Thick outline | Flat cut | Uniform density | Dense | Dead Cells |
| **Grimdark-Mix** | Hard tile | Hard pixel | SoftAlpha16 patch | Hard edge-biased x8 | Bold | Hybrid (recommended default) |

### 9.3 Implementation

`AlphaMode` enum maps to either:
- `Hard` — default Unity sprite material
- `SoftAlpha8` / `SoftAlpha16` — sprite + post-process shader that fades edges per sprite mask
- `MultiplyBlend` — alternate material with multiply blend mode

`LayerRenderRule.overrideMaterial` allows per-layer custom shader (rare).

---

## 10. Integration With Existing Karar #143 Painters

**Critical: do NOT rewrite painters. The brush tool sits ABOVE them.**

```
┌─────────────────────────────────────────────┐
│  MapDesignerBrushTool (NEW — UI + dispatch) │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  BrushExecutor router (NEW)                  │
│  - Reads BrushLayerOperation                 │
│  - For each op, calls correct painter        │
└─────────────────────────────────────────────┘
                    │
       ┌────────────┼─────────────┬───────────┐
       ▼            ▼             ▼           ▼
┌────────────┐ ┌──────────┐ ┌──────────┐ ┌─────────┐
│ Tilemap    │ │ WallOver │ │ Trans    │ │ Detail  │
│ (L1/L2)    │ │ Painter  │ │ Painter  │ │ + Accent│
│ direct API │ │ (LIVE)   │ │ (LIVE)   │ │ (LIVE)  │
└────────────┘ └──────────┘ └──────────┘ └─────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  NaturalFeatureGraph (LIVE Aşama 2)          │
│  FeatureEdgeSmoothingPass (LIVE Aşama 2)     │
│  FeatureMaskSO (LIVE Aşama 2)                │
└─────────────────────────────────────────────┘
```

**The router exposes one API:**

```csharp
public interface IBrushExecutor {
    void ApplyStroke(BrushStroke stroke, BrushLayerOperation op, RoomData room, int seed);
}
```

Each PaintMode has its executor:
- `GridTileExecutor` → calls `Tilemap.SetTile`
- `FreeformDecalExecutor` → calls `TransitionBrushPainter.PlaceAt` or `DetailDecalPainter.PlaceAt` based on op.targetLayer
- `ScatterAlongStrokeExecutor` → walks stroke path, calls scatter with min-distance
- `CompositeStrokeExecutor` → loops through all `operations`, dispatches to other executors

Karar #143-D/E/K filters are enforced INSIDE the executor before calling the painter (the painters already enforce these too — double-belt for safety).

---

## 11. Sprint Plan

**Each sprint is independently testable. After each, user reviews + approves before next sprint.**

| # | Sprint | Scope | Effort | Deliverable |
|---|---|---|---|---|
| **1** | **Data Layer** | All SOs + enums + JSON round-trip + 3 sample .asset files | 1 day Codex | Compile pass + EditMode test pass |
| **2** | **Brush Execution Engine** | IBrushExecutor + 6 paint mode executors + router + integration with LIVE painters | 1.5 day Codex | EditMode tests for each paint mode, walkable/edge-bias enforcement |
| **3** | **Editor UI Refactor** | RimaMapDesignerWindow → 3-panel layout, brush palette, settings panel, scene tooling, ghost preview, hotkeys | 1.5 day Codex | Window opens, select brush → paint → see result |
| **4** | **Default Brush Pack** | 8–12 presets (Clean Stone, Subtle Variation, Hades Wall Cap, Moss Soft Oval, Dirt Patch, Crack Scatter, Rubble Cluster, Rift Scar, Mossy Broken Edge, Rift-Damaged Corner, Edge Trim) | 0.5 day Codex + 0.5 day manual tuning | 8-12 .asset files visible in palette |
| **5** | **PixelLab Asset Generation** | 14 wall + 6 transition + 6 detail + 3 accent = 29 sprites, soft-edge variants | 1 day gen + 0.5 QC | 29 sprites imported + bound to AssetPool .asset files |
| **6** | **Automation Features** | Auto-Dress, Brush Along Edges, Regenerate Decorative, Smart Fill | 1 day Codex | Button-based room dressing works |
| **7** | **BiomeSkin + Render Rules** | 4 BiomeSkin .asset, soft alpha shader, biome dropdown swap | 1 day Codex | Same room renders in 4 distinct visual styles |

**Total ≈ 7–8 days. No deadline pressure (user confirmed).**

---

## 12. Risks

| Risk | Mitigation |
|---|---|
| Composite brush spawns 50+ sprites/stroke → frame hitch | Spread spawn across frames, use GameObject pool, batch SpriteRenderer mesh |
| Soft alpha shader breaks pixel-perfect look on Hades-Net skin | Shader is opt-in per LayerRenderRule, Hades-Net uses Hard mode only |
| JSON round-trip loses Unity-specific references (e.g., Material) | Document path-resolvable fields explicitly, fall back to defaults on missing |
| Brush pack import collisions (two packs define same brushName) | Pack-scoped names with namespace prefix, conflict UI on import |
| Karar #143 painters change signature → router breaks | Pin interface contract, add EditMode regression test |

---

## 13. Open Questions for ChatGPT Review

These are questions I'd like cross-review on. Each is a decision point where I have a recommendation but would benefit from a second opinion.

1. **Composite brush — should it support nested composites?** (e.g., "Battle Aftermath" = "Mossy Broken Edge" + extra L6 corruption). Pro: more reusable. Con: dependency cycles risk. My lean: **no nesting in v1**, flatten on import.

2. **Brush stroke = single click or drag?** ScatterAlongStroke needs drag. FreeformDecal works for both. Should we unify to "click = single point, drag = stroke" universally? My lean: **yes, unified**.

3. **Edge-bias curve — AnimationCurve or named preset?** AnimationCurve is flexible but tedious to author. Named presets (linear/x5/x10/exponential) are simpler. My lean: **AnimationCurve with 5 quick-pick presets in Inspector**.

4. **Seed scope — per-stroke or per-room?** Per-stroke = each stroke deterministic but room is collage of seeds. Per-room = whole room reproducible from one seed. My lean: **per-room as default, per-stroke override available**.

5. **Should AssetPool support weighted picks?** (sprite[0] picked 50%, sprite[1] 30%, sprite[2] 20%). My lean: **yes, optional weights field, default uniform**.

6. **BiomeSkin swap performance — pre-bake or live?** Pre-bake all skin variants on save (4× memory). Live re-render on dropdown (1-2s delay). My lean: **live re-render**, cache last skin in memory.

7. **Standalone export format — single .brushpack folder or .zip?** Folder = git-friendly, easy to edit. Zip = single file, distribution-friendly. My lean: **both — folder is canonical, zip is export shortcut**.

8. **L3 wall placement — fully auto via "Brush Along Edges" or also manual click-by-click?** Auto is faster; manual gives control for irregular layouts. My lean: **both available, auto is default**.

---

## 14. Approval Checklist

Before Sprint 1 dispatch, confirm:

- [ ] Hybrid Unity-host + JSON-portable architecture approved
- [ ] 6-layer technical mapping kept (L1-L6 internal, hidden from user)
- [ ] Composite brush as primary "painter feel" mechanism approved
- [ ] BiomeSkin separation from brush data approved
- [ ] 7-sprint sequential plan (no parallel work) approved
- [ ] Hotkey scheme (Photoshop-like B/E/[/]/1-9/Alt-click) approved
- [ ] Karar #143-D/E/K enforcement at executor layer approved
- [ ] Open questions 1-8 reviewed (decisions can defer to Sprint 1 task spec)

---

## 15. Glossary

- **Karar #143** — RIMA decision #143, locks 6-layer map architecture (2026-05-15).
- **Aşama 1 / Aşama 2** — Phase 1 (flat floor 6-layer) and Phase 2 (water/elevation features) of Karar #143 — both LIVE in code.
- **L1–L6** — six map layers (Floor / Variation / Wall / Transition / Detail / Accent).
- **Walkable mask** — per-cell boolean array, `walkable[x,y] == true` means decorations may land there.
- **Edge-biased density** — Karar #143-E rule, density multiplier scaling with distance-to-nearest-wall.
- **FeatureMaskSO** — Karar #143-K, optional Texture2D mask multiplier for density.
- **BiomeSkin** — render preset that swaps visual style (alpha mode, tint, shader) without touching brush data.
- **BrushPack** — distributable bundle of brushes + asset pools + sprites, JSON serializable.

---

# ADDENDUM — Locked decisions after ChatGPT cross-review (2026-05-16)

## 16. V1 vs V2 Scope Split (LOCKED)

ChatGPT correctly flagged scope creep. To prevent "building tooling infrastructure instead of shipping the game," the following split is now binding.

### V1 — RIMA shipping (this 8-sprint plan)

- ScriptableObject-first data model
- **Minimal** JSON export/import (single-file round-trip only)
- Composite brush (flat, NO nesting)
- Karar #143-D walkable filter enforcement
- Karar #143-E edge-biased density curve
- Karar #143-K FeatureMask integration
- Executor router above existing painters
- 8–12 starting brushes only (no infinite ecosystem thinking)
- BiomeSkin live swap with **subtle alpha** (NO Gaussian blur — see §17)
- Editor UI: 3-panel + hotkeys + ghost preview
- Automation: Auto-Dress, Brush Along Edges (default auto), Regenerate Decorative, Smart Fill
- Per-room seed default, per-stroke override
- AnimationCurve + quick-preset templates for edge bias
- Weighted asset picks (optional weights field)
- Folder-canonical brushpack format, zip optional

### V2 — Post-ship ecosystem (NOT this plan)

- Marketplace metadata + author/license fields
- Namespace prefixing for pack imports
- Conflict resolution UI on import collision
- Biome Brush (sub-region painting + per-cell biome graph)
- Standalone editor migration
- Advanced soft alpha shader (multi-sample edge fade)
- Brushpack download manager
- Pack signing / versioning / migration
- Cross-project portability beyond basic JSON

**Rule:** Any feature creep into V2 territory during V1 sprints requires explicit user approval. The orchestrator rejects scope drift by default.

---

## 17. Soft Alpha Warning (LOCKED)

ChatGPT critical warning: soft alpha in pixel art easily becomes "blurry texture soup."

**The "Death's Door softness" target must come from:**
- Irregular pixel cluster breakup at sprite edges (organic silhouette)
- Decal overlap (multiple semi-transparent decals layered)
- Soft density distribution (edge-biased, not uniform)
- Organic shape variation across asset pool

**NOT from:**
- Gaussian blur
- Heavy alpha gradient fades
- Anti-aliased smooth curves
- Bilinear filtering on sprites

**Implementation rules:**
- `AlphaMode.SoftAlpha8` (8-pixel edge fade) is the **maximum** allowed
- `AlphaMode.SoftAlpha16` exists in enum but is **flagged for review** before use
- No `MultiplyBlend` mode on L3 wall (would destroy silhouette readability)
- Sprite import settings: **Filter Mode = Point (no filter)**, no compression

**Target hybrid look:** Hades-Net floor (sharp pixel) + organic L4 patches with irregular silhouettes (softness from shape, not blur).

---

## 18. Sprint Plan REVISION — L3 Wall as Priority Gate

ChatGPT identified L3 Wall Overlay as the critical missing piece. Without walls, rooms do not visually close. Sprint order revised:

| # | Sprint | Scope | Effort | Parallel With |
|---|---|---|---|---|
| **1** | **Data Layer (V1 minimum)** | All SOs + enums + minimal JSON + 3 sample .asset + EditMode tests | 1 day Codex | — |
| **2** | **Executor Router + L3 Wall + Brush Along Edges** | IBrushExecutor, BrushExecutorRouter, GridTileExecutor, WallStampExecutor, BrushAlongEdgesAutomation. Wraps existing `WallOverlayPainter`. | 1.5 day Codex | Sprint 3 starts in parallel |
| **3** | **PixelLab Asset Gen (L3 priority)** | 14 wall sprites first, then 6 transition + 6 detail + 3 accent = 29 total. L3 batch gates Sprint 2 visual validation. | 1 day gen + 0.5 QC | Parallel with Sprint 2 |
| **4** | **L4+L5+L6 Executors + Karar #143 enforcement** | FreeformDecalExecutor, ScatterAlongStrokeExecutor, StampExecutor. Wraps `TransitionBrushPainter` / `DetailDecalPainter` / `AccentPainter`. Walkable filter + edge-bias + FeatureMask. | 1.5 day Codex | — |
| **5** | **Editor UI Refactor** | RimaMapDesignerWindow → 3-panel layout, brush palette, settings panel, scene tooling, ghost preview, hotkeys (B/E/[/]/1-9/Alt-click/Shift-click), Undo grouping. | 1.5 day Codex | — |
| **6** | **Default Brush Pack + CompositeExecutor** | CompositeStrokeExecutor (flat). 8–12 brush .asset + 1 default BrushPack .asset. | 0.5 day Codex + 0.5 manual tuning | — |
| **7** | **Automation** | Auto-Dress Room, Regenerate Decorative Layers, Smart Fill Selection. All under single Undo group. | 1 day Codex | — |
| **8** | **BiomeSkin + Render Rules** | 4 BiomeSkin .asset, LayerRenderRule per L1–L6, live re-render + cache. Subtle alpha only. | 1 day Codex | — |

**Total:** 8–9 days of Codex work + parallel asset generation. No deadline pressure.

**Why L3 priority:** wall language defines the room silhouette. Without walls, every test scene looks like an open void. Transition/detail/accent only enhance an already-readable space.

---

## 19. ChatGPT Q1–Q8 Locked Answers

| # | Question | LOCKED Decision |
|---|---|---|
| Q1 | Nested composite brushes? | **NO** for V1. Flatten on import. Recursion risk + dependency cycles + balancing complexity. |
| Q2 | Unified click/drag interaction? | **YES**. Click = single point. Drag = continuous stroke. Universal across all paint modes. |
| Q3 | AnimationCurve vs named presets for edge-bias? | **AnimationCurve with quick-preset templates**. Inspector shows curve + 5 preset buttons (Linear, x5, x10, Exponential, Plateau). |
| Q4 | Seed scope? | **Per-room default**, per-stroke override available. RoomRecipeSO holds room seed; brush operation may override. |
| Q5 | Weighted asset picks in AssetPoolSO? | **YES**, optional `List<float> spriteWeights` parallel to `List<Sprite> sprites`. Empty = uniform. |
| Q6 | BiomeSkin swap — pre-bake or live? | **Live re-render** with last-skin cache in memory. 1–2s acceptable. |
| Q7 | Brushpack format? | **Folder canonical**, zip optional convenience export. Folder = git-friendly; zip = distribution. |
| Q8 | L3 wall placement — manual or auto? | **Both available, auto is default.** Brush Along Edges fires on click for auto. Manual stamp brush available for irregular layouts. |

---

## 20. Strategic Principle (LOCKED)

> **The purpose of the Map Designer is not to become a generic environment editor. Its purpose is to rapidly create visually rich, combat-readable RIMA rooms using layered artistic presets.**
>
> **Priority:** RIMA shipping tool first → Marketplace ecosystem later → Standalone editor later → Brushpack economy later.
>
> The tool must never become more important than the game itself.

This principle is binding on all sprint decisions. Any feature suggestion failing this test is automatically deferred to V2.

---

## 21. Updated Approval Checklist

Before Sprint 1 dispatch, confirmed:

- [x] Hybrid Unity-host + JSON-portable architecture approved (B route)
- [x] 6-layer technical mapping kept (L1–L6 internal, hidden from user)
- [x] Composite brush as primary "painter feel" mechanism approved (flat, no nesting)
- [x] BiomeSkin separation from brush data approved (subtle alpha only)
- [x] 8-sprint sequential plan with L3 wall priority approved
- [x] Hotkey scheme approved (Photoshop-like)
- [x] Karar #143-D/E/K enforcement at executor + data layer approved
- [x] Q1–Q8 decisions locked per §19
- [x] V1 vs V2 scope split locked per §16
- [x] Soft alpha warning locked per §17
- [x] Strategic principle locked per §20
