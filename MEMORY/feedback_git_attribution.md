---
name: Git Attribution
type: feedback
trigger: git, commit, attribution, prefix
description: Commit prefix rules per agent
---

## Rule
Any non-Claude agent editing tracked project files must commit immediately after.

## Prefixes
- Codex: `CODEX: <what changed and why>`
- Antigravity: `ANTIGRAVITY: <what changed and why>`
- Other agents: `<AGENT_NAME>: <what changed and why>`

Claude edits via Claude Code -- no manual git commit needed from Claude.
