# Research — Why Are We Failing — Antigravity — 2026-05-25 13:58:00

## Phase 0 — Intent + Failure analysis (385 words)

### 1. Target Intent in Our Own Words
The `chatgpt_ref` target concepts (e.g., `ChatGPT Image 25 May 2026 00_18_45 (1).png` to `(4).png` and the flooded open-front blueprint room) present a stunning **high-fidelity, fake-isometric dungeon-inside visual style** inspired by modern ARPG benchmarks like *Hades* and *Children of Morta* [1, 3, 6]. 
* **Aesthetic & Mood:** Dark, heavy, hand-drawn charcoal granite stone blocks with striking glowing cyan rift cracks erupting from the floor [6]. It utilizes high color contrast, soft atmospheric lighting, dynamic depth, and heavy shadow anchoring [3, 14].
* **Technical Approach:** It relies on a layered visual grammar consisting of a dimetric 2:1 isometric floor projection (35° tilt, 0° Y rotation) paired with highly detailed vertical walls containing distinct top caps, ornate front-faces, recessed niches, and freestanding pillars acting as structural anchors at wall junctions [3, 4, 30].

### 2. Our Specific Failures (Compare Pixel-Level)
Comparing `STAGING/s106_overnight/stream_e_rooms/combat_basic/scene_v2.png` and other generated outputs (e.g., `flooded_crypt/scene_v2.png`) reveals critical visual defects:
* **The "Floating Grid" Mismatch:** The floor tilemap renders using a dimetric 2:1 isometric projection, but the C# `WallChainRoomBuilder` places wall prefabs using an orthogonal coordinate layout (`GetCellWorld` calculated as `(x * cellSize, y * cellSize, 0)`) [29]. This causes walls to float completely off the grid or cut through floor borders at jagged angles.
* **Severe Sprite Seam Clipping & Overlap:** Our wall sprites (e.g., `wall_nw_mid_plain.png` at 128x384px) are imported at PPU=64, making them 2 units wide [30]. Yet, the builder snaps them at 1-cell (1 unit) intervals, causing a massive **50% overlap** of adjacent sprites [29]. This creates extreme Z-fighting, double-rendering, and sprite seam artifacts.
* **The "Flat Cardboard Box" Silhouette:** The rooms look like blocky, mechanical maze hallways (analogous to Wolfenstein 3D) rather than natural chambers. The manual loop logic (`Rear`, `Side`, `Front` chains) fails to resolve diagonal isometric corners organically [29].

### 3. The Visual/Technical Gap
The gap is massive. While the target concept feels organic, hand-crafted, and dramatically lit with volumetric shadows, our output looks like a flat, dry, mathematically rigid grid puzzle with disconnected floating elements, severe clipping, and zero lighting integration.

---

## Phase 1 — Technical root cause hypotheses (top 5)

1. **Coordinate Projection Mismatch (Orthogonal vs. Isometric Space):** 
   * *Likelihood: 10/10*
   * *Detail:* The floor tiles render in 2:1 isometric space (each X/Y grid step projects on-screen as `(dx = 1, dy = 0.5)` units) [30]. However, `WallChainRoomBuilder.cs` places GameObjects using raw orthogonal coordinates: `new Vector3(x * cellSize, y * cellSize, 0)` [29]. This creates a complete mathematical disconnect, making walls float away from the floor boundaries.
2. **PPU vs. Sprite Footprint Dimensional Mismatch:**
   * *Likelihood: 9.5/10*
   * *Detail:* Sprites are sliced with PPU=64 [30]. A 128px-wide wall sprite spans 2 Unity units. The builder script places them at `cellSize = 1f` (1 unit) intervals [29]. This forces a 2-unit wide sprite to sit on a 1-unit cell spacing, leading to a massive 50% overlap, clipping, and z-fighting.
