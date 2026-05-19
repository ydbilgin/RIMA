ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethayday.md AS THE VERY LAST STEP.

# Codex Task — Unity Legacy Asset Cleanup Execute (Karar #150)

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`
NLM canonical artık güncel — Karar #150 LIVE doğru yanıtlanır.

---

## Görev

`STAGING/UNITY_LEGACY_CLEANUP_PLAN.md` plan dokümanını uygula. Sonnet analysis output, tüm GUID risk'leri flag'lendi. Codex pre-cleanup fixes (commit `d83d20d`) PASS — broken refs fixed, test paths migrated, Wang16 dead code removed. Cleanup execute artık unblocked.

Bu **file batch operation** — mv/rm/mkdir. Kod düzenleme YOK.

---

## Bağlam (önce oku)

1. **`STAGING/UNITY_LEGACY_CLEANUP_PLAN.md`** — full cleanup plan (KEEP/ARCHIVE/DELETE listeleri + risk flags)
2. **`STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md`** — Karar #150 stance (flat sprite YASAK, fake-iso depth required)
3. Pre-cleanup PASS evidence: commit `d83d20d` — `LargeDungeonMapPainterBase.cs` gate ref nullsafe + `BrushDataTests.cs` Act1 paths + Wang16 editor scripts deleted

---

## Execute steps

### STEP 1 — DELETE batch (outside Unity Assets, no GUID impact)

Tüm bu klasör/dosyaları **sil** (`rm -rf` veya `Remove-Item -Recurse -Force`):

```
STAGING/TILESET_OUTPUT/F1_FloorVariants_64batch/
STAGING/TILESET_OUTPUT/F1_FloorVariants_16batch_MCP_v2/
STAGING/TILESET_OUTPUT/F1_BaseClean_16_MCP_v3/
STAGING/TILESET_OUTPUT/F1_Organic_16_MCP_v4/
STAGING/TILESET_OUTPUT/F1_Base_Granite_PURE_16_v5/
STAGING/TILESET_OUTPUT/F1_Microtexture_16_MCP_v6/
STAGING/TILESET_OUTPUT/undercroft_connected/
Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/
```

**Riski sıfır:** STAGING/ Unity Assets dışında, hiçbir scene/SO referansı yok. RIMA_Painterly_Pack_v1/ sadece 5 boş .meta stub — sprite içermez.

**Tahmini delete count:** ~120 PNG (STAGING) + 5 .meta stub = ~125 files

### STEP 2 — ARCHIVE batch (Unity Assets, move with .meta preserved)

Hedef klasör: `Assets/Art/_archive_karar150/` (yoksa oluştur).

Aşağıdaki 118 file'ı `Assets/Art/_archive_karar150/` altına **move** et. **Her sprite + .meta birlikte taşınmalı** (GUID preservation kritik):

**ARCHIVE batch 1 — Flat walls (Karar #150 fake-iso ile uyumsuz):**
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painterly_wall_01.png` through `_12.png` (12 PNG + 12 .meta)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/wall_decoration_vines.png` + .meta

**ARCHIVE batch 2 — Stranded gates (Resources versions deleted):**
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/gate_arch.png` (no .meta, may need check)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/gate_spikes.png` + .meta

**ARCHIVE batch 3 — Concept references v1-v3 (v4 production):**
- `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` + .meta
- `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v2_35deg.png` (.meta check)
- `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v3_fakeiso.png` (.meta check)

**ARCHIVE batch 4 — StoneDungeon pre-painterly:**
- `Assets/Sprites/Environment/StoneDungeon/Tiles/stone_floor_pro_0.png` through `_15.png` (6 PNG + 6 .meta — count varies, glob actual)
- `Assets/Sprites/Environment/StoneDungeon/Walls/stone_wall_pro_0.png` through `_15.png` (16 PNG + 16 .meta)
- `Assets/Sprites/Environment/StoneDungeon/Rejected/` (all PNG + .meta ~11+11)

**ARCHIVE batch 5 — Wang16 v2 iteration (RIMA Wang16 closed):**
- `Assets/Sprites/Environment/StoneDungeon_v2/Walls/wang16_*.png` + .meta (17+17)
- `Assets/Sprites/Environment/StoneDungeon_v2/Tiles/` (7+7)
- `Assets/Sprites/Environment/StoneDungeon_v2/Decals_L4/` (1+1)
- `Assets/Sprites/Environment/StoneDungeon_v2/Detail_L5/` (1+1)
- `Assets/Sprites/Environment/StoneDungeon_v2/Accents_L6/` (1+1)

**ARCHIVE batch 6 — Keep legacy:**
- `Assets/Art/Tiles/Keep/Walls/wall_0.png` through `wall_3.png` + .asset tiles (4+4 PNG/.meta + 4+4 .asset/.meta)
- `Assets/Art/Tiles/Keep/_old_purple_Walls/` (full folder content)
- `Assets/Art/Tiles/Keep/_old_purple_Decals/` (full folder content)
- `Assets/Art/Tiles/Keep/Floor/_old_blue_tileset.png` + `.json` + .meta
- `Assets/Art/Tiles/Keep/Floor/floor_tileset.png` + `floor_tileset.json` + .meta
- `Assets/Art/Tiles/Keep/Floor/floor_rift_tile.asset` + .meta
- `Assets/Art/Tiles/Keep/Keep_Combat.asset.meta` (orphan .meta — verify .asset missing then delete)

**ARCHIVE batch 7 — F1 staging loose / Wang16 ColdWall:**
- `Assets/Art/Tiles/F1/FlatTileset_GraniteV2/` (folder .meta only — empty folder, just rm or archive)
- `Assets/Art/Tiles/F1/SeamlessV1/` (folder .meta only — empty folder)
- `Assets/Art/Tiles/F1/ColdWall/` (wang_cold_wall.png + metadata.json + .meta files)
- `Assets/Art/Tiles/F1/wang_rubble_path.png` + .meta (root-level loose)
- `Assets/Art/Tiles/F1/wang_floor_wall.png` + .meta (root-level loose)

### STEP 3 — Critical KEEP verification (DO NOT TOUCH)

Aşağıdakiler **KEEP** — taşımayın silmeyin:
- `Assets/Resources/Environment/StoneDungeon/Walls/RIMA_*.png` (6 wall PNG runtime ref'd)
- `Assets/Resources/Environment/StoneDungeon/Decor/RIMA_*.png` (4 decor PNG runtime ref'd)
- `Assets/Art/Tiles/Keep/Floor/tile_*.png` (24 PNG — BrushDataTests artık Act1 paths kullanıyor ama Keep tiles bağımsız korunsun)
- `Assets/Art/Tiles/F1/Tilesets/` (11 Wang tilesets active)
- `Assets/Art/Tiles/F1/Generated/` (.asset Wang tiles — Codex Wang16 dead code removed editor scripts ama .asset'ler kalır, biome preset reference ediyor olabilir)
- `Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset`
- `Assets/Art/Tiles/F1/Alabaster_Dawn_BiomePreset.asset`
- `Assets/Art/BrushAtlas/Pools/L3_Wang_ShatteredKeep.asset`
- Tüm `_Universal/` ve mevcut Act1 KEEP listesindeki (decals, accents, props, floor_tiles painterly_*)

### STEP 4 — Post-cleanup validation

1. **Compile check:** `dotnet build Assembly-CSharp.csproj` + `dotnet build RIMA.Tests.EditMode.csproj` → 0 error gerekli
2. **Test run:** `dotnet test RIMA.Tests.EditMode.csproj --filter Brush` → PASS gerekli (BrushDataTests Act1 path doğrula)
3. **Git status:** Tüm deleted/moved file'lar git'te tracked olarak görünmeli (D / R status)

### STEP 5 — Commit

Tek commit:

```
[S94 LATE] Karar #150 cleanup — flat walls + legacy assets archived, STAGING intermediates deleted

