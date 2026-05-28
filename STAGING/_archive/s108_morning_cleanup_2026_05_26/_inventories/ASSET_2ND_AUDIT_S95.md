# 2nd-Pass Visual Asset Audit — S95 LATE NIGHT 2

> Source: rima-sonnet dispatch, görsel Read tool ile asset incelemesi.
> User direktifi: "Anlamsızları arşive taşıyalım, kalanlarıyla yapalım, sonra gerçek eksikler için üretim analizi."

## Özet

- **FIT:** 38 asset
- **OVERLAY_OK:** 22 asset (projection-agnostic — banner/decal/silhouette/decoration)
- **DRIFT_ARCHIVE:** 4 asset (somut görsel sebep var)
- **AMBIGUOUS:** 2 asset (user default archive)

**Toplam archive:** 6 asset → `Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/`

---

## DRIFT_ARCHIVE (4)

| Path | Tür | Görsel Sebep |
|---|---|---|
| `walls/pilot_a_test/pilot_a_frame_0_face_NS.png` | Wall | Tamamen frontal/top-down texture; flat düz yüzey, hiç iso perspektif yok. Diğer 3 frame ile projection-inconsistent. Pilot A Batch 1.1b'de yeniden gen (kuyrukta zaten). |
| `patches/act1_patch_cave_moss_v01.png` | Floor patch | Üstten bakılan yuvarlak çukur — merkezde derinlik varmış gibi render. "Zemin içi 3D oyuk" efekti, flat overlay değil. |
| `patches/act1_patch_cracked_rubble_v01.png` | Floor patch | Tam iso projeksiyonda taş blok grubu — bağımsız 3D obje. Zemin üzerinde "öbek" gibi yükseliyor, prop ile çakışır. |
| `props/act1_prop_iron_grate_floor_v01.png` | Floor prop | Tam frontal/top-down 2D grid pattern, siyah dikdörtgen, hiç iso açısı yok. Iso sahneye yerleştirilince projection çelişir. |

## AMBIGUOUS → user default archive (2)

| Path | Sorun |
|---|---|
| `props/act1_prop_pressure_plate_v01.png` | Tamamen frontal top-down kare, iso açısı yok. Floor mechanic için iso versiyonu re-gen gerekir. |
| `props/act1_prop_wooden_ladder_v01.png` | Tam ön-cephe görünüm (front-view ladder). Wall-decoration olarak kabul edilebilir ama floor prop olarak projection uyumsuz. |

## KEEP_FIT — Devam Edilecek (60 asset)

### Floor (3) — FIT
3 iso granite tile (clean/worn/chiseled). Relief detail var ama tileable kabul edilebilir; mild baked shading minor.

### Walls — Pilot A (3 FIT, 1 DRIFT taşındı)
face_EW + corner_outer + arch_opening — 35° iso, stil tutarlı.

### Arches (2) — FIT
entry + exit cyan_rift_v01 — Iso portal, cyan fill, strong silhouette.

### Patches (1 KEEP, 2 archived)
Sadece `dust_drift_v01` — gerçek 2D alpha blob, projection-agnostic.

### Pillars (3) — FIT
broken_granite, chained, intact_cyan_crack — iso, consistent, thematic.

### Ritual (5) — FIT
obelisk, stone_altar, stone_bench, stone_marker_cyan, tomb_headstone — clean iso silhouettes.

### Statues (3) — FIT
pedestal_base, warrior_intact, warrior_toppled — strong silhouettes, dungeon narrative.

### Scatter (2) — FIT
bone_offering_pile, skull_pile_cluster — iso ground scatter.

### Rift Accent (1) — OVERLAY_OK
fracture_overlay — alpha overlay, minor purple/cyan tema drift kabul edilebilir.

