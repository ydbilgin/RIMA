# Wang Tileset Generation Queue — Shattered Keep F1 (v2 split workflow)
**Karar #134 LOCKED RIMA-canon production**
**Split:** Pro UI (USER) = new base textures + raggedness | Standard MCP (CLAUDE) = chained pairs

---

## Strateji — Pro UI base picker yok, çözüm: Hybrid split

Pro UI'da "use existing base tile" picker yok → her Pro üretimi **fresh texture** çıkarır. Tüm pair'leri Pro yaparsak aynı terrain'in 3-4 farklı texture versiyonu çıkar (rubble Pair A'da farklı, Pair B'de farklı → seam görünür).

**Çözüm:**
1. **Sadece YENİ BASE üretmek için Pro kullan** (her terrain için 1 kez)
2. **Aynı base'i tekrar kullanan pair'leri Standard MCP'de chain et** — `create_topdown_tileset` `lower_base_tile_id` + `upper_base_tile_id` parametreleriyle texture birebir kopyalanır

**Quality trade-off:**
- Pro pair'leri: raggedness/spread tam kontrol — organik boundary
- Standard pair'leri: discrete transition_size (0.25/0.5/1.0) ama **Pro-quality base texture'lar inherit edilir** — boundary tile'ları biraz daha geometric ama base'ler hand-painted hissini korur
- Same-family pair'ler için zaten kimse Wang üretmiyor (patch scatter)

---

## Production DAG

```
Pair 0 (DONE Pro)     path↔moss        → path_base ✓, moss_base ✓
Pro Pair A (USER)     rubble↔wall      → rubble_base + wall_base (TWO new bases, en kıymetli pair)
Pro Pair B (USER)     rubble↔rift      → rift_base (rubble duplicate — ignore, Pair A authoritative)
Standard Pair C (ME)  wall↔path        ← chain wall_base + path_base
Standard Pair D (ME)  wall↔rift        ← chain wall_base + rift_base
```

**Toplam:** 4 yeni pair (2 Pro + 2 Standard) + Pair 0 done = **5 pair Phase 1 MVP scope**

**Unique base inventory after generation:** path, moss, rubble, wall, rift = 5 canonical base tile (her terrain için tek texture)

**Senin iş yükün:** 2 Pro UI üretimi (~2 dk total)
**Benim iş yükün:** 2 MCP chain dispatch (~3-4 dk async, otomatik)

---

## ✓ Pair 0 — path ↔ moss (DONE Pro reference)

| Field | Value |
|---|---|
| Tileset ID | `b41919aa-d20c-441e-a812-67e1f25f3331` |
| **path_base UUID** | `3bdfb21d-c5c9-4a93-993a-6313f9b57d08` |
| **moss_base UUID** | `21223297-9461-4f62-945f-7366a47b90aa` |

---

## ⏳ Pro Pair A — rubble ↔ wall (USER, Pro UI)

**Niye Pro:** İki yeni base texture (rubble + wall) tek seferde — en kıymetli pair. Wall raggedness + rubble organik debris gerek.

### Tile Size
`32×32`

### Lower Terrain (rubble — YENİ canonical base)
```
Dark rubble stone floor for a shattered keep, 32x32 top-down pixel art tile viewed from approximately 35 degrees, uneven charcoal flagstones in muted #2C2A2A with worn cracked mortar, chipped slab edges, subtle cold blue shadow tint, sparse dust and tiny stone chips, hairline cyan rift cracks scattered asymmetrically, mat painterly pixel, dark gritty Shattered Keep palette, walkable flat ground, no characters, no props, tileable.
```

### Upper Terrain (wall — YENİ canonical base)
```
Dark broken stone wall mass for a ruined keep, 32x32 top-down pixel art tile viewed from ~35 degrees, raised collapsed masonry blocks in muted #4A3F3F with darker crevices and restrained cold-blue rim shadows RGB(123,167,188), ancient fortress stonework, heavy readable silhouette, dark gritty Shattered Keep palette, blocks movement, no characters, no props, tileable.
```

### Transition Description
```
Broken rubble seam between charcoal floor and raised wall masonry, loose dark stones spilling outward from the wall base, small debris piles, chipped flagstone edges, very subtle cyan-violet rift dust in tiny accents only, narrow readable transition, dark gritty palette.
```

### Sliders

