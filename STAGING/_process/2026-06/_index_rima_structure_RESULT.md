# RIMA Organizasyon Yapısı İndeksi

Bu dosya, RIMA projesinin tüm organizasyonel yapısını ve bellek bileşenlerini tahrif etmeden listeleyen bir salt-okunur envanter raporudur.

## A. .claude/ Envanteri

| Dosya Yolu | Boyut (Byte) | Ne İşe Yaradığı |
|---|---|---|
| PROJECT_RULES.md | 12270 | RIMA projesine özgü ana agent kuralları (mimariler, kodlama prensipleri, roller). |
| settings.json | 1964 | Kullanıcı tarafından izin verilen UnityMCP ve PixelLab MCP araç listesini barındıran ayar dosyası. |
| codex_anchor_prompt.txt | 4562 | Codex modelinin RIMA projesinde çalışırken uyacağı temel yönlendirme promptu. |
| codex_anim_guides_prompt.txt | 9830 | Animasyon üretiyor ve doğruluyor iken kullanılacak animasyon yönlendirmeleri promptu. |
| codex_anim_prompt_update.txt | 18149 | Animasyon durum makinesi ve sprite güncellemeleri için geliştirilmiş prompt şablonu. |
| codex_arch_review_prompt.txt | 2318 | Mimari tasarım değişikliklerinin test edilmesi ve gözden geçirilmesi promptu. |
| codex_brush_prompt.txt | 2288 | Room Designer içindeki Brush fırça araçları üretimi için özel Codex promptu. |
| codex_brush_task.md | 18159 | Room Designer F1 Brush fırçası görev şablonu ve gereksinimleri. |
| codex_chibi_lore_review_output.txt | 20568 | Chibi karakter lore stress testi çıktısı. |
| codex_chibi_lore_review_prompt.txt | 7825 | RIMA 2D chibi karakter lore tasarımını stress-test eden prompt. |
| codex_concept_art_f1_2_prompt.txt | 14740 | PixelLab için konsept sanat üretimi promptu. |
| codex_f2_t1t2_prompt.txt | 3741 | Faz 2 - Tier 1 ve Tier 2 animasyonlarının üretimi promptu. |
| codex_ghost_attack_review_prompt.txt | 2401 | Hayalet saldırı sistemi tasarım değişikliğinin stress-test promptu. |
| codex_open_vista_review_prompt.txt | 1819 | Open Vista mimari tasarım taslağı stress-test promptu. |
| codex_pixellab_video_synthesis_prompt.txt | 4514 | PixelLab video kaynaklarından RIMA-spesifik ipuçları üretimi promptu. |
| codex_roomdesigner_f1_compact.txt | 1058 | Room Designer compact görev promptu. |
| codex_roomdesigner_f1_prompt.txt | 8765 | Room Designer F1 tasarım görevi promptu. |
| codex_s60_design_bundle_review_prompt.txt | 10099 | Sprint 60 tasarım paketlerinin stress-test promptu. |
| codex_scene_gen_prompt.txt | 8747 | Sahne oluşturma aracı için özel prompt. |
| codex_sprite_qc_prompt.txt | 4916 | AI tarafından üretilen 16-slot sprite batch görselinin kalite kontrol promptu. |
| codex_t3a_prompt.txt | 7709 | T3A görevi için geliştirilmiş Codex promptu. |
| codex_t3b_prompt.txt | 10133 | T3B görevi için geliştirilmiş Codex promptu. |
| codex_tiled_task.txt | 16347 | Tiled harita editörü üretim görevi promptu. |
| e1e2_codex_prompt.txt | 6138 | E1/E2 görevleri için özel override promptu. |
| nlm_last_sync.txt | 17 | NotebookLM ile yapılan son senkronizasyon zaman damgası. |
| nlm_sync_state.txt | 289356 | NotebookLM senkronizasyon durumu ve dosya MD5 hash listesi. |
| run_codex_brush.py | 564 | Room Designer Brush aracı Codex görevini başlatan Python betiği. |
| run_codex_f1.sh | 347 | F1 görevi başlatıcı kabuk betiği. |
| run_codex_f1_compact.sh | 348 | F1 compact görevi başlatıcı kabuk betiği. |
| run_codex_t3a.py | 8715 | T3A görevlerini otonom başlatan python betiği. |
| scheduled_tasks.lock | 91 | Zamanlanmış görevlerin kilitlenme durumunu tutan lock dosyası. |
| terrain_blend_prompt.md | 8096 | Terrain Blend sistemi üretimi için Claude kodlama promptu. |
| agents/rima-asset.md | 2858 | PixelLab promptları ve animasyon planlamaları üretmek için kullanılan alt-agent. |
| agents/rima-design.md | 2550 | Sistemler arası karmaşık tasarım kararları (Opus) için kullanılan alt-agent. |
| agents/rima-doc.md | 2276 | Döküman güncellemeleri, memory yazımları ve arşivleme için kullanılan alt-agent. |
| agents/rima-qc.md | 3276 | Codex çıktılarını incelemek ve kod kalitesi kontrolü yapmak için kullanılan alt-agent. |
| agents/rima-research.md | 867 | Dış kaynak araştırmaları yapmak için gemini -p ile bağlantılı alt-agent. |
| agents/rima-sonnet.md | 1854 | Çok adımlı analizler ve planlamalar için genel amaçlı Sonnet alt-agent. |
| commands/codex-task.md | 2080 | Codex görev delegasyonunu formatlayan komut (/codex-task). |
| commands/commit.md | 2634 | Grup halinde değişiklikleri commit eden komut (/commit). |
| commands/lint.md | 1856 | Knowledge base sağlık kontrolü ve yetim dosya tarama komutu (/lint). |
| commands/nlm-sync.md | 8428 | Bellek ve tasarım dosyalarını NotebookLM ile senkronize eden komut (/nlm-sync). |
| commands/nlm.md | 443 | NotebookLM üzerinden tasarım kararlarını sorgulayan komut (/nlm). |
| commands/phase-close.md | 2021 | Faz kapanış adımlarını, testleri ve arşivleri çalıştıran komut (/phase-close). |
| commands/plan.md | 2793 | Uygulama planı üretip risk analizi yapan komut (/plan). |
| commands/playtest.md | 6462 | Otomatik playtest görevleri üreten komut (/playtest). |
| commands/promptforge.md | 1044 | Ham tasarım fikirlerini Gemini prompt formatına dönüştüren komut (/promptforge). |
| commands/save-session.md | 9977 | Oturum durumunu güncelleyip yedekleyen komut (/save-session). |

## B. Global Skills

| Skill Adı | Açıklama | Proje-Bağımsız mı (E/H) | RIMA Hardcode Notu |
|---|---|---|---|
| ax_flash | Bir görevi ax (Gemini 3.5 Flash High) ile dispatch et - yalın/hızlı bilgi-toplama, lean eleştiri, ship-fast lens. | H | Ortak auth manager dosyasına referans veriyor |
| ax_opus | Bir görevi ax (Claude Opus 4.6 Thinking) ile dispatch et - KRİTİK review/karmaşık implementasyon/cx-BLOCKED. | H | Ortak auth manager dosyasına referans veriyor |
| ax_pro | Bir görevi ax (Gemini 3.1 Pro High) ile dispatch et - derin analiz, mimari/tasarım yargısı, vision. | H | Ortak auth manager dosyasına referans veriyor |
| brainstorming | Yaratıcı süreçlerde gereksinimleri ve fanteziyi geliştirme amaçlı genel yetenek. | E |  |
| cx | Bir görevi cx (Codex, gpt-5.5) ile dispatch et - kod yazma, Unity değişiklik, mekanik batch, review. | E |  |
| find-skills | Skill bulma ve kurma yardımcısı. | E |  |
| flux-2-klein | Flux resim üretim modeli entegrasyonu. | E |  |
| generate2dmap | Görsel ve kod tabanlı 2D harita tasarımı yeteneği. | E |  |
| generate2dsprite | 2D sprite sayfası üretme ve postprocess yeteneği. | E |  |
| gpt-image-2 | GPT tabanlı resim üretimi yeteneği. | E |  |
| graphify | Girdileri knowledge graph'a dönüştürme yeteneği. | E |  |
| humanizer | Metin insansılaştırıcı. | E |  |
| image-inpainting | Görsel iç boyama yeteneği. | E |  |
| image-outpainting | Görsel dış boyama yeteneği. | E |  |
| impeccable | Sanatsal eleştiri ve tasarım geliştirme yeteneği. | E |  |
| pixel-cleanup | AI tarafından üretilen pixel art PNG'lerindeki parazitleri temizleme yeteneği. | H | SKILL.md içinde RIMA yolu barındırıyor |
| pixelify | HD görselleri PixelLab referanslarıyla pixel art'a dönüştürme yeteneği. | H | ppu_grid_reference.md referansı içeriyor |
| rima-conventions | RIMA proje kurallarını tarayarak ihlalleri bulma yeteneği. | H | Tamamen RIMA kurallarına özel tasarlanmış |
| skill-creator | Yeni yetenekler üretme yeteneği. | E |  |
| subagent-driven-development | Alt agentlar ile paralel geliştirme disiplini yeteneği. | E |  |
| systematic-debugging | Sistematik hata ayıklama yeteneği. | E |  |
| unity-mcp-skill | MCP üzerinden Unity Editörü yönetme yeteneği. | E |  |
| verification-before-completion | Görevi bitirmeden önce test ve doğrulama disiplini yeteneği. | E |  |
| write-a-skill | Yetenek yazma rehberi. | E |  |
| yt-transcript | YouTube transkript çekici. | E |  |

## C. Memory (user-level)

