# Codex Dispatch 2: AI Room Generator (Hades-style, RIMA-özgü)

## Context
**ÖNCE Dispatch 1 başarılı bitmeli.** Bu task Dispatch 1'in çıktısına bağlı:
- `Assets/Editor/RimaMapDesignerWindow.cs` per-layer grid destekli
- 6 CornerWangTileSetSO mevcut + slice doğru

**Hedef:** User Map Designer'da **"Generate Room"** butonuna basınca AI-tasarımlı Hades-style oda layout'u çıksın. User üzerine boyayıp/genişletebilsin.

Hades referans:
- Asimetrik organik şekiller
- Ana chamber + side alcoves + corridor çıkışlar
- Boundary'de düzensizlik (köşeler kırık, kenarda çukur)
- Path overlay (zemin üzerinde belirgin yol)
- Elevation farkları (cliff'ler oda içinde)

RIMA-özgü Shattered Keep estetiği:
- Dark rubble base
- Stone walkway path (rubble_path overlay)
- Rift crack accent (debris_rift detay)
- Yıkılmış duvar boundary (floor_wall wall kısmı)

---

## Görev 1: Room Template Library

`Assets/RIMA_MapData/templates/` klasörü oluştur. 8 hand-curated template JSON kaydet:

Format (Dispatch 1'in MapSaveData format'ı):
```json
{
  "width": W, "height": H,
  "layers": [
    { "name": "Base", "tileSet": "FloorWall...", "vertexData": [...] },
    { "name": "Path", "tileSet": "RubblePath...", "vertexData": [...] }
  ]
}
```

**8 template tasarımı (Codex algoritma ile generate edecek):**

1. **`small_chamber_24x16.json`** — Tek oda, asimetrik corners (NW köşe oval, SE köşe kırık), 2 hücre wall thickness, ortada path overlay diagonal
2. **`large_hall_36x24.json`** — Geniş ana hall, ortada 2 sütun (2×2 wall ada), 2 side alcove (NW + SE), main path L-shape
3. **`corridor_junction_28x20.json`** — Haç şeklinde kavşak, merkez 8×8 oda, 4 koridor uzantı
4. **`l_shape_keep_30x24.json`** — L şeklinde oda, kısa kol 12×12, uzun kol 24×10, debris_rift accent köşede
5. **`broken_courtyard_32x28.json`** — Dış duvarda 3 yerde gediği (collapse), iç alan organik, path zigzag
6. **`twin_chamber_28x20.json`** — 2 oda yan yana, ince geçit ile bağlı, her odanın path overlay farklı yönde
7. **`crypt_aisle_24x32.json`** — Dar uzun (24×32), ortada sütun sırası, kenar nişler
8. **`rift_shrine_24x24.json`** — Kare oda, ortada debris_rift accent zone (5×5), path radyal

Codex bu 8 layout'u procedural hesaplasın (manuel JSON yazma):
- Base layer vertex grid (rubble + boundary wall)
- Path layer vertex grid (sadece path alanları)
- (Opsiyonel) Debris_rift accent layer

Asimetri için: deterministic Perlin noise (seed=template name hash), Karar #126 raggedness yaklaşımı:
- Boundary vertex'lerini noise ile ±1 cell yer değiştir (organic edge)
- Iç alan content alanlarını noise ile düzensiz kıl

---

## Görev 2: RoomGeneratorWindow.cs

`Assets/Editor/RoomGeneratorWindow.cs` (NEW):

Menu: `RIMA/Tools/Room Generator`

UI:
```
┌─ Room Generator ──────────────────┐
│ Template: [Dropdown: 8 templates] │
│ Seed:     [Random] [12345]        │
│ Variation: ●○○ Subtle             │
│             ○●○ Medium            │
│             ○○● Wild              │
│                                    │
│ [Preview]  [Generate → Map Desr]  │
└────────────────────────────────────┘
```

**Generate butonu:**
1. Selected template'i JSON'dan yükle
2. Variation slider'a göre Perlin noise apply et (boundary perturbation + path jitter)
3. Çıktıyı Map Designer'ın aktif sahnesine yükle (RimaMapDesignerWindow'a publish)
4. Map Designer açıksa direkt LoadMapData(generated) çağır
5. Map Designer kapalıysa otomatik aç + yükle

**Preview butonu:** mini 200×150 canvas'ta render et (renkli vertex grid).

---

## Görev 3: RimaMapDesignerWindow Entegrasyonu

`RimaMapDesignerWindow.cs`'e ekle:

**Toolbar yeni buton:**
```csharp
if (GUILayout.Button("Generate Room", EditorStyles.toolbarButton, GUILayout.Width(110f))) {
    RoomGeneratorWindow.Open(this); // pass reference so it can publish back
}
```

