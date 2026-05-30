# RIMA — 24-48h AUTONOMOUS WORK ORDER (S6) — sequential, check-as-you-go

> **Post-/clear pickup: read `.claude/PROJECT_RULES.md` + `CURRENT_STATUS.md`, then THIS file, then execute top-to-bottom.**
> Mode: **full autonomous** — Opus decides+writes, cx+agy advise/review (writer≠reviewer), every code step `dotnet build
> RIMA.Runtime.csproj` green (~2s), don't ask unless a real conflict. Audio DEFERRED. Animate step USER-GATED.
> **Canon/decisions (LOCKED, don't re-litigate):** `RIMA_DIRECTION_LOCK_S6.md` · `RIMA_ROADMAP_AND_CLEAN_STRUCTURE_S6.md`
> · `CONTROL_SCHEME_SYNTHESIS_S6.md` · `VFX_STRATEGY_SYNTHESIS_S6.md` · live-path map in `CODEX_DONE.md` (tail).
> **Build-verify rule:** after deleting/adding a .cs, also sync `RIMA.Runtime.csproj` (`<Compile Include>` lines) — Unity
> may be closed so the csproj won't auto-regen; stale ref = CS2001.

## LIVE SPINE (the ONLY demo path — confirmed by cx GUID trace)
`PlayableArena_Test01.unity → Systems.Map.RoomLoader → Phase1_Room5_BossArena → PenitentSovereign.prefab →
PenitentSovereign.cs` · MapFragment = `RIMA.Environment.MapFragment` · boss-death = direct `Health.OnDeath →
RoomLoader.WireBossDeathListener → RaiseDemoComplete → DemoCompleteOverlay.Show()` · camera = `Camera/CameraFollow.cs`.
**Dormant duplicates (referenced in ~10 files → [Obsolete], do NOT mass-delete):** RuntimeRoomManager,
BossAI_PenitentSovereign, Core/MapFragment, Player/CameraFollow, static Map/Runtime/RoomLoader, VFX/ScreenShake(+Core/CameraShake).

---
## ✅ DONE THIS SESSION (don't redo)
Direction+roadmap+control+VFX+style-upscale synthesis docs · 24 Python placeholders · **5/7 hero imagegen** (menu/victory/
logo/next-class/boss, cx, QC-pass) · NLM fixed + `nlm_relogin.ps1` · cx live-path map · **SkillDraftSystem.cs deleted**
(csproj synced, build GREEN) · SkillData kept (workflow false-positive). Tasks: #7 imagegen, #10 consolidation in flight.

---
## BLOCK A — Phase 0 CONSOLIDATE (CODE, autonomous) ~6-8h
> ✅ **BLOCK A DONE (2026-05-30, Opus autonomous, build GREEN throughout).** A1-A6 all landed.
> cx reviewed A3 (found 3 real issues → all fixed in v2: combat drop-at-player, event-leak teardown
> unsubscribe, reachability). agy second-review A3v2+A6 in flight. **CHECK A item-2 finding (live→dormant
> coupling map, pre-existing debt, migrate in BLOCK B + deferred RRM refactor):**
> ScreenShake ← live boss ×6 (→B1) · CameraShake ← live CameraFollow + 3 (→B1) · RuntimeRoomManager ←
> live DraftManager+boss+DoorTrigger+RewardPickup+RunStats+SubRoom (→deferred) · dormant CameraFollow ← SubRoom.
> Truly-dormant (0 live ref): BossAI_PenitentSovereign, static Map/Runtime/RoomLoader, Core/MapFragment.

Each step: Opus write → dotnet build green → cx or agy review → next.
- **A1** `PenitentSovereign.cs` phase threshold `0.33f → 0.5f` (canon). [LIVE boss] ~10m
- **A2** Mark dormant duplicates `[System.Obsolete("Not the live spine — see WORK_ORDER")]`: RuntimeRoomManager,
  BossAI_PenitentSovereign, Core/MapFragment (RIMA.MapFragment), Player/CameraFollow (RIMA.CameraFollow),
  Map/Runtime/RoomLoader, VFX/ScreenShake, Core/CameraShake. (Annotation only — no deletion; compile stays green.) ~45m
- **A3** B3 fragment-in-combat-room: `RoomLoader.cs:285` + `MapFragmentSpawner.cs:25` gate on `isRewardRoom` → spawn
  fragment on COMBAT room clear too; wire pickup → draft → `Gate.Unlock()`. ~1.5h. cx review (softlock risk).
- **A4** B4 MapFragment consolidation (code half): ensure ALL live refs use `Environment.MapFragment`; the two prefabs
  carry the Core script GUID → note for SCENE block (G) to reassign. Don't delete Core class yet (tests ref it). ~1h
- **A5** B7 test scene name: `BootstrapContract.GameSceneName "_IsoGame" → "PlayableArena_Test01"`; for `RoomFlowTests`/
  `PlaytestScenarios` (written for _IsoGame) → `[Ignore("legacy _IsoGame scene retired")]` rather than blind repoint. ~45m
- **A6** Beat3 finisher: `CombatHandler.OnCommitBeat` → add `CombatEventBus.PublishCommitBeat(...)` so finisher
  hitstop+shake fire (`Beat3CommitTrigger.cs:26`). ~30m
- **CHECK A:** dotnet build green + grep no live ref to a dormant duplicate + agy review of A3/A6.

## BLOCK B — Phase 1 COMBAT-FEEL code (autonomous; FEEL-tune = user F5) ~4-6h
- **B1** Camera shake unify: make the LIVE `Camera/CameraFollow.cs` read `ScreenShakeDriver.Instance.CurrentOffset`
  (confirm it does); ensure `VFX/ScreenShake` (rotates camera!) is NOT on the live camera (it isn't per audit) — leave
  [Obsolete]. ~1h
- **B2** `VFXRouter.entries` wire placeholder hit/kill/dash prefabs (hitspark `11127e69` is ready — wire as `hit_default`
  via a tiny `SpriteFlipbookVFX` frame-driver; NET-NEW script, additive). ~2h. cx review.
- **B3** Juice tier numbers to canon (hitstop 0.04/0.07/0.12/0.18) in HitPauseDriver/ScreenShakeDriver serialized
  defaults. ~30m. (Real feel = user F5 — GATED.)
- **CHECK B:** dotnet build green. **F5 FEEL GATE = USER** (combat feel lock before art).

## BLOCK C — Phase 2 CONTROLS/HUD code (autonomous) ~6-8h  (= task #8)
Per `CONTROL_SCHEME_SYNTHESIS_S6.md`. **Opus writes (multi-system) → cx+agy review → dotnet build.**
- **C1** `KeyBindManager` → binding registry (MoveX/Y, Dash, Attack, ClassSecondary, RiftBreak, Skill1-4) + PlayerPrefs
  JSON persistence + duplicate/reserved guard + `OnBindingsChanged` + **`RebuildBindings()`** (currently SetKey writes
  prefs but never rebuilds). ~3h
- **C2** Repoint hardcoded `new InputAction` in PlayerController/PlayerAttack/Warblade_SkillController to the registry. ~2h
- **C3** Bug-1: `SkillBarUI.SlotKeys` → binding-driven labels (Q/E/R/F not 1-5); SlotCount 7→6. ~45m
- **C4** Bug-2: unify settings — `SettingsMenuUI` canonical, wire aim toggle to `PlayerController.AttackAimMode` + add
  DashMode + Controls section; `[Obsolete]`/disable `Core/SettingsMenu` (dup ESC + timeScale). ~1.5h
- **C5** Interact key: route the 4 hardcoded `Key.G` (MapFragment/DoorTrigger/RewardPickup/IntraEncounterDoorTrigger) +
  HUD prompt through one rebindable Interact action. ~1h
- **CHECK C:** dotnet build green + cx review (input ripple) + agy review. F5 verify rebind works (GATED).

## BLOCK D — Phase 3 CONVERSION code (autonomous) ~3-4h
- **D1** Victory `DemoCompleteOverlay.cs:37` timeScale `0.2 → 0`. ~10m
- **D2** Next-class teaser: wire `next_class_silhouette.png` instead of hardcoded string. ~45m
- **D3** T4 PlayMode test: boss kill → `DemoCompleteOverlay.Show()` → CTA active (the win-condition test). ~1.5h
- **D4** ⛔ Steam URL `/app/0/` (DeathScreenManager + DemoCompleteOverlay + RunStats) — **BLOCKED: need real Steam App ID
  from user.** Leave a clear TODO + the exact 3 lines.
- **CHECK D:** dotnet build green + T4 passes (if Unity test-runner reachable).

## BLOCK E — ART production (semi-autonomous) ~ongoing
- **E1** imagegen Groups B/C via cx (`IMAGEGEN_ASSET_PACK_PLAN_S6.md`): Group B ~12 UI frames/glyphs (Path B chroma-
  bbox), Group C ~20-30 skill icons (64px). cx produces → Opus+agy QC → regen fails. (death/lowhp/particles stay Python.)
- **E2** ⛔ GATED (user): PixelLab Style-Reference refine of the gpt-image-2 hero set to pixel-perfect (per
  `STYLE_PRESERVING_UPSCALE_ANALYSIS_S6.md`) · slash-arc painterly flipbook · boss sprite + 8-dir · **any animate step.**

## BLOCK G — SCENE wiring (Unity-GATED: needs Unity Editor / UnityMCP stable / user) — batch
B1 DraftManager.offerUI+offerGenerator refs · B2 MapFragmentSpawner.fragmentPrefab (assign Environment MapFragment) ·
reassign the 2 MapFragment prefabs to Environment script · B10 delete inactive Systems GO · Player.prefab layer 0→10 +
IgnoreLayerCollision(10,11) · add MapProgressController + RoomTransitionFX GOs · skill icons → SkillIconRegistry.asset ·
remove stale Gate_Room0_Exit. **Do when Unity MCP is stable or hand to user; restart Unity first.**

## BLOCK H — Phase 6 READABILITY/POLISH (after demo plays) 
Color budget (cyan ONLY player/rift/telegraph; enemies+bg muted) · juice scaling (big shake/hitstop on finishers only) ·
final E2E + F5 QC (USER gate).

## DEFERRED (not in 24-48h): audio (BLOCK F), full RRM deletion refactor, full 3-phase boss, other classes.

---
## EXECUTION CADENCE
Work BLOCK A→B→C→D autonomously (code; cx/agy review; dotnet verify each), interleave E1 imagegen when cx free, batch
G for the user's next Unity availability, H last. After each BLOCK: dotnet green + short progress note. **Conflict →
get cx+ax input, Opus decides, ask user only if still unresolved.** Update CURRENT_STATUS + this file's checkboxes as you go.
