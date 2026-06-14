# Codex Task — RIMA Cleanup Execution (TIER A + B)

ACTIVE RULES: (1) think before moving (2) min destructive — move not delete (3) surgical — only files in this list (4) BLOCKED if any file already moved/missing.

NLM ACCESS: If needed:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / STAGING

# AMAÇ
User RIMA proje klasöründe "o kadar fazla eski dosya var ki her şey karışıyor" dedi. rima-sonnet analiz raporu hazır (`STAGING/CLEANUP_ANALYSIS_2026_05_25.md`). Sen Codex olarak TIER A (4 single-line edit) + TIER B (~44 file move) işle. TIER C (5 user-input gerektiren) ASLA dokunma. DELETE YOK — sadece MOVE.

# TIER A — Single-line edits (4 fix)

## A1. Mark diamond_iso_tilemap_lock as REVOKED
File: `MEMORY/project_diamond_iso_tilemap_lock_2026_05_24.md`
Action: Prepend at line 1 (before any existing content):
```
> ⚠️ REVOKED 2026-05-24 evening (Opus debugging verdict). Iso math + HTD 3/4 sprite = "dominoes leaning" visual. See [[project-high-top-down-3-4-lock-2026-05-24]] for live direction.
>
```

## A2. Add high_top_down_3_4 to MEMORY/INDEX.md
File: `MEMORY/INDEX.md`
Find appropriate "Active" section (camera/projection grouping). Add line:
```
- project_high_top_down_3_4_lock_2026_05_24.md — LIVE 2026-05-24 camera/projection lock (HIGH TOP-DOWN 3/4, Karar #114 reaffirm)
```

## A3. Fix pixellab_master_pipeline path in INDEX.md
File: `MEMORY/INDEX.md`
Search for `pixellab_master_pipeline.md` entry. Update path to `_archive/pixellab_master_pipeline.md` (file actually lives there).

## A4. Warning header on wrong-PPU brief
File: `STAGING/s106_overnight/UNITY_ISO_TILEMAP_BRIEF.md`
Prepend at line 1:
```
> ⚠️ HISTORICAL WARNING — This doc recommends PPU=32, which was the BUG that caused Stream O overlap. The correct rule is PPU=64 (matching cellSize.x=1). Verified at Stream O.1. Reference: MEMORY/feedback_pixellab_iso_tile_ppu_cellsize_rule.md
>
```

# TIER B — Bulk file moves (NO DELETE)

## B1. Move 12 root CODEX_DONE files → STAGING/_codex_done/
Source dir: `<project root>/`
Target dir: `STAGING/_codex_done/` (create if needed)
Files (12):
- CODEX_DONE_history_backup.md
- CODEX_DONE_sanity.md
- CODEX_DONE_bg_field_review.md
- CODEX_DONE_multilayer_painter_review.md
- CODEX_DONE_pixellab_v2_docs_fetch.md
- CODEX_DONE_multilayer_phase1_impl_review.md
- CODEX_DONE_multilayer_size_strategy.md
- CODEX_DONE_spawn01_layers_generation.md
- CODEX_DONE_hades_preset_button.md
- CODEX_DONE_brush_zone_layer_binding.md
- CODEX_DONE_spawn01_layers_1_to_4.md
- CODEX_DONE_ydbilgin.md

KEEP at root: `CODEX_DONE.md` (main shared file), `CODEX_DONE_laurethayday.md`, `CODEX_DONE_laurethgame.md`, `AGY_DONE.md`, `AGY_DONE_*.md` (active dispatch logs)

