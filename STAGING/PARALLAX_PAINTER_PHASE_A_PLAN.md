# RIMA Realtime Room Painter — Phase A Plan (MVP)

**Session:** S110
**Source spec:** `STAGING/s109_chatgpt_room_painter_spec.md` (ChatGPT verbatim, Hendrix-inspired)
**Reuse base:** `Packages/com.laureth.painter-suite/` (v0.3.0 window code / v0.4.0 LIVE per task spec — see Ambiguity A1)
**Total Phase A budget:** 5–9 working days
**Authoring style:** human-authored visual room builder. NOT a procgen pipeline.

> Doc kapsamı: SADECE Phase A (MVP) için scope + 3 SO + 10-layer enum + EditorWindow mimarisi + günlük plan + risk + Phase B/C teaser. Kod yok. Karar paragrafları kısa, designer-ready.

---

## Section 1 — Phase A Scope (MVP)

### 1.1 Mantra
> "Click + place + save + reload — bunun dışı Phase B'ye."

Phase A bir prefab-merkezli room builder verir. 10 layer enum **tanımlanır** (RoomLayerDefinition asset olarak) ama yalnız **4 layer aktif paint hedefi** olur. Diğer 6 layer slot'lar görünür ama disable (placeholder, "Phase B").

### 1.2 Aktif 4 Layer (Phase A)
| # | Layer | Sebep |
|---|-------|-------|
| 1 | **Floor** | Her odanın tabanı, en sık kullanılan. Tilemap optional (tek child SR de yeterli MVP için). |
| 2 | **Cliff** | RIMA wall-less Hades Elysium kilidinin core'u (S106 lock). Cliff face = sorting -10. |
| 3 | **Props** | Sütun, brazier, altar, rune. Y-sort gerektirir (player ile etkileşim). |
| 4 | **Parallax** | ParallaxLayer.cs (UPM) zaten LIVE. Painter sadece prefab/sprite yerleştirir + factor field expose eder. |

### 1.3 Phase A IN-SCOPE Davranışlar
- EditorWindow tek pencere (`RIMARoomPainterWindow`), 3 panel (asset palette / scene / inspector).
- Asset palette: configured folder list (`SerializedField string[] paletteFolders`), thumbnail grid.
- SceneView click placement (LMB place, drag move, Del delete, Ctrl+D duplicate).
- Snap toggle + snap size (default 16 px / 0.25 unit).
- Layer dropdown (4 aktif + 6 disabled).
- Per-placement sorting layer + order in layer + visualOffsetPixels.
- Save: room root prefab + RoomData SO yan yana (ör. `Assets/Data/Rooms/Room_X.prefab` + `Room_X_Data.asset`).
- Load: RoomData → scene reconstruct (designer üzerinde iterate edebilsin).
- Undo: tek paint = tek undo group (PainterSuite ColliderPainter pattern).
- Ghost preview cursor (placement öncesi sprite yarı saydam görünür).

### 1.4 Phase A OUT-OF-SCOPE (Phase B/C'ye iter)
- Collision painting (rect/polygon). → **Phase B**
- Occlusion / fade zones. → **Phase B**
- Themed brushes (Cliff Edge Brush, Wall Line Brush, Prop Scatter Brush, Rift Decal Brush). → **Phase C**
- 5 theme presets (ShatteredKeep / RitualChamber / Library / Prison / FloodedCrypt). → **Phase C**
- Tilemap auto-conversion (floor sheet → RuleTile). → **Phase B**
- JSON export (sadece `.asset` ScriptableObject Phase A). → **Phase B**
- 6 inactive layer (Edge / Wall / Decals / Lighting / Collision / Occlusion). → enum'da tanımlı, painter disable.

---

## Section 2 — Three-Tier ScriptableObject Hierarchy

### 2.1 RoomPainterAsset (palette entry)
Tek bir "stamp"-able asset tanımı. Palette'in atomik birimi.

