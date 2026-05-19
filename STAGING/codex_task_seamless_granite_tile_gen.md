# Codex Task — Truly Seamless Granite Ground Tile (BUILT-IN image_gen)

ACTIVE RULES: (1) think before generating (2) seamless edge mandatory (3) NO border/frame/outline (4) BLOCKED if unclear.

---

## CRITICAL MODE INSTRUCTION

Use the **built-in `image_gen` tool** (default mode per imagegen SKILL.md). DO NOT fall back to CLI `scripts/image_gen.py` — that path requires `OPENAI_API_KEY` which is NOT set in this environment. The built-in tool mode does NOT require any API key and is the preferred path per the skill spec.

If the built-in tool is unavailable for any reason, report BLOCKED — do NOT attempt CLI fallback.

## Mission

User core problem: current Tilemap floor shows visible cell boundaries because PixelLab tile assets have dark border baked in (PixelLab's "tile" semantic → framed object). Need ONE truly seamless ground texture that tiles edge-to-edge with NO visible cell boundary.

## Generation prompt (paste into built-in `image_gen` call)

```
A seamless top-down painterly cool granite ground texture, edge-to-edge tileable.

CRITICAL: NO border, NO frame, NO outline. Top edge pixels match bottom edge pixels. Left edge matches right edge. Continuous monolithic ground surface — NOT individual stones with gaps. NOT cobblestone. NOT brick. NOT mortar grid.

Painterly hand-drawn look. Muted cool gray with subtle blue-violet undertones (#3A3D42 to #4E5260). Hairline natural cracks sparsely scattered across the WHOLE surface (NOT one per tile-grid unit). Chunky 32-pixel-scale grain. Top-down perfectly flat view, no shading from any direction.

Worn ancient temple stone, monolithic, continuous surface. No tile boundaries. No grid pattern.

Style: 32px pixel art, painterly, no anti-aliasing artifacts.
Format: 1024x1024 PNG showing the seamless texture (will be downscaled and cropped for use as 32x32 tile).
```

## After generation succeeds

1. Built-in `image_gen` will save under `$CODEX_HOME/generated_images/...`
2. Copy/move the final PNG to: `Assets/Art/Tiles/F1/SeamlessV1/granite_seamless_01.png`
3. Verify the image:
   - Inspect with Pillow: does top row == bottom row pixel-wise? left col == right col?
   - Visually: is there any visible border at edges?
4. If seamless verification fails → iterate prompt 1 more time
5. If 2 attempts fail seamless test, save as PATCH tile (use 128x128 region instead of 32x32) and report partial pass

## Required output

`STAGING/CODEX_DONE_seamless_granite_tile.md`:

```
# STATUS
[LIVE / PARTIAL / BLOCKED]

# Generated file
[path to final PNG in Assets/]

# Seamless verification
[honest read — visible seam yes/no, pixel-edge match yes/no]

# Cost
[number of built-in image_gen calls — usually $0 user-side, $0 OPENAI_API_KEY]

# Next steps
[Unity import settings + how to swap into Tilemap]
```

## Hard rules

- **DO NOT** use CLI fallback (`scripts/image_gen.py`). It requires OPENAI_API_KEY which is unset.
- **DO** use built-in `image_gen` tool only.
- If built-in unavailable → BLOCKED, do NOT auto-fallback to CLI.
- Budget: 3 built-in image_gen calls max for iteration.
