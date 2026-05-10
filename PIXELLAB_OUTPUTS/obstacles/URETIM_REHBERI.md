# Engel ve Prop'lar -- Uretim Rehberi
*MEMORY/pixellab_master_pipeline.md Bolum 0 HARD RULES uygulanir*

---

## TEMEL KURALLAR

- **Tool**: PixelLab web app -> Create S-L Image (Pro) -- TUM static objeler icin
- **YASAK**: Create Tiles Pro | Create Tile Isometric | S-XL/Pixflux (tek goruntu, batch workflow degil)
- **Transparent Background**: ON -- chromakey gerekmez, `process_tiles.py` ATLANIR
- **Pixel Art Mode**: ON | Anti-aliasing: OFF | Upscale: OFF
- **Candidate grid**: 4 candidates per call (Pro grid)
- **Style Reference**: W1 approved wall tile yukle -- ortak palette tutarliligi icin
- Pillar onaylananinca diger tum objelerde **Pillar'i style ref olarak yukle**
- **Output**: Her candidate'i ayri PNG olarak kaydet -- Pro grid sheet Unity'ye suruklenMEZ

### Animated Tile Kategorisi
Animated tile'lar (lava flow, su, vs.) **prop kategorisinde** kalir -- floor setine KARISMAZ.
- Floor F1/F2/F3 setleri: statik tile only. WeightedRandomTile / Rule Tile sistemi animated tile'i desteklemez.
- Torch animasyonu (ADIM 4) bu kuralin ornegi: prop layer'da, Ground tilemap'te degil.
- F3 hero tier'da animated gorunen "lava vein" istegi varsa -> ayri prop sprite, overlay layer'da.

---

## PALETTE

```
Shadow/outline:  #1A1C20
Dark stone:      #2A2D34
Mid stone:       #3A3D48
Lit face:        #4E5260
Highlight:       #606575

Wood:            #3A2818 / #5A4028 / #7A5838
Iron:            #282830 / #3A3A45
Flame:           #C84000 / #FF6800 / #FFAA00  (sadece torch)
Bone:            #C8C0A8 / #8A8070 / #5A5048
Lichen:          #263530
```

---

## ASSET TANIMI

| # | Obje | Canvas | Boyut (logical) | View | Var | Pro calls |
|---|---|---|---|---|---|---|
| 1 | Pillar | 256px | 32x64px | Low top-down | 4 | 1 |
| 2 | Rubble cluster | 128px | 64x48px | High top-down | 16 | 4 |
| 3 | Wall torch (static) | 64px | 16x32px | Low top-down | 8 | 2 |
| 4 | Wall torch (animasyon) | -- | -- | -- | 1 anim set | Animate Object |
| 5 | Floor crack decal | 128px | 64x64px | High top-down | 16 | 4 |
| 6 | Barrel / Crate | 128px | 32x32px | Low top-down | 8 | 2 |
| 7 | Bone pile | 128px | 48x32px | High top-down | 8 | 2 |
| 8 | Broken pillar stump | 128px | 32x32px | Low top-down | 8 | 2 |
| 9 | Large altar | 256px | 64x64px | Low top-down | 4 | 1 |

**Uretim sirasi**: 1 -> 2 -> 3 -> 4 -> 5 -> 6 -> 7 -> 8 -> 9

---

## ADIM 1 -- Pillar (Stil Anchor)

> Ilk uretilen obje -- tum projenin stil referansi olacak. Onaylandiktan sonra kalan tum obje calllarinda style ref olarak yukle.

**Settings**

| Param | Deger |
|---|---|
| Tool | Create S-L Image (Pro) |
| Canvas | 256px |
| View | Low top-down |
| Background | Transparent ON |
| Pixel Art Mode | ON |
| Candidates | 4 |
| Style Ref | W1 approved wall tile |

**Prompt** (copy-paste)
```
Isometric pixel art stone pillar prop, 32x64 pixels. Pure transparent background.
Viewed from 2:1 isometric angle, pillar stands vertically. Three sections:
decorative capital (top 12px) with simple carved detail, smooth shaft (middle 40px)
with subtle stone texture, square base (bottom 12px) slightly wider.
Palette: #1A1C20 (shadow left face), #2A2D34 (dark face), #3A3D48 (front face),
#4E5260 (lit right face), #606575 (top cap highlight). Two-face isometric shading:
left face darker, right face lit. Hard pixel edges. No gradient.
4 variations: intact, cracked shaft, moss-streaked (#263530 max 6px), damaged capital.
```

