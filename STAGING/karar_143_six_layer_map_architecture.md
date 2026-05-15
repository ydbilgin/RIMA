# Karar #143 — 6-Aşamalı Map Mimarisi (Karar #135 REVIZE)

**Date:** 2026-05-15 S82 gece (S82+ ek netleştirme: production sırası + tileset rolü daraltma + Hades-style wall)
**Status:** LOCK adayı (kullanici onayi sonrasi MASTER_KARAR'a yazilir)
**Supersedes:** Karar #135 (Procedural+Paint+Organic Hybrid — 5 katman)
**Reason:** Wang tileset her şeyi basinca grid-evident "bayağı" görünüm; duvar/zemin semantic farkı yok; patch'ler duvar üstüne düşüyor

---

## S82+ Production Sırası (kullanıcı LOCK)

**Faz sırası — kesin:**

1. **AŞAMA 1 — 6-katman düz floor pipeline LOCK** (tileset YOK, sadece L1-L6 düz floor)
   - L1+L2 düz floor base + variation
   - L3 wall overlay (Hades-style perimeter cap)
   - L4+L5+L6 patch / detail / accent
   - **Bu aşama tek başına "doğal görünümlü oda" hedefine ulaşmalı.** Tileset olmadan çalışsın.

2. **AŞAMA 2 — Tileset (derinlik için)** Aşama 1 sonrası
   - Tileset = sadece **floor→water** + **floor→yükseklik (elevation up/down, cliff)** transition için
   - "Biome boundary" / "iç biome variation" tileset için KULLANILMAZ
   - Pair E (rubble↔cliff_drop) + Pair F (rubble↔rift_pool) + benzeri ihtiyaca göre üretilir

3. **AŞAMA 3 — Wall karakterizasyonu (kapalı dungeon hissi)**
   - Hades dungeon kenarı kapanması mantığı: map'in çevresindeki "boşluk" siyah/karanlık değil, **wall sprite cluster** ile doldurulur
   - Wall = tileset değil, SpriteRenderer overlay brush (Karar #143-C zaten bunu söylüyor)
   - Perimeter polyline boyunca büyük organik brush sprite'ları

---

## Eski (Karar #135) — Sorunlar

- Wang tileset MAP'IN HER ŞEYINI basiyor (duvar + zemin + transition)
- PatchOverlay tilemap.HasTile() check yapiyor → duvar tile'i da "has tile" döner → patch duvar üstüne düşer
- ScatterBrush aynı sorun
- Wall = Wang tile pattern = grid'li repetitive görünüm
- Map "tile-based stamp" hissi veriyor, hand-painted değil

---

## Yeni Mimari — 6 Katman

```
L1 — Floor Base (Tilemap, 32x32)
       │
       └─ Walkable cells only
          Simple readable stone tiles
          NO transitions, NO walls

L2 — Floor Variation (Tilemap, 32x32, aynı veya farklı tilemap)
       │
       └─ Random variant of same terrain
          Multi-variant pick within terrain ID
          Subtle stone changes only

L3 — Wall Overlay (SpriteRenderer, 128-512px) — HADES-STYLE PERİMETER CAP
       │
       └─ Room perimeter polyline boyunca yerleştirilir
          Büyük organik brush sprites
          256x128 horizontal, 128x256 vertical, 128x128 corners
          IRREGULAR EDGES — grid görünmez
          Doorway = gap (boşluk)
          MAP DIŞI BOŞLUK = bu wall cluster'lar ile doldurulur (Hades dungeon hissi)
          Wall = TILESET DEĞİL (S82+ LOCK), her zaman SpriteRenderer overlay

L4 — Transition Brush (SpriteRenderer, 256x256 / 512x512)
       │
       └─ Floor cells üstüne SADECE
          Biome transitions (moss patch on stone, dirt patch)
          Oval irregular shape
          Düşük yoğunluk (1-3/map)
          Center floor area'da sparse
          Wall yakını edge-biased

L5 — Detail Decals (SpriteRenderer, 32-128px)
       │
       └─ Floor cells üstüne SADECE
          Cracks, rubble, küçük moss tufts
          Edge-biased (corner + wall yakını dense)
          Center clean (gameplay path)
          High frequency, low intensity per item

L6 — Rift Accent (SpriteRenderer, 64-128px)
       │
       └─ Sparse magical traces
          Cyan rift cracks, corruption marks
          ONLY in flagged "rift_zone" rooms
          Asla overpower etmemeli (gameplay readability)
          Per-map 0-3 instance
```

---

## Wang Tileset Yeni Rolü — Sadece Derinlik Transition (S82+ DARALTILDI)

**Wang KULLANILACAK (sadece 2 kategori):**
- ✓ **Floor → Water** transition (floor ile water pool kenarı arasında)
- ✓ **Floor → Yükseklik (elevation up/down)** transition (cliff drop, ledge)

**Wang KULLANILMAYACAK:**
- ✗ Floor base (L1 simple tile yeter)
- ✗ Wall (L3 overlay brush yeter — Hades-style cap, tileset değil)
- ✗ İç biome variation (L2 multi-variant yeter)
- ✗ Biome boundary (S82+ kararı — biome geçişleri L4 transition brush ile, tileset YOK)
- ✗ Cross-style same-family pair (rubble↔moss gibi — L4/L5 patch yeter)

**Mevcut 11 Wang tileset'ten Faz 1'de aktif kalacaklar (DARALTILDI):**
- `DebrisRift_CornerWangTileSet` — Floor→rift_pool (water-type derinlik) — Pair F için referans
- `PathRift_CornerWangTileSet` — Special elevation/path için
- (Cliff_drop için yeni tileset üretilecek — Pair E queue'da)
- **Kalan 8-9 tileset → `_archive_faz1/` Faz 2+ değerlendirme**

**Tileset üretim sırası (Aşama 2):**
- Floor→Water pair: rubble↔water_pool veya benzeri (Pair F revize)
- Floor→Elevation pair: rubble↔cliff_drop (Pair E)
- Sonraki Faz: biome-specific water/elevation varyantları

---

## Wall/Floor Semantic Farkı (Tek Kural)

**Decoration painter'lari (L4, L5, L6) cell'e dokunmak için:**
1. Hücre TERRAIN ID'yi sorgular
2. Terrain.walkable=true ise → decoration ekle
3. Terrain.walkable=false ise → SKIP (wall, hazard, cliff)

Bu kural L1-L2 dışında her painter'da zorunlu. Wall/floor semantic ProceduralRoomGenerator output'unda her cell için bilinir.

---

## ProceduralRoomGenerator Output (Yeni Yapı)

```csharp
public struct RoomData {
    public Vector2Int size;
    public int seed;
    public int[,] terrainGrid;            // L1 + L2 source
    public bool[,] walkable;               // derived from biome.terrains
    public List<WallSegment> wallEdges;    // L3 perimeter — yeni
    public List<EncounterPlacement> encounters;
    public PatchAtlasSO transitionAtlas;   // L4
    public PatchAtlasSO decalAtlas;         // L5
    public PatchAtlasSO accentAtlas;        // L6
}

public struct WallSegment {
    public Vector2Int start;        // cell coord
    public Vector2Int end;
    public WallDirection direction; // N/S/E/W
    public bool isCorner;
    public bool isDoorway;
}
```

`wallEdges` listesi: room perimeter scan ile, walkable→non-walkable geçiş cells'lerinde segment olusturulur. WallOverlayPainter bu polyline'i SpriteRenderer brushes ile kaplar.

---

## Yeni Asset İhtiyacı (PixelLab)

### L3 — Wall Overlay (öncelik)

| Asset | Boyut | Adet | Tool | Credit |
|---|---|---|---|---|
| Wall horizontal (uzun) | 256x128 | 4 varyasyon | MCP `create_object` n=16 | 4 |
| Wall vertical | 128x256 | 4 varyasyon | MCP `create_object` n=16 | 4 |
| Wall corner (NE/NW/SE/SW) | 128x128 | 4 varyant | MCP `create_object` n=16 | 4 |
| Doorway gap | 128x96 | 2 | MCP `create_object` n=4 | 2 |
| **L3 toplam** | | **14** | | **14** |

### L4 — Transition Brush

| Asset | Boyut | Adet | Tool | Credit |
|---|---|---|---|---|
| Moss patch oval | 256x256 | 3 | MCP `create_object` n=16 | 3 |
| Dirt patch oval | 256x256 | 3 | MCP `create_object` n=16 | 3 |
| Large biome blend | 512x512 | 2-3 | **Web UI Pro** (MCP max 256) | manuel |
| **L4 toplam** | | **8-9** | | **~6 + manuel** |

### L5 — Detail Decals (mevcut atlas'i reuse + yeni)

| Asset | Boyut | Adet | Tool | Credit |
|---|---|---|---|---|
| Mevcut moss_2 + decal_2_moss_trail + decal_6_moss_curve | 64x64 | 3 (LIVE) | — | 0 |
| Crack lines (thin) | 64-128 | 3 | MCP `create_object` n=4 | 2 |
| Small rubble tufts | 32-64 | 3 | MCP `create_object` n=4 | 2 |
| **L5 toplam** | | **9** | | **4** |

### L6 — Rift Accent

| Asset | Boyut | Adet | Tool | Credit |
|---|---|---|---|---|
| Rift fracture thin | 64-128 | 2 | MCP `create_object` n=4 | 2 |
| Rift corruption blob | 128x128 | 1 | MCP `create_object` n=4 | 1 |
| **L6 toplam** | | **3** | | **3** |

**Faz 1 yeni asset toplam:** 34 sprite, ~27 PixelLab credit + 2-3 Web UI manuel

---

## Code Refactor (Codex dispatch)

### Yeni dosyalar
- `Assets/Scripts/MapDesigner/WallOverlayPainter.cs` (L3)
- `Assets/Scripts/MapDesigner/TransitionBrushPainter.cs` (L4 — `PatchOverlayPainter` yeniden adlandirilir + edge-biased mode)
- `Assets/Scripts/MapDesigner/DetailDecalPainter.cs` (L5 — yeni)
- `Assets/Scripts/MapDesigner/AccentPainter.cs` (L6 — yeni)
- `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs` (6 katman yöneticisi)
- `Assets/Scripts/Data/WallSegment.cs` (yeni struct)
- `Assets/Scripts/Data/WallBrushSetSO.cs` (L3 asset library)

### Değişen dosyalar
- `Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs` — wallEdges output
- `Assets/Scripts/Systems/Map/CornerWangPainter.cs` — sadece `featureEdge=true` pairing'de aktif
- `Assets/Scripts/Systems/Map/TilesetPairing.cs` — yeni field `bool isFeatureEdge`
- `Assets/Scripts/Data/PatchAtlasSO.cs` — yeni field `bool edgeBiased`, `float wallProximityFactor`
- `Assets/Editor/RimaMapDesignerWindow.cs` — 6-layer toggle UI

### Arşiv
- 8 Wang tileset SO `_archive_faz1/` altına tasinir (Faz 2 yeniden değerlendirilir)

---

## Edge-Biased Density Algorithm (L4 + L5)

```csharp
float DensityForCell(Vector2Int cell, RoomData room) {
    if (!room.walkable[cell.x, cell.y]) return 0f;
    
    // Distance to nearest wall
    int distToWall = ComputeDistanceToWall(cell, room);
    
    // Edge-biased curve:
    //   distToWall = 1 -> factor = 1.0 (high density at wall edge)
    //   distToWall = 2 -> factor = 0.6
    //   distToWall = 3 -> factor = 0.3
    //   distToWall >= 4 -> factor = 0.1 (center clean)
    float factor = distToWall <= 1 ? 1.0f
                 : distToWall == 2 ? 0.6f
                 : distToWall == 3 ? 0.3f
                 : 0.1f;
    
    return baseDensity * factor;
}
```

Center 4x4 zone (gameplay arena) effective density = 0.008 (neredeyse temiz). Wall yakını = 0.08 (zengin).

---

## Faz 1 MVP Roadmap (Karar #143 sonrası, S82+ revize)

### AŞAMA 1 — 6-katman düz floor pipeline (tileset YOK)

1. **Asset gen (PixelLab MCP dispatch):** 14 wall + 9 detail + 3 accent = 26 sprite
2. **Web UI 512x512 patch (kullanıcı eli):** 2-3 large biome blend (L4)
3. **Codex refactor:** 6-layer orchestrator + WallOverlayPainter + DetailDecalPainter + AccentPainter + edge-biased density (walkable mask zorunlu)
4. **Asset import + biome wire:** WallBrushSetSO + atlas update + map designer 6-layer test
5. **5-10 yeni map gallery (tileset KAPALI):** her biome için organik test
6. **LOCK Aşama 1:** Düz floor pipeline doğal görünüm doğrulandı

### AŞAMA 2 — Tileset (sadece floor→water + floor→elevation)

7. **Pair E (rubble↔cliff_drop):** PixelLab Pro tileset gen
8. **Pair F (rubble↔water_pool):** PixelLab Pro tileset gen
9. **Tileset import + Wang painter integration:** Map'in özel zone'larında aktif
10. **Test:** 3-5 map'te tileset zone (cliff/water) ile birlikte gallery

### AŞAMA 3 — LOCK + MASTER_KARAR

11. **Karar #143 MASTER_KARAR'a yazılır**, Karar #135 archived
12. **Production demo (2026-05-18):** Aşama 1+2 ile çalışan map

---

## Lockable Karar Maddeleri

**Karar #143-A:** 6-katman map mimarisi LIVE, Karar #135 5-katman archived
**Karar #143-B:** Wang tileset = SADECE floor→water + floor→elevation transition (S82+ DARALTILDI), %90 mevcut tileset archived
**Karar #143-C:** Wall = SpriteRenderer overlay brush, Hades-style perimeter cap (256x128 / 128x256 / 128x128), tileset DEĞİL
**Karar #143-D:** Decoration painter'ları walkable=true cell filter zorunlu (L4/L5/L6 hep)
**Karar #143-E:** Edge-biased density: wall yakını 10x, center 0.1x (gameplay readability)
**Karar #143-F:** L4 transition brush 256x256 MCP, L5 detail 32-128px MCP, L6 rift sparse 64-128px MCP
**Karar #143-G:** 512x512 large biome patch Web UI manuel (MCP max 256 kısıt)
**Karar #143-H:** Production sırası: Aşama 1 (düz floor pipeline LOCK) → Aşama 2 (tileset entegre) → Aşama 3 (production demo). Tileset Aşama 1 öncesi YOK.
**Karar #143-I:** "Biome boundary" / iç biome variation Wang tileset KULLANMAZ — L2 multi-variant + L4 patch yeterli (S82+ kararı)
**Karar #143-J:** Aşama 2 water/elevation feature zone'ları için pipeline: `NaturalFeatureGraph → RasterMask → Wang semantic boundary → L4/L5 smoothing/detail mask`. Voronoi/jittered nearest-site rasterizer (dependency-free, Burst gerekmez, 200x200 grid 1-5ms editor) sadece feature skeleton üretir. Temel combat grid ve walkable semantic L1-L2'de kalır. Site mode: uniform grid + jitter; pure random REJECT (combat readability bozar). Site sayısı 64-256 oda başına.
**Karar #143-K:** L5 detail density sadece wall proximity ile değil, **feature proximity** (river bank, cliff rim, rift crack, vb.) ile de çarpılır. Map Designer'da manuel paint + procedural Perlin mask kombinasyonu desteklenir. `FeatureMaskSO` (Texture2D alpha + AnimationCurve remap + invert flag) asset tip.
**Karar #143-L:** Cliff smoothing tekniği seçimi: **Tile-based Wang semantic boundary + Sprite-based L4/L5 overlay smoothing** kombinasyonu. Shader-based SDF approach Faz 1.5+ polish için defer. Sebep: pixel-perfect readability + Map Designer manuel düzeltme kolay + asset üretim makul.

### Aşama 2 Codex Implementation Task'ları (her biri 4-8 saat)

| Task | Scope | Files |
|---|---|---|
| **A** — NaturalFeatureGraph + Voronoi Water Mask | Feature generator | `Scripts/MapDesigner/NaturalFeatureGraph.cs`, `VoronoiWaterFeatureGenerator.cs`, `Data/NaturalFeatureSettingsSO.cs`, `Tests/...Tests.cs` |
| **B** — FeatureEdgeSmoothingPass (Pair E/F) | Wang overlay smoothing pass | `Scripts/MapDesigner/FeatureEdgeSmoothingPass.cs`, `Data/FeatureEdgeSmoothingProfileSO.cs`, `Editor/RimaMapDesignerWindow.cs` |
| **C** — FeatureMaskSO + DetailDecalPainter Integration | Vertex paint analog | `Data/FeatureMaskSO.cs`, `Scripts/MapDesigner/DetailDecalPainter.cs`, `PatchOverlayPainter.cs`, `Editor/RimaMapDesignerWindow.cs` |

**Acceptance kriterleri (tüm task'lar):**
- `walkable == false` cell'e asla decal/mask yazılmaz (Karar #143-D zorunlu)
- Deterministic seed: aynı seed = aynı output
- Map Designer'da seed lock + manuel erase/paint desteği
- 200x200 grid editor gen ≤ 20ms

Kullanici onayindan sonra MASTER_KARAR_BELGESI'ne #143-A..L eklenir, Karar #135 archive bölümüne.
