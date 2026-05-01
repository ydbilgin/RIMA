---
description: Token-efficient RIMA knowledge lookup — check memory before opening heavy files.
allowed-tools: Read, Glob, Grep
---

# /query [question] — Token-Efficient Knowledge Access

Rule: **check memory first, open files second.**

## Step 1 — Scan Memory Index

Read: `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/MEMORY.md`

Is there a relevant memory entry? If yes, read that file. **If sufficient — stop here. Do not load heavy docs.**

## Step 2 — No Match? Use the Document Map

From CLAUDE.md, pick only the matching file:

| Topic | File |
|---|---|
| Class skill detail | `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` |
| Approved decisions | `TASARIM/MASTER_KARAR_BELGESI.md` |
| Mob/combat design | `TASARIM/COMBAT_ROSTER.md` |
| Boss design | `TASARIM/BOSS_DESIGN.md` |
| Room mechanics | `TASARIM/ROOM_MECHANICS.md` |
| Animation | `TASARIM/ANIMATION_REDESIGN.md` |
| Active phase scope | `TASARIM/FAZLAR/FAZ[N]_*.md` |
| General game design | `TASARIM/GDD.md` |

**Load only the matching file. Do not touch others.**

## Step 3 — Answer and Release

Give the answer. Do not hold the full file in context after extracting the information.

## Step 4 — Save to Memory (optional)

If this question is likely to recur and the answer is non-obvious, append it to the appropriate memory file.

---

**Usage:** `/query which warblade skill applies stagger?`
