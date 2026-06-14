# DONE P7.5 fixup — Elementalist player persistent sprite-keeper (FIX A insufficient → keeper added)

Date: 2026-06-14 · VERDICT: DONE · console: 0 errors · play stopped, `_Arena` clean (not saved) · throwaway test script deleted

## Problem (from 4-advisor gate)
FIX A (PlayerClassManager one-time fallback) only set the Body sprite at frame 0. The Elementalist
player animator (default state idle_S, WriteDefaultValues=1, idle clip m_Sprite PPtr → missing GUID
927669a7) writes the Body SpriteRenderer.sprite to NULL EVERY frame, overwriting the one-time
fallback next frame → Elementalist flashes visible 1 frame then invisible. FIX B (EnemyAnimator
LateUpdate keeper) is sound — left untouched.

## Fix (auditor-recommended) — give the PLAYER the same persistent per-frame guard

### PlayerAnimator.cs (Assets/Scripts/Player/PlayerAnimator.cs)
- New field `private Sprite fallbackSprite;` + `private Health health;` cached in Awake (`GetComponent<Health>()`).
- New `public void SetFallbackSprite(Sprite)` — PlayerClassManager publishes the chosen class idle sprite here.
- New `LateUpdate()`: runs after the animator's per-frame sprite write; if `sr.sprite == null` and a
  fallback exists, restores it. DEATH GUARD: skipped when `health.IsDead` (mirrors EnemyAnimator._isDead)
  so a sprite-clearing death animation isn't fought (no revival bug).
- LAZY HEALTH resolution: PlayerAnimator.Awake runs during prefab Instantiate, BEFORE
  RoomRunDirector.EnsurePlayerRuntime / ChamberSelectBootstrap add Health (`AddIfMissing<Health>` runs
  post-instantiate — verified RoomRunDirector.cs:797). So Awake caches health=null in the real flow;
  LateUpdate re-resolves `health = GetComponent<Health>()` until present → death guard actually effective.

### PlayerClassManager.cs (Assets/Scripts/Systems/PlayerClassManager.cs)
- FIX A kept as the INITIAL sprite set + fallback SOURCE (necessary, not sufficient alone).
- `ApplyClassIdleSprite` now publishes the loaded idle sprite to the keeper:
  `player.GetComponentInChildren<PlayerAnimator>(true)?.SetFallbackSprite(idle);` (least-coupled clean wiring).
- Auditor MINOR: extracted the duplicated "Body"-name SR search loop into one shared helper
  `FindBodyRenderer(GameObject)`; both `FindBodySprite` and `ApplyClassIdleSprite` now use it.
- Important consequence: for Warblade, `FindBodySprite != null` so `ApplyClassIdleSprite` is SKIPPED →
  `SetFallbackSprite` is NEVER called → keeper `fallbackSprite` stays null → LateUpdate is a guaranteed
  no-op for Warblade (cannot freeze/override its animation).

### EnemyAnimator.cs — NOT touched (FIX B sound).

## Verification (data-proof, Play mode, MULTI-FRAME)
Compile: 0 errors (isCompiling=READY; reflection confirms SetFallbackSprite + fallbackSprite + health +
LateUpdate members; read_console error filter empty). The one "Cannot access a disposed object" line is
a UnityMCP client-handler artifact, not a compiler error (no CSxxxx).

Test harness: prefab instantiated in Play (mirroring real flow), Elementalist applied via real
SetPrimaryClass→ApplyPrimaryClassVisual path, throwaway `_TestSpriteKeeperSampler` recorded the Body
SR.sprite each real LateUpdate. Script deleted + recompiled clean after.

- **Per-frame mechanism (deterministic, anim.Update then keeper):** every frame afterAnimator=`<NULL>`
  (bug confirmed each frame), afterLateUpdate=`elementalist_idle_south` (keeper wins + persists).
- **Elementalist multi-frame (real frames, movement-driven NE):** 90/90 LateUpdate frames non-null,
  everNull=False, real frame span 7772→7861, sprite=`elementalist_idle_south` throughout.
- **Elementalist multi-frame (realistic ordering, Health added AFTER Awake, SW movement):** Awake cached
  health=null (real-flow risk reproduced); 70/70 frames non-null over span 9674→9743; lazy resolution
  then populated health.
- **Warblade no-regression:** start `warblade_idle_south`, fallbackPublished=`<NULL/none>` (keeper not
  armed → no-op); 90/90 frames non-null; animator drives 4 DISTINCT directional sprites (S/E/N/W) with
  late==anim each → keeper never alters animator output, no freeze.
- **Death guard (end-to-end, real frames):** alive→keeper restores; after Health.TakeDamage(999999)
  (IsDead=true), 40/40 death-phase frames stayed `<NULL>` (0 revived) → keeper does NOT fight the death
  state. No revival bug, no flicker.

Play stopped; `_Arena` dirty=False (not saved); all test GameObjects + throwaway script removed.

## Files written
- `Assets/Scripts/Player/PlayerAnimator.cs` (~28 lines added: fallbackSprite + health fields, Awake
  health cache, LateUpdate keeper w/ lazy-health + death guard, SetFallbackSprite + comments)
- `Assets/Scripts/Systems/PlayerClassManager.cs` (~10 lines net: FindBodyRenderer shared helper,
  SetFallbackSprite publish in ApplyClassIdleSprite)

## Out of scope (documented only — post-demo follow-up)
The PROPER root fix = repair the Elementalist idle clip sprite GUIDs (927669a7 → the existing
elementalist_idle_*.png GUIDs) for full 8-way animation. The runtime keeper makes the class VISIBLE
(static idle pose) meanwhile and no-ops once the clips are repaired. NOT attempted here.
