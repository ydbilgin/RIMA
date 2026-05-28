# Minimum Modular Asset Pack + Sample Dungeon Composition

## 1. Reference quality target analysis

Inspected: all 6 PNGs in `STAGING/CHATGPT_TOPDOWN`, all Phase K screenshots, Act 1 layout JSON, v4 master plan, and the prior v4 Codex review. The task says 3 PNGs, but the folder has 6; I used all 6 as the visual target. Phase K is useful as a room/blockout baseline, but it is visually behind the references: flat top-down floor, little wall height, sparse vertical props, weak decal density, and almost no warm/cold lighting contrast.

**Mockup 1:** Floors: dark large slabs, brighter center slabs, cyan stains. Walls: north/east/west height, south doorway. Props: columns, urn/debris, skull-like scatter, torch. Decals: cracks, dust, cyan marks, rubble. Lighting: one warm torch plus cold floor glow.

**Mockup 2:** Floors: green-gray stone, vertical cyan rift seam. Walls: back wall and side boundaries. Props: four columns, corner torches/braziers, stones. Decals: rift crack, dust, chips, grime. Lighting: symmetric orange corners against cyan center.

**Mockup 3:** Floors: circular ritual stone, dark edge rubble, cyan glyph stains. Walls: tall curved back wall, ornate cyan portal. Props: candles, braziers, banners, urns, altar, statues/stones. Decals: glyphs, soot, moss/dust, cracks. Lighting: many small warm sources plus cyan portal.

**Mockup 4:** Floors: gray-blue cracked stone beside a dark void/cliff. Walls: back/left boundaries and broken edge mass. Props: candles, rubble, banners, collapsed stone, urn/skull piles. Decals: cyan marks, cracks, dust, edge debris. Lighting: side torches plus cyan cracks.

**Mockup 5:** Floors: large dark hall slabs, faint grid breakup, cyan stains. Walls: continuous north wall and shadow side edges. Props: four columns, torches, small debris, possible banners. Decals: cracks, dust, cyan stains, scattered rubble. Lighting: warm wall points plus cool accents.

**Mockup 6:** Floors: dark slabs, central broken rift pit, cyan stains. Walls: clear north/east/west height. Props: braziers, broken columns, banners, skull/debris clusters. Decals: large rift glow, cracks, dust, rubble, glyph marks. Lighting: strong cyan center plus warm corners. VFX overlays and UI are excluded from all counts.

## 2. Minimum modular asset inventory

Target: **24 unique sprites**, not a final Act 1 library. Variants come from rotate/flip, tile randomization, lighting, and placement density.

| # | Asset name | Canvas | BG | Var | Method | Reuse | Why mandatory |
|---|---|---:|---|---:|---|---|---|
| F01 | floor_granite_slab_a | 64x64 | opaque | 1 | create_tiles_pro 4-mix | Base | Main slab floor |
| F02 | floor_granite_slab_b | 64x64 | opaque | 1 | create_tiles_pro 4-mix | Rotate/flip | Breaks repetition |
| F03 | floor_walkway_trim_a | 64x64 | opaque | 1 | create_tiles_pro 4-mix | Paths | Door/corridor read |
| F04 | floor_walkway_trim_b | 64x64 | opaque | 1 | create_tiles_pro 4-mix | Paths | Long corridor variation |
| F05 | floor_cracked_rubble | 64x64 | opaque | 1 | create_tiles_pro 4-mix | Ruin zones | Mockups 1/4/6 |
| F06 | floor_cyan_rift | 64x64 | opaque | 1 | create_tiles_pro 4-mix | Accent | Cyan identity |
| W01 | wall_straight_n | 64x96 | transparent | 1 | create_object 16-candidate | Repeat north | Wall height |
| W02 | wall_straight_e | 64x96 | transparent | 1 | create_object 16-candidate | FlipX west | Side walls |
| W03 | wall_corner_outer | 64x96 | transparent | 1 | create_object 16-candidate | Rotate | Corners |
| W04 | wall_corner_inner | 64x96 | transparent | 1 | create_object 16-candidate | Rotate | Door breaks |
| W05 | wall_collapsed_stub | 64x96 | transparent | 1 | create_object 16-candidate | Blocker | Ruined edge |
| P01 | prop_round_column | 64x96 | transparent | 1 | create_object 16-candidate | Repeat | Mockups 2/5 |
| P02 | prop_broken_column | 64x80 | transparent | 1 | create_object 16-candidate | Flip | Ruined cover |
| P03 | prop_tattered_banner | 48x80 | transparent | 1 | create_object 16-candidate | Wall dress | Back wall identity |
| P04 | prop_wall_torch | 48x64 | transparent | 1 | create_object 16-candidate | Light source | Warm anchor |
| P05 | prop_floor_brazier | 64x64 | transparent | 1 | create_object 16-candidate | Light/block | Boss drama |
| P06 | prop_urn_cluster | 48x48 | transparent | 1 | create_object 16-candidate | Scatter | Wall clutter |
| P07 | prop_rubble_pile | 64x48 | transparent | 1 | create_object 16-candidate | Edge fill | Ruin feel |
| D01 | decal_moss_patch | 32x32 | transparent | 1 | create_object 64-candidate | Scatter | L13 nature layer |
| D02 | decal_crack | 32x32 | transparent | 1 | create_object 64-candidate | Scatter | Floor breakup |
| D03 | decal_blood | 32x32 | transparent | 1 | create_object 64-candidate | Combat rooms | History |
| D04 | decal_dust | 32x32 | transparent | 1 | create_object 64-candidate | Scatter | Low-cost fill |
| D05 | decal_glyph | 32x32 | transparent | 1 | create_object 64-candidate | Lore/rift | Mockups 3/6 |
| H01 | hero_archway_entry | 64x128 | transparent | 1 | create_object 4-candidate | Door/focal | Only hero kept |

