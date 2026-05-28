# RIMA Asset Pack Organization Plan (S94 morning)

**Goal:** Asset Pack Browser (Map Designer tool) doğru klasör yapısıyla beslensin. Her Act'in kendi pack'i, paylaşılan asset'ler ortak pack'te. Gereksiz pack YOK.

## Klasör hiyerarşisi (önerilen)

```
Assets/Art/AssetPacks/
├── _Universal/              ← TÜM act'lerde kullanılan generic asset'ler
│   ├── floor_decals/        (small chips, dust specks — biome-agnostic)
│   ├── small_stones/        (generic pebbles — neutral palette)
│   └── vfx_atomic/          (sparks, dust puff — color-tintable)
├── Act1_ShatteredKeep/      ← F1 specific
│   ├── floor_tiles/         (cool granite, worn stone path, mud crust)
│   ├── walls/               (cool granite block wall caps, Hades 35° style)
│   ├── gates/               (iron-banded granite arch, closed/open variants)
│   ├── patches/             (cool cave moss, dust drift, ash smear, mud crust)
│   ├── scatter/             (cool granite rubble, weathered chip, ritual fragment)
│   └── rift_accents/        (cyan/violet seepage, cracks, faint residue)
├── Act2_BleedingWastes/     ← F2 specific
│   ├── floor_tiles/         (corrupted bog substrate, bone path)
│   ├── walls/               (overgrown bog wall, root-tangled stone)
│   ├── gates/               (corrupted iron gate, bone-fragment frame)
│   ├── patches/             (corrupted moss, dried blood, dark roots)
│   ├── scatter/             (bone fragments, skull pieces, rust embers)
│   └── rift_accents/        (heavier rift contamination Act 1 + dried blood mix)
├── Act3_CoreApproach/       ← F3 specific
│   ├── floor_tiles/         (void substrate, cosmic dust ground)
│   ├── walls/               (void-edged stone, fading reality)
│   ├── gates/               (sigil-engraved gate, gold inlay)
│   ├── patches/             (incandescent sigils, star fragments, void bleed)
│   ├── scatter/             (cosmic dust, sigil shards)
│   └── rift_accents/        (full rift overflow, reality tear)
└── Special/                 ← Bosses, hub rooms, unique scenes
    ├── Nexus_Core/          (final boss arena)
    ├── Spirit_Rooms/        (curse gates, blessing rooms)
    └── Spawn_Hub/           (player home base before runs)
```

## Cross-Act reuse rules (memo)

Bazı asset'ler birden fazla Act'te kullanılır — özellikle:

| Asset | Hangi Act'lerde | Why reused |
|---|---|---|
| `_Universal/floor_decals/dust_speck.png` | Act 1, 2, 3 (color-tintable) | Tinting ile her Act'e uyum |
| `_Universal/small_stones/generic_pebble.png` | Act 1, 2 | Cool gray neutral, Act 2'de subtle |
| `Act1/patches/rift_seepage.png` | Act 1, 2 (heavier) | Rift contamination Act 2'de büyür, Act 1 art base |
| `Act1/scatter/ritual_fragment.png` | Act 1, 2, 3 | Tüm Act'lerde ritüel kalıntı motifi |
| `Act1/walls/cool_granite_wall.png` | Act 1 main, Act 2 ruin variant | Act 2'de moss/bog overlay'le aynı duvar |
| `_Universal/vfx_atomic/spark.png` | Tümü | VFX neutral, class color tinting ile |

**Kural:** Yeni asset üretmeden ÖNCE check: `_Universal` veya başka Act'te benzer var mı? Varsa tinting/scale ile reuse, sıfırdan üretme.

## Migration plan (mevcut asset'leri yeniden organize et)

| Şu an | Yeni yer | Aksiyon |
|---|---|---|
| `Assets/Art/Rooms/AssetPack/FloorStones/*.png` | `Assets/Art/AssetPacks/_Universal/small_stones/` | Move (neutral gray, generic) |
| `Assets/Art/Rooms/AssetPack/LargePatches/*.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/` | Move (cool gray = Act 1) |
| `Assets/Art/Rooms/Backgrounds/Spawn_01/layer_11_wall_edge_stone.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/` | Move + rename |
| `Assets/Resources/Environment/StoneDungeon/Walls/RIMA_gate_*.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/` | Move + new palette gen |
| `Assets/Art/Tiles/F1/FlatTileset_GraniteV2/*.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/` | Move (F1 = Act 1) |
| `Assets/Art/Tiles/F1/SeamlessV1/granite_pure_noise_01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/` | Move |
| `Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/*` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/` (decals/walls/props subfolders) | Move + audit which are Act 1 specific vs Universal |

## Asset count budget per Act (target)

| Category | Per Act | Universal |
|---|---|---|
| Floor base tiles | 3-4 materials × 4-8 variants = ~24 | — |
| Wall sprites | 4-6 segments + 2 corner variants | — |
| Gate sprites | 2-3 styles × 2 states (open/closed) | — |
| L4 patches | 5-8 organic blobs | — |
| L5 scatter | 6-10 small props | 4-6 universal pebbles |
| L6 accents | 3-5 hero objects | — |
| **Total Act** | **~50-60 sprites** | **~15-20 universal** |

3 Act × 55 + 18 universal = **~183 sprite** target full F1-F3.

## Şu an mevcut + ne eksik (Act 1 inventory)

| Need | Have | Missing |
|---|---|---|
| Cool granite floor | ✅ `granite_pure_noise_01.png` (plain) | Painterly variant for visual interest |
| Worn stone path | ⚠️ `tile_01-15` (random mix, var ama clean değil) | Truly seamless variant |
| Cool granite wall cap | ❌ (current = pebble stone, mismatch) | **PROMPT 3** PixelLab Create Image Pro |
| Granite-matching gate | ❌ (current = dark stone, mismatch) | **PROMPT 4** PixelLab Create Image Pro |
| Cool cave moss patch | ✅ `layer_101_patch_moss_organic.png` | OK |
| Dust drift patch | ✅ `layer_102_patch_dust_drift.png` | OK |
| Cracked rubble | ✅ `layer_103_patch_cracked_rubble_area.png` | OK |
| Small stones (Universal) | ✅ `FloorStones/*` (5 variants) | Move to `_Universal/` |
| Rift seepage/contamination | ❌ | **NEW gen** Act 1 patches (cyan + violet) |
| Blood drop (semantic detail) | ⏳ PixelLab `cbe06cc3` processing | Wait for arrival |

## Decision queue (user approval needed)

1. **Klasör hiyerarşi onayla** — `Assets/Art/AssetPacks/{Act1, Act2, Act3, _Universal, Special}` yapısı OK?
2. **Migration aksiyonu** — şu anki dağınık asset'leri yeniden organize et (~20 dk Codex iş)
3. **Eksik Act 1 asset gen sırası** — wall cap → gate → painterly granite variant → rift seepage
4. **Universal asset definition** — bu listede ekstra eklemen gerek var mı?

## Memory + decision documentation

Onay sonrası:
- Memory: `project_asset_pack_organization_lock.md` (drift-proof)
- Map Designer Asset Pack Browser hard-coded paths varsa update gerek
- CURRENT_STATUS S94 başlık altında log
