# CX REVIEW — BLOCK C C1-C3 binding registry + player input repoint (regression audit)

ACTIVE RULES: (1) think before reviewing (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: query NLM if you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING. Verdict PASS/FAIL + file:line. Do NOT edit files.

## Scope — commit 5fc4a51f (4 files)
- `Assets/Scripts/Core/KeyBindManager.cs` — rewritten static class: GameAction enum + Defaults dict + JSON PlayerPrefs
  persistence + reserved(Esc/Tab)/duplicate guard + OnBindingsChanged + RebuildBindings + PathToLabel + legacy slot shim.
- `Assets/Scripts/Player/PlayerController.cs` — move/dash now built from registry; BuildInputActions()/RebuildInput();
  OnEnable subscribes KeyBindManager.OnBindingsChanged, OnDisable unsubscribes.
- `Assets/Scripts/Player/PlayerAttack.cs` — attack/secondary/riftbreak from registry; RebuildInputActions() disposes+rebuilds;
  OnEnable/OnDisable subscribe/unsubscribe.
- `Assets/Scripts/UI/SkillBarUI.cs` — SlotCount 7→6, registry-driven labels {LMB,RMB,Q,E,R,F}, RefreshKeyLabels on change.

## Questions (PASS/FAIL + file:line)
1. **Regression:** Do move (WASD composite + gamepad leftStick) and dash still bind identically to before? Any behavior
   change vs the old hardcoded paths? Gamepad bindings preserved?
2. **InputAction lifecycle:** In PlayerController.RebuildInput and PlayerAttack.RebuildInputActions — is Dispose()+recreate
   safe while enabled? Any double-enable, missing re-hook of dashAction.performed, or use-after-dispose?
3. **Event leak:** Is KeyBindManager.OnBindingsChanged unsubscribed on every disable/destroy path for PlayerController,
   PlayerAttack, SkillBarUI? PlayerAttack self-heals InputActions in Update — does that interact badly with RebuildInputActions?
4. **Persistence/guard:** JSON round-trip via JsonUtility correct? Does the duplicate guard wrongly block a legit rebind
   (e.g. binding Skill1 to its own current key)? Does TrySetBinding reverting to default correctly drop the override?
5. **Back-compat:** The 5 skill controllers call KeyBindManager.GetBinding(int i) (i=0..3). Does the shim map 0..3 →
   Skill1..Skill4 with the same default keys (q/e/r/f) as before? Any off-by-one?
6. **SkillBarUI:** SlotCount 6 vs GetActiveSlotCount Min(ctrl.SlotCount, 6) — any class that exposed a 7th slot now broken?
   Width/layout for 6 slots correct?

Be terse. Cite file:line. If FAIL, give the exact minimal fix.
