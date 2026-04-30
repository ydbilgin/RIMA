---
name: Orchestra discipline — don't do mechanical work on Opus
description: When orchestrator (Opus) is on a multi-file rename or any pure text-replace pass, dispatch to rima-codex instead of editing inline.
type: feedback
---

# Orchestra discipline — Opus does not do mechanical replaces

**Rule:** If the task is a pure text-replace / rename / mechanical refactor across multiple files (or even one file with N+ identical edits), the orchestrator MUST dispatch to **rima-codex**. Do NOT inline-Edit on Opus.

**Why:** 2026-05-01 — user flagged that the S43 dirty-pass rename (15 renames × 7 files, ~25 Edit calls) was done entirely on Opus 4.7 main thread. CLAUDE.md Orchestra Rule explicitly forbids this: "Claude main thread is the orchestra conductor. It dispatches; it does not do mechanical work itself." Opus tokens + cache miss = waste when Sonnet/cx CLI does the same work cheaper.

**How to apply:**
1. **Opus phase (correct):** design judgment — which renames make sense, tone consistency, identity-fit, drop weak candidates. This IS Opus work.
2. **Dispatch phase (was missed):** package the approved rename mapping as a clear instruction to rima-codex:
   - Exact mapping table (old → new)
   - List of files to touch (active only; ARCHIVE/ excluded)
   - Output format (commit message, summary)
3. **rima-codex executes** via `cx sonnet exec` — mechanical sed-like replaces, single commit.
4. **QC phase:** Opus or rima-qc spot-checks the diff, signs off.

**Threshold:** if Edit call count would exceed ~5 on a single mechanical pass, stop and dispatch. Single targeted edit (1-2 surgical changes) is fine inline.

**Related agents:**
- rima-codex — mechanical Codex execution via cx CLI
- rima-qc — diff review after Codex output
- Gemini — currently RIMA-bound to playtest MCP only, NOT for text replace

**Token math:** Opus 4.7 inline Edit pass with ~25 calls + tool result reads ≈ several K tokens at Opus rate. Same task on rima-codex (Sonnet) ≈ 1/5 cost, no orchestrator context bloat.
