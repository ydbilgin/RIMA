# PixelLab Workflow Insights

Purpose: capture practical PixelLab findings from Discord and local tests so future RIMA sessions can avoid repeating tool-selection mistakes during Phase 2-6 production.

## 1. Executive Summary

Key operating principle: use the Web UI Pro tools for sheet-level work, non-square dimensions, and style-controlled generation. Use MCP tools only where their output shape is already proven for the asset type.

Most important production conclusion: master sheets plus Edit Image Pro re-theming are the cheapest path for multi-act wall and decoration coverage. Per-piece generation should be reserved for hero props, style anchors, and variants that need individual review.

## 2. Tool Selection Matrix

| Need | Preferred tool | Avoid / caution |
|---|---|---|
| Large master sheet | Create Image Pro or Edit Image Pro | create_object |
| Whole-sheet re-theme | Edit Image Pro | independent per-piece calls |
| New single asset with exact dimensions | Create Image S-XL with init image | create_map_object for non-square |
| Variant in same style | Same Style Pro | unrelated fresh prompts |
| Floor terrain transitions | create_topdown_tileset / create_tiles_pro | wall-specific assumptions |
| Centered single prop | create_map_object | multi-object grid prompts |
| Animation frames | animate_object / animate_character | manual frame guessing |

## 3. Tool-by-Tool Analysis

### Create Image Pro

- Supports large canvases, including non-square canvases such as 688x384.
- Can edit or transform an existing image with a text instruction.
- Works well for asset-pack production and multi-tile sheets.
- Best use: master sheets, tilesets, and broad re-theming.

### Edit Image Pro

- Transforms an existing image, for example grass tileset to lava tileset.
- Strong prompt format: "Change the X to Y".
- Can re-theme a whole sheet in one call.
- Tileset adherence freedom around 925 produced smoother tile connections in testing.
- Best use: multi-act re-theme, damage states, palette shifts, and sheet variants.

### Same Style Pro

- Takes a reference asset and generates a new asset in matching style.
- Best use: style-anchored variants after a first approved asset.
- Main risk: the reference quality sets the quality ceiling for later outputs.

### Create Image S-XL

- Newer model with outline and detail controls.
- Standard sizes: 32, 64, 128, 256, 512, 768.
- Non-square presets include 344x192, 384x216, 512x288, 632x424, 424x632, and 688x384.
- Init Image locks the output dimensions to the init image size.
- Init Image Strength range is 0-1000; 300 is the current balanced refine baseline.
- Detail control: Low, Medium, Highly detailed.
- Outline control: Single color, Selective, Lineless.
- Direction should be None for non-character assets.
- View supports High top-down, Low top-down, and Side.
- Blank background should be enabled for standalone sprites.
- Best use: individual assets with controlled style and dimensions.

### create_object MCP

- Returned an "Unknown tool" error in this session and should be treated as unavailable until retested.
- create_object_state exists for variant chains.
- Size is square only, 32-256, max 256.
- Candidate counts are 1, 4, 16, or 64 frames depending on size.
- Best use remains unclear until the tool is available again.

### create_map_object MCP

- Accepts width and height from 32-400.
- Practical test showed non-square requests can clamp to square output: 96x160 became 96x96.
- Produces a centered single-object fill.
- Supports detail, outline, and shading controls.
- Best use: standalone single sprites such as props and items when square output is acceptable.

### create_topdown_tileset / create_tiles_pro

- Designed around floor terrain and Wang16 transitions.
- Supports multi-variant tile sets in one call.
- Important controls include tileset adherence, lower elevation, higher elevation, and transition descriptions.
- Best use: floor tilemaps, not walls.

### animate_object / animate_character

- Converts static sprites or characters into animation frames.
- Can interpolate between first and end keyframes.
- Best use: torch flames, brazier fire, rift pulse, banner sway, and similar looping motion.

## 4. Workflow Patterns

### Pattern A: Init-Locked Generation

