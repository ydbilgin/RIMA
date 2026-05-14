# Per-Biome PixelLab Prompt Templates

**Son güncelleme:** 2026-05-13 (S67)
**Strateji:** Karar #100 (cold F1) + NLM biome bağlamı + Layered composition (Karar #118 + Faz 1.5 DecalPainter)
**Pipeline:** Her biome için 4 batch — Base + 3 Decal (dirt/moss/crack veya biome'a özgü)

---

## Genel Kurallar (her biome aynı)

**Settings:**
- `tile_type: square_topdown`
- `tile_size: 32`
- `tile_view: "top-down"` (PURE, "low/high" değil)
- `tile_depth_ratio: 0` (no extrusion)
- `outline_mode: "segmentation"` (no outline)
- Numaralı format: `1). ... 2). ...` 16 tile için

**Universal STRICT REQUIREMENTS bloğu (her prompt'ta):**
```
- TOP-DOWN view ONLY, ZERO perspective
- NO walls, NO vertical surface, NO extruded edges
- NO baked highlights/shadows, NO directional lighting
- NO vignette, NO outline stroke
- NO geometric grid, NO brick/cobble/flagstone pattern
- Pure flat albedo only (URP 2D Lights add atmosphere at runtime)
- Organic asymmetric shapes, irregular boundaries
- Each tile fully tileable on all 4 edges
```

**Universal YASAK words (prompt'tan çıkarılacak — geometric pattern tetikliyor):**
- flagstone, slab, mortar, brick, cobble, masonry, cut-stone
- "tile" kelimesi (tile pattern'i çağrıştırıyor — "floor surface" kullan)
- regular, grid, pattern, uniform, repeating
- baked light, shadow cast, directional

---

## F1 Shattered Ruins (COLD granite)

**Tema:** Parçalanmış eski kale, soğuk taş + uzak rift sızıntısı
**Mood:** Salt and Sanctuary dark gritty + Hades cold mythic
**Karar #100 LOCKED:** Soğuk gri granit + buz mavisi, warm tone YASAK

### Palette (strict)
- Base stone: `#3A3D42` cool grey granite (DOMINANT)
- Deep shadow: `#252830` cold crevice
- Rim accent: `#7BA7BC` cold blue (subtle, microcracks only)
- Decal moss: `#5A6B5A` cool grey-green (warm yeşil YASAK)
- Decal rift: `#4FB8C4` cyan + `#7B5BAA` violet (rift accent)
- Decal dust: `#4A4845` cool grey-brown (warm brown YASAK)

### Batch A — F1-Base-Granite-PURE (16 tile)
Sade soğuk granit base, hiç feature yok. Micro texture variation only. (Dispatched: tile ID `51845163...`)

### Batch B — F1-Decal-Dirt (8 sprite, create_object)
```
Single transparent-background pixel art decal patch: small irregular dirt/dust accumulation, cool grey-brown #4A4845 tone, organic asymmetric shape, partial alpha edges, 32x32. NOT a tile, NOT seamless, isolated standalone object on transparent background. Top-down view. NO outline border. NO baked lighting.

8 variations:
1. small dirt patch corner accumulation
2. medium dirt patch center spread
3. dirt streak diagonal direction
4. dirt cluster scattered pebbles
5. dirt smudge wide thin
6. dirt pool concentrated
7. dirt drift trail
8. dirt splatter irregular
```

### Batch C — F1-Decal-Moss (8 sprite, create_object)
```
Single transparent-background pixel art decal patch: small grey-green lichen/moss cluster, cool tone #5A6B5A, organic irregular shape with partial alpha edges, 32x32. NOT a tile, isolated standalone object. Top-down. NO outline. NO baked lighting. NO warm green.

8 variations:
1. small moss patch corner
2. medium moss cluster center
3. moss strip elongated
4. moss with small fern tuft accent
5. moss creeping spread
6. moss patches scattered (2-3 small)
7. moss with damp shadow tint
8. moss heavy coverage central
```

### Batch D — F1-Decal-Rift (8 sprite, create_object)
```
Single transparent-background pixel art decal: thin cyan-violet rift crack #4FB8C4 + #7B5BAA hairline, sometimes with tiny silver rune dust particle accents, organic branching pattern, partial alpha, 32x32. NOT a tile, isolated. Top-down. NO outline. NO baked lighting.

8 variations:
1. thin straight rift crack diagonal
2. branching rift crack Y-shape
3. curved rift sweep
4. rift crack with rune dust scatter
5. small rift fragment cluster
6. rift hairline triple-branched
7. rift crack with violet bloom edge
8. rift dust + crack mixed
```

---

## F2 Bleeding Wastes (CORRUPTED PURPLE)

**Tema:** Yozlaşmış orman, kemik kalıntı, ritüel hapishane
**Mood:** Don't Starve macabre + Salt and Sanctuary dark, warmth-from-corruption
**Karar #100 LOCKED:** Derin mor bataklık + kızıl altın, jenerik "swamp green" YASAK

### Palette (strict)
- Base ground: `#3A2840` deep purple-grey bog (DOMINANT)
- Shadow crevice: `#1F1428` darker
- Warm accent: `#C8502A` rust gold ember (sparse)
- Decal moss: `#5A4870` corrupted violet-grey (NORMAL moss yeşil YASAK)
- Decal bone: `#A89880` weathered bone ivory (sparse fragments)
- Decal blood: `#5E2A35` dark dried crimson
- Decal root: `#3A2820` corrupted dark roots peeking

### Batch A — F2-Base-PurpleBog-PURE (16 tile)
**Prompt iskeleti:**
```
Top-down corrupted forest floor — UNIFORM purple-grey decayed substrate BASE layer for layered composition. NO bones, NO roots, NO blood, NO features — pure base for decals on top.

STRICT [universal block]

RIMA F2 Bleeding Wastes palette:
- Base substrate: #3A2840 deep purple-grey corrupted bog (DOMINANT)
- Shadow crevice: #1F1428 darker void shadow
- Rim accent: #C8502A rust gold ember hint (very subtle)
- NO bright green, NO yellow, NO warm brown, NO blood red — CORRUPTED PURPLE only

Mat painterly pixel art, Don't Starve macabre + Salt and Sanctuary dark gritty, corrupted forest floor that has consumed itself. Subtle organic texture variation (micro grain) but UNIFORM purple substrate — variety from MICRO texture differences.

1-16). uniform purple-grey corrupted substrate, subtle micro grain variation N, flat albedo, top-down [N=1..16]
```

### Batch B/C/D — F2 decals (create_object)
- F2-Decal-Bone (8): scattered ivory bone fragments, small skull, rib pieces
- F2-Decal-Root (8): corrupted dark roots peeking through ground, twisted
- F2-Decal-Blood (8): dried dark crimson splatter patterns

---

## F3 Core Approach (VOID + GOLD)

**Tema:** Kozmik, gerçeklik inceliyor, akkor altın tiles
**Mood:** Death's Door cosmic + Tunic temple, transcendental
**Karar #100 LOCKED:** Void siyah + akkor altın (incandescent), normal yer karoyu YASAK

### Palette (strict)
- Base void: `#0A0810` near-pure black void substrate (DOMINANT)
- Gold accent tile: `#FFD700` incandescent gold (sparse, ritual pattern only)
- Cold edge: `#3A4858` cool void-grey rim
- Decal star: `#E8DFC0` pale gold star fragments (rare)
- Decal rune: `#FFD700` + `#8B6914` carved sigil fragments
- Decal void-bleed: `#4F2A6B` violet sızıntı

### Batch A — F3-Base-Void-PURE (16 tile)
**Prompt iskeleti:**
```
Top-down cosmic void approach floor — UNIFORM near-pure black substrate with occasional incandescent gold ritual sigil hint BASE layer.

STRICT [universal block]

RIMA F3 Core Approach palette:
- Base void: #0A0810 near-pure black cosmic substrate (DOMINANT)
- Shadow: #000000 absolute void
- Gold accent: #FFD700 incandescent ritual hint (very rare, subtle)
- Cold rim: #3A4858 void-grey edge
- NO blue, NO purple, NO green, NO warm brown — VOID + GOLD only

Mat painterly pixel art, Death's Door cosmic + Tunic temple, transcendental floor where reality thins. UNIFORM void substrate, variety from MICRO gold sparkle differences.

1-16). uniform void-black substrate, subtle micro variation N, flat albedo, top-down [N=1..16]
```

### Batch B/C/D — F3 decals (create_object)
- F3-Decal-StarFragment (8): pale gold star pieces, cosmic dust
- F3-Decal-Rune (8): carved gold sigil fragments, half-erased
- F3-Decal-VoidBleed (8): violet sızıntı void seepage

---

## Üretim Sırası (öneri)

```
1. F1-Base-Granite-PURE (DISPATCHED)  ──→  QC PASS ──┐
2. F1-Decal-Dirt                                       │
3. F1-Decal-Moss                                       ├── Demo sahne F1 dolu
4. F1-Decal-Rift                                       │
                                                       ↓
                                              Unity Room Designer'da
                                              katmanlar test (Generate→Paint→Save)
                                                       ↓
5. F2-Base-PurpleBog-PURE ────→ PASS ─────────────────┐
6. F2-Decal-Bone                                       │
7. F2-Decal-Root                                       ├── Demo sahne F2 dolu
8. F2-Decal-Blood                                      │
                                                       ↓
9. F3-Base-Void-PURE ─────────→ PASS ─────────────────┐
10. F3-Decal-StarFragment                              │
11. F3-Decal-Rune                                      ├── Demo sahne F3 dolu
12. F3-Decal-VoidBleed                                 │
                                                       ↓
                                                  3-act full demo
```

**Toplam:** 12 batch (~3 base + 9 decal), ~240 kredi tahmini. Walls + props ayrı.

---

## YASAK / Hata Listesi (geçmiş batch derslerinden)

❌ Tek tile'da feature mix (clean+moss+rift birlikte) — AI heyecanlanır, dirt dominate eder
❌ "flagstone/slab/mortar" — brick grid tetikliyor
❌ "low top-down" — yan görünüm wall şeritleri sızdırıyor (top-down preset kullan)
❌ Warm brown tone F1 base'de (Karar #100 cold lock)
❌ Bright green moss (F1 grey-green, F2 violet-grey, yeşil yeşili YASAK)
❌ Outline mode "outline" (segmentation kullan)
❌ Decal'i create_tiles_pro ile (seamless zorlar — create_object kullan transparent BG için)
❌ 64-tile beklemek (32px = 16 max, PixelLab fixed)

---

## QC Checklist (her batch sonrası)

1. Top-down ✓ (yan görünüm/wall sızıntısı yok)
2. Organic asymmetric ✓ (grid/brick yok)
3. Palette discipline ✓ (biome'a uygun ton, jenerik renk yok)
4. Feature ayrımı ✓ (base'de sadece base feature, decal'de sadece decal)
5. Transparent BG ✓ (decal'ler için zorunlu)
6. Tileable ✓ (base'ler için 4-edge seamless)
7. NO baked lighting ✓ (URP 2D Light için temiz albedo)
