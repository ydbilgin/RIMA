---
description: RIMA knowledge base health scan — detect conflicts, stale entries, and orphan memory files. Output a cleanup list.
allowed-tools: Read, Glob, Grep
---

# /lint — RIMA Knowledge Base Health Scan

Run without asking the user. Scan and report directly.

## Step 1 — Load Index

Read:
- `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/MEMORY.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/CURRENT_STATUS.md`

## Step 2 — Scan Critical Docs

Read all in parallel:
- `TASARIM/MASTER_KARAR_BELGESI.md`
- `TASARIM/FAZLAR/FAZ_MASTER.md`
- `TASARIM/ANIMATION_REDESIGN.md`
- `TASARIM/GDD.md` (first 100 lines only)

## Step 3 — Conflict Check

| Check | Source A | Source B |
|---|---|---|
| Class roster (10 classes?) | MEMORY: project_rima.md | MASTER_KARAR_BELGESI |
| Active phase | CURRENT_STATUS | FAZ_MASTER |
| Sprite system (body+weapon separate?) | MEMORY: project_character_system.md | ANIMATION_REDESIGN |
| PixelLab pipeline status | MEMORY: project_pixellab_pipeline.md | MASTER_KARAR_BELGESI |
| Cross-class skill slots (2 slots?) | MEMORY: project_cross_class_skills.md | GDD |

## Step 4 — Stale Memory Detection

For each entry in MEMORY.md:
- Marked "DONE" but still active in CURRENT_STATUS?
- Project memories older than 2026-04 — still valid?
- Duplicate or overlapping memory files?

## Step 5 — Orphan Check

For each entry in MEMORY.md, verify the physical file exists (use Read to check).

## Step 6 — Report

```
## LINT REPORT — [date]

### 🔴 Conflicts (fix immediately)
- [A vs B: description]

### 🟡 Stale Entries (needs update)
- [memory file: why stale]

### 🟢 Orphans / Missing Files
- [listed in MEMORY.md but file not found]

### ✅ Clean
- [checked, no issues]

### Recommended Actions
1. [action]
```

Show report, then ask "Which should we fix?"
