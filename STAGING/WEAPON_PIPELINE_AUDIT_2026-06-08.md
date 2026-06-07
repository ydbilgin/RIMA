# WEAPON PIPELINE AUDIT 2026-06-08

Scope: static file/code audit only. No code changes, no Unity MCP, no Play Mode.

Primary verdict: the Warblade Level1 weapon path is demo-usable for one linear blade, but it is not a 10-class weapon mount architecture yet. The live path is `Player.prefab -> HandAnchorAttach -> WeaponDatabase.asset -> Warblade_Greatsword.prefab -> OrientationSync`, and several older docs are stale in both directions: some still call OrientationSync dead, while current code wires it; some still propose weapon art/anim work that the 2026-06-05 code-animation decision moved out of demo.

## 1. Executive verdict

Pipeline status: PARTIAL PASS for Warblade demo, FAIL for 10-class coverage.

- CONFIRMED: Warblade/Base is wired in the canonical runtime `Assets/Prefabs/Player.prefab`: `HandAnchorAttach` exists, `weaponDatabase` points to GUID `4ff6c6f519482f54da6847ca1e91ed7a`, `classId: Warblade`, `attachMode: 0`, `bodyRenderer` assigned, `orientationSync` assigned (`Assets/Prefabs/Player.prefab:268-277`).
- CONFIRMED: `Start()` spawns `AttachWeapon("Base")`, then wires the spawned weapon into `OrientationSync.SetWeaponTransform()` and auto-finds one `SpriteRenderer` (`Assets/Scripts/Systems/Combat/HandAnchorAttach.cs:82-94`).
- CONFIRMED: `WeaponDatabase.asset` contains only Warblade/Base, with `twoHanded=1` and `orientBetweenHands=1` metadata, but no usable multi-class schema (`Assets/Resources/WeaponDatabase.asset:15-23`).
- REFUTED/STALE: "OrientationSync is dead" from `A1_WEAPONDB_CLARIFY.md` is no longer true for the canonical runtime player. That doc says only the old combat weapon prefab references it (`STAGING/A1_WEAPONDB_CLARIFY.md:56-65`), but current `Player.prefab` has an `OrientationSync` component and `HandAnchorAttach` calls it (`Assets/Prefabs/Player.prefab:301-321`, `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs:87-89`, `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs:117-128`).
- REFUTED/STALE: the old plan's "Greatsword PNG yok" is superseded. The lock doc already corrects it to a 280-byte placeholder (`STAGING/WEAPON_ANIM_VFX_PRODUCTION_LOCK.md:15-18`), and the live file is 280 bytes with PPU 64, Point, custom pivot 0.18/0.5 (`Assets/Resources/Weapons/Warblade_Greatsword.png.meta:41-57`).
- CONFIRMED: demo animation strategy is now code-only for new char/mob animation. `CODEANIM_DECISION` says new char/mob animation production is zero for demo (`STAGING/CODEANIM_DECISION_2026-06-05.md:14-17`), while `ANIMATION_PROMPT_CATALOG` still describes expensive SPLIT body animation (`STAGING/ANIMATION_PROMPT_CATALOG.md:142-157`, `STAGING/ANIMATION_PROMPT_CATALOG.md:352-378`). Treat catalog as post-demo/reference unless a later decision reopens it.

Executive decision: keep current Warblade fade-visible behavior for today; do not implement weapon hide until a weapon-inclusive slash arc flipbook exists. Build a minimal mount schema before adding Ranger/Shadowblade/Elementalist. Do not produce 8-direction weapon sprites.

## 2. Dosya dosya bulgular

### `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs`

1. [KRITIK] Class is serialized but effectively single-class by default.
   Evidence: `classId = "Warblade"` is serialized at line 24; runtime spawn always calls `AttachWeapon("Base")` at line 84; lookup uses `weaponDatabase?.GetWeapon(classId, formId)` at line 165. Effect: class selection cannot drive weapon identity unless prefab/inspector or runtime class code updates `classId`. Recommendation: add a small runtime binding from selected class id before Start, or expose `SetClassIdAndAttach`.

