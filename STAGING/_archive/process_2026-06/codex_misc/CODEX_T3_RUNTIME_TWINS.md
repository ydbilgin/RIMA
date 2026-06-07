ACTIVE RULES: (1) think before coding (2) min code, faithful to spec (3) surgical — write only the 2 files below, into STAGING (NOT Assets/) (4) BLOCKED if unclear.
NLM ACCESS: probably not needed.
Direct-read (authoritative): STAGING/livetool_t3/REVIEW_AND_INTEGRATION.md (§3 asmdef, §5 porting plan — THIS IS THE SPEC), plus the real sources it cites:
  - Assets/Editor/RoomPainter/LiveTool/RuntimeBrushPalette.cs (C6 source to copy)
  - Assets/Editor/RoomPainter/LiveTool/RuntimeColliderHandles.cs (C7 source: reuse math, rewrite view)
  - STAGING/livetool_t3/ToolBootstrap.cs + BrushExecutorRouter.cs (they CALL these twins — match their expected API exactly)
  - Assets/Scripts/Live/RoomLayoutData.cs (ColliderOverrideData{instance_id,size,offset,shape}), Assets/Scripts/Live/RuntimeAssetRegistry.cs (RegistryEntry), Assets/Scripts/RoomPainter/*.cs (ColliderShape, RoomLayer)

# Amaç
Live Editor T3 scaffold'ın derlenememesinin tek sebebi 2 eksik runtime-ikiz. Onları yaz ki batch RIMA.LiveTool assembly'sinde compile-ready olsun. **STAGING'e yaz** (collision-safe; Unity derlemesin; integration sonra rehberli yapılacak).

# DOSYA 1 — C6 runtime twin (MEKANİK, ~30 dk, LOW)
Write `STAGING/livetool_t3/RuntimeBrushPalette.cs`. First line comment: `// TARGET: Assets/Scripts/LiveTool/Palette/RuntimeBrushPalette.cs`
- Kaynak: `Assets/Editor/RoomPainter/LiveTool/RuntimeBrushPalette.cs`'i VERBATIM kopyala.
- Değişiklik: `namespace RIMA.LiveTool` yap; `using UnityEditor;` satırını SİL; **`PaletteMode` enum'unu bu dosyada `RIMA.LiveTool` namespace'ine TAŞI** (review B1: ToolBootstrap+BrushExecutorRouter PaletteMode'u RIMA.LiveTool'da bekliyor).
- Tüm üyeleri koru: ActiveMode/LayerFilter/SearchText/SelectedEntry, SetRegistry/SetMode/SetLayerFilter/ClearLayerFilter/SetSearch/Select/DeselectIfCurrent/GetFiltered/HasEntries/PassesModeFilter.
- Editor kopyası yerinde kalır (hibrit için); bu runtime kopya Tool.exe için. Sıfır Editor API.

# DOSYA 2 — C7 runtime twin (REWRITE, kritik path, ~1 gün, HIGH)
Write `STAGING/livetool_t3/RuntimeColliderHandles.cs`. First line: `// TARGET: Assets/Scripts/LiveTool/Authoring/RuntimeColliderHandles.cs`
Review §5 C7'yi BİREBİR uygula:
- **REUSE (verbatim lift):** ColliderState struct + Stack<ColliderState> depth 32, box 8-handle / circle 2-handle / capsule 2-handle drag-delta math, axis-delta apply, unit-scale, shape resolution. (Editor API'ye dokunmayan saf math.)
- **REWRITE (yeni view + persistence):**
  - Public API ToolBootstrap'ın çağırdığı GİBİ TAM: `Initialize(VisualElement canvas, Camera previewCamera)`, `SetTarget(GameObject, RegistryEntry)`, `Tick(RoomLayoutData)`, `bool Undo()`, `ColliderShape CurrentShape`. (ToolBootstrap.cs'i oku, imzaları doğrula.)
  - 8 handle dot → preview-canvas'ın VisualElement/Image child'ları, her frame `previewCamera.WorldToScreenPoint` ile absolute konum. Hit-test 16px.
  - Outline → dünyada LineRenderer (IMGUI Handles DEĞİL).
  - Input → UXML PointerDown/Move/Up + KeyDownEvent Ctrl+Z (Event.current YOK).
  - Persistence → `RoomLayoutData.collider_overrides`'a yaz (ColliderOverrideData) + ToolBootstrap.RequestSave() çağır. EditorUtility/AssetDatabase YOK.
  - Shape swap → runtime: Destroy(old Collider2D)+AddComponent<Box/Circle/Capsule>. ColliderShapeSwapper'ı KULLANMA. Polygon defer.
  - namespace `RIMA.LiveTool`. `#if RIMA_LIVE_TOOL` guard.

# Kısıt
- SIFIR UnityEditor/Handles/Gizmos/AssetDatabase referansı (Player-build-safe).
- ToolBootstrap + BrushExecutorRouter'ın beklediği imzalarla TAM uyum (onları oku).
- Compile-ready hedefle ama Unity'de TEST EDEMEZSİN (lock) — dotnet build denemen olursa not düş.
- Bitince CODEX_DONE'a: 2 dosya özeti, ToolBootstrap API'siyle uyum nasıl doğrulandı, varsayımlar.
