# S74 Batch B — Map Designer PixelLab-style UI redesign

**Effort:** high
**Prereq:** Batch A merged (TilesetPairing.transitionSize var, Auto-BiomePreset Builder exists, RoomDesigner archived)
**Reference:** `STAGING/pixellab_map_export_analysis_LOCK.md`
**File:** `Assets/Editor/RimaMapDesignerWindow.cs` (1712 satır — kapsamlı refactor)

---

## HEDEF
Kullanıcı dedi: **"PixelLab Map Tool gibi kullanıcı dostu olsun."**
Şu an: çok buton, karmaşık layer/cell/vertex toggle, terrain palette renkli buton (preview yok), mouse koordinat algısı oturmuyor.
İstenen: temiz, tek-akış, PixelLab benzeri UX.

---

## TASARIM KARARLARI (LOCKED)

### 1. Single grid, no multi-layer
Multi-terrain refactor S73'te zaten yapıldı — tek `terrainGrid` per cell. Multi-layer (List<MapLayer>) artık **GEREKSİZ**. Kaldır:
- `List<MapLayer> layers` → **TEK output** `Tilemap outputTilemap` + `CornerWangTileSetSO` (artık biome'dan resolved, ayrıca tutmaya gerek yok)
- ReorderableList layerList → KALDIRILDI
- MapLayer class → KALDIRILDI (MapSaveData/LayerSaveData korunuyor backward compat için)

### 2. Cell/Vertex toggle KALDIR (default Cell)
PaintMode enum kalıyor (Vertex ileri seviye için) ama UI'da görünmesin. Default `PaintMode.Cell`. Vertex moduna sadece Advanced foldout içinden geçilsin.

### 3. Terrain palette = TILE THUMBNAIL + isim
Şu an: GUILayout.Button("Wall", height=40) renkli arka plan.
**Yeni:** her terrain butonu = 64x64 alan; üst 48x48 baseTile sprite preview, alt 12px renkli alan + name label. Hover: highlight. Selected: 2px parlak kenar.

Render kodu örnek:
```csharp
Rect btnRect = GUILayoutUtility.GetRect(60f, 72f, GUILayout.Width(60f));
Rect spriteRect = new Rect(btnRect.x + 6f, btnRect.y + 6f, 48f, 48f);
Rect labelRect = new Rect(btnRect.x, btnRect.y + 54f, 60f, 16f);

EditorGUI.DrawRect(btnRect, terrain.paletteColor * 0.5f);
if (active) {
    Handles.color = Color.cyan;
    Handles.DrawSolidRectangleWithOutline(btnRect, Color.clear, Color.cyan);
}
// Draw sprite preview via GUI.DrawTextureWithTexCoords (DrawLiveTilePreviewCells gibi)
TileBase tile = terrain.baseTile;
Sprite sp = (tile as Tile)?.sprite;
if (sp != null && sp.texture != null) {
    Rect tc = new Rect(sp.rect.x/sp.texture.width, sp.rect.y/sp.texture.height,
                       sp.rect.width/sp.texture.width, sp.rect.height/sp.texture.height);
    GUI.DrawTextureWithTexCoords(spriteRect, sp.texture, tc);
} else {
    EditorGUI.DrawRect(spriteRect, terrain.paletteColor);
}
GUI.Label(labelRect, terrain.name, EditorStyles.miniBoldLabel);

if (Event.current.type == EventType.MouseDown && btnRect.Contains(Event.current.mousePosition)) {
    activeTerrainId = terrain.id;
    eraseMode = false;
    Event.current.Use();
    Repaint();
}
```

GridLayout: 3 sütun yan yana, sığmazsa wrap. Maksimum görsel yoğunluk.

### 4. Right panel sadeleştir
Şu an: PAINT (büyük buton), ERASE (büyük buton), Brush slider, Cell/Vertex, Advanced foldout (Procedural + Tool + Resize + ShowTilePreview).

**Yeni:** 
```
[Right Panel - 200px wide]
─────────────────────────
  ┌──ERASE──┐  ← toggle button (red when on)
  └────────┘
  
  Brush ●────○──○  (1-5)
  
  ── Advanced ▼ ──
    [ ] Vertex mode
    Tool: Brush | Fill | Rect
    Room W: [16]  H: [12]
    [Resize]
    [ ] Show tiles
  ─────────────
  
  ── Procedural ▼ ──
    [Rectangular]
    [L-Shape]
    [Perlin Noise]
  ─────────────
```

PAINT default mode (terrain selected → click paint), erase explicit toggle.

### 5. Status bar daha açıklayıcı
Şu an: çok bilgi tek satır, mouse off-canvas iken cell info kaybolur.

**Yeni:** 2 satır:
```
Line1: Room 16x12 | Biome: Shattered Keep | Active: Wall (id=1) | Output: Floor_Tilemap | Erase: Off
Line2 (sadece mouse canvas içindeyken): Cell (5,3) | Corners: NW=1 NE=0 SW=1 SE=0 | WangKey=10 | TileSet: floor_wall
Line2 (off-canvas): Tip: Drag to paint, Space+drag to pan, scroll to zoom, +/- to zoom
```

Status bar height 22 → 40 (iki satır için).

### 6. Mouse precision iyileştirme
`BrushInputHandler.GetCellAtMouse` kontrol:

```csharp
// Şu anki
int x = Mathf.FloorToInt((mousePos.x - canvasPadding) / cellSize);
int invertedY = Mathf.FloorToInt((mousePos.y - canvasPadding) / cellSize);
int y = roomH - invertedY - 1;
```

Bu MATEMATİKSEL OLARAK DOĞRU AMA: `canvasPadding` shaped offset, scroll view ile koordinat dönüşümleri arasında 1-2 pixel sapma olabiliyor. **Çözüm:**
1. `cellSize` her zaman integer çek `cellSize = Mathf.Round(cellSize)` — subpixel kaybı önle.
2. `CanvasPadding` 24f sabit — değiştirme ihtiyacı yok.
3. **EKSTRA:** hover preview rect'i DrawCellHover'da `Mathf.Round` ile yuvarla — visual feedback pixel-aligned olsun.
4. **DEBUG MODE:** Advanced foldout altında "Show mouse debug" toggle ekle → status bar'a `mouse=(mx,my) cellSize=N padding=N → cell=(x,y)` yaz.

Mouse algısı oturmaması büyük ihtimal `roomHeight - invertedY - 1` ifadesindeki invertY ile pure-canvas vs scroll-content space mix. Test:
- Cell (0,0) sol-alt köşe olmalı. Mouse oraya tıkla → hoveredCell=(0,0) doğrula.
- Cell (15,11) sağ-üst olmalı. Mouse sağ-üst → (15,11) doğrula.

Eğer sapma varsa: `Event.current.mousePosition` BeginScrollView içinde mi DIŞINDA mı çağrılıyor incele. ŞU AN içeride çağrılıyor (HandleGridInput → DrawCenterPanel → BeginScrollView içinde) — doğru olmalı.

### 7. Toolbar sadeleştir
Şu an: New, Save, Load, Apply to Scene, Generate Room, Clear All, Cell/Vertex toggle, Erase toggle, Fit, Cell size slider.

**Yeni toolbar (tek satır):**
```
[New] [Save] [Load] | [Apply] [Generate] [Clear] | [Fit] [─ Cell ●──○ 32px ─] [Auto-Biome]
```

- Cell/Vertex toggle KALDIR (toolbar'dan)
- Erase toggle KALDIR (toolbar'dan, sağ panel'de var)
- **Yeni: [Auto-Biome] buton** → AutoBiomePresetBuilder menu trigger (RIMA > Tools > Auto-Build BiomePreset)
- **Yeni: [Generate]** RoomGeneratorWindow.Open(this) zaten var, ismi kısalt

### 8. Sol panel sadeleştir
Şu an: Biome dropdown + Terrains list + Output (Tilemap + Enabled toggle).

**Yeni:**
```
[Biome ▼ Shattered_Keep_F1_BiomePreset]
[New Biome] [Edit Biome]   ← yeni butonlar (Edit = Selection.activeObject = biome)

── Terrains (3 columns wrapping) ──
[Sprite] [Sprite] [Sprite]
 Wall    Path    Rift
[Sprite] [Sprite]
 Moss    ...

── Output ──
[Tilemap field]
```

"New Biome" → CreateInstance<RimaBiomePreset>() + SaveAssetAs dialog.
"Edit Biome" → Selection.activeObject = activeBiome; (Inspector açar, kullanıcı manual edit yapsın).

### 9. Object Layer placeholder (Faz 1.5 stub)
Toolbar'a `[Objects]` butonu ekle. Tıklanınca `EditorUtility.DisplayDialog("Object Layer", "Objects (NPCs, props, spawn points) coming in Faz 1.5", "OK")`. Şimdilik şamandıralı.

### 10. Compile + test

Compile temiz. Mevcut Save/Load JSON formatı backward compat (MapSaveData.layers null/empty olabilir → tek terrainGrid yeter).

---

## YENI BIOME EDIT WINDOW (opsiyonel ama önerilen)

PixelLab UI'ında biome edit ayrı bir panel gibi açılıyor. Bizim için: **Selection-based** yeterli (Edit Biome butonu → biome asset Inspector'da). Custom Inspector eklemek istersen:

**Yeni dosya (opsiyonel):** `Assets/Editor/RimaBiomePresetInspector.cs`
- CustomEditor(typeof(RimaBiomePreset))
- terrains array için: her satırda id (read-only), name, paletteColor, baseTile drop, + Delete buton
- tilesetPairings için: lowerTerrainId dropdown (terrain names), upperTerrainId dropdown, tileSet drop, transitionSize slider, transitionDescription text area
- + Add Terrain / + Add Pairing butonları

Bu opsiyonel — eğer batch çok büyürse Faz 1.5'a ertele.

---

## ÇIKTI

Sıralı yap, her aşamada compile check:
1. MapLayer/ReorderableList kaldır (büyük refactor — 200+ satır siliniyor)
2. UI sırası: Toolbar → Sol panel → Right panel → Status bar yeni layouts
3. Mouse debug toggle ekle
4. Auto-Biome toolbar button + Object placeholder
5. Compile temiz, Unity console log'da error 0

Tek commit:
```
[S74-B] Map Designer PixelLab-style UI redesign

- Removed multi-layer system (single terrainGrid)
- Terrain palette: tile thumbnails + names (PixelLab-style)
- Simplified toolbar/right panel/left panel
- 2-line status bar with tips off-canvas
- Mouse debug toggle, integer cellSize for pixel-perfect feedback
- [Auto-Biome] toolbar button -> AutoBiomePresetBuilder menu
- [Objects] placeholder for Faz 1.5
```

CODEX_DONE.md'ye yaz, sonuç + screenshot path özetle.
