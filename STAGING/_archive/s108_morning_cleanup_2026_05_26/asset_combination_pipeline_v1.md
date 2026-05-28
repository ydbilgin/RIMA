# RIMA Asset Combination Pipeline v1
# Shattered Keep -- Modular Asset Production + Room Scaling Strategy
# Date: 2026-05-22 | Status: READY FOR NEXT SESSION

---

## Section 1 -- Vision

RIMA Act 1 (Shattered Keep) needs ~50+ visually distinct rooms across 6 room archetypes without proportionally scaling manual layout work. The approach: PixelLab generates approximately 160-180 modular assets organized into four categories -- floor tile, wall, prop, decal. Unity RoomLoader reads a JSON definition per room that specifies which assets go where, allowing combinatorial remixing at runtime or edit-time. Floor tiles supply material identity (granite, rubble, walkway, rift). Props supply narrative weight (columns, banners, chests, thrones). Decals supply atmospheric layering (moss, cracks, blood, glyphs). Walls supply boundary logic. Because each category is sliced into typed sub-variants with controlled counts, any single room archetype (e.g. Crypt) can be re-expressed with different material zone ratios, different prop selections, and different decal overlays -- producing a visual space that reads as a new room to the player, without generating a new asset or writing a new layout. Total generation cost stays well under PixelLab budget; total designer time per room variant drops to JSON editing.

---

## Section 2 -- Asset Taxonomy

| Category    | Sub-type                        | Count target         | PixelLab tool                  | Canvas   | BG          | Notes                                    |
|-------------|----------------------------------|----------------------|--------------------------------|----------|-------------|------------------------------------------|
| Floor tile  | granite                         | 8-12 tiles           | create_tiles_pro 4x4 batch     | 64x64    | opaque      | One batch covers full set                |
| Floor tile  | rubble                          | 8-12 tiles           | create_tiles_pro 4x4 batch     | 64x64    | opaque      |                                          |
| Floor tile  | walkway (stone slab)            | 8-12 tiles           | create_tiles_pro 4x4 batch     | 64x64    | opaque      |                                          |
| Floor tile  | rift-scarred                    | 8-12 tiles           | create_tiles_pro 4x4 batch     | 64x64    | opaque      | Cyan vein accents                        |
| Wall        | straight (N/S/E/W)              | 4 variants x 4 dir = 16 | create_object n_frames=16  | 64x64    | opaque      | Top-down low-profile shadow              |
| Wall        | corner (4 outer + 4 inner)      | 8                    | create_object n_frames=8       | 64x64    | opaque      |                                          |
| Wall        | archway / hero piece            | 4                    | create_object n_frames=4       | 64x96    | opaque      | Narrative entrance markers               |
| Prop        | column (whole + broken)         | 4-6                  | create_object n_frames=6       | 64x128   | transparent |                                          |
| Prop        | banner (tattered)               | 3-4                  | create_object n_frames=4       | 32x64    | transparent |                                          |
| Prop        | candle / torch                  | 4                    | create_object n_frames=4       | 32x64    | transparent | Static now; Phase J+ VFX for flame anim  |
| Prop        | urn / vase                      | 4-6                  | create_object n_frames=6       | 32x48    | transparent |                                          |
| Prop        | skull pile / bone heap          | 4                    | create_object n_frames=4       | 32x32    | transparent |                                          |
| Prop        | chest (closed + open, 4 tiers)  | 4 tiers x 2 = 8      | create_object n_frames=8       | 32x32    | transparent |                                          |
| Prop        | brazier (lit + extinguished)    | 4                    | create_object n_frames=4       | 32x64    | transparent |                                          |
| Prop        | throne / dais                   | 2-3                  | create_object n_frames=3       | 96x128   | transparent | Boss/elite room anchor                   |
| Prop        | sarcophagus                     | 4                    | create_object n_frames=4       | 64x128   | transparent |                                          |
| Decal       | moss patch                      | 4-6                  | create_object n_frames=6       | 32x32    | transparent | Organic age signal                       |
| Decal       | crack pattern (hairline + wide) | 4-6                  | create_object n_frames=6       | 32x32    | transparent | Damage/history signal                    |
| Decal       | rift glyph (cyan + violet)      | 4-6                  | create_object n_frames=6       | 32x32    | transparent | Magic/rift theme identity                |
| Decal       | blood splatter                  | 4                    | create_object n_frames=4       | 32x32    | transparent | Combat history                           |
| Decal       | water puddle / ichor pool       | 4-6                  | create_object n_frames=6       | 32x32    | transparent | Environmental storytelling               |
| Decal       | dust pile / ash pile            | 4                    | create_object n_frames=4       | 32x32    | transparent | Decay signal                             |
| Decal       | bone fragment scatter           | 4                    | create_object n_frames=4       | 32x32    | transparent | Death signal                             |
| Decal       | scorch mark                     | 4                    | create_object n_frames=4       | 32x32    | transparent | Fire/magic damage history                |
| Decal       | footprint trail                 | 4                    | create_object n_frames=4       | 32x32    | transparent | Subtle narrative (someone was here)      |
| Decal       | summoning circle fragment       | 4                    | create_object n_frames=4       | 64x64    | transparent | Boss/ritual room emphasis                |

