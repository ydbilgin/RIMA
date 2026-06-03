# MODULAR PIXELLAB PRODUCTION — MASTER PLAN (Opus synthesis)
**Synthesizes:** MODULAR_PIPELINE_AGY.md (visual) + MODULAR_PIPELINE_CX.md (engineering) + Opus + PixelLab community tool-clarification + user "daha büyük" steer.
**Date:** 2026-06-01 | **Direction:** ISO floating-island, slate/iron granite + sparring cyan #00FFCC, painterly Hades/CoM look (LOCKED by concept01-10).

---

## 0. VISION — Style-Propagation Tileset FACTORY (not one room kit)

The goal is bigger than a single Shattered Keep kit. It is a **factory**:
1. **Phase A** — author ONE definitive master kit in the locked look (the "golden master").
2. **Phase B** — *propagate* that style cheaply to N themed sibling tilesets/biomes (Keep -> Crypt -> Library -> Forge...) using PixelLab's style-reference/edit tools, so the whole game shares one art language at low per-theme cost.

Like the PixelLab community example: a "wooden mansion" tileset -> a "wood theatre" tileset in the SAME style. Same geometry/tileability, swapped material/theme.

---

## 1. TOOL DECISION TABLE (community-clarified — resolves the (pro)/(new)/pixflux naming confusion)

| PixelLab tool | What it is actually for | Our use |
|---|---|---|
| **create_map (pixflux)** | whole map SCENE from scratch (older model) | ❌ bakes a fused scene = same non-modular problem as the Imagen concepts. Concept/backdrop only, never playable tiles |
| **create_tiles (pro)** | standalone tiles in SHAPES (isometric / hex / oct) | ✅ **PRIMARY floor + iso geometry** (this made floor451). Wang16 NOT guaranteed -> use RandomTile variants |
| **create_topdown_tileset** | real Wang16 **TOP-DOWN** terrain (4x4/16-tile + metadata.json) | ❌ TOP-DOWN projection — using it for iso re-creates the flat-floor bug. Skip (only if a literal top-down screen ever needed) |
| **create_1_direction_object** | batch objects (size<=42 -> 64 cand/call; <=85 -> 16) | ✅ props, rubble, cliff edges/corners, wall segments, cyan decals — the batch workhorse |
| **create_8_direction_object** | single object, 8 angles | ✅ props needing rotation (rare; most iso props are 1-dir) |
| **edit_image (pro)** | new tiles from an EXISTING tileset (full set in hand) | ✅ **Phase B propagation engine** — themed variants |
| **create_from_style_pro / create_image_from_style_reference (pro)** | style-matched asset SET from refs | ✅ Phase B: spin a themed kit in the master style; also cyan-decal sets |
| **inpaint_v3 / extend_map / edit_image_pro** | seam fix / extend / add missing pieces | ✅ seam repair + gap fill in generated sets |
| **create_image_pro** | one high-quality hero sprite (max 512² / 688×384) | ✅ single focal props (portal frame, boss rune core) when batch quality insufficient |
| **create_character** | 8-dir actors | ✅ hero/boss (unchanged; characters = PixelLab only) |

**RESOLVED CONFLICT (floor):** agy=create_tiles_pro(iso) vs cx=create_topdown_tileset(Wang16). -> **create_tiles_pro (iso) wins.** Wang16 tool is top-down; the iso island floor is uniform granite and its border is the *cliff-edge layer*, not floor terrain-transitions. No Wang needed on the floor.

---

## 2. LAYER DECOMPOSITION (merged agy+cx — 7 Unity layers, cyan never baked into stone)

