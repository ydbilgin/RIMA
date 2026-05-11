# Room Designer F2 — UX Kararları (Opus, 2026-05-11)

## Q1: FloorVariantPainter UX — KARAR: (C) On Save + Preview Toggle

Save anında bake → determinizm korunur. Ek olarak sağ panelde "Preview Bake" toggle:
- Toggle ON → FloorVariantPainter.PreviewVariants() canvas'ta görsel gösterir (blueprint'e yazmaz)
- Toggle OFF → RestoreDefault() ile geri alır
- Save → FloorVariantPainter.BakeVariants() çalışır, floorVariantIndex byte[] blueprint'e yazılır
- Aynı seed = aynı sonuç garantisi (preview ve save aynı algoritmayı kullanır)

**Sistemler:** FloorVariantPainter, RoomDesignerCanvas, RoomSaver, TileLibraryPanel

## Q2: WallAutoConnect UX — KARAR: (A) Immediate, 3x3 dirty rect

Her wall tile placement'ta anında 3x3 komşu güncellenir (tüm harita sweep yok).
- StampBrush.OnStrokeEnd → WallAutoConnect.RefreshNeighborhood(touchedCells)
- EraserBrush da aynı hook'u tetikler
- overrideVariantIndex = true olan hücreler atlanır (manuel override korunur)

**Sistemler:** StampBrush, EraserBrush, WallAutoConnect, RoomBlueprint.overrideVariantIndex

## Q3: Metadata Panel placement — KARAR: (A) Right panel, her zaman görünür

Sağ panelde brush toolbar'ın altında, always visible.
- Compact layout: roomId + biome dropdown yan yana (Row 1)
- gateCount + noiseSeed + Reseed butonu (Row 2)
- "Preview Floor" toggle (Row 3)
- İlk save'den sonra auto-collapse (TODO F2 polish)

**Sistemler:** RoomMetadataPanel, RimaRoomDesignerWindow, RoomDesignerWindow.uss

## Q4: F2 demo hedefi — "Reseed Loop"

30 saniyelik demo:
1. Wall corridor stamp → otomatik isometric köşe/düz bağlantı (anında)
2. Floor region bucket-fill → Preview Bake toggle → organik zemin variety (no tiling)
3. Reseed → farklı dağılım → tekrar Reseed → farklı yine
4. Save → PlayMode → oda yürünebilir, deterministik

**Kritik:** Reseed end-to-end (FloorVariantPainter + noiseSeed + canvas refresh) F2'nin en öncelikli deliverable'ı.
WallAutoConnect slipse demo yaşar; Reseed slipse demo ölür.

## Bağlantılı Kararlar
- FloorVariantPainter parametreleri: warpFreq=0.05, zoneFreq=0.05, detailFreq=0.22 (LOCKED 2026-05-11)
- Wall variant set: 8 varyant (straight_H/V, corner_NW/NE/SW/SE, end_L/R) (LOCKED 2026-05-11)
- Floor tier dağılımı: base(<0.65), accent(0.65-0.88), hero(>=0.88) (LOCKED 2026-05-11)
