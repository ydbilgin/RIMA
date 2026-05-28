# S99 Sonnet Cleanup Analiz Raporu

Tarih: 2026-05-22
Agent: rima-sonnet
Scope: Deep Unity + Painter + asset + memory + STAGING cleanup analysis (read-only)

## 1. Editor scripts — Dead/Orphan Candidates

### CONFIRMED DEAD (safe to delete)
- `Assets/Editor/DevTools/IsometricSortSetup.cs` — DEPRECATED 2026-05-21, wrapped `#if false`. MenuItem dead.
- `Assets/Editor/RepaintRoom.cs` — Loads `Assets/Art/Tiles/Act1/Act1_Floor.asset` + `Act1_Wall.asset`; klasör boş. Dead.
- `Assets/Editor/DevTools/F1TileSetup.cs` — Tarar `Assets/Art/Tiles/Act1/F1`; klasör yok.
- `Assets/Editor/DevTools/Act1TileImporter.cs` — `Assets/Art/Tiles/Act1` hedefler (boş/yok).

### ARCHIVE
- `Assets/Editor/WangVariantStubGenerator.cs` — Wang deprecated, CornerWangTileSetSO bağımlı.
- `Assets/Editor/CreateDemoScene.cs` — `_FazMVP_Demo.unity` üretir; eski pipeline.

### Likely orphan — risk: kontrol
- `Assets/Editor/RimaMapDesignerWindow.cs` — `RIMA_MapData` klasörü yok.
- `Assets/Editor/RoomVariationProcessor.cs` — Hangi sistem çağırıyor belirsiz.
- `Assets/Editor/GameFeelSetup.cs`, `SceneViewSetup.cs` — Use belirsiz.
- `Assets/Editor/CreateUIScenes.cs` — UI scene'ler zaten var.
- `Assets/Editor/SpriteHandAnnotatorWindow.cs` — Weapon decouple için olabilir.

### KEEP (confirmed)
- `RimaWorldPainterWindow.cs`, `MapDesigner/` klasörü, `TileImport/`, `Tools/SpritePivotBatchFix.cs`, `ApplySeloutMaterial.cs`, `StubSpriteGenerator.cs`, DevTools (GameViewSetup, ClearTilemaps, RemoveStrayRoomRoot, CreateDepthBandSOs), `RimaSortingLayerValidator.cs`, `BrushInputHandler.cs`, `TilemapMutator.cs`, `TilesetPaletteDrawer.cs`, `ObjectsPanelDrawer.cs`, `AutoBiomePresetBuilder.cs`, `BiomeQuickEditorWindow.cs`

### Already archived (do not touch)
- `Assets/Editor/_archive_S73/`, `Assets/Editor/_Archive_painter_alt/`

### LaurethTilesetTools
- Klasör YOK, hiç yaratılmamış. Phase D planned (defer).

## 2. World Painter Dead Sections

- **Line 521 `LoadPainterPrefs`:** Default `PaintMode.Isometric` → değiştir `PaintMode.TopDown`
- **Lines 34-35 `PaintMode/GridProjectionMode`:** `Isometric` enum dead code; serialize uyumluluğu için Codex'e flag
- **Lines 73-79 `wallScanFolders`:** `Assets/Prefabs/Props/ShatteredKeep_PixelLab` path mevcut mu? Kaldırılabilir
- **Lines 94-95 `wallRuleTileMode + wallRuleTile`:** Wang rule mode, dead UI section, gizle/kaldır (düşük öncelik)
- **`ModularFloorTileFolder`** çalışıyor ✓ KEEP

## 3. Asset Orphans

### PNG / SO
- `Assets/Art/Tiles/F1/Tilesets/` 11 spritesheet — Wang cloud download, deprecated RIMA → ARCHIVE
- `Assets/Art/Tiles/F1/Generated/` wang tile asset'ler → ARCHIVE
- `Assets/Art/Tiles/Keep/` Floor/Walls (tile_1..24 + wall_0..3 + Keep_Combat.asset) — Keep_Combat BiomePreset ref olabilir, kontrol gerek → koşullu ARCHIVE
- `Assets/Art/Tiles/Act1.meta` — Boş klasör meta → DELETE
- `Assets/Data/Tiles/Act1_ShatteredKeep/wang_rules/` BOŞ klasör → DELETE

### Prefab
- `Assets/Prefabs/Walls/pilot_a/` 7 prefab — Eski iso experiment. WallPrefabRegistry'de kayıtlı olmamalı, kontrol gerek → koşullu ARCHIVE

