# Iso Showcase Room — Composition Spec (S95)

> **Author:** rima-design (Opus)
> **Scene target:** `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity`
> **Status:** Spec ready for Codex UnityMCP build (no code in this doc)
> **Source assets:** Pilot A walls (LIVE) + iso floor tiles (LIVE) + Act1_ShatteredKeep library (LIVE).
>   No new generation.

---

## 0. Coordinate Contract (READ FIRST)

All cell coordinates are **Tilemap cell-coords** `(cx, cy)`. Codex MUST convert to
world via `Floor_Tilemap.CellToWorld(new Vector3Int(cx, cy, 0))` (then add any
listed offsets). This makes the Karar #148 Y-squash (parent.localScale.y = 0.819)
honored automatically, because the squash is on the Tilemap parent transform.

- Floor grid origin: cell `(0, 0)` = south corner of the room.
- Floor cells used: `cx ∈ [0..9]`, `cy ∈ [0..7]` → **10 × 8 cell room**.
- "World units" mentioned below = the world position **after** CellToWorld + offset.
- Offsets are given in **post-squash world units**, i.e. final Unity transform values.
- Z values: leave 0 for sprites; sortingLayer + IsoZAsY handles depth.

If Codex finds Floor_Tilemap parent localScale.y ≠ 0.819 → STOP and surface,
do not adjust per-object — Karar #148 is a single Tilemap-level squash.

---

## 1. Concept

**"Forgotten Rift Antechamber."** A combat-sized antechamber inside the Shattered
Keep where a cyan rift has split the back wall and is leaking into the room. A
toppled warrior statue lies near the rift, a stone altar sits off-axis as the
ritual focal point, and a torch-lit entry arch faces the player. The player walks
in from the south, eye is pulled diagonally up-left to the rift archway (cyan
focal), then sweeps right across the altar (warm orange focal) and exits through
the east arch.

This room is **not** a corridor, not a boss arena, not a merchant. It is a
**Combat sub-room** big enough to fight in (10×8 iso cells ≈ 12 cells of
walking area after walls/props) with enough props to read "this place has a
history" without crowding play space. Density target: **22-25% prop coverage**,
**~25% negative space** (the central diagonal walking band stays clear).

