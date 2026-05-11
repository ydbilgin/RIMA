# PixelLab Tool Guide

Last checked: 2026-05-11

Purpose: this guide is a practical reference for using PixelLab tools without re-checking the site every time. It is written so it can be given to another AI agent as context for choosing the right PixelLab tool for game asset generation.

Security note: do not paste API secrets, cookies, session tokens, billing links, or account-private credentials into prompts. This guide intentionally excludes them.

## Account Context

- Current plan observed in the UI: Tier 2: Pixel Artisan.
- Creator tools visible in the live UI were selectable under this plan.
- Monthly generation allowance observed: 5,000 generations.
- Pro tools commonly cost 20 generations per run.
- Some animation Pro tools cost 20-40 generations depending on size.
- Game Builder was shown as Coming Soon and disabled.
- Map Workshop had 0 maps and 0 tilesets at the time of inspection, so some "connect existing" options were disabled because there were no existing terrains to connect.

## Main Navigation

PixelLab has several separate work areas:

- Creator: `/create`
- Character Creator: `/create-character`
- Object Creator: `/create-object`
- Map Workshop: `/maps`
- Pixelorama editor: `/editor`
- Account: `/account`
- Docs: `/docs`
- API docs: `/pixellab-api`
- Vibe coding / MCP: `/mcp`

Use Creator for one-off image, transform, utility, and animation tools. Use Character Creator and Object Creator when you want managed asset libraries with export/delete/share actions. Use Map Workshop for terrain maps and tilesets.

## Creator Tool URL Mapping

Creator tools can be selected with URLs like:

```text
https://www.pixellab.ai/create?tool=<tool_id>
```

Observed tool IDs:

| Category | Tool ID | UI Name | Access | Cost |
|---|---|---|---|---|
| Create | `create_image_pixen` | Create image S-XL (new) | Included | not shown as Pro |
| Create | `create_m_xl_image` | Create M-XL image | Included | not shown as Pro |
| Create | `create_s_m_image` | Create S-M image | Included | not shown as Pro |
| Create | `image_to_pixel_art` | Image to pixel art | Included | not shown as Pro |
| Create | `create_image_pro` | Create image | Pro | 20 generations |
| Create | `create_from_style_pro` | Create from style reference | Pro | 20 generations |
| Create | `create_8_directional_pro` | Create 8-directional sprite | Pro | 20 generations |
| Create | `create_tiles_pro` | Create tiles | Pro | 20 generations |
| Create | `create_ui_pro` | Create UI elements | Pro | 20 generations |
| Create | `create_ui_basic` | Create UI elements | Experimental | not shown as Pro |
| Transform | `image_to_image` | Image to image | Included | not shown as Pro |
| Transform | `edit_image` | Edit image | Included | not shown as Pro |
| Transform | `edit_image_pro` | Edit image | Pro | 20 generations |
| Utility | `unzoom` | Unzoom | Included, New | not shown as Pro |
| Utility | `remove_background` | Remove background | Included, New | not shown as Pro |
| Utility | `pixel_art_correction` | Pixel art correction | Included, New | not shown as Pro |
| Animate | `generate_8_rotations_v3` | Generate 8 rotations | Included, New | not shown as Pro |
| Animate | `animate_with_text` | Animate with text | Included, New | not shown as Pro |
| Animate | `interpolate` | Interpolate | Included, New | not shown as Pro |
| Animate | `create_animated_pro` | Create animated object/character | Pro | 20 generations |
| Animate | `animate_with_text_pro` | Animate with text | Pro | 20-40 generations |
| Animate | `edit_animation_pro` | Edit animation | Pro | 20 generations |
| Animate | `transfer_outfit_pro` | Transfer outfit to animation | Pro | 20 generations |
| Animate | `interpolate_pro` | Interpolate | Pro | 20-40 generations |

## Which Tool To Use

### Fast General Pixel Art Image

Use `create_image_pixen` when:

- You want a new pixel art image from text.
- You need detail and outline controls.
- You want direction/view hints for characters but do not need multi-view output.
- You want larger size choices up to 768 in the Creator UI.

Fields observed:

- Description
- Direction: None, South, South-West, West, North-West, North, North-East, East, South-East
- View: None, Side/Sidescroller, Low top-down, High top-down
- Detail: Highly detailed, Low detail
- Outline: Default, Black outline, Single color outline, Selective outline, Lineless
- Init Image upload
- Width: 32, 64, 128, 256, 512, 768
- Height: 32, 64, 128, 256, 512, 768
- Transparent background

Recommended prompt structure:

```text
[asset type], [subject], [game camera view], [style], [palette/materials], [important silhouette details], transparent background, crisp pixel art, no anti-aliasing
```

Example:

```text
dark fantasy skill icon, cracked iron gauntlet punching forward, blue-purple rift energy impact burst, single centered object, transparent background, crisp pixel art, no anti-aliasing
```

