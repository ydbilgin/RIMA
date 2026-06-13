# CURRENT_STATUS

## ⏯️ RESUME (2026-06-13 — sunum ~20 Haz, UNITY IN-EDITOR)

**⚠️ ROUTING:** Orchestrator=Opus 4.8 · execute=Claude Opus sub-agent (Agent model:opus / builder-opus; ax Opus DEĞİL) · review=auditor-opus veya cx (writer≠reviewer) · council=cx+ax 3.1 Pro+ax 3.5 Flash · Unity'ye dokunan her dispatch'e UNITY ERROR CHECK · E1-E8 aktif · kota-aware: `python ~/.claude/quota.py --recommend`. Global agentlar: builder-opus(execute)/auditor-opus(review)/crafter-sonnet(light).

**✅ EDİTÖR KONSOLİDASYONU TAMAM — commit `9d47dc3c`.** Workflow `wf_0c13e81f-591` (6 agent: 3 contract→implementer→test→audit). Bağımsız doğrulama: **EditMode 25/25 + PlayMode 8/8 GREEN (re-run kanıtlı), 0 compile error, auditor APPROVE.** Tek editör = uGUI Build Mode; eski IMGUI `InPlayMapPaintOverlay` `RIMA_LEGACY_MAPPAINT` (OFF, define yok) arkasında EMEKLİ (dosya duruyor). F2 tek-sahip = yeni `InPlayToolKeyRegistry`. Save = CopySerialized working→source (editor-only). TMP fallback (LiberationSans dynamic) bağlandı → garbled Türkçe fix. Palet aydınlatıldı + world-space iso-grid overlay.

**🏗️ ANA İŞ = OYUN-İÇİ SEVİYE EDİTÖRÜ (Build Mode).** Açılış = **F2** (NOT `"`: Türkçe-Q klavye). Tek/güzel/entegre/genişleyebilen/test-korumalı editör; ileride taşınabilir framework (LaurethStudio.LevelEditor).
- **§3.5 TILE KANUNU:** grid=Isometric (fake-iso, floor 64×64 kare, kamera düz). Yerleştirme HER ZAMAN Grid API (`WorldToCell`/`GetCellCenterWorld`/`SetTile`), ASLA dikdörtgen matematik.

**📐 FAZ PLANI:** ✅P1 ✅P2 ✅P3 ✅Phase-A ✅CONSOLIDATION → **⏳ P4 (light + runtime save/load)** → P5 (select/move/delete). **POST-DEMO:** save-write-back live test (auditor MINOR-1) · UnifiedDesignerCore'a kademeli merge · `ResizePreserveCells`/Expand-Bounds (decision item 7) · production static TMP atlas (tam Türkçe glyph) · package extraction (ISpaceMapper/IAssetCatalog/IPlacementValidator/ILevelStore/IPlaceable +adapter).

**📄 KARAR DOKÜMANLARI (STAGING):** INGAME_BUILD_MODE_DESIGN · BUILDMODE_TERRAIN_DECISION · LEVEL_EDITOR_FRAMEWORK_DECISION · LEVEL_EDITOR_UI_DESIGN · EDITOR_CONSOLIDATION_DECISION (hepsi 2026-06-13).

**🔁 KRONİK:** PlayMode test/oyun → blueprint `.asset` + LiberationSans Fallback (dynamic atlas 9KB→550KB) + `Assets/InitTestScene*.unity` KİRLENİR → commit ÖNCESİ `git checkout --` blueprint+fallback, InitTestScene SİL (TMP wiring = sadece TMP Settings + Jersey10 küçük diff'i, fallback asset DEĞİL). Pre-existing junk: `InitTestSceneebd0160c…unity` (commit a754c640'ta yanlış eklenmiş) → post-demo sil.

**⏳ DEFER:** E2E playtest 10/10 PASS (`_process/2026-06/_e2e_playtest_2026-06-13.md`) · silah-ele-oturtma · NLM cleanup · GECE_RAPORU/hoca raporu güncelle · Ravager LMB · Weakened/Scorch çarpanı · mob CharacterJuice · memory index-dışı ~20 dosya · `quota.py` v2 codex-reaktif hook · auditor MINOR-2 (on-disk test XML).

---
*Önceki bloklar git history'de. Demo 9/9 vaat çalışıyor (E2E-kanıtlı); editör = sunuma kadar ana geliştirme.*
