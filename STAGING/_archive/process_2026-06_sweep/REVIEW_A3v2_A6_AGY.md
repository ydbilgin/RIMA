# AGY REVIEW — A3 v2 (post-cx-fix fragment flow) + A6 (finisher commit-beat)

ACTIVE RULES: (1) think before reviewing (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.
Respond INLINE in your AGY_DONE file (do NOT write to ~/.gemini scratch). Verdict: PASS/FAIL + file:line evidence. Do NOT edit files.

## Context
A first cx review of A3 found 3 issues; they were fixed in v2. Verify the FIXES are correct and complete.

## A3 v2 — `Assets/Scripts/Systems/Map/RoomLoader.cs` method `SpawnFragmentThenDraftUnlock(Gate gate)` + `ClearPendingFragmentPickup()` + `TeardownCurrentRoom` hook.
Combat rooms now drop a fragment on clear; pickup → draft → gate unlock. Fixes applied:
- **issue5 (combat had no anchor → no fragment):** when no FragmentDropAnchor, drop at the PLAYER's position (provably reachable). If no anchor AND no player → direct `UnlockGateAfterDraft` fallback.
- **issue3 (static event leak):** the one-shot `EnvironmentMapFragment.OnAnyFragmentPickedUp` subscriber is stored in field `_pendingFragmentPickup`; `ClearPendingFragmentPickup()` unsubscribes it in `TeardownCurrentRoom` (before destroying fragments) and at the start of the method.
- **issue1 (unreachable softlock):** mitigated by spawning at player feet (reachable by construction).
Also: `MapFragmentSpawner` is now passive (no `OnRoomCleared` auto-subscribe, LOCK-1 removed) so only RoomLoader spawns.

### Questions (PASS/FAIL + evidence)
1. Does the player-feet drop truly avoid softlock in ALL combat paths? Any case where player is null/destroyed at clear, or fragment spawns then is destroyed before pickup?
2. Is `_pendingFragmentPickup` unsubscribed on EVERY teardown path (room swap, demo-complete, boss room)? Any path that tears down without calling TeardownCurrentRoom?
3. Reward room: anchor exists → still works? Drop-at-player only triggers when anchor==null — confirm reward unaffected.
4. Any double-unlock (gate.Unlock called twice) or draft-while-draft-active risk?
5. Did making MapFragmentSpawner passive break the passing T2_GateFlowTest (it FireRoomCleared then waits for the spawner; it has a fallback at ~line 168-179)?

## A6 — `Assets/Scripts/Combat/CombatHandler.cs` `OnCommitBeat()`
Now calls `CombatEventBus.PublishCommitBeat(new CommitBeatEvent{ worldPos, attacker=gameObject, beatIndex=3 })` so HitPauseDriver + ScreenShakeDriver + VFXRouter finisher juice fire. Subscribers confirmed present.
### Questions
6. Any risk publishing every commit-beat (ICD 1.2s guards it) spams hitstop/shake? Is beatIndex=3 the right semantic vs the demo's PublishCommitBeat3?
7. Is the `using RIMA.Combat;` + struct construction correct?

Be terse. Cite file:line. If FAIL, give the exact minimal fix.
