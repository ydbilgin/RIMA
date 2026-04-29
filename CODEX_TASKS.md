# CODEX_TASKS — S43 Anchor QC + _STAGING Cleanup

**Date:** 2026-04-29  
**Assigned to:** Codex  
**Priority:** High — blocks Elementalist regen decision  
**Language:** English throughout

---

## Context (read before starting)

Memory files are at:
`C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/`
Read `MEMORY.md` index first, then open relevant `*.md` files (style, feedback, pipeline).

Project docs:
- `CURRENT_STATUS.md` — session state, open questions
- `_STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md` — S43 pipeline rules
- `_STAGING/anchors/_QC_REPORT_S43.md` — existing QC notes
- `_STAGING/anchors/_GEMINI_REVIEW_S43.md` — Gemini review pass
- `_STAGING/anchors/_CODEX_VERIFY_S43.md` — previous Codex verify

---

## TASK 1 — Elementalist Anchor QC: `elementalist_anchor_mu.png`

**File:** `_STAGING/anchors/elementalist_anchor_mu.png`  
**What it is:** A candidate Elementalist anchor ("Delete-the-book" variant) — tunic + skirt outfit, orb in hand. Different silhouette from current `_STAGING/anchors/elementalist/elementalist_anchor.png` (crop top + skirt).

**Your job:** Evaluate style consistency against the confirmed PASS anchors. Use Gemma (`gemma4:e4b`) for pixel-level analysis if available, then give your own verdict.

### Reference anchors (all confirmed PASS):
- `_STAGING/anchors/warblade/warblade_anchor.png`
- `_STAGING/anchors/shadowblade/shadowblade_anchor.png`
- `_STAGING/anchors/ronin/ronin_anchor.png`
- `_STAGING/anchors/ravager/ravager_anchor.png`
- `_STAGING/anchors/gunslinger/gunslinger_anchor.png`
- `_STAGING/anchors/ranger/ranger_anchor.png`
- `_STAGING/anchors/brawler/brawler_anchor.png`

### QC checklist (score each 0/1):

| Gate | Criteria |
|---|---|
| 1. Outline | Single-color heavy dark outline, consistent weight across all edges |
| 2. Palette tone | Muted/weathered — no oversaturated colors, no neon, no pure white highlights |
| 3. Proportion | ~7-head realistic (NOT chibi, NOT super-deformed) — head:body ratio matches PASS anchors |
| 4. Pixel density | Similar detail level — not too clean/smooth (AI-looking), not too noisy |
| 5. Camera angle | 30-35° low top-down ARPG view — consistent with other anchors |
| 6. Canvas usage | Character fills roughly 70-80% of 128×128, not floating tiny or cropped |
| 7. Style cohesion | Overall "reads as same game" next to Warblade/Shadowblade — silhouette language consistent |

**Output format:**

```
TASK 1 RESULT
File: elementalist_anchor_mu.png
Score: X/7

Gate results:
1. Outline: PASS/FAIL — [note]
2. Palette: PASS/FAIL — [note]
3. Proportion: PASS/FAIL — [note]
4. Pixel density: PASS/FAIL — [note]
5. Camera angle: PASS/FAIL — [note]
6. Canvas usage: PASS/FAIL — [note]
7. Style cohesion: PASS/FAIL — [note]

VERDICT: PASS / CONDITIONAL / FAIL
Recommendation: [keep as anchor candidate / discard / regen needed]
```

---

## TASK 2 — _STAGING Lint & Cleanup

**Scope:** Everything under `_STAGING/` and project root `.md` files.  
**Goal:** Remove duplicates, merge overlapping reports, fix internal contradictions, delete dead files.  

Work through each area below in order. For every action, log it in the report (Task 2 output).

### 2A — Duplicate PNG files in `_STAGING/anchors/` — REPORT ONLY, NO DELETE

Pattern found: each class has BOTH a root-level copy AND a subfolder copy:
- `_STAGING/anchors/warblade_anchor.png` AND `_STAGING/anchors/warblade/warblade_anchor.png`
- Same for: shadowblade, ranger, gunslinger, ronin, brawler, hexer, summoner, elementalist, ravager

