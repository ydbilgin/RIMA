# DONE — CombatJuice + Enemy_SeloutOutline (beautification #1-2)

Date: 2026-06-17 · Scope: 2 surgical asset/scene changes. NO controller/AI/combat/placement logic touched.

## DEĞİŞİKLİK 1 — CombatJuice.prefab → _Arena.unity
- Instantiated `Assets/Prefabs/Combat/CombatJuice.prefab` (GUID `2b4f2f85d031f4e429fe752646926eb7`)
  as a PrefabInstance at root of `Assets/Scenes/_Arena.unity`; **scene saved** (12 prefab-GUID refs persisted, named "CombatJuice").
- Component count VERIFIED = 5 MonoBehaviours (runtime enumerated): HitPauseDriver, ScreenShakeDriver,
  CameraPunchController, VFXRouter, DamageNumberDriver. HitFlashDriver / ImpactFrameDriver NOT added (correct).
- Play-test juice observation (event-bus published HitEvent/KillEvent = exact combat path):
  - HIT → DamageNumberDriver spawned damage numbers (delta +2 active children per hit). ✓
  - KILL → HitPause freeze engaged: timeScale 1→0, auto-restored →1 after WaitForSecondsRealtime. ✓
  - CameraPunchController.Instance + ScreenShakeDriver.Instance alive & subscribed. ✓
  - Unscaled-time immunity CONFIRMED: damage numbers spawned even while timeScale=0 (draft pause). ✓

## DEĞİŞİKLİK 2 — Enemy_SeloutOutline.mat → 3 wave enemies
- Created `Assets/Materials/Enemy_SeloutOutline.mat` (GUID `5733b44492dcc4149b7da1cab37ffaf7`)
  from shader `RIMA/SeloutOutline`, `_OutlineStrength=0.55` (on-disk verified).
- Assigned to SpriteRenderer.m_Materials of: FractureImp / HalfThrall / Penitent prefabs (was Sprites-Default).
  DoorTrigger skipped (runtime procedural sprite). Runtime-confirmed: spawned HalfThrall(Clone) reports
  mat=Enemy_SeloutOutline shader=RIMA/SeloutOutline.
- Cyan-budget: shader darkens pure-black edge pixels toward the sampled NON-black neighbor color (ember-neutral);
  injects NO cyan → ≤15% budget preserved by construction.
- Before/after silhouette: `Assets/Screenshots/selout_silhouette.png` — live HalfThrall readable vs floor.
  Note: a runtime red damage/aura vignette tinted the slate reddish in-capture (post-FX state, not the material).

## F2 / Director SEAM TEST (timeScale leak gate)
- After Build Mode (F2) Enter→Exit: re-published HIT → damage numbers STILL fire (delta +2). ✓
- After DirectorMode ToggleState cycle: re-published HIT → damage numbers STILL fire (delta +1). ✓
- timeScale-leak attribution (definitive isolation): with NO overlay open, clean baseline ts=1 → crit HIT freeze
  (ts→0) → after freeze window ts restored to **exactly 1**. CombatJuice = ZERO leak. ✓
- Latent ts=0 states encountered are held by the opening DRAFT (UIManager.skillOfferOpen→ApplyTimeScale) and
  DirectorMode (ResolveTimeScaleForState) — PRE-EXISTING systems, NOT CombatJuice. CombatJuice HitPause
  captures previousTimeScale and restores it faithfully (does not worsen the latent bug, per spec).

## CONSOLE
- 0 Error / 0 Warning throughout (after static edits, during full play session, after exit). ✓

## EDITOR STATE (no-debug-state-leak)
- Temporarily disabled `RIMA_PlayFromStartScene` pref + cleared `playModeStartScene` to play _Arena directly;
  BOTH RESTORED (pref=True, playModeStartScene=MainMenu, timeScale=1). Play-mode runtime pollution discarded on Stop.
- _Arena saved & not dirty (rootCount=9 = 8 original + CombatJuice).

## VERDICT: PASS
Both changes landed, verified live (not green-assert-only). Juice fires + kill-freeze + camera/shake wired;
3 enemies carry SeloutOutline; F2/Director seams keep juice firing with zero CombatJuice timeScale leak.

LIMITATION: SeloutOutline shader only recolors pure-black sprite pixels — for the colorful `16_rift_acolyte`
sprite the effect is a subtle dark-edge treatment, not a bright halo (matches "subtle, ember-neutral" intent).
Pre-existing latent draft/Director timeScale-hold remains (out of scope; flagged, not fixed).
