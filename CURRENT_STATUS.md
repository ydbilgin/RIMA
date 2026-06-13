# CURRENT_STATUS

## ⏯️ RESUME (2026-06-13 — sunum ~1 HAFTA sonra (~20 Haz), UNITY IN-EDITOR; yeni session devralıyor)

**⚠️ ROUTING:** Orchestrator=Opus 4.8 · execute=Claude Opus sub-agent (Agent model:opus / builder-opus; ax Opus DEĞİL) · review=auditor-opus veya cx (writer≠reviewer) · council=cx+ax 3.1 Pro+ax 3.5 Flash. Unity'ye dokunan her dispatch'e UNITY ERROR CHECK. E1-E8 aktif. **YENİ:** kota-aware routing → `python ~/.claude/quota.py --recommend` (Claude bol→execute Claude; detay memory `reference_quota_routing_tooling`). Yeni global agentlar: builder-opus(execute)/auditor-opus(review)/crafter-sonnet(light).

**🔴 İLK İŞ — arka planda koşan workflow:** `wj4doy5m6` (test-koşumlu editor consolidation) clear'dan SONRA bitebilir. Task-notification gelince: çıktısını oku → `STAGING/EDITOR_CONSOLIDATION_DECISION_2026-06-13.md`'ye göre doğrula → testler YEŞİL mi + audit verdict + **play-mode'da F2 ile aç, Save/grid/aydınlık data-proof** → temizse COMMIT. (Uncommitted kod olabilir: keybind fix + workflow'un düzenledikleri.) Workflow bitmemişse `/workflows` ile izle.

**🏗️ ANA İŞ = OYUN-İÇİ SEVİYE EDİTÖRÜ (Build Mode).** Kullanıcı: tek, GÜZEL (aydınlık, grid'li), entegre (kaydeden), genişleyebilen-map, TEST-korumalı editör; "tekrar bu sorunla karşılaşmayalım". İleride taşınabilir framework (LaurethStudio.LevelEditor).
- **Açılış tuşu = F2** (NOT `"`: kullanıcı Türkçe-Q klavyede, quoteKey tetiklenmiyor).
- **🔴 KRİTİK KEŞİF:** Projede ZATEN F2'ye bağlı eski IMGUI editör VARMIŞ → `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` (çirkin, ama kaydediyor + Editor `UnifiedDesignerCore` ile entegre). Benim uGUI Build Mode bundan habersiz kuruldu → F2 çakıştı. **Konsolidasyon = uGUI Build Mode tek editör, eski IMGUI F2 EMEKLİ. InPlayMapPaintOverlay'i YENİDEN KURMA.**
- **Garbled yazı kökü:** `Jersey10-Regular SDF` static + fallback listesi BOŞ + TMP Settings fallback boş → LiberationSans dynamic-fallback bağla (consolidation workflow yapıyor).
- **§3.5 TILE KANUNU:** grid=Isometric (fake-iso, floor451_0=64×64 kare, kamera düz). Yerleştirme HER ZAMAN Grid API (`WorldToCell`/`GetCellCenterWorld`/`SetTile`), ASLA dikdörtgen matematik.

**📐 FAZ PLANI:** ✅P1 (F2→kamera-zoom+pause+overlap-hide) · ✅P2 (prop palette+iso ghost+rotate+erase+undo, prop tam hücre merkezine oturuyor) · ✅P3 (tile/walkability/overlay brush, working-copy no-pollution) · ✅Phase-A (data-driven Asset Catalog 4 genişletilebilir kategori + premium uGUI Asset Browser) → **⏳ CONSOLIDATION (workflow `wj4doy5m6`: F2 key-guard+tek-sahip, IMGUI emekli, Save, TMP fix, palet-aydınlat, grid overlay, regresyon test suite)** → P4 (light+runtime save/load) · P5 (select/move/delete). **POST-DEMO:** UnifiedDesignerCore'a kademeli merge · `ResizePreserveCells` (büyük-genişleyen map; gerçek-sonsuz/chunk DEĞİL) · production static TMP atlas (tam Türkçe glyph) · package extraction (LaurethStudio.LevelEditor: ISpaceMapper/IAssetCatalog/IPlacementValidator/ILevelStore/IPlaceable +Render/Input/Host adapter).

**📄 KARAR DOKÜMANLARI (STAGING):** INGAME_BUILD_MODE_DESIGN · BUILDMODE_TERRAIN_DECISION (organik terrain=ATLA, demo) · LEVEL_EDITOR_FRAMEWORK_DECISION · LEVEL_EDITOR_UI_DESIGN (asset_strategy=procedural-only) · EDITOR_CONSOLIDATION_DECISION (hepsi 2026-06-13).

**✅ BU SESSION COMMIT'LERİ:** 70bf3ab7(lighting:intensity-0 global skip) · fbc21be9(P1) · bcf7b9a2(P2) · 8b995a49(terrain council) · c252898e(P3+UI overlap-hide+Act1 styling) · 355be485(framework council) · 3dbcdfba(Phase-A catalog+browser) · ac3c0fe9(consolidation council).

**🔁 KRONİK:** TMP/blueprint Play Mode'da kirleniyor → commit ÖNCESİ blueprint/.asset `git checkout --`. (TMP fallback kök fix consolidation'da; static-atlas tam fix post-demo.) Bu session play-mode'lar `.asset` KİRLETMEDİ (working-copy çalışıyor).

**⏳ DEFER (editör sonrası / demo-koruma kuyruğu, ikincil):** E2E playtest 10/10 PASS edildi (`_process/2026-06/_e2e_playtest_2026-06-13.md`) · Smoke test süiti (gece data-proof'ları→EditMode) · silah-ele-oturtma · NLM cleanup · GECE_RAPORU/hoca raporu güncelle · Ravager LMB · Weakened/Scorch çarpanı · mob CharacterJuice · memory index-dışı ~20 dosya · `quota.py` v2 codex-reaktif hook.

---
*Önceki bloklar git history'de. Demo 9/9 vaat çalışıyor (E2E-kanıtlı); editör = sunuma kadar ana geliştirme.*
