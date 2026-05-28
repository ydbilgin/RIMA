# Codex Dispatch: Import 5 New Tilesets + Floor BaseTile Bug Fix

## Context
PixelLab MCP'den 5 yeni tileset generate edildi:

| Folder name | Tileset ID | Pair | Type |
|---|---|---|---|
| `wall_path` | `8c154e37-8c0a-450a-82fd-126cc8b35c97` | wall ↔ path | floor-to-wall variant |
| `wall_rift` | `02a5a97b-9475-4bdb-b2e4-cde475068f4d` | wall ↔ rift | floor-to-wall variant |
| `path_rift` | `ecfee0a0-a5ec-4992-b435-1f1d3ae2dfdb` | path ↔ rift | floor-to-rift variant |
| `rubble_moss` | `9591f35a-2373-4150-b737-7b4620d1834c` | rubble ↔ moss | **zemin↔zemin (transition_size=0)** |
| `pink_cream` | `ea19bab2-fea4-4c36-b5ef-6db1d103cc74` | pink ↔ cream | **zemin↔zemin (Alabaster Dawn aesthetic)** |

Her tileset PixelLab API endpoint'lerinden indirilmeli:
- PNG: `https://api.pixellab.ai/mcp/tilesets/<id>/image`
- Metadata: `https://api.pixellab.ai/mcp/tilesets/<id>/metadata`

## Görev 1: 5 Tileset'i İndir

Her tileset için:

### 1a. PNG ve JSON indir
PixelLab API key gerekli olabilir — eğer environment'ta `PIXELLAB_API_KEY` varsa onu kullan. Yoksa MCP server (eğer Codex CLI'ında PixelLab MCP varsa) `get_topdown_tileset` ile getirmeyi dene.

**Alternatif (önerilen):** `mcp__pixellab__get_topdown_tileset(<id>)` çağırıp dönen image (base64 veya URL) ve metadata'yı al, dosyaya yaz.

Hedef path: 
- `Assets/Art/Tiles/F1/Tilesets/{folder_name}/spritesheet.png`
- `Assets/Art/Tiles/F1/Tilesets/{folder_name}/tileset_meta.json`

### 1b. Yeni JSON formatını eski formatla uyumlu yap
PixelLab API yeni format döner (`tiles[].corners`). Bizim mevcut RebuildAllWangTilesets eski format (`cornerKeyToSpriteIndex`) bekliyor. 

İki seçenek:
- **A) Yeni format'ı eskiye dönüştür** (geriye uyum kolay):
  ```csharp
  // tiles[].corners → cornerKeyToSpriteIndex array oluştur
  int[] lookup = new int[16];
  for (int i = 0; i < 16; i++) {
      var bbox = tiles_with_matching_corners(cornerKey=i).bounding_box;
      int spriteIdx = (bbox.y / 32) * 4 + (bbox.x / 32);
      lookup[i] = spriteIdx;
  }
  ```
- **B) RebuildAllWangTilesets'i yeni format'ı da destekleyecek şekilde extend et**

Recommended: **A** — minimal değişiklik.

### 1c. Eğer indirme tool yoksa: WebRequest fallback
Codex `using UnityEngine.Networking; UnityWebRequest.Get(...)` ile indir. Editor'de çalışır.

## Görev 2: 5 Tileset için Slice + Tile assets + SO

`Assets/Editor/RebuildAllWangTilesets.cs` mevcut. Bu script şu 5 folder'ı işlemeli (mevcut 6'ya ek):
- wall_path, wall_rift, path_rift, rubble_moss, pink_cream

Script'i çağır → 80 yeni Tile asset + 5 yeni CornerWangTileSetSO oluşur.

Beklenen output:
- `Assets/Art/Tiles/F1/Generated/wang_wall_path_tile_0.asset` ... `_15.asset`
- `Assets/Art/Tiles/F1/Generated/WallPath_CornerWangTileSet.asset`
- (her 5 tileset için aynı pattern)

## Görev 3: BiomePreset Güncelle

`Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset` (RimaBiomePreset):

