# PixelLab Map Export — Full Analysis (LOCKED Reference)
**Source:** `Untitled Map-export (2).zip` — kullanıcı tarafından PixelLab Map Tool web UI ile çizilen örnek harita
**Tarih:** 2026-05-14 S74
**Amaç:** Bizim Map Designer mimarisini PixelLab Map Tool ile bire bir karşılaştırmak, kalıcı reference olarak tutmak

---

## ZIP İçeriği (10 entry)

| Dosya | Boyut | Açıklama |
|---|---|---|
| `map.json` | 7.2 KB | Map config + terrain list + tileset list + overlays + objects refs |
| `map-composite.png` | 825 KB | 2432×1408 rendered composite (tüm map tek imaj) |
| `terrain-tiles/terrain-1.png` | 1.7 KB | Terrain 1 base tile (rubble) 32×32 |
| `terrain-tiles/terrain-2.png` | 1.7 KB | Terrain 2 base tile (wall) 32×32 |
| `terrain-tiles/terrain-3.png` | 2.0 KB | Terrain 3 base tile (path) 32×32 |
| `terrain-tiles-gallery.png` | 5.5 KB | 3 terrain base tile yan yana gallery |
| `tilesets/<long-name>.png` (x2) | ~28 KB | 128×128 4×4 spritesheet (rubble↔path, rubble↔wall) |
| `objects/manifest.json` | 564 B | Object placements (NPC/char/prop) |
| `objects/<uuid>.png` | 3.4 KB | Object sprite (warblade 120×120) |

---

## map.json Şeması

```jsonc
{
  "version": "1.0.0",
  "name": "Untitled Map",
  "exportDate": "ISO-8601",
  "engine": "pixellab-map-editor",
  "mapConfig": {
    "tileSize": 32,
    "dimensions": { "width": 76, "height": 44, "pixelWidth": 2432, "pixelHeight": 1408 },
    "boundingBox": { "minX": 8, "maxX": 83, "minY": 5, "maxY": 48 }
  },
  "terrains": [
    {
      "id": 1,
      "name": "<full PixelLab generation prompt>",   // ← Tüm prompt name'e gömülü
      "color": "#9ca3af",                            // ← Palette color (hex)
      "baseTileId": "<uuid>"
    }
    // ... daha fazla terrain
  ],
  "tilesets": [
    {
      "id": "<uuid>",
      "filename": "<long PNG name>",
      "lowerTerrain": "<prompt>",
      "upperTerrain": "<prompt>",
      "lowerTerrainId": 1,
      "upperTerrainId": 3,
      "transitionSize": 0.25,                        // ← KRİTİK: 0.0 zemin↔zemin, 0.1 dar, 0.25 organik
      "transitionDescription": "<edge prompt>",
      "tileSize": 32,
      "gridLayout": "4x4",
      "wangIndexMapping": "standard"                 // ← Standard corner-Wang lookup
    }
  ],
  "overlays": [],                                    // ← Decals/scatter layer (boş)
  "background": null                                 // ← Background fill (yok)
}
```

### ÖNEMLİ: Per-cell terrain grid YOK
PixelLab export'unda `cells[]` veya `terrainGrid[]` **YOK**. Authoritative output = `map-composite.png`. Re-edit için tüm grid kullanıcı UI'ında recreate edilmek zorunda. **Bizim avantajımız:** MapSaveData.terrainGrid var, re-edit mümkün.

---

## Bizim Mimari vs PixelLab — Eşleşme Tablosu

| Bizim | PixelLab | Durum |
|---|---|---|
| `RimaBiomePreset.terrains` (List<MapTerrain>) | `terrains` array | ✅ Aynı şekil |
| `MapTerrain.id` (int) | `id` (int) | ✅ |
| `MapTerrain.name` (kısa) | `name` (full prompt) | ⚠️ Biz kısa isim, onlar tam prompt |
| `MapTerrain.paletteColor` (Color) | `color` (hex) | ✅ |
| `MapTerrain.baseTile` (TileBase ref) | `baseTileId` (uuid) | ✅ |
| `RimaBiomePreset.tilesetPairings` (List<TilesetPairing>) | `tilesets` array | ✅ Aynı kavram |
| `TilesetPairing.lowerTerrainId/upperTerrainId` | `lowerTerrainId/upperTerrainId` | ✅ Aynı |
| `TilesetPairing.tileSet` (CornerWangTileSetSO ref) | `filename` (PNG path) | ✅ Equivalent |
| `cornerKeyToSpriteIndex` (16-entry array, JSON meta) | `wangIndexMapping: "standard"` | ✅ Aynı: standard Wang |
| `gridLayout: "4x4"` | `gridLayout: "4x4"` | ✅ |
| `tileSize: 32` | `tileSize: 32` | ✅ |
| MapSaveData.terrainGrid (int[]) | YOK | ⭐ Bizim avantaj |
| `transitionSize` | YOK | ⭐ Biz kayıt etmiyoruz |
| `transitionDescription` | YOK | ⭐ Biz kayıt etmiyoruz |
| `overlays[]` | YOK | ⚠️ Faz 1.5 scatter brush |
| `objects` manifest | YOK | ⚠️ Faz 1.5 NPC/prop placement |
| `boundingBox` | Fixed roomWidth×roomHeight | ⚠️ Minor: sub-region painting yok |

