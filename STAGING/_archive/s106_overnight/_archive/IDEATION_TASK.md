ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Otherwise direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / STAGING/s106_overnight/MASTER_CONTEXT.md / Assets/Scripts/Runtime/Walls/V2/**/*.cs / Assets/Scripts/Editor/Walls/V2/**/*.cs / STAGING/concepts/chatgpt_ref/**

Amaç: RIMA S106 overnight pipeline için optimum implementation sırası + risk listesi + concrete recommendations çıkar. User uyuyor, orchestrator (Opus) self-approve mode'da çalışıyor. Senin çıktın MASTER_PLAN.md sentezine girecek.

---

# IDEATION TASK — Pipeline Design for Logic-First Room Builder

## Background (must read first)

**MANDATORY:** Read `STAGING/s106_overnight/MASTER_CONTEXT.md` IN FULL. It contains the user's verbatim spec, asset inventory, V2 code map, hard rules, and the multi-agent review pattern.

## Your job

You are one of TWO independent strategists (the other is Antigravity if you're Codex, or Codex if you're Antigravity). The orchestrator will read both responses and reconcile.

Output an actionable plan that answers these questions concretely. Be specific — file paths, line numbers, concrete trade-offs. NO fluff.

### 1. PIPELINE ORDER
Given the 5 streams in MASTER_CONTEXT.md (A Master+Asset / B Taxonomy / C Validation+Prefab / D Painter UX / E Test Rooms), what's the OPTIMAL sequence considering:
- Dependencies between streams
- Risk-adjusted parallelism (which can run concurrently?)
- User's #1 priority is Painter UX (Stream D)
- 6.5-hour budget overnight

Propose a Gantt-style ordering with concrete start/end estimates.

### 2. RISKS THE CURRENT PLAN MISSES
Read MASTER_CONTEXT.md hard rules + the proposed streams. What goes wrong overnight that the orchestrator hasn't accounted for? Examples to consider (not exhaustive):
- AssetDatabase batching failure scenarios
- WallChainRoomBuilder algorithm edge cases blueprint_room methodology demands
- Painter tool refactor backward compat (existing room layouts)
- Sub-agent prompt drift (forgetting MASTER_CONTEXT after 30+ min)
- File-write race conditions across parallel dispatches

### 3. SPECIFIC TECHNICAL FINDINGS
Read these files and identify concrete gaps vs blueprint_room methodology:
- `Assets/Scripts/Runtime/Walls/V2/WallChainRoomBuilder.cs` — does the 12-step builder flow map cleanly? Which steps are missing or buggy beyond S105 fixes?
- `Assets/Scripts/Runtime/Walls/V2/RoomSpec.cs` — does the schema support all 5 test rooms (Combat/Ritual/Flooded/Library/Boss)?
- `Assets/Scripts/Editor/Walls/V2/RoomPainterWindow.cs` — list every UX paper-cut. What would make this "world's easiest"? What features are required for blueprint-to-room workflow user described?
- `Assets/Sprites/AssetPackV3/walls/sheet_{2,3,4}/` — based on file names, can you guess the A-H group taxonomy? What needs human/AI inspection?

### 4. WHAT THE BLUEPRINT_ROOM METHODOLOGY DEMANDS THAT V2 DOESN'T YET SUPPORT
ADIM 4 (Library/Alcove) and ADIM 5 (Flooded) show specific construction sequences. Read the user's spec in MASTER_CONTEXT. Which blueprint_room flow steps does V2 ALREADY handle, and which need new code?

### 5. PAINTER TOOL UX RECOMMENDATIONS
User wants: "Ben oraya blueprint çizeyim otomatik duvarları entegre etsin oraya bunu istiyorum ve saçma sapan da etmesin birbirine bağlayacak şekilde. 2dboxları ayarlı şekilde olacak."

Translate to concrete features. Prioritize. What's mandatory v. nice-to-have?

### 6. AGENT ASSIGNMENT REFINEMENT
The orchestrator's draft (in MASTER_CONTEXT § Multi-Agent Review Loop) assigns implementers and reviewers. Suggest refinements based on:
- Agent strengths (Codex = code, Antigravity = vision+research, etc.)
- Risk distribution (avoid putting all critical-path tasks on one agent)
- Validation independence (no one reviews their own work)

### 7. ANYTHING THE ORCHESTRATOR HASN'T THOUGHT OF
Free-form. What's missing? What's wrong? What edge case nobody considered?

---

## Output format

**ANTIGRAVITY CRITICAL:** Do NOT use any tool to write the response to a file. Respond INLINE only — emit the markdown directly as your reply text. The orchestrator's `agy_dispatch.py` captures stdout via ConPTY, so any file-write through your scratch sandbox will NOT be picked up. Type the report directly in your response window.

**CODEX:** Write your response as markdown to the DONE file the dispatcher provided (`CODEX_DONE_<profile>.md`). The cx_dispatch wrapper handles this automatically.

Structure (both implementers):

```
# Ideation Response — <Codex profile / Antigravity account> — 2026-05-25

## 1. Pipeline Order
<your Gantt + reasoning>

## 2. Risks Missed
<bullet list, each with mitigation>

## 3. Technical Findings
### WallChainRoomBuilder.cs
### RoomSpec.cs
### RoomPainterWindow.cs
### Asset sheets

## 4. Blueprint_room Gaps in V2
<list>

## 5. Painter UX Mandatory Features
<prioritized list>

## 6. Agent Assignment Refinements
<table or list>

## 7. Anything Else
<free form>
```

**Length:** 1200-2500 words. Comprehensive but no fluff.

**Quality bar:** Senior engineer level analysis. Specific files, line numbers, concrete trade-offs. Not "we should consider X" — "in WallChainRoomBuilder.cs line 245 the corner derivation uses Y, but blueprint_room ADIM 4 step 4 demands Z, so we need to add Z behavior gated by the alcove spec".

If anything is unclear or under-spec'd → write BLOCKED with the specific question. Don't guess.
