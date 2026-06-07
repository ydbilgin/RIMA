Here is the foundational logic and system architecture brief for the RIMA auto-tiling room editor. This solves the core math required for depthful, isometric, open-front procedural rooms without relying on z-rotation.

### A. FLOOR SYSTEM
**1. Authoring Model**
- **Layers:** A background/clear color acts as the implicit **Void**. There are two explicit terrain layers: `BaseTerrain` (e.g., Stone) and `Overlay` (e.g., Cyan Rift). 
- **Interaction:** Painting `BaseTerrain` overwrites Void. Erasing `BaseTerrain` returns the cell to Void. `Overlay` is strictly painted on top of existing `BaseTerrain` cells. The painter handles arbitrary brush sizes by queuing cell coordinates, deduping them, and passing them to the resolution pipeline.

**2. Auto-merge MATH (8-bit vs 4-bit)**
- **Decision:** **8-bit corner/edge mask (47-tile blob/dual-grid).** 
- **Math/Why:** For isometric floors that form organic "islands," a 4-bit mask is insufficient. If a cell has floor neighbours at N, E, S, W but *not* NE, it is an **inner corner** of the void. A 4-bit mask (15) would treat it as a solid center, drawing floor over the void. The 8-bit mask: `Mask = N(1) + NE(2) + E(4) + SE(8) + S(16) + SW(32) + W(64) + NW(128)` ensures the system can pick sprites with recessed inner corners and cleanly capped outer cliffs.

**3. Diamond-grid Neighbours**
- Assuming Unity's standard Isometric Grid (Z-as-Y sorting, bottom-up):
- In CELL coordinates `(x, y)`:
  - `N` (Up-Left edge) = `(0, 1)`
  - `E` (Up-Right edge) = `(1, 0)`
  - `S` (Down-Right edge) = `(0, -1)`
  - `W` (Down-Left edge) = `(-1, 0)`
- The resolver's `N/E/S/W` directly map to the 4 flush edges of the isometric diamond. Diagonals (vertices) are `NE(1,1)`, `SE(1,-1)`, `SW(-1,-1)`, `NW(-1,1)`.

**4. Resolution Pipeline**
- **Trigger:** Paint cell `(x,y)`.
- **Dirty Set:** Mark a 3x3 neighbourhood around `(x,y)` as dirty in `RoomData`.
- **Re-resolve:** Loop dirty cells. Calculate `mask = EdgeMask8(cell)`.
- **Determinism:** Map mask to `SpriteId`. To select interior variants (cracks, rubble) without flickering on repaint, use spatial hashing: `VariantIndex = Hash(cell.x * 73856093 ^ cell.y * 19349663) % NumVariants`. Rotation is always Z=0.

**5. Edge/Rim Treatment**
- Sprites bordering the void (any mask < 255) are drawn with the depthful cliff face baked into the sprite. The sprite's visual bottom extends downward, but its pivot remains at the logical cell center.

---

### B. WALL SYSTEM
**1. Connection Model**
- **Click (Place):** Places a wall entity. Instantly re-resolves the 3x3 envelope. Connects automatically to adjacent wall entities using `WangResolver.Resolve4`.
- **Drag (Line):** Emits a Bresenham line array. Evaluates footprints, places walls, and runs the 3x3 resolve on the aggregate envelope.
- **Erase:** Removes wall. Adjacent walls downgrade (e.g., Corner -> Straight, Straight -> End).

**2. DEPTH & FACING (Inner vs Outer)**
- Walls use `WangShape` (4-bit) for connectivity, but isometric depth requires mapping this to **Facings** because we cannot Z-rotate. 
- **The Inside/Outside Algorithm (B2b):** To know which way a wall faces, the math samples the **Floor Layer**. A wall divides the room from the void.
  - *Algorithm:* Let Wall cell = `W`. For an `N + E` corner, the "interior" of the room lies either at the `SW` diagonal (making it an Outer/Convex wall facing the camera) or the `NE` diagonal (making it an Inner/Concave wall facing away). 
  - `bool isInner = Floor[W + NE_Vector]; bool isOuter = Floor[W + SW_Vector];`
- **State -> Sprite Mapping (No Z-rotation, FlipX allowed):**
  - `Single` -> Column/Pillar.
  - `Straight-NS` (UpLeft to DownRight), `Straight-EW` (UpRight to DownLeft).
  - `End-N`, `End-S`, `End-E`, `End-W` (Can be reduced to 2 sprites using `flipX`).
  - `Corner-Outer-FacingCamera`, `Corner-Inner-FacingAway`, `Corner-Left`, `Corner-Right`.
  - `T-Junction` (4 facings), `Cross` (1 facing).

