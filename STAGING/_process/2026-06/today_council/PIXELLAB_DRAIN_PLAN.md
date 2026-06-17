# PixelLab Generation Drain Plan
# Target: ~634 generations (use-it-or-lose-it, ay-sonu reset)
# Execution order: A (demo-critical, wire edilecek) -> B (enrichment) -> C (reusable library drain)
# Orchestrator bu dosyayi queue olarak kullanir; her satir = 1 MCP call veya 1 web-UI call.

## TOOL LEGEND
- `create_object`       = MCP create_object (directions=1 or 8)
- `create_map_object`   = MCP create_map_object (non-square, transparent bg)
- `create_topdown_tileset` = MCP create_topdown_tileset (Wang 16-tile pack)
- `create_tiles_pro`    = MCP create_tiles_pro (batch up to 4 types, ~20-25 gen)
- `create_character`    = MCP create_character standard (8-dir, ~1 gen/call NOTE: base sprite only)
- `animate_character`   = MCP animate_character template (1 gen/dir, cheap)
- `create_1dir_obj`     = shorthand for create_object directions=1
- `create_8dir_obj`     = shorthand for create_object directions=8

## COST ASSUMPTIONS
- create_object 1-dir, size<=85: 1 call ~ 20-40 gen (MCP object suite). Use n_frames=4 to pick best.
  ACTUAL observation: "20-40 gen per call regardless of candidate count" -- treat as 20 gen per call.
- create_object 8-dir: same cost band (~20 gen per call).
- create_topdown_tileset: ~20 gen per call (Wang 16-tile pack).
- create_tiles_pro (4-type batch): ~20-25 gen per call.
- create_character standard 8-dir: ~1 gen per call (standard mode, NOT pro).
- animate_character template 1 gen/dir: S+SW+W+NW+N = 5 gen per animation per character.
- create_map_object: ~1 gen per call (basic mode, non-square).

## GEN BUDGET BREAKDOWN
- A. Demo-critical:     ~200 gen  (props + enemies)
- B. RIMA enrichment:  ~80  gen  (tiles + decals + VFX)
- C. Global library:   ~354 gen  (reusable drain)
- TOTAL TARGET:        ~634 gen

---

## A. DEMO-CRITICAL (wire edilecek -- ONCE QUEUE ET)
### A1. Build Mode Props (Act1 slate/void palette, PPU=64, top-down, 1-dir static)
# Style lock: muted desaturated palette, weathered field-worn, slate #3A3D42 / void purple #3A1A4A
# Prompt suffix every row: "top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray and void purple tones, transparent background, crisp pixel art, no anti-aliasing, 64x64"
# Each call: create_object, directions=1, n_frames=4, size=64, view="high top-down", object_view="top-down"

| # | Tool            | Prompt (full, English)                                                                                                                                      | Size  | dirs | n_frames | est-gen | tag       |
|---|-----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------|-------|------|----------|---------|-----------|
| 1 | create_object   | wooden barrel, old weathered staves with iron bands, standing upright, top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray and void purple tones, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
| 2 | create_object   | wooden crate, nailed shut with rope lashing, top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray and void purple tones, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
| 3 | create_object   | ornate chest with iron lock and brass fittings, closed lid, top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray and void purple tones, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
| 4 | create_object   | iron brazier on tripod legs filled with glowing embers, top-down view, pixel art, muted desaturated palette weathered field-worn, ember orange accent, void purple glow, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
| 5 | create_object   | broken stone pillar, crumbled top, mossy cracks, top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray tones, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
| 6 | create_object   | torn cloth banner hanging on wall hook, faded emblem, top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray and void purple tones, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
| 7 | create_object   | stone statue fragment, armless torso on pedestal, weathered, top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray tones, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
| 8 | create_object   | rubble pile of broken stone and dust, top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray tones, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
| 9 | create_object   | wall-mounted torch with flickering flame, iron bracket, top-down view, pixel art, muted desaturated palette weathered field-worn, ember orange flame, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |
|10 | create_object   | burlap sack tied with rope, lumpy shape, top-down view, pixel art, muted desaturated palette weathered field-worn, slate gray tones, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | A-prop |

