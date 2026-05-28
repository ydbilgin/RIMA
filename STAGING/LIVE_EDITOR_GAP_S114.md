# Live Editor Gap Analizi (S114)

**Tarih:** 2026-05-29  
**Referans spec:** `STAGING/T3_TOOL_FULL_DESIGN.md` §0.5 L1-L9 + §2 komponent tablosu + §3 F1-F7  
**Yöntem:** Kurulu dosyalar doğrudan okundu, spec ile karşılaştırıldı. Kod yazılmadı.

---

## Komponent Durum Tablosu

| C# | Bileşen | Dosya (gerçek konum) | Durum | Ne Yapıyor (gerçekte) |
|----|---------|---------------------|-------|----------------------|
| C2 | RoomLayoutSerializer | `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs` | **ÇALIŞIYOR** | Aktif Editor sahnesini tarar; floor tilemap → `floor_tiles[]`, prefab instances → `prop_instances[]` + `collider_overrides[]` JSON'a yazar. `cliff_cells[]` alanı şema tanımlı ama **serialize edilmiyor** — sadece boş liste. `WriteCurrent()` `StreamingAssets/live/room_current.json`'a yazar. Deserialize (geri okuma) de implemente edilmiş; JSON→Tilemap+prefab sahneye aktarır. |
| C3 | RuntimeAssetRegistryBaker | `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs` | **ÇALIŞIYOR** | 6 scan root tarar (Sprites/Environment, KitB_Cliff, Prefabs/Props, Walls, Obstacles, ScriptableObjects); Sprite+TileBase+Prefab GUID kaydeder. `RoomPainterPhysicsRules` ile layer + keyword tag resolver. `registry_manifest.txt` diff üretir. RIMA menüsünde `Bake Asset Registry` olarak görünür. |
| C4 | RuntimeAssetRegistry | `Assets/Scripts/Live/RuntimeAssetRegistry.cs` | **ÇALIŞIYOR** | Baked SO. `Get/GetSprite/GetTile/GetPrefab/GetByTag/GetByLayer/Contains` API frozen. O(1) dictionary lookup (OnEnable'da rebuild). `Resources.Load<>` ile runtime erişim + EditorAssetDatabase ile editor erişim ayrıştırılmış. |
| C5 | Tool UI Bootstrap | `Assets/Scripts/LiveTool/ToolBootstrap.cs` | **EKSİK** | Dosya yok. Spec: UIDocument + UXML panel + Tool.exe sahne kökü. |
| C5 | ToolMain.unity | `Assets/Scenes/LiveTool/ToolMain.unity` | **EKSİK** | Sahne yok. LiveToolLauncher build adımında bu sahneyi arar ama bulamaz → Tool build skip log üretir. |
| C5 | ToolMain.uxml / .uss | `Assets/UI/LiveTool/` | **EKSİK** | Klasör bile yok. |
| C6 | RuntimeBrushPalette | `Assets/Editor/RoomPainter/LiveTool/RuntimeBrushPalette.cs` | **ÇALIŞIYOR (Editor-hybrid)** | Spec'te `Assets/Scripts/LiveTool/Palette/` konumunda çalışma-zamanı bileşeni olması öngörüldü. Gerçekte Editor namespace'inde (`RIMA.Editor.RoomPainter.LiveTool`). Mode/Layer/Search filter, dirty-flag rebuild, SelectedEntry — tam işlevsel veri modeli. Tool.exe'de kullanılamaz (Editor API'siz derlenmez). |
| C7 | RuntimeColliderHandles | `Assets/Editor/RoomPainter/LiveTool/RuntimeColliderHandles.cs` | **ÇALIŞIYOR (Editor-hybrid)** | Spec: Tool.exe içinde UI Toolkit tabanlı. Gerçekte: IMGUI EditorWindow paneli. BoxCollider2D (8 handle), CircleCollider2D (2 handle), CapsuleCollider2D (2 handle) drag-resize + Ctrl+Z undo (depth 32) + ColliderShapeSwapper entegrasyonu LIVE. `Handles.BeginGUI` kullandığı için Tool.exe'ye taşınamaz. |
| C8 | RuntimeCliffHoverIndicator | `Assets/Scripts/LiveTool/Authoring/RuntimeCliffHoverIndicator.cs` | **EKSİK** | Dosya yok. Spec: kamera+cursor tabanlı SpriteRenderer cliff hover önizleme. |
| C9 | RuntimeAssetLoader | `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetLoader.cs` | **ÇALIŞIYOR (Editor-hybrid)** | Spec konumu: `Assets/Scripts/LiveTool/Runtime/`. Gerçek konum: Editor namespace. `AssetDatabase.LoadAssetAtPath` ile `RuntimeAssetRegistry.asset` yükler, cache tutar. Tool.exe'de kullanılamaz. |
| C10 | LiveRoomReloader | `Assets/Scripts/Live/LiveRoomReloader.cs` | **ÇALIŞIYOR** | `RuntimeInitializeOnLoadMethod` ile bootstrap. `RoomLoader.OnRoomLoaded` static event'e hook (spec §7.1 düzeltmesi uygulanmış). Floor tilemap apply (`ClearAllTiles` + `SetTile` batch). Prop diff (stable ID: `instance_id` veya `guid+index`). Player snap via reflection (WalkabilityMap). `#if DEVELOPMENT_BUILD \|\| UNITY_EDITOR` guard. |
| C11 | JsonFileWatcher | `Assets/Scripts/Live/JsonFileWatcher.cs` | **ÇALIŞIYOR** | `FileSystemWatcher` + 100 ms debounce + 500 ms polling fallback + lock file wait (timeout 500 ms) + volatile flag main-thread marshal. `#if DEVELOPMENT_BUILD \|\| UNITY_EDITOR` guard. |
| C12 | LiveToolLauncher | `Assets/Editor/RoomPainter/LiveTool/LiveToolLauncher.cs` | **ÇALIŞIYOR (kısmi)** | `Builds/RIMA_Tool/RIMA_Tool.exe` + `Builds/RIMA_Game/RIMA.exe` process spawn. PID tracking + StopAll. `RimaRoomPainterWindow` toolbar'a entegre (F6 button LIVE). Build pipeline: `ToolMain.unity` olmadığı için Tool build skip eder, Game build normal pipeline'dan çalışır. |
| — | LiveToolBuildProcessor | `Assets/Editor/Build/LiveToolBuildProcessor.cs` | **EKSİK** | Dosya yok. Spec: `BuildPlayerProcessor` pre-build hook, `RIMA_LIVE_TOOL` scripting define inject. LiveToolLauncher içinde inline `BuildBothTargets()` ile kısmen kapanıyor ama ayrı processor sınıfı yok. |
| — | RuntimeAssetRegistry.asset | `Assets/Resources/Live/RuntimeAssetRegistry.asset` | **EKSİK (runtime)** | Klasör var (`Assets/Resources/Live/` dizini oluşturulmamış — bake yapılmamış demek). Baker bake edilmeden dosya üretilmez. |
| — | room_current.json | `Assets/StreamingAssets/live/room_current.json` | **EKSİK (üretilecek)** | `Assets/StreamingAssets/live/` dizini `.gitkeep` ile var. JSON yok — `WriteCurrent()` çağrılmamış veya commit edilmemiş. |
| F7 | LiveToolSmokeTests | `Assets/Tests/EditMode/LiveToolSmokeTests.cs` | **ÇALIŞIYOR** | 23 EditMode test: `RoomLayoutData.FromJson` parse (8 test) + `RuntimeAssetRegistry` GUID lookup API (9 test) + JSON↔Registry integration (3 test). Manual smoke checklist (15 step) inline comment olarak mevcut. FileSystemWatcher/process-spawn testleri intentionally exclude. |

