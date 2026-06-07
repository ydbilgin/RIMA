ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA oynanabilir demo mimarisi + sıralamasını FEASIBILITY / REUSE / EFFORT lensinden değerlendir — ANALYSIS ONLY, kod yazma.

## OKU (PATH ile, kendin oku — inline yok)
- `STAGING/_council_demo_brief.md` (TAM bağlam + mevcut sistemler + fork + 6 sub-question — ÖNCE BUNU OKU)
- `STAGING/PORTAL_PREVIEW_SYSTEM_SPEC_S6.md` (preview-island spec)
- `STAGING/ROOM_SYSTEM_DECISION_2026-06-03.md` (Part 3 = RoomRunDirector planı)
- `Assets/Scripts/Systems/Map/MapFlowManager.cs`, `Assets/Scripts/Systems/Map/RoomLoader.cs`
- `Assets/Scripts/Encounter/EncounterController.cs`
- `Assets/Scripts/Skills/SkillOfferGenerator.cs`, `Assets/Scripts/UI/SkillBarUI.cs`
- `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs`

## Lens: FEASIBILITY / REUSE / EFFORT
Brief'teki 6 sub-question'ı yanıtla. Senin özel odağın:
- Her demo-parçası için: ZATEN VAR mı / küçük-wire mi / sıfırdan-build mi? Effort tahmini (S/M/L).
- **Fork A vs B:** Hangisi DAHA AZ yeni kod? Path B'de RoomRunDirector + reward/wave/door'u _Arena'ya taşımak ne kadar iş — yoksa mevcut MapFlowManager+scene loop'u Path A'da korumak daha mı ucuz? Preview-adaları için A'da thumbnail-bake vs B'de mini-IsoRoomBuilder hangisi daha az kod?
- **En riskli/uzun parça** hangisi, demo'dan ÇIKARILMALI mı (defer)?
- Reuse fırsatları: `RoomBankSO.Pick`, `FanLayoutSolver`, `RoomTypeData.PickPortalCount`, `GateBehavior` typed-sprite — bunlar preview/typed-path'i ne kadar hazır kılıyor?

Sonucu CODEX_DONE.md'ye yaz. Her sub-question için somut öneri + effort. Prior audit'i tekrarlama.
