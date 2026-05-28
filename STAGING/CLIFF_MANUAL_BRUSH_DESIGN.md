# CLIFF MANUAL BRUSH + CLIFF/PARALLAX DEPTH CHOOSER — DESIGN DOC

**Status:** DESIGN ONLY (no code). S110 Phase 1.
**Author:** general-purpose Sonnet sub-agent (orchestrator dispatch).
**Spec:** `STAGING/s110_cliff_manual_brush_design_sonnet.md`.
**Date:** 2026-05-26.
**Amac:** Auto cliff system DEPRECATED. Designer Room Painter icine manuel cliff brush + Cliff/Parallax layer depth chooser entegrasyonu icin tam plan.

User talimati (verbatim):
> "Daha once tile cizip sildigim yerlerde cliff var su an aktif tile olmayan yerlerdeki cliffi sil"
> "Ben istedigim takdirde oralara serpistiririm ve istedigim seviyede (layer cliff ve layer parallax arasina secme imkani koyarak derinlik icin)"

---

## Bolum 1 — Onceki sistem ile karsilastirma

### Eski (CliffAutoPlacer)
- Konum: `Assets/Scripts/Environment/CliffAutoPlacer.cs` (LIVE kod, DISABLED component).
- Mantik: Floor tilemap'in S/SE/SW komsulari bos olan her cell'e cliff sprite kondu (S109 "ters yerlestirme" mantigi).
- Override: User cliff cell'i silince `manualOverrideCellsSerialized` listesine yazildi; sonraki `Regenerate()` o cell'i atladi.
- Trigger noktalari: MonoBehaviour `tilemapTileChanged` event + Editor `MouseUp` (DOUBLE auto-trigger anti-pattern, S109 feedback).
- Sonuc: Designer floor sildikten sonra eski cliff'ler kaldi (artikli void); user manuel silmek zorunda kaldi → otomatik sistem ters dustu.

### Yeni (Manuel cliff brush + depth chooser)
- Konum: Room Painter pipeline'i icinde (`Assets/Scripts/RoomPainter/*` + yeni Editor brush).
- Mantik: Designer paint eder, sistem onerme yapmaz. Her cliff bir `PlacementRecord` (RoomData.placements icinde).
- Layer secimi: Brush sirasinda toolbar/shortcut ile `RoomLayer.Cliff` veya `RoomLayer.Parallax` arasi gecis.
- Sprite secimi: Asset Palette'ten KitB_Cliff (9+ varyant) tek tek secim, scroll-wheel ile cycling.

### Avantaj / dezavantaj tablosu

| Konu | Auto (eski) | Manuel (yeni) |
|---|---|---|
| Kontrol | Algoritma karar verir | Designer karar verir |
| Iteration suresi | Floor degisince auto-regen (yas zaman fast) | Her cliff bir click (yavas) |
| Predictability | Surprise placements (algo overshoot) | Tam predictable |
| Auto-cleanup | Override blacklist ile karmasik | Tile siler = cliff siler (bir aksiyon) |
| Layer flexibility | Tek layer (Cliff) | 2 layer (Cliff + Parallax) |
| Asset variant | Tek sprite, dogal varyasyon yok | Designer scroll ile sec |
| Bug surface | DOUBLE trigger, override sync, regen race | Daha az durum, daha az event |
| Sahnedeki rol | Runtime DEPRECATED | Production-grade |

### CliffAutoPlacer akibeti
- **Code silinmez.** `MonoBehaviour.enabled = false` durumunda kalir (sahnede GO disable).
- Asm-def ve namespace `RIMA.Environment` korunur (gelecekteki "experimental auto" denemeleri icin reservoir).
- `[ContextMenu("Regenerate Cliff Ring")]` ve `Regenerate()` API erisilebilir — debug/test/emergency icin.
- ManualOverride code bloku (lines 22-70) artik unused; ileride silinebilir, su an dokunma (acil prod restore icin).
- Inspector'a "DEPRECATED — manual brush kullan" header eklenebilir (Phase A Day 3'de eklenmesi onerilir).

---

## Bolum 2 — Depth chooser UX

### Brush durumunda layer secimi
Designer Asset Palette'ten bir cliff sprite secince (orn `cliff_S`), brush tool'a u 3 yontemden biri ile layer atanir:

