# Codex Dispatch FIX: Map Designer Mouse Alignment + UI Basitleştirme

## Context
**KRİTİK:** Dispatch 1 (`commit f871495`) implementation çalışıyor ama UX BOZUK:
- Kullanıcı: "mouse doğru yerde değil" — tıkladığı cell paint olmuyor
- Kullanıcı: "tilesete basınca kaplıyor" — palette click sonrası UI bozuluyor
- Kullanıcı: "direkt 16lı hallerde gride bölünmüş" — Wang preview panel overflow
- Kullanıcı: "tuşlar çalışmıyor"

**Root cause analizi:**
- Right panel 200px sabit
- `DrawTilePreviewPanel` (16 tile × 4 col × 80px + label) **right panel'i taşırıyor**
- Tileset seçince Wang preview yeniden render → layout shift → center canvas alanı değişiyor → mouse coord hesabı bozuluyor
- `DrawCenterPanel`'in `centerRect = GUILayoutUtility.GetRect(...)` çağrısı her frame farklı dönebiliyor

## Ek Sorun — 32×32 vs 16 confusion

Kullanıcı: "her gride normalde 32x32 hali olması gerkemiyor mu 16'lı hali eklenmiş"

- **cellSize default = 28** (kullanıcı 32 bekliyor, tile gerçek boyutu)
- **16-piece Wang preview panel** kullanıcıyı yanıltıyor — "1 cell paint = 1 tile" sanılıyor ama yanında 16 thumbnail görünce kafa karışıyor
- **FIX:** default cellSize=32 yap + Wang preview kaldır + canvas hover'da net "1 cell" highlight (32×32 görsel boyut)

## Hedef: Mouse-Perfect + Minimal UI

### Görev 1: Wang Tile Preview Panel'i Right Panel'den KALDIRR

`RimaMapDesignerWindow.DrawRightPanel`:
- `DrawTilePreviewPanel(...)` çağrısını **SİL**
- `DrawTilePreviewPanel` metodunu **SİL** (tüm gövdesi)
- `tilePreviewFoldout` field'ını **SİL**
- `GetTilePreview`, `GetTilePreviewBackground`, `GetCornerTooltip` metodları artık kullanılmıyorsa **SİL**
- `CornerKeyNames` array sadece status bar'da kullanılıyorsa kalsın, değilse sil