### M-XL Image For Larger Assets

Use `create_m_xl_image` when:

- You want a larger or more composition-aware image.
- The target is 64px or larger.
- You need broad prompt understanding.

Fields observed:

- Description
- Direction: None, South, South-West, West, North-West, North, North-East, East, South-East
- Init Image upload
- Width: 64, 128, 256
- Height: 64, 128, 256
- Tier max size displayed: 400x400
- Transparent background

Use this for larger icons, props, portraits, scene snippets, and assets where prompt understanding matters more than tiny-pixel precision.

### S-M Image For Small Assets

Use `create_s_m_image` when:

- You want 16x16, 32x32, or 64x64 assets.
- The output is a small icon, item, tiny prop, or compact sprite.

Fields observed:

- Prompt
- View: optional Side, Low top-down, High top-down
- Direction: optional South, South-east, East, North-east, North, North-west, West, South-west
- Init Image, resized to 32x32
- Size: 16x16, 32x32, 64x64
- Transparent background

Prompt small assets with fewer details. Too many details will become unreadable.

### Image To Pixel Art

Use `image_to_pixel_art` when:

- You already have an image and want a pixel art conversion.
- You want a quick conversion instead of creating from text.

Fields observed:

- Source Image upload
- Upload accepts PNG, JPG, GIF up to 10MB
- Images larger than 1280x1280 are automatically resized

Use this for concept art, sketches, screenshots, or generated non-pixel references.

### Pro Create Image

Use `create_image_pro` when:

- You need higher quality generation.
- You want multiple reference images.
- You want a style image to guide the output.
- You are okay spending 20 generations.

Fields observed:

- Cost: 20 generations
- Description
- Output sizes: 32x32, 64x64, 128x128, 256x256, 344x192, 341x341, 384x216, 512x512, 512x288, 632x424, 424x632, 688x384, custom size beta
- Reference images: up to 4
- Style image: max 512x512
- Remove background

Use this for hero assets, detailed items, important character concepts, promotional sprites, and final-quality images.

### Create From Style Reference

Use `create_from_style_pro` when:

- You need new assets matching an existing art style.
- You have multiple reference images.
- You want a consistent pack.

Fields observed:

- Cost: 20 generations
- Style reference images: up to 8, max 512x512 each
- Description
- Optional style description
- Output: 64x64, 4x4 grid, 16 frames
- Remove background

Best use: feed several already-approved game assets, then request new assets in the same style.

### 8-Directional Sprite

Use `create_8_directional_pro` when:

- You need directional views for a character or object.
- You need all 8 directions for top-down or isometric gameplay.
- You can spend 20 generations.

Fields observed:

- Cost: 20 generations
- Methods:
  - Create new image with style reference
  - Create new image from concept + reference
  - Rotate reference character
- Reference image optional, max 168x168
- Description
- View: Low top-down, High top-down, Side
- Output sizes: 32x32, 48x48, 48x64, 64x64, 84x84, 96x96, 128x128, 160x160, custom size beta
- Remove background

For game characters, specify exact facing directions and silhouette requirements. Keep weapons visible from all views.

### Create Tiles

Use `create_tiles_pro` when:

- You need multiple game tile variations.
- You need isometric, hex, octagon, or square top-down tile shapes.
- You have style tiles to guide the output.

Fields observed:

- Cost: 20 generations
- Description
- Style tiles: up to 16, max 128x128
- Tile type: Isometric, Hexagonal flat-top, Hexagonal pointy-top, Octagonal, Square top-down
- Tile size: 16px, 16x32, 32px, 32x64, 48px, 48x96, 64px, 64x128, 96px, 128px
- View angle slider: 0 degrees side to 90 degrees top-down
- Thickness slider
- Outline mode: Outline, No outline

Prompt each tile with numbering for control:

```text
1). cracked dark stone floor 2). mossy dark stone floor 3). clean worn stone floor 4). blood-stained stone floor
```

For seamless tiles, explicitly state edge rules and palette constraints.

### UI Elements

Use `create_ui_pro` when:

- You need polished game UI assets.
- You want a concept image or color palette.
- You can spend 20 generations.

Fields observed:

- Cost: 20 generations
- Description
- Output sizes: 32x32, 64x32, 64x64, 128x64, 64x96, 128x128, 256x256, 344x192, 512x512, 632x424, 688x384, custom beta
- Concept image optional
- Color palette optional
- Transparent background

Use for buttons, frames, panels, inventory slots, skill frames, icons, HUD components.

Use `create_ui_basic` when:

- You want the experimental simpler UI tool.
- You want quick UI assets with init image support.

Fields observed:

- Experimental
- Description
- Init Image
- Width: 64, 128, 256
- Height: 64, 128, 192
- Transparent background

