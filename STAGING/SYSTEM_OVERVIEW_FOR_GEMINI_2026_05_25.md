# RIMA Wall System V2 — System Overview (for Gemini 3.5 Flash review)

**Date**: 2026-05-25
**Session**: S104 (post-cleanup + V2 Pass 2 + World Painter)
**Project**: RIMA — Unity 2D top-down ARPG roguelite
**Target visual**: High top-down 3/4 (Hades / Diablo III / Children of Morta style)

---

## 1. HEDEF (vision)

**Logic-first room builder + visual dressing.**
- Önce sade kutu placeholder ile algorithm doğrulanır (PHASE 1, ŞU AN BURADAYIZ)
- Sonra PixelLab pixel art sprite'lar Visual child olarak takılır (PHASE 2)
- Decoration + VFX katmanları en son (PHASE 3)

**Referans görseller**: Hades dungeon, Diablo III crypt, Children of Morta — yüksek arka duvar + stepped yan zincirler + açık ön cephe + karanlık void + dekorasyon ile zenginleştirilmiş combat alanları.

**Kritik kural**: Diamond form için "tek parça diyagonal duvar KULLANMA" → kare grid üstünde kademeli (stair-step) yan zincirlerle pseudo-diamond.

---

## 2. SİSTEM MİMARİSİ

### 4 Katman (ChatGPT spec'ten)
```
1. Gameplay / Logic Layer
   - room footprint, walkable area, blocked wall footprint
   - collider bounds, entrance sockets, enemy spawn points, prop sockets
   
2. Visual Wall Layer
   - rear wall setpiece, side wall chain visuals, connectors, corners
   - door / arch / portal, low front edge, rubble / seam / shadow overlays
   
3. Decoration Layer (henüz placeholder marker only)
   - torch, banner, cages, bookshelves, altar, brazier, sarcophagus, barrels, rift crystals
   
4. VFX / Atmosphere Layer (henüz yok)
   - cyan rift cracks, fog, glow decals, water reflections, torch light overlays
```

### Şu anki implementation seviyesi
- ✅ **Layer 1 (Logic)**: tam, placeholder ile çalışıyor
- ✅ **Layer 2 (Visual Walls)**: placeholder kutular, real sprite henüz değil
- ⚠️ **Layer 3 (Decoration)**: marker placeholder (water pool, island, center reserved) ama prop spawn yok
- ❌ **Layer 4 (VFX)**: yok

---

## 3. KLASÖR YAPISI

```
RIMA/
├── Assets/
│   ├── Editor/
│   │   └── AssetPackV3Importer.cs       ← yeni AssetPostprocessor (PPU 64 wall / 32 floor)
│   ├── Scripts/
│   │   ├── Editor/Walls/V2/
│   │   │   ├── RoomBuilderTestRunner.cs ← Editor menu: "RIMA/V2/Run All Test Rooms" + "Run Preset Rooms (4 guide)"
│   │   │   └── RoomPainterWindow.cs     ← Editor menu: "RIMA/V2/World Painter"
│   │   └── Runtime/Walls/V2/
│   │       ├── WallPieceEnums.cs        ← WallPieceType, WallDirection, WallHeight + RoomShapeType, FrontEdgeMode
│   │       ├── WallPieceData.cs         ← ScriptableObject metadata
│   │       ├── WallPieceRegistry.cs     ← lookup (GetById, GetByType, GetSpanForLength)
│   │       ├── WallPiece.cs             ← MonoBehaviour (prefab root) + OnDrawGizmosSelected
│   │       ├── RoomSpec.cs              ← input data (RoomSpec + NicheSpec + ProtrusionSpec)
│   │       └── WallChainRoomBuilder.cs  ← algorithm (ComputeFootprint → ExtractEdges → BuildChain*)
│   ├── Prefabs/
│   │   ├── Environment/Walls/
│   │   │   ├── Placeholders/            ← 14 V2 placeholder prefab (renkli kutu)
│   │   │   └── AssetPackV3/             ← 12 real wall prefab (Sheet 1 + 2 PixelLab sprite ile, V1 sistem)
│   │   └── Characters/
│   │       └── Warblade.prefab          ← karakter prefab (CapsuleCollider2D + PlayerMovementController)
│   ├── ScriptableObjects/Walls/V2/
│   │   ├── WallPieceRegistry_v1.asset   ← 14 placeholder ref
│   │   └── wpd_*.asset                  ← 14 WallPieceData (rear_wall_1x/2x/3x, side_wall_*, connector, corners, door, low_front, open_gap, seam)
│   ├── ScriptableObjects/Tiles/Floor/
│   │   └── tile_0..14.asset             ← 15 Unity Tile (floor)
│   ├── Sprites/AssetPackV3/
│   │   ├── walls/sheet_{1,2,3,4}/       ← sliced PixelLab wall sprites
│   │   └── floor/                       ← 16 floor tile (32x32, PPU 32)
│   ├── Scenes/Test/
│   │   ├── RoomBuilderTest_v1.unity     ← 5 eski test oda
│   │   ├── WallRoomTest_v1.unity        ← V1 real-sprite test (eski iter)
│   │   └── PainterTestAll_v1.unity      ← yeni 5-oda render (4 preset + custom L)
│   └── _archive~/                       ← Unity ignore (tilde) — eski iter scripts arşivi
├── STAGING/
│   ├── concepts/asset_pack_v3/          ← sliced sprite source + render screenshots
│   ├── _codex_tasks/                    ← aktif/geçmiş Codex task'ları
│   ├── _archive_old_tasks/              ← arşivlenmiş eski codex tasks
│   └── _archive_old_analysis/           ← arşivlenmiş eski analiz dosyaları
├── CODEX_TASK.md                        ← aktif Codex dispatch
├── CODEX_DONE.md                        ← son Codex çıktısı
└── scripts/slice_asset_pack_v3.py       ← Python slice helper
```