**Total asset count: ~160-180 individual sprites**
**PixelLab generation estimate: ~25 batches x 25 gen/batch = ~625 generations (budget remaining: 2265/5000 as of S98, well clear)**

---

## Section 3 -- Combinatorial Scaling Math

A single 16x12 room (192 tiles) can be divided into 4 material zones. Each zone draws from a pool of floor tile variants. On top of that, 8 prop placement slots and 12 decal overlay slots each draw from categorized pools independently.

Example room with conservative pool sizes:
- 10 floor variants per material type (granite, rubble, walkway, rift)
- 8 prop variants per prop slot (which prop + which variant of that prop)
- 6 decal variants per decal category slot

Mathematical ceiling: 4 material zone combinations x C(8,8) prop draw x C(6,12) decal draw = far exceeds 100,000 distinct arrangements before spatial rotation/mirroring.

Practical reality (aesthetically meaningfully distinct rooms): picking just material zone ratio shifts (e.g. 80% granite/20% rift vs 40%/60%) combined with swapping 3-4 props produces a room that reads as a new space. Conservative estimate: **50+ aesthetically distinct rooms per Act using the same ~160 assets**, scaling to all 4 Acts by swapping base palette and material theme (Act 1 = granite/rift, Act 2 = different biome, etc.) with a single style-prefix change in the PixelLab batch prompts.

---

## Section 4 -- ChatGPT Mockup Decomposition (10 Room Concepts)

Reference images: `STAGING/CHATGPT_TOPDOWN/*.png` -- style reference only, not final assets.

### 4.1 Crypt of Forgotten Kings
- **Material zones:** 70% granite (aged, worn), 20% rubble (collapsed sections along walls), 10% rift-scarred (center ritual area)
- **Props:** 4x sarcophagus (corners + center), 2x column-broken (flanking entrance), 1x throne/dais (north wall), 2x candle (bracketing throne)
- **Decals:** bone fragment scatter (dense, center+south), rift glyph (sparse, ritual zone center), moss patch (medium, wall edges), crack pattern (medium, floor near rubble zones)
- **Lighting:** 2x point lights warm amber near candles, 1x cold cyan fill at ritual zone, low ambient
- **Mood:** solemn, ancient, undisturbed power

### 4.2 Broken Forge
- **Material zones:** 60% walkway (flagstone work floor), 30% rubble (collapsed roof debris), 10% scorch-adjacent (near forge sites, use rift-scarred with warm tint override)
- **Props:** 2x brazier-lit (forge stations), 1x urn-cluster (near supply wall), 2x chest-closed (storage), 4x column-broken (structural collapse)
- **Decals:** scorch mark (dense, forge quadrant), ash pile (medium, surrounding forge), crack pattern (medium, load-bearing zones), dust pile (sparse, corners)
- **Lighting:** 3x point lights warm orange/red at forge sites, cool shadows elsewhere, high contrast
- **Mood:** abandoned industry, residual heat, dangerous footing

### 4.3 Ruined Apothecary
- **Material zones:** 50% walkway (shelving floor), 30% granite (structural base), 20% rubble (explosion/collapse sites)
- **Props:** 4-6x urn/vase (varied sizes, shelves implicit via placement rows), 2x banner-tattered (north wall), 2x skull pile (research detritus), 1x chest-open (looted)
- **Decals:** ichor pool (medium, 2-3 spill sites), moss patch (dense, south wall -- damp), bone fragment scatter (sparse), footprint trail (sparse, 1 trail from entrance)
- **Lighting:** 1x cold daylight shaft (skylight, top-center), 2x candle warmth (near worktables), desaturated fill
- **Mood:** former scholarship, organic decay, something went wrong

