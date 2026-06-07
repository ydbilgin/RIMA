# 06 — Claude / Codex Master Brief (Portal Only)

RIMA is a 2D isometric action roguelite with floating stone island rooms above a void. For the current demo scope, rooms should NOT be treated as fully enclosed wall-heavy dungeons.

## Main direction
Use:
- existing floor set
- cliff perimeter
- portals instead of physical doors
- a few props / landmark props
- void background layers

Do NOT focus on:
- full perimeter walls
- 8-direction doors
- complex indoor wall logic

## Floor status
The base floor set already exists. Do not spend time redoing floors.

## Portal decision
For demo production:
- portal facing directions to produce: **1**
- placement sockets to support:
  - ENTRY_S
  - EXIT_NW
  - EXIT_N
  - EXIT_NE

Interpretation:
- The room visually has one readable exit side: the back/north side.
- That side supports up to three exit positions: left, center, right.
- All exit portals may reuse the same main facing direction.
- Entry may be a south spawn point or a simpler arrival effect.

## Portal variants
Produce 5 portal types:
1. Combat
2. Elite
3. Reward
4. Heal/Lore
5. Boss

Use one common base arch/frame, then vary:
- rune / crest
- icon
- particle intensity
- accent color
- reward badge

## Cliff production
Focus on:
- straight front cliff
- damaged front cliff
- outer corners
- inner corners
- front end-caps
- optional top-edge trims
- optional low parapet / broken edge pieces

## Props
Prioritize:
- chamber pedestal
- broken altar
- seal monolith
- rift crystal cluster
- brazier
- rubble / bone pile / broken pillar

## Room strategy
Normal combat rooms:
- no full walls
- floating island silhouette
- 1 to 3 back-edge portals
- 2 to 5 props max
- clean center for combat

Special rooms can be more framed:
- Attunement Chamber
- Boss room
- Vault / ritual room
- Lore sanctuary

## Report-friendly justification
Most runtime rooms are intentionally presented as floating combat islands rather than enclosed rooms. This improves combat readability, reduces procedural assembly complexity, and reinforces the game's void-bound world identity.