Defer post-slice: throne backdrop, sarcophagus, chest tiers, skull pile, wall sconce, shrine pedestal, large ritual circle, rift crystal, gate halves. They add spectacle, not the minimum visual grammar.

## 3. Production budget

| Batch | Contents | Gen | Manual labor |
|---|---|---:|---:|
| Floor 4-mix | F01-F06 selected from 16 | 25 | 5 min |
| Wall batch | W01-W05 selected from 16 | 40 | 10-15 min |
| Medium prop batch | P01-P07 selected from 16 | 30 | 10-15 min |
| Small/decal batch | D01-D05 selected from 64 | 20-30 | 10 min |
| Hero arch batch | H01 selected from 4 | 40 | 10-15 min |

Total: **155-165 generations** and about **45-60 minutes** of selection/import cleanup. No Edit Image Pro variants are required. Against 2265/5000 remaining, this leaves roughly **2100 generations spare**, satisfying the >=1000 spare requirement for Acts 2-4, boss, retries, and polish.

## 4. Sample dungeon composition - 3 rooms

All rooms use only the 24 assets above.

```json
[
{"schema_version":"1.0","room_id":"minpack_combat_16x12","display_name":"Combat - Broken Slab Hall","act":"act1_shattered_keep","room_type":"combat_medium","width":16,"height":12,"shape":{"type":"rectangle"},"floor":{"default_material":"granite","variant_pool":["F01","F02"],"zones":[{"material":"rubble","asset":"F05","rect":{"x":1,"y":1,"width":4,"height":3},"blend_edges":true},{"material":"walkway","asset":"F03","rect":{"x":6,"y":0,"width":4,"height":12},"blend_edges":true}],"accents":[{"material":"rift","asset":"F06","x":8,"y":6}]},"walls":[{"prefab":"W01","x":2,"y":11},{"prefab":"W01","x":7,"y":11},{"prefab":"W01","x":12,"y":11},{"prefab":"W02","x":15,"y":6},{"prefab":"W02","x":0,"y":6,"flip_x":true},{"prefab":"W03","x":0,"y":11},{"prefab":"W03","x":15,"y":11,"rotation":90},{"prefab":"W05","x":13,"y":3}],"props":[{"prefab":"P01","x":4,"y":4},{"prefab":"P02","x":11,"y":7,"flip_x":true},{"prefab":"P03","x":7,"y":10},{"prefab":"P04","x":2,"y":10},{"prefab":"P04","x":14,"y":10,"flip_x":true},{"prefab":"P06","x":2.5,"y":2},{"prefab":"P07","x":12,"y":2},{"prefab":"D01","x":5,"y":2},{"prefab":"D02","x":9,"y":4},{"prefab":"D03","x":7,"y":7},{"prefab":"D04","x":3,"y":8}],"doors":[{"direction":"south","x":8,"y":0,"target_room_id":"minpack_corridor_8x24"},{"direction":"north","x":8,"y":11,"target_room_id":"minpack_boss_24x18","locked_initial":true}],"mob_spawns":[{"mob_id":"rift_husk","x":5,"y":6},{"mob_id":"shattered_knight","x":11,"y":6,"wave":2}],"lighting":{"global_color":"#20242A","global_intensity":0.65,"point_lights":[{"x":2,"y":10,"color":"#FFA060","intensity":1.1,"outer_radius":4,"flicker":true},{"x":14,"y":10,"color":"#FFA060","intensity":1.1,"outer_radius":4,"flicker":true},{"x":8,"y":6,"color":"#5DEFFF","intensity":0.8,"outer_radius":4}]}},
{"schema_version":"1.0","room_id":"minpack_corridor_8x24","display_name":"Corridor - Hairline Rift","act":"act1_shattered_keep","room_type":"corridor_linear","width":8,"height":24,"shape":{"type":"rectangle"},"floor":{"default_material":"walkway","variant_pool":["F03","F04"],"zones":[{"material":"granite","asset":"F01","rect":{"x":1,"y":0,"width":6,"height":24},"blend_edges":true}],"accents":[{"material":"rift","asset":"F06","x":4,"y":6},{"material":"rift","asset":"F06","x":4,"y":17}]},"walls":[{"prefab":"W02","x":0,"y":5,"flip_x":true},{"prefab":"W02","x":7,"y":5},{"prefab":"W02","x":0,"y":15,"flip_x":true},{"prefab":"W02","x":7,"y":15},{"prefab":"W04","x":0,"y":23},{"prefab":"W04","x":7,"y":23,"rotation":90},{"prefab":"H01","x":4,"y":23}],"props":[{"prefab":"P04","x":1,"y":5},{"prefab":"P04","x":6,"y":15,"flip_x":true},{"prefab":"P06","x":2,"y":10},{"prefab":"P07","x":5,"y":19},{"prefab":"D02","x":4,"y":8},{"prefab":"D04","x":3,"y":13},{"prefab":"D05","x":4,"y":17}],"doors":[{"direction":"south","x":4,"y":0,"target_room_id":"minpack_combat_16x12"},{"direction":"north","x":4,"y":23,"target_room_id":"minpack_boss_24x18"}],"lighting":{"global_color":"#191D23","global_intensity":0.5,"point_lights":[{"x":1,"y":5,"color":"#FFA060","intensity":0.9,"outer_radius":3,"flicker":true},{"x":6,"y":15,"color":"#FFA060","intensity":0.9,"outer_radius":3,"flicker":true},{"x":4,"y":17,"color":"#5DEFFF","intensity":0.7,"outer_radius":3}]}},
{"schema_version":"1.0","room_id":"minpack_boss_24x18","display_name":"Boss Arena - Rift Court","act":"act1_shattered_keep","room_type":"boss_arena","width":24,"height":18,"shape":{"type":"rectangle"},"floor":{"default_material":"granite","variant_pool":["F01","F02"],"zones":[{"material":"rubble","asset":"F05","rect":{"x":2,"y":2,"width":5,"height":4},"blend_edges":true},{"material":"walkway","asset":"F04","rect":{"x":9,"y":0,"width":6,"height":18},"blend_edges":true},{"material":"rift","asset":"F06","rect":{"x":10,"y":7,"width":4,"height":4},"blend_edges":true}],"accents":[{"material":"rift","asset":"F06","x":6,"y":9},{"material":"rift","asset":"F06","x":18,"y":9}]},"walls":[{"prefab":"W01","x":4,"y":17},{"prefab":"W01","x":10,"y":17},{"prefab":"W01","x":16,"y":17},{"prefab":"W03","x":0,"y":17},{"prefab":"W03","x":23,"y":17,"rotation":90},{"prefab":"W05","x":3,"y":5},{"prefab":"W05","x":20,"y":5,"flip_x":true},{"prefab":"H01","x":12,"y":17}],"props":[{"prefab":"P01","x":5,"y":5},{"prefab":"P01","x":19,"y":5},{"prefab":"P02","x":7,"y":13},{"prefab":"P02","x":17,"y":13,"flip_x":true},{"prefab":"P03","x":8,"y":16},{"prefab":"P03","x":16,"y":16,"flip_x":true},{"prefab":"P05","x":4,"y":15},{"prefab":"P05","x":20,"y":15},{"prefab":"P06","x":2,"y":3},{"prefab":"P07","x":21,"y":3},{"prefab":"D01","x":6,"y":2},{"prefab":"D02","x":12,"y":5},{"prefab":"D03","x":14,"y":11},{"prefab":"D04","x":18,"y":2},{"prefab":"D05","x":12,"y":9}],"doors":[{"direction":"south","x":12,"y":0,"target_room_id":"minpack_corridor_8x24","locked_initial":true}],"mob_spawns":[{"mob_id":"act1_boss_shattered_king","x":12,"y":14,"elite":true}],"lighting":{"global_color":"#15191F","global_intensity":0.45,"point_lights":[{"x":12,"y":9,"color":"#5DEFFF","intensity":2.0,"outer_radius":8},{"x":4,"y":15,"color":"#FFA060","intensity":1.2,"outer_radius":4,"flicker":true},{"x":20,"y":15,"color":"#FFA060","intensity":1.2,"outer_radius":4,"flicker":true}]}}
]
```

