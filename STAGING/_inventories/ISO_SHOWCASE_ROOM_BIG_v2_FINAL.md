# Iso Showcase Room — Big Room Spec v2 FINAL (S95)

> **Status:** FINAL — user approved archive list (6 assets removed). Ready for Codex UnityMCP build.
> **Author:** rima-sonnet v2 adapted from Opus v1 (ISO_SHOWCASE_ROOM_COMPOSITION_S95.md)
> **Changes from v1:** 24×18 cell room, 17 wall pieces (west wall 0, ruin gap), 1 floor hazard (spike_trap only), 1 patch type (dust_drift) 3× placement, WD reassignments from west wall to N/E/S.

---

## 0. Coordinate Contract (READ FIRST)

All cell coordinates are **Tilemap cell-coords** `(cx, cy)`. Codex MUST convert to
world via `Floor_Tilemap.CellToWorld(new Vector3Int(cx, cy, 0))` then add listed offsets.
Karar #148 Y-squash (`parent.localScale.y = 0.819`) is honored automatically — squash
lives on the Tilemap parent transform, not on individual objects.

- Floor grid origin: cell `(0, 0)` = south corner of the room.
- Floor cells used: `cx ∈ [0..23]`, `cy ∈ [0..17]` → **24 × 18 cell room**.
- "World units" = post-`CellToWorld` + offset values = final Unity transform values.
- Z values: leave 0 for sprites; sortingLayer + IsoZAsY handles depth.

If Codex finds `Floor_Tilemap` parent `localScale.y ≠ 0.819` → STOP and surface.

---

## 1. Concept

**"Forgotten Rift Antechamber."** A combat-sized antechamber inside the Shattered Keep
where a cyan rift has split the back wall and is leaking into the room. A toppled warrior
statue lies near the rift, a stone altar sits off-axis as the ritual focal point, and a
torch-lit entry arch faces the player. The player walks in from the south, eye is pulled
diagonally up-left to the rift archway (cyan focal), then sweeps right across the altar
(warm orange focal) and exits through the east arch.

This room is a **Combat sub-room** big enough to fight in with enough props to read
"this place has a history" without crowding play space. Density target: **22-25% prop
coverage**, **~25% negative space** (central diagonal walking band stays clear).

West wall is **intentionally absent** — the keep is shattered. No wall pieces on the
west side. Debris at the west ruin opening tells the story without needing new art.

---

## 2. Room Dimensions & Hierarchy

- **Cell grid:** W × H = **24 × 18** (cx 0..23, cy 0..17)
- **Origin gameobject:** `IsoShowcaseRoom_S95_Root` at world (0,0,0).

### Scene hierarchy

```
IsoShowcaseRoom_S95_Root
├── Grid                 (Grid, CellLayout=Isometric, CellSize 1/0.5/1)
│   └── Floor_Tilemap    (Tilemap; parent localScale=(1,0.819,1); Ground sort, IsoZAsY)
├── Walls_Root
├── WallDecorations_Root
├── Props_Root
│   ├── Pillars
│   ├── Ritual
│   ├── Statues
│   ├── Containers
│   ├── Rubble
│   └── Floor_Hazards
├── Patches_Root
├── Decals_Root
├── Silhouettes_Root
├── Lights_Root
└── PlayerSpawn
```

Walls: sortingLayer=Walls, sortingOrder=20. Floor: Ground. Patches+decals: Ground/order=1+2.
Entities prefabs already have sortingLayer=Entities.

---

## 3. Floor Layout

### 3.1 Base fill — weighted

| Variant | Weight | Asset |
|---|---|---|
| `act1_iso_granite_clean` | 60% | `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/act1_iso_granite_clean.png` |
| `act1_iso_granite_worn` | 30% | `.../act1_iso_granite_worn.png` |
| `act1_iso_granite_chiseled` | 10% | `.../act1_iso_granite_chiseled.png` |

Seed: `"IsoShowcaseRoom_S95".GetHashCode()`. If PNG → Tile SOs absent, create under `Assets/Data/Tiles/Act1_ShatteredKeep/iso_v01/`.

### 3.2 Manual overrides

| Cell | Tile | Why |
|---|---|---|
| (10,15) | `chiseled` | Altar west slab |
| (11,15) | `chiseled` | Altar east slab |
| (5,12) | `worn` | Toppled statue impact |
| (7,10) | `worn` | Path drag from statue |
| (2,2) | `chiseled` | South entry threshold |
| (20,9) | `chiseled` | East exit threshold |
| (12,5) | `clean` | Walking band |
| (14,7) | `clean` | Walking band |

