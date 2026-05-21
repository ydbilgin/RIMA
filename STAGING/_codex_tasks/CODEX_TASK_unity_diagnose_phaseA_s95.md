# Codex Task — Unity Console Diagnose + Scene/Asset Metadata (Faz A + Faz B context)

> **Profile:** any active cx profile (Unity açık olmalı)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_unity_diagnose_phaseA_s95.md`

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

Üç ayrı rapor + (varsa) fix. Hepsi tek output dosyasına.

### Bölüm 1 — Unity Console Diagnose

UnityMCP üzerinden Console oku (`read_console` veya muadili). Sahne: `Assets/Scenes/Demo/PathC_BaseTest.unity` açık olmalı.

Her **error** için:
- Mesaj (full text)
- Dosya:satır
- Stack trace ilk 3-5 satır
- Sınıflandırma: `compile_error` / `runtime_null` / `missing_reference` / `import_error` / `other`

Her **warning** için sadece: mesaj + dosya, kısa liste (top 10).

### Bölüm 2 — PathC_BaseTest Scene Verify

UnityMCP ile `Assets/Scenes/Demo/PathC_BaseTest.unity` sahnesinden oku:

- **Grid GameObject:**
  - `transform.position`, `transform.localScale`, `transform.rotation.eulerAngles`
  - `Grid.cellSize`, `Grid.cellLayout`, `Grid.cellSwizzle`
- **Tilemap child(ren):** Her Tilemap için:
  - Component: `Tilemap.tileAnchor`, `TilemapRenderer.mode`, `TilemapRenderer.sortingLayerName`, `TilemapRenderer.sortOrder`
  - `IsometricZAsY` (TransparencySortAxis ayarı): aktif mi, değer ne
- **Camera:**
  - `Camera.transform.rotation.eulerAngles` (varsa tilt)
  - `Camera.orthographicSize`
- **Props_Root:** var mı, transform identity mi
- **Walls / Mobs / WallMountings vb sub-grup:** scene root altında mı, Props_Root altında mı, hiç yok mu

### Bölüm 3 — Wall PNG Metadata

5 dosya için:
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_straight_horizontal_v01.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_corner_L_NE_v01.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_arch_opening_v01.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_partition_low_stub_v01.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_cyan_rift_integrated_v01.png`

Her PNG için raporla:
- **TextureImporter ayarları:**
  - `spriteImportMode`, `spritePivot`, `spritePixelsPerUnit`, `filterMode`
  - `meta.alphaIsTransparency`, `wrapMode`
- **Sprite dimensions:** width × height (PNG kendi)
- **Foot row analizi (kritik):**
  - PNG'nin en alt 4 satırı boş (alpha 0) mı, dolu mu?
  - Boşsa = padding var, pivot (0.5, 0.0) "floating" yapar → re-import gerekli
  - Doluysa = foot row temiz, pivot (0.5, 0.0) direkt kullanılabilir
- **Görsel sınıflandırma (filename + heuristic):**
  - `straight_horizontal` → L2b face_EW kandidat
  - `corner_L_NE` → L2b corner kandidat
  - `arch_opening` → L2b arch kandidat
  - `partition_low_stub` → **L2a base mi, L2b short mı?** (sprite height ratio'ya bak — yüksekliği canvas'ın %30'undan az ise L2a baz, fazla ise L2b)
  - `cyan_rift_integrated` → variant (intact damaged variant mı, ayrı archetype mi?)

### Bölüm 4 — Fix (sadece compile_error varsa)

Bölüm 1'de `compile_error` veya `missing_reference` (script type missing) varsa:
- Root cause + minimal fix (1-3 dosya max)
- Uygula
- `dotnet build` çalıştır → 0 error confirm
- (Test run YOK — Faz B'ye geçiş için sadece build temiz olsun)
- **Auto-commit YOK** — user manual commit eder. Sadece working tree değişikliği bırak.
- Git diff özeti (hangi dosya kaç satır)

Diğer error tipleri (runtime_null, import_error) için fix UYGULAMA — sadece rapor + öneri. User karar verir.

## Output Format

```markdown
# Unity Diagnose Phase A — Codex Report

## Bölüm 1: Console
### Errors
| # | Type | File:Line | Message | Stack (truncated) |
|---|---|---|---|---|
| 1 | compile_error | ... | ... | ... |

### Warnings (top 10)
- ...

## Bölüm 2: Scene Verify
### Grid
- transform.position: (0, 0, 0)
- transform.localScale: (1, 0.5, 1)  ← **squash gerçeği**
- Grid.cellLayout: Isometric
- Grid.cellSize: (1, 0.5, 1)
...

### Camera
- rotation: (0, 0, 0)
- orthographicSize: ...

### Props_Root
- exists: yes/no, identity: yes/no
- sub-groups: [list]

## Bölüm 3: Wall PNG Metadata
| File | W×H | Pivot | PPU | Foot row | Classification |
|---|---|---|---|---|---|
| straight_horizontal | 128×128 | (0.5, 0.5) | 64 | empty 6 rows | L2b face_EW, re-import needed |
| ... |

## Bölüm 4: Fix Applied (varsa)
- Error #1: ...
  - Root cause: ...
  - Fix: `Assets/.../X.cs` line 42 → ...
- dotnet build: 0 error
- Git diff: 2 files, +3 -1

## Açık Sorular / Drift Tespiti
- Transform squash 0.5 (sahne) vs 0.819 (memory) → SAHNE GEÇERLİ (current state authoritative)
- ...
```

## Bitiş

- Output: `STAGING/CODEX_DONE_unity_diagnose_phaseA_s95.md`
- Orchestrator next-step kararı: console temizse Faz B visual test, error varsa fix loop