**Unity Import**
- Klasor: `Assets/Art/Props/Act1/pillar/`
- Sprite Mode: Single
- Pivot: Bottom Center (0.5, 0.0)
- PPU: 64
- Sorting layer: Entities (Y-sort aktif)
- Collider: Polygon (Physics Shape)
- Direkt Unity'ye drag-drop (process_tiles.py gerekmez)

**Output**: `outputs/pillar/pillar_v1_var01.png` ... `pillar_v1_var04.png`

---

## ADIM 2 -- Rubble Cluster

> Pillar onaylandi -> style ref olarak yukle.

**Settings**

| Param | Deger |
|---|---|
| Tool | Create S-L Image (Pro) |
| Canvas | 128px |
| View | High top-down |
| Background | Transparent ON |
| Pixel Art Mode | ON |
| Candidates | 4 |
| Style Ref | Approved pillar PNG |
| Pro calls | 4 (4 x 4 = 16 variant) |

**Prompt** (copy-paste -- her 4 callda ayni prompt kullan, farkli seed ile)
```
Isometric pixel art rubble and broken stone debris cluster, 64x48 pixels.
Pure transparent background. Scattered broken stone fragments viewed from
2:1 isometric angle, irregular silhouette. Stones have same palette as
dungeon walls: #1A1C20, #2A2D34, #3A3D48, #4E5260. Fragments range from
4px to 16px in length, randomly angled. Dust/gravel particles 1-2px dots
around edges (#2E2E3A). Hard pixel edges. No gradient, no smooth shading.
16 variations: different fragment arrangements, sizes, and scatter patterns.
```

**Unity Import**
- Klasor: `Assets/Art/Props/Act1/rubble/`
- Sprite Mode: Single
- Pivot: Bottom Center (0.5, 0.0)
- PPU: 64
- Sorting layer: Entities (Y-sort aktif)
- Collider: Polygon (Physics Shape)
- Direkt Unity'ye drag-drop (process_tiles.py gerekmez)

**Output**: `outputs/rubble/rubble_v1_var01.png` ... `rubble_v1_var16.png`

---

## ADIM 3 -- Wall Torch (Static)

> Static sprite. Animasyon ADIM 4'te ayri yapilacak.

**Settings**

| Param | Deger |
|---|---|
| Tool | Create S-L Image (Pro) |
| Canvas | 64px |
| View | Low top-down |
| Background | Transparent ON |
| Pixel Art Mode | ON |
| Candidates | 4 |
| Style Ref | Approved pillar PNG |
| Pro calls | 2 (2 x 4 = 8 variant) |

**Prompt** (copy-paste -- her 2 callda ayni prompt, farkli seed)
```
Isometric pixel art wall-mounted torch prop, 16x32 pixels. Pure transparent
background. Torch viewed from 2:1 isometric side. Iron bracket (bottom 10px):
#282830, #3A3A45. Wooden handle (middle 12px): #3A2818, #5A4028. Flame head
(top 10px): #C84000 base, #FF6800 mid flame, #FFAA00 hot tip -- flame is
8px wide tapering to 4px, 3-frame implied motion captured in still.
Hard pixel edges. No gradient outside the flame. 8 variations: flame slightly
different shapes (left-leaning, right-leaning, tall, wide, dimmer versions
with less #FFAA00).
```

**Unity Import**
- Klasor: `Assets/Art/Props/Act1/torch/`
- Sprite Mode: Single
- Pivot: Bottom Center (0.5, 0.0)
- PPU: 64
- Sorting layer: Entities (Y-sort aktif)
- Collider: Polygon (Physics Shape)
- Direkt Unity'ye drag-drop (process_tiles.py gerekmez)

**Output**: `outputs/torch/torch_static_v1_var01.png` ... `torch_static_v1_var08.png`

