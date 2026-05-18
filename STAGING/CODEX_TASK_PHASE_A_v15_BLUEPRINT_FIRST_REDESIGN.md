# Phase A v15: Combat Room Blueprint-First Redesign

Status: SPEC_READY_FOR_DISPATCH
Date: 2026-05-18
Profile: laurethgame (33% post-B-3, room to spare)
Effort: medium
Timeout: 3600s
Trigger: User direktifi "yeni blueprint kurulunca yeni mantıkla baştan o mapi dizayn edersiniz" — Phase B-3 LIVE (commit f76c36e), trigger met.

## Context

- Combat v14 fix LIVE (commit 005444e): cover-based intentional layout PASS_PARTIAL — major "saçma scatter" çözüldü ama side walls eksik, boş alan dressing eksik
- Phase B-3 Blueprint Painter LIVE: 6 semantic zones + AutoPopulator + Adjacency
- Phase B-3 already showed auto-populated map quality (Hades-tone preserved, logical zone-based) but didn't save the scene — that was a screenshot-only test
- This dispatch = save the actual Combat v15 scene using Blueprint Painter programmatically

## Goal

Build `Pro_Redesign_v15_BlueprintFirst_CombatRoom` GameObject in `Assets/Scenes/Demo/RoomPipelineTest.unity`. Deactivate v14, activate v15. v15 composition = Blueprint Painter API-driven with combat-focused layout.

## Procedure

1. **Open scene:** `Assets/Scenes/Demo/RoomPipelineTest.unity`
2. **Locate existing GameObject:** `Pro_Redesign_v14_CombatRoom` — `SetActive(false)` (don't delete, preserve for git diff comparison)
3. **Create v15 root:** `new GameObject("Pro_Redesign_v15_BlueprintFirst_CombatRoom")` at same world position as v14
4. **Programmatic Blueprint Painter run via execute_code:**
   ```csharp
   var profile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(
       "Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset");
   var canvas = new BlueprintCanvas();
   
   // Combat-focused 36x22 layout (y=0 bottom, y=21 top):
   // Top boundary wall (y=20-21)
   for (int x = 0; x < 36; x++) {
       canvas.Paint(new Vector2Int(x, 20), "wall", 1);
       canvas.Paint(new Vector2Int(x, 21), "wall", 1);
   }
   // Feature anchors NW (corner y=17-19, x=2-5) + NE (y=17-19, x=30-33)
   for (int x = 2; x <= 5; x++)
       for (int y = 17; y <= 19; y++)
           canvas.Paint(new Vector2Int(x, y), "feature", 1);
   for (int x = 30; x <= 33; x++)
       for (int y = 17; y <= 19; y++)
           canvas.Paint(new Vector2Int(x, y), "feature", 1);
   // Stone arena center (y=12-19 middle, x=8-27)
   for (int x = 8; x <= 27; x++)
       for (int y = 12; y <= 19; y++)
           canvas.Paint(new Vector2Int(x, y), "stone", 1);
   // Grass flanks (W: x=0-7 y=8-16; E: x=28-35 y=8-16) — side cover
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
   // Bottom path band (y=0-3 full width)
   for (int x = 0; x < 36; x++)
       for (int y = 0; y <= 3; y++)
           canvas.Paint(new Vector2Int(x, y), "path", 1);
   
   // No water zone (pool empty until imagegen DONE — separate dispatch)
   
   int placed = AutoPopulator.PopulateZones(canvas, profile, v15Root.transform, seed: 2026);
   int transitions = AutoPopulator.PopulateAdjacency(canvas, profile, v15Root.transform, seed: 2026);
   ```
5. **Save scene:** `EditorSceneManager.SaveScene(scene)`
6. **Screenshot:** Unity MCP `manage_camera` with orthographic projection, frame 36×22 room fully visible, output `Assets/Screenshots/PlayableRoom_combat_v15_blueprint_first.png`
7. **EditMode regression:** run `RIMA.MapDesigner.Editor.Tests` + `RIMA.Tests.EditMode` filters — expect 364/364 PASS
8. **Write DONE marker:** `STAGING/CODEX_TASK_PHASE_A_v15_BLUEPRINT_FIRST_REDESIGN_DONE.md`

## Acceptance Criteria

1. `Pro_Redesign_v14_CombatRoom` exists in scene, `activeSelf == false`
2. `Pro_Redesign_v15_BlueprintFirst_CombatRoom` exists, `activeSelf == true`, has ≥100 `_BlueprintPlaced_*` children
3. Scene saved (file modified, `.unity` updated)
4. Screenshot LIVE showing v15 composition (v14 not visible)
5. EditMode 364/364 PASS preserved
6. Console 0 errors (pool_water empty warning OK — expected)

## Self-iteration

- If placed count < 50 → iterate seed and re-populate, or report pool gap in DONE
- If feature zone places 0 in either NW/NE region → adjust region size + retry
- If scene save fails → iterate

Max 2 internal iterations.

## Verdict format

```
# Phase A v15 Blueprint-First Redesign Done
Status: DONE_FOR_ORCHESTRATOR_REVIEW
Date: 2026-05-18
Executor: Codex laurethgame

## Files modified
- Assets/Scenes/Demo/RoomPipelineTest.unity (v14 deactivated, v15 added)

## Files added
- Assets/Screenshots/PlayableRoom_combat_v15_blueprint_first.png

## Composition stats
- Zones painted: 6 (path, grass, stone, wall, feature; water skipped per spec)
- Cells painted: X
- Props placed by AutoPopulator: X
- Transition decals placed by AdjacencyPass: X
- Feature anchors placed: NW=X, NE=X

## EditMode regression
- 364/364 PASS preserved

## Console errors
- {none / list, pool_water gap warning expected}

## Phase A v15 deliverable verdict
{PASS_FOR_ORCHESTRATOR_REVIEW / NEEDS_HELP}
```

## Non-goals

- Adjacency completeness (3 placeholder pools — separate imagegen dispatch handles this)
- Water zone visualization (pool empty — imagegen dispatch handles)
- v14 deletion (preserved for comparison)
- Lighting tweaks (use scene's existing Light2D setup)
- Camera tweaks (use scene's existing camera)

## Important context

- Blueprint Painter API: `BlueprintCanvas` + `AutoPopulator` static methods are in `Assets/Editor/MapDesigner/Blueprint/` — these are EditMode-only types accessible via execute_code in editor context
- The `BlueprintPainterWindow` UI is NOT needed for this dispatch — direct API call is faster and reproducible
- Use namespace: `RIMA.MapDesigner.Blueprint` (or whatever Codex set in B-3 implementation)
- Verify with `unity_reflect` if uncertain about exact class API
