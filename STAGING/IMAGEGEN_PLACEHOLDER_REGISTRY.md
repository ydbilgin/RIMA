# IMAGEGEN PLACEHOLDER REGISTRY — "produced by imagegen, RE-PRODUCE in PixelLab later"

**Purpose:** every asset generated via Codex/cx imagegen is a PLACEHOLDER. This is the running list so we can systematically replace them with PixelLab-made finals later. **Rule:** after EVERY cx imagegen run, append its outputs here. Keep them at PixelLab-standard resolution + cleanly cell-sliceable + transparent so the swap is seamless ([[feedback-imagegen-asset-pack-clean-cell-split]]).

Status legend: 🟡 placeholder (awaiting PixelLab) · 🟢 replaced by PixelLab · ⏳ generating

| Date | Asset(s) | Path | Type | Status |
|---|---|---|---|---|
| 2026-05-31 | Floor perspective concepts (01_isometric, 02_topdown_3q, 03_wallless_improved) | `STAGING/floor_perspective_concepts/` | concept refs (not shipped) | 🟡 ref only |
| 2026-05-31 | Map fragment + reward pickups (map_fragment, reward_skill_rune, reward_treasure, reward_soul_essence + contact) | `STAGING/fragment_reward_concepts/` | ground-pickup items, 64/128px transparent | ⏳ generating → 🟡 |
| 2026-05-31 (X-video frames) | NOT imagegen — extracted video frames (reference only) | `STAGING/xpost_dart_frames/` | — | n/a |
| ~2026-05-31 overnight | UI/HUD Pack (bg_seal_keep, pedestal_seal, panel_frame_9slice, card_frame_9slice, button_9slice, bar_frame_9slice, bar_fill) | `Resources/UI/RIMA/Pack/` | UI chrome (9-slice) | 🟡 |
| earlier S6 | Menu/hero screens (menu_bg_island, RIMA_MenuDungeonBackground, logo_rima_glyph, victory_bg_bloom, next_class_silhouette, death_overlay, wishlist_cta_btn) | `Resources/UI/RIMA/` | UI/menu chrome | 🟡 (verify origin per item) |

**NOT imagegen (do NOT log/replace here):** character sprites = PixelLab ONLY ([[feedback-imagegen-onbrand-not-realistic-s6]]); python procedural placeholders (death/lowhp/particles); extracted video frames.