### 4.4 Drowned Chapel
- **Material zones:** 40% granite, 40% walkway (raised dry areas), 20% rift-scarred (flooded channels -- represent water as separate decal layer over dark tile)
- **Props:** 2x column-whole (nave flanking), 1x archway (altar north), 2x candle-extinguished (altar), 1x sarcophagus (altar base)
- **Decals:** water puddle/ichor pool (dense, low areas), moss patch (dense, all surfaces), crack pattern (medium), blood splatter (sparse, 1 incident site)
- **Lighting:** 1x cool blue-white shaft from archway, 2x dim candle remnants (orange barely visible), wet surface reflections (material property not decal)
- **Mood:** faith submerged, water rising, sacred space violated

### 4.5 Prison Block
- **Material zones:** 60% granite (heavy stone), 30% walkway (guard corridor), 10% rubble (cell collapses)
- **Props:** 2x column-broken (cell division), 4x skull pile (former occupants), 1x chest-locked (warden loot), 2x banner-tattered (old insignia)
- **Decals:** blood splatter (medium, multiple incident sites), crack pattern (dense, cell walls), bone fragment scatter (dense, cells), footprint trail (medium, guard route)
- **Lighting:** 3x point lights cold blue-white (torch sconces, mostly extinguished), deep shadow fills, high darkness
- **Mood:** punishment, forgotten, evidence of struggle

### 4.6 Rift Vault
- **Material zones:** 30% granite, 30% walkway, 40% rift-scarred (dominant -- this is the rift source room)
- **Props:** 4x column-whole (ritual circle corners), 1x sarcophagus (sealed containment center), 4x brazier-lit (cyan-tinted fire, recolor), 2x banner-rift-sigil (reuse tattered banner with glyph decal layered)
- **Decals:** rift glyph (dense, floor mandala layout), summoning circle fragment (1x center large), ichor pool (cyan-tinted, sparse), crack pattern (medium -- rift pressure)
- **Lighting:** 4x point lights cyan #5DEFFF at braziers, violet accent #8B5CF6 at rift center, no warm tones
- **Mood:** contained power, wrong science, imminent breach

### 4.7 Overgrown Garden (interior courtyard)
- **Material zones:** 40% walkway (paths), 30% rubble (overgrown beds), 30% granite (structural perimeter)
- **Props:** 2x column-whole (archway remnants), 4x urn/vase-broken (planters), 1x throne (stone seat, nature-reclaimed)
- **Decals:** moss patch (dense, all non-path surfaces), footprint trail (sparse, path center), water puddle (medium, low spots), bone fragment scatter (sparse, hidden under moss)
- **Lighting:** 1x diffuse cool daylight (overhead, open sky), 2x warm amber highlights (west wall sunset simulation), green tint ambient from foliage color
- **Mood:** nature reclaiming, quiet, deceptive peace before encounter

### 4.8 Shattered Library
- **Material zones:** 50% walkway (reading floor), 30% rubble (collapsed shelving), 20% granite (structural)
- **Props:** 2x column-broken (shelf columns), 6x urn/vase (scroll containers, used as book-pile stand-in until dedicated prop), 2x chest-open (looted archives), 2x candle-extinguished
- **Decals:** ash pile (medium -- burned texts), dust pile (dense -- everything coated), crack pattern (medium), footprint trail (medium, research trails), rift glyph (sparse, 1-2 -- someone was studying rifts)
- **Lighting:** 2x cool daylight shafts (broken roof), 2x dim candle remnants, high dust particle ambient (VFX future phase)
- **Mood:** lost knowledge, urgency of what was studied, forbidden research

### 4.9 Council Chamber
- **Material zones:** 60% walkway (formal floor), 25% granite (raised dais perimeter), 15% rift-scarred (where a rift opened during last council)
- **Props:** 1x throne/dais (head of table, north), 4x column-whole (chamber corners), 4x banner-tattered (council houses, each unique variant), 2x brazier-extinguished (ceremonial)
- **Decals:** blood splatter (medium -- something ended violently here), rift glyph (sparse -- intrusion point), crack pattern (medium), footprint trail (sparse, last movements)
- **Lighting:** 4x point lights cold gray-white (ceiling sconces, mostly dead), 1x cyan accent at rift scar, dramatic shadows
- **Mood:** authority collapsed, betrayal, historical weight

