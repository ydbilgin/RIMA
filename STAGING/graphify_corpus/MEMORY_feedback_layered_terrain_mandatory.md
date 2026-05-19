---
name: layered-terrain-mandatory
description: "PlayableRoom 8-layer painted top-down (Hades + Alabaster Dawn canonical) zorunlu. Macro fill → base floor → mid-tone → detail → scatter → medium → tall focal → atmospheric. \"Boş cell siyah görünme\" YASAK, \"skulls floating in void\" YASAK."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: 2beacb18-e0f5-45c2-8ce5-d1b7fd2c9826
---

# Layered Terrain Mandatory — 8-Layer Painted Top-Down

**Kural:** Tüm PlayableRoom **8-layer painted recipe** ile inşa edilir (Hades + Alabaster Dawn + Octopath canonical). Tek "props scatter" YASAK — her layer kendi rolünü oynar.

| # | Layer | Coverage | Sorting | Per-zone asset |
|---|---|---|---|---|
| **1** | **Macro ambient fill** — painterly room background, big sweeping shapes | %100 | -100 | Layer 1 imagegen (zone-themed macro: stone-vault / forest-canopy / sandy-arena / blood-altar / cave-void / water-pool) |
| **2** | **Base floor tile** — cell-aligned biome tile, asla siyah görünmez | %100 | -90 | BiomeFloor_* (path=Sandy / grass=Mossy / stone=Cave / wall=Cave-dark / water=pool_water tile / feature=Blood) |
| **3** | **Mid-tone gradient overlay** — subtle color region patches | %30-50 | -80 | AtmosphericAccents + new gradient imagegen if needed |
| **4** | **Detail texture** — cracks, mossy patches, dirt stains | %30 | -70 | Moss, Dirt, Pebbles, Cracks (RIMA_v2_Pack) |
| **5** | **Small scatter** — pebbles, grass tufts, dried leaves, footprints | %40 | -60 | Yeni grass tufts (Phase B-3 imagegen), transitions, small Pebbles |
| **6** | **Medium props** — rocks, bushes, debris piles | %15 | YPosition | Pebbles big variants, AssetParts_v2 medium, transition blobs |
| **7** | **Tall focal** — statues, banners, columns, ritual circles | %5 (cap 1-2/region) | YPosition | Walls, VerticalProps, Ritual, Rift |
| **8** | **Atmospheric overlay** — god rays, fog, embers, ambient particles, fog-edges | %10-30 | +100 | Layer 8 imagegen (atmospheric: god-rays / smoke / embers / blood-mist / dust-motes / water-mist) |

**Per-zone layer recipe örnek (path zone):**
- L1: warm-tan macro fill big sprite
- L2: BiomeFloor_Sandy tile per-cell
- L3: subtle warm gradient patches (sparse)
- L4: dirt stains, footprint marks
- L5: small pebbles, dried grass tufts
- L6: debris piles, broken cobble
- L7: lanterns, signposts (1-2 per region max)
- L8: dust motes, warm ambient haze

**Why:** User feedback 2026-05-18 sabah ("playable room saçma duruyor, en altı kaplayan bir şey olacak üstüne bir kaplama daha olacak demek ki yerde siyah siyah görünmeyecek onun üstüne diğer kaplamalar olacak"). Sonraki feedback: "Hades ve Alabaster Dawn tarzına uygun 6 layerlı 8 layerlı mantıklı boyamayı düşün, temelde bir hata yapıyoruz". v15b screenshot: cobble/skulls var ama altta floor tile yok → dark scene background sızıyor. Profesyonel painted top-down (Hades, Alabaster Dawn, Octopath, CrossCode) hep 6-8 layer kullanır.

**How to apply:**
- **BlueprintZoneTypeSO schema MAJOR refactor** — 8 layer pool field:
  - `Sprite[] macroFillSprites` (Layer 1, full-cell or multi-cell)
  - `Sprite[] baseFloorSprites` (Layer 2, per-cell tile)
  - `BlueprintPropPoolSO midToneOverlayPool` (Layer 3)
  - `BlueprintPropPoolSO detailTexturePool` (Layer 4)
  - `BlueprintPropPoolSO smallScatterPool` (Layer 5)
  - `BlueprintPropPoolSO mediumPropPool` (Layer 6)
  - `BlueprintPropPoolSO tallFocalPool` (Layer 7) + `int maxTallFocalPerRegion = 2`
  - `BlueprintPropPoolSO atmosphericPool` (Layer 8) + `[Range(0,1)] float atmosphericDensity = 0.2f`
- **AutoPopulator 8-pass execute** — `PopulateZones` chains 8 single-layer methods sequentially with explicit sorting order
- **Sorting Layer registry** — ensure Unity 2D Sorting Layers contain "BlueprintL1" through "BlueprintL8" OR use sortingOrder integers (-100, -90, -80, -70, -60, YPos, YPos, +100)
- **Cell-aligned placement** — Layer 1+2 use cell-center pivot (1×1 unit), Layer 3-8 free position (sub-cell jitter OK)
- **Imagegen gaps** — Layer 1 macro fill (6 zone × 2 variant = 12 sprites) + Layer 8 atmospheric (8 sprites). Separate imagegen dispatch.
- **v15c rebuild** — atılır v14/v15/v15b artık DEPRECATED, v15c = new canonical with 8-layer fill
- **BlueprintProfileSO migration** — existing profile_combat_room_default.asset 6 zone × 8 layer = 48 pool field assignment needed (mevcut data layer 6-7'ye mapping)

**Sorting validation:**
```
L1 macro:   SR.sortingOrder = -100 (or layer "Background")
L2 floor:   SR.sortingOrder = -90  (or layer "Floor")
L3 midtone: SR.sortingOrder = -80
L4 detail:  SR.sortingOrder = -70
L5 scatter: SR.sortingOrder = -60
L6 medium:  SR.sortingMode = YPosition (PropSortingMode.YPosition existing)
L7 focal:   SR.sortingMode = YPosition
L8 atmos:   SR.sortingOrder = +100 (above all, fog top)
```

Related: [[blueprint-first-map-design]] [[brush-v1-manual-composition-system]] [[karar-143-layered-pipeline]] [[hybrid-asset-pipeline-lock]]