# A1 subtotal: 10 calls x 20 gen = 200 gen   (pick best frame from n_frames=4 per call)
# NOTE: weapon rack, bone pile deferred to C (global library) to avoid palette-lock conflict

### A2. Enemy Mob Sprites (RIMA 8-dir, standard mode, black-blob silhouette fix)
# CAMERA RULE: MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes
# facing forward -- not looking up at the camera. The steep overhead angle hides the eyes naturally.
# Preset: male human / female human ONLY. No "Heroic".
# Standard mode = 1 gen/call for base 8-dir sprite. Animate separately (template).

| # | Tool             | Prompt (full, English)                                                                                                                                                                                | Size | dirs | est-gen | tag      |
|---|------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------|------|---------|----------|
|11 | create_character | ChainWarden enemy, heavily armored humanoid guard in rusted plate with chain manacles trailing, distinct silhouette wide shoulders, muted desaturated palette weathered field-worn, slate and rust tones, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, standard 8-directional, transparent background, pixel art, no anti-aliasing | 128 | 8 | 1 | A-enemy |
|12 | create_character | VoidThrall enemy, corrupted humanoid with cracked void-purple skin and hollow glowing cyan eyes, hunched posture distinct silhouette, muted desaturated palette weathered field-worn, void purple and cyan accent, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, standard 8-directional, transparent background, pixel art, no anti-aliasing | 128 | 8 | 1 | A-enemy |
|13 | create_character | RelicCaster enemy, robed humanoid with ornate cracked relic staff, tall hood, distinct silhouette, muted desaturated palette weathered field-worn, slate and ember orange accent, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, standard 8-directional, transparent background, pixel art, no anti-aliasing | 128 | 8 | 1 | A-enemy |
|14 | create_character | FractureImp enemy, small hunched imp creature with cracked stone hide and glowing fracture lines, compact readable silhouette, muted desaturated palette weathered field-worn, slate and cyan fracture glow, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, standard 8-directional, transparent background, pixel art, no anti-aliasing | 128 | 8 | 1 | A-enemy |
|15 | create_character | HalfThrall enemy, partially corrupted humanoid with one normal arm and one void-warped arm, asymmetric silhouette, muted desaturated palette weathered field-worn, void purple and flesh tones, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png, standard 8-directional, transparent background, pixel art, no anti-aliasing | 128 | 8 | 1 | A-enemy |

# A2 subtotal: 5 calls x 1 gen = 5 gen (standard mode base sprites only)
# Post-QC: animate each with template animations (walk=5 gen/char, idle=5 gen/char -> 50 gen for all 5)

### A2b. Enemy Template Animations (animate_character, template mode, 1 gen/dir)
# Run AFTER A2 QC passes; use character_id from A2 calls
# 5 directions produced (S/SW/W/NW/N), E/SE/NE mirrored in web UI or Unity
# Per enemy: idle(5gen) + walk(5gen) = 10 gen. 5 enemies x 10 = 50 gen.

| # | Tool               | character_id    | animation                  | dirs | est-gen | tag       |
|---|--------------------|-----------------|-----------------------------|------|---------|-----------|
|16 | animate_character  | [ChainWarden]   | template: breathing idle    | 5    | 5       | A-anim    |
|17 | animate_character  | [ChainWarden]   | template: walk cycle        | 5    | 5       | A-anim    |
|18 | animate_character  | [VoidThrall]    | template: breathing idle    | 5    | 5       | A-anim    |
|19 | animate_character  | [VoidThrall]    | template: walk cycle        | 5    | 5       | A-anim    |
|20 | animate_character  | [RelicCaster]   | template: breathing idle    | 5    | 5       | A-anim    |
|21 | animate_character  | [RelicCaster]   | template: walk cycle        | 5    | 5       | A-anim    |
|22 | animate_character  | [FractureImp]   | template: breathing idle    | 5    | 5       | A-anim    |
|23 | animate_character  | [FractureImp]   | template: walk cycle        | 5    | 5       | A-anim    |
|24 | animate_character  | [HalfThrall]    | template: breathing idle    | 5    | 5       | A-anim    |
|25 | animate_character  | [HalfThrall]    | template: walk cycle        | 5    | 5       | A-anim    |

