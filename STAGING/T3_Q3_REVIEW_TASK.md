# T3 Integration — Q3 REVIEW (ToolMain.unity scene built + wired)

ACTIVE RULES: (1) think before answering (2) terse (3) REVIEW ONLY — no edits (4) PASS/FAIL + evidence.

NLM ACCESS: not needed. Direct-read: Assets/Scenes/LiveTool/ToolMain.unity (YAML), Assets/Scripts/LiveTool/**, Assets/UI/LiveTool/**, STAGING/livetool_t3/REVIEW_AND_INTEGRATION.md.

## Context — Q3 just executed
Built `Assets/Scenes/LiveTool/ToolMain.unity` programmatically (additive, original scene untouched). Orchestrator's builder returned: all 6 ToolBootstrap fields wired (`uiDocument/previewCamera/grid/floorTilemap/cliffTilemap/propRoot = ok`), theme `UnityDefaultRuntimeTheme.tss` assigned to a new `Assets/UI/LiveTool/ToolPanelSettings.asset`, UXML loaded, camera ortho z=-10, Grid cellSize (1,1,1), tilemaps named `Floor_Tilemap`/`Cliff_Tilemap`, `[ToolRoot]`+ToolBootstrap, PropRoot, Directional Light. Console = 0 errors.

## Verify (PASS/FAIL each, with evidence from the .unity YAML)
1. **Reference integrity:** open `ToolMain.unity` YAML — do ToolBootstrap's 6 serialized fields point to the CORRECT in-scene components (uiDocument→the UIDocument, previewCamera→the Camera, grid→the Grid, floorTilemap→Floor_Tilemap's Tilemap, cliffTilemap→Cliff_Tilemap's Tilemap, propRoot→PropRoot Transform)? Any unassigned (fileID: 0) or cross-pointing?
2. **UIDocument:** does it reference the `ToolPanelSettings.asset` (with theme) + `ToolMain.uxml` source? Will `rootVisualElement` be non-null at play (so ToolBootstrap won't abort at the `uiDocument==null/root==null` guard)?
3. **Tilemap naming:** GO names contain `floor`/`cliff` (case-insensitive) for LiveRoomReloader discovery? Each has Tilemap + TilemapRenderer?
4. **Camera:** orthographic + z=-10 (B6 paint-projection correctness)?
5. **Registry precondition:** `Assets/Resources/Live/RuntimeAssetRegistry.asset` exists with >0 entries (else bootstrap aborts with banner)?
6. **Play-mode prediction:** if someone opens ToolMain.unity and hits Play, will ToolBootstrap.Start() complete (UI binds, no abort, no NRE)? Name any remaining trap.
7. **Anything missing** before Q4 (dual-build): is the scene self-sufficient, or does it still need something (e.g., scene not in any build list yet — is that correct for Q4)?

Terse, PASS/FAIL + .unity:line evidence. This gates Q4 (build + smoke).