| Slider | Value |
|---|---|
| **Transition Height** | **25%** |
| Transition (Adv) | 5% |
| Spread (Adv) | 25% |
| Raggedness (Adv) | 45% |

### After generation — bana ver

- Yeni tileset ID: `_____________________________`
- **rubble_base UUID:** `_____________________________`
- **wall_base UUID:** `_____________________________`

---

## ⏳ Pro Pair B — rubble ↔ rift (USER, Pro UI)

**Niye Pro:** Rift_base canonical. Cyan-violet hairline fracture'lar organik olmalı → raggedness yüksek. Rubble duplicate olacak — kullanmıyoruz, sadece rift_base önemli.

### Tile Size
`32×32`

### Lower Terrain (rubble — duplicate, IGNORE)
```
Dark rubble stone floor for a shattered keep, 32x32 top-down pixel art tile, uneven charcoal flagstones in muted #2C2A2A with worn cracked mortar, chipped slab edges, dark gritty Shattered Keep palette, walkable flat ground.
```

### Upper Terrain (rift — YENİ canonical base)
```
Rift-fractured stone floor variant for a shattered keep, 32x32 top-down pixel art tile viewed from ~35 degrees, dark warm charcoal-grey RGB(44,42,42) base broken by cold blue cyan RGB(123,167,188) and violet RGB(110,90,140) hairline rift fracture overlay, weathered ruined keep floor with magical seam tearing through stone, matte desaturated palette with sparse cyan-violet thread accent, dark gritty serious Shattered Keep mood, walkable flat ground at same elevation as rubble, no characters, no props, tileable.
```

### Transition Description
```
Cold blue cyan rift cracks splitting through ancient flagstones, hairline fracture pattern spreading from the rift boundary onto adjacent rubble, very subtle violet shimmer at the seam edges, magical fracture seam with no height change, asymmetric organic spread, dark gritty palette, Karar 98 rift cyan and violet accent.
```

### Sliders

| Slider | Value | Niye |
|---|---|---|
| **Transition Height** | **5%** | Rift düz, sadece doku farkı |
| Transition (Adv) | 0% | Sharp seam (rift dramatik) |
| Spread (Adv) | 35% | Cyan fade outward |
| Raggedness (Adv) | 50% | Organik fracture |

### After generation — bana ver

- Yeni tileset ID: `_____________________________`
- **rift_base UUID:** `_____________________________`
- (rubble_base'i kullanmıyoruz — Pro Pair A'nınki canonical)

---

## 🤖 Standard Pair C — wall ↔ path (CLAUDE, MCP chain)

**Bana iletmen gerekenler:**
- Pro Pair A'dan **wall_base UUID** ✓
- Pair 0'dan **path_base UUID** `3bdfb21d-c5c9-4a93-993a-6313f9b57d08` ✓

**Ben otomatik fire ederim:**
```python
mcp__pixellab__create_topdown_tileset(
    lower_description = "Dark broken stone wall mass for a ruined keep...",
    upper_description = "Worn stone walkway path...",
    lower_base_tile_id = "<wall_base from Pro Pair A>",
    upper_base_tile_id = "3bdfb21d-c5c9-4a93-993a-6313f9b57d08",
    transition_size = 0.25,
    transition_description = "Broken stone debris where wall masonry meets walkway, fallen blocks at the boundary, dust accumulation in the crevice...",
    view = "high top-down",
    detail = "highly detailed",
    outline = "selective outline",
    shading = "detailed shading",
    text_guidance_scale = 10,
    tile_strength = 1.2,
    tileset_adherence = 250,
    tileset_adherence_freedom = 350,
)
```

**Sonuç:** Wall texture Pro-quality (Pro Pair A'dan inherit), path texture Pro-quality (Pair 0'dan inherit), boundary tile'ları Standard ama distinct-pair olduğu için OK.

---

## 🤖 Standard Pair D — wall ↔ rift (CLAUDE, MCP chain)

**Bana iletmen gerekenler:**
- Pro Pair A'dan **wall_base UUID** ✓ (tekrar)
- Pro Pair B'den **rift_base UUID** ✓

