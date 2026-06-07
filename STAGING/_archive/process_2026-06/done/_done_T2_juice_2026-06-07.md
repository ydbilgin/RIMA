# T2 Done — juice tuning + execute prompt + SFX pack + dash buffer

**Commit:** `feat(combat): juice tuning, execute prompt, sfx pack, dash input buffer`
**Date:** 2026-06-07

---

## Work Items Status

| # | Item | Status |
|---|------|--------|
| 1 | Juice value pass (light/heavy/execute/knockdown tiers) | DONE |
| 2 | [RMB] Execute world-prompt (ExecutePromptDriver) | DONE |
| 3 | 8-SFX package (CC0 Kenney clips + Resources/Audio) | DONE (18 clips total) |
| 4 | M1 dash input buffer ~80ms | DONE |
| 5 | Verify (compile + tests) | DONE — 0 compile errors, KnockbackTests 16/16, WalkableEnforcementTests 16/16, ScreenshotModeTests+SocketTests+RoundTripTests 7/7, LiveToolSmokeTests 31/31 |
| 6 | Commit + summary | DONE |

---

## 1. Juice Value Pass — Old vs New Values

### HitPauseDriver (`Assets/Scripts/Combat/Juice/HitPauseDriver.cs`)

| Tier | Old | New |
|------|-----|-----|
| hit (light) | 0.04s | **0.03s** |
| crit (heavy) | 0.07s | **0.06s** |
| kill | 0.12s | 0.12s (unchanged) |
| finisher | 0.18s | 0.18s (unchanged) |
| execute | — | **0.10s** (new) |
| boss death | 0.20s | 0.20s (unchanged) |

Added: `TriggerExecutePause()` public method called by ExecutePromptDriver.

### ScreenShakeDriver (`Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs`)

| Tier | Magnitude Old | Magnitude New | Duration Old | Duration New |
|------|--------------|--------------|-------------|-------------|
| hit (S) | 0.05 | **0.04** | 0.10 | 0.10 |
| crit (M) | 0.12 | **0.10** | 0.18 | 0.18 |
| commitBeat3 (L) | 0.15 | **0.18** | 0.20 | **0.22** |
| kill (M) | 0.10 | 0.10 | 0.15 | 0.15 |
| knockdown (M) | — | **0.13** | — | **0.22** (new) |
| execute (L) | — | **0.18** | — | **0.28** (new) |

Added: `TriggerKnockdownShake()`, `TriggerExecuteShake()` public methods.

---

## 2. [RMB] Execute World-Prompt

**New file:** `Assets/Scripts/Combat/ExecutePromptDriver.cs`

- Attaches to player (added to `Warblade.prefab` + `ChamberSelectBootstrap.EnsurePlayerRuntime`)
- Polls enemies via `Physics2D.OverlapCircleAll(detectRadius=2f, enemyLayer)` in Update
- Shows world-space `TextMeshPro` label "[RMB] İnfaz" above nearest Broken/Sundered target
- Color: warm gold `#FFF399` (NOT cyan — cyan reserved for player/Rift)
- Pulsing alpha at 6Hz for visibility
- Static `OnExecuteFired()` called by `DeathBlow.Execute()`:
  - `AudioManager.Play(Sfx.ExecutePayoff)`
  - `HitPauseDriver.Instance?.TriggerExecutePause()` (0.10s freeze)
  - `ScreenShakeDriver.Instance?.TriggerExecuteShake()` (L-tier shake)

---

## 3. SFX Pack

**Source:** Pre-existing Kenney CC0 clips in `Assets/Audio/SFX/` (packs: Impact Sounds, Interface Sounds, RPG Audio)
**License file:** `Assets/Audio/_licenses/CC0_SOURCES.txt`
**Destination:** `Assets/Resources/Audio/` (18 .wav files named by Sfx enum)

### Clip Mapping

| Sfx enum | Resources/Audio file | Source file |
|----------|---------------------|-------------|
| Hit | Hit.wav | sfx_enemy_hit.wav |
| Shatter | Shatter.wav | sfx_enemy_death_shard.wav |
| Dash | Dash.wav | sfx_dash.wav |
| Cast | Cast.wav | sfx_cleave.wav |
| DraftSelect | DraftSelect.wav | sfx_skill_select.wav |
| GateOpen | GateOpen.wav | sfx_room_clear.wav |
| Death | Death.wav | sfx_player_death.wav |
| BossIntro | BossIntro.wav | sfx_boss_intro.wav |
| Finisher | Finisher.wav | sfx_ground_slam.wav |
| **SwingLight** | SwingLight.wav | sfx_hit_light.wav |
| **SwingHeavy** | SwingHeavy.wav | sfx_cleave.wav |
| **HitImpact** | HitImpact.wav | sfx_enemy_hit.wav |
| **EnemyDeath** | EnemyDeath.wav | sfx_enemy_death_void.wav |
| **ExecutePayoff** | ExecutePayoff.wav | sfx_boss_death.wav |
| **RoomClear** | RoomClear.wav | sfx_room_clear.wav |
| **DraftHover** | DraftHover.wav | sfx_ui_hover.wav |
| **ChamberAmbient** | ChamberAmbient.wav | sfx_iron_charge.wav |
| **KnockdownThud** | KnockdownThud.wav | sfx_ground_slam.wav |