Sebep: Wang preview panel UX için kritik değil (kullanıcı zaten canvas'ta live tile preview görüyor). Layout overflow'a sebep oluyor.

### Görev 2: Right Panel Minimal UI

`DrawRightPanel` aşağıdaki şekilde **YENİDEN YAZ** (üzerine yaz, eski kodu sil):

```csharp
private void DrawRightPanel()
{
    EditorGUILayout.BeginVertical(GUILayout.Width(RightPanelWidth), GUILayout.ExpandHeight(true));
    
    // === BIG BUTTONS: Wall / Floor / Erase ===
    EditorGUILayout.LabelField("Paint", EditorStyles.boldLabel);
    
    GUIStyle bigButton = new GUIStyle(GUI.skin.button) { fixedHeight = 36f, fontSize = 12, fontStyle = FontStyle.Bold };
    
    bool isWall = !eraseMode && currentPaintValue == 1;
    bool isFloor = !eraseMode && currentPaintValue == 0;
    bool isErase = eraseMode;
    
    Color prevBg = GUI.backgroundColor;
    GUI.backgroundColor = isWall ? new Color(0.8f, 0.4f, 0.2f) : prevBg;
    if (GUILayout.Button("WALL", bigButton)) { currentPaintValue = 1; eraseMode = false; }
    GUI.backgroundColor = isFloor ? new Color(0.3f, 0.5f, 0.8f) : prevBg;
    if (GUILayout.Button("FLOOR", bigButton)) { currentPaintValue = 0; eraseMode = false; }
    GUI.backgroundColor = isErase ? new Color(0.9f, 0.3f, 0.3f) : prevBg;
    if (GUILayout.Button("ERASE", bigButton)) { eraseMode = true; }
    GUI.backgroundColor = prevBg;
    
    EditorGUILayout.Space(6f);
    
    // === Brush Radius ===
    brushRadius = EditorGUILayout.IntSlider("Brush", brushRadius, 1, 5);
    
    EditorGUILayout.Space(6f);
    
    // === Paint Mode (Cell/Vertex toggle) ===
    paintMode = (PaintMode)GUILayout.Toolbar((int)paintMode, new[] { "Cell", "Vertex" });
    
    EditorGUILayout.Space(8f);
    
    // === ADVANCED (collapsed by default) ===
    proceduralFoldout = EditorGUILayout.Foldout(proceduralFoldout, "Advanced", true);
    if (proceduralFoldout)
    {
        EditorGUILayout.LabelField("Procedural Helpers", EditorStyles.miniBoldLabel);
        if (GUILayout.Button("Make Rectangular Room")) MakeRectangularRoom();
        if (GUILayout.Button("L-Shape Room")) MakeLShapeRoom();
        if (GUILayout.Button("Perlin Noise Fill")) PerlinNoiseFill();
        
        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("Tool", EditorStyles.miniBoldLabel);
        activeTool = (PaintTool)GUILayout.Toolbar((int)activeTool, new[] { "Brush", "Fill", "Rect" });
        
        EditorGUILayout.Space(4f);
        EditorGUILayout.LabelField("Room Size", EditorStyles.miniBoldLabel);
        int newW = EditorGUILayout.IntField("W", roomWidth);
        int newH = EditorGUILayout.IntField("H", roomHeight);
        newW = Mathf.Clamp(newW, MinRoomSize, MaxRoomSize);
        newH = Mathf.Clamp(newH, MinRoomSize, MaxRoomSize);
        if (newW != roomWidth || newH != roomHeight) { roomWidth = newW; roomHeight = newH; }
        if (GUILayout.Button("Resize")) ResizeGrid(roomWidth, roomHeight, true);
        
        EditorGUILayout.Space(4f);
        showTilePreview = EditorGUILayout.Toggle("Show Tiles on Canvas", showTilePreview);
    }
    
    EditorGUILayout.EndVertical();
}
```

`proceduralFoldout` default değerini **false** yap.

### Görev 2.5: cellSize Default = 32

```csharp
[SerializeField] private float cellSize = 32f; // was 28f
```

Tile gerçek boyutu 32 — editor canvas'ı 32 göstermek "1 cell = 1 tile" hissini netleştirir.

### Görev 3: Mouse Alignment FIX

`DrawCenterPanel` ve `HandleGridInput`'ta koord sistemi sorunu var. **TANI**:

GUI.BeginScrollView içinde `Event.current.mousePosition` viewRect-local coordsta. Ama `centerRect.Contains(evt.mousePosition + gridScroll)` kontrolü mismatch (centerRect window space'te).

**FIX:**

```csharp
private void DrawCenterPanel(float height)
{
    Rect centerRect = GUILayoutUtility.GetRect(0f, height, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
    float canvasWidth = (roomWidth * cellSize) + CanvasPadding * 2f;
    float canvasHeight = (roomHeight * cellSize) + CanvasPadding * 2f;
    Rect viewRect = new Rect(0f, 0f, canvasWidth, canvasHeight);

    gridScroll = GUI.BeginScrollView(centerRect, gridScroll, viewRect, true, true);
    
    // İçeride mousePosition zaten viewRect-local. Sadece viewRect bounds kontrolü yap.
    DrawGridCanvas(viewRect);
    HandleGridInput(viewRect);  // ← centerRect değil, viewRect geç
    
    GUI.EndScrollView();
}

private void HandleGridInput(Rect viewRect)
{
    Event evt = Event.current;
    Vector2 mouse = evt.mousePosition; // viewRect-local
    
    // viewRect içinde mi kontrol et (zaten içeride sayılır, scroll view clip ediyor)
    bool inCanvas = viewRect.Contains(mouse);
    
    if (evt.type == EventType.ScrollWheel && inCanvas) {
        cellSize = Mathf.Clamp(cellSize - evt.delta.y * 2f, 10f, 80f);
        evt.Use(); Repaint(); return;
    }
    
    hoveredCell = brushInput.GetCellAtMouse(mouse, cellSize, CanvasPadding, roomHeight);
    hoveredVertex = GetNearestVertex(mouse);
    
    // Pan middle mouse...
    if (evt.type == EventType.MouseDown && evt.button == 2) {
        isPanning = true; panStartMouse = mouse; panStartScroll = gridScroll;
        evt.Use(); return;
    }
    if (evt.type == EventType.MouseDrag && isPanning && evt.button == 2) {
        gridScroll = panStartScroll - (mouse - panStartMouse);
        evt.Use(); Repaint(); return;
    }
    if (evt.type == EventType.MouseUp && evt.button == 2) {
        isPanning = false; evt.Use(); return;
    }
    
    if (!inCanvas) return;
    
    bool hasPaintTarget = paintMode == PaintMode.Cell
        ? brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight)
        : IsValidVertex(hoveredVertex);
    if (!hasPaintTarget) {
        if (evt.type == EventType.MouseMove) Repaint();
        return;
    }
    
    // Paint logic (mevcut kalır)
    // ...
}
```

### Görev 4: Visual Debug Overlay

Mouse fix doğrulamak için, `DrawGridCanvas`'ta `hoveredCell` çevresinde **belirgin yeşil kontur** çiz (Cell mode'da). Bu sayede kullanıcı hover'daki cell'i NET görüyor:

```csharp
if (paintMode == PaintMode.Cell && brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight)) {
    Rect cellRect = new Rect(
        CanvasPadding + hoveredCell.x * cellSize,
        CanvasPadding + (roomHeight - hoveredCell.y - 1) * cellSize,
        cellSize, cellSize);
    Color hi = eraseMode ? new Color(1f, 0.2f, 0.2f, 0.4f) : new Color(0.2f, 1f, 0.4f, 0.4f);
    EditorGUI.DrawRect(cellRect, hi);
    // Border
    Handles.color = eraseMode ? Color.red : Color.green;
    Handles.DrawSolidRectangleWithOutline(cellRect, Color.clear, Handles.color);
}
```

### Görev 5: ITERATIVE TEST (zorunlu)

Implementasyondan sonra şu testleri **execute_code ile** yap:

#### Test A — Window açılışı bozuk değil
```csharp
var winType = System.Type.GetType("RIMA.Editor.RimaMapDesignerWindow,Assembly-CSharp-Editor");
var open = winType.GetMethod("Open", BindingFlags.Public | BindingFlags.Static);
open?.Invoke(null, null);
// Repaint
EditorApplication.delayCall += () => {
    var w = Resources.FindObjectsOfTypeAll(winType).FirstOrDefault();
    if (w == null) Debug.LogError("Window not created");
    else Debug.Log("Window opened: " + (w as EditorWindow).position);
};
```

Verify: 0 error in console after window open.

#### Test B — Mouse coord precision
Mouse-perfect olduğunu doğrulamak için:
1. Window aç, room 20×15, cellSize=28
2. CanvasPadding=24
3. Simulate mouse at (CanvasPadding + 5*28 + 14, CanvasPadding + 9*28 + 14) — should be middle of cell (5, ?)
   - mouseY = 24 + 252 + 14 = 290
   - invertedY = (290 - 24) / 28 = 9 (floor)
   - y = 15 - 9 - 1 = 5
   - Expected cell: (5, 5)
4. Call brushInput.GetCellAtMouse() with that coord, verify result is (5, 5)

```csharp
var brushInput = winType.GetField("brushInput", BindingFlags.NonPublic|BindingFlags.Instance).GetValue(w);
var getCell = brushInput.GetType().GetMethod("GetCellAtMouse");
Vector2 testMouse = new Vector2(24 + 5*28 + 14, 24 + 9*28 + 14);
var cell = (Vector2Int)getCell.Invoke(brushInput, new object[] { testMouse, 28f, 24f, 15 });
Debug.Assert(cell == new Vector2Int(5, 5), "Expected (5,5) got " + cell);
Debug.Log("Mouse precision test: " + (cell == new Vector2Int(5, 5) ? "PASS" : "FAIL " + cell));
```

#### Test C — UI layout
Right panel buton ve kontrolleri 200px içine sığıyor mu? Wang preview KALDIRILDI mı?
```csharp
// Grep check: DrawTilePreviewPanel kullanılmıyor olmalı
var win = Resources.FindObjectsOfTypeAll(winType).FirstOrDefault() as EditorWindow;
var methodNames = winType.GetMethods(BindingFlags.NonPublic|BindingFlags.Instance).Select(m => m.Name).ToList();
Debug.Assert(!methodNames.Contains("DrawTilePreviewPanel"), "DrawTilePreviewPanel must be removed!");
Debug.Log("UI methods: " + string.Join(",", methodNames.Where(n => n.StartsWith("Draw"))));
```

#### Test D — E2E paint
1. Map Designer aç
2. Sol palette'te `FloorWall` SO seç (ilk SO)
3. Active layer'a auto-assign olduğunu doğrula
4. PaintCell (5, 5) value=1 → vertex grid'de (5,5)(6,5)(5,6)(6,6) hepsi 1
5. PaintCell (5, 5) value=0 → hepsi 0 dönmeli (eraseMode test)
6. ApplyToScene → tilemap'te tile sayısı > 0

#### Test E — EDITOR Screenshot (Map Designer Window)
**GAME VIEW DEĞİL** — Editor'daki Map Designer window'unun kendisini yakala. PowerShell ile:
```csharp
// Save window pos first
var w = Resources.FindObjectsOfTypeAll(winType).FirstOrDefault() as EditorWindow;
w.Focus();
Rect pos = w.position; // editor window position (screen coords)

// Then use System.Diagnostics to run PowerShell screenshot of that region
string psCmd = $@"
Add-Type -AssemblyName System.Windows.Forms;
Add-Type -AssemblyName System.Drawing;
$bmp = New-Object System.Drawing.Bitmap {pos.width:F0}, {pos.height:F0};
$g = [System.Drawing.Graphics]::FromImage($bmp);
$g.CopyFromScreen({pos.x:F0}, {pos.y:F0}, 0, 0, $bmp.Size);
$bmp.Save('F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\qc_editor_fix.png');
$g.Dispose(); $bmp.Dispose();
";
// Run via Process.Start
System.Diagnostics.Process.Start("powershell.exe", $"-NoProfile -Command \"{psCmd.Replace("\"","\\\"")}\"");
System.Threading.Thread.Sleep(2000);
```

Sonra dosyayı kontrol et: `STAGING/qc_editor_fix.png` > 30KB. 

Bu Editor screenshot **kullanıcının gerçek gördüğü** şey. QC'de bunu inceleyebilirim.

#### Test F — Status bar görsel doğrulama
```csharp
// Status bar mesajı:
// "Cell (5,7) → WangKey 12 (N Edge) | FloorWall_CornerWangTileSet"
// Bu mesaj her hover'da update olmalı, kullanıcı NE OLDUĞUNU anlasın.
```
RimaMapDesignerWindow `DrawStatusBar`'a hover bilgisi ekle:
```csharp
private void DrawStatusBar() {
    string s = $"Room {roomWidth}×{roomHeight}";
    if (brushInput.IsValidCell(hoveredCell, roomWidth, roomHeight)) {
        var grid = GetActiveLayer()?.vertGrid;
        if (grid != null) {
            int nw = grid[hoveredCell.x, hoveredCell.y + 1];
            int ne = grid[hoveredCell.x + 1, hoveredCell.y + 1];
            int sw = grid[hoveredCell.x, hoveredCell.y];
            int se = grid[hoveredCell.x + 1, hoveredCell.y];
            int wangKey = (nw << 3) | (ne << 2) | (sw << 1) | se;
            s += $" | Cell ({hoveredCell.x},{hoveredCell.y}) WangKey={wangKey}";
        }
    }
    GUI.Label(new Rect(0, position.height - StatusHeight, position.width, StatusHeight), s);
}
```

## Allowed Files
**Modify:**
- Assets/Editor/RimaMapDesignerWindow.cs (DrawRightPanel rewrite + HandleGridInput coord fix + remove DrawTilePreviewPanel)

**DO NOT TOUCH:**
- Diğer hiçbir dosya (Karar #131 lock + Dispatch 1'in stable kodu)

## Commit
```
git add Assets/Editor/RimaMapDesignerWindow.cs
git commit -m "[map-designer fix] Mouse coord precision + minimal right panel UI (remove Wang preview)"
```

## QC Done Criteria
- [ ] Test B PASS (mouse precision)
- [ ] Test C PASS (DrawTilePreviewPanel kaldırıldı, UI buton bazlı)
- [ ] Test D PASS (palette → assign → paint cell → vertex set)
- [ ] Test E (screenshot 50KB+, görsel doğru)
- [ ] read_console 0 error 0 warning

Tahmini süre: 2-3h.
