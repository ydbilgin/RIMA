---
name: rima-conventions
description: Review RIMA Unity ARPG project files against canonical project rules and detect convention violations. Use this skill aggressively when the user asks to "review against rima rules", "check rima conventions", "$rima-conventions", "scan for violations", "qc this output", or after any Codex/sub-agent dispatch produces output that should be checked against project standards. Also use proactively whenever you finish writing or modifying any RIMA project file (dispatch task .md, memory file, Unity script, asset spec) without being asked — it catches violations before they propagate. Detects missing ACTIVE RULES/NLM ACCESS/Amaç headers in dispatch tasks, non-ASCII Turkish chars in memory bodies, REVOKED rule references like S101 PILLAR-LESS, wrong PPU values, wrong Y-sort axis, banner/torch baked violations, wrong asset paths, and missing dispatch effort flags. Reads .claude/PROJECT_RULES.md, MEMORY/MEMORY.md, CURRENT_STATUS.md, and project memory files as ground truth.
metadata:
  type: project-review
  project: RIMA
---

# RIMA Conventions Review

This skill scans any RIMA project file and reports convention violations. The point is to catch project-rule drift early — before a Codex task ships with the wrong format, before a memory file gets committed with non-ASCII drift, before a wall asset gets specified at the wrong PPU.

## When to use

Trigger this skill any time work touches a RIMA project file:

- After a Codex dispatch finishes and the orchestrator wants to verify the output
- After writing a new memory file in `~/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/`
- After drafting a dispatch task .md in `STAGING/`
- After modifying a Unity script in `Assets/Scripts/`
- When the user explicitly asks "$rima-conventions <file>" or "check this against rima rules"
- Before any git commit involving RIMA project files

You can also run it across many files at once — useful for periodic audits.

## How to use

The skill is a thin Claude wrapper around a Python script. Workflow:

1. Identify target file(s)
2. Run `python ~/.claude/skills/rima-conventions/scripts/check_violations.py <path>` (or pass `--all` to scan project)
3. Parse the violation report
4. Surface findings to the user in priority order with file:line citations and fix steps

If the target file looks like a multi-file artifact (e.g., a Codex output that touched 5 files), run the script against each file.

The script is the source of truth for what counts as a violation. If you find a violation pattern that the script misses, tell the user — the skill needs updating, not workarounds.

## What it checks

Five violation categories, each with its own checks:

### 1. Dispatch task format (`STAGING/codex_*_task.md`, `STAGING/*dispatch*.md`)
- First non-empty line must start with `ACTIVE RULES:`
- `NLM ACCESS:` block must follow within first 10 lines
- `Amaç:` (purpose) line required somewhere in the file
- If file mentions image generation: must say "built-in tool mode" and must NOT mention `OPENAI_API_KEY` as a hard requirement
- If file is a dispatch task: must reference `--effort xhigh` (current lock, see memory)

### 2. Memory file format (`memory/*.md`)
- Frontmatter required with `name`, `description`, `metadata.type`
- Body must be ASCII-only (Turkish chars transliterated)
- New memory must have matching entry in `MEMORY.md` index

### 3. Design lock violations (any file)
- References to **S101 PILLAR-LESS** as live → REVOKED (memory: project_pillar_seam_cover_lock_2026_05_24)
- Wall asset sizes not multiples of 64 → PPU 64 LOCK (Karar #114)
- Y-sort axis other than `(0, 1, 0)` → research lock
- New wall pieces with baked banner → user lock 2026-05-24
- Wood doors in doorway sprites → user lock (empty void interior only)
- Torch alcoves baked into wall sprites → torch must be separate object

### 4. Code/Unity conventions (`Assets/**/*.cs`, sprite import settings)
- Sprite import: PPU 64, Pivot Bottom, Filter Point, Compression None
- Wrong asset path (fractured chamber assets must go to `Assets/Art/FracturedChamber/`)
- Test scenes outside `Assets/Scenes/Demo/`
- Missing URP 2D Renderer references when 2D Light involved

### 5. Workflow violations (dispatch tasks, orchestrator decisions)
- Orchestrator bulk work that should be delegated (memory: feedback_orchestrator_delegate_dont_do_yourself)
- PixelLab autonomous night runs (memory: feedback_no_pixellab_night_autonomous)
- Codex dispatch without explicit purpose line

The exact rule sources, regex patterns, and priority assignment live in `scripts/check_violations.py`. The reference catalog at `references/rima_rules_index.md` summarizes every rule with its source memory file or PROJECT_RULES.md line, so the user can audit the skill's claims.

## Output format

The script emits a markdown report:

```
# RIMA Conventions Review — <filename>

## Summary
- Total violations: N
- HIGH priority: X
- MEDIUM priority: Y
- LOW priority: Z

## Violations

### [HIGH] [DISPATCH] Missing ACTIVE RULES header
**File:** STAGING/codex_xyz_task.md:1
**Rule:** .claude/PROJECT_RULES.md — every dispatch task must start with ACTIVE RULES
**Found:** First line is "# Codex Task — ..." instead of "ACTIVE RULES: ..."
**Fix:** Prepend the standard ACTIVE RULES line as the first line. See PROJECT_RULES.md section "Sub-agent dispatch'inde her zaman ilk satır olarak inline ekle".

[... more violations ...]
```

Pass through the script output verbatim to the user, then add a one-paragraph orchestrator summary at the end (which violations matter most, which can be batched, whether the file is shippable as-is or needs revision).

## Updating the rule set

Project rules drift over time. When the user reports a new lock or revokes an old one:

1. Update the relevant memory file in `~/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/`
2. Update `references/rima_rules_index.md` here
3. Update or add the check in `scripts/check_violations.py`
4. Re-test against a known-bad file to confirm the check fires

Do NOT hardcode rules in the SKILL.md body. The script + reference catalog are the single source of truth so updates land in one place.

## Limitations

- The script uses regex, not a full markdown parser — pathological formatting can confuse it
- It cannot check semantic correctness (e.g., whether a Wang16 tile actually wang-tiles), only structural rules
- Design lock violations are detected by keyword matching; nuanced cases (e.g., "discussing S101 historically vs. asserting it as live") may produce false positives — review HIGH violations before acting

When in doubt, treat the script's output as a starting point for review, not a final verdict.