**Ben otomatik fire ederim:**
```python
mcp__pixellab__create_topdown_tileset(
    lower_description = "Dark broken stone wall mass for a ruined keep...",
    upper_description = "Rift-fractured stone floor variant...",
    lower_base_tile_id = "<wall_base from Pro Pair A>",
    upper_base_tile_id = "<rift_base from Pro Pair B>",
    transition_size = 0.25,
    transition_description = "Cold blue cyan rift cracks splitting through ancient wall masonry, fractured masonry blocks with magical seam where rift overlay meets raised wall surface, hairline cyan-violet fracture pattern, asymmetric organic boundary, the rift consuming the wall stone, dark gritty palette.",
    view = "high top-down",
    detail = "highly detailed",
    outline = "selective outline",
    shading = "detailed shading",
    text_guidance_scale = 10,
    tile_strength = 1.2,
    tileset_adherence = 250,
    tileset_adherence_freedom = 350,
)
```

---

## ⏳ Pro Pair E — rubble ↔ cliff_drop (USER, Pro UI) — vertical traversal/hazard

**Niye Pro:** Dramatik cliff edge raggedness şart. Hades-style broken stone cliff face. Phase 1 MVP'ye vertical dimension katar.

### Tile Size
`32×32`

### Lower Terrain (rubble — DUPLICATE, IGNORE — Pro Pair A canonical kullanılacak)
```
Dark rubble stone floor for a shattered keep, 32x32 top-down pixel art tile, uneven charcoal flagstones in muted #2C2A2A, dark gritty Shattered Keep palette, walkable flat ground.
```

### Upper Terrain (cliff_drop — YENİ canonical base)
```
Dark stone cliff face dropping into shadowed void for a shattered keep, 32x32 top-down pixel art tile viewed from approximately 35 degrees, broken collapsed cliff edge revealing fractured stone strata below in muted #1A1818 base value with darkest crevice voids #0A0A0A, vertical rim shadow casting downward into abyss, jagged stone teeth at the broken edge, hairline cyan rift dust accents at the fracture lines, ancient fortress floor torn away into rift below, dark gritty Salt-and-Sanctuary Shattered Keep palette, blocks movement, drop hazard, no characters, no props, tileable.
```

### Transition Description
```
Broken stone cliff edge where flagstone floor drops away into shadowed void below, jagged stone teeth at the fractured boundary, hairline cyan rift dust falling into the darkness, vertical rim shadow casting dramatically downward, ancient masonry torn open exposing the abyss beneath the keep, dark gritty palette, dramatic Hades-style vertical traversal edge.
```

### Sliders

| Slider | Value | Niye |
|---|---|---|
| **Transition Height** | **60%** | Dramatic vertical drop — Hades cliff signature |
| Transition (Adv) | 5% | Slight slope at the edge before drop |
| Spread (Adv) | 25% | Steep cliff, dramatic |
| Raggedness (Adv) | **55%** | Broken organic fracture edge — kritik |

### After generation — bana ver
- Yeni tileset ID: `_____________________________`
- **cliff_drop_base UUID:** `_____________________________`

### Gameplay metadata (TerrainDefinition için)
- `walkable: false`
- `blocksMovement: true` (uçuruma giremez)
- `elevationLevel: -1` (lower than floor)
- `collisionType: Hazard` (düşme hasarı + reposition)
- `isCliff: true`

---

## ⏳ Pro Pair F — rubble ↔ rift_pool (USER, Pro UI) — magical hazard water

**Niye Pro:** Stagnant magical water shoreline organik kıyı çizgisi raggedness şart. Karar #98 cyan-violet palette. F1 hazard surface variant.

### Tile Size
`32×32`

### Lower Terrain (rubble — DUPLICATE, IGNORE)
```
Dark rubble stone floor for a shattered keep, 32x32 top-down pixel art tile, uneven charcoal flagstones in muted #2C2A2A, dark gritty Shattered Keep palette, walkable flat ground.
```

### Upper Terrain (rift_pool — YENİ canonical base)
```
Stagnant magical rift pool surface for a shattered keep, 32x32 top-down pixel art tile viewed from approximately 35 degrees, dark fluid liquid surface in deep blue-black #0C1A28 with swirling cyan RGB(0,255,204) and violet RGB(90,42,138) magical currents on the surface, faint subtle ripples breaking the reflection of a charcoal stone ceiling above, occasional hairline rift wisps rising from the pool, ancient magical water collected in keep ruin, matte desaturated palette with bright cyan-violet accent only at swirl lines, dark gritty Salt-and-Sanctuary Shattered Keep palette, walkable false drowning hazard, no characters, no props, tileable.
```

