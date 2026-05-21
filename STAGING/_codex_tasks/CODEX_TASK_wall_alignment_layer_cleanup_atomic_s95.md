# Codex Task — Wall Alignment + Layer Cleanup ATOMIC (S95)

> **Profile:** any active cx profile (Unity açık)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_wall_alignment_layer_cleanup_atomic_s95.md`
> **Geri dönülebilir:** .meta + TagManager + script + scene değişiklikleri. Auto-commit YOK.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev — 6 Bölüm (Atomik)

### Bölüm 1 — Wall .meta Custom Alignment Fix

**Sorun:** Önceki re-import `spritePivot` set etti AMA `spriteAlignment` enum `9 = Custom` yapmadı → Unity custom pivot'u görmüyor.

**5 dosya:**
1. `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_straight_horizontal_v01.png`
2. `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_corner_L_NE_v01.png`
3. `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_arch_opening_v01.png`
4. `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_partition_low_stub_v01.png`
5. `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_cyan_rift_integrated_v01.png`

**Her dosya için:**
```csharp
var importer = AssetImporter.GetAtPath(path) as TextureImporter;
var settings = new TextureImporterSettings();
importer.ReadTextureSettings(settings);
settings.spriteAlignment = (int)SpriteAlignment.Custom;  // 9
settings.spritePivot = new Vector2(0.5f, customPivotY);  // 0.0313 (128h) / 0.0417 (96h)
importer.SetTextureSettings(settings);
importer.SaveAndReimport();
```

**Verify:** `AssetDatabase.LoadAssetAtPath<Sprite>()` ile yükle, `sprite.pivot` pixel cinsinden (W*0.5, ~4) olmalı.

### Bölüm 2 — PathC_BaseTest.unity Floor_Tilemap → Ground SortingLayer

- Mevcut: Floor_Tilemap `sortingLayerID: 0` (Default)
- Yeni: `Ground` (mevcut layer, uniqueID 2024493761)
- Scene SAVE ET (kalıcı setup)

### Bölüm 3 — Orphan Sorting Layer Cleanup

Wall transparency research (`STAGING/CODEX_DONE_wall_transparency_research_s95.md`) tespit etti:

**Silinecek (DELETE):**
- `Detail` (uniqueID 351335743) — 0 active usage
- `Accent` (uniqueID 1570199623) — 0 active usage
- `Props` (uniqueID 399489520) — 0 active usage
- `Wall` (singular, uniqueID 2024493762) — 0 active usage, drift kaynak

**KEEP (canonical):**
- Default, Ground, Walls, Entities, VFX

**ÖNCE script fix (Bölüm 4) yapılmalı**, sonra layer silme. Aksi halde script tekrar yaratır.

### Bölüm 4 — Script Drift Fix

#### 4a. `Assets/Editor/DevTools/IsometricSortSetup.cs`

**Sorun:** Script "Wall" (singular) sorting layer string'i kullanıyor → drift kaynağı.

**Fix:** Tüm `"Wall"` → `"Walls"` (canonical) replace. **Sadece sortingLayer name string'leri** — başka identifier'a dokunma. Code review et + minimal surgical edit.

#### 4b. `Assets/Editor/RimaSortingLayerValidator.cs`

**Sorun:** Script Patch/Scatter/Detail/Accent/Props sorting layer'ları yaratıyor (drift kaynak).

**Karar:**
- Detail/Accent/Props (orphan) yaratımı **kaldır** — bu layer'lar artık DELETE listesinde.
- Patch/Scatter yaratımı **şartlı bırak**: Phase1_ProceduralMap_Test.unity sahnesi archive edilecek (Bölüm 5), ondan sonra Patch/Scatter de gereksiz. Şimdilik kaldır.
- Script'in canonical set'i `Default, Ground, Walls, Entities, VFX` olsun.

### Bölüm 5 — Phase1_ProceduralMap_Test.unity Archive

User: "emin değilim yeni map çizecez zaten" → archive et, silme.

**Adım:**
- `AssetDatabase.MoveAsset("Assets/Scenes/Phase1_ProceduralMap_Test.unity", "Assets/_archive/Scenes/Phase1_ProceduralMap_Test.unity")`
- `Assets/_archive/Scenes/` yoksa yarat
- `.meta` dosyası takip eder (GUID korunur)
- Eski klasörü silme (başka sahne varsa)

**Doğrula:** Move sonrası `Build Settings → Scenes In Build` listesinde varsa orijinal path bozulur. Önce kontrol et, varsa Build Settings'ten temizle (`EditorBuildSettings.scenes` ile).

### Bölüm 6 — Final Verify

1. **Build:** `dotnet build` targeted (RIMA.Runtime, Assembly-CSharp, Assembly-CSharp-Editor) → 0 error
2. **Unity Console:** clean (no error/warning yeni)
3. **Sprite pivot verify:** 5 wall sprite pixel pivot = (W*0.5, ~4)
4. **Sorting layer set:**
   - Var: Default, Ground, Walls, Entities, VFX
   - Yok: Detail, Accent, Props, Wall (singular), Patch, Scatter (Phase1 archive sonrası)
5. **Scene verify:** PathC_BaseTest.unity Floor_Tilemap sortingLayerName = "Ground", scene saved
6. **Script string check:** IsometricSortSetup.cs grep'te "Wall" string yok (sadece "Walls"), RimaSortingLayerValidator.cs canonical 5 layer

### Sıralama (Kritik)

1. Bölüm 4 — Script drift fix (önce script, sonra layer)
2. Bölüm 5 — Phase1 sahne archive (Patch/Scatter usage temizlensin)
3. Bölüm 3 — Orphan layer cleanup (TagManager.asset edit)
4. Bölüm 1 — Wall .meta alignment fix
5. Bölüm 2 — Scene Floor_Tilemap → Ground + save
6. Bölüm 6 — Verify

## Output Format

```markdown
# Wall Alignment + Layer Cleanup Atomic — Codex Report

