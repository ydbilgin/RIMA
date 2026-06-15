# CURRENT_STATUS

## ⏯️ RESUME (2026-06-15 — sunum ~20 Haz; demo BATCH-FIX DONE+dual-verified → SIRADAKİ: OBS prova + post-demo design (Claude 4.8-reset sonrası ~21s))

**⚠️ ROUTING:** Orchestrator=Opus 4.8 · execute=**crafter-sonnet (iyi-specli cerrahi) / Opus (yeni/tasarım)** + **zorunlu audit gate** (auditor-opus/cx, writer≠reviewer) · council=cx+ax 3.1 Pro+ax 3.5 Flash · E1-E8. HARD: aynı anda TEK Unity-süren ajan. cx profil: yasinderyabilgin→yekta(son). Routing revizyonu (2026-06-15): `STAGING/SONNET_EXECUTE_AUDIT_ROUTING_2026-06-15.md`.

**🔒 STATE:** Play=full-flow (`playModeStartScene=MainMenu`, dokunma/null bırakma). Commit öncesi pollution temizle. Play bitince STOP.

**🧭 STRATEJİ KİLİTLENDİ (2026-06-14, 4× council + 2×2 deney; detay decision dosyaları):**
- **Sunum tezi:** RIMA = "oyun değil **environment + ilk vertical slice**"; domain-specific (genel framework/engine DEME); eksen **%20 oyun / %60 mimari / %20 graphify-audit**; centerpiece=**Edit-to-Play video**. → `STAGING/PRESENTATION_VISION_DECISION_2026-06-14.md` · [[project-presentation-vision]]
- **Graphify config:** map=**AST-only** · bug-hunt=**deep+opus** cerrahi (≤25 dosya) · deep+sonnet elendi · global LLM=post-demo. → [[reference-graphify-config-policy]]
- **Graphify update:** on-demand **full rebuild** (`STAGING/_process/2026-06/graphify_fullmap/build_ast_map.py`), per-commit/incremental DEĞİL. Full map mevcut: `graphify_fullmap/graphify-out/` (6925 node; **god-node 6/10=editor → "environment" veri-kanıtı**). → [[reference-graphify-update-policy]]
- **Ponytail:** tam plugin SKIP; post-demo sadece /ponytail-review checklist. → `STAGING/ponytail_ADOPTION_DECISION_2026-06-14.md` · [[reference-ponytail-verdict]]
- **F2 tanısı (cerrahi graphify):** #1 `RewardPickup.DraftThenOpenExit` sessiz `ShowDraft` return huni (muhtemelen downstream semptom) · #2 Forge oda 4/8 early-return · #3 dep early-return · #4 Echo seç→bind(kart yok) · #5 Chest'te Echo case yok. → `STAGING/_process/2026-06/f2_echo_graphify_deepopus.json`

