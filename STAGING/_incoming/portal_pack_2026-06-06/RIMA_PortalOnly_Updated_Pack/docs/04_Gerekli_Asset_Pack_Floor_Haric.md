# 04 — Gerekli Asset Pack (Floor Hariç)

Bu listede floor yok; çünkü base floor set zaten mevcut.

## A. Cliff set — minimum gerekli
Bunlar asıl sınır hissini verecek parçalar.

1. straight front cliff
2. damaged front cliff
3. outer corner left cliff
4. outer corner right cliff
5. inner corner left cliff
6. inner corner right cliff
7. front cliff end-cap left
8. front cliff end-cap right

### Opsiyonel
9. north top-edge trim
10. west top-edge trim
11. low parapet straight
12. low parapet broken
13. small broken edge fragment

## Cliff boyut önerisi
- footprint align: 64x32 grid
- export canvas:
  - 64x96
  - 64x128
- büyük köşe cliff için 128x128 de olabilir

## B. Portal set — minimum gerekli
### Ana karar
Portal asset yönü = **1**

### Slotlar
- ENTRY_S
- EXIT_NW
- EXIT_N
- EXIT_NE

### Türler
1. combat portal
2. elite portal
3. reward portal
4. heal/lore portal
5. boss portal

### Portal üretim mantığı
- 1 temel stone arch / frame
- 5 işlevsel varyant
- farklılaştırma:
  - top rune / crest
  - icon
  - particle intensity
  - color accent
  - reward badge

### Boyut önerisi
- 96x128
- 128x160

## C. Prop set — minimum gerekli
### Landmark props
1. chamber pedestal
2. broken altar
3. seal monolith
4. rift crystal cluster
5. broken statue fragment
6. chain/banner ruin

### Edge filler / support props
7. bone pile
8. brazier
9. broken pillar
10. rubble pile
11. candles / urn set

### Boyut
- edge filler: 64x64 – 96x96
- landmark: 128x128 – 160x160

## D. VFX / overlay
1. portal burst
2. portal idle core
3. rift crack ground decal
4. chamber ring glow
5. light particle dust
6. small fog wisps

## Öncelik sırası
1. Cliff
2. Portal
3. Prop
4. Optional parapet / trim
5. VFX polish
