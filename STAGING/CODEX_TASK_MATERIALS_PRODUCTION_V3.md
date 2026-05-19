# Codex Task — Materials Production v3 (Walls + Props + Biome Variants)

**Profile:** auto-selected (quota-aware, cx_dispatch.py select_profile_quota_aware)
**Effort:** high
**Type:** Codex imagegen (gpt-image-1 backend, built-in skill) — produce 4 new sprite sheets in `STAGING/RIMA_AssetParts_v3/`

## Context

Current PlayableRoom (Game view `Assets/Screenshots/PlayableRoom_openworld_v6.png`):
- Floor + macro patches + decals + scatter + 1 rift + 1 ritual + Warblade character + camera follow
- LIVE 36×22 open world expansion (792 tiles), walls hidden
- 7 PatchAtlasSO from prior `RIMA_AssetParts_v2` LIVE at `Assets/Data/Brush/AssetParts_v2/`

User goal: **Alabaster Dawn + Hades** visual feel. Current is close to it; missing:
1. **Wall sprites** (vertical dark stone, angled top-down 30-35° front-face visible)
2. **Vertical props** (column intact, column broken, brazier lit, banner torn, statue) for visual depth + fake-3D
3. **Biome variation floor** (current single dark slate → add mossy ground, blood-stained, ritual chamber variants)

PixelLab gen budget renews 2026-05-19 (~5000 gens). This task uses Codex imagegen ONLY (built-in skill).

## CRITICAL — Camera + style lock (hard rules)

- Low top-down 30-35° angled ARPG perspective (Hades / Hyper Light Drifter / Diablo 2)
- Painterly pixel-art-compatible
- Each prop with subtle front-face direction (upper darker, lower lighter — fake depth)
- Tone family: Hades + Salt-and-Sanctuary dark gritty mood
- PPU=32 IMMUTABLE — sprite native sizes designed for PPU=32

## Sheets to generate

### Sheet 09: Wall variants (1024×1024, 4×3 grid → 12 variants at 64×64 native each, RGBA transparent BG)

```
Twelve dark stone wall tile variants for ARPG roguelite map borders, arranged in a 4x3 grid where each cell is 64x64 pixels with TRANSPARENT background outside the wall sprite. Low top-down 30-35 degree angled view — wall faces are visibly slightly tilted toward camera (front face dominant, subtle top edge visible as thin lit highlight, subtle bottom edge as deep shadow). All tiles share muted slate stone palette: base hex 2A2622, midtone hex 3D3530, highlight hex 5A4E42, deep crack hex 1A1814. Wall front face has hand-laid brick pattern (irregular blocks ~16x12px each), warm amber dust in cracks. Variations:
1) straight wall horizontal segment (top of room)
2) straight wall vertical segment (side of room)
3) corner outer NE (90° outer corner)
4) corner outer NW
5) corner outer SE
6) corner outer SW
7) wall with hairline crack network
8) wall with chipped block (one block missing top)
9) wall with moss growth on lower portion
10) wall with banner-attachment iron hook
11) wall with arched doorway gap
12) wall with torch sconce (no flame)
NO geometric ornament, NO carved decorations, NO runes, NO sci-fi elements, NO bright colors. Each tile must tile seamlessly with adjacent identical tiles. Subtle hand-painted irregularity per brick.
```

Output: `STAGING/RIMA_AssetParts_v3/sheet_09_walls_64x64.png`

### Sheet 10: Vertical props (1024×1024, 4×2 grid → 8 props at 128×128 native each, RGBA transparent BG)

```
Eight tall vertical environmental props for dark fantasy roguelite map, arranged in a 4x2 grid where each cell is 128x128 pixels with TRANSPARENT background outside the prop silhouette. Low top-down 30-35 degree angled view (props show top plane + front face for fake-3D depth, soft oval ground shadow at base). Painterly pixel art, 1px dark outline, max 3 tones per color. Muted dark fantasy palette: slate gray base, warm amber accents, dark iron details, subtle cold blue rim highlights. Props:
1) intact stone column (tall, weathered, with subtle moss tufts at base)
2) broken stone column (large diagonal break, leaning slightly, debris at base)
3) lit iron brazier (3-leg base, warm orange ember in dish, tight warm glow only)
4) torn cloth banner (hanging from iron rod, dark crimson red fabric, frayed bottom, faded emblem)
5) kneeling stone statue (broken neck stump, prayer pose hands, moss tufts at base)
6) hanging iron chain (single continuous chain from ceiling bracket to near ground, rust accents)
7) stacked debris pile (mixed stone rubble + a few bone fragments, irregular silhouette)
8) standing iron candelabra (4-arm holder, small candles, tight warm flame near each wick)
NO bright neon, NO modern elements, NO duplicate copies in one cell, NO multiple props per cell. Each prop fully visible, centered in its cell.
```

Output: `STAGING/RIMA_AssetParts_v3/sheet_10_vertical_props_128x128.png`

