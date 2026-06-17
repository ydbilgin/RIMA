# 06 — Claude Master Prompt

You are working on the private Unity repository:

`github.com/ydbilgin/RIMA`

Target branch/snapshot:

- branch: `master`
- requested review snapshot: `48a006ef`
- academic demo deadline: approximately 19 June 2026

This ZIP contains a full visual audit of `capture_v3` and a combat-risk review. Read the files in this exact order:

1. `README_FIRST.md`
2. `01_EXECUTIVE_VERDICT.md`
3. `02_SCREENSHOT_AUDIT_35.md`
4. `03_RECAPTURE_PLAN.md`
5. `04_COMBAT_TECHNICAL_REVIEW.md`
6. `05_FULL_FLOW_TEST_PROTOCOL.md`
7. `07_REPO_READ_LIST.md`
8. `08_DELIVERY_CHECKLIST.md`
9. `09_SIX_QUESTIONS_ANSWERED.md`

Then inspect the repository files listed in `07_REPO_READ_LIST.md`.

## Critical correction

Do not trust the original screenshot filenames or `DONE_capture_v3.md` as proof of visible state.

All 35 PNGs were opened and checked. Exact SHA uniqueness is true, but many semantic labels are false:

- combat screenshots are behind a death screen
- run-map is actually draft
- character sheet is death screen
- HUD states are draft
- merchant and elite are not shown
- boss frames do not show a valid boss encounter or HP bar

The corrected screenshot audit is the source of truth.

## Your mission

Finish the demo safely, not expansively.

### P0 blockers

1. Verify/fix canonical runtime Player tag and target resolution.
2. Add boss Player re-acquire/shared resolver.
3. Verify/fix `AttackTokenManager` scene lifecycle.
4. Remove Penitent from the opening wave and reduce demo combo lethality.
5. Run the full cold-flow test protocol three times.

Do not claim success from synthetic instantiate tests or green assertions alone.

### P1 polish after P0 passes

1. Add `CombatJuice.prefab` to `_Arena` and manually verify timeScale recovery.
2. Apply enemy outline/contrast with visual comparison.
3. Reuse the existing `RIMA.EnemyTelegraph`; do not build another framework.
4. Prioritize ChainExplosion, Wrath, Charge, then HolyLash.
5. Lock telegraph geometry and damage to the same attack snapshot.
6. Demonstrate one deterministic Edit-to-Play before/after sequence.

### P2 evidence

Recapture the critical shots in `03_RECAPTURE_PLAN.md`. A smaller truthful set is preferred over 35 mislabeled screenshots.

## Engineering rules

- Surgical edits only.
- No broad refactors.
- No new speculative framework.
- Preserve already-working systems.
- Read Console after every Unity change.
- Manually observe play behavior.
- Stop new polish work if a seam fails.
- Do not modify screenshots to fake state.
- Do not use exact hash uniqueness as the only capture validation.

## Required final output

Produce:

1. changed-file list
2. reason for each change
3. manual test result for each P0 blocker
4. Console error/warning status
5. full-flow result, run by run
6. remaining risks
7. corrected screenshot manifest with `KEEP / RECAPTURE / REMOVE`
8. final presentation shot list
9. explicit GO / NO-GO verdict

Do not mark combat DONE until the full chain passes from the real entry route.
