# EXEC 1 — Menu Consolidation (CX, mechanical, surgical)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — ONLY the `[MenuItem("...")]` path strings + the 2 ExecuteMenuItem/test literals listed below; touch NOTHING else (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
RIMA menüsü ~40 dağınık giriş. Oda-authoring kodu UnifiedMapDesigner'a birleşti AMA menü hâlâ kalabalık. Hedef: **sadece `[MenuItem("...")]` path string'lerini** değiştirip menüyü topla. Fonksiyon mantığı DEĞİŞMEZ. Çıktıyı `CODEX_DONE.md` → `## EXEC1 MENU CONSOLIDATION` başlığına yaz.

## KURALLAR
- `_archive~` / `_archive_S73~` / `_Archive_iso_pre_topdown~` / `pre_v2_editor` / `pre_s106*` altındaki MenuItem'lara DOKUNMA (compile-dışı).
- Kısayol suffix (` _F5`, ` _F6`) KORU.
- `priority = N` ve validate `, true` argümanlarını KORU.
- Validate overload'u olan girişlerde (DungeonSetup, CombatTestSetup) HEM ana HEM validate MenuItem'ı AYNI yeni path'e güncelle.
- ZATEN doğru yerde (DOKUNMA): UnifiedMapDesigner.cs:37 `RIMA/Map Designer`, RimaDevShortcuts.cs:10/27 `RIMA/Play Arena _F5` + `RIMA/Stop Play _F6`, RimaRoomPainterWindow.cs:133 `RIMA/Legacy/Room Painter (classic)`, RimaVisualMapEditorWindow.cs:42 `RIMA/Legacy/Visual Map Designer`, MapDesignerBrushWindow.cs:43 `RIMA/Legacy/Map Designer Brush Tool`.

## TAŞI → `RIMA/Legacy/...` (oda-authoring alt-araçları)
Her birinde `RIMA/` prefix'ini `RIMA/Legacy/` yap, kalan alt-yolu KORU:
- Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs:141 `RIMA/Room Painter Tools/Toggle Visual Collider Edit (SceneView)`
- RimaRoomPainterWindow.cs:149 `RIMA/Room Painter Tools/Mode/Tile (1)`
- RimaRoomPainterWindow.cs:158 `RIMA/Room Painter Tools/Mode/Cliff (2)`
- RimaRoomPainterWindow.cs:167 `RIMA/Room Painter Tools/Mode/Decor (3)`
- RimaRoomPainterWindow.cs:176 `RIMA/Room Painter Tools/Mode/Object (4)`
- RimaRoomPainterWindow.cs:185 `RIMA/Room Painter Tools/Generate Metadata for All Sprites`
- Assets/Editor/RoomPainter/LiveTool/LiveToolLauncher.cs:155 `RIMA/Live Tool/Launch Live Tool`
- LiveToolLauncher.cs:158 `RIMA/Live Tool/Stop Live Tool`
- LiveToolLauncher.cs:161 `RIMA/Live Tool/Build Both Targets`
- Assets/Editor/RoomPainter/LiveTool/LiveToolPaletteWindow.cs:69 `RIMA/Live Tool/Palette`
- Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs:44 `RIMA/Live Tool/Bake Asset Registry`
- Assets/Editor/Build/LiveToolBuildProcessor.cs:335 `RIMA/Live Tool/Build Tool (RIMA_Tool.exe)`
- LiveToolBuildProcessor.cs:338 `RIMA/Live Tool/Build Game (RIMA.exe)`
- LiveToolBuildProcessor.cs:341 `RIMA/Live Tool/Build Both (Tool + Game)`
- LiveToolBuildProcessor.cs:349 `RIMA/Live Tool/Enable Tool Define (Editor)`
- Assets/Editor/MapDesigner/PatchAtlasSpriteAtlasBuilder.cs:14 `RIMA/MapDesigner/Build SpriteAtlas from PatchAtlas`
- Assets/Editor/MapDesigner/Brush/DependencyReportGenerator.cs:14 `RIMA/MapDesigner/Brush/Generate Dependency Report`
- Assets/Editor/MapDesigner/SampleRoomLibraryGenerator.cs:18 → menü sabiti `MenuPath` (dosyada `const string MenuPath = "RIMA/MapDesigner/Brush/Generate Sample Library v1"`); bu sabiti `RIMA/Legacy/MapDesigner/Brush/Generate Sample Library v1` yap. **AYRICA `Assets/Tests/EditMode/Editor/SampleRoomLibraryGeneratorTests.cs:12`'deki `MenuPath` literalini AYNI yeni değere güncelle** (test ExecuteMenuItem ile çalıştırıyor, satır 76).
- Assets/Scripts/MapDesigner/Brush/Editor/SliceTemplateFactory.cs:13 `RIMA/Brush/Create Default Slice Templates`
- Assets/Scripts/MapDesigner/Brush/Editor/BrushVariantPreviewWindow.cs:15 `RIMA/Brush/Variant Preview`
- Assets/Scripts/MapDesigner/Brush/Editor/BrushAtlasImportMenu.cs:12 `RIMA/Brush/Import Atlas...`
- BrushAtlasImportMenu.cs:48 `RIMA/Brush/Validate Sorting Layers`
- Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateMenu.cs:12 `RIMA/Room/Save Selection as Room Template...`
- RoomTemplateMenu.cs:31 `RIMA/Room/Load Template Into Scene...`
- RoomTemplateMenu.cs:48 `RIMA/Room/Validate Template`
- RoomTemplateMenu.cs:64 `RIMA/Room/Validate Bank`

