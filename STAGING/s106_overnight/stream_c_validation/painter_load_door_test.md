# Painter Door Save/Load Regression Test

Date: 2026-05-25

## Scenario
- Open RoomPainterWindow.
- Paint a valid walkable layout.
- Set the door brush at cell (5,3).
- Save the layout.
- Reload the same layout.

## Expected Result
- Saved JSON keeps the v2 flat door schema: `"door": [5,3]`.
- LoadLayout parses the flat point through `MiniJson.GetPointOrNull("door")`.
- `doorCell` is restored to `(5,3)` when the point is inside the current grid bounds.

## Result
- Parser code updated for the flat `[x,y]` schema.
- Unity editor reflection check returned `(5, 3)` for `{ "door": [5,3] }`.
- Direct manual UI execution was not performed in this pass because the task procedure forbids scene operations.
