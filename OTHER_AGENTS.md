# OTHER AGENTS -- RIMA Project Context
Covers: Gemini CLI (web research), Ollama (local prep), any one-off or occasional agent.

---

## Session Start
Read CURRENT_STATUS.md only. Open other files only if the task explicitly requires.

---

## Memory Architecture (Shared)
| File | Contents |
|---|---|
| `MEMORY/INDEX.md` | Master memory index -- read this first, open files on demand |
| CURRENT_STATUS.md | Active phase, locked decisions, priority queue |
| SYSTEM_MAP.md | Unity system wiring |
| MASTER_KARAR_BELGESI.md | Locked design decisions |
| CODEX.md | Codex brain: import rules, conventions |
| ANTIGRAVITY.md | Antigravity context and rules |
| AGENTS.md | Full agent routing matrix |

All memory lives in `MEMORY/`. No private/local paths.

---

## Git Attribution Rule (MANDATORY)
If any agent in this file edits a tracked project file (CURRENT_STATUS.md, SYSTEM_MAP.md, any TASARIM/ doc):
```
git add <changed files>
git commit -m "<AGENT_NAME>: <one line describing what changed>"
```
Use the agent's actual name as prefix. Example: `git commit -m "GEMINI_CLI: Updated Unity forum research note in CURRENT_STATUS.md"`

---

## Gemini CLI

**Role:** Web research, external documentation lookup, Unity forum/changelog research.

**Can do:**
- Search Unity docs, forums, changelogs
- Look up PixelLab API / MCP documentation
- Summarize external sources for Claude

**Cannot do:**
- Unity MCP execution
- Project file writes (read-only role)
- Design or architecture decisions
- Playtest or code review

**Usage pattern:** Claude asks a specific question → user runs Gemini CLI → pastes result back to Claude.
Gemini researches; Claude decides.

---

## Ollama (Local, RTX 5080)

**Role:** Log analysis, document clustering, cheap prep work, offline research.

**Can do:**
- Parse and cluster log files
- Summarize long documents for Claude review
- Offline pattern analysis, RNG table prep

**Cannot do:**
- Final decisions
- Architectural suggestions
- Direct project file writes

**Usage pattern:** Ollama prepares → Claude decides.

---

## General Rules (All Agents in This File)
- No edits to TASARIM/GDD.md -- Claude only
- No edits to MASTER_KARAR_BELGESI.md without Claude instruction
- No writes to Library/ or PackageCache/
- No archived file edits without Claude approval
- No parallel writes to the same file
- Scope drift → stop and report back to Claude
- Encoding: ASCII-only in all internal .md files