1. Generate an exact-dimension init image with Python or another deterministic tool.
2. Upload it to the Web UI as the init image.
3. Select the matching Width / Height standard preset when available.
4. Use a verbose, AI-enhanced style prompt.
5. Set Init Strength to 300 for the current balanced refine baseline.
6. Generate and verify that output dimensions stayed locked.

Use for: exact wall blocks, columns, archways, and non-square assets.

### Pattern B: Asset Pack Master Sheet

1. Use a large canvas such as 256x256, 512x512, or a non-square sheet.
2. Build an init image with a clean grid of tile outlines or silhouettes.
3. Use Edit Image Pro with a sheet-level instruction.
4. Slice the resulting sheet into individual tiles with PIL or Unity Sprite Editor.
5. Prefer this when many assets share one theme.

Cost advantage: one call can replace many individual generations.

### Pattern C: Multi-Act Re-Theme

1. Produce and approve the Act 1 base sheet.
2. Use Edit Image Pro to re-theme it for Act 2, for example bone and rust.
3. Repeat for Act 3 and Act 4, for example void and gold, then mirror.
4. Keep geometry stable across acts unless the design requires a gameplay-readable change.

Use for: broad act coverage at low generation cost.

### Pattern D: 3D Box Reference Method

1. Generate an isometric or low-top-down box outline at the target dimensions.
2. Feed it into Edit Image Pro or Create Image S-XL.
3. Prompt the model to transform the box into a wall or structural asset.
4. Prefer filled silhouettes over wireframes when possible.

Risk: wireframe lines may leak into the output. Mitigate with filled silhouette, explicit negative prompt, or manual cleanup.

### Pattern E: Iterative Reference Build

1. Generate the first asset and approve it.
2. Use it as the reference for the next asset through Same Style Pro.
3. Continue the chain while monitoring style drift.
4. Restart from the best approved anchor if a later output drifts.

Use for: variant generation when a master sheet is not appropriate.

### Pattern F: Overlay Decoration

1. Keep base sprites clean and readable.
2. Generate decoration as separate overlay sprites: scatter, glow, banner, chain, cracks, runes.
3. Compose overlays in Unity as child SpriteRenderers.
4. Reuse overlays across multiple bases.

Production benefit: fewer baked-in variants and more runtime flexibility.

## 5. RIMA Sizing Standards

| Asset type | Locked size |
|---|---|
| Modular wall block tile | 32x64 |
| Low inner divider wall | 96x96 or 128x128 |
| Tall perimeter wall | 128x256 |
| Wall feature / archway | 256x256 |
| Narrow wall endcap / column | 128x256 with horizontal margin |
| Floor tile | 64x64 |

PixelLab batch sizing:

| PixelLab tier | Items per batch | Best use |
|---|---:|---|
| 32-40 px | 64 | tiny decals, pickups, runes |
| 48-80 px | 16 | small props, decorations |
| 88-168 px | 4 | walls, large props, features |

## 6. Known Limitations and Fail Patterns

### FAIL 1: create_object grid prompt

- Prompting "2x2 grid of 4 different walls" tends to produce one object.
- Reason: create_object is designed for single-object generation.
- Use Edit Image Pro for multi-item sheets.

### FAIL 2: create_map_object dimension clamp

- Test case: 96x160 request produced 96x96 output.
- Use Web UI Create Image S-XL for non-square assets.

### FAIL 3: wireframe leak

- 3D box outlines may survive as unwanted lines.
- Use filled silhouettes, a stronger negative prompt, or manual cleanup.

### FAIL 4: expanded canvas density mismatch

- Expanding a smaller init image can cause smoother generated content in the new area.
- Use an exact target-size init image and prompt for sharp pixel boundaries.

### FAIL 5: independent-call style drift

- Separate generations can produce slightly different styles.
- Use Same Style Pro reference chaining or a single master sheet.

### FAIL 6: weak text-to-tileset performance

- Direct prompts such as grass-bush transition tileset can produce unusable output.
- Use Edit Image Pro with a known-good tileset reference.

### FAIL 7: AI-enhanced prompt lore drift