2. [ORTA] Missing DB/entry fails silently.
   Evidence: null-safe lookup returns silently when entry or prefab is null (`Assets/Scripts/Systems/Combat/HandAnchorAttach.cs:165-166`). Effect: Brawler/no-weapon is indistinguishable from broken DB wiring. Recommendation: use explicit mount mode/NoWeapon and warn once for missing non-NoWeapon entries.

3. [ORTA] Spawn-to-OrientationSync bridge is implemented and current.
   Evidence: after spawn, `SetWeaponTransform(_weaponInstance.transform)` is called (`HandAnchorAttach.cs:87-89`), and `OrientationSync.Sync(dir)` runs in Level1 LateUpdate (`HandAnchorAttach.cs:117-128`). This refutes older dead-code claims in `A1_WEAPONDB_CLARIFY.md:56-65`.

4. [KRITIK] Level2 is present but not demo-ready.
   Evidence: Level2 loops `SpriteHandData[]` per frame (`HandAnchorAttach.cs:132-146`) and computes two-hand midpoint plus optional between-hands orientation (`HandAnchorAttach.cs:143-158`). `SpriteHandData` itself is per-sprite SO data (`Assets/Scripts/Data/SpriteHandData.cs:8-29`). Effect: needs hand data for every unique body frame. Recommendation: Demo = Level1; Post-demo = Level2 only after final frames.

5. [ORTA] Swing fade conflicts with older L3 hide lock but matches newer CURRENT_STATUS/playable decision.
   Evidence: lock L3 says attack should disable weapon SR and use weapon-inclusive slash arc (`STAGING/WEAPON_ANIM_VFX_PRODUCTION_LOCK.md:34-35`); current code fades alpha to 0.4 while swinging (`HandAnchorAttach.cs:35-37`, `HandAnchorAttach.cs:102-113`); playable roadmap says alpha-0.4 superseded NLM hide (`STAGING/PLAYABLE_ROADMAP_DECISION_2026-06-05.md:7-13`, `CURRENT_STATUS.md:147-149`). Recommendation: no enum today. Add `SwingVisibilityPolicy` only when Hide+slash-arc is actually implemented.

### `Assets/Scripts/Combat/OrientationSync.cs`

6. [ORTA] 8-direction data is live, but visual body animator remains 4-diagonal.
   Evidence: `FacingDir8` has S, SE, E, NE, N, NW, W, SW (`Assets/Scripts/Core/FacingDir8.cs:3-12`); `OrientationSync` has 8 `handOffsets` and 8 `weaponRotations` (`OrientationSync.cs:11-32`); `PlayerAnimator` explicitly documents and snaps to 4 diagonal facings (`Assets/Scripts/Player/PlayerAnimator.cs:8-10`, `Assets/Scripts/Player/PlayerAnimator.cs:142-156`). Effect: weapon can rotate through 8 octants while body visuals are 4-diagonal, which is acceptable for Warblade but can look disconnected.

7. [KRITIK] Flip policy is hardcoded for linear blades only.
   Evidence: `ApplyFlipY` flips W/NW/SW and comments say it is for a linear blade (`OrientationSync.cs:80-88`). Effect: bow, pistols, grimoire, lantern, rune disc may invert incorrectly. Recommendation: move flip policy into weapon profile (`None`, `FlipYOnLeftDirs`, `OffhandMirrorX`, `Custom`).

8. [KRITIK] Single-renderer assumption blocks dual/compound weapons.
   Evidence: `SetWeaponTransform` gets the first child `SpriteRenderer` only (`OrientationSync.cs:52-56`); `HandAnchorAttach` does the same for sort/fade (`HandAnchorAttach.cs:91-93`, `HandAnchorAttach.cs:199-204`). Effect: dual daggers/axes/pistols cannot independently sort, flip, fade, or offset. Recommendation: introduce `WeaponMountView` with main/offhand/static/hover renderer refs.

9. [ORTA] Rotation convention is horizontal-right.
   Evidence: E rotation is 0, S -90, N 90 (`OrientationSync.cs:22-32`); production constraints identify live greatsword as horizontal, tip right, grip left (`STAGING/chatgpt_weapon_pack/02_PRODUCTION_CONSTRAINTS.md:3-8`, `STAGING/chatgpt_weapon_pack/02_PRODUCTION_CONSTRAINTS.md:25-28`). Recommendation: keep all directional weapons authored horizontal-right; non-directional focus objects opt out of blade rotation.

