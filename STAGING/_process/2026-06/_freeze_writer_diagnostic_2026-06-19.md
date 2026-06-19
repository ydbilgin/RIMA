# Freeze Writer Diagnostic — 2026-06-19

## (A) Reproduce Rate
0/5 boots froze. All 5 resolved to timeScale=1 immediately after Card_0 pick
(DeathScreen activated from enemy kills — player dies fast at default spawn).
Boots: Warblade x3 (B1 OK, B2 OK, B3 OK), Elementalist x2 (B4 OK, B5 OK).
Each boot: draft confirmed open (IsDraftActive=True, timeScale=0), Card_0 invoked,
next round-trip: timeScale=1, IsDraftActive=False, DeathScreen=True.

## (B) timeScale=0 Writer (post-death freeze)
In the HPD/combat check boot, timeScale returned to 0 ~4s after card pick (after
death + DeathScreenManager slow-mo completed). This is INTENTIONAL, not a freeze:

DeathScreenManager.cs lines 98-101:
  Time.timeScale = slowMoScale;          // 0.15 for 1.5s
  yield return new WaitForSecondsRealtime(slowMoDuration);
  Time.timeScale = 0f;                   // hard 0 — death-screen freeze

Confirmed by runtime field dump:
  DSM.isDead=True, DSM.IsDeathActiveForDemo=True, deathRoutine= (coroutine done)
  slowMoScale=0.15, slowMoDuration=1.5 — coroutine completed; 0f is the final write.
  RestartButton present — player must click to restore timeScale=1.

RunStats.frozen=True was seen in scan but RunStats never writes Time.timeScale —
frozen is a run-timer guard only (timer stops on player death).

HitPauseDriver: Instance=NULL, count=0 at all measured points. The prior HPD fix
was a no-op in _Arena as suspected.

## (C) HitPauseDriver in Combat
Checked T=0 (post pick), T+2s, T+4s across 3 round-trips:
  HPD_Instance=NULL, HPD_count=0 at every check.
HitPauseDriver is NEVER instantiated in _Arena during real combat. Answer: NO.

## Raw Dumps

### Boot check results (all 5)
Boot1 (WB): timeScale=0 draft-open → Card_0 → Check1: timeScale=1, IsDraftActive=False, DeathScreen=True → OK
Boot2 (WB): timeScale=0 draft-open → Card_0 → Check1: timeScale=1, IsDraftActive=False, DeathScreen=True → OK
Boot3 (WB): timeScale=0 draft-open → Card_0 → Check1: timeScale=1, IsDraftActive=False, DeathScreen=True → OK
Boot4 (EL): timeScale=0 draft-open → Card_0 → Check1: timeScale=1, IsDraftActive=False, DeathScreen=True → OK
Boot5 (EL): timeScale=0 draft-open → Card_0 → Check1: timeScale=1, IsDraftActive=False, DeathScreen=True → OK

### HPD combat boot raw
T=0 pick:  HPD_Instance=NULL count=0, scale=0 (draft still processing)
T+2:       HPD_Instance=NULL count=0, scale=1, Draft=False, Death=True
T+4:       HPD_Instance=NULL count=0, scale=0 (DSM death-freeze settled)

### Broad MB scan at timeScale=0 (post DSM death-freeze)
Notable entries from Resources.FindObjectsOfTypeAll scan:
  [DeathScreenManager] isDead=T, IsDeathActiveForDemo=T, fadeInDuration=0.8, slowMoScale=0.15, slowMoDuration=1.5
  [RunStats] runStarted=T, frozen=T, echoAwarded=T  (frozen = timer guard, NOT timeScale writer)
  [DraftManager] RoomClearDraftDelay=2, IsDraftActive=False
  [RoomRunDirector] clearSlowMoScale=0.3, openingDraftShown=T
  [HitPauseDriver]: not present in scan (0 instances)
  [HitStop]: not present in scan (0 instances)
UIManager flags: all false (no pauseOpen/skillOfferOpen/etc.)

## Console Status
0 errors, 0 warnings at session end.

## Conclusion
The reported Card_0 freeze (timeScale stuck at 0 post-draft) did NOT reproduce across
5 boots today. The timeScale=0 seen ~4s post-pick is DeathScreenManager intentional
death-freeze (DSM runs 0.15 slow-mo for 1.5s, then hard-sets 0 for death screen).
HitPauseDriver is never instantiated in _Arena — the prior HPD fix was a no-op there.
If the freeze was real, its root cause was likely somewhere else (UIManager flag leak,
DraftManager not restoring timeScale, or CheckpointManager). Recommend a targeted
UIManager flag audit if the freeze re-emerges.
