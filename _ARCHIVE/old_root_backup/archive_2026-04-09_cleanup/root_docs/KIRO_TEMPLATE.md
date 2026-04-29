# KIRO TASK — [Task Name]
*Date: YYYY-MM-DD | Read this file, apply in order. Do not read other files.*

---

## RISK LEVEL: LOW
> This task may only be given to Kiro if ALL of the following are true:
> - Deterministic: same input produces same output
> - Mechanical: no judgment or interpretation required
> - Isolated: touches only the files listed in this document
> - Bounded: all steps are explicit, no scope expansion possible
> - Mechanically verifiable: QC can be done without interpretation
>
> If any of the above is false → this task stays with Claude Code.

---

## CREDENTIALS

**PixelLab Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## CONTEXT

[1-3 sentences: what needs to be done. No interpretation required.]

---

## FILES TOUCHED (complete list)

- `[exact path 1]`
- `[exact path 2]`

Do not touch any file not listed above. If you need to touch an unlisted file, stop and report to Claude.

---

## STOP AND ESCALATE — Report to Claude if:

- Any step requires a decision or judgment
- Any output looks unexpected or ambiguous
- You are about to touch a file not listed above
- A scope change appears
- QC criteria cannot be mechanically verified
- You encounter an error not covered by these instructions
- The task requires reading any other project file

---

## MANDATORY QC PROTOCOL — APPLY AFTER EVERY OUTPUT

After each generation call:

1. **Read the output file** (e.g. `frame_000.png`)
2. **Describe exactly what you see** — action, direction, pose
3. **PASS criteria:** [exact pass definition — no interpretation required]
4. **FAIL criteria:** [exact fail definition]
5. On **FAIL:** re-generate with `-v2` suffix, add more specific description
6. On **PASS:** save files, proceed to next

**Never save frames that fail QC.**
**If retries do not resolve a failure, stop and report to Claude.**

---

## STEP 0 — Find Character IDs

```
mcp__pixellab__list_characters()
```

Note the IDs for: [Character names]

---

## TASK 1 — [Task name]

[Exact steps. No vague instructions.]

```
mcp__pixellab__animate_character(
  character_id="[ID]",
  animation_name="[english-name-with-hyphens]",
  direction="[direction]",
  n_frames=[N],
  action_description="[English — specific: action, direction, outfit, weapon]"
)
```

**QC pass criteria:** [exact]

**Save path:**
```
Assets/[path]/[direction]/frame_000.png
...
Assets/[path]/[direction]/frame_00N.png
```

---

## TASK 2 — [Task name]

[Repeat pattern]

---

## CLEANUP STEP — Run when all tasks are DONE (not on FAILED/PARTIAL)

Move source/temp files to backup. Keep final Unity assets in place.

```
Move: [exact source paths — e.g. downloaded ZIPs, staging folders]
  → F:\Antigravity Projeler\2d roguelite\_BACKUP\[task_name]_[YYYYMMDD]\

Do NOT move: final files already placed under RIMA\Assets\
```

Use file system move (not copy). If _BACKUP subfolder doesn't exist, create it.

---

## REPORT FILE — Write before saying anything to user

> **MANDATORY.** Write this file when all tasks are done OR stopped.
> Path is always the same — Claude reads this file after every Kiro batch.

**Write to:** `F:\Antigravity Projeler\2d roguelite\KIRO_LAST_REPORT.md`

File contents:
```
# KIRO REPORT — [Task Name]
Date: YYYY-MM-DD

STATUS: DONE / FAILED / PARTIAL

COMPLETED:
  - Task 1 — [one line result]
  - Task 2 — [one line result]

ERRORS:
  - [exact error message + which task] or NONE

QC_RESULT:
  - [Character/Asset] — PASS / FAIL — [one line reason]

CLEANUP:
  - DONE — moved [X] items to _BACKUP\[folder] or SKIPPED (task not complete)

NEXT_SIGNAL: "[exact phrase to say to Claude to trigger next step]"
```

**Overwrite the file every time** — it always holds the last batch result.

---

## AFTER KIRO FINISHES — Claude Code handles

When user says "[trigger phrase]", Claude Code will:
1. Read `KIRO_LAST_REPORT.md`
2. `AssetDatabase.Refresh()`
3. [Build step]
4. [Verification step]
5. Screenshot in play mode
6. Update CURRENT_STATUS
