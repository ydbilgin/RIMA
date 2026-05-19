# Codex Review Task — Karar #150 LIVE (Act-Aware Dungeon-Inside)

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

**NOTE about NLM staleness:** NLM hâlâ Karar #149 öncesi "1 oda = 1 arena wave-based" canonical'ını söylüyor. Karar #149 (S94 LIVE 2026-05-19) bunu sub-room sequence ile süpersede etti. Bu review'de Karar #149 LIVE'ı authoritative kabul et — local memory + MASTER_KARAR_BELGESI.md kontrol et.

---

## Görev

Karar #150 LIVE design doc'unu (`STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md`) review et. Bu doküman v4 image_gen PASS (`Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png`) sonrası ortaya çıkan **Act-Aware Dungeon-Inside Architecture** kararını lockluyor. 3 Act'e yayılan production roadmap'ı specifiy ediyor.

Bu DESIGN review — kod yazma. Verdict + revision önerileri ile döndür.

---

## Bağlam (önce oku)

1. **Bu görev hedefi:** `STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md` (review hedefi, tam doc)
2. **Concept reference v4 (PASS):** `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png` (1536×1024)
3. **Concept reference v3 (REJECTED):** `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v3_fakeiso.png`
4. **Karar #149 LIVE spec:** `STAGING/SUBROOM_ENCOUNTER_TECH_SPEC_OPUS.md` (Opus tech spec, if exists)
5. **Karar #149 review:** `STAGING/CODEX_DONE_subroom_encounter_review.md` (kendi önceki verdict'in)
6. **Roadmap discipline:** `STAGING/ROADMAP_dungeon_buildup.md` (6-faz piece-by-piece)
7. **Master karar belgesi:** `TASARIM/MASTER_KARAR_BELGESI.md` (Karar #143 #147 #148 #149 mevcut)
8. **Mevcut kod:**
   - `Assets/Scripts/Rima/MapDesigner/SO/RoomTemplateSO.cs`
   - `Assets/Scripts/Rima/MapDesigner/SO/EncounterTemplateSO.cs` (if exists, Karar #149 Codex Step 1)
   - `Assets/Scripts/Rima/MapDesigner/Runtime/SubRoomSequenceController.cs` (Karar #149 Codex Step 2, recently shipped)
   - `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs`
   - `Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs`

---

## Review boyutları

### A. Architecture feasibility (en kritik)

1. v4 "dungeon-inside" 32×22 sub-room size + internal-architecture-primary layout → mevcut `RoomTemplateSO` schema ile uyumlu mu? `RoomTemplateSO.bounds` 32×22 destekliyor mu?
2. 5 wall class + arch + pillar + collapsed_stub = 8 sınıf — `BrushAtlasImporter` + `AssetPoolSO` mevcut sistemi 8 class × 3 variant'ı clean handle ediyor mu? Yoksa schema değişikliği gerek mi?
3. Sub-room transition fade-to-black: `SubRoomSequenceController` (S94 LATE PASS) bu mechanic'i destekliyor mu? "Archway exit match" (sub-room N exit = sub-room N+1 entry mirror) için ek API gerek mi?
4. `MapLayerOrchestrator.Paint(RoomTemplateSO)` API'ı 6-layer pipeline'ı sub-room granular call edebiliyor mu, yoksa encounter-level batch mi? (Karar #149 review'de bunu flagged hatırlatıyorum.)

### B. Asset count realism

1. 110 asset per Act × 3 Act = 330 PixelLab gen. Mevcut bütçe 3500/5000 + ~25 image_gen $0 reserve. Math doğru mu?
2. Hidden iteration cost: regen rate %25-35 (Karar #143 NLM observed). 330 × 1.35 = ~445 effective gens. Hâlâ comfortable mı?
3. Cross-Act palette-swap (Act 2 bone-wrapped granite from Act 1 granite) → PixelLab `create_object` palette override destekliyor mu, yoksa regen mi şart?

### C. Sub-room connection design

1. "Sağ-alt archway → sol-üst archway mirror" rule sub-room template'lerde nasıl encode edilir? `DoorSocket` array + `mirrorPair` ID gerek mi?
2. Storytelling trail (debris breadcrumb sub-room N → N+1) implementation: L5 decal sequence olarak placement-mirror vs dedicated `EncounterTrailSO` system. Hangisi minimum code?
3. Vertical slice (Karar #149) = 2 sub-room. 32×22 + fade transition + archway mirror çalıştırılması için kaç ek file/SO?

### D. Layout grammar (5 sub-room slot types)

1. **Entry chamber / Pillar arena / Collapse corridor / Ritual hall / Crypt cell** — bu type'lar `RoomTemplateSO.roomType` enum'una eklenir mi, yoksa `EncounterTemplateSO.sequence[i].slotType` ayrı enum mu olur?
2. Karar #149 Codex Step 1 verdict'inde `EncounterTemplateSO` schema önerin neydi? Slot type field var mı? Yoksa naked `RoomTemplateSO[]` array mi?
3. Combat encounter "Entry → Pillar arena → Collapse corridor → Pillar arena (reward)" pattern data-driven mı yoksa hard-coded mı? Hangi tercih edilir?

### E. Roadmap × 3 Act sürdürülebilirlik

1. Faz 1-6 disiplini × 3 Act × 6 faz = 18 faz cycle. Production time estimate (1 faz ~1-3 saat orchestrator + 1-4 saat dispatch wait) → ~18-36 hour effective scope. Realistic mi MVP için?
2. Act 2 + Act 3 deferred (Act 1 LIVE sonrası) — `_Universal` package ne kadar Act 1 üretiminden çıkarılabilir? Aggressive universal kullanım önerin var mı?

### F. NLM sync gap

1. NLM canonical Karar #149 öncesi modeli söylüyor. Karar #150 doc + Karar #149 memory NLM'e push edilmeli — bu sync `/nlm-sync` ile mi yapılır, manuel mi?
2. `TASARIM/MASTER_KARAR_BELGESI.md` Karar #150 wording önerin nedir? (8. section'da "next action" yazan placeholder var.)

### G. Scope creep / drift risks

1. v4 PASS ama v5/v6 iteration baskısı doğabilir. Hangi kriterler "Faz 1 dispatch trigger" için yeterli? Yoksa daha çok concept iteration yapılmalı mı?
2. Per-Act 110 asset Karar #149 sub-room model + Karar #150 dungeon-inside ile ANCAK Act 1'i tamamen yapar mı? Act 2/3 için sub-room sayısı azaltılması gerek mi?

---

## Verdict format

```
VERDICT: APPROVE | APPROVE_WITH_REVISIONS | NEEDS_REWORK | REJECT

ARCHITECTURE FEASIBILITY: <PASS/CONCERN/BLOCKER>
- <evidence + reasoning>

ASSET ECONOMICS: <PASS/CONCERN/BLOCKER>
- <evidence>

SUB-ROOM CONNECTION: <PASS/CONCERN/BLOCKER>
- <evidence>

LAYOUT GRAMMAR: <PASS/CONCERN/BLOCKER>
- <evidence>

SUSTAINABILITY: <PASS/CONCERN/BLOCKER>
- <evidence>

REVISIONS (if APPROVE_WITH_REVISIONS):
1. <specific change to doc>
2. <specific change>

MASTER_KARAR_BELGESI wording önerin:
> Karar #150 (LIVE 2026-05-19 — ...): <full wording>

IMPLEMENTATION NEXT STEPS (priority order):
1. <SO/schema change needed>
2. <code file impact>
3. <Faz 1' dispatch hazır mı kontrol>
```

---

## Done report

Yaz to: `STAGING/CODEX_DONE_karar_150_review.md`
Then trigger `CODEX_DONE_<profile>.md` per standart workflow.

Effort: high (design review, judgment-heavy, no code).
