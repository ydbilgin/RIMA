# PixelLab Pro Generation Log

> Pro UI slider değerleri MCP metadata'ya yazılmıyor. Reproduction için elle log tutuyoruz.

## Format
Her entry: tileset id + 4 slider + 3 prompt referansı + tarih + notlar.

---

## 2026-05-14 — Path↔Moss (Shattered Keep zemin↔zemin)

| Alan | Değer |
|---|---|
| **Tileset ID** | `b41919aa-d20c-441e-a812-67e1f25f3331` |
| Lower base tile ID | `3bdfb21d-c5c9-4a93-993a-6313f9b57d08` |
| Upper base tile ID | `21223297-9461-4f62-945f-7366a47b90aa` |
| Tile Size | 32×32 |
| **Transition Height** | **5%** |
| **Transition** (advanced) | **0%** |
| **Spread** (advanced) | **20%** |
| **Raggedness** (advanced) | **55%** |
| Generator | PixelLab Web UI → Generate Pro |
| Date | 2026-05-14 |

**Lower Terrain prompt:**
> Worn stone walkway path, 32x32 top-down pixel art tile, paler #4A4842 stone slabs slightly smoother than rubble floor, foot-traffic polish, occasional cracks, hairline cyan rift dust accents very rare. Mat painterly pixel, dark gritty palette, asymmetric weathering, must connect cleanly against surrounding shattered keep floor, no characters, no props, tileable.

**Upper Terrain prompt:**
> Moss-overgrown stone patches on shattered keep floor, 32x32 top-down pixel art tile, same #4A4842 stone base partially covered with damp dark green moss patches RGB(54,75,46), small wet glistening puddles in cracks, walkable flat ground at exact same elevation as the path — NO height difference, NO wall, NO raised edge, just texture variation, dark gritty palette, Salt and Sanctuary chibi-but-serious mood.

**Transition Description:**
> Soft organic moss patch edges where damp green growth creeps onto worn walkway stones, mossy clumps fading into bare polished stone, occasional small puddles at the boundary, no straight line, irregular natural fade with subtle damp glistening, walkable continuous surface, no height edge, dark gritty palette, asymmetric weathering, Raggedness 50 percent or higher organic boundary.

**Visual result:** Dramatic improvement over Standard-mode rubble↔moss (9591f35a). Organic moss patch boundaries, painterly texture, no visible grid seam. 31 KB vs 9 KB pixel complexity. → A/B comparison in `STAGING/pro_vs_standard_test/`.

**Notes:**
- Raggedness 55% = sweet spot for zemin↔zemin "moss creeping into stone" effect
- Transition Height 5% (not 0%) = micro-rise barely visible, prevents totally-flat dead look
- Spread 20% = gentle fade gradient
- Slider state lost in MCP metadata → this log is reproduction source-of-truth

---

## Template — sonraki entry'ler için kopyalanacak

```
## YYYY-MM-DD — <Lower>↔<Upper> (<biome>)

| Alan | Değer |
|---|---|
| Tileset ID | `...` |
| Lower base tile ID | `...` |
| Upper base tile ID | `...` |
| Tile Size | 32×32 |
| Transition Height | %X |
| Transition | %X |
| Spread | %X |
| Raggedness | %X |
| Generator | PixelLab Web UI → Generate Pro |
| Date | YYYY-MM-DD |

Lower prompt: …
Upper prompt: …
Transition Description: …

Visual result: …
Notes: …
```
