# Review: A1 Sundered/Broken visual tell — CX

ACTIVE RULES: (1) think before judging (2) real issues only, file:line + concrete fix (3) reviewer not writer — don't rewrite (4) BLOCKED if can't read.

NLM ACCESS: not needed.

## Context
New code makes the "Sundered Beat" signature visible: when an enemy gains Broken (red crack) or Sundered (orange-red split + shard burst + SFX) state, a crack overlay + burst fire. Trigger = a new event on the existing SkillStateTracker; visual lazily attached via AddComponent.

## Files
- `Assets/Scripts/Combat/Juice/BrokenStateVisual.cs` (new)
- `Assets/Scripts/Skills/SkillStateTracker.cs` (added `OnStateEntered`/`OnStateExpired`, lazy-attach in `Apply`)
- `Assets/Scripts/Skills/Elementalist/ElementalistRuntimeVisuals.cs` (added `GetCrackSprite()` procedural placeholder)

## Review focus — PASS/FAIL + file:line
1. **Double-fire guard / ordering:** `Apply` calls `BrokenStateVisual.Ensure()` THEN raises `OnStateEntered`; the component subscribes in `OnEnable` (synchronous during AddComponent) and relies on receiving that same event without double-firing the shard burst (`firstEnter` guard + `activeKey==null` guard). Verify this ordering holds and a re-apply / stack-up does NOT double-burst.
2. **Event lifecycle / leak:** does `BrokenStateVisual` UNSUBSCRIBE from `SkillStateTracker.OnStateEntered/OnStateExpired` in OnDestroy/OnDisable? Static or instance event? Any leak when the enemy dies mid-effect.
3. **Existing readers unaffected:** `SkillStateTracker.Apply` is read by DeathBlow/Shadowblade/Ranger/HeatGauge etc. — confirm the additions are purely additive (no behavior change to Has()/stacks/expiry).
4. **Depth rule:** overlay uses same sortingLayer/order as body + spriteSortPoint=Pivot, NO manual sortingOrder bump (Custom-Axis Y-sort). Confirm.
5. **Per-frame cost:** the overlay tracks enemy bounds/flipX each frame — any per-frame allocation (new Vector3[], GetComponent in Update, etc.)?
6. **Null-safety:** enemy destroyed while Broken; tracker null; renderer null.
7. **No cyan** in the tell (cyan reserved for seal/player). Confirm colors are red / orange-red.
8. **Sundered vs Broken precedence** sane (Sundered supersedes; demotes to Broken on Sundered expiry if still present).

## Output
Top line `STATUS: PASS` or `STATUS: FAIL`, then findings. Tight.