### 3a. Floor Terrain BaseTile BUG FIX
Mevcut: Floor terrain'in baseTile'ı mauve_hexagon tile gibi görünüyor (QC screenshot'tan).
DOĞRUSU: Floor baseTile = `wang_floor_wall_tile_0` (rubble floor visual).

### 3b. Yeni Terrain'ler Ekle (Faz 1 için minimal, sonra genişletilebilir)
Şu an Floor, Wall, Path, Rift var. Eklenecek:
- (Opsiyonel) Moss terrain ID 4 — baseTile = `wang_rubble_moss_tile_0`

### 3c. Yeni Pairing'leri Ekle (full mesh için)
```
ÖNCEDEN VAR:
- Floor (0) ↔ Wall (1)  → FloorWall_CornerWangTileSet
- Floor (0) ↔ Path (2)  → RubblePath_CornerWangTileSet
- Floor (0) ↔ Rift (3)  → DebrisRift_CornerWangTileSet

YENİ EKLENECEK (full mesh):
- Wall (1) ↔ Path (2)  → WallPath_CornerWangTileSet
- Wall (1) ↔ Rift (3)  → WallRift_CornerWangTileSet
- Path (2) ↔ Rift (3)  → PathRift_CornerWangTileSet

OPSIYONEL (moss):
- Floor (0) ↔ Moss (4) → RubbleMoss_CornerWangTileSet
```

Pink/cream tileset farklı biome'a ait (Alabaster Dawn) — F1'e KOYMA. Yeni biome preset oluştur: `Alabaster_Dawn_BiomePreset.asset` (Faz 2+ için iskelet).

## Görev 4: Test

### Test A — Import doğrulama
```csharp
foreach (var name in new[]{"wall_path","wall_rift","path_rift","rubble_moss","pink_cream"}) {
    string p = $"Assets/Art/Tiles/F1/Generated/{ToPascal(name)}_CornerWangTileSet.asset";
    var so = AssetDatabase.LoadAssetAtPath<RIMA.CornerWangTileSetSO>(p);
    Debug.Assert(so != null, name + " SO not created");
    int nullCount = 0;
    for (int i = 0; i < 16; i++)
        if ((so.tiles[i] as Tile)?.sprite == null) nullCount++;
    Debug.Assert(nullCount == 0, name + " has null tiles");
    Debug.Log(name + ": 16/16 ✓");
}
```

### Test B — BiomePreset full mesh
```csharp
var biome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>("Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset");
Debug.Assert(biome.terrains.Count >= 4, "Need at least 4 terrains");
Debug.Assert(biome.tilesetPairings.Count >= 6, "Need 6 pairings for full mesh");

// Floor baseTile check
var floor = biome.terrains.Find(t => t.id == 0);
Debug.Assert(floor.baseTile != null, "Floor baseTile null");
Debug.Log("Floor baseTile: " + floor.baseTile.name);
// Should be wang_floor_wall_tile_0 (rubble), NOT mauve_hexagon
```

### Test C — Visual paint test
Map Designer'da F1 biome seç, sırayla:
1. Wall paint (5,5) → wall tile gözükmeli (rubble base)
2. Path paint (8,8) → path tile  
3. Wall paint (5,8) + Path paint (8,5) — bu **2 terrain bordering** durumu → ana cellde wall_path pairing kullanılmalı
4. Apply to Scene → BaseTilemap'te tile count > 0
5. Screenshot al: `STAGING/qc_5tilesets_imported.png`

## Allowed Files
**Create:**
- Assets/Art/Tiles/F1/Tilesets/{wall_path,wall_rift,path_rift,rubble_moss,pink_cream}/spritesheet.png + tileset_meta.json
- Assets/Art/Tiles/F1/Generated/wang_{name}_tile_{0-15}.asset (80 dosya)
- Assets/Art/Tiles/F1/Generated/{PascalName}_CornerWangTileSet.asset (5 dosya)
- Assets/Art/Tiles/F1/Alabaster_Dawn_BiomePreset.asset (iskelet)

**Modify:**
- Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset (4 terrain + 6 pairing + Floor baseTile fix)
- Assets/Editor/RebuildAllWangTilesets.cs (5 yeni folder'ı destekle)

**DO NOT TOUCH:**
- Map Designer kod (Dispatch 1.6'da güzel oldu)
- CornerWangTileSetSO/Painter

## Commit
```
git add -A
git commit -m "[tilesets] Full mesh (3 pairings) + 2 floor-to-floor (moss, alabaster dirt) + Floor baseTile bug fix + BiomePreset extension"
```

Tahmini: 3-4h Codex.
