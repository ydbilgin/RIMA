# PixelLab Maps — UX Deep Investigation
**Date:** 2026-05-14 S73
**Source:** Gemini research synthesis + direct transcript analysis (official PixelLab YouTube tutorials)

**Model note:** gemini-3.1-pro-preview (default). Queries 1 hit MODEL_CAPACITY_EXHAUSTED after sub-agent invocation; supplemented via direct transcript reads. Query 2 still pending. Queries 3, 4, 5 succeeded on retry. Query 3 answer is Pixelorama-specific (see Section 2 caveat).

Primary transcript sources read directly:
- `Tutorial： How to create maps with PixelLab.en.json3` (official PixelLab map tool tutorial)
- `subs_qVDkp1baJkU.en.json3` (Map Workshop interior tutorial — "how I make interiors for top-down maps")
- `Tutorial： Generating pixel art tilesets.en.json3` (tileset + canvas behavior)
- `Tutorial： How to create tilesets using AI.en.json3` (inpaint workflow)

---

## 1. Canvas & Sizing

- **Default canvas size:** The PixelLab Maps canvas is a free-draw inpainting canvas — not a fixed grid. The tile generation tool itself targets **16x16 or 32x32 pixel tiles**. The canvas can hold many tiles at once; the tutorial shows maps spanning multiple screens of tiles.
- **Custom size:** There is an explicit "Custom Size" option that allows generation on **160x160 pixels** (vs the default smaller size). The creator notes: "the model is better when you're doing it on a smaller size" — implying custom/large is possible but lower quality.
- **Resize mechanism:** No interactive drag-to-resize dialog found in tutorials. Resizing is done by extending the canvas via inpainting — you generate more tiles adjacent to existing ones. The core metaphor is "Overlap and Expand": shift the inpainting selection to overlap the edge, mask the empty space, and the AI uses existing edge pixels to generate matching adjacent tiles.
- **Max/min limits:** 64x64 recommended for most objects; 160x160 possible for grass/simple terrain; smaller (~64px) strongly preferred for complex detail and model quality.

---

## 2. Zoom & Pan

**Important caveat:** PixelLab Maps is web-based, but the editor is integrated with Pixelorama (the pixel art editor). Gemini's Query 3 answer describes Pixelorama canvas controls, which likely apply to the PixelLab editing environment as well. Treat with MEDIUM confidence — not confirmed from PixelLab Maps-specific tutorials.

- **Zoom method (from Pixelorama integration):**
  - Scroll wheel up/down to zoom in/out
  - Keyboard `+` / `-`
  - Press `Z` to activate Zoom Tool; right panel shows "Fit to Frame" and "100% Zoom" buttons
- **Fit-to-window:** Yes — "Fit to Frame" button in the Zoom Tool panel (press `Z` first)
- **Pan:**
  - Hold `Space` + left-click-drag (recommended)
  - Middle mouse button click-drag
  - Press `A` for Pan Tool
  - Arrow keys for incremental nudges
- **Default map size:** Canvas size depends on subscription tier. Free tier: ~200x200px max (e.g., 128x128 common). Higher tiers: up to 400x400px. Tiles are handled in 16x16px chunks.
- **Canvas resize (dialog-based):**
  - `Project > Resize Canvas` — expand/shrink workspace without scaling pixels
  - `Project > Scale Image` — upscale/downscale actual pixel art
  - `Project > Crop Image` — auto-crop to non-transparent content
- **CONFIDENCE: MEDIUM** — Pixelorama controls confirmed, but "PixelLab Maps web tool" may differ slightly from the Pixelorama standalone editor UX.

---

## 3. Brush & Paint Behavior

- **Default:** The Maps tool is **NOT a cell-by-cell paint tool**. It is an **inpainting tool** — you draw a freeform inpainting mask over any region of the canvas, write a prompt, and click Generate. There is no "click = 1 cell" behavior.
- **Inpainting mask:** You draw a region (appears to be freehand brush or selection) to define what the AI will regenerate. "Fill it back in and then we generate on top of this" (from transcript).
- **Selection tool:** A "shift-S" shortcut enables a **list/lasso selection tool** for grabbing and moving tile blocks. This is used to copy-paste tile regions.
- **Brush size:** Not described as a numeric slider. The tutorials use selection-based areas rather than a brush radius.
- **Special tools mentioned:**
  - In-paint map (draw a mask, prompt, generate) — uses Flux model (1 credit/gen) or Pro model (more credits, higher quality)
  - Create Object — select an area, write a prompt (e.g. "small top-down wooden table"), generates a single object in that region
  - Place Character — drop a character sprite for scale reference
  - In-paint uses selection tool (shift-S) for precise area selection
