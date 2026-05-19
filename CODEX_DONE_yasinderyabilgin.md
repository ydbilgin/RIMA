# CODEX DONE - yasinderyabilgin

Task: Combat Juice P0/P1/P2

Changed files:
- Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs: added CombatEventBus PublishHit after successful melee damage, marks combo finisher as isCrit, publishes KillEvent when target dies, kept legacy juice calls intact.
- Assets/Scripts/Player/PlayerController.cs: cached Health, enables dash immunity at dash start, restores previous IsImmune state at dash end.
- Assets/Scripts/Enemies/EnemyAI.cs: added 0.35s attack windup with EnemyTelegraph.SpawnCircle, enemy remains stopped during windup, damage applies after windup.

Diff stat:
- 3 files changed, 42 insertions, 5 deletions.

Validation:
- Checked VFXBusDemo HitEvent/KillEvent schema and matched fields.
- Ran: dotnet build .\Assembly-CSharp.csproj
- Result: build succeeded, 0 errors. Existing project warnings remain.
