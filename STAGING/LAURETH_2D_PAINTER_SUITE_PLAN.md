# LaurethStudio 2D Painter Suite -- Implementation Plan

**Version:** 1.0
**Date:** 2026-05-26
**Status:** DRAFT for cross-account review
**Author:** Drafted via Opus 4.7 + Codex + Opus sub-agent synthesis, based on conversation with project owner (laurethgame@gmail.com)
**Source inspiration:** Hendrix Realtime Parallax Map Builder (RPG Maker MZ, $17.99)
**Strategic path:** Yol B (dual-use: RIMA tooling + Unity Asset Store product)

---

## 0. NASIL OKUNUR

Bu dokuman baska bir hesapta degerlendirilmek uzere yazildi. Tek pass ile okunabilir, ASCII-only, hicbir dis dosyaya bagimli degil.

- **Bolum 1-3** = neden / ne / kim icin (yonetici ozeti)
- **Bolum 4-6** = mimari + ozellik + UI/UX spec'i (teknik karar)
- **Bolum 7** = haftalik kirilim + AI dispatch breakdown
- **Bolum 8-10** = risk, kabul kriteri, post-launch
- **Ek A-C** = dispatch sablonlari (Codex/PixelLab/Gemini icin)

Degerlendirme yapacak agent icin: bu plan **agresif AI pipeline**'a gore takvimlenmis (Codex + Gemini + PixelLab + UnityMCP + Claude orchestration). Geleneksel solo dev takvimine cevirmek istersen carpan **3-4x**.

---

## 1. EXECUTIVE SUMMARY

LaurethStudio adi altinda Unity Asset Store'a tek paket olarak **"2D Painter Suite"** cikarmak. Paket icinde:

1. **Iso/2D Parallax Layer Painter** -- Hendrix esinli, SceneView'de realtime layer painting (sort axis aware, iso destekli)
2. **Visual Collider Painter** -- SceneView'de mouse drag ile dogrudan Collider2D olusturma + duzenleme + template + copy-paste. Unity'nin Inspector-driven collider editing'inin alternatifi. **Killer USP.**
3. **Demo Asset Pack** -- 3-5 prop sprite + collider template + parallax demo scene (buyer'a starter, satici icin dogfood)

**Hedef:** RIMA gelistirmesini yavaslatmadan, dogal yan urun olarak 5-6 haftada Asset Store submit. RIMA dogfood test pilot. Sonradan bagimsiz urun olarak buyume yolu acik (UPM package modularity ile).

**Fiyat:** $45-55 USD (Asset Store sweet spot, tek-tool $35 yerine 3-degerli paket).

**Beklenen pazar:** Unity 2D top-down/iso/2.5D indie devs, ~5000-15000 aktif hedef kitle, **Visual Collider Painter dominant rakipsiz**.

---

## 2. PRODUCT VISION

### 2.1 Neden Bu Urun?

Unity'nin 2D pipeline'inda iki ayrı pain point var:

**Pain 1: Decoration layer composition**
- Tilemap grid-bound, esnek degil
- Manuel sprite placement = lots of GameObjects + sort order chaos
- Iso/2.5D sort axis konfigurasyonu hata baslangıcı
- "Photoshop'taki gibi layer dusunsem" hissi karsiliksiz

**Pain 2: Collider editing**
- Inspector-driven, Edit Collider mode'a gir-cik
- Cok sayida collider eklemek = multi-component spam, organize zor
- Template/preset sistemi yok (her seferinde sifirdan)
- Asset pack icinde paylasimi imkansiz (data lock-in yok)

**Bizim cozumumuz:** Tek paket, iki pain'i birden coz, ortak SceneView paradigmasi ile.

### 2.2 Hedef Kitle Profili

| Persona | Anaihtiyac | Pazar boyutu |
|---|---|---|
| Solo iso/2.5D ARPG dev (RIMA tipi) | Iso parallax + per-prop unique collider | ~5000 |
| 2D top-down roguelite/adventure dev | Tilemap augmentation + decoration | ~15000 |
| Visual novel + adventure 2D dev | Photoshop-like layered map composition | ~8000 |
| Asset pack creator (PixelLab vb. kullanan) | Sprite'a collider cizip pack icine entegre etmek | ~3000 |

### 2.3 USP (Tek Cumle)

**"SceneView'da el ile elle cek, anlik gor -- Tilemap'i kirmadan, Inspector'a girmeden, asset pack'inin parcasi olarak sakla."**

---

## 3. SCOPE

### 3.1 MVP'de Var (v1.0)

- UPM package: `com.laureth.painter-suite`
- 4 ana modul:
  - `Painter.Core` -- ortak EditorWindow + SceneView orchestration
  - `Painter.Layers` -- parallax/decoration layer painter
  - `Painter.Colliders` -- visual collider painter
  - `Painter.Templates` -- ScriptableObject template system
- Demo: 1 sample scene + 5 prop sprite (kendi PixelLab uretimimiz)
- Docs: README + tutorial GIF'leri + 1 YouTube video
- Unity surum destegi: 2022.3 LTS + 2023.3 LTS + Unity 6 LTS

### 3.2 MVP'de Yok (v1.1+ ertelenmis)

- Tile autopaint / autotile generation (Super Tilemap Editor alani)
- Hex grid (niche, ileride opsiyonel)
- 3D collider painter (kapsam disi)
- Multi-user collaboration / cloud
- Runtime editor (sadece Editor-time tool)
- Animasyon timeline (ParticleSystem entegrasyonu var ama timeline yok)

### 3.3 Asset Store Submission'da Gerekli (Liste)

- 5+ store screenshot (1920x1080)
- 1 trailer video (30-60sn, 1080p mp4)
- Long description + short description
- Tags + category
- EULA / End User License Agreement
- Demo build (optional ama trust artirir)
- Asset Store icon (200x258)

---

## 4. ARCHITECTURE

### 4.1 UPM Package Yapisi

```
Packages/com.laureth.painter-suite/
+-- package.json                       # UPM manifest
+-- LICENSE.md
+-- README.md
+-- CHANGELOG.md
+-- Documentation~/
|   +-- index.md
|   +-- getting-started.md
|   +-- collider-painter.md
|   +-- layer-painter.md
|   +-- templates.md
+-- Runtime/
|   +-- LaurethStudio.PainterSuite.Runtime.asmdef
|   +-- ColliderTemplate.cs           # ScriptableObject
|   +-- LayerProfile.cs               # ScriptableObject
|   +-- (no MonoBehaviours -- pure data + utility)
+-- Editor/
|   +-- LaurethStudio.PainterSuite.Editor.asmdef
|   +-- Core/
|   |   +-- PainterWindow.cs           # Main EditorWindow
|   |   +-- PainterSceneOverlay.cs     # SceneView.duringSceneGui handler
|   |   +-- ToolMode.cs                # Enum: Layer/Collider/Template
|   +-- Colliders/
|   |   +-- ColliderPainter.cs         # Mouse drag -> Collider2D logic
|   |   +-- ColliderHandleRenderer.cs  # SceneView gizmo drawer
|   |   +-- ColliderTemplateLibrary.cs # Template panel
|   +-- Layers/
|   |   +-- LayerPainter.cs            # Drag image -> layer logic
|   |   +-- LayerPanel.cs              # Photoshop-like layer list UI
|   |   +-- ParallaxDepthHandle.cs     # Depth multiplier control
|   +-- UI/
|   |   +-- (UXML/USS files)
|   |   +-- PainterWindow.uxml
|   |   +-- PainterWindow.uss
|   |   +-- LaurethTheme.uss
|   +-- Utils/
|       +-- SpriteUtils.cs             # bounds, pivot, ppu helpers
|       +-- UndoUtils.cs               # Undo.RecordObject wrappers
|       +-- PrefabSafe.cs              # PrefabUtility.RecordPrefabInstance...
+-- Samples~/
    +-- DemoPack/
        +-- Sprites/
        +-- Scenes/
        +-- Templates/
        +-- README.md
```

### 4.2 Decoupling Rules (RIMA spin-out garantisi)

**KESIN KURAL:** Package icinde **hicbir RIMA-spesifik tipe** referans olmayacak. RIMA bu paketi tuketir, package RIMA'yi tanimaz.

- Package interface'leri (`IPaintableTarget`, `IColliderTemplate`) tanimlar
- RIMA tarafinda RIMA'nin kendi tip'leri bu interface'leri implement eder
- Package'in test'leri RIMA olmadan da gecmeli (standalone Unity project'te)
- Demo asset pack PixelLab/jenerik asset -- RIMA art style'a bagli degil

**Namespace plani:**
- `LaurethStudio.PainterSuite.Runtime`
- `LaurethStudio.PainterSuite.Editor`
- `LaurethStudio.PainterSuite.Editor.Colliders`
- `LaurethStudio.PainterSuite.Editor.Layers`

**Spin-out yontemi:**
1. `git subtree split --prefix=Packages/com.laureth.painter-suite -b painter-suite-export`
2. Yeni repo'ya push
3. Asset Store submission, RIMA Manifest.json'da git URL referansi olarak kullan

### 4.3 Veri Modeli

**ColliderTemplate (ScriptableObject):**
```csharp
[CreateAssetMenu(menuName="LaurethStudio/Painter/Collider Template")]
public class ColliderTemplate : ScriptableObject {
    public string templateName;
    public Texture2D thumbnail;
    public List<ColliderShape> shapes;  // Box/Circle/Polygon, each with size+offset
}

[System.Serializable]
public class ColliderShape {
    public ShapeType type;  // Box, Circle, Polygon, Edge
    public Vector2 size;
    public Vector2 offset;
    public Vector2[] polygonPoints;  // only for Polygon/Edge
    public float radius;             // only for Circle
    public bool isTrigger;
    public PhysicsMaterial2D material;
}
```

**LayerProfile (ScriptableObject):**
```csharp
[CreateAssetMenu(menuName="LaurethStudio/Painter/Layer Profile")]
public class LayerProfile : ScriptableObject {
    public List<LayerEntry> layers;
}

[System.Serializable]
public class LayerEntry {
    public string layerName;
    public Sprite spriteAsset;
    public int sortingOrder;
    public Vector3 parallaxDepth;  // x,y,z multiplier
    public BlendMode blend;
    [Range(0,1)] public float alpha;
    public bool affectedByPlayerOcclusion;
}
```

---

## 5. FEATURE SPECIFICATIONS

### 5.1 Visual Collider Painter (KILLER FEATURE)

**Kullanim akisi (3-step Hendrix felsefesi):**
1. Painter Window'u ac -> Target sprite/GameObject'i sec (drop zone'a birak veya hierarchy'den)
2. Mode sec: Box / Circle / Polygon / Edge
3. SceneView'da mouse drag -> anlik Collider2D olusur, drag birakirken final

**Detayli ozellikler:**

| Ozellik | Davranis | Unity API |
|---|---|---|
| Drag-to-create Box | LMB down/drag/up -> BoxCollider2D component eklenir, size+offset hesaplanir | `Event.current`, `Undo.AddComponent<BoxCollider2D>()` |
| Drag-to-create Circle | LMB down/drag -> center fixed, radius = drag distance | Same, `CircleCollider2D` |
| Click-to-add Polygon | Her LMB click -> vertex; double-click -> close polygon | `PolygonCollider2D.SetPath(0, points)` |
| Resize via handles | Her collider icin Handles.PositionHandle (center) + 4-corner Handles.RectHandleCap (resize) | `Handles`, `EditorGUI.BeginChangeCheck()` |
| Move (drag center) | Center handle drag = collider offset degisir | Same |
| Delete | Shape select edip Del tusu | `Undo.DestroyObjectImmediate(component)` |
| Duplicate | Right-click -> "Duplicate" -> aynisi 0.5 unit yana | `Object.Instantiate` impossible for components; manual copy field-by-field |
| Save as Template | Selected collider(s) -> "Save as Template" -> `ColliderTemplate.asset` olusturur | `AssetDatabase.CreateAsset` |
| Load Template | Library panelden drag-drop sprite'a -> tum sekiller component olarak eklenir | Iterate `template.shapes`, `Undo.AddComponent` |
| Multi-collider per object | Unity zaten destekliyor (N tane Collider2D component) | Built-in |
| Visual layering | Painter panelinde collider list, expand/collapse, eye toggle (Scene'de gizle) | `SerializedObject` reflection |
| Snap to PPU grid | Toggle: drag sirasinda Mathf.Round(pos * PPU) / PPU | Hesap |
| Prefab variant safe | Override olarak kayit, base prefab'i bozmaz | `PrefabUtility.RecordPrefabInstancePropertyModifications` |
| Asset pack export | Selected collider'lari `ColliderTemplate.asset` olarak export, .unitypackage'a dahil | `AssetDatabase.ExportPackage` |

**Edge case'ler:**
- Sprite import settings (PPU, pivot) farkliysa -> collider relative-to-sprite'i koruyup uyarla
- Cok kucuk drag (< 0.05 unit) -> "click instead" anlamina gelir, BoxCollider eklenmez
- Sprite olmayan GameObject'e drag -> Renderer.bounds kullanilir
- World-space vs local-space drag -> default local, modifier (Shift+drag) world

**Eksik Unity built-in feature'lar (USP'mizi kuran):**
- Built-in Unity'de "drag rectangle to add collider" yok -- bizim oz USP
- Multi-collider list panel yok (sadece Component slot'lari)
- Template/preset library yok
- Cross-prefab template paylasimi yok

### 5.2 Iso/2D Parallax Layer Painter

**Kullanim akisi:**
1. Painter Window -> Layer mode
2. SceneView'a sprite drag-drop -> yeni LayerEntry olusur, otomatik GameObject + SpriteRenderer
3. Layer panelden sortingOrder, parallax depth, blend mode, alpha ayarla
4. Iso projeler icin sort axis override (default RIMA-style (0,1,-0.26) opsiyonel)

**Ozellikler:**

| Ozellik | Davranis |
|---|---|
| Layer panel | Photoshop-like list: eye, lock, name, sortingOrder, blend, alpha |
| Drag drop sprite | SceneView'a sprite -> instant GameObject olusur, layer'a eklenir |
| Reorder | List'te drag = sortingOrder degisir |
| Parallax depth | Per-layer float (0=static, 1=full camera follow), runtime LateUpdate component |
| Blend mode | Multiply / Screen / Additive / Normal -- custom shader veya URP 2D blend |
| Occlusion | Player Y vs layer Y compare -> alpha tween (opsiyonel) |
| Sort axis override | Per-layer custom sort axis (iso projeler icin) |
| Bake to Tilemap | Birden cok statik sprite layer'i tek Tilemap'e flatten et (performance) |
| Save as profile | Tum layer setup'i LayerProfile.asset olarak kaydet |

**Iso uyumluluk:**
- Default sort axis Unity 2D = (0,0,1). Iso projeler icin (0,1,-0.26) gibi.
- Per-layer override + global preset
- Sort axis preview gizmo (SceneView'da yon gosterir)

### 5.3 Template System

ScriptableObject tabanli, hem collider hem layer icin.

**Template browser:**
- Painter Window'da sag panel
- Thumbnail grid view
- Search + tag filter
- Drag thumbnail -> active sprite/scene'e apply

**Built-in template library (MVP icin):**
- Collider: "Tree Trunk", "Barrel", "Wall Slice (Iso)", "Door Frame", "Crate", "Round Stone", "Spike Trap"
- Layer: "Static BG", "Mid-distance Parallax", "Foreground Pop", "Lighting Overlay"

**Custom templates:**
- Kullanici kendi `.asset` dosyalarini olusturur, paketle paylasilir
- Folder convention: `Assets/PainterTemplates/`

---

## 6. UI/UX SPECIFICATION

### 6.1 Design Language

**LaurethStudio brand identity (yeni olusturulacak):**
- **Brand colors:**
  - Primary: `#2E3440` (dark slate, Editor uyumlu)
  - Accent: `#88C0D0` (frost blue, action highlights)
  - Warning: `#EBCB8B` (mute yellow)
  - Error: `#BF616A` (mute red)
  - Success: `#A3BE8C` (sage green)
- **Typography:**
  - Headings: Inter / Roboto (Unity Editor default uyumlu)
  - Mono: JetBrains Mono / Cascadia Code
- **Spacing:** 4px base unit, 8/12/16/24/32 step
- **Motion:** 150ms cubic-out ease default, 80ms snap for toggle states

### 6.2 PainterWindow Layout (UI Toolkit / UXML)

```
+-- LaurethStudio Painter (EditorWindow) ---------------+
| [Logo] LaurethStudio 2D Painter Suite       [docs?]   |
+-------------------------------------------------------+
| Mode: ( Layer )  ( Collider )  ( Template )           |
+-------------------------------------------------------+
| LEFT PANEL (240px)         | CENTER (SceneView reuse) |
|                            |                          |
| Target: [sprite drop zone] |  (SceneView -- overlay   |
|                            |   rendered here)         |
| --- Layers / Colliders --- |                          |
| > [eye] [lock] Box #1      |                          |
| > [eye] [lock] Circle #2   |                          |
| > [eye] [lock] Polygon #3  |                          |
|                            |                          |
| [+ Add Box] [+ Add Circle] |                          |
| [+ Polygon] [+ Edge]       |                          |
|                            |                          |
| Selected: Box #1           |                          |
| Size: (1.20, 0.80)         |                          |
| Offset: (0.00, 0.40)       |                          |
| IsTrigger: [ ]             |                          |
| Material: [None v]         |                          |
+----------------------------+                          |
|                                                       |
+-------------------------------------------------------+
| RIGHT PANEL (200px) -- Templates                      |
| [Search...]                                           |
| +--------+ +--------+ +--------+                      |
| | Tree   | | Barrel | | Crate  |                      |
| +--------+ +--------+ +--------+                      |
| +--------+ +--------+                                 |
| | Door   | | Wall   |                                 |
| +--------+ +--------+                                 |
| [+ Save Selected as Template]                         |
+-------------------------------------------------------+
| Status bar: PPU: 64 | Snap: ON | Mode: Box | Painting |
+-------------------------------------------------------+
```

### 6.3 SceneView Overlay

- Painter aktifken SceneView'in sol ust kosesinde mini-toolbar (mode toggle + snap toggle + ppu indicator)
- Active collider gizmo'lari yesil renkte (accent), selected olan vurgulu
- Hover state: collider outline glow
- Drag preview: cizilen rectangle kesik cizgili, dolu olunca solid

### 6.4 Onboarding UX

Yeni kullanici Window'u ilk acinca:
- 3-frame mini tutorial overlay
- Step 1: "Drop a sprite here"
- Step 2: "Pick a tool"
- Step 3: "Drag in SceneView -- that's it"
- "Don't show again" checkbox

### 6.5 Accessibility

- Tum tool tip'ler tooltip metniyle erisilebilir
- Keyboard shortcuts: B (Box), C (Circle), P (Polygon), Del (delete), Ctrl+D (duplicate)
- High contrast mode toggle
- Screen reader friendly labels (UI Toolkit native destek)

---

## 7. IMPLEMENTATION PHASES + AI PIPELINE DISPATCH

### 7.1 Toplam Takvim Ozeti

| Hafta | Faz | Cikis |
|---|---|---|
| **Hafta 1** | Foundation + Collider Painter MVP | UPM package iskelet + drag-to-create Box/Circle + undo + temel UI |
| **Hafta 2** | Collider Painter polish + Layer Painter MVP | Polygon/Edge + template system + Layer painter ana akis |
| **Hafta 3** | Demo pack + docs + marketing | PixelLab assets + demo scene + trailer + Asset Store submission |
| **Hafta 4-6** | Review wait + bug fixes + community setup | Discord, docs site, ilk patch hazirligi |

### 7.2 Hafta 1 -- Foundation + Collider Painter MVP

**Gun 1-2: UPM Package Iskelet + UI Toolkit Setup**

Dispatch hedefi: **Codex (high effort)**

```
TASK 1A: Create UPM package structure
- Create directory Packages/com.laureth.painter-suite/
- Write package.json (version 0.1.0, Unity 2022.3+)
- Create asmdef files: LaurethStudio.PainterSuite.Runtime, LaurethStudio.PainterSuite.Editor
- Stub EditorWindow class: PainterWindow.cs with [MenuItem("Window/LaurethStudio/Painter Suite")]
- Stub UXML/USS files (empty layouts)
- Create README.md, LICENSE.md (MIT for now, swap later)
- Verify Unity compiles, window opens
```

```
TASK 1B: Brand assets staging
- Generate via PixelLab/Gemini: LaurethStudio logo concept (3 variants)
- Create color palette swatches as Unity Editor preferences asset
- Set up LaurethTheme.uss with brand color CSS variables
```

**Gun 3-4: Collider Painter Drag-to-Create**

Dispatch hedefi: **Codex (xhigh effort)**

```
TASK 2A: ColliderPainter.cs core
- Subscribe to SceneView.duringSceneGui in OnEnable
- Handle Event.current: MouseDown/MouseDrag/MouseUp
- HandleUtility.GUIPointToWorldRay for world-space conversion
- On MouseDown: store start world pos
- On MouseDrag: render preview rectangle via Handles
- On MouseUp: Undo.AddComponent<BoxCollider2D>(target), set size+offset
- Repaint SceneView on every drag event
```

```
TASK 2B: Multi-collider list panel
- ListView in UXML, bound to target.GetComponents<Collider2D>()
- Each entry: eye toggle, lock toggle, name, type icon
- Select entry -> highlight in SceneView via Handles
- Delete button -> Undo.DestroyObjectImmediate
- Verify undo/redo works end-to-end
```

**Gun 5: Resize handles + circle support**

Dispatch hedefi: **Codex (high effort)**

```
TASK 3: Resize handles for existing colliders
- For each Collider2D in target, render Handles.PositionHandle (center)
- Render Handles.RectHandleCap at 4 corners for BoxCollider2D
- EditorGUI.BeginChangeCheck/EndChangeCheck wrapper
- Record undo on change
- Add CircleCollider2D mode: drag = radius from start point
- Verify in test scene with 5+ colliders
```

**Gun 6-7: Polygon + Edge + selection state polish**

Dispatch hedefi: **Codex (high effort)** + **Gemini (overflow if Codex limited)**

```
TASK 4: Polygon and Edge collider
- Click-to-add-vertex mode for PolygonCollider2D
- Double-click closes polygon (SetPath(0, points))
- Right-click cancels mid-polygon
- EdgeCollider2D same flow without closing
- Visualize vertices with Handles.SphereHandleCap
- Selected vertex draggable
```

**Hafta 1 verification gate:**
- [ ] Package opens via Window menu
- [ ] Drop sprite on drop zone -> target assigned
- [ ] Mouse drag in SceneView creates BoxCollider2D
- [ ] Click-to-add creates PolygonCollider2D
- [ ] Existing colliders show resize handles
- [ ] Undo/redo works for all operations
- [ ] No console errors in fresh Unity project
- [ ] Snap to PPU works when toggled
- [ ] Package builds standalone (without RIMA dependencies)

### 7.3 Hafta 2 -- Polish + Template + Layer Painter

**Gun 8-9: Template system**

Dispatch hedefi: **Codex (high effort)**

```
TASK 5A: ColliderTemplate ScriptableObject
- Define ColliderTemplate.cs in Runtime/
- Define ColliderShape struct with type/size/offset/radius/polygon/isTrigger/material
- CreateAssetMenu attribute
- Helper: ApplyTo(GameObject target) -> instantiates components

TASK 5B: Template library panel UI
- UXML: right panel grid of template thumbnails
- Each thumbnail: drag source (DragAndDrop.PrepareStartDrag)
- Drop on SceneView target -> ApplyTo(target)
- Search field filters by name
- "Save Selected as Template" button -> serialize current colliders, AssetDatabase.CreateAsset
```

**Gun 10-11: Duplicate + copy-paste + prefab safety**

Dispatch hedefi: **Codex (medium effort)**

```
TASK 6: Duplicate and clipboard
- Right-click context menu on collider list entry: Duplicate, Copy, Paste, Delete
- Duplicate: field-by-field copy to new component, offset by 0.5 unit
- Copy: serialize collider to JSON in EditorPrefs
- Paste: deserialize, AddComponent, restore fields
- Prefab safety: PrefabUtility.RecordPrefabInstancePropertyModifications wrap
- Verify on prefab variant: changes are overrides, base not affected
```

**Gun 12-13: Layer Painter MVP**

Dispatch hedefi: **Codex (xhigh effort)**

```
TASK 7A: LayerPainter.cs
- LayerProfile ScriptableObject definition (see Section 4.3)
- SceneView drag-drop sprite -> create GameObject with SpriteRenderer
- Auto-assign sortingOrder based on layer index
- Layer list panel (Photoshop-like): eye, lock, name, order, blend, alpha

TASK 7B: Parallax depth runtime component
- ParallaxLayer.cs MonoBehaviour in Runtime/
- LateUpdate: compute camera delta, multiply by depth, apply offset
- Iso sort axis override per-layer

TASK 7C: Blend modes
- 4 custom URP 2D shaders: Normal, Multiply, Screen, Additive
- Material auto-assigned based on blend dropdown
- Verify URP 2D Renderer compatible
```

**Gun 14: Layer painter polish + iso preset**

Dispatch hedefi: **Codex (medium)** + **Claude orchestration (testing)**

```
TASK 8: Iso preset and sort axis preview
- Project setting: GlobalSortAxis (default (0,1,-0.26) for iso, (0,0,1) for top-down)
- Per-layer override toggle
- SceneView gizmo showing sort axis direction
- "Iso Preset" button: applies sort axis + scales camera ortho size
```

**Hafta 2 verification gate:**
- [ ] Template panel shows builtin templates (7 collider + 4 layer)
- [ ] Drag template thumbnail to sprite applies all shapes
- [ ] Save Selected as Template creates .asset file
- [ ] Duplicate/Copy/Paste works on colliders
- [ ] Prefab variant override works (base prefab unchanged)
- [ ] Layer painter: drag sprite creates GameObject with proper sort order
- [ ] Parallax depth visible when SceneView camera moves
- [ ] Blend modes render correctly in URP 2D project

### 7.4 Hafta 3 -- Demo Pack + Docs + Marketing + Submit

**Gun 15-16: Demo asset pack**

Dispatch hedefi: **PixelLab MCP** + **UnityMCP**

```
TASK 9A: Generate 5 demo prop sprites
- PixelLab: tree, barrel, crate, door, brazier
- 4-direction sprites (faces only needed for demo)
- 64x64 base, PPU 64
- Style: pixel art, RIMA-adjacent but neutral

TASK 9B: Create 7 collider templates
- ColliderTemplate.asset for: Tree (trunk+canopy), Barrel (round), Crate (box), Door (frame), Brazier (round+trigger), Wall Slice, Spike Trap

TASK 9C: Demo scene
- Sample scene with all 5 props placed
- Player capsule + camera
- Parallax background using 3 LayerProfile assets
- "Walk into colliders" demo

TASK 9D: Samples~/DemoPack import path
- UPM Samples~ folder convention
- Importable via Package Manager -> Samples tab
```

**Gun 17-18: Documentation**

Dispatch hedefi: **Claude (direct write)**

```
TASK 10A: Docs site
- README.md: install, quickstart, screenshots
- getting-started.md: 5-min tutorial with GIFs
- collider-painter.md: full feature ref
- layer-painter.md: full feature ref
- templates.md: how to make/share templates
- api-reference.md: public scripting API

TASK 10B: Tutorial GIFs
- 5-10 short GIFs (5-10sec each)
- "Drag to create collider", "Apply template", "Layer reorder", "Parallax preview"
- Generate via Unity Recorder + ffmpeg
```

**Gun 19-20: Marketing assets + Asset Store submission**

Dispatch hedefi: **Manuel (Claude orchestration)**

```
TASK 11A: Trailer video
- 30-45sec, 1080p, mp4
- Cuts: title, problem, solution shots, before/after, CTA
- Music: royalty-free upbeat (Pixabay/freesound)
- Tool: DaVinci Resolve or CapCut

TASK 11B: Store screenshots (5 frames)
- 1920x1080 each
- 1: PainterWindow full screen
- 2: SceneView drag-to-create in action
- 3: Multi-collider on one prop
- 4: Template library panel
- 5: Layer painter with parallax

TASK 11C: Store icon
- 200x258 px
- LaurethStudio logo + "2D Painter Suite" wordmark
- Brand colors

TASK 11D: Submission
- Asset Store publisher account (LaurethStudio)
- Fill submission form
- Pricing: $45 launch
- Wait for review (~2-3 weeks)
```

### 7.5 Hafta 4-6 -- Review Wait + Community

Asset Store review typically 2-3 hafta. Bu surede:

- Discord server setup (basic, RIMA + LaurethStudio brand)
- Patreon page (optional, Hendrix gibi)
- Sample project repo (GitHub, gostermelik kullanim ornekleri)
- Bug fix patches (v0.1.1, v0.1.2)
- Tohum 2 (Living Decoration Layer) icin scope draft

### 7.6 Dispatch Effort Tablosu (Hafta 1-3 Toplam)

| Agent | Saat | Dolarlik karsilik (approx) |
|---|---|---|
| Codex (cx exec, gpt-5.5) | ~40-50 dispatch, hafif/orta/agir karisik | Pipeline maliyet |
| Gemini (Antigravity, overflow) | ~10 dispatch (Codex limit asinca) | Pipeline maliyet |
| PixelLab MCP | ~15 sprite + 5 tile | $ pipeline kredisi |
| UnityMCP | ~30-50 manage_* cagrisi (scene wire, prefab) | Local |
| Claude (Opus 4.7) | Tum orkestrasyon + design + docs + qc | Plan |

---

## 8. RISKS AND MITIGATIONS

| Risk | Olasilik | Etki | Mitigasyon |
|---|---|---|---|
| Asset Store review reddi (technical) | Dusuk | Yuksek | MVP'yi Unity Asset Store technical guidelines'a gore yaz, asset path conventions takip et, demo scene'i bos crash etmeden acilmali |
| Asset Store reddi (kalite) | Orta | Yuksek | 5+ screenshot kalitesi yuksek, video render iyi, docs eksiksiz, demo pack pratik |
| Realtime preview Editor + PlayMode parity bug | Yuksek | Orta | Her ozellik icin Edit-mode + Play-mode test ciftli, regression suite |
| URP/2D renderer uyumsuzlugu | Orta | Yuksek | URP 2D + Built-in pipeline 2D ikisinde de test, shader fallback |
| Unity 2022/2023/6 LTS API farkliliklari | Orta | Orta | CI'da 3 surum test, #if UNITY_2023_1_OR_NEWER conditional |
| Tilemap conflict (asset store rakip) | Orta | Dusuk | USP = augmentation degil replacement, demo'da net goster |
| Maintenance burden (post-launch) | Yuksek | Orta | Discord + community moderation icin 3 saat/hafta dedicated |
| RIMA gelisimi yavaslama | Orta | Yuksek | Hafta 1-3 boyunca RIMA'da ozellik freeze; ondan sonra parallel |
| Single-dev solo risk | Yuksek | Yuksek | AI pipeline = solo dev'in ekibi; Discord destek icin community ilk haftadan kurulsun |
| Refund flood (kalitesiz alirsa) | Dusuk | Orta | Asset Store policy + Discord destek + hizli patch cycle |

---

## 9. DEFINITION OF DONE (Acceptance Criteria)

### 9.1 v0.1.0 (Hafta 1 sonu, internal alpha)

- [ ] UPM package Unity 2022.3'te derler, 0 error 0 warning
- [ ] PainterWindow Window menu'den acilir
- [ ] Box ve Circle collider drag-to-create calisir
- [ ] Polygon click-to-add calisir
- [ ] Resize handles tum collider'larda calisir
- [ ] Undo/redo tutarli
- [ ] Snap to PPU toggle calisir
- [ ] Console temiz (warning yok)
- [ ] Standalone Unity project'te test edilir (RIMA bagimsiz)

### 9.2 v0.5.0 (Hafta 2 sonu, internal beta)

- [ ] Template system tam (7 builtin collider + 4 layer template)
- [ ] Save/Load template calisir
- [ ] Duplicate/Copy/Paste calisir
- [ ] Prefab variant override safe
- [ ] Layer painter drag-drop calisir
- [ ] Parallax depth runtime gorunur
- [ ] Blend modes URP 2D'de render eder
- [ ] Iso sort axis preset calisir
- [ ] Unity 2023.3 ve Unity 6'da derler
- [ ] RIMA'da dogfood test (Map Designer scene'inde 5+ prop placement)

### 9.3 v1.0.0 (Hafta 3 sonu, submission ready)

- [ ] Demo pack 5 sprite + 7 collider template + 4 layer profile
- [ ] Sample scene oynanabilir (walk + collide + parallax)
- [ ] README + docs/ tam
- [ ] 5-10 tutorial GIF
- [ ] YouTube tutorial video (3-5 dakika)
- [ ] Trailer video 30-45sec
- [ ] 5 store screenshot
- [ ] Store icon
- [ ] EULA / LICENSE
- [ ] Submission formu doldurulmus
- [ ] Discord server kurulmus, davet linki docs'ta

### 9.4 Spin-out hazirligi (her zaman gecerli)

- [ ] Package icinde 0 RIMA referansi (`grep -r "RIMA\." Packages/com.laureth.painter-suite/` bos doner)
- [ ] Package standalone Unity project'te derler
- [ ] Demo asset pack RIMA art-style'a bagimli degil
- [ ] `git subtree split --prefix=Packages/com.laureth.painter-suite` test edildi

---

## 10. POST-LAUNCH ROADMAP (v1.1+)

| Surum | Hedef | Sure | Yeni ozellik |
|---|---|---|---|
| v1.0.0 | Launch | Hafta 3 sonu | -- |
| v1.0.1 | Bug fixes from review feedback | 1 hafta | -- |
| v1.1.0 | Tohum 2 modulu: Living Decoration Layer | 2 hafta | Animated tile layer, Tilemap augmentation |
| v1.2.0 | Tohum 3 / Iso Sort Axis Inspector spin-off | 2 hafta | Standalone debugger tool (ayri urun veya paket icinde) |
| v1.3.0 | Hex grid support | 2 hafta | Hex collider templates, hex layer painter |
| v2.0.0 | Major UX refresh + GPU paint | 4 hafta | Brush mode for batch placement |

**Ek urun dallari (Tohum'lar):**
- **A. RIMA Spawn Painter** -- SceneView brush ile enemy/loot/event spawn -- ayri urun ($15-20)
- **B. Pixel Light Studio** -- 2D Light2D Photoshop layer mantigi -- ayri urun ($25-35)
- **C. Iso Sort Axis Inspector** -- z-bleed debugger -- ayri urun ($10-15)
- **D. Asset Pack Composer** -- PixelLab/external PNG drag-drop pipeline -- ayri urun ($30-45)
- **E. Roguelite Room Stitcher** -- prefab room magnet stitch -- ayri urun ($35-50)

**Bundle strategy (6 ay sonra):**
- "LaurethStudio Complete 2D Toolkit" tum tool'lar tek pakette $99-129
- Cross-promotion her tool'da bundle linki

---

## EK A. CODEX DISPATCH SABLONU

```
GOREV: <Task name from Section 7>

KONTEKST:
- Unity 2022.3 LTS (Editor)
- UPM package: Packages/com.laureth.painter-suite/
- Namespace: LaurethStudio.PainterSuite.<Module>
- URP 2D Renderer
- No RIMA-specific dependencies (standalone package)

YAPILACAKLAR:
<Concrete steps from task definition>

KURALLAR:
- ASCII only, English code comments
- Undo-aware (Undo.RecordObject / Undo.AddComponent)
- Prefab-safe (PrefabUtility.RecordPrefabInstancePropertyModifications)
- 0 warning, 0 error
- Asmdef references: only Unity built-in + this package
- Unit test where applicable (EditMode tests in Tests/Editor/)

VERIFICATION:
<Specific verification commands>

DONE FILE:
CODEX_DONE_<profile>.md icine STATUS + Files modified listele
```

---

## EK B. PIXELLAB DISPATCH SABLONU (Demo Assets)

```
PIXELLAB CONFIG:
- Model: create_character or create_map_object (sprite type'a gore)
- Size: 64x64 base, PPU 64
- Style: pixel art, neutral RPG fantasy
- Palette: 16-color, Nord-inspired (#2E3440, #88C0D0, #A3BE8C, #BF616A, ...)

ASSETS (Hafta 3, TASK 9A):
1. Tree (oak, 64x64, 4-dir not needed, top + bottom anchor)
2. Barrel (wooden, 48x48)
3. Crate (wooden, 48x48)
4. Door (single-tile, 32x64)
5. Brazier (animated 4-frame fire, 48x64)

OUTPUT:
- Save to /tmp/painter_demo_assets/
- Magenta background for chroma key (pixel-cleanup skill compatible)
```

---

## EK C. UNITYMCP DISPATCH SABLONU (Demo Scene)

```
SCENE: Samples~/DemoPack/Scenes/DemoScene.unity

STEPS:
1. manage_scene action=create path=Samples~/DemoPack/Scenes/DemoScene
2. manage_gameobject action=create name=Player, add CapsuleCollider2D + Rigidbody2D
3. manage_gameobject action=create name=Camera, OrthographicCamera setup
4. For each demo prop (tree/barrel/crate/door/brazier):
   - manage_prefab action=create from sprite
   - Apply ColliderTemplate via Painter tool API
   - Place in scene
5. Create 3 parallax background layers via LayerProfile
6. Verify scene plays without console errors
7. Capture screenshot via manage_editor action=screenshot
```

---

## EK D. ASSET STORE SUBMISSION CHECKLIST

- [ ] Publisher account created (LaurethStudio)
- [ ] Tax info filled
- [ ] Bank account verified
- [ ] Package version: 1.0.0
- [ ] Unity minimum version: 2022.3
- [ ] Description short (< 200 chars)
- [ ] Description long (full features list, USP, technical req)
- [ ] Tags: 2D, tilemap, collider, parallax, editor extension, level design
- [ ] Category: Tools / Level Design
- [ ] 5+ screenshots uploaded
- [ ] Trailer video URL (YouTube unlisted)
- [ ] Icon 200x258
- [ ] Banner 1950x1300
- [ ] EULA accepted
- [ ] Demo project link (GitHub)
- [ ] Documentation URL
- [ ] Support email (laurethgame@gmail.com or new domain)
- [ ] Pricing $45
- [ ] Submission button clicked

---

## EK E. AI PIPELINE NOTLARI

**Codex (cx exec):**
- Profile: laurethayday -> laurethgame -> yasinderyabilgin (priority order)
- Effort: high default, xhigh for complex (shaders, prefab edge cases)
- Timeout: 1200s default, 1500s for xhigh
- All dispatches log to CODEX_DONE_<profile>.md
- run_in_background: true zorunlu

**Gemini (Antigravity fallback):**
- Use when Codex quota saturates
- Full-file edit capability
- UnityMCP entegre

**PixelLab MCP:**
- create_character / create_map_object / create_topdown_tileset
- Cost-aware: 1 cost per asset
- Output to STAGING/ first, pixel-cleanup skill ile temizle

**UnityMCP:**
- manage_scene, manage_gameobject, manage_prefab, execute_code
- Background autonomous OK
- read_console regularly to catch errors

**Claude (Opus 4.7):**
- Orchestration + design + QC + docs writing
- Direct Bash for thin dispatches
- Sub-agent spawn only for multi-step reasoning

---

## EK F. DEGERLENDIRME SORULARI (Diger hesap agent'i icin)

Bu plani okuyan baska bir agent'in cevap vermesi gereken sorular:

1. **Scope dogru mu?** v1.0 MVP'de gereksiz ozellik var mi, eksik ozellik var mi?
2. **Timeline gercekci mi?** 3 hafta AI pipeline ile yetisir mi, yoksa 4-5 hafta gerekir mi?
3. **Visual Collider Painter teknik olarak feasible mi?** Unity API'leri yeterli, eksik nokta var mi?
4. **USP gercek mi?** Asset Store'da bu kombinasyon (parallax + collider painter + template) gercekten rakipsiz mi? Daha derin tarama gerek mi?
5. **Fiyat $45 dogru mu?** Asset Store comparables verisine gore yuksek/dusuk/uygun?
6. **UPM modularity disiplini yeterli mi?** Spin-out icin baska nelere dikkat?
7. **Risk listesi eksiksiz mi?** Eksik gordugun risk?
8. **Hangi adimda durdurulabilir?** Hafta 1 sonunda iptal edilirse maliyet ne?
9. **Marketing zayif mi?** Trailer + 5 screenshot Asset Store algoritmasi icin yeterli mi?
10. **Post-launch roadmap mantikli mi?** Tohum 2/3/4 sirasi dogru mu?

---

**End of document. Total ~10,500 words.**