### `Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs`

10. [KRITIK] Current schema cannot carry 10 class mount needs.
    Evidence: fields are class/form/prefab/anchor/grip/twoHanded/orientBetweenHands/orientationOffset/handOffsets only (`WeaponDatabaseSO.cs:8-20`). Canon requires no-weapon Brawler, hover Elementalist/Summoner, dual Shadowblade/Ravager/Gunslinger, Ronin scabbard, Ranger left-hand bow (`STAGING/chatgpt_weapon_pack/01_CANON_WEAPONS.md:11-18`, `STAGING/chatgpt_weapon_pack/01_CANON_WEAPONS.md:22-31`). Recommendation: add minimal profile fields, not a mega-SO.

11. [ORTA] `handOffsets` in `WeaponEntry` is unused in live Level1.
    Evidence: `WeaponEntry.handOffsets` exists (`WeaponDatabaseSO.cs:19`), but Level1 uses `OrientationSync.handOffsets`; `HandAnchorAttach.AttachWeapon` uses only `anchorOffset` for spawn (`HandAnchorAttach.cs:169-171`). `A1_WEAPONDB_CLARIFY` also notes canonical asset lacks serialized handOffsets and Level1 does not use them (`STAGING/A1_WEAPONDB_CLARIFY.md:87-90`). Recommendation: either remove from current entry or migrate into the new mount profile as intentional per-weapon data.

12. [ORTA] `twoHanded` and `orientBetweenHands` only matter in Level2.
    Evidence: use is inside Level2 branch only (`HandAnchorAttach.cs:143-158`). In canonical asset they are set for Warblade (`Assets/Resources/WeaponDatabase.asset:21-23`), but Level1 ignores them. Recommendation: keep as future metadata; do not expect two-hand behavior today.

### `Assets/Scripts/Data/SpriteHandData.cs`

13. [ORTA] Correct primitive for pixel anchors, but high authoring cost.
    Evidence: per-sprite `handLeftPx`, `handRightPx`, `hasLeftHand`, `hasRightHand`, and `Matches(Sprite)` (`SpriteHandData.cs:8-29`). Effect: useful after animation lock; expensive before timing/art freeze. Recommendation: Post-demo or hero-polish only.

### `Assets/Scripts/Editor/Combat/OrientationSyncAnchorEditor.cs`

14. [ORTA] Useful 8-dir hand anchor tuning, but not weapon-profile tuning.
    Evidence: custom editor only intercepts `handOffsets` (`OrientationSyncAnchorEditor.cs:23-36`) and provides active direction/vector field/scene handles (`OrientationSyncAnchorEditor.cs:49-80`, `OrientationSyncAnchorEditor.cs:119-185`). It does not edit `WeaponDatabaseSO` entries, rotations, flip policy, offhand, hover, or static scabbard. Recommendation: enough for Warblade/Ranger first pass; insufficient once dual/hover/static modes land.

### Docs and asset facts

15. [KRITIK] `WEAPON_BATCH_PLAN` has canon drift.
    Evidence: it lists Hexer as curse staff, Elementalist staff/orb, Gunslinger flintlock, Brawler gauntlet/fist (`STAGING/WEAPON_BATCH_PLAN.md:21-26`, `STAGING/WEAPON_BATCH_PLAN.md:34-37`, `STAGING/WEAPON_BATCH_PLAN.md:46-50`). Canon says Hexer is grimoire/totem/scepter, Elementalist is floating rune disc and staff/wand forbidden, Gunslinger western forbidden, Brawler no weapon (`STAGING/chatgpt_weapon_pack/01_CANON_WEAPONS.md:11-18`, `STAGING/chatgpt_weapon_pack/01_CANON_WEAPONS.md:24-31`). Recommendation: supersede or rewrite `WEAPON_BATCH_PLAN`.

16. [ORTA] PixelLab session plan mostly fixes weapon drift for the 3 immediate weapons.
    Evidence: Ranger bow left-hand canon (`STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md:154-172`), Shadowblade single dagger/offhand flipX/no glow (`STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md:176-195`), Elementalist rune disc not attached/no staff (`STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md:199-218`, `STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md:327-333`). Recommendation: use this over `WEAPON_BATCH_PLAN` for the next gated session.

