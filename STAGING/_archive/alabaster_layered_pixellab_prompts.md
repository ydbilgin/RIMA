# Alabaster Dawn -- Katmanli Map Sistemi PixelLab Batch Promptlari
# Tarih: 2026-05-13
# Hedef: 5 katmanli sistem icin Floor/Accent/Wall/Decal sprite'lari
# Stil: Alabaster Dawn cold slate, organik, seamless, top-down view

---

## Batch 1 -- Floor Base (alabaster_floor_base) (32x32, 16 candidate)

Prompt: pixel art 32x32 seamless floor tile, top-down overhead view, cold slate blue-grey stone surface RGB(62,60,77) as dominant tone, subtle organic micro-texture with gentle natural grain and soft hairline veins staying at 2-3 pixel width, muted desaturated palette with soft shading, tile edges seamlessly blend with adjacent copies creating invisible seam, stone surface stays flat and even with gentle tonal variation across the tile, no shadow, no border, transparent background, cave dungeon floor, intimate enclosed atmosphere, smooth loop-compatible tile

PixelLab MCP Parametreleri:
- size: 32x32
- candidates: 16
- no_animation: true
- generation_mode: Custom V3
- background: transparent

---

## Batch 2 -- Floor Accent (alabaster_floor_accent) (32x32, 16 candidate)

Prompt: pixel art 32x32 seamless floor accent tile, top-down overhead view, cold slate blue-grey palette shifted 15-20 percent lighter toward RGB(78,76,95) with subtle warm mineral undertone, gentle texture difference from base stone suggesting a distinct stone block or mineral inclusion, soft organic shape that blends naturally when scattered as sparse patches over base floor, tile edges stay seamlessly tileable, subtle contrast stays gentle and understated, no harsh edges, no shadow, no border, transparent background, top-down dungeon floor accent patch, soft atmospheric variation

PixelLab MCP Parametreleri:
- size: 32x32
- candidates: 16
- no_animation: true
- generation_mode: Custom V3
- background: transparent

---

## Batch 3 -- Wall Single (alabaster_wall_single) (32x32, 16 candidate)

Prompt: pixel art 32x32 dungeon wall tile, top-down steep overhead ARPG view, cold slate blue-grey stone RGB(55,53,70) as base, upper portion carries a gentle highlight suggesting light source from above with soft warm shimmer at 2-3 pixel rim, lower portion fades to deeper shadow tone toward RGB(40,38,55), stone face shows natural weathered surface with organic worn texture staying subtle and soft, edges stay naturally eroded rather than sharp or geometric, tile designed for perimeter repeat placement without corner logic, consistent vertical shading throughout the tile, no shadow cast on floor, no transparent background, opaque stone wall, muted desaturated field-worn palette, intimate cave stone mood

PixelLab MCP Parametreleri:
- size: 32x32
- candidates: 16
- no_animation: true
- generation_mode: Custom V3
- background: opaque (solid wall tile)

---

## Batch 4 -- Decals (alabaster_decals) (32x32, 16 candidate)

Prompt: pixel art 32x32 decorative floor decal sprite sheet, top-down overhead view, 16 unique decal variations including thin crack lines, mineral veins, water stain marks, worn scuff patches, and erosion marks, each decal stays semi-transparent with 1-3 pixel stroke width, cold slate blue-grey tones RGB(45,43,60) for cracks and RGB(70,85,90) for damp mineral veins, lines stay organic and irregular following natural fracture patterns, decals individually occupy sparse area of the 32x32 canvas leaving surrounding area fully transparent, stroke edges softly anti-aliased at single pixel, gentle tonal variation along each stroke suggesting depth, no solid fill regions, no shadow, fully transparent background, overlay-ready for dungeon floor decoration layer

PixelLab MCP Parametreleri:
- size: 32x32
- candidates: 16
- no_animation: true
- generation_mode: Custom V3
- background: transparent (overlay layer)

---

## Estetik Ozet

Bu dort sprite katmani birlikte basildigi zaman su gorunum ortaya cikmali: FloorBase soguk mavi-gri bir tas zemini olarak odayi kaplar, FloorAccent hafif farkli mineral tonuyla %10-15 alanda organik patches olusturur ve zemin tek duz blok gibi degil dogal tas gibi gorunur, Wall ayni palette'den soguklugunu koruyarak perimeter'i cizer ve ustundeki hafif highlight ile alttaki golge zamk hissi verir, Decals en usttan ince yari saydam catlaklarla ve damarlarla zemini yaslandirir -- toplamda Loop Hero'nun klostrofobik, el cizilmis, dogal malzeme hissini veren ama kendi soguk midnight-cave kimliginde duran bir Alabaster Dawn dungeon atmosferi.