**Sonraki adim**: En iyi static torch sprite'i se -> ADIM 4 animasyon girdisi olarak kullan.

---

## ADIM 4 -- Wall Torch Animasyonu (Animate Object)

> Bu adim PixelLab web app Objects bolumunu kullanir -- Create S-L Image (Pro) DEGIL.
> Girdi: ADIM 3'te onaylanan en iyi static torch PNG.

**Settings**

| Param | Deger |
|---|---|
| Tool | PixelLab web app -> Objects -> Animate Object |
| Girdi | En iyi static torch sprite (ADIM 3 output) |
| Frame sayisi | 4-6 frame loop |
| Loop | ON |

**Prompt** (copy-paste)
```
Wall torch flame flicker loop. Candle-like warm orange-yellow flame
(#C84000 / #FF6800 / #FFAA00) flickers in place. Stone wall bracket static.
Short 4-frame loop. Flame tip dances left-right 1-2px each frame.
Flame base stays anchored to handle top. Iron bracket and wooden handle:
zero motion throughout. No smoke particles. Hard pixel edges. No anti-aliasing.
```

**Unity Import -- Animator Controller**
- Klasor: `Assets/Art/Props/Act1/torch/`
- Sprite Mode: Multiple (frame sheet) veya Individual PNGs
- PPU: 64
- Pivot: Bottom Center (0.5, 0.0)
- Sorting layer: Entities (Y-sort aktif)
- Animator Controller: `TorchFlicker`
  - State: `Flicker` -- loop true
  - Sample rate: 8-12 fps (alev donme hissi vermeden)
  - Motion: torch_flicker frame sequence

**Output**: `outputs/torch/torch_anim_frames/` (4-6 PNG frame) veya `torch_anim_sheet.png`

---

## ADIM 5 -- Floor Crack Decal

**Settings**

| Param | Deger |
|---|---|
| Tool | Create S-L Image (Pro) |
| Canvas | 128px |
| View | High top-down |
| Background | Transparent ON |
| Pixel Art Mode | ON |
| Candidates | 4 |
| Style Ref | Approved pillar PNG |
| Pro calls | 4 (4 x 4 = 16 variant) |

**Prompt** (copy-paste -- her 4 callda ayni prompt, farkli seed)
```
Isometric pixel art floor crack decal overlay, 64x64 pixels. Pure transparent
background fills ALL pixels except the crack itself. The crack is a single
thin irregular fracture line, 1-3px wide, crossing the 2:1 isometric diamond
area diagonally. Crack color: #1A1C20 (black crack) with #2A2A30 edge pixels.
NO stone texture -- this overlays on top of floor tiles as a transparent decal.
16 variations: different diagonal directions, lengths (half-diamond to
full-crossing), branching vs straight. Crack never touches the diamond edges
(leave 4px buffer) so it floats naturally on floor.
```

**Unity Import**
- Klasor: `Assets/Art/Props/Act1/crack/`
- Sprite Mode: Single
- Pivot: Center (0.5, 0.5)
- PPU: 64
- Sorting layer: Ground (floor tile ustunde, Entities altinda)
- Collider: YOK (decal overlay -- fiziksel engel degil)
- Direkt Unity'ye drag-drop (process_tiles.py gerekmez)

**Output**: `outputs/crack/crack_v1_var01.png` ... `crack_v1_var16.png`

---

## ADIM 6 -- Barrel / Crate

**Settings**

| Param | Deger |
|---|---|
| Tool | Create S-L Image (Pro) |
| Canvas | 128px |
| View | Low top-down |
| Background | Transparent ON |
| Pixel Art Mode | ON |
| Candidates | 4 |
| Style Ref | Approved pillar PNG |
| Pro calls | 2 (2 x 4 = 8 variant -- 4 barrel + 4 crate) |

**Prompt** (copy-paste)
```
Isometric pixel art storage barrel/crate, 32x32 pixels. Pure transparent background.
Viewed from 2:1 isometric angle showing top face and front face.
Barrel variant (4 vars): wooden staves #3A2818 / #5A4028, iron bands
#282830 / #3A3A45, lid top face slightly lighter.
Crate variant (4 vars): wooden plank box, nail heads visible as 1px dots,
iron corner brackets.
Isometric shading: top face lightest, front face mid, left face darkest.
Hard pixel edges, no gradient. Each variant shows different damage level:
intact, cracked, broken open, charred.
```

