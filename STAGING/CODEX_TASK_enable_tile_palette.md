# Codex Task — Enable Unity Tile Palette Direct Paint Workflow

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Goal
Enable Unity's STANDARD Tile Palette workflow alongside Blueprint Painter. Designer should be able to:
1. See the actual rendered Tilemap
2. Pick a tile from a palette window
3. Paint directly on the Tilemap with immediate visual feedback
4. Use erase/bucket-fill/rectangle/line tools

Currently RIMA uses Blueprint Painter (semantic zone abstraction) only. User wants direct paint capability alongside it.

## User context
User feedback: "unity map designerda direkt görüp boyama işleminin de eklenmesini istiyorum blueprintle değil bunu sağlayamaz mıyız cidden?"

Answer: YES, Unity Tilemap Extras supports this natively. Just need to import RIMA tiles as Unity Tile assets + create a Tile Palette asset + open Window > 2D > Tile Palette.

## Files to create/modify

### 1. Import PixelLab tiles as Unity Tile assets
Source: `STAGING/pixellab_tiles_pro_pilot/` (16 cobble tiles) + `STAGING/pixellab_dirt_v1/` (16 dirt tiles)

Steps:
- Import PNGs as Sprites (16x16 pixel grid, point filter, no compression, PPU=32)
- For each Sprite: Right-click > Create > 2D > Tiles > Tile (creates Tile.asset)
- Output: `Assets/Data/Brush/AssetParts_v3/CombatBiome_v15g/Tiles/` (32 Tile.asset)
- Also create WeightedRandomTile.asset for groups: dominant pool, secondary pool, dirt pool (Unity Tilemap Extras)

### 2. Create Tile Palette asset
`Assets/Editor/TilePalettes/RIMA_Combat_v15g.prefab`:
- Standard Unity Tile Palette (Grid + Tilemap GameObject prefab)
- Drag all v15g Tiles into the palette
- Categorize: Floor / Path / Dirt / Transition

### 3. Documentation
`STAGING/UNITY_TILE_PALETTE_WORKFLOW.md`:
- How to open: Window > 2D > Tile Palette
- How to select palette: dropdown at top → "RIMA_Combat_v15g"
- How to paint: brush tool + click on Tilemap
- How to switch tile: click tile in palette
- How to erase/bucket/rect/line: toolbar buttons
- Best practice: Blueprint Painter for STRUCTURE, Tile Palette for POLISH

### 4. Optional: Quick-open menu item
`Assets/Editor/MapDesigner/Workflow/OpenTilePaletteMenu.cs`:
```csharp
[MenuItem("Tools/RIMA/Map Designer/Open Tile Palette")]
public static void OpenTilePalette() {
    EditorWindow.GetWindow(System.Type.GetType("UnityEditor.Tilemaps.GridPaintPaletteWindow,UnityEditor"));
}
```

## Acceptance
- Window > 2D > Tile Palette opens with RIMA_Combat_v15g palette available
- Designer can paint cobble/dirt tiles directly on a Tilemap
- All existing tests still PASS (no regression)
- DONE marker `STAGING/CODEX_TASK_enable_tile_palette_DONE.md` with: tile asset count, palette path, workflow doc path

## What NOT to do
- No replace Blueprint Painter (keep both, complementary)
- No remove AutoPopulator
- No new ScriptableObject for tile management (use Unity standard)
- No Wang autotile rules yet (those need RuleTile setup, separate task)
- No commit
- No PixelLab/Codex generation calls
