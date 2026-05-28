# Codex Task — Image Gen: Asset Pack Sheet v2 (Test Reference)

## Amaç

User direction (2026-05-22): "codex önce asset pack yapacak, ondan sonra o asset pack'i kullanarak benim karakterlerimi ve benim oyunuma uygun roomlar üretecek. boyutu önemsiz, test için istiyorum, modüler ve sonsuz üretim kanıtı."

Codex imagegen skill (gpt-image-1) ile **24-sprite asset pack görsel envanteri** üret. Step 2'ye (RIMA karakterli oda compose) input olacak — Step 2 bu sheet'teki sprite'ları kullanarak compose çizecek.

**Test reference.** Boyut önemsiz, kalite/atmosfer önemli.

---

## Output

`STAGING/concepts/asset_pack_sheet_v1.png` (önceki dispatch'te kayboldu, re-gen)

After generation: `STAGING/concepts/asset_pack_sheet_v1_NOTES.md` (kısa).

---

## Reference

- `STAGING/codex_dungeon_asset_pack_proposal.md` Section 2 — 24-sprite inventory table (asset id + name + canvas + production method)
- `STAGING/CHATGPT_TOPDOWN/*.png` — style/atmosphere target
- `STAGING/concepts/dungeon_concept_minpack_combat_v1.png` — earlier mockup, same style

---

## Hedef görsel

### Layout
- **4 sütun × 6 satır grid** = 24 cell, her cell 1 sprite
- Boyut: 1024×768 OR 1536×1024 (önemsiz, küçük OK — test için)
- Dark background (#15191F)
- Her cell: sprite render + altında label (asset_id + name)

### Sprite categorization (group by section in sheet)

**Row 1 — FLOOR (6 sprite):**
| Cell | Asset ID | Name | Detay |
|---|---|---|---|
| 1.1 | F01 | granite_slab_a | dark slate gray, asymmetric crack |
| 1.2 | F02 | granite_slab_b | rotation variation |
| 1.3 | F03 | walkway_trim_a | paler stone, foot-worn |
| 1.4 | F04 | walkway_trim_b | walkway variant b |
| 1.5 | F05 | cracked_rubble | broken stone debris |
| 1.6 | F06 | cyan_rift | cyan hairline #5DEFFF |

**Row 2 — WALL (5 sprite + 1 hero):**
| Cell | Asset ID | Name | Detay |
|---|---|---|---|
| 2.1 | W01 | wall_straight_n | 64×96 north edge, front-face Hades-iso |
| 2.2 | W02 | wall_straight_e | 64×96 east edge |
| 2.3 | W03 | wall_corner_outer | 64×96 outer corner |
| 2.4 | W04 | wall_corner_inner | 64×96 inner corner |
| 2.5 | W05 | wall_collapsed_stub | 64×96 ruined |
| 2.6 | H01 | hero_archway_entry | 64×128 dramatic arch w/ cyan keystone |

**Row 3 — PROP A (4 sprite):**
| Cell | Asset ID | Name | Detay |
|---|---|---|---|
| 3.1 | P01 | round_column | 64×96 intact stone column |
| 3.2 | P02 | broken_column | 64×80 collapsed |
| 3.3 | P03 | tattered_banner | 48×80 crimson cloth |
| 3.4 | P04 | wall_torch | 48×64 iron sconce + flame |

**Row 4 — PROP B (3 sprite + 1 spacer):**
| Cell | Asset ID | Name | Detay |
|---|---|---|---|
| 4.1 | P05 | floor_brazier | 64×64 iron brazier + ember |
| 4.2 | P06 | urn_cluster | 48×48 grouped urns |
| 4.3 | P07 | rubble_pile | 64×48 stone debris pile |
| 4.4 | (empty) | — | — |

**Row 5 — DECAL (5 sprite + 1 spacer):**
| Cell | Asset ID | Name | Detay |
|---|---|---|---|
| 5.1 | D01 | moss_patch | 32×32 deep green tuft |
| 5.2 | D02 | crack | 32×32 hairline floor crack |
| 5.3 | D03 | blood | 32×32 dark crimson splatter |
| 5.4 | D04 | dust | 32×32 pale gray film |
| 5.5 | D05 | glyph | 32×32 cyan arcane fragment |
| 5.6 | (empty) | — | — |

**Row 6 — RESERVED (future expansion or filler decorative cells)**

### Style per cell
- Each sprite rendered at **2-3× display scale** (so detail visible at glance)
- Pixel art aesthetic with painterly polish — visible pixel grain but organic shapes
- Each sprite isolated, dark cell background (NOT pure black, slight ambient gradient)
- Cell border: 1-2px subtle cyan or dark gray outline
- Label below each sprite: asset_id (cyan, larger) + name (white, smaller), tiny canvas size (gray, smallest)

### Sheet header
- Title: **"RIMA — Act 1 Shattered Keep — Min Asset Pack v1 (24 sprites)"**
- Subtitle: "Hades-iso ~70-75° tilt | Test reference for modular composition"

### Mood
- Dark slate background, ambient cyan/gold accent lights between cells
- Game studio asset reference sheet aesthetic
- Pixel art at concept-art resolution per sprite

---

## Negative directives
- NO single-scene composition (this is a SHEET grid, not a room mockup)
- NO mixing sprites across cells (each cell isolated 1 sprite)
- NO bright cartoon palette
- NO 3D render

---

## Tool

Codex imagegen skill (gpt-image-1 force). Single output PNG. Don't overthink resolution — test reference only.

Save to: `STAGING/concepts/asset_pack_sheet_v1.png`

---

## After image gen — NOTES.md

`STAGING/concepts/asset_pack_sheet_v1_NOTES.md`:

### Sprite check
| Asset | Visible in sheet? | Matches spec? |
|---|---|---|
| F01-F06, W01-W05, H01, P01-P07, D01-D05 (24 entries) | YES/NO | PASS/TWEAK |

### Recommendation
- Sheet usable as Step 2 reference? YES/NO

---

## Commit
```
[Codex] [S98 IMAGEGEN] Asset pack sheet v2 — 24-sprite visual reference

- Codex imagegen 4×6 grid sheet
- 24 sprite labeled (F/W/P/D/H sections)
- Step 1 of sequential: input for Step 2 (RIMA character room compose)
- PNG: STAGING/concepts/asset_pack_sheet_v1.png
```

Wall clock: ~5-10 min.
