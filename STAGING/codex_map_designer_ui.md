# Codex Task: RIMA Map Designer EditorWindow

## Context

RIMA is a 2D top-down Unity roguelite using URP 2D Renderer. We have a Corner Wang tileset system where `CornerWangPainter.cs` paints a Unity Tilemap from a vertex grid (`int[,]`). We need a rich EditorWindow that lets designers (and agents) build maps visually.

**Depends on:** `CornerWangTileSetSO.cs` and `CornerWangPainter.cs` must already exist (another Codex task creates them). If they don't exist yet, create stubs so this compiles.

## File to Create

### `Assets/Editor/RimaMapDesignerWindow.cs`

Full EditorWindow. Namespace: `RIMA.Editor`. Menu: `RIMA > Tools > Map Designer`.

## Detailed Feature Spec

### Layout (3-column)

```
[Toolbar row: New | Save | Load | Apply to Scene | Clear All]
[Left 200px | Center (flex, scrollable grid) | Right 200px]
[Status bar at bottom]
```

### Left Panel — Layers
- Label: "Layers"
- `ReorderableList` of `MapLayer` entries (max 4)
- Each entry shows:
  - String name field ("Base", "Decal", etc.)
  - `ObjectField` for `Tilemap` (scene reference)
  - `ObjectField` for `CornerWangTileSetSO` asset
  - Enabled bool toggle
- [+] / [-] buttons to add/remove layers
- Clicking a layer row = selects active layer (highlighted)

### Center Panel — Vertex Grid Canvas
- Scrollable `ScrollView`
- Draw a grid of vertices: (roomWidth+1) × (roomHeight+1)
- Each vertex = a small circle (radius 5px)
  - value 0 = dark gray `#3A3A3A`
  - value 1 = sienna brown `#7A4A2A`
- Grid lines between vertices: light gray `#555555` at 0.5 alpha
- Hover effect: cyan circle outline on hovered vertex
- **Mouse interactions** (use `Event.current`):
  - Left click/drag → set vertex to currentPaintValue (wall=1 or floor=0)
  - Right click/drag → set vertex to opposite of currentPaintValue
  - Middle click → pan scroll view
- Cell size: 28px between vertices (adjustable with a slider in toolbar)

### Right Panel — Tools & Generation

**Paint Tools section:**
- Radio buttons: [Brush] [Fill] [Rectangle]
  - Brush: single vertex paint on click/drag
  - Fill: flood-fill same-valued connected region on click
  - Rectangle: click+drag selects a rect, release paints all vertices in rect
- Terrain toggle: [Floor (0)] [Wall (1)] — sets currentPaintValue

**Procedural Generation section** (foldout):
- `[Make Rectangular Room]` — border vertices = 1 (wall), interior = 0 (floor). Wall thickness: int field (default 2)
- `[L-Shape Room]` — fills a room with an L-shaped floor area
- `[Perlin Noise Fill]` — density slider 0..1, seed int field → randomize vertex values
- `[Corridor H]` / `[Corridor V]` — paints a horizontal/vertical floor corridor through the current map
- `[Clear → All Floor]` → set all to 0
- `[Fill → All Wall]` → set all to 1

**Room Size section:**
- Width int field (default 20, range 4..64)
- Height int field (default 15, range 4..64)
- `[Resize]` button — resizes vertex grid, preserving existing values where possible

### Toolbar
- `[New]` — confirm dialog, resets to empty 20×15 floor map
- `[Save]` — saves vertex grid as JSON to `Assets/RIMA_MapData/{name}.json`
- `[Load]` — loads a previously saved JSON
- `[Apply to Scene]` — calls `CornerWangPainter.Paint()` for each enabled layer using current vertex grid
- `[Clear All]` — clears all tilemaps assigned in layers
- Cell size slider: 16px..36px

### Status Bar
Format: `Room: {W}×{H} | Vertices: {W+1}×{H+1} | Active Layer: {name} ({tilesetName}) | Tool: {tool}`

## Data Structures (inner classes)

```csharp
[System.Serializable]
public class MapLayer
{
    public string name = "Base";
    public Tilemap tilemap;
    public CornerWangTileSetSO tileSet;
    public bool enabled = true;
}

[System.Serializable]
public class MapSaveData
{
    public int width, height;
    public int[] vertexData; // flattened (width+1)*(height+1) array
    public string[] layerNames;
}
```

## Apply Logic

```csharp
private void ApplyToScene()
{
    foreach (var layer in layers)
    {
        if (!layer.enabled || layer.tilemap == null || layer.tileSet == null) continue;
        CornerWangPainter.Paint(layer.tilemap, layer.tileSet, vertGrid, roomWidth, roomHeight);
    }
    Debug.Log($"[MapDesigner] Applied {roomWidth}×{roomHeight} map to {layers.Count} layer(s).");
}
```

## Implementation Notes

- Use `GUILayout` + `EditorGUILayout` (not UIToolkit — keep compatible with older Unity versions)
- For the scrollable canvas, use `GUI.BeginScrollView` + `GUI.EndScrollView`
- For vertex hit testing: `Vector2 mousePos` in scroll view space → nearest vertex = `Mathf.RoundToInt((mousePos.x - padding) / cellSize)` etc.
- `Repaint()` on every `MouseMove` event for hover effect
- Flood fill: simple BFS on vertex grid
- Rectangle tool: track `rectStart` Vector2Int on mouse down, draw the selection rect overlay, apply on mouse up

## Stub if CornerWangPainter missing

If `CornerWangTileSetSO` or `CornerWangPainter` don't exist yet, create minimal stubs that compile:

```csharp
// Stub — will be replaced by real implementation
namespace RIMA {
    public class CornerWangTileSetSO : UnityEngine.ScriptableObject { 
        public UnityEngine.Tilemaps.TileBase[] tiles = new UnityEngine.Tilemaps.TileBase[16]; 
        public UnityEngine.Tilemaps.TileBase GetTile(int nw, int ne, int sw, int se) => null;
    }
    public static class CornerWangPainter {
        public static void Paint(UnityEngine.Tilemaps.Tilemap tm, CornerWangTileSetSO ts, int[,] v, int w, int h, UnityEngine.Vector3Int o = default) { }
    }
}
```

## Steps

1. Create `Assets/Editor/RimaMapDesignerWindow.cs` with full implementation
2. Create `Assets/RIMA_MapData/` folder (with a placeholder .gitkeep)
3. Check Unity console for 0 errors
4. Open `RIMA > Tools > Map Designer` → verify window opens
5. Click `[Make Rectangular Room]` → verify grid shows floor/wall pattern
6. Click `[Apply to Scene]` with a Tilemap assigned → verify paint (log message OK if no tilemap)
7. Commit all

## Commit message

`[map-designer] RimaMapDesignerWindow — multi-layer vertex grid editor with Wang painting + procedural generation`
