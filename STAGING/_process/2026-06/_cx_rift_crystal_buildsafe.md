ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only listed files (4) BLOCKED if unclear.

NLM ACCESS: Gerekmez (lokal kod fix). Gerekirse: `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"`. Direct-read: CURRENT_STATUS.md / kod / STAGING.

# Amaç
`rift_crystal` prefab'ı DirectorMode prop paletine SADECE `#if UNITY_EDITOR` + `AssetDatabase.LoadAssetAtPath` ile ekleniyor (`Assets/Scripts/UI/DirectorMode.cs:1145-1149`). Standalone/DEMO build'de bu blok derlenmez → DirectorMode self-bootstrap olduğu için `directorPlaceableProps` boş → **prop paleti BOŞ**. Demo build alınırsa "editörsüz içerik yerleştiriyorum" ayağı çöker. Fix = prefab'ı `Resources/`'a taşı + `Resources.Load` ile her build'de yükle (satır ~771'deki `DirectorWaveResourcePath` → `Resources.Load<EncounterWaveSO>` pattern'i AYNALA).

## Surgical kapsam (SADECE bunlar)
1. **Prefab taşı** (UnityMCP `manage_asset` move veya dosya+.meta birlikte):
   - `Assets/Prefabs/Props/ShatteredKeep_PixelLab/rift_crystal.prefab` → `Assets/Resources/DirectorProps/rift_crystal.prefab`
   - `.meta` GUID korunmalı (move, kopyala-sil değil). Sprite/material GUID referansları değişmez.
   - `Assets/Resources/DirectorProps/` yoksa oluştur.
2. **`Assets/Scripts/UI/DirectorMode.cs`:**
   - Satır ~111 `DefaultRiftCrystalPrefabPath` const'ının yanına ekle: `private const string RiftCrystalResourcePath = "DirectorProps/rift_crystal";`
   - Satır 1145-1149 bloğunu değiştir. YENİ mantık (her build'de çalışır):
     ```csharp
     GameObject riftCrystal = Resources.Load<GameObject>(RiftCrystalResourcePath);
 #if UNITY_EDITOR
     if (riftCrystal == null)
         riftCrystal = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(DefaultRiftCrystalPrefabPath);
 #endif
     if (riftCrystal != null && !source.Contains(riftCrystal))
         source.Add(riftCrystal);
     ```
   - `DefaultRiftCrystalPrefabPath`'i editor-fallback için BIRAK (sil deme), ama yeni yolu yansıt: değeri `"Assets/Resources/DirectorProps/rift_crystal.prefab"` olarak güncelle.

## YASAK (dokunma)
- DirectorMode'un başka hiçbir metodu, başka prop yolu, başka prefab. Sadece yukarıdaki 2 madde.
- Yeni abstraction/helper YOK. Resources.Load tek satır yeter.

## GATE (doğrulama — rapor et)
1. **Derleme temiz** — `read_console` types=error → 0 error.
2. **Test:** `run_tests` EditMode → `RIMA.Tests.DirectorModeValidationTests` → `PropValidationPlacesAndErasesSelectedProp` HÂLÂ GEÇMELİ (Resources.Load EditMode'da da prefab'ı bulur). Eğer 0 test koşarsa assembly_names=`RIMA_EditMode_Tests` ile tüm assembly koş, fail listesinde bu test YOKSA geçti say.
3. Prefab yeni yolda mı doğrula: `Assets/Resources/DirectorProps/rift_crystal.prefab` var, eski yol yok.

## RAPOR
`CODEX_DONE_<profil>.md`'ye: değişen dosyalar + diff özeti + derleme sonucu + test sonucu + prefab yeni yol teyidi. Belirsizlik/fail → **BLOCKED** yaz, tahmin etme.
