ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query <NLM_NOTEBOOK_ID> "<your question>"
  (NLM_NOTEBOOK_ID: NB=$(cat .claude/nlm.local 2>/dev/null) ile oku — repo'da gizli/gitignored)
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/), bulk-read'den ~71x ucuz.

# Amaç
F2 "reward→kart çıkmıyor" — codebase-grounded kök neden + gerçek-akış repro reçetesi (cx feasibility/reuse lens). ANALYSIS ONLY, kod değiştirme.

## OKU (READ these files first)
1. STAGING/_process/2026-06/_council_f2_rootcause_2026-06-15.md  ← brief + orchestrator'ın canlı repro bulguları (ŞART)
2. Assets/Scripts/Core/RewardPickup.cs
3. Assets/Scripts/Skills/DraftManager.cs
4. Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs
5. Assets/Scripts/UI/SkillOfferUI.cs
6. Assets/Scripts/Core/RuntimeRoomManager.cs

## ÖZET (brief'te tam hali var)
Orchestrator canlı repro ile KANITLADI: _Arena-direct play'de `DraftManager.Instance.ShowDraft()` doğrudan çağrılınca SkillOfferPanel canvas (ScreenSpaceOverlay, sort 1050) + 6 buton kuruluyor, IsDraftActive=True → **kart-render İZOLASYONDA ÇALIŞIYOR**. Ama o harness'ta `RuntimeRoomManager.Instance=NULL` → `GetLiveRoomDepth()`=`?? 1`=room 1. ELENDİ: #1 (Instance null), #3 (dep-null), render hatası.

## CX LENS SORULARI (feasibility / what-exists / reuse)
**Q1 (kök neden, cite'lı):** ShowDraft izolasyonda çalıştığına göre gerçek F2 kök nedeni en olası hangisi — (A) gerçek akışta `RuntimeRoomManager.CurrentRoom` Forge'a (4/8) çözülmesi → Forge early-return / (C) pickup-collect path (G tuşu/trigger/coroutine guard) / (D) flow-spesifik? file:line ile gerekçelendir.

**Q2 (KRİTİK — golden-path):** F2 NORMAL ilk combat odasını (depth 1-3, Echo değil) bozuyor mu, yoksa SADECE Forge(4/8)/Echo'yu mu? Kod kanıtıyla net söyle.

**Q3 (repro reçetesi — senin en güçlü olduğun yer):** `RuntimeRoomManager.CurrentRoom` (Core/RuntimeRoomManager.cs) NASIL artıyor (hangi event/çağrı)? Gerçek full-flow'da İLK reward-veren combat odasının CurrentRoom depth'i KAÇ olur? `RoomRunDirector.BeginRun` → ilk oda → reward zincirini cite'la. Reward G tuşuyla mı toplanıyor (RewardPickup.cs:15 InteractKey=G) yoksa otomatik (ForceCollect timeout) mu? _Arena full-flow'a Editor'dan hızlı ulaşmak için dev hook/debug kısayolu VAR MI (kod-tabanlı)?

**Q4 (en küçük fix):** Düzeltme gerekiyorsa EN KÜÇÜK cerrahi fix. 5 adayı körlemesine fixleme YOK.

Çıktı: CODEX_DONE.md'ye yaz, ≤ kısa, her iddia file:line. Önceki audit'i tekrar üretme. Format: Q1-Q4 başlıkları + net verdict.