### Sheet 11: Biome floor variants (1024×1024, 4×4 grid → 16 variants at 32×32 native each, full opaque)

```
Sixteen ground floor tile variants for FOUR DIFFERENT biome types, arranged in a 4x4 grid where each cell is 32x32 pixels. Each row is one biome (4 variants per biome). Painterly pixel-art style, top-down 30-35 degree angled view, subtle upper-darker lower-lighter shading for depth.

ROW 1 (top): Mossy temple ground — dark slate with moss tufts and lichen patches, mossy green hex 4A5A35, slate hex 3A4250, blends naturally tile-to-tile, no geometric pattern
ROW 2: Sandy desert floor — warm sand palette hex 8A7860 with darker grit hex 6A5840, micro pebble scatter, no patterns
ROW 3: Blood-stained ritual ground — dark slate base with dried blood smears hex 5A2520, ritual chamber atmosphere, no geometric carvings
ROW 4: Damp cave floor — cool wet stone hex 3A4555 with subtle water sheen, dark crevice hex 1A2025 in cracks, no fungal glow

ALL biomes designed to tile seamlessly within each row, NO geometric patterns, NO Y-channels, NO circuit grooves, NO carved decorations. Subtle organic variation only.
```

Output: `STAGING/RIMA_AssetParts_v3/sheet_11_biome_floors_32x32.png`

### Sheet 12: Atmospheric accents (1024×1024, 2×2 grid → 4 accents at 256×256 native each, RGBA transparent BG)

```
Four atmospheric environment accent overlays for ARPG roguelite map (used SPARINGLY, 1-2 per room max), arranged in a 2x2 grid where each cell is 256x256 pixels with TRANSPARENT background. Low top-down 30-35 degree angled view. Painterly pixel art. Each is a focal visual beat:
1) glowing portal puddle (cool blue water with soft glow ring, faint runes around edge, hex 3A6A8A glow, hex 1A2030 deep water)
2) ash circle remnant (dark gray scorched ground with bone fragments + warm amber ember smoldering, hex 3A3530 ash + hex 8A4520 embers)
3) overgrown ruin tile (cracked stone with thick moss + small flowers + ivy creeping, lush green hex 4A5A30 + warm amber dust)
4) cursed obsidian shard cluster (dark crystalline shards rising from ground with subtle violet glow, hex 1A0F1F + hex 4A3A5A glow tints)
NO bright neon, NO heavy particles, NO full glow flood. Used as rare focal narrative beats in a room.
```

Output: `STAGING/RIMA_AssetParts_v3/sheet_12_atmospheric_accents_256x256.png`

## Tasks after generation

1. QC each sheet:
   - Camera angle 30-35° angled ✓
   - Per-tile borders absent (no dark ring around each cell) ✓
   - Transparent BG outside organic shapes (sheets 9, 10, 12) ✓
   - Style consistent painterly pixel-art ✓
   - Palette muted Hades/Salt-and-Sanctuary tone ✓

2. Apply alpha-clamp + edge-desaturate Python pipeline (same as v2):
   - Read STAGING/cx_limits.py for pattern reference if needed (but it's a different script — just match the alpha-fix logic from v2 backup `STAGING/RIMA_AssetParts_v2/_pre_alpha_fix_backup/`)
   - Output: clean sheets in `STAGING/RIMA_AssetParts_v3/`

3. Re-slice via Python pipeline:
   - Sheet 09 walls: 4×3 grid → 12 PNG at 64×64 (downsample 256×192 cell? Hmm grid is 4 wide × 3 tall on 1024×1024 → cells are 256×341, autocrop required)
   - Sheet 10 props: 4×2 grid → 8 PNG at 128×128 (cells 256×512, autocrop)
   - Sheet 11 biome floors: 4×4 grid → 16 PNG at 32×32 (cells 256×256, NO autocrop because full opaque)
   - Sheet 12 accents: 2×2 grid → 4 PNG at 256×256 (cells 512×512, autocrop)
   - Output: `STAGING/RIMA_AssetParts_v3/sliced/{walls,props,biome_floors,accents}/`

4. Write `STAGING/CODEX_TASK_MATERIALS_PRODUCTION_V3_DONE.md`:
   - 4 sheet paths
   - Sliced PNG counts per category
   - SHA256 of one sample per category
   - QC notes (any failures + retry counts)
   - Suggested next step (Unity import + extend `RoomVisualProfileSO` to include new atlases)

## Constraints

- Do NOT modify any `Assets/` files (output to `STAGING/RIMA_AssetParts_v3/` only)
- Do NOT modify SO contract scripts
- Use Codex built-in `imagegen` skill (gpt-image-1)
- Each sheet must be 1024×1024 PNG
- Style consistent across all 4 sheets + matches v2 palette family

## NEXT_SIGNAL

After DONE: orchestrator dispatches Unity import + creates new PatchAtlasSO assets for walls/props/biomes + updates `RoomVisualProfileSO`. User then adds walls + props to PlayableRoom scene + Play mode WASD walking test.
