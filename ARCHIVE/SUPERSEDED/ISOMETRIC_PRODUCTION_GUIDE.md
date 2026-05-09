# RIMA -- Isometric Tile & Asset Production Guide
**Date:** 2026-05-02 | **Status:** CANONICAL | **Applies to:** S43 tile/environment production

---

## 1. Perspective Convention

RIMA uses **true 2:1 isometric** projection:
- 2 pixels wide : 1 pixel tall on diamond floor tiles
- Camera axis: 45 deg horizontal rotation, ~30 deg vertical tilt (Hades / Diablo standard)
- Light source: **top-left** (universal isometric convention -- never change)

### Tile Grid

| Asset | Canvas (px) | Notes |
|---|---|---|
| Floor tile | 64 x 32 | Standard 2:1 isometric diamond |
| Wall segment | 64 x 96 | Same footprint as floor, 3x height for face + edge |
| Character | 128 x 128 | "Low Top-Down" camera in PixelLab CFR = correct for this projection |
| Props / objects | 64 x 64 or 64 x 96 | Depends on object height |

Characters generated with "Low Top-Down" camera in PixelLab are **already isometric-compatible** -- no re-generation needed.

---

## 2. Project Cleanup First (Before Any Generation)

Before generating new assets, Codex must clean Unity project state:

### Remove
- All placeholder GameObjects in `Assets/Scenes/_IsoGame.unity` (cubes, capsules, debug objects)
- `Assets/Scripts/UI/CharacterSelectScreen.cs` -- replace with proper scene-based implementation later
- Any `_test` or `_placeholder` tagged prefabs in Assets/Prefabs/

### Keep
- All idle sprites and .anim files in `Assets/Animations/Characters/`
- All .controller files in `Assets/Animations/Characters/`
- `Assets/Scripts/Player/PlayerAnimator.cs`
- `Assets/Scripts/Systems/PlayerClassManager.cs`

### Unity Isometric Scene Setup
1. In Unity Editor: Create new Tilemap -> Isometric (not Regular)
   - `GameObject > 2D Object > Isometric Tilemap`
   - Grid component: Cell Layout = Isometric, Cell Size = (1, 0.5, 1)
2. Sorting layer order: Floor → Props → Characters → Walls (front) → UI
3. Enable `Isometric Z As Y` in Project Settings > Graphics > Transparency Sort Mode

---

## 3. Asset Production List (Wave 1 -- Dungeon Tileset)

### Priority 1 -- Floor Tiles (Codex generates these first)
| Asset | Count | PixelLab tool | Notes |
|---|---|---|---|
| Stone floor -- base | 3 variants | `create_isometric_tile` | Main floor, subtle texture variation |
| Stone floor -- cracked | 2 variants | `create_isometric_tile` | Damaged variant |
| Stone floor -- mossy | 1 variant | `create_isometric_tile` | Edge/corner feel |

### Priority 2 -- Wall Tiles
| Asset | Count | PixelLab tool | Notes |
|---|---|---|---|
| Wall segment -- straight | 2 variants | `create_object` (64x96) | Main wall face |
| Wall segment -- cracked | 1 variant | `create_object` | Damaged |
| Wall corner -- inner | 1 | `create_object` | Concave corner |
| Wall corner -- outer | 1 | `create_object` | Convex corner |
| Wall top edge | 1 | `create_isometric_tile` | Top cap tile |

### Priority 3 -- Environment Props
| Asset | Count | PixelLab tool | Notes |
|---|---|---|---|
| Torch (animated later) | 1 | `create_object` | Wall-mounted |
| Barrel | 1 | `create_object` | Destructible prop |
| Crate | 1 | `create_object` | Destructible prop |
| Dungeon door -- closed | 1 | `create_object` | 64x96 |

---

## 4. PixelLab Tool Routing

| Need | Tool | Reason |
|---|---|---|
| Isometric floor tile | `create_isometric_tile` | Purpose-built, handles 2:1 diamond correctly |
| Wall / prop sprites | `create_object` | Non-tile sprites with transparent background |
| Tile variation batches | `create_tiles_pro` | When 4+ variants needed at once |
| Top-down overview map | `create_topdown_tileset` | NOT for main game view -- only for minimap if added |
| Character animations | PixelLab UI manual only | MCP animate forbidden |

