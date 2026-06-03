ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the files you identify as needed (4) BLOCKED if unclear.
UnityMCP read_console UNRELIABLE — do NOT block on it. Roslyn/static parse per file + report. Opus verifies compile via Editor.log. Do NOT self-certify.

# BUILD — PHASE 1 Batch C: input forgiveness + skill-hit feel parity (PURE .cs)

From `STAGING/DESIGN_LOCK_DEMO_S6.md` §5 P2 + §9 Opus decision #6. C# only — do NOT edit the scene.

## C1 — Attack input buffer (`Assets/Scripts/Combat/InputBufferService.cs` + caller)
- Today `InputBufferService.Pending` buffers Dash only (retry within 0.18s). Extend it with an **Attack** pending +
  `RequestAttack()`, so an LMB pressed during attack-commit/hitstop re-fires when the commit clears. Window **0.15-0.18s**
  (mirror the existing dash buffer). Wire the re-fire at the existing attack entry point (find the LMB/BasicAttack
  trigger in `PlayerController`/attack input). Do NOT change attack damage logic.

## C2 — Dash-edge "cliff grace" (`Assets/Scripts/Player/PlayerController.cs`)
- Add ~**0.10s** grace in `TryDash`: a dash requested within 0.10s of leaving a walkable cell still launches
  (diegetic "cliff-edge grace" for a floating island). Pair with existing `WalkabilityMap.IsDashableWorld` /
  `IsReachableFromPlayer`. Track last-walkable-time; do not let it enable dashing into the void permanently.

## C3 — Skill-hit feel parity (§9 #6)
- Problem: only `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs` publishes `CombatEventBus.OnHit`, so
  hitspark / hitstop / camera-punch / shake fire on BASIC attacks but are SILENT on SKILL / projectile hits.
- Find where skill & projectile damage is applied (grep for skill behaviors calling `Health.TakeDamage` /
  `ApplyMeleeHit` outside BasicAttack). At those damage sites, publish `CombatEventBus.OnHit` (mirror how
  BasicAttackBehaviorBase builds the hit event: position, hitDirection, isCrit/isFinisher=false for skills unless a
  skill defines it). Do NOT double-publish for basic attacks. If a single shared damage helper exists, prefer hooking
  there once. If the skill damage path is ambiguous, list what you found and mark C3 BLOCKED rather than guess.

## DELIVER (write to DONE file)
Per item: files changed (file:line), what changed, Roslyn/static parse result. Confirm NO scene edits.
List anything BLOCKED + the exact code locations you inspected. Concise.
