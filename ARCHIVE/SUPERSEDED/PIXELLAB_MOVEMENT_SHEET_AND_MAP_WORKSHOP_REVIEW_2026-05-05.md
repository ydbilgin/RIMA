# PixelLab / External Workflow Review -- Movement Sheets + Map Workshop
Date: 2026-05-05

Sources:
- X thread: https://x.com/techhalla/status/2051273931907010815
- YouTube: https://youtu.be/O9maOTbLuHQ

## Executive Read

Both sources are useful, but for different parts of RIMA.

- The TechHalla X thread is useful as an animation-control idea, not as a direct RIMA production pipeline.
- The PixelLab Map Workshop video is useful for fast map ideation and terrain-transition prototyping, not as a replacement for Unity-authored rooms.
- Best immediate RIMA use: adopt "reference-first" asset prompting. Generate or assemble pose/movement sheets before animation, and use map/object context for environment props.

## Source 1 -- TechHalla Movement Sheet Thread

Observed workflow:
- Main claim: cleaner 2D animation comes from using proper movement sheets as references instead of asking the model to invent poses from text only.
- Follow-up says the workflow combines:
  - GPT-2 to generate movement sheets
  - Nano Banana Pro for editing
  - Seedance 2.0 for animation
  - Magnific as the host/workflow wrapper
- Attached video shows a stylized hoodie character animated from movement-sheet references:
  - squash and stretch sequence
  - side kick sequence
  - laughing/gesture sequence
  - dance sequence
- Public replies include a useful warning: some generated motion is too linear; spacing, timing, and overlapping action still need animator judgment.

Production meaning:
- This is not "one prompt solves animation."
- The actual lesson is: make the AI copy a pose/timing plan instead of asking it to invent body mechanics.
- For RIMA, this maps directly to keyframe/pose planning for attacks, run cycles, hit reactions, and NPC idles.

RIMA decision:
- Use the concept, not the exact toolchain.
- Do not move RIMA animation production to Magnific/Seedance by default.
- Continue PixelLab-first, but add a movement-sheet/reference-sheet step before high-risk animations.

Recommended RIMA animation pipeline:
1. Create or collect a pose sheet for the action.
2. Keep the sheet style-neutral and silhouette-focused.
3. Generate or edit the RIMA sprite pose set from the sheet.
4. Use PixelLab interpolation / Animate with Text v3 only after the key poses are stable.
5. Manually review timing: ease-in/ease-out, spacing, overlap, anticipation, recovery.

High-value RIMA targets:
- Warblade: iron_charge, crippling_blow, death_blow, earthsplitter.
- Brawler: flying_knee, counter_blow, cyclone_drive.
- Shadowblade: phase_step, chain_cull, backstab_mark.
- NPC/background: idle, laugh, gesture, kneel, point, startled.

Risk:
- AI motion can look smooth but game-useless if the hit frame, hurtbox timing, or readable silhouette is wrong.
- Seedance-style video output is not automatically sprite-sheet-ready.
- External tools may bake camera motion, blur, or deformation into frames.

## Source 2 -- PixelLab Map Workshop Tutorial

Video metadata:
- Title: PixelLab Map Workshop Tutorial: Make Maps 10x Faster with Al Tilesets
- Channel: PixelLab
- Duration: 4:50
- Upload date from yt-dlp metadata: 2025-11-19

Observed workflow from transcript:
1. Create a new tileset.
2. Use lower terrain and higher terrain slots.
3. Drag an existing grass tile into the higher terrain slot.
4. Prompt lower terrain as "calm river blue water."
5. Choose transition size, example uses large.
6. AI generates water plus transitions between custom grass and water edge.
7. Paint the map background with grass, then paint a river.
8. Use Inpaint Map to add static terrain-integrated features, example prompt: "wooden bridge on the water."
9. Use Create Object for movable/selectable props, example prompt: "beautiful pine tree in a forest."
10. Objects can be selected, moved, duplicated, and edited in Pixelorama.
11. Drag and drop an existing character sprite onto the map.
12. Add another connected tileset from an existing tile, example prompt: "yellow dirt tile."
13. Generate a building object, example prompt: "front-facing compact red brick post office with a flat gray roof, centered glass entrance on the bottom, a blue postal sign above the doorway."

Important distinction:
- Inpaint Map is static: good for baked bridge/terrain details.
- Create Object is movable: good for props, obstacles, interactables, decoration, and later manual placement.

Production meaning:
- Map Workshop is good for visual sketching and transition exploration.
- It is not enough by itself for RIMA production rooms because gameplay needs sockets, collision, spawn zones, pathing readability, minimap logic, and authored combat space.

RIMA decision:
- Use Map Workshop as a concept/blockout accelerator only.
- Do not replace the locked room pipeline:
  - authored combat skeleton
  - connected naturalization
  - blueprint-defined gate sockets
  - Unity RuleTiles/RandomTiles/props
- Use Map Workshop outputs as reference tiles or style targets, then rebuild production-ready modules in Unity.

