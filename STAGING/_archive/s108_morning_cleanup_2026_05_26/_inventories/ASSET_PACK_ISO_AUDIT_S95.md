# Wall + Floor Asset Pack Iso/Legacy Audit — S95 LATE NIGHT 2 (2026-05-20)

> Source: rima-sonnet dispatch, user direktif "isometric harici hepsini archive".
> Sonuç: 41 PNG + 32 companion .asset + RoomPipelineTest.unity → archive.

## Özet

- Total scanned: **59 PNG** (walls: 10 + wall_decoration: 15 + arches: 2 + floor_tiles: 31 + patches: 3 + decals: 16)
- **ISO_KEEP: 34** — RIMA canonical (iso + projection-agnostic overlay)
- **ARCHIVE: 41 PNG** (37 kesin legacy + 4 AMBIGUOUS user kararı = archive)
- Companion `.asset` (tile assets): **32** taşınacak

`wall_blocks/` ve `gates/` klasörleri boş.

---

## ISO_KEEP (bırak)

| Path | Tür | Neden |
|---|---|---|
| `walls/pilot_a_test/pilot_a_frame_0_face_NS.png` | Wall piece | S95 Pilot A LIVE — fake-iso 35° |
| `walls/pilot_a_test/pilot_a_frame_1_face_EW.png` | Wall piece | S95 Pilot A LIVE |
| `walls/pilot_a_test/pilot_a_frame_2_corner_outer.png` | Wall piece | S95 Pilot A LIVE |
| `walls/pilot_a_test/pilot_a_frame_3_arch_opening.png` | Wall piece | S95 Pilot A LIVE |
| `floor_tiles/iso/act1_iso_granite_clean.png` | Iso tile | LIVE iso diamond |
| `floor_tiles/iso/act1_iso_granite_worn.png` | Iso tile | LIVE iso diamond |
| `floor_tiles/iso/act1_iso_granite_chiseled.png` | Iso tile | LIVE iso diamond |
| `arches/act1_arch_entry_cyan_rift_v01.png` | Arch overlay | Projection-agnostic, Path C overlay |
| `arches/act1_arch_exit_cyan_rift_v01.png` | Arch overlay | Projection-agnostic, Path C overlay |
| `wall_decoration/*` (15 dosya) | Decor overlay | Projection-agnostic (banner/torch/chain/skull/...) |
| `patches/*` (3 dosya) | Floor patch overlay | Projection-agnostic scatter |
| `decals/*` (16 dosya) | Decal | Projection-agnostic (crack/pebble/dust/bone) |

---

## ARCHIVE — Hedef: `Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/`

### Walls (5 dosya)

| Path | GUID | Neden |
|---|---|---|
| `walls/painted_v01/walls_set_v01.png` | `f8d4b642928ec2c46bb3bd35505356f8` | Pre-iso Path A iteration |
| `walls/act1_wall_partition_low_stub_v01.png` | `2d6890022a252ab4283bef6111b8bba3` | Non-iso, ref yok |
| `walls/act1_wall_straight_horizontal_v01.png` | `e156651e1b7a08446921534b8933fb11` | Flat top-view |
| `walls/act1_wall_corner_L_NE_v01.png` | `e0472abeef4bcb4419a383e1fea6a4fb` | Belirsiz proj, ref yok |
| `walls/act1_wall_cyan_rift_integrated_v01.png` | `b8a3998a4d26c6f4fa23dcc756020d8a` | "integrated" eski pipeline — user archive |

### Floor (36 dosya)

| Grup | Dosya sayısı | Neden |
|---|---|---|
| `floor_tiles/granite_low_topdown_v02/` (tüm 16 PNG) | 16 | İsimde "low_topdown" |
| `floor_tiles/painted_v03/` (4 PNG) | 4 | Pre-iso Path A iteration |
| `floor_tiles/granite_base/` (16 PNG) | 16 | "4mat" flat 64px square, iso değil, hiç ref yok |

### Companion Tile `.asset` (32 dosya)

| Klasör | İçerik |
|---|---|
| `Assets/Data/Tiles/Act1_granite_low_v02/` | 16 tile asset (low_topdown PNG'lerden ref) |
| `Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/` | 16 tile asset (painted_v03 PNG'lerden ref) |

### Scene (1 dosya)

| Path | Aksiyon |
|---|---|
| `Assets/Scenes/Demo/RoomPipelineTest.unity` | `Assets/_ARCHIVE/Scenes/` altına taşı (sadece legacy asset'leri ref ediyor, açıldığında missing ref olur) |

---

## Toplam Aksiyonlar

| Tür | Adet |
|---|---|
| PNG taşınacak | 41 |
| .meta taşınacak (PNG + .asset companion) | 73 |
| `.asset` taşınacak | 32 |
| Scene taşınacak | 1 + meta |
| Boş klasör silinecek | `granite_base/`, `granite_low_topdown_v02/`, `painted_v03/`, `painted_v01/`, `Act1_granite_low_v02/`, `Act1_ShatteredKeep/painted_v03/` |

---

## Kritik Uyarılar

1. **Git history korunsun:** `git mv` veya filesystem move + Unity restart. Asla copy+delete (GUID bozulur).
2. **`.meta` her PNG/`.asset` ile birlikte taşınır** — GUID preservation (Asset Pack Organization LOCK memory).
3. **`PathC_BaseTest.unity` (live scene) etkilenmemeli** — legacy asset ref yok zaten.
4. Codex bittikten sonra Unity'de **Project Refresh** + Console error check.