### 4.10 Throne Antechamber
- **Material zones:** 50% walkway (formal approach), 30% granite (flanking), 20% rift-scarred (throne corruption)
- **Props:** 1x throne/dais (dominant, north center), 4x column-whole (grand flanking), 4x sarcophagus (fallen guard/honor guard), 2x banner-tattered (royal colors, faded), 4x candle-extinguished (ceremonial line)
- **Decals:** summoning circle fragment (1x center-floor, large), rift glyph (medium, throne approach), bone fragment scatter (medium), scorch mark (sparse, near throne -- power discharge), crack pattern (medium)
- **Lighting:** 1x dramatic cold shaft (throne spotlight, overhead), 4x dim amber remnants (candles), cyan creep from throne rift scar, very high contrast
- **Mood:** fallen majesty, corrupted power, boss prelude

---

## Section 5 -- PixelLab Prompt Batches (Ready for User Web UI)

Style prefix for ALL prompts (copy to each):
```
Pixel art, RIMA Shattered Keep aesthetic, muted desaturated palette base #2A2D34 + cyan rift accent #5DEFFF, painterly matte not flat, weathered field-worn aged surfaces, no decorative excess, near-pure top-down view approximately 85-90 degree camera, no character no UI.
```

CRITICAL per-type flags (must set in PixelLab UI, not in text prompt):
- Floor tiles: `tile_view_angle: 90`, `tile_depth_ratio: 0` (prevents S98 modular_v1 edge-band bug)
- Props/Decals: transparent background, direction = None

---

### FLOOR BATCH 1 -- Granite Expand
**Tool:** create_tiles_pro | **Canvas:** 64x64 | **tile_view_angle:** 90 | **tile_depth_ratio:** 0 | **BG:** opaque
```
Pixel art top-down floor tile, ancient granite flagstone, Shattered Keep dungeon floor, heavy stone slabs with worn mortar lines, subtle surface variation (chips, age pitting, minor discoloration), muted desaturated gray-brown palette base #2A2D34, painterly matte texture not flat fill, no cracks (separate decal layer), near-seamless tile edges, 4x4 batch of variants.
```

---

### FLOOR BATCH 2 -- Rubble Expand
**Tool:** create_tiles_pro | **Canvas:** 64x64 | **tile_view_angle:** 90 | **tile_depth_ratio:** 0 | **BG:** opaque
```
Pixel art top-down floor tile, collapsed rubble and stone debris field, dungeon floor with broken stone pieces, irregular loose rock surfaces, dust and gravel fill between chunks, muted gray palette, readable overhead silhouette for gameplay clarity, painterly matte, near-seamless, 4x4 batch of variants.
```

---

### FLOOR BATCH 3 -- Walkway (Stone Slab)
**Tool:** create_tiles_pro | **Canvas:** 64x64 | **tile_view_angle:** 90 | **tile_depth_ratio:** 0 | **BG:** opaque
```
Pixel art top-down floor tile, formal cut stone walkway, rectangular flagstone slabs in organized grid pattern, worn smooth by foot traffic, subtle grout lines, slightly lighter tone than granite (formal/constructed feel), muted warm-gray palette, painterly matte surface texture, near-seamless edges, 4x4 batch of variants.
```

---

### FLOOR BATCH 4 -- Rift-Scarred
**Tool:** create_tiles_pro | **Canvas:** 64x64 | **tile_view_angle:** 90 | **tile_depth_ratio:** 0 | **BG:** opaque
```
Pixel art top-down floor tile, dungeon floor cracked open by rift energy, deep fissures with cyan glow (#5DEFFF) emanating from within, surrounding stone darkened and warped, rift veins spreading across tile surface, base granite material showing rift corruption, muted dark palette with controlled cyan accent, painterly matte, near-seamless, 4x4 batch of variants.
```

---

### WALL BATCH 1 -- Straight Set (N/S/E/W x 4 variants)
**Tool:** create_object | **Canvas:** 64x64 | **n_frames:** 16 | **BG:** opaque | **direction:** None
```
Pixel art dungeon wall segment, pure top-down 85-90 degree overhead view, heavy ancient stone wall with low-profile depth shadow cast downward, worn rough stone surface texture, mortar visible between blocks, muted gray-brown palette #2A2D34, painterly matte not flat, no decorative carvings, 16 frames total: 4 directional variants (N S E W) x 4 style variants (clean / moss-stained / cracked / scorched), each frame 64x64.
```

