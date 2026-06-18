Verdict: PASS-WITH-FIXES

Scope: static review only. No code, git, Unity play, or Unity compile actions were run. Reviewed dispatch, spec, builder report, git diff for *.cs, graphify query attempts, and targeted source files.

Findings

HIGH - FIX 2 is infra-only and does not fix the reported spend-before-veto symptom yet.
Evidence:
- Assets/Scripts/Skills/Base/SkillBase.cs:80 adds protected virtual bool CanExecute() => true.
- Assets/Scripts/Skills/Base/SkillBase.cs:91 checks CanExecute before resource spend.
- rg found no subclass override: only SkillBase.cs:80 and SkillBase.cs:91 mention CanExecute.
- Assets/Scripts/Skills/Elementalist/ChainLightning.cs:25 sets resourceCost = 25, then Execute finds first target at line 33 and returns at line 34 when none exists. Because CanExecute defaults true, TryActivate still spends mana at SkillBase.cs:93 and starts cooldown at SkillBase.cs:96 before this no-op return.
Ruling: the exact example symptom, Chain Lightning with no enemy in range, is not solved. This is demo-critical if Chain Lightning can be drafted/used in the demo. Required minimum fix: add CanExecute override to ChainLightning using the same first-target predicate as Execute.

Range/target/state-gated no-op skills that should get CanExecute overrides for demo hardening:
- Assets/Scripts/Skills/Elementalist/ChainLightning.cs:33-34: false when FindNearestEnemy(transform.position, null) is null, using jumpRange/Health filtering from lines 90-103.
- Assets/Scripts/Skills/Elementalist/LivingBomb.cs:30-31: false when FindNearestEnemy() returns null. Note this helper is global EnemyAI search, not range-limited.
- Assets/Scripts/Skills/Shadowblade/ShadowStep.cs:38-39: false when FindNearestEnemyInRange() is null; predicate uses OverlapCircleAll(maxRange) at line 54.
- Assets/Scripts/Skills/Shadowblade/Ambush.cs:32-39: false when not stealthed, or when Hemorrhage.FindNearestEnemy(transform.position, attackRange) is null.
- Assets/Scripts/Skills/Shadowblade/DeathMark.cs:23-27: false when SkillRuntime.FindNearestEnemy(transform.position, range) is null. Current code resets cooldown only, not resource.
- Assets/Scripts/Skills/Shadowblade/BackstabMark.cs:20-24: false when SkillRuntime.FindNearestEnemy(transform.position, range) is null. Current code resets cooldown only, not resource.
- Assets/Scripts/Skills/Shadowblade/VeilBurst.cs:28-29: false when SkillRuntime.FindNearestEnemy(transform.position, radius) is null before starting the burst.
- Assets/Scripts/Skills/Shadowblade/ChainCull.cs:30-41: false when no enemy in radius has BackstabMarked or RiftScar.
- Assets/Scripts/Skills/Ranger/FinalStrike.cs:18-39: false when no target in range is found. Current code resets cooldown only, not resource.
- Assets/Scripts/Skills/Ranger/MarkedDetonate.cs:20-33: false when no enemy in radius has RangerMarked stacks. Current code resets cooldown only, not resource.
- Assets/Scripts/Skills/Ranger/PredatorsMark.cs:21-29: false when EnemiesInCircle(transform.position, radius) is empty; otherwise it spends 30 for only a visual.
Related non-range config/state no-ops worth separate cleanup: projectilePrefab-null ranger skills, MirrorImage clonePrefab null, ExplosiveTrap maxTraps reached. These are not the Chain Lightning demo symptom but use the same hook well.

MED - Blink does not mirror PlayerController destination validation exactly.
Evidence:
- Assets/Scripts/Environment/WalkabilityMap.cs:265 has public bool IsDashableWorld(Vector3 worldPos).
- Assets/Scripts/Player/PlayerController.cs:397-404 wraps IsDashableWorld in IsReachableDashDestination and also requires IsReachableFromPlayer(cell) when floorTilemap exists.
- Assets/Scripts/Skills/Elementalist/Blink.cs:48 and :55 call IsDashableWorld directly and never check reachability.
Ruling: the API call exists and prevents non-walkable void landings, but it is not birebir with PlayerController.TryDash. Risk is lower than the original off-map bug, but disconnected walkable islands could still be valid blink destinations. Also, if snap fails and end=start, Blink still spends its resource/cooldown because this is handled in Execute after SkillBase spend.

