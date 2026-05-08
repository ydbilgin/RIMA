# RIMA — Universal Project Rules
Working dir: `F:/Antigravity Projeler/2d roguelite/RIMA/`. No `../`.
All agents (Claude, Codex, Gemini) follow these rules.

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

Root files: CLAUDE.md, CODEX.md, RULES.md, ANTIGRAVITY.md, CURRENT_STATUS.md, AGENTS.md, SYSTEM_MAP.md, README.md.

## Cleanup
- Completed one-time docs -> `ARCHIVE/`. Temp -> delete or archive immediately after use.
- **Commit frequently** -- git status loads every message; dirty state = token waste.

## Test
NUnit + Unity Test Runner + MCP `run_tests`.
- EditMode: No `Awake()` -> explicit init. Seeded: `Random.InitState(42)`.
- DungeonGraph: `Is.InRange`. Coroutine/Singleton -> PlayMode.

## Sprite/Asset (S43)
128px canvas, PPU=128, 4 cardinal (S/E/N/W). `create_character` FORBIDDEN.
Detail: `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`.

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

## Context Economy (Claude token priority)
**Goal: minimize what Claude reads/writes. Gemini/Codex token cost is irrelevant.**

### HARD RULE: NO FILE READS (Claude)
Claude NEVER opens any file directly except `CURRENT_STATUS.md` at session start.
ALL other context comes from NotebookLM:
```
uvx --from notebooklm-mcp-cli nlm notebook query ed3c8952-417c-4988-84a7-425d25ba3b08 "question"
```
No exceptions. If NotebookLM is down or answer is insufficient, state it explicitly — do not silently fall back to file reads.

- File writing: delegate to Gemini (--yolo) for mechanical updates. Claude only writes when judgment is required.
- **Per-class file rule:** When implementing a new class, create a dedicated file (e.g. `TASARIM/WARBLADE_IMPL.md`). Do NOT append to multi-class canonical docs. One file = one class = one concern. Existing LOCKED multi-class docs stay as-is.
- NotebookLM sync: at session end via `git diff` + source delete/re-add for changed files.

## Project Specs
Unity 2D URP, namespace `RIMA`, scene `Assets/Scenes/_IsoGame.unity`.
S43: Char 128px, Floor 64x32, Wall 64x96, PPU=128.
Tone: Fractured Epic (compact, non-chibi).
