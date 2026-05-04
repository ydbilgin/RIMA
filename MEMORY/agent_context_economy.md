---
topic: agent_context_economy
updated: 2026-05-04
---

# Agent Context Economy

Use when: CURRENT_STATUS bloat, CODEX.md/CLAUDE.md size, task planning rules, lessons files,
subagent workflow, token savings.

## Rule

Keep always-loaded files compact:
- `CURRENT_STATUS.md`
- `CLAUDE.md`
- `CODEX.md`

Put detail in topic docs and leave pointers.

## Current Status Rule

`CURRENT_STATUS.md` should contain:
- active block
- latest compact changes
- next priorities
- critical risks
- pointer list

Move long history to `MEMORY/status_snapshot_*` or topic-specific docs.

Latest archived detailed status snapshot:
- `MEMORY/status_snapshot_current_2026_05_03_before_compact.md`

## CODEX / CLAUDE Rule

Do not paste generic long workflow manuals wholesale into always-loaded project files.
Convert useful rules into short project-specific instructions.

## AI-Facing Doc Rule

If the user is not the audience, write compact operational context:
- decision
- why
- source/pointer
- action
- avoid

Do not optimize for human tutorial reading. Optimize for agent recall without context drift.

## Assessment Of forrestchang/andrej-karpathy-skills

Repo reviewed:
- https://github.com/forrestchang/andrej-karpathy-skills

Useful ideas:
- think before coding
- write concise plans for non-trivial work
- maintain a lessons loop after user corrections
- verify before marking done
- prefer simple, root-cause fixes

Not suitable as-is for RIMA:
- "Use subagents liberally" conflicts with RIMA authority/context discipline. In RIMA, Claude
  dispatches subagents; Codex does not spawn agents unless explicitly authorized.
- Always writing `tasks/todo.md` for every task would create more context clutter.
- "Check in before implementation" conflicts with Codex autonomy for clear bounded fixes.
- Dropping the whole CLAUDE.md block into this project would violate the existing lean
  `CLAUDE.md` / `CURRENT_STATUS.md` rule.

Adopted approach:
- Short context-economy and verification rules go into `CODEX.md`.
- Detailed workflow thinking lives here in memory.

## Assessment Of mksglu/context-mode

Repo reviewed:
- https://github.com/mksglu/context-mode

Detailed RIMA evaluation:
- `STAGING/CONTEXT_MODE_RIMA_EVALUATION_2026-05-04.md`

Useful ideas:
- sandbox large tool/log/API/MCP output and return only summaries/search results
- use programmed analysis for large file scans instead of reading raw output into context
- track session savings with `ctx stats`
- treat session-level continuity separately from durable RIMA `MEMORY/`

Do not adopt as-is:
- do not replace RIMA `AGENTS.md`
- do not paste upstream generic routing files into always-loaded RIMA files
- do not enable aggressive terse output for design/balance judgment
- do not assume Codex/Antigravity get the same automatic hook savings as Claude Code

Adoption status:
- hold install; do not set up context-mode now
- extract useful habits from the tweet/repo instead of adopting the tool
- install/pilot only if a later explicit review reopens the question

Adopted habits:
- treat context savings as three layers: terminal noise, assistant verbosity, MCP/tool output bloat
- for large scans/logs/API reads, program the analysis and return only counts, paths, failures, or short summaries
- keep implementation/QC reports compact; keep design/balance reviews nuanced
- measure local savings before believing external percentage claims
- avoid raw build/test/install dumps unless the exact failing lines matter
