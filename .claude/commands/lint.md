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
- `TASARIM/_ARCHIVE_2.5D_2026-05-12/ANIMATION_REDESIGN.md` (ARCHIVED — mob-only referans)
- `TASARIM/GDD.md` (first 100 lines only)

## Step 3 — Conflict Check

| Check | Source A | Source B |
|---|---|---|
| Class roster (10 classes?) | MEMORY: MEMORY.md index (project_rima.md 2026-06-13'te arşivlendi) | MASTER_KARAR_BELGESI Karar #4 |
| Active phase | CURRENT_STATUS | FAZ_MASTER |
| Sprite system (body+weapon separate?) | MASTER_KARAR_BELGESI Karar #144 (weaponless body) | ANIMATION_REDESIGN (archived) |
| PixelLab pipeline status | MEMORY: project_pixellab_character_states_workflow.md | MASTER_KARAR_BELGESI Karar #35/#41/#90 |
| Cross-class skill slots (2 slots?) | MEMORY: project_cross_class_skills.md | GDD §7 (toplam 6 aktif slot, max 2 cross-class) |

## Step 4 — Stale Memory Detection

For each entry in MEMORY.md:
- Marked "DONE" but still active in CURRENT_STATUS?
- Project memories older than 2026-04 — still valid?
- Duplicate or overlapping memory files?

## Step 5 — Orphan Check

For each entry in MEMORY.md, verify the physical file exists (use Read to check).

## Step 5b — STAGING Top-Level Hygiene (2026-06-15)

STAGING üst-seviye = SADECE LIVE `*_DECISION/PLAN/MASTER/SPEC/LOCK/AUDIT/BIBLE.md`. Geri kalan = clutter (PROJECT_RULES "Süreç Artifact Konvansiyonu" + [[project-staging-process-convention]]).

Glob (üst-seviye, non-recursive): `STAGING/*.py` `STAGING/*.log` `STAGING/*.png` `STAGING/*.txt` `STAGING/*.json` `STAGING/*.zip` `STAGING/*.vtt` `STAGING/*.meta`.
- **Allowlist (clutter DEĞİL — aktif kod/araç referansı):** `chatgpt_rooms.json`, `chatgpt_rooms.sample.json` (RoomJsonImporter.cs:14 hardcoded `STAGING/chatgpt_rooms.json`). Yeni aktif top-level non-md çıkarsa buraya ekle.
- Allowlist dışı her non-md üst-seviye dosya = **clutter** → `STAGING/_process/<YYYY-MM>/` veya `STAGING/_archive/<date>_sweep/`'e taşınmasını öner (aktif tool'u taşıma — kuşkuluyu flag'le, kullanıcıya sor).
- Process `.md` deseni (`_council_*`, `cx_task_*`, `_done_*`, `_review_*`, `_research_*`, `AGY_*`, `CX_*`) üst-seviyede kaldıysa → `STAGING/tools/archive_staging_process.ps1` çalıştırmayı öner (whitelist, idempotent, silmez).

## Step 6 — Report

```
## LINT REPORT — [date]

### 🔴 Conflicts (fix immediately)
- [A vs B: description]

### 🟡 Stale Entries (needs update)
- [memory file: why stale]

### 🟢 Orphans / Missing Files
- [listed in MEMORY.md but file not found]

### 🧹 STAGING Clutter (üst-seviye temizlik)
- [allowlist-dışı non-md veya kalmış process .md → önerilen hedef]

### ✅ Clean
- [checked, no issues]

### Recommended Actions
1. [action]
```

Show report, then ask "Which should we fix?"