1. **Toolbar dropdown** (default): VisualEditor window'unun ust kisminda `Layer: [Cliff] [Parallax]` segment butonu. Cliff aktif iken: yesil/cyan vurgu. Parallax aktif iken: mor vurgu.
2. **Klavye shortcut** (hizli):
   - `1` → `RoomLayer.Cliff` (foreground, gameplay-aware)
   - `2` → `RoomLayer.Parallax` (background, camera-relative)
   - Shortcut isleminden sonra brush mode korunur, sadece target layer degisir.
3. **Asset default fallback** (bypass): `RoomPainterAsset.defaultLayer` field'i. Designer bunu cliff asset'lerinde `Cliff` olarak set ettiyse, layer secimi yapilmadiginda da Cliff'e duser.

### Ghost preview color coding
Brush cursor'unun (drag-mouse pattern, ColliderPainter.cs lines 110-121 reuse) seffaf ghost overlay renkleri:
- `RoomLayer.Cliff` aktif: cyan `(0.4, 0.9, 1.0, 0.30)` fill, `(0.4, 0.9, 1.0, 0.85)` edge.
- `RoomLayer.Parallax` aktif: purple `(0.8, 0.5, 1.0, 0.25)` fill, `(0.8, 0.5, 1.0, 0.85)` edge.
- Cursor yaninda kucuk label: `[Cliff]` veya `[Parallax]` (Handles.Label, ColliderPainter line 120 pattern).

### Layer label / inspector hint
Sahnede placement uretilince `GameObject.name` su patternde uretilir:
- `Cliff_KitB_Cliff_S` veya `Parallax_KitB_Cliff_E`
Hierarchy'de kolay ayirt etmek icin. Sirasiyla `Sorting Layer` ve `Sorting Order` `RoomLayerData[Cliff]` / `RoomLayerData[Parallax]` config'inden gelir.

