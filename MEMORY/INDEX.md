# RIMA Memory Index

Open a file ONLY when its keywords match the current task.

**Last regenerated:** 2026-05-26 S108 late-morning (post cleanup).
**Auto-memory canonical:** `C:\Users\ydbil\.claude\projects\F--Antigravity-Projeler-2d-roguelite-RIMA\memory\MEMORY.md` (loaded every session, S107-S108 entries).
**This file:** Project-internal manual curation index. Auto-memory is canonical for orchestrator routing; this index is for explicit memory-file lookup.

---

## LaurethStudio Knowledge Base
- [project_lauretthstudio_2d_illusion_kb_locked.md](project_lauretthstudio_2d_illusion_kb_locked.md) — studio-level 2D illusion catalog (32 teknik + cookbook + 3 platform seed), source: agy 2026-05-26 S109. Full doc in `STAGING/LAURETH_2D_ILLUSION_LIBRARY.md`.
- [wang_tile_build_workflow_rima.md](wang_tile_build_workflow_rima.md) — Studio S15 universal Wang workflow'unun RIMA adapte versiyonu. PixelLab Create Image S-XL Pro (Tiles Pro YASAK), Yöntem A (tek tile) + Yöntem B (composition+grid böl), Aseprite CLI palette remap daemon. Karar #131 + #143 + #98 ile uyumlu. Studio kaynak: `F:/LaurethStudio/MEMORY/studio_custom_wang_build_workflow.md`.
- [active_ai_asset_qa_gate_v2.md](active_ai_asset_qa_gate_v2.md) — RIMA AI Asset QA Gate v2 (Studio S15 transfer 2026-05-27). 10 test çift katman (5 technical + 5 game-artist). PixelLab/AI çıktı production'a girmeden önce zorunlu gate. RIMA palette/scale değerleri adapte. Studio kaynak: `F:/LaurethStudio/01_PIPELINE/2D/ai_asset_qa_gate.md`.

## Faz 1 Milestone Demo (2026-05-27 LOCK)
- [map_fragment_canonical_spec.md](map_fragment_canonical_spec.md) — Map Fragment (Kırık Taş Tablet) canonical: Cyan #00FFCC, hover ±0.10u @ 2.2Hz, alpha pulse 0.6-1.0 @ 3Hz, G key + 2.5u proximity, cyan beam VFX + glyph reveal SFX, 8 fragment Act 1 boss gate threshold.
- [gate_socket_canonical_spec.md](gate_socket_canonical_spec.md) — Gate canonical 8 stil (taş kemer/gedik/merdiven/zincirli/rift/asansör/köprü/tapınak), 1.5-2x karakter boyut, 4-layer composition, lock state machine (Locked/AwaitingFragment/Unlocked), 6-8 frame open anim, 3-slot prefab (N/E/W).
- [warblade_12_common_skills_spec.md](warblade_12_common_skills_spec.md) — Warblade 12 Common (8 active + 3 passive/echo + 1 Death Blow execute), Rage kaynak (pasif dolmaz), demo draft havuzu öncelik 6 (Iron Charge garantili + Earthsplitter/Gravity Cleave/Sunder Mark/Death Blow/Iron Counter).
- [warblade_animation_states_demo_phase1_plan.md](warblade_animation_states_demo_phase1_plan.md) — Warblade demo Faz 1 animation state plan. PixelLab character_state ile first/end keyframe, kullanıcı in-between animate eder. Tier 1 (5 mandatory: Idle/Walk/Attack/Hit/Death) + Tier 2 (4 skill: Dash/Earthsplitter/Counter/Death Blow) + Tier 3 (polish Track B). Prompt template + production sıra + Unity clip mapping.

