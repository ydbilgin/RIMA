# REVIEW TASK: combat_weapon weapon-mount changes (agy / Antigravity)

ACTIVE RULES: (1) think before judging (2) be surgical - only the 3 listed files (3) respond INLINE, NOT to a file (4) ASCII only.

Amac: Review the just-implemented "combat_weapon" weapon-mount changes for correctness bugs and RIMA convention issues, then give a concise findings list and a final PASS/CONCERNS verdict.

## Project root
F:/Antigravity Projeler/2d roguelite/RIMA

## Files to read and review (ONLY these three)
1. Assets/Scripts/Combat/OrientationSync.cs
2. Assets/Scripts/Systems/Combat/HandAnchorAttach.cs
3. Assets/Scripts/Combat/BasicAttack/MeleeChainBehavior.cs

## What changed (context)
- OrientationSync now drives per-direction hand offset, weapon base rotation, a per-direction flipY, and a NEW time-based procedural swing (BeginSwing + Update) that composes an arc on top of the per-direction base rotation.
- HandAnchorAttach subscribes to PlayerAttack.OnComboStep and calls OrientationSync.BeginSwing on the attack input frame; it also drives Sync() + weapon sort order per facing change in LateUpdate.
- MeleeChainBehavior (A3) defers the active hit (EmitSlashArc + ApplyMeleeHit + finisher) until attackStartup elapses, while raising the combo step immediately on the input frame.

## Design focus - check each point explicitly
(a) flipY (NOT flipX) is used for W / NW / SW directions. Confirm flipY is correct and flipX is not used.
(b) The procedural swing COMPOSES with the per-dir base rotation - no double-apply, and the swing does not fight OrientationSync.Sync(). Verify Sync() yields rotation control while a swing is active and Update() re-applies base + arc.
(c) The swing returns cleanly to the base rotation with no drift (final snap exactly equals base).
(d) Swing timing syncs with the attackStartup strike frame. Inspect: swing is started from OnComboStep (input frame) with duration CurrentSwingWindow, while the hit lands after attackStartup. Flag any mismatch between the swing arc strike moment and the deferred hit strike frame.
(e) Sort order is unaffected by the swing (weapon sortingOrder logic stays correct, not clobbered by swing).
(f) Single-sprite-per-weapon assumption preserved - no per-direction sprite bake; rotation/flip only.
(g) Null-safety on weaponRenderer / weaponTransform (and orientationSync, playerController, weaponDatabase where relevant).
(h) RIMA conventions: correct namespaces (RIMA / RIMA.Combat), no speculative/dead code, minimal surgical change, no banned patterns.

## Additional correctness checks
- BeginSwing duration source: _playerAttack.CurrentSwingWindow. If it is zero/negative, BeginSwing early-returns; confirm graceful behavior (no stuck flip / no missing rotation).
- Update() runs every frame but early-returns when not swinging - confirm no per-frame cost concern and no race with LateUpdate Sync().
- VectorToDir8 octant mapping vs weaponRotations[] / handOffsets[] index order (S=0..SW=7). Confirm flipY dirs (W,NW,SW) and behindBody dirs (N,NE,NW) match the intended octants.
- HandleComboStep calls Sync(dir) then BeginSwing(dir,...): after BeginSwing sets _swinging=true, does the immediately-prior Sync() base-rotation write get correctly overridden by Update() next frame? Any one-frame visual glitch?

## Output format (INLINE, ASCII only)
- Numbered findings list (each: file:line-ish, severity LOW/MED/HIGH, issue, suggested fix).
- Note explicitly any of (a)-(h) that PASS with no issue.
- Final line: VERDICT: PASS  or  VERDICT: CONCERNS - <one-line summary>.

Respond INLINE in your reply. Do NOT write to a file.