- AI-enhancement can replace lore-specific details, such as cyan rift language becoming moss.
- Keep proprietary descriptions out of AI-enhance and manually preserve locked details.

## 7. Prompt Engineering Rules

Use these rules as defaults:

- Do not use proprietary names such as RIMA or Shattered Keep in generation prompts.
- Use familiar style anchors such as Hades, Diablo, Souls, or Stardew when useful.
- Prefer verbose prompts over terse prompts.
- Specify palette with hex codes when color identity matters.
- Keep each call focused on one concept.
- Put pixel-art constraints near the end of the prompt.
- Add negative prompts for blurry, anti-aliased, modern brick, photo-realistic, and white background.

Reusable pixel-art constraint:

```text
The output must be true pixel art with sharp 1:1 pixel boundaries throughout
the entire canvas. Every pixel must be a solid color block aligned to the
same pixel grid as the init image. NO anti-aliasing, NO smooth gradients,
NO blurry edges.
```

## 8. Cost Model

| Production type | Estimated cost | Output |
|---|---:|---|
| Single asset with create_map_object | about 30 gen | 1 asset |
| Edit Image Pro re-theme | about 30 gen | whole sheet |
| Same Style Pro variant | about 30 gen | 1 variant |
| Asset pack master sheet | about 30-50 gen | 16-30 small tiles |
| State variant chain | about 15 gen each | N states per base |
| animate_object | about 20-30 gen | 4-8 frames |

RIMA Phase 1 estimate:

- Naive per-piece approach: 24 walls x 30 = 720 gen.
- Master sheet approach: 1-2 sheets x 50 = about 100 gen.
- Multi-act re-theme: 3 themes x 30 = about 90 gen.
- Practical target: about 300 gen for all four acts of wall coverage after iteration buffer.

## 9. Production Recommendations

- Build Act 1 walls as one or two master sheets.
- Re-theme approved Act 1 sheets for later acts before generating unrelated variants.
- Use Create Image S-XL with init-lock for non-square wall features.
- Use create_map_object only for square, centered single props.
- Keep decorative identity in overlays where possible.
- Treat floor tilesets and wall tilesets as different production problems.
- Record the first passing prompt, settings, reference image, and output size for each asset family.

## 10. Future Research Questions

- Same Style Pro vs Edit Image Pro: which is better for variant generation?
- Init Image Strength sweet spot for wall expansion: 200, 300, or 400?
- animate_object quality: how stable are keyframes and interpolation?
- Can create_tiles_pro produce usable wall tiles despite being floor-oriented?
- Does Pixelorama integration provide better slider control through the desktop app?
- What is the best master sheet layout for a 16-tile modular wall pack?

## 11. Key Insights Index

1. Web UI Pro tools are the safest route for non-square production.
2. Create Image S-XL init images can lock output dimensions.
3. Init Strength 300 is the current baseline for balanced refinement.
4. Edit Image Pro is strong for whole-sheet transformation.
5. Master sheets reduce generation cost dramatically.
6. Multi-act re-theme should start from an approved Act 1 sheet.
7. create_map_object may clamp non-square requests to square output.
8. create_object was unavailable in this session and needs retest before use.
9. create_object-style grid prompts are not reliable for multi-item sheets.
10. Same Style Pro is useful after a strong style anchor exists.
11. Reference chains can drift if early assets are flawed.
12. Floor tileset tools should not be assumed to solve wall production.
13. Wireframe references can leak into final art.
14. Filled silhouettes are safer than wireframes for structure constraints.
15. Exact target-size init images reduce density mismatch.
16. AI-enhanced prompts can alter lore-specific details.
17. Proprietary names should stay out of generation prompts.
18. Familiar style anchors improve model interpretation.
19. Verbose prompts generally outperform terse prompts.
20. Palette hex codes help preserve color identity.
21. Decoration overlays give better reuse than baked-in variants.
22. Animations are best started from approved static sprites.
23. RIMA wall production should prioritize sheets over per-piece calls.
24. Each asset family needs saved prompt/settings metadata.
