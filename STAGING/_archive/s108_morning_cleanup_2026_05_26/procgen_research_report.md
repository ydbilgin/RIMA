# RIMA Procgen Research Report
*Generated: 2026-05-23 | Model: gemini-3.1-pro-preview (default)*

---

## Video: "How One Guy FIXED Procedural Generation" — Game Dev Buddies

### A. Video Analysis

**Techniques covered:**

1. **Stalberg Grid (Irregular Quadrilateral Grid):** Start from a hexagonal triangle mesh, dissolve specific edges into quads, then apply Laplacian Smoothing (relaxation). This breaks the rigid grid feel while maintaining topological connectivity. Result: organic-looking layouts that do not look hand-placed.

2. **Dual Grid Workflow:** Shift the grid so corners become cell centers. This simplifies autotiling math significantly — you only need to reason about how *corners* meet, not full face adjacency. Reduces unique tile asset count dramatically.

3. **Wave Function Collapse (WFC) / Model Synthesis:** Constraint propagation across the grid. Every cell starts as a "superposition" of all possible tiles. The solver collapses cells one by one based on adjacency rules until the whole grid is valid. The "fix" the title refers to is backtracking + constraint propagation to prevent dead-end contradictions.

**Dimension:** Primarily 3D (references Townscaper, Bad North). Grid math is engine-agnostic and applicable to 2D.

**Engine/Tools:** Unity + C#. Custom mesh deformation for the irregular grid.

**Key insight for RIMA:** The Dual Grid trick — if RIMA ever adds fine-grain autotiling beyond the current template system, the dual grid reduces the number of unique tile sprites required vs. a naive 4-corner blob tileset. Not immediately actionable given the locked template pipeline, but relevant if the tilemap layer expands.

**Channel specialty:** Game Dev Buddies specializes in Technical Art and Systems Reconstruction — breaking down complex AAA/high-indie visual systems (Death Stranding terrain scans, Hogwarts Legacy environment tech, Townscaper-style generation) and rebuilding them in Unity with Shader Graph and C#.

---

## Channel Survey

**Procgen-relevant videos on Game Dev Buddies:**

| Title | Topic |
|---|---|
| I build a complete procedural low poly terrain generator from scratch | Mesh gen, Perlin noise, falloff maps in Unity |
| I Was Tired Of EMPTY AAA Games! So I fixed them | Procedural world population — meaningful detail density |
| Procedural World Generation in Godot (Series) | Heightmaps + clipmap infinite terrain in Godot |
| I Made Oceans Feel ALIVE To Prove AAA Studios WRONG | Procedural water shaders + dynamic env effects |
| I Tried Re-creating Death Stranding Terrain Scan | Terrain visualization + procedural environment scanning |
| I Made A Procedural Forest In 10 Minutes | Scatter + procedural vegetation placement |

**Most relevant for RIMA (top-down 2D dungeon roguelite, hybrid chunk + decor model):**

1. **"How One Guy FIXED Procedural Generation"** (primary video) — WFC + dual grid directly applicable to RIMA tile logic
2. **"I Was Tired Of EMPTY AAA Games!"** — Procedural population density directly maps to decor overlay anchor logic
3. **"I Made A Procedural Forest In 10 Minutes"** — Scatter system patterns applicable to OverlayAnchor decor placement
4. The terrain + world gen videos are 3D/macro-scale and less directly applicable, but useful for noise pattern intuitions

---

## 5 Procgen Approaches for RIMA

### 1. Wave Function Collapse (WFC)

**What it generates:** Tile-level arrangement — ensures every 32x32 tile piece has valid neighbors. Can also operate at room-connection level (which room exits connect to which corridors).

**How it integrates with RIMA's locked architecture:**
- At tile level: WFC fills the tilemap *within* the 6 base templates with contextually valid tiles (floor near walls only, corridor tiles only where corridors are). Templates still provide the painted backdrop; WFC handles tilemap logic on top.
- At room-graph level: WFC can govern which of the 6 templates spawn at each graph node, given adjacency constraints (e.g., "boss room never adjacent to start room").

