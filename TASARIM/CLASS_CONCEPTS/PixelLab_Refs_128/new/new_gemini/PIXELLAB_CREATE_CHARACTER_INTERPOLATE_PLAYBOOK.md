# PIXELLAB Create Character + Interpolate Playbook (RIMA)

Purpose: A complete, implementation-ready guide for generating class characters with PixelLab Create Character and producing smooth animations using an interpolate-first workflow.
Audience: Claude (decision + QC), production operators (execution).

---

## 1) Canonical Documentation Sources

Use these links as the authoritative source set:

1. Create Character (entry page)
- https://www.pixellab.ai/docs/tools/create-character
- Note: This page delegates to Create Image (Style).

2. Create Image (Style) (core generation options)
- https://www.pixellab.ai/docs/tools/style

3. Character options (description, negative, action, character type, direction type, scale)
- https://www.pixellab.ai/docs/options/character

4. Camera options (view/direction behavior and limits)
- https://www.pixellab.ai/docs/options/camera
- Important: view/direction controls are weak; init image often improves control.

5. Init image options (strength ranges)
- https://www.pixellab.ai/docs/options/init-image

6. Inpainting options (black mask behavior)
- https://www.pixellab.ai/docs/options/inpainting

7. Getting started (Aseprite flow + inpainting walkthrough)
- https://www.pixellab.ai/docs/getting-started

8. Ways to use PixelLab (Characters product capabilities)
- https://www.pixellab.ai/docs/ways-to-use-pixellab

9. Animation to animation (frame generation for consistency)
- https://www.pixellab.ai/docs/tools/animation-to-animation

10. Animate with skeleton (high control animation path)
- https://www.pixellab.ai/docs/tools/animate-with-skeleton

11. Create animated object/character (from scratch, animation+character)
- https://www.pixellab.ai/docs/tools/text2animation

---

## 2) RIMA Production Intent (Non-negotiable)

- We can use Create Character for fast base loops and simple animation starts.
- We must keep smooth transitions via Interpolate-first sequencing.
- We do not rely on a single one-click animation output for final quality.

Target result:
- Consistent class identity
- Directionally stable frames
- Smooth transitions between states (idle/walk/run/attack/hit/death)

---

## 3) Tool Capability Summary (Operational)

Create Character / Characters page:
- Generates 4 or 8 directional character outputs from text.
- Can add simple animations quickly.
- Best for rapid base asset creation.

Animation to animation:
- Generates frame sequences with consistency from references.
- Good for controlled variation and extending existing loops.

Animate with skeleton:
- Best control for combat arcs and pose-critical actions.
- Reusable skeletons supported.

Inpainting:
- Edits selected regions only.
- Draw black on "Inpainting" layer where modifications are needed.

Init image:
- Strongest practical control lever for consistency.
- Strength ranges (docs guidance):
  - 0-300 rough color guidance
  - 300-400 rough shape guidance
  - 400-600 medium guidance / variations
  - 600-900 detailed guidance / near-final edits

---

## 4) Recommended End-to-End Pipeline (RIMA)

### Phase A: Character Base Creation

1. Create Character with short, clean class description.
2. Generate multiple candidates.
3. Select one "master source" per class (best silhouette + readability).
4. Lock seed if available for repeatability.

Output:
- Master class sprite set (directional baseline)

### Phase B: Simple Loop Generation

1. Generate simple loops first: idle, walk, run.
2. Keep description stable across loops.
3. Use init image from master source when available.

Output:
- Baseline locomotion loops

### Phase C: Combat Set Creation

1. For attack/hit/death, use Animation to animation or Skeleton path.
2. Keep one reference style source per class.
3. Generate short segments instead of long monolithic animations.

Output:
- Segmented action clips (e.g., windup, strike, recover)

### Phase D: Interpolate-first Smoothing (Critical)

Use interpolate between segment boundaries and state transitions:
- idle -> walk
- walk -> run
- idle -> attack windup
- attack recover -> idle
- hit react -> locomotion return

Rules:
1. Interpolate on transition boundaries, not across entire clips.
2. Preserve silhouette anchors (head, shoulders, pelvis, feet) during interpolation QC.
3. If interpolation introduces drift, regenerate shorter transition windows.

Output:
- Smooth state transitions suitable for in-game blending

### Phase E: Cleanup + Export

1. Use inpainting for local fixes (weapon read, hand artifacts, face drift).
2. Use Aseprite cleanup for pixel polish.
3. Export frames/sheets to pipeline format.

---

## 5) Prompt Strategy (Create Character)

Use concise descriptions. Avoid overlong prompt stacks.

Recommended structure:
1. Identity line (class fantasy + role)
2. Silhouette line (major shapes, posture)
3. Equipment line (primary weapon, key worn gear)
4. Color accent line (single controlled accent system)
5. Exclusions line (short negative constraints)

Example skeleton:
- "female rift-tech gunslinger, practical dark coat, dual pistols hip-ready, copper-orange hair, subtle cold-silver barrel trim, worn gear, no cowboy style, no oversized VFX"

Do not overload with 20+ constraints unless fixing a specific failure mode.

---

## 6) Failure Modes and Corrective Actions

1. Squat / dwarf-like proportions
- Add: "tall readable full-body silhouette, long legs, normal head size"
- Reject and regenerate (do not patch with many extra clauses)

2. Camera/direction drift
- Reuse init image from accepted frame
- Keep camera text stable
- Prefer shorter generation steps

3. Weapon unreadable
- Inpaint weapon region only
- Request explicit weapon readability (clear silhouette separation)

4. Identity collapse across frames
- Move from one-click to animation-to-animation or skeleton path
- Freeze head behavior when needed (skeleton workflow guidance)

5. Over-noisy visuals
- Reduce VFX language
- Reassert practical worn gear and clarity-first silhouette

---

## 7) QC Checklist (Claude Final Gate)

Character QC:
- Class identity readable at gameplay scale
- Weapon/equipment readable in silhouette
- No proportion collapse (no short-leg/chibi drift)
- Palette consistent with RIMA dark fantasy tone

Animation QC:
- Loop continuity passes (idle/walk/run)
- Transition smoothness passes (interpolated boundaries)
- Combat timing readable (windup/impact/recover)
- No frame-to-frame face/armor instability beyond acceptable variation

Pipeline QC:
- Segmented clip naming is consistent
- Transition clips mapped in engine state machine
- Rejected generations are documented by failure reason

---

## 8) Operational Defaults (Recommended)

- Start simple loops in Create Character.
- Use Animation-to-animation for clip extension.
- Use Skeleton for precision actions.
- Use Interpolate for all state transitions.
- Use Inpaint for local repair, not full redraw.

This keeps speed high without sacrificing motion quality.

---

## 9) Claude Execution Policy (Project-specific)

When asked "can we do it like in the video":
- Answer: "Yes for simple loops and base generation."
- Add: "Final production smoothness requires segmented clips + interpolate transitions."

When quality drops:
- Do not add random prompt complexity first.
- First enforce reference/init consistency, then regenerate targeted segments.

When production is blocked:
- Fall back to skeleton path for the problematic action only.
- Keep the rest on faster Create Character path.

---

## 10) Short Decision Statement (Copy-ready)

Create Character is approved for simple animation generation in RIMA.
Final-quality animation pipeline remains interpolate-first:
- Generate base loops quickly
- Build combat as segmented clips
- Smooth all state changes with interpolate
- Use inpaint/skeleton only where control is required

This is the fastest path that preserves consistent class identity and smooth motion.
