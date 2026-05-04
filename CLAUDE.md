# RIMA — Project Rules (All Agents)
Working dir: `F:/Antigravity Projeler/2d roguelite/RIMA/`. No `../`.

**This file is the single source of truth for ALL agents (Claude, Codex, Gemini).**
Agent stack definition: `ANTIGRAVITY.md`. Routing details: `AGENTS.md`.

## Orchestra Rule (PRIMARY)
Claude is the orchestra conductor. Dispatches work, does not do mechanical bulk itself.

- Hold project map via `MEMORY/INDEX.md`. Do NOT re-read files already read this session.
- Delegate to sub-agents with: (a) explicit file paths, (b) inline excerpts when small, (c) exact output format.
- Sub-agents do NOT auto-discover context. Orchestrator feeds them.
- **Mutual QC:** Claude reviews Codex/Gemini commits. Errors -> fix and re-commit. All revertible via git.

## Session Start
Read `CURRENT_STATUS.md` only. Continue from there. Pull other files on demand.

## Memory
Shared: `MEMORY/INDEX.md` -> open related `*.md` only when trigger matches current task.
All agents (Claude, Codex, Gemini) read from `MEMORY/`. All must commit after changes.

## Folders
| Folder | Purpose |
|---|---|
| `Assets/` | Unity project files |
| `TASARIM/` | Design docs (Turkish: "design") |
| `GUIDES/` | Production guides, references |
| `STAGING/` | Work-in-progress, prompts, temp outputs |
| `ARCHIVE/` | Completed/historical (RESEARCH_RAW, AKADEMIK, SCREENSHOTS_OLD, etc.) |
| `MEMORY/` | Shared agent memory |
| `Tools/` | Python/PS scripts |

Root files: CLAUDE.md, ANTIGRAVITY.md, CURRENT_STATUS.md, AGENTS.md, SYSTEM_MAP.md, README.md.

## Cleanup
- Completed one-time docs -> `ARCHIVE/`. Temp -> delete or archive immediately after use.
- **Commit frequently** — git status loads every message; dirty state = token waste.

## Test
NUnit + Unity Test Runner + MCP `run_tests`.
- EditMode: No `Awake()` -> explicit init. Seeded: `Random.InitState(42)`.
- DungeonGraph: `Is.InRange`. Coroutine/Singleton -> PlayMode.

## Sprite/Asset (S43)
128px canvas, PPU=128, 4 cardinal (S/E/N/W). `create_character` FORBIDDEN.
Detail: `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`.

## Token Saving
- Session start: `CURRENT_STATUS.md` only. Edit: surgical line ranges.
- Bulk mechanical work -> Codex (GPT 5.5 High).
- Bulk analysis/audit -> Gemini (2.5 Pro).
- CLAUDE.md and CURRENT_STATUS.md must stay lean. Pointers, not content.
- `/lint` at: phase transitions, 5+ decisions, before asset work.

## /clear
Call after: Review PASS, new phase, 20+ messages, heavy batch, topic switch.
Cache TTL = 5 min.

## Language
User: Turkish. Internal files (.md, prompts, code): English.
**Encoding:** ASCII-only in .md files. No Turkish diacritics (encoding mismatch between agents).

## File Map
| Load | Files |
|---|---|
| Every session | CURRENT_STATUS.md |
| On demand | SYSTEM_MAP.md, AGENTS.md, ANTIGRAVITY.md |
| As needed | TASARIM/STYLE_BIBLE.md, GDD.md, MASTER_KARAR_BELGESI.md, SINIF_VE_SKILL_KARAR_BELGESI.md |
| Phase scope | TASARIM/FAZLAR/FAZ_MASTER.md -> active phase file |

## Output Economy
- **Mechanical work** (code, test, commit): terse, fragments OK.
- **Design/decision work** (architecture, balance): nuanced output.
- Toggle automatic. Never compress design judgment.

## Multi-Account Routing
- **"AWS'deyim"** -> Bedrock. Opus 4.6 decisions, Sonnet 4.6 mechanical.
- **"Asil hesabimdayim"** -> Claude Pro.

## Project Specs
Unity 2D URP, namespace `RIMA`, scene `Assets/Scenes/_IsoGame.unity`.
S43: Char 128px, Floor 64x32, Wall 64x96, PPU=128.
Tone: Fractured Epic (compact, non-chibi).
