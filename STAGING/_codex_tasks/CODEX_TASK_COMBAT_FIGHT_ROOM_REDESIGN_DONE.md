# Combat Fight Room Redesign DONE

## Iterations attempted
- v14 initial: built combat room, but self-QC failed because the game-view screenshot was too dark and clipped the north exit.
- v14 revised: increased cool ambient visibility, reduced vignette opacity, widened camera framing, and kept the player at the south entry. Self-QC PASS.

## Critique findings v13
- v13 read as a ritual chamber because the player stood on a gold ritual sigil framed by braziers and shrine-like central props.
- The blue portal puddle, candelabra, moss creep, banner, and decorative pebble clutter created objective/ambience noise instead of encounter readability.
- The redesign needed open center movement, perimeter cover, north exit, enemy spawn markers, and a single threat focal mark.

## Combat redesign decisions by zone
- Center 6x6: kept clear of colliders and props; added only one small scorch/rift crack decal to imply danger without blocking dodge space.
- North wall and exit: added v3 wall sprites with `wall_11` arched doorway as the room exit, plus the existing `DoorExit` trigger moved to the north.
- NW cover: broken column and debris pile create a tactical cover pocket outside the center.
- NE cover: brazier plus column create the single warm threat accent and a readable cover pair.
- SW cover: defeated statue and scorch marker create combat aftermath without shrine/loot language.
- SE cover: debris stack and ash circle create a second tactical pocket and enemy-entry read.
- South entry: player starts at the south entry with sparse pebble guidance into the arena.
- Enemy markers: five perimeter `MobSpawnPoint` placeholders with ash/blood/scorch decals; no enemies or loot spawned.
- Lighting/camera: cooler global light, one warm brazier accent, heavy-but-readable edge vignette, camera widened to keep entry, center, cover, and north arch visible.

## New assets generated
- No Codex imagegen assets were needed.
- Generated procedural combat assets under `Assets/Sprites/Environment/CombatV14/`:
  - `PlayableRoom_DesignedFloor_combat_v14.png`
  - `CombatV14_EdgeVignette.png`
  - `CombatV14_AshCircle.png`
  - `CombatV14_BloodSplat.png`
  - `CombatV14_RiftCrackRed.png`

## Verification
- Screenshot: `Assets/Screenshots/PlayableRoom_combat_v14.png`
- Visual gate verdict: PASS. The room now reads as a real fight encounter: open center, perimeter cover, implied enemy entries, north exit, no loot, no ritual sigil, no portal puddle.
- Play mode probe: PASS. Player moved 4.44 units with `rb.linearVelocity=(0,2.2)` over 2 seconds, and camera follow target remained Player.
- Play mode console errors: 0 project errors after console clear during the probe. MCP bridge connect/disconnect noise only.
- EditMode tests: PASS, 333/333 passed, 0 failed, 0 skipped.
