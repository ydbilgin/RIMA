# Imagegen Batch-3 2026-06-07 Manifest

Pipeline: built-in imagegen source on solid magenta chroma key, local chroma cleanup, nearest-neighbor target sizing, PNG with alpha.

## QC Log — 2026-06-07 (Sonnet real-eyes pass)
- Defringe pass 1 (R>150,G<80,B>100): 2820 pixels removed across 20 files
- Defringe pass 2 purple-fringe (R>110,G<70,B>90,|R-B|<40): 753 pixels across 3 files
- 3 REGEN (ax imagegen, 1 attempt each):
  - ground_decal_combat_scratches.png: was empty (143 bytes) → 4 claw scratches, 64x32, PASS
  - ground_decal_portal_scorch.png: was broken artifact → radial scorch + cyan sparks, 128x64, PASS
  - hole_rim_glow.png: had text watermark → stone ledge strip + cyan glow edge, 64x32, PASS
- All 24 files Read+visually confirmed by agent (not reported without seeing)


## Assets

### arrival_ring.png
- Final size: 96x96
- Purpose: spawn varış halkası
- Status: PASS
- Retries: 0
- Prompt used: glowing cyan spawn arrival ring, flat circular glyph seen from a 2:1 dimetric projection (flattened ellipse), intricate rune markings, 96x96 pixels. Dark gritty fantasy style. Solid flat magenta background.

### distant_island_silhouette_01.png
- Final size: 640x320
- Purpose: uzak yüzen ada silüeti 01
- Status: PASS
- Retries: 0
- Prompt used: A pixel art sprite of a distant floating island silhouette, dark purple/black gritty silhouette, low detail, faint cyan glowing energy cracks. Dark fantasy roguelite style. Solid flat magenta background.

### distant_island_silhouette_02.png
- Final size: 640x320
- Purpose: uzak yüzen ada silüeti 02
- Status: PASS
- Retries: 0
- Prompt used: A pixel art sprite of a second distant floating island silhouette, dark purple/black gritty silhouette, low detail, faint cyan glowing energy cracks. Dark fantasy roguelite style. Solid flat magenta background.

### distant_island_silhouette_03.png
- Final size: 640x320
- Purpose: uzak yüzen ada silüeti 03
- Status: PASS
- Retries: 0
- Prompt used: A pixel art sprite of a third distant floating island silhouette, dark purple/black gritty silhouette, low detail, faint cyan glowing energy cracks. Dark fantasy roguelite style. Solid flat magenta background.

### void_nebula_sheet.png
- Final size: 1024x1024
- Purpose: void sisi/nebula dokusu
- Status: PASS
- Retries: 0
- Prompt used: Seamless tileable dark fantasy void nebula texture. Very dark purple-black and deep cyan wispy gradient cloud patterns, extremely dark, low saturation, moody space background, gritty pixel art texture, 1024x1024.

### hole_rim_straight.png
- Final size: 64x32
- Purpose: iç-delik düz kenar decal
- Status: PASS
- Retries: 0
- Prompt used: Pixel art sprite of a horizontal straight edge stone hole rim decal. Dark gritty slate stone texture, 2:1 dimetric perspective. Solid flat magenta background.

### hole_rim_corner.png
- Final size: 64x64
- Purpose: iç-delik köşe kenar decal
- Status: PASS
- Retries: 0
- Prompt used: Pixel art sprite of a curved corner stone hole rim decal. Dark gritty slate stone texture, 2:1 dimetric perspective. Solid flat magenta background.

### hole_rim_cracked.png
- Final size: 64x32
- Purpose: iç-delik kırık kenar decal
- Status: PASS
- Retries: 0
- Prompt used: Pixel art sprite of a cracked broken stone hole rim decal. Dark gritty slate stone texture, 2:1 dimetric perspective. Solid flat magenta background.

### hole_rim_glow.png
- Final size: 64x32
- Purpose: iç-delik cyan parlayan kenar decal
- Status: PASS (REGEN 2026-06-07)
- Retries: 1 (original had text/watermark "CURVED CORNER TRY")
- Prompt used: pixel art broken stone ledge strip seen from above, thin cyan glow along bottom broken edge, on plain magenta background, dark fantasy dungeon style, no text no letters no watermark

### ground_decal_thin_cyan_crack.png
- Final size: 128x64
- Purpose: ince cyan yer çatlağı
- Status: PASS
- Retries: 0
- Prompt used: Pixel art ground decal: a thin cyan glowing crack lying flat. 2:1 dimetric projection, dark gritty fantasy style. Solid flat magenta background.

