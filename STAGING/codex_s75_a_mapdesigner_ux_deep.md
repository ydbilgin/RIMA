# S75-A — Map Designer UX Deep PixelLab Parity

**Effort:** high
**Prereq:** S74-C committed (089e596)
**File:** `Assets/Editor/RimaMapDesignerWindow.cs` (~998 lines after S74-B)
**Reference:** `STAGING/pixellab_map_export_analysis_LOCK.md`, `STAGING/s75_autonomous_plan.md`

---

## USER COMPLAINT (verbatim)

> "bu tam istediğim gibi pixellabdaki gibi çalışmıyor... upper lower terrain mantığını doğru şekilde ver yaptırt çalıştır"

Functional bazda mouse + paint çalışıyor (Sonnet UnityMCP test PASS), AMA UX hâlâ PixelLab Map Tool deneyimine ulaşmamış. Aşağıdaki fix'leri yap.

---

## DELIVERABLES (12 fix sırayla)

### 1. Canvas default size 32×24
**File:** `RimaMapDesignerWindow.cs`
Şu an `DefaultRoomWidth = 16, DefaultRoomHeight = 12`. Yap:
```csharp
private const int DefaultRoomWidth = 32;
private const int DefaultRoomHeight = 24;
```
Mevcut .json save dosyaları load edildiğinde kendi boyutlarını kullanır (sorun yok).

### 2. Auto-Fit on window open
`EnsureInitialized()` veya `OnEnable()` sonunda **ilk açılışta** `FitCanvasToWindow()` çağır. Sentinel: serialized bool `hasFittedOnce = false`. Sadece bir kere.

### 3. Hover preview = REAL TILE sprite overlay
`DrawCellHover(Vector2Int cell)` mevcut: yeşil/kırmızı renkli filled rect. **YENİ:** Eğer paintMode == Cell:
- O cell'e activeTerrainId paint edilirse ne olacağını **hesapla** (4 corner = activeTerrainId, surrounding korner'lar mevcut grid'den oku)
- Resolve tile via CornerWangPainter.ResolveTile(biome, nw, ne, sw, se)
- Cell rect'e tile sprite'ı **alpha 0.6** ile çiz (GUI.DrawTextureWithTexCoords)
- Cell rect üstüne ince yeşil outline (paint) veya kırmızı (erase)

Bu sayede kullanıcı paint öncesi sonucu görür → PixelLab UX parity.

### 4. Brush radius visual indicator
`brushRadius > 1` ise hover cell'in çevresinde radius çember/kare çiz. Cell-paint için kare uygun:
```csharp
if (brushRadius > 1) {
  int r = brushRadius - 1;
  Vector2Int br = new Vector2Int(hoveredCell.x - r, hoveredCell.y - r);
  Vector2Int tl = new Vector2Int(hoveredCell.x + r, hoveredCell.y + r);
  Rect a = CellToCanvasRect(br); Rect b = CellToCanvasRect(tl);
  Rect outline = Rect.MinMaxRect(a.xMin, b.yMin, b.xMax, a.yMax);
  Handles.color = new Color(0f, 1f, 1f, 0.5f);
  Handles.DrawSolidRectangleWithOutline(outline, Color.clear, Handles.color);
}
```

### 5. Drag-paint cell interpolation (Bresenham line)
`HandleGridInput` içinde `EventType.MouseDrag + button==0 + isPainting`. Şu an: sadece current cell'i paint ediyor. **YENİ:** Bresenham line between `lastPaintedCell` and `current` — arada kalan tüm cell'leri paint et:
```csharp
void PaintLineFromTo(Vector2Int a, Vector2Int b, int value) {
  int dx = Mathf.Abs(b.x - a.x), dy = Mathf.Abs(b.y - a.y);
  int sx = a.x < b.x ? 1 : -1, sy = a.y < b.y ? 1 : -1;
  int err = dx - dy;
  int x = a.x, y = a.y;
  while (true) {
    PaintWithRadius(new Vector2Int(x, y), value);
    if (x == b.x && y == b.y) break;
    int e2 = err * 2;
    if (e2 > -dy) { err -= dy; x += sx; }
    if (e2 <  dx) { err += dx; y += sy; }
  }
}
```
İlk MouseDown'da `lastPaintedCell = hoveredCell`, drag sırasında `PaintLineFromTo(lastPaintedCell, current, value); lastPaintedCell = current;`.

