# Council Review — cx Faz 2 demo-tools diff (writer≠reviewer)

Sen kıdemli bir Unity C# code reviewer'ısın. cx (Codex) RIMA Director Mode'a prop/ışık placement + validation hook'ları + güvenlik ekledi. Bu DIFF'i denetle.

## OKU (zorunlu)
- Diff: `STAGING/_process/2026-06/_review_cx_faz2_diff.txt` (DirectorMode.cs +483 satır, +yeni test)
- Tam dosya bağlamı gerekirse: `Assets/Scripts/UI/DirectorMode.cs`, `Assets/Tests/EditMode/DirectorModeValidationTests.cs`
- Yeni prefab: `Assets/Prefabs/Props/ShatteredKeep_PixelLab/rift_crystal.prefab`

## Ne yapıldı (cx raporu)
Build sekmesi stub'ı → curated prop palette placement (Spawn UX aynalı: palette→ghost→sol-tık koy→sağ-tık sil). Transient `directorPlacedProps`. Spawn cap=10. `SetStatForValidation(statKey,value)` clamp'li slider yolundan. Class buton = 5 implement sınıf (Warblade/Elementalist/Shadowblade/Ranger/Ronin). Lazy validation init. 6 yeni hook: SelectFirstProp/PlaceSelectedPropAt/DirectorPlacedPropCount/ErasePlacedPropAt/HasPropGhost/SetStat ForValidation.

## SORULAR (numaralı cevap ver)
1. **Doğruluk:** Prop placement Spawn pattern'ini doğru mu aynalıyor? Ghost/place/erase/cap mantığı sağlam mı? Null-guard, list temizliği (transient prop'lar play-exit'te leak eder mi)?
2. **`SetStatForValidation` + clamp:** stat yazma yolu mevcut `OnStatSliderChanged`/Apply'ı doğru çağırıyor mu, yoksa paralel/kopuk yol mu uydurmuş? Clamp min/max nereden geliyor, güvenli mi?
3. **Spawn cap=10:** doğru yerde mi enforce ediliyor (hem director-spawn hem leak sayımı)? Off-by-one / prune doğru mu?
4. **Class limit:** 5 sınıf güvenli mi — desteklenmeyen sınıf controller'sız çökme riski kapandı mı?
5. **Surgical ihlali:** Diff sadece DirectorMode + test mi? `#if DEMO_BUILD||DEVELOPMENT_BUILD||UNITY_EDITOR` guard korundu mu? Combat/encounter/roguelite'a sızıntı var mı?
6. **Sunum riski:** Canlı demoda crash/NRE riski olan satır? (timeScale=0 etkileşim, EventSystem, ghost cleanup)
7. **Test failure yargısı:** cx full EditMode'da 619/13-fail raporladı, "değişiklik dışı pre-existing" dedi (eksik STAGING asset, MCP scene load imza drift, performance scan, PrefabHealth field drift, SubRoomSequenceController editor). Bu mantıklı mı — bu DirectorMode değişikliği o 13'ü tetikler mi, yoksa gerçekten ilgisiz mi?

PASS/FAIL ver + bulduğun her sorun için dosya:satır + öncelik (BLOCKER/MAJOR/MINOR). Türkçe, net.
