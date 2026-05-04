# PIXELLAB DISCORD SYNTHESIS -- MASTER REFERENCE DOCUMENT
# Last Updated: 2026-05-02
# Source: 6 Discord Channels, ~130 Screenshots, 10 Transcript Files

---

## TABLE OF CONTENTS
1. [Channel Coverage Map](#channel-coverage-map)
2. [API Endpoint Reference](#api-endpoint-reference)
3. [Tool Routing Matrix](#tool-routing-matrix)
4. [Character Generation Pipeline](#character-generation-pipeline)
5. [Animation Generation Pipeline](#animation-generation-pipeline)
6. [Tileset & Map Generation](#tileset--map-generation)
7. [Prompt Engineering Rules](#prompt-engineering-rules)
8. [Known Bugs & Workarounds](#known-bugs--workarounds)
9. [Community-Validated Workflows](#community-validated-workflows)
10. [Version History & Pricing](#version-history--pricing)
11. [External Tools & Resources](#external-tools--resources)
12. [Transcript File Index](#transcript-file-index)

---

## CHANNEL COVERAGE MAP

| Channel | Screenshots | Transcripts | Status |
|---------|------------|-------------|--------|
| `announcements` | 20 | `announcements_transcript_full.md` | COMPLETE |
| `api-and-sdk` | 20 | `api-and-sdk_transcript_1-2.md`, `_3.md`, `_4.md` | COMPLETE |
| `share-your-tips-and-tricks` | 40 | `tips-tricks_transcript_[1-4].md` | COMPLETE |
| `pixellab-art-gallery` | 20 | `art-gallery_transcript_[1-2].md` | COMPLETE |
| `mcp-and-vibe-coding` | 29 | `mcp-vibe_transcript_full.md` | COMPLETE |
| `help-questions-support` | 0 | N/A | NO DATA (no screenshots captured) |

---

## API ENDPOINT REFERENCE

### Primary Endpoints (V2 API -- ALWAYS USE V2)
| Endpoint | Purpose | Max Size | Cost | Notes |
|----------|---------|----------|------|-------|
| `/v2/generate-image-v2` | Single image generation | varies | varies | Use for concept art |
| `/v2/create-character-with-8-directions` | 8-dir character sprite sheet | 256x256 | Pro: 20-25 gens | Primary character tool |
| `/v2/generate-8-rotations-v2` | Rotate existing sprite to 8 dirs | 256x256 | varies | Retains items in right hand |
| `/v2/animate-with-text-v3` | Animate from reference image | 256x256 | 1-9 gens | USE V3 NOT V2 |
| `/v2/generate-ui-v2` | UI element generation | 64x64 | varies | Fixed as of 2026-04-08 |
| `/v2/create-tiles-pro` | Tileset generation | varies | 20-25 gens | Pro pricing |
| `/v2/objects/{id}` | Object metadata/retrieval | N/A | free | rotation_urls may return null (known bug) |

### API Documentation Sources
- **V2 API Docs:** `https://api.pixellab.ai/v2/docs`
- **MCP Docs:** `https://api.pixellab.ai/mcp/docs`
- **LLM Context File:** `https://api.pixellab.ai/v2/llms.txt` (recommended for agentic use)
- **V1 Docs (DEPRECATED):** `https://api.pixellab.ai/v1/docs` (missing many tools)

### Critical API Rules
1. **2-second minimum delay** between API calls (community consensus, prevents 429 errors)
2. **Polling timeout: minimum 10 minutes** -- animate-with-text-v3 can take 2-3 minutes; under load up to 8 minutes
3. **Custom proportions range: 0.5 to 2.0** -- values outside this return 422 error
4. **head_size recommended max: 1.7** (even though API accepts up to 2.0)
5. **Canvas auto-expansion:** 256x256 output = ~168x168 actual content + padding for animation
6. **V2 API is NOT officially released** but is the recommended path (will be official "soon")

---

## TOOL ROUTING MATRIX

### Character/Monster Generation
| Asset Type | Tool | Endpoint |
|-----------|------|----------|
| Humanoid character (8-dir) | `Rotations Pro` | `/v2/create-character-with-8-directions` |
| Monster/creature | `Create Image S-XL` | `/v2/generate-image-v2` (S-XL model) |
| Quadruped (experimental) | `Character Creator` | experimental support only |
| Character recolor | **In-game shader** | NOT via API -- use palette swap shader |

### Equipment/Item Generation
| Asset Type | Tool | Prompt Style |
|-----------|------|-------------|
| Weapons batch | `Create image from style reference (pro)` | Numbered list format |
| Armor sets | `Create image from style reference (pro)` | Row-based prompting |
| UI elements | `Create UI Elements` | Direct description |
| Objects/props | `Object Creator` | Via web: pixellab.ai/create-object |

### Animation Generation
| Animation Type | Tool | Notes |
|---------------|------|-------|
| Walk/run cycle | `Animate with Text v3` | 4-16 frames, up to 256x256 |
| Attack animations | `Animate with Text v3` | Generate base body first, add weapons second |
| Idle animations | `Animate with Text v3` | Use character's idle pose as reference |
| Equipment overlay | `Edit Image Pro` | Layer weapons/equipment on top of base animation |

### DEPRECATED TOOLS (DO NOT USE)
- **Bitforge** -- Legacy model, inferior quality. Use S-XL or Pro tools instead.
- **animate-with-text-v2** -- Known 50% stall bug. Use v3.
- **V1 API** -- Missing most tools. Use V2.
- **MCP alone** -- Missing Pro tools and latest features. Use API V2 directly.

---

## CHARACTER GENERATION PIPELINE

### Step 1: Base Character Creation
1. Use `/v2/create-character-with-8-directions` with `high top-down` view
2. Set canvas to 256x256 (auto-expands, actual content ~168x168)
3. Include strict prompt constraints (see Prompt Engineering Rules)
4. Save `character_id` from response -- THIS IS MANDATORY for consistency

### Step 2: Rotation Generation
1. Use `/v2/generate-8-rotations-v2` with the saved `character_id`
2. New rotation tool (v0.4.101) retains items in right hand and keeps pose
3. Alternative: Use Sjalsol's 3x3 grid prompt template for maximum determinism

### Step 3: Animation Base
1. Generate base animation body FIRST (no weapons/equipment)
2. Use `Animate with Text v3` with the character's idle/south sprite as reference
3. Select best extreme pose from output (Brian's workflow)

### Step 4: Equipment Overlay
1. Use `Edit Image Pro` to layer weapons/equipment onto animation frames
2. Maintain anchor points from base animation
3. Validate weapon position consistency across all directions

### Sjalsol's Deterministic 3x3 Grid Template
```
Generate a pixel art sprite sheet using the provided base sprite as the exact reference.
Keep the exact same design, proportions, colors, shading, outline, palette, pixel density, and scale.

SIZE LOCK:
Each frame must use identical canvas size as the base. No scaling, zooming, cropping.

FOOTPRINT LOCK:
All frames must share identical pixel extents (top, bottom, left, right). No overflow.

ANCHOR:
Feet must align to the same pixel row. Center and head height must match exactly.

Create an 8-direction turnaround sheet.
LAYOUT: 3x3 grid, center empty.
Top: back-left, back, back-right
Middle: left, (empty), right
Bottom: front-left, front, front-right

--- CHARACTER ---
TYPE: [hot-swap variable]
STYLE: [hot-swap variable]
HEAD: [hot-swap variable]
BODY: [hot-swap variable]
LIMBS: [hot-swap variable]
CLOTHING: [hot-swap variable]
HANDS: [hot-swap variable]
SILHOUETTE: [hot-swap variable]
COLOR: [hot-swap variable]

--- RULES ---
Clean pixel clusters, no dithering, no noise.
No anti-aliasing, no blur, no AA.
```

---

## ANIMATION GENERATION PIPELINE

### animate-with-text-v3 Specifications
- **Input:** Reference image (sprite) + text description of action
- **Output:** 4-16 frames depending on input size
  - 128x128 reference -> 4 frames
  - 32x32 / 64x64 reference -> 16 frames
  - Up to 256x256 supported (v3)
- **Cost:** 1-9 generations (v0.4.92+), was 40 generations in v0.4.69

### Brian's Run Cycle Workflow (Validated)
1. Give idle pose to "Animate with Text" with prompt: `running fast, alternating arms and legs`
2. From 16-frame output, select the most extreme pose (knee highest point)
3. Discard other frames
4. Horizontally flip the selected keyframe
5. Hand-draw interpolation frames between the two keyframes

### Kaninen's Mid-Pose Reference Method
- Instead of requesting full animation directly, create a "mid pose" reference first
- Feed mid-pose to "Edit Image Pro" to fill gaps between keyframes
- More controllable results than direct animation request

### True South Fix (Kibyra)
- For 256x256 sprites, add `fully visible top to bottom body` to prompt
- Without this, characters tend to face diagonal (SE/SW) instead of true South
- Critical for RIMA's 8-directional consistency

---

## TILESET & MAP GENERATION

### Tiles Tool (Pro) -- v0.4.89+
- Template-based tile generation
- Cost: 20 generations for <=256x256, 25 for <=341x341
- 16x16 tilesets NOT yet supported via `create_tileset` endpoint (Kaninen confirmed)

### YumYum's Tileset Blending Technique
1. Place target tile (e.g., snow) in center 32x32 area
2. Surround with 8 grass tiles
3. Feed to `Edit Image Pro`
4. AI naturally blends/harmonizes the tile transitions
5. Result: smooth biome transition tiles

### Map Editor
- Top-down mode (RIMA-relevant)
- Sidescroller mode added in v0.4.101 (not needed for RIMA)
- `create_tileset` API endpoint doesn't support direct map assembly yet

---

## PROMPT ENGINEERING RULES

### Numbered List Format (Items/Equipment)
```
Various weapons and armour copper material:
1. Sword
2. Spear
3. Mace
4. Axe
5. Dagger
```
- Load 11-16 style reference images
- Use "New frame" output method
- Enable "Remove background"
- Each number maps to one output frame

### Palette Control (force_colors)
```json
{
  "color_image": {"type": "base64", "base64": "...", "format": "png"},
  "force_colors": true
}
```
- Send a palette image as base64 PNG
- API was patched to accept both PNG and raw RGBA
- Include PALETTE SLOT MEANINGS and SKIN COLOR RULES in description

### Pixel Art Strict Rules (Tommy's Template)
```
PIXEL RENDERING RULES (STRICT):
- no anti-aliasing
- no smoothing
- no blended pixels
- no semi-transparent pixels
- no gradients, blur, glow, or filters
- hard pixel edges, discrete palette usage, clean pixel clusters

NEGATIVE PROMPT:
- no anti-aliasing
- no blur
- no soft shading
- no gradients
```

### Direction Lock
```
FRAMING:
- Final frame: 48x48
- Target visible character: ~24x44
- Leave ~2px padding top and bottom (10% vertical buffer)
- 4 directions only: south, north, east, west
- Exact same character across directions (no redesign)
```

---

## KNOWN BUGS & WORKAROUNDS

| Bug | Severity | Workaround | Status |
|-----|----------|-----------|--------|
| `animate-with-text-v2` stalls at 50% | HIGH | Use v3 endpoint instead | Known, unfixed |
| `rotation_urls` returns null for objects | MEDIUM | Download via web UI manually | Bug reported |
| `generate-ui-v2` immediate failures | FIXED | Patched 2026-04-08 | Resolved |
| `force_colors` + PNG base64 rejected | FIXED | API now accepts both PNG and RGBA | Resolved |
| Background gray on animate-v3 | LOW | Retry; `no_background` param inconsistent | Known |
| Jobs stuck at "initializing" | LOW | Contact PixelLab to clear stuck jobs | Server-side |
| `Create from Reference` broken | FIXED | Fixed same day (2026-03-30) | Resolved |
| Worker shortage incidents | TRANSIENT | Retry with backoff logic | Resolved |

---

## COMMUNITY-VALIDATED WORKFLOWS

### rorriM's End-to-End Automated Pipeline
- **Stack:** Claude + PixelLab MCP/REST API
- **Features:** Layering, anchor frames/pixels for rigging animation logic
- **Result:** Fully automated, no manual rigging, never opened Aseprite
- **Video:** https://youtu.be/PV-oXn8_EzM
- **Key insight:** `character_id` field is the backbone for multi-directional persistence

### Sjalsol's Deterministic Pipeline
- **Stack:** Aseprite plugin (prototyping) -> API pipeline (production)
- **Key innovation:** SIZE LOCK + FOOTPRINT LOCK + ANCHOR = consistent 3x3 sheets
- **Philosophy:** "Skills" (prompt templates) with hot-swappable variables
- **Preference:** 3x3 grid over 8-direction feature ("hit/miss on consistency")

### Strychnine's pixellab-forge-mcp
- **GitHub:** https://github.com/rabbitcannon/pixellab-forge-mcp
- **Purpose:** Custom MCP targeting V2 endpoints (official MCP was behind)
- **Also built:** Local LLM prompt enhancer proxy for better tool results
- **Also built:** Bugbot with feedback loop (find bugs -> fix -> add rulesets -> prevent recurrence)

### Ultimatefrisbie1's Agentic Sprite Workflow
- Fully agentic animated sprite generation with human review only
- Python script stitches individual PNGs into spritesheet
- Self-reflection via `lessons-learned.md` after each pipeline run
- Uses Godot MCP for engine integration

### MCP vs API Decision Tree
```
Need Pro tools? -> USE API V2
Need latest features? -> USE API V2
Need skeleton creation? -> USE API V2
Simple prototyping? -> MCP is OK for quick iteration
Need map generation? -> API only (not in MCP)
Need UI generation? -> API V2
Need animation? -> API V2 (animate-with-text-v3)
```

---

## VERSION HISTORY & PRICING

### Version Timeline
```
v0.4.101 (2026-04-25) -- Object Creator, Sidescroller, S-XL (32-512px), 8-Dir Rotation (new, retains items)
v0.4.92  (2026-03-16) -- Animate with Text (new): 4-16 frames, up to 256x256, 1-9 gens
v0.4.89  (~2026-03)   -- Tiles Tool (Pro), Pro pricing reduced (20/25 gens)
v0.4.84  (2026-01-16) -- Create UI Elements, ALL Pro tools added to API v2
v0.4.75  (2025-12-08) -- Inpainting v3, API: Image S-L + Consistent Style + Ref to 8 Dir
v0.4.71  (2025-11-28) -- Create image with 8 rotations (name pending)
v0.4.69  (2025-11-23) -- Animate with Text v1 (128x128 max, 40 gens), Tier limits updated
v0.4.xx  (2025-11-17) -- Create from Reference (Character Creator, bipedal south only)
v0.4.xx  (2025-11-08) -- Improved character model, experimental quadruped
```

### Tier Pricing (as of v0.4.69+)
| Tier | Generations | Notable |
|------|------------|---------|
| Tier 1 | 2,000 | Entry level |
| Tier 2 | 5,000 | Pixel Artisan |
| Tier 3 | 10,000 | Pixel Architect |

### Pro Tool Costs
| Operation | Size | Cost |
|-----------|------|------|
| Pro tools | <= 256x256 | 20 generations |
| Pro tools | <= 341x341 | 25 generations |
| Animate with Text (v0.4.92+) | any | 1-9 generations |
| Animate with Text (v0.4.69) | any | 40 generations |

---

## EXTERNAL TOOLS & RESOURCES

### Mandatory Pipeline Tools
| Tool | URL | Purpose |
|------|-----|---------|
| `make_aseprite` | (CLI tool) | Convert API exports to .aseprite files |
| `Aseprite CLI` | Compile from source | Free automated pipeline integration |
| `cleanEdge` | https://torcado.com/cleanEdge/ | Clean pixel art rotations (removes jaggies) |
| `Skeleton Pose Tool` | https://lysle.net/satoshi/skeleton | Generate 2D skeleton poses for reference images |

### Community MCP Alternatives
| Tool | GitHub | Notes |
|------|--------|-------|
| pixellab-forge-mcp | rabbitcannon/pixellab-forge-mcp | V2 endpoints, extensible |
| pixellab-mcp (Aseprite) | nekocon233/pixellab-mcp | Based on Aseprite extension, beta |

### Reference Videos
| Video | URL | Content |
|-------|-----|---------|
| rorriM's Automated Pipeline | https://youtu.be/PV-oXn8_EzM | End-to-end animation rigging with Claude |
| PixelLab Workshop LIVE | YouTube (PixelLab channel) | Tips, Workflows & Q&A with Nikola/PatricioStar |
| Stew's 100-Pose Spritesheet | YouTube (Stew's channel) | Skeleton tool -> PixelLab batch workflow |

---

## AGENTIC SAFETY RULES (from mcp-and-vibe-coding channel)

### HARD LESSONS FROM COMMUNITY
1. **NEVER leave agents unsupervised** -- One user's 200-file project became 261,231 C++ files overnight
2. **Git commit after every change** -- "My record was resetting 7 days of work back"
3. **Agents hallucinate bugs** -- Claude documented finding "hypothetical bugs" and fixing them, creating real bugs
4. **Use Skills/Guardrails over free-form agents** -- "You will get more guardrail alignments with skills over agents"
5. **Self-reflection pipeline** -- Every pipeline run should output a `lessons-learned.md` for future reference
6. **Prompt SHA manipulation** -- Change prompt strings slightly between sessions to avoid cached reasoning

---

## TRANSCRIPT FILE INDEX (FULL ABSOLUTE PATHS)

Detayli transcript dosyalarina ihtiyac duyarsan asagidaki tam yollardan eris:

### announcements (Version history, feature releases)
```
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\announcements\announcements_transcript_full.md
```
All 20 screenshots, PixelLab version history Nov 2025 - Apr 2026

### api-and-sdk (API endpoints, bugs, payloads)
```
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\api-and-sdk\api-and-sdk_transcript_1-2.md
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\api-and-sdk\api-and-sdk_transcript_3.md
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\api-and-sdk\api-and-sdk_transcript_4.md
```
Screenshots 001-010: rotation_urls, animate-v3, canvas expansion
Screenshots 011-015: timeouts, proportions, stalling bugs
Screenshots 016-020: force_colors, Tommy's payload, MCP vs V2

### share-your-tips-and-tricks (Workflows, prompt engineering, tools)
```
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\share-your-tips-and-tricks\tips-tricks_transcript_1.md
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\share-your-tips-and-tricks\tips-tricks_transcript_2.md
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\share-your-tips-and-tricks\tips-tricks_transcript_3.md
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\share-your-tips-and-tricks\tips-tricks_transcript_4.md
```
1: API throttle, endpoint routing, Bitforge deprecation
2: Equipment batch gen, numbered lists, row-based prompting
3: Brian's run cycle, Kaninen's mid-pose, cleanEdge shader
4: Tool hierarchy, community workflows, known limitations

### pixellab-art-gallery (Visual examples, validated pipelines)
```
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\pixellab-art-gallery\art-gallery_transcript_1.md
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\pixellab-art-gallery\art-gallery_transcript_2.md
```
1: Screenshots 001-010: animation workflows, S-XL model, prompts
2: Screenshots 011-020: character_id discovery, rorriM's pipeline

### mcp-and-vibe-coding (MCP vs API, agentic safety, Skills system)
```
F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\discord_analysis\mcp-and-vibe-coding\mcp-vibe_transcript_full.md
```
All 20 screenshots: MCP vs API debate, Skills/Guardrails, unsupervised agent disasters
