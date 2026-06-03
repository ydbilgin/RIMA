# S106 Overnight Test Rooms Summary

Generated: 2026-05-25 04:06

| Room | Status | Verdict | Asset Gaps | Critical Issues |
|---|---|---|---|---|
| Combat Basic | DONE | 7/10 | [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real | none |
| Ritual Diamond | DONE | 8/10 | [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real | none |
| Flooded Crypt | DONE | 7/10 | [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real | none |
| Library Alcove | DONE | 7/10 | [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real; Grouped alcoves are present only as cell-level alcovePositions; no NicheSpec group classification metadata was available in this JSON. | none |
| Boss Arena | DONE | 8/10 | [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real; [V2] No prefab for side_wall_stepped_2x_real | none |

## Overall Assessment
The V2 system produces recognizable room silhouettes from the five golden layouts using the placeholder wall kit. The strongest matches are the ritual diamond and boss arena because their silhouettes and socket intent survive without props; the weakest gap is grouped alcove semantics, which are still cell-level in the JSON path.

## Top 5 Priority Next Actions
1. Convert library alcove cell lists into grouped NicheSpec metadata.
2. Pilot Stream B real-asset swap on one generated room.
3. Add object/setpiece pass for sockets: altar, portal, bookshelves, sarcophagi, boss gate.
4. Promote screenshot gizmo colors into the native RoomDebugGizmo if the current gizmo should show blocked cells and piece classes.
5. Re-run hierarchy validation after real prefab swap, with special attention to door/open colliders.
