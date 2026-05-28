# Sonnet Task — RIMA World Painter (Editor Window)

## Amaç
Kullanıcı **grid-based paint UI** ile oda şeklini çizecek → "Generate Room" butonuna basacak → mevcut `WallChainRoomBuilder` otomatik wall'ları yerleştirecek. Sade ilk MVP: walkable + door + alcove + protrusion paint brush'ları.

## Context — mevcut sistem (LIVE)

### V2 wall system (KULLANILACAK, dokunma)
- `Assets/Scripts/Runtime/Walls/V2/WallChainRoomBuilder.cs` — `Build(RoomSpec spec)` public entry
- `Assets/Scripts/Runtime/Walls/V2/RoomSpec.cs` — input data class:
  ```csharp
  public int widthCells, heightCells;
  public RoomShapeType shapeType;  // Rectangle, Diamond, Irregular
  public bool rearWallEnabled, sideWallsEnabled;
  public FrontEdgeMode frontEdgeMode;  // Open, LowWall, Broken
  public Vector2Int doorPosition;  // (-1,-1) = no door
  public List<Vector2Int> alcovePositions, protrusionPositions;
  ```
- `Assets/Scripts/Runtime/Walls/V2/WallPieceRegistry.cs` — registry asset
- Registry asset: `Assets/ScriptableObjects/Walls/V2/WallPieceRegistry_v1.asset`
- 14 placeholder prefab hazır (renkli kutular)
- Test scene: `Assets/Scenes/Test/RoomBuilderTest_v1.unity`

### Mevcut test runner (KULLANILACAK, referans)
- `Assets/Scripts/Editor/Walls/V2/RoomBuilderTestRunner.cs` — `RoomBuilderTestRunner.SpawnRoom(parent, registry, name, origin, spec)` helper pattern. Aynı pattern World Painter'da kullanılır.

## Görev — World Painter Editor Window

### Klasör
`Assets/Scripts/Editor/Walls/V2/RoomPainterWindow.cs`

### Spec

**Window**: Unity menu `RIMA → V2 → World Painter` → EditorWindow açar

**Layout**:
- Sol panel: Brush seçici + Settings
- Sağ panel: Grid canvas (paint area)
- Alt: Action butonları

### Brush modes (toolbar tek-seç)
1. **Walkable** (default, mavi) — paint floor cell (footprint)
2. **Erase** (gri) — cell sil
3. **Door** (sarı) — tek cell, son tıklanan kalır
4. **Alcove** (mor) — niche cell (walkable footprint'ten çıkarılır, inner corner spawn)
5. **Protrusion** (turuncu) — bump-out cell (walkable footprint'e eklenir, outer corner spawn)

### Grid Canvas
- Default size 22×22 cell (önerilen guide ölçü)
- Settings'ten width/height değiştirilebilir (8-32 arası slider)
- Her cell ~20-30 px ekranda
- Paint: mouse click + drag (continuous paint)
- Hücre rengi brush mode'a göre
- Zoom + pan basit (opsiyonel: mouse wheel zoom, middle-click pan)

### Settings panel (sol üst)
- **Grid Width**: slider 8-32 (default 22)
- **Grid Height**: slider 8-32 (default 22)
- **Front Edge Mode**: dropdown (Open / LowWall / Broken)
- **Rear Wall Enabled**: toggle (default true)
- **Side Walls Enabled**: toggle (default true)
- **Cell Size (world units)**: slider 0.5-2.0 (default 1)

### Action butonları (alt)
- **Generate Room** → RoomSpec instance oluştur, painted cell'leri shape=Irregular ile çevir, `WallChainRoomBuilder.Build(spec)` çağır
- **Clear Canvas** → tüm paint'i sıfırla
- **Save Layout** → JSON file (`STAGING/room_layouts/<isim>.json`) — UnityEditor.EditorUtility.SaveFilePanel
- **Load Layout** → JSON file load
- **Frame in Scene View** → spawn edilen oda parent'ına Scene View focus

### RoomSpec generation logic
```
- shapeType = Irregular (her zaman painter için)
- widthCells = max_x_painted + 1
- heightCells = max_y_painted + 1
- doorPosition = painted door cell (-1,-1 if none)
- alcovePositions = painted alcove cells
- protrusionPositions = painted protrusion cells
- footprint = walkable painted cells (Irregular shape için RoomSpec'e ek field gerekirse ekle, yoksa rectangle yaklaşımıyla iterate et)
```

**ÖNEMLİ**: Mevcut `WallChainRoomBuilder.ComputeFootprint` Irregular mode'da rectangle generate ediyor. Eğer painter custom footprint istiyorsa, RoomSpec'e yeni field ekle: `List<Vector2Int> walkableCells` ve ComputeFootprint bu varsa onu kullansın (yoksa eski davranış).

Bu küçük bir change RoomSpec.cs + WallChainRoomBuilder.ComputeFootprint'te.

### Generated room placement
- Yeni GameObject "PaintedRoom_<timestamp>" oluştur
- Sahnede origin (0,0,0) veya kullanıcı tercih ettiği konum (slider/Vector3 field)
- WallChainRoomBuilder MonoBehaviour ekle, registry ref ver
- Build çağır
- Eski "PaintedRoom_*" varsa sor: replace mı, yan yana mı?

### Save/Load JSON format
```json
{
  "gridWidth": 22,
  "gridHeight": 22,
  "frontEdgeMode": "LowWall",
  "rearWallEnabled": true,
  "sideWallsEnabled": true,
  "cellSize": 1.0,
  "walkable": [[x,y], ...],
  "door": [x,y],
  "alcoves": [[x,y], ...],
  "protrusions": [[x,y], ...]
}
```

### Debug + UX
- Toolbar üstte aktif brush vurgusu (renkli)
- Canvas üzerinde cell highlight (mouse hover)
- Status bar alt: "<cell count> walkable | door at (x,y) | <N> alcoves | <N> protrusions"
- Undo/Redo (opsiyonel — Unity EditorWindow Undo system)

## Constraint
- **Sadece Editor scripting** (UnityEditor namespace, asmdef Editor)
- IMGUI veya UI Toolkit — IMGUI tercih (daha hızlı yazılır)
- Mevcut V2 sistemini değiştirme (sadece RoomSpec'e opsiyonel walkableCells field eklenebilir)
- Test: Window açılır, brush'lar çalışır, paint yapılır, Generate Room basılınca sahnede oda render olur

## Definition of Done
- `Assets/Scripts/Editor/Walls/V2/RoomPainterWindow.cs` — EditorWindow class
- Menu item çalışıyor (RIMA/V2/World Painter)
- 5 brush mode (walkable, erase, door, alcove, protrusion) çalışıyor
- Grid canvas paint + drag çalışıyor
- Generate Room → wall'lar sahnede spawn oluyor
- Save/Load JSON çalışıyor
- Console 0 error
- RoomBuilderTest_v1.unity'de 1 örnek "PaintedRoom_" GameObject Build edilmiş + screenshot:
  `STAGING/concepts/asset_pack_v3/painter_test_v1.png`

## NOT
- Bu MVP. Center reserved zone, water pool zone, interior islands ileride eklenebilir
- 4 yeni room preset (Library/Combat/Ritual/Flooded) ayrı bir task (Codex'in V2 refactor proposal'ı sonrası)
- World Painter chat'in en sonunda yapılıyor — Codex paralel V2 refactor analizi yapıyor
