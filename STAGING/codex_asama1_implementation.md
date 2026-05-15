# Codex Task: RIMA Karar #143 Aşama 1 — Düz Floor 6-Katman Implementation

**Model:** gpt-5.5, effort=high
**Çıktı + commit:** Codex implementation + commit (kendi protokol)
**Süre tahmini:** 2-4 saat
**Bekleyen kaynak:** Karar #143 spec LOCK 2026-05-15 (`STAGING/karar_143_six_layer_map_architecture.md`, MASTER_KARAR_BELGESI #143-A..L)

---

## Bağlam

RIMA map architecture LOCK aldı: 6-katman düz floor pipeline (Aşama 1). Tileset Aşama 2'ye defer. Bu task **sadece Aşama 1**'i implement eder. Aşama 2 (Voronoi/Wang water/elevation) ayrı dispatch.

**Hedef:** "Doğal görünümlü oda" — kare-kare olmayan, layered overlay'lerle organik hisli oda. Asset henüz üretilmedi (sprite library archive edildi, yenileri Aşama 1 implementation sırasında PixelLab'dan üretilecek). Bu task code refactor + scaffolding + placeholder asset entegrasyonu.

## Spec Referansları

- `STAGING/karar_143_six_layer_map_architecture.md` — TÜM Aşama 1 spec, dosya formatı, Karar #143-A..L, edge-biased density algoritması, ScriptableObject şeması
- `TASARIM/MASTER_KARAR_BELGESI.md` — Karar #143 LOCK satırları, MAP MİMARİSİ KARARLARI bölümü
- `STAGING/banditknightg_vision_analysis_codex.md` — Karar #144 Room Density Formula (TR-1 edge-biased prop density referansı)
- `STAGING/artofsully_voronoi_analysis_codex.md` — Aşama 2 hazırlık (BU TASK için sadece L1-L6 spec sayfaları kritik, Voronoi defer)
- `F:\LaurethStudio\05_RESEARCH\artofsully_chatgpt_takeaways.md` — Large shapes first hiyerarşi

## Görev — Tek Aşama 1 Implementation

### 1. Code refactor + scaffolding

Yeni dosyalar:
- `Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs` — 6-katman yöneticisi (L1-L6 paint sequence + walkable mask validation)
- `Assets/Scripts/MapDesigner/WallOverlayPainter.cs` — L3 Hades-style perimeter cap painter
- `Assets/Scripts/MapDesigner/TransitionBrushPainter.cs` — L4 oval irregular patch (256x256/512x512), walkable mask zorunlu
- `Assets/Scripts/MapDesigner/DetailDecalPainter.cs` — L5 edge-biased density (wall proximity curve)
- `Assets/Scripts/MapDesigner/AccentPainter.cs` — L6 sparse rift accent
- `Assets/Scripts/Data/WallSegment.cs` — struct (start, end, direction, isCorner, isDoorway)
- `Assets/Scripts/Data/WallBrushSetSO.cs` — L3 asset library (horizontal/vertical/corner/doorway sprite refs)

Değişen dosyalar:
- `Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs` — wallEdges output ekle (perimeter scan)
- `Assets/Scripts/Data/PatchAtlasSO.cs` — yeni fields `bool edgeBiased`, `float wallProximityFactor`
- `Assets/Editor/RimaMapDesignerWindow.cs` — 6-layer toggle UI
- `Assets/Scripts/Systems/Map/CornerWangPainter.cs` — Aşama 1'de **disable** (sadece `featureEdge=true` pairing'de aktif, Aşama 2'de açılır)

### 2. Edge-biased density implementation

`DetailDecalPainter` ve `TransitionBrushPainter`'da Karar #143-E formülü:

```csharp
float DensityForCell(Vector2Int cell, RoomData room) {
    if (!room.walkable[cell.x, cell.y]) return 0f; // Karar #143-D walkable filter
    int distToWall = ComputeDistanceToWall(cell, room);
    float factor = distToWall <= 1 ? 1.0f
                 : distToWall == 2 ? 0.6f
                 : distToWall == 3 ? 0.3f
                 : 0.1f;
    return baseDensity * factor;
}
```