**Audit TODO:** confirm which older `Resources/UI/RIMA/` items are imagegen vs python before the PixelLab replacement pass.
| 2026-05-31 | Top-down 3/4 cracked-stone floor tiles (td_floor_01-06 + td_floor_sheet) | `STAGING/floor_topdown_tiles/` | floor tiles, 64px opaque PNG, math-grid sheet | 🟡 |
| 2026-05-31 | Ruined-Keep wall kit (wall_tall_intact, wall_mid_cracked, wall_low_parapet, pillar_tall, pillar_broken, rubble_pile, arch_gate, corner_buttress) | `STAGING/ruinedkeep_wallkit/` | top-down 3/4 wall kit, 8 transparent PNGs, clean cell-split placeholder | 🟡 placeholder |
| 2026-05-31 | Ruined-Keep TILEABLE wall-run kit (wall_run_mid, wall_run_cracked, wall_run_low, wall_cap_left, wall_cap_right [+ optional wall_run_corner_NE/NW]) | `Assets/Sprites/Environment/RuinedKeepKit/` | flat front-face seamless-tiling wall-run segments, transparent PNG-32 @PPU64, clean cell-split placeholder | 🟡 placeholder |
| 2026-05-31 | iso_floor_diamond | `Assets/Sprites/Environment/IsoMockKit/iso_floor_diamond.png` | iso A/B mock placeholder -> PixelLab later | placeholder |
| 2026-05-31 | iso_wall_block | `Assets/Sprites/Environment/IsoMockKit/iso_wall_block.png` | iso A/B mock placeholder -> PixelLab later | placeholder |
- STAGING/iso_raw/floor_iso_sheet.png - iso asset pack RAW sheet -> sliced into IsoKit, PixelLab-replaceable later
- STAGING/iso_raw/keepwall_sheet.png - iso asset pack RAW sheet -> sliced into IsoKit, PixelLab-replaceable later
- STAGING/iso_raw/keepcolumn.png - iso asset pack RAW sheet -> sliced into IsoKit, PixelLab-replaceable later
- STAGING/iso_raw/keeparch.png - iso asset pack RAW sheet -> sliced into IsoKit, PixelLab-replaceable later
| 2026-05-31 | Iso asset pack explainer board | `STAGING/iso_raw/iso_pack_board.png` | design-board infographic placeholder, 1536x1024 | placeholder |
| 2026-05-31 | RIMA iso floor auto-merge sheet (iso_floor_256) | `STAGING/iso_raw/iso_floor_256.png` | 4x4 iso floor sheet, 256px cells, RGBA transparent | placeholder |
| 2026-05-31 | RIMA iso wall modular sheet (iso_walls_256) | `STAGING/iso_raw/iso_walls_256.png` | 4x3 iso wall sheet, 256x512 cells, RGBA transparent | placeholder |
| 2026-05-31 | RIMA iso decor modular sheet (iso_decor_256) | `STAGING/iso_raw/iso_decor_256.png` | 4x4 iso decor sheet, 256px cells, RGBA transparent | placeholder |
| 2026-05-31 | RIMA iso doorway arch (iso_arch_256) | `STAGING/iso_raw/iso_arch_256.png` | individual 512px iso arch, RGBA transparent | placeholder |
| 2026-05-31 | Iso room 3-way door concept | `STAGING/iso_raw/concept_room_3door.png` | concept render, 1536x1024 | placeholder |
| 2026-05-31 | Iso room-to-room transition concept | `STAGING/iso_raw/concept_room_transition.png` | concept render, 1536x1024 | placeholder |
| 2026-05-31 | Iso dungeon overhead map concept | `STAGING/iso_raw/concept_map_overhead.png` | concept render, 1536x1024 | placeholder |
| 2026-05-31 | Iso combat room concept | `STAGING/iso_raw/concept_room_combat.png` | concept render, 1536x1024 | placeholder |
| 2026-06-03 | gate_north | `STAGING/imagegen/assets/gate_north.png` | door threshold, 128x144, pivot bottom-center, north room gate | placeholder |
| 2026-06-03 | gate_west | `STAGING/imagegen/assets/gate_west.png` | door threshold, 128x144, pivot bottom-center, west room gate; east via flipX | placeholder |
| 2026-06-03 | rune_combat | `STAGING/imagegen/assets/rune_combat.png` | UI rune icon, 32x32, pivot center, combat room marker | placeholder |
| 2026-06-03 | rune_elite | `STAGING/imagegen/assets/rune_elite.png` | UI rune icon, 32x32, pivot center, elite room marker | placeholder |
| 2026-06-03 | obstacle_pillar | `STAGING/imagegen/assets/obstacle_pillar.png` | obstacle, 64x96, pivot bottom-center, cracked obelisk blocker | placeholder |
| 2026-06-03 | obstacle_wall_stub | `STAGING/imagegen/assets/obstacle_wall_stub.png` | obstacle, 128x80, pivot bottom-center, broken L wall blocker | placeholder |
| 2026-06-03 | chasm_gap | `STAGING/imagegen/assets/chasm_gap.png` | floor decal, 192x128, pivot center, visual chasm decal | placeholder |
| 2026-06-03 | floor_riftcrack | `STAGING/imagegen/assets/floor_riftcrack.png` | floor decal, 64x48, pivot center, cyan crack decal | placeholder |
| 2026-06-03 | brazier | `STAGING/imagegen/assets/brazier.png` | decor, 64x80, pivot bottom-center, warm flame marker | placeholder |
| 2026-06-03 | bones_marker | `STAGING/imagegen/assets/bones_marker.png` | floor decor, 64x48, pivot center, failed containment body marker; re-roll: clearer high-contrast skull/bones | placeholder |
