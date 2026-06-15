# CURRENT_STATUS

## ⏯️ RESUME (2026-06-15 — sunum ~20 Haz; tooling/housekeeping session bitti → EXECUTION'a DÖN)

**⚠️ ROUTING:** Orchestrator=Opus 4.8 · execute=Opus sub-agent · review=auditor-opus/cx (writer≠reviewer) · council=cx+ax 3.1 Pro+ax 3.5 Flash · E1-E8. HARD: aynı anda TEK Unity-süren ajan. cx profil: yasinderyabilgin→yekta(son).

**🔒 STATE:** Play=full-flow (`playModeStartScene=MainMenu`, dokunma/null bırakma). Commit öncesi pollution temizle. Play bitince STOP.

**🧭 STRATEJİ KİLİTLENDİ (2026-06-14, 4× council + 2×2 deney; detay decision dosyaları):**
- **Sunum tezi:** RIMA = "oyun değil **environment + ilk vertical slice**"; domain-specific (genel framework/engine DEME); eksen **%20 oyun / %60 mimari / %20 graphify-audit**; centerpiece=**Edit-to-Play video**. → `STAGING/PRESENTATION_VISION_DECISION_2026-06-14.md` · [[project-presentation-vision]]
- **Graphify config:** map=**AST-only** · bug-hunt=**deep+opus** cerrahi (≤25 dosya) · deep+sonnet elendi · global LLM=post-demo. → [[reference-graphify-config-policy]]
- **Graphify update:** on-demand **full rebuild** (`STAGING/_process/2026-06/graphify_fullmap/build_ast_map.py`), per-commit/incremental DEĞİL. Full map mevcut: `graphify_fullmap/graphify-out/` (6925 node; **god-node 6/10=editor → "environment" veri-kanıtı**). → [[reference-graphify-update-policy]]
- **Ponytail:** tam plugin SKIP; post-demo sadece /ponytail-review checklist. → `STAGING/ponytail_ADOPTION_DECISION_2026-06-14.md` · [[reference-ponytail-verdict]]
- **F2 tanısı (cerrahi graphify):** #1 `RewardPickup.DraftThenOpenExit` sessiz `ShowDraft` return huni (muhtemelen downstream semptom) · #2 Forge oda 4/8 early-return · #3 dep early-return · #4 Echo seç→bind(kart yok) · #5 Chest'te Echo case yok. → `STAGING/_process/2026-06/f2_echo_graphify_deepopus.json`

**🎯 SIRADAKİ = EXECUTION, golden-path-first** (3 advisor + orchestrator hemfikir; "tüm oyunu bug'sız yap" tuzağına düşme — sadece videodaki akış kusursuz olsun):
1. ✅ **Edit-to-Play storyboard LOCKED** (council cx+ax 3.1 Pro+ax Flash → `STAGING/EDIT_TO_PLAY_STORYBOARD_DECISION_2026-06-14.md`). 2:00-2:20; wow=F2 Build Mode toggle (bug-free); canlı tile-çizimi KESİLDİ; kart-pool-%100 wow ELENDİ; golden-path segmenti F2-fix'e BAĞLI (fallback=slayt).
2. **F2:** minimal repro (1 log/assertion ile gerçek tetikleyiciyi doğrula) → en küçük fix → playtest. 5'i KÖRLEMESİNE fixleme YOK. ⬅️ storyboard golden-path segmentinin ÖN-KOŞULU.
3. **Video kaydı** (OBS 10× prova, tek take) + doğrula.
Golden-path dışı her şey = "bilinen limitasyon", post-demo.

**🔴 DEMO POLISH BACKLOG** (golden-path filtresinden geçir — sadece videodaki akışı bozanı ez):
- **F1** reward room-leak (oda geçişinde önceki ödül kalıyor→despawn) · **F2** reward al→kart çıkmıyor (tanı yukarıda).
- **J1** reward slow-mo juice · **U1** tooltip dikey-şerit→kart (`TooltipSystem` preferredWidth/ContentSizeFitter) · **U2** Codex scroll · **U3** kaynak barları+sayı · **U4** "ODA TEMİZLENDİ" ortala.
- **A1** arena görseli (canon-revizyon → ÖNCE netleştir) · **P9** hoca raporu docx (EN SON; **graphify figürleri HAZIR**: `STAGING/report/graphify/` native HTML+screenshot+Obsidian vault).
- ⚠️ Overlay UI MCP screenshot'ta ÇIKMAZ → her UI fix sonrası kullanıcıdan görsel teyit ŞART.

**🟠 DEFERRED (post-demo):** skill-slot other-class host (ResolvePrimarySlotHost) · chest room-depth gating · tooltip→SO encapsulation · Elementalist/enemy clip GUID onarımı (keeper'lar o zaman no-op) · ponytail /ponytail-review checklist · `Assets/Scripts/graphify-out/` (624 cache Unity ağacında, gitignored/regenerable → sil/taşı) · hidden `.cx_dispatch`/`.ax_dispatch` done-file birikmesi (minör).

**🧹 2026-06-15 session (tooling/housekeeping — ⚠️ EXECUTION'a DOKUNULMADI, sıradaki o):**
- **Agent mimarisi base+mixin:** `rima-context` skill + 6 agent `skills:[rima-context]`; rima-codex leak fix (`.claude/_agent_backups/`); stale drift temizliği · **CODE_MAP.md** + PROJECT_INDEX link.
- **3 HARD RULE eklendi:** Unity-error-check (ilgili→çöz/ilgisiz→bildir) · STAGING non-md konvansiyon · **Graphify query-first** (cross-file soruda önce `graphify query`, ~71× ucuz; global CLAUDE.md+propagate / GEMINI.md / `.codex` / PROJECT_RULES / rima-context / AGENTS → [[graphify-query-first]]).
- **Graphify DOĞRU kullanım netleşti:** asıl değer=**sorgu katmanı (AI hafızası)**, görsel ikincil. Rapor/P9 figürleri HAZIR: `STAGING/report/graphify/` (native HTML dense/file-level/closeup + screenshot) + Obsidian vault 7043-not (regenerable, commit'leme).
- **STAGING 286→217** (69 non-md arşiv) + `/lint` hygiene check (Step 5b).
- **Edit-to-Play storyboard LOCKED** (council) → `STAGING/EDIT_TO_PLAY_STORYBOARD_DECISION_2026-06-14.md`.
- **PixelLab:** Discord izin maili council-onaylı (`STAGING/PIXELLAB_DISCORD_LEGAL_EMAIL_DRAFT_2026-06-15.md` — kullanıcı GÖNDERECEK, DM-first; contact `support@pixellab.ai`) · LaurethStudio 3 prompt (agent-arch / RIMA-briefing / graphify-adopt) `_process/`.
- ⚠️ **Working tree COMMIT BEKLİYOR** (graphify figür/script + email + laureth prompt + PROJECT_RULES/rima-context/AGENTS/CODE_MAP). 1 commit `c9a72f33` atıldı (storyboard+sweep+agent-arch); gerisi açık.

---
*Önceki bloklar git history'de: P1-P7.5c (skill/tooltip/arc/weapon/invisible-sprite) + facing/render batch DONE+pushed (`938e8da9`); demo E2E 10/10 + 9/9 sistem.*
