**COMBAT: BROKEN** - Data does not support a healthy mid-combat gate. Opening draft can be skipped and the player does not instantly die at natural spawn, but enemies can remain idle at spawn distances, and forced non-overlap engagement repeatedly produced rapid death. A controlled runtime damage probe did prove kill accounting can increment through `SkillRuntime.DealDamageRaw`, but that is not proof of healthy player-driven combat.

## Method
- Loaded `_Arena` through full flow: MainMenu -> CharacterSelect -> `_Arena`.
- Picked one opening draft card programmatically.
- Sampled combat state via Unity MCP `execute_code`; no screenshot inference used for verdict.
- Captured one valid alive/non-draft/non-death screenshot after data-confirming the state.
- Checked Unity console errors/warnings at end: 0 entries.

## Measurement Table

Times are `RunStats.RunTimeSeconds` from the fresh post-draft runs. The first table is natural post-draft survival without teleport/input.

| sample | t | playerHP | aliveEnemies | kills | enemy states / notes |
|---|---:|---:|---:|---:|---|
| S0 | 47.2 | 100/100 | 2 | 0 | FractureImp Idle dist 8.41 visible; HalfThrall Idle dist 6.05 visible |
| S1 | 71.6 | 100/100 | 2 | 0 | same: both Idle, no velocity, visible |
| S2 | 106.3 | 100/100 | 2 | 0 | same: both Idle, no velocity, visible |
| S3 | 140.1 | 100/100 | 2 | 0 | same: both Idle, no velocity, visible |

Engagement probes:

| sample | t | playerHP | aliveEnemies | kills | notes |
|---|---:|---:|---:|---:|---|
| forced Penitent range | 208.6 | 0/100 | 1 | 0 | player placed 1.25 from Penitent; Penitent Attack; DeathScreen active after ~4s wall wait |
| forced clustered range | 170.9 | 0/100 | 2 | 0 | player placed 1.25 from HalfThrall; FractureImp also at 1.47, both Attack; DeathScreen active after ~3s wall wait |
| final pre-shot | 42.8 | 100/100 | 1 | 0 | Penitent Attack at dist 1.49; draft false; death false |
| controlled kill probe | 98.1 | 100/100 | 2 | 1 | `SkillRuntime.DealDamageRaw` killed Penitent; kills 0 -> 1; death false |

## Validation
- Player survives >=15s after opening draft if left at natural spawn: yes, HP stayed 100/100 through S0-S3.
- At least 1 enemy can be killed and `RunStats.Kills` can increase: yes, controlled damage probe changed kills 0 -> 1 while player stayed 100/100.
- Enemies damage but do not instant-kill: not healthy. Natural spawn did not damage because enemies stayed idle. Forced range engagement killed player rapidly.
- Enemies visible + active + not idle: inconsistent. Final pre-shot had Penitent `Attack`, visible, player alive. Natural post-draft survival had enemies visible but Idle.

## Instant-Death / Soft-Lock Findings
- Prior death at KILLS 0 / time 00:00 was not reproduced by simply entering `_Arena` and skipping draft. Natural post-draft state stayed alive.
- Spawn-overlap was not observed in natural data. Natural distances were 8.41 and 6.05 in one run; no immediate HP loss.
- Draft-skip -> death was not reproduced directly. Draft hidden and player HP stayed full in natural samples.
- Environment/fall or invalid-position death remains a candidate for manually placed test positions, but the organic spawn did not prove it.
- Combat reachability is broken/inconclusive: natural spawn often leaves enemies idle outside their active behavior, while forced engagement can immediately become lethal due clustered enemies.

## Likely Code Surfaces
- `Assets/Scripts/Enemies/BaseMobBehavior.cs:22` / `:23` define `detectionRange` and `attackRange`.
- `Assets/Scripts/Enemies/BaseMobBehavior.cs:175`-`:182` switches enemies to Idle/Chase/Attack by distance.
- `Assets/Scripts/Enemies/Attacks/MobAttack_PenitentCombo.cs:19`-`:21` define Penitent combo damage 20/25/40.
- `Assets/Scripts/Enemies/Attacks/MobAttack_PenitentCombo.cs:99`-`:109` applies overlap damage to player `Health`.
- `Assets/Scripts/Core/Health.cs:49` applies HP damage and death.

## Screenshot
- `capture_v3_combat/combat_health_final_alive_penitent_attack.png`
- Unity original: `Assets/Screenshots/capture_v3_combat_combat_health_final_alive_penitent_attack.png`

Semantic screenshot state before capture:
`FINAL_PRE_SHOT t=42.8 hp=100/100 aliveEnemies=1 kills=0 draftActive=False deathActive=False ... Penitent(Clone):hp=100/100,state=Attack,dist=1.49,visible=True`

## Recommendation
Do not start CombatJuice beautification yet. First fix/decide the room-1 combat gate: enemy spawn distances/detection should produce actual reachable combat without idle soft-lock, and first engagement should not collapse into unavoidable rapid death from clustered simultaneous attacks. No code fix was applied.