**Public publish method:**
```csharp
public void LoadFromGenerator(MapSaveData generated) {
    roomWidth = generated.width;
    roomHeight = generated.height;
    // load each layer's vertexData
    layers.Clear();
    foreach (var layerData in generated.layers) {
        var layer = new MapLayer { name = layerData.name };
        // find tileSet by name
        var so = FindTilesetByName(layerData.tileSetName);
        layer.tileSet = so;
        // create vertGrid from flatVertexData
        layer.vertGrid = new int[roomWidth+1, roomHeight+1];
        Unflatten(layerData.vertexData, layer.vertGrid);
        layers.Add(layer);
    }
    BuildLayerList();
    Repaint();
}
```

---

## Görev 4: RoomVariationProcessor.cs

`Assets/Editor/RoomVariationProcessor.cs` (NEW) — Perlin-based template varyasyonu:

```csharp
public static class RoomVariationProcessor {
    public enum Level { Subtle = 1, Medium = 3, Wild = 6 }
    
    public static void Apply(int[,] baseGrid, int w, int h, int seed, Level level) {
        Random.InitState(seed);
        float strength = (int)level * 0.15f;
        
        // 1. Boundary perturbation: outer wall vertices ±1 cell jitter
        for (int y = 0; y <= h; y++) {
            for (int x = 0; x <= w; x++) {
                if (IsBoundaryVertex(x, y, baseGrid, w, h)) {
                    float n = Mathf.PerlinNoise(x * 0.3f, y * 0.3f);
                    if (n < strength) Toggle(baseGrid, x, y);
                }
            }
        }
        
        // 2. Path layer: jitter overlay vertices
        // 3. Optional: scatter "natural ruins" (random 1-vertex wall blocks inside floor)
    }
}
```

`RoomGeneratorWindow.Generate`:
1. Load base template grids
2. Apply RoomVariationProcessor.Apply on each layer
3. Publish to MapDesigner

---

## Görev 5: Test

### Test 1: Template generation
```csharp
[MenuItem("RIMA/Tools/Test - Generate All Templates")]
static void TestAll() {
    foreach (var name in templateNames) {
        var data = LoadTemplate(name);
        Debug.Assert(data.layers.Count >= 2, name + " missing layers");
        Debug.Assert(data.layers[0].vertexData.Length == (data.width+1)*(data.height+1));
    }
    Debug.Log("All 8 templates valid");
}
```

### Test 2: Variation determinism
Same seed → same output. Different seed → different.

### Test 3: E2E
1. Open Map Designer
2. Click Generate Room → select "broken_courtyard_32x28"
3. Variation: Medium, Seed: 42
4. Generate → Map Designer canvas'ında oda görünmeli
5. Apply to Scene → BaseTilemap + PathTilemap iki ayrı tile pattern
6. Edit on top: cell mode → bir köşeye wall paint → 4 vertex set
7. Re-apply → user edit'i korunmuş olmalı
8. Screenshot al `STAGING/qc_dispatch2_final.png`

---

## Allowed Files
**Create:**
- Assets/Editor/RoomGeneratorWindow.cs
- Assets/Editor/RoomVariationProcessor.cs
- Assets/Editor/RoomTemplateGenerator.cs (8 template oluştur)
- Assets/RIMA_MapData/templates/*.json (8 dosya)

**Modify:**
- Assets/Editor/RimaMapDesignerWindow.cs (toolbar button + LoadFromGenerator method)

**DO NOT TOUCH:**
- Assets/Scripts/Systems/Map/ (runtime kod dokunulmaz)
- Dispatch 1'in oluşturduğu BrushInputHandler/CliffYSortManager/etc.

---

## Compile + Commit
```
git add -A
git commit -m "[room-generator] AI room template library (8 Hades-style) + RoomGeneratorWindow + variation processor"
```

---

## QC Done Criteria
- [ ] 8 template JSON oluştu, hepsi geçerli (vertex count doğru, 2+ layer)
- [ ] RoomGeneratorWindow açılıyor, dropdown 8 template gösteriyor
- [ ] Generate → Map Designer'a load oluyor
- [ ] Variation slider farklı seed/level farklı sonuç veriyor (deterministic)
- [ ] Generated room üzerine cell-paint çalışıyor (edit on top)
- [ ] Apply to Scene → tüm layer'lar düzgün render (siyah alan yok)
- [ ] Final screenshot dungeon Hades-feel veriyor

**Tahmini süre:** 4-6h Codex. **Iter 2 zorunlu** — screenshot QC.
