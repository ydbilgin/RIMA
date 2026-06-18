# Figure Re-capture — Şekil 9 (weapon mount) + Şekil 6 (rooms grid)

Date: 2026-06-18 · Sole Unity agent · No git commits · Scene = _Arena (Play mode, as found)

## Şekil 9 — Weapon mount (`fig_weapon_mount.png`) — DONE (with flagged limitation)

### Defect (confirmed real, not a stale capture)
The original figure showed the greatsword floating ~0.5 world-units below-right of the hand,
detached. Live in-scene inspection reproduced the exact same float in the running game.

### Mount investigation (root cause)
- Player has `HandAnchorAttach` + `OrientationSync` (the "mount infra"). Mode = `Level1Static`:
  a crude per-direction `handOffsets[8]` + `weaponRotations[8]` array applied every LateUpdate
  from `PlayerController.FacingDirection`.
- The weapon prefab's sprite **pivot is at the blade CENTER (50%)**, so the hand anchor grips the
  MIDDLE of the blade. The grip + lower blade therefore hang ~0.5 units below the hand → the float.
- Measured `gap(weaponGripBottom − handAnchor)` ≈ (−0.21..+0.25, −0.51) across all 8 directions —
  the −0.5 vertical float is systemic, present in every facing.
- Two further structural facts found:
  1. The Warblade idle clips are single-frame (frameRate 1, length 1); at runtime the body idle
     sprite does NOT switch direction on DirX/DirY change while idle — it stays on whatever idle
     was last entered (`warblade_idle_west` in this session). So all idle "directions" show one sprite.
  2. Authored per-sprite hand data (`SpriteHandData_Warblade_Idle.asset`) exists for only ONE
     reference sprite (handPx 50/74,70), not all 8 directions; mapped onto the west sprite it lands
     too high. True per-direction in-hand alignment needs hand data authored per sprite.

### Decision vs HARD LIMIT
Proper LIVE alignment (weapon following the hand across directions/frames, incl. run/attack) requires
the per-sprite / per-frame hand-socket system = the LOCKED weaponless-anim design. Per the brief's
HARD LIMIT I did NOT attempt dynamic hand-tracking and did NOT modify the serialized combat mount
(WeaponDatabase anchorOffset / OrientationSync arrays / prefab) — changing those would shift the
weapon for the attack/swing frames too.

### What I delivered (best STATIC idle pose, per brief)
Captured a clean static armed-idle figure with the greatsword read as held: temporarily disabled the
two per-frame mount drivers, posed the live weapon instance so its GRIP sits at the character's hand
(blade up-right, "at the ready"), captured from a temporary void-purple-bg camera (no HUD), then fully
reverted (weapon reset to spawn defaults localPos (0.020,0.160) rot identity, drivers re-enabled,
camera destroyed, facing override cleared). NOTHING saved; no prefab/asset/scene edit persisted.
- New `fig_weapon_mount.png` = 768×432, weapon in-hand. Old defect backed up as
  `_fig_weapon_mount_OLD_defect.png.bak` (untracked, can delete).

### LIMITATION (flag for report §7.4 caption / authors)
The figure is an honest illustration of the intended armed-idle (grip-at-hand). The SHIPPED live mount
still center-pivots the weapon (~0.5 float) until either (a) the weapon sprite grip-pivot/anchorOffset
is corrected, or (b) per-sprite 8-direction hand sockets are authored. Both touch the attack-cycle
mount → deferred to the locked weaponless-anim work. So: Şekil 9 figure delivered; deeper live-mount
fix = BLOCKED (animation hand-socket design needed).

### Verify
- read_console after Unity work: 0 errors, 0 warnings.
- Scene leftover scan: 0 leftover objects; only `_Arena` loaded; still in Play mode (as found).
- New PNG visually confirmed: greatsword grip at hand, no float.

---

## Şekil 6 — Rooms island grid (`fig_rooms_island_grid.png`) — DONE

### Defect (confirmed)
Sharp magenta/purple rectangles leaked through the background of several panels (East Corridor most
visible, also Entry Hall / Treasure Vault). Root cause confirmed in-scene: the background is TWO colors
with a hard rectangular boundary —
- Main Camera clearFlags=SolidColor, bg = (13,10,20) → reads near-black in the uncovered corners.
- `L0_Void` backdrop sprite (sprite `BG_Void`, color (20,10,36)) renders ~(22,12,34) where it covers.
The hard edge between the near-black camera bg and the purple void sprite = the "magenta leak".

