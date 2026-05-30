# CX REVIEW TASK — A3 fragment-on-combat-clear (softlock + double-spawn audit)

ACTIVE RULES: (1) think before reviewing (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç
Review ONLY the A3 change that makes COMBAT rooms drop a map-fragment whose pickup
triggers draft→gate-unlock (previously combat rooms unlocked immediately on clear).
Verdict format: PASS / FAIL + concrete evidence (file:line). Do NOT write code. Do NOT edit files.

## What changed (read these exact spots)
1. `Assets/Scripts/Systems/Map/RoomLoader.cs`
   - Combat branch (~line 290): `clearToUnlock` now calls `SpawnFragmentThenDraftUnlock(gate)` instead of `UnlockGateAfterDraft(gate)`.
   - NEW method `SpawnFragmentThenDraftUnlock(Gate gate)` (~line 322): find FragmentDropAnchor;
     if null → `UnlockGateAfterDraft(gate)` fallback (no fragment); else optionally call spawner via
     SendMessage, then GUARANTEE a fragment by building a GO if `FindFirstObjectByType<EnvironmentMapFragment>()==null`;
     then one-shot subscribe `EnvironmentMapFragment.OnAnyFragmentPickedUp` → `UnlockGateAfterDraft(gate)`.
   - `RewardRoomAutoTrigger` now just waits 2s then calls the same shared method.
2. `Assets/Scripts/Environment/MapFragmentSpawner.cs`
   - REMOVED `OnEnable/OnDisable` auto-subscription to `RoomLoader.OnRoomCleared`.
   - REMOVED LOCK-1 `isRewardRoom` early-return in `HandleRoomCleared`.
   - Now a passive helper, only invoked by RoomLoader via SendMessage.

## Questions to answer (each: PASS/FAIL + evidence)
1. **Softlock:** Is there ANY path where a combat room clears but the gate never unlocks?
   (e.g. anchor present but pickup event never fires; fragment spawned unreachable; player can't reach it.)
2. **Double-spawn:** Can two fragments spawn for one room now? Consider: spawner present + RoomLoader
   build-fallback both firing; the `FindFirstObjectByType<EnvironmentMapFragment>()==null` guard timing
   (is the SendMessage-spawned GO findable synchronously the same frame?).
3. **Event leak:** The one-shot `onFragment` lambda — does it always unsubscribe? What if the room is
   torn down (RoomLoader destroys all EnvironmentMapFragment at ~line 196 on next-room load) before pickup?
   Does a stale static-event subscriber leak across rooms?
4. **Reward-room regression:** Did removing the spawner auto-subscription / LOCK-1 break the reward-room
   flow (which previously passed LOCK-1)? Reward rooms have no combat, so OnRoomCleared may not fire there.
5. **Anything else** that smells (draft-while-draft-active, gate double-unlock, etc.).

## Output
Write verdict to CODEX_DONE.md (or stdout). Be terse. Cite file:line. If FAIL, give the exact minimal fix.
