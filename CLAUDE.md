# RIMA — Claude Code Instructions
Working dir: `F:/Antigravity Projeler/2d roguelite/RIMA/`. No `../`.

## Session Start
Read `CURRENT_STATUS.md`, continue from there. Do not ask for reminders.

## Memory
Shared: `MEMORY/INDEX.md` → open related `*.md` on demand.
All agents (Claude, Codex, Antigravity, others) read from `MEMORY/`.
No private/local memory path. Claude writes memory files directly; other agents must git commit after editing.

## Folders
`Assets/`, `TASARIM/`, `GUIDES/`, `CONCEPT_ART/`, `ARCHIVE/`, `STAGING/`.
Root: CLAUDE.md, CURRENT_STATUS.md, AGENTS.md, README.md, Unity files. New docs → GUIDES/ or TASARIM/.

## Cleanup
Completed `CODEX_*.md` → `ARCHIVE/CODEX_TAMAMLANDI/`. Temp → `ARCHIVE/`. Deletions → mark with `# [ARCHIVE]`.
**Temp files rule:** When creating a one-time file (QC report, review prompt, eval), note its deletion target in the same message. Delete immediately after use — do not accumulate.

## Roles
**Claude:** Design, architecture, debug, decisions, QC judgment.
**Codex:** Mechanical+Analytical (import, anim, prefab, SO, isolated C#, files, doc, review).
**Delegation Gate (Codex if all YES):** Deterministic · Mechanical · Isolated · Defined boundaries.

### Agent Routing
Agent routing → see `AGENTS.md`.
Claude orchestrates; does not write prompts/reviews. Token-first · Save state before spawn · Escalate scope creep to Claude.

## /clear
Call after: Review PASS · New phase · 20+ messages · Heavy batch.

## Test
NUnit + Unity Test Runner + MCP `run_tests`. Sonnet: write+run+fix.
- EditMode: No `Awake()` → explicit init.
- Seeded: `Random.InitState(42)` in SetUp.
- DungeonGraph: `Is.InRange` · Coroutine/Singleton → PlayMode.

## Sprite/Asset (S43)
128px canvas · Style Reference PRO · MCP `create_character` FORBIDDEN (credit).
User generates PixelLab UI → Claude provides prompt+style ref → Codex imports → QC → Claude decides.
Detail: `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`, `memory/feedback_pixellab_create_character_workflow.md`.

## Token Saving
1. Session start: `CURRENT_STATUS.md` only. Open other files on demand. Do not open source files for reference.
2. Edit: surgical line ranges only.
3. Update `SYSTEM_MAP.md` for structural changes.
4. Analysis/Cleanup: Gemma/Codex; deletion: PowerShell.
**Compact default:** All internal .md compact (except Guides).
**/lint:** Phase transition · 5+ decisions · Before asset work · Inconsistency check.

## Model
Sonnet 4.6 (default). Opus 4.7 for multi-system trade-offs. Claude manages.

## Language
User: Turkish · Internal files (.md, prompts, code, internal files): English.
**Encoding rule:** Internal .md files must use ASCII-only characters. No Turkish special chars (s/i/g/u/o/c with diacritics). Use plain English or ASCII-safe workarounds (e.g. "derece" instead of degree symbol). Reason: Claude and Codex write files with different encoding, causing double-encoding corruption on Turkish chars.

## File Map
**Every session:** `CURRENT_STATUS.md` only.
**On demand:** `SYSTEM_MAP.md`, `AGENTS.md` (open when architecture/routing needed).
**As needed:** `TASARIM/STYLE_BIBLE.md`, `GDD.md`, `MASTER_KARAR_BELGESI.md`, `ROOM_MECHANICS.md`, `SINIF_VE_SKILL_KARAR_BELGESI.md`, `COMBAT_ROSTER.md`, `BOSS_DESIGN.md`.
**Phase scope:** `TASARIM/FAZLAR/FAZ_MASTER.md` → active phase file.
**PixelLab ref** (`F:/Antigravity Projeler/Pixellab/`): `PIXELLAB_PIPELINE.md`, `PIXELLAB_API_V2.md`, `AGENTS.md`.

## Project Specs
Unity 2D URP · Namespace RIMA · Scene `Assets/Scenes/_IsoGame.unity`.
**S43:** Char 128² · Floor 64×32 · Wall 64×96 · PPU=64.
Tone: Fractured Epic (compact, non-chibi).
Interaction (G-key) / Inspector → see `SYSTEM_MAP.md`.
