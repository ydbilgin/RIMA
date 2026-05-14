# S74 Batch A — Data model + Auto-BiomePreset + Archive

**Effort:** medium
**Owner:** Codex via cx_dispatch
**Context:** Map Designer S73 multi-terrain refactor commit `19a4828` (already merged). Now adding PixelLab parity fields + auto-builder + archive. Reference: `STAGING/pixellab_map_export_analysis_LOCK.md`.

---

## TASK 1: TilesetPairing'e transition field'ları ekle

**File:** `Assets/Scripts/Systems/Map/TilesetPairing.cs`

Şu hâle getir:

```csharp
using RIMA;
using UnityEngine;

namespace RIMA.Systems.Map
{
    [System.Serializable]
    public class TilesetPairing
    {
        public int lowerTerrainId;
        public int upperTerrainId;
        public CornerWangTileSetSO tileSet;

        [Range(0f, 1f)]
        [Tooltip("PixelLab transition size: 0.0 = zemin↔zemin (no elevation), 0.1 = dar seam, 0.25 = organik wide blend, 0.5+ = çok geniş")]
        public float transitionSize = 0.25f;

        [TextArea(2, 4)]
        [Tooltip("PixelLab transition prompt — re-generation için referans")]
        public string transitionDescription = "";
    }
}
```

**Compile check:** RimaBiomePreset.cs ve CornerWangPainter.cs'i değiştirmen GEREKMEZ — additive. Mevcut .asset dosyalarındaki pairing'ler default value alacak (0.25 ve boş string), bu beklenen davranış.

---

## TASK 2: Auto-BiomePreset Builder Editor tool yaz

**Yeni dosya:** `Assets/Editor/AutoBiomePresetBuilder.cs`

**Menu:** `RIMA > Tools > Auto-Build BiomePreset from Tilesets`

**Davranış:**
1. EditorUtility.OpenFolderPanel ile tileset kök klasörü iste (default: `Assets/Art/Tiles/F1/Tilesets`).
2. Klasör altındaki tüm `tileset_meta.json` dosyalarını recursive bul (Directory.GetFiles).
3. Her meta için: `lower` ve `upper` terrain name string'lerini topla.
4. Distinct terrain name listesi oluştur. Alphabetic sort.
5. ID atama:
   - **Reserved:** id=0 her zaman "Floor" base'i için (bizim biome'da kullanılan ilk terrain — ÇOĞUNLUKLA "rubble floor" varyantı).
   - id=0 atanan terrain'i seçmek için: name'lerin içinde "rubble" veya "floor" geçen ilki id=0. Hiçbiri yoksa alfabetik ilk.
   - Geri kalanlar id=1, 2, ... alfabetik sırada.
6. RimaBiomePreset.terrains listesini doldur:
   - MapTerrain.id = atanan
   - MapTerrain.name = JSON'daki short name (eğer kısa, max 24 char varsa kullan; uzunsa keyword extract: "wall", "path", "rift", "moss", "cream" gibi tek kelime). Fallback: "Terrain " + id.
   - MapTerrain.paletteColor = tutarlı renk üret (Color.HSVToRGB(id * 0.137f % 1f, 0.55f, 0.75f)) — basit deterministic.
   - MapTerrain.baseTile = CornerWangTileSetSO bul (bu terrain'in lower veya upper olduğu ilk pairing'in tileset'i):
     - Eğer terrain pairing'de **lower** ise → tiles[0] (all-lower)
     - Eğer terrain pairing'de **upper** ise → tiles[15] (all-upper)
     - Önce upper olarak arandı → bulundu → tiles[15] (öncelik upper)
     - Bulunamadıysa null bırak.
7. RimaBiomePreset.tilesetPairings listesini doldur:
   - Her tileset_meta.json için bir TilesetPairing oluştur.
   - lowerTerrainId/upperTerrainId = terrain isim → atanan ID mapping.
   - tileSet = `Assets/Art/Tiles/F1/Generated/{PascalName}_CornerWangTileSet.asset` yükle (PascalName meta.name'i underscore'dan ayır, ToTitleCase et).
   - transitionSize = 0.0 if (lower terrain name'inde "floor" veya "rubble" VE upper terrain name'inde de "floor"/"rubble"/"moss"/"cream"/"path") çift-zemin paterni — heuristic; default 0.25.
   - transitionDescription = meta JSON'dan al (meta'da yoksa boş string).
