# RIMA — AUTOMATED BALANCE / DIFFICULTY TESTING (S6)

> Status: proposal + first guardrail shipped. Scope: 5-room demo vertical slice.
> Companion test: `Assets/Tests/EditMode/DifficultyBalanceTests.cs` (pure-math EditMode guardrail).

Goal: catch balance regressions automatically so a tuning pass (mob HP, player DPS, mob
damage, wave size) cannot silently make a room unwinnable, unfair, or a faceroll — without a
human replaying the whole demo each time.

---

## 1. Three layers, cheapest first

### Layer A — Pure-math EditMode guardrail (SHIPPED)
`DifficultyBalanceTests.cs`. No scene, no PlayMode, no MonoBehaviour. Encodes the canonical
encounter (R1 = 3 FractureImp @ 60 HP) plus real combat constants read from code
(`BasicAttackProfile` combo {25,30,40}/1.2s, `MobAttack_Melee` dmg 14, `BaseMobBehavior`
cd 1.5s, `Health` maxHP 100) and asserts the **relationship** that defines "winnable first try":

- `timeToClear = totalMobHP / playerDPS` is inside a tutorial window (1.5s..20s),
- `timeToClear < timeToDie` (player clears the room before the room kills the player),
- player ends the clear with a > 25% HP margin (a tutorial win is comfortable, not a photo finish),
- player effective DPS > room incoming DPS (offense-favored, no attrition stall).

Runs in milliseconds, no Editor focus needed, perfect for CI. It is a tripwire, not a simulator:
if it goes red after retuning, the curve drifted out of the "winnable tutorial" band.
Assumptions (`ASSUMED_AttackUptime`, `ASSUMED_SimultaneousAttackers`) are named constants with
rationale comments — adjust them as playtest data arrives, don't fudge the thresholds.

**Extend per room:** add `R2_*`/`R3_*` constants from the §2.2 table (R2 = 4 Imp -> 2 Imp + 1
ShardWalker; R3 = 2 ShardWalker + 2 Imp -> 1 Hulk + 3 Imp) and assert a MONOTONIC difficulty
curve: `timeToClear(R1) < timeToClear(R2) < timeToClear(R3)` and `hpMargin` shrinks room over
room. That single curve test guards the whole pacing intent ("monotype -> +ranged -> +tank/mix").

### Layer B — Headless combat sim (recommended next)
A pure-C# (or EditMode) deterministic mini-simulator that steps a fixed-dt loop over plain data
objects (no GameObjects): player {hp, dps, dashCd}, mobs {hp, dmg, cd, range}, a simple
approach/attack/cooldown state machine, and `AttackTokenManager`-style concurrency gating.
Run N seeds per room, log per-run metrics, assert aggregate distributions (e.g. "R1 win rate
== 100%, median clear 4-8s"; "R3 win rate 70-95%"). This bridges the gap between Layer A's
closed-form formula and a full PlayMode run — still fast, still CI-able, but models target
switching and token gating that the formula approximates. Keep the sim's constants sourced from
the same SO/profile assets so it never diverges from the real game.

### Layer C — PlayMode integration test (real combat, slow)
A `[UnityTest]` PlayMode test that loads the real combat scene, jumps to a room, spawns the
canonical wave, and drives an autopilot player (scripted "attack nearest + dash on telegraph")
until the room clears or the player dies. Validates the actual `Health`/`BaseMobBehavior`/
`PlayerAttack`/`AttackTokenManager` interaction, not a model of it. Slowest — reserve for a
nightly job and the boss fight, where emergent timing (telegraph windows, Phase-3 overlay)
can't be captured by a formula.

---

## 2. Driving Layer C via JumpToRoom

The project already has dev jump tooling (F5 boot / JumpToRoom dev-tools per memory). Reuse it
so a PlayMode test does not depend on full demo flow:

1. Load the combat scene (`manage_scene` / `EditorSceneManager` in setup).
2. Enter PlayMode (`yield return new EnterPlayMode()` in a `[UnityTest]`).
3. Call the existing JumpToRoom entry point with the target room id (R1..R5) — same path the F-key
   dev jump uses — so the encounter spawns in isolation, no upstream rooms required.
4. Attach a minimal `AutopilotAgent` to the player: each frame, face+attack nearest live mob,
   dash when a mob telegraph is active within range. Deterministic, no AI nondeterminism.
5. `yield` until `room cleared` event OR player `Health` hits 0 OR a timeout (e.g. 60s) trips.
6. Assert outcome + emit metrics (below).

The boss room (R5) gets its own test that additionally asserts the phase gates fire (50% chains
break -> P2, 33% -> P3 overlay) and death routes to Victory (`RaiseDemoComplete`), not class-select.

---

## 3. Metrics to log (every automated run)

| Metric | Why |
|---|---|
| `TTK` per mob (time-to-kill) | detects an HP/DPS retune making a single enemy a slog |
| `timeToClear` per room | pacing; must rise R1->R3, dip at R4 (reward), spike at R5 (boss) |
| `hitsTaken` / `damageTaken` per room | fairness; tutorial rooms should stay low |
| `hpRemaining` / `hpMargin %` at clear | "winnable first try" comfort; flags photo-finish rooms |
| `deaths` (win/loss flag) | hard gate — R1/R2 must be 0 deaths on autopilot |
| `win rate` over N seeds (Layer B) | distributional difficulty, not a single lucky run |
| boss: `timeInEachPhase`, `phaseGatesFired` | confirms 90-120s target + 2+1 phase structure |
| `peakConcurrentAttackers` | guards the AttackTokenManager on-screen cap (<=5 R1-2, <=6 R3) |

Emit as a single JSON/CSV line per room per run so a loop can diff runs over time and a human
can eyeball the curve. Store under `STAGING/balance_runs/` (gitignored or pruned).

---

## 4. CI / loop idea

- **PR gate (fast):** run Layer A (`DifficultyBalanceTests` + the future curve test) on every
  push via `unity-test-runner` EditMode. Fails the PR if any room leaves the winnable band.
  Sub-second, no GPU, no Editor focus.
- **Nightly (full):** run Layer B sim across all 5 rooms x N seeds, then Layer C PlayMode for R1,
  R3, and R5. Append metrics to `STAGING/balance_runs/<date>.csv`; post a one-line delta vs the
  previous night ("R3 clear +1.2s, R3 win rate 88% -> 81%") so balance drift is visible without
  re-reading the whole file.
- **On-demand:** a `/loop` or dispatch task that re-runs Layer B after any combat-constant edit,
  so a tuning change gets an instant winnability read before it ships.

Single source of truth: every layer reads the same `BasicAttackProfile` / mob SO / `Health`
values the game uses at runtime, so the tests can never quietly disagree with the build.
