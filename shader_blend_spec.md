# Shader-Based Terrain Blending — Claude Code Spec

## Amac
Wang transition tile'lara gerek kalmadan, vertexGrid'den shader ile dogal terrain gecisi.
Mevcut `CornerWangPainter`'a toggle-bazli alternatif.

---

## Proje Bilgisi

- **Unity 6** / URP 17.3.0
- **Namespace:** `RIMA.Systems.Map` (runtime), `RIMA.MapDesigner` (editor)
- **Mevcut shader dizini:** `Assets/Shaders/`

## Mevcut Veri (DEGISTIRME)

```csharp
// RoomData struct (ProceduralRoomGenerator.cs)
public struct RoomData {
    public Vector2Int size;         // ornek: 20x15
    public int[,] vertexGrid;      // (width+1) x (height+1), her vertex = terrain ID (int)
    public int[,] terrainGrid;     // width x height, cell-bazli terrain
    public bool[,] walkable;       // width x height
}

// RimaBiomePreset.cs
public class RimaBiomePreset : ScriptableObject {
    public List<MapTerrain> terrains;           // id, name, baseTile, walkable
    public List<TilesetPairing> tilesetPairings;
}

// MapTerrain.cs
public class MapTerrain {
    public int id;
    public string name;
    public TileBase baseTile;
    public bool walkable;
}
```

## Entegrasyon Noktasi

`MapLayerOrchestrator.Paint()` satir 47-52:
```csharp
// MEVCUT (aynen kalacak, toggle ile secim eklenecek):
CornerWangPainter.Paint(floorTilemap, biome, room.vertexGrid, room.size.x, room.size.y, default, seed, false);
```

---

## 4 Dosya

### 1. `Assets/Shaders/TerrainBlend.shader` [NEW]

URP uyumlu fragment shader. Teknik:

**Bilinear Trick:** `_TerrainIndexTex` (R8 texture, FilterMode.Bilinear) uzerine vertexGrid terrain ID'lerini yaz. Bilinear filtering komsular arasi gradient otomatik olusturur. Bu gradient'i blend weight olarak kullan.

**Inputs:**
- `_TerrainIndexTex` (Texture2D, R8) — her pixel = terrainId / maxTerrainId
- `_TerrainTex0..3` (Texture2D) — tiling terrain doku texture'lari
- `_NoiseTex` (Texture2D) — kenar kirma icin noise
- `_BlendWidth` (float, 0.1-1.0)
- `_BlendSharpness` (float, 1-20)
- `_NoiseScale` (float)
- `_NoiseStrength` (float, 0-0.5)
- `_TerrainTiling` (float)
- `_MaxTerrainId` (float) — normalize icin

**Fragment pseudocode:**
```hlsl
float rawId = tex2D(_TerrainIndexTex, uv).r * _MaxTerrainId;
int idA = floor(rawId);
int idB = ceil(rawId);
float blend = frac(rawId);
blend += (tex2D(_NoiseTex, worldUV * _NoiseScale).r - 0.5) * _NoiseStrength;
blend = saturate(blend);
float4 colA = SampleTerrainById(idA, worldUV * _TerrainTiling);
float4 colB = SampleTerrainById(idB, worldUV * _TerrainTiling);
return lerp(colA, colB, blend);
```

`SampleTerrainById`: if/else veya Texture2DArray ile terrain texture sec.

**Sorting:** SortingLayer uyumlu, tilemap ile ayni seviyede renderlanmali.

### 2. `Assets/Scripts/Systems/Map/TerrainBlendConfig.cs` [NEW]

```csharp
[CreateAssetMenu(fileName = "TerrainBlendConfig", menuName = "RIMA/Terrain Blend Config")]
public class TerrainBlendConfig : ScriptableObject
{
    public Texture2D[] terrainTextures; // index = terrain ID, tiling capable
    public Texture2D noiseTexture;
    [Range(0.05f, 2f)] public float blendWidth = 0.5f;
    [Range(1f, 20f)] public float blendSharpness = 4f;
    [Range(0.5f, 8f)] public float terrainTiling = 1f;
    [Range(0f, 0.5f)] public float noiseStrength = 0.15f;
    [Range(0.1f, 4f)] public float noiseScale = 1f;
    public Material blendMaterial; // TerrainBlend shader'li material
}
```

### 3. `Assets/Scripts/Systems/Map/TerrainBlendRenderer.cs` [NEW]

MonoBehaviour. `Render()` metodu:

1. `room.vertexGrid` (int[,]) → `Texture2D` (R8, Bilinear) donustur
   - Her vertex = 1 pixel, value = terrainId / maxTerrainId
   - Boyut = (width+1) x (height+1)

2. Quad `Mesh` olustur (oda boyutunda, width x height unit)
   - UV 0-1 arasi

3. `MeshFilter` + `MeshRenderer` ata
   - Material = config.blendMaterial
   - SortingLayer = "Default" veya tilemap ile ayni

