# Codex Dispatch 1: Smart Map Painter Faz 1 — Clean Slate

## Context
RIMA 2D top-down roguelite, Unity 6.3 LTS, URP 2D, PPU=64, 32×32 tiles, 35° ARPG perspective.

**Mevcut state (CLAUDE TEMIZLEDI):**
- `Assets/Art/Tiles/F1/Generated/` → SİLİNDİ (216 dosya gitti)
- 6 spritesheet .meta dosyası → SİLİNDİ (force reimport)
- `Assets/Art/Tiles/F1/Tilesets/{name}/` 6 klasörde **spritesheet.png + tileset_meta.json** var

**LOCKED:**
- Karar #131 corner Wang — algoritma DOĞRU, koru
- Karar #72 2D Pure — billboard YASAK
- Karar #126-130 — Faz 1.5, bu task'ta YOK

**REVIZE EDILEN tasarım:**
1. Per-layer independent vertex grid (her layer kendi grid'i)
2. Cell-paint default, Vertex toggle
3. Tileset palette (sol panel, 6 thumbnail)
4. Erase mode (red cursor)
5. Cliff Y-sort (9 cliff key, runtime CliffYSortManager)
6. Yeni JSON format support (`tiles[].corners` + `bounding_box`) — eski cornerKeyToSpriteIndex backward compat

**Tasarım LOCK:** `STAGING/smart_map_painter_design_LOCK.md` (oku, Q1-Q5 verdict orada)

---

## 6 Tileset Yapısı

```
Assets/Art/Tiles/F1/Tilesets/{name}/
├── spritesheet.png       (128×128, 4×4 grid = 16 tile, 32×32 each)
└── tileset_meta.json     (PixelLab metadata)
```

6 tileset isimleri:
- `floor_wall` (rubble↔broken wall) — ana boundary
- `rubble_path` (rubble↔stone path) — yol overlay  
- `debris_rift` (debris↔rift crack) — detay overlay
- `cold_floor_wall` (cold floor↔cold wall) — alternatif biome
- `slate_mineral` (slate↔mineral) — overlay
- `mauve_hexagon` (mauve↔hexagon) — alternatif biome

JSON formatı (OLD — backward compat):
```json
{
  "name": "floor_wall",
  "tileSize": 32,
  "lower": "rubble floor",
  "upper": "broken stone wall",
  "gridRows": 4, "gridCols": 4,
  "cornerKeyToSpriteIndex": [6,7,10,9,2,11,4,15,5,14,1,8,3,0,13,12]
}
```

Eski format yeterli. Yeni format opsiyonel — değiştirme gerek yok.

---

## Görev 0: Clean Slate Tileset İşleme

Her 6 tileset için:

### Adım 0a: Spritesheet'i Multiple Sprite Olarak Import Et

```csharp
string name = "floor_wall"; // tüm 6 için tekrarla
string spritesheetPath = $"Assets/Art/Tiles/F1/Tilesets/{name}/spritesheet.png";

var importer = (TextureImporter)AssetImporter.GetAtPath(spritesheetPath);
importer.textureType = TextureImporterType.Sprite;
importer.spriteImportMode = SpriteImportMode.Multiple;
importer.filterMode = FilterMode.Point;
importer.textureCompression = TextureImporterCompression.Uncompressed;
importer.mipmapEnabled = false;
importer.spritePixelsPerUnit = 32f;

int tileSize = 32, cols = 4, rows = 4;
var rects = new List<SpriteMetaData>();
for (int row = 0; row < rows; row++) {
    for (int col = 0; col < cols; col++) {
        int idx = row * cols + col;
        rects.Add(new SpriteMetaData {
            name = $"{name}_{idx}",
            rect = new Rect(col * tileSize, (rows - 1 - row) * tileSize, tileSize, tileSize),
            pivot = new Vector2(0.5f, 0.5f),
            alignment = (int)SpriteAlignment.Center
        });
    }
}
importer.spritesheet = rects.ToArray();
AssetDatabase.ImportAsset(spritesheetPath, ImportAssetOptions.ForceUpdate);
```

### Adım 0b: 16 Tile Asset Oluştur

```csharp
var allSprites = AssetDatabase.LoadAllAssetsAtPath(spritesheetPath)
    .OfType<Sprite>()
    .OrderByDescending(s => (int)s.rect.y) // top first
    .ThenBy(s => (int)s.rect.x)            // left first
    .ToArray();

// allSprites[0] = top-left, allSprites[15] = bottom-right (row-major)

string genFolder = "Assets/Art/Tiles/F1/Generated/";
Directory.CreateDirectory(genFolder);

int[] cornerKeyToSpriteIndex = meta.cornerKeyToSpriteIndex; // read from JSON

for (int cornerKey = 0; cornerKey < 16; cornerKey++) {
    var tile = ScriptableObject.CreateInstance<Tile>();
    tile.sprite = allSprites[cornerKeyToSpriteIndex[cornerKey]];
    AssetDatabase.CreateAsset(tile, $"{genFolder}wang_{name}_tile_{cornerKey}.asset");
}
```

### Adım 0c: CornerWangTileSetSO Oluştur

```csharp
string pascalName = ToPascal(name); // floor_wall → FloorWall
var so = ScriptableObject.CreateInstance<CornerWangTileSetSO>();
so.lowerTerrainLabel = meta.lower;
so.upperTerrainLabel = meta.upper;
so.tiles = new TileBase[16];
for (int cornerKey = 0; cornerKey < 16; cornerKey++) {
    so.tiles[cornerKey] = AssetDatabase.LoadAssetAtPath<TileBase>(
        $"{genFolder}wang_{name}_tile_{cornerKey}.asset");
}
AssetDatabase.CreateAsset(so, $"{genFolder}{pascalName}_CornerWangTileSet.asset");
```

**Bu işi `Assets/Editor/RebuildAllWangTilesets.cs` MenuItem'a koy:**
```
[MenuItem("RIMA/Tools/Rebuild All Wang Tilesets")]
```

**Doğrulama (Adım 0d):**
Her 6 SO için `tiles[0..15]` hepsinin sprite=null OLMAMASI.

---

## Görev 1: PaintMode + Cell-Paint Hybrid

`RimaMapDesignerWindow.cs`:

```csharp
public enum PaintMode { Cell, Vertex }
[SerializeField] private PaintMode paintMode = PaintMode.Cell; // default Cell

// Toolbar'da:
paintMode = (PaintMode)GUILayout.Toolbar((int)paintMode, new[] { "Cell", "Vertex" });
```

Cell mode mantığı: Click yapılan cell'in 4 köşe vertex'ini `currentPaintValue`'ya set et:
```csharp
private void PaintCell(Vector2Int cellOrigin, int value) {
    PaintVertex(new Vector2Int(cellOrigin.x,     cellOrigin.y),     value); // SW
    PaintVertex(new Vector2Int(cellOrigin.x + 1, cellOrigin.y),     value); // SE
    PaintVertex(new Vector2Int(cellOrigin.x,     cellOrigin.y + 1), value); // NW
    PaintVertex(new Vector2Int(cellOrigin.x + 1, cellOrigin.y + 1), value); // NE
}
```

Hover preview:
- Cell mode: full cell rect highlight (cyan 30% alpha)
- Vertex mode: existing disc (no change)

---

## Görev 2: BrushInputHandler.cs

`Assets/Editor/BrushInputHandler.cs` (NEW):

```csharp
public class BrushInputHandler {
    public Vector2Int GetCellAtMouse(Vector2 mousePos, float cellSize, float canvasPadding, int roomH) {
        int x = Mathf.FloorToInt((mousePos.x - canvasPadding) / cellSize);
        int invertedY = Mathf.FloorToInt((mousePos.y - canvasPadding) / cellSize);
        int y = roomH - invertedY - 1;
        return new Vector2Int(x, y);
    }
    public bool IsValidCell(Vector2Int cell, int w, int h) =>
        cell.x >= 0 && cell.y >= 0 && cell.x < w && cell.y < h;
}
```

`RimaMapDesignerWindow.HandleGridInput` bu helper'ı kullansın. Mevcut Vertex logic kalır, Cell mode için ek branch.

---

## Görev 3: Per-Layer Independent Vertex Grid

**ŞU AN:** `vertGrid` paylaşılan field (`int[,] vertGrid`).

**OLACAK:** Her layer'ın kendi grid'i.

```csharp
[System.Serializable]
public class MapLayer {
    public string name = "Base";
    public Tilemap tilemap;
    public CornerWangTileSetSO tileSet;
    public bool enabled = true;
    [HideInInspector] public int[] flatVertexData; // serialized
    [System.NonSerialized] public int[,] vertGrid; // runtime
}
```

`EnsureInitialized` her layer için ayrı grid oluştursun:
```csharp
foreach (var layer in layers) {
    if (layer.vertGrid == null || ...)
        layer.vertGrid = new int[roomWidth + 1, roomHeight + 1];
}
```

`PaintVertex`, `PaintCell`, vs. ACTIVE LAYER'in grid'ini etkilesin:
```csharp
private void PaintVertex(Vector2Int v, int value) {
    var grid = layers[activeLayerIndex].vertGrid;
    if (!IsValidVertex(v)) return;
    grid[v.x, v.y] = Mathf.Clamp(value, 0, 1);
}
```

`DrawGridCanvas` aktif layer'ın grid'ini göstersin. Inactive layer'lar overlay olarak yarı saydam çizilsin (optional Faz 1.5).

`ApplyToScene`: her enabled layer için kendi grid'i ile `CornerWangPainter.Paint` çağrılsın.

`SaveMap` / `LoadMap`: tüm layer grid'lerini JSON'a serialize et:
```json
{
  "width": 20, "height": 15,
  "layers": [
    { "name": "Base", "tileSet": "FloorWall...", "vertexData": [...] },
    { "name": "Path", "tileSet": "RubblePath...", "vertexData": [...] }
  ]
}
```

`MapSaveData` class güncellensin.

---

## Görev 4: TilesetPaletteDrawer.cs

`Assets/Editor/TilesetPaletteDrawer.cs` (NEW):

```csharp
public class TilesetPaletteDrawer {
    private List<CornerWangTileSetSO> palette;
    
    public void Refresh() {
        var guids = AssetDatabase.FindAssets("t:CornerWangTileSetSO");
        palette = guids.Select(g => AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(
            AssetDatabase.GUIDToAssetPath(g))).ToList();
    }
    
    public CornerWangTileSetSO Draw(float width, CornerWangTileSetSO currentSelection) {
        CornerWangTileSetSO clicked = null;
        EditorGUILayout.LabelField("Tilesets", EditorStyles.boldLabel);
        foreach (var so in palette) {
            // 64x64 thumbnail, click → return so
            var coverTile = so.tiles?[15]; // All Wall as cover
            Texture preview = AssetPreview.GetAssetPreview(coverTile);
            bool isSelected = so == currentSelection;
            Color bg = isSelected ? new Color(0.3f, 0.6f, 1f, 0.3f) : Color.clear;
            Rect r = GUILayoutUtility.GetRect(width, 80f);
            EditorGUI.DrawRect(r, bg);
            if (preview != null) GUI.DrawTexture(new Rect(r.x + 8, r.y + 4, 64, 64), preview, ScaleMode.ScaleToFit);
            EditorGUI.LabelField(new Rect(r.x + 78, r.y + 30, width - 80, 18), so.name);
            if (GUI.Button(r, GUIContent.none, GUIStyle.none)) clicked = so;
        }
        return clicked;
    }
}
```

`RimaMapDesignerWindow.DrawLeftPanel`: ReorderableList'in ÜSTÜNE palette ekle. Click → `layers[activeLayerIndex].tileSet = clicked; Repaint();`

---

## Görev 5: Erase Mode

```csharp
[SerializeField] private bool eraseMode = false;
// Toolbar:
eraseMode = GUILayout.Toggle(eraseMode, "Erase", "Button");
```

Paint logic: `int actualValue = eraseMode ? 1 - currentPaintValue : currentPaintValue;`

Hover cursor: paint=cyan, erase=red. `DrawGridCanvas` içinde:
```csharp
Color cursorColor = eraseMode ? new Color(1f, 0.3f, 0.3f, 0.6f) : new Color(0f, 1f, 1f, 0.6f);
```

---

## Görev 6: CliffYSortManager.cs (Runtime)

`Assets/Scripts/Systems/Map/CliffYSortManager.cs`:

```csharp
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA {
    [RequireComponent(typeof(Tilemap))]
    [ExecuteAlways]
    public class CliffYSortManager : MonoBehaviour {
        public static readonly int[] CliffKeys = { 1, 2, 3, 5, 6, 7, 9, 10, 11 };
        public static readonly float[] YOffsetByKey = {
            0f, -0.5f, -0.5f, -0.5f, +0.5f, -0.25f, -0.25f, -0.5f,
            +0.5f, -0.25f, -0.25f, -0.5f, +0.5f, +0.5f, +0.5f, 0f
        };
        
        [SerializeField] public CornerWangTileSetSO tileSet;
        
        void Awake() => ApplySortMode();
        
        public void ApplySortMode() {
            var renderer = GetComponent<TilemapRenderer>();
            var tm = GetComponent<Tilemap>();
            if (tileSet == null || renderer == null || tm == null) return;
            
            int cliffCount = 0;
            var bounds = tm.cellBounds;
            for (int y = bounds.yMin; y < bounds.yMax; y++)
                for (int x = bounds.xMin; x < bounds.xMax; x++) {
                    var tile = tm.GetTile(new Vector3Int(x, y, 0));
                    if (tile == null) continue;
                    int key = GetCornerKey(tile);
                    if (System.Array.IndexOf(CliffKeys, key) >= 0) cliffCount++;
                }
            
            renderer.mode = cliffCount > 0 
                ? TilemapRenderer.Mode.Individual 
                : TilemapRenderer.Mode.Chunk;
        }
        
        public int GetCornerKey(TileBase t) {
            if (tileSet?.tiles == null) return -1;
            for (int i = 0; i < 16; i++) if (tileSet.tiles[i] == t) return i;
            return -1;
        }
        public bool IsCliffCell(Vector3Int pos) {
            var tm = GetComponent<Tilemap>();
            return System.Array.IndexOf(CliffKeys, GetCornerKey(tm.GetTile(pos))) >= 0;
        }
    }
}
```

`RimaMapDesignerWindow.ApplyToScene`: her enabled layer için tilemap GameObject'ine CliffYSortManager AddComponent if not present, tileSet ata, ApplySortMode çağır.

---

## Görev 7: TilemapMutator.cs (Refactor)

`Assets/Editor/TilemapMutator.cs` (NEW):

```csharp
public static class TilemapMutator {
    public static void ApplyVertexGrids(List<MapLayer> layers, int w, int h) {
        int painted = 0;
        foreach (var layer in layers) {
            if (!layer.enabled || layer.tilemap == null || layer.tileSet == null) continue;
            CornerWangPainter.Paint(layer.tilemap, layer.tileSet, layer.vertGrid, w, h);
            EditorUtility.SetDirty(layer.tilemap);
            painted++;
        }
        Debug.Log($"[TilemapMutator] Applied {painted} layers");
    }
}
```

`RimaMapDesignerWindow.ApplyToScene` bu sınıfı kullansın (kod azalır).

---

## Görev 8: SÜREKLİ TEST (zorunlu, iteratif)

**Her büyük adımdan sonra TEST:**

### Test 1 — Görev 0 sonrası (clean slate slice)
```csharp
// execute_code:
foreach (var name in new[]{"floor_wall","rubble_path","debris_rift","cold_floor_wall","slate_mineral","mauve_hexagon"}) {
    string p = $"Assets/Art/Tiles/F1/Generated/{ToPascal(name)}_CornerWangTileSet.asset";
    var so = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(p);
    int nullCount = 0;
    for (int i = 0; i < 16; i++) {
        if (so.tiles[i] == null) nullCount++;
        else if ((so.tiles[i] as Tile)?.sprite == null) nullCount++;
    }
    Debug.Log($"{name}: null tiles = {nullCount}/16");
    Assert.AreEqual(0, nullCount, name + " has null sprites!");
}
```

### Test 2 — Görev 1-2 sonrası (Cell-paint)
1. Open Map Designer
2. New room (20×15)
3. Active layer → FloorWall SO ata
4. Cell mode → click cell (5, 5)
5. Verify: vertex (5,5), (6,5), (5,6), (6,6) hepsi 1
6. Apply to Scene → BaseTilemap'te 1 cell wall tile olmalı

### Test 3 — Görev 3 sonrası (Per-layer)
1. Layer 1 (Base) → FloorWall
2. Layer 2 (Path) → RubblePath
3. Active Layer 1 → middle area wall paint (cell mode)
4. Active Layer 2 → diagonal path paint
5. Apply → iki tilemap ayrı paint olmalı
6. Save/Load → JSON'da 2 layer grid

### Test 4 — Görev 6 sonrası (Cliff Y-sort)
1. Player prefab spawn (5, 5, 0)
2. Wall row at y=10 (south face cliff)
3. Move player y=8 → render in FRONT of wall
4. Move player y=12 → render BEHIND wall
5. Verify TilemapRenderer.mode = Individual on wall tilemap

### Test 5 — Final E2E
1. Open _FazMVP_Demo
2. Map Designer → Load dungeon_main.json (mevcut)
3. Apply → tüm zemin + duvar görünür olmalı (yeni slice fix bunu sağlamalı)
4. Screenshot al (Game View, play mode)
5. Game View'de: dungeon room görünmeli, oyuncu spawnda, kamera takip ediyor
6. Görselde **siyah alan YOK** olmalı (eski bug)

### Test 6 — Visual screenshot QC
```csharp
ScreenCapture.CaptureScreenshot("STAGING/qc_dispatch1_final.png", 1);
```
Bu screenshot'ı commit'ten önce kontrol et. Tüm tile'lar dolu görünüyor mu?

---

## Allowed Files
**Modify:**
- Assets/Editor/RimaMapDesignerWindow.cs (major rewrite)

**Create:**
- Assets/Editor/RebuildAllWangTilesets.cs
- Assets/Editor/BrushInputHandler.cs
- Assets/Editor/TilesetPaletteDrawer.cs
- Assets/Editor/TilemapMutator.cs
- Assets/Scripts/Systems/Map/CliffYSortManager.cs

**Delete (cleanup):**
- Assets/Editor/CreateMissingWangSOs.cs (deprecated by RebuildAllWangTilesets)

**DO NOT TOUCH (Karar #131 LOCK):**
- Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs (sadece extend gerekirse minimal)
- Assets/Scripts/Systems/Map/CornerWangPainter.cs

---

## Compile + Commit

Her büyük görevden sonra:
1. read_console → 0 error
2. Manual execute_code test (Test 1-6 yukarıdaki)
3. Issues fix → re-test

**Final commit:**
```
git add -A
git commit -m "[map-designer Faz 1] Clean reslice 6 tilesets + Cell-paint + Palette + Per-layer + Erase + CliffYSort"
```

---

## QC Done Criteria
- [ ] 6 SO oluştu, hepsi tiles[0..15] sprite-dolu
- [ ] Map Designer açılıyor, palette 6 tileset thumbnail görünür
- [ ] Cell mode click → 4 vertex set + Wang transition render
- [ ] Per-layer grid çalışıyor (2 layer farklı şekil paint)
- [ ] Erase mode kırmızı cursor + invert paint
- [ ] CliffYSortManager runtime add ediliyor
- [ ] Final screenshot _FazMVP_Demo'da dungeon görsel olarak doğru (siyah alan yok)
- [ ] read_console 0 error 0 warning (script seviyesinde)

**Tahmini süre:** 5-7h Codex. **Iter 2 zorunlu** — ilk commit sonrası screenshot QC, hata varsa fix + amend YOK yeni commit.