Konum: `C:\Users\ydbil\.claude\projects\F--Antigravity-Projeler-2d-roguelite-RIMA\memory\`

| Dosya | Type | Açıklama Satırı | Bayraklar |
|---|---|---|---|
| feedback_2track_gameplay_decor_strategy.md | feedback | RIMA orchestrator iki paralel track ile ilerler: | orphan, stale_pit |
| feedback_8dir_mirror_production_strategy.md | feedback | **Rule:** RIMA karakter sprite'ları **8 yön LOCKED** (S, SE, E, NE, N, NW, W, SW). Production: **5 yön sprite üret** (S, N, E, SE, NE), **3 yön Uni... | orphan, stale_pit |
| feedback_agent_dispatch_model_explicit.md | feedback | Agent tool kullanırken `model` parametresi explicit verilmezse parent'ın modeli inherit edilir. Parent Opus 4.7 → tüm general-purpose / rima-* agen... | orphan, stale_pit |
| feedback_agy_always_detached_wrapper.md | feedback | 🚫 HARD (2026-05-29, S114 S6 — user: "bu pythonla calistirma isini kalici kapat wrapperla calistir"): | orphan, stale_pit |
| feedback_agy_dispatch_cmd_wrapper.md | feedback | 🚫 HARD 2026-05-26 evening (S109 close revision) — agy dispatch için `.cmd` wrapper kullan, doğrudan `python agy_dispatch.py` değil. | orphan, stale_pit |
| feedback_agy_dispatch_offscreen_console_fix.md | feedback | User reports while dispatching agy: "agy dispatch'te hemen bi ekran açılıp geri kapanıyor ama oyun oynarken falan ekranıma geliyor direkt". | orphan, stale_pit |
| feedback_agy_dispatch_unique_output_sequential.md | feedback | When dispatching MORE THAN ONE agy (ax) task, make each save to a **distinct output location** so results don't mix — OR run them strictly **sequen... | orphan, stale_pit |
| feedback_agy_inline_response_only.md | feedback | **Always include this in agy_dispatch task files:** | orphan, stale_pit |
| feedback_agy_invocation_isolation.md | feedback | Kural: agy CLI'i ASLA mevcut PowerShell session'ında direkt `& $agy ...` ile çağırma. agy server-based çalışıyor (gRPC + HTTP localhost ports), ayn... | orphan, stale_pit |
| feedback_agy_print_term_env_fix.md | feedback | **Always set `$env:TERM = "xterm-256color"` (and ideally `$env:COLORTERM = "truecolor"`) before any `agy --print` invocation on Windows.** | orphan, stale_pit |
| feedback_agy_research_synthesis_plus_raw.md | feedback | User directive (2026-05-29, S114 S6): **agy (Antigravity) is especially strong at web search.** When dispatching agy for research, the task must as... | orphan, stale_pit |
| feedback_agy_snapshots_live_config.md | feedback | 🚫 HARD 2026-05-26 (S109). | orphan, stale_pit |
| feedback_antigravity_in_every_pipeline.md | feedback | **User explicit directive (2026-05-25 morning):** "antigravity'i de pipeline'a dahil edeceksin" | orphan, stale_pit |
| feedback_antigravity_prompt_format.md | feedback | **Kural:** Antigravity'ye verilecek görev → orchestrator user'a **doğrudan yapıştırılabilir prompt** olarak sunar. User Antigravity IDE'ye yapıştırır. | orphan, stale_pit |
| feedback_autonomous_no_block.md | feedback | 🚫 HARD RULE 2026-05-27 gece: Orchestrator otonom mode'da. Sıralı task'lar otomatik dispatch eder. Kritik karar gerektiğinde sorar ama akışı **durd... | orphan, stale_pit |
| feedback_autonomous_workflow_loop_playable_s6.md | feedback | 2026-06-02: user told Opus to run RIMA toward a PLAYABLE game AUTONOMOUSLY via a backlog of workflows, then move to animations (later); audio defer... | orphan |
| feedback_ax_fastest_failover_if_stuck.md | feedback | 🚫 HARD 2026-05-31: **ax = Gemini Flash (High) is the fastest agent — default to it for research/analysis.** But agents sometimes hang (e.g. ax-ydb... | orphan |
| feedback_ax_gemini_flash_mcp_info_gathering.md | feedback | The user explicitly OKs routing **MCP-driven information-gathering / collection tasks** to **ax Gemini 3.5 Flash High** — "bu tür görevleri gayet i... |  |
| feedback_ax_prompt_small_cx_codemod_flaky.md | feedback | **ax/agy (antigravity Gemini via ax_dispatch ConPTY) HANGS on huge prompts.** 2026-06-03: an ~88KB inline prompt (two reference docs concatenated) ... | orphan |
| feedback_blueprint_first_map_design.md | feedback | **Kural:** Map composition dispatch'i (Codex pro redesign, Phase A v* iterasyonu, vb.) **3 adımlı sırayla** çalışır. Random prop scatter YASAK; her... | orphan, stale_pit |
| feedback_brief_short_lowrisk_constraints_destructive.md | feedback | 2026-06-05, user: "sen buna görev verirken... o da akıllı, şunu istiyorum de, gitsin yapsın rapor versin, full anlatma" — agents are smart, don't s... | orphan |
| feedback_character_state_planning_before_production.md | feedback | **Kural:** Bir karakter (player class veya mob) için PixelLab V3 web UI'da üretime başlamadan ÖNCE, **gerekli state'lerin tamamı listelenir**. Orch... | orphan, stale_pit |
| feedback_chatgpt_pixellab_hybrid_workflow.md | feedback | **2026-05-21 S97 LATE NIGHT LOCK:** Asset pack üretim için **ChatGPT/DALL-E + PixelLab Create Image Pro hybrid** = RIMA standardı. Pure PixelLab da... | orphan, stale_pit |
| feedback_chatgpt_web_github_review.md | feedback | Whatever gets committed+pushed to GitHub (master) is readable by the user's ChatGPT-web (private repo is connected). So ChatGPT-web is effectively ... |  |
| feedback_clean_project_no_clutter.md | feedback | **Source:** User feedback 2026-05-23 after RIMA → RIMA_HD2D migration. RIMA had accumulated ~35GB clutter (463MB twitter research, 160MB archives, ... | orphan, stale_pit |
| feedback_code_writer_rotation.md | feedback | 🚫 HARD RULE 2026-05-27 gece: Kod yazımı tek agent değil, rotation. Orchestrator iş tipine göre yazan + review eden ayırır. | orphan, stale_pit |
| feedback_codex_agy_dispatch_invocation_fix.md | feedback | HARD 2026-05-28 (S114 S3): Iki dispatch bug'i teshis + fix edildi. | orphan, stale_pit |
| feedback_codex_agy_profile_race.md | feedback | 🚫 HARD RULE 2026-05-27 gece S113: agy_dispatch + cx_dispatch paralel çalıştırılırsa **aynı Codex profile'ini kapatamazlar**. Race condition → ikin... | orphan, stale_pit |
| feedback_codex_as_reviewer_until_may18.md | feedback | **Rule:** Bu 3 günlük pencerede (16-18 Mayıs 2026) **Codex reviewer rolünde**. Claude Code (Opus/Sonnet) implementation yazar, Codex sadece **bağla... | orphan, stale_pit |
| feedback_codex_effort_xhigh_2026_05_24.md | feedback | **HARD RULE:** Codex dispatch atarken `cx_dispatch.py --effort xhigh` kullan, `high` DEĞİL. | orphan, stale_pit |
| feedback_codex_fix_unity_errors_always.md | feedback | **Kural (LOCK 2026-05-21):** Unity'ye dokunan her Codex dispatch'inde (UnityMCP, scene edit, script create, asset import, vs.): | orphan, stale_pit |
| feedback_codex_imagegen_skill_not_env_var.md | feedback | **HARD RULE:** Codex dispatch'lerinde `$imagegen` skill kullanırken **explicit built-in tool mode** talimat ver. CLI fallback (`scripts/image_gen.p... | orphan, stale_pit |
| feedback_codex_parallel_profile_workflow.md | feedback | User feedback (2026-05-18): "codex için farklı profilleri aynı anda çalıştırabilirsin cx ile biriyle imagegen yapıp biriyle review ya da biriyle un... | orphan, stale_pit |
| feedback_codex_stale_lock_after_taskstop.md | feedback | 2026-05-25 S106 overnight session — after 3 TaskStop calls (Streams G partial, M, N, P early kill), all 3 profile locks remained: `laurethayday.loc... | orphan, stale_pit |
| feedback_codex_vs_opus_split.md | feedback | User: "her şeyi codexe dispatch ediyor gibisin bunun için sonnet opus da kullanabilirsin agent olarak" | orphan, stale_pit |
| feedback_compile_in_unity_autonomous.md | feedback | 🚫 HARD 2026-05-31: When doing autonomous code work, after each code change **recompile in Unity** (`refresh_unity` with compile=request, then `rea... | orphan |
| feedback_consult_gemini_flash_before_decisions.md | feedback | Before making a non-trivial decision, the user wants me to first consult **Gemini 3.5 Flash (High)** — it answers fast — harvest any good ideas, bu... | orphan |
| feedback_current_status_lean.md | feedback | CURRENT_STATUS.md sadece 1 aktif RESUME bloğu tutar (~50 satır max). Session sonu yeni blok yazılırken ESKİ BLOK SİLİNİR — prepend yapılmaz. Eski i... |  |
| feedback_cx_dispatch_auto_discover.md | feedback | 🚫 HARD 2026-05-29 (S114 S5): `cx_dispatch.py` artık Codex profillerini **`cx accounts`'tan OTOMATİK keşfeder** — eski hardcoded `PROFILE_ORDER` al... | orphan, stale_pit |
| feedback_cx_parallel_different_profiles.md | feedback | Two (or more) `cx_dispatch.py` tasks CAN run concurrently as long as each uses a DIFFERENT `--profile`. The dispatcher writes each task's result to... | orphan |
| feedback_cx_profile_order_laureth_yekta_yasin.md | feedback | User-set cx (Codex) dispatch profile PRIORITY ORDER (2026-06-02): **1) laurethayday → 2) yekta → 3) yasinderyabilgin (last)**. Use laurethayday by ... | orphan |
| feedback_cx_profile_yekta_last.md | feedback | cx dispatch profil önceliği (kullanıcı 2026-06-11, ÖNCEKİ tüm cx-order notlarını SUPERSEDE eder): |  |
| feedback_cx_review_only_routing.md | feedback | 2026-06-07 akşam, kullanıcı talimatı: "codexi reviewe kullan limitim bitiyor; opus/sonnet kullanabilirsin; ax opus 4.6 kullanabilirsin; basit işler... | revoked |
| feedback_depth_sort_custom_axis_not_manual_ysort_s6.md | feedback | User: player renders ON TOP of pillars (no 3D depth), stands on pillars, gets stuck near edges/bridges. | orphan, stale_pit |
| feedback_disabled_profile_hard_ban.md | feedback | 2026-06-05: laurethayday BLOCKED olunca T5'i `--profile yekta` ile dispatch ettim; kullanici uyardi: "yekta disabled olmasina ragmen hala kullaniyo... |  |
| feedback_double_auto_regen_avoid.md | feedback | 🚫 HARD 2026-05-26 evening (S109). | orphan, stale_pit |
| feedback_enhance_pixellab_anchor.md | feedback | * **Rule:** Anchor prompts MUST use full long format (Elementalist template). | orphan |
| feedback_fable_diagnose_agents_execute.md | feedback | Kullanıcı 2026-06-10: "sen fable modelisin 22 hazirana kadar kullanabilecem seni o yüzden pahalısın ama zekisin. bunun için sen tespitleri yap exec... | orphan |
| feedback_female_class_clothing.md | feedback | * **Rule:** **Combat-Practical Exposed** (Sleeveless, cropped, asymmetric). | orphan |
| feedback_git_commit_identity_ydbilgin_no_claude.md | feedback | When committing/pushing to the user's **own public GitHub repos** (e.g. `ydbilgin/CodexAuthManager`, and the `cx-agy` share bundle), commit **as th... | orphan, stale_pit |
| feedback_image_skill_naming.md | feedback | İki ayrı görsel-üretim slash skill'i var, **motora göre adlandırıldı** (2026-06-12, kullanıcı isteği). Kullanıcı isimle çağırır, karıştırma: |  |
| feedback_imagegen_asset_pack_clean_cell_split.md | feedback | 🚫 HARD 2026-05-31: When producing asset packs via imagegen, the output MUST be **cleanly splittable into correct cells, background-free**. AI shee... | orphan |
| feedback_imagegen_onbrand_not_realistic_s6.md | feedback | 🚫 HARD 2026-05-31: cx imagegen output for RIMA MUST be on-brand or it is rejected. | orphan |
| feedback_internalize_target_before_judging.md | feedback | **User directive (2026-05-25 morning):** "codex ve antigravity chatgpt_ref teki odalardan yapmak istediğimizi anlayıp ona göre değerlendirsin hata ... | orphan, stale_pit |
| feedback_iso_grid_neighbor_vectors.md | feedback | 🚫 HARD 2026-05-26 evening — iso/diamond grid'de orthogonal coord kullanma. | orphan, stale_pit |
| feedback_kinematic_enemy_shoves_dynamic_player.md | feedback | **Symptom:** Player drifts off the island into the void with a constant velocity (e.g. -3 Y) — no input, gravityScale=0, isDashing=false. Camera fo... | orphan, stale_pit |
| feedback_language_turkish_user_english_agents.md | feedback | 🚫 HARD (2026-05-30): Kullanıcıya **Türkçe** cevap ver. Agent'lar arası iletişim **İngilizce**. | orphan, stale_pit |
| feedback_layered_terrain_mandatory.md | feedback | **Kural:** Tüm PlayableRoom **8-layer painted recipe** ile inşa edilir (Hades + Alabaster Dawn + Octopath canonical). Tek "props scatter" YASAK — h... | orphan, revoked, stale_pit |
| feedback_legacy_script_kinematic_override.md | feedback | S111 kapanışta player map dışı persist sorunu için 3 fix (VoidBlocker padding, OuterContainerWall, Rigidbody2D Dynamic Inspector) denendi, hiçbiri ... | orphan, stale_pit |
| feedback_list_and_sequence_tasks.md | feedback | When the user hands a request that contains several sub-tasks (or a multi-part instruction), **do NOT try to do everything at once.** First **list ... |  |
| feedback_long_dispatch_via_agent_2026_05_24.md | feedback | **Hata 2026-05-24:** bnc0am4rw WallChainBuilder dispatch ~4-6 saat effort spec'i ile direkt cx_dispatch.py'ye verildi. Default 1200s (20 dk) subpro... | orphan, stale_pit |
| feedback_mcp_tool_schema.md | feedback | * **Rule:** Call `ToolSearch` for every MCP tool before writing task files. | orphan |
| feedback_mob_creation_v3_web_ui_only_hard_lock.md | feedback | **Rule:** Mob/canavar sprite üretimi için sadece **PixelLab V3 web UI** (create_character + create_state + Custom Animation V3 + mirror + cleanup).... | orphan, stale_pit |
| feedback_modal_dialog_auto_dismiss_ui_automation.md | feedback | A Unity modal dialog (e.g. "The open scene(s) have been modified externally — Reload / Ignore", or any EditorUtility.DisplayDialog) BLOCKS Unity's ... | orphan |
| feedback_model_routing_s112.md | feedback | S112 user direktif (verbatim): "test scriptlerini yaz bi seviyeye getirdikten sonra benden playtest iste. bunun için kod yazdırmak için sonnet kull... | orphan, stale_pit |
| feedback_model_routing_sonnet_only_s6.md | feedback | 🚫 HARD 2026-06-01: Kullanıcının **Sonnet-only limiti** var. Alt-agent dispatch'lerinde (Agent tool: Explore / general-purpose / Plan / workflow ag... | orphan |
| feedback_model_selection.md | feedback | * **Rule:** Claude chooses the model; user executes. No "Should we use Opus?" questions. | orphan |
| feedback_naming_agy_via_ax_codex_via_cx.md | feedback | When the user (Turkish) says **"agy"** they mean: run the **Antigravity/Gemini** agent through the **`ax`** dispatch tooling (`agy_detached.ps1` / ... | orphan, stale_pit |
| feedback_natural_paint_integration.md | feedback | Agent'lar (Codex, Opus, Claude orchestrator) Brush V1 + Sprint 10/11/12 entegrasyonunda **sadece spec'i mekanik uygulamayacak**. RIMA'ya özgü "daha... | orphan, stale_pit |
| feedback_negation_to_positive_prompts.md | feedback | * **Rule:** Negations (No Tolkien, etc.) are insufficient. Use 1:3 ratio (1 Negation : 3 Positive). | orphan |
| feedback_never_animate_without_approval.md | feedback | 🚫 HARD RULE (user-stated 2026-05-30, S6): **NEVER run an animation-generation step (PixelLab `animate_object`/`animate_character`, or any tool tha... | orphan, stale_pit |
| feedback_never_run_local_model_without_asking.md | feedback | Kullanıcının makinesinde Ollama local modelleri var (örn. `qwen3:14b` ~9.3GB, `gemma4:12b` ~7.6GB). Bunlar duruma göre (ör. video frame-frame görse... |  |
| feedback_nlm_first_context_strict.md | feedback | **Kural:** Hem orchestrator HEM sub-agent'lar context query için **önce NLM**. Direct file read sadece allowed exceptions. | orphan, revoked, stale_pit |
| feedback_no_dialog.md | feedback | * **Rule:** `EditorUtility.DisplayDialog(...)` is **BANNED**. | orphan |
| feedback_no_pixellab_night_autonomous.md | feedback | **Rule:** When user goes offline for an autonomous run (e.g. overnight), orchestrator MUST NOT call any PixelLab generation tool. Permitted: `mcp__... | orphan, stale_pit |
| feedback_object_state_animation_workflow_split.md | feedback | **Karar:** RIMA obje üretim pipeline'ı iki parçaya ayrılır: | orphan, stale_pit |
| feedback_opus_decides_codex_agy_review_s6.md | feedback | 2026-05-29 (S114 S6) user directive: **Opus makes the decisions AND writes the code** (Sonnet for mechanical bulk). **Codex (cx) + agy are used for... | orphan, stale_pit |
| feedback_opus_dispatch_via_rima_design_agent.md | feedback | **Kural:** Kullanıcı "Opus'a sor / Opus'a görev ver" dediğinde, **rima-design agent** (Opus model, `subagent_type: rima-design`) ile ayrı bir dispa... | orphan, stale_pit |
| feedback_orchestration_workflow_2026-06-11.md | feedback | Kullanıcı 2026-06-11: Claude limitini korumak için orkestrasyon kuralları (Opus→Sonnet geçişiyle birlikte). |  |
| feedback_orchestrator_delegate_dont_do_yourself.md | feedback | **Rule:** Claude as RIMA orchestrator dispatches work to sub-agents. Direct execution by orchestrator is the exception, not the default. | orphan, stale_pit |
| feedback_orchestrator_delegate_route_by_difficulty.md | feedback | As Opus orchestrator I should **delegate work to subagents, not execute it myself** — and this includes non-research/build tasks (Unity map-buildin... | orphan |
| feedback_orchestrator_delegation_strict.md | feedback | **Kural:** Orchestrator (Opus/Sonnet) **bulk iş YAPMAZ**. Sub-agent dispatch zorunlu. User orchestrator ile sohbet eder, sub-agent'lar paralel iş y... | orphan, stale_pit |
| feedback_pixellab_character_via_web_ui_v3.md | feedback | **Rule:** Karakter sprite üretimi için MCP `create_character` (standard veya pro mode) **dispatch etme**. User karakter işlerini PixelLab web UI'da... | orphan, stale_pit |
| feedback_pixellab_create_character_workflow.md | feedback | * **S43 Pivot (LOCKED):** | orphan |
| feedback_pixellab_create_image_pro_format.md | feedback | **Rule:** Create Image Pro V3'te negative prompt için **ayrı field YOK**. Tek prompt kutusu var. | orphan, stale_pit |
| feedback_pixellab_decals_always_transparent.md | feedback | Kullanıcı kuralı (2026-06-11): PixelLab decal/prop üretiminde **arka plan KESİNLİKLE şeffaf olmalı**, ve yerleştirme **rastgele/saçma serpiştirme D... |  |
| feedback_pixellab_dispatch_halt_codex_imagegen_first.md | feedback | NO autonomous PixelLab MCP gen for new content. All visual asset generation goes via Codex `$imagegen` (built-in, free) first, producing reference ... | orphan, stale_pit |
| feedback_pixellab_download_via_api_endpoint.md | feedback | **Rule:** When downloading PixelLab-generated objects/sprites to disk, the `backblaze.pixellab.ai` rotation/frame URLs are **NOT reachable from thi... |  |
| feedback_pixellab_english.md | feedback | * **Rule:** `action_description`, `animation_name`, and `KIRO_*.md` instructions must be in English. | orphan |
| feedback_pixellab_iso_tile_ppu_cellsize_rule.md | feedback | 2026-05-25 S106 — Stream O imported 16 b340684f 35° iso tiles at PPU=32 with iso Grid cellSize=(1, 0.5, 1). Visual result: tiles rendered 2 units w... | orphan, stale_pit |
| feedback_pixellab_mcp_autodelete_download.md | feedback | PixelLab MCP `create_map_object` ile üretilen objeler **8 saat sonra otomatik silinir** (get_map_object çıktısında "auto-deletes after 8 hours" uya... |  |
| feedback_pixellab_mcp_halt_strict.md | feedback | **Kural:** PixelLab MCP'nin `create_*`, `animate_*`, `regenerate_*` tool'ları **autonomous çağırılmaz**. Her sprite üretim talebi için: | orphan, stale_pit |
| feedback_pixellab_mcp.md | feedback | * **Prohibition:** `mcp__pixellab__create_character` is **BANNED** for autonomous use. | orphan |
| feedback_pixellab_no_bg_in_prompt_2026_05_24.md | feedback | **HARD RULE 2026-05-24:** PixelLab Create Image Pro'da "Remove background" toggle AÇIKSA, prompt'ta arka plan rengi/dolgu belirtme. | orphan, stale_pit |
| feedback_pixellab_no_dark_fantasy.md | feedback | * **Rule:** Do not use genre labels (dark fantasy, high fantasy, grimdark). | orphan |
| feedback_pixellab_right_left_bias.md | feedback | * **Constraint:** Avoid "right hand" / "left hand" in prompts for south-facing sprites. | orphan |
| feedback_pixellab_style_ref_instruction.md | feedback | * **Standard Text:** | orphan |
| feedback_pixellab_sxl_low_detail.md | feedback | * **Rule:** Detail = **Low detailed** (S-XL Dropdown). | orphan |
| feedback_queue_decide_order_dont_ask_each.md | feedback | 🚫 HARD (user, 2026-05-30 S6): When the user rapidly stacks multiple requests, **do not try to do them all at once** — it scatters focus and causes... | orphan, stale_pit |
| feedback_reference_first_asset_production.md | feedback | **2026-05-21 S97 SOFT GUIDELINE:** PixelLab batch'i öncesi style-uyumlu reference pack(ler) toplama. Anatomik liste / boyut / perspektif validate e... | orphan, stale_pit |
| feedback_research_delegate_to_agents.md | feedback | Orchestrator araştırma görevlerini kendi yapmaz. Daima sub-agent'a delegasyon. | orphan, stale_pit |
| feedback_research_on_block.md | feedback | When working autonomously and you hit a question that requires opinion or non-obvious knowledge ("how do other people do this?"), do NOT guess and ... | orphan, stale_pit |
| feedback_review_roster_autonomous.md | feedback | Otonom (gece) run'larda **writer ≠ reviewer** zorunlu. Writer çoğunlukla cx (dispatch). Review için council kullan: |  |
| feedback_round_brush_same_elevation_lock.md | feedback | **Kural:** Map Designer floor composition workflow = Photoshop/MSPaint **spray brush** mantığı. | orphan, stale_pit |
| feedback_screenspaceoverlay_not_in_screenshot.md | feedback | 2026-06-09, chamber prompt iterasyonunda: chamber-local bottom-center [G] prompt (ScreenSpaceOverlay canvas, anchor (0.5,0.13), TextMeshProUGUI) `S... |  |
| feedback_sonnet_default_opus_deep_routing.md | feedback | Kullanıcı 2026-06-09'da ChatGPT'yle netleştirdiği working-model stratejisini RIMA'ya kilitledi: **Sonnet = günlük production'ın VARSAYILAN çalışan ... | orphan |
| feedback_sonnet_default_opus_exception.md | feedback | 🚫 HARD RULE 2026-05-27 gece S113 (LATE): **Sonnet default agent**. Plan/analiz/design/impl/review/triple-AI orchestration **hep Sonnet first**. Op... | orphan, stale_pit |
| feedback_sonnet_first_routing.md | feedback | **Date:** 2026-05-16 S86 (Opus limit ~2-3 days, after that Sonnet weekly reset abundant). | orphan, stale_pit |
| feedback_sonnet_mechanical_codex_review_only.md | feedback | **Kural:** Tüm mekanik işler (Unity scene/prefab edit, kod refactor, YAML manipulation, file ops, multi-step automation) **Sonnet agent** (general-... | orphan, stale_pit |
| feedback_state_gen_mcp_user_approval_exception.md | feedback | PixelLab MCP autonomous gen halt rule'a tek istisna: | orphan, stale_pit |
| feedback_state_task_purpose_explicitly.md | feedback | **Rule:** When orchestrator presents ANY of the following to user, attach a one-line "Why / Amaç" statement: | orphan, stale_pit |
| feedback_state_vs_n_frames_cost_lock.md | feedback | **Rule:** Aynı style chain hedeflemek için **`create_object` `n_frames=N` numbered list ALWAYS WIN** over `create_object_state` chain. State pipeli... | orphan, stale_pit |
| feedback_test_workflow.md | feedback | * **Rule:** Run tests at session end or post-major refactor. Not for every file edit. | orphan, stale_pit |
| feedback_token_management_weekly_2026_05.md | feedback | **Karar:** Weekly limit ile model + dispatch allocation kuralı. | orphan, stale_pit |
| feedback_tool_visibility_4_surfaces.md | feedback | Editor tool kullanıma açılırken sadece menüye bir entry eklemek YETMEZ. 4 yerden visible olmalı: | orphan, stale_pit |
| feedback_triple_ai_cross_validation_pattern.md | feedback | **HARD RULE 2026-05-25:** Sistem mimarisinde veya algoritma refactor öncesi büyük P0 fix dispatch'lerinden ÖNCE üçlü AI cross-validation yap. | orphan, stale_pit |
| feedback_triple_ai_inside_subagent_synthesis.md | feedback | 🚫 HARD RULE 2026-05-27 gece: Kullanıcı bir şey istediğinde (plan, karar, çoklu seçenek, refactor önerisi vb.), triple AI flow'u subagent içinde te... | orphan, stale_pit |
| feedback_turkish_chars_doc_agents.md | feedback | 2026-06-07: Rapor güncelleme turunda (11 edit, Sonnet rima-doc) yeni bölümler ASCII'ye düşmüş Türkçe ile yazıldı ("Oyun Hissi Katmani", "Cift Yonlu... |  |
| feedback_ui_verify_authored_disabled.md | feedback | When verifying a RIMA runtime-built UI screen (procedural, MainMenuController/CharacterSelectScreen pattern), structural object-counting ("does the... | orphan |
| feedback_unity_safety_protocol.md | feedback | **2026-05-22 LOCK** — user direktifi: "Unity crash vermesin temkinli ilerlensin" | orphan, stale_pit |
| feedback_user_cannot_draw_full_autonomy_required.md | feedback | **Kural:** RIMA asset pipeline'ında **hiçbir adım** user'dan manuel pixel art çizimi gerektirmemeli. Aseprite/Pixelorama/herhangi bir piksel art ed... | orphan, stale_pit |
| feedback_user_draws_weapons_claude_mounts.md | feedback | 2026-06-09: Kullanıcı silah/asset/animasyon seansında iş bölümünü netleştirdi: **"prompt vereceksin ben üretecem onu zaten sen mcp ile indireceksin... |  |
| feedback_user_manual_paint_not_auto_distribute.md | feedback | For floor/wall/tile painting: **build PAINTER TOOL for user manual paint, NOT auto-distribute via execute_code.** | orphan, stale_pit |
| feedback_void_blocker_collider_edge_stop_s6.md | feedback | User report (live, [Image #4]): "I see floor ahead but can't walk onto it" on the floating island arenas. | orphan, stale_pit |
| feedback_wall_decoration_pure_attachment_only.md | feedback | **Kural:** Wall-mounted decoration sprite'ları (banner, candle, torch, skeleton-chained, chain hanging, cage, wall rift glow, lantern, trophy, vine... | orphan, stale_pit |
| feedback_warn_then_apply_if_insistent.md | feedback | 🚫 HARD 2026-05-31: When the user's request conflicts with locked canon / a prior decision, WARN once (state the conflict + the canon-safe alternat... | orphan |
| project_3d_portability_strategy.md | project | User sordu: "İleride bu oyunu veya başka oyunları 3D ortamda 2D boyamayla yapabilir miyiz? Map designer / sistemimiz uygun mu?" | orphan, stale_pit |
| project_3kit_bg_architecture_lock.md | project | RIMA wall-less arenas use 3 modular Kits + Unity Tilemap. NOT single big image. NOT panorama. PARALLAX-MODULAR. | orphan, revoked, stale_pit |
| project_act1_environment_asset_canon.md | project | Act 1 "Shattered Keep" CEVRE/OBJE asset uretim canon'u (NLM notebook 30ddffa5 query, 2026-06-03). Obstacle/door/decor pixel-art uretirken bu lore-g... | orphan |
| project_act1_shattered_keep_lore_lock.md | project | **2026-05-21 S97 HARD LOCK:** Act 1 Shattered Keep biphasic lore — eski zaman SHELTER (insan/antik inşa), bugün CONVERGENCE POINT (rift wards çöktü... | orphan, stale_pit |
| project_acts_canonical_1to4.md | project | **Source:** NLM canonical query 30ddffa5-292f-4248-8e77-68074af901be. | orphan, stale_pit |
| project_agy_dispatch_built_2026_05_25.md | project | Sibling of `cx_dispatch.py` for Codex. Solves the Windows `agy --print` stdio capture bug ([[feedback-agy-print-term-env-fix]]) by allocating a pse... | orphan, stale_pit |
| project_animation_prompt_catalog_warblade.md | project | ⭐ LIVE 2026-05-27 gece S113: Warblade animation production rehberi tamamlandı. | orphan, stale_pit |
| project_asset_pack_organization_lock.md | project | **Karar:** Tüm sprite/tile asset'leri `Assets/Art/AssetPacks/` altına Act-bazlı organize edildi. `_Universal` cross-Act reuse için. Concept art `As... | orphan, stale_pit |
| project_ax_account_pool_deferred.md | project | ax (Antigravity/Google) tarafında **laurethayday + laurethgame + ydbilginn** üçü de birebir aynı Opus 4.6 limitini (ör. %20) ve **aynı reset zamanı... | orphan |
| project_ax_flash_fix_vbs_shim.md | project | 2026-06-05: `ax dispatch` window/focus flash eliminated (AntigravityAuthManager commit `56399bb`, pushed). Root cause (3.1 Pro diagnosis): pythonw.... | orphan |
| project_ax_opus46_available.md | project | ax (agy CLI, Antigravity) model listesinde **Claude Opus 4.6** mevcut (user-confirmed 2026-06-05). Kullanici: "ax Opus 4.6 ile mekanik isleri yapti... |  |
| project_brush_tool_v1.md | project | - ✅ Sprint 1 PASS (commit d0cd49c, tag brush-sprint-1-pass) — Data layer 9 files, 8/8 tests, dotnet build all PASS | orphan, revoked, stale_pit |
| project_brush_v1_manual_composition_system.md | project | **Status:** LIVE 2026-05-18 (`Assets/Scenes/Demo/RoomPipelineTest.unity` PlayableRoom prefab) — open-world 36×22 floor + Warblade character + WASD-... | orphan, revoked, stale_pit |
| project_camera_pixelperfect_640x360_lock.md | project | Main Camera `UnityEngine.U2D.PixelPerfectCamera` (PlayableArena_Test01.unity) ayarı: | orphan, stale_pit |
| project_canonical_character_roster_lock.md | project | **LOCK tarihi:** 2026-05-18 S87 | orphan, revoked, stale_pit |
| project_canonical_character_roster_v2.md | project | User produced + cleaned the 10-class roster via PixelLab Web UI V3. Identity state edits applied (cursemark, recolor, hair removal, young variant).... | orphan, stale_pit |
| project_character_64px_canvas_large_for_animation.md | project | 2026-06-02 (user-stated): RIMA character ACTUAL visual size = **64 px**. The PixelLab idle-sprite CANVAS is intentionally LARGER (measured: Warblad... | orphan |
| project_character_system.md | project | > WARNING: DEPRECATED 2026-05-17. See [[weaponless-animation-v1]] (Karar #144) for current spec. Aseprite composite ref is obsolete. | orphan, revoked, stale_pit |
| project_character_visual_identity.md | project | > REVISED 2026-05-17 per NLM canonical query. Source: NLM notebook 30ddffa5-292f-4248-8e77-68074af901be. | orphan, stale_pit |
| project_charselect_v3_council_2026-06-05.md | project | 2026-06-05 session: Kullanici ChatGPT konsept gorseli + prompt verdi -> council inceledi. | orphan |
| project_chatgpt_ref_object_inventory_2026_05_25.md | project | Source: Antigravity research `STAGING/s106_overnight/ideation/agy_object_inventory_research.md` + rima-sonnet vision `STAGING/s106_overnight/CHATGP... | orphan, stale_pit |
| project_class_balance.md | project | * **Key Changes (2026-04-14):** | orphan |
| project_class_colors.md | project | > REVISED 2026-05-17 per NLM canonical query (hex codes locked from RIMA design docs). | orphan, stale_pit |
| project_class_genders.md | project | \| Class \| Gender \| | orphan |
| project_cliff_depth_resolution_s114s5.md | project | ⭐ LIVE 2026-05-29 (S114 S5, Opus autonomous + triple-AI). The long-rejected cliff visual (#1 open issue) was largely resolved through iterative use... | orphan, stale_pit |
| project_cliff_directional_inward_tuck_fix.md | project | The recurring cliff sideways-overflow was finally root-caused and fixed 2026-06-03 (user "matematikte hata var" + "iso düşün" + "boşluk olan tile'd... | orphan |
| project_cliff_iso_direction_lock_2026_05_26.md | project | ⭐ LIVE 2026-05-26 late afternoon (S109 CORRECTION). S108 evening entry was MATHEMATICALLY WRONG. Antigravity's S109-morning revise was correct. Ver... | orphan, stale_pit |
| project_cliff_manual_override_pending.md | project | 📋 S109 evening LOCKED USER INTENT, S110 PICKUP'a hazır. | orphan, stale_pit |
| project_cliff_pivot_manual_brush_2026_05_26.md | project | **User talimati (verbatim):** "Ben istedigim takdirde oralara serpistiririm ve istedigim seviyede (layer cliff ve layer parallax arasina secme imka... | orphan, revoked, stale_pit |
| project_cliff_reward_portal_locks_2026-06-02.md | project | Three locked decisions from the 2026-06-02 agent-driven session (cx=laurethayday, ax=Gemini 3.1Pro/3.5Flash, NLM). Full detail in CURRENT_STATUS to... | orphan |
| project_combat_architecture.md | project | * **Input:** | orphan |
| project_combat_feel_research_combined.md | project | **Status:** Research findings, ready for Phase 2 combat feel sprint implementation. | orphan, stale_pit |
| project_connected_wallrun_depth_dragplace_s6.md | project | 2026-05-31 (Opus otonom, workflow `wf_a41382c6` 4-ajan + cx + ax): kullanıcının 3 feedback'i çözüldü — oda **çok düz** + duvarlar **bağlanmıyor** +... | orphan |
| project_core_wall_system_v2_lock_2026_05_24.md | project | **LIVE 2026-05-24:** ChatGPT v2 spec adopted. 4 sheet, 40 piece, sade ilk (variations LATER via PixelLab Edit Image). 512x512 PixelLab Create Image... | orphan, revoked, stale_pit |
| project_cross_class_skills.md | project | * **Pool:** 10 classes x 8 exportable skills (80 total). | orphan |
| project_cx_dispatch_global.md | project | Since 2026-06-05: Codex dispatch = global **`cx dispatch --task-file <f> --effort <e> [--profile <p>] [--timeout 3600]`** (CodexAuthManager repo, c... |  |
| project_data_driven_room_combat_2026-06-04.md | project | 2026-06-04 (UI-redesign + rooms session): the data-driven `_Arena` path is now the PRODUCTION room/combat system; scene-based `MapFlowManager` (the... | orphan |
| project_demo_asset_locks_s114.md | project | LOCK 2026-05-28 S114 (audit workflow w0hs7kt6c sentezi, Opus karar). "Tekrar dusunmeyelim" — Phase-1 demo asset + anim kararlari. | orphan, stale_pit |
| project_demo_blite_isoroom_system.md | project | RIMA oynanabilir demo = **Path B-lite** (council karari, `STAGING/DEMO_ARCHITECTURE_DECISION_2026-06-03.md`): data-driven, tek `_Arena` sahnesi, od... | orphan |
| project_demo_blite_test_suite.md | project | Demo B-lite run loop now has an automated regression suite (2026-06-03 GECE-5, commit 5fc5197e). Designed via /council (cx feasibility + Gemini 3.1... | orphan |
| project_demo_map_pipeline_s6.md | project | 2026-06-01 gece, Opus TAM-OTONOM (kullanıcı "tüm yetki sende, ax+cx'e danış son kararı sen ver, ben gidiyorum"). Hedef: güzel seamless izometrik de... | orphan |
| project_demo_phase_order.md | project | RIMA jüri demosu = baştan sona OYNANABILIR, HATASIZ/STUCK'SIZ dikey-slice. Akış: MainMenu→Chamber(Warblade/Elementalist)→Combat→Combat→Shop→Combat→... | orphan |
| project_demo_phase1_milestone_lock.md | project | RIMA demo iki tier: **Faz 1 Milestone** (ilk oynanabilir loop, 10 dk) vs **Faz 4 Steam Demo** (10 sınıf + Act 1+2, 30-45 dk). | orphan, stale_pit |
| project_demo_tools_autonomous_2026-06-13.md | project | 2026-06-13 baslatildi. Demo = hocaya **CANLI SUNUM** (Steam/oyuncu DEGIL), capstone, not=SISTEMLER uzerinden. Anlati iki ayak: "editorsuz dengeliyo... |  |
| project_demo_tools_plan_2026-06-12.md | project | RIMA demo "dengeleme altyapısı" araçları — kararlar KİLİT, implementasyon yeni session'da cx ile (2026-06-12). |  |
| project_designer_regression_iso_fix_s6.md | project | 2026-06-01 (user-present, Opus orchestrator; cx=laurethayday writer -> Opus review; ax=ydbilgin design). User returned after the crashed autonomous... | orphan |
| project_diamond_iso_tilemap_lock_2026_05_24.md | project | **LIVE 2026-05-24:** Unity Tilemap mode `IsometricZAsY` (cell size 1x0.5 world units, 128x64 px at PPU=64). ChatGPT analiz + industry standard (Had... | orphan, revoked, stale_pit |
| project_environmental_props_v2.md | project | **Status:** DEFER — V1 vertical slice + Sprint 9-13 hardening tamamlanmadan başlama. | orphan, stale_pit |
| project_fakeiso_term_revoked_2026_05_22.md | project | On 2026-05-22 (post-S98), user clarified: "bizim fake-iso dememiz aslinda top-down'a donduk degil mi? objeler top-down karakter oyle gibi". This co... | orphan, revoked, stale_pit |
| project_feel_toggles.md | project | Karar #50 (2026-04-24). Bütün feel/juice feature'ları default ON, Settings → Accessibility menüsünden ayrı ayrı kapatılabilir. | orphan, stale_pit |
| project_ghost_attack_system.md | project | * Trigger: Cross-class skill pool + Secondary skills (Z/X). | orphan |
| project_high_top_down_3_4_lock_2026_05_24.md | project | **LIVE Karar (2026-05-24):** RIMA projection **HIGH TOP-DOWN 3/4** (camera ~70-80 degree from horizon + 3/4 sprite styling). Reaffirms Karar #114 +... | orphan, revoked, stale_pit |
| project_hold_to_build_drag_place_ideas.md | project | 2026-05-31: User saw a factory game (Conor Dart, X video — also praising Opus 4.8 for gamedev) where transport belts are placed by **holding the mo... | orphan |
| project_hud_overlay_decision.md | project | **Problem:** Existing "Run Codex Build Overlay" concept covers ~70% of screen, too heavy for action gameplay. Genre peers (Hades, Vampire Survivors... | orphan, stale_pit |
| project_hybrid_asset_pipeline_lock.md | project | **Karar:** #157 (candidate, user onayı sonrası LOCK) | orphan, stale_pit |
| project_iso_paint_cliff_zoom_mcp_s6.md | project | 2026-06-01 gece-4 (Opus, user present->autonomous, %76 session limit). User in `_IsoGame` Play+F2 reported 4 issues. Opus diagnosed (the valuable p... | orphan |
| project_iso_pivot_diamond_rooms_s6.md | project | 2026-05-31 PM4: Kullanici `STAGING/concepts/chatgpt_ref` (ChatGPT gameplay shot'lari + ADIM 1/5 asset-pack board'lari) ile vizyonu netlestirdi -> *... | orphan |
| project_item_matrix_decisions.md | project | * Ruin Eternal (Ravager): Kill-gate ext cap +5s | orphan |
| project_juice_features_v1.md | project | **Status:** Backlog (not LOCKED). Brainstorm from Bandit Knight + Hades + HLD reference + research takeaways. User to pick final cut at Phase 2 entry. | orphan, stale_pit |
| project_karar_149_subroom_encounter_lock.md | project | **Supersedes:** [[project-subroom-encounter-system-proposal]] (proposal phase, now locked) | orphan, stale_pit |
| project_karar_150_act1_envanter_live.md | project | **2026-05-19 S95 LIVE:** Act 1 envanter v4 referans iskeleti hazır. Karar #150 LIVE + temiz local pack. Eski painterly_* envanter (63 PNG) archive'... | orphan, revoked, stale_pit |
| project_l3_island_large_boss_spawn_trigger.md | project | 2026-05-25 S106 NIGHT user directive: `island large boss odasına gidilecek odaya anca geçilecğei zaman olabilir spawn olarak`. | orphan, stale_pit |
| project_laureth_studio_master_plan.md | project | **Location:** `F:/LaurethStudio/` | orphan, stale_pit |
| project_lighting_roomfill_lock_s6.md | project | User found gas lamps absurd on a floating slab + rooms too empty (live playtest 2026-05-30). 3-AI (NLM-input/timeout + agy visual + cx code) → Opus... | orphan, stale_pit |
| project_livetool_t3_scaffold_s114s5.md | project | ⭐ LIVE 2026-05-29 (S114 S5, Opus autonomous, workflow + Codex + agy). User asked to "start full T3" via workflow while away. Entry point: `STAGING/... | orphan, stale_pit |
| project_localization.md | project | * Priority: TR (Turkish), EN (English) - Phase 1 | orphan |
| project_modular_design_philosophy.md | project | Source video: Feed the Overlord, "modular design saved my entire project" — https://youtu.be/9CQgPaHAV1E (yt id 9CQgPaHAV1E). Full transcript analy... |  |
| project_modular_pipeline_lock.md | project | > **⚠️ PARTIAL STALE 2026-05-24** | orphan, revoked, stale_pit |
| project_modular_pixellab_pipeline_s6.md | project | RIMA art-production plan locked 2026-06-01 (user-present, Opus synthesis of agy+cx+forum). | orphan |
| project_multi_projection_architecture_lock.md | project | 3 bağımsız high-quality verdict converge etti — Codex narrow (`STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md`), Opus overnight (conversation log), Codex mu... | orphan, stale_pit |
| project_multilayer_painter_v1_lock.md | project | **Source:** `STAGING/MULTILAYER_PAINTER_PLAN_v1.md` (LOCKED, Codex reviewed). | orphan, revoked, stale_pit |
| project_music_production_pipeline.md | project | **Status:** Strategy defined, implementation Phase 2-3. | orphan, revoked, stale_pit |
| project_overnight_autonomous_designbuild_s6.md | project | ⭐ DIRECTIVE 2026-05-30 (kullanıcı AWAY, ~10h otonom): RIMA demo'yu tasarım+kod olarak ilerlet. **Opus TEK karar verici** (sorma, [[feedback-autonom... | orphan, stale_pit |
| project_overnight_run_2026-06-12.md | project | 2026-06-12 gecesi kullanıcı uykuda, otonom run başlatıldı. Plan = `STAGING/AUTONOMOUS_RUN_2026-06-12.md`. |  |
| project_painter_consolidation_lock.md | project | > **⚠️ SUPERSEDED 2026-05-24** | orphan, revoked, stale_pit |
| project_perspective_templates.md | project | * Shadow Locking: 1px Contact Shadow template -> Concept Image | orphan |
| project_pillar_seam_cover_lock_2026_05_24.md | project | **LIVE Karar (2026-05-24, S102 close):** Pillar-as-seam-cover stratejisi modüler duvar production için **LIVE**. S101 "PILLAR-LESS walls + NO baked... | orphan, revoked, stale_pit |
| project_pixellab_character_states_workflow.md | project | **Karar:** #145 LOCKED 2026-05-16 S86 | orphan, revoked, stale_pit |
| project_playable_arena_iso_proof_2026_05_25.md | project | `Assets/Scenes/Test/PlayableArena.unity` | orphan, stale_pit |
| project_playable_iso_map_s6.md | project | Session 2026-06-01 gece-3 (Opus orchestrator, user-present, cx laurethayday + ax/Gemini + Sonnet-agent + Opus-judge + a 6-agent gap WORKFLOW). _Iso... | orphan |
| project_playable_roadmap_2026-06-05.md | project | 2026-06-05 ogleden sonra (3 council: code-anim + TBH/walk-select + playable-roadmap; hepsi commit'li): | orphan |
| project_player_collider_hitbox_spec_s6.md | project | User asked "what exactly should the player hitbox be?" — definitive answer for RIMA (top-down 3/4): | orphan, stale_pit |
| project_portal_preview_orb_system_s6.md | project | 2026-05-31 PM·6 — Önceki session API hatasıyla çöktü (`thinking blocks cannot be modified`, context bozulması). Kullanıcı kararlar kaybolmasın diye... | orphan |
| project_procgen_stack_lens.md | project | **Canonical location:** `F:/LaurethStudio/MEMORY/procgen_stack_lens.md` | orphan, stale_pit |
| project_progression_canonical_lock.md | project | **Source:** NLM canonical query 30ddffa5-292f-4248-8e77-68074af901be (Karar #61/62/63 verified). | orphan, stale_pit |
| project_props_placement_env_bg_plan_2026-06-11.md | project | **2026-06-11 PLAN-ONLY session.** User handed a ChatGPT-built plan zip (`Downloads/RIMA_Props_Doors_AI_Placement_PLAN_ONLY (1).zip`, 8 docs) and sa... | orphan |
| project_reference_games_weapon_combat_takeaways.md | project | Codex web pass + Antigravity second-eye iki referans oyunu araştırdı. Raporlar: `STAGING/BLADES_OF_MIRAGE_PIPELINE_REPORT.md`, `STAGING/COLOSSUS_ET... | orphan, stale_pit |
| project_resume_2026-06-11.md | project | **Tamamlananlar:** |  |
| project_review_fix_2026-06-12.md | project | ChatGPT comparative review (overnight run kodu) geldi, Claude gerçek kodla **5 bulgu CONFIRMED**, council (cx+ax 3.1 Pro+ax Flash) risk-denetledi. ... |  |
| project_reward_portal_phase1_done_2026_05_26.md | project | ⭐ LIVE 2026-05-26 evening — Reward+Portal Phase 1 implementation complete. | orphan, stale_pit |
| project_rift_break.md | project | * Normal Rooms: Auto-ultimate | orphan |
| project_rift_crack_architecture.md | project | 1. Crack LINE (PixelLab): Baked in sprite. | orphan |
| project_rift_threshold_door_design_s95.md | project | **Rift Threshold:** Sub-room transition için **vertical rift seam through wall** — gravity-aligned reality tear. Framed door DEĞİL, **wound in the ... | orphan, stale_pit |
| project_rima_backlog.md | project | * Combat VFX/Hit Stop: Screen shake, pixel-perfect (Phase 1) | orphan |
| project_rima_hades_style_cb_wang_split_lock.md | project | **Karar:** RIMA görsel paradigm = **Hades-style tile + sprite overlay**. CircuitBreaker (LaurethStudio 2. oyun) = **pure top-down with Wang tiles**... | orphan, stale_pit |
| project_rima_style_manifesto.md | project | **User direktif (S95+ LATE):** "Hades alabaster dawn gibi oyunlardan feyz alarak RIMA stilini oluşturuyoruz işte asıl amacımız bu." | orphan, stale_pit |
| project_rima.md | project | - Root: `F:\Antigravity Projeler\2d roguelite\RIMA\` | orphan |
| project_roadmap_dungeon_buildup_lock.md | project | **Roadmap: Dungeon Buildup 6-Faz Piece-by-Piece LOCK (2026-05-19 S94 LATE)** | orphan, stale_pit |
| project_room_library_architecture.md | project | **Status:** Sprint 10 implementation LIVE (2026-05-16 S86 SPRINT10_IMPL — Opus impl, Codex review pending). Karar otoritesi: sprite_strategy_FINAL_... | orphan, stale_pit |
| project_room_system_model_b_2026-06-03.md | project | Decided 2026-06-03 (Opus synth of cx + Gemini 3.1 Pro + Gemini 3.5 Flash + user). RIMA moves FROM scene-per-map (MapFlowManager + baked _IsoGame* s... | orphan |
| project_roomtool_townscaper_s6.md | project | 2026-05-31 PM·3 (Opus otonom + cx + 2 workflow): kullanıcı oda-look iterasyonundan **ARACA pivot etti.** Birkaç tur look (connected walls→enclosure... | orphan |
| project_ruined_keep_room_build_feedback.md | project | 2026-05-31: Built the RIMA top-down 3/4 "Ruined Keep" room (PlayableArena_Test01) and pivoted toward gameplay. All SAVED, build green. | orphan |
| project_runmap_ui_asset_pipeline_2026-06-11.md | project | RIMA RunMap + HUD/UI görselleri için council-kilit üretim stratejisi (2026-06-11): |  |
| project_s100_batch_summary.md | project | S100 (2026-05-22) oturumunda 8 kod görevi dispatch + QC + fix döngüsüyle tamamlandı. | orphan, stale_pit |
| project_s102_session_close_2026_05_23.md | project | - **ComfyUI server** (port 8188) — Flux Kontext model loaded in VRAM (~12GB), idle. **Killable** for clean state. | orphan, stale_pit |
| project_s103_session_close_2026_05_24.md | project | \| # \| Karar \| Memory \| | orphan, revoked, stale_pit |
| project_s105_agy_switcher_built_2026_05_25.md | project | S105 (2026-05-25 gece) — V2 P0 fix wave + Antigravity CLI switcher infrastructure. | orphan, stale_pit |
| project_s106_overnight_session_2026_05_25.md | project | User went to bed 02:55. Orchestrator (Opus) running fully autonomous — no user questions allowed back. Self-approve all plans. Update CURRENT_STATU... | orphan, stale_pit |
| project_s110_evening_snapshot_2026_05_26.md | project | **Codex Day 5a** task id `bj3tz4lpc` | orphan, revoked, stale_pit |
| project_s110_late_collider_visible_menu_clean_2026_05_27.md | project | S110 LATE durum snapshot (2026-05-27). Önceki Claude hesap rate-limit yedi, bu memory pickup için yazıldı. | orphan, stale_pit |
| project_s6_autonomous_build_s114.md | project | S114 S6 (2026-05-29) long autonomous build. Model: Opus writes+decides, cx + agy are reviewers (see [[feedback_opus_decides_codex_agy_review_s6]]).... | orphan, stale_pit |
| project_s6_autonomous_bulk_build_wave1to4.md | project | ⭐ 2026-05-31 OVERNIGHT (Opus full-autonomous, user AWAY, NO-questions): big BULK vertical build. All dotnet+Unity-compile-clean, cx-reviewed (write... | orphan |
| project_s6_autonomous_production_workorder.md | project | ⭐ **S6 CLOSE (2026-05-30, Opus full-autonomous, user PRESENT then /clear):** The game's DIRECTION is now fully locked from NLM canon, a clean archi... | orphan, stale_pit |
| project_sang_hendrix_live_editor_canonical_ref.md | project | LOCK 2026-05-28 (S114 S3, triple-AI: Opus+Codex+agy converge): **Sang Hendrix** = RIMA T3 live tool canonical referansi. Steam oyunu "Little Master... | orphan, stale_pit |
| project_sang_parallax_study_done_2026_05_26.md | project | 📋 S109 (2026-05-26) — Sang Hendrix Realtime Parallax Map Builder (RPG Maker MZ plugin) inceleme tamamlandı. | orphan, stale_pit |
| project_senior_design_report_s6.md | project | Kullanıcı bitirme/senior-design **ARA RAPOR'un detaylı versiyonunu** yazdırmak istiyor (~30-40 sayfa, Türkçe, "öğrenci gibi" dil, map screenshot'lı... | orphan |
| project_session_cleanup_iso_tooling_s6.md | project | 2026-06-01 evening session (Opus, user-present). All dotnet-build clean, UNCOMMITTED + gated; only gate left = Unity visual/F5 verify. | orphan |
| project_sfx_pipeline.md | project | * Model: stable-audio-model (15.7GB) | orphan |
| project_sfx_v2.md | project | * Path: generate_rima_sfx_v2.py | orphan |
| project_shadow_standard.md | project | * Type: Separate Unity sprite layer (Oval contact shadow) | orphan |
| project_sim_philosophy.md | project | * Every battle (Room/Elite/Boss) must be simulated. | orphan |
| project_skill_mechanic_final_report_s6.md | project | ⭐ FINAL LOCK 2026-05-31 (Opus synthesis of 4-AI: Sonnet+cx+agy+Opus-subagent, 2 rounds, + NLM canon + code verify). Full report = `STAGING/RIMA_SKI... | orphan |
| project_sodaman_skill_hover_ux_plan.md | project | 2026-06-04 GECE·3 /council (cx yekta + ax-3.1-Pro + ax-3.5-Flash -> Opus) on Sodaman (Steam bullet-heaven roguelite app/2178990). ANALYSIS/DECISION... | orphan |
| project_staging_process_convention.md | project | 2026-06-07: STAGING'de 884 üst-seviye dosya birikmişti (337 süreç artifact'i `_archive/process_2026-06/`'ya arşivlendi, ax-Flash yaptı). Kök neden:... |  |
| project_studio_transfer_2026_05_27.md | project | LaurethStudio S15 close (2026-05-26/27) transfer kaydı. RIMA action roguelite context filter sonrası karar matrisi. | orphan, stale_pit |
| project_subroom_canonical_tags_lock.md | project | **Karar #150 LIVE → Karar #149 sub-room slot grammar** Codex APPROVE_WITH_REVISIONS verdict #4 ile lock. | orphan, stale_pit |
| project_t3_char_refresh_ui_pack_2026_05_31.md | project | ⭐ 2026-05-31 session (Opus orchestrator, sequential-autonomous via UnityMCP, each step cx+agy parallel review). Pickup = `CURRENT_STATUS.md` top bl... | orphan |
| project_topdown_pivot_lock.md | project | > **⚠️ SUPERSEDED 2026-05-24 by [[project-high-top-down-3-4-lock-2026-05-24]]** | orphan, revoked, stale_pit |
| project_ui_redesign_camera_softlock_2026-06-04.md | project | Playtest-driven fixes for _IsoGame (2026-06-04, council + cx laurethayday + Opus). 9 commits, all play-verified. Decision doc = STAGING/UI_UX_REDES... | orphan |
| project_unified_designer_s6.md | project | 2026-06-01 (ultracode, Opus): Kullanıcı "çok sayıda designer'ı TEK geniş-UI/UX'li, iki-yüzeyli (Unity Editor + oyun-içi F2), ortak-RoomData, tablı,... | orphan |
| project_v2_wall_system_built_2026_05_25.md | project | **LIVE 2026-05-25:** Logic-first room builder + placeholder phase tamamlandı. RIMA.Walls.V2 namespace, eski RIMA.Walls (V1) ile parallel co-exist. | orphan, stale_pit |
| project_vfx_production.md | project | * Slash Arc (Warblade): 128x128, Cold Blue crescent, 1 peak frame | orphan |
| project_visual_editor_refactor_2026_05_26.md | project | ⭐ LIVE 2026-05-26 evening — Antigravity tarafından refactor edilen Visual Map Designer EditorWindow. S109 review tamamlandı. | orphan, stale_pit |
| project_wall_chunk_pixellab_pipeline_2026_05_23.md | project | Multiple wall production approaches tried this session and failed: | orphan, stale_pit |
| project_wall_production_pipeline_s99_late.md | project | > **⚠️ SUPERSEDED 2026-05-24 by [[project-pillar-seam-cover-lock-2026-05-24]]** | orphan, stale_pit |
| project_walless_v1_hades_elysium_lock.md | project | RIMA visual direction is **PURE wall-less floating-arena Hades Elysium hybrid**. User confirmed: "tüm oyunumu buna oturtup çeşitliliğini yapacağım". | orphan, stale_pit |
| project_wang16_compositor_pipeline_lock.md | project | **Karar:** RIMA top-down floor blending = **custom Python Wang16 compositor + PixelLab `create_tiles_pro` bases + Karar #143 L4/L5/L6 overlays**. T... | orphan, stale_pit |
| project_weapon_anim_converged_s114.md | project | ⭐ LOCK 2026-05-28 S114 (Opus 4.8 sentez, 2 AI cross-validation): Weapon/animasyon pipeline kararlari. Kullanici "8 dir mi / kac piksel / ele nasil ... | orphan, stale_pit |
| project_weapon_hand_separate_lock_s6.md | project | User decision (2026-05-30, with Challacade Moonshire screenshot refs): **the weapon stays a SEPARATE sprite from the character — NEVER baked into c... | orphan, stale_pit |
| project_weapon_pipeline_lock.md | project | **Source:** NLM canonical query 2026-05-22. Full spec at `STAGING/weapon_pipeline_v1.md`. Web prompts at `STAGING/weapon_web_prompts_v1.md`. | orphan, stale_pit |
| project_weapon_production_plan_s114.md | project | LOCK 2026-05-29 (S114 S4, Opus 4.8). RIMA silah uretim + mount karari. | orphan, stale_pit |
| project_weapon_system_8dir_lock.md | project | **Fact:** Weapon = 1 static sprite (2 for asymmetric dual-wield L+R). 8-yön karakter için yön farkı 4 katmanla çözülür: | orphan, stale_pit |
| project_weaponless_animation_v1.md | project | **Status:** LOCK in this session (Karar #144 proposal). User'a iletildi, onay sonrası MASTER_KARAR_BELGESI'ne eklenecek. | orphan, stale_pit |
| project_yarik_3scale_language.md | project | \| Ölçek \| Görsel \| Kit \| Sorting \| | orphan, stale_pit |
| reference_3d_pipeline.md | reference | * RIMA remains 2D (Variety + Readability critical) | orphan |
| reference_act1_shattered_keep_visual_canon.md | reference | Act 1 "Shattered Keep" görsel kanon — NotebookLM canonical (2026-06-11 sorgu). Drift: NLM > bu memory; ama exact değerler cx FAZ1/FAZ2 için burada. | orphan |
| reference_agy_cli_paths.md | reference | - **Executable:** `C:\Users\ydbil\AppData\Local\agy\bin\agy.exe` | orphan, stale_pit |
| reference_agy_dispatch_cli_flags.md | reference | ``` | orphan, stale_pit |
| reference_agy_dispatch_no_output_flag.md | reference | 📍 S113 LATE 2026-05-28: `agy_dispatch.py` CLI sadece bu flag'leri kabul eder: | orphan, stale_pit |
| reference_antigravity_opus46_escalation.md | reference | **Antigravity IDE** = user's secondary AI workspace with **Opus 4.6** access. | orphan, stale_pit |
| reference_antigravity_vs_codex_distinction.md | reference | İki ayrı sistem, sıkça karışıyor. Status/dispatch yazarken her zaman netleştir. | orphan, stale_pit |
| reference_asset_map.md | reference | * Characters: Assets/Sprites/Characters/{Class}/base/ | orphan |
| reference_ax_agy_cli_mechanism.md | reference | Antigravity Gemini CLI mechanism (verified 2026-06-01, user-present): | orphan |
| reference_ax_dispatch_model_flag.md | reference | `F:\Antigravity Projeler\AntigravityAuthManager\ax_dispatch.py`'ye **`--model "<ad>"`** opsiyonel argümanı eklendi (2026-06-13, additive, sıfır ris... |  |
| reference_codex_skills.md | reference | \| Skill \| RIMA Use Case \| | orphan |
| reference_cx_agy_share_bundle.md | reference | Shareable, genericized copy of the **cx** (Codex dispatch) + **agy** (Antigravity dispatch) + **limits** tooling, built S6 2026-05-29 for sharing w... | orphan, stale_pit |
| reference_directormode_validation_api.md | reference | `Assets/Scripts/UI/DirectorMode.cs` (RIMA.DirectorMode) runtime overlay tool'u, programatik test icin tam bir `*ForValidation` public API tasiyor. ... |  |
| reference_dispatch_skills.md | reference | 2026-06-13 olusturuldu, **GLOBAL** (`C:\Users\ydbil\.claude\skills\` — tum projelerde calisir, RIMA'ya bagli degil). **Dinamik proje koku:** her sk... |  |
| reference_dynamic_workflows_usage.md | reference | 📍 LOCK 2026-05-28 S114: **Dynamic workflows** Claude Code'da mevcut (research preview). Claude anlik JS orchestration script yazar → koordineli su... | orphan, stale_pit |
| reference_gemini_dispatch_tool.md | reference | **Path:** `F:\Antigravity Projeler\2d roguelite\RIMA\gemini_dispatch.py` | orphan, stale_pit |
| reference_gemini_mcp.md | reference | * Access: ENABLED via Antigravity workspace | orphan |
| reference_gemini_models_verified_2026_05_21.md | reference | \| Model \| Durum \| Notlar \| | orphan, stale_pit |
| reference_graphify_usage_strategy.md | reference | **Trigger:** User feedback 2026-05-18 "graphify aşırı fazla harcadı session limitten, bu hep böyle mi olacak?" — full build maliyetli, ama doğru ku... | orphan, stale_pit |
| reference_lauretstudio_library_strategy.md | reference | LaurethStudio'nun cross-game asset library'si = **PixelLab cloud account**. Ayrı filesystem mirror veya "global folder" yok. | orphan, revoked, stale_pit |
| reference_loc_bilingual_exists.md | reference | RIMA localization altyapısı **mevcut ve çalışıyor** — `Assets/Scripts/Core/Loc.cs`. |  |
| reference_mechanic_bank_studio.md | reference | - **Master bank:** `F:\LaurethStudio\03_IDEAS\MECHANIC_BANK\_MEKANIK_BANKASI.md` | orphan, stale_pit |
| reference_mechanic_bank_youtube60.md | reference | LaurethStudio idea bank: game-mechanic primitives from YouTube video `-zjKT4Av52M` (titled "60 cool game mechanics"). | orphan |
| reference_nlm_auth_recovery_manual_cookie.md | reference | ⭐⭐⭐⭐ S6 v4 SOURCE-PATCHED (2026-06-01, Opus applied upstream PR #211 locally) — bug kökten kapandı, `--force` ARTIK GEREKMİYOR (zararsız ama opsiyo... | orphan |
| reference_nlm_conflict_resolution_s114.md | reference | Full doc: `STAGING/NLM_CONFLICT_RESOLUTION_S114.md`. Method: NLM query (canonical) → Opus 4.8 pre-decision → Codex (yasinderyabilgin) + agy (ydbilg... | orphan, revoked, stale_pit |
| reference_pixel_fluid_townscaper_dig_stopsignalart.md | reference | User flagged this as VERY important (2026-06-02). Source: https://x.com/stopsignalart/status/2061483004506096130 (downloaded via yt-dlp → `C:\Users... | orphan |
| reference_pixelart_scaling.md | reference | Council + web synthesis (2026-06-04). Full report: `STAGING/PIXELART_SCALING_REPORT_2026-06-04.md` (committed `ecf98200`). | orphan |
| reference_pixellab_create_image_pro.md | reference | **Üretim akışı:** User PixelLab web UI > "Create Image Pro" sekmesinde manuel çalıştırır. MCP'de YOK (bkz: [[pixellab-tool-inventory]] — create_obj... | orphan, stale_pit |
| reference_pixellab_create_modes.md | reference | 1. Pro / Rotate: Single sprite -> 8-dir. Fast, weak diagonals. Backup only. | orphan, stale_pit |
| reference_pixellab_create_tiles_pro_4type.md | reference | **Kapasite:** Tek `create_tiles_pro` çağrısında **4 farklı tile type** numaralı prompt ile batch üretilir, toplam maliyet **25 generation**. | orphan, stale_pit |
| reference_pixellab_direction_sequence.md | reference | \| Index \| Direction \| Code \| | orphan |
| reference_pixellab_knowledge_base_s114.md | reference | 📍 LOCK 2026-05-28 S114: PixelLab tum bilgisi tek dosyada → **STAGING/PIXELLAB_KNOWLEDGE_BASE.md**. Workflow `pixellab-knowledge-base` (local-docs ... | orphan, stale_pit |
| reference_pixellab_knowledge_map.md | reference | * Content: Official docs, Discord community gems, RIMA specific wins. | orphan |
| reference_pixellab_production_knowledge.md | reference | **Amaç:** Hangi tool ne sonuç verir, parametreler nasıl davranır, bilinen sınırlar nedir. Her session aynı hataları yapmayalım diye. | orphan, stale_pit |
| reference_pixellab_real_sizes_and_tools.md | reference | **Area constraint ~262,144 px² (constant across aspect ratios via slider):** | orphan, revoked, stale_pit |
| reference_pixellab_tested_prompts.md | reference | **Amaç:** Test edilmiş prompt'ları sakla — PASS verenler reusable, FAIL verenler benzer yanlışı engellesin. | orphan, stale_pit |
| reference_pixellab_tool_inventory.md | reference | Transport: HTTP, `https://api.pixellab.ai/mcp`, Bearer auth. Connected via `claude mcp add`. **Restart required** after first add. | orphan, stale_pit |
| reference_pixellab_ui_icon_anim.md | reference | PixelLab yetenekleri — 2026-06-09 (MCP agent_help + resmi tutorial https://youtu.be/LcJQQwltQ2Q). | orphan |
| reference_pixellab_v3_budget_formula.md | reference | **Yanılgı:** V3 web UI MCP'den farklı pricing pool'u sanılıyordu (Production Plan v1 ilk turda "Faz 2 = 0 gen" yazdım, **YANLIŞ**). | orphan, stale_pit |
| reference_pixellab_v3_gen_cost_by_frame.md | reference | 📍 LIVE 2026-05-28: PixelLab Custom Animation V3 gen cost per-direction kullanıcı verbatim lock. | orphan, stale_pit |
| reference_pixellab_v3_ui.md | reference | **MCP DURUMU (2026-05-16 confirmed):** V3 Custom Animation ve Custom Frames özellikleri **MCP'de EXPOSE EDİLMEMİŞ**. `animate_character` sadece tem... | orphan, stale_pit |
| reference_room_mechanics.md | reference | * Act 1: 8 - 9 rooms | orphan |
| reference_sonnet_skill_capability.md | reference | Sonnet dispatch (general-purpose agent) skill'leri **slash command olarak** çağıramaz (skill mechanism orchestrator-only). Ama skill'in altındaki *... | orphan, stale_pit |
| reference_tweet_fetching_workflow.md | reference | **Problem:** `WebFetch https://x.com/...` returns **HTTP 402 Payment Required** for unauth requests. Nitter public mirrors all dead (`nitter.net` e... | orphan, stale_pit |
| user_system_specs.md | user | * CPU: Ryzen 7 9800X3D (8C/8T, 4.7GHz) | orphan |

## D. Memory (repo MEMORY/)

Konum: `F:\Antigravity Projeler\2d roguelite\RIMA\MEMORY\`

| Dosya | Type | Açıklama Satırı | Bayraklar |
|---|---|---|---|
| active_ai_asset_qa_gate_v2.md | project | RIMA AI Asset QA Gate v2 — Studio'dan adapte (2026-05-27 transfer). | stale_pit |
| agent_context_economy.md | project | Use when: CURRENT_STATUS bloat, CODEX.md/CLAUDE.md size, task planning rules, lessons files, | stale_pit |
| agents.md | feedback | 1. Claude -- final decisions, architecture, QC judgment, orchestration. Non-delegatable. | stale_pit |
| brawler_12_common_skills_spec.md | project | - **Kaynak:** Charge (0-5), kombo dokudukça dolar, 5 Charge → "Charged State" | stale_pit |
| discord_automation_risk.md | reference | \| Method \| Risk \| Why \| | stale_pit |
| discord_monitoring_channels.md | reference | \| # \| Channel \| Why \| | stale_pit |
| discord_pipeline.md | project | Export scripts deleted (export.ps1, run-all.ps1) -- caused Discord account restriction. |  |
| elementalist_12_common_skills_spec.md | project | - **Kaynak:** Element charge ritmi (Fire ↔ Frost ↔ Lightbreak) | stale_pit |
| encoding.md | feedback | - Internal .md files: ASCII-only. No Turkish diacritics. |  |
| feedback_agent_architecture.md | feedback | İki tip sub-agent var, karıştırma: |  |
| feedback_animate_character.md | feedback | Never call animate_character MCP for character animations. User generates all character animations manually in PixelLab UI. |  |
| feedback_autosprite_vs_pixellab_verdict.md | feedback | PixelLab production karakter + tile + prop pipeline kalsin. Autosprite production'a girmesin. |  |
| feedback_basic_attack_combo_identity.md | feedback | Every playable class must own its LMB and RMB identity. |  |
| feedback_camera_lock_hd2d.md | feedback | **Kural:** HD-2D oyun kamerası sabit. Hiçbir scene, hiçbir codex task, hiçbir agent şu değerleri DEĞİŞTİRMESİN. |  |
| feedback_canvas_size.md | feedback | animate-with-text-v3 obeys a pixel budget formula: width x height x frame_count <= 524,288 |  |
| feedback_claude_md_stub.md | feedback | **`CLAUDE.md` (3 satır, ~30 token):** | stale_pit |
| feedback_codex_task_routing.md | feedback | **1. UnityMCP gerekiyor mu?** |  |
| feedback_current_status_format.md | feedback | CLAUDE.md and CURRENT_STATUS.md must stay as lean as possible. Both load automatically every message; bloat compounds across the session. | stale_pit |
| feedback_gemma_models.md | feedback | \| Model \| Size \| Vision \| Notes \| |  |
| feedback_git_attribution.md | feedback | Commit after every logical unit of work — do not let untracked/modified files accumulate. |  |
| feedback_mcp_unity.md | feedback | - Provider: CoplayDev uvx mcpforunityserver; default port: 6401. | stale_pit |
| feedback_memory_system.md | feedback | When the user says "memory guncelle" (update memory), update **shared memory only**: | stale_pit |
| feedback_nlm_auth_recovery.md | feedback | NLM sorgusunda hata: |  |
| feedback_ollama_model_management.md | feedback | Do NOT unload gemma4:26b from GPU memory just because it is idle. Loaded models are fine to leave resident. | stale_pit |
| feedback_orchestra_discipline.md | feedback | **Rule:** If the task is a pure text-replace / rename / mechanical refactor across multiple files (or even one file with N+ identical edits), the o... |  |
| feedback_pixellab_direction.md | feedback | This file documents an existing/legacy direction implementation state. Current production source of |  |
| feedback_pixellab_init_image_dimension_lock.md | feedback | PixelLab **Create Image S-XL (new)** (`create_image_pixen`) — kullanırken **init image upload** edersen, output dimensions **otomatik kilitlenir** ... |  |
| feedback_pixellab_prompt_structure.md | feedback | Net adımlar ve blok yapısı PixelLab'dan daha iyi çıktı üretiyor. |  |
| feedback_temp_files.md | feedback | When creating a one-time file (QC report, review prompt, eval): | stale_pit |
| gate_socket_canonical_spec.md | project | - UI floating arrows YASAK | stale_pit |
| gunslinger_12_common_skills_spec.md | project | - **Kaynak:** Heat (0-100), atış ve mobilite ile dolar, Overheat sınırı aşılırsa risk/ödül | stale_pit |
| hexer_12_common_skills_spec.md | project | - **Kaynak:** Hex Stacks (0-10) düşman üzerinde | stale_pit |
| map_fragment_canonical_spec.md | project | - **Form:** Kırık Taş Tablet (Broken Stone Tablet) | stale_pit |
| nine_class_animation_states_demo_phase1_plan.md | project | - Claude state spec yazar → kullanıcı onaylar → MCP `create_character_state` gen ([[feedback-state-gen-mcp-user-approval-exception]]) | stale_pit |
| notebooklm_workflow.md | project | **Claude hiçbir dosyayı direkt Read ile açamaz — tek istisna: CURRENT_STATUS.md (session start).** | stale_pit |
| painter_suite_plan_v2_locked.md | project | **Locked:** 2026-05-26 (S109 close window) |  |
| painter_suite_progress_2026_05_26.md | project | **Plan reference:** [[painter-suite-plan-v2-locked]] | stale_pit |
| painter_suite_v1_1_roadmap_seeds.md | project | **Source:** agy report `STAGING/x_posts_research_agy_2026_05_26.md` (2026-05-26) | stale_pit |
| PIXELLAB_TOOL_GUIDE.md | project | Last checked: 2026-05-11 | stale_pit |
| pixellab_animation_techniques.md | feedback | 1. Feed idle pose into Animate with Text |  |
| pixellab_api_reliability.md | feedback | - V2: `https://api.pixellab.ai/v2/docs` -- has all Pro tools | revoked |
| pixellab_budget.md | project | ~2414 remaining as of 2026-05-03 (no significant spend in 2026-05-03 session). | stale_pit |
| pixellab_pipeline_workflows.md | reference | Stack: Claude + PixelLab MCP/REST API | stale_pit |
| pixellab_prompt_rules.md | feedback | Describe only the target change. Do not add "keep X unchanged" boilerplate -- it adds noise. |  |
| pixellab_sprites.md | project | 1. PixelLab UI -> Create from Reference v3 |  |
| project_64px_armed_character_locked.md | project | **Tarih:** 2026-05-12 | revoked |
| project_autosprite_trial_pending.md | project | - `claude mcp add autosprite` LIVE | stale_pit |
| project_chatgpt_canvas_fix.md | project | ChatGPT, tile sheet üretirken yanlış boyut veriyor (örn. 1254×1254 yerine 1024×1024, 1774×887 yerine 1024×512). |  |
| project_class_integration_order.md | project | Core 4 (integrate into game first): Warblade, Ranger, Shadowblade, Elementalist |  |
| project_dev_tool_rift_makeup.md | project | Canonical draft: `STAGING/CLAUDE_REVIEW_DEV_TOOL_RIFT_PORTAL_MAKEUP_2026-05-06.md`. |  |
| project_gate_map_reveal.md | project | Use when: gates, door sockets, map fragment, partial map, route reveal, camera framing. |  |
| project_karar_149_subroom_encounter_lock.md | project | **Supersedes:** [[project-subroom-encounter-system-proposal]] (proposal phase, now locked) | stale_pit |
| project_karar_150_act1_envanter_live.md | project | **2026-05-19 S95 LIVE:** Act 1 envanter v4 referans iskeleti hazır. Karar #150 LIVE + temiz local pack. Eski painterly_* envanter (63 PNG) archive'... | revoked, stale_pit |
| project_lauretthstudio_2d_illusion_kb_locked.md | project | **Full doc (RIMA STAGING):** `STAGING/LAURETH_2D_ILLUSION_LIBRARY.md` |  |
| project_nlm_notebook_id.md | project | ``` | revoked |
| project_path_c_hybrid_lock.md | project | Use when: Act 1 production pipeline, Codex image_gen floor/wall, painted base + sprite overlay, layer architecture, Path A vs C decision, Hades for... | revoked |
| project_player_hades_system.md | project | Movement facing, combat aim, and visual facing are **three separate concerns** in RIMA. |  |
| project_pure_2d_topdown_pivot_2026-05-12.md | project | **RIMA = Pure 2D Top-Down chibi pixel art ARPG roguelite.** |  |
| project_resume_2026-06-11.md | project | **Tip:** project | orphan |
| project_room_blueprints.md | project | Use when: room catalogue, map blueprint, Hades-like inspiration, Act 1 room design, room families, |  |
| project_room_staging_system.md | project | Use when: map staging, Hades-like room edges, black camera edge, room variants, authored masks, |  |
| project_skill_offer_system.md | project | Canonical decision doc: `TASARIM/SKILL_OFFER_SYSTEM_DECISION_2026-05-03.md`. |  |
| project_story_lore.md | project | Use when: story, lore, RIMA meaning, Rift March, final boss reveal, ending hook, |  |
| project_subroom_canonical_tags_lock.md | project | **Karar #150 LIVE → Karar #149 sub-room slot grammar** Codex APPROVE_WITH_REVISIONS verdict #4 ile lock. | stale_pit |
| project_subroom_encounter_system_proposal.md | project | > SUPERSEDED 2026-05-19 — see [[project-karar-149-subroom-encounter-lock]] | stale_pit |
| project_test_automation.md | project | RIMA'da **Behavioral Contract Testing** sistemi kuruldu. Tüm beklenen davranışlar önce `Contracts/` class'larında sabit olarak tanımlanır, ardından... |  |
| project_ui_qa_ai_skills.md | project | Use when: UI concept, character menu, encounter UI, QA tester flow, Unity tests, shared |  |
| project_ui_state_blueprint.md | project | Use when: HUD, skill bar, reward UI, gate choice, partial map, character/build overlay, UI concept. |  |
| project_wall_production_pipeline_s99_late.md | project | **Modular atlas pipeline DROPPED. Continuous source art pipeline PRIMARY.** | stale_pit |
| project_warblade_weapon_animation_plan_s99_late.md | project | S99 LATE evening — wall pipeline Discord cevabı bekliyor. Paralel iş olarak karakter weapon + animation production'a geçiş. Vertical slice için War... | stale_pit |
| ranger_12_common_skills_spec.md | project | - **Kaynak:** Focus (mesafe disiplini, düşman uzakta → dolum) | stale_pit |
| ravager_12_common_skills_spec.md | project | - **Kaynak:** Fury (0-100), SADECE hasar ALARAK dolar (diğer 9 class'ın aksine, UNIQUE), HP düştükçe daha hızlı dolar | stale_pit |
| reference_pixellab_prompt_grammar.md | reference | Bu dosya RIMA icin PixelLab prompt yazim referansidir. Basliklar Turkce tutuldu, prompt template'leri PixelLab daha iyi anladigi icin Ingilizce yaz... |  |
| ronin_12_common_skills_spec.md | project | - **Kaynak:** Tension (0-100), kılıç kınında biriken pasif yoğunluk | stale_pit |
| shadowblade_12_common_skills_spec.md | project | - **Kaynak:** Sever (0-100), düşmandan/vektörden phase-through ile dolar (pozisyonel, stack-on-hit YOK) | stale_pit |
| statusline.md | feedback | Script: C:/Users/ydbil/.claude/usage_statusline.py |  |
| subagents.md | feedback | \| Agent \| Model \| Scope \| Writes \| | stale_pit |
| summoner_12_common_skills_spec.md | project | - **Kaynak:** Charges (0-4), zamanla dolar veya minyon feda ile instant | stale_pit |
| wang_tile_build_workflow_rima.md | project | **Studio kaynak (üst-otorite):** `F:/LaurethStudio/MEMORY/studio_custom_wang_build_workflow.md` (S15 LOCK) |  |
| warblade_12_common_skills_spec.md | project | - Pasif dolmaz, hasar verme + CC + skill bonus ile dolar | stale_pit |
| warblade_animation_states_demo_phase1_plan.md | project | - **State'leri Claude (orchestrator) belirler + prompt yazar** → kullanıcı onaylar → PixelLab Web UI gen | stale_pit |
| weapon_master_spec_10_class.md | project | 1. **Body-Only Sprite:** Karakter bedeni silahsız PixelLab gen |  |

### User-Level Memory İle Çakışan/Tekrar Eden Konular

Aşağıdaki dosyalar hem kullanıcı-seviyesi hem de repo-seviyesi bellek klasörlerinde aynı isimlerle bulunmaktadır ve drift/senkronizasyon riski taşımaktadır:

1. `project_karar_149_subroom_encounter_lock.md`
2. `project_karar_150_act1_envanter_live.md`
3. `project_resume_2026-06-11.md`
4. `project_subroom_canonical_tags_lock.md`
5. `project_wall_production_pipeline_s99_late.md`

## E. STAGING Üst-Seviye

Konum: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\` (Sadece üst-seviye dosyalar)

| Dosya | Tarih | LIVE/Bayat Şüphesi |
|---|---|---|
| _agy_flash_canary.task.md | 2026-05-28 | LIVE (Canonical spec/plan/decision) |
| _agy_weapon_anim_vfx_secondeye.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| _codex_done_garbled_backup.md | 2026-05-31 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| _codex_weapon_anim_vfx_feasibility.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| A0_MORE_HITLAYER_FIX_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| A1_SUNDERED_TELL_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| A1_WEAPONDB_CLARIFY.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| A1b_A2_A3_EXECUTELOOP_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| A4_CHAINWINDOW_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| A5_CHAINUI_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| a6_wall_imagegen_codex_qc.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| a6_wall_mockup_codex_summary.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| a6_wall_pixelart_codex_qc.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| a6_wall_user_pixellab_guide.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ANALYZE_AX_CLEANUP_MAPDESIGNER_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| ANALYZE_AX_DEMO_MAP_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| ANALYZE_AX_ISO_PERSPECTIVE_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| ANALYZE_CX_DEMO_MAP_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| ANIM_CATALOG_agy.task.md | 2026-05-27 | LIVE (Canonical spec/plan/decision) |
| ANIM_CATALOG_codex.task.md | 2026-05-27 | LIVE (Canonical spec/plan/decision) |
| ANIMATION_PROMPT_CATALOG.md | 2026-06-05 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| antigravity_feedback.md | 2026-05-25 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| APPLY_CX_ISO_CODEPATHS_S6.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| APPLY_CX_ISO_PERSPECTIVE_S6.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| APPLY_CX_MAPDESIGNER_STAGING_S6.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ASSET_PIPELINE_BRIEF_2026-06-04.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ASSET_PIPELINE_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| ASSET_USAGE_AUDIT_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| AUTOMATED_BALANCE_TESTING_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| AUTONOMOUS_BACKLOG_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| AUTONOMOUS_RUN_2026-06-12.md | 2026-06-12 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ax_card_anim_research.md | 2026-06-03 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| ax_cliff_visual_review.md | 2026-06-02 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| AX_FLASH_DONE.md | 2026-05-31 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| ax_mech_video_check.md | 2026-06-03 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| AX_PRO_DONE.md | 2026-05-31 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| b1_animation_catalog_weaponless_cleanup_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| B1_B3_REVIEW_AGY_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| B1_PROMPT_CLEANUP_DONE.md | 2026-05-28 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| B3_OVERLAY_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| B4_CARDSELECT_WIRE_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| BG_LAYER_ARCHITECTURE_VERDICT.md | 2026-05-25 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BLADES_OF_MIRAGE_PIPELINE_REPORT.md | 2026-05-28 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| blades_of_mirage_agy_research_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| BOSS_MOB_DESIGN_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_IMAGEGEN_A_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_IMG_MENU_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_IMPACTFRAME_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_P1A_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_P1B_FEEL_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_P1C_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_P1D_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_P2_STORY_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_P3_AUDIO_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| BUILD_RUNSTATS_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CAMERA_ZOOM_RECOMMENDATION.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CHAMBER_PRESENTATION_DECISION_2026-06-08.md | 2026-06-08 | LIVE (Canonical spec/plan/decision) |
| CHARSELECT_FINAL_BRIEF_2026-06-04.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CHARSELECT_LAYOUT2_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| CHARSELECT_REFINE_BRIEF_2026-06-04.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CHARSELECT_REFINE_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| CHARSELECT_ROSTERROOM_BRIEF_2026-06-04.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CHARSELECT_ROSTERROOM_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| CHARSELECT_V3_DECISION_2026-06-05.md | 2026-06-05 | LIVE (Canonical spec/plan/decision) |
| CHATGPT_BATCH_REVIEW_PACKAGE_2026-06-12.md | 2026-06-12 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| CHATGPT_CODE_REVIEW_PROMPT_2026-06-09.md | 2026-06-09 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| CHATGPT_DOSSIER_animated_bg_2026-06-11.md | 2026-06-11 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CHATGPT_DOSSIER_room_beauty_2026-06-11.md | 2026-06-11 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CHATGPT_OVERNIGHT_REVIEW_DOSSIER_2026-06-12.md | 2026-06-12 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| CHATGPT_RIMA_WALLLESS_PROMPT.md | 2026-05-25 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CHATGPT_ROOM_DESIGN_PROMPT.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CHATGPT_STATUS_REVIEW_2026-06-08.md | 2026-06-08 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| CLEANUP_ANALYSIS_2026_05_25.md | 2026-05-25 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLEANUP_EXECUTION_CODEX.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLEANUP_NLM_FIX_LAURETH_S6.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLIFF_ART_CROSSROADS_BRIEF.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLIFF_AUTONOMOUS_REVIEW_BRIEF.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| CLIFF_BLACK_LAYER_DIAGNOSIS.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLIFF_CORNER_ILLUSION_BRIEF.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLIFF_DEPTH_BRIEF.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLIFF_DEPTH_SYNTHESIS_S114S5.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLIFF_F_FULL_SANG_DESIGN.md | 2026-05-27 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLIFF_F1_F4_F5_REVIEW.md | 2026-05-27 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| CLIFF_F2_F3_F4F5_FIX_REVIEW.md | 2026-05-27 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| CLIFF_FLOATING_FEEL_DECISION.md | 2026-05-27 | LIVE (Canonical spec/plan/decision) |
| CLIFF_FLOATING_FEEL_research_agy.md | 2026-05-27 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| CLIFF_FLOATING_FEEL_research_agy.task.md | 2026-05-27 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| CLIFF_FLOATING_FEEL_research_codex.md | 2026-05-27 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| CLIFF_FLOATING_FEEL_research_codex.task.md | 2026-05-27 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| CLIFF_MANUAL_BRUSH_DESIGN.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CLIFF_NATURAL_BRIEF.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| cliff_gaptest_report.md | 2026-06-02 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| CODEANIM_DECISION_2026-06-05.md | 2026-06-05 | LIVE (Canonical spec/plan/decision) |
| codex_chatgpt_spec_eval_2026-05-24.md | 2026-05-24 | LIVE (Canonical spec/plan/decision) |
| COLOSSUS_ETERNAL_BLIGHT_RIMA_WEAPON_REPORT.md | 2026-05-28 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| colossus_eternal_blight_agy_research_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| COMBAT_RUN_ISSUES_2026-06-09.md | 2026-06-09 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| commit_plan_review_codex_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| CONSULT_AX_FEEL_EDITOR_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_AX_ISO_FLOOR_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_AX_PORTAL_ANIM_R2_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_AX_PORTAL_MAP_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_AX_REALLOOK_EDITOR_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_CX_FEEL_EDITOR_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_CX_ISO_FLOOR_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_CX_PORTAL_ANIM_R2_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_CX_PORTAL_MAP_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_CX_REALLOOK_EDITOR_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONSULT_FLOOR_MAP_APPROACH_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONTROL_SCHEME_CONSULT_AX.md | 2026-05-30 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONTROL_SCHEME_CONSULT_CX.md | 2026-05-30 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| CONTROL_SCHEME_SYNTHESIS_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CROSS_CLASS_DESIGN_SPEC.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| CX_FRAGMENT_GATE_LOOP_FIX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CX_ISO_DEPTH_WALKABLE_FIX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CX_LIGHT_SOURCE_IMPL.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CX_LIGHTING_ROOMFILL_CODE.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CX_SWING_VISIBILITY_CODE.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| CX_WEAPON_HAND_TECH_ANALYSIS.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| D_SKILLOFFERUI_ICON_WIRE_DONE.md | 2026-05-28 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| D2_LAYER_ARCH_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| d2_layer_arch_lock_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| D3_PAINTER_MODE_TABS_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| d3_painter_mode_tabs_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| D4_COLLIDER_DRAG_HANDLE_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| d4_collider_drag_handle_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| D5_5_CLIFF_2STAGE_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| d5_5_cliff_2stage_separation_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| D5_5_orphan_cleanup_log.md | 2026-05-27 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| D5_5_orphan_cliff_inventory.md | 2026-05-27 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| D5_DIRECTIONAL_CLIFF_WIRE_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| d5_directional_cliff_wire_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| DAMAGE_TYPE_TAXONOMY_DECISION_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| day2_gate_mapfragment_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| day2_gate_mapfragment_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| day25_mapfragment_spawner_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| day25_mapfragment_spawner_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| DECISIONS_3AI_TASK.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| DECISIONS_LOCK.md | 2026-05-28 | LIVE (Canonical spec/plan/decision) |
| DECISIONS_S6.md | 2026-05-30 | LIVE (Canonical spec/plan/decision) |
| DEMO_24H_PLAN_DECISION_2026-06-09.md | 2026-06-09 | LIVE (Canonical spec/plan/decision) |
| DEMO_ARCHITECTURE_DECISION_2026-06-03.md | 2026-06-03 | LIVE (Canonical spec/plan/decision) |
| DEMO_DESIGN_PLAN_2026-06-10.md | 2026-06-10 | LIVE (Canonical spec/plan/decision) |
| DEMO_FINAL_PLAN_2026-06-10.md | 2026-06-10 | LIVE (Canonical spec/plan/decision) |
| DEMO_FINALIZE_DECISION_2026-06-10.md | 2026-06-10 | LIVE (Canonical spec/plan/decision) |
| DEMO_MAP_PLAN_OPUS_LOCK_S6.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| DEMO_MASTER_PLAN_2026-06-09.md | 2026-06-09 | LIVE (Canonical spec/plan/decision) |
| DEMO_MOB_AUDIT_S114.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| DEMO_SKELETON_PLAN.md | 2026-05-28 | LIVE (Canonical spec/plan/decision) |
| DEMO_TEST_MATRIX_DECISION_2026-06-03.md | 2026-06-03 | LIVE (Canonical spec/plan/decision) |
| DEMO_TOOLS_REPORT_AND_PLAN_2026-06-12.md | 2026-06-12 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| DEMO_TOOLS_SCOPE_DECISION_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| DEPTH_AND_WALLRUN_RECIPE_S6.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| DEPTH_CAMERA_EDGE_AX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| DEPTH_CAMERA_EDGE_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| DESIGN_CONSULT_AX.md | 2026-05-30 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DESIGN_CONSULT_CX_RESULT.md | 2026-05-30 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DESIGN_CONSULT_CX.md | 2026-05-30 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DESIGN_LOCK_DEMO_S6.md | 2026-05-30 | LIVE (Canonical spec/plan/decision) |
| DETECT_AX_RESULT_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DETECT_AX_UNIFIED_DESIGNER_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DETECT_CX_PROJECT_AUDIT_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DETECT_CX_RESULT_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DETECT_CX_UNIFIED_DESIGNER_S6.md | 2026-05-31 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DIAGNOSE_AX_VARIANT_PAINT_UX_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DIAGNOSE_CX_DESIGNER_REGRESSION_S6.md | 2026-06-01 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DIRECTION_CONSULT_AX.md | 2026-05-30 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| DRAG_PLACE_IMPL_PLAN_S6.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| DRIFT_DEBUG.task.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| EDGE_OF_WATER_RESEARCH_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| EXEC1_MENU_CONSOLIDATION_CX.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| EXEC2_ASSETPACK_REGISTRY_CX.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| EXEC3_CLIFF_MATH_CX.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| EXEC5_IMAGEGEN_PLACEHOLDERS_CX.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| EXECUTION_WORKFLOWS_S6.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| F1_ADAPTIVE_CLUSTER_FILTER_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| f1_adaptive_cluster_filter_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| F2_DROP_SHADOW_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| f2_drop_shadow_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| F2_F5_FINAL_FIX_DONE.md | 2026-05-28 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| f2_f5_final_fix_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| F2_PAUSE_EDITOR_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| F3_PARALLAX_6LAYER_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| f3_parallax_6layer_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| F4_DUST_PARTICLE_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| f4_dust_particle_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| F4_F5_FIX_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| f4_f5_fix_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| F5_CLIFF_ANIM_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| f5_cliff_idle_anim_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| F6_F7_CULLING_SMOKE_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| f6_f7_culling_smoke_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| FIXFWD_STEP12_CX_TASK.md | 2026-06-01 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| FIXFWD_STEP3_VARIANT_CX_TASK.md | 2026-06-01 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| FLOOR_ISO_VS_TOPDOWN_AX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| FLOOR_ISO_VS_TOPDOWN_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| FLOOR_PERSPECTIVE_CONCEPT_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| FLOOR_TILE_TOPDOWN_64_IMAGEGEN_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| FORWARD_EXECUTION_ROADMAP.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| FRAGMENT_REWARD_CONCEPT_IMAGEGEN_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| FUTURE_DESIGN_IDEAS_S6.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| GAME_STATE_AND_PLAN_2026-06-02.md | 2026-06-02 | LIVE (Canonical spec/plan/decision) |
| GATESLOT_DECISION_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| GODOT_MIGRATION_DECISION_2026-06-08.md | 2026-06-08 | LIVE (Canonical spec/plan/decision) |
| HADES_INTERACT_PROMPT_DECISION_2026-06-09.md | 2026-06-09 | LIVE (Canonical spec/plan/decision) |
| HOLD_TO_BUILD_DRAG_PLACE_DESIGN_S6.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| HOLD_TO_BUILD_PATTERN_AX_RESEARCH.md | 2026-05-31 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| HUD_LAYOUT_DECISION_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| IMAGEGEN_ASSET_PACK_PLAN_S6.md | 2026-05-30 | LIVE (Canonical spec/plan/decision) |
| IMAGEGEN_PACK_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| IMAGEGEN_PLACEHOLDER_REGISTRY.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| IMAGEGEN_PRODUCE_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| IMAGEGEN_PRODUCE_GROUPA_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| IMPL_P0_CODE_cx.md | 2026-06-02 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| IMPL_PLAN_DEPTH_AND_CROSSCLASS.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| INTEGRATION_BACKLOG_S6.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| INTERACTION_PROMPT_DECISION_2026-06-08.md | 2026-06-08 | LIVE (Canonical spec/plan/decision) |
| INVERSION_MECHANIC_DECISION_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| ISO_ASSET_PACK_BLUEPRINT_S6.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ISO_GEN_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ISO_MOCK_IMAGEGEN_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ISO_PACK_BOARD_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ISO_PACK_FULL_GEN_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ISO_ROOM_CONCEPTS_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ISO_TILING_LOGIC_BRIEF.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ISO_TILING_LOGIC_DECISION.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| ISO_VS_TOPDOWN_DECISION_AX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ISO_VS_TOPDOWN_DECISION_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| iso_asset_codex_qc.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| iso_walls_max_dramatic_qc.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| karar_118a_wizard_report.md | 2026-05-24 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| LAURETH_2D_ILLUSION_LIBRARY.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| LAURETH_2D_PAINTER_SUITE_PLAN_V2_RIMA_REUSE.md | 2026-05-26 | LIVE (Canonical spec/plan/decision) |
| LAURETH_2D_PAINTER_SUITE_PLAN.md | 2026-05-26 | LIVE (Canonical spec/plan/decision) |
| LAURETHSTUDIO_PLAYBOOK_EXTRACTION_2026-06-13.md | 2026-06-13 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| LAYERED_ISO_LOOP_DECISIONS_S6.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| LAYERED_ISO_LOOP_DESIGN_S6.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| LIGHTING_ROOMFILL_LOCK_S6.md | 2026-05-30 | LIVE (Canonical spec/plan/decision) |
| LITTLE_MASTER_RESEARCH_agy.task.md | 2026-05-28 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| LITTLE_MASTER_RESEARCH_codex.task.md | 2026-05-28 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| LIVE_EDITOR_ASSET_BROWSER_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| LIVE_EDITOR_GAP_S114.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| LIVE_PATH_DETERMINATION_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| LMB_RMB_SLOT_DECISION_2026-06-11.md | 2026-06-11 | LIVE (Canonical spec/plan/decision) |
| MAP_PLACEMENT_DIAGNOSE_AX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| MAPDESIGNER_DECISION_2026-06-06.md | 2026-06-06 | LIVE (Canonical spec/plan/decision) |
| MASTER_EXECUTION_PLAN.md | 2026-05-28 | LIVE (Canonical spec/plan/decision) |
| MASTER_PLAN_FINAL_2026-06-06.md | 2026-06-06 | LIVE (Canonical spec/plan/decision) |
| MASTER_PLAN_S6_AUTONOMOUS.md | 2026-05-30 | LIVE (Canonical spec/plan/decision) |
| MCP_RELOAD_ROBUSTNESS_FINDINGS.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| MECHANIC_ADDITIONS_SYNTHESIS_2026-06-03.md | 2026-06-03 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| MECHANIC_SYNTHESIS_2docs_2026-06-03.md | 2026-06-03 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| MENU_ART_IMAGEGEN_SPEC_S6.md | 2026-05-30 | LIVE (Canonical spec/plan/decision) |
| MINA_RESEARCH_AGY_FULL_TASK.md | 2026-05-30 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| MINA_RESEARCH_AGY_TASK.md | 2026-05-30 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| MINA_RESEARCH_CX_TASK.md | 2026-05-30 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| MOB_BOSS_PIXELLAB_SHEET_2026-06-10.md | 2026-06-10 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| MOB_PRODUCTION_PLAN_S6.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| MODULAR_ABILITY_DECISION_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| MODULAR_PIPELINE_AGY.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| MODULAR_PIPELINE_CX.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| MODULAR_PIPELINE_MASTER.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| MODULAR_PROPS_DECISION_2026-06-05.md | 2026-06-05 | LIVE (Canonical spec/plan/decision) |
| MOMENT_SPEC_S6.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| MORNING_REPORT_S6.md | 2026-05-30 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| MULTI_RESOLUTION_SCALING_RESEARCH.md | 2026-05-28 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| N10_DEV_TOOLS.task.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| N2_INVENTORY_CONSISTENCY_ACTION.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| N3_LIGHTING_DESIGN_FINAL.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| N3_LIGHTING_DESIGN.task.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| N4_CRACKS_DESIGN_FINAL.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| N4_CRACKS_DESIGN.task.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| N5_AMBIANCE_BIBLE.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| N8_CLIFF_RELOAD.task.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| N9_UX_TOOLS_FINAL.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| N9_UX_TOOLS.task.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| NATURAL_ROOM_COMPOSITION_AX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| NEW_SESSION_WORKLIST_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| NLM_CLEANUP_DONE.md | 2026-05-28 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| NLM_CONFLICT_RESOLUTION_S114.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| OBSTACLES_DOORS_DECISION_2026-06-03.md | 2026-06-03 | LIVE (Canonical spec/plan/decision) |
| ODA_TRANSITIONS_WIRE_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| oda_transitions_deferred_wire_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| OVERLAP_CLEANUP_DECISION_2026-06-09.md | 2026-06-09 | LIVE (Canonical spec/plan/decision) |
| OVERNIGHT_REVIEW_FIX_DECISION_2026-06-12.md | 2026-06-12 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| P0_WARBLADE_HITLAYER_FIX_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| PARALLAX_BUILDER_PREVIEW_SCRUB_DONE.md | 2026-05-28 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| PARALLAX_BUILDER_STATE_AUDIT.md | 2026-05-28 | LIVE (Canonical spec/plan/decision) |
| PARALLAX_L3_DESIGN.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PARALLAX_PAINTER_PHASE_A_PLAN.md | 2026-05-26 | LIVE (Canonical spec/plan/decision) |
| PARALLAX_REVIEW_CODEX.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| PIXELART_SCALING_BRIEF_2026-06-04.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELART_SCALING_REPORT_2026-06-04.md | 2026-06-04 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| PIXELLAB_ANALYSIS_AGY.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_ANALYSIS_CODEX.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_ANALYSIS_OPUS.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_API_V2_LLMS.md | 2026-05-13 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_API_V2_RAW.md | 2026-05-18 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_API_V2_REFERENCE.md | 2026-05-18 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_INVENTORY_CATALOG_2026_05_25.md | 2026-05-25 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_INVENTORY_DUMP_DONE.md | 2026-05-28 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| PIXELLAB_INVENTORY_MASTER.md | 2026-05-28 | LIVE (Canonical spec/plan/decision) |
| PIXELLAB_KNOWLEDGE_BASE.md | 2026-05-28 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_PIPELINE_GAP_RESEARCH_2026-06-08.md | 2026-06-08 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| PIXELLAB_PRODUCTION_GUIDE_v1.md | 2026-05-17 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_PRODUCTION_SHEET_2026-06-10.md | 2026-06-10 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_SESSION_PLAN_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| PIXELLAB_SHORTS_REPORT_S6.md | 2026-05-30 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| PIXELLAB_SHORTS_RESEARCH_AX.md | 2026-05-30 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| PIXELLAB_SKILL_ICON_PACK_2026-06-09.md | 2026-06-09 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_SYNTHESIS_S114.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_VFX_BATCH_LIMITS_2026-06-12.md | 2026-06-12 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PIXELLAB_WEAPON_METHOD_DECISION_2026-06-08.md | 2026-06-08 | LIVE (Canonical spec/plan/decision) |
| PIXELLAB_WEAPON_PROMPT_SHEET_2026-06-09.md | 2026-06-09 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| pixellab_analysis_agy_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| pixellab_analysis_codex_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| pixellab_analysis_opus_task.md | 2026-05-24 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| pixellab_inventory_dump_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| PLAYABLE_ROADMAP_DECISION_2026-06-05.md | 2026-06-05 | LIVE (Canonical spec/plan/decision) |
| PLAYABLE_WORKFLOW_BACKLOG.md | 2026-06-02 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PLAYTEST_BUGS_DECISION_2026-06-08.md | 2026-06-08 | LIVE (Canonical spec/plan/decision) |
| PORTAL_IMPORT_SETTINGS.md | 2026-06-07 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PORTAL_ORB_PIXELLAB_RESEARCH_S6.md | 2026-05-31 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| PORTAL_PACK_DECISION_2026-06-06.md | 2026-06-06 | LIVE (Canonical spec/plan/decision) |
| PORTAL_PREVIEW_SYSTEM_SPEC_S6.md | 2026-05-31 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| PORTAL_SOCKET_PLACEMENT_GUIDE.md | 2026-06-07 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PRESENTATION_PREP_2026-06-11.md | 2026-06-11 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PROBE_IMAGEGEN_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| PROPS_DOORS_PLACEMENT_PLAN_2026-06-11.md | 2026-06-11 | LIVE (Canonical spec/plan/decision) |
| Q4_CLIFF_ASSET_PACK_PROMPTS.md | 2026-06-02 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| QUEUE10_ROUTING_DECISION_2026-06-05.md | 2026-06-05 | LIVE (Canonical spec/plan/decision) |
| R4_DECISION_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| R5_VIDEO_DECISION_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| RAPOR_REVIEW2_DECISION_2026-06-07.md | 2026-06-07 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| README.md | 2026-05-21 | LIVE (README dosyasi) |
| REPORT_CONTENT_DECISION_2026-06-06.md | 2026-06-06 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| REPORT_NOTES_2026-06-10.md | 2026-06-10 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| research_2tweets_npaka123_summary.md | 2026-05-24 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| research_arpg_dungeon_room_industry_practices.md | 2026-05-24 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| research_wall_less_room_games.md | 2026-05-24 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| REVIEW_A3_FRAGMENT_FLOW_CX.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_A3v2_A6_AGY.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_AX_EXEC1_EXEC2_S6.md | 2026-06-01 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_AX_RESULT_S6.md | 2026-06-01 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_AX_UNIFIED_DESIGNER_S6.md | 2026-06-01 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_backlog_agy.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_backlog_w2_codex.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_BIGWORK_AX.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_BIGWORK_CX.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_BOSS_P3_CX.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_C1C3_INPUT_CX.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_camera_fix.task.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_combat_weapon_agy.task.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_combat_weapon_cx.task.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_CX_UNIFIED_DESIGNER_S6.md | 2026-06-01 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_DECISIONS_AX.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_DESIGN_CX.md | 2026-06-01 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_IMPACTFRAME_AX.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_mob_plan_agy.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_mob_plan_codex.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_moment_spec_agy.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_moment_spec_codex.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_N1_conflict.task.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_NLM_RECOVERY_AX.md | 2026-05-30 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_parallax_L4_agy.task.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_parallax_L4_cx.task.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_PLAN_ax.md | 2026-06-02 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_PLAN_cx.md | 2026-06-02 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_synthesis_agy.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_synthesis_codex.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_W1_code_codex.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REVIEW_W1_map_agy.md | 2026-05-29 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| REWARD_PORTAL_DECISION_2026-06-02.md | 2026-06-02 | LIVE (Canonical spec/plan/decision) |
| RIMA_CANON_BRIEF_FROM_NLM.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| RIMA_COMBO_DEPTH_AGY_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| RIMA_COMBO_DEPTH_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| RIMA_DIRECTION_LOCK_S6.md | 2026-05-30 | LIVE (Canonical spec/plan/decision) |
| RIMA_EVAL_ROUND2_AGY_TASK.md | 2026-05-30 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| RIMA_EVAL_ROUND2_CX_TASK.md | 2026-05-30 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| RIMA_LIVE_TOOL_codex_appendix.md | 2026-05-27 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| RIMA_LIVE_TOOL_DECISION.md | 2026-05-27 | LIVE (Canonical spec/plan/decision) |
| RIMA_LIVE_TOOL_research_agy.md | 2026-05-27 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| RIMA_LIVE_TOOL_research_codex.md | 2026-05-27 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| RIMA_ROADMAP_AND_CLEAN_STRUCTURE_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| RIMA_SKILL_MECHANIC_EVAL_AGY_TASK.md | 2026-05-30 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| RIMA_SKILL_MECHANIC_EVAL_CX_TASK.md | 2026-05-30 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| RIMA_SKILL_MECHANIC_FINAL_REPORT_S6.md | 2026-05-31 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| ROADMAP_FEASIBILITY_CX.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ROOM_DESIGN_COUNCIL_BRIEF_2026-06-04.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ROOM_DESIGN_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| ROOM_DESIGN_DECISION_2026-06-11.md | 2026-06-11 | LIVE (Canonical spec/plan/decision) |
| room_layout_phase1_demo.md | 2026-05-27 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md | 2026-05-26 | LIVE (Canonical spec/plan/decision) |
| ROOM_PAINTER_DAY5_LIVE_PREVIEW_SPEC.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| ROOM_QC_REPORT_2026-06-05.md | 2026-06-05 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| ROOM_SYSTEM_DECISION_2026-06-03.md | 2026-06-03 | LIVE (Canonical spec/plan/decision) |
| ROOM_TRANSITIONS_codex_output.md | 2026-05-27 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ROOM_TRANSITIONS_codex_write.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ROOM_TRANSITIONS_DESIGN.md | 2026-05-27 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| ROOM_TRANSITIONS_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| ROOM_TRANSITIONS_sonnet_review.md | 2026-05-27 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| room_transitions_dispatch_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ROOMPAINTER_V2_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ROOMTOOL_AUDIT_S6.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| ROOMTOOL_FIX_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ROOMTOOL_FUNC_SPEC_S6.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| ROOMTOOL_IMPL_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| ROOMTOOL_IMPROVEMENT_PLAN_S6.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| ROOMTOOL_UX_SPEC_S6.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| ROOMTOOL_VERIFY_REPORT_S6.md | 2026-05-31 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| RUINED_KEEP_BUILD_PLAN_S6.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| RUINED_KEEP_ORGANIC_COMPOSITION_RULES.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| RUINED_KEEP_ROOM_LOOK_LOCK_S6.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| RUINED_KEEP_SEGMENT_DATA_S6.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| RUINED_KEEP_WALLKIT_IMAGEGEN_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| RUNMAP_UI_ASSET_PRODUCTION_DECISION_2026-06-11.md | 2026-06-11 | LIVE (Canonical spec/plan/decision) |
| s109_2d_illusion_tricks_pixellab_agy.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s109_agy_wrapper_smoke_test.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s109_antigravity_cliff_review_agy.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s109_antigravity_cliff_review_codex.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s109_chatgpt_room_painter_spec.md | 2026-05-26 | LIVE (Canonical spec/plan/decision) |
| s109_lauretthstudio_2d_illusion_knowhow_agy.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s109_rpgmaker_js_and_free_2d_assets_agy.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s109_sang_parallax_review_agy.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s109_sang_parallax_review_codex.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s109_x_eringijirou_review_agy.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s109_x_eringijirou_review_codex.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s110_agy_conpty_full_fix.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_cliff_manual_brush_design_sonnet.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_cliff_parallax_depth_pattern_research_agy.md | 2026-05-26 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| s110_parallax_authoring_ux_benchmark_agy.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_parallax_yol_b_phase_a_plan_sonnet.md | 2026-05-26 | LIVE (Canonical spec/plan/decision) |
| s110_parallax_yol_b_phase_a_skeleton_codex.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_phase_a_day2_asset_palette_codex.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_phase_a_day2_review_sonnet.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s110_phase_a_day3_review_sonnet.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s110_phase_a_day3_sceneview_placement_codex.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_phase_a_day4_inspector_postprocessor_codex.md | 2026-05-26 | LIVE (Canonical spec/plan/decision) |
| s110_phase_a_day4_patch_codex.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_phase_a_day5_live_preview_collider_authoring_sonnet.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_phase_a_day5a_live_preview_pane_codex.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_phase1_cliff_double_trigger_fix.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_phase2_cliff_override_and_ui_category.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_phase3_cliff_refactor_review_codex.md | 2026-05-26 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| s110_pro_prompts_tweets_wang_research_agy.md | 2026-05-26 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| s110_room_painter_all_in_one_ux_redesign_sonnet.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| s110_wang_pixellab_diy_followup_agy.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| S112_REVIEW_TASK.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| SAFE_DELETE_AUDIT_S114.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| SANDBOX_DIRECTOR_DECISION_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| SCENE_WIRING_RUNBOOK_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| SENIOR_DESIGN_REPORT_DRAFT.md | 2026-06-01 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| SENIOR_DESIGN_REPORT_PLAN.md | 2026-06-01 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| SESSION_RESUME_2026-06-10.md | 2026-06-10 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| SHATTERED_KEEP_BACKDROP_DECISION_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| SKILL_VFX_IMPLEMENTATION_PLAN_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md | 2026-06-12 | LIVE (Canonical spec/plan/decision) |
| skills_batch_9_install_report.md | 2026-05-24 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| skills_install_universal_report.md | 2026-05-24 | BAYAT (Rapor dosyasi, _process/ altina tasinmali.) |
| SODAMAN_LEARNINGS_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| SPELLVFX_SKILLICON_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| sprite_forge_vs_imagegen_comparison.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| STATICAUDIT_DECISION_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| STORY_REVISION_S6.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| STRATEGIC_SYNTHESIS_S6.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| studio_layer_splatmap_wang_bilgi_havuzu_2026_05_26.md | 2026-05-26 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| STYLE_PRESERVING_UPSCALE_ANALYSIS_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| STYLE_PRESERVING_UPSCALE_RESEARCH_AX.md | 2026-05-30 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| SYSTEM_OVERVIEW_FOR_GEMINI_2026_05_25.md | 2026-05-25 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| T3_ARCHITECTURE_BRIEF.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| T3_F1_DONE.md | 2026-05-27 | BAYAT (Tamamlanmis gorev kaydi (DONE), _process/ veya _archive/ altina tasinmali.) |
| t3_f1_json_schema_serializer_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| T3_Q1_REVIEW_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| T3_Q2_REVIEW_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| T3_Q3_REVIEW_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| T3_TOOL_FULL_DESIGN.md | 2026-05-27 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| task_art_wiring_cx.md | 2026-06-02 | LIVE (Canonical spec/plan/decision) |
| task_brush_size_cx.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| task_cliff_align_math_mcp_cx.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| task_cliff_archive_review_cx.md | 2026-06-02 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| task_cliff_voidfade_cx.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| task_f2_topdown_camera_cx.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| task_iso_paint_cliff_zoom_cx.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| task_multimap_foundation_cx.md | 2026-06-02 | LIVE (Canonical spec/plan/decision) |
| task_portal_reward_imagegen.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| TASK_portalpack_production_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| task_research_pixelart_quantize_cx.md | 2026-06-01 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| task_review_design_cx.md | 2026-06-01 | BAYAT (Review dosyasi, _process/ altina tasinmali.) |
| task_w2_pivot_archive_cx.md | 2026-06-02 | LIVE (Canonical spec/plan/decision) |
| task_w3_gameloop_cx.md | 2026-06-02 | LIVE (Canonical spec/plan/decision) |
| task_yt_transcript_summary.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| TBH_WALK_SELECT_DECISION_2026-06-05.md | 2026-06-05 | LIVE (Canonical spec/plan/decision) |
| TILEMAP_VISUAL_QUALITY_DECISION_2026-06-11.md | 2026-06-11 | LIVE (Canonical spec/plan/decision) |
| TILES_PRO_GEN_LOG_S6.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| track_b_cliff_decor_cleanup_task.md | 2026-05-27 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| UI_HUD_ASSET_CONSULT_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| UI_HUD_PACK_GEN_BATCH1_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| UI_HUD_PACK_GEN_BATCH2_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| UI_REDESIGN_3SCREENS_BRIEF_2026-06-04b.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| UI_REDESIGN_3SCREENS_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| UI_REDESIGN_BRIEF_2026-06-04.md | 2026-06-04 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| UI_REDESIGN_SCREENS_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| UI_UX_REDESIGN_DECISION_2026-06-04.md | 2026-06-04 | LIVE (Canonical spec/plan/decision) |
| UNIFIED_DESIGNER_ARCHITECTURE_LOCK_S6.md | 2026-06-01 | LIVE (Canonical spec/plan/decision) |
| UNIFIED_DESIGNER_TASK_S6.md | 2026-05-31 | LIVE (Canonical spec/plan/decision) |
| UNIFIED_PAINTER_PLAN.md | 2026-05-27 | LIVE (Canonical spec/plan/decision) |
| UX_INPUT_agy.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| UX_INPUT_codex.md | 2026-05-29 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| UXFLOW_DECISION_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| V2_PLAN_DECISION_2026-06-06.md | 2026-06-06 | LIVE (Canonical spec/plan/decision) |
| VFX_STRATEGY_CONSULT_AX.md | 2026-05-30 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| VFX_STRATEGY_CONSULT_CX.md | 2026-05-30 | BAYAT (Analiz/Konsultasyon dosyasi, _process/ altina tasinmali.) |
| VFX_STRATEGY_SYNTHESIS_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| VIDEO_ANALYSIS_J1TK8oOg6_U_2026-06-03.md | 2026-06-03 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| VISUAL_MASTER_PLAN_2026-06-11.md | 2026-06-11 | LIVE (Canonical spec/plan/decision) |
| VOID_BACKGROUND_DEPTH_SPEC_2026-06-07.md | 2026-06-07 | LIVE (Canonical spec/plan/decision) |
| WALLRUN_CODE_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| WALLRUN_IMAGEGEN_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| walls_high_topdown_34_qc.md | 2026-05-24 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| WAVE2_CROSSCLASS_ECHO_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| WAVE2_FINISH_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| WAVE3_GATE_HUD_REVIEW_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| WEAPON_ANIM_VFX_PRODUCTION_LOCK.md | 2026-05-28 | LIVE (Canonical spec/plan/decision) |
| WEAPON_BATCH_PLAN.md | 2026-05-29 | LIVE (Canonical spec/plan/decision) |
| WEAPON_HAND_ATTACH_RESEARCH_agy.task.md | 2026-05-28 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| WEAPON_HAND_ATTACH_RESEARCH_codex.task.md | 2026-05-28 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| WEAPON_HAND_SYNTHESIS_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| WEAPON_PIPELINE_AUDIT_2026-06-08.md | 2026-06-08 | LIVE (Canonical spec/plan/decision) |
| WEAPON_SUBBUILDS_DESIGN.md | 2026-05-31 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md | 2026-05-28 | LIVE (Canonical spec/plan/decision) |
| weaponless_anim_weapon_mount_plan_task.md | 2026-05-28 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| WORK_ORDER_24_48H_S6.md | 2026-05-30 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |
| x_posts_research_agy_2026_05_26.md | 2026-05-26 | BAYAT (Arastirma dosyasi, _process/ altina tasinmali.) |
| XPOST_PROJECTION_CHECK_AX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| XPOST_PROJECTION_CHECK_CX_TASK.md | 2026-05-31 | BAYAT (Şüpheli) (Gorev dosyasi, tamamlanmis veya eski bir task olabilir.) |
| yt_48C9hYoLMis_summary.md | 2026-06-01 | BAYAT (Şüpheli) (Isimlendirme kuralina uymayan dosya (LIVE spec veya karar degilse _process/'e gitmeli).) |

## F. ŞÜPHELİ/BAYAT TOP-10

| # | Dosya / Konu | Gerekçe |
|---|---|---|
| 1 | C:.../project_fakeiso_term_revoked_2026_05_22.md | Dosya adı ve içeriği açıkça iptal edilmiş bir kararı (fakeiso terimi) belirtmektedir. Temizlenmesi veya arşive kaldırılması gerekir. |
| 2 | C:.../project_karar_149_subroom_encounter_lock.md & MEMORY/project_karar_149_subroom_encounter_lock.md | Kullanıcı-seviyesi bellek ile repo-seviyesi bellek arasında birebir çakışan dosya. Tek bir canonical konuma çekilmelidir. |
| 3 | C:.../project_karar_150_act1_envanter_live.md & MEMORY/project_karar_150_act1_envanter_live.md | Kullanıcı-seviyesi bellek ile repo-seviyesi bellek arasında çakışan dosya. Aynı envanter verisinin iki kopyası mevcuttur. |
| 4 | C:.../project_resume_2026-06-11.md & MEMORY/project_resume_2026-06-11.md | Kullanıcı-seviyesi bellek ile repo-seviyesi bellek arasında çakışan oturum özeti dosyası. Tarihi geçtiği için bayattır. |
| 5 | C:.../project_subroom_canonical_tags_lock.md & MEMORY/project_subroom_canonical_tags_lock.md | Kullanıcı-seviyesi bellek ile repo-seviyesi bellek arasında çakışan dosya. Drift riskine karşın birleştirilmelidir. |
| 6 | C:.../project_wall_production_pipeline_s99_late.md & MEMORY/project_wall_production_pipeline_s99_late.md | Hem çakışan kopya barındırıyor hem de s108 sonrasında geçerli olan duvar kuralına göre eski/revoked durumdadir. |
| 7 | C:.../feedback_camera_lock_hd2d.md | HD2D dönemine ait kamera kilidi kuralını barındırıyor. Proje topdown-pivot kararı sonrası geçerliliğini yitirmiş bayat bir gözlemdir. |
| 8 | C:.../project_s106_overnight_session_2026_05_25.md | 20 günden eski ve sadece o geceki otonom koşunun detaylarını içeren, geçerliliğini yitirmiş point-in-time gözlemdir (stale_pit). |
| 9 | STAGING/ANALYZE_AX_CLEANUP_MAPDESIGNER_S6.md | S6 phase dönemine ait tamamlanmış analiz dosyası. Yeni kurallar gereği STAGING root'unda durmamali, _process/ altına taşınmalıdır. |
| 10 | STAGING/D5_5_cliff_2stage_separation_task.md | Tamamlanmış veya süresi geçmiş eski bir görev (task) dosyası. STAGING root'unda birikme yapmaktadır, _process/ altına gitmelidir. |

## G. İstatistik

- **.claude/ altındaki toplam dosya sayısı:** 48
- **Global skills sayısı:** 25
- **Global memory (user-level) dosya sayısı:** 315
- **Local memory (repo MEMORY/) dosya sayısı:** 82
- **Staging (üst-seviye) dosya sayısı:** 536
- **Toplam envanterlenen öğe sayısı:** 1006
