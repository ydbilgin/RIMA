# Implementation Order

This order minimizes the chance that one fix invalidates another.

## Phase 0 — Verification checkpoint

Before editing:

1. Confirm actual current line numbers.
2. Read the full `RoomRunDirector.StopClearSequences`.
3. Search every `Time.timeScale =` assignment in the repository.
4. Search every `ShowDraft`, `ShowOpeningKitDraft`, and `ShowDraftDelayed` caller.
5. Search all subscriptions to `OnSecondaryClassSelected`.
6. Identify actual gameplay scene names and transition path.
7. Check existing EditMode/PlayMode tests.

Deliver a short verification table before patching.

## Phase 1 — Time-scale authority

Fix RIMA-001 first.

Expected changed files:

- `UIManager.cs`
- `DirectorMode.cs`
- possibly a new `GameTimeCoordinator.cs`
- tests

Do not proceed until overlay + Director combinations are covered.

## Phase 2 — Build Mode entry safety and bootstrap

Fix:

- RIMA-002
- RIMA-003
- RIMA-009

Expected changed files:

- `DirectorMode.cs`
- `BuildModeController.cs`
- tests

Verify both normal menu flow and direct Arena development flow.

## Phase 3 — Draft serialization

Fix together:

- RIMA-004
- RIMA-005
- RIMA-006
- RIMA-007

Expected changed files:

- `DraftManager.cs`
- `RewardPickup.cs`
- `RoomRunDirector.cs`
- possibly `SkillOfferUI.cs`
- tests

Do not fix these independently with separate flags. One serialized request path should cover all sources.

## Phase 4 — Build authoring polish

Fix:

- RIMA-008

Then run Build Mode tool regressions.

## Phase 5 — Contract documentation

For RIMA-010:

- add a test documenting current one-damage floor,
- or change it only after confirming the intended zero-damage contract.

RIMA-011 and RIMA-012 require no production change.

## Suggested commit grouping

1. `fix: centralize demo time-scale resolution`
2. `fix: bootstrap and guard build mode lifecycle`
3. `fix: serialize draft requests and timeout recovery`
4. `fix: isolate build-mode hotkeys from text input`
5. `test: add demo state interaction regressions`

Small coherent commits make rollback possible when Unity decides a harmless serialization change is a personal insult.
