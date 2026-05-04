# RIMA Map Layout and Spawn Pass - 2026-05-03

## Scope

User requested stronger map designs, better mob placement, helper-tool direction, and status
persistence.

Files touched:
- `Assets/Scripts/Core/RuntimeRoomManager.cs`
- `CURRENT_STATUS.md`

## Map Layout Changes

`LargeDungeonMapPainterBase` template cycle expanded from 9 combat templates to 15 combat
templates, plus the existing boss antechamber.

New combat layouts:
- CrescentSanctum
- BrokenCauseway
- ReliquaryLoop
- ForkedOssuary
- AmbushCloister
- RiftWell

Each new layout has:
- its own floor mask silhouette
- interior wall/pillar feature pass
- per-layout warm/cool/magic light socket pattern

The immediate goal is not final art. It is stronger authored room composition: readable combat
core, broken outline, side chambers, negative space, and visible anchor points.

## Enemy Spawn Distribution

Enemy spawning is no longer a plain random point in the floor area.

RuntimeRoomManager now uses banded spawn zones:
- flank left/right
- rear/top and lower pressure
- side lanes
- secondary diagonal pockets
- special elite bands with wider spacing

Spawn validation now checks:
- margin away from walls
- floor tile presence
- wider wall-nearby rejection
- minimum distance from room center/player
- minimum spacing from already reserved spawn positions
- collider overlap fallback

This should prevent early mobs from stacking in one random clump and should make waves read as
intentional pressure patterns.

## Tool Direction

Use the existing tool research verdict:
- LDtk is the first candidate for authored room masks and metadata.
- Tiled is the second candidate.
- Unity Tilemap remains the runtime renderer/import target.
- Unity 2D Tilemap Extras is useful for RuleTile/RandomTile style rendering.
- WFC is deferred to micro-detail only, not main room authoring.

Target authored room data:
- FloorMask
- WallMask
- Door sockets
- Light sockets
- EnemySpawnZone
- PropZone
- LandmarkSlot
- RoomType
- DifficultyBand

## Verification

Unity script validation:
- `Assets/Scripts/Core/RuntimeRoomManager.cs`: PASS, 0 errors, 0 warnings.

Unity assembly smoke:
- Runtime enum now reports 15 combat templates plus BossAntechamber.
- Room index 1..15 maps to all expected layouts and valid sizes.

Unity console:
- No game compile errors observed after script refresh.

