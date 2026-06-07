You are a senior technical/graphics game programmer. Solve a 2D isometric cliff PLACEMENT geometry problem for RIMA (Unity URP 2D). Give a concrete, implementable algorithm with the math worked out. Be precise and decisive.

SETUP (all measured, exact):
- Ground is an isometric Tilemap, cellSize (0.96, 0.585). For a cell-index delta (dc,dr) the WORLD delta is: dWorldX = (dc - dr) * 0.48 ; dWorldY = (dc + dr) * 0.2925. So screen directions: S=(-1,-1)=straight down, SE=(0,-1)=down-right, SW=(-1,0)=down-left, E=(1,-1)=straight right, W=(-1,1)=straight left, N/NE/NW = up/back.
- The floating island is a solid blob of floor cells in this iso grid, surrounded by VOID (no tile). It is viewed high-top-down 3/4; the camera looks "down the screen", so the player sees the SOUTH/front and side faces of the island, NOT the north/back face.
- Cliff art = sprites from CliffKit_RefB_pixelified, 128x192 px at PPU64 = **2.0 x 3.0 WORLD UNITS each** (so ~2 cells wide, ~5 cells tall), pivot = TOP-CENTER.
- Rendering/sorting: cliffs are on sorting layer "Floor", order = -30 + round(20 - worldY), which is BEHIND the Ground tiles (order 0). RESULT: any cliff pixel that overlaps a floor tile is OCCLUDED (hidden); only cliff pixels hanging over the VOID (where there is no floor tile in front) are VISIBLE.

CURRENT BROKEN APPROACH: for each floor cell, find the first void-facing dir in [S>SE>SW>E>W], place one cliff sprite per cell at the cell. Because the sprite is 2.0 units wide but a cell is ~1 unit, on W/E side edges and convex corners the wide sprite sticks out ~1.0-1.5 units SIDEWAYS into the void beyond the island silhouette, where there is no floor to occlude it -> ugly overflow past the silhouette. Scaling the sprite down is REJECTED (kills the chunky cliff look).

USER'S DIRECTION (design the math around this, do NOT scale):
1. Keep sprites full size; instead, POSITION + SORT so the sprite's TOP is tucked UNDER the floor (occluded) and ONLY the face hanging straight down below the silhouette edge into the void shows.
2. Use a SMART PER-CELL RULE that decides PLACE vs SKIP: in some cells place NO cliff at all (e.g. where one would overflow), rather than always 1-per-cell. Think it through logically/mathematically.
3. The result must look like a continuous thick rock underside hanging below the island's front silhouette, with NO sideways spill into the void.

ANSWER THESE, concretely:
A. PLACEMENT POSITION: for a placed cliff on an edge cell facing void-dir D, give the exact world position (relative to cellCenter, using the dWorld math) so the visible hanging part sits directly below the silhouette edge and the top is occluded by floor. Should the sprite be horizontally centered on the cell, or shifted toward floor-interior (-D) so its outer half lands over floor (occluded) and only the inner-but-downward part hangs? Work out the x-extent vs the floor silhouette so NO pixel lands in void sideways.
B. PLACE vs SKIP RULE: which edge cells get a cliff and which are skipped? Consider: straight S/SE/SW front runs (need continuous skirt) vs straight E/W side edges (seen edge-on, thin) vs convex corners (where wide sprites overflow) vs concave corners vs single-cell spurs. Propose the rule (e.g., "only place on cells where the floor extends >= X cells behind the sprite to occlude its top AND the sprite's outer x-extent stays within the union of floor cells' x-projection; otherwise skip" — make it concrete). Is it acceptable / better to place cliffs ONLY on the S/SE/SW front edges and skip pure E/W sides, or do sides still need a thin treatment?
C. CONTINUITY: how does spacing/overlap on straight front edges produce a seamless skirt without gaps, given sprite width 2.0 and cell pitch ~0.96?
D. GAP TEST: if we punch interior holes (1-cell, 3-cell, 4-cell) in the floor, how should the rule behave around an interior hole's rim (which faces void in many dirs) so it reads correctly and does not overflow into the hole?
E. Give final pseudocode for: for each floor cell -> decide skip/place + chosen dir + exact position + sort, including the skip predicate.

Be specific with numbers. This becomes a Unity execute_code implementation. Prose + pseudocode.