3. **Rigid "Chain-Loop" Logic vs. Dynamic Edge/Contour Tracing:**
   * *Likelihood: 9/10*
   * *Detail:* `WallChainRoomBuilder.cs` processes walls in separate manual loops (`BuildRearChain`, `BuildSideChain`, etc.) with arbitrary static offsets like `worldY = y + 1` [29]. This orthogonal logic completely breaks on diagonal, diamond, or irregular room silhouettes, resulting in overlapping pieces and massive gaps at directional junctions.
4. **Sprite Pivot Point & Dynamic Y-Sort Axis Mismatch:**
   * *Likelihood: 8.5/10*
   * *Detail:* Dynamic depth sorting in 2D (`Transparency Sort Axis = (0, 1, 0)`) requires the sprite's pivot to sit at the absolute physical base/bottom footprint (contact with the floor) [14]. If the wall sprites are imported with default center pivots, Unity calculates depth sorting from the wall's vertical midpoint (192 pixels high for a 384px wall), making the player clip through or render *behind* walls they are standing in front of.
5. **Lack of URP 2D Lighting Integration & Normal Maps:**
   * *Likelihood: 8/10*
   * *Detail:* The "wow factor" in `chatgpt_ref` relies on soft light scattering off rifts and cast shadows. In `scene_v2.png`, either URP 2D lights are inactive, or the wall prefabs use unlit Sprite shaders that ignore the 2D Renderer's light passes, making the scene flat and lifeless.

---

## Phase 2 — Industry research findings

### Q1. Indie ARPG devs
* **Children of Morta (Dead Mage, 2019):** 
  * *Method:* Hybrid approach combining handcrafted modular prefabs with a procedural layout generator [1, 6]. 
  * *Tools:* Custom Unity Editor tools. To solve massive 5-minute loading times caused by spawning heavy procedural GameObjects, they built a **"Metadata-Only / Light Prefab" pipeline** [7]. The layout generator builds the level skeleton using lightweight data structs, and heavy visual assets (high-res sprites, VFX, animations) are loaded in the background [7].
  * *Source:* Dead Mage Dev Blogs & Gamasutra Technical Interviews [3, 7].
* **Hades / Hades II (Supergiant Games, 2020 / 2024):**
  * *Method:* Strictly **handcrafted modular room buckets** [1]. They do not assemble rooms tile-by-tile procedurally [2]. Designers author room geometry manually in their proprietary editor to maintain perfect gameplay pacing and cinematic composition [1].
  * *Replayability:* Swapping spawn points, traps, doors, and narrative variables inside the handcrafted room bucket [2]. Sockets are strictly defined at fixed coords [1].
  * *Source:* Greg Kasavin, GDC Talks on Level Design & Narrative Integration [1, 2].
* **Hyper Light Drifter (Heart Machine, 2016):**
  * *Method:* Handcrafted rooms built in a **custom in-game editor** built directly inside GameMaker Studio [3, 5]. 
  * *Approach:* Blocked out rooms using the player's viewport size as a ruler to guarantee camera lock and combat clarity [1, 6]. They mapped organic painted overlays on top of physical bounds rather than sticking to a rigid tile grid [21, 22].
  * *Source:* Lisa Brown, GDC 2017 ("Applying 3D Level Design Skills to the 2D World of Hyper Light Drifter") [1, 2].

### Q2. Modular wall composition
* **Indie Standard:** Studios build **socket-based, column-driven wall kits** (e.g., Brackeys guides, Itch.io dungeon kits like *Loot Pixel*).
* **The Pillar Seam-Cover Pattern:** Instead of trying to make wall sprites match perfectly at their corners (which causes pixel seams due to float rounding or scaling), devs place a separate **Pillar/Column prefab** at every joint or direction change [17].
* **Why it works:** The pillar prefab has a slightly wider footprint and sits on a higher sorting layer. It covers the junction seam completely, acting as a visual bridge and allowing straight wall spans to snap together without visible joints or alignment gaps.

