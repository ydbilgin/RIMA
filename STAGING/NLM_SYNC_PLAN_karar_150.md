# NLM Sync Plan — Karar #150 LIVE + #149 canonical correction

**Date:** 2026-05-19 S94 LATE NIGHT
**Last NLM sync:** 2026-05-18 20:38 (state)
**Total unsynced:** 93 files

**Reason:** NLM canonical hâlâ pre-Karar #149 "1 oda = 1 arena wave-based" modeli dönüyor (NLM query bu session evidence). Karar #149 LIVE (S94 morning) + Karar #150 LIVE (S94 LATE NIGHT) NLM'e push edilmemiş — sub-agent dispatch'leri yanlış routing yapar.

---

## Priority 1 — Karar #149 + #150 canonical correction (push FIRST)

Bu 9 file NLM canonical drift'i düzeltiyor. Sub-agent routing doğru çalışsın diye en erken push.

| # | File | İçerik | Why critical |
|---|---|---|---|
| 1 | `TASARIM/MASTER_KARAR_BELGESI.md` | Karar #148 + #149 + #150 LIVE wordings | THE source of truth. Drift root cause |
| 2 | `CURRENT_STATUS.md` | S94 LATE NIGHT + Karar #150 LIVE status | Session state, canonical correction signal |
| 3 | `STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md` | Full Karar #150 spec + 3-Act material | Karar #150 LIVE definitive doc |
| 4 | `STAGING/CODEX_DONE_karar_150_review.md` | Codex APPROVE_WITH_REVISIONS verdict | Karar #150 review evidence |
| 5 | `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md` | Karar #149 sub-room sequence tech spec | Sub-room mechanic canonical |
| 6 | `STAGING/CODEX_DONE_subroom_encounter_review.md` | Karar #149 Codex verdict | Sub-room review evidence |
| 7 | `STAGING/ROADMAP_dungeon_buildup.md` | 6-faz piece-by-piece discipline | Production discipline canonical |
| 8 | `STAGING/RIMA_FRESH_ASSET_PLAN.md` | Karar #150 fresh asset gen path | Asset production canonical |
| 9 | `STAGING/UNITY_LEGACY_CLEANUP_PLAN.md` | Karar #150 cleanup plan | Asset cleanup decisions canonical |

**Execution:** Tek tek `/nlm-sync <path>` veya hepsi paralel script.

```bash
# Priority 1 sync script
NB=30ddffa5-292f-4248-8e77-68074af901be
REPO="F:/Antigravity Projeler/2d roguelite/RIMA"

FILES=(
  "TASARIM/MASTER_KARAR_BELGESI.md"
  "CURRENT_STATUS.md"
  "STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md"
  "STAGING/CODEX_DONE_karar_150_review.md"
  "STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md"
  "STAGING/CODEX_DONE_subroom_encounter_review.md"
  "STAGING/ROADMAP_dungeon_buildup.md"
  "STAGING/RIMA_FRESH_ASSET_PLAN.md"
  "STAGING/UNITY_LEGACY_CLEANUP_PLAN.md"
)

for rel in "${FILES[@]}"; do
  echo "Syncing: $rel"
  # use /nlm-sync single-file mode (delete-then-add by basename)
  # actual command embedded in nlm-sync.md
done
```

Tercih: `/nlm-sync` batch mode (priority filter atlamış olur ama hash-based, hepsini bir kerede push eder).

---

## Priority 2 — Recent S94 design context (sync next)

Karar #150 öncesi S94 sabah-öğle research/decision dokümanları. Sub-agent context için faydalı ama canonical drift değil.