## 3. Karar tablosu

| Topic | Decision | Evidence |
|---|---|---|
| PPU/filter | PPU 64, Point, alpha transparency | live weapon meta `filterMode: 0`, `spritePixelsToUnits: 64`, `alphaIsTransparency: 1` (`Assets/Resources/Weapons/Warblade_Greatsword.png.meta:41-61`); constraints lock PPU64+Point (`STAGING/chatgpt_weapon_pack/02_PRODUCTION_CONSTRAINTS.md:3-8`). |
| Size strategy | Generate target-size native, do not generate large then downscale | constraints identify 128->64 pixel-grid risk and PixelLab native-pixel advantage (`02_PRODUCTION_CONSTRAINTS.md:10-23`); session plan locks target-size and ref-downscale (`STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md:143-149`). |
| Drawing angle | Directional weapons horizontal-right, grip left, tip right | live convention in constraints (`02_PRODUCTION_CONSTRAINTS.md:3-8`, `02_PRODUCTION_CONSTRAINTS.md:25-28`) and OrientationSync E=0/S=-90/N=90 (`OrientationSync.cs:22-32`). |
| Animation strategy | Demo: no new char/mob anim; code/VFX melee; catalog post-demo/reference | codeanim decision (`STAGING/CODEANIM_DECISION_2026-06-05.md:14-17`, `STAGING/CODEANIM_DECISION_2026-06-05.md:28-33`) supersedes catalog SPLIT production (`STAGING/ANIMATION_PROMPT_CATALOG.md:142-157`). |
| Mount mode strategy | Demo: Level1 static + minimal profile. Post-demo: SpriteHandData Level2 | Level1 current prefab `attachMode: 0` (`Assets/Prefabs/Player.prefab:272`); Level2 SO cost in code (`HandAnchorAttach.cs:132-158`, `SpriteHandData.cs:8-29`). |
| PixelLab batch | Use <=8 item batches by size/ref target; do not regenerate live Warblade unless replacing placeholder intentionally | API limits and style ref sizing (`02_PRODUCTION_CONSTRAINTS.md:16-23`, `STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md:143-149`); live placeholder exists but is low-quality (`WEAPON_ANIM_VFX_PRODUCTION_LOCK.md:15-18`). |

## 4. 10 class mount matrix

