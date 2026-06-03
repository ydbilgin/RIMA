# RIMA — 2D-Townscaper ISO AUTO-TILING: FOUNDATIONAL LOGIC BRIEF (decide the core math/system)

This is a DESIGN/LOGIC decision brief. We are NOT writing final code yet — we are LOCKING the foundational
mathematical + system logic of a 2D-Townscaper-style auto-tiling room editor, so that rooms like the references
can be built by select -> place -> auto-connect. Answer every section A-D concretely. Be a systems architect.

## Goal look (study these)
RIMA = 2D ARPG, ISOMETRIC diamond rooms (Diablo-Immortal / Hades). Target = the gameplay screenshots in
`STAGING/concepts/chatgpt_ref/` (esp. `ChatGPT Image 25 May 2026 00_18_45 (1).png`,
`new_chatgpt/ChatGPT Image 23 May 2026 21_29_23 (2).png`, `ChatGPT Image 22 May 2026 16_12_48 (4).png`) and the
asset-pack/room-build boards `blueprint_room/ChatGPT Image 24 May 2026 23_42_09 (1).png` (ADIM 1) and
`...23_42_10 (5).png` (ADIM 5). READ those images before answering. Depthful ruined-keep masonry, open-front
diamond rooms, columns, braziers, cyan rift, dark void framing.

## What already exists (verified live code — build WITHIN this, propose changes explicitly)
- `WangResolver.Resolve4(cell, isOccupied)` -> (WangShape{Single,End,Straight,Corner,T,Cross}, rotationDegrees, mask).
  4-neighbour bits N=1,E=2,S=4,W=8. CCW table VERIFIED (End S@0/E90/N180/W270, Corner S+E@0/N+E90/N+W180/S+W270,
  T openN@0/openW90/openS180/openE270). 20/20 unit + 12/12 runtime smoke pass.
- `WangResolver.EdgeMask8(cell, isOccupied)` -> 8-neighbour bitmask N=1,NE=2,E=4,SE=8,S=16,SW=32,W=64,NW=128.
- `RoomData` (ScriptableObject): per-layer cell lists (floorCells, cliffCells, wallCells[WallCell{cell,kind,shape,rotation,pieceId,height}]).
- `RoomDataMutator.AppendWallRun` (Bresenham line + footprint step + Wang re-resolve), `WangRebuild.ReorientWallCells`
  (dirty cell + 4 neighbours re-resolve), `WallRunBuilder` places sprites on a Unity `Grid`.
- Unity `Grid` supports `cellLayout=Isometric` -> `GetCellCenterWorld` returns diamond positions automatically.
- KNOWN GOTCHA (decided): depthful iso walls with vertical faces CANNOT be Z-rotated (front face lays on its side)
  -> resolved shape+rotation must map to a SPRITE-SWAP (per-facing sprite) + flipX, residual Z=0. Floor never rotates.
- Camera: Custom-Axis transparency sort (0,1,0), single "Entities" layer, SpriteSortPoint.Pivot, bottom-center foot pivots.

## DECIDE — answer all, concrete + with the math:

### A. FLOOR SYSTEM
1. Authoring model: mouse PAINT/ERASE, arbitrary brush size, on a chosen TERRAIN LAYER (stone / void / rift / etc.).
   How many layers, how do they stack/interact (does void erase stone? does rift overlay?). Painterly per the refs.
2. Auto-merge MATH: same-layer auto-blend. Which bitmask — 4-edge (16-tile) or 8-corner (47/blob "dual-grid")? For
   ISO DIAMOND floors specifically, which is correct and why? Define the exact tile set + the mask->tile mapping.
3. Diamond-grid neighbours: on an iso grid, what are the 4 (and 8) "neighbours" of a cell in CELL coords, and how do
   they map to the on-screen diamond edges? Confirm the resolver's N/E/S/W make sense as diamond edges.
4. Resolution pipeline: paint -> which cells get marked dirty -> re-resolve neighbourhood -> sprite-swap (no rotation).
   How big is the dirty neighbourhood? Determinism + interior variant selection (avoid obvious repetition).
5. Edge/rim treatment so a painted region reads as a real floor island with capped outer edges (ref look).

### B. WALL SYSTEM
1. Connection model: Townscaper-2D. Click = place + auto-connect to wall neighbours (Single/End/Straight/Corner/T/
   Cross). Drag = continuous run. Define the place/erase/drag interactions precisely.
2. DEPTH (girintili-cikintili / recessed-protruding): walls have height + 3 faces (top cap, front, side). Given the
   iso angle, the CONNECTION math must pick not just the shape but the FACING (which faces are visible: rear-facing,
   front-facing, side-facing, inner-corner vs outer-corner). Enumerate ALL connection states and the sprite+flipX
   each resolves to (extend the no-Z-rotate principle). How many distinct authored sprites are logically required?
2b. Inner corner vs outer corner: on a diamond room, the same Corner mask can be a concave (inner) or convex (outer)
   turn depending on which side is "inside the room". How does the math know inside vs outside? (occupancy of the
   diagonal? a room-interior flood-fill? an explicit facing hint?) THIS IS THE HARD PART — solve it.
3. Open-front rule: the camera-facing (south/front) walls must stay LOW or open so they do not occlude the hero.
   How does the system know which edges are "front" and swap to low-edge / cutaway pieces automatically?
4. Footprints != 1x1 (columns tall, arches 2-wide): how does the connection/placement math handle multi-cell and
   tall-overhang pieces without breaking neighbour resolution?

### C. THE UNIFIED MATH
1. When to use 4-bit cardinal (connectivity/shape) vs 8-bit corner (blending) — for floor vs walls. Justify.
2. The single resolution pipeline both share (paint/place -> dirty set -> neighbour re-resolve -> sprite-swap+flipX,
   Z=0 -> position at GetCellCenterWorld foot). Where do floor and wall logic diverge?
3. Inside/outside determination (the corner-facing problem in B2b) — give the concrete algorithm.
4. Performance + determinism (stable under repaint, no flicker, hash variants).

### D. ACHIEVING chatgpt_ref ROOMS
1. Show how this logic, end-to-end, produces a ref-style room: floor island + depthful connected walls + open front
   + columns/arch + void framing. Walk one concrete small room through the pipeline.
2. The minimal LOGICALLY-COMPLETE tile/piece set (floor tiles + wall facings) the masks emit — no more, no less.
3. Biggest risks / ambiguities in the math, and how to resolve them.

## Output
A structured proposal answering A-D with the actual math (bitmask tables, the inside/outside algorithm, the
state->sprite mapping). Concrete, not prose. This will be merged with two other independent proposals (cx + Gemini)
into ONE locked foundational-logic decision.
