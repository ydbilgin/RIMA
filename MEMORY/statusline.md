---
name: Claude Code statusline configuration
type: feedback
trigger: statusline, usage_statusline.py, usage display, percentage
description: Statusline script paths and fixed parsing rules
---

Script: C:/Users/ydbil/.claude/usage_statusline.py
Launcher: C:/Users/ydbil/.claude/usage_statusline.bat
Settings: C:/Users/ydbil/.ccs/instances/claude-laurethgame/settings.json

## Fixed (2026-04-29)
- Username: read from transcript_path instance name (e.g. claude-laurethgame -> laurethgame).
  Do NOT use ~/.claude.json -- it holds personal account (ydbilgin), not the active project instance.
- Percentage display: use int() cast. used_percentage can arrive as 28.000000000000004.

## Rule
Preserve transcript_path parsing for username and int() cast for percentages on any future edit.