## Bölüm 1: Wall .meta Alignment Fix
| File | spriteAlignment | spritePivot | Verify (pixel) |
|---|---|---|---|
| straight_horizontal | 0→9 | (0.5, 0.03125) | (64, 4) PASS |
| ... |

## Bölüm 2: PathC_BaseTest Floor_Tilemap
- sortingLayerID: 0 (Default) → 2024493761 (Ground)
- Scene saved: YES

## Bölüm 3: Orphan Layers Deleted
- Detail (351335743): DELETED
- Accent (1570199623): DELETED
- Props (399489520): DELETED
- Wall singular (2024493762): DELETED
- Patch (1365605006): DELETED (Phase1 archive sonra)
- Scatter (27625511): DELETED (Phase1 archive sonra)

## Bölüm 4: Script Drift Fix
- IsometricSortSetup.cs: 2 "Wall" string → "Walls"
- RimaSortingLayerValidator.cs: canonical 5 layer, Patch/Scatter/Detail/Accent/Props yaratımı kaldırıldı

## Bölüm 5: Phase1 Sahne Archive
- Assets/Scenes/Phase1_ProceduralMap_Test.unity → Assets/_archive/Scenes/
- Build Settings'te yoktu / temizlendi (hangisi)

## Bölüm 6: Final Verify
- dotnet build: 0 error
- Sprite pivot: 5/5 PASS
- Sorting layers final: [Default, Ground, Walls, Entities, VFX]
- Floor_Tilemap on Ground: PASS
- Scripts canonical: PASS

## Git Diff Summary
- 5 wall .meta (alignment+pivot)
- ProjectSettings/TagManager.asset (-6 layers)
- Assets/Editor/DevTools/IsometricSortSetup.cs (Wall→Walls string)
- Assets/Editor/RimaSortingLayerValidator.cs (canonical set)
- Assets/Scenes/Phase1_ProceduralMap_Test.unity → _archive/Scenes/
- Assets/Scenes/Demo/PathC_BaseTest.unity (Floor_Tilemap sortingLayer)

## Açık Sorular
- {varsa}
```

## Hard Constraints

- **Sadece listelenen dosyalar.** Başka prefab/scene/script'e dokunma.
- **Atomik:** Bölüm 1-6 sırayla yapılmalı. Herhangi birinde BLOCKED → durdur, sonraki bölümleri yapma.
- **Auto-commit YOK** — user manual commit.
- **PathC_BaseTest.unity save ET** (Bölüm 2 istisna, kalıcı setup).
- **BLOCKED if unclear:** Phase1 sahnesinin başka referansları varsa (prefab içinde sahne reference, scriptable object linki) durdur, archive yapma.
