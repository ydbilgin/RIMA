# PixelLab Object Strategy — RECONCILED (Antigravity + Sonnet + Opus) — 2026-05-25

## TL;DR (Opus reconcile verdict)

**Antigravity caught a critical constraint Sonnet missed** — `create_1_direction_object` view parameter has ONLY `top-down` or `sidescroller` (NO iso/oblique). For RIMA's dimetric layout (matching b340684f's iso projection), we MUST use `create_map_object` with `view="low top-down"`.

Sonnet's aspect-ratio analysis still partially valid but Antigravity's perspective constraint trumps it — credits are abundant (1389 remaining), use the right tool.

## Reconciled Strategy — ALL 5 P0 use `create_map_object`

| Object | Size | Tool | View | Mode | Credit |
|---|---|---|---|---|---|
| Cyan Archway | 96×128 | `create_map_object` | low top-down | basic (no bg) | 3 attempts = 3 |
| Monolithic Column | 64×128 | `create_map_object` | low top-down | basic | 3 attempts = 3 |
| Wall Torch Sconce | 32×64 | `create_map_object` | low top-down | basic | 3 attempts = 3 |
| Granite Altar | 96×64 | `create_map_object` | low top-down | basic | 3 attempts = 3 |
| Freestanding Brazier | 32×64 | `create_map_object` | low top-down | basic | 3 attempts = 3 |

**Total: 15 credits → 15 candidates (3 per object). Final selection: 2-3 per type after rubric filtering.**

### Why basic mode (no background_image) instead of bg style match?
- `create_map_object` has NO `style_images` parameter (unlike `create_1_direction_object`)
- `background_image` parameter does INPAINTING (oval 0.6) — places object ONTO background → composite PNG, NOT transparent prop
- Workaround would be `create_map_object` + `remove_asset_background` (2 calls per object × 5 = 10 extra credits)
- For first pass: use basic mode + explicit hex colors in prompt → transparent props, 1 call per attempt
- If basic mode palette drifts → switch to bg+remove pipeline

### Color palette in prompt (substitute for style_images)
Include this template in every prompt:
> "dark granite stone #3A3D42, glowing cyan accent #00FFCC, warm amber flame #FF8800 where flame present, dimetric isometric projection, pixel art, high contrast, single color black outline thickness 1"

## Consensus on other questions (Sonnet + Antigravity agree)

### Socket-paint vs Pre-gen — **Option B** (registry auto-select)
- `RoomPainterWindow.cs` line 27, 63, 165 — socket architecture LIVE
- Build `PropRegistry` ScriptableObject NOW (before generation)
- `TorchSocket → registry.GetProp(SocketType.Torch).random()`
- Roguelite palette swap (Granite → Sulphur biome) needs only registry remap, not layout rebuild

### Multi-candidate selection — DIVIDED
- **Sonnet structural screening:**
  - Silhouette readability at 24px painter cell
  - Vertical clearance (≤4 cells)
  - Palette anchor (#00FFCC presence for light/portal)
  - Transparency edges (no halo >2px)
- **Antigravity visual screening (in parallel after Sonnet pass):**
  - 3/4 dimetric angle correctness
  - Hand-drawn flame quality (no baked light halos)
  - Rune circle legibility on altars
  - Brick texture density match to b340684f

### Animation workflow — **n_frames + reference_image_base64** (NOT `animate_object`)
- `animate_object` documented for character-class only — non-character props produce undefined results
- For spike trap, pressure plate, chest open/close, portal pulse: generate static prop first, then request 4-6 frame sprite sheet via n_frames numbered list + ref image
- Honors `[[feedback-state-vs-n-frames-cost-lock]]` (state pipeline 4-8× more expensive)

## Selection criteria details

### For Wall Torch & Brazier (low priority for fire animation now, static first)
Keep:
- Flame: hot yellow center, amber-orange outer band (#FF8800), hand-drawn pixel shapes
- Iron bracket: solid dark grey, readable metallic contours, 3/4 depth visible
- Continuous 1px black outline
Reject:
- Soft glow halos / semi-transparent halos (Unity URP 2D lights handle glow)
- Flat side-view profile not sitting on diagonal walls

### For Monolithic Column
Keep:
- Top cap + bottom base: visible isometric diamond curvature
- Granite: #3A3D42 palette, high-contrast cracks
- Cyan runes: thin sharp #00FFCC lines (don't distort silhouette)
Reject:
- Flat top/bottom cuts (orthographic side view artifact)
- Bloated/squat proportions (square canvas distortion)

### For Granite Altar
Keep:
- Top surface: clean 26.5° dimetric projection
- Carved runes: centered, legible
Reject:
- Camera too high (straight down, hides rune face)
- Camera too flat (loses top surface visibility)

### For Cyan Archway (TOP priority focal)
Keep:
- Gothic arched stone frame with carved details
- Bright cyan energy fill (#00FFCC) inside the arch opening
- Dark cracked granite outer frame (#3A3D42)
- 3/4 dimetric angle (cyan fill projects forward/down)
Reject:
- Pure 2D flat archway with no depth
- Cyan accent missing or wrong hue (drifted to teal/blue/white)
- Frame too thin (loses focal weight)

## Pitfalls flagged (Antigravity)

1. **Oblique Camera Mismatch:** Don't fall back to `create_1_direction_object` for credit efficiency — it can't produce iso/oblique view, breaks visual alignment with iso floor.

2. **Baked Sprite Illumination:** PixelLab often generates fire assets with built-in light-glow halos (semi-transparent pixels). These look dusty on different background tiles. Specify "physical prop only" in prompt, add lighting via Unity URP 2D point lights.

3. **Canvas Stretching:** `create_1_direction_object` square padding distorts rectangular props. `create_map_object` non-square is the only correct path.

## Disagreements RESOLVED

| Issue | Sonnet | Antigravity | Winner | Why |
|---|---|---|---|---|
| Aspect ratio handling | Split (torch/brazier 1-dir, column/archway map_object) | All map_object | **Antigravity** | View angle constraint Sonnet missed |
| Style locking | style_images + prompt hex | bg_image + prompt hex | **Sonnet path technically more correct** — but create_map_object has no style_images! Use prompt hex only for first pass, or bg+remove if drift |
| Socket architecture | Option B (registry) | Option B (registry) | **Both — Option B** |
| Animation | n_frames + ref_image | (didn't address) | **Sonnet — n_frames** |
| Multi-candidate selection | Sonnet structural / Antigravity visual | (didn't address division) | **Sonnet's role-split** |

## Recommended dispatch for object generation

When user approves, dispatch:
1. **Codex Stream K** — implement PropRegistry ScriptableObject + bind to RoomPainterWindow socket subtype (no gen yet, just infrastructure)
2. **Opus direct PixelLab MCP calls** — 5 `create_map_object` × 3 attempts = 15 candidate sprites
3. **Sonnet structural triage** of 15 candidates → keep ~10 passing structural
4. **Antigravity vision triage** of 10 → keep ~6-8 final (1-2 per type)
5. **Codex Stream L** — download finals + import to Unity + populate PropRegistry + test render with new objects in combat scene
