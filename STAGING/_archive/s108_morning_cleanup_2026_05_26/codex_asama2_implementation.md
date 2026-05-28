# Codex Task: RIMA Karar #143 Aşama 2 — Voronoi NaturalFeatureGraph + Wang Smoothing + FeatureMaskSO

**Model:** gpt-5.5, effort=high
**Çıktı + commit:** Codex implementation + commit (kendi protokol)
**Süre tahmini:** 12-20 saat (3 task A+B+C combined)
**Bekleyen kaynak:** Karar #143 Aşama 1 LOCK 2026-05-15 (`STAGING/codex_asama1_implementation.md` DONE — MapLayerOrchestrator + 4 painter + walkable mask hazır)

---

## Bağlam

RIMA Aşama 1 LOCK: 6-katman düz floor pipeline çalışıyor (MapLayerOrchestrator, WallOverlayPainter, TransitionBrushPainter, DetailDecalPainter, AccentPainter, WallSegment, WallBrushSetSO, walkable filter, edge-biased density, EditMode tests PASS, dotnet build PASS).

**Bu task Aşama 2'yi implement eder:** Voronoi-tabanlı NaturalFeatureGraph + Wang semantic boundary smoothing + FeatureMaskSO ile density modulation. Hedef: organik su/uçurum/rift zone'ları + L4/L5 detail decal density'sini bu zonların yakınına çekmek.

## Spec Referansları

- `STAGING/karar_143_six_layer_map_architecture.md` — Karar #143-J/K/L (Aşama 2 spec)
- `STAGING/artofsully_voronoi_analysis_codex.md` — Voronoi/cliff/grass-mask pipeline analizi (library reco + cliff smoothing comparison)
- `TASARIM/MASTER_KARAR_BELGESI.md` — Karar #143-J/K/L LOCK satırları
- `F:\LaurethStudio\01_PIPELINE\layered_environment_pipeline.md` — Studio universal pattern (Sites → Organic Graph → Raster Mask → Semantic Boundary → Visual Smoothing → Detail Density)
- `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs` — Aşama 1 entrypoint
- `Assets/Scripts/MapDesigner/DetailDecalPainter.cs` — Aşama 1 painter (FeatureMaskSO ile genişletilecek)
- `Assets/Scripts/Systems/Map/CornerWangPainter.cs` — `allowFeatureEdges=true` parametresi mevcut

---

## Görev — 3 Task Combined Implementation

### Task A — NaturalFeatureGraph + Voronoi Water/Elevation Generator (Karar #143-J)

**Yeni dosyalar:**
- `Assets/Scripts/MapDesigner/NaturalFeatureGraph.cs` — site-based feature graph (jittered grid)
- `Assets/Scripts/MapDesigner/VoronoiWaterFeatureGenerator.cs` — voronoi cell builder + raster mask out
- `Assets/Scripts/MapDesigner/VoronoiElevationFeatureGenerator.cs` — elevation/cliff variant
- `Assets/Scripts/Data/NaturalFeatureSettingsSO.cs` — ScriptableObject (siteMode/uniform-grid+jitter, siteCount 64-256, featureType water/elevation/rift, seed)
- `Assets/Tests/EditMode/Editor/NaturalFeatureGraphTests.cs` — deterministic + walkable filter + perf budget

**Algoritma (dependency-free):**
```csharp
// Jittered grid sites: divide room into NxN cells, place 1 site per cell with random offset
// Pure random REJECT (combat readability bozar) — sadece uniform grid + jitter
public static List<Vector2> GenerateSites(Vector2Int roomSize, int siteCount, int seed) {
    int gridN = Mathf.Max(2, Mathf.RoundToInt(Mathf.Sqrt(siteCount)));
    float cellW = roomSize.x / (float)gridN;
    float cellH = roomSize.y / (float)gridN;
    var sites = new List<Vector2>();
    for (int gy = 0; gy < gridN; gy++) {
        for (int gx = 0; gx < gridN; gx++) {
            float jx = Hash01(seed, gx, gy, 0);
            float jy = Hash01(seed, gx, gy, 1);
            sites.Add(new Vector2((gx + jx) * cellW, (gy + jy) * cellH));
        }
    }
    return sites;
}

// Nearest-site rasterizer: for each cell, find nearest site (Euclidean), tag cell with site index
public static int[,] RasterizeVoronoi(Vector2Int size, List<Vector2> sites) { ... }

// Feature mask: tag a subset of sites as "feature_water" or "feature_elevation"
// Output: bool[,] featureMask (true = inside feature zone) + Vector2[] siteCenters
```

**Performance:** 200x200 grid + 256 sites ≤ 5ms editor (no Burst). Use squared distance, avoid sqrt.

