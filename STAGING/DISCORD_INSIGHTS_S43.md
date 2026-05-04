# PixelLab Discord + Community Insights — S43 Production
Source: mcp-and-vibe-coding (366KB), share-tips-tricks (22KB), sjalsol screenshots (2026-05-01)

---

## CRITICAL: 8-Direction Feature — Avoid

**Confirmed by sjalsol (power user) and multiple community members:**
PixelLab's built-in `n_directions=8` / 8-direction MCP tool is "hit/miss on consistency."

**Better approach (sjalsol's validated method):**
- Generate one canonical reference sprite
- Use `/v2/generate-8-rotations-v2` endpoint: feed single sprite, get 8 directional outputs
- Or: custom 3x3 grid prompt with explicit structure (see template below)

This aligns with RIMA's existing char_id anchor approach.

---

## sjalsol Prompt Template (validated, produces consistent 3x3 sheets)

```
Generate a pixel art sprite sheet using the provided base sprite as the exact reference.
Keep the exact same design, proportions, colors, shading, outline, palette, pixel density, and scale.
Do not redesign, restyle, or add detail.

SIZE LOCK:
Each frame must use identical canvas size as the base.
No scaling, zooming, cropping, or resizing.

FOOTPRINT LOCK:
All frames must share identical pixel extents (top, bottom, left, right).
No overflow beyond original bounds.

ANCHOR:
Feet must align to the same pixel row.
Center and head height must match exactly.

Create an 8-direction turnaround sheet.
Layout: 3x3 grid, center empty.
Top: back-left, back, back-right
Middle: left, (empty), right
Bottom: front-left, front, front-right

Each sprite: Centered, same scale, same proportions. Same character rotated, not redesigned.

---- CHARACTER ----
[TYPE / STYLE / HEAD / BODY / LIMBS / EXTRA / CLOTHING / HANDS / SILHOUETTE / COLOR]

---- RULES ----
Use the provided sprite only as scale, grid layout, cell placement, and pixel density reference.
[Replace base design with target character description]
No props, weapons, or tools.
Clean pixel clusters, no dithering, no noise.
```

**Key:** Character section is a hot-swappable variable. Skeleton prompt stays fixed.

---

## API: Confirmed Working Endpoints (v2, as of April 2026)

```
GET  /v2/balance                     -> 200
GET  /v2/llms.txt                    -> 200 (LLM-readable API summary)
POST /v2/animate-with-text           -> 200 (sync)
POST /v2/animate-with-text-v2        -> 202 (async)
POST /v2/edit-animation-v2           -> 202
POST /v2/interpolation-v2            -> 202
POST /v2/transfer-outfit-v2          -> 202
POST /v1/rotate                      -> 200
POST /v2/generate-8-rotations-v2     -> WORKS (key endpoint for RIMA)
```

Auth: `Authorization: Bearer <SECRET>` (Secret from pixellab.ai/mcp page)

---

## API: Known Broken / Unreliable (as of March-April 2026)

```
POST /v1/estimate-skeleton    -> 500 ("Keypoint lists must have the same length")
POST /v2/estimate-skeleton    -> 500
POST /v1/animate-with-skeleton -> 500
POST /v2/animate-with-text-v3  -> 500 on REST; workaround: Python SDK
```

---

## MCP: What Is and Is NOT Exposed

**In MCP:** create_character, animate_character, create_map_object, create_isometric_tile, create_tiles_pro, create_topdown_tileset, create_sidescroller_tileset + get/list/delete variants.

**NOT in MCP (API-only):**
- edit-images-v2 / pro edit
- generate-with-style-pro
- generate-8-rotations-v2
- Pro character creator (style reference / concept image input)
- generate-ui-pro, generate-image-pro

**RIMA implication:** For style-locked consistent chars (our requirement), use API directly. `concept_image` parameter is API-only.

---

## Pipeline Rules (community-validated)

1. **One sprite at a time for animation** — batch input to estimate-skeleton returns 500.
2. **No spritesheet export** — PixelLab returns individual PNGs. Build Python stitching script.
3. **API outputs invisible on web** — manage all outputs programmatically with IDs.
4. **V2 over V1** — v1 is sparse. All important endpoints are v2.
5. **White pixel artifacts** in animation are common — build per-frame QC into pipeline.

---

## Skills > MCP for Long Sessions

Replacing MCP with Claude Code skills (REST calls wrapped in .md skill files) "clears the context significantly." MCP tool list adds tokens to every turn. For RIMA's multi-class batch sessions: consider skills-based approach.

Community reference: `github.com/rabbitcannon/pixellab-forge-mcp` — community MCP targeting v2 endpoints the official MCP doesn't expose.

---

## Animation Limitations (confirmed by Kaninen / PixelLab dev)

Animations that will fail or produce poor results due to insufficient training data:
- Wall-climbing
- Wall-rappelling
- Air-descending / air-ascending
- Cannonball poses
- Superman-fly

**RIMA impact:** aerial skill animations (Brawler aerial_rave, flying_knee) may need manual frame-by-frame work or simplified keyframes.

**Tip (from Kaninen):** For animation: do not start from idle frame. Start from a mid-animation frame. Use "Animate with text (Pro)" > "New". Use edit-image-pro + interpolate-new + prompt-enhance combo for best results.

---

## Recommended RIMA Production Flow Update

Old: generate sprite -> MCP 8-dir -> animate
New: generate canonical sprite -> /v2/generate-8-rotations-v2 -> animate per direction (one at a time) -> Python stitch -> QC

Prompt structure: sjalsol skeleton template + RIMA character variable block (from PRODUCTION_GUIDE_S43)
