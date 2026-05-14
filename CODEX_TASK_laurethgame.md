ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethgame.md AS THE VERY LAST STEP.

# Codex Task: 5 Example Map JSONs + UnityMCP Map Painting

## Context
RIMA 2D top-down roguelite, Unity 6, URP 2D.
Map Designer (Assets/Editor/RimaMapDesignerWindow.cs) Load butonu ile JSON yükler.
JSON format: `{"width":W,"height":H,"vertexData":[...],"layerNames":["Base"]}`
vertexData: 1D array size=(W+1)*(H+1), index=y*(W+1)+x, 0=floor 1=wall

Corner Wang vertex kuralı:
- Vertex(x,y)=1 → "wall" köşe
- Vertex(x,y)=0 → "floor" köşe
- Cell(cx,cy) tile'ı = 4 köşesinin kombinasyonu → 16 tile'dan biri

## Görev 1: 5 Map JSON Dosyası Oluştur

Klasör: Assets/RIMA_MapData/examples/ (oluştur yoksa)

Aşağıdaki 5 haritayı JSON olarak kaydet. Her biri için vertexData'yı hesapla ve dosyayı yaz.

### Map 1: small_room.json (20×15)
Basit dikdörtgen oda, 2 tile kalınlığında duvar:
- Vertex(x,y) = 1 eğer (x<=1 OR x>=19 OR y<=1 OR y>=14) else 0
- Sonuç: 16×11 iç alan

### Map 2: large_chamber.json (30×22)
Büyük oda, 2 tile duvar + ortada 4 sütun:
- Temel: Vertex = 1 eğer (x<=1 OR x>=29 OR y<=1 OR y>=21)
- Sütunlar (2×2 vertex blok, yani 1×1 cell duvar):
  - Sütun A: x∈[7,8], y∈[7,8] → 1
  - Sütun B: x∈[13,14], y∈[7,8] → 1  
  - Sütun C: x∈[7,8], y∈[14,15] → 1
  - Sütun D: x∈[13,14], y∈[14,15] → 1

### Map 3: corridor_cross.json (24×18)
Haç şeklinde koridor, duvarlar dışında her şey wall:
- Tüm grid önce 1 (duvar) yap
- Yatay koridor: x∈[0..24], y∈[7..11] → 0 (floor)
- Dikey koridor: x∈[10..14], y∈[0..18] → 0 (floor)

### Map 4: l_shape.json (24×20)
L şeklinde oda:
- Tüm grid önce 1 yap
- Alan A: x∈[2..22], y∈[12..18] → 0 (alt yatay)
- Alan B: x∈[2..10], y∈[2..18] → 0 (sol dikey)

### Map 5: dungeon_main.json (28×20)
Ana oda + 2 geçit:
- Tüm grid önce 1 yap
- Ana oda: x∈[5..23], y∈[4..16] → 0
- Sol geçit: x∈[0..5], y∈[8..12] → 0
- Sağ geçit: x∈[23..28], y∈[8..12] → 0

Tüm JSON'ları `layerNames: ["Base"]` ile kaydet.

## Görev 2: UnityMCP ile 3 Haritayı Sahneye Uygula

_FazMVP_Demo sahnesini yükle (zaten açıksa gerek yok).

### Kullanılacak SO'lar:
- FloorWall: `Assets/Art/Tiles/F1/Generated/FloorWall_CornerWangTileSet.asset`
- RubblePath: `Assets/Art/Tiles/F1/Generated/RubblePath_CornerWangTileSet.asset`
- DebrisRift: `Assets/Art/Tiles/F1/Generated/DebrisRift_CornerWangTileSet.asset`

### Görev 2a: BaseTilemap temizle, dungeon_main'i FloorWall ile uygula
```csharp
// execute_code:
var floorWallSO = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(
    "Assets/Art/Tiles/F1/Generated/FloorWall_CornerWangTileSet.asset");

// Find BaseTilemap
var tilemapGO = GameObject.Find("BaseTilemap");
var tilemap = tilemapGO?.GetComponent<Tilemap>();
tilemap?.ClearAllTiles();

// dungeon_main vertex grid (28×20)
int w=28, h=20;
int[,] grid = new int[w+1, h+1];
// fill all 1
for (int y=0;y<=h;y++) for (int x=0;x<=w;x++) grid[x,y]=1;
// main room
for (int y=4;y<=16;y++) for (int x=5;x<=23;x++) grid[x,y]=0;
// left passage
for (int y=8;y<=12;y++) for (int x=0;x<=5;x++) grid[x,y]=0;
// right passage
for (int y=8;y<=12;y++) for (int x=23;x<=28;x++) grid[x,y]=0;

CornerWangPainter.Paint(tilemap, floorWallSO, grid, w, h);
Debug.Log("[MapExamples] dungeon_main applied: " + tilemap.GetUsedTilesCount() + " tiles");
```

### Görev 2b: RubblePath SO ile ikinci layer (varsa GroundTilemap veya benzer)
Sahnede "RubbleTilemap" veya "GroundTilemap" isimli ikinci tilemap varsa:
```csharp
var rubbleSO = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(
    "Assets/Art/Tiles/F1/Generated/RubblePath_CornerWangTileSet.asset");
var rubbleTM = GameObject.Find("RubbleTilemap")?.GetComponent<Tilemap>()
             ?? GameObject.Find("GroundTilemap")?.GetComponent<Tilemap>();
if (rubbleTM != null && rubbleSO != null) {
    // overlay: only the passage areas get rubble path
    int w2=28, h2=20;
    int[,] grid2 = new int[w2+1, h2+1];
    for (int y=0;y<=h2;y++) for (int x=0;x<=w2;x++) grid2[x,y]=1;
    // passages only as floor
    for (int y=8;y<=12;y++) for (int x=0;x<=5;x++) grid2[x,y]=0;
    for (int y=8;y<=12;y++) for (int x=23;x<=28;x++) grid2[x,y]=0;
    CornerWangPainter.Paint(rubbleTM, rubbleSO, grid2, w2, h2);
}
```
Eğer ikinci tilemap yoksa bu adımı atla, sadece log'a yaz.

### Görev 2c: Sahneyi kaydet
```csharp
UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
```

## Okuyacağın dosyalar
- Assets/Scripts/Systems/Map/CornerWangPainter.cs (Paint metod signature için)
- Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs

## QC
read_console: 0 error doğrula.
Tile count log'u oku: 0'dan büyük olmalı.

## Commit
```
git add Assets/RIMA_MapData/ Assets/Scenes/
git commit -m "[map-examples] 5 dungeon map presets + dungeon_main applied to demo scene"
```


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_laurethgame.md AS THE VERY LAST STEP.