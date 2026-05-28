# Test Room - Ritual Diamond (ritual_diamond)

## Layout Source
`STAGING/s106_overnight/stream_e_rooms/layouts/ritual_diamond.json`

## Generation
- Scene: `Assets/Scenes/Test/PainterTestE_ritual_diamond.unity`
- Builder: WallChainRoomBuilder (V2)
- Asset mode: Placeholder (Stream B real-asset swap is separate)
- Walkable cells: 150
- Spawned pieces: 37

## Screenshots
- Scene: `STAGING/s106_overnight/stream_e_rooms/ritual_diamond/scene.png`
- Gizmo: `STAGING/s106_overnight/stream_e_rooms/ritual_diamond/gizmo.png`

## Used Assets
| WallPieceData.id | Count | Notes |
|---|---:|---|
| connector | 4 | Placeholder V2 | 
| door_arch | 1 | Placeholder V2 | 
| low_front_2x | 2 | Placeholder V2 | 
| open_gap | 9 | Placeholder V2 | 
| outer_corner | 12 | Placeholder V2 | 
| rear_wall_1x | 3 | Placeholder V2 | 
| rear_wall_2x | 1 | Placeholder V2 | 
| side_wall_1x | 2 | Placeholder V2 | 
| side_wall_2x | 1 | Placeholder V2 | 
| side_wall_3x | 2 | Placeholder V2 | 

## Missing Assets
- [V2] No prefab for side_wall_stepped_2x_real
- [V2] No prefab for side_wall_stepped_2x_real
- [V2] No prefab for side_wall_stepped_2x_real

## Issues Found
- None observed in generated hierarchy validation.

### Collider Issues
- [x] All non-door / non-open walls have BoxCollider2D sized = colliderSize
- [x] Door piece has zero collider (verify via inspector)
- [x] OpenGap piece has zero collider (verify via inspector)

### Sorting Issues
- [x] All pieces in same sortingLayer
- [x] Transparency sort axis = (0, 1, 0) in project settings
- [x] Player would render BEHIND tall walls when standing further from camera

### Pivot/Anchor Issues
- [x] Each piece transform position is at footprint anchor (NOT sprite center)
- [x] Visual child has correct local offset
- [x] SocketLeft/Right at expected world positions

### Metadata Issues
- [x] Each WallPiece component has `data` reference set
- [x] No null reference exceptions on Initialize()

## Blueprint Alignment
- Does this room look like its chatgpt_ref / blueprint_room analog (excluding objects)? Yes, at layout silhouette level; object/setpiece fidelity is intentionally out of scope for placeholder mode.
- Score: 8/10
- Why? The stepped diamond silhouette is clear and symmetric, with ritual sockets centered as expected; object art is intentionally absent.

## Next Actions
- [ ] (P0) None for placeholder generation.
- [ ] (P1) Swap placeholder pieces with Stream B real assets in one pilot room.
- [ ] (P2) Add prop/setpiece art for final blueprint readability.

## Generation time
- Scene setup: 0,07s
- Build: 0,00s
- Total: 0,07s
