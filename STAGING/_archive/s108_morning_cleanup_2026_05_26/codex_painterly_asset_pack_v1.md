# Codex Task — Painterly Asset Pack v1 (RIMA Room Builder)

## STIL HEDEFI (kritik — explicit)

**Reference:** `STAGING/concept_art_rima_sample_room.png` — Codex'in v1 painterly sample room render'ı.

Bu render içindeki HER GÖRSEL ÖĞENİN AYRI BİR ASSET DOSYASI olarak üretilmesini istiyorum. Painterly Hades-tradition, atmospheric, hand-painted-feel pixel art. **NOT hard-pixel discipline.** **NOT geometric Pillow shapes.**

Sample render'daki stil aile bağı:
- Painterly stone weathered surfaces with multi-tone brush strokes
- Atmospheric color depth (deep slate gray + warm brown undertone + dusty blue + moss green + faint rift red)
- Soft pixel edges acceptable (concept_art_v1 style)
- High-detail 128-256px native per element
- Top-down high-angle ~30-35° perspective
- Transparent backgrounds for ALL items

## Hedef Output — 50 sprite painterly asset pack

Tüm asset'ler: `Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/`

### FLOOR TILES (8 variants, 128×128 each)
Klasör: `floor/`

Seamless tileable, transparent edges OK (subtle blend):
1. `floor_01_clean.png` — Clean weathered stone, dark slate dominant, deep brown undertone, slight grain
2. `floor_02_mossy.png` — Stone with sparse moss patch, deep moss green organic
3. `floor_03_cracked.png` — Hairline fractures, darker shadow lines
4. `floor_04_worn.png` — Polished smooth, faint cold blue rim
5. `floor_05_stained.png` — Dusty blue mineral residue, abstract sigil discoloration
6. `floor_06_rift_touched.png` — Cold blue glow at crack edges (rift influence)
7. `floor_07_dirt.png` — Dirt-covered stone, organic earth tones
8. `floor_08_blood_old.png` — Faded blood stain on stone, dark crimson aged

### WALL PIECES (12 variants, 128×192 each — top cap visible)
Klasör: `walls/`

Painterly stone wall blocks with visible top cap, sparse moss at bottom edge, perspective-aware:
1. `wall_01_top_left_corner.png` — Top-left corner piece
2. `wall_02_top_edge.png` — Top edge straight run
3. `wall_03_top_right_corner.png` — Top-right corner
4. `wall_04_left_edge.png` — Left edge vertical
5. `wall_05_right_edge.png` — Right edge vertical
6. `wall_06_bottom_left_corner.png` — Bottom-left corner
7. `wall_07_bottom_edge.png` — Bottom edge
8. `wall_08_bottom_right_corner.png` — Bottom-right corner
9. `wall_09_T_junction.png` — T-junction (3-way)
10. `wall_10_cross_junction.png` — 4-way intersection
11. `wall_11_door_arch.png` — Doorway / arch opening
12. `wall_12_alcove_niche.png` — Decorative alcove/niche in wall

### DECAL OVERLAYS (12 variants, 64×64 or 96×96, transparent bg)
Klasör: `decals/`

Organic scatter elements, blend on top of floor:
1. `decal_01_moss_tuft_small.png` — Small moss tuft
2. `decal_02_moss_patch_large.png` — Large moss patch with creeping edges
3. `decal_03_dirt_stain.png` — Irregular dirt stain
4. `decal_04_crack_hairline.png` — Hairline crack overlay
5. `decal_05_crack_deep.png` — Deeper jagged crack
6. `decal_06_pebbles.png` — Small stone pebbles scatter
7. `decal_07_bone_fragment.png` — Single weathered bone shard
8. `decal_08_bone_pile.png` — Small bone pile cluster (like in concept_v1)
9. `decal_09_dust.png` — Subtle dust/debris patch
10. `decal_10_vegetation.png` — Small weeds + grass tuft
11. `decal_11_blood_drop.png` — Single small blood splatter
12. `decal_12_burn_mark.png` — Small scorch mark

### LARGE ACCENT OVERLAYS (6 variants, 256×256, transparent bg)
Klasör: `accents/`

Big atmospheric overlays — central feature pieces:
1. `accent_01_rift_scar.png` — Large dark crimson irregular multi-blob with radial cracks, cold blue rim glow (like concept_v1 center)
2. `accent_02_battle_splatter.png` — Blood splatter + dust cloud combo
3. `accent_03_scorch_burn.png` — Large burned area, charcoal black center fading to ember
4. `accent_04_ritual_circle.png` — Faded ritual circle with sigil markings
5. `accent_05_blood_pool.png` — Dried blood pool, dark crimson
6. `accent_06_ash_pile.png` — Pile of grey ash with embers

