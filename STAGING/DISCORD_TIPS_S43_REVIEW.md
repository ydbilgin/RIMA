# Discord Tips -- S43 Production Review
<!-- 2026-05-02 -->

## Source
Screenshot: C:\Users\ydbil\Downloads\discord-tips-screenshot.png
Channel: #tips-and-tricks (PixelLab AI Discord)
Analysis: Gemini CLI, MEDIUM confidence

## Actionable Prompt Adaptations (MCP API)

### 1. Bracket-style descriptions
Old: `[item, weapon, sword]`
MCP: In the `description` field, lead with bracketed tags before prose.
Example: `"[isometric, floor tile, stone cobblestone] seamless top-down dungeon floor"`

### 2. Seed consistency for tilesets
Old: get seed via emoji reaction -> bot DMs job details; reuse --seed across variants
MCP: `create_tiles_pro` and `create_topdown_tileset` expose a `seed` parameter.
Workflow: generate base tile, note returned seed, pass same seed for all variants (wall, corner, edge).
This is already our standard -- confirm seed is logged in MEMORY/pixellab_sprites.md per run.

### 3. Isometric background tip
Old: `--isometric` flag + "transparent background" in prompt
MCP: `create_isometric_tile` handles isometric projection natively.
Add "transparent background" or "isolated on white" to `description` for easier sprite extraction.
For characters: "no background, isolated figure" reduces edge bleed at 128px.

### 4. Downscale workflow
Old: `--size 32x32` -> 512px output -> downscale nearest-neighbor in Aseprite
MCP: Our canvas is 128px (PPU=64). Output from PixelLab is already high-res.
Downscaling to 64px display size: use nearest-neighbor in Aseprite or Unity sprite importer (Filter Mode = Point).
No action needed -- already enforced by S43 pipeline.

### 5. Color count hint
Old: `--style pixelart --colors 32`
MCP: Add to `description`: "limited palette, 16 to 32 colors, pixel art, no anti-aliasing"
Useful for floor/wall tiles to keep visual consistency across tilesets.

## Non-Actionable (noted for reference)
- LoRA training: mentioned as upcoming Web UI feature, not available via MCP API.
  Watch PixelLab changelogs; when released, could enable style-locked character batches.
- --img reference URL: image-to-image is available via `image_reference_url` param in some endpoints.
  Already tracked in MEMORY/pixellab_sprites.md -- use for character consistency if variant drift occurs.

## Artwork Observed
- Pixel swords and RPG inventory UI items (16-32px range)
- Isometric wooden house buildings (multi-tile structures)
- Medieval knight characters (top-down/isometric)
- Fire explosion animation (sprite sheet, ~4-8 frames)
Style note: all observed assets skew toward classic RPG aesthetic -- compatible with Fractured Epic tone at higher color fidelity.

## Integration Recommendations
1. Add bracket-tag prefix to all S43 `description` strings this week (e.g., `[isometric, dungeon, stone wall]`).
2. Log seeds from every `create_tiles_pro` run in MEMORY/pixellab_sprites.md immediately after generation.
3. Append "limited palette, 32 colors max, no anti-aliasing" to tile descriptions to reduce palette sprawl across the dungeon tileset batch.