### ground_decal_circular_rift_scar.png
- Final size: 128x64
- Purpose: dairesel rift çatlak izi
- Status: PASS
- Retries: 0
- Prompt used: Pixel art ground decal: a circular rift scar/crack lying flat. 2:1 dimetric projection, dark gritty fantasy style. Solid flat magenta background.

### ground_decal_ritual_line_broken.png
- Final size: 128x64
- Purpose: kırık ritüel çizgisi
- Status: PASS
- Retries: 0
- Prompt used: Pixel art ground decal: a broken ritual rune circle lying flat. 2:1 dimetric projection, dark gritty fantasy style. Solid flat magenta background.

### ground_decal_combat_scratches.png
- Final size: 64x32
- Purpose: çatışma sıyrık izleri
- Status: PASS (REGEN 2026-06-07)
- Retries: 1 (original was empty/broken)
- Prompt used: pixel art game ground decal, cluster of 4 thick dark-gray claw scratch marks on plain magenta background, gritty fantasy style, high contrast, no text no letters no watermark

### ground_decal_portal_scorch.png
- Final size: 128x64
- Purpose: portal yanık izi
- Status: PASS (REGEN 2026-06-07)
- Retries: 1 (original was broken artifact/multi-pixel blob)
- Prompt used: pixel art game ground decal, radial dark scorch burn mark with small cyan ember sparks, on plain magenta background, dark fantasy style, no text no letters no watermark

### ground_decal_faded_rune_tiles.png
- Final size: 128x64
- Purpose: soluk rünlü zemin karoları
- Status: PASS
- Retries: 0
- Prompt used: Pixel art ground decal: faded stone floor tiles with faint runes lying flat. 2:1 dimetric projection, dark gritty fantasy style. Solid flat magenta background.

### edge_filler_broken_stone_chunk.png
- Final size: 32x32
- Purpose: kırık taş parçası
- Status: PASS
- Retries: 0
- Prompt used: Pixel art prop: broken slate stone chunk. Dark gritty fantasy style. Solid flat magenta background.

### edge_filler_rift_shard.png
- Final size: 32x32
- Purpose: parlayan rift şardı
- Status: PASS
- Retries: 0
- Prompt used: Pixel art prop: glowing cyan rift shard. Dark gritty fantasy style. Solid flat magenta background.

### edge_filler_chain_stump.png
- Final size: 32x32
- Purpose: zincir kalıntısı
- Status: PASS
- Retries: 0
- Prompt used: Pixel art prop: broken iron chain stump. Dark gritty fantasy style. Solid flat magenta background.

### edge_filler_rubble_pile.png
- Final size: 64x32
- Purpose: moloz yığını
- Status: PASS
- Retries: 0
- Prompt used: Pixel art prop: pile of dark stone rubble. Dark gritty fantasy style. Solid flat magenta background.

### edge_filler_void_crystal_nub.png
- Final size: 32x32
- Purpose: ufak void kristali
- Status: PASS
- Retries: 0
- Prompt used: Pixel art prop: tiny dark purple void crystal cluster. Dark gritty fantasy style. Solid flat magenta background.

### edge_filler_altar_debris.png
- Final size: 96x64
- Purpose: sunak kalıntısı
- Status: PASS
- Retries: 0
- Prompt used: Pixel art prop: debris from a broken ancient stone altar. Dark gritty fantasy style. Solid flat magenta background.

### parapet_low_segment_01.png
- Final size: 96x64
- Purpose: alçak korkuluk segmenti 01
- Status: PASS
- Retries: 0
- Prompt used: Pixel art wall segment: low broken slate stone parapet railing. Cracked dark slate, dark gritty fantasy style. Solid flat magenta background.

### parapet_low_segment_02.png
- Final size: 96x64
- Purpose: alçak korkuluk segmenti 02
- Status: PASS
- Retries: 0
- Prompt used: Pixel art wall segment: second low broken slate stone parapet railing. Cracked dark slate, dark gritty fantasy style. Solid flat magenta background.

### portal_label_plaque.png
- Final size: 128x32
- Purpose: portal taş plaket frame'i
- Status: PASS
- Retries: 0
- Prompt used: Pixel art empty rectangular stone plaque frame, empty flat center, detailed frame border. Dark gritty fantasy style. Solid flat magenta background.

