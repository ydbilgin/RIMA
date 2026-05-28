ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — A1 Floor Wang16 Sheet Inpaint Fix + RuleTile Mapping

**Amaç:** User PixelLab web UI'da Wang16 4×4 sheet üretti (`STAGING/concepts/fractured_chamber/a1_floor_wang16_workflow_c_v2.png`). 16 cell'in ~%60-70'i Wang16 standart layout'a uyuyor, eksik/yanlış olanları PixelLab MCP inpaint tool ile cell-by-cell düzelt. Final Wang-uyumlu sheet üret + Unity RuleTile setup için ScriptableObject + mapping yaz.

## Görev 1 — Source sheet analizi

`STAGING/concepts/fractured_chamber/a1_floor_wang16_workflow_c_v2.png` (256×256, 4×4 grid, cell 64×64) görselini AÇ ve incele.

Her cell için (row-major sırayla) tespit et:
- Granite hangi kenarlara extending? (N, E, S, W kombinasyonu)
- Hangi Wang16 standart pattern'a karşılık geliyor?
- Beklenen Wang pattern ile uyumlu mu? PASS/FAIL/PARTIAL

### Beklenen Wang16 layout (RIMA standart):

| Cell | Row,Col | Beklenen Pattern | Granite Extends |
|---|---|---|---|
| 1 | 1,1 | ISOLATED | (none — isolated) |
| 2 | 1,2 | CAP-N | S only |
| 3 | 1,3 | CAP-S | N only |
| 4 | 1,4 | VERTICAL-PASS | N + S |
| 5 | 2,1 | CAP-W | E only |
| 6 | 2,2 | CORNER-NW (inner top-left) | E + S |
| 7 | 2,3 | CORNER-SW (inner bottom-left) | E + N |
| 8 | 2,4 | T-LEFT | E + N + S |
| 9 | 3,1 | CAP-E | W only |
| 10 | 3,2 | CORNER-NE (inner top-right) | W + S |
| 11 | 3,3 | CORNER-SE (inner bottom-right) | W + N |
| 12 | 3,4 | T-RIGHT | W + N + S |
| 13 | 4,1 | HORIZONTAL-PASS | E + W |
| 14 | 4,2 | T-TOP | E + W + S |
| 15 | 4,3 | T-BOTTOM | E + W + N |
| 16 | 4,4 | FULL | N + E + S + W (all 4) |

Çıktı: `STAGING/a1_wang16_v2_cell_analysis.md` — markdown tablo:
```markdown
| Cell | Beklenen | Mevcut | Verdict | Action |
|---|---|---|---|---|
| 1 (1,1) | ISOLATED | ... | PASS/FAIL | Keep / Inpaint |
```

## Görev 2 — Eksik/Yanlış cell'leri PixelLab inpaint ile düzelt

FAIL verdict alan cell'ler için **PixelLab MCP inpaint tool** kullan:

### Tool seçimi
- `mcp__pixellab__inpaint_v3` — primary (en stabil)
- `mcp__pixellab__inpaint_pixpatch_v2` — alternative (M-L sized)
- Eğer ikisi de uygun değilse `mcp__pixellab__edit_image` veya `mcp__pixellab__edit_image_pro`

### Inpaint workflow (her FAIL cell için)
1. Source image: `a1_floor_wang16_workflow_c_v2.png` (256×256)
2. Mask area: exact 64×64 cell bounds (örn. Cell 6 (Row 2 Col 2) → x:64, y:64, w:64, h:64)
3. Inpaint prompt: cell-spesifik Wang pattern + RIMA style lock
4. Output: yeni 256×256 image, sadece o cell değişmiş
5. Sonraki FAIL cell için yine inpaint (zincirleme)

### Inpaint prompt template (her cell için)

```
dark fantasy pixel art fractured dark granite floor tile on pure black void background, RIMA Shattered Keep palette, charcoal grey stone, thin cyan rift hairline cracks, top-down 70-80 degree 3/4 view, jagged broken stone transition between granite and void.

[CELL-SPESIFIK GEOMETRI]:
- [örn: granite L-shape connecting east and bottom edges, void on top and left]

no text, no labels, no captions, no numbers, no walls, no pillars, no props, no characters, no UI.
```

### Cell-spesifik geometri açıklamaları (Codex bu açıklamaları kullanır)

