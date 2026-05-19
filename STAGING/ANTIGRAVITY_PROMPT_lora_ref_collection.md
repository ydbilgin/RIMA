# ANTIGRAVITY PROMPT — Pixel Art Tile Reference Collection

**(User: bunu Antigravity'e yapıştır. Self-contained, harici dosya yok.)**

---

Hi Antigravity — I'm building a custom SDXL LoRA to generate pixel-art floor/wall/decal tiles for a 2D roguelite called RIMA. I need you to collect a **training reference dataset**: 150-250 high-quality pixel art tile PNG files, organized and licensed properly.

## What I need

Collect **150-250 PNG tile assets** from the source list below, save them in folder `F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/refs_raw/antigravity/`. Each source pack gets its own subfolder inside `antigravity/`. Each subfolder must have a `SOURCE.md` with URL + license + attribution requirement.

**Note:** Two other agents (Codex CLI and Anthropic Opus subagent) are running this same task in parallel into sibling folders `refs_raw/codex/` and `refs_raw/opus/`. We will deduplicate and merge results afterwards — focus on **broad coverage**, don't worry about overlap.

## Style bar (every ref must hit these)

- **Native pixel art** — hard pixel edges, NO anti-aliasing, NO soft gradient
- **Top-down view** — high angle (30-60° tilt) or pure overhead; for ground/floor tiles either is fine
- **Tile size** 16-128px range (32, 48, 64 px ideal; 128 max)
- **Atmosphere** — weathered stone, dungeon/temple/cave/ruin, dark moody. NOT bright pastoral. NOT cartoon. NOT farming-sim.
- **Palette** — dark slate/brown/deep blue/moss green dominant. NOT bright saturated.
- **Era** — modern indie pixel art tradition (post-2018 standards) — CrossCode / Hyper Light Drifter / Eastward / Tunic / Death's Door family.

## Reject these (do NOT download)

- Bright cartoon RPG Maker assets
- Anime/manga-painted tiles
- Vector/illustrator style
- 3D rendered downscaled
- Photo-mosaic textures
- AI-generated fake-pixel-art (anti-aliased)
- Cute pastel daylight farming-sim aesthetic
- Sprite sheets (characters/mobs/UI) — **ONLY floor/wall/ground/cave/dungeon tiles**
- Single images larger than 256×256 (those are usually full sheets, not single tiles)

## Source list (priority order)

### PRIORITY 0 — itch.io CC-licensed packs

- Cainos — `cainos.itch.io/pixel-art-top-down-basic` (also Castle/Forest/Caves variants)
- ZB-Kappa — `zb-kappa.itch.io` (multiple dungeon tilesets)
- HylianShield — `hylianshield.itch.io/rpg-tileset` and related
- ansimuz — `ansimuz.itch.io` (dungeon asset packs)
- 0x72 — `0x72.itch.io/dungeontileset-ii` (16×16 native, very popular)
- PixelFrog — `pixelfrog-assets.itch.io/pixel-adventure-1` and dungeon variants
- AnokoYZ — Stone Caverns (search itch.io)
- Pita Dreams — `pita-dreams.itch.io` (some free packs in CrossCode tradition)

### PRIORITY 1 — Kenney.nl (CC0)

- Tiny Dungeon (16×16 native, large pack)
- Tiny Town
- Roguelike Pack
- Direct: `kenney.nl/assets`
- All Kenney = CC0 (no attribution required)

### PRIORITY 1 — OpenGameArt.org

- Search: `opengameart.org/art-search?keys=dungeon+tileset+pixel`
- Top contributors: Buch, Reemax, surt, daneeklu
- Filter by tilesize 16/32

### PRIORITY 2 — GitHub free repos

- `deepnight/ldtk-examples` — sample tilesets in LDtk format
- Various indie game devs with public asset folders

### PRIORITY 3 — CraftPix.net free section

- `craftpix.net/freebies/` filter free tilesets, top-down RPG, dungeon

## Licensing rules — STRICT

**OK to download:**
- CC0 (no attribution)
- CC-BY (attribution required — note in SOURCE.md)
- "Free for personal use" — note in SOURCE.md
- Kenney.nl assets (all CC0)
- Marked free on itch.io

**DO NOT download:**
- "Paid only" packs
- CrossCode official sprites (Radical Fish copyrighted)
- Alabaster Dawn screenshots (Radical Fish copyrighted)
- Hyper Light Drifter assets (Heart Machine copyrighted)
- Any "personal portfolio" piece without explicit license
- Reuploads where license is unclear

## Folder structure (create exactly this)

```
F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/
  refs_raw/
    antigravity/                       ← your output goes HERE (not refs_raw/ directly)
      cainos_top_down_basic/
        SOURCE.md                       ← URL, license, author, date
        <png files from pack>
      kenney_tiny_dungeon/
        SOURCE.md
        <png files>
      0x72_dungeon_tileset_ii/
        SOURCE.md
        <png files>
      opengameart_buch_dungeon/
        SOURCE.md
        <png files>
      ... (one folder per source pack)
      _INDEX.md                         ← summary report (see below)
    codex/                             ← parallel Codex CLI agent (don't touch)
    opus/                              ← parallel Opus subagent (don't touch)
```

## SOURCE.md template (for each pack folder)

```markdown
# <Pack Name>

- URL: <original itch.io / opengameart / kenney URL>
- Author: <name>
- License: <CC0 / CC-BY 4.0 / Free for personal use / etc.>
- Attribution required: <yes/no — if yes, exact attribution text>
- Download date: 2026-05-19
- File count: <n>
- Notes: <any quality/style notes>
```

## Final summary report

After all downloads, write `F:/LaurethStudio/05_TRAINING/RIMA_tile_lora_v1/refs_raw/_INDEX.md`:

```markdown
# Reference Collection — Summary

- Total PNG files: <n>
- Source pack count: <n>
- License breakdown:
  - CC0: <n files>
  - CC-BY: <n files>
  - Free personal: <n files>
- Pack-level breakdown:
  | Pack | Files | License | Style notes |
  |---|---|---|---|
  | Cainos Top Down Basic | 45 | CC-BY | warm forest, may not match dungeon mood |
  | Kenney Tiny Dungeon | 80 | CC0 | 16px native, very clean |
  | ... | | | |
- Issues / skipped sources: <list>
```

## What I'll do after you finish

Don't curate/filter. Just bulk download with proper licensing. I'll visually QC and pick the ~150 best refs for training.

## Constraints

- **Real downloads only** — curl / wget / Python `requests`. No synthetic placeholders.
- **Skip blocked sources** — note in report, move on
- **Don't extract from archives** that have unclear licensing — fold those into a `_QUARANTINE/` subfolder with a note
- **Skip dynamically-loaded gallery sites** that need JS rendering — note in report

Total time estimate: 1-2 hours.

---

**End of Antigravity task.**
