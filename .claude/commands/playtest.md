---
description: Generate a self-contained Gemini CLI playtest task. Claude designs the scenarios, Gemini executes via MCP, results written to STAGING/playtest_report.md.
allowed-tools: Read, Write, ReadMcpResourceTool
---

# /playtest — RIMA Playtest Task Generator

Claude designs and writes the playtest task. Gemini CLI executes it via the same MCP server.

## Architecture

```
Claude       → designs scenarios
Codex        → reviews scenario design (completeness, edge cases)
Claude       → finalizes task prompt for Gemini
Gemini CLI   → connects to UnityMCP (same server, port 6401)
             → executes MCP tool calls
             → writes report to STAGING/playtest_report.md
Codex        → reviews result report (consistency, missed checks)
Claude       → reads Codex review → final decision
```

## When to Use

- After integrating a new system (BossAI, VFX, new enemy)
- "Is the game broken?" quick check
- Before phase close when formal tests don't cover a scenario
- Formal PlayMode tests are the first choice — use /playtest for what they can't cover

## Protocol

### Step 1 — Check Editor State

Read `mcpforunity://editor/state` — confirm Unity is idle and not compiling.

### Step 2 — Write Scenario Draft + Codex Review Request

Write a brief scenario list to `STAGING/playtest_scenarios_draft.md`.
Include for each scenario: what it tests, expected values, pass condition.

Then output this to the user:

```
Codex review request — paste to Codex (GPT-5.5):

Review this playtest scenario list for RIMA (Unity 2D roguelite).
Check: are there missing edge cases? wrong expected values? scenarios that will always pass trivially?
File: STAGING/playtest_scenarios_draft.md
Return: APPROVED or list of corrections.
```

Wait for user to confirm Codex approved before proceeding.

### Step 3 — Generate Gemini Task Prompt

Write the following prompt to `STAGING/gemini_playtest_task.md`, then show it to the user:

---

```markdown
# RIMA Playtest Task

You are connected to UnityMCP via MCP tools (same server Claude uses — port 6401).
Execute the following steps. Write your final report to:
  STAGING/playtest_report.md

---

## Pre-flight

1. Read editor state via resource `mcpforunity://editor/state`
   - If `is_compiling: true` → wait, retry after 5s
   - If `ready_for_tools: false` → stop and report why

2. Start play mode:
   Tool: manage_editor  action=play

3. Wait 2 seconds for scene to load (Awake/Start).

---

## System Check (execute_code for each)

Run each block. After each one, call read_console and note any [Error] or [Exception].

**Check A — Core systems**
```csharp
bool ok = RuntimeRoomManager.Instance != null
       && DraftManager.Instance != null
       && DungeonGraph.Instance != null;
int nodes = DungeonGraph.Instance?.TotalNodes ?? -1;
Debug.Log($"[PT-A] CoreSystems={ok}, Nodes={nodes}");
```

**Check B — Player components**
```csharp
var p = GameObject.FindGameObjectWithTag("Player");
bool hasAll = p != null
           && p.GetComponent<Health>() != null
           && p.GetComponent<RageSystem>() != null
           && p.GetComponent<PlayerController>() != null;
Debug.Log($"[PT-B] Player={p != null}, Components={hasAll}");
```

**Check C — Rage system**
```csharp
var rage = GameObject.FindGameObjectWithTag("Player")?.GetComponent<RageSystem>();
if (rage != null) {
    rage.AddRage(50);
    Debug.Log($"[PT-C] Rage={rage.CurrentRage}, IsFury={rage.IsFury}");
} else Debug.LogError("[PT-C] RageSystem null");
```

**Check D — Draft flow**
```csharp
DraftManager.Instance.ShowDraft();
bool active = DraftManager.Instance.IsDraftActive;
DraftManager.Instance.HideDraft();
bool ts = Mathf.Approximately(Time.timeScale, 1f);
Debug.Log($"[PT-D] DraftActive={active}, TimeScaleRestored={ts}");
```

**Check E — Force-kill enemies**
```csharp
var enemies = GameObject.FindGameObjectsWithTag("Enemy");
int count = enemies.Length;
foreach (var e in enemies) e.GetComponent<Health>()?.TakeDamage(9999);
Debug.Log($"[PT-E] Killed={count}");
```

**Check F — RiftGlowVFX**
```csharp
var vfx = Object.FindObjectOfType<RiftGlowVFX>();
if (vfx != null) { vfx.SetColor(Color.cyan); vfx.SetCastState(true); Debug.Log("[PT-F] VFX OK"); }
else Debug.LogWarning("[PT-F] RiftGlowVFX not in scene");
```

---

## Console Analysis

Call read_console with filter types: ["error", "warning"].

For each [PT-*] log line, record the values.
For any [Error] or [Exception] NOT from MCP-FOR-UNITY: mark as FAIL.

---

## Stop Play Mode

Tool: manage_editor  action=stop

---

## Write Report

Write the following to STAGING/playtest_report.md:

```
# PLAYTEST REPORT — [date]

## Results

| Check | Result | Value |
|---|---|---|
| A — Core Systems | PASS/FAIL | [CoreSystems=X, Nodes=X] |
| B — Player Components | PASS/FAIL | [Player=X, Components=X] |
| C — Rage System | PASS/FAIL | [Rage=X, IsFury=X] |
| D — Draft Flow | PASS/FAIL | [DraftActive=X, TimeScaleRestored=X] |
| E — Enemy Kill | PASS/FAIL | [Killed=X] |
| F — VFX | PASS/WARN | [note] |

## Errors Found
[list any [Error]/[Exception] with system name, or "None"]

## Warnings
[list or "None"]

## Overall: PASS / FAIL
[summary sentence]
```
```

---

### Step 3 — Hand Off

Tell the user:

```
Gemini task written to: STAGING/gemini_playtest_task.md

Run in terminal:
  gemini < "STAGING/gemini_playtest_task.md"

Or paste the contents into Gemini CLI.
When done, run /playtest-results to read the report.
```

---

## After Gemini Runs — Codex Review + Final Interpretation

1. Read `STAGING/playtest_report.md`

2. Output this to the user for Codex:

```
Codex review request — paste to Codex (GPT-5.5):

Review this RIMA playtest report for consistency and missed issues.
Check: do the PASS/FAIL verdicts match the logged values? any suspicious results?
any system that should have been checked but wasn't?
File: STAGING/playtest_report.md
Return: CONSISTENT or list of discrepancies.
```

3. Wait for Codex response. Then Claude interprets:
- Any FAIL or discrepancy → identify system, investigate root cause
- All PASS + Codex CONSISTENT → confirmed clean
- Clean up: delete `STAGING/gemini_playtest_task.md`, `STAGING/playtest_scenarios_draft.md`
- Keep `STAGING/playtest_report.md` until next playtest run

## Token Note

Claude writes ~1 prompt file. Gemini does all MCP execution.
Claude reads ~1 result file. Total Claude cost: ~500 tokens vs ~5000 for direct execution.
