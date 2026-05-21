# Codex Task — Combat Room v14 Visual Fix (grid + obj placement + walls)

**Profile:** laurethayday (user explicit: "laurethayday'i dene tekrar")
**Effort:** xhigh
**Timeout:** 3600s (60 min)
**Type:** Surgical visual fix on existing Combat Room v14

## User feedback (verbatim 2026-05-18 night)

> "şu an grid olarak görünüyor ve objeler etrafta saçma duruyor duvarlar da aynı şekilde"

Combat Room v14 (`Assets/Screenshots/PlayableRoom_combat_v14.png`) — 3 specific complaints:
1. **Grid hala görünüyor** (floor texture grid-checkered pattern)
2. **Objeler saçma yerleştirilmiş** (perimeter cover not intentional enough)
3. **Walls de aynı şekilde saçma** (north border + side walls grid + placement)

## Reference: v13 Ritual Chamber PASS pattern

v13 (`Assets/Screenshots/PlayableRoom_pro_redesign_v13.png`) was visually PASS — its multi-biome procedural floor + intentional zone placement worked. v14 abandoned that floor technique → returned to grid look.

LaurethStudio reference: `F:/LaurethStudio/01_PIPELINE/multi_biome_blended_floor_technique.md` — procedural single-sprite biome blended floor.

## Fix plan (3 surgical changes, KEEP combat encounter design intent)

### Fix 1 — Replace v14 grid floor with multi-biome blended procedural floor

DO NOT keep the current grid-checkered floor. Generate a new procedural single-sprite floor (1152×704 at PPU=32) using the multi-biome blended technique from LaurethStudio doc:
- Base: cool dark slate (matches combat mood, NOT warm v13 amber)
- 6-8 region tints (Gaussian falloff): cool slate NW + bloodstain center + cooler shadow E + faint red-warm SE near brazier + dim ambient overall
- Edge vignette (cooler than v13)
- Pixel grain noise
- NO grid pattern

Replace `Floor_BigBiomes` or v14's floor child with this new big sprite.

### Fix 2 — Reposition objects intentionally (combat encounter layout)

v14 has props but spread feels arbitrary. Apply combat encounter design principles:

| Position | Object | Combat purpose |
|---|---|---|
| (10, 16) | 1 broken column (cover) | NW cover for player approach |
| (26, 16) | 1 brazier + 1 column (cover + warm threat) | NE cover, fire hazard |
| (10, 6) | 1 kneeling statue (cover) | SW cover, defeated foe lore |
| (26, 6) | 1 debris stack (cover) | SE cover |
| (18, 11) center | 1 small lava crack focal (existing v14 — KEEP) | Danger marker |
| (18, 2) south | EMPTY player entry | Clear path |
| (18, 20) north arch | EMPTY exit | Clear path |

5 enemy spawn markers (ash circles, currently red halos — KEEP but reposition to (12,16) (24,16) (12,6) (24,6) (18,18) — symmetric perimeter combat staging.

Remove any extra props beyond this list (no candelabra, no banner, no obsidian — combat encounter, not decoration).

### Fix 3 — Walls intentional (north border + door, NO grid)

Current v14 walls grid'i hissi veriyor. Improve:

- North wall border: 6-8 wall sprites at y=20, x=12-24 (existing v14 placement OK but...)
- USE WALL VARIANTS NOT IDENTICAL: alternate wall_01 (straight), wall_07 (cracked), wall_02 (worn) randomly (deterministic seed=42) so they don't look tiled
- Wall_11 (arched doorway) at center x=18, y=20 (exit gap)
- Remove `StageRoot/WallS/WallW/WallE_top/WallE_bot` (these are old collider walls from Phase 1.5 paint test setup — visual noise)
- Add 2-3 side wall fragments: NW corner (x=4, y=17) 1 wall + SW corner (x=4, y=4) 1 wall (visual room boundary suggestion, not full enclosure)

## CRITICAL — Unity pre-check

Unity OPEN. NOT in Play mode. Active scene `RoomPipelineTest`. Apply fixes to `PlayableRoom/Pro_Redesign_v14_CombatRoom` (active) — DO NOT touch `Pro_Redesign_v13_RitualChamber` (inactive, preserved as reference).

## Stages

### Stage 1 — Implement 3 fixes

Use `execute_code` C# in Unity-MCP. Generate new procedural floor texture in Edit mode. Replace floor sprite. Reposition props. Replace walls with varied sprites + remove old collider walls.

### Stage 2 — Verify

- Compile + isCompiling=false
- EditMode tests: 341/341 PASS (must remain)
- 0 console errors
- Save scene Edit mode

### Stage 3 — Game view screenshot

`Assets/Screenshots/PlayableRoom_combat_v14_fixed.png` via `manage_camera capture_source=game_view max_resolution=1100`

### Stage 4 — Self verify

Look at new screenshot:
- Floor grid GONE? (multi-biome blend visible, no checker)
- Objects feel intentional (4 corner cover + center focal + entry/exit clear)?
- Walls feel intentional (varied sprites, north border + 2 fragment hints, no full grid enclosure)?

If FAIL: iterate v14b, v14c (max 2 internal iterations).

### Stage 5 — DONE marker

`STAGING/CODEX_TASK_COMBAT_v14_VISUAL_FIX_DONE.md`:
- 3 fixes applied (yes/no + details)
- Iterations attempted
- New screenshot path
- EditMode test count
- Console error count
- Visual self-verdict

## Constraints

- DO NOT modify SO contract scripts
- DO NOT modify Phase 1.5 executors
- DO NOT touch `Pro_Redesign_v13_RitualChamber`
- DO NOT enter Play mode
- DO NOT add new Codex imagegen assets (use existing v2 + v3 only)
- Save scene Edit mode at end
- KEEP combat encounter intent (open center, perimeter cover, enemy markers)

## NEXT_SIGNAL

DONE → orchestrator review screenshot. If PASS → ready for user playtest. If FAIL → iterate or escalate.
