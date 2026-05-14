ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethgame.md AS THE VERY LAST STEP.

# Codex Dispatch 1.6: Multi-Terrain Refactor + PixelLab Algorithm + Pixelorama Controls

## Context

**Sorun:** Mevcut Map Designer per-layer BINARY grid kullanıyor → her layer ayrı paint ediliyor → 2+ tileset üst üste binince **görsel çakışma** (user: "anlamsız duruyor"). PixelLab Maps farklı: **TEK grid, multi-terrain ID**.

**PixelLab export analizinden öğrendiğimiz:**
- 1 grid, her cell vertex'inde terrain ID (1, 2, 3, ...)
- Tileset'ler **terrain ÇİFTLERİ** (lowerTerrainId + upperTerrainId)
- Sistem her cell için corner terrain'lere bakar → uygun tileset'i bulur → Wang index hesaplar
- Cell'in 4 köşesinde 3+ terrain → **ERROR** (kırmızı işaret)

Locked:
- Karar #131 Corner Wang algoritma DOĞRU — sadece vertex value binary'den int terrain ID'ye genişliyor
- Karar #72 2D Pure
- Karar #129 BiomePreset (mevcut, extend edilecek)

## Hedef Mimari

```
RimaBiomePreset (SO, mevcut — extend edilecek)
 ├─ List<MapTerrain> terrains
 │    ├─ Terrain 0: "Floor" (base — paint edilmez, default)
 │    ├─ Terrain 1: "Wall"
 │    ├─ Terrain 2: "Path"
 │    └─ Terrain 3: "Rift"
 │
 └─ List<TilesetPairing> tilesets
      ├─ Pairing: lowerTerrain=0 (Floor), upperTerrain=1 (Wall), so=FloorWall_CornerWangTileSet
      ├─ Pairing: lowerTerrain=0 (Floor), upperTerrain=2 (Path), so=RubblePath_CornerWangTileSet
      └─ Pairing: lowerTerrain=0 (Floor), upperTerrain=3 (Rift), so=DebrisRift_CornerWangTileSet

Map vertex grid: int[w+1, h+1] — value = terrain ID
Per-cell render:
  1. Sample 4 corner terrain IDs
  2. unique terrains = set of distinct IDs
  3. if unique.Count == 1: render baseTile of that terrain
  4. if unique.Count == 2: find tileset pairing for those 2 terrains, compute corner Wang key, render
  5. if unique.Count >= 3: render ERROR overlay (red X)
```

---

## Görev 1: Veri Modeli Refactor

### 1a. MapTerrain class (NEW)
`Assets/Scripts/Systems/Map/MapTerrain.cs`:
```csharp
[System.Serializable]
public class MapTerrain {
    public int id;                  // 0=base, 1+=feature
    public string name;             // "Floor", "Wall", "Path"
    public Color paletteColor;      // editor palette display
    public TileBase baseTile;       // base "All [terrain]" tile (cornerKey=0 for that terrain alone)
    public CornerWangTileSetSO baseTileSource; // optional: SO whose tiles[0] = base
}
```

### 1b. TilesetPairing class (NEW)
`Assets/Scripts/Systems/Map/TilesetPairing.cs`:
```csharp
[System.Serializable]
public class TilesetPairing {
    public int lowerTerrainId;
    public int upperTerrainId;
    public CornerWangTileSetSO tileSet;
}
```

### 1c. RimaBiomePreset EXTEND (mevcut)
`Assets/Scripts/Systems/Map/RimaBiomePreset.cs` (modify):
- Mevcut field'lar KORUNUR (backward compat)
- EKLE:
  ```csharp
  public List<MapTerrain> terrains = new();
  public List<TilesetPairing> tilesetPairings = new();
  
  public TilesetPairing FindPairing(int t1, int t2) {
      foreach (var p in tilesetPairings)
          if ((p.lowerTerrainId == t1 && p.upperTerrainId == t2) ||
              (p.lowerTerrainId == t2 && p.upperTerrainId == t1))
              return p;
      return null;
  }
  
  public bool IsValidPair(int t1, int t2) => t1 == t2 || FindPairing(t1, t2) != null;
  ```

### 1d. F1 Shattered Keep preset güncelle
`Assets/Art/Templates/F1_ShatteredRuins.asset` (RimaBiomePreset):
- 3 terrain ekle: Floor (id=0, baseTile from FloorWall.tiles[0]), Wall (id=1), Path (id=2)
- 2 pairing: (0,1)→FloorWall, (0,2)→RubblePath
- (İsteğe bağlı 3. terrain: Rift (id=3), pairing (0,3)→DebrisRift — Faz 1.5)

---

## Görev 2: CornerWangPainter Refactor

`Assets/Scripts/Systems/Map/CornerWangPainter.cs` (modify):

