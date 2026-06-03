ACTIVE RULES: (1) think before answering (2) cite specific PixelLab parameter names (3) compare against chatgpt_ref visually (4) BLOCKED if Phase 0 incomplete.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

**ANTIGRAVITY CRITICAL:** Respond INLINE only. Do NOT use file-write tools.

Amaç: User soruyor "chatgpt_ref tarzı 35° iso dungeon floor tile'lar için PixelLab'da hangi parametrelerle generate yapmalıyım". Gece önceki gen'lerimiz 90° top-down olarak yapılmıştı, oda visual'ı dümdüz çıktı. Bu kez SAĞDOĞRUDAN parametrelerle gen yapmadan önce sen + Codex (paralel) en doğru parameter setini söyle. Çıktıdan sonra ben sentezleyip generate'i tetikleyeceğim.

---

# PIXELLAB PARAMETER ADVICE — Iso Floor Tile for chatgpt_ref look

## ⚠️ Phase 0 — INTERNALIZE chatgpt_ref intent (MANDATORY 250-350 words at top)

Look at these images and describe in your own words what the floor LOOKS LIKE:
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (1).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (2).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (3).png`

Specifically:
- What iso angle? (15°? 30°? 35°? 45°?)
- Floor tile thickness (thin / medium / thick)?
- Brick/stone size relative to character (1 char = how many tiles)?
- Color palette (warm/cool, value range)?
- Detail level — cracks, debris, magical glow, edges?
- Tile-to-tile transitions — clean seams or organic blend?
- Lighting baked into tile (top-lit, side-lit, ambient)?

THEN compare to our existing `Assets/Sprites/AssetPackV3/floor/tile_0.png` and explain WHY ours doesn't match.

---

## Phase 1 — PixelLab tool parameter recommendations

PixelLab offers these floor-relevant tools. Tell which to use AND with what params:

### Tool A: `create_isometric_tile`
- description: <what string?>
- size: 16-64 (recommend?)
- tile_shape: thick / thin / block (recommend?)
- detail: low/medium/highly detailed (recommend?)
- outline: single-color / selective / lineless (recommend?)
- shading: flat/basic/medium/detailed/highly detailed (recommend?)
- text_guidance_scale: 1-20 (recommend?)

### Tool B: `create_topdown_tileset` (Wang autotile, 16 tiles output)
- lower_description / upper_description (what terrains?)
- transition_size (0/0.25/0.5/1.0)
- transition_description
- tile_size (16 or 32 px)
- detail / outline / shading (same options)
- view: "low top-down" or "high top-down" — which matches chatgpt_ref's iso feel?
- tile_strength: 0.1-2.0 (consistency)
- tileset_adherence: 0-500
- tileset_adherence_freedom: 0-900

### Tool C: `create_tiles_pro` (numbered tile variants)
- description: numbered list "1). X 2). Y..."
- tile_type: hex / hex_pointy / **isometric** / octagon / square_topdown — which for 35° iso?
- tile_size: 16-128
- tile_view: top-down / **high top-down** / **low top-down** / side
- tile_view_angle: 0-90 (continuous, 0=side, 90=topdown) — what value for chatgpt_ref's iso? 35°? 45°? 60°?
- tile_depth_ratio: 0-1.0 (controls vertical depth)
- tile_height: optional explicit pixel height
- outline_mode: outline vs segmentation
- style_images: optional style reference
- style_options: color_palette/outline/detail/shading copy

## Phase 2 — Specific RECOMMENDED CALL

Give the EXACT call I should make. Concrete values, not ranges. Pick ONE tool and write the full parameter list.

Example output format:
```
RECOMMENDED CALL:
mcp__pixellab__create_tiles_pro(
  description="1). dark granite dungeon floor cracked 2). worn stone with moss 3). debris and rubble 4). intact granite tile 5). dripping cracked stone with cyan rift glow",
  tile_type="isometric",
  tile_size=64,
  tile_view="low top-down",  
  tile_view_angle=35,         # <-- chatgpt_ref iso match
  tile_depth_ratio=0.3,
  outline_mode="segmentation",  # cleaner pixels
  seed=4123
)
```

Justify each parameter choice with reference to chatgpt_ref visual or industry standard.

## Phase 3 — Validation gates

After PixelLab returns the generated tiles, what should I CHECK before importing to Unity?
- Pixel size / PPU consistency
- Iso angle (35° measured)
- Tile-to-tile seamless test
- Sort/depth correctness
- Color palette match
- Resolution
- Tile thickness/depth ratio

List 5-8 verification checkpoints with specific measurements.

## Phase 4 — Comparison with existing PixelLab inventory

I have these existing iso/Wang tilesets that MIGHT work without new generation:
- `b340684f` — dark granite cobblestone dungeon isometric 64px 16var
- `e61f9c7b` — pure granite (corner p... isometric 64px 16var
- 4 isometric_tile single tiles (granite floor variants)

Should I:
- (A) USE one of these existing tiles directly?
- (B) Generate NEW with style_images referencing one of these?
- (C) Generate FRESH with my recommended params (your Phase 2 answer)?

Pick ONE and justify.

---

## Output format (INLINE only, ~600-800 words total)

```markdown
# PixelLab Parameter Advice — <Codex profile / Antigravity account> — 2026-05-25

## Phase 0 — chatgpt_ref intent (300 words)
<your description>

## Phase 1 — Tool parameter recommendations
### Tool A (create_isometric_tile): <recommend or skip>
### Tool B (create_topdown_tileset): <recommend or skip>
### Tool C (create_tiles_pro): <recommend or skip>

## Phase 2 — RECOMMENDED CALL
<exact tool + params, justified>

## Phase 3 — Validation gates (5-8 checkpoints)
1. ...

## Phase 4 — Reuse existing vs generate new
<A/B/C verdict + reason>
```

## Constraints
- Be SPECIFIC — angle in degrees, size in pixels, exact tile_type
- Cite chatgpt_ref pixel-level details
- If you're not sure, say so (e.g. "best guess 35° but could be 30-40°")

## Estimated time: 15-20 min