# A2b subtotal: 10 calls x 5 gen = 50 gen

## A TOTAL: ~255 gen  (200 props + 5 base chars + 50 anim)
# [DEMO-CRITICAL -- execute first, wire to Unity after QC]

---

## B. RIMA ENRICHMENT
### B1. Floor / Wall Tile Varyantlari (create_topdown_tileset, Wang 16-tile)
# Act1 Shattered Keep canonical tones: slate / void / ember accent
# Each create_topdown_tileset call = ~20 gen, produces 16 Wang tiles (or 23 with transition)

| # | Tool                    | lower_description                                           | upper_description                        | transition | tile_size | est-gen | tag     |
|---|-------------------------|-------------------------------------------------------------|------------------------------------------|------------|-----------|---------|---------|
|26 | create_topdown_tileset  | cracked stone dungeon floor, slate gray worn texture        | void darkness, empty abyss               | full       | 32        | 20      | B-tile  |
|27 | create_topdown_tileset  | mossy stone dungeon floor, dark green moss in cracks        | void darkness, empty abyss               | full       | 32        | 20      | B-tile  |
|28 | create_topdown_tileset  | blood-stained stone floor, old dark stains                  | cracked stone dungeon floor slate gray   | small      | 32        | 20      | B-tile  |

# B1 subtotal: 3 calls x 20 gen = 60 gen

### B2. Decal / Scatter (create_map_object, transparent, small)
# Transparent bg, RIMA palette, decal-style (flat, no elevation)

| # | Tool               | Prompt (full, English)                                                                                                              | w  | h  | est-gen | tag      |
|---|--------------------|-------------------------------------------------------------------------------------------------------------------------------------|----|----|---------|----------|
|29 | create_map_object  | scattered bone fragments and dust, top-down view, pixel art, muted desaturated palette, slate gray, transparent background, no anti-aliasing | 64 | 64 | 1   | B-decal  |
|30 | create_map_object  | dark dried blood pool stain, irregular shape, top-down view, pixel art, muted desaturated palette, dark brownish red, transparent background, no anti-aliasing | 96 | 64 | 1 | B-decal |
|31 | create_map_object  | void energy crack in floor, glowing cyan fissure, top-down view, pixel art, muted desaturated palette with cyan accent, transparent background, no anti-aliasing | 96 | 32 | 1 | B-decal |

# B2 subtotal: 3 calls x 1 gen = 3 gen

### B3. Skill VFX (create_object, animated, 1-dir)
# Slash, impact, cast burst -- transparent bg, RIMA ember/cyan palette

| # | Tool          | Prompt (full, English)                                                                                                                 | Size | dirs | n_frames | est-gen | tag    |
|---|---------------|----------------------------------------------------------------------------------------------------------------------------------------|------|------|----------|---------|--------|
|32 | create_object | sword slash arc effect, white-cyan energy streak, top-down view, pixel art, transparent background, crisp pixel art, no anti-aliasing  | 64   | 1    | 4        | 20      | B-vfx  |
|33 | create_object | spell impact burst, ember orange-cyan radial explosion sparks, top-down view, pixel art, transparent background, crisp pixel art, no anti-aliasing | 64 | 1 | 4 | 20 | B-vfx |

# B3 subtotal: 2 calls x 20 gen = 40 gen (n_frames=4 to pick best)
# Animate after QC via animate_object (4-8 frames)

## B TOTAL: ~103 gen  (60 tiles + 3 decals + 40 vfx)
# NOTE: B total overshoots original 80-gen estimate by ~23 gen; acceptable, still within budget.

---

