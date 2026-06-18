# DONE — Elementalist 8-direction sprite wiring (#9b)

Date: 2026-06-18
Scope: make Elementalist resolve its REAL 8-dir idle sprites instead of the single fallback idle. Surgical, reversible, no code change.

## Root cause (confirmed)
The Elementalist idle sprites were ALREADY imported and valid. The break was purely in the **AnimationClip → sprite GUID** references.

- Live controller (what `PlayerClassManager.ApplyPrimaryClassVisual` loads): `Resources.Load("Characters/Elementalist/Elementalist")` = `Assets/Resources/Characters/Elementalist/Elementalist.controller` (GUID `13fd0805...`).
- That controller references 8 idle clips in `Assets/Animations/Characters/Elementalist/elementalist_idle_*.anim`.
- ALL 8 of those clips referenced **stale, dropped sprite GUIDs** (idle_south = `927669a7...`, the GUID cx flagged; the other 7 were also dropped — verified each resolves to NO `.meta` in `Assets/`).
- Result: animator wrote a null sprite every frame → `PlayerAnimator.LateUpdate` restored only the single cached `idle_south` fallback (via `PlayerClassManager.ApplyClassIdleSprite`). Player looked correct only facing south; all other facings fell back.

## Warblade pattern found (the working reference)
- Sprites: `Assets/Resources/Characters/Warblade/warblade_idle_{south,east,north,west,SE,SW,NE,NW}.png` (cardinals lowercase, diagonals UPPERCASE in filename).
- Import settings (idle_south.meta): `spriteMode 1` (single), `spritePixelsToUnits 64`, `filterMode 0` (Point), `spritePivot {0.5, 0.25}`, `alignment 9` (custom), `alphaIsTransparency 1`, `textureType 8` (Sprite). 120x120 RGBA.
- Controller `Resources/Characters/Warblade/Warblade.controller` = exactly **8 idle clips**, one per direction; NO run/cast clips wired in (movement = `anim.speed` + DirX/DirY params, run/cast .anim files exist on disk but are not in this controller — same for Elementalist).
- Each Warblade idle clip's `m_Sprite` PPtr GUID == the matching PNG's GUID (e.g. idle_south clip GUID `54fe664d...` == warblade_idle_south.png GUID). 1:1 by direction.
- `PlayerAnimator.Awake` keeps `sr.flipX = false` permanently and documents the "8-full-sprite scheme" (a separate weapon hand-anchor reads facing; mirroring would desync it).

## 5+flipX vs 8 decision: **8 full sprites** (matches Warblade)
Warblade uses 8 distinct imported sprites with NO flipX mirroring. Task mandates consistency over cleverness, so Elementalist uses the same 8-sprite scheme. The 8 real Elementalist sprites already exist; no mirroring logic added.

## Import: NONE NEEDED
The STAGING source PNGs (`STAGING/_process/2026-06/elementalist_8dir/elementalist_*.png`) are **byte-identical (SHA1)** to the already-imported `Assets/Resources/Characters/Elementalist/elementalist_idle_*.png`, which already carry Warblade-identical import settings (PPU 64, Point, pivot 0.5/0.25, single sprite, 120x120 RGBA). So this was purely a WIRING fix — no copy, no meta change, no reimport of sprites.

## What was wired
Re-pointed all 8 Elementalist idle clips (`Assets/Animations/Characters/Elementalist/elementalist_idle_*.anim`) from their stale GUIDs to the matching real sprite, 1:1 by direction:
- idle_east  -> elementalist_idle_east  (`a5cd4502...`)
- idle_north -> elementalist_idle_north (`b6669f54...`)
- idle_south -> elementalist_idle_south (`11ca6b5d...`)
- idle_west  -> elementalist_idle_west  (`aaefa00a...`)
- idle_NE -> elementalist_idle_NE (`c4e4201e...`)
- idle_NW -> elementalist_idle_NW (`449d0d95...`)
- idle_SE -> elementalist_idle_SE (`75e6b8ed...`)
- idle_SW -> elementalist_idle_SW (`de64764c...`)

Method note: a raw text GUID swap left Unity re-serializing the 4 diagonal clips with a CORRUPTED PPtr `fileID` (e.g. `862668387` instead of the sprite sub-asset `21300000`), so diagonals still resolved NULL. Final fix used the canonical `UnityEditor.AnimationUtility.SetObjectReferenceCurve` API (binding: path="", type=SpriteRenderer, "m_Sprite"), which wrote correct GUID + `fileID: 21300000` for all 8. On-disk verified: all 8 now `fileID: 21300000` + real GUID.

No code touched. No controller state machine touched. No change to how Warblade resolves. Reversible (clips are the only edited files).

## Verification (data-proof, not screenshots)
1. Controller-clip resolution (`Resources.Load` controller, AnimationUtility.GetObjectReferenceCurve): **8/8 idle clips resolve to their distinct real sprite WITH valid texture; null=0.**
2. Runtime animator proof: instantiated a temp Animator with the live controller, drove all 8 (DirX,DirY) facings per `PlayerAnimator.SnapToEight`, read `SpriteRenderer.sprite` each direction:
   - E->east, NE->NE, N->north, NW->NW, W->west, SW->SW, S->south, SE->SE — **8/8 OK, 0 bad.**
   - => animator now produces a valid sprite every frame; `PlayerClassManager`'s broken-clip fallback path is NO LONGER triggered for Elementalist.
3. Warblade cross-check: its controller = 8 idle clips, 8/8 resolve — Elementalist now mirrors it exactly.

## Console status
read_console (Error + Warning) after refresh AND after all runtime proofs: **0 errors, 0 warnings.** Clean (0-surprise).

## BLOCKED / deviations
None. No code or locked-system changes. Only the 8 Elementalist idle `.anim` files were modified (sprite PPtr re-pointed via Unity API). Run/cast clips exist on disk but are not part of either class's Resources controller — left untouched (out of scope, matches Warblade).
