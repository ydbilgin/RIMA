# CODEX BRAIN (Persistent)

## Purpose
This file is Codex's persistent operational memory for this project. Codex reads this on every invocation — both when launched via the user directly (`cx <profile>`) and when invoked through Claude's `rima-codex` agent (`cx <profile> exec "<prompt>"`).

## Protection Rule
- CODEX.md is protected and must NOT be deleted.
- If missing, recreate immediately and restore these rules.

## Authority Level
Codex is a mechanical executor inside RIMA's agent stack:
1. Claude orchestrator -- dispatches, decides, integrates
2. Sub-agents (rima-design, rima-doc, rima-qc, rima-asset, rima-research)
3. Codex (this agent, invoked via rima-codex or user-direct)

Codex does NOT make design decisions. Codex does NOT override Claude QC. Codex stays inside the allowed-files boundary in the prompt, period.

## Workflow (current — 2026-05-01)
- Codex is invoked **directly with a full task prompt**. The CODEX_TASKS.md / CODEX_DONE.md file workflow is RETIRED.
- When invoked via `rima-codex`: prompt contains the task body + allowed file list + report format. Execute and respond on stdout.
- When invoked directly by the user: same expectation — wait for explicit task scope, do not improvise.
- Report back in the format the prompt specifies (typically: `STATUS / COMPLETED / ERRORS / FILES_TOUCHED / NEXT_SIGNAL`).

## Memory Sources (Shared -- Read These When Relevant)
| File | Contents |
|---|---|
| `MEMORY/INDEX.md` | Master memory index -- read first, open files only by trigger keyword match |
| CURRENT_STATUS.md | Active phase, locked decisions, anchor status, priority queue |
| SYSTEM_MAP.md | Unity system wiring, Inspector refs, prefab slots |
| MASTER_KARAR_BELGESI.md | All major locked design decisions, numbered |
| AGENTS.md | Agent routing matrix + authority order |

All memory lives in `MEMORY/`. No private/local paths.

## Git Attribution Rule (MANDATORY)
When Codex edits CURRENT_STATUS.md, SYSTEM_MAP.md, any TASARIM/ doc, or any file under MEMORY/, commit immediately:
```
git add <changed files>
git commit -m "CODEX: <one line describing what changed and why>"
```

## Priority Order
1. User's latest explicit command (in prompt or direct chat)
2. Rules in CODEX.md (this file)
3. Allowed-files boundary in the task prompt

## Core Workflow Rules
1. Stay strictly within task scope and allowed-files boundary.
2. Do not edit unrelated files.
3. If a task requires touching a file outside the allowed list, STOP and report scope drift to the caller (rima-codex agent or user). Do not proceed.
4. Default report mode: structured (STATUS / COMPLETED / ERRORS / FILES_TOUCHED / NEXT_SIGNAL).

## MCP Connection Memory
- Do NOT call any MCP tool unless the current task truly requires it (Unity MCP, PixelLab MCP, etc.).
- Default: solve via local files + shell. Escalate to MCP only when needed (Unity scene inspection, automated import/wiring).

### PixelLab MCP
Config: `C:\Users\ydbil\.codex\config.toml` -- `[mcp_servers.pixellab]` via npx mcp-remote, Bearer token.
API docs: https://api.pixellab.ai/mcp/docs

**Available tools:**
- `animate_character` -- params: `character_id`, `template_animation_id`, `action_description`, `animation_name`, `directions`, `confirm_cost`
  - ALWAYS pass `confirm_cost=true` first to check gen cost before committing.
- `get_character` -- check status, get rotation URLs, ZIP download. params: `character_id`, `include_preview`
- `list_characters` -- pagination: `limit`, `offset`, optional `tags`

**FORBIDDEN:**
- `create_character` -- credit cost, user generates via PixelLab UI only.

**Keyframe / First-Last Frame -- NOT in MCP API.**
Neither `animate_character` nor `animate_object` has `first_frame`, `last_frame`, or `keyframe` params.
Keyframe-driven smooth animations must be done by the user in the PixelLab UI.

**Gen Budget (2026-04-30):** 2586/5000 used, ~2414 left, deadline 2026-05-18.
Rule: estimate cost with `confirm_cost=true`, report to caller before any batch. Do NOT run speculative generations.

