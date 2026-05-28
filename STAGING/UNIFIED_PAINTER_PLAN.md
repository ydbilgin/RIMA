# UNIFIED PAINTER PLAN — RIMA Editor Tool Konsolidasyon

**Kaynak:** rima-design (Opus) verdict 2026-05-27 gece.
**Status:** Plan onayı bekleniyor. agy + Codex appendix opsiyonel (asmdef audit + LOC confirm için), Opus verdict tek başına karar veriyor.

---

## 1. ANA KARAR — Tek Window, Tab-Mode + 3-Pane

**Verdict:** Yeni window yaratma. **`RimaRoomPainterWindow`'u SHELL kabul et**, diğer painter window'larını bunun **mode tab**'ları altına çek.

### Neden RoomPainter shell?
- Day 5a 3-pane (palette/inspector/preview) LIVE — kullanıcının zaten alıştığı yer.
- AssetPostprocessor + metadata pipeline RoomPainter altında.
- Visual Collider Authoring sistemi RoomPainter'a bağlı (4-surface görünürlük HARD rule referansı).
- LOC: 0 yeni window, 4 mod ekle.

### Pencere şeması (final)

```
┌──────────────────────────────────────────────────────────────────────────┐
│ Folder: [Assets/Sprites/Environment ] [Refresh]  Mode: [Tile][Decor]     │
│ [Cliff][Prefab] | Brush: [□][○][⬡] Size:[64px] | Layer:[Floor▼]          │
├──────────────────────────────────────────────────────────────────────────┤
│ Category filters: [All][Floor][Cliff][Cliff Mount.][Props][Parallax]     │
├────────────┬──────────────────────────────────┬──────────────────────────┤
│ PALETTE    │  PREVIEW PANE (Day 5a LIVE)      │  INSPECTOR               │
│ [thumb]    │  Live SceneView reflection +     │  Selected: cliff_a01     │
│ [thumb]    │  ghost brush preview             │  Physics:    [Wall▼]     │
│ [thumb]    │  (R-rotate, iso-snap)            │  Sorting:    [-10]       │
│ [thumb]    │                                  │  Pivot:      [Top]       │
│ [thumb]    │  Mode-aware overlay:             │  ──                      │
│ [thumb]    │  Tile  → cell highlight          │  Cliff Mode-specific:    │
│            │  Cliff → S/SE/SW void hint       │  [Regenerate] (C)        │
│            │  Prefab→ instance bounds         │  Manual painted: 311     │
│            │                                  │  Manual erased:   42     │
│            │                                  │  [Clear Erased]          │
│            │                                  │  [Clear Painted]         │
├────────────┴──────────────────────────────────┴──────────────────────────┤
│ STATUS: Mode=Cliff | Asset=cliff_mounting_a01 | Cells=311 | Hover=(3,-7) │
└──────────────────────────────────────────────────────────────────────────┘
```

### Mode'lar (toolbar tab)

| Mode | Hotkey | Sol-tık | Alt+Sol-tık | Sağ-tık |
|---|---|---|---|---|
| **Tile** | 1 | Paint floor tile | Erase tile | Pick |
| **Decor** | 2 | Place decor prop | Delete instance | Pick |
| **Cliff** | 3 | `AddManualPainted` + SetTile | `AddManualOverride` + erase | Pick |
| **Prefab** | 4 | Instantiate prefab | Delete | Pick |
| **Erase** | E | Universal erase (delegate to active layer) | — | — |
| **Pick** | P | Eyedropper → palette select | — | — |
| **Box-Select** | B | Drag region select | — | — |

**HARD rule:** her mode 4-surface visible (toolbar button + statusbar mode label + inspector mode panel + menu item). `feedback_tool_visibility_4_surfaces`.

---

## 2. Konsolidasyon Matrisi

| Mevcut tool | Yeni durum | Action |
|---|---|---|
| **RimaRoomPainterWindow** | **ANA SHELL** — tab toolbar eklendi | `RimaRoomPainterWindow.cs` extend |
| **RimaVisualMapEditorWindow** | **Tile Mode'a merge** — BrushExecutorRouter motor RoomPainter Tile mode'undan çağrılır | Window deprecate ("Legacy" submenu) |
| **MapDesignerBrushWindow** | **DEPRECATE** — palette/settings/scene-tooling pattern'leri RoomPainter palette panel'ine adapte | Window archive (~suffix). Hotkey handler taşı. |
| **BlueprintPainterWindow** | **OPEN Q** — kullanım frekansı düşükse legacy; yüksekse Tile sub-mode | Kullanıcıya sor. Default: archive. |
| **AssetPackBrowserWindow** | **Inline panel** — RoomPainter palette panel'inin üstüne "Asset Pack" dropdown | Window archive, panel kodu inline |
| **CliffAutoPlacerEditor** (CustomEditor) | **Inspector pane'e mode-specific extend** — Cliff mode aktifken "Cliff Settings" expandable section | CustomEditor kalır + RoomPainter Cliff mode reflection ile aynı UI'yi çizer |
| **TileImportWizard** | **DOKUNULMAZ** — workflow ayrı, painter scope dışı | No change |