Archived 118 legacy files to Assets/Art/_archive_karar150/:
- 13 flat walls (painterly_wall_01-12 + wall_decoration_vines) — Karar #150 fake-iso uyumsuz
- 3 stranded gate sprites (Resources versions D in git)
- 3 concept references v1-v3 (v4 production reference kalır)
- 50+ StoneDungeon/StoneDungeon_v2 pre-painterly + Wang16 v2 sprites
- 16+ Keep legacy (purple walls/decals + old tilesets + Wang loose PNGs)

Deleted ~125 intermediate files:
- STAGING/TILESET_OUTPUT/ all gen batch subfolders (6+ batches)
- Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/ empty folder + .meta stubs

KEEP intact:
- All Resources/Environment/StoneDungeon (active runtime refs)
- All _Universal + Act1 painterly production assets
- F1/Tilesets + F1/Generated (active biome preset refs)
- All BrushAtlas pools + biome presets

Compile: 0 errors. EditMode tests pass.

Refs: STAGING/UNITY_LEGACY_CLEANUP_PLAN.md, Karar #150 STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md
Co-Authored-By: Codex
```

---

## Done report

`STAGING/CODEX_DONE_unity_legacy_cleanup_execute.md` yaz:

```
STEP 1 DELETE: <success_count>/<expected_count>
- Subfolders deleted: <list>
- Notes: <any blockers or partial>

STEP 2 ARCHIVE: <success_count>/<expected_count>
- Archive folder created: <path>
- File counts per batch:
  - Batch 1 flat walls: X
  - Batch 2 stranded gates: X
  - Batch 3 concept v1-v3: X
  - Batch 4 StoneDungeon pre-painterly: X
  - Batch 5 Wang16 v2: X
  - Batch 6 Keep legacy: X
  - Batch 7 F1 loose: X
- .meta preservation: <verified yes/no>

STEP 3 KEEP verification: <PASS/FAIL>
- All Resources files intact: <yes/no>
- All KEEP list files intact: <yes/no>

STEP 4 Validation:
- Compile: <PASS/FAIL>
- Tests: <PASS count>
- Git status sane: <yes/no>

STEP 5 Commit: <hash or PENDING>

OVERALL: <DONE / NEEDS_REWORK / BLOCKED>
```

Effort: high. Profile: laurethayday.


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethayday.md AS THE VERY LAST STEP.