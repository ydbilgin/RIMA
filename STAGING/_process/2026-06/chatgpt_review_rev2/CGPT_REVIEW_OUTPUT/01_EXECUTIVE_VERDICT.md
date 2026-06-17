# 01 — Executive Verdict

## Current go/no-go

**Conditional NO-GO until combat full-flow passes.**

The `detectionRange 8 → 12` change fixes a real engagement bug, but it does not prove the complete combat chain. The demo must not be considered stable until the following sequence works from a cold start:

`target resolve → chase → attack → player damage → enemy damage/death → wave completion → reward draft → run-map/next room → boss`

## Top blockers in order

1. **Canonical runtime Player tag/target resolution**
   - `BaseMobBehavior` and several attacks depend on `Player` tag.
   - `_Arena` contains an untagged `DemoPlayer` risk.
   - `PlayerController.Awake()` sets the physics layer but does not guarantee the tag.
   - Verify the actual runtime player object, not only the prefab.

2. **Boss player re-acquire**
   - `PenitentSovereign` resolves Player once in `Start()`.
   - If spawn order or transition loses the reference, the boss loop can remain idle forever.

3. **AttackTokenManager lifecycle**
   - `OnDestroy()` sets `_shuttingDown = true`.
   - The manager does not visibly use `DontDestroyOnLoad`.
   - A scene transition may permanently stop token acquisition until subsystem reset.

4. **Penitent opening-wave lethality**
   - Combo total: `20 + 25 + 40 = 85` against a 100 HP player.
   - Token gating limits simultaneous attackers; it does not reduce burst damage.
   - The encounter entry can consume the complete opening budget.

5. **Screenshot evidence is currently unreliable**
   - Exact SHA uniqueness passed.
   - Semantic/state correctness failed for most critical proof shots.

## Two-day priority

### P0: make the game demonstrably work

- Player tag/target resolver
- Boss re-acquire
- AttackTokenManager lifecycle
- Penitent opening/damage adjustment
- Three cold full-flow runs
- Zero Console errors

### P1: highest-value presentation improvements

- CombatJuice in `_Arena`
- Enemy outline/contrast
- ChainExplosion / Wrath / Charge telegraphs
- One clean Edit-to-Play before/after demonstration

### P2: recapture evidence

Capture only after P0/P1 passes. Do not create another 35-shot pile of mislabeled states. A smaller truthful set is stronger.
