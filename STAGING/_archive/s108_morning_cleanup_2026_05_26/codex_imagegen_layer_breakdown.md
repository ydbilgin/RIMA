# Codex Task — Layer Breakdown of v1 Concept Art

## Gorev
**v1 concept art** (`STAGING/concept_art_rima_sample_room.png`) bizim RIMA pipeline'i ile gercekten uretilir mi? User ikna olmak istiyor.

Codex **imagegen skill** ile **ayni sahnenin layer-by-layer breakdown'unu** ciz. Her layer'i ayri panel olarak goster — boylece kullanici **"AH, bu pipeline bu adimlarla bu sonucu uretir"** seklinde gorur.

## Reference Image
`F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/concept_art_rima_sample_room.png`

Bu image v1 imagegen output'u — bizim hedef sample combat room (Ranger + props + lighting + moss + rift).

## Hedef: 8-Panel Layer Breakdown Composition

Tek wide image (1920x1080 ratio), 4x2 grid layout, her panel ayni odanin farkli build-stage'i:

### Panel 1 — L1 Base Tone
- Sadece dark blue-teal (#1A2438) flat ambient renk
- Bos canvas, hicbir tile yok
- Caption: "L1 Base Tone (ambient)"

### Panel 2 — + L2 Floor Atlas (random tiles)
- L1 uzerine StoneDungeon floor tiles 64x64 random variant
- Tile grid GORUNUR su anda (kasitli)
- Caption: "+ L2 Floor Atlas (grid visible)"

### Panel 3 — + L3 Walls Wang16
- L2 uzerine top/bottom/left/right Wang16 wall tiles (64x96)
- Wall corners auto-blend
- Hala grid hissi var ama border belli oldu
- Caption: "+ L3 Walls Wang16 (border, still grid)"

### Panel 4 — + L4 Organic Moss
- L3 uzerine moss patches eklendi
- Patches tile boundaries'i AS-IYOR
- Grid kaybolmaya basladi
- Caption: "+ L4 Moss patches (grid breaks down)"

### Panel 5 — + L5 Detail (Cracks + Pebbles + Bones)
- L4 uzerine cracks, pebbles, scattered bones
- Tile araliklari filled
- Caption: "+ L5 Detail scatter (organic noise)"

### Panel 6 — + L6 Accent (Rift Scar)
- L5 uzerine buyuk dark crimson rift scar (center-right)
- Multi-blob irregular oval, radial cracks
- Major focal organic element
- Caption: "+ L6 Rift accent (large overlay)"

### Panel 7 — + Props + Character
- L6 uzerine wooden crate, broken urn, brazier, 2 candle, banner
- Karakter (Ranger chibi) sahnenin merkezinde
- Caption: "+ Props + Character (Ranger anchor)"

### Panel 8 — + Lighting (Final)
- Tum onceki layer'lar + warm brazier glow + candle halos + ambient depth
- Final image = v1 concept art ile aynisi
- Caption: "+ Lighting = Final Game View"

## Style Direktifleri (her panel icin)
- Ayni camera angle (top-down 30-35° tilt) tum 8 panel'de
- Ayni komposit dimensions
- Pixel art aesthetic — bizim Vivid Vulnerability palette
- Panel 1-3: grid VISIBLE (kasitli)
- Panel 4-8: grid progressively kaybolur
- Panel 8 = v1 output ile EXACTLY ayni gorunum

## Panel Layout
```
+-----+-----+-----+-----+
| P1  | P2  | P3  | P4  |
| L1  | +L2 | +L3 | +L4 |
+-----+-----+-----+-----+
| P5  | P6  | P7  | P8  |
| +L5 | +L6 |+Prop| Lig |
+-----+-----+-----+-----+
```

Her panel ~480x270 pixels, total composition 1920x1080.

## Acik Mesaj
Bu image **gosterimsel**: "AH, bizim pipeline bu 8 adimi sirayla yapar — sonuc v1 concept art'taki gibi olur."

Concept art idealize edilmis pixel art quality, gercek Unity render'i biraz farkli olabilir (PPU=64 tiles), AMA bu visual breakdown user'a **pipeline mantigi**'ni kanitliyor.

## Negative Direktifler
- NO labels INSIDE the panels (sadece footer'da panel name)
- NO different camera angles between panels (hepsi top-down 30-35°)
- NO bright cartoon
- NO 3D render
- NO grid lines on Panel 4-8 (sadece P1-P3'te grid kasitli)

## Cikti
`STAGING/concept_art_layer_breakdown.png` — 1920x1080 4x2 grid breakdown
`STAGING/codex_imagegen_layer_breakdown_DONE.md` — Codex transcript

## Iterasyon
Codex 1-2 iteration yapabilir kalite icin. Final result user'a sunulacak.