Room A reads as balanced combat through mixed walkway/rubble, moderate blockers, and blood/crack scatter. Room B shifts to sparse, narrow, torch-led navigation. Room C uses the same pool for drama by concentrating arch, braziers, columns, banners, and cyan rift light.

## 5. Modular reuse verification matrix

| Asset | A | B | C | Uses |
|---|---:|---:|---:|---:|
| F01 | 1 | 1 | 1 | 3 |
| F02 | 1 | 0 | 1 | 2 |
| F03 | 1 | 1 | 0 | 2 |
| F04 | 0 | 1 | 1 | 2 |
| F05 | 1 | 0 | 1 | 2 |
| F06 | 1 | 1 | 1 | 3 |
| W01 | 3 | 0 | 3 | 6 |
| W02 | 2 | 4 | 0 | 6 |
| W03 | 2 | 0 | 2 | 4 |
| W04 | 0 | 2 | 0 | 2 |
| W05 | 1 | 0 | 2 | 3 |
| P01 | 1 | 0 | 2 | 3 |
| P02 | 1 | 0 | 2 | 3 |
| P03 | 1 | 0 | 2 | 3 |
| P04 | 2 | 2 | 0 | 4 |
| P05 | 0 | 0 | 2 | 2 |
| P06 | 1 | 1 | 1 | 3 |
| P07 | 1 | 1 | 1 | 3 |
| D01 | 1 | 0 | 1 | 2 |
| D02 | 1 | 1 | 1 | 3 |
| D03 | 1 | 0 | 1 | 2 |
| D04 | 1 | 1 | 1 | 3 |
| D05 | 0 | 1 | 1 | 2 |
| H01 | 0 | 1 | 1 | 2 |