| Class | Canon weapon | Mount mode | Sprite count | Draw direction | Runtime behavior | Risk/code need |
|---|---|---|---|---|---|---|
| Warblade | Two-handed greatsword | SingleHand/TwoHandVisual Level1 now; TwoHanded metadata later | 1 | horizontal-right | rotate 8-dir, blade flipY left dirs, sort behind N/NE/NW, attack fade today | PASS demo; hide waits for slash arc (`HandAnchorAttach.cs:102-113`, `WEAPON_ANIM_VFX_PRODUCTION_LOCK.md:34-35`). |
| Ranger | Compound bow, left hand | SingleHand left primary | 1 | horizontal-right | no blade flipY by default; custom rotation table likely | Needs `leftHandPrimary`; current `preferRightHand` is local and not DB-driven (`HandAnchorAttach.cs:29`, canon `01_CANON_WEAPONS.md:12`, `01_CANON_WEAPONS.md:23`). |
| Shadowblade | Twin reverse-grip daggers | DualMirrored | 1 main sprite + mirrored offhand renderer | horizontal-right/reverse grip | offhand flipX/custom offset, independent sorting | Current first-renderer model fails (`OrientationSync.cs:52-56`); no glow (`01_CANON_WEAPONS.md:24`). |
| Ravager | Dual compact axes | DualMirrored | 1 sprite + mirrored offhand | horizontal-right | two hands, aggressive wide offsets | Same dual renderer/sort gap; canon dual axes (`01_CANON_WEAPONS.md:15`, `01_CANON_WEAPONS.md:26`). |
| Gunslinger | Dual rift-tech pistols | DualMirrored | 1 sprite + mirrored offhand | horizontal-right | custom pistol rotations; no western | Same dual gap; western/flintlock batch drift must be removed (`01_CANON_WEAPONS.md:11`, `01_CANON_WEAPONS.md:28`, `WEAPON_BATCH_PLAN.md:25`). |
| Elementalist | Floating golden rune disc | HoverPalm | 1 disc | non-directional/top-down disc | hover above right palm, bob/spin, no blade rotate, no hand attach | Needs `HoverWeaponMount` or mode branch; current attach should not own it (`01_CANON_WEAPONS.md:13`, `01_CANON_WEAPONS.md:25`, `PIXELLAB_SESSION_PLAN_2026-06-07.md:199-218`). |
| Summoner | Soul lantern, left-hand/hover | HoverPalm or LeftHover | 1 lantern | mostly non-directional | left hover/hand-adjacent; no staff swing | Can share hover mode plus left primary; canon says staff swing no (`01_CANON_WEAPONS.md:18`, `01_CANON_WEAPONS.md:30`). |
| Hexer | Grimoire/totem/scepter | StaticFrontFocus or HoverPalm or SingleHand | 1 focus | non-directional or slight horizontal | front focus/grimoire, no whip | Needs non-blade flip/rotation policy; batch plan stale (`01_CANON_WEAPONS.md:16`, `01_CANON_WEAPONS.md:31`, `WEAPON_BATCH_PLAN.md:21`). |
| Ronin | Katana drawn + mandatory left scabbard | SingleHand + StaticTorsoAttachment | 1 katana + baked or static scabbard | katana horizontal-right | right-hand drawn katana; scabbard stays left waist | Demo-safe: bake scabbard into body. Post-demo: static torso attachment (`01_CANON_WEAPONS.md:10`, `01_CANON_WEAPONS.md:27`). |
| Brawler | No weapon; wraps/cosmetic | NoWeapon | 0 weapon prefab | none | spawn nothing by design | Must not be silent DB miss; add explicit NoWeapon (`01_CANON_WEAPONS.md:17`, `01_CANON_WEAPONS.md:29`). |

Minimal schema that covers all 10 without mega-SO:

```text
mountMode: SingleHand | DualMirrored | HoverPalm | StaticFrontFocus | StaticTorsoAttachment | NoWeapon
flipPolicy: None | FlipYOnLeftDirs | OffhandMirrorX | Custom
swingVisibility: KeepVisible | Fade | Hide
leftHandPrimary: bool
usesSwingArc: bool
mainPrefab: GameObject
offhandPrefab: GameObject optional
staticAttachmentPrefab: GameObject optional
anchorOffset: Vector3
gripOffset: Vector3
handOffsets[8]: Vector2 optional
rotations[8]: float optional
hoverOffset: Vector2
```

ChatGPT proposed schema assessment: CONFIRMED in principle for `mountMode`, `flipPolicy`, `swingVisibility`, `leftHandPrimary`, `usesSwingArc`, `handOffsets[8]`, `rotations[8]`, `hoverOffset`, `offhandPrefab`, `staticTorsoAttachmentPrefab`; REFUTED only if implemented as a huge monolithic SO. Keep fields small and weapon-facing.

## 5. Minimal kod degisiklikleri

No code was written in this audit. Proposed patch set after approval:

1. `WeaponDatabaseSO.cs` add minimal mount profile fields.
   Reason: current fields cannot express dual/hover/static/no-weapon (`WeaponDatabaseSO.cs:8-20`).
   Size: about 45-70 LOC.
   Risk: serialized asset migration.

2. `HandAnchorAttach.cs` add explicit missing-entry diagnostics and NoWeapon handling.
   Reason: current missing DB/entry silently returns (`HandAnchorAttach.cs:165-166`).
   Size: about 15-25 LOC.
   Risk: low.

3. New `WeaponMountView.cs` on weapon prefab for main/offhand/static/hover renderer refs.
   Reason: current first child renderer assumption (`OrientationSync.cs:52-56`, `HandAnchorAttach.cs:91-93`).
   Size: about 40-60 LOC.
   Risk: prefab wiring.

4. `OrientationSync.cs` make flip policy/profile-driven, optionally consume per-weapon rotations.
   Reason: hardcoded blade flipY (`OrientationSync.cs:80-88`).
   Size: about 40-70 LOC.
   Risk: Warblade regression; needs 8-dir rotation tests.