Mood mapping to the S95 reference:
- Cyan rift archway, partial back wall (reference's hero shot).
- Torch + brazier flicker (reference's warm-cold contrast).
- Toppled statue + rubble + skull pile (reference's "this place fell").
- Hanging banner + chains + cage (reference's vertical wall depth).
- Background silhouette (rat_king behind grate) — Diablo crowd-on-the-edge.

---

## 2. Room Dimensions

- **Cell grid:** W × H = **10 × 8** (cx 0..9, cy 0..7)
- **Diamond extent in world units** (rough, post-squash):
  - X extent ≈ `(W + H) * 0.5` = 9 units (south-corner to east-corner span)
  - Y extent ≈ `(W + H) * 0.25 * 0.819` ≈ 3.7 units
- **Origin gameobject:** new empty `IsoShowcaseRoom_S95_Root` at world (0,0,0).
  All hierarchy below it.

### Scene hierarchy (Codex creates exactly this)

```
IsoShowcaseRoom_S95_Root
├── Grid                 (Grid component, CellLayout=Isometric, CellSize 1/0.5/1)
│   └── Floor_Tilemap    (Tilemap; parent transform localScale = (1, 0.819, 1);
│                         TilemapRenderer sortingLayer=Ground, mode=IsoZAsY)
├── Walls_Root           (empty; pilot_a wall prefabs go here)
├── WallDecorations_Root (empty; banners/torches/skulls/chains)
├── Props_Root           (empty)
│   ├── Pillars
│   ├── Ritual
│   ├── Statues
│   ├── Containers
│   ├── Rubble
│   └── Floor_Hazards    (grate, pressure plate, spike trap)
├── Patches_Root         (empty; 3 patch sprites on Ground+1)
├── Decals_Root          (empty; small floor noise sprites on Ground+1)
├── Silhouettes_Root     (empty; far-corner mood sprites)
├── Lights_Root          (empty; URP 2D Lights)
└── PlayerSpawn          (empty marker at south entry; informational)
```

All Entities-layer prefabs (statues, mountings, decorations, props, silhouettes)
already use sortingLayer=Entities — do not override.
Walls explicit sortingLayer=Walls, sortingOrder=20 (per constraint #4).
Floor sortingLayer=Ground.
Patches & decals sortingLayer=Ground, sortingOrder=1 (above floor, below
Entities Y-sort).

---

## 3. Floor Layout (Floor_Tilemap)

**Strategy:** Weighted variant fill, NOT per-cell hand-painted, with **3 manual
overrides** for composition reasons.

### 3.1 Base fill — weighted

Iterate all cells `cx ∈ [0..9]`, `cy ∈ [0..7]` and assign:

| Variant | Weight | Tile asset (or RuleTile if available) |
|---|---|---|
| `act1_iso_granite_clean` | 60% | `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/act1_iso_granite_clean.png` |
| `act1_iso_granite_worn` | 30% | `.../act1_iso_granite_worn.png` |
| `act1_iso_granite_chiseled` | 10% | `.../act1_iso_granite_chiseled.png` |

Codex MUST seed the random with **`int.GetHashCode("IsoShowcaseRoom_S95")`** so
the result is reproducible (this is a showcase scene — drift = re-review pain).

If the PNGs are not yet wrapped into Tile assets, Codex creates `Tile`
ScriptableObjects under `Assets/Data/Tiles/Act1_ShatteredKeep/iso_v01/` with the
PNG sprite assigned. Do **not** generate new art.

### 3.2 Manual overrides (composition hot spots)

Apply AFTER 3.1. These are "the eye should land here":

| Cell | Tile | Why |
|---|---|---|
| (4, 6) | `granite_chiseled` | Beneath altar — ritually significant slab. |
| (5, 6) | `granite_chiseled` | Altar twin tile (altar straddles two cells visually). |
| (2, 5) | `granite_worn` | Beneath toppled statue — wear from impact. |
| (3, 4) | `granite_worn` | Just south of statue (path drag). |
| (1, 1) | `granite_chiseled` | South entry threshold (one tile reads "this is the door"). |
| (8, 4) | `granite_chiseled` | East exit threshold. |
| (5, 2) | `granite_clean` | Centerline walking band — keep clean so eye doesn't bog. |
| (6, 3) | `granite_clean` | Centerline walking band. |

### 3.3 What this gives us
- 60% clean → readable walking surface, not noisy.
- 30% worn → mass texture, breaks tile grid.
- 10% chiseled → "decorated" slabs land on focal points by chance + manual overrides.
- Iso seams sit at 45° (Karar #148) — fine, that's the whole point of the pivot.

---

## 4. Wall Layout (Walls_Root)

### 4.1 Pilot A piece inventory (what we have)

| Prefab | Role | Notes |
|---|---|---|
| `pilot_a_wall_face_EW.prefab` | side wall billboard along the East-West axis (cell rows of constant cx differ by cy) | The "long wall" |
| `pilot_a_wall_corner_outer.prefab` | outer 90° corner | Use at room corners that face into the room |
| `pilot_a_wall_arch_opening.prefab` | opening in a wall | Use for one entry; we reuse for second entry |
| `pilot_a_frame_0_face_NS.png` (PNG, no prefab) | NS face (the "short" wall) | Codex MUST wrap as a SpriteRenderer GameObject — see 4.4 |

We have **NO** corner_inner, NO end_cap, NO T_junction. The composition uses an
**asymmetric, half-open** wall plan so that the missing pieces are not needed.
This is intentional (memory: rima-visual-vision-reference-s95 specifies partial
walls / Hades-style — full perimeter is not the goal).

### 4.2 Wall plan (top-down, looking at the iso room)

```
                      NORTH (rift wall)
        [face_EW][face_EW][ARCH cyan][face_EW][face_EW]
   W                                                       E
   E   [face_NS sprite]              [face_EW (east face)]
   S   (one segment only)            [ARCH cyan exit]
   T                                 [face_EW (east face)]
                       (south wall: OPEN, this is where player enters)
                              SOUTH
```

The west wall has **only one short NS segment** (the rest is "broken away" — a
ruin opening, no asset needed because we just don't place anything there).
This sells the "shattered keep" theme without needing pieces we don't have.

### 4.3 Wall piece placements (exact)

All wall pieces parent under `Walls_Root`. SortingLayer=Walls, sortingOrder=20
on every Pilot A prefab's SpriteRenderer (already authored per Pilot A LOCK; if
not, Codex sets it). Pivot per memory: (0.5, 0.0313) — already in the prefabs.

Positions are given as **cell anchor + offset**, so Codex calls
`Floor_Tilemap.CellToWorld(Vector3Int(cx, cy, 0))` then adds the per-piece
offset listed.

**NORTH WALL (back wall, cy=7 row, rift line)** — 5 pieces:

| # | Anchor cell | Offset | Prefab | Rotation Y | Note |
|---|---|---|---|---|---|
| N1 | (0, 7) | (0, +0.4, 0) | pilot_a_wall_face_EW | 0 | Far-west NW corner segment |
| N2 | (2, 7) | (0, +0.4, 0) | pilot_a_wall_face_EW | 0 | |
| N3 | (4, 7) | (0, +0.5, 0) | pilot_a_wall_arch_opening | 0 | **PRIMARY FOCAL — cyan rift arch** |
| N4 | (6, 7) | (0, +0.4, 0) | pilot_a_wall_face_EW | 0 | |
| N5 | (8, 7) | (0, +0.4, 0) | pilot_a_wall_corner_outer | 0 | NE outer corner |

The +0.4..+0.5 Y offset lifts the billboard so its base sits at the back edge of
the cell row, not at the cell center. **Codex: verify visually**, this is a
billboard offset only; if the prefab pivot already does this, set offset to 0.

**EAST WALL (cx=9 column, side wall)** — 3 pieces; this side is the exit:

| # | Anchor cell | Offset | Prefab | Rotation Y | Note |
|---|---|---|---|---|---|
| E1 | (9, 6) | (+0.4, 0, 0) | pilot_a_wall_face_EW | 90 | (or use face_NS sprite if rotation looks wrong) |
| E2 | (9, 4) | (+0.4, 0, 0) | pilot_a_wall_arch_opening | 90 | **SECONDARY FOCAL — exit arch** |
| E3 | (9, 2) | (+0.4, 0, 0) | pilot_a_wall_face_EW | 90 | |

If `Rotation Y = 90` on face_EW looks wrong (sprite is a billboard, not 3D), then
**fallback:** use face_NS sprite (4.4) instead and orient by sprite mirroring.
Codex tries rotation first, if it shows a flipped/garbled face → fallback.

**WEST WALL (one short segment only — intentional ruin)** — 1 piece:

| # | Anchor cell | Offset | Prefab | Rotation Y | Note |
|---|---|---|---|---|---|
| W1 | (0, 4) | (-0.4, 0, 0) | (see 4.4 face_NS GameObject) | — | Lone west segment, the rest broken away |

**SOUTH WALL: NO PIECES.** This is the player's entry, intentionally open.
A pillar bracket (see Props §6) frames the opening implicitly instead.

### 4.4 face_NS GameObject (no prefab — Codex builds inline)

Because there's no NS prefab, Codex creates a GameObject:
- Name: `pilot_a_face_NS_W1`
- Parent: `Walls_Root`
- Component: `SpriteRenderer`
  - Sprite: `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_0_face_NS.png`
  - SortingLayer: `Walls`, sortingOrder: 20
  - Pivot: ensure the underlying sprite import uses pivot (0.5, 0.0313).
    If the PNG meta does not yet set this, Codex updates `.png.meta` accordingly
    (or sets custom pivot on the SpriteRenderer GameObject via an additional
    transform — preferred: fix the .meta).

### 4.5 Why this wall plan works
- Asymmetric: dense north (5 pieces), sparse west (1 piece), open south.
- Z-pattern eye flow: enter south → eye up-left to rift arch → diagonally right
  to altar → exit east arch.
- No straight-line repetition of 3+ identical pieces (we have ≤2 face_EW in a
  row before the arch breaks the rhythm).
- All missing piece types (corner_inner, end_cap, T) are designed around, not
  faked.

---

## 5. Focal Point Hierarchy

- **Primary focal:** Cyan Rift Archway (wall piece N3 + the rift_fracture_overlay
  sprite + 2D point light). This is the hero of the scene. Eye lands here first.
- **Secondary focal A:** Stone Altar (ritual prop at cells 4-5, 6) backlit by
  brazier_cyan_rift_flame to the north, brazier_orange_flame to the south. Two
  flames on the altar say "ritual was running when this place broke."
- **Secondary focal B:** Toppled Statue (cells 2, 5) — narrative beat, "the
  warrior fell here." Pulls eye to the west.
- **Detail layer (no focal weight, just texture):** chained pillar (E side),
  broken pillar (W side), wall decorations, treasure pile teaser, scatter
  rubble.

Visual weight balance:
- North half (cy ≥ 4): heavy (rift, altar, statue, north wall mass).
- South half (cy < 4): light (open floor + 2 pillars + south entry).
- East: arch exit + chained pillar (medium weight).
- West: ruin opening + broken pillar (medium-light weight).
- Diagonal walking band (cells from (1,1) → (5,3) → (8,4)) kept clear.

---

## 6. Props & Statues Placement (Props_Root)

All coordinates: cell + offset. SortingLayer=Entities (already set on prefabs).
Y-sort handles depth automatically.

### 6.1 Pillars (`Props_Root/Pillars`)

| # | Cell | Offset | Asset (PNG path; Codex wraps as SpriteRenderer if no prefab) | Why |
|---|---|---|---|---|
| P1 | (1, 6) | (0, 0, 0) | `act1_pillar_intact_cyan_crack_v01.png` | Frames north wall, far west. Cyan crack echoes rift theme. |
| P2 | (8, 6) | (0, 0, 0) | `act1_pillar_chained_v01.png` | Frames NE corner, story beat ("they chained something here"). |
| P3 | (2, 2) | (0, 0, 0) | `act1_pillar_broken_granite_v01.png` | South-west, fallen — signals "ruin starts here." |
| P4 | (8, 2) | (0, 0, 0) | `act1_pillar_intact_cyan_crack_v01.png` | South-east, mirrors P1 with cyan accent but on opposite quadrant. |

**Saçmalık check:** No pillar on cells (1,1), (5,2), (6,3), (8,4) — i.e. not in
the diagonal walking band. P1 and P4 use the same asset twice but on opposite
diagonal quadrants — this is **asymmetric reuse** (different visual neighbors),
not symmetric mirror.

### 6.2 Ritual (`Props_Root/Ritual`)

| # | Cell | Offset | Asset | Why |
|---|---|---|---|---|
| R1 | (4, 6) → (5, 6) | (+0.25, 0, 0) on cell (4,6) | `act1_ritual_stone_altar_v01.png` | **Secondary focal A** — straddles 2 cells visually, anchor on (4,6) and let sprite extend right. |
| R2 | (4, 5) | (0, 0, 0) | `act1_ritual_stone_marker_cyan_v01.png` | Just south of altar — reads as "approach the altar from here." |
| R3 | (6, 5) | (0, 0, 0) | `act1_ritual_tomb_headstone_v01.png` | East of altar; tilted (rotation Z = -8°) → "fallen over." Story: ritual disturbed. |

**Obelisk and stone_bench are NOT used** — would crowd the altar zone.

### 6.3 Statues (`Props_Root/Statues`)

Use exactly **2** statue prefabs. The library has 14; pick by role.

| # | Cell | Offset | Prefab | Why |
|---|---|---|---|---|
| S1 | (2, 5) | (+0.1, -0.1, 0) | **Toppled warrior** statue. Codex: pick a "toppled" variant from `statue_00..13`. Recommended: try statues 02, 05, 09 first; choose the one whose sprite reads as fallen/on-side. | **Secondary focal B** — narrative anchor. |
| S2 | (7, 6) | (0, 0, 0) | **Intact warrior** statue, pedestal. Codex: pick an "intact + pedestal" variant. Recommended start: statues 00, 03, 11. | Honor guard at the rift's east flank. Mirrors weight of P2 chained pillar. |

**Saçmalık check:** Only 2 statues, on opposite halves of the room. NEVER 3+ in
a row (rule). Codex MUST inspect the sprite preview to confirm the toppled vs
intact distinction — file name doesn't tell us.

### 6.4 Containers (`Props_Root/Containers`)

| # | Cell | Offset | Asset | Why |
|---|---|---|---|---|
| C1 | (1, 3) | (0, 0, 0) | `act1_prop_wooden_crate_stack_v01.png` | South-west cluster (with P3 broken pillar). Reads "supply dump." |
| C2 | (1, 2) | (+0.3, -0.1, 0) | `act1_prop_wooden_barrel_v01.png` | Adjacent to C1, slight offset → cluster reads natural, not grid. |
| C3 | (7, 5) | (0, 0, 0) | `act1_prop_pottery_urn_weathered_v01.png` | Near intact statue, ritual offering vibe. |
| C4 | (6, 1) | (0, 0, 0) | `act1_prop_treasure_pile_v01.png` | South-east, small reward tease near exit. **Do NOT center on path** — south side of cell. |

### 6.5 Rubble (`Props_Root/Rubble`)

| # | Cell | Offset | Asset | Why |
|---|---|---|---|---|
| RB1 | (3, 6) | (-0.2, +0.1, 0) | `act1_prop_rubble_debris_small_v01.png` | Beneath rift arch — wall collapse residue. |
| RB2 | (5, 7) | (+0.1, +0.1, 0) | `act1_prop_rubble_heap_skulls_v01.png` | Tucked against north wall east of arch. Adds depth + skull theme. |
| RB3 | (0, 4) | (+0.2, 0, 0) | `act1_prop_rubble_debris_small_v01.png` | West ruin opening — debris explains why the wall is missing. |
| RB4 | (3, 3) | (0, 0, 0) | `act1_scatter_bone_offering_pile_v01.png` | Mid-room scatter; ritual remnant. South of altar approach. |
| RB5 | (7, 1) | (0, 0, 0) | `act1_skull_pile_cluster_v01.png` | South-east, balances RB1 + bone pile weight. |

### 6.6 Floor hazards / mechanical props (`Props_Root/Floor_Hazards`)

| # | Cell | Offset | Asset | Why |
|---|---|---|---|---|
| FH1 | (5, 4) | (0, 0, 0) | `act1_prop_iron_grate_floor_v01.png` | Centerline — sells "something is below." Mob silhouette (§9) sits beneath. |
| FH2 | (3, 1) | (0, 0, 0) | `act1_prop_pressure_plate_v01.png` | Player walks over on entry — telegraph mechanical depth. |
| FH3 | (7, 3) | (0, 0, 0) | `act1_prop_spike_trap_dormant_v01.png` | Mid-east floor — adjacent to exit, "watch your step." |

**spike_trap_dormant must read as dormant**, not active. If the sprite reads
"active spikes", swap with another dormant variant or omit (do not show armed
spikes in a peaceful showcase).

**Pottery_urn, barrel, crate_stack** are at most 1 instance each in this room.
No duplication of containers — small library, would tile-up obviously.

**NOT USED in this room:** wooden_ladder (no vertical use case here),
lever_wall (we don't have a door mechanic to wire), ritual obelisk + stone_bench
(would compete with altar focal).

---

## 7. Wall Decoration Placement (WallDecorations_Root)

Hard rule (memory: feedback-wall-decoration-pure-attachment-only): every wall
decoration **MUST sit on a real wall piece**. No floating. Every decoration also
gets paired with a mounting prefab where the asset is a hanging element
(banner, cage, chain, lantern, skeleton, trophy). Wall sconce + candle bracket
provide their own mounting visual baked-in — no extra mounting prefab.

### 7.1 Pairing table

Each wall decoration is anchored to a wall piece (N1..N5, E1..E3, W1). The
"local offset" is relative to the wall piece's transform (post-instantiation).
The mounting prefab (where listed) is parented to the same wall piece and
placed first, then the decoration sprite parented under or beside the mounting.

| # | Wall piece | Decoration | Mounting prefab | Local offset (X, Y) | Why |
|---|---|---|---|---|---|
| WD1 | N1 (NW segment) | `act1_banner_red_torn_v02.png` | `mounting_00..14` — Codex picks a "banner pole" variant (try mounting_00, 04, 08 first; choose one whose silhouette reads as a horizontal pole/bracket) | (0, +0.15) | Red banner draws the eye to the west side and balances the rift cyan east. |
| WD2 | N2 (between west face & arch) | `act1_wall_torch_sconce_v02.png` | none (sconce includes bracket) | (0, +0.05) | **Light anchor** — warm flicker just to the left of the cyan arch. |
| WD3 | N4 (between arch & NE corner) | `act1_wall_torch_sconce_v02.png` | none | (0, +0.05) | **Light anchor** — second torch, framing the cyan arch from the east. Two flanking torches = portal framing trope. |
| WD4 | N5 (NE corner) | `act1_trophy_skull_v01.png` | mounting_* (pick a "shelf/peg" variant) | (0, +0.10) | Skull trophy at the corner — Diablo-style "they were here." |
| WD5 | E1 (east north) | `act1_chain_hanging_long_v02.png` | mounting_* (pick "ceiling hook") | (0, +0.20) | Long chain dangles from above — depth + texture. |
| WD6 | E1 (east north) | `act1_cage_iron_hanging_v01.png` | mounting_* (pick "ceiling hook") | (-0.15, +0.20) | Iron cage near the chain; tiny silhouette inside (could be skull_pile already inside cage sprite — verify, if not, don't add a sub-element). |
| WD7 | E2 (east arch) | `act1_lantern_hanging_v01.png` | mounting_* (pick "lantern hook") | (0, +0.18) | Hanging lantern over the exit arch — telegraphs "exit is lit." |
| WD8 | E3 (east south) | `act1_wall_candle_bracket_v02.png` | none | (0, +0.05) | Candle bracket — softer light, balances big torches on north. |
| WD9 | E3 (east south) | `act1_ivy_vine_v01.png` | none (vine drapes naturally) | (+0.10, +0.10) | Ivy creeper — adds organic asymmetry vs hard metal cage/chain on E1. |
| WD10 | W1 (lone west) | `act1_skeleton_shackled_v01.png` | mounting_* (pick "wall shackle/bracket") | (0, +0.05) | Shackled skeleton — single high-impact lore beat on the lonely west wall. |
| WD11 | W1 (lone west) | `act1_grate_iron_v01.png` | none (grate is its own anchor) | (+0.10, -0.10) | Iron grate at base — silhouette (§9) goes BEHIND this grate. |
| WD12 | N1 | `act1_chain_hook_short_v02.png` | none | (-0.15, +0.10) | Short chain near red banner — texture detail. |

**Saçmalık check:**
- Every decoration is on a real wall (N1, N2, N4, N5, E1, E2, E3, W1). ✓
- Every hanging element has a mounting prefab paired (banners, cage, chain,
  lantern, skeleton, trophy). ✓
- No two banners (purple/teal banners unused — would over-color the room). ✓
- No banners on N3 (arch) — would block the focal. ✓
- Two torches flank the arch (WD2, WD3) — intentional portal-framing. ✓
- W1 (lone west wall) carries the skeleton + grate combo = its own micro-story.
  Because it's the only west wall piece, it pulls weight without competing
  with the rift. ✓
- N4 has only 1 decoration (WD3 torch). N5 has 1 (WD4 skull). Not crowded. ✓

**13 wall decorations total** (some walls hold 2, some 1, some 0). Library has
15 wall decoration assets; we use 11 distinct (chain_long, chain_hook, cage,
banner_red, torch_sconce ×2, candle_bracket, lantern, skeleton, ivy, grate,
trophy_skull). Unused: banner_purple, banner_teal, trophy_bone, trophy_sword_iron
— intentional restraint (4-color budget per memory: cyan + orange + neutral
granite + red banner accent; purple/teal would bloat palette).

---

## 8. Lighting (Lights_Root)

URP 2D Point Lights. **5 lights** total — minimum 3 per constraint, we use 5 for
hierarchy. Every light is anchored to a sprite source (no floating light).

| # | Anchor source | Cell / world | Color | Inner R | Outer R | Intensity | Flicker | Why |
|---|---|---|---|---|---|---|---|---|
| L1 | N3 arch (rift) | cell (4, 7) +offset(+0.25, +0.2) | `#00FFCC` cyan | 0.5 | 3.5 | 1.0 | sine 1.5–2.5 Hz, amp 0.15 | **Primary light** — rift glow. Big, dominant. |
| L2 | WD2 torch_sconce | wall-local (N2) | `#FF8800` warm orange | 0.3 | 2.5 | 0.8 | sine 4–6 Hz, amp 0.2 | West flank torch. |
| L3 | WD3 torch_sconce | wall-local (N4) | `#FF8800` warm orange | 0.3 | 2.5 | 0.8 | sine 4–6 Hz, amp 0.2 | East flank torch. |
| L4 | brazier_orange (Props §10) | cell (4, 4) | `#FFAA44` warm orange-yellow | 0.4 | 2.0 | 0.6 | sine 5–7 Hz, amp 0.25 | Altar approach light — warm focal at altar. |
| L5 | brazier_cyan_rift | cell (5, 7) +offset(0, -0.2) | `#00FFCC` cyan | 0.3 | 1.8 | 0.5 | sine 2 Hz, amp 0.1 | Small cyan reinforce next to rift, ground-level. |

Brazier prefabs (no actual brazier prefabs in current library — they're PNGs):
- L4 anchor sprite: `act1_prop_brazier_orange_flame_v01.png` instantiated at
  cell (4, 4) under `Props_Root/Ritual` (it's a ritual fixture in this layout).
  This brazier serves as the "altar approach" beacon between the player path
  and the altar.
- L5 anchor sprite: `act1_prop_brazier_cyan_rift_flame_v01.png` instantiated at
  cell (5, 7) under `Props_Root/Ritual` (just inside the rift arch).

**Saçmalık check:**
- Every brazier/torch in the scene has a real Light2D component. ✓
- No light without an in-world flame source. ✓
- No light in player walking band cells (L4 at altar approach is OK — altar
  is a stop, not a pass-through). ✓
- Cyan vs orange counts: 2 cyan (L1, L5) + 3 warm (L2, L3, L4). Warm wins
  total count, cyan wins intensity (L1=1.0). Balance reads "warm room
  invaded by cyan rift." ✓

**Global light** (ambient): URP 2D Global Light, color `#1A1A2A` (cool dark
ambient per memory `#0A0810` is Act3 — Act1 stays slightly warmer/bluer-gray),
intensity 0.25. This keeps shadows dark but not pitch black.

---

## 9. Floor Patches & Decals

### 9.1 Patches (large soft overlay, `Patches_Root`)

3 patches available; use **all 3**, placed to reinforce composition zones.

| # | Asset | Cells covered (approx) | Anchor cell + offset | Alpha hint | Why |
|---|---|---|---|---|---|
| PA1 | `act1_patch_cracked_rubble_v01.png` | (2,5)–(4,5) | (3, 5) center | 0.85 | Under toppled statue + altar approach — sells the "impact" zone. |
| PA2 | `act1_patch_cave_moss_v01.png` | (0,4)–(1,5) | (0, 4) +(+0.3, 0) | 0.7 | West ruin opening — moss grew where the wall fell. Reinforces ruin reading. |
| PA3 | `act1_patch_dust_drift_v01.png` | (5,2)–(7,3) | (6, 2) center | 0.5 | South-east floor — light dust on the unwalked east approach. Low alpha so it doesn't compete with prop reads. |

Patches sortingLayer=Ground, sortingOrder=1, sprite scale 1.2–1.5 to feel
oversized vs. tile grid (per memory: "LARGE ORGANIC PATCHES" Alabaster Dawn
pattern is the rule).

Patches DO NOT cover focal points hot-spot tiles (altar twin chiseled tiles
(4,6)+(5,6), arch threshold (1,1), exit threshold (8,4)). Leave focal tiles
crisp.

### 9.2 Decals (small noise, `Decals_Root`)

Library has 16 decals (crack ×4, pebble ×4, dust ×4, bone_chip ×4). Use **12**
decals total, distributed by region:

| Region | Decals | Cell area |
|---|---|---|
| North band (rift zone) | 2 cracks + 1 bone_chip + 1 pebble | cy ∈ [5..7], any cx |
| South entry band | 2 pebbles + 1 dust | cy ∈ [0..2], cx ∈ [0..4] |
| East exit band | 1 crack + 1 dust + 1 pebble | cy ∈ [3..5], cx ∈ [7..9] |
| Center walking band | 1 dust | along (3,2)–(6,3) |

Pseudo-rule for Codex (no per-decal coord table — too granular; use seeded
random):
1. Seed = `int.GetHashCode("IsoShowcaseRoom_S95_Decals")`.
2. For each region, pick decal variants from the listed types.
3. Random position offset within the listed cell area: `(rand(-0.3, +0.3), rand(-0.2, +0.2))` from cell center.
4. SortingLayer=Ground, sortingOrder=2 (above patches).
5. Random rotation Z ∈ [-15°, +15°] (slight randomness).
6. Random scale 0.8–1.1.

**Hard rule reaffirm:** decals only on floor. Never overlap a wall piece's
visual footprint. Codex MUST check that the chosen offset stays in the floor
cell, not into a wall piece's billboard.

---

## 10. Decor Silhouettes (Silhouettes_Root)

Library: 8 silhouettes. Use **3** silhouettes — far-corner / behind-grate
placements for mood. Per memory rules: silhouettes are background dressing,
not gameplay, sit "in the dark."

| # | Silhouette | Position (cell + offset) | Sorting | Alpha | Why |
|---|---|---|---|---|---|
| SH1 | `decor_silhouette_rat_king_v01.png` | cell (0, 5) + (-0.2, -0.1, 0) | sortingLayer=Entities, sortingOrder=-5 (behind walls? — see note) | 0.5 | Behind the iron grate (WD11) on lone west wall. Half-visible threat. |
| SH2 | `decor_silhouette_specter_ghost_v01.png` | cell (5, 7) + (0, +0.4, 0) | sortingLayer=Walls, sortingOrder=18 (just under wall pieces at 20) | 0.4 | Inside the rift arch opening — drifts in the cyan. |
| SH3 | `decor_silhouette_husk_v01.png` | cell (9, 5) + (+0.3, 0, 0) | sortingLayer=Entities, sortingOrder=-3 | 0.45 | Just outside east arch — implies "what waits in the next room." |

**Sorting note:** For SH1 we want it to read as *behind the grate on the wall*.
Since the grate (WD11) is on sortingLayer=Walls/order=20, the silhouette can go
on sortingLayer=Entities at Z=+0.5 forward of the wall but rendered with alpha
0.5 so the grate's silhouette wins. Easier solution: put SH1 also on
sortingLayer=Walls at sortingOrder=19 (just under grate's 20). Codex picks
whichever matches Pilot A wall renderer setup; if grate is its own
sortingOrder, ensure SH1 = grate.sortingOrder - 1.

**Saçmalık check:**
- No silhouette in player walking path. ✓
- No silhouette competes with primary/secondary focals (rat_king is on the
  west fringe, husk is past the exit, specter is INSIDE the rift = reinforces
  primary focal). ✓
- No 3+ silhouettes piled — they are spread to corners. ✓
- Cyan slime / cyan wisp NOT used — would echo rift color too much and steal
  rift focal. ✓

---

## 11. Rift Accent Overlay

`act1_rift_fracture_overlay_v01.png`:
- Instantiate as child of wall piece N3 (the cyan arch).
- Local offset: (0, +0.15, 0)
- SortingLayer=Walls, sortingOrder=21 (one above the wall piece's 20).
- Alpha: 0.85
- Scale: 1.0
- Add a subtle pulse: scale.x oscillates 0.97–1.03 at 0.8 Hz (cheap, no shader).

This overlay is the cyan crack veining INTO the wall around the arch — sells
"the rift is actively cracking the keep." It's the single most important detail
sprite in the room.

---

## 12. Eye Flow Map

```
ASCII top-down (N up, E right). Numbers/letters = focal weight.

cy=7  W W W N3=** W W W W
       .  .  .  ✱✱✱ ...        ✱ = primary focal (rift arch)
cy=6  P1 .  .  R1*R1 S2 .
       .  .  .  ●●● ..         ● = secondary focal A (altar)
cy=5  PA2 . S1* . R3 . C3
       . moss ▲ ..             ▲ = secondary focal B (toppled statue)
cy=4  RB3 . . FH1 L4 . . E2=*
        .  .  .grate ..        ★ = secondary focal (exit arch)
cy=3  . . RB4 . . FH3 . .
cy=2  . P3 C1 . . . P4 .
cy=1  . . FH2 . . . C4 .
cy=0  . . . . . . . .  ← player enters south
       cx 0  1  2  3  4  5  6  7  8  9
```

Flow:
1. Player enters at south (low cy, mid cx). Eye lands on threshold tile (1,1).
2. **Eye is pulled up-left** by the warm-cool contrast: torch L2 (cool dark
   north-west of player) + the red banner (WD1) + the rift's cyan halo.
3. Eye **stops at the rift arch (N3)** — primary focal. Stays ~1 sec.
4. Eye drifts **diagonally down-right** following the warm pull of L4 brazier
   onto the **altar (R1)** — secondary focal A.
5. Eye sees the **toppled statue (S1)** in peripheral on the left — secondary B.
6. Eye sweeps **right** to the **exit arch (E2)** with hanging lantern — exit
   read.
7. Player walks. Decals + patches + silhouettes do their dressing job without
   ever grabbing focal attention.

---

## 13. "Neden Hades+Diablo" Justification

5 specific composition decisions tied to the S95 visual vision reference:

1. **Asymmetric walls (Hades pattern).** Full north wall + partial east wall +
   one west segment + open south. Hades rooms break perimeter intentionally to
   sell "this is part of a bigger ruin." Symmetric perimeter reads as
   tile-editor. Decision: north 5 pieces, east 3 pieces, west 1 piece, south 0.

2. **Cyan-rift focal centered on north wall (Reference parity).** The S95
   reference image shows the cyan archway as the centerpiece. We placed N3 at
   `cx=4` (mid of a 10-wide room), not perfectly centered (would be cx=4.5)
   — slight off-center makes it not-symmetric-mirror.

3. **Three-tier focal hierarchy (Diablo trope).** Primary = rift arch (cyan,
   biggest light L1=1.0 intensity). Secondary = altar (warm L4 + brazier).
   Tertiary = toppled statue (no own light, only the patch_cracked_rubble PA1
   under it). Eye knows the rank.

4. **Layered depth (Hades+Diablo both).** Foreground = south rubble + container
   cluster (P3 + C1 + C2). Mid = altar + statues + pillars. Background = north
   wall + rift + 2 silhouettes (rat_king behind grate west, specter in rift).
   Player camera angle (35° iso) reveals the layering naturally.

5. **Restrained palette (Hades discipline).** Cyan (rift) + warm orange (torches
   + braziers) + neutral granite + 1 red banner accent. Purple/teal banners
   intentionally NOT used. Per memory: 4-color zone budget. Painted-look
   readability requires this restraint.

---

## 14. Saçmalık Pass (self-review)

I (rima-design) re-read this spec once. Concerns and resolutions:

| Concern | Resolution |
|---|---|
| Floating wall decoration risk | §7 every decoration paired to a real wall piece + mounting. ✓ |
| 3+ statues in a row | Only 2 statues, opposite halves. ✓ |
| Brazier without light source | All 5 lights anchored to sprite sources (2 torches, 1 cyan brazier, 1 orange brazier, 1 arch). ✓ |
| Decal on wall | §9.2 explicit "decals only on floor." ✓ |
| Mounting prefab inside without decor pair | §7 every mounting paired to a decoration. ✓ |
| Iso tile crack on focal hotspot | §3.2 forces `chiseled` (not cracked) on altar tiles. `worn`/`chiseled` may show crack texture; if it does, those are off-focal tiles. ✓ |
| Pillar in walking path | §6.1 P1..P4 all outside the diagonal walking band (cells 1,1 → 5,3 → 8,4). ✓ |
| Library too small → visible duplication | §6.1 uses `pillar_intact_cyan_crack` twice (P1, P4) on opposite quadrants — deliberate asymmetric reuse. No other prop is duplicated. Two torches are intentional portal-framing (Hades trope). ✓ |
| Missing wall pieces (corner_inner, T, end_cap) | §4 wall plan designed around the missing pieces — they are not needed because of asymmetric/partial perimeter. ✓ |
| face_NS prefab absent | §4.4 inline GameObject build from PNG. ✓ |
| Iso parent scale Y=0.819 | §0 coordinate contract uses Tilemap.CellToWorld so squash is implicit. §3.1 puts the squash on the Tilemap parent transform per Karar #148. ✓ |
| Spike trap reading as armed | §6.6 hard rule: must read as dormant; swap if not. ✓ |
| Treasure pile on path | §6.4 C4 placed at south of cell (6,1), not center — not on diagonal walking band. ✓ |

One **open risk** I flag for Codex:
- **Wall rotation Y=90 on a billboard sprite.** Pilot A face_EW is a billboard
  (single-faced sprite). Rotating it 90° around Y in Unity will likely render
  edge-on. The fallback (use face_NS sprite, see §4.3 and §4.4) is the safe
  path. Codex MUST try once with rotation, immediately check visually
  (read_console + scene screenshot via UnityMCP), and if broken, switch to the
  face_NS PNG approach for all east-wall pieces.

---

## 15. Codex Build Order (priority)

Codex executes in this order. Each step ends with a `read_console` check.

1. **Scene scaffold.**
   - Create scene `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` (copy from
     PathC_BaseTest as a template if convenient, then strip its existing
     content — but DO NOT modify PathC_BaseTest itself).
   - Add Camera (orthographic, size 5, position centered on cell (5,3)),
     Directional Light (or just rely on URP 2D Global Light).
   - Build hierarchy from §2.

2. **Grid + Floor_Tilemap.**
   - Grid: CellLayout=Isometric, CellSize=(1, 0.5, 1).
   - Floor_Tilemap parent transform localScale = (1, 0.819, 1).
   - TilemapRenderer sortingLayer=Ground, mode=IsoZAsY.
   - If iso Tile assets don't exist, create them under
     `Assets/Data/Tiles/Act1_ShatteredKeep/iso_v01/` wrapping the 3 PNGs.

3. **Floor fill.** §3.1 weighted random with seed + §3.2 manual overrides.

4. **Walls (§4).** Place N1..N5 first (north wall is the hero). Then E1..E3.
   Then W1. Pivot/sortingLayer/sortingOrder per Pilot A LOCK.

5. **Primary focal reinforcement.**
   - Add `act1_rift_fracture_overlay_v01.png` on N3 (§11).
   - Add L1 cyan 2D Point Light on N3 (§8).

6. **Secondary focal A — Altar.**
   - Place R1 ritual_stone_altar at (4,6) with offset.
   - Place R2 stone_marker_cyan, R3 tomb_headstone.
   - Place brazier_cyan_rift at (5,7) + brazier_orange at (4,4).
   - Add L4, L5 lights.

7. **Secondary focal B — Statues.**
   - Pick toppled variant S1, place at (2,5).
   - Pick intact variant S2, place at (7,6).
   - Verify visually (toppled vs intact).

8. **Pillars (§6.1).** P1..P4.

9. **Containers + Rubble (§6.4, §6.5).** Place C1..C4 and RB1..RB5.

10. **Floor hazards (§6.6).** FH1..FH3. Verify spike trap reads dormant.

11. **Wall decorations + mountings (§7).** WD1..WD12. For each, instantiate
    mounting prefab first under the wall piece, then decoration sprite under
    the mounting.

12. **Wall sconce / brazier lights (§8).** L2, L3 on torch sconces.

13. **Patches (§9.1).** PA1, PA2, PA3.

14. **Decals (§9.2).** Seeded random, 12 decals distributed per region.

15. **Silhouettes (§10).** SH1 (rat_king behind grate), SH2 (specter in rift),
    SH3 (husk past exit).

16. **Global light.** URP 2D Global Light, color `#1A1A2A`, intensity 0.25.

17. **Final pass.**
    - Save scene.
    - `read_console` for warnings/errors.
    - Take a Game-view screenshot via UnityMCP (camera centered on (5,3),
      orthographic size 5) and save as
      `STAGING/screenshots/IsoShowcaseRoom_S95_first_build.png`.
    - Flag any: floating decoration, broken wall rotation, missing tile,
      silhouette in walking band, light without source.

18. **Self-QC checklist.** Confirm each item from §14 visually in the
    screenshot. If a check fails, fix the offender and re-screenshot.

---

## 16. Out of Scope (do NOT do)

- No new PixelLab generation. The library is locked.
- No new asset variants (no "we should have a banner_red_v02 facing left").
- No gameplay logic (no spawn points beyond the informational PlayerSpawn
  marker, no doors, no triggers).
- No code changes to Pilot A wall prefabs themselves — instantiate as-is.
- No modifications to PathC_BaseTest scene.
- No new shader / VFX / particle systems (sine-wave pulse on rift overlay is
  a cheap script — already inside Pilot A pattern; if absent, Codex writes
  a 20-line `RiftPulse2D` MonoBehaviour, that's the only code allowed).

---

## 17. Done When

- Scene `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` exists, saves clean.
- All wall pieces placed without rotation/sorting artifacts.
- All wall decorations are on a real wall (no floating).
- All lights are anchored to a sprite source.
- Screenshot saved under `STAGING/screenshots/`.
- `read_console` shows 0 errors.
- Spec §14 checklist passes visually.

Then orchestrator can decide: ship as showcase, iterate one more pass, or hand
to user for review.

---

**Spec ends. Codex: execute §15 build order. Do not deviate from coordinates
without flagging — every cell here was placed for a reason listed in §14
saçmalık-pass.**