### Sample shortcut combos
- `1` + click → cliff yerlestir
- `2` + drag → parallax cliff cizgi cek (3-5 sprite art arda paint)
- `Alt+click` → erase (her iki layer'da da)
- `R` → 90 derece rotate (VisualEditor `RotateBrush` zaten var)
- `Scroll` → varyant cycle (Bolum 3)

---

## Bolum 3 — Cliff brush spec

### Brush mode'lar (tek window state machine)
- **Brush** (default): MouseDown + Drag = paint, MouseUp = commit.
- **Erase** (RMB): Alt+click veya right-click ile aktif layer'daki cliff'i temizle.
- **Variant cycle**: scroll wheel ile ayni "cliff" kategorisinin varyantlari arasi gecis (Asset Palette'in `category == "Cliff"` filtresi ile siralanmis liste).

### Interaction model (ColliderPainter pattern reuse)
ColliderPainter.cs'in `HandleBox` metodu (lines 71-96) referans alinacak:

```
MouseDown (button 0) → StartDrag(worldPos, "Paint Cliff")
  + Undo.IncrementCurrentGroup()
  + capture _undoGroup

MouseDrag (while _isDragging) → 
  + _dragCurrentWorld = worldPos
  + cell delta ile sonraki cell'e gecince TryPaintCell()
  + sceneView.Repaint()

MouseUp (button 0) → 
  + _isDragging = false
  + Undo.CollapseUndoOperations(_undoGroup)
  + (NO regen, NO event broadcast — placement zaten her cell'de yapildi)

Repaint → 
  + DrawGhost(currentWorldPos, brushSize)
  + ghost rengi = layer-based (Bolum 2)
```

### Sprite varyant cycling
KitB_Cliff folder'i (verified 2026-05-26):
- 8 yon: `cliff_N, cliff_NE, cliff_E, cliff_SE, cliff_S, cliff_SW, cliff_W, cliff_NW`
- 4 S varyant: `cliff_S_new1..4`
- 1 ozel: `cliff_cyan_glow`
- TOPLAM: 13 sprite

Scroll wheel index = `(index + scrollDelta) % count`. Active variant Asset Palette'te highlight'lanir (UI feedback). Default sprite olarak palette secimi (`SelectedAsset`) baz alinir; cycling RoomPainterAsset listesini `category=="Cliff"` filter'inden uretir.

### Cell snapping
- `SnapToPixel = true` (default), `PixelsPerUnit = 64` (KitB_Cliff PPU ile uyumlu).
- Isometric Tilemap kullanildiginda: ColliderPainter'in serbest world-pixel snap'i YETMEZ. Cliff icin `Grid.WorldToCell` → `Grid.CellToWorld` snap kullanmali ki tile'lar duzgun yerine otursun. Bu cliff brush'a OZGU bir snap mode'u (`SnapMode.IsoCell`) gerektirir.
- Iso vector reference: S=(-1,-1), N=(1,1), E=(1,-1), W=(-1,1), SE=(0,-1), SW=(-1,0) (S109 LIVE memory).

### Erase pattern
- Alt+left-click veya right-click → currentCell'deki ayni-layer cliff placement'i bul, `RoomData.placements.RemoveAll(...)` + sahne GO Destroy + `Undo.DestroyObjectImmediate`.
- Layer-aware: Cliff mode iken Parallax cliff silinmez (toggle ile diger layer'a gecmen lazim). Veya ekstra `EraseAllLayers` toggle (UI option, default OFF).

### Undo group discipline
- Her bir drag bir undo unit (StartDrag → MouseUp aralligi tek `Undo.CollapseUndoOperations` ile sarilir).
- Designer Ctrl+Z bastiginda tum drag (orn 8 cell paint) bir kerede geri alinir — VisualEditor `BrushExecutorRouter` ile uyumlu pattern.

---

## Bolum 4 — Layer integration (RoomData)

### Veri akisi
Mevcut `RoomData.PlacementRecord` (RoomData.cs lines 56-64) yapisi:
```
public struct PlacementRecord {
    public RoomPainterAsset asset;
    public Vector3 worldPos;
    public RoomLayer layer;
    public int orderOverride;
    public Vector2 scaleOverride;
}
```

Cliff brush her paint isleminde sunu yapar:
1. `RoomPainterAsset` = palette'ten secili cliff sprite asset (orn `cliff_S` asset).
2. `worldPos` = snap'lenmis world pos.
3. `layer` = brush tool'un aktif layer'i (`Cliff` veya `Parallax`).
4. `orderOverride` = 0 (RoomLayerData.defaultOrder kullanilir).
5. `scaleOverride` = `Vector2.one` (default).

Bu kayit `RoomData.placements` listesine eklenir, sahne'de GO instantiate edilir.

### RoomLayerData config (cliff / parallax)

`RoomLayerData` (RoomLayerData.cs) icin onerilen onerilen production setting'leri:

#### `Cliff` layer asset (RoomLayerData_Cliff.asset)
```
layer            = RoomLayer.Cliff
sortingLayerName = "Cliff"           (yeni — Project Settings'e eklenmesi gerek)
defaultOrder     = 0
depthValue       = 0.0               (parallax factor yok — gameplay layer)
isStatic         = true
isRoomLocked     = true
isCameraRelative = false
ySortEnabled     = true              (Y-sort drop edge sarkma icin)
```

#### `Parallax` layer asset (RoomLayerData_Parallax.asset)
```
layer            = RoomLayer.Parallax
sortingLayerName = "Background"      (mevcut)
defaultOrder     = -50
depthValue       = 0.8               (factor formul argumani — bkz asagi)
isStatic         = false             (camera-relative)
isRoomLocked     = false
isCameraRelative = true
ySortEnabled     = false             (parallax bg sabit z-order)
```

### Parallax factor formulu
`ParallaxLayer.cs` (Packages/...PainterSuite/Runtime/ParallaxLayer.cs lines 17-30) factor'i `Vector2 factor = (0.1f, 0.05f)` ile gelir.

Cliff sprite'i Parallax layer'a konunca runtime'da ParallaxLayer component eklenir:
```
factor.x = (1.0 - depthValue) * 0.15   // 0.8 depth → 0.03 factor (uzak)
factor.y = (1.0 - depthValue) * 0.10   // sortax aware
```

Yani `depthValue=0` (kameraya yakin) → factor=(0.15, 0.10). `depthValue=1` (en uzak) → factor=(0, 0). Designer `depthValue`'yu RoomLayerData'da slider ile ayarlar.

NOT: Bu formul `RoomLayerData.depthValue` field'ini Parallax kullanir, Cliff'i etkilemez. Cliff'te `depthValue` Y-sort offset tweak'i icin kullanilabilir (ileride).

### Runtime instantiator (mevcut sistem kullan)
- Eger projede RoomData → sahne instantiate eden RoomBuilder/RoomLoader varsa, oraya `if (layer == Parallax) GO.AddComponent<ParallaxLayer>()` blok eklenir.
- Yoksa: Yeni mini-runtime `RoomDataInstantiator.cs` script'i (Phase A Day 4'de yazilir, BU TASK'ta YOK).

### Cliff Y-sort integration
Mevcut `CliffYSortManager` (Assets/Scripts/Systems/Map/CliffYSortManager.cs) Tilemap-based calisir (tile renderer mode flip). Manuel cliff brush GO-based oldugu icin `CliffYSortManager` DOGRUDAN cliff GO'lari etkilemez.

**Cozum:** Cliff GO'larin SpriteRenderer'inin `sortingOrder`'i Y-pos'a gore otomatik update edilir (mevcut RIMA Y-sort system varsa onunla entegre, yoksa basit `transform.position.y * -10` formulu).

---

## Bolum 5 — Migration path

### Mevcut sahne durumu
- `Assets/Scenes/Test/PlayableArena_Test01.unity` — cliff Tilemap orchestrator tarafindan temizlendi (S110 sabah).
- `CliffAutoPlacer` GO `enabled = false` durumunda sahnede mevcut.
- `Tilemap_Cliff` GO (Grid altinda) BOS — hicbir tile yok.

### Adim 1: Asset Palette'e cliff kategorisi
- `RoomPainterAsset` instance'lari otomatik scan: `Resources.LoadAll<RoomPainterAsset>("...")` veya `AssetDatabase.FindAssets("t:RoomPainterAsset")` ile yuklenir.
- Eger cliff asset'leri henuz yok ise: KitB_Cliff PNG'leri icin 13 adet `RoomPainterAsset` SO yaratilmali. Otomatik bir editor menu item ile (`RIMA/Room Painter/Generate Cliff Assets from KitB`) yapilabilir. Bu da BU TASK disinda — Phase A Day 2.

### Adim 2: Cliff layer assets uretimi
- `RoomLayerData_Cliff.asset` ve `RoomLayerData_Parallax.asset` yaratilir (CreateAssetMenu var, manuel right-click ile).
- Default `RoomData` asset'lerine eklenir (RoomData.layers slot 2 = Cliff, slot 9 = Parallax).

### Adim 3: Sahneye cliff yerlestirme (designer flow)
1. Window > RIMA > Room Painter (yeni) ac.
2. Active Pack + Active Skin sec (mevcut auto-load).
3. Asset Palette'te Cliff kategorisi (filter dropdown).
4. Bir sprite sec (orn `cliff_S`).
5. Layer toolbar'da `Cliff` veya `Parallax` sec.
6. SceneView'de cell'e tikla / drag.
7. Ctrl+S kaydet.

### Adim 4: Legacy auto regen (acil durum)
- Inspector'da `CliffAutoPlacer` GO'sini sec.
- "DEPRECATED — manual brush kullan" header'i gor.
- Yine de `Regenerate Cliff Ring` ContextMenu ile bir kerelik auto-fill yapabilir. Sonra manuel rotusla bitirir.
- NOT: auto-regen TileBase tile koyar (TilemapBased), manuel brush GO koyar — iki sistem ayni sahne'de cakismaz cunku farkli layer'lar.

### Adim 5: Phase A Day 3-4 testi
- Test scene'de 5 farkli cliff config dene:
  1. Floor kenarinda 4 yonlu cliff (basic case).
  2. Ic pocket (donut floor) cliff iceriye sarkma.
  3. Parallax cliff sahnenin uzak bg'sinde.
  4. Karisik: yakinda Cliff, uzakta Parallax.
  5. Variant mix: cliff_S_new1, _new2, _new3 ardisik.

---

## Bolum 6 — Phase A Day 2-3 etkisi

### Day 2 (Asset Palette)
**Etki:** MINIMAL. Cliff sprite'lar dogal sekilde `category == "Cliff"` ile palette'te gorunur. Sadece 13 `RoomPainterAsset` SO uretimi gerekir (auto menu item ile 5 dakika). Filter dropdown'una Cliff kategorisi eklenir.

**Day 2 acceptance criteria (cliff icin):**
- Asset Palette'te "Cliff" filter secilince 13 sprite gosterilir.
- Sprite secince `RoomPainterAsset.defaultLayer == Cliff`.
- Scroll wheel cycling icin liste hazir.

### Day 3 (SceneView placement)
**Etki:** BUYUK. Manuel cliff brush bu day'in temel dönüşümü. Implementation noktalari:
- `VisualEditorScenePainter` (mevcut, lines: bkz RimaVisualMapEditorWindow.cs) icine cliff brush handler eklenir.
- ColliderPainter `HandleBox` pattern'i adapte edilir (drag = paint cell, mouseup = commit undo group).
- Layer toolbar UI VisualEditor window'una eklenir.
- Ghost preview color coding eklenir.
- Iso cell snap mode (`Grid.WorldToCell`) implement edilir.

**Day 3 acceptance criteria:**
- Designer cliff_S secip 5 cell drag → 5 cliff GO sahnede.
- Layer Cliff / Parallax toolbar segment calisir.
- Alt+click erase calisir.
- Ctrl+Z tek seferde tum drag'i geri alir.

### Day 4 (Layer system)
**Etki:** ORTA. Cliff/Parallax depth chooser bu day'e bagli. Implementation noktalari:
- RoomLayerData_Cliff.asset ve _Parallax.asset uretilir.
- `RoomDataInstantiator` (yeni runtime) `if (layer == Parallax) AddComponent<ParallaxLayer>()` mantigi.
- `ParallaxLayer.factor` = `RoomLayerData.depthValue`'dan turetilen formul.
- `CliffYSortManager` Tilemap-based oldugu icin DOKUNULMAZ; GO-based cliff icin ayri SpriteRenderer Y-sort sistemi (mevcut RIMA Y-sort varsa onunla baglan).
- Drop edge sarkma (`transformOffset.y < 0` ile asagi sarkma) Cliff layer'da default ON.

**Day 4 acceptance criteria:**
- Parallax cliff sahnede kamera hareket edince yavas hareket eder.
- Cliff cliff yakindaki player'in onunde/arkasinda dogru Y-sort (drop edge sarkma).
- Designer `depthValue` slider'ini cevirince parallax hizi degisir.

---

## OPEN QUESTIONS (orchestrator karar versin / Codex+agy yorum)

1. **Cliff sprite varyant secimi:** Manuel mi (Asset Palette secimi + scroll cycle) yoksa otomatik mi (komsu cliff'e bakarak otomatik yon secimi gibi auto wang16 mantigi)?
   - Bu doc default manuel oneriyor (designer kontrolu). Auto-yon kontrolu (komsu floor'a bakarak yon segmesi) Phase B'ye birakilabilir.
2. **R tuslu rotasyon vs farkli sprite seti:** KitB_Cliff zaten 8 yon var, R rotasyonu gerekli mi? VisualEditor'da `RotateBrush(float)` mevcut.
   - Default: R tusu sprite cycling'e degil, brush rotasyon transform'una bagli (mevcut davranis). Cliff icin de geçerli — designer rotate ederse sprite donerek yerlesir.
3. **Cliff Tilemap (eski) sahnede kalsin mi?** GO `enabled=false` mevcut. Tamamen kaldirilsa daha temiz ama legacy `Regenerate` icin lazim.
   - Default: kalsin (disabled), Inspector'a "DEPRECATED" header eklensin.
4. **Erase tek-layer mi tum-layer mi?** Default tek layer (aktif olan), opsiyonel "Erase All Layers" toggle.
5. **Cliff GO instantiation timing:** Editor-time (RoomData asset'e yazilinca hemen sahnede gorulur) mi yoksa Play-time (RoomData runtime'da yuklenince) mi?
   - Default: Editor-time (designer hemen gorsun). Play-time RoomDataInstantiator ek olarak.

---

## TOP 3 RISK

### Risk 1 — RoomDataInstantiator henuz yok (Day 4 bagimliligi)
**Problem:** Manuel cliff brush placement'lar `RoomData.placements`'a yazar ama bu listeyi runtime'da sahneye instantiate eden bir script HENUZ YOK. Phase A Day 4'de yazilmasi planlanan `RoomDataInstantiator` (veya benzer) olmadan cliff sahne'de gorulmez.
**Etki:** Day 3'de manuel brush calisir gibi gozukur ama sahne save edilince hicbir sey gorulmez (data var, GO yok).
**Cozum:** Day 3 brush implement edilirken AYNI ZAMANDA editor-time `OnGUI` veya `OnSceneGUI` hook'unda anlik GO instantiate edilmeli (ColliderPainter `Undo.AddComponent` pattern'i: brush her cell'de `Undo.AddComponent<SpriteRenderer>` benzeri Instantiate). Bu Day 3'un scope'una giriyor.

### Risk 2 — Parallax cliff Y-sort cakismasi
**Problem:** Cliff layer'da Y-sort enabled, Parallax layer'da Y-sort disabled. Designer ayni sprite'i (cliff_S) hem Cliff'e hem Parallax'a koyarsa, runtime'da:
- Cliff_S (Cliff layer) → Y-sort enabled → player'in onunde/arkasinda dinamik.
- Cliff_S (Parallax layer) → Y-sort disabled, sortingLayer="Background" → her zaman bg'de.

Bu DOGRU davranis ama designer baslangicta confuse olabilir ("neden ayni sprite farkli gorunuyor?").
**Cozum:** Ghost preview rengi + GO name prefix (`Cliff_*` / `Parallax_*`) + RoomLayerData inspector'da `sortingLayerName` field'ini gorunür yap. Visual cue cok.

### Risk 3 — Iso cell snap mode'u ColliderPainter pattern'inde YOK
**Problem:** ColliderPainter free-pixel snap kullanir (`Mathf.Round(world.x * PPU) / PPU`). Manuel cliff brush icin **iso tilemap cell snap** sart (yoksa cliff'ler diamond grid'e oturmaz). Iso vector hesaplama (S109 memory: S=(-1,-1) vb) brush'in cell ↔ world donusumune entegre edilmeli.
**Etki:** Eger free-pixel snap kullanilirsa cliff'ler diamond grid'in disinda kalir, gorsel olarak yamuk durur.
**Cozum:** Brush'in `Snap()` fonksiyonu `Grid.WorldToCell(world)` → `Grid.CellToWorld(cell)` cevrim yapsin. Grid component sahne hierarchy'sinden bulunur (RoomData veya VisualEditor active grid reference'i ile). Test scene'de Grid.CellLayout = Isometric oldugu dogrulanmali.

---

## REFERENCES (file paths, absolute)

- Eski sistem: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\Environment\CliffAutoPlacer.cs`
- Brush pattern: `F:\Antigravity Projeler\2d roguelite\RIMA\Packages\com.laureth.painter-suite\Editor\Colliders\ColliderPainter.cs`
- Parallax runtime: `F:\Antigravity Projeler\2d roguelite\RIMA\Packages\com.laureth.painter-suite\Runtime\ParallaxLayer.cs`
- RoomData: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\RoomPainter\RoomData.cs`
- RoomLayer enum: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\RoomPainter\RoomLayer.cs`
- RoomLayerData: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\RoomPainter\RoomLayerData.cs`
- RoomPainterAsset: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\RoomPainter\RoomPainterAsset.cs`
- Mevcut VisualEditor: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Editor\MapDesigner\VisualEditor\RimaVisualMapEditorWindow.cs`
- KitB_Cliff sprite'lari: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Environment\KitB_Cliff\` (13 PNG)
- Cliff Y-sort (Tilemap-based, dokunma): `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\Systems\Map\CliffYSortManager.cs`
- Test sahne: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scenes\Test\PlayableArena_Test01.unity`

---

**END DESIGN DOC.** Implementation Phase A Day 3-4'de Codex/Sonnet'e dispatch edilecek. Bu doc'tan baska kod yazilmadi.
