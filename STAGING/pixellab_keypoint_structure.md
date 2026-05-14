# PixelLab Keypoint Structure Probe

## Probe
- Date: 2026-05-14
- Character: `072be2d5-cd87-4334-84be-c30044cfe88b`
- Template animation requested: `walk-1`
- Direction requested: `south`
- Download command: `curl.exe --fail -L https://api.pixellab.ai/mcp/characters/072be2d5-cd87-4334-84be-c30044cfe88b/download -o STAGING/pixellab_keypoint_probe/072be2d5.zip`

## metadata.json Shape
- Top-level fields: `group_id`, `states`, `export_version`, `export_date`
- `export_version`: `3.0`
- Character data lives at `states[0].character`
- Frame paths live at `states[0].frames`
- Rotation frame paths live at `states[0].frames.rotations.<direction>`
- Animation frame paths live at `states[0].frames.animations.<animationId>.<direction>[]`

Observed animation key:
- `states[0].frames.animations.walking-89dccdfd.south`
- Frame count: 6
- Frame path format: `states/<stateFolder>/animations/<animationId>/<direction>/frame_000.png`

## Keypoint Findings
- No top-level `keypoints` field is present.
- No per-state `keypoints` field is present.
- No frame-level keypoint payload is present under `states[0].frames`.
- No `hand_left`, `hand_right`, `LEFT HAND`, `RIGHT HAND`, wrist, or arm endpoint fields are present in this export.
- Coordinate format cannot be verified from this export because no keypoint coordinates were included.

## Gate Decision
Path B: Unity-side manual annotation fallback.

Reason: the real PixelLab ZIP export for the requested character and template animation contains animation frame paths, but no keypoint structure at all. Level 2 per-frame attach must therefore read hand anchors from manually authored `SpriteHandData` ScriptableObjects. PixelLab metadata-driven import remains a Phase 2.5 follow-up if the MCP/API exposes keypoints in a future export shape.
