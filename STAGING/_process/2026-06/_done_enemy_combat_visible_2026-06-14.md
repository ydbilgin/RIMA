# Enemy + Player body visibility in real combat — DEFINITIVE diagnosis + fix (2026-06-14)

VERDICT: DONE

## STEP 1 — Definitive diagnosis (real arena combat, Play mode, scene `_Arena`)

Scenario: `_Arena` auto-spawns a live PlayerController + live FractureImp/HalfThrall in Chase/Attack
(real combat, real EnemyAnimator/BaseMobBehavior running). Frame-by-frame via execute_code.

### FractureImp body SpriteRenderer (Chase/Attack), BEFORE fix
- `sr.sprite == null` -> **False** (sprite is NON-NULL)
- `ReferenceEquals(sr.sprite, null)` -> False; `spr.name == ''`; instanceID **negative** (-101492 / -106632 = runtime-created)
- `spr.texture != null` (48x48, RGBA32, readable); ALL 2304 pixels = solid **(217,38,38,255)** = `Color(0.85,0.15,0.15,1)` — a single distinct color
- `sr.enabled=True`, `sr.color.a=1`, sortingLayer=Entities, order=0 (so NOT alpha/enabled/sorting)
- `EnemyAnimator._fallbackSprite = 'fracture_imp'` (REAL art, instID 58724/58678, valid texture) — keeper cache is GOOD
- Animator current clip = `fractureimp_idle_south` / `fractureimp_attack_*`; ALL 32 FractureImp clips have
  `m_Sprite` keys whose value is NULL (`nullKeys == spriteKeys`, sampleSpriteName='-')

### Who writes the red square (the actual culprit)
- Manually set `sr.sprite = fracture_imp` (instID 58678) -> **overwritten back to red (-106632) within ONE frame** (matches P7 report)
- `BaseMobBehavior.LateUpdate()` calls `EnsureVisibleSprite()` EVERY frame. Its guard is
  `if (SR.sprite != null && SR.sprite.texture != null) return;`. The clip nulls the sprite, so each
  frame EnsureVisibleSprite creates/assigns a runtime **solid-red 48x48 placeholder**
  (`new Color(0.85f,0.15f,0.15f,1f)`, `BaseMobBehavior.cs:124-136`) and stores it in its own
  `fallbackSprite` (instID -106632 == the live `sr.sprite`).
- `EnemyAnimator.LateUpdate` (same GameObject, BOTH default execution order 0 -> UNDEFINED order)
  ran with the OLD guard `if (sr.sprite == null)`. By the time it ran, BaseMobBehavior had already
  filled red (non-null) -> the `sprite==null` guard NEVER fired -> `fracture_imp` was never restored.

### TRUE ROOT (real combat)
Enemy clips null the body sprite -> `BaseMobBehavior.LateUpdate` overwrites with a non-null,
textured, solid-red 48x48 placeholder -> the enemy body renders as a RED SQUARE (the real
`fracture_imp` art never shows). The body is not literally "invisible" — it is the wrong (red
placeholder) sprite. Not alpha/enabled/sorting; not textureless. It is a competing-LateUpdate race
between two scripts on the same GameObject, won by BaseMobBehavior's placeholder.

