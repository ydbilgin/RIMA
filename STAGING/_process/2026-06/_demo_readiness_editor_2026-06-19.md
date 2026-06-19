# Demo Readiness — Editor Tools Re-Verify (edit-mode) — 2026-06-19

Runs AFTER the in-play agent finished; Unity free. SOLE Unity driver. Read-only intent: all mutations restored.

## In-Play phase (BOOT → run start)
- BOOT via full-flow: MainMenu(Button_Basla) → CharacterSelect(Warblade Hit → StartButton) → _Arena → opening draft active (IsDraftActive=True, timeScale=0) → PickCard(0).
- Post-pick: scene=_Arena, timeScale=1, IsDraftActive=False, PlayerController live at (5.28,4.10,0), RunStats present. Run started cleanly (no T9/T7 freeze).
- Screenshot: Assets/Screenshots/Playtest_2026-06-19/inplay_arena_run.png
- Console (in-play): 0 errors / 0 warnings.

## Editor Tools phase
1. MAP DESIGNER — execute_menu_item "RIMA/Map Designer" → UnifiedMapDesigner window open (1 instance), _roomTemplates count=26. PASS.
2. CLIFF-GENERATE — CliffAutoPlacer on 'CliffRing' (floorTilemap=Ground, cliffTilemap=CliffTilemap). Cleared cliff to 0, Regenerate() → 22 tiles (0→22), LastGeneratedCount=22 (deterministic match). Ground floor=128 tiles. PASS.
3. HAND-PAINT — AddManualPainted((1,-1,0)) registered cell in painted HashSet (count 1); Regenerate folded it in → cliff 22→23 (delta +1), LastGeneratedCount=23, painted cell survived geometry reconcile. PASS.
   - Note: AddManualPainted registers the override; tilemap stamp happens on Regenerate (auto-geometry pass folds painted cells). A far/non-adjacent painted cell is geometry-rejected (correct behavior).

## Restore (no-leak)
- ClearManualPainted + Regenerate → cliff back to 22, LastGeneratedCount=22, paintedSet empty. Scene NOT saved. Pre-test baseline restored.

## Console
- Both phases: 0 errors / 0 warnings (read_console Error+Warning).

Screenshots:
- Assets/Screenshots/Playtest_2026-06-19/inplay_arena_run.png
- Assets/Screenshots/Playtest_2026-06-19/editor_cliff_island_room.png
