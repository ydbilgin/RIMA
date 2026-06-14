# P6b — Warblade greatsword grip-to-hand offset (O1) — 2026-06-14

## VERDICT: DONE

User decision O1 (single static value, per-facing handOffsets OUT OF SCOPE) applied and verified.

---

## WHERE the weapon localPos actually comes from (corrects a P6 assumption)

The weapon instance localPos is NOT set in the weapon prefab and NOT hardcoded at spawn.
It comes from `WeaponDatabaseSO.WeaponEntry.anchorOffset`:
- `HandAnchorAttach.AttachWeapon()` (Assets/Scripts/Systems/Combat/HandAnchorAttach.cs:185):
  `_weaponInstance.transform.localPosition = entry.anchorOffset;`
- entry = `WeaponDatabase.asset` → Warblade/Base entry.

The HandAnchor localPos is DYNAMIC, NOT the static (0.30, 0.02) the P6 diag implied:
- `OrientationSync.Sync(dir)` (Assets/Scripts/Combat/OrientationSync.cs:64-67) OVERWRITES
  `handAnchor.localPosition = handOffsets[dir]` on every direction change.
- So the Warblade.prefab HandAnchor override (0.3, 0.0) only holds at idle BEFORE the first
  Sync; once facing syncs, HandAnchor.x = handOffsets[dir].x (±0.10).

Therefore the runtime grip world (relative to body) = `handOffsets[dir] + anchorOffset`.
The P6 diag's "+0.50 / floats right" was the IDLE/first-frame state (0.30 + 0.20). In actual
synced gameplay the dominant error was the LOW Y (grip near feet), plus a smaller X over-reach.

The cleanest single field for O1 = **`anchorOffset` in `Assets/Resources/WeaponDatabase.asset`**
(single static value, applies to all 8 facings; handOffsets per-facing untouched = O1 not O2).

## Ground-truth hand locations (actual in-game idle sprites, skin-tone detection)

In-game sprites = `Assets/Resources/Characters/Warblade/warblade_idle_*.png` (120x120, PPU64,
pivot (60,30)). NOTE: the animator uses these, NOT the `Art/.../Rotations/*` source sprites.

| Facing | sprite | RHand world |
|--------|--------|-------------|
| SE (primary) | warblade_idle_SE | (0.100, 0.288) |
| E | warblade_idle_east | (0.047, 0.375) |
| N | warblade_idle_north | (0.197, 0.229) |

## Chosen value

`anchorOffset` (0.20, 0.10) → **(0.02, 0.33)**.

Solves SE: grip = handOffsets[SE](0.08,-0.04) + (0.02,0.33) = (0.10, 0.29) ≈ SE RHand (0.100,0.288).

Live end-to-end (Play, Warblade variant, real AttachWeapon code path, Sync(SE)):
- weapon.localPos = (0.020, 0.330) ✓
- grip world (rel body) = (0.100, 0.290); SE hand target (0.100, 0.288);
  **gap = (0.000, 0.002)** — pixel-perfect.

## E / N not-worse check (single-value tradeoff)

- E grip = (0.12, 0.33) vs E RHand (0.047, 0.375): gap (+0.07, -0.05). Overlay: dot on hand/front
  side. Better than OLD (0.30, 0.10) gap (0.25, -0.28).
- N grip = (0.02, 0.41) vs N RHand (0.197, 0.229): grip at center-back. N is a BACK view; weapon
  sorts BEHIND body (UpdateWeaponSortOrder order -1) so it reads as held behind — NOT inside torso
  on the wrong side, not flipped. Acceptable O1 tradeoff.

No facing puts the weapon inside the torso or on the wrong side (all grip X stay >= 0).

## Sorting / rotation: UNCHANGED (verified correct in P6).

## Files changed
- `Assets/Resources/WeaponDatabase.asset` — Warblade/Base `anchorOffset` (0.2,0.1,0) → (0.02,0.33,0).
  (1 line.) No code changed. No prefab changed.

## Verify
- Asset reloaded: anchorOffset=(0.020, 0.330). 
- read_console: 0 real errors (only the known benign play-exit teardown "Some objects were not
  cleaned up when closing the scene" — file empty, no stack, same benign msg flagged in P6/STATUS).
- Play STOPPED. Scene _Arena isDirty=False, 0 stray objects.

## Screenshots (STAGING/_process/2026-06/)
- `p6b_SE_overlay_final.png` — PRIMARY: red grip dot on SE-idle right hand (grip-in-hand proof).
- `p6b_E_overlay_final.png` — E not-worse (grip on hand/front).
- `p6b_N_overlay_final.png` — N not-worse (grip center-back, behind body).
- (intermediate: p6b_grip_overlay2.png, p6b_south_4x.png — earlier iteration on wrong sprite set.)