8. Mevcut RimaBiomePreset asset seçtir (EditorUtility.OpenFilePanelWithFilters, .asset filter). Bulunan/seçilen asset'in `terrains` ve `tilesetPairings` field'larını üzerine yaz, SetDirty + SaveAssets.
9. Sonunda Debug.Log özetle: kaç terrain, kaç pairing, baseTile null sayısı.

**Önemli kısıtlar:**
- TilesetMeta JSON parse için RebuildAllWangTilesets.cs'teki TilesetMeta class'ı (WangTilesetBuilder static) zaten var → onu reuse veya kopya.
- Hiçbir mevcut field'ı override etme dışında biome asset'in başka field'ları (allowedFloorTiles, decalDensity vb.) korunmalı.
- Compile temiz olmalı, mevcut testleri kırma.
- Kullanım: kullanıcı `RIMA > Tools > Auto-Build BiomePreset from Tilesets` → klasör seç → biome asset seç → kayıt.

---

## TASK 3: Eski RimaRoomDesignerWindow arşivle

**Hedef:** `Assets/Editor/RoomDesigner/` klasörünün TAMAMI (alt klasörler ve .meta dosyaları dahil) `Assets/Editor/_archive_S73/RoomDesigner/` altına taşınsın.

Adımlar:
1. `Assets/Editor/_archive_S73/` klasörü yoksa oluştur (.meta de oluştur).
2. `Assets/Editor/RoomDesigner/` → `Assets/Editor/_archive_S73/RoomDesigner/` taşı. Tüm alt klasör ve dosyalar dahil.
3. **Önemli:** Unity .meta dosyalarını da taşı (asset GUID'leri korumak için). AssetDatabase.MoveAsset() kullanma — File.Move + AssetDatabase.Refresh() daha güvenli (Unity Editor execution).
   - Aslında BÖYLE YAP: Tek satır taşıma için `AssetDatabase.MoveAsset("Assets/Editor/RoomDesigner", "Assets/Editor/_archive_S73/RoomDesigner")` deneyebilirsin — başarısız olursa hata logla, manual file ops'a düş.
4. Move sonrası AssetDatabase.Refresh().
5. Compile errors çıkmasını bekliyoruz çünkü RoomDesigner içindeki script'ler artık `_archive_S73` namespace altında. Compile error'ları logla ve sustur:
   - Eğer hata bağıran external reference varsa (örneğin başka bir editor script `RimaRoomDesignerWindow` import ediyorsa), o referansı işaretle / KALDIRMA — sadece raporla.
6. Manual archive yapamıyorsan: alternatif olarak `RimaRoomDesignerWindow.cs` dosyasının başına `#if false` ekleyip sonuna `#endif` koyarak script'i devre dışı bırakabilirsin. Bu fallback yaklaşımı sadece taşıma başarısız olursa.

**Test:** Unity compile errors clean olmalı. RIMA > Tools menüsünde "Room Designer" görünmemeli.

---

## ÇIKTI KURALLARI

Her task için:
- Hangi dosyalar değişti
- Compile sonucu (errors/warnings — varsa düzelt)
- Test sonucu (Unity console log)

Sonunda tek commit:
```
[S74-A] PixelLab parity + Auto-BiomePreset + Archive RoomDesigner

- TilesetPairing: +transitionSize +transitionDescription
- AutoBiomePresetBuilder: Tools menu, scan tileset_meta.json folder, populate biome
- Archive: Assets/Editor/RoomDesigner/ → _archive_S73/
```

CODEX_DONE.md'ye yaz, sonuç özetle.
