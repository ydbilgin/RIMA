# Codex Task — Image Gen: RIMA Min-Pack Sample Dungeon Concept

## Amaç

Codex imagegen skill (gpt-image-1) ile **Hades-iso Act 1 Shattered Keep dungeon concept screenshot** üret. Asset pack proposal `STAGING/codex_dungeon_asset_pack_proposal.md` (24 sprite, Room A combat 16×12) görsel kanıtı. User'a paslanır → onay sonrası PixelLab production order netleşir.

**NOT asset.** Concept reference PNG only.

---

## Output

`STAGING/concepts/dungeon_concept_minpack_combat_v1.png`

After generation, also write `STAGING/concepts/dungeon_concept_minpack_combat_v1_NOTES.md`:
- Asset breakdown — her görsel element hangi proposal asset (F01-F06 / W01-W05 / P01-P07 / D01-D05 / H01)
- PixelLab production verdict — hangi sprite öncelikle üretilmeli + tahmini neden
- Visual quality gap — referans seviyesi (ChatGPT_TOPDOWN) ile karşılaştırma

---

## Reference materials (read before image gen)

- `STAGING/CHATGPT_TOPDOWN/*.png` (6 mockup, ChatGPT'nin RIMA için yaptığı concept) — **tilt + atmosphere + lighting reference target**
- `STAGING/codex_dungeon_asset_pack_proposal.md` Section 4 Room A JSON — **layout spec**
- `STAGING/asset_production_master_plan_v3.md` Section 7 — Image #3/4/5 attributes
- `Assets/Screenshots/Phase_K_room_2_west_chamber.png` — **current state baseline** (zayıf, ne ürettiğimiz vs hedef fark)
- Image #3 ref (önceden user share, Çatlak Mezarlık combat sahne) — **ana visual target**

---

## Hedef görsel — Room A "Combat - Broken Slab Hall"

### Composition
- Pixel-art game screenshot, **Hades-iso ~70-75° tilt** (camera near pure top-down with slight angle — NOT Hyper Light flat top-down, NOT side view). Walls visible front-face. Floor flat textured.
- 16×12 tile room (Unity-size, but render at concept-art polish, NOT raw 16×12 pixel grid)
- 16:9 aspect ratio composition
- Player character (Warblade or similar 8-dir chibi) **center**, slightly toward south (player just entered from south door, about to face north combat)
- 3 enemies scattered: 1 close left, 1 mid-right, 1 far north (different distances for combat tension feel)

### Floor (F01-F06)
- Default: **F01/F02 granite slab** (dark slate gray, weathered, asymmetric crack patterns)
- North-east zone 4×3: **F05 cracked rubble** (debris scatter, slightly lighter brown-gray, broken stones)
- Vertical strip x=6 to x=10: **F03/F04 walkway trim** (worn lighter stone path, foot-traffic smoothing)
- One cell x=8 y=6: **F06 cyan rift accent** (cyan hairline glow crack, RGB #5DEFFF)
- NO visible square tile grid — material zones blend organically through scattered decals + decals + transition softness
- Subtle moss tufts at wall edges (D01)
- Hairline cracks scattered (D02)
- Blood stain center-mid (D03, dark crimson ritual scar)
- Dust film at NE corner (D04, faint pale gray patches)

### Walls (W01-W05)
- North wall (top of screen): **W01 wall_straight_n × 3 segments** continuous edge, front-face visible, ~64-96px height, top-down perspective showing upper-face at slight angle
- East + West walls: **W02 wall_straight_e** (E native), **W02 mirrored** (W via flipX)
- 4 corners: **W03 corner_outer** at NE/NW/SE/SW (one native + 3 rotation/flip variants)
- One **W05 collapsed_stub** at east side (broken half-wall, partial blocker)
- South wall has **gap for door** (player just entered)
- Walls have moss creep, hairline cracks, weathered stone texture
- NO visible Wang seam between wall segments

### Props (P01-P07)
- **P01 round_column** at x=4 y=4 (intact stone column, 64×96, vertical, weathered)
- **P02 broken_column** at x=11 y=7 (collapsed, flipX, leaning, dust at base)
- **P03 tattered_banner** hanging on north wall at x=7 (faded crimson cloth, torn edges)
- **P04 wall_torch** × 2: x=2 y=10 (left wall) + x=14 y=10 (right wall, flipX). Burning warm orange fire emitting halo glow
- **P06 urn_cluster** at x=2.5 y=2 (3-4 grouped weathered clay urns, NE area)
- **P07 rubble_pile** at x=12 y=2 (broken stone debris pile, blocker visual)

### Lighting (CRITICAL — atmosphere)
- Global ambient: **dark deep teal-blue #20242A**, intensity 0.65 (dim, dungeon mood)
- 2 warm point lights at torches: #FFA060, intensity 1.1, flicker, radius 4 (orange halos on floor + wall adjacency)
- 1 cool cyan point light at rift accent x=8 y=6: #5DEFFF, intensity 0.8, radius 4 (cold magic glow)
- **Dual-tone contrast** = Hades signature: warm corners + cool center
- Soft fall-off from light sources, gradient on floor (not hard pixel edges for light)
- Subtle rim light on character (cool blue from rift glow side)

### Character (Warblade)
- Center, slight south (y=3 area), facing north
- Chibi 3-4 head tall, big-head readable face
- 3/4 view (8-dir sprite style, current Warblade reference)
- Pose: slight forward stance, ready
- Sprite ~1/12 of room height (chibi proportion)

### Enemies (placeholder — visual variety only, not final sprite)
- 1 humanoid skeletal/hooded combatant at x=5 y=6 (close left, ready to engage)
- 1 armored figure at x=11 y=6 (mid-right, holding weapon)
- 1 distant scout at x=12 y=10 (north area, far away — depth cue)
- All chibi same scale as Warblade
- Each with thin red HP bar overhead (1px outline, optional)

### Style
- **Pixel art** aesthetic but at **concept-art resolution** (not raw 16×12 pixel grid — composition LOOKS like in-game view with concept art polish)
- Hard pixel-ish edges on props/character; soft organic blending on light halos, moss patches, rift stains
- Painterly pixel art — visible pixel grain but organic shapes
- NO visible square tile grid on floor — tiles BLEND through moss, stains, lighting, decals
- NO UI elements, NO HUD, NO health bars (except enemy thin red), NO inventory, NO text labels
- 16:9 ratio

### Mood — "Vivid Vulnerability + Ritual Catastrophe"
- Dark gritty atmospheric — NOT sterile, NOT clean, NOT cartoon-cute
- Faded earth tones + cyan rift accent
- Dominant: dark slate gray (#2A2D34), deep brown shadows
- Accent: warm orange (#FFA060) torches, cyan rift (#5DEFFF), faint dark red blood
- Post-battle ancient hollow watchful — Alabaster Dawn + Hades + Children of Morta fusion

### Negative directives
- NO visible grid tiles, NO obvious square floor borders
- NO 3D render, NO photorealistic
- NO modern UI elements
- NO bright cartoon palette
- NO side view, NO pure 90° flat top-down — must be ~70-75° Hades tilt

---

## Tool

Use Codex imagegen built-in skill (gpt-image-1 force) — same pattern as `STAGING/codex_imagegen_organic_room.md`. Output PNG at concept art quality.

If imagegen fails or quality not matching reference target, regen up to 2 times with prompt refinement.

Save to: `STAGING/concepts/dungeon_concept_minpack_combat_v1.png`

---

## After image gen — write NOTES.md

`STAGING/concepts/dungeon_concept_minpack_combat_v1_NOTES.md` should include:

### Asset breakdown table
| Visual element in mockup | Asset proposal ID | Native canvas | Production method | Pre-production note |
|---|---|---|---|---|
| North wall segments × 3 | W01 | 64×96 | create_object n_frames=16 | Verify tile-mate left-right edges |
| East wall | W02 | 64×96 | create_object | Will be flipX'd for west |
| ... (all visible elements) | | | | |

### PixelLab production order verdict
After seeing the mockup composition, which sprite should be produced FIRST? Justify.

### Visual gap assessment
Compare mockup quality vs Phase K screenshots vs ChatGPT_TOPDOWN reference:
- Tile read: PASS/TWEAK/FAIL
- Wall height + perspective: PASS/TWEAK/FAIL
- Lighting dual-tone: PASS/TWEAK/FAIL
- Prop density: PASS/TWEAK/FAIL
- Character scale: PASS/TWEAK/FAIL
- Overall "Hades match": PASS/TWEAK/FAIL

### Recommendation
- Mockup quality enough to proceed PixelLab production? YES/NO
- If NO, what to regen or adjust in concept

---

## Commit
```
[Codex] [S98 IMAGEGEN] Min-pack dungeon concept Room A — Combat Broken Slab Hall

- Codex imagegen (gpt-image-1) concept screenshot of asset pack proposal Room A
- 16×12 combat room composition, Hades-iso ~70-75° tilt
- 24-sprite asset pack visualized in single scene
- PNG: STAGING/concepts/dungeon_concept_minpack_combat_v1.png
- NOTES: STAGING/concepts/dungeon_concept_minpack_combat_v1_NOTES.md
```

## Wall clock
~10-15 min (image gen + notes).
