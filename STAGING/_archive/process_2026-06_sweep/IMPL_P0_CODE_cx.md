ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

Amaç (purpose): Implement the P0 *code-only* fixes for the RIMA playable-demo pass (the run-end soft-lock, class-select gate, elite teleport, reward fallback, finisher-prep lookup). These were just reviewed by you and approved with corrections — this is the implementation of YOUR review.

⚠️ HARD CONSTRAINT: **DO NOT touch Unity.** No UnityMCP tools, no play mode, no scene/prefab edits, no asset reimport, no `refresh`/recompile trigger. **Edit ONLY the .cs files below.** The orchestrator owns all Unity/scene/prefab work and will recompile + verify afterward. Keep edits surgical; do not refactor unrelated code. Do NOT commit (commit is GATED).

### 1. Assets/Scripts/Core/DemoCompleteOverlay.cs — fix PLAY AGAIN soft-lock + timeScale leak
- `Restart()` (≈line 113-117): currently `Time.timeScale=1f; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);`. Change to: `Time.timeScale = 1f; MapFlowManager.Instance?.ResetRun(); SceneManager.LoadScene("MainMenu");` (PLAY-AGAIN after victory returns to the clean MainMenu→CharacterSelect→_IsoGame flow which already does StartNewRun/ResetRun).
- Add an `OnDestroy()` method that sets `Time.timeScale = 1f;` (the overlay sets timeScale=0 in Build(); if it is destroyed without Restart/LoadMainMenu the freeze leaks). Guard: only reset if still 0 is unnecessary — just set 1f.
- `LoadMainMenu()` already loads MainMenu with timeScale=1 — also add `MapFlowManager.Instance?.ResetRun();` before the load for consistency.

### 2. Assets/Scripts/Core/DeathScreenManager.cs — same soft-lock on TRY AGAIN
- `RestartRun()` (≈line 141-146): currently reloads active buildIndex. Change to a clean fresh-run-in-place: `Time.timeScale = 1f; MapFlowManager.Instance?.ResetRun(); RunStats.Instance?.StartNewRun(); SceneManager.LoadScene("_IsoGame");` (selected class persists via static PlayerClassManager.SelectedClass). If `RunStats.Instance` may be null, the `?.` guard is enough. Keep the `Application.isPlaying` guard.
- `LoadMainMenu()` (≈line 148-153): add `MapFlowManager.Instance?.ResetRun();` before the load.

### 3. Assets/Scripts/UI/CharacterSelectController.cs — gate locked classes
- This component is LIVE in Assets/Scenes/UI/CharacterSelect.unity and currently exposes all 10 classes with no unlock gate, so a player could launch a locked/stub class (invisible player, disabled attack). Add the SAME unlock rule used by CharacterSelectScreen.cs (`IsUnlocked`: Warblade, Elementalist, Ranger, Shadowblade unlocked; all others locked).
- Find where it builds/selects classes (e.g. `GetDefaultClasses()` ≈line 135-147 and the start/confirm handler). Implement: locked classes are either not selectable or the START button is disabled + shows a "locked" state for them. Mirror CharacterSelectScreen's `IsUnlocked` logic exactly (copy the static predicate). Do NOT change CharacterSelectScreen.cs. Minimal change: block `OnStart`/launch when the chosen class is locked, and visually mark locked cards (reuse whatever lock/disabled affordance the file already has; if none, set the start action to no-op + log for locked).
- If the cleanest fix is to make CharacterSelectController defer to the same gate, do that; keep it minimal.

### 4. Assets/Scripts/Enemies/EliteAffix.cs — teleport wall-escape (≈line 179-185)
- `Teleport()` currently does `transform.position = newPos;` directly (has a TODO). Before assigning, validate `newPos` is inside playable space: use `Physics2D.OverlapCircle(newPos, <smallRadius ~0.3f>, <wall/obstacle LayerMask>)`. If blocked (overlap with wall/obstacle) or out of bounds, do NOT teleport (keep current position) — optionally retry a couple of candidate offsets, else abort. Use the same wall/obstacle layer the project uses for boundaries (check how other enemies detect walls; if unclear, use `LayerMask.GetMask("Default")` for the EdgeCollider boundary OR the obstacle layer — pick what matches existing collision and note your choice). Goal: an elite can never teleport outside the arena and become unkillable/blocking room-clear.

### 5. Assets/Scripts/Core/RoomClearVictoryTrigger.cs — reward sprite Resources fallback (≈line 139-153)
- `ResolveRewardSprite()` currently uses an `#if UNITY_EDITOR` AssetDatabase path (works in editor only). Add a runtime fallback: if the serialized `rewardSprite` is null, try `Resources.Load<Sprite>(...)` for a sensible reward sprite path (search Assets/Resources/UI/RIMA for an existing reward/relic sprite, e.g. a reward_relic or RIMA_UI_Node_Chest; pick one that EXISTS and note it). Keep the serialized `rewardSprite` as the primary source. Do NOT remove the editor path; just ensure non-editor builds + scenes without a serialized sprite still get a sprite.

### 6. Assets/Scripts/Combat/Beat3CommitTrigger.cs — root CombatHandler lookup (≈line 17) [P1-prep, cheap]
- Currently `animator.GetComponent<CombatHandler>()`. PlayerAnimator's Animator lives on a CHILD while CombatHandler is on the player root. Change to `animator.GetComponentInParent<CombatHandler>()` (or `animator.transform.root.GetComponentInChildren<CombatHandler>()` if parent lookup is unreliable). This unblocks the P1 finisher; it is a safe 1-line change.

OUTPUT: For each file, a short summary of the exact change (and the chosen LayerMask in #4 / reward sprite path in #5). Report any item you hit as BLOCKED with the reason. Remember: edit .cs only, do NOT touch Unity, do NOT commit.
