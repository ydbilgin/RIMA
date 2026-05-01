# RIMA — Claude Code Instructions
Working dir: `F:/Antigravity Projeler/2d roguelite/RIMA/`. No `../`.

## Orchestra Rule (PRIMARY)
Claude main thread is the orchestra conductor. It dispatches; it does not do mechanical work itself.

- You hold the project map in head via `MEMORY/INDEX.md` (pointer index). Do NOT re-read files you already read this session.
- When you delegate to a sub-agent, hand over: (a) explicit file paths the agent may open, (b) inline excerpts when small, (c) exact output format.
- Sub-agents do NOT auto-discover context. No "open CURRENT_STATUS.md to figure out what to do." That is the orchestrator's job.
- Token budget: orchestrator keeps its own context lean by NOT opening files for reference. Agents pay the open-cost in their isolated context.

See `AGENTS.md` for the full routing matrix and Context Discipline section.

## Session Start
Read `CURRENT_STATUS.md` only. Continue from there. Do not auto-open other files; pull them on demand.

## Memory
Shared: `MEMORY/INDEX.md` -> open related `*.md` only when its trigger matches the current task.
All agents (Claude, Codex via rima-codex, Antigravity, others) read from `MEMORY/`.
Claude writes memory directly via Write tool; other agents must `git commit` after editing.

## Folders
`Assets/`, `TASARIM/`, `GUIDES/`, `CONCEPT_ART/`, `ARCHIVE/`, `STAGING/`.
Root: CLAUDE.md, CURRENT_STATUS.md, AGENTS.md, CODEX.md, README.md, Unity files. New docs -> GUIDES/ or TASARIM/.

## Cleanup
Completed Codex run reports / one-time docs -> `ARCHIVE/CODEX_TAMAMLANDI/`. Temp -> `ARCHIVE/`.
**Temp files rule:** When creating a one-time file (QC report, review prompt, eval), note its deletion target in the same message. Delete or archive immediately after use.

Roles, model routing, context discipline: see `AGENTS.md`.

## /clear
Call after: Review PASS · New phase · 20+ messages · Heavy batch · Topic switch.
Cache TTL = 5 min. "N uncached" in statusline = N tokens paid full-price this turn. /clear at phase boundaries, not mid-task.

## Test
NUnit + Unity Test Runner + MCP `run_tests`. Sonnet: write+run+fix.
- EditMode: No `Awake()` -> explicit init.
- Seeded: `Random.InitState(42)` in SetUp.
- DungeonGraph: `Is.InRange` · Coroutine/Singleton -> PlayMode.

## Sprite/Asset (S43)
128px canvas · `create_character` FORBIDDEN. Detail: `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`.

## Token Saving
Session start: `CURRENT_STATUS.md` only. Edit: surgical line ranges. Bulk work -> rima-codex.
**CLAUDE.md and CURRENT_STATUS.md must stay lean. No redundant content — if it lives in another doc, use a pointer.**
**/lint:** Phase transition · 5+ decisions · Before asset work.

## Language
User: Turkish · Internal files (.md, prompts, code): English.
**Encoding rule:** Internal .md files must use ASCII-only characters. No Turkish diacritics. Reason: Claude/Codex write with different encodings; double-encoding corrupts Turkish chars.

## File Map
**Every session:** `CURRENT_STATUS.md` only.
**On demand:** `SYSTEM_MAP.md`, `AGENTS.md` (open when architecture/routing needed), `CODEX.md` (when dispatching rima-codex needs context).
**As needed:** `TASARIM/STYLE_BIBLE.md`, `GDD.md`, `MASTER_KARAR_BELGESI.md`, `ROOM_MECHANICS.md`, `SINIF_VE_SKILL_KARAR_BELGESI.md`, `COMBAT_ROSTER.md`, `BOSS_DESIGN.md`.
**Phase scope:** `TASARIM/FAZLAR/FAZ_MASTER.md` -> active phase file.
**PixelLab ref** (`F:/Antigravity Projeler/Pixellab/`): `PIXELLAB_PIPELINE.md`, `PIXELLAB_API_V2.md`.

## Project Specs
Unity 2D URP · Namespace RIMA · Scene `Assets/Scenes/_IsoGame.unity`.
**S43:** Char 128² · Floor 64×32 · Wall 64×96 · PPU=64.
Tone: Fractured Epic (compact, non-chibi).
Interaction (G-key) / Inspector -> see `SYSTEM_MAP.md`.