---

## 4. KOD MİMARİSİ — DETAYLI

### A. WallPieceEnums.cs (data types)
```csharp
public enum WallPieceType {
    RearWall, SideWall, Connector,
    OuterCorner, InnerCorner,
    DoorArch, LowFront, OpenGap, Seam
}
public enum WallDirection { Rear, SideLeft, SideRight, Front, Any }
public enum WallHeight { Low, Normal, Tall }
public enum RoomShapeType { Rectangle, Diamond, Irregular }
public enum FrontEdgeMode { Open, LowWall, Broken }
```

### B. WallPieceData.cs (ScriptableObject)
```csharp
[CreateAssetMenu(menuName="RIMA/Walls V2/Wall Piece Data")]
public class WallPieceData : ScriptableObject {
    [Header("Identity")]
    public string id;
    public WallPieceType type;
    public WallDirection direction;
    
    [Header("Footprint (cells)")]
    public Vector2Int footprintSize;     // örn (3, 1) for rear_wall_3x
    public int lengthInCells;
    public WallHeight heightType;
    
    [Header("Connectivity")]
    public bool connectLeft, connectRight;
    
    [Header("Pivot + Sockets")]
    public Vector2 anchorOffset;
    public Vector2 leftSocketLocal, rightSocketLocal;
    public Vector2 seamSocketLeftLocal, seamSocketRightLocal;
    
    [Header("Collider")]
    public Vector2 colliderSize;
    public Vector2 colliderOffset;
    
    [Header("Visual")]
    public Color placeholderColor;
    
    [Header("Prefab")]
    public GameObject prefab;            // ← swap noktası: placeholder vs real
}
```

### C. WallPiece.cs (MonoBehaviour, prefab root component)
```csharp
public class WallPiece : MonoBehaviour {
    public WallPieceData data;
    public SpriteRenderer visual;        // child "Visual"
    public BoxCollider2D footprintCollider;
    public Transform footprintAnchor, leftSocket, rightSocket;
    public Transform seamSocketLeft, seamSocketRight;
    
    public void Initialize(WallPieceData d) { /* apply metadata to components */ }
    void OnDrawGizmosSelected() {
        // Footprint outline (yellow)
        // Collider bounds (green)
        // Sockets (cyan + magenta dots)
    }
}
```

