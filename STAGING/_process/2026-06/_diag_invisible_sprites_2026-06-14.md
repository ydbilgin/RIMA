# DIAG P7.5 — Invisible body sprites (Elementalist player + FractureImp enemy)

Date: 2026-06-14 · DIAGNOSE ONLY (no code change) · demo-critical

## 1. Real or capture artifact? REAL (runtime SpriteRenderer.sprite = null)

Verified via `execute_code` in Play mode (then STOPPED, console clean).

| Subject | Path tested | SR.sprite result | SR.enabled |
|---|---|---|---|
| Warblade player body (control) | controller swap + `Rebind()`+`Update(0f)` (exact `ApplyPrimaryClassVisual` path) | **`warblade_idle_south`** (NON-NULL) | true |
| Elementalist player body | same path | **NULL** | true |
| FractureImp enemy body | prefab instantiated, animator `Rebind()`+`Update()` (as EnemyAnimator drives) | **NULL** (was `fracture_imp` before the first animator Update) | true |

The SpriteRenderers are ENABLED; the `.sprite` reference itself is null at runtime → genuinely invisible, not a screenshot/render-target artifact. Shadow + hitbox come from other components, so enemies show "only shadow+hitbox".

## 2. Elementalist player — ROOT CAUSE = broken animator-clip sprite GUIDs, EXPOSED by P1/facing-batch regression. P1 REGRESSION: YES.

Mechanism:
- `PlayerClassManager.ApplyPrimaryClassVisual` loads the class AnimatorController and, when found, sets `animatorDriving = true` and SKIPS `ApplyClassIdleSprite` (introduced in commit **9557791f** "8-way facing", the facing/P1 batch — NOT 1a943a0f, which is the chest-reward commit and does not touch this file). Pre-9557791f the code ALWAYS ran `ApplyClassIdleSprite` and carried a BUG-1 comment warning the demo animator clips were empty placeholders.
- The new code trusts the controller's idle clip to drive the SpriteRenderer. For Warblade that works; for Elementalist the clip drives `m_Sprite` to a MISSING asset → null.

Asset evidence (all 8 Elementalist idle clips):
- `elementalist_idle_south.anim` PPtr curve target GUID = `927669a7ee694872b73f2cc0951d97c6` → **does not exist in project**. Actual `elementalist_idle_south.png` GUID = `11ca6b5d1c6c47abab73de76c3a79edf`. Mismatch.
- All 8 idle clips (NE/NW/SE/SW/E/N/S/W) point at stale GUIDs — every one MISSING. The Elementalist PNGs were re-imported (png dated May 31, .meta Jun 6) and got new GUIDs; the .anim clips were never re-pointed.
- Warblade contrast: all 8 idle clips' target GUIDs RESOLVE to the real warblade idle PNGs (e.g. idle_south → `54fe664d...` = the actual file). That is why Warblade renders.

So the latent broken-clip data was harmless while the old unconditional `ApplyClassIdleSprite` fallback masked it (it loads sprites directly from Resources by NAME, bypassing GUIDs). Commit 9557791f removed that mask for any class with a controller → Elementalist went invisible. = regression introduced by the P1/facing batch.

NOTE: the current in-code comment ("the controller's idle clips drive real sprite curves") is factually wrong for Elementalist — its clips DO have sprite curves but they point at missing assets.

## 3. FractureImp enemy — SEPARATE root (different broken-asset source), SAME mechanism.

PlayerClassManager does not touch enemies, confirmed separate cause.

- Prefab `Assets/Prefabs/Enemies/FractureImp.prefab`: single GameObject (Animator + SpriteRenderer same object), static sprite GUID `0671f278...` = `Assets/Art/Mobs/Act1Roster/fracture_imp.png` (VALID — that's why "before update" sprite is `fracture_imp`).
- Animator controller `Assets/Animations/Enemies/FractureImp/FractureImp.controller`; default-state idle clips (`fractureimp_idle_*.anim`) carry 6-frame sprite PPtr curves whose target GUIDs (e.g. `3db389319c6d991448a0da3e72122615`) are **MISSING**. They resolve to `ARCHIVE/Sprites_Enemies_old/Enemies/FractureImp/animations/.../frame_000.png` — i.e. sprite frames that were MOVED to `ARCHIVE/` (outside `Assets/`), so Unity dropped them. The clips were never re-pointed to the new single `fracture_imp.png`.
- `EnemyAnimator.cs` only drives Animator params — it never assigns a sprite, so nothing compensates. On the first animator Update the idle clip overwrites the valid prefab sprite with null.
- Pre-existing, NOT a 2026-06-14 regression: the FractureImp clips/archive move predate the player work (clip files dated Apr; archive set is the old roster). FractureImp likely has been showing only shadow+hitbox since the sprite frames were archived.

## 4. Recommended fix(es) — NOT implemented

Shared root in MECHANISM (animator idle clip resolves to a missing/null sprite); broken-asset SOURCE differs, so two surfaces.

A. Elementalist player (and any class whose controller fails to produce a sprite) — `PlayerClassManager.ApplyPrimaryClassVisual`:
   - After the animator setup (`Rebind()+Update(0f)`), check the body SpriteRenderer's `.sprite`. If still null, fall back to `ApplyClassIdleSprite(player, type)` (which loads by name from Resources, bypassing GUIDs). Minimal, targeted, restores pre-regression safety net without re-freezing Warblade (Warblade's SR is non-null so the fallback is skipped).
   - Change surface: `ApplyPrimaryClassVisual` (lines ~226-254). Locate the body SR (reuse the "Body"-name search from `ApplyClassIdleSprite`), read `.sprite` after the controller branch, and run the fallback when null. Fix the now-incorrect comment.
   - Deeper (post-demo): re-point the 8 Elementalist idle .anim clips to the current sprite GUIDs (`AssetDatabase`), which would make the animator drive correctly. The runtime fallback above is the demo-safe minimal fix.

B. FractureImp enemy — data fix, not a script change:
   - Re-author the FractureImp idle/walk/attack/death clips to reference an existing sprite (the single `fracture_imp.png`, or a re-imported sliced sheet), OR
   - Demo-quick: give the enemy a static-sprite guarantee. Either (i) point the controller's default state at a clip whose frames resolve, or (ii) add a tiny spawn-time guard (mirror of fix A) that, if `SR.sprite == null` after the animator runs, restores the prefab's authored sprite. Cleanest demo option = repair the clip sprite refs so the prefab's valid `fracture_imp` sprite is what shows.
   - If a generic runtime guard is wanted for ALL enemies, `EnemyAnimator.Awake/Update` could cache the prefab sprite and restore it when the animator yields null — but that is broader than this single enemy.

Recommendation for the demo: A (runtime null-fallback in PlayerClassManager) + B-repair-clips (or B static-sprite guard) — both small, both restore visible bodies.