### PROP OBJECTS (12 variants, 64-128px native, transparent bg)
Klasör: `props/`

Free-standing furniture/clutter, place anywhere on floor:
1. `prop_01_wooden_crate.png` — Wooden crate (like concept_v1)
2. `prop_02_stone_urn_intact.png` — Stone urn intact
3. `prop_03_stone_urn_broken.png` — Broken stone urn (like concept_v1)
4. `prop_04_barrel.png` — Wooden barrel
5. `prop_05_candle_holder.png` — Iron candle holder with lit candle (like concept_v1)
6. `prop_06_burning_brazier.png` — Iron tripod brazier with fire (like concept_v1)
7. `prop_07_hanging_banner_torn.png` — Torn red banner hanging (like concept_v1)
8. `prop_08_stone_column.png` — Intact stone column
9. `prop_09_stone_pillar_broken.png` — Broken pillar fragment
10. `prop_10_chest_closed.png` — Closed treasure chest
11. `prop_11_chest_open.png` — Open chest revealing contents
12. `prop_12_statue_torso.png` — Stone statue torso (humanoid bust)

## TOTAL: 50 sprites

## STIL DIRECTIVES (her sprite için tut)

**Match concept_art_rima_sample_room.png:**
- Painterly brush strokes, hand-painted feel
- Multi-tone shading within pixel grid (NOT flat 2-tone, NOT hard-pixel-only)
- Atmospheric color depth (NOT flat saturated)
- Vivid Vulnerability palette: dark slate gray + deep brown + dusty blue + moss green + warm brown + faint dark red rift + cold blue rim
- Mood: ancient ritual temple, weathered, hollow watchful, post-battle aftermath
- Top-down 30-35° angle
- Visible perspective on 3D objects (props with depth, walls with top cap)

**Transparent backgrounds:**
- All decals, accents, props transparent BG
- Floor tiles can have opaque BG (tileable)
- Walls have opaque body, transparent top edge (for layering)

**Anti-patterns (AVOID):**
- NOT geometric Pillow shapes
- NOT cartoon flat colors
- NOT anime
- NOT 3D render
- NOT photorealistic
- NOT illustrator vector
- NOT isometric (diamond grid)
- NOT pure top-down 90°
- NOT character / mob / creature sprites (sadece environment + props)

## Pipeline & Compose

Codex imagegen skill ile **50 ayrı imagegen call**. Her sprite kendi PNG dosyası.

Compose step **DAHIL DEĞIL** — bu task sadece asset pack üretiyor. Compose'u sonra biz (Unity Brush V1 + procgen) yaparız.

**Ama final QC için:** Codex bir **demo composite** üretir, asset pack'in nasıl görüneceğini gösterir:

`STAGING/painterly_asset_pack_v1_demo_room.png` — 1024×768, kendi pack'inden tile + decal + prop + accent compose. RIMA character anchor (`ANCHORS/characters/01_warblade.png`) merkeze paste — Unity'de görsel uyum test.

## Done Report

`STAGING/codex_painterly_asset_pack_v1_DONE.md`

İçerik:
1. Asset count: 8 floor + 12 wall + 12 decal + 6 accent + 12 prop = 50 ✅
2. Style consistency assessment: tüm 50 sprite concept_art_v1 stil aile içinde mi?
3. Tool/backend: imagegen skill
4. Demo composite preview path
5. Issues encountered
6. Recommended next steps for Unity import

## Constraints

- **MUST use imagegen skill** — NO Pillow shapes generation
- **MUST follow concept_art_v1 painterly style**, NOT hard-pixel
- **Each sprite SEPARATE PNG file** in correct subfolder
- Transparent BG for decals/accents/props (where appropriate)
- Time budget: 45-90 minutes (50 imagegen calls)
- Output paths exact:
  ```
  Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/floor/
  Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/walls/
  Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/decals/
  Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/accents/
  Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/props/
  STAGING/painterly_asset_pack_v1_demo_room.png
  STAGING/codex_painterly_asset_pack_v1_DONE.md
  ```

## Stil Anchor (referans — Codex bu render'ı tekrar yorumlayacak)

`STAGING/concept_art_rima_sample_room.png` — bu render'daki HER ÖĞE individual sprite olacak. Render'ı stil-only reference olarak kullan, layout'u kopyalama.

Asset pack'in bütününün uyumu için **`STAGING/asset_pack_v1_painterly.png`** referansı da var — eski Codex pack visualization, 12 sprite gösteriyor, stil aile aynı kalmalı.
