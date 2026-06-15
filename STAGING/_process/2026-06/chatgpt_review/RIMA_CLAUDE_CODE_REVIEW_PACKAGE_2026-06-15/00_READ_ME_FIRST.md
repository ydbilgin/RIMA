# RIMA Independent Code Review Package

**Repository:** `ydbilgin/RIMA`  
**Target:** Unity 2D top-down ARPG technical demo  
**Review date:** 2026-06-15  
**Primary branch inspected:** `master`

This package converts the independent review into an implementation-ready handoff for Claude Code.

## What this package contains

1. A ready-to-paste Claude task prompt.
2. A concise risk summary.
3. Detailed findings with failure scenarios, evidence, recommended fixes, and validation steps.
4. A proposed architecture for pause/time-scale and draft scheduling.
5. A low-risk implementation order.
6. EditMode and PlayMode regression tests.
7. Final acceptance criteria.
8. A source-file review manifest.

## Important reliability note

The report distinguishes between:

- **CONFIRMED:** The defect follows directly from the inspected source.
- **SUSPECTED:** The source contains a credible failure path, but scene wiring, lifecycle order, or a caller outside the reviewed slice may prevent it. Claude must verify the live call path before editing.
- **NON-ISSUE / FUTURE:** A suspicious-looking area that currently matches the code contract or is explicitly deferred.

Line numbers may drift after later commits. Claude must locate the referenced method and verify the actual current source instead of blindly editing a numeric line.

## Intended workflow

Claude should:

1. Read `01_CLAUDE_TASK_PROMPT.md`.
2. Re-open every source file listed in `08_FILES_TO_REVIEW.md`.
3. Verify all suspected findings.
4. Implement fixes in the order in `05_IMPLEMENTATION_ORDER.md`.
5. Add the tests in `06_TEST_MATRIX.md`.
6. Report any rejected finding with concrete source evidence.
7. Avoid unrelated refactors before the technical demo.

The goal is not to make the code “beautiful.” The goal is to prevent embarrassing live-demo failures, which is a more practical ambition for software created by mammals.
