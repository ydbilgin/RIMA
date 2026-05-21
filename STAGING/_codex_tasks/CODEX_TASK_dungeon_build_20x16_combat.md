# CODEX TASK — 20×16 Combat Dungeon Build (Yeni 16 Tile Bank)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

`Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` sahnesi şu an **TEMİZ** (boş hierarchy + Grid + PlayerSpawn + Main Camera). Bu sahneye programmatic full dungeon build et.

## Yeni KEEP Asset Bank

### Floor — 16 Iso Diamond Tile (b340684f bundle, re-downloaded)
`Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/` altında:
- `tile_floor_granite_v1.asset` ... `_v4.asset` (4 dark granite cobblestone)
- `tile_floor_cyan_v1.asset` ... `_v4.asset` (4 cracked granite + cyan rift veins)
- `tile_floor_dirt_v1.asset` ... `_v4.asset` (4 packed ancient dirt + stone fragments)
- `tile_floor_ritual_v1.asset` ... `_v4.asset` (4 ritual stone + arcane rune cyan)

### Walls — Pilot A (3 prefab)
- `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_face_EW.prefab`
- `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_corner_outer.prefab`
- `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_arch_opening.prefab`
- face_NS YOK (archived) — west wall = 0 piece, ruin gap

### Props + Decorations — Assets/Art/AssetPacks/Act1_ShatteredKeep/
- statues/ (warrior_intact + warrior_toppled + pedestal_base)
- statue_00..13 prefab (variants) + mounting_00..14 prefab
- pillars/ (broken_granite, chained, intact_cyan_crack)
- ritual/ (altar, marker_cyan, headstone, bench, obelisk)
- props/ (brazier_cyan, brazier_orange, pottery_urn, rubble_debris_small, rubble_heap_skulls, spike_trap_dormant, treasure_pile, wooden_barrel, wooden_crate_stack, lever_wall)
- scatter/ (bone_offering_pile, skull_pile_cluster)
- patches/ (dust_drift_v01 only — moss+cracked archived)
- decals/ (16 var: crack/pebble/dust/bone_chip)
- wall_decoration/ (15: banners 3 renk, torch_sconce, candle_bracket, chain long+short, cage, grate, ivy, lantern, skeleton_shackled, trophy bone/skull/sword)
- arches/ (entry_cyan_rift, exit_cyan_rift)
- rift_accents/ (fracture_overlay)
- decor_silhouettes/ (8: rat_king, goblin, specter_ghost, imp_demon, cyan_slime, cyan_wisp, wraith_specter, husk)

## Spec — 20×16 Combat Dungeon

### Konum
Scene root: `IsoShowcaseRoom_S95_Root` (mevcut)
Hierarchy: Walls_Root, WallDecorations_Root, Props_Root (Pillars/Ritual/Statues/Containers/Rubble/Floor_Hazards), Patches_Root, Decals_Root, Silhouettes_Root, Lights_Root, PlayerSpawn (mevcut, hepsi boş)

### Floor (Floor_Tilemap)
- Grid: cellLayout=Isometric, cellSize (1, 0.5, 1), localScale (1, 0.819, 1) — mevcut OK
- Floor fill cx 0..19, cy 0..15 (20×16 = 320 cell)
- **Weighted random:**
  - granite_v1..v4 → %50 (base granite floor)
  - dirt_v1..v4 → %25 (path/edge variation)
  - cyan_v1..v4 → %15 (rift bias kuzey-orta civarı)
  - ritual_v1..v4 → %10 (focal point + entry threshold cells)
- Seed: `"DungeonCombatV3".GetHashCode()` (reproducible)
- **Manual override hot spots:**
  - Altar twin (cx=9,10 / cy=13): `ritual_v3` veya `v4`
  - North arch threshold (cx=10..11, cy=15): `cyan_v3` (rift glow under arch)
  - South entry threshold (cx=10..11, cy=0..1): `ritual_v1` (entry sacred slab)
  - Toppled statue zone (cx=5, cy=11): `dirt_v3` (impact marks)

### Walls — 14 piece total
- **North (8):** cy=15, cx=0,3,5,7,**11 ARCH**,13,16,19 (corner_outer at cx=0 + 19)
  - cx=0 → corner_outer (NW)
  - cx=3,5,7 → face_EW
  - cx=11 → **arch_opening (PRIMARY FOCAL — rift)**
  - cx=13,16 → face_EW
  - cx=19 → corner_outer (NE)
- **East (4):** cx=19, cy=2,5,8 (face_EW Y=90 → fallback flipX=true if rotation garbles), cy=11 = arch_opening (exit, also flipX or rotation)
- **South (2):** cy=0, cx=4 + cx=15 (face_EW entry flanks, S1 + S2). Entry gap cx=5..14 (10 cell wide).
- **West (0):** ruin gap intentional (Pilot A face_NS archived).