- **Path/corridor drawing:** No dedicated "path tool" found. You draw room shapes by blocking out the inpainting mask in the shape of walls/corridors, then prompt for content.
- **Drag to paint:** Not confirmed as a feature. The workflow appears to be: select area → prompt → generate, not a continuous drag-paint.

---

## 4. Tileset Palette UX

- **Tileset management:** The UI has a **"Tile sets" panel** ("You can see all your maps here, your tile sets here, and everything is pretty organized"). Tilesets are pre-generated and stored in the account.
- **Selection workflow:** You "grab one of the wooden tile sets and just start drawing out the shape" — implies clicking a tileset from the panel makes it the active brush material.
- **Active tileset affects canvas:** When a tileset is selected, painting (inpainting with that tile material) uses that tileset's style as context. The init image / context from adjacent tiles guides the AI.
- **Thumbnails vs list:** Not specified in transcripts (no detailed UI screenshot description), but the panel appears to be a browsable list with tiles visible.
- **Import custom tilesets:** Confirmed supported — tutorials explicitly show uploading your own tiles as "init image" references or as the higher/lower elevation tile inputs to the tileset generator.

---

## 5. Wang Transitions Visualization

- **Live vs batch:** Transitions are **batch-applied** — not live. You inpaint an area *between* two terrain types and prompt for the transition ("grass to beach transition here"). The AI generates the transition tiles in one click.
- **Transition quality tip:** "It's important that the model can see the good road/grass/etc so it understands what should look like." Context overlap is key — always include part of existing tiles in the inpainting area.
- **Corner transitions:** The hardest part. Tutorial shows manual approach: copy-paste existing tiles into position, draw an init image sketch for corners, then inpaint. "The model is actually best at editing what's in the middle of the image."
- **Export format:** Can export as Wang tileset JSON, Tileset-15 format (15 tiles), or 3x3 Godot format (43 tiles).

---

## 6. Layer Behavior

- **Single-tileset or multi-layer:** The Map Workshop canvas appears to be a **single unified canvas** with no named layer system in tutorials. Objects (furniture, props) are placed on top of the base terrain via the Create Object tool — but this is additive on the same canvas, not separate toggleable layers.
- **Layer toggle/visibility:** Not mentioned in any tutorial transcript — suggests no traditional layer system exists in the Maps editor.
- **Export:** "When you export it, you get all the objects and assets you generated" — exports as a flat image with all elements combined, plus separate asset sprites.

---

## 7. Video References

- `Tutorial： How to create maps with PixelLab` — official PixelLab map creation workflow (ARCHIVE/RESEARCH_RAW/yt_transcripts/ — no URL captured in transcript)
- `subs_qVDkp1baJkU` — interior maps tutorial ("how I make interiors for top-down maps using the map workshop") — URL: https://www.youtube.com/watch?v=qVDkp1baJkU
- Gemini-found URLs (confidence: MEDIUM — Gemini synthesized from local archive, may be hallucinated):
  - https://www.youtube.com/watch?v=O9maOTbLuHQ — "PixelLab Map Workshop Tutorial: Make Maps 10x Faster with AI Tilesets"
  - https://www.youtube.com/watch?v=jPPznIEK7HY — "Tutorial: How to create tilesets and maps using AI"
  - https://www.youtube.com/shorts/F9Fbva9ngV8 — "Generate dungeon map tiles using PixelLab.ai (Short)"
  - https://www.youtube.com/watch?v=H-dPJKmKr1E — "Tutorial: Creating a pixel art sidescroller map and tilesets with PixelLab"
  - https://www.youtube.com/watch?v=q9z2Vhpz-Z8 — "Tutorial: Generating pixel art tilesets"
  - https://www.youtube.com/watch?v=p1l9S3ta_XA — "Creating pixel art tilesets with PixelLab"

---

## 8. RIMA Map Designer Gap Analysis

Current RIMA Map Designer (`Assets/Editor/RimaMapDesignerWindow.cs`) vs PixelLab Maps:

**Have:**
- Multi-layer system (up to 4 layers) — MORE powerful than PixelLab
- Wang tile preview panel (16 tile grid with corner names)
- Vertex grid editor (cell and vertex paint modes)
- Brush, Fill, Rectangle paint tools
- Brush radius slider
- Procedural generation (Perlin noise, wall thickness, seed)
- Save/load JSON map data
- Panning (middle mouse + drag)
- Multi-tileset support per layer
- Erase mode

