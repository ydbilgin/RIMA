# Smart Map Painter — Design LOCK (2026-05-14 S73)
**Source:** rima-design (Opus) verdict, dispatch ab83b7b32c89fc33b
**Status:** LOCKED for Codex implementation
**Faz:** 1 MVP (3-5h Codex), 25-gün deadline scope-uyumlu

## Locked Decisions

### Q1: Cell-paint Hybrid
**Default mode = Cell.** Click cell = lower/upper boya (all 4 corners set). Vertex mode toolbar toggle (power-user, diagonal cleanup). Karar #131 corner Wang vertex grid altta source-of-truth kalır.

### Q2: Cliff Y-Sort Classification
**9 cliff key:** `{1, 2, 3, 5, 6, 7, 9, 10, 11}` — SW veya SE corner=wall AND NW&NE != ikisi de wall.

Y-offset tablosu:
| Key | Cell Role | Y-Sort Offset |
|---|---|---|
| 0 (All Floor) | floor | 0 |
| 1, 2 (SE/SW Corner) | cliff partial | -0.5 |
| 3, 7, 11 (S Edge variants) | cliff full bottom | -0.5 |
| 4, 8, 12, 13, 14 (N/NW/NE top edges) | top wall | +0.5 |
| 5, 10 (E/W Edge) | cliff side | -0.25 |
| 6, 9 (diagonals) | mixed | -0.25 |
| 15 (All Wall) | wall body | 0 |

**Render:** TilemapRenderer.Individual mode for cliff tilemap, Chunk for base. Player y < cliff y → player önde, y > cliff y → arkada.

### Q3: Tileset Palette Panel
6 PixelLab tileset 64×64 thumbnail strip (sol panel). Cover sprite = tile[15] (All Wall). Click → active layer tileSet field auto-assign. Mevcut ObjectField fallback olarak kalır.

### Q4: Erase = Paint Floor + Red Cursor
Erase mode toolbar veya Shift+Click. Vertex değeri = `1 - currentPaintValue`. Cursor outline RED (vs paint=cyan). Karar #131 corner Wang bozulmaz.

### Q5: Multi-Tileset Stacking
**Faz 1:** shared vertex grid (mevcut), layer = farklı tileset render. Sorting: Base=0, Overlay=10, Decal=20.
**Faz 1.5 deferred:** per-layer vertex grid + active mask.

## Architecture (SOLID)
```
RimaMapDesignerWindow (orchestrator)
 ├─ BrushInputHandler (NEW)        — mouse → cell/vertex intent, no tilemap knowledge
 ├─ TilesetPaletteDrawer (NEW)     — left panel thumbnails, click → assign
 ├─ TilemapMutator (NEW)           — wraps CornerWangPainter, undo, dirty
 └─ delegates Y-sort → CliffYSortManager (NEW runtime MB)

UNTOUCHED (Karar #131 LOCK):
- CornerWangTileSetSO.cs
- CornerWangPainter.cs
```

## Faz 1 MVP Scope (IN)
1. PaintMode {Vertex, Cell} toggle, default Cell
2. Cell→4-corner paint helper
3. Tileset palette thumbnails (sol panel)
4. Erase mode (red cursor)
5. CliffYSortManager runtime MonoBehaviour + 16-key classification
6. ApplyToScene auto-adds CliffYSortManager to layer tilemaps
7. Cell-mode hover preview (full rect highlight)

## Faz 1.5 Deferred (OUT)
- Per-layer vertex grids
- Stamp/Cluster library (Karar #127)
- Naturalness Validator (Karar #130)
- Scatter brush (Karar #121 — ayrı tool)
- 3-state vertex
- Per-frame Y-sort updates

## Rejection Notes
- **Gemini 4-way neighbor bitmask REJECTED** — PixelLab tilesetleri corner Wang formatında, neighbor mask algoritması yanlış görsel çıktı verir.
- **3D billboard sprite tilt REJECTED** — Karar #72 2D Pure violation.
- **3-state vertex (empty terrain) REJECTED** — 81 tile lazım, PixelLab generate etmiyor, cost > value.

## Locked Rule Verification
- Karar #131 corner Wang algorithm ✓ PRESERVED
- Karar #72 2D Pure ✓ PRESERVED
- Karar #126-130 ✓ Faz 1.5 explicit defer
- Karar #99 weapon ✓ Alakasız