1. **Floor** — IsoGrid/Ground Tilemap, clean granite (create_tiles_pro iso). NO baked cyan.
2. **Cliff-edge + island underside** — separate Cliff Tilemap / y-sorted prefabs: thick granite rim, broken side face, hanging rock volume into the void. Walkability from floor mask; cliff is visual + drop-read.
3. **Walls / ruins** — Wall Tilemap + y-sorted wall prefabs: short back walls, broken blocks, gate arch, chain anchors, boss-arena rims. Colliders where needed.
4. **Props** — y-sorted prefabs under Props root: chest, portal foot, shrine, rubble, chain, pillar, rune slab, floating shard. Pulled via AssetPackSO / RuntimeAssetRegistry.
5. **Cyan VFX / decal** — Decals Tilemap + Lighting root + particles: cracks, runes, shock-ring, portal swirl, boss seal. Transparent sprites + additive material + Light2D pool. **This is where ALL cyan lives.**
6. **Character / boss** — runtime entity prefabs (PixelLab). Kit only provides arena decal/light anchors.
7. **Void / parallax** — background root: purple void, distant stars/shards, map-node link lines. Never mixed into room tilemaps.

**Cyan budget rule:** ~5-8% of frame. Base stone stays charcoal/blue-grey. Cyan only in (a) thin crack/rune decal sprites, (b) Light2D alpha/intensity, (c) particle bursts, (d) focal-prop emissive (portal/boss). Floor RuleTile = clean granite.

---

## 3. PHASE A — MASTER KIT (Act1 Shattered Keep, the style anchor)

| Asset | Tool | Style ref + init-strength | Variants | ~Cost |
|---|---|---|---|---|
| Floor granite (iso) | create_tiles_pro | floor451 + crop of concept01 floor; **str 250-350** | 12-16 | 20-25 |
| Cliff edges/corners + underside | create_1_direction_object (64-85px) | concept01/03 crop; **str 350-500** | straight×4, in-corner×4, out-corner×4, underside chunks | 20-40 |
| Broken walls / ruins | create_1_direction_object | concept01 crop; **str 350-500** | half-wall, corner, fallen block, pillar stump, gate arch | 20-40 |
| Common props / rubble | create_1_direction_object (<=42px) | concept crops; **str 300-450** | 32-64 | 20-40 |
| Cyan decals (cracks/runes) | create_1_direction_object / create_from_style_pro | concept03/07 crop; **str 300-450** | 8-12 | 20-40 |
| Reward focal (portal+chest) | create_1_direction_object (+create_image_pro if needed) | concept05 crop; **str 450-600** | 1-2 each | 20-40 (+20) |

**Prompt grammar (iso-corrected):** start from RIMA Style Lock BUT swap the old top-down clause:
`... matte hand-pixeled clusters, hard pixel edges, no anti-aliasing, charcoal slate (#2C2A2A-#4E5260), cold blue-grey shadows, ISOMETRIC PROJECTION (2:1), transparent background`
Negatives: `no text, no labels, no numbers, no watermark, no UI, no frame, no blurry edges, no anti-aliasing, no smooth vector gradients`.
**Cyan stripped from all base-stone prompts** (cyan only in the decal pack).

**Style-ref strategy:** never feed a whole concept painting (it tries to rebuild the room). Crop a 256² sub-region (just the floor patch / just the chest) as `style_images`. Strength per KB scale: 0-300 color-only (safest for tileables), 300-400 rough shape (cliff/wall), 400-600 near-ref (identity props).

**Phase A cost envelope:** ~80-160 gen (tightly scoped MVP) / ~120-240 gen (full first pass with separate cyan+reward calls).

---

## 4. PHASE B — STYLE PROPAGATION (the "daha büyük" scale engine)

Once the master kit is QC-locked, for each new theme/biome do NOT regenerate from scratch:
1. Take the master tiles/props as `style_images` into **edit_image (pro)** / **create_from_style_pro**.
2. Prompt the THEME swap only (e.g. "mossy crypt stone", "arcane marble library", "blackened forge iron"), geometry/tileability/cyan-layer unchanged.
3. inpaint_v3 / extend_map to fix seams + fill gaps.
4. Import into a SIBLING folder (Act2_X/, Act3_Y/) reusing the same SO/atlas/registry structure.

**Per-theme cost ~20-40 gen** vs 100+ from scratch. This is how 1 master kit -> the whole game's biome set in one consistent language.

---

## 5. UNITY ASSEMBLY (reuse existing tooling; minimal new tooling)

