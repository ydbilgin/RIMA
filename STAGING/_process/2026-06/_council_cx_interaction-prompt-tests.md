ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Decide interaction-prompt convention (A vs B) + test-automation scope from a CODE-FEASIBILITY / REUSE lens.

## READ FIRST — reason FROM this, do NOT re-derive
`STAGING/_process/2026-06/_council_interaction_prompt_tests_2026-06-08.md` — full file:line ground truth + the 4 questions + the A-vs-B fork.

## Spot-check (cheap, optional) the ground-truth claims in real code
`Assets/Scripts/Core/Loc.cs` (string tables), `RewardPickup.cs:100`, `HUDController.cs:319`, `ChamberSelectBootstrap.cs` LoadClassIdleSouthSprite (~945-954).

## MANDATORY disk check (decides question #4)
Count how many of the 10 classes actually have an `idle_south` sprite ON DISK. Look under `Assets/Resources/Characters/<Class>/<lower>_idle_south.png` (and any editor path the loader uses). Report the exact list: which classes HAVE it, which FALL BACK to the generic silhouette. If most fall back, all those pedestals render the SAME silhouette in play (the thing that could LOOK like "repeated Warblade").

## Answer the 4 questions from a feasibility / what-already-exists / reuse-vs-build lens
1. **A or B?** Decisive pick + 1-2 line why (code-churn vs correctness). Note exact files/lines each option touches.
2. **Exact test files + 4-8 highest-value assertions** under your pick. CRITICAL: how do we enumerate the loc strings for the lint test? `Loc._tr`/`_en` are private static dicts — propose the concrete mechanism (reflection? a small internal test-hook like `Loc.AllKeys()`/`Loc.AllStrings()`? `[assembly:InternalsVisibleTo]`?) that fits Loc.cs AS IT EXISTS. Give the assertion names.
3. **Regression-guard tests** for pedestal-identity (#1) + HUD-off-in-chamber (#2): worth adding (code already satisfies them) or wasted effort?
4. **Sprite-existence:** report the actual disk count; recommend a distinct-sprite test (assert 10 classes resolve distinct sprites) or not.

ANALYSIS ONLY — no code changes. Write result to CODEX_DONE.md. Do NOT reproduce any prior audit. Keep it lean and decisive.
