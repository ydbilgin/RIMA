---
name: pixellab-tool-inventory
description: "Live MCP tool inventory for PixelLab (32 endpoints) — size limits, pro modes, review pipeline. Snapshot 2026-05-16."
metadata: 
  node_type: memory
  type: reference
  originSessionId: acfbcb3e-45ce-4896-b9be-0301b00dee90
---

# PixelLab MCP Tool Inventory (snapshot 2026-05-16)

Transport: HTTP, `https://api.pixellab.ai/mcp`, Bearer auth. Connected via `claude mcp add`. **Restart required** after first add.

## 32 endpoints — full list

### Characters (humanoid + quadruped)
- `create_character` — **mode: standard | pro**
  - standard: 4 or 8 dir, 1 generation, full style controls
  - pro: always 8 dir, **20-40 generations**, AI reference-based, IGNORES outline/shading/detail/ai_freedom/proportions
  - quadruped templates: bear, cat, dog, horse, lion (template= required)
  - **size: 16-128px** (canvas ~140% to fit animations)
  - view: low top-down | high top-down | side
- `create_character_state` — variant of existing char (sitting, armor, pose), shares group_id, all rotations consistent
- `animate_character` — template OR custom (action_description)
  - **Template** = 1 gen/dir (cheap). 50+ humanoid templates (walk variants, kicks, jumps, fireball, etc.)
  - **Custom** = 20-40 gen/dir. REQUIRES `confirm_cost=true` after user approval — NEVER set true on first call
- `get_character` / `list_characters` (tags filter) / `delete_character`

### Objects (general assets, decorations, items)
- `create_object` — 1 or 8 directions
  - **8-dir**: object you rotate around (~2-4 min)
  - **1-dir + n_frames in [1,4,16,64]**: consistent-style pack → enters **review status** for n>1
  - **size: 32-256px** ← 256px supported here
  - view: low/high top-down | side. object_view: top-down | sidescroller
  - state_of= groups variants
- `create_object_state` — variant of existing (add moss, golden, etc.), auto-wait 30s for source
- `animate_object` — 4-16 frames (even). For 8-dir pre-checks slot availability, refuses partial sets
- `select_object_frames(indices=[...])` — promote review candidates to individual completed objects
- `dismiss_review` — discard all review candidates
- `get_object` (inline preview of 16 review candidates) / `list_objects` (status_filter, tags) / `delete_object`

### Map Objects (inpainting + style match)
- `create_map_object` — game-map decoration with transparent bg
  - **width/height: 32-400px** ← 400px max (basic ≤160k area, inpainting ≤36k area = 192×192)
  - **Basic mode**: standalone gen from description
  - **Style Matching mode**: provide `background_image` (path= recommended, saves 5-20k tokens via curl), AI matches style
  - Inpainting: oval / rectangle / mask (custom base64), fraction 0.05-0.95, default oval 0.6
  - view: low/high top-down | side
- `get_map_object`

### Tiles & Tilesets

**Isometric (single):**
- `create_isometric_tile` — size **16-64px** (>24 better), tile_shape: thick tile | thin tile | block, view fixed isometric
- `get_isometric_tile` / `list_isometric_tiles` / `delete_isometric_tile`

**Topdown Wang tileset:**
- `create_topdown_tileset` — 16 or 23 tile pack (depending on transition_size), tile_size **16 or 32 only**
  - lower_description + upper_description + optional transition
  - Connected tilesets via `lower_base_tile_id` / `upper_base_tile_id`
  - ~100s generation
- `get_topdown_tileset` / `list_topdown_tilesets` / `delete_topdown_tileset`

**Sidescroller (platformer):**
- `create_sidescroller_tileset` — side-view, transparent bg, no slopes, tile_size **16 or 32**
- `get_sidescroller_tileset` (include_example_map) / `list_sidescroller_tilesets` / `delete_sidescroller_tileset`

**Tiles Pro (new generation tile system):**
- `create_tiles_pro` — hex / hex_pointy / **isometric** / octagon / **square_topdown**
  - **tile_size: 16-128px**, tile_height up to 256 (non-square)
  - **Shape mode** (default): tile_type + tile_view + tile_view_angle (0-90°)
  - **Style mode**: style_images=[{base64, width, height}, ...] → match existing tile style
  - `outline_mode: "segmentation"` = RED/BLUE zone, NO outline artifact (cleaner output)
  - Numbered prompt: "1). grass 2). dirt 3). stone" (count auto-computed)
