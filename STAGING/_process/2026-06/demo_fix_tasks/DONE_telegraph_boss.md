# DONE — Boss attack telegraph + VFX wire (2026-06-17)

## Scope
Additive only. Boss damage/phase/timing LOGIC untouched (git diff PenitentSovereign.cs = 0 deletions).
Only telegraph-spawn + FlashImpact calls added. Spec: `STAGING/ENEMY_TELEGRAPH_VFX_SPEC_2026-06-17.md`.

## Engine extensions — `Assets/Scripts/Enemies/EnemyTelegraph.cs`
- `SpawnDelayedRing(Vector2 center, float radius, float delay)` + instance `ShowDelayedRing` + `holdMode`
  field. Behaviour: ring holds at full alpha for first 80% of `delay` (steady "WILL explode here"
  signal), then sin-flash-fades the last 20%. REUSES existing Update()/decal/teardown motor; pulse
  path for normal telegraphs preserved byte-identical.
- `FlashImpact(Vector2 pos, VfxElement element = Physical)` — wraps existing `SkillVfx.ImpactBurst`
  (self-destructing additive burst). No new VFX engine.

## Wired 6 telegraph-less attacks — `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs`
Added helper `WindupSeconds(d)` that mirrors `Telegraph()`'s P3 scaling EXACTLY
(`phase3Active ? Max(0.22, d*0.85) : d`) so ground-telegraph duration == color-pulse windup.

| Attack | Telegraph added | Snap |
|---|---|---|
| HolyLash | SpawnCone(180°, holyLashRadius) | FlashImpact |
| ShackleThrow | SkillVfx.CastFlash (P1 origin cue) | — (projectile unannounced by convention) |
| FractureStrike | SpawnCircle(meleeStopRange+0.4) | FlashImpact per strike (×3) |
| ChainExplosion | SpawnDelayedRing per marker | FlashImpact at blast |
| SovereignsWrath | SpawnCircle outer danger + inner safe ring | FlashImpact (big) |
| FractureCharge | SpawnLine dash lane (16f) | FlashImpact at terminus |
Also added FlashImpact snaps to the 2 already-telegraphed P1 attacks (ChainWhip, PenitentSurge).

## Windup-sync proof (runtime, edit-mode reflection drive of attack coroutines)
Captured each spawned telegraph's `Duration` vs the real windup — BIT-FOR-BIT EXACT:

P1: HolyLash 0.75=0.75 · FractureStrike 0.3=0.3 · Wrath 1.45=1.45 (both rings) ·
    FractureCharge 0.65=0.65 · ChainWhip 0.75 · Surge 0.85
P3 (0.85x): HolyLash 0.6375 · FractureStrike 0.255 · Wrath 1.2325 · FractureCharge 0.5525
ChainExplosion ring delay: P1=3.0 (==chainExplosionDelay) · P3=2.55 (==Max(0.22, 3*0.85)) —
    delay extracted from the SAME ring inside ChainMarkerSequence that gates the explosion loop.

## Windup→damage sync proof (PLAY MODE, real frames)
- HolyLash: at t=0 cone spawned, player HP unchanged (damage gated behind window); after windup
  HP dropped → damage lands AFTER telegraph, not at t=0. Telegraph self-destructed.
- ChainExplosion (headline fix): DelayedRing spawned (dur=3 at player pos), held full 3s, then
  player HP 999→974 = exactly chainExplosionDmg(25) at ring completion; rings remaining=0.
  => ring lifetime == explosion delay, synchronized, no leak.
- FlashImpact runs in Play mode (SkillVfxRunner created). Its edit-mode "DontDestroyOnLoad" error
  is a pre-existing SkillVfx constraint (edit-mode only), NOT a regression.

## Orphan — `Assets/Scripts/Enemy/Telegraph/EnemyTelegraph.cs`
Marked `[Obsolete]` (not deleted, Karpathy-3). Grep confirms zero external consumers → no new warnings.

## Boss regression
0 deletions in boss logic. Intro/health-bar/phase code untouched (BuildRimLayer/BossIntroController/
DoPhaseTransition/DoPhase3Transition/HandleDeath all unmodified). Types + new methods resolve via
reflection; isCompiling=False.

## Console
read_console (Error+Warning) after force-recompile AND after Play→Stop = 0 entries. VERIFIED.

## VERDICT: PASS — all 6 attacks telegraphed, windup-sync bit-exact (incl P3 0.85x + ChainExplosion 3s↔ring), damage lands at telegraph end, no leak, 0 console errors.
