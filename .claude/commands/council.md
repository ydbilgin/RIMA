Multi-advisor synthesis for RIMA: consult **cx (Codex)** + **ax Gemini 3.1 Pro High** + **ax Gemini 3.5 Flash High**, then YOU (Opus, the orchestrator) synthesize a single final decision. Advisors inform; you decide.

`$ARGUMENTS` = the question/topic. It MAY include source file paths to analyze (e.g. "Mina mechanics: STAGING/mechanic_refs/Foo.md, STAGING/mechanic_refs/Bar.md — what applies to RIMA?"). If file paths are present, tell every advisor to READ them (do NOT inline large files — Codex and ax/Gemini both have file tools).

# Steps (you, the orchestrator)

## 0. Slug + brief
Pick a short slug from the topic. Decide 4-6 focused sub-questions. Note any source file paths.

## 1. cx — CODE / FEASIBILITY / REUSE lens (background, runs independently of ax)
Write `STAGING/_process/<YYYY-MM>/_council_cx_<slug>.md` (create the month dir if missing — process artifacts NEVER go to STAGING top-level; top-level is reserved for LIVE DECISION/PLAN/SPEC docs) starting with these MANDATORY header lines:
```
ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query <NLM_NOTEBOOK_ID> "<your question>"
  (NLM_NOTEBOOK_ID: bu proje için CLAUDE.md/PROJECT_RULES'tan al; RIMA default=30ddffa5-292f-4248-8e77-68074af901be)
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
<one-line purpose>
```
Then: the topic + sub-questions + "READ these source files: <paths>" + "ANALYSIS ONLY, no code changes; answer from a feasibility / what-already-exists-in-RIMA / reuse-vs-build lens; write result to CODEX_DONE.md; do NOT reproduce any prior audit."
Dispatch (run_in_background: true):
```
cx dispatch --task-file STAGING/_process/<YYYY-MM>/_council_cx_<slug>.md --effort high
# (global subcommand, works from any project root — task/done files land in CWD)
```
Read CODEX_DONE_<profile>.md when the background task notifies completion. (Only ONE cx task per profile at a time — different `--profile` values CAN run in parallel, each writes its own CODEX_DONE_<profile>.md.)

## 2. ax Gemini 3.1 Pro High — DEEP / architecture / design lens
Write the question (+ "Read these files: <paths>") to `STAGING/_process/<YYYY-MM>/_council_q_31pro_<slug>.md`, then dispatch with the model passed per-call via `--model` (no settings.json swap):
```powershell
ax dispatch --task-file 'STAGING\_process\<YYYY-MM>\_council_q_31pro_<slug>.md' --model 'Gemini 3.1 Pro (High)' --print-timeout 480 2>$null
```

## 3. ax Gemini 3.5 Flash High — PRAGMATIC / lean / ship-fast lens
SAME pattern with `--model 'Gemini 3.5 Flash (High)'` and file `STAGING\_process\<YYYY-MM>\_council_q_35flash_<slug>.md`. Frame it as the "leanest path + over-engineering critique" lens (a DISTINCT angle from 3.1 Pro).
```powershell
ax dispatch --task-file 'STAGING\_process\<YYYY-MM>\_council_q_35flash_<slug>.md' --model 'Gemini 3.5 Flash (High)' --print-timeout 300 2>$null
```

## 4. Synthesize (you, Opus)
Read cx (CODEX_DONE.md) + both Gemini answers. Resolve disagreements explicitly. Produce a final DECISION/recommendation. Write it to `STAGING/<TOPIC>_DECISION_<YYYY-MM-DD>.md` and present a tight summary (agreements, the key disagreement + your call, final recommendation).

# Hard rules
- **--model per-dispatch (2026-06-13):** model is passed per call via `--model` → no shared settings.json swap, no try/finally restore. Two different-model ax dispatches CAN run in parallel (use distinct `--account` + ~25s stagger). The old model-race serialization constraint is GONE.
- **cx single-instance PER PROFILE:** never run two `cx dispatch` jobs on the SAME profile at once; different profiles can run in parallel. Profile selection = quota-aware auto (skips DISABLED; manage with `cx enable/disable`).
- **Distinct lenses:** cx = feasibility/reuse, 3.1 Pro = deep architecture, 3.5 Flash = lean/ship-fast. Diversity makes the synthesis worth more than one opinion.
- **You decide.** The three are advisors; the final call + doc is yours (Opus).

USER TOPIC:
$ARGUMENTS
