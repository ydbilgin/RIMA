# Codex Task — Hierarchy Fixes (rima-sonnet audit 4 items)

**Profile:** auto-selected (laurethayday should win — yasinderyabilgin near limit)
**Effort:** high
**Timeout:** 1200s (20 min)
**Type:** Surgical scene fixes via Unity-MCP

## Context

rima-sonnet hierarchy audit of `Assets/Scenes/Demo/RoomPipelineTest.unity` (S88_NIGHT post-Combat-v14) flagged 4 fixes. Codex Combat Room redesign (`bgzxjs1b6`) is DONE, no scene-write conflict.

Active hierarchy: `PlayableRoom/Pro_Redesign_v14_CombatRoom` (active) + `Pro_Redesign_v13_RitualChamber` (inactive preserved).

## CRITICAL — Unity pre-check

Unity OPEN, instance `RIMA@ed023e0b`. Verify scene `RoomPipelineTest` active. NOT in Play mode (save fails).

## 4 fixes (apply in order, save scene at end)

### Fix 1 — Create `Decoration/06_AtmosphericAccents` container

Audit found atmospheric accents (Portal_E, Puddle_SW, Obsidian_NE) scattered into zone groups instead of a dedicated container. The DONE marker promised this container but Codex skipped.

- If `PlayableRoom/Decoration` exists (from v11/v13/v14): add child empty GameObject `06_AtmosphericAccents` at end of Decoration children
- Find all GameObjects matching name pattern: `Portal_*`, `Puddle_*`, `Obsidian_*` under `PlayableRoom` (anywhere in zone hierarchies)
- Reparent each found object to `Decoration/06_AtmosphericAccents` (use `Transform.SetParent` with worldPositionStays=true)
- If `Decoration` parent doesn't exist (v14 may use different structure), document in DONE marker and skip Fix 1

### Fix 2 — `WallsTilemap_Front` sortingOrder 2 → 6

- Find GameObject `WallsTilemap_Front` (anywhere in scene)
- Get its `TilemapRenderer` component
- Set `sortingOrder = 6` (was 2 — rendering behind decals incorrectly)

If `WallsTilemap_Front` doesn't exist (v14 may have removed it), document + skip.

### Fix 3 — `WallS/WallN/WallW/WallE_top/WallE_bot` sortingOrder 5 → 6

- These are 5 collider-only wall sprites under `StageRoot` (from earlier Phase 1.5 paint test setup)
- Find each by name, get `SpriteRenderer`, set `sortingOrder = 6`
- Spec layer 6 = Walls

### Fix 4 — `Floor_BigBiomes` sibling index → 0 (first child of PlayableRoom)

- Find `Floor_BigBiomes` GameObject (or `Floor_BigBiomes_Combat` if v14 renamed)
- `transform.SetSiblingIndex(0)` so it appears first in PlayableRoom children
- Unity sibling order affects render tie-breaking when sortingOrders match

If `Floor_BigBiomes` doesn't exist in active hierarchy (v14 may use different floor), apply to whatever floor GameObject is the largest sprite in PlayableRoom.

## Verification

After all 4 fixes:
1. `manage_scene action=get_hierarchy` — verify `Decoration/06_AtmosphericAccents` exists with reparented children
2. `find_gameobjects` — verify wall sortingOrders = 6
3. Save scene (Edit mode only)
4. `read_console` — 0 new errors
5. Run EditMode tests: must remain 333/333 PASS

## DONE marker

`STAGING/CODEX_TASK_HIERARCHY_FIXES_DONE.md`:
- Fix 1 result (created? how many reparented? or skipped because container missing?)
- Fix 2 result (changed? skipped?)
- Fix 3 result (5 walls changed? any missing?)
- Fix 4 result (sibling index 0 confirmed?)
- EditMode test count
- Console error count

## Constraints

- DO NOT modify any SO contract scripts
- DO NOT modify Codex's Pro_Redesign_v14_CombatRoom or Pro_Redesign_v13_RitualChamber composition (only fix sortingOrders + reparent containers)
- DO NOT enter Play mode
- Save scene in Edit mode at end

## NEXT_SIGNAL

After DONE: orchestrator confirms cleanup. Next dispatch = Phase B-1 implementation (Asset Pack Browser Editor Tool) using verified spec.
