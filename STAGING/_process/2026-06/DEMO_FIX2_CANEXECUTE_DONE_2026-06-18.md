# FIX-2 DONE: SkillBase CanExecute() override wiring (2026-06-18)

Council fix-up complete. FIX-2 infra (`SkillBase.CanExecute()` veto hook, checked in `TryActivate()`
before cost/cooldown) is now actually used by the range-gated skills. 4 of 5 target skills wired,
1 skipped per critical rule 3. Side-effect-free, value-mirrored from each Execute().

## Infra confirmed (already present, uncommitted)
`Assets/Scripts/Skills/Base/SkillBase.cs`:
- `protected virtual bool CanExecute() => true;` (line 80)
- `TryActivate()` calls `if (!CanExecute()) return false;` (line 91) BEFORE rage/resource spend
  and cooldown. Default `true` keeps all non-overriding skills identical to legacy.

## Wired (4)

### 1. ChainLightning.cs — ASIL demo bug (Elementalist)
- **Added:** `protected override bool CanExecute() => FindNearestEnemy(transform.position, null) != null;`
- **Execute no-op source:** `var first = FindNearestEnemy(transform.position, null); if (first == null) return;` (Execute line ~37, after the wiring shift).
- **Read-only?** Yes. `FindNearestEnemy` = `Physics2D.OverlapCircleAll(from, jumpRange)` + filtering
  (skip "Player" tag, skip excluded set, require `Health`); pure query, no state/cost/VFX.
- **Note:** Execute fires `SkillVfx.CastFlash(...)` BEFORE the null-check — that cosmetic side effect
  is intentionally NOT replicated in CanExecute (query only). Mirrors `jumpRange` exactly.

### 2. CripplingBlow.cs — melee (Warblade)
- **Added:** `protected override bool CanExecute() => FindNearest(range) != null;`
- **Execute no-op source:** `var target = FindNearest(range); if (target == null) return;`
- **Read-only?** Yes. `FindNearest(radius)` = `OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"))`
  + player-exclusion + non-dead `Health` filter. Mirrors `range` + "Enemy" mask exactly.

### 3. SunderMark.cs — ranged throw (Warblade)
- **Added:** `protected override bool CanExecute() => FindNearest(range) != null;`
- **Execute no-op source:** `var target = FindNearest(range); if (target == null) return;` (PRIMARY gate).
- **Read-only?** Yes. Same `FindNearest` shape as CripplingBlow; mirrors `range` (10f) + "Enemy" mask.
- **Secondary guard NOT mirrored:** Execute also returns if the found target lacks a
  `StatusEffectSystem`. Deliberately left out of CanExecute to avoid a false-positive block of a
  valid cast (rule 2: mana waste < wrongly blocking). Primary range gate covers the demo "dead button".

### 4. DeepWound.cs — melee (Warblade)
- **Added:** `protected override bool CanExecute() => HasEnemyInRange();` + a new private read-only
  helper `HasEnemyInRange()`.
- **Execute no-op source:** inline `OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"))`
  loop → `if (target == null) return;` (Execute uses an inline scan, not a `FindNearest` helper).
- **Why a helper:** Execute scans inline, so to keep CanExecute side-effect-free I added
  `HasEnemyInRange()` that duplicates the EXACT same filter (same `range`, "Enemy" mask,
  player-exclusion, non-dead `Health`) and returns a bool. Execute's inline loop is untouched
  (no behavior change). Mirrors `range` (2f) + "Enemy" mask exactly.

## Skipped (1)

### 5. IronCounter.cs — UNCERTAIN/NOT target-gated → SKIPPED (rule 3 + rule 2)
- **Reason:** Execute() does NOT have a target-in-range no-op. It is a reactive parry:
  `Execute()` sets `counterCharges` and `StartCoroutine(ParryWindow())` — it ALWAYS starts the
  0.8s window regardless of enemies. The enemy search (`OverlapCircleAll(counterRange, "Enemy")`)
  happens later inside `TriggerCounter()`, only when the player takes damage during the window.
- A CanExecute gate on "enemy in counterRange now" would be a FALSE POSITIVE: a valid parry setup
  could be blocked when no enemy is currently in range even though one may approach (or already be
  attacking) during the 0.8s window. Per critical rule 3 (self-buff/reactive → don't add) and
  rule 2 (no false-positive blocking), override NOT added.

## Verification
- Recompile: `refresh_unity(compile=request, scope=scripts, mode=force)` → state went to `compiling`.
- `editor_state`: `is_compiling=false`, `is_domain_reload_pending=false`, `ready_for_tools=true`,
  active scene `_Arena` not playing/dirty (NOT touched — code-only).
- `read_console(types=[error,warning])`: **0 entries** (0 errors, 0 warnings) after domain reload.
- Did NOT enter Play mode. Scene untouched/unsaved.

## Files changed (code-only, 4 files)
- `Assets/Scripts/Skills/Elementalist/ChainLightning.cs` (+8 lines: CanExecute override)
- `Assets/Scripts/Skills/Warblade/CripplingBlow.cs` (+8 lines: CanExecute override)
- `Assets/Scripts/Skills/Warblade/SunderMark.cs` (+9 lines: CanExecute override)
- `Assets/Scripts/Skills/Warblade/DeepWound.cs` (+17 lines: CanExecute override + HasEnemyInRange helper)
- `Assets/Scripts/Skills/Warblade/IronCounter.cs` — UNCHANGED (skipped, see above)

## Deviations
None vs spec. SunderMark secondary StatusEffectSystem guard intentionally not mirrored (rationale
above, aligns with rule 2). DeepWound required a new read-only helper because Execute scans inline
rather than via a shared `FindNearest` — additive only, Execute behavior unchanged.
