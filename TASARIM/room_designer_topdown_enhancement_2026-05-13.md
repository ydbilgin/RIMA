---
status: LOCKED
faz: 1
tarih: 2026-05-13
ozet: "Room Designer E1 TileImportWizard + E2 PixelPerfectCanvas planı"
---
# Room Designer Top-Down Enhancement — Draft Plan (Opus)
**Tarih:** 2026-05-13 (S60)
**Status:** DRAFT for Codex review
**Trigger:** User 2026-05-13 — "room design durumunu top downa ozel olarak gelistirmeye devam et"

---

## Mevcut Durum (F2 Complete)

`Assets/Editor/RoomDesigner/` icerigi:
- `RimaRoomDesignerWindow.cs` — ana editor window
- `IRoomDesignerContext.cs` — context interface
- `FloorVariantPainter.cs` — domain-warped Perlin 3-katman bake
- `WallAutoConnect.cs` — 4-bit NSEW mask, 8 tip
- `RoomMetadataPanel.cs` — Biome/Reseed/Preview/Override toggle
- `Brushes/` — Stamp/Eraser/Picker/Bucket + BrushController
- `Canvas/` — paint canvas logic
- `Palette/` — tile palette UI
- `SaveLoad/` — RoomSaver (BiomeType enum-driven)
- `UI/` — UXML/USS

Onceki S58'de 2.5D refactor pending vardi → S59'da REVOKED, pure 2D top-down geri.

Mevcut hal: **F2 zaten 2D top-down icin yapildi**, ek yeni mimari gerekmez. Sadece top-down ozel iyilestirmeler + tile asset ureti bekleyen darbogaz.

---

## Top-Down Spesifik Enhancement Plani

### High Priority (v1 Production Enabler)

#### E1 — Tile Import Wizard (Codex implementation candidate)
PixelLab output PNG'leri (32x32 floor/wall/decal) Unity Tile asset'lerine donusturen one-click wizard:
- PixelLab raw PNG → Sprite Editor (PPU=64, Point filter, no compression, no mipmap)
- Sprite → Tile asset (single sprite tile, no Rule Tile yet)
- Rule Tile autoconnect mapping (4-bit NSEW → 8 wall variants)
- Batch import folder (ornegin `PIXELLAB_OUTPUTS/floors/F1/`) → `Assets/Art/Tiles/F1/Floor/` automatic
- Naming convention enforce: `F1_floor_v01.png` → `F1_floor_v01.asset`

**Sebep:** Tile asset production = current bottleneck (Karar #75 — PixelLab Map Tools yasak, single tile only). Manuel import 16 floor + 8 wall + decal = ~30dk per biome × 3 biome = ~1.5 saat tekrarli is. Wizard = 5 dakika.

**Codex scope:** EditorWindow + AssetDatabase + Sprite import settings. ~200 satir C#.

#### E2 — Pixel-Perfect Canvas Preview (Codex implementation candidate)
Designer canvas'in PPU=64 + 32x32 grid'i pixel-perfect render etmesi. Mevcut canvas grid generic; top-down kontrol icin:
- Grid major lines per 8 cell (256px) — orienteering icin
- Grid minor lines per cell (32px) — snap visualization
- Pixel snap zorunlu (no sub-pixel rendering)
- Camera preview overlay: "in-game" gorulus simulasyonu (Pixel Perfect Camera ile)

**Sebep:** Designer'da urettigin oda in-game'de farkli gozukursa workflow trust kaybi. PPC overlay = WYSIWYG.

**Codex scope:** Canvas component genisleme + camera setup. ~120 satir.

### Medium Priority (Faz 1-2 Polish)

#### E3 — Brush Size Cycle Hotkey
Stamp/Eraser brush'ina size dimension ekle: 1x1 (default), 2x2, 3x3, 5x5. Hotkey `[` ve `]` (decrease/increase). Wall painting batch hizlandirir.

#### E4 — Layer Visibility Toggle
F/W/D layer'lari ayri ayri hide/show. Decals layer'ini gecici gizleyerek floor paint, sonra geri ac. Hotkey `1`/`2`/`3` toggle (Shift = solo, default = hide).

#### E5 — AutoConnect Dry-Run Preview
Wall fircasinda her stroke oncesi AutoConnect'in hangi variant'i sececeğini real-time gosteren overlay (hover preview). User pre-stroke gorur, surprise yok.

### Low Priority (F3 Roadmap)

#### E6 — Object Library (T10)
ScriptableObject-driven obstacle/decoration palette. Drag-drop place. Mevcut F3 plan'da var.

#### E7 — Template Wizard (T11)
Room template'leri (Combat/Elite/Rest/Shop) one-click instantiate. Pre-laid floor + wall border + safe spawn zones.

#### E8 — RoomSaver Export (T12)
Mevcut RoomSaver.Save() metadata + thumbnail PNG export ekler. RoomRegistry pool selection kolaylasir.

### Future (Faz 2-3+)

#### E9 — AI Panel + MCP Bridge (T9, AI-assisted room generation)
STAGING/mcp_requests/ → responses/ async pipeline. Mevcut Karar #61 (Hades-style) ile uyumlu: AI room layout suggestion, designer override eder. Detayli plan post-pilot.

---

## Recommendation

**Faz 1 implement: E1 + E2** (production enabler — tile asset bottleneck + WYSIWYG canvas)

**Faz 1 polish: E3 + E4 + E5** (workflow speed-up)

**F3 sira: E6, E7, E8**

**Faz 2-3 sonra: E9 AI panel**

---

## Dispatch Plan

**E1 + E2 = single Codex dispatch** (2 enhancement, ~320 satir C#):
- Allowed paths: `Assets/Editor/RoomDesigner/SaveLoad/` (yeni TileImportWizard.cs), `Assets/Editor/RoomDesigner/Canvas/` (canvas extension), `Assets/Editor/RoomDesigner/RIMA.RoomDesigner.Editor.asmdef` (eger asmdef ref ekleniyorsa)
- Test scope: EditMode test ekle TileImportWizard + canvas grid logic
- Prompt file: `.claude/codex_room_designer_E1E2_prompt.txt` (yazilacak)

**Dispatch metodu:** Direct Bash + cx.cmd (per new sub-agent token economy kurali, rima-codex spawn etme).

**Asset bekleme noktasi:** E1 wizard hazir, ama tile PNG yok → wizard test edemiyor. Mock PNG'lerle test PASS, gercek tile geldiginde re-test.

---

## Yeni Karar Onerisi (eger Codex onaylanir)

Bu plan icin yeni LOCKED karar GEREK YOK — F2 mimari KEEP, sadece F2 polish + tile import infrastructure. Mevcut Karar #75 (Map Tools yasak, single tile) ile uyumlu.

---

## Sonraki Adimlar (Codex review sonrasi)

1. Codex onayi → `.claude/codex_room_designer_E1E2_prompt.txt` yaz.
2. cx dispatch (background): TileImportWizard + Canvas pixel-perfect.
3. Unity refresh + tests pass verify.
4. CURRENT_STATUS update.
5. E3-E5 polish dispatch sirayla.
6. Tile asset uretimi geldikten sonra E1 wizard end-to-end test.

