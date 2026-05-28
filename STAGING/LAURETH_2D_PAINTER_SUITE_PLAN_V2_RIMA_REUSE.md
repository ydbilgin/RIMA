# LaurethStudio 2D Painter Suite -- Plan V2 (RIMA Reuse Edition)

**Version:** 2.0
**Date:** 2026-05-26 (S109 close window)
**Status:** DRAFT -- companion to V1 (`LAURETH_2D_PAINTER_SUITE_PLAN.md`)
**Relationship:** V1 = sifirdan tasarim, V2 = RIMA'da zaten var olan tooling'i extract + adapt + extend yontemiyle. V1 BOZULMAZ, ikisi paralel okunur.

---

## 0. NIYE V2?

V1 plani sifirdan tasarim ile 3 hafta MVP veriyor. AMA RIMA'da zaten:
- Drag-paint pipeline (`VisualEditorScenePainter`) %80 hazir
- Tabbed editor window (`UnifiedMapDesigner`) hazir
- ScriptableObject brush/pack/skin sistemi (`MapDesignerBrushPresetSO`, `BrushPackSO`, `BiomeSkinSO`) hazir
- Parallax runtime (`ParallaxLayer.cs`) hazir + pixel snap PPU 64
- BrushExecutorRouter pattern hazir
- BrushPalettePanel UI hazir (search, category filter, thumb size)
- Auto-tilemap resolution (`AutoLayeringService`) hazir
- Ghost preview + R rotation + undo group pattern hazir
- Cliff auto-placer (`CliffAutoPlacer`) -- neighbor-based detection pattern

**Sonuc:** V2 planinda **3 hafta -> 5-7 gun MVP** dusuyor. Cunku bazi modullerin "yeni yazma" maliyeti yok, sadece **extract + decouple + rename + publish**.

V2 ek faydasi: RIMA'nin S110 yapilacaklar listesinde **"Parallax Layer Authoring Tab" P0** olarak var (CURRENT_STATUS Codex Q5 Oneri 1). Yani RIMA dogal yatirim **zaten yapilacak** -- ayni isi PainterSuite'e cikartmak iki kez calismayi sifirlar.

---

## 1. RIMA REUSE MAP

| RIMA Modulu | Path | PainterSuite Karsiligi | Reuse Stratejisi |
|---|---|---|---|
| `VisualEditorScenePainter.cs` | `Assets/Editor/MapDesigner/VisualEditor/` | `ColliderPainter.cs` + `LayerPainter.cs` ortak base | **EXTRACT**: SceneView handler + ghost preview + undo group logic generic'lestir |
| `RimaVisualMapEditorWindow.cs` | ayni klasor | `PainterSuiteWindow.cs` ana sekme yapisi | **TEMPLATE**: dockable window + auto-load default assets pattern |
| `AutoLayeringService.cs` | ayni klasor | `TargetResolverService.cs` | **GENERIC'LESTIR**: BrushCategory yerine generic `IPaintTarget` interface |
| `LiveAutotiler.cs` | ayni klasor | `LivePreviewService.cs` | **ADAPT**: MouseUp -> trigger Regenerate pattern |
| `BrushPalettePanel.cs` | `Assets/Editor/MapDesigner/Brush/Panels/` | `TemplateLibraryPanel.cs` | **RENAME + extract**: search/category/thumb size UI direkt kullan |
| `BrushSettingsPanel.cs` | ayni | `PainterSettingsPanel.cs` | **ADAPT**: brush settings -> painter settings |
| `LayerVisibilityPanel.cs` | ayni | `LayerPanel.cs` | **DIREKT KULLAN**: layer eye/lock pattern aynen |
| `BrushSceneTooling.cs` | ayni | `SceneToolingService.cs` | **ADAPT**: SceneGUI tooling pattern |
| `MapDesignerBrushPresetSO` | `Assets/Data/Brush/` (varsayim) | `ColliderTemplateSO` + `LayerProfileSO` | **PATTERN COPY**: SO yapisini Collider + Layer'a uyarla |
| `BrushExecutorRouter` | `Assets/Scripts/MapDesigner/Brush/Executors/` | `PainterExecutorRouter` | **GENERIC'LESTIR**: dispatch pattern aynen |
| `ParallaxLayer.cs` | `Assets/Scripts/Background/` | UPM `Runtime/ParallaxLayer.cs` | **DIREKT KOPYALA**: namespace degisikligi yeterli (RIMA.Background -> LaurethStudio.PainterSuite.Runtime) |
| `CliffAutoPlacer` neighbor pattern | `Assets/Scripts/Environment/` | Bonus -- "Auto Collider Slicer" feature | **REFERANS**: future feature icin pattern (v1.1+) |
| `WalkabilityMap.cs` | `Assets/Scripts/Environment/` | Bonus -- collider walkability QC | **REFERANS**: collider integrity verify icin (v1.1+) |
| `BrushHotkeyHandler.cs` | `Assets/Editor/MapDesigner/Brush/Hotkeys/` | `PainterHotkeyHandler.cs` | **DIREKT KULLAN**: B/E/R hotkey pattern |