---

## 3. Cliff Sorunları — Root Cause + Fix Plan

### 3.1 "cliff_mounting görünmüyor"

`CliffAutoPlacer.cs:14-17` envanteri: `cliffTile` **TEK** TileBase reference — yani sadece 1 cliff variant.

**Root cause adayları (öncelik sırası):**
1. **A — Slot ataması yok:** `CliffAutoPlacer.cliffTile` Inspector'da hala eski cliff tile'ı gösteriyor. `cliff_mounting` üretildi ama wire edilmedi.
2. **B — Asset type mismatch:** `cliff_mounting` Prefab olarak import edildi ama `cliffTile` `TileBase` bekliyor.
3. **C — Palette category filter:** RoomPainter palette `RoomLayer.Cliff` filter'ı kullanıyor, `cliff_mounting` metadata'sı yanlış kategori.
4. **D — Sub-variant naming:** Palette'te görünüyor ama Regenerate tek field kullandığı için variant rotation desteği yok.

**Fix path:**
- **Fix 1 (kısa vade):** Cliff mode'a "Active Cliff Tile" dropdown ekle, `CliffAutoPlacer.cliffTile` field'ını oradan set et. Tek slot, görünür kontrol.
- **Fix 2 (orta vade):** `cliffTile` → `cliffTiles` (`List<TileBase>`) + variant weights veya cell-position hash. cliff_mounting ayrı variant. Multi-variant cliff.
- **Fix 3 (uzun vade):** Cliff variant'larını sub-mode olarak ayır (Cliff/Cliff_Mounting/Cliff_Cap). RuleTile-style neighbor lookup ileride.

**Önerim:** Fix 1 hemen (Day 4), Fix 2 takiben.

### 3.2 "Hala havada boş cliffler var silemiyorum"

**Root cause adayları:**
1. **A — Erase UI surface yok:** PAINT hook `VisualEditorScenePainter.cs:546` var, ERASE hook UI'de keşfedilebilir değil. `AddManualOverride` çağrılmıyor.
2. **B — Whitelist persist:** Stray cliff `ManualPaintedCells` whitelist'te → her Regenerate geri getiriyor. `ClearAllTiles` + `UnionWith(ManualPaintedCells)` sonra yeniden ekliyor.
3. **C — HasTile guard:** `if (cliffTilemap.HasTile(cell)) continue;` zaten varsa skip. ClearAllTiles yapılmadan paint edilen "havada cliff"ler Regenerate'siz kalır.

**Fix path:**
- **Cliff Mode UI'de** Alt+Sol-tık = `AddManualOverride(cell)` + `cliffTilemap.SetTile(cell, null)` + `RemoveManualPainted(cell)` (whitelist'ten de sil, yoksa Regenerate geri getirir). Tek tıkla kalıcı erase.
- **Status bar'a "Erased: N" göster.**
- **Inspector pane'de "Clear Manual Painted" + "Clear Manual Override" butonlar** — şu an sadece `[ContextMenu]` (gizli).
- **Hover SceneView indicator:** Cliff Mode'da mouse-over cell `ManualPaintedCells`/`ManualOverrideCells` durumu cell tint ile (yeşil=painted, kırmızı=erased, gri=auto).

### 3.3 Step-by-step kullanıcı akışı

**Senaryo: "Havada boş cliff sil"**
1. `RIMA > Room Painter` aç.
2. Toolbar'dan **Cliff** mode (hotkey **3**).
3. SceneView'de havada cliff cell'e mouse hover → Inspector pane cell durumu (auto/painted/erased).
4. **Alt + Sol-tık** → cell silinir + blacklist'e eklenir.
5. Statusbar "Erased: 1".
6. **C** (Regenerate hotkey) → otomatik cliff yeniden hesaplanır, silinen cell geri gelmez.

**Senaryo: "cliff_mounting kullan"**
1. RoomPainter → Cliff mode (3).
2. Palette Category filter `[Cliff Mount.]` seç.
3. Thumb'tan `cliff_mounting_a01` seç → Inspector "Active Cliff Tile: cliff_mounting_a01".
4. SceneView cliff cell'e **Sol-tık** → SetTile o variant + `AddManualPainted` whitelist.
5. C → Regenerate. cliff_mounting boyanan cell'lerde kalır, void-neighbor floor cell'leri default `cliffTile` ile dolar.

---

## 4. UI/UX Prensipleri

1. **4-surface visibility** — her mode için: toolbar + statusbar + inspector + menu.
2. **Tek-pencere disiplini** — diğer painter window'lar menüden tamamen kaldırılır. `RIMA > Legacy > ` altında erişilebilir.
3. **Hotkey discovery overlay** — `?` veya `F1` → SceneView'de transparan overlay aktif hotkey'leri listeler.
4. **Mode switch keyboard** — 1/2/3/4 tab, E/P/B universal.
5. **Preview Pane LIVE korunur** — Day 5a yatırımı kayıp olmasın.
6. **Undo/Redo Unity native** — `Undo.RecordObject` cliff regenerate öncesi + her PAINT/ERASE öncesi.
7. **Auto-load default assets** — `RimaVisualMapEditorWindow.AutoLoadDefaultAssets()` pattern'i RoomPainter'a taşı.
8. **Cliff Mode özel SceneView gizmo** — S/SE/SW void cell'leri kırmızı X ile gösterir.

