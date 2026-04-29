# PixelLab Scale Research

## Root Cause
Idle and run were generated through different PixelLab paths with different framing behavior: `Create Character` (single-pose portrait-like framing) vs `Animate Character / Custom Animation V3` (motion-aware framing for strips). In practice, animation generation tends to keep subject larger in-frame to preserve motion readability, while single-pose generation often leaves more empty canvas.

Observed on actual assets:
- `warblade_idle_S.png` (128x128) non-transparent bbox height: `58px` (~45.3% of canvas)
- `warblade_run_E.png` frames bbox height average: `77.8px` (~60.7% of canvas)
- Effective run/idle height ratio: ~`1.34x`

So Unity import/PPU is not the root issue here; sprite art occupancy is inconsistent before import.

## Create Character Option
Using `Create Character` for idle can work, but scale control is indirect.

What can be controlled:
- Canvas size (`128x128`) directly
- Framing/zoom behavior only via prompt wording + reference locking
- Direction/style consistency via style/reference image

Prompt keywords that help scale/framing:
- `full body, centered`
- `same scale as reference sprite`
- `no zoom-in, keep consistent margins`
- `character occupies ~60% canvas height` (soft hint, not guaranteed)

Pros:
- Fast to produce clean static base poses
- Good for first concept/base identity capture

Cons:
- Scale is less deterministic vs run strips
- Mixing `Create Character` (idle) and `Animate` (run) is the main drift source
- More re-generation needed to match run occupancy

## Animate Character for Idle Option
Using the same `Animate Character / Custom Animation V3` flow for idle is the stronger consistency option.

Expected consistency behavior:
- Same generation engine, same character reference, same direction, same canvas
- Idle/run framing tends to align better automatically

Suggested workflow:
1. Start from the same Warblade character/reference used for run.
2. `Add Animation -> Custom Animation V3`
3. Idle generation settings:
   - `Keep first frame (idle pose): ON`
   - `Frame count: 4-6`
   - `Start frame`: base idle
   - `End frame`: slight weight-shift idle frame (gallery/custom frame)
4. Generate per direction with identical framing constraints in prompt.
5. Run bbox QC (target tolerance ±5% vs baseline direction frame).

Risks:
- Minor jitter/micro-motion artifacts in idle frames
- Higher cost than static create
- Still possible drift if prompts/references are changed between directions

## Recommended Base Pose
For top-down ARPG (~60-degree overhead), base idle should be **combat-ready neutral**, not T-pose and not run-like motion.

Recommended idle pose spec:
- Slight combat crouch (small knee bend), torso stable
- Weight centered or very slight forward bias
- Feet shoulder-width; no stepping motion
- Weapon visible but compact (no extreme extension)
- For Warblade: two hands on hilt, blade held in controlled guard (hip-to-shoulder diagonal), silhouette readable from top-down

Why this pose:
- Reads as "ready" while stationary
- Blends cleanly into run/attack
- Avoids huge silhouette jumps caused by exaggerated idle weapon placement

## Prevention Rules (for CODEX.md)
1. Do not mix pipelines for blend-critical states: generate idle and run with the same PixelLab flow (`Custom Animation V3`) and same character reference.
2. Lock one baseline scale frame per class/direction and enforce bbox-height tolerance `±5%` for all related clips.
3. Keep import constants fixed (`128x128`, `PPU=64`, `Multiple`, `center pivot`) and treat size issues as art occupancy problems, not Unity scaling problems.
4. Add framing constraints in every prompt: `full body, centered, same scale as reference, no zoom-in`.
5. Add pre-Unity QC step in Aseprite: compare idle/run overlay (same direction) and reject/regenerate if occupancy or anchor differs visibly.

## Recommendation
For this project, use `Animate Character / Custom Animation V3` for both idle and run (idle as low-motion 4-6 frame clip with Keep First Frame ON) and keep a per-class baseline scale check. `Create Character` should remain for initial concept/base capture only, not as the final source for blend-critical idle when run is generated via animation flow. This minimizes scale drift and reduces Unity-side troubleshooting.