**Action:** Report only — do NOT delete anything.
1. For each pair, check if they are identical (file size).
2. Log result: identical / different.
3. Cleanup deferred until all anchor decisions are finalized.

### 2B — QC/Report file consolidation in `_STAGING/anchors/`

Three overlapping report files:
- `_QC_REPORT_S43.md`
- `_GEMINI_REVIEW_S43.md`
- `_CODEX_VERIFY_S43.md`

**Action:**
1. Read all three.
2. Identify overlapping content (same class reviewed in multiple files).
3. Merge into a single `_ANCHOR_QC_MASTER_S43.md` with clear per-class sections.
4. Delete the three originals after merge.
5. Format: one section per class — `## Warblade`, `## Shadowblade`, etc. Each section: verdict, score, notes, date.

### 2C — PROMPTS_S43 contradiction: Elementalist outfit

Two conflicting files:
- `_STAGING/PROMPTS_S43/elementalist_character_description.md` — says "fitted dark trousers" (v1, stale)
- `_STAGING/PROMPTS_S43/elementalist_anchor.md` — says "Short asymmetric DUSTY-INDIGO miniskirt" (v2, current)

**Action:**
1. Read both files.
2. Update `elementalist_character_description.md` to match v2 anchor prompt: replace "fitted dark trousers" with "short asymmetric DUSTY-INDIGO miniskirt" and align any other conflicting details.
3. Add a `# v2 — synced with elementalist_anchor.md 2026-04-29` header note.
4. Do NOT delete either file — both serve different purposes (description vs prompt).

### 2D — PROMPTS_S43 overlap check

Files to check for redundancy:
- `characters_styleref_v1.md`
- `styleref_cheatsheet_v1.md`
- `roster_sheet_merged_v1.md`
- `batch_all_s43.md`

**Action:**
1. Read all four.
2. Identify: are any of these subsets of another? Any dead/superseded content?
3. If two files cover the same ground: merge into the more complete one, delete the other.
4. If a file is purely historical with no current use: move to `_ARCHIVE/CODEX_TAMAMLANDI/` (do NOT delete — archive).
5. If all are distinct: leave them, just report.

### 2E — `elementalist_anchor_mu.png` disposition

After Task 1:
- If PASS: rename to `_STAGING/anchors/elementalist/elementalist_anchor_mu_CANDIDATE.png` and flag in report — Claude will decide whether to replace current anchor.
- If FAIL: move to `_ARCHIVE/` with note.
- Either way: do NOT replace `elementalist/elementalist_anchor.png` — that decision stays with Claude.

### 2F — Orphan/stale file scan

Scan `_STAGING/` for:
- Files with `_old`, `_v1`, `_test`, `_temp`, `_draft` in name that have a newer version present
- Files not referenced in any `.md` doc
- Empty files or placeholder files

**Action:** List them in the report. For clear orphans (superseded by newer version), delete. For ambiguous ones, flag to Claude.

---

## TASK 2 Output Format

Write results to `_STAGING/_CLEANUP_REPORT_2026-04-29.md`:

```
# _STAGING Cleanup Report — 2026-04-29

## 2A Duplicate PNGs
- [class]: identical → root copy deleted / DIFFERENT → flagged
...

## 2B QC Report Merge
- Merged to: _ANCHOR_QC_MASTER_S43.md
- Deleted: [list]
- Classes covered: [list]

## 2C Elementalist contradiction
- Fixed: elementalist_character_description.md — [what changed]

## 2D PROMPTS_S43 overlap
- [file]: kept / merged into X / archived
...

## 2E elementalist_anchor_mu.png
- Task 1 result: PASS/FAIL
- Action taken: [renamed/archived]

## 2F Orphans
- Deleted: [list]
- Flagged for Claude: [list]

## Summary
Total files deleted: X
Total files merged: X
Total files archived: X
Flags for Claude: [list]
```

---

## Escalate to Claude if:
- Any two files have conflicting content that can't be auto-resolved
- A file deletion would affect Unity `Assets/` or anything outside `_STAGING/` and root `.md`
- Task 1 score is 3/7 or lower (major style failure — Claude needs to see it)
- Any file appears to be in-progress work (not clearly stale)