Mevcut signature:
```csharp
Paint(Tilemap tilemap, CornerWangTileSetSO tileSet, int[,] vertGrid, int w, int h)
```

YENİ overload (yeni primary):
```csharp
Paint(Tilemap tilemap, RimaBiomePreset biome, int[,] terrainGrid, int w, int h)
```

Implementation:
```csharp
public static void Paint(Tilemap tilemap, RimaBiomePreset biome, int[,] terrainGrid, int w, int h) {
    tilemap.ClearAllTiles();
    var positions = new List<Vector3Int>();
    var tiles = new List<TileBase>();
    
    for (int y = 0; y < h; y++) {
        for (int x = 0; x < w; x++) {
            int nw = terrainGrid[x, y + 1];
            int ne = terrainGrid[x + 1, y + 1];
            int sw = terrainGrid[x, y];
            int se = terrainGrid[x + 1, y];
            
            var unique = new HashSet<int>(){ nw, ne, sw, se };
            TileBase tile = null;
            
            if (unique.Count == 1) {
                int id = unique.First();
                var terrain = biome.terrains.Find(t => t.id == id);
                tile = terrain?.baseTile;
            }
            else if (unique.Count == 2) {
                int lower = Math.Min(unique.First(), unique.Last());
                int upper = Math.Max(unique.First(), unique.Last());
                var pairing = biome.FindPairing(lower, upper);
                if (pairing?.tileSet != null) {
                    int nwBit = nw == upper ? 1 : 0;
                    int neBit = ne == upper ? 1 : 0;
                    int swBit = sw == upper ? 1 : 0;
                    int seBit = se == upper ? 1 : 0;
                    tile = pairing.tileSet.GetTile(nwBit, neBit, swBit, seBit);
                }
            }
            // else unique.Count >= 3: tile stays null (error case)
            
            if (tile != null) {
                positions.Add(new Vector3Int(x, y, 0));
                tiles.Add(tile);
            }
        }
    }
    tilemap.SetTiles(positions.ToArray(), tiles.ToArray());
}
```

Eski binary overload KORUNUR (backward compat, deprecated yorumla işaretle).

---

## Görev 3: Map Designer UI Refactor

`Assets/Editor/RimaMapDesignerWindow.cs`:

### 3a. Single Grid, Multi-Terrain
```csharp
// Mevcut: List<MapLayer> layers (her layer kendi binary grid)
// Yeni:
[SerializeField] private RimaBiomePreset activeBiome;
[NonSerialized] private int[,] terrainGrid;  // value = terrain ID
[SerializeField] private int activeTerrainId = 1;  // hangi terrain'le paint
```

EnsureInitialized:
```csharp
if (terrainGrid == null || terrainGrid.GetLength(0) != roomWidth+1 || terrainGrid.GetLength(1) != roomHeight+1) {
    terrainGrid = new int[roomWidth+1, roomHeight+1];
    // Default: tüm vertex = 0 (base terrain)
}
```

### 3b. Terrain Palette (Sol Panel)
ESKİ tileset palette KALDIRR. YENİ:
```csharp
private void DrawLeftPanel() {
    // Biome selector at top
    activeBiome = (RimaBiomePreset)EditorGUILayout.ObjectField("Biome", activeBiome, typeof(RimaBiomePreset), false);
    if (activeBiome == null) {
        EditorGUILayout.HelpBox("Assign a Biome preset.", MessageType.Info);
        return;
    }
    
    EditorGUILayout.LabelField("Terrains", EditorStyles.boldLabel);
    foreach (var terrain in activeBiome.terrains) {
        if (terrain.id == 0) continue; // base = not paintable
        
        bool isActive = activeTerrainId == terrain.id;
        Color prevBg = GUI.backgroundColor;
        GUI.backgroundColor = isActive ? terrain.paletteColor : prevBg * 0.7f;
        
        if (GUILayout.Button(terrain.name, GUILayout.Height(40f))) {
            activeTerrainId = terrain.id;
        }
        GUI.backgroundColor = prevBg;
    }
}
```

### 3c. Right Panel — Just Paint/Erase
```csharp
private void DrawRightPanel() {
    EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
    
    GUIStyle big = new GUIStyle(GUI.skin.button) { fixedHeight = 40f, fontSize = 12, fontStyle = FontStyle.Bold };
    
    Color prevBg = GUI.backgroundColor;
    GUI.backgroundColor = !eraseMode ? new Color(0.3f, 0.7f, 0.4f) : prevBg;
    if (GUILayout.Button("PAINT", big)) eraseMode = false;
    GUI.backgroundColor = eraseMode ? new Color(0.9f, 0.3f, 0.3f) : prevBg;
    if (GUILayout.Button("ERASE", big)) eraseMode = true;
    GUI.backgroundColor = prevBg;
    
    // Brush radius, paint mode, advanced foldout aynı kalsın
}
```