**Required input data:** An adjacency matrix — a list of "which tiles can be neighbors on which side." Authoring this is the main labor cost. Can be extracted automatically from a sample tilemap.

**Output:** Directly usable as a Unity Tilemap layer. Unity 2D Extras RuleTile partially covers this; full WFC is in the open-source `mxgmn/WaveFunctionCollapse` repo or the paid **Tessera** package (Unity Asset Store).

**Best fit for RIMA:** Room-graph connectivity layer + fine-grain tile variation within corridor sections.

**Pros/cons for solo dev:**
- Pro: Once rules are authored, infinite non-repetitive tile layouts for free
- Pro: Tessera handles 2D cases well with Unity integration
- Con: Rule authoring is front-loaded effort (2-4 days to get right)
- Con: Backtracking can cause hitches on large maps — keep tile grids small (max 30x30)

---

### 2. Cellular Automata (CA)

**What it generates:** Organic negative-space shapes — cave walls, irregular dungeon boundaries.

**How it integrates with RIMA's locked architecture:**
- Does NOT conflict with the 6 room templates. Use CA to generate the *void between* templates, not inside them.
- Workflow: Place templates via BSP nodes → fill surrounding void with 45% random wall noise → run CA for 5 iterations with template zones masked out → result is organic wall shapes connecting rooms.

**Algorithm:** Standard 4-5 Rule: `B3 / S12345` — a cell becomes wall if it has 5+ wall neighbors; becomes floor if it has fewer than 3. Run 4-5 iterations on the initial noise.

