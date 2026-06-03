# Council question — Gemini 3.1 Pro (High): DEEP / test-architecture lens

You are advising the RIMA 2D roguelite project (Unity, C#). We just built a "Demo B-lite" run loop and now need a TEST MATRIX before writing tests. Your lens = deep test architecture + invariants + edge cases.

## Read these files (use your file tools; they are NOT inlined here):
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\MapDesigner\Room\Runtime\DungeonGraph.cs   (NEW pure-C# class RIMA.MapDesigner.Room.Runtime.DungeonGraph: static Generate(seed, depthCount), nodes/startId/ChildrenOf/Get/NodesAtDepth)
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\MapDesigner\Room\Runtime\RoomRunDirector.cs
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\MapDesigner\Room\Runtime\IsoRoomBuilder.cs
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\MapDesigner\Room\Runtime\RunMapOverlay.cs
- F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\DEMO_ARCHITECTURE_DECISION_2026-06-03.md

## Produce a TEST MATRIX (table per component). For each component list: behavior/invariant → assertion → EditMode vs PlayMode vs manual-play-verify → priority (P0/P1/P2).

Focus questions:
1. **DungeonGraph (the NEW class):** What invariants must hold for EVERY seed (property-based)? Consider: determinism (same seed+depthCount → identical graph), depth0 = exactly 1 node of type Combat, last depth = exactly 1 node of type Boss, boss has 0 children, NO orphan nodes (every node depth>0 has >=1 parent), every node reachable from start, child count per node in [1,3], NodesAtDepth counts. Edge cases: depthCount<2 (clamped to 2), depthCount=2, large depthCount, seed=0/negative.
2. **RoomRunDirector:** Navigation invariants — BeginRun sets CurrentNodeId=startId; CurrentChoices == graph.ChildrenOf(current); AdvanceTo(valid) moves to that child; AdvanceTo(invalid index) is a no-op; IsRunComplete true only at boss (childless) node; door count passed to BuildExitDoors == CurrentChoices.Count. Which of these are testable WITHOUT a Unity scene?
3. **IsoRoomBuilder:** What can a headless/PlayMode test assert without human eyes (build throws no exception, floor cell count, PlayerSpawnMarker non-null, exit-door GameObject count == doorTypes.Count, no cliff geometry overflow)?
4. **RunMapOverlay (M-key StS map) + branch-doors visual layout (side-by-side Hades doors):** These are OnGUI / visual. Define CRISP acceptance criteria a human/agent can play-verify (what exactly to look for), since they resist unit testing.
5. Which invariants are best expressed as PROPERTY tests looping many seeds vs single example tests?

Keep it concrete and tabular. This feeds an Opus synthesis + then real NUnit tests.