---

### WALL BATCH 2 -- Corner Set (4 outer + 4 inner)
**Tool:** create_object | **Canvas:** 64x64 | **n_frames:** 8 | **BG:** opaque | **direction:** None
```
Pixel art dungeon wall corner piece, pure top-down 85-90 degree view, heavy stone corner with correct overhead shadow, matches straight wall texture (worn ancient stone, muted gray-brown, painterly matte), 8 frames total: 4 outer convex corners + 4 inner concave corners, each with matching material to straight wall set, 64x64 per frame.
```

---

### WALL BATCH 3 -- Archway / Hero Piece
**Tool:** create_object | **Canvas:** 64x96 | **n_frames:** 4 | **BG:** opaque | **direction:** None
```
Pixel art dungeon archway entrance piece, pure top-down 85-90 degree view, grand carved stone arch with visible overhead keystone, slight elevation depth shadow, ancient runic border worn into stone, muted dark palette with optional single cyan rift accent glyph on keystone, painterly matte, 4 variants (intact / cracked / rift-marked / mossy), 64x96 per frame.
```

---

### PROP BATCH 1 -- Column (Whole + Broken)
**Tool:** create_object | **Canvas:** 64x128 | **n_frames:** 6 | **BG:** transparent | **direction:** None
```
Pixel art dungeon column, pure top-down 85-90 degree overhead view, heavy ancient stone pillar, circular cross-section visible from above, shadow cast radially downward onto floor, muted gray palette, painterly matte stone texture, weathered field-worn aged surfaces, 6 variants: 2 intact columns (clean/mossy) + 2 broken-top columns (sheared fracture) + 2 rubble base (column collapsed), 64x128 per frame, transparent background.
```

---

### PROP BATCH 2 -- Banner (Tattered)
**Tool:** create_object | **Canvas:** 32x64 | **n_frames:** 4 | **BG:** transparent | **direction:** None
```
Pixel art dungeon wall banner, tattered torn fabric hanging from iron rod, pure top-down 85-90 degree view showing banner face as overhead plane, faded heraldic device barely visible, torn ragged edges, muted desaturated fabric colors (4 variants: dark red / dark blue / black / rift-marked with cyan glyph), weathered field-worn texture, painterly matte, transparent background, 32x64.
```

---

### PROP BATCH 3 -- Candle / Torch
**Tool:** create_object | **Canvas:** 32x64 | **n_frames:** 4 | **BG:** transparent | **direction:** None
```
Pixel art dungeon candle, pure top-down 85-90 degree view, wax column in iron holder viewed from above, melted wax pooled around base, static flame stub visible as small bright pixel cluster at top (no animation this pass), 4 variants: tall-lit / short-lit / extinguished-fresh / extinguished-old (cold wax), warm amber palette for lit (#FFB347 accent), muted gray-brown for extinguished, transparent background, 32x64.
```

---

### PROP BATCH 4 -- Urn / Vase
**Tool:** create_object | **Canvas:** 32x48 | **n_frames:** 6 | **BG:** transparent | **direction:** None
```
Pixel art dungeon storage urn, pure top-down 85-90 degree overhead view showing circular vessel opening from above, thick stone or clay walls visible as ring, aged cracked glaze, some sealed some broken, 6 variants: large sealed / large cracked-open / medium sealed / medium broken / small sealed / small shattered-spill, muted earth palette, painterly matte, weathered surfaces, transparent background, 32x48.
```

---

### PROP BATCH 5 -- Skull Pile / Bone Heap
**Tool:** create_object | **Canvas:** 32x32 | **n_frames:** 4 | **BG:** transparent | **direction:** None
```
Pixel art dungeon skull pile, pure top-down 85-90 degree overhead view, cluster of humanoid skulls and bones viewed from above, muted aged bone white (#D4C89A), shadow pooled under heap, 4 variants: small tidy pile / large chaotic heap / single skull prominent / scattered low pile, painterly matte, no gore excess just archaeological, transparent background, 32x32.
```

---

### PROP BATCH 6 -- Chest (4 Tiers x 2 States)
**Tool:** create_object | **Canvas:** 32x32 | **n_frames:** 8 | **BG:** transparent | **direction:** None
```
Pixel art dungeon loot chest, pure top-down 85-90 degree view showing chest lid face from above, 4 tier quality levels (common wood / uncommon iron-bound / rare ornate / epic glowing rift-marked), 2 states each (closed with latch / open empty), 8 frames total, muted desaturated palette with quality-tier color accent (gray / silver / gold / cyan rift), painterly matte, weathered, transparent background, 32x32.
```

