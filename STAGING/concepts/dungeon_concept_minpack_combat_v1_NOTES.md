# Dungeon Concept Min-Pack Combat v1 Notes

Generated file: `STAGING/concepts/dungeon_concept_minpack_combat_v1.png`

## Asset breakdown

| Visual element in mockup | Asset proposal ID | Native canvas | Production method | Pre-production note |
|---|---|---:|---|---|
| Default dark granite slab floor | F01 | 64x64 | create_tiles_pro 4-mix | Primary base slab. Needs strong crack and stain variety without hard grid borders. |
| Alternate dark granite slab floor | F02 | 64x64 | create_tiles_pro 4-mix | Rotate/flip variation to reduce repetition across Room A. |
| Worn vertical walkway strip | F03 | 64x64 | create_tiles_pro 4-mix | Must read as foot-traffic smoothing from south entry to north wall. |
| Walkway trim variation | F04 | 64x64 | create_tiles_pro 4-mix | Use as secondary strip variation and transition support. |
| Northeast cracked rubble field | F05 | 64x64 | create_tiles_pro 4-mix | Needs debris density and softer blend into granite. |
| Center cyan hairline rift | F06 | 64x64 | create_tiles_pro 4-mix | First-pass rift should stay hairline, not become a large portal. |
| North wall segments x3 | W01 | 64x96 | create_object n_frames=16 | Verify left-right tile-mate edges and avoid repeated seam visibility. |
| East wall | W02 | 64x96 | create_object n_frames=16 | Needs front-face height and top cap perspective. |
| West wall | W02 | 64x96 | create_object n_frames=16 | FlipX from east wall; confirm lighting still reads correctly after flip. |
| Outer room corners | W03 | 64x96 | create_object n_frames=16 | One native plus rotation/flip variants; corners must hide wall joins. |
| East collapsed half-wall stub | W05 | 64x96 | create_object n_frames=16 | Good early blocker candidate because it proves ruined-wall silhouette. |
| Round intact column | P01 | 64x96 | create_object n_frames=16 | Needs vertical mass and grounded shadow for depth cue. |
| Broken leaning column | P02 | 64x80 | create_object n_frames=16 | Produce after P01 if column material lock is good. |
| North tattered banner | P03 | 48x80 | create_object n_frames=16 | Crimson cloth is a strong identity marker; avoid readable symbols/text. |
| Wall torches x2 | P04 | 48x64 | create_object n_frames=16 | Must include source flame; Unity lighting handles final halo. |
| Urn cluster | P06 | 48x48 | create_object n_frames=16 | Cluster should be irregular and readable at gameplay zoom. |
| Rubble pile | P07 | 64x48 | create_object n_frames=16 | Needs silhouette distinct from F05 rubble floor. |
| Moss edge scatter | D01 | 32x32 | create_object n_frames=64 | Use low-opacity variations along walls and blocker bases. |
| Hairline cracks | D02 | 32x32 | create_object n_frames=64 | Critical for breaking floor repetition. |
| Blood / ritual stain | D03 | 32x32 | create_object n_frames=64 | Should stay dark crimson and irregular, not glossy. |
| Dust film | D04 | 32x32 | create_object n_frames=64 | Use for NE corner and rubble transition softness. |
| Enemy HP bars | Runtime UI only | N/A | Unity runtime overlay | Not part of sprite pack; concept uses them only for combat readability. |
| Player Warblade placeholder | Character pipeline | N/A | Existing / separate character production | Scale target is roughly 1/12 room height in concept. |
| Three enemy placeholders | Enemy pipeline | N/A | Separate mob sprite production | Not part of the min-pack; used to validate room combat read. |

## PixelLab production order verdict

Produce W01 `wall_straight_n` first.

Reason: the concept succeeds mainly because wall height, north-wall continuity, and visible front faces establish the Hades-iso camera. If W01 fails, the room falls back toward flat tilemap read even if floors are good. W01 also becomes the style anchor for W02, W03, W05, banners, torches, and wall-adjacent lighting decisions.

Second priority should be the F01/F02/F03/F05/F06 floor set as a single palette-locked floor batch, because the room needs organic material transitions more than many prop variants. Third priority should be P04 wall_torch, since the warm/cyan dual-tone lighting depends on readable torch source points.

## Visual gap assessment

| Criterion | Verdict | Assessment |
|---|---|---|
| Tile read | TWEAK | Floor is far ahead of Phase K and visually coherent, but slab borders are still more visible than the best ChatGPT_TOPDOWN references. Production should rely on decals and blended transition tiles to soften repetition. |
| Wall height + perspective | PASS | North/east/west walls read as tall, front-faced Hades-iso architecture. This is the biggest improvement over Phase K. |
| Lighting dual-tone | PASS | Warm torches and central cyan rift create the target contrast. The center glow is readable without becoming a boss portal. |
| Prop density | PASS | Columns, banner, urns, rubble, collapsed stub, moss, cracks, and blood create enough visual density for a min-pack proof. |
| Character scale | PASS | Player and enemies are readable chibi scale and do not dominate the room. |
| Overall Hades match | TWEAK | Camera and atmosphere are close. Remaining gap is mostly floor-grid softness and final sprite consistency once PixelLab assets replace concept-painted objects. |

## Recommendation

Mockup quality enough to proceed PixelLab production: YES.

Proceed with W01 first, then floor material batch, then P04 torch. Do not regen the concept before PixelLab unless the next decision requires a stricter no-visible-slab-border reference. For production, prioritize wall perspective, palette lock, and decal density over adding new prop categories.