**API V2 directly** for all Pro tools -- MCP does not expose Pro endpoints reliably.

---

## 5. Natural Look Rules -- Isometric

### Universal Lighting Model
```
Top face (floor diamond center):  LIGHTEST (+20% brightness from base)
Left-facing surfaces:             BASE tone
Right-facing surfaces:            DARK (-15% from base)
Bottom edges / recesses:          DARKEST (-25% from base)
```
Paste these rules into every tile prompt. Hex palette locks consistency.

### Floor Tile -- Natural Appearance Checklist
- [ ] 4-6 colors max per tile (pixel art budget)
- [ ] Subtle texture: irregular stone shapes, NOT uniform grid lines
- [ ] Slight blue-gray desaturation (dungeon cold atmosphere)
- [ ] No specular highlight -- matte stone surface
- [ ] 3-4 variants minimum -- tiling repetition is the #1 readability killer
- [ ] Add occasional micro-detail: small pebble, dust, crack -- not every tile

### Wall -- Natural Appearance Checklist
- [ ] 3 tonal zones: top edge highlight / face midtone / base shadow
- [ ] Stone block pattern: irregular sizes, NOT uniform bricks
- [ ] Slight moss or moisture stain at base (~20% of height from bottom)
- [ ] No ambient occlusion baked in -- Unity handles depth sorting
- [ ] Wall face and top edge must read as separate surfaces (tone contrast)
- [ ] 2 pixel dark outline on bottom edge only (grounds wall to floor)

### Palette Lock (Dungeon -- Dark Stone)
Apply to ALL dungeon tiles. Pass these HEX codes in every Pro tool prompt:
```
Floor base:   #3a3532
Floor light:  #4d4844
Floor dark:   #2a2522
Wall base:    #403c38
Wall light:   #555048
Wall dark:    #2e2a27
Wall top:     #5e5850
Moss:         #3d4a32
Accent:       #7a6a50  (worn stone / grout lines)
```

---

## 6. Prompt Templates

### Floor Tile Template
```
Isometric pixel art floor tile, 64x32px, 2:1 diamond shape.
[VARIANT DESCRIPTION: e.g. "worn stone dungeon floor, irregular stone slabs"]
Lighting: top-left light source, top face brightest, slight perspective shadow on right edge.
No anti-aliasing, hard pixel edges, no dithering.
Palette: #3a3532 #4d4844 #2a2522 #403c38 #7a6a50
Transparent background.
```

### Wall Segment Template
```
Isometric pixel art wall tile, 64x96px.
[VARIANT DESCRIPTION: e.g. "dungeon stone wall, large irregular blocks, rough mortar"]
3 tonal zones: highlighted top edge, midtone face, shadowed base.
Moss or moisture stain on bottom 20%. 2px dark outline on bottom edge.
Lighting: top-left source.
No anti-aliasing, hard pixel edges, no dithering.
Palette: #403c38 #555048 #2e2a27 #5e5850 #3d4a32 #7a6a50
Transparent background.
```

### Sjalsol SIZE LOCK addition (add to any prompt requiring footprint consistency)
```
SIZE LOCK: character/object must fit within exact [WxH]px bounding box.
FOOTPRINT LOCK: base of object centered on tile anchor point at bottom-center of canvas.
ANCHOR: ground contact at pixel row [H-4], not at canvas bottom edge.
```

---

## 7. Codex Execution Plan (GPT-5.5 xhigh)

### Step 1 -- Project Cleanup (Unity MCP)
```
Task: Remove placeholder content from _IsoGame.unity
- Find all GameObjects not in layer "Characters" or "Persistent"
- Delete primitives (cubes, capsules, spheres) used as placeholders
- Save scene
- Verify no compile errors via read_console
```

### Step 2 -- Isometric Tilemap Setup (Unity MCP)
```
Task: Configure Unity project for isometric tilemap
- Project Settings > Graphics: set Transparency Sort Mode to "Custom Axis", Sort Axis (0, 1, 0)
- Create Tilemap: Grid component, Cell Layout = Isometric, Cell Size = (1, 0.5, 1)
- Add sorting layers: Floor / Props / Characters / WallFront / UI
- Save scene
```