### Why the two prior agents contradicted
- P7.5 measured EnemyAnimator's keeper in isolation: animator nulls -> keeper restores fracture_imp.
  Correct ONLY when BaseMobBehavior.LateUpdate isn't racing/winning. Their `afterAnimator=null ->
  afterLateUpdate=fracture_imp` proof omitted BaseMobBehavior's per-frame red write.
- P7 measured REAL combat where BaseMobBehavior.LateUpdate fills red BEFORE EnemyAnimator's null-guard
  runs, so the keeper never fires and a manually-set sprite is overwritten within one frame.
  Both observations are individually correct; the conflict was undefined same-GameObject LateUpdate
  order + a guard (`sprite==null`) blind to the non-null red placeholder.

### PLAYER
- Elementalist controller has ONLY 8 idle clips, ALL with a single NULL `m_Sprite` key (no walk clips;
  idle is used in motion too). No BaseMobBehavior competes on the player.
- Live Elementalist body (`Body` GameObject, Animator beside it): animator writes genuine NULL ->
  PlayerAnimator keeper restores `elementalist_idle_south` -> VISIBLE. (A transient null reading was
  only because the long-running session's player had DIED — death guard correctly skips restore.)
- So the player's textureless risk is the same failure CLASS; broadened the guard defensively.

## STEP 2 — Fix (surgical)

`Assets/Scripts/Enemies/EnemyAnimator.cs` (128 lines; +19/-4)
1. Added `[DefaultExecutionOrder(100)]` to the class so EnemyAnimator.LateUpdate runs AFTER
   BaseMobBehavior.LateUpdate (order 0) -> EnemyAnimator is the LAST sprite writer (zero flicker),
   independent of Unity's undefined same-GameObject ordering. (Verified present at runtime: order=100.)
2. Broadened the keeper guard:
   `if (_isDead || sr == null || _fallbackSprite == null) return;`
   `if (sr.sprite == null || string.IsNullOrEmpty(sr.sprite.name)) sr.sprite = _fallbackSprite;`
   The red placeholder is a runtime `Sprite.Create` with an EMPTY name; the authored prefab sprite
   has a real name. So this restores over BOTH null AND the nameless red placeholder, while leaving
   asset-backed animation frames (working enemies, real sprite names) untouched -> no regression.
   (NOT `sprite==null || texture==null`: the red placeholder HAS a texture, so that would miss it.)

`Assets/Scripts/Player/PlayerAnimator.cs` (273 lines; +5/-1)
- Broadened keeper from `sr.sprite == null` to `sr.sprite == null || sr.sprite.texture == null`
  (death guard + lazy-Health resolution unchanged). Covers a non-null-but-textureless write; the
  player has no placeholder competitor so the simpler texture-null broadening (per spec) suffices.

Penitent: has EnemyAnimator + an assigned prefab sprite (m_WasSpriteAssigned=1) + same null-keyed
clip pattern -> fixed by the SAME EnemyAnimator code path (no separate change). Verified generically:
HalfThrall restored its own `16_rift_acolyte`, FractureImp restored `fracture_imp`.

## STEP 3 — Verify (real combat, mandatory)

console: 0 errors, 0 warnings (after recompile and in steady-state combat; cleared one pre-existing
tooling-artifact line `FindAllObjectsOfType: Invalid Type` from a failed diagnostic reflection call,
re-checked clean).

Sustained 40 CONTIGUOUS frames, real combat:
- FractureImp body (Attack, frames 84994-85033): `fracture_imp` instID 58678, sprite!=null &&
  texture!=null && enabled -> impVIS = **40/40**
- Player body: VIS -> **40/40**

Elementalist-in-motion (forced moveInput, cycling 5 directions, imp transitioning Attack<->Chase,
frames 130569-130608):
- Elementalist body `elementalist_idle_south` VIS -> **ElementalistMovingVIS = 40/40**
- FractureImp `fracture_imp` VIS -> **impVIS = 40/40**
- Zero VIS=False frames in either run.

Screenshot (scene_view, combat moment, both bodies showing REAL art, no red square, not invisible):
`F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_process/2026-06/enemy_combat_visible_2026-06-14.png`

Cleanup: debug side-effects reverted (PlayerClassManager.SelectedClass restored to Warblade;
clipboard transport cleared). Play STOPPED. `_Arena.unity` NOT saved/modified (git shows only the two
.cs files changed). Editor not compiling.

## Deviations / notes
- Used `[DefaultExecutionOrder]` attribute (inside EnemyAnimator.cs) rather than Project Settings to
  keep the ordering fix within the in-scope file. Surgical, no other files touched.
- BaseMobBehavior.cs is the source of the red placeholder but is OUT OF SCOPE; the fix makes
  EnemyAnimator authoritative without editing it. (Post-demo: re-import the archived enemy animation
  frames / re-point the null-keyed clips and both keepers become inert.)