---

## 5. Phase Plan (Day breakdown)

| Day | İçerik | Dispatcher | LOC est. |
|---|---|---|---|
| **D1** | Plan onayı + (opsiyonel) Codex asmdef audit + agy UX research | Orchestrator | 0 (plan) |
| **D2** | RoomPainter toolbar mode tab'leri (4 button + statusbar label + hotkey 1-4) + AssetPackBrowser inline panel | Sonnet | ~250 |
| **D3** | Tile Mode SceneView entegrasyonu — BrushExecutorRouter motor RoomPainter Tile mode'undan çağrılır, ghost preview + R-rotate korunur | Sonnet + Codex review | ~400 |
| **D4** | Cliff Mode UI fix: Active Cliff Tile dropdown (cliff_mounting wire), Alt+click erase, Inspector "Clear Manual Painted/Override" butonları, hover indicator | Sonnet | ~300 |
| **D5** | Menu cleanup — eski painter MenuItem'ları "RIMA > Legacy > " altına | Sonnet + lint | ~50 |
| **D6** | Hotkey discovery overlay (`?`), polish, smoke test | Sonnet + agy QA | ~150 |

**Total LOC est:** ~1150.

---

## 6. Open Questions (kullanıcı kararı bekleyen)

1. **Default boot mode** — Tile / Decor / Cliff / Prefab? (Önerim: **Cliff** — şu an aktif sorun orada.)
2. **Tile palette source** — Unity native Tile Palette mi RoomPainter custom AssetPalette mi? (Önerim: RoomPainter custom — metadata pipeline orada.)
3. **BlueprintPainterWindow** — tab mı legacy archive mı? (Önerim: Legacy.)
4. **AssetPackBrowserWindow** — inline panel mi opsiyonel detach window mi? (Önerim: **inline** + "Open in separate window" küçük button.)
5. **"Havada cliff" erase pattern** — Alt+click / separate Erase Mode (E) / her ikisi? (Önerim: **her ikisi**.)
6. **Cliff variant multi-tile** — Fix 1 (tek slot dropdown) mı Fix 2 (List + variants) mi Day 4'te? (Önerim: **Fix 2** — cliff_mounting'in asıl çözümü.)

---

## 7. Risk & Rollback

| Risk | Mitigation |
|---|---|
| Day 5a preview pane regress | Day 2-6 boyunca preview pane smoke test her dispatch sonrası |
| BrushExecutorRouter ghost preview davranışı bozulur | Codex review her Tile Mode commit sonrası (cx_dispatch effort xhigh) |
| Menü temizliği başka skill/automation kırar | D5 öncesi `Grep` ile menu path string referansları proje genelinde audit |
| cliff_mounting Fix 2 (`List<TileBase>`) `[Serializable]` migration | Eski scene'lerdeki `cliffTile` field default first element olarak migrate |
| User "eski painter geri istiyorum" | RIMA > Legacy altında eski menu item'lar korunur (silinmez) |

---

## 8. Conflicts with Locked Rules

| Rule | Status |
|---|---|
| `feedback_tool_visibility_4_surfaces` | ✅ UYUMLU |
| `feedback_2track_gameplay_decor_strategy` | ✅ UYUMLU (Track A combat/cliff hitbox, Track B decor) |
| `project_s110_late_collider_visible_menu_clean` | ✅ UYUMLU (menü temizliği hedefine paralel) |
| `feedback_orchestrator_delegate_dont_do_yourself` | ✅ Day 2-6 Sonnet impl + Codex review delege |
| `feedback_no_pixellab_night_autonomous` | ✅ Bu plan asset gen yapmıyor |

---

## 9. Relevant File Paths (impl scope)

- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (shell extend)
- `Assets/Editor/MapDesigner/VisualEditor/RimaVisualMapEditorWindow.cs` (Tile mode motor source)
- `Assets/Editor/MapDesigner/Brush/MapDesignerBrushWindow.cs` (palette pattern source)
- `Assets/Scripts/Environment/CliffAutoPlacer.cs` (Fix 2 — `List<TileBase>` migration target)
- `Assets/Editor/Environment/CliffAutoPlacerEditor.cs` (Inspector pattern, Cliff mode panel kaynağı)
- `Assets/Editor/MapDesigner/VisualEditor/VisualEditorScenePainter.cs` (line ~546 PAINT hook, ERASE simetrik eklenecek)
- `Assets/Editor/MapDesigner/AssetPackBrowserWindow.cs` (inline panel source)
- `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs` (legacy archive candidate)
- `Assets/Editor/RoomPainter/Preview/RoomPainterPreviewPane.cs` (LIVE Day 5a — preserve)

---

## Orchestrator Next Step
1. (Opsiyonel) agy + Codex appendix dispatch
2. Kullanıcı 6 open question cevapla → Section 6 LOCK
3. Onay sonrası D2-D6 Sonnet impl dispatch sequence (`model: "sonnet"` explicit)