5. `HoverWeaponMount.cs` or `HandAnchorAttach` mode branch for hover objects.
   Reason: Elementalist/Summoner are not hand-held (`01_CANON_WEAPONS.md:13`, `01_CANON_WEAPONS.md:18`).
   Size: about 45-80 LOC.
   Risk: visual-only, low if isolated.

6. Optional later: `SwingVisibilityPolicy`.
   Reason: hide only matters when slash arc flipbook exists (`WEAPON_ANIM_VFX_PRODUCTION_LOCK.md:141-146`).
   Size: about 20 LOC.
   Risk: low, but do not do today.

## 6. Dokuman cleanup

Stale/superseded:

- `STAGING/WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md`: PPU100 and asset-first order are explicitly superseded by the lock doc (`WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md:6`, `WEAPON_ANIM_VFX_PRODUCTION_LOCK.md:26-41`). Keep only prompt/mount history.
- `STAGING/WEAPON_ANIM_VFX_PRODUCTION_LOCK.md`: its older "OrientationSync dead" and 4-diagonal bridge lock is stale after current `Player.prefab`/`HandAnchorAttach` wiring (`WEAPON_ANIM_VFX_PRODUCTION_LOCK.md:37-41`, `Assets/Prefabs/Player.prefab:301-321`, `HandAnchorAttach.cs:117-128`).
- `STAGING/A1_WEAPONDB_CLARIFY.md`: update dead-code status; its canonical DB/prefab sections remain useful (`A1_WEAPONDB_CLARIFY.md:7-18`, `A1_WEAPONDB_CLARIFY.md:40-52`).
- `STAGING/WEAPON_BATCH_PLAN.md`: replace due to Elementalist/Gunslinger/Hexer/Brawler canon drift (`WEAPON_BATCH_PLAN.md:21-26`, `01_CANON_WEAPONS.md:13-18`).
- `STAGING/ANIMATION_PROMPT_CATALOG.md`: prepend status: "DEMO ACTIVE: none/new char anim 0; POST-DEMO reference only; DO NOT GENERATE BEFORE TIMING FREEZE" because `CODEANIM_DECISION` supersedes demo animation generation (`CODEANIM_DECISION_2026-06-05.md:14-17`).
- `STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md`: keep as active for 4 mobs + 3 weapons, but remove the "ask user whether 64px or roster sizes" ambiguity. COMBAT_ROSTER S43 wins (`TASARIM_COMBAT_ROSTER.md:9-18`, `PIXELLAB_SESSION_PLAN_2026-06-07.md:7-20`).

Canonical docs to use:

- Weapon canon: `STAGING/chatgpt_weapon_pack/01_CANON_WEAPONS.md`.
- Production constraints: `STAGING/chatgpt_weapon_pack/02_PRODUCTION_CONSTRAINTS.md` plus `STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md` for immediate 3-weapon session.
- Animation demo policy: `STAGING/CODEANIM_DECISION_2026-06-05.md`.
- Live runtime status: `CURRENT_STATUS.md` top blocks, especially weapon/status line (`CURRENT_STATUS.md:147-149`).

## 7. Patch plani

1. Commit audit doc only: this file.
2. Schema/profile minimal patch: `WeaponDatabaseSO`, assets migration for Warblade/Base only.
3. Runtime modes: NoWeapon + dual + hover + static attachment.
4. Tests:
   - Warblade spawn.
   - Brawler no-weapon.
   - Missing non-NoWeapon warning, no crash.
   - PPU64/Point import validation.
   - OrientationSync 8-dir rotation degrees.
   - Per-weapon flip policy.
   - Dual main+offhand renderer.
   - Elementalist hover not blade-rotated.
   - Ranger left-hand primary.
   - Ronin scabbard static/baked decision.
5. Docs cleanup: update/mark stale docs above.
6. Asset plan cleanup: replace batch plan with target-size, <=8 item, style refs downscaled, no live asset regen unless explicitly replacing placeholder.

Visual QC checklist:

- 8 dirs: weapon remains near intended hand/palm.
- N/NE/NW behind body; S/SE/SW/E/W front, unless profile overrides.
- Attack fade/hide matches profile.
- Slash arc carries weapon identity only after flipbook exists.
- Bow/pistol/grimoire/lantern/disc are not broken by blade flipY.
- Hover disc/lantern stays above palm and sorts readably.

