ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files/scenes only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç (Purpose)
MVP "door -> VERTICAL PORTAL" swap. Decision (locked by Opus, see STAGING/REWARD_PORTAL_DECISION_2026-06-02.md): the room-exit is a VERTICAL cyan rift the player WALKS INTO (auto-enter on overlap), replacing the old press-G door. Reuse the PROVEN clear->exit->next-map chain — do NOT rebuild it. Real PixelLab portal art is GATED (later together-session); use a code-generated PLACEHOLDER vertical cyan rift sprite now. The full canon Portal.cs / FanLayoutSolver / preview system is a LATER P2/P3 effort — NOT this task.

# Verified live mechanism (do NOT break)
- `RoomClearVictoryTrigger` (Assets/Scripts/Core/RoomClearVictoryTrigger.cs): on last enemy death -> `HandleRoomCleared()` -> if MapFlowManager.ActiveInstance.HasMapList -> `UnlockSceneExit()` (finds the North `DoorTrigger`, calls `GateBehavior.Unlock(RoomType.Combat)` + `door.SetActive(true)`) + spawns reward pickup.
- `DoorTrigger` (Assets/Scripts/Core/DoorTrigger.cs) on `DoorNorth`: player in range + press **G** + `IsGateUnlocked()` (GateBehavior.IsOpen) -> `TriggerTransition()` -> (RuntimeRoomManager.Instance==null, so) `MapFlowManager.ActiveInstance.GoToNextMap()`.
- `GateBehavior` (Assets/Scripts/Core/GateBehavior.cs) drives DoorNorth's SpriteRenderer: Hidden -> (Unlock) -> Open shows `GetRoomTypeSprite` (falls back to `spriteUnlockedBase`).
- Live scenes: `_IsoGame.unity`, `_IsoGame_Map02.unity`, `_IsoGame_Map03.unity`. Each has `DoorNorth` (active, ~pos (-3, 8.75), comps: BoxCollider2D + DoorTrigger + SpriteRenderer + GateBehavior) at the north (back) island edge. North edge has NO cliffs (clean) — good for a standing rift.

# Implementation (surgical)
## 1. DoorTrigger.cs — add auto-enter (the ONLY script edit)
- Add `[SerializeField] private bool autoEnterOnOverlap = false;` (default false → all existing doors unchanged).
- In `OnTriggerEnter2D`: if `isActive && !triggered && other.CompareTag("Player")`:
    - if `autoEnterOnOverlap`: if `IsGateUnlocked()` → set `triggered=true; col.enabled=false; ClearPlayerRange(); TriggerTransition();` (walk-in, NO prompt, NO press-G). If gate not yet unlocked, just ignore (don't show prompt).
    - else (current behavior): `playerInRange=true; ShowPrompt();` (press-G unchanged).
- Do NOT change the press-G `Update()` path (keep it for autoEnter=false). Keep everything else identical. Compile-clean (read_console after).

## 2. Placeholder vertical cyan rift sprite (code-gen asset, once)
- Generate a PNG `Assets/Sprites/Environment/Portal/portal_vertical_placeholder.png`: a TALL rift, e.g. 64x128 px. Look: dark transparent edges, a glowing cyan (#00FFCC) vertical jagged crack down the center, brighter core, soft falloff. (Texture2D + EncodeToPNG; transparent background.) Import as: Sprite (Single), PPU64, Point filter, alphaIsTransparency=true, **pivot = Bottom-Center (0.5, 0)**.

## 3. Configure DoorNorth in all 3 scenes
For each scene, on the `DoorNorth` GameObject:
- Set `DoorTrigger.autoEnterOnOverlap = true` (serialized).
- Assign the placeholder rift sprite to `GateBehavior.spriteUnlockedBase` (and `spriteRoomCombat`) so the Open state shows the vertical rift. (Use SerializedObject so it persists.)
- The SpriteRenderer: ensure sortingLayer/order matches the scene's entity Y-sort convention (so the player sorts in front when approaching from below — Y-sort by bottom pivot). If the scene uses Camera custom-axis Y-sort (per project), just ensure the rift SpriteRenderer is on the right sorting layer ("Entities" or whatever the player uses) and not "Floor". Keep its transform at the door pos but make sure the rift's BOTTOM sits at the edge ground (bottom-center pivot handles this).
- Optionally widen the BoxCollider2D trigger slightly (e.g. size ~2x2) so walk-in is reliable.
- SAVE the scene.

## 4. Verify
- Compile-clean (read_console = 0 errors) FIRST.
- Then runtime-verify on `_IsoGame` IF play-mode is stable (note: project has had occasional D3D12 GPU crashes — if play-mode crashes, STOP, report edit-mode state, and say runtime-verify is pending for Opus): Play → kill all mobs (or use any debug clear) → confirm the vertical rift appears at the north edge (GateBehavior Open) + DoorTrigger active → walk the player INTO it → confirm `MapFlowManager` RoomReached increments / next map loads. Screenshot the cleared-room-with-portal to `Assets/Screenshots/portal_vertical_mvp.png`.
- If runtime is skipped due to crash risk, at least confirm in EDIT mode: autoEnter serialized true, rift sprite assigned, scene saved.

# Notes
- NO other scripts touched. NO commit. Do NOT touch the cliffs / CliffRing. Keep press-G working for any non-portal doors (autoEnter default false).
- Idle shimmer animation + real PixelLab rift art = deferred to the together-session (note in your output, do NOT build a shimmer system now).

# Output
- DoorTrigger.cs diff summary (autoEnter addition).
- Placeholder sprite path + import settings confirm.
- Per-scene: DoorNorth autoEnter=true confirm + rift sprite assigned + saved.
- Compile status (console errors=?). Runtime-verify result OR "skipped (crash risk) — edit-mode confirmed". Screenshot path if taken.
