# A0 FIX — remaining skills hit-layer "Default" -> "Enemy" — CX

ACTIVE RULES: (1) think before coding (2) min code — change ONLY the enemy-detection LayerMask on the matching line, nothing else (3) surgical — 4 listed files only (4) BLOCKED/flag if an occurrence is NOT enemy-detection.

NLM ACCESS: not needed.

## Bug (confirmed by a code audit)
Enemies live on physics layer **"Enemy"** (layer 11). These 4 skills still detect enemies with `LayerMask.GetMask("Default")` (layer 0 = empty) -> they silently hit 0 enemies. (Earlier the 7 Warblade skills were already fixed; these are the remaining ones in other classes.)

## Fix — in EACH file, on the enemy-detection Physics2D call only
Find the `Physics2D.OverlapCircleAll(...)` / `CircleCastAll(...)` / `OverlapCircle(...)` call that passes `LayerMask.GetMask("Default")` for ENEMY detection and change ONLY that argument to `LayerMask.GetMask("Enemy")`. Match the string `LayerMask.GetMask("Default")` within each file.

Files (audit-reported lines, but match by string — don't trust exact numbers):
1. `Assets/Scripts/Skills/**/CripplingBlow.cs` (~line 54)
2. `Assets/Scripts/Skills/**/DeepWound.cs` (~line 24)
3. `Assets/Scripts/Skills/**/IronCounter.cs` (~line 69)
4. `Assets/Scripts/Skills/**/Blink.cs` (~line 44)

⚠️ **GUARD:** Before changing each, confirm the call is for ENEMY DAMAGE/HIT detection (an OverlapCircle/CircleCast that then applies damage / reads Health). If any `GetMask("Default")` occurrence is for something else (wall/obstacle/teleport-clearance — e.g. Blink might check geometry), DO NOT change it — flag it in your report instead. Blink especially: verify it's hitting enemies, not checking teleport clearance.

## Verify
`dotnet build "RIMA.Runtime.csproj" -nologo -clp:ErrorsOnly` -> 0 errors. Report command + result.

## Report
Per file: old line -> new line (file:line), OR "SKIPPED (not enemy-detection: <reason>)". Then build result.