---

## 4. Wall Layout — 17 pieces total

### 4.1 Inventory (KEEP — 3 prefabs)

- `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_face_EW.prefab`
- `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_corner_outer.prefab`
- `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_arch_opening.prefab`

**face_NS PNG ARCHIVED** — west wall = 0 pieces. Do NOT create face_NS GameObject.

### 4.2 North wall — 9 pieces (cy=17)

| # | Anchor | Offset | Prefab | Note |
|---|---|---|---|---|
| N1 | (0,17) | (0,+0.4,0) | face_EW | Far-west |
| N2 | (3,17) | (0,+0.4,0) | face_EW | |
| N3 | (6,17) | (0,+0.4,0) | face_EW | |
| N4 | (9,17) | (0,+0.4,0) | face_EW | |
| N5 | (11,17) | (0,+0.5,0) | arch_opening | **PRIMARY FOCAL — cyan rift arch** |
| N6 | (14,17) | (0,+0.4,0) | face_EW | |
| N7 | (17,17) | (0,+0.4,0) | face_EW | |
| N8 | (20,17) | (0,+0.4,0) | face_EW | |
| N9 | (23,17) | (0,+0.4,0) | corner_outer | NE corner |

### 4.3 East wall — 6 pieces (cx=23)

| # | Anchor | Offset | Prefab | Note |
|---|---|---|---|---|
| E1 | (23,15) | (+0.4,0,0) | face_EW rotY=90 | |
| E2 | (23,13) | (+0.4,0,0) | face_EW rotY=90 | |
| E3 | (23,11) | (+0.4,0,0) | arch_opening rotY=90 | **Exit arch — secondary** |
| E4 | (23,9) | (+0.4,0,0) | face_EW rotY=90 | |
| E5 | (23,6) | (+0.4,0,0) | face_EW rotY=90 | |
| E6 | (23,3) | (+0.4,0,0) | face_EW rotY=90 | |

### 4.4 South wall — 2 pieces

| # | Anchor | Offset | Prefab | Note |
|---|---|---|---|---|
| S1 | (3,0) | (0,+0.3,0) | face_EW | West entry flank |
| S2 | (20,0) | (0,+0.3,0) | face_EW | East entry flank |

### 4.5 West wall — 0 pieces (ruin gap intentional)

cx=0 column wall deliberately absent. RB3 debris at (0,9) explains absence.

### 4.6 East rotation fallback

face_EW billboard with Y=90 may render edge-on. Codex: try rotation first via UnityMCP; if garbled, fallback to `SpriteRenderer.flipX = true` without Y rotation. Applies to all E1..E6.

---

## 5. Focal Hierarchy

- **Primary:** Cyan Rift Archway (N5 + rift_fracture_overlay + L1).
- **Secondary A:** Stone Altar (10-11, 15) — brazier_cyan north + brazier_orange south.
- **Secondary B:** Toppled Statue (5,12) — narrative.
- **Detail:** pillars, walls decor, rubble, decals.

---

## 6. Props & Statues (Props_Root)

### 6.1 Pillars

| # | Cell | Asset | Why |
|---|---|---|---|
| P1 | (2,15) | `act1_pillar_intact_cyan_crack_v01.png` | NW, cyan echoes rift |
| P2 | (21,15) | `act1_pillar_chained_v01.png` | NE — "chained something" |
| P3 | (4,4) | `act1_pillar_broken_granite_v01.png` | SW ruin |
| P4 | (20,4) | `act1_pillar_intact_cyan_crack_v01.png` | SE — mirrors P1 asymmetric |

### 6.2 Ritual

| # | Cell | Offset | Asset | Why |
|---|---|---|---|---|
| R1 | (10,15) | (+0.25,0,0) | `act1_ritual_stone_altar_v01.png` | Secondary A — straddles (10,15)+(11,15) |
| R2 | (10,13) | (0,0,0) | `act1_ritual_stone_marker_cyan_v01.png` | Altar approach marker |
| R3 | (13,14) | (0,0,0) rotZ=-8° | `act1_ritual_tomb_headstone_v01.png` | Tilted — ritual disturbed |

Brazier props:
- `act1_prop_brazier_orange_flame_v01.png` @ (10,10) — L4 anchor
- `act1_prop_brazier_cyan_rift_flame_v01.png` @ (12,17) — L5 anchor

