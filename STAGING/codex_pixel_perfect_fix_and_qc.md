# Codex Task: Pixel Perfect Camera Fix + Map Designer QC

## Context
RIMA 2D top-down roguelite, Unity 6, URP 2D. PPU=64, 32×32 tiles.

Play moduna basıldığında şu uyarı çıkıyor:
"Rendering at an Odd-numbered resolution (1081 × 751). Pixel Perfect Camera may not work properly."

Bu uyarı Game View penceresi tek sayılı boyutta olduğunda çıkar. PixelPerfectCamera `upscaleRT = true` yapılırsa, referans çözünürlüğünde render edip scale eder → window boyutundan bağımsız çalışır.

## Görev 1: Pixel Perfect Camera Fix

### Adım 1a — Sahneleri tara, PixelPerfectCamera bul
```
manage_scene / find_gameobjects ile PixelPerfectCamera component'ı olan obje bul.
Bakılacak sahneler (sırayla): _FazMVP_Demo, RoomPipelineTest, MainMenu, CharacterSelect.
```

### Adım 1b — upscaleRT = true yap
Her bulunan PixelPerfectCamera için:
```csharp
// execute_code ile:
using UnityEngine.U2D;
var cams = UnityEngine.Object.FindObjectsByType<PixelPerfectCamera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
foreach (var cam in cams) {
    cam.upscaleRT = true;
    UnityEditor.EditorUtility.SetDirty(cam);
}
UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
```

### Adım 1c — Player Settings default resolution
Project Settings > Player > Resolution:
- Default Screen Width: 1920
- Default Screen Height: 1080
- Fullscreen Mode: Windowed (if not already set for dev)
```csharp
UnityEditor.PlayerSettings.defaultScreenWidth = 1920;
UnityEditor.PlayerSettings.defaultScreenHeight = 1080;
```

## Görev 2: Map Designer QC

Map Designer'ın tüm yeni özellikleri çalışıyor mu kontrol et.

### Adım 2a — Compile check
read_console ile Assets/Editor/RimaMapDesignerWindow.cs için compile error yok doğrula.

### Adım 2b — SO varlık testi
Şu SO'ların hepsinin var olup olmadığını AssetDatabase.LoadAssetAtPath ile kontrol et:
- Assets/Art/Tiles/F1/Generated/FloorWall_CornerWangTileSet.asset
- Assets/Art/Tiles/F1/Generated/RubblePath_CornerWangTileSet.asset
- Assets/Art/Tiles/F1/Generated/DebrisRift_CornerWangTileSet.asset
- Assets/Art/Tiles/F1/Generated/ColdFloorWall_CornerWangTileSet.asset
- Assets/Art/Tiles/F1/Generated/SlateMineral_CornerWangTileSet.asset
- Assets/Art/Tiles/F1/Generated/MauveHexagon_CornerWangTileSet.asset

### Adım 2c — Tile asset varlık testi
wang_floor_wall_tile_0.asset ... wang_floor_wall_tile_15.asset (16 adet) var mı? Count doğrula.
wang_debris_rift_tile_0.asset ... 15 (16 adet) var mı?

### Adım 2d — DrawGridCanvas tile preview fix (varsa sorun)
RimaMapDesignerWindow.cs içinde DrawGridCanvas metodunu oku.
Tile preview kodu `(tile as Tile)?.sprite?.texture` kullanıyor, bu nullable. 
Null referans exception riski: `UnityEngine.Tilemaps.Tile` cast başarısız olabilir.
Eğer `using UnityEngine.Tilemaps` import eksikse veya null check yetersizse düzelt.

### Adım 2e — CornerWangPainter functional test
_FazMVP_Demo sahnesinde BaseTilemap GameObject'i bul.
FloorWall SO'yu yükle.
Basit bir 20×15 rectangular vertex grid oluştur (kenarlarda 1, içinde 0) ve CornerWangPainter.Paint() çağır.
Sonucu doğrula: tilemap tile count > 0.

```csharp
// execute_code örneği:
var so = AssetDatabase.LoadAssetAtPath<CornerWangTileSetSO>(
    "Assets/Art/Tiles/F1/Generated/FloorWall_CornerWangTileSet.asset");
var tilemapGO = GameObject.Find("BaseTilemap");
var tilemap = tilemapGO?.GetComponent<Tilemap>();
if (so == null || tilemap == null) { Debug.LogError("SO or tilemap null"); return; }

int w=20, h=15;
int[,] grid = new int[w+1, h+1];
for (int y=0;y<=h;y++) for (int x=0;x<=w;x++)
    grid[x,y] = (x<=1||x>=w-1||y<=1||y>=h-1) ? 1 : 0;

CornerWangPainter.Paint(tilemap, so, grid, w, h);
Debug.Log("Painted tile count: " + tilemap.GetUsedTilesCount());
```

## Okuyacağın dosyalar
- Assets/Editor/RimaMapDesignerWindow.cs (sadece DrawGridCanvas + DrawCenterPanel metodları, 50 satır)
- Assets/Scripts/Systems/Map/CornerWangPainter.cs

## Compile + Save
Her şeyden sonra: read_console 0 error. Dirty sahneleri kaydet.

## Commit
Değişiklik varsa:
```
git add -A
git commit -m "[qc] Pixel Perfect Camera upscaleRT fix + Map Designer functional test"
```
Değişiklik yoksa (zaten doğruysa) commit'e gerek yok, sadece raporla.