- `get_tiles_pro` / `list_tiles_pro` / `delete_tiles_pro`

## Size Reality Check

| Need | Endpoint | Max |
|---|---|---|
| 256px object/decal | `create_object` | 256 ✅ |
| 256-400px map decoration | `create_map_object` | 400 ✅ |
| 256px tile height (non-square) | `create_tiles_pro` | 256 ✅ |
| 512px ANYTHING | **none** | ❌ — hard ceiling 400 |
| 256px character | **none** | ❌ — char max 128 |

## When to use which endpoint (RIMA scope LOCK 2026-05-16)

**`create_object` (PRIMARY image production path — user calls this "create image pro"):**
- **Always use for:** L3 wall sprites (perimeter cap), L4-L6 decals (moss, dirt crack, rubble, rift fracture, rift corruption), any 256px decoration
- **Recipe:** `directions=1, n_frames=16, view="high top-down"` → review pipeline → `select_object_frames(indices=[...])` keeps best picks
- **256px now possible** (was 128 max before) — use for hi-res walls/decals where Karar #143 painter needs detail
- **8-dir mode**: only when object truly rotates (rare for RIMA map work)

**`create_tiles_pro` (when tile autotiling needed):**
- Hex / isometric / square_topdown → Wang-style tile sets
- segmentation outline mode → no artifact
- **NOT used for L3-L6** (those are perimeter cap + decal, not Wang tiles per Karar #143)

**`create_map_object` (when inpainting into existing map):**
- Background_image + oval/rectangle mask → AI matches surrounding style
- Useful if Karar #143 painter needs spot-fix on existing rendered map
- Max 400px (basic) / 192px (inpainting)

**`create_character` mode=pro → DO NOT DISPATCH**
- User produces ALL characters via PixelLab web UI V3 manually with Claude-provided prompts
- See: [[pixellab-character-via-web-ui-v3]] feedback rule

**`animate_character` custom mode → use sparingly**
- 20-40 gen/dir cost gate — REQUIRES user confirm before `confirm_cost=true`
- Templates first (50+ available, 1 gen/dir)

## RIMA Sprint 3 production scope (S86 LOCK)

| Layer | Endpoint | Recipe | Sprite count | Credit |
|---|---|---|---|---|
| L3 wall (horiz/vert/corners/doorway) | `create_object` | dir=1, n=16, hi-top | 7 types × 14 pick total | ~21 |
| L4 moss decal | `create_object` | dir=1, n=16, hi-top | 1 type | ~3 |
| L5 dirt crack | `create_object` | dir=1, n=16, hi-top | 1 type | ~3 |
| L6 rubble + rift fracture + rift corruption | `create_object` | dir=1, n=16, hi-top | 3 types | ~9 |
| **Total** | — | — | **29 sprites** | **~36 credit** |

**NO `create_tiles_pro` dispatch in Sprint 3** — L3 is perimeter cap (not Wang), L4-L6 are decal overlays (not tiles).

## RIMA-Critical Patterns

**Consistent-style review pipeline** (NEW for L4-L6 decals):
1. `create_object(description=..., directions=1, n_frames=16, view="high top-down")`
2. `get_object` → inline 16 candidates
3. `select_object_frames(indices=[i,j,k,l])` → 4 separate 1-dir objects
4. `dismiss_review` if none worth keeping

**Style-match map editing** (NEW for Karar #143 painter):
1. `create_map_object(background_image={"type":"path","path":"..."}, inpainting={"type":"oval","fraction":0.4})`
2. Returns curl command, run locally → save token

**Pro character mode** (when standard mode quality insufficient):
- Always 8 dir, 20-40 generation cost, ignores most style fields
- Only useful for unique/detailed characters that standard template fails to capture
- Cost gate: warn user before dispatch

## See Also
- [[pixellab-knowledge-map]] — older Discord/docs scrape (path-only pointer, 17+ days)
- [[pixellab-create-character-workflow]] — Ref Standard, output=ref size, High Top-Down LOCK
- [[pixellab-v3-ui]] — V3 Custom Frames KF+Interp
- [[pixellab-create-modes]] — Pro vs Standard comparison (older)
- [[pixellab-direction-sequence]] — S, SW, W, NW, N, NE, E, SE