Best RIMA use cases:
- Fast Act 1 visual prototypes.
- Testing terrain transition language: cracked stone to rift water, rubble to flagstone, ash to blood-marked floor.
- Generating style references for PixelLab Create Image Pro / Tiles Pro.
- Creating prop concepts with map context: bridges, broken pillars, torches, collapsed stairs, shrine debris.

Avoid:
- Treating a Map Workshop map as final gameplay layout.
- Baked props that should be interactable or destructible.
- Full-route reveal maps that violate RIMA gate/minimap rules.
- One-shot "make a dungeon" prompts.

## Prompt Examples For RIMA

### Movement Sheet Prompt -- General

Use this outside PixelLab or in a reference-image generation tool to create a sheet that later guides PixelLab animation.

```text
Create a clean 2D movement reference sheet for a compact isometric ARPG character.
Action: heavy armored shoulder charge.
Camera: low top-down 30 degree ARPG view.
Show 6 clear poses in a horizontal row:
1. guarded idle
2. windup with weight shifted back
3. low forward launch
4. sprinting shoulder lead
5. impact pose with planted front foot
6. recovery back to guard
Use simple readable silhouettes, full body visible, same character scale in every pose, neutral background, no motion blur, no VFX, no extra weapons or redesign.
```

### PixelLab Animation Prompt -- Use After Pose Sheet

```text
Animate a heavy armored shoulder charge using the provided pose sheet as motion reference.
FOOTPRINT LOCK: identical pixel extents across all frames.
ANCHOR: feet aligned to the same pixel row, head height stable except for intentional crouch.
CONTINUITY: same character design, armor, palette, and weapon.
NO EMBEDDED VFX: no sparks, no trails, no particles.
ACTION: windup, low forward launch, shoulder-led dash, hard impact pose, recovery to guard.
full body, centered, same scale as reference, low top-down ARPG camera.
```

### PixelLab Map/Tile Prompt -- Act 1 Transition

```text
Lower terrain: dark cracked stone floor with faint cyan rift water in the cracks.
Higher terrain: worn castle flagstone floor with chipped edges and cold grey palette.
Transition: broken stone lip with thin cyan glow leaking between uneven slabs.
Style: compact 2D isometric ARPG dungeon tile, readable at 64px, selective dark outline, muted grey stone, cyan rift accent only.
```

### PixelLab Map Object Prompt -- Movable Prop

```text
broken stone shrine fragment with cyan rift cracks, compact dungeon prop, low top-down ARPG view, readable silhouette, muted grey stone, selective dark outline, transparent background
```

### PixelLab Inpaint Prompt -- Static Bridge Equivalent

```text
collapsed narrow stone crossing over a cyan rift fissure, broken slabs aligned as a walkable bridge, dark grey dungeon stone, subtle cyan glow from below
```

## Final Recommendation

Adopt both ideas as controlled pre-production helpers:
- Movement sheets become the planning layer for character motion.
- Map Workshop becomes the sketch/reference layer for tiles and terrain transitions.

Do not promote either into the main production pipeline until a small RIMA test passes:
1. Warblade iron_charge pose sheet -> PixelLab animation test.
2. Act 1 stone-to-rift transition tile -> Unity import test.
3. One movable shrine prop -> create_map_object or create_object comparison.

## Qwen-VL Segmented Video Pass -- 2026-05-06

Method:
- Downloaded the YouTube video at 360p for analysis only.
- Split it into three visual contact sheets:
  - segment 1: 0:00-1:40
  - segment 2: 1:40-3:20
  - segment 3: 3:20-4:50
- Fed each sheet to `qwen2.5vl:7b` with the matching transcript summary.
- Forced Ollama `num_gpu=999`; `ollama ps` reported `100% GPU`.

Qwen-VL confirmed:
- Segment 1: create new tileset, lower/upper terrain slots, custom grass as upper terrain,
  "calm river blue water" as lower terrain, large edge transition, AI transition generation,
  then painting grass and river onto the map.
- Segment 2: inpaint map for a wooden bridge over water, surrounding map used as context,
  create object for a pine tree, object can be moved/duplicated/edited, character sprite can be
  dragged onto the map for scale/readability checks.
- Segment 3: add a connected dirt path tileset from the existing grass tile, choose layer relation,
  prompt "yellow dirt tile", then create a front-facing compact red brick post office object.

Qwen-VL limitations:
- A single dense contact sheet caused generic answers.
- Feeding three images at once caused an Ollama connection reset.
- Best method is one segment image plus matching transcript text per call.
- Qwen sometimes misread "RIMA" as an acronym; final design interpretation must be done by us.

Updated RIMA takeaways:
- Use visual+text analysis for PixelLab tutorials: contact sheet alone is weaker than contact sheet
  plus transcript.
- For RIMA Map Workshop tests, require three outputs:
  1. terrain transition sheet
  2. movable object sheet
  3. Unity-imported screenshot with player scale check
- Treat Map Workshop as a style/blockout helper. Production rooms still need Unity sockets,
  collision, spawn zones, gate logic, minimap rules, and manual readability QA.