### D. WallChainRoomBuilder.cs (algorithm)
```csharp
public class WallChainRoomBuilder : MonoBehaviour {
    public WallPieceRegistry registry;
    public Transform roomParent;
    public float cellSize = 1f;
    
    public void Build(RoomSpec spec) {
        Clear();
        var footprint = ComputeFootprint(spec);  // HashSet<Vector2Int>
        var rear = ExtractRearEdges(footprint);
        var left = ExtractSideEdges(footprint, side: -1);
        var right = ExtractSideEdges(footprint, side: +1);
        var front = ExtractFrontEdges(footprint);
        
        if (spec.rearWallEnabled) BuildRearChain(rear, spec);
        if (spec.sideWallsEnabled) {
            BuildSideChain(left, WallDirection.SideLeft);
            BuildSideChain(right, WallDirection.SideRight);
        }
        BuildFrontEdge(front, spec);
        PlaceCornersAtJunctions(footprint, spec);
        // Pass 2: PlaceCenterReserved, PlaceInteriorIslands, PlaceWaterPoolZones (Codex eklendi)
    }
}
```

**Algorithm adımları:**
1. **ComputeFootprint** — Rectangle / Diamond (stair-step) / Irregular (HasCustomWalkable ise spec.walkableCells, yoksa rect)
2. **ExtractEdges** — boundary cell'lerin direction'ı (rear/left/right/front)
3. **BuildRearChain** — connector + span (3x/2x/1x greedy fill) + connector + door (EnforceDoorCenter Pass 2)
4. **BuildSideChain** — L/R orientation (Pass 2 fix), stair turns için corner
5. **BuildFrontEdge** — Open / LowWall / Broken, min 3 cell açıklık (EnsureFrontMinOpening Pass 2)
6. **PlaceCornersAtJunctions** — convex (outer) vs concave (inner) corner detection
7. **Pass 2 markers** — center reserved (sarı çember gizmo), interior islands (rect), water pools (cyan)

### E. RoomSpec.cs (input)
```csharp
[Serializable]
public class RoomSpec {
    public string roomName = "Room";
    public int widthCells = 8, heightCells = 6;
    public RoomShapeType shapeType = RoomShapeType.Rectangle;
    public bool rearWallEnabled = true, sideWallsEnabled = true;
    public FrontEdgeMode frontEdgeMode = FrontEdgeMode.LowWall;
    public Vector2Int doorPosition = new Vector2Int(-1, -1);
    public List<Vector2Int> alcovePositions, protrusionPositions;
    
    // World Painter ile painted cells
    public List<Vector2Int> walkableCells;  // null/empty → fallback rect
    public bool HasCustomWalkable => walkableCells != null && walkableCells.Count > 0;
    
    // Pass 2 intent fields
    public int interiorMarginCells = 1;
    public int rearMinWidthCells = 3;
    public int frontMinOpeningCells = 3;
    public bool enforceCenteredRearDoor = true;
    public int diamondTopWidthCells = 6;
    public int diamondStepMin = 1, diamondStepMax = 2;
    public int connectorSpacingMin = 4, connectorSpacingMax = 7;
    public int reservedCenterRadiusCells = 0;
    public List<RectInt> interiorIslandRects, waterPoolRects;
    public List<NicheSpec> nicheSpecs;
    public List<ProtrusionSpec> protrusionSpecs;
    public string roomPresetId = "";
}

[Serializable] public struct NicheSpec { public string side; public int anchorRow, width, depth; public bool mirror; }
[Serializable] public struct ProtrusionSpec { /* same */ }
```

