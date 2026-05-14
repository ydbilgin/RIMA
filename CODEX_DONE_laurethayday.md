S74 Batch B complete.

Commit:
- 3c08ae4 [S74-B] Map Designer PixelLab-style UI redesign

Files changed:
- Assets/Editor/RimaMapDesignerWindow.cs
- Assets/Editor/BrushInputHandler.cs
- Assets/Editor/TilemapMutator.cs

Result:
- Removed MapLayer/ReorderableList usage from RimaMapDesignerWindow.
- Switched Map Designer to one terrainGrid and one output Tilemap.
- Rebuilt toolbar as New/Save/Load | Apply/Generate/Clear | Objects | Fit/Cell/Auto-Biome.
- Rebuilt left panel with New Biome/Edit Biome and 3-column tile thumbnail terrain palette.
- Rebuilt right panel with erase toggle, brush slider, Advanced foldout, Procedural foldout, hidden default Cell mode, and Advanced-only Vertex mode.
- Added two-line status bar with off-canvas tips and optional mouse debug text.
- Rounded cellSize and canvas hover rectangles for pixel-aligned mouse feedback.
- Added Objects placeholder dialog for Faz 1.5.
- Updated TilemapMutator for the single terrainGrid/biome paint API.

Verification:
- dotnet build RIMA.Editor.csproj --no-restore: 0 errors.
- dotnet build Assembly-CSharp-Editor.csproj --no-restore: 0 errors.
- Unity MCP refresh/compile completed after final build.
- Unity console filtered checks: 0 entries for "error CS", "RimaMapDesignerWindow.cs", and "TilemapMutator.cs".
- Note: Unity console still produced MCP transport disconnect exception entries from the tool connection itself; no compile errors from this change.

Screenshot:
- Temp/codex_s74b_map_designer_updated.png
