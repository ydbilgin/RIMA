---
name: karar-150-act1-envanter-live
description: Act 1 Shattered Keep envanter LIVE 46 PNG (cloud 27 + fresh 19). v4 referans iskeleti karşılandı. Faz 2 gaps + anim user notu.
metadata: 
  node_type: memory
  type: project
  originSessionId: e9fd7f48-3db0-45d4-86d2-bd8a5101b3c7
---

**2026-05-19 S95 LIVE:** Act 1 envanter v4 referans iskeleti hazır. Karar #150 LIVE + temiz local pack. Eski painterly_* envanter (63 PNG) archive'a alındı.

## Local envanter (46 PNG)

```
Assets/Art/AssetPacks/Act1_ShatteredKeep/
├── arches/                  2  arch_entry + arch_exit (cyan rift)
├── floor_tiles/
│   ├── wang/                4  Wang sheets (keep floor+wall, rift floor, wall→path, floor→moss)
│   ├── granite_base/       16  4-material × 4 variant (granite + worn stone + cave moss + mud)
│   └── iso/                 3  granite isometric (clean, worn, chiseled)
├── pillars/                 3  broken + intact cyan crack + chained
├── props/                   5  rubble heap + rubble debris + cage + brazier orange + brazier cyan
├── rift_accents/            3  fracture overlay + wall rift vertical + horizontal
├── scatter/                 1  skull pile cluster
└── wall_decoration/         9  candle bracket + torch sconce + chain×2 + skeleton×2 + banner×3
```

Empty folders (Faz 2): `decals/`, `accents/`, `walls/`, `wall_blocks/`, `gates/`, `patches/`

## Karar #150 v4 referansa eşleşme

| v4 referans öğesi | Local karşılığı |
|---|---|
| Fake-iso stacked granite wall | Wang sheet `act1_keep_floor_wall_pair_sheet.png` (slice gerek) |
| Arch portal cyan rift glow | `arches/act1_arch_entry_cyan_rift_v01.png` + exit variant |
| Free-standing intact pillar | `pillars/act1_pillar_intact_cyan_crack_v01.png` |
| Broken pillar stub | `pillars/act1_pillar_broken_granite_v01.png` (cloud) |
| Wall-mounted decoration | `wall_decoration/` 9 PNG (chain, skeleton, banner, candle, torch) |
| Wall rift glow | `rift_accents/wall_rift_*.png` (vertical + horizontal) |
| Brazier point light | `props/brazier_orange + brazier_cyan` |
| Cobble floor cool granite | `floor_tiles/granite_base/` 4-mat × 4 var + iso 3 |
| Rubble heap with skulls | `props/rubble_heap_skulls + rubble_debris` |
| Skull scatter | `scatter/skull_pile_cluster` |

## Eksik (Faz 2 gen ihtiyacı)

| Kategori | Detay | Boyut | Hedef |
|---|---|---|---|
| **L4 large patches** | Cave moss / Dust drift / Cracked rubble — Act 1 cool granite palette | 128-192px | ~12 (4 per type) `create_map_object` |
| **decals/** | Small crack + pebble + dust + bone — Act 1 desaturated | 32-64px | ~12 `create_object` |
| **walls/ wall_blocks/** | Wang sheet slice OR fresh fake-iso wall block gen | 32-128px | 4 Wang slice + opt fresh |
| **gates/** | Locked door / open archway (ZorDoor mechanic) | 96px | 2-3 |
| **accents/** floor hero | ritual_circle Act 1 + battle splatter + scorch | 64-96px | 6-8 |

## Aksiyonlar — sırayla

1. **Wang sheet slice** — 4 Wang sheet'i Unity'de slice et (32px grid), RuleTile setup
2. **Scene rebuild** — Spawn_01 entry_chamber + Spawn_02 pillar_arena bu envanterle compose (Karar #150 v4 hedefli, Karar #147 Multi-Layer Painter authored data)
3. **L4 patches fresh gen** — Cave moss × 4, Dust drift × 4, Cracked rubble × 4 (`create_map_object` 128-192px)
4. **decals + accents fresh gen** — ~15 PNG ek (`create_object` 32-64px)
5. **gates fresh gen** — 2-3 PNG (`create_object` 96px)

## Anim adayları — USER yapacak (V3 Web UI)

Orchestrator dispatch YASAK ([[object-state-animation-workflow-split]]). 10 sprite için flicker/pulse/sway:
- `brazier_orange_flame`, `brazier_cyan_rift_flame` (4-6 frame flicker)
- `wall_rift_vertical_glow`, `wall_rift_horizontal_glow` (6-8 pulse)
- `rift_fracture_overlay` (cloud f2ba1bed, 6-8 shimmer)
- `arch_entry / arch_exit` (inner rift pulse 6-8)
- `pillar_intact_cyan_crack` (4-6 crack pulse)
- `chain_hanging_long` (optional sway 4-6)
- `banner_*_torn` × 3 (optional cloth sway 4-6)

## Bütçe

- PixelLab: 2,820/5,000 gen (19 fresh × 20 = 380 gen used bu session)
- 27 cloud download maliyetsiz (Backblaze direct)

## PixelLab cloud isim convention

Tüm fresh gen `Act 1 Shattered Keep ...` ile başlar (library list'te prefix sıralı görünür). 19 yeni obje library'de canonical adlandırılmış.

## Archive

`Assets/Art/_archive_karar150_envanter_clean_S95/Act1_ShatteredKeep/` — 63 PNG eski painterly_* set. RIMA-specific deprecated, Karar #150 vizyon-uyumsuz. **1-2 hafta cooldown sonra DELETE** (cross-game için uygun değil, [[lauretstudio-library-strategy]]).

## POLISH BACKLOG (playable demo bittikten sonra)

User direktifi 2026-05-19: "demo yapmak istiyorum önce, güzelleştirme sonra".

- Tile grid hâlâ görünüyor (granite_base 64×64 PURE top-down) → Wang sheet slice + Hades-style overlay patch
- Banner/chain üst kenar dışına taşıyor → composition refine veya camera framing
- L4 Act1 patches gen (cave_moss / dust_drift / cracked_rubble × 4)
- L2 Act1 decals gen (~12 small floor scatter)
- L3 fake-iso wall_blocks (5 class × 3 var = 15, Wang sheet slice veya fresh gen)
- L6 floor accents gen (ritual_circle Act1, battle_splatter, scorch)
- Gates gen (locked door, open archway — ZorDoor mechanic için)
- Light2D intensity tuning (brazier point light glow)
- Anim USER V3 Web UI (10 env sprite + 10 char anchor)

## Bağlantılar

- [[karar-150-fake-isometric-lock]] — Karar #150 LIVE spec
- [[subroom-canonical-tags-lock]] — sub-room canonical tags
- [[lauretstudio-library-strategy]] — cloud-first library mantığı
- [[object-state-animation-workflow-split]] — anim user/orchestrator split
- [[asset-pack-organization-lock]] — Act1/Act2/Act3 hierarchy
- [[create-tiles-pro-4type]] — 4×4 batch tile pattern