## 6. Comparison vs v4 RULE 1-13

| v4 RULE | v4 cost | This proposal | Delta |
|---|---:|---:|---:|
| RULE 1 Floor base | 25 gen | 25 gen | 0 |
| RULE 4 Wall full library | 125-200 gen | 40 gen | saves 85-160 |
| RULE 5 Prop 32x32 | 30 gen | 20-30 gen | saves 0-10 |
| RULE 6 Prop 48-80 | 30-60 gen | 30 gen | saves 0-30 |
| RULE 7 Prop 88-168 | 120-200 gen | 40 gen | saves 80-160 |
| RULE 8 Decal layer | 75-90 gen | 20-30 gen | saves 45-70 |
| **Total Act 1 base** | **~580 gen v4** | **155-165 gen** | **saves ~415-425** |

Replaced: RULE 4 full wall library becomes 5 wall sprites; RULE 5 and RULE 8 merge into one small/decal batch. Simplified: RULE 6 keeps only high-reuse props; RULE 7 keeps one arch. Kept: RULE 1. Deferred: RULE 2 transition batches, parca-parca hero scope, Act 2-4 variants, boss, UI, and full L13 density.

## 7. Risks + trade-offs

Sacrifices: less room variety, weaker boss spectacle without throne/sarcophagus/large ritual circle, and L13 reduced from a full 6-layer floor to a practical 4-5 layer read. Act 2-4 reuse will need later biome variants because this pack is very Act 1 stone/keep flavored.

Gains: much lower generation cost, under one hour of manual cleanup, faster vertical-slice proof, and much more spare budget for boss, VFX, Act 2-4 shifts, and retries. Net: worth it for a vertical slice, not sufficient as the final Act 1 library.

## 8. Recommendation - GO

GO. Produce in this order: floor 4-mix, wall candidate batch, medium props, small/decal batch, hero arch. Test gate: compose these three rooms and approve only if wall height reads, floor repetition is tolerable at 100%, and warm/cold lighting makes the rooms distinct.

Final verdict: This 24-sprite pack preserves the reference essentials: dark slabs, readable walls, vertical props, decals, and dual-tone lighting. It cuts hero spectacle and full L13 density to save roughly 415 generations against v4 Act 1 base. Recommendation is GO for a vertical-slice asset pack, then revise only after seeing the composed three-room test.
