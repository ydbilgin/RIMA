# D3: Painter Mode Tabs + L1-L6 Sub-Filter

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: gerekirse `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`
Direct-read sadece: CURRENT_STATUS.md, .claude/PROJECT_RULES.md, kod, STAGING, memory files.

## Amaç
RimaRoomPainterWindow'a 4 mode tab (Tile/Cliff/Decor/Object) + L1-L6 sub-filter ekle. 4-surface visibility HARD rule (toolbar+statusbar+inspector+menu). D2'de eklenen RoomLayer/AssetCategory enum'ları consume et.

## Bağlam
- D2 tamamlandı: Enums.cs RoomLayer + AssetCategory (CliffFaceDecor/WallBlocker/GameplayObject), RoomPainterPhysicsRules +RoomLayer field + 8 keyword, 2 sorting layer LIVE, 33 prefab backfill
- Day 5a 3-pane LIVE: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (679 LOC) + `Editor/RoomPainter/Preview/RoomPainterPreviewPane.cs`
- Spec ref: `STAGING/RIMA_LIVE_TOOL_DECISION.md` Section 1.4 (T3 path) + `STAGING/UNIFIED_PAINTER_PLAN.md` Section 1 (4-mode toolbar tasarım)
- HARD rule `feedback_tool_visibility_4_surfaces`: her mode 4 surface (toolbar + statusbar + inspector + menu)

## İş kalemleri

### 1. RoomPainterMode enum (yeni)
- Yeni file VEYA mevcut enum dosyaya inject. Önerim: `Assets/Editor/RoomPainter/RoomPainterMode.cs` (küçük dosya, painter-scoped)
- Enum: `{ Tile, Cliff, Decor, Object }` (4 entry)
- Her mode için: hotkey (1/2/3/4), display name, icon name (Unity built-in `EditorGUIUtility.IconContent` string), brush behavior summary

### 2. RimaRoomPainterWindow.cs — mode toolbar
- File: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (679 LOC, EXTEND)
- Toolbar üst sıraya 4 toggle button ekle (Tile/Cliff/Decor/Object), aktif olan highlight
- `_currentMode` private field, hotkey 1-4 keyboard handle `OnGUI` veya `keyDown` event
- Mode değişince:
  - PreviewPane overlay update (cell highlight rengi değişir mode'a göre)
  - Palette filter L1-L6 default mode'a göre set olur:
    - Tile mode → L1 (Floor)
    - Cliff mode → L2+L3 (Cliff base + face)
    - Decor mode → L4+L5 (Walkable + Wall)
    - Object mode → L6 (Gameplay)
  - Inspector pane mode-specific section göster

### 3. Statusbar mode label
- Mevcut statusbar Day 5a'da var (`RimaRoomPainterWindow.cs:519-520` ref'i)
- Sol tarafa "Mode: <current>" ekle (renkli — Tile=mavi, Cliff=gri, Decor=yeşil, Object=altın)
- Sağ tarafta mevcut hitbox edit status korunur

### 4. L1-L6 sub-filter dropdown
- Toolbar 2. sıraya (mode toolbar altına) layer filter dropdown
- Default mode'a göre dolu gelir (yukarıdaki mapping)
- "All" seçeneği + 6 layer toggle (multi-select)
- Palette panel'i layer filter'a göre asset'leri gösterir/gizler

### 5. Palette filter integration
- File: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` veya palette panel sub-file
- Palette'te her asset için RoomLayer detect → filter'a uymayan asset'leri gizle/gri yap
- AssetCategory enum'dan RoomLayer map (D2 backfill ile prefab metadata'ya yazıldı)
- Filter uygulama RoomPainterPhysicsRules.cs keyword + manuel metadata fallback

### 6. Inspector mode-specific section
- File: `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` veya yeni sub-section
- Mode'a göre Inspector altında farklı section göster:
  - Tile mode → Floor tile + sub-variant selector
  - Cliff mode → Cliff variant dropdown (DirectionalCliffTile sprites array preview), Regenerate button, ManualPainted/Override count
  - Decor mode → Walkable/Wall sub-filter, sortingOrder override
  - Object mode → Trigger collider settings, interaction range

### 7. Menu cleanup (4-surface menu surface)
- `RIMA > Room Painter Tools/` submenu altına 4 entry:
  - `Mode/Tile (1)`
  - `Mode/Cliff (2)`
  - `Mode/Decor (3)`
  - `Mode/Object (4)`
- Mevcut `Toggle Visual Collider Edit (SceneView)` korunur

## Dosyalar (scope)
- `Assets/Editor/RoomPainter/RoomPainterMode.cs` (NEW, ~30 LOC)
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (EXTEND, ~150 LOC eklenecek)
- `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` (EXTEND, ~80 LOC)
- (Opsiyonel) `Assets/Editor/RoomPainter/Palette/RoomPainterPaletteFilter.cs` (NEW, ~50 LOC)
- Toplam ~300 LOC

## YASAK
- Mevcut Day 5a 3-pane layout BOZMA (palette/inspector/preview oranları korunur)
- BrushExecutorRouter (RimaVisualMapEditorWindow motor) D3'te TAŞINMAZ (D5 scope)
- AssetPostprocessor pipeline değişiklik (D2'de zaten yapıldı)
- mode mapping kullanıcı verbatim 6-layer kapatma — sadece default presetler, multi-select açık kalmalı
- Yeni .cs dosya yazılırsa `mcp__UnityMCP__refresh_unity scope=all mode=force` ZORUNLU (HARD `feedback-new-cs-needs-scope-all-refresh`)
- Input.GetKey* YASAK → InputSystem.Keyboard.current (HARD `feedback-input-system-active-keyboard-current`)

## Verify
- UnityMCP: `refresh_unity scope=all mode=force` + `read_console` → 0 error / 0 warning
- RimaRoomPainterWindow aç (`RIMA > Room Painter` menu)
- 4 mode toggle button toolbar'da görünür
- Hotkey 1/2/3/4 mode switch çalışır (keyboard focus painter window'da olunca)
- Statusbar "Mode: Tile" gibi label rengiyle görünür
- Mode değiştirince palette filter otomatik default L'lere snap
- Multi-select L1-L6 toggle çalışır (sub-filter dropdown)
- Inspector altta mode-specific section value değişir

## Output
- `STAGING/D3_PAINTER_MODE_TABS_DONE.md` — değişen dosya envanteri + verify checklist + compile durum + screenshot path (UnityMCP capture varsa)

## Süre
~45-60 dk Sonnet bg. Background dispatch.

BLOCKED durumu: (a) RimaRoomPainterWindow.cs içinde mevcut toolbar layout bilinmeyen pattern kullanıyor → mevcut toolbar pattern'ini reuse et, yeni toolbar ekleme. (b) AssetCategory metadata prefab'larda nerede saklı (D2 backfill nereye yazdı) belirsiz → Codex review için ayrı task aç, partial implement YASAK.
