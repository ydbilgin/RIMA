# RIMA — Claude Review & Demo Rescue Package

**Repo:** `github.com/ydbilgin/RIMA`  
**Target:** `master`, requested snapshot `48a006ef`  
**Demo deadline:** ~19 June 2026  
**Package date:** 17 June 2026

## Read this first

The original `capture_v3` manifest correctly reports:

- 35 PNG files are present.
- Every PNG is 1920×1080.
- There are no exact SHA-256 duplicates.

However, **exact hash uniqueness is not visual/state correctness**. All 35 PNGs were opened and visually inspected. The filename/manifest state is wrong, unsupported, or presentation-unsafe for a large part of the set.

### Audit result

- **8 KEEP**
- **4 KEEP_SECONDARY**
- **1 RELABEL**
- **1 REMOVE**
- **21 RECAPTURE**

The current set is **not sufficient as final presentation evidence**. It strongly proves menus, draft, codex, part of Director Mode, and Build Mode UI. It does **not** cleanly prove live combat, HUD HP states, run-map, character sheet, merchant, elite, free-cam, non-zero telemetry, or a functioning boss fight.

## Required reading order

1. `01_EXECUTIVE_VERDICT.md`
2. `02_SCREENSHOT_AUDIT_35.md`
3. `03_RECAPTURE_PLAN.md`
4. `04_COMBAT_TECHNICAL_REVIEW.md`
5. `05_FULL_FLOW_TEST_PROTOCOL.md`
6. `06_CLAUDE_MASTER_PROMPT.md`
7. `07_REPO_READ_LIST.md`
8. `08_DELIVERY_CHECKLIST.md`
9. `09_SIX_QUESTIONS_ANSWERED.md`

Original screenshots and their original manifest are under:

`evidence/capture_v3_original/`

Color-coded contact sheets are under:

`evidence/annotated_contact_sheets/`

## Important truth correction

The original `DONE_capture_v3.md` says no invisible/unbound enemy blocker was detected and describes several frames as capture-truth evidence. Visual inspection contradicts the semantic labels:

- Combat frames 11–13 are shown **behind a death screen** with `KILLS 0` and `SÜRE 00:00`.
- Run-map frame 06 is actually a reward draft.
- Character sheet frame 10 is a death screen.
- HUD mid/low frames 15–16 are reward drafts.
- Director spawn frame 17 is a reward draft.
- Merchant and elite frames show no merchant/elite proof.
- Boss frames do not show a usable boss fight or HP bar.

Treat the corrected audit as the source of truth.