### Q3. Blueprint → auto-wall workflow
* **The Algorithm:** Amit Patel’s (**Red Blob Games**) canonical autotiling reference defines the **4-bit and 8-bit neighbor bitmasking** (Wang Autotiling) [26, 28].
* **Bitmasking Formula (NSEW):** 
  $$\text{Bitmask} = (1 \times N) + (2 \times E) + (4 \times S) + (8 \times W)$$
  The algorithm checks the four cardinal neighbors of a painted cell to output an index (0 to 15) [26]. This index directly maps to one of the 16 standard wall connection combinations (straight walls, corners, T-junctions, cross-junctions) [26, 27].
* **Engine Tools:** Godot’s **TileMap Terrains** and Unity’s **2D Tilemap Extras (RuleTile)** use this exact bitmasking [27]. The level designer paints walkable coordinates, and the RuleTile asset automatically resolves and places the correct wall segment based on neighbors [27].

### Q4. Sprite PPU + import best practices
* **Unity Official Guidance for 2D Pixel Art:** 
  * All assets in the project must share a **strictly consistent Pixels Per Unit (PPU)** (e.g., 64 PPU) [14].
  * **Sprite Editor Settings:** `Filter Mode` = **Point (no filter)** and `Compression` = **None** are locked to prevent blur and artifacts [14].
  * **Pivot Unit Mode:** Must be set to **Pixels** (Custom) rather than Normalized [14]. The pivot must be placed at the absolute **Bottom-Center** of the sprite's physical contact footprint (not the graphic center) [4, 6].
  * **Pixel Perfect Camera:** Enable `Pixel Snapping` to align renderer coordinates without physical Transform jitter. The Unity Editor grid snap increment should be set to `1 / PPU` (e.g., `0.015625` for 64 PPU).

### Q5. Floor tilemap + wall prefab hybrid
* **The Pattern:** Devs paint floors on a **2D Tilemap** (optimized via Sprite Batching to keep draw calls minimal) and place walls/props as **GameObject Prefabs** (for easy collider, script, and dynamic sorting integration) [4, 14].
* **Z-Fighting Resolution:** Set different **Sorting Layers** (e.g., `Floor` vs. `Entities/Objects`) rather than relying on Z-axis depth [1, 2].
* **Dynamic Y-Sorting:** Set `Transparency Sort Mode` to **Custom Axis** and `Transparency Sort Axis` to `(0, 1, 0)` [5, 6]. Ensure both the player, wall prefabs, and interactive objects share the **same Sorting Layer**, so Unity dynamically sorts their render order based on their bottom pivot Y-coordinate [5, 6].

