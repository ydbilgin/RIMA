# Scene Composition: Kit B + Kit C Placement — Codex (xhigh)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Kit B pixelified cliff face sprites + Kit C HD parallax BG layers'ı PlayableArena sahnesine yerleştir. User henüz PixelLab production'a almayacak — önce mevcut refs ile sahne görselini görmek istiyor. Ortaya çıkan sahne agy + codex review'e gidecek, beğenirse PixelLab S-XL Pro Web UI'da init image olarak üretim. **NO autonomous PixelLab MCP gen** (user explicit halt).

## INPUTS

### Kit B pixelified (9 PNG, ready)
- Folder: `STAGING/s106_overnight/ref_kit_b_pixelified/`
- Files: `cliff_{S,N,E,W,NE,NW,SE,SW,cyan_glow}.png`
- All 128×192 RGBA, pixel art, transparent BG
- Tonal note: brighter/warmer than Kit A floor (per `KIT_B_PIXELIFY_REPORT.md`)

### Kit C HD refs (7 PNG, ready)
- Folder: `STAGING/s106_overnight/ref_kit_c/`
- Files:
  - `bg_L0_void.png` (1254×1254) — void base, opaque
  - `bg_L1_nebula.png` (1254×1254) — nebula gas, semi-transparent overlay
  - `bg_L2_ruins_A.png` (1672×941) — far ruins strip, opaque
  - `bg_L2_ruins_B.png` (1672×941) — far ruins strip variant
  - `bg_L3_island_small.png` (1254×1254 alpha) — small floating island
  - `bg_L3_island_large.png` (1254×1254 alpha) — large floating island
  - `bg_L4_fog.png` (1672×941 alpha) — atmospheric fog overlay

### Existing scene
- `Assets/Scenes/Test/PlayableArena.unity` (active, has painted floor on Tilemap GameObject)
- Grid cellSize=(1, 0.609375, 1), iso layout, PPU=64
- Camera: orthographic, position roughly centered on arena
- Verdict reference: `STAGING/s106_overnight/KIT_ABC_LAYOUT_VERDICT_AGY.md` + `KIT_ABC_LAYOUT_VERDICT_CODEX.md` (sorting/Z/parallax table)

## PRODUCTION SPEC

### 1. Import Kit B pixelified to Unity
- **Destination folder:** `Assets/Sprites/Environment/KitB_Cliff/`
- **Copy** all 9 PNGs from `STAGING/s106_overnight/ref_kit_b_pixelified/`
- **Texture import settings per asset:**
  - `textureType = Sprite (2D and UI)`
  - `spritePixelsPerUnit = 64` (final world size = 128/64 × 192/64 = 2×3 world units)
  - `spriteAlignment = SpriteAlignment.TopCenter` (Codex verdict — cliff hangs from top edge)
  - `filterMode = FilterMode.Point` (pixel art, no blur)
  - `mipmapEnabled = false`
  - `textureCompression = None` (or Low if needed)
  - `spriteImportMode = Single`
  - `wrapMode = Clamp`
- After import: `AssetDatabase.SaveAssets(); AssetDatabase.Refresh()`

### 2. Import Kit C HD to Unity
- **Destination folder:** `Assets/Sprites/Environment/KitC_BG/`
- **Copy** all 7 PNGs from `STAGING/s106_overnight/ref_kit_c/`
- **Texture import settings per asset** (per Codex verdict):

| Asset | PPU | World size (approx) | Filter | Compression |
|---|---|---|---|---|
| `bg_L0_void` | 32 | ~39×39 units | Bilinear | Normal |
| `bg_L1_nebula` | 32 | ~39×39 units | Bilinear | Normal |
| `bg_L2_ruins_A` | 32 | ~52×29 units | Bilinear | Normal |
| `bg_L2_ruins_B` | 32 | ~52×29 units | Bilinear | Normal |
| `bg_L3_island_small` | 64 | ~19.6×19.6 units (resize later) | Bilinear | Normal |
| `bg_L3_island_large` | 64 | ~19.6×19.6 units (resize later) | Bilinear | Normal |
| `bg_L4_fog` | 32 | ~52×29 units | Bilinear | Normal |

All: `spriteAlignment = Center`, `mipmapEnabled = true` (HD), `wrapMode = Clamp`

### 3. Sorting layers (verify/create)
Check `TagManager.asset` — ensure these sorting layers exist in order:
- `Ground` (back, sortingOrder use for Kit C)
- `Default` / `Floor` (Kit A tilemap, Kit B cliff)
- `Characters` (player/enemies, y-sorted)

