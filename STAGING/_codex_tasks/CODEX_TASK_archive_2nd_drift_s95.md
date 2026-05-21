# CODEX TASK — Archive 2nd-Drift Assets (S95)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

2nd-pass visual audit (`STAGING/ASSET_2ND_AUDIT_S95.md`) sonucu 6 asset DRIFT/AMBIGUOUS olarak işaretlendi. User onayladı archive et.

Hedef: `Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/`

## Taşınacak (6 PNG + 6 .meta)

```
# Pilot A wall drift
Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_0_face_NS.png (+ .meta)

# Patch drift (3D oyuk + 3D pile)
Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/act1_patch_cave_moss_v01.png (+ .meta)
Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/act1_patch_cracked_rubble_v01.png (+ .meta)

# Prop drift (flat top-down / front-view)
Assets/Art/AssetPacks/Act1_ShatteredKeep/props/act1_prop_iron_grate_floor_v01.png (+ .meta)
Assets/Art/AssetPacks/Act1_ShatteredKeep/props/act1_prop_pressure_plate_v01.png (+ .meta)
Assets/Art/AssetPacks/Act1_ShatteredKeep/props/act1_prop_wooden_ladder_v01.png (+ .meta)
```

## Hedef Struct

```
Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/
  walls/
    pilot_a_frame_0_face_NS.png (+ .meta)
  patches/
    act1_patch_cave_moss_v01.png (+ .meta)
    act1_patch_cracked_rubble_v01.png (+ .meta)
  props/
    act1_prop_iron_grate_floor_v01.png (+ .meta)
    act1_prop_pressure_plate_v01.png (+ .meta)
    act1_prop_wooden_ladder_v01.png (+ .meta)
```

## Komutlar

```bash
mkdir -p "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/walls"
mkdir -p "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/patches"
mkdir -p "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/props"

# Pilot A wall (note: PNG was untracked in 1st archive, .meta tracked — same here)
git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_0_face_NS.png.meta" \
       "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/walls/" 2>&1
mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_0_face_NS.png" \
   "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/walls/" 2>&1

# Patches (use git mv; fallback to mv if untracked)
for f in act1_patch_cave_moss_v01.png act1_patch_cracked_rubble_v01.png; do
  git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/$f" \
         "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/patches/" 2>&1 || \
    mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/$f" \
       "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/patches/" 2>&1
  git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/$f.meta" \
         "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/patches/" 2>&1 || \
    mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/$f.meta" \
       "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/patches/" 2>&1
done

# Props
for f in act1_prop_iron_grate_floor_v01.png act1_prop_pressure_plate_v01.png act1_prop_wooden_ladder_v01.png; do
  git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/props/$f" \
         "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/props/" 2>&1 || \
    mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/props/$f" \
       "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/props/" 2>&1
  git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/props/$f.meta" \
         "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/props/" 2>&1 || \
    mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/props/$f.meta" \
       "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/props/" 2>&1
done
```

## Doğrulama

```bash
# Hedef dolu
ls -R "Assets/Art/AssetPacks/_archive/act1_2nd_drift_s95/"

# Kaynak temiz
ls "Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/"     # sadece frame_1, _2, _3 olmalı
ls "Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/"                # sadece dust_drift_v01 olmalı
ls "Assets/Art/AssetPacks/Act1_ShatteredKeep/props/" | wc -l          # 10 PNG (13-3) + meta'lar
```

## Rapor

`STAGING/CODEX_DONE_archive_2nd_drift_s95.md`:
- Taşınan dosya sayısı (6 PNG + 6 meta = 12)
- Tracked vs untracked durumu
- Hedef tree çıktısı
- Hata varsa flag

## Constraint

- Sadece bu 6 PNG + meta'ları taşı. Başka dosyaya dokunma.
- GUID korunur (.meta birlikte).
- Commit YOK.

## Effort

low — 6 dosya pair, ~5 dakika.
