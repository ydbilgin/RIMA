---
description: Format and write a Codex delegation task to CODEX_TASKS.md. Use after passing the delegation gate check.
allowed-tools: Read, Write, Edit
---

# /codex-task — Codex Task Writer

Format a task for Codex delegation and append it to `CODEX_TASKS.md`.

## When to Use

- Work is mechanical, isolated, and has clear boundaries
- No architectural decision required
- Can be: file import, anim setup, prefab wiring, doc update, SO creation, isolated C# script

## Delegation Gate (all must be YES)

- [ ] Deterministic? (one correct answer, no judgment calls)
- [ ] Mechanical? (no design decisions)
- [ ] Isolated? (touches ≤ 3 files, no cross-system side effects)
- [ ] Boundaries defined? (input/output clearly specified)

If any NO → Claude handles it, not Codex.

## Protocol

### Step 1 — Draft the Task

Fill in all fields:

```markdown
## [Task ID] — [Task Title]

**Status:** PENDING
**Priority:** High / Medium / Low
**Assigned:** Codex
**Estimated:** [~N files / ~N lines]

### Goal
[One sentence: what must exist or change when done]

### Inputs
- [file or asset that already exists]
- [spec or constraint]

### Outputs
- [file to create or modify]
- [expected state/content]

### Acceptance Criteria
- [ ] [verifiable condition]
- [ ] [verifiable condition]

### Constraints
- Namespace: <PROJECT_NAMESPACE> (default: RIMA)
- No new dependencies
- [any other hard constraint]

### Context
[One short paragraph: why this exists, what system it connects to]
```

### Step 2 — Check Existing Tasks

Read `CODEX_TASKS.md`. Is there a duplicate or conflicting task? If yes, update existing rather than adding a new one.

### Step 3 — Append

Add the formatted task to `CODEX_TASKS.md`. Preserve existing task IDs.

### Step 4 — Confirm

Show the user the task and say: "Added to CODEX_TASKS.md as [Task ID]."

## Common Mistakes

- Giving Codex a task that requires a design decision ("pick the best approach")
- Vague acceptance criteria ("works correctly")
- Missing namespace/constraint info → Codex produces non-RIMA code
- Not checking for duplicate tasks before adding