### MCP run_tests -- Authority & Syntax
When a task explicitly grants MCP `run_tests` authority:
```
run_tests mode=PlayMode assembly=RIMA.Tests.PlayMode
run_tests mode=EditMode assembly=RIMA.Tests
```
- PlayMode tests require Unity scene loaded; EditMode tests do not.
- Compile errors -> fix in test file, re-run. Max 2 attempts.
- Runtime failures from missing scene objects -> report as-is, do NOT fix game logic.
- Test file: `Assets/Tests/PlayMode/RoomFlowTests.cs` (namespace: `RIMA.Tests`)

## RIMA Project Infrastructure Snapshot
- Unity 6000.3.6f1, URP 17.3.0, 2D Animation, Aseprite Importer, PSD Importer, Tilemap, Input System, Test Framework.
- Main scene: `Assets/Scenes/_IsoGame.unity`.
- Runtime systems: room loop, dungeon graph, HUD, draft, enemy, resource systems.
- Useful local tools:
  - `Tools/prefab_health_check.py` -- prefab health/QC check
  - `Tools/audit_cleanup.py` -- staging/archive/root clutter audit
  - `Tools/remove_bg.py` -- asset background cleanup
  - `Tools/comfy_*` -- pixel art candidate generation/selection support
- Active bottleneck is pipeline discipline:
  - 128x128 canvas
  - PPU 64
  - Multiple sprite mode
  - center pivot
  - direction suffix standard
  - idle/run scale drift QC
  - PixelLab -> Aseprite QC -> Unity import -> prefab/animator wiring

## Project File Map
### Root (F:/Antigravity Projeler/2d roguelite/RIMA/)
| File | Purpose |
|---|---|
| CURRENT_STATUS.md | Active next-work source -- read every session |
| CLAUDE.md | Claude orchestrator instructions |
| AGENTS.md | Agent routing matrix |
| SYSTEM_MAP.md | Unity system wiring |
| CODEX.md | This file -- Codex persistent brain |

### TASARIM/
| File | Purpose |
|---|---|
| GDD.md | Game design document -- core pillars |
| MASTER_KARAR_BELGESI.md | All major design decisions, numbered |
| SINIF_VE_SKILL_KARAR_BELGESI.md | Per-class skill/lore decisions |
| STYLE_BIBLE.md | Visual identity -- palette, tone, proportions |
| COMBAT_ROSTER.md | Enemy list + combat stats |
| BOSS_DESIGN.md | Boss mechanics |
| FAZLAR/FAZ_MASTER.md | Phase roadmap |

### Assets/
| Path | Purpose |
|---|---|
| Scenes/_IsoGame.unity | Main active scene |
| Scripts/ | All C# runtime code |
| Sprites/Characters/ | Imported character spritesheet PNGs |
| Prefabs/ | Runtime prefabs (player, enemy, room, HUD) |

## Reporting Memory
- Default report structure (every non-trivial task):
  1) direct answers/results
  2) evidence table (source -> claim -> confidence)
  3) explicit assumptions and gaps
  4) reviewer checklist (PASS/FAIL points)
- Send short summary-only when caller explicitly asks for brief output.

## Default Editable Files (without extra permission)
- CODEX.md (this file)
- Files explicitly listed in the current task's allowed-files boundary.

Note: CODEX_TASKS.md and CODEX_DONE.md no longer exist as a workflow. They are retired.

## Failure Recovery
If any step is missed:
- Fix immediately.
- Note the correction in the report under ERRORS section.
- Continue from corrected state.

## PixelLab Production Baseline (RIMA)
- Final in-game character/animation standard: `128x128`.
- User-confirmed live behavior:
  - `Animate with text NEW` at `128px` can reach `16 frames`.
  - `Animate with text NEW` at `220px` observed max around `10`.
- Practical default:
  - Use `Animate with text NEW` for key motion chunks.
  - Use `Interpolate NEW` for in-betweens and transitions.

## Prompt Policy Memory
- Keep prompts short and structural.
- One generation = one motion intention.
- No camera angle text in prompt (camera handled in UI).
- Use hard constraints for weapon-hand continuity.
- Warblade mandatory constraints:
  - `both hands on same long hilt`
  - `right hand near crossguard, left hand near pommel`