**Özet:** 12 bileşenden 7'si çalışıyor, 5'i eksik veya stub.

---

## T3-Spec vs Mevcut Hibrit (Eksik T3 Parçaları)

Spec §1.1 tanımına göre T3 üç ayrıfat: Editor (mevcut) + Tool.exe (ayrı Player Build, UI Toolkit Runtime) + Game.exe (LiveRoomReloader ile). Mevcut durum **T3 değil, T2-benzeri editor-hibrit**:

### Mevcut hibrit nasıl çalışıyor
```
Editor Window (IMGUI)                 Game.exe (Development Build)
──────────────────────────────────    ────────────────────────────
LiveToolPaletteWindow (C5 rolü)  →    LiveRoomReloader (C10) ✅
RuntimeBrushPalette (C6)         →    JsonFileWatcher (C11) ✅
RuntimeColliderHandles (C7)           RoomLayoutData (schema) ✅
RuntimeAssetLoader (C9)               RuntimeAssetRegistry ✅ (baked SO)
LiveToolLauncher (C12) ✅
```

C6, C7, C9 Editor namespace'inde olduğu için Tool.exe Player Build'ına giremez.

### Spec'e göre eksik T3 parçaları

| Eksik Parça | Spec Referansı | Etkisi |
|-------------|---------------|--------|
| `ToolBootstrap.cs` (C5) | F3, §2.1 C5 | Tool.exe başlatma mantığı yok — sahne boş kalır |
| `Assets/Scenes/LiveTool/ToolMain.unity` | F3, §1.4 | Tool build mevcut değil; LiveToolLauncher build'da bu sahneyi bulamayınca skip eder |
| `Assets/UI/LiveTool/ToolMain.uxml` | F3, §3 UXML hierarchy | UI Toolkit Runtime panel yok |
| `Assets/UI/LiveTool/ToolMain.uss` | F3 | Stil yok |
| `RuntimeCliffHoverIndicator.cs` (C8) | F4, §2.1 C8 | Cliff hover preview Tool.exe'de yok |
| `LiveToolBuildProcessor.cs` | F6, §7.2 | `RIMA_LIVE_TOOL` scripting define inject yok; build config belirsiz |
| `Assets/Scripts/LiveTool/Runtime/BrushExecutorRouter.cs` | §7.2 | Runtime paint action router yok — paint olmuyor |
| `Assets/Resources/Live/RuntimeAssetRegistry.asset` | F2 | Bake yapılmamış; çalışan bir Game.exe'de asset resolve çalışmaz |

