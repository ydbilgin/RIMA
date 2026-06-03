# Task: Robust depth-sort + walkable for a TOP-DOWN 3/4 game with an ISO-rendered floor (RIMA)

ACTIVE RULES: (1) think before answering (2) cite file:line / project-setting (3) analysis + recommended fix, write code ONLY if a clear surgical fix (else specify exact change) (4) BLOCKED if a setting can't be read — say which.
NLM ACCESS: optional. Direct-read: the files/settings below + this file. Do NOT use live UnityMCP (the orchestrator is driving Unity; just READ files/asset YAML).
RESPOND INLINE -> CODEX_DONE.md. cx task (yekta).

## The whole problem (user playtest, escalating)
RIMA = top-down 3/4 ARPG (canon: "HIGH TOP-DOWN 3/4, NO iso projection math"), BUT the floor is built as a Unity **Isometric** Tilemap (CellLayout.Isometric, cellSize (1, 0.61, 1)). Movement is top-down cardinal (WASD -> world X/Y). Player capsule 0.53 x 0.95 (taller than the 0.61 iso tile). Symptoms the user reported, in order:
1. Player blocked ~1 row BEFORE the visible floor edge / can't traverse narrow iso-diagonal bridges.
2. Player can STAND ON TOP of pillars (no obstacle) and renders over everything (no depth).
3. "Shouldn't it look 3D?" — wants pillars to occlude the player (depth), walk AROUND them.

## What the orchestrator (Opus) already changed (verify these are right / not conflicting)
- Disabled the `VoidBlocker` TilemapCollider2D/CompositeCollider2D — void-fall now guarded ONLY by `PlayerController.FixedUpdate`'s `WalkabilityMap.IsWalkableWorld(nextPos)` pivot-check.
- Added EDGE-SLIDE to `PlayerController.FixedUpdate` (when next cell is void, keep the X-only or Y-only velocity component that stays walkable, instead of zeroing). User says bridges STILL feel stuck.
- Added `BoxCollider2D` base-footprints to the `Pillar_*` objects (obstacle).
- Wired the EXISTING `Assets/Scripts/Core/YSortBehaviour.cs` (sets sortingLayerName="Entities", sortingOrder = baseOrder - round(transform.position.y * 100)) onto the player Body + StoneColumn_* + Pillar_*.

## THE KEY CONTRADICTION to explain (this is the crux)
After wiring YSort, with the player positioned just NORTH of a pillar:
- player Body sortingOrder = **170**, pillar sortingOrder = **300**, BOTH on sortingLayer "Entities".
- Lower order (170) should draw FIRST = BEHIND. So the player SHOULD be occluded by the pillar.
- **BUT the player still renders ON TOP of the pillar.** Why?

Investigate the likely causes (read the actual project to confirm):
1. **Transparency Sort Mode / Custom Axis:** check `ProjectSettings/GraphicsSettings.asset` + the Camera (in `PlayableArena_Test01.unity`) for `TransparencySortMode` / `TransparencySortAxis` (e.g. Custom Axis (0,1,0)). If the project/camera does Y-axis transparency sorting, does it override/conflict with manual `sortingOrder`? (Within a sorting layer, sortingOrder is primary; transparency-sort only breaks ties — so equal-order objects sort by Y. If orders DIFFER (170 vs 300) the order should win... unless something resets it.)
2. **Multiple player SpriteRenderers:** does the player have MORE than one child SpriteRenderer (Body + weapon + shadow + ...)? `GetComponentInChildren<SpriteRenderer>()` returns only the FIRST. If the VISIBLE character renderer is a DIFFERENT child still on layer "Characters" (which is ABOVE "Entities" in the layer order: Floor<Decals<Walls<Entities<VFX<...<Characters<Props), it draws on top regardless. List the player's renderer hierarchy + their layers/orders.
3. **YSortBehaviour pivot/Y reference:** YSort uses `transform.position.y` of the object it's on. Player Body's world Y vs the pillar's world Y vs their PIVOTS — are they comparable? (Player pivot = feet bottom; pillar pivot = ? ). If the pillar's transform.y is its BASE but its sprite extends far UP, the "ground Y" used for sorting may not match the visual overlap. Recommend the correct ground-reference for each.
4. Does `HandAnchorAttach.UpdateWeaponSortOrder` (sets weapon order = body order ± 1) + the body now on "Entities" interact wrongly?

## Also address
5. **Walkable matches visible floor** + traversable narrow bridges with KEPT cardinal movement: is edge-slide enough, or is the iso `WorldToCell(transform.position)` (feet) the right check? Should the check use the feet ground-point consistently? Why might bridges still stick?
6. The capsule 0.95 tall vs 0.61 tile — does it still matter now the void collider is off?

## Deliver (inline -> CODEX_DONE.md)
- The ROOT cause of the "player renders on top despite lower order" (cite the setting/renderer you found).
- The robust, canonical fix for depth-sort in a top-down-3/4 + iso-floor Unity URP 2D game (sortingOrder-by-Y vs Camera TransparencySortMode CustomAxis — which to use, and the exact settings).
- Whether the orchestrator's edge-slide / collider / YSort changes are correct or should be replaced.
- Concrete file:line / setting changes.
