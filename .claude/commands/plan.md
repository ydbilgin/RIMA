---
description: Analyze requirements, assess risks, and produce a step-by-step implementation plan for RIMA. Do NOT write any code until user confirms.
allowed-tools: Read, Glob, Grep
---

# /plan — RIMA Implementation Planner

Produce a plan and wait for user confirmation before touching any code or files.

## When to Use

- Starting a new system or feature
- Multiple scripts/scenes will be affected
- Clarifying scope before delegating to Codex
- Architectural change or refactoring
- Requirements are ambiguous

## Protocol

### Step 1 — Load Context

Read in parallel:
- `CURRENT_STATUS.md` — active block and priority queue
- `SYSTEM_MAP.md` — current script/system map

Add if in scope: `TASARIM/MASTER_KARAR_BELGESI.md`, active phase file.

### Step 2 — Restate Requirement

Express the user's request in RIMA context:
- What must the system do?
- Which existing systems does it interact with?
- Which scene/prefab/scripts are affected?

### Step 3 — Delegation Decision

For each work unit:

| Criterion | Claude | Codex |
|---|---|---|
| Contains architectural decision | Claude | — |
| Mechanical + isolated + clear boundaries | — | Codex |
| Requires MCP tooling | Claude | — |
| Pure file/script/doc work | — | Codex |

### Step 4 — Step-by-Step Plan

For each step specify:
- What will be done (one sentence)
- Which file is affected
- Depends on previous step?
- Claude or Codex?

### Step 5 — Risk Assessment

RIMA-specific risks:

| Risk | Mitigation |
|---|---|
| MCP timeout during compile | Wait for compile before calling |
| EditMode vs PlayMode boundary | Awake/Singleton required → PlayMode |
| Scene not saved | Save after every prefab/scene change |
| Codex scope creep | Unclear boundary → escalate to Claude |
| Missing RequireComponent | Check all new MB dependencies |

### Step 6 — Complexity Estimate

`Low` / `Medium` / `High` — with justification.

### Step 7 — Present and Wait

Show the plan. **DO NOT write any code.** Wait for:
- `yes` / `go` → proceed
- `modify: [...]` → revise
- `codex` → convert to CODEX_TASKS.md format

## Output Format

```
## RIMA Plan: [feature name]

### Requirement
[clear description]

### Affected Systems
- Scripts: X, Y
- Prefab/Scene: Z
- Tests: EditMode / PlayMode

### Steps
1. [work] — [Claude/Codex] — [depends: none]
2. [work] — [Claude/Codex] — [depends: Step 1]
...

### Risks
- [risk]: [mitigation]

### Complexity: [Low/Medium/High]
[justification]

**WAITING FOR CONFIRMATION — should I proceed?**
```

## Common Mistakes

- Giving Codex work that contains architectural decisions
- Trying to test Awake()/Singleton code in EditMode
- Forgetting to save scene after hierarchy changes
- Using SceneManager.LoadScene instead of LoadSceneAsync in PlayMode tests