**Best fit for RIMA:** Act 1 "Shattered Keep" biome — rough stone walls with irregular edges between room chunks. Low code overhead (< 50 lines in C#).

**Pros/cons for solo dev:**
- Pro: Extremely low implementation cost
- Pro: Results look organic immediately
- Con: Does not guarantee connectivity — must run a flood-fill pass after CA to connect disconnected floor regions
- Con: Not useful inside hand-crafted rooms; only for the space between them

---

### 3. BSP + Random Walk (Drunkard's Walk)

**What it generates:** The macro dungeon layout — room placement graph + corridor connections.

**How it integrates with RIMA's locked architecture:**
- BSP defines zone rectangles. Each leaf zone hosts one of the 6 room templates (randomly selected, weighted by encounter type).
- After BSP partitioning, Drunkard's Walk carves corridors between zone centers. Walker starts at Room A center, moves toward Room B center with ±20% deviation, carves floor tiles as it goes.

**Algorithm sketch:**
1. BSP split dungeon canvas (e.g., 80x60 tiles) until leaf zones are ≥ template size
2. Select + place a template at each leaf zone center
3. For each BSP partition boundary, run one Drunkard's Walk to connect the two children
4. CA pass on corridor sections for organic edges (optional)

**Best fit for RIMA:** This is the primary dungeon-layout engine. The 6 templates become the "rooms"; BSP + walk gives the graph + corridors. This is the Diablo/Dead Cells model.

**Pros/cons for solo dev:**
- Pro: Guarantees connectivity (unlike naive CA or WFC)
- Pro: Rectilinear-to-organic spectrum controlled by walk deviation parameter
- Pro: Well-documented, minimal dependencies
- Con: BSP tends to produce slightly grid-like zone placement — add randomized room offset within zone to break this

---

### 4. PixelLab `create_topdown_tileset` + Edit Image Workflow

**Wang-compatibility:** PixelLab's `create_topdown_tileset` outputs a blob/Wang-style 47-tile set. Not guaranteed seamless without a specific prompt. Must explicitly request: *"32x32 seamless tileable dark gothic stone dungeon floor, top-down, pixel art, no perspective, flat lit."*

**Quality for dark fantasy dungeon style:** Medium-high. PixelLab outputs consistent pixel art style but defaults to a bright palette. RIMA's dark fantasy palette requires post-processing (palette swap or Levels/Hue adjustments in Photoshop or a Unity Shader LUT).

**Step-by-step workflow:**

1. **Generate:** Call `mcp__pixellab__create_topdown_tileset` with the above prompt. Request 32x32 output size.
2. **Audit seamlessness:** Download the sheet. Visually check tile edges under 2x zoom. If seams visible, re-prompt with `reference_image_base64` pointing to a known seamless RIMA floor tile.
3. **Palette sync:** Run through a palette-swap script (Python/Pillow or Unity Shader) to force output into RIMA's canonical dark fantasy LUT. This is the single most important quality step.
4. **AI edit refinement:** Feed the generated sheet into ChatGPT (gpt-image-1) with prompt: *"Darken the palette, add mossy cracks, maintain pixel art style, do not add new tile types."* Use img2img with low strength (0.3-0.4) to preserve tile boundaries.
5. **Slice:** Import to Unity. Use Sprite Editor → Grid by Cell Size: 32x32.
6. **Rule Tile:** Create a Unity RuleTile (from 2D Extras package). Assign the 47 sliced sprites to the blob/Wang rule positions.
7. **Test:** Paint a test Tilemap. Verify all 47 transitions render correctly.
8. **Iterate:** If 3+ transitions look wrong, go back to step 4 with targeted fix prompts.

**Limitations:**
- PixelLab credits: a full 47-tile topdown tileset costs ~50-100 credits. Do NOT generate speculatively — define the exact palette and reference image first.
- Resolution: 32x32 output is small. Upscale with nearest-neighbor only (no bilinear).
- Seamlessness: Not guaranteed on first generation. Budget 2-3 iterations per tileset.
- Edit Image (ChatGPT/Gemini) has no spatial awareness of tile boundaries — aggressive edits break seams. Keep strength low.

---

### 5. Poisson Disc Sampling + Perlin Noise (Decor Placement)

**Best fit from L-Systems / Voronoi / Noise options:** Perlin/Simplex Noise combined with Poisson Disc Sampling. L-Systems are for branching structures (trees, rivers) — not useful in dungeon rooms. Voronoi is useful for large biome partitioning but overkill for room-level decor.

**What it generates:** Weighted decor placement across RIMA's 28 decor objects onto OverlayAnchor spawn points.

**Mechanism:**
1. Generate a Simplex Noise map over the room footprint (seed changes per run).
2. Map noise values to decor categories:
   - High value (0.7-1.0): rare heavy objects — Statues, Altars, Rift Veins
   - Medium (0.4-0.7): common mid-weight — Torches, Banners, Debris piles
   - Low (0.0-0.4): sparse/empty — occasional Breakable Crates, nothing
3. Run Poisson Disc Sampling on OverlayAnchor positions with a minimum separation radius (e.g., 1.5 units). This prevents clumping — torches do not appear in clusters of 5.
4. Assign the decor object whose noise-mapped category matches the anchor's sampled noise value.

**Best fit for RIMA:** Decor overlay placement. This converts 6 static templates into effectively unlimited room variants without generating new art. The 6 templates feel like 600+ unique rooms.

**Solo dev cost:** ~1 day implementation. Unity has `Mathf.PerlinNoise` built in. Poisson Disc is a 30-line algorithm.

---

## 2D vs 3D

**Cutting 3D output for 2D rooms:** Not viable for RIMA. 3D noise terrain (Volumetric/Voxel) outputs mesh geometry, not pixel art sprites. Converting requires baking + manual cleanup — 10x the effort for no visual gain over direct 2D generation. Do not pursue.

**Value of 3D procgen for RIMA (explicit answer):** No, with one narrow exception. RIMA is pixel art top-down; all visual assets must read as flat chibi sprites in a 2D tilemap. 3D procgen engines (Houdini, Gaia, Wave Function Collapse 3D) operate in a coordinate system incompatible with pixel-perfect 2D URP rendering. The one exception: **3D baking for complex props** — model a statue or column in 3D (Blender), render from 90° top-down to a 64x64 sprite, import that sprite. This is not *procgen* — it is a one-time production step. No runtime 3D procgen pipeline for RIMA.

---

## PixelLab Wang + Edit Image Workflow (Detail)

**Can PixelLab MCP generate Wang tile sets?** Yes. `create_topdown_tileset` targets exactly this use case. The API supports blob tilesets (47-tile standard). It does not guarantee Wang 2-color or 3-color strict edge compatibility — "seamless" in PixelLab means the tile art tiles visually, not that the tile IDs follow the Wang encoding. You must import via Unity RuleTile and manually verify the 47 rule positions.

**Quality for dark fantasy dungeon style:** The base model is biased toward brighter fantasy palettes. Dark gothic dungeon outputs require: (a) a strong negative prompt ("no bright colors, no cartoon, no brown sand"), (b) a RIMA reference image via `reference_image_base64`, and (c) post-processing palette sync. With all three, quality is production-viable.

**Edit Image iteration:** ChatGPT gpt-image-1 img2img is currently the best option for RIMA-style palette enforcement because it respects pixel art style at low strength settings. Gemini's image editing model tends to over-smooth. Keep edit strength at 0.3 to preserve tile boundaries.

**Credit cost:** At current PixelLab pricing, budget 150-200 credits per finalized 47-tile topdown tileset (generation + 2-3 refinement rounds). Floor tiles for Act 1 = 1 tileset = ~150 credits. Wall edge tiles = second tileset. This is affordable but not disposable — do not generate without a locked prompt and a reference image.

---

## Recommendation

### Top 3 approaches RIMA should adopt

**Rank 1: BSP + Drunkard's Walk (dungeon layout)**
The mandatory backbone. BSP places the 6 room templates in a navigable graph; Drunkard's Walk carves organic corridors between them. Lowest effort-to-value ratio in the entire procgen stack. Implement this first.

**Rank 2: Poisson Disc Sampling + Simplex Noise (decor placement)**
The "secret sauce." Makes 6 templates feel like 600 rooms. Directly integrates with the existing OverlayAnchor system already scaffolded in `Assets/Scripts/Rooms/`. Second implementation priority.

**Rank 3: PixelLab topdown tileset + palette sync pipeline**
The tile art production workflow. Not runtime procgen — it is an offline asset production process. Enables high-quality floor and wall tilesets without hand-painting each tile. Priority: Act 1 floor → Act 1 wall edges → corridors.

### Top 2 to skip

**Skip: WFC for tile-level layout (for now)**
High rule-authoring overhead for a solo dev. The value is real but the investment is 2-4 days. The locked template system already handles room variety without WFC. Revisit post-launch or if corridor variety becomes a visible problem.

**Skip: L-Systems, Voronoi, 3D procgen**
None of these map cleanly to a 2D pixel art dungeon roguelite. L-Systems generate branching structures; Voronoi partitions are overkill for room-scale decor; 3D procgen adds pipeline complexity with no pixel art payoff.

### What to research next

- **GitHub:** `mxgmn/WaveFunctionCollapse` (open source, MIT) — bookmark for the future WFC corridor pass
- **Unity Asset Store:** Tessera ($50) — the most polished Unity WFC implementation for 2D tilemaps; evaluate when corridor variation becomes a priority
- **Game Dev Buddies channel videos:** "I Was Tired Of EMPTY AAA Games" and "I Made A Procedural Forest In 10 Minutes" — the scatter/population density logic maps directly to OverlayAnchor weighting
- **Poisson Disc Sampling reference:** Bridson 2007 "Fast Poisson Disk Sampling in Arbitrary Dimensions" — 30-line Unity implementation widely available; search "Unity Poisson Disc Sample" on GitHub
- **PixelLab tileset prompt refinement:** Before generating Act 1 floor tiles, run 3-5 low-credit test generations with varied negative prompts to find the palette sweet spot before committing to a full 47-tile batch

---
*Research by: RIMA Research Agent (Claude Sonnet 4.6) via Gemini CLI*
*Video source: https://youtu.be/Y19Mw5YsgjI*
*Channel: Game Dev Buddies*
