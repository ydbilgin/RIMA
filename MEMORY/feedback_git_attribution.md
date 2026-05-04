---
name: Git Attribution and Commit Frequency
type: feedback
trigger: git, commit, attribution, prefix, status, bloat
description: Commit prefix rules per agent + commit-often rule to keep git status lean
---

## Commit Frequency Rule
Commit after every logical unit of work — do not let untracked/modified files accumulate.
**Why:** `git status` is auto-loaded into every message. 40+ pending files = ~3-4K extra tokens per turn.
**How to apply:** After completing a doc pass, agent run, or Unity code batch, immediately commit. Suggest it to the user if status looks bloated.

## Attribution Prefixes
Any non-Claude agent editing tracked project files must commit immediately after.
- Codex: `CODEX: <what changed and why>`
- Antigravity: `ANTIGRAVITY: <what changed and why>`
- Other agents: `<AGENT_NAME>: <what changed and why>`

Claude edits via Claude Code -- no manual git commit needed from Claude.