## Transform Tools

### Image To Image

Use `image_to_image` when:

- You want to preserve structure from a reference image.
- You want to transform an image using depth guidance.

Fields observed:

- Reference Image
- AI freedom slider, default displayed around 100 / very strict
- Init Image Strength slider, default displayed around 200 / creative
- Prompt
- Transparent background

Good for preserving pose/composition while changing style or subject.

### Edit Image

Use `edit_image` when:

- You want a simple image edit without the Pro tool.
- You need to add, remove, change, or replace something.

Fields observed:

- Edit Instruction
- Edit Image required
- No Background

Use direct edit verbs:

```text
change the cloak color to dark red
add a cracked blue rift mark on the sword
remove the background
replace the helmet with a hood
```

### Pro Edit Image

Use `edit_image_pro` when:

- The image edit is important or nuanced.
- You can spend 20 generations.

Fields observed:

- Cost: 20 generations
- Image to edit
- Edit description
- Remove background

Be explicit about what must stay unchanged:

```text
Keep pose, scale, outline, face, proportions, and transparent background unchanged. Only add a thin cold-blue crack along the blade.
```

## Utility Tools

### Unzoom

Use `unzoom` when:

- A pixel art image is scaled up and you want to detect pixel scale and downscale.

Fields observed:

- Image to unzoom

### Remove Background

Use `remove_background` when:

- You need transparent output from an existing image.

Fields observed:

- Image upload
- Background type: Simple background, Complex background

Use Simple for flat/solid backgrounds. Use Complex for noisy or detailed backgrounds.

### Pixel Art Correction

Use `pixel_art_correction` when:

- Pixel art needs cleanup/refinement.
- You want to reduce artifacts.

Fields observed:

- Images to correct
- Correction strength, default shown around 0.10
- Range labels: Subtle to Strong

Start subtle. Strong correction can change the design more aggressively.

## Animation Tools

### Generate 8 Rotations

Use `generate_8_rotations_v3` when:

- You have one reference image and need 8 directional rotations.
- You want the newer non-Pro rotation tool.

Fields observed:

- Reference image, max 256x256
- Transparent background

### Animate With Text, New

Use `animate_with_text` when:

- You have a reference image and want an animation from an action prompt.
- You want frame count control with the newer tool.

Fields observed:

- Reference image
- Animation action
- Frame count: 4, 8, 16
- Remove background

Keep action prompts about motion only:

```text
breathing idle, cloak sways gently
heavy sword slash from upper-right to lower-left
walk cycle facing south
```

Avoid environment or unrelated style changes in the animation action.

### Interpolate, New

Use `interpolate` when:

- You have first and last frames.
- You want PixelLab to generate intermediate frames.

Fields observed:

- First frame
- Last frame
- Animation action
- Frame count: 4, 8, 16
- Remove background

### Create Animated Object/Character Pro

Use `create_animated_pro` when:

- You want to create and animate from text in one flow.
- You can spend 20 generations.

Fields observed:

- Cost: 20 generations
- Description
- Animation action
- Output sizes: 32x32, 48x48, 64x64, 96x64, 64x96, 128x128, custom beta
- Output shown as 4x4 grid / 16 frames
- Remove background

### Animate Existing Image Pro

Use `animate_with_text_pro` when:

- You need higher-quality animation from an existing image.
- You are okay with 20-40 generations depending on size.

Fields observed:

- Cost: 20-40 generations depending on size
- Reference image
- Action description
- Output size automatically calculated based on reference image and grid size
- Remove background
- Camera view optional: None, Side, Low top-down, High top-down, Oblique
- Character direction optional: None, South, South-East, East, North-East, North, North-West, West, South-West

### Edit Animation Pro

Use `edit_animation_pro` when:

- You have animation frames and need consistent edits across them.
- You can spend 20 generations.

Fields observed:

- Cost: 20 generations
- Images to edit, max 512x512 each
- Edit description
- Remove background

### Transfer Outfit Pro

Use `transfer_outfit_pro` when:

- You have an outfit image and an animation, and want the outfit applied to the animation.

Fields observed:

- Cost: 20 generations
- Outfit image, max 256x256
- Images to edit, max 256x256 each
- Remove background

### Interpolate Pro

Use `interpolate_pro` when:

- You need higher-quality interpolation between start and end images.
- You are okay with 20-40 generations depending on size.

Fields observed:

- Cost: 20-40 generations depending on size
- Start image
- End image
- Optional action
- Output size and grid auto-determined from image dimensions
- Remove background

## Character Creator

URL: `/create-character`

Use when:

- You want managed character assets.
- You want character cards in a library.
- You want ZIP export and share showcase.

Observed controls:

