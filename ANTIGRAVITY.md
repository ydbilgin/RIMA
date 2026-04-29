# ANTIGRAVITY -- RIMA Project Context
Model: Gemini 2.5 Pro High / Claude Opus 4.6
Role: Asset & Analysis assistant

---

## Session Start
1. Read CURRENT_STATUS.md -- this is the ground truth for active state.
2. Open additional files only when the task explicitly requires them.
3. Do not scan all files; use the File Map below.

---

## Memory Architecture

### Memory (shared -- all agents)
| File | Contents |
|---|---|
| `MEMORY/INDEX.md` | Master memory index -- read this first, open files on demand |
| CURRENT_STATUS.md | Active phase, locked decisions, anchor status, priority queue |
| SYSTEM_MAP.md | Unity system wiring, Inspector refs, prefab slots |
| MASTER_KARAR_BELGESI.md | All major locked design decisions, numbered |
| SINIF_VE_SKILL_KARAR_BELGESI.md | Per-class skill and lore decisions |
| STYLE_BIBLE.md | Visual identity -- palette, tone, proportions |
| CODEX.md | Codex brain: import rules, direction table, sprite conventions, PixelLab baseline |
| AGENTS.md | Agent routing matrix -- who does what task type |

All memory lives in `MEMORY/`. No private/local paths.

---

## Git Attribution Rule (MANDATORY)
When Antigravity edits any of the following files, a git commit is REQUIRED immediately after:
- CURRENT_STATUS.md
- Any file under `MEMORY/`
- SYSTEM_MAP.md
- CODEX.md (only if explicitly permitted)

Commit format:
```
git add <changed files>
git commit -m "ANTIGRAVITY: <one line describing what changed and why>"
```
Example: `git commit -m "ANTIGRAVITY: Updated CURRENT_STATUS anchor table -- Hexer locked after PixelLab QC pass"`

Claude edits MEMORY/ files directly -- no git commit needed from Claude (Claude Code handles version tracking).
Codex uses CODEX: prefix. Antigravity uses ANTIGRAVITY: prefix.

---

## Can Do
- Write PixelLab prompts (CFR v3, animation, edit image)
- Write Gemini concept prompts
- Sprite pipeline guidance and QC feedback notes
- Animation frame/direction planning
- Non-critical doc edits (GUIDES/, STAGING/, staging prompt files)
- Analysis, balance checks, damage curve math
- Research summaries for Claude review
- CURRENT_STATUS.md anchor table updates (with git commit)

## Cannot Do
- Make design decisions (class, skill, boss, combat balance)
- Edit TASARIM/GDD.md (Claude only)
- Edit MASTER_KARAR_BELGESI.md (Claude only)
- Edit SINIF_VE_SKILL_KARAR_BELGESI.md without Claude approval
- C# code changes to game logic
- Write CODEX_TASKS.md (Claude writes, Codex executes)
- Final QC judgment (PASS/FAIL) -- Claude decides
- Spawn other agents

---

## Key Rules

### Direction Offset (S43 -- Permanent)
All 10 S43 class anchors were generated SW-facing. Raw PixelLab direction labels are NOT canonical game directions.
- Source files: `Characters/anchors/<class>/rotations/*.png` -- do NOT rename
- Remap happens at Unity import time, not at generation time
- In PixelLab generation tasks: always add "keep current character direction setup, do not reinterpret facing"
- In Unity import/wiring tasks: always add "apply known direction offset mapping before naming/wiring"

### PixelLab Prompt Rules
- Short and structural -- no paragraph descriptions
- One generation = one motion intention
- No camera angle text in prompt (camera is set in UI)
- Hard constraints for weapon-hand continuity
- Full body: `full body, centered, same scale as reference, no zoom-in`

### Encoding
- Internal .md files: ASCII-only characters
- No Turkish diacritics (no s/i/g/u/o/c with accents)
- Use plain English for all internal files

### Temp Files
- When creating a one-time file, note its deletion target in the same message
- Delete after use; do not accumulate in STAGING/

---

## File Map

### Root
| File | Purpose |
|---|---|
| CURRENT_STATUS.md | Active next-work source -- read every session |
| CLAUDE.md | Claude Code auto-config (routing, rules, workflow) |
| CODEX.md | Codex brain (import rules, sprite conventions, direction table) |
| ANTIGRAVITY.md | This file |
| OTHER_AGENTS.md | Context for Gemini CLI, Ollama, occasional agents |
| AGENTS.md | Full agent routing matrix |
| SYSTEM_MAP.md | Unity system wiring |

### TASARIM/
| File | Purpose |
|---|---|
| GDD.md | Game design -- Claude only edits |
| MASTER_KARAR_BELGESI.md | Locked decisions -- Claude only edits |
| STYLE_BIBLE.md | Visual identity |
| SINIF_VE_SKILL_KARAR_BELGESI.md | Per-class skill/lore |

### STAGING/
| Path | Purpose |
|---|---|
| PROMPTS_S43/PRODUCTION_GUIDE_S43.md | PixelLab prompt guide, all 10 classes |
| PROMPTS_S43/styleref_cheatsheet_v1.md | Anchor visual identity per class |
| anchors/chars/<class>/rotations/ | Raw PixelLab rotation PNGs (do not rename) |
| anchors/_ANCHOR_QC_MASTER_S43.md | QC master sheet |

---

## Output Format
For Claude review tasks: include evidence table + explicit assumptions + reviewer checklist.
For prompt/doc tasks: write directly to STAGING/ unless task specifies otherwise.