## Animation Construction Memory
- Prefer segmented chains over one long request:
  - `A -> B`
  - `B -> C`
  - optional `C -> D`
- For run to idle, generate a dedicated short bridge (`run_end -> idle_start`) instead of full regen.

## Direction Convention (RIMA -- Permanent)
| Direction | DirX  | DirY  | Sprite suffix | BlendPos       | PixelLab source file (S43 offset) |
|-----------|-------|-------|---------------|----------------|-----------------------------------|
| S         | 0     | -1    | _S            | (0, -1)        | south-east.png                    |
| SE        | +0.71 | -0.71 | _SE           | (0.71, -0.71)  | east.png                          |
| E         | +1    | 0     | _E            | (1, 0)         | north-east.png                    |
| NE        | +0.71 | +0.71 | _NE           | (0.71, 0.71)   | north.png                         |
| N         | 0     | +1    | _N            | (0, 1)         | north-west.png                    |
| NW        | -0.71 | +0.71 | _NW           | (-0.71, 0.71)  | west.png                          |
| W         | -1    | 0     | _W            | (-1, 0)        | south-west.png                    |
| SW        | -0.71 | -0.71 | _SW           | (-0.71, -0.71) | south.png                         |

S43 offset note: anchors generated SW-facing (1 step CW offset). Use PixelLab source file column for import.
Full details: `MEMORY/feedback_pixellab_direction.md`

Import standard:
- PPU = 64
- Sprite Mode = Multiple
- Frame size = 128x128 per cell
- Pivot = center (0.5, 0.5)
- Filter = Point (no filter)
- Compression = Uncompressed

Naming: uppercase suffix. `warblade_run_SE.png` correct, `warblade_run_se.png` wrong.

## Sprite Import Pitfalls (Permanent — Do Not Repeat)

### Mistake 1 — PPU inconsistency
Idle sprite PPU=44, Run sprite PPU=64.
Result: idle rendered 1.34x-1.66x different size from run.
RULE: ALL animation types (idle, run, attack, skill, dash, death) use PPU=64. Never set different PPU as workaround.

### Mistake 2 — Single Mode auto-trim + wrong pivot
Idle imported as Single mode. Unity auto-trimmed transparent pixels: 128x128 canvas -> 94x101 character area.
Pivot auto-assigned to (0,0) bottom-left.
Run was Multiple mode, full 128x128, center pivot.
Result: idle/run transitions caused character position jump.
RULE: always Multiple mode, explicit 128x128 rect, pivot=(0.5, 0.5) center.

### Mistake 3 — Blend position swap
run_SE clip assigned to blend tree (-0.71, -0.71) = SW position.
Result: moving SE direction played SW sprite.
RULE: copy from Direction Convention table above. Never guess.

## Sprite Scale Consistency Rules (Permanent)

Idle/Run sprites had bbox-occupancy mismatch. Idle ~45% canvas height, Run ~61%. Root cause: different PixelLab production flow.

### Rule 1 — Same pipeline
Blend-critical states (idle, run, attack) MUST use the SAME PixelLab flow.
`Custom Animation V3` for both idle and run.
`Create Character` produces base sprite (scale anchor); use as V3 first frame. Do NOT generate animation clips directly from Create Character.

### Rule 2 — Baseline scale lock
Per class: produce a baseline direction frame (typically `_S`) and lock it.
All other directions must be within +/-5% bbox-height tolerance. Outside -> regenerate.

### Rule 3 — Import is fixed; do not fix art with code
PPU=64, 128x128, Multiple, center pivot are FIXED. Scale drift visible -> art problem (sprite occupancy), not Unity. Regenerate sprite, do not change import settings as workaround.

### Rule 4 — Prompt framing constraint (mandatory)
Every PixelLab prompt must include: `full body, centered, same scale as reference, no zoom-in`
Animation flow: same character reference every time (gallery or upload).

### Rule 5 — Aseprite QC step
Before Unity import: in Aseprite, overlay same-direction idle and run frames. Silhouette + foot-point alignment matches -> proceed. Mismatch -> regenerate.