## B2. Archive 24 completed s106 stream task files → s106_overnight/_archive/
Source dir: `STAGING/s106_overnight/`
Target dir: `STAGING/s106_overnight/_archive/` (create if needed)
Files (24):
- STREAM_B_ASSET_TAXONOMY_TASK.md
- STREAM_B_FOLLOWUP_CONVERSION_TASK.md
- STREAM_C_P0_SAFETY_TASK.md
- STREAM_D_PAINTER_UX_TASK.md
- STREAM_E_TEST_ROOMS_TASK.md
- STREAM_F_VISUAL_DEPTH_TASK.md
- STREAM_F_VISION_REVIEW_TASK.md
- STREAM_G_COMBAT_PROOF_TASK.md
- STREAM_G_RESEARCH_WHY_FAIL_TASK.md
- STREAM_J_FULL_ISO_PIVOT_TASK.md
- STREAM_M_PLAYABLE_ARENA_TASK.md
- STREAM_N_ARENA_POLISH_TASK.md
- STREAM_O_ISO_TILE_INTEGRATION_TASK.md
- STREAM_O1_PPU_FIX_TASK.md
- STREAM_REVIEW_BATCH_TASK.md
- IDEATION_TASK.md
- RESEARCH_TASK.md
- OBJECT_INVENTORY_RESEARCH_TASK.md
- PIXELLAB_OBJECT_STRATEGY_TASK.md
- TILE_ANGLE_EVAL_TASK.md
- KRAKEN_TWITTER_RESEARCH_TASK.md
- PARAM_ADVICE_ISO_FLOOR_TASK.md
- BONUS_1_WALLPIECEDATA_SPRITE_TASK.md
- PIXELLAB_STRATEGY_COUNTERCHECK_SONNET.md

KEEP active:
- STREAM_P_MINIMAL_TILE_PAINTER_TASK.md
- MASTER_CONTEXT.md, MASTER_PLAN.md, SESSION_LOG.md
- PIXELLAB_STRATEGY_RECONCILED.md
- WALLLESS_DESIGN_ANTIGRAVITY.md, WALLLESS_DESIGN_CODEX.md
- WALLLESS_DOOR_BG_ANTIGRAVITY.md (just dispatched)
- WALLLESS_IMAGEGEN_MOCKUPS_CODEX.md (just dispatched)
- TILE_CATEGORIZATION.md (today)
- walless_mockup_v{1-4}.png + walless_mockup_REPORT.md (today)
- ideation/*, stream_e_rooms/*, stream_c_validation/*, stream_b_assets/*, stream_g_metrics.txt (output folders)

## B3. Archive 13 root STAGING pre-S99 files → STAGING/_archive_pre_s99/
Source: `STAGING/`
Target: `STAGING/_archive_pre_s99/` (create)
Files:
- warblade_pilot_prompt_2026-05-12.md
- warblade_south_prompt_v2.md
- warblade_codex_review_dispatch.md
- pilot_warblade_prompt_2026-05-12.md
- roomdesigner_F1_skeleton.md
- roomdesigner_F1_palette.md
- roomdesigner_F1_brush.md
- roomdesigner_F1_2_visual_fix.md
- idle_regen_batch_v1.md
- anim_prompts_review_task.md
- anim_prompts_review_result.md
- attacktoken_impl.md
- unity_mcp_test_task.md
- ELEMENTALIST_FIRE_FROST_LIGHTNING_BUILD_MATRIX_2026-05-04.md
- pixellab_videos_synthesis_2026-05-13.md

## B4. Archive STAGING/pilot_a_candidates/ → STAGING/_archive_pre_s99/pilot_a_candidates/
Move entire folder.

# RULES
- ❌ NO DELETE — pure MOVE only
- ❌ DO NOT touch TIER C items (5 user-input pending):
  - Assets/Sprites/AssetPackV3/floor/ folder
  - MEMORY/project_karar_150_act1_envanter_live.md
  - STAGING/graphify_corpus/ folder
  - MEMORY/project_path_c_hybrid_lock.md
  - Wang16 "review later" entries
- ✅ Create target folders if missing (`mkdir -p`)
- ✅ Use `git mv` if files are tracked, plain `mv` if untracked
- ✅ Verify each move with `ls` after
- ✅ Report ANY file already missing (might have been moved earlier) as warning, not error

# REPORT FORMAT
```
# CLEANUP EXECUTION REPORT - <profile> - <time>

## STATUS: DONE | PARTIAL | FAILED

## TIER A (4 single-line edits)
- A1: y/n
- A2: y/n
- A3: y/n
- A4: y/n

## TIER B (4 bulk moves)
- B1 root CODEX_DONE × 12: <moved>/<total>
- B2 s106 stream tasks × 24: <moved>/<total>
- B3 root STAGING pre-S99 × 15: <moved>/<total>
- B4 pilot_a_candidates folder: y/n

## Files skipped (already moved): <list>
## Errors: <list, if any>

## Time: <N> min
```

# Estimated time: 5-10 min
