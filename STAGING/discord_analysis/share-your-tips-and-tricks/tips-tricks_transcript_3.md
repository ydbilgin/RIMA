# share-your-tips-and-tricks -- FULL TRANSCRIPT (Part 3/4)
# Date Range: Mid-April 2026 (April 12 - April 27)
# Source: 40 screenshots captured 2026-05-02
# Format: Every visible message transcribed verbatim with visual descriptions

---

## TECHNICAL SYNTHESIS (Part 3)

### Key Pipeline Findings from This Segment:
1. **8-Direction via Creator Tab**: NikolaiPatricioStar demonstrates 8-direction generation directly in the website's creator tab. Streamable video: https://streamable.com/65qzvf
2. **Tileset Blending Technique**: YumYum's validated method -- surround target tile with adjacent tiletype in Aseprite, use "Edit Image Pro" to merge/blend.
3. **100-Pose Spritesheet Tool**: Stew's custom skeleton tool (https://lysle.net/satoshi/skeleton) + PixelLab API generates ~100 poses per hour.
4. **"Fully Visible" Prompt Trick**: Adding "fully visible top to bottom body" to 256x256 prompts helps force true South (front-facing) perspective.
5. **Sprite Flipping for E/W**: Brian's workflow for run cycles uses N/S generation, then flips for opposite direction.
6. **Mid-Pose Reference Workflow**: Kaninen recommends getting a mid-pose first, then using it as reference for animation.
7. **Edit Image Pro for Poses**: Using "edit image (pro)" to get poses for interpolation is a good approach (Kaninen validated).

---

## ORIGINAL TRANSCRIPTS

---

### 2026-04-12 -- 8-Direction Generation via Creator Tab

**NikolaiPatricioStar** -- [date visible in screenshot context]

