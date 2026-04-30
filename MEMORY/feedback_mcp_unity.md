---
name: MCP Unity Workflow
type: feedback
trigger: Unity MCP, run_tests, compile, stuck MCP, domain reload
description: MCP server setup, run_tests syntax, compile-wait rule, and stuck-job recovery
---

## Rules
- Provider: CoplayDev uvx mcpforunityserver; default port: 6401.
- Full Unity + MCP server restart required when MCP gets stuck.
- MCP calls during domain reload may queue, drop, or return stale data.
- After script edits, wait for compile completion before next MCP call.

## Test Syntax
```
run_tests mode=PlayMode assembly=RIMA.Tests.PlayMode
run_tests mode=EditMode assembly=RIMA.Tests
```
- Compile errors: fix in test file, re-run. Max 2 attempts.
- Runtime failures from missing scene objects: report as-is, do NOT fix game logic.

## Recovery
- Prefer batch_execute for repeated operations.
- Stuck job: close Unity, restart MCP server, then run read_console.