**Existing importers (reuse, do NOT rewrite):**
- `PixelLabPngSheetImporter.cs` — 8×8 256px sheet -> 64 sprites + RandomTile/RuleTile (iso floor variant import).
- `PixelLabWangImporter.cs` — only if a Wang set is ever used (4×4 32px + metadata.json).
- `PatchAtlasSpriteAtlasBuilder.cs` — decal/patch SpriteAtlas from PatchAtlasSO.
- `RuntimeAssetRegistryBaker.cs` — bakes sprite/tile/prefab + AssetPackSO.Entry (category/registryTag/layer) into RuntimeAssetRegistry.

**Canonical folder + SO structure (from cx):**
```
Assets/Art/AssetPacks/Act1_ShatteredKeep/
  ShatteredKeep_Base/  Tiles/Floor/  Tiles/Cliff_Edge/  Sprites/{Walls,Decals_Cyan,Props_Common}/  Prefabs/{Walls,Cliff,Decals_Lights}/  AssetPacks/AP_..._Base.asset  Atlases/PA_..._Decals.asset
  Archetypes/{Combat,Boss,Reward,Hub}/  (each: Sprites/ Prefabs/ AssetPacks/AP_..._<arch>.asset)
```
- Base AssetPackSO (packId=act1_shattered_keep_base): floor/cliff/prop/decal/light tagged entries.
- One AssetPackSO per archetype (Boss=rune ring+chains+rim; Reward=portal+chest+shard; Hub=shrine+pedestal+lights).
- PatchAtlasSO: Decals_Cyan (Accent, low density, high center-reduction), Rubble (DetailScatter, edge-biased), BossRunes (Accent, rotation-restricted).
- Procedural builder: ALWAYS base kit + merge archetype pack by RoomType.

**Minimal NEW tooling (list only, build later):**
1. PixelLab kit MANIFEST standard (per batch: source refs, tool, prompt, strength, cost, selected candidate id, Unity target layer) — reproducibility + Phase B re-runs.
2. Wang/sheet importer output-folder PRESET pointing at the canonical Act folder.
3. Cliff/edge kit ASSEMBLER (selected sprites -> prefab/TileBase + AssetPackSO entries).
4. Cyan decal+Light2D PAIRING preset (decal sprite + light intensity/radius/sorting in one prefab).
5. RuntimeAssetRegistryBaker scan-root update to include the new Act folder.

**Closes the earlier floor-fix loop:** the runtime floor TILE source (LargeDungeonMapPainter floorTiles pool / DepthBandTileSet_F1-3.asset — STEP 2 follow-up) gets repointed to this kit's floor451/iso RuleTiles. RoomLoader now applies the iso Grid recipe (done); this kit fills the tile content.

---

## 6. PRODUCTION ORDER + QUALITY GATES

**Order:** (1) Floor iso -> (2) Cliff/edge -> (3) Walls -> (4) Props/rubble -> (5) Cyan decals -> (6) Reward focal -> (7) Boss add-on (after MVP passes).
**Cost:** MVP ~80-160 gen; practical first pass ~120-240; boss +40-100. (KB: ~1208 gen left this tier.)
**Quality gates:** import floor -> verify IsoGrid (0.96,0.585,1) no squash -> assemble cliff around floor mask, readable floating-island silhouette -> cyan decal/light pass capped 5-8% -> bake registry (tags floor/cliff/prop/portal/light/decal) -> RoomLoader pulls base+1 archetype in a test room.

---

## 7. OPEN DECISIONS (before spending gen budget)
1. **De-risk first:** run a tiny STYLE-TEST (1 floor batch + 1 cliff batch + 1 cyan decal batch ≈ 40-80 gen) to validate the concept->PixelLab style bridge BEFORE committing the full MVP kit. Strongly recommended.
2. Confirm Phase-B factory is the intended "bigger" (vs more runtime procedural combinatorics, or more hand-authored rooms).
3. Floor tool locked = create_tiles_pro (iso). Override only if Wang auto-tiling is deemed essential (would force top-down).