`ComputeDistanceToWall` BFS veya wall edge cache kullan, 200x200 grid için ≤5ms.

### 3. WallOverlayPainter Hades-style cap

- ProceduralRoomGenerator'dan `List<WallSegment> wallEdges` al
- Her segment için WallBrushSetSO'dan uygun sprite seç:
  - Horizontal segment → 256x128 brush
  - Vertical segment → 128x256 brush
  - Corner → 128x128 brush
  - Doorway gap → boş bırak (Karar #143-C, doorway = gap)
- Sprite renderer instance'la, sortingLayer "Wall", Y-axis sort
- Irregular edge için sprite varyasyonlarından rastgele seç (4 horizontal + 4 vertical + 4 corner = 12 varyant)

### 4. ScriptableObject + Inspector

`WallBrushSetSO`:
```csharp
[CreateAssetMenu(menuName = "RIMA/Map/Wall Brush Set")]
public sealed class WallBrushSetSO : ScriptableObject {
    public List<Sprite> horizontal;   // 256x128
    public List<Sprite> vertical;     // 128x256
    public List<Sprite> corner;       // 128x128
    public Sprite doorwayGap;         // optional, 128x96 if used
    public string biomeKey;           // "ShatteredKeep", "RiftFracture" vs
}
```

### 5. Asset placeholder (PixelLab gen sonra)

Bu task'ta **asset üretimi YOK**, sadece SO/code scaffolding. Asset gen ayrı dispatch (`STAGING/asset_gen_asama1_prompts.md` yazılacak, kullanıcı onayı sonrası):
- 14 wall sprite (4 horizontal + 4 vertical + 4 corner + 2 doorway)
- 9 detail decal (3 crack + 3 rubble tuft + 3 moss small)
- 3 rift accent

### 6. Test scene

`Assets/Tests/EditMode/Editor/Karar143Asama1Tests.cs` (yeni):
- ProceduralRoomGenerator → wallEdges output sayısı doğru
- WallOverlayPainter walkable cell'lere wall sprite koymaz
- DetailDecalPainter walkable=false cell'e dokunmaz
- Edge-biased density: wall yakını high, center low

### 7. RimaMapDesignerWindow UI

Layer toggle paneli:
```
[x] L1 Floor Base
[x] L2 Floor Variation
[x] L3 Wall Overlay
[ ] L4 Transition Brush (disabled if no PatchAtlasSO assigned)
[ ] L5 Detail Decal (disabled if no PatchAtlasSO assigned)
[ ] L6 Accent (disabled if no PatchAtlasSO assigned)
[ ] [Disabled] Wang Tileset (Aşama 2)
```

## Kısıtlar

- **Aşama 2 LOCK YOK**: Voronoi, FeatureMaskSO, FeatureEdgeSmoothingPass dahil ETMEYIN. Sadece L1-L6 scaffolding.
- **PixelLab asset YOK**: SO field'ları boş kalsın, kullanıcı sabah PixelLab dispatch sonrası dolduracak.
- **Eski kütüphane archive'da**: Mevcut `Assets/Art/_archive_faz1/Scatter/Decals/` referansı verilmez (Karar #143 yeni asset gen ile dolar).
- **Karar #143-D walkable filter**: TÜM painter'larda zorunlu, test ile doğrula.
- **dotnet build PASS** + **Unity compile PASS** + **EditMode test PASS** zorunlu.

## Commit Message

```
[S82+][Karar #143 Aşama 1] 6-layer map orchestrator + WallOverlayPainter + walkable mask filter

- MapLayerOrchestrator + 4 painter (Wall/Transition/Detail/Accent)
- WallSegment + WallBrushSetSO scaffolding
- ProceduralRoomGenerator wallEdges output
- Edge-biased density formula (Karar #143-E)
- RimaMapDesignerWindow 6-layer toggle UI
- Aşama 2 (Voronoi/Wang) defer (CornerWangPainter disabled)
- EditMode tests: walkable filter + density + wall edge count

Karar #143-A..L LOCK 2026-05-15. Asset gen ayrı dispatch.
```

## CODEX_DONE Protokol

`CODEX_DONE_<profil>.md` güncelle (cx_dispatch.py protokol).
