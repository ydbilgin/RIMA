# PixelLab Research Synthesis -- S43 Production Reference
<!-- date: 2026-05-02 | sources: Discord #share-tips-tricks + 15 YouTube tutorials -->

## API Rules (Non-Negotiable)

- **2 seconds minimum between API calls** (staff confirmed: Kaninen/MEGA)
- bitforge model = old/hidden -- AVOID. Use Pro tools only.
- Credit cost: all Pro tools = 40 credits per run
- Pro tool batch sizes at 40 credits:
  - 32x32 canvas -> 64 images
  - 64x64 canvas -> 16 images
  - 128x128 canvas -> 4 images (inferred)
- Animate with Text: **costs 40 gens per run** -- plan before triggering

## Character Animation Workflow

### Run Cycle (Brian method -- community gold, staff confirmed)
1. Feed idle pose -> Animate with Text -> prompt: `"running fast, alternating arms and legs as they lift their knees"` -> 12 frames
2. Pick best extreme pose (high knee, long leg back) as seed frame
3. Delete all others; copy seed frame + flip horizontally for opposite extreme
4. Interpolate between the two extremes; pick best iteration
5. Manual cleanup + flipping in Aseprite/Pixelorama

- Reliable for N/S directions only -- E/W not consistent
- Staff shortcut (Kaninen): `animate -> get mid pose -> use as reference -> generate straight`
- Staff tip: "edit image (pro) to get poses to interpolate or animate from"

### Skeleton Animation
- "Freeze one, generate two frames" technique = highest quality result
- Always fine-tune skeleton estimation before animating -- highest-leverage step
- Fix weapon/element position BEFORE animating, not after
- Clean up frames before generating subsequent ones

### Animation to Animation Transfer
- Transfers motion from an existing animation to a new character
- **Use this for S43:** transfer Warblade walk cycle to other Wave 1 characters

### Frame Counts (128x128 canvas)
- Animate with Text: up to 16 frames
- Interpolation (animate between 2 frames): up to 16 frames
- Chain last frame as new reference for infinite extension

## Style Consistency Protocol

- Tool: "Create image from style reference"
- Upload **5-10 reference images** -- not 60 random ones
- Prompt rule: describe WHAT to generate, not the style
  - Good: `"medieval weapons"`, `"small slime enemies"`
  - Bad: `"in the style of my reference images, make a..."` (style is implicit)
- Chain: use previously-generated assets as concept images for next generation -- locks visual language
- **Hex color palette input** -> dramatic consistency improvement (staff confirmed)
  - Pass palette hex codes directly in prompt: `"Palette: #1a1a2e #16213e #0f3460 #e94560"`
- Reference Pro for icon/item batches: `"Follow the exact reference image's 2D Fantasy RPG spritesheet layout and style, and generate: [item list]"` with 2-3 reference images
- Numbered list prompts for item batches: `"Various weapons and tools: 1. Longsword 2. War hammer 3. Dagger"`

## Tile & Object Production

### Isometric Tiles
- Tile type dropdown: thick / thin / block / reference -- pick per use case
- Adjust **Guidance Weight** in Advanced Options -- critical for quality; tune per tile type
- init image parameter: controls closeness to reference image
- Post-gen cleanup: reduce color count, fix artifacts in Aseprite

### Map Building
- Layer order: ground tiles first -> decoration on top -> objects last
- Edit Image (pro) + Inpainting (pro) can build maps without tile-by-tile placement
- Use Extend Map tool for high-resolution scene extensions

### Destructible Objects
- Simple damage state: Edit Image (normal) -- 1 gen
- Complex destruction: Edit Image Pro -- 40 gens
- Prompt chain for destruction: `"make it damaged"` -> `"more cracked"` -> `"crumbling"` -> `"cut in half"`
- Full destruction animation: Animate with Text Pro + Interpolate Pro combined
- Object Creator: use "Create state" for damage/variation states

### Object Creator Notes
- 8-direction rotation supported natively
- Animation Pro vs V3: Pro = higher quality + higher cost; V3 = cheaper

## Icon / UI Production

- Generate UI tool: always enable **"remove background"**
- Paste hex color palette for cross-element consistency
- Icon grid prompt format (256x256): `"10x16 icons in a 1x1 grid, spaced out evenly. 1. [item] 2. [item] ..."`
- Chain: use previous UI output as concept image for each new element
- Background rule: generate background separately, composite sprite on top -- never mix in one generation
- Inpainting match trick: place existing sprite on left of canvas, inpaint on right with context description

## Budget Notes

- Remaining: ~2400 credits | Expires: 2026-05-18
- Each Pro run = 40 credits -> 60 runs total remaining
- Animate with Text = 40 credits per trigger -- do not test-fire
- 128x128 create_tiles_pro = 40 credits for 4 images -> 10 credits/image effective
- Priority spend: animation runs > tile generation > UI icons

## THIS WEEK -- Top 3 Things to Try

1. **Run cycle via extreme-frame interpolation** (Brian method): feed Warblade idle -> generate 12-frame run -> extract two extreme frames -> interpolate -> saves ~3 manual animation runs vs frame-by-frame
2. **Hex palette lock on all new generations**: extract RIMA palette hex codes from existing approved assets, paste into every new prompt -- eliminates style drift across tile/char batches
3. **Animation to Animation transfer**: use confirmed Warblade walk cycle as motion donor for Ranger and Brawler -- test one direction first, estimate credit savings before full batch
