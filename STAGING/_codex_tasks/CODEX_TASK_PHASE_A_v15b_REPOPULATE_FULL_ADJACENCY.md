# Phase A v15b: Re-populate Combat Room with Full Adjacency

Status: SPEC_READY_FOR_DISPATCH
Date: 2026-05-18
Profile: yasinderyabilgin (FRESH post-reset)
Effort: medium
Timeout: 1800s

## Context

Combat v15 LIVE (commit 7238130): Blueprint-First layout, 378 props, 5 transition decals (transitions sparse because 3 pools used placeholder).

Phase B-3 Asset Gap Imagegen LIVE (just committed): 32 new sprites, 3 new transition pools (grass/stone, path/grass, water/grass) wired into adjacency rules.

This dispatch = re-run v15 composition with the populated transition pools to get **rich boundary decals**.

## Goal

Create new `Pro_Redesign_v15b_FullAdjacency_CombatRoom` GameObject with same paint pattern as v15 but using newly populated transition pools. Preserve v15 for comparison.

## Procedure

1. **Open scene:** `Assets/Scenes/Demo/RoomPipelineTest.unity`
2. **Locate:** `Pro_Redesign_v15_BlueprintFirst_CombatRoom` — keep active for now (will deactivate at step 7)
3. **Create:** `new GameObject("Pro_Redesign_v15b_FullAdjacency_CombatRoom")` at same world position as v15
4. **Execute Blueprint API via execute_code** — SAME paint pattern as v15 spec:
   ```csharp
   var profile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(
       "Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset");
   var canvas = new BlueprintCanvas();
   
   // Wall band (y=20-21 full width)
   for (int x = 0; x < 36; x++) {
       canvas.Paint(new Vector2Int(x, 20), "wall", 1);
       canvas.Paint(new Vector2Int(x, 21), "wall", 1);
   }
   // Feature anchors NW (x=2-5 y=17-19) + NE (x=30-33 y=17-19)
   for (int x = 2; x <= 5; x++)
       for (int y = 17; y <= 19; y++)
           canvas.Paint(new Vector2Int(x, y), "feature", 1);
   for (int x = 30; x <= 33; x++)
       for (int y = 17; y <= 19; y++)
           canvas.Paint(new Vector2Int(x, y), "feature", 1);
   // Stone arena center (x=8-27 y=12-19)
   for (int x = 8; x <= 27; x++)
       for (int y = 12; y <= 19; y++)
           canvas.Paint(new Vector2Int(x, y), "stone", 1);
   // Grass flanks (W: x=0-7 y=8-16; E: x=28-35 y=8-16)
   for (int x = 0; x <= 7; x++)
       for (int y = 8; y <= 16; y++)
           canvas.Paint(new Vector2Int(x, y), "grass", 1);
   for (int x = 28; x <= 35; x++)
       for (int y = 8; y <= 16; y++)
           canvas.Paint(new Vector2Int(x, y), "grass", 1);
   // Path approach (y=4-11, x=12-23)
   for (int x = 12; x <= 23; x++)
       for (int y = 4; y <= 11; y++)
           canvas.Paint(new Vector2Int(x, y), "path", 1);
   // Bottom path band (y=0-3 full)
   for (int x = 0; x < 36; x++)
       for (int y = 0; y <= 3; y++)
           canvas.Paint(new Vector2Int(x, y), "path", 1);
   // OPTIONAL: small water pocket in grass (x=2-4 y=12-13) — test water pool
   for (int x = 2; x <= 4; x++)
       for (int y = 12; y <= 13; y++)
           canvas.Paint(new Vector2Int(x, y), "water", 1);
   
   int placed = AutoPopulator.PopulateZones(canvas, profile, v15bRoot.transform, seed: 2026);
   int transitions = AutoPopulator.PopulateAdjacency(canvas, profile, v15bRoot.transform, seed: 2026);
   ```
5. **Deactivate v15:** `v15Root.SetActive(false)` (preserve for diff)
6. **Save scene**
7. **Screenshot:** Unity MCP `manage_camera` orthographic, frame 36×22 room, output `Assets/Screenshots/PlayableRoom_combat_v15b_full_adjacency.png`
8. **EditMode regression:** 364/364 PASS verify
9. **Write DONE marker:** `STAGING/CODEX_TASK_PHASE_A_v15b_REPOPULATE_DONE.md`

## Acceptance Criteria

1. v15 GameObject EXISTS + `activeSelf == false`
2. v15b GameObject EXISTS + `activeSelf == true` + has Blueprint children
3. **Transition decals count > 5** (v15 had 5; expect 20+ now with populated pools)
4. Water zone in v15b shows water pool props (pool_water now non-empty, 8 sprites)
5. Scene saved
6. Screenshot LIVE
7. EditMode 364/364 PASS preserved
8. Console 0 errors

## Self-iteration

- If transition decals still < 10 → check adjacency rule wiring, iterate
- If water zone has 0 props → verify pool_water.asset entries (should have 8 sprites)
- Max 2 internal iterations

## Verdict format

```
# Phase A v15b Re-populate Done
Status: DONE_FOR_ORCHESTRATOR_REVIEW
Date: 2026-05-18
Executor: Codex yasinderyabilgin

## Composition stats
- Props placed: X
- Transition decals: X (target > 10, much better than v15's 5)
- Water zone props: X (target > 0)
- Total children: X

## v15 vs v15b diff
- v15 transitions: 5
- v15b transitions: X
- Improvement factor: X (target > 4x)

## EditMode regression
- 364/364 PASS

## Sample screenshot
- Assets/Screenshots/PlayableRoom_combat_v15b_full_adjacency.png

## Console errors
- {none / list}

## Phase A v15b deliverable verdict
{PASS_FOR_ORCHESTRATOR_REVIEW / NEEDS_HELP}
```
