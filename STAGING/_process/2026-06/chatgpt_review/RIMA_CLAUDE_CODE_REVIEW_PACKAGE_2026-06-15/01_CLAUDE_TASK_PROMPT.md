# Prompt to give Claude Code

You are reviewing and hardening the Unity repository `ydbilgin/RIMA` for a live technical demo.

This ZIP contains an independent review of the current `master` source. Treat it as a hypothesis set, not unquestionable truth.

## Your job

1. Read every file in this ZIP.
2. Open the actual current repository source files listed in `08_FILES_TO_REVIEW.md`.
3. Verify each finding against real code and scene/runtime contracts.
4. For every finding:
   - Mark it `CONFIRMED`, `REJECTED`, or `NEEDS-RUNTIME-VERIFICATION`.
   - Explain why.
   - Give exact current `file:line`.
5. Implement confirmed live-demo blockers with the smallest coherent change.
6. Add focused regression tests.
7. Run compilation and available Unity tests.
8. Produce a final change report.

## Non-negotiable engineering constraints

- Do not trust the structural/call graph blindly.
- Do not change gameplay balance unless a test proves the current behavior violates the intended contract.
- Do not add a second pause system. Consolidate ownership or introduce a single arbiter.
- Do not let Build Mode silently hide an active modal UI while its state/coroutines continue running.
- Do not permit more than one delayed room-clear draft.
- Do not leave anonymous event handlers that cannot be unsubscribed.
- Do not open doors after a draft timeout while leaving the draft active and `Time.timeScale == 0`.
- Do not perform broad unrelated refactors.
- Preserve the normal MainMenu → CharacterSelect → gameplay flow.
- Preserve direct `_Arena` developer entry.
- Preserve F12 panic recovery.
- Keep all timeout logic on unscaled time.
- Add tests before calling a fix complete.

## Preferred implementation strategy

Use the detailed guidance in:

- `03_DETAILED_FINDINGS.md`
- `04_RECOMMENDED_ARCHITECTURE.md`
- `05_IMPLEMENTATION_ORDER.md`
- `06_TEST_MATRIX.md`

The report proposes two levels of time-scale correction:

1. **Preferred:** one central pause/time-scale arbiter using pause reasons.
2. **Minimum demo-safe patch:** Director Mode and Build Mode must defer resuming to `UIManager`/death state instead of directly forcing `Time.timeScale = 1`.

Choose the smallest approach that fully eliminates conflicting ownership and is covered by tests.

## Required final output

Return:

### A. Verification table

| ID | Verdict | Severity | Current file:line | Reason |
|---|---|---:|---|---|

### B. Changes made

For each changed file:
- Why it changed
- Behavioral effect
- Risks
- Tests covering it

### C. Test results

Include:
- Unity compilation result
- EditMode tests
- PlayMode tests
- Any test not run and why

### D. Remaining risks

Explicitly list:
- Findings not fixed
- Findings rejected
- Runtime-only verification still needed
- Scene/prefab assumptions

### E. Patch summary

Provide a concise diff summary suitable for a commit message.

Do not merely repeat the report. Verify, patch, test, and challenge incorrect assumptions.
