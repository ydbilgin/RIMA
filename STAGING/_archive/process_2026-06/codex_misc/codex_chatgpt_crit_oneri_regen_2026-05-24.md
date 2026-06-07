# Codex Task — ChatGPT CRIT_ONERI Asset Pack Regen (2026-05-24)

ACTIVE RULES: (1) think before generating (2) min credit (3) batch sheet economy (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç

ChatGPT'nin CHATGPT_CRIT_ONERI/ klasöründeki 7 infographic'e göre tüm wall asset pack'i yeniden üret. User feedback: mevcut sprite'lar "ince gibi + aşağı kayan". Hedef: **kalın, net, edge-to-edge tile eden** wall pieces.

# Üretim Stratejisi

**Tool:** Codex built-in `image_gen` (gpt-image-1 / $imagegen) — proven (bdkrtgasb commit fd00ad23).

**Batch ekonomisi (Karar #90):** 1024×1024 sheet → grid slice. Sheet başına 4-8 piece.

**Style anchor:** Reference image olarak `STAGING/concepts/master_room_pilot/room_v1_gptimage.png` + `STAGING/CHATGPT_TOPDOWN/*.png` kullan.

**Style rules (CRIT_ONERI Image #7 özet):**
- HIGH TOP-DOWN ~85-90° (chatgpt_ref Image 1 angle)
- Dark stone gothic masonry
- Cyan rift cracks (dim, accent only — NOT baked decor)
- Warm torch sockets EMPTY (light goes in Unity 2D Light, NOT baked)
- NO banners baked
- NO door panels baked (empty void doorways)
- KALIN front face, OKUNABILIR top cap
- Edge-to-edge tile etmeli (NO transparent padding outside sprite content)

# Sheet List — 7 Batches

## Sheet A — Connector Columns (8 piece, 1024×1024)
4×2 grid, her cell ~256×512.

| Cell | Piece | Notes |
|---|---|---|
| A1 | Straight column | Düz sütun, simple vertical |
| A2 | Outer corner connector | Dış köşe, 2 faces visible |
| A3 | Inner corner connector | İç köşe, 2 faces (concave) |
| A4 | End column | Uç, single-face termination |
| A5 | Door column LEFT | Kapı sol, jamb edge |
| A6 | Door column RIGHT | Kapı sağ, jamb edge |
| A7 | Alcove connector | Alkov giriş, recess corner |
| A8 | Protrusion connector | Çıkıntı, push-out corner |

Output: `STAGING/concepts/asset_pack_v2/sheet_A_connectors.png`

## Sheet B — Wall Spans Thick (6 piece + 2 variation, 1024×1024)
4×2 grid.

| Cell | Piece | Notes |
|---|---|---|
| B1 | Wall span SHORT (1 tile, ~128×384 thick) | KALIN main face |
| B2 | Wall span MEDIUM (2 tile, ~256×384) | Edge-to-edge tile |
| B3 | Wall span LONG (3 tile, ~384×384) | Multi-tile chain |
| B4 | Cracked wall span | Cyan rift accent |
| B5 | Broken wall span | Collapsed top |
| B6 | Low parapet | Half-height, front edge open |
| B7 | Cracked alt | Variation |
| B8 | Broken alt | Variation |

Output: `STAGING/concepts/asset_pack_v2/sheet_B_walls.png`

## Sheet C — Specialty Walls (4 piece, 1024×512)
2×2 grid.

| Cell | Piece | Notes |
|---|---|---|
| C1 | Prison-bar wall | Vertical bars + cell |
| C2 | Library/bookcase wall | Shelves with books |
| C3 | Ritual wall | Carved runes glowing cyan |
| C4 | Flooded low wall | Waterline mid-height |

Output: `STAGING/concepts/asset_pack_v2/sheet_C_specialty.png`

## Sheet D — Seam Overlays (8 piece P0, 1024×1024)
4×2 grid, ince overlay pieces.

| Cell | Piece | Notes |
|---|---|---|
| D1 | Straight seam | Vertical mortar line |
| D2 | Corner seam (outer) | 2 face junction |
| D3 | Corner seam (inner) | Concave junction |
| D4 | Doorway jamb seam | Door frame edge |
| D5 | Base shadow strip | Horizontal ground shadow |
| D6 | Crack continuation (horizontal) | Cyan rift extension |
| D7 | Crack continuation (vertical) | Cyan rift extension |
| D8 | Pillar-to-wall seam | Connector → span junction |

Output: `STAGING/concepts/asset_pack_v2/sheet_D_seams.png`

## Sheet E — Floor Variants (2 new, 1024×512)
2×2 grid (2 cells unused or for variants).

| Cell | Piece | Notes |
|---|---|---|
| E1 | Ritual floor | Concentric runes |
| E2 | Flooded floor | Water shallow |

Output: `STAGING/concepts/asset_pack_v2/sheet_E_floors.png`

## Sheet F — Socket Props Set 1 (6 piece, 1024×1024)
3×2 grid.

| Cell | Piece | Notes |
|---|---|---|
| F1 | Torch | Wall-mounted with flame |
| F2 | Brazier | Standing fire bowl |
| F3 | Banner | Hanging fabric |
| F4 | Chain | Hanging metal links |
| F5 | Skull chain | Skulls on chain |
| F6 | Shield plaque | Decorative shield mount |

Output: `STAGING/concepts/asset_pack_v2/sheet_F_props1.png`

## Sheet G — Socket Props Set 2 (7 piece, 1024×1024)
3×3 grid (1 cell unused).

| Cell | Piece | Notes |
|---|---|---|
| G1 | Bookshelf insert | Shelf with books |
| G2 | Candle stand | Floor candle |
| G3 | Small statue | Stone statue |
| G4 | Crate | Wooden box |
| G5 | Barrel | Wooden barrel |
| G6 | Bone pile | Pile of skulls/bones |
| G7 | Cyan rift crystal sprout | Floor crystal |

Output: `STAGING/concepts/asset_pack_v2/sheet_G_props2.png`

# Üretim Sırası

```
1. Sheet A (connectors) — en kritik, 8 piece (P0)
2. Sheet B (walls thick) — replace mevcut 8, kalın
3. Sheet D (seams P0) — gap'leri çözer
4. Sheet F (props 1) — torch/banner/chain
5. Sheet E (floors) — ritual + flooded
6. Sheet C (specialty walls)
7. Sheet G (props 2)
```

# Post-Process (Codex auto)

Her sheet için:
1. PIL chroma-key alpha cleanup (background → transparent)
2. Grid slice → individual PNG files
3. Save to `STAGING/concepts/asset_pack_v2/sliced/<sheet>/<piece>.png`
4. Verify dimensions consistent

# Verification

1. 7 sheet PNG exists at expected paths
2. Sliced versions exist (44 total piece)
3. Each piece transparent BG verified
4. Visual QC: chatgpt_ref style match (HIGH TOP-DOWN ~85°, thick walls, no baked decor)

# Çıktı raporu

`STAGING/codex_asset_pack_v2_DONE.md` yaz:
- Generated files (7 sheet paths)
- Sliced files (44 path list)
- Issues / blockers
- Visual observations

git commit otomatik yapma.

# Effort

7 sheet × ~3-5 dk gen + ~10-15 dk slice/cleanup = ~1.5-2.5 saat Codex total.

# Constraints

- Background MAGENTA (chroma-key safe color) — Codex'in image_gen output'unda
- NO transparent direct (PIL ile chroma-key sonradan)
- 17 mevcut asset KORUNUR (yeni v2 ayrı path)
- Codex effort xhigh