**Sonuç:** Tool.exe yolu (~C5+C8+BrushExecutorRouter+UXML/USS) tamamen inşa edilmemiş. Mevcut kurulum "Editor palette göster + Game.exe file-watch reload" kombinasyonundan ibaret. Bu fonksiyonel olarak T2 kapsamına denk düşüyor — T3'ün artı değeri olan bağımsız Tool.exe henüz yok.

---

## Cliff Live-Reload Neden No-Op (Kök Neden)

### Birincil neden: Serializer cliff hücreleri yazmıyor

`RoomLayoutSerializer.cs` satır 50-58 incelendiğinde, tüm Tilemap bileşenlerine `AddTiles()` çağrılıyor. Bu `AddTiles()` metodu **tile_guid** bekliyor — ama cliff tilemap'teki `TileBase`'ler için GUID'ler geçerli olsa dahi `cliff_cells[]` dizisine değil, `floor_tiles[]` dizisine yazılıyor (her tilemap aynı metoda aktarılıyor). Schema'da `cliff_cells` listesi şu alanları içeriyor:

```csharp
private sealed class CliffCell { public int[] cell; public bool is_decor; }
```

Burada `tile_guid` yok. `AddTiles()` fonksiyonu sadece `FloorTile` üretir — cliff tilemap'i floor_tiles'a karıştırır ya da tamamen atlar.

