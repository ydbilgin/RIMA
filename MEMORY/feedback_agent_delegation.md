---
name: Agent Delegation
type: feedback
description: Role boundaries for Claude, Codex, Gemini, and Antigravity
---

## PlayMode Tests
- Claude writes tests and reviews failures.
- Codex may run Unity MCP tests inside a defined task and report results.
- Gemini CLI is not the default Unity MCP executor for this project.

## Design Decisions
- Claude owns class, skill, boss, combat, and architecture decisions.
- Codex executes bounded mechanical tasks only.
- Antigravity supports asset work and analysis; it does not make final design decisions.
