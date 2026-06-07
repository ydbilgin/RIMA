# T2 Review Fix Summary — 2026-06-07

Commit: `d2e76d6a`  
Review source: `STAGING/_review_T2_axflash.md` (static review of commit `549e185b`)

## All 9 fixes applied

| ID | File | Change |
|---|---|---|
| HIGH-1 | `InputBufferService.cs` | Split `bufferWindow` into `dashBufferWindow=0.08f` + `attackBufferWindow=0.18f`; attack chain leniency restored to 180ms |
| HIGH-2 | `AudioManager.cs` | Added `SceneManager.sceneLoaded` hook; ambient starts only in chamber scenes (name-contains check), stops in all others |
| HIGH-3 | `HitPauseDriver.cs` + `ScreenShakeDriver.cs` | `TriggerExecutePause` guards `FeelToggleSettings.HitstopEnabled`; `TriggerExecuteShake` + `TriggerKnockdownShake` guard `FeelToggleSettings.ShakeEnabled` |
| MED-1 | `ExecutePromptDriver.cs` | Prompt label now parented to player (`SetParent(transform, false)`) — lifecycle tied to player, no MissingReferenceException |
| MED-2 | `Health.cs` | Added `HitImpactIcd=0.08f` internal cooldown; DoT/multi-hit ticks no longer spam HitImpact SFX |
| MIN-1 | `ExecutePromptDriver.cs` | `OverlapCircleNonAlloc` with pre-allocated `Collider2D[16]`; `TryGetComponent` replaces per-frame `GetComponent` |
| MIN-2 | `HitPauseDriver.cs` | `TriggerExecutePause` stops any running coroutine before starting; total freeze = pauseDurationExecute (0.10s), not 0.13s |
| MIN-3 | `AudioManager.cs` + `DeathBlow.cs` | `AudioManager.SuppressNextHitDeathSfx()` API; `DeathBlow.Execute` calls it before `DealDamage` to silence HitImpact+EnemyDeath on execute kill |
| MIN-4 | `SkillOfferUI.cs` | `_nextDraftHoverSfxTime` debounce (0.1s) on DraftHover SFX entry |

## Verification

- Compile: clean (no errors in console post-save)
- Knockback tests: 6/6 PASS
- Walkable/RoundTrip/Socket tests (LiveToolSmokeTests + RoomTemplateJsonRoundTripTests + WalkableEnforcementTests): 46/46 PASS
- Screenshot tests (ScreenshotModeTests): 1/1 PASS
- Pre-existing failures not caused by T2: PerformanceAntiPatternTests (2), BrushDataTests (2), CharacterSelectTests (3), MCPSceneLoadModalBypassTests (4), SubRoomSequenceControllerTests (2), PrefabHealthTests (2), PlayerAnimatorDirectionTests (2), V15g/V15h MapDesignerTests (3) — all pre-date `549e185b` and are unrelated to these fixes
