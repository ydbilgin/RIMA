# RIMA Rules Index — rima-conventions skill catalog

Quick-lookup reference for every rule the skill checks. When updating rules, sync this catalog + `scripts/check_violations.py` + the actual memory file.

## Source files (ground truth)

| Source | Path |
|---|---|
| Project rules | `F:/Antigravity Projeler/2d roguelite/RIMA/.claude/PROJECT_RULES.md` |
| Current status | `F:/Antigravity Projeler/2d roguelite/RIMA/CURRENT_STATUS.md` |
| Memory index | `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/MEMORY.md` |
| Memory body | `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/*.md` |

## Rule catalog

### Dispatch task format (priority HIGH)

| Rule | Source | Check |
|---|---|---|
| ACTIVE RULES first line | PROJECT_RULES.md | First non-empty line starts with `ACTIVE RULES:` |
| NLM ACCESS block | PROJECT_RULES.md S91 LOCK | Found in first 15 lines |
| `Amaç:` purpose line | feedback_state_task_purpose_explicitly | Pattern `**Amaç:**` or `Amaç:` somewhere in body |
| $imagegen built-in mode | feedback_codex_imagegen_skill_not_env_var | If image gen mentioned, no unguarded OPENAI_API_KEY ref |
| --effort xhigh flag | feedback_codex_effort_xhigh_2026_05_24 | Dispatch cmd uses xhigh |

### Memory file format (priority MEDIUM-LOW)

| Rule | Source | Check |
|---|---|---|
| Frontmatter present | memory convention | Starts with `---`, closed with `---` |
| name/description/type fields | memory convention | All three in frontmatter |
| ASCII-only body | memory convention | No non-ASCII chars in body (transliterate Turkish) |
| MEMORY.md index entry | memory convention | New memory file has matching one-line index entry |

### Design locks (priority HIGH-MEDIUM)

| Rule | Source | Live status | Check |
|---|---|---|---|
| **S101 PILLAR-LESS = REVOKED** | project_pillar_seam_cover_lock_2026_05_24 | Pillar-as-seam-cover LIVE | Asserts of "PILLAR-LESS" as live = HIGH violation |
| **PPU 64 LOCK** | Karar #114, S99 LOCK | LIVE | PPU value other than 64 (or 100 in research quotes) |
| **Custom Axis Y-sort (0, 1, 0)** | research lock | LIVE | Y-sort axis other than (0,1,0) |
| **Iso 3/4 ARPG geometry** | user lock 2026-05-24 | LIVE | NW + NE walls visible, S + E open |
| **NO banner in new walls** | user lock 2026-05-24 | LIVE | Banner reference in wall sprite spec |
| **Doorway = empty void** | user lock 2026-05-24 | LIVE | Wood door reference in doorway spec |
| **Torch = separate object** | user lock 2026-05-24 | LIVE | Torch alcove baked into wall sprite (user adds via PixelLab) |
| **Karakter 64x64 chibi** | Karar #114 LOCK | LIVE | Character canvas other than 64x64 (or 120x120 PixelLab raw) |
| **Top-down 70-80 degree** | project_topdown_pivot_lock | LIVE | "True isometric 30-45" reference as current target (use only as ref to old, not active) |

### Code/Unity conventions (priority LOW)

| Rule | Source | Check |
|---|---|---|
| URP 2D Renderer | PROJECT_RULES.md S59 LOCK | Scene uses URP 2D Renderer for new work |
| Pixel Perfect Camera | PROJECT_RULES.md S59 LOCK | Test scenes have Pixel Perfect Camera |
| Sprite import PPU 64 | sprite import convention | PPU 64 specified in import settings |
| Pivot Bottom | sprite import convention | Pivot = Bottom for Y-sort compatibility |
| Filter Point | sprite import convention | Filter Mode = Point (no anti-alias) |
| Compression None | sprite import convention | Compression = None (pixel art preservation) |
| Fractured chamber asset path | RIMA convention | `Assets/Art/FracturedChamber/` for FC assets |
| Test scene path | RIMA convention | `Assets/Scenes/Demo/` for test scenes |

### Workflow conventions (priority HIGH-MEDIUM)

| Rule | Source | Check |
|---|---|---|
| Orchestrator delegates | feedback_orchestrator_delegate_dont_do_yourself | Orchestrator does no bulk work, uses sub-agents/Codex |
| No PixelLab night autonomous | feedback_no_pixellab_night_autonomous | No overnight/autonomous PixelLab gen |
| Dispatch via cx_dispatch.py | PROJECT_RULES.md | All Codex work via cx_dispatch.py, not direct cx exec |
| `--effort xhigh` | feedback_codex_effort_xhigh_2026_05_24 | Codex dispatch uses xhigh |

## Memory file pointers (relevant to this skill)

```
feedback_codex_imagegen_skill_not_env_var.md   - $imagegen built-in tool mode
feedback_codex_effort_xhigh_2026_05_24.md      - cx_dispatch --effort xhigh
feedback_state_task_purpose_explicitly.md      - Amaç: purpose line
feedback_orchestrator_delegate_dont_do_yourself.md - orchestrator delegation
feedback_no_pixellab_night_autonomous.md       - no overnight PixelLab
project_pillar_seam_cover_lock_2026_05_24.md   - S101 PILLAR-LESS REVOKED
project_topdown_pivot_lock.md                  - 70-80 deg top-down (3/4 view)
project_modular_pipeline_lock.md               - modular asset pipeline
project_weapon_system_8dir_lock.md             - 8-dir weapon system
```

## Adding new rules

When you add a new lock or revoke an old one:

1. Update the canonical memory file under `~/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/`
2. Add or revise the row in this catalog (under the right priority section)
3. Add or revise the check in `scripts/check_violations.py`
4. Test against a known-bad file to confirm the check fires
5. Increment the skill version in SKILL.md if behavior changes meaningfully
