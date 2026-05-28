# Test Room - Library Alcove (library_alcove)

## Layout Source
`STAGING/s106_overnight/stream_e_rooms/layouts/library_alcove.json`

## Generation
- Scene: `Assets/Scenes/Test/PainterTestE_library_alcove.unity`
- Builder: WallChainRoomBuilder (V2)
- Asset mode: Placeholder (Stream B real-asset swap is separate)
- Walkable cells: 109
- Spawned pieces: 50

## Screenshots
- Scene: `STAGING/s106_overnight/stream_e_rooms/library_alcove/scene.png`
- Gizmo: `STAGING/s106_overnight/stream_e_rooms/library_alcove/gizmo.png`

## Used Assets
| WallPieceData.id | Count | Notes |
|---|---:|---|
| door_arch | 1 | Placeholder V2 | 
| inner_corner | 12 | Placeholder V2 | 
| low_front_2x | 4 | Placeholder V2 | 
| open_gap | 9 | Placeholder V2 | 
| outer_corner | 10 | Placeholder V2 | 
| rear_wall_1x | 4 | Placeholder V2 | 
| rear_wall_3x | 2 | Placeholder V2 | 
| side_wall_1x | 8 | Placeholder V2 | 

## Missing Assets
- [V2] No prefab for side_wall_stepped_2x_real
- [V2] No prefab for side_wall_stepped_2x_real
- Grouped alcoves are present only as cell-level alcovePositions; no NicheSpec group classification metadata was available in this JSON.

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
- Score: 7/10
- Why? The alcove structure is visible and maps to the library analog, but grouped alcoves are represented through cell cuts rather than higher-level niche groups.

## Next Actions
- [ ] (P0) None for placeholder generation.
- [ ] (P1) Convert cell-level alcoves to grouped NicheSpec metadata for cleaner grouped-alcove semantics.
- [ ] (P2) Add prop/setpiece art for final blueprint readability.

## Generation time
- Scene setup: 0,09s
- Build: 0,00s
- Total: 0,09s