- Main text prompt
- Create button
- Filter/settings panel:
  - Template Type: All, Humanoid, Quadruped
  - Camera View: All, Low Top-Down, High Top-Down, Sidescroller, Oblique
  - Number of Directions: All, 4 Directions, 8 Directions
  - Canvas Size range: 16x16 to 160x160
  - Character Proportions section

Observed library actions:

- Share character showcase
- Export character as ZIP
- Delete
- Select characters to download
- Select characters to delete

Use this for reusable character generation rather than one-off sprites.

## Object Creator

URL: `/create-object`

Use when:

- You want managed object assets.
- You want props, skill icons, environmental objects, or objects with rotations.

Observed controls:

- Main object prompt
- Create button
- Filter/settings panel:
  - Camera View: All, Low Top-Down, High Top-Down, Side
  - Number of Directions: All, 1 Direction, 4 Directions, 8 Directions
  - Canvas Size range: 16x16 to 256x256

Observed library actions:

- Export object as ZIP
- Delete
- Select objects to download
- Select objects to delete

Use Object Creator for asset libraries and ZIP exports. Use Creator one-off tools for quick tests.

## Map Workshop And Tilesets

Map Workshop URL: `/maps`

Observed:

- Maps tab
- Tilesets tab
- Search
- New Map button
- Empty account state at inspection: 0 maps, 0 tilesets

Create Tileset URL: `/create-tileset`

### Standard Tileset

Fields observed:

- Tile Size: 16x16, 32x32
- Map orientation: Top-down, Sidescroller
- Lower Terrain:
  - Create New
  - Upload Image
  - Connect Existing, disabled when no terrains exist
- Upper Terrain:
  - Create New
  - Upload Image
  - Connect Existing, disabled when no terrains exist
- Transition:
  - None
  - Small 25%
  - Large 50%
  - Full 100%
- Style Options collapsed section

### Pro Tileset

Fields observed:

- Tile Size:
  - 16x16 not supported yet
  - 32x32 supported
- Lower Terrain text
- Upper Terrain text
- Shape Controls:
  - Transition Height slider
  - Advanced Options
- Advanced Options:
  - Transition slider
  - Spread slider
  - Raggedness slider, default displayed around 15%
- Generate Pro disabled until required fields are filled
- Cost note: 20 generations
- UI note: Uses Gemini for higher quality results

Use Pro tilesets for higher-quality terrain transitions. Use Standard when you want guided terrain creation/upload/connect workflow.

## Practical Selection Rules

- Small item/icon at 16-64px: use `create_s_m_image`.
- General single pixel art asset: use `create_image_pixen`.
- Larger or more complex image: use `create_m_xl_image`.
- Highest quality one-off image with references: use `create_image_pro`.
- New assets matching an existing style: use `create_from_style_pro`.
- 8-direction character/object: use `create_8_directional_pro` or `generate_8_rotations_v3`.
- Tile variations: use `create_tiles_pro`.
- Terrain map tileset transitions: use `/create-tileset`.
- UI buttons/panels/icons: use `create_ui_pro`.
- Convert non-pixel image to pixel: use `image_to_pixel_art`.
- Preserve structure from a reference: use `image_to_image`.
- Simple edit: use `edit_image`.
- Important edit: use `edit_image_pro`.
- Remove background only: use `remove_background`.
- Clean artifacts: use `pixel_art_correction`.
- Animate an existing image: use `animate_with_text` or `animate_with_text_pro`.
- Create animated asset from text: use `create_animated_pro`.
- Animate between exact start/end frames: use `interpolate` or `interpolate_pro`.

## Prompting Rules For AI Agents

When another AI is choosing a PixelLab tool, ask it to output:

```text
Tool:
URL:
Reason:
Inputs:
Size:
View:
Direction:
Transparent background:
Cost:
Prompt:
Negative constraints:
```

For asset consistency, include:

- Camera view
- Direction/facing
- Pixel size
- Palette
- Material
- Silhouette constraints
- Background requirement
- "No anti-aliasing" when crisp pixel edges matter
- "Keep X unchanged" for edits

For character edits, always preserve:

- pose
- proportions
- face
- outline
- scale
- transparent background
- weapon visibility

For tiles, specify:

- tile geometry
- tile size
- edge rule
- seamless/tileable requirement
- lighting direction
- palette constraints
- numbered tile variations

For animations, describe motion only:

- body action
- timing
- frame count
- loop or non-loop
- start/end pose
- camera view
- facing direction

Avoid mixing generation, edit, and animation instructions in one prompt unless the selected tool supports all of them.

## Safe Operating Notes

- Do not include API secrets or account tokens in AI prompts.
- Do not ask an AI agent to click Generate unless you are ready to spend generations.
- Pro tools often spend 20 generations immediately.
- Animation Pro tools may spend 20-40 generations depending on size.
- Confirm image size before Pro animation.
- Prefer lower-cost/new/free tools for exploration, then move to Pro for final assets.

