# Demo Readiness — In-Play Re-Verify (2026-06-19)

Driver: Opus sub-agent (sole Unity driver). Scene: `_Arena`. Method: real boot flow
(MainMenu Basla -> CharacterSelect class Hit -> StartButton -> opening draft -> PickCard)
+ reflection-driven combat/skill/system invocation. No scene saved. Editor left clean
(isPlaying=False, scene=_Arena, Time.timeScale=1).

Screenshots: `Assets/Screenshots/Playtest_2026-06-19/`
- `elem_riftbolt_inflight_fire.png` (bolt frozen in flight, Fire/orange)
- `enemy_burning_red_tint.png` (Burning status RED tint)
- `enemy_chill_blue_tint.png` (Chill status BLUE tint)

Note: overlay UI (skill bar, draft cards, Director panels, HP bar) does NOT appear in
scene_view screenshots by design; those items verified via execute_code data-proof.

---

## 1. FULL FLOW (Elementalist) — PASS
- scene=_Arena, Player spawned @(5.28,4.10)
- RoomRunDirector.builder = IsoRoomBuilder (WIRED, not null)
- enemies = 2 (EnemyTier count; FractureImp + reinforcement)
- Boot path required TWO Basla clicks (title InkTitleColumn/Button_Basla -> MainMenu
  MenuButtons/BaslaButton -> CharacterSelect) then class Hit + StartButton.

## 2. OPENING DRAFT — PASS
- On _Arena entry: DraftManager.IsDraftActive=True, 3 Card buttons present, draft pauses
  (DraftManager backing-field path; timeScale read 1 mid-call but draft input-gated).
- PickCard(0) -> IsDraftActive=False, Time.timeScale=1. Skill equipped
  (Elementalist_SkillController slots#4 active).

## 3. ELEMENTALIST COMBAT + NEW VFX/TINT — PASS
- RiftBolt fired via CastRhythmBehavior.OnLMBInput (LMB "Arcane Blast" / Rift Bolt).
- Bolt = runtime GameObject "RiftBolt_Runtime": scale=0.32 (empowered; base 0.28) =
  SMALL, not a huge blob (the fix). color=RGBA(1.00,0.42,0.16) Fire/orange.
  trail=TrailRenderer present (ProjectileBlaze). sortLayer=VFX. 2 child VFX nodes.
- Real damage: point-blank bolt dropped FractureImp 60 -> 38 HP.
- Burning (Fire) tint: applied Burning -> enemy SpriteRenderer color RGBA(1.00,0.65,0.59)
  = RED-shifted (TintBurning 1,0.35,0.25 lerped). Screenshot captured.
- Chill (Frost) tint: switched ActiveElement=Frost, applied Chill -> color
  RGBA(0.80,0.89,1.00) = BLUE-shifted (TintChill). Screenshot captured.
- CAVEAT (report this): live FractureImp spawns WITHOUT a StatusEffectSystem component,
  so the bolt's on-hit Burning/Chill tint path is a no-op against the demo's default
  enemy. I had to AddComponent StatusEffectSystem+StatusEffectTint manually to prove the
  tint renders. Tint SYSTEM works; tint-on-bolt-hit won't show on FractureImp as-shipped.

## 4. WARBLADE COMBAT (quick) — PASS
- Re-booted as Warblade. Profile=BasicAttackProfile_Warblade, LMB=Cleave, RMB=War Stomp,
  behaviorType=Melee (MeleeChainBehavior).
- LMB Cleave: deferred strike (attackStartup=0.08) -> HalfThrall 30 -> 2 HP (28 dmg).
- RMB War Stomp: rage-gated (rmbCost=30). Granted rage 100, fired -> Rage 100 -> 70
  (consumed 30), AoE (rmbRadius=2.2, rmbDamage=34) killed FractureImp 32 -> 0.

## 5. T9 RESTART — PASS
- In active Warblade run: DeathScreenManager(_Auto).RestartRun() invoked.
- Opening draft RE-APPEARED: scene=_Arena, IsDraftActive=True, timeScale=0, 3 cards.
- PickCard(1) -> IsDraftActive=False, Time.timeScale=1. Clean restart cycle (T9 fix holds).

## 6. T7 REWARD — PASS
- Cleared room via REAL combat path (Health.TakeDamage) across both waves
  (EncounterController activeWave=Act1_Wave_Pilot, secondWaveSpawned=True,
  encounterActive=False after kills).
- Room cleared: enemiesAlive=0, RRD.LifecycleState=Cleared, RewardPickup spawned @(1.9,6.8).
- Collect() (real collect method, not ForceCollect) -> draft opened: IsDraftActive=True,
  timeScale=0, 3 cards. PickCard(0) -> IsDraftActive=False, timeScale=1,
  RRD.LifecycleState=DoorOpen, 7 door GameObjects active.

## 7. BUILD MODE — PASS
- BuildModeController.EnterBuildMode() -> IsBuildModeActive=True, timeScale=0.
- BuildPlacementController: SelectFirstPropForValidation + PlaceForValidation(2,2)/(3,2)
  -> PlacedCountForValidation 0 -> 2.
- ExitBuildMode() -> IsBuildModeActive=False, timeScale=1, prop count PERSISTS (2/2),
  player alive (HP=100).

## 8. DIRECTOR MODE — PASS (data-proof; overlay-only visual)
- DirectorMode.SetState(Director) (State enum {Director,Test}). EnsureOverlayReadyForValidation.
- SPAWN proof: SelectFirstSpawnEnemyForValidation + SpawnSelectedEnemyAtForValidation(Vector2)
  -> DirectorSpawnedEnemyCountForValidation 0 -> 1.
- Telemetry active: TelemetryEventCountForValidation=4 (recorded prior combat damage).
- Stat tweak: SetStatForValidation("maxHP",250) returned True (slider binding matched;
  Slider_maxHP read 250 in HUD). NOTE: routes through PlayerClassManager stat layer, so
  Health.MaxHP did not instantly mutate (read 100). Director restored to Test +
  ApplyDefaultPreset/ResetStatsFromProfile before exit.

## 9. HUD — PASS
- SkillBarUI on "SkillBar": 6 slots, CD + CDTimer cooldown-countdown nodes per slot
  (12 cooldown nodes, 18 TMP texts; CDTimer text empty when skills off-cooldown = correct).
- HP bar: HPFill (Image.Filled) on HPTrack, fillAmount=1.00, active. HUDController on HUD_Canvas.

## 10. BOSS — code-confirmed (not live-reached)
- PenitentSovereign type EXISTS (no live instance; no boss-node fast-track from current room).
- Telegraph + 3-phase system present: Telegraph/1, WindupSeconds/1, Phase1Turn, Phase2Turn,
  DoPhaseTransition, DoPhase3Transition + 8 named attacks (ChainWhip, PenitentSurge,
  ShackleThrow, HolyLash, FractureStrike, ChainExplosion, SovereignsWrath, FractureCharge).

---

## CONSOLE
- 0 errors. 1 benign warning: "Some objects were not cleaned up when closing the scene"
  (standard Unity message from RestartRun scene-reload). No new regressions.

## CLEANUP
- Stopped play (runtime state discarded). Scene NOT saved. Time.timeScale=1.
- Director stats reset to default. Debug StatusEffectSystem/Tint were added to a transient
  runtime enemy that is now destroyed (no asset/scene pollution).
