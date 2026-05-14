# Codex Task: Map Designer UX Overhaul — Zoom + Live Tile Preview + Brush Radius

## Context
RIMA 2D top-down roguelite, Unity 6, URP 2D.
`Assets/Editor/RimaMapDesignerWindow.cs` — tek EditorWindow dosyası.

Mevcut canvas: vertex noktaları renkli disc olarak çiziliyor (gri=floor, kahve=wall).
Pan: orta mouse butonu zaten çalışıyor.
Zoom: sadece slider (16-36px).
Brush: tek vertex boyuyor.

## Bu görevde 3 değişiklik yapılacak

### Değişiklik 1: Scroll Wheel Zoom
`HandleGridInput` fonksiyonunda, mevcut event handler'ların BAŞINA ekle:

```csharp
if (evt.type == EventType.ScrollWheel)
{
    float zoomDelta = -evt.delta.y * 2f;
    cellSize = Mathf.Clamp(cellSize + zoomDelta, 10f, 80f);
    evt.Use();
    Repaint();
    return;
}
```

Toolbar'daki slider range'i de 10f-80f olarak güncelle (mevcut 16f-36f).

### Değişiklik 2: Live Tile Preview on Canvas
`DrawGridCanvas` fonksiyonunda, `Handles.BeginGUI()` çağrısından ÖNCE tile sprite'larını çiz.

Her cell için (x: 0..roomWidth-1, y: 0..roomHeight-1):
1. 4 köşe değerini oku:
   - nw = vertGrid[x, y+1], ne = vertGrid[x+1, y+1]
   - sw = vertGrid[x, y],   se = vertGrid[x+1, y]
2. Active layer'ın tileSet'i varsa: `TileBase tile = layers[activeLayerIndex].tileSet?.GetTile(nw, ne, sw, se)`
3. Texture al: önce `(tile as Tile)?.sprite?.texture`, yoksa `AssetPreview.GetAssetPreview(tile)`
4. Cell rect'i hesapla: `new Rect(CanvasPadding + x * cellSize, CanvasPadding + (roomHeight - y - 1) * cellSize, cellSize, cellSize)`
5. Texture varsa `GUI.DrawTexture(cellRect, tex, ScaleMode.StretchToFill)`, yoksa renk fallback:
   - wangKey = (nw<<3)|(ne<<2)|(sw<<1)|se; wangKey==0 → `Color(0.20, 0.20, 0.20)`, wangKey==15 → `Color(0.45, 0.28, 0.16)`, mix → gri-kahve arası
   - `EditorGUI.DrawRect(cellRect, fallbackColor)`

Sağ panelde "Show Tiles" toggle ekle (`[SerializeField] private bool showTilePreview = true`). Bu toggle false ise tile rendering atla (sadece vertex discs göster).

`using UnityEngine.Tilemaps;` namespace zaten var, `Tile` cast için ekstra using gerekmez.

### Değişiklik 3: Brush Radius
Sağ panel "Paint Tools" bölümüne slider ekle:
```
[SerializeField] private int brushRadius = 1; // 1-5
```
`currentPaintValue` toolbar'ının hemen ALTINA:
```csharp
brushRadius = EditorGUILayout.IntSlider("Brush Radius", brushRadius, 1, 5);
```

`PaintVertex` çağrılarını (Brush modunda) `PaintWithRadius` ile değiştir:
```csharp
private void PaintWithRadius(Vector2Int center, int value)
{
    int r = brushRadius - 1;
    for (int dy = -r; dy <= r; dy++)
        for (int dx = -r; dx <= r; dx++)
            PaintVertex(new Vector2Int(center.x + dx, center.y + dy), value);
}
```
`HandleGridInput` içinde Brush modundaki `PaintVertex(hoveredVertex, value)` → `PaintWithRadius(hoveredVertex, value)` ile değiştir (MouseDown ve MouseDrag her ikisinde de).

## Okuyacağın dosyalar
- Assets/Editor/RimaMapDesignerWindow.cs

## DeğiştirMEYECEĞİN dosyalar
- CornerWangPainter.cs, CornerWangTileSetSO.cs, runtime script'ler

## Compile check
Değişiklik bittikten sonra read_console ile 0 error doğrula. Error varsa düzelt.

## Commit
```
git add Assets/Editor/RimaMapDesignerWindow.cs
git commit -m "[map-designer] Scroll wheel zoom + live tile preview + brush radius UX"
```
