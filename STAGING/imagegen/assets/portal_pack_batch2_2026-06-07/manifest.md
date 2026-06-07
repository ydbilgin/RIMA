# portal_pack_batch2_2026-06-07 manifest

Output folder: STAGING/imagegen/assets/portal_pack_batch2_2026-06-07/
Generated: 2026-06-07
Mode: built-in image_gen, one asset at a time, local magenta chroma-key cleanup, nearest-neighbor downscale/postprocess.
Reference family: STAGING/imagegen/assets/portal_pack_2026-06-07/ portal arches and ritual decal.
Chroma key: #FF00FF requested. Final PNGs have transparent backgrounds and no visible pure-magenta pixels in QC.
Skips: none.
Unity import: none.
Commit: none.

## Assets

| # | File | Final size | Retries | Source/process note | Prompt summary |
|---|---|---:|---:|---|---|
| 1 | portal_arch_combat_angled.png | 96x128 | 0 | Generated on magenta, keyed, downscaled. | Angled-left combat portal; slate stone, cyan slanted rift, crossed-blades rune, high 3/4 dimetric left-side depth. |
| 2 | portal_arch_elite_angled.png | 96x128 | 0 | Generated on magenta, keyed, downscaled. | Angled-left elite portal; slate stone dominant, cyan slanted rift, subtle deep-crimson cracks, skull-crown rune. |
| 3 | portal_arch_reward_angled.png | 96x128 | 1 | First pass too frontal/large base; retry keyed and downscaled. | Angled-left reward portal; calm dim cyan rift, soft gold trim, chest/star rune, small dimetric base. |
| 4 | portal_arch_boss_angled.png | 128x176 | 0 | Source key was slightly uneven; wider matte cleanup, downscaled. | Angled-left boss portal; oversized fractured slate arch, great-seal crest, dark red rune/cracks, dim cyan slanted core. |
| 5 | portal_arch_elite_v2.png | 96x128 | 1 | First pass had too much red; retry keyed/downscaled plus tiny crimson rune/crack pixel pass. | Frontal elite v2; batch-1 silhouette, slate dominant, crimson only as hairline cracks and keystone skull-crown accent. |
| 6 | boss_seal_fragment_set.png | 128x32 | 0 | Generated four shards, keyed, each shard extracted and placed into strip at preserved aspect. | Four floating slate rune shards with cyan rift cracks and glowing edges; no shadow. |
| 7 | telegraph_line_beam.png | 256x32 | 0 | Generated beam, keyed/cropped, resized, deterministic crimson/white recolor pass. | Horizontal crimson (#D8364C) beam ground decal with white hot centerline and end fade. |
| 8 | telegraph_circle_ring.png | 256x256 | 0 | Generated ring, keyed, resized. | Top-down perfect crimson ring, transparent center, thin outer line, faint inner glow, N/E/S/W notch marks. |
| 9 | telegraph_cone_fan.png | 256x256 | 1 | First pass too wide; retry attempted, then final procedural sector pass for exact 60-degree geometry and clean alpha. | Crimson 60-degree cone fan, apex bottom-center, sharp outer arc, white hot centerline, soft inner fill. |
| 10 | boss_intro_seal_ring.png | 256x256 | 0 | Generated top-down cyan seal, keyed, resized. | Cyan ritual seal ring ground decal; runic perimeter glyphs, radial ticks, inner geometric lines, transparent center. |

## QC results

Final audit command checked all root-level PNGs in this folder for expected dimensions, alpha mode, transparent corners, visible #FF00FF residue, and file count. See CODEX_DONE_yasinderyabilgin.md for the exact per-asset audit summary.