**Output struct:**
```csharp
public struct NaturalFeatureGraphResult {
    public Vector2[] sites;
    public int[,] siteIndex;
    public bool[,] featureMask;
    public FeatureType[] siteTypes;  // None / Water / Elevation / Rift
}
public enum FeatureType { None, Water, Elevation, Rift }
```

**Walkable filter ZORUNLU:** feature mask, `room.walkable[x,y]=false` cell'lere yazılmaz (Karar #143-D).

**Test acceptance:**
- Aynı seed = aynı output (deterministic)
- 200x200 grid 64 site ≤ 5ms, 256 site ≤ 15ms (Stopwatch)
- featureMask sadece walkable cell'leri kapsar
- siteCount=64 verirse 64-65 site üretir (gridN^2 yakın)

---

### Task B — FeatureEdgeSmoothingPass + Pair E/F Wang Integration (Karar #143-L)

**Yeni dosyalar:**
- `Assets/Scripts/MapDesigner/FeatureEdgeSmoothingPass.cs` — feature mask boundary'sini Wang tile semantic boundary'sine map'ler
- `Assets/Scripts/Data/FeatureEdgeSmoothingProfileSO.cs` — ScriptableObject (smoothingMode/Wang+Sprite/SpriteOnly, overlaySpriteSet, boundaryWidth)
- `Assets/Tests/EditMode/Editor/FeatureEdgeSmoothingTests.cs`

**Değişen dosyalar:**
- `Assets/Scripts/Systems/Map/CornerWangPainter.cs` — `Paint(...)` parametre listesi: feature mask + smoothing profile (eğer null değilse Wang boundary aktif)
- `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs` — eğer `room.naturalFeatures` set'liyse, L1 floor sonrası FeatureEdgeSmoothingPass çağır
- `Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs` + `RoomData` — `NaturalFeatureGraphResult naturalFeatures` field ekle
- `Assets/Scripts/Data/RoomRecipe.cs` — `NaturalFeatureSettingsSO featureSettings` field

**Algoritma:**
```csharp
// 1. Voronoi feature mask boundary cell'leri bul (mask[x,y] != mask[neighbour])
// 2. Boundary cell'lere uygun Wang corner tile yerleştir (Pair E rubble↔cliff_drop için, Pair F rubble↔water_pool için)
// 3. SADECE featureEdge=true pairing'lerde aktif (Aşama 1'de disable kalıyordu)
// 4. Boundary'nin DIŞINA SpriteRenderer overlay yerleştir (overlaySpriteSet'ten rastgele) — pixel-perfect smoothing
```

**Wang lookup:** terrainGrid sentinel int kullan (örn. floor=1, water=10, cliff=11). `RimaBiomePreset.FindPairing(lower, upper)` ile pair seç. Aşama 1'de mevcut method imzası: `FindPairing(int lower, int upper) -> TilesetPairing` (TilesetPairing.cs zaten `isFeatureEdge` field'ı vardı).

**Test acceptance:**
- Feature mask boundary cell sayısı = wang tile placement sayısı
- Walkable filter sonrası boundary tile'ları walkable cell üstüne düşmez
- Aynı seed deterministic

---

### Task C — FeatureMaskSO + DetailDecalPainter Integration (Karar #143-K)

**Yeni dosyalar:**
- `Assets/Scripts/Data/FeatureMaskSO.cs` — ScriptableObject (Texture2D alphaMask + AnimationCurve remap + bool invert + Vector2 worldOffset/scale)
- `Assets/Tests/EditMode/Editor/FeatureMaskSOTests.cs`

**Değişen dosyalar:**
- `Assets/Scripts/MapDesigner/DetailDecalPainter.cs` — `DensityForCell` formülünü genişlet: `wallProximity * featureProximity * baseDensity`
- `Assets/Scripts/MapDesigner/TransitionBrushPainter.cs` — opsiyonel `FeatureMaskSO featureMask` field (atlas density modulation)
- `Assets/Scripts/Data/PatchAtlasSO.cs` — yeni field `FeatureMaskSO featureMask`, `[Range(0,1)] float featureMaskWeight = 0.5f`
- `Assets/Editor/RimaMapDesignerWindow.cs` — Feature Mask preview + manual paint (Texture2D brush)

**Algoritma:**
```csharp
public float SampleFeatureProximity(Vector2Int cell, NaturalFeatureGraphResult features, Vector2Int roomSize) {
    if (features.sites == null || features.sites.Length == 0) return 1f;
    
    // 1. Distance to nearest feature site (Euclidean)
    Vector2 cellCenter = new Vector2(cell.x + 0.5f, cell.y + 0.5f);
    float minDistSq = float.MaxValue;
    int nearestSite = -1;
    for (int i = 0; i < features.sites.Length; i++) {
        if (features.siteTypes[i] == FeatureType.None) continue;
        float dsq = (features.sites[i] - cellCenter).sqrMagnitude;
        if (dsq < minDistSq) { minDistSq = dsq; nearestSite = i; }
    }
    if (nearestSite < 0) return 1f;
    
    float dist = Mathf.Sqrt(minDistSq);
    // 2. Feature-bias curve: inside mask = 1.0, edge band (≤4 cells) = 0.8, far (>8 cells) = 0.1
    if (features.featureMask[cell.x, cell.y]) return 1.0f;
    if (dist <= 4f) return 0.8f;
    if (dist <= 8f) return 0.3f;
    return 0.1f;
}

// DetailDecalPainter.DensityForCell composite:
float density = baseDensity * WallProximityFactor(cell) * FeatureProximityFactor(cell);
```

