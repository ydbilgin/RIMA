# DONE — Readability / Contrast Pass (enemy visibility + scene contrast)

Date: 2026-06-17 · Scope: VISUAL only (shader/material/lighting + 1 visual-fallback guard). No combat/AI/gameplay logic touched.

## PROBLEM (confirmed by inspection)
- Old `RIMA/SeloutOutline` only recolors PURE-BLACK edge pixels (`isPureBlack` gate). On the colored
  enemy sprites (`fracture_imp`, `16_rift_acolyte`, `13_penitent_bruiser`, `15_hollow_hulk`) it did
  almost nothing -> dark mobs blended into the slate floor (ChatGPT #1 cross-screen flaw).
- Boss (PenitentSovereign) had NO outline at all (`Sprites-Default`).
- Latent: FractureImp/Penitent animators drive `SpriteRenderer.sprite -> null` every frame, so
  `BaseMobBehavior.EnsureVisibleSprite()` (called per-frame in Update) re-assigned `Sprites-Default`
  EVERY FRAME, clobbering any authored readability material. (Why the old selout never showed on them.)

## CHANGE 1 — New shader `Assets/Shaders/EnemyReadable.shader` (137 lines, GUID-backed .meta)
- TRUE neighbor-sampled silhouette outline: draws an outline ring on transparent texels that have an
  opaque neighbor within `_OutlineThickness` — works on ANY sprite color (not just pure-black).
- Subtle body readability lift: `_Brightness` + `_Saturation` (separates mob from slate without wash-out).
- Telegraph-COMPATIBLE: exposes `_OutlineColor` / `_OutlineAlpha` / `_OutlineThickness` so
  `EnemyOutlinePulse` pulses a RED windup ring through this same material (verified live, see below) —
  strictly better than the old whole-sprite color-tint fallback.
- Unlit transparent (samples texture directly) -> does NOT reintroduce the "URP 2D-lit material" issue.

## CHANGE 2 — Material `Assets/Materials/Enemy_Readable.mat` (GUID 62c40fd7c00e68b48be06253bee00384)
- Tuned values (chosen by play-mode iteration): RestOutlineColor warm light-gray (0.92,0.86,0.72) @ 0.85,
  Thickness 1.4 texels, Brightness 1.18, Saturation 1.12. Telegraph OutlineColor red (1,0.08,0.04), Alpha 0 at rest.
- **CYAN BUDGET PRESERVED:** outline = warm gray (NO cyan), telegraph = red, body lift = neutral. Scene cyan
  (cliff crystals / rift lights) untouched.
- Assigned to body SpriteRenderer of 4 prefabs: FractureImp / HalfThrall / Penitent / **Boss PenitentSovereign**.

## CHANGE 3 — `Assets/Scripts/Enemies/BaseMobBehavior.cs` (visual-fallback guard, ~6 lines)
- `EnsureVisibleSprite()` now only supplies the `Sprites-Default` fallback material when
  `SR.sharedMaterial == null` — never clobbers an authored unlit material. This is the minimal fix that
  lets the readability material survive on animator-nulls-sprite enemies (FractureImp/Penitent). Pure
  visual fallback path; combat/AI/sprite-restore behavior unchanged.

## CHANGE 4 — Lighting (ambient lift, persisted to `_Arena.unity`)
- `Global Light 2D` intensity 0.22 -> **0.35** (iterated 0.22/0.30/0.35; 0.30 too subtle, 0.35 = best
  floor/prop readability while staying dark-atmospheric, no wash-out). Scene saved (dirty=False).

## ITERATION + VERIFY (live, clean-session — not green-assert-only)
- BEFORE (slate floor, ambient 0.22, old selout): `Assets/Screenshots/readability_BEFORE-1.png` — FractureImp
  nearly invisible vs slate.
- AFTER outline-only (ambient still 0.22): `readability_AFTER_outline.png` / `_closeup-2.png` — bright rim, mob pops.
- AMBIENT sweep: `readability_AFTER_outline_amb030.png` (0.30) vs `_amb035.png` (0.35) -> 0.35 chosen.
- TELEGRAPH check (red windup ring via MPB through new shader): `readability_telegraph_check.png` — crisp red outline.
- DEFINITIVE AFTER (fresh play, prefab-spawned, ambient-from-disk 0.35, on-floor):
  `Assets/Screenshots/readability_AFTER_final_floor.png` — both FractureImps clearly outlined vs slate;
  player (no outline) distinct; props/floor readable; mood preserved.
- Clean-session proof: spawned `FractureImp(Clone)` keeps `mat=Enemy_Readable shader=RIMA/EnemyReadable`
  across frames (no clobber), sprite=`fracture_imp`, ambient(disk)=0.35.

## CONSOLE / STATE
- 0 Error / 0 Warning (after edits, during full play session, after exit).
- State restored: playFromStart=True, playModeStartScene=MainMenu, timeScale=1, _Arena dirty=False,
  no leftover runtime objects (rootCount=10, no __EnemyReadableHolder). Cyan budget intact. Telegraph readable.

## VERDICT: PASS
Enemy silhouettes now read against the slate floor (incl. boss + previously-broken FractureImp/Penitent);
ambient lift adds modest prop/floor readability while keeping the dark mood. Telegraph enhanced (red outline ring).

## BEFORE/AFTER for eyeball check (user verifies balance)
- BEFORE: `Assets/Screenshots/readability_BEFORE-1.png`
- AFTER (on-floor, definitive): `Assets/Screenshots/readability_AFTER_final_floor.png`
- AFTER (ambient 0.30 vs 0.35): `readability_AFTER_outline_amb030.png` / `readability_AFTER_outline_amb035.png`
- Telegraph: `Assets/Screenshots/readability_telegraph_check.png`

## NOTES / LIMITATIONS
- Old `RIMA/SeloutOutline` shader + `Enemy_SeloutOutline.mat` left in place (now unused by these 4 prefabs);
  not deleted (Karpathy #3 — surgical; other content may reference them).
- Outline ring lives inside each sprite's 64x64 quad (chibi sprites have transparent margin, so the ring
  shows). Sprites that touch the texture edge would clip the ring on that side — none of the 4 do.
- Pre-existing latent draft/Director timeScale-hold unchanged (out of scope).
