# Verified Screenshot Set — P7 (demo report) — 2026-06-14

Documents the post-P1..P6 working state. All world-space shots captured via Unity
**scene_view** (gizmos OFF, reference grid OFF), full-flow Play (`playModeStartScene=MainMenu`),
real Player prefab spawned by `RoomRunDirector` in the canonical `_Arena` scene.
No code changes; no faked states. Play STOPPED and scene left clean (`isDirty=false`) afterward.

Method note: the live runtime states were reached by driving the REAL flow inside Play —
enter at MainMenu → set `PlayerClassManager.SelectedClass` → `SceneManager.LoadScene("_Arena")`
(exactly what the CharacterSelect Confirm button does). The slash arcs were emitted with the
SAME `SkillVfx.MeleeArc(center, facing, element)` call the gameplay code uses (Warblade basic =
`VfxElement.Physical`; GravityCleave = `VfxElement.Void`), then the game was paused to freeze the
0.18s arc for a clean frame. This is the genuine VFX path, not a mock.

P7.5 re-capture (2026-06-14, after invisible-sprite fix commit 38cad978): shots 3 and 6 were
re-captured by driving the same full-flow path with Elementalist selected. Each subject's
SpriteRenderer.sprite was confirmed non-null at the exact capture moment via execute_code. Shot 3
(Elementalist player body) now genuinely VISIBLE → P7.5 player fix verified. Shot 6's player is
visible, but FractureImp/Penitent enemy bodies remain invisible — the keeper does not cover the
animator's non-null-empty-sprite failure mode (honest block; details in shot 6).

## Shots

| # | Filename | Verifies | Status |
|---|----------|----------|--------|
| 1 | `warblade_idle_weapon.png` | P6 — greatsword grip IN HAND | CAPTURED |
| 2 | `warblade_ember_swing.png` | P5 — single EMBER arc, no blue clash | CAPTURED |
| 3 | `elementalist_idle.png` | P7.5 — Elementalist player BODY now VISIBLE | CAPTURED (verifies P7.5 fix) |
| 4 | `gravitycleave_void.png` | P5b — element-agnostic tint stays PURPLE for Void | CAPTURED |
| 5 | `arena_overview.png` | canonical cliff-tile floating-island room | CAPTURED |
| 6 | `combat_moment.png` | P7.5 — enemy BODY render | PARTIAL — player VISIBLE; FractureImp/Penitent body still BLOCKED (see note) |

### 1. warblade_idle_weapon.png — CAPTURED (verifies P6)
Warblade idle, facing SE (`warblade_idle_SE`). The `Warblade_Greatsword(Clone)` is mounted and
active via `HandAnchorAttach`; blade reads as gripped in the hand with no detached gap (P6 anchor
fix, `WeaponDatabase` Warblade anchorOffset (0.02,0.33)). Data-proof: weapon mount enabled=True,
weaponInstance active=True, primary=Warblade.

### 2. warblade_ember_swing.png — CAPTURED (verifies P5)
Single ember/orange-yellow crescent arc at the player (`SkillVfx.MeleeArc(..., VfxElement.Physical)`,
the exact call in `MeleeChainBehavior`). One warm ember arc only — the old blue `SlashArcVFX`
LineRenderer clash is gone (P5).

### 3. elementalist_idle.png — CAPTURED (verifies P7.5 player fix) — 2026-06-14
Re-captured after the P7.5 invisible-sprite fix (commit 38cad978) landed. Full-flow Play
(`playModeStartScene=MainMenu`): entered at MainMenu → `PlayerClassManager.SelectedClass =
Elementalist` → `SceneManager.LoadScene("_Arena")` (the CharacterSelect Confirm path). The
Elementalist BODY now RENDERS: the `PlayerAnimator` persistent LateUpdate sprite-keeper restores the
cached class idle sprite (`PlayerClassManager.SetFallbackSprite`) every frame after the broken idle
clip drives the body to null. Data-proof at the exact capture moment: Body SpriteRenderer
`sprite = elementalist_idle_south` (non-null), enabled=True, color=white, bounds 1.88×1.88. Framed
tight on the character (scene_view, ortho size 2.4, gizmos OFF). The PNG shows the visible chibi
Elementalist figure on the cliff-tile floor. Previously BLOCKED (FIX A regression nulled the body);
now genuinely visible — no fake, no forced state.