**FeatureMaskSO sample:**
```csharp
public float Sample(Vector2Int cell, Vector2Int roomSize) {
    if (alphaMask == null) return 1f;
    float u = (cell.x + 0.5f) / roomSize.x * scale.x + worldOffset.x;
    float v = (cell.y + 0.5f) / roomSize.y * scale.y + worldOffset.y;
    float alpha = alphaMask.GetPixelBilinear(u, v).a;
    if (invert) alpha = 1f - alpha;
    return remap != null && remap.keys.Length > 0 ? remap.Evaluate(alpha) : alpha;
}
```

**Test acceptance:**
- Feature mask null → density unchanged (backward compat with Aşama 1 tests)
- Feature mask center cell yakını feature site → density artar
- Feature mask uzak cell → density azalır
- Aynı seed deterministic

---

### Tüm Aşama 2 Acceptance (Karar #143 Aşama 2 LOCK kriteri)

1. **Compile:** `dotnet build Assembly-CSharp.csproj` 0 hata.
2. **EditMode tests:** Mevcut Aşama 1 tests + 3 yeni test file = TÜM PASS.
3. **Walkable filter:** TÜM yeni painter'lar `walkable=false` cell'leri SKIP eder.
4. **Determinism:** Aynı seed = aynı output (her 3 task için test).
5. **Performance:** 200x200 grid + 256 site ≤ 20ms editor gen (Stopwatch log).
6. **Backward compat:** `room.naturalFeatures` null veya `FeatureMaskSO` null → Aşama 1 davranış (regression yok).
7. **MapLayerOrchestrator order:**
   ```
   L1 Floor Base (mevcut)
   L2 Floor Variation (mevcut)
   [NEW] FeatureEdgeSmoothingPass (eğer features mevcut)
   L3 Wall Overlay (mevcut)
   L4 Transition Brush (feature mask aware)
   L5 Detail Decal (feature mask aware)
   L6 Accent (mevcut)
   ```
8. **RimaMapDesignerWindow:** "Natural Features" foldout (siteCount slider + feature type + generate button + preview).

---

## Kısıtlar

- **Burst YASAK:** Pure C# yeter, 200x200 grid ≤ 20ms.
- **External library YASAK:** MIConvexHull/csDelaunay gibi paket eklenmez. Jittered grid + nearest-site rasterizer manuel yazılır.
- **Asset YOK:** SO field'ları boş kalsın, kullanıcı sabah Pair E/F PixelLab + overlay sprite gen sonrası dolduracak.
- **Karar #143-D walkable filter:** TÜM yeni painter/pass'larda zorunlu, test ile doğrula.
- **dotnet build PASS** + **Unity compile PASS** + **EditMode test PASS** zorunlu.
- **Pair E (rubble↔cliff_drop) ve Pair F (rubble↔water_pool) asset YOK** — `TilesetPairing.isFeatureEdge=true` flag ile mevcut tileset'ler placeholder olarak kalsın.

## Commit Message

```
[S83+][Karar #143 Aşama 2] NaturalFeatureGraph + Voronoi + Wang smoothing + FeatureMaskSO

- NaturalFeatureGraph + VoronoiWater/ElevationFeatureGenerator (jittered grid sites + nearest-site rasterizer)
- NaturalFeatureSettingsSO + FeatureEdgeSmoothingProfileSO + FeatureMaskSO
- FeatureEdgeSmoothingPass — feature boundary → Wang tile + sprite overlay
- DetailDecalPainter feature-aware density (wallProximity * featureProximity * featureMaskAlpha)
- ProceduralRoomGenerator + RoomData naturalFeatures field
- MapLayerOrchestrator feature smoothing pass entegrasyonu
- RimaMapDesignerWindow Natural Features foldout
- 3 EditMode test file: Graph deterministic + Smoothing boundary + MaskSO sample
- Karar #143-J/K/L LOCK 2026-05-15 — Aşama 2 LOCK

Aşama 3 (Wall karakterizasyonu + production demo) ayrı dispatch.
```

## CODEX_DONE Protokol

`CODEX_DONE_<profil>.md` güncelle (cx_dispatch.py protokol). Tüm 3 task tek dispatch'te yapılır, ayrı commit YOK — tek consolidated commit.