Bold = new Sfx enum additions.

### SFX Wire Points

| Event | SFX | File |
|-------|-----|------|
| Enemy hit (all) | HitImpact | Health.TakeDamage |
| Player death | Death | Health.TakeDamage |
| Enemy death | EnemyDeath | Health.TakeDamage + BasicAttackBehaviorBase |
| Light swing (M1 steps 1-2) | SwingLight | BasicAttackBehaviorBase.ApplyMeleeHit |
| Heavy swing (M1 step 3/finisher) | SwingHeavy | BasicAttackBehaviorBase.ApplyMeleeHit |
| Finisher beat | Finisher | BasicAttackBehaviorBase.ApplyMeleeHit |
| Execute land | ExecutePayoff | ExecutePromptDriver.OnExecuteFired |
| Knockdown start | KnockdownThud | KnockdownDriver.DoKnockdown |
| Dash | Dash | PlayerController.TryDash (pre-existing) |
| Room cleared | RoomClear | RoomRunDirector.HandleEncounterCleared |
| Gate/portal open | GateOpen | Gate.Unlock (pre-existing) |
| Draft hover | DraftHover | SkillOfferUI.SetPointerHover |
| Draft select | DraftSelect | DraftManager (pre-existing) |
| Chamber ambient | ChamberAmbient | AudioManager.TryPlayAmbient (loop, if real clip present) |

---

## 4. Dash Input Buffer

**File:** `Assets/Scripts/Combat/InputBufferService.cs`

| Field | Old | New |
|-------|-----|-----|
| `bufferWindow` | 0.18s | **0.08s** (≈80ms) |

The buffer already existed and is called by `PlayerController.TryDash()` → `inputBuffer?.RequestDash()` when mid-commit blocks the dash. No new code required; only the window duration was tuned.

---

## Test Results

| Suite | Pass | Total | Notes |
|-------|------|-------|-------|
| KnockbackTests | 16 | 16 | Green |
| WalkableEnforcementTests | 16 | 16 | Green |
| ScreenshotModeTests + SocketTests + RoundTripTests | 7 | 7 | Green |
| LiveToolSmokeTests | 31 | 31 | Green |
| Full EditMode (RIMA.Tests.EditMode) | 432 | 451 | 19 pre-existing failures (PixelLab STAGING missing, DontDestroyOnLoad-in-editmode, Wang v2 PNG missing, CameraFollow.Find) — none from T2 |

**Pre-existing failures confirmed unrelated:** V15g PixelLab pool test (STAGING/pixellab_dirt_v1 missing), V15h Wang PNG, CharacterSelectScreen DontDestroyOnLoad-in-editmode, MCPSceneLoad private method mismatch, PerformanceAntiPattern CameraFollow.cs (pre-existing), PlayerAnimatorDirection (Warblade controller missing), PrefabHealth RewardPickup field missing, SubRoomSequenceController DontDestroyOnLoad + state.

---

## Files Changed

- `Assets/Scripts/Audio/AudioManager.cs` — Sfx enum +9 values, TryPlayAmbient, PlayLooped, procedural fallbacks
- `Assets/Scripts/Combat/Juice/HitPauseDriver.cs` — value tuning, TriggerExecutePause
- `Assets/Scripts/Combat/Juice/ScreenShakeDriver.cs` — value tuning, TriggerKnockdownShake, TriggerExecuteShake
- `Assets/Scripts/Combat/InputBufferService.cs` — bufferWindow 0.18→0.08
- `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs` — SwingLight/SwingHeavy/EnemyDeath SFX
- `Assets/Scripts/Core/Health.cs` — HitImpact/EnemyDeath/Death SFX routing
- `Assets/Scripts/Core/KnockdownDriver.cs` — KnockdownThud SFX + TriggerKnockdownShake
- `Assets/Scripts/UI/SkillOfferUI.cs` — DraftHover SFX on pointer enter
- `Assets/Scripts/Skills/Warblade/DeathBlow.cs` — OnExecuteFired call
- `Assets/Scripts/UI/ChamberSelectBootstrap.cs` — ExecutePromptDriver add in EnsurePlayerRuntime
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` — RoomClear SFX on encounter clear
- `Assets/Scripts/Combat/ExecutePromptDriver.cs` — NEW: world-prompt + execute juice orchestrator
- `Assets/Resources/Audio/` — NEW: 18 .wav clips (CC0 Kenney)
- `Assets/Audio/_licenses/CC0_SOURCES.txt` — NEW: license attribution
- `Assets/Resources/Prefabs/Warblade.prefab` — ExecutePromptDriver component added