**Unity Import**
- Klasor: `Assets/Art/Props/Act1/barrel_crate/`
- Sprite Mode: Single
- Pivot: Bottom Center (0.5, 0.0)
- PPU: 64
- Sorting layer: Entities (Y-sort aktif)
- Collider: Polygon (Physics Shape)
- Direkt Unity'ye drag-drop (process_tiles.py gerekmez)

**Output**: `outputs/barrel_crate/barrel_v1_var01..04.png` | `crate_v1_var01..04.png`

---

## ADIM 7 -- Bone Pile

**Settings**

| Param | Deger |
|---|---|
| Tool | Create S-L Image (Pro) |
| Canvas | 128px |
| View | High top-down |
| Background | Transparent ON |
| Pixel Art Mode | ON |
| Candidates | 4 |
| Style Ref | Approved pillar PNG |
| Pro calls | 2 (2 x 4 = 8 variant) |

**Prompt** (copy-paste -- her 2 callda ayni prompt, farkli seed)
```
Isometric pixel art scattered bones and skull pile, 48x32 pixels.
Pure transparent background. Viewed from 2:1 isometric top-down angle.
Bones: off-white #C8C0A8, shadow side #8A8070, crack lines #5A5048.
Small skull visible in half the variations (12px diameter, simplified --
2 dark eye socket pixels). Irregular silhouette. Hard pixel edges.
8 variations: different bone arrangements, with/without skull, different
pile density.
```

**Unity Import**
- Klasor: `Assets/Art/Props/Act1/bone_pile/`
- Sprite Mode: Single
- Pivot: Bottom Center (0.5, 0.0)
- PPU: 64
- Sorting layer: Entities (Y-sort aktif)
- Collider: Polygon (Physics Shape)
- Direkt Unity'ye drag-drop (process_tiles.py gerekmez)

**Output**: `outputs/bone_pile/bone_pile_v1_var01.png` ... `bone_pile_v1_var08.png`

---

## ADIM 8 -- Broken Pillar Stump

**Settings**

| Param | Deger |
|---|---|
| Tool | Create S-L Image (Pro) |
| Canvas | 128px |
| View | Low top-down |
| Background | Transparent ON |
| Pixel Art Mode | ON |
| Candidates | 4 |
| Style Ref | Approved pillar PNG (kaliplari ayni olmali) |
| Pro calls | 2 (2 x 4 = 8 variant) |

**Prompt** (copy-paste -- her 2 callda ayni prompt, farkli seed)
```
Isometric pixel art broken stone pillar stump, 32x32 pixels.
Pure transparent background. Lower half of a pillar after collapse --
base (12px) intact, shaft (20px) sheared off at irregular angle.
Palette: #1A1C20, #2A2D34, #3A3D48, #4E5260. Top fracture surface shows
broken stone texture (lighter, exposed core #4E5260 with cracks).
Hard pixel edges. 8 variations: different fracture angles, varying degrees
of damage, occasional moss patch (#263530).
```

**Unity Import**
- Klasor: `Assets/Art/Props/Act1/broken_pillar/`
- Sprite Mode: Single
- Pivot: Bottom Center (0.5, 0.0)
- PPU: 64
- Sorting layer: Entities (Y-sort aktif)
- Collider: Polygon (Physics Shape)
- Direkt Unity'ye drag-drop (process_tiles.py gerekmez)

**Output**: `outputs/broken_pillar/broken_pillar_v1_var01.png` ... `broken_pillar_v1_var08.png`

---

## ADIM 9 -- Large Altar (Altar of Resonance)

> Son uretim adimi -- gameplay node. Cyan glow accenti vardir (diger objelerde yoktur).

**Settings**

| Param | Deger |
|---|---|
| Tool | Create S-L Image (Pro) |
| Canvas | 256px |
| View | Low top-down |
| Background | Transparent ON |
| Pixel Art Mode | ON |
| Candidates | 4 |
| Style Ref | Approved pillar PNG |
| Pro calls | 1 (1 x 4 = 4 variant) |