### F. RoomPainterWindow.cs (Editor UI)
- Menu: `RIMA → V2 → World Painter`
- Grid canvas 8-64 cell (Pass 2 update)
- 5 brush: Walkable / Erase / Door / Alcove / Protrusion
- 4 preset button: Library / Combat / Ritual / Flooded
- Wall preview overlay (real-time): walkable sınırlarında renkli kenar (rear=mavi, side=yeşil, front=açık mavi)
- Shift+drag = rectangle fill
- Ctrl+scroll = zoom
- Bottom-left anchor (resize'da paint sabit kalır)
- JSON save/load layout
- Generate Room → RoomSpec instance → WallChainRoomBuilder.Build

---

## 5. ASSET PIPELINE — PixelLab → V2 sistem

### Şu anki state (placeholder)
- **14 V2 placeholder prefab** (`Assets/Prefabs/Environment/Walls/Placeholders/`):
  - rear_wall_1x/2x/3x (mavi), side_wall_1x/2x/3x (yeşil)
  - connector (gri), outer_corner (turuncu), inner_corner (mor)
  - door_arch (sarı), low_front_1x/2x (açık mavi), open_gap (kırmızı), seam (pembe)
- Her placeholder: `WallPiece_Root` (BoxCollider2D + WallPiece script) → child Visual (SpriteRenderer + 1x1 white sprite + color) + 5 socket child

### PixelLab assets (HAZIR, henüz wire edilmedi V2'ye)
- Sheet 1 (8 connector): `STAGING/concepts/asset_pack_v3/sliced/sheet_1/*.png` — 128×256 uniform canvas
- Sheet 2 (9 wall span): sliced — 256×192
- Sheet 3 (13 piece): sliced — 256×128 / 128×128 mix
- Sheet 4 (16 piece): sliced — 128×128
- Floor (16 tile): `Assets/Sprites/AssetPackV3/floor/tile_*.png` — 32×32 PPU 32

### Eksik: SideWall sprite'ı YOK
Sheet 1+2 sadece N-facing (Rear) 3/4 sprite. Side wall için yeni PixelLab gen lazım (kullanıcı onayı bekleniyor).

### Real asset entegrasyon planı (PHASE 2)
1. Her placeholder prefab'ın `WallPieceData.prefab` field'ını **real wall prefab** ile değiştir
2. Real prefab içinde Visual child SpriteRenderer.sprite → PixelLab PNG
3. Collider visual'dan bağımsız — `colliderSize/Offset` zaten metadata'da, sprite size'tan değil
4. SideWall sprite gelene kadar Rear sprite flip ile temporary use OR sadece placeholder bırak

---

## 6. TEST ODALARI — 5 PRESET

### Render: `STAGING/concepts/asset_pack_v3/painter_test_all_scene.png` + `painter_test_all_game.png`

| # | Preset | Spec | Render verdict |
|---|---|---|---|
| 1 | **Library** (22×22 Rect) | rear+side+front, side niches (mor), door üst orta, 2 interior island | ✅ Niche + door + island gizmo görünüyor |
| 2 | **Narrow Combat** (Diamond) | top width 8, stair-step 1-cell, frontMinOpening 4 | ✅ Stair-step diamond net, sarı door üst, kırmızı open gap |
| 3 | **Ritual** (Diamond) | center reserved radius 3, low front | ✅ Diamond + sarı reserved çember + low front mix |
| 4 | **Flooded** (22×16 Rect) | 2 water pool side rects, open front | ✅ 2 cyan water rect, low front, rear+side |
| 5 | **Custom L-shape** (Irregular) | 10×6 base + 4×4 wing top-right + extension | ⚠️ L köşesi corner detection biraz dağınık (P1 iter) |

---

## 7. KOD VS SPEC GAP — Bilinen eksikler (Pass 3 todos)

| # | Spec | Mevcut | Gap |
|---|---|---|---|
| 1 | A/B/C/D asset grouping (ChatGPT) | WallPieceType lookup | Group intent + validation yok |
| 2 | Visual / Collider full ayrım | Component-level ayrı ama collider auto-fit sprite'tan | colliderFootprint RectInt ayrı field gerek |
| 3 | PropSocket_1, PropSocket_2 | Yok | Prefab hierarchy'e child ekle |
| 4 | Seam overlay placement | PlaceSeamsAtJunctions method YOK | Wall birleşimlerine seam spawn |
| 5 | Spawn point logic (enemy, objective) | Yok | RoomSpec'e spawnPoints, objectiveSockets ekle |
| 6 | Decoration spawning (torch, banner, etc.) | Marker only | Prop registry + spawn algorithm |
| 7 | VFX layer (rift, fog) | Yok | 4. katman hiç başlamadı |
| 8 | Layer GameObject organize | Tek "Pieces" parent | 4 child: Logic / Visual / Decoration / VFX |
| 9 | Debug gizmos — walkable area, chain arrows | Cell-level gizmo var, builder-level zayıf | Builder OnDrawGizmos genişlet |
| 10 | Rear wall "setpiece" (büyük portal/arch) | Sadece basit door arch | Setpiece prefab + RoomSpec.rearSetpieceId |
| 11 | Side wall sprite (PixelLab) | Yok (gen onayı bekliyor) | Sheet 2'ye 3 SideWall + 2 LowFront piece üret |
| 12 | Niche/Protrusion full formula görsel test | NicheSpec çalışıyor ama tam karşılık (WallSpan+InnerCorner+ShortWall+InnerCorner+WallSpan) verify yok | Test runner'a niche/protrusion test ekle |
| 13 | Character integration test | Yok | Play mode'da Warblade walls'a çarpsın |

---

## 8. SORULAR GEMINI'YE

Lütfen aşağıdaki soruları sistem değerlendirmesi için cevapla:

1. **Mimari**: 4 katman ayrımı (Logic / Visual / Decoration / VFX) ARPG dungeon için yeterli mi? Karanlık void, ambient light overlay gibi 5. katman gerekli mi?

2. **WallPieceData metadata**: 13+ field var (id, type, direction, footprint, length, height, connect, sockets, collider, prefab, placeholder color). Eksik field var mı? **prop_socket_1, prop_socket_2**, **light_socket**, **vfx_socket** lazım mı?

3. **Algorithm**: ChatGPT "stepped diamond" istemiş, ComputeFootprint'te Manhattan mask kullanılıyor (Pass 2'de explicit stair-step istenmiş ama henüz refactor yapılmadı; mevcut Manhattan zaten stepped çıkıyor). Bu yeterli mi yoksa explicit stair-step algorithm tercih edilir mi?

