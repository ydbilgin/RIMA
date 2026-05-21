# Painter Scan Path Proposal

Generated: 2026-05-21

## Existing `wallScanFolders`

| Path | Exists? | Prefab count | Assessment |
|---|---:|---:|---|
| Assets/Prefabs/Props/ShatteredKeep_PixelLab | yes | 29 | Exists, but contains prop/statue/mounting prefabs rather than wall-only top-down scan assets. Flag for removal from wall scan path. |
| Assets/Prefabs/Walls/pilot_a | yes | 7 | Exists and contains 7 wall prefabs. Keep until wall_pack_v3 replaces it. |
| Assets/Prefabs/Walls | yes | 7 | Exists, but currently duplicates pilot_a recursively. Keep only if it becomes the canonical wall root; otherwise narrow to the active pack. |

## Proposed Top-Down Scan Paths

| Path | Exists now? | Recommendation |
|---|---:|---|
| Assets/Prefabs/Walls/wall_pack_v3 | no | Use as preferred wall scan root after prefab pack is generated/imported. |
| Assets/Prefabs/Decoration/Act1_ShatteredKeep | no | Use for decoration/prop category, not wall scan, after folder exists. |
| Assets/Prefabs/Characters | no | Use for character palette only after canonical 10 prefabs exist. |

## Deferred Change
No edit was made to `Assets/Editor/RimaWorldPainterWindow.cs` in this task.