Paint logic: `eraseMode ? 0 : activeTerrainId` — eraseMode true ise terrain 0 (base) yap.

### 3d. Cell Render with Error Detection
DrawLiveTilePreviewCells:
```csharp
for (int y = 0; y < roomHeight; y++) {
    for (int x = 0; x < roomWidth; x++) {
        int nw = terrainGrid[x, y + 1];
        int ne = terrainGrid[x + 1, y + 1];
        int sw = terrainGrid[x, y];
        int se = terrainGrid[x + 1, y];
        var unique = new HashSet<int>(){ nw, ne, sw, se };
        Rect cellRect = CellToCanvasRect(new Vector2Int(x, y));
        
        TileBase tile = null;
        if (unique.Count == 1) {
            tile = activeBiome.terrains.Find(t => t.id == nw)?.baseTile;
        } else if (unique.Count == 2) {
            int lower = Math.Min(unique.First(), unique.Last());
            int upper = Math.Max(unique.First(), unique.Last());
            var pairing = activeBiome.FindPairing(lower, upper);
            if (pairing?.tileSet != null) {
                int nwBit = nw == upper ? 1 : 0;
                int neBit = ne == upper ? 1 : 0;
                int swBit = sw == upper ? 1 : 0;
                int seBit = se == upper ? 1 : 0;
                tile = pairing.tileSet.GetTile(nwBit, neBit, swBit, seBit);
            }
        }
        
        if (tile != null) {
            // Render tile via DrawTextureWithTexCoords (already implemented)
            var sprite = (tile as Tile)?.sprite;
            if (sprite != null) {
                Rect tc = new Rect(sprite.rect.x / sprite.texture.width, ..., ...);
                GUI.DrawTextureWithTexCoords(cellRect, sprite.texture, tc);
            }
        } else if (unique.Count >= 3) {
            // ERROR: 3+ terrains in cell
            EditorGUI.DrawRect(cellRect, new Color(1f, 0f, 0f, 0.5f));
            // Draw X mark
            Handles.color = Color.red;
            Handles.DrawLine(new Vector3(cellRect.x, cellRect.y), new Vector3(cellRect.xMax, cellRect.yMax));
            Handles.DrawLine(new Vector3(cellRect.xMax, cellRect.y), new Vector3(cellRect.x, cellRect.yMax));
        } else {
            // Pairing missing (only 2 terrains but no tileset)
            EditorGUI.DrawRect(cellRect, new Color(1f, 0.7f, 0f, 0.4f));  // orange warning
        }
    }
}
```

---

## Görev 4: Pixelorama-Style Canvas Controls

`HandleGridInput` ek:

```csharp
// Space + drag pan
if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Space) {
    spaceHeld = true; evt.Use();
}
if (evt.type == EventType.KeyUp && evt.keyCode == KeyCode.Space) {
    spaceHeld = false; evt.Use();
}
if (spaceHeld && evt.type == EventType.MouseDrag && evt.button == 0) {
    gridScroll -= evt.delta;
    evt.Use(); Repaint();
    return;
}

// +/- keys for zoom
if (evt.type == EventType.KeyDown) {
    if (evt.keyCode == KeyCode.Plus || evt.keyCode == KeyCode.KeypadPlus) {
        cellSize = Mathf.Clamp(cellSize + 4, 10, 80); Repaint(); evt.Use();
    }
    if (evt.keyCode == KeyCode.Minus || evt.keyCode == KeyCode.KeypadMinus) {
        cellSize = Mathf.Clamp(cellSize - 4, 10, 80); Repaint(); evt.Use();
    }
}
```

Toolbar'a "Fit to Window" buton ekle:
```csharp
if (GUILayout.Button("Fit", EditorStyles.toolbarButton, GUILayout.Width(40))) {
    float availW = position.width - LeftPanelWidth - RightPanelWidth - CanvasPadding * 2;
    float availH = position.height - ToolbarHeight - StatusHeight - CanvasPadding * 2;
    cellSize = Mathf.Floor(Mathf.Min(availW / roomWidth, availH / roomHeight));
    cellSize = Mathf.Clamp(cellSize, 10, 80);
    gridScroll = Vector2.zero;
}
```

Default values:
- `cellSize = 32f`
- `roomWidth = 16`, `roomHeight = 12` (PixelLab benzeri küçük başlangıç)

---

## Görev 5: Drag-Paint

`HandleGridInput`:
- MouseDown → first cell paint
- MouseDrag → if hovered cell != lastPaintedCell → paint new cell
- Track `lastPaintedCell` to avoid re-painting same cell

