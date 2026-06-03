# RIMA — Game State & Critical Path to a Playable Demo (2026-06-02)

> Source: `rima-state-audit` workflow (7 read-only subsystem audits + Opus synthesis, run wf_4bb08d37-81b). Owner-deferred (do NOT do now): animations beyond idle, audio, StS graph/preview/orb layer, mob VISUAL upgrades. Sequencing locked earlier by user: safe items first.

## 1. VERDICT
RIMA is a **playable vertical-slice skeleton**: MainMenu → CharacterSelect → 3 maps → Victory/Death runs end-to-end (runtime-verified spine). But it is **hollow in the middle** — combat lands damage with zero feel (no hitstop/shake/damage-numbers/hit-flash/knockback/slash-arc; the signature Sundered Beat finisher never fires), the 3 maps are content-identical duplicates, and pressing **PLAY AGAIN soft-locks the run**. Demos as a tech proof, not yet as a game.

## 2. STATE TABLE
| Subsystem | State | Impact | Note |
|---|---|---|---|
| Scene Flow & Run Loop | 🟡 wired-rough | major | Driven by `[Obsolete]` RuntimeRoomManager; intended `Systems/Map/RoomLoader` is in NO scene; PLAY AGAIN reloads mid-run map. |
| Combat, Skills & Juice | 🔴 stub/broken | major | Damage works; all 5 juice drivers + `CombatHandler` finisher absent from live scenes — every hit feels dead. |
| Mobs / Enemies | 🟡 wired-rough | major | Mobs chase/attack/die, but `KnockbackReceiver` + `HitFlashDriver` missing on ALL enemy prefabs — no hit confirmation. |
| UI / HUD / Menus | ✅ solid | minor | 5 screens build, NREs fixed; gaps = dead assets + unverified Pack-bar/vignette import settings. |
| Player & Classes | 🟡 wired-rough | minor | 4 unlocked classes solid; dual `IsoSorter`+`YSortBehaviour` sort race on Player; Ronin has no animator. |
| Environment / Map / Graph | 🟡 wired-rough | minor | Iso art solid; 3 maps duplicates; hazards + StS graph deferred (correct); reward spawn off-camera risk. |
| Bugs / Tech-Debt / Integrity | 🔴 stub/broken | major | PLAY AGAIN soft-lock + timeScale leak + elite teleport wall-escape + dual room-clear conflict. |

## 3. CRITICAL PATH

### P0 — must-fix so the demo isn't hollow or broken

**Run-loop integrity:**
- [ ] PLAY AGAIN soft-lock: `DemoCompleteOverlay.Restart()` must `MapFlowManager.Instance?.ResetRun()` then load MainMenu/`startSceneName`, not reload active buildIndex. — `Core/DemoCompleteOverlay.cs:116-119`
- [ ] Add `OnDestroy()` to `DemoCompleteOverlay` → `Time.timeScale = 1f` (freeze-on-destroy leak). — `Core/DemoCompleteOverlay.cs:39`
- [ ] Resolve dual room-clear owner: remove `[Obsolete]` RuntimeRoomManager **component** from all 3 iso scenes (keep the GameObject as DungeonGraph/DraftManager container); let `RoomClearVictoryTrigger`+`MapFlowManager` own flow. — `_IsoGame.unity:601`, Map02/03:601, `RuntimeRoomManager.cs:1143`
- [ ] After removal verify door/gate still completes + `OnRoomCleared` fires exactly once (kills double RunStats increment). — `DoorTrigger.cs:113-116`, `RoomClearVictoryTrigger.cs:86-108`
- [ ] Elite teleport wall-escape: `Physics2D.OverlapCircle` bounds check before `transform.position = newPos`. Prevents unkillable mob blocking room-clear. — `Enemies/EliteAffix.cs:184-185`
- [ ] Reward sprite: wire `rewardSprite` in 3 scenes OR switch `ResolveRewardSprite` to `Resources.Load` (current `AssetDatabase` path is editor-only → reward vanishes in build). — `RoomClearVictoryTrigger.cs:139-153`

