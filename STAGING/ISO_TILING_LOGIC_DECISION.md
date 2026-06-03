# RIMA — ISO 2D-TOWNSCAPER AUTO-TILING: LOCKED FOUNDATIONAL LOGIC (S6)

4-source convergence (Workflow / cx-Codex / agy-Gemini-3.5-Flash-High / agy-Gemini-3.1-Pro-High), adjudicated by Opus.
This is the canonical math/system spec for the iso diamond-room editor. Supersedes ad-hoc tiling assumptions.
Build the asset pack + tool WITHIN this. Grounded in verified code (WangResolver/FloorWangResolver/WallRunBuilder/
RoomData) + chatgpt_ref images.

## SOURCE AGREEMENT MAP
- **Unanimous (4/4):** no-Z-rotate → sprite-swap + flipX (Z=0); walls = 4-bit Resolve4 connectivity; bottom-center
  foot pivot + Custom-Axis(0,1,0) sort; deterministic spatial-hash variants; open-front auto-low-edge.
- **3/4 (Workflow+cx+pro):** inside/outside corner is decided by the FLOOR/interior field, not wall loops.
- **ADJUDICATED CONFLICT — floor merge bitmask:** Workflow+cx = **4-edge (16)**; both Gemini = 8-blob (47).
  **DECISION: 4-EDGE (16).** Proof below. The 47-blob is a square-grid solution misapplied to a diamond grid.

---

## §0. THE FLOOR 4-vs-8 ADJUDICATION (the one real conflict — locked)

**DECISION: structural floor = 4-edge cardinal Wang, 16 tiles. 8-neighbour data is overlay/AO/tie-break ONLY.**

Geometric proof (why 47-blob is wrong here):
- Unity `cellLayout=Isometric` is a square grid rotated 45° on screen. Cell (x,y) center ≈ screen `((x−y)·W/2, (x+y)·H/2)`.
- The 4 grid-CARDINAL neighbours (±x,±y) are the cells that share a full diamond **EDGE** (screen NE/NW/SE/SW).
- The 4 grid-DIAGONAL neighbours (±x,±y both) land at screen-cardinal positions (N/E/S/W) and share only a **VERTEX (point)**.
- A void diamond that is "diagonal" to cell A is, to the two cardinal cells B/C that actually flank it, an **EDGE** void —
  so B and C cap that rim correctly via their own 4-edge masks. **Every void↔floor boundary is some floor cell's edge.**
- The 47-blob exists to fill the concave **corner notch** that appears when *square* tiles meet edge-to-edge with an empty
  diagonal. On diamonds that region is a zero-area vertex — **no notch, no uncapped corner.** Both Gemini runs applied
  square-grid blob intuition without the 45° rotation; cx: "47/blob creates false diagonal bridges for iso floor islands."
- The shipped `FloorWangResolver.cs` is ALREADY 4-edge by deliberate design → no rework; it is correct as written.
- 8-neighbour `EdgeMask8` is retained for: floor overlay/decal smoothing (rift/moss/rubble), corner-sense tie-break
  samples (§B), and optional AO micro-detail near concave holes. NEVER as the structural floor tile selector.

---

## §A. FLOOR SYSTEM (locked)

