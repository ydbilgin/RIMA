# RIMA — Claude Agent Rules
**Universal project rules: see `RULES.md`.**
Agent stack definition: `ANTIGRAVITY.md`. Routing details: `AGENTS.md`.

## Role: Orchestra Conductor (PRIMARY)
Claude dispatches work; does NOT do mechanical bulk itself.

- Hold project map via `MEMORY/INDEX.md`. Do NOT re-read files already read this session.
- Delegate to sub-agents with: (a) explicit file paths, (b) inline excerpts when small, (c) exact output format.
- Sub-agents do NOT auto-discover context. Orchestrator feeds them.
- **Mutual QC:** Claude reviews Codex/Gemini commits. Errors -> fix and re-commit. All revertible via git.

## Session Start
Read `CURRENT_STATUS.md` only. Continue from there. Pull other files on demand.

## Memory
Shared: `MEMORY/INDEX.md` -> open related `*.md` only when trigger matches current task.
All agents (Claude, Codex, Gemini) read from `MEMORY/`. All must commit after changes.

## Claude Skills (Slash Commands)
| Skill | When to use |
|---|---|
| `/plan` | Before any non-trivial implementation -- alignment first |
| `/lint` | Phase transitions, 5+ decisions, before asset work |
| `/phase-close` | Before moving to next phase |
| `/query` | Token-efficient RIMA knowledge lookup |
| `/graphify` | Codebase knowledge graph (run on demand, not every session) |
| `/playtest` | Generate Gemini-executed playtest scenarios |
| `/save-session` | Save session state for future resume |
| `/simplify` | Review changed code for reuse/quality after Codex commits |
| `/update-config` | Configure Claude Code settings/hooks/permissions |
| `/accounts` | Check all Claude account rate limit status |

## Claude Sub-Agents
| Agent | Purpose |
|---|---|
| `rima-design` | Game design decisions, balance, combat systems |
| `rima-codex` | Dispatch tasks to Codex CLI (laurethgame / laurethayday / yasinderyabilgin) |
| `rima-doc` | Docs/memory updates, CURRENT_STATUS sync |
| `rima-qc` | QC/review of Codex output, images, cross-file consistency |
| `rima-asset` | Sprite/asset PixelLab prompts |
| `rima-research` | Web research, external docs (Unity, PixelLab API, etc.) |
| `Explore` | Fast read-only codebase search |
| `Plan` | Implementation planning before code |

## NotebookLM (Context Source)
TASARIM/ ve MEMORY/ dosyalarını direkt okumadan önce NotebookLM'e sorgula:
```bash
uvx --from notebooklm-mcp-cli nlm notebook query ed3c8952-417c-4988-84a7-425d25ba3b08 "soru"
```
Notebook: `RIMA Game Design Knowledge Base` — 80 kaynak, 2026-05-06 sync.
Detay: `MEMORY/notebooklm_workflow.md`

## Token Saving
- Session start: `CURRENT_STATUS.md` only. Edit: surgical line ranges.
- **Bağlam için:** NotebookLM query önce, dosya okuma sonra (sadece NotebookLM yetersiz kalırsa).
- Bulk mechanical work -> Codex (GPT 5.5 High).
- Bulk analysis/audit -> Gemini (2.5 Pro).
- CLAUDE.md and CURRENT_STATUS.md must stay lean. Pointers, not content.
- `/lint` at: phase transitions, 5+ decisions, before asset work.

## Output Economy
- **Mechanical work** (code, test, commit): terse, fragments OK.
- **Design/decision work** (architecture, balance): nuanced output.
- Toggle automatic. Never compress design judgment.

## /clear
Call after: Review PASS, new phase, 20+ messages, heavy batch, topic switch.
Cache TTL = 5 min.

## Multi-Account Routing
- **"AWS'deyim"** -> Bedrock. Opus 4.6 decisions, Sonnet 4.6 mechanical.
- **"Asil hesabimdayim"** -> Claude Pro.
