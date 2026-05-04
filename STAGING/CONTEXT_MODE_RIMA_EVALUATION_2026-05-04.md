---
title: Context Mode RIMA Evaluation
date: 2026-05-04
status: hold_install_extract_lessons
sources:
  - user_tweet_paste_2026-05-04
  - https://github.com/mksglu/context-mode
  - antigravity_opus_feedback_2026-05-04
---

# Context Mode RIMA Evaluation

## User Raw Signal

User shared a Twitter/X post claiming three repos reduce Claude Code cost:
- RTK: terminal output filtering.
- Caveman Claude: terse output behavior in `CLAUDE.md`.
- Context Mode: MCP/tool output sandboxing into SQLite with summarized context.

Target repo:
- https://github.com/mksglu/context-mode

Primary claim to verify:
- Context Mode stores raw MCP/tool output locally, indexes it in SQLite/FTS5, and returns small summaries/search results to the model.
- Claimed best fit: Playwright, browser/API calls, multiple MCP servers, large tool responses.

## Verified From Repo README

Useful repo claims:
- Claude Code plugin install exists via plugin marketplace.
- MCP-only install exists via `claude mcp add context-mode -- npx -y context-mode`.
- Tools include `ctx_batch_execute`, `ctx_execute`, `ctx_execute_file`, `ctx_index`, `ctx_search`, `ctx_fetch_and_index`, plus stats/doctor/upgrade/purge/insight.
- Claude Code has full hook support per README: PreToolUse, PostToolUse, PreCompact, SessionStart.
- Codex CLI has MCP support and hook scripts; README says hooks are stable, but PreToolUse argument rewriting remains limited by upstream `updatedInput` support.
- Antigravity is MCP-only/no hooks; no session event capture.
- License is Elastic License 2.0, not MIT.

Important caveats:
- Context Mode is not a simple transparent shell output filter for every environment.
- Hook coverage differs by platform.
- Without hooks, compliance relies on model instructions and explicit use of ctx tools.
- Third-party MCP execution tools add another sandbox/security layer; do not assume it matches RIMA/Codex approval semantics.

## Antigravity Opus Feedback Summary

Agree:
- RIMA has real context pressure from Unity project size, long command output, grep/find/cat noise, and MCP/tool-heavy sessions.
- Think-in-code is a strong fit for mechanical audits like prefab checks, sorting layer scans, asset import validation, and cross-file counts.
- Context Mode and RIMA `MEMORY/` do not occupy the same layer:
  - `MEMORY/` = durable project memory.
  - Context Mode = session-level tool-output indexing and compaction support.
- Do not use aggressive output compression for design/balance judgment.
- Track savings via `ctx_stats`.

Adjust:
- "Every rima-codex task uses Claude Code CLI" is not guaranteed in current RIMA rules. `rima-codex` may wrap Codex CLI via `cx <profile> exec`; Context Mode support depends on that client and active hook/MCP setup.
- "SQLite session DB may be in project .context-mode" needs verification before `.gitignore` change. Repo README says SQLite DBs live in the home directory, but local install behavior should be checked with `ctx doctor`.
- Start with Claude Code plugin or MCP-only depends on risk appetite:
  - MCP-only is safer for a first read-only pilot.
  - Plugin is where automatic savings actually happen for Claude Code.

## RIMA Adoption Position

Recommended: pilot, not immediate full adoption.

Pilot order:
1. Claude Code read-only pilot:
   - install MCP-only or plugin in a separate Claude session.
   - run `ctx doctor`.
   - run one known heavy read/search task.
   - run `ctx stats`.
2. Compare against current RIMA workflow:
   - direct shell/rg output vs ctx_batch_execute/ctx_execute_file summary.
   - compaction/session continuity behavior.
   - CPU/process cleanup on Windows.
3. If useful, adopt a compact RIMA rule:
   - For large analysis/search/log/API/MCP output, prefer programmed analysis and summarized results.
   - Do not paste the upstream context-mode `AGENTS.md` into RIMA root.
   - Do not enable caveman compression for design/balance sessions.
4. Only after pilot PASS:
   - add any needed ignore rule if local files appear in project.
   - add minimal rules to `CLAUDE.md` or `CODEX.md` only if necessary.
   - keep detailed usage guidance in `MEMORY/agent_context_economy.md`.

## Suggested Claude Review Prompt

Claude, review this Context Mode adoption proposal for RIMA.

Inputs:
- User shared tweet claims: RTK filters terminal noise, Caveman Claude compresses output, Context Mode sandboxes MCP/tool output into SQLite and returns summaries.
- Repo: https://github.com/mksglu/context-mode
- Local RIMA memory rule: do not paste generic workflow manuals into always-loaded `CLAUDE.md`/`CODEX.md`; convert useful ideas into short RIMA-specific memory rules.
- RIMA context pressure: Unity project, dirty worktree, long shell outputs, MCP/Unity instability, many STAGING/TASARIM/MEMORY docs.
- Current RIMA memory layers: `CURRENT_STATUS.md`, `MEMORY/INDEX.md`, scoped `MEMORY/project_*.md` pointers.
- Antigravity Opus feedback says Context Mode is useful for RIMA, especially rima-codex/mechanical tasks, but warns about aggressive output compression, hook fragility, possible SQLite files, and overlap with MEMORY.

Questions:
1. Should RIMA pilot Context Mode now?
2. Should the pilot be Claude Code plugin, Claude MCP-only, Codex MCP-only, or all separately?
3. What exact PASS/FAIL criteria should gate adoption?
4. Which RIMA rules should be adopted if pilot passes?
5. Which upstream instructions should be rejected to avoid conflict with RIMA orchestration?

Requested output:
- Decision: PILOT / HOLD / REJECT.
- Pilot plan: 5-8 concrete steps.
- RIMA adoption rules: max 8 bullets.
- Files to edit only after pilot PASS.
- Risks and rollback.

## Updated Decision

Decision: HOLD INSTALL.

Do not install Context Mode now.

Useful items to extract:
- From the tweet: separate context savings into three layers:
  - terminal noise
  - assistant output verbosity
  - MCP/tool output bloat
- From Context Mode: prefer programmed analysis and summarized output for large searches/logs/API reads.
- From Context Mode: measure savings before adopting tools; do not trust headline percentages without local RIMA data.
- From Caveman-style output: keep implementation/QC reports compact, but keep design/balance reviews nuanced.
- From RTK-style terminal filtering: avoid dumping raw build/test/install output into agent context; summarize failures and counts.

RIMA application:
- Add compact habits to `MEMORY/agent_context_economy.md`.
- Do not install or configure context-mode unless a later Claude/Codex review explicitly asks for a pilot.
- Do not add upstream context-mode routing files to RIMA.
- Do not edit `CLAUDE.md`, `CODEX.md`, or `AGENTS.md` for context-mode now.

Do not yet:
- Replace RIMA `AGENTS.md`.
- Paste upstream `configs/codex/AGENTS.md` into project root.
- Enable aggressive output compression for design review.
- Route destructive or security-sensitive commands through `ctx_execute`.
