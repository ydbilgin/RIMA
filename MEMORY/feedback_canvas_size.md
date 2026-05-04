---
name: PixelLab canvas size and frame limits
description: animate-with-text-v3 pixel budget formula. 252px = 8 frames. Interpolate for smooth motion.
type: feedback
---

animate-with-text-v3 obeys a pixel budget formula: width x height x frame_count <= 524,288

| Canvas | Max Frames | Notes |
|---|---|---|
| 128x128 | 16 | Sword clips at idle -- impractical for Warblade |
| 160x160 | 16 | Minimum safe size for characters without weapons |
| 252x252 | 8 | Project standard -- enough motion space, no clipping |
| 256x256 | 8 | Same as 252px |

**Why:** 252 * 252 * 8 = 508,032 which is within budget. 252 * 252 * 9 = 571,536 which exceeds it.

**Project decision (2026-05-02, LOCKED):**
- Keep 252px canvas for all character animation.
- 8 frames from "Animate with Text NEW" is the baseline.
- Use interpolation-v2 ("Interpolate NEW") between frame pairs to double effective count.
- 252px is an ADVANTAGE: more motion space = better keyframes = better interpolation output.
- Do NOT crop to 128px. The sword on Warblade already clips at 128px idle pose.

**How to apply:**
- Always use 252px canvas for character animation production.
- After 8-frame generation: run interpolation-v2 for attack/skill/dash clips.
- Idle clips: 8 frames is sufficient, no interpolation needed.
- Never recommend 128px for characters with weapons or large motion arcs.
- West-direction clips: generate east, flip in Unity (SpriteRenderer.flipX) to save credits.

**Pipeline doc:** STAGING/WARBLADE_ANIMATION_PIPELINE.md (Warblade-specific, reusable template)