**A1. Layer stack (cx's, adopted):**
1. ParallaxVoid (backdrop, not a cell). 2. **StructuralFloor** — exactly ONE terrainId per cell or empty (Stone/WetStone/
   Bridge/WalkableWater). Empty = void/hole; "paint Void" = ERASE structural (+ prompt-remove walls supported only by it).
3. Generated Rim/Edge (derived from missing structural edges). 4. **Overlay** terrains/decals (Rift/Water/Moss/rubble) —
   stack by priority, 8-neighbour blob smoothing allowed, NON-structural (never change connectivity unless tagged walkable).
5. Wall/LowEdge. 6. Props/Lighting. 7. generated Collision/Occlusion.
- Brush: cell-space stamp, arbitrary radius (circle=Manhattan, square=Chebyshev, line=Bresenham `GridLine`). Dedup → pipeline.

**A2. Merge math: 4-edge 16-tile (per terrainId).** Local bits N=1,E=2,S=4,W=8 (= `FloorWangResolver` key). Bit set = same
terrain across that diamond edge (interior/blend); bit clear = capped outer rim on that edge. 16-tile table = the shipped
`FloorTileName[16]`. Interior key 15 → 3-6 variants by `Hash(roomId,terrainId,x,y)%n` (deterministic, no list-order, no compose-time random).

**A3. Neighbours:** N=(0,+1) E=(+1,0) S=(0,−1) W=(−1,0) = the 4 diamond edges (screen NE/NW/SE/SW). Diagonals NE/SE/SW/NW =
vertices (overlay/tie-break only). Front edge set default `{S,W}`, rear `{N,E}` — a ROOM SETTING, not hardcoded (mirror-safe).

**A4. Pipeline:** brush→mutate→dirty = changed ∪ 8-ring (4-ring drives the 16-key, diagonal ring drives rim/overlay)→ for
each dirty floor cell: `key=same4Mask(cell)`, sprite=`floor_{terrain}_{name[key]}`, **rotation=0 always**, variant=hash →
remove generated output where structural gone → regen rim → compose at `GetCellCenterWorld`, centered. Idempotent; canonical
occupancy is source of truth (resolvedKey is cache only).

**A5. Rim:** every missing structural edge emits a capped rim (preferred: baked into the 16 silhouettes; flexible: separate
rim_{N,E,S,W} + rim_corner_{NE,SE,SW,NW} where two adjacent edges are missing, sprite-swapped not rotated). S/W (front) rims
get the stronger cyan-drip/broken underside (camera-visible); N/E rims shorter/darker (walls cover them). → "floating slab" look.

---

## §B. WALL SYSTEM (locked)

**B1. Connectivity = 4-bit Resolve4** (N=1,E=2,S=4,W=8) → graph shape ONLY {Single,End,Straight,Corner,T,Cross}. Click=place
node; Alt-click=exact/manual (one-off arch/cleanup); drag=Bresenham run (footprint-step); Shift=axis-lock; RMB=erase+re-resolve.
Dirty = changed ∪ 4-ring (connectivity) ∪ 8-ring (corner-sense + seam) ∪ multi-cell reserved cells.

**B2. Depth/facing — NO Z-ROTATE. Resolve to `spriteKey + flipX + rotationZ=0`.** Each wall cell resolves:
`connectMask4, shape, insideMask4, outsideMask4, cornerSense∈{None,Inner,Outer}, heightProfile∈{HighWall,LowFrontEdge,
Entrance/BrokenGap,Column}, facingClass∈{Rear,Front,SideL,SideR,Junction}, spriteKey, flipX`.
`spriteKey = wall_{material}_{heightProfile}_{orientationToken}[_{cornerSense}]_{facingClass}`.
**Logical state count = 20 per height profile** (12 non-corner: single+4 ends+2 straights+4 T+cross; 8 corner: 4 masks × Inner/Outer)
**× 2 profiles (HighWall + LowFrontEdge) = 40 per material** (→ **24 authored** if art approves the flipX mirror pairs:
end_N↔end_E, end_W↔end_S, corner_NE↔corner_WN, corner_ES↔corner_SW, T_openN↔T_openE, T_openS↔T_openW). flipX is an art-registry
optimization, NEVER a math assumption (fixed lighting can break mirrors) — logic always stores the explicit orientation token.
> Refines the earlier blueprint's WallFacingResolver: it must also derive cornerSense + heightProfile + facingClass from the
> floor interior field (below), not just shape→sprite.

**B2b. Inner-vs-Outer corner — FLOOR INTERIOR FIELD is authoritative (NOT diagonal-wall-occupancy).** (cx's algorithm; the
Gemini diagonal-only test is the degenerate fallback.) Build interior `I` = walkable StructuralFloor cells (minus ReservedVoid/
Pit/SolidOccupied, plus WalkableWater/Bridge). For a corner mask with connected legs a,b:
`quadrantScore(d1,d2) = I(w+d1)+I(w+d2)+I(w+d1+d2)`; compare qTurn (between a,b) vs qOpp (between −a,−b) vs qSides.
`turn>max(opp,sides)→Inner` · `opp>max→Outer` · else prev-stable → roomInsideHint → camera-away tie-break (Inner for room-boundary,
Outer for obstacle/island). Inner = interior between the two legs (normal perimeter corner); Outer = legs wrap a protrusion.

**B3. Open-front (automatic height profile, not hand-authored):** room settings `frontEdgeDirs={S,W}`, `rearEdgeDirs={N,E}`,
`minOpenFrontWidth=3`, `frontLowBandDepth=1-2`. Per wall cell: `outsideMask=¬I` per dir; if `frontExposure (outside∈front)` &&
`backInterior (inside∈rear)` → **LowFrontEdge**; explicit Entrance interval → Entrance/BrokenGap + no-collision; if a HighWall
would cover the hero-readability band → downgrade. ADIM-5 open-front: place NO graph nodes across the central ≥3-cell entrance;
LowFrontEdge on the L/R front lips; side chains step outward 1 cell every 2-3 to make the diamond footprint.

**B4. Footprints >1×1 — separate sockets from occupancy** (cx). `GraphNode` (Wang-connecting cell) ≠ `OccupiedCells` ≠
`VisualBoundsCells` (overhang for dirty/selection) ≠ `SupportCells`. 1×1 block: node=occupied. 2-wide arch = a `DoorSpan` record
with sockets at BOTH endpoints; under-arch cells stay walkable. Tall column = Single/Anchor, oversized sprite, occupancy 1×1
(FLAG 4), overhang affects sorting/occlusion/dirty ONLY — **tall overhang creates NO graph neighbour** (prevents an arch/pillar
"connecting through" every covered cell). Bottom-center pivot + Custom-Axis sort makes the overhang Y-sort correctly for free.

---

## §C. UNIFIED MATH (locked)

**C1. 4-bit cardinal** for: wall connectivity, floor structural silhouette, collision island continuity, open-front edge
detection. **8-bit** for: floor overlay/blob decoration, seam/rubble fillers, corner-sense tie-break, rim micro-detail. Reason:
connectivity & rim are edge-sharing (4-bit correct); diagonal touch is neither walkable nor wall-connected nor a shared edge.

**C2. Shared pipeline:** mutate canonical cells → collect dirty (changed ∪ neighbour ring) → recompute masks from canonical
occupancy → resolve logical state → resolve to `spriteKey + flipX + rotationZ=0` → compose at `GetCellCenterWorld` (foot-adjusted
for walls) → sort (Entities, Pivot, bottom foot). **Divergence:** floors use same-terrain occupancy → 16 silhouettes + rim, centered;
walls use wall-graph + interior field → 20 states/profile, foot-positioned, may reserve cells.

**C3. Inside/outside = floor occupancy is authoritative; never infer interior from wall loops first** (wall loops can be open-front
by design). Flood-fill is a fallback only (invalid for open-front → real rooms use floor occupancy).

**C4. Determinism/perf:** O(dirty) per edit; HashSet/Dictionary occupancy; sort dirty writes (z,y,x,layer) for stable serialization;
variant hash includes roomId+terrain+cell, never list index; re-resolve idempotent; **CRITICAL COUPLING — a FLOOR edit that changes
`I(cell)` must re-enqueue all WALL cells within Chebyshev radius 2** (inside/outside + front-low state can change even when wall
connectivity does not).

---

## §D. ACHIEVING chatgpt_ref ROOMS — pipeline walkthrough (9×7 open-front keep)
1. Paint Stone diamond (rows widen to center); side pockets = Water/Rift OVERLAY (keep island continuous for wall logic).
2. Floor resolves: interior=key15 fill variants; boundary=missing-edge keys + capped rims; S/W rims get cyan drip/void underside.
3. Drag rear wall chain (upper-L→upper-R): straights+rear corners+ends; interior toward floor → HighWall, facing=Rear.
4. Drag side chains stepping outward 1/2-3 cells for diamond width; corners resolve Inner/Outer via floor `I` (not diagonal guess).
5. Open front: leave central ≥3-cell span node-free; L/R lips = LowFrontEdge; any S/W-exterior + N/E-interior cell auto-downgrades.
6. Arch/door = DoorSpan with endpoint sockets; under-arch walkable. 7. Columns/braziers = Anchor/Prop, no forced connectivity.
8. Cyan rift = RiftOverlay (8-neighbour visual smoothing, deterministic). 9. Compose: floors centered, walls/props foot-positioned,
   Z=0, Custom-Axis(0,1,0)/Entities/bottom-pivot. → full readable combat floor, high rear masonry, side depth, low/open front,
   floating broken-slab rim, cyan cracks, void framing, props that don't block play. Matches the refs.

**Minimal logically-complete set per material:** floor = 16 silhouettes + 3-6 key-15 variants (+ optional 4 rim + 4 corner-join);
walls = 40 states (24 authored w/ flipX); + DoorSpans, Columns/Connectors, Seam/Filler, Lighting props as special pieces.

---

## §E. DELTA vs the earlier ISO_ASSET_PACK_BLUEPRINT_S6 (reconcile before generating)
- **Floor:** blueprint's 16-tile 4-edge is CORRECT (matches this lock). No change. `FloorWangResolver.cs` already shipped = correct.
- **Walls:** this lock makes the wall set RICHER than the blueprint's 18 sprites: it needs **inner/outer corner variants +
  HighWall/LowFrontEdge profiles + facing classes** (~40 logical, 24 authored per material) — the blueprint's flat 12-structural
  set is the HighWall subset only. The `WallFacingResolver` (task #7) must also output cornerSense + heightProfile (from floor `I`),
  not just shape→sprite. → Re-scope the wall imagegen batch to the 24-authored set, OR ship HighWall-only first (placeholder demo)
  and add LowFront + Inner/Outer in pass 2.
- **Footprint:** adopt cx's GraphNode≠Occupied≠VisualBounds split (cleaner than the blueprint's arch-span offset).
- **Data:** add resolved metadata fields (terrainId on floor; connectMask/insideMask/cornerSense/heightProfile/spriteKey/flipX on
  wall) — keep current RoomData fields for compatibility.

## LOCKED RECOMMENDATION (one line)
Structural floor = 4-cardinal 16-tile iso Wang + generated capped rim; overlays = optional 8-blob (non-structural); walls =
4-cardinal graph + floor-interior-field facing (inner/outer + open-front height); rendering = sprite-swap + flipX, rotationZ=0.