**Direkt kopyalanabilen LOC:** ~1500-2000 satir (refactor + decouple sonrasi)
**Yeniden yazilacak LOC:** ~800-1200 satir (Collider-spesifik mouse drag, blend mode shader, template export)

---

## 2. DECOUPLING DISCIPLINE

RIMA'dan extract edilirken **hicbir RIMA-spesifik tipi tasimaya yok**:

| RIMA tipi | Bagimlilik | Cozum |
|---|---|---|
| `RIMA.MapDesigner.Brush.Data.BrushCategory` | enum | LaurethStudio.PainterSuite.Editor.PainterCategory enum (Box/Circle/Polygon/Edge/Layer) |
| `RIMA.MapDesigner.Brush.Data.MapDesignerBrushPresetSO` | SO | LaurethStudio.PainterSuite.Runtime.ColliderTemplate (+ LayerProfile) SO |
| `RIMA.Data.AssetPoolSO` | SO | Direkt copy + namespace rename |
| `RIMA.Background.ParallaxLayer` | MB | LaurethStudio.PainterSuite.Runtime.ParallaxLayer (namespace rename) |
| `GameObject.Find("FloorTilemap")` | scene lookup | Generic `ITargetResolver` interface, package implementation: target field |
| `RIMA.MapDesigner.Brush.Stroke.*` | stroke types | Sifirdan generic `IPaintStroke` (Box/Polygon/Drag) |

**Compile gate:** Package klasoru ayri Unity project'te (RIMA Assets/ icermeden) derlenmeli. Test:
```
mkdir TestProject_Standalone/
cp -r Packages/com.laureth.painter-suite TestProject_Standalone/Packages/
unity -batchmode -projectPath TestProject_Standalone/ -quit -logFile out.log
# 0 compile error verify
```

---

## 3. REVISED TIMELINE (RIMA Reuse Avantajli)

**V1 takvimi:** 3 hafta MVP, 5-6 hafta release
**V2 takvimi:** **5-7 gun MVP**, 2-3 hafta release (Asset Store review dahil)

| Gun | Faz | Cikis |
|---|---|---|
| **Gun 1** | UPM iskelet + namespace plan + RIMA dosya audit | Package skeleton, asmdef, README stub, ekstrakte edilecek dosyalarin listesi |
| **Gun 2** | Core extract: SceneView painter + ghost preview + undo group | `ColliderPainter.cs` calistirilabilir POC (drag-to-create Box) |
| **Gun 3** | Multi-collider list panel + resize handles + Circle + Polygon | `LayerPanel.cs` (collider list) + `CircleColliderPainter` + `PolygonColliderPainter` |
| **Gun 4** | Template system extract: `ColliderTemplateSO` + library panel | Save/Load template, drag thumbnail to apply |
| **Gun 5** | Layer painter extract: `ParallaxLayer` + drag-drop sprite + blend modes | `LayerPainter.cs` calisir, parallax depth visible |
| **Gun 6** | Demo pack: 5 sprite PixelLab + 7 templates + sample scene + docs draft | Samples~/DemoPack/ tam |
| **Gun 7** | Trailer + screenshots + Asset Store submission + RIMA dogfood | Submit |

**Asset Store review wait:** 2-3 hafta (kontrolsuz)
**Toplam release:** 5-6 hafta (V1 ile ayni; ama gercek iste 1 hafta yerine 2-3 hafta tasarrufu).

---

## 4. RIMA + PAINTERSUITE SINERJI: S110 ENTEGRASYONU

**S110 yapilacaklarda var (CURRENT_STATUS):**
- **Parallax Layer Authoring Tab** -- Codex Q5 Oneri 1, P0, 2-3 gun
- **Foreground Occlusion/Fade Zones** -- Codex Q5 Oneri 2, P0, 1-2 gun
- **Animated/Stateful Atmospheric Layers** -- Codex Q5 Oneri 3, P1, 2-4 gun

**Plan:** S110 calismasi **dogrudan UPM package icine** yazilsin, RIMA.MapDesigner namespace yerine LaurethStudio.PainterSuite. RIMA package'i tuketir.

**Yan etki:** S110 boyunca yapilan her commit -- Asset Store urununde de calisir. **Cift kullanim disiplini 1. gun lock'lanir.**