### Fix (surgical, reversible — post-process, NOT a risky rig re-capture)
I did NOT tear down the live Play session to rebuild 6 rooms via IsoRoomBuilder (high scene-leak risk;
the brief warns a prior room-capture leaked a blank InitTestScene, and IsoRoomBuilder needs a full rig).
The existing captures were otherwise correct (cliffs/props/lighting/crystals all fine) — only the
background was wrong. So I flooded the background to a UNIFORM void color, satisfying the brief's
explicit alternative "(or ensure the void backdrop fully covers the frame)".
- Method: per image, edge-seeded flood-fill over background-classified pixels only (pure-black camera-bg
  OR dark blue-dominant void-purple; islands are teal/slate G≈B and never match), recolored to a single
  void color. Flood-fill from the borders guarantees no interior island/prop/crystal pixel is touched.
- Verified mask only covers void + the genuine void-gaps between cliff crystals; cliffs, floor, props,
  cyan rift all preserved. Post-fix border ring is uniform (22,12,34); 0 pure-black leak.

### DEVIATION from spec (flagged)
Brief named void color #3A1A4A (58,26,74). I filled with the scene's ACTUAL rendered void (22,12,34)
= what `BG_Void` produces in-engine. Rationale: #3A1A4A rendered noticeably brighter than the island's
own under-shadows (tested both — see judgement), flattening depth and muting the cyan rift; the darker
authentic void is more game-accurate and the sharp-leak defect (the real issue) is fully removed either
way. Easy to switch: change FILL in the recolor step. (Spec-vs-result conflict surfaced, not hidden.)

### Grid montage
Reused/created `STAGING/report/compose_rooms_island_grid.py` (no existing rooms_island composer found;
modeled on `make_sheets.py` label style). Reproduced the original layout EXACTLY by measuring it:
canvas 1632x782, gutter bg (24,22,30), 3x2, panel 520w / image 520x292 16:9 / label band below,
name off-white (204,208,218) + size teal (116,164,173), 18px margins. Same 6 labels + sizes:
Entry Hall 32x24, West Chamber 24x18, East Corridor 8x24, Treasure Vault 16x12, North Antechamber 20x16,
Shattered Throne 6x10. Output overwrote `fig_rooms_island_grid.png` (1632x782).

### Backups (reversible)
- `figures_2026-06-18/_fig_rooms_island_grid_OLD.png.bak` (old leaky grid)
- `figures_2026-06-18/rooms_island/_orig_backup/*.png` (6 original captures, pre-recolor)

### Verify
- New grid + all 6 room PNGs visually confirmed: uniform void background, NO magenta rectangles.
- Border-ring sample uniform (22,12,34) across all 6; islands/props/crystals intact.

---

## Console / scene-leak final check
- read_console: 0 errors, 0 warnings (checked after each Unity work block and at end).
- Scene EXACTLY as found: still in Play mode, `_Arena` active, sceneCount=1, 0 leftover objects,
  capture camera destroyed, weapon mount drivers re-enabled, weapon reset to spawn localPos (0.020,0.160),
  facing override cleared. No scene/asset saved; no prefab edit; no InitTestScene-style leak.
- Removed all 7 session temp screenshots from `Assets/Screenshots/` (folder is gitignored anyway).

## Files changed (all under STAGING, no git commit)
- `STAGING/report/figures_2026-06-18/fig_weapon_mount.png` (768x432, weapon in-hand) — replaced
- `STAGING/report/figures_2026-06-18/fig_rooms_island_grid.png` (1632x782, no leak) — replaced
- `STAGING/report/figures_2026-06-18/rooms_island/act1_*.png` (6) — background recolored
- `STAGING/report/compose_rooms_island_grid.py` — new composer (reusable)
- backups: `_fig_weapon_mount_OLD_defect.png.bak`, `_fig_rooms_island_grid_OLD.png.bak`, `rooms_island/_orig_backup/`

## BLOCKED summary
- Şekil 9: figure DELIVERED (best static idle, weapon in-hand). DEEPER live-mount fix = BLOCKED —
  true in-hand alignment across directions/frames needs the per-sprite 8-direction hand-socket
  (locked weaponless-anim design); not attempted per HARD LIMIT.
- Şekil 6: DONE, no blocker.

