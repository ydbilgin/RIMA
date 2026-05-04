---
name: PixelLab animation production techniques
description: Proven workflow techniques for character animation -- run cycles, skeleton, interpolation, pose extraction
type: feedback
---

## Run Cycle Workflow (community + staff confirmed, May 2026)

### Brian's Extreme Pose Method (works for N/S directions)
1. Feed idle pose into Animate with Text
2. Prompt: "running fast, alternating arms and legs as they lift their knees" -- generate 12 frames
3. Pick best extreme pose (high knee, long leg) -- this becomes seed frame
4. Delete all others. Copy seed frame, flip horizontally.
5. Interpolate between the two extreme frames
6. Choose best iteration, manual cleanup + flipping

### Staff Shortcut (Kaninen, PixelLab MEGA)
"animate → get a mid pose → use that as reference → generate straight away"
Even faster: Edit Image Pro to extract a specific pose, then use as reference for animation.

**Why:** N/S works well. E/W directions unreliable with this method.

## Pose Extraction for Animation
- Use Edit Image (Pro) to get specific poses for interpolation/animation targets
- Better quality than generating poses from scratch via animate
- Cost: 40 gens per edit image pro run

## Skeleton Animation Tips
- "Freeze one, generate two frames" technique = best quality
- Fine-tune skeleton estimation before generating -- highest leverage step
- Fix initial element placement (weapon position, glove location) BEFORE animating
- Clean up each frame BEFORE generating the next batch

## Animation to Animation Tool
Transfer motion from one character's animation to another character.
S43 USE: Transfer Warblade walk/run cycle to Elementalist, Ranger, Shadowblade.
Saves ~4 animation runs per class.

## Animate with Text -- Frame Counts
- 64x64: up to 16 frames
- 128x128: up to 16 frames (our S43 canvas)
- 256x256: up to 8 frames
- Last frame chaining: use final frame as new reference for infinite extension

## Interpolation (Animate Between 2 Frames)
- Best for: attack animations, walk cycles, portals, chest opening
- Same frame limits as above
- Cleanup in Pixelorama/Aseprite afterward

**How to apply:** Always attempt Brian method or staff shortcut before skeleton for run/walk.
Use skeleton only for complex custom attacks where timing control matters.

## Weapon Separation Rule (Kaninen staff, HIGH confidence)
Generate base character animation WITHOUT weapon first.
Add weapon in second pass via "Edit Image Pro" or "Transfer Animation Pro".
Mixing weapon into initial animation = inconsistent weapon placement across frames.

## snowli_on 2-Step Batch Workflow (community-validated)
Step 1: Use "Create image from style reference (pro)" with base sprite as reference.
Prompt ALL needed poses in one call: walking, hurt, knockdown, attack, jump, run.
Step 2: Feed each pose into interpolation tool or "Animate with Text NEW" for smoothing.
"The AI thrives from a stable first frame reference."

## North/Forward Walking Prompt Fix (staff + community confirmed)
"walking forward" or "walking north" prompts produce BACKWARD walking animations.
Fix: use "walking away from camera" explicitly. Use Custom (v3) mode.

## animate-with-text VERSION RULE
- v2 stalls at exactly 49-50% progress in ~90% of attempts. NEVER use v2.
- v3 is the only valid endpoint. Confirmed: `POST /v2/animate-with-text-v3`.

## Complex VFX Fallback
"Animate with Text NEW" fails on multi-stage sequences (burn-to-ash death etc).
Fallback: "Animation Pro" tool for death/transformation sequences.

## LLM as Prompt Engineer (community-validated)
Run animation prompts through Claude or ChatGPT first before submitting to PixelLab.
Community: "Once ChatGPT got it right it was smooth sailing."
This is especially useful for getting precise action descriptions for attack animations.