If missing, create. Don't disturb existing layer order.

### 4. Create CliffRing in PlayableArena scene
- New empty GameObject `CliffRing` at world origin (or parent under existing Tilemap parent)
- For now: **fixed positions around the visible painted floor** (~12×8 cell visible footprint)
- The painted floor lives on the Tilemap; use `tilemap.cellBounds` to find extents, then place cliffs at outer edges
- **Configuration (24 sprites total):**
  - **South edge (visible, closest to camera):** 6× cliff_S at y = bottomY (just below floor rim), x evenly spaced across south edge width
  - **North edge:** 6× cliff_N at y = topY (just above floor top rim)
  - **East edge:** 4× cliff_E at right edge, y evenly spaced
  - **West edge:** 4× cliff_W at left edge, y evenly spaced
  - **4 corners:** cliff_NE / cliff_NW / cliff_SE / cliff_SW at respective corners
- Each cliff sprite:
  - Component: `SpriteRenderer`
  - `sortingLayerName = "Default"` (or whichever Floor's layer is)
  - `sortingOrder = -20` (per verdict — behind floor in same layer, since y-sort)
  - `transform.position` based on calculated rim positions
  - `transform.position.z = -0.05` (slight depth offset)

### 5. Create RoomBackgroundRig in PlayableArena scene
- New empty GameObject `RoomBackgroundRig` at world origin
- 5 child GameObjects, each with `SpriteRenderer`:

| Child | Sprite | Sorting Layer | Order | World Position | Notes |
|---|---|---|---|---|---|
| L0_Void | bg_L0_void | Ground | -500 | (0, 0, +20) | Fill full view (already huge at PPU 32) |
| L1_Nebula | bg_L1_nebula | Ground | -430 | (0, 0, +16) | Slight upward offset for nebula band |
| L2_Ruins | bg_L2_ruins_A | Ground | -380 | (0, 3, +12) | Above floor rim, distant horizon |
| L3_Islands | bg_L3_island_small + large | Ground | -320 | (-8, 5, +8) and (10, 4, +8) | 2 islands, art-direction placement |
| L4_Fog | bg_L4_fog | Ground | -260 | (0, -2, +5) | Lower band, masks cliff bottoms |

L3: create 2 child SpriteRenderers (small + large) at different positions; resize transform scale if needed (e.g. small island scale (0.4, 0.4, 1), large scale (0.7, 0.7, 1))

For now: NO ParallaxLayer.cs (skip parallax math — that's a later step). Just static placement to verify visual.

### 6. Camera adjust (if needed)
- Current camera orthographic size should be ~5 (per Codex verdict)
- Position roughly (0, -0.35, -10) for slight upward tilt to see south cliff
- Don't disturb existing camera if it's already close to this

### 7. Take screenshots
- Save Scene first: `EditorSceneManager.SaveOpenScenes()`
- Take 4 screenshots:
  - `STAGING/s106_overnight/scene_v1_game.png` — Game view, 1280×720
  - `STAGING/s106_overnight/scene_v1_scene.png` — Scene view (current zoom)
  - `STAGING/s106_overnight/scene_v1_scene_topdown.png` — Scene view, framed on Floor with orthographic top-down (use scene_view_frame)
  - `STAGING/s106_overnight/scene_v1_hierarchy.png` — Optional, hierarchy panel view (skip if not feasible)

### 8. Done report
Write `STAGING/s106_overnight/SCENE_COMPOSITION_V1_REPORT.md`:
- List of imported assets (paths + PPU + pivot)
- CliffRing child count + positions
- RoomBackgroundRig child count + sorting
- Console errors/warnings (must be 0)
- Screenshot paths
- Any tonal/visual issues you noticed (be honest — Kit B too bright? Kit C clashing? etc.)
- Next steps recommendation (if any)

## CONSTRAINTS
- **NO PixelLab calls** (autonomous gen halted by user)
- **NO ParallaxLayer.cs** for now (later step after visual verification)
- **Single scene edit pass** — don't iterate ad-hoc
- Preserve existing painted floor (don't touch Tilemap tiles)
- Preserve player + light setup
- 0 error 0 warning required

## TIME ESTIMATE
~45-60 min at xhigh.

## DELIVERABLE
- 16 imported sprite assets (9 Kit B + 7 Kit C)
- 2 new scene GameObjects (CliffRing + RoomBackgroundRig) with appropriate children
- 3-4 screenshots
- 1 markdown report
- Final `CODEX_DONE_<profile>.md` with `STATUS: DONE`