LOW - RoomRunDirector bridge ordering is acceptable, with one caveat.
Evidence:
- Assets/Scripts/Core/RunStats.cs:169-172 OnRoomCleared only StartRunIfNeeded() and roomsCleared++.
- Assets/Scripts/Core/RunStats.cs:181-183 NotifyRoomCleared only calls OnRoomCleared(). No reward/event side effects.
- Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs:1264-1274 gates on lifecycle.MarkCleared(), invokes RoomCleared, then notifies RunStats.
- rg found no RoomRunDirector.RoomCleared.AddListener usages except encounterController.OnRoomCleared.AddListener(HandleEncounterCleared), so no observed listener reads RunStats in the stale window.
Ruling: no static double-count risk in the _Arena path. If a future listener on RoomRunDirector.RoomCleared reads RunStats immediately, it would see the pre-increment value because NotifyRoomCleared is after RoomCleared?.Invoke(). Not a current blocker.

Question answers

1. API correctness: yes, signatures are real. WalkabilityMap.ClampVelocityToWalkable(WalkabilityMap walkMap, Vector3 currentPos, Vector2 desiredVelocity, float dt) is at Assets/Scripts/Environment/WalkabilityMap.cs:295. WalkabilityMap.IsDashableWorld(Vector3 worldPos) is at :265. IronCharge/BladeRush mirror PlayerController/KnockbackReceiver velocity usage closely: PlayerController.cs:334/343 and KnockbackReceiver.cs:93 use the same static clamp pattern. Blink does not mirror PlayerController destination validation exactly because PlayerController uses private IsReachableDashDestination at PlayerController.cs:397-404, not raw IsDashableWorld only.

2. FIX 2 critical ruling: no, infra-only CanExecute does not solve the Chain Lightning no-enemy mana/cooldown loss. Required demo minimum is ChainLightning. Strongly recommended demo hardening is the target/state-gated list above. Demo-critical: yes for ChainLightning because it is the named symptom; likely high value for ShadowStep/MarkedDetonate/DeathMark-style spenders if those classes are playable in the demo.

3. FIX 3 double-count: OnRoomCleared only starts the run if needed and increments roomsCleared. NotifyRoomCleared mirrors only that. In _Arena, HandleEncounterCleared is guarded by lifecycle.MarkCleared(), and rg found no RoomLoader.OnRoomCleared emission in RoomRunDirector, so the static RunStats subscription and this bridge should not both fire for the same _Arena clear.

4. FIX 4 phase lock: logic is correct for the stated intent. phase2EnterTime is stamped at the 50 percent transition before DoPhaseTransition, and phase 3 checks both HP <= 33 percent and Time.time - phase2EnterTime >= 8f at PenitentSovereign.cs:245-247. If HP drops below 33 percent before 8 seconds, the boss remains in Phase 2 and the loop rechecks every iteration, so no static softlock unless the boss dies before the lock expires; death exits the loop normally.

5. Regression: SkillBase behavior changes only by adding a pre-spend hook that defaults true, so with no overrides current subclass behavior is unchanged. That is also the problem for FIX 2. Movement clamp should only restrict dash/charge when next frame would be unwalkable; ClampVelocityToWalkable returns desired velocity when walkMap is null and Vector2.zero/axis slide when blocked. Static risk: Blink direct IsDashableWorld is weaker than PlayerController reachability validation, and Blink can still spend on snap-cancel.

Overall: PASS-WITH-FIXES. Fix 1 velocity clamp, Fix 3 bridge, and Fix 4 phase lock are statically acceptable. Fix 2 needs at least ChainLightning.CanExecute before this can be PASS.
