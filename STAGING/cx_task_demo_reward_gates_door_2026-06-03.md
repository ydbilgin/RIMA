ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files/scenes only (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>" if needed. Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

# Amaç (Purpose)
Build the demo loop + place the real generated assets. DEMO FLOW (user-specified):
  kill mobs -> room clears -> PORTAL APPEARS (visible) but NOT yet enterable -> reward relic spawns at room center -> player COLLECTS reward -> PORTAL BECOMES ACTIVE (enterable) -> walk into portal -> next map.
Currently the door activates immediately on clear (no reward gate). We must gate the door's ACTIVATION on reward collection. Also swap the procedural placeholder sprites for the real generated ones.

# Current code (verified)
- `Assets/Scripts/Core/RoomClearVictoryTrigger.cs`: on last mob death -> `HandleRoomCleared()` -> if MapFlowManager.ActiveInstance.HasMapList: `UnlockSceneExit()` + `SpawnRewardPickup()`. `UnlockSceneExit()` (private static) finds the North `DoorTrigger`, calls `GateBehavior.Unlock(RoomType.Combat)` AND `door.SetActive(true)` (THIS immediate activation is what we must defer).
- `Assets/Scripts/Core/RewardPickup.cs`: `OnTriggerEnter2D` (Player) -> collected=true -> `RunStats.Instance.RecordRewardCollected()` -> `if (RuntimeRoomManager.Instance != null) RuntimeRoomManager.Instance.OpenDoorsAfterReward();` (RRM is null/removed -> currently a no-op) -> Destroy. THIS is the orphaned reward-gates-door hook to re-wire.
- `DoorTrigger` has `autoEnterOnOverlap=true` (walk-in) + `IsGateUnlocked()` (GateBehavior.IsOpen). `GateBehavior.Unlock()` shows the portal arch sprite after a ~0.3s coroutine.

# PART A — Place generated assets (replace procedural placeholders)
Generated (transparent, on-brand) live OUTSIDE Assets at `STAGING/imagegen/assets/`. Copy into Assets + import:
- `STAGING/imagegen/assets/portal_arch_gen.png` -> `Assets/Sprites/Environment/Portal/portal_arch_gen.png`; import Sprite(Single), PPU 64, Point, alphaIsTransparency, pivot **bottom-center (0.5,0)**.
- `STAGING/imagegen/assets/reward_relic_gen.png` -> `Assets/Sprites/Reward/reward_relic_gen.png`; Sprite/PPU64/Point/alpha, pivot center.
- `STAGING/imagegen/assets/echo_mote_gen.png` -> `Assets/Sprites/Reward/echo_mote_gen.png`; same, pivot center (no wiring).
- Archive the OLD procedural ones to `Assets/Sprites/_archive~/` (move file + .meta; do NOT delete): `Assets/Sprites/Environment/Portal/portal_arch_cyan.png`, `Assets/Sprites/Reward/reward_relic_cyan.png`, `Assets/Sprites/Reward/echo_mote.png`.

# PART B — Wire reward-gates-door (2 surgical script edits)
1. `RoomClearVictoryTrigger.cs`:
   - Add a `private static readonly System.Collections.Generic.List<DoorTrigger> pendingExitDoors = new();`
   - In `UnlockSceneExit()`: keep `GateBehavior.Unlock(...)` (and the legacy Gate.Unlock() if present) so the portal VISUAL appears on clear, but REMOVE/skip the immediate `door.SetActive(true)`. Instead `pendingExitDoors.Add(door);` for each North door. (Portal shows but not enterable yet.)
   - Add `public static void ActivateExitDoors()` that iterates pendingExitDoors, calls `door.SetActive(true)` on each non-null, then clears the list. (Called when reward collected.)
   - Keep the no-MapFlow fallback (RoomLoader.RaiseDemoComplete) path unchanged.
2. `RewardPickup.cs`:
   - In `OnTriggerEnter2D`, after `RunStats.Instance.RecordRewardCollected();`, add `RoomClearVictoryTrigger.ActivateExitDoors();` (this activates the gated portal). Keep the existing RuntimeRoomManager null-safe call too (harmless). Then Destroy as before.

# PART C — Wire sprites + save + verify
- 3 scenes (_IsoGame, _IsoGame_Map02, _IsoGame_Map03): DoorNorth GateBehavior.spriteUnlockedBase + spriteRoomCombat = `portal_arch_gen`; RoomClearVictoryTrigger.rewardSprite = `reward_relic_gen`. SAVE each.
- Compile-clean: read_console must be 0 errors (recompile happens from the 2 script edits — poll editor_state.isCompiling, then read_console).
- Runtime-verify on `_IsoGame` IF play-mode is stable (project has had D3D12 crashes — if it crashes, STOP, report edit-mode wiring done + runtime pending for Opus): Play -> kill/clear all mobs -> confirm portal arch sprite APPEARS at north edge AND DoorTrigger is NOT yet active (can't enter) -> confirm reward relic at center -> move Player onto the relic (collect) -> confirm portal DoorTrigger becomes active -> move Player into portal -> confirm next map loads. Screenshot `Assets/Screenshots/demo_reward_gate.png`.

# Notes
- profile laurethayday; if quota-limited, STOP and report so orchestrator re-dispatches with yekta. NO commit. Surgical edits only — do not refactor unrelated code.

# Output
Part A import+archive confirm; Part B script diffs (the 2 edits); Part C per-scene wiring + compile status + runtime/edit verify + screenshot. Note any BLOCKED.