**Combat feel (biggest "game is dead" gap):**
- [ ] Create a `JuiceManager` prefab (HitPauseDriver + ScreenShakeDriver + DamageNumberDriver + CameraPunchController + VFXRouter); drop into `_IsoGame`, Map02, Map03. Today only `PlayableArena_Test01` has them. — `Combat/Juice/`
- [ ] Add `KnockbackReceiver` to all enemy prefabs (VoidThrall, SeamCrawler, Penitent, HalfThrall, FractureImp, ChainWarden, RelicCaster, HollowMite) — melee `GetComponent<KnockbackReceiver>()` returns null on every mob. — `BasicAttackBehaviorBase.cs:101`, `Prefabs/Enemies/`
- [ ] Add `HitFlashDriver` to all enemy prefabs (or `AddComponent` fallback in `BaseMobBehavior.Awake`). — `Combat/Juice/HitFlashDriver.cs`, `BaseMobBehavior.cs`
- [ ] Assign `slashArcVFX` on scene Player's `PlayerAttack` (currently `{fileID:0}` in all 3 scenes). — `_IsoGame.unity:22736`, `PlayerAttack.cs`

**Class-select safety:**
- [ ] Confirm live CharacterSelect: keep gated `CharacterSelectScreen.cs`; mark `CharacterSelectController.cs` `[Obsolete]` or add unlock gate — it exposes 6 broken stub classes (invisible player, disabled attack). — `CharacterSelectController.GetDefaultClasses()`

### P1 — makes it feel good
- [ ] CombatHandler on scene Player (3 scenes) + `Beat3CommitTrigger` StateMachineBehaviour on attack state → fires Sundered Beat BREAK→EXECUTE. **BLOCKED: needs an attack animation state to host the trigger; idle-only controllers have none → see risk #6.** — `CombatHandler.cs`, `Beat3CommitTrigger.cs`, `Warblade.controller`
- [ ] Replace `RiftBreak` Q-key stub (`Debug.Log` after spending 100 rage) with real AoE via `SkillRuntime.EnemiesInCircle` + `DealDamage`. — `PlayerAttack.cs:244-247`
- [ ] Remove `IsoSorter` from scene Player (3 scenes) — races `YSortBehaviour` → sort flicker; YSort is canonical. — `_IsoGame.unity:22516`, `IsoSorter.cs`
- [ ] Ronin animator controller at `Resources/Characters/Ronin/Ronin.controller` (skills already coded). — `PlayerClassManager.ApplyPrimaryClassVisual`
- [ ] At least one visible difference between the 3 maps (floor tint / obstacle layout / distinct painter layout). — Map02/03
- [ ] Wire `rewardSpawnPoint` Transform near room centre/DoorNorth in 3 scenes (on-camera drop). — `RoomClearVictoryTrigger.cs`
- [ ] `RunStats.GetBuildName/GetEquippedSkillNames` + `SkillBarUI` handle all 10 classes, not just 5 (others → null skill list). — `RunStats.cs:196-305`, `SkillBarUI.cs:271-276`
- [ ] Verify Pack sprite import = `Sprite (2D and UI)` (`bar_frame_9slice`, `bar_fill`, `lowhp_vignette`, `Pack/*.png`) — wrong import → flat-color HUD fallback. — `HUDController.cs`
- [ ] Wire `VoidThrall_DeathSplit.halfThrallPrefab` (`{fileID:0}`) so death-split fires. — `VoidThrall.prefab:178`

### P2 — deferred / nice-to-have
- [ ] `HollowMite` collider-disable + death-fade on `OnDeath()` (currently instant-disappear); route through telegraph pattern. — `HollowMite.cs`
- [ ] Remove dead `[Obsolete]` HitStop + CameraShake GameObjects from `_IsoGame`. — `_IsoGame.unity`
- [ ] Retire orphan `PlayerStats.cs` (duplicate HP system) + `Map/Runtime/RoomLoader.cs` `[Obsolete]`. — archive, never delete.