## 4. Scene Cleanup — 6 archive candidate

| Scene | Aksiyon |
|---|---|
| `_FazMVP_Demo.unity` | ARCHIVE |
| `IsoShowcaseRoom_S95.unity` | ARCHIVE |
| `ShaderBlend_Test.unity` | ARCHIVE |
| `PathC_BaseTest.unity` | ARCHIVE |
| `Phase2_WeaponAttach_Test.unity` | ARCHIVE |
| `PainterlyPack_Test_Room.unity` | ARCHIVE |

KEEP: `TopDownTest_Map1`, `Act1_ShatteredKeep`, `PlaceholderRoomTest`, `UI/MainMenu`, `UI/CharacterSelect`

## 5. Memory Drift

### MEMORY.md Index'te olmayan (orphan) — 25+ dosya
- ARCHIVE: discord_export_pipeline, anchor_selections_s43, class_identity_pivots_s43, comfyui_pipeline, compact_pass_s40, gemini_concept_pipeline, lint_findings, perspective_templates, style_bible_drift_s43, cleanup_s43_refactor, feel_toggles
- RE-INDEX: class_genders, ghost_attack_system, hud_design, idle_poses, item_matrix_decisions, lighting_wip, localization, rift_break, rift_crack_architecture, rima_backlog, shadow_standard, sim_philosophy, vfx_production, warblade_anim, hud_overlay_decision, rima.md
- KEEP: graphify

## 6. STAGING Archive Candidates — 15+ dosya

### ARCHIVE (completed)
- `codex_task_phase_E_unity_setup.md` through `codex_task_phase_K_vertical_slice_test.md` (8 dosya)
- `phase_E_verdict.md` through `phase_K_verdict.md` (8 dosya)
- `roomdesigner_codex_review.md`, `roomdesigner_F1_*.md` (5 dosya, S73 era)
- `unity_mcp_test_task.md`, `attacktoken_impl.md`
- `anim_prompts_review_*`, `anim_prompts_skill_review_*` (4 dosya)
- `warblade_codex_review_dispatch.md`, `idle_regen_batch_v1.md`

### KEEP (aktif)
- `wall_production_user_web_prompts_v2.md` (LIVE)
- `weapon_web_prompts_v1.md`, `asset_pack_v2_proposal.md`, `S98_autonomous_roadmap_expanded.md`
- `map_system_design_v1.md`, `map_schema_v1.json`, `act1_shattered_keep_layout_v1.json`
- `phase_K_verdict.md` (REWORK ref), `mapdesigner_audit_report.md`
- `concepts/`, `graphify_corpus/`
- `codex_task_s99_*.md`, `s99_*_verdict.md` (current session)

## 7. Console
- Önceki commit 0 error doğrulanmış
- `IsometricSortSetup` #if false wraplı, warning üretmez
- Fix: `LoadPainterPrefs` default Iso → TopDown

## Summary

| Kategori | Delete | Archive | Re-index | KEEP |
|---|---:|---:|---:|---:|
| Editor scripts | 4 | 3+ | — | 30+ |
| Painter sections | — | — | — | 2 flag |
| Asset PNG/SO | 2 empty folders | 3 grup | — | modular_v1, Batch1 |
| Prefabs | — | pilot_a (kontrol) | — | Env/Walls/Act1 |
| Scenes | — | 6 | — | 5 |
| Memory | — | 8-10 | 15+ | Active section |
| STAGING | — | 15+ | — | 10+ active |

- **Toplam delete:** ~6 dosya/klasör
- **Toplam archive:** ~45-55 dosya/klasör
- **Risk:** MEDIUM (pilot_a registry kontrol, F1 wang asset cross-ref, memory orphan kontrol)

## Önerilen Codex dispatch — 3 batch

| Batch | Scope | Risk |
|---|---|---|
| **A** | Unity asset cleanup: 4 dead script delete, F1/Tilesets+Generated archive, Keep/ kontrol, 6 scene archive, painter default mode fix, 2 empty folder delete, pilot_a registry check | MEDIUM (compile risk) |
| **B** | STAGING archive (15+ file) + Memory re-index (15 file) + archive (8-10 file) | LOW (yalnızca .md taşıma) |
| **C** | Painter deep refactor: Isometric enum cleanup, wallRuleTileMode UI gizle | HIGH (serialize compat) |

Önerilen sıra: A → B → C ayrı dispatch'lerde.
