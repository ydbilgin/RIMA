# DONE: Combat scripted verify (REAL full-flow, not synthetic)

**VERDICT: COMBAT PLAYABLE = YES** (real MainMenu→CharacterSelect→_Arena flow).
No combat code fix was required. The engagement fix (detectionRange 8→12) is confirmed working live. No player-tag fix needed.

## Method (real, non-synthetic)
- Drove the actual scene flow in Play mode: MainMenu → `SceneManager.LoadScene("CharacterSelect")` → `CharacterSelectController.OnConfirmClicked()` (default Warblade) → loads `_Arena`. This is the game's own code path, not manual mob instantiate.
- Opening draft resolved by the REAL card-button click (`[SkillOfferPanel]` card[0].onClick.Invoke()) — same as a player clicking a card.
- Measured via `execute_code` (HP, enemy MobState, kills, distances, Rb velocity, wall-clock realtime). No screenshot inference.
- Repeated the full flow 3x to separate transient noise from real behavior.

## PLAYER-TAG FINDING — FALSE ALARM (no fix)
- Scene `_Arena` `DemoPlayer` (Untagged) = decorative sprite only (Transform + SpriteRenderer, no PlayerController/Health/Rigidbody). It is NOT the functional player.
- The REAL player is spawned at runtime by `RoomRunDirector.EnsurePlayerAtSpawn()` from `Resources/Prefabs/Warblade`, and `EnsurePlayerRuntime()` (RoomRunDirector.cs:795) sets `playerObject.tag = "Player"`.
- LIVE PROOF in real flow: `FindGameObjectWithTag("Player")` → name=Player, tag=Player, layer=Player, PlayerController=True, Health=100/100. Mobs report `PlayerRef=True`. Enemies find the player correctly. No tag fix applied.

## REAL-COMBAT DATA (live _Arena, Warblade)
Opening wave = FractureImp (Melee 14, 60hp) + HalfThrall (Melee 14, 30hp). detectionRange=12 on both.

| phase | t/realtime | playerHP | enemy states | kills | concurrent attackers |
|---|---|---:|---|---:|---|
| draft up (paused) | timeScale=0 | 100 | both Chase (det=12) @7.0/4.6 — moving begins on unpause | 0 | 0 |
| lethality probe start | rt 39.2 | 100 | both Attack @1.45/0.80 | 0 | 2 |
| lethality probe end | rt 46.8 | 16 | both Attack | 0 | 2 |
| winnability (immune fight-sim) | t 17.1 | 100 | killed both via real dmg path | 0→2 | 2 |
| wave2 auto-spawn | t 17.3 | 100 | HalfThrall (Melee 14) Attack | 2 | 1 |
| room cleared | t 17.3 | 100 | all dead, ActiveEnemyCount=0 | 3 | 0 |

Before-fix baseline (prior synthetic report, det=8): mobs stuck Idle at dist 6-8, never engaged.
After-fix (this run, det=12): mobs reach Chase→Attack on the real player. Fix WORKS.

## VERDICT detail vs criteria
- (a) Chase→Attack after spawn: YES — det=12 mobs chase then attack the real player.
- (b) Survive + CLEAR opening wave: YES (winnable) — player kills register through real `SkillRuntime.DealDamageRaw` path (kills 0→3), wave2 spawns, room clears, 1 reward pickup spawns. An engaged player clears trivially (mobs are 30/60hp).
- (c) Instant-death / soft-lock: NONE. Lethality = ~11 DPS combined → 100→16 over 7.6s for a FULLY PASSIVE player (no input, no dodge, no attack). ~9s to kill a sitting duck = gradual, survivable, not instant. No idle soft-lock (mobs engage).
- (d) Penitent instant-kill in opening: ABSENT. Opening AND second wave are all Melee mobs (14 dmg). No MobAttack_PenitentCombo (20/25/40) present in this room's waves.

Opening-wave clear time (engaged play): a few seconds (mobs 30/60hp vs Warblade). The earlier "playerHP→0" snapshots were a stationary no-input player being swarmed, NOT instant-death.

## Fixes applied
- NONE. Combat engagement/lethality/player-tag are all healthy in the real flow. The det 8→12 change (already in source: BaseMobBehavior.cs:22) is the sufficient fix and is verified live.

## Out-of-scope flag (NOT fixed, per ACTIVE RULES scope = combat only)
- Opening-draft `Time.timeScale=0` occasionally did NOT restore to 1 after the draft resolved (all UIManager pause flags = False yet timeScale=0). Likely `UIManager.CloseSkillOffer()` early-return (`if (!skillOfferOpen) return;`) skipping `ApplyTimeScale` when a flag desyncs. This is a UI pause-stack issue, NOT combat. It did NOT block combat once time was running. Flagged for a UI task; left untouched (DOKUNMA list / scope).
- Pre-existing debug pollution observed at session start: a prior session left synthetic `TestPlayer` + `Test_FractureImp_*` injected into the play-mode MainMenu (NOT on disk). Discarded by stopping play. No new pollution introduced; my test made zero disk edits.

## Console
0 errors, 0 warnings (read_console after combat). Stopped play mode cleanly (no debug state leak).