### Focal Hierarchy
- **Primary:** Cyan Rift Arch (north arch + rift_fracture_overlay + L1 cyan Light2D 1.5 intensity, 5.5 radius)
- **Secondary A:** Stone Altar (cx=9, cy=13) — backlit cyan brazier (cx=10, cy=14) + orange brazier (cx=9, cy=11). L4 orange + L5 cyan.
- **Secondary B:** Toppled Statue (cx=5, cy=11) — narrative anchor. No own light, dust patch beneath.
- **Secondary C:** East exit arch (cx=19, cy=11) — L6 lantern overhead.

### Props (Props_Root subgroups)

**Pillars (4):**
- P1 (cx=2, cy=13) — intact_cyan_crack (NW frame)
- P2 (cx=17, cy=13) — chained (NE frame)
- P3 (cx=2, cy=4) — broken_granite (SW ruin)
- P4 (cx=17, cy=4) — intact_cyan_crack (SE frame, mirrors P1 asymmetric)

**Ritual (3):**
- R1 (cx=9.25, cy=13) — stone_altar (straddles 9+10)
- R2 (cx=9, cy=11) — stone_marker_cyan (approach marker)
- R3 (cx=12, cy=12) rotZ=-8° — tomb_headstone (tilted)

**Statues (2):**
- S1 (cx=5, cy=11) +(+0.1,-0.1,0) — pick **toppled** statue variant from `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_XX.prefab` (try 02, 05, 09)
- S2 (cx=15, cy=13) — pick **intact + pedestal** variant (try 00, 03, 11)

**Containers (4):**
- C1 (cx=2, cy=7) — wooden_crate_stack
- C2 (cx=2, cy=6) +(+0.3,-0.1,0) — wooden_barrel (cluster with C1)
- C3 (cx=15, cy=11) — pottery_urn_weathered (near S2)
- C4 (cx=13, cy=2) — treasure_pile (SE exit teaser, off-path)

**Rubble (5):**
- RB1 (cx=8, cy=14) — rubble_debris_small (under arch)
- RB2 (cx=12, cy=15) — rubble_heap_skulls (north wall east of arch)
- RB3 (cx=0, cy=7) — rubble_debris_small (**WEST RUIN GAP explainer**)
- RB4 (cx=7, cy=8) — scatter_bone_offering_pile (mid-room ritual remnant)
- RB5 (cx=16, cy=3) — skull_pile_cluster (SE balance)

**Floor Hazards (1):**
- FH1 (cx=14, cy=7) — spike_trap_dormant (VERIFY dormant, NOT armed)

**Brazier props (2 for lights):**
- B1 (cx=10, cy=14) — brazier_cyan_rift_flame (L5 anchor, just under arch)
- B2 (cx=9, cy=11) — brazier_orange_flame (L4 anchor, altar approach)

### Wall Decorations (WallDecorations_Root) — 12 total
Hard rule: every decoration on a real wall piece, mounting prefab paired for hanging items, max 2 per wall.

| # | Wall | Decoration | Mounting | Offset (X,Y) |
|---|---|---|---|---|
| WD1 | N3 cx=5 | banner_red_torn_v02 | mounting_00 (banner pole) | (0, +0.15) |
| WD2 | N4 cx=7 (west of arch) | wall_torch_sconce_v02 | none | (0, +0.05) |
| WD3 | N6 cx=13 (east of arch) | wall_torch_sconce_v02 | none | (0, +0.05) |
| WD4 | N9 cx=19 (NE corner) | trophy_skull_v01 | mounting_10 (shelf) | (0, +0.10) |
| WD5 | N3 cx=5 | skeleton_shackled_v01 | mounting_06 (wall shackle) | (-0.2, +0.05) — N3 takes WD1+WD5 (max 2 OK) |
| WD6 | E1 cy=2 | chain_hanging_long_v02 | mounting_01 (ceiling hook) | (0, +0.20) |
| WD7 | E_arch cy=11 | lantern_hanging_v01 | mounting_03 (lantern hook) | (0, +0.18) |
| WD8 | E3 cy=8 | cage_iron_hanging_v01 | mounting_02 (ceiling hook) | (0, +0.20) |
| WD9 | E4 cy=5 | wall_candle_bracket_v02 | none | (0, +0.05) |
| WD10 | S1 cx=4 | wall_torch_sconce_v02 | none | (0, +0.05) — south entry torch |
| WD11 | S2 cx=15 | trophy_sword_iron_v01 | mounting_12 (shelf) | (0, +0.10) |
| WD12 | N7 cx=16 | banner_teal_torn_v02 | mounting_04 (banner pole) | (0, +0.15) — far from red (WD1) |