**Prompt** (copy-paste)
```
Isometric pixel art ritual altar prop, 64x64 pixels. Pure transparent background.
Three-tiered stone altar viewed from 2:1 isometric angle. Wide square base
(bottom 20px), narrower middle tier (middle 20px), top altar surface with
central recess (top 24px). Palette: #1A1C20, #2A2D34, #3A3D48, #4E5260, #606575.
Cyan rift glow accent in central recess: #00FFCC max 8 pixels (faint pulsing inlay).
Hard pixel edges, no gradient outside cyan glow. Two-face shading.
4 variations: intact altar, weathered (chipped corners), runic glow stronger
(cyan crack web on top), partially collapsed (middle tier missing).
```

**Unity Import**
- Klasor: `Assets/Art/Props/Act1/altar/`
- Sprite Mode: Single
- Pivot: Bottom Center (0.5, 0.0)
- PPU: 64
- Sorting layer: Entities (Y-sort aktif)
- Collider: Polygon (Physics Shape)
- Direkt Unity'ye drag-drop (process_tiles.py gerekmez)

**Output**: `outputs/altar/altar_v1_var01.png` ... `altar_v1_var04.png`

---

## QC CHECKLIST

**Her obje icin:**
- [ ] Background tam transparent (alpha 0, her spritenin etrafinda)
- [ ] Tool Create S-L Image (Pro) kullanildi (S-XL/Pixflux degil)
- [ ] Her candidate ayri PNG olarak kaydedildi (Pro grid sheet Unity'ye suruklenMEDI)
- [ ] Palette shared listesinden tasmiyor
- [ ] Pixel cluster min 4px (tek pixel noise yok)
- [ ] Hard pixel edge -- anti-aliasing yok
- [ ] process_tiles.py kullanilmadi (transparent bg zaten temiz)

**View kontrolu:**
- [ ] Low top-down: 3D objeler icin (Pillar, Torch, Barrel/Crate, Broken Pillar, Altar) -- top + front face gorunur
- [ ] High top-down: flat objeler (Rubble, Crack, Bone Pile) -- sadece ust gorunur

**Obje-ozel:**
- [ ] Pillar/Altar: bottom center pivot ground'a oturuyor
- [ ] Rubble/Crack/Bone: irregular silhouette dogru
- [ ] Torch (static): flame 3-frame implied motion hissi var (still frame)
- [ ] Torch (anim): 4-6 frame loop, bracket/handle hareket etmiyor
- [ ] Floor crack: alpha=0 dolgu alan, sadece catlak cizgileri opak
- [ ] Altar: cyan glow (#00FFCC) max 8px -- diger objelerde cyan YOK
- [ ] Broken pillar stump: pillar ile ayni base palette (stil tutarliligi)

**Animated prop / tilemap siniri:**
- [ ] Animated prop sprite'lar Ground tilemap'te degil, Entities layer'da
- [ ] Floor setlerinde animated tile variant bulunmuyor

**Unity:**
- [ ] PPU: 64
- [ ] Pivot: Bottom Center (0.5, 0.0) -- crack haric (Center)
- [ ] Sorting layer: Entities (crack icin Ground)
- [ ] Collider: Polygon Physics Shape -- crack haric (collider yok)

---

## KAYIT KLASORU

```
PIXELLAB_OUTPUTS/obstacles/outputs/
  pillar/              pillar_v1_var01..04.png
  rubble/              rubble_v1_var01..16.png
  torch/
    torch_static_v1_var01..08.png
    torch_anim_frames/   (4-6 PNG frame)
  crack/               crack_v1_var01..16.png
  barrel_crate/        barrel_v1_var01..04.png | crate_v1_var01..04.png
  bone_pile/           bone_pile_v1_var01..08.png
  broken_pillar/       broken_pillar_v1_var01..08.png
  altar/               altar_v1_var01..04.png

Assets/Art/Props/Act1/
  pillar/ rubble/ torch/ crack/ barrel_crate/ bone_pile/ broken_pillar/ altar/
```

**Toplam**: 9 obje tipi | 88 static sprite + 4-6 anim frame | 19 Pro call + 1 Animate Object call
