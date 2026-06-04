# REPORT — Pixel-art: generate SMALL → scale in engine? (quality + algorithm)
Date: 2026-06-04 · Synthesis: cx (RIMA-specific) + ax-3.1-Pro (deep) + ax-3.5-Flash (lean) + Opus (web) → Opus
Brief: STAGING/PIXELART_SCALING_BRIEF_2026-06-04.md · Sources: web (ocias/colececil/Unity-docs) + 3 advisors

## VERDICT (all 4 agree)
**YES — generating small native pixel art and scaling in-engine IS quality-preserving (lossless, crisp) — under a strict contract:**
1. **Integer scale only** (2x/3x/4x). Non-integer (1.5x, 2.3x) = uneven pixel rows + **shimmer/crawl** on movement.
2. **Nearest-neighbor (Point) filter** — never Bilinear (blurs/muddies edges).
3. **No mipmaps, no compression** for pixel-critical assets (mip = blur on zoom; compression = color/edge artifacts).
4. **Pixel Perfect Camera** + integer pixel-ratio + pixel-grid snapping (kills sub-pixel shimmer).

The math: a 64px sprite at integer ×N maps 1 source pixel → exactly N×N screen pixels (grid intact). Non-integer splits a source pixel across a fractional screen area → broken grid.

## THE ALGORITHM / METHOD
- **Runtime (correct):** import at NATIVE res → render via **Point + Pixel Perfect Camera at integer scale** (+ Upscale Render Texture). You do NOT pre-upscale the asset file.
- **Offline upscalers (scale2x/EPX/hqx/xBRZ/AdvMAME):** edge-detect smoothers. **NOT for a coherent pixel game** (they destroy the pixel grid → "smooth vector/flash" look). Only for retro-porting style. RIMA: do NOT use.
- **Photoshop offline resize:** if ever needed, Nearest-Neighbor only (never bicubic).

## GENERATE-SMALL vs GENERATE-BIG (per tool)
- **PixelLab = native small (32/64px), TRUE pixel art** → ideal for sprites/tiles/icons/VFX. Immediately usable.
- **Imagen/imagegen = 1024² "pixel-STYLE"** (thousands of colors, anti-aliased, off-grid) → to become a real 64px sprite needs nearest-downscale + palette-snap + heavy manual cleanup (often slower than drawing from scratch). → Use ONLY for **backdrops/concept/abstract glyphs**, NOT active gameplay sprites.
- This matches RIMA's existing decisions (ASSET_PIPELINE + SPELLVFX_SKILLICON): characters=PixelLab; abstract glyphs/backdrops=imagegen+cleanup.

## ⚠️ RIMA-SPECIFIC NUANCE (cx — important correction to the Gemini advice)
"Generate small" does NOT mean switch CHARACTERS to a tight 32/64px canvas. RIMA canon (PROJECT_RULES) = **character canvas 120×120, ~64px visible body** (locked, [[project-character-64px-canvas-large-for-animation]]). Author at 120px / ~64px-visible — NOT a 1024 downscale, NOT an arbitrary 32px upscale. So: small ≠ tiny; small = the asset's CANONICAL native size.

## PER-ASSET NATIVE RES + TOOL (RIMA)
| Asset | Native res | Tool | Import |
|---|---|---|---|
| Character | 120×120 canvas (~64px visible) | PixelLab | PPU64, Point, no-mip, uncompressed, pivot feet |
| Tile (top-down) | 32×32 | PixelLab/hand | PPU64, Point, uncompressed; cell 0.5×0.5 (see drift below) |
| VFX | 64-128px | PixelLab (Phase-3)/code-particle (Phase-1) | PPU64, Point, keep_alpha for glow |
| Skill icon | 64×64 | PixelLab (no-text, 4×4 batch) | PPU64, **Point** (currently wrong, see fix) |
| Backdrop | 1024²/1920 | imagegen OK (the exception) | PPU64, Point; treat as backdrop art, not pixel-critical |

## ✅ RIMA ALREADY DOES THIS (mostly correct)
PPU64 locked · character metas Point/no-mip/uncompressed/PPU64 · Pixel Perfect Camera (assetsPPU 64) in UI + gameplay scenes · `CameraZoom.cs` snaps to integer pixel-ratio zoom (`NearestCrispZoom`, RequireComponent PixelPerfectCamera) = **exactly the correct runtime-zoom model** · room_bg backdrop imported Point/PPU64/uncompressed · `RIMAWallChainBuilderMenu` camera = upscaleRT+pixelSnapping true.

## 🔧 GAPS / HIGHEST-VALUE FIXES (cx found — actionable, NOT done yet)
1. **Skill icons** `Assets/Sprites/UI/Icons/*.png` are 64×64 BUT metas = **Bilinear filter + PPU100 + compressed** → blur when scaled. **FIX: Point + PPU64 + no-mip + uncompressed.** (Highest value — these are gameplay UI.)
2. **Gameplay PPC robustness:** `_IsoGame*` / `PlayableArena` PPC + `RoomRunDirector.ConfigurePixelPerfectCamera()` use **upscaleRT=0** (UI scenes use upscaleRT=1). For strict crispness align gameplay to upscaleRT=true.
3. **Tile/grid contract drift:** rule says 32px top-down (0.5×0.5 cell) but code/scenes use **iso cells 0.96×0.585** (`RoomConfig.IsoCellSize`, `RoomBuilder`, `_IsoGame`). Known iso-vs-topdown tension — needs an explicit re-decision (NOT a silent fix).
4. **Non-integer world scales** on some objects (CentralPortal 6.89/6.66, AutoCliff ~0.92, Atmosphere_Fog) — fine for soft FX/fog/light, BAD if ever on pixel-critical sprites. Audit before applying to sprites.
5. `character_select_bg` / `bg_seal_keep` = PPU100/compressed → OK as backdrop chrome, not pixel-critical.

## BOTTOM LINE
The Gemini advice (generate small → scale in engine) is **CORRECT in principle and already RIMA's approach.** The one correction: for characters "small" = the 120px/64px-visible canon, not 32px. The crispness recipe (Point + integer + Pixel-Perfect-Camera + no-mip/compression) is the whole game. Don't bother with xBRZ/hqx upscalers or pre-upscaling. **Quick win available now:** fix the skill-icon imports (Bilinear/PPU100/compressed → Point/PPU64/uncompressed).