## C. GLOBAL REUSABLE LIBRARY (tema-bagimsiz, drain)
# Style: clean generic pixel art, mid-tone palette, top-down OR side view as noted
# NO RIMA palette lock -- generic readable colors. Use n_frames=4 where size<=85 for variant pick.
# Goal: re-use in any 2D game project, genre-neutral.
# All sizes 64x64 unless noted. All transparent background.

### C1. Generic Props -- Container / Storage (create_object, 1-dir)
# ~20 gen each (MCP object suite cost floor)

| # | Tool          | Prompt (full, English)                                                                                                  | Size | dirs | n_frames | est-gen | tag    |
|---|---------------|-------------------------------------------------------------------------------------------------------------------------|------|------|----------|---------|--------|
|34 | create_object | wooden barrel, generic brown stave-and-ring barrel, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|35 | create_object | wooden crate, generic pine slat crate with iron corners, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|36 | create_object | treasure chest, generic wooden chest with gold latch, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|37 | create_object | burlap sack with rope tie, lumpy filled shape, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|38 | create_object | clay pottery jug, round-bodied with small handle, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|39 | create_object | ceramic vase, tall narrow-neck vase, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|40 | create_object | wicker basket, oval woven basket with lid, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |

# C1 subtotal: 7 x 20 = 140 gen

### C2. Generic Props -- Furniture (create_object, 1-dir, top-down or side)
| # | Tool          | Prompt (full, English)                                                                                                | Size | dirs | n_frames | est-gen | tag    |
|---|---------------|-----------------------------------------------------------------------------------------------------------------------|------|------|----------|---------|--------|
|41 | create_object | wooden table, simple four-leg rectangle table, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|42 | create_object | wooden chair, simple back-rest chair, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|43 | create_object | single bed with pillow and blanket, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|44 | create_object | wooden shelf with books on it, side view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|45 | create_object | wooden barrel stool, round-top barrel shaped seat, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|46 | create_object | iron anvil on wooden stump, heavy dark metal, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|47 | create_object | large iron cauldron on legs, round black pot, top-down view, pixel art, clean generic mid-tone palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |

# C2 subtotal: 7 x 20 = 140 gen

### C3. Generic Props -- Lighting / Atmosphere (create_object, 1-dir)
| # | Tool          | Prompt (full, English)                                                                                                           | Size | dirs | n_frames | est-gen | tag    |
|---|---------------|----------------------------------------------------------------------------------------------------------------------------------|------|------|----------|---------|--------|
|48 | create_object | wall-mounted torch bracket with flame, side view, pixel art, clean generic warm orange palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|49 | create_object | iron lantern hanging, round glass-pane lantern with warm glow, top-down view, pixel art, clean generic warm palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|50 | create_object | tall candle on holder, single white candle dripping wax, side view, pixel art, clean generic palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|51 | create_object | campfire, small ring of stones with burning logs, top-down view, pixel art, clean generic warm orange palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |

# C3 subtotal: 4 x 20 = 80 gen

### C4. Generic Consumables / Pickups (create_object, 1-dir, 64px)
| # | Tool          | Prompt (full, English)                                                                                                               | Size | dirs | n_frames | est-gen | tag       |
|---|---------------|--------------------------------------------------------------------------------------------------------------------------------------|------|------|----------|---------|-----------|
|52 | create_object | red health potion, round flask with cork, side view, pixel art, clean generic palette, red liquid, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|53 | create_object | blue mana potion, round flask with cork, side view, pixel art, clean generic palette, blue liquid, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|54 | create_object | gold coin pile, stack of shiny gold coins, top-down view, pixel art, clean generic palette, gold and yellow tones, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|55 | create_object | shiny gem, faceted cut ruby, top-down view, pixel art, clean generic palette, deep red gem, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|56 | create_object | old iron key, ornate bow key, side view, pixel art, clean generic palette, iron gray, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|57 | create_object | leather-bound book closed, thick volume with clasp, side view, pixel art, clean generic palette, brown leather, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|58 | create_object | rolled parchment scroll, tied with ribbon, side view, pixel art, clean generic palette, parchment cream tones, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|59 | create_object | loaf of bread, rustic round loaf, side view, pixel art, clean generic palette, warm brown, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|60 | create_object | roast meat on bone, cooked leg piece, side view, pixel art, clean generic palette, warm brown meat tones, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |
|61 | create_object | apple, ripe red apple, side view, pixel art, clean generic palette, red and green, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-pickup |