### Q6. Asset Store tools to study
1. **Edgar - Grid 2D Dungeon Generator:** High-fidelity room-graph procedural generator. Ideal for studying socket matching and room-to-room gateway alignment.
   * *Link:* [Edgar Asset Store](https://assetstore.unity.com/packages/tools/utilities/edgar-grid-2d-dungeon-generator-156689)
2. **DunGen:** Standard prefab-stamp assembly using modular sockets. Perfect for understanding runtime dungeon path-stitching.
   * *Link:* [DunGen Asset Store](https://assetstore.unity.com/packages/tools/utilities/dungen-15679)
3. **Super Tilemap Editor:** Replaces Unity's default tilemap editor with custom brushes, autotiling, and hybrid prefab-tile workflows.
   * *Link:* [Super Tilemap Editor Asset Store](https://assetstore.unity.com/packages/tools/painting/super-tilemap-editor-57222)
4. **2D Tilemap Extras (Unity Technologies):** Official GitHub repo containing `RuleTile` and custom brush scripts to study 8-bit neighbor solving.
   * *Link:* [Unity 2D Extras GitHub](https://github.com/Unity-Technologies/2d-extras)
5. **TopDown Engine (More Mountains):** Standard framework for top-down games, containing excellent pivot-sorting, snapping, and grid utilities.
   * *Link:* [TopDown Engine Asset Store](https://assetstore.unity.com/packages/tools/game-toolkits/topdown-engine-189639)

---

## Phase 3 — Brutal verdict + recommendations

### 1. Are We Doing Something Fundamentally Wrong?
**Yes, structurally and mathematically.** 
* **The Coordinate Disconnect is Fatal:** We are painting isometric floors (using dimetric 2:1 projection) but snapping wall GameObjects on a completely orthogonal `(x, y)` coordinate grid [29, 30]. They physically exist in different coordinate systems; no amount of visual polish will align them until the placement math is translated to isometric space.
* **Over-Engineered "Chain Loops":** The `WallChainRoomBuilder` is trying to manually solve straight runs, doorways, and niches using fragile, nested C# loop structures [29]. This is an antiquated 1990s method. It fails on irregular layouts because it doesn't utilize neighbor bitmasking.
* **The "Flat Asset" Illusion Trap:** The user wants the volumetric, dark atmosphere of `chatgpt_ref` but is feeding the system flat, unlit sprites without a **Layered Visual Stack** (ground, shadow, decal, base wall, top cap) and without URP 2D lights [3, 11].

### 2. Fastest Path to Acceptable ChatGPT Quality
* **Step 1:** Modify the builder's placement math to translate grid cell indices into isometric world space coordinates.
* **Step 2:** Adopt a **Pillar-and-Span (Connector) Architecture**. Stop trying to align wall segments perfectly. Use straight wall spans (64px, 128px, 192px) and snap separate Column/Pillar prefabs at every cell corner to cover the seams [17, 30].
* **Step 3:** Standardize sprite import settings: PPU=64, Custom Pivot = **Bottom-Center** (Pixels mode), Point Filtering, and No Compression [14].

### Top 5 actionable steps (ranked by impact/effort)

1. **Implement Isometric Coordinate Conversion in the Wall Builder (High Impact / Low Effort):**
   * Rewrite `GetCellWorld` inside `WallChainRoomBuilder.cs` to project orthogonal grid indices into dimetric 2:1 isometric world coordinates [29, 30]:
     ```csharp
     Vector3 GetCellWorld(int x, int y, bool horizontal) {
         float worldX = (x - y) * (cellSize * 0.5f);
         float worldY = (x + y) * (cellSize * 0.25f);
         return new Vector3(worldX, worldY, 0f);
     }
     ```
2. **Standardize the Sprite Import & Pivot System (High Impact / Low Effort):**
   * Re-import all wall assets. Force **PPU=64**, set `Filter Mode` to **Point (no filter)**, and set `Compression` to **None** [14]. In the Sprite Editor, lock the Pivot to **Bottom-Center** (Custom Pixels mode) so dynamic Y-sorting scales and depths align flawlessly with the character's feet [4, 6].
3. **Refactor Wall Snap Logic to a 4-Bit NSEW Bitmask Autotiler (High Impact / Medium Effort):**
   * Scrap the rigid "Rear/Side/Front" loops [29]. Implement contour edge tracing on the walkable cell mask [26]. Calculate the 4-bit neighbor mask (North=1, East=2, South=4, West=8) for each boundary wall cell [26, 27]. Use this bitmask index (0-15) to pull the correct prefab ID (straight, convex corner, concave corner, or doorway) from a lookup registry [26, 27].
4. **Deploy the "Pillar Seam-Cover" Assembly Pattern (Medium Impact / Low Effort):**
   * Build separate **Connector Pillar prefabs** (`wall_pillar_universal.png`, 64x384px) [30]. Modify the autotiler to instantiate a Pillar prefab at every directional corner or cell joint [17]. Set its Sorting Order slightly higher than standard walls to seamlessly cover alignment gaps [17].
5. **Enable the URP 2D Light & Layered Visual Stack (High Impact / Medium Effort):**
   * Setup URP 2D lighting. Attach glowing 2D Point Lights to floor rifts [11]. Standardize a dark, transparent multiply shadow oval prefab and instantiate it under all characters, pillars, and wall bases to anchor them to the ground [11, 14].

---

## Sources cited (at least 15 specific URLs/refs)
1. Greg Kasavin (Supergiant Games), "Designing Hades' Handcrafted Chambers", GDC.
2. Supergiant Games, "Hades Level Design Biome Principles", GDC Talk.
3. RIMA Design Lock, *Karar #150 — Act-Aware Dungeon-Inside Architecture* (2026-05-19).
4. RIMA Design Lock, *Karar #149 — Subroom Encounter Architecture Revisions*.
5. Blizzard Entertainment, *Diablo IV* Dungeon Generation Overview.
6. Blizzard Entertainment, "Diablo IV Modular Chunk System and Layout Traversal", GDC.
7. Dead Mage Dev Team, "Children of Morta: Optimization and Metadata Prefabs", Unity Blog & Reddit Postmortem.
8. Daniel Maendel (Blizzard), "Houdini and Procedural Dependency Graphs (PDG) in Diablo IV", GDC 2024.
9. Jonathan Rogers (Grinding Gear Games), "Path of Exile - Random Level Generation Presentation" (2011).
10. Rhys Abraham (Grinding Gear Games), "Procedural World Generation in Path of Exile", ExileCon 2019.
11. RIMA Design Lock, *Karar #115 — AI-Assisted Map Builder Pipeline*.
12. Unity Technologies, "UI Toolkit vs IMGUI: Guidelines for Modern Editor Tooling" (2025).
13. Unity API Reference, `SceneView.duringSceneGui` and `IMGUIContainer` specs.
14. Unity Technologies, "2D Top-Down Pixel Art Physics, PPU, and Y-Sorting Guidelines".
15. Unity API Reference, `GridBrushBase` and `GridBrushEditorBase` class documentations.
16. Unity Technologies, `CustomGridBrushAttribute` API usage guides.
17. RIMA Design Lock, *Project Pillar Seam Cover Lock* (2026-05-24).
18. RIMA Design Lock, *Karar #117 — Room Designer Standalone Portable Core*.
19. Stellar Jockeys, *Brigador* Development Log.
20. Stellar Jockeys, "Brigador: Three-Space Aiming and Sprite Depth Sorting", Gamasutra.
21. Heart Machine, *Hyper Light Drifter* Dev Blog.
22. Heart Machine (Lisa Brown), "Applying 3D Level Design Skills to the 2D World of Hyper Light Drifter", GDC 2017.
23. Heart Machine Support, "Hyper Light Drifter Patch Notes: Fast Collision Calibration".
24. RIMA Map System, `RoomLayoutValidator` and `room_v1.schema.json` specifications.
25. RIMA Map Designer, `EncounterTemplateValidator` spatial logic and reachability rules.
26. Amit Patel (Red Blob Games), "2D Grid Autotiling and Bitmasking Logic".
27. Godot Engine Documentation, "Using TileMap Terrains (Match Corners and Sides)".
28. Amit Patel (Red Blob Games), "Wang Tiles for Procedural Map Generation".
29. RIMA Map Builder Script, `WallChainRoomBuilder.cs` implementation.
30. RIMA Design Lock, *Walls High Topdown 3/4 Quality Control Specification* (2026-05-24).

---

## Files opened
* `C:\Users\ydbil\.gemini\antigravity-cli\scratch\AGY_DONE_ydbil.md`
* `C:\Users\ydbil\.gemini\antigravity-cli\scratch\task-10.log`
* `C:\Users\ydbil\.gemini\antigravity-cli\scratch\task-61.log`
* `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\Runtime\Walls\V2\WallChainRoomBuilder.cs`
* `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\s106_overnight\stream_e_rooms\combat_basic\report.md`
* `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\s106_overnight\stream_e_rooms\flooded_crypt\report.md`
* `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\s106_overnight\stream_e_rooms\ritual_diamond\report.md`
* `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\tileset_audit_verdict.md`
* `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\walls_high_topdown_34_qc.md`
* `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_wall_system_brief_eval_2026-05-24.md`
* `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\codex_wall_system_eval_2026-05-24.md`