### İkincil neden: Reloader cliff_cells'i bilinçli olarak görmezden geliyor

`LiveRoomReloader.ApplyCliffTiles()` (satır 205-213) açık bir `// no-op` içeriyor:

```csharp
// Cliff cells don't carry a tile_guid in schema 1.0 cliff_cells list —
// they are placed by the painter via their own tilemap channel.
// If the cliff tilemap exists we leave it untouched unless explicit data arrives.
// (Cliff reconstruction from GUID data is F5+ scope — no-op here is safe.)
```

### Üçüncül neden: Cliff tilemap bulma zayıf

`HandleRoomLoaded()` satır 107: cliff tilemap'i `tm.name.ToLowerInvariant().Contains("cliff")` ile arar. Eğer sahne'deki GameObject adı farklıysa `_cliffTilemap` null kalır ve reload hiç tetiklenmez.

### Kök neden özeti

Schema tasarımında `cliff_cells[]` sadece hücre koordinatı + `is_decor` flag tutuyor, `tile_guid` yok. Dolayısıyla Game.exe hangi cliff tile varlığını koyacağını bilemez. Floor pipeline'da çalışan "GUID → TileBase.SetTile" zinciri cliff için mevcut değil. Bu Serializer tarafında kasıtlı bir defer — "F5+ scope" olarak bırakılmış.

**Fix için gerekli adımlar:**
1. `CliffCell` struct'ına `tile_guid` ekle (Serializer tarafı)
2. `AddTiles()` yerine cliff tilemap için ayrı `AddCliffTiles()` yaz, `CliffCell` listesine yaz
3. `LiveRoomReloader.ApplyCliffTiles()` içindeki no-op'u aktif kod ile değiştir
4. `_cliffTilemap` bulma mantığını GameObject adından naming convention'a bağla (veya tag kullan)

---

## FİZİBİLİTE-Sıralı Backlog

| # | Item | Efor | Değer | Risk | Gece-yapılabilir? | Notlar |
|---|------|------|-------|------|-------------------|--------|
| 1 | **Cliff live-reload fix** (Serializer + Reloader) | S (2-3 saat) | Yüksek — "bilinen defer" kapanır, demo flow tamamlanır | Düşük — schema additive, mevcut floor pipeline template | Evet — Unity kapalıyken C# düzenleme, test EditMode | `tile_guid` cliff_cells'e eklenir + `ApplyCliffTiles` aktif olur. Floor pipeline zaten kanıtlı. |
| 2 | **E2E file-watch verify** (room_current.json üret → Game.exe reload) | S (1-2 saat) | Yüksek — temel "yazı görünür" loop doğrulanır | Düşük — C10+C11 LIVE, sadece test edilmedi | Evet — bake + test playmode | `WriteCurrent()` → JSON oluştur → Development Build'da Game'e bakıp Tilemap+prop reload çalışıyor mu doğrula. |
| 3 | **RuntimeAssetRegistry.asset bake + verify** | XS (30 dk) | Yüksek — C10 ResolveTile/ResolvePrefab çağrıları bu olmadan null döner | Düşük — baker LIVE, sadece bake edilmemiş | Evet | `RIMA → Live Tool → Bake Asset Registry` çalıştır, manifest.txt oluştuğunu doğrula, EditMode testleri koş. |
| 4 | **L5 Transient persist (explicit Save/Apply ayrımı)** | M (4-6 saat) | Orta — spec'te "YENI" işaretli, T3 ikinci katman | Orta — şema değişikliği, geri uyumluluk testi gerekir | Evet — animasyon bağımsız, saf C# | Spec §0.5 L5: authoring JSON yazılır, "transient" (test drag) yazılmaz. "Save/Apply" butonu ile iki-state. Şu an her tile yerleştirmede JSON güncellenyor — explicit save ayrımı yok. |
| 5 | **Palette UX polish** (thumbnail async reload, Cliff mode filter doğrulaması) | S (2-3 saat) | Orta — kullanılabilirlik | Düşük — sadece EditMode Window, Unity compiler yeterli | Evet | `PaletteMode.Cliff` şu an `e.tag == "cliff"` kullanıyor — baker'ın KitB_Cliff klasörünü doğru tag ile yakaladığı doğrulanmalı. Thumb cache async yükleme için `AssetPreview.IsLoadingAssetPreviews()` + Repaint loop eklenebilir. |
| 6 | **Standalone Tool.exe (C5 ToolBootstrap + ToolMain.unity + UXML/USS)** | L (3-5 gün) | Yüksek (T3 tam hedef) | Yüksek — UI Toolkit Runtime, yeni sahne, scripting define build config, IMGUI bileşenlerini runtime portlama | Hayır — Unity açık gerekli, büyük iş | C5+C8+BrushExecutorRouter tümüyle eksik. Önce item #1-3 ile Editor-hibrit stable olsun, sonra Tool.exe portlaması başlasın. |
| 7 | **L9 Region-ID occlusion** (foreground/çatı fade) | L (1-2 hafta) | Düşük-orta — görsel polish, demo için zorunlu değil | Yüksek — Shader/SortingGroup + yeni veri modeli | Hayır | Spec §0.5 L9 "YENI feature kandidati (roadmap)". Demo milestone için gerekli değil. |