# C4 subtotal: 10 x 20 = 200 gen

### C5. Generic Nature / Environment Props (create_map_object, non-square ok)
| # | Tool               | Prompt (full, English)                                                                                                        | w   | h   | est-gen | tag      |
|---|--------------------|-------------------------------------------------------------------------------------------------------------------------------|-----|-----|---------|----------|
|62 | create_map_object  | mossy rock cluster, group of three rounded stones with green moss, top-down view, pixel art, clean generic palette, transparent background | 96 | 64 | 1 | C-env |
|63 | create_map_object  | tree stump with mushrooms, cut stump with small mushroom cluster, top-down view, pixel art, clean generic palette, transparent background | 64 | 80 | 1 | C-env |
|64 | create_map_object  | wooden fence segment, horizontal plank fence two rails, side view, pixel art, clean generic palette, wooden brown, transparent background | 96 | 64 | 1 | C-env |
|65 | create_map_object  | wooden bucket with handle, side view, pixel art, clean generic palette, brown wood, transparent background | 48 | 64 | 1 | C-env |
|66 | create_map_object  | small potted plant, green leafy plant in terracotta pot, side view, pixel art, clean generic palette, transparent background | 48 | 64 | 1 | C-env |
|67 | create_map_object  | signpost with blank sign, wooden post with rectangular sign board, side view, pixel art, clean generic palette, transparent background | 64 | 96 | 1 | C-env |
|68 | create_map_object  | small bush, round leafy bush, top-down view, pixel art, clean generic palette, dark green, transparent background | 64 | 64 | 1 | C-env |
|69 | create_map_object  | flat boulder, large flat-top natural stone, top-down view, pixel art, clean generic palette, gray stone, transparent background | 96 | 64 | 1 | C-env |

# C5 subtotal: 8 x 1 gen = 8 gen

### C6. Generic Tilesets -- Terrain Variety (create_topdown_tileset, Wang 16-tile)
| # | Tool                    | lower_description                                       | upper_description              | transition | tile_size | est-gen | tag     |
|---|-------------------------|---------------------------------------------------------|-------------------------------|------------|-----------|---------|---------|
|70 | create_topdown_tileset  | green grass, lush green field with subtle texture       | bare dirt earth, brown soil   | full       | 32        | 20      | C-tile  |
|71 | create_topdown_tileset  | gray cobblestone floor, mortared rectangular stones     | mud and dirt, earth            | full       | 32        | 20      | C-tile  |
|72 | create_topdown_tileset  | wooden plank floor, light oak horizontal planks         | rough stone foundation         | full       | 32        | 20      | C-tile  |
|73 | create_topdown_tileset  | sandy desert floor, fine pale sand with pebbles         | cracked dry earth              | full       | 32        | 20      | C-tile  |
|74 | create_topdown_tileset  | shallow water, clear blue rippling water surface        | sandy shore, pale sand         | full       | 32        | 20      | C-tile  |
|75 | create_topdown_tileset  | snow-covered ground, white snow with slight texture     | frozen ice, pale blue ice      | full       | 32        | 20      | C-tile  |

# C6 subtotal: 6 x 20 gen = 120 gen

### C7. Generic Props -- Tools / Utility (create_object, 1-dir, small/mid)
| # | Tool          | Prompt (full, English)                                                                                                     | Size | dirs | n_frames | est-gen | tag    |
|---|---------------|----------------------------------------------------------------------------------------------------------------------------|------|------|----------|---------|--------|
|76 | create_object | wooden barrel with open top, half-filled with water, top-down view, pixel art, clean generic palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|77 | create_object | rope coil, circular coil of brown rope, top-down view, pixel art, clean generic palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|78 | create_object | wooden ladder leaning, propped against invisible wall, side view, pixel art, clean generic palette, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |
|79 | create_object | metal chain loops, linked iron chain pile, top-down view, pixel art, clean generic palette, iron gray, transparent background, crisp pixel art | 64 | 1 | 4 | 20 | C-prop |

