# Memory Index

| Category | File | Description |
| :--- | :--- | :--- |
| **Active** | [Map Plan v1 LOCK](project_map_plan_v1_lock.md) | **2026-05-18 S91 LOCK:** 16 decisions. Hibrit "monolithic painted background + gameplay overlay" (Hades model). v15h tile composition DEPRECATED for production. MVP = 8-9 room vertical slice, taxonomy LIVE enum (Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor). Spirit of the Goddess 3D yol REJECTED, 2D + Hades painted LOCK. |
| | [Orchestrator Strict Delegation](feedback_orchestrator_delegation_strict.md) | **S91 LOCK:** Orchestrator BULK iş YAPMAZ. >200 satır doc / multi-file analiz / batch sync → sub-agent dispatch. User orchestrator ile sohbet, agent'lar paralel iş yürütür. |
| | [NLM-First Context Strict](feedback_nlm_first_context_strict.md) | **S91 LOCK:** Context query için ÖNCE NLM (30ddffa5-...). Direct file read sadece allowed exceptions (CURRENT_STATUS, PROJECT_RULES, code, STAGING). Sub-agent dispatch'lerine NLM CLI satırı zorunlu ekle. /lint MASTER_KARAR+ANIMATION_BIBLE+FAZ_MASTER+GDD direct okumam YANLIŞTI — tek NLM query yeterdi. |
| | [Canonical Character Roster v2](project_canonical_character_roster_v2.md) | **2026-05-18 LIVE:** 10 class anchor PixelLab IDs locked (Warblade 2656075d, Ronin a7957352, Gunslinger a78545eb, Ranger d5b1cf71, Elementalist 4c83c0be, Shadowblade deee34b5, Ravager 091e9552, Hexer e260a1af, Brawler d4fa3d13, Summoner 83039c80). 4-char playtest focus = Warblade FIRST + Elementalist + Ranger + Shadowblade. |
| | [v15d Composition Budget LOCK](project_v15d_composition_budget_lock.md) | **2026-05-18 LOCK (3-agent verdict):** 20% neg space + 70/20/10 floor + 3 cluster cap + 8 palette/zone + 15% path. AutoPopulator two-pass (reserve→place). 8 layer infrastructure korunur, visually 3-tier hierarchy. Boona-driven. v15e-A L8 cap LIVE, v15e-B secondary cluster cap LIVE. |
| | [5000 PixelLab Allocation LOCK](project_5000_pixellab_allocation_lock.md) | **2026-05-18 LOCK (Opus+Codex synthesis):** 900 chars + 600 mobs + 400 props + 250 VFX + 200 item + 150 HUD + 150 hazard + 200 boss prep + 2150 reserve. Mid-checkpoint after 3150 spend. Phase 1: Warblade+Elementalist+Gunslinger. |
| | [Brush V1 Manual Composition System](project_brush_v1_manual_composition_system.md) | **S88 LIVE:** PlayableRoom 36×22 open-world + Warblade + WASD ready. Manual MCP composition (`execute_code`) bypasses Brush V1 chunked renderer bugs. Hybrid pipeline: Codex imagegen → Python slicer → Unity import → manual SpriteRenderer scatter. PixelLab production workflow appendix (yarın 5000 gen). |
| | [Multi-Projection Arch LOCK](project_multi_projection_architecture_lock.md) | **3-verdict consensus LOCK:** RIMA angled top-down (30-35°). NO HD-2D pivot. Multi-projection architecture prepared, implementation deferred. Phase 1.5 data-first decal migration = critical path. 6 hard rules locked. |
| | [Room Composer Paint-Intent LOCK](project_room_composer_paint_intent_lock.md) | Paint-intent semantic brush + procedural dressing layers. "Paint big → 64/128/256 macro patches" DOĞRU, "Paint big → 32×32 tile pool" YASAK. Brush V1 semantic 3-mode. L2b eklendi. Phase 1A 14-dispatch. |
| | [3D Portability Strategy](project_3d_portability_strategy.md) | **2026-05-17:** Mimari renderer-agnostic — HD-2D/sprite-in-3D'ye port'ta data+logic taşınır, sadece rendering swap. SO interface Unity-rendering ref tutmasın kuralı. |
| | [Hybrid Asset Pipeline](project_hybrid_asset_pipeline_lock.md) | **S87 LOCK Karar #157 candidate:** PixelLab characters/mobs/props + Codex imagegen tiles/walls/decals. v1 painterly mood proven, gpt-image-1 backend. |
| | [Char State Tweaks PENDING](project_character_state_tweaks_pending.md) | **S87 LOCK:** 10 anchor için state tweak listesi (3 mandatory + 6 optional + 1 user-approved Gunslinger outfit). **TRIGGER:** "karakterleri üretecem" → bu listeyi göster. Warblade yaş yönü TBD. |
| | [Canonical Char Roster LOCK](project_canonical_character_roster_lock.md) | **S87 LOCK:** Antigravity 10 Ana → 10-class canonical anchor (5M/5F); tüm PixelLab ID'leri TBD (user yeni create character workflow ile üretecek). |
| | [PixelLab Char States](project_pixellab_character_states_workflow.md) | **LOCK S86+S87 Karar #145 v2:** State-first workflow LIVE; **6 use case** (anim anchor / enemy variant / boss phase / class skin / interp / **conditional variant via natural language**); pilot Warblade. Lauret Studio global'de kanonik. |
| | [Weaponless Anim V1](project_weaponless_animation_v1.md) | **LOCK S86:** Silahsız body + WeaponSR child SR (Karar #144 önerisi, #71+#73 override). |
| | [Combat Feel Research](project_combat_feel_research_combined.md) | **Phase 2 ready:** Bandit Knight + ReBlade tuning matrix (hitstop/shake/slow-mo). |
| | [Juice Features V1](project_juice_features_v1.md) | **BACKLOG:** Top 10 polish features (footprints, embers, coin bounce) Phase 2. |
| | [Music Pipeline](project_music_production_pipeline.md) | **STRATEGY:** Stable Audio Open + REAPER + 2-state dynamic; royalty-free prompts. |
| | [Blueprint-First Map Design](feedback_blueprint_first_map_design.md) | **S88 LATE LOCK:** Map composition 3-adımlı zorunlu: (1) semantic zone blueprint (path/grass/stone/wall/water/feature) → consultation gate, (2) rule-based prop placement (her zone kendi pool'u), (3) adjacency transition decals. Random scatter YASAK. Phase B-3 Blueprint Painter feature. |
| | [Layered Terrain Mandatory](feedback_layered_terrain_mandatory.md) | **S88 SABAH LOCK:** PlayableRoom 3-layer fill zorunlu — Layer 0 base floor (her cell kapanır, asla siyah görünmez, biome'a göre tile), Layer 1 decoration overlay (sparse), Layer 2 props (hero). v15b "boş cell siyah" sorununu çözer. Phase A v15c re-dispatch gerek. |
| | [Sonnet-First Routing](feedback_sonnet_first_routing.md) | **FEEDBACK:** Opus tasarruf, Sonnet sub-agent öncelikli; user has high Sonnet limit. |
| | [Research = Delegate](feedback_research_delegate_to_agents.md) | **FEEDBACK 2026-05-18:** Orchestrator araştırma + harici inceleme + multi-step research synthesis YAPMAZ — rima-research/rima-sonnet/Codex'e delegasyon zorunlu. |
| | [Codex Parallel Profile](feedback_codex_parallel_profile_workflow.md) | **FEEDBACK 2026-05-18:** 3 logged-in profile (laurethgame/laurethayday/yasinderyabilgin), `--profile` flag ile paralel dispatch. Imagegen + refactor + Unity-MCP ayrı profile'larda eşzamanlı. |
| | [Camera Angle + 8-Dir](project_camera_angle_revisit_s43.md) | **S86 UPDATE:** 30-35° + **8-dir LOCK** (5 produce + 3 mirror). |
| | [8-Dir Mirror Strategy](feedback_8dir_mirror_production_strategy.md) | **S86 LOCK:** 5 sprite üret + 3 mirror, weapons Unity child. |
| | [Codex Reviewer 16-18 May](feedback_codex_as_reviewer_until_may18.md) | **TEMP:** Opus implement → Codex review (Claude limit yüksek). |
| | [Style Drift S43](project_style_bible_drift_s43.md) | Sync cheatsheet vs style-bible via rima-doc. |
| | [Class Identity S43](project_class_identity_pivots_s43.md) | Elem orb pivot; Cursemark idle; signature capes. |
| | [Shadow Standard](project_shadow_standard.md) | **LOCKED:** Runtime shadows, no bake. |
| | [Persp. Templates](project_perspective_templates.md) | itch.io template rules + 3 geometry rules. |
| | [PixelLab Map](reference_pixellab_knowledge_map.md) | Older Discord/docs scrape (path pointer). |
| | [PixelLab Tools](reference_pixellab_tool_inventory.md) | **LIVE:** 32 MCP endpoints, size limits, pro/review pipeline (2026-05-16). |
| | [PixelLab Workflow](feedback_pixellab_create_character_workflow.md) | Ref Standard, output=ref size, High Top-Down. |
| | [PixelLab Pro Prompt Format](feedback_pixellab_create_image_pro_format.md) | **S87 LOCK:** Create Image Pro V3'te negative prompt ayrı field YOK — `Negative Prompt :` ana prompt'un sonunda inline. Tek block / batch. |
| | [Char→Web UI V3](feedback_pixellab_character_via_web_ui_v3.md) | **S86 LOCK:** Karakter MCP dispatch YOK — user web UI V3'te manuel yapar. |
| | [Room Library Arch](project_room_library_architecture.md) | **LOCK S86:** Editor-authored runtime-readable RoomBank + SO+Prefab + 20-30 oda MVP. |
| | [S86 Opus Signoff](project_s86_opus_signoff_decisions.md) | **S86 PREP-3:** 6 Codex blocker → kararlar (Sprint 9 thin stub, RIMA.RoomType, file paths, Wang NE-NW-SE-SW + 15 variant). |
| | [Env Props V2](project_environmental_props_v2.md) | **DEFER:** Ağaç + flora çeşitliliği biome-aware V2 scope. |
| | [Natural Paint Integration](feedback_natural_paint_integration.md) | **S86:** Agent'lar paint integration'da composition roles primary, Hades-tone okunabilirlik gözetir — mekanik spec uygulamasın. |
| | [Compact Pass](project_compact_pass_s40.md) | Batch update for 35 files. |
| | [EditorUtility Dialog BAN](feedback_no_dialog.md) | `EditorUtility.DisplayDialog` BANNED in Editor scripts; use `Debug.Log` (MCP timeout prevention) |
| | [Positive Prompts](feedback_negation_to_positive_prompts.md) | Positive RIMA specs mandatory (1:3 negation ratio); narrative anchors + color placement + silhouette signatures required |
| | [No Genre Labels](feedback_pixellab_no_dark_fantasy.md) | Genre labels ("dark fantasy", "grimdark") BANNED in prompts; use visual descriptors instead |
| | [No Left/Right](feedback_pixellab_right_left_bias.md) | Avoid "right/left hand" in PixelLab prompts; use position-based terms; Mirror tool for fixes |
| | [Style Ref Instruction](feedback_pixellab_style_ref_instruction.md) | Standard style-ref instruction text for PixelLab when using Elementalist anchor (camera/scale only, not identity) |
| | [PixelLab MCP Rule](feedback_pixellab_mcp.md) | `mcp__pixellab__create_character` BANNED autonomous; `animate_character` allowed with user-provided character_id |
| | [Model Selection](feedback_model_selection.md) | Sonnet 4.6 default; Opus 4.7 for multi-system architecture/design; Claude chooses, user executes |
| | [MCP Schema Check](feedback_mcp_tool_schema.md) | Always verify MCP tool schema via ToolSearch before writing task files; REST docs != MCP signatures |
| | [Test Workflow](feedback_test_workflow.md) | PlayMode tests at session end/post-refactor; add 1-2 tests to RoomFlowTests.cs per new system |
| | [No Enhance for Anchor](feedback_enhance_pixellab_anchor.md) | "Enhance with AI" BANNED for anchor prompts; use full long-format prompt (Elementalist template) |
| | [S-XL Low Detail](feedback_pixellab_sxl_low_detail.md) | S-XL detail = "Low detailed" for 64px sprites; 256x256 -> downsample 64px nearest-neighbor; High Top-Down BANNED |
| | [Female Class Clothing](feedback_female_class_clothing.md) | Female class clothing = Combat-Practical Exposed; full robes and bikini armor BANNED; per-class rules for Elem/Ranger/Gunslinger/Hexer |
| **Sprite** | [Anchor Cohesion](project_anchor_selections_s43.md) | **LOCKED:** Warblade/Ravager anchors. REGEN Brawler. |
| | [Rift Crack VFX](project_rift_crack_architecture.md) | Line baked, Glow runtime. Use flat fills. |
| **Project** | [RIMA Overview](project_rima.md) | Phase 1, 78 tests passed, IsoGame ready. |
| | [Karar #143 Pipeline](project_karar_143_layered_pipeline.md) | **LOCKED + S86 Wang16:** 6-layer pipeline (L1+L2 LIVE) + L3 Wang Full 16 corner set (eski 7-type iptal). |
| | [Brush Tool V1](project_brush_tool_v1.md) | **SHIP-READY S87 + Phase 1A SO contracts 2026-05-17:** 13/13 sprint LIVE + 5 SO + ImportAssetRole. **328/328 EditMode PASS** (321+7). Existing PaintMode enum maps to ChatGPT semantic 3-mode; TerrainDataWriter executor pending. |
| | [Research-on-Block](feedback_research_on_block.md) | **S85 LOCK:** Use Codex/Gemini/NLM/Web when stuck during autonomous work — don't guess. |
| | [Codex vs Opus Split](feedback_codex_vs_opus_split.md) | **S85 LOCK:** Don't auto-route everything to Codex. Opus for taste/judgment, Codex for spec-following. |
| | [Combat Arch](project_combat_architecture.md) | v4 combat, dash v2, projectile deflect. |
| | [Rift Break](project_rift_break.md) | Meta progression, tier unlocks (Ph 4-5). |
| | [Cross-Class](project_cross_class_skills.md) | 80 skills total, ghost VFX, 2 slots. |
| | [Ghost Attack](project_ghost_attack_system.md) | Ghost sprite animations (12 frames). |
| | [Class Colors](project_class_colors.md) | Dominant color locks for 10 classes. |
| | [Class Balance](project_class_balance.md) | Sim v3, Build vs Boss matrix. |
| | [Sim Philosophy](project_sim_philosophy.md) | Simulated combat logic; contact_scale. |
| | [Item Matrix](project_item_matrix_decisions.md) | Legendary items, body-based animations. |
| | [Gemini Pipeline](project_gemini_concept_pipeline.md) | 8 class concepts ready; Codex QC. |
| | [SFX Pipeline](project_sfx_pipeline.md) | Stable Audio Open 1.0, RTX 5080. |
| | [SFX v2](project_sfx_v2.md) | Guidance_scale=10, seed control. |
| | [HUD Design](project_hud_design.md) | UI layout: HP/Rage, Skills, Map. |
| | [HUD Overlay Decision](project_hud_overlay_decision.md) | **LOCKED:** 3-layer UI (HUD/TAB overlay/ESC pause). |
| | [Char System](project_character_system.md) | ⚠️ STALE — Aseprite composite ref eski; [[weaponless-animation-v1]] yeni spec. |
| | [Warblade Anim](project_warblade_anim.md) | v7 passed; awaiting character_id. |
| | [Idle Poses](project_idle_poses.md) | Anchor positions for all 10 classes. |
| | [Visual Identity](project_character_visual_identity.md) | Hair/Skin/Clothing locks per class. |
| | [Lint Findings](project_lint_findings.md) | S41 fixes done; FAZ_MASTER sync needed. |
| | [S43 Cleanup](project_cleanup_s43_refactor.md) | God object split + 6 legacy scripts deleted. |
| | [Graphify](project_graphify.md) | 252 files, 2162 nodes. |
| | [Mob Sprites](project_mob_sprites.md) | 8 mobs; Imp to Hulk sequence. |
| | [Discord Pipeline](project_discord_export_pipeline.md) | Discord scrape + Ollama digest. |
| | [Backlog](project_rima_backlog.md) | Difficulty modes, Z-axis (Ph 3+). |
| | [VFX Production](project_vfx_production.md) | Slash/Spark/Burst; cold blue/void purple. |
| | [Class Genders](project_class_genders.md) | Male/Female class distribution list. |
| | [ComfyUI Pipe](project_comfyui_pipeline.md) | FLUX on RTX 5080; concepts ready. |
| | [Lighting WIP](project_lighting_wip.md) | Global 0.35 intensity; Ph 2 torch fix. |
| | [Feel Toggles](project_feel_toggles.md) | Shake/Vignette/Hitstop ON. |
| | [Localization](project_localization.md) | TR+EN support; hardcoding BANNED. |
| **Reference** | [Tweet Fetching Workflow](reference_tweet_fetching_workflow.md) | **2026-05-18 LOCK:** x.com WebFetch 402 → use yt-dlp (OP+video) + Playwright (replies, needs cookies). Nitter mirrors all dead. Reusable assets in STAGING/. |
| | [Mechanic Bank Studio](reference_mechanic_bank_studio.md) | **S87 LIVE:** 68 mekanik 10 kategori studio bank (M59-M68 RIMA-anchored) + RIMA adaptation tier S/A/B proposal. |
| | [Asset Map](reference_asset_map.md) | Paths for sprites and prefabs. |
| | [Codex Skills](reference_codex_skills.md) | CLI/Doc/Script tools for Codex. |
| | [Room Mech](reference_room_mechanics.md) | Draft/Echo/Tag/Economy rules. |
| | [Gemini MCP](reference_gemini_mcp.md) | Workspace access for Gemini. |
| | [PixelLab UI](reference_pixellab_v3_ui.md) | V3 Custom Frames (KF+Interp). |
| | [PixelLab Modes](reference_pixellab_create_modes.md) | Pro/Standard tool comparison. |
| | [8-dir Seq](reference_pixellab_direction_sequence.md) | S, SW, W, NW, N, NE, E, SE. |
| | [3D Pipeline](reference_3d_pipeline.md) | Mixamo + AI tool chain. |
| | [PixelLab Create Image Pro](reference_pixellab_create_image_pro.md) | PixelLab Create Image Pro web UI spec -- size table (512px max), prompt formula, style image behavior, Sprint 3 RIMA mapping |
| **User** | [System Specs](user_system_specs.md) | RTX 5080, 9800X3D, 32GB RAM. |