### Lights (Lights_Root) — 6 URP 2D
- L1 N5 arch — `#00FFCC` cyan, inner 0.5, outer 5.5, intensity 1.5, sine 1.5-2.5 Hz amp 0.15
- L2 WD2 torch — `#FF8800` orange, outer 3.0, intensity 1.2, sine 4-6 Hz amp 0.2
- L3 WD3 torch — `#FF8800` orange, outer 3.0, intensity 1.2, sine 4-6 Hz amp 0.2
- L4 B2 brazier orange — `#FFAA44`, outer 2.5, intensity 1.0, sine 5-7 Hz amp 0.25
- L5 B1 brazier cyan — `#00FFCC`, outer 2.0, intensity 0.7, sine 2 Hz amp 0.1
- L6 WD7 lantern — `#FFCC77`, outer 2.0, intensity 0.6, sine 3-4 Hz amp 0.15
- **Global Light2D** `#1A1A2A`, intensity 0.6 (mevcut Lights_Root içine ekle)

### Patches (Patches_Root) — 3 placements, dust_drift only (sortingLayer Ground, order 1)
- PA1 (cx=6, cy=11) alpha 0.7 (NW altar approach)
- PA2 (cx=1, cy=10) alpha 0.6 (west ruin zone)
- PA3 (cx=15, cy=5) alpha 0.5 (SE approach subtle)

### Decals (Decals_Root) — 10 distributed (sortingLayer Ground, order 2)
Seeded `"DungeonCombatV3_Decals".GetHashCode()`. Per region random 0.8-1.1 scale, ±15° rotZ:
- North rift zone (cy 11..15): 2 crack + 1 bone_chip + 1 pebble
- South entry (cy 0..3): 2 pebble + 1 dust
- East exit (cx 16..19, cy 5..10): 1 crack + 1 pebble
- Center walking band (cx 8..12, cy 5..8): 1 dust

### Silhouettes (Silhouettes_Root) — 3
- SH1 rat_king (cx=0, cy=10) sortLayer=Walls/19, alpha 0.5 (behind west grate area)
- SH2 specter_ghost (cx=11, cy=15) +(0,+0.4,0) sortLayer=Walls/18, alpha 0.4 (inside rift arch)
- SH3 husk (cx=19, cy=11) +(+0.3,0,0) sortLayer=Entities/-3, alpha 0.45 (past east exit)

### Rift Accent Overlay
- act1_rift_fracture_overlay_v01.png child of N5 arch
- Offset (0, +0.15, 0)
- sortLayer=Walls, order=21
- Alpha 0.85
- Optional: 20-line RiftPulse2D MonoBehaviour for scale.x oscillation 0.97-1.03 @ 0.8 Hz

## CRITICAL Constraints

- **Main Camera mevcut** — sahne içinde "Main Camera" GameObject var (orthographic, size 7). Yenisini oluşturma.
- **Hierarchy mevcut** — Walls_Root, Props_Root subgroups (Pillars/Ritual/Statues/Containers/Rubble/Floor_Hazards), WallDecorations_Root, Patches_Root, Decals_Root, Silhouettes_Root, Lights_Root, PlayerSpawn boş duruyor. Yenisini OLUŞTURMA, sadece ALTLARINI doldur.
- **Karar #148 squash** — Grid.transform.localScale.y = 0.819, dokunma.
- **CellToWorld kullan** — manuel iso math YAPMA.
- **Antigravity painter fix** — wall pieces YENİ Z-rotation YOK, flipX kullan (east wall y=90 yerine flipX=true).
- **face_NS YOK** — west wall 0 piece. Hiçbir face_NS reference YOK.
- **PathC_BaseTest** dokunma.

## Test (UnityMCP)

1. Build çıkış sonrası `read_console` → 0 error
2. Screenshot: `STAGING/screenshots/DungeonCombatV3_first.png` — camera at (1.5, 4.3, -10), ortho 7
3. Verify: 320 floor tile, 14 wall, 4 pillar, 2 statue, 3 ritual + 2 brazier, 4 container, 5 rubble, 1 hazard, 12 deco + 12 mounting, 6 light + 1 global, 3 patch, 10 decal, 3 silhouette = **~95 GameObject + 320 tile**.

## Rapor

`STAGING/CODEX_DONE_dungeon_combat_v3.md`:
- Step PASS/FAIL listesi
- Decisions: statue picks (S1/S2 prefab variant IDs), mounting picks (WD1..WD12 mounting IDs)
- Saçmalık checklist (her item PASS/FAIL evidence):
  - Floating deco
  - Statue lining
  - Brazier without light
  - Decal on wall
  - Pillar walking band
  - West wall 0 piece (ruin gap)
  - face_NS reference YOK
  - Active spike trap
- Screenshot path
- Console error count

## Effort

high — ~95 GameObject + 320 floor tile + sprite/Light2D component setup, ~30-40 min.
