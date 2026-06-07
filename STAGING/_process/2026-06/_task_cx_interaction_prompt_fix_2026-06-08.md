ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this task yourself.

# Amaç
Implement Convention A (LOCKED) interaction-prompt fix + EditMode test automation. Decision = `STAGING/INTERACTION_PROMPT_DECISION_2026-06-08.md` (READ IT). Convention B (formatter/strip loc tables) is REJECTED — do NOT build it.

## SCOPE — exactly these, nothing else

### 1. Fix the ONE real bug (token-aware prepend)
`Assets/Scripts/UI/HUDController.cs` ~316-321 `SetInteractionPrompt(string actionName)`:
- Extract the compose logic into a PURE, testable static helper (e.g. `internal static string ComposeInteractionPrompt(string actionName)`).
- Behavior: if `actionName` (after TrimStart) already begins with a bracket token `[...]`, return it UNCHANGED (no `[G]` prepend). Otherwise prepend `[G] ` exactly as today.
- `SetInteractionPrompt` calls the helper. Net effect: `RewardPickup.cs:100` → no more `[G] [G] Ödülü Al`.
- DO NOT touch loc tables, DO NOT change other call sites, DO NOT add a formatter class. Plain-text callers (DoorTrigger "Enter", MapFragment, IntraEncounterDoorTrigger) keep getting `[G] ` — unchanged.

### 2. Tests — EditMode only (PlayMode rejected)
`Assets/Tests/EditMode/InteractionPromptConventionTests.cs`:
- `PromptLocTables_TokenBearingKeysAreExactlyTheApprovedSet`
- `PromptLocTables_ApprovedPromptKeysHaveExactlyOneTokenInTrAndEn`
- `PromptLocTables_ApprovedPromptTokensMatchBetweenLanguages`
- `HudSetInteractionPrompt_RawActionAddsExactlyOneGToken`  (calls ComposeInteractionPrompt)
- `HudSetInteractionPrompt_TokenizedActionDoesNotDoublePrepend`
- `RewardPickupHudRoute_LocalizedTakeRewardEndsWithExactlyOneGToken`

Approved 6 token-bearing keys: `chamber_select.prompt.attune`, `chamber_select.prompt.unlock`, `chamber_select.prompt.enter_rift`, `combat.prompt.execute`, `reward.prompt.take`, `death.btn.retry`.
Lint mechanism: reflection over `Loc._tr`/`_en` (`BindingFlags.NonPublic|Static`, cast to `Dictionary<string,string>`), bracket-token regex `\[[A-Z]+\]` (also catch `[RMB]`, `[R]`, `[TAB]` etc.). Inverted invariant under A: ONLY the approved 6 keys may contain a bracket token; every other loc value must contain ZERO bracket tokens; each approved key has exactly ONE token and the token is identical in TR and EN. No production hook / InternalsVisibleTo change needed.

`Assets/Tests/EditMode/CharacterIdleSouthAssetTests.cs`:
- `ClassIdleSouthSprites_AllTenClassPathsExistOnDisk` — for all 10 ClassType enums, assert the loader path resolves (mirror `ChamberSelectBootstrap.cs:949-950`: `Assets/Resources/Characters/<Class>/<lower>_idle_south.png` / `Resources.Load<Sprite>($"Characters/{Class}/{lower}_idle_south")`).

Put both files in the correct EditMode asmdef (match existing `Assets/Tests/EditMode/*` references).

## VERIFY (mandatory)
- After writing, `refresh_unity` then `read_console` — confirm 0 compile errors.
- If Unity is connected, run the EditMode tests (`run_tests` EditMode filter on the new fixtures) and paste pass/fail counts. If Unity is NOT connected / tests can't run, write `BLOCKED: tests not run (Unity offline)` and report compile status only — do NOT claim green without running.
- Report the exact final diff summary (files + line counts).

## COMMIT
After 0-compile + tests green (or compile-clean if Unity offline), commit:
`fix(ui): token-aware interaction prompt + EditMode lint tests (Convention A) - council-decided`
End the commit message with the Co-Authored-By trailer for Claude if your workflow adds one; otherwise plain. Write result + commit hash to CODEX_DONE.md.
