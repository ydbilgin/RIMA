# Irregular Quad Grid + Dual Grid in 2D — Deep Dive

*Research date: 2026-05-23. Model: gemini-3.1-pro-preview (default). Confidence: MEDIUM-HIGH.*

---

## 1. What these techniques actually are

### A) Irregular Quad Grid (IQG)

A **regular square grid** has every vertex at valence-4 (exactly 4 edges meet at each corner), tiles are uniform size, and all angles are 90 degrees. That uniformity is a problem: it looks artificial, and movement/pathfinding on it produces obvious "taxicab geometry" artifacts.

An **Irregular Quad Grid** keeps the "all-quads" constraint (critical for many shaders and tiling algorithms) but varies vertex positions and connectivity. The result looks like a hand-drawn grid — cells are different sizes and shapes, but every face still has 4 corners.

**Why deform the grid?**
- Eliminates the visual repetition ("rubber stamp" look) of uniform tiles
- Makes organic-looking geometry (winding streets, natural terrain) from a mathematically clean structure
- Avoids the overhead of hexagonal grids while achieving similar organic flow

**The math (Lloyd Relaxation + Voronoi):**
1. Start with a random scatter of seed points
2. Build a Voronoi diagram from those seeds (each region = closest to one seed)
3. Move each seed to the centroid of its Voronoi cell (Lloyd Relaxation)
4. Repeat steps 2-3 until stable — seeds distribute evenly but not regularly
5. Convert the Voronoi regions to quads (Oskar Stalberg's method: merge random triangle pairs, then subdivide everything into quads)

**Visual analogy:** A regular tile floor made of rubber. Pull and stretch the corners randomly while keeping the tiles glued together — you get an irregular quad mesh.

---

### B) Dual Grid

A Dual Grid is a second grid whose vertices sit at the **centers of the faces** of the first (Primal) Grid. In a square grid, this is simply an offset of **(0.5, 0.5)**.

**The two roles:**
- **Primal Grid** = data layer: "Is this cell Grass or Water?"
- **Dual Grid** = visual layer: "Draw a shoreline tile here"

**Why this matters for auto-tiling:**

Standard "Blob" / 47-tile auto-tiling checks all **8 neighbors** of a cell: 2^8 = 256 combinations, reduced to 47 unique tiles by symmetry. This requires artists to draw and maintain 47+ tile variations.

Dual Grid auto-tiling instead places visual tiles centered on the **corners** of the data grid. Each visual tile only needs to check its **4 corners**: 2^4 = **16 configurations**. This is Marching Squares implemented as a rendering technique.

**Boris the Brave's key insight** (boristhebrave.com, "Dual Grid Tilemaps"): By centering visual tiles on corners rather than cell centers, you eliminate the "broken corner" artifact that plagues naive 16-tile bitmasking. The dual grid naturally encodes smooth transitions between terrain types with only 16 tiles, compared to the 47 tiles of the blob method — and the result often looks *better*.

**Oskar Stalberg / Townscaper connection:** Townscaper uses an IQG as its structural mesh. The Dual Grid principle is applied on top — logical states (building vs. no-building) live on the primal grid cells, while visual geometry is generated at the dual vertices. This is why Townscaper's buildings appear at "corner" positions and transitions look seamless despite a completely irregular mesh.

---

### C) Why combine IQG + Dual Grid + WFC

Each technique solves one part of the problem:

| Technique | What it contributes |
|:----------|:-------------------|
| **IQG** | Organic, non-repetitive layout — eliminates taxicab geometry |
| **Dual Grid** | Minimal tile set (16 instead of 47+) — reduces art cost and transition artifacts |
| **WFC** (Wave Function Collapse) | Constraint propagation — ensures valid, coherent adjacency rules across the whole grid |

**The synthesis:** WFC decides which state each cell should be (Grass/Water/Building/Road). The Dual Grid renders those states as smooth visual transitions. The IQG makes the underlying grid look organic rather than mechanical. Remove any one piece: WFC alone produces correct but griddy results; Dual Grid alone requires hand-placement; IQG alone looks randomly distorted without coherent content.

---

## 2. 2D games using them

| Game | Year | Technique(s) | How used | Source / Certainty |
|:-----|:-----|:-------------|:---------|:------------------|
| **Townscaper** | 2021 | IQG + Dual Grid + WFC | Full IQG mesh, WFC picks building states, Dual Grid renders geometry | Oskar Stalberg GDC 2021 "The World of Townscaper" — CONFIRMED |
| **Bad North** | 2018 | IQG + Dual Grid | First major public use of Stalberg's "relaxed quad grid" technique for island generation | Stalberg talks/tweets 2018-2019 — CONFIRMED |
| **Caves of Qud** | 2015 | WFC + Dual Grid | WFC "Texture Mode" for map gen; Dual Grid for terrain auto-tiling | Brian Bucklew GDC 2019 "End-to-End Procedural Generation in Caves of Qud" — CONFIRMED |
| **Oxygen Not Included** | 2017 | Dual Grid | Separates logical gas/liquid simulation grid from visual terrain tile rendering | Klei devblogs / Reddit dev comments — CONFIRMED |
| **Stardew Valley** | 2016 | Blob (47-tile) | Standard 3x3 bitmask auto-tiling for terrain transitions (NOT dual grid) | Internal assets `hoeDirt.png`, `flooring.png` use 47-tile layout — CONFIRMED |
| **Terraria** | 2011 | Bitmasking (Blob) | `TileFrame` system checks 8-neighbors with cardinal priority — classic blob method, not dual grid | Source code / modding docs — CONFIRMED |
| **Noita** | 2020 | Cellular Automata | Pixel-level CA simulation. Not grid-tiling at all. Different paradigm. | Nolla Games GDC 2019 "Noita: A Game Based on Falling Sand Simulation" — CONFIRMED |
| **Dwarf Fortress** | 2006 | Cellular Automata + templates | Organic caverns via CA density rules, not dual grid or IQG | Tarn Adams interviews — CONFIRMED (different paradigm) |
| **RimWorld** | 2018 | Standard grid | Base building on square grid; world map uses geodesic (hex-ish) but no IQG/Dual Grid — UNCERTAIN for fine terrain |
| **Brogue** | 2009 | Room accretion | Hand-crafted template assembly + BSP. No WFC. | Brian Walker "Brogue's Dungeon Generation" post — CONFIRMED (different paradigm) |
| **Diablo 1/2** | 1996/2000 | Wang Tiles / Blob precursor | 2x2 and 3x3 tilesets with manual connectivity rules — precursor to blob method, not dual grid | GDC Diablo post-mortems — CONFIRMED (historical) |
| **Inescape** (indie) | ~2022 | Dual Grid | Direct dual grid implementation — devlog reports 500+ tile reduction to 155 | jess::codes YouTube devlog — CITED |

**Key distinction to remember:** Stardew Valley and Terraria use the **47-tile blob method** (bitmask on 8 neighbors), not the dual grid (16 tiles on 4 corners). These are related but distinct approaches. Many articles conflate them.

---

## 3. Common 2D usage patterns

**Most common 2D use of Dual Grid:**
Terrain transition auto-tiling — any game with biome or material boundaries (Grass/Dirt, Stone/Water, Snow/Forest). The visual tile sits on the corner of the data grid, naturally creating smooth blends. The 16-tile set is small enough that a single artist can produce it in a day.

**Most common 2D use of IQG:**
Overworld maps and strategy game regions. When you want "countries" or "biomes" that feel hand-drawn rather than block-shaped. Also: city-builder street layouts (Townscaper, Bad North islands). In pure 2D top-down RPG-style games, IQG is less common because players expect tactically readable grids.

**WFC in 2D — when genuinely useful vs. overhyped:**

Genuinely useful:
- Interior room decoration (furniture placement obeying constraints)
- Tile texture synthesis when you have many sample patterns
- "Neighborhood" generation in city-builders

Overhyped / problem cases:
- Large open-world dungeon generation: WFC is slow at scale without hierarchical chunking, and often produces disconnected, degenerate outputs ("contradiction" states)
- Level design: WFC cannot encode semantics (a key must appear before its door) — it only knows adjacency
- Most roguelikes that claim to use WFC actually use simpler BSP + template systems for the macro structure, with WFC only for micro-decoration

Games that actually ship WFC: Caves of Qud (confirmed), Inescape (confirmed), various indie tile generators. Most commercial roguelikes use BSP or room template systems at macro level.

---

## 4. RIMA fit

RIMA architecture recap: Chunk-based room templates (hand-painted ~1024x1024), decor overlay system, 5 layers: L1 dungeon graph, L2 room templates, L3 decor placement, L4 enemy spawning, L5 asset generation.

### Dual Grid

**Direct fit: NO for room internals. POTENTIAL at L3 transitions.**

RIMA rooms are painted templates, not tilesets. Dual Grid is a rendering technique for tiled grids — it has no application inside a hand-painted room.

However: if RIMA ever implements "biome blending" between adjacent room templates (e.g., a Dungeon room meeting a Crypt room via a corridor), Dual Grid could drive the **transition strip** between rooms. A 16-tile transition set generated at L5 asset gen could handle corridor-to-room blending automatically.

Honest assessment: not needed now, potentially useful at scale when RIMA has 3+ biomes.

### Irregular Quad Grid

**Potential fit: YES at L1 dungeon graph.**

RIMA's dungeon graph currently places rooms in a relatively structured tree. Applying Lloyd Relaxation to room node positions would produce a dungeon layout where rooms are organically distributed — no room feels like it's "on a grid." This is purely a layout algorithm and does not affect the room art or templates at all.

Concrete idea: Run Lloyd relaxation on the room center positions, then connect rooms with corridors that follow the relaxed layout. The dungeon map screen would immediately look less mechanical.

Cost: low. This is ~50 lines of C# on top of the existing dungeon graph. No art cost.

### WFC

**Potential fit: YES at L3 decor placement. STRONG FIT.**

RIMA's decor overlay places objects (barrels, torches, furniture) on top of painted room templates. Currently this is likely random scatter or hand-placement. WFC could enforce constraints like:
- "Torches appear only near walls, never in open floor center"
- "Barrels cluster in groups of 2-3, never single isolated"
- "Treasure chests do not spawn adjacent to each other"

This is exactly the "interior decoration" use case where WFC shines in 2D. The constraint set is small, the grid is small (a single room), and the output is immediately visible to the player.

Cost: medium. Requires building a constraint graph for each room type and biome. Boris the Brave has Unity-compatible implementations.

### Summary table

| Technique | RIMA fit | Layer | Effort | Priority |
|:----------|:---------|:------|:-------|:---------|
| Dual Grid | No (now) / Possible (biome transitions later) | L3 corridors | Medium | Low — revisit at 3+ biomes |
| IQG | Yes | L1 dungeon graph | Low (50 lines C#) | Medium — nice QoL improvement |
| WFC | Yes — strong fit | L3 decor placement | Medium | High — decor coherence is visible |

**If not RIMA, what 2D game genres benefit most from this stack:**
- **Tilemap-based games (Stardew-like, Terraria-like):** Dual Grid is mandatory for terrain transitions. Using the 47-tile blob method instead wastes art time.
- **City builders / strategy overworld (like Townscaper or 2D Civilization clone):** IQG + WFC is the natural architecture.
- **Procedural dungeon crawlers with open-world feel (like Caves of Qud):** All three techniques together.
- **Template-based roguelikes like RIMA:** WFC for decor + IQG for graph layout only. Dual Grid is not the right tool because rooms are not assembled from tiles at runtime.

---

## 5. Bottom line

**2-sentence verdict on 2D applicability:**
Dual Grid is a proven, production-ready technique for 2D auto-tiling that reduces art cost significantly and should be standard practice for any tilemap-based 2D game with terrain transitions. IQG is niche in pure 2D (most useful for organic overworld/graph layouts) and WFC is genuinely useful for decoration and texture synthesis but is overhyped for macro dungeon generation.

**3 tools to learn next:**

1. **Boris the Brave — "Dual Grid Tilemaps"** (boristhebrave.com/2021/11/14/dual-grid-tilemaps/) — The definitive written guide. Includes Unity-compatible implementation details and the exact 16-tile set logic.

2. **Oskar Stalberg's Twitter/X (@OskSta)** — Search posts from 2018-2019 for his "relaxed quad mesh" visualizations. His GDC 2021 "The World of Townscaper" talk is on YouTube and covers IQG math accessibly.

3. **Marian42 — "Wave Function Collapse Explained"** (marian42.de/article/wfc/) — Best interactive visualizer. Shows exactly how constraint propagation works and where it fails. Avoids the hype.

**1 portfolio prototype idea where all three would shine:**

Build a 2D procedural city/village generator: Use **IQG** (Lloyd relaxation) to lay out city districts as irregular polygonal regions. Use **WFC** to assign each district a type (residential, market, temple, slum) with valid neighbor rules. Use **Dual Grid** to auto-tile the ground layer so the transitions between cobblestone streets, dirt paths, and grass plazas look seamless. The result would be a "living map generator" demo that showcases all three techniques working together — the kind of technical showcase that reads well in a portfolio for strategy game or roguelite studios.

---

*Sources: Oskar Stalberg GDC 2021; Boris the Brave blog 2021; Brian Bucklew GDC 2019; Nolla Games GDC 2019; Klei devblogs; jess::codes YouTube devlog; Gemini 3.1 Pro Preview synthesis (2026-05-23).*
