# AGENTS.md — RIMA Agent Routing
Load when: routing a task, choosing an agent, delegating work.

---

## Authority Order (highest to lowest)
1. **Claude (orchestrator)** — final decisions, architecture, QC judgment, agent dispatch. Non-delegatable.
2. **rima-design (Opus sub-agent)** — cross-system design judgment when orchestrator escalates.
3. **Antigravity (Codex / GPT 5.5 High)** — mechanical executor. Reads `ANTIGRAVITY.md`. Commits as `[Codex]`.
4. **rima-doc / rima-qc / rima-asset / rima-research (Sonnet sub-agents)** — doc writes, validation, asset prompts, web research.
5. **User-driven Codex CLI** — when the user runs Codex directly and reports back to Claude.

No agent overrides Claude. No agent spawns another agent.
Antigravity (Codex) reads `ANTIGRAVITY.md` for its rules. Claude reviews all Codex commits before proceeding.

---

## Claude Model Routing (orchestrator's own model)

Claude is the orchestra conductor. It does NOT do mechanical work itself; it dispatches. Model choice depends on the decision weight of the orchestration step.

| Step | Model |
|---|---|
| Day-to-day orchestration (dispatch, dispatch result review, single-file edits, doc edits) | Sonnet 4.6 (default) |
| Cross-system trade-off, balance call across multiple classes, hard architecture choice | Opus 4.7 (or spawn rima-design) |
| Final approval of a design decision before it ships | Opus 4.7 (or rima-design) |
| Mechanical refactor planning (1-2 file scope) | Sonnet 4.6 |
| Codex output review for mechanical correctness | Sonnet 4.6 (rima-qc) |
| Codex output review for design implications | Opus 4.7 (rima-design) |

Round-trip rule for high-weight decisions:
**Opus decides -> Sonnet executes (via rima-codex) -> Sonnet QCs (via rima-qc) -> Opus signs off.**
Skip the final Opus sign-off only if the QC pass is purely mechanical and no design surface was touched.

---

## Sub-Agent Table (.claude/agents/)
| Agent | Model | Scope | Writes |
|---|---|---|---|
| rima-design | Opus 4.7 | cross-system design, balance, identity calls | no |
| rima-doc | Sonnet 4.6 | docs, status, memory, archive moves | yes (project files) |
| rima-qc | Sonnet 4.6 | code/visual/lint review | no |
| rima-asset | Sonnet 4.6 | PixelLab / Gemini concept prompts | yes (`STAGING/` only) |
| rima-codex | Sonnet 4.6 | invokes Codex via `cx <profile> exec` | indirectly (Codex writes) |
| rima-research | Sonnet 4.6 | Gemini CLI external research | no |

---

## Routing Table
| Task | Agent / Model | Notes |
|---|---|---|
| Class / skill / boss / room design call | rima-design (Opus) | escalate from orchestrator |
| Single-skill numeric tweak, no cross-system effect | Claude orchestrator (Sonnet) | direct decide |
| CURRENT_STATUS / SYSTEM_MAP / GUIDE write | rima-doc | orchestrator passes file + content |
| Memory file write/update | rima-doc | orchestrator passes file + content |
| Archive / file move | rima-doc | `mv`, no delete |
| Mechanical Codex implementation (bounded) | rima-codex | orchestrator passes full task body |
| Codex output mechanical review (compile, scope) | rima-qc | spec inline or path |
| Codex output design review | rima-design (Opus) | when judgment needed |
| Visual QC (sprite, animation) | rima-qc | spec inline |
| Lint / cross-doc consistency | rima-qc | scope listed by orchestrator |
| PixelLab / Gemini concept prompt | rima-asset | writes `STAGING/` only |
| Animation frame / direction plan | rima-asset | |
| Web research, external docs lookup | rima-research | runs `gemini -p`, returns summary |
| Final architecture / cross-system decision | Claude orchestrator (Opus or rima-design) | non-delegatable |
| 1-2 line direct edit | Claude orchestrator (Sonnet) | no agent spawn |

---

## Context Discipline (HARD RULE — applies to ALL agents)

