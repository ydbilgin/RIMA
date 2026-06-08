# COMBAT RUN — ISSUES + FIX PLAN (2026-06-09)

Live-playtest complaints (combat dungeon, NOT chamber). Root-caused by Explore agent. Fix round = AFTER the chamber prompt fix + push.

## 1. Softlock: cleared room but stuck after passing gate
`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs`
- `RoomClearSequence()` coroutine can STALL: reward-collection wait loop (~770-773 `while !WasCollected`) and DraftManager wait (~782-785) have NO timeout → if reward never collected / draft never closes, coroutine never reaches `OpenExitDoors()` (~787).
- `OpenExitDoors()` early-returns if `lifecycle.MarkDoorsOpened()` is false (state not RewardTaken) (~894-895) — no fallback.
- `EnsureAtLeastOneExitDoor()` spawns a STATIC fallback door (~940-942) but it does NOT wire into `TryEnterDoor()` → walking into it doesn't advance the run.
- FIX: add timeouts to the reward/draft waits (auto-proceed after N seconds), guarantee `OpenExitDoors` runs even if state path was skipped, and make the fallback door call `TryEnterDoor`/`AdvanceTo` so it actually progresses.

## 2. Gate hitboxes too big
`RoomRunDirector.cs:966` — exit-door `BoxCollider2D.size = (1.2, 1.0)` (player collider is 0.46×0.34 → gate is ~2.6-3.5× player width). isTrigger set at ConfigureExitDoors (~1098).
- FIX: shrink the gate collider to ~(0.7, 0.7) (footprint-sized), keep it a trigger for the [G] prompt; don't let it body-block a large area.

## 3. Rooms too small
`Assets/Data/Rooms/DemoRoomBank.asset` combatRooms[] includes:
- [0] combat_large_teardrop_01 (24×18 — good)
- [1] Combat_Medium_01 (12×8 — marginal)
- Library has Combat_Small_01 (8×6 — very tight). `RoomBankSO.Pick()` random → playtest can hit a small room.
- FIX: remove the small/medium rooms from the active combat bank OR enlarge them; ensure every combatRooms[] entry is adequately sized (≥ ~20×14). Keep variety via layout, not by shrinking.

## 4. Camera zoom inconsistent (user wants FIXED)
`RoomRunDirector.cs:233-255` `FitCameraToRoom` computes orthoSize PER-ROOM from bounds (Small≈4.25, Medium≈5.25, Large≈10.25) + padding 1.25 (~96). Chamber is fixed 4.2 (`ChamberSelectBootstrap.cs:41`).
- User: "karakterin yakınlığı aynı değil — sabit yakınlıkta olmalı."
- FIX: replace fit-to-room with a FIXED combat orthoSize (follow camera handles large rooms by scrolling). Pick one consistent value (~5.0-5.5) so zoom matches across rooms AND is close to the chamber feel. Expose as a tunable. (Camera already follows the player, so a fixed zoom + follow = consistent "character closeness" everywhere.)

## Sequencing
Do this AFTER the chamber prompt-render fix is committed+pushed (so ChatGPT reviews the current state and can independently confirm these). Then one combat-run fix round (RoomRunDirector.cs + DemoRoomBank.asset + _Arena camera). Cross-reference with ChatGPT's review before implementing #1 (softlock) — get the exact stall path confirmed.