---

### PROP BATCH 7 -- Brazier
**Tool:** create_object | **Canvas:** 32x64 | **n_frames:** 4 | **BG:** transparent | **direction:** None
```
Pixel art dungeon brazier, pure top-down 85-90 degree view, iron bowl on legs viewed from above showing circular fire bowl, 4 variants: lit-warm (orange glow #FF6B35) / lit-rift (cyan glow #5DEFFF, rift-corrupted fire) / extinguished-fresh (ash visible, hot metal) / extinguished-cold (rust, cold ash), static flame (no animation this pass), muted iron-dark base with controlled accent, painterly matte, transparent background, 32x64.
```

---

### PROP BATCH 8 -- Sarcophagus
**Tool:** create_object | **Canvas:** 64x128 | **n_frames:** 4 | **BG:** transparent | **direction:** None
```
Pixel art dungeon sarcophagus, pure top-down 85-90 degree overhead view, stone burial vessel with carved lid seen from above, lid face visible as main face, heavy stone texture, carved relief barely visible (worn), 4 variants: sealed-intact / sealed-cracked / open-empty / open-disturbed (lid half-aside), muted gray stone palette, painterly matte, aged weathered surfaces, transparent background, 64x128.
```

---

### DECAL BATCH 1 -- Moss Patch
**Tool:** create_object | **Canvas:** 32x32 | **n_frames:** 6 | **BG:** transparent | **direction:** None
```
Pixel art floor decal, moss and lichen growth patch, pure top-down overhead view, organic spreading shape, muted desaturated green-gray (#556B47 range), thin wispy edges fading to transparent, 6 variants by size and density (small sparse / small dense / medium spreading / medium dense / large irregular / wall-edge strip), painterly matte organic texture, transparent background, 32x32.
```

---

### DECAL BATCH 2 -- Crack Pattern
**Tool:** create_object | **Canvas:** 32x32 | **n_frames:** 6 | **BG:** transparent | **direction:** None
```
Pixel art floor decal, stone crack pattern, pure top-down overhead view, branching fracture lines in floor stone, 6 variants: hairline-single / hairline-branching / wide-single / wide-branching / impact-radial / rift-crack (faint cyan trace in crack, #5DEFFF very low opacity edge), dark gray-brown crack color on transparent field, painterly, transparent background, 32x32.
```

---

### DECAL BATCH 3 -- Rift Glyph
**Tool:** create_object | **Canvas:** 32x32 | **n_frames:** 6 | **BG:** transparent | **direction:** None
```
Pixel art floor decal, rift energy glyph burned into stone, pure top-down overhead view, angular arcane symbol, 6 variants: small simple (cyan #5DEFFF) / medium compound (cyan) / large complex (cyan) / small faded (violet #8B5CF6) / medium split (half cyan half violet) / large full ritual mark, glowing paint-on-stone appearance, faint edge bloom, painterly matte, transparent background, 32x32.
```

---

### DECAL BATCH 4 -- Liquid (Blood + Water + Ichor)
**Tool:** create_object | **Canvas:** 32x32 | **n_frames:** 6 | **BG:** transparent | **direction:** None
```
Pixel art floor decal, liquid pool and splash, pure top-down overhead view, 6 variants: blood splatter large (#8B1A1A muted dark red) / blood splatter small / water puddle clear (blue-gray) / stagnant ichor pool (murky green-gray) / rift ichor pool (dark with cyan edge #5DEFFF) / mixed spill (water-blood border), irregular organic shapes, painted matte surface tension look, no excessive gore, transparent background, 32x32.
```

---

### DECAL BATCH 5 -- Detritus (Ash + Bone + Dust + Scorch)
**Tool:** create_object | **Canvas:** 32x32 | **n_frames:** 8 | **BG:** transparent | **direction:** None
```
Pixel art floor decal, environmental debris, pure top-down overhead view, 8 variants: ash pile (light gray-white mound) / dust drift (very light, near-invisible edge) / bone fragment scatter (small yellowed chips) / bone fragment dense / scorch mark circular (burn damage, dark char) / scorch mark directional (blast line) / footprint trail short (4-5 prints, humanoid) / footprint trail long (7-8 prints leading direction), muted palettes per type, painterly matte, transparent background, 32x32.
```

