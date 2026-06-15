ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekirse `NB=$(cat .claude/nlm.local 2>/dev/null); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`.
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/).

# Amaç
F2 sonrası golden-path sıradaki iş prioritizasyonu — codebase/feasibility lens. ANALYSIS ONLY, kod değiştirme.

## OKU
1. STAGING/_process/2026-06/_council_next_priority_2026-06-15.md  ← tam brief + sorular (ŞART)
2. STAGING/F2_ROOTCAUSE_DECISION_2026-06-15.md (F2 sonucu)
3. Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs (F1: AdvanceTo/BuildCurrentRoom/DestroyActiveReward)
4. Assets/Scripts/UI/BuildModeController.cs + Assets/Scripts/UI/BuildMode/BuildPlacementController.cs (centerpiece F2-toggle)
5. Assets/Scripts/UI/DirectorMode.cs (stat→damage + telemetry segment)

## CX LENS — codebase facts (file:line ZORUNLU)
**Q1 (F1 golden-path):** Golden-path oda-geçişi F1 leak tetikler mi yoksa BuildCurrentRoom→DestroyActiveReward güvenli path mi? F1 golden-path-non-issue mi (F2 gibi)?
**Q2 (centerpiece):** BuildModeController F2-toggle (gir/çık, prop yerleştir, aynı odada devam, working-copy restore) kod-seviyesinde sağlam mı? Bilinen risk/eksik?
**Q3 (prioritize):** 20 Haz öncesi en yüksek-kaldıraç otonom iş sırası (Build Mode doğrula / F1 teyit / DirectorMode stat→damage / Telemetry CSV / UI kod-tarafı). Hangi segmentlerde kod-seviyesi risk var?
**Q4 (skip):** Ne yapılmamalı (over-engineering)?

Çıktı: CODEX_DONE.md, ≤ kısa, her iddia file:line. Önceki audit'i tekrar üretme.