### 6.3 Statues

| # | Cell | Offset | Prefab | Why |
|---|---|---|---|---|
| S1 | (5,12) | (+0.1,-0.1,0) | statue_XX **toppled** (try 02, 05, 09) | Secondary B |
| S2 | (18,15) | (0,0,0) | statue_XX **intact + pedestal** (try 00, 03, 11) | Honor guard NE |

Codex MUST inspect sprite preview to confirm toppled vs intact.

### 6.4 Containers

| # | Cell | Offset | Asset | Why |
|---|---|---|---|---|
| C1 | (2,7) | (0,0,0) | `act1_prop_wooden_crate_stack_v01.png` | SW cluster |
| C2 | (2,6) | (+0.3,-0.1,0) | `act1_prop_wooden_barrel_v01.png` | Cluster with C1 |
| C3 | (17,13) | (0,0,0) | `act1_prop_pottery_urn_weathered_v01.png` | Near S2 |
| C4 | (15,2) | (0,0,0) | `act1_prop_treasure_pile_v01.png` | SE reward tease |
| C5 | (3,3) | (0,0,0) | `act1_prop_pottery_urn_weathered_v01.png` | NEW — replaces FH4 pressure_plate |

### 6.5 Rubble & Scatter

| # | Cell | Offset | Asset | Why |
|---|---|---|---|---|
| RB1 | (8,16) | (-0.2,+0.1,0) | `act1_prop_rubble_debris_small_v01.png` | Under rift arch |
| RB2 | (13,17) | (+0.1,+0.1,0) | `act1_prop_rubble_heap_skulls_v01.png` | North wall east of arch |
| RB3 | (0,9) | (+0.2,0,0) | `act1_prop_rubble_debris_small_v01.png` | **West ruin gap explainer** |
| RB4 | (7,8) | (0,0,0) | `act1_scatter_bone_offering_pile_v01.png` | Mid-room ritual remnant |
| RB5 | (18,3) | (0,0,0) | `act1_skull_pile_cluster_v01.png` | SE balance |
| RB6 | (9,14) | (0,0,0) | `act1_scatter_bone_offering_pile_v01.png` | NW altar zone debris |
| RB7 | (6,14) | (0,0,0) | `act1_prop_rubble_debris_small_v01.png` | NW debris |
| RB8 | (7,7) | (0,0,0) | `act1_scatter_bone_offering_pile_v01.png` | NEW — replaces FH1 iron_grate cell |

### 6.6 Floor Hazards — 1 only

| # | Cell | Asset | Why |
|---|---|---|---|
| FH3 | (16,7) | `act1_prop_spike_trap_dormant_v01.png` | Mid-east; MUST read dormant |

**Removed:** FH1 iron_grate (archived), FH2 + FH4 pressure_plate (archived). Cell (11,7) intentionally empty (walking band).

---

## 7. Wall Decorations — 19 total (WallDecorations_Root)

Every decoration on real wall piece (N1..N9 except N5, N8; E1..E6; S1; not S2). Max 2 per wall. Every hanging element paired with mounting prefab from `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_XX_*.prefab`.

