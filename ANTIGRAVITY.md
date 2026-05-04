# ANTIGRAVITY — Studio Agent Stack

## What Is This
Antigravity is the solo dev studio behind RIMA. This file defines the available AI agents
and how they coordinate. All agents share the same rules (`CLAUDE.md`) and memory (`MEMORY/`).

## Available Agents

| Agent | Model | Role | Availability |
|---|---|---|---|
| Claude | Opus 4.6 / Sonnet 4.6 | Orchestrator, decisions, QC, dispatch | Primary (always on) |
| Codex | GPT 5.5 High | Mechanical executor, bounded code tasks | On-demand (user invokes) |
| Gemini | Gemini 2.5 Pro | Research, file audits, bulk analysis | On-demand (user invokes) |

## Shared Rules
**All agents read `CLAUDE.md` as their instruction set.** No separate rule files per agent.
- CLAUDE.md = project rules, constraints, workflow
- CURRENT_STATUS.md = active work state
- MEMORY/INDEX.md = shared knowledge pointer

## Commit Attribution
| Agent | Commit Format |
|---|---|
| Claude | `Co-Authored-By: Claude <noreply@anthropic.com>` |
| Codex | `Co-Authored-By: Codex (GPT 5.5) <noreply@antigravity.dev>` |
| Gemini | `Co-Authored-By: Gemini <noreply@antigravity.dev>` |
| User (manual) | Default git user |

## Mutual QC Rule
- Claude reviews Codex/Gemini commits before proceeding.
- If issues found: agent fixes and re-commits.
- All commits are revertible via git. Errors are cheap to undo.

## Task Dispatch
- Claude dispatches tasks with explicit scope (files, context, output format).
- Codex/Gemini do NOT auto-discover context. They receive what they need.
- If an agent needs a file not in its task scope, it STOPs and reports.

## Report Format (Codex/Gemini)
```
STATUS: DONE | PARTIAL | BLOCKED
COMPLETED: <what was done>
FILES_TOUCHED: <list>
ERRORS: <any issues>
COMMIT: <hash if committed>
```

## User Availability Note
User (Yasin) cannot always work. Agents must:
- Commit frequently so progress is never lost
- Keep state in files (CURRENT_STATUS.md, MEMORY/) not in conversation
- Be resumable: any agent can pick up from last committed state