### Props (10 KEEP, 1 DRIFT + 2 AMBIG archived)
brazier_cyan/orange, lever_wall, pottery_urn, rubble_debris_small, rubble_heap_skulls, spike_trap_dormant, treasure_pile, wooden_barrel, wooden_crate_stack — hepsi iso, gameplay-ready.

### Wall Decoration (15) — OVERLAY_OK
Banners (3 renk), torch_sconce, cage, chain (long+short), grate_iron, ivy, lantern, skeleton_shackled, trophy (bone+skull+sword), candle_bracket — projection-agnostic wall-attach.

### Decor Silhouettes (8) — OVERLAY_OK
rat_king, goblin, specter_ghost, imp_demon, cyan_slime, cyan_wisp, wraith_specter, husk — flat front-view, far-corner mood dekorasyon.

### Decals (16) — OVERLAY_OK
crack ×4, pebble ×4, dust ×4, bone_chip ×4 — alpha overlays. Not: dust_var0_08 sample nearly blank, subtle.

### Statue + Mounting Prefab (14 + 15) — KEEP variants
Visual review skipped (kompleks), variants olarak korunur.

---

## Yeni Üretim Analizi — Gerçek Eksikler (PixelLab Batch Priority)

Mevcut KEEP_FIT sonrası pipeline açıkları:

| # | Eksik | Tür | Tahmini gen | Öncelik |
|---|---|---|---|---|
| 1 | iso face_NS (Pilot A 1.1b) | Wall piece | 5 gen | 🔴 Kritik (showcase room için zorunlu) |
| 2 | Wang Core gap (corner_inner, T_junction, end_cap) | Wall pieces | ~20 gen (3 piece × varyant) | 🔴 Kritik (full perimeter wall için) |
| 3 | Flat iso granite tile (relief-free) | Floor base | ~10 gen | 🟡 Yüksek (Y axis seam fix) |
| 4 | Floor patch overlay (pure 2D alpha) | Overlay | ~15 gen (3 type × 5 variant: moss_stain, blood_stain, rift_seep) | 🟡 Yüksek (patches şu an sadece dust) |
| 5 | iso pressure_plate | Floor mechanic | ~5 gen | 🟢 Orta (gameplay) |
| 6 | iso iron_grate_floor | Floor mechanic | ~5 gen | 🟢 Orta |
| 7 | Floor tile state'ler (mossy_floor, cracked_floor, rift_floor) | Floor variant | ~15 gen | 🟢 Düşük (variation katmanı) |

**Toplam tahmini:** ~75 gen. Mevcut bütçe 2,413/5,000 → reserve 2,587. Rahatlıkla sığar.

**Önerilen sıralı dispatch:**
- Batch A: face_NS + Wang Core (25 gen) → showcase room kompozisyonu için ÖNCE bu
- Batch B: Flat granite tile + 3 floor patch type (25 gen) → seam fix
- Batch C: Mechanic obje iso versiyonları (10 gen) → gameplay completeness
- Batch D: Floor tile state (15 gen) → opsiyonel future variation

---

## Saçmalık Pass

- ✅ Her DRIFT verdict somut görsel sebep var (frontal/sunken bowl/3D pile/top-down grid)
- ✅ AMBIGUOUS minimal (2 adet, ikisi de archive yönünde — "şüphede archive" prensibi)
- ✅ Yeni gen önerileri bütçe ile uyumlu (~75 gen / 2587 reserve)
- ✅ Üretim öncelik kritik gameplay > visual fix > optional polish sırası

## Sonraki Adım

1. Archive 6 dosya → `_archive/act1_2nd_drift_s95/` (Codex batch dispatch)
2. Büyük oda v2 spec'i KEEP_FIT listeyle adapt (patches §10 sade leş, iron_grate kaldır, pressure_plate kaldır, face_NS Codex inline build YOK)
3. Codex Unity scene build (24×18 cell, v2 spec)
4. Screenshot review
5. Gerçek üretim batch A dispatch (face_NS + Wang Core) → 25 gen
