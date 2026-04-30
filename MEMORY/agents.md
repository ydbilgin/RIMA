---
name: Agent authority and routing
type: feedback
trigger: routing, delegate, authority, Codex, Gemini, playtest
description: Authority order and routing gates for RIMA agents
---

## Authority Order (highest to lowest)
1. Claude -- final decisions, architecture, QC judgment, orchestration. Non-delegatable.
2. Codex (GPT-5.5 high) -- isolated C# implementation, MCP execution, run_tests, bounded tasks
3. Antigravity (Gemini Pro / Claude Opus) -- asset prompts, analysis, STAGING/ writes
4. Gemini CLI -- web research only (no MCP, no design, no code)
5. Ollama -- local log analysis, offline prep only

## Core Routing Rules
- PlayMode/EditMode tests -> Codex (MCP run_tests). Never Gemini.
- Playtest design + execution -> Codex. Never Gemini CLI.
- Web/external research -> Gemini CLI only. No Unity MCP, no design decisions.
- Class/skill/boss/combat design -> Claude. Non-delegatable.
- Cross-file refactor, architecture decision -> Claude. Non-delegatable.
- Final QC judgment (PASS/FAIL) -> Claude. Non-delegatable.
- Scope drift in any agent -> stop and report to Claude immediately.

## Delegation Gate (delegate to Codex only when ALL YES)
- Deterministic output
- Mechanical task
- Isolated files (no cross-system changes)
- Defined file boundaries

Spawn threshold: 1-2 line direct edit -> Claude. Everything else -> relevant agent.