**Sonuç: Mimarimiz PixelLab ile %95 uyumlu.** Sadece overlay/object/transition-metadata eksik.

---

## Composite Analizi (visual)

Karakter: Warblade chibi (south facing) `objects` katmanında 120×120 piksel boyutta yerleştirilmiş.

Görsel gözlemler:
1. **Tile grid 32px** — yakından bakınca grid çizgileri görünüyor (bizdeki gibi)
2. **Path/Floor transition organik** — rubble↔path tileset'inde `transitionSize=0.25` ile daha geniş geçiş bölgesi, path kenarları curvi
3. **Wall transition keskin** — `transitionSize=0.1` ile daha dar seam, walls keskin gölgesiyle yükseliyor
4. **Layered Z-stacking** — Map kenarları "shelved" görünüyor; floor tile'ları painted area dışına da uzanıyor (padding effect, içeride en üst layer)
5. **Karakter görsel olarak duruyor** — object overlay rendered last, tilemap üstünde
6. **Bizim Moss problemi BURADA YOK** çünkü path/rubble palette uyumlu ve transition prompt'u "natural worn fade" diyor — kalite **prompt'tan geliyor**, mimariden değil

---

## Kritik Çıkarımlar

### 1. **PixelLab Map Tool görünümü tamamen erişilebilir bizim için**
Aynı 4x4 corner-Wang sprite + pairing model. Görsel kalite farkı:
- **Prompt mühendisliği** (tileset description'larında "must connect cleanly across boundaries" ifadeleri zorunlu)
- **transitionSize tuning** (0.1 = dar, 0.25 = organik, 0.5 = çok geniş)
- **Çoklu tileset compose** (rubble + wall pair + path pair = layered look)
- **Pro mode raggedness** (Web UI exclusive, MCP'de yok)

### 2. **RubbleMoss "gridli" görünüyor → 2 sebep**
- Moss yeşili rubble gri ile **palette kontrastı yüksek** (pink/cream gibi yumuşak değil)
- Prompt'ta "must blend organically across tile boundaries" ifadesi yoksa hard edge
- **Çözüm:** Re-generate, Pro mode raggedness 50%, prompt'ta organic blend keyword'leri

### 3. **Bizim Map Designer kayıt formatı GELİŞTİRMELİ**
PixelLab'da olmayan ama bizde olan: `terrainGrid` (per-cell terrain ID). **Bu avantajı kaybetmemeliyiz.**
PixelLab'da olan ama bizde olmayan: `transitionSize`, `transitionDescription` per pairing. **TilesetPairing'e ekle.**

### 4. **Object Layer Faz 1.5 için gerekli**
PixelLab `objects/manifest.json` + 120x120 sprite overlays. Bizim için: spawn points, NPC placements, prop overlays. **Faz 1.5 task.**

### 5. **PixelLab "boundingBox" desteği**
Sub-region painting (76×44 viewport, painted area 8-83 × 5-48). Bizde Fixed 16×12. **Optional Faz 1.5 enhancement.**

---

## Action Items (sıra ile)

### Hemen (S74)
- [x] PixelLab export tam analiz + reference doc (this file)
- [ ] **Auto-BiomePreset Builder** (RIMA > Tools): tileset_meta.json klasörü → BiomePreset otomatik kur
- [ ] **Eski RimaRoomDesignerWindow arşivle** (kullanıcı kafa karışıklığı)
- [ ] **TilesetPairing'e `transitionSize` + `transitionDescription` field ekle** (PixelLab parity)
- [ ] **RubbleMoss re-generate** (Pro mode raggedness 50%, organik blend prompt)

### Faz 1.5
- [ ] Multi-variant per Wang key (3-5 alternatif tile/key, hash shuffle)
- [ ] Object layer (NPC/prop placement)
- [ ] Sub-region painting (boundingBox support)
- [ ] Scatter brush (Karar #121 LOCKED implement)
- [ ] Background/overlay layers

---

## Reference Files (proje içinde)

- `Assets/Editor/RimaMapDesignerWindow.cs` — main grid editor (S73 LOCKED)
- `Assets/Editor/RebuildAllWangTilesets.cs` — tileset_meta.json → CornerWangTileSetSO importer
- `Assets/Scripts/Systems/Map/RimaBiomePreset.cs` — biome data
- `Assets/Scripts/Systems/Map/CornerWangPainter.cs` — Wang lookup + Tilemap paint
- `Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs` — 16-tile array + GetTile(nw,ne,sw,se)
- `Assets/Art/Tiles/F1/Tilesets/*/tileset_meta.json` — per-tileset PixelLab metadata
- `Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset` — F1 biome (5 terrain, 7 pairing)

---

## Notes for future agents

1. **DON'T re-extract this ZIP** — analysis is locked here. Extracted files at `C:\px2\` (gitignored).
2. **PixelLab Standard MCP mode is fine for most tilesets** — only zemin↔zemin (RubbleMoss, PinkCream) need Pro mode raggedness.
3. **Wang index mapping "standard"** = our `cornerKeyToSpriteIndex` (already validated, see RebuildAllWangTilesets line 156+).
4. **Long PixelLab filenames** = full prompt embedded in PNG name. We use short pascal case (`RubbleMoss_CornerWangTileSet.asset`) — no need to mimic.
