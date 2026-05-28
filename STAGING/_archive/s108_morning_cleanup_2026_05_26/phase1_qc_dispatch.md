# Phase 1 QC Dispatch — RIMA Local Flux Room Generation

## Purpose
Compare 12 Phase 1 outputs against RIMA style references. Decide pipeline winner + escalation path.

## Inputs

### Generated outputs (Phase 1)
Path: `F:/AI/ComfyUI/output/RIMA/rooms/`
- `sdxl/small_diamond_*.png` (2 seeds)
- `sdxl/ritual_chamber_*.png` (2 seeds)
- `sdxl/boss_arena_*.png` (2 seeds)
- `flux/small_diamond_*.png` (2 seeds)
- `flux/ritual_chamber_*.png` (2 seeds)
- `flux/boss_arena_*.png` (2 seeds)

### Reference (target style)
- `F:/AI/ComfyUI/input/rima_master_room.png` — PixelLab master room (primary anchor)
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/concepts/chatgpt_ref/*.png` — 8 RIMA style refs
- `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/concepts/master_room_pilot/room_v1_gptimage.png` — gpt-image alt

## QC criteria (per output)

| Criterion | Weight | Pass threshold |
|---|---|---|
| Pixel art look (hard edges, no AA, native pixel grid) | 25% | Looks like pixel art, not blurry/painterly |
| RIMA palette (dark stone + warm torch + cyan rift) | 20% | Dominant dark stone, torches present, cyan accents present |
| Top-down iso ~85-90° perspective | 15% | Diamond V-shape iso, not front view, not pure top-down |
| Decor density (banners, torches, debris, runes) | 15% | At least 4-5 decor elements present |
| Composition match to prompt (small/ritual/boss) | 15% | Recognizable as the target room type |
| Pixel scale appropriate for 64px character | 10% | Stone bricks ~16-32px chunks, not too detailed/not too blurry |

Total: each image 0-100% score.

## Output expected

For each image:
- Score per criterion (% out of weight)
- Total %
- Major issues (if any)

Per pipeline (sdxl vs flux):
- Average score
- Strengths and weaknesses
- Best output filename

Verdict:
- **PASS** (avg ≥ 70%): proceed to Phase 2 with winner pipeline
- **PARTIAL** (avg 50-70%): identify fixable prompt/param issues, iterate Phase 1
- **FAIL** (avg < 50%): escalate to Phase 1.5 (custom LoRA training)

Recommended next steps with concrete actions.

## Constraints
- Compare visually using Read tool on the image files
- No code changes
- Output: structured markdown report
- Save report to: `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/phase1_qc_report.md`
