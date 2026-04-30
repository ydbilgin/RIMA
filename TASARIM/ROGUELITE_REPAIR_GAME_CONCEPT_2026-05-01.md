# Roguelite Repair Game Concept
Date: 2026-05-01
Owner mark: CODEX_RESEARCH
Status: concept exploration, not locked design

## Question
Can a roguelite repair game work? Can it be similar to a workshop game, but with roguelite progression and short-session hooks?

Short answer: yes, but the repair layer must be a real decision system, not just a cosmetic minigame.

## Core Fantasy
The player runs a strange repair shop for broken relics, cursed tools, damaged machines, or dungeon gear. Each run provides broken items, parts, defects, and risks. Between or during runs, the player repairs items through short tactile challenges and chooses what to keep, sell, risk, or upgrade.

Possible RIMA-compatible framing:
- The world leaves behind broken relics after rooms.
- The player repairs "wounded" artifacts in a forge/workshop after a run.
- Each repaired relic can become a modifier, skill augment, class-specific charm, or challenge-room key.

## Why Roguelite + Repair Can Work
Repair is naturally procedural:
- item type
- defect type
- missing part
- curse side effect
- repair method
- quality grade
- risk/reward outcome

That maps well to roguelite structure:
- random drops
- temporary builds
- permanent unlocks
- escalating risk
- meaningful choices under imperfect information

## Strong Core Loop Options

### Option A - Run First, Repair After
1. Enter rooms.
2. Defeat enemies and collect broken relics/parts.
3. Return to workshop.
4. Inspect defects.
5. Pick repair method.
6. Repair through a short minigame.
7. Keep, equip, sell, or archive item.

Best for RIMA as an endgame layer.

### Option B - Repair During Combat Run
1. Find broken stations/items in rooms.
2. Decide whether to spend time repairing while danger exists.
3. Successful repair creates temporary advantage.
4. Failed repair spawns risk, curse, or malfunction.

Best for a standalone roguelite repair game.

### Option C - Orders + Dungeon Supply
1. Customers request repaired items.
2. Player enters short dungeons to get parts.
3. Workshop phase repairs and ships order.
4. Better orders unlock harder part biomes.

Best for a cozy commercial workshop game.

## Repair Minigame Types
Keep minigames short, readable, and system-driven.

Good candidates:
- align cracked rune lines before timer ends
- clean corrosion by tracing a path
- route energy through a small circuit
- hammer dents with timing windows
- match missing gear shapes
- cool overheating part without freezing it
- remove cursed splinters in correct order
- pack repaired item into delivery box with space constraints

Avoid:
- long QTE chains
- vague "hold button until done"
- minigames that do not affect outcome
- too many station types before the core loop is fun

## Roguelite Design Levers

Item properties:
- base item family
- defect
- curse
- rarity
- repair difficulty
- repair material cost
- hidden flaw
- post-repair quality

Player choices:
- quick fix vs perfect repair
- stabilize curse vs extract power
- sell now vs keep for build
- repair for customer vs repair for self
- risk overcharge for bonus effect

Progression:
- unlock tools
- unlock stations
- hire assistants
- improve diagnosis accuracy
- expand storage
- unlock new defect families
- unlock daily repair contracts

Failure states:
- item breaks
- item becomes cursed
- lower quality
- repair time penalty
- customer trust loss
- temporary combat drawback

## RIMA Integration Idea
This should not enter Phase 1. It is a possible late/endgame layer after core room combat is stable.

Possible name/role:
"Relic Repair Bench" or "Forge Lab"

How it could fit:
- After boss or after act clear, player receives damaged relics.
- Player chooses one relic to repair.
- Repair creates a build modifier for future runs.
- Higher quality repair gives stronger but still bounded effect.
- Some damaged relics open short challenge rooms.

Why it fits RIMA:
- RIMA already has Forge room type in system map.
- Current room system is single-room rebuild, so short repair/challenge rooms are cheap to add later.
- Class identity can feed repair effects:
  - Warblade repairs broken armor/relic blades.
  - Ranger repairs trap mechanisms.
  - Gunslinger repairs heat chambers.
  - Summoner repairs vessels/binding tokens.
  - Hexer stabilizes cursed fragments.

Risk for RIMA:
- It can bloat the project if added before combat readability and skill contracts are locked.
- It may compete with draft/reward clarity.
- It needs its own economy; shallow repair would feel like menu friction.

Recommendation for RIMA:
Keep as a "post-Phase 1 / endgame candidate":
- not mandatory for main loop
- useful for daily challenge or post-run progression
- can reuse PixelLab props and generated repair boards
- should be prototyped as one station, one item family, one defect family only

## Standalone Small Game Pitch
Title placeholder:
Repairlite

Pitch:
A cozy roguelite repair shop where every broken item is a tiny procedural puzzle. Go out for parts, repair cursed objects, ship orders, upgrade the shop, and risk dangerous over-repairs for rare effects.

Minimum prototype:
- one workshop screen
- 12 item icons
- 4 defect types
- 2 repair minigames
- 1 order board
- 1 short part-gathering run
- 5 upgrades

Success test:
Can a player finish one order in under 90 seconds and immediately want another?

## PixelLab Asset Plan
PixelLab can efficiently generate:
- repair bench
- tools
- broken relic icons
- item variants
- boxes
- shop props
- small customer portraits
- biome-themed parts
- repair board skins

Do not use PixelLab for:
- puzzle solvability
- economy math
- procedural rules
- final UI layout

## Prototype Recommendation
If this becomes a real prototype, start with:
1. One repair minigame: align cracked rune lines.
2. One item family: broken relic plates.
3. Three defect types: cracked, corroded, unstable.
4. One reward type: run modifier.
5. One failure type: cursed drawback.

This is enough to test whether "repair as roguelite decision" is fun.

