ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

Amaç (purpose): REVIEW ONLY — do NOT change any code. We are about to execute a P0→P1 plan to take RIMA from a "playable skeleton" to a "playable demo". Critique the plan for technical correctness against the actual code BEFORE we start.

READ: `STAGING/GAME_STATE_AND_PLAN_2026-06-02.md` (the full plan, with file:line refs). Then spot-check the cited files to confirm/refute each P0 claim.

Answer concisely, numbered (this is a code review, NOT an implementation):
1. Is the VERDICT accurate (playable spine but hollow combat, 3 duplicate maps, PLAY AGAIN soft-lock)? Over/understated?
2. P0 correctness — verify each against code:
   a. PLAY AGAIN soft-lock fix in `DemoCompleteOverlay.Restart()` (Core/DemoCompleteOverlay.cs:116-119) — is reloading active buildIndex really the bug? Is `MapFlowManager.ResetRun()` + load MainMenu the right fix?
   b. **CRITICAL:** removing the `[Obsolete]` RuntimeRoomManager *component* from the 3 iso scenes — will `RoomClearVictoryTrigger` + `MapFlowManager` + DungeonGraph genuinely keep the loop completing (door→gate→next map→victory)? Or does RuntimeRoomManager still own something essential (enemy spawn, gate state, map-fragment) that would break? This is the riskiest P0 — verify hard against RuntimeRoomManager.cs + RoomClearVictoryTrigger.cs + DoorTrigger.cs + Gate.cs.
   c. JuiceManager + KnockbackReceiver + HitFlashDriver wiring — confirm these drivers exist under Assets/Scripts/Combat/Juice and that mob prefabs truly lack KnockbackReceiver.
   d. Elite teleport wall-escape (Enemies/EliteAffix.cs:184-185), reward sprite editor-only path (RoomClearVictoryTrigger.cs:139-153) — real?
3. Anything MISSING from P0 that would block a playable demo?
4. Best EXECUTION ORDER for P0 (dependencies between items)?
5. Finisher fork: (a) defer Sundered Beat with the rest of animations, or (b) add a minimal placeholder attack animation-state now so CombatHandler+Beat3CommitTrigger can fire? Which is lower-risk / higher-value for a demo?
6. Any plan claim that is factually WRONG when checked against code (file:line)?

Output: numbered answers + final "VERDICT: sound / needs-changes (bullet list)".