**🎯 SIRADAKİ = EXECUTION, golden-path-first** (3 advisor + orchestrator hemfikir; "tüm oyunu bug'sız yap" tuzağına düşme — sadece videodaki akış kusursuz olsun):
1. ✅ **Edit-to-Play storyboard LOCKED** (council cx+ax 3.1 Pro+ax Flash → `STAGING/EDIT_TO_PLAY_STORYBOARD_DECISION_2026-06-14.md`). 2:00-2:20; wow=F2 Build Mode toggle (bug-free); canlı tile-çizimi KESİLDİ; kart-pool-%100 wow ELENDİ; golden-path segmenti F2-fix'e BAĞLI (fallback=slayt).
2. ✅ **F2 ÇÖZÜLDÜ = golden-path GREEN, 0 kod fix** (2026-06-15, council cx+ax Flash + 2 canlı repro → `STAGING/F2_ROOTCAUSE_DECISION_2026-06-15.md`). Gerçek spawn reward + ForceCollect → 3 kart render; ilk oda depth=1 (Forge 4/8 altı). F2 = Forge/Echo için bilinen limitasyon, post-demo. **Tek açık caveat: literal G-tuşu+menzil gate ForceCollect'le bypass edildi → video OBS provası nihai teyit.**
3. ✅ **Golden-path segment verification TAMAM** (council cx+ax_flash → `STAGING/GOLDEN_PATH_VERIFICATION_DECISION_2026-06-15.md`): **Build Mode F2-toggle+placement BUG-FREE** (data-proof: prop persist+oyun devam, 0 error) · F1 code-confirmed non-issue · **stat→damage math empirik DOĞRULANDI** (LMB base100→phys50=50/phys250=250 lineer; Q/E/R/F bypassStatScaling=stat-deaf → koreografi SADECE LMB) · Telemetry code-confirmed wired. **Video ANA tezi (edit-to-play + stat→damage) tam doğrulandı.**
4. ✅ **DEMO BATCH-FIX DONE + DUAL-VERIFIED (2026-06-15).** 6 cerrahi fix uygulandı (crafter-sonnet, yeni Sonnet-execute+audit kuralı) + **iki bağımsız cross-model verify** (cx + ax_opus): FIX-1/2/3/5 **PASS** · FIX-4 **waived** (fonksiyonel doğru, IsBuildModeActive-sonrası guard amacı karşılıyor; literal-placement nitpick) · FIX-6 **patched** (cx 3-çıkış null'lama) + 4.8-teyit. Compile **0-error**; YAPMA-listesi temiz (timescale/GameTimeCoordinator/draft-serialization/BuildMode-FSM/RewardPickup/Director-bootstrap dokunulmadı). Runtime smoke ertelendi → **OBS prova** (savunmacı guard/null = düşük risk). Detay → `STAGING/_process/2026-06/laureth_handoff/VERIFY_{cx,axopus}.md` + `BATCHFIX_RESULT.md`. ⚠️ timescale-patch(RIMA-001) = POST-DEMO (en-riskli, choreograph kapsıyor).
5. ⚠️ **Director overlay bleed FIX UYGULANDI, RUNTIME-VERIFY EDİLMEDİ** (DirectorMode.cs Awake + SetState-else `SetOverlayVisible(false)`; compile temiz; uncommitted). Batch-fix'le birlikte runtime test et. Trigger-map → `STAGING/DIRECTOR_MODE_TRIGGERS_2026-06-15.md`.
6. **DEMO SETUP (kritik, kod değil):** dev-direct `_Arena` koş (full-flow MainMenu → Director/BuildMode HİÇ kurulmaz, F2/backquote ÖLÜ — RIMA-002). maxHP slider'a dokunma (HP görsel yalan). draft/menu açıkken F2 yok. F12 panic hazır.
7. **Video kaydı** (OBS 10× prova) — kullanıcı işi; batch-fix + runtime-verify sonrası. Film-proof'lar orada: G-collect + canlı stat→damage + telemetry.
Golden-path dışı her şey = "bilinen limitasyon", post-demo.

**🔴 DEMO POLISH BACKLOG** (golden-path filtresinden geçir — sadece videodaki akışı bozanı ez):
- **F1** reward room-leak (oda geçişinde önceki ödül kalıyor→despawn) · ~~**F2** reward al→kart çıkmıyor~~ ✅ ÇÖZÜLDÜ (golden-path GREEN, 0 fix — F2_ROOTCAUSE_DECISION).
- **J1** reward slow-mo juice · **U1** tooltip dikey-şerit→kart (`TooltipSystem` preferredWidth/ContentSizeFitter) · **U2** Codex scroll · **U3** kaynak barları+sayı · **U4** "ODA TEMİZLENDİ" ortala.
- **A1** arena görseli (canon-revizyon → ÖNCE netleştir) · **P9** hoca raporu docx (EN SON; **graphify figürleri HAZIR**: `STAGING/report/graphify/` native HTML+screenshot+Obsidian vault).
- ⚠️ Overlay UI MCP screenshot'ta ÇIKMAZ → her UI fix sonrası kullanıcıdan görsel teyit ŞART.

**🗺️ RUN-MAP TASARIM KARARI (yeni session — council'le bak):** RIMA-özgü run-map (StS-tarzı dallı node haritası; kullanıcı referans görseli paylaştı: "Act 1 - Floor 0", HP bar, chest/combat/elite/boss/shop/event node'ları, dallanan kesikli yollar, RIMA bölgeleri Spacial Cliffs/Steel Aegis Cave/Mystifying Forest + RIMA element sembolleri). **HER RUN procedural DEĞİŞİR.** ⭐ **AÇIK KARAR:** oyuncu **tüm haritayı baştan TÜMÜYLE mi görür** (StS-tarzı tam stratejik planlama) yoksa **aşamalı/fog-of-war reveal mı?** ("full-run planı yapmadan ilerle" fikriyle düşünülecek). Mevcut iş: [[project-runmap-ui-asset-pipeline-2026-06-11]] (chrome=ChatGPT · sembol=PixelLab · çizgi=Unity; wiring bloker=enum köprüsü). Council değerlendirip karar verecek, sonra ilerlenecek.

**🟠 DEFERRED (post-demo):** skill-slot other-class host (ResolvePrimarySlotHost) · chest room-depth gating · tooltip→SO encapsulation · Elementalist/enemy clip GUID onarımı (keeper'lar o zaman no-op) · ponytail /ponytail-review checklist · `Assets/Scripts/graphify-out/` (624 cache Unity ağacında, gitignored/regenerable → sil/taşı) · hidden `.cx_dispatch`/`.ax_dispatch` done-file birikmesi (minör).

**🧹 2026-06-15 SESSION-2 ÖZET (F2 → verification → demo bug-sweep):**
- F2 GREEN (0 fix) · Build Mode centerpiece BUG-FREE + stat→damage math doğrulandı (data-proof, `*ForValidation`) · F1 code-safe · Director overlay bleed FIX (uncommitted, verify-pending).
- **Demo bug-sweep:** council 4-lens (cx+ax_pro+ax_flash+yapısal graphify) + **ChatGPT bağımsız review** (13dk, repo+kondanse-subgraph) → **6-fix batch SPEC + scope-kilit (NO refactor)**. ChatGPT paketi `STAGING/_process/2026-06/chatgpt_review/`; subgraph `STAGING/graphify_demo_subgraph_2026-06-15.md`; ChatGPT-review-prompt `STAGING/CHATGPT_REVIEW_PROMPT_2026-06-15.md`.
- ⚠️ **graph.json gitignored (10MB) — repo'da YOK; GitHub origin 8 commit GERİDE (`65c44d1a`) ama demo KOD dosyaları aynı (push gerekmez ChatGPT için).**
- ⚠️ **WORKING TREE COMMIT BEKLİYOR (BÜYÜK):** DirectorMode.cs fix + ~25 yeni STAGING doc (F2/verification/bugsweep/chatgpt-eval decision+council) + memory. Yeni session başında commit'le.
- *(Session-1 housekeeping — agent base+mixin, 3 HARD RULE graphify-query-first, graphify figürleri, storyboard, PixelLab mail — commit'lendi: `c9a72f33`+`a281329c`.)*

---
*Önceki bloklar git history'de: P1-P7.5c (skill/tooltip/arc/weapon/invisible-sprite) + facing/render batch DONE+pushed (`938e8da9`); demo E2E 10/10 + 9/9 sistem.*