### 6. Active terrain indicator on cursor — thumbnail at mouse
Hover cell hesabı + sağ üst kısmında küçük 24×24 thumbnail çiz (activeTerrain.baseTile sprite). `DrawCellHover` içinde:
```csharp
Rect rect = CellToCanvasRect(cell);
Rect thumbRect = new Rect(rect.xMax + 4f, rect.yMin - 4f, 24f, 24f);
// Draw activeTerrain baseTile sprite into thumbRect with alpha 0.9
```

### 7. Pairing info panel (right panel bottom)
`DrawRightPanel` sonunda yeni mini section:
```
── Active Pairing ──
Hover cell: (X, Y)
Corners: NW=1 NE=0 SW=1 SE=0
Wang Key: 10 (Diag NE-SW)
Resolves: FloorWall_CornerWangTileSet[10]
Transition: 0.25 (default)
─────────────────────
```
Mouse hover-out ise: "Hover a cell to see pairing info"

### 8. Lower/Upper labels in terrain palette
Palette'da her terrain butonun altında küçük subtitle:
```
[Wall]
 id=1
 ↕ Floor, Path, Rift, Moss   (kısa: hangi pairing'lerin upper/lower'ı)
```
Sığmıyorsa truncate "... +N more"

### 9. Status bar 3-line
Şu an 2-line. **YENİ:**
- Line 1: Room/Biome/Active terrain/Output/Erase mode (mevcut)
- Line 2: Cell info (mouse on canvas iken: cell+corners+wangKey+tileSet+transitionSize)
- Line 3: Tips OR pairing warnings (e.g., "⚠ Cell (5,3) has 3 terrains: rendering fallback")

Height: 22 → 60 (3 × 20).

### 10. Smooth scroll zoom (cubic ease)
`EventType.ScrollWheel`:
```csharp
float delta = evt.delta.y;
float t = Mathf.Sign(delta) * Mathf.Pow(Mathf.Abs(delta) * 0.1f, 1.5f);
cellSize = Mathf.Clamp(cellSize - t * 2f, 10f, 80f);
```
Daha smooth feel.

### 11. Cursor sprite preview overlay
`HandleGridInput` sonunda — eğer paint mode active ve hovering canvas:
- Cursor altında 32×32 area'da activeTerrain.baseTile preview alpha 0.5
- Erase mode'da: kırmızı X icon

EditorGUIUtility.AddCursorRect kullanabilirsin VEYA direct GUI.DrawTexture mouse position'da.

### 12. Larger default cellSize
`[SerializeField] private float cellSize = 32f` → `40f`. Auto-Fit (#2) zaten ilk açılışta override edecek ama default 40 daha modern hisset.

---

## ADDITIONAL — Edit Biome window

Şu an "Edit Biome" butonu Selection.activeObject = biome (Inspector açar). **GELİŞTİR:** Custom popup window `BiomeQuickEditorWindow` (yeni dosya `Assets/Editor/BiomeQuickEditorWindow.cs`):
- Terrains list — her satırda: id (read-only), name TextField, paletteColor color picker, baseTile object field, [Delete] button
- Pairings list — her satırda: lower dropdown (terrain names), upper dropdown, tileSet field, transitionSize slider, transitionDescription TextArea, [Delete] button
- Footer: [+ Add Terrain] [+ Add Pairing] [Save] [Cancel]
- "Edit Biome" butonu → BiomeQuickEditorWindow.Open(activeBiome)

Bu opsiyonel ama PixelLab "biome editor" parity için değerli.

---

## VALIDATION

1. `dotnet build RIMA.slnx` PASS
2. Unity compile errors: 0 (`read_console` types=["error"])
3. RimaMapDesignerWindow open → Auto-Fit triggers, canvas dolu görünür
4. Hover cell on empty area → status bar shows pairing info ("Resolves: …")
5. Drag-paint hızlı → no missed cells (Bresenham çalışıyor)
6. Brush radius=3 → 5×5 outline görünür
7. Edit Biome → BiomeQuickEditorWindow açılır (varsa)

**Screenshot:** STAGING/s75a_mapdesigner_final.png — Map Designer Auto-Fit + painted dummy 32×24 map + status bar 3-line görünür.

---

## COMMIT MESAJI

```
[S75-A] Map Designer UX deep PixelLab parity

- Canvas default 32x24, cellSize 40, Auto-Fit on first open
- Hover preview shows REAL tile sprite (alpha 0.6) + brush radius outline
- Drag-paint Bresenham line interpolation (no missed cells)
- Active terrain thumbnail at cursor
- Pairing info panel (right) + 3-line status bar
- Smooth cubic scroll zoom
- Terrain palette: id label + pairing peer hint
- (optional) BiomeQuickEditorWindow for inline biome edit
```

CODEX_DONE.md'ye sonuç yaz.
