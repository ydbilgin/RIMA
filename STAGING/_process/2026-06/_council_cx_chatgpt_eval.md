ACTIVE RULES: (1) think (2) min/no-speculation (3) surgical (4) BLOCKED if unclear.
GRAPHIFY: cross-file soruda graph.json (STAGING/_process/2026-06/graphify_fullmap/graphify-out/), ~71× ucuz.

# Amaç
ChatGPT review'unu CODE-VERIFY + scope feasibility (cx lens). ANALYSIS ONLY.

OKU: STAGING/_process/2026-06/_council_chatgpt_eval_2026-06-15.md (tam brief + Q1-Q3 ŞART) + ChatGPT paketi (STAGING/_process/2026-06/chatgpt_review/.../03_DETAILED_FINDINGS.md, 04_RECOMMENDED_ARCHITECTURE.md) + kod (DirectorMode.cs, DraftManager.cs, RewardPickup.cs, BuildModeController.cs, RoomRunDirector.cs).

## CX LENS
**Q1:** ChatGPT'nin YENİ bulguları 003 (Build Mode aktif modal'ı sadece gizler, kapatmaz), 006 (RewardPickup 90s guard HideDraft çağırmaz, RewardPickup.cs:186-193), 007 (BeginRun opening-draft coroutine'i retain etmez restart'ta sağ kalır), 008 (build hotkey search-input focus'ta tetiklenir) — GERÇEK mi? file:line ile teyit/red. SUSPECTED'ler (002/004/007) scripted demo akışında tetiklenir mi?
**Q2:** ChatGPT'nin "minimum-risk demo patch"i (UIManager.ApplyTimeScale merkezi + RefreshTimeScale + resume-by-assignment kaldır) feasible mi, kaç dosya/satır? Full GameTimeCoordinator refactor'a göre risk?
**Q3:** Demo öncesi GERÇEKTEN yapılacak cerrahi fix listesi (golden-path/centerpiece önce), her biri dosya:line + yap/erteleme.

Çıktı: CODEX_DONE.md, ≤ kısa, file:line.