| Cell | Wang Pattern | Inpaint geometry açıklaması |
|---|---|---|
| 1 | ISOLATED | small granite patch in center, pure black void on all 4 sides |
| 2 | CAP-N | granite extends only to bottom edge of tile, void on top left and right |
| 3 | CAP-S | granite extends only to top edge of tile, void on bottom left and right |
| 4 | V-PASS | granite vertical strip from top to bottom, void on left and right |
| 5 | CAP-W | granite extends only to right edge, void on top bottom and left |
| 6 | CORNER-NW | granite L-shape connecting right and bottom edges, void on top and left |
| 7 | CORNER-SW | granite L-shape connecting right and top edges, void on bottom and left |
| 8 | T-LEFT | granite T-shape extending right top and bottom, void only on left |
| 9 | CAP-E | granite extends only to left edge, void on top bottom and right |
| 10 | CORNER-NE | granite L-shape connecting left and bottom edges, void on top and right |
| 11 | CORNER-SE | granite L-shape connecting left and top edges, void on bottom and right |
| 12 | T-RIGHT | granite T-shape extending left top and bottom, void only on right |
| 13 | H-PASS | granite horizontal strip from left to right, void on top and bottom |
| 14 | T-TOP | granite T-shape extending left right and bottom, void only on top |
| 15 | T-BOTTOM | granite T-shape extending left right and top, void only on bottom |
| 16 | FULL | granite fills entire tile, no void edges visible |

### Önemli inpaint notları
- Her inpaint çağrısı job_id döndürür, async poll et
- Style consistency için her inpaint prompt'unun başında ortak style lock paragrafı kullan
- FAIL cell sayısı 4'ten fazla ise paralel batch yerine sequential yap (drift prevention)
- Her successful inpaint sonrası output dosyayı `STAGING/concepts/fractured_chamber/a1_floor_wang16_v3_iteration_N.png` olarak versionla
- Final temiz sheet: `STAGING/concepts/fractured_chamber/a1_floor_wang16_FINAL.png`

## Görev 3 — Sheet'i 16 ayrı PNG'ye slice et

Final sheet hazır olunca Codex Python (PIL) ile slice:
- Output: `STAGING/concepts/fractured_chamber/a1_floor_wang16_final_tiles/tile_01_isolated.png` ... `tile_16_full.png`
- Her tile 64×64 PNG
- Naming: `tile_NN_pattern.png` (örn: `tile_06_corner_nw.png`)

## Görev 4 — Unity RuleTile + ScriptableObject setup

Final tile'lar hazır olunca Unity tarafı:

1. **Tile'ları Unity'e import:**
   - `Assets/Art/FracturedChamber/floor/wang16/*.png`
   - Sprite Mode: Single, PPU 64, Pivot: Bottom, Filter: Point, Compression: None
   - .meta dosyaları doğru üret

2. **RuleTile ScriptableObject yarat:**
   - `Assets/Tiles/FracturedChamber/floor_wang16.asset`
   - 16 rule entry — komşu pattern'a göre tile mapping
   - Wang16 corner-based veya edge-based ne uygunsa Unity 2D Tilemap Extras API'siyle setup

3. **Test scene:**
   - `Assets/Scenes/Demo/FloorWang16_Test.unity`
   - Tilemap GameObject (Grid cell 64×64)
   - Paint 10×10 floor area, auto-tile çalışıyor mu doğrula
   - Camera + minimum lighting

4. **Verify console + dotnet build:**
   - Console error/warning yok mu
   - `dotnet build` 0 errors

## Çıktı dosyaları

```
STAGING/concepts/fractured_chamber/
├─ a1_floor_wang16_v2_cell_analysis.md       (Görev 1 raporu)
├─ a1_floor_wang16_v3_iteration_N.png         (her iterasyon, opsiyonel)
├─ a1_floor_wang16_FINAL.png                  (final temiz sheet)
└─ a1_floor_wang16_final_tiles/
    ├─ tile_01_isolated.png
    ├─ tile_02_cap_n.png
    ├─ ...
    └─ tile_16_full.png

Assets/
├─ Art/FracturedChamber/floor/wang16/*.png + .meta
├─ Tiles/FracturedChamber/floor_wang16.asset (RuleTile)
└─ Scenes/Demo/FloorWang16_Test.unity

STAGING/
└─ a1_wang16_final_qc.md                      (final report)
```

## Final QC raporu

`STAGING/a1_wang16_final_qc.md`:
```markdown
# A1 Wang16 Final QC

## Source
- v2 sheet: [path]
- FAIL cells: [list]
- Inpaint iterasyon sayısı: [N]

## Final sheet
- Path: [path]
- 16/16 Wang pattern PASS?
- Stil tutarlılığı: [skor 1-10]

## Unity setup
- RuleTile path: [path]
- Test scene path: [path]
- Console: [errors/warnings]
- dotnet build: [PASS/FAIL]

## Verdict
- READY for Battered Hall MVP? (PASS/FAIL)
- Önerilen sonraki adım
```

## Önemli notlar
- `mcp__pixellab__inpaint_v3` parametrelerini doc'tan kontrol et (PixelLabDocs/inpaint-v3.md mevcut olabilir)
- Inpaint mask'ı **pure black/white binary** olarak ver (PixelLab format) — Python PIL ile cell-bound mask üret
- FAIL cell sayısı 8'den fazlaysa BLOCKED ver, user'a "tüm sheet regen daha mantıklı, Workflow B'ye geç" öner
- Codex direkt Unity import + RuleTile setup yapsın (asset üretim + Unity ikisi de bu task'ta)
- Her inpaint iterasyonu sonrası diff göster (eğer mümkünse)
