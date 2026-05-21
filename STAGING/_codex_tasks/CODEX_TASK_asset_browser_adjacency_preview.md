# Codex Task — Asset Pack Browser Adjacency Preview + Semantic Grouping

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Goal
Enhance existing `Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs` with:
1. **Adjacency preview** — when user selects/hovers a tile, show 3x3 grid demonstrating how it tiles with neighbors
2. **Semantic auto-grouping** — categorize PixelLab/Codex tiles into floor/path/transition/dirt/secondary groups based on filename or pool reference
3. **Larger hover preview** — pop-out larger image when mouse hovers a thumbnail (256×256 popup)

## Files to modify

### 1. `Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs`
Add UI section "Adjacency Preview" (right panel, below inspector):
- Renders 3×3 cell grid (256×256 total, each cell 85×85)
- Center cell = currently selected tile
- 8 surrounding cells = best-match neighbors from same category (if available) OR same tile repeated
- Optional checkbox: "Show 2×2 Wang preview" (for Wang-style transition tiles)

Add semantic grouping logic:
- Parse tile filename: `combat_floor_dominant_*` → "Floor Dominant", `combat_path_*` → "Path", etc.
- Auto-populate categories from filename patterns if AssetPackManifest doesn't already group them
- Display group counts: "Floor Dominant (3) | Path (8) | Transition (6)"

Add hover popup:
- 256×256 floating panel with tile detail when hover > 0.5s
- Shows tile name + size + source pack + variant index

### 2. `Assets/Editor/MapDesigner/AssetPackBrowserAdjacency.cs` (new helper)
Logic:
- `BuildAdjacencyMatrix(AssetPackEntry center)` — returns 8 neighbor entries
- `RenderAdjacency3x3(Rect rect, Sprite center, Sprite[8] neighbors)` — draws grid
- `RenderWang2x2(Rect rect, Sprite[4] corners)` — for Wang tiles

### 3. Test scaffold (optional)
`Assets/Tests/EditMode/MapDesigner/AssetPackBrowserAdjacencyTests.cs`:
- Verify semantic grouping pattern matches
- Verify adjacency builder returns 8 entries (or null fallback)
- Verify hover state transitions

## Acceptance
- Asset Pack Browser shows adjacency preview when tile selected
- Semantic auto-grouping visible in category dropdown
- Hover popup appears after 0.5s
- All existing tests still PASS
- DONE marker: `STAGING/CODEX_TASK_asset_browser_adjacency_preview_DONE.md`

## What NOT to do
- No replace Blueprint Painter integration
- No new ScriptableObject types (use existing AssetPackManifest)
- No Wang tile generation (separate task)
- No commit
- Surgical: only AssetPackBrowserWindow.cs + new adjacency helper + optional tests