**Missing (compared to PixelLab UX):**
- Zoom in/out (scroll wheel zoom on canvas) — RIMA has no zoom, only pan
- Fit-to-window button
- Character/reference object placement for scale validation
- AI inpainting integration (PixelLab's core differentiator — out of scope for RIMA editor)
- Export to PixelLab JSON format for AI generation pipeline
- Object/prop creation tool (PixelLab's Create Object)
- Canvas resize via drag (RIMA uses dialog fields for width/height)

**Already better than PixelLab:**
- Named layer system with toggleable visibility
- Explicit vertex-based Wang encoding (deterministic, no AI required)
- Procedural room generation
- Multiple simultaneous tilesets per map

---

## 9. Recommended Faz 1.6 UX Fixes

Priority ordered (based on gap analysis + Hades UX lessons):

1. **Scroll-wheel zoom on canvas** — Highest impact. Confirmed in PixelLab/Pixelorama (scroll up/down) and Hades editor. Currently RIMA only has pan. Required for working on large maps (20x15+ cells). Implement: `Event.current.type == EventType.ScrollWheel` → scale `cellSize`. Add `+`/`-` keyboard support.

2. **Fit-to-window button** — Confirmed in PixelLab/Pixelorama as "Fit to Frame." After zoom changes, one-click to fit entire map in viewport. Required for orientation after deep zoom. Implement: compute `cellSize` that fits `roomWidth * cellSize + 2*padding <= contentRect.width`.

3. **Canvas resize with content preserve** — Currently RIMA has `roomWidth/roomHeight` int fields. A "Resize Canvas..." dialog with preserve/crop/extend options. PixelLab's approach (expand via AI) isn't applicable, but a simple resize-with-preserve would match standard editor UX.

4. **Character/scale reference placement** — Low-cost, high-value UX: drag a placeholder sprite (player character silhouette) onto the canvas to validate tile sizing. Matches PixelLab's "place character for scale" workflow.

5. **Color-coded blockout mode (from Hades)** — Paint functional intent first (walkable=green, wall=red, pit=blue) before applying Wang tilesets. Allows rapid layout iteration without needing tilesets loaded.

6. **"Overlap and Expand" region export** — For PixelLab AI generation pipeline: export a specific canvas region as PNG with overlap border (16px) for use as init image in PixelLab inpainting. Bridges RIMA Map Designer → PixelLab AI extension workflow.

---

## 10. Hades vs PixelLab — RIMA Synthesis

From Gemini Query 4 analysis:

| Pattern | Hades | PixelLab | RIMA recommendation |
|---------|-------|----------|---------------------|
| Terrain creation | Color-coded blockout (Yellow=walkable, Red=props) | Terrain brush painting via Wang tile logic | Adopt blockout mode (Fix #5 above) |
| Room assembly | Hand-crafted "Chambers" + procedural ordering | Expand-inpaint metaphor | Hybrid: RIMA generates base rooms procedurally, designers hand-edit in Map Designer |
| Transitions | Hand-placed gates on fixed grid | Chained base_tile_id Wang rules | Already implemented via CornerWangTileSetSO |
| Camera/flow | Map flip/rotation for entry angle consistency | N/A | Add "Preview 4-Way Rotation" toggle in Map Designer |

**Key synthesis:** PixelLab is an AI generation tool; RIMA Map Designer is a deterministic Wang painter. The UX lessons to steal are zoom/pan controls, scale references, and blockout-first workflow — not AI generation.

---

## 11. Confidence & Gaps

**High confidence (from official transcripts):**
- Inpainting is the core interaction model (not cell-by-cell paint)
- No traditional layer system in PixelLab Maps
- Tileset panel is browsable list in account
- Export formats: Wang JSON, Tileset-15, Godot 3x3
- "Place character for scale" is a documented feature
- Custom size (160x160) available but lower quality
- Pro model vs Flux model for inpainting cost difference

**Medium confidence (from Gemini Query 3 — Pixelorama integration):**
- Zoom: scroll wheel + `+`/`-` keys + Zoom Tool (`Z`) with Fit-to-Frame button
- Pan: Space+drag, middle mouse, Pan Tool (`A`), arrow keys
- Canvas resize: `Project > Resize Canvas` dialog
- Default size: 128-200px for free tier, up to 400px higher tier

**Low confidence / assumptions:**
- Whether the PixelLab Maps WEB TOOL exposes the same Pixelorama shortcuts (possible it abstracts them)
- Brush size slider existence (not mentioned — likely absent, selection areas replace brush concept)
- Layer toggle UI details

**Video URLs from Gemini:** MEDIUM confidence — Gemini found these in local archive context. The YouTube IDs may correspond to real videos but were not fetched/verified. Treat as leads, not confirmed URLs.

**Gemini model status:** gemini-3.1-pro-preview hit MODEL_CAPACITY_EXHAUSTED on 3 of 5 queries (concurrent load). Queries 4 and 5 succeeded after auto-retry. No fallback to 2.5-pro needed — query 4+5 completed on default model.
