# COUNCIL: ChatGPT review'unu DEĞERLENDİR + scope kararı + final batch-fix

ACTIVE RULES: (1) think (2) min/no-speculation (3) evidence file:line (4) BLOCKED if unclear.
GRAPHIFY: cross-file soruda graph.json (STAGING/_process/2026-06/graphify_fullmap/graphify-out/), ~71× ucuz.

## AMAÇ
ChatGPT (13dk düşünme, repo+graphify-subgraph ile bağımsız) demo-kritik kod review'u + tam fix-paketi üretti. Council bunu DEĞERLENDİRECEK. Sunum ~20 Haz (5 gün). Golden-path + Build Mode centerpiece + stat→damage ZATEN çalışıyor/kanıtlı. Demo REHEARSED (10× prova, scripted) + `_Arena`-direct koşulacak (full-flow değil).

## OKU
1. ChatGPT paketi: STAGING/_process/2026-06/chatgpt_review/RIMA_CLAUDE_CODE_REVIEW_PACKAGE_2026-06-15/ → 02_EXECUTIVE_SUMMARY, 03_DETAILED_FINDINGS, 04_RECOMMENDED_ARCHITECTURE, 09_FINDINGS_INDEX.json
2. Council önceki sentez: STAGING/DEMO_BUGSWEEP_SYNTHESIS_2026-06-15.md
3. İlgili kod (doğrulama için): DirectorMode.cs · DraftManager.cs · RewardPickup.cs · BuildModeController.cs · RoomRunDirector.cs

## YAKINSAMA (orchestrator ön-analizi)
ChatGPT + council GÜÇLÜ yakınsıyor: RIMA-001 (timescale ownership), 002 (Director full-flow bootstrap), 004 (draft serileştirme), 005 (lambda leak — 4/4 reviewer), 009 (kamera drift). RIMA-011 (/100f doğru) = orchestrator unit-test'iyle kanıtlı non-issue. YENİ ChatGPT bulguları: 003 (Build Mode aktif modal'ı gizler), 006 (reward 90s-guard HideDraft çağırmaz), 007 (opening-draft coroutine restart'ta sağ kalır), 008 (build hotkey text-input'ta tetiklenir).

## SORULAR (her advisor AYRI)
**Q1 (yeni bulgu doğrulama):** ChatGPT'nin council'in kaçırdığı 003/006/007/008'i GERÇEK mi? file:line ile teyit/red. SUSPECTED olanlar (002/004/007) demo'da GERÇEKTEN tetiklenir mi?

**Q2 (SCOPE — EN KRİTİK):** ChatGPT'nin önerdiği BÜYÜK refactor (GameTimeCoordinator arbiter + draft-serialization sistemi + BuildMode 4-state machine) — 5 gün önce, çalışan golden-path/centerpiece varken **DOĞRU mu yoksa OVER-ENGINEERING mi?** Üç seçenek değerlendir: (a) full-refactor (b) ChatGPT'nin "minimum-risk demo patch"i (c) choreograph-around + sadece ucuz-kesin fix'ler + F12 panic. Hangisi June 20 için optimal? Gerekçe.

**Q3 (final batch-fix listesi):** Yukarıdaki scope kararına göre, demo öncesi GERÇEKTEN yapılması gereken cerrahi fix'leri sırala (golden-path/centerpiece-relevant ÖNCE). Her biri: [yap/erteleme] + dosya:line + 1-cümle. Refactor gerektirenleri "post-demo" işaretle.

Çıktı: ≤ kısa, Q1-Q3, evidence file:line. Over-engineering bulgusu DEME.