### Step 3 -- Floor Tile Generation (PixelLab API V2)
```
Task: Generate 6 floor tile variants
- Use create_isometric_tile endpoint
- Apply floor template + palette from Section 6
- Variants: stone_base x3, stone_cracked x2, stone_mossy x1
- Save outputs to: Assets/Sprites/Tiles/Floor/
- Name convention: floor_stone_v01.png, floor_stone_v02.png, etc.
- After each batch: git commit with message "Sprites: add floor tile batch N"
```

### Step 4 -- Wall Tile Generation (PixelLab API V2)
```
Task: Generate 6 wall segment variants
- Use create_object endpoint (not create_isometric_tile -- walls are sprites not tiles)
- Apply wall template + palette from Section 6
- Variants: wall_straight x2, wall_cracked x1, wall_corner_inner x1, wall_corner_outer x1, wall_top x1
- Save to: Assets/Sprites/Tiles/Walls/
- Name convention: wall_straight_v01.png, etc.
- git commit after each batch
```

### Step 5 -- Import to Unity
```
Task: Import tile sprites and create Tile Assets
- Set all tile PNGs: PPU=64, Filter=Point, Compression=None, Sprite Mode=Single
- Create Tile assets (.asset) for each sprite via Tile Palette
- Place sample room in scene: 10x8 floor, 1-tile-thick walls, 1 door
- Verify visual output in Scene view
```

---

## 8. Agentic Safety Rules (from Antigravity research)

Apply to Codex at all times:

- **git commit after EVERY batch** -- not after whole step. One failed overnight run = 7 days lost (community incident).
- **No open-ended tasks** -- "generate all tiles" with no list = hallucinated loop risk
- **One asset type per Codex run** -- floor tiles one run, walls next run
- **Verify before next step** -- always check Unity compile + read_console after scene changes
- **Prompt variation** -- change prompt strings slightly between sessions to avoid cached LLM reasoning (community finding)
- **Log lessons** -- Codex writes brief notes on what worked/failed at end of each run

---

## 9. Key Antigravity Research Findings Applied Here

| Finding | Source | Applied Where |
|---|---|---|
| character_id backbone for direction consistency | rorriM | Not for tiles, applies to char animation runs |
| SIZE LOCK + FOOTPRINT LOCK + ANCHOR in prompts | Sjalsol | Section 6 wall/prop templates |
| HEX palette in Pro tool prompts = major consistency gain | PixelLab staff | Section 5 palette block, all templates |
| API V2 directly for Pro tools (MCP unreliable) | Community | Section 4 tool routing |
| Generate without weapon first, add in second pass | Kaninen (staff) | N/A for tiles; applies to char weapon props |
| 5-10 focused style references > 60 random | Staff | Keep style reference count low per batch |
| "walking away from camera" not "walking north" | Staff + community | N/A tiles; applies to char animation prompts |
| Git commit after every agentic change | Community incident | Section 8 safety rules |
| Numbered lists force specific output in prompts | Community | Use for wall variant batches: "1. straight 2. cracked" |
| animate-with-text v3 only, v2 stalls at 49% | Community | N/A tiles; applies when torch anim added later |
| snowli_on 2-step: batch poses then interpolate | Community | Applies to char run cycles, not tiles |

---

## 10. What NOT to Do

- Do NOT use `create_character` for tile generation
- Do NOT call `animate_character` MCP (quality issues, confirmed forbidden)
- Do NOT generate all wall variants in a single API call -- separate calls per variant for quality control
- Do NOT mix character shadows into tile sprites -- keep tiles clean, Unity handles shadows
- Do NOT use `create_topdown_tileset` for main game view -- it produces flat tiles, not isometric
- Do NOT skip git commits between batches

---

## Next Action After Guide Review

1. User confirms guide (or requests changes)
2. Codex run #1: Project cleanup + isometric scene setup
3. Codex run #2: Floor tile generation (6 variants)
4. Codex run #3: Wall generation (6 variants)
5. Codex run #4: Unity import + sample room placement
6. User playtests room layout in Play mode