---

### DECAL BATCH 6 -- Summoning Circle Fragment
**Tool:** create_object | **Canvas:** 64x64 | **n_frames:** 4 | **BG:** transparent | **direction:** None
```
Pixel art floor decal, arcane summoning circle, pure top-down overhead view, circular ritual marking burned or carved into stone, 4 variants: partial fragment quarter-circle (faded, ancient) / half circle (partially erased) / full circle faded (old ritual) / full circle active (cyan rift glow #5DEFFF, recently used), complex inner geometry of nested rings and angular runes, stone-burn aesthetic not painted-on, large presence 64x64, transparent background.
```

---

## Section 6 -- Implementation Order (Post-Generation)

Production priority order for maximum visual unlock per hour:

1. **Floor expand (immediate)** -- 4 floor batches unlock full material zone differentiation across all 6 current rooms. Fastest visual impact per batch generated. Do these first.

2. **Wall set** -- Straight + corner + archway. Replaces any placeholder collision geometry with styled boundary. Required before room screenshots look production-quality.

3. **Primary props** -- Column, banner, candle. These three define room archetype identity faster than any other prop category. A room with columns reads as "castle hall"; without, it reads as "box".

4. **Decal layer** -- Moss + crack + glyph. Decals turn a "placed props" layout into a "lived-in dungeon". Add after primary props to see full effect delta.

5. **Secondary props** -- Urn, skull, chest, brazier. Fill density and loot-room identity signals.

6. **Narrative props** -- Throne, sarcophagus, archway hero piece. Low frequency (1-2 per room max) but high impact for boss/elite zones.

7. **Remaining decals** -- Blood, water, ash, bone, scorch, footprint, summoning circle. Polish layer; apply last for final atmosphere tuning.

---

## Section 7 -- Decal Categorical Defense ("Mantikli Uretim")

Why decal production is categorized by function rather than generated randomly:

**Organic decay (moss / vine):** Signals age and abandonment. Rooms with heavy organic growth read as "untouched for centuries." Rooms with none read as "recently active." This ratio carries direct lore communication without dialogue. Production rule: moss goes on floor+wall edges in damp/sealed rooms; absent in forge/active rooms.

**Physical damage (crack / scorch):** Carries combat and event history. A cracked floor near a sarcophagus suggests the inhabitant tried to escape. Scorch near a doorway suggests a last stand. These decals do environmental storytelling work that saves designer time writing lore texts. Production rule: cracks = structural age or impact; scorch = fire/magic discharge, placed with intent not randomly.

**Magic markers (rift glyph / summoning circle):** Connects floor space to the rift narrative and class identity system. A room with summoning circle fragments signals "ritual site, something was called here." Rift glyphs signal rift energy progression toward Act 1 boss. Production rule: used sparsely (1-3 per room) and only in rift-connected rooms to preserve narrative weight.

**Liquid evidence (water / blood / ichor):** Water pools = environmental hazard potential and dungeon hydrology (believability). Blood = specific violence occurred here, signals enemy encounter history. Ichor = non-human entity, signals monster presence or corruption. Each type carries different inference. Production rule: liquid types must not be mixed without intent; a puddle of blood near a sarcophagus is narrative; same puddle in an empty corridor is noise.

**Detritus signals (bone / ash / dust / footprint):** Fine-grain inhabitation markers. Bone scatter = death density (how many died here). Ash = fire event or magical decay. Dust = time depth (thick dust = very old). Footprints = recent activity, creates implied threat (something passed through). Production rule: footprint trails are the highest-value decal because they imply present or near-present danger; use 1 per room maximum, always leading toward an encounter zone.

**Ritual emphasis (summoning circle fragment):** Full 64x64 canvas differentiates this from small decals. Reserved for rooms with explicit ritual narrative function. Not scattered. 1 per ritual room, centered or near altar prop. The "fragment" variants allow rooms where the ritual was interrupted or partially erased, adding lore nuance.

Result: every decal placed is defensible by category function. No random placement. JSON room definitions will specify decal type + density class (sparse/medium/dense) + placement zone (center / wall-edge / doorway / prop-adjacent). This is "mantikli uretim" -- production that serves design intent.

---

## Section 8 -- User Web UI Workflow (Post-Session)

Step-by-step for full ~160 asset run (~3-4 hours total):

1. Open `pixellab.ai` in browser, log in with account.

