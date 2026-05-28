# A1 Floor Create Tiles Pro QC Report

## Uretim Detaylari
- Job ID: 85548e6c-963b-434b-82e3-a1a9defbfaad
- Uretim suresi: ~4.5 dk
- Cikti format: individual tiles + composed 4x4 review sheet
- Cikti boyutu: 16 x 64x64 px tiles; review sheet 256x256 px
- Tile sayisi: 16
- Sheet path: STAGING/concepts/fractured_chamber/a1_floor_create_tiles_pro_v1.png
- Tile path: STAGING/concepts/fractured_chamber/a1_floor_tiles/tile_01.png ... tile_16.png

## Wang16 Uyumluluk Degerlendirmesi
- [x] 16 tile mi geldi?
- [ ] Tile'lar Wang16 blob pattern'a map edilebilir mi?
  - Isolated (4 void neighbor) var mi? No clear isolated blob tile.
  - Cap tile'lar (1 neighbor) var mi? (N, S, E, W) No explicit cap set.
  - Corner tile'lar var mi? (NW, NE, SW, SE - inner/outer) No explicit corner set.
  - T-intersection tile'lar var mi? No explicit T set.
  - Cross/Full tile var mi? No explicit cross/full blob tile.
- [x] Yoksa hangi varyant turleri var? Mostly rectangular flagstone floor texture variants, including cracked slabs, rubble-like clusters, and dark groove/grate-like patterns. These are not structured Wang adjacency masks.

## Gorsel QC (Accept Criteria)
- [x] Tile'lar duz walkable ground gibi okunuyor mu (cliff/wall/stair degil)?
- [ ] Clean tile 5x5 blokta seamless tekrar ediyor mu? Not proven; several tiles have strong border lines and large slab cuts that will likely reveal repetition.
- [ ] Cracked tile low-emissive mi (telegraph VFX'den parlak DEGIL)? Cyan emission is mostly absent; cracks read dark instead of low-emissive cyan.
- [ ] Rift glow tile thin cyan hairline mi (kalin bloom DEGIL)? No clear cyan rift hairline pass in the generated sheet.
- [x] Stone palette tutarli mi (charcoal grey, RIMA mood)?
- [ ] Outer corner sorunu var mi? Not directly assessable because no explicit Wang outer-corner tiles were generated.
- [x] View angle Low top-down mi (Side veya High DEGIL)? Reads as top-down/low top-down floor, not side-view.

## RuleTile Uygunluk Skoru
4/10

## Verdict
- [ ] PASS -> Workflow A kullan, RuleTile setup icin Unity dispatch ata
- [ ] PARTIAL -> Bazi tile'lar yeniden uretilmeli (cell-spesifik regen)
- [x] FAIL -> Workflow C (Wang16 sheet) tek basina kullanilsin

## Next Step
Use Workflow C for the Wang16 RuleTile candidate. Keep this Workflow A output only as visual reference or possible random floor detail variants after manual curation.
