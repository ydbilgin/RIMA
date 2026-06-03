# Council question — Gemini 3.5 Flash (High): LEAN / ship-fast lens + over-engineering critique

RIMA (Unity C# 2D roguelite). We built a "Demo B-lite" run loop and are about to write tests. Your lens = the LEANEST test set that catches REAL regressions, plus a critique of any over-testing.

## Read these files (file tools; NOT inlined):
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\MapDesigner\Room\Runtime\DungeonGraph.cs   (NEW pure-C# class: static Generate(seed, depthCount))
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\MapDesigner\Room\Runtime\RoomRunDirector.cs
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\MapDesigner\Room\Runtime\IsoRoomBuilder.cs
- F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\MapDesigner\Room\Runtime\RunMapOverlay.cs

## Answer:
1. If you could write only ~6-10 tests total for this whole demo, WHICH ones (exact behavior)? Rank by regression-value-per-effort.
2. Which proposed tests would be OVER-ENGINEERING for a demo (visual polish, things unlikely to regress, things better caught by just playing the game)? Name them so we DON'T write them.
3. Which tests are FLAKY-RISK in Unity (scene load timing, frame-dependent, OnGUI, Time.timeScale, physics settle) and should be avoided or made deterministic? How?
4. EditMode (fast, no scene) vs PlayMode (slow, needs scene) — push as much as possible to EditMode. What's the minimum that genuinely needs PlayMode?
5. One-paragraph "ship-fast" recommendation: what to test NOW vs defer.

Be blunt and lean. This feeds an Opus synthesis.