2. Select tool:
   - Floor tiles: use **Tiles Pro** tool
   - Props and Decals: use **Object** tool

3. For each **Floor batch** (Batches 1-4):
   - Set tile_view_angle = **90** (critical -- prevents edge band bug from S98)
   - Set tile_depth_ratio = **0**
   - Set canvas = **64x64**
   - Set background = **opaque**
   - Set n_tiles = **4x4** (16 tiles per batch)
   - Paste prompt from Section 5
   - Generate, preview, download full batch
   - Import to: `Assets/Art/Tiles/Act1_ShatteredKeep/[subtype]/`

4. For each **Wall batch** (Batches 5-7):
   - Set canvas per table (64x64 or 64x96)
   - Set background = **opaque**
   - Set direction = **None**
   - Set n_frames per batch spec
   - Paste prompt from Section 5
   - Generate, download
   - Import to: `Assets/Art/Props/Act1_ShatteredKeep/Walls/`

5. For each **Prop batch** (Batches 8-14):
   - Set canvas per table
   - Set background = **transparent** (critical)
   - Set direction = **None**
   - Set n_frames per batch spec
   - Paste prompt from Section 5
   - Generate, download
   - Import to: `Assets/Art/Props/Act1_ShatteredKeep/[prop_subtype]/`

6. For each **Decal batch** (Batches 15-20):
   - Set canvas per table (32x32 or 64x64 for summoning circle)
   - Set background = **transparent** (critical)
   - Set direction = **None**
   - Set n_frames per batch spec
   - Paste prompt from Section 5
   - Generate, download
   - Import to: `Assets/Art/Decals/Act1_ShatteredKeep/[decal_subtype]/`

7. After each import group: run Unity **Refresh** (Ctrl+R in editor), verify sprite slicing at PPU=64, confirm transparent BG assets do not have white fringe (check Texture Import settings: Alpha is Transparency = ON).

8. Register each new asset batch in `Assets/Data/Props/Act1_ShatteredKeep/` corresponding `.json` catalog file so RoomLoader can reference by ID.

Estimated time: Floor batches (~30 min) + Wall batches (~30 min) + Prop batches (~90 min) + Decal batches (~60 min) + Unity import+verify (~30 min) = ~4 hours total.

---

## Section 9 -- Open Questions (Decide Before Production)

**Q1: Wall depth representation**
Should walls be rendered as flat low-profile silhouettes (pure top-down, no visible upper edge) or include a subtle 1-2 pixel depth edge on south face to hint at wall height? Option A (flat) is cleanest for pure 90-degree gameplay; Option B (subtle depth edge) adds readability at cost of slight angle inconsistency. Recommendation: prototype both in Phase B test room, decide before committing to wall batch generation. Lock into wall prompt before generating.

**Q2: Decal animation -- static vs breathing**
Rift glyphs and summoning circles: generate as static sprites now, or design for future pulsing animation (Phase J+ VFX layer)? Recommendation: static this pass. Design the sprite so that the glow area can be isolated as a separate layer later if Phase J adds VFX -- meaning glyph base + glow are distinguishable in the sprite (glow stays in a consistent pixel cluster). Mark this as Phase J candidate in VFX backlog.

**Q3: tile_view_angle 90 vs 80 trade-off**
90 = perfectly flat, no thickness, no edge band (confirmed correct per S98 modular_v1 lesson). 80 = 2-3 pixel subtle depth hint on tile north edge. The S98 lesson was that unintended depth causes visible seam bands. However, 80 might be acceptable if applied uniformly. Decision: **lock 90** for this production run. If a future playtest shows floor tiles read as too flat, revisit with a dedicated test batch at 80 before changing all tiles.

**Q4: Act 2-4 palette adaptation**
This pipeline document is written for Act 1 (granite/rift, #2A2D34 base). For Acts 2-4, the same taxonomy and tool settings apply -- only the style prefix palette description changes. Plan: when Act 2 biome is locked by design, write a `asset_combination_pipeline_v2_act2.md` with updated style prefix. All section structure reuses exactly.

**Q5: RoomLoader JSON schema**
Before decal placement becomes automated, the RoomLoader JSON schema needs a `decals` array with fields: `decal_id`, `placement_zone`, `density_class`, `position_override (optional)`. This is a Codex task (Unity side), not an asset task. Flag for Codex delegation when decal assets are ready.

---

*End of document. Next session: confirm Q1 wall depth decision, then begin floor batch generation in PixelLab web UI.*
