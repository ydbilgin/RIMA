# P0 FIX — Warblade active skills hit-layer "Default" -> "Enemy" — CX

ACTIVE RULES: (1) think before coding (2) min code — change ONLY the layer-mask string on the specified detection lines, nothing else (3) surgical — 7 listed files only (4) BLOCKED if a file/string isn't found as described.

NLM ACCESS: not needed.

## Bug (confirmed)
A layer refactor moved enemies onto physics layer **"Enemy"** (BaseMobBehavior.cs:82-83 sets `gameObject.layer = LayerMask.NameToLayer("Enemy")`). But 7 Warblade active skills still detect enemies with `LayerMask.GetMask("Default")` (layer 0 = now empty) -> they silently hit 0 enemies. Basic attack + Earthsplitter use no-mask / tag filtering and still work, which hid the bug.

## Fix — in EACH file below, replace the enemy-detection LayerMask
Find the `Physics2D.OverlapCircleAll(...)` / `Physics2D.CircleCastAll(...)` call that passes `LayerMask.GetMask("Default")` and change ONLY that argument to `LayerMask.GetMask("Enemy")`. Match by the string `LayerMask.GetMask("Default")` within each file (do not trust exact line numbers; grep within the file). There should be exactly ONE such occurrence per file (if more or none, report it).

Files:
1. `Assets/Scripts/Skills/Warblade/IronCharge.cs`
2. `Assets/Scripts/Skills/Warblade/GravityCleave.cs`
3. `Assets/Scripts/Skills/Warblade/Cleave.cs`
4. `Assets/Scripts/Skills/Warblade/BladeRush.cs`
5. `Assets/Scripts/Skills/Warblade/DeathBlow.cs`
6. `Assets/Scripts/Skills/Warblade/WarStomp.cs`
7. `Assets/Scripts/Skills/Warblade/SunderMark.cs`

Do NOT touch Earthsplitter.cs, basic attack, passives, or any other file.

## Verify
Run `dotnet build "RIMA.Runtime.csproj" -nologo -clp:ErrorsOnly` from repo root -> must be 0 errors. Report the command + result.

## Report
For each of the 7 files: the old line -> new line (file:line). Then the build result. Flag any file where the `"Default"` mask was NOT found exactly once.
