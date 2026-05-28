# Kit A+B+C Logical Layout Analysis — Antigravity (Gemini 3 Pro)

ACTIVE RULES: (1) think before answering (2) net, no waffle (3) visual+game design lens (4) BLOCKED if can't be concrete.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Kit A (floor) + Kit B (cliff face) + Kit C (parallax BG) referans assetleri logical bir araya geliyor mu? Doğru ölçek, doğru yer, doğru tonal hierarchy? Ben (Opus) sentezleyeceğim, Codex paralel mühendislik perspektifi verecek.

## RIMA TECHNICAL CONTEXT
- Unity 6, URP 2D Renderer + Pixel Perfect Camera + 2D Lights, PPU=64
- Camera: high top-down 3/4, ~70-80°, Hades/Children of Morta tier
- Kit A floor tiles: 64×64 PNG, PPU=64, iso CellLayout, Grid cellSize=(1, 0.5, 1)
- **DISCOVERED BUG:** Kit A visible diamond is 62×39 px (NOT proper 64×32 dimetric 2:1) → vertically adjacent tiles overlap ~7px. See `STAGING/s106_overnight/iso_overlap_test.png` for visual proof.
- Camera target arena visible size: ~12×8 cells = 768×256 game pixels at PPU 64 → roughly 12×4 world units footprint
- 3-Kit BG architecture LOCKED (see `STAGING/BG_LAYER_ARCHITECTURE_VERDICT.md`):
  - Kit A: floor Tilemap, sorting 0
  - Kit B: cliff face hanging stone, sorting -10
  - Kit C: parallax BG, sorting -300 to -500

## ASSETS (all under `STAGING/s106_overnight/`)

### Kit A floor (existing, USE AS-IS this analysis)
- 16 PixelLab iso tiles, 64×64 PNG, transparent diamond
- Categories: STONE(0-3), CYAN_CRACK(4-6), DIRT(7-10), RUNE(11-15)
- Visible diamond: ~62×39 px (NOT 2:1 dimetric — diamond is "taller")

### Kit B cliff face (9 refs, ALL 1024×1536 HD)
- `ref_kit_b/cliff_N.png` (back face)
- `ref_kit_b/cliff_S.png` (front face — most visible, MAIN)
- `ref_kit_b/cliff_E.png`, `cliff_W.png` (side faces)
- `ref_kit_b/cliff_NE.png`, `NW.png`, `SE.png`, `SW.png` (corner faces)
- `ref_kit_b/cliff_cyan_glow.png` (energy seam variant, overlay)

### Kit C parallax BG (7 refs)
- `ref_kit_c/bg_L0_void.png` (1254×1254) — deep void base
- `ref_kit_c/bg_L1_nebula.png` (1254×1254) — purple/magenta nebula
- `ref_kit_c/bg_L2_ruins_A.png`, `B.png` (1672×941) — far ruins wide strips
- `ref_kit_c/bg_L3_island_small.png`, `large.png` (1254×1254, alpha) — floating islands
- `ref_kit_c/bg_L4_fog.png` (1672×941, alpha) — atmospheric fog overlay

### Reference mockup
- `STAGING/s106_overnight/kit_abc_logical_mockup.png` — Python composite, 1280×720, all kits layered

## ANALYZE THIS MOCKUP

Read `kit_abc_logical_mockup.png` and the 3 kit ref folders. Answer concrete:

### 1. SCALE — does it work?
- Kit A iso tile (62×39 visible diamond) — okay for ~64px characters?
- Kit B cliff face: in mockup, resized to 96-128 wide × 144-192 tall. Is this the RIGHT scale relative to floor cells?
- Kit C BG: at 1280×720 viewport, full-fill BG seems okay. Will it tile / parallax cleanly?

### 2. PPU (target import setting per kit)
- Kit A: PPU=64 (locked, working)
- Kit B: What PPU should we import at? Options:
  - HD raw 1024×1536 @ PPU=1024 → 1 unit per cliff (too big for 1-cell-wide cliff)
  - Downscale to 128×192 first, then PPU=64 → 2×3 unit per cliff (matches mockup)
  - Downscale to 96×144 + PPU=64 → 1.5×2.25 unit per cliff
- Kit C: HD layers — PPU=1 (super low → fill viewport)? PPU=64? Recommend.

### 3. PIXELIFY needed?
- Kit B/C are HD ChatGPT-style refs, NOT pixel art. Kit A is true pixel art.
- Mixing HD + pixel = "muddy" look risk. OR — HD parallax BG is intentional artistic choice (e.g., Octopath Traveler style HD background + pixel sprites).
- Verdict: pixelify Kit B/C through PixelLab Style Reference (init image + AI Freedom=0) before Unity? OR use as-is HD?

### 4. LAYERING — does the mockup respect depth?
- Tonal hierarchy: BG (Kit C) should READ flat → mid (Kit A floor) MOST DETAILED → cliff (Kit B) frames arena
- Mockup currently: do tones separate cleanly? Do islands "float" properly behind arena? Does fog mask cliff base?
- Color: cyan rift visible at floor (rune) + cliff (glow) + BG (nebula). 3-point cyan working or too much?

### 5. CLIFF FACE LOGICAL PLACEMENT
- Top-down camera looking at floating arena → cliff is the VERTICAL SIDE of the floating island
- Arena perimeter → which cliff direction goes where:
  - South edge (closest to camera) → cliff_S
  - North edge (furthest, behind) → cliff_N (mostly hidden)
  - East/West edges → cliff_E/W
  - Corners → cliff_NE/NW/SE/SW
- In mockup: are we using correctly? Should cliff_S hang BELOW the arena bottom rim (yes) or behind it?
- Glow placement: under cyan rune clusters in arena? Random?

### 6. SAMPLE ROOMS — 3 variations
For COMBAT / RITUAL / PRE-BOSS rooms, sketch (text):
- Arena floor size (cells)
- Cliff perimeter placement
- BG layer combination (which Kit C images, which scaled how)
- Cyan density mapping (Combat LOW / Ritual MED / Pre-boss RISING per existing rule)

### 7. OVERLAP FIX
Given Kit A diamond is 62×39 (not 64×32), 3 fix paths:
- A) Grid cellSize.y = 0.61 (from 0.5) — quick, non-standard iso
- B) Regenerate Kit A floor tiles with strict 2:1 dimetric (64×32 footprint)
- C) Crop/scale existing tiles to 64×32 (loses detail)

Recommend with rationale.

## OUTPUT FORMAT
- Max 800 words
- Section headers: 1. Scale | 2. PPU | 3. Pixelify | 4. Layering | 5. Cliff Placement | 6. Sample Rooms | 7. Overlap Fix
- Final line: `VERDICT: <USE-AS-IS | PIXELIFY-BC | REGEN-A | OTHER>` + 1 sentence rationale
- Write your analysis to `STAGING/s106_overnight/KIT_ABC_LAYOUT_VERDICT_AGY.md`