**3. Open-front Rule**
- To prevent walls from occluding the hero, the camera-facing (front) walls must stay low.
- **Algorithm:** Calculate the Wall's **Normal Vector** after determining Inside/Outside. If the normal points `S`, `SW`, or `SE` (towards the isometric camera), apply the `Low` sprite variant (e.g., `Straight-EW` swaps to `Straight-EW-Low` — a cutaway ruined brick line).

**4. Footprints != 1x1**
- Walls remain strictly **1x1** in the logical bitmask array so neighbour math doesn't break.
- For columns or arches that are visually 2x wide or 3x tall, the sprite is simply authored oversized. Because the `SpriteSortPoint` is `Pivot` (Bottom-Center) and the camera uses Custom Axis Sort `(0, 1, 0)`, the overhang perfectly Y-sorts against adjacent grid cells without needing complex multi-cell footprint logic.

---

### C. THE UNIFIED MATH
**1. 4-bit vs 8-bit Justification**
- **Floors:** Use 8-bit (47-tile). They are area-fills. You must detect diagonal negative space to draw recessed floor rims correctly.
- **Walls:** Use 4-bit (16-tile) for structural graph connectivity, plus Floor-sampling for normal/facing. Walls are line-graphs; they only connect orthogonally.

**2. The Resolution Pipeline**
The paths diverge right after the dirty set generation:
- **Floor:** `mask = EdgeMask8(cell)` -> `SpriteId` -> Place at `GetCellCenterWorld()`.
- **Wall:** `(shape, rot, mask) = Resolve4(cell)` -> `Normal = GetInsideNormal(cell, mask)` -> Check Open-Front Rule -> Map to `(WallSprite, flipX)` -> Place at `GetCellCenterWorld()`.

**3. Concrete Inside/Outside Algorithm**
```csharp
Vector2Int GetInsideNormal(Vector2Int cell, int wallConnMask) {
    bool fN = hasFloor(cell.x, cell.y + 1);
    bool fS = hasFloor(cell.x, cell.y - 1);
    bool fE = hasFloor(cell.x + 1, cell.y);
    bool fW = hasFloor(cell.x - 1, cell.y);

    // Straight Wall Check
    if (wallConnMask == 5 /* N+S */) return fE ? Vector2Int.right : Vector2Int.left;
    if (wallConnMask == 10 /* E+W */) return fN ? Vector2Int.up : Vector2Int.down;

    // Corner Check (e.g. N+E = 3)
    if (wallConnMask == 3) {
        bool fSW = hasFloor(cell.x - 1, cell.y - 1);
        return fSW ? new Vector2Int(-1, -1) /* Outer */ : new Vector2Int(1, 1) /* Inner */;
    }
    // ... repeat for masks 6 (E+S), 12 (S+W), 9 (W+N)
    return Vector2Int.zero; // Freestanding/Bridge fallback
}
```

---

### D. ACHIEVING chatgpt_ref ROOMS
**1. Walkthrough to Target Look**
- User paints a 6x6 floor diamond. System triggers 8-bit resolution, generating a stone island with deep, dark, capped masonry rims overlapping the Void.
- User drags Walls along the outer perimeter. 
- North/East perimeter walls detect floor to their S/W. They resolve to `Straight`, normals point outward. They render as tall, depthful ruined keeps.
- South/West perimeter walls detect floor to their N/E. Normals point toward the camera. The Open-front rule intercepts and assigns them `Straight-Low` (low crumbled masonry), keeping the play area visible.
- User clicks an isolated tile inside the room. `Resolve4` returns `Single`. Renders as an upright Brazier/Column.

**2. Minimal Logically-Complete Tile Set**
- **Floor:** Exactly **47 unique sprites** (Z-rotation is banned, so we must author all 47 blob permutations to capture every inner/outer edge combo).
- **Walls (~20 base sprites):** 1 Pillar, 2 Ends (exploiting flipX), 2 Straights (NS/EW), 2 Straight-Low (front), 4 Corners (Inner/Outer * L/R via flipX), 4 Corner-Low (front), 4 T-Junctions, 1 Cross.

**3. Biggest Risks & Resolutions**
- **Risk:** Walls placed on empty space (bridges) without floor neighbours will return `Vector2Int.zero` from `GetInsideNormal`, causing facing ambiguity.
  - *Resolution:* Default `Vector2Int.zero` to act as "Facing Camera". This triggers the Open-Front rule, ensuring freestanding walls do not unintentionally block the camera as tall pillars. 
- **Risk:** Depthful concave inner corners occluding the player character.
  - *Resolution:* Strict enforcement of bottom-center pivots for all art assets. If the pivot perfectly aligns with the cell's center, the Custom Axis Sort will mathematically guarantee the player renders in front of walls located on cells "behind" them in screen space.
