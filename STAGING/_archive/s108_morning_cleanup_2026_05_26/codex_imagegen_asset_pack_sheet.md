# Codex Task — Image Gen: RIMA Min-Pack Asset Sheet (Large Detailed)

## Amaç

Codex imagegen skill (gpt-image-1) ile **24-sprite asset pack envanterini görsel sheet/atlas** olarak üret. Her sprite ayrı kutuda, etiketli (asset_id + name + size). User'a paslanır → her sprite'ın somut görünümü netleşir → PixelLab production öncesi referans.

**NOT asset.** Görsel envanter referansı.

---

## Output

`STAGING/concepts/asset_pack_sheet_v1.png`

After generation: `STAGING/concepts/asset_pack_sheet_v1_NOTES.md` (kısa breakdown).

---

## Reference materials

- `STAGING/codex_dungeon_asset_pack_proposal.md` Section 2 (24-sprite inventory table)
- `STAGING/CHATGPT_TOPDOWN/*.png` (style + atmosphere reference, NOT layout)
- `STAGING/asset_production_master_plan_v3.md` (pivot Hades-iso lock)

---

## Hedef görsel — Asset Pack Sheet

### Boyut + format
- **YÜKSEK ÇÖZÜNÜRLÜK:** En az **2048×1536** (4:3) veya **2400×1600**. Her sprite zoom'lanmış net görünmeli.
- Layout: **4 sütun × 6 satır grid** (24 sprite tam dolduran)
- Her cell: ~512×256 alan (sprite + label)
- Dark background (#15191F) atmosfer için
- Etiketler: beyaz/cyan font, küçük puntolar (Asset ID + name + native canvas size)

### Sheet sectionları (gruplandırılmış)

**Section 1: FLOOR TILES (top row, 6 sprite):**
- F01 floor_granite_slab_a — dark slate gray, asymmetric crack pattern, weathered stone block
- F02 floor_granite_slab_b — same tone but different pattern (rotation/flip variation)
- F03 floor_walkway_trim_a — paler stone slab, foot-worn polished surface
- F04 floor_walkway_trim_b — walkway variant b, slight edge crack
- F05 floor_cracked_rubble — broken stone chunks, debris scatter, warm-gray tone
- F06 floor_cyan_rift — charcoal stone with cyan #5DEFFF hairline rift accent

**Section 2: WALLS (second row, 5 sprite + 1 hero):**
- W01 wall_straight_n — north edge, 64×96, front-face visible Hades-iso, weathered stone block, top profile
- W02 wall_straight_e — east edge, dikey orientation, front-face left-facing
- W03 wall_corner_outer — köşe block, 90° angle, weathered
- W04 wall_corner_inner — iç köşe (door break / L-shape)
- W05 wall_collapsed_stub — yıkık duvar parçası, partial blocker, rubble base
- H01 hero_archway_entry — 64×128 dramatic stone archway, cyan rift hint at keystone, two pillars + arch top

**Section 3: PROPS (third row, 7 sprite):**
- P01 prop_round_column — intact stone column, vertical 64×96, weathered groove rings
- P02 prop_broken_column — collapsed leaning column, dust at base
- P03 prop_tattered_banner — faded crimson cloth hanging, torn edges, 48×80
- P04 prop_wall_torch — iron sconce + warm orange flame, mounted on wall
- P05 prop_floor_brazier — standalone iron brazier, warm orange ember fire, base 64×64
- P06 prop_urn_cluster — 3-4 grouped weathered clay urns, asymmetric
- P07 prop_rubble_pile — broken stone debris pile, blocker visual, asymmetric

**Section 4: DECALS (fourth row, 5 sprite):**
- D01 decal_moss_patch — irregular moss tuft, deep green, organic edges
- D02 decal_crack — hairline floor crack pattern, branching
- D03 decal_blood — dark crimson irregular splatter, faded
- D04 decal_dust — pale gray faint dust film patch
- D05 decal_glyph — cyan #5DEFFF arcane circle fragment, runic inscription hint

### Style per sprite cell
- Each sprite rendered at **2-3x display scale** (so detail visible at glance)
- Pixel art aesthetic with painterly polish — visible pixel grain but organic shapes
- Each sprite isolated against subtle dark cell background (NOT pure black, slight gradient)
- Cell border: 1-2px subtle outline (cyan or dark gray)
- Label below each sprite: ASSET_ID + name (one line), tiny canvas size text below (e.g. "64×96 px")

### Sheet header
- Title at top: **"RIMA — Act 1 Shattered Keep — Min Asset Pack v1 (24 sprites)"**
- Subtitle: "Hades-iso ~70-75° tilt | 64 PPU | Acts 1 production candidate"

### Sheet footer
- Total count: "24 unique sprites | ~155-165 gen | Acts 2-4 variant via Hibrit C plan"
- Date stamp + version

### Style — overall sheet
- Game art reference document feel — like AAA studio asset sheet
- Dark background (deep slate), subtle ambient lighting per cell
- Professional polished, NOT cartoon, NOT bright
- Pixel art at concept-art resolution

---

## Negative directives
- NO single-scene composition (NOT a room mockup — this is a SHEET)
- NO mixing sprites across cells (each cell isolated 1 sprite)
- NO grid lines between tile cells (boundaries via subtle outline only)
- NO bright cartoon palette
- NO 3D render
- NO photorealistic
- NO modern UI elements (HUD-style is OK for label aesthetics)

---

## Tool

Codex imagegen skill (gpt-image-1 force). Aim for highest detail mode + largest resolution available.

If first gen quality insufficient (sprites too small, labels unreadable, atmosphere wrong), regen up to 2 times with prompt refinement.

Save to: `STAGING/concepts/asset_pack_sheet_v1.png`

---

## After image gen — NOTES.md

`STAGING/concepts/asset_pack_sheet_v1_NOTES.md`:

### Sprite check table
| Asset ID | Sprite visible in sheet? | Matches inventory spec? | Quality grade |
|---|---|---|---|
| F01 | YES/NO | PASS/TWEAK | A/B/C/F |
| ... (24 rows) | | | |

### Visual quality assessment
- Sheet readable at glance: PASS/TWEAK/FAIL
- Each sprite individually clear: PASS/TWEAK/FAIL
- Labels readable: PASS/TWEAK/FAIL
- Style consistent across all 24 sprites: PASS/TWEAK/FAIL

### Recommendation
- Sheet quality enough as production reference? YES/NO
- If NO, what's missing or needs regen

---

## Commit
```
[Codex] [S98 IMAGEGEN] Asset pack sheet v1 — 24-sprite inventory visual reference

- Codex imagegen (gpt-image-1) high-res sheet of asset pack proposal
- 4×6 grid layout, each sprite labeled
- Floor + Wall + Prop + Decal + Hero sections
- PNG: STAGING/concepts/asset_pack_sheet_v1.png
- NOTES: STAGING/concepts/asset_pack_sheet_v1_NOTES.md
```

Wall clock: ~10-15 min.
