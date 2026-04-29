---
description: RIMA phase close protocol — run tests, sync docs, archive completed tasks, prepare commit. Run before moving to the next phase.
allowed-tools: Read, Glob, Grep, mcp__UnityMCP__run_tests, mcp__UnityMCP__get_test_job, mcp__UnityMCP__manage_scene, ReadMcpResourceTool
---

# /phase-close — RIMA Phase Close Protocol

Run this when a phase is declared complete. Follow steps in order — do not skip ahead.

## When to Use

- "Phase X is done" decision made
- Before entering the next phase scope
- End-of-sprint review

## Protocol

### Step 1 — Test Gate

Run EditMode → all tests PASS?
- Any FAIL → fix first, re-run. Phase does not close.
- If PlayMode tests exist → run those too.

### Step 2 — Update CURRENT_STATUS

Check and update:
- Clear completed items from Priority Queue
- Update Anchor Status table to current state
- Update Script Status table
- Change active block heading to next phase
- Update date

### Step 3 — SYSTEM_MAP Sync

Verify:
- New scripts present in tables?
- Test count current?
- VFX/system sections complete?
- `Last Updated` is today?

### Step 4 — Doc Debt Review

Scan "Pending Doc Tasks" in CURRENT_STATUS:
- Critical items (SYSTEM_MAP, MASTER_KARAR_BELGESI) → close here
- Low-priority items (FAZ3, FAZ5, stale docs) → write as Codex tasks

### Step 5 — Archive

Completed CODEX_TASKS.md files and temp files:
- Move to `ARCHIVE/CODEX_TAMAMLANDI/`
- Clean up stale entries in `STAGING/`

### Step 6 — Save Scene

If Unity is open → `manage_scene action=save`

### Step 7 — Report

```
## PHASE [N] CLOSE REPORT — [date]

### ✅ Completed
- [item]

### 🔴 Deferred to Next Phase
- [item]: [reason]

### Test Status
- EditMode: X/X PASS
- PlayMode: X/X PASS (or "not yet written")

### Next Phase Scope
[summary from FAZ_MASTER]

**Phase closed. Proceed?**
```

## Common Mistakes

- Closing phase with failing tests
- Moving to next phase without archiving CODEX_TASKS.md
- Forgetting SYSTEM_MAP update (most frequently skipped step)
