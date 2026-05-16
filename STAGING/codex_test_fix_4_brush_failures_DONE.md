# Codex Done - 4 Brush Failure Fixes

### FeatureEdgeSmoothingTests
- Symptom (from CURRENT_STATUS): "2 vs 8" expected delta.
- Root cause: TEST was wrong. `Tilemap.GetUsedTilesCount()` counts distinct TileBase assets used by the tilemap, not occupied tile cells. The SUT placed one tile per boundary cell correctly; the assertion measured the wrong thing.
- Side fixed (TEST or SUT or BOTH): TEST.
- Diff summary (1-3 bullets, file:line):
  - `Assets/Tests/EditMode/Editor/FeatureEdgeSmoothingTests.cs:56` now asserts occupied cell count instead of unique tile asset count.
  - `Assets/Tests/EditMode/Editor/FeatureEdgeSmoothingTests.cs:146` adds the Tilemap before TilemapRenderer per S86 test fixture pattern.
  - `Assets/Tests/EditMode/Editor/FeatureEdgeSmoothingTests.cs:157` adds `CountOccupiedCells` using `tilemap.HasTile` over `cellBounds`.
- Verified PASS: yes.

### FeatureMaskSOTests
- Symptom (from CURRENT_STATUS): density issue.
- Root cause: TEST was wrong. It generated natural features with `featureSiteRatio = 1f`, so every site was a feature site and the helper could not reliably find a true far non-feature comparison cell. That made near/far density equal.
- Side fixed (TEST or SUT or BOTH): TEST.
- Diff summary (1-3 bullets, file:line):
  - `Assets/Tests/EditMode/Editor/FeatureMaskSOTests.cs:61` uses deterministic near/far cells.
  - `Assets/Tests/EditMode/Editor/FeatureMaskSOTests.cs:86` builds a single-feature `NaturalFeatureGraphResult` fixture instead of random all-feature generation.
- Verified PASS: yes.

### HitPauseDriverTests
- Symptom (from CURRENT_STATUS): timing flake.
- Root cause: TEST was wrong for EditMode. The SUT coroutine zeros `Time.timeScale` immediately, but the EditMode test runner did not reliably advance the MonoBehaviour `WaitForSecondsRealtime` coroutine before the assertion.
- Side fixed (TEST or SUT or BOTH): TEST.
- Diff summary (1-3 bullets, file:line):
  - `Assets/Tests/EditMode/Editor/HitPauseDriverTests.cs:48` replaces the real-time wait test with direct enumerator completion of `PauseRoutine`.
  - `Assets/Tests/EditMode/Editor/HitPauseDriverTests.cs:50` uses reflection only inside the test to drive the private coroutine deterministically.
- Verified PASS: yes.

### NaturalFeatureGraphTests
- Symptom (from CURRENT_STATUS): perf budget 57ms vs 20ms.
- Root cause: SUT was wrong for the stated budget. `Generate` used full O(width * height * sites) Voronoi rasterization for generated jittered-grid sites, producing about 58-62ms for 200x200/256 sites in this editor.
- Side fixed (TEST or SUT or BOTH): SUT.
- Diff summary (1-3 bullets, file:line):
  - `Assets/Scripts/MapDesigner/NaturalFeatureGraph.cs:112` routes generated jittered-grid sites through a generated-grid rasterizer.
  - `Assets/Scripts/MapDesigner/NaturalFeatureGraph.cs:269` adds `RasterizeGeneratedSiteGrid`, checking only local grid neighbors while keeping deterministic site indexing.
  - `Assets/Scripts/MapDesigner/NaturalFeatureGraph.cs:290` uses a fixed radius 2 neighbor search for generated site grids.
- Verified PASS: yes.

## Suite verification
- dotnet build: PASS (`dotnet build RIMA.Runtime.csproj`; root-level bare `dotnet build` is ambiguous because the repo contains multiple csproj files. `dotnet build RIMA.Tests.EditMode.csproj` also PASS.)
- EditMode 197/197 PASS: yes (current editor suite is 261/261 PASS, 0 failed, 0 skipped; runner progress showed 262 total with 1 inconclusive prefab-health probe: `_IsoGame scene bulunamadi`).
- Sprint 11 LIVE files touched: no (did not touch `Assets/Scripts/MapDesigner/Composition/*` or `Assets/Scripts/MapDesigner/WallOverlayPainter.cs`).
- Karar #143-D/E/K compliance maintained: yes (did not touch walkableMask, wallProximityCurve, or featureMaskMultiplier defaults/enforcement).
