# Codex Task — Mob PNG → Decor Silhouette Reorganize (16 dosya)

> **Profile:** any active cx profile
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_mob_png_reorganize_s95.md`
> **Geri dönülebilir:** PNG'ler silinmez, sadece taşınır + rename. .meta dosyaları takip eder (GUID korunur).

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

16 statik `act1_mob_*.png` dosyasını **decor silhouette** olarak hibrit reorganize et:
- 8 generic → `_Universal/decor_silhouettes/`
- 8 Act1-spesifik → `Act1_ShatteredKeep/decor_silhouettes/`
- Hepsi prefix değişimi: `act1_mob_*` → `decor_silhouette_*`

### Dosya Listesi

**Universal (8 — generic, cross-Act reuse uygun):**
| # | Eski Path | Yeni Path |
|---|---|---|
| 1 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_dungeon_rat_v01.png` | `Assets/Art/AssetPacks/_Universal/decor_silhouettes/decor_silhouette_rat_v01.png` |
| 2 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_bat_v01.png` | `Assets/Art/AssetPacks/_Universal/decor_silhouettes/decor_silhouette_bat_v01.png` |
| 3 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_giant_spider_v01.png` | `Assets/Art/AssetPacks/_Universal/decor_silhouettes/decor_silhouette_spider_v01.png` |
| 4 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_bone_walker_v01.png` | `Assets/Art/AssetPacks/_Universal/decor_silhouettes/decor_silhouette_bone_walker_v01.png` |
| 5 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_ground_crawler_v01.png` | `Assets/Art/AssetPacks/_Universal/decor_silhouettes/decor_silhouette_ground_crawler_v01.png` |
| 6 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_animated_skull_v01.png` | `Assets/Art/AssetPacks/_Universal/decor_silhouettes/decor_silhouette_animated_skull_v01.png` |
| 7 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_bone_hand_v01.png` | `Assets/Art/AssetPacks/_Universal/decor_silhouettes/decor_silhouette_bone_hand_v01.png` |
| 8 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_bone_archer_v01.png` | `Assets/Art/AssetPacks/_Universal/decor_silhouettes/decor_silhouette_bone_archer_v01.png` |

**Act1 spesifik (8 — cyan tint veya Act1 lore):**
| # | Eski Path | Yeni Path |
|---|---|---|
| 9 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_cyan_slime_v01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/decor_silhouette_cyan_slime_v01.png` |
| 10 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_cyan_wisp_v01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/decor_silhouette_cyan_wisp_v01.png` |
| 11 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_imp_demon_v01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/decor_silhouette_imp_demon_v01.png` |
| 12 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_goblin_v01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/decor_silhouette_goblin_v01.png` |
| 13 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_wraith_specter_v01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/decor_silhouette_wraith_specter_v01.png` |
| 14 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_husk_v01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/decor_silhouette_husk_v01.png` |
| 15 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_specter_ghost_v01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/decor_silhouette_specter_ghost_v01.png` |
| 16 | `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/act1_mob_rat_king_v01.png` | `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/decor_silhouette_rat_king_v01.png` |

### Import Setting Standardize

Her PNG için decor_silhouette standartı:
```
spriteMode: Single (1)
spritePixelsPerUnit: 64
filterMode: Point (0)
alphaIsTransparency: true
wrapMode: Clamp
spritePivot: (0.5, 0.0)     ← bottom-center, foot pixel ground
spriteMeshType: FullRect (0)
maxTextureSize: 256          ← küçültme: 64×64 hedef render, ama PNG kaynak korunur, sadece runtime resize
textureCompression: Uncompressed
```

### Adımlar

1. **Klasör oluştur:**
   - `Assets/Art/AssetPacks/_Universal/decor_silhouettes/` (yoksa yarat)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/decor_silhouettes/` (yoksa yarat)
   - Her klasör için `.gitkeep` veya `_README.md` opsiyonel (boş bırakma sorunu varsa)
2. **Her PNG için:**
   - `AssetDatabase.MoveAsset(eski, yeni)` — GUID korunur, .meta dosyası takip eder
   - Move sonrası import setting'i hedefe uygun standardize et (PPU/Filter/Pivot/MaxSize)
3. **Eski klasör temizliği:**
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/` boşalmış olmalı
   - Boşsa klasörü sil (ve .meta'sını da)
   - Boş değilse (başka dosyalar varsa) klasörü bırak
4. **Verify:**
   - Her yeni path'te dosya var mı
   - Her dosyanın GUID'i değişmemiş mi (move başarılı)
   - Import setting'ler hedefe uygun mu
   - Git diff: 16 move + import setting değişikliği + (varsa) eski klasör silinmesi

### Hard Constraints

- **PNG silme** — sadece taşı. `AssetDatabase.MoveAsset` kullan (raw file system move değil — GUID korumak için).
- **GUID korunmalı.** Move sonrası her dosyanın GUID'i değişmedi.
- **Prefab/scene reference KIRILMA** — bu dosyalar şu an scene'de kullanılmıyor (RimaUnifiedPainter sadece `mob_/enemy_` prefix tarıyor), risk düşük. Yine de move sonrası `AssetDatabase.GetDependencies` ile referans çıktısı kontrol.
- **Auto-commit YOK.**

### Output Format

```markdown
# Mob PNG Reorganize — Codex Report

## Moved Files (16)
### Universal (8)
| # | Old → New | GUID | Import OK |
|---|---|---|---|
| 1 | act1_mob_dungeon_rat_v01 → _Universal/.../decor_silhouette_rat_v01 | abc123... | ✓ |
| ...

### Act1 (8)
| # | Old → New | GUID | Import OK |
|---|---|---|---|
...

## Old Folder Cleanup
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/mobs/`: removed (was empty after move) / kept (X other files)

## Verify
- 16 files moved, 16 GUIDs preserved
- No broken references (AssetDatabase.GetDependencies = clean)
- Git diff: 16 moves + 16 import setting changes + (maybe) 1 folder delete

## Açık Sorular (varsa)
- {soru}
```