```csharp
private Vector2Int lastPaintedCell = new Vector2Int(-1, -1);

if (evt.type == EventType.MouseDrag && evt.button == 0 && !spaceHeld && !isPanning) {
    if (hoveredCell != lastPaintedCell && brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight)) {
        PaintCell(hoveredCell, currentTerrainValue);
        lastPaintedCell = hoveredCell;
        evt.Use(); Repaint();
    }
}
if (evt.type == EventType.MouseUp) lastPaintedCell = new Vector2Int(-1, -1);
```

---

## Görev 6: AI Generator Compatibility

Dispatch 2'nin generated templates (per-layer binary format) yeni multi-terrain format'a port et:
- `RoomTemplateGenerator.cs`: 8 template'i regenerate et, JSON yeni format (terrain ID grid)
- Eski format JSON'ları auto-convert: layer 0 binary → terrain 0/1, layer 1 binary → terrain 0/2, etc.
- `MapSaveData` yeni format:
  ```csharp
  public int[] terrainGrid;  // flat (W+1)*(H+1), terrain IDs
  public string biomePresetGuid;
  ```

---

## Görev 7: Test (Sürekli, zorunlu)

### Test A — Compile
```csharp
read_console: 0 error 0 warning
```

### Test B — Biome Preset Setup
```csharp
var biome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>("Assets/Art/Templates/F1_ShatteredRuins.asset");
Debug.Assert(biome.terrains.Count >= 3, "F1 needs 3 terrains");
Debug.Assert(biome.tilesetPairings.Count >= 2, "F1 needs 2 pairings");
Debug.Log("Biome OK: " + biome.terrains.Count + " terrains, " + biome.tilesetPairings.Count + " pairings");
```

### Test C — Paint Multi-Terrain
```csharp
var win = ...;
// Set biome, terrainGrid[5,5] = 1 (wall), terrainGrid[10,10] = 2 (path)
// ApplyToScene
// BaseTilemap'te 2 tile farklı tileset'ten gelmeli
```

### Test D — Error Detection
```csharp
// 3 terrain in one cell:
// terrainGrid[5,5] = 1 (wall), [5,6] = 2 (path), [6,5] = 3 (rift), [6,6] = 0 (base)
// Canvas'ta cell (5,5) RED X göstermeli
// Status bar: "ERROR: 4 terrains in cell"
```

### Test E — Drag Paint
Mouse simulate drag (10 cell yatay), verify all 10 cells terrain ID set.

### Test F — Pixelorama Controls
Space + drag → pan. + / - → zoom. Fit button → cellSize auto-set.

### Test G — Screenshot
```csharp
var w = Resources.FindObjectsOfTypeAll<RimaMapDesignerWindow>()[0];
w.Focus();
// Paint multi-terrain test pattern
// Editor screenshot via PowerShell:
var pos = w.position;
System.Diagnostics.Process.Start("powershell.exe", "-NoProfile -Command \"Add-Type -AssemblyName System.Drawing; $b = New-Object System.Drawing.Bitmap " + (int)pos.width + "," + (int)pos.height + "; $g = [System.Drawing.Graphics]::FromImage($b); $g.CopyFromScreen(" + (int)pos.x + "," + (int)pos.y + ",0,0,$b.Size); $b.Save('STAGING/qc_d16_final.png'); $g.Dispose(); $b.Dispose();\"");
```

---

## Allowed Files
**Create:**
- Assets/Scripts/Systems/Map/MapTerrain.cs
- Assets/Scripts/Systems/Map/TilesetPairing.cs

**Modify:**
- Assets/Scripts/Systems/Map/CornerWangPainter.cs (new overload, old kept)
- Assets/Scripts/Systems/Map/RimaBiomePreset.cs (extend)
- Assets/Editor/RimaMapDesignerWindow.cs (terrain palette, multi-terrain grid, drag-paint, Pixelorama controls)
- Assets/Editor/RoomTemplateGenerator.cs (regenerate 8 templates in new format)
- Assets/Art/Templates/F1_ShatteredRuins.asset (add terrains + pairings)

**DO NOT TOUCH:**
- Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs (KARAR #131 LOCK)
- Karakter/animation related files

## Commit
```
git add -A
git commit -m "[map-designer 1.6] Multi-terrain refactor + terrain compatibility validation + Pixelorama controls + drag-paint"
```

## QC Done Criteria
- [ ] F1_ShatteredRuins preset 3 terrain + 2 pairing içeriyor
- [ ] Map Designer terrain palette gösteriyor (Wall, Path butonları)
- [ ] Wall paint → wall tile (FloorWall'dan), Path paint → path tile (RubblePath'ten)
- [ ] Wall ve Path bir cell'in farklı köşelerinde → ERROR red X (3+ terrain)
- [ ] Space+drag pan, scroll zoom, +/- keys, Fit button çalışıyor
- [ ] Drag paint çalışıyor
- [ ] Editor screenshot QC pass
- [ ] read_console 0 error

Tahmini süre: 6-10h Codex.


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethgame.md AS THE VERY LAST STEP.