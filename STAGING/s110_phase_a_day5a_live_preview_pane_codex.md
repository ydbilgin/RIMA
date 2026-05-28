ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**Output dosyası:** `CODEX_DONE_room_painter_day5a.md` (max 500 kelime)

---

# Amaç

Phase A Day 5a — UI stability fixes + Live Preview pane + 3D-mock depth rendering. **Collider authoring YOK** (Day 5b).

## Source spec — ZORUNLU OKU

`F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ROOM_PAINTER_DAY5_LIVE_PREVIEW_SPEC.md` (Sonnet yazdı, 3500 kelime, 6 bölüm).

**Bu task = Day 5a kısmı.** Sonnet handoff outline (Bölüm 6'da) ready-to-paste Codex prompts içeriyor — referans al.

## Mevcut state (Day 4 LIVE + patch LIVE)

- Window: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (~400 LOC, 2-pane palette + inspector LIVE)
- Inspector: 6 section files LIVE (`Inspector/Sections/`)
- AssetPipeline: 4 dosya LIVE (Postprocessor + Events + PhysicsRules + Applier)
- Compile 0 error
- Parallax SO fields HAZIR (Day 4 patch M2 fix)

## Görev — Day 5a, 2 ana iş

### İş 1: UI Stability Fixes (Sonnet spec Bölüm 1)

Sonnet 5 jitter source diagnose etti. Hepsini fix:

1. **Foldout EditorPrefs persistence** — `RoomPainterInspectorPanel.cs` her foldout için `EditorPrefs.GetBool/SetBool` ile state persist
2. **Dropdown caching** — Layer dropdown + Parallax tier dropdown'lar her OnGUI'de re-enum populate ediyor. Static cache + lazy init.
3. **Banner static GUIContent** — SO vs instance banner her repaint'te `new GUIContent` allocate ediyor. Static field.
4. **Locked section order** — sections sırası: Identity → Placement → Physics → Parallax → Visual → Metadata. ASLA swap etme tab değişimine göre. Conditional render OK ama order fixed.
5. **Scroll anchor dict per-target** — selected asset değişince scroll position 0'a dönüyor. `Dictionary<RoomPainterAsset, Vector2> _scrollPositions` ile per-asset hatırla.

### İş 2: Live Preview Pane (Sonnet spec Bölüm 2)

#### Yeni layout 3-pane

`RimaRoomPainterWindow.cs` 2-pane → 3-pane:
```
[ASSET PALETTE 300px | LIVE PREVIEW 400px | INSPECTOR 350px]
                       ↑ YENİ orta pane
        min window 1200×700, splitter handles between panes
```

Splitter rules: drag bar between panes, min 200px each, max ratio constraints.

#### 5 yeni dosya

1. **`Assets/Editor/RoomPainter/Preview/RoomPainterPreviewPane.cs`** (~250 LOC) — Orchestrator
   - `Draw(Rect area, AssetEntry selectedAsset, RoomLayer targetLayer, float currentRotation)`
   - State: zoom level (1x-8x), pan offset (Vector2), rotation override
   - Sub-renderer'ları dispatch eder

2. **`Assets/Editor/RoomPainter/Preview/PreviewBackgroundDrawer.cs`** (~100 LOC) — Layer 1
   - Dark checkerboard background (Unity-style transparency indicator)
   - Pixel-perfect at zoom = integer multiples
   - Cached texture for performance

3. **`Assets/Editor/RoomPainter/Preview/PreviewSpriteRenderer.cs`** (~150 LOC) — Layer 2
   - Sprite preview at center, scaled by zoom, rotated by current rotation
   - Prefab varsa: PrefabUtility.GetPrefabAssetType + thumb fallback
   - Pivot anchor crosshair (red horizontal, green vertical)

4. **`Assets/Editor/RoomPainter/Preview/PreviewOverlayRenderer.cs`** (~200 LOC) — Layer 3 (3D-mock)
   - **Drop shadow ellipse** under sprite (yarı saydam gri, sprite size %80)
   - **Cliff ramp shading** (if layer=Cliff): sprite alt %40 darker gradient overlay
   - **Parallax atmospheric tint** (if layer=Parallax): sprite üzerinde hafif mavi tint + scale küçültme (atmospheric perspective)
   - **Y-sort axis indicator** (vertical line through pivot)
   - **Bounding box** (collider preview, cyan dashed — Day 5b'de interactive olacak)

5. **`Assets/Editor/RoomPainter/Preview/PreviewInputHandler.cs`** (~100 LOC)
   - Mouse wheel = zoom 1x-8x (integer steps)
   - Middle-mouse drag = pan
   - `R` key = rotation += 90°
   - `0` key = reset zoom + pan + rotation

### 5 modified dosya

1. **`RimaRoomPainterWindow.cs`** (+80 LOC) — 3-pane layout + splitter handles + `_previewPane` field + Draw dispatch
2. **`RoomPainterInspectorPanel.cs`** (+60 LOC) — foldout persistence + scroll anchor dict
3. **`Inspector/Sections/PlacementSection.cs`** (+15 LOC) — dropdown cache static
4. **`Inspector/Sections/ParallaxSection.cs`** (+15 LOC) — tier dropdown cache static + **slider eklenmesi de bu turda** (user direktifi 2026-05-26 — tier preset + slider çubuk, snap on preset click, free on slider drag)
5. **`Inspector/Sections/PhysicsSection.cs`** — minor: dropdown cache

### User direktifi entegrasyonu (2026-05-26)

> "Tier presetleri olsun ama istediğimi bi barla da değiştirebileyim. Çubuk kaydırarak."

**ParallaxSection güncellemesi:**
- Tier dropdown (FG 1.20 / Playable 1.00 / Near 0.65 / Mid 0.40 / Far 0.22 / Skyline 0.10 / Horizon 0.03)
- ALTINA: `EditorGUILayout.Slider("Custom override", parallaxFactor, 0.01f, 1.50f)`
- Slider değeri herhangi bir tier preset değerine eşit ise dropdown o tier'da kalsın
- Slider farklı değere çekilirse dropdown "Custom" olur (yeni enum value veya null state)
- Dropdown tıklayınca slider o tier değerine **snap**

### Reuse pointer (Sonnet handoff'tan)

`Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs`:
- `:36-39` color constants (cyan/yellow) — preview overlay'de reuse
- `:110-121` drag handle pattern — Day 5b'de kullanılacak, Day 5a için pas
- `:163-169` ghost preview rendering — preview pane'in sprite rendering pattern'i için referans

## Yapma

- Collider authoring (Box/Circle/Polygon tools + handles) **YOK** (Day 5b)
- Erase/Pick/Box-select **YOK** (Day 6)
- Save/Load RoomData **YOK** (Day 7)
- Mevcut SceneView placement / SceneView ghost preview — dokunma
- Mevcut Day 4 dosyaları (Postprocessor / PhysicsRules / Applier / Events) — dokunma

## Verification

1. `grep -n "PreviewPane\|RoomPainterPreviewPane" Assets/Editor/RoomPainter/Preview/` → en az 3 hit
2. `grep -n "EditorPrefs.GetBool\|EditorPrefs.SetBool" Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` → en az 4 hit (foldout state)
3. `grep -n "_scrollPositions\|ScrollPosition" Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` → en az 2 hit
4. `grep -n "EditorGUILayout.Slider" Assets/Editor/RoomPainter/Inspector/Sections/ParallaxSection.cs` → en az 1 hit (USER REQUEST)
5. `grep -n "Mouse.button == 2\|GUI.ScrollWheel\|EventType.ScrollWheel" Assets/Editor/RoomPainter/Preview/PreviewInputHandler.cs` → en az 2 hit
6. Unity compile error 0

## Çıktı

`CODEX_DONE_room_painter_day5a.md`:
- 10 dosya listesi (5 yeni + 5 modified) + LOC + 1 satır işlev
- Verification grep çıktıları
- Compile durumu
- Parallax slider entegrasyonu confirmed
- Splitter implementation pattern (eğer Unity native splitter yoksa custom EditorGUILayout.Slider with handle yapısı kullanıldı mı)
- Eğer Sonnet spec'inde belirsizlik veya çakışma BLOCKED + detay

**Day 5a ship hedefi: 3-pane window LIVE + Preview pane sprite + 3D-mock depth visual + UI artık jitter etmiyor + Parallax tier slider LIVE.**