| Field | Type | Notes |
|-------|------|-------|
| `id` | `string` | GUID veya `slug` (asset name = display fallback). |
| `displayName` | `string` | UI label. |
| `category` | `enum {Floor, Cliff, Prop, Parallax}` | Phase A için 4'lü; Phase B'de 10'a çıkar. |
| `sprite` | `Sprite` | Optional; prefab varsa fallback. |
| `prefab` | `GameObject` | Optional; sprite varsa null kalabilir. **At least one required** (validator). |
| `defaultLayer` | `RoomLayerDefinition` | SO reference, drag-drop. |
| `defaultSortingLayer` | `string` | Sorting layer name (boş ise layer'dan inherit). |
| `defaultOrder` | `int` | Order in layer override (0 = inherit). |
| `defaultScale` | `Vector2` | Default 1,1. |
| `defaultVisualOffset` | `Vector2Int` | Pixel-level offset (PPU divide at runtime). |
| `defaultCollisionMode` | `enum {None, FromSprite, Manual}` | Phase A: sadece `None` veya `FromSprite` (read-only). Manual paint = Phase B. |
| `themeTags` | `string[]` | Boş Phase A; Phase C theme preset filter için. |
| `randomWeight` | `float` | Default 1.0; Phase C scatter brush input. Phase A unused but stored. |

**Phase A validator:** sprite==null && prefab==null → console error in OnValidate.

### 2.2 RoomLayerDefinition (layer config asset)
Project-wide layer tanımı. 10 adet asset oluşturulur (`Assets/Data/RoomLayers/L1_Floor.asset` ... `L10_Parallax.asset`).

| Field | Type | Notes |
|-------|------|-------|
| `id` | `RoomLayerKind` enum | See Section 3 (10 values). |
| `displayName` | `string` | UI label, lokalize edilebilir. |
| `sortingLayer` | `string` | Unity sorting layer name. |
| `baseOrder` | `int` | Order in layer base. Per-asset override. |
| `isParallax` | `bool` | True → ParallaxLayer eklenir parent rig'e. |
| `parallaxDepth` | `Vector2` | Phase A: yalnız L10 için kullanılır (L1=0.03, L2=0.05, L3=0.08, L4=0.14 mapping doc'ta — see Section 3). |
| `locked` | `bool` | Painter UI'da locked → click ignore. |
| `visible` | `bool` | Painter toggle; runtime'da Active state. |
| `staticOrCameraRelative` | `enum {Static, RoomLocked, CameraRelative}` | ChatGPT spec madde 8. Phase A: yalnız `Static` ve `CameraRelative` (parallax). |

### 2.3 RoomData (top-level room asset)
Sahnedeki room'un serialize'lı snapshot'ı. Designer Open ettiğinde reconstruct edilir.

| Field | Type | Notes |
|-------|------|-------|
| `roomName` | `string` | Display + prefab name. |
| `roomSize` | `Vector2Int` | Bounds (tile cinsinden, snap grid'e bound). |
| `layers` | `RoomLayerDefinition[]` | Hangi layer'lar aktif (10'dan subset). |
| `placedObjects` | `RoomPlacement[]` | (struct) — asset ref + worldPos + scale + rot + sortingLayer override + order override. |
| `collisionZones` | `RoomCollisionZone[]` | **Phase A boş array, schema reserved.** |
| `occlusionZones` | `RoomOcclusionZone[]` | **Phase A boş array, schema reserved.** |
| `parallaxRigRef` | `GameObject` (prefab) | Optional — Phase A ParallaxLayer rig prefab reference. |
| `metadata` | `RoomMetadata` (struct) | author, createdAt, theme tag, RIMA biome enum (placeholder Phase A). |

**Ambiguity A2 — RoomData layout:** ChatGPT spec sadece field adı verir; tip belirsiz. Recommended interpretation: `RoomData` ScriptableObject + `[Serializable] RoomPlacement` struct (Position/Rotation/Scale/AssetGUID/SortingLayer/Order). Prefab GameObject hierarchy ile **çift kaynak**: prefab = scene runtime, SO = editor metadata.

### 2.4 Cross-reference diagram

```
                +---------------------+
                | RoomLayerDefinition |  (asset, 10 adet)
                +----------+----------+
                           ^
                           | references (defaultLayer)
                           |
+----------------------+   |    +---------------------+
| RoomPainterAsset     +---+    | RoomData            |
| (palette entry, N)   |        | (room asset, 1/room)|
+-----------+----------+        +---+--------+--------+
            ^                       |        |
            | references            | uses   | contains
            | (per placement)       |        |
            |                       v        v
            +----------------- RoomPlacement[] (struct list)
                                       |
                                       | spawn at runtime
                                       v
                              +-----------------+
                              | Scene RoomRoot  |
                              | + child layers  |
                              | (prefab export) |
                              +-----------------+
```

---

## Section 3 — 10-Layer Enum + Phase Ramp

`enum RoomLayerKind { Floor, Edge, Cliff, Wall, Props, Decals, Lighting, Collision, Occlusion, Parallax }`

| # | Layer | Phase | Default Sorting Layer | Default Order | Y-Sort? | Notes |
|---|-------|-------|------------------------|---------------|---------|-------|
| 1 | Floor | **A** | `Floor` | 0 | No | Tilemap or single SR child. Static. |
| 2 | Edge | B | `FloorEdge` | 5 | No | Floor border accent (cliff lip, walkway trim). |
| 3 | Cliff | **A** | `Cliff` | -10 | No | Per RIMA Cliff system (CliffYSortManager separate component). Face only — overhang Phase B. |
| 4 | Wall | B | `Wall` | 50 | Yes | Wall Line Brush hedefi (Phase C). |
| 5 | Props | **A** | `Props` | 100 | **Yes** | Sütun/brazier/altar. Y-sort axis Y (RIMA project convention). |
| 6 | Decals | B | `Decals` | 200 | No | Rune, crack, blood. Above floor below props in shaders. |
| 7 | Lighting | B | `Lighting` | 300 | No | URP 2D Light placements (gizmo hint Phase B). |
| 8 | Collision | B | (none) | n/a | n/a | Invisible — Phase A reserved field only. |
| 9 | Occlusion | B | (none) | n/a | n/a | Trigger volumes — Phase B. |
| 10 | Parallax | **A** | `Background` | -500 | No | Uses `LaurethStudio.PainterSuite.Runtime.ParallaxLayer`. Camera-relative. |

### 3.1 Parallax depth ramp (Phase A — sadece L10 için)
ParallaxLayer.cs (LIVE) factor presets canonical:
- Void: `(0.03, 0.02)`
- Nebula: `(0.05, 0.04)`
- Ruins: `(0.08, 0.05)`
- Islands: `(0.14, 0.08)`
- Fog: `(0.10, 0.06)`

Phase A: parallax layer asset 5 preset dropdown sağlar; custom yok. Phase B: arbitrary depth.

**Ambiguity A3 — Hangi 4 layer Phase A?** Task spec "Floor + Cliff + Props + Parallax önerim" diyor. Bu plan o öneriyi kabul ediyor. Eğer designer "Wall daha kritik" derse, Phase A'da swap önerisi: **Cliff yerine Wall yapma**, çünkü RIMA wall-less Hades Elysium LOCK (S106) wall'ı V2 LEGACY'ye gönderdi. Cliff = canonical.

---

## Section 4 — EditorWindow Architecture

### 4.1 File listesi (yeni asmdef + scripts)

**Location:** `Packages/com.laureth.painter-suite/Editor/RoomPainter/` (PainterSuite içine modül olarak — yeni asmdef değil, mevcut `LaurethStudio.PainterSuite.Editor.asmdef` extend).

> **Karar:** ChatGPT spec `/Assets/Editor/RIMA/RoomPainter/` öneriyor. **Bu plan PainterSuite paketi içine koymayı öneriyor** — sebep: ColliderPainter / ParallaxLayer / ShortcutManager altyapısı zaten orada. Paket export'lanırsa Room Painter de gider. Phase A scope dışı: ayrı asmdef. **Eğer user "Asset folder altında olsun" derse**, Phase A başında refactor maliyeti = 1 saat (sadece namespace + asmdef move).

| File | Sorumluluk |
|------|-----------|
| `Editor/RoomPainter/RIMARoomPainterWindow.cs` | Ana EditorWindow. Mode = RoomPainter (PainterMode enum'a 4. değer ekle). |
| `Editor/RoomPainter/RoomPainterScenePainter.cs` | SceneView click/drag handler. ColliderPainter.cs pattern reuse. |
| `Editor/RoomPainter/RoomAssetPaletteDrawer.cs` | Sol panel — folder scan + thumbnail grid + selection state. |
| `Editor/RoomPainter/RoomInspectorPanel.cs` | Sağ panel — seçili placement transform/sort/order/scale edit. |
| `Editor/RoomPainter/RoomSaveLoadService.cs` | Prefab + SO export/import. AssetDatabase batch wrapper. |
| `Editor/RoomPainter/Data/RoomPainterAsset.cs` | SO (Section 2.1). |
| `Editor/RoomPainter/Data/RoomLayerDefinition.cs` | SO (Section 2.2). |
| `Editor/RoomPainter/Data/RoomData.cs` | SO + nested `[Serializable] RoomPlacement` struct (Section 2.3). |
| `Editor/RoomPainter/Hotkeys/RoomPainterShortcuts.cs` | Shift+R cycle layer, Shift+G snap toggle, Del delete (PainterShortcuts.cs pattern). |

**Toplam:** 9 yeni dosya. ~800-1200 satır tahmin (Phase A için).

### 4.2 ASCII window layout

```
+--- RIMA Room Painter (EditorWindow) -----------------------------------+
| Toolbar: [Floor][Edge*][Cliff][Wall*][Props][Decals*][Light*]          |
|          [Coll*][Occ*][Parallax]    Snap [x] 16px   Mode: [Place v]    |
|          (* = disabled Phase A — Phase B label)                        |
+--- left palette (240px) -----+--- scene preview (drives SceneView) ----+
|  [Folder ▼ Assets/Art/Cliff] |                                          |
|  +--+ +--+ +--+ +--+         |  (Lives in SceneView, not in EW)        |
|  |sp| |sp| |sp| |sp|         |  Click → ghost cursor → place           |
|  +--+ +--+ +--+ +--+         |  Drag selected → move                   |
|  +--+ +--+ +--+ +--+         |  Del → remove                           |
|  |sp| |sp| |sp| |sp|         |  Ctrl+D → duplicate at offset           |
|  +--+ +--+ +--+ +--+         |                                          |
|                              +-- right inspector (260px) ---------------+
|                              | Selected placement:                      |
|                              |   Layer:    [Floor v]                    |
|                              |   Sort lyr: [Floor v]                    |
|                              |   Order:    [ 0 ]                        |
|                              |   Scale:    [1, 1]                       |
|                              |   Vis off:  [0, 0] px                    |
|                              |   Pos:      [-1.25, 0.50]                |
|                              | --- Room ---                             |
|                              |   Name: Room_01                          |
|                              |   Size: 16 x 16                          |
|                              |   [Save Prefab]   [Save Room Data]       |
|                              |   [Load Room Data]                       |
+------------------------------+------------------------------------------+
| Status bar: "Placed 47 / Selected #12 / Layer: Props"                   |
+------------------------------------------------------------------------+
```

### 4.3 SceneView overlay sorumlulukları
- **Ghost preview**: aktif palette selection cursor pozisyonunda yarı saydam çizilir (ColliderPainter `GhostFill/GhostEdge` pattern).
- **Selection gizmo**: seçili placement sarı handle (Handles.DotHandleCap).
- **Snap grid**: snap toggle açıkken kameranın görünür alanında gridLines (light grey, alpha 0.3).
- **Sorting anchor debug**: ChatGPT spec madde 5. Pivot pozisyonunda magenta cross gizmo (toggle).

### 4.4 PainterSuite altyapısından NE kullanılır

| Mevcut bileşen | Kullanım |
|----------------|----------|
| `PainterSuiteWindow` (Core) | **Pattern reuse only** — RoomPainterWindow ayrı window, **VEYA** PainterSuiteWindow'a 4. PainterMode (`RoomPainter`) ekle. **Recommendation:** ayrı window (RoomPainter ayrı use-case, mode ayrımı karışıklık yaratır). |
| `PainterShortcuts` | **Pattern reuse** — `RoomPainterShortcuts` ayrı static class, `typeof(RIMARoomPainterWindow)` context filter. |
| `ColliderPainter` (Colliders/) | **Pattern reuse** — undo group lifecycle, ghost preview, snap-to-pixel math. **Phase B'de** direkt kullanılır (collision painting). |
| `ColliderTemplate / ColliderTemplateService` | **Phase B** — template = "prop scatter preset" benzeri. |
| `ParallaxLayer` (Runtime) | **Direkt kullanım** — Parallax layer placement'ı bu component'i auto-add eder. Phase A ana entegrasyon noktası. |
| `ShapeMode` enum | Phase A unused. Phase B collision için. |
| Mevcut asmdef (`LaurethStudio.PainterSuite.Editor`) | **Genişlet** — yeni script'ler aynı asmdef altında. |

### 4.5 BrushExecutorRouter / AutoLayeringService / VisualEditorScenePainter pattern reuse

> **Ambiguity A4:** Task spec'te bu üç isim geçiyor (`BrushExecutorRouter`, `AutoLayeringService`, `VisualEditorScenePainter`). MEMORY'de `Antigravity RimaVisualMapEditorWindow refactor LIVE (Assets/Editor/MapDesigner/VisualEditor/)` ve "Native BrushExecutorRouter + ghost preview + R rotation" geçiyor. PainterSuite paketi içinde bu isimler **yok** (search confirmed). Recommended interpretation: bunlar **Assets/Editor/MapDesigner/VisualEditor/** altında RIMA-internal helper'lar. Phase A scope için:
> - **VisualEditorScenePainter pattern** — RoomPainterScenePainter.cs için reference (ghost cursor + R rotation + tile-grid placement).
> - **BrushExecutorRouter** — Phase C'de Cliff Edge Brush vb. routing için bakılacak. Phase A out.
> - **AutoLayeringService** — Phase A için faydalı: yeni placement'ın sortingLayer ve order'ı otomatik atansın. Reuse şansı **YÜKSEK**, ilk gün incele.

---

## Section 5 — Daily Breakdown (5–9 Days)

### Day 1 — Foundation
- 3 SO sınıfı stub'la (RoomPainterAsset, RoomLayerDefinition, RoomData + RoomPlacement struct).
- 10 RoomLayerDefinition asset oluştur (manuel, `Assets/Data/RoomLayers/`).
- Asmdef genişletmesi (RoomPainter klasörü).
- RIMARoomPainterWindow stub (boş window, MenuItem `LaurethStudio/RIMA Room Painter`).
- **Exit:** Window açılıyor, log var.

### Day 2 — Asset Palette
- `RoomAssetPaletteDrawer.cs` — `paletteFolders` field, AssetDatabase.FindAssets folder scan.
- Thumbnail grid (`AssetPreview.GetAssetPreview`), seçim state.
- Folder dropdown filter (Cliff / Props / Parallax / Floor).
- **Exit:** Palette'den sprite seçilebiliyor, selection state window'da görülüyor.

### Day 3 — SceneView Placement
- `RoomPainterScenePainter.cs` (ColliderPainter pattern). Mouse world position + snap.
- Ghost preview cursor (palette selection yarı saydam çizilir).
- LMB click → instantiate (`Undo.RegisterCreatedObjectUndo`).
- Drag selected, Del, Ctrl+D.
- Snap toggle + size field.
- **Exit:** Sahneye sprite konuyor, undo çalışıyor.

### Day 4 — Layer & Sorting
- Layer dropdown toolbar (10 enum, 4 aktif).
- Yeni placement → defaultLayer.sortingLayer + baseOrder assign.
- RoomInspectorPanel sağ panel — seçili placement layer/order override.
- AutoLayeringService varsa entegrasyon (Phase A için Y-sort sadece Props layer).
- Sorting anchor gizmo (magenta cross toggle).
- **Exit:** Floor altta, props üstte, Y-sort doğru çalışıyor; cliff layer üstte değil önde değil ARKA görünüyor.

### Day 5 — Save / Load RoomData
- `RoomSaveLoadService.cs`:
  - `SaveRoom(roomRoot, path)` → prefab + RoomData.asset yan yana.
  - `LoadRoom(roomData)` → scene'e yeni RoomRoot, child layer'lar, placements reconstruct.
- AssetDatabase.StartAssetEditing / StopAssetEditing batch wrapper.
- Parallax layer reconstruct: ParallaxLayer component auto-add, factor preset apply.
- **Exit:** Room kaydedilip yeniden açılabiliyor; bit-exact reconstruction.

### Day 6 — Polish
- Shortcut: Shift+R cycle layer, Shift+G snap toggle, Esc cancel placement, Del delete selected.
- HelpBox text + workflow doc.
- Snap grid visualization (SceneView light grid).
- OnValidate guard'lar (sprite/prefab both null vb.).

### Day 7 — Documentation + Bug Hunt
- `Packages/com.laureth.painter-suite/Documentation~/RoomPainter.md` (PainterSuite paketi convention'ı).
- 1 demo room sahnesi (`Assets/_Demo/RoomPainter_Demo.unity`).
- Console error sweep, undo testleri.
- Phase A close criteria: 4 layer × 5 prop = 20 placement room kaydedilebiliyor, reload eşit, undo 30 step temiz.

### Day 8-9 — Buffer
- Risk register itemleri için fix payı.
- User feedback iterasyonu.
- Erken bitti ise Phase B Day 1 başlat (collision rect painting).

**Hard stop:** Day 9 sonu. Day 9 sonunda code freeze; Phase B planning task ayrı kapı.

---

## Section 6 — Risk Register

### R1 — RIMA Cliff System çakışması (HIGH)
**Risk:** `CliffAutoPlacer.cs` + `CliffYSortManager.cs` + `CliffDynamicFade.cs` LIVE. Room Painter Cliff layer'ı bu sistemleri **ezerse** runtime'da cliff yanlış davranır. S109 evening "double auto-trigger anti-pattern" hatırlatması (CliffAutoPlacer tilemapTileChanged + LiveAutotiler MouseUp aynı anda execute → S110 Phase 1 fix bekliyor).
**Mitigation:**
- Phase A'da Cliff layer placement'ı **`CliffAutoPlacer` component'ini scene'e EKLEMEZ**, sadece sprite/prefab tek başına yerleştirir.
- RoomData.metadata'da `cliffAutoPlacerEnabled: bool = false` (default) — designer manuel açar.
- Day 1'de `CliffAutoPlacer` arayüzü oku, double-trigger kontrolü doğrula.
- Day 4 sonunda RIMA cliff scene'inde test: Room Painter ile placed cliff + LIVE auto-tiler birbirini bozmuyor.

### R2 — ParallaxLayer.cs (UPM package) entegrasyon (MED)
**Risk:** `Packages/com.laureth.painter-suite/Runtime/ParallaxLayer.cs` `_layerStart`'i OnEnable'da Capture eder. RoomPainter Load sırasında transform.position set ederse, ParallaxLayer eski origin tutar → ilk frame jump.
**Mitigation:**
- RoomSaveLoadService Load akışında: transform set → `ParallaxLayer.RecaptureOrigin()` çağır (zaten ContextMenu method'u var).
- Day 5'te bit-exact reload testinde parallax rig kontrol et (kamera hareketinde drift olmamalı).
- Fallback: ParallaxLayer Update öncesi `_captured = false` set et ve next Capture'ı tetikle.

### R3 — PainterSuite ColliderPainter namespace + window single-instance (MED)
**Risk:** Phase B'de collision painting eklenince Window iki rol oynayabilir (RoomPainter vs ColliderPainter). Aynı asmdef altında namespace flat ise dosya isim collision riski (`ColliderPainter` zaten var).
**Mitigation:**
- Yeni namespace: `LaurethStudio.PainterSuite.Editor.RoomPainter` (alt-namespace).
- Window ayrı: `RIMARoomPainterWindow` (≠ `PainterSuiteWindow`).
- Phase B'de collision tab eklenirse, ColliderPainter sınıfı **internal olarak kullanılır**, ayrı window açılmaz.
- Day 1'de namespace planı confirm.

### R4 (bonus) — Version drift (LOW)
**Risk:** `package.json` v0.1.0, window header v0.3.0, task spec "v0.4.0 LIVE". Phase A Day 1'de Room Painter ekleyince hangisi?
**Mitigation:** package.json'ı v0.4.0'a bump et + window header'ı sync et. PR description'da version source of truth = package.json.

---

## Section 7 — Phase B/C Teaser

### Phase B (Week 2 — 5-7 days)
- **Collision Painting** — ColliderPainter pattern Room Painter'a embed. Layer 8. Box/Circle/Polygon/Edge zone'lar RoomData.collisionZones'a serialize.
- **Occlusion / Fade Zones** — Layer 9. Polygon trigger, opacity field, runtime ProxyOcclusionFade component.
- **Parallax Depth Controls** — Custom factor field, 5 preset + manual. Per-layer override. Camera-relative toggle UI.
- **JSON Export** — RoomData → JSON yan dosyası (CI/Git diff friendly). Designer-facing: ham asset GUID'leri görünür.
- **Remaining 6 Layer Activation** — Edge, Wall, Decals, Lighting (URP 2D Light wrapper), Collision, Occlusion. Hepsi paint targets.
- **Tilemap Auto-Convert** — Floor layer için bulk sprite → RuleTile painter (optional).

### Phase C (Week 3 — 5-7 days)
- **Cliff Edge Brush** — Floor sınırını drag, otomatik cliff face/cap yerleşir (RIMA `CliffAutoPlacer` ile router üzerinden konuşur).
- **Wall Line Brush** — Drag line, straight/corner/cap/door auto-place. Wall-less Hades Elysium kilidiyle çakışmamak için **opt-in**, default off.
- **Prop Scatter Brush** — Themed weighted random scatter. RoomPainterAsset.themeTags + randomWeight kullanılır.
- **Rift Decal Brush** — Cyan crack/glow decals, Layer 6. RIMA yarık 3-scale visual language ile sync (S106 lock).
- **5 Room Theme Presets:**
  - ShatteredKeep
  - RitualChamber
  - Library
  - Prison
  - FloodedCrypt
  Her preset = RoomLayerDefinition setpiece + RoomPainterAsset filter + brush defaults.

---

## Open Ambiguities (Recorded)

| # | Issue | Recommended Interpretation |
|---|-------|----------------------------|
| A1 | Package version drift: package.json v0.1.0, window v0.3.0, task spec v0.4.0 LIVE. | Day 1: bump package.json → v0.4.0, sync window. |
| A2 | RoomData "metadata" tip belirsiz. | `[Serializable] RoomMetadata` struct: author/createdAt/themeTag/biome enum. |
| A3 | "Hangi 4 layer Phase A?" task spec öneriyor, alternatif var. | Floor + Cliff + Props + Parallax kabul. Wall yerine **Cliff** tercih (S106 walless lock). |
| A4 | `BrushExecutorRouter` / `AutoLayeringService` / `VisualEditorScenePainter` PainterSuite paketinde yok. | RIMA-internal helper'lar (`Assets/Editor/MapDesigner/VisualEditor/`). Day 1'de tara, AutoLayeringService reuse'u önceliklendir. |
| A5 | ChatGPT spec code location `/Assets/Editor/RIMA/RoomPainter/` öneriyor, plan PainterSuite paketi öneriyor. | PainterSuite paketi içinde (`Packages/.../Editor/RoomPainter/`) — reuse maksimum, paket export'lanırsa Room Painter de gider. User isterse Day 1 refactor 1 saat. |

---

**End of Phase A Plan.** Phase B planning kapısı ayrı task — Phase A close kriteri Day 9 sonu, 4 layer × 5 placement × 20 prop = test room round-trip.