**Risk:** RIMA-spesifik isimlendirme (CliffTilemap, FloorTilemap) UPM'e sizar. Cozum: `ITargetResolver` interface ile RIMA wrapper paketi (sadece RIMA Assets/'te) tilemap isimlerini resolve eder, UPM generic kalir.

---

## 5. UPDATED PACKAGE STRUCTURE (V1'den fark)

V1'deki yapi gecerli; sadece su eklemeler:

```
Packages/com.laureth.painter-suite/
+-- Runtime/
|   +-- ParallaxLayer.cs           # EXTRACT from RIMA Assets/Scripts/Background/
|   +-- ColliderTemplate.cs
|   +-- LayerProfile.cs
|   +-- Interfaces/
|       +-- ITargetResolver.cs
|       +-- IPaintStroke.cs
+-- Editor/
|   +-- Core/
|   |   +-- PainterSuiteWindow.cs            # ADAPT from RimaVisualMapEditorWindow
|   |   +-- PainterSceneOverlay.cs           # EXTRACT from VisualEditorScenePainter
|   |   +-- LivePreviewService.cs            # ADAPT from LiveAutotiler
|   +-- Colliders/
|   |   +-- ColliderPainter.cs               # NEW (mouse drag -> Collider2D)
|   |   +-- ColliderHandleRenderer.cs        # NEW (Handles gizmos)
|   |   +-- ColliderTemplateLibrary.cs       # ADAPT from BrushPalettePanel
|   +-- Layers/
|   |   +-- LayerPainter.cs                  # ADAPT from VisualEditorScenePainter
|   |   +-- LayerPanel.cs                    # ADAPT from LayerVisibilityPanel
|   |   +-- ParallaxDepthHandle.cs           # NEW
|   +-- Hotkeys/
|   |   +-- PainterHotkeyHandler.cs          # ADAPT from BrushHotkeyHandler
|   +-- Services/
|   |   +-- TargetResolverService.cs         # ADAPT from AutoLayeringService
|   |   +-- PainterExecutorRouter.cs         # ADAPT from BrushExecutorRouter
|   +-- UI/
|       +-- PainterSuiteWindow.uxml
|       +-- PainterSuiteWindow.uss
|       +-- LaurethTheme.uss
```

**Adim listesi gun-gun (Bolum 3) bu yapiya gore yazildi.**

---

## 6. SUB-AGENT DISPATCH PLANI (V2)

Her gun sonunda **memory + CURRENT_STATUS update** -- limit krash guvenligi icin.

### Gun 1: UPM iskelet (Codex)

Dispatch: **Codex (medium effort)**
```
GOREV: Create UPM package skeleton at Packages/com.laureth.painter-suite/
- package.json (version 0.1.0, Unity 2022.3+)
- Runtime/ + Editor/ asmdef files
- LaurethStudio.PainterSuite.Runtime + LaurethStudio.PainterSuite.Editor namespaces
- README stub, LICENSE stub, CHANGELOG stub
- Empty UXML/USS placeholders
KONTEKST: Standalone package, no RIMA references
VERIFY: Unity opens, package recognized, 0 compile error
DONE FILE: CODEX_DONE_<profile>.md
```

Memory update sonrasi: `feedback_painter_suite_v2_extract_pattern.md` (extract+decouple discipline notu)
Status update: S110 Phase 1 (PainterSuite) gun 1 done.

### Gun 2: Core extract (Codex xhigh)

Dispatch: **Codex (xhigh effort)**
```
GOREV: Extract VisualEditorScenePainter.cs -> Packages/.../Editor/Core/PainterSceneOverlay.cs
- Strip RIMA.MapDesigner.* references
- Replace BrushCategory with generic enum
- Replace AssetPoolSO with placeholder interface
- Keep: ghost preview, R rotation, undo group, snap logic
- Add minimal ColliderPainter.cs that uses overlay for drag-to-create BoxCollider2D
KONTEKST: <inline VisualEditorScenePainter.cs first 100 lines>
VERIFY: Package compiles standalone, drag in SceneView creates BoxCollider2D, undo works
DONE FILE: CODEX_DONE
```

Memory + status update.

### Gun 3-5: parallel modules (Codex chain)

Her biri ayri dispatch, hafta sonu QC pass.

### Gun 6-7: Asset gen (PixelLab) + manuel marketing

User manuel: PixelLab Web UI ile 5 prop sprite, trailer video, screenshot.

---

## 7. MEMORY + STATUS UPDATE PROTOKOLU

Her dispatch sonunda:

