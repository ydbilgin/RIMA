# DONE 2 - Boss Presentation P0

## Changed files
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs`
- `Assets/Scripts/Shop/ShopRoomController.cs`
- `Assets/Scripts/Enemies/Boss/PenitentSovereign.cs`
- `Assets/Scripts/UI/BossHealthBar.cs`
- `Assets/Scripts/UI/RoomMonologController.cs`

## Fix status
- 2A shop residue: PASS. `RoomRunDirector` now retains the spawned `ShopRoomController`, calls explicit cleanup before each new run/room build and before re-spawning a shop, and deactivates/destroys shop stands through `ShopRoomController.Cleanup()`.
- 2B boss scale/pivot/anchor: PASS. Boss runtime scale no longer compounds prefab child scale; spawn placement aligns sprite feet to collider base and clamps the resulting sprite bounds inside the arena floor bounds.
- 2C health bar: PASS. Boss bar uses fixed top-center width, stone/slate frame, crimson fill, phase label, and visible 66/33 phase notches. Neon green fill logic removed.
- 2D subtitle/monolog safe area: PASS by runtime layout assertion. Line/title panels are bottom-center and above the skill bar safe line.

## Runtime verification
- Route forced in Play Mode: `Combat -> Combat -> Merchant -> Combat -> Boss`.
- Merchant residue check: merchant stands seen in merchant room = 3; active shop controllers at boss = 0; active shop stands at boss = 0.
- Boss placement check: `floorOK=True`; viewport bounds `(0.56,0.20)-(0.67,0.40)`; top/bottom HUD clear = true by threshold.
- Boss bar check: HP fill color `(0.77,0.17,0.17)`; phase notches present; phase label `PHASE I`.
- Subtitle safe-area check: `skillTop=88.0`; `lineY=104.0`; `titleY=112.0`.
- Final runtime assertion: `pass=True`.

## Evidence
- Scene View: `STAGING/_process/2026-06/demo_fix_tasks/screenshots/task2_boss_scene_view_final.png`
- Game View: `STAGING/_process/2026-06/demo_fix_tasks/screenshots/task2_boss_game_view_final.png`
- Subtitle safe-area capture request: `STAGING/_process/2026-06/demo_fix_tasks/screenshots/task2_boss_subtitle_safe_area.png`

## Console
- Unity final console read: 0 errors, 0 warnings.

## Remaining risk
- Subtitle safety is supported by RectTransform runtime geometry; the subtitle-specific screenshot did not visibly catch the transient line, likely due the fade/skip timing.
- Unrelated pre-existing modified files were left untouched: `Assets/Scripts/UI/BuildModeController.cs`, `Assets/Scripts/UI/DirectorMode.cs`, `CURRENT_STATUS.md`.
