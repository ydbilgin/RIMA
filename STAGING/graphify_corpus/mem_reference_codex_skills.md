---
name: Codex CLI Skills
description: Available skills for ChatGPT 5.5 in RIMA workflow.
type: reference
---

# SKILLS TABLE
| Skill | RIMA Use Case |
|-------|---------------|
| imagegen | Concept art, VFX ideation, UI mockups (NOT for production) |
| screenshot | Visual QC, scale/pose/handedness checks, proof of work |
| doc / pdf | Summarizing GDD, guides, and contracts |
| jupyter | Numerical balance, RNG tables, combat simulation |

# LOCAL TOOLS
* prefab_health_check.py: Prefab QC
* audit_cleanup.py: Workspace clutter management
* remove_bg.py: Asset transparency cleanup
* comfy_*: Pixel art candidate generation

# ROUTING: CODEX vs CLAUDE
* Codex: UnityMCP wiring, Prefab assembly, Value setting, Scene placement, Testing
* Claude: Debugging, Architecture, Multi-system errors, Complex logic
