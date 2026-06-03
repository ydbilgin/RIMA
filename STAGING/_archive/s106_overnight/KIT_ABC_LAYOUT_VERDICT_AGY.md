# Kit A+B+C Logical Layout Analysis — Antigravity (Gemini 3.5 Flash)

## 1. Scale
- **Kit A (Floor)**: The 62x39px visible diamond translates to a ~1.6:1 aspect ratio. At PPU=64, it is visually compatible with ~64px character sprites (chibi style, typically having a 24-32px feet footprint). Characters fit comfortably within the walkable diamond boundaries.
- **Kit B (Cliffs)**: Resizing to 96-128px wide x 144-192px tall is correct. A height of 144-192px (2.25 to 3.0 world units at PPU=64) gives the floating island a heavy, vertical presence, avoiding a flat cardboard appearance. A width of 128px (2.0 units) matches exactly two grid cells, making tiling seamless and rock formations feel chunky rather than repeating too frequently.
- **Kit C (Parallax BG)**: The 1280x720 viewport (20x11.25 units at PPU=64) is fully covered by the L2 ruins (1672x941 -> 26.1x14.7 units) and L0 void (19.6x19.6 units). Because player movement in a 12x8 cell arena is small, camera panning is minimal (~3-4 units), meaning the background layers easily cover the viewport without tiling seams or gaps.

## 2. PPU
- **Kit A**: PPU=64 (Locked).
- **Kit B**: Recommend **PPU=64** after downscaling raw refs to **128x192**. This preserves the 1:1 screen-to-texture pixel ratio, preventing "mixels" (mismatched pixel density) where the cliff face meets the floor tiles. Direct import of HD raw at PPU=512 would result in high-res textures clashing with the pixel-art floor.
- **Kit C**: Recommend **PPU=64** for L0, L1, L2, L4 to maintain a consistent scale factor. For **L3 Floating Islands**, import at **PPU=128** or **PPU=256** to naturally scale down these assets into distant floating backdrop elements (e.g., a 1254px island at PPU=256 occupies ~4.9 world units).

## 3. Pixelify
- **Kit B**: **PIXELIFY REQUIRED**. Since the cliff face is in direct contact with the pixel-art floor, mixing HD concept art with pixel-art floor tiles will look muddy and inconsistent. Downscale and pixelify via PixelLab Style Reference.
- **Kit C**: **KEEP AS-IS (HD / Painterly)**. Keeping the distant background elements soft-painted creates a natural depth-of-field effect (similar to Octopath Traveler). It visually separates the gameplay plane from the background void, reducing noise and focusing player attention on characters and hazards.

## 4. Layering
- **Tonal/Contrast Hierarchy**:
  1. *Foreground/Midground (Kit A Floor)*: Highest detail, highest contrast.
  2. *Frame (Kit B Cliffs)*: Medium contrast, anchoring the island.
  3. *Background (Kit C BG)*: Lowest contrast, dark/cool values to recede.
- **Depth separation**: The mockup successfully separates depth. L4 Fog must sit between L3 Islands and L2 Ruins to create atmosphere, and also overlay the bottom of Kit B cliffs to mask the hard edge where cliffs meet the void.
- **3-Point Cyan**: The cyan connection (Runes -> Cliff Glow -> Nebula) works but requires strict intensity mapping: Floor Runes (100% brightness, gameplay-critical) > Cliff Glow (50% brightness, ambient highlight) > BG Nebula (15% brightness, low-contrast void fill).

## 5. Cliff Placement
- **Perimeter Mapping**:
  - *South Edge (facing camera)*: Place cliff_S directly below the southern floor tiles. It must hang **below** the floor rim, sorted at sortingOrder = -10 to represent the vertical edge of the island.
  - *North Edge (facing away)*: Use cliff_N. Mostly hidden, only the top rim is visible.
  - *East/West & Corners*: Use cliff_E/W and transitional corner prefabs (cliff_SE, etc.).
- **Cyan Glow Variant**: Do **not** distribute randomly. Place cliff_cyan_glow exclusively underneath floor zones painted with RUNE_FOCAL or heavy STONE_CYAN_CRACK clusters to create logical narrative flow.

## 6. Sample Rooms
- **Combat Room (Elysium Outskirts)**:
  - *Size*: 14x10 cells.
  - *Cliffs*: Standard cliff_S / cliff_E / cliff_W. No glow.
  - *BG*: L0 Void + L2 Ruins A (scaled 1.0) + L4 Fog (10% alpha).
  - *Cyan*: LOW. STONE_BASE (95%), STONE_CYAN_CRACK (5% scattered), DIRT (0%).
- **Ritual Room (The Temple)**:
  - *Size*: 10x10 cells.
  - *Cliffs*: S/E/W cliffs with 1-2 cliff_cyan_glow on active corners.
  - *BG*: L0 Void + L1 Nebula + L2 Ruins B + L3 Islands Small (4-5 floating).
  - *Cyan*: MEDIUM. Central ring of 5 RUNE_FOCAL tiles, 10% STONE_CYAN_CRACK.
- **Pre-Boss Room (The Threshold)**:
  - *Size*: 12x12 stepped diamond.
  - *Cliffs*: Extensive cliff_cyan_glow flanking the central walkway.
  - *BG*: L0 Void + L1 Nebula (large) + L2 Ruins A/B + L3 Island Large + L4 Fog (heavy).
  - *Cyan*: HIGH. Walkway lined with RUNE_FOCAL, heavy STONE_CYAN_CRACK (30%).

## 7. Overlap Fix
- **Option B (Regenerate Kit A floor tiles with strict 2:1 dimetric 64x32 footprint)** is strongly recommended.
- **Rationale**: Option A (adjusting cellSize.y = 0.61) is a temporary Unity fix but introduces non-standard aspect ratios that will break future asset imports and complicate tile painter math. Option C scales down pixels, causing double-pixel errors. Option B preserves the pixel-perfect aesthetic and standardizes the project pipeline.

VERDICT: PIXELIFY-BC + REGEN-A. Pixelifying Kit B ensures visual cohesion with the floor, while keeping Kit C HD creates premium depth. Regenerating Kit A to 64x32 is necessary to maintain standard 2:1 isometric grid math.