### Transition Description
```
Magical rift water meeting flagstone at a ruined pool edge, wet flagstone darkening as the cyan-violet liquid laps the stone, small wet cyan glint at the immediate boundary, very fine ripple texture touching the floor, occasional hairline violet wisp rising at the seam, organic irregular shoreline, no height change but visual darkening, Karar 98 rift cyan and violet accent canon.
```

### Sliders

| Slider | Value | Niye |
|---|---|---|
| **Transition Height** | **0%** | Su düz, floor seviyesinde |
| Transition (Adv) | 0% | Sharp shoreline |
| Spread (Adv) | 40% | Gentle wet edge darkening fade |
| Raggedness (Adv) | **55%** | Organik kıyı şeridi — kritik (water'ın asıl raggedness use case'i) |

### After generation — bana ver
- Yeni tileset ID: `_____________________________`
- **rift_pool_base UUID:** `_____________________________`

### Gameplay metadata (TerrainDefinition için)
- `walkable: false`
- `blocksMovement: false` (yürüyebilir ama)
- `elevationLevel: 0` (floor level)
- `collisionType: Hazard` (drown / slow + DoT)
- `isWater: true`

---

## 🎯 Final canonical base inventory (üretim bitince)

| Terrain | Canonical base UUID | Source |
|---|---|---|
| path | `3bdfb21d-c5c9-4a93-993a-6313f9b57d08` | Pair 0 Pro |
| moss | `21223297-9461-4f62-945f-7366a47b90aa` | Pair 0 Pro |
| wall | `_____________________________` | Pro Pair A |
| rubble | `_____________________________` | Pro Pair A (B/E/F rubble duplicates IGNORE) |
| rift | `_____________________________` | Pro Pair B |
| cliff_drop | `_____________________________` | Pro Pair E (cliff hazard) |
| rift_pool | `_____________________________` | Pro Pair F (water hazard) |

→ Bunlar **TerrainDefinition.baseTile** alanına bağlanacak (Phase 1 Codex dispatch). Tek-terrain interior'larda her zaman bu canonical tile çekilir, pair-spesifik wang_0/wang_15 kullanılmaz (Karar #134 §F1 fix).

---

## 🔄 Workflow özet — adım adım

1. **Sen:** Pro Pair A (rubble↔wall) Pro UI'a yapıştır, slider'ları ayarla, Generate Pro
2. **Sen:** Bittiğinde "Pair A bitti" de bana
3. **Ben:** `list_topdown_tilesets` → yeni ID + `get_topdown_tileset` → rubble_base + wall_base UUID'leri çekerim, bu dosyaya yazarım
4. **Sen:** Pro Pair B (rubble↔rift) Pro UI'a yapıştır, slider'ları ayarla, Generate Pro
5. **Sen:** "Pair B bitti" de
6. **Ben:** rift_base UUID'sini çekerim, bu dosyaya yazarım
7. **Ben:** Standard Pair C (wall↔path) MCP chain ile fire ederim (~100 sn async)
8. **Ben:** Standard Pair D (wall↔rift) MCP chain ile fire ederim (~100 sn async, paralel)
9. **Ben:** Hepsi bitince `pixellab_pro_generation_log.md`'ye 5 entry yazarım (Pair 0/A/B/C/D slider state + UUID inventory)
10. **Ben:** PNG + JSON'ları `STAGING/pixellab_tilesets_dump/`'a indiririm, A/B karşılaştırma görseli yaparım
11. Sonraki: Codex Phase 1 dispatch'i bu 5 tileset + canonical base inventory ile fire edilir

**Total süre:** 2 dk Pro (sen) + 4 dk Standard (ben) + 1 dk log/ingest = **~7 dk tüm Phase 1 Wang asset pool**

---

## Notes

- **Same-family pair'ler bu queue'da yok** — pink↔cream, rubble↔moss-on-stone gibi same-family için Wang yerine create_map_object patch + create_tiles_pro variant scatter (Phase 1.5'te ayrı queue)
- **Slider preset'leri sabit değil** — duruma göre Pro UI'da deneyim ekleyebilirsin, ama bu değerler RIMA Shattered Keep canon için tuned
- **Standard pair'lerde transition_size = 0.25 sabit** (discrete API kısıtı). Boundary biraz geometric ama base'ler Pro-quality
- **Pro Pair B rubble texture ignored** — sadece rift_base alıyoruz, all-rubble corners (wang_0) Pro Pair A'dan
- **Reference image yok hiçbirinde** — chaining sadece base tile UUID üzerinden