## 🔴 ExecuteMenuItem YAN-ETKİ (taşıma ile AYNI anda güncelle)
- `Assets/Editor/RoomPainter/LiveTool/LiveToolPaletteWindow.cs:229` → `EditorApplication.ExecuteMenuItem("RIMA/Live Tool/Bake Asset Registry")` çağrısını `"RIMA/Legacy/Live Tool/Bake Asset Registry"` yap (yukarıda RuntimeAssetRegistryBaker.cs:44'ü Legacy'ye taşıdık). Ayrıca aynı dosyada :220 civarı eski path'i GÖSTEREN bir string varsa onu da güncelle.

## TAŞI → `RIMA/Utilities/...` (saf yardımcılar)
- Assets/Editor/TileImport/PixelLabWangImporter.cs:33 `RIMA/PixelLab Wang Tileset Importer` → `RIMA/Utilities/PixelLab Wang Tileset Importer`
- Assets/Editor/TileImport/PixelLabPngSheetImporter.cs:19 `RIMA/PixelLab PNG Sheet Importer` → `RIMA/Utilities/PixelLab PNG Sheet Importer`
- Assets/Editor/Tools/SpritePivotBatchFix.cs:14 `RIMA/Tools/Fix All Sprite Pivots` → `RIMA/Utilities/Fix All Sprite Pivots`
- Assets/Editor/Tools/ApplySeloutMaterial.cs:9 `RIMA/Tools/Apply Selout to All Characters` → `RIMA/Utilities/Apply Selout to All Characters`
- Assets/Editor/Skills/SkillIconRegistryBuilder.cs:44 `RIMA/Skills/Rebuild Icon Registry` → `RIMA/Utilities/Rebuild Skill Icon Registry`
- Assets/Editor/DevTools/GameViewSetup.cs:25 `RIMA/Setup Game View (1080p + Maximize)` → `RIMA/Utilities/Setup Game View (1080p + Maximize)`
- Assets/Editor/DevTools/CreateDepthBandSOs.cs:10 `RIMA/Create DepthBand SOs` → `RIMA/Utilities/Create DepthBand SOs`
- Assets/Editor/DevTools/ClearTilemaps.cs:11 `RIMA/Clear All Tilemap Tiles` → `RIMA/Utilities/Clear All Tilemap Tiles`
- Assets/Scripts/Editor/RoomPreviewPanel.cs:13 `RIMA/Scene View/Room Preview Panel` → `RIMA/Utilities/Room Preview Panel`
- Assets/Scripts/Map/RoomBuilder.cs:74 `RIMA/3. Build Room` → `RIMA/Utilities/Build Room`
- RoomBuilder.cs:77 `RIMA/3b. Build Room (New Seed)` → `RIMA/Utilities/Build Room (New Seed)`
- Assets/Scripts/Editor/DungeonSetup.cs:28 `RIMA/4. Dungeon Wiring` → `RIMA/Utilities/Dungeon Wiring` (AYRICA :125 validate `, true` overload AYNI yeni path)
- Assets/Scripts/Map/ObstaclePrefabBuilder.cs:11 `RIMA/4. Create Obstacle Prefabs` → `RIMA/Utilities/Create Obstacle Prefabs`
- Assets/Scripts/Editor/CombatTestSetup.cs:13 `RIMA/Combat Test Setup` → `RIMA/Utilities/Combat Test Setup` (AYRICA :152 validate `, true` overload AYNI yeni path)

## DoD (Definition of Done)
1. Yukarıdaki TÜM MenuItem path'leri + SampleRoomLibraryGenerator MenuPath sabiti + test literali + LiveToolPaletteWindow ExecuteMenuItem çağrısı güncellendi.
2. Validate overload'lar (DungeonSetup:125, CombatTestSetup:152) ana giriş ile AYNI yeni path.
3. ÖN-DOĞRULAMA: işlem sonrası `grep -rn '\[MenuItem("RIMA/' Assets --include=*.cs | grep -v _archive | grep -vE 'RIMA/(Legacy|Utilities|Map Designer|Play|Stop)'` çıktısı BOŞ olmalı (taşınmamış stray kalmasın). Bu komutu çalıştır, sonucu raporda göster.
4. Değişen dosya listesi + her dosyada kaç MenuItem güncellendi.
5. Compile: Unity AÇIK — sen build/execute_menu_item DENEME. "compile bekleniyor, Opus doğrulayacak" de.
6. BLOCKED durumları işaretle.
