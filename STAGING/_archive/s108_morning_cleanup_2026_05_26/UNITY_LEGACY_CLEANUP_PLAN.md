# Unity Legacy Asset Cleanup Plan — Karar #150 LIVE (S94 LATE NIGHT 2026-05-19)

**Source:** rima-sonnet sub-agent analysis (a20c607a40c87d475)
**Scope:** 10 directories scanned, ~420 unique asset files analyzed

## Summary

- **Total scanned:** ~420 files (PNG + .asset + .meta across 10 scope directories)
- **KEEP (Karar #150 production-ready):** 62 files
- **ARCHIVE (ref value, remove from production):** 118 files
- **DELETE (intermediate output, zero value):** ~120+ STAGING PNG files + orphan stubs
- **Concept reference v1-v3:** 3 files — archive option (history), v4 production

---

## KEEP — Karar #150 production-ready

| Path | Why |
|---|---|
| `Assets/Art/AssetPacks/_Universal/small_stones/stone_a_rounded.png` through `stone_e_mossy.png` (5 PNG + 5 .meta) | Neutral, cross-Act tintable — explicit KEEP in Karar #150 criteria |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/decals/painterly_decal_01` through `_12` (12 PNG + 12 .meta) | Organic decals compatible with Hades-style overlay pipeline |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/accents/painterly_accent_01` through `_06` (6 PNG + 6 .meta) | Rift + battle accents — cyan rift accent `_01_rift_scar` directly Karar #150 compatible |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/props/painterly_prop_01` through `_12` (12 PNG + 11 .meta — prop_06 missing .meta noted) | Props are view-agnostic (crate/urn/barrel/statue etc.), no perspective issue |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painterly_floor_01_clean.png` through `_08_blood_old.png` (8 PNG + 8 .meta) | PURE top-down floor tiles — Karar #143/150 Alabaster Dawn compliant |
| `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png` | Art bible reference LIVE |
| `Assets/Art/Rooms/Backgrounds/Spawn_01/layer_00_floor_painted_granite.png` | Active — referenced by `RoomTemplateSOInspector.cs` via `layer_00_floor_*` glob pattern |
| `Assets/Art/Rooms/Backgrounds/Spawn_01/layer_01_decal_rift_crack.png` through `layer_04_accent_glow_mote.png` | Active room composition layers — referenced by inspector pattern |
| `Assets/Art/Rooms/Backgrounds/Spawn_01/layer_10_floor_variation_moss.png` | Active — referenced by `layer_10_floor_variation_*` glob pattern |
| `Assets/Resources/Environment/StoneDungeon/Walls/RIMA_wall_base.png` through `RIMA_wall_banner.png` (6 PNG + 6 .meta) | **CRITICAL ACTIVE REFERENCE** — `LargeDungeonMapPainterBase.cs` loads these via `Resources.Load` at runtime |
| `Assets/Resources/Environment/StoneDungeon/Decor/RIMA_pillar_base.png`, `RIMA_debris.png`, `RIMA_rift_crystal.png`, `RIMA_shrine.png` (4 PNG + 4 .meta) | **CRITICAL ACTIVE REFERENCE** — same script, runtime Resources.Load |
| `Assets/Resources/Environment/Markers/RIMA_PlayerStartMarker.png` | Runtime marker, unrelated to visual style |
| `Assets/Art/Tiles/Keep/Floor/tile_1.png` through `tile_24.png` (24 PNG + 24 .meta) | **TEST REFERENCE** — `BrushDataTests.cs` references tile_1/2/3 by path; full set needed for test stability |
| `Assets/Art/Tiles/F1/Tilesets/` (11 tilesets, spritesheet + json per tileset) | Wang tilesets from `project_f1_canonical_tilesets_discovery` LOCK — `RebuildAllWangTilesets.cs` + `AutoBiomePresetBuilder.cs` actively reference root |
| `Assets/Art/Tiles/F1/Generated/` (all wang tile .asset files) | Referenced by `WangImporterTests.cs` and `CreateCornerWangTileSetAsset.cs` — active test + editor code |
| `Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset` | Active biome preset used by map system |
| `Assets/Art/Tiles/F1/Alabaster_Dawn_BiomePreset.asset` | Alabaster Dawn pipeline LOCK asset |
| `Assets/Art/BrushAtlas/Pools/L3_Wang_ShatteredKeep.asset` | Active pool asset referenced by brush system |

---

## ARCHIVE — move to `Assets/Art/_archive_karar150/`

| Path | Why archived |
|---|---|
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painterly_wall_01` through `_12` (12 PNG + 12 .meta) | **FLAT perspective walls** — 2D orthographic layout tiles, NOT fake-iso depth walls with top cap + front face. Karar #150 requires fake-iso depth |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/wall_decoration_vines.png` + .meta | Flat overlay, no iso depth context |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/gate_arch.png` (no .meta — orphan PNG) | RIMA_gate_arch deleted from Resources per git status D, but AssetPacks copy stranded |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/gate_spikes.png` + .meta | Same — Resources versions deleted, AssetPacks copy is stranded |
| `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` + .meta | v1 50-60° REJECTED — archive as history |
| `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png` | v2 35° flat — superseded by v4 fake-iso |
| `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v3_fakeiso.png` | v3 prototype, v4 is canonical |
| `Assets/Sprites/Environment/StoneDungeon/Tiles/stone_floor_pro_0.png` through `_15` (6 active PNG + 6 .meta) | Early-gen floor sprites, no active code reference found |
| `Assets/Sprites/Environment/StoneDungeon/Walls/stone_wall_pro_0.png` through `_15` (16 PNG + 16 .meta) | No code references — pre-painterly-pack iteration |
| `Assets/Sprites/Environment/StoneDungeon/Rejected/stone_floor_A.png` through `stone_floor_pro_13.png` (11 PNG + 11 .meta) | Already "Rejected" folder |
| `Assets/Sprites/Environment/StoneDungeon_v2/Walls/wang16_00.png` through `wang16_15.png` + `wang16_set.png` (17 PNG + 17 .meta) | Wang16 wall sprites — RIMA Wang16 pipeline KAPATILDI per [[project-rima-hades-style-cb-wang-split-lock]]. CB pivot |
| `Assets/Sprites/Environment/StoneDungeon_v2/Tiles/floor_set_a.png`, `floor_variant_1-6.png` (7 PNG + 7 .meta) | Wang16 v2 iteration, no reference, CB pivot |
| `Assets/Sprites/Environment/StoneDungeon_v2/Decals_L4/decals_organic.png`, `Detail_L5/detail_scatter.png`, `Accents_L6/accents_overlay.png` (3 PNG + 3 .meta) | Wang16 companion layers, no reference |
| `Assets/Art/Tiles/Keep/Walls/wall_0.png` through `wall_3.png` + .asset tiles (4 PNG + 4 .meta + 4 .asset + 4 .asset.meta) | Legacy Keep walls — no active scene/SO reference |
| `Assets/Art/Tiles/Keep/_old_purple_Walls/` (4 PNG + 4 .meta + 4 .asset + 4 .asset.meta) | Old purple palette walls — legacy color scheme |
| `Assets/Art/Tiles/Keep/_old_purple_Decals/` (4 PNG + 4 .meta + 4 .asset + 4 .asset.meta) | Old purple palette decals |
| `Assets/Art/Tiles/Keep/Floor/_old_blue_tileset.png` + `.json` (+ .meta x2) | Legacy blue tileset, superseded |
| `Assets/Art/Tiles/Keep/Floor/floor_tileset.png` + `floor_tileset.json` (+ .meta x2) | Legacy tileset composite |
| `Assets/Art/Tiles/Keep/Floor/floor_rift_tile.asset` + .meta | Wang-era rift tile asset |
| `Assets/Art/Tiles/Keep/Keep_Combat.asset.meta` (orphan .meta) | Orphan .meta — .asset not present |
| `Assets/Art/Tiles/F1/FlatTileset_GraniteV2/` (folder .meta only — folder empty) | Empty folder stub |
| `Assets/Art/Tiles/F1/SeamlessV1/` (folder .meta only — folder empty) | Empty folder stub |
| `Assets/Art/Tiles/F1/ColdWall/` (wang_cold_wall.png + metadata.json + .meta files) | Wang16 ColdWall variant — CB pivot, no RIMA reference |
| `Assets/Art/Tiles/F1/wang_rubble_path.png` + `wang_floor_wall.png` (root-level loose PNGs) | Staging loose source PNGs for Wang16 — superseded by Tilesets/ |

---

## DELETE — kalıntı

| Path | Why delete |
|---|---|
| `STAGING/TILESET_OUTPUT/F1_FloorVariants_64batch/tile_0-15.png` + `PREVIEW_4x4_grid_8x.png` (17 files) | Intermediate gen batch v1 — superseded |
| `STAGING/TILESET_OUTPUT/F1_FloorVariants_16batch_MCP_v2/tile_0-15.png` + PREVIEW (17 files) | Intermediate gen batch v2 — superseded |
| `STAGING/TILESET_OUTPUT/F1_BaseClean_16_MCP_v3/tile_0-15.png` + PREVIEW (17 files) | Intermediate gen batch v3 |
| `STAGING/TILESET_OUTPUT/F1_Organic_16_MCP_v4/tile_0-15.png` + PREVIEW (17 files) | Intermediate gen batch v4 |
| `STAGING/TILESET_OUTPUT/F1_Base_Granite_PURE_16_v5/tile_0-15.png` + PREVIEW (17 files) | Intermediate gen batch v5 — PURE top-down granite (confirm import lineage before delete) |
| `STAGING/TILESET_OUTPUT/F1_Microtexture_16_MCP_v6/tile_0-15.png` + PREVIEW (~17 files) | Intermediate gen batch v6 |
| `STAGING/TILESET_OUTPUT/undercroft_connected/B1-B8, C1-C4b` (13 files) | "undercroft" theme gen — never imported |
| `Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/` (5 .meta stubs only) | Empty folder shell — only .meta files, no PNG |

---

## Risk Flags

### CRITICAL — Do NOT archive/delete without migration

| Asset | Risk | Action required |
|---|---|---|
| `Assets/Resources/Environment/StoneDungeon/Walls/RIMA_wall_*.png` (6) + `Decor/RIMA_*.png` (4) | `LargeDungeonMapPainterBase.cs` loads ALL via `Resources.Load` at runtime (lines 1041-1132) | Must NOT move. Karar #150 migration means replacing with fake-iso sprites in SAME Resources path OR updating LargeDungeonMapPainterBase.cs (Codex pre-cleanup task) |
| `RIMA_gate_arch` + `RIMA_gate_spikes` (Resources) | `LargeDungeonMapPainterBase.cs` line 1044/1045 references these. Git status D (deleted) — runtime NullRef LIVE | **Already-broken reference.** Codex pre-cleanup fix Task #9 covers this |
| `Assets/Art/Tiles/Keep/Floor/tile_1-3.png` | `BrushDataTests.cs` lines 175-185 hardcode these paths | Keep set OR Codex pre-cleanup migrate test paths (Task #9 covers) |
| `Assets/Art/Tiles/F1/Generated/` Wang .asset | `WangImporterTests.cs` + `CreateCornerWangTileSetAsset.cs` + `RebuildAllWangTilesets.cs` active code refs | Archive only after Wang16 code removed (Codex pre-cleanup Task #9 Wang16 dead code removal handles) |

### MEDIUM RISK — Verify before archive

| Asset | Risk |
|---|---|
| `Keep_Combat.asset.meta` (orphan .meta, no .asset) | Verify .asset was deleted — .meta safe to delete |
| `STAGING/TILESET_OUTPUT/F1_Base_Granite_PURE_16_v5/` | May be canonical imported source for `F1/Tilesets/floor_wall/spritesheet.png` — confirm import lineage |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/gate_arch.png` | No .meta — may never imported. Safe delete if confirmed |

---

## Execution Sequence (orchestrator executes after Codex Task #9 PASS + user approval)

1. **GUID reference verification** — final check before any move
2. **Codex Task #9 PASS gate** — broken gate ref fixed, test paths migrated, Wang16 dead code removed
3. **DELETE batch** — `STAGING/TILESET_OUTPUT/` all subfolders (outside Unity Assets, no GUID impact)
4. **DELETE batch** — `Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/` empty folder + 5 .meta stubs
5. **ARCHIVE batch** — Move 118 files to `Assets/Art/_archive_karar150/` with .meta preserved
6. **Asset Pack Browser cache invalidate** — `AssetPackBrowserWindow.cs` refresh trigger
7. **Unity reimport** — Force reimport on `Assets/Art/` after moves

---

## Estimated Cleanup Impact

- **STAGING/TILESET_OUTPUT deleted:** ~120 PNG files (6+ batches × 17 files each)
- **Assets archive moved:** ~118 files (PNG + .meta + .asset)
- **Disk space freed:** ~15-25 MB estimated
- **Production-ready assets remaining:** ~62 unique files + all Resources/StoneDungeon (active runtime)
- **Test impact:** 0 if Codex pre-cleanup Task #9 migrates BrushDataTests paths

---

## Cross-references

- Codex pre-cleanup task: `STAGING/CODEX_TASK_pre_cleanup_fixes.md` (Task #9 dispatch)
- Karar #150 spec: `STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md`
- Asset pack hierarchy: memory `[[project-asset-pack-organization-lock]]`
- Wang16 closure: memory `[[project-rima-hades-style-cb-wang-split-lock]]`