| # | Wall | Decoration | Mounting | Offset | Why |
|---|---|---|---|---|---|
| WD1 | N1 | `act1_banner_red_torn_v02.png` | banner pole (try 00,04,08) | (0,+0.15) | Red banner west anchor |
| WD2 | N2 | `act1_wall_torch_sconce_v02.png` | none | (0,+0.05) | Light L2 west of arch |
| WD3 | N4 | `act1_wall_torch_sconce_v02.png` | none | (0,+0.05) | Light L3 east of arch — portal framing |
| WD4 | N9 | `act1_trophy_skull_v01.png` | shelf/peg | (0,+0.10) | NE corner trophy |
| WD5 | E1 | `act1_chain_hanging_long_v02.png` | ceiling hook | (0,+0.20) | Long chain depth |
| WD6 | E1 | `act1_cage_iron_hanging_v01.png` | ceiling hook | (-0.15,+0.20) | Iron cage pair on E1 |
| WD7 | E3 | `act1_lantern_hanging_v01.png` | lantern hook | (0,+0.18) | L7 — exit arch lantern |
| WD8 | E6 | `act1_wall_candle_bracket_v02.png` | none | (0,+0.05) | L8 — SE candle |
| WD9 | E6 | `act1_ivy_vine_v01.png` | none | (+0.10,+0.10) | Organic asymmetry |
| WD10 | N3 | `act1_chain_hook_short_v02.png` | none | (-0.15,+0.10) | Texture between west face & arch |
| WD11 | N6 | `act1_grate_iron_v01.png` | none | (+0.10,-0.10) | SH1 silhouette behind |
| WD12 | N7 | `act1_trophy_bone_v01.png` | shelf/peg | (0,+0.10) | Bone trophy east |
| WD13 | E2 | `act1_chain_hook_short_v02.png` | none | (0,+0.12) | East texture |
| WD14 | E5 | `act1_wall_candle_bracket_v02.png` | none | (0,+0.05) | Mid-east candle |
| WD15 | S1 | `act1_wall_torch_sconce_v02.png` | none | (0,+0.05) | L6 — south entry torch |
| WD16 | N1 | `act1_skeleton_shackled_v01.png` | wall shackle | (0,+0.05) | **Moved from W1** (archived) — N1 carries WD1 + WD16 (max 2 OK) |
| WD17 | E5 | `act1_banner_teal_torn_v02.png` | banner pole | (+0.10,+0.15) | **Moved from W2** — E5 carries WD14 + WD17 (max 2 OK) |
| WD18 | S1 | `act1_trophy_sword_iron_v01.png` | shelf/peg | (+0.10,+0.10) | **Moved from W3** — S1 carries WD15 + WD18 (max 2 OK) |
| WD19 | E4 | `act1_ivy_vine_v01.png` | none | (+0.10,+0.10) | Additional east ivy |

---

## 8. Lighting — 8 lights (Lights_Root)

URP 2D Point Lights, every light anchored to in-world sprite source.

| # | Anchor | Pos | Color | Inner | Outer | Intensity | Flicker |
|---|---|---|---|---|---|---|---|
| L1 | N5 rift arch | (11,17)+(+0.25,+0.2) | `#00FFCC` | 0.5 | **6.5** | 1.0 | sine 1.5-2.5 Hz amp 0.15 |
| L2 | WD2 torch | wall-local N2 | `#FF8800` | 0.3 | 2.5 | 0.8 | sine 4-6 Hz amp 0.2 |
| L3 | WD3 torch | wall-local N4 | `#FF8800` | 0.3 | 2.5 | 0.8 | sine 4-6 Hz amp 0.2 |
| L4 | brazier_orange | (10,10) | `#FFAA44` | 0.4 | 2.0 | 0.6 | sine 5-7 Hz amp 0.25 |
| L5 | brazier_cyan_rift | (12,17)+(0,-0.2) | `#00FFCC` | 0.3 | 1.8 | 0.5 | sine 2 Hz amp 0.1 |
| L6 | WD15 torch | wall-local S1 | `#FF8800` | 0.3 | 2.0 | 0.6 | sine 4-6 Hz amp 0.2 |
| L7 | WD7 lantern | wall-local E3 | `#FFCC77` | 0.2 | 1.8 | 0.5 | sine 3-4 Hz amp 0.15 |
| L8 | WD8 candle | wall-local E6 | `#FFCC77` | 0.15 | 1.2 | 0.4 | sine 5 Hz amp 0.1 |

**Global light:** URP 2D Global Light, `#1A1A2A`, intensity **0.6** (upgraded from 0.25 — brightness lesson).

---

## 9. Floor Patches — 1 type, 3 placements (Patches_Root)

Only `act1_patch_dust_drift_v01.png` (other 2 patches archived).

| # | Anchor | Alpha | Why |
|---|---|---|---|
| PA1 | (7,12) center | **0.7** | NW altar approach |
| PA2 | (1,13) +(+0.3,0) | **0.6** | West ruin zone (subtle) |
| PA3 | (17,6) center | **0.5** | SE approach (lightest) |

sortingLayer=Ground, sortingOrder=1, scale 1.2-1.5. Don't cover hot spots (altar, thresholds).

---

## 10. Decals — 12 distributed (Decals_Root)

| Region | Decals | Cell area |
|---|---|---|
| North rift | 2 crack + 1 bone_chip + 1 pebble | cy 12..17, any cx |
| South entry | 2 pebble + 1 dust | cy 0..4, cx 0..10 |
| East exit | 1 crack + 1 dust + 1 pebble | cy 7..12, cx 18..23 |
| Center band | 1 dust | (5,5)–(14,9) |

Seeded random: `"IsoShowcaseRoom_S95_Decals".GetHashCode()`. Offset rand(-0.3,+0.3 / -0.2,+0.2). RotationZ ±15°. Scale 0.8-1.1. sortingOrder=2.

