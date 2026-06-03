# Review: BREAK->EXECUTE loop (A1b auto-convert + A2 commit-beat-target + A3 execute payoff) — CX

ACTIVE RULES: (1) think before judging (2) real issues only, file:line + concrete fix (3) reviewer not writer (4) BLOCKED if can't read.

NLM ACCESS (verify canon claims if needed):
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"

## Files
- `Assets/Scripts/Skills/SkillStateTracker.cs` (A1b: Broken×3 -> Sundered auto-convert, recursive Apply)
- `Assets/Scripts/Combat/CombatEventBus.cs` (A2: added `target` field on commit-beat payload)
- `Assets/Scripts/Combat/CombatHandler.cs` (A2: target resolve via OverlapCircle("Enemy"), applies Broken)
- `Assets/Scripts/Player/PlayerAttack.cs` (A2: `TryGetFinisherReach` accessor)
- `Assets/Scripts/Skills/Warblade/DeathBlow.cs` (A3: canon gate, state Consume on execute)
- `Assets/Scripts/Skills/Warblade/BattleSurge.cs` (A3: rewritten event-driven per-spend heal)

## Review focus — PASS/FAIL + file:line
1. **A1b recursion safety:** Broken stacks>=3 removes Broken + recursively `Apply(Sundered)`. Confirm NO infinite recursion / re-entrancy (Sundered path must not re-trigger Broken branch), and OnStateExpired("Broken")+OnStateEntered("Sundered") fire exactly once (the A1 tell upgrades, no double shard-burst).
2. **A2 target resolve:** `OverlapCircleAll(hitCenter, radius, GetMask("Enemy"))` with hitCenter/reach mirroring the basic-attack FINISHER — confirm it matches `BasicAttackBehaviorBase.ApplyMeleeHit` geometry (so the commit beat hits what the swing hit). Nearest-LIVE-enemy selection (skips dead/null Health). `CommitBeatEvent.target` additive (existing HitPause/Shake/VFX subscribers unaffected). CombatHandler is on player root — confirm PlayerAttack cache is valid.
3. **A3 BattleSurge rewrite:** event-driven on `RageSystem.OnRageChanged`, +5% MaxHP per spend, 2s internal cooldown, only while buff active. Confirm: no double-heal on a single spend, ICD enforced, null-guarded subscribe/unsubscribe (OnRageChanged is a serialized UnityEvent — subscribe in OnEnable, unsubscribe OnDisable, no leak), heal clamped to MaxHP.
4. **A3 DeathBlow gate:** generic HP<30% execute now behind `allowLowHpExecute=false`; execute REQUIRES Broken/Sundered; state Consume on execute (prefers Sundered) prevents infinite free re-execute. Confirm DeathBlow still works on Broken/Sundered targets and the consume can't underflow/throw.
5. **General:** no cyan in tells; Enemy layer used; no regression to existing SkillStateTracker/CombatEventBus readers; no per-frame alloc introduced in CombatHandler commit path.

## Output
Top line `STATUS: PASS`/`FAIL`, then findings. Tight.
