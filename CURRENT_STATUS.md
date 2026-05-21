# CURRENT_STATUS

## LIVE: Top-Down + Fake-Iso PIVOT LOCKED (2026-05-21 S97 LATE NIGHT 2)

RIMA is now top-down + fake-iso, using the Children of Morta / Death Trash model. The iso experiment is closed as production direction. Active baseline target: clean top-down game structure, top-down painter defaults, and new top-down tile generation.

## ARCHIVED: Iso Experiment

The 3-sprint iso experiment is archived to `_archive/` locations. Lessons learned: AI generation and strict iso tile architecture mismatched RIMA production needs; wall_pack_v3 hero pieces can remain as decoration overlays only; new tilemap wall generation is required for the top-down baseline.

## NEXT SESSION

1. Open `Assets/Scenes/Demo/TopDownTest_Map1.unity`
2. First wall + floor PixelLab gen dispatch: top-down `create_tiles_pro`
3. Painter usage test in TopDown mode
4. Combat test: Warblade + 1 mob top-down

## Tools Active

PixelLab (top-down focus), Codex imagegen, Painter, UnityMCP.

## Known

`wall_pack_v3` hero pieces remain decoration overlays: archway, large column. Tilemap wall requires new generation.

## Budget

~2,400/5,000 PixelLab. Top-down baseline estimate: ~80-120 generations.

## Reference

Children of Morta + Death Trash + Tunic + user screenshot.