Asset validation:

- PPU64, Point, no compression/mip blur, alpha transparency.
- Grip pivot set manually.
- Target-size native output.
- No anti-aliasing.
- No wrong-class motifs.
- No Elementalist staff/wand.
- No western Gunslinger.
- No Shadowblade blade glow.
- No Brawler weapon prefab.

## 8. RED LIST

- No big rewrite.
- Do not touch `PlayerAnimator` for this mount patch; it already owns 4-diagonal body visual facing (`PlayerAnimator.cs:8-10`, `PlayerAnimator.cs:142-156`).
- No PPU100.
- No big-canvas-then-downscale workflow.
- No Elementalist staff/wand.
- No western/flintlock Gunslinger.
- No Shadowblade embedded blade glow.
- No 8-direction weapon sprites.
- No Brawler weapon prefab.
- Do not start producing all body animations in PixelLab for demo.
- Do not hide attack weapon until weapon-inclusive slash arc flipbook exists.

## 9. Son oneri: Bugun yapilmali / post-demo / asla

Bugun yapilmali:

- Mark this audit as the current weapon pipeline ground truth.
- Use target-size PixelLab output, horizontal-right directional weapons, PPU64/Point.
- Produce only the gated immediate weapon risk set: Elementalist rune disc, Ranger bow, Shadowblade dagger. Warblade placeholder replacement is optional if the current visible placeholder is unacceptable, but do not regenerate unrelated weapons.
- Patch minimal mount profile only after approval.
- Keep Warblade attack fade at alpha 0.4 for now.

Post-demo:

- SpriteHandData Level2 per-frame anchors.
- Weapon-inclusive slash arc flipbooks and `SwingVisibilityPolicy.Hide`.
- Ronin static scabbard if baked body is insufficient.
- Full 10-class weapon import and tuning.
- SPLIT body animation catalog for hero/boss/polish only after timing freeze.

Asla:

- 8-direction weapon sprite production.
- PPU100 mixed weapon/body pipeline.
- Big-canvas downscale as default pixel-art strategy.
- Elementalist staff, Gunslinger western/flintlock, Shadowblade glow, Brawler weapon.
- Mega-SO rewrite.

## Mob/PixelLab addendum

Decision: COMBAT_ROSTER S43 wins over T2_MOB_PROTOTYPE_SPEC for current demo mob sizes.

Evidence:

- `TASARIM_COMBAT_ROSTER.md` marks Faz 1 mob list canonical/S43 current (`STAGING/graphify_corpus/TASARIM_COMBAT_ROSTER.md:9-18`).
- It sets Shard Walker 112px (`TASARIM_COMBAT_ROSTER.md:63-70`, `TASARIM_COMBAT_ROSTER.md:196-197`), Void Thrall 128px (`TASARIM_COMBAT_ROSTER.md:212-220`), Chain Warden 128px (`TASARIM_COMBAT_ROSTER.md:224-238`), Relic Caster 80px (`TASARIM_COMBAT_ROSTER.md:259-273`).
- Older T2 spec is locked but older scope and gives Shard/Bruiser/Imp 64px prototype sizes (`STAGING/graphify_corpus/TASARIM_T2_MOB_PROTOTYPE_SPEC.md:12-17`, `STAGING/graphify_corpus/TASARIM_T2_MOB_PROTOTYPE_SPEC.md:38-45`, `STAGING/graphify_corpus/TASARIM_T2_MOB_PROTOTYPE_SPEC.md:111-118`, `STAGING/graphify_corpus/TASARIM_T2_MOB_PROTOTYPE_SPEC.md:200-204`).
- PixelLab session plan already identifies the conflict and uses the COMBAT_ROSTER set while asking for user confirmation (`STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md:7-20`, `STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md:37-129`).

Mob decision: demo 4 archetypes are Shard Walker 112 ranged, Void Thrall 128 splitter, Chain Warden 128 controller, Relic Caster 80 support. Penitent Bruiser, Fracture Imp, Seam Crawler, Augur, Hulk are post-demo or later encounter expansion unless an encounter-design decision explicitly pulls them back in.
