# Research Task — Free Asset Survey + Alternative RIMA Tile Solutions

ACTIVE RULES: (1) think before deciding (2) honest review (3) actionable specifics (4) BLOCKED if unclear.

---

## Mission

User asks: "RIMA tile sorununu nasıl çözerim — free asset mi, başka türlü mü, kamera açısı mı? Düşün."

Context (memory):
- Character 35° high top-down LOCKED (Karar #100, 10 anchor PixelLab, ~5000 gen invested)
- PixelLab tile generation = sprite-frame projection, NOT ground-plane (Opus + Codex verdict)
- All existing RIMA tiles have framing/border issues (1200+ gen pipeline failed)
- Tile angle architecture LOCKED: Branch D (Hades floor de-emphasis) + Branch E (camera tilt 4-8° smoke test) — `STAGING/TILE_ANGLE_ARCHITECTURE_OPUS.md`
- Visual asset gen FROZEN for this night

## Research scope — 4 directions

### Direction 1: Free / cheap asset packs for 35° angled top-down

Survey for **production-ready** RIMA-compatible floor + wall + prop packs:
- itch.io top-down pixel art tile packs
- OpenGameArt CC0/CC-BY assets
- Kenney.nl free assets (kenney.nl is high quality, CC0)
- ArtStation Marketplace bundles ($5-30 range)
- Search terms: "35 degree top-down pixel tiles", "Hades-style 2D tiles", "angled top-down RPG tilesheet"

For each promising pack:
- Style match RIMA (cool granite/cobble + organic + chunky 32px painterly)
- License (CC0 / CC-BY / paid)
- Size (number of tiles, walls, props)
- Direct download / commercial use allowed
- Concrete link

### Direction 2: Alternative tile generation tools (non-PixelLab)

- gpt-image-1 (Codex subscription) — single-prompt seamless tilesheet feasibility?
- Local Stable Diffusion + ControlNet tile mode (RTX 5080 user)
- FLUX dev/schnell — quality at 32px chunky?
- Diffusion-based: samsartor/content_aware_tiles (already analyzed, future R&D)
- Tilesetter ($25, GUI manual) — rejected for autonomy but evaluate anyway

Compare honest:
- Quality at 32px chunky
- Seamless? Border-free?
- Time per tile pair
- Cost
- Autonomy (Claude/Codex can drive without manual click?)

### Direction 3: Camera angle architectural solutions

Beyond Branch E (4-8° tilt):
- Full perspective camera + low FOV (Octopath Traveler style HD-2D)
- Multiple cameras (one un-tilted for chars, one tilted for floor)
- Render-texture composite
- 2.5D billboard sprites with vertical quad rotation
- 3D-like effects without 3D: shader perspective transform, faux-depth via per-tile shadow

For each: feasibility in URP 2D Renderer + Pixel Perfect setup, complexity score, expected visual gain.

### Direction 4: Hybrid approach RIMA-specific

Combinations:
- Free asset base + AI-generated overlay (best of both)
- AI tile + AI-generated angled prop overlay matching 35°
- "Wash floor" (flat low-detail) + 3D-rendered prop sprites at angle
- Stardew approach adapted: single-color seamless base + heavy scatter density
- Death's Door approach: full 3D low-poly diorama (out of scope but reference)

## Required output

`STAGING/RESEARCH_DONE_free_asset_alternatives.md`:

```
# VERDICT
[Single paragraph: top 3 actionable directions ranked by ROI]

# 1. Free asset survey
[Per pack: link, license, style match, size, RIMA compatibility]

# 2. Alternative gen tools
[Comparison table: gpt-image-1 / FLUX / Tilesetter / etc.]

# 3. Camera architectural alternatives
[Per approach: feasibility + complexity + visual gain]

# 4. Hybrid combinations
[Top 3 hybrid paths]

# 5. Concrete morning action items (ranked)
[Top 3 things user can do in <2 hours]
```

Effort: deep web research. ~30-40 min. Honest evaluation, ranked by ROI.

Hard rules:
- NO PixelLab gen tonight (already done via Opus tile angle Branch A REJECT)
- DON'T re-recommend Branch A
- DO consider gpt-image-1 (different billing than PixelLab, Codex subscription)
- DO include concrete links for free assets
- Estimate time + cost for each option