### 4. gravitycleave_void.png — CAPTURED (verifies P5b)
Purple/magenta crescent arc via `SkillVfx.MeleeArc(center, facing, VfxElement.Void)` — the exact arc
emitted by `GravityCleave.Execute`. Confirms the element-agnostic hue-preserving additive tint
(`1.5/max(r,g,b)`) keeps Void → PURPLE (the P5b regression fix), distinct from the ember-orange of
the Physical arc produced by the same helper.

### 5. arena_overview.png — CAPTURED (canonical room)
`_Arena` IsoRoomBuilder cliff-tile floating island: iso-diamond floor, cliff-drip edges, void/purple
backdrop, cyan rift crack, two rift-light pillars. Matches the room canon.

### 6. combat_moment.png — PARTIAL (player VISIBLE; FractureImp/Penitent body still BLOCKED) — 2026-06-14
Re-captured in the same full-flow Play session as shot 3. Player (Elementalist) is VISIBLE next to a
live AI-active `FractureImp(Clone)` (attack state, adjacent at ~1u). Data-proof at capture: player
Body `sprite = elementalist_idle_south` (non-null) — the P7.5 PLAYER fix is verified here too.

HONEST BLOCK on the ENEMY part: the P7.5 enemy keeper does NOT make FractureImp/Penitent visible in
this runtime. Root cause found this pass: the enemy attack clips' object-reference (sprite) keyframes
are NULL in the asset (`fractureimp_attack_*`, `penitent_attack_*`: 4/4 null frames), and when the
Animator samples a null sprite keyframe it writes a NON-NULL but empty/textureless placeholder Sprite
(name='' , texture=null, 48×48) — NOT a true `null`. `EnemyAnimator.LateUpdate` only restores its
cached `_fallbackSprite` (the valid `fracture_imp` / `13_penitent_bruiser` prefab sprite) when
`sr.sprite == null`, so this non-null-empty case bypasses the keeper. Verified decisively: manually
setting the body to the valid `fracture_imp` sprite was overwritten back to the empty sprite within
one frame by the animator. Both prefabs (`Penitent.prefab`, `FractureImp.prefab`) carry valid
authored sprites + valid cached fallbacks; the keeper's guard condition is the only gap.

A visible FractureImp/Penitent body therefore cannot be captured without a code change (e.g. keeper
restore when `sprite == null || sprite.texture == null`, or disabling the broken animator) — out of
scope for this capture-only pass. The PNG shows the genuine combat frame: visible Elementalist player
+ the FractureImp's hitbox/AI marker (the enemy body art is absent, NOT faked).

FOLLOW-UP (post-demo, supersedes the P7.5 DEFER): widen the keeper guard to
`sprite == null || sprite.texture == null`, OR re-point/re-import the archived enemy attack frames so
the clips no longer sample null object-references.

## UI-overlay verifications (NOT screenshottable in MCP — data-proof only)
ScreenSpace-Overlay UI (skill bar, tooltips, draft panel) never appears in MCP scene_view/game-view
captures, so these are verified by the data-proofs in their `_done` reports, not by screenshot:

- **Skill-bar basic-attack tooltips (P2)** → `STAGING/_process/2026-06/_done_tooltip_2026-06-14.md`
  Data-proof: slot 0 = "Cleave / Temel Saldırı / Hasar: 25"; slot 1 = "War Stomp / Temel Yetenek /
  Hasar: 34 / Maliyet: 30 / CD: 1,5s" (BuildBasicAttackTooltip found=True).
- **Reward → 2nd-skill draft (P1)** → `STAGING/_process/2026-06/_done_skillreward_2b_2026-06-14.md`
  Data-proof: chest reward path fixes (AssignActive null-guard, ShowDraftWithSkill EnsureDependencies
  + IsDraftActive gate, ChestBehavior pool); 2nd skill now reaches the skill bar.
- **4-slot replace (P4)** → `STAGING/_process/2026-06/_done_4slot_2c_2026-06-14.md`
  Data-proof (reflection): band-aware HasFreePrimarySlot/HasFreeSecondarySlot replace trigger;
  full-band → replace-mode, half-empty primary → fills next slot; slot-0 clobber aborted. PASS.

## Folder
`STAGING/_process/2026-06/_verified_shots_2026-06-14/`