4. **Custom Irregular shape**: L-shape painted footprint için corner detection algoritması basit (her concave/convex cell'e corner). Düzgün L için nasıl iyileştirilir?

5. **Real asset wire**: Placeholder → PixelLab swap için en güvenli yöntem ne? (a) `WallPieceData.prefab` field swap, (b) Variant prefab inherit, (c) Tek prefab + multiple SpriteRenderer (placeholder + real)?

6. **Side wall problem**: PixelLab Sheet 1+2 sadece N-facing sprite. Side wall için: (a) yeni PixelLab gen, (b) rear sprite 90° rotate, (c) flip + offset hack? Hades / Diablo III nasıl çözmüş?

7. **Decoration spawning**: Torch, banner, sarcophagus props — wall'lara mı (PropSocket'larda) yoksa floor'a mı (Tilemap üstüne) yerleştirilir? Hybrid mı?

8. **VFX katman**: Cyan rift, fog, water reflection — Unity Particle System mi yoksa Shader Graph + sprite overlay mı? Performance trade-off?

9. **Sorting order**: 4 katman için sortingLayer setup ne olmalı? (Ground / Wall / Prop / Character / VFX şu an placeholder). Y-sort character için doğru çalışacak mı walls 4 unit tall sprite ile?

10. **Test sırası**: 7 unit test (Connector+Span hizalama, Corner dönüş, Front edge, Door, Stepped diamond, Collider/Visual bağımsızlık, Character sorting) implement edilmeli mi yoksa visual screenshot QC yeter mi?

---

## 9. ÖZET — Mevcut sistem ne yapıyor

✅ **Kod tarafı çalışır halde**:
- 5 farklı oda preset (Library/Combat/Ritual/Flooded/Custom) algorithm ile render olur
- Painter ile manuel paint + Generate Room sahnede wall'ları spawn eder
- 14 placeholder prefab (renkli kutu) sistem doğruluğunu gösterir
- Console 0 error

⚠️ **Eksikler** (priority order):
- Real PixelLab sprite wire (P0 — placeholder visual swap)
- Side wall sprite gen (P0 — PixelLab budget onayı)
- 4-layer GameObject organize (P1 — Decoration + VFX hazırlığı)
- Niche/Protrusion görsel verify (P1)
- Decoration spawn algorithm (P2 — prop registry)
- VFX layer (P3 — son katman)

❌ **Hiç yok**:
- Decoration prop sprite'ları
- VFX assets
- Character integration testi (Play mode walls collide)
- Spawn point / objective socket data

---

**SON**

Gemini, lütfen yukarıdaki 10 soruyu sırayla cevapla + sistemde kritik eksik gördüğün başka noktalar varsa belirt.