The orchestrator (Claude main thread) is the conductor. Agents are dumb workers and they DO NOT auto-load project files.

**For the orchestrator:**
- You hold the project map in head: `MEMORY/INDEX.md` is your pointer index. You do NOT need to re-open files you have already read this session.
- **NotebookLM first:** Before reading any TASARIM/ or MEMORY/ file, query NotebookLM. Only open the file if the answer is insufficient.
  `uvx --from notebooklm-mcp-cli nlm notebook query ed3c8952-417c-4988-84a7-425d25ba3b08 "question"`
- When dispatching to an agent, hand it: (a) the explicit file paths it may open, (b) any inline excerpts it needs, (c) the exact output format expected.
- Do not let agents discover context on their own. Discovery in agents = wasted tokens + drift.

**For agents:**
- Open ONLY paths the orchestrator listed in your prompt.
- If you need a file that was not listed, stop and report missing context. Do not browse, do not glob, do not grep beyond your scope.
- No file in your prompt -> no file you may open. Period.
- **Graphify query-first (HARD RULE, ALL agents):** cross-file/architecture question ("what calls X", "how does Y flow", "which files are involved", "what depends on Z") -> query the project's graphify graph FIRST (`graphify query`/`explain`/`path` on graph.json), ~71x cheaper than bulk-reading. RIMA graph: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/graph.json`. graphify = queryable index, not visualization.

**Exception:** Antigravity (Codex) reads `ANTIGRAVITY.md` and operates within the allowed-files boundary specified in its task prompt.

---

## Codex Profile Selection (rima-codex)

`cx accounts` lists available profiles. As of 2026-05-01:
- `laurethgame` — primary (project owner)
- `laurethayday` — secondary
- `yasinderyabilgin` — tertiary
- `ydbilgin` — not logged in (skip)

Selection: round-robin starting from `laurethgame`. If `LastRefresh` info is available, prefer the profile with the OLDEST refresh (load spreading). Skip `not logged in`. On rate-limit error (429 / quota), retry once with the next profile (max 2 attempts per task).

User may add or remove profiles via `cx add <name>` / `cx delete <name>`. Re-read `cx accounts` at the start of every dispatch — do not cache.

---

## Gemini CLI

Default model: `gemini-3.1-pro-preview` (set in `~/.gemini/settings.json` — no `-m` flag needed).

**rima-research:** External lookups only. No project file access. Returns summary inline.

**Mechanical file writing (new):** Gemini replaces rima-doc for mechanical doc updates.
- Use `gemini --yolo` for: CURRENT_STATUS updates, design doc additions, session notes, per-class impl files.
- Claude token cost = zero. Gemini/Codex token cost = irrelevant.
- rima-doc stays for judgment-heavy writes only (complex cross-file doc decisions).
- After Gemini writes: Claude tags for NotebookLM sync at session end (git diff based).

---

## /clear Signals
| Condition | Signal |
|---|---|
| Review/QC cycle done (PASS received) | suggest "/clear, start production clean session" |
| Before new production phase | suggest "/clear, context heavy" |
| 20+ messages | auto-warn |
| Heavy doc batch done | suggest "/clear" |
| rima-codex result received and next work is large | suggest "/clear" |

---

## /clear Cost Reminder

Anthropic prompt cache TTL = 5 minutes. Statusline "N uncached" means N tokens are paid full-price this turn (cache miss).
- Active flow (mesages within 5 min) -> cache stays warm, ~10% cost on cached prefix.
- Idle >5 min -> cache expires -> next turn pays full-price write.
- /clear at logical phase boundaries, not mid-task.

---

## Escalation (All Agents)
Return to Claude orchestrator when:
- Scope drift detected
- Output quality becomes subjective
- Risk of affecting another system or file
- Conflict with CURRENT_STATUS or locked decisions

---

## Forbidden (All Agents)
- No edits to `Library/` or `PackageCache/`
- No edits to `TASARIM/GDD.md` (orchestrator only)
- No edits to `MASTER_KARAR_BELGESI.md` without orchestrator instruction
- No edits to archived files without orchestrator approval
- No parallel writes to the same file
- ASCII-only in all internal .md files
- No spawning sub-agents from within an agent