## 9 Class × 12 Common Skill (Faz 4 Steam Demo scope, 2026-05-27 NLM lock)
- [ronin_12_common_skills_spec.md](ronin_12_common_skills_spec.md) — Tension/Quickdraw/Opened. Wait & Punish fantezi. Havuz 5: Quickdraw/Iaido/Sakura Veil/Counter Draw/Sōken-giri.
- [gunslinger_12_common_skills_spec.md](gunslinger_12_common_skills_spec.md) — Heat/Overheat/Reload. Run-and-gun. Havuz 5: Rift Dash/Fan the Hammer/Cursor Storm/Reload Dance/Deadshot.
- [ranger_12_common_skills_spec.md](ranger_12_common_skills_spec.md) — Focus/Trap/Mark. Mesafe disiplini. Havuz 5: Aimed Shot/Concussive/Explosive Trap/Disengage/Rapid Fire.
- [elementalist_12_common_skills_spec.md](elementalist_12_common_skills_spec.md) — Fire/Frost/Lightbreak. Orb only (NO STAFF Karar #146). Havuz 5: Fireball/Glacial Spike/Living Bomb/Blink/Meteor.
- [shadowblade_12_common_skills_spec.md](shadowblade_12_common_skills_spec.md) — Sever/Scar/Marked. Phase-through identity. Havuz 5: Scarbinding/Phase Step/Backstab Mark/Severance/Smoke Veil.
- [ravager_12_common_skills_spec.md](ravager_12_common_skills_spec.md) — Fury (hasar ALARAK dolar UNIQUE)/Wounded/Frenzy. Havuz 5: Bloodlust Strike/Carnage Spin/Frenzied Leap/Bloodied Roar/Wild Hack.
- [hexer_12_common_skills_spec.md](hexer_12_common_skills_spec.md) — Hex Stacks (0-10, 4 faz). DoT spread. Havuz 5: Corruption/Pandemic/Hexblast/Enervate/Blight Sigil.
- [brawler_12_common_skills_spec.md](brawler_12_common_skills_spec.md) — Charge (0-5)/Cracked/Shattered. Cross-class Sundered consumer (Karar #55). Havuz 5: Bully/Crackjaw/Counter Blow/Curbstomp/Glass Strike.
- [summoner_12_common_skills_spec.md](summoner_12_common_skills_spec.md) — Charges (0-4)/Sic/Sacrifice. Soul Lantern (NO staff swing). Havuz 5: Raise Skeleton/Summon Golem/Corpse Explosion/Commanding Strike/Blood for Power.

## 9 Class Animation State Master Plan + Weapon Master Spec (Faz 4 hazırlık)
- [nine_class_animation_states_demo_phase1_plan.md](nine_class_animation_states_demo_phase1_plan.md) — 9 class master state plan. Cross-class shared base (Idle/Walk/BasicAttack/Hit/Death) + class-spesifik Tier 2 skill state (4 her class). Char ID mapping, Tier 1/2/3 production sıra, PixelLab prompt template universal.
- [weapon_master_spec_10_class.md](weapon_master_spec_10_class.md) — 10 class silah canonical spec. Decouple Karar #144/#123 (HandAnchor child, 1-dir + AnimationCurve runtime rotation), PixelLab S-XL new + Init Image. Mevcut LIVE 2 silah (Warblade longsword + Ronin katana drawn), 8 class production queue Faz 4.

## System & Workflow
- [agents.md](agents.md) — routing, delegate, authority, Codex, Antigravity
- [subagents.md](subagents.md) — sub-agent, rima-doc, rima-qc, rima-asset
- [encoding.md](encoding.md) — ASCII, encoding, Turkish, prompt, internal-doc
- [agent_context_economy.md](agent_context_economy.md) — token savings, context bloat, lean docs
- [statusline.md](statusline.md) — statusline, usage display
- [notebooklm_workflow.md](notebooklm_workflow.md) — NotebookLM workflow, source sync
- [project_nlm_notebook_id.md](project_nlm_notebook_id.md) — RIMA notebook ID, deprecated IDs

## Feedback (Behavioral Rules)
- [feedback_agent_architecture.md](feedback_agent_architecture.md) — router vs reasoning, rima-codex/research role, sub-agent spawn decision
- [feedback_animate_character.md](feedback_animate_character.md) — animate_character tool, char_id
- [feedback_autosprite_vs_pixellab_verdict.md](feedback_autosprite_vs_pixellab_verdict.md) — autosprite vs pixellab, VFX pipeline routing
- [feedback_basic_attack_combo_identity.md](feedback_basic_attack_combo_identity.md) — basic attack, LMB/RMB, combo, weapon speed
- [feedback_camera_lock_hd2d.md](feedback_camera_lock_hd2d.md) — camera lock (HD2D era, may be stale post-topdown-pivot)
- [feedback_canvas_size.md](feedback_canvas_size.md) — canvas size, 128px, frame limit
- [feedback_claude_md_stub.md](feedback_claude_md_stub.md) — CLAUDE.md stub pattern, sub-agent token economy
- [feedback_codex_task_routing.md](feedback_codex_task_routing.md) — UnityMCP routing, cx wrapper, CODEX_TASK.md
- [feedback_current_status_format.md](feedback_current_status_format.md) — status format, lean, compact
- [feedback_gemma_models.md](feedback_gemma_models.md) — gemma, local model, vision
- [feedback_git_attribution.md](feedback_git_attribution.md) — git, commit, attribution
- [feedback_mcp_unity.md](feedback_mcp_unity.md) — Unity MCP, run_tests, compile
- [feedback_memory_system.md](feedback_memory_system.md) — memory, shared memory
- [feedback_nlm_auth_recovery.md](feedback_nlm_auth_recovery.md) — NLM auth expired, login fix
- [feedback_ollama_model_management.md](feedback_ollama_model_management.md) — ollama, gemma, GPU memory
- [feedback_orchestra_discipline.md](feedback_orchestra_discipline.md) — dispatch, mechanical refactor
- [feedback_pixellab_direction.md](feedback_pixellab_direction.md) — direction, blend tree, SW-facing
- [feedback_pixellab_init_image_dimension_lock.md](feedback_pixellab_init_image_dimension_lock.md) — PixelLab init image dimension lock, S-XL new vs Pro tool
- [feedback_pixellab_prompt_structure.md](feedback_pixellab_prompt_structure.md) — PixelLab prompt yazımı, CHARACTER blok, SIZE LOCK
- [feedback_temp_files.md](feedback_temp_files.md) — temp file, one-time report

## PixelLab Production
- [PIXELLAB_TOOL_GUIDE.md](PIXELLAB_TOOL_GUIDE.md) — PixelLab live UI tool map, tool ID + cost + NEW vs PRO (CANONICAL 2026-05-11)
- [pixellab_budget.md](pixellab_budget.md) — PixelLab budget, confirm_cost, batch
- [pixellab_prompt_rules.md](pixellab_prompt_rules.md) — PixelLab prompt, boilerplate
- [pixellab_sprites.md](pixellab_sprites.md) — sprite production, char_id, rotation
- [pixellab_animation_techniques.md](pixellab_animation_techniques.md) — run cycle, interpolation, animate-with-text
- [pixellab_api_reliability.md](pixellab_api_reliability.md) — API error, retry, timeout, polling
- [pixellab_pipeline_workflows.md](pixellab_pipeline_workflows.md) — pipeline, community workflow, spritesheet
- [reference_pixellab_prompt_grammar.md](reference_pixellab_prompt_grammar.md) — PixelLab prompt grammar, RIMA templates, negative prompts

## Project — Active Design State
- [painter_suite_plan_v2_locked.md](painter_suite_plan_v2_locked.md) — LaurethStudio 2D Painter Suite V2 plan (RIMA reuse), S110 P0 -> UPM package
- [painter_suite_progress_2026_05_26.md](painter_suite_progress_2026_05_26.md) — PainterSuite Day 1-4 DONE (UPM scaffold + 4 shape modes + shortcuts + resize handles + templates)
- [painter_suite_v1_1_roadmap_seeds.md](painter_suite_v1_1_roadmap_seeds.md) — Post-v1.0 seeds from X research (GameObject-free + splat shader + auto-collider)
- [project_64px_armed_character_locked.md](project_64px_armed_character_locked.md) — 64px armed character lock (NOTE: 120×120 canvas verified S104; check auto-memory)
- [project_pure_2d_topdown_pivot_2026-05-12.md](project_pure_2d_topdown_pivot_2026-05-12.md) — 2D top-down pivot (HD2D revoke), chibi 64px, asset boyutları
- [project_path_c_hybrid_lock.md](project_path_c_hybrid_lock.md) — Act 1 production pipeline, image_gen floor/wall, S95 HARD LOCK
- [project_class_integration_order.md](project_class_integration_order.md) — class priority, which class next
- [project_player_hades_system.md](project_player_hades_system.md) — player movement, combat aim, facing
- [project_skill_offer_system.md](project_skill_offer_system.md) — skill offers, draft, passive routing
- [project_story_lore.md](project_story_lore.md) — story, lore, RIMA meaning, Rift March
- [project_room_blueprints.md](project_room_blueprints.md) — room catalogue, Act 1 rooms
- [project_room_staging_system.md](project_room_staging_system.md) — map staging, connected generation
- [project_gate_map_reveal.md](project_gate_map_reveal.md) — gates, door sockets, route reveal
- [project_ui_state_blueprint.md](project_ui_state_blueprint.md) — HUD, skill bar, reward UI, gate choice
- [project_dev_tool_rift_makeup.md](project_dev_tool_rift_makeup.md) — dev tool, rift portal, chaos gate, buff visuals
- [project_ui_qa_ai_skills.md](project_ui_qa_ai_skills.md) — UI concept, QA tester flow, Unity tests
- [project_karar_150_act1_envanter_live.md](project_karar_150_act1_envanter_live.md) — Act 1 PixelLab envanter, 119 PNG breakdown
- [project_karar_149_subroom_encounter_lock.md](project_karar_149_subroom_encounter_lock.md) — sub-room sequence, EncounterTemplateSO, 32×22 canvas
- [project_subroom_canonical_tags_lock.md](project_subroom_canonical_tags_lock.md) — sub-room tag, 5 canonical (entry_chamber/pillar_arena/collapse_corridor/ritual_hall/crypt_cell)
- [project_subroom_encounter_system_proposal.md](project_subroom_encounter_system_proposal.md) — sub-room encounter design proposal
- [project_wall_production_pipeline_s99_late.md](project_wall_production_pipeline_s99_late.md) — wall pipeline (NOTE: V1 wall-less LOCK 2026-05-25 supersedes; check auto-memory)
- [project_warblade_weapon_animation_plan_s99_late.md](project_warblade_weapon_animation_plan_s99_late.md) — weapon sprite, HandAnchor scaffold, Warblade animation
- [project_autosprite_trial_pending.md](project_autosprite_trial_pending.md) — autosprite MCP VFX pilot
- [project_chatgpt_canvas_fix.md](project_chatgpt_canvas_fix.md) — ChatGPT tile üretimi canvas fix
- [project_test_automation.md](project_test_automation.md) — test otomasyon, contract test, behavioral testing

## Research (Digested)
- [discord_pipeline.md](discord_pipeline.md) — Discord analysis, PixelLab research
- [discord_monitoring_channels.md](discord_monitoring_channels.md) — Discord channel monitor
- [discord_automation_risk.md](discord_automation_risk.md) — Discord automation, bot detection

---

## Notes on Drift
- Some S99-S100 era entries (wall pipeline, 64px armed character) may be superseded by S104-S108 locks in auto-memory.
- Always check auto-memory (`MEMORY.md` loaded in session prompt) for current LIVE locks before relying on individual entries here.
- Archive folder `_archive_overnight_2026_05_26/` contains stale entries moved during S107 overnight cleanup.

## Add Memory
Create `MEMORY/<topic>.md` with frontmatter, then add one line here.