4. Material property'leri set et:
   - `_TerrainIndexTex`, `_NoiseTex`, `_BlendWidth`, `_BlendSharpness`
   - `_TerrainTex0..3` = config.terrainTextures'dan biome terrain ID'lerine gore

5. Position = tilemap origin ile hizala

`Clear()` metodu: mesh ve texture'i temizle.

### 4. `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs` [MODIFY]

Kucuk ekleme:
```csharp
// Yeni alanlar:
[SerializeField] private bool useShaderBlend = false;
[SerializeField] private TerrainBlendConfig blendConfig;

// Paint() icinde mevcut CornerWangPainter cagrisini toggle ile sar:
if (useShaderBlend && blendConfig != null)
{
    var renderer = EnsureComponent<TerrainBlendRenderer>(transform, "TerrainBlendRenderer");
    renderer.Render(room, biome, blendConfig);
}
else if (floorTilemap != null && biome != null)
{
    CornerWangPainter.Paint(floorTilemap, biome, room.vertexGrid, ...);
}
```

---

## Dokunulmayacak Dosyalar

`CornerWangPainter.cs`, `CornerWangTileSetSO.cs`, `TilesetPairing.cs`,
`RimaBiomePreset.cs`, `ProceduralRoomGenerator.cs`, `RoomData` struct — hepsi aynen kalsin.

---

## Test & Dogrulama

1. `TerrainBlendConfig` SO olustur, test texture'lari ata (basit renkli kare)
2. Noise texture ata (Perlin noise PNG)
3. Inspector'da `useShaderBlend = true` yap
4. Room generate et → shader blend gorunmeli
5. blendWidth/blendSharpness slider'lariyla ayarla

## FASE 2: Test Map + Karakter Yerlestirme

Shader blend calisdiktan sonra, asagidaki test sahnesini olustur.

### Sahne
- `Assets/Scenes/Demo/ShaderBlend_Test.unity` [NEW]
- Veya mevcut `Phase1_ProceduralMap_Test.unity` kullanilabilir

### Karakter
- **Prefab:** `Assets/Prefabs/Player.prefab`
- **Idle Sprite'lar:** `Assets/Resources/Characters/Warblade/` (8 yon)
- Sahneye Player prefab'ini yerlestir, zemin ortasinda
- WASD ile hareket edebilmeli (mevcut PlayerController var)

### Map Gereksinimleri — Dogal Gorunumlu Mekan

Basit grid degil, **dogal gorunen bir alan** olustur:

**Boyut:** 40x30 (genis, gezilecek alan)

**Terrain Dagilimi (Perlin noise ile organik sekiller):**
- **Tas zemin (Floor, id:0)** — ana yurume alani, merkezde genis
- **Cimen (Moss, id:4)** — kenarlarda ve koseserde buyuk organik yamalar
- **Patika (Path, id:2)** — merkezden kenarlara uzanan 2-3 tile genisliginde yollar
- Terrain'ler arasi gecisler shader blend ile yumusak olmali — sert cizgi OLMAMALI

**Layout fikri:**
```
  [cimen]  [cimen]          [cimen]
    ~~~    ~~~               ~~~
      ~~--patika--~~       ~~
         |        |       |
    cimen| TAS    |patika |  cimen
         | ZEMIN  |       |
      ~~--patika--~~    ~~
    ~~~    ~~~          ~~~
  [cimen]     [cimen]
```
- Ortada genis tas zemin (combat alani)
- Patikalar kuzey-guney ve dogu-bati yonunde kesisir
- Cimenler organik bloblar halinde kenarlarda
- Gecisler yumusak, dogal, noise ile kirilmis

**ONEMLI:** `PaintOrganicSecondary()` fonksiyonu zaten Perlin noise ile organik terrain dagilimi yapiyor (ProceduralRoomGenerator.cs satir 139-159). Bu fonksiyonu kullan veya benzer noise-based dagilim uygula.

### Beklenen Sonuc
- Play'e basinca Warblade zeminde gorunur
- WASD ile hareket eder
- Tas↔cimen, tas↔patika, cimen↔patika gecisleri **dogal ve yumusak**
- Duz cizgi yok — noise ile kirilmis organik kenarlar
- Hades/Alabaster Dawn hissi: temiz ama canli zemin

### Karakter Sprite Dizini
```
Assets/Resources/Characters/Warblade/
  warblade_idle_north.png
  warblade_idle_south.png
  warblade_idle_east.png
  warblade_idle_west.png
  warblade_idle_NE.png
  warblade_idle_NW.png
  warblade_idle_SE.png
  warblade_idle_SW.png
```

### Mevcut Sahne Referanslari
- `Assets/Scenes/Phase1_ProceduralMap_Test.unity`
- `Assets/Scenes/Demo/RoomPipelineTest.unity`
- `Assets/Scenes/Demo/_FazMVP_Demo.unity`

---

## Tilesetter Notu

Tilesetter'in MCP server'i yok. Shader blend transition tile ihtiyacini tamamen kaldiriyor.