- `STAGING/RIMA_VISUAL_PRODUCTION_PLAN.md`
- `STAGING/RIMA_WALL_INVENTORY_AND_CANON.md`
- `STAGING/OPUS_WALL_FINAL_DECISION.md`
- `STAGING/OPUS_WALL_PRODUCTION_DESIGN.md`
- `STAGING/RESEARCH_dungeon_scene_integration.md`
- `STAGING/RESEARCH_rima_project_knowledge.md`
- `STAGING/RESEARCH_hades_art_pipeline.md`
- `STAGING/ASSET_PACK_ORGANIZATION_PLAN.md`
- `STAGING/CODEX_DONE_concept_v2_35deg.md`
- `STAGING/CODEX_DONE_concept_v3_fakeiso.md`
- `STAGING/CODEX_DONE_concept_v4_inside_dungeon.md`
- `STAGING/PIXELLAB_OBJECT_PRODUCTION.md`
- `STAGING/EPIC_MECHANIC_AND_CB_PIVOT_OPUS.md`
- `STAGING/OPUS_DONE_skill_bank_balance_review.md`
- `STAGING/RIMA_4CLASS_SKILL_DESIGN_BANK.md`
- `STAGING/RIMA_MECHANIC_ANTI_GENERIC_OPUS.md`
- `STAGING/CODEX_DONE_full_autonomy_pipeline.md`
- `STAGING/CODEX_DONE_ronin_implementation.md`
- `STAGING/CODEX_DONE_review_tile_angle_verdict.md`
- `STAGING/TILE_ANGLE_ARCHITECTURE_OPUS.md`
- `STAGING/MORNING_BRIEFING.md`
- `STAGING/NEXT_SESSION_PICKUP.md`
- `STAGING/NIGHT_WORK_PLAN.md`
- `STAGING/WAVE_E_DONE.md`

---

## SKIP — sync etme (noise, archive candidate)

Bunlar NLM'e gitmemeli — eski iteration / external lib / completed task prompt'ları.

### wang16_full_autonomy_research/ — external library docs (SKIP TÜMÜ)

`STAGING/wang16_full_autonomy_research/` altındaki TÜM .md → external Wang16 library README/docs. RIMA Wang16 KAPATILDI (CB pivot). Bu klasör STAGING/_archive/'a taşınmalı veya `.nlmignore` benzeri filter eklenmeli.

- `WangTiling/README.md`
- `Wangscape/{CODE_OF_CONDUCT, CONTRIBUTING, README, STYLE_GUIDE, doc/algorithm}.md`
- `content_aware_tiles/README.md`
- `perlin-wang/README.md`
- `wangTiles/README.md`
- `wangTiles/experimental_code/goxel/{INTERNALS, README}.md`
- `wangTiles/experimental_code/py-vox-io/README.md`

### CB pivot dokümanları (SKIP — RIMA scope dışı)

- `STAGING/CB_VISION_DOC_draft.md`
- `STAGING/CODEX_TASK_cb_pivot_design_review_DONE.md`
- `STAGING/CODEX_TASK_cb_synthesis_subgenre_DONE.md`
- `STAGING/CODEX_TASK_cb_vision_doc_review_DONE.md`
- `STAGING/codex_task_cb_*.md`
- `STAGING/CODEX_DONE_review_cb_pivot_epic_mechanic.md`
- `STAGING/CODEX_DONE_wang16_compositor_review.md`
- `STAGING/codex_task_wang16_compositor_review.md`
- `STAGING/CODEX_DONE_pixellab_flat_test_review.md`
- `STAGING/codex_task_pixellab_flat_test_review.md`

CB ayrı game olduğu için RIMA NLM'inde noise yapar.

### Completed task prompts (low value)

`codex_task_*.md` task prompt'ları (input), `CODEX_DONE_*.md` verdict (output) önemli. Prompt'lar verdict yapıldıktan sonra historical record. SKIP edilmez ama Priority 3'e atıla:

- `STAGING/codex_task_concept_v2_35deg.md`
- `STAGING/codex_task_concept_v3_fakeiso.md`
- `STAGING/codex_task_concept_v4_inside_dungeon.md`
- `STAGING/codex_task_encounter_step2.md`
- `STAGING/codex_task_encounter_template_step1.md`
- `STAGING/codex_task_full_autonomy_pipeline_research.md`
- `STAGING/codex_task_morning_briefing_synthesis.md`
- `STAGING/codex_task_opus_mechanic_verdict_review.md`
- `STAGING/codex_task_review_tile_angle_verdict.md`
- `STAGING/codex_task_rima_act1_reference_concept_art.md`
- `STAGING/codex_task_ronin_implementation.md`
- `STAGING/codex_task_seamless_granite_tile_gen.md`
- `STAGING/codex_task_subroom_encounter_review.md`
- `STAGING/codex_task_topdown_floor_pipeline_research.md`
- `STAGING/codex_task_wall_inventory_curation.md`
- `STAGING/codex_task_wall_spec_review.md`
- `STAGING/research_task_free_asset_alternatives.md`
- `STAGING/opus_task_cb_pivot_epic_mechanic.md`
- `STAGING/opus_task_skill_bank_balance_review.md`
- `STAGING/opus_task_tile_angle_architecture.md`
- `STAGING/CODEX_DONE_opus_mechanic_verdict_review.md`
- `STAGING/CODEX_DONE_seamless_granite_tile.md`
- `STAGING/CODEX_DONE_topdown_floor_pipeline_decision.md`
- `STAGING/CODEX_TASK_karar_150_review.md`
- `STAGING/CODEX_TASK_pre_cleanup_fixes.md`
- `STAGING/CODEX_DONE_rima_act1_concept.md`
- `STAGING/RESEARCH_DONE_free_asset_alternatives.md`
- `STAGING/RESEARCH_mechanic_bank_summary.md`
- `STAGING/RESEARCH_tinyworld_build.md`
- `STAGING/RESEARCH_yudou_tweet.md`
- `STAGING/ANTIGRAVITY_PROMPT_visual_rethink.md`
- `STAGING/CLAUDE_PROMPT_fake_isometric_dungeon.md`
- `STAGING/chatgpt_tile_prompts_RIMA_style.md`
- `STAGING/chatgpt_tile_prompts_act1_2_3.md`
- `STAGING/codex_task_4class_skill_bank.md`
- `STAGING/pixellab_create_image_pro_prompts.md`
- `STAGING/graphify_corpus/graphify-out/obsidian/Trigger Phrase 'karakterleri üretecem'.md`

---

## Execute strategy

**Önerilen:** Codex pre-cleanup PASS sonrası 3-aşamalı sync:

### Aşama 1 (NOW-READY, Codex'i beklemez)
9 Priority 1 file tek tek `/nlm-sync <path>` veya curated batch script. **Asıl drift-correction bu 9 file**.

### Aşama 2 (Codex pre-cleanup PASS sonrası)
Priority 2 + Priority 3 files. `/nlm-sync` batch mode = tüm 93 unsynced'i push. wang16 noise dahil olur — ya batch öncesi wang16 klasörü `_archive_wang16/`'ya taşı, ya da sync sonrası NLM'den manual sil.

### Aşama 3 (cleanup sonrası)
`/nlm-sync --cleanup-dry` — archive/delete edilmiş local file'ların NLM'deki orphan'larını listele → onayla → `/nlm-sync --cleanup`.

---

## Risk flags

1. **Batch sync 93 file** = rate limit riski. NLM CLI paralel push yapar; rate limit yerse hata. Eğer fail → Priority 1 9-file öncelik prensibini koru.
2. **wang16_full_autonomy_research/** = STAGING/'da bırakılmamalı. Cleanup execute (Codex PASS sonrası) bu klasörü `_archive/`'ya taşımalı VEYA `STAGING/wang16_full_autonomy_research/` `_archive_wang16/`'ya rename.
3. **Memory dosyaları sync EDİLMİYOR** (`C:\Users\ydbil\.claude\projects\...\memory\` user-personal scope, repo MEMORY/ farklı). Repo `MEMORY/` zaten old INDEX-based memory. Karar #149/#150 memory user-personal scope'ta — NLM'e gitmiyor. Bu OK, çünkü Karar #150 spec STAGING + MASTER_KARAR'da zaten canonical.

---

## Recommendation

**Bu session:** Priority 1 (9 file) sync edilebilir Codex beklerken. File çakışması yok — markdown push vs C# Codex edit.

**Codex PASS sonrası:** Aşama 2 batch. Önce wang16_full_autonomy_research/ archive (cleanup batch dahil), sonra `/nlm-sync` full batch.
