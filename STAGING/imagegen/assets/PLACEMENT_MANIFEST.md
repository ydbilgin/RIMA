# RIMA Imagegen Placement Manifest

Generated assets live outside Unity `Assets/` by design. Do not import until the orchestrator/user is ready to wire them.

## portal_arch_gen.png

- Source path: `STAGING/imagegen/assets/portal_arch_gen.png`
- Later wiring: `DoorNorth` GameObject's `GateBehavior.spriteUnlockedBase` and `GateBehavior.spriteRoomCombat`
- Scenes: `_IsoGame`, `_IsoGame_Map02`, `_IsoGame_Map03`
- Unity import settings: Sprite (Single), PPU 64, Point filter, alphaIsTransparency enabled
- Pivot: bottom-center `(0.5, 0)`
- Recommended in-game size: about 2 units wide by 4 units tall at PPU 64; bottom should sit on the doorway/floor anchor.

## reward_relic_gen.png

- Source path: `STAGING/imagegen/assets/reward_relic_gen.png`
- Later wiring: `RoomClearVictoryTrigger.rewardSprite`
- Scenes: `_IsoGame`, `_IsoGame_Map02`, `_IsoGame_Map03`
- Unity import settings: Sprite (Single), PPU 64, Point filter, alphaIsTransparency enabled
- Pivot: center `(0.5, 0.5)`
- Recommended in-game size: about 2 units square at PPU 64; use existing reward pickup placement scale unless it needs minor scene-side adjustment.

## echo_mote_gen.png

- Source path: `STAGING/imagegen/assets/echo_mote_gen.png`
- Later wiring: future Echo currency pickup; no wiring yet
- Unity import settings: Sprite (Single), PPU 64, Point filter, alphaIsTransparency enabled
- Pivot: center `(0.5, 0.5)`
- Recommended in-game size: about 1 unit square at PPU 64; keep it visually smaller than the relic pickup.

## Replaces Old Placeholders

- `Assets/Sprites/Environment/Portal/portal_arch_cyan.png`
- `Assets/Sprites/Reward/reward_relic_cyan.png`