# C7 subtotal: 4 x 20 gen = 80 gen

## C TOTAL: ~768 gen raw estimate (140+140+80+200+8+120+80)
# BUDGET CAP REQUIRED: C total exceeds ~354-gen C-budget + overrun from A/B.
# PRIORITIZATION: execute C1->C3->C4 first (highest value), then C5->C6->C7 until balance nears 0.
# STOP when get_balance <= 10 gen.

---

## EXECUTION ORDER (orchestrator queue)
1. A1: Props rows 1-10  (200 gen)        -- FIRST, demo wire
2. A2: Enemy base rows 11-15 (5 gen)     -- SECOND, demo wire
3. B1: Tiles rows 26-28 (60 gen)         -- THIRD, enrichment
4. B2: Decals rows 29-31 (3 gen)
5. B3: VFX rows 32-33 (40 gen)
6. A2b: Enemy anims rows 16-25 (50 gen) -- AFTER A2 QC (needs character_id)
7. C1: rows 34-40 (140 gen)              -- drain
8. C2: rows 41-47 (140 gen)              -- drain
9. C3: rows 48-51 (80 gen)               -- drain
10. C4: rows 52-61 (200 gen)             -- drain
11. C5: rows 62-69 (8 gen)               -- drain
12. C6: rows 70-75 (120 gen)             -- drain (STOP if balance low)
13. C7: rows 76-79 (80 gen)              -- drain (STOP if balance low)

## BALANCE CHECKPOINTS
- After A: call get_balance. Expected remaining: ~379.
- After B: call get_balance. Expected remaining: ~276.
- After A2b: call get_balance. Expected remaining: ~226.
- During C: call get_balance every 5 calls. STOP queuing when remaining <= 10.

## HARD STOPS
- get_balance <= 10 gen: STOP all calls immediately.
- Rate limit exceeded: wait 60s, retry wave (max 5-6 concurrent).
- Any call returns error: log object_id=null, continue next row, do NOT retry same row twice.
- Map object: download immediately after completed (auto-deletes after 8 hours).

## GEN SUMMARY TABLE
| Category              | Items | est-gen | Priority |
|-----------------------|-------|---------|----------|
| A1 Props (demo)       | 10    | 200     | FIRST    |
| A2 Enemy base (demo)  | 5     | 5       | SECOND   |
| A2b Enemy anim (demo) | 10    | 50      | AFTER QC |
| B1 Tiles (enrich)     | 3     | 60      | THIRD    |
| B2 Decals (enrich)    | 3     | 3       | FOURTH   |
| B3 VFX (enrich)       | 2     | 40      | FIFTH    |
| C1 Containers (lib)   | 7     | 140     | DRAIN    |
| C2 Furniture (lib)    | 7     | 140     | DRAIN    |
| C3 Lighting (lib)     | 4     | 80      | DRAIN    |
| C4 Consumables (lib)  | 10    | 200     | DRAIN    |
| C5 Nature (lib)       | 8     | 8       | DRAIN    |
| C6 Tilesets (lib)     | 6     | 120     | DRAIN    |
| C7 Tools (lib)        | 4     | 80      | DRAIN    |
| **TOTAL (all rows)**  | **79**| **1126**| --       |
| **ACTUAL TARGET**     | --    | **634** | --       |

# The plan has MORE rows than budget -- this is intentional (use-it-or-lose-it drain).
# Execute in priority order; get_balance checkpoints cut off C-rows when budget is consumed.
# A+B+A2b = ~358 gen (wire-critical + enrichment). C-drain fills remaining ~276 gen.
# Expected actual drain: A+B+A2b+C1+partial-C2 = ~634 gen approximately.