---

## 11. Silhouettes — 3 (Silhouettes_Root)

| # | Asset | Cell + offset | Sorting | Alpha | Why |
|---|---|---|---|---|---|
| SH1 | `decor_silhouette_rat_king_v01.png` | (0,12)+(-0.2,-0.1,0) | Walls/19 | 0.5 | Behind WD11 grate west side |
| SH2 | `decor_silhouette_specter_ghost_v01.png` | (12,17)+(0,+0.4,0) | Walls/18 | 0.4 | Inside rift arch — reinforces primary |
| SH3 | `decor_silhouette_husk_v01.png` | (23,12)+(+0.3,0,0) | Entities/-3 | 0.45 | Past east arch |

---

## 12. Rift Accent Overlay

`act1_rift_fracture_overlay_v01.png` child of N5:
- Local offset (0,+0.15,0)
- sortingLayer=Walls, sortingOrder=21
- Alpha 0.85, scale 1.0
- Pulse: scale.x 0.97-1.03 @ 0.8 Hz via `RiftPulse2D` MonoBehaviour (≤20 lines if absent)

---

## 13. Saçmalık Pass

| Concern | Resolution |
|---|---|
| Floating decoration | Every deco on real wall, mounting paired ✓ |
| 3+ statues in row | Only 2, opposite halves ✓ |
| Brazier without light | All 8 lights anchored ✓ |
| Decal on wall | §10 floor-only rule ✓ |
| Mounting without decor | Every mounting paired ✓ |
| Pillar in walking band | P1..P4 outside (2,2)→(12,8)→(21,11) diagonal ✓ |
| Library duplication | pillar_intact_cyan_crack ×2 asymmetric; 2 torches portal-framing ✓ |
| face_NS absent | West wall 0 pieces, RB3 debris explains ruin ✓ |
| Iso squash | §0 contract via CellToWorld ✓ |
| Spike trap armed | §6.6 must read dormant; STOP if armed ✓ |
| Treasure on path | C4 south side of cell, off walking band ✓ |
| **West wall 0 piece** | **Ruin gap intentional, debris RB3 ✓** |
| **Floor patch variety** | **1 type 3 placements, alpha 0.7/0.6/0.5 variation ✓** |
| **Floor hazard reduced** | **FH3 only; cells reassigned to RB8 / C5 / empty ✓** |

---

## 14. Codex Build Order

1. Scene scaffold (new scene, camera ortho size=7, pos centered on cell (12,9)). Build hierarchy §2.
2. Grid + Floor_Tilemap (Isometric, CellSize 1/0.5/1, parent localScale 1/0.819/1, Ground/IsoZAsY).
3. Tile SOs (§3.1 if missing).
4. Floor fill §3.1 + §3.2 overrides.
5. Walls: 17 pieces total — N1..N9 + E1..E6 + S1..S2. West nothing.
6. Primary focal — rift overlay on N5 + L1.
7. Secondary A — altar R1..R3 + braziers + L4 + L5.
8. Statues S1 (toppled) + S2 (intact). Visual verify.
9. Pillars P1..P4.
10. Containers C1..C5 + Rubble RB1..RB8.
11. Floor hazard FH3 (verify dormant).
12. Wall decorations WD1..WD19 + mountings.
13. Remaining lights L2,L3,L6,L7,L8.
14. Patches PA1..PA3 (alpha 0.7/0.6/0.5).
15. Decals — 12 seeded random per region.
16. Silhouettes SH1..SH3.
17. Global Light `#1A1A2A` intensity 0.6.
18. Save scene, `read_console` 0 errors, screenshot to `STAGING/screenshots/IsoShowcaseRoom_S95_v2.png` (camera at cell (12,9), ortho size 7).
19. Self-QC §13 checklist passes visually.

---

## 15. Out of Scope

- No new PixelLab generation.
- No new assets or variants.
- No gameplay logic.
- No modifications to PathC_BaseTest or other scenes.
- No new shader/VFX. Only `RiftPulse2D` MonoBehaviour (≤20 lines) if absent.

---

## 16. Done When

- Scene saves clean, 0 errors.
- 17 walls placed, no edge-on artifacts.
- All decorations on real walls.
- All 8 lights anchored.
- Screenshot saved.
- §13 saçmalık checklist passes visually.

**Codex: execute §14 build order. Do not deviate without flagging.**
