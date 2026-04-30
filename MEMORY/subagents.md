---
name: Claude sub-agent table
type: feedback
trigger: sub-agent, rima-doc, rima-qc, rima-asset, general-purpose
description: Claude sub-agent scopes and write permissions
---

## Sub-Agents (.claude/agents/)
| Agent | Model | Scope | Writes |
|---|---|---|---|
| rima-design | Opus 4.7 | class/skill/boss/room design decisions | no |
| rima-doc | Sonnet 4.6 | doc write/update: status, guide, memory, archive | yes |
| rima-qc | Sonnet 4.6 | validate + report: code, visual, lint | no |
| rima-asset | Sonnet 4.6 | PixelLab/ChatGPT prompts, writes to STAGING/ | STAGING/ only |
| general-purpose | Sonnet 4.6 | prompt-only output, no file reads needed | if specified |

No sub-agent may spawn another sub-agent.
Token rule: agent at scope boundary escalates to Claude, does not touch other agents' work.
