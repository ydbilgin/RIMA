# STATUS
Ronin LIVE with caveats

Caveats:
- Unity batchmode compile could not run because this project is already open in another Unity instance.
- `dotnet build .\Assembly-CSharp.csproj -v:minimal` passes with 0 errors.
- Sakura Veil is implemented as a playable placeholder deflect/counter window using nearby enemy overlap and trigger entry, not yet a true incoming-damage parry hook.
- NLM returned a newer Ronin canon where Draw Tension gains while moving and drains after idle; this implementation follows the task contract instead: idle gain, movement drain.

# Files added
- `Assets/Scripts/Combat/Classes/Ronin/RoninController.cs`
- `Assets/Scripts/Combat/Classes/Ronin/TensionSystem.cs`
- `Assets/Scripts/Combat/Classes/Ronin/Skills/RoninQuickdraw.cs`
- `Assets/Scripts/Combat/Classes/Ronin/Skills/RoninIaidoStance.cs`
- `Assets/Scripts/Combat/Classes/Ronin/Skills/RoninFinalDraw.cs`
- `Assets/Scripts/Combat/Classes/Ronin/Skills/RoninSakuraVeil.cs`
- `Assets/Data/Combat/Profiles/Ronin_BasicAttackProfile.asset`
- `Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Ronin.asset`
- `Assets/Data/Skills/Ronin/ronin_quickdraw.asset`
- `Assets/Data/Skills/Ronin/ronin_iaido_stance.asset`
- `Assets/Data/Skills/Ronin/ronin_final_draw.asset`
- `Assets/Data/Skills/Ronin/ronin_sakura_veil.asset`

# Files modified
- `Assets/Scripts/Combat/BasicAttack/BasicAttackProfile.cs` lines 9-13, 75-79: added `IaidoStance` behavior type mapped to melee-chain execution.
- `Assets/Scripts/Combat/BasicAttack/MeleeChainBehavior.cs` lines 54-59: Warblade beat-3 hook triggers Ronin Quickdraw echo.
- `Assets/Scripts/CrossClass/CrossClassSkillManager.cs` lines 123-134, 162-171: Ronin Quickdraw echo placeholder registry/runtime trigger.
- `Assets/Scripts/Player/PlayerAttack.cs` lines 86-98: runtime basic attack profile setter for class switching.
- `Assets/Scripts/Skills/Base/SkillBase.cs` lines 83-88: Ronin skills resolve `TensionSystem`.
- `Assets/Scripts/Systems/PlayerClassManager.cs` lines 68-82, 92-130, 176-220: Ronin primary/secondary wiring, `SwitchClass`, profile assignment.
- `Assets/Scripts/UI/HUDController.cs` lines 69-91, 151-174, 256-271, 377-387: generic resource subscription and Ronin Tension support.
- `Assets/Scripts/UI/RimaUITheme.cs` lines 69-102: Ronin cyan-violet resource color and `TENSION` label.
- `Assets/Scripts/UI/SkillBarUI.cs` lines 16-19, 70-77, 243-305: Ronin controller slot display support.

# Gap inventory (Warblade/Elem/Ranger/Shadowblade)
- Warblade: 100%. BasicAttackProfile exists, controller exists, 14 skill scripts exist, `RageSystem` exists, PlayerClassManager wires primary Warblade.
- Elementalist: 100%. BasicAttackProfile exists, controller exists with element-state handling, 17 skill/visual scripts exist, `ManaSystem` exists, PlayerClassManager wires primary and secondary Elementalist.
- Ranger: 100%. BasicAttackProfile exists, controller exists, 22 skill scripts exist, `FocusSystem` exists, PlayerClassManager wires primary and secondary Ranger.
- Shadowblade: 80%. BasicAttackProfile exists, controller exists, 24 skill scripts exist, `EnergySystem` exists, PlayerClassManager wires primary and secondary Shadowblade. Gap: `ComboPointSystem` exists but is not wired by PlayerClassManager when Shadowblade is selected.

# Tension resource validation
Manual in-editor plan:
- Add/select Ronin via `PlayerClassManager.SwitchClass(ClassType.Ronin)`.
- Confirm player receives `TensionSystem` and `RoninController`, and `PlayerAttack` receives `BasicAttackProfile_Ronin`.
- Stand still for 5 seconds: Tension should rise by about 5.
- Move for 5 seconds from nonzero Tension: Tension should drain by about 10.
- Use Iaido Stance: player should be rooted for 0.8s and Tension should gain at 5/sec during stance.
- Use Quickdraw at 20+ Tension: Tension spends 20 and refunds 10 when the line hit connects.
- Use Final Draw at nonzero Tension: spends all current Tension and scales cone damage by amount spent.
- Use Sakura Veil near an enemy or trigger collider during the 0.4s window: refunds 30 Tension, white flash, 50ms hitstop.

# Next steps for Day 2-5 of stress test
- Replace Sakura Veil placeholder overlap detection with an incoming-hit parry event from `Health`/damage pipeline.
- Add Ronin animator/controller asset under `Resources/Characters/Ronin/Ronin.controller`; current PlayerClassManager will warn if missing.
- Add Tension-specific HUD polish: cyan-violet track, max pulse, optional stance/deflect flash.
- Decide whether task-contract Tension or NLM latest Draw Tension canon is authoritative before tuning numbers.
- Add focused EditMode tests for `TensionSystem` idle gain, movement drain, spend/refund, and `SwitchClass(ClassType.Ronin)` component/profile wiring.
- Promote Warblade beat-3 -> Ronin Quickdraw echo from placeholder VFX/runtime scan into a data-driven cross-class registry entry if cross-class echoes become persistent build content.

# Verification
- `dotnet build .\Assembly-CSharp.csproj -v:minimal` passed with 0 warnings and 0 errors after adding Ronin compile entries to the generated local csproj.
- `dotnet test .\RIMA.Tests.EditMode.csproj --no-restore -v:minimal` exited 0.
- Unity batchmode compile was attempted but aborted because another Unity instance has the project open.
