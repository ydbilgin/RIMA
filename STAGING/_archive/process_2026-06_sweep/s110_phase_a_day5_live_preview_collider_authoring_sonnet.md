# S110 Phase A Day 5 — Live Preview Pane + Visual Collider Authoring

**Agent:** general-purpose Sonnet sub-agent (design + planning, no code)
**Effort:** ~40 dakika (kapsamlı spec doc)

---

## User direktifi (verbatim)

> "Şu an RIMA Room Painter'da bir sürü şey oynayıp duruyor. Ayrıca bir şeye tıklayınca yanda preview halini görecem oradan da istediğim takdirde rigid2d ekleyebilecem body'i ama mantıksal olarak ekleyebilecem 3d şeklinde"

## User'ın 2 net şikayeti / 3 net özellik isteği

### Şikayetler

1. **UI sürekli değişiyor — "bir sürü şey oynayıp duruyor"**
   - Foldout'lar, dropdown'lar, dropdown items, banner state'leri sahne refresh + asset select sırasında sürekli kayıyor olabilir
   - Codex Day 4 sonrası 30+ Inspector field eklendi, kullanıcı "kaybolmuş"

### Özellikler

2. **Live Preview Pane (CENTER pane, yeni)**
   - Asset palette'ten sprite/prefab tıkla → ortada büyük preview (256-512px)
   - Sadece sprite değil, "sahnede nasıl duracak" simulation: floor cell + cliff sprite + drop face + (varsa) drop shadow + bounding box
   - Real-time: rotation `R` → preview döner, Y-sort axis değişikliği → preview pivot oynar

3. **Visual Collider Authoring (preview üstünde, 3D-mock)**
   - Preview üstüne **collider shape outline** çizilir (Box/Circle/Capsule/Polygon)
   - Designer **drag handle'larla** edit eder (corner drag = box size; radius drag = circle; vertex add/move = polygon)
   - "3D mock" = sprite üstüne pseudo-3D depth visualization (sprite'ın aşağıya doğru sarkımı simulate edilir; collider top face + side face wireframe ile)
   - Designer Unity Inspector'a hiç gitmez

## Hedef — kapsamlı UX redesign

`STAGING/ROOM_PAINTER_DAY5_LIVE_PREVIEW_SPEC.md` yaz. **6 bölüm.**

### Bölüm 1: UI stability fixes (kullanıcı şikayeti #1) — 250 kelime

Mevcut Day 4 sonrası Inspector panel ~30 field. Designer "oynayıp duruyor" diyor. Diagnose:

- **Foldout state inconsistency:** Her asset select'te foldout'lar tekrar default'a dönüyor mu? EditorPrefs persist kontrol et.
- **Dropdown re-population:** Layer dropdown / Parallax tier dropdown her OnGUI'de yeniden enum populate ediliyor mu? Cache et.
- **Banner indicator (SO vs instance edit) flicker:** Banner her repaint'te recreate ediliyor mu? Static GUIContent kullan.
- **Section order:** Identity → Placement → Physics → Parallax → Visual → Metadata. Bu sıra fixed mi yoksa active tab'a göre değişiyor mu? FIXED tutmalı, asla swap etme.
- **Scroll position reset:** Asset değişince scroll position sıfırlanıyor mu? Önceki position'u tut.

5 fix listesi + her birinin dosya:satır referansı (RoomPainterInspectorPanel.cs ve 6 Section dosyası).

### Bölüm 2: Center pane — Live Preview architecture (500 kelime)

Yeni 3. pane: 3-pane layout `[ASSET PALETTE 300px | LIVE PREVIEW 400px | INSPECTOR 350px]`. Toolbar + status bar korunur.

#### 2.1 Live Preview İçeriği

```
+--------------------------------------------+
|  [256x256] Asset thumbnail (zoom 2-3x)     |
|                                            |
|  Box collider outline (cyan, dashed)       |
|  +---+                                     |
|  | X |  <- sprite preview                  |
|  +---+                                     |
|                                            |
|  Drop shadow disk (gray ellipse below)     |
|  Pivot anchor crosshair (red + green)      |
|                                            |
|  Y-sort axis indicator (vertical line)     |
+--------------------------------------------+
|  Tools: [Box] [Circle] [Capsule] [Poly]    |
|  [+ Trigger] [- Block]                     |
+--------------------------------------------+
|  Status: collider size 64x32, anchor (0.5, 0)
+--------------------------------------------+
```

- Background: dark checkerboard (Unity-style transparent indicator)
- Zoom: 2x default, mouse wheel +/- 1x to 8x
- Pan: middle-mouse drag

#### 2.2 Preview rendering pipeline

- `EditorGUI.DrawPreviewTexture` veya `GUI.DrawTexture` + sprite sliced rect
- Real-time `Repaint()` when:
  - Asset selection change
  - R key (rotation)
  - Inspector field change (Y-sort, pivot anchor, tint, etc.)
- Sprite null + prefab varsa: PrefabUtility.InstantiateWith preview camera mini-render

#### 2.3 "3D-mock depth" rendering

Top-down sprite üstüne pseudo-3D illusion:
- Sprite alt kenarına **drop shadow ellipse** (yarı saydam gri, sprite size 80%)
- Eğer Cliff layer: sprite alt 40%'i daha koyu **shadow ramp** (gradient simulating drop face)
- Eğer Parallax layer: sprite'ın "uzaklığı" simule edilsin — küçültülmüş + tint biraz mavi (atmospheric perspective)

### Bölüm 3: Visual Collider Authoring spec (kullanıcı şikayeti/istek #3) — 600 kelime — KRİTİK

Preview pane üzerinde **interactive collider editor**. PainterSuite'in ColliderPainter pattern'i reuse + adapt et.

#### 3.1 Tool seçici (preview alt toolbar)
- `[None]` (collider yok, sadece visual)
- `[Box]` BoxCollider2D
- `[Circle]` CircleCollider2D
- `[Capsule]` CapsuleCollider2D
- `[Polygon]` PolygonCollider2D
- `[Edge]` EdgeCollider2D (Phase B)

Tıklayınca shape live preview'e overlay olur. `[Trigger]` toggle + `[Block]` toggle (sağ üst).

#### 3.2 Box collider authoring
- Default: sprite bounds (auto-infer from texture alpha)
- Designer: 4 corner handle (Handles.FreeMoveHandle) → resize
- Visual: cyan dashed outline (Block YES) veya yellow dashed (Trigger YES)
- Live update: drag → `RoomPainterAsset.colliderSize` yazılır

#### 3.3 Circle collider authoring
- Default: sprite half-width radius
- Designer: 1 radius handle (drag dış)
- Visual: cyan dashed circle

#### 3.4 Polygon collider authoring
- Default: sprite alpha edge auto-trace (PolygonCollider2D Unity native auto-build)
- Designer: vertex handle'lar (Handles.FreeMoveHandle) → vertex move
- Designer: shift + click = vertex insert
- Designer: alt + click = vertex delete
- Designer: ctrl + click = vertex move only on X or Y
- Live: vertex array → `RoomPainterAsset.polygonVertices Vector2[]`

#### 3.5 "3D mock" collider overlay
- Collider outline cyan top, **shaded extrusion downward** (sprite drop face length)
- Visual: top face cyan + side face mid-cyan (transparent fill) → user "bu kutu 2D ama 3D mantığıyla yerleşiyor" hissi
- Toggle: `Show 3D mock` checkbox (default ON)

#### 3.6 Apply to scene
- Designer "Apply" tıklayınca: SO update + scene'deki bu asset'in tüm instance'larına collider güncelle (eğer per-instance override yoksa)
- Per-instance customization: SceneView'da seçili GO için "Apply only this instance" buton

### Bölüm 4: Rigidbody2D mantıksal ekleme (kullanıcı isteği #3 son cümle) — 250 kelime

User "rigid2d ekleyebilecem body'i ama mantıksal olarak" diyor. Yorum:
- Designer Unity Inspector'dan Rigidbody2D AddComponent yapmaz
- Inspector Physics section'ında: **Body type dropdown** (None / Static / Dynamic / Kinematic)
- Eğer Block YES + body=Dynamic → designer'a sorulur "Bu dinamik mi?" + warning (cliff dynamic anlamsız)
- Eğer Block YES + body=Static → otomatik AddComponent Rigidbody2D + bodyType=Static
- "Apply" tıklayınca SceneView'daki ilgili GO'lara propagate
- Mass, drag, gravity scale → advanced foldout (default closed)

### Bölüm 5: Day 5 yeni roadmap (Day 5 scope büyüdü)

Önceki plan: D5 Tools + Drag-drop.
Yeni plan:

| Day | İş |
|---|---|
| **D5a (1-1.5 gün)** | UI stability fixes + Center Preview pane + 3D-mock depth rendering |
| **D5b (1.5-2 gün)** | Visual Collider Authoring (Box + Circle + Polygon + Capsule + handles + apply propagation) |
| **D6 (1 gün)** | Tools (Erase/Pick/Box-select) + Drag-drop |
| **D7 (1 gün)** | Save/Load RoomData + Export Prefab |
| **D8 (1 gün)** | Parallax slider tuning + minimap + polish |
| **D9 (1 gün)** | Docs + demo room |

Phase A total: 9 gün → 10-11 gün (D5 ek 1-2 gün). Ship hedefi 1.5-2 hafta.

### Bölüm 6: Codex Day 5a + Day 5b handoff (implementable form)

#### Day 5a handoff (~10 dosya)
- New: `Assets/Editor/RoomPainter/Preview/RoomPainterPreviewPane.cs` (orchestrator, ~200 LOC)
- New: `Assets/Editor/RoomPainter/Preview/PreviewRenderer.cs` (sprite + 3D-mock depth, ~150 LOC)
- New: `Assets/Editor/RoomPainter/Preview/PreviewBackgroundDrawer.cs` (checkerboard + zoom/pan)
- Modify: `RimaRoomPainterWindow.cs` (3-pane → preview pane'i ortaya ekle, splitter rules)
- UI stability fixes: `RoomPainterInspectorPanel.cs` + 6 section files

#### Day 5b handoff (~5 dosya)
- New: `Assets/Editor/RoomPainter/Preview/ColliderAuthoring.cs` (interactive editor, 250-350 LOC)
- New: `Assets/Editor/RoomPainter/Preview/ColliderHandleDrawer.cs` (4 shape handles + 3D-mock overlay)
- New: `Assets/Editor/RoomPainter/Preview/ColliderApplier.cs` (SO → scene propagation, reuse PhysicsApplier from Day 4)
- Modify: `RoomPainterAsset.cs` (+polygonVertices, +capsuleSize, +colliderOffset fields)
- Reuse: `Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs` drag pattern, `ColliderTemplateService.cs` for shape conversions

## Çıktı

`STAGING/ROOM_PAINTER_DAY5_LIVE_PREVIEW_SPEC.md` 6 bölüm full. **3000-4000 kelime** kapsamlı.

Inline rapor (Agent yanıtı, ≤500 kelime):
- 6 bölüm done/not
- TOP 3 critical decisions
- 3 risk + mitigation
- Day 5a vs Day 5b split rationale (neden 2 ayrı dispatch)
- Codex Day 5a için handoff özet

DESIGN ONLY — kod yazma. Mevcut `STAGING/ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md` (Day 4 spec, 5200 word) ve cliff pivot memory `cliff_pivot_manual_brush_2026_05_26.md` referans al.