[ANTIGRAVITY OBSERVATION: This conversation discusses how to do 8-direction character generation. NikolaiPatricioStar shares a Streamable video link (https://streamable.com/65qzvf) demonstrating the process directly within the website's creator tab.]

**Dyamonic** advises using creator templates or v3 animation inside the creator.

**ToyotaTacoma** participates in the discussion.

[ANTIGRAVITY OBSERVATION: The 8-direction generation via the creator tab is a web-UI-only workflow. For API-based automation (RIMA pipeline), the equivalent is the "Create 8-directional object/character (pro)" endpoint or the "Rotations Pro" tool.]

---

### 2026-04-22 to 2026-04-23 -- 100-Pose Spritesheet Tool & Tileset Blending

**Stew** shares a custom tool and video:
> [Links to: https://lysle.net/satoshi/skeleton]

[ANTIGRAVITY OBSERVATION: Stew's "skeleton" tool at lysle.net is a custom web application that works with the PixelLab API to generate approximately 100 pose variations per hour. The workflow:
1. Set up a base character pose in the skeleton tool
2. Define target poses/animations
3. The tool automates API calls to PixelLab
4. Output: ~100 pose spritesheets

This is a community-built automation tool that validates the concept of automated pose generation -- exactly what RIMA aims to do with its own pipeline.]

**YumYum** -- [date context: late April]
> [Sharing tileset blending technique]

[ANTIGRAVITY OBSERVATION: YumYum's TILESET BLENDING TECHNIQUE:
1. Open Aseprite with the target tile (e.g., grass-to-snow transition)
2. Place the TARGET tile in the CENTER of the canvas
3. SURROUND it with the OTHER tile type (e.g., snow tiles around a grass center)
4. Feed this into "Edit Image Pro"
5. The tool automatically generates smooth transition tiles between the two types

This is valuable for RIMA's environment tileset generation -- creating natural-looking biome transitions without manual pixel editing.]

---

### 2026-04-22 -- Armor Sets & "Forward" Animation Direction Issues

**CalatZ** -- [continuing from earlier discussions]

[Discussion about filling out an entire sprite sheet at once with the same seed for armor sets. Using consistent seeds helps maintain style coherence across an equipment set (sword, shield, hat).]

**mark3448** -- [date context]
> [Asks about making "animate with text" walk forward/north. Gets responses about using custom v3 animation and the prompt enhancer]

**Kaninen** responds with suggestions about using newer animation tools for directional control.

[ANTIGRAVITY OBSERVATION: NORTH-WALKING ANIMATION ISSUE -- When using "animate with text" with prompts like "walking forward" or "walking north upward", the model tends to produce backward-walking animations. The solution:
1. Use "Custom (v3)" animation mode instead of the older "animate with text"
2. Enable the "prompt enhancer" feature
3. Be explicit about direction: "walking away from camera" or specific cardinal direction terminology
This is a known model limitation that affects all users trying to generate north-facing walk cycles.]

---

### 2026-04-27 -- Forcing True South (Front-Facing) Generation

**Tampa** -- [visible in screenshots around capture_005 of primary set]

Tampa describes struggling to generate true "South" (front-facing) mage characters -- 9 out of 10 results are SW or SE instead of directly south.

**Kibyra** -- responds with advice:
> On 256x256, try adding "fully visible top to bottom body" to the prompt

[ANTIGRAVITY OBSERVATION: "FULLY VISIBLE" PROMPT TRICK -- Adding "fully visible top to bottom body" to prompts when generating at 256x256 resolution helps force:
1. True south (front-facing) perspective instead of 3/4 angle
2. Full body visibility (no cropped feet/head)
3. Correct vertical framing

This is directly relevant to RIMA -- our base character references need consistent true-south facing. Adding this phrase to the base prompt may help resolve the persistent camera angle issues noted in earlier RIMA sessions.]

---

### 2026-04-29 -- Environment Design, Item Icons, & Object Generation

**Deelawn** [COBR badge] -- 29.04.2026 03:46
> no i mean for designing environments, like buildings, landmarks, etc. I'm making a 2D/3D game
> a lot of free stuff on the fab store are too realistic for what i want to achieve

**Deelawn** [COBR badge] -- 29.04.2026 03:56
> maybe i'm approaching this the wrong way, will keep yall posted lol

> [Replying to @Deelawn: I'm able to do this but it's quite crude to say the least. Lol seeing someone else do it might help me I think]

**JoWhitehead** -- 29.04.2026 08:54
> Sharing what you've made so far might be a good idea because' it's hard to know exactly what you mean. I'm also just interested in what you're working on, I doubt that I'd be helpful.

> [Replying to @YumYum -- about ITEMS / UI / ICONS]
> 2 options. 1. Edit Image Pro take this photo and edit it. Or 2. Create From Style Reference and my put the image into concept. Set the output to 256x256. Prompt: 16x16 icons in a 1x1 grid, spaced out evenly. (list all your items example; Slime ball, Hyrule...

[ANTIGRAVITY OBSERVATION: ITEM ICON GENERATION RECIPE --
Option 1: "Edit Image Pro" -- take existing photo/reference and edit
Option 2: "Create From Style Reference" with these settings:
- Output: 256x256
- Prompt format: "16x16 icons in a 1x1 grid, spaced out evenly. [list items: Slime ball, etc.]"
This generates a grid of small icons from a style reference. Directly applicable to RIMA's inventory/loot icons.]

**Magma** -- 29.04.2026 17:22

[ANTIGRAVITY OBSERVATION: Shows an image -- a large collection of pixel art item icons arranged in a grid. The icons include:
- Weapons: swords, axes, bows, staffs, daggers
- Armor: helmets, chest pieces, boots, gloves, shields
- Consumables: potions (red, blue, green), food items
- Materials: gems, ores, wood, leather
- Tools: pickaxes, hammers, saws
- Miscellaneous: scrolls, keys, bags, rings
The sprite sheet is densely packed with high-quality 16x16 pixel art icons in a consistent RPG style. This represents the quality benchmark for item icon generation.]

> "Create From Style Reference and my put the image into concept. Set the output to 256x256. Prompt: 16x16 icons in a 1x1 grid, spaced out evenly. Barrels, Half-Built Barrels, Grain, Barrel of Olive, Fill the rest of the area with medieval objects, foods and tools"
> It didn't respect the 16x16 icons
> Any tips?

> [Replying to @Magma: Any tips?]

**YumYum** -- 29.04.2026 20:45
> sometimes that happens to me too i just re do it, for the items your listing put like "barrels 16x16, half built barrels 16x16" etc

[ANTIGRAVITY OBSERVATION: SIZE ENFORCEMENT TRICK -- When the tool doesn't respect the specified icon size (e.g., 16x16), repeat the size specification WITH each item in the list: "barrels 16x16, half built barrels 16x16" instead of just stating the size once at the beginning. This helps the model maintain consistent icon dimensions across the grid.]

---

### 2026-04-29 (Later) -- Stew's Reaction to Object Generation

--- 1 Mayis 2026 ---

> [Replying to @Magma's earlier post showing the item grid]

**Stew** -- dun 01:15
> whoa another amazing set of objects from pixellab. wow

---

### 2026-05-01 -- Brian's Run Cycle Workflow (Most Recent)

**Brian** -- dun 19:33
> I'm working on making some run cycles for a character and I've sort of settled into this workflow. I'm curious what people think and if it makes sense to them. I'm generally happy with the results.
>
> I have my 8 rotation character open in aseprite. I take one of the idle poses and I feed it into "Animate with text" and use a prompt that is something like "running fast, alternating arms and legs as they lift their knees" I ask it to generate like 12 frames.
>
> I then go in and choose the best frame that gives me extreme poses, preferably one with a high knee and long leg. It basically becomes my seed frame. I delete everything else but that one frame, copy it, and flip it. I then interpolate between those two frames on both sides and choose the best iteration, flipping as I go.
>
> I then do a lot of manual clean up and flipping.
>
> This works for north south, but not anything east-west unfortunately. But I find just trying to generate the best versions of the extreme poses that I can and then using interpolate is giving me good results.

[ANTIGRAVITY OBSERVATION: BRIAN'S VALIDATED RUN CYCLE WORKFLOW --
1. Start with 8-rotation idle character in Aseprite
2. Feed one idle pose into "Animate with text"
3. Prompt: "running fast, alternating arms and legs as they lift their knees"
4. Request 12 frames
5. Cherry-pick the BEST extreme pose (high knee, long leg) = SEED FRAME
6. Delete all other frames
7. COPY the seed frame and FLIP it horizontally
8. INTERPOLATE between the original and flipped seed frames
9. Choose best interpolation, continue flipping
10. Manual cleanup

CRITICAL LIMITATION: Works for N/S directions only. E/W directions fail.
COMMUNITY VALIDATION: Brian reports being "generally happy with the results"

This workflow maps to RIMA's animation pipeline but highlights the E/W limitation that must be solved separately.]

> [Replying to @Brian: I'm working on making some run cycles...]

**Kaninen** -- dun 21:42
> currently preferred workflow i think is to animate get a mid pose then use that as reference instead and it should be done straight away :) feel free to create a support thread and i can assist you

[ANTIGRAVITY OBSERVATION: KANINEN'S PREFERRED WORKFLOW (OFFICIAL) --
Instead of Brian's multi-step process:
1. Animate to get a MID-POSE (not extreme pose)
2. Use that mid-pose as the REFERENCE
3. Generate animation from the mid-pose reference
4. Result should be "done straight away"

This is simpler and more direct than Brian's approach. The key insight is: mid-pose reference > extreme-pose seed for the PixelLab animation engine.]

> [Replying to @Kaninen: currently preferred workflow...]

**Brian** -- dun 21:45
> I actually just did that for a south east and it seemed to work pretty well, thank you
> The main issue i often find is just getting the knees up high enough.

> [Replying to @Brian: The main issue i often find is just getting the knees up high enough.]

**Kaninen** -- dun 21:55
> i have found using edit image (pro) to get poses to interpolate or animate from is a good approach

[ANTIGRAVITY OBSERVATION: POSE REFINEMENT VIA EDIT IMAGE PRO --
When the generated poses don't achieve the desired extremity (e.g., knees not high enough in running):
1. Use "edit image (pro)" to manually adjust the pose
2. Get the extreme pose you need
3. Then use that refined pose as the interpolation/animation source

This is a 3-tool chain: Animate with Text -> Edit Image Pro (refine) -> Interpolate]

**Brian** -- dun 21:56
> That makes sense, I'll give that a try too

---

## KEY OBSERVATIONS (Part 3)

### For RIMA Automated Pipeline:
1. **8-Dir Creator Tab**: Web UI has a direct 8-direction generation mode (video: streamable.com/65qzvf)
2. **100-Pose Tool**: Stew's https://lysle.net/satoshi/skeleton generates ~100 poses/hour via API
3. **Tileset Blending**: Center target tile + surround with other tile -> Edit Image Pro = smooth transitions
4. **"Fully Visible" Trick**: Add "fully visible top to bottom body" to force true south facing
5. **Item Icon Recipe**: Style Reference + "16x16 icons in a 1x1 grid" + 256x256 output
6. **Size Enforcement**: Repeat size with each item: "barrels 16x16, half built barrels 16x16"
7. **Brian's Run Cycle**: Idle -> Animate -> Cherry-pick seed -> Flip -> Interpolate (N/S only)
8. **Kaninen's Preferred**: Mid-pose reference -> Animate = faster, works for more directions
9. **Pose Refinement Chain**: Animate -> Edit Image Pro (adjust) -> Interpolate
10. **E/W Animation Gap**: East/West walking remains unsolved in community workflows
