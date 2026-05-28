# Karar #126-130 MASTER_KARAR LOCK + FAZ_MASTER sync — execute every step, commit at end

## Context

rima-design (Opus) önerdi, user Karar #126-130'u onayladı (auto-lock: user uyudu, Opus verdict izlendi). Bu 5 karar Organic Room Dressing Pipeline'ı oluşturuyor. MASTER_KARAR_BELGESI.md ve FAZ_MASTER.md'ye eklenmesi gerekiyor.

## Karar Detayları (STAGING/karar_126_to_130_organic_pipeline.md'den al)

### Karar #126 — Organic Room Dressing Pipeline (9-stage umbrella)
Faz: 1.5 | Priority: P1
9-stage pipeline: Tilemap Base → WangResolver → Decal overlay → Scatter brush → Stamp/Cluster → Path readability → Biome preset SO → Naturalness Validator → PixelLab Pro re-gen pass

### Karar #127 — Stamp/Cluster Library
Faz: 1.5 | Priority: P1
Handcrafted 3-7 tile clusters (rubble pile, cracked wall corner, moss patch). ScriptableObject library. Room Designer one-click stamp. ChatGPT en kritik öneri.

### Karar #128 — Tile Metadata SO + WangResolver
Faz: 1 | Priority: P0
TileAssetMetadata ScriptableObject (biome, terrain type, weight, decal allowed). 16-tile NSEW WangTileResolver. TileImportWizard #118a integration. Karar #115 deterministic seed compliance.

### Karar #129 — Biome Preset SO
Faz: 1 | Priority: P0
RimaBiomePreset ScriptableObject: allowed terrain list, decal sets, scatter sets, palette tags. F1 Shattered Keep preset as MVP. RimaBiomeType enum upgrade.

### Karar #130 — Naturalness Validator + Path Readability
Faz: 1.5 | Priority: P1
Editor tool: scan room, flag unnatural patterns (uniform rows, zero decals, no scatter). Path readability check: walkable path min 2-tile wide. Automated before-PixelLab check.

## STEP 1 — Read spec file

Read `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/karar_126_to_130_organic_pipeline.md` for full spec details.
Read `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/MASTER_KARAR_BELGESI.md` — find the table format (son karar #125'in altına ekleyeceksin).

## STEP 2 — MASTER_KARAR_BELGESI.md'ye 5 karar ekle

Son mevcut karar (#125) altına 5 yeni satır ekle (table format korunacak):

| # | Karar | Faz | Durum | Notlar |
|---|-------|-----|-------|--------|
| 126 | Organic Room Dressing Pipeline (9-stage umbrella) | 1.5 | LOCKED | #127-130 alt karar |
| 127 | Stamp/Cluster Library (handcrafted hissi) | 1.5 | LOCKED | Room Designer one-click stamp, SO |
| 128 | Tile Metadata SO + WangResolver (16-tile NSEW) | 1 | LOCKED | P0, #118a integration, Karar #115 seed |
| 129 | Biome Preset SO (F1 Shattered Keep MVP) | 1 | LOCKED | P0, RimaBiomeType enum upgrade |
| 130 | Naturalness Validator + Path Readability editor | 1.5 | LOCKED | P1, pre-PixelLab check |

## STEP 3 — FAZ_MASTER.md sync

Read `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/FAZ_MASTER.md`.
Add or update Faz 1 P0 section: #128, #129 olarak ekle.
Add or update Faz 1.5 P1 section: #126, #127, #130 olarak ekle.

## STEP 4 — Commit

```bash
git add TASARIM/MASTER_KARAR_BELGESI.md TASARIM/FAZ_MASTER.md
git commit -m "[karar] #126-130 organic pipeline + stamp library + wang resolver + biome preset + validator

- #126 Organic Room Dressing Pipeline (9-stage) LOCKED
- #127 Stamp/Cluster Library LOCKED (Faz 1.5 P1)
- #128 Tile Metadata SO + WangResolver LOCKED (Faz 1 P0)
- #129 BiomePreset SO Shattered Keep MVP LOCKED (Faz 1 P0)
- #130 Naturalness Validator LOCKED (Faz 1.5 P1)"
```

## STEP 5 — CODEX_DONE.md append

```
## [2026-05-14 S70 gece] Karar #126-130 MASTER_KARAR LOCK
- MASTER_KARAR_BELGESI.md: 5 karar eklendi (#126-130)
- FAZ_MASTER.md: Faz 1 P0 (#128/#129) + Faz 1.5 P1 (#126/#127/#130) sync
- Commit: [hash]
```

## Constraints

- Table format MASTER_KARAR'daki mevcut formatla birebir eşleş
- Mevcut satırları silme veya değiştirme
- Sadece 5 yeni satır ekle
- FAZ_MASTER row format: mevcut Faz 1 section'ına kondense ekle