---

## ÖNERİ: Bu Gece Hangi 1-2 Item Yapılmalı

### Item A — RuntimeAssetRegistry.asset bake + E2E verify (#3 + #2 birleşik)
**Amaç:** Mevcut live-reload pipeline'ının gerçekten uçtan uca çalıştığını kanıtla.

Adımlar:
1. Unity Editor'da `RIMA → Live Tool → Bake Asset Registry` çalıştır
2. `Assets/Resources/Live/RuntimeAssetRegistry.asset` oluştuğunu doğrula
3. `WriteCurrent()` bir test sahnesinde çağır, `StreamingAssets/live/room_current.json` incele
4. Development Build al, Game.exe çalıştır, JSON değiştir, reload log'u doğrula
5. EditMode smoke testleri koş (`LiveToolSmokeTests.cs` — 23 test)

**Efor:** 1-2 saat. **Risk:** Çok düşük. **Demo değeri:** Yüksek — "floor+prop live reload ÇALIŞIYOR" kanıtlanır.

### Item B — Cliff live-reload fix (#1)
**Amaç:** Bilinen tek "floor çalışıyor, cliff çalışmıyor" asimetrisi kapatılır.

Değişiklikler (tümü C# dosyası, Unity açık gerekmez):
- `RoomLayoutSerializer.cs`: `CliffCell` iç sınıfına `public string tile_guid;` ekle + cliff tilemap için `AddTiles()` yerine `AddCliffTiles()` yaz (FloorTile yerine CliffCell üretsin)
- `RoomLayoutData.cs`: `CliffCellData` sınıfına `public string tile_guid;` ekle
- `LiveRoomReloader.cs`: `ApplyCliffTiles()` no-op yorumunu kaldır, floor pipeline ile aynı mantığı uygula (`_cliffTilemap.ClearAllTiles()` + `SetTile` loop, `ResolveTile(ct.tile_guid)`)

**Efor:** 2-3 saat. **Risk:** Düşük — schema additive (mevcut JSON'lar çalışmaya devam eder, yeni field null = boş liste, mevcut davranış). **Demo değeri:** Yüksek — cliff tile değişikliği artık Game.exe'ye yansır, demo room editing complete.

**Öneri:** A → B sırası. A ile pipeline çalışır kanıtlanır, B ile cliff kapanır. İki item birlikte tamamlanırsa gece sabahı **floor+cliff+prop live-reload doğrulanmış** olur — T3 Editor-hibrit path'in "done" noktası.

---

*Dosya: STAGING/LIVE_EDITOR_GAP_S114.md — kod yazılmadı, sadece okuma + analiz.*
