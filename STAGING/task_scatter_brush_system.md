# CODEX TASK — Scatter Brush System (Alabaster Dawn Style)

## Hedef
Unity Editor'da çalışan, grid'e bağlı olmayan serbest scatter brush sistemi.
Kullanıcı fırça boyutunu ayarlayıp sahneye tıklayıp sürükleyerek doğal görünümlü
zemin detayları (taş, yosun, moloz) ekleyebilir. AI "Generate" butonu noise tabanlı
otomatik dağılım üretir, kullanıcı üzerine boyar.

## Execute every step below:

---

### Step 1 — Klasör yapısı oluştur
```
Assets/Scripts/Editor/ScatterBrush/
Assets/Scripts/Runtime/Scatter/
Assets/Art/Scatter/Stones/       (placeholder için boş bırak)
Assets/Art/Scatter/Moss/         (placeholder için boş bırak)
Assets/Art/Scatter/Rubble/       (placeholder için boş bırak)
Assets/Art/Scatter/Dirt/         (placeholder için boş bırak)
```

---

### Step 2 — ScatterItem.cs (Runtime)
`Assets/Scripts/Runtime/Scatter/ScatterItem.cs`

```csharp
[System.Serializable]
public class ScatterItem
{
    public string category;       // "Stones", "Moss", "Rubble", "Dirt"
    public Sprite[] sprites;      // rastgele seçilecek varyantlar
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public bool randomRotation = true;
    public int sortingOrder = 5;  // Floor=0, Scatter=5, Wall=10, Prop=15
}
```

---

### Step 3 — ScatterDatabase.cs (ScriptableObject)
`Assets/Scripts/Runtime/Scatter/ScatterDatabase.cs`

ScriptableObject. ScatterItem listesi tutar (Stones, Moss, Rubble, Dirt kategorileri).
Asset path: `Assets/Art/Scatter/ScatterDatabase.asset`
Başlangıçta 4 kategori ekle, sprite array'leri boş bırak (sonradan doldurulacak).

---

### Step 4 — ScatterBrushWindow.cs (Editor Window)
`Assets/Scripts/Editor/ScatterBrush/ScatterBrushWindow.cs`

**Menu:** Tools > RIMA > Scatter Brush

**UI Elemanları:**
- ScatterDatabase referans alanı (ObjectField)
- Category seçici (toggle butonlar: Stones / Moss / Rubble / Dirt)
- Brush Radius slider: 0.5 - 8.0 (world units)
- Density slider: 0.1 - 1.0 (0.5 default)
- Min Scale / Max Scale float field
- "Generate Natural Map" butonu (Perlin noise ile otomatik dağıtım)
- "Clear Category" butonu (seçili kategorinin scatter objelerini siler)
- "Clear All" butonu

**Scene View Entegrasyonu:**
- Window açıkken `SceneView.duringSceneGui` event'ine abone ol
- Mouse sol tık + sürükleme → scatter paint mode
- Sol tık bırakınca stop
- Scene View'da fırça radius'unu gösteren circle handle çiz (Handles.DrawWireDisc)

**Paint Logic:**
Her mouse drag event'inde:
1. Brush radius içinde `density` değerine göre rastgele N nokta üret
2. Her nokta için:
   - Seçili kategoriden rastgele bir Sprite seç (database'den)
   - ScatterRoot/[Category] altına yeni GameObject oluştur
   - SpriteRenderer ekle, sprite ata
   - Random scale: minScale - maxScale arasında uniform
   - Random rotation: 0-360 (randomRotation true ise)
   - SortingLayer "Default", Order = kategori sortingOrder değeri
   - GameObject adı: "[Category]_scatter_[timestamp]"
3. Undo.RegisterCreatedObjectUndo ile Unity undo sistemine kaydet

**ScatterRoot Yönetimi:**
Sahnede "ScatterRoot" adlı GameObject yoksa oluştur.
Altında "Stones", "Moss", "Rubble", "Dirt" adlı child GameObject'lar.

---

### Step 5 — Natural Map Generator
ScatterBrushWindow içinde "Generate Natural Map" butonu:

```
Perlin noise tabanlı dağıtım:
- noiseScale = 0.15f
- Oda boyutu: mevcut sahnedeki FloorTilemap bounds'ından al
  (yoksa default 20x20 world units kullan)
- Her kategori için farklı noise offset:
  Stones:  offset (0, 0),    threshold > 0.6, density 0.3
  Moss:    offset (50, 50),  threshold > 0.5, density 0.4
  Rubble:  offset (100, 0),  threshold > 0.7, density 0.2
  Dirt:    offset (0, 100),  threshold > 0.45, density 0.5
- Grid step: 0.5 world units (her 0.5 birimde bir noise sample)
- Threshold geçilince o noktaya scatter objesi yerleştir
```

---

### Step 6 — Mevcut sahnede ScatterRoot kur
`Assets/Scenes/Demo/RoomPipelineTest.unity` sahnesini aç.
ScatterRoot GameObject'ı oluştur ve kaydet.

---

### Step 7 — Console kontrol ve kaydet
read_console ile hata var mı kontrol et.
Sahneyi kaydet.
CODEX_DONE.md'ye yaz: ne oluşturuldu, nasıl kullanılır (Tools > RIMA > Scatter Brush).

## Notlar
- Sprite'lar şimdilik boş — PixelLab'den gelince ScatterDatabase.asset'e atanacak
- Sorting: Floor tilemap=0, Scatter=5, Wall=10, Prop=15
- Proje: URP 2D, Pixel Perfect Camera, 32x32 tile grid ama scatter grid'den bağımsız
- Scatter objeleri Tilemap değil, normal SpriteRenderer GameObject