## 4. TOP RISKS
1. **PLAY AGAIN soft-lock + timeScale leak** — currently traps/freezes the player at the most-shared moment of a demo.
2. **Dual room-clear ownership** (RuntimeRoomManager `[Obsolete]`-but-live + RoomClearVictoryTrigger + MapFlowManager) — fragile; removal MUST be re-tested or the loop could stop completing.
3. **D3D12 GPU-driver crash may recur** on heavy editor sessions (today's crash was driver-level, not code). Verification work risks losing the editor session → prefer code/prefab edits + minimal play-mode; consider D3D11 editor API / GPU driver update.
4. **MainMenu + Death backdrops written but NOT play-verified** (GPU crash blocked the screenshot) — same proven mechanism as the verified Victory backdrop, but unconfirmed.
5. **CharacterSelectController exposes 6 broken stub classes** (invisible player) if it — not the gated `CharacterSelectScreen` — is the live select component. Confirm which is live.
6. **Finisher vs deferred-animations tension:** the signature Sundered Beat finisher (P1) needs an attack animation STATE to host `Beat3CommitTrigger`, but animations are owner-deferred. Either accept a minimal placeholder attack state, or defer the finisher with the rest of the animation work.

## REVIEW SYNTHESIS & LOCKED P0 (cx laurethayday + ax Gemini-3.5-Flash, 2026-06-02)
Both reviews agree the plan is sound; cx corrected several claims against code. **Locked corrections:**
- RRM-component removal is right because `DoorTrigger` only falls through to `MapFlowManager` when `RuntimeRoomManager.Instance==null` (`DoorTrigger.cs:113-122`). DungeonGraph is NOT in the post-removal loop. Do NOT raise `RoomLoader.OnRoomCleared` from RoomClearVictoryTrigger (DraftManager subscribes); RunStats advances via `MapFlowManager.IsMapTransition` (`RunStats.cs:94-100`).
- `FractureImp.prefab` ALREADY has HitFlashDriver (skip it). All 8 enemy prefabs lack KnockbackReceiver.
- 3 iso scenes already serialize `rewardSprite` (no current-build vanish); add Resources fallback for robustness + set `rewardSpawnPoint` (null in all 3).
- **PROMOTED TO P0:** `DeathScreenManager.RestartRun` same soft-lock (`:141-146`); `CharacterSelectController` is live in CharacterSelect.unity exposing 10 classes ungated → add the unlock gate.
- **Finisher:** `Warblade.controller` ALREADY has an Attack state — blocker is missing CombatHandler/Beat3CommitTrigger wiring + Beat3CommitTrigger uses `animator.GetComponent<CombatHandler>()` but CombatHandler is on root → must be `GetComponentInParent` (`Beat3CommitTrigger.cs:17`). Finisher = FIRST P1, not P0.
- **ax adds to P1:** enemy death visual (no instant-pop), skill cooldown indicators, "audio muted" notice on MainMenu. Move "10-class skill-list" P1→P2.
- **Risk:** hitstop MUST run camera/UI/input on unscaled time or it reproduces the timeScale leak; tune juice to avoid over-saturation.

### LOCKED P0 EXECUTION ORDER
1. (cx, code) Run-end resets: `DemoCompleteOverlay.Restart` → timeScale=1 + `MapFlowManager.ResetRun()` + load MainMenu; add `OnDestroy()`→timeScale=1. `DeathScreenManager.RestartRun` → timeScale=1 + ResetRun + `RunStats.StartNewRun()` + load `_IsoGame`.
2. (cx, code) `CharacterSelectController` unlock gate (Warblade/Elementalist/Shadowblade/Ranger only).
3. (cx, code) `EliteAffix.Teleport` bounds/wall check before position assign.
4. (cx, code) `RoomClearVictoryTrigger.ResolveRewardSprite` Resources fallback. (+ Beat3CommitTrigger root lookup fix — cheap, unblocks P1.)
5. (me, Unity) Remove RuntimeRoomManager COMPONENT from _IsoGame/Map02/Map03 (keep Systems GO + DungeonGraph/DraftManager). Verify north DoorTrigger/GateBehavior present.
6. (me, Unity) `rewardSpawnPoint` on-camera Transform in 3 scenes.
7. (me, Unity) JuiceManager (HitPauseDriver/ScreenShakeDriver/DamageNumberDriver/CameraPunchController/VFXRouter — VFXRouter is under Combat/, not Combat/Juice) replicated from PlayableArena_Test01 → into 3 scenes; assign `PlayerAttack.slashArcVFX` in 3 scenes.
8. (me, Unity) KnockbackReceiver on all 8 enemy prefabs; HitFlashDriver on all EXCEPT FractureImp.
9. One careful play-mode pass: clear→reward→gate→Map02/03→victory→PLAY AGAIN; death→TRY AGAIN. (GPU caution.)

## 5. NEXT STEP
Do the **P0 run-loop integrity fixes** (PLAY AGAIN soft-lock, timeScale leak, dual-system cleanup, elite wall-escape, reward sprite) **+ the JuiceManager + mob KnockbackReceiver/HitFlash wiring** — all pure code/prefab/scene edits (cx-friendly, low GPU risk), delivering the biggest "feels alive" jump. Then ONE careful play-mode pass to verify combat feel + that the loop still completes + the unverified backdrops.
