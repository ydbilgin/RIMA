# Codex Task — Sub-Room Encounter System Review

ACTIVE RULES: (1) think before deciding (2) honest verdict (3) cross-system impact analysis (4) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission

User proposed new design: **Combat Encounter = 4-5 connected sub-room sequence** instead of current single arena room. Fade-to-black transitions, reward at encounter end, mob distributed across sub-rooms.

Full proposal in `memory/project_subroom_encounter_system_proposal.md` and inline below.

User asked Codex to review honestly. This becomes Karar #149 candidate if approved.

## Proposal (verbatim user words + orchestrator synthesis)

User: "böyle bi dungeon görünümü oda oda geçişler olacaksa da olur bazı kapılar aynı room içinde devam eden odalar olabilir mesela. onu da hafif karartıp yeni ekrana geçip devam edebiliriz. ödülsüz olarak anladın mı? yani bu şekilde aslında room templatelerini yaparız ve mesela mob room aslında 4-5 tane template küçük roomdan oluşuyor gibi bu fikir doğru mu sence?"

Translation: "Dungeon-style room-to-room transitions OK. Some doors lead to continuation rooms within the same logical room. Slight darken + screen swap. NO reward at sub-room level. So Combat Room template = actually 4-5 small sub-room templates. Is this idea correct?"

## Orchestrator (Opus) initial take — provided to Codex for verification

- Industry precedent: Dead Cells biome model, Hollow Knight area transitions
- Mevcut RIMA architecture supports it (RoomTemplateSO + Multi-Layer Painter + DungeonGraph all LIVE)
- ~1 week MVP implementation
- Solves "single big room hard to compose" problem
- Improves combat pacing (Hades arena boredom fix)
- Integrates with Death Imprint Cascade (which sub-room you died in matters)

## Questions for Codex review

### 1. Architectural feasibility — verify

Read these files briefly:
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs`
- `Assets/Scripts/Runtime/Rooms/GateSocket.cs`
- `Assets/Scripts/Core/DoorTrigger.cs`
- `Assets/Scripts/MapDesigner/Blueprint/*` (high level)

Verify orchestrator's claim that mevcut architecture supports sub-room sequence. What ACTUALLY needs to be added vs what works as-is?

### 2. Combat design integration

NLM query:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "Combat Room mob spawn distribution, wave system vs pre-placed, reward placement at room end, Karar #80 room mechanics"
```

Sub-room sequence ile uyumlu mu? Wave system zaten varsa onun yerine geçer mi? Reward placement Karar'larıyla çakışıyor mu?

### 3. Procedural map generation impact

- DungeonGraph şu an oda-bazlı graph oluşturuyor mu, yoksa "encounter"-level mi?
- 4-5 sub-room sequence = 1 encounter ise, map generator buna nasıl uyum sağlar?
- Şu anki Faz 1 dungeon generation invasive değişiklik mi gerektirir?

### 4. Mob/reward design impact

- Her sub-room'da farklı mob set mantıklı mı, yoksa balance cehennemi mi?
- Reward son sub-room'da olunca player "fast-clear" stratejisi yapar mı?
- Death Imprint Cascade ile etkileşim — sub-room granularity gerçekten signature güçlendiriyor mu?

### 5. 3 user-open question

User üç soruyu açık bıraktı:
1. **Sub-room size:** 12x8 (intimate) vs 16x10 (current Spawn)?
2. **Transition type:** Fade-to-black (Dead Cells fast) vs Door + camera pan (Hollow Knight) vs Seamless no-transition (Hades atmospheric)?
3. **Mob spawn timing:** Pre-distributed across sub-rooms vs enter-trigger spawn vs hybrid?

Her biri için Codex önerini ver, justification ile.

### 6. Production cost honest re-estimate

User cost estimate orchestrator verdi: **~1 hafta MVP**. Codex realistic mi?

Specifically:
- Camera fade shader: actual hours?
- EncounterTemplateSO architecture: simple wrapper veya kompleks state machine?
- 5 sub-room template manual compose: aesthetic discipline gerek, kaç saat?
- Playtest validation: 1 gün gerçekçi mi?

### 7. Risks + blockers

Mevcut RIMA architecture'da gizli blocker var mı?
- Save/load mid-encounter (player sub-room 3'tede quit ederse?)
- Camera tilt + fade interaction (Branch E tilt 6° + transition shader)
- Pixel Perfect Camera compatibility
- 2D Lighting transition (Light2D state per sub-room)

## Required output

`STAGING/CODEX_DONE_subroom_encounter_review.md`:

```
# FINAL VERDICT
[Single paragraph: APPROVE / APPROVE_WITH_REVISIONS / REJECT]

# 1. Architectural feasibility
[What's truly supported vs what's missing]

# 2. Combat design integration
[Conflicts with existing wave system / reward placement / Karar 80]

# 3. Procedural map impact
[Major refactor needed or incremental?]

# 4. Mob/reward design
[Sound or balance hell?]

# 5. 3 open questions — Codex recommendations
[Sub-room size, transition type, mob spawn timing]

# 6. Production cost re-estimate
[Realistic hours per task, total]

# 7. Risks + blockers
[Hidden gotchas]

# 8. Karar #149 draft
[Proposed Karar text if APPROVE]

# 9. Conflict check
[Karar #25, #27, #80, #143, #147 — all preserved?]

# 10. Morning action item
[Exact next step orchestrator should do]
```

Effort: high. ~25-30 min. This is foundational design decision for next 6 months of room work. Quality > speed.

Hard rules:
- DO read actual code files (don't assume orchestrator's claim)
- DO query NLM for combat design canonical
- DO be honest if it's bad idea
- DON'T propose more agent dispatches (orchestrator's call)
