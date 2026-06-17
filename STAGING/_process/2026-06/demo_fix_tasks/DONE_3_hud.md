Evidence: STAGING/_process/2026-06/demo_fix_tasks/screenshots/TASK_3_hud_readability_runtime.png
Changed: Assets/Scripts/UI/HUDController.cs; Assets/Scripts/UI/SkillBarUI.cs
3A: HPTrack live 212x16; ResourceGroup live 160x10; slots remain LMB/RMB 56 and Q/E/R/F 44; key/CD fonts 14/12 and 18/16.
3B: Low HP now <20%, 0.90s pulse, alpha 0.12-0.18; hit flash capped 0.22; vignette asset center alpha=0.
SDF: Runtime TMP uses LiberationSans SDF Material; screenshot inspected, no obvious blur from resize.
Console: Unity runtime pre-stop 0 error/warning; post-stop cleanup error appeared: "Some objects were not cleaned up when closing the scene."
Build: dotnet build RIMA.Runtime.csproj --no-restore passed 0 warnings/0 errors.
Risk: Full low-HP state was code/asset verified, not force-driven in play without adding temporary test files; Assembly-CSharp.csproj still has unrelated missing CliffPlacementRules.cs.
