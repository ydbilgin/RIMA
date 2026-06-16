# RUN-MAP BRANCHING — IMPLEMENTATION REPORT (2026-06-16)

Surgical implementation of RUNMAP_BRANCHING_DESIGN_2026-06-16 with critic binding fixes.
Lineer -> procedural-branching. Code + compile-verify (read_console 0-error) + EditMode 30/30 PASS.
PLAY-verify NOT done (orchestrator serial, single-Unity-agent rule).

## CHANGED FILES

1. `Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs` (+~110 lines net)
   - Split node-type assignment out of the create loop into `AssignRoomTypes` (post-pass aware).
   - `RollRoomType`: new mid-depth mix — Combat ~50% / Elite ~20% (no-consecutive guard,
     `prevWasElite` -> falls back to Combat) / Chest ~15% / Merchant ~15%. depth0=Combat,
     last=Boss preserved. Event/Forge/Curse stay OUT (post-demo).
   - `ForceOneMerchant`: deterministic guarantee >=1 Merchant at mid-depth when rolls produced
     none (walks latest->earliest mid-depth, prefers non-Elite/non-Boss; last-resort first mid node).
   - Connectivity/orphan-free logic UNCHANGED (childIds<=3, boss 0 children, all-reachable hold).

2. `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` (2 edits)
   - BeginRun else-branch: per-run `runSeed = UnityEngine.Random.Range(int.Min,int.Max)`
     + `Debug.Log("[RunMap] seed=... depthCount=...")` (Date.now NOT used). Demo branch untouched.
   - `depthCount` C# default 5 -> 6 (design target). `forceDemoSequence` code default LEFT = true
     (toggle preserved per brief; flag NOT deleted).

3. `Assets/Scenes/_Arena.unity` (SerializedObject + SaveScene, NOT raw YAML)
   - RoomRunDirector: `forceDemoSequence` 1 -> 0 (branching ON for demo), `depthCount` 5 -> 6.
   - YAML verified persisted: line 3938 `forceDemoSequence: 0`, line 3948 `depthCount: 6`.

4. `Assets/Tests/EditMode/Room/RoomRuntimeDungeonGraphTests.cs` (+3 tests)
   - `Generate_GuaranteesAtLeastOneMerchant_AtMidDepth` (depthCount>=3, 20 seeds x 5 depths).
   - `Generate_NeverPlacesTwoConsecutiveElites_InGenerationOrder`.
   - `Generate_NeverGeneratesPostDemoNodeTypes` (only Combat/Elite/Chest/Merchant/Boss).
   - Existing tests (determinism, structure invariants, no-Event, demo-sequence, director) untouched & still pass.

## REVEAL = FULL (spec #5) — NO CODE NEEDED
`RunMapOverlay.OnGUI` already draws EVERY node + EVERY edge with no fog/reveal-gating. Full reveal
is the existing behavior. Fog post-demo. Nothing to change. (Critic agreed: demo = full-reveal.)

## VERIFICATION
- Compile: refresh_unity(force) x2 -> read_console ERRORS=0 WARNINGS=0 (baseline was 0).
- validate_script: DungeonGraph 0 err/0 warn; RoomRunDirector 0 err (4 generic linter heuristic
  warns = pre-existing FindObjectOfType/string-concat patterns, NOT from this change; Unity console 0).
- EditMode tests: job succeeded, 30/30 PASS, 0 failures (Room namespace: DungeonGraph + DemoSequence + Director).

## KNOWN LIMITATIONS / DEVIATIONS
- NLM canon NOT re-verified (auth EXPIRED). Node-mix numbers (50/20/15/15, Merchant-guaranteed,
  no-consec-Elite) implemented per brief's binding instructions + design doc, NOT NLM-confirmed.
  Critic binding-fix #6 (NLM verify) UNMET due to expired auth -> flagged here per warn-then-apply.
- PLAY not run (per brief). Critic binding-fix #1 (play-test multi-door geometry / BuildExitDoors
  socket-vs-fallback / AdvanceTo correct branch) is the orchestrator's serial verify step. NOT VERIFIED here.
- Console state at handoff: 0 errors, 0 warnings.