```
1. Memory update:
   - File: MEMORY/painter_suite_progress_<date>.md
   - Content: gun X done, files added/modified, next step
   - Add line in MEMORY/INDEX.md

2. CURRENT_STATUS update:
   - Append entry under "S110 PICKUP" section:
     "### PainterSuite Gun X
      - Done: <list>
      - Next: <next dispatch>
      - Files: <paths>"

3. Crash safety: even if session dies, next session reads CURRENT_STATUS + memory and resumes.
```

**Dispatch coreography:**
- Codex/Sonnet sub-agent dispatch -> beklerken Claude memory + status update yazar
- Dispatch sonucu gelince Claude verify + memory append + status append
- Limit %30'a duserse: full status sync + final memory checkpoint, sonra durdur

---

## 8. RISKS V2-SPECIFIC

| Risk | Mitigation |
|---|---|
| RIMA-spesifik isimler (CliffTilemap, FloorTilemap) extract sirasinda sizinti | `ITargetResolver` interface ile soyutla, RIMA wrapper'da hardcode kalir |
| `MapDesignerBrushPresetSO` SO referansi UPM'e gelirse Unity error | Pattern copy, tip yeniden tanimla |
| Asmdef referansi yanlis kurulursa Editor/Runtime ayrimi bozulur | RIMA'nin asmdef'lerini inceleyerek ayni pattern, ama bagimsiz |
| S110 calismasi UPM'de degil RIMA Assets'te yapilirsa cift refactor | **HARD RULE**: S110 PainterSuite konusu UPM package'a yazilir, RIMA wrapper sadece interface implementation |
| Decoupling test'i unutulursa, RIMA olmadan compile etmez | Her dispatch sonunda standalone compile test (otomatik) |
| `BrushPackSO` ve `BiomeSkinSO` -- RIMA-specific concept'ler UPM'e sizarsa | Generic `IAssetPack` interface, UPM ic tip yeni |

---

## 9. ACCEPTANCE CRITERIA (V2)

**v0.1.0 (Gun 1-2 sonu, internal alpha):**
- [ ] UPM package standalone compile (RIMA olmadan)
- [ ] 0 RIMA.* reference (`grep -r "RIMA\." Packages/com.laureth.painter-suite/` bos)
- [ ] PainterSuiteWindow Window menu'den acilir
- [ ] Drag-to-create BoxCollider2D calisir
- [ ] Undo/redo tutarli

**v0.5.0 (Gun 5 sonu, internal beta):**
- [ ] Template system tam
- [ ] Layer painter + ParallaxLayer extract calisir
- [ ] RIMA Project'inde (yan yana) sorunsuz beraber calisir (cakisma yok)
- [ ] S110 P0 isleri PainterSuite'e tasinmis durumda

**v1.0.0 (Gun 7 sonu, submission ready):**
- V1'deki acceptance + standalone compile + RIMA wrapper test edildi

---

## 10. NEXT IMMEDIATE STEPS

User onayi sonrasi:

1. **Memory update yaz** (`MEMORY/painter_suite_plan_v2_locked.md`)
2. **CURRENT_STATUS append** ("S110 P0 -> PainterSuite UPM extract")
3. **Dispatch 1**: Codex'e UPM iskelet gorev dosyasi (Gun 1)
4. Dispatch sonucu verify + memory + status update
5. Sonraki gun dispatch

---

## EK A: KAYIT TUTMA ANI (limit krash sigortasi)

Bu plan dosyasi + CURRENT_STATUS append + MEMORY entry, herhangi bir session'da kald ki yerden devam etmek icin yeterli.

Pickup checklist:
1. CURRENT_STATUS oku -> "S110 PainterSuite" entry'sine bak
2. MEMORY/painter_suite_progress_*.md son entry oku
3. Bu plan dosyasini oku (Bolum 6 dispatch listesi)
4. Bir sonraki gun gorevini calistir

---

## EK B: V1 ILE FARK OZETI

| Boyut | V1 (sifirdan) | V2 (RIMA reuse) |
|---|---|---|
| MVP timeline | 3 hafta | **5-7 gun** |
| Release toplam | 5-6 hafta | 2-3 hafta + Asset Store wait |
| Yeni LOC | ~3000-4000 | ~800-1200 (yeniden) + ~1500-2000 (extract) |
| Risk | Yeni kodun bug'i | Decoupling disiplini |
| RIMA sinerjisi | Ozel entegrasyon gerek | **Otomatik** (UPM consume) |
| Spin-out | UPM modularity gerek | **Zaten UPM'de** |

---

**End of V2 plan. ~3800 words.**

User'a sorulacak (V2 onaylanmadan once):
1. V2'yi V1 yerine takip et = OK?
2. S110 P0 isleri (Parallax Tab, Occlusion, Animated) UPM package'a yaz = OK?
3. Cliff manuel override + double trigger fix S110 onceligi bunlarla cakisir mi? Sira ne olmali?
