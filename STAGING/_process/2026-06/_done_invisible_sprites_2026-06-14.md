# DONE P7.5 — Invisible body sprites fix (Elementalist player + enemies)

Date: 2026-06-14 · VERDICT: DONE (both fixes verified via data-proof in Play mode) · console: 0 errors · play stopped, scene clean (not saved)

## FIX A — Elementalist player body (PlayerClassManager.cs)

File: `Assets/Scripts/Systems/PlayerClassManager.cs`
- `ApplyPrimaryClassVisual`: after the animator Rebind+Update in the `animatorDriving` path, now reads the body SpriteRenderer; if its `.sprite` is STILL null, falls back to `ApplyClassIdleSprite(player, type)` (loads class idle sprite by name from Resources, bypassing the broken clip GUIDs).
  - New condition: `if (!animatorDriving || FindBodySprite(player) == null) ApplyClassIdleSprite(player, type);`
  - Added small private helper `FindBodySprite(GameObject)` (reuses the same "Body"-name SR search as `ApplyClassIdleSprite`).
  - Fixed the now-inaccurate comment (it claimed all controllers' idle clips drive real sprite curves; corrected to note some classes' clips point at missing/stale GUIDs).
- Warblade unaffected: its animator drives SR.sprite to a valid sprite → `FindBodySprite != null` → fallback skipped → still animator-driven, no regression.

Data-proof (Play mode, exact SetPrimaryClass → ApplyPrimaryClassVisual path):
- Warblade (control) body = `warblade_idle_south` (NON-NULL, animator-driven — no regression)
- Elementalist body = `elementalist_idle_south` (NON-NULL — was NULL before; fallback restored it)

## FIX B — enemy bodies (EnemyAnimator.cs) — general guard, scope = 2 enemy types

Scoping scan (editor-side, all 16 enemy prefabs under Assets/Prefabs/Enemies): for each prefab's animator controller, counted SpriteRenderer object-reference curve keys that resolve to null (missing/archived sprite assets):
- **FractureImp** | ctrl=FractureImp | static prefab sprite=`fracture_imp` (VALID, Assets/Art/Mobs/Act1Roster/fracture_imp.png) | **BROKEN 192/192 null sprite keys**
- **Penitent** | ctrl=Penitent | static prefab sprite=`13_penitent_bruiser` (VALID) | **BROKEN 192/192 null sprite keys**
- HalfThrall | ctrl has NO sprite curves → animator can't null the static sprite → NOT affected
- ChainWarden, HollowHulk_GB, RelicCaster, ShardWalker_GB, VoidThrall → Animator present but NO runtime controller → animator inert → static sprite shows → NOT affected
- Others (Projectile, bosses, elites, etc.) → no animator-driven body in this mechanism

So MANY-but-bounded: TWO enemy types (FractureImp + Penitent), SAME mechanism (idle clips reference sprite frames moved to ARCHIVE/ outside Assets/), BOTH have a valid authored static prefab sprite. Both use `EnemyAnimator`; SR is on the same GameObject as the Animator on both.

Chosen fix = general runtime sprite-keeper in the shared `EnemyAnimator` (least-hacky for the real scope; self-heals any future enemy with the same defect; no per-prefab re-authoring of 192 broken keys × 2; no faking missing art since both enemies have real sprites):
File: `Assets/Scripts/Enemies/EnemyAnimator.cs`
- `Awake`: cache `_fallbackSprite = sr.sprite` (the authored prefab sprite).
- New `LateUpdate`: if not dead and `sr.sprite == null && _fallbackSprite != null`, restore `sr.sprite = _fallbackSprite`.
- LateUpdate is the correct deterministic hook: verified player-loop order is `ScriptRunBehaviourUpdate(74) → DirectorUpdateAnimationBegin/End(80/82) [animator writes sprite=null] → ScriptRunBehaviourLateUpdate(91) [our restore]`. So the restore runs AFTER the animator each frame and persists into render + next-frame reads.
  - (Briefly tried OnWillRenderObject thinking LateUpdate lost the race — player-loop dump proved LateUpdate runs after the animator, so reverted to LateUpdate. The earlier "still null" reads were execute_code sampling between phases 82 and 91, not a logic failure.)

Data-proof (Play mode, deterministic frame-order replication: `anim.Update(dt)` = animator phase, then `EnemyAnimator.LateUpdate` = script-late phase):
```
FractureImp  start=fracture_imp        frame0/1/2: afterAnimator=<NULL> | afterLateUpdate=fracture_imp
Penitent     start=13_penitent_bruiser frame0/1/2: afterAnimator=<NULL> | afterLateUpdate=13_penitent_bruiser
```
Every frame the animator nulls the sprite (bug confirmed) and LateUpdate restores it (fix confirmed, wins + persists). Enemy is VISIBLE (static authored sprite). Full idle animation = OUT OF SCOPE (needs the archived frames re-imported and clips re-pointed; post-demo).

## BLOCKED-needs-art enemies
NONE. The two affected enemies both have a valid in-Assets static sprite; no faking required.

## Verification summary
- Compile: 0 errors (confirmed reflection-presence of new members + read_console error filter empty).
- Console during Play: 0 errors.
- Play stopped; active scene `_Arena` not dirty (not saved); no leftover test objects.

## Files written
- `Assets/Scripts/Systems/PlayerClassManager.cs` (Fix A: ~12 lines added/changed incl. FindBodySprite helper + comment)
- `Assets/Scripts/Enemies/EnemyAnimator.cs` (Fix B: ~12 lines added — `_fallbackSprite` field, Awake cache, LateUpdate restore + comments)

## Notes / post-demo follow-ups
- Elementalist: deeper fix = re-point the 8 idle .anim clips to the current sprite GUIDs (`AssetDatabase`) so the animator drives correctly; runtime fallback is the demo-safe minimal fix.
- FractureImp + Penitent: re-import the archived idle/walk/death frames and re-point their clips to restore true animation; the EnemyAnimator keeper restores visibility (static) until then.